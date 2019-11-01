#############################################################################
## Copyright (c) Microsoft. All rights reserved.
## Licensed under the MIT license. See LICENSE file in the project root for full license information.
#############################################################################

[string]$share
[string]$directoryName
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

$exist = Test-Path -Path "$share\$directoryName"
if ($exist -eq $true)
{
	Try
	{
		CMD /C "net.exe use $share $password /user:$account"
		if (Test-Path  -Path "$share\$directoryName" )
		{
			Remove-Item -path "$share\$directoryName" -Force -Recurse -ErrorAction SilentlyContinue
		}
	}
	Finally
	{
		CMD /C "net.exe use $share /delete /yes" | out-null	
	}
}
