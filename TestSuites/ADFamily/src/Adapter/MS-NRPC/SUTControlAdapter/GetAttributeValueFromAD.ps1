#############################################################
## Copyright (c) Microsoft. All rights reserved.
## Licensed under the MIT license. See LICENSE file in the project root for full license information.
#############################################################

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
# Get Information
#----------------------------------------------------------------------------
$primaryDCName = $PTFProp_Common_WritableDC1_NetbiosName
$primaryDomainDNSName =  $PTFProp_Common_PrimaryDomain_DNSName
$primaryDomainNetBiosName = $PTFProp_Common_PrimaryDomain_NetBiosName
$normalDomainUserAccount = $PTFProp_Common_DomainAdministratorName
$userName = "$primaryDomainNetBiosName\$normalDomainUserAccount"
$password = $PTFProp_Common_DomainUserPassword

$objectInstance = New-Object System.DirectoryServices.DirectoryEntry("LDAP://$primaryDCName/$objectPath",$userName,$password)
if ($objectInstance -eq $null)
{
    Throw "EXECUTE [GetAttributeValueFromAD.ps1] FAILED. Object does not exist in Active Directory."
}

#----------------------------------------------------------------------------
# Verifying the result
#----------------------------------------------------------------------------
try
{
    [string]$value = $objectInstance.Get($attribute)
}
catch
{
    return $null
}

return $value