########################################################################################################
## Copyright (c) Microsoft. All rights reserved.
## Licensed under the MIT license. See LICENSE file in the project root for full license information.
########################################################################################################

# Remote SUT computer
$SutIpAddress         = ${PTFPropSUT.SutIPAddress}
$SutAdminUserName     = ${PTFPropSUT.Username}
$SutAdminPassword     = ${PTFPropSUT.Password}

# Script to be run remotely on SUT
$RunCommand = [ScriptBlock] {

    # get the proxy health state
    Get-WebApplicationProxyHealth

} 

$SutCredential = New-Object System.Management.Automation.PSCredential -ArgumentList $SutAdminUserName,`
                 $(ConvertTo-SecureString -AsPlainText $SutAdminPassword -Force)

$ProxyState = Invoke-Command -ComputerName $SutIpAddress -Credential $SutCredential -ScriptBlock $RunCommand 

$IsApplicationProxyConfigured = $true
foreach ($Component in $ProxyState) {
   if ( $Component.HealthState.ToString().ToLower() -ne "ok" ){
    $IsApplicationProxyConfigured = $false
   }
}

return $IsApplicationProxyConfigured