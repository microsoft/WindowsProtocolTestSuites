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

$result = $FALSE
Try
{
	CMD /C "net.exe use $share $password /user:$account"
	# LastExitCode 0 - Connect succeed 
	# LastExitCode 2 - Get error "multiple connections to a server or shared resource by the same user", but this error does not block New-Item
	if($LastExitCode -eq 0 -or $LastExitCode -eq 2)
	{
	    $ret = New-Item "$share\$directoryName" -type Directory -force
		if($ret -ne $null)
		{
		    $result = $TRUE
		}
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