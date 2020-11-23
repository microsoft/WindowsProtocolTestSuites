#!/bin/bash
#
# Copyright (c) Microsoft. All rights reserved.
# Licensed under the MIT license. See LICENSE file in the project root for full license information.
#
# Argument 1: Configuration, default value "Release".

Configuration=$1

echo ======================================
echo           Start to build RDPSUTControlAgent
echo ======================================

InvocationPath=$(dirname "$0")

TestSuiteRoot="$InvocationPath/../../../.."

if [ -z $Configuration ]
then
    Configuration="Release"
fi

Cmd="dotnet publish \"$TestSuiteRoot/TestSuites/RDP/RDPSUTControlAgent/CSharp/RDPSUTControlAgent.sln\" -c $Configuration"

eval $Cmd

if [ $? -ne 0 ]
then
    echo "Failed to build RDP SUT Control Agent"
    exit 1
fi

echo ==========================================================
echo          Build RDPSUTControlAgent successfully          
echo ==========================================================
