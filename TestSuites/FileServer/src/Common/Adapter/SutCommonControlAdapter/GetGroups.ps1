# Copyright (c) Microsoft. All rights reserved.
# Licensed under the MIT license. See LICENSE file in the project root for full license information.

[string]$target
[string]$adminUserName

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
        [string]$target
    )

    $domainFqn = "DC=" + $target.Replace(".", ",DC=")

    [array]$domainGroups = Get-ADGroup -Filter "*" -SearchBase $domainFqn 

    return $domainGroups
}

$commandForLocalComputer = {
    return @(Get-LocalGroup)
}

try {
    [array]$results = if ($isDomainEnv) {
        Invoke-Command -HostName $remoteComputerName -UserName "$target\$adminUserName" -ScriptBlock $commandForDomain -ArgumentList @($target)
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
        Sid  = $result.SID.Value
    }
    $groups += $group
}

if ($groups.Length -eq 0) {
    return "[]"
}
else {
    return ($groups | ConvertTo-Json)
}