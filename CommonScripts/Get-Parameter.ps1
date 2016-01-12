#############################################################################
## Copyright (c) Microsoft. All rights reserved.
## Licensed under the MIT license. See LICENSE file in the project root for full license information.
##
## Microsoft Windows Powershell Scripting
## Purpose: Get Parameter which used in Protocol in XML file.
##
##############################################################################

Param(
[string]$sourceFileName,
[String]$attrName,
[String]$defaultValue     = ""
)

# Write Call Stack
if($function:EnterCallStack -ne $null)
{
	EnterCallStack "Get-Parameter.ps1"
}

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
if($sourceFileName.Contains(":") -eq $false -and $sourceFileName.Contains("\\") -eq $false)
{
    $currentPath = Split-Path $MyInvocation.MyCommand.Definition -parent  
    $fileName = Split-Path $sourceFileName -leaf
    $sourceFileName = "$currentPath\$fileName"
}


#----------------------------------------------------------------------------
# Get parameter
#----------------------------------------------------------------------------
$fileExist =  Test-Path $sourceFileName
if($fileExist -eq $false)
{
	# Write Call Stack
	if($function:ExitCallStack -ne $null)
	{
		ExitCallStack "Get-Parameter.ps1"
	}
    return $defaultValue
}
else
{
    [String]$retValue = ""

    [xml]$content = Get-Content $sourceFileName
	$newPara = $TRUE
	
    if($content -eq $NULL) 
    {
        $retValue = $defaultValue
    }
    elseif($content.SelectNodes("Parameters") -eq $NULL -or $content.SelectNodes("Parameters") -eq "")
    {        
        $Parameters = $content.SelectNodes("lab/Parameters")
			if($Parameters -eq $NULL -or $Parameters -eq "")
			{
				$retValue = $defaultValue
    		}
			else
			{				
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
			}
	}else
	{
		
		$propertyNodes = $content.GetElementsByTagName("Parameter")

		if($propertyNodes -eq $null -or $propertyNodes -eq "")
		{
			$retValue = $defaultValue
		}else
		{
			 foreach($node in $propertyNodes)
			{
				if($node.Name -eq $attrName)
				{
					$newPara = $FALSE
					$retValue = $node.Value
					break    
				}
			}
		}

	}
        if($newPara)
        {
            $retValue = $defaultValue
        }
        
    
	# Write Call Stack
	if($function:ExitCallStack -ne $null)
	{
		ExitCallStack "Get-Parameter.ps1"
	}
    return $retValue
}