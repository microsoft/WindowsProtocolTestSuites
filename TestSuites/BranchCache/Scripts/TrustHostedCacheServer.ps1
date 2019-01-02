#############################################################################
## Copyright (c) Microsoft Corporation. All rights reserved.
## Licensed under the MIT license. See LICENSE file in the project root for full license information.
#############################################################################

#----------------------------------------------------------------------------
# This function is used to add the cert with password into cert store
#----------------------------------------------------------------------------
param(
[String]$certpath="c:\temp\Data\Certificate.pfx",
[String]$rootCertStore    = "LocalMachine",
[String]$certStore = "AuthRoot",
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
	if(Test-Path -Path $certPath)
	{
		Remove-Item -Path $certPath
	}
    $pfx=new-object System.Security.Cryptography.X509Certificates.X509Certificate2
	$pfx.import($certPath,$pfxPass,"Exportable,PersistKeySet")
	$store=new-object System.Security.Cryptography.X509Certificates.X509Store($certStore,$certRootStore)
	$store.open("MaxAllowed")
	$store.add($pfx)
	$store.close()
}

#----------------------------------------------------------------------------
# Add cert to Trusted Root Certification Authorities store
#----------------------------------------------------------------------------
Write-Info.ps1 "Add certificate to Trusted Root Certification Authorities Store." -foregroundcolor Green
ImportPfxCert $certpath $rootCertStore $certStore $certPsd

#----------------------------------------------------------------------------
# Ending
#----------------------------------------------------------------------------
Write-Info.ps1 "Completed enable routing and remote access."
Stop-Transcript
exit 0