:: Copyright (c) Microsoft. All rights reserved.
:: Licensed under the MIT license. See LICENSE file in the project root for full license information.

@echo off

::Get build version from AssemblyInfo
set AssemblyInfo=%TestSuiteRoot%AssemblyInfo\SharedAssemblyInfo.cs
set FindExe="%SystemRoot%\system32\findstr.exe"
set versionStr="[assembly: AssemblyVersion("1.0.0.0")]"
for /f "delims=" %%i in ('""%FindExe%" "AssemblyVersion" "%AssemblyInfo%""') do set versionStr=%%i
set TESTSUITE_VERSION=%versionStr:~28,-3%

exit /b 0
