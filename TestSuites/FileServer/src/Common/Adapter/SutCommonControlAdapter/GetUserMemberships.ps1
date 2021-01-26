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

    [array]$domainMemberships = Get-ADGroup -LDAPFilter "(member=$userFqn)" -SearchBase $domainFqn

    return $domainMemberships
}

$commandForLocalComputer = {
    param(
        [string]$userName
    )

    $localUser = Get-LocalUser -Name $userName
    $localUserSid = $localUser.SID.Value

    $localGroups = @(Get-LocalGroup)
    $localMemberships = @()
    foreach ($localGroup in $localGroups) {
        $localGroupMembers = @(Get-LocalGroupMember -Name $localGroup.Name)
        foreach ($localGroupMember in $localGroupMembers) {
            if ($localGroupMember.SID.Value -eq $localUserSid) {
                $localMemberships += $localGroup
                break
            }
        }
    }

    return $localMemberships
}

try {
    [array]$results = if ($isDomainEnv) {
        Invoke-Command -HostName $remoteComputerName -UserName "$target\$adminUserName" -ScriptBlock $commandForDomain -ArgumentList @($target, $userName)
    }
    else {
        Invoke-Command -HostName $remoteComputerName -UserName "$adminUserName" -ScriptBlock $commandForLocalComputer -ArgumentList @($userName)
    }
}
catch {
    Get-Error
}

$memberships = @()
foreach ($result in $results) {
    $membership = @{
        Name = $result.Name
        Sid  = $result.SID.Value
    }
    $memberships += $membership
}

if ($memberships.Length -eq 0) {
    return "[]"
}
else {
    return ($memberships | ConvertTo-Json)
}