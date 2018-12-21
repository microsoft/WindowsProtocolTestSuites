#############################################################################
## Copyright (c) Microsoft Corporation. All rights reserved.
## Licensed under the MIT license. See LICENSE file in the project root for full license information.
#############################################################################

#-------------------------------------------------------------------------------------
# This function is used to import the cert with password into the appointed cert store
#-------------------------------------------------------------------------------------
param(
[String]$certhash="5873ce11f1c4ed311a61d0510f998a90bf7331c9",
[String]$appid="{d673f5ee-a714-454d-8de2-492e4c1bd8f8}",
[String]$certpath="c:\temp\Data\Certificate.pfx",
[String]$rootCertStore    = "LocalMachine",
[String]$certStore = "My",
[String]$certPsd   = "1"
)

#----------------------------------------------------------------------------
# Global variables
#----------------------------------------------------------------------------
$scriptPath = Split-Path $MyInvocation.MyCommand.Definition -parent
$env:Path += ";$scriptPath;$scriptPath\Scripts"

#----------------------------------------------------------------------------
# Start loging using start-transcript cmdlet
#----------------------------------------------------------------------------
[string]$logFile = $MyInvocation.MyCommand.Path + ".log"
try { Stop-Transcript -ErrorAction SilentlyContinue } catch {} # Ignore Stop-Transcript error messages
Start-Transcript -Path "$logFile" -Append -Force

#----------------------------------------------------------------------------
# Define common functions
#----------------------------------------------------------------------------
function ImportPfxCert([string]$certPath,[string]$certRootStore,[string]$certStore,[string]$pfxPass)
{
    $pfx=new-object System.Security.Cryptography.X509Certificates.X509Certificate2
	$pfx.import($certPath,$pfxPass,"Exportable,PersistKeySet")
	$store=new-object System.Security.Cryptography.X509Certificates.X509Store($certStore,$certRootStore)
	$store.open("MaxAllowed")
	$store.add($pfx)
	$store.close()
}

#----------------------------------------------------------------------------
# Add cert to Personal store
#----------------------------------------------------------------------------
Write-Info.ps1 "Add certificate to Personal Store." -foregroundcolor Green
ImportPfxCert $certpath $rootCertStore $certStore $certPsd

#----------------------------------------------------------------------------
# Bind the certificate to SSL
#----------------------------------------------------------------------------
Write-Info.ps1 "Remove certificate which already bind to SSL." -foregroundcolor Green
NETSH.exe HTTP DELETE SSLCERT IPPORT=0.0.0.0:443 2>&1 | Write-Info.ps1

Write-Info.ps1 "Start to bind the certificate to SSL." -foregroundcolor Green
NETSH.exe HTTP ADD SSLCERT IPPORT=0.0.0.0:443 certhash=$certhash appid=$appid 2>&1 | Write-Info.ps1


#----------------------------------------------------------------------------
# Ending
#----------------------------------------------------------------------------
Write-Info.ps1 "Completed Build SSL Cert."
Stop-Transcript
exit 0