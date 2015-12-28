#############################################################################
## Copyright (c) Microsoft. All rights reserved.
## Licensed under the MIT license. See LICENSE file in the project root for full license information.
#############################################################################

[string]$nodeName
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

if($clusterName -eq "" -or $nodeName -eq "")
{
    throw "Invalid cluster name or cluster node name"
}
else 
{
	Try
	{
		$nodeObj = Get-WmiObject -class MSCluster_Node -namespace "root\mscluster" -computername $clusterName -Authentication PacketPrivacy -Credential $credential | Where-Object {$nodeName -match $_.Name}
	
		if ($nodeObj -ne $null)
		{
			if($nodeObj.State -eq 0)
			{
				return "Running"
			}
			else
			{
				return "NotRunning"
			}
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