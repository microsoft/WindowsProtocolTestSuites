:: Copyright (c) Microsoft. All rights reserved.
:: Licensed under the MIT license. See LICENSE file in the project root for full license information.

:: Get the path to MSTest.exe and set it to mstest.
:: Calls to setVsPath.cmd.

@echo off

set mstest=

set _currentPath=%~dp0
call "%_currentPath%setVsPath.cmd"
if ErrorLevel 1 (
    exit /b 1
)

if not exist "%vspath%\Common7\IDE\MSTest.exe" (
	echo Error: could not find MSTest.exe, please make sure you have installed "Testing tools core features" in Visual Studio 2017 or later.
	exit /b 1
)

set mstest="%vspath%\Common7\IDE\MSTest.exe"
exit /b 0
