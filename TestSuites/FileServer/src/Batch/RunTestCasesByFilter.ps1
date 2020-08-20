# Copyright (c) Microsoft. All rights reserved.
# Licensed under the MIT license. See LICENSE file in the project root for full license information.

param(
    [string]$Filter = "",    # Expression used to filter test cases.For example, "TestCategory=BVT&TestCategory=SMB311" will filter out test cases which have test category BVT and SMB311. 
    [switch]$DryRun = $false # If set, just list all test cases instead of running tests actually.
)

$invocationPath =  Split-Path $MyInvocation.MyCommand.Definition -Parent

$script = Join-Path $invocationPath "RunTestCasesByBinariesAndFilter.ps1"

$binaries = @(
    "Auth_ServerTestSuite.dll",
    "MS-DFSC_ServerTestSuite.dll",
    "MS-FSAModel_ServerTestSuite.dll",
    "MS-FSA_ServerTestSuite.dll",
    "MS-FSRVP_ServerTestSuite.dll",
    "MS-RSVD_ServerTestSuite.dll",
    "MS-SMB2Model_ServerTestSuite.dll",
    "MS-SMB2_ServerTestSuite.dll",
    "MS-SQOS_ServerTestSuite.dll",
    "ServerFailoverTestSuite.dll"
)

$cmd = "$script -Binaries `$binaries -Filter `$Filter -DryRun:`$(`$DryRun.IsPresent)"

Invoke-Expression $cmd
