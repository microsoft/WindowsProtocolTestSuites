:: Copyright (c) Microsoft. All rights reserved.
:: Licensed under the MIT license. See LICENSE file in the project root for full license information.

@echo off

echo ======================================
echo          Start to build MS-SMB
echo ======================================

set CurrentPath=%~dp0
set TestSuiteRoot=%CurrentPath%..\..\..\

call "%CurrentPath%..\..\..\common\setBuildTool.cmd"
if ErrorLevel 1 (
	exit /b 1
)

call "%CurrentPath%..\..\..\common\setVsPath.cmd"
if ErrorLevel 1 (
	exit /b 1
)

call "%CurrentPath%..\..\..\common\checkWix.cmd"
if ErrorLevel 1 (
	exit /b 1
)

call "%CurrentPath%..\..\..\common\checkSpecExplorer.cmd"
if ErrorLevel 1 (
	exit /b 1
)

call "%CurrentPath%..\..\..\common\setPtfVer.cmd"
if ErrorLevel 1 (
	exit /b 1
)

call "%CurrentPath%..\..\..\common\setTestSuiteVer.cmd"
if ErrorLevel 1 (
	exit /b 1
)

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