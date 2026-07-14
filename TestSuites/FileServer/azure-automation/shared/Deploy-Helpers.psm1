# Copyright (c) Microsoft. All rights reserved.
# Licensed under the MIT license. See LICENSE file in the project root for full license information.

# Deploy-Helpers.psm1
# Shared deployment helper functions for File Server Test Suite Azure deployments
# Used by both cluster-bicep/deploy.ps1 and domain-bicep/deploy.ps1

function Import-AzureModules {
    [CmdletBinding()]
    param()

    Write-Output "`n📦 Importing Azure PowerShell modules..."
    Import-Module Az.Accounts -Force -ErrorAction Stop
    Import-Module Az.Resources -Force -ErrorAction Stop
    Import-Module Az.Storage -Force -ErrorAction Stop
    Import-Module Az.Compute -Force -ErrorAction Stop
    Write-Output "✅ Azure modules imported"
}

function Connect-AzureSubscription {
    [CmdletBinding()]
    param(
        [Parameter(Mandatory=$true)]
        [string]$SubscriptionId
    )

    Write-Output "`n🔐 Connecting to Azure subscription: $SubscriptionId"
    $context = Get-AzContext
    if (-not $context -or $context.Subscription.Id -ne $SubscriptionId) {
        Connect-AzAccount -SubscriptionId $SubscriptionId -ErrorAction Stop | Out-Null
    }
    Write-Output "✅ Connected to Azure"
}

function Initialize-ResourceGroup {
    [CmdletBinding()]
    param(
        [Parameter(Mandatory=$true)]
        [string]$ResourceGroupName,

        [Parameter(Mandatory=$true)]
        [string]$Location
    )

    Write-Output "`n📁 Verifying resource group: $ResourceGroupName in $Location"
    $rg = Get-AzResourceGroup -Name $ResourceGroupName -ErrorAction SilentlyContinue
    if (-not $rg) {
        New-AzResourceGroup -Name $ResourceGroupName -Location $Location -ErrorAction Stop | Out-Null
        Write-Output "✅ Resource group created"
    } else {
        Write-Output "✅ Resource group exists"
    }
}

function ConvertFrom-SecurePassword {
    [CmdletBinding()]
    param(
        [Parameter(Mandatory=$true)]
        [SecureString]$SecurePassword
    )

    $bstr = [System.Runtime.InteropServices.Marshal]::SecureStringToBSTR($SecurePassword)
    try {
        return [System.Runtime.InteropServices.Marshal]::PtrToStringAuto($bstr)
    } finally {
        [System.Runtime.InteropServices.Marshal]::ZeroFreeBSTR($bstr)
    }
}

function New-TemporaryStorageAccount {
    [CmdletBinding()]
    param(
        [Parameter(Mandatory=$true)]
        [string]$ResourceGroupName,

        [Parameter(Mandatory=$true)]
        [string]$Location,

        [Parameter(Mandatory=$false)]
        [string]$ContainerName = "packages"
    )

    $name = "fststorage$((New-Guid).ToString('N').Substring(0,12))"
    Write-Host "📦 Creating temporary storage account: $name"

    New-AzStorageAccount -ResourceGroupName $ResourceGroupName `
        -Name $name -Location $Location `
        -SkuName "Standard_LRS" -Kind "StorageV2" `
        -AllowBlobPublicAccess $false -ErrorAction Stop | Out-Null

    $ctx = (Get-AzStorageAccount -ResourceGroupName $ResourceGroupName -Name $name).Context
    New-AzStorageContainer -Name $ContainerName -Context $ctx -Permission Off -ErrorAction Stop | Out-Null
    Write-Host "✅ Storage account created with private container '$ContainerName'"

    return @{
        Name = $name
        Context = $ctx
        ContainerName = $ContainerName
    }
}

function Send-BlobWithSasUrl {
    [CmdletBinding()]
    param(
        [Parameter(Mandatory=$true)]
        [string]$FilePath,

        [Parameter(Mandatory=$true)]
        [string]$BlobName,

        [Parameter(Mandatory=$true)]
        [string]$ContainerName,

        [Parameter(Mandatory=$true)]
        $StorageContext,

        [Parameter(Mandatory=$false)]
        [int]$SasExpiryHours = 4
    )

    Write-Host "📤 Uploading $BlobName..."
    $oldProgress = $ProgressPreference
    $ProgressPreference = 'SilentlyContinue'
    try {
        Set-AzStorageBlobContent -File $FilePath -Container $ContainerName `
            -Blob $BlobName -Context $StorageContext -Force -ErrorAction Stop | Out-Null
    } finally {
        $ProgressPreference = $oldProgress
    }

    $sasUrl = New-AzStorageBlobSASToken -Container $ContainerName `
        -Blob $BlobName -Context $StorageContext `
        -Permission r -ExpiryTime (Get-Date).AddHours($SasExpiryHours) -FullUri

    Write-Host "✅ Uploaded (SAS-protected, expires in ${SasExpiryHours}h)"
    return $sasUrl
}

function New-PasswordParameterFile {
    [Diagnostics.CodeAnalysis.SuppressMessageAttribute('PSAvoidUsingPlainTextForPassword', 'Password',
        Justification = 'Accepts already-decrypted password from ConvertFrom-SecurePassword for ARM parameter file generation')]
    [CmdletBinding()]
    param(
        [Parameter(Mandatory=$true)]
        [string]$Password,

        [Parameter(Mandatory=$false)]
        [string]$ClusterPackageZipUrl = ""
    )

    $params = @{
        adminPassword = @{ value = $Password }
    }
    if ($ClusterPackageZipUrl) {
        $params['clusterPackageZipUrl'] = @{ value = $ClusterPackageZipUrl }
    }

    $path = Join-Path $env:TEMP "deploy-params-$((New-Guid).ToString('N')).json"
    @{
        '$schema' = 'https://schema.management.azure.com/schemas/2019-04-01/deploymentParameters.json#'
        contentVersion = '1.0.0.0'
        parameters = $params
    } | ConvertTo-Json -Depth 5 | Set-Content -Path $path -Force

    return $path
}

function Wait-ForDomainController {
    [CmdletBinding()]
    param(
        [Parameter(Mandatory=$true)]
        [string]$ResourceGroupName,

        [Parameter(Mandatory=$false)]
        [string]$VMNamePattern = "*-dc01",

        [Parameter(Mandatory=$true)]
        [string]$CheckScript,

        [Parameter(Mandatory=$false)]
        [int]$TimeoutMinutes = 45,

        [Parameter(Mandatory=$false)]
        [int]$PollIntervalSeconds = 30
    )

    Write-Output "`n⏳ Waiting for Domain Controller to be ready..."
    Write-Output "   Timeout: $TimeoutMinutes minutes"
    Write-Output ""

    $startTime = Get-Date
    $deadline = $startTime.AddMinutes($TimeoutMinutes)

    while ((Get-Date) -lt $deadline) {
        $elapsed = [math]::Round(((Get-Date) - $startTime).TotalMinutes, 1)
        $dcVm = Get-AzVM -ResourceGroupName $ResourceGroupName |
            Where-Object { $_.Name -like $VMNamePattern } |
            Select-Object -First 1

        if ($dcVm) {
            try {
                $result = Invoke-AzVMRunCommand -ResourceGroupName $ResourceGroupName `
                    -VMName $dcVm.Name -CommandId 'RunPowerShellScript' `
                    -ScriptString $CheckScript -ErrorAction Stop

                if ($result.Value[0].Message -match "True") {
                    $duration = [math]::Round(((Get-Date) - $startTime).TotalMinutes, 1)
                    Write-Output "[$elapsed min] ✅ Domain Controller is ready! (took $duration min)"
                    return $true
                }
                Write-Output "[$elapsed min] ⏳ DC still configuring..."
            } catch {
                Write-Output "[$elapsed min] ⚠️  Error checking DC: $($_.Exception.Message)"
            }
        } else {
            Write-Output "[$elapsed min] ⚠️  DC VM not found yet..."
        }

        Start-Sleep -Seconds $PollIntervalSeconds
    }

    Write-Error "❌ Timeout ($TimeoutMinutes min) waiting for Domain Controller"
    return $false
}

function Get-OrCreateStorageAccount {
    <#
    .SYNOPSIS
        Returns a storage context for the given storage account, or creates a temporary one.
    .DESCRIPTION
        If StorageAccountName is provided, uses that existing account.
        Otherwise creates a new temporary storage account.
    .OUTPUTS
        Hashtable with Name, Context, IsTemporary keys.
    #>
    [CmdletBinding()]
    param(
        [Parameter(Mandatory=$true)]
        [string]$ResourceGroupName,

        [Parameter(Mandatory=$true)]
        [string]$Location,

        [Parameter(Mandatory=$false)]
        [string]$StorageAccountName = '',

        [Parameter(Mandatory=$false)]
        [string]$ContainerName = 'packages'
    )

    if ($StorageAccountName) {
        Write-Host "Using existing storage account: $StorageAccountName"
        $sa = Get-AzStorageAccount -ResourceGroupName $ResourceGroupName -Name $StorageAccountName -ErrorAction Stop
        $ctx = $sa.Context

        # Ensure the container exists
        $container = Get-AzStorageContainer -Name $ContainerName -Context $ctx -ErrorAction SilentlyContinue
        if (-not $container) {
            New-AzStorageContainer -Name $ContainerName -Context $ctx -Permission Off -ErrorAction Stop | Out-Null
            Write-Host "   Created container '$ContainerName'"
        }

        return @{
            Name          = $StorageAccountName
            Context       = $ctx
            ContainerName = $ContainerName
            IsTemporary   = $false
        }
    }

    $result = New-TemporaryStorageAccount -ResourceGroupName $ResourceGroupName `
        -Location $Location -ContainerName $ContainerName
    $result['IsTemporary'] = $true
    return $result
}

function New-ResultsUploadConfig {
    <#
    .SYNOPSIS
        Creates a test-results container and generates a write SAS token for uploading test results.
    .OUTPUTS
        Hashtable with StorageAccountName, ContainerName, SasUrl keys,
        suitable for serializing to ResultsUpload.json.
    #>
    [CmdletBinding()]
    param(
        [Parameter(Mandatory=$true)]
        [string]$StorageAccountName,

        [Parameter(Mandatory=$true)]
        $StorageContext,

        [Parameter(Mandatory=$false)]
        [string]$ContainerName = 'test-results',

        [Parameter(Mandatory=$false)]
        [int]$SasExpiryHours = 48
    )

    # Ensure test-results container exists
    $container = Get-AzStorageContainer -Name $ContainerName -Context $StorageContext -ErrorAction SilentlyContinue
    if (-not $container) {
        New-AzStorageContainer -Name $ContainerName -Context $StorageContext -Permission Off -ErrorAction Stop | Out-Null
        Write-Host "   Created container '$ContainerName' for test results"
    }

    # Generate container-level SAS with write+create+list permissions
    $sasToken = New-AzStorageContainerSASToken -Name $ContainerName `
        -Context $StorageContext `
        -Permission rwl `
        -ExpiryTime (Get-Date).AddHours($SasExpiryHours)

    $sasUrl = "$($StorageContext.BlobEndPoint)${ContainerName}${sasToken}"

    Write-Host "   Test results SAS URL generated (expires in ${SasExpiryHours}h)"

    return @{
        StorageAccountName = $StorageAccountName
        ContainerName      = $ContainerName
        SasUrl             = $sasUrl
    }
}

function Remove-TemporaryStorageAccount {
    [CmdletBinding()]
    param(
        [Parameter(Mandatory=$true)]
        [string]$ResourceGroupName,

        [Parameter(Mandatory=$true)]
        [string]$StorageAccountName
    )

    Write-Output "`n🧹 Removing temporary storage account: $StorageAccountName"
    try {
        Remove-AzStorageAccount -ResourceGroupName $ResourceGroupName `
            -Name $StorageAccountName -Force -ErrorAction Stop
        Write-Output "✅ Storage account removed"
    } catch {
        Write-Warning "⚠️  Could not remove storage account: $($_.Exception.Message)"
    }
}

function Resolve-AvailableVmSize {
    <#
    .SYNOPSIS
        Checks if a VM size is available in the given region and falls back to alternatives if not.
    .PARAMETER PreferredSize
        The preferred VM size (e.g., 'Standard_F4as_v6').
    .PARAMETER FallbackSizes
        Ordered list of fallback sizes to try if the preferred size is unavailable.
    .PARAMETER AvailableSkus
        Pre-fetched output of Get-AzComputeResourceSku (filtered to virtualMachines).
        Pass this to avoid repeated API calls when resolving multiple sizes.
    .PARAMETER Role
        Label for log messages (e.g., 'Driver', 'SUT').
    .PARAMETER ReturnAll
        When set, returns ALL statically-valid candidate sizes as an ordered array
        instead of just the first match. Used for deployment-time capacity retry.
    #>
    [CmdletBinding()]
    param(
        [Parameter(Mandatory=$true)]
        [string]$PreferredSize,

        [Parameter(Mandatory=$true)]
        [string[]]$FallbackSizes,

        [Parameter(Mandatory=$true)]
        [array]$AvailableSkus,

        [Parameter(Mandatory=$false)]
        [string]$Role = 'VM',

        [switch]$ReturnAll
    )

    $candidates = @($PreferredSize) + $FallbackSizes
    $validSizes = [System.Collections.Generic.List[string]]::new()

    foreach ($size in $candidates) {
        $sku = $AvailableSkus | Where-Object { $_.Name -eq $size }
        if ($null -eq $sku) { continue }

        # Location-level restrictions (any reason) mean the SKU is completely unavailable.
        $locationBlocked = $sku.Restrictions | Where-Object { $_.Type -eq 'Location' }
        if ($locationBlocked) { continue }

        # Zone-level restrictions only block specific zones. If ALL zones for this
        # region are restricted, the SKU is effectively unavailable.
        $zoneRestrictions = $sku.Restrictions | Where-Object { $_.Type -eq 'Zone' }
        if ($zoneRestrictions) {
            $allZones = ($sku.LocationInfo | ForEach-Object { $_.Zones }) | Sort-Object -Unique
            $blockedZones = ($zoneRestrictions | ForEach-Object { $_.RestrictionInfo.Zones }) | Sort-Object -Unique
            if ($allZones -and $blockedZones -and
                ($allZones | Where-Object { $_ -notin $blockedZones }).Count -eq 0) {
                continue
            }
        }

        $validSizes.Add($size)
    }

    if ($validSizes.Count -eq 0) {
        throw "${Role}: None of the candidate sizes ($($candidates -join ', ')) are available in this region. Change the VM size in the bicepparam file or deploy to a different region."
    }

    if ($ReturnAll) {
        return @($validSizes)
    }

    # Original behavior: return first match with log message
    $resolved = $validSizes[0]
    if ($resolved -ne $PreferredSize) {
        Write-Host "   $Role VM size: $PreferredSize not available, using $resolved"
    } else {
        Write-Host "   $Role VM size: $PreferredSize"
    }
    return $resolved
}

function Test-CapacityError {
    <#
    .SYNOPSIS
        Checks whether a deployment error is a SkuNotAvailable / capacity-restriction
        error that can be retried with a different VM size.
    .PARAMETER ErrorRecord
        The caught ErrorRecord from Receive-Job or deployment failure.
    .PARAMETER ResourceGroupName
        Resource group name (used to query deployment operations for error details).
    .PARAMETER DeploymentName
        Deployment name (used to query deployment operations for error details).
    .OUTPUTS
        [bool] True if the error is a capacity/SKU availability error.
    #>
    [CmdletBinding()]
    param(
        $ErrorRecord,
        [string]$ResourceGroupName,
        [string]$DeploymentName
    )

    # Check the exception message
    $errorText = ''
    if ($ErrorRecord) {
        $errorText = "$($ErrorRecord.Exception.Message) $($ErrorRecord.Exception.InnerException)"
    }

    # Also check deployment operations for SkuNotAvailable details
    if ($ResourceGroupName -and $DeploymentName) {
        try {
            $failedOps = Get-AzResourceGroupDeploymentOperation `
                -ResourceGroupName $ResourceGroupName `
                -DeploymentName $DeploymentName -ErrorAction SilentlyContinue |
                Where-Object { $_.ProvisioningState -eq 'Failed' }
            foreach ($op in $failedOps) {
                if ($op.StatusMessage) {
                    $msg = if ($op.StatusMessage -is [string]) { $op.StatusMessage }
                           else { "$($op.StatusMessage)" }
                    $errorText += " $msg"
                }
            }
        } catch { }
    }

    return $errorText -match 'SkuNotAvailable|Capacity Restriction|not currently available in location|not available in location'
}

function Test-RegionalVCpuQuota {
    <#
    .SYNOPSIS
        Validates that sufficient Total Regional vCPU quota remains for the planned deployment.
    .DESCRIPTION
        Extracts vCPU counts from previously-fetched SKU data and compares the total against
        the subscription's regional core quota (Get-AzVMUsage).  Fails fast with a clear message
        when the deployment would exceed the quota, saving the user from a long deployment that
        ultimately rolls back.
    .PARAMETER Location
        Azure region (e.g. 'westus2').
    .PARAMETER VmSizes
        Hashtable mapping Role label -> resolved VM size name.
        Example: @{ 'Driver' = 'Standard_F4as_v6'; 'SUT' = 'Standard_D8ls_v5' }
    .PARAMETER AvailableSkus
        Pre-fetched output of Get-AzComputeResourceSku (filtered to virtualMachines).
    #>
    [CmdletBinding()]
    param(
        [Parameter(Mandatory=$true)]
        [string]$Location,

        [Parameter(Mandatory=$true)]
        [hashtable]$VmSizes,

        [Parameter(Mandatory=$true)]
        [array]$AvailableSkus
    )

    # Extract vCPU count from SKU capabilities
    $totalNeeded = 0
    $breakdown = @()
    foreach ($entry in $VmSizes.GetEnumerator()) {
        $role = $entry.Key
        $size = $entry.Value
        $sku  = $AvailableSkus | Where-Object { $_.Name -eq $size } | Select-Object -First 1
        $vcpuCap = $sku.Capabilities | Where-Object { $_.Name -eq 'vCPUs' }
        $vcpus = if ($vcpuCap) { [int]$vcpuCap.Value } else { 0 }
        if ($vcpus -eq 0) {
            Write-Warning "Could not determine vCPU count for '$size' ($role) — skipping quota check for this VM."
            continue
        }
        $totalNeeded += $vcpus
        $breakdown += "     $role : $size = $vcpus vCPUs"
    }

    if ($totalNeeded -eq 0) {
        Write-Warning "Unable to determine vCPU requirements; skipping regional quota check."
        return
    }

    # Query subscription regional quota
    $usages = Get-AzVMUsage -Location $Location
    $regionalQuota = $usages | Where-Object { $_.Name.Value -eq 'cores' }
    if (-not $regionalQuota) {
        Write-Warning "Could not retrieve regional vCPU quota for '$Location'; skipping quota check."
        return
    }

    $currentUsage = $regionalQuota.CurrentValue
    $limit        = $regionalQuota.Limit
    $available    = $limit - $currentUsage

    Write-Output "   Regional vCPU quota ($Location):"
    Write-Output "     Limit: $limit, Current Usage: $currentUsage, Available: $available"
    $breakdown | ForEach-Object { Write-Output $_ }
    Write-Output "     Total required: $totalNeeded vCPUs"

    if ($totalNeeded -gt $available) {
        $minLimit = $currentUsage + $totalNeeded
        throw @"
Insufficient regional vCPU quota in '$Location'.
  Available: $available vCPUs (Limit=$limit, Usage=$currentUsage)
  Required : $totalNeeded vCPUs ($($VmSizes.GetEnumerator() | ForEach-Object { "$($_.Key)=$($_.Value)" } | Join-String -Separator ', '))

Request a quota increase to at least $minLimit Total Regional Cores:
  https://aka.ms/ProdportalCRP/#blade/Microsoft_Azure_Capacity/UsageAndQuota.ReactView

Alternatively, reduce VM sizes in the bicepparam file or deploy to a region with more headroom.
"@
    }

    Write-Output "   [OK] Quota sufficient ($totalNeeded of $available available vCPUs)"

    # --- Per-family vCPU quota check ---
    # Reuse $usages (already fetched) to avoid extra API calls.
    foreach ($entry in $VmSizes.GetEnumerator()) {
        $role = $entry.Key
        $size = $entry.Value
        $sku  = $AvailableSkus | Where-Object { $_.Name -eq $size } | Select-Object -First 1
        if (-not $sku) { continue }

        $vcpuCap = $sku.Capabilities | Where-Object { $_.Name -eq 'vCPUs' }
        $vcpus = if ($vcpuCap) { [int]$vcpuCap.Value } else { 0 }
        if ($vcpus -eq 0) { continue }

        # Extract the VM family from SKU capabilities
        $familyCap = $sku.Capabilities | Where-Object { $_.Name -eq 'VMFamily' }
        if (-not $familyCap) {
            # Fallback: try the Family property on the SKU object itself
            $familyName = $sku.Family
        } else {
            $familyName = $familyCap.Value
        }
        if (-not $familyName) {
            Write-Warning "Could not determine VM family for '$size' ($role) -- skipping family quota check."
            continue
        }

        $familyUsage = $usages | Where-Object { $_.Name.Value -eq $familyName }
        if (-not $familyUsage) {
            # Family not in usage list means zero current usage, but also possibly zero limit
            Write-Warning "No quota entry found for family '$familyName' ($size / $role) -- skipping family quota check."
            continue
        }

        $famCurrent   = $familyUsage.CurrentValue
        $famLimit     = $familyUsage.Limit
        $famAvailable = $famLimit - $famCurrent

        if ($vcpus -gt $famAvailable) {
            $famLocalizedName = if ($familyUsage.Name.LocalizedValue) { $familyUsage.Name.LocalizedValue } else { $familyName }
            throw @"
Insufficient '$famLocalizedName' family vCPU quota in '$Location'.
  Available: $famAvailable vCPUs (Limit=$famLimit, Usage=$famCurrent)
  Required : $vcpus vCPUs for $role ($size)

Request a quota increase for the '$famLocalizedName' family:
  https://aka.ms/ProdportalCRP/#blade/Microsoft_Azure_Capacity/UsageAndQuota.ReactView

Alternatively, change the $role VM size in the bicepparam file or deploy to a region with more headroom.
"@
        }

        Write-Output "   [OK] Family quota '$familyName' sufficient for $role ($vcpus of $famAvailable available)"
    }
}

function Test-VmImageAvailability {
    <#
    .SYNOPSIS
        Checks whether a VM image (publisher/offer/sku) is available in the given region.
    .OUTPUTS
        [bool] True if the image is available, False otherwise.
    #>
    [CmdletBinding()]
    param(
        [Parameter(Mandatory=$true)]
        [string]$Location,

        [Parameter(Mandatory=$true)]
        [string]$Publisher,

        [Parameter(Mandatory=$true)]
        [string]$Offer,

        [Parameter(Mandatory=$true)]
        [string]$Sku
    )

    try {
        $images = Get-AzVMImage -Location $Location -PublisherName $Publisher -Offer $Offer -Skus $Sku -ErrorAction Stop
        return ($null -ne $images -and @($images).Count -gt 0)
    } catch {
        return $false
    }
}

function Resolve-DeploymentConfig {
    [CmdletBinding()]
    param(
        [Parameter(Mandatory)]
        [hashtable]$Params,

        [Parameter(Mandatory)]
        [hashtable]$Defaults
    )
    $merged = @{}
    foreach ($key in $Defaults.Keys) {
        $merged[$key] = if ($Params[$key]) { $Params[$key] } else { $Defaults[$key] }
    }
    [PSCustomObject]$merged
}

function ConvertFrom-BicepParam {
    [CmdletBinding()]
    param(
        [Parameter(Mandatory=$true)]
        [string]$Path
    )

    $params = @{}
    foreach ($line in Get-Content $Path) {
        $trimmed = $line.Trim()
        if ($trimmed.StartsWith('//') -or -not $trimmed.StartsWith('param ')) { continue }

        $stringMatch = [regex]::Match($trimmed, "^param\s+(\w+)\s*=\s*'([^']*)'")
        if ($stringMatch.Success) {
            $params[$stringMatch.Groups[1].Value] = $stringMatch.Groups[2].Value
            continue
        }

        $nonStringMatch = [regex]::Match($trimmed, "^param\s+(\w+)\s*=\s*(true|false|\d+)\s*$")
        if ($nonStringMatch.Success) {
            $name = $nonStringMatch.Groups[1].Value
            $value = $nonStringMatch.Groups[2].Value
            $params[$name] = switch ($value) {
                'true' { $true }
                'false' { $false }
                default { [int]$value }
            }
        }
    }
    return $params
}

function Format-IsoDuration {
    param([string]$Iso)
    if (-not $Iso -or $Iso -notmatch '^PT') { return '' }
    $m = [regex]::Match($Iso, 'PT(?:(\d+)H)?(?:(\d+)M)?(?:([\d.]+)S)?')
    if (-not $m.Success) { return $Iso }
    $parts = @()
    if ($m.Groups[1].Success) { $parts += "$($m.Groups[1].Value)h" }
    if ($m.Groups[2].Success) { $parts += "$($m.Groups[2].Value)m" }
    if ($m.Groups[3].Success) { $parts += "$([math]::Round([double]$m.Groups[3].Value))s" }
    return ($parts -join ' ')
}

function Watch-Deployment {
    <#
    .SYNOPSIS
        Monitors an Azure deployment in real-time, printing resource creation events as they happen.
    .DESCRIPTION
        Polls Get-AzResourceGroupDeploymentOperation in a loop, printing a live event log of
        resource creation/completion. Automatically discovers and tracks nested deployments.
    #>
    [CmdletBinding()]
    param(
        [Parameter(Mandatory)] [string]$ResourceGroupName,
        [Parameter(Mandatory)] [string]$DeploymentName,
        [int]$PollIntervalSeconds = 15,
        [int]$TimeoutMinutes = 120,
        $Job = $null
    )

    $startTime = Get-Date
    $deadline = $startTime.AddMinutes($TimeoutMinutes)
    $seenOps = @{}
    $currentInterval = $PollIntervalSeconds
    $nestedNames = [System.Collections.Generic.HashSet[string]]::new([System.StringComparer]::OrdinalIgnoreCase)

    Write-Host "Monitoring deployment '$DeploymentName'..." -ForegroundColor Cyan

    # -- Phase 1: Wait for deployment to appear (up to 60s) --
    # Also monitor the PowerShell job (if provided) so that ARM validation errors
    # surface immediately instead of blocking for the full timeout.
    $waitStart = Get-Date
    $deployment = $null
    while (((Get-Date) - $waitStart).TotalSeconds -lt 60) {
        if ($Job -and $Job.State -eq 'Failed') {
            Write-Host "`nDeployment job failed during ARM validation." -ForegroundColor Red
            return
        }
        try {
            $deployment = Get-AzResourceGroupDeployment -ResourceGroupName $ResourceGroupName `
                -Name $DeploymentName -ErrorAction Stop
            break
        } catch {
            Start-Sleep -Seconds 5
        }
    }
    if (-not $deployment) {
        if ($Job -and $Job.State -eq 'Failed') {
            Write-Host "`nDeployment job failed during ARM validation." -ForegroundColor Red
            return
        }
        Write-Warning "Deployment '$DeploymentName' not found after 60s. Continuing to wait..."
    }

    # Parse a TargetResource value into resource type and name.
    # TargetResource can be either:
    #   - A TargetResource object with .ResourceType and .ResourceName properties (newer Az.Resources)
    #   - A string resource ID like /subscriptions/.../providers/Microsoft.Network/virtualNetworks/vnet
    function Get-ResourceInfo {
        param($TargetResource)
        if (-not $TargetResource) { return $null }

        # Try object properties first (newer Az.Resources returns TargetResource object)
        $resType = $null
        $resName = $null
        try {
            $resType = $TargetResource.ResourceType
            $resName = $TargetResource.ResourceName
        } catch { }

        if ($resType -and $resName) {
            return @{ ResourceType = $resType; ResourceName = $resName }
        }

        # Fall back to parsing resource ID string
        $idStr = "$TargetResource"
        if (-not $idStr -or $idStr -eq '') { return $null }
        # Match .../providers/Microsoft.Foo/barType/baz or .../providers/Microsoft.Foo/barType/baz/childType/childName
        $m = [regex]::Match($idStr, '/providers/(.+)')
        if (-not $m.Success) { return $null }
        $segments = $m.Groups[1].Value -split '/'
        if ($segments.Count -lt 2) { return $null }
        # Build type/name from segments: segments are type1/name1/type2/name2/...
        $types = @()
        $names = @()
        for ($i = 0; $i -lt $segments.Count - 1; $i += 2) {
            $types += $segments[$i]
            if ($i + 1 -lt $segments.Count) { $names += $segments[$i + 1] }
        }
        $resType = $types -join '/'
        $resName = $names -join '/'
        if (-not $resType -or -not $resName) { return $null }
        return @{ ResourceType = $resType; ResourceName = $resName }
    }

    # Helper to print a single operation event
    function Write-OperationEvent {
        param($Op)
        $opId = $Op.OperationId
        $state = $Op.ProvisioningState
        if (-not $opId -or -not $state) { return }

        $lastState = $seenOps[$opId]
        if ($lastState -eq $state) { return }
        $seenOps[$opId] = $state

        $info = Get-ResourceInfo $Op.TargetResource
        if (-not $info) { return }
        $resType = $info.ResourceType
        $resName = $info.ResourceName

        $ts = (Get-Date).ToString('HH:mm:ss')
        $duration = Format-IsoDuration $Op.Duration
        $durStr = if ($duration) { "  ($duration)" } else { '' }

        $color = switch ($state) {
            'Succeeded' { 'Green' }
            'Failed'    { 'Red' }
            'Running'   { 'Yellow' }
            default     { 'Gray' }
        }

        $pad = $state.PadRight(10)
        Write-Host "[$ts]  " -NoNewline
        Write-Host $pad -ForegroundColor $color -NoNewline
        Write-Host " ${resType}/${resName}${durStr}"

        # Print error details on failure
        if ($state -eq 'Failed') {
            $errMsg = $null
            try {
                if ($Op.StatusMessage -and $Op.StatusMessage.error) {
                    $errMsg = $Op.StatusMessage.error.message
                } elseif ($Op.StatusMessage -is [string]) {
                    $parsed = $Op.StatusMessage | ConvertFrom-Json -ErrorAction SilentlyContinue
                    if ($parsed.error.message) { $errMsg = $parsed.error.message }
                }
            } catch { }
            if ($errMsg) {
                Write-Host "           Error: $errMsg" -ForegroundColor Red
            }
        }

        # Track nested deployments
        if ($resType -eq 'Microsoft.Resources/deployments' -and $resName) {
            [void]$nestedNames.Add($resName)
        }
    }

    # -- Phase 2: Main polling loop --
    $pollCount = 0
    while ((Get-Date) -lt $deadline) {
        $pollCount++
        # Early exit if the deployment job failed (e.g., ARM validation error)
        if ($Job -and $Job.State -eq 'Failed') {
            Write-Host "`nDeployment job failed." -ForegroundColor Red
            return
        }
        try {
            $deployment = Get-AzResourceGroupDeployment -ResourceGroupName $ResourceGroupName `
                -Name $DeploymentName -ErrorAction Stop
        } catch {
            Start-Sleep -Seconds $currentInterval
            continue
        }

        $eventsBefore = $seenOps.Count

        # Get top-level operations
        try {
            $ops = @(Get-AzResourceGroupDeploymentOperation -ResourceGroupName $ResourceGroupName `
                -DeploymentName $DeploymentName -ErrorAction Stop)
            $currentInterval = $PollIntervalSeconds  # reset backoff on success

            foreach ($op in $ops) {
                Write-OperationEvent $op
            }
        } catch {
            if ($_.Exception.Message -match '429' -or $_.Exception.Message -match 'throttl') {
                $currentInterval = [math]::Min($currentInterval * 2, 120)
                Write-Host "[$(Get-Date -Format 'HH:mm:ss')]  Throttled, backing off to ${currentInterval}s" -ForegroundColor DarkYellow
            }
        }

        # Get nested deployment operations
        foreach ($nested in @($nestedNames)) {
            try {
                $nestedOps = @(Get-AzResourceGroupDeploymentOperation -ResourceGroupName $ResourceGroupName `
                    -DeploymentName $nested -ErrorAction Stop)
                foreach ($op in $nestedOps) {
                    Write-OperationEvent $op
                }
            } catch {
                # Nested deployment may not be queryable yet; ignore
            }
        }

        # Check terminal state
        $provState = $deployment.ProvisioningState
        if ($provState -in @('Succeeded', 'Failed', 'Canceled')) {
            break
        }

        # Periodic heartbeat when no new events were printed
        if ($seenOps.Count -eq $eventsBefore -and ($pollCount % 4) -eq 0) {
            $elapsed = [math]::Round(((Get-Date) - $startTime).TotalMinutes, 1)
            Write-Host "[$(Get-Date -Format 'HH:mm:ss')]  Waiting... ($elapsed min elapsed, $($seenOps.Count) resources tracked)" -ForegroundColor DarkGray
        }

        Start-Sleep -Seconds $currentInterval
    }

    # -- Phase 3: Final summary --
    $totalMin = [math]::Round(((Get-Date) - $startTime).TotalMinutes, 1)
    $finalState = if ($deployment) { $deployment.ProvisioningState } else { 'Unknown' }
    $color = switch ($finalState) {
        'Succeeded' { 'Green' }
        'Failed'    { 'Red' }
        default     { 'Yellow' }
    }
    Write-Host ""
    Write-Host "[$(Get-Date -Format 'HH:mm:ss')]  ** Deployment '$DeploymentName' $($finalState.ToLower()) in $totalMin min **" -ForegroundColor $color
}

function Enable-VmDiskEncryption {
    <#
    .SYNOPSIS
        Enables Azure Disk Encryption on a VM using a Key Vault.
    .DESCRIPTION
        Waits for the VM to be in a running state, then applies
        AzureDiskEncryption for the OS volume. Mirrors the pipeline's
        sequential disk encryption step.
    .PARAMETER ResourceGroupName
        Azure resource group containing the VM.
    .PARAMETER VMName
        Name of the VM to encrypt.
    .PARAMETER KeyVaultUrl
        Key Vault URL (vaultUri) for disk encryption keys.
    .PARAMETER KeyVaultId
        Key Vault resource ID.
    #>
    [CmdletBinding()]
    param(
        [Parameter(Mandatory)]
        [string]$ResourceGroupName,

        [Parameter(Mandatory)]
        [string]$VMName,

        [Parameter(Mandatory)]
        [string]$KeyVaultUrl,

        [Parameter(Mandatory)]
        [string]$KeyVaultId
    )

    Write-Output "   Encrypting VM '$VMName'..."

    # Wait for VM to be in running state (may be rebooting from TKFRSAR)
    $maxWaitSeconds = 300
    $waited = 0
    while ($waited -lt $maxWaitSeconds) {
        $vmStatus = Get-AzVM -ResourceGroupName $ResourceGroupName -Name $VMName -Status -ErrorAction SilentlyContinue
        $powerState = ($vmStatus.Statuses | Where-Object { $_.Code -match 'PowerState' }).Code
        if ($powerState -eq 'PowerState/running') { break }
        Write-Output "     VM '$VMName' state: $powerState — waiting..."
        Start-Sleep 15
        $waited += 15
    }
    if ($waited -ge $maxWaitSeconds) {
        Write-Warning "VM '$VMName' not running after ${maxWaitSeconds}s. Skipping disk encryption."
        return $false
    }

    try {
        Set-AzVMDiskEncryptionExtension -ResourceGroupName $ResourceGroupName `
            -VMName $VMName `
            -DiskEncryptionKeyVaultUrl $KeyVaultUrl `
            -DiskEncryptionKeyVaultId $KeyVaultId `
            -VolumeType 'OS' `
            -SkipVmBackup `
            -Force `
            -ErrorAction Stop | Out-Null
        Write-Output "   [OK] Disk encryption enabled for '$VMName'"

        # ADE reboots the VM to finalize encryption. Wait for it to come back
        # so that callers (e.g., deploy.ps1) don't proceed while the VM is down.
        Write-Output "   Waiting for '$VMName' to finish post-encryption reboot..."
        $postWait = 0
        $postMax  = 300
        Start-Sleep 15  # brief grace period for reboot to initiate
        while ($postWait -lt $postMax) {
            $vmStatus = Get-AzVM -ResourceGroupName $ResourceGroupName -Name $VMName -Status -ErrorAction SilentlyContinue
            $powerState = ($vmStatus.Statuses | Where-Object { $_.Code -match 'PowerState' }).Code
            if ($powerState -eq 'PowerState/running') {
                Write-Output "   [OK] '$VMName' is running after encryption"
                break
            }
            Start-Sleep 15
            $postWait += 15
        }
        if ($postWait -ge $postMax) {
            Write-Warning "'$VMName' not running after post-encryption wait (${postMax}s). Continuing anyway."
        }

        return $true
    }
    catch {
        Write-Warning "Disk encryption failed for '$VMName': $($_.Exception.Message)"
        return $false
    }
}

function Invoke-DiskEncryptionForVMs {
    <#
    .SYNOPSIS
        Encrypts OS disks for a list of VMs using Key Vault info from deployment outputs.
    .PARAMETER ResourceGroupName
        Azure resource group containing the VMs and Key Vault.
    .PARAMETER DeploymentOutputs
        The .Outputs hashtable from Get-AzResourceGroupDeployment (must contain
        keyVaultId and keyVaultUrl).
    .PARAMETER VMNames
        Array of VM names to encrypt.
    #>
    [CmdletBinding()]
    param(
        [Parameter(Mandatory)]
        [string]$ResourceGroupName,

        [Parameter(Mandatory)]
        $DeploymentOutputs,

        [Parameter(Mandatory)]
        [string[]]$VMNames
    )

    $kvId  = $DeploymentOutputs.keyVaultId.Value
    $kvUrl = $DeploymentOutputs.keyVaultUrl.Value

    if (-not $kvId -or -not $kvUrl) {
        Write-Warning "Key Vault outputs not found in deployment. Skipping disk encryption."
        return
    }

    Write-Output "`n  Encrypting VM disks..."
    foreach ($vmName in $VMNames) {
        Enable-VmDiskEncryption -ResourceGroupName $ResourceGroupName `
            -VMName $vmName -KeyVaultUrl $kvUrl -KeyVaultId $kvId
    }
}

function Install-DscPackageAssets {
    <#
    .SYNOPSIS
        Downloads external assets (ParamConfig.json, GPOBackup.zip) into the
        DSC\Scripts folder of a deployment package.

    .DESCRIPTION
        Some files cannot live in the repo because they contain passwords
        (ParamConfig.json) or are large binaries (GPOBackup.zip).  They are
        hosted in a public Azure Front Door storage account and downloaded at
        packaging time.

        GPOBackup.zip has a local fallback path for offline / dev builds.
        ParamConfig.json is download-only (contains secrets).

    .PARAMETER ScriptsFolder
        Absolute path to the DSC\Scripts folder inside the temp package.

    .PARAMETER Scenario
        Domain, Cluster, or Workgroup.  GPOBackup.zip is only needed for
        Domain and Cluster (DC configuration).

    .PARAMETER LocalGpoBackupPath
        Optional local path to GPOBackup.zip (checked before downloading).
    #>
    [CmdletBinding()]
    param(
        [Parameter(Mandatory)]
        [string]$ScriptsFolder,

        [Parameter(Mandatory)]
        [ValidateSet('Domain', 'Cluster', 'Workgroup')]
        [string]$Scenario,

        [string]$LocalGpoBackupPath = ''
    )

    $baseUrl = 'https://ptsresources-czfwdxa0fdbychcp.b01.azurefd.net'

    # -- ParamConfig.json (all scenarios -- Create-TestAccount.ps1 needs it) --
    $paramConfigDest = Join-Path $ScriptsFolder 'ParamConfig.json'
    try {
        Invoke-WebRequest -Uri "$baseUrl/configs/ParamConfig.json" `
            -OutFile $paramConfigDest -UseBasicParsing
        Write-Host "   [OK] Downloaded ParamConfig.json to DSC\Scripts"
    } catch {
        Write-Warning "Failed to download ParamConfig.json: $_"
    }

    # -- GPOBackup.zip (Domain / Cluster only -- needed for Import-GPOForClaims.ps1) --
    if ($Scenario -in @('Domain', 'Cluster')) {
        $gpoBackupDest = Join-Path $ScriptsFolder 'GPOBackup.zip'
        if ($LocalGpoBackupPath -and (Test-Path $LocalGpoBackupPath)) {
            Copy-Item $LocalGpoBackupPath -Destination $gpoBackupDest -Force
            Write-Host "   [OK] Copied GPOBackup.zip to DSC\Scripts (local)"
        } else {
            try {
                Invoke-WebRequest -Uri "$baseUrl/utils/GPOBackup.zip" `
                    -OutFile $gpoBackupDest -UseBasicParsing
                Write-Host "   [OK] Downloaded GPOBackup.zip to DSC\Scripts"
            } catch {
                Write-Warning "GPOBackup.zip not found locally and download failed: $_"
            }
        }
    }
}

function Build-DscPackage {
    <#
    .SYNOPSIS
        Assembles a DSC deployment package from a scenario-specific DSC folder,
        overlays shared scripts, generates Config.json, and uploads to Azure Storage.

    .DESCRIPTION
        All three deployment scenarios (Domain, Cluster, Workgroup) follow the same
        packaging pattern:
          1. Copy local DSC folder into a temp package directory
          2. Overlay shared DSC scripts (shared is the source of truth)
          3. Download external assets (GPOBackup.zip, ParamConfig.json)
          4. Generate Config.json from deployment parameters
          5. Generate ResultsUpload.json for test result collection
          6. Zip the package and upload to Azure Storage
          7. Return a SAS URL for the uploaded blob

        This function extracts that common logic so each deploy.ps1 only
        provides the scenario-specific parameters.

    .PARAMETER DscFolderPath
        Absolute path to the scenario-specific DSC folder (e.g., domain-bicep/DSC).

    .PARAMETER SharedDscPath
        Absolute path to the shared DSC folder (shared/DSC).

    .PARAMETER Scenario
        Deployment scenario: Domain, Cluster, or Workgroup.

    .PARAMETER BlobName
        Name for the uploaded zip blob (e.g., "Domain-Package.zip").

    .PARAMETER ConfigJsonParams
        Hashtable of parameters to splat to Generate-ConfigJson.ps1.
        The function adds -OutputPath automatically.

    .PARAMETER GenerateConfigScript
        Absolute path to Generate-ConfigJson.ps1.

    .PARAMETER StorageContext
        Azure Storage context for uploading the package.

    .PARAMETER ContainerName
        Blob container name for the upload.

    .PARAMETER StorageAccountName
        Storage account name (used for ResultsUpload.json).

    .PARAMETER LocalGpoBackupPath
        Optional local path to GPOBackup.zip. Only used for Domain/Cluster.
    #>
    [CmdletBinding()]
    param(
        [Parameter(Mandatory)]
        [string]$DscFolderPath,

        [Parameter(Mandatory)]
        [string]$SharedDscPath,

        [Parameter(Mandatory)]
        [ValidateSet('Domain', 'Cluster', 'Workgroup')]
        [string]$Scenario,

        [Parameter(Mandatory)]
        [string]$BlobName,

        [Parameter(Mandatory)]
        [hashtable]$ConfigJsonParams,

        [Parameter(Mandatory)]
        [string]$GenerateConfigScript,

        [Parameter(Mandatory)]
        $StorageContext,

        [Parameter(Mandatory)]
        [string]$ContainerName,

        [Parameter(Mandatory)]
        [string]$StorageAccountName,

        [string]$LocalGpoBackupPath = ''
    )

    # Use Write-Host for progress messages inside this function so they don't
    # pollute the return pipeline. The only value on the output stream must be
    # the SAS URL returned at the end.

    # Generate ResultsUpload.json config (SAS-based write token for test results).
    $resultsConfig = New-ResultsUploadConfig `
        -StorageAccountName $StorageAccountName `
        -StorageContext $StorageContext

    # Assemble + zip the package. New-DscPackageZip holds the shared packaging
    # logic reused by Publish-DscPackage.ps1 (public-blob publishing).
    $tempZipPath = Join-Path $env:TEMP "DscPackage-$(Get-Random).zip"
    New-DscPackageZip `
        -DscFolderPath $DscFolderPath `
        -SharedDscPath $SharedDscPath `
        -Scenario $Scenario `
        -ConfigJsonParams $ConfigJsonParams `
        -GenerateConfigScript $GenerateConfigScript `
        -ResultsUploadConfig $resultsConfig `
        -LocalGpoBackupPath $LocalGpoBackupPath `
        -OutputZipPath $tempZipPath

    $sasUrl = Send-BlobWithSasUrl `
        -FilePath $tempZipPath -BlobName $BlobName `
        -ContainerName $ContainerName -StorageContext $StorageContext

    # Cleanup temp files
    Remove-Item $tempZipPath -Force -ErrorAction SilentlyContinue

    return $sasUrl
}

function New-DscPackageZip {
    <#
    .SYNOPSIS
        Assembles a DSC deployment package and compresses it to a zip file.

    .DESCRIPTION
        Pure packaging step with no Azure dependency. Follows the same pattern as
        Build-DscPackage (copy DSC folder, overlay shared scripts, download external
        assets, generate Config.json, copy Tools.json) but stops at a local zip file.

        Shared by:
          * Build-DscPackage       -- then uploads via SAS and returns a SAS URL.
          * Publish-DscPackage.ps1 -- then uploads the zip to a public blob.

        ResultsUpload.json is written only when -ResultsUploadConfig is supplied.
        Publicly hosted packages MUST omit it, because it embeds a write-capable
        SAS token that must never be published.

    .PARAMETER ResultsUploadConfig
        Optional hashtable (as returned by New-ResultsUploadConfig) serialized to
        ResultsUpload.json at the package root. Omit for public packages.

    .PARAMETER OutputZipPath
        Absolute path of the zip file to create (overwritten if it exists).
    #>
    [CmdletBinding()]
    param(
        [Parameter(Mandatory)]
        [string]$DscFolderPath,

        [Parameter(Mandatory)]
        [string]$SharedDscPath,

        [Parameter(Mandatory)]
        [ValidateSet('Domain', 'Cluster', 'Workgroup')]
        [string]$Scenario,

        [Parameter(Mandatory)]
        [hashtable]$ConfigJsonParams,

        [Parameter(Mandatory)]
        [string]$GenerateConfigScript,

        [hashtable]$ResultsUploadConfig,

        [string]$LocalGpoBackupPath = '',

        [Parameter(Mandatory)]
        [string]$OutputZipPath
    )

    Write-Host "   Building DSC package from: $DscFolderPath"

    # Create temp directory for packaging (root = WorkingPath on the VM)
    $tempPackagePath = Join-Path $env:TEMP "DscPackage-$(Get-Random)"
    New-Item -ItemType Directory -Path $tempPackagePath -Force | Out-Null

    try {
        # Copy source into the package. If the source IS a DSC folder (leaf name "DSC"),
        # nest it as a subdirectory. If it's a full package directory (already contains
        # a DSC/ subfolder), copy its contents directly.
        $isDscFolder = (Split-Path $DscFolderPath -Leaf) -eq 'DSC'
        if ($isDscFolder) {
            $dscDestination = Join-Path $tempPackagePath "DSC"
            Copy-Item -Path $DscFolderPath -Destination $dscDestination -Recurse -Force
        } else {
            Copy-Item -Path $DscFolderPath -Destination $tempPackagePath -Recurse -Force
            $dscDestination = Join-Path $tempPackagePath "DSC"
        }
        Write-Host "   [OK] Copied source to package"

        # Ensure DSC\Scripts target directory exists
        $dscScriptsTarget = Join-Path $dscDestination "Scripts"
        if (-not (Test-Path $dscScriptsTarget)) {
            New-Item -ItemType Directory -Path $dscScriptsTarget -Force | Out-Null
        }

        # Overlay shared DSC files into package (shared is the source of truth)
        if (Test-Path $SharedDscPath) {
            # Root-level scripts (Deploy-DC.ps1, DC-Configuration.ps1, etc.)
            foreach ($sharedFile in (Get-ChildItem -Path $SharedDscPath -Filter '*.ps1' -File)) {
                Copy-Item -Path $sharedFile.FullName -Destination $dscDestination -Force
            }
            Write-Host "   [OK] Overlaid shared DSC root scripts"

            # Scripts/ subfolder
            $sharedScriptsPath = Join-Path $SharedDscPath "Scripts"
            if (Test-Path $sharedScriptsPath) {
                Copy-Item -Path "$sharedScriptsPath\*" -Destination $dscScriptsTarget -Recurse -Force
                Write-Host "   [OK] Overlaid shared DSC/Scripts"
            }
        } else {
            Write-Warning "Shared DSC folder not found at $SharedDscPath -- package may be incomplete"
        }

        # Download external assets (GPOBackup.zip, ParamConfig.json)
        $assetParams = @{
            ScriptsFolder = $dscScriptsTarget
            Scenario      = $Scenario
        }
        if ($LocalGpoBackupPath) {
            $assetParams['LocalGpoBackupPath'] = $LocalGpoBackupPath
        }
        Install-DscPackageAssets @assetParams

        # Copy Tools.json to package root
        $toolsSource = Join-Path $dscScriptsTarget "Tools.json"
        if (Test-Path $toolsSource) {
            Copy-Item -Path $toolsSource -Destination (Join-Path $tempPackagePath "Tools.json") -Force
            Write-Host "   [OK] Copied Tools.json to package root"
        } else {
            Write-Warning "Tools.json not found at $toolsSource"
        }

        # Generate Config.json at package root
        Write-Host "   Generating Config.json..."
        $configJsonParams['OutputPath'] = Join-Path $tempPackagePath "Config.json"
        & $GenerateConfigScript @configJsonParams | Out-Null
        if (-not $?) { throw "Generate-ConfigJson.ps1 failed" }
        Write-Host "   [OK] Config.json generated"

        # Copy Config.json into DSC\Scripts (overwrite template so on-VM scripts find real values)
        Copy-Item (Join-Path $tempPackagePath "Config.json") -Destination "$dscScriptsTarget\Config.json" -Force
        Write-Host "   [OK] Copied Config.json into DSC\Scripts"

        # Generate ResultsUpload.json for test results upload (only when a config is
        # supplied -- public packages must not embed a write-capable SAS token).
        if ($ResultsUploadConfig) {
            $ResultsUploadConfig | ConvertTo-Json -Depth 3 |
                Set-Content -Path (Join-Path $tempPackagePath "ResultsUpload.json") -Force
            Write-Host "   [OK] ResultsUpload.json generated"
        }

        # Zip the assembled package
        if (Test-Path $OutputZipPath) { Remove-Item $OutputZipPath -Force }
        Compress-Archive -Path (Join-Path $tempPackagePath "*") -DestinationPath $OutputZipPath -Force
        Write-Host "   [OK] Created zip: $OutputZipPath"
    }
    finally {
        Remove-Item $tempPackagePath -Recurse -Force -ErrorAction SilentlyContinue
    }
}

Export-ModuleMember -Function @(
    'Import-AzureModules',
    'Connect-AzureSubscription',
    'Initialize-ResourceGroup',
    'ConvertFrom-SecurePassword',
    'New-TemporaryStorageAccount',
    'Get-OrCreateStorageAccount',
    'New-ResultsUploadConfig',
    'Send-BlobWithSasUrl',
    'New-PasswordParameterFile',
    'Wait-ForDomainController',
    'Remove-TemporaryStorageAccount',
    'ConvertFrom-BicepParam',
    'Resolve-DeploymentConfig',
    'Resolve-AvailableVmSize',
    'Test-RegionalVCpuQuota',
    'Test-VmImageAvailability',
    'Test-CapacityError',
    'Watch-Deployment',
    'Install-DscPackageAssets',
    'Build-DscPackage',
    'New-DscPackageZip',
    'Enable-VmDiskEncryption',
    'Invoke-DiskEncryptionForVMs'
)
