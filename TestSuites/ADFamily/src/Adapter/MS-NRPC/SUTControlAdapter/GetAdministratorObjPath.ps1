#############################################################
## Copyright (c) Microsoft. All rights reserved.
## Licensed under the MIT license. See LICENSE file in the project root for full license information.
#############################################################

$domainName = $PTFProp_Common_PrimaryDomain_DNSName
$domainNC = "DC=" + $domainName.Replace(".",",DC=")

$objectPath = "cn=Administrator, cn=Users, $domainNC"

return $objectPath