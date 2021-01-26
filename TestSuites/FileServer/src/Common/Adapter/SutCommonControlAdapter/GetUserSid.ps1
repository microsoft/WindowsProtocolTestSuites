# Copyright (c) Microsoft. All rights reserved.
# Licensed under the MIT license. See LICENSE file in the project root for full license information.

[string]$target
[string]$adminUserName
[string]$userName

$domainName = $PtfProp_Common_DomainName
$sutComputerName = $PtfProp_Common_SutComputerName
$dcName = $PtfProp_Common_DCServerComputerName

$isDomainEnv = (-not [string]::IsNullOrEmpty($domainName)) -and ($domainName -ne $sutComputerName)
$remoteComputerName = if ($isDomainEnv) {
	$dcName
}
else {
	$target
}

$commandForDomain = {
	param(
		[string]$target,
		[string]$userName
	)

	$domainFqn = "DC=" + $target.Replace(".", ",DC=")
	$userFqn = "CN=$userName,CN=Users,$domainFqn"

	$domainUser = Get-ADUser -Filter "*" -SearchBase $userFqn | Select-Object -First 1

	return $domainUser.SID.Value
}

$commandForLocalComputer = {
	param(
		[string]$userName
	)

	$localUser = Get-LocalUser -Name $userName

	return $localUser.SID.Value
}

try {
	[array]$userSid = if ($isDomainEnv) {
		Invoke-Command -HostName $remoteComputerName -UserName "$target\$adminUserName" -ScriptBlock $commandForDomain -ArgumentList @($target, $userName)
	}
	else {
		Invoke-Command -HostName $remoteComputerName -UserName "$adminUserName" -ScriptBlock $commandForLocalComputer -ArgumentList @($userName)
	}
}
catch {
	Get-Error
}

return $userSid