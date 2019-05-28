########################################################################################################
## Copyright (c) Microsoft. All rights reserved.
## Licensed under the MIT license. See LICENSE file in the project root for full license information.
########################################################################################################

$StsIpAddress     = $PTFProp_Common_RealADFSIP
$StsAdminUserName = $PTFProp_Domain_Username
$StsAdminPassword = $PTFProp_Domain_Password

$stsCredential = New-Object System.Management.Automation.PSCredential -ArgumentList $StsAdminUserName,`
                        $(ConvertTo-SecureString -AsPlainText $StsAdminPassword -Force)
$Output = Invoke-Command -ComputerName $StsIpAddress -Credential $stsCredential `
                    -ScriptBlock {revoke-adfsproxytrust -Confirm:$False}
$Output

$Output = Invoke-Command -ComputerName $StsIpAddress -Credential $stsCredential `
                    -ScriptBlock {revoke-adfsproxytrust -Confirm:$False}
$Output

# variables used for remoting
$SutIpAddress         = $PTFProp_SUT_SutIPAddress
$SutAdminUserName     = $PTFProp_SUT_Username
$SutAdminPassword     = $PTFProp_SUT_Password

$SutCredential = New-Object System.Management.Automation.PSCredential -ArgumentList $SutAdminUserName,`
                        $(ConvertTo-SecureString -AsPlainText $SutAdminPassword -Force)
$ScriptBlock = 
$Output = Invoke-Command -ComputerName $SutIpAddress -Credential $SutCredential `
                    -ScriptBlock {Remove-WindowsFeature Web-Application-Proxy}
$Output
$Output = Invoke-Command -ComputerName $SutIpAddress -Credential $SutCredential `
                    -ScriptBlock {shutdown -r -t 0}
$Output