:: Copyright (c) Microsoft. All rights reserved.
:: Licensed under the MIT license. See LICENSE file in the project root for full license information.

@echo off

echo ======================================
echo        Start to build ProtoSDK
echo ======================================

if not defined buildtool (
	for /f %%i in ('dir /b /ad /on "%windir%\Microsoft.NET\Framework\v4*"') do (@if exist "%windir%\Microsoft.NET\Framework\%%i\msbuild".exe set buildtool=%windir%\Microsoft.NET\Framework\%%i\msbuild.exe)
)

if not defined buildtool (
	echo No msbuild.exe was found, install .Net Framework version 4.0 or higher
	goto :eof
)

set CurrentPath=%~dp0
set TestSuiteRoot="%CurrentPath%..\"


%buildtool% "%TestSuiteRoot%ProtoSDK\Asn1Base\Asn1Base.csproj" /t:Clean;Rebuild
%buildtool% "%TestSuiteRoot%ProtoSDK\Messages\Messages.csproj" /t:Clean;Rebuild
%buildtool% "%TestSuiteRoot%ProtoSDK\Common\Common.csproj" /t:Clean;Rebuild
%buildtool% "%TestSuiteRoot%ProtoSDK\Claim\Claim.csproj" /t:Clean;Rebuild
%buildtool% "%TestSuiteRoot%ProtoSDK\CryptoLib\CryptoLib.csproj" /t:Clean;Rebuild
%buildtool% "%TestSuiteRoot%ProtoSDK\Sspi\Sspi.csproj" /t:Clean;Rebuild
%buildtool% "%TestSuiteRoot%ProtoSDK\MS-NLMP\Nlmp.csproj" /t:Clean;Rebuild
%buildtool% "%TestSuiteRoot%ProtoSDK\TransportStack\TransportStack.csproj" /t:Clean;Rebuild
%buildtool% "%TestSuiteRoot%ProtoSDK\MS-PAC\Pac.csproj" /t:Clean;Rebuild
%buildtool% "%TestSuiteRoot%ProtoSDK\FileAccessService\FileAccessService.csproj" /t:Clean;Rebuild
%buildtool% "%TestSuiteRoot%ProtoSDK\KerberosLib\KerberosLib.csproj" /t:Clean;Rebuild

%buildtool% "%TestSuiteRoot%ProtoSDK\MS-FSCC\Fscc.csproj" /t:Clean;Rebuild
%buildtool% "%TestSuiteRoot%ProtoSDK\MS-CIFS\Cifs.csproj" /t:Clean;Rebuild
%buildtool% "%TestSuiteRoot%ProtoSDK\MS-SMB\Smb.csproj" /t:Clean;Rebuild
%buildtool% "%TestSuiteRoot%ProtoSDK\MS-DFSC\Dfsc.csproj" /t:Clean;Rebuild
%buildtool% "%TestSuiteRoot%ProtoSDK\MS-RPCE\Rpce.csproj" /t:Clean;Rebuild
%buildtool% "%TestSuiteRoot%ProtoSDK\MS-FSRVP\Fsrvp.csproj" /t:Clean;Rebuild
%buildtool% "%TestSuiteRoot%ProtoSDK\MS-SRVS\Srvs.csproj" /t:Clean;Rebuild
%buildtool% "%TestSuiteRoot%ProtoSDK\MS-SMB2\Smb2.csproj" /t:Clean;Rebuild
%buildtool% "%TestSuiteRoot%ProtoSDK\MS-RSVD\Rsvd.csproj" /t:Clean;Rebuild
%buildtool% "%TestSuiteRoot%ProtoSDK\MS-SWN\Swn.csproj" /t:Clean;Rebuild

%buildtool% "%TestSuiteRoot%ProtoSDK\MS-RDPBCGR\Rdpbcgr.csproj" /t:Clean;Rebuild
%buildtool% "%TestSuiteRoot%ProtoSDK\MS-RDPEDISP\Rdpedisp.csproj" /t:Clean;Rebuild
%buildtool% "%TestSuiteRoot%ProtoSDK\MS-RDPEDYC\MS-RDPEDYC.csproj" /t:Clean;Rebuild
%buildtool% "%TestSuiteRoot%ProtoSDK\MS-RDPEGFX\Rdpegfx.csproj" /t:Clean;Rebuild
%buildtool% "%TestSuiteRoot%ProtoSDK\MS-RDPEGT\Rdpegt.csproj" /t:Clean;Rebuild
%buildtool% "%TestSuiteRoot%ProtoSDK\MS-RDPEI\Rdpei.csproj" /t:Clean;Rebuild
%buildtool% "%TestSuiteRoot%ProtoSDK\MS-RDPEMT\Rdpemt.csproj" /t:Clean;Rebuild
%buildtool% "%TestSuiteRoot%ProtoSDK\MS-RDPEUDP\Rdpeudp.csproj" /t:Clean;Rebuild
%buildtool% "%TestSuiteRoot%ProtoSDK\MS-RDPEUSB\Rdpeusb.csproj" /t:Clean;Rebuild
%buildtool% "%TestSuiteRoot%ProtoSDK\MS-RDPEVOR\Rdpevor.csproj" /t:Clean;Rebuild
%buildtool% "%TestSuiteRoot%ProtoSDK\MS-RDPRFX\Rdprfx.csproj" /t:Clean;Rebuild