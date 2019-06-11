#######################################################################################################
## Copyright (c) Microsoft. All rights reserved.
## Licensed under the MIT license. See LICENSE file in the project root for full license information.
########################################################################################################

# Remote SUT computer
$SutIpAddress         = $PTFProp_SUT_SutIPAddress
$SutAdminUserName     = $PTFProp_SUT_Username
$SutAdminPassword     = $PTFProp_SUT_Password
# Web application properties
$ProxyAppName         = $PTFProp_WebApp_App1Name

# Script to be run remotely on SUT
$RunCommand = [ScriptBlock] {


    Param ([string]$AppName)

    # remove a published applications on the proxy
    Remove-WebApplicationProxyApplication -Name $AppName -ErrorVariable Err

	return $Err
} 

$SutCredential = New-Object System.Management.Automation.PSCredential -ArgumentList $SutAdminUserName,`
                 $(ConvertTo-SecureString -AsPlainText $SutAdminPassword -Force)

$ErrMsg = Invoke-Command -ComputerName $SutIpAddress -Credential $SutCredential -ScriptBlock $RunCommand `
    -ArgumentList $ProxyAppName | Out-String

if (-not [string]::IsNullOrEmpty($ErrMsg)) {
	return $false
}

return $true; 
