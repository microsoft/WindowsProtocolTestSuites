# Copyright (c) Microsoft. All rights reserved.
# Licensed under the MIT license. See LICENSE file in the project root for full license information.

Param
(
    [Parameter(Mandatory = $false)]
    $WorkingPath = $PSScriptRoot,
    [Parameter(Mandatory = $false)]
    $ConfigureFile = "$WorkingPath\Config.json",
    [int]$Step = 1
)

$Role = "DC"
$CurrentScriptPath = $MyInvocation.MyCommand.Definition
$ScriptsSignalFile = "$WorkingPath\Configure-DC.Completed.signal"
Push-Location $WorkingPath

Function Prepare() {
    .\Write-Info.ps1 "Executing [Configure-DC.ps1] ..." -ForegroundColor Cyan
	
    if (Test-Path -Path $ScriptsSignalFile) {
        .\Write-Info.ps1 "The script execution is complete." -ForegroundColor Red
        exit 0
    }

    .\Write-Info.ps1 "Current path is $CurrentScriptPath" -ForegroundColor Cyan
    $WorkingPath = (Get-Item $WorkingPath).FullName

    $LogPath = "$WorkingPath\Configure-DC.ps1.log"
    try {
        Stop-Transcript -ErrorAction SilentlyContinue
    }
    catch {
        # Ignore any errors from stopping transcript
    }
    Start-Transcript -Path $LogPath -Append

    if (-not (.\Validate-ConfigFile.ps1 -ConfigPath $ConfigureFile)) {
        Pause
        Stop-Transcript
        exit 1
    }
}

Function RestartAndResume {
    .\Write-Info.ps1 "Start Setting next step"
    $NextStep = $Step + 1

    $WorkingPath = (Get-Item $WorkingPath).FullName
    $ConfigureFile = (Get-Item $ConfigureFile).FullName

    .\RestartAndRun.ps1 -ScriptPath $CurrentScriptPath `
        -PhaseIndicator "-Step $NextStep" `
        -ArgumentList "-WorkingPath '$WorkingPath' -ConfigureFile '$ConfigureFile'" `
        -AutoRestart $true
}


Function Phase1 {
    .\Write-Info.ps1 "Entering Phase 1..."
    
    # Validate Configurations
    .\Write-Info.ps1 "Validating Configurations..." -ForegroundColor Yellow
    .\Validate-Configs.ps1 -Role $Role -ConfigureFile $ConfigureFile -Update $true

    # Create and Promote to DC
    .\Write-Info.ps1 "Creating Domain Controller..." -ForegroundColor Yellow
    .\Install-AD-Features.ps1

    # Install RemoteAccess Feature
    .\Write-Info.ps1 "Installing RemoteAccess Feature..." -ForegroundColor Yellow
    .\Install-RemoteAccess-Feature.ps1

    # Install MSI and Tools
    .\Write-Info.ps1 "Installing Tools..." -ForegroundColor Yellow
    .\InstallMSIAndTools.ps1 -Role $Role
}

Function Phase2 {
    .\Write-Info.ps1 "Entering Phase 2..."
    
    # Turn off firewall
    .\Write-Info.ps1 "Disable firewall..." -ForegroundColor Yellow
    .\Disable_Firewall.ps1

    # Create and Promote to DC
    .\Write-Info.ps1 "Creating Domain Controller..." -ForegroundColor Yellow
    .\Create-DC.ps1

}

Function Phase3 {
    .\Write-Info.ps1 "Entering Phase 3..."

    # Create Test Account
    .\Write-Info.ps1 "Create Test Account..." -ForegroundColor Yellow
    .\Create-TestAccount.ps1

    # Create CbacObjectsInDC
    .\Write-Info.ps1 "Create CbacObjectsInDC..." -ForegroundColor Yellow
    .\Create-CbacObjectsInDC.ps1

    # Import GPOForClaims
    .\Write-Info.ps1 "Import GPOForClaims..." -ForegroundColor Yellow
    .\Import-GPOForClaims.ps1

    # Disable LDAP signing to skip "Novell.Directory.Ldap.LdapException: Strong Authentication Required" error when connect with 389 port.
    .\Write-Info.ps1 "Disable LDAP signing..." -ForegroundColor Yellow
    $params = Get-Item "HKLM:\SYSTEM\CurrentControlSet\Services\NTDS\Parameters" -ErrorAction SilentlyContinue
    if ($null -ne $params) {
        $ldapServerEnforceIntegrity = Get-ItemProperty -Path "HKLM:\SYSTEM\CurrentControlSet\Services\NTDS\Parameters" -Name 'LDAPServerEnforceIntegrity' -ErrorAction SilentlyContinue
        if ($null -ne $ldapServerEnforceIntegrity) {
            Set-ItemProperty -Path "HKLM:\SYSTEM\CurrentControlSet\Services\NTDS\Parameters" -Name 'LDAPServerEnforceIntegrity' -Value '0x00000000' -Force | Out-Null
        }
        else {
            New-ItemProperty -Path "HKLM:\SYSTEM\CurrentControlSet\Services\NTDS\Parameters" -Name 'LDAPServerEnforceIntegrity' -Value '0x00000000' -PropertyType 'DWord' -Force | Out-Null
        }
        $ldapServerIntegrity = Get-ItemProperty -Path "HKLM:\SYSTEM\CurrentControlSet\Services\NTDS\Parameters" -Name 'LDAPServerIntegrity' -ErrorAction SilentlyContinue
        if ($null -ne $ldapServerIntegrity) {
            Set-ItemProperty -Path "HKLM:\SYSTEM\CurrentControlSet\Services\NTDS\Parameters" -Name 'LDAPServerIntegrity' -Value '0x00000001' -Force | Out-Null
        }
        else {
            New-ItemProperty -Path "HKLM:\SYSTEM\CurrentControlSet\Services\NTDS\Parameters" -Name 'LDAPServerIntegrity' -Value '0x00000001' -PropertyType 'DWord' -Force | Out-Null
        }
    }

    # Create DNS records for cluster hosts
    
    .\Write-Info.ps1 "Create DNS records for cluster hosts..." -ForegroundColor Yellow
    .\Create-DNSRecords.ps1

    # Check DCStatus
    .\Write-Info.ps1 "Check DCStatus..." -ForegroundColor Yellow
    .\Check-DCStatus.ps1
}

Function Finish {
    .\Write-Info.ps1 "Write signal file: Configure-DC.Completed.signal to system drive."
    cmd /C ECHO CONFIG FINISHED>$ScriptsSignalFile

    .\Write-Info.ps1 "DC Configuration finished."
    .\Write-Info.ps1 "EXECUTE [Configure-DC.ps1] FINISHED (NOT VERIFIED)." -ForegroundColor Green
    
    .\RestartAndRunFinish.ps1
}

Function Main {
    Prepare

    switch ($Step) {
        1 { 
            Phase1
            RestartAndResume
        }
        2 { 
            Phase2
            RestartAndResume
        }
        3 {
            Phase3
            RestartAndResume
        }
        4 {
            Finish
        }
    }
}

Main
Pop-Location