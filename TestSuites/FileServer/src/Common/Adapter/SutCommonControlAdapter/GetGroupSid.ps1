# Copyright (c) Microsoft. All rights reserved.
# Licensed under the MIT license. See LICENSE file in the project root for full license information.

[string]$groupName

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
		[string]$groupName
	)

	$domainFqn = "DC=" + $domainName.Replace(".", ",DC=")

	$domainGroup = Get-ADGroup -Filter "Name -eq '$groupName'" -SearchBase $domainFqn | Select-Object -First 1

	return $domainGroup.SID.Value ?? $domainGroup.SID
}

$commandForLocalComputer = {
	param(
		[string]$groupName
	)

	$localGroup = Get-LocalGroup -Name $groupName

	return $localGroup.SID.Value ?? $localGroup.SID
}

try {
	$groupSid = if ($isDomainEnv) {
		Invoke-Command -HostName $remoteComputerName -UserName "$domainName\$adminUserName" -ScriptBlock $commandForDomain -ArgumentList @($domainName, $groupName)
	}
	else {
		Invoke-Command -HostName $remoteComputerName -UserName "$adminUserName" -ScriptBlock $commandForLocalComputer -ArgumentList @($groupName)
	}
}
catch {
	Get-Error
}

return $groupSid