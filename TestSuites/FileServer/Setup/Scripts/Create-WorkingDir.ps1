#############################################################################
## Copyright (c) Microsoft. All rights reserved.
## Licensed under the MIT license. See LICENSE file in the project root for full license information.
##
#############################################################################

param($workingDir = "$env:SystemDrive\Temp", $protocolConfigFile = "$workingDir\Protocol.xml")

$scriptPath = Split-Path $MyInvocation.MyCommand.Definition -parent

if(!(Test-Path "$workingDir"))
{
    New-Item -ItemType directory -Path $workingDir
}

if(!(Test-Path "$protocolConfigFile"))
{
    $protocolConfigFile = "$scriptPath\Protocol.xml"
    Copy-Item $protocolConfigFile -destination $workingDir
}