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
echo          Start to build RDP Server Test Suite
echo ======================================

InvocationPath=$(dirname "$0")

TestSuiteRoot="$InvocationPath/../../../.."

if [ -z $Configuration ]
then
    Configuration="Release"
fi

if [ -z $OutDir ]
then
    OutDir="$TestSuiteRoot/drop/TestSuites/RDP/Server"
fi

declare -a CommonScripts=("Disable_Firewall.ps1" "Enable-WinRM.ps1" "Set-AutoLogon.ps1" "RestartAndRunFinish.ps1" "RestartAndRun.ps1" "TurnOff-FileReadonly.ps1")

if [ -d $OutDir -a "$OutDir" != "/" ]
then
    rm -rf $OutDir
fi

mkdir -p $OutDir/Batch
cp $TestSuiteRoot/TestSuites/RDP/Server/src/Batch/* $OutDir/Batch/ -f
cp $TestSuiteRoot/common/RunTestCasesByBinariesAndFilter.* $OutDir/Batch/ -f

mkdir -p $OutDir/Scripts
cp -R $TestSuiteRoot/TestSuites/RDP/Server/Setup/Scripts/* $OutDir/Scripts/ -f
for curr in "${CommonScripts[@]}"
do
    cp $TestSuiteRoot/CommonScripts/$curr $OutDir/Scripts/ -f
done

cp $TestSuiteRoot/TestSuites/RDP/Server/src/Deploy/LICENSE.rtf $OutDir/LICENSE.rtf -f

Cmd="dotnet publish \"$TestSuiteRoot/TestSuites/RDP/Server/src/RDP_Server.sln\" -c $Configuration -o $OutDir/Bin"

eval $Cmd

if [ $? -ne 0 ]
then
    echo "Failed to build RDP Server test suite"
    exit 1
fi

cp $TestSuiteRoot/AssemblyInfo/.version $OutDir/Bin -f

echo ==========================================================
echo          Build RDP Server test suite successfully         
echo ==========================================================
