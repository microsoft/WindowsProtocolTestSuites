# Copyright (c) Microsoft. All rights reserved.
# Licensed under the MIT license. See LICENSE file in the project root for full license information.

# deploy.ps1
# Phased deployment script that waits for DC configuration before deploying domain-joined VMs
# Phase 1: Network, Domain Controller, Storage
# Phase 2: Cluster Nodes, Driver Computer (after DC is fully configured)

[Diagnostics.CodeAnalysis.SuppressMessageAttribute('PSAvoidUsingConvertToSecureStringWithPlainText', '',
    Justification = 'Only appears in a help/error-message here-string, not in executable code.')]
[CmdletBinding()]
param(
    [Parameter(Mandatory=$true)]
    [string]$SubscriptionId,

    [Parameter(Mandatory=$true)]
    [string]$ResourceGroupName,

    [Parameter(Mandatory=$true)]
    [SecureString]$AdminPassword,

    [Parameter(Mandatory=$false)]
    [string]$Phase1ParametersFile = "parameters/phase1.bicepparam",

    [Parameter(Mandatory=$false)]
    [string]$Phase2ParametersFile = "parameters/phase2.bicepparam",

    [Parameter(Mandatory=$false)]
    [string]$ClusterPackagePath = "",

    [Parameter(Mandatory=$false)]
    [string]$ClusterPackageZip = "",

    [Parameter(Mandatory=$false)]
    [string]$ClusterPackageZipUrl = "",

    [Parameter(Mandatory=$false)]
    [string]$StorageAccountName = "",

    [Parameter(Mandatory=$false)]
    [int]$DCReadyTimeoutMinutes = 45,

    [Parameter(Mandatory=$false)]
    [ValidateRange(1, 1440)]
    [int]$TestTimeoutMinutes = 360,

    [Parameter(Mandatory=$false)]
    [switch]$SkipPhase1,

    [Parameter(Mandatory=$false)]
    [switch]$SkipPhase2,

    [Parameter(Mandatory=$false)]
    [switch]$SkipDCReadyCheck,

    [Parameter(Mandatory=$false)]
    [switch]$ValidateOnly,

    [Parameter(Mandatory=$false)]
    [switch]$SkipDiskEncryption
)

$ErrorActionPreference = "Stop"
$operationStartUtc = [DateTime]::UtcNow
$phase1Deployment = $null

$Phase1ParametersFile = if ([System.IO.Path]::IsPathRooted($Phase1ParametersFile)) {
    $Phase1ParametersFile
} else {
    Join-Path $PSScriptRoot $Phase1ParametersFile
}
$Phase2ParametersFile = if ([System.IO.Path]::IsPathRooted($Phase2ParametersFile)) {
    $Phase2ParametersFile
} else {
    Join-Path $PSScriptRoot $Phase2ParametersFile
}

Write-Output @"
╔═══════════════════════════════════════════════════════════════╗
║   File Server Test Suite - Phased Cluster Deployment         ║
║   Phase 1: Network + DC + Storage                            ║
║   Phase 2: Nodes + Driver (after DC ready)                   ║
║                                                               ║
║   Resume Phase 2: -SkipPhase1                                ║
╚═══════════════════════════════════════════════════════════════╝
"@

if ($SkipPhase1) {
    Write-Output "`n⏩ Resuming from Phase 2 (Phase 1 skipped)"
}
if ($SkipPhase2) {
    Write-Output "`n⏩ Running Phase 1 only (Phase 2 skipped)"
}
if ($SkipPhase1 -and $SkipPhase2) {
    throw 'Both -SkipPhase1 and -SkipPhase2 were specified; there is nothing to deploy.'
}

# Import shared helpers
$helpersPath = Join-Path $PSScriptRoot "..\shared\Deploy-Helpers.psm1"
if (-not (Test-Path $helpersPath)) {
    Write-Error "❌ Shared helpers not found at: $helpersPath"
    exit 1
}
Import-Module $helpersPath -Force
Initialize-BicepCli

# Initialize Azure connection
Import-AzureModules
Connect-AzureSubscription -SubscriptionId $SubscriptionId

# Convert password securely
$plainPassword = ConvertFrom-SecurePassword -SecurePassword $AdminPassword

# Parse all parameters from bicepparam files (single source of truth for Config.json
# generation, custom-image validation, and Bicep deployment).
$phase1Params = ConvertFrom-BicepParam -Path $Phase1ParametersFile
$phase2Params = ConvertFrom-BicepParam -Path $Phase2ParametersFile

$allParams = @{} + $phase1Params
foreach ($key in $phase2Params.Keys) { $allParams[$key] = $phase2Params[$key] }
$config = Resolve-DeploymentConfig -Params $allParams -Defaults @{
    location           = 'West US 2'
    adminUsername      = 'testadmin'
    domainName         = 'contoso.com'
    domainNetBiosName  = 'CONTOSO'
    dcExternal1Ip      = '192.168.1.10'
    dcExternal2Ip      = '192.168.2.10'
    storageExternal1Ip = '192.168.1.12'
    node01External1Ip  = '192.168.1.21'
    node01External2Ip  = '192.168.2.21'
    node02External1Ip  = '192.168.1.22'
    node02External2Ip  = '192.168.2.22'
    driverExternal1Ip  = '192.168.1.111'
    driverExternal2Ip  = '192.168.2.111'
    driverOsType       = 'Windows'
    clusterName        = 'Cluster01'
    scaleOutFSName     = 'ScaleoutFS'
}

# Override enableDiskEncryption if -SkipDiskEncryption was specified
if ($SkipDiskEncryption) {
    $phase1Params['enableDiskEncryption'] = $false
}

$autoShutdownRequested = ($phase1Params['enableAutoShutdown'] -eq $true) -or
    ($phase2Params['enableAutoShutdown'] -eq $true)
$autoShutdownTime = if ($phase1Params['autoShutdownTime']) { $phase1Params['autoShutdownTime'] } else { '20:00' }
$autoShutdownTimeZone = if ($phase1Params['autoShutdownTimeZone']) { $phase1Params['autoShutdownTimeZone'] } else { 'UTC' }
$phase1Params['enableAutoShutdown'] = $false
$phase2Params['enableAutoShutdown'] = $false

Write-Output "   Location (from bicepparam): $($config.location)"

$phase1TemplateFile = Join-Path $PSScriptRoot 'phase1.bicep'
$phase2TemplateFile = Join-Path $PSScriptRoot 'phase2.bicep'

# Validate all role sizes before creating resources. The lightweight regional
# endpoint avoids the full resource-SKU feed's recurring 100-second timeout;
# ARM pre-validation remains authoritative for policy and dynamic capacity.
Write-Output "`nValidating VM sizes and OS images in $($config.location)..."
$vmSkus = @(Get-RegionalVmSkuSnapshot -Location $config.location)
$vmSizeFallbacks = Import-PowerShellDataFile (Join-Path $PSScriptRoot '..\shared\parameters\VmSizeFallbacks.psd1')

$dcCandidates = Resolve-AvailableVmSize `
    -PreferredSize $phase1Params['dcVmSize'] `
    -FallbackSizes $vmSizeFallbacks.DC -AvailableSkus $vmSkus -Role 'DC' -ReturnAll
$storageCandidates = Resolve-AvailableVmSize `
    -PreferredSize $phase1Params['storageVmSize'] `
    -FallbackSizes $vmSizeFallbacks.Driver -AvailableSkus $vmSkus -Role 'Storage' -ReturnAll
$nodeCandidates = Resolve-AvailableVmSize `
    -PreferredSize $phase2Params['clusterNodeVmSize'] `
    -FallbackSizes $vmSizeFallbacks.SUT -AvailableSkus $vmSkus -Role 'Cluster Node' -ReturnAll
$driverCandidates = Resolve-AvailableVmSize `
    -PreferredSize $phase2Params['driverVmSize'] `
    -FallbackSizes $vmSizeFallbacks.Driver -AvailableSkus $vmSkus -Role 'Driver' -ReturnAll

$phase1Params['dcVmSize'] = $dcCandidates[0]
$phase1Params['storageVmSize'] = $storageCandidates[0]
$phase2Params['clusterNodeVmSize'] = $nodeCandidates[0]
$phase2Params['driverVmSize'] = $driverCandidates[0]

Write-Output "   DC VM size: $($dcCandidates[0]) (+$([math]::Max(0, $dcCandidates.Count - 1)) fallbacks)"
Write-Output "   Storage VM size: $($storageCandidates[0]) (+$([math]::Max(0, $storageCandidates.Count - 1)) fallbacks)"
Write-Output "   Cluster Node VM size: $($nodeCandidates[0]) (+$([math]::Max(0, $nodeCandidates.Count - 1)) fallbacks)"
Write-Output "   Driver VM size: $($driverCandidates[0]) (+$([math]::Max(0, $driverCandidates.Count - 1)) fallbacks)"

Test-RegionalVCpuQuota -Location $config.location -AvailableSkus $vmSkus -VmSizes @{
    'DC' = $dcCandidates[0]
    'Storage' = $storageCandidates[0]
    'Node01' = $nodeCandidates[0]
    'Node02' = $nodeCandidates[0]
    'Driver' = $driverCandidates[0]
}

if (-not $phase1Params['dcCustomImageId'] -and
    -not (Test-VmImageAvailability -Location $config.location -Publisher 'MicrosoftWindowsServer' `
        -Offer 'WindowsServer' -Sku $phase1Params['dcOsVersion'])) {
    throw "DC image '$($phase1Params['dcOsVersion'])' is unavailable in $($config.location)."
}
if (-not $phase1Params['storageCustomImageId'] -and
    -not (Test-VmImageAvailability -Location $config.location -Publisher 'MicrosoftWindowsServer' `
        -Offer 'WindowsServer' -Sku $phase1Params['storageOsVersion'])) {
    throw "Storage image '$($phase1Params['storageOsVersion'])' is unavailable in $($config.location)."
}
if (-not $phase2Params['clusterNodeCustomImageId'] -and
    -not (Test-VmImageAvailability -Location $config.location -Publisher 'MicrosoftWindowsServer' `
        -Offer 'WindowsServer' -Sku $phase2Params['clusterNodeOsVersion'])) {
    throw "Cluster node image '$($phase2Params['clusterNodeOsVersion'])' is unavailable in $($config.location)."
}
if (-not $phase2Params['driverCustomImageId']) {
    if ($config.driverOsType -eq 'Linux') {
        $driverPublisher = 'Canonical'
        $driverOffer = 'ubuntu-24_04-lts'
        $driverSku = $phase2Params['driverLinuxOsVersion']
    } else {
        $driverPublisher = 'MicrosoftWindowsDesktop'
        $driverOffer = if ($phase2Params['driverOsVersion'] -like 'win10-*') { 'Windows-10' } else { 'Windows-11' }
        $driverSku = $phase2Params['driverOsVersion']
    }
    if (-not (Test-VmImageAvailability -Location $config.location -Publisher $driverPublisher `
        -Offer $driverOffer -Sku $driverSku)) {
        throw "Driver image '$driverPublisher/$driverOffer/$driverSku' is unavailable in $($config.location)."
    }
}

if ($ValidateOnly) {
    Write-Output "`nValidating Cluster Phase 1 template without creating or modifying Azure resources..."
    $existingResourceGroup = Get-AzResourceGroup -Name $ResourceGroupName -ErrorAction SilentlyContinue
    if ($existingResourceGroup) {
        $phase1ValidationParams = @{} + $phase1Params
        $phase1ValidationParams['adminPassword'] = $AdminPassword
        if ($ClusterPackageZipUrl) {
            $phase1ValidationParams['clusterPackageZipUrl'] = $ClusterPackageZipUrl
        }
        $validationResult = Test-AzResourceGroupDeployment `
            -ResourceGroupName $ResourceGroupName `
            -TemplateFile $phase1TemplateFile `
            -TemplateParameterObject $phase1ValidationParams `
            -ErrorAction SilentlyContinue
        if ($validationResult) {
            $nonCapacityErrors = $validationResult | Where-Object {
                $_.Code -notmatch 'SkuNotAvailable|ZonalAllocationFailed|AllocationFailed'
            }
            if ($nonCapacityErrors) {
                Write-Error "Phase 1 template validation failed:`n$($nonCapacityErrors | ForEach-Object { "  - [$($_.Code)] $($_.Message)" } | Out-String)"
                throw 'Bicep template validation failed. Fix the errors above before deploying.'
            }
            Write-Warning 'Template validation returned capacity warnings; deployment-time SKU fallback handles these.'
        } else {
            Write-Output '[OK] Phase 1 ARM template validation passed'
        }
    } else {
        Write-Output "Resource group '$ResourceGroupName' does not exist; ARM validation was skipped. Local Bicep, image, SKU, and quota checks passed."
    }
    Write-Output 'Validation-only mode: no resource group, storage account, package, or schedule was created or modified.'
    return
}

# Create or validate the resource group only after local/read-only preflight.
Initialize-ResourceGroup -ResourceGroupName $ResourceGroupName -Location $config.location
$envPrefix = if ($phase1Params['environmentPrefix']) { $phase1Params['environmentPrefix'] } else { 'fstest-cluster' }
$autoShutdownVmNames = @(
    "$envPrefix-dc01", "$envPrefix-storage01", "$envPrefix-node01",
    "$envPrefix-node02", "$envPrefix-client01"
)
$autoShutdownRestored = $false

# Validate custom images - non-driver VMs require Windows images
if ($phase1Params['dcCustomImageId']) {
    Write-Warning "dcCustomImageId is set to a custom image. Ensure it is a Windows-based image — the DC VM requires Windows."
}
if ($phase1Params['storageCustomImageId']) {
    Write-Warning "storageCustomImageId is set to a custom image. Ensure it is a Windows-based image — the Storage VM requires Windows."
}
if ($phase2Params['clusterNodeCustomImageId']) {
    Write-Warning "clusterNodeCustomImageId is set to a custom image. Ensure it is a Windows-based image — cluster node VMs require Windows."
}

# Handle Cluster-Package upload
Write-Output "`n📦 Preparing Cluster-Package..."
$actualClusterPackageZipUrl = $ClusterPackageZipUrl
$tempStorage = $null

# Default: build from local DSC folder if no explicit package/zip/url provided
$dscFolderPath = Join-Path $PSScriptRoot "DSC"
if (-not $ClusterPackagePath -and -not $ClusterPackageZip -and (Test-Path $dscFolderPath)) {
    $ClusterPackagePath = $dscFolderPath
}

