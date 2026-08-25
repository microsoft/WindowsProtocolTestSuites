# Copyright (c) Microsoft. All rights reserved.
# Licensed under the MIT license. See LICENSE file in the project root for full license information.

[Diagnostics.CodeAnalysis.SuppressMessageAttribute('PSAvoidUsingConvertToSecureStringWithPlainText', '',
    Justification = 'The private deployment Config.json and deterministic test-only machine password are already plaintext inputs and must be converted to SecureString for AD password-reset cmdlets.')]
[CmdletBinding()]
param(
    [Parameter(Mandatory)]
    [string]$ConfigFile,

    [string]$MachinePassword = 'Password04!',

    [ValidateRange(1, 20)]
    [int]$MaximumAttempts = 10
)

$ErrorActionPreference = 'Stop'
$config = Get-Content -LiteralPath $ConfigFile -Raw -ErrorAction Stop |
    ConvertFrom-Json -ErrorAction Stop
$computerSystem = Get-CimInstance Win32_ComputerSystem -ErrorAction Stop
if (-not $computerSystem.PartOfDomain) {
    throw 'Kerberos machine-password alignment requires a domain-joined computer.'
}

$domainName = "$($config.Core.DomainName)"
$domainNetBios = if ($config.Domain -and $config.Domain.NetBiosName) {
    "$($config.Domain.NetBiosName)"
} else {
    $domainName.Split('.')[0].ToUpperInvariant()
}
$dcName = if ($config.Machines.DC -and $config.Machines.DC.ComputerName) {
    "$($config.Machines.DC.ComputerName)"
} else {
    ''
}
$dcServer = if ([string]::IsNullOrWhiteSpace($dcName)) {
    $domainName
} else {
    "$dcName.$domainName"
}
if ([string]::IsNullOrWhiteSpace("$($config.Core.Username)") -or
    [string]::IsNullOrWhiteSpace("$($config.Core.Password)")) {
    throw 'Config.json Core.Username and Core.Password are required for Kerberos machine-password alignment.'
}

if (-not (Get-Module -ListAvailable -Name ActiveDirectory -ErrorAction SilentlyContinue)) {
    Install-WindowsFeature RSAT-AD-PowerShell -ErrorAction Stop | Out-Null
}
Import-Module ActiveDirectory -ErrorAction Stop

$domainPassword = ConvertTo-SecureString "$($config.Core.Password)" -AsPlainText -Force
$domainCredential = [pscredential]::new(
    "$domainNetBios\$($config.Core.Username)",
    $domainPassword
)
$machineSecurePassword = ConvertTo-SecureString $MachinePassword -AsPlainText -Force
$machineAccount = "$($env:COMPUTERNAME)$"

for ($attempt = 1; $attempt -le $MaximumAttempts; $attempt++) {
    try {
        Get-ADDomainController -Identity $dcServer -Credential $domainCredential `
            -ErrorAction Stop | Out-Null
        Set-ADAccountPassword -Identity $machineAccount -Reset `
            -NewPassword $machineSecurePassword -Credential $domainCredential `
            -Server $dcServer -ErrorAction Stop

        $ksetupOutput = @(& ksetup.exe /SetComputerPassword $MachinePassword 2>&1)
        $ksetupOutput | ForEach-Object { Write-Output "$_" }
        if ($LASTEXITCODE -ne 0) {
            throw "ksetup /SetComputerPassword failed with exit code $LASTEXITCODE."
        }

        if (-not (Test-Path 'HKLM:\SYSTEM\CurrentControlSet\Services\Netlogon\Parameters')) {
            New-Item -Path 'HKLM:\SYSTEM\CurrentControlSet\Services\Netlogon\Parameters' `
                -Force | Out-Null
        }
        Set-ItemProperty `
            -Path 'HKLM:\SYSTEM\CurrentControlSet\Services\Netlogon\Parameters' `
            -Name DisablePasswordChange -Value 1 -Type DWord -Force

        # Restore the live secure channel so the domain-account resume task can
        # be registered reliably. A full computer reboot is still mandatory:
        # Microsoft documents that ksetup's computer-password change does not
        # take effect for all consumers until restart.
        Restart-Service Netlogon -Force -ErrorAction Stop
        Start-Sleep -Seconds 5
        if (-not (Test-ComputerSecureChannel -ErrorAction Stop)) {
            throw 'The live secure channel did not recover after restarting Netlogon.'
        }
        return $true
    }
    catch {
        if ($attempt -ge $MaximumAttempts) {
            throw "Kerberos machine-password alignment failed after $MaximumAttempts attempts: $($_.Exception.Message)"
        }
        Write-Warning (
            "Kerberos machine-password alignment attempt $attempt/$MaximumAttempts failed: " +
            "$($_.Exception.Message). Retrying in 30 seconds."
        )
        Start-Sleep -Seconds 30
    }
}

throw 'Kerberos machine-password alignment ended without a success result.'
