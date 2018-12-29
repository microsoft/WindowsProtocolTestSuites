##################################################################################
## Copyright (c) Microsoft Corporation. All rights reserved.
## Licensed under the MIT license. See LICENSE file in the project root for full license information.
##################################################################################
##
## Microsoft Windows Powershell Scripting
## File:           Modify-TestRunConfig.ps1
## Purpose:        Update .TestRunConfig file to make all deployment items' 
##                 path refer to current directory, and check if each 
##                 deployment item exists.
## Version:        1.0 (11 Sep, 2008)
##
##############################################################################

Param(
[String]$TestRunConfigPath 
)

#----------------------------------------------------------------------------
# Starting script
#----------------------------------------------------------------------------

Write-Host "EXECUTING [Modify-TestRunConfig.ps1] ..." -foregroundcolor cyan
Write-Host "`$TestRunConfigPath       = $TestRunConfigPath"

#----------------------------------------------------------------------------
# Function: Show-ScriptUsage
# Usage   : Describes the usage information and options
#----------------------------------------------------------------------------
function Show-ScriptUsage
{    
    Write-host 
    Write-host "Usage: Update the deployment items' path of .TestRunConfig file"
    Write-host "Parm1: Path of the .TestRunConfig file"
    Write-host
    Write-host "Example: Modify-TestRunConfig.ps1 C:\Test\Bin"
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

#-----------------------------------------------------
# Validation
#-----------------------------------------------------
if(!$TestRunConfigPath -or !(Test-Path $TestRunConfigPath -PathType Container))
{
    throw "Test run configuration file path not exists."
}

#-----------------------------------------------------
# Definition
#-----------------------------------------------------
function FileStringReplace
(
[string]$filePath,
[string]$origStr,
[string]$newStr,
[bool]  $useRegularExpression = $false
)
{
    if(!(Test-Path -Path $filePath -PathType Leaf))
    {
        throw File ($filePath) not found.
    }
    if(!$origStr)
    {
        throw String to be replaced can not be empty.
    }
    if(!$newStr)
    {
        throw New string can not be empty.
    }
    
    $regex = New-Object System.Text.RegularExpressions.Regex -ArgumentList "$origStr"

    $tmpFile = ($filePath + ".tmp")
    $lines = Get-Content $filePath
    foreach($line in $lines)
    {
        $newLine = $line
        if($useRegularExpression -eq $true)
        {
            $newLine = $regex.Replace($line, $newStr)
        }
        else
        {            
            $newLine = $line.Replace($origStr, $newStr)
        }
        Add-Content $tmpFile $newLine
    }
    Remove-Item $filePath
    if($filePath.Contains("\"))
    {
       $filePath = $filePath.Remove(0, $filePath.LastIndexOf("\") + 1)
    }
    Rename-Item $tmpFile $filePath
}

#-----------------------------------------------------
# Begin to update the paths
#-----------------------------------------------------
Write-Host Update paths...
$nameList1 = ((Get-Childitem $TestRunConfigPath -Include *.testsettings -Recurse -Force) | Select FullName)
$nameList2 = ((Get-Childitem $TestRunConfigPath -Include *.testrunconfig -Recurse -Force) | Select FullName)

Foreach($nameList in ($nameList1,$nameList2))
{
    Foreach($file in $nameList)
    {
        $trcFile = $file.FullName
        Write-Host $trcFile
        FileStringReplace $trcFile "<DeploymentItem filename=`".*`\\" "<DeploymentItem filename=`".\" $true
            
        Write-Host Verify deployment items...
        $cfgContent = [System.IO.File]::ReadAllText($trcFile)
        $regex = New-Object System.Text.RegularExpressions.Regex -ArgumentList "<DeploymentItem filename=.*`\\.*`""
        $matches = $regex.Matches($cfgContent)
        Foreach($match in $matches)
        {
            $value = $match.Value.Replace("<DeploymentItem filename=", "");
            $value = $value.Replace("`"", "");
            Write-Host ("    " + $value + "    ") -NoNewline
            if(Test-Path -Path $value -PathType Leaf)
            {
                Write-Host OK -ForegroundColor Green
            }
            else
            {
                Write-Host NOT EXISTS -ForegroundColor Red
            }
        }
        
        Write-Host "Delete the CodeCoverage node..."
        [XML]$runconfigFile = Get-Content $trcFile
        $TestRunConfigurationNodeList = $runconfigFile.GetElementsByTagName("TestRunConfiguration")
        if($TestRunConfigurationNodeList -ne $null -and $TestRunConfigurationNodeList.Count -gt 0)
        {
            $TestRunConfigurationNode = $TestRunConfigurationNodeList.Item(0)
            $CodeCoverageNodeList     = $TestRunConfigurationNode.GetElementsByTagName("CodeCoverage")
            if($CodeCoverageNodeList -ne $null -and $CodeCoverageNodeList.Count -gt 0)
            {
                $CodeCoverageNode = $CodeCoverageNodeList.Item(0)
                Write-Host "CodeCoverage node found..."
                $TestRunConfigurationNode.RemoveChild($CodeCoverageNode)
                $runconfigFile.Save($trcFile)
            }
            else
            {
                Write-Host "CodeCoverage node not found..."
            }
        }
    }
}

#----------------------------------------------------------------------------
# Ending script
#----------------------------------------------------------------------------
Write-Host "Config finished."
Write-Host "EXECUTE [Modify-TestRunConfig.ps1] FINISHED (NOT VERIFIED)."