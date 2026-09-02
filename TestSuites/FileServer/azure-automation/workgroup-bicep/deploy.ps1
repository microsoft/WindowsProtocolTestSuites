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

    [Parameter(Mandatory=$false)]
    [string]$Location = 'West US 2',

    [Parameter(Mandatory=$true)]
    [SecureString]$AdminPassword,

    [Parameter(Mandatory=$false)]
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
    [switch]$Resume,

    [Parameter(Mandatory=$false)]
    [ValidateRange(1, 1440)]
    [int]$ConfigurationTimeoutMinutes = 90,

    [Parameter(Mandatory=$false)]
    [ValidateRange(0, 3)]
    [int]$ConfigurationRecoveryAttempts = 1,

    [Parameter(Mandatory=$false)]
    [ValidateRange(1, 1440)]
    [int]$TestTimeoutMinutes = 360,

    [Parameter(Mandatory=$false)]
    [switch]$SkipTestWait,

    [Parameter(Mandatory=$false)]
    [switch]$DeferAutoShutdownRestore
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

Initialize-BicepCli

# Initialize Azure connection
Import-AzureModules
Connect-AzureSubscription -SubscriptionId $SubscriptionId

# Convert passwords securely
$plainPassword = ConvertFrom-SecurePassword -SecurePassword $AdminPassword
$plainLocalUserPassword = if ($LocalUserPassword) {
    ConvertFrom-SecurePassword -SecurePassword $LocalUserPassword
} else {
    $plainPassword
}

# Parse all parameters from bicepparam file (single source of truth for Config.json
# generation, pre-flight validation, and Bicep deployment).
$templateParams = ConvertFrom-BicepParam -Path $ParametersFile

$config = Resolve-DeploymentConfig -Params $templateParams -Defaults @{
    location          = $Location
    adminUsername     = 'testadmin'
    driverOsType     = 'Windows'
    sutExternal1Ip   = '192.168.1.11'
    sutExternal2Ip   = '192.168.2.11'
    driverExternal1Ip = '192.168.1.111'
    driverExternal2Ip = '192.168.2.111'
    enableTestAutoRun = $true
}
$testAutoRun = $config.enableTestAutoRun -ne $false

$autoShutdownRequested = $templateParams['enableAutoShutdown'] -eq $true
$autoShutdownTime = if ($templateParams['autoShutdownTime']) { $templateParams['autoShutdownTime'] } else { '2000' }
$autoShutdownTimeZone = if ($templateParams['autoShutdownTimeZone']) { $templateParams['autoShutdownTimeZone'] } else { 'UTC' }
$templateParams['enableAutoShutdown'] = $false

# Override enableDiskEncryption if -SkipDiskEncryption was specified
if ($SkipDiskEncryption) {
    $templateParams['enableDiskEncryption'] = $false
}

Write-Output "   Location: $($config.location)"

# Post-deploy Azure Disk Encryption runs only when the bicepparam enables it
# (otherwise no Key Vault exists) and -SkipDiskEncryption was not passed.
$diskEncryptionRequested = (-not $SkipDiskEncryption) -and ($templateParams['enableDiskEncryption'] -ne $false)
if ($SkipTestWait -and $testAutoRun -and $diskEncryptionRequested) {
    throw "-SkipTestWait cannot be combined while Azure Disk Encryption is enabled because ADE may reboot a VM during the automatic test run. Also pass -SkipDiskEncryption, or allow the script to wait for tests."
}

$verifyScript = Join-Path $PSScriptRoot '..\shared\scripts\Verify-Deployment.ps1'
function Invoke-WorkgroupVerification {
    [CmdletBinding()]
    param(
        [Parameter(Mandatory)] [datetime]$NotBeforeUtc,
        [switch]$IncludeTests,
        [switch]$DeferTestFailure,
        [int]$VmTimeoutMinutes = $ConfigurationTimeoutMinutes
    )

    $verificationParams = @{
        ResourceGroupName = $ResourceGroupName
        SubscriptionId    = $SubscriptionId
        Scenario          = 'Workgroup'
        TimeoutMinutes    = $VmTimeoutMinutes
        TestTimeoutMinutes = $TestTimeoutMinutes
        NotBeforeUtc      = $NotBeforeUtc
    }
    if ($IncludeTests) {
        $verificationParams['WaitForTests'] = $true
    }
    if ($DeferTestFailure) {
        $verificationParams['DeferTestFailure'] = $true
    }
    if ($tempStorage -and $tempStorage.Name) {
        $verificationParams['ResultsStorageAccountName'] = $tempStorage.Name
    } elseif ($StorageAccountName) {
        $verificationParams['ResultsStorageAccountName'] = $StorageAccountName
    }

    & $verifyScript @verificationParams
}

function Invoke-WorkgroupDiskEncryptionSafely {
    [CmdletBinding()]
    param(
        [Parameter(Mandatory)] $DeploymentOutputs,
        [Parameter(Mandatory)] [string]$SutVmName,
        [Parameter(Mandatory)] [string]$DriverVmName,
        [Parameter(Mandatory)] [datetime]$NotBeforeUtc
    )

    Invoke-DiskEncryptionForVMs -ResourceGroupName $ResourceGroupName `
        -DeploymentOutputs $DeploymentOutputs -VMNames @($SutVmName, $DriverVmName)
    Invoke-WorkgroupVerification -NotBeforeUtc $NotBeforeUtc -VmTimeoutMinutes 20
}

function Test-IsWorkgroupConfigurationFailure {
    param([System.Management.Automation.ErrorRecord]$ErrorRecord)

    if ($null -eq $ErrorRecord) { return $false }
    if ($ErrorRecord.FullyQualifiedErrorId -match 'DeploymentConfiguration(Timeout|Failed)') {
        return $true
    }

    $errorText = @(
        $ErrorRecord.Exception.Message
        $ErrorRecord.ErrorDetails.Message
        $ErrorRecord.Exception.InnerException.Message
    ) -join "`n"
    return $errorText -match "Timeout after \d+ minutes: some 'Workgroup' VMs did not complete configuration" -or
        $errorText -match 'ResumeDeployment task failed'
}

function Write-WorkgroupTimeoutDiagnostics {
    param([string[]]$VMNames)

    Write-Output "`nCollecting bounded Workgroup timeout diagnostics..."
    $diagnosticScript = @'
$packageRoot = 'C:\Workgroup-Package\DSC'
Write-Output "Computer: $env:COMPUTERNAME"
Write-Output "UTC: $([DateTime]::UtcNow.ToString('o'))"
Write-Output '--- Scheduled tasks ---'
foreach ($taskName in @('ResumeDeployment', 'TKFRSAR', 'PostDeployReboot', 'Config-ForceLevel2', 'RunFileServerTests')) {
    $task = Get-ScheduledTask -TaskName $taskName -ErrorAction SilentlyContinue
    $info = if ($task) { Get-ScheduledTaskInfo -TaskName $taskName -ErrorAction SilentlyContinue } else { $null }
    Write-Output "$taskName state=$($task.State) lastResult=$($info.LastTaskResult) lastRun=$($info.LastRunTime)"
}
Write-Output '--- Signals ---'
Get-ChildItem $packageRoot -Filter '*.signal' -File -ErrorAction SilentlyContinue |
    Sort-Object Name | ForEach-Object { Write-Output "$($_.Name) modifiedUtc=$($_.LastWriteTimeUtc.ToString('o'))" }
Write-Output '--- Latest deployment logs (last 40 lines each) ---'
Get-ChildItem $packageRoot -Filter 'Deploy-*.log' -File -ErrorAction SilentlyContinue |
    Sort-Object Name | ForEach-Object {
        Write-Output "=== $($_.Name) ==="
        Get-Content -LiteralPath $_.FullName -Tail 40 -ErrorAction SilentlyContinue
    }
'@

    $diagnosticJobs = @()
    foreach ($vmName in $VMNames) {
        try {
            $job = Invoke-AzVMRunCommand -ResourceGroupName $ResourceGroupName `
                -VMName $vmName -CommandId 'RunPowerShellScript' `
                -ScriptString $diagnosticScript -AsJob -ErrorAction Stop
            $diagnosticJobs += [pscustomobject]@{ VMName = $vmName; Job = $job }
        } catch {
            Write-Warning "Could not submit diagnostics for '$vmName': $($_.Exception.Message)"
        }
    }

    if ($diagnosticJobs.Count -eq 0) { return }
    Wait-Job -Job @($diagnosticJobs.Job) -Timeout 180 | Out-Null
    foreach ($entry in $diagnosticJobs) {
        try {
            if ($entry.Job.State -in @('Running', 'NotStarted')) {
                Stop-Job -Job $entry.Job -ErrorAction SilentlyContinue
                throw 'Diagnostic Run Command exceeded 180 seconds.'
            }
            if ($entry.Job.State -ne 'Completed') {
                throw "Diagnostic Run Command ended in state '$($entry.Job.State)'."
            }
            Write-Output "`n--- $($entry.VMName) diagnostics ---"
            $result = @($entry.Job | Receive-Job -ErrorAction Stop)
            @($result | ForEach-Object { @($_.Value) } | ForEach-Object { $_.Message }) |
                ForEach-Object { Write-Output $_ }
        } catch {
            Write-Warning "Could not collect diagnostics for '$($entry.VMName)': $($_.Exception.Message)"
        } finally {
            Remove-Job -Job $entry.Job -Force -ErrorAction SilentlyContinue
        }
    }
}

function Write-ManualWorkgroupRecoveryCommand {
    Write-Output "`nAutomatic recovery is exhausted. The VMs and logs were retained."
    Write-Output 'Run this bounded manual recovery after reviewing the diagnostics:'
    Write-Output '$password = Read-Host ''Admin password'' -AsSecureString'
    Write-Output "& '$PSCommandPath' -SubscriptionId '$SubscriptionId' -ResourceGroupName '$ResourceGroupName' -AdminPassword `$password -ParametersFile '$ParametersFile' -Resume -ConfigurationRecoveryAttempts 0"
}

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

Write-Output "`nValidating VM sizes and OS images in $($config.location)..."

# Fetch the lightweight regional VM-size snapshot once and reuse it for candidate
# selection and regional quota math. ARM pre-validation below remains authoritative
# for policy, zone restrictions, family quota, and current capacity.
$vmSkus = @(Get-RegionalVmSkuSnapshot -Location $config.location)

# Resolve driver VM size (with fallbacks for capacity-constrained regions).
# The per-role fallback lists are data, not code: ../shared/parameters/VmSizeFallbacks.psd1.
$vmSizeFallbacks = Import-PowerShellDataFile (Join-Path $PSScriptRoot '..\shared\parameters\VmSizeFallbacks.psd1')
$driverCandidates = Resolve-AvailableVmSize `
    -PreferredSize $templateParams['driverVmSize'] `
    -FallbackSizes $vmSizeFallbacks.Driver `
    -AvailableSkus $vmSkus `
    -Role 'Driver' `
    -ReturnAll
$resolvedDriverSize = $driverCandidates[0]
$templateParams['driverVmSize'] = $resolvedDriverSize
Write-Host "   Driver VM size: $resolvedDriverSize$(if ($driverCandidates.Count -gt 1) { " (+$($driverCandidates.Count - 1) fallbacks)" })"

# Resolve SUT VM size
$sutCandidates = Resolve-AvailableVmSize `
    -PreferredSize $templateParams['sutVmSize'] `
    -FallbackSizes $vmSizeFallbacks.SUT `
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
        Write-Error "Driver image '$driverImgLabel' is not available in $($config.location). Change driverOsVersion in the bicepparam file or deploy to a different region."
    }
}

if (-not $templateParams['sutCustomImageId']) {
    $sutImgOk = Test-VmImageAvailability -Location $config.location `
        -Publisher 'MicrosoftWindowsServer' -Offer 'WindowsServer' -Sku $templateParams['sutOsVersion']
    $sutImgLabel = "MicrosoftWindowsServer/WindowsServer/$($templateParams['sutOsVersion'])"
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

# ===========================================================================
# Validation-only gate -- BEFORE any resource creation (resource group, storage
# account, package upload). ARM template validation needs an existing resource
# group, so it runs only when one is already there.
# ===========================================================================
if ($ValidateOnly) {
    if (Get-AzResourceGroup -Name $ResourceGroupName -ErrorAction SilentlyContinue) {
        Write-Output "`nValidating Bicep template against existing resource group..."
        $validationParams = @{} + $templateParams
        $validationParams['adminPassword'] = $AdminPassword

        $validationResult = Test-AzResourceGroupDeployment `
            -ResourceGroupName $ResourceGroupName `
            -TemplateFile $templateFile `
            -TemplateParameterObject $validationParams `
            -ErrorAction SilentlyContinue

        if ($validationResult) {
            # Capacity/SKU errors are handled by the deployment-time retry loop
            $nonCapacityErrors = $validationResult | Where-Object {
                $_.Code -notmatch 'SkuNotAvailable|ZonalAllocationFailed|AllocationFailed'
            }
            if ($nonCapacityErrors) {
                Write-Error "Template validation failed:`n$($nonCapacityErrors | ForEach-Object { "  - [$($_.Code)] $($_.Message)" } | Out-String)"
                exit 1
            }
            Write-Warning "Template validation returned capacity warnings (SkuNotAvailable) -- the deployment retry loop will handle these."
        } else {
            Write-Output "[OK] Template validation passed"
        }
    } else {
        Write-Output "`nResource group '$ResourceGroupName' does not exist yet; skipping ARM template validation (pre-flight SKU/quota/image checks passed)."
    }
    Write-Output "Validation-only mode: no resources were created."
    return
}

# Create or validate the resource group.
Initialize-ResourceGroup -ResourceGroupName $ResourceGroupName -Location $config.location
$envPrefix = if ($templateParams['environmentPrefix']) { $templateParams['environmentPrefix'] } else { 'fstest' }
$autoShutdownVmNames = @("$envPrefix-node01", "$envPrefix-client01")
$autoShutdownRestored = $false

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
            EnableTestAutoRun = $testAutoRun
            # Create every test account with the single admin password so secondary
            # accounts match the framework's PasswordForAllUsers (works for any chosen
            # password; a no-op when the admin password already matches ParamConfig).
            UnifyAccountPasswords = $true
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
    # The on-VM resume script downloads the package from this URL; failing here is
    # clearer than an Invoke-WebRequest error surfacing inside the VM run-command.
    if (-not $actualDscPackageZipUrl) {
        Write-Error "Resume requires a package: no local DSC folder was found to package and -DscPackageZipUrl was not provided."
        return
    }

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
`$downloaded = `$false
for (`$downloadAttempt = 1; `$downloadAttempt -le 5; `$downloadAttempt++) {
    try {
        Invoke-WebRequest -Uri '$actualDscPackageZipUrl' -OutFile 'C:\Temp\DSC-Package.zip' -UseBasicParsing -TimeoutSec 120 -ErrorAction Stop
        `$downloaded = `$true
        break
    } catch {
        if (`$downloadAttempt -eq 5) { throw }
        `$delaySeconds = [math]::Min(10 * [math]::Pow(2, `$downloadAttempt - 1), 60)
        Write-Output "Download attempt `$downloadAttempt failed: `$(`$_.Exception.Message). Retrying in `$delaySeconds seconds."
        Start-Sleep -Seconds `$delaySeconds
    }
}
if (-not `$downloaded) { throw 'DSC package download did not complete.' }
Write-Output 'Stopping stale deployment and test tasks...'
foreach (`$taskName in @('ResumeDeployment', 'TKFRSAR', 'PostDeployReboot', 'Config-ForceLevel2', 'RunFileServerTests')) {
    `$task = Get-ScheduledTask -TaskName `$taskName -ErrorAction SilentlyContinue
    if (`$task) {
        Stop-ScheduledTask -TaskName `$taskName -ErrorAction SilentlyContinue
        Unregister-ScheduledTask -TaskName `$taskName -Confirm:`$false -ErrorAction SilentlyContinue
    }
}
`$allProcesses = @(Get-CimInstance Win32_Process -ErrorAction SilentlyContinue)
`$staleProcessIds = [System.Collections.Generic.HashSet[int]]::new()
foreach (`$process in `$allProcesses) {
    if (`$process.CommandLine -match 'C:\\$packageName\\DSC\\(Deploy-(SUT|Driver)|Scripts\\(Invoke-TestRun|Create-TestAccount))') {
        [void]`$staleProcessIds.Add([int]`$process.ProcessId)
    }
}
do {
    `$addedChild = `$false
    foreach (`$process in `$allProcesses) {
        if (`$staleProcessIds.Contains([int]`$process.ParentProcessId) -and
            -not `$staleProcessIds.Contains([int]`$process.ProcessId)) {
            [void]`$staleProcessIds.Add([int]`$process.ProcessId)
            `$addedChild = `$true
        }
    }
} while (`$addedChild)
foreach (`$processId in @(`$staleProcessIds)) {
    Stop-Process -Id `$processId -Force -ErrorAction SilentlyContinue
}
Write-Output 'Extracting to C:\$packageName...'
New-Item -ItemType Directory -Path 'C:\$packageName' -Force | Out-Null
Expand-Archive -Path 'C:\Temp\DSC-Package.zip' -DestinationPath 'C:\$packageName' -Force
Remove-Item 'C:\Temp\DSC-Package.zip' -Force
Write-Output 'Synchronizing the existing local deployment account credential...'
`$resumeConfig = Get-Content -LiteralPath 'C:\$packageName\Config.json' -Raw -ErrorAction Stop | ConvertFrom-Json
if ([string]::IsNullOrWhiteSpace(`$resumeConfig.Core.Username) -or
    [string]::IsNullOrWhiteSpace(`$resumeConfig.Core.Password)) {
    throw 'The resumed package does not contain a usable local deployment credential.'
}
`$localAdmin = Get-LocalUser -Name `$resumeConfig.Core.Username -ErrorAction Stop
`$resumeAdminPassword = ConvertTo-SecureString `$resumeConfig.Core.Password -AsPlainText -Force
Set-LocalUser -InputObject `$localAdmin -Password `$resumeAdminPassword -ErrorAction Stop
Write-Output 'Local deployment account credential synchronized.'
Write-Output 'Clearing signal files and registry state...'
Get-ChildItem 'C:\$packageName\DSC' -Filter 'Deploy-*.Completed.signal' -Recurse -ErrorAction SilentlyContinue | Remove-Item -Force
Remove-Item 'C:\$packageName\DSC\ForceLevel2.Completed.signal' -Force -ErrorAction SilentlyContinue
Remove-Item 'C:\Test\test.finished.signal', 'C:\Test\test.run.completed.signal', 'C:\Test\test.results.upload.failed.signal' -Force -ErrorAction SilentlyContinue
Remove-ItemProperty -Path 'HKLM:\SOFTWARE\ProtocolTestSuites' -Name 'DeployStep' -ErrorAction SilentlyContinue
Remove-ItemProperty -Path 'HKLM:\SOFTWARE\ProtocolTestSuites' -Name 'RebootCount' -ErrorAction SilentlyContinue
Write-Output 'Scheduling $($vc.Script)...'
`$a = New-ScheduledTaskAction -Execute 'powershell.exe' -Argument '-ExecutionPolicy Unrestricted -File C:\$packageName\DSC\$($vc.Script) -WorkingPath C:\$packageName'
`$t = New-ScheduledTaskTrigger -AtStartup
`$s = New-ScheduledTaskSettingsSet -AllowStartIfOnBatteries -DontStopIfGoingOnBatteries -StartWhenAvailable
Register-ScheduledTask -TaskName 'ResumeDeployment' -Action `$a -Trigger `$t -Settings `$s -User 'SYSTEM' -RunLevel Highest -Force | Out-Null
`$launchCommand = 'powershell.exe -NoProfile -ExecutionPolicy Unrestricted -File "C:\$packageName\DSC\$($vc.Script)" -WorkingPath "C:\$packageName"'
`$launchResult = Invoke-CimMethod -ClassName Win32_Process -MethodName Create -Arguments @{ CommandLine = `$launchCommand } -ErrorAction Stop
if (`$launchResult.ReturnValue -ne 0) {
    throw "Detached deployment launch failed with Win32 return code `$(`$launchResult.ReturnValue)."
}
Write-Output "Detached deployment process started (PID `$(`$launchResult.ProcessId)); startup task retained as reboot fallback."
Write-Output '=== Resume setup complete ==='
"@
        $job = Invoke-AzVMRunCommand -ResourceGroupName $ResourceGroupName `
            -VMName $vc.Name -CommandId 'RunPowerShellScript' `
            -ScriptString $script -AsJob
        $resumeJobs += @{ Job = $job; VM = $vc; Script = $script }
    }

    # Wait for all RunCommand jobs with a per-command bound so one unhealthy VM
    # cannot block the resume operation forever.
    $resumeFailures = @()
    foreach ($rj in $resumeJobs) {
        Write-Output "`n   [$($rj.VM.Role)] Waiting for resume command on $($rj.VM.Name)..."
        $resumeSucceeded = $false
        for ($resumeAttempt = 1; $resumeAttempt -le 3; $resumeAttempt++) {
            try {
                $completedResumeJob = Wait-Job -Job $rj.Job -Timeout 300
                if ($null -eq $completedResumeJob) {
                    Stop-Job -Job $rj.Job -ErrorAction SilentlyContinue
                    throw "Resume Run Command exceeded 300 seconds."
                }
                if ($rj.Job.State -ne 'Completed') {
                    throw "Resume Run Command ended in state '$($rj.Job.State)'."
                }
                $result = $rj.Job | Receive-Job -ErrorAction Stop
                Remove-Job -Job $rj.Job -Force -ErrorAction SilentlyContinue
                $resumeOutput = @($result | ForEach-Object { @($_.Value) } |
                    ForEach-Object { $_.Message }) -join "`n"
                if ($resumeOutput -notmatch '=== Resume setup complete ===') {
                    throw "Resume Run Command did not report successful setup. Output: $resumeOutput"
                }
                if ($result.Value) {
                    foreach ($v in $result.Value) {
                        if ($v.Message) {
                            $v.Message -split "`n" | ForEach-Object { Write-Output "      $_" }
                        }
                    }
                }
                $resumeSucceeded = $true
                break
            } catch {
                $failedAzureJob = $rj.Job.State -eq 'Failed'
                $resumeError = $_
                Remove-Job -Job $rj.Job -Force -ErrorAction SilentlyContinue
                if ($failedAzureJob -and $resumeAttempt -lt 3) {
                    Write-Warning "   Retrying failed resume Run Command on $($rj.VM.Name) (attempt $($resumeAttempt + 1)/3)."
                    Start-Sleep -Seconds 5
                    $rj.Job = Invoke-AzVMRunCommand -ResourceGroupName $ResourceGroupName `
                        -VMName $rj.VM.Name -CommandId 'RunPowerShellScript' `
                        -ScriptString $rj.Script -AsJob -ErrorAction Stop
                    continue
                }
                Write-Warning "   Resume command failed on $($rj.VM.Name): $($resumeError.Exception.Message)"
                break
            }
        }
        if (-not $resumeSucceeded) {
            $resumeFailures += $rj.VM.Name
        }
    }

    if ($resumeFailures.Count -gt 0) {
        throw "Resume commands failed for: $($resumeFailures -join ', ')"
    }

    $resumeDuration = [math]::Round(((Get-Date) - $resumeStart).TotalMinutes, 1)

    $resumeVerification = Invoke-WorkgroupVerification -NotBeforeUtc $resumeStart.ToUniversalTime() `
        -IncludeTests:($testAutoRun -and -not $SkipTestWait) `
        -DeferTestFailure:($testAutoRun -and -not $SkipTestWait)
    Connect-AzureSubscription -SubscriptionId $SubscriptionId | Out-Host

    $resumeInfrastructureFinalizationError = $null
    if ($diskEncryptionRequested) {
        $resumeDeployment = Get-AzResourceGroupDeployment -ResourceGroupName $ResourceGroupName `
            -ErrorAction Stop |
            Where-Object {
                $_.ProvisioningState -eq 'Succeeded' -and $_.Outputs -and
                $_.Outputs.ContainsKey('keyVaultId') -and
                $_.Outputs.ContainsKey('keyVaultUrl')
            } |
            Sort-Object Timestamp -Descending |
            Select-Object -First 1
        if (-not $resumeDeployment) {
            throw "No successful Workgroup deployment with Key Vault outputs was found in '$ResourceGroupName'; disk encryption cannot be finalized."
        }

        try {
            Invoke-WorkgroupDiskEncryptionSafely -DeploymentOutputs $resumeDeployment.Outputs `
                -SutVmName $sutVmName -DriverVmName $driverVmName `
                -NotBeforeUtc $resumeStart.ToUniversalTime()
        } catch {
            $resumeInfrastructureFinalizationError = $_
            Write-Warning "Workgroup resume disk encryption or post-encryption verification failed: $($_.Exception.Message)"
        }
    }

    if ($autoShutdownRequested -and -not $DeferAutoShutdownRestore) {
        Connect-AzureSubscription -SubscriptionId $SubscriptionId | Out-Host
        Enable-VmAutoShutdownSchedules -ResourceGroupName $ResourceGroupName `
            -Location $config.location -VMNames @($sutVmName, $driverVmName) `
            -Time $autoShutdownTime -TimeZone $autoShutdownTimeZone
        $autoShutdownRestored = $true
    }

    if ($resumeInfrastructureFinalizationError) {
        throw "Workgroup resume infrastructure finalization failed after configuration/test finalization; shutdown schedules were restored. $($resumeInfrastructureFinalizationError.Exception.Message)"
    }
    Complete-DeploymentTestOutcome -Verification $resumeVerification

    $resumeTestDetails = if (-not $testAutoRun) {
@"
  Test execution: automatic execution was disabled.
  Start tests manually on ${driverVmName}:
    pwsh -File "C:\$packageName\DSC\Scripts\Invoke-TestRun.ps1" -WorkingPath "C:\$packageName"
"@
    } elseif ($SkipTestWait) {
@"
  Test execution: automatic execution was started without waiting for its terminal outcome.
  Driver log: C:\$packageName\DSC\Invoke-TestRun.log
"@
    } else {
@"
  Test execution completed.
  Classification: $($resumeVerification.TestClassification)
  Passed tests: $($resumeVerification.PassedTestCount)
  Inconclusive tests: $($resumeVerification.InconclusiveTestCount)
  Failed tests: $($resumeVerification.FailedTestCount)
  Results: C:\Test\TestResults\
"@
    }

    Write-Output @"

  RESUME COMPLETE ($resumeDuration min to initiate configuration)

  VMs received updated packages and passed the requested readiness checks.

$resumeTestDetails
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

# Add secure overrides to the already-built $templateParams; the VM-size
# parameters are supplied per-attempt by Invoke-DeploymentWithSkuFallback.
$baseParams = @{} + $templateParams
$baseParams.Remove('driverVmSize')
$baseParams.Remove('sutVmSize')
$baseParams['adminPassword'] = $AdminPassword
if ($actualDscPackageZipUrl) {
    $baseParams['dscPackageZipUrl'] = $actualDscPackageZipUrl
}

# Deployment-time capacity retry for Driver + SUT: static pre-flight
# (Resolve-AvailableVmSize) filters Location/Zone restrictions, but dynamic
# capacity exhaustion is only detected when ARM actually provisions --
# Invoke-DeploymentWithSkuFallback pre-validates each attempt and walks the
# candidate lists on SkuNotAvailable/AllocationFailed.
$deploymentResult = Invoke-DeploymentWithSkuFallback `
    -ResourceGroupName $ResourceGroupName `
    -TemplateFile $templateFile `
    -BaseParameters $baseParams `
    -SizeCandidates @{ driverVmSize = $driverCandidates; sutVmSize = $sutCandidates } `
    -DeploymentNamePrefix 'Workgroup'

$deployDuration = [math]::Round(((Get-Date) - $deployStart).TotalMinutes, 1)
Write-Output "`n[OK] Azure resource deployment completed in $deployDuration minutes"

# The VM extensions can return after scheduling a reboot or the automatic test
# task. Verify fresh guest postconditions before ADE, whose reboot would otherwise
# race with DSC, tool installation, or the test run.
try {
    $verification = Invoke-WorkgroupVerification -NotBeforeUtc $deployStart.ToUniversalTime() `
        -IncludeTests:($testAutoRun -and -not $SkipTestWait) `
        -DeferTestFailure:($testAutoRun -and -not $SkipTestWait)
} catch {
    if ($ConfigurationRecoveryAttempts -gt 0 -and
        (Test-IsWorkgroupConfigurationFailure -ErrorRecord $_)) {
        $envPrefix = if ($templateParams['environmentPrefix']) { $templateParams['environmentPrefix'] } else { 'fstest' }
        $recoveryVmNames = @("$envPrefix-node01", "$envPrefix-client01")
        Write-Warning "Workgroup configuration exceeded $ConfigurationTimeoutMinutes minutes. Starting one automatic reconciliation cycle."
        Write-WorkgroupTimeoutDiagnostics -VMNames $recoveryVmNames

        $recoveryParams = @{} + $PSBoundParameters
        $recoveryParams['Resume'] = $true
        $recoveryParams['ConfigurationRecoveryAttempts'] = 0
        $recoveryParams['DeferAutoShutdownRestore'] = $true
        try {
            & $PSCommandPath @recoveryParams
            if ($autoShutdownRequested) {
                Connect-AzureSubscription -SubscriptionId $SubscriptionId | Out-Host
                Enable-VmAutoShutdownSchedules -ResourceGroupName $ResourceGroupName `
                    -Location $config.location -VMNames $recoveryVmNames `
                    -Time $autoShutdownTime -TimeZone $autoShutdownTimeZone
                $autoShutdownRestored = $true
            }
            Write-Output '[OK] Automatic Workgroup reconciliation completed and passed verification.'
            return
        } catch {
            $recoveryError = $_
            Write-Warning "Automatic Workgroup reconciliation failed: $($recoveryError.Exception.Message)"
            Write-WorkgroupTimeoutDiagnostics -VMNames $recoveryVmNames
            Write-ManualWorkgroupRecoveryCommand
            throw $recoveryError
        }
    } else {
        throw
    }
}
Connect-AzureSubscription -SubscriptionId $SubscriptionId | Out-Host

# ===========================================================================
# Disk Encryption — SUT + Driver (after deployment)
# ===========================================================================
$infrastructureFinalizationError = $null
if ($diskEncryptionRequested -and $deploymentResult) {
    $envPrefix = if ($templateParams['environmentPrefix']) { $templateParams['environmentPrefix'] } else { 'fstest' }
    $sutVmName    = "$envPrefix-node01"
    $driverVmName = "$envPrefix-client01"
    try {
        Invoke-WorkgroupDiskEncryptionSafely -DeploymentOutputs $deploymentResult.Outputs `
            -SutVmName $sutVmName -DriverVmName $driverVmName `
            -NotBeforeUtc $deployStart.ToUniversalTime()
    } catch {
        $infrastructureFinalizationError = $_
        Write-Warning "Workgroup disk encryption or post-encryption verification failed: $($_.Exception.Message)"
    }
}

    if ($autoShutdownRequested) {
        Connect-AzureSubscription -SubscriptionId $SubscriptionId | Out-Host
        $envPrefix = if ($templateParams['environmentPrefix']) { $templateParams['environmentPrefix'] } else { 'fstest' }
        Enable-VmAutoShutdownSchedules -ResourceGroupName $ResourceGroupName `
        -Location $config.location `
        -VMNames @("$envPrefix-node01", "$envPrefix-client01") `
        -Time $autoShutdownTime -TimeZone $autoShutdownTimeZone
        $autoShutdownRestored = $true
    }

if ($infrastructureFinalizationError) {
    throw "Workgroup infrastructure finalization failed after configuration/test finalization; shutdown schedules were restored. $($infrastructureFinalizationError.Exception.Message)"
}
Complete-DeploymentTestOutcome -Verification $verification

$testExecutionDetails = if (-not $testAutoRun) {
    $manualCommand = if ($config.driverOsType -eq 'Linux') {
        'pwsh -File "/opt/Workgroup-Package/DSC/Scripts/Invoke-TestRun.ps1" -WorkingPath "/opt/Workgroup-Package"'
    } else {
        'pwsh -File "C:\Workgroup-Package\DSC\Scripts\Invoke-TestRun.ps1" -WorkingPath "C:\Workgroup-Package"'
    }
@"
Test execution:
  - Automatic FileServer test execution was disabled.
  - Start tests manually on Client01:
    $manualCommand
"@
} elseif ($SkipTestWait) {
@"
Test execution:
  - Automatic FileServer test execution was started.
  - This command was configured not to wait for the terminal test outcome.
  - Log: C:\Workgroup-Package\DSC\Invoke-TestRun.log
"@
} else {
@"
Test execution:
  - Automatic FileServer test execution completed.
  - Classification: $($verification.TestClassification)
  - Passed tests: $($verification.PassedTestCount)
  - Inconclusive tests: $($verification.InconclusiveTestCount)
  - Failed tests: $($verification.FailedTestCount)
  - Results: C:\Test\TestResults\
  - Completion signal: C:\Test\test.finished.signal
"@
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

$testExecutionDetails

For detailed instructions, see: README.md
"@

} finally {
    if ($autoShutdownRequested -and -not $DeferAutoShutdownRestore -and -not $autoShutdownRestored) {
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
    $plainLocalUserPassword = $null
}
