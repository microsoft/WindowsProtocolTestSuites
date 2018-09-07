##################################################################################
## Copyright (c) Microsoft Corporation. All rights reserved.
## Licensed under the MIT license. See LICENSE file in the project root for full license information.
##################################################################################

#############################################################################
##
## Microsoft Windows Powershell Scripting
## File:           Set-MsDsBehaviorVersion.ps1
## Purpose:        Set msDS-Behavior-Version attribute.
## Requirements:   Windows Powershell 2.0
## Supported OS:   Windows Server 2008 R2, Windows Server 2012, Windows Server 2012 R2,
##                 Windows Server 2016 and later
##
##############################################################################

Param
(
    [Parameter(Mandatory=$true)]
    [ValidateNotNullOrEmpty()]
	[string]$Domain,

    [Parameter(Mandatory=$true)]
    [ValidateNotNullOrEmpty()]
	[string]$ForestFuncLvl
)

$DomainNc = "DC=" + $Domain.Replace(".", ",DC=")
$Attribute = Get-ADObject -Identity "CN=Partitions,CN=Configuration,$DomainNc" -Properties * | Select "msds-Behavior-Version" 
$Version = $Attribute.'msds-Behavior-Version' 
Write-Host "Current msds-Behavior-Version level of forest is`: $Version"
Write-Host "Targeting forest functional level is`: $ForestFuncLvl"
if($Version -ne $ForestFuncLvl)
{
    Write-Host "Set msDS-Behavior-Version of forest to $ForestFuncLvl"
    $DataPath = ".\Set-MsDsBehaviorVersion.txt"
    if(Test-Path -Path $DataPath)
    {
        Remove-item $DataPath
    }
    $DataFile = New-Item -Type file -Path $DataPath
    "dn:cn=partitions,cn=configuration,$DomainNc">>$DataFile
    "changetype:modify">>$DataFile
    "replace:msDS-Behavior-Version">>$DataFile
    "msDS-Behavior-Version:$ForestFuncLvl">>$DataFile
    "-">>$DataFile
    cmd.exe /c ldifde -v -i -f $DataPath | Write-Output
}