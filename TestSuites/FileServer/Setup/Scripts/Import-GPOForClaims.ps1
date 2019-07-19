#############################################################################
## Copyright (c) Microsoft. All rights reserved.
## Licensed under the MIT license. See LICENSE file in the project root for full license information.
##
#############################################################################
param($workingDir = "$env:SystemDrive\Temp")
#----------------------------------------------------------------------------
# Global variables
#----------------------------------------------------------------------------
$scriptPath = Split-Path $MyInvocation.MyCommand.Definition -parent
$env:Path += ";$scriptPath;$scriptPath\Scripts"
Push-Location $workingDir

#----------------------------------------------------------------------------
# Start loging using start-transcript cmdlet
#----------------------------------------------------------------------------
[string]$logFile = $MyInvocation.MyCommand.Path + ".log"
Start-Transcript -Path "$logFile" -Append -Force

#----------------------------------------------------------------------------
# Extract GPOBackup files
#----------------------------------------------------------------------------
$ZipFile = "$workingDir\Scripts\GPOBackup.zip"
$gpoBackupFolder = "$workingDir\GPOBackup"

## Expand-Archive is only supported in Powerhshell 5.0 or later
if ($psversiontable.PSVersion.Major -ge 5)
{
    .\Write-Info.ps1 "Extract GPOBackup files"
    Expand-Archive $ZipFile $gpoBackupFolder
}
else
{
    $shell = New-Object -com shell.application
    $zip = $shell.NameSpace($ZipFile)
    if(!(Test-Path -Path $gpoBackupFolder))
    {
        New-Item -ItemType directory -Path $gpoBackupFolder
    }
    $shell.Namespace($gpoBackupFolder).CopyHere($zip.items(), 0x14)
}
#----------------------------------------------------------------------------
# Configure Group Policy for Claims
#----------------------------------------------------------------------------
.\Write-Info.ps1 "Configure Group Policy for Claims"

$gpoFolder = Get-ChildItem -Path $gpoBackupFolder
$gpoGuid = $gpoFolder.Name.Replace("{","").Replace("}","")

.\Write-Info.ps1 "Update Group Policy"
$domainName = (Get-WmiObject win32_computersystem).Domain
$domain = Get-ADDomain $domainName
if($domain.name -ne "contoso") {
    Get-ChildItem -Path $gpoBackupFolder -exclude *.pol -File -Recurse | ForEach-Object {
        $content =($_|Get-Content)
        if ($content | Select-String -Pattern 'contoso') {
            $content = $content -replace 'contoso',$domain.name   
            [IO.File]::WriteAllText($_.FullName, ($content -join "`r`n"))
        }
    }
}

.\Write-Info.ps1 "Configurating Group Policy"
Import-GPO -BackupId $gpoGuid -TargetName "Default Domain Policy" -Path $gpoBackupFolder -CreateIfNeeded

.\Write-Info.ps1 "Publish the group policy updates to all computers by command: gpupdate /force"
CMD /C gpupdate /force 2>&1 | .\Write-Info.ps1

#----------------------------------------------------------------------------
# Ending
#----------------------------------------------------------------------------
Pop-Location
Stop-Transcript
exit 0