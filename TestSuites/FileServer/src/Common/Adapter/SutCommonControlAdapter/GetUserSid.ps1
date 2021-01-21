# Copyright (c) Microsoft. All rights reserved.
# Licensed under the MIT license. See LICENSE file in the project root for full license information.

[string]$target
[string]$adminUserName
[string]$adminPassword
[string]$userName

$domainName = $PtfProp_Common_Domain
$sutComputerName = $PtfProp_Common_SutComputerName
$dcName = $PtfProp_Common_DCServerComputerName

$sessionUserName = $PtfProp_Common_AdminUserName

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
		[string]$adminUserName,
		[string]$adminPassword,
		[string]$userName
	)

	$passwordConverted = $adminPassword | ConvertTo-SecureString -AsPlainText -Force
	$cred = New-Object PSCredential -ArgumentList @("$target\$adminUserName", $passwordConverted)

	$domainFqn = "DC=" + $target.Replace(".", ",DC=")
	$userFqn = "CN=$userName,CN=Users,$domainFqn"

	$domainUser = Get-ADUser -Credential $cred -Filter "*" -SearchBase $userFqn | Select-Object -First 1

	return $domainUser.SID.Value
}

$commandForLocalComputer = {
	param(
		[string]$userName
	)

	$localUser = Get-LocalUser -Name $userName

	return $localUser.SID.Value
}

$userSid = $null
try {
	$userSid = if ($isDomainEnv) {
		Invoke-Command -HostName $remoteComputerName -UserName "$target\$sessionUserName" -ScriptBlock $commandForDomain -ArgumentList @($target, $adminUserName, $adminPassword, $userName)
	}
	else {
		Invoke-Command -HostName $remoteComputerName -UserName "$target\$sessionUserName" -ScriptBlock $commandForLocalComputer -ArgumentList @($userName)
	}
}
catch {
	Get-Error
}

return $userSid