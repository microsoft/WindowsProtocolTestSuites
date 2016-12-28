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


$domainName = GetPtfVariable "Common.PrimaryDomain.DNSName"
$domainNC = "DC=" + $domainName.Replace(".",",DC=")

$objectPath = "cn=Administrator, cn=Users, $domainNC"

return $objectPath