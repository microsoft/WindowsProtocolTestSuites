#!/bin/sh
#############################################################################
## Copyright (c) Microsoft Corporation. All rights reserved.
## Licensed under the MIT license. See LICENSE file in the project root for full license information.
#############################################################################

#####################################################################################################################
##
##  File:     JoinDomainPredefAcct.sh
##  Purpose:  To trigger the machine join domain by a predefined account.
#####################################################################################################################

# Check the argument number
if [ $# -ne 3 ]; then
{
cat<<HELP
JoinDomainPredefAcct -- To trigger the client join domain by a predefined account.

USAGE: JoinDomainPredefAcct fulldomainname username password
HELP
echo false
exit 1
}
fi

# Get the parameters
fulldomainname=$1
username=$2
password=$3

# Trigger the client to join domain
cd /usr/local/samba-master/bin
msg=`samba-tool domain join $fulldomainname MEMBER -U$username%$password --realm=$fulldomainname`
echo $msg
result=`echo $msg|grep -l "Joined domain"|wc -l`


# If the result contains "Joined Domain ...", it indicates that the joining procedure is successful
if [ $result -ne 0 ]; then
{
echo true
exit 0
}
else
{
echo "Not joined to any domain."
echo false
exit 1
}
fi
