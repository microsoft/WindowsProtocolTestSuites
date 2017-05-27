########################################################################################################
## Copyright (c) Microsoft. All rights reserved.
## Licensed under the MIT license. See LICENSE file in the project root for full license information.
########################################################################################################

# Remote SUT computer
$SutIpAddress         = ${PTFPropSUT.SutIPAddress}
$SutAdminUserName     = ${PTFPropSUT.Username}
$SutAdminPassword     = ${PTFPropSUT.Password}
# Web application properties
$ProxyAppName         = ${PTFPropWebApp.App1Name}
$ExternalUrl          = ${PTFPropWebApp.App1Url}
$BackendServerUrl     = ${PTFPropWebApp.App1Url}
$AdfsRelyingPartyName = ${PTFPropWebApp.App1Name}
$WebAppCert           = ${PTFPropWebApp.WebAppCert}
$PfxPwd               = ${PTFPropCommon.PfxPassword}

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
 
