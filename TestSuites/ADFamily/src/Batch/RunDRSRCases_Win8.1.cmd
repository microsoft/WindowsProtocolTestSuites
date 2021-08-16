:: Copyright (c) Microsoft. All rights reserved.
:: Licensed under the MIT license. See LICENSE file in the project root for full license information.

@echo off

echo ==============================================
echo          Start to run DRSR Win8.1 test cases
echo ==============================================

set CurrentPath=%~dp0
call "%CurrentPath%setVsTestPath.cmd"
if ErrorLevel 1 (
	exit /b 1
)

%vstest% "..\bin\AD_ServerTestSuite.dll" /Settings:..\bin\Serverlocaltestrun.testrunconfig /TestCaseFilter:"TestCategory!=WinThreshold&TestCategory!=Winv1803&FullyQualifiedName~Microsoft.Protocols.TestSuites.ActiveDirectory.Drsr" /Logger:trx


