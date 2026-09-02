# Copyright (c) Microsoft. All rights reserved.
# Licensed under the MIT license. See LICENSE file in the project root for full license information.

function Get-DscPackageRequiredPaths {
    [CmdletBinding()]
    param(
        [Parameter(Mandatory)]
        [ValidateSet('Workgroup', 'Domain', 'Cluster')]
        [string]$Scenario
    )

    $shared = @(
        'Config.json',
        'Tools.json',
        'DSC/Deploy-CommonHelpers.ps1',
        'DSC/Deploy-Driver.ps1',
        'DSC/Driver-Configuration.ps1',
        'DSC/Scripts/InstallMSIAndTools.ps1',
        'DSC/Scripts/Configure-ForceLevel2.ps1',
        'cse-bootstrap.ps1',
        'DSC/Scripts/Package-Contracts.ps1',
        'DSC/Scripts/Set-ConfigCredential.ps1',
        'DSC/Scripts/Tools.json',
        'DSC/Scripts/Validate-ConfigFile.ps1'
    )

    $scenarioPaths = switch ($Scenario) {
        'Workgroup' {
            @(
                'DSC/Deploy-SUT.ps1',
                'DSC/SUT-Configuration.ps1',
                'DSC/SUT-FeatureConfiguration.ps1',
                'DSC/Invoke-SutImperativeSteps.ps1'
            )
        }
        'Domain' {
            @(
                'DSC/Deploy-DC.ps1',
                'DSC/DC-Configuration.ps1',
                'DSC/DC-FeatureConfiguration.ps1',
                'DSC/Deploy-SUT.ps1',
                'DSC/SUT-Configuration.ps1',
                'DSC/SUT-FeatureConfiguration.ps1',
                'DSC/Invoke-DcImperativeSteps.ps1',
                'DSC/Invoke-SutImperativeSteps.ps1',
                'DSC/Scripts/Set-KerberosMachinePasswordAlignment.ps1'
            )
        }
        'Cluster' {
            @(
                'DSC/Deploy-DC.ps1',
                'DSC/DC-Configuration.ps1',
                'DSC/DC-FeatureConfiguration.ps1',
                'DSC/Deploy-Storage.ps1',
                'DSC/Storage-FeatureConfiguration.ps1',
                'DSC/Storage-Configuration.ps1',
                'DSC/Deploy-ClusterDriver.ps1',
                'DSC/Deploy-ClusterNode.ps1',
                'DSC/Deploy-Node01.ps1',
                'DSC/Deploy-Node02.ps1',
                'DSC/Node-FeatureConfiguration.ps1',
                'DSC/Node-Configuration.ps1',
                'DSC/Invoke-DcImperativeSteps.ps1',
                'DSC/Invoke-StorageImperativeSteps.ps1',
                'DSC/Invoke-Node01ImperativeSteps.ps1',
                'DSC/Invoke-Node02ImperativeSteps.ps1',
                'DSC/Invoke-ClusterEnvironmentSteps.ps1',
                'DSC/Scripts/Set-KerberosMachinePasswordAlignment.ps1',
                'DSC/Scripts/Test-StorageReadiness.ps1',
                'DSC/Scripts/Test-NodeFoundationReadiness.ps1',
                'DSC/Scripts/Test-ClusterReadiness.ps1'
                'DSC/Scripts/Test-ClusterDriverReadiness.ps1'
            )
        }
    }

    return @($shared + $scenarioPaths | Sort-Object -Unique)
}

function Get-DscPackageRequiredToolRoles {
    [CmdletBinding()]
    param(
        [Parameter(Mandatory)]
        [ValidateSet('Workgroup', 'Domain', 'Cluster')]
        [string]$Scenario
    )

    switch ($Scenario) {
        'Workgroup' { return @('SUT', 'DriverComputer') }
        'Domain' { return @('DC', 'SUT', 'DriverComputer') }
        'Cluster' { return @('DC', 'Storage', 'Node01', 'Node02', 'DriverComputer') }
    }
}

function Test-DscPackageRequiredContent {
    [CmdletBinding()]
    param(
        [Parameter(Mandatory)]
        [string]$PackageRoot,

        [Parameter(Mandatory)]
        [ValidateSet('Workgroup', 'Domain', 'Cluster')]
        [string]$Scenario,

        [switch]$ThrowOnFailure
    )

    $failures = New-Object System.Collections.Generic.List[string]
    $resolvedRoot = $null
    try {
        $resolvedRoot = (Resolve-Path -LiteralPath $PackageRoot -ErrorAction Stop).Path
    }
    catch {
        $failures.Add("Package root '$PackageRoot' was not found.")
    }

    if ($resolvedRoot) {
        foreach ($relativePath in (Get-DscPackageRequiredPaths -Scenario $Scenario)) {
            $fullPath = Join-Path $resolvedRoot ($relativePath -replace '/', '\')
            $item = Get-Item -LiteralPath $fullPath -ErrorAction SilentlyContinue
            if ($null -eq $item -or $item.PSIsContainer) {
                $failures.Add("Required package file is missing: $relativePath")
            }
            elseif ($item.Length -le 0) {
                $failures.Add("Required package file is empty: $relativePath")
            }
        }

        $toolsPath = Join-Path $resolvedRoot 'Tools.json'
        if (Test-Path -LiteralPath $toolsPath -PathType Leaf) {
            try {
                $tools = Get-Content -LiteralPath $toolsPath -Raw -ErrorAction Stop |
                    ConvertFrom-Json -ErrorAction Stop
                foreach ($role in (Get-DscPackageRequiredToolRoles -Scenario $Scenario)) {
                    if ($tools.PSObject.Properties.Name -notcontains $role) {
                        $failures.Add("Tools.json is missing required role '$role'.")
                        continue
                    }
                    $roleConfiguration = $tools.$role
                    if ($null -eq $roleConfiguration.PSObject.Properties['Tools']) {
                        $failures.Add("Tools.json role '$role' is missing its Tools array.")
                    }
                    if ($null -eq $roleConfiguration.PSObject.Properties['TestsuiteZips']) {
                        Add-Member -InputObject $roleConfiguration -NotePropertyName TestsuiteZips `
                            -NotePropertyValue @()
                    }
                }
            }
            catch {
                $failures.Add("Tools.json is invalid: $($_.Exception.Message)")
            }
        }
    }

    if ($failures.Count -gt 0) {
        $message = "DSC package required-content validation failed:`n - $($failures -join "`n - ")"
        if ($ThrowOnFailure) { throw $message }
        Write-Warning $message
        return $false
    }
    return $true
}

function New-DscPackageManifest {
    [CmdletBinding()]
    param(
        [Parameter(Mandatory)]
        [string]$PackageRoot,

        [Parameter(Mandatory)]
        [ValidateSet('Workgroup', 'Domain', 'Cluster')]
        [string]$Scenario,

        [Parameter(Mandatory)]
        [ValidateNotNullOrEmpty()]
        [string]$SourceRevision,

        [string]$SchemaVersion = '1.0'
    )

    Test-DscPackageRequiredContent -PackageRoot $PackageRoot -Scenario $Scenario `
        -ThrowOnFailure | Out-Null

    $resolvedRoot = (Resolve-Path -LiteralPath $PackageRoot -ErrorAction Stop).Path
    $manifestPath = Join-Path $resolvedRoot 'PackageManifest.json'
    Remove-Item -LiteralPath $manifestPath -Force -ErrorAction SilentlyContinue

    $files = foreach ($relativePath in (Get-ChildItem -LiteralPath $resolvedRoot `
        -Recurse -File -Name | Sort-Object)) {
        $fullPath = Join-Path $resolvedRoot $relativePath
        $file = Get-Item -LiteralPath $fullPath
        [ordered]@{
            Path = $relativePath -replace '\\', '/'
            Length = [long]$file.Length
            SHA256 = (Get-FileHash -LiteralPath $fullPath -Algorithm SHA256).Hash.ToLowerInvariant()
        }
    }

    $manifest = [ordered]@{
        SchemaVersion = $SchemaVersion
        Scenario = $Scenario
        SourceRevision = $SourceRevision
        GeneratedUtc = (Get-Date).ToUniversalTime().ToString('o')
        Files = @($files)
    }
    $manifest | ConvertTo-Json -Depth 6 |
        Set-Content -LiteralPath $manifestPath -Encoding UTF8 -Force
    return $manifestPath
}

function Test-DscPackageManifest {
    [CmdletBinding()]
    param(
        [Parameter(Mandatory)]
        [string]$PackageRoot,

        [Parameter(Mandatory)]
        [ValidateSet('Workgroup', 'Domain', 'Cluster')]
        [string]$ExpectedScenario,

        [string]$ExpectedSchemaVersion = '1.0',

        [switch]$ThrowOnFailure
    )

    $failures = New-Object System.Collections.Generic.List[string]
    $manifest = $null
    $resolvedRoot = $null
    try {
        $resolvedRoot = (Resolve-Path -LiteralPath $PackageRoot -ErrorAction Stop).Path
        $manifestPath = Join-Path $resolvedRoot 'PackageManifest.json'
        $manifest = Get-Content -LiteralPath $manifestPath -Raw -ErrorAction Stop |
            ConvertFrom-Json -ErrorAction Stop
    }
    catch {
        $failures.Add("Package manifest is missing or invalid: $($_.Exception.Message)")
    }

    if ($manifest) {
        if ("$($manifest.SchemaVersion)" -ne $ExpectedSchemaVersion) {
            $failures.Add(
                "Package manifest schema version '$($manifest.SchemaVersion)' does not match '$ExpectedSchemaVersion'."
            )
        }
        if ("$($manifest.Scenario)" -ne $ExpectedScenario) {
            $failures.Add(
                "Package manifest scenario '$($manifest.Scenario)' does not match '$ExpectedScenario'."
            )
        }
        if ([string]::IsNullOrWhiteSpace("$($manifest.SourceRevision)")) {
            $failures.Add('Package manifest SourceRevision is empty.')
        }

        $manifestEntries = @($manifest.Files)
        if ($manifestEntries.Count -eq 0) {
            $failures.Add('Package manifest contains no file entries.')
        }

        $manifestPaths = New-Object System.Collections.Generic.HashSet[string](
            [System.StringComparer]::OrdinalIgnoreCase
        )
        foreach ($entry in $manifestEntries) {
            $relativePath = "$($entry.Path)" -replace '\\', '/'
            if ([string]::IsNullOrWhiteSpace($relativePath) -or
                [System.IO.Path]::IsPathRooted($relativePath) -or
                @($relativePath -split '/') -contains '..') {
                $failures.Add("Package manifest contains an unsafe path: '$relativePath'.")
                continue
            }
            if (-not $manifestPaths.Add($relativePath)) {
                $failures.Add("Package manifest contains duplicate path '$relativePath'.")
                continue
            }

            $fullPath = Join-Path $resolvedRoot ($relativePath -replace '/', '\')
            $file = Get-Item -LiteralPath $fullPath -ErrorAction SilentlyContinue
            if ($null -eq $file -or $file.PSIsContainer) {
                $failures.Add("Manifest file is missing: $relativePath")
                continue
            }
            if ([long]$entry.Length -ne [long]$file.Length) {
                $failures.Add("Package length mismatch for '$relativePath'.")
            }
            $actualHash = (Get-FileHash -LiteralPath $fullPath `
                -Algorithm SHA256).Hash.ToLowerInvariant()
            if ($actualHash -ne "$($entry.SHA256)".ToLowerInvariant()) {
                $failures.Add("Package hash mismatch for '$relativePath'.")
            }
        }

        if ($resolvedRoot) {
            $actualPaths = @(Get-ChildItem -LiteralPath $resolvedRoot -Recurse -File -Name |
                Where-Object { (Split-Path $_ -Leaf) -ne 'PackageManifest.json' } |
                ForEach-Object { $_ -replace '\\', '/' })
            foreach ($actualPath in $actualPaths) {
                if (-not $manifestPaths.Contains($actualPath)) {
                    $failures.Add("Package contains unmanifested file '$actualPath'.")
                }
            }
        }

        try {
            Test-DscPackageRequiredContent -PackageRoot $resolvedRoot `
                -Scenario $ExpectedScenario -ThrowOnFailure | Out-Null
        }
        catch {
            $failures.Add($_.Exception.Message)
        }
    }

    if ($failures.Count -gt 0) {
        $message = "DSC package manifest validation failed:`n - $($failures -join "`n - ")"
        if ($ThrowOnFailure) { throw $message }
        Write-Warning $message
        return $false
    }
    return $true
}
