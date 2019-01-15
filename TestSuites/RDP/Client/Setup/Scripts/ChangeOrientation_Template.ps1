#############################################################################
## Copyright (c) Microsoft Corporation. All rights reserved.
## Licensed under the MIT license. See LICENSE file in the project root for full license information.
#############################################################################

Param(
[int]
$Orientation = 1
)

. ScriptPath\Set-DisplaySettings.ps1
Set-ScreenOrientation $Orientation
