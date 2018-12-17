#!/bin/sh
#############################################################################
## Copyright (c) Microsoft Corporation. All rights reserved.
## Licensed under the MIT license. See LICENSE file in the project root for full license information.
#############################################################################

#####################################################################################################################
##
##  File:     UnjoinDomain.sh
##  Purpose:  To trigger the machine unjoin domain.
#####################################################################################################################

# Check the argument number
if [ $# -ne 0 ]; then
{
cat<<HELP
UnjoinDomain -- To trigger the machine to unjoin domain.

USAGE: UnjoinDomain
HELP
echo false
exit 1
}
fi

cd /usr/local/samba/private/

# Tigger unjoin domain
if [ -f sam.ldb ]; then
{
mv sam.ldb sam.ldb.bak
}
fi
if [ -f secrets.ldb ]; then
{
mv secrets.ldb secrets.ldb.bak
}
fi

# Check if the removed files still exist, if any of the file still exist, it indicates that the unjoin is not successful
if [ ! -f sam.ldb ] && [ ! -f secrets.ldb ]; then
{
echo "Unjoin domain succeeded."
echo true
exit 0
}
else
{
echo "Unjoin domain failed."
echo false
exit 1
}
fi
