# Copyright (c) Microsoft. All rights reserved.
# Licensed under the MIT license. See LICENSE file in the project root for full license information.

<#
.SYNOPSIS
    DSC Configuration for the Driver Computer (Client01) in Domain scenario.
    Handles hosts file, firewall, PowerShell remoting, and password-never-expires
    declaratively.

.DESCRIPTION
    The Driver machine in a domain scenario is similar to the workgroup scenario
    but includes DC01 entries in the hosts file.

    Declarative (this file):
      - Hosts file entries (DC01, Node01, Client01)
      - Firewall disabled for all profiles
      - PowerShell Remoting enabled
      - Password never expires for the local admin account

    Imperative (Invoke-DriverImperativeSteps.ps1):
      - Domain join via domainjoin.ps1 (requires reboot)
      - Tool installation (DotNetCore, OpenSSH, PowerShellCore, PTMService, etc.)
      - RSA key copy (domain-aware user folder)
      - sshd service restart
      - ForceLevel2 configuration via ShareUtil.exe

    DSC is idempotent -- re-running is safe and only changes what has drifted.

.EXAMPLE
    . .\Driver-Configuration.ps1
    DriverConfiguration -OutputPath .\MOF
    Start-DscConfiguration -Path .\MOF -Wait -Verbose -Force
#>

