:: Copyright (c) Microsoft. All rights reserved.
:: Licensed under the MIT license. See LICENSE file in the project root for full license information.

@echo off

echo ======================================
echo          Start to build ADFamiliy
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
	%buildtool% "%TestSuiteRoot%TestSuites\ADFamily\src\AD_Server.sln" /t:clean;rebuild /p:ProtocolName="ADFamily" /p:VisualStudioVersion=11.0
) else (
	%buildtool% "%TestSuiteRoot%TestSuites\ADFamily\src\AD_Server.sln" /t:clean;rebuild /p:AssemblyOriginatorKeyFile=%KeyFile% /p:DelaySign=true /p:SignAssembly=true /p:ProtocolName="ADFamily" /p:VisualStudioVersion=11.0
)

if ErrorLevel 1 (
	echo Error: Failed to build ADFamiliy test suite
	exit /b 1
)

if exist "%TestSuiteRoot%drop\TestSuites\ADFamily" (
	rd /s /q "%TestSuiteRoot%drop\TestSuites\ADFamily"
)

%buildtool% "%TestSuiteRoot%TestSuites\ADFamily\src\deploy\deploy.wixproj" /t:Clean;Rebuild /p:Platform="x64" /p:ProtocolName="ADFamily" /p:VisualStudioVersion=11.0

if ErrorLevel 1 (
	echo Error: Failed to generate the msi installer
	exit /b 1
)

echo ==========================================================
echo          Build ADFamiliy test suite successfully
echo ==========================================================
