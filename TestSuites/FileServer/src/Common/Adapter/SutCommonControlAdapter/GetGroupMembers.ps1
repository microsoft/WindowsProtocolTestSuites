# Copyright (c) Microsoft. All rights reserved.
# Licensed under the MIT license. See LICENSE file in the project root for full license information.

[string]$target
[string]$adminUserName
[string]$adminPassword
[string]$groupName

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
        [string]$groupName
    )

    $passwordConverted = $adminPassword | ConvertTo-SecureString -AsPlainText -Force
    $cred = New-Object PSCredential -ArgumentList @("$target\$adminUserName", $passwordConverted)

    $domainFqn = "DC=" + $target.Replace(".", ",DC=")

    $domainGroup = Get-ADGroup -Filter "Name -eq '$groupName'" -SearchBase $domainFqn | Select-Object -First 1
    [array]$domainMembers = Get-ADGroupMember -Credential $cred -Identity $domainGroup
    [array]$results = $domainMembers | ForEach-Object {
        @{
            Name            = $_.Name
            ObjectClass     = $_.ObjectClass
            PrincipalSource = $_.PrincipalSource
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
            PrincipalSource = $_.PrincipalSource
            Sid             = $_.SID.Value
        }
    }

    return $results
}

$users = @()
try {
    $users = if ($isDomainEnv) {
        Invoke-Command -HostName $remoteComputerName -UserName "$target\$sessionUserName" -ScriptBlock $commandForDomain -ArgumentList @($target, $adminUserName, $adminPassword, $groupName)
    }
    else {
        Invoke-Command -HostName $remoteComputerName -UserName "$target\$sessionUserName" -ScriptBlock $commandForLocalComputer -ArgumentList @($groupName)
    }
}
catch {
    Get-Error
}

return ($users | ConvertTo-Json)