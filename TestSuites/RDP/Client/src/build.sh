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
echo          Start to build RDP Client Test Suite
echo ======================================

InvocationPath=$(dirname "$0")

TestSuiteRoot="$InvocationPath/../../../.."

if [ -z $Configuration ]
then
    Configuration="Release"
fi

if [ -z $OutDir ]
then
    OutDir="$TestSuiteRoot/drop/TestSuites/RDP/Client"
fi

declare -a CommonScripts=("Disable_Firewall.ps1" "Get-Parameter.ps1" "Modify-ConfigFileNode.ps1" "Set-Parameter.ps1" 
"TurnOff-FileReadonly.ps1" "Enable-WinRM.ps1")

if [ -d $OutDir -a "$OutDir" != "/" ]
then
    rm -rf $OutDir
fi

mkdir -p $OutDir/Batch
cp $TestSuiteRoot/TestSuites/RDP/Client/src/Batch/* $OutDir/Batch/ -f
cp $TestSuiteRoot/common/RunTestCasesByBinariesAndFilter.* $OutDir/Batch/ -f

mkdir -p $OutDir/Data
cp $TestSuiteRoot/TestSuites/RDP/Client/Setup/Data/* $OutDir/Data/ -fr

mkdir -p $OutDir/Scripts
cp -R $TestSuiteRoot/TestSuites/RDP/Client/Setup/Scripts/* $OutDir/Scripts/ -f
for curr in "${CommonScripts[@]}"
do
    cp $TestSuiteRoot/CommonScripts/$curr $OutDir/Scripts/ -f
done

mkdir -p $OutDir/TestData
cp -R $TestSuiteRoot/TestSuites/RDP/Client/src/TestSuite/TestData/*.bmp $OutDir/TestData -f
cp -R $TestSuiteRoot/TestSuites/RDP/Client/src/TestSuite/TestData/*.xml $OutDir/TestData -f
cp -R $TestSuiteRoot/TestSuites/RDP/Client/src/TestSuite/RDPEDISP/RdpedispEnhancedAdapterImages/*.png $OutDir/TestData -f
cp -R $TestSuiteRoot/TestSuites/RDP/Client/src/TestSuite/RDPEGFX/H264TestData/*.* $OutDir/TestData -f
cp -R $TestSuiteRoot/TestSuites/RDP/Client/src/TestSuite/RDPEGFX/H264TestData/BaseImage/* $OutDir/TestData -f

cp $TestSuiteRoot/TestSuites/RDP/Client/src/Deploy/LICENSE.rtf $OutDir/LICENSE.rtf -f

Cmd="dotnet publish \"$TestSuiteRoot/TestSuites/RDP/Client/src/RDP_Client.sln\" -c $Configuration -o $OutDir/Bin"

eval $Cmd

if [ $? -ne 0 ]
then
    echo "Failed to build RDP Client test suite"
    exit 1
fi

cp $TestSuiteRoot/AssemblyInfo/.version $OutDir/Bin -f

echo ==========================================================
echo          Build RDP Client test suite successfully        
echo ==========================================================
