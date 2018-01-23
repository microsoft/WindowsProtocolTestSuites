:: Copyright (c) Microsoft. All rights reserved.
:: Licensed under the MIT license. See LICENSE file in the project root for full license information.

@echo off
echo ==============================================================
echo          Start to Build Test Category Generation Tool
echo ==============================================================

if not defined buildtool (
    for /f %%i in ('dir /b /ad /on "%windir%\Microsoft.NET\Framework\v4*"') do (@if exist "%windir%\Microsoft.NET\Framework\%%i\msbuild".exe set buildtool=%windir%\Microsoft.NET\Framework\%%i\msbuild.exe)
)

if not defined buildtool (
    echo Error: No msbuild.exe was found, install .Net Framework version 4.0 or higher
    goto :eof
)

set CurrentPath=%~dp0
set TestSuiteRoot=%CurrentPath%

%buildtool% "%TestSuiteRoot%\TestCategoryGenerationTool.csproj" /t:clean;rebuild

if exist TestCategoryGenerationTool.exe (
    xcopy /Y TestCategoryGenerationTool.exe ..\FileServerPlugin\FileServerPlugin\
)
