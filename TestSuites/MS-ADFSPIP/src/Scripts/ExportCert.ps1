########################################################################################################
## Copyright (c) Microsoft. All rights reserved.
## Licensed under the MIT license. See LICENSE file in the project root for full license information.
########################################################################################################

param
(
[string]$certSubject = "CN=ADFS ProxyTrust - Proxy",
[string]$pwd = "123",
[string]$location = "c:\temp\proxy.pfx"
)$store = New-Object System.Security.Cryptography.X509Certificates.X509Store("My","LocalMachine")
$store.Open("ReadOnly")
remove-item $location
Foreach($cert in $store.Certificates)
{
       if($cert.SubjectName.Name -eq $certSubject)
       {
         $certPwd = ConvertTo-SecureString -String $pwd -Force –AsPlainText
         Export-PfxCertificate –Cert $cert -FilePath $location -Password $certPwd
       }
}
return 0