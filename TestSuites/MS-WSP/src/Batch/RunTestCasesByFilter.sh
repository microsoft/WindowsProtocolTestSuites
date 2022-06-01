#!/bin/bash
#
# Copyright (c) Microsoft. All rights reserved.
# Licensed under the MIT license. See LICENSE file in the project root for full license information.
#
# Argument 1: Expression used to filter test cases. For example, "TestCategory=BVT&TestCategory=CPMConnect" will filter out test cases which have test category BVT and CPMConnect. 
# Argument 2: If set, just list all filtered test cases instead of running tests actually.

InvocationPath=$(dirname "$0")

Binaries="MS-WSP_ServerTestSuite.dll"

Filter=$1

DryRun=$2

Cmd="$InvocationPath/RunTestCasesByBinariesAndFilter.sh \"$Binaries\" \"$Filter\" \"$DryRun\""

eval $Cmd
