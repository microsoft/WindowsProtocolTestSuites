:: Copyright (c) Microsoft. All rights reserved.
:: Licensed under the MIT license. See LICENSE file in the project root for full license information.

@echo off

set TestSuiteRoot=%~dp0
set BLDVersion=%1

if not defined BLDVersion (
	set BLDVersion=1.0.0.0
)

if exist "%TestSuiteRoot%drop" (
 rd /s /q "%TestSuiteRoot%drop"
)

call ProtoSDK\build.cmd
call TestSuites\MS-SMB\src\build.cmd %BLDVersion%
call TestSuites\FileServer\src\build.cmd %BLDVersion%
call TestSuites\RDP\src\build.cmd %BLDVersion%
call ProtocolTestManager\build.cmd %BLDVersion%

