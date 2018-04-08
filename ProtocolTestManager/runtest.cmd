:: Copyright (c) Microsoft. All rights reserved.
:: Licensed under the MIT license. See LICENSE file in the project root for full license information.

@echo off

set CurrentPath=%~dp0
set TestSuiteRoot=%CurrentPath%..\

call "%CurrentPath%..\common\setVsPath.cmd"
if ErrorLevel 1 (
	exit /b 1
)

%vspath%..\IDE\CommonExtensions\Microsoft\TestWindow\vstest.console.exe ".\KernelTest\bin\Debug\CodeCoverage.dll" /Settings:testsettings1.testsettings /Logger:trx
