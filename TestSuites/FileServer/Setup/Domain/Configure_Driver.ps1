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

$Role = "DriverComputer"
$CurrentScriptPath = $MyInvocation.MyCommand.Definition
$ScriptsSignalFile = "$WorkingPath\Configure-Driver.Completed.signal"
Push-Location $WorkingPath

Function Prepare() {
    .\Write-Info.ps1 "Executing [Configure-Driver.ps1] ..." -ForegroundColor Cyan
	
    if (Test-Path -Path $ScriptsSignalFile) {
        .\Write-Info.ps1 "The script execution is complete." -ForegroundColor Red
        exit 0
    }

    .\Write-Info.ps1 "Current path is $CurrentScriptPath" -ForegroundColor Cyan
    $WorkingPath = (Get-Item $WorkingPath).FullName

    $LogPath = "$WorkingPath\Configure-Driver.ps1.log"
    Start-Transcript -Path $LogPath -Append 2>&1 | Out-Null

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
    # Validate Configurations
    .\Write-Info.ps1 "Entering Phase 1..."

    .\Write-Info.ps1 "Validating Configurations..." -ForegroundColor Yellow
    .\Validate-Configs.ps1 -Role $Role -ConfigureFile $ConfigureFile -Update $true

    # Join Domain
    .\Write-Info.ps1 "Joining Domain..." -ForegroundColor Yellow
    if (-not (.\domainjoin.ps1)) {      
        .\Write-Info.ps1 "Domain Join Failed" -ForegroundColor Red
        return $false
    }
    
    return $true
}

Function Phase2 {
    .\Write-Info.ps1 "Entering Phase 2..."

    Start-Sleep 30

    # Disable firewall
    .\Write-Info.ps1 "Disable firewall..." -ForegroundColor Yellow
    .\Disable_Firewall.ps1

    # Set Password Never Expires
    .\Write-Info.ps1 "Set Password Never Expires..." -ForegroundColor Yellow
    .\SetPasswordNeverExpires.ps1

    
    # Install MSI and Tools
    .\Write-Info.ps1 "Installing Tools..." -ForegroundColor Yellow
    if (-not (.\InstallMSIAndTools.ps1 -Role $Role)) {
        .\Write-Info.ps1 "Failed to install MSI and Tools" -ForegroundColor Red
        return $false
    }

    # Config RSA Keys
    .\Write-Info.ps1 "Configuring RSA Keys..." -ForegroundColor Yellow
    if (-not (.\Config-RSAKeys.ps1)) {
        .\Write-Info.ps1 "Failed to configure RSA Keys" -ForegroundColor Red
        return $false
    }

    # Config ForceLevel2
    .\Write-Info.ps1 "Config ForceLevel2..." -ForegroundColor Yellow
    if (-not (.\Config-ForceLevel2.ps1)) {
        .\Write-Info.ps1 "Failed to configure ForceLevel2" -ForegroundColor Red
        return $false
    }

    return $true
}

Function Finish {
    .\Write-Info.ps1 "Write signal file: Configure-Driver.Completed.signal to system drive."
    cmd /C ECHO CONFIG FINISHED>$ScriptsSignalFile

    .\Write-Info.ps1 "post Config finished."
    .\Write-Info.ps1 "EXECUTE [Configure-Driver.ps1] FINISHED (NOT VERIFIED)." -ForegroundColor Green
    
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
                .\Write-Info.ps1 "Driver Configuration failed." -ForegroundColor Red
                Pause
            }
        }
        2 { 
            $success = Phase2
            if ($success) {
                RestartAndResume
            }
            else {
                .\Write-Info.ps1 "Step 2 of Driver Configuration failed" -ForegroundColor Red
                Pause
            }
        }
        3 {
            Finish
        }
    }
}

Main
Pop-Location