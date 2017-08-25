:: Copyright (c) Microsoft. All rights reserved.
:: Licensed under the MIT license. See LICENSE file in the project root for full license information.

@echo off

echo ===========================================================================
echo          Start to build RDPSUTControlAgent
echo ===========================================================================

@echo off

::- Get the Java Version
set KEY="HKLM\SOFTWARE\JavaSoft\Java Development Kit"
set VALUE=CurrentVersion
reg query %KEY% /v %VALUE% 2>nul || (
    echo Error: JDK not installed 
    exit /b 1
)
set JDK_VERSION=
for /f "tokens=2,*" %%a in ('reg query %KEY% /v %VALUE% ^| findstr %VALUE%') do (
    set JDK_VERSION=%%b
)

echo JDK VERSION: %JDK_VERSION%

::- Get the JavaHome
set KEY="HKLM\SOFTWARE\JavaSoft\Java Development Kit\%JDK_VERSION%"
set VALUE=JavaHome
reg query %KEY% /v %VALUE% 2>nul || (
    echo Error: JavaHome not installed
    exit /b 1
)

set JAVAHOME=
for /f "tokens=2,*" %%a in ('reg query %KEY% /v %VALUE% ^| findstr %VALUE%') do (
    set JAVAHOME=%%b
)

set buildtool=%JAVAHOME%\bin\javac

set CurrentPath=%~dp0
set TestSuiteRoot=%CurrentPath%..\..\..\..\

echo Delete files under bin\
DEL build.txt
DEL /q "Bin\*"
FOR /D %%p IN ("Bin\*.*") DO rmdir "%%p" /s /q

for /r src %%i in (*.java) do echo %%i>>build.txt

echo Start build java files
"%buildtool%" @build.txt -d bin\

echo ===========================================================================
echo          Build RDPSUTControlAgent successfully
echo ===========================================================================