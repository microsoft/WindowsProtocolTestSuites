# Copyright (c) Microsoft. All rights reserved.
# Licensed under the MIT license. See LICENSE file in the project root for full license information.

$dcName = Invoke-Command -ScriptBlock { CMD /C "dsquery server -o rdn" }
$computerName = $env:COMPUTERNAME

if ($null -ne $dcName -and $dcName -eq $computerName) {
    dsquery user -samid * | dsmod user -pwdneverexpires yes -mustchpwd no
    dsquery user -samid * | dsget user -samid -pwdneverexpires
}
else {

    $accounts = Get-CimInstance -ClassName Win32_UserAccount -Filter "Domain='$env:ComputerName'"
    foreach ($account in $accounts) {
        $properties = @{PasswordExpires = $false }
        Set-CimInstance -InputObject $account -Property $properties
    }
    Get-WmiObject -Class Win32_UserAccount | Where-Object { $_.Domain -eq $env:ComputerName } | Format-Table Caption, PasswordExpires                                                                                         
}
