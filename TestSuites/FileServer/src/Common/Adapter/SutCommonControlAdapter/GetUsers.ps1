# Copyright (c) Microsoft. All rights reserved.
# Licensed under the MIT license. See LICENSE file in the project root for full license information.

[string]$target
[string]$adminUserName

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
        [string]$adminUserName
    )

    $domainFqn = "DC=" + $target.Replace(".", ",DC=")
    $usersFqn = "CN=Users,$domainFqn"

    [array]$domainUsers = Get-ADUser -Filter "*" -SearchBase $usersFqn 
    [array]$results = $domainUsers | ForEach-Object {
        @{
            Name = $_.Name
            Sid  = $_.SID.Value
        }
    }

    return $results
}

$commandForLocalComputer = {
    $localUsers = @(Get-LocalGroup)
    [array]$results = $localUsers | ForEach-Object {
        @{
            Name = $_.Name
            Sid  = $_.SID.Value
        }
    }

    return $results
}

$users = @()
try {
    $users = if ($isDomainEnv) {
        Invoke-Command -HostName $remoteComputerName -UserName "$target\$sessionUserName" -ScriptBlock $commandForDomain -ArgumentList @($target, $adminUserName)
    }
    else {
        Invoke-Command -HostName $remoteComputerName -UserName "$sessionUserName" -ScriptBlock $commandForLocalComputer
    }
}
catch {
    Get-Error
}

return ($users | ConvertTo-Json)