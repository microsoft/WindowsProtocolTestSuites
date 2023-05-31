#!/bin/bash
#
# Copyright (c) Microsoft. All rights reserved.
# Licensed under the MIT license. See LICENSE file in the project root for full license information.
#
# Argument 1: Configuration, default value "Release".
# Argument 2: OutDir, output directory.

Configuration=$1

OutDir=$2

echo =============================================
echo     Start to build MS-XCA Test Suite
echo =============================================

InvocationPath=$(dirname "$0")

TestSuiteRoot="$InvocationPath/../../.."

if [ -z $Configuration ]; then
    Configuration="Release"
fi

if [ -z $OutDir ]; then
    OutDir="$TestSuiteRoot/drop/TestSuites/MS-XCA"
fi

declare -a CommonScripts=("Get-OSVersionNumber.ps1" "Write-Error.ps1" "Write-Info.ps1")

if [ -d $OutDir -a "$OutDir" != "/" ]; then
    rm -rf $OutDir
fi

mkdir -p $OutDir/Batch
cp $TestSuiteRoot/TestSuites/FileServer/src/Batch/* $OutDir/Batch/ -f
cp $TestSuiteRoot/common/RunTestCasesByBinariesAndFilter.* $OutDir/Batch/ -f

mkdir -p $OutDir/TestData
cp $TestSuiteRoot/TestSuites/MS-XCA/Setup/TestData/* $OutDir/TestData/ -fr

mkdir -p $OutDir/UserData
cp $TestSuiteRoot/TestSuites/MS-XCA/Setup/UserData/* $OutDir/UserData/ -fr


cp $TestSuiteRoot/TestSuites/MS-XCA/src/Deploy/LICENSE.rtf $OutDir/LICENSE.rtf -f


Cmd="dotnet publish \"$TestSuiteRoot/TestSuites/MS-XCA/XcaTestApp/XcaTestApp.sln\" -c $Configuration -o $OutDir/Utils/XcaTestApp"

eval $Cmd

if [ $? -ne 0 ]; then
    echo "Failed to build XcaTestApp tool"
    exit 1
fi

Cmd="dotnet publish \"$TestSuiteRoot/TestSuites/MS-XCA/src/MS-XCA.sln\" -c $Configuration -o $OutDir/Bin"

eval $Cmd

if [ $? -ne 0 ]; then
    echo "Failed to build MS-XCA test suite"
    exit 1
fi

cp $TestSuiteRoot/AssemblyInfo/.version $OutDir/Bin -f

echo ==============================================
echo    Built MS-XCA test suite successfully
echo ==============================================
