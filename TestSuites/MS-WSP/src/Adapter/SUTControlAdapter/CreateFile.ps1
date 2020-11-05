#############################################################################
## Copyright (c) Microsoft. All rights reserved.
## Licensed under the MIT license. See LICENSE file in the project root for full license information.
#############################################################################

[string]$serverName = $PtfProp_ServerComputerName
[string]$domainName = $PtfProp_DomainName
[string]$userName = $PtfProp_UserName
[string]$password = $PtfProp_Password
[string]$fileName
[string]$shareName = $PtfProp_ShareName
[string]$share = "\\$serverName\$shareName"
[string]$content = "newfile"

$filePath = "$share\Data\Test\$fileName"

if ([System.String]::IsNullOrEmpty($domainName)) {
	$account = $userName
}
else {
	$netBiosName = $domainName.Split(".")[0]
	$account = "$netBiosName\$userName"
}

$result = 1
Try {
	CMD /C "net.exe use $share $password /user:$account"
	# LastExitCode 0 - Connect succeed 
	# LastExitCode 2 - Get error "multiple connections to a server or shared resource by the same user", but this error does not block New-Item
	if ($LastExitCode -eq 0 -or $LastExitCode -eq 2) {
		$ret = New-Item $filePath -type File -value $content -force
		if ($ret -ne $null) {
			$result = 0
		}
	}
}
Catch {
	$result = 1
}
Finally {
	CMD /C "net.exe use $share /delete /yes" | out-null
}

Start-Sleep -Seconds 5

return $result
