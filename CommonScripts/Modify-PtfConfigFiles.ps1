##################################################################################
## Copyright (c) Microsoft Corporation. All rights reserved.
## Licensed under the MIT license. See LICENSE file in the project root for full license information.
##################################################################################

#############################################################################
##
## Microsoft Windows Powershell Scripting
## File:           Modify-PtfConfigFiles.ps1
## Purpose:        Modify PtfConfig files in both \bin and source\server\testcode\testsuite folders with specified property name and value.
## Requirements:   Windows Powershell 2.0
## Supported OS:   Windows Server 2008 R2, Windows Server 2012, Windows Server 2012 R2,
##                 Windows Server 2016 and later
##
##############################################################################

Param
(
    [parameter(Mandatory=$true)]
    [ValidateNotNullOrEmpty()]
    [System.IO.FileInfo[]]$Files,

    [parameter(Mandatory=$true)]
    [ValidateNotNullOrEmpty()]
    [string]$ProperyName,

    [parameter(Mandatory=$true)]
    [ValidateNotNullOrEmpty()]
    [string]$ProperyValue
)

foreach ($File in $Files)
{
    Write-Host "Modifying file: $($File.FullName)..." -ForegroundColor Yellow

    Write-Host "Turn off Read-Only arribute"
    Set-ItemProperty -Path $File.FullName -Name IsReadOnly -Value $false

    Write-Host "Modify property: $ProperyName with value: $ProperyValue"
    [xml]$FileContent = Get-Content $File.FullName
    $PropertyNodes = $FileContent.GetElementsByTagName("Property")
    foreach($PropertyNode in $PropertyNodes)
    {
        if($PropertyNode.GetAttribute("name") -eq $ProperyName)
        {
            $PropertyNode.SetAttribute("value", $ProperyValue)
            $IsNodeFound = $true
            break
        }
    }
    if($IsNodeFound)
    {
        $FileContent.Save($File.FullName)
    }
    else
    {
        throw "Set PtfConfig failed: Cannot find the node whose name attribute is $ProperyName"
    }

    Write-Host "Turn on Read-only attribute"
    Set-ItemProperty -Path $File.FullName -Name IsReadOnly -Value $true
}