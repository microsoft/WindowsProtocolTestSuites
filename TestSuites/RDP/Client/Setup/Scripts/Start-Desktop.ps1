#############################################################################
## Copyright (c) Microsoft Corporation. All rights reserved.
## Licensed under the MIT license. See LICENSE file in the project root for full license information.
#############################################################################

$rootPath = Split-Path $MyInvocation.MyCommand.Definition -parent

#----------------------------------------------------------------------------
# Start loging using start-transcript cmdlet
#----------------------------------------------------------------------------
Stop-Transcript -ErrorAction Continue | Out-Null
Start-Transcript -Path "$rootPath\Start-Desktop.ps1.log" -Append -Force

#----------------------------------------------------------------------------
# Go to desktop 
#----------------------------------------------------------------------------
Write-Host "Go to desktop (in case on Start Menu currently)" 
explorer.exe 
Start-Sleep -Milliseconds 1000

$shell = New-Object -ComObject WScript.Shell
Write-Host "Select Poll(P) mode!" 
$shell.Sendkeys("P")
Start-Sleep -Milliseconds 500
$shell.SendKeys( "{ENTER}" )

#----------------------------------------------------------------------------
# Stop logging
#----------------------------------------------------------------------------
Stop-Transcript

exit 0