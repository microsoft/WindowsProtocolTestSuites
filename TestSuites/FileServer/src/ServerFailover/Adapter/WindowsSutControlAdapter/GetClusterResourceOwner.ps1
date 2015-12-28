#############################################################################
## Copyright (c) Microsoft. All rights reserved.
## Licensed under the MIT license. See LICENSE file in the project root for full license information.
#############################################################################

[string]$resName
[string]$clusterName = ${PtfPropCluster.ClusterName}
[string]$domain = ${PtfPropCommon.DomainName}
[string]$userName = ${PtfPropCommon.AdminUserName}
[string]$password = ${PtfPropCommon.PasswordForAllUsers}

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
$credential = New-Object system.Management.Automation.PSCredential($account,$SecurePassword)

if ($resName -eq $null -or $resName -eq "")
{
	throw "Invalid cluster resource name"
}
else
{
	Try
	{
		$ret = Get-WMIObject -ComputerName $clusterName -Authentication PacketPrivacy -Namespace "root\mscluster" -Class MSCluster_Resource -Credential $credential | Where-Object{$resName -match $_.Name}
		if($ret -ne $null -and $ret.State -eq 2)
		{
			return $ret.OwnerNode
		}
		else
		{
			return $null
		}
	}
	Catch
	{
		return $null
	}
}
