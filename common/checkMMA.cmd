:: Copyright (c) Microsoft. All rights reserved.
:: Licensed under the MIT license. See LICENSE file in the project root for full license information.

@echo off

if not exist "%programfiles(x86)%\Microsoft Message Analyzer\MessageAnalyzer.exe" (
    if not exist "%ProgramW6432%\Microsoft Message Analyzer\MessageAnalyzer.exe" (
        echo Error: Microsoft Message Analyzer should be installed
        exit /b 1
    )
)

exit /b 0
