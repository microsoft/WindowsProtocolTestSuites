# Copyright (c) Microsoft. All rights reserved.
# Licensed under the MIT license. See LICENSE file in the project root for full license information.

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
        [string]$domainName
    )

    $domainFqn = "DC=" + $domainName.Replace(".", ",DC=")

    [array]$domainGroups = Get-ADGroup -Filter "*" -SearchBase $domainFqn 

    return $domainGroups
}

$commandForLocalComputer = {
    return @(Get-LocalGroup)
}

try {
    [array]$results = if ($isDomainEnv) {
        Invoke-Command -HostName $remoteComputerName -UserName "$domainName\$adminUserName" -ScriptBlock $commandForDomain -ArgumentList @($domainName)
    }
    else {
        Invoke-Command -HostName $remoteComputerName -UserName "$adminUserName" -ScriptBlock $commandForLocalComputer
    }
}
catch {
    Get-Error
}

$groups = @()
foreach ($result in $results) {
    $group = @{
        Name = $result.Name
        Sid  = $result.SID
    }
    $groups += $group
}

if ($groups.Length -eq 0) {
    return "[]"
}
else {
    return ($groups | ConvertTo-Json -AsArray)
}