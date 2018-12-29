#!/bin/sh
#############################################################################
## Copyright (c) Microsoft Corporation. All rights reserved.
## Licensed under the MIT license. See LICENSE file in the project root for full license information.
#############################################################################

#####################################################################################################################
##
##  File:     IsJoinDomainSuccess.sh
##  Purpose:  To verify if the machine is successfully joined domain by samba.
#####################################################################################################################

# Check the argument number
if [ $# -ne 1 ]; then
{
cat<<HELP
IsJoinDomainSuccess -- To verify if the machine is domain joined successfully.

USAGE: IsJoinDomainSuccess fulldomainname
HELP
echo false
exit 1
}
fi

# Get the parameters
fulldomainname=$1

# Get the domain name of the joined domain from local machine
domainjoined=`ldbsearch -H /usr/local/samba/private/secrets.ldb 'objectclass=primaryDomain'|grep realm|awk '{print $2}'`

# Check if the domain name of the joined domain of local machine is the same as expected
if [ -z "$domainjoined" ]; then
{
echo "Not joined to any domain."
echo false
exit 1
}
fi
if [ -n "$domainjoined" ]; then
{
if [ $domainjoined = $fulldomainname ]; then
{
echo "Joined to domain $domainjoined."
echo true
exit 0
}
fi
}
fi
echo false
exit 1
