# Copyright (c) Microsoft. All rights reserved.
# Licensed under the MIT license. See LICENSE file in the project root for full license information.

$here = Split-Path -Parent $MyInvocation.MyCommand.Path
$root = (Resolve-Path (Join-Path $here '..')).Path

function Read-AutomationFile {
    param([string]$RelativePath)
    return Get-Content -Path (Join-Path $root $RelativePath) -Raw
}

Describe 'One-click location selection' {
    foreach ($scenario in @('workgroup', 'domain', 'cluster')) {
        It "uses only the Portal resource-group Region for $scenario" {
            $main = Read-AutomationFile "$scenario-bicep\main.bicep"
            $json = Read-AutomationFile "$scenario-bicep\azuredeploy.json" |
                ConvertFrom-Json

            $main.Contains('var location = resourceGroup().location') |
                Should Be $true
            $main.Contains('param location string') | Should Be $false
            @($json.parameters.PSObject.Properties.Name) -contains 'location' |
                Should Be $false
        }
    }

    It 'keeps Workgroup CLI region selection outside the Portal template' {
        $deploy = Read-AutomationFile 'workgroup-bicep\deploy.ps1'
        $parameters = Read-AutomationFile `
            'workgroup-bicep\parameters\workgroup.bicepparam'

        $deploy.Contains("[string]`$Location = 'West US 2'") | Should Be $true
        $deploy.Contains('location          = $Location') | Should Be $true
        $parameters.Contains('param location') | Should Be $false
    }
}
