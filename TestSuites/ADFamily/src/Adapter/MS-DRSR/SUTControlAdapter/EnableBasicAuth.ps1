#############################################################################
## Copyright (c) Microsoft. All rights reserved.
## Licensed under the MIT license. See LICENSE file in the project root for full license information.
#############################################################################

$pwd = ConvertTo-SecureString $remotePassword -AsPlainText -Force
$cred = New-Object -TypeName System.Management.Automation.PSCredential -ArgumentList $userName, $pwd

try {
	Invoke-Command -ComputerName $remoteServerAddress -ScriptBlock { winrm set winrm/config/service/auth `@`{Basic=`"`True`"`} } -Credential $cred
	Invoke-Command -ComputerName $remoteServerAddress -ScriptBlock { winrm set winrm/config/service `@`{AllowUnencrypted=`"`True`"`} } -Credential $cred
} catch {
	return $false
}

return $true