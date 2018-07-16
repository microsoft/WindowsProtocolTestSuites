:: Copyright (c) Microsoft. All rights reserved.
:: Licensed under the MIT license. See LICENSE file in the project root for full license information.

@echo off
echo:
echo Please select test categories below
echo    MS-LDAP                          Test cases for MS-ADTS-LDAP
echo    MS-PublishDC                     Test cases for MS-ADTS-PublishDC
echo    MS-Schema                        Test cases for MS-ADTS-Schema
echo    MS-Security                      Test cases for MS-ADTS-Security
echo    MS-APDS                          Test cases for MS-APDS 
echo    MS-DRSR                          Test cases for MS-DRSR
echo    MS-FRS2                          Test cases for MS-FRS2
echo    MS-LSAD                          Test cases for MS-LSAD
echo    MS-LSAT                          Test cases for MS-LSAT
echo    MS-NRPC                          Test cases for MS-NRPC
echo    MS-SAMR                          Test cases for MS-SAMR
echo    PDC                              Test cases which need PDC
echo    SDC                              Test cases which need SDC
echo    RODC                             Test cases which need RODC
echo    CDC                              Test cases which need CDC
echo    TDC                              Test cases which need TDC
echo    DM                               Test cases which need DM
echo    DomainWin2008R2                  Test cases which need domain function level equal or higher than 2008R2
echo    DomainWin2012                    Test cases which need domain function level equal or higher than 2012
echo    DomainWin2012R2                  Test cases which need domain function level equal or higher than 2012R2
echo    DomainWinV1803                   Test cases which need domain function level equal or higher than v1803
echo:
echo To combine multiple categories, please use logical operators "|", "!" and "&":
echo Examples:
echo  MS-LSAD^&DM      Execute test cases have categories "MS-LSAD" and "DM"
echo  SDC^|RODC        Execute test cases have categories "SDC" or "RODC"
echo  PDC^&!SDC        Execute test cases have category "PDC" but do NOT have category "SDC"
echo:
set /P category=Set test categories you want to execute, default is "MS-LDAP" if nothing input:
if ("%category%") == ("") (
    set category=MS-LDAP
)
echo Test Category is "%category%"

set category="%category%"

REM Replace "!" to "#" because "!" is special char.
set category=%category:!=#%

REM Below codes translate format "BVT&FSRVP" to "TestCategory=BVT&TestCategory=FSRVP" as TestCaseFilter for VSTest.Console.exe
Setlocal ENABLEDELAYEDEXPANSION

set TestCaseFilter=
set IsCategoryChar=FALSE

:striploop  
    REM Get first char, like substring(0,1) in C#
    set stripchar=!category:~0,1!
    REM Get substring containing all chars after the first one, like substring(1) in C#
    set category=!category:~1!

    REM If last char is empty or NULL, goto ENDLOOP
    REM When last char is NULL, "set stripchar=!category:~0,1!" returns "~0,1"
    if "!stripchar!" EQU " " goto ENDLOOP
    if "!stripchar!" EQU "~0,1" goto ENDLOOP

    if '!stripchar!' EQU '^"' goto APPENDDELIMS
    if "!stripchar!" EQU "(" goto APPENDDELIMS
    if "!stripchar!" EQU ")"  goto APPENDDELIMS
    if "!stripchar!" EQU "&"  goto APPENDDELIMS
    if "!stripchar!" EQU "|"  goto APPENDDELIMS

    goto APPENDCATEGORY

REM append delims, such as "(", ")", "&", "|"
:APPENDDELIMS
    set TestCaseFilter=!TestCaseFilter!!stripchar!
    set IsCategoryChar=FALSE
    goto striploop 

REM append category
:APPENDCATEGORY
    if "!stripchar!" EQU "#" (
        set TestCaseFilter=!TestCaseFilter!TestCategory#=
        set IsCategoryChar=TRUE
        goto striploop
    )
    REM First category char
    if "!IsCategoryChar!" EQU "FALSE" (set TestCaseFilter=!TestCaseFilter!TestCategory=!stripchar!)
    if "!IsCategoryChar!" EQU "TRUE" (set TestCaseFilter=!TestCaseFilter!!stripchar!)
    set IsCategoryChar=TRUE
    goto striploop  

:ENDLOOP

setlocal disableDelayedExpansion

REM Replace "#" back to "!"
set TestCaseFilter=%TestCaseFilter:#=!%

echo TestCaseFilter is %TestCaseFilter%
echo.

REM Run test suite
set CurrentPath=%~dp0
call "%CurrentPath%setVsTestPath.cmd"
if ErrorLevel 1 (
	exit /b 1
)

%vstest% "..\bin\AD_ServerTestSuite.dll" /Settings:..\bin\Serverlocaltestrun.testrunconfig /Logger:trx /TestCaseFilter:%TestCaseFilter%

endlocal
pause