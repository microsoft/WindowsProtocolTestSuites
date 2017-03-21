#############################################################################
## Copyright (c) Microsoft. All rights reserved.
## Licensed under the MIT license. See LICENSE file in the project root for full license information.
#############################################################################

#############################################################################
##
## Microsoft Windows PowerShell Scripting
## File:           Modify-ConfigFileNode.ps1
## Purpose:        Modify the node value for the ".deployment.ptfconfig" file.
## Version:        1.1 (26 June, 2008)
##
##############################################################################

Param(
[string]$sourceFileName, 
[string]$nodeName, 
[string]$newContent
)

#----------------------------------------------------------------------------
# Starting script
#----------------------------------------------------------------------------
Write-Host "EXECUTING [Modify-ConfigFileNode.ps1] ..." -foregroundcolor cyan
Write-Host "`$sourceFileName = $sourceFileName"
Write-Host "`$nodeName       = $nodeName"
Write-Host "`$newContent     = $newContent"

#----------------------------------------------------------------------------
# Function: Show-ScriptUsage
# Usage   : Describes the usage information and options
#----------------------------------------------------------------------------
function Show-ScriptUsage
{    
    Write-host 
    Write-host "This script used to modify a node value of the .deployment.ptfconfig file.The Path of the config must be a absoluted directory!"
    Write-host
    Write-host "The Path of the config file must be a absoluted path!"
    Write-host
    Write-host "Example: Modify-ConfigFileNode.ps1 C:\MS-WDVSE.deployment.ptfconfig hostName SUT04"
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
if ($sourceFileName -eq $null -or $sourceFileName -eq "")
{
    Throw "Parameter sourceFileName is required."
}
if ($nodeName -eq $null -or $nodeName -eq "")
{
    Throw "Parameter nodeName is required."
}


#----------------------------------------------------------------------------
# Modify the content of the node
#----------------------------------------------------------------------------
$ifFileExist = $false
$IsFindNode = $false

$ifFileExist = Test-Path $sourceFileName
if($ifFileExist -eq $true)
{
    attrib -R $sourceFileName
    
    [xml]$configContent = Get-Content $sourceFileName
    $PropertyNodes = $configContent.GetElementsByTagName("Property")
    foreach($node in $PropertyNodes)
    {
        if($node.GetAttribute("name") -eq $nodeName)
        {
            $node.SetAttribute("value",$newContent)
            $IsFindNode = $true
            break
        }
    }
    
    if($IsFindNode)
    {
        $configContent.save($sourceFileName)
    }
    else
    {
        Throw "Config failed: Can't find the node whoes name attribute is $nodeName" 
    }

    attrib +R $sourceFileName
}
else
{
    Throw "Config failed: The config file $sourceFileName does not existed!" 
}

#----------------------------------------------------------------------------
# Verify the result
#----------------------------------------------------------------------------
if($ifFileExist -eq $true -and $IsFindNode)
{
    [xml]$configContent = Get-Content $sourceFileName
    $PropertyNodes = $configContent.GetElementsByTagName("Property")
    foreach($node in $PropertyNodes)
    {
        if($node.GetAttribute("name") -eq $nodeName)
        {
            if($node.GetAttribute("value") -eq $newContent)
            {
                Write-Host "Config success: Set $nodeName to $newContent" -ForegroundColor green
                return
            }    
        }
    }
    Write-Error "Config failed: Please check if the $sourceFileName is readonly." 
    Throw "EXECUTE [Modify-ConfigFileNode.ps1] FAILED."
}

#----------------------------------------------------------------------------
# Ending script
#----------------------------------------------------------------------------
Write-Host "EXECUTE [Modify-ConfigFileNode.ps1] SUCCEED." -foregroundcolor green

exit