Configuration DriverConfiguration {

    param (
        [Parameter(Mandatory = $false)]
        [string]$ConfigFilePath
    )

    Import-DscResource -ModuleName PSDesiredStateConfiguration

    # -- Read Config.json at compile time to avoid hardcoded values -----
    $hostsEntries = @()
    $hostsMarker = '# --- Managed by DSC HostsFileEntries ---'
    if ($ConfigFilePath -and (Test-Path $ConfigFilePath)) {
        $configData = Get-Content -Path $ConfigFilePath -Raw | ConvertFrom-Json
        $domainName = $null
        if ($null -ne $configData.Core -and $null -ne $configData.Core.DomainName) {
            $domainName = $configData.Core.DomainName
        }
        foreach ($prop in $configData.Machines.PSObject.Properties) {
            $machine = $prop.Value
            $name    = $machine.ComputerName
            if ($null -ne $name -and $null -ne $machine.IpConfig -and $machine.IpConfig.Count -gt 0) {
                $ip = $machine.IpConfig[0].Ip
                if (-not [string]::IsNullOrWhiteSpace($ip)) {
                    $hostsEntries += "$ip`t$name"
                    # Add FQDN entry for domain-joined machines
                    if ($null -ne $domainName -and
                        $null -ne $machine.Domain -and
                        $machine.Domain -ne 'Workgroup' -and
                        $machine.Domain -ne '') {
                        $hostsEntries += "$ip`t$name.$domainName"
                    }
                }
            }
        }
        # Also add Endpoints if present.
        # For Azure clusters, cluster virtual IPs (GeneralFS, etc.) are not routable
        # without an Azure Load Balancer. Map endpoint names to Node01's real IP instead
        # so clients can reach the CA shares directly via the owning node.
        $isAzureCluster = ($null -ne $configData.Core.RegressionType -and $configData.Core.RegressionType -match 'Azure') -and
                          ($null -ne $configData.Core.Scenario -and $configData.Core.Scenario -match 'Cluster')
        $node01Ip = $null
        if ($isAzureCluster -and $null -ne $configData.Machines.Node01) {
            $node01Ip = $configData.Machines.Node01.IpConfig[0].Ip
        }
        if ($null -ne $configData.Endpoints) {
            foreach ($ep in $configData.Endpoints.PSObject.Properties) {
                $endpoint = $ep.Value
                $epName   = $endpoint.Name
                if ($null -ne $epName -and $null -ne $endpoint.IpConfig -and $endpoint.IpConfig.Count -gt 0) {
                    # In Azure clusters, use Node01's IP instead of the unreachable virtual IP
                    $epIp = if ($isAzureCluster -and $null -ne $node01Ip) { $node01Ip } else { $endpoint.IpConfig[0].Ip }
                    if (-not [string]::IsNullOrWhiteSpace($epIp)) {
                        $hostsEntries += "$epIp`t$epName"
                        if ($null -ne $domainName -and $domainName -ne '') {
                            $hostsEntries += "$epIp`t$epName.$domainName"
                        }
                    }
                }
            }
        }
    }
    else {
        Write-Warning "ConfigFilePath not provided or not found. Hosts file entries will be skipped."
    }

    Node 'localhost' {

        #region -- Hosts File ----------------------------------------------
        # Entries are built from Config.json at MOF compile time.
        Script HostsFileEntries {
            GetScript  = {
                $hostsPath = "$env:SystemRoot\System32\drivers\etc\hosts"
                @{ Result = (Get-Content $hostsPath -Raw) }
            }
            TestScript = {
                $entries = $using:hostsEntries
                if ($entries.Count -eq 0) { return $true }
                $marker = $using:hostsMarker
                $hostsPath = "$env:SystemRoot\System32\drivers\etc\hosts"
                $content = Get-Content $hostsPath -Raw -ErrorAction SilentlyContinue
                if ([string]::IsNullOrEmpty($content)) { return $false }
                if ($content -notmatch [regex]::Escape("$marker START")) { return $false }
                foreach ($entry in $entries) {
                    if ($content -notmatch [regex]::Escape($entry)) { return $false }
                }
                return $true
            }
            SetScript  = {
                $entries = $using:hostsEntries
                $marker = $using:hostsMarker
                $hostsPath = "$env:SystemRoot\System32\drivers\etc\hosts"
                $content = Get-Content $hostsPath -Raw -ErrorAction SilentlyContinue
                if ($null -eq $content) { $content = '' }
                # Remove existing managed block
                $startPattern = [regex]::Escape("$marker START")
                $endPattern   = [regex]::Escape("$marker END")
                $content = $content -replace "(?s)$startPattern.*?$endPattern\r?\n?", ''
                $content = $content.TrimEnd()
                # Build new managed block
                $block = "`r`n$marker START`r`n"
                foreach ($entry in $entries) {
                    $block += "$entry`r`n"
                }
                $block += "$marker END`r`n"
                Set-Content -Path $hostsPath -Value ($content + $block) -Force -Encoding ASCII
            }
        }
        #endregion

        #region -- Multi-NIC Routing ------------------------------------------
        # Azure VMs with multiple NICs each get a default route (0.0.0.0/0).
        # When traffic arrives on NIC2, the response may route back through
        # NIC1's lower-metric default gateway -- Azure drops this (asymmetric
        # routing). Fix: set a high InterfaceMetric on secondary NICs so even
        # if Azure DHCP re-adds a default route on renewal, the primary NIC's
        # lower-metric route always wins. Also remove existing secondary
        # default routes for immediate effect.
        Script MultiNicRouting {
            DependsOn  = '[Script]HostsFileEntries'
            GetScript  = {
                $routes = Get-NetRoute -AddressFamily IPv4 -DestinationPrefix '0.0.0.0/0' -ErrorAction SilentlyContinue
                @{ Result = ($routes | Select-Object InterfaceIndex, NextHop, RouteMetric | Out-String) }
            }
            TestScript = {
                $nics = Get-NetIPAddress -AddressFamily IPv4 -ErrorAction SilentlyContinue |
                    Where-Object {
                        $_.IPAddress -notlike '127.*' -and
                        $_.IPAddress -notlike '169.254.*' -and
                        $_.PrefixOrigin -ne 'WellKnown'
                    }
                if ($nics.Count -le 1) { return $true }

                # Find primary NIC by lowest effective metric (InterfaceMetric + RouteMetric).
                # Azure DHCP sets RouteMetric=0 on both NICs, so sorting by RouteMetric alone
                # is non-deterministic. The Bicep-primary NIC always gets a lower InterfaceMetric.
                $primaryIfIdx = (Get-NetRoute -DestinationPrefix '0.0.0.0/0' -ErrorAction SilentlyContinue |
                    Sort-Object { $_.RouteMetric + (Get-NetIPInterface -InterfaceIndex $_.InterfaceIndex -AddressFamily IPv4 -ErrorAction SilentlyContinue).InterfaceMetric } |
                    Select-Object -First 1).InterfaceIndex
                if (-not $primaryIfIdx) { return $true }

                foreach ($nic in $nics) {
                    if ($nic.InterfaceIndex -eq $primaryIfIdx) { continue }
                    $iface = Get-NetIPInterface -InterfaceIndex $nic.InterfaceIndex -AddressFamily IPv4 -ErrorAction SilentlyContinue
                    if ($iface -and $iface.InterfaceMetric -lt 9000) { return $false }
                    $secDefault = Get-NetRoute -DestinationPrefix '0.0.0.0/0' `
                        -InterfaceIndex $nic.InterfaceIndex -ErrorAction SilentlyContinue
                    if ($secDefault) { return $false }
                }

                # Verify cross-subnet routes exist on secondary NICs
                $nicSubnets = foreach ($n in $nics) {
                    $p = $n.IPAddress.Split('.')
                    [PSCustomObject]@{
                        IfIndex = $n.InterfaceIndex
                        Subnet  = "$($p[0]).$($p[1]).$($p[2]).0/$($n.PrefixLength)"
                    }
                }
                foreach ($nic in $nics) {
                    if ($nic.InterfaceIndex -eq $primaryIfIdx) { continue }
                    $mySub = ($nicSubnets | Where-Object { $_.IfIndex -eq $nic.InterfaceIndex }).Subnet
                    foreach ($other in $nicSubnets) {
                        if ($other.IfIndex -eq $nic.InterfaceIndex) { continue }
                        if ($other.Subnet -eq $mySub) { continue }
                        $xRoute = Get-NetRoute -DestinationPrefix $other.Subnet `
                            -InterfaceIndex $nic.InterfaceIndex -ErrorAction SilentlyContinue
                        if (-not $xRoute) { return $false }
                    }
                }
                return $true
            }
            SetScript  = {
                $nics = Get-NetIPAddress -AddressFamily IPv4 -ErrorAction SilentlyContinue |
                    Where-Object {
                        $_.IPAddress -notlike '127.*' -and
                        $_.IPAddress -notlike '169.254.*' -and
                        $_.PrefixOrigin -ne 'WellKnown'
                    }

                $primaryIfIdx = (Get-NetRoute -DestinationPrefix '0.0.0.0/0' -ErrorAction SilentlyContinue |
                    Sort-Object { $_.RouteMetric + (Get-NetIPInterface -InterfaceIndex $_.InterfaceIndex -AddressFamily IPv4 -ErrorAction SilentlyContinue).InterfaceMetric } |
                    Select-Object -First 1).InterfaceIndex

                foreach ($nic in $nics) {
                    if ($nic.InterfaceIndex -eq $primaryIfIdx) { continue }
                    Set-NetIPInterface -InterfaceIndex $nic.InterfaceIndex -InterfaceMetric 9999 -ErrorAction SilentlyContinue
                    Get-NetRoute -DestinationPrefix '0.0.0.0/0' `
                        -InterfaceIndex $nic.InterfaceIndex -ErrorAction SilentlyContinue |
                        Remove-NetRoute -Confirm:$false -ErrorAction SilentlyContinue
                }

                # Add cross-subnet routes on secondary NICs so they can reach
                # other NICs' subnets.  Without these, test traffic bound to a
                # secondary NIC source IP (e.g., Driver NIC2 192.168.2.111) cannot
                # reach a different subnet (e.g., SUT NIC1 192.168.1.11) because
                # the default route was removed to prevent asymmetric routing.
                # Route via the Azure gateway (.1) on each secondary NIC's subnet.
                $nicSubnets = foreach ($n in $nics) {
                    $p = $n.IPAddress.Split('.')
                    [PSCustomObject]@{
                        IfIndex = $n.InterfaceIndex
                        Subnet  = "$($p[0]).$($p[1]).$($p[2]).0/$($n.PrefixLength)"
                        Gateway = "$($p[0]).$($p[1]).$($p[2]).1"
                    }
                }
                foreach ($sec in ($nics | Where-Object { $_.InterfaceIndex -ne $primaryIfIdx })) {
                    $myInfo = $nicSubnets | Where-Object { $_.IfIndex -eq $sec.InterfaceIndex } | Select-Object -First 1
                    foreach ($other in $nicSubnets) {
                        if ($other.IfIndex -eq $sec.InterfaceIndex) { continue }
                        if ($other.Subnet -eq $myInfo.Subnet) { continue }
                        $exists = Get-NetRoute -DestinationPrefix $other.Subnet `
                            -InterfaceIndex $sec.InterfaceIndex -ErrorAction SilentlyContinue
                        if (-not $exists) {
                            New-NetRoute -DestinationPrefix $other.Subnet `
                                -InterfaceIndex $sec.InterfaceIndex `
                                -NextHop $myInfo.Gateway `
                                -RouteMetric 10 -ErrorAction SilentlyContinue
                        }
                    }
                }
            }
        }
        #endregion

        #region -- Firewall ------------------------------------------------
        Script DisableFirewall {
            DependsOn  = '[Script]MultiNicRouting'
            GetScript  = {
                $profiles = Get-NetFirewallProfile
                @{ Result = ($profiles | Select-Object -Property Name, Enabled | Out-String) }
            }
            TestScript = {
                $enabled = Get-NetFirewallProfile | Where-Object { $_.Enabled -eq $true }
                return ($null -eq $enabled -or $enabled.Count -eq 0)
            }
            SetScript  = {
                Set-NetFirewallProfile -Profile Domain, Public, Private -Enabled False
            }
        }
        #endregion

        #region -- PowerShell Remoting -------------------------------------
        Script EnablePSRemoting {
            GetScript  = {
                $listener = Get-WSManInstance -ResourceURI winrm/config/listener -Enumerate -ErrorAction SilentlyContinue |
                    Where-Object { $_.Transport -eq 'HTTP' }
                @{ Result = if ($listener) { 'Enabled' } else { 'Disabled' } }
            }
            TestScript = {
                $listener = Get-WSManInstance -ResourceURI winrm/config/listener -Enumerate -ErrorAction SilentlyContinue |
                    Where-Object { $_.Transport -eq 'HTTP' }
                return ($null -ne $listener)
            }
            SetScript  = {
                Enable-PSRemoting -Force -SkipNetworkProfileCheck
            }
            DependsOn = '[Script]DisableFirewall'
        }
        #endregion

        #region -- Password Never Expires ----------------------------------
        Script PasswordNeverExpires {
            GetScript  = {
                $accts = Get-CimInstance -ClassName Win32_UserAccount -Filter "Domain='$env:COMPUTERNAME'"
                $expiring = $accts | Where-Object { $_.PasswordExpires -eq $true }
                @{ Result = "Expiring accounts: $($expiring.Count)" }
            }
            TestScript = {
                $accts = Get-CimInstance -ClassName Win32_UserAccount -Filter "Domain='$env:COMPUTERNAME'"
                $expiring = $accts | Where-Object { $_.PasswordExpires -eq $true }
                return ($null -eq $expiring -or $expiring.Count -eq 0)
            }
            SetScript  = {
                $accts = Get-CimInstance -ClassName Win32_UserAccount -Filter "Domain='$env:COMPUTERNAME'"
                foreach ($acct in $accts) {
                    Set-CimInstance -InputObject $acct -Property @{ PasswordExpires = $false }
                }
            }
            DependsOn = '[Script]EnablePSRemoting'
        }
        #endregion
    }
}

# ===========================================================================
# Compile helper
# ===========================================================================
function Invoke-DriverDsc {
    param(
        [switch]$Apply,
        [string]$OutputPath = "$PSScriptRoot\MOF",
        [string]$ConfigFilePath = "$PSScriptRoot\..\Config.json"
    )

    Write-Host "Compiling Domain Driver DSC configuration..." -ForegroundColor Cyan
    DriverConfiguration -ConfigFilePath $ConfigFilePath -OutputPath $OutputPath

    if ($Apply) {
        Write-Host "Applying Domain Driver DSC configuration..." -ForegroundColor Yellow
        Start-DscConfiguration -Path $OutputPath -Wait -Verbose -Force
        Write-Host "Domain Driver DSC configuration applied." -ForegroundColor Green
    }
    else {
        Write-Host "MOF compiled to $OutputPath. Run:" -ForegroundColor Green
        Write-Host "  Start-DscConfiguration -Path '$OutputPath' -Wait -Verbose -Force"
    }
}
