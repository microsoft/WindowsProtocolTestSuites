:: Copyright (c) Microsoft. All rights reserved.
:: Licensed under the MIT license. See LICENSE file in the project root for full license information.

@echo off

echo =============================================
echo          Start to build RDP Test Suite
echo =============================================

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

if not exist "%programfiles(x86)%\Protocol Test Framework\bin\Microsoft.Protocols.TestTools.dll" (
        echo Error: Protocol Test Framework should be installed
	exit /b 1
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

:: Get PTF version
set REGEXE="%SystemRoot%\System32\REG.exe"
set PTF_VERSION=
:: Try get PTF_VERSION from registry under Wow6432Node
FOR /F "usebackq tokens=3" %%A IN (`%REGEXE% QUERY HKEY_LOCAL_MACHINE\SOFTWARE\Wow6432Node\Microsoft\ProtocolTestFramework /v PTFVersion`) DO (
	set PTF_VERSION=%%A
)
:: not found in Wow6432
if not defined PTF_VERSION (
	FOR /F "usebackq tokens=3" %%A IN (`%REGEXE% QUERY HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\ProtocolTestFramework /v PTFVersion`) DO (
		set PTF_VERSION=%%A
	)
)
:: check if get PTF_VERSION
if not defined PTF_VERSION (
	:: set envrionment value incase build deploy.wixproj failed
	set PTF_VERSION="0.0" 
    echo Warning: Windows Protocol Test Framework Should be installed.
)

set CurrentPath=%~dp0
set TestSuiteRoot=%CurrentPath%..\..\..\

::Get build version from AssemblyInfo
set path=%TestSuiteRoot%AssemblyInfo\SharedAssemblyInfo.cs
set FindExe="%SystemRoot%\system32\findstr.exe"
set versionStr="[assembly: AssemblyVersion("1.0.0.0")]"
for /f "delims=" %%i in ('""%FindExe%" "AssemblyVersion" "%path%""') do set versionStr=%%i
set TESTSUITE_VERSION=%versionStr:~28,-3%

set KeyFile=%1
if not defined KeyFile (
	%buildtool% "%TestSuiteRoot%TestSuites\RDP\src\RDP_Client.sln" /t:clean;rebuild 
) else (
	%buildtool% "%TestSuiteRoot%TestSuites\RDP\src\RDP_Client.sln" /t:clean;rebuild /p:AssemblyOriginatorKeyFile=%KeyFile% /p:DelaySign=true /p:SignAssembly=true	
)

if exist "%TestSuiteRoot%drop\TestSuites\RDP" (
 rd /s /q "%TestSuiteRoot%drop\TestSuites\RDP"
)

%buildtool% "%TestSuiteRoot%TestSuites\RDP\src\deploy\deploy.wixproj" /t:Clean;Rebuild

echo ==============================================
echo          Build RDP test suite successfully
echo ==============================================