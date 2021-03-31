# Copyright (c) Microsoft. All rights reserved.
# Licensed under the MIT license. See LICENSE file in the project root for full license information.

[string]$userName

$domainName = $PtfProp_Common_DomainName
$sutComputerName = $PtfProp_Common_SutComputerName
$dcName = $PtfProp_Common_DCServerComputerName

$adminUserName = $PtfProp_Common_AdminUserName

$isDomainEnv = (-not [string]::IsNullOrEmpty($domainName)) -and ($domainName -ne $sutComputerName)
$remoteComputerName = if ($isDomainEnv) {
    $dcName
}
else {
    $sutComputerName
}

$commandForDomain = {
	param(
		[string]$domainName,
		[string]$userName
	)

	$domainFqn = "DC=" + $domainName.Replace(".", ",DC=")
	$userFqn = "CN=$userName,CN=Users,$domainFqn"

	$domainUser = Get-ADUser -Filter "*" -SearchBase $userFqn | Select-Object -First 1

	return $domainUser.SID.Value ?? $domainUser.SID
}

$commandForLocalComputer = {
	param(
		[string]$userName
	)

	$localUser = Get-LocalUser -Name $userName

	return $localUser.SID.Value ?? $localUser.SID
}

try {
	$userSid = if ($isDomainEnv) {
		Invoke-Command -HostName $remoteComputerName -UserName "$domainName\$adminUserName" -ScriptBlock $commandForDomain -ArgumentList @($domainName, $userName)
	}
	else {
		Invoke-Command -HostName $remoteComputerName -UserName "$adminUserName" -ScriptBlock $commandForLocalComputer -ArgumentList @($userName)
	}
}
catch {
	Get-Error
}

return $userSid