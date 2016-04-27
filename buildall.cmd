:: Copyright (c) Microsoft. All rights reserved.
:: Licensed under the MIT license. See LICENSE file in the project root for full license information.

@echo off

set TestSuiteRoot=%~dp0

if exist "%TestSuiteRoot%drop" (
 rd /s /q "%TestSuiteRoot%drop"
)

call TestSuites\FileServer\src\build.cmd
if errorlevel 1 exit /b 1

call TestSuites\MS-SMB\src\build.cmd
if errorlevel 1 exit /b 1

call TestSuites\RDP\src\build.cmd
if errorlevel 1 exit /b 1

call ProtocolTestManager\build.cmd
if errorlevel 1 exit /b 1

