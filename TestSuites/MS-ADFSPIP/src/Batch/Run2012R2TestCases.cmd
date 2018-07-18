:: Copyright (c) Microsoft. All rights reserved.
:: Licensed under the MIT license. See LICENSE file in the project root for full license information.

@echo off

echo ==============================================
echo          Start to run ADFSPIP 2012R2 test cases
echo ==============================================

set CurrentPath=%~dp0
call "%CurrentPath%setMSTestPath.cmd"
if ErrorLevel 1 (
	exit /b 1
)

%mstest% /testcontainer:..\Bin\MS-ADFSPIP_ClientTestSuite.dll /runconfig:..\Bin\ClientLocal.TestSettings /category:"!Disabled&!Win2016"
pause
