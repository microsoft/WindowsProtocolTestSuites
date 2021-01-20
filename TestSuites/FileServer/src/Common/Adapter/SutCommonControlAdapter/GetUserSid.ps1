# Copyright (c) Microsoft. All rights reserved.
# Licensed under the MIT license. See LICENSE file in the project root for full license information.

[string]$nodeName
[string]$clusterName = $PtfProp_Cluster_ClusterName
[string]$domain = $PtfProp_Common_DomainName
[string]$userName = $PtfProp_Common_AdminUserName
[string]$password = $PtfProp_Common_PasswordForAllUsers

$mydir=Split-Path $MyInvocation.MyCommand.Path -Parent

if($nodeName -match "\.")
{
	$nodeName=$nodeName.Split(".")[0]
}

if($clusterName -notmatch "\.")
{
	$clusterName = "$clusterName.$domain"
}

if($clusterName -eq "" -or $nodeName -eq "")
{
    throw "Invalid cluster name or cluster node name"
}
else 
{
	try
	{
		$nodeObj = . "$mydir/Get-WMIObject.ps1" -ComputerName $clusterName -Namespace "root\mscluster" -ClassName MSCluster_Node -Filter "Name = '$nodeName'"

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
	catch
	{
		Get-Error
		return $null
	}
}