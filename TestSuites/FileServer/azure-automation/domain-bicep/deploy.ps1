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
Initialize-BicepCli

function Write-DcDeploymentHeartbeat {
    param(
        [string]$ResourceGroupName,
        [string]$VMNamePattern = '*-dc01'
    )
    try {
        $vm = Get-AzVM -ResourceGroupName $ResourceGroupName |
            Where-Object { $_.Name -like $VMNamePattern } |
            Select-Object -First 1
        if ($null -eq $vm) {
            Write-Warning "DC heartbeat unavailable because no VM matched '$VMNamePattern'."
            return
        }
        $result = Invoke-AzVMRunCommand -ResourceGroupName $ResourceGroupName `
            -VMName $vm.Name -CommandId 'RunPowerShellScript' -ScriptString @'
$heartbeat = 'C:\Domain-Package\DSC\Deploy-DC.heartbeat.json'
if (Test-Path $heartbeat) {
    Get-Content -Path $heartbeat -Raw
} else {
    '{"Status":"Heartbeat file is not present; inspect Deploy-DC.log."}'
}
'@
        $messages = @($result.Value | ForEach-Object { $_.Message } |
            Where-Object { -not [string]::IsNullOrWhiteSpace($_) })
        if ($messages.Count -gt 0) {
            Write-Output "Last DC deployment heartbeat:"
            $messages | ForEach-Object { Write-Output "  $_" }
        }
    }
    catch {
        Write-Warning "Could not retrieve the DC deployment heartbeat: $($_.Exception.Message)"
    }
}

# Initialize Azure connection
Import-AzureModules
Connect-AzureSubscription -SubscriptionId $SubscriptionId

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

# Post-deploy Azure Disk Encryption runs only when the bicepparam enables it
# (otherwise no Key Vault exists) and -SkipDiskEncryption was not passed.
$diskEncryptionRequested = (-not $SkipDiskEncryption) -and ($phase1Params['enableDiskEncryption'] -ne $false)
$phase2AutoShutdownRequested = $phase2Params['enableAutoShutdown'] -eq $true
$autoShutdownRequested = ($phase1Params['enableAutoShutdown'] -eq $true) -or $phase2AutoShutdownRequested
$autoShutdownTime = if ($phase2AutoShutdownRequested -and $phase2Params['autoShutdownTime']) {
    $phase2Params['autoShutdownTime']
} elseif ($phase1Params['autoShutdownTime']) {
    $phase1Params['autoShutdownTime']
} else {
    '20:00'
}
$autoShutdownTimeZone = if ($phase2AutoShutdownRequested -and $phase2Params['autoShutdownTimeZone']) {
    $phase2Params['autoShutdownTimeZone']
} elseif ($phase1Params['autoShutdownTimeZone']) {
    $phase1Params['autoShutdownTimeZone']
} else {
    'UTC'
}

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

Write-Output "`nValidating VM sizes and OS images in $($config.location)..."

# Fetch the lightweight regional VM-size snapshot once. ARM pre-validation and
# deployment-time fallback remain authoritative for policy and current capacity.
$vmSkus = @(Get-RegionalVmSkuSnapshot -Location $config.location)

# Resolve VM sizes (with fallbacks for capacity-constrained regions).
# -ReturnAll gives us ALL statically-valid sizes so we can retry at deployment
# time if the first choice hits dynamic capacity restrictions (SkuNotAvailable).
# The per-role fallback lists are data, not code: parameters/VmSizeFallbacks.psd1.
$vmSizeFallbacks = Import-PowerShellDataFile (Join-Path $PSScriptRoot '..\shared\parameters\VmSizeFallbacks.psd1')
$dcCandidates = Resolve-AvailableVmSize `
    -PreferredSize $phase1Params['dcVmSize'] `
    -FallbackSizes $vmSizeFallbacks.DC `
    -AvailableSkus $vmSkus `
    -Role 'DC' `
    -ReturnAll
$resolvedDcSize = $dcCandidates[0]
$phase1Params['dcVmSize'] = $resolvedDcSize
Write-Host "   DC VM size: $resolvedDcSize$(if ($dcCandidates.Count -gt 1) { " (+$($dcCandidates.Count - 1) fallbacks)" })"

$driverCandidates = Resolve-AvailableVmSize `
    -PreferredSize $phase2Params['driverVmSize'] `
    -FallbackSizes $vmSizeFallbacks.Driver `
    -AvailableSkus $vmSkus `
    -Role 'Driver' `
    -ReturnAll
$resolvedDriverSize = $driverCandidates[0]
$phase2Params['driverVmSize'] = $resolvedDriverSize
Write-Host "   Driver VM size: $resolvedDriverSize$(if ($driverCandidates.Count -gt 1) { " (+$($driverCandidates.Count - 1) fallbacks)" })"

$sutCandidates = Resolve-AvailableVmSize `
    -PreferredSize $phase2Params['sutVmSize'] `
    -FallbackSizes $vmSizeFallbacks.SUT `
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
        Write-Error "DC image '$dcImgLabel' is not available in $($config.location). Change dcOsVersion in the bicepparam file or deploy to a different region."
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
        Write-Error "Driver image '$driverImgLabel' is not available in $($config.location). Change driverOsVersion in the bicepparam file or deploy to a different region."
    }
}

if (-not $phase2Params['sutCustomImageId']) {
    $sutImgOk = Test-VmImageAvailability -Location $config.location `
        -Publisher 'MicrosoftWindowsServer' -Offer 'WindowsServer' -Sku $phase2Params['sutOsVersion']
    $sutImgLabel = "MicrosoftWindowsServer/WindowsServer/$($phase2Params['sutOsVersion'])"
    if ($sutImgOk) {
        Write-Output "   SUT image: $sutImgLabel"
    } else {
        Write-Error "SUT image '$sutImgLabel' is not available in $($config.location). Change sutOsVersion in the bicepparam file or deploy to a different region."
    }
}

# ===========================================================================
# Advisory: Warn if auto-shutdown is near
# ===========================================================================
if ($autoShutdownRequested) {
    $shutdownTime = $autoShutdownTime
    $shutdownTz   = $autoShutdownTimeZone
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
# Validation-only gate -- BEFORE any resource creation (resource group, storage
# account, package upload). ARM template validation needs an existing resource
# group, so it runs only when one is already there.
# ===========================================================================
$phase1Params['enableAutoShutdown'] = $false
$phase2Params['enableAutoShutdown'] = $false

if ($ValidateOnly) {
    if (Get-AzResourceGroup -Name $ResourceGroupName -ErrorAction SilentlyContinue) {
        Write-Output "`nValidating Phase 1 template against existing resource group..."
        $phase1ValidationParams = @{} + $phase1Params
        $phase1ValidationParams['dcVmSize'] = $dcCandidates[0]
        $phase1ValidationParams['adminPassword'] = $AdminPassword

        $validationResult = Test-AzResourceGroupDeployment `
            -ResourceGroupName $ResourceGroupName `
            -TemplateFile (Join-Path $PSScriptRoot 'phase1.bicep') `
            -TemplateParameterObject $phase1ValidationParams `
            -ErrorAction SilentlyContinue

        if ($validationResult) {
            # Capacity/SKU errors are handled by the deployment-time retry loop
            $nonCapacityErrors = $validationResult | Where-Object {
                $_.Code -notmatch 'SkuNotAvailable|ZonalAllocationFailed|AllocationFailed'
            }
            if ($nonCapacityErrors) {
                Write-Error "Phase 1 template validation failed:`n$($nonCapacityErrors | ForEach-Object { "  - [$($_.Code)] $($_.Message)" } | Out-String)"
                exit 1
            }
            Write-Warning "Template validation returned capacity warnings (SkuNotAvailable) -- the deployment retry loop will handle these."
        } else {
            Write-Output "[OK] Phase 1 template validation passed"
        }
    } else {
        Write-Output "`nResource group '$ResourceGroupName' does not exist yet; skipping ARM template validation (pre-flight SKU/quota/image checks passed)."
    }
    Write-Output "Validation-only mode: no resources were created."
    Write-Output "Note: Phase 2 template depends on Phase 1 outputs and cannot be validated without deploying Phase 1."
    return
}

# Create or validate the resource group (uses location from bicepparam)
Initialize-ResourceGroup -ResourceGroupName $ResourceGroupName -Location $config.location
$envPrefix = if ($phase1Params['environmentPrefix']) { $phase1Params['environmentPrefix'] } else { 'fstest' }
$autoShutdownVmNames = @("$envPrefix-dc01", "$envPrefix-node01", "$envPrefix-client01")
$autoShutdownRestored = $false

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

if ($autoShutdownRequested) {
    Remove-VmAutoShutdownSchedules -ResourceGroupName $ResourceGroupName `
        -VMNames $autoShutdownVmNames
}

if (-not $DscPackageZipUrl -and (Test-Path $DscFolderPath)) {
    Write-Output "`nPreparing DSC package for upload..."

    $tempStorage = Get-OrCreateStorageAccount `
        -ResourceGroupName $ResourceGroupName -Location $config.location `
        -StorageAccountName $StorageAccountName -ContainerName "dsc-package"

    $actualDscPackageZipUrl = Build-DscPackage `
        -DscFolderPath $DscFolderPath `
        -SharedDscPath (Join-Path $PSScriptRoot "..\shared\DSC") `
        -Scenario 'Domain' `
        -BlobName 'Domain-Package.zip' `
        -ConfigJsonParams @{
            Scenario          = 'Domain'
            AdminUsername     = $config.adminUsername
            AdminPassword     = $plainPassword
            DomainName        = $config.domainName
            DomainNetBiosName = $config.domainNetBiosName
            DCExternal1Ip     = $config.dcExternal1Ip
            DCExternal2Ip     = $config.dcExternal2Ip
            SutExternal1Ip    = $config.sutExternal1Ip
            SutExternal2Ip    = $config.sutExternal2Ip
            DriverExternal1Ip = $config.driverExternal1Ip
            DriverExternal2Ip = $config.driverExternal2Ip
            DriverOSType      = $config.driverOsType
            # Create every test account with the single admin password so secondary
            # accounts match the framework's PasswordForAllUsers (works for any chosen
            # password; a no-op when the admin password already matches ParamConfig).
            UnifyAccountPasswords = $true
        } `
        -GenerateConfigScript $generateScript `
        -StorageContext $tempStorage.Context `
        -ContainerName $tempStorage.ContainerName `
        -StorageAccountName $tempStorage.Name `
        -LocalGpoBackupPath (Join-Path $PSScriptRoot "..\..\..\Setup\Scripts\GPOBackup.zip")

} elseif ($DscPackageZipUrl) {
    Write-Output "[OK] Using provided DscPackageZipUrl"
}

# ===========================================================================
# PHASE 1: Deploy Network + Domain Controller
# (Invoke-DeploymentWithSkuFallback pre-validates the template each attempt.)
# ===========================================================================

if (-not $SkipPhase1) {
    Write-Output @"

  PHASE 1: Deploying Network + Domain Controller
  - Virtual Network + Bastion
  - Domain Controller (DC01) with AD DS

"@

    $phase1Start = Get-Date

    # Deployment-time capacity retry: static pre-flight (Resolve-AvailableVmSize)
    # filters Location/Zone restrictions, but dynamic capacity exhaustion is only
    # detected when ARM actually provisions -- Invoke-DeploymentWithSkuFallback
    # walks the candidate list on SkuNotAvailable/AllocationFailed.
    $phase1BaseParams = @{} + $phase1Params
    $phase1BaseParams.Remove('dcVmSize')
    $phase1BaseParams['adminPassword'] = $AdminPassword
    if ($actualDscPackageZipUrl) {
        $phase1BaseParams['domainPackageZipUrl'] = $actualDscPackageZipUrl
    }

    # Preserved for KV outputs (disk encryption)
    $phase1DeploymentResult = Invoke-DeploymentWithSkuFallback `
        -ResourceGroupName $ResourceGroupName `
        -TemplateFile (Join-Path $PSScriptRoot 'phase1.bicep') `
        -BaseParameters $phase1BaseParams `
        -SizeCandidates @{ dcVmSize = $dcCandidates } `
        -DeploymentNamePrefix 'Phase1'

    $phase1Duration = [math]::Round(((Get-Date) - $phase1Start).TotalMinutes, 1)
    Write-Output "`n[OK] Phase 1 completed in $phase1Duration minutes"

    # A Phase-1-only run has no member provisioning to overlap with DC
    # configuration, so retain the original readiness and encryption contract.
    if ($SkipPhase2) {
        if (-not $SkipDCReadyCheck) {
            $dcReady = Wait-ForDomainController `
                -ResourceGroupName $ResourceGroupName `
                -VMNamePattern "*-dc01" `
                -CheckScript "Test-Path 'C:\Domain-Package\DSC\Deploy-DC.Completed.signal'" `
                -TimeoutMinutes $DCReadyTimeoutMinutes `
                -PollIntervalSeconds 30
            if (-not $dcReady) {
                Write-DcDeploymentHeartbeat -ResourceGroupName $ResourceGroupName
                throw "Domain Controller did not signal readiness within $DCReadyTimeoutMinutes minutes."
            }
        }

        if ($diskEncryptionRequested -and $phase1DeploymentResult) {
            $dcVmName = $phase1DeploymentResult.Outputs.dcVmName.Value
            Invoke-DiskEncryptionForVMs -ResourceGroupName $ResourceGroupName `
                -DeploymentOutputs $phase1DeploymentResult.Outputs `
                -VMNames @($dcVmName)
        }

        if ($autoShutdownRequested -and $phase1DeploymentResult) {
            Enable-VmAutoShutdownSchedules -ResourceGroupName $ResourceGroupName `
                -Location $config.location `
                -VMNames @($phase1DeploymentResult.Outputs.dcVmName.Value) `
                -Time $autoShutdownTime -TimeZone $autoShutdownTimeZone
            $autoShutdownRestored = $true
        }
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
            Write-DcDeploymentHeartbeat -ResourceGroupName $ResourceGroupName
            Write-Output @"

Domain Controller does not appear to be ready.
If you are sure the DC is configured, re-run with -SkipDCReadyCheck.
Otherwise, connect to DC via Bastion and check: C:\Domain-Package\DSC\Deploy-DC.log
"@
            exit 1
        }
        Write-Output "[OK] Domain Controller is ready"
    }

    # Clean up computer objects only for member VMs that no longer exist. Removing
    # an object for a surviving domain member immediately breaks its secure channel.
    $resultsStorageAccountName = if ($tempStorage -and $tempStorage.Name) {
        $tempStorage.Name
    } else {
        $StorageAccountName
    }
    if ($SkipPhase1) {
        $envPrefix = if ($phase1Params['environmentPrefix']) { $phase1Params['environmentPrefix'] } else { 'fstest' }
        $dcVmName = "$envPrefix-dc01"
        $memberDefinitions = @(
            [pscustomobject]@{ VMName = "$envPrefix-client01"; ComputerName = 'Client01' }
            [pscustomobject]@{ VMName = "$envPrefix-node01"; ComputerName = 'Node01' }
        )
        $existingMemberVmNames = @(Invoke-AzureOperationWithRetry `
            -OperationName 'List existing Domain member VMs' `
            -Operation { @(Get-AzVM -ResourceGroupName $ResourceGroupName -ErrorAction Stop).Name })
        $staleComputerNames = @($memberDefinitions |
            Where-Object { $_.VMName -notin $existingMemberVmNames } |
            ForEach-Object { $_.ComputerName })

        if ($staleComputerNames.Count -eq 0) {
            Write-Output "`n[OK] Domain member VMs already exist; preserving their computer accounts."
        } else {
            Write-Output "`nCleaning stale computer accounts for missing VMs: $($staleComputerNames -join ', ')..."
            $cleanupJob = $null
            try {
                $cleanupJob = Invoke-AzVMRunCommand -ResourceGroupName $ResourceGroupName `
                    -VMName $dcVmName -CommandId 'RunPowerShellScript' `
                    -ScriptPath (Join-Path $PSScriptRoot 'scripts\Remove-StaleComputerAccounts.ps1') `
                    -Parameter @{ ComputerNamesCsv = ($staleComputerNames -join ',') } `
                    -AsJob -ErrorAction Stop
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
                Write-Warning "If domain join fails, manually remove only the account for the missing VM."
            } finally {
                if ($cleanupJob) {
                    Remove-Job -Job $cleanupJob -Force -ErrorAction SilentlyContinue
                }
            }
        }
    }

    Write-Output @"

  PHASE 2A: Provisioning Domain Member Infrastructure
  - Driver Computer (Client01)
  - SUT Computer (Node01)
  Guest configuration waits for DC readiness

"@

    $phase2Start = Get-Date

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
        $storageAccounts = @(Invoke-AzureOperationWithRetry `
            -OperationName 'Find Domain package storage accounts' `
            -Operation { Get-AzStorageAccount -ResourceGroupName $ResourceGroupName -ErrorAction Stop }) |
            Where-Object { $_.StorageAccountName -like 'fststorage*' }
        foreach ($sa in $storageAccounts) {
            $saCtx = $sa.Context
            # Check both new (dsc-package) and legacy (domain-package) container names
            foreach ($cName in @('dsc-package', 'domain-package')) {
                try {
                    $blob = @(Invoke-AzureOperationWithRetry `
                        -OperationName "Find Domain package in '$($sa.StorageAccountName)/$cName'" `
                        -Operation { Get-AzStorageBlob -Container $cName -Context $saCtx -ErrorAction Stop }) |
                        Where-Object { $_.Name -eq 'Domain-Package.zip' } | Select-Object -First 1
                } catch {
                    $blob = $null
                }
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
            throw 'No reusable Domain-Package.zip was found. Supply -DscPackageZipUrl or -DscFolderPath.'
        }
    }

    # Deployment-time capacity retry for Phase 2 (Driver + SUT): on a capacity
    # error, Invoke-DeploymentWithSkuFallback advances the failing role's
    # candidate (or both, when the error names neither size).
    $phase2BaseParams = @{} + $phase2Params
    $phase2BaseParams.Remove('driverVmSize')
    $phase2BaseParams.Remove('sutVmSize')
    $phase2BaseParams['adminPassword'] = $AdminPassword
    $phase2BaseParams['external1SubnetId'] = $external1SubnetId
    $phase2BaseParams['external2SubnetId'] = $external2SubnetId
    $phase2BaseParams['dcExternal1Ip'] = $dcExternal1Ip
    $phase2BaseParams['dcExternal2Ip'] = $config.dcExternal2Ip
    $phase2BaseParams['configureGuests'] = $false
    if ($actualDscPackageZipUrl) {
        $phase2BaseParams['domainPackageZipUrl'] = $actualDscPackageZipUrl
    }

    Write-Output "Phase 2 parameters:"
    foreach ($k in ($phase2BaseParams.Keys | Sort-Object)) {
        $v = $phase2BaseParams[$k]
        if ($k -match '(?i)password|url|sas') { $v = '***' }
        Write-Output "   $k = $v"
    }

    # Preserved for VM name outputs (disk encryption)
    $phase2DeploymentResult = Invoke-DeploymentWithSkuFallback `
        -ResourceGroupName $ResourceGroupName `
        -TemplateFile (Join-Path $PSScriptRoot 'phase2.bicep') `
        -BaseParameters $phase2BaseParams `
        -SizeCandidates @{ driverVmSize = $driverCandidates; sutVmSize = $sutCandidates } `
        -DeploymentNamePrefix 'Phase2'

    $phase2Duration = [math]::Round(((Get-Date) - $phase2Start).TotalMinutes, 1)
    Write-Output "`n[OK] Phase 2A member infrastructure completed in $phase2Duration minutes"

    # On a fresh deployment, DC post-reboot configuration has been running while
    # member NICs/disks/VMs provisioned. Gate only the guest extensions/domain join.
    if (-not $SkipPhase1 -and -not $SkipDCReadyCheck) {
        Write-Output @"

  Waiting for Domain Controller Configuration
  Member VMs are provisioned; guest configuration remains blocked until DC readiness.

"@

        $dcReady = Wait-ForDomainController `
            -ResourceGroupName $ResourceGroupName `
            -VMNamePattern "*-dc01" `
            -CheckScript "Test-Path 'C:\Domain-Package\DSC\Deploy-DC.Completed.signal'" `
            -TimeoutMinutes $DCReadyTimeoutMinutes `
            -PollIntervalSeconds 30

        if (-not $dcReady) {
            Write-DcDeploymentHeartbeat -ResourceGroupName $ResourceGroupName
            Write-Output @"

Domain Controller did not signal readiness within the timeout.
Member VM infrastructure is preserved, but guest configuration was not started.

Options:
1. Increase timeout with -DCReadyTimeoutMinutes parameter
2. Connect to DC via Bastion and check: C:\Domain-Package\DSC\Deploy-DC.log
3. Resume Phase 2 later once DC is ready:

   .\deploy.ps1 ``
       -SubscriptionId '$SubscriptionId' ``
       -ResourceGroupName '$ResourceGroupName' ``
       -AdminPassword (ConvertTo-SecureString '<password>' -AsPlainText -Force) ``
       -SkipPhase1$(if ($actualDscPackageZipUrl) { " ``
    -DscPackageZipUrl '<signed-package-url>'" })
"@
            exit 1
        }
        Write-Output "[OK] Domain Controller is ready"
    }

    # Deploy only the guest extensions after readiness. This avoids an idempotent
    # second PUT of every NIC/disk/VM and keeps the readiness dependency explicit.
    if ($actualDscPackageZipUrl) {
        Write-Output @"

  PHASE 2B: Starting Domain Member Guest Configuration
  - Domain join and post-join configuration now have a ready DC

"@
        $configurationStart = Get-Date
        $configurationParams = @{
            location            = $config.location
            environmentPrefix   = if ($phase2Params['environmentPrefix']) { $phase2Params['environmentPrefix'] } else { 'fstest' }
            adminPassword       = $AdminPassword
            driverOsType        = $config.driverOsType
            domainPackageZipUrl = $actualDscPackageZipUrl
        }
        New-AzResourceGroupDeployment `
            -ResourceGroupName $ResourceGroupName `
            -Name "Phase2-Configuration-$(Get-Date -Format 'yyyyMMdd-HHmmss')" `
            -TemplateFile (Join-Path $PSScriptRoot 'phase2-configuration.bicep') `
            @configurationParams `
            -ErrorAction Stop | Out-Null
        $configurationDuration = [math]::Round(((Get-Date) - $configurationStart).TotalMinutes, 1)
        Write-Output "[OK] Phase 2B guest configuration initiated in $configurationDuration minutes"
    } else {
        throw 'No Domain-Package URL is available; refusing to leave unconfigured member VMs.'
    }

    if ($SkipPhase1) {
        & "$PSScriptRoot\..\shared\scripts\Verify-Deployment.ps1" `
            -SubscriptionId $SubscriptionId -ResourceGroupName $ResourceGroupName `
            -Scenario Domain -ExpectedRoles 'Domain Controller' -TimeoutMinutes 10 | Out-Null
        $verification = & "$PSScriptRoot\..\shared\scripts\Verify-Deployment.ps1" `
            -SubscriptionId $SubscriptionId -ResourceGroupName $ResourceGroupName `
            -Scenario Domain -ExpectedRoles @('SUT', 'Driver Computer') `
            -TimeoutMinutes 120 -TestTimeoutMinutes $TestTimeoutMinutes `
            -NotBeforeUtc $operationStartUtc -WaitForTests -DeferTestFailure `
            -ResultsStorageAccountName $resultsStorageAccountName
    } else {
        $verification = & "$PSScriptRoot\..\shared\scripts\Verify-Deployment.ps1" `
            -SubscriptionId $SubscriptionId -ResourceGroupName $ResourceGroupName `
            -Scenario Domain -TimeoutMinutes 120 -TestTimeoutMinutes $TestTimeoutMinutes `
            -NotBeforeUtc $operationStartUtc -WaitForTests -DeferTestFailure `
            -ResultsStorageAccountName $resultsStorageAccountName
    }
            Connect-AzureSubscription -SubscriptionId $SubscriptionId | Out-Host

    # ADE can reboot all three machines. Apply it only after DC/member guest
    # configuration and automatic tests have terminal, verified outcomes.
    if ($diskEncryptionRequested -and $phase1Deployment -and $phase2DeploymentResult) {
        $dcVmName     = $phase1Deployment.Outputs.dcVmName.Value
        $driverVmName = $phase2DeploymentResult.Outputs.driverVmName.Value
        $sutVmName    = $phase2DeploymentResult.Outputs.sutVmName.Value

        Invoke-DiskEncryptionForVMs -ResourceGroupName $ResourceGroupName `
            -DeploymentOutputs $phase1Deployment.Outputs `
            -VMNames @($dcVmName, $sutVmName, $driverVmName)

        & "$PSScriptRoot\..\shared\scripts\Verify-Deployment.ps1" `
            -SubscriptionId $SubscriptionId -ResourceGroupName $ResourceGroupName `
            -Scenario Domain -TimeoutMinutes 20 | Out-Null
    }

    if ($autoShutdownRequested) {
        Connect-AzureSubscription -SubscriptionId $SubscriptionId | Out-Host
        Enable-VmAutoShutdownSchedules -ResourceGroupName $ResourceGroupName `
            -Location $config.location `
            -VMNames @(
                $phase1Deployment.Outputs.dcVmName.Value,
                $phase2DeploymentResult.Outputs.sutVmName.Value,
                $phase2DeploymentResult.Outputs.driverVmName.Value
            ) `
            -Time $autoShutdownTime -TimeZone $autoShutdownTimeZone
            $autoShutdownRestored = $true
    }

    Complete-DeploymentTestOutcome -Verification $verification
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
