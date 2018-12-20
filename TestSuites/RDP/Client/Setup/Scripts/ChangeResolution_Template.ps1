#############################################################################
## Copyright (c) Microsoft Corporation. All rights reserved.
## Licensed under the MIT license. See LICENSE file in the project root for full license information.
#############################################################################

Param(
[int]
$Width = 1024,
[int]
$Height = 768
)

. ScriptPath\Set-DisplaySettings.ps1
Set-ScreenResolution $Width $Height
