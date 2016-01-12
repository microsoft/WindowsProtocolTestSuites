#############################################################################
## Copyright (c) Microsoft. All rights reserved.
## Licensed under the MIT license. See LICENSE file in the project root for full license information.
##
## Microsoft Windows Powershell Scripting
## File:           Create-Folder.ps1
## Purpose:        Create a new folder
## Version:        1.1 (26 Jun, 2008])
##
##############################################################################

param(
[string] $targetFolder
)

#----------------------------------------------------------------------------
# Print execution information
#----------------------------------------------------------------------------
Write-Host "EXECUTING [Create-File.ps1]..." -foregroundcolor cyan
Write-Host "`$targetFolder = $targetFolder" 

#----------------------------------------------------------------------------
# Function: Show-ScriptUsage
# Usage   : Describes the usage information and options
#----------------------------------------------------------------------------
function Show-ScriptUsage
{    
    Write-host 
    Write-host "Usage: This scripts will create a new folder."
    Write-host
    Write-host "Example: Create-File.ps1 c:\target"
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
#This methon requires the folder set in driver C or D.
if($targetFolder.Length -eq [int]0)
{
    Throw "The folder'name is empty."
}
else
{
    $ifParameterLegal = $false
    #Check the parameter legal
    $tempDrivers = $targetFolder.Split(':')[0]

    $ifParameterLegal = $true

    #Check the parameter legal
}

#----------------------------------------------------------------------------
# EXECUTION
#----------------------------------------------------------------------------
if($ifParameterLegal -eq $true)
{
    $ifFolderExisting = $false
    [string[]] $folderArray = $targetFolder.Split('\')
    for([int]$i = 0; $i -lt $folderArray.Count ; $i = $i+1)
    {
        $ifHaveSubFolder = $false
        if($i -eq [int]0)
        {
            [string]$parentPathName =  $folderArray[$i]
        }
        else
        {
            $tempObjFolders = Get-Item ($parentPathName+"\*")
            foreach($tempObjFolder in $tempObjFolders)
            {
                if($tempObjFolder.Name -eq $folderArray[$i])
                {
                    $ifHaveSubFolder = $true
                }
            }
            $parentPathName = $parentPathName + "\" + $folderArray[$i]
            if($ifHaveSubFolder -eq $true)
            {
                Write-Host "The Path:"$parentPathName "has already existed."
            }
            else
            {
                new-item -Type Directory -Path $parentPathName | out-null
            }
        }
    }
    Write-Host "Succeed to create a new folder with the name:" $targetFolder "."
}
else
{
    Write-Host "The folder's name "$targetFolder" has mistake."
}

#----------------------------------------------------------------------------
# Print Exit information
#----------------------------------------------------------------------------
Write-Host "EXECUTE [Create-Folder.ps1] FINISHED (NOT VERIFIED)." -foregroundcolor yellow

exit 0