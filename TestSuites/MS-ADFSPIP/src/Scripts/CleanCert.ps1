########################################################################################################
## Copyright (c) Microsoft. All rights reserved.
## Licensed under the MIT license. See LICENSE file in the project root for full license information.
########################################################################################################

param
(
[string]$certSubject = "CN=ADFS ProxyTrust - Proxy")
$store = New-Object System.Security.Cryptography.X509Certificates.X509Store("My","LocalMachine")
$store.Open("ReadWrite")
remove-item "c:\\temp\\proxy.pfx"
Foreach($cert in $store.Certificates)
{
       if($cert.SubjectName.Name -eq $certSubject)
       {
       $path = "cert:\\LocalMachine\My\\" + $cert.Thumbprint 
         Remove-Item -Path $path
       }
}
return 0