#############################################################################
##
## Microsoft Windows Powershell Scripting
## File:           Write-Error.ps1
## Purpose:        Writes error log to the screen with time stamp.
## Requirements:   Windows Powershell 2.0
## Copyright (c) Microsoft. All rights reserved.
## Licensed under the MIT license. See LICENSE file in the project root for full license information.
##
########################################################################
<#
.SYNOPSIS
Writes the log message with the time stamp.
.DESCRIPTION
Writes the log message with the time stamp.
.PARAMETER filename
Append the log message to the specified file when it is not null.
.PARAMETER logContent
The log message.
.PARAMETER ForegroundColor
The color of the text.
#>
[CmdletBinding(PositionalBinding=$false)]
param
(
    [alias("h")][switch]$help,
    [string]$filename = $null,
    $ForegroundColor = "Red",
    $BackgroundColor = "Black",
    [Parameter(ValueFromPipeline=$True, ValueFromRemainingArguments=$True)]
    [string]$logContent
)
Begin{
    if($help)
    {
        Get-Help $myInvocation.MyCommand.Definition
        Exit 0
    }
}
Process{
    $timeString = (Get-Date).ToString("yyyy-MM-dd HH:mm:ss")
    $message = "[$timeString] $logContent"
    $osbuildnum= "" + [Environment]::OSVersion.Version.Major + "." + [Environment]::OSVersion.Version.Minor
    if([double]$osbuildnum -eq [double]"6.3")
    {
        # WinBlue issue: Start-Transcript cannot write the log printed out by Write-Host, as a workaround, use Write-output instead
        # Write-Output does not support color
        "$message" | Out-Host
    }
    else
    {
        Write-Host -NoNewline "$message`r`n" -ForegroundColor $ForegroundColor -BackgroundColor $BackgroundColor
    }
    if($filename)
    {
        Add-Content -Path $filename -Force -Value $message
    }
}