:: Copyright (c) Microsoft. All rights reserved.
:: Licensed under the MIT license. See LICENSE file in the project root for full license information.

@echo off

:: Set path of Reg.exe
set REGEXE="%SystemRoot%\System32\REG.exe"

:: Try get PTF_VERSION from registry under Wow6432Node, this is for 64-bit OS
%REGEXE% QUERY HKEY_LOCAL_MACHINE\SOFTWARE\Wow6432Node\Microsoft\ProtocolTestFramework /v PTFVersion
if ErrorLevel 1 (
	:: If not found, try searching the other path, this is for 32-bit OS
	%REGEXE% QUERY HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\ProtocolTestFramework /v PTFVersion
	if ErrorLevel 1 (
	    :: If not found in two paths
		echo Error: Protocol Test Framework should be installed
		exit /b 1
	) else (
	    :: If found in 32-bit OS
		FOR /F "usebackq tokens=3" %%A IN (`%REGEXE% QUERY HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\ProtocolTestFramework /v PTFVersion`) DO (
			set PTF_VERSION=%%A
		)
	)
) else (
    :: If found in 64-bit OS
	FOR /F "usebackq tokens=3" %%A IN (`%REGEXE% QUERY HKEY_LOCAL_MACHINE\SOFTWARE\Wow6432Node\Microsoft\ProtocolTestFramework /v PTFVersion`) DO (
		set PTF_VERSION=%%A
	)
)

exit /b 0
