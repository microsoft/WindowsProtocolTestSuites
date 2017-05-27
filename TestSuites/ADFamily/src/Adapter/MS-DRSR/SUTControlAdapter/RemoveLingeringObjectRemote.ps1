#############################################################################
## Copyright (c) Microsoft. All rights reserved.
## Licensed under the MIT license. See LICENSE file in the project root for full license information.
#############################################################################

#remove lingering object on DC2

Param(
    [string]$dc1Name,
    [string]$dc2Name,
    [string]$configNC
    )

$dc1NetBIOSName = $dc1Name.Split(".")[0]

Write-Host "Cleaning lingering object on NC: $configNC"
$dc1 = Get-ADObject -LDAPFilter "(name=$dc1NetBIOSName)" -SearchBase "$configNC"

Write-Host "Looking for DN of DSA :$dc1.distinguishedName"
$dc1guid = Get-ADObject -LDAPFilter "(objectclass=ntdsdsa)" -SearchBase $dc1.distinguishedName

Write-Host "Found DSA GUID: ", $dc1guid.objectguid

# repadmin /removelingeringobjects $dc2Name $dc1guid.objectguid $configNC /ADVISORY_MODE
repadmin /removelingeringobjects $dc2Name $dc1guid.objectguid $configNC