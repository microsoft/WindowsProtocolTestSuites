#############################################################
## Copyright (c) Microsoft. All rights reserved.
## Licensed under the MIT license. See LICENSE file in the project root for full license information.
#############################################################

switch -Wildcard ([environment]::OSVersion.Version)
{
    "10.0.*"
    {
        cmd /c "$PSScriptRoot\RunDRSRCases_Threshold.cmd"
        return
    }
}
cmd /c "$PSScriptRoot\RunDrsrCases_Win8.1.cmd"
