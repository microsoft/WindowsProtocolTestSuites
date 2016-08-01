:: Copyright (c) Microsoft. All rights reserved.
:: Licensed under the MIT license. See LICENSE file in the project root for full license information.

@echo off

echo ====================================================
echo          Start to build Protocol Test Manager
echo ====================================================
:: build.cmd debug -- build debug version.

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

set CurrentPath=%~dp0
set TestSuiteRoot=%CurrentPath%..\


::Get build version from AssemblyInfo
set path=%TestSuiteRoot%AssemblyInfo\SharedAssemblyInfo.cs
set FindExe="%SystemRoot%\system32\findstr.exe"
set versionStr="[assembly: AssemblyVersion("1.0.0.0")]"
for /f "delims=" %%i in ('""%FindExe%" "AssemblyVersion" "%path%""') do set versionStr=%%i
set TESTSUITE_VERSION=%versionStr:~28,-3%

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

%buildtool% "%TestSuiteRoot%ProtocolTestManager\deploy\ProtocolTestManagerInstaller.wixproj" /t:clean;Rebuild /p:NoWarn=1591 /p:FORDEBUG=%DEBUGVER%
