#############################################################################
## Copyright (c) Microsoft. All rights reserved.
## Licensed under the MIT license. See LICENSE file in the project root for full license information.
##
## Microsoft Windows Powershell Sripting
## File:           ReplacePauses.ps1
## Requirements:   Windows Powershell 2.0
## Supported OS:   Windows Server 2012
##
##############################################################################

#----------------------------------------------------------------------------
#
# This script removes all the PAUSEs in the batch file
# so that the batch closes automatically after execution
#
#----------------------------------------------------------------------------

#----------------------------------------------------------------------------
#
#  Function: Replace Pause in the scripts.
#  Parameter:
#           $contentPath:The file's full path
#
#----------------------------------------------------------------------------
Function ReplacePause
(
    [string]$contentPath
)
{
    [bool]$IsReplaced = $False 
    write-host "`$contentPath is $contentPath"
    $scriptsContents = Get-Content $contentPath
    $tempscriptsContents = ""
    foreach($scriptscontent in $scriptsContents)
    {
        #Only Replace "cmd /c pause" and "Pause"
        if($scriptscontent.ToLower().Contains(" pause") -or ($scriptscontent.ToLower().Trim() -eq "pause") )
        {
            $IsReplaced = $True
            $scriptscontent = ""
        }
        $tempscriptsContents = $tempscriptsContents + $scriptscontent + "`r`n"
    }
    #When the content is changed,set the content. 
    if($IsReplaced)
    {
        Set-Content $contentPath $tempscriptsContents
    }
}

#----------------------------------------------------------------------------
# Get MSI Full Path
#----------------------------------------------------------------------------
Write-Host "Get the scripts path from MSIInstalled.signal file"
$signalFile  = "$env:HOMEDRIVE\MSIInstalled.signal"
if (Test-Path -Path $signalFile)
{
    $MSIFullPath = Get-Content $signalFile
}
else
{
    Write-Host "MSI has not been installed. please check"
    exit 0
}

#----------------------------------------------------------------------------
# Replace PAUSEs
#----------------------------------------------------------------------------
Write-Host "Replace pause in the scripts"
$scripts = Get-ChildItem $MSIFullPath
foreach($script in $scripts)
{
    if (($script.Attributes -ne "Directory") -and ($script.Name.EndsWith(".ps1") -or $script.Name.EndsWith(".bat") -or $script.Name.EndsWith(".cmd")))
    {
        $scriptPath = $MSIFullPath + "\" + $script.Name
        ReplacePause $scriptPath
    }
}

$BatchFolder = "$MSIFullPath\..\Batch"
if (Test-Path -Path $BatchFolder)
{
    Write-Host "Replace pause in the batch folder"
    $batches = Get-ChildItem $BatchFolder
    foreach($batch in $batches)
    {
        if (($batch.Attributes-ne "Directory") -and ($batch.Name.EndsWith(".bat") -or $batch.Name.EndsWith(".cmd")))
        {
            $batchPath = $BatchFolder + "\" + $batch.Name
            ReplacePause $batchPath
        }
    }
}

Write-Host "All PAUSEs in batch files are successfully removed."
exit 0
