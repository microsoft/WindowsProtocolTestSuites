:: Copyright (c) Microsoft. All rights reserved.
:: Licensed under the MIT license. See LICENSE file in the project root for full license information.

@echo off

set CurrentPath=%~dp0
set TestSuiteRoot=%CurrentPath%..\

call "%CurrentPath%..\common\setBuildTool.cmd"
if ErrorLevel 1 (
	exit /b 1
)

call "%CurrentPath%..\common\setVsPath.cmd"
if ErrorLevel 1 (
	exit /b 1
)

call "%CurrentPath%..\common\checkNuGet.cmd"
if ErrorLevel 1 (
	exit /b 1
)

%nuget% restore "%TestSuiteRoot%ProtocolTestManager\Kernel\Kernel.csproj" -SolutionDirectory "%TestSuiteRoot%ProtocolTestManager"
if ErrorLevel 1 (
	echo Error: Failed to restore NuGet dependencies
	exit /b 1
)

%nuget% restore "%TestSuiteRoot%ProtocolTestManager\PtmCli\PtmCli.csproj" -SolutionDirectory "%TestSuiteRoot%ProtocolTestManager"
if ErrorLevel 1 (
	echo Error: Failed to restore NuGet dependencies
	exit /b 1
)

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
