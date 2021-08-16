:: Copyright (c) Microsoft. All rights reserved.
:: Licensed under the MIT license. See LICENSE file in the project root for full license information.

@echo off

echo ==============================================
echo          Start to run ADFamily all test cases
echo ==============================================

set CurrentPath=%~dp0
call "%CurrentPath%setVsTestPath.cmd"
if ErrorLevel 1 (
	exit /b 1
)

%vstest% "..\bin\AD_ServerTestSuite.dll" /Settings:..\bin\Serverlocaltestrun.testrunconfig /TestCaseFilter:"TestCategory=MS-SAMR|TestCategory=MS-ADTS-PublishDC|TestCategory=MS-ADTS-LDAP|TestCategory=MS-ADTS-Schema|TestCategory=MS-ADTS-Security|TestCategory=MS-APDS|TestCategory=MS-DRSR|TestCategory=MS-FRS2|TestCategory=MS-LSAD|TestCategory=MS-LSAT|TestCategory=MS-NRPC" /Logger:trx
pause