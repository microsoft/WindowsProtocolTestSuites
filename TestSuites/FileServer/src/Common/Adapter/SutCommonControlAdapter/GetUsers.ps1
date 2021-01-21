# Copyright (c) Microsoft. All rights reserved.
# Licensed under the MIT license. See LICENSE file in the project root for full license information.

[string]$target
[string]$adminUserName
[string]$adminPassword

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
        [string]$adminPassword
    )

    $passwordConverted = $adminPassword | ConvertTo-SecureString -AsPlainText -Force
    $cred = New-Object PSCredential -ArgumentList @("$target\$adminUserName", $passwordConverted)

    $domainFqn = "DC=" + $target.Replace(".", ",DC=")
    $usersFqn = "CN=Users,$domainFqn"

    [array]$domainUsers = Get-ADUser -Credential $cred -Filter "*" -SearchBase $usersFqn 
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
        Invoke-Command -HostName $remoteComputerName -UserName "$target\$sessionUserName" -ScriptBlock $commandForDomain -ArgumentList @($target, $adminUserName, $adminPassword)
    }
    else {
        Invoke-Command -HostName $remoteComputerName -UserName "$target\$sessionUserName" -ScriptBlock $commandForLocalComputer
    }
}
catch {
    Get-Error
}

return ($users | ConvertTo-Json)