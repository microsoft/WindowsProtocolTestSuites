#############################################################################
## Copyright (c) Microsoft. All rights reserved.
## Licensed under the MIT license. See LICENSE file in the project root for full license information.
##
## Microsoft Windows Powershell Scripting
## File:           Modify-ConfigGroupFileNode.ps1
## Purpose:        Modify the node value for the ".deployment.ptfconfig" file.
## Version:        1.1 (26 June, 2008)
##
##############################################################################

Param(
[string]$sourceFileName, 
[string]$groupName,
[string]$nodeName, 
[string]$newContent
)

#----------------------------------------------------------------------------
# Starting script
#----------------------------------------------------------------------------
Write-Host "EXECUTING [Modify-ConfigGroupFileNode.ps1] ..." -foregroundcolor cyan
Write-Host "`$sourceFileName = $sourceFileName"
Write-Host "`$groupName      = $groupName"
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
    Write-host "Example: Modify-ConfigGroupFileNode.ps1 C:\MS-WDVSE.deployment.ptfconfig hostName SUT04"
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
if ($groupName -eq $null -or $groupName -eq "")
{
    Throw "Parameter groupName is required."
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
	$groupNode = $configContent.TestSite.Properties.Group | where {$_.name -eq $groupName}
	$node = $groupNode.Property | where {$_.name -eq $nodeName}
	if($node -eq $null){
		Throw "Config failed: Can't find the node whoes name attribute is $nodeName" 
	}
	$node.SetAttribute("value",$newContent)
	$configContent.save($sourceFileName)
	attrib +R $sourceFileName
}
else
{
    Throw "Config failed: The config file $sourceFileName does not existed!" 
}

#----------------------------------------------------------------------------
# Verify the result
#----------------------------------------------------------------------------
if($ifFileExist -eq $true)
{
	[xml]$configContent = Get-Content $sourceFileName
	$groupNode = $configContent.TestSite.Properties.Group | where {$_.name -eq $groupName}
	$node = $groupNode.Property | where {$_.name -eq $nodeName}
	if($node -eq $null){
		Throw "Config failed: Can't find the node whoes name attribute is $nodeName" 
	}
	
	if($node.GetAttribute("value") -eq $newContent)
    {
        Write-Host "Config success: Set $nodeName to $newContent" -ForegroundColor green
		return
	}else{
		Write-Error "Config failed: Please check if the $sourceFileName is readonly." 
		Throw "EXECUTE [Modify-ConfigGroupFileNode.ps1] FAILED."
	}
}

#----------------------------------------------------------------------------
# Ending script
#----------------------------------------------------------------------------
Write-Host "EXECUTE [Modify-ConfigGroupFileNode.ps1] SUCCEED." -foregroundcolor green

exit
