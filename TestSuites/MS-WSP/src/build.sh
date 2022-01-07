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
echo Start to build MS-WSP
echo ======================================

InvocationPath=$(dirname "$0")

TestSuiteRoot="$InvocationPath/../../.."

if [ -z $Configuration ]; then
    Configuration="Release"
fi

if [ -z $OutDir ]; then
    OutDir="$TestSuiteRoot/drop/TestSuites/MS-WSP"
fi

declare -a CommonScripts=()

if [ -d $OutDir -a "$OutDir" != "/" ]; then
    rm -rf $OutDir
fi

PluginDir="$OutDir/Plugin"
mkdir -p $PluginDir
cp $TestSuiteRoot/TestSuites/MS-WSP/src/Plugin/WSPServerPlugin/*.xml "$PluginDir/" -f

TargetDir="$PluginDir/doc"
mkdir -p $TargetDir
cp $TestSuiteRoot/TestSuites/MS-WSP/src/Plugin/WSPServerPlugin/Docs/* "$TargetDir/" -f

mkdir -p $OutDir/Batch
cp $TestSuiteRoot/TestSuites/MS-WSP/src/Batch/*.sh $OutDir/Batch/ -f
cp $TestSuiteRoot/TestSuites/MS-WSP/src/Batch/*.ps1 $OutDir/Batch/ -f
cp $TestSuiteRoot/common/RunTestCasesByBinariesAndFilter.* $OutDir/Batch/ -f

mkdir -p $OutDir/Scripts
cp -R $TestSuiteRoot/TestSuites/MS-WSP/Setup/Scripts/* $OutDir/Scripts/ -f
for curr in "${CommonScripts[@]}"; do
    cp $TestSuiteRoot/CommonScripts/$curr $OutDir/Scripts/ -f
done

mkdir -p $OutDir/Data
cp -R $TestSuiteRoot/TestSuites/MS-WSP/Setup/Data/* $OutDir/Data -f

cp $TestSuiteRoot/TestSuites/MS-WSP/src/Deploy/LICENSE.rtf $OutDir/LICENSE.rtf -f

Cmd="dotnet publish \"$TestSuiteRoot/TestSuites/MS-WSP/src/MS-WSP_Server.sln\" -c $Configuration -o $OutDir/Bin"

eval $Cmd

if [ $? -ne 0 ]; then
    echo "Failed to build MS-WSP test suite"
    exit 1
fi

cp $TestSuiteRoot/AssemblyInfo/.version $OutDir/Bin -f

echo ==========================================================
echo Build MS-WSP test suite successfully
echo ==========================================================
