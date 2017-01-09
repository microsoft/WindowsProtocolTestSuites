#############################################################################
## Copyright (c) Microsoft. All rights reserved.
## Licensed under the MIT license. See LICENSE file in the project root for full license information.
#############################################################################


$pwd = ConvertTo-SecureString $remotePassword -AsPlainText -Force

# When creating the user credential, need to note that when "Basic" auth is set, need to
# provide a username in a form of "username@domain.com"
$cred = New-Object -TypeName System.Management.Automation.PSCredential -ArgumentList ($userName + "@" + $domainFQDN), $pwd

try {
	If ($isEnabling -eq $false) {
		Invoke-Command -ComputerName $remoteServerAddress -Credential $cred -ScriptBlock {
			repadmin /options localhost +disable_inbound_repl +disable_outbound_repl
		}
	} else {
		Invoke-Command -ComputerName $remoteServerAddress -Credential $cred -ScriptBlock {
			repadmin /options localhost -disable_inbound_repl -disable_outbound_repl
		}
	}
} catch {
	return $false
}

return $true