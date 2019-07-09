#############################################################
## Copyright (c) Microsoft. All rights reserved.
## Licensed under the MIT license. See LICENSE file in the project root for full license information.
#############################################################

$PSScriptRoot = Split-Path -Parent -Path $MyInvocation.MyCommand.Definition
Push-Location $PSScriptRoot

$objectPath = .\GetClientObjPath.ps1
$attribute = "dNSHostName"

# Verify required parameters
if ($objectPath -eq $null -or $objectPath -eq "")
{
    Throw "Parameter `$objectPath is required."
}
if ($attribute -eq $null -or $attribute -eq "")
{
    Throw "Parameter `$attribute is required."
}

#----------------------------------------------------------------------------
# Get Infomation
#----------------------------------------------------------------------------
$primaryDCName = $PTFProp_Common_WritableDC1_NetbiosName
$primaryDomainNetBiosName = $PTFProp_MS_NRPC_SUT_PrimaryDomainNetBiosName
$normalDomainUserAccount = $PTFProp_Common_DomainAdministratorName
$userName = "$primaryDomainNetBiosName\$normalDomainUserAccount"
$password = $PTFProp_Common_DomainUserPassword

$objectInstance = New-Object System.DirectoryServices.DirectoryEntry("LDAP://$primaryDCName/$objectPath",$userName,$password)
if ($objectInstance -eq $null)
{
    Throw "EXECUTE [SetDnsHostNameAttributeOfClient.ps1] FAILED. Object does not exist in Active Directory."
}

#----------------------------------------------------------------------------
# Verifying the result
#----------------------------------------------------------------------------
try
{
    $objectInstance.Put($attribute, $dnsHostName) 
    $objectInstance.SetInfo()
    return $true
}
catch
{
    return $false
}