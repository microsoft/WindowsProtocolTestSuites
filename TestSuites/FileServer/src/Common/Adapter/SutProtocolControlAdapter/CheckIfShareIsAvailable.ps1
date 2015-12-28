#############################################################################
## Copyright (c) Microsoft. All rights reserved.
## Licensed under the MIT license. See LICENSE file in the project root for full license information.
#############################################################################

[string]$share
[string]$domain = ${PtfPropCommon.DomainName}
[string]$userName = ${PtfPropCommon.AdminUserName}
[string]$password = ${PtfPropCommon.PasswordForAllUsers}

if([System.String]::IsNullOrEmpty($domain))
{
	$account = $UserName
}
else
{
    $account = "$Domain\$UserName"
}

$result = $FALSE
Try
{
	CMD /C "net.exe use $share $password /user:$account"
	# LastExitCode 0 - Connect succeed 
	# LastExitCode 2 - Get error "multiple connections to a server or shared resource by the same user", but this error does not block New-Item
	if($LastExitCode -eq 0 -or $LastExitCode -eq 2)
	{
	    $result = $TRUE
	}
}
Catch
{
    $result = $FALSE
}
Finally
{
   CMD /C "net.exe use $share /delete /yes" | out-null
}

return $result