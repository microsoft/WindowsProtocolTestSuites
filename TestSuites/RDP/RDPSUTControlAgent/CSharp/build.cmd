:: Copyright (c) Microsoft. All rights reserved.
:: Licensed under the MIT license. See LICENSE file in the project root for full license information.

@echo off

echo ===========================================================================
echo          Start to build RDPSUTControlAgent
echo ===========================================================================

if not defined buildtool (
	for /f %%i in ('dir /b /ad /on "%windir%\Microsoft.NET\Framework\v4*"') do (@if exist "%windir%\Microsoft.NET\Framework\%%i\msbuild".exe set buildtool=%windir%\Microsoft.NET\Framework\%%i\msbuild.exe)
)

if not defined buildtool (
	echo Error: No msbuild.exe was found, install .Net Framework version 4.0 or higher
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

set CurrentPath=%~dp0
set TestSuiteRoot=%CurrentPath%..\..\..\..\

%buildtool% "%TestSuiteRoot%TestSuites\RDP\RDPSUTControlAgent\CSharp\RDPSUTControlAgent.sln" /t:clean;rebuild 

if ErrorLevel 1 (
	echo Error: Failed to build RDP Server test suite
	exit /b 1
)

echo ===========================================================================
echo          Build RDPSUTControlAgent successfully
echo ===========================================================================