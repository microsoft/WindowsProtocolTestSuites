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
echo          Start to build FileServer
echo ======================================

InvocationPath=$(dirname "$0")

TestSuiteRoot="$InvocationPath/../../.."

if [ -z $Configuration ]
then
    Configuration="Release"
fi

if [ -z $OutDir ]
then
    OutDir="$TestSuiteRoot/drop/TestSuites/FileServer"
fi

declare -a CommonScripts=("Get-OSVersionNumber.ps1" "Write-Error.ps1" "Write-Info.ps1")

if [ -d $OutDir -a "$OutDir" != "/" ]
then
    rm -rf $OutDir
fi

mkdir -p $OutDir/Batch
cp $TestSuiteRoot/TestSuites/FileServer/src/Batch/*.sh $OutDir/Batch/ -f
cp $TestSuiteRoot/TestSuites/FileServer/src/Batch/*.ps1 $OutDir/Batch/ -f
cp $TestSuiteRoot/common/RunTestCasesByBinariesAndFilter.* $OutDir/Batch/ -f

mkdir -p $OutDir/Scripts
cp -R $TestSuiteRoot/TestSuites/FileServer/Setup/Scripts/* $OutDir/Scripts/ -f
for curr in "${CommonScripts[@]}"
do
    cp $TestSuiteRoot/CommonScripts/$curr $OutDir/Scripts/ -f
done

mkdir -p $OutDir/Bin/Data
cp -R $TestSuiteRoot/TestSuites/FileServer/src/Data/* $OutDir/Bin/Data -f
cp $TestSuiteRoot/TestSuites/FileServer/src/Deploy/LICENSE.rtf $OutDir/LICENSE.rtf -f

Cmd="dotnet publish \"$TestSuiteRoot/TestSuites/FileServer/ShareUtil/ShareUtil.sln\" -c $Configuration -o $OutDir/Utils"

eval $Cmd

if [ $? -ne 0 ]
then
    echo "Failed to build ShareUtil tool"
    exit 1
fi

Cmd="dotnet publish \"$TestSuiteRoot/TestSuites/FileServer/src/FileServer.sln\" -c $Configuration -o $OutDir/Bin"

eval $Cmd

if [ $? -ne 0 ]
then
    echo "Failed to build FileServer test suite"
    exit 1
fi

cp $TestSuiteRoot/AssemblyInfo/.version $OutDir/Bin -f

echo ==========================================================
echo          Build FileServer test suite successfully         
echo ==========================================================
