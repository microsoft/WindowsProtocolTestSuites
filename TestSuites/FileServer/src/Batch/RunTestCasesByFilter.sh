#!/bin/bash
#
# Copyright (c) Microsoft. All rights reserved.
# Licensed under the MIT license. See LICENSE file in the project root for full license information.
#
# Argument 1: Expression used to filter test cases. For example, "TestCategory=BVT&TestCategory=SMB311" will filter out test cases which have test category BVT and SMB311. 
# Argument 2: If set, just list all filtered test cases instead of running tests actually.

InvocationPath=$(dirname "$0")

Binaries="Auth_ServerTestSuite.dll,\
    MS-DFSC_ServerTestSuite.dll,\
    MS-FSAModel_ServerTestSuite.dll,\
    MS-FSA_ServerTestSuite.dll,\
    MS-RSVD_ServerTestSuite.dll,\
    MS-SMB2Model_ServerTestSuite.dll,\
    MS-SMB2_ServerTestSuite.dll,\
    MS-SQOS_ServerTestSuite.dll,\
    ServerFailoverTestSuite.dll"

Filter=$1

DryRun=$2

Cmd="$InvocationPath/RunTestCasesByBinariesAndFilter.sh \"$Binaries\" \"$Filter\" \"$DryRun\""

eval $Cmd
