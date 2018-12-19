:: Copyright (c) Microsoft. All rights reserved.
:: Licensed under the MIT license. See LICENSE file in the project root for full license information.

@echo off

echo =======================================================
echo          Start to run Kerberos test cases by category
echo =======================================================

echo:
echo Please select test categories below
echo You could use logical operators "|", "!" and "&" to combine multiple categories:
echo   TestCategory=BVT:               All BVT test cases
echo   TestCategory=Negative:          All negative test cases
echo   TestCategory=KILE:              Test cases for MS-KILE
echo   TestCategory=FAST:              Test cases for RFC6113
echo   TestCategory=Claims:            Test cases for Claims
echo   TestCategory=CBAC:              Test cases for file access on CBAC aware file server
echo   TestCategory=Silo:              Test cases for authentication silo and policy
echo: 
echo Examples:
echo   "(TestCategory=BVT)&(TestCategory=KILE)"         (Execute test cases have categories "BVT" and "KILE")
echo   "(TestCategory=BVT)|(TestCategory=KILE)"         (Execute test cases have categories "BVT" or "KILE")
echo   "(TestCategory=BVT)&!(TestCategory=KILE)"        (Execute test cases have category "BVT" but do NOT have category "KILE")
echo:
set /P category=Set test categories you want to execute, default is "TestCategory=BVT" if nothing input:
if ("%category%") == ("") (
	set category="TestCategory=BVT"
)
echo Selected category combination is: "%category%"

set CurrentPath=%~dp0
call "%CurrentPath%setVsTestPath.cmd"
if ErrorLevel 1 (
	exit /b 1
)

%vstest% "..\Bin\Kerberos_ServerTestSuite.dll" /Settings:..\Bin\ServerLocalTestRun.testrunconfig /Logger:trx /TestCaseFilter:%category%
pause