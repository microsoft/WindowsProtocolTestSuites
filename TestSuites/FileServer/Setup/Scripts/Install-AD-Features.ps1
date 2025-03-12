# Copyright (c) Microsoft. All rights reserved.
# Licensed under the MIT license. See LICENSE file in the project root for full license information.

# Install ADDS
Install-WindowsFeature -Name AD-Domain-Services `
                    -IncludeAllSubFeature `
                    -IncludeManagementTools `
                    -ErrorAction Stop

$feature = Get-WindowsFeature -Name AD-Domain-Services -ErrorAction SilentlyContinue
$featureInstalled = $feature -and $feature.Installed

return $featureInstalled