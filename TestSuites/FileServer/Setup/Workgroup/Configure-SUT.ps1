# Copyright (c) Microsoft. All rights reserved.
# Licensed under the MIT license. See LICENSE file in the project root for full license information.

Param
(
    [Parameter(Mandatory=$false)]
    $WorkingPath = $PSScriptRoot,
    [Parameter(Mandatory=$false)]
    $ConfigureFile = "$WorkingPath\Config.json",
    [int]$Step = 1
)

$Role = "SUT"
$CurrentScriptPath = $MyInvocation.MyCommand.Definition
$ScriptsSignalFile = "$WorkingPath\Configure-SUT.Completed.signal"
Push-Location $WorkingPath

Function Prepare() {
    .\Write-Info.ps1 "Executing [Configure-SUT.ps1] ..." -ForegroundColor Cyan
	
    if (Test-Path -Path $ScriptsSignalFile) {
        .\Write-Info.ps1 "The script execution is complete." -ForegroundColor Red
        exit 0
    }

    .\Write-Info.ps1 "Current path is $CurrentScriptPath" -ForegroundColor Cyan
    $WorkingPath = (Get-Item $WorkingPath).FullName

    $LogPath = "$WorkingPath\Configure-SUT.ps1.log"
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
    .\Write-Info.ps1 "Entering Phase 1..." -ForegroundColor Yellow

    # Validate Configurations
    .\Write-Info.ps1 "Validating Configurations..." -ForegroundColor Yellow
    .\Validate-Configs.ps1 -Role $Role -ConfigureFile $ConfigureFile -Update $true
    
    # Install MSI and Tools
    .\Write-Info.ps1 "Installing Tools..." -ForegroundColor Yellow
    .\InstallMSIAndTools.ps1 -Role $Role

    # Install Windows Features
    .\Write-Info.ps1 "Installing Windows Features..." -ForegroundColor Yellow
    .\Install-WindowsFeature.ps1
}

Function Phase2 {
    .\Write-Info.ps1 "Entering Phase 2..."

    # Wait for computer to be stable
    Start-Sleep 30
	
    .\Write-Info.ps1 "Disable firewall..." -ForegroundColor Yellow
    .\Disable_Firewall.ps1

    .\Write-Info.ps1 "Set Password Never Expires..." -ForegroundColor Yellow
    .\SetPasswordNeverExpires.ps1

    .\Write-Info.ps1 "Create SMB2 Environment..." -ForegroundColor Yellow
    .\Create-SMB2Env.ps1

    .\Write-Info.ps1 "Create DFSC Environment..." -ForegroundColor Yellow
    .\Create-DFSCEnv.ps1

    .\Write-Info.ps1 "Create FSA Environment..." -ForegroundColor Yellow
    .\Create-FSAEnv.ps1

    .\Write-Info.ps1 "Create Auth Environment..." -ForegroundColor Yellow
    .\Create-AuthEnv.ps1

    .\Write-Info.ps1 "Set RequireSigning..." -ForegroundColor Yellow
    .\Set-RequireSigning.ps1

    .\Write-Info.ps1 "Set ComputerPassword..." -ForegroundColor Yellow
    .\Set-ComputerPassword.ps1
}

Function Finish {
    .\Write-Info.ps1 "Write signal file: Configure-SUT.Completed.signal to system drive."
    cmd /C ECHO CONFIG FINISHED>$ScriptsSignalFile

    .\Write-Info.ps1 "SUT Configuration finished."
    .\Write-Info.ps1 "EXECUTE [Configure-SUT.ps1] FINISHED (NOT VERIFIED)." -ForegroundColor Green
    
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
            Finish
        }
    }
}

Main
Pop-Location