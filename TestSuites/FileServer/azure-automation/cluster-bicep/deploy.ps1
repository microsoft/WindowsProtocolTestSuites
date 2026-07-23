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

# Import shared helpers
$helpersPath = Join-Path $PSScriptRoot "..\shared\Deploy-Helpers.psm1"
if (-not (Test-Path $helpersPath)) {
    Write-Error "❌ Shared helpers not found at: $helpersPath"
    exit 1
}
Import-Module $helpersPath -Force

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

Write-Output "   Location (from bicepparam): $($config.location)"

# Create or validate the resource group (uses location from bicepparam)
Initialize-ResourceGroup -ResourceGroupName $ResourceGroupName -Location $config.location

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
    Write-Output "✅ Using provided clusterPackageZipUrl: $ClusterPackageZipUrl"
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
    -TemplateFile 'phase1.bicep' `
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

if ($ValidateOnly) {
    Write-Output "`nValidation-only mode: skipping deployment."
    Write-Output "Note: Phase 2 template depends on Phase 1 outputs and cannot be validated without deploying Phase 1."
    return
}

# ═══════════════════════════════════════════════════════════
# PHASE 1: Deploy Network, Domain Controller, Storage
# ═══════════════════════════════════════════════════════════

try {

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
    $deploymentName = "Phase1-$(Get-Date -Format 'yyyyMMdd-HHmmss')"

    $phase1TemplateParams = @{} + $phase1Params
    $phase1TemplateParams['adminPassword'] = $AdminPassword
    if ($actualClusterPackageZipUrl) {
        $phase1TemplateParams['clusterPackageZipUrl'] = $actualClusterPackageZipUrl
    }

    try {
        Write-Output "🚀 Starting Phase 1 deployment..."
        $deployment = New-AzResourceGroupDeployment `
            -ResourceGroupName $ResourceGroupName `
            -Name $deploymentName `
            -TemplateFile 'phase1.bicep' `
            -TemplateParameterObject $phase1TemplateParams `
            -ErrorAction Stop

        $phase1Duration = [math]::Round(((Get-Date) - $phase1Start).TotalMinutes, 1)
        Write-Output "`n✅ Phase 1 deployment completed in $phase1Duration minutes"
    } catch {
        Write-Error "❌ Phase 1 deployment error: $($_.Exception.Message)"
        exit 1
    }

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
       -ClusterPackageZipUrl '$actualClusterPackageZipUrl'" })
"@
        exit 1
    }

    # ═══════════════════════════════════════════════════════════
    # Disk Encryption — DC + Storage (after DC is ready)
    # ═══════════════════════════════════════════════════════════
    if (-not $SkipDiskEncryption -and $deployment) {
        $envPrefix = if ($phase1Params['environmentPrefix']) { $phase1Params['environmentPrefix'] } else { 'fstest-cluster' }
        $dcVmName      = "$envPrefix-dc01"
        $storageVmName = "$envPrefix-storage01"

        Invoke-DiskEncryptionForVMs -ResourceGroupName $ResourceGroupName `
            -DeploymentOutputs $deployment.Outputs `
            -VMNames @($dcVmName, $storageVmName)
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

    # Clean up stale computer accounts on DC before fresh Phase 2 deployment.
    # When -SkipPhase1 is used, the DC may have leftover computer objects from a
    # previous cluster deployment. Add-Computer on the new VMs can fail with
    # "The account already exists" if these aren't removed first.
    if ($SkipPhase1) {
        Write-Output "`nCleaning up stale computer accounts on DC..."
        $envPrefix = if ($phase1Params['environmentPrefix']) { $phase1Params['environmentPrefix'] } else { 'fstest' }
        $dcVmName = "$envPrefix-dc01"

        # Read machine names and endpoint names from Config.json on the DC.
        # This avoids hardcoding and always matches the previous deployment's config.
        $cleanupScript = @"
Import-Module ActiveDirectory -ErrorAction Stop

# Discover names from the Config.json left by the previous deployment
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

`$staleNames = @()
if (`$config) {
    # Machine computer names (Node01, Node02, Client01, etc.)
    foreach (`$prop in `$config.Machines.PSObject.Properties) {
        if (`$prop.Value.ComputerName) { `$staleNames += `$prop.Value.ComputerName }
    }
    # Cluster endpoint virtual names (Cluster CNO, GeneralFS, ScaleoutFS, InfraFS)
    if (`$config.Endpoints) {
        foreach (`$prop in `$config.Endpoints.PSObject.Properties) {
            if (`$prop.Value.Name) { `$staleNames += `$prop.Value.Name }
        }
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

╔═══════════════════════════════════════════════════════════════╗
║   PHASE 2: Deploying Domain-Joined VMs                       ║
║   - Cluster Node01                                           ║
║   - Cluster Node02                                           ║
║   - Driver Computer (Client01)                               ║
╚═══════════════════════════════════════════════════════════════╝
"@

    $phase2Start = Get-Date
    $deploymentName = "Phase2-$(Get-Date -Format 'yyyyMMdd-HHmmss')"

    # Get Phase 1 outputs
    $phase1Deployment = Get-AzResourceGroupDeployment -ResourceGroupName $ResourceGroupName |
        Where-Object { $_.DeploymentName -like "Phase1-*" } |
        Sort-Object Timestamp -Descending |
        Select-Object -First 1

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
        $storageAccounts = Get-AzStorageAccount -ResourceGroupName $ResourceGroupName -ErrorAction SilentlyContinue |
            Where-Object { $_.StorageAccountName -like '*temp*' -or $_.StorageAccountName -like '*cluster*' }
        foreach ($sa in $storageAccounts) {
            $saCtx = $sa.Context
            $blob = Get-AzStorageBlob -Container 'packages' -Context $saCtx -ErrorAction SilentlyContinue |
                Where-Object { $_.Name -eq 'Cluster-Package.zip' } |
                Select-Object -First 1
            if ($blob) {
                $sasToken = New-AzStorageBlobSASToken -Container 'packages' -Blob $blob.Name `
                    -Permission r -ExpiryTime (Get-Date).AddHours(2) -Context $saCtx -FullUri
                $actualClusterPackageZipUrl = $sasToken
                Write-Output "   ✅ Found existing package blob: $($blob.Name) in $($sa.StorageAccountName)"
                break
            }
        }
        if (-not $actualClusterPackageZipUrl) {
            Write-Output "   ⚠️  No previously uploaded package found. VMs will deploy without package configuration."
            Write-Output "      To include a package, re-run with -ClusterPackageZipUrl or -ClusterPackageZip/-ClusterPackagePath"
        }
    }

    try {
        $phase2TemplateParams = @{} + $phase2Params
        $phase2TemplateParams['adminPassword'] = $AdminPassword
        $phase2TemplateParams['external1SubnetId'] = $external1SubnetId
        $phase2TemplateParams['external2SubnetId'] = $external2SubnetId
        $phase2TemplateParams['dcExternal1Ip'] = $dcExternal1Ip
        if ($actualClusterPackageZipUrl) {
            $phase2TemplateParams['clusterPackageZipUrl'] = $actualClusterPackageZipUrl
        }

        Write-Output "`n🚀 Starting Phase 2 deployment..."
        $deployment = New-AzResourceGroupDeployment `
            -ResourceGroupName $ResourceGroupName `
            -Name $deploymentName `
            -TemplateFile 'phase2.bicep' `
            -TemplateParameterObject $phase2TemplateParams `
            -ErrorAction Stop

        $phase2Duration = [math]::Round(((Get-Date) - $phase2Start).TotalMinutes, 1)
        Write-Output "`n✅ Phase 2 deployment completed in $phase2Duration minutes"
    } catch {
        throw
    }

    # ═══════════════════════════════════════════════════════════
    # Disk Encryption — Nodes + Driver (after Phase 2 deployment)
    # ═══════════════════════════════════════════════════════════
    if (-not $SkipDiskEncryption -and $deployment) {
        # Get KV outputs from Phase 1 deployment
        if (-not $phase1Deployment) {
            $phase1Deployment = Get-AzResourceGroupDeployment -ResourceGroupName $ResourceGroupName |
                Where-Object { $_.DeploymentName -like "Phase1-*" } |
                Sort-Object Timestamp -Descending |
                Select-Object -First 1
        }
        if ($phase1Deployment) {
            $envPrefix = if ($phase1Params['environmentPrefix']) { $phase1Params['environmentPrefix'] } else { 'fstest-cluster' }
            $node01Name = "$envPrefix-node01"
            $node02Name = "$envPrefix-node02"
            $driverName = "$envPrefix-client01"

            Invoke-DiskEncryptionForVMs -ResourceGroupName $ResourceGroupName `
                -DeploymentOutputs $phase1Deployment.Outputs `
                -VMNames @($node01Name, $node02Name, $driverName)
        } else {
            Write-Warning "Phase 1 deployment not found. Skipping Phase 2 disk encryption."
        }
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
& "$PSScriptRoot\scripts\Verify-ClusterDeployment.ps1" -ResourceGroupName $ResourceGroupName -TimeoutMinutes 30

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
