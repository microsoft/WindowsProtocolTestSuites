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
if exist "%vs2017path%\MSBuild\15.0\Bin\MSBuild.exe" (
    set buildtool="%vs2017path%\MSBuild\15.0\Bin\MSBuild.exe"
)

if not defined buildtool (
    echo msbuild.exe cannot be found. Please install visual studio 2017.
    exit /b 1
)

if exist "%vs2017path%" (
    if not exist "%vs2017path%\MSBuild\Microsoft\WiX\v3.x\Wix.targets" (
        echo Error: "%vs2017path%\MSBuild\Microsoft\WiX\v3.x\Wix.targets" cannot be found. Please install Wix Toolset Visual Studio 2017 Extension from https://marketplace.visualstudio.com/items?itemName=RobMensching.WixToolsetVisualStudio2017Extension
        exit /b 1
    ) else (
        exit /b 0
    )
)

exit /b 0
