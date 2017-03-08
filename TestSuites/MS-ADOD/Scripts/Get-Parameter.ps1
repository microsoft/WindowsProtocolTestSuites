########################################################################################################
## Copyright (c) Microsoft. All rights reserved.
## Licensed under the MIT license. See LICENSE file in the project root for full license information.
########################################################################################################

#############################################################################
##
## Microsoft Windows PowerShell Scripting
## Purpose: Get Parameter which used in Protocol in XML file.
##
##############################################################################

Param(
[string]$sourceFileName,
[String]$attrName,
[String]$defaultValue     = ""
)

#----------------------------------------------------------------------------
# Function: Show-ScriptUsage
# Usage   : Describes the usage information and options
#----------------------------------------------------------------------------
function Show-ScriptUsage
{    
    Write-host 
    Write-host "Get an attribute of node in XML file."
    Write-host
    Write-host "The Path of the XML type file must be a absoluted path!"
    Write-host
    Write-host "Example: Get-Parameter.ps1 C:\config.xml ClientOS"
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
if ($attrName -eq $null -or $attrName -eq "")
{
    Throw "Parameter attribute Name is required."
}

#----------------------------------------------------------------------------
# Get the full path of $sourceFileName
#----------------------------------------------------------------------------
$fileName = Split-Path $sourceFileName -leaf
$path = Get-Location
$sourceFileName = "$path\$fileName"

#----------------------------------------------------------------------------
# Get parameter
#----------------------------------------------------------------------------
$fileExist =  Test-Path $sourceFileName
if($fileExist -eq $false)
{
    return $defaultValue
}
else
{
    [String]$retValue = ""

    [xml]$content = Get-Content $sourceFileName

    if($content -eq $NULL -or $content.Parameters -eq $NULL -or $content.Parameters -eq "")
    {
        $retValue = $defaultValue
    }
    else
    {
        $newPara = $TRUE
        $propertyNodes = $content.GetElementsByTagName("Parameter")
        foreach($node in $propertyNodes)
        {
            if($node.Name -eq $attrName)
            {
                $newPara = $FALSE
                $retValue = $node.Value
                break    
            }
        }

        if($newPara)
        {
            $retValue = $defaultValue
        }
    }
    
    return $retValue
}