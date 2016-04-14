:: Copyright (c) Microsoft. All rights reserved.
:: Licensed under the MIT license. See LICENSE file in the project root for full license information.

@echo off

echo =============================================
echo          Start to build RDP Test Suite
echo =============================================

set BLDVersion=%1

if not defined BLDVersion (
	set BLDVersion=1.0.0.0
)

if not defined buildtool (
	for /f %%i in ('dir /b /ad /on "%windir%\Microsoft.NET\Framework\v4*"') do (@if exist "%windir%\Microsoft.NET\Framework\%%i\msbuild".exe set buildtool=%windir%\Microsoft.NET\Framework\%%i\msbuild.exe)
)

if not defined buildtool (
	echo Error: No msbuild.exe was found, install .Net Framework version 4.0 or higher
	goto :eof
)

if not defined WIX (
	echo Error: WiX Toolset version 3.7 or higher should be installed
	goto :eof
)

if not exist "%programfiles(x86)%\Protocol Test Framework\bin\Microsoft.Protocols.TestTools.dll" (
        echo Error: Protocol Test Framework should be installed
	goto :eof
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
		goto :eof
	)
)

set CurrentPath=%~dp0
if not defined TestSuiteRoot (
	set TestSuiteRoot=%CurrentPath%..\..\..\
)

%buildtool% "%TestSuiteRoot%TestSuites\RDP\src\RDP_Client.sln" /t:clean;rebuild
if exist "%TestSuiteRoot%drop\TestSuites\RDP" (
 rd /s /q "%TestSuiteRoot%drop\TestSuites\RDP"
)

%buildtool% "%TestSuiteRoot%TestSuites\RDP\src\deploy\deploy.wixproj" /t:Clean;Rebuild /p:BLDVersion=%BLDVersion%

