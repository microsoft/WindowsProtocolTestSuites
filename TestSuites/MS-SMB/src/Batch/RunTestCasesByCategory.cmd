:: Copyright (c) Microsoft. All rights reserved.
:: Licensed under the MIT license. See LICENSE file in the project root for full license information.

@echo off
echo:
echo Please select test categories below
echo    Traditional            All traditional test cases
echo    Win7_Win2K8R2          Model based cases that client is Win7, SUT is Win2K8R2
echo    Win7_Win2K8            Model based cases that client is Win7, SUT is Win2K8

echo: 
echo Examples:
echo  "Traditional|Win7_Win2K8R2"    Execute test cases have categories "Traditional" or "Win7_Win2K8R2"
echo:
set /P category=Set test categories you want to execute, default is "Traditional" if nothing input:
if ("%category%") == ("") (
    set category=Traditional
)
echo Test Category is "%category%"

set CurrentPath=%~dp0
call "%CurrentPath%setMSTestPath.cmd"
if ErrorLevel 1 (
	exit /b 1
)

%mstest% /category:"%category%" /testcontainer:..\Bin\MS-SMB_ServerTestSuite.dll /runconfig:..\Bin\ServerLocalTestRun.testrunconfig 
pause