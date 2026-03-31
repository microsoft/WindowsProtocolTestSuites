# Copyright (c) Microsoft. All rights reserved.
# Licensed under the MIT license. See LICENSE file in the project root for full license information.

##############################################################################
#
# Microsoft Windows Powershell Scripting
# File:           Write-Error.ps1
# Purpose:        Thin wrapper around Write-Info.ps1 with Red as default color.
# Requirements:   Windows Powershell 5.1+
#
##############################################################################
<#
.SYNOPSIS
Writes an error-level log message with the time stamp (red by default).
.PARAMETER filename
Append the log message to the specified file when it is not null.
.PARAMETER logContent
The log message.
.PARAMETER ForegroundColor
The color of the text (default: Red).
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
    $params = @{ ForegroundColor = $ForegroundColor; BackgroundColor = $BackgroundColor }
    if ($filename) { $params['filename'] = $filename }
    # Delegate to Write-Info.ps1 (single implementation, DRY)
    $logContent | & "$PSScriptRoot\Write-Info.ps1" @params
}
