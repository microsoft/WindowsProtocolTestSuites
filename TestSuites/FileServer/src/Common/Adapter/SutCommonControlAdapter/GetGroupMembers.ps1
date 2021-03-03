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
    [array]$domainMembers = Get-ADGroupMember -Identity $domainGroup

    return $domainMembers
}

$commandForLocalComputer = {
    param(
        [string]$groupName
    )

    return @(Get-LocalGroupMember -Name $groupName)    
}

try {
    [array]$results = if ($isDomainEnv) {
        Invoke-Command -HostName $remoteComputerName -UserName "$domainName\$adminUserName" -ScriptBlock $commandForDomain -ArgumentList @($domainName, $groupName)
    }
    else {
        Invoke-Command -HostName $remoteComputerName -UserName "$adminUserName" -ScriptBlock $commandForLocalComputer -ArgumentList @($groupName)
    }
}
catch {
    Get-Error
}

$members = @()
foreach ($result in $results) {
    $member = @{
        Name            = $result.Name
        ObjectClass     = $result.ObjectClass
        PrincipalSource = if ($isDomainEnv) { "ActiveDirectory" } else { $result.PrincipalSource }
        Sid             = $result.SID
    }
    $members += $member
}

if ($member.Length -eq 0) {
    return "[]"
}
else {
    return ($members | ConvertTo-Json -AsArray)
}