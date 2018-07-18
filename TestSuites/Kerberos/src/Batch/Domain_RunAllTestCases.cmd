:: Copyright (c) Microsoft. All rights reserved.
:: Licensed under the MIT license. See LICENSE file in the project root for full license information.

@echo off

echo ==============================================
echo          Start to run Kerberos all test cases
echo ==============================================

set CurrentPath=%~dp0
call "%CurrentPath%setVsTestPath.cmd"
if ErrorLevel 1 (
	exit /b 1
)

%vstest% "..\Bin\Kerberos_ServerTestSuite.dll" /Settings:..\Bin\ServerLocalTestRun.testrunconfig /Logger:trx
pause
powershell ..\Scripts\Modify-ConfigFileNodeWithGroup.ps1 ..\Bin\Kerberos_ServerTestSuite.deployment.ptfconfig UseProxy true
pause
%vstest% "..\Bin\Kerberos_ServerTestSuite.dll" /Settings:..\Bin\ServerLocalTestRun.testrunconfig /Logger:trx
pause