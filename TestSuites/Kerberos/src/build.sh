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
echo          Start to build Kerberos
echo ======================================

InvocationPath=$(dirname "$0")

TestSuiteRoot="$InvocationPath/../../.."

if [ -z $Configuration ]
then
    Configuration="Release"
fi

if [ -z $OutDir ]
then
    OutDir="$TestSuiteRoot/drop/TestSuites/Kerberos"
fi

declare -a CommonScripts=("Change-DomainUserPassword.ps1" "Check-ReturnValue.ps1" "Disable_Firewall.ps1" "Get-DomainControllerParameters.ps1" "Get-OSVersionNumber.ps1" "Get-Parameter.ps1" "GetVMNameByComputerName.ps1" "GetVmParameters.ps1" "Install-ADDS.ps1" "Install-FSRM.ps1" "Install-IIS.ps1" "Join-Domain.ps1" "PromoteDomainController.ps1" "RestartAndRun.bat" "RestartAndRun.ps1" "RestartAndRunFinish.ps1" "Set-AutoLogon.ps1" "Set-ExecutionPolicy-Unrestricted.ps1" "Set-NetworkConfiguration.ps1" "Set-Parameter.ps1" "TurnOff-FileReadonly.ps1" "Write-Info.ps1")

if [ -d $OutDir -a "$OutDir" != "/" ]
then
    rm -rf $OutDir
fi

mkdir -p $OutDir/Batch
cp $TestSuiteRoot/TestSuites/Kerberos/src/Batch/*.sh $OutDir/Batch/ -f
cp $TestSuiteRoot/TestSuites/Kerberos/src/Batch/*.ps1 $OutDir/Batch/ -f
cp $TestSuiteRoot/common/RunTestCasesByBinariesAndFilter.* $OutDir/Batch/ -f

mkdir -p $OutDir/Scripts
cp -R $TestSuiteRoot/TestSuites/Kerberos/Setup/Scripts/* $OutDir/Scripts/ -f
for curr in "${CommonScripts[@]}"
do
    cp $TestSuiteRoot/CommonScripts/$curr $OutDir/Scripts/ -f
done

cp $TestSuiteRoot/TestSuites/Kerberos/src/Deploy/LICENSE.rtf $OutDir/LICENSE.rtf -f

mkdir -p $OutDir/Bin
Cmd="dotnet publish \"$TestSuiteRoot/TestSuites/Kerberos/src/Kerberos_Server.sln\" -c $Configuration -p:Platform=x86 -o $OutDir/Bin"

eval $Cmd

if [ $? -ne 0 ]
then
    echo "Failed to build Kerberos test suite"
    exit 1
fi

cp $TestSuiteRoot/AssemblyInfo/.version $OutDir/Bin -f

echo ==========================================================
echo          Build Kerberos test suite successfully         
echo ==========================================================
