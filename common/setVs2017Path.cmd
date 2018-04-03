:: Copyright (c) Microsoft. All rights reserved.
:: Licensed under the MIT license. See LICENSE file in the project root for full license information.

@echo off

:: Find vs2017path in registry
set REGEXE="%windir%\System32\reg.exe"
set KEY_NAME="HKEY_LOCAL_MACHINE\SOFTWARE\WOW6432Node\Microsoft\VisualStudio\SxS\VS7"
set VALUE_NAME=15.0
set REG_QUERY_VS_2017_PATH=%REGEXE% QUERY %KEY_NAME% /v %VALUE_NAME%

%REG_QUERY_VS_2017_PATH%
if ErrorLevel 1 (
    echo Visual Studio 2017 is not installed.
) else (
    FOR /F "usebackq tokens=1-2*" %%A IN (`"%REG_QUERY_VS_2017_PATH%"`) DO (
        set name=%%A
        set type=%%B
        set vs2017path=%%C
    )
)
