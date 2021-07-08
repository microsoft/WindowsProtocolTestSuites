#!/bin/bash
#
# Copyright (c) Microsoft. All rights reserved.
# Licensed under the MIT license. See LICENSE file in the project root for full license information.
#
# Argument 1: Configuration, default value "Release".
# Argument 2: OutDir, output directory.

Configuration=$1

OutDir=$2

echo ======================================
echo Start to build PTMService
echo ======================================

InvocationPath=$(dirname "$0")

TestSuiteRoot="$InvocationPath/../.."

if [ -z $Configuration ]; then
    Configuration="Release"
fi

if [ -z $OutDir ]; then
    OutDir="$TestSuiteRoot/drop/PTMService"
fi

if [ -d $OutDir -a "$OutDir" != "/" ]; then
    rm -rf $OutDir
fi

Cmd="dotnet publish \"$TestSuiteRoot/ProtocolTestManager/PTMService/PTMService.sln\" -c $Configuration -o $OutDir"

eval $Cmd

if [ $? -ne 0 ]; then
    echo "Failed to build PTMService"
    exit 1
fi

echo ==========================================================
echo Build PTMService successfully
echo ==========================================================
