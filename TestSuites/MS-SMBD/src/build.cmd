:: Copyright (c) Microsoft. All rights reserved.
:: Licensed under the MIT license. See LICENSE file in the project root for full license information.

@echo off

echo ==========================================
echo          Start to build MS-SMBD test suite
echo ==========================================

set CurrentPath=%~dp0
if not defined TestSuiteRoot (
	set TestSuiteRoot=%CurrentPath%..\..\..\
)

call "%CurrentPath%..\..\..\common\setBuildTool.cmd"
call "%CurrentPath%..\..\..\common\setVsPath.cmd"
call "%CurrentPath%..\..\..\common\checkWix.cmd"
call "%CurrentPath%..\..\..\common\checkSpecExplorer.cmd"
call "%CurrentPath%..\..\..\common\setPtfVer.cmd"
call "%CurrentPath%..\..\..\common\setTestSuiteVer.cmd"

if not exist "%TestSuiteRoot%ProtoSDK\RDMA\include\ndspi.h" (
	echo Error: WindowsProtocolTestSuites\ProtoSDK\RDMA\include\ndspi.h does not exist, it can be extracted from NetworkDirect_DDK.zip in HPC Pack 2008 R2 SDK @ http://www.microsoft.com/en-us/download/details.aspx?id=12218
	exit /b 1
) 

if not exist "%TestSuiteRoot%ProtoSDK\RDMA\include\ndstatus.h" (
	echo Warning: WindowsProtocolTestSuites\ProtoSDK\RDMA\include\ndstatus.h does not exist, it can be extracted from NetworkDirect_DDK.zip in HPC Pack 2008 R2 SDK @ http://www.microsoft.com/en-us/download/details.aspx?id=12218
	exit /b 1
) 

set KeyFile=%1
if not defined KeyFile (
	%buildtool% "%TestSuiteRoot%TestSuites\MS-SMBD\src\MS-SMBD_Server.sln" /t:clean;rebuild /p:VisualStudioVersion=12.0
) else (
	%buildtool% "%TestSuiteRoot%TestSuites\MS-SMBD\src\MS-SMBD_Server.sln" /t:clean;rebuild /p:AssemblyOriginatorKeyFile=%KeyFile% /p:DelaySign=true /p:SignAssembly=true /p:VisualStudioVersion=12.0
)

if ErrorLevel 1 (
	echo Error: Failed to build MS-SMBD test suite
	exit /b 1
)

if exist "%TestSuiteRoot%drop\TestSuites\MS-SMBD" (
	rd /s /q "%TestSuiteRoot%drop\TestSuites\MS-SMBD"
)

%buildtool% "%TestSuiteRoot%TestSuites\MS-SMBD\src\deploy\deploy.wixproj" /t:Clean;Rebuild /p:Platform="x64" /p:Configuration="Release" /p:VisualStudioVersion=12.0

if ErrorLevel 1 (
	echo Error: Failed to generate the msi installer
	exit /b 1
)

echo ==================================================
echo          Build MS-SMBD test suite successfully
echo ==================================================
