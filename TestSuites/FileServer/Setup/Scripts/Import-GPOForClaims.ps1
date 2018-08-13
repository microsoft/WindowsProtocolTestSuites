
#############################################################################
## Copyright (c) Microsoft Corporation. All rights reserved.
##############################################################################

#----------------------------------------------------------------------------
# Global variables
#----------------------------------------------------------------------------
$scriptPath = Split-Path $MyInvocation.MyCommand.Definition -parent
$env:Path += ";$scriptPath;$scriptPath\Scripts"

#----------------------------------------------------------------------------
# Start loging using start-transcript cmdlet
#----------------------------------------------------------------------------
[string]$logFile = $MyInvocation.MyCommand.Path + ".log"
Start-Transcript -Path "$logFile" -Append -Force

#----------------------------------------------------------------------------
# Extract GPOBackup files
#----------------------------------------------------------------------------
Write-Info.ps1 "Extract GPOBackup files"
$ZipFile = "$scriptPath\Scripts\GPOBackup.zip"
$gpoBackupFolder = "$scriptPath\GPOBackup"
Expand-Archive $ZipFile $gpoBackupFolder

#----------------------------------------------------------------------------
# Configure Group Policy for Claims
#----------------------------------------------------------------------------
Write-Info.ps1 "Configure Group Policy for Claims"

$gpoFolder = Get-ChildItem -Path $gpoBackupFolder
$gpoGuid = $gpoFolder.Name.Replace("{","").Replace("}","")

Write-Info.ps1 "Configurating Group Policy"
Import-GPO -BackupId $gpoGuid -TargetName "Default Domain Policy" -Path $gpoBackupFolder -CreateIfNeeded

Write-Info.ps1 "Publish the group policy updates to all computers by command: gpupdate /force"
CMD /C gpupdate /force 2>&1 | Write-Info.ps1

#----------------------------------------------------------------------------
# Ending
#----------------------------------------------------------------------------
Stop-Transcript
exit 0