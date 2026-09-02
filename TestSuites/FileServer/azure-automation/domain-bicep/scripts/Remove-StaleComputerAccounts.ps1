# Copyright (c) Microsoft. All rights reserved.
# Licensed under the MIT license. See LICENSE file in the project root for full license information.

# Runs ON THE DC before a resumed Phase 2 deployment. The caller passes only the
# computer names whose Azure VMs are absent; objects for surviving members must
# remain untouched so their secure channels continue to work.

param(
    [Parameter(Mandatory)]
    [ValidateNotNullOrEmpty()]
    [string]$ComputerNamesCsv
)

Import-Module ActiveDirectory -ErrorAction Stop

$staleNames = @($ComputerNamesCsv.Split(',') |
    ForEach-Object { $_.Trim() } |
    Where-Object { -not [string]::IsNullOrWhiteSpace($_) } |
    Select-Object -Unique)

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
