:: Copyright (c) Microsoft. All rights reserved.
:: Licensed under the MIT license. See LICENSE file in the project root for full license information.

@echo off

:: Find installed path of Visual Studio 2017
set _currentPath=%~dp0
call "%_currentPath%setVs2017Path.cmd"

:: Set VS150COMNTOOLS
if exist "%vs2017path%\Common7\Tools\" (
    set VS150COMNTOOLS="%vs2017path%\Common7\Tools\"
)

:: Set vspath
if not defined vspath (
    if defined VS150COMNTOOLS (
        set vspath=%VS150COMNTOOLS%
    ) else if defined VS140COMNTOOLS (
        set vspath="%VS140COMNTOOLS%"
    ) else if defined VS110COMNTOOLS (
        set vspath="%VS110COMNTOOLS%"
    ) else (
        echo Visual Studio or Visual Studio test agent should be installed (version 2012, 2015 or 2017)
        exit /b 1
    )
)
