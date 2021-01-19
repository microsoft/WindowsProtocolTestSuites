# Copyright (c) Microsoft. All rights reserved.
# Licensed under the MIT license. See LICENSE file in the project root for full license information.

[string]$clientName
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
    Move-SmbWitnessClient -ClientName $Name -DestinationNode $Node -Confirm:$false
}

$ret = $FALSE
try
{
    $result = Invoke-Command -Session $psSession -ScriptBlock $command -ArgumentList $clientName,$nodeName
    $ret = $TRUE
}
catch
{
	Get-Error
    throw "Failed to call Move-SmbWitnessClient to move SWN client"
}
finally
{
    Get-PSSession|Remove-PSSession
}
return $ret