:: Copyright (c) Microsoft. All rights reserved.
:: Licensed under the MIT license. See LICENSE file in the project root for full license information.

@echo off
set CurrentPath=%~dp0
call "%CurrentPath%setVsTestPath.cmd"
if ErrorLevel 1 (
	@pause
	exit /b 1
)

call "%CurrentPath%CheckAdminPrivilege.cmd"
if ErrorLevel 1 (
	@pause
	exit /b 1
)

%vstest% "%CurrentPath%..\Bin\MS-SMBD_ServerTestSuite.dll" /Settings:"%CurrentPath%..\Bin\ServerLocal.TestSettings" /Logger:trx
pause