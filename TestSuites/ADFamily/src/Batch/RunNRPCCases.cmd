:: Copyright (c) Microsoft. All rights reserved.
:: Licensed under the MIT license. See LICENSE file in the project root for full license information.

@echo off

echo ==============================================
echo          Start to run NRPC test cases
echo ==============================================

if not defined vspath (
	if defined VS110COMNTOOLS (
		set vspath="%VS110COMNTOOLS%"
	) else if defined VS120COMNTOOLS (
		set vspath="%VS120COMNTOOLS%"
	) else if defined VS140COMNTOOLS (
		set vspath="%VS140COMNTOOLS%"
	) else (
		echo Error: Visual Studio or Visual Studio test agent should be installed, version 2012 or higher
		goto :eof
	)
)

%vspath%"..\IDE\CommonExtensions\Microsoft\TestWindow\vstest.console.exe" "..\bin\AD_ServerTestSuite.dll" /Settings:..\bin\Serverlocaltestrun.testrunconfig /Tests:"Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc" /Logger:trx
pause