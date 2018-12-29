#############################################################################
## Copyright (c) Microsoft Corporation. All rights reserved.
## Licensed under the MIT license. See LICENSE file in the project root for full license information.
#############################################################################

#############################################################################
##
## Microsoft Windows Powershell Scripting
## File:           Check-ReturnValue.ps1
## Purpose:        Check return value of the previous operation and dump the error message and information.
## Requirements:   Windows Powershell 2.0
## Supported OS:   Windows Server 2008 R2, Windows Server 2012, Windows Server 2012 R2,
##                 Windows Server 2016 and later
##
##############################################################################

Param(
    [switch]$StopScriptWhenErrorOccur
)

if( -not $?)
{
    $Vars = Get-Variable
    $Date = Get-Date -f MM-dd-yyyy_HH_mm_ss
    $Line = $MyInvocation.ScriptLineNumber.ToString()
    Write-Output "`r`n$Date Error in line $Line."
    Write-Output "********************** "
    Write-Output "Dump local variables "
    Write-Output "********************** "
    Format-Table Name,Value -wrap -autosize -inputobject $Vars
    if($StopScriptWhenErrorOccur)
    {
        Stop-Transcript
        throw "Error in line $Line."
    }
}
else
{
    Write-Output "$Date Action succeeded."
}