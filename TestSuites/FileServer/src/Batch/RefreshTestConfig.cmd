:: Copyright (c) Microsoft. All rights reserved.
:: Licensed under the MIT license. See LICENSE file in the project root for full license information.

@echo off

set CurrentPath=%~dp0
call "%CurrentPath%setVsTestPath.cmd"
if ErrorLevel 1 (
	exit /b 1
)

echo Define the command to run file sharing test suite

if "%USERDOMAIN%" == "%COMPUTERNAME%" (
    REM workgroup environment
    set RunFileSharingTestSuite=%vstest% "..\Bin\MS-DFSC_ServerTestSuite.dll" "..\Bin\MS-SMB2_ServerTestSuite.dll" "..\Bin\MS-SMB2Model_ServerTestSuite.dll" "..\Bin\MS-FSA_ServerTestSuite.dll" /Settings:..\Bin\ServerLocalTestRun.testrunconfig /Logger:trx
) else (
    REM domain environment
    set RunFileSharingTestSuite=%vstest% "..\Bin\ServerFailoverTestSuite.dll" "..\Bin\MS-SMB2Model_ServerTestSuite.dll" "..\Bin\MS-FSRVP_ServerTestSuite.dll" "..\Bin\MS-DFSC_ServerTestSuite.dll" "..\Bin\MS-SMB2_ServerTestSuite.dll" "..\Bin\MS-RSVD_ServerTestSuite.dll" "..\Bin\MS-SQOS_ServerTestSuite.dll" "..\Bin\Auth_ServerTestSuite.dll" "..\Bin\MS-FSA_ServerTestSuite.dll" /Settings:..\Bin\ServerLocalTestRun.testrunconfig /Logger:trx
)

