#############################################################################
## Copyright (c) Microsoft. All rights reserved.
## Licensed under the MIT license. See LICENSE file in the project root for full license information.
#############################################################################

[string]$fileName
[string]$domain = $PtfProp_DomainName
[string]$userName = $PtfProp_SutUserName
[string]$password = $PtfProp_SutPassword

$share = "\\" + $PtfProp_SutComputerName + "\" + $PtfProp_ShareFolder

if($Domain -eq $null -or $Domain.trim() -eq "")
{
    $account = $UserName
}
$account = "$Domain\$UserName"

$SecurePassword = New-Object System.Security.SecureString
for($i=0; $i -lt $Password.Length; $i++)
{
    $SecurePassword.AppendChar($Password[$i])
}
$credential = New-Object system.Management.Automation.PSCredential($account,$SecurePassword)

try
{
    net use /del * /y
    New-PSDrive -psProvider FileSystem -name TestDrive -root $share -credential $credential
    Remove-Item TestDrive:\$fileName -force
    Remove-PSDrive -name TestDrive
}
catch
{
    net use /del * /y
}
