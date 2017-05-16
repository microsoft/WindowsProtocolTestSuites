:: Copyright (c) Microsoft. All rights reserved.
:: Licensed under the MIT license. See LICENSE file in the project root for full license information.

@echo off

echo ==========================================
echo          Start to build MS-SMBD test suite
echo ==========================================

if not defined buildtool (
	for /f %%i in ('dir /b /ad /on "%windir%\Microsoft.NET\Framework\v4*"') do (@if exist "%windir%\Microsoft.NET\Framework\%%i\msbuild".exe set buildtool=%windir%\Microsoft.NET\Framework\%%i\msbuild.exe)
)

if not defined buildtool (
	echo Error: No msbuild.exe was found, install .Net Framework version 4.0 or higher
	exit /b 1
)

if not defined WIX (
	echo Error: WiX Toolset version 3.7 or higher should be installed
	exit /b 1
)

if not exist "%programfiles(x86)%\Spec Explorer 2010\SpecExplorer.exe" (
    if not exist "%programfiles%\Spec Explorer 2010\SpecExplorer.exe" (
		echo Error: Spec Explorer 2010 v3.5.3146.0 should be installed
		exit /b 1
	)
)

if not defined vspath (
	if defined VS110COMNTOOLS (
		set vspath="%VS110COMNTOOLS%"
	) else if defined VS120COMNTOOLS (
		set vspath="%VS120COMNTOOLS%"
	) else if defined VS140COMNTOOLS (
		set vspath="%VS140COMNTOOLS%"
	) else (
		echo Error: Visual Studio or Visual Studio test agent should be installed, version 2012 or higher
		exit /b 1
	)
)
 
:: Set path of Reg.exe
set REGEXE="%SystemRoot%\System32\REG.exe"

:: Try get PTF_VERSION from registry under Wow6432Node, this is for 64-bit OS
%REGEXE% QUERY HKEY_LOCAL_MACHINE\SOFTWARE\Wow6432Node\Microsoft\ProtocolTestFramework /v PTFVersion
if ErrorLevel 1 (
	:: If not found, try searching the other path, this is for 32-bit OS
	%REGEXE% QUERY HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\ProtocolTestFramework /v PTFVersion
	if ErrorLevel 1 (
	    :: If not found in two paths
		echo Error: Protocol Test Framework should be installed
		exit /b 1		
	) else (
	    :: If found in 32-bit OS
		FOR /F "usebackq tokens=3" %%A IN (`%REGEXE% QUERY HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\ProtocolTestFramework /v PTFVersion`) DO (
			set PTF_VERSION=%%A
		)
	)	
) else (
    :: If found in 64-bit OS
	FOR /F "usebackq tokens=3" %%A IN (`%REGEXE% QUERY HKEY_LOCAL_MACHINE\SOFTWARE\Wow6432Node\Microsoft\ProtocolTestFramework /v PTFVersion`) DO (
		set PTF_VERSION=%%A
	)
)

set CurrentPath=%~dp0
if not defined TestSuiteRoot (
	set TestSuiteRoot=%CurrentPath%..\..\..\
)

if not exist "%TestSuiteRoot%ProtoSDK\RDMA\include\ndspi.h" (
	echo Error: WindowsProtocolTestSuites\ProtoSDK\RDMA\include\ndspi.h does not exist, it can be extracted from NetworkDirect_DDK.zip in HPC Pack 2008 R2 SDK @ http://www.microsoft.com/en-us/download/details.aspx?id=12218
	exit /b 1
) 

if not exist "%TestSuiteRoot%ProtoSDK\RDMA\include\ndstatus.h" (
	echo Warning: WindowsProtocolTestSuites\ProtoSDK\RDMA\include\ndstatus.h does not exist, it can be extracted from NetworkDirect_DDK.zip in HPC Pack 2008 R2 SDK @ http://www.microsoft.com/en-us/download/details.aspx?id=12218
	exit /b 1
) 

::Get build version from AssemblyInfo
set path=%TestSuiteRoot%AssemblyInfo\SharedAssemblyInfo.cs
set FindExe="%SystemRoot%\system32\findstr.exe"
set versionStr="[assembly: AssemblyVersion("1.0.0.0")]"
for /f "delims=" %%i in ('""%FindExe%" "AssemblyVersion" "%path%""') do set versionStr=%%i
set TESTSUITE_VERSION=%versionStr:~28,-3%

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
