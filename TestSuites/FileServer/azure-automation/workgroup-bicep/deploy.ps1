# Copyright (c) Microsoft. All rights reserved.
# Licensed under the MIT license. See LICENSE file in the project root for full license information.

# deploy.ps1
# Deployment script for File Server Test Suite - Workgroup Environment
# Single-phase deployment: Network + Driver (Client01) + SUT (Node01)
# No domain controller needed for workgroup scenario

[CmdletBinding()]
param(
    [Parameter(Mandatory=$true)]
    [string]$SubscriptionId,

    [Parameter(Mandatory=$true)]
    [string]$ResourceGroupName,

    [Parameter(Mandatory=$true)]
    [SecureString]$AdminPassword,

    [Parameter(Mandatory=$true)]
    [SecureString]$LocalUserPassword,

    [Parameter(Mandatory=$false)]
    [string]$ParametersFile = "parameters/workgroup.bicepparam",

    [Parameter(Mandatory=$false)]
    [string]$DscFolderPath = "DSC",

    [Parameter(Mandatory=$false)]
    [string]$DscPackageZipUrl = "",

    [Parameter(Mandatory=$false)]
    [string]$StorageAccountName = "",

    [Parameter(Mandatory=$false)]
    [switch]$ValidateOnly,

    [Parameter(Mandatory=$false)]
    [switch]$SkipDiskEncryption,

    [Parameter(Mandatory=$false)]
    [switch]$Resume
)

$ErrorActionPreference = "Stop"

if ($Resume -and $ValidateOnly) {
    Write-Error "-Resume and -ValidateOnly cannot be used together."
    return
}

# Resolve paths relative to script directory
$ParametersFile = if ([System.IO.Path]::IsPathRooted($ParametersFile)) {
    $ParametersFile
} else {
    Join-Path $PSScriptRoot $ParametersFile
}
$templateFile = Join-Path $PSScriptRoot "main.bicep"

Write-Output @"

  File Server Test Suite - Workgroup Deployment
  Single-phase: Network + Driver (Client01) + SUT (Node01)
  No domain controller - workgroup mode

"@

# Import shared helpers
$helpersPath = Join-Path $PSScriptRoot "..\shared\Deploy-Helpers.psm1"
if (-not (Test-Path $helpersPath)) {
    Write-Error "Shared helpers not found at: $helpersPath"
    exit 1
}
Import-Module $helpersPath -Force

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

# Initialize Azure connection
Import-AzureModules
Connect-AzureSubscription -SubscriptionId $SubscriptionId

# Convert passwords securely
$plainPassword = ConvertFrom-SecurePassword -SecurePassword $AdminPassword
$plainLocalUserPassword = ConvertFrom-SecurePassword -SecurePassword $LocalUserPassword

# Parse all parameters from bicepparam file (single source of truth for Config.json
# generation, pre-flight validation, and Bicep deployment).
$templateParams = ConvertFrom-BicepParam -Path $ParametersFile

$config = Resolve-DeploymentConfig -Params $templateParams -Defaults @{
    location          = 'West US 2'
    adminUsername     = 'testadmin'
    driverOsType     = 'Windows'
    sutExternal1Ip   = '192.168.1.11'
    sutExternal2Ip   = '192.168.2.11'
    driverExternal1Ip = '192.168.1.111'
    driverExternal2Ip = '192.168.2.111'
}

# Override enableDiskEncryption if -SkipDiskEncryption was specified
if ($SkipDiskEncryption) {
    $templateParams['enableDiskEncryption'] = $false
}

Write-Output "   Location (from bicepparam): $($config.location)"

# Create or validate the resource group (uses location from bicepparam)
Initialize-ResourceGroup -ResourceGroupName $ResourceGroupName -Location $config.location

$generateScript = Join-Path $PSScriptRoot "..\shared\Generate-ConfigJson.ps1"

# Validate custom images - non-driver VMs require Windows images
if ($templateParams['sutCustomImageId']) {
    Write-Warning "sutCustomImageId is set to a custom image. Ensure it is a Windows-based image -- the SUT VM requires Windows."
}

# Skip pre-flight VM size/image validation and auto-shutdown warning when resuming
# (VMs already exist -- we only need to re-package and reconfigure)
if (-not $Resume) {

# ===========================================================================
# Pre-flight: Validate VM sizes and OS images before creating
# any Azure resources (storage accounts, VMs, etc.)
# ===========================================================================

Write-Output "`nValidating VM sizes and OS images in $config.location..."

# Fetch all VM SKUs for the region (single API call, reused for both checks)
$vmSkus = Get-AzComputeResourceSku -Location $config.location |
    Where-Object { $_.ResourceType -eq 'virtualMachines' }

# Resolve driver VM size (with fallbacks for capacity-constrained regions)
$driverFallbacks = @('Standard_F4s_v2', 'Standard_D4s_v6', 'Standard_D4as_v6', 'Standard_D4s_v5', 'Standard_D4as_v5')
$driverCandidates = Resolve-AvailableVmSize `
    -PreferredSize $templateParams['driverVmSize'] `
    -FallbackSizes $driverFallbacks `
    -AvailableSkus $vmSkus `
    -Role 'Driver' `
    -ReturnAll
$resolvedDriverSize = $driverCandidates[0]
$templateParams['driverVmSize'] = $resolvedDriverSize
Write-Host "   Driver VM size: $resolvedDriverSize$(if ($driverCandidates.Count -gt 1) { " (+$($driverCandidates.Count - 1) fallbacks)" })"

# Resolve SUT VM size
$sutFallbacks = @('Standard_D8s_v6', 'Standard_D8as_v6', 'Standard_D8s_v5', 'Standard_D8as_v5', 'Standard_D8s_v4')
$sutCandidates = Resolve-AvailableVmSize `
    -PreferredSize $templateParams['sutVmSize'] `
    -FallbackSizes $sutFallbacks `
    -AvailableSkus $vmSkus `
    -Role 'SUT' `
    -ReturnAll
$resolvedSutSize = $sutCandidates[0]
$templateParams['sutVmSize'] = $resolvedSutSize
Write-Host "   SUT VM size: $resolvedSutSize$(if ($sutCandidates.Count -gt 1) { " (+$($sutCandidates.Count - 1) fallbacks)" })"

# Validate regional vCPU quota before creating any resources
Test-RegionalVCpuQuota -Location $config.location `
    -VmSizes @{ 'Driver' = $resolvedDriverSize; 'SUT' = $resolvedSutSize } `
    -AvailableSkus $vmSkus

# Validate OS image availability (skip when using custom images)
if (-not $templateParams['driverCustomImageId']) {
    if ($config.driverOsType -eq 'Linux') {
        $driverImgOk = Test-VmImageAvailability -Location $config.location `
            -Publisher 'Canonical' -Offer 'ubuntu-24_04-lts' -Sku $templateParams['driverLinuxOsVersion']
        $driverImgLabel = "Canonical/ubuntu-24_04-lts/$($templateParams['driverLinuxOsVersion'])"
    } else {
        $driverOffer = if ($templateParams['driverOsVersion'] -like 'win10-*') { 'Windows-10' } else { 'Windows-11' }
        $driverImgOk = Test-VmImageAvailability -Location $config.location `
            -Publisher 'MicrosoftWindowsDesktop' -Offer $driverOffer -Sku $templateParams['driverOsVersion']
        $driverImgLabel = "MicrosoftWindowsDesktop/$driverOffer/$($templateParams['driverOsVersion'])"
    }
    if ($driverImgOk) {
        Write-Output "   Driver image: $driverImgLabel"
    } else {
        Write-Error "Driver image '$driverImgLabel' is not available in $config.location. Change driverOsVersion in the bicepparam file or deploy to a different region."
    }
}

if (-not $templateParams['sutCustomImageId']) {
    $sutImgOk = Test-VmImageAvailability -Location $config.location `
        -Publisher 'MicrosoftWindowsServer' -Offer 'WindowsServer' -Sku $templateParams['sutOsVersion']
    $sutImgLabel = "MicrosoftWindowsServer/WindowsServer/$($templateParams['sutOsVersion'])"
    if ($sutImgOk) {
        Write-Output "   SUT image: $sutImgLabel"
    } else {
        Write-Error "SUT image '$sutImgLabel' is not available in $config.location. Change sutOsVersion in the bicepparam file or deploy to a different region."
    }
}

# ===========================================================================
# Advisory: Warn if auto-shutdown is near
# ===========================================================================
if ($templateParams['enableAutoShutdown'] -eq $true) {
    $shutdownTime = $templateParams['autoShutdownTime']       # e.g. '2000'
    $shutdownTz   = $templateParams['autoShutdownTimeZone']   # e.g. 'UTC'
    if ($shutdownTime -and $shutdownTz) {
        try {
            $tzInfo = [System.TimeZoneInfo]::FindSystemTimeZoneById($shutdownTz)
            $nowInTz = [System.TimeZoneInfo]::ConvertTimeFromUtc([DateTime]::UtcNow, $tzInfo)
            # autoShutdownTime is HHmm format (no colon) in workgroup
            $hh = [int]$shutdownTime.Substring(0, [math]::Min(2, $shutdownTime.Length))
            $mm = if ($shutdownTime.Length -gt 2) { [int]$shutdownTime.Substring(2) } else { 0 }
            $shutdownToday = $nowInTz.Date.AddHours($hh).AddMinutes($mm)
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

} # end if (-not $Resume) -- pre-flight validation

# Handle DSC package upload
$actualDscPackageZipUrl = $DscPackageZipUrl
$tempStorage = $null

# Resolve DSC folder path relative to script directory
$DscFolderPath = if ([System.IO.Path]::IsPathRooted($DscFolderPath)) {
    $DscFolderPath
} else {
    Join-Path $PSScriptRoot $DscFolderPath
}

# Wrap packaging + deployment in try/finally so the temporary storage account
# is always cleaned up, even if packaging (Generate-ConfigJson, Compress-Archive,
# Send-BlobWithSasUrl) or deployment fails.
try {

if (-not $DscPackageZipUrl -and (Test-Path $DscFolderPath)) {
    Write-Output "`nPreparing DSC package for upload..."

    $tempStorage = Get-OrCreateStorageAccount `
        -ResourceGroupName $ResourceGroupName -Location $config.location `
        -StorageAccountName $StorageAccountName -ContainerName "dsc-package"

    $actualDscPackageZipUrl = Build-DscPackage `
        -DscFolderPath $DscFolderPath `
        -SharedDscPath (Join-Path $PSScriptRoot "..\shared\DSC") `
        -Scenario 'Workgroup' `
        -BlobName 'Workgroup-Package.zip' `
        -ConfigJsonParams @{
            Scenario          = 'Workgroup'
            AdminUsername     = $config.adminUsername
            AdminPassword     = $plainPassword
            SutExternal1Ip    = $config.sutExternal1Ip
            SutExternal2Ip    = $config.sutExternal2Ip
            DriverExternal1Ip = $config.driverExternal1Ip
            DriverExternal2Ip = $config.driverExternal2Ip
            LocalUserPassword = $plainLocalUserPassword
            DriverOSType      = $config.driverOsType
        } `
        -GenerateConfigScript $generateScript `
        -StorageContext $tempStorage.Context `
        -ContainerName $tempStorage.ContainerName `
        -StorageAccountName $tempStorage.Name

} elseif ($DscPackageZipUrl) {
    Write-Output "[OK] Using provided DscPackageZipUrl"
}

# ===========================================================================
# Resume mode: Reconfigure existing VMs without full Bicep redeployment
# ===========================================================================
if ($Resume) {
    $envPrefix = if ($templateParams['environmentPrefix']) { $templateParams['environmentPrefix'] } else { 'fstest' }
    $driverVmName = "$envPrefix-client01"
    $sutVmName = "$envPrefix-node01"
    $packageName = 'Workgroup-Package'

    Write-Output "`nResume mode: Looking for existing VMs in '$ResourceGroupName'..."

    $vmConfigs = @(
        @{ Name = $sutVmName;    Role = 'SUT';    Script = 'Deploy-SUT.ps1' }
        @{ Name = $driverVmName; Role = 'Driver'; Script = 'Deploy-Driver.ps1' }
    )

    $existingVms = @()
    foreach ($vc in $vmConfigs) {
        $vm = Get-AzVM -ResourceGroupName $ResourceGroupName -Name $vc.Name -ErrorAction SilentlyContinue
        if ($vm) {
            $existingVms += $vc
            Write-Output "   Found $($vc.Role): $($vc.Name)"
        } else {
            Write-Warning "   Not found: $($vc.Name) ($($vc.Role))"
        }
    }

    if ($existingVms.Count -eq 0) {
        Write-Error "No existing VMs found. Run without -Resume for initial deployment."
        return
    }

    # Start VMs if stopped/deallocated
    foreach ($vc in $existingVms) {
        $vmStatus = Get-AzVM -ResourceGroupName $ResourceGroupName -Name $vc.Name -Status
        $powerState = ($vmStatus.Statuses | Where-Object { $_.Code -like 'PowerState/*' }).Code
        if ($powerState -ne 'PowerState/running') {
            Write-Output "   Starting $($vc.Name)..."
            Start-AzVM -ResourceGroupName $ResourceGroupName -Name $vc.Name -ErrorAction Stop | Out-Null
            Write-Output "   [OK] $($vc.Name) started"
        } else {
            Write-Output "   [OK] $($vc.Name) is running"
        }
    }

    # Send resume commands to each VM (parallel via -AsJob)
    Write-Output "`nSending resume commands..."
    $resumeStart = Get-Date
    $resumeJobs = @()

    foreach ($vc in $existingVms) {
        $script = @"
`$ErrorActionPreference = 'Stop'
`$ProgressPreference = 'SilentlyContinue'
Write-Output '=== Resume Configuration ($($vc.Role)) ==='
Write-Output 'Downloading DSC package...'
New-Item -ItemType Directory -Path C:\Temp -Force | Out-Null
Invoke-WebRequest -Uri '$actualDscPackageZipUrl' -OutFile 'C:\Temp\DSC-Package.zip' -UseBasicParsing
Write-Output 'Extracting to C:\$packageName...'
New-Item -ItemType Directory -Path 'C:\$packageName' -Force | Out-Null
Expand-Archive -Path 'C:\Temp\DSC-Package.zip' -DestinationPath 'C:\$packageName' -Force
Remove-Item 'C:\Temp\DSC-Package.zip' -Force
Write-Output 'Clearing signal files and registry state...'
Get-ChildItem 'C:\$packageName' -Filter '*.signal' -Recurse -ErrorAction SilentlyContinue | Remove-Item -Force
Remove-ItemProperty -Path 'HKLM:\SOFTWARE\ProtocolTestSuites' -Name 'DeployStep' -ErrorAction SilentlyContinue
Remove-ItemProperty -Path 'HKLM:\SOFTWARE\ProtocolTestSuites' -Name 'RebootCount' -ErrorAction SilentlyContinue
Write-Output 'Scheduling $($vc.Script)...'
`$a = New-ScheduledTaskAction -Execute 'powershell.exe' -Argument '-ExecutionPolicy Unrestricted -File C:\$packageName\DSC\$($vc.Script) -WorkingPath C:\$packageName'
`$t = New-ScheduledTaskTrigger -Once -At (Get-Date).AddSeconds(10)
`$s = New-ScheduledTaskSettingsSet -AllowStartIfOnBatteries -DontStopIfGoingOnBatteries -StartWhenAvailable
Register-ScheduledTask -TaskName 'ResumeDeployment' -Action `$a -Trigger `$t -Settings `$s -User 'SYSTEM' -RunLevel Highest -Force | Out-Null
Write-Output '=== Resume setup complete ==='
"@
        $job = Invoke-AzVMRunCommand -ResourceGroupName $ResourceGroupName `
            -VMName $vc.Name -CommandId 'RunPowerShellScript' `
            -ScriptString $script -AsJob
        $resumeJobs += @{ Job = $job; VM = $vc }
    }

    # Wait for all RunCommand jobs
    foreach ($rj in $resumeJobs) {
        Write-Output "`n   [$($rj.VM.Role)] Waiting for resume command on $($rj.VM.Name)..."
        try {
            $result = $rj.Job | Receive-Job -Wait -AutoRemoveJob -ErrorAction Stop
            if ($result.Value) {
                foreach ($v in $result.Value) {
                    if ($v.Message) {
                        $v.Message -split "`n" | ForEach-Object { Write-Output "      $_" }
                    }
                }
            }
        } catch {
            Write-Warning "   Resume command failed on $($rj.VM.Name): $($_.Exception.Message)"
            $rj.Job | Remove-Job -Force -ErrorAction SilentlyContinue
        }
    }

    $resumeDuration = [math]::Round(((Get-Date) - $resumeStart).TotalMinutes, 1)

    Write-Output @"

  RESUME INITIATED ($resumeDuration min)

  VMs have received updated packages. Deploy scripts are running in the background.

  Monitor progress:
    - SUT:    RDP/Bastion -> $sutVmName -> C:\$packageName\DSC\Deploy-SUT.log
    - Driver: RDP/Bastion -> $driverVmName -> C:\$packageName\DSC\Deploy-Driver.log

  Completion signals:
    - SUT:    C:\$packageName\DSC\Deploy-SUT.Completed.signal
    - Driver: C:\$packageName\DSC\Deploy-Driver.Completed.signal
"@
    return
}

# ===========================================================================
# Deploy Workgroup Environment
# ===========================================================================

Write-Output @"

  Deploying Workgroup Environment
  - Virtual Network + Bastion
  - Driver Computer (Client01)
  - SUT Computer (Node01) with File Server role

"@

$deployStart = Get-Date

# Add secure overrides to the already-built $templateParams
$templateParams['adminPassword'] = $AdminPassword
if ($actualDscPackageZipUrl) {
    $templateParams['dscPackageZipUrl'] = $actualDscPackageZipUrl
}

# ===========================================================================
# Bicep template validation (dry run)
# ===========================================================================
Write-Output "Validating Bicep template..."
$validationResult = Test-AzResourceGroupDeployment `
    -ResourceGroupName $ResourceGroupName `
    -TemplateFile $templateFile `
    -TemplateParameterObject $templateParams `
    -ErrorAction SilentlyContinue

if ($validationResult) {
    $nonCapacityErrors = $validationResult | Where-Object {
        $_.Code -notmatch 'SkuNotAvailable|ZonalAllocationFailed|AllocationFailed'
    }
    if ($nonCapacityErrors) {
        Write-Error "Template validation failed:`n$($nonCapacityErrors | ForEach-Object { "  - [$($_.Code)] $($_.Message)" } | Out-String)"
        throw "Bicep template validation failed. Fix the errors above before deploying."
    }
    Write-Warning "Template validation returned capacity warnings (SkuNotAvailable) — the deployment retry loop will handle these."
} else {
    Write-Output "[OK] Template validation passed"
}

if ($ValidateOnly) {
    Write-Output "`nValidation-only mode: skipping deployment."
    return
}

# Deployment-time capacity retry for Driver + SUT.
# Static pre-flight (Resolve-AvailableVmSize) filters Location/Zone restrictions,
# but dynamic capacity exhaustion is only detected when ARM actually provisions.
$driverSizeIndex = 0
$sutSizeIndex = 0
$deployed = $false

while (-not $deployed) {
    $driverCurrentSize = $driverCandidates[$driverSizeIndex]
    $sutCurrentSize    = $sutCandidates[$sutSizeIndex]
    $deploymentName    = "Workgroup-$(Get-Date -Format 'yyyyMMdd-HHmmss')"

    $templateParams['driverVmSize'] = $driverCurrentSize
    $templateParams['sutVmSize']    = $sutCurrentSize

    Write-Output "Starting workgroup deployment (Driver: $driverCurrentSize, SUT: $sutCurrentSize)..."

    # Pre-validate to catch SkuNotAvailable before starting the full deployment.
    # ARM validation errors don't produce deployment operations, so Test-CapacityError
    # cannot detect them after the fact.
    $preValidation = Test-AzResourceGroupDeployment `
        -ResourceGroupName $ResourceGroupName `
        -TemplateFile $templateFile `
        -TemplateParameterObject $templateParams
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
            throw "Pre-validation failed (SkuNotAvailable) and no more fallback sizes available:`n  $allMessages"
        } else {
            throw "Pre-validation failed:`n  $allMessages"
        }
    }
    Write-Output "  Pre-validation passed"

    $deploymentJob = New-AzResourceGroupDeployment `
        -ResourceGroupName $ResourceGroupName `
        -Name $deploymentName `
        -TemplateFile $templateFile `
        -TemplateParameterObject $templateParams `
        -ErrorAction Stop `
        -AsJob

    Watch-Deployment -ResourceGroupName $ResourceGroupName -DeploymentName $deploymentName -Job $deploymentJob

    $deployment = $null
    $deployError = $null
    try {
        $deployment = $deploymentJob | Receive-Job -Wait -AutoRemoveJob -ErrorAction Stop
    } catch {
        $deployError = $_
        $deploymentJob | Remove-Job -Force -ErrorAction SilentlyContinue
    }

    $provState = if ($deployment) { $deployment.ProvisioningState } else { 'Failed' }

    if ($provState -eq 'Succeeded' -and -not $deployError) {
        $deployed = $true
    } else {
        $isCapacity = Test-CapacityError -ErrorRecord $deployError `
            -ResourceGroupName $ResourceGroupName -DeploymentName $deploymentName

        $errText = if ($deployError) { "$($deployError.Exception.Message)" } else { '' }
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
            if ($deployError) { throw $deployError }
            throw "Deployment '$deploymentName' finished with state: $provState"
        }
    }
}

$deployDuration = [math]::Round(((Get-Date) - $deployStart).TotalMinutes, 1)
Write-Output "`n[OK] Deployment completed in $deployDuration minutes"

# ===========================================================================
# Disk Encryption — SUT + Driver (after deployment)
# ===========================================================================
if (-not $SkipDiskEncryption -and $deployment) {
    $envPrefix = if ($templateParams['environmentPrefix']) { $templateParams['environmentPrefix'] } else { 'fstest' }
    $sutVmName    = "$envPrefix-node01"
    $driverVmName = "$envPrefix-client01"

    Invoke-DiskEncryptionForVMs -ResourceGroupName $ResourceGroupName `
        -DeploymentOutputs $deployment.Outputs `
        -VMNames @($sutVmName, $driverVmName)
}

# ===========================================================================
# Deployment Complete
# ===========================================================================

Write-Output @"

  DEPLOYMENT COMPLETE

  Your workgroup environment is ready!

VMs deployed:
  - Client01 (Driver Computer) - configured by DSC/Deploy-Driver.ps1
  - Node01   (SUT)             - configured by DSC/Deploy-SUT.ps1

Network Configuration:
  - Client01: External1 = $($config.driverExternal1Ip), External2 = $($config.driverExternal2Ip)
  - Node01:   External1 = $($config.sutExternal1Ip), External2 = $($config.sutExternal2Ip)

What happens next (fully automatic):
1. Both VMs run their deployment scripts in parallel (DSC + tools install)
2. Driver VM waits for SUT readiness, then runs Execute-TestCaseByContext.ps1
3. Results will be written to C:\Test\TestResults\ once complete
4. Signal file: C:\Test\test.finished.signal indicates completion

To monitor progress:
  - RDP/Bastion into Client01 and check C:\Workgroup-Package\DSC\Invoke-TestRun.log

For detailed instructions, see: README.md
"@

} finally {
    # Storage account is kept alive for test results upload.
    if ($tempStorage) {
        Write-Output "`nStorage account '$($tempStorage.Name)' retained for test results upload."
        if ($tempStorage.IsTemporary) {
            Write-Output "   To clean up later: Remove-AzStorageAccount -ResourceGroupName '$ResourceGroupName' -Name '$($tempStorage.Name)' -Force"
        }
    }
    # Clear sensitive variables from memory
    $plainPassword = $null
    $plainLocalUserPassword = $null
}
