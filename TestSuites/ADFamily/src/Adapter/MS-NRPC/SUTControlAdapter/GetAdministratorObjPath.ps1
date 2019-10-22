#############################################################
## Copyright (c) Microsoft. All rights reserved.
## Licensed under the MIT license. See LICENSE file in the project root for full license information.
#############################################################

$domainName = $PTFProp_Common_PrimaryDomain_DNSName
$normalDomainUserAccount = $PTFProp_Common_DomainAdministratorName
$domainNC = "DC=" + $domainName.Replace(".",",DC=")

$objectPath = "cn=$normalDomainUserAccount, cn=Users, $domainNC"

return $objectPath