:: Copyright (c) Microsoft. All rights reserved.
:: Licensed under the MIT license. See LICENSE file in the project root for full license information.

set CurrentPath=%~dp0
call "%CurrentPath%setMSTestPath.cmd"
if ErrorLevel 1 (
	exit /b 1
)

%mstest% /category:Win7_Win2K8R2 /testcontainer:..\Bin\MS-SMB_ServerTestSuite.dll /runconfig:..\Bin\ServerLocalTestRun.testrunconfig 
pause