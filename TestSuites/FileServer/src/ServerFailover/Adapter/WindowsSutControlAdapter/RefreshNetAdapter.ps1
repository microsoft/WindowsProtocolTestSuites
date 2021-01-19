# Copyright (c) Microsoft. All rights reserved.
# Licensed under the MIT license. See LICENSE file in the project root for full license information.

[string]$IPAddress
[string]$nodeName
[string]$domain = $PtfProp_Common_DomainName
[string]$userName = $PtfProp_Common_AdminUserName
[string]$password = $PtfProp_Common_PasswordForAllUsers

if($nodeName -notmatch "\.")
{
	$nodeName = "$nodeName.$domain"
}

try
{
    Get-PSSession|Remove-PSSession
    $psSession=New-PSSession -HostName $nodeName -UserName "$domain\$userName"
}
catch
{
    Get-Error
}

$command = {
    Param
    (
    [String]$ip
    )
    $IPConfig = Get-NetIPAddress -IPAddress $ip
    $adapter = Get-NetAdapter -InterfaceIndex $IPConfig.InterfaceIndex
    Disable-NetAdapter -Name $adapter.Name -Confirm:$false
	Start-Sleep -s 10
	Enable-NetAdapter -Name $adapter.Name -Confirm:$false
}

$ret = $FALSE
try
{
    $result = Invoke-Command -Session $psSession -ScriptBlock $command -ArgumentList $IPAddress
    $ret = $TRUE
}
catch
{
	Get-Error
    throw "Failed to refresh NetAdapter."
}
finally
{
    Get-PSSession|Remove-PSSession
}
return $ret