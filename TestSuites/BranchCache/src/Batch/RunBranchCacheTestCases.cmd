@echo off

:choosesut
echo Please choose SUT:
echo.
echo [1] Content Server
echo [2] Hosted Cache Server
echo.
set /p c=SUT:

if "%c%"=="1" (
    set "category=ContentServer"
    goto choosetransport 
)

if "%c%"=="2" (
    set "category=HostedCacheServer"
    goto chooseversion 
)

echo Invalid choice
goto choosesut

:choosetransport
echo Please select content information transport:
echo.
echo [1] PCCRTP
echo [2] SMB2
echo.
set /p c=Content information transport:

if "%c%"=="1" (
    set "category=%category%&PCCRTP"
    goto chooseversion 
)

if "%c%"=="2" (
    set "category=%category%&SMB2"
    goto chooseversion 
)

echo Invalid choice
goto choosetransport

:chooseversion
echo Please select supported version:
echo.
echo [1] Branch Cache V1 only
echo [2] Branch Cache V2 only
echo [3] Branch Cache V1 and V2
echo.
set /p c=Branch Cache version:

if "%c%"=="1" (
    set "category=%category%&BranchCacheV1"
    goto choosetestcase 
)

if "%c%"=="2" (
    set "category=%category%&BranchCacheV2"
    goto choosetestcase 
)

if "%c%"=="3" (
    goto choosetestcase 
)

echo Invalid choice
goto chooseversion

:choosetestcase
echo Please choose test cases to run:
echo.
echo [1] BVT only
echo [2] All test cases
echo.
set /p c=Test cases:

if "%c%"=="1" (
    set "category=%category%&BVT"
    goto runtestcase 
)

if "%c%"=="2" (
    goto runtestcase 
)

echo Invalid choice
goto choosetestcase

:runtestcase
@echo on

set CurrentPath=%~dp0
call "%CurrentPath%setMSTestPath.cmd"
if ErrorLevel 1 (
	exit /b 1
)

%mstest% /testcontainer:..\bin\BranchCache_TestSuite.dll /category:"%category%" /runconfig:..\bin\LocalTestRun.testsettings

pause
