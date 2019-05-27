#############################################################################
## Copyright (c) Microsoft. All rights reserved.
## Licensed under the MIT license. See LICENSE file in the project root for full license information.
#############################################################################

[string]$clientName
[string]$nodeName
[string]$clusterName = $PtfProp_Cluster_ClusterName
[string]$domain = $PtfProp_Common_DomainName
[string]$userName = $PtfProp_Common_AdminUserName
[string]$password = $PtfProp_Common_PasswordForAllUsers

$SecurePassword = New-Object System.Security.SecureString
if($domain -eq $null -or $domain.trim() -eq "")
{
    $account = $userName
}
else
{
    $NetBIOSName = $Domain.Split(".")[0]
    $account = "$NetBIOSName\$UserName"
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
    Move-SmbWitnessClient -ClientName $Name -DestinationNode $Node -Confirm:$false
}

try
{
    invoke-Command -ComputerName $clusterName -Credential $credential -scriptblock $command -ArgumentList $clientName,$nodeName
}
catch
{
    throw "Failed to call Move-SmbWitnessClient to move SWN client"
}
