#!/bin/sh
#############################################################################
## Copyright (c) Microsoft Corporation. All rights reserved.
## Licensed under the MIT license. See LICENSE file in the project root for full license information.
#############################################################################

#####################################################################################################################
##
##  File:     IsJoinDomainSuccess.sh
##  Purpose:  To verify if the machine is successfully unjoined domain by samba.
#####################################################################################################################

# Check the argument number
if [ $# -ne 0 ]; then
{
cat<<HELP
IsUnjoinDomainSuccess -- To verify if the machine is domain joined successfully.

USAGE: IsUnjoinDomainSuccess
HELP
echo false
exit 1
}
fi

# Get the domain name of the joined domain from local machine
domainjoined=`ldbsearch -H /usr/local/samba/private/secrets.ldb 'objectclass=primaryDomain'|grep realm|awk '{print $2}'`

# If no joined domain found, unjoin domain succeed
if [ -z "$domainjoined" ]; then
{
echo "Not joined to any domain."
echo true
exit 0
}
fi
if [ -n "$domainjoined" ]; then
{
echo "Joined to domain $domainjoined."
echo false
exit 1
}
fi
echo false
exit 1
