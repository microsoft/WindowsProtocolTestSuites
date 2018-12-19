:: Copyright (c) Microsoft. All rights reserved.
:: Licensed under the MIT license. See LICENSE file in the project root for full license information.

@echo off

echo ===========================================================================
echo          Start to build RDPSUTControlAgent
echo ===========================================================================

set CurrentPath=%~dp0
set TestSuiteRoot=%CurrentPath%..\..\..\..\

call "%CurrentPath%..\..\..\..\common\setBuildTool.cmd"
if ErrorLevel 1 (
    exit /b 1
)

call "%CurrentPath%..\..\..\..\common\setVsPath.cmd"
if ErrorLevel 1 (
    exit /b 1
)

%buildtool% "%TestSuiteRoot%TestSuites\RDP\RDPSUTControlAgent\CSharp\RDPSUTControlAgent.sln" /t:clean;rebuild 
if ErrorLevel 1 (
	echo Error: Failed to build RDP Server test suite
	exit /b 1
)

echo ===========================================================================
echo          Build RDPSUTControlAgent successfully
echo ===========================================================================
