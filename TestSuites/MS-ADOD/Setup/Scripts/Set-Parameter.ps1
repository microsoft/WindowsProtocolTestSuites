#############################################################################
## Copyright (c) Microsoft. All rights reserved.
## Licensed under the MIT license. See LICENSE file in the project root for full license information.
#############################################################################

#############################################################################
##
## Microsoft Windows PowerShell Scripting
## Purpose: Set Parameter which used in Protocol in XML file.
##
##############################################################################

Param(
[string]$sourceFileName,
[String]$attrName,
[string]$value,
[string]$comment = ""
)

#----------------------------------------------------------------------------
# Function: Show-ScriptUsage
# Usage   : Describes the usage information and options
#----------------------------------------------------------------------------
function Show-ScriptUsage
{    
    Write-host 
    Write-host "Modify an attribute of node in XML file."
    Write-host
    Write-host "The Path of the XML type file must be a absoluted path!"
    Write-host
    Write-host "Example: Set-Parameter.ps1 C:\config.xml ClientOS Vista"
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
if ($value -eq $null -or $value -eq "")
{
    Throw "Parameter value is required."
}

#----------------------------------------------------------------------------
# Get the full path of $sourceFileName
#----------------------------------------------------------------------------
$fileName = Split-Path $sourceFileName -leaf
$path = Get-Location
$sourceFileName = "$path\$fileName"

#----------------------------------------------------------------------------
# Set parameter
#----------------------------------------------------------------------------
$fileExist =  Test-Path $sourceFileName
if($fileExist -eq $false)
{
    New-Item -Path $sourceFileName -Type file -Force

    [xml]$content = Get-Content $sourceFileName

    $content = "<Parameters><Parameter Name=`"$attrName`" Value=`"$value`" Comment=`"$comment`" /></Parameters>"
    
    $content.Save($sourceFileName)
}
else
{
    attrib.exe $sourceFileName -R
    [xml]$content = Get-Content $sourceFileName

    if($content -eq $NULL -or $content.Parameters -eq $NULL -or $content.Parameters -eq "")
    {
        $content = "<Parameters><Parameter Name=`"$attrName`" Value=`"$value`" Comment=`"$comment`" /></Parameters>"
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
                $node.Value = $value
                $node.Comment = $comment
                break    
            }
        }

        if($newPara)
        {
            $element = $content.CreateElement("Parameter")
            $element.SetAttribute("Name",$attrName)
            $element.SetAttribute("Value",$value)
            $element.SetAttribute("Comment",$comment)
            $content.Parameters.AppendChild($element)
        }
    }
    
    $content.Save($sourceFileName)
}

#----------------------------------------------------------------------------
# Verify the result
#----------------------------------------------------------------------------
[xml]$content = Get-Content $sourceFileName
$propertyNodes = $content.GetElementsByTagName("Parameter")
foreach($node in $propertyNodes)
{
    if($node.Name -eq $attrName)
    {
        $newValue = $node.Value
        break    
    }
}