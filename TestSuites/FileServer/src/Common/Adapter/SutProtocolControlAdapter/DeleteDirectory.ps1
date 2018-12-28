#############################################################################
## Copyright (c) Microsoft. All rights reserved.
## Licensed under the MIT license. See LICENSE file in the project root for full license information.
#############################################################################

[string]$share
[string]$directoryName
[string]$domain = ${PtfPropCommon.DomainName}
[string]$userName = ${PtfPropCommon.AdminUserName}
[string]$password = ${PtfPropCommon.PasswordForAllUsers}

if([System.String]::IsNullOrEmpty($domain))
{
	$account = $UserName
}
else
{
    $NetBiosName = $Domain.Split(".")[0]
    $account = "$NetBiosName\$UserName"
}

$exist = Test-Path -Path "$share\$directoryName"
if ($exist -eq $true)
{
	Try
	{
		CMD /C "net.exe use $share $password /user:$account"
		Get-ChildItem -Path "$share\$directoryName" -Recurse | Remove-Item -force -recurse
		Remove-Item "$share\$directoryName" -force
	}
	Finally
	{
		CMD /C "net.exe use $share /delete /yes" | out-null	
	}
}
