:: Copyright (c) Microsoft. All rights reserved.
:: Licensed under the MIT license. See LICENSE file in the project root for full license information.

set CurrentPath=%~dp0
call "%CurrentPath%setVsTestPath.cmd"
if ErrorLevel 1 (
	exit /b 1
)

%vstest% "..\Bin\RDP_ServerTestSuite.dll" /Settings:..\Bin\ServerLocal.testsettings /Logger:trx
pause