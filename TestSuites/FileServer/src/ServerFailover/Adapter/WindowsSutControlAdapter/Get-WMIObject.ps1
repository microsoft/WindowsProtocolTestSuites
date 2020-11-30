# Copyright (c) Microsoft. All rights reserved.
# Licensed under the MIT license. See LICENSE file in the project root for full license information.

##############################################################################
# Microsoft Windows Powershell Scripting
# File:           Get-WMIObject.ps1
# Purpose:        Implement Get-WMIObject in PowerShellCore
# Version:        1.0 (20 Nov, 2020])
#
##############################################################################

param(
[string] $computerName,
[string] $namespace,
[string] $className,
[string] $filter
)

try
{
    Get-PSSession|Remove-PSSession
    $domain = $PtfProp_Common_DomainName
    $userName = $PtfProp_Common_AdminUserName
    $psSession=New-PSSession -HostName $computerName -UserName "$domain\$userName"
}
catch
{
    Get-Error
}

$myScriptBlock = {
    param(
    [string] $namespace,
    [string] $className,
    [string] $filter
    )
    if([string]::IsNullOrEmpty($namespace))
    {
        $result = Get-CimInstance -ClassName $className -Filter $filter
    }
    else
    {
        $result = Get-CimInstance -Namespace $namespace -ClassName $className -Filter $filter
    }
    return $result
}

try
{
    $result = Invoke-Command -Session $psSession -ScriptBlock $myScriptBlock -ArgumentList $namespace,$className,$filter
    $result
}
catch
{
	Get-Error
}
finally
{
    Get-PSSession|Remove-PSSession
}