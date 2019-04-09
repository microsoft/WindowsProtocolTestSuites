#!/bin/sh
#############################################################################
## Copyright (c) Microsoft Corporation. All rights reserved.
## Licensed under the MIT license. See LICENSE file in the project root for full license information.
#############################################################################

#####################################################################################################################
##
##  File:     LocateDomainController.sh
##  Purpose:  To trigger the machine to locate a domain controller.
#####################################################################################################################

# Check the argument number
if [ $# -ne 1 ]; then
{
cat<<HELP
LocateDomainController -- To trigger the client to locate a domain controller.

USAGE: LocateDomainController fulldomainname
HELP
echo false
exit 1
}
fi

# Get the parameters
fulldomainname=$1

# Trigger the client to locate domain controller
result=`nslookup -type=srv _ldap._tcp.$fulldomainname|grep _ldap._tcp.$fulldomainname|awk '{print $7}'|sed 's/.$//'`

# Return the domain controller name if successfully found
if [ -z "$result" ]; then
{
echo ""
exit 1
}
else
{
echo $result
exit 0
}
fi
