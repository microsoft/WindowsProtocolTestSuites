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
SET TestCategorys="(%CommonCategorys:~1,-1%)"
)

if not defined vspath (
	if defined VS110COMNTOOLS (
		set vspath="%VS110COMNTOOLS%"
	) else if defined VS120COMNTOOLS (
		set vspath="%VS120COMNTOOLS%"
	) else if defined VS140COMNTOOLS (
		set vspath="%VS140COMNTOOLS%"
	) else (
		echo Error: Visual Studio or Visual Studio test agent should be installed, version 2012 or higher
		goto :eof
	)
)
IF [%1]==[] GOTO RunCase

SET newCategory=%1
SET TestCategorys="%TestCategorys:~1,-1%&(%newCategory:~1,-1%)"

:RunCase

set RunRDPTestSuite=%vspath%"..\IDE\CommonExtensions\Microsoft\TestWindow\vstest.console.exe"  "..\Bin\RDP_ClientTestSuite.dll" /Settings:..\Bin\ClientLocal.TestSettings /TestCaseFilter:%TestCategorys%  /Logger:trx
echo %RunRDPTestSuite%