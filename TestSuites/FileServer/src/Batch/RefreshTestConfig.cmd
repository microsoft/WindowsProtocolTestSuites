:: Copyright (c) Microsoft. All rights reserved.
:: Licensed under the MIT license. See LICENSE file in the project root for full license information.

@echo off
echo Test config files were refreshed
echo:
echo Define the command to run file sharing test suite
if "%USERDOMAIN%" == "%COMPUTERNAME%" (
    REM workgroup environment
    set RunFileSharingTestSuite="%VS110COMNTOOLS%..\IDE\CommonExtensions\Microsoft\TestWindow\vstest.console.exe" "..\Bin\MS-DFSC_ServerTestSuite.dll" "..\Bin\MS-SMB2_ServerTestSuite.dll" "..\Bin\MS-SMB2Model_ServerTestSuite.dll" "..\Bin\MS-FSA_ServerTestSuite.dll" /Settings:..\Bin\ServerLocalTestRun.testrunconfig /Logger:trx
) else (
    REM domain environment
    set RunFileSharingTestSuite="%VS110COMNTOOLS%..\IDE\CommonExtensions\Microsoft\TestWindow\vstest.console.exe" "..\Bin\ServerFailoverTestSuite.dll" "..\Bin\MS-SMB2Model_ServerTestSuite.dll" "..\Bin\MS-FSRVP_ServerTestSuite.dll" "..\Bin\MS-DFSC_ServerTestSuite.dll" "..\Bin\MS-SMB2_ServerTestSuite.dll" "..\Bin\MS-RSVD_ServerTestSuite.dll" "..\Bin\MS-SQOS_ServerTestSuite.dll" "..\Bin\Auth_ServerTestSuite.dll" "..\Bin\MS-FSA_ServerTestSuite.dll" /Settings:..\Bin\ServerLocalTestRun.testrunconfig /Logger:trx
)

