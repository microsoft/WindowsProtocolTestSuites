# Copyright (c) Microsoft. All rights reserved.
# Licensed under the MIT license. See LICENSE file in the project root for full license information.

# Tier-0 drift guard for committed ARM templates.
# The generated JSON files must stay synchronized with their Bicep entry points.

$here = Split-Path -Parent $MyInvocation.MyCommand.Path
$root = (Resolve-Path (Join-Path $here '..')).Path

function Get-BicepBuilder {
    # Prefer the standalone bicep CLI, fall back to `az bicep`. Returns a hashtable
    # with a Version string and a Build scriptblock, or $null when neither is present.
    $parseVersion = { param($text) if ($text -match '(\d+\.\d+\.\d+)') { $matches[1] } else { $null } }

    $bicep = Get-Command bicep -ErrorAction SilentlyContinue
    if ($bicep) {
        return @{
            Version = (& $parseVersion (& bicep --version 2>$null | Out-String))
            Build   = { param($in, $out) & bicep build $in --outfile $out 2>$null; $LASTEXITCODE }
        }
    }
    $az = Get-Command az -ErrorAction SilentlyContinue
    if ($az) {
        return @{
            Version = (& $parseVersion (& az bicep version 2>$null | Out-String))
            Build   = { param($in, $out) & az bicep build --file $in --outfile $out 2>$null; $LASTEXITCODE }
        }
    }
    return $null
}

function Get-CommittedGeneratorVersion {
    param([string]$TemplatePath)
    $gen = (Get-Content $TemplatePath -Raw | ConvertFrom-Json).metadata._generator.version
    if ($gen -match '(\d+\.\d+\.\d+)') { return $matches[1] }
    return $null
}

function ConvertTo-NormalizedTemplateText {
    param([string]$Text)
    return (($Text -replace "`r`n", "`n").TrimEnd())
}

function ConvertTo-NormalizedOutputJson {
    param([object]$Outputs)

    $json = $Outputs | ConvertTo-Json -Depth 50
    $deploymentReferencePattern = "(reference\(resourceId\('Microsoft\.Resources/deployments'.*?\), )'\d{4}-\d{2}-\d{2}'(\)\.outputs\.)"
    return ($json -replace $deploymentReferencePattern, "`$1'<compiler-selected-api-version>'`$2")
}

$templates = @(
    @{ Name = 'workgroup';     Dir = 'workgroup-bicep'; Bicep = 'main.bicep';   Json = 'azuredeploy.json' }
    @{ Name = 'domain';        Dir = 'domain-bicep';    Bicep = 'main.bicep';   Json = 'azuredeploy.json' }
    @{ Name = 'cluster';       Dir = 'cluster-bicep';   Bicep = 'main.bicep';   Json = 'azuredeploy.json' }
    @{ Name = 'cluster-phase1'; Dir = 'cluster-bicep';  Bicep = 'phase1.bicep'; Json = 'phase1.json' }
    @{ Name = 'cluster-phase2'; Dir = 'cluster-bicep';  Bicep = 'phase2.bicep'; Json = 'phase2.json' }
)

Describe 'Generated ARM template drift' {

    It 'has a bicep builder available (standalone bicep or az bicep)' {
        (Get-BicepBuilder) | Should Not BeNullOrEmpty
    }

    foreach ($template in $templates) {
        Context $template.Name {
            $dir = $template.Dir
            $committedPath = Join-Path (Join-Path $root $dir) $template.Json
            $mainPath = Join-Path (Join-Path $root $dir) $template.Bicep

            It 'recompiles the Bicep entry point and matches the committed JSON' {
                $builder = Get-BicepBuilder
                $builder | Should Not BeNullOrEmpty

                $tmp = Join-Path ([System.IO.Path]::GetTempPath()) "$dir-drift-$([guid]::NewGuid().ToString('N')).json"
                try {
                    $exit = & $builder.Build $mainPath $tmp
                    $exit | Should Be 0

                    $committedJson = Get-Content $committedPath -Raw | ConvertFrom-Json
                    $freshJson     = Get-Content $tmp -Raw | ConvertFrom-Json

                    # Version-tolerant guard: the root parameters and outputs are a direct
                    # projection of main.bicep and are stable across bicep CLI versions, so
                    # they catch added/removed/renamed params, changed defaults/allowedValues,
                    # and rewired outputs regardless of the local bicep version.
                    ($committedJson.parameters | ConvertTo-Json -Depth 50) |
                        Should Be ($freshJson.parameters | ConvertTo-Json -Depth 50)
                    # Bicep selects the nested-deployment reference API version. That
                    # version can change between compilers without changing output wiring.
                    (ConvertTo-NormalizedOutputJson $committedJson.outputs) |
                        Should Be (ConvertTo-NormalizedOutputJson $freshJson.outputs)

                    # Exact guard: only enforced when the local bicep version matches the one
                    # that produced the committed template, otherwise formatting differences
                    # across CLI versions would be false positives. Regenerate with the pinned
                    # version (see the scenario README "Publishing the public package").
                    $committedVersion = Get-CommittedGeneratorVersion $committedPath
                    if ($builder.Version -and $committedVersion -and $builder.Version -eq $committedVersion) {
                        (ConvertTo-NormalizedTemplateText (Get-Content $tmp -Raw)) |
                            Should Be (ConvertTo-NormalizedTemplateText (Get-Content $committedPath -Raw))
                    }
                }
                finally {
                    if (Test-Path $tmp) { Remove-Item $tmp -Force -ErrorAction SilentlyContinue }
                }
            }
        }
    }
}
