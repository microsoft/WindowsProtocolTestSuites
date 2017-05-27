#############################################################################
## Copyright (c) Microsoft. All rights reserved.
## Licensed under the MIT license. See LICENSE file in the project root for full license information.
#############################################################################

# Create lingering object on DC2

Param(
    [string]$dc1Name,
    [string]$dc2Name,
    [string]$userName,
    [string]$password,
    [string]$newSite,
    [string]$configNC
    )

$pwd = ConvertTo-SecureString $password -AsPlainText -Force
$cred = New-Object -TypeName System.Management.Automation.PSCredential -ArgumentList $userName, $pwd

$session = New-PSSession -ComputerName $dc2Name -Credential $cred

Write-Host "Create site"
New-ADReplicationSite -Name $newSite

Write-Host "Force replication"

repadmin /syncall
Invoke-Command -Session $session -ScriptBlock { 
    Param(
    [string]$newSite,
    [string]$configNC
    )

    repadmin /syncall;

    do {

    Write-Host "checking for object..."

    $obj = Get-ADObject -LDAPfilter "(name=$newSite)" -SearchBase $configNC

    Start-Sleep -Seconds 1

    } while ($obj -eq $null)
} -ArgumentList $newSite, $configNC

Write-Host "Change tombstone lifetime to 2 days"
Set-ADObject "CN=Directory Service,CN=Windows NT,CN=Services,$configNC" -Replace @{'tombstoneLifetime'='2'}

Write-Host "Disable replication traffic"
repadmin /options localhost +disable_inbound_repl +disable_outbound_repl

Write-Host "Delete user object"
Remove-ADObject "CN=$newSite,CN=Sites,$configNC" -Recursive:$true -Confirm:$false

Write-Host "Advance three days in time"
Set-Date -Date (Get-Date).AddDays(3)

Write-Host "Perform garbage collection"
$rootDse = New-Object System.DirectoryServices.DirectoryEntry("LDAP://RootDSE")
$rootDse.Put("doGarbageCollection","1")
$rootDse.SetInfo()  # commit

Write-Host "Enable replication traffic"
repadmin /options localhost -disable_inbound_repl -disable_outbound_repl

Write-Host "Restore system time"
Set-Date -Date (Get-Date).AddDays(-3)

Write-Host "Touch the lingering object on the remote DC"

Invoke-Command -Session $session -ScriptBlock { 
    Param(
    [string]$newSite,
    [string]$defaultNC
    )
    Set-ADObject "CN=$newSite,CN=Sites,$configNC" -Replace @{'location'='new'} 
} -ArgumentList $newSite, $configNC

# close the remote session
# Remove-PSSession -ComputerName $dc2Name