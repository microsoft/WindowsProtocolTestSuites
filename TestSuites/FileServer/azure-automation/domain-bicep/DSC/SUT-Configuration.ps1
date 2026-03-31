# Copyright (c) Microsoft. All rights reserved.
# Licensed under the MIT license. See LICENSE file in the project root for full license information.

<#
.SYNOPSIS
    DSC Configuration for the Domain SUT (Node01).
    Handles Windows features, SMB shares, firewall, registry, hosts file,
    and directories declaratively.

.DESCRIPTION
    Covers the same resources as the Workgroup SUT-Configuration plus
    domain-specific considerations:
    - Domain join happens imperatively (requires reboot)
    - WindowsFeatureSet includes file-server features (batched)
    - All SMB shares on C: drive
    - Registry keys for FSA, FSRM classification
    - SMB require-signing, computer password

    Imperative steps (Invoke-SutImperativeSteps.ps1) handle:
    - Domain join + reboot
    - Disk partitioning (ReFS K:, FAT32 J:)
    - Symbolic links, mount points, shadow copies
    - DFS namespaces (standalone + domain-based)
    - QUIC certificate mapping
    - ForceLevel2 oplock (actually done from Driver side)
    - Tool installation

.EXAMPLE
    . .\SUT-Configuration.ps1
    SutConfiguration -ConfigFilePath .\..\Config.json -OutputPath .\MOF
    Start-DscConfiguration -Path .\MOF -Wait -Verbose -Force
#>

Configuration SutConfiguration {

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
        # Also add Endpoints if present
        if ($null -ne $configData.Endpoints) {
            foreach ($ep in $configData.Endpoints.PSObject.Properties) {
                $endpoint = $ep.Value
                $epName   = $endpoint.Name
                if ($null -ne $epName -and $null -ne $endpoint.IpConfig -and $endpoint.IpConfig.Count -gt 0) {
                    $epIp = $endpoint.IpConfig[0].Ip
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

        #region -- Windows Features (batched) ---------------------------------
        WindowsFeatureSet FileServerFeatures {
            Name   = @(
                'File-Services',
                'FS-BranchCache',
                'FS-VSS-Agent',
                'BranchCache',
                'FS-DFS-Namespace',
                'RSAT-File-Services',
                'RSAT-DFS-Mgmt-Con',
                'FS-Resource-Manager'
            )
            Ensure = 'Present'
        }

        # FS-SMB1: conditionally install on OS builds < 26100
        Script FSSMB1Feature {
            DependsOn  = '[WindowsFeatureSet]FileServerFeatures'
            GetScript  = {
                $f = Get-WindowsFeature FS-SMB1 -ErrorAction SilentlyContinue
                @{ Result = if ($f) { $f.InstallState } else { 'NotAvailable' } }
            }
            TestScript = {
                $osBuild = [System.Environment]::OSVersion.Version.Build
                if ($osBuild -ge 26100) { return $true }
                $f = Get-WindowsFeature FS-SMB1 -ErrorAction SilentlyContinue
                if ($null -eq $f) { return $true }
                return ($f.InstallState -eq 'Installed')
            }
            SetScript  = {
                $result = Add-WindowsFeature FS-SMB1 -IncludeAllSubFeature -IncludeManagementTools -ErrorAction SilentlyContinue
                if ($null -eq $result -or -not $result.Success) {
                    Write-Warning 'FS-SMB1 installation failed (source files may not be available on this image).'
                }
            }
        }

        # Hyper-V needs Enable-WindowsOptionalFeature -- use Script resource
        Script HyperV {
            DependsOn  = '[WindowsFeatureSet]FileServerFeatures'
            GetScript  = {
                $hv = Get-WindowsOptionalFeature -Online -FeatureName Microsoft-Hyper-V -ErrorAction SilentlyContinue
                @{ Result = if ($hv) { $hv.State } else { 'NotAvailable' } }
            }
            TestScript = {
                $hv = Get-WindowsOptionalFeature -Online -FeatureName Microsoft-Hyper-V -ErrorAction SilentlyContinue
                return ($null -ne $hv -and $hv.State -eq 'Enabled')
            }
            SetScript  = {
                try {
                    Enable-WindowsOptionalFeature -Online -FeatureName Microsoft-Hyper-V -All -NoRestart -ErrorAction Stop
                } catch {
                    Write-Warning "Hyper-V install failed (nested virt may not be supported): $($_.Exception.Message)"
                }
            }
        }

        WindowsFeature RSATHyperV {
            DependsOn            = '[Script]HyperV'
            Name                 = 'RSAT-Hyper-V-Tools'
            Ensure               = 'Present'
            IncludeAllSubFeature = $true
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

                # Find primary NIC by lowest effective metric (InterfaceMetric + RouteMetric).
                # Azure DHCP sets RouteMetric=0 on both NICs, so sorting by RouteMetric alone
                # is non-deterministic. The Bicep-primary NIC always gets a lower InterfaceMetric.
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

        #region -- Firewall Off --------------------------------------------
        Script DisableFirewall {
            DependsOn  = '[Script]MultiNicRouting'
            GetScript  = {
                $profiles = Get-NetFirewallProfile
                @{ Result = ($profiles | Select-Object Name, Enabled | Out-String) }
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

        #region -- PS Remoting ---------------------------------------------
        Script EnablePSRemoting {
            DependsOn  = '[Script]DisableFirewall'
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
                Enable-PSRemoting -SkipNetworkProfileCheck -Force
            }
        }
        #endregion

        #region -- Password Never Expires ----------------------------------
        # 3-branch logic matching SetPasswordNeverExpires.ps1:
        #   DC: dsquery/dsmod (AD DS tools available)
        #   Domain-joined non-DC: CIM local accounts (dsquery not available)
        #   Workgroup: CIM local accounts
        Script PasswordNeverExpires {
            GetScript  = {
                $isDomain = (Get-CimInstance Win32_ComputerSystem).PartOfDomain
                @{ Result = "PartOfDomain: $isDomain" }
            }
            TestScript = {
                $isDomain = (Get-CimInstance Win32_ComputerSystem).PartOfDomain
                if ($isDomain) {
                    # Check if this machine is a DC
                    $isDC = $false
                    try {
                        $dcName = CMD /C "dsquery server -o rdn 2>nul"
                        if ($null -ne $dcName -and $dcName -eq $env:COMPUTERNAME) { $isDC = $true }
                    } catch {}

                    if ($isDC) {
                        $expiring = dsquery user -samid * | dsget user -pwdneverexpires 2>$null |
                            Select-String 'no'
                        return ($null -eq $expiring -or $expiring.Count -eq 0)
                    }
                    else {
                        # Domain-joined non-DC: use CIM for local accounts
                        $accts = Get-CimInstance -ClassName Win32_UserAccount -Filter "Domain='$env:COMPUTERNAME'"
                        $expiring = $accts | Where-Object { $_.PasswordExpires -eq $true }
                        return ($null -eq $expiring -or $expiring.Count -eq 0)
                    }
                }
                else {
                    $accts = Get-CimInstance -ClassName Win32_UserAccount -Filter "Domain='$env:COMPUTERNAME'"
                    $expiring = $accts | Where-Object { $_.PasswordExpires -eq $true }
                    return ($null -eq $expiring -or $expiring.Count -eq 0)
                }
            }
            SetScript  = {
                $isDomain = (Get-CimInstance Win32_ComputerSystem).PartOfDomain
                if ($isDomain) {
                    $isDC = $false
                    try {
                        $dcName = CMD /C "dsquery server -o rdn 2>nul"
                        if ($null -ne $dcName -and $dcName -eq $env:COMPUTERNAME) { $isDC = $true }
                    } catch {}

                    if ($isDC) {
                        dsquery user -samid * | dsmod user -pwdneverexpires yes -mustchpwd no 2>&1 | Out-Null
                    }
                    else {
                        # Domain-joined non-DC: use CIM for local accounts
                        $accts = Get-CimInstance -ClassName Win32_UserAccount -Filter "Domain='$env:COMPUTERNAME'"
                        foreach ($acct in $accts) {
                            Set-CimInstance -InputObject $acct -Property @{ PasswordExpires = $false }
                        }
                    }
                }
                else {
                    $accts = Get-CimInstance -ClassName Win32_UserAccount -Filter "Domain='$env:COMPUTERNAME'"
                    foreach ($acct in $accts) {
                        Set-CimInstance -InputObject $acct -Property @{ PasswordExpires = $false }
                    }
                }
            }
        }
        #endregion

        #region -- SMB RequireSigning --------------------------------------
        Script RequireSigning {
            GetScript  = {
                $cfg = Get-CimInstance -Namespace root/Microsoft/Windows/SMB -ClassName MSFT_SmbServerConfiguration -ErrorAction SilentlyContinue
                @{ Result = if ($cfg) { $cfg.RequireSecuritySignature } else { 'Unknown' } }
            }
            TestScript = {
                $cfg = Get-CimInstance -Namespace root/Microsoft/Windows/SMB -ClassName MSFT_SmbServerConfiguration -ErrorAction SilentlyContinue
                return ($null -ne $cfg -and $cfg.RequireSecuritySignature -eq $true)
            }
            SetScript  = {
                Set-SmbServerConfiguration -RequireSecuritySignature $true -Confirm:$false
            }
        }
        #endregion

        #region -- SMB Share Directories (C: drive) ------------------------
        $shareDirs = @(
            'FileShare', 'SMBBasic', 'SMBBasic\sub',
            'DifferentFromSMBBasic', 'ShareForceLevel2',
            'SMBEncrypted', 'SMBCompressed',
            'DFSRoots\SMBDfs', 'DFSRoots\Standalone',
            'AzShare', 'AzFile', 'AzFolder', 'AzCBAC'
        )

        foreach ($dir in $shareDirs) {
            File "Dir_$($dir -replace '\\','_')" {
                DestinationPath = "C:\$dir"
                Type            = 'Directory'
                Ensure          = 'Present'
            }
        }
        #endregion

        #region -- SMB Shares ----------------------------------------------
        $smbShares = @(
            @{ Name = 'FileShare';              Path = 'C:\FileShare';              FullAccess = 'BUILTIN\Administrators'; Encrypt = $false; Compress = $false },
            @{ Name = 'SMBBasic';               Path = 'C:\SMBBasic';               FullAccess = 'BUILTIN\Administrators'; Encrypt = $false; Compress = $false },
            @{ Name = 'SameWithSMBBasic';       Path = 'C:\SMBBasic';               FullAccess = 'BUILTIN\Administrators'; Encrypt = $false; Compress = $false },
            @{ Name = 'DifferentFromSMBBasic';  Path = 'C:\DifferentFromSMBBasic';  FullAccess = 'BUILTIN\Administrators'; Encrypt = $false; Compress = $false },
            @{ Name = 'ShareForceLevel2';       Path = 'C:\ShareForceLevel2';       FullAccess = 'BUILTIN\Administrators'; Encrypt = $false; Compress = $false },
            @{ Name = 'SMBEncrypted';           Path = 'C:\SMBEncrypted';           FullAccess = 'BUILTIN\Administrators'; Encrypt = $true;  Compress = $false },
            @{ Name = 'SMBCompressed';          Path = 'C:\SMBCompressed';          FullAccess = 'BUILTIN\Administrators'; Encrypt = $false; Compress = $true  },
            @{ Name = 'SMBDfs';                 Path = 'C:\DFSRoots\SMBDfs';        FullAccess = 'BUILTIN\Administrators'; Encrypt = $false; Compress = $false },
            @{ Name = 'Standalone';             Path = 'C:\DFSRoots\Standalone';    FullAccess = 'BUILTIN\Administrators'; Encrypt = $false; Compress = $false },
            @{ Name = 'AzShare';                Path = 'C:\AzShare';                FullAccess = 'BUILTIN\Administrators'; Encrypt = $false; Compress = $false }
        )

        foreach ($share in $smbShares) {
            $shareName     = $share.Name
            $sharePath     = $share.Path
            $shareAccess   = $share.FullAccess
            $shareEncrypt  = $share.Encrypt
            $shareCompress = $share.Compress

            Script "Share_$shareName" {
                DependsOn  = '[WindowsFeatureSet]FileServerFeatures'
                GetScript  = {
                    $s = Get-CimInstance -ClassName Win32_Share -Filter "Name='$($using:shareName)'" -ErrorAction SilentlyContinue
                    @{ Result = if ($s) { $s.Path } else { 'NotFound' } }
                }
                TestScript = {
                    $s = Get-CimInstance -ClassName Win32_Share -Filter "Name='$($using:shareName)'" -ErrorAction SilentlyContinue
                    return ($null -ne $s -and $s.Path -eq $using:sharePath)
                }
                SetScript  = {
                    if (-not (Test-Path $using:sharePath)) {
                        New-Item -ItemType Directory -Path $using:sharePath -Force
                    }
                    icacls $using:sharePath /grant "BUILTIN\Administrators:(OI)(CI)(F)" | Out-Null

                    New-SmbShare -Name $using:shareName -Path $using:sharePath `
                        -FullAccess $using:shareAccess -CachingMode None `
                        -EncryptData $using:shareEncrypt

                    if ($using:shareCompress) {
                        Set-SmbShare -Name $using:shareName -CompressData $true -Force
                    }
                }
            }
        }

        # Auth shares with restricted ACLs
        $authSharesRestricted = @(
            @{ Name = 'AzFile';   Path = 'C:\AzFile' },
            @{ Name = 'AzFolder'; Path = 'C:\AzFolder' }
        )
        foreach ($share in $authSharesRestricted) {
            $shareName = $share.Name
            $sharePath = $share.Path

            Script "Share_$shareName" {
                DependsOn  = '[WindowsFeatureSet]FileServerFeatures'
                GetScript  = {
                    $s = Get-CimInstance -ClassName Win32_Share -Filter "Name='$($using:shareName)'" -ErrorAction SilentlyContinue
                    @{ Result = if ($s) { $s.Path } else { 'NotFound' } }
                }
                TestScript = {
                    $s = Get-CimInstance -ClassName Win32_Share -Filter "Name='$($using:shareName)'" -ErrorAction SilentlyContinue
                    return ($null -ne $s -and $s.Path -eq $using:sharePath)
                }
                SetScript  = {
                    if (-not (Test-Path $using:sharePath)) {
                        New-Item -ItemType Directory -Path $using:sharePath -Force
                    }
                    icacls $using:sharePath /grant "BUILTIN\Administrators:(OI)(CI)(F)" | Out-Null
                    New-SmbShare -Name $using:shareName -Path $using:sharePath `
                        -FullAccess @('BUILTIN\Administrators', 'BUILTIN\Users')

                    $acl = Get-Acl -Path $using:sharePath
                    $acl.SetAccessRuleProtection($true, $true)
                    $acl | Set-Acl
                    $acl = Get-Acl -Path $using:sharePath
                    foreach ($access in $acl.Access) {
                        if ($access.IdentityReference.Value -in @('BUILTIN\Users', 'NT AUTHORITY\Authenticated Users')) {
                            $acl.RemoveAccessRuleAll($access) | Out-Null
                        }
                    }
                    Set-Acl -Path $using:sharePath -AclObject $acl
                }
            }
        }

        Script Share_AzCBAC {
            DependsOn  = '[WindowsFeatureSet]FileServerFeatures'
            GetScript  = {
                $s = Get-CimInstance -ClassName Win32_Share -Filter "Name='AzCBAC'" -ErrorAction SilentlyContinue
                @{ Result = if ($s) { $s.Path } else { 'NotFound' } }
            }
            TestScript = {
                $s = Get-CimInstance -ClassName Win32_Share -Filter "Name='AzCBAC'" -ErrorAction SilentlyContinue
                return ($null -ne $s -and $s.Path -eq 'C:\AzCBAC')
            }
            SetScript  = {
                if (-not (Test-Path 'C:\AzCBAC')) { New-Item -ItemType Directory -Path 'C:\AzCBAC' -Force }
                icacls 'C:\AzCBAC' /grant "BUILTIN\Administrators:(OI)(CI)(F)" | Out-Null
                New-SmbShare -Name 'AzCBAC' -Path 'C:\AzCBAC' `
                    -FullAccess @('BUILTIN\Administrators', 'BUILTIN\Users')
            }
        }
        #endregion

        #region -- FSRM Classification -------------------------------------
        # Uses a registry sentinel to track whether Update-FSRMClassificationPropertyDefinition
        # has been run, since the command's presence alone doesn't indicate initialization.
        Script FSRMClassification {
            DependsOn  = '[WindowsFeatureSet]FileServerFeatures'
            GetScript  = {
                $marker = Get-ItemProperty -Path 'HKLM:\SOFTWARE\ProtocolTestSuites' -Name 'FSRMClassificationUpdated' -ErrorAction SilentlyContinue
                @{ Result = if ($marker) { 'Updated' } else { 'NotUpdated' } }
            }
            TestScript = {
                $marker = Get-ItemProperty -Path 'HKLM:\SOFTWARE\ProtocolTestSuites' -Name 'FSRMClassificationUpdated' -ErrorAction SilentlyContinue
                return ($null -ne $marker)
            }
            SetScript  = {
                Import-Module FileServerResourceManager -ErrorAction SilentlyContinue
                if (Get-Command Update-FSRMClassificationPropertyDefinition -ErrorAction SilentlyContinue) {
                    Update-FSRMClassificationPropertyDefinition
                }
                if (-not (Test-Path 'HKLM:\SOFTWARE\ProtocolTestSuites')) {
                    New-Item -Path 'HKLM:\SOFTWARE\ProtocolTestSuites' -Force | Out-Null
                }
                New-ItemProperty -Path 'HKLM:\SOFTWARE\ProtocolTestSuites' -Name 'FSRMClassificationUpdated' -Value 1 -PropertyType DWord -Force | Out-Null
            }
        }
        #endregion

        #region -- Registry: FSA Last Access Time --------------------------
        Registry NtfsLastAccess {
            Key       = 'HKLM:\SYSTEM\CurrentControlSet\Control\FileSystem'
            ValueName = 'NtfsDisableLastAccessUpdate'
            ValueType = 'Dword'
            ValueData = '0'
            Ensure    = 'Present'
        }

        Registry RefsLastAccess {
            Key       = 'HKLM:\SYSTEM\CurrentControlSet\Control\FileSystem'
            ValueName = 'RefsDisableLastAccessUpdate'
            ValueType = 'Dword'
            ValueData = '0'
            Ensure    = 'Present'
        }
        #endregion

        #region -- Computer Password (ksetup) ------------------------------
        # Moved to Invoke-SutImperativeSteps.ps1 (post-domain-join).
        # The pipeline runs ksetup /SetComputerPassword AFTER domain join
        # (Set-ComputerPassword.ps1 is in <postscript>). Running it before
        # domain join is a no-op because domain join overwrites the local
        # Kerberos key with the AD machine password.
        #endregion
    }
}

# ===========================================================================
function Invoke-SutDomainDsc {
    param(
        [switch]$Apply,
        [string]$OutputPath = "$PSScriptRoot\MOF",
        [string]$ConfigFilePath = "$PSScriptRoot\..\Config.json"
    )

    Write-Host "Compiling Domain SUT DSC configuration..." -ForegroundColor Cyan
    SutConfiguration -ConfigFilePath $ConfigFilePath -OutputPath $OutputPath

    if ($Apply) {
        Write-Host "Applying Domain SUT DSC configuration..." -ForegroundColor Yellow
        Start-DscConfiguration -Path $OutputPath -Wait -Verbose -Force
        Write-Host "Domain SUT DSC configuration applied." -ForegroundColor Green
    }
    else {
        Write-Host "MOF compiled to $OutputPath. Run:" -ForegroundColor Green
        Write-Host "  Start-DscConfiguration -Path '$OutputPath' -Wait -Verbose -Force"
    }
}
