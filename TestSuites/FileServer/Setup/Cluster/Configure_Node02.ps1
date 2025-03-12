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

$Role = "Node02"
$CurrentScriptPath = $MyInvocation.MyCommand.Definition
$ScriptsSignalFile = "$WorkingPath\Configure-Node02.Completed.signal"
Push-Location $WorkingPath

Function Prepare() {
    .\Write-Info.ps1 "Executing [Configure-Node02.ps1] ..." -ForegroundColor Cyan
	
    if (Test-Path -Path $ScriptsSignalFile) {
        .\Write-Info.ps1 "The script execution is complete." -ForegroundColor Red
        exit 0
    }

    .\Write-Info.ps1 "Current path is $CurrentScriptPath" -ForegroundColor Cyan
    $WorkingPath = (Get-Item $WorkingPath).FullName

    $LogPath = "$WorkingPath\Configure-Node02.ps1.log"
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
    # Check if domain exists
    .\domainjoin.ps1
}

Function Phase2 {
    .\Write-Info.ps1 "Entering Phase 2..." -ForegroundColor Yellow
    
    # Install MSI and Tools
    .\Write-Info.ps1 "Installing Tools..." -ForegroundColor Yellow
    .\InstallMSIAndTools.ps1 -Role $Role

    # Install Windows Features
    .\Write-Info.ps1 "Installing Windows Features..." -ForegroundColor Yellow
    .\Install-WindowsFeature.ps1
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

    # Connect IscsiTarget
    .\Write-Info.ps1 "Connect IscsiTarget..." -ForegroundColor Yellow
    .\Connect-IscsiTarget.ps1

    # Configure Cluster Node02
    .\Write-Info.ps1 "Configure Cluster Node02..." -ForegroundColor Yellow
    .\Config-ClusterNode2.ps1

    # Create Asymmetry Mode
    .\Write-Info.ps1 "Create Asymmetry Mode..." -ForegroundColor Yellow
    .\Config-AsymmetryMode.ps1

    # Set RequireSigning
    .\Write-Info.ps1 "Set RequireSigning..." -ForegroundColor Yellow
    .\Set-RequireSigning.ps1

    # Check Cluster Node Status
    .\Write-Info.ps1 "Check Cluster Node Status..." -ForegroundColor Yellow
    .\Check-ClusterNodeStatus.ps1
}

Function Finish {
    .\Write-Info.ps1 "Write signal file: Configure-Node02.Completed.signal to system drive."
    cmd /C ECHO CONFIG FINISHED>$ScriptsSignalFile

    .\Write-Info.ps1 "Node02 Configuration finished."
    .\Write-Info.ps1 "EXECUTE [Configure-Node02.ps1] FINISHED (NOT VERIFIED)." -ForegroundColor Green
    
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