:: Copyright (c) Microsoft. All rights reserved.
:: Licensed under the MIT license. See LICENSE file in the project root for full license information.

@echo off

echo ==============================================
echo          Start to run FRS2 test cases
echo ==============================================

set CurrentPath=%~dp0
call "%CurrentPath%setVsTestPath.cmd"
if ErrorLevel 1 (
	exit /b 1
)

%vstest% "..\bin\AD_ServerTestSuite.dll" /Settings:..\bin\Serverlocaltestrun.testrunconfig /TestCaseFilter:"TestCategory=MS-FRS2" /Logger:trx
pause