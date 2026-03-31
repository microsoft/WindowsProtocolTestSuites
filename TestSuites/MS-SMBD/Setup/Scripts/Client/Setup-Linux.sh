#!/bin/bash
# Copyright (c) Microsoft. All rights reserved.
# Licensed under the MIT license. See LICENSE file in the project root for full license information.

Sudo apt update
# Install dotnet 8 SDK
sudo apt install -y dotnet-sdk-8.0

#Install PowerShell
sudo apt-get install -y wget apt-transport-https software-properties-common
source /etc/os-release
wget -q https://packages.microsoft.com/config/ubuntu/24.04/packages-microsoft-prod.deb
sudo dpkg -i packages-microsoft-prod.deb
rm packages-microsoft-prod.deb
sudo apt update
sudo apt-get install -y powershell

#Install C++ build
sudo apt install build-essential

#Install RDMA libray
sudo apt update
sudo apt install -y rdma-core ibverbs-utils librdmacm-dev perftest infiniband-diags

#Disable firewall
sudo ufw disable