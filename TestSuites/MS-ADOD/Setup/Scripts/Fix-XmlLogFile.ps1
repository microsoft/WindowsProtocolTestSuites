#############################################################################
## Copyright (c) Microsoft. All rights reserved.
## Licensed under the MIT license. See LICENSE file in the project root for full license information.
#############################################################################

#############################################################################
##
## Microsoft Windows PowerShell Scripting
## File:           Fix-XmlLogFile.ps1
## Purpose:        Fix Xml log file for some un-closed tags
## Version:        1.1 (21 Feb., 2012)
## Copyright (c) Microsoft Corporation. All rights reserved.
##
##############################################################################
param(
[String]$logPath
)

# Write Call Stack
if($function:EnterCallStack -ne $null)
{
	EnterCallStack "Fix-XmlLogFile.ps1"
}

function FixUnclosedTages($logFile)
{
	$content = Get-Content -Path $logFile
	try{
		$xmlContent = [xml]$content
	}
	catch {
		$content += "`n</LogEntries>"
		$content += "`n</TestLog>"
		$xmlContent = [xml]$content
		$xmlContent.Save("$logFile")
	}
}

Write-Host "Get log file list ..."
$logFiles     = [System.IO.Directory]::GetFiles($logPath, "*log.xml", [System.IO.SearchOption]::AllDirectories)
foreach($logFile in $logFiles)
{
	Write-Host "FixUnclosedTages $logFile"
	FixUnclosedTages $logFile
}

# Write Call Stack
if($function:ExitCallStack -ne $null)
{
	ExitCallStack "Fix-XmlLogFile.ps1"
}