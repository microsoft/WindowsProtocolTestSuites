#############################################################################
## Copyright (c) Microsoft. All rights reserved.
## Licensed under the MIT license. See LICENSE file in the project root for full license information.
#############################################################################

[string]$RemotePassword = ${PtfPropLocalRealm.Users.Admin.Password}
[string]$RemoteUserName = ${PtfPropLocalRealm.Users.Admin.Username}
[string]$RemoteComputerName = ${PtfPropLocalRealm.KDC01.FQDN}
[string]$Command = "ksetup /SetEncTypeAttr " + ${PtfPropTrustedRealm.RealmName} + " AES128-CTS-HMAC-SHA1-96 AES256-CTS-HMAC-SHA1-96"

$PWD = ConvertTo-SecureString $RemotePassword -AsPlainText -Force
$Cred = New-Object -TypeName System.Management.Automation.PSCredential -ArgumentList $RemoteUserName, $PWD
$Script = [ScriptBlock]::Create($Command)
Invoke-Command -ComputerName $RemoteComputerName -Credential $Cred $Script
