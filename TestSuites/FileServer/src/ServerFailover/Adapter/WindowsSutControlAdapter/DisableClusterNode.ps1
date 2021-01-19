# Copyright (c) Microsoft. All rights reserved.
# Licensed under the MIT license. See LICENSE file in the project root for full license information.

[string]$nodeName
[string]$domain = $PtfProp_Common_DomainName
[string]$userName = $PtfProp_Common_AdminUserName
[string]$password = $PtfProp_Common_PasswordForAllUsers

if($nodeName -notmatch "\.")
{
	$nodeName = "$nodeName.$domain"
}

$clusterServiceName = "ClusSvc"

try
{
    Get-PSSession|Remove-PSSession
    $psSession=New-PSSession -HostName $nodeName -UserName "$domain\$userName"
}
catch
{
    Get-Error
}

$myScriptBlock = {
    param(
    [string] $className,
    [string] $filter
    )
    $result = Get-CimInstance -ClassName $className -Filter $filter
    if($result -eq $null)
    {
        return $FALSE
    }

    return $result | Invoke-CimMethod -Name StopService
}

$ret = $FALSE
try
{
    $className="Win32_Service"
    $filter="Name = '$clusterServiceName'"
    $result = Invoke-Command -Session $psSession -ScriptBlock $myScriptBlock -ArgumentList $className,$filter
    $ret = $TRUE
}
catch
{
    Get-Error
}
finally
{
    Get-PSSession|Remove-PSSession
}
return $ret