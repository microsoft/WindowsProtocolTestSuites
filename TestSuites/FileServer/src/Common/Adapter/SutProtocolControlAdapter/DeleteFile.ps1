## Copyright (c) Microsoft. All rights reserved.
## Licensed under the MIT license. See LICENSE file in the project root for full license information.

[string]$uncSharePath
[string]$fileName
[string]$domain = $PtfProp_Common_DomainName
[string]$userName = $PtfProp_Common_AdminUserName
[string]$password = $PtfProp_Common_PasswordForAllUsers

if([System.String]::IsNullOrEmpty($domain))
{
	$account = $UserName
}
else
{
    $NetBiosName = $Domain.Split(".")[0]
    $account = "$NetBiosName\$UserName"
}

Try
{
	CMD /C "net.exe use $uncSharePath $password /user:$account"

	if (Test-Path  -Path "$uncSharePath\$fileName" )
	{
		Remove-Item "$uncSharePath\$fileName" -force -ErrorAction SilentlyContinue
	}	
}
Finally
{
    CMD /C "net.exe use $uncSharePath /delete /yes" | out-null	
}
