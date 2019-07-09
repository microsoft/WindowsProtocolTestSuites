#############################################################
## Copyright (c) Microsoft. All rights reserved.
## Licensed under the MIT license. See LICENSE file in the project root for full license information.
#############################################################

$clientNetBiosName = $PTFProp_Common_ENDPOINT_NetbiosName
$domainName = $PTFProp_Common_PrimaryDomain_DNSName

$nameSplits=$domainName.Split('.')
$domainNetBiosName=$nameSplits[0]
$domainNameSuffix=$nameSplits[1]
$objectPath = "cn=$clientNetBiosName, cn=computers, dc=$domainNetBiosName, dc=$domainNameSuffix"

return $objectPath