:: Copyright (c) Microsoft. All rights reserved.
:: Licensed under the MIT license. See LICENSE file in the project root for full license information.

@echo off
if not defined buildtool (
	for /f %%i in ('dir /b /ad /on "%windir%\Microsoft.NET\Framework\v4*"') do (@if exist "%windir%\Microsoft.NET\Framework\%%i\msbuild".exe set buildtool=%windir%\Microsoft.NET\Framework\%%i\msbuild.exe)
)

if not defined buildtool (
	echo No msbuild.exe was found, install .Net Framework version 4.0 or higher
	goto :eof
)

%buildtool% KernelTest\KernelTest.csproj /t:clean;rebuild /p:NoWarn=1591
%buildtool% SampleDetector\SampleDetector.csproj /t:clean;rebuild /p:NoWarn=1591