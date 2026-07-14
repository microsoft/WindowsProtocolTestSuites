# Copyright (c) Microsoft. All rights reserved.
# Licensed under the MIT license. See LICENSE file in the project root for full license information.

# Get-RemoteFile.ps1
# Hardened file download used across the DSC scripts. Uses BITS (Start-BitsTransfer),
# which is robust on the VM's Windows PowerShell 5.1: it follows HTTP redirects
# (including 307/308 -- which Invoke-WebRequest does NOT auto-follow on 5.1), shows
# progress, and cleans up partial downloads on failure.
#
# Dot-source this script to import the function:
#     . "$PSScriptRoot\Get-RemoteFile.ps1"
#     Get-RemoteFile -Url $url -OutputPath $dest

Function Get-RemoteFile {
    param (
        [Parameter(Mandatory)]
        [string]$Url,

        [Parameter(Mandatory)]
        [string]$OutputPath
    )

    # Ensure directory exists
    $directory = Split-Path -Path $OutputPath -Parent
    if (-not (Test-Path -Path $directory)) {
        New-Item -ItemType Directory -Force -Path $directory | Out-Null
    }

    try {
        # Display download starting message
        .\Write-Info.ps1 "Starting download of: $([System.IO.Path]::GetFileName($OutputPath)) URL -- $Url" -ForegroundColor Cyan

        # Use BITS transfer with progress
        $job = Start-BitsTransfer -Source $Url -Destination $OutputPath -DisplayName "Downloading..." -Dynamic -Asynchronous

        .\Write-Info.ps1 "Downloading file..." -ForegroundColor Cyan
        # Wait for and complete the transfer
        while ($job.JobState -in @('Transferring', 'Connecting')) {
            $pct = if ($job.BytesTotal -gt 0) { [math]::Min(100, $job.BytesTransferred / $job.BytesTotal * 100) } else { 0 }
            Write-Progress -Activity $job.DisplayName -Status "$($job.BytesTransferred) / $($job.BytesTotal) bytes" -PercentComplete $pct
            Start-Sleep -Seconds 1
        }

        Switch ($job.JobState) {
            'Transferred' {
                Complete-BitsTransfer -BitsJob $job
                .\Write-Info.ps1 "Download completed successfully" -ForegroundColor Green
                return $true
            }
            'Error' {
                $job | Remove-BitsTransfer
                .\Write-Info.ps1 "Error downloading file: $($job.ErrorDescription)" -ForegroundColor Red
                return $false
            }
            default {
                $job | Remove-BitsTransfer
                .\Write-Info.ps1 "Transfer ended with unexpected state: $($job.JobState)" -ForegroundColor Yellow
                return $false
            }
        }
    }
    catch {
        .\Write-Info.ps1 "Error downloading file: $($_.Exception.Message)" -ForegroundColor Red

        # Clean up failed download if file exists
        if (Test-Path $OutputPath) {
            Remove-Item -Path $OutputPath -Force
        }
        return $false
    }
}
