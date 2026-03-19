# Copyright (c) Microsoft. All rights reserved.
# Licensed under the MIT license. See LICENSE file in the project root for full license information.

# deploy.ps1
# Phased deployment script for File Server Test Suite - Domain Environment
# Phase 1: Network + Domain Controller (DC01)
# Phase 2: Driver Computer (Client01) + SUT (Node01) - after DC is fully configured

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
    [string]$DscFolderPath = "DSC",

    [Parameter(Mandatory=$false)]
    [string]$DscPackageZipUrl = "",

    [Parameter(Mandatory=$false)]
    [string]$StorageAccountName = "",

    [Parameter(Mandatory=$false)]
    [int]$DCReadyTimeoutMinutes = 45,

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

# Resolve paths relative to script directory
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

  File Server Test Suite - Phased Domain Deployment
  Phase 1: Network + Domain Controller (DC01)
  Phase 2: Driver (Client01) + SUT (Node01) after DC ready

  Resume Phase 2: -SkipPhase1

"@

if ($SkipPhase1 -and $SkipPhase2) {
    Write-Error "Both -SkipPhase1 and -SkipPhase2 specified. Nothing to deploy."
    exit 1
}
if ($SkipPhase1) {
    Write-Output "Resuming from Phase 2 (Phase 1 skipped)`n"
}
if ($SkipPhase2) {
    Write-Output "Running Phase 1 only (Phase 2 skipped)`n"
}

# Import shared helpers
$helpersPath = Join-Path $PSScriptRoot "..\shared\Deploy-Helpers.psm1"
if (-not (Test-Path $helpersPath)) {
    Write-Error "Shared helpers not found at: $helpersPath"
    exit 1
}
Import-Module $helpersPath -Force

# Initialize Azure connection
Import-AzureModules
Connect-AzureSubscription -SubscriptionId $SubscriptionId

# Ensure Bicep CLI is on PATH (required for New-AzResourceGroupDeployment with .bicep files)
if (-not (Get-Command bicep -ErrorAction SilentlyContinue)) {
    Write-Output "Bicep CLI not found on PATH. Installing via Azure CLI..."
    az bicep install
    if ($LASTEXITCODE -ne 0) {
        Write-Error "Failed to install Bicep CLI. Please install manually: https://aka.ms/bicep-install"
        exit 1
    }
    # az bicep install places the binary in ~/.azure/bin — add it to PATH for this session
    $azBicepDir = Join-Path $env:USERPROFILE '.azure\bin'
    if (Test-Path (Join-Path $azBicepDir 'bicep.exe')) {
        $env:PATH = "$azBicepDir;$env:PATH"
        Write-Output "✅ Bicep CLI installed and added to PATH"
    } else {
        Write-Warning "Bicep installed via az, but binary not found at '$azBicepDir'. You may need to add it to PATH manually."
    }
}

# Convert password securely
$plainPassword = ConvertFrom-SecurePassword -SecurePassword $AdminPassword

# Parse all parameters from bicepparam files (single source of truth for Config.json
# generation, pre-flight validation, and Bicep deployment).
$phase1Params = ConvertFrom-BicepParam -Path $Phase1ParametersFile
$phase2Params = ConvertFrom-BicepParam -Path $Phase2ParametersFile

$allParams = @{} + $phase1Params
foreach ($key in $phase2Params.Keys) { $allParams[$key] = $phase2Params[$key] }
$config = Resolve-DeploymentConfig -Params $allParams -Defaults @{
    location          = 'West US 2'
    adminUsername     = 'testadmin'
    domainName        = 'contoso.com'
    domainNetBiosName = 'CONTOSO'
    dcExternal1Ip     = '192.168.1.10'
    dcExternal2Ip     = '192.168.2.10'
    driverOsType      = 'Windows'
    sutExternal1Ip    = '192.168.1.11'
    sutExternal2Ip    = '192.168.2.11'
    driverExternal1Ip = '192.168.1.111'
    driverExternal2Ip = '192.168.2.111'
}

# Override enableDiskEncryption if -SkipDiskEncryption was specified
if ($SkipDiskEncryption) {
    $phase1Params['enableDiskEncryption'] = $false
}

Write-Output "   Location (from bicepparam): $($config.location)"

# Create or validate the resource group (uses location from bicepparam)
Initialize-ResourceGroup -ResourceGroupName $ResourceGroupName -Location $config.location

$generateScript = Join-Path $PSScriptRoot "..\shared\Generate-ConfigJson.ps1"

# Validate custom images - non-driver VMs require Windows images
if ($phase1Params['dcCustomImageId']) {
    Write-Warning "dcCustomImageId is set to a custom image. Ensure it is a Windows-based image -- the DC VM requires Windows."
}
if ($phase2Params['sutCustomImageId']) {
    Write-Warning "sutCustomImageId is set to a custom image. Ensure it is a Windows-based image -- the SUT VM requires Windows."
}

# ===========================================================================
# Pre-flight: Validate VM sizes and OS images before creating
# any Azure resources (storage accounts, VMs, etc.)
# ===========================================================================

Write-Output "`nValidating VM sizes and OS images in $config.location..."

# Fetch all VM SKUs for the region (single API call, reused for all checks)
$vmSkus = Get-AzComputeResourceSku -Location $config.location |
    Where-Object { $_.ResourceType -eq 'virtualMachines' }

