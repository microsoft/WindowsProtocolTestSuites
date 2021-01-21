# Copyright (c) Microsoft. All rights reserved.
# Licensed under the MIT license. See LICENSE file in the project root for full license information.

[string]$target
[string]$adminUserName
[string]$adminPassword 

$domainName = $PtfProp_Common_Domain
$sutComputerName = $PtfProp_Common_SutComputerName
$dcName = $PtfProp_Common_DCServerComputerName

$sessionUserName = $PtfProp_Common_AdminUserName

$isDomainEnv = $domainName -ne $sutComputerName
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

    $target

    [array]$domainUsers = Get-ADUser -Filter "*" -SearchBase 
    

}

$commandForLocalComputer = {
    param (
        [string]$target,
        [string]$adminUserName,
        [string]$adminPassword
    )

    $passwordConverted = $adminPassword | ConvertTo-SecureString -AsPlainText -Force
    $cred = New-Object PSCredential -ArgumentList @($adminUserName, $passwordConverted)

    $localUsers = Invoke-Command -ComputerName "." -Credential $cred -ScriptBlock { return @(Get-LocalUser) }
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
    $commmand = if ($isDomainEnv) {
        $commandForDomain
    }
    else {
        $commandForLocalComputer
    }

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