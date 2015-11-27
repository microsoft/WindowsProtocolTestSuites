#############################################################################
## Copyright (c) Microsoft. All rights reserved.
## Licensed under the MIT license. See LICENSE file in the project root for full license information.
#############################################################################

[string]$IPAddress
[string]$nodeName
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
    [String]$ip
    )
    $IPConfig = Get-NetIPAddress -IPAddress $ip
    $adapter = Get-NetAdapter -InterfaceIndex $IPConfig.InterfaceIndex
    Disable-NetAdapter -Name $adapter.Name -Confirm:$false
	Start-Sleep -s 10
	Enable-NetAdapter -Name $adapter.Name -Confirm:$false
}

try
{
    invoke-Command -ComputerName $nodeName -Credential $credential -scriptblock $command -ArgumentList $IPAddress
}
catch
{
    throw "Failed to refresh NetAdapter."
}
