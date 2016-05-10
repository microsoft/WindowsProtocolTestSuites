#############################################################################
## Copyright (c) Microsoft. All rights reserved.
## Licensed under the MIT license. See LICENSE file in the project root for full license information.
##
## Microsoft Windows Powershell Scripting
## Purpose: Get OS version number.
##
##############################################################################

#----------------------------------------------------------------------------
# Get OSVersion Number
#----------------------------------------------------------------------------
[int]$major = [Environment]::OSVersion.Version.Major
[int]$minor = [Environment]::OSVersion.Version.Minor

$OSVersionNumber = [double]("$major" + "." + "$minor")

#----------------------------------------------------------------------------
# Print exit information
#----------------------------------------------------------------------------
Write-Host "OS version number is: $OSVersionNumber" -foregroundcolor Green
return $OSVersionNumber
