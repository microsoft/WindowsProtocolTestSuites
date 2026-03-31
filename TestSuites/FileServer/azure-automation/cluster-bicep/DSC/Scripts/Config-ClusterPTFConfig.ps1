# Copyright (c) Microsoft. All rights reserved.
# Licensed under the MIT license. See LICENSE file in the project root for full license information.

# Patches .deployment.ptfconfig files with cluster-specific values from Config.json
# so the File Server Protocol Test Suite recognizes the cluster environment.

param(
    $workingDir = $PSScriptRoot,
    $protocolConfigFile = "$workingDir\Config.json"
)

$scriptPath = Split-Path $MyInvocation.MyCommand.Definition -parent
$env:Path += ";$scriptPath"

.\Write-Info.ps1 "Configuring ptfconfig files for cluster support..." -ForegroundColor Yellow

# Load config
if (!(Test-Path $protocolConfigFile)) {
    .\Write-Info.ps1 "Config file not found at $protocolConfigFile. Skipping ptfconfig configuration." -ForegroundColor Yellow
    return
}

$config = $null
try {
    $config = Get-Content -Path $protocolConfigFile -Raw | ConvertFrom-Json
}
catch {
    .\Write-Info.ps1 "Failed to parse config file: $_. Skipping ptfconfig configuration." -ForegroundColor Yellow
    return
}

# Find the test suite bin directory
$testSuitePath = "$env:SystemDrive\FileServer-TestSuite-ServerEP\Bin"
if (!(Test-Path $testSuitePath)) {
    .\Write-Info.ps1 "Test suite not found at $testSuitePath. Skipping ptfconfig configuration." -ForegroundColor Yellow
    return
}

# Helper function to patch a ptfconfig property
function Set-PTFConfigProperty {
    param(
        [string]$FilePath,
        [string]$PropertyName,
        [string]$Value
    )
    if (!(Test-Path $FilePath)) { return }

    [xml]$xml = Get-Content $FilePath
    $node = $xml.GetElementsByTagName("Property") | Where-Object { $_.GetAttribute("name") -eq $PropertyName }
    if ($null -ne $node) {
        $oldValue = $node.GetAttribute("value")
        $node.SetAttribute("value", $Value)
        Set-ItemProperty -Path $FilePath -Name IsReadOnly -Value $false -ErrorAction SilentlyContinue
        $xml.Save((Resolve-Path $FilePath))
        .\Write-Info.ps1 "  $PropertyName`: $oldValue -> $Value"
    }
}

# Extract values from Config.json
$domain = $config.Core.DomainName
if ([string]::IsNullOrEmpty($domain)) { throw "Config.json Core.DomainName is required for cluster PTF configuration" }

$adminUser = $config.Core.Username
if ([string]::IsNullOrEmpty($adminUser)) { throw "Config.json Core.Username is required for cluster PTF configuration" }

$adminPassword = $config.Core.Password

# Find machine roles
$node01 = $config.Machines.PSObject.Properties | Where-Object { $_.Name -match "Node01" } | Select-Object -First 1
$node02 = $config.Machines.PSObject.Properties | Where-Object { $_.Name -match "Node02" } | Select-Object -First 1
$dc = $config.Machines.PSObject.Properties | Where-Object { $_.Name -match "DC" } | Select-Object -First 1
$driver = $config.Machines.PSObject.Properties | Where-Object { $_.Name -match "Driver" } | Select-Object -First 1

$sutName = if ($null -ne $node01) { $node01.Value.ComputerName } else { "Node01" }
$sutIp = if ($null -ne $node01 -and $null -ne $node01.Value.IpConfig) { $node01.Value.IpConfig[0].Ip } else { "" }
$sutAltIp = if ($null -ne $node01 -and $null -ne $node01.Value.IpConfig -and $node01.Value.IpConfig.Count -gt 1) { $node01.Value.IpConfig[1].Ip } else { "" }
$dcName = if ($null -ne $dc) { $dc.Value.ComputerName } else { "DC01" }
$driverName = if ($null -ne $driver) { $driver.Value.ComputerName } else { "Client01" }
$driverIp1 = if ($null -ne $driver -and $null -ne $driver.Value.IpConfig) { $driver.Value.IpConfig[0].Ip } else { "" }
$driverIp2 = if ($null -ne $driver -and $null -ne $driver.Value.IpConfig -and $driver.Value.IpConfig.Count -gt 1) { $driver.Value.IpConfig[1].Ip } else { "" }

# Endpoint names
$clusterName = if ($null -ne $config.Endpoints.Cluster) { $config.Endpoints.Cluster.Name } else { "Cluster01" }
$generalFSName = if ($null -ne $config.Endpoints.GeneralFS) { $config.Endpoints.GeneralFS.Name } else { "GeneralFS" }
$scaleoutFSName = if ($null -ne $config.Endpoints.ScaleoutFS) { $config.Endpoints.ScaleoutFS.Name } else { "ScaleoutFS" }
$infraFSName = if ($null -ne $config.Endpoints.InfrastructureFS) { $config.Endpoints.InfrastructureFS.Name } else { "InfraFS" }

