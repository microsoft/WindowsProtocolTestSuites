:: Copyright (c) Microsoft. All rights reserved.
:: Licensed under the MIT license. See LICENSE file in the project root for full license information.

@echo off

set _currentPath=%~dp0
call "%_currentPath%setVsPath.cmd"
if ErrorLevel 1 (
    exit /b 1
)

if not defined vstest (
    if exist "%vs2017path%Common7\IDE\CommonExtensions\Microsoft\TestWindow\vstest.console.exe" (
        set vstest="%vs2017path%Common7\IDE\CommonExtensions\Microsoft\TestWindow\vstest.console.exe"
        exit /b 0
    ) else (
        echo Error: Visual Studio or Visual Studio test agent should be installed (version 2017)
        exit /b 1
    )
)

exit /b 0
