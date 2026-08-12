# Copyright (c) Microsoft. All rights reserved.
# Licensed under the MIT license. See LICENSE file in the project root for full license information.

# Get-RemoteFile.ps1
# Hardened file download used across the DSC scripts. Tries BITS (Start-BitsTransfer)
# first -- robust on Windows PowerShell 5.1: follows HTTP redirects (including 307/308,
# which Invoke-WebRequest does NOT auto-follow on 5.1), shows progress, cleans up partial
# downloads. Falls back to an in-process HttpClient download when BITS fails.
#
# WHY THE FALLBACK: BITS requires an interactive user logon session. When these scripts
# resume after a reboot via the TKFRSAR scheduled task (e.g. domain member running as
# CONTOSO\testadmin), BITS fails with 0x800704DD ("the user has not logged on to the
# network"). HttpClient is a plain in-process socket call with no logon-token requirement
# and explicitly follows redirects, so it works in that context.
#
# Dot-source this script to import the function:
#     . "$PSScriptRoot\Get-RemoteFile.ps1"
#     Get-RemoteFile -Url $url -OutputPath $dest

Function Get-RemoteFileViaHttpClient {
    param(
        [string]$Url,
        [string]$OutputPath,
        [ValidateRange(0, 50)]
        [int]$MaxRedirects = 10
    )

    $client = $null
    $response = $null
    try {
        [Net.ServicePointManager]::SecurityProtocol = [Net.SecurityProtocolType]::Tls12
        Add-Type -AssemblyName System.Net.Http -ErrorAction SilentlyContinue
        $handler = New-Object System.Net.Http.HttpClientHandler
        # .NET Framework does not consistently auto-follow HTTP 308 responses.
        $handler.AllowAutoRedirect = $false
        $client = New-Object System.Net.Http.HttpClient($handler)
        $client.Timeout = [TimeSpan]::FromMinutes(30)

        $currentUri = New-Object System.Uri($Url, [System.UriKind]::Absolute)
        if ($currentUri.Scheme -notin @('http', 'https')) {
            throw "Unsupported download URI scheme '$($currentUri.Scheme)'."
        }
        $requireHttps = $currentUri.Scheme -eq 'https'

        $visitedUris = @{}
        for ($redirectCount = 0; ; $redirectCount++) {
            $uriKey = $currentUri.AbsoluteUri.ToLowerInvariant()
            if ($visitedUris.ContainsKey($uriKey)) {
                throw "HTTP redirect loop detected at '$currentUri'."
            }
            $visitedUris[$uriKey] = $true

            $response = $client.GetAsync(
                $currentUri,
                [System.Net.Http.HttpCompletionOption]::ResponseHeadersRead
            ).GetAwaiter().GetResult()

            $statusCode = [int]$response.StatusCode
            if ($statusCode -notin @(301, 302, 303, 307, 308)) {
                break
            }

            if ($redirectCount -ge $MaxRedirects) {
                throw "HTTP redirect limit of $MaxRedirects exceeded while downloading '$Url'."
            }

            $location = $response.Headers.Location
            if ($null -eq $location) {
                throw "HTTP $statusCode response from '$currentUri' did not include a Location header."
            }

            $nextUri = if ($location.IsAbsoluteUri) {
                $location
            } else {
                New-Object System.Uri($currentUri, $location)
            }
            if ($nextUri.Scheme -notin @('http', 'https')) {
                throw "HTTP redirect from '$currentUri' used unsupported URI scheme '$($nextUri.Scheme)'."
            }
            if ($requireHttps -and $nextUri.Scheme -ne 'https') {
                throw "Refusing to follow HTTPS to HTTP redirect from '$currentUri' to '$nextUri'."
            }

            .\Write-Info.ps1 "Following HTTP $statusCode redirect to: $nextUri" -ForegroundColor DarkGray
            $response.Dispose()
            $response = $null
            $currentUri = $nextUri
        }

        if (-not $response.IsSuccessStatusCode) {
            throw "HTTP $([int]$response.StatusCode) $($response.ReasonPhrase)"
        }

        $fs = [System.IO.File]::Create($OutputPath)
        try {
            [void]$response.Content.CopyToAsync($fs).GetAwaiter().GetResult()
        } finally {
            $fs.Dispose()
        }
        .\Write-Info.ps1 "Download completed successfully (HttpClient fallback)" -ForegroundColor Green
        return $true
    }
    catch {
        .\Write-Info.ps1 "HttpClient download failed: $($_.Exception.Message)" -ForegroundColor Red
        if (Test-Path $OutputPath) { Remove-Item -Path $OutputPath -Force -ErrorAction SilentlyContinue }
        return $false
    }
    finally {
        if ($response) { $response.Dispose() }
        if ($client) { $client.Dispose() }
    }
}

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

    $bitsOk = $false
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
                $bitsOk = $true
            }
            'Error' {
                .\Write-Info.ps1 "BITS download error: $($job.ErrorDescription)" -ForegroundColor Yellow
                $job | Remove-BitsTransfer
            }
            default {
                .\Write-Info.ps1 "BITS transfer ended with unexpected state: $($job.JobState)" -ForegroundColor Yellow
                $job | Remove-BitsTransfer
            }
        }
    }
    catch {
        # BITS threw (e.g. 0x800704DD when running without an interactive logon session).
        .\Write-Info.ps1 "BITS download failed: $($_.Exception.Message)" -ForegroundColor Yellow
        if (Test-Path $OutputPath) { Remove-Item -Path $OutputPath -Force -ErrorAction SilentlyContinue }
    }

    if ($bitsOk) { return $true }

    # Fall back to a logon-independent, redirect-following HttpClient download.
    .\Write-Info.ps1 "Falling back to HttpClient download (BITS unavailable in this session)..." -ForegroundColor Yellow
    return (Get-RemoteFileViaHttpClient -Url $Url -OutputPath $OutputPath)
}
