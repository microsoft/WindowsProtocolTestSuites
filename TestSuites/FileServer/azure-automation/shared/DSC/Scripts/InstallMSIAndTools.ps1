# Copyright (c) Microsoft. All rights reserved.
# Licensed under the MIT license. See LICENSE file in the project root for full license information.

#############################################################################
##
## Microsoft Windows Powershell Scripting
## File:           InstallMSIandTools.ps1
## Requirements:   Windows Powershell 2.0
## Supported OS:   Windows 7
##
##############################################################################
Param
(
    [Parameter(Mandatory = $true)]
    [ValidateSet("SUT", "DriverComputer", "DC", "Node01", "Node02", "Storage")]
    [string]$Role
)

Function Test-InternetConnection {

    Try {
        # Switching to Google - MaxRequest issue
        $response = Invoke-WebRequest -Uri http://www.google.com -UseBasicParsing -TimeoutSec 10
        return $response.StatusCode -eq 200
    }
    Catch {
        return $false
    }
}

# Get-RemoteFile is shared across the DSC scripts (BITS-based, redirect-hardened).
. "$PSScriptRoot\Get-RemoteFile.ps1"


if (-not (Test-InternetConnection)) {
    .\Write-Info.ps1 "No internet connection. The script requires the computer be connected to internet. Exiting script." -ForegroundColor Red
    return $false
}

$testDir = $PSScriptRoot
$toolsPath = "$testDir\Tools"
$ConfigureFile = "$testDir\Tools.json"
$signalfile = "$testDir\InstallMSIAndTools.Completed.signal"
if (Test-Path $signalfile) {
    .\Write-Info.ps1 "Tools Installed" -ForegroundColor Green
    return $true
}

Start-Transcript -Path "$testDir\InstallMSIAndTools.ps1.log" -Append -Force

$config = $null

try {
    $config = Get-Content -Path $ConfigureFile -Raw | ConvertFrom-Json
}
catch {
    .\Write-Info.ps1 "Failed to parse config file: $_" -ForegroundColor Red
    Stop-Transcript
    return $false
}

$Tools = $config.$Role.Tools

