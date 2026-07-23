#!/bin/bash
# Copyright (c) Microsoft. All rights reserved.
# Licensed under the MIT license. See LICENSE file in the project root for full license information.
#
# Custom Script Extension bootstrap shared by the Linux Driver across scenarios
# (Workgroup, Domain). The Bicep module loads this file with loadTextContent(),
# substitutes the __TOKENS__ (same set as cse-bootstrap.ps1), and delivers it
# base64-encoded via the extension's encrypted protectedSettings 'script'
# property. The script downloads the package itself (no fileUris), so it never
# depends on the waagent download directory layout, whose sequence number
# changes when the extension re-runs.

set -e
export DEBIAN_FRONTEND=noninteractive

echo 'Starting __SCENARIO__ __ROLE__ setup...'
mkdir -p '/opt/__PACKAGE_NAME__'
apt-get update -qq
apt-get install -y -qq wget unzip apt-transport-https

if ! command -v pwsh >/dev/null 2>&1; then
    wget -q https://packages.microsoft.com/config/ubuntu/24.04/packages-microsoft-prod.deb -O /tmp/packages-microsoft-prod.deb
    dpkg -i /tmp/packages-microsoft-prod.deb
    rm -f /tmp/packages-microsoft-prod.deb
    apt-get update -qq
    apt-get install -y -qq powershell
fi

echo 'Waiting for DNS resolution of __PACKAGE_HOST__ (DNS may still be coming up)...'
dns=0
for i in $(seq 1 60); do
    if getent hosts '__PACKAGE_HOST__' >/dev/null 2>&1; then
        dns=1
        break
    fi
    sleep 30
done
if [ $dns -ne 1 ]; then
    echo 'DNS resolution failed'
    exit 1
fi

dl=0
for i in $(seq 1 10); do
    if wget -q '__PACKAGE_URL__' -O '/opt/__PACKAGE_NAME__.zip'; then
        dl=1
        break
    fi
    sleep 30
done
if [ $dl -ne 1 ]; then
    echo 'Package download failed'
    exit 1
fi

unzip -o '/opt/__PACKAGE_NAME__.zip' -d '/opt/__PACKAGE_NAME__'
rm -f '/opt/__PACKAGE_NAME__.zip'

if [ -f '/opt/__PACKAGE_NAME__/DSC/Scripts/Set-ConfigCredential.ps1' ]; then
    echo 'Injecting credential into Config.json...'
    pwsh -ExecutionPolicy Unrestricted -File '/opt/__PACKAGE_NAME__/DSC/Scripts/Set-ConfigCredential.ps1' -PasswordBase64 '__PASSWORD_B64__'
fi

if [ -f '/opt/__PACKAGE_NAME__/DSC/__DEPLOY_SCRIPT__' ]; then
    echo 'Starting __DEPLOY_SCRIPT__ (DSC + imperative)...'
    cd '/opt/__PACKAGE_NAME__/DSC'
    pwsh -ExecutionPolicy Unrestricted -File '/opt/__PACKAGE_NAME__/DSC/__DEPLOY_SCRIPT__' -WorkingPath '/opt/__PACKAGE_NAME__'
else
    echo '__DEPLOY_SCRIPT__ not found, skipping configuration'
fi

echo '__SCENARIO__ __ROLE__ setup completed'
