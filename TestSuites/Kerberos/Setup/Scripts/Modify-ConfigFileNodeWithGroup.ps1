#############################################################################
## Copyright (c) Microsoft Corporation. All rights reserved.
## Licensed under the MIT license. See LICENSE file in the project root for full license information.
#############################################################################
#############################################################################
##
## Microsoft Windows Powershell Scripting
## Purpose: Modify the node value for the ".deployment.ptfconfig" file.
##
##############################################################################

Param(
[string]$sourceFileName, 
[string]$nodeName, 
[string]$newContent
)

#----------------------------------------------------------------------------
# Global variables
#----------------------------------------------------------------------------
$env:Path += ";c:\temp;c:\temp\Scripts"

#----------------------------------------------------------------------------
# Starting script
#----------------------------------------------------------------------------
Write-Info.ps1 "Modify-ConfigFileNode: `$sourceFileName: $sourceFileName `$nodeName:$nodeName `$newContent: $newContent" -foregroundcolor cyan

#----------------------------------------------------------------------------
# Function: Show-ScriptUsage
# Usage   : Describes the usage information and options
#----------------------------------------------------------------------------
function Show-ScriptUsage
{    
    Write-Info.ps1 
    Write-Info.ps1 "This script used to modify a node value of the .deployment.ptfconfig file.The Path of the config must be a absoluted directory!"
    Write-Info.ps1
    Write-Info.ps1 "The Path of the config file must be a absoluted path!"
    Write-Info.ps1
    Write-Info.ps1 "Example: Modify-ConfigFileNode.ps1 C:\MS-WDVSE.deployment.ptfconfig hostName SUT04"
    Write-Info.ps1
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
    $nodes = $configContent.TestSite.Properties
    [Array]$propName = $nodeName.Split('.')
    for($i = 0; $i -lt $propName.Count; $i++)
    {
        $findNodeName = $propName[$i]
        $node = $nodes.ChildNodes | where{$_.name -eq $findNodeName}
        while(($node -eq $null) -and ($i -lt $propName.Count - 1))
        {
            $i++
            $findNodeName += "." + $propName[$i]         
            $node = $nodes.ChildNodes | where{$_.name -eq $findNodeName}
        }
        $nodes = $node
    }

    if(($node -ne $null) -and ($nodeName.EndsWith($node.GetAttribute("name"))))
    {
        $node.SetAttribute("value",$newContent)
        $IsFindNode = $true
    }
    
    if($IsFindNode)
    {
        $configContent.save($sourceFileName)
    }
    else
    {
        Throw "Config failed: Can't find the node whoes name attribute is $nodeName" 
    }
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
    $nodes = $configContent.TestSite.Properties
    [Array]$propName = $nodeName.Split('.')
    for($i = 0; $i -lt $propName.Count; $i++)
    {
        $findNodeName = $propName[$i]       
        $node = $nodes.ChildNodes | where{$_.name -eq $findNodeName}
        while(($node -eq $null) -and ($i -lt $propName.Count - 1))
        {
            $i++
            $findNodeName += "." + $propName[$i]            
            $node = $nodes.ChildNodes | where{$_.name -eq $findNodeName}
        }
        $nodes = $node
    }

    if($nodeName.EndsWith($node.GetAttribute("name")))
    {
        if($node.GetAttribute("value") -eq $newContent)
        {
            return
        }    
    }

    Write-Error "Config failed: Please check if the $sourceFileName is readonly." 
    Throw "EXECUTE [Modify-ConfigFileNode.ps1] FAILED."
}

exit
