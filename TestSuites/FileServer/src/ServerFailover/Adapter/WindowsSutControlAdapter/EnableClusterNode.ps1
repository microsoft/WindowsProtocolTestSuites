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

$ret = $FALSE
try
{
    $result = Invoke-Command -HostName $nodeName -UserName "$domain\$userName" -ScriptBlock { cmd /c net start ClusSvc }
    $ret = $TRUE
}
catch
{
    Get-Error
}
return $ret