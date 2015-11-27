#############################################################################
## Copyright (c) Microsoft. All rights reserved.
## Licensed under the MIT license. See LICENSE file in the project root for full license information.
#############################################################################

[string]$resName
[string]$nodeName
[string]$clusterName = ${PtfPropCluster.ClusterName}
[string]$domain = ${PtfPropCommon.DomainName}
[string]$userName = ${PtfPropCommon.AdminUserName}
[string]$password = ${PtfPropCommon.PasswordForAllUsers}

$SecurePassword = New-Object System.Security.SecureString
if($domain -eq $null -or $domain.trim() -eq "")
{
    $account = $userName
}
else
{
    $account = "$domain\$userName"
}

for($i=0; $i -lt $password.Length; $i++)
{
    $SecurePassword.AppendChar($password[$i])
}
$credential = New-Object system.Management.Automation.PSCredential($account,$SecurePassword)

$command = {
    Param
    (
    [String]$Name,
    [String]$Node
    )
    Move-ClusterGroup -Name $Name -Node $Node
}

try
{
    invoke-Command -ComputerName $clusterName -Credential $credential -scriptblock $command -ArgumentList $resName,$nodeName
	return $TRUE
}
catch
{
	return $FALSE
}