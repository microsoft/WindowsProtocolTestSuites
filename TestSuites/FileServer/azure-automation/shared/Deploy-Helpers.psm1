# Copyright (c) Microsoft. All rights reserved.
# Licensed under the MIT license. See LICENSE file in the project root for full license information.

# Deploy-Helpers.psm1
# Shared deployment helper functions for File Server Test Suite Azure deployments
# Used by both cluster-bicep/deploy.ps1 and domain-bicep/deploy.ps1

function Test-TransientAzureError {
    [CmdletBinding()]
    param(
        [Parameter(Mandatory)]
        [System.Management.Automation.ErrorRecord]$ErrorRecord
    )

    $messages = [System.Collections.Generic.List[string]]::new()
    if ($ErrorRecord.ErrorDetails.Message) {
        $messages.Add($ErrorRecord.ErrorDetails.Message)
    }

    $exception = $ErrorRecord.Exception
    while ($null -ne $exception) {
        if ($exception.Message) { $messages.Add($exception.Message) }
        $exception = $exception.InnerException
    }

    $errorText = $messages -join ' '
    return $errorText -match '(?i)HttpClient\.Timeout|TaskCanceledException|TimeoutException|request (?:was )?canceled|timed?\s*out|operation canceled|\b408\b|RequestTimeout|\b429\b|TooManyRequests|throttl|\b500\b|InternalServerError|\b502\b|BadGateway|\b503\b|ServiceUnavailable|temporarily unavailable|\b504\b|GatewayTimeout|connection (?:was )?(?:reset|closed|aborted)|connection attempt failed|failed to respond|transport connection|name resolution|remote name could not be resolved|socket exception'
}

function Get-AzureRetryAfterSeconds {
    [CmdletBinding()]
    param(
        [Parameter(Mandatory)]
        [System.Management.Automation.ErrorRecord]$ErrorRecord
    )

    try {
        $retryAfter = $ErrorRecord.Exception.Response.Headers.RetryAfter
        if ($retryAfter.Delta) {
            return [math]::Ceiling($retryAfter.Delta.TotalSeconds)
        }
        if ($retryAfter.Date) {
            return [math]::Max(0, [math]::Ceiling(($retryAfter.Date.UtcDateTime - [DateTime]::UtcNow).TotalSeconds))
        }
    } catch { }

    return $null
}

function Invoke-AzureOperationWithRetry {
    <#
    .SYNOPSIS
        Executes a read-only or idempotent Azure control-plane operation with
        bounded retries for transient failures.
    #>
    [CmdletBinding()]
    param(
        [Parameter(Mandatory)]
        [string]$OperationName,

        [Parameter(Mandatory)]
        [scriptblock]$Operation,

        [ValidateRange(1, 10)]
        [int]$MaxAttempts = 5,

        [ValidateRange(0, 300)]
        [int]$InitialDelaySeconds = 5,

        [ValidateRange(0, 600)]
        [int]$MaximumDelaySeconds = 60
    )

    for ($attempt = 1; $attempt -le $MaxAttempts; $attempt++) {
        try {
            return & $Operation
        } catch {
            $isTransient = Test-TransientAzureError -ErrorRecord $_
            if (-not $isTransient) {
                throw
            }

            if ($attempt -eq $MaxAttempts) {
                throw "$OperationName failed after $MaxAttempts attempts because Azure continued returning a transient error. Last error: $($_.Exception.Message)"
            }

            $exponentialDelay = [math]::Min(
                $MaximumDelaySeconds,
                $InitialDelaySeconds * [math]::Pow(2, $attempt - 1))
            $retryAfterSeconds = Get-AzureRetryAfterSeconds -ErrorRecord $_
            $baseDelaySeconds = if ($null -ne $retryAfterSeconds) {
                [math]::Min($MaximumDelaySeconds, $retryAfterSeconds)
            } else {
                $exponentialDelay
            }
            $jitterMaximum = [math]::Max(0, [int][math]::Ceiling($baseDelaySeconds * 0.2))
            $jitterSeconds = if ($jitterMaximum -gt 0) {
                Get-Random -Minimum 0 -Maximum ($jitterMaximum + 1)
            } else { 0 }
            $delaySeconds = [math]::Min($MaximumDelaySeconds, $baseDelaySeconds + $jitterSeconds)

            Write-Warning "$OperationName hit a transient Azure error (attempt $attempt of $MaxAttempts): $($_.Exception.Message) Retrying in $delaySeconds seconds."
            if ($delaySeconds -gt 0) {
                Start-Sleep -Seconds $delaySeconds
            }
        }
    }
}

function Get-RegionalVmSkuSnapshot {
    <#
    .SYNOPSIS
        Returns regional VM names and vCPU counts using the lightweight VM-sizes
        endpoint rather than the full resource-SKU feed.
    .DESCRIPTION
        The full Microsoft.Compute/skus response can exceed the Az SDK's
        100-second HTTP timeout. This endpoint supplies the fields needed for
        candidate selection and regional quota math. ARM pre-validation remains
        authoritative for policy, zone restrictions, family quota, and capacity.
    #>
    [CmdletBinding()]
    param(
        [Parameter(Mandatory)]
        [string]$Location
    )

    $context = Get-AzContext
    if (-not $context -or -not $context.Subscription.Id) {
        throw 'An Azure subscription context is required before querying regional VM sizes.'
    }

    $normalizedLocation = ($Location.ToLowerInvariant() -replace '[^a-z0-9]', '')
    $path = "/subscriptions/$($context.Subscription.Id)/providers/Microsoft.Compute/locations/$normalizedLocation/vmSizes?api-version=2024-07-01"
    $response = Invoke-AzureOperationWithRetry `
        -OperationName "Read regional VM sizes in '$Location'" `
        -Operation { Invoke-AzRestMethod -Method GET -Path $path -ErrorAction Stop }

    if ($response.StatusCode -lt 200 -or $response.StatusCode -ge 300) {
        throw "Regional VM-size query for '$Location' returned HTTP $($response.StatusCode)."
    }

    $body = $response.Content | ConvertFrom-Json -ErrorAction Stop
    $sizes = @($body.value)
    if ($sizes.Count -eq 0) {
        throw "Regional VM-size query for '$Location' returned no VM sizes."
    }

    return @($sizes | ForEach-Object {
        [pscustomobject]@{
            Name          = $_.name
            ResourceType  = 'virtualMachines'
            Family        = $null
            Restrictions  = @()
            LocationInfo  = @()
            Capabilities  = @(
                [pscustomobject]@{ Name = 'vCPUs'; Value = [string]$_.numberOfCores }
                [pscustomobject]@{ Name = 'MemoryGB'; Value = [string]([math]::Round($_.memoryInMB / 1024, 2)) }
            )
        }
    })
}

function Import-AzureModules {
    [CmdletBinding()]
    param()

    $requiredModules = @('Az.Accounts', 'Az.Resources', 'Az.Storage', 'Az.Compute')
    $missingModules = @($requiredModules | Where-Object {
        -not (Get-Module -ListAvailable -Name $_)
    })

    if ($missingModules.Count -gt 0) {
        $installModuleCommand = Get-Command Install-Module -ErrorAction SilentlyContinue
        if (-not $installModuleCommand) {
            throw "Required Azure PowerShell modules are missing ($($missingModules -join ', ')), and Install-Module is unavailable. Install PowerShellGet, then rerun the deployment."
        }

        Write-Output "`nInstalling missing Azure PowerShell modules for the current user: $($missingModules -join ', ')"
        foreach ($moduleName in $missingModules) {
            $installed = $false
            for ($attempt = 1; $attempt -le 3 -and -not $installed; $attempt++) {
                try {
                    Install-Module -Name $moduleName -Repository PSGallery -Scope CurrentUser `
                        -Force -AllowClobber -Confirm:$false -ErrorAction Stop
                    $installed = $true
                } catch {
                    if ($attempt -eq 3) {
                        throw "Failed to install required module '$moduleName' after $attempt attempts: $($_.Exception.Message)"
                    }
                    $retryDelaySeconds = [math]::Pow(2, $attempt)
                    Write-Warning "Could not install '$moduleName' (attempt $attempt of 3): $($_.Exception.Message). Retrying in $retryDelaySeconds seconds."
                    Start-Sleep -Seconds $retryDelaySeconds
                }
            }
        }
    }

    Write-Output "`n📦 Importing Azure PowerShell modules..."
    foreach ($moduleName in $requiredModules) {
        Import-Module $moduleName -Force -ErrorAction Stop
    }
    Write-Output "✅ Azure modules imported"
}

function Initialize-BicepCli {
    [CmdletBinding()]
    param(
        [ValidateRange(1, 10)]
        [int]$MaxAttempts = 3
    )

    $bicepCommand = Get-Command bicep -ErrorAction SilentlyContinue
    if ($bicepCommand) {
        & $bicepCommand.Source --version | Write-Output
        if ($LASTEXITCODE -eq 0) {
            return
        }
        Write-Warning "The Bicep command at '$($bicepCommand.Source)' is not executable; reinstalling it."
    }

    $azCommand = Get-Command az -ErrorAction SilentlyContinue
    if (-not $azCommand) {
        throw "Bicep CLI is not available and Azure CLI ('az') is not installed. Install either Bicep CLI (https://aka.ms/bicep-install) or Azure CLI, then rerun the deployment."
    }

    Write-Output "Bicep CLI not found on PATH. Installing through Azure CLI..."
    $installed = $false
    for ($attempt = 1; $attempt -le $MaxAttempts -and -not $installed; $attempt++) {
        & $azCommand.Source bicep install
        if ($LASTEXITCODE -eq 0) {
            $installed = $true
            break
        }

        if ($attempt -lt $MaxAttempts) {
            $retryDelaySeconds = [math]::Pow(2, $attempt)
            Write-Warning "Bicep installation failed (attempt $attempt of $MaxAttempts). Retrying in $retryDelaySeconds seconds."
            Start-Sleep -Seconds $retryDelaySeconds
        }
    }
    if (-not $installed) {
        throw "Failed to install Bicep CLI after $MaxAttempts attempts. See https://aka.ms/bicep-install."
    }

    $bicepFileName = if ($IsWindows -or $env:OS -eq 'Windows_NT') { 'bicep.exe' } else { 'bicep' }
    $azBicepPath = Join-Path (Join-Path $HOME '.azure/bin') $bicepFileName
    if (Test-Path -LiteralPath $azBicepPath) {
        $azBicepDirectory = Split-Path $azBicepPath -Parent
        $pathEntries = @($env:PATH -split [IO.Path]::PathSeparator)
        if ($azBicepDirectory -notin $pathEntries) {
            $env:PATH = "$azBicepDirectory$([IO.Path]::PathSeparator)$env:PATH"
        }
    }

    $bicepCommand = Get-Command bicep -ErrorAction SilentlyContinue
    if (-not $bicepCommand) {
        throw "Azure CLI reported a successful Bicep installation, but the 'bicep' executable was not found on PATH. Expected it under '$azBicepPath'."
    }

    & $bicepCommand.Source --version | Write-Output
    if ($LASTEXITCODE -ne 0) {
        throw "Bicep CLI was installed but failed its version check."
    }
    Write-Output "✅ Bicep CLI is ready"
}

function Get-AzureCliAccessContext {
    [CmdletBinding()]
    param(
        [Parameter(Mandatory)]
        [string]$SubscriptionId
    )

    $azCommand = Get-Command az -ErrorAction SilentlyContinue
    if (-not $azCommand) { return $null }

    try {
        $accountJson = @(& $azCommand.Source account show --subscription $SubscriptionId `
            --only-show-errors --output json 2>$null) -join [Environment]::NewLine
        if ($LASTEXITCODE -ne 0 -or [string]::IsNullOrWhiteSpace($accountJson)) { return $null }
        $account = $accountJson | ConvertFrom-Json -ErrorAction Stop
        if ($account.id -ne $SubscriptionId -or [string]::IsNullOrWhiteSpace($account.tenantId) -or
            [string]::IsNullOrWhiteSpace($account.user.name)) {
            return $null
        }

        $accessToken = (@(& $azCommand.Source account get-access-token `
            --subscription $SubscriptionId --resource 'https://management.azure.com/' `
            --query accessToken --output tsv --only-show-errors 2>$null) -join '').Trim()
        if ($LASTEXITCODE -ne 0 -or [string]::IsNullOrWhiteSpace($accessToken)) { return $null }

        return [pscustomobject]@{
            AccessToken    = $accessToken
            AccountId      = [string]$account.user.name
            TenantId       = [string]$account.tenantId
            SubscriptionId = [string]$account.id
        }
    } catch {
        return $null
    }
}

function Connect-AzAccountFromAzureCli {
    [CmdletBinding()]
    param(
        [Parameter(Mandatory)]
        [string]$SubscriptionId
    )

    $cliContext = Get-AzureCliAccessContext -SubscriptionId $SubscriptionId
    if (-not $cliContext) { return $false }

    try {
        Connect-AzAccount -AccessToken $cliContext.AccessToken `
            -AccountId $cliContext.AccountId -Tenant $cliContext.TenantId `
            -Subscription $SubscriptionId -Scope Process -ErrorAction Stop | Out-Null
        return $true
    } catch {
        return $false
    } finally {
        $cliContext = $null
    }
}

function Connect-AzureSubscription {
    [CmdletBinding()]
    param(
        [Parameter(Mandatory=$true)]
        [string]$SubscriptionId
    )

    Write-Output "`n🔐 Selecting Azure subscription: $SubscriptionId"
    $context = Get-AzContext
    if (-not $context -or $context.Subscription.Id -ne $SubscriptionId) {
        try {
            Set-AzContext -SubscriptionId $SubscriptionId -ErrorAction Stop | Out-Null
        } catch {
            Write-Output "No usable cached Azure context was found. Authentication is required."
            if (-not (Connect-AzAccountFromAzureCli -SubscriptionId $SubscriptionId)) {
                Connect-AzAccount -SubscriptionId $SubscriptionId -Scope Process -ErrorAction Stop | Out-Null
            }
        }
    }

    $context = Get-AzContext
    $contextIsUsable = $false
    if ($context -and $context.Subscription.Id -eq $SubscriptionId) {
        try {
            $probe = Invoke-AzRestMethod -Method GET `
                -Path "/subscriptions/$SubscriptionId`?api-version=2022-12-01" `
                -ErrorAction Stop
            $contextIsUsable = $probe.StatusCode -ge 200 -and $probe.StatusCode -lt 300
        } catch {
            Write-Warning "The cached Azure context targets the requested subscription but its token is unusable: $($_.Exception.Message)"
        }
    }

    if (-not $contextIsUsable) {
        Write-Output 'Refreshing Azure authentication before deployment operations...'
        Clear-AzContext -Scope Process -Force -ErrorAction SilentlyContinue | Out-Null
        if (Connect-AzAccountFromAzureCli -SubscriptionId $SubscriptionId) {
            Write-Output 'Reused the authenticated Azure CLI session.'
        } else {
            Connect-AzAccount -SubscriptionId $SubscriptionId -Scope Process -ErrorAction Stop | Out-Null
        }
        $context = Get-AzContext
        $probe = Invoke-AzRestMethod -Method GET `
            -Path "/subscriptions/$SubscriptionId`?api-version=2022-12-01" `
            -ErrorAction Stop
        $contextIsUsable = $probe.StatusCode -ge 200 -and $probe.StatusCode -lt 300
    }

    if (-not $context -or $context.Subscription.Id -ne $SubscriptionId) {
        throw "Azure context selection failed. Expected subscription '$SubscriptionId', but the active subscription is '$($context.Subscription.Id)'."
    }
    if (-not $contextIsUsable) {
        throw "Azure authentication validation failed for subscription '$SubscriptionId'."
    }

    Write-Output "✅ Using Azure subscription: $($context.Subscription.Name) ($($context.Subscription.Id))"
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

function Remove-VmAutoShutdownSchedules {
    [CmdletBinding()]
    param(
        [Parameter(Mandatory)]
        [string]$ResourceGroupName,

        [Parameter(Mandatory)]
        [string[]]$VMNames
    )

    $expectedScheduleNames = @($VMNames | Where-Object { $_ } |
        Select-Object -Unique | ForEach-Object { "shutdown-computevm-$_" })
    $schedules = @(@(Invoke-AzureOperationWithRetry `
        -OperationName "List auto-shutdown schedules in '$ResourceGroupName'" `
        -Operation {
            Get-AzResource -ResourceGroupName $ResourceGroupName `
                -ResourceType 'Microsoft.DevTestLab/schedules' -ErrorAction Stop
        }) | Where-Object { $_.Name -in $expectedScheduleNames })

    foreach ($schedule in $schedules) {
        Invoke-AzureOperationWithRetry `
            -OperationName "Remove auto-shutdown schedule '$($schedule.Name)'" `
            -Operation {
                Remove-AzResource -ResourceId $schedule.ResourceId -Force -ErrorAction Stop | Out-Null
            } | Out-Null
    }

    if ($schedules.Count -gt 0) {
        Write-Output "Removed $($schedules.Count) auto-shutdown schedule(s) while deployment validation is active."
    }
}

function Enable-VmAutoShutdownSchedules {
    [CmdletBinding()]
    param(
        [Parameter(Mandatory)] [string]$ResourceGroupName,
        [Parameter(Mandatory)] [string]$Location,
        [Parameter(Mandatory)] [string[]]$VMNames,
        [Parameter(Mandatory)] [string]$Time,
        [Parameter(Mandatory)] [string]$TimeZone
    )

    $normalizedTime = $Time -replace ':', ''
    if ($normalizedTime -notmatch '^(?:[01]\d|2[0-3])[0-5]\d$') {
        throw "Auto-shutdown time '$Time' must be HH:mm or HHmm in 24-hour format."
    }

    foreach ($vmName in @($VMNames | Where-Object { $_ } | Select-Object -Unique)) {
        $vm = Invoke-AzureOperationWithRetry `
            -OperationName "Read VM '$vmName' for auto-shutdown" `
            -Operation {
                Get-AzVM -ResourceGroupName $ResourceGroupName -Name $vmName -ErrorAction Stop
            }

        $properties = [pscustomobject]@{
            status = 'Enabled'
            taskType = 'ComputeVmShutdownTask'
            dailyRecurrence = [pscustomobject]@{ time = $normalizedTime }
            timeZoneId = $TimeZone
            targetResourceId = $vm.Id
            notificationSettings = [pscustomobject]@{ status = 'Disabled' }
        }

        Invoke-AzureOperationWithRetry `
            -OperationName "Create auto-shutdown schedule for '$vmName'" `
            -Operation {
                New-AzResource -ResourceGroupName $ResourceGroupName `
                    -ResourceType 'Microsoft.DevTestLab/schedules' `
                    -Name "shutdown-computevm-$vmName" -ApiVersion '2018-09-15' `
                    -Location $Location -Properties $properties -Force -ErrorAction Stop
            } | Out-Null
    }

    Write-Output "Enabled auto-shutdown at $normalizedTime ($TimeZone) for $($VMNames.Count) VM(s)."
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
        [int]$PollIntervalSeconds = 30,

        [Parameter(Mandatory=$false)]
        [int]$ProbeTimeoutSeconds = 120
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
            $probeJob = $null
            try {
                $remainingSeconds = [Math]::Max(1, [int]($deadline - (Get-Date)).TotalSeconds)
                $probeWaitSeconds = [Math]::Min($ProbeTimeoutSeconds, $remainingSeconds)
                $probeJob = Invoke-AzVMRunCommand -ResourceGroupName $ResourceGroupName `
                    -VMName $dcVm.Name -CommandId 'RunPowerShellScript' `
                    -ScriptString $CheckScript -AsJob -ErrorAction Stop
                $completedProbe = Wait-Job -Job $probeJob -Timeout $probeWaitSeconds
                if ($null -eq $completedProbe) {
                    Stop-Job -Job $probeJob
                    Write-Output "[$elapsed min] ⚠️  DC readiness probe exceeded ${probeWaitSeconds}s; retrying with a fresh Run Command."
                    Start-Sleep -Seconds $PollIntervalSeconds
                    continue
                }
                $result = Receive-Job -Job $probeJob -ErrorAction Stop

                if ($result.Value[0].Message -match "True") {
                    $duration = [math]::Round(((Get-Date) - $startTime).TotalMinutes, 1)
                    Write-Output "[$elapsed min] ✅ Domain Controller is ready! (took $duration min)"
                    return $true
                }
                Write-Output "[$elapsed min] ⏳ DC still configuring..."
            } catch {
                Write-Output "[$elapsed min] ⚠️  Error checking DC: $($_.Exception.Message)"
            } finally {
                if ($null -ne $probeJob) {
                    Remove-Job -Job $probeJob -Force -ErrorAction SilentlyContinue
                }
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

function Join-StorageSasUrl {
    [CmdletBinding()]
    param(
        [Parameter(Mandatory)] [string]$BlobEndpoint,
        [Parameter(Mandatory)] [string]$ContainerName,
        [Parameter(Mandatory)] [string]$SasToken
    )

    $uriBuilder = [UriBuilder]$BlobEndpoint
    $uriBuilder.Path = "$($uriBuilder.Path.TrimEnd('/'))/$ContainerName"
    $uriBuilder.Query = $SasToken.TrimStart('?')
    return $uriBuilder.Uri.AbsoluteUri
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

    $sasUrl = Join-StorageSasUrl -BlobEndpoint $StorageContext.BlobEndPoint `
        -ContainerName $ContainerName -SasToken $sasToken

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
        Pre-fetched VM SKU snapshot. Get-RegionalVmSkuSnapshot provides the
        lightweight name/vCPU shape; full resource-SKU objects are also accepted.
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

function Invoke-DeploymentWithSkuFallback {
    <#
    .SYNOPSIS
        Runs a resource-group deployment with per-role VM-size fallback on capacity
        errors. Consolidates the pre-validate -> deploy -> classify-capacity-error ->
        advance-candidate loop that phase deployments previously duplicated inline.
    .PARAMETER ResourceGroupName
        Target resource group.
    .PARAMETER TemplateFile
        Path to the Bicep template.
    .PARAMETER BaseParameters
        Template parameters EXCLUDING the VM-size parameters named in SizeCandidates.
    .PARAMETER SizeCandidates
        Hashtable: template size-parameter name -> string[] of candidate sizes in
        preference order (e.g. @{ dcVmSize = $dcCandidates }). On a capacity error the
        failing role's candidate index advances; if the error text names no role's
        current size, every role with candidates left advances.
    .PARAMETER DeploymentNamePrefix
        Prefix for the timestamped deployment name (e.g. 'Phase1').
    .OUTPUTS
        The successful deployment object (with .Outputs). Throws when candidates are
        exhausted or the failure is not capacity-related.
    #>
    [CmdletBinding()]
    param(
        [Parameter(Mandatory=$true)]
        [string]$ResourceGroupName,

        [Parameter(Mandatory=$true)]
        [string]$TemplateFile,

        [Parameter(Mandatory=$true)]
        [hashtable]$BaseParameters,

        [Parameter(Mandatory=$true)]
        [hashtable]$SizeCandidates,

        [Parameter(Mandatory=$true)]
        [string]$DeploymentNamePrefix
    )

    $indices = @{}
    foreach ($k in $SizeCandidates.Keys) { $indices[$k] = 0 }
    $currentSizes = @{}

    # Advance the candidate index of any role whose CURRENT size appears in the
    # error text; if the text names none of them, advance every role that still
    # has candidates left. Returns $true if at least one role advanced.
    $stepFailedSizes = {
        param([string]$ErrorText)
        $advanced = $false
        $anyMatched = $false
        foreach ($k in @($SizeCandidates.Keys)) {
            if ($ErrorText -match [regex]::Escape($currentSizes[$k])) {
                $anyMatched = $true
                if (($indices[$k] + 1) -lt $SizeCandidates[$k].Count) {
                    Write-Warning "VM size '$($currentSizes[$k])' ($k) hit capacity restrictions. Trying next fallback..."
                    $indices[$k]++
                    $advanced = $true
                }
            }
        }
        if (-not $anyMatched) {
            foreach ($k in @($SizeCandidates.Keys)) {
                if (($indices[$k] + 1) -lt $SizeCandidates[$k].Count) {
                    Write-Warning "Capacity error did not name a VM size; advancing $k to next fallback..."
                    $indices[$k]++
                    $advanced = $true
                }
            }
        }
        return $advanced
    }

    while ($true) {
        foreach ($k in @($SizeCandidates.Keys)) { $currentSizes[$k] = $SizeCandidates[$k][$indices[$k]] }

        $deployParams = @{} + $BaseParameters
        foreach ($k in $currentSizes.Keys) { $deployParams[$k] = $currentSizes[$k] }

        $sizeDesc = ($currentSizes.GetEnumerator() | Sort-Object Name | ForEach-Object { "$($_.Key)=$($_.Value)" }) -join ', '
        $deploymentName = "$DeploymentNamePrefix-$(Get-Date -Format 'yyyyMMdd-HHmmss')"
        Write-Output "`nStarting $DeploymentNamePrefix deployment ($sizeDesc)..."

        # Pre-validate to catch SkuNotAvailable before starting the full deployment.
        # ARM validation errors don't produce deployment operations, so Test-CapacityError
        # cannot classify them after the fact.
        $preValidation = Test-AzResourceGroupDeployment `
            -ResourceGroupName $ResourceGroupName `
            -TemplateFile $TemplateFile `
            -TemplateParameterObject $deployParams
        if ($preValidation) {
            $allCodes = @($preValidation | ForEach-Object { $_.Code })
            $allCodes += $preValidation | ForEach-Object { $_.Details } | Where-Object { $_ } | ForEach-Object { $_.Code }
            $allMessages = ($preValidation | ForEach-Object {
                $msg = "[$($_.Code)] $($_.Message)"
                if ($_.Details) { $msg += " Details: $(($_.Details | ForEach-Object { "[$($_.Code)] $($_.Message)" }) -join '; ')" }
                $msg
            }) -join "`n  "

            $isSkuError = $allCodes -match 'SkuNotAvailable|AllocationFailed|ZonalAllocationFailed'
            if ($isSkuError) {
                if (& $stepFailedSizes $allMessages) { continue }
                throw "$DeploymentNamePrefix pre-validation failed (SkuNotAvailable) and no more fallback sizes available:`n  $allMessages"
            }
            throw "$DeploymentNamePrefix pre-validation failed:`n  $allMessages"
        }
        Write-Output "  Pre-validation passed"

        $job = New-AzResourceGroupDeployment `
            -ResourceGroupName $ResourceGroupName `
            -Name $deploymentName `
            -TemplateFile $TemplateFile `
            -TemplateParameterObject $deployParams `
            -ErrorAction Stop `
            -AsJob

        $deployment = $null
        try {
            $deployment = Watch-Deployment -ResourceGroupName $ResourceGroupName `
                -DeploymentName $deploymentName -Job $job
        } catch {
            if ($job.State -in @('Running', 'NotStarted')) {
                Stop-Job -Job $job -ErrorAction SilentlyContinue
            }
            Remove-Job -Job $job -Force -ErrorAction SilentlyContinue
            throw
        }
        if ($job.State -in @('Running', 'NotStarted')) {
            Stop-Job -Job $job -ErrorAction SilentlyContinue
        }
        Remove-Job -Job $job -Force -ErrorAction SilentlyContinue

        $provState = $deployment.ProvisioningState
        if ($provState -eq 'Succeeded') {
            return $deployment
        }

        $isCapacity = Test-CapacityError -ErrorRecord $null `
            -ResourceGroupName $ResourceGroupName -DeploymentName $deploymentName
        $failedOperationText = try {
            @(Get-AzResourceGroupDeploymentOperation -ResourceGroupName $ResourceGroupName `
                -DeploymentName $deploymentName -ErrorAction Stop |
                Where-Object ProvisioningState -eq 'Failed' |
                ForEach-Object { "$($_.StatusMessage)" }) -join ' '
        } catch { '' }

        if ($isCapacity -and (& $stepFailedSizes $failedOperationText)) { continue }

        throw "$DeploymentNamePrefix deployment '$deploymentName' finished with state '$provState'. $failedOperationText"
    }
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
        Pre-fetched VM SKU snapshot from Get-RegionalVmSkuSnapshot or the full
        Get-AzComputeResourceSku response.
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
    $usages = @(Invoke-AzureOperationWithRetry `
        -OperationName "Read regional vCPU quota in '$Location'" `
        -Operation { Get-AzVMUsage -Location $Location -ErrorAction Stop })
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

    $images = @(Invoke-AzureOperationWithRetry `
        -OperationName "Check VM image '$Publisher/$Offer/$Sku' in '$Location'" `
        -Operation {
            Get-AzVMImage -Location $Location -PublisherName $Publisher `
                -Offer $Offer -Skus $Sku -ErrorAction Stop
        })
    return ($images.Count -gt 0)
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
    # The local Az job can fail because its client connection times out while the
    # registered ARM deployment continues. Once ARM exposes the named deployment,
    # ARM state is authoritative and local job state is diagnostic only.
    $waitStart = Get-Date
    $deployment = $null
    while (((Get-Date) - $waitStart).TotalSeconds -lt 60) {
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
            $jobFailure = try {
                $Job | Receive-Job -ErrorAction Stop | Out-Null
                'The local deployment job failed before ARM registered the deployment.'
            } catch { $_.Exception.Message }
            throw "Deployment job failed before ARM registered '$DeploymentName': $jobFailure"
        }
        throw "Deployment '$DeploymentName' was not registered in ARM within 60 seconds."
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
    if ($finalState -notin @('Succeeded', 'Failed', 'Canceled')) {
        if ($Job -and $Job.State -in @('Running', 'NotStarted')) {
            Stop-Job -Job $Job -ErrorAction SilentlyContinue
        }
        throw [TimeoutException]::new("Deployment '$DeploymentName' did not reach a terminal state within $TimeoutMinutes minutes (last state: $finalState).")
    }
    $color = switch ($finalState) {
        'Succeeded' { 'Green' }
        'Failed'    { 'Red' }
        default     { 'Yellow' }
    }
    Write-Host ""
    Write-Host "[$(Get-Date -Format 'HH:mm:ss')]  ** Deployment '$DeploymentName' $($finalState.ToLower()) in $totalMin min **" -ForegroundColor $color
    return $deployment
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

    # Resume-safe idempotency: avoid reapplying ADE (and another reboot) when a
    # previous run already completed OS-volume encryption.
    try {
        $encryptionStatus = Get-AzVMDiskEncryptionStatus `
            -ResourceGroupName $ResourceGroupName -VMName $VMName -ErrorAction Stop
        if ($encryptionStatus.OsVolumeEncrypted -eq 'Encrypted') {
            Write-Output "   [OK] Disk encryption is already enabled for '$VMName'"
            return $true
        }
    }
    catch {
        Write-Output "   Could not read existing encryption status for '$VMName'; applying ADE."
    }

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
        try {
            $encryptionStatus = Get-AzVMDiskEncryptionStatus `
                -ResourceGroupName $ResourceGroupName -VMName $VMName -ErrorAction Stop
            if ($encryptionStatus.OsVolumeEncrypted -eq 'Encrypted') {
                Write-Warning "ADE reported an extension error for '$VMName', but the OS volume is encrypted. Treating the postcondition as authoritative."
                return $true
            }
        } catch { }
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
    .PARAMETER ThrottleLimit
        Maximum number of VMs encrypted concurrently. A single VM remains sequential,
        which preserves the required DC-before-domain-members ordering.
    #>
    [CmdletBinding()]
    param(
        [Parameter(Mandatory)]
        [string]$ResourceGroupName,

        [Parameter(Mandatory)]
        $DeploymentOutputs,

        [Parameter(Mandatory)]
        [string[]]$VMNames,

        [ValidateRange(1, 16)]
        [int]$ThrottleLimit = 2
    )

    $kvId  = $DeploymentOutputs.keyVaultId.Value
    $kvUrl = $DeploymentOutputs.keyVaultUrl.Value

    if (-not $kvId -or -not $kvUrl) {
        Write-Warning "Key Vault outputs not found in deployment. Skipping disk encryption."
        return
    }

    $uniqueVmNames = @($VMNames | Where-Object { $_ } | Select-Object -Unique)
    if ($uniqueVmNames.Count -eq 0) {
        return
    }

    $mountPointScript = Join-Path $PSScriptRoot 'scripts\Set-FsaMountPointsForDiskEncryption.ps1'
    if (-not (Test-Path -LiteralPath $mountPointScript)) {
        throw "FSA mount-point helper was not found at '$mountPointScript'."
    }
    $preparedVmNames = [System.Collections.Generic.List[string]]::new()
    try {
        foreach ($vmName in $uniqueVmNames) {
            Invoke-AzureOperationWithRetry `
                -OperationName "Prepare '$vmName' mount points for disk encryption" `
                -Operation {
                    Invoke-AzVMRunCommand -ResourceGroupName $ResourceGroupName `
                        -VMName $vmName -CommandId RunPowerShellScript `
                        -ScriptPath $mountPointScript -Parameter @{ Mode = 'Detach' } `
                        -ErrorAction Stop | Out-Null
                } | Out-Null
            $preparedVmNames.Add($vmName)
        }

        Write-Output "`n  Encrypting VM disks..."
        $results = [System.Collections.Generic.List[object]]::new()
        $threadJobCommand = Get-Command Start-ThreadJob -ErrorAction SilentlyContinue

        if ($uniqueVmNames.Count -gt 1 -and $threadJobCommand) {
        Write-Output "   Encrypting $($uniqueVmNames.Count) VMs concurrently (throttle: $ThrottleLimit)..."
        $encryptionFunction = ${function:Enable-VmDiskEncryption}.ToString()
        $azureContext = Get-AzContext
        $jobs = foreach ($vmName in $uniqueVmNames) {
            $job = Start-ThreadJob -ThrottleLimit $ThrottleLimit -ArgumentList @(
                $ResourceGroupName, $vmName, $kvUrl, $kvId, $encryptionFunction, $azureContext
            ) -ScriptBlock {
                param($rg, $vm, $vaultUrl, $vaultId, $functionBody, $context)

                Set-AzContext -Context $context -ErrorAction Stop | Out-Null
                Set-Item -Path Function:\Enable-VmDiskEncryption -Value ([scriptblock]::Create($functionBody))
                $output = @(Enable-VmDiskEncryption -ResourceGroupName $rg `
                    -VMName $vm -KeyVaultUrl $vaultUrl -KeyVaultId $vaultId)

                [PSCustomObject]@{
                    VMName  = $vm
                    Success = ($output.Count -gt 0 -and $output[-1] -eq $true)
                    Output  = @($output | Select-Object -SkipLast 1)
                }
            }
            $job | Add-Member -NotePropertyName TargetVMName -NotePropertyValue $vmName
            $job
        }

        foreach ($job in $jobs) {
            try {
                $result = $job | Receive-Job -Wait -AutoRemoveJob -ErrorAction Stop
                foreach ($message in $result.Output) {
                    Write-Output $message
                }
                $results.Add($result)
            }
            catch {
                $results.Add([PSCustomObject]@{
                    VMName  = $job.TargetVMName
                    Success = $false
                    Output  = @("Disk encryption job failed: $($_.Exception.Message)")
                })
                $job | Remove-Job -Force -ErrorAction SilentlyContinue
            }
        }
        }
        else {
        if ($uniqueVmNames.Count -gt 1) {
            Write-Warning 'Start-ThreadJob is unavailable; encrypting VMs sequentially.'
        }
        foreach ($vmName in $uniqueVmNames) {
            $output = @(Enable-VmDiskEncryption -ResourceGroupName $ResourceGroupName `
                -VMName $vmName -KeyVaultUrl $kvUrl -KeyVaultId $kvId)
            foreach ($message in @($output | Select-Object -SkipLast 1)) {
                Write-Output $message
            }
            $results.Add([PSCustomObject]@{
                VMName  = $vmName
                Success = ($output.Count -gt 0 -and $output[-1] -eq $true)
                Output  = @()
            })
        }
        }

        $failed = @($results | Where-Object { -not $_.Success })
        if ($failed.Count -gt 0) {
            $failedNames = ($failed.VMName | Sort-Object -Unique) -join ', '
            throw "Disk encryption failed for: $failedNames"
        }

        return @($results)
    } finally {
        foreach ($vmName in @($preparedVmNames)) {
            $restored = $false
            for ($attempt = 1; $attempt -le 5 -and -not $restored; $attempt++) {
                try {
                    Invoke-AzVMRunCommand -ResourceGroupName $ResourceGroupName `
                        -VMName $vmName -CommandId RunPowerShellScript `
                        -ScriptPath $mountPointScript -Parameter @{ Mode = 'Restore' } `
                        -ErrorAction Stop | Out-Null
                    $restored = $true
                } catch {
                    if ($attempt -eq 5) { throw }
                    Start-Sleep -Seconds ([math]::Min(15 * $attempt, 60))
                }
            }
        }
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

        # Windows Custom Script Extensions invoke this generic bootstrap from
        # the extracted package. This avoids embedding the full script in
        # commandToExecute, which is subject to Windows' command-line limit.
        $sharedRoot = Split-Path $SharedDscPath -Parent
        $windowsBootstrapSource = Join-Path $sharedRoot 'scripts\cse-bootstrap.ps1'
        $windowsBootstrapDestination = Join-Path $tempPackagePath 'cse-bootstrap.ps1'
        if (-not (Test-Path -LiteralPath $windowsBootstrapSource)) {
            throw "Windows CSE bootstrap not found at '$windowsBootstrapSource'."
        }
        Copy-Item -LiteralPath $windowsBootstrapSource `
            -Destination $windowsBootstrapDestination -Force
        Write-Host "   [OK] Copied Windows CSE bootstrap to package root"

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

function Resolve-FreeSubnetIp {
    <#
    .SYNOPSIS
        Selects free private IPv4 host addresses inside a subnet CIDR (pure, offline, testable).
    .DESCRIPTION
        Given a subnet prefix, the set of already-allocated addresses, and an ordered list of
        DESIRED addresses, returns an ordered list of the same length where each entry is a
        usable host address in the subnet that is not already allocated and not duplicated across
        the returned set. A desired address is preserved verbatim when it is a valid host in the
        subnet and still free; otherwise it is replaced with the next free host address.

        Azure reserves the first four addresses of every subnet (network, gateway ".1", and two
        further reserved addresses ".2"/".3") plus the broadcast address, so selection starts at
        network+4 and stops at broadcast-1. This function performs NO Azure calls -- callers gather
        the prefix and used set (see Resolve-VmNicIp) and pass them in, which keeps the arithmetic
        unit-testable without a live subscription.
    .PARAMETER SubnetPrefix
        Subnet CIDR, e.g. '10.1.3.0/24'.
    .PARAMETER UsedIp
        Addresses already allocated in the subnet (any out-of-subnet/invalid entries are ignored).
    .PARAMETER DesiredIp
        Ordered preferred addresses. Out-of-subnet or taken entries are auto-reassigned. The
        returned array preserves this order and length.
    .OUTPUTS
        [string[]] resolved addresses, one per DesiredIp entry, all unique/free/in-subnet.
    #>
    [CmdletBinding()]
    param(
        [Parameter(Mandatory = $true)]
        [string]$SubnetPrefix,

        [string[]]$UsedIp = @(),

        [Parameter(Mandatory = $true)]
        [string[]]$DesiredIp
    )

    if ($SubnetPrefix -notmatch '^(\d{1,3}\.\d{1,3}\.\d{1,3}\.\d{1,3})/(\d{1,2})$') {
        throw "Resolve-FreeSubnetIp: '$SubnetPrefix' is not a valid IPv4 CIDR (expected e.g. 10.1.3.0/24)."
    }
    $baseStr = $Matches[1]
    $maskLen = [int]$Matches[2]
    if ($maskLen -lt 0 -or $maskLen -gt 32) { throw "Resolve-FreeSubnetIp: invalid prefix length /$maskLen." }

    # Integer octet math only (no [System.Net.IPAddress]) so this stays constrained-language safe.
    $toInt = {
        param([string]$ip)
        if ([string]::IsNullOrWhiteSpace($ip)) { return $null }
        $o = $ip.Trim().Split('.')
        if ($o.Count -ne 4) { return $null }
        foreach ($p in $o) { if ($p -notmatch '^\d{1,3}$' -or [int]$p -gt 255) { return $null } }
        return ([long]$o[0] * 16777216) + ([long]$o[1] * 65536) + ([long]$o[2] * 256) + [long]$o[3]
    }
    $toIp = {
        param([long]$v)
        '{0}.{1}.{2}.{3}' -f [long](($v -shr 24) -band 255), [long](($v -shr 16) -band 255), `
            [long](($v -shr 8) -band 255), [long]($v -band 255)
    }

    $baseInt = & $toInt $baseStr
    $size = [long]1 -shl (32 - $maskLen)          # total addresses in the block
    $mask = if ($maskLen -eq 0) { [long]0 } else { ([long]0xFFFFFFFF -shl (32 - $maskLen)) -band [long]0xFFFFFFFF }
    $network = $baseInt -band $mask
    $broadcast = $network + $size - 1
    $usableStart = $network + 4                       # skip network + .1/.2/.3 (Azure-reserved)
    $usableEnd = $broadcast - 1                        # skip broadcast

    if ($usableStart -gt $usableEnd) {
        throw "Resolve-FreeSubnetIp: subnet $SubnetPrefix has no assignable host addresses (too small)."
    }

    # Pre-compute the allocated set as integers (ignoring anything outside this subnet).
    $usedInts = @()
    foreach ($u in $UsedIp) {
        $ui = & $toInt $u
        if ($null -ne $ui -and $ui -ge $network -and $ui -le $broadcast) { $usedInts += $ui }
    }

    $claimed = @()
    $result = @()
    foreach ($d in $DesiredIp) {
        $dInt = & $toInt $d
        $chosen = $null

        if ($null -ne $dInt -and $dInt -ge $usableStart -and $dInt -le $usableEnd -and
            ($usedInts -notcontains $dInt) -and ($claimed -notcontains $dInt)) {
            $chosen = $dInt
        }
        else {
            for ($c = $usableStart; $c -le $usableEnd; $c++) {
                if (($usedInts -notcontains $c) -and ($claimed -notcontains $c)) { $chosen = $c; break }
            }
            if ($null -eq $chosen) {
                throw "Resolve-FreeSubnetIp: no free host address left in $SubnetPrefix (requested $($DesiredIp.Count))."
            }
        }

        $claimed += $chosen
        $result += (& $toIp $chosen)
    }

    return , $result
}

function Resolve-VmNicIp {
    <#
    .SYNOPSIS
        Resolves collision-free static private IPs for a set of NICs against a live Azure VNet.
    .DESCRIPTION
        Thin Azure-facing orchestrator over Resolve-FreeSubnetIp. For each request it looks up the
        target subnet's address prefix and the private IPs already allocated to NICs referencing
        that subnet (across the subscription), then delegates the arithmetic to Resolve-FreeSubnetIp
        so a desired IP is kept when free and reassigned when taken or out-of-subnet. Requests that
        share a subnet are resolved together so the returned IPs never collide with each other.

        Selection must happen ONCE per run (before the shared DSC package bakes Config.json), which
        is why this is called from the pipeline wrapper rather than per-VM inside Deploy-PipelineVm.
    .PARAMETER VnetName
        Name of the existing virtual network the NICs attach to.
    .PARAMETER VnetResourceGroup
        Resource group that holds the VNet.
    .PARAMETER Request
        Ordered array of hashtables, each @{ Key = <resultKey>; Subnet = <subnetName>; DesiredIp = <preferredIp> }.
    .OUTPUTS
        [hashtable] mapping each request Key to its resolved IP string.
    #>
    [CmdletBinding()]
    param(
        [Parameter(Mandatory = $true)]
        [string]$VnetName,

        [Parameter(Mandatory = $true)]
        [string]$VnetResourceGroup,

        [Parameter(Mandatory = $true)]
        [object[]]$Request
    )

    $vnet = Get-AzVirtualNetwork -Name $VnetName -ResourceGroupName $VnetResourceGroup -ErrorAction Stop
    $allNics = @(Get-AzNetworkInterface -ErrorAction SilentlyContinue)

    $result = @{}
    $subnetOrder = @($Request | ForEach-Object { "$($_.Subnet)" } | Select-Object -Unique)

    foreach ($sn in $subnetOrder) {
        $subnet = $vnet.Subnets | Where-Object { $_.Name -eq $sn } | Select-Object -First 1
        if (-not $subnet) { throw "Resolve-VmNicIp: subnet '$sn' not found in VNet '$VnetName' (RG '$VnetResourceGroup')." }
        $prefix = @($subnet.AddressPrefix)[0]
        $subnetId = $subnet.Id

        # Private IPs already claimed on this subnet (authoritative for VM/NIC collisions).
        $used = @()
        foreach ($nic in $allNics) {
            foreach ($ipc in @($nic.IpConfigurations)) {
                if ($ipc.Subnet -and $ipc.Subnet.Id -eq $subnetId -and $ipc.PrivateIpAddress) {
                    $used += "$($ipc.PrivateIpAddress)"
                }
            }
        }

        $reqForSubnet = @($Request | Where-Object { "$($_.Subnet)" -eq $sn })
        $desired = @($reqForSubnet | ForEach-Object { "$($_.DesiredIp)" })
        $resolved = @(Resolve-FreeSubnetIp -SubnetPrefix $prefix -UsedIp $used -DesiredIp $desired)

        for ($i = 0; $i -lt $reqForSubnet.Count; $i++) {
            $key = "$($reqForSubnet[$i].Key)"
            $result[$key] = $resolved[$i]
            $orig = "$($reqForSubnet[$i].DesiredIp)"
            if ($orig -ne $resolved[$i]) {
                Write-Host "   [IP] $key : '$orig' in use/out-of-subnet on '$sn' -> reassigned to '$($resolved[$i])'"
            }
            else {
                Write-Host "   [IP] $key : '$($resolved[$i])' (free on '$sn')"
            }
        }
    }

    return $result
}

function Complete-DeploymentTestOutcome {
    [CmdletBinding()]
    param(
        [Parameter(Mandatory = $true)]
        [object]$Verification
    )

    if (-not $Verification.TestsComplete) {
        return
    }

    $classification = "$($Verification.TestClassification)"
    $failedTestCount = [int]$Verification.FailedTestCount
    $message = "Automatic tests reached terminal finalization with classification '$classification' and $failedTestCount non-passing results. Requested post-test infrastructure handling completed."

    switch ($classification) {
        'Passed' {
            Write-Host '[OK] Automatic tests passed and deployment orchestration completed.'
        }
        'TestFailures' {
            Write-Warning "$message The environment and test run completed successfully; review the reported protocol-test failures."
        }
        'InfrastructureOrConfigurationFailure' {
            Write-Warning "$message The environment orchestration completed, but the test run reported infrastructure or configuration failures; review the complete summary."
        }
        'MixedTestAndInfrastructureFailures' {
            Write-Warning "$message The environment orchestration completed, but the test run reported both protocol-test and infrastructure/configuration failures; review the complete summary."
        }
        default {
            throw "$message The test classification is missing or unknown, so deployment validation cannot be trusted."
        }
    }
}

Export-ModuleMember -Function @(
    'Invoke-AzureOperationWithRetry',
    'Get-RegionalVmSkuSnapshot',
    'Import-AzureModules',
    'Initialize-BicepCli',
    'Connect-AzureSubscription',
    'Initialize-ResourceGroup',
    'Remove-VmAutoShutdownSchedules',
    'Enable-VmAutoShutdownSchedules',
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
    'Invoke-DeploymentWithSkuFallback',
    'Watch-Deployment',
    'Install-DscPackageAssets',
    'Build-DscPackage',
    'New-DscPackageZip',
    'Enable-VmDiskEncryption',
    'Invoke-DiskEncryptionForVMs',
    'Complete-DeploymentTestOutcome',
    'Resolve-FreeSubnetIp',
    'Resolve-VmNicIp'
)
