:: Copyright (c) Microsoft. All rights reserved.
:: Licensed under the MIT license. See LICENSE file in the project root for full license information.

@echo off

for %%f in (nuget.exe) do (
	if exist "%%~$PATH:f" (
		set nuget="%%~$PATH:f"
		exit /b 0
	)
)

echo Error: nuget.exe should be downloaded from nuget.org/downloads and placed to Path environment variable.

exit /b 1
