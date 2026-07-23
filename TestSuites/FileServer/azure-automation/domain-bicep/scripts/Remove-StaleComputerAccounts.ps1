# Copyright (c) Microsoft. All rights reserved.
# Licensed under the MIT license. See LICENSE file in the project root for full license information.

# Runs ON THE DC (via Invoke-AzVMRunCommand -ScriptPath) before a resumed Phase 2
# deployment. When -SkipPhase1 is used, the DC may hold computer objects from a
# previous SUT/Driver deployment, and Add-Computer on the new VMs can fail with
# "The account already exists" unless they are removed first. Machine names are
# discovered from the Config.json left by the previous deployment -- nothing is
# hardcoded.

Import-Module ActiveDirectory -ErrorAction Stop

$configPaths = @(
    'C:\Domain-Package\Config.json',
    'C:\Domain-Package\DSC\Scripts\Config.json'
)
$config = $null
foreach ($p in $configPaths) {
    if (Test-Path $p) {
        $config = Get-Content $p -Raw | ConvertFrom-Json
        break
    }
}

if (-not $config) {
    Write-Output "WARNING: Config.json not found on DC - cannot discover machine names."
    Write-Output "Skipping stale account cleanup."
    return
}

# Machine computer names (Node01, Client01, etc.), excluding the DC itself
$staleNames = @()
foreach ($prop in $config.Machines.PSObject.Properties) {
    if ($prop.Value.ComputerName) { $staleNames += $prop.Value.ComputerName }
}
$dcName = ($config.Machines.PSObject.Properties | Where-Object { $_.Name -match 'DC' }).Value.ComputerName
$staleNames = $staleNames | Where-Object { $_ -ne $dcName } | Select-Object -Unique

Write-Output "Cleaning up stale accounts for: $($staleNames -join ', ')"
$cleaned = @()
foreach ($name in $staleNames) {
    $acct = Get-ADComputer -Filter "Name -eq '$name'" -ErrorAction SilentlyContinue
    if ($acct) {
        $acct | Remove-ADObject -Recursive -Confirm:$false
        $cleaned += $name
    }
}
if ($cleaned.Count -gt 0) {
    Write-Output "Removed stale computer accounts: $($cleaned -join ', ')"
} else {
    Write-Output "No stale computer accounts found."
}
