:: Copyright (c) Microsoft. All rights reserved.
:: Licensed under the MIT license. See LICENSE file in the project root for full license information.

@echo off

echo ======================================
echo          Start to build MS-SMB
echo ======================================

set CurrentPath=%~dp0
set TestSuiteRoot=%CurrentPath%..\..\..\

call "%CurrentPath%..\..\..\common\setBuildTool.cmd"
call "%CurrentPath%..\..\..\common\setVsPath.cmd"
call "%CurrentPath%..\..\..\common\checkWix.cmd"
call "%CurrentPath%..\..\..\common\checkSpecExplorer.cmd"
call "%CurrentPath%..\..\..\common\setPtfVer.cmd"
call "%CurrentPath%..\..\..\common\setTestSuiteVer.cmd"

set KeyFile=%1
if not defined KeyFile (
	%buildtool% "%TestSuiteRoot%TestSuites\MS-SMB\src\MS-SMB_Server.sln" /t:clean;rebuild 
) else (
	%buildtool% "%TestSuiteRoot%TestSuites\MS-SMB\src\MS-SMB_Server.sln" /t:clean;rebuild /p:AssemblyOriginatorKeyFile=%KeyFile% /p:DelaySign=true /p:SignAssembly=true	
)

if ErrorLevel 1 (
	echo Error: Failed to build MS-SMB test suite
	exit /b 1
)

if exist "%TestSuiteRoot%drop\TestSuites\MS-SMB" (
	rd /s /q "%TestSuiteRoot%drop\TestSuites\MS-SMB" 
)

%buildtool% "%TestSuiteRoot%TestSuites\MS-SMB\src\deploy\deploy.wixproj" /t:Clean;Rebuild

if ErrorLevel 1 (
	echo Error: Failed to generate the msi installer
	exit /b 1
)

echo ==============================================
echo          Build MS-SMB test suite successfully
echo ==============================================