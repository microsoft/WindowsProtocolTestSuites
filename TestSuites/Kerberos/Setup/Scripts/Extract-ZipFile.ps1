#############################################################################
## Copyright (c) Microsoft Corporation. All rights reserved.
## Licensed under the MIT license. See LICENSE file in the project root for full license information.
#############################################################################

Param (
    [Parameter(Mandatory=$true)]
    [string]$ZipFile,
    [Parameter(Mandatory=$true)]
    [string]$Destination
    )

$shell = New-Object -com shell.application
$zip = $shell.NameSpace($ZipFile)
if(!(Test-Path -Path $Destination))
{
    New-Item -ItemType directory -Path $Destination
}
$shell.Namespace($Destination).CopyHere($zip.items(), 0x14)