:: Copyright (c) Microsoft. All rights reserved.
:: Licensed under the MIT license. See LICENSE file in the project root for full license information.

@echo off

set TestSuiteRoot=%~dp0

if exist "%TestSuiteRoot%drop" (
 rd /s /q "%TestSuiteRoot%drop"
)

::Get build version from AssemblyInfo
set path=%TestSuiteRoot%AssemblyInfo\SharedAssemblyInfo.cs
set FindExe="%SystemRoot%\system32\findstr.exe"
set versionStr="[assembly: AssemblyVersion("1.0.0.0")]"
for /f "delims=" %%i in ('""%FindExe%" "AssemblyVersion" "%path%""') do set versionStr=%%i
set TESTSUITE_VERSION=%versionStr:~28,-3%

call ProtoSDK\build.cmd
call TestSuites\MS-SMB\src\build.cmd
call TestSuites\FileServer\src\build.cmd
call TestSuites\RDP\src\build.cmd
call ProtocolTestManager\build.cmd

