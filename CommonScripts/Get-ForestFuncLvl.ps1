##################################################################################
## Copyright (c) Microsoft Corporation. All rights reserved.
## Licensed under the MIT license. See LICENSE file in the project root for full license information.
##################################################################################

#############################################################################
##
## Microsoft Windows Powershell Scripting
## File:           Get-ForestFuncLvl.ps1
## Purpose:        Get forest functional level and log it to system drive.
## Requirements:   Windows Powershell 2.0
## Supported OS:   Windows Server 2008 R2, Windows Server 2012, Windows Server 2012 R2,
##                 Windows Server 2016 and later
##
##############################################################################

Param
(
    [switch]$log
)

$ForestMode = (Get-ADForest).ForestMode

switch -Exact ($ForestMode)
{
    "Windows2016Forest" {
        $ForestFuncLvl = "7"
        }
    "Windows2012R2Forest" {
        $ForestFuncLvl = "6"
        }
    "Windows2012Forest" {
        $ForestFuncLvl = "5"
        }
    "Windows2008R2Forest" {
        $ForestFuncLvl = "4"
        }
    "Windows2008Forest" {
        $ForestFuncLvl = "3"
        }
    default {
        Write-Host "Unknown OS version retrieved! Set default forest functional level as 7." -ForegroundColor Red
        $ForestFuncLvl = "7"
        }
}

if($log)
{
    # Log Forest Functional Level in the system drive
    $ForestFuncLvl > "$env:SystemDrive\forestfunclvl.txt"
}

return $ForestFuncLvl