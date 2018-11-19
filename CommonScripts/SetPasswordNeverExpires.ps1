##################################################################################
## Copyright (c) Microsoft Corporation. All rights reserved.
## Licensed under the MIT license. See LICENSE file in the project root for full license information.
##################################################################################
$dcName = Invoke-Command -ScriptBlock {CMD /C "dsquery server -o rdn"}
$computerName = $env:COMPUTERNAME

if($dcName -eq $computerName)
{
    dsquery user -samid * | dsmod user -pwdneverexpires yes -mustchpwd no
    dsquery user -samid *| dsget user -samid -pwdneverexpires

}
else
{
    Get-WmiObject -Class Win32_UserAccount | where {$_.Domain -eq $env:ComputerName}  | foreach {$_.PasswordExpires = $false;$_.Put()}
    Get-WmiObject -Class Win32_UserAccount | where {$_.Domain -eq $env:ComputerName} | ft Caption,PasswordExpires                                                                                         
}




