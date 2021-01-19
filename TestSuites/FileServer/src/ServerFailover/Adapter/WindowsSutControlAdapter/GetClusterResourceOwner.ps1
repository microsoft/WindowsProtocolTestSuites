# Copyright (c) Microsoft. All rights reserved.
# Licensed under the MIT license. See LICENSE file in the project root for full license information.

[string]$resName
[string]$clusterName = $PtfProp_Cluster_ClusterName
[string]$domain = $PtfProp_Common_DomainName
[string]$userName = $PtfProp_Common_AdminUserName
[string]$password = $PtfProp_Common_PasswordForAllUsers

$mydir=Split-Path $MyInvocation.MyCommand.Path -Parent

$node01 = $PtfProp_Cluster_ClusterNode01
$node02 = $PtfProp_Cluster_ClusterNode02

if($resName -match "\.")
{
	$resName=$resName.Split(".")[0]
}

if($clusterName -notmatch "\.")
{
	$clusterName = "$clusterName.$domain"
}

function Get-OwnerNode {
    param (
		$mydir,
        $nodeName,
		$resName
    )

    try
	{
		$ret = . "$mydir/Get-WMIObject.ps1" -ComputerName $nodeName -Namespace "root\mscluster" -ClassName MSCluster_Resource -Filter "Name = '$resName'"
		if($ret -ne $null -and $ret.State -eq 2)
		{
			return $ret.OwnerNode
		}
	}
	catch
	{
		Get-Error
	}

	return $null
}

if ($resName -eq $null -or $resName -eq "")
{
	throw "Invalid cluster resource name"
}
else
{
	# Try to get OwnerNode from node01 and node02, if we get the OwnerNode the cluster is ready.
	$ownerNode = Get-OwnerNode -mydir $mydir -nodeName $node01 -resName $resName
	if($ownerNode -eq $null)
	{
		$ownerNode = Get-OwnerNode -mydir $mydir -nodeName $node02 -resName $resName
	}
	return $ownerNode
}
