# Copyright (c) Microsoft. All rights reserved.
# Licensed under the MIT license. See LICENSE file in the project root for full license information.

param(
    $workingDir = $PSScriptRoot,
    $protocolConfigFile = "$workingDir\Config.json",
    $toolsPath = "$workingDir\Tools.json"
)

$scriptPath = Split-Path $MyInvocation.MyCommand.Definition -Parent
$env:Path += ";$scriptPath"

[string]$logFile = $MyInvocation.MyCommand.Path + ".log"
Start-Transcript -Path "$logFile" -Append -Force

# Load config files
try {
    $config = Get-Content -Path $protocolConfigFile -Raw | ConvertFrom-Json
}
catch {
    .\Write-Error.ps1 "Failed to parse config file: $_"
    Stop-Transcript; return $false
}

try {
    $tools = Get-Content -Path $toolsPath -Raw | ConvertFrom-Json
}
catch {
    .\Write-Error.ps1 "Failed to parse tools config file: $_"
    Stop-Transcript; return $false
}

# Resolve SUT machine
$sut = $config.Machines.PSObject.Properties |
    Where-Object { $_.Name -match "Sut|Node01" -or $_.Value.Role -match "SUT" } |
    Select-Object -First 1

if ($null -eq $sut) {
    .\Write-Error.ps1 "Failed to find SUT machine in config file."
    Stop-Transcript; return $false
}

$sutComputerName = $sut.Value.ComputerName

# Linux SUT: update hosts and exit early (ShareUtil doesn't support Linux yet)
if ($sut.Value.OS -eq "Linux") {
    $ip = $sut.Value.IpConfig[0].Ip
    "$ip $sutComputerName" | Out-File -FilePath "$env:windir\System32\drivers\etc\hosts" -Append -Encoding ascii
    .\Write-Info.ps1 "Linux SUT detected - ForceLevel2 not supported. Skipping."
    Stop-Transcript; return $true
}

# Locate ShareUtil.exe
$endPointPath = [System.Environment]::ExpandEnvironmentVariables($tools.DriverComputer.TestsuiteZips[0].targetFolder)
$ShareUtil = "$endPointPath\Utils\ShareUtil.exe"
if (-not (Test-Path $ShareUtil)) {
    .\Write-Error.ps1 "ShareUtil.exe not found at $ShareUtil"
    Stop-Transcript; return $false
}

# Determine if scaleout cluster share needs configuration (check once, not per-retry)
$scaleoutFSName = $null
$isDomain = -not [string]::IsNullOrEmpty($config.Core.DomainName) -and $config.Core.DomainName -ne "WORKGROUP"
if ($isDomain -and (Get-CimInstance Win32_ComputerSystem).PartOfDomain) {
    $scaleoutFSName = $config.Endpoints.ScaleoutFS.Name
    if (-not [string]::IsNullOrEmpty($scaleoutFSName) -and
        -not (Test-Connection -ComputerName $scaleoutFSName -Quiet -Count 1 -ErrorAction SilentlyContinue)) {
        .\Write-Info.ps1 "ScaleoutFS '$scaleoutFSName' is unreachable. Skipping clustered share." -ForegroundColor Yellow
        $scaleoutFSName = $null
    }
}

# Configure ForceLevel2 with retry
$maxRetries = 10
$success = $false
for ($i = 0; $i -lt $maxRetries; $i++) {
    .\Write-Info.ps1 "Configure forcelevel2 for share: ShareForceLevel2 (attempt $($i + 1)/$maxRetries)"
    CMD /C "$ShareUtil $sutComputerName ShareForceLevel2 SHI1005_FLAGS_FORCE_LEVELII_OPLOCK true" 2>&1 | .\Write-Info.ps1

    if ($LASTEXITCODE -eq 0) {
        $success = $true
        break
    }
    Start-Sleep 5
}

if (-not $success) {
    .\Write-Info.ps1 "Warning: ForceLevel2 configuration failed after $maxRetries retries. $sutComputerName may not be ready." -ForegroundColor Yellow
}

# Configure clustered share if applicable
if ($success -and -not [string]::IsNullOrEmpty($scaleoutFSName)) {
    .\Write-Info.ps1 "Configure forcelevel2 for share: SMBClusteredForceLevel2"
    CMD /C "$ShareUtil $scaleoutFSName SMBClusteredForceLevel2 SHI1005_FLAGS_FORCE_LEVELII_OPLOCK true" 2>&1 | .\Write-Info.ps1
}

Stop-Transcript
return $true