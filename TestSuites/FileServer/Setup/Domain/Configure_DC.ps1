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
    if (-not (.\Validate-Configs.ps1 -Role $Role -ConfigureFile $ConfigureFile -Update $true)) {
        .\Write-Info.ps1 "Configuration Validation failed" -ForegroundColor Red
        return $false
    }

    # Create and Promote to DC
    .\Write-Info.ps1 "Installing AD Features..." -ForegroundColor Yellow
    if (-not (.\Install-AD-Features.ps1)) {
        .\Write-Info.ps1 "Failed to install AD Features" -ForegroundColor Red
        return $false
    }

    # Install RemoteAccess Feature
    .\Write-Info.ps1 "Installing RemoteAccess Feature..." -ForegroundColor Yellow
    if (-not (.\Install-RemoteAccess-Feature.ps1)) {
        .\Write-Info.ps1 "Failed to install RemoteAccess Feature" -ForegroundColor Red
        return $false
    }

    # Install MSI and Tools
    .\Write-Info.ps1 "Installing Tools..." -ForegroundColor Yellow
    if (-not (.\InstallMSIAndTools.ps1 -Role $Role)) {
        .\Write-Info.ps1 "Failed to install Tools" -ForegroundColor Red
        return $false
    }

    return $true
}

Function Phase2 {
    .\Write-Info.ps1 "Entering Phase 2..."
    
    # Turn off firewall
    .\Write-Info.ps1 "Disable firewall..." -ForegroundColor Yellow
    .\Disable_Firewall.ps1

    # Create and Promote to DC
    .\Write-Info.ps1 "Creating Domain Controller..." -ForegroundColor Yellow
    if (-not (.\Create-DC.ps1)) {
        .\Write-Info.ps1 "Failed to create Domain Controller" -ForegroundColor Red
        return $false
    }

    return $true
}

Function Phase3 {
    .\Write-Info.ps1 "Entering Phase 3..."

    # Create Test Account
    .\Write-Info.ps1 "Create Test Account..." -ForegroundColor Yellow
    if (-not (.\Create-TestAccount.ps1)) {
        .\Write-Info.ps1 "Failed to create Test Accounts" -ForegroundColor Red
        return $false
    }

    # Create CbacObjectsInDC
    .\Write-Info.ps1 "Create CbacObjectsInDC..." -ForegroundColor Yellow
    if (-not (.\Create-CbacObjectsInDC.ps1)) {
        .\Write-Info.ps1 "Failed to create CbacObjectsInDC" -ForegroundColor Red
        return $false
    }

    # Import GPOForClaims
    .\Write-Info.ps1 "Import GPOForClaims..." -ForegroundColor Yellow
    if (-not (.\Import-GPOForClaims.ps1)) {
        .\Write-Info.ps1 "Failed to import GPOForClaims" -ForegroundColor Red
        return $false
    }

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

    # Check DCStatus
    .\Write-Info.ps1 "Check DCStatus..." -ForegroundColor Yellow
    .\Check-DCStatus.ps1

    return $true
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
            $success = Phase1
            if ($success) {
                RestartAndResume
            }
            else {
                .\Write-Info.ps1 "DC Configuration failed." -ForegroundColor Red
                Pause
            }
        }
        2 { 
            $success = Phase2
            if ($success) {
                RestartAndResume
            }
            else {
                .\Write-Info.ps1 "Step 2 of DC Configuration failed" -ForegroundColor Red
                Pause
            }
        }
        3 {
            $success = Phase3
            if ($success) {
                RestartAndResume
            }
            else {
                .\Write-Info.ps1 "Step 3 of DC Configuration failed" -ForegroundColor Red
                Pause
            }
        }
        4 {
            Finish
        }
    }
}

Main
Pop-Location