#############################################################
## Copyright (c) Microsoft. All rights reserved.
## Licensed under the MIT license. See LICENSE file in the project root for full license information.
#############################################################

$pdcNetBiosName = $PTFProp_Common_WritableDC1_NetbiosName
$domainName = $PTFProp_Common_PrimaryDomain_DNSName

$nameSplits=$domainName.Split('.')
$domainNetBiosName=$nameSplits[0]
$domainNameSuffix=$nameSplits[1]
$objectPath = "cn=$pdcNetBiosName, OU=Domain Controllers, dc=$domainNetBiosName, dc=$domainNameSuffix"

return $objectPath