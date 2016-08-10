#############################################################################
## Copyright (c) Microsoft. All rights reserved.
## Licensed under the MIT license. See LICENSE file in the project root for full license information.
#############################################################################

[string]$nodeName
[string]$domain = ${PtfPropCommon.DomainName}
[string]$userName = ${PtfPropCommon.AdminUserName}
[string]$password = ${PtfPropCommon.PasswordForAllUsers}

$clusterServiceName = "ClusSvc"

$SecurePassword = New-Object System.Security.SecureString
if($Domain -eq $null -or $Domain.trim() -eq "")
{
    $account = $UserName
}
else
{
    $NetBIOSName = $Domain.Split(".")[0]
    $account = "$NetBIOSName\$UserName"
}

for($i=0; $i -lt $Password.Length; $i++)
{
    $SecurePassword.AppendChar($Password[$i])
}
$credential = New-Object system.Management.Automation.PSCredential($account,$SecurePassword)

$service = Get-WmiObject -ComputerName $nodeName -Credential $credential -Class Win32_Service | Where-Object {$_.Name -eq $clusterServiceName}

if($service -eq $null)
{
    return $FALSE
}

try
{
    $ret = $service.StopService()
}
catch
{
    return $FALSE
}
return $TRUE
