:: Copyright (c) Microsoft. All rights reserved.
:: Licensed under the MIT license. See LICENSE file in the project root for full license information.

@echo off

if not defined vspath (
	if defined VS150COMNTOOLS (
		set vspath="%VS150COMNTOOLS%"
	) else if defined VS140COMNTOOLS (
		set vspath="%VS140COMNTOOLS%"
	) else if defined VS120COMNTOOLS (
		set vspath="%VS120COMNTOOLS%"
	) else (
		echo Visual Studio or Visual Studio test agent should be installed, version 2013 or higher
		exit /b 1
	)
)

%vspath%..\IDE\CommonExtensions\Microsoft\TestWindow\vstest.console.exe ".\KernelTest\bin\Debug\CodeCoverage.dll" /Settings:testsettings1.testsettings /Logger:trx