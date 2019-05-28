########################################################################################################
## Copyright (c) Microsoft. All rights reserved.
## Licensed under the MIT license. See LICENSE file in the project root for full license information.
########################################################################################################

$SutIpAddress               = $PTFProp_SUT_SutIPAddress
$SutAdminUserName           = $PTFProp_SUT_Username
$SutAdminPassword           = $PTFProp_SUT_Password
$AdfsName                   = $PTFProp_ADFS_AdfsDns
$AdfsCert                   = $PTFProp_ADFS_AdfsCert
$PfxPwd                     = $PTFProp_Common_PfxPassword

$DomainAdminCred = New-Object System.Management.Automation.PSCredential -ArgumentList $SutAdminUserName,`
                        $(ConvertTo-SecureString -AsPlainText $SutAdminPassword -Force)
$CertThumbprint = (New-Object System.Security.Cryptography.X509Certificates.X509Certificate2 `
						-ArgumentList $AdfsCert, $PfxPwd).Thumbprint

# Script to be run remotely on SUT
$RunCommand = [ScriptBlock] {

    Param ($Cert, $Adfs, $Cred)
       
	Install-WebApplicationProxy -CertificateThumbprint $Cert `
								-FederationServiceName $Adfs `
								-FederationServiceTrustCredential $Cred `
								-ErrorVariable Err | Out-Null

	return $Err
} 


$ErrMsg = Invoke-Command -ComputerName $SutIpAddress -Credential $DomainAdminCred -ScriptBlock $RunCommand `
    -ArgumentList $CertThumbprint, $AdfsName, $DomainAdminCred | Out-String

if (-not [string]::IsNullOrEmpty($ErrMsg)) {
	return $false
}

return $true
