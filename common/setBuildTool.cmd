:: Copyright (c) Microsoft. All rights reserved.
:: Licensed under the MIT license. See LICENSE file in the project root for full license information.

@echo off

:: Find installed path of Visual Studio 2017
set _currentPath=%~dp0
call "%_currentPath%setVs2017Path.cmd"
if ErrorLevel 1 (
    exit /b 1
)

:: Set buildtool
if not defined buildtool (
    if exist "%vs2017path%\MSBuild\15.0\Bin\MSBuild.exe" (
        set buildtool="%vs2017path%\MSBuild\15.0\Bin\MSBuild.exe"
        set VisualStudioVer=15.0
    ) else (
        if exist "%ProgramFiles(x86)%\MSBuild\14.0\Bin\MSBuild.exe" (
            set buildtool="%ProgramFiles(x86)%\MSBuild\14.0\Bin\MSBuild.exe"
            set VisualStudioVer=14.0
        ) else (
            if exist "%ProgramFiles(x86)%\MSBuild\12.0\Bin\MSBuild.exe" (
                set buildtool="%ProgramFiles(x86)%\MSBuild\12.0\Bin\MSBuild.exe"
                set VisualStudioVer=12.0
            ) else (
                for /f %%i in ('dir /b /ad /on "%windir%\Microsoft.NET\Framework\v4*"') do (@if exist "%windir%\Microsoft.NET\Framework\%%i\msbuild".exe set buildtool=%windir%\Microsoft.NET\Framework\%%i\msbuild.exe)
                set VisualStudioVer=11.0
            )
        )
    )
)

if not defined buildtool (
    echo No msbuild.exe was found. Please install visual studio 2012, 2013, 2015 or 2017.
    exit /b 1
)

exit /b 0
