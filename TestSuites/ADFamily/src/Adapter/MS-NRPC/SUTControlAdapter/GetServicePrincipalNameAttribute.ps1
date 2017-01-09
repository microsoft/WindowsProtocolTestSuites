#############################################################
## Copyright (c) Microsoft. All rights reserved.
## Licensed under the MIT license. See LICENSE file in the project root for full license information.
#############################################################

$PSScriptRoot = Split-Path -Parent -Path $MyInvocation.MyCommand.Definition
Push-Location $PSScriptRoot

$objectPath = .\GetClientObjPath.ps1
$attribute = "servicePrincipalName"

return .\GetAttributeValueFromAD.ps1 $objectPath $attribute