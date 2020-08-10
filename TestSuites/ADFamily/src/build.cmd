:: Copyright (c) Microsoft. All rights reserved.
:: Licensed under the MIT license. See LICENSE file in the project root for full license information.

@echo off

echo ======================================
echo          Start to build ADFamiliy
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
	echo %ErrorLevel%
	exit /b 1
)

call "%CurrentPath%..\..\..\common\checkSpecExplorer.cmd"
if ErrorLevel 1 (
	exit /b 1
)

call "%CurrentPath%..\..\..\common\checkMMA.cmd"
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
	%buildtool% "%TestSuiteRoot%TestSuites\ADFamily\src\AD_Server.sln" /t:clean;rebuild /p:ProtocolName="ADFamily" /p:Configuration="Release"
) else (
	%buildtool% "%TestSuiteRoot%TestSuites\ADFamily\src\AD_Server.sln" /t:clean;rebuild /p:AssemblyOriginatorKeyFile=%KeyFile% /p:DelaySign=true /p:SignAssembly=true /p:ProtocolName="ADFamily" /p:Configuration="Release"
)

if ErrorLevel 1 (
	echo Error: Failed to build ADFamiliy test suite
	exit /b 1
)

if exist "%TestSuiteRoot%drop\TestSuites\ADFamily" (
	rd /s /q "%TestSuiteRoot%drop\TestSuites\ADFamily"
)

%buildtool% "%TestSuiteRoot%TestSuites\ADFamily\src\deploy\deploy.wixproj" /t:Clean;Rebuild /p:ProtocolName="ADFamily" /p:Platform="x64" /p:Configuration="Release"

if ErrorLevel 1 (
	echo Error: Failed to generate the msi installer
	exit /b 1
)

echo ==========================================================
echo          Build ADFamiliy test suite successfully
echo ==========================================================
