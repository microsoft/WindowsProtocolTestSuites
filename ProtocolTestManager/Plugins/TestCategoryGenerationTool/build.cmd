:: Copyright (c) Microsoft. All rights reserved.
:: Licensed under the MIT license. See LICENSE file in the project root for full license information.

@echo off
echo ==============================================================
echo          Start to Build Test Category Generation Tool
echo ==============================================================

set CurrentPath=%~dp0
set TestSuiteRoot=%CurrentPath%

call "%CurrentPath%..\..\..\common\setBuildTool.cmd"
if ErrorLevel 1 (
	exit /b 1
)

%buildtool% "%TestSuiteRoot%\TestCategoryGenerationTool.csproj" /t:clean;rebuild
if ErrorLevel 1 (
	echo Error: Failed to build TestCategoryGenerationTool
	exit /b 1
)

if exist TestCategoryGenerationTool.exe (
    copy /Y TestCategoryGenerationTool.exe ..\FileServerPlugin\FileServerPlugin\
)
