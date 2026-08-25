# Copyright (c) Microsoft. All rights reserved.
# Licensed under the MIT license. See LICENSE file in the project root for full license information.

$here = Split-Path -Parent $MyInvocation.MyCommand.Path
$root = (Resolve-Path (Join-Path $here '..')).Path

$scenarios = @(
    @{
        Name = 'Workgroup'
        Template = 'workgroup-bicep\azuredeploy.json'
        Parameters = @{
            driverVmSize = 'Standard_B4ms'
            sutVmSize = 'Standard_B8ms'
        }
    }
    @{
        Name = 'Domain'
        Template = 'domain-bicep\azuredeploy.json'
        Parameters = @{
            dcVmSize = 'Standard_B4ms'
            driverVmSize = 'Standard_B4ms'
            sutVmSize = 'Standard_B8ms'
        }
    }
    @{
        Name = 'Cluster'
        Template = 'cluster-bicep\azuredeploy.json'
        Parameters = @{
            dcVmSize = 'Standard_B4ms'
            storageVmSize = 'Standard_B4ms'
            clusterNodeVmSize = 'Standard_B8ms'
            driverVmSize = 'Standard_B4ms'
        }
    }
)

Describe 'One-click VM-size dropdowns' {
    foreach ($scenario in $scenarios) {
        Context $scenario.Name {
            $template = Get-Content (Join-Path $root $scenario.Template) -Raw |
                ConvertFrom-Json

            foreach ($parameterName in $scenario.Parameters.Keys) {
                $expectedDefault = $scenario.Parameters[$parameterName]

                It "provides curated choices for $parameterName" {
                    $parameter = $template.parameters.$parameterName
                    @($parameter.allowedValues).Count | Should BeGreaterThan 1
                    @($parameter.allowedValues) -contains $expectedDefault |
                        Should Be $true
                }

                It "uses the burstable default for $parameterName" {
                    $template.parameters.$parameterName.defaultValue |
                        Should Be $expectedDefault
                }
            }
        }
    }
}