# TODO: Simplify this logic
if ($null -eq $Tools) {
    .\Write-Info.ps1 "Cannot find Tools configuration for Role $Role." -ForegroundColor Red
} else {
    .\Write-Info.ps1 "Installing tools for Role $Role." -ForegroundColor Cyan    
    $IsVcredistInstalled = $false
    $currentOSBuild = [System.Environment]::OSVersion.Version.Build.ToString();

    foreach ($Tool in $Tools) {
        $CPUArchitecture = $Tool.CPUArchitecture
        $ArgumentList = $Tool.ArgumentList
        $MSIName = $Tool.MSIName
        $NotSupportedOSBuilds = $Tool.NotSupportedOSBuilds
        $DownloadURL = $Tool.Url

        if ($null -ne $MSIName) {
            $toolPath = "$toolsPath\$MSIName"
            if (-not (Test-Path($toolPath)) -and $null -ne $Tool.Url) {
                .\Write-Info.ps1 "The MSI file $MSIName does not exist in the tools folder. Attempting Download."
                Get-RemoteFile -Url $DownloadURL -OutputPath $toolPath
            }
            if (($null -ne $NotSupportedOSBuilds) -and ($NotSupportedOSBuilds -match $currentOSBuild)) {
                continue;
            }
            .\Write-Info.ps1 "Install tool: $toolsPath\$MSIName"
            if ($null -ne $ArgumentList) {
                cmd /c "msiexec /i $toolsPath\$MSIName $ArgumentList" 2>&1 | .\Write-Info.ps1
            }
            else {
                cmd /c msiexec /quiet /i $toolsPath\$MSIName 2>&1 | .\Write-Info.ps1
            }
        }
        else {
            $EXEName = $Tool.EXEName
            $toolPath = "$toolsPath\$EXEName"
            $DownloadURL = $Tool.Url

            if (-not (Test-Path($toolPath)) -and $null -ne $Tool.Url) {
                .\Write-Info.ps1 "The EXE file $EXEName does not exist in the tools folder. Attempting Download."
                Get-RemoteFile -Url $DownloadURL -OutputPath $toolPath
            }

            if ($null -ne $EXEName) {
                if ($EXEName -eq "vcredist.exe") {
                    if ($IsVcredistInstalled -eq $false) {
                        $IsVcredistInstalled = $true
                    }
                    else {
                        $EXEName = $CPUArchitecture + "_vcredist.exe"
                    }
                }

                $SupportedOSBuilds = $Tool.SupportedOSBuilds
                $InstallWaitSeconds = $Tool.InstallWaitSeconds
                if ($null -ne $SupportedOSBuilds) {
                    foreach ($SupportedOSBuild in $SupportedOSBuilds.Split(",")) {
                        if ($SupportedOSBuild -eq $currentOSBuild) {
                            .\Write-Info.ps1 "install tool on OSBuild $SupportedOSBuild : $toolsPath\$EXEName $ArgumentList"
                            CMD /C "$toolsPath\$EXEName $ArgumentList" 2>&1 | .\Write-Info.ps1
                            # Wait the msu installation to complete.
                            if ($null -ne $InstallWaitSeconds) {
                                Start-Sleep -s $InstallWaitSeconds
                            }
                            break
                        }
                    }
                }
                else {
                    .\Write-Info.ps1 "install tool: $toolsPath\$EXEName $ArgumentList"
                    CMD /C "$toolsPath\$EXEName $ArgumentList" 2>&1 | .\Write-Info.ps1
                }
            }
            else {
                $ZipName = $Tool.ZipName
                $toolPath = "$toolsPath\$ZipName"
                $DownloadURL = $Tool.Url

                if (-not (Test-Path($toolPath)) -and $null -ne $Tool.Url) {
                    .\Write-Info.ps1 "The Zip file $ZipName does not exist in the tools folder. Attempting Download."
                    Get-RemoteFile -Url $DownloadURL -OutputPath $toolPath
                }

                if ($null -ne $ZipName) {
                    $targetFolder = [Environment]::ExpandEnvironmentVariables($Tool.targetFolder)
                    $installScript = $Tool.installScript

                    try {
                        # Extract zip file
                        .\Write-Info.ps1 "Expanding zip: $toolPath to $targetFolder"
                        try {
                            # Try Expand-Archive first (PowerShell 5+)
                            if ($PSVersionTable.PSVersion.Major -ge 5) {
                                Expand-Archive -Path $toolPath -DestinationPath $targetFolder -Force
                            }
                            else {
                                # Fallback to .NET method for older PowerShell versions
                                Add-Type -Assembly "System.IO.Compression.FileSystem"
                                [IO.Compression.ZipFile]::ExtractToDirectory($toolPath, $targetFolder)
                            }
                        }
                        catch {
                            throw "Failed to extract zip file: $($_.Exception.Message)"
                        }

                        if ($null -ne $installScript) {
                            $installScriptArgs = $Tool.installScriptArgs
                            .\Write-Info.ps1 "install script of zip tool: $targetFolder\$installScript $installScriptArgs"
                            # Run install script in a separate interactive process to avoid
                            # Read-Host failures when the parent runs with -NonInteractive
                            $scriptArgs = "-ExecutionPolicy Bypass -File `"$targetFolder\$installScript`""
                            if (-not [string]::IsNullOrEmpty($installScriptArgs)) {
                                $scriptArgs += " $installScriptArgs"
                            }
                            # PTMService install.ps1 has multiple Read-Host prompts, provide answers for all
                            $stdinValue = "N`n"
                            if ($Tool.name -eq "PTMService") {
                                # Answers: keep default ID, keep default path, no shortcut, default host, default HTTP, default HTTPS, don't start now
                                $stdinValue = "N`nN`nN`nN`nN`nN`nN`n"
                            }
                            $installProc = Start-Process -FilePath "powershell.exe" `
                                -ArgumentList $scriptArgs `
                                -RedirectStandardInput (New-Item -Path $env:TEMP\nul_input.txt -Value $stdinValue -Force).FullName `
                                -Wait -PassThru -NoNewWindow
                            if ($installProc.ExitCode -ne 0) {
                                .\Write-Info.ps1 "Install script exited with code $($installProc.ExitCode)" -ForegroundColor Yellow
                            }
                        }
                    }
                    catch {
                        .\Write-Info.ps1 "Error installing zip tool: $($_.Exception.Message)" -ForegroundColor Red
                    }
                }
            }
        }
    }
    }

    $TestsuiteZips = $config.$Role.TestsuiteZips
    foreach ($TestsuiteZip in $TestsuiteZips) {
        $ZipName = $TestsuiteZip.ZipName
        $targetFolder = [Environment]::ExpandEnvironmentVariables($TestsuiteZip.targetFolder)
        $toolPath = "$toolsPath\$ZipName"
        $DownloadURL = $TestsuiteZip.Url

        if (-not (Test-Path($toolPath)) -and $null -ne $TestsuiteZip.Url) {
            .\Write-Info.ps1 "The Zip file $ZipName does not exist in the tools folder. Attempting Download."
            Get-RemoteFile -Url $DownloadURL -OutputPath $toolPath
        }

        .\Write-Info.ps1 "Expand test suite: $ZipName"
        if ($psversiontable.PSVersion.Major -ge 5) {
            Expand-Archive $toolPath -DestinationPath $targetFolder -Force
        }
        else {
            $shell = New-Object -com shell.application
            $zip = $shell.NameSpace("$toolsPath\$ZipName")
            if (!(Test-Path -Path $targetFolder)) {
                New-Item -ItemType directory -Path $targetFolder
            }
            $shell.Namespace($targetFolder).CopyHere($zip.items(), 0x14)
        }
    }


    CMD /C ECHO "Completed" > $signalfile
    Stop-Transcript
    return $true