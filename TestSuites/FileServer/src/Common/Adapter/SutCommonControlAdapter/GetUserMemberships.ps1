# Copyright (c) Microsoft. All rights reserved.
# Licensed under the MIT license. See LICENSE file in the project root for full license information.

[string]$target
[string]$adminUserName
[string]$groupName

$domainName = $PtfProp_Common_DomainName
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
        [string]$groupName
    )

    $domainFqn = "DC=" + $target.Replace(".", ",DC=")

    $domainGroup = Get-ADGroup -Filter "Name -eq '$groupName'" -SearchBase $domainFqn | Select-Object -First 1
    [array]$domainMembers = Get-ADGroupMember -Identity $domainGroup
    [array]$results = $domainMembers | ForEach-Object {
        @{
            Name            = $_.Name
            ObjectClass     = $_.ObjectClass
            PrincipalSource = "ActiveDirectory"
            Sid             = $_.SID.Value
        }
    }

    return $results
}

$commandForLocalComputer = {
    param(
        [string]$groupName
    )

    $localMembers = @(Get-LocalGroupMember -Name $groupName)
    [array]$results = $localMembers | ForEach-Object {
        @{
            Name            = $_.Name
            ObjectClass     = $_.ObjectClass
            PrincipalSource = $_.PrincipalSource.ToString()
            Sid             = $_.SID.Value
        }
    }

    return $results
}

$members = @()
try {
    $members = if ($isDomainEnv) {
        Invoke-Command -HostName $remoteComputerName -UserName "$target\$sessionUserName" -ScriptBlock $commandForDomain -ArgumentList @($target, $adminUserName, $groupName)
    }
    else {
        Invoke-Command -HostName $remoteComputerName -UserName "$sessionUserName" -ScriptBlock $commandForLocalComputer -ArgumentList @($groupName)
    }
}
catch {
    Get-Error
}

return ($members | ConvertTo-Json)