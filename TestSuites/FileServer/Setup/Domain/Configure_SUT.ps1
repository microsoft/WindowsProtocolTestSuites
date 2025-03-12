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

    # Join Domain
    .\Write-Info.ps1 "Joining Domain..." -ForegroundColor Yellow
    if (-not (.\domainjoin.ps1)) {      
        .\Write-Info.ps1 "Domain Join Failed" -ForegroundColor Red
        return $false
    }

    return $true
}

Function Phase2 {
    .\Write-Info.ps1 "Entering Phase 2..." -ForegroundColor Yellow
    
    # Install MSI and Tools
    .\Write-Info.ps1 "Installing Tools..." -ForegroundColor Yellow

    if (-not (.\InstallMSIAndTools.ps1 -Role $Role)) {
        .\Write-Info.ps1 "Failed to install MSI and Tools" -ForegroundColor Red
        return $false
    }

    # Install Windows Features
    .\Write-Info.ps1 "Installing Windows Features..." -ForegroundColor Yellow
    if (-not (.\Install-WindowsFeature.ps1)) {
        .\Write-Info.ps1 "Failed to install Windows Features" -ForegroundColor Red
        return $false
    }

    return $true
}

Function Phase3 {
    .\Write-Info.ps1 "Entering Phase 3..."

    # Wait for computer to be stable
    Start-Sleep 30
	
    # Turn off firewall
    .\Write-Info.ps1 "Disable firewall..." -ForegroundColor Yellow
    .\Disable_Firewall.ps1

    # Set Password Never Expires
    .\Write-Info.ps1 "Set Password Never Expires..." -ForegroundColor Yellow
    .\SetPasswordNeverExpires.ps1

    # Create QUIC Environment
    .\Write-Info.ps1 "Create QUIC Environment..." -ForegroundColor Yellow
    if (-not (.\Create-QUICEnv.ps1)) {
        .\Write-Info.ps1 "Failed to create QUIC Environment" -ForegroundColor Red
        return $false
    }

    # Create SMB2 Environment
    .\Write-Info.ps1 "Create SMB2 Environment..." -ForegroundColor Yellow
    if (-not (.\Create-SMB2Env.ps1)) {
        .\Write-Info.ps1 "Failed to create SMB2 Environment" -ForegroundColor Red
        return $false
    }

    # Create DFSC Environment
    .\Write-Info.ps1 "Create DFSC Environment..." -ForegroundColor Yellow
    if (-not (.\Create-DFSCEnv.ps1)) {
        .\Write-Info.ps1 "Failed to create DFSC Environment" -ForegroundColor Red
        return $false
    }

    # Create FSA Environment
    .\Write-Info.ps1 "Create FSA Environment..." -ForegroundColor Yellow
    if (-not (.\Create-FSAEnv.ps1)) {
        .\Write-Info.ps1 "Failed to create FSA Environment" -ForegroundColor Red
        return $false
    }

    # Create Auth Environment
    .\Write-Info.ps1 "Create Auth Environment..." -ForegroundColor Yellow
    if (-not (.\Create-AuthEnv.ps1)) {
        .\Write-Info.ps1 "Failed to create Auth Environment" -ForegroundColor Red
        return $false
    }

    # Set RequireSigning
    .\Write-Info.ps1 "Set RequireSigning..." -ForegroundColor Yellow
    .\Set-RequireSigning.ps1

    # Set ComputerPassword
    .\Write-Info.ps1 "Set ComputerPassword..." -ForegroundColor Yellow
    .\Set-ComputerPassword.ps1

    # Config ForceLevel2
    # Can only be done after configuring driver computer
    # TODO: Trigger remote execution from driver computer
    # .\Write-Info.ps1 "Config ForceLevel2..." -ForegroundColor Yellow
    # .\Config-ForceLevel2.ps1

    return $true
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
                Finish
            }
            else {
                .\Write-Info.ps1 "Step 3 of DC Configuration failed" -ForegroundColor Red
                Pause
            }
        }
    }
}

Main
Pop-Location