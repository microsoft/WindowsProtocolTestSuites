#############################################################################
# Copyright (c) Microsoft. All rights reserved.
# Licensed under the MIT license. See LICENSE file in the project root for full license information.
##############################################################################
Param(
[String]$ptfConfigFileName,
[String]$nodeName,
[String]$newContent,
[String]$signFileName,
[String]$pftFilePath = ""
)

#----------------------------------------------------------------------------
# Starting script
#----------------------------------------------------------------------------
Write-Host "EXECUTING [Modify-PTFConfigFileNode.ps1] ..." -foregroundcolor cyan
Write-Host "`$ptfConfigFileName = $ptfConfigFileName"
Write-Host "`$nodeName          = $nodeName"
Write-Host "`$newContent        = $newContent"

#----------------------------------------------------------------------------
# Function: Show-ScriptUsage
# Usage   : Describes the usage information and options
#----------------------------------------------------------------------------
function Show-ScriptUsage
{    
    Write-host 
    Write-host "This script used to modify a node value of the .ptfconfig file."
    Write-host
    Write-host "Example: Modify-PTFConfigFileNode.ps1 RDP_ClientTestSuite.deployment.ptfconfig RDP.Security.Protocol TLS"
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
# Get the full path of PTF config file
#----------------------------------------------------------------------------
Write-Host "Get the scripts path from MSIInstalled.signal file"
$signalFile  = "$env:HOMEDRIVE/MSIInstalled.signal"

$ptfConfigFileFullPath =""
if($pftFilePath -ne ""){
    $ptfConfigFileFullPath = "$pftFilePath/Bin/$ptfConfigFileName"
}elseif(Test-Path -Path $signalFile){
    $TestSuiteScriptsFullPath = Get-Content $signalFile
    $ptfConfigFileFullPath = "$TestSuiteScriptsFullPath/../Bin/$ptfConfigFileName"
}else{
    Write-Host "MSI has not been installed. please check"
    return
}

#----------------------------------------------------------------------------
# Modify the content of the node
#----------------------------------------------------------------------------
$scriptsPath  = [System.IO.Path]::GetDirectoryName($myInvocation.MyCommand.Definition)

.$scriptsPath/Modify-ConfigFileNode.ps1 $ptfConfigFileFullPath $nodeName $newContent

Write-Host  "Write signal file to system drive."
Write-Output "Update PTF config file node Finished. $nodeName, $newContent" > $env:HOMEDRIVE/$signFileName

exit 0