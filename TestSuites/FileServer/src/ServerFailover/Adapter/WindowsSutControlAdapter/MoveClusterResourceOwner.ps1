# Copyright (c) Microsoft. All rights reserved.
# Licensed under the MIT license. See LICENSE file in the project root for full license information.

[string]$resName
[string]$nodeName
[string]$clusterName = $PtfProp_Cluster_ClusterName
[string]$domain = $PtfProp_Common_DomainName
[string]$userName = $PtfProp_Common_AdminUserName
[string]$password = $PtfProp_Common_PasswordForAllUsers

if($clusterName -notmatch "\.")
{
	$clusterName = "$clusterName.$domain"
}

try
{
    Get-PSSession|Remove-PSSession
    $psSession=New-PSSession -HostName $clusterName -UserName "$domain\$userName"
}
catch
{
    Get-Error
}

$command = {
    Param
    (
    [String]$Name,
    [String]$Node
    )
    Move-ClusterGroup -Name $Name -Node $Node
}

$ret = $FALSE
try
{
    $result = Invoke-Command -Session $psSession -ScriptBlock $command -ArgumentList $resName,$nodeName
    $ret = $TRUE
}
catch
{
	Get-Error
}
finally
{
    Get-PSSession|Remove-PSSession
}
return $ret