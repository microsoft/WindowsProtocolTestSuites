# Copyright (c) Microsoft. All rights reserved.
# Licensed under the MIT license. See LICENSE file in the project root for full license information.

##############################################################################
#
# Microsoft Windows Powershell Scripting
# File:           Prefix-ConfigFileNode.ps1
# Purpose:        Add a prefix to the existing value of a node in the .ptfconfig filevalue.
# Requirements:   Windows Powershell 2.0
# Supported OS:   Windows Server 2008 R2, Windows Server 2012, Windows Server 2012 R2,
#                 Windows Server 2016 and later
#
##############################################################################

Param(
[String]$sourceFileName,
[String]$nodeName,
[String]$prefix
)

#----------------------------------------------------------------------------
# Starting script
#----------------------------------------------------------------------------
Write-Host "EXECUTING [Prefix-ConfigFileNode.ps1] ..." -foregroundcolor cyan
Write-Host "`$sourceFileName = $sourceFileName"
Write-Host "`$nodeName          = $nodeName"
Write-Host "`$prefix            = $prefix"

#----------------------------------------------------------------------------
# Function: Show-ScriptUsage
# Usage   : Describes the usage information and options
#----------------------------------------------------------------------------
function Show-ScriptUsage
{    
    Write-host 
    Write-host "This script used to add a prefix to the existing value of a node in the .ptfconfig file."
    Write-host "The path of the config must be an absolute directory path"
    Write-host
    Write-host "Example: Prefix-ConfigFileNode.ps1 C:\MS-XCA\MS-XCA.deployment.ptfconfig PLAIN_LZ77_COMPRESSION_COMMAND dotnet"
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
$newContent = $prefix


#----------------------------------------------------------------------------
# Modify the content of the node
#----------------------------------------------------------------------------
$ifFileExist = $false
$IsFindNode = $false

$ifFileExist = Test-Path $sourceFileName
if($ifFileExist -eq $true)
{   
    [xml]$configContent = Get-Content $sourceFileName
    $PropertyNodes = $configContent.GetElementsByTagName("Property")
    foreach($node in $PropertyNodes)
    {
        if($node.GetAttribute("name") -eq $nodeName)
        {
            $newContent = $prefix + $node.GetAttribute("value")
            $node.SetAttribute("value",$newContent)
            $IsFindNode = $true
            break
        }
    }
    
    if($IsFindNode)
    {
        Set-ItemProperty -Path $sourceFileName -Name IsReadOnly -Value $false
        $configContent.save((Resolve-Path $sourceFileName))
    }
    else
    {
        Throw "Config failed: Can't find the node with name attribute '$nodeName''" 
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
