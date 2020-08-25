#!/bin/bash
#
# Copyright (c) Microsoft. All rights reserved.
# Licensed under the MIT license. See LICENSE file in the project root for full license information.
#
# Argument 1: A list containing name of the test binaries seperated by comma.
# Argument 2: Expression used to filter test cases. For example, "TestCategory=BVT&TestCategory=SMB311" will filter out test cases which have test category BVT and SMB311. 
# Argument 3: If set, just list all filtered test cases instead of running tests actually.

InvocationPath=$(dirname "$0")

RootPath="$InvocationPath/.."

BinPath="$RootPath/Bin"

TestResultPath="$RootPath/TestResults"

Binaries=$1

declare -a TestBinaries=(`echo $Binaries | tr "," " "`)

TestBinariesString=""

for TestBinary in "${TestBinaries[@]}"
do
    TestBinariesString+="\"$BinPath/$TestBinary\" "
done

Filter=$2

if [ -z $Filter ]
then
    TestCaseFilter=""
else
    TestCaseFilter="--filter \"$Filter\""
fi

DryRun=$3

if [ ! -z $DryRun ]
then
    Cmd="dotnet test $TestBinariesString $TestCaseFilter --list-tests"
else
    Cmd="dotnet test $TestBinariesString $TestCaseFilter --logger trx --ResultsDirectory \"$TestResultPath\""
fi

eval $Cmd
