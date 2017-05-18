:: Copyright (c) Microsoft. All rights reserved.
:: Licensed under the MIT license. See LICENSE file in the project root for full license information.

@ECHO OFF
FOR /F Tokens^=^3^,5Delims^=^<^"^= %%a IN (..\Bin\RDP_ClientTestSuite.deployment.ptfconfig) DO (
	IF "%%a" EQU "RDP.Version" SET RDPVersion=%%b
)
@ECHO RDPVersion=%RDPVersion%

SET TestCategorys=""

IF "%RDPVersion%" == "7.0" (
SET TestCategorys="(TestCategory=RDP7.0&TestCategory=BVT&TestCategory!=Interactive&TestCategory!=DeviceNeeded)&(TestCategory=RDPBCGR|TestCategory=RDPRFX|TestCategory=RDPEVOR|TestCategory=RDPEUDP|TestCategory=RDPEMT|TestCategory=RDPEGFX|TestCategory=RDPEDISP)"
)

IF "%RDPVersion%" == "7.1" (
SET TestCategorys="((TestCategory=RDP7.0|TestCategory=RDP7.1)&TestCategory=BVT&TestCategory!=Interactive&TestCategory!=DeviceNeeded)&(TestCategory=RDPBCGR|TestCategory=RDPRFX|TestCategory=RDPEVOR|TestCategory=RDPEUDP|TestCategory=RDPEMT|TestCategory=RDPEGFX|TestCategory=RDPEDISP)"
)

IF "%RDPVersion%" == "8.0" (
SET TestCategorys="((TestCategory=RDP7.0|TestCategory=RDP7.1|TestCategory=RDP8.0)&TestCategory=BVT&TestCategory!=Interactive&TestCategory!=DeviceNeeded)&(TestCategory=RDPBCGR|TestCategory=RDPRFX|TestCategory=RDPEVOR|TestCategory=RDPEUDP|TestCategory=RDPEMT|TestCategory=RDPEGFX|TestCategory=RDPEDISP)"
)

IF "%RDPVersion%" == "8.1" (
SET TestCategorys="((TestCategory=RDP7.0|TestCategory=RDP7.1|TestCategory=RDP8.0|TestCategory=RDP8.1)&TestCategory=BVT&TestCategory!=Interactive&TestCategory!=DeviceNeeded)&(TestCategory=RDPBCGR|TestCategory=RDPRFX|TestCategory=RDPEVOR|TestCategory=RDPEUDP|TestCategory=RDPEMT|TestCategory=RDPEGFX|TestCategory=RDPEDISP)"
)

IF "%RDPVersion%" == "10.0" (
SET TestCategorys="((TestCategory=RDP7.0|TestCategory=RDP7.1|TestCategory=RDP8.0|TestCategory=RDP8.1|TestCategory=RDP10.0)&TestCategory=BVT&TestCategory!=Interactive&TestCategory!=DeviceNeeded)&(TestCategory=RDPBCGR|TestCategory=RDPRFX|TestCategory=RDPEVOR|TestCategory=RDPEUDP|TestCategory=RDPEMT|TestCategory=RDPEGFX|TestCategory=RDPEDISP)"
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

.\CommonRunTestCase.cmd  "..\Bin\RDP_ClientTestSuite.dll" /Settings:..\Bin\ClientLocal.TestSettings /TestCaseFilter:%TestCategorys% 

IF NOT "%RDPVersion%" == "7.0" IF NOT "%RDPVersion%" == "7.1" IF NOT "%RDPVersion%" == "8.0" IF NOT "%RDPVersion%" == "8.1" IF NOT "%RDPVersion%" == "10.0" (
@ECHO ERROR: The RDP Version is not configured correctly.
)

pause

