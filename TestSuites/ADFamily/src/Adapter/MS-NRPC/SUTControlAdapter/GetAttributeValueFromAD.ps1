#############################################################
## Copyright (c) Microsoft. All rights reserved.
## Licensed under the MIT license. See LICENSE file in the project root for full license information.
#############################################################

function GetPtfVariable
{
    param($name)
	$v = Get-Variable -Name ("PTFProp"+$name)
	return $v.Value
}

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
$primaryDCName = GetPtfVariable "Common.WritableDC1.NetbiosName"
$primaryDomainDNSName =  GetPtfVariable "Common.PrimaryDomain.DNSName"
$primaryDomainNetBiosName = GetPtfVariable "Common.PrimaryDomain.NetBiosName"
$normalDomainUserAccount = GetPtfVariable "Common.DomainAdministratorName"
$userName = "$primaryDomainNetBiosName\$normalDomainUserAccount"
$password = GetPtfVariable "Common.DomainUserPassword"

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