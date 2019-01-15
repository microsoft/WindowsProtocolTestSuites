:: Copyright (c) Microsoft. All rights reserved.
:: Licensed under the MIT license. See LICENSE file in the project root for full license information.

:: Param %1: testCaseFilter

@ECHO OFF
FOR /F Tokens^=^3^,5Delims^=^<^"^= %%a IN (..\Bin\RDP_ClientTestSuite.deployment.ptfconfig) DO (
	IF "%%a" EQU "RDP.Version" SET RDPVersion=%%b
)
@ECHO RDPVersion=%RDPVersion%

SET TestCategorys=""

SET CommonCategorys="(TestCategory!=Interactive&TestCategory!=DeviceNeeded)&(TestCategory=RDPBCGR|TestCategory=RDPRFX|TestCategory=RDPEVOR|TestCategory=RDPEUDP|TestCategory=RDPEMT|TestCategory=RDPEGFX|TestCategory=RDPEUSB|TestCategory=RDPEDISP|(TestCategory=RDPEI&(TestCategory=BVT|TestCategory=TouchSimulated)))"

IF "%RDPVersion%" == "7.0" (
SET TestCategorys="(TestCategory=RDP7.0&(%CommonCategorys:~1,-1%))"
)

IF "%RDPVersion%" == "7.1" (
SET TestCategorys="((TestCategory=RDP7.0|TestCategory=RDP7.1)&(%CommonCategorys:~1,-1%))"
)

IF "%RDPVersion%" == "8.0" (
SET TestCategorys="((TestCategory=RDP7.0|TestCategory=RDP7.1|TestCategory=RDP8.0)&(%CommonCategorys:~1,-1%))"
)

IF "%RDPVersion%" == "8.1" (
SET TestCategorys="((TestCategory=RDP7.0|TestCategory=RDP7.1|TestCategory=RDP8.0|TestCategory=RDP8.1)&(%CommonCategorys:~1,-1%))"
)

IF "%RDPVersion%" == "10.0" (
SET TestCategorys="((TestCategory=RDP7.0|TestCategory=RDP7.1|TestCategory=RDP8.0|TestCategory=RDP8.1|TestCategory=RDP10.0)&(%CommonCategorys:~1,-1%))"
)

IF "%RDPVersion%" == "10.1" (
SET TestCategorys="((TestCategory=RDP7.0|TestCategory=RDP7.1|TestCategory=RDP8.0|TestCategory=RDP8.1|TestCategory=RDP10.0|TestCategory=RDP10.1)&(%CommonCategorys:~1,-1%))"
)

IF "%RDPVersion%" == "10.2" (
SET TestCategorys="((TestCategory=RDP7.0|TestCategory=RDP7.1|TestCategory=RDP8.0|TestCategory=RDP8.1|TestCategory=RDP10.0|TestCategory=RDP10.1|TestCategory=RDP10.2)&(%CommonCategorys:~1,-1%))"
)

IF "%RDPVersion%" == "10.3" (
SET TestCategorys="((TestCategory=RDP7.0|TestCategory=RDP7.1|TestCategory=RDP8.0|TestCategory=RDP8.1|TestCategory=RDP10.0|TestCategory=RDP10.1|TestCategory=RDP10.2|TestCategory=RDP10.3)&(%CommonCategorys:~1,-1%))"
)

IF "%RDPVersion%" == "10.4" (
SET TestCategorys="((TestCategory=RDP7.0|TestCategory=RDP7.1|TestCategory=RDP8.0|TestCategory=RDP8.1|TestCategory=RDP10.0|TestCategory=RDP10.1|TestCategory=RDP10.2|TestCategory=RDP10.3|TestCategory=RDP10.4)&(%CommonCategorys:~1,-1%))"
)

IF "%RDPVersion%" == "10.5" (
SET TestCategorys="((TestCategory=RDP7.0|TestCategory=RDP7.1|TestCategory=RDP8.0|TestCategory=RDP8.1|TestCategory=RDP10.0|TestCategory=RDP10.1|TestCategory=RDP10.2|TestCategory=RDP10.3|TestCategory=RDP10.4|TestCategory=RDP10.5)&(%CommonCategorys:~1,-1%))"
)

IF "%RDPVersion%" == "10.6" (
SET TestCategorys="(%CommonCategorys:~1,-1%)"
)

set CurrentPath=%~dp0
call "%CurrentPath%setVsTestPath.cmd"
if ErrorLevel 1 (
	exit /b 1
)

IF [%1]==[] GOTO RunCase

SET newCategory=%1
SET TestCategorys="%TestCategorys:~1,-1%&(%newCategory:~1,-1%)"

:RunCase

set RunRDPTestSuite=%vstest%  "..\Bin\RDP_ClientTestSuite.dll" /Settings:..\Bin\ClientLocal.TestSettings /TestCaseFilter:%TestCategorys%  /Logger:trx
echo %RunRDPTestSuite%