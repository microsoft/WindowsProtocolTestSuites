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

$clientNetBiosName = GetPtfVariable 'Common.ENDPOINT.NetbiosName'
$domainName = GetPtfVariable 'Common.PrimaryDomain.DNSName'

$nameSplits=$domainName.Split('.')
$domainNetBiosName=$nameSplits[0]
$domainNameSuffix=$nameSplits[1]
$objectPath = "cn=$clientNetBiosName, cn=computers, dc=$domainNetBiosName, dc=$domainNameSuffix"

return $objectPath