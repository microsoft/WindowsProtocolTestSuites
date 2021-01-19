#!/bin/sh
# Copyright (c) Microsoft. All rights reserved.
# Licensed under the MIT license. See LICENSE file in the project root for full license information.

##############################################################################
#
#  File:     Create-SelfSignedCertificate.sh
#  Purpose:  Create a self signed certificate.
##############################################################################

# Check the argument number
if [ $# -ne 4 ]; then
{
cat<<HELP
Create-SelfSignedCertificate -- To create a self signed certificate.

USAGE: Create-SelfSignedCertificate certificateFileName certCN certPwd certificatePath
Example:  Create-SelfSignedCertificate RDPClient_Client RDPClient_Client Password01! /
HELP
echo false
exit 1
}
fi

# parameters
certificateFileName=$1
certCN=$2
certPwd=$3
certificatePath=$4
crtFileName="${certificateFileName}.crt"
keyFileName="${certificateFileName}.key"
pfxFileName="${certificateFileName}.pfx"

echo $certPwd > certPassword.txt

openssl req -newkey rsa:4096 \
            -x509 \
            -sha256 \
            -days 3650 \
            -nodes \
            -out $crtFileName \
            -keyout $keyFileName \
            -passout file:certPassword.txt \
            -subj "/C=CN/ST=Shanghai/L=Shanghai/O=Microsoft/OU=MS Department/CN=$certCN"

openssl pkcs12 -export -nodes -out $pfxFileName -inkey $keyFileName -in $crtFileName -passout pass:$certPwd

cp $certificateFileName.* $certificatePath