# ============================================================
# Patch CommonTestSuite.deployment.ptfconfig
# ============================================================
$commonPtf = "$testSuitePath\CommonTestSuite.deployment.ptfconfig"
if (Test-Path $commonPtf) {
    .\Write-Info.ps1 "Patching CommonTestSuite.deployment.ptfconfig..." -ForegroundColor Cyan
    Set-PTFConfigProperty -FilePath $commonPtf -PropertyName "SutComputerName" -Value "$sutName.$domain"
    Set-PTFConfigProperty -FilePath $commonPtf -PropertyName "SutIPAddress" -Value $sutIp
    Set-PTFConfigProperty -FilePath $commonPtf -PropertyName "DomainName" -Value $domain
    Set-PTFConfigProperty -FilePath $commonPtf -PropertyName "DCServerComputerName" -Value "$dcName.$domain"
    Set-PTFConfigProperty -FilePath $commonPtf -PropertyName "AdminUserName" -Value $adminUser
    if (-not [string]::IsNullOrEmpty($adminPassword)) {
        Set-PTFConfigProperty -FilePath $commonPtf -PropertyName "PasswordForAllUsers" -Value $adminPassword
    }
    Set-PTFConfigProperty -FilePath $commonPtf -PropertyName "IsPersistentHandlesSupported" -Value "true"
    Set-PTFConfigProperty -FilePath $commonPtf -PropertyName "CAShareName" -Value "SMBClustered"
    Set-PTFConfigProperty -FilePath $commonPtf -PropertyName "CAShareServerName" -Value "$generalFSName.$domain"
    if (-not [string]::IsNullOrEmpty($driverIp1)) {
        Set-PTFConfigProperty -FilePath $commonPtf -PropertyName "ClientNic1IPAddress" -Value $driverIp1
    }
    if (-not [string]::IsNullOrEmpty($driverIp2)) {
        Set-PTFConfigProperty -FilePath $commonPtf -PropertyName "ClientNic2IPAddress" -Value $driverIp2
    }
}

# ============================================================
# Patch ServerFailoverTestSuite.deployment.ptfconfig
# ============================================================
$failoverPtf = "$testSuitePath\ServerFailoverTestSuite.deployment.ptfconfig"
if (Test-Path $failoverPtf) {
    .\Write-Info.ps1 "Patching ServerFailoverTestSuite.deployment.ptfconfig..." -ForegroundColor Cyan
    Set-PTFConfigProperty -FilePath $failoverPtf -PropertyName "ClusterName" -Value "$clusterName.$domain"
    Set-PTFConfigProperty -FilePath $failoverPtf -PropertyName "ClusterNode01" -Value "$sutName.$domain"
    $node02Name = if ($null -ne $node02) { $node02.Value.ComputerName } else { "Node02" }
    Set-PTFConfigProperty -FilePath $failoverPtf -PropertyName "ClusterNode02" -Value "$node02Name.$domain"
    Set-PTFConfigProperty -FilePath $failoverPtf -PropertyName "ClusteredFileServerName" -Value "$generalFSName.$domain"
    Set-PTFConfigProperty -FilePath $failoverPtf -PropertyName "ClusteredScaleOutFileServerName" -Value "$scaleoutFSName.$domain"
    Set-PTFConfigProperty -FilePath $failoverPtf -PropertyName "WitnessClientName" -Value "$driverName.$domain"
    Set-PTFConfigProperty -FilePath $failoverPtf -PropertyName "AsymmetricShare" -Value "SMBClustered"
    Set-PTFConfigProperty -FilePath $failoverPtf -PropertyName "CAShareWithDataEncryption" -Value "SMBClusteredEncrypted"
    Set-PTFConfigProperty -FilePath $failoverPtf -PropertyName "OptimumNodeOfAsymmetricShare" -Value "$scaleoutFSName.$domain"
    Set-PTFConfigProperty -FilePath $failoverPtf -PropertyName "NonOptimumNodeOfAsymmetricShare" -Value "$scaleoutFSName.$domain"
}

# ============================================================
# Patch MS-SMB2_ServerTestSuite.deployment.ptfconfig
# ============================================================
$smb2Ptf = "$testSuitePath\MS-SMB2_ServerTestSuite.deployment.ptfconfig"
if (Test-Path $smb2Ptf) {
    .\Write-Info.ps1 "Patching MS-SMB2_ServerTestSuite.deployment.ptfconfig..." -ForegroundColor Cyan
    Set-PTFConfigProperty -FilePath $smb2Ptf -PropertyName "ClusteredInfrastructureFileServerName" -Value "$infraFSName.$domain"
    Set-PTFConfigProperty -FilePath $smb2Ptf -PropertyName "DriverComputerName" -Value "$driverName.$domain"
    if (-not [string]::IsNullOrEmpty($sutAltIp)) {
        Set-PTFConfigProperty -FilePath $smb2Ptf -PropertyName "SutAlternativeIPAddress" -Value $sutAltIp
    }
}

# ============================================================
# Patch MS-SMB2Model_ServerTestSuite.deployment.ptfconfig
# ============================================================
$smb2ModelPtf = "$testSuitePath\MS-SMB2Model_ServerTestSuite.deployment.ptfconfig"
if (Test-Path $smb2ModelPtf) {
    .\Write-Info.ps1 "Patching MS-SMB2Model_ServerTestSuite.deployment.ptfconfig..." -ForegroundColor Cyan
    Set-PTFConfigProperty -FilePath $smb2ModelPtf -PropertyName "ScaleOutFileServerName" -Value "$scaleoutFSName.$domain"
}

.\Write-Info.ps1 "Cluster ptfconfig configuration complete." -ForegroundColor Green
