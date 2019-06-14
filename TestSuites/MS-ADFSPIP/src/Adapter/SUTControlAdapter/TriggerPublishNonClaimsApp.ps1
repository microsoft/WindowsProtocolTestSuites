########################################################################################################
## Copyright (c) Microsoft. All rights reserved.
## Licensed under the MIT license. See LICENSE file in the project root for full license information.
########################################################################################################

# Remote SUT computer
$SutIpAddress         = $PTFProp_SUT_SutIPAddress
$SutAdminUserName     = $PTFProp_SUT_Username
$SutAdminPassword     = $PTFProp_SUT_Password
# Web application properties
$ProxyHostName        = $PTFProp_WebApp_ProxyHostName
$ProxyAppName         = $PTFProp_WebApp_App2Name
$ExternalUrl          = $PTFProp_WebApp_App2Url
$BackendServerUrl     = $PTFProp_WebApp_App2Url
$AdfsRelyingPartyName = $PTFProp_WebApp_App2Name
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

    # publish new application for Rich Client
    Add-WebApplicationProxyApplication `
		-BackendServerUrl $BackendServerUrl `
		-ExternalCertificateThumbprint $ExternalCertificate `
		-EnableHTTPRedirect:$true `
		-ExternalUrl $ExternalUrl `
        -Name $ProxyAppName `
        -ExternalPreAuthentication ADFSforRichClients `
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


# restart service Restart-Service appproxysvc
$RunCommand = [ScriptBlock] {
	Restart-Service appproxysvc
} 

Invoke-Command -ComputerName $SutIpAddress -Credential $SutCredential -ScriptBlock $RunCommand | Out-String

# Append new entry to set client request redirect to Proxy

if( (Get-Content $env:windir\System32\drivers\etc\hosts |?{$_ -imatch "\s$ProxyHostName"}) -eq $null)
{
"
$SutIpAddress $ProxyHostName" | Out-File -FilePath "$env:windir\System32\drivers\etc\hosts" -Append -encoding ascii
}

return $true
