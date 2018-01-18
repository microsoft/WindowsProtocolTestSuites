:: Copyright (c) Microsoft. All rights reserved.
:: Licensed under the MIT license. See LICENSE file in the project root for full license information.

:: This command is used to pick only one test case to run
:: Only one parameter is required to indicate the case name to be run

:: Param %1: TestCaseName

@ECHO OFF

IF [%1]==[] GOTO END

SET TestCaseName=%1

:RunCase

IF NOT defined vspath (
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

set RunRDPTestCase=%vspath%"..\IDE\CommonExtensions\Microsoft\TestWindow\vstest.console.exe"  "..\Bin\RDP_ClientTestSuite.dll" /Settings:..\Bin\ClientLocal.TestSettings /Logger:trx  /Tests:%1

echo %RunRDPTestCase%
%RunRDPTestCase%

exit /b
Pause 

:END
echo "No case is selected to run. Will exit the script."
Pause