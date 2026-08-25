# Copyright (c) Microsoft. All rights reserved.
# Licensed under the MIT license. See LICENSE file in the project root for full license information.

<#
.SYNOPSIS
    Non-disruptive convergence DSC for the Storage Server (Storage01).

.DESCRIPTION
    Storage01 is a workgroup machine (NOT domain-joined) that serves as an iSCSI target
    for the failover cluster nodes.

    Declarative convergence:
      - Firewall disabled
      - Hosts file from Config.json
      - Password never expires (workgroup accounts)
      - WinTarget service set to Automatic and Running

    Disruptive features are installed by Storage-FeatureConfiguration.ps1.
    Imperative convergence (Invoke-StorageImperativeSteps.ps1):
      - iSCSI target creation + virtual disk mapping

.EXAMPLE
    . .\Storage-Configuration.ps1
    StorageConfiguration -ConfigFilePath .\..\Config.json -OutputPath .\MOF
    Invoke-VerifiedDscConfiguration -Path .\MOF
#>

Configuration StorageConfiguration {

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
                    if ($null -ne $domainName -and
                        $null -ne $machine.Domain -and
                        $machine.Domain -ne 'Workgroup' -and
                        $machine.Domain -ne '') {
                        $hostsEntries += "$ip`t$name.$domainName"
                    }
                }
            }
        }
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
                $startPattern = [regex]::Escape("$marker START")
                $endPattern   = [regex]::Escape("$marker END")
                $content = $content -replace "(?s)$startPattern.*?$endPattern\r?\n?", ''
                $content = $content.TrimEnd()
                $block = "`r`n$marker START`r`n"
                foreach ($entry in $entries) {
                    $block += "$entry`r`n"
                }
                $block += "$marker END`r`n"
                Set-Content -Path $hostsPath -Value ($content + $block) -Force -Encoding ASCII
            }
        }
        #endregion

        #region -- WinTarget Service Auto-Start ----------------------------
        Script WinTargetAutoStart {
            GetScript  = {
                $svc = Get-Service WinTarget -ErrorAction SilentlyContinue
                @{ Result = if ($svc) { "$($svc.StartType)/$($svc.Status)" } else { 'NotInstalled' } }
            }
            TestScript = {
                $svc = Get-Service WinTarget -ErrorAction SilentlyContinue
                return ($null -ne $svc -and
                    $svc.StartType -eq 'Automatic' -and
                    $svc.Status -eq 'Running')
            }
            SetScript  = {
                Set-Service WinTarget -StartupType Automatic -ErrorAction Stop
                $svc = Get-Service WinTarget -ErrorAction Stop
                if ($svc.Status -ne 'Running') {
                    Start-Service WinTarget -ErrorAction Stop
                }
            }
        }
        #endregion

        #region -- Firewall Off --------------------------------------------
        Script DisableFirewall {
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

        #region -- Password Never Expires ----------------------------------
        Script PasswordNeverExpires {
            GetScript  = {
                @{ Result = "Workgroup machine" }
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
        }
        #endregion
    }
}

# ===========================================================================
function Invoke-StorageDsc {
    param(
        [string]$OutputPath = "$PSScriptRoot\MOF",
        [string]$ConfigFilePath = "$PSScriptRoot\..\Config.json"
    )

    Write-Host "Compiling Storage DSC configuration..." -ForegroundColor Cyan
    StorageConfiguration -ConfigFilePath $ConfigFilePath -OutputPath $OutputPath
    Write-Host "Storage convergence MOF compiled to '$OutputPath'." -ForegroundColor Green
}
