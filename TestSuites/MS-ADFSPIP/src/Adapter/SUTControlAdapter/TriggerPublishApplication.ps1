########################################################################################################
## Copyright (c) Microsoft. All rights reserved.
## Licensed under the MIT license. See LICENSE file in the project root for full license information.
########################################################################################################

# Remote SUT computer
$SutIpAddress         = $PTFProp_SUT_SutIPAddress
$SutAdminUserName     = $PTFProp_SUT_Username
$SutAdminPassword     = $PTFProp_SUT_Password
# Web application properties
$ProxyAppName         = $PTFProp_WebApp_App1Name
$ExternalUrl          = $PTFProp_WebApp_App1Url
$BackendServerUrl     = $PTFProp_WebApp_App1Url
$AdfsRelyingPartyName = $PTFProp_WebApp_App1Name
$WebAppCert           = $PTFProp_WebApp_WebAppCert
$PfxPwd               = $PTFProp_Common_PfxPassword

# Script to be run remotely on SUT
$RunCommand = [ScriptBlock] {

    Param 
    (
        [string]$ProxyAppName,
        [string]$ExternalUrl,
        [string]$BackendServerUrl,
        [string]$AdfsRelyingPartyName,
        [string]$ExternalCertificate
    )

    # publish new application
    Add-WebApplicationProxyApplication `
        -Name $ProxyAppName `
        -ExternalPreauthentication ADFS `
        -ExternalUrl $ExternalUrl `
        -ExternalCertificateThumbprint $ExternalCertificate `
        -BackendServerUrl $BackendServerUrl `
        -ADFSRelyingPartyName $AdfsRelyingPartyName `
		-ErrorVariable Err

	return $Err
} 

$SutCredential = New-Object System.Management.Automation.PSCredential -ArgumentList $SutAdminUserName,`
                 $(ConvertTo-SecureString -AsPlainText $SutAdminPassword -Force)
$ExternalCertificate  = (New-Object System.Security.Cryptography.X509Certificates.X509Certificate2 `
							-ArgumentList $WebAppCert, $PfxPwd).Thumbprint

$ErrMsg = Invoke-Command -ComputerName $SutIpAddress -Credential $SutCredential -ScriptBlock $RunCommand `
    -ArgumentList $ProxyAppName,$ExternalUrl,$BackendServerUrl,$AdfsRelyingPartyName,$ExternalCertificate | Out-String

if (-not [string]::IsNullOrEmpty($ErrMsg)) {
	return $false
}

return $true
 
