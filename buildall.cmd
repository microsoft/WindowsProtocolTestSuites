:: Copyright (c) Microsoft. All rights reserved.
:: Licensed under the MIT license. See LICENSE file in the project root for full license information.

@echo off

set TestSuiteRoot=%~dp0

if exist "%TestSuiteRoot%drop" (
 rd /s /q "%TestSuiteRoot%drop"
)

call ProtoSDK\build.cmd
call TestSuites\MS-SMB\src\build.cmd
call TestSuites\FileServer\src\build.cmd
call TestSuites\MS-AZOD\src\build.cmd
call TestSuites\Kerberos\src\build.cmd
call ProtocolTestManager\build.cmd

