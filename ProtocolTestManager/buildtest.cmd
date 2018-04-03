:: Copyright (c) Microsoft. All rights reserved.
:: Licensed under the MIT license. See LICENSE file in the project root for full license information.

@echo off

set CurrentPath=%~dp0
set TestSuiteRoot=%CurrentPath%..\

call "%CurrentPath%..\common\setBuildTool.cmd"
call "%CurrentPath%..\common\setVsPath.cmd"

%buildtool% KernelTest\KernelTest.csproj /t:clean;rebuild /p:NoWarn=1591
if ErrorLevel 1 (
	echo Error: Failed to build KernelTest
	exit /b 1
)

%buildtool% SampleDetector\SampleDetector.csproj /t:clean;rebuild /p:NoWarn=1591
if ErrorLevel 1 (
	echo Error: Failed to build SampleDetector
	exit /b 1
)
