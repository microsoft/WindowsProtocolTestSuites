#############################################################
## Copyright (c) Microsoft Corporation. All rights reserved.
## Licensed under the MIT license. See LICENSE file in the project root for full license information.
#############################################################

#############################################################################
##
## Microsoft Windows Powershell Scripting
## File:           Get-CertHash.ps1
## Purpose:        Get the cert hash of a certificate in the store.
## Version:        1.0 (14 July, 2008)
##
##############################################################################

param(
[string]$certIndex,
[string]$storeName = "My"
)

#----------------------------------------------------------------------------
# Starting script
#----------------------------------------------------------------------------
Write-Host "EXECUTING [Get-CertHash.ps1] ..." -foregroundcolor cyan
Write-Host "`$certIndex     = $certIndex"
Write-Host "`$storeName     = $storeName"
 
#----------------------------------------------------------------------------
# Function: Show-ScriptUsage
# Usage   : Describes the usage information and options
#----------------------------------------------------------------------------
function Show-ScriptUsage
{    
    Write-host 
    Write-host "Get the cert hash of a certificate in the store."
    Write-host
    Write-host "Example: Get-CertHash.ps1 account my"
    Write-host
}

#----------------------------------------------------------------------------
# Show help if required
#----------------------------------------------------------------------------
if ($args[0] -match '-(\?|(h|(help)))')
{
    Show-ScriptUsage 
    return
}

#----------------------------------------------------------------------------
# Verify required parameters
#----------------------------------------------------------------------------
if ($certIndex -eq $null -or $certIndex -eq "")
{
    Throw "the path of config file is required."
}

#----------------------------------------------------------------------------
# Get the cert hash of a certificate in store
#----------------------------------------------------------------------------
$certInfos =  certutil -store $storeName $certIndex

foreach($line in $certInfos)
{
    if($line.Contains("Cert Hash"))
    {
        $certHashLine = $line
        break
    }
}

if($certHashLine -ne $null)
{
    $startIndex = $certHashLine.IndexOf(':')
    $certHash = $certHashLine.SubString($startIndex + 1 )
    $certHash = $certHash.replace(" ", "")
}
else
{
    throw "Failed to get the cert hash of certIndex: $certIndex in $storeName store."  
}

#----------------------------------------------------------------------------
# Ending script
#----------------------------------------------------------------------------
Write-Host "EXECUTE [Get-CertHash.ps1] Finished(SUCCEED)." -foregroundcolor green
return $certHash

exit
