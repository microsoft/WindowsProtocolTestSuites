:: Copyright (c) Microsoft. All rights reserved.
:: Licensed under the MIT license. See LICENSE file in the project root for full license information.

@echo off

:: Find installed path of Visual Studio 2017
set _currentPath=%~dp0
call "%_currentPath%setVs2017Path.cmd"

:: Set buildtool
if exist "%vs2017path%\MSBuild\15.0\Bin\MSBuild.exe" (
    set buildtool="%vs2017path%\MSBuild\15.0\Bin\MSBuild.exe"
) else if exist "%ProgramFiles(x86)%\MSBuild\14.0\Bin\MSBuild.exe" (
    set buildtool="%ProgramFiles(x86)%\MSBuild\14.0\Bin\MSBuild.exe"
) else (
    for /f %%i in ('dir /b /ad /on "%windir%\Microsoft.NET\Framework\v4*"') do (@if exist "%windir%\Microsoft.NET\Framework\%%i\msbuild".exe set buildtool=%windir%\Microsoft.NET\Framework\%%i\msbuild.exe)
)

if not defined buildtool (
    echo No msbuild.exe was found, install .Net Framework version 4.7.1 or higher
    exit /b 1
)
