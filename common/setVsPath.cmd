:: Copyright (c) Microsoft. All rights reserved.
:: Licensed under the MIT license. See LICENSE file in the project root for full license information.

::Get the root path of Visual Studio installation folder and set it to vspath.

@echo off

set vswhere=

for /d %%i in ("%ProgramFiles(x86)%","%ProgramFiles%") do (
	if exist "%%~i\Microsoft Visual Studio\Installer\vswhere.exe" (
		set "vswhere=%%~i\Microsoft Visual Studio\Installer\vswhere.exe"
		break
	)
)

if [vswhere] equ [] (
	echo Error: please make sure you have installed Visual Studio 2017 or later.
	exit /b 1
)

set vspath=

for /f "usebackq tokens=1*" %%i in (`"%vswhere%"`) do (
	if %%i equ installationPath: (
		set vspath=%%j
		exit /b 0
	)
)

echo Error: please make sure you have installed Visual Studio 2017 or later.
exit /b 1