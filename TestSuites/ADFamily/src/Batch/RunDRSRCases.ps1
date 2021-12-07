# Copyright (c) Microsoft. All rights reserved.
# Licensed under the MIT license. See LICENSE file in the project root for full license information.

switch -Wildcard ([environment]::OSVersion.Version)
{
    "10.0.*"
    {
        powershell "$PSScriptRoot\RunDRSRCases_Threshold.ps1"
        return
    }
}
powershell "$PSScriptRoot\RunDRSRCases_Win8.1.ps1"