# Resolve VM sizes (with fallbacks for capacity-constrained regions).
# -ReturnAll gives us ALL statically-valid sizes so we can retry at deployment
# time if the first choice hits dynamic capacity restrictions (SkuNotAvailable).
$dcFallbacks = @('Standard_D2s_v6', 'Standard_D2as_v6', 'Standard_D2s_v5', 'Standard_D2as_v5', 'Standard_B2s_v2', 'Standard_D2s_v4')
$dcCandidates = Resolve-AvailableVmSize `
    -PreferredSize $phase1Params['dcVmSize'] `
    -FallbackSizes $dcFallbacks `
    -AvailableSkus $vmSkus `
    -Role 'DC' `
    -ReturnAll
$resolvedDcSize = $dcCandidates[0]
$phase1Params['dcVmSize'] = $resolvedDcSize
Write-Host "   DC VM size: $resolvedDcSize$(if ($dcCandidates.Count -gt 1) { " (+$($dcCandidates.Count - 1) fallbacks)" })"

$driverFallbacks = @('Standard_F4s_v2', 'Standard_D4s_v6', 'Standard_D4as_v6', 'Standard_D4s_v5', 'Standard_D4as_v5')
$driverCandidates = Resolve-AvailableVmSize `
    -PreferredSize $phase2Params['driverVmSize'] `
    -FallbackSizes $driverFallbacks `
    -AvailableSkus $vmSkus `
    -Role 'Driver' `
    -ReturnAll
$resolvedDriverSize = $driverCandidates[0]
$phase2Params['driverVmSize'] = $resolvedDriverSize
Write-Host "   Driver VM size: $resolvedDriverSize$(if ($driverCandidates.Count -gt 1) { " (+$($driverCandidates.Count - 1) fallbacks)" })"

$sutFallbacks = @('Standard_D8s_v6', 'Standard_D8as_v6', 'Standard_D8s_v5', 'Standard_D8as_v5', 'Standard_D8s_v4')
$sutCandidates = Resolve-AvailableVmSize `
    -PreferredSize $phase2Params['sutVmSize'] `
    -FallbackSizes $sutFallbacks `
    -AvailableSkus $vmSkus `
    -Role 'SUT' `
    -ReturnAll
$resolvedSutSize = $sutCandidates[0]
$phase2Params['sutVmSize'] = $resolvedSutSize
Write-Host "   SUT VM size: $resolvedSutSize$(if ($sutCandidates.Count -gt 1) { " (+$($sutCandidates.Count - 1) fallbacks)" })"

# Validate regional vCPU quota before creating any resources
Test-RegionalVCpuQuota -Location $config.location `
    -VmSizes @{ 'DC' = $resolvedDcSize; 'Driver' = $resolvedDriverSize; 'SUT' = $resolvedSutSize } `
    -AvailableSkus $vmSkus

# Validate OS image availability (skip when using custom images)
if (-not $phase1Params['dcCustomImageId']) {
    $dcImgOk = Test-VmImageAvailability -Location $config.location `
        -Publisher 'MicrosoftWindowsServer' -Offer 'WindowsServer' -Sku $phase1Params['dcOsVersion']
    $dcImgLabel = "MicrosoftWindowsServer/WindowsServer/$($phase1Params['dcOsVersion'])"
    if ($dcImgOk) {
        Write-Output "   DC image: $dcImgLabel"
    } else {
        Write-Error "DC image '$dcImgLabel' is not available in $config.location. Change dcOsVersion in the bicepparam file or deploy to a different region."
    }
}

if (-not $phase2Params['driverCustomImageId']) {
    if ($config.driverOsType -eq 'Linux') {
        $driverImgOk = Test-VmImageAvailability -Location $config.location `
            -Publisher 'Canonical' -Offer 'ubuntu-24_04-lts' -Sku $phase2Params['driverLinuxOsVersion']
        $driverImgLabel = "Canonical/ubuntu-24_04-lts/$($phase2Params['driverLinuxOsVersion'])"
    } else {
        $driverOffer = if ($phase2Params['driverOsVersion'] -like 'win10-*') { 'Windows-10' } else { 'Windows-11' }
        $driverImgOk = Test-VmImageAvailability -Location $config.location `
            -Publisher 'MicrosoftWindowsDesktop' -Offer $driverOffer -Sku $phase2Params['driverOsVersion']
        $driverImgLabel = "MicrosoftWindowsDesktop/$driverOffer/$($phase2Params['driverOsVersion'])"
    }
    if ($driverImgOk) {
        Write-Output "   Driver image: $driverImgLabel"
    } else {
        Write-Error "Driver image '$driverImgLabel' is not available in $config.location. Change driverOsVersion in the bicepparam file or deploy to a different region."
    }
}

if (-not $phase2Params['sutCustomImageId']) {
    $sutImgOk = Test-VmImageAvailability -Location $config.location `
        -Publisher 'MicrosoftWindowsServer' -Offer 'WindowsServer' -Sku $phase2Params['sutOsVersion']
    $sutImgLabel = "MicrosoftWindowsServer/WindowsServer/$($phase2Params['sutOsVersion'])"
    if ($sutImgOk) {
        Write-Output "   SUT image: $sutImgLabel"
    } else {
        Write-Error "SUT image '$sutImgLabel' is not available in $config.location. Change sutOsVersion in the bicepparam file or deploy to a different region."
    }
}

# ===========================================================================
# Advisory: Warn if auto-shutdown is near
# ===========================================================================
if ($phase1Params['enableAutoShutdown'] -eq $true) {
    $shutdownTime = $phase1Params['autoShutdownTime']       # e.g. '20:00'
    $shutdownTz   = $phase1Params['autoShutdownTimeZone']   # e.g. 'UTC'
    if ($shutdownTime -and $shutdownTz) {
        try {
            $tzInfo = [System.TimeZoneInfo]::FindSystemTimeZoneById($shutdownTz)
            $nowInTz = [System.TimeZoneInfo]::ConvertTimeFromUtc([DateTime]::UtcNow, $tzInfo)
            $parts = $shutdownTime -split ':'
            $shutdownToday = $nowInTz.Date.AddHours([int]$parts[0]).AddMinutes([int]$parts[1])
            # If shutdown time already passed today, it fires tomorrow
            if ($shutdownToday -lt $nowInTz) {
                $shutdownToday = $shutdownToday.AddDays(1)
            }
            $hoursUntilShutdown = ($shutdownToday - $nowInTz).TotalHours
            if ($hoursUntilShutdown -lt 3) {
                Write-Warning ("Auto-shutdown is scheduled at {0} ({1}), which is in {2:N1} hours. " +
                    "The deployment may not complete before VMs are shut down. " +
                    "Consider disabling auto-shutdown or adjusting the time.") -f $shutdownTime, $shutdownTz, $hoursUntilShutdown
            }
        } catch {
            # Non-fatal: if timezone lookup fails, skip the warning
        }
    }
}

# ===========================================================================
# Handle DSC package upload
# ===========================================================================
$actualDscPackageZipUrl = $DscPackageZipUrl
$tempStorage = $null

# Resolve DSC folder path relative to script directory
$DscFolderPath = if ([System.IO.Path]::IsPathRooted($DscFolderPath)) {
    $DscFolderPath
} else {
    Join-Path $PSScriptRoot $DscFolderPath
}

# Wrap packaging + deployment in try/finally so the temporary storage account
# is always cleaned up, even if packaging or deployment fails.
try {

if (-not $DscPackageZipUrl -and (Test-Path $DscFolderPath)) {
    Write-Output "`nPreparing DSC package for upload..."

    $tempStorage = Get-OrCreateStorageAccount `
        -ResourceGroupName $ResourceGroupName -Location $config.location `
        -StorageAccountName $StorageAccountName -ContainerName "dsc-package"

    $ctx = $tempStorage.Context
    $containerName = $tempStorage.ContainerName

    # Create temp directory for packaging (root = WorkingPath on the VM)
    $tempPackagePath = Join-Path $env:TEMP "DscPackage-$(Get-Random)"
    New-Item -ItemType Directory -Path $tempPackagePath -Force | Out-Null

    # Copy DSC folder into package as DSC/ subdirectory
    $dscDestination = Join-Path $tempPackagePath "DSC"
    Copy-Item -Path $DscFolderPath -Destination $dscDestination -Recurse -Force
    Write-Output "   [OK] Copied DSC folder to: $dscDestination"

    # Ensure DSC\Scripts target directory exists (may already have files from the DSC folder copy)
    $dscScriptsTarget = Join-Path $dscDestination "Scripts"
    if (-not (Test-Path $dscScriptsTarget)) {
        New-Item -ItemType Directory -Path $dscScriptsTarget -Force | Out-Null
    }

    # Overlay shared DSC files into package (shared is the source of truth)
    $sharedDscPath = Join-Path $PSScriptRoot "..\shared\DSC"
    if (Test-Path $sharedDscPath) {
        # Root-level scripts (Deploy-DC.ps1, DC-Configuration.ps1, etc.)
        foreach ($sharedFile in (Get-ChildItem -Path $sharedDscPath -Filter '*.ps1' -File)) {
            Copy-Item -Path $sharedFile.FullName -Destination $dscDestination -Force
        }
        Write-Output "   [OK] Overlaid shared DSC root scripts"

        # Scripts/ subfolder
        $sharedScriptsPath = Join-Path $sharedDscPath "Scripts"
        if (Test-Path $sharedScriptsPath) {
            Copy-Item -Path "$sharedScriptsPath\*" -Destination $dscScriptsTarget -Recurse -Force
            Write-Output "   [OK] Overlaid shared DSC/Scripts from: $sharedScriptsPath"
        }
    } else {
        Write-Warning "Shared DSC folder not found at $sharedDscPath -- package may be incomplete"
    }

    # Download external assets (GPOBackup.zip, ParamConfig.json) into the package
    $gpoSource = Join-Path $PSScriptRoot "..\..\..\Setup\Scripts\GPOBackup.zip"
    Install-DscPackageAssets -ScriptsFolder $dscScriptsTarget -Scenario 'Domain' `
        -LocalGpoBackupPath $gpoSource

    # Copy Tools.json to package root (after overlay, it's in the packaged Scripts folder)
    $toolsSource = Join-Path $dscScriptsTarget "Tools.json"
    if (Test-Path $toolsSource) {
        Copy-Item -Path $toolsSource -Destination (Join-Path $tempPackagePath "Tools.json") -Force
        Write-Output "   [OK] Copied Tools.json to package root"
    } else {
        Write-Warning "Tools.json not found at $toolsSource"
    }

    # Generate Config.json at package root with deployment parameters
    Write-Output "   Generating Config.json with deployment parameters..."
    & $generateScript `
        -Scenario "Domain" `
        -OutputPath (Join-Path $tempPackagePath "Config.json") `
        -AdminUsername $config.adminUsername `
        -AdminPassword $plainPassword `
        -DomainName $config.domainName `
        -DomainNetBiosName $config.domainNetBiosName `
        -DCExternal1Ip $config.dcExternal1Ip `
        -DCExternal2Ip $config.dcExternal2Ip `
        -SutExternal1Ip $config.sutExternal1Ip `
        -SutExternal2Ip $config.sutExternal2Ip `
        -DriverExternal1Ip $config.driverExternal1Ip `
        -DriverExternal2Ip $config.driverExternal2Ip `
        -DriverOSType $config.driverOsType
    if (-not $?) { throw "Generate-ConfigJson.ps1 failed" }
    Write-Output "   [OK] Config.json generated"

    # Copy generated Config.json into DSC\Scripts (overwrite template so domain utility
    # scripts that default to $PSScriptRoot\Config.json find the real values)
    Copy-Item (Join-Path $tempPackagePath "Config.json") -Destination "$dscScriptsTarget\Config.json" -Force
    Write-Output "   [OK] Copied Config.json into DSC\Scripts (overwriting template)"

    # Generate ResultsUpload.json for test results upload
    Write-Output "   Generating ResultsUpload.json for test results upload..."
    $resultsConfig = New-ResultsUploadConfig `
        -StorageAccountName $tempStorage.Name `
        -StorageContext $ctx
    $resultsConfig | ConvertTo-Json -Depth 3 | Set-Content -Path (Join-Path $tempPackagePath "ResultsUpload.json") -Force
    Write-Output "   [OK] ResultsUpload.json generated"

    # Create zip file
    $tempZipPath = Join-Path $env:TEMP "DSC-Package-$(Get-Random).zip"
    Compress-Archive -Path (Join-Path $tempPackagePath "*") -DestinationPath $tempZipPath -Force
    Write-Output "   [OK] Created zip: $tempZipPath"

    # Upload
    $actualDscPackageZipUrl = Send-BlobWithSasUrl `
        -FilePath $tempZipPath -BlobName "Domain-Package.zip" `
        -ContainerName $containerName -StorageContext $ctx

    # Cleanup temp files
    Remove-Item $tempPackagePath -Recurse -Force
    Remove-Item $tempZipPath -Force

} elseif ($DscPackageZipUrl) {
    Write-Output "[OK] Using provided DscPackageZipUrl"
}

# ===========================================================================
# Bicep template validation (dry run)
# ===========================================================================
Write-Output "Validating Bicep templates..."

$phase1ValidationParams = @{} + $phase1Params
$phase1ValidationParams['dcVmSize'] = $dcCandidates[0]
$phase1ValidationParams['adminPassword'] = $AdminPassword
if ($actualDscPackageZipUrl) {
    $phase1ValidationParams['domainPackageZipUrl'] = $actualDscPackageZipUrl
}

$validationResult = Test-AzResourceGroupDeployment `
    -ResourceGroupName $ResourceGroupName `
    -TemplateFile (Join-Path $PSScriptRoot 'phase1.bicep') `
    -TemplateParameterObject $phase1ValidationParams `
    -ErrorAction SilentlyContinue

if ($validationResult) {
    # Filter out capacity/SKU errors — those are handled by the deployment retry loop
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

if ($ValidateOnly) {
    Write-Output "`nValidation-only mode: skipping deployment."
    Write-Output "Note: Phase 2 template depends on Phase 1 outputs and cannot be validated without deploying Phase 1."
    return
}

# ===========================================================================
# PHASE 1: Deploy Network + Domain Controller
# ===========================================================================

if (-not $SkipPhase1) {
    Write-Output @"

  PHASE 1: Deploying Network + Domain Controller
  - Virtual Network + Bastion
  - Domain Controller (DC01) with AD DS

"@

    $phase1Start = Get-Date

    # Deployment-time capacity retry: if SkuNotAvailable, try next fallback size.
    # Static pre-flight (Resolve-AvailableVmSize) filters Location/Zone restrictions,
    # but dynamic capacity exhaustion is only detected when ARM actually provisions.
    $dcSizeIndex = 0
    $phase1Deployed = $false

    while (-not $phase1Deployed -and $dcSizeIndex -lt $dcCandidates.Count) {
        $dcCurrentSize = $dcCandidates[$dcSizeIndex]
        $deploymentName = "Phase1-$(Get-Date -Format 'yyyyMMdd-HHmmss')"

        $phase1DeployParams = @{} + $phase1Params
        $phase1DeployParams['dcVmSize'] = $dcCurrentSize
        $phase1DeployParams['adminPassword'] = $AdminPassword
        if ($actualDscPackageZipUrl) {
            $phase1DeployParams['domainPackageZipUrl'] = $actualDscPackageZipUrl
        }

        if ($dcSizeIndex -gt 0) {
            Write-Output "`nRetrying Phase 1 with fallback DC size: $dcCurrentSize"
        }
        Write-Output "Starting Phase 1 deployment (DC: $dcCurrentSize)..."

        # Pre-validate to catch SkuNotAvailable before starting the full deployment.
        $preValidation = Test-AzResourceGroupDeployment `
            -ResourceGroupName $ResourceGroupName `
            -TemplateFile (Join-Path $PSScriptRoot 'phase1.bicep') `
            -TemplateParameterObject $phase1DeployParams
        if ($preValidation) {
            $allCodes = @($preValidation | ForEach-Object { $_.Code })
            $allCodes += $preValidation | ForEach-Object { $_.Details } | Where-Object { $_ } | ForEach-Object { $_.Code }
            $allMessages = ($preValidation | ForEach-Object {
                $msg = "[$($_.Code)] $($_.Message)"
                if ($_.Details) { $msg += " Details: $(($_.Details | ForEach-Object { "[$($_.Code)] $($_.Message)" }) -join '; ')" }
                $msg
            }) -join "`n  "

            $isSkuError = $allCodes -match 'SkuNotAvailable|AllocationFailed|ZonalAllocationFailed'
            if ($isSkuError -and ($dcSizeIndex + 1) -lt $dcCandidates.Count) {
                Write-Warning "DC VM size '$dcCurrentSize' not available: $allMessages"
                Write-Warning "Trying next fallback..."
                $dcSizeIndex++
                continue
            }
            throw "Phase 1 pre-validation failed:`n  $allMessages"
        }
        Write-Output "  Pre-validation passed"

        $phase1Job = New-AzResourceGroupDeployment `
            -ResourceGroupName $ResourceGroupName `
            -Name $deploymentName `
            -TemplateFile (Join-Path $PSScriptRoot 'phase1.bicep') `
            -TemplateParameterObject $phase1DeployParams `
            -ErrorAction Stop `
            -AsJob

        Watch-Deployment -ResourceGroupName $ResourceGroupName -DeploymentName $deploymentName -Job $phase1Job

        $deployment = $null
        $phase1Error = $null
        try {
            $deployment = $phase1Job | Receive-Job -Wait -AutoRemoveJob -ErrorAction Stop
        } catch {
            $phase1Error = $_
            $phase1Job | Remove-Job -Force -ErrorAction SilentlyContinue
        }

        $provState = if ($deployment) { $deployment.ProvisioningState } else { 'Failed' }

        if ($provState -eq 'Succeeded' -and -not $phase1Error) {
            $phase1Deployed = $true
            $phase1DeploymentResult = $deployment  # Preserve for KV outputs
        } else {
            $isCapacity = Test-CapacityError -ErrorRecord $phase1Error `
                -ResourceGroupName $ResourceGroupName -DeploymentName $deploymentName

            if ($isCapacity -and ($dcSizeIndex + 1) -lt $dcCandidates.Count) {
                Write-Warning "DC VM size '$dcCurrentSize' hit capacity restrictions in $config.location. Trying next fallback..."
                $dcSizeIndex++
            } else {
                if ($phase1Error) { throw $phase1Error }
                throw "Phase 1 deployment '$deploymentName' finished with state: $provState"
            }
        }
    }

    $phase1Duration = [math]::Round(((Get-Date) - $phase1Start).TotalMinutes, 1)
    Write-Output "`n[OK] Phase 1 completed in $phase1Duration minutes"

    # ===========================================================================
    # Wait for Domain Controller to be fully configured
    # ===========================================================================

    Write-Output @"

  Waiting for Domain Controller Configuration
  DC must be fully configured before Client01/Node01 deploy

"@

    $dcReady = Wait-ForDomainController `
        -ResourceGroupName $ResourceGroupName `
        -VMNamePattern "*-dc01" `
        -CheckScript "Test-Path 'C:\Domain-Package\DSC\Deploy-DC.Completed.signal'" `
        -TimeoutMinutes $DCReadyTimeoutMinutes `
        -PollIntervalSeconds 30

    if (-not $dcReady) {
        Write-Output @"

Domain Controller did not signal readiness within the timeout.

Options:
1. Increase timeout with -DCReadyTimeoutMinutes parameter
2. Connect to DC via Bastion and check: C:\Domain-Package\DSC\Deploy-DC.log
3. Resume Phase 2 later once DC is ready:

   .\deploy.ps1 ``
       -SubscriptionId '$SubscriptionId' ``
       -ResourceGroupName '$ResourceGroupName' ``
       -AdminPassword (ConvertTo-SecureString '<password>' -AsPlainText -Force) ``
       -SkipPhase1$(if ($actualDscPackageZipUrl) { " ``
       -DscPackageZipUrl '$actualDscPackageZipUrl'" })
"@
        exit 1
    }

    # ===========================================================================
    # Disk Encryption — DC (after configuration is complete)
    # ===========================================================================
    if (-not $SkipDiskEncryption -and $phase1DeploymentResult) {
        $dcVmName = $phase1DeploymentResult.Outputs.dcVmName.Value
        Invoke-DiskEncryptionForVMs -ResourceGroupName $ResourceGroupName `
            -DeploymentOutputs $phase1DeploymentResult.Outputs `
            -VMNames @($dcVmName)
    }
}

# ===========================================================================
# PHASE 2: Deploy Driver Computer + SUT
# ===========================================================================

if (-not $SkipPhase2) {

    # Verify DC readiness when resuming (Phase 1 was skipped)
    if ($SkipPhase1 -and -not $SkipDCReadyCheck) {
        Write-Output @"

  Verifying Domain Controller Readiness
  Checking DC configuration before deploying Phase 2 VMs

"@

        $dcReady = Wait-ForDomainController `
            -ResourceGroupName $ResourceGroupName `
            -VMNamePattern "*-dc01" `
            -CheckScript "Test-Path 'C:\Domain-Package\DSC\Deploy-DC.Completed.signal'" `
            -TimeoutMinutes 5 `
            -PollIntervalSeconds 15

        if (-not $dcReady) {
            Write-Output @"

Domain Controller does not appear to be ready.
If you are sure the DC is configured, re-run with -SkipDCReadyCheck.
Otherwise, connect to DC via Bastion and check: C:\Domain-Package\DSC\Deploy-DC.log
"@
            exit 1
        }
        Write-Output "[OK] Domain Controller is ready"
    }

    # Clean up stale computer accounts on DC before fresh Phase 2 deployment.
    # When -SkipPhase1 is used, the DC may have leftover computer objects from a
    # previous SUT/Driver deployment. Add-Computer on the new VMs can fail with
    # "The account already exists" if these aren't removed first.
    if ($SkipPhase1) {
        Write-Output "`nCleaning up stale computer accounts on DC..."
        $envPrefix = if ($phase1Params['environmentPrefix']) { $phase1Params['environmentPrefix'] } else { 'fstest' }
        $dcVmName = "$envPrefix-dc01"

        # Read machine names from Config.json on the DC to avoid hardcoding.
        $cleanupScript = @"
Import-Module ActiveDirectory -ErrorAction Stop

# Discover names from the Config.json left by the previous deployment
`$configPaths = @(
    'C:\Domain-Package\Config.json',
    'C:\Domain-Package\DSC\Scripts\Config.json'
)
`$config = `$null
foreach (`$p in `$configPaths) {
    if (Test-Path `$p) {
        `$config = Get-Content `$p -Raw | ConvertFrom-Json
        break
    }
}

`$staleNames = @()
if (`$config) {
    # Machine computer names (Node01, Client01, etc.)
    foreach (`$prop in `$config.Machines.PSObject.Properties) {
        if (`$prop.Value.ComputerName) { `$staleNames += `$prop.Value.ComputerName }
    }
    # Deduplicate and exclude the DC itself
    `$dcName = (`$config.Machines.PSObject.Properties | Where-Object { `$_.Name -match 'DC' }).Value.ComputerName
    `$staleNames = `$staleNames | Where-Object { `$_ -ne `$dcName } | Select-Object -Unique
} else {
    Write-Output "WARNING: Config.json not found on DC - cannot discover machine names."
    Write-Output "Skipping stale account cleanup."
    return
}

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
"@

        try {
            $result = Invoke-AzVMRunCommand -ResourceGroupName $ResourceGroupName `
                -VMName $dcVmName -CommandId 'RunPowerShellScript' `
                -ScriptString $cleanupScript
            if ($result.Value) {
                foreach ($v in $result.Value) {
                    if ($v.Message) {
                        $v.Message -split "`n" | ForEach-Object { Write-Output "   $_" }
                    }
                }
            }
        } catch {
            Write-Warning "Could not clean up stale computer accounts on DC: $($_.Exception.Message)"
            Write-Warning "If domain join fails, manually remove stale computer accounts from AD."
        }
    }

    Write-Output @"

  PHASE 2: Deploying Domain-Joined VMs
  - Driver Computer (Client01)
  - SUT Computer (Node01)

"@

    $phase2Start = Get-Date
    $deploymentName = "Phase2-$(Get-Date -Format 'yyyyMMdd-HHmmss')"

    # Get Phase 1 outputs
    $phase1Deployment = Get-AzResourceGroupDeployment -ResourceGroupName $ResourceGroupName |
        Where-Object { $_.DeploymentName -like "Phase1-*" -and $_.ProvisioningState -eq 'Succeeded' } |
        Sort-Object Timestamp -Descending |
        Select-Object -First 1

    if (-not $phase1Deployment) {
        Write-Error "Phase 1 deployment not found in resource group '$ResourceGroupName'. Run Phase 1 first."
        exit 1
    }

    Write-Output "Retrieved Phase 1 outputs from deployment: $($phase1Deployment.DeploymentName)"
    $external1SubnetId = $phase1Deployment.Outputs.external1SubnetId.Value
    $external2SubnetId = $phase1Deployment.Outputs.external2SubnetId.Value
    $dcExternal1Ip = $phase1Deployment.Outputs.dcExternal1Ip.Value
    Write-Output "   External1 Subnet: $external1SubnetId"
    Write-Output "   External2 Subnet: $external2SubnetId"
    Write-Output "   DC IP: $dcExternal1Ip"

    # When resuming without a local package, try to reuse existing blob from storage account
    if ($SkipPhase1 -and -not $actualDscPackageZipUrl) {
        Write-Output "`nLooking for previously uploaded Domain-Package in storage..."
        $storageAccounts = Get-AzStorageAccount -ResourceGroupName $ResourceGroupName -ErrorAction SilentlyContinue |
            Where-Object { $_.StorageAccountName -like 'fststorage*' }
        foreach ($sa in $storageAccounts) {
            $saCtx = $sa.Context
            # Check both new (dsc-package) and legacy (domain-package) container names
            foreach ($cName in @('dsc-package', 'domain-package')) {
                $blob = Get-AzStorageBlob -Container $cName -Context $saCtx -ErrorAction SilentlyContinue |
                    Where-Object { $_.Name -eq 'Domain-Package.zip' } |
                    Select-Object -First 1
                if ($blob) {
                    $sasToken = New-AzStorageBlobSASToken -Container $cName -Blob $blob.Name `
                        -Permission r -ExpiryTime (Get-Date).AddHours(2) -Context $saCtx -FullUri
                    $actualDscPackageZipUrl = $sasToken
                    Write-Output "   [OK] Found existing package blob: $($blob.Name) in $($sa.StorageAccountName)/$cName"
                    break
                }
            }
            if ($actualDscPackageZipUrl) { break }
        }
        if (-not $actualDscPackageZipUrl) {
            Write-Output "   [WARN] No previously uploaded package found. VMs will deploy without package configuration."
            Write-Output "      To include a package, re-run with -DscPackageZipUrl or -DscFolderPath"
        }
    }

    # Deployment-time capacity retry for Phase 2 (Driver + SUT).
    # Track candidate index per role; on SkuNotAvailable, advance the failing role's index.
    $driverSizeIndex = 0
    $sutSizeIndex = 0
    $phase2Deployed = $false

    while (-not $phase2Deployed) {
        $driverCurrentSize = $driverCandidates[$driverSizeIndex]
        $sutCurrentSize    = $sutCandidates[$sutSizeIndex]
        $deploymentName    = "Phase2-$(Get-Date -Format 'yyyyMMdd-HHmmss')"

        $phase2DeployParams = @{} + $phase2Params
        $phase2DeployParams['driverVmSize'] = $driverCurrentSize
        $phase2DeployParams['sutVmSize']    = $sutCurrentSize
        $phase2DeployParams['adminPassword'] = $AdminPassword
        $phase2DeployParams['external1SubnetId'] = $external1SubnetId
        $phase2DeployParams['external2SubnetId'] = $external2SubnetId
        $phase2DeployParams['dcExternal1Ip'] = $dcExternal1Ip
        $phase2DeployParams['dcExternal2Ip'] = $config.dcExternal2Ip
        if ($actualDscPackageZipUrl) {
            $phase2DeployParams['domainPackageZipUrl'] = $actualDscPackageZipUrl
        }

        Write-Output "`nStarting Phase 2 deployment (Driver: $driverCurrentSize, SUT: $sutCurrentSize)..."
        Write-Output "Phase 2 parameters:"
        foreach ($k in ($phase2DeployParams.Keys | Sort-Object)) {
            $v = $phase2DeployParams[$k]
            if ($k -match 'password|Password') { $v = '***' }
            Write-Output "   $k = $v"
        }

        # Pre-validate to catch SkuNotAvailable before starting the full deployment.
        # ARM validation errors don't produce deployment operations, so Test-CapacityError
        # cannot detect them after the fact. Test-AzResourceGroupDeployment returns
        # structured error objects with .Code and .Details we can inspect.
        $preValidation = Test-AzResourceGroupDeployment `
            -ResourceGroupName $ResourceGroupName `
            -TemplateFile (Join-Path $PSScriptRoot 'phase2.bicep') `
            -TemplateParameterObject $phase2DeployParams
        if ($preValidation) {
            $allCodes = @($preValidation | ForEach-Object { $_.Code })
            $allCodes += $preValidation | ForEach-Object { $_.Details } | Where-Object { $_ } | ForEach-Object { $_.Code }
            $allMessages = ($preValidation | ForEach-Object {
                $msg = "[$($_.Code)] $($_.Message)"
                if ($_.Details) { $msg += " Details: $(($_.Details | ForEach-Object { "[$($_.Code)] $($_.Message)" }) -join '; ')" }
                $msg
            }) -join "`n  "
            Write-Output "  Pre-validation result: $allMessages"

            $isSkuError = $allCodes -match 'SkuNotAvailable|AllocationFailed|ZonalAllocationFailed'
            if ($isSkuError) {
                $canRetry = $false
                $skuMsg = ($preValidation | ForEach-Object { $_.Details } | Where-Object { $_ } | ForEach-Object { $_.Message }) -join ' '
                $driverFailed = $skuMsg -match [regex]::Escape($driverCurrentSize)
                $sutFailed    = $skuMsg -match [regex]::Escape($sutCurrentSize)
                if ($driverFailed -and ($driverSizeIndex + 1) -lt $driverCandidates.Count) {
                    Write-Warning "Driver VM size '$driverCurrentSize' not available. Trying next fallback..."
                    $driverSizeIndex++; $canRetry = $true
                }
                if ($sutFailed -and ($sutSizeIndex + 1) -lt $sutCandidates.Count) {
                    Write-Warning "SUT VM size '$sutCurrentSize' not available. Trying next fallback..."
                    $sutSizeIndex++; $canRetry = $true
                }
                if (-not $driverFailed -and -not $sutFailed) {
                    if (($driverSizeIndex + 1) -lt $driverCandidates.Count) { $driverSizeIndex++; $canRetry = $true }
                    if (($sutSizeIndex + 1) -lt $sutCandidates.Count) { $sutSizeIndex++; $canRetry = $true }
                }
                if ($canRetry) { continue }
                throw "Phase 2 pre-validation failed (SkuNotAvailable) and no more fallback sizes available:`n  $allMessages"
            } else {
                # Non-capacity validation error -- fail immediately
                throw "Phase 2 pre-validation failed:`n  $allMessages"
            }
        }
        Write-Output "  Pre-validation passed"

        $phase2Job = New-AzResourceGroupDeployment `
            -ResourceGroupName $ResourceGroupName `
            -Name $deploymentName `
            -TemplateFile (Join-Path $PSScriptRoot 'phase2.bicep') `
            -TemplateParameterObject $phase2DeployParams `
            -ErrorAction Stop `
            -AsJob

        Watch-Deployment -ResourceGroupName $ResourceGroupName -DeploymentName $deploymentName -Job $phase2Job

        $deployment = $null
        $phase2Error = $null
        try {
            $deployment = $phase2Job | Receive-Job -Wait -AutoRemoveJob -ErrorAction Stop
        } catch {
            $phase2Error = $_
            $phase2Job | Remove-Job -Force -ErrorAction SilentlyContinue
        }

        $provState = if ($deployment) { $deployment.ProvisioningState } else { 'Failed' }

        if ($provState -eq 'Succeeded' -and -not $phase2Error) {
            $phase2Deployed = $true
            $phase2DeploymentResult = $deployment  # Preserve for VM name outputs
        } else {
            # Dump full error details for diagnosis
            if ($phase2Error) {
                Write-Output "`nPhase 2 deployment error details:"
                Write-Output "  Message: $($phase2Error.Exception.Message)"
                $inner = $phase2Error.Exception.InnerException
                while ($inner) {
                    Write-Output "  Inner: $($inner.Message)"
                    $inner = $inner.InnerException
                }
                if ($phase2Error.ErrorDetails) {
                    Write-Output "  ErrorDetails: $($phase2Error.ErrorDetails.Message)"
                }
            }

            $isCapacity = Test-CapacityError -ErrorRecord $phase2Error `
                -ResourceGroupName $ResourceGroupName -DeploymentName $deploymentName

            # Identify which role's SKU failed by matching the error text
            $errText = if ($phase2Error) { "$($phase2Error.Exception.Message)" } else { '' }
            $driverFailed = $errText -match [regex]::Escape($driverCurrentSize)
            $sutFailed    = $errText -match [regex]::Escape($sutCurrentSize)

            $canRetry = $false
            if ($isCapacity) {
                if ($driverFailed -and ($driverSizeIndex + 1) -lt $driverCandidates.Count) {
                    Write-Warning "Driver VM size '$driverCurrentSize' hit capacity restrictions. Trying next fallback..."
                    $driverSizeIndex++
                    $canRetry = $true
                }
                if ($sutFailed -and ($sutSizeIndex + 1) -lt $sutCandidates.Count) {
                    Write-Warning "SUT VM size '$sutCurrentSize' hit capacity restrictions. Trying next fallback..."
                    $sutSizeIndex++
                    $canRetry = $true
                }
                # If we can't identify which role, try advancing both
                if (-not $driverFailed -and -not $sutFailed) {
                    if (($driverSizeIndex + 1) -lt $driverCandidates.Count) {
                        $driverSizeIndex++; $canRetry = $true
                    }
                    if (($sutSizeIndex + 1) -lt $sutCandidates.Count) {
                        $sutSizeIndex++; $canRetry = $true
                    }
                }
            }

            if (-not $canRetry) {
                if ($phase2Error) { throw $phase2Error }
                throw "Phase 2 deployment '$deploymentName' finished with state: $provState"
            }
        }
    }

    $phase2Duration = [math]::Round(((Get-Date) - $phase2Start).TotalMinutes, 1)
    Write-Output "`n[OK] Phase 2 completed in $phase2Duration minutes"

    # ===========================================================================
    # Disk Encryption — Driver + SUT (after Phase 2 deployment)
    # ===========================================================================
    if (-not $SkipDiskEncryption -and $phase2DeploymentResult) {
        $driverVmName = $phase2DeploymentResult.Outputs.driverVmName.Value
        $sutVmName    = $phase2DeploymentResult.Outputs.sutVmName.Value

        Invoke-DiskEncryptionForVMs -ResourceGroupName $ResourceGroupName `
            -DeploymentOutputs $phase1Deployment.Outputs `
            -VMNames @($sutVmName, $driverVmName)
    }
}

# ===========================================================================
# Deployment Complete
# ===========================================================================

Write-Output @"

  DEPLOYMENT COMPLETE

  Your domain environment is ready!

VMs deployed:
  - DC01     (Domain Controller) - AD DS, DNS configured
  - Client01 (Driver Computer)   - Domain-joined
  - Node01   (SUT)               - Domain-joined, File Server role

Network Configuration:
  - DC01:     External1 = $($config.dcExternal1Ip), External2 = $($config.dcExternal2Ip)
  - Client01: External1 = $($config.driverExternal1Ip), External2 = $($config.driverExternal2Ip)
  - Node01:   External1 = $($config.sutExternal1Ip), External2 = $($config.sutExternal2Ip)

What happens next (fully automatic):
1. DC configures AD DS and creates domain accounts
2. Driver and SUT join domain, install tools via DSC
3. Driver VM runs Execute-TestCaseByContext.ps1 automatically
4. Results will be written to C:\Test\TestResults\ once complete
5. Signal file: C:\Test\test.finished.signal indicates completion

To monitor progress:
  - RDP/Bastion into Client01 and check C:\Domain-Package\DSC\Invoke-TestRun.log

For detailed instructions, see: README.md
"@

} finally {
    # Storage account is kept alive for test results upload.
    # Only print info so the user knows where results will go.
    if ($tempStorage) {
        Write-Output "`nStorage account '$($tempStorage.Name)' retained for test results upload."
        if ($tempStorage.IsTemporary) {
            Write-Output "   To clean up later: Remove-AzStorageAccount -ResourceGroupName '$ResourceGroupName' -Name '$($tempStorage.Name)' -Force"
        }
    }
    # Clear sensitive variables from memory
    $plainPassword = $null
}
