:: Copyright (c) Microsoft. All rights reserved.
:: Licensed under the MIT license. See LICENSE file in the project root for full license information.

@echo off

echo ======================================
echo          Start to build Kerberos
echo ======================================

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
	echo Error: Spec Explorer 2010 v3.5.3146.0 should be installed
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
C:\Windows\System32\REG.exe QUERY HKEY_LOCAL_MACHINE\SOFTWARE\Wow6432Node\Microsoft\ProtocolTestFramework > NUL
IF NOT ERRORLEVEL 1 (
	set KEY_NAME="HKEY_LOCAL_MACHINE\SOFTWARE\Wow6432Node\Microsoft\ProtocolTestFramework"
	FOR /F "usebackq skip=2 tokens=1-3" %%A IN (`C:\Windows\System32\REG.exe QUERY %KEY_NAME% /v %VALUE_NAME% 2^>nul`) DO (
		set ValueName=%%A
		set ValueValue=%%C
	)
) ELSE (
	set KEY_NAME="HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\ProtocolTestFramework"
	FOR /F "usebackq skip=2 tokens=1-3" %%A IN (`C:\Windows\System32\REG.exe QUERY %KEY_NAME% /v %VALUE_NAME% 2^>nul`) DO (
		set ValueName=%%A
		set ValueValue=%%C
	)
)

if defined ValueName (
	set PTF_VERSION=%ValueValue%
) else (
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
	%buildtool% "%TestSuiteRoot%TestSuites\Kerberos\src\Kerberos_Server.sln" /t:clean;rebuild 
) else (
	%buildtool% "%TestSuiteRoot%TestSuites\Kerberos\src\Kerberos_Server.sln" /t:clean;rebuild /p:AssemblyOriginatorKeyFile=%KeyFile% /p:DelaySign=true /p:SignAssembly=true	
)

if exist "%TestSuiteRoot%drop\TestSuites\Kerberos" (
 rd /s /q "%TestSuiteRoot%drop\TestSuites\Kerberos"
)

%buildtool% "%TestSuiteRoot%TestSuites\Kerberos\src\deploy\deploy.wixproj" /t:Clean;Rebuild

echo ==============================================
echo          Build Kerberos test suite successfully
echo ==============================================