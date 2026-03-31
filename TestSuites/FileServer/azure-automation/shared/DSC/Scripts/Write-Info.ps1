# Copyright (c) Microsoft. All rights reserved.
# Licensed under the MIT license. See LICENSE file in the project root for full license information.

##############################################################################
#
# Microsoft Windows Powershell Scripting
# File:           Write-Info.ps1
# Purpose:        Writes log to the screen with time stamp.
# Requirements:   Windows Powershell 5.1+
#
##############################################################################
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
    $ForegroundColor = "Green",
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
    Write-Host -NoNewline "$message`r`n" -ForegroundColor $ForegroundColor -BackgroundColor $BackgroundColor

    if($filename)
    {
        Add-Content -Path $filename -Force -Value $message
    }
}
