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

$pdcNetBiosName = GetPtfVariable "Common.WritableDC1.NetbiosName"
$domainName = GetPtfVariable "Common.PrimaryDomain.DNSName" 

$nameSplits=$domainName.Split('.')
$domainNetBiosName=$nameSplits[0]
$domainNameSuffix=$nameSplits[1]
$objectPath = "cn=$pdcNetBiosName, OU=Domain Controllers, dc=$domainNetBiosName, dc=$domainNameSuffix"

return $objectPath