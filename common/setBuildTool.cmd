:: Copyright (c) Microsoft. All rights reserved.
:: Licensed under the MIT license. See LICENSE file in the project root for full license information.

@echo off

:: Find installed path of Visual Studio 2017
set _currentPath=%~dp0
call "%_currentPath%setVs2017Path.cmd"
if ErrorLevel 1 (
	exit /b 1
)

set VisualStudioVer=

:: Set buildtool
if exist "%vs2017path%\MSBuild\15.0\Bin\MSBuild.exe" (
    set buildtool="%vs2017path%\MSBuild\15.0\Bin\MSBuild.exe"
    set VisualStudioVer=15.0
) else (
    if exist "%ProgramFiles(x86)%\MSBuild\14.0\Bin\MSBuild.exe" (
        set buildtool="%ProgramFiles(x86)%\MSBuild\14.0\Bin\MSBuild.exe"
        set VisualStudioVer=14.0
    ) else (
        echo Error: Microsoft Build Tools 2015 is not found. Please install Microsoft Build Tools 2015 from https://www.microsoft.com/en-us/download/details.aspx?id=48159
        exit /b 1
    )
)

exit /b 0
