#############################################################################
## Copyright (c) Microsoft. All rights reserved.
## Licensed under the MIT license. See LICENSE file in the project root for full license information.
#############################################################################

[string]$remoteComputerName
[string]$remoteUsername
[string]$remotePassword
[string]$command = "REG ADD HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\System\Kerberos\Parameters /v SupportedEncryptionTypes /t REG_DWORD /d 0x00000004 /f"

$pwd = ConvertTo-SecureString $remotePassword -AsPlainText -Force
$cred = New-Object -TypeName System.Management.Automation.PSCredential -ArgumentList $remoteUsername, $pwd
$script = [ScriptBlock]::Create($command)
Invoke-Command -ComputerName $remoteComputerName -Credential $cred $script