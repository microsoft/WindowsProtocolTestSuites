########################################################################################################
## Copyright (c) Microsoft. All rights reserved.
## Licensed under the MIT license. See LICENSE file in the project root for full license information.
########################################################################################################

param(
[string]$proxyAddr             = ${PTFPropSUT.SutIPAddress},
[string]$proxyAdmin            = ${PTFPropSUT.Username},
[string]$proxyAdminPwd         = ${PTFPropSUT.Password},
[string]$domainAdmin           = ${PTFPropDomain.Username},
[string]$domainAdminPwd        = ${PTFPropDomain.Password},
[string]$adfsName              = ${PTFPropADFS.AdfsDns},
[string]$AdfsCert              = ${PTFPropADFS.AdfsCert},
[string]$PfxPwd                = ${PTFPropCommon.PfxPassword}
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