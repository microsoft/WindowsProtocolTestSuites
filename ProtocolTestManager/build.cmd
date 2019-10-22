:: Copyright (c) Microsoft. All rights reserved.
:: Licensed under the MIT license. See LICENSE file in the project root for full license information.

@echo off

echo ====================================================
echo          Start to build Protocol Test Manager
echo ====================================================
:: build.cmd debug -- build debug version.

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

call "%CurrentPath%..\common\checkWix.cmd"
if ErrorLevel 1 (
	exit /b 1
)

call "%CurrentPath%..\common\checkNuGet.cmd"
if ErrorLevel 1 (
	exit /b 1
)

if not exist "%TestSuiteRoot%ProtoSDK\RDMA\include\ndspi.h" (
	echo Error: WindowsProtocolTestSuites\ProtoSDK\RDMA\include\ndspi.h does not exist, it can be extracted from NetworkDirect_DDK.zip in HPC Pack 2008 R2 SDK with Service Pack 2  @ https://www.microsoft.com/en-us/download/details.aspx?id=26645
	exit /b 1
) 

if not exist "%TestSuiteRoot%ProtoSDK\RDMA\include\ndstatus.h" (
	echo Warning: WindowsProtocolTestSuites\ProtoSDK\RDMA\include\ndstatus.h does not exist, it can be extracted from NetworkDirect_DDK.zip in HPC Pack 2008 R2 SDK with Service Pack 2  @ https://www.microsoft.com/en-us/download/details.aspx?id=26645
	exit /b 1
) 

call "%CurrentPath%..\common\setTestSuiteVer.cmd"
if ErrorLevel 1 (
	exit /b 1
)

if /i "%~1"=="debug" (
:: build debug version
	set DEBUGVER=1  
	echo Build Debug Version...
) else (
	set DEBUGVER=0
)

if exist "%TestSuiteRoot%drop\ProtocolTestManager" (
 rd /s /q "%TestSuiteRoot%drop\ProtocolTestManager"
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

%buildtool% "%TestSuiteRoot%ProtocolTestManager\deploy\ProtocolTestManagerInstaller.wixproj" /p:Platform="x64" /p:Configuration="Release" /t:clean;Rebuild /p:NoWarn=1591 /p:FORDEBUG=%DEBUGVER%
if ErrorLevel 1 (
	echo Error: Failed to build Protocol Test Manager
	exit /b 1
)
