#############################################################################
## Copyright (c) Microsoft. All rights reserved.
## Licensed under the MIT license. See LICENSE file in the project root for full license information.
##
## Microsoft Windows Powershell Scripting
## Purpose: Write logs according to LogLevel configured in ParamConfig.xml.
##
##############################################################################

Param(
[String]$logContent,
[String]$level = "Debug",
[String]$logPath = "",
[String]$configFile = ".\ParamConfig.xml"
)

#----------------------------------------------------------------------------
# Starting script
#----------------------------------------------------------------------------

#----------------------------------------------------------------------------
# Function: Show-ScriptUsage
# Usage   : Describes the usage information and options
#----------------------------------------------------------------------------
function Show-ScriptUsage
{    
    Write-host 
    Write-host "Write logs in log file."
    Write-host
    Write-host "The Path of the log file can be saved to $configFile."
    Write-host "If you don't input log file path, the saved value should be used."
    Write-host
    Write-host "Example: Write-Log.ps1 `"Test log`" Debug C:\Test.log"
    Write-host
}

#----------------------------------------------------------------------------
# Show help if required
#----------------------------------------------------------------------------
if ($args[0] -match '-(\?|(h|(help)))')
{
    Show-ScriptUsage 
    return
}

#----------------------------------------------------------------------------
# Verify required parameters
#----------------------------------------------------------------------------
if ($logContent -eq $null -or $logContent -eq "")
{
    Throw "Parameter logContent is required."
}

#----------------------------------------------------------------------------
# Convert log level to int
#----------------------------------------------------------------------------
switch ($level.ToUpper())
{
    "DEBUG"
    {
        $level = "1"
    }

    "VERBOSE"
    {
        $level = "2"
    }

    "CLIENT"
    {
        $level = "3"
    }

    Default
    {
        $level = "0"
    }
}

#----------------------------------------------------------------------------
# Get the LogLevel, if `$level less than LogLevel, don't write log on screen
#----------------------------------------------------------------------------
$logLevel = .\Get-Parameter.ps1 $configFile LogLevel

#----------------------------------------------------------------------------
# Get the full path of $logPath
#----------------------------------------------------------------------------
if ($logPath -ne "")
{
    $fileName = Split-Path $logPath -leaf
    $path = .\Get-Parameter.ps1 $configFile LogPath
    if (!(Test-Path $path))
    {
        New-Item -Type Directory -Path $path -Force
    }
    $logPath = "$path\$fileName"
    .\Set-Parameter.ps1 $configFile LogFile $logPath "If no log file path specified, this value should be used."
}
else
{
    $logPath = .\Get-Parameter.ps1 $configFile LogFile
}

#----------------------------------------------------------------------------
# Write log info and add time stamp
#----------------------------------------------------------------------------
$timeString = (Get-Date).ToString("yyyy-MM-dd HH:mm:ss")
$logContent = "[$timeString] $logContent"

#Write log on screen
$logLevel = $logLevel - 1
if ([Int]$level -gt [Int]$logLevel)
{
    Write-Host $logContent
}

#write log in log file
if (Test-Path $logPath)
{
    Add-Content -Path $logPath -Force -Value $logContent
}
else
{
    Set-Content -Path $logPath -Force -Value $logContent
}