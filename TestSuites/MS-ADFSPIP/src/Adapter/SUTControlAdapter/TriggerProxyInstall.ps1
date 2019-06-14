########################################################################################################
## Copyright (c) Microsoft. All rights reserved.
## Licensed under the MIT license. See LICENSE file in the project root for full license information.
########################################################################################################

param(
[string]$proxyAddr             = $PTFProp_SUT_SutIPAddress,
[string]$proxyAdmin            = $PTFProp_SUT_Username,
[string]$proxyAdminPwd         = $PTFProp_SUT_Password,
[string]$domainAdmin           = $PTFProp_Domain_Username,
[string]$domainAdminPwd        = $PTFProp_Domain_Password,
[string]$adfsName              = $PTFProp_ADFS_AdfsDns,
[string]$AdfsCert              = $PTFProp_ADFS_AdfsCert,
[string]$PfxPwd                = $PTFProp_Common_PfxPassword
)

# variables used for remoting
$SutIpAddress = $proxyAddr
$SutAdminUserName = $proxyAdmin
$SutAdminPassword = $proxyAdminPwd

$SutCredential = New-Object System.Management.Automation.PSCredential -ArgumentList $SutAdminUserName,`
                        $(ConvertTo-SecureString -AsPlainText $SutAdminPassword -Force)
$CertThumbprint = (New-Object System.Security.Cryptography.X509Certificates.X509Certificate2 `
						-ArgumentList $AdfsCert, $PfxPwd).Thumbprint

$session = New-PSSession -ComputerName $SutIpAddress -Credential $SutCredential

$Output = Invoke-Command -Session $session `
                    -ScriptBlock {Install-WindowsFeature Web-Application-Proxy -IncludeManagementTools}
$Output
$Output = Invoke-Command -Session $session `
                    -ScriptBlock {
					 Param (
        [string]$username,
        [string]$pwd
    )
	$DomainAdminCredential = New-Object System.Management.Automation.PSCredential -ArgumentList $username,`
                        $(ConvertTo-SecureString -AsPlainText $pwd -Force)} -ArgumentList $domainAdmin,$domainAdminPwd
$Output
$Output = Invoke-Command -Session $session `
                    -ScriptBlock {
					Param (
        [string]$cert,
	[string]$adfs
		)
		Install-WebApplicationProxy -CertificateThumbprint $cert -FederationServiceName $adfs -FederationServiceTrustCredential $DomainAdminCredential } -ArgumentList $certThumbprint,$adfsName
$Output
sleep(20)