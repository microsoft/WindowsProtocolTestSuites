:: Copyright (c) Microsoft. All rights reserved.
:: Licensed under the MIT license. See LICENSE file in the project root for full license information.

@echo off

set CurrentPath=%~dp0
set TestSuiteRoot=%CurrentPath%..\

call "%TestSuiteRoot%\common\setVsTestPath.cmd"
if ErrorLevel 1 (
	exit /b 1
)

%vstest% "%CurrentPath%\KernelTest\bin\Debug\CodeCoverage.dll" /Settings:"%CurrentPath%\testsettings1.testsettings" /Logger:trx