if (-not $ClusterPackageZipUrl -and ((Test-Path $ClusterPackagePath) -or (Test-Path $ClusterPackageZip))) {
    Write-Output "📦 No clusterPackageZipUrl provided. Setting up storage for upload..."

    $tempStorage = Get-OrCreateStorageAccount `
        -ResourceGroupName $ResourceGroupName -Location $config.location `
        -StorageAccountName $StorageAccountName -ContainerName "packages"

    $ctx = $tempStorage.Context
    $containerName = $tempStorage.ContainerName

    # Build Generate-ConfigJson parameters from the resolved config object
    $configParams = @{
        Scenario           = "Cluster"
        AdminUsername      = $config.adminUsername
        AdminPassword      = $plainPassword
        DomainName         = $config.domainName
        DomainNetBiosName  = $config.domainNetBiosName
        DCExternal1Ip      = $config.dcExternal1Ip
        DCExternal2Ip      = $config.dcExternal2Ip
        Node01External1Ip  = $config.node01External1Ip
        Node01External2Ip  = $config.node01External2Ip
        Node02External1Ip  = $config.node02External1Ip
        Node02External2Ip  = $config.node02External2Ip
        StorageExternal1Ip = $config.storageExternal1Ip
        DriverExternal1Ip  = $config.driverExternal1Ip
        DriverExternal2Ip  = $config.driverExternal2Ip
        DriverOSType       = $config.driverOsType
        ClusterName        = $config.clusterName
        ScaleOutFSName     = $config.scaleOutFSName
        # Create every test account with the single admin password so secondary
        # accounts match the framework's PasswordForAllUsers (works for any chosen
        # password; a no-op when the admin password already matches ParamConfig).
        UnifyAccountPasswords = $true
    }

    if (Test-Path $ClusterPackageZip) {
        Write-Output "`n📦 Found existing Cluster-Package.zip"
        Write-Output "   Extracting, updating Config.json, and re-packaging..."

        $tempExtractPath = Join-Path $env:TEMP "ClusterPackage-Extract-$(Get-Random)"
        Expand-Archive -Path $ClusterPackageZip -DestinationPath $tempExtractPath -Force
        Write-Output "   ✅ Extracted to: $tempExtractPath"

        $configParams['OutputPath'] = Join-Path $tempExtractPath "Config.json"
        & "$PSScriptRoot\..\shared\Generate-ConfigJson.ps1" @configParams
        Write-Output "   ✅ Config.json updated"

        # Generate ResultsUpload.json for test results upload
        $resultsConfig = New-ResultsUploadConfig `
            -StorageAccountName $tempStorage.Name -StorageContext $ctx
        $resultsConfig | ConvertTo-Json -Depth 3 | Set-Content -Path (Join-Path $tempExtractPath "ResultsUpload.json") -Force
        Write-Output "   ✅ ResultsUpload.json generated"

        $tempZipPath = Join-Path $env:TEMP "Cluster-Package-$(Get-Random).zip"
        Compress-Archive -Path (Join-Path $tempExtractPath "*") -DestinationPath $tempZipPath -Force
        Write-Output "   ✅ Created new zip: $tempZipPath"

        $actualClusterPackageZipUrl = Send-BlobWithSasUrl `
            -FilePath $tempZipPath -BlobName "Cluster-Package.zip" `
            -ContainerName $containerName -StorageContext $ctx

        Remove-Item $tempExtractPath -Recurse -Force
        Remove-Item $tempZipPath -Force

    } elseif (Test-Path $ClusterPackagePath) {
        Write-Output "`n📦 Building Cluster-Package from: $ClusterPackagePath"

        $actualClusterPackageZipUrl = Build-DscPackage `
            -DscFolderPath $ClusterPackagePath `
            -SharedDscPath (Join-Path $PSScriptRoot "..\shared\DSC") `
            -Scenario 'Cluster' `
            -BlobName 'Cluster-Package.zip' `
            -ConfigJsonParams $configParams `
            -GenerateConfigScript (Join-Path $PSScriptRoot "..\shared\Generate-ConfigJson.ps1") `
            -StorageContext $ctx `
            -ContainerName $containerName `
            -StorageAccountName $tempStorage.Name `
            -LocalGpoBackupPath (Join-Path $PSScriptRoot "..\..\Setup\Scripts\GPOBackup.zip")
    }
} else {
    Write-Output "✅ Using provided clusterPackageZipUrl"
}

# ═══════════════════════════════════════════════════════════
# Bicep template validation (dry run)
# ═══════════════════════════════════════════════════════════
Write-Output "Validating Bicep templates..."

$phase1ValidationParams = @{} + $phase1Params
$phase1ValidationParams['adminPassword'] = $AdminPassword
if ($actualClusterPackageZipUrl) {
    $phase1ValidationParams['clusterPackageZipUrl'] = $actualClusterPackageZipUrl
}

$validationResult = Test-AzResourceGroupDeployment `
    -ResourceGroupName $ResourceGroupName `
    -TemplateFile $phase1TemplateFile `
    -TemplateParameterObject $phase1ValidationParams `
    -ErrorAction SilentlyContinue

if ($validationResult) {
    $nonCapacityErrors = $validationResult | Where-Object {
        $_.Code -notmatch 'SkuNotAvailable|ZonalAllocationFailed|AllocationFailed'
    }
    if ($nonCapacityErrors) {
        Write-Error "Phase 1 template validation failed:`n$($nonCapacityErrors | ForEach-Object { "  - [$($_.Code)] $($_.Message)" } | Out-String)"
        throw "Bicep template validation failed. Fix the errors above before deploying."
    }
    Write-Warning "Template validation returned capacity warnings (SkuNotAvailable) — the deployment retry loop will handle these."
} else {
    Write-Output "[OK] Phase 1 template validation passed"
}

# ═══════════════════════════════════════════════════════════
# PHASE 1: Deploy Network, Domain Controller, Storage
# ═══════════════════════════════════════════════════════════

try {

if ($autoShutdownRequested) {
    Remove-VmAutoShutdownSchedules -ResourceGroupName $ResourceGroupName `
        -VMNames $autoShutdownVmNames
}

if (-not $SkipPhase1) {
    Write-Output @"

╔═══════════════════════════════════════════════════════════════╗
║   PHASE 1: Deploying Infrastructure & Domain Services        ║
║   - Virtual Network + Bastion                                ║
║   - Domain Controller (DC01)                                 ║
║   - Storage Server (Storage01)                               ║
╚═══════════════════════════════════════════════════════════════╝
"@

    $phase1Start = Get-Date
    $phase1TemplateParams = @{} + $phase1Params
    $phase1TemplateParams.Remove('dcVmSize')
    $phase1TemplateParams.Remove('storageVmSize')
    $phase1TemplateParams['adminPassword'] = $AdminPassword
    if ($actualClusterPackageZipUrl) {
        $phase1TemplateParams['clusterPackageZipUrl'] = $actualClusterPackageZipUrl
    }

    Write-Output "🚀 Starting Phase 1 deployment..."
    $deployment = Invoke-DeploymentWithSkuFallback `
        -ResourceGroupName $ResourceGroupName -TemplateFile $phase1TemplateFile `
        -BaseParameters $phase1TemplateParams `
        -SizeCandidates @{ dcVmSize = $dcCandidates; storageVmSize = $storageCandidates } `
        -DeploymentNamePrefix 'Cluster-Phase1'
    $phase1Deployment = $deployment

    $phase1Duration = [math]::Round(((Get-Date) - $phase1Start).TotalMinutes, 1)
    Write-Output "`n✅ Phase 1 deployment completed in $phase1Duration minutes"

    # ═══════════════════════════════════════════════════════════
    # Wait for Domain Controller to be fully configured
    # ═══════════════════════════════════════════════════════════

    Write-Output @"

╔═══════════════════════════════════════════════════════════════╗
║   Waiting for Domain Controller Configuration                ║
║   DC must be fully configured before nodes can join domain   ║
╚═══════════════════════════════════════════════════════════════╝
"@

    $dcReady = Wait-ForDomainController `
        -ResourceGroupName $ResourceGroupName `
        -VMNamePattern "*-dc01" `
        -CheckScript "Test-Path 'C:\Cluster-Package\DSC\Deploy-DC.Completed.signal'" `
        -TimeoutMinutes $DCReadyTimeoutMinutes `
        -PollIntervalSeconds 30

    if (-not $dcReady) {
        Write-Output @"

⚠️  Domain Controller did not signal readiness within the timeout.

Options:
1. Increase timeout with -DCReadyTimeoutMinutes parameter
2. Connect to DC via Bastion and check: C:\dc-extension-setup.log
3. Resume Phase 2 later once DC is ready:

   .\deploy.ps1 ``
       -SubscriptionId '$SubscriptionId' ``
       -ResourceGroupName '$ResourceGroupName' ``
       -AdminPassword (ConvertTo-SecureString '<password>' -AsPlainText -Force) ``
       -SkipPhase1$(if ($actualClusterPackageZipUrl) { " ``
    -ClusterPackageZipUrl '<signed-package-url>'" })
"@
        exit 1
    }

}

# ═══════════════════════════════════════════════════════════
# PHASE 2: Deploy Cluster Nodes and Driver Computer
# ═══════════════════════════════════════════════════════════

if (-not $SkipPhase2) {

    # ═══════════════════════════════════════════════════════════
    # Verify DC readiness when resuming (Phase 1 was skipped)
    # ═══════════════════════════════════════════════════════════
    if ($SkipPhase1 -and -not $SkipDCReadyCheck) {
        Write-Output @"

╔═══════════════════════════════════════════════════════════════╗
║   Verifying Domain Controller Readiness                      ║
║   Checking DC configuration before deploying Phase 2 VMs     ║
╚═══════════════════════════════════════════════════════════════╝
"@

        $dcReady = Wait-ForDomainController `
            -ResourceGroupName $ResourceGroupName `
            -VMNamePattern "*-dc01" `
            -CheckScript "Test-Path 'C:\Cluster-Package\DSC\Deploy-DC.Completed.signal'" `
            -TimeoutMinutes 5 `
            -PollIntervalSeconds 15

        if (-not $dcReady) {
            Write-Output @"

⚠️  Domain Controller does not appear to be ready.
If you are sure the DC is configured, re-run with -SkipDCReadyCheck.
Otherwise, connect to DC via Bastion and check: C:\dc-extension-setup.log
"@
            exit 1
        }
        Write-Output "✅ Domain Controller is ready"
    }

    # Clean up identities only for VMs that no longer exist. Removing an object
    # for a surviving domain member immediately breaks its secure channel. Live
    # cluster endpoint identities must also remain while either node survives.
    if ($SkipPhase1) {
        $envPrefix = if ($phase1Params['environmentPrefix']) { $phase1Params['environmentPrefix'] } else { 'fstest-cluster' }
        $dcVmName = "$envPrefix-dc01"
        $memberDefinitions = @(
            [pscustomobject]@{ VMName = "$envPrefix-client01"; ComputerName = 'Client01'; IsClusterNode = $false }
            [pscustomobject]@{ VMName = "$envPrefix-node01"; ComputerName = 'Node01'; IsClusterNode = $true }
            [pscustomobject]@{ VMName = "$envPrefix-node02"; ComputerName = 'Node02'; IsClusterNode = $true }
            [pscustomobject]@{ VMName = "$envPrefix-storage01"; ComputerName = 'Storage01'; IsClusterNode = $false }
        )
        $existingMemberVmNames = @(Invoke-AzureOperationWithRetry `
            -OperationName 'List existing Cluster member VMs' `
            -Operation { @(Get-AzVM -ResourceGroupName $ResourceGroupName -ErrorAction Stop).Name })
        $missingComputerNames = @($memberDefinitions |
            Where-Object { $_.VMName -notin $existingMemberVmNames } |
            ForEach-Object { $_.ComputerName })
        $survivingClusterNodeCount = @($memberDefinitions |
            Where-Object { $_.IsClusterNode -and $_.VMName -in $existingMemberVmNames }).Count
        $cleanupClusterEndpoints = $survivingClusterNodeCount -eq 0

        if ($missingComputerNames.Count -eq 0 -and -not $cleanupClusterEndpoints) {
            Write-Output "`n[OK] Cluster VMs already exist; preserving member and endpoint computer accounts."
        } else {
            Write-Output "`nCleaning stale identities for missing Cluster VMs: $($missingComputerNames -join ', ')..."
            $missingComputerNamesLiteral = @($missingComputerNames |
                ForEach-Object { "'$(($_ -replace "'", "''"))'" }) -join ', '
            $cleanupEndpointsLiteral = if ($cleanupClusterEndpoints) { '$true' } else { '$false' }
            $cleanupScript = @"
Import-Module ActiveDirectory -ErrorAction Stop

`$staleNames = @($missingComputerNamesLiteral)
`$cleanupClusterEndpoints = $cleanupEndpointsLiteral
`$configPaths = @(
    'C:\Cluster-Package\Config.json',
    'C:\Cluster-Package\DSC\Scripts\Config.json'
)
`$config = `$null
foreach (`$p in `$configPaths) {
    if (Test-Path `$p) {
        `$config = Get-Content `$p -Raw | ConvertFrom-Json
        break
    }
}

if (`$cleanupClusterEndpoints -and `$config) {
    if (`$config.Endpoints) {
        foreach (`$prop in `$config.Endpoints.PSObject.Properties) {
            if (`$prop.Value.Name) { `$staleNames += `$prop.Value.Name }
        }
    }
}
`$staleNames = @(`$staleNames | Select-Object -Unique)

Write-Output "Cleaning up stale accounts for: `$(`$staleNames -join ', ')"
`$cleaned = @()
foreach (`$name in `$staleNames) {
    `$acct = Get-ADComputer -Filter "Name -eq '`$name'" -ErrorAction SilentlyContinue
    if (`$acct) {
        `$acct | Remove-ADObject -Recursive -Confirm:`$false
        `$cleaned += `$name
    }
}
if (`$cleaned.Count -gt 0) {
    Write-Output "Removed stale computer accounts: `$(`$cleaned -join ', ')"
} else {
    Write-Output "No stale computer accounts found."
}

# Also remove stale DNS records for cluster endpoints
`$zoneName = (Get-ADDomain).DNSRoot
foreach (`$name in `$staleNames) {
    try {
        `$records = Get-DnsServerResourceRecord -ZoneName `$zoneName -Name `$name -ErrorAction SilentlyContinue
        if (`$records) {
            `$records | Remove-DnsServerResourceRecord -ZoneName `$zoneName -Name `$name -Force -ErrorAction SilentlyContinue
            Write-Output "Removed DNS records for `$name"
        }
    } catch { }
}
"@

            $cleanupJob = $null
            try {
                $cleanupJob = Invoke-AzVMRunCommand -ResourceGroupName $ResourceGroupName `
                    -VMName $dcVmName -CommandId 'RunPowerShellScript' `
                    -ScriptString $cleanupScript -AsJob -ErrorAction Stop
                $completedCleanup = Wait-Job -Job $cleanupJob -Timeout 180
                if ($null -eq $completedCleanup) {
                    Stop-Job -Job $cleanupJob -ErrorAction SilentlyContinue
                    throw 'Stale account cleanup exceeded 180 seconds.'
                }
                if ($cleanupJob.State -ne 'Completed') {
                    throw "Stale account cleanup ended in state '$($cleanupJob.State)'."
                }
                $result = $cleanupJob | Receive-Job -ErrorAction Stop
                if ($result.Value) {
                    foreach ($v in $result.Value) {
                        if ($v.Message) {
                            $v.Message -split "`n" | ForEach-Object { Write-Output "   $_" }
                        }
                    }
                }
            } catch {
                Write-Warning "Could not clean up stale computer accounts on DC: $($_.Exception.Message)"
                Write-Warning "If domain join fails, manually remove only identities for missing VMs."
            } finally {
                if ($cleanupJob) {
                    Remove-Job -Job $cleanupJob -Force -ErrorAction SilentlyContinue
                }
            }
        }
    }

    Write-Output @"

╔═══════════════════════════════════════════════════════════════╗
║   PHASE 2: Deploying Domain-Joined VMs                       ║
║   - Cluster Node01                                           ║
║   - Cluster Node02                                           ║
║   - Driver Computer (Client01)                               ║
╚═══════════════════════════════════════════════════════════════╝
"@

    $phase2Start = Get-Date
    # Get Phase 1 outputs
    if (-not $phase1Deployment) {
        $phase1Deployment = Get-AzResourceGroupDeployment -ResourceGroupName $ResourceGroupName |
            Where-Object { $_.DeploymentName -like 'Cluster-Phase1-*' -or $_.DeploymentName -like 'Phase1-*' } |
            Sort-Object Timestamp -Descending |
            Select-Object -First 1
    }

    if (-not $phase1Deployment) {
        Write-Error "❌ Phase 1 deployment not found in resource group '$ResourceGroupName'. Run Phase 1 first."
        exit 1
    }

    Write-Output "📋 Retrieved Phase 1 outputs from deployment: $($phase1Deployment.DeploymentName)"
    $external1SubnetId = $phase1Deployment.Outputs.external1SubnetId.Value
    $external2SubnetId = $phase1Deployment.Outputs.external2SubnetId.Value
    $dcExternal1Ip = $phase1Deployment.Outputs.dcExternal1Ip.Value
    $domainName = $phase1Deployment.Outputs.domainName.Value
    $domainNetBiosName = $phase1Deployment.Outputs.domainNetBiosName.Value
    Write-Output "   External1 Subnet: $external1SubnetId"
    Write-Output "   External2 Subnet: $external2SubnetId"
    Write-Output "   DC IP: $dcExternal1Ip"
    Write-Output "   Domain: $domainName ($domainNetBiosName)"

    # When resuming without a local package, try to reuse existing blob from storage account
    if ($SkipPhase1 -and -not $actualClusterPackageZipUrl) {
        Write-Output "`n🔍 Looking for previously uploaded Cluster-Package in storage..."
        $storageAccounts = @(Invoke-AzureOperationWithRetry `
            -OperationName 'Find Cluster package storage accounts' `
            -Operation { Get-AzStorageAccount -ResourceGroupName $ResourceGroupName -ErrorAction Stop }) |
            Where-Object {
                if ($StorageAccountName) {
                    $_.StorageAccountName -eq $StorageAccountName
                } else {
                    $_.StorageAccountName -like 'fststorage*'
                }
            }
        $packageCandidates = New-Object System.Collections.Generic.List[object]
        foreach ($sa in $storageAccounts) {
            try {
                $saCtx = $sa.Context
                $blob = @(Invoke-AzureOperationWithRetry `
                    -OperationName "Find Cluster package in '$($sa.StorageAccountName)'" `
                    -Operation { Get-AzStorageBlob -Container 'packages' -Context $saCtx -ErrorAction Stop }) |
                    Where-Object { $_.Name -eq 'Cluster-Package.zip' } |
                    Select-Object -First 1
                if ($blob) {
                    $lastModified = if ($blob.LastModified) {
                        [DateTimeOffset]$blob.LastModified
                    } elseif ($blob.ICloudBlob -and $blob.ICloudBlob.Properties.LastModified) {
                        [DateTimeOffset]$blob.ICloudBlob.Properties.LastModified
                    } else {
                        $null
                    }
                    $packageCandidates.Add([pscustomobject]@{
                        StorageAccountName = $sa.StorageAccountName
                        Context = $saCtx
                        BlobName = $blob.Name
                        LastModified = $lastModified
                    }) | Out-Null
                }
            } catch {
                Write-Warning "Skipping storage account '$($sa.StorageAccountName)': $($_.Exception.Message)"
            }
        }
        if ($packageCandidates.Count -eq 0) {
            throw 'No reusable Cluster-Package.zip was found. Supply -ClusterPackageZipUrl, -ClusterPackageZip, or -ClusterPackagePath.'
        }
        if (-not $StorageAccountName -and $packageCandidates.Count -gt 1) {
            $candidateSummary = @($packageCandidates |
                Sort-Object LastModified -Descending |
                ForEach-Object {
                    $timestamp = if ($_.LastModified) {
                        $_.LastModified.UtcDateTime.ToString('u')
                    } else {
                        'unknown'
                    }
                    "$($_.StorageAccountName)/$($_.BlobName) (LastModified: $timestamp)"
                }) -join '; '
            throw "Multiple reusable Cluster-Package.zip blobs were found. Provide -StorageAccountName or -ClusterPackageZipUrl to select one explicitly. Candidates: $candidateSummary"
        }
        $selectedPackage = $packageCandidates |
            Sort-Object LastModified -Descending |
            Select-Object -First 1
        $actualClusterPackageZipUrl = New-AzStorageBlobSASToken `
            -Container 'packages' -Blob $selectedPackage.BlobName `
            -Permission r -ExpiryTime (Get-Date).AddHours(2) `
            -Context $selectedPackage.Context -FullUri
        Write-Output "   ✅ Using package blob: $($selectedPackage.BlobName) in $($selectedPackage.StorageAccountName)"
    }

    try {
        $phase2TemplateParams = @{} + $phase2Params
        $phase2TemplateParams.Remove('clusterNodeVmSize')
        $phase2TemplateParams.Remove('driverVmSize')
        $phase2TemplateParams['adminPassword'] = $AdminPassword
        $phase2TemplateParams['external1SubnetId'] = $external1SubnetId
        $phase2TemplateParams['external2SubnetId'] = $external2SubnetId
        $phase2TemplateParams['dcExternal1Ip'] = $dcExternal1Ip
        if ($actualClusterPackageZipUrl) {
            $phase2TemplateParams['clusterPackageZipUrl'] = $actualClusterPackageZipUrl
        }

        Write-Output "`n🚀 Starting Phase 2 deployment..."
        $deployment = Invoke-DeploymentWithSkuFallback `
            -ResourceGroupName $ResourceGroupName -TemplateFile $phase2TemplateFile `
            -BaseParameters $phase2TemplateParams `
            -SizeCandidates @{ clusterNodeVmSize = $nodeCandidates; driverVmSize = $driverCandidates } `
            -DeploymentNamePrefix 'Cluster-Phase2'

        $phase2Duration = [math]::Round(((Get-Date) - $phase2Start).TotalMinutes, 1)
        Write-Output "`n✅ Phase 2 deployment completed in $phase2Duration minutes"
    } catch {
        throw
    }

}

# ═══════════════════════════════════════════════════════════
# Final Verification
# ═══════════════════════════════════════════════════════════

Write-Output @"

╔═══════════════════════════════════════════════════════════════╗
║   Verifying Complete Cluster Configuration                   ║
╚═══════════════════════════════════════════════════════════════╝
"@

Write-Output "`n🔍 Running final verification..."
$envPrefix = if ($phase1Params['environmentPrefix']) { $phase1Params['environmentPrefix'] } else { 'fstest-cluster' }
$resultsStorageAccountName = if ($tempStorage -and $tempStorage.Name) {
    $tempStorage.Name
} else {
    $StorageAccountName
}
if ($SkipPhase2) {
    $expectedRoles = @('Domain Controller', 'Storage Server')
    $verifiedVmNames = @("$envPrefix-dc01", "$envPrefix-storage01")
    & "$PSScriptRoot\scripts\Verify-ClusterDeployment.ps1" `
        -SubscriptionId $SubscriptionId -ResourceGroupName $ResourceGroupName `
        -ExpectedRoles $expectedRoles -TimeoutMinutes 120 -NotBeforeUtc $operationStartUtc | Out-Null
} else {
    $verifiedVmNames = @(
        "$envPrefix-dc01",
        "$envPrefix-storage01",
        "$envPrefix-node01",
        "$envPrefix-node02",
        "$envPrefix-client01"
    )
    if ($SkipPhase1) {
        & "$PSScriptRoot\scripts\Verify-ClusterDeployment.ps1" `
            -SubscriptionId $SubscriptionId -ResourceGroupName $ResourceGroupName `
            -ExpectedRoles @('Domain Controller', 'Storage Server') -TimeoutMinutes 10 | Out-Null
        $verification = & "$PSScriptRoot\scripts\Verify-ClusterDeployment.ps1" `
            -SubscriptionId $SubscriptionId -ResourceGroupName $ResourceGroupName `
            -ExpectedRoles @('Cluster Node 1', 'Cluster Node 2', 'Driver Computer') `
            -TimeoutMinutes 120 -TestTimeoutMinutes $TestTimeoutMinutes `
            -NotBeforeUtc $operationStartUtc -WaitForTests -DeferTestFailure `
            -ResultsStorageAccountName $resultsStorageAccountName
    } else {
        $verification = & "$PSScriptRoot\scripts\Verify-ClusterDeployment.ps1" `
            -SubscriptionId $SubscriptionId -ResourceGroupName $ResourceGroupName `
            -TimeoutMinutes 120 -TestTimeoutMinutes $TestTimeoutMinutes `
            -NotBeforeUtc $operationStartUtc -WaitForTests -DeferTestFailure `
            -ResultsStorageAccountName $resultsStorageAccountName
    }
    Connect-AzureSubscription -SubscriptionId $SubscriptionId | Out-Host
}

# ADE may reboot every VM, so apply it only after cluster configuration and
# automatic tests have reached verified terminal states.
if (-not $SkipDiskEncryption) {
    if (-not $phase1Deployment) {
        $phase1Deployment = Get-AzResourceGroupDeployment -ResourceGroupName $ResourceGroupName |
            Where-Object { $_.DeploymentName -like 'Cluster-Phase1-*' -or $_.DeploymentName -like 'Phase1-*' } |
            Sort-Object Timestamp -Descending |
            Select-Object -First 1
    }
    if (-not $phase1Deployment) {
        throw 'Phase 1 deployment outputs are unavailable for disk encryption.'
    }

    Invoke-DiskEncryptionForVMs -ResourceGroupName $ResourceGroupName `
        -DeploymentOutputs $phase1Deployment.Outputs `
        -VMNames $verifiedVmNames

    $postEncryptionParams = @{
        SubscriptionId = $SubscriptionId
        ResourceGroupName = $ResourceGroupName
        TimeoutMinutes = 20
    }
    if ($expectedRoles) { $postEncryptionParams['ExpectedRoles'] = $expectedRoles }
    & "$PSScriptRoot\scripts\Verify-ClusterDeployment.ps1" @postEncryptionParams | Out-Null
}

if ($autoShutdownRequested) {
    Connect-AzureSubscription -SubscriptionId $SubscriptionId | Out-Host
    Enable-VmAutoShutdownSchedules -ResourceGroupName $ResourceGroupName `
        -Location $config.location -VMNames $verifiedVmNames `
        -Time $autoShutdownTime -TimeZone $autoShutdownTimeZone
    $autoShutdownRestored = $true
}

if (-not $SkipPhase2) {
    Complete-DeploymentTestOutcome -Verification $verification
}

Write-Output @"

╔═══════════════════════════════════════════════════════════════╗
║   DEPLOYMENT COMPLETE                                         ║
╚═══════════════════════════════════════════════════════════════╝

🎉 Your failover cluster environment is ready!

All VMs are configured automatically. Tests will run via scheduled task on Client01.

Monitor progress:
1. Connect to Client01 via Azure Bastion
2. Check task: Get-ScheduledTask -TaskName 'RunFileServerTests'
3. Logs: C:\Cluster-Package\DSC\Deploy-Driver.log, Invoke-TestRun.log
4. Results: C:\Test\TestResults\*.trx

For detailed instructions, see: README.md
"@

} finally {
    if ($autoShutdownRequested -and -not $autoShutdownRestored) {
        try {
            Connect-AzureSubscription -SubscriptionId $SubscriptionId | Out-Host
            Enable-VmAutoShutdownSchedules -ResourceGroupName $ResourceGroupName `
                -Location $config.location -VMNames $autoShutdownVmNames `
                -Time $autoShutdownTime -TimeZone $autoShutdownTimeZone
            $autoShutdownRestored = $true
        } catch {
            Write-Warning "Failed to restore VM auto-shutdown schedules (non-fatal): $($_.Exception.Message)"
        }
    }
    # Storage account is kept alive for test results upload.
    if ($tempStorage) {
        Write-Output "`nStorage account '$($tempStorage.Name)' retained for test results upload."
        if ($tempStorage.IsTemporary) {
            Write-Output "   To clean up later: Remove-AzStorageAccount -ResourceGroupName '$ResourceGroupName' -Name '$($tempStorage.Name)' -Force"
        }
    }
    # Clear sensitive variables from memory
    $plainPassword = $null
}
