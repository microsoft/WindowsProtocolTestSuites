#############################################################################
## Copyright (c) Microsoft. All rights reserved.
## Licensed under the MIT license. See LICENSE file in the project root for full license information.
#############################################################################

# Upload the domain rename script into the directory that will perform the domain rename related directory changes 
# on all domain controllers.

#This script can only be run on the system which installed AD DS and AD LDS Tools 

rendom /list /listfile:/DomainList.xml


#[xml]$domainListFile = Get-Content "/DomainList.xml"
#$domainEntries = $domainListFile.Forest.Domain
#foreach($domain in $domainEntries)
#{
 #   if($domain.DNSname -eq "$oldDNSName")
#	{
#	    $domain.DNSname = $newDNSName
#		break
#	}
#}
#$domainListFile.Save("/DomainList.xml")

#convert to unicode file
#get-content -path "/DomainList.xml" | out-file "/DomainListUnicode.xml" -encoding unicode

rendom /upload /listfile:/DomainList.xml
return $true