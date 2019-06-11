######################################################################################################
## Copyright (c) Microsoft. All rights reserved.
## Licensed under the MIT license. See LICENSE file in the project root for full license information.
######################################################################################################

[string]$domain = $PtfProp_DomainName
[string]$userName = $PtfProp_UserName
[string]$password = $PtfProp_UserPassword

$SecurePassword = New-Object System.Security.SecureString
if($Domain -eq $null -or $Domain.trim() -eq "")
{
	$account = $UserName
}
$account = "$Domain\$UserName"
for($i=0; $i -lt $Password.Length; $i++)
{
	$SecurePassword.AppendChar($Password[$i])
}
return New-Object system.Management.Automation.PSCredential($account,$SecurePassword)
