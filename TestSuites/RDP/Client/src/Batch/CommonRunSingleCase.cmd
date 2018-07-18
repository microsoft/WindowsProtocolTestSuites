:: Copyright (c) Microsoft. All rights reserved.
:: Licensed under the MIT license. See LICENSE file in the project root for full license information.

:: This command is used to pick only one test case to run
:: Only one parameter is required to indicate the case name to be run

:: Param %1: TestCaseName

@ECHO OFF

IF [%1]==[] GOTO END

SET TestCaseName=%1

:RunCase

set CurrentPath=%~dp0
call "%CurrentPath%setVsTestPath.cmd"
if ErrorLevel 1 (
	exit /b 1
)

set RunRDPTestCase=%vstest%  "..\Bin\RDP_ClientTestSuite.dll" /Settings:..\Bin\ClientLocal.TestSettings /Logger:trx  /Tests:%1

echo %RunRDPTestCase%
%RunRDPTestCase%

exit /b
Pause 

:END
echo "No case is selected to run. Will exit the script."
Pause