:: Copyright (c) Microsoft. All rights reserved.
:: Licensed under the MIT license. See LICENSE file in the project root for full license information.

:: Get the path to MSBuild.exe and set it to buildtool.
:: Calls to setVsPath.cmd.

@echo off

set buildtool=

set _currentPath=%~dp0
call "%_currentPath%setVsPath.cmd"
if ErrorLevel 1 (
    exit /b 1
)

if not exist "%vspath%\MSBuild" (
	echo Error: could not find MSBuild.exe, please make sure you have installed "C# and Visual Basic Roslyn compilers" in Visual Studio 2017 or later.
	exit /b 1
)

for /d %%i in ("%vspath%\MSBuild\*") do (
	if exist "%%~fi\Bin\MSBuild.exe" (
		set buildtool="%%~fi\Bin\MSBuild.exe"
		exit /b 0
	)
)

echo Error: could not find MSBuild.exe, please make sure you have installed "C# and Visual Basic Roslyn compilers" in Visual Studio 2017 or later.
exit /b 1