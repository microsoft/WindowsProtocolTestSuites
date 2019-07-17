:: Copyright (c) Microsoft. All rights reserved.
:: Licensed under the MIT license. See LICENSE file in the project root for full license information.

:: Get the path to vstest.console.exe and set it to vstest.
:: Calls to setVsPath.cmd.

@echo off

set vstest=

set _currentPath=%~dp0
call "%_currentPath%setVsPath.cmd"
if ErrorLevel 1 (
    exit /b 1
)

if not exist "%vspath%\Common7\IDE\CommonExtensions\Microsoft\TestWindow\vstest.console.exe" (
	echo Error: could not find vstest.console.exe, please make sure you have installed "Testing tools core features" in Visual Studio 2017 or later.
	exit /b 1
)

set vstest="%vspath%\Common7\IDE\CommonExtensions\Microsoft\TestWindow\vstest.console.exe"
exit /b 0
