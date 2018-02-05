// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Protocols.TestSuites.ActiveDirectory.Adts.MAAdapter;
using Microsoft.Protocols.TestSuites.ActiveDirectory.Common;
using Microsoft.Protocols.TestTools;
using Microsoft.Protocols.TestTools.MessageAnalyzer;
using Microsoft.Protocols.TestTools.StackSdk.ActiveDirectory.Adts;
using Microsoft.Protocols.TestTools.StackSdk.ActiveDirectory.Adts.Asn1CodecV3;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.DirectoryServices.Protocols;
using System.IO;
using System.Net;
using System.Text;
using System.Threading;

namespace Microsoft.Protocols.TestSuites.ActiveDirectory.Adts.PublishDc
{
    /// <summary>
    /// A PTF test class
    /// </summary>
    [TestClass]
    public class TestSuite : TestClassBase
    {
        #region Variables

        new static ITestSite Site;
        static PublishDCAdapter publishDCAdapter;
        static AdLdapClient ldapAdapter;
        ICollection<AdtsSearchResultEntryPacket> searchResponse;
        static IMessageAnalyzerAdapter MaAdapter;

        // Extracts the required lines from specified file based on the keyword.
        ICollection<string> matchedLines = new List<string>();
        // Server will return those matching records based on the question sent by requester.
        string response = string.Empty;
        // Redirects the Specified Data to a text file,if it successful, it will be true.
        bool isRedirectionDone = false;

        #endregion

        #region Test Suite Initialization

        [ClassInitialize]
        public static void ClassInitialize(TestContext testContext)
        {
            TestClassBase.Initialize(testContext);
            Site = TestClassBase.BaseTestSite;
            publishDCAdapter = new PublishDCAdapter();
            publishDCAdapter.Initialize(Site);
            ldapAdapter = AdLdapClient.Instance();
            Site.DefaultProtocolDocShortName = "MS-ADTS-PublishDC";

            Site.Log.Add(LogEntryKind.Debug, "Initialize Message Analyzer adapter.");
            MaAdapter = Site.GetAdapter<IMessageAnalyzerAdapter>();
        }

        [ClassCleanup]
        public static void ClassCleanup()
        {
            TestClassBase.Cleanup();
        }

        #endregion

        #region Test Cases

        /// <summary>
        /// Connect and send an LDAP bind request to LDAP server
        /// </summary>
        [TestMethod]
        [TestCategory("MS-ADTS-PublishDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("PDC")]
        public void PublishDC_Scenario0()
        {
            //Connect and send an LDAP bind request to LDAP Server
            string connectStatus = ldapAdapter.ConnectAndBind(publishDCAdapter.PDCFullName, IPAddress.Parse(publishDCAdapter.PDCIPAddress), Int32.Parse(publishDCAdapter.ADDSPortNum),
                publishDCAdapter.DomainAdministratorName, publishDCAdapter.DomainUserPassword, publishDCAdapter.PrimaryDomainDnsName, AuthType.Kerberos);
            Assert.IsTrue(connectStatus.Contains("STATUS_SUCCESS"), "Bind response result should be LDAPResult_resultCode.success.");
        }

        /// <summary>
        /// Verify SamLogonResponseEX structure
        /// </summary>
        [TestMethod]
        [TestCategory("MS-ADTS-PublishDC")]
        [TestCategory("DomainWin2012")]
        [TestCategory("ForestWin2012")]
        [TestCategory("PDC")]
		[DeploymentItem(@"C:\Program Files\Microsoft Message Analyzer\Microsoft.WindowsAzure.Storage.dll")]
        public void PublishDC_TestCaseSamLogonResponseEx()
        {
            #region Capture
            MaAdapter.Reset();
            MaAdapter.StartCapture(publishDCAdapter.capturePath_SamLogonResponseEx);

            string exePath = GetResponseTriggerPath();
            ProcessStartInfo startInfo = new ProcessStartInfo(exePath);
            startInfo.Arguments = publishDCAdapter.PDCIPAddress + " " + publishDCAdapter.PrimaryDomainDnsName + " ex";
            startInfo.UseShellExecute = false;
            Process.Start(startInfo);
            Thread.Sleep(10000);

            MaAdapter.StopCapture();

            string filter = publishDCAdapter.SamLogonResponseExFilter + " and IPv4.Address == " + publishDCAdapter.PDCIPAddress;
            Message message = MaAdapter.GetMessages(publishDCAdapter.capturePath_SamLogonResponseEx, filter)[0];
            #endregion Capture

            #region Sbz
            object fieldValue = message.TryGetValue<object>(publishDCAdapter.SbzFilter);
            Site.CaptureRequirementIfAreEqual<UInt16>(0, Convert.ToUInt16(fieldValue),
            149, "Sbz (2 bytes) field of NETLOGON_SAM_LOGON_RESPONSE_EX structure MUST be set to 0.");
            #endregion Sbz

            #region Flags
            fieldValue = null;
            fieldValue = message.TryGetValue<object>(publishDCAdapter.FlagsFilter);
            Site.CaptureRequirementIfIsNotNull(
                fieldValue,
                150,
                "Flags (4 bytes) field of NETLOGON_SAM_LOGON_RESPONSE_EX structure is the DS_FLAG Options.");
            Site.CaptureRequirementIfIsNotNull(
                fieldValue,
                279,
                @"If the server uses NETLOGON_SAM_LOGON_RESPONSE_EX to pack the value of Netlogon attribute in 
                SearchResultEntry, Bit values of Flags field in Netlogon Attribute of SearchResultEntry are taken 
                from DS_FLAGS structure.");
            if (publishDCAdapter.IsSutGc)
            {
                Site.CaptureRequirementIfAreNotEqual<UInt32>(
                    0, Convert.ToUInt32(fieldValue) & publishDCAdapter.DS_GC_FLAG,
                    281,
                    @"If the server uses NETLOGON_SAM_LOGON_RESPONSE_EX to pack the value of Netlogon attribute in 
                    SearchResultEntry and the server is a global catalog server, then the DS_GC_FLAG bit of Flags field 
                    in Netlogon Attribute of SearchResultEntry is set.");
            }
            // "GlobalCatalogReady" is set to true by default.
            Site.CaptureRequirementIfAreNotEqual<UInt32>(
                0, Convert.ToUInt32(fieldValue) & publishDCAdapter.DS_GC_FLAG,
                282,
                @"If the server uses NETLOGON_SAM_LOGON_RESPONSE_EX to pack the value of Netlogon attribute in 
                SearchResultEntry, then DS_GC_FLAG bit of Flags field in Netlogon Attribute of SearchResultEntry is 
                set if and only if the is GlobalCatalogReady attribute on the rootDSE is true.");
            if (publishDCAdapter.IsSutKdc)
            {
                Site.CaptureRequirementIfAreNotEqual<UInt32>(
                    0, Convert.ToUInt32(fieldValue) & publishDCAdapter.DS_KDC_FLAG,
                    283,
                    @"If the server uses NETLOGON_SAM_LOGON_RESPONSE_EX to pack the value of Netlogon attribute in 
                    SearchResultEntry and the server is a KDC, then the DS_KDC_FLAG bit of Flags field in Netlogon 
                    Attribute of SearchResultEntry is set.");
            }
            if (publishDCAdapter.IsSutHostingWin32TimeService)
            {
                Site.CaptureRequirementIfAreNotEqual<UInt32>(
                    0, Convert.ToUInt32(fieldValue) & publishDCAdapter.DS_TIMESERV_FLAG,
                    284,
                    @"If the server uses NETLOGON_SAM_LOGON_RESPONSE_EX to pack the value of Netlogon attribute in 
                    SearchResultEntry and hosts the Win32 Time Service, then the DS_TIMESERV_FLAG bit of Flags field in 
                    Netlogon Attribute of SearchResultEntry is set.");
                Site.CaptureRequirementIfAreNotEqual<UInt32>(
                    0, Convert.ToUInt32(fieldValue) & publishDCAdapter.DS_TIMESERV_FLAG,
                    10290,
                    @"If the server uses NETLOGON_SAM_LOGON_RESPONSE_EX to pack the value of Netlogon attribute in 
                    SearchResultEntry and hosts Win32 Time Service, as specified in [MS-W32T], then the DS_TIMESERV_FLAG
                    bit of DS_FLAGS is set.");
            }
//            if (publishDCAdapter.IsSutInSameSiteAsClient)
//            {
//                Site.CaptureRequirementIfAreNotEqual<UInt32>(
//                    0, Convert.ToUInt32(fieldValue) & publishDCAdapter.DS_CLOSEST_FLAG,
//                    285,
//                    @"If the server uses NETLOGON_SAM_LOGON_RESPONSE_EX to pack the value of Netlogon attribute in 
//                    SearchResultEntry and the server is in the same site as the client, then the DS_CLOSEST_FLAG bit 
//                    of Flags field in Netlogon Attribute of SearchResultEntry is set.");
//            }
            if (!publishDCAdapter.IsSutRodc)
            {
                Site.CaptureRequirementIfAreNotEqual<UInt32>(
                   0, Convert.ToUInt32(fieldValue) & publishDCAdapter.DS_WRITABLE_FLAG,
                   286,
                   @"If the server uses NETLOGON_SAM_LOGON_RESPONSE_EX to pack the value of Netlogon attribute in 
                   SearchResultEntry and the server is not an RODC, then the DS_WRITABLE_FLAG bit of Flags field 
                   in Netlogon Attribute of SearchResultEntry is set.");
                Site.CaptureRequirementIfAreEqual<UInt32>(
                    0, Convert.ToUInt32(fieldValue) & publishDCAdapter.DS_SELECT_SECRET_DOMAIN_6_FLAG,
                    289,
                    @"If the server uses NETLOGON_SAM_LOGON_RESPONSE_EX to pack the value of Netlogon attribute in 
                    SearchResultEntry and the server is an RODC, then the DS_SELECT_SECRET_DOMAIN_6_FLAG bit of Flags 
                    field in Netlogon Attribute of SearchResultEntry is set.");
            }
            if (publishDCAdapter.IsSutRodc)
            {
                Site.CaptureRequirementIfAreNotEqual<UInt32>(
                    0, Convert.ToUInt32(fieldValue) & publishDCAdapter.DS_SELECT_SECRET_DOMAIN_6_FLAG,
                    289,
                    @"If the server uses NETLOGON_SAM_LOGON_RESPONSE_EX to pack the value of Netlogon attribute in 
                    SearchResultEntry and the server is an RODC, then the DS_SELECT_SECRET_DOMAIN_6_FLAG bit of Flags 
                    field in Netlogon Attribute of SearchResultEntry is set.");
            }
            Site.CaptureRequirementIfIsTrue(
                (((Convert.ToUInt32(fieldValue) & publishDCAdapter.DS_LDAP_FLAG) != 0) && ((Convert.ToUInt32(fieldValue) & publishDCAdapter.DS_DS_FLAG) != 0)),
                294,
                @"If the server uses NETLOGON_SAM_LOGON_RESPONSE_EX to pack the value of Netlogon attribute in 
                SearchResultEntry, Always set the DS_LDAP_FLAG and DS_DS_FLAG bits of Flags field in Netlogon Attribute 
                of SearchResultEntry.");
            if (publishDCAdapter.PDCOSVersion >= ServerVersion.Win2008)
            {
                if (publishDCAdapter.IsSutWritableDc)
                {
                    Site.CaptureRequirementIfAreNotEqual<UInt32>(
                        0,
                        Convert.ToUInt32(fieldValue) & publishDCAdapter.DS_FULL_SECRET_DOMAIN_6_FLAG,
                        290,
                        @"If the server uses NETLOGON_SAM_LOGON_RESPONSE_EX to pack the value of Netlogon attribute in 
                        SearchResultEntry and the server is a writable DC and not running Windows Server 2003 operating 
                        system or Microsoft Windows 2000 Server operating system, then the DS_FULL_SECRET_DOMAIN_6_FLAG 
                        bit of Flags field in Netlogon Attribute of SearchResultEntry is set.");
                }
            }
            Site.Assert.IsTrue((Convert.ToUInt32(fieldValue) & publishDCAdapter.DS_Reserved) == 0,
                @"X: Reserved for future expansion. The server MUST return zero, and the client MUST ignore.");
            if (publishDCAdapter.PDCOSVersion >= ServerVersion.Win2012)
            {
                Site.Assert.IsTrue((Convert.ToUInt32(fieldValue) & publishDCAdapter.DS_DS_8_FLAG) !=0,
                    @"FW8 (DS_DS_8_FLAG, 0x00004000): The server is not running Windows 2000, Windows 2003, Windows Server 2008,
                or Windows Server 2008 R2 operating system");

            }
            else
            {
                Site.Assert.IsTrue((Convert.ToUInt32(fieldValue) & publishDCAdapter.DS_DS_8_FLAG) == 0,
                    @"DS_DS_8_Flag should be set to 0 when server OS version is lower than Windows Server 2012.");
            }
            #endregion Flags

            #region NtVersion
            fieldValue = null;
            fieldValue = message.TryGetValue<object>(publishDCAdapter.NtVersionFilter);
            Site.CaptureRequirementIfIsTrue(
                (((Convert.ToUInt32(fieldValue) & 0x1) != 0) && ((Convert.ToUInt32(fieldValue) & 0x4) != 0)), 181,
                @"NtVersion (4 bytes) field of NETLOGON_SAM_LOGON_RESPONSE_EX structure is NETLOGON_NT_VERSION_1 | NETLOGON_NT_VERSION_5EX.");
            #endregion NtVersion

            #region LmNtToken
            fieldValue = null;
            fieldValue = message.TryGetValue<object>(publishDCAdapter.LmNtTokenFilter);
            Site.CaptureRequirementIfIsTrue(Convert.ToUInt16(fieldValue) == 0xFFFF, 182,
                "LmNtToken (2 bytes] field of NETLOGON_SAM_LOGON_RESPONSE_EX structure MUST be set to 0xFFFF.");
            Site.CaptureRequirementIfIsTrue(Convert.ToUInt16(fieldValue) == 0xFFFF, 311,
                @"If the server uses NETLOGON_SAM_LOGON_RESPONSE_EX to pack the value of Netlogon attribute in 
                SearchResultEntry, LmNtToken is always set to 0xFFFF.");
            #endregion LmNtToken

            #region Lm20Token
            fieldValue = null;
            fieldValue = message.TryGetValue<object>(publishDCAdapter.Lm20TokenFilter);
            Site.CaptureRequirementIfIsTrue(Convert.ToUInt16(fieldValue) == 0xFFFF, 183,
                "Lm20Token (2 bytes) field of NETLOGON_SAM_LOGON_RESPONSE_EX structure MUST be set to 0xFFFF.");
            Site.CaptureRequirementIfIsTrue(Convert.ToUInt16(fieldValue) == 0xFFFF, 312,
                @"If the server uses NETLOGON_SAM_LOGON_RESPONSE_EX to pack the value of Netlogon attribute in 
                SearchResultEntry, Lm20Token is always set to 0xFFFF.");
            #endregion Lm20Token

            #region Opcode
            fieldValue = message.TryGetValue<object>(publishDCAdapter.OpcodeFilter);
            // 0x17 represents a NETLOGON_SAM_LOGON_RESPONSE_EX structure.
            Site.CaptureRequirementIfAreEqual<UInt16>(
                0x17,
                Convert.ToUInt16(fieldValue),
                278,
                @"If the server uses NETLOGON_SAM_LOGON_RESPONSE_EX to pack the value of Netlogon attribute in 
                SearchResultEntry, Operation code is set to LOGON_SAM_LOGON_RESPONSE_EX if t(Refer to algorithm 
                mentioned in TD Section 7.3.3.2) is not equal to 1 and (u(Refer to algorithm mentioned in TD 
                Section 7.3.3.2) is NULL and x(Refer to algorithm mentioned in TD Section 7.3.3.2) is not NULL).");
            #endregion Opcode

            #region DomainGuid
            fieldValue = null;
            fieldValue = message.TryGetValue<object>(publishDCAdapter.DomainGuidFilter);

            Site.CaptureRequirementIfAreEqual<string>(
                publishDCAdapter.PrimaryDomainSrvGUID.ToLower(),
                fieldValue.ToString().ToLower().Replace("{","").Replace("}",""),
                296,
                @"If the server uses NETLOGON_SAM_LOGON_RESPONSE_EX to pack the value of Netlogon attribute in 
                SearchResultEntry, Domain GUID is set to the Guid of NC nc(NC designated by whichever of reqDnsNC 
                and reqGuidNC that is not NULL).");
            #endregion DomainGuid

            #region DnsForestName
            fieldValue = null;
            fieldValue = message.TryGetValue<object>(publishDCAdapter.DnsForestNameFilter);
            Site.CaptureRequirementIfAreEqual<string>(
                publishDCAdapter.PrimaryDomainDnsName.ToLower(),
                fieldValue.ToString().ToLower(),
                297,
                @"If the server uses NETLOGON_SAM_LOGON_RESPONSE_EX to pack the value of Netlogon attribute in 
                SearchResultEntry, DnsForestName is set to the DNS name of the forest.");
            #endregion DnsForestName

            #region DnsDomainName
            fieldValue = null;
            fieldValue = message.TryGetValue<object>(publishDCAdapter.DnsDomainNameFilter);
            Site.CaptureRequirementIfAreEqual<string>(
                publishDCAdapter.PrimaryDomainDnsName.ToLower(),
                fieldValue.ToString().ToLower(),
                298,
                @"If the server uses NETLOGON_SAM_LOGON_RESPONSE_EX to pack the value of Netlogon attribute in 
                SearchResultEntry, DnsDomainName is set to the DNS name of the NC nc(NC designated by whichever of 
                reqDnsNC and reqGuidNC that is not NULL).");
            #endregion DnsDomainName

            #region DnsHostName
            fieldValue = null;
            fieldValue = message.TryGetValue<object>(publishDCAdapter.DnsHostNameFilter);
            Site.CaptureRequirementIfAreEqual<string>(
                publishDCAdapter.PDCFullName.ToLower(),
                fieldValue.ToString().ToLower(),
                299,
                @"If the server uses NETLOGON_SAM_LOGON_RESPONSE_EX to pack the value of Netlogon attribute in 
                SearchResultEntry, DnsHostnName is set to the DNS name of the server.");
            #endregion DnsHostName

            #region NetbiosDomainName
            fieldValue = null;
            fieldValue = message.TryGetValue<object>(publishDCAdapter.NetbiosDomainNameFilter);
            Site.CaptureRequirementIfAreEqual<string>(
                publishDCAdapter.PrimaryDomainNetBiosName.ToLower(),
                fieldValue.ToString().ToLower(),
                300,
                @"If the server uses NETLOGON_SAM_LOGON_RESPONSE_EX to pack the value of Netlogon attribute in 
                SearchResultEntry, NetbiosDomainName is set to the NetBIOS name of the NC nc(NC designated by whichever
                of reqDnsNC and reqGuidNC that is not NULL).");
            #endregion NetbiosDomainName

            #region NetbiosComputerName
            fieldValue = null;
            fieldValue = message.TryGetValue<object>(publishDCAdapter.NetbiosComputerNameFilter);
            Site.CaptureRequirementIfAreEqual<string>(
                publishDCAdapter.PDCNetbiosName.ToLower(),
                fieldValue.ToString().ToLower(),
                301,
                @"If the server uses NETLOGON_SAM_LOGON_RESPONSE_EX to pack the value of Netlogon attribute in 
                SearchResultEntry, NetbiosComputerName is]Set to the NetBIOS name of the server.");
            #endregion NetbiosComputerName

            #region DcSiteName
            fieldValue = null;
            fieldValue = message.TryGetValue<object>(publishDCAdapter.DcSiteNameFilter);
            Site.CaptureRequirementIfAreEqual<string>(
                publishDCAdapter.ActualDcSiteName.ToLower(),
                fieldValue.ToString().ToLower(),
                303,
                @"If the server uses NETLOGON_SAM_LOGON_RESPONSE_EX to pack the value of Netlogon attribute in 
                SearchResultEntry, DcSiteName is set to the site name of the server.");
            #endregion DcSiteName

//            #region ClientSiteName
//            fieldValue = null;
//            fieldValue = message.TryGetValue<object>(publishDCAdapter.ClientSiteNameFilter);
//            Site.CaptureRequirementIfAreEqual<string>(
//                publishDCAdapter.ActualDcSiteName.ToLower(),
//                fieldValue.ToString().ToLower(),
//                304,
//                @"If the server uses NETLOGON_SAM_LOGON_RESPONSE_EX to pack the value of Netlogon attribute in 
//                SearchResultEntry, ClientSiteName is set to the site s(site containing the client's IP address).");
//            #endregion ClientSiteName

            #region DcSockAddr
            fieldValue = null;
            fieldValue = message.TryGetValue<object>(publishDCAdapter.DcSockAddrFilter);
            ComplexType fieldValueComplex = (ComplexType)fieldValue;

            Site.CaptureRequirementIfAreEqual<UInt16>(2, fieldValueComplex.TryGetValue<UInt16>(publishDCAdapter.SinFamilyFilter), 165,
                @"sin_family (2 bytes) field of DcSockAddr SHOULD always be AF_INET (that is, 2).");
            Site.CaptureRequirementIfAreEqual<UInt16>(2, fieldValueComplex.TryGetValue<UInt16>(publishDCAdapter.SinFamilyFilter), 166,
                @"sin_family (2 bytes) of DcSockAddr does always be AF_INET (that is, 2).");
            Site.CaptureRequirementIfAreEqual<UInt16>(0, fieldValueComplex.TryGetValue<UInt16>(publishDCAdapter.SinPortFilter), 168,
                @"sin_port (2 bytes) of DcSockAddr SHOULD always be zero.");
            Site.CaptureRequirementIfAreEqual<UInt16>(0, fieldValueComplex.TryGetValue<UInt16>(publishDCAdapter.SinPortFilter), 169,
                @"sin_port (2 bytes) of DcSockAddr does always be zero.");
            Site.CaptureRequirementIfAreEqual<UInt64>(0, fieldValueComplex.TryGetValue<UInt64>(publishDCAdapter.SinZeroFilter), 176,
                "sin_zero (8 bytes) of DcSockAddr MUST set to zero when sending.");
            string actualAddr = string.Empty;
            byte[] addrArray = fieldValueComplex.TryGetValue<ComplexType>(publishDCAdapter.SinAddrFilter).TryGetValue<byte[]>("Octets");
            foreach (byte addrByte in addrArray)
            {
                actualAddr += ((int)addrByte).ToString();
            }
            Site.CaptureRequirementIfAreEqual<string>(publishDCAdapter.PDCIPAddress.Replace(".",""),
                actualAddr, 
                306,
                @"If the server uses NETLOGON_SAM_LOGON_RESPONSE_EX to pack the value of Netlogon attribute in 
                SearchResultEntry, SockAddr is set to the server's  IP address of the server.");
            #endregion DcSockAddr

            #region DcSockAddrSize
            fieldValue = null;
            fieldValue = message.TryGetValue<object>(publishDCAdapter.DcSockAddrSizeFilter);
            // Size of the IPAddress is 16 bytes. Hence compared the dcAddressSize with "16"
            Site.CaptureRequirementIfAreEqual<UInt16>(16, Convert.ToUInt16(fieldValue), 305,
                @"If the server uses NETLOGON_SAM_LOGON_RESPONSE_EX to pack the value of Netlogon attribute in 
                SearchResultEntry, DcSockAddrSize is set to the size of the server's IP address.");
            #endregion DcSockAddrSize
        }

        /// <summary>
        /// Verify SamLogonResponse structure
        /// </summary>
        [TestMethod]
        [TestCategory("MS-ADTS-PublishDC")]
        [TestCategory("DomainWin2012")]
        [TestCategory("ForestWin2012")]
        [TestCategory("PDC")]
		[DeploymentItem(@"C:\Program Files\Microsoft Message Analyzer\Microsoft.WindowsAzure.Storage.dll")]
        public void PublishDC_TestCaseSamLogonResponse()
        {
            #region Capture
            MaAdapter.Reset();
            MaAdapter.StartCapture(publishDCAdapter.capturePath_SamLogonResponse);

            string exePath = GetResponseTriggerPath();
            ProcessStartInfo startInfo = new ProcessStartInfo(exePath);
            startInfo.Arguments = publishDCAdapter.PDCIPAddress + " " + publishDCAdapter.PrimaryDomainDnsName + " nonex";
            startInfo.UseShellExecute = false;
            Process.Start(startInfo);
            Thread.Sleep(10000);

            MaAdapter.StopCapture();

            string filter = publishDCAdapter.SamLogonResponseFilter + " and IPv4.Address == " + publishDCAdapter.PDCIPAddress;
            Message message = MaAdapter.GetMessages(publishDCAdapter.capturePath_SamLogonResponse, filter)[0];
            #endregion Capture
            
            #region NtVersion
            object fieldValue = message.TryGetValue<object>(publishDCAdapter.NtVersionFilter);
            Site.CaptureRequirementIfIsTrue(
                (((Convert.ToUInt32(fieldValue) & 0x00000001) != 0) && ((Convert.ToUInt32(fieldValue) & 0x00000002) != 0)), 143,
                @"NtVersion (4 bytes) field of NETLOGON_SAM_LOGON_RESPONSE structure is set to NETLOGON_NT_VERSION_1 | 
                NETLOGON_NT_VERSION_5.");
            Site.CaptureRequirementIfIsTrue(
                (((Convert.ToUInt32(fieldValue) & 0x00000001) != 0) && ((Convert.ToUInt32(fieldValue) & 0x00000002) != 0)), 328,
                @"If the server uses NETLOGON_SAM_LOGON_RESPONSE to pack the value of Netlogon attribute in 
                SearchResultEntry, NtVersion is set to NETLOGON_NT_VERSION_1 | NETLOGON_NT_VERSION_5.");
            #endregion NtVersion

            #region LmNtToken
            fieldValue = null;
            fieldValue = message.TryGetValue<object>(publishDCAdapter.LmNtTokenFilter);
            Site.CaptureRequirementIfIsTrue(Convert.ToUInt16(fieldValue) == 0xFFFF, 144,
                "LmNtToken (2 bytes) field of NETLOGON_SAM_LOGON_RESPONSE structure MUST be set to 0xFFFF.");
            Site.CaptureRequirementIfIsTrue(Convert.ToUInt16(fieldValue) == 0xFFFF, 329,
                @"If the server uses NETLOGON_SAM_LOGON_RESPONSE to pack the value of Netlogon attribute in 
                SearchResultEntry, LmNtToken is always set to 0xFFFF.");
            #endregion LmNtToken

            #region Lm20Token
            fieldValue = null;
            fieldValue = message.TryGetValue<object>(publishDCAdapter.Lm20TokenFilter);
            Site.CaptureRequirementIfIsTrue(Convert.ToUInt16(fieldValue) == 0xFFFF, 145,
                "Lm20Token (2 bytes) field of NETLOGON_SAM_LOGON_RESPONSE structure MUST be set to 0xFFFF.");
            Site.CaptureRequirementIfIsTrue(Convert.ToUInt16(fieldValue) == 0xFFFF, 330,
                @"If the server uses NETLOGON_SAM_LOGON_RESPONSE to pack the value of Netlogon attribute in 
                SearchResultEntry, Lm20Token is always set to 0xFFFF.");
            #endregion Lm20Token

            #region DomainGuid
            fieldValue = null;
            fieldValue = message.TryGetValue<object>(publishDCAdapter.DomainGuidFilter);

            Site.CaptureRequirementIfAreEqual<string>(
                publishDCAdapter.PrimaryDomainSrvGUID.ToLower(),
                fieldValue.ToString().ToLower().Replace("{", "").Replace("}", ""),
                319,
                @"If the server uses NETLOGON_SAM_LOGON_RESPONSE to pack the value of Netlogon attribute in 
                SearchResultEntry, DomainGuid is set to the GUID of the domain.");
            #endregion DomainGuid

            #region NullGuid
            fieldValue = null;
            fieldValue = message.TryGetValue<object>(publishDCAdapter.NullGuidFilter);
            Site.CaptureRequirementIfAreEqual<Guid>(
                Guid.Empty,
                (Guid)fieldValue,
                320,
                @"If the server uses NETLOGON_SAM_LOGON_RESPONSE to pack the value of Netlogon attribute in 
                SearchResultEntry, SiteGuid is always set to NULL GUID.");
            #endregion NullGuid

            #region DnsForestName
            fieldValue = null;
            fieldValue = message.TryGetValue<object>(publishDCAdapter.DnsForestNameFilter);
            Site.CaptureRequirementIfAreEqual<string>(
                publishDCAdapter.PrimaryDomainDnsName.ToLower(),
                fieldValue.ToString().ToLower(),
                321,
                @"If the server uses NETLOGON_SAM_LOGON_RESPONSE to pack the value of Netlogon attribute in 
                SearchResultEntry, DnsForestName is set to the DNS name of the forest.");
            #endregion DnsForestName

            #region DnsDomainName
            fieldValue = null;
            fieldValue = message.TryGetValue<object>(publishDCAdapter.DnsDomainNameFilter);
            Site.CaptureRequirementIfAreEqual<string>(
                publishDCAdapter.PrimaryDomainDnsName.ToLower(),
                fieldValue.ToString().ToLower(),
                322,
                @"If the server uses NETLOGON_SAM_LOGON_RESPONSE to pack the value of Netlogon attribute in 
                SearchResultEntry, DnsDomainName is set to the DNS name of the domain.");
            #endregion DnsDomainName

            #region DnsHostName
            fieldValue = null;
            fieldValue = message.TryGetValue<object>(publishDCAdapter.DnsHostNameFilter);
            Site.CaptureRequirementIfAreEqual<string>(
                publishDCAdapter.PDCFullName.ToLower(),
                fieldValue.ToString().ToLower(),
                323,
                @"If the server uses NETLOGON_SAM_LOGON_RESPONSE to pack the value of Netlogon attribute in 
                SearchResultEntry, DnsHostName is set to the DNS name of the server.");
            #endregion DnsHostName

            #region Flags
            fieldValue = null;
            fieldValue = message.TryGetValue<object>(publishDCAdapter.FlagsFilter);
            Site.CaptureRequirementIfIsTrue(
                ((Convert.ToUInt32(fieldValue) & publishDCAdapter.DS_DS_FLAG) != 0),
                326,
                @"If the server uses NETLOGON_SAM_LOGON_RESPONSE to pack the valueof Netlogon attribute in 
                SearchResultEntry, bit DS_DS_FLAG of Flags field is always set.");
            Site.CaptureRequirementIfIsTrue(
                (((Convert.ToUInt32(fieldValue) & publishDCAdapter.DS_DNS_CONTROLLER_FLAG) == 0)
                && ((Convert.ToUInt32(fieldValue) & publishDCAdapter.DS_DNS_DOMAIN_FLAG) == 0)
                && ((Convert.ToUInt32(fieldValue) & publishDCAdapter.DS_DNS_FOREST_FLAG) == 0)),
                327,
                @"If the server uses NETLOGON_SAM_LOGON_RESPONSE to pack the value of Netlogon attribute in 
                SearchResultEntry, all the other bits (other than DS_PDC_FLAG, DS_DS_FLAG) of DS_FLAG structure are 
                set to 0.");
            if (publishDCAdapter.IsSutPdc)
            {
                Site.CaptureRequirementIfAreNotEqual<UInt32>(
                    0,
                    Convert.ToUInt32(fieldValue) & publishDCAdapter.DS_PDC_FLAG,
                    325,
                    @"If the server uses NETLOGON_SAM_LOGON_RESPONSE to pack the value of Netlogon attribute in 
                    SearchResultEntry, if the server is a PDC, bit DS_PDC_FLAG of Flags field is set");
            }
            if (publishDCAdapter.PDCOSVersion >= ServerVersion.Win2012)
            {
                Site.Assert.IsTrue((Convert.ToUInt32(fieldValue) & publishDCAdapter.DS_DS_8_FLAG) != 0,
                    @"FW8 (DS_DS_8_FLAG, 0x00004000): The server is not running Windows 2000, Windows 2003, Windows Server 2008,
                or Windows Server 2008 R2 operating system");
            }
            else
            {
                Site.Assert.IsTrue((Convert.ToUInt32(fieldValue) & publishDCAdapter.DS_DS_8_FLAG) == 0,
                    @"DS_DS_8_Flag should be set to 0 when server OS version is lower than Windows Server 2012.");
            }
            #endregion Flags

            #region UnicodeLogonServer
            fieldValue = null;
            fieldValue = message.TryGetValue<object>(publishDCAdapter.UnicodeLogonServerFilter);

            Site.CaptureRequirementIfAreEqual<string>(
                "\\\\" + publishDCAdapter.PDCNetbiosName.ToLower(),
                fieldValue.ToString().ToLower(),
                316,
                @"If the server uses NETLOGON_SAM_LOGON_RESPONSE to pack the value of Netlogon attribute in 
                SearchResultEntry, UnicodeLogonServer is set to the NetBIOS name of the server.");
            Site.CaptureRequirementIfAreNotEqual<string>(
                string.Empty,
                fieldValue.ToString(),
                127,
                @"UnicodeLogonServer (variable) field of NETLOGON_SAM_LOGON_RESPONSE structure always contains at least 
                one character: the null terminator.");
            #endregion UnicodeLogonServer

            #region UnicodeDomainName
            fieldValue = null;
            fieldValue = message.TryGetValue<object>(publishDCAdapter.UnicodeDomainNameFilter);

            Site.CaptureRequirementIfAreEqual<string>(
                publishDCAdapter.PrimaryDomainNetBiosName.ToLower(),
                fieldValue.ToString().ToLower(),
                318,
                @"If the server uses NETLOGON_SAM_LOGON_RESPONSE to pack the value of Netlogon attribute in 
                SearchResultEntry, UnicodeDomainName is set to the NetBIOS name of the domain.");
            Site.CaptureRequirementIfAreNotEqual<string>(
                string.Empty,
                fieldValue.ToString(),
                133,
                @"UnicodeDomainName (variable) field of NETLOGON_SAM_LOGON_RESPONSE structure always contains 
                at least one character: the null terminator.");
            #endregion UnicodeDomainName

            #region UnicodeUserName
            fieldValue = null;
            fieldValue = message.TryGetValue<object>(publishDCAdapter.UnicodeUserNameFilter);
            Site.CaptureRequirementIfAreEqual<string>(
                string.Empty,
                fieldValue.ToString().ToLower(),
                130,
                @"UnicodeUserName (variable) field of NETLOGON_SAM_LOGON_RESPONSE structure always contains at least one
                character: the null terminator.");
            #endregion UnicodeUserName
        }

        /// <summary>
        /// Verify AD LDS publication
        /// </summary>
        [TestMethod]
        [TestCategory("MS-ADTS-PublishDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("PDC")]
        public void PublishDC_ADLDSDCPublication()
        {
            #region LocalVariables
            
            // Domain name excluding com or net part
            string ldapProtectedServiceBind = string.Format("LDAPS://{0}:{1}",
                                              publishDCAdapter.PDCFullName.ToLower(),
                                              publishDCAdapter.ADLDSSSLPortNum);
            string ldapDefaultServiceBind = string.Format("LDAP://{0}:{1}",
                                            publishDCAdapter.PDCFullName.ToLower(),
                                            publishDCAdapter.ADLDSPortNum);
            StringBuilder serviceBindInformation = new StringBuilder();
            StringBuilder keyWordsInformation = new StringBuilder();

            bool isValidate = false;
            #endregion

            //Connect and send an LDAP bind request to LDAP Server
            string connectStatus = ldapAdapter.ConnectAndBind(publishDCAdapter.PDCFullName,IPAddress.Parse(publishDCAdapter.PDCIPAddress),Int32.Parse(publishDCAdapter.ADDSPortNum),
                publishDCAdapter.DomainAdministratorName,publishDCAdapter.DomainUserPassword,publishDCAdapter.PrimaryDomainDnsName,AuthType.Kerberos);
            Assert.IsTrue(connectStatus.Contains("STATUS_SUCCESS"), "Bind response result should be LDAPResult_resultCode.success."); 
            

            #region Search For ServiceConnectionPointObject
            // Searching For computer object in domain nc
            response = ldapAdapter.SearchObject(publishDCAdapter.DomainNC, System.DirectoryServices.Protocols.SearchScope.Subtree, "(objectClass=ServiceConnectionPoint)", null, null, out searchResponse);                              
                if (searchResponse != null)
                {
                    foreach (AdtsSearchResultEntryPacket entryPacket in searchResponse)
                    {
                        // This requirement is a combination of AD DS and AD LDS.
                        // We have validated with an assumption
                        // while creating AD/LDS instance the SCP PublicationService object is created in 
                        // DirectoryServiceContainer
                        // and  msDS-SCPContainer attribute value is null(We can see these values in adsiedit.msc)
                        // After that we are verifying service connection object properties
                        // i.e it contains guid of dc or not and it is relative to computer object on which it is running.
                        // If the object is under domain controllers container on which the instance is running 
                        // it is relative.
                        Microsoft.Protocols.TestTools.StackSdk.ActiveDirectory.Adts.Asn1CodecV3.SearchResultEntry entry =
                            (Microsoft.Protocols.TestTools.StackSdk.ActiveDirectory.Adts.Asn1CodecV3.SearchResultEntry)
                        entryPacket.GetInnerRequestOrResponse();
                        string entryObjectName = Encoding.ASCII.GetString(entry.objectName.ByteArrayValue).ToLower();
                        if (entryObjectName ==
                            String.Concat(
                                String.Concat("CN=".ToLower(),publishDCAdapter.DsaGuid),
                                ",CN=",publishDCAdapter.PDCNetbiosName,
                                ",OU=Domain Controllers,",
                                publishDCAdapter.DomainNC).ToLower())
                        {
                            bool isTrue = entryObjectName.Contains(publishDCAdapter.DsaGuid)
                                && entryObjectName.Contains(String.Concat("CN=",publishDCAdapter.PDCNetbiosName).ToLower());

                            Site.CaptureRequirementIfIsTrue(
                                isTrue,
                                443,
                                "The created (or updated) serviceConnectionPoint object S satisfies the condition:"+
                                "If O(the msDS-ServiceConnectionPointPublicationService object) not exists and O!"+
                                "msDS-SCPContainer is null, then the DN of S is \"CN={dsaGuid}\" relative to the "+
                                "computer object of the machine running AD LDS.");

                            isValidate = true;
                            PartialAttributeList attributeNames = entry.attributes;

                            foreach (PartialAttributeList_element attribute in attributeNames.Elements)
                            {
                                string attributeString = Encoding.ASCII.GetString(attribute.type.ByteArrayValue);
                                AttributeValue[] attributeValList = attribute.vals.Elements;

                                foreach (AttributeValue attributeVal in attributeValList)
                                {
                                    string attributeValue = Encoding.ASCII.GetString(attributeVal.ByteArrayValue);

                                    if (attributeString.ToLower().Equals("ServiceDNSNameType".ToLower()) )
                                    {
                                        Site.CaptureRequirementIfAreEqual<string>(
                                            "A",
                                            attributeValue,
                                            444,
                                            @"The created (or updated) serviceConnectionPoint object S satisfies the 
                                            condition: ServiceDNSNameType attribute of S is equal to A.");
                                    }

                                    if (attributeString.ToLower().Equals("ServiceClassName".ToLower()))
                                    {
                                        Site.CaptureRequirementIfAreEqual<string>(
                                            "LDAP",
                                            attributeValue,
                                            445,
                                            "The created (or updated) serviceConnectionPoint object S satisfies the "+
                                            "condition: ServiceClassName attribute of S is equal to \"LDAP\".");
                                    }

                                    if (attributeString.ToLower().Equals("ServiceDNSName".ToLower()))
                                    {
                                        Site.CaptureRequirementIfAreEqual<string>(
                                            publishDCAdapter.PDCFullName.ToLower(),
                                            attributeValue.ToLower(),
                                            446,
                                            @"The created (or updated) serviceConnectionPoint object S satisfies 
                                            the condition: ServiceDNSName attribute of S is the DNS name of 
                                            the computer on which the AD LDS DC is running.");
                                    }

                                    if (attributeString.ToLower().Equals("ServiceBindingInformation".ToLower()))
                                    {
                                        serviceBindInformation.Append(String.Concat(attributeValue.ToLower(), ";"));
                                    }

                                    if (attributeString.ToLower().Equals("Keywords".ToLower()))
                                    {
                                        keyWordsInformation.Append(String.Concat(attributeValue.ToLower(), ";"));
                                    }

                                }
                            }
                        }

                        // Once the service connection point object is created its "serviceBindingInformation" 
                        // attribute contains two values
                        // "ldap://dnsName:ldapPort" and "ldaps://dnsName:ldapsPort", 
                        // where dnsName is the DNS name of the computer on which the AD LDS DC is running, 
                        // ldapPort is the port on which the AD LDS DC is listening for LDAP requests, 
                        // and ldapsPort is the port on which the AD LDS DC is listening for SSL/TLS-protected LDAPS requests.
                        // So here we are retrieving serviceBindigInformation attribute value and verifying
                        // whether it contains ldap://dnsName:ldapPort" and "ldaps://dnsName:ldapsPort.
                        if (isValidate)
                        {
                            string serviceBindInfo = serviceBindInformation.ToString();
                            
                            Site.CaptureRequirementIfIsTrue(
                                serviceBindInfo.ToString().ToLower().Contains(ldapProtectedServiceBind.ToLower()),
                                449, 
                                "The created (or updated) serviceConnectionPoint object S satisfies the condition: "+
                                "ServiceBindingInformation attribute of S contains \"ldaps://dnsName:ldapsPort\", "+
                                "where dnsName is the DNS name of the computer on which the AD LDS DC is running and "+
                                "ldapsPort is the port on which the AD LDS DC is listening for SSL/TLS-protected LDAPS requests.");
                            
                            Site.CaptureRequirementIfIsTrue(
                                serviceBindInfo.ToString().ToLower().Contains(ldapDefaultServiceBind.ToLower()),
                                448, 
                                "The created (or updated) serviceConnectionPoint object S satisfies the condition: "+
                                "ServiceBindingInformation attribute of S contains \"ldap://dnsName:ldapPort\", "+
                                "where dnsName is the DNS name of the computer on which the AD LDS DC is running and "+
                                "ldapPort is the port on which the AD LDS DC is listening for LDAP requests.");
                            
                            // The capture code MS-ADTS-PublishDC_R447 is combined with MS-ADTS-PublishDC_R449 
                            // and MS-ADTS-PublishDC_R448 above.
                            // Verified the MS-ADTS-PublishDC_R449 and MS-ADTS-PublishDC_R448, 
                            // it means the MS-ADTS-PublishDC_R447 is also can be verified.
                            Site.CaptureRequirement(
                                447,
                                "The created (or updated) serviceConnectionPoint object S satisfies the condition: "+
                                "ServiceBindingInformation attribute of S contains two values, "+
                                "\"ldap://dnsName:ldapPort\" and \"ldaps://dnsName:ldapsPort\", where dnsName is the "+
                                "DNS name of the computer on which the AD LDS DC is running, ldapPort is the port on "+
                                "which the AD LDS DC is listening for LDAP requests, and ldapsPort is the port on which"+
                                " the AD LDS DC is listening for SSL/TLS-protected LDAPS requests.");
                            
                            string keyWords = keyWordsInformation.ToString().ToLower();
                            
                            // This is partial verification only because we verified a few in keyword attribute i.e
                            // site part,ApplicationNamingContextPart and InstancePart
                            Site.CaptureRequirementIfIsTrue(
                                keyWords.Contains(publishDCAdapter.AdLdsAppNamingContext.ToLower()) &&
                                keyWords.Contains(publishDCAdapter.AdLdsSite.ToLower()) &&
                                keyWords.Contains(publishDCAdapter.ADLDSInstanceName.ToLower()), 
                                450,
                                @"The created (or updated) serviceConnectionPoint object S satisfies the condition: The 
                                keywords attribute of S contains the values mentioned in the algorithm section 7.3.8.");
                        }
                    }
                }

                #endregion           
        }


        /// <summary>
        /// Verify DNS discovery
        /// </summary>
        [TestMethod]
        [TestCategory("MS-ADTS-PublishDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("PDC")]
        public void PublishDC_DnsBasedDiscovery1()
        {
            bool isClassTypeIn = false;

            #region R219_222_223
            
            // In debug interactive mode only we can see classType=In and TTL=600 Seconds of a TYPE CNAME record.
            // That is the reason why we started nslookup in interactive debug mode by setting -d2 option.
            // Please find the following information in the log which validates the above requirements
            // The server is sending the requrest for TYPE CNAME rercods with class=IN type
              
            // Nslookup is not providing any provision for retreiving TTL and classType of SRV/CNAME/A records.
            // How can we vaidate this?
            // --> Run nslookup.exe in debug interactive mode.In this mode we can see the server is processing
            //    The DNS Request given by the requester.
            // --> Please verify Questions Section in log file for more details.
            // --> In that section server is receving Request with classType=In and server will return
            //    those matching records based on the question sent by requester.
            bool isCnameType = false;
            response = HelperClass.ExecuteCommand(String.Concat("-d2 -type=CNAME ", publishDCAdapter.PrimaryDomainDnsName));

            if (response.ToLower() != "Invalid Parameter".ToLower())
            {
                isRedirectionDone = HelperClass.RedirectOutputToTextFile(response, "219_222_223.txt");
                Thread.Sleep(5000);
            }

            if (isRedirectionDone)
            {
                matchedLines = HelperClass.ReadLine(@"..\..\..\219_222_223.txt", "type = SOA, class = IN");

                if (matchedLines.Count > 0)
                {
                    isClassTypeIn = true;
                    isCnameType = true;
                }

                // Actual Validation
                // If it is in IPV4 we will get directly TTL = 600  (10 mins) like that
                // If it is in IPV6 we need to verify retry field to verify the Default TTL value
                // Here we need to observe the query given to the server and response from the server.
                matchedLines = HelperClass.ReadLine(@"..\..\..\219_222_223.txt", "retry   = 600 (10 mins)");

                foreach (string output in matchedLines)
                {
                    response = output.Split('=')[1].Trim();
                   
                    Site.CaptureRequirementIfIsTrue(
                        (response.Contains("600") && isCnameType), 
                        219, 
                        "The TTL field of each CNAME record registered by a server is set to 600 seconds");
                   
                    Site.CaptureRequirementIfIsTrue(
                        isClassTypeIn, 
                        222, 
                        "The Class field of each CNAME record registered by a server is set to IN.");
                    
                    Site.CaptureRequirementIfIsTrue(
                        isCnameType, 
                        223, 
                        "The Type field of each CNAME record registered by a server is set to CNAME.");

                    break;
                }
            }

            #endregion

            #region R227

         
            // We have provided the command as per the TD and we are able to see two Type A records along with their 
            // IP addresses.
            // We have provided input "-type=A" To ExecuteCommand method which will only retrieve the records with 
            // type=A. So 234 is also validated as part of this command.
            if (publishDCAdapter.IsSutGc)
            {
                response = HelperClass.ExecuteCommand(String.Concat("-type=A gc._msdcs." + publishDCAdapter.PrimaryDomainDnsName));

                if (response.ToLower() != "Invalid Parameter".ToLower())
                {
                    isRedirectionDone = HelperClass.RedirectOutputToTextFile(response, "227.txt");
                    Thread.Sleep(5000);

                    if (isRedirectionDone)
                    {
                        matchedLines = HelperClass.ReadLine(@"..\..\..\227.txt", "Name");

                        foreach (string output in matchedLines)
                        {
                            response = output.Split(':')[1].Trim();

                            Site.CaptureRequirementIfIsTrue(
                                (response.ToLower().Contains(String.Concat("gc._msdcs."
                                + publishDCAdapter.PrimaryDomainDnsName.ToLower()))),
                                227,
                                @"If a server is a DC with default NC X in forest Z, then it publishes a type A record 
                               with Name field X. If the DC is a GC server, it also publishes a type A record with
                               Name field gc._msdcs.Z.");

                            break;
                        }
                    }
                }
            }
            
            #endregion

            #region R230_233_234
         
            // In debug interactive mode only we can see classType=In and TTL=600 Seconds of a TYPE A record.
            // That is the reason why we started nslookup in interactive debug mode by setting -d2 option.
            // Please find the following information in the log which validates the above two requirements
            // The server is sending the requrest for TYPE A rercods with class=IN type
            bool isAType = false;
            response = HelperClass.ExecuteCommand(String.Concat("-d2 -type=A " + publishDCAdapter.PrimaryDomainDnsName));
           
            if (response.ToLower() != "Invalid Parameter".ToLower())
            {
                isRedirectionDone = HelperClass.RedirectOutputToTextFile(response, "230_233_234.txt");
                Thread.Sleep(5000);
            }

            matchedLines = HelperClass.ReadLine(@"..\..\..\230_233_234.txt", "type = A, class = IN");
            
            if (matchedLines.Count > 0)
            {
                isClassTypeIn = true;
                isAType = true;
            }

            matchedLines = HelperClass.ReadLine(@"..\..\..\230_233_234.txt", "ttl = 600 (10 mins)");
            
            foreach (string output in matchedLines)
            {
                response = output.Split('=')[1].Trim();
                
                Site.CaptureRequirementIfIsTrue(
                    (response.Contains("600") && isAType), 
                    230, 
                    "The TTL field of each type A record registered by a server is set to 600 seconds");
                
                Site.CaptureRequirementIfIsTrue(
                    isClassTypeIn, 
                    233, 
                    "The Class field of each type A record registered by a server is set to IN.");
                
                Site.CaptureRequirementIfIsTrue(
                    isAType, 
                    234, 
                    "The Type field of each type A record registered by a server is set to A.");

                break;
            }

            #endregion
        }


        /// <summary>
        /// Verify DNS discovery
        /// </summary>
        [TestMethod]
        [TestCategory("MS-ADTS-PublishDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("PDC")]
        public void PublishDC_DnsBasedDiscovery2()
        {
            #region R194
             
            // We have provided the command as per the TD and we are able to see the corresponding SRV records.
            // Please refer the log file for corresponding SRV records. The following are the list of commands we passed
            // as an input to ExecuteCommand Method.
            // _ldap._tcp.site1._sites.dc._msdcs.na.fabrikam.com
            response = HelperClass.ExecuteCommand(
                String.Concat("-type=srv _ldap._tcp.Default-First-Site-Name._sites.dc._msdcs.", publishDCAdapter.PrimaryDomainDnsName));

            if (response.ToLower() != "Invalid Parameter".ToLower())
            {
                isRedirectionDone = HelperClass.RedirectOutputToTextFile(response, "194.txt");
                Thread.Sleep(5000);
               
                if (isRedirectionDone)
                {
                    // Actual Validation
                    // Here the main verification is the structure is created or not as per the TD.That we have done by 
                    // providing input to ExecuteCommand. Here we are verifying "svr hostname" variable because
                    // it contais the SRV Records of a Domain Controller. Finally we will get Ping Information from DC.
                    matchedLines = HelperClass.ReadLine(@"..\..\..\194.txt", "svr hostname");
                    
                    foreach (string output in matchedLines)
                    {
                        response = output.Split('=')[1].Trim();
                        
                        //This validates that all DCs with default NC.
                        Site.CaptureRequirementIfIsTrue(
                            (response.ToLower().Contains(publishDCAdapter.PDCFullName.ToLower())
                            || response.ToLower().Contains(publishDCAdapter.SDCFullName.ToLower())
                            || response.ToLower().Contains(publishDCAdapter.RODCFullName.ToLower())), 
                            194, 
                            @"If a DC with default NC: X = na.fabrikam.com,is in site: Y = site1, and forest: 
                            Z = fabrikam.com and NC X's GUID is: G = 52f6c43b-99ec-4040-a2b0-e9ebf2ec02b8  then, 
                            its record of type _ldap._tcp.Y._sites.dc._msdcs.X has: Service.Proto.Name = _ldap.
                            _tcp.site1._sites.dc._msdcs.na.fabrikam.com");
                         break;
                    }
                }
            }

            #endregion

            #region R196_199

            // In debug interactive mode only we can see classType=In and TTL=600 Seconds of a SRV record.
            // That is the reason why we started nslookup in interactive debug mode by setting -d2 option.
            // Please find the following information in the log which validates the above two requirements
            // The server is sending the requrest for SRV rercods with class=IN type(Questions)
            bool isClassTypeIn = false;
            bool isSrvType = false;
            response = HelperClass.ExecuteCommand(String.Concat("-d2 -type=SRV ", publishDCAdapter.PrimaryDomainDnsName));
            
            if (response.ToLower() != "Invalid Parameter".ToLower())
            {
                isRedirectionDone = HelperClass.RedirectOutputToTextFile(response, "196_199.txt");
                Thread.Sleep(5000);
            }

            matchedLines = HelperClass.ReadLine(@"..\..\..\196_199.txt", "type = SRV, class = IN");
            
            if (matchedLines.Count > 0)
            {
                isClassTypeIn = true;
                isSrvType = true;
            }
            // Actual Validation
            // If it is in IPV4 we will get directly TTL = 600  (10 mins) like that
            // If it is in IPV6 we need to verify retry field to verify the Default TTL value
            // Here we need to observe the query given to the server and response from the server.
            matchedLines = HelperClass.ReadLine(@"..\..\..\196_199.txt", "retry   = 600 (10 mins)");
            
            foreach (string output in matchedLines)
            {
                response = output.Split('=')[1].Trim();
                
                Site.CaptureRequirementIfIsTrue(
                    (response.Contains("600") && isSrvType), 
                    196,
                    "The TTL field of SRV record registered by a server is set to 600 seconds.");
                
                Site.CaptureRequirementIfIsTrue(
                    (isClassTypeIn && isSrvType), 
                    199, 
                    "The Class field of SRV record registered by a server is set to IN.");

                break;
            }

            #endregion

            #region R216
           
            // We have provided the command as per the TD and we are able to see Type CNAME records along with their IP 
            // addresses.
            // We have provided the input "-type=CNAME" To ExecuteCommand method which will only retrieve the records with
            // type=CNAME.
            // In the log we are able to see the canonical name of each Domain controller.
            response = HelperClass.ExecuteCommand(String.Concat("-type=CNAME ", publishDCAdapter.PdcDnsAlias, "._msdcs.", 
                publishDCAdapter.PrimaryDomainDnsName));
           
            if (response.ToLower() != "Invalid Parameter".ToLower())
            {
                isRedirectionDone = HelperClass.RedirectOutputToTextFile(response, "216.txt");
                Thread.Sleep(5000);
               
                if (isRedirectionDone)
                {
                    matchedLines = HelperClass.ReadLine(@"..\..\..\216.txt", "canonical name");
                   
                    foreach (string output in matchedLines)
                    {
                        response = output.Split('=')[1].Trim();
                        
                        Site.CaptureRequirementIfAreEqual<string>(
                            publishDCAdapter.PDCFullName.ToLower(),
                            response.ToLower(), 
                            216,
                            @"If a server is a DC in forest Z, and its DSA GUID is G, then the server registers a CNAME 
                            record with Name field set to G._msdcs.Z.");

                        break;
                    }
                }
            }

            #endregion
        }


        /// <summary>
        /// Verify DNS discovery
        /// </summary>
        [TestMethod]
        [TestCategory("MS-ADTS-PublishDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("PDC")]
        public void PublishDC_DnsBasedDiscovery3()
        {
            //The tool will display "Invalid Parameter" if parameter is incorrect
            string invalidParam = "invalid parameter";

            #region R200
             
            // --> Requirements R200,201,204,207,211 are fall under same category.So we redirected output to a single File: 
            //    200_201_204_207_211.txt
            // -->The SRV field of SRV record
            //   To the "ExecuteCommand" Method we have provided -type=srv as an input which means nslookup will fetch 
            //   SRV records registered by server.
            response = HelperClass.ExecuteCommand(String.Concat("-type=srv _ldap._tcp.", publishDCAdapter.PrimaryDomainDnsName));
           
            if (response.ToLower() != "Invalid Parameter".ToLower())
            {
                isRedirectionDone = HelperClass.RedirectOutputToTextFile(response, "200_201_204_207_211.txt");
               
                if (isRedirectionDone)
                {
                    matchedLines = HelperClass.ReadLine(@"..\..\..\200_201_204_207_211.txt", "SRV");
                    
                    foreach (string output in matchedLines)
                    {
                        Site.CaptureRequirementIfIsTrue(
                            output.ToLower().Contains("SRV".ToLower()), 
                            200, 
                            "The SRV field of SRV record registered by a server is set to SRV.");

                        break;
                    }
                }
            }

            #endregion

            #region R201

            // To the "ExecuteCommand" Method we have provided -type=srv and SRV Record format i.e _ldap._tcp as an input
            // which means nslookup will fetch SRV records registered by server along with their properties.
            // Priority,Weight and Port fields are straight forward.We can able to see the values directly in the txt file.
            matchedLines = HelperClass.ReadLine(@"..\..\..\200_201_204_207_211.txt", "Priority");
           
            foreach (string output in matchedLines)
            {
                response = output.Split('=')[1].Trim();
               
                Site.CaptureRequirementIfAreEqual<string>(
                    "0", 
                    response, 
                    201, 
                    "The Priority field of SRV record registered by a server is set to 0");

                break;
            }

            #endregion

            #region R204

            // To the "ExecuteCommand" Method we have provided -type=srv and SRV Record format i.e _ldap._tcp as an input 
            // which means nslookup will fetch SRV records registered by server along with their properties.
            // Priority,Weight and Port fields are straight forward.We can able to see the values directly in the txt file.
            matchedLines = HelperClass.ReadLine(@"..\..\..\200_201_204_207_211.txt", "Weight");
           
            foreach (string output in matchedLines)
            {
                response = output.Split('=')[1].Trim();
                
                Site.CaptureRequirementIfAreEqual<string>(
                    "100", 
                    response, 
                    204, 
                    "The Weight field of SRV record registered by a server is set to 100");
               
                break;
            }

            #endregion

            #region R207

            // To the "ExecuteCommand" Method we have provided -type=srv and SRV Record format i.e _ldap._tcp as an input 
            // which means nslookup will fetch SRV records registered by server along with their properties.
            // Priority,Weight and Port fields are straight forward.We can able to see the values directly in the txt file.
            matchedLines = HelperClass.ReadLine(@"..\..\..\200_201_204_207_211.txt", "Port");
            
            foreach (string output in matchedLines)
            {
                response = output.Split('=')[1].Trim();
                
                Site.CaptureRequirementIfAreEqual<string>(
                    "389",
                    response,
                    207, 
                    "The Port field of SRV record registered by a server is set to 389 for LDAP service");
               
                break;
            }

            #endregion

            #region R211
           
            //-->To the "ExecuteCommand" Method we have provided -type=srv and SRV Record format i.e _ldap._tcp as 
            //   an input which means nslookup will fetch SRV records registered by server along with their properties.
            //   Target filed of SRV record:
            //   We worked on 2 to 3 DNS tools like dnsmgmt.msc and nslookup. The name(TARGET Field) is different with 
            //   different tools we used.
            //   We can replace svr hostname with the target field here which is nothing but fully qualified dns name.
            matchedLines = HelperClass.ReadLine(@"..\..\..\200_201_204_207_211.txt", "svr hostname");
            
            foreach (string output in matchedLines)
            {
                response = output.Split('=')[1].Trim();
               
                Site.CaptureRequirementIfIsTrue(
                    (response.ToLower().Contains(publishDCAdapter.PDCFullName.ToLower())
                    ||response.ToLower().Contains(publishDCAdapter.SDCFullName.ToLower())), 
                    211, 
                    @"The Target field of SRV record registered by a server is set to the fully qualified DNS name of
                    the server.");

                break;
            }

            #endregion

            #region R189
             
            // We have provided the command as per the TD and we are able to see the corresponding SRV records.
            // Please refer the log file for corresponding SRV records. The following are the list of commands we passed
            // as an input to ExecuteCommand Method.
            // _ldap_tcp.X
            // _ldap._tcp.Y._sites.X 
            // _ldap_tcp.dc._msdcs.X
            // _ldap._tcp.Y._sites.dc._msdcs.X
            // _ldap._tcp.G. domains._msdcs.Z
            // _kerberos._tcp.X
            // _kerberos._udp.X
            // _kerberos._tcp.Y._sites.X
            // _kerberos._tcp.dc._msdcs.X
            // _kerberos._tcp.Y._sites.dc._msdcs.X
            // _kpasswd._tcp.X
            // _kpasswd._udp.X
             
            // This requirement contains So many commands.So First command output we are redirecting to 189.txt.
            // After that we are appending the rest of the command outputs to 189.txt without writing them to different 
            // files ,because all these commands are fall under single requirement.
            
            // Here we need to verify The record structure is created or not mentioned above(_ldap_tcp.X).If we receive 
            // some ping information then the structure is created otherwise not.
            response = HelperClass.ExecuteCommand("-type=srv _ldap._tcp." + publishDCAdapter.PrimaryDomainDnsName);
           
            if (response.ToLower() != "Invalid Parameter".ToLower())
            {
                isRedirectionDone = HelperClass.RedirectOutputToTextFile(response, "189.txt");
            }

            response = HelperClass.ExecuteCommand(
                String.Concat("-type=srv _ldap._tcp.Default-First-Site-Name._sites.", publishDCAdapter.PrimaryDomainDnsName));

            if (response.ToLower() != "Invalid Parameter".ToLower())
            {
                isRedirectionDone = HelperClass.AppendOutputToTextFile(response, "189.txt");
            }

            response = HelperClass.ExecuteCommand(
                String.Concat("-type=srv _ldap_tcp.dc._msdcs.", publishDCAdapter.PrimaryDomainDnsName));

            if (response.ToLower() != "Invalid Parameter".ToLower())
            {
                isRedirectionDone = HelperClass.AppendOutputToTextFile(response, "189.txt");
            }

            response = HelperClass.ExecuteCommand(
                String.Concat("-type=srv _ldap._tcp.Default-First-Site-Name._sites.dc._msdcs.", publishDCAdapter.PrimaryDomainDnsName));

            if (response.ToLower() != "Invalid Parameter".ToLower())
            {
                isRedirectionDone = HelperClass.AppendOutputToTextFile(response, "189.txt");
            }

            response = HelperClass.ExecuteCommand(
                String.Concat("-type=srv _ldap._tcp.", publishDCAdapter.PrimaryDomainSrvGUID, ".domains._msdcs.", publishDCAdapter.PrimaryDomainDnsName));

            if (response.ToLower() != "Invalid Parameter".ToLower())
            {
                isRedirectionDone = HelperClass.AppendOutputToTextFile(response, "189.txt");
            }

            response = HelperClass.ExecuteCommand(String.Concat("-type=srv _kerberos._tcp.", publishDCAdapter.PrimaryDomainDnsName));
            
            if (response.ToLower() != "Invalid Parameter".ToLower())
            {
                isRedirectionDone = HelperClass.AppendOutputToTextFile(response, "189.txt");
            }

            response = HelperClass.ExecuteCommand(String.Concat("-type=srv _kerberos._udp.", publishDCAdapter.PrimaryDomainDnsName));
            
            if (response.ToLower() != "Invalid Parameter".ToLower())
            {
                isRedirectionDone = HelperClass.AppendOutputToTextFile(response, "189.txt");
            }

            response = HelperClass.ExecuteCommand(
                String.Concat("-type=srv _kerberos._tcp.Default-First-Site-Name._sites.", publishDCAdapter.PrimaryDomainDnsName));

            if (response.ToLower() != "Invalid Parameter".ToLower())
            {
                isRedirectionDone = HelperClass.AppendOutputToTextFile(response, "189.txt");
            }

            response = HelperClass.ExecuteCommand(
                String.Concat("-type=srv _kerberos._tcp.dc._msdcs.", publishDCAdapter.PrimaryDomainDnsName));

            if (response.ToLower() != "Invalid Parameter".ToLower())
            {
                isRedirectionDone = HelperClass.AppendOutputToTextFile(response, "189.txt");
            }

            response = HelperClass.ExecuteCommand(
                String.Concat("-type=srv _kerberos._tcp.Default-First-Site-Name._sites.dc._msdcs.", publishDCAdapter.PrimaryDomainDnsName));

            if (response.ToLower() != "Invalid Parameter".ToLower())
            {
                isRedirectionDone = HelperClass.AppendOutputToTextFile(response, "189.txt");
            }

            response = HelperClass.ExecuteCommand(String.Concat("-type=srv  _kpasswd._tcp.", publishDCAdapter.PrimaryDomainDnsName));
            
            if (response.ToLower() != "Invalid Parameter".ToLower())
            {
                isRedirectionDone = HelperClass.AppendOutputToTextFile(response, "189.txt");
            }

            response = HelperClass.ExecuteCommand(String.Concat("-type=srv  _kpasswd._udp." + publishDCAdapter.PrimaryDomainDnsName));
            
            if (response.ToLower() != "Invalid Parameter".ToLower())
            {
                isRedirectionDone = HelperClass.AppendOutputToTextFile(response, "189.txt");
            }

            // Actual Validation
            // Here the main verification is the structure is created or not as per the TD.That we have done by 
            // providing input to ExecuteCommand. Here we are verifying "svr hostname" variable because
            // it contais the SRV Records of a Domain Controller. Finally we will get Ping Information from DC.
            if (isRedirectionDone)
            {
                matchedLines = HelperClass.ReadLine(String.Concat(@"..\..\..\", "189.txt"), "svr hostname");
               
                foreach (string output in matchedLines)
                {
                    //The response in file is displayed like"x=y.z.com", we will get "y.z.com".
                    response = output.Split('=')[1].Trim();
                    
                    Site.CaptureRequirementIfIsTrue(
                        (response.ToLower().Contains(publishDCAdapter.PDCFullName.ToLower()) 
                        || response.ToLower().Contains(publishDCAdapter.SDCFullName.ToLower())
                        || response.ToLower().Contains(publishDCAdapter.RODCFullName.ToLower())), 
                        189,
                        @"If a server is a non-RODC with default NC X (and NC X's GUID is G) in site Y and in forest Z, then it registers SRV records with Service.Proto.Name equal to the following:
                        _ldap._tcp.X
                        _ldap._tcp.Y._sites.X 
                        _ldap._tcp.dc._msdcs.X
                        _ldap._tcp.Y._sites.dc._msdcs.X
                        _ldap._tcp.G. domains._msdcs.Z
                        _kerberos._tcp.X
                        _kerberos._udp.X
                        _kerberos._tcp.Y._sites.X
                        _kerberos._tcp.dc._msdcs.X
                        _kerberos._tcp.Y._sites.dc._msdcs.X
                        _kpasswd._tcp.X
                        _kpasswd._udp.X");

                    break;
                }
            }

            #endregion

            #region R10189

            if (publishDCAdapter.RODCNetbiosName != string.Empty)
            {
                // RODCRecordCorrect is the count parameter for response contains RODCName
                int RODCRecordCorrect = 0;
                 
                // We have provided the command as per the TD and we are able to see the corresponding SRV records.
                // Please refer the log file for corresponding SRV records. The following are the list of commands we 
                // passed as an input to ExecuteCommand Method.
                // _ldap._tcp.Y._sites.X
                // _ldap._tcp.Y._sites.dc._msdcs.X 
                // _kerberos._tcp.Y._sites.X 
                // _kerberos._tcp.Y._sites.dc._msdcs.X
             
                // This requirement contains So many commands.So First command output we are redirecting to a temp file.
                // After that we are appending the rest of the command outputs to the temp file without writing them to 
                // different files
                // because all these commands are fall under single requirement.
            
                // Here we need to verify The record structure is created or not mentioned above( _ldap._tcp.Y._sites.X).
                // If we receive some ping information 
                // then the structure is created otherwise not.
                
                // temporary file for storing response text
                string tmpFile10189 = "10189.txt";

                response = HelperClass.ExecuteCommand(
                            "-type=srv _ldap._tcp.Default-First-Site-Name._sites." 
                            + publishDCAdapter.PrimaryDomainDnsName);

                if (response.ToLower() != invalidParam)
                {
                    isRedirectionDone = HelperClass.RedirectOutputToTextFile(response, tmpFile10189);
                   
                    if (response.ToLower().Contains(publishDCAdapter.RODCFullName.ToLower()))
                    {
                        RODCRecordCorrect = RODCRecordCorrect + 1;
                    }
                }

                response = HelperClass.ExecuteCommand(
                    String.Concat("-type=srv _ldap._tcp.Default-First-Site-Name._sites.dc._msdcs.", publishDCAdapter.PrimaryDomainDnsName));
               
                if (response.ToLower() != invalidParam)
                {
                    isRedirectionDone = HelperClass.AppendOutputToTextFile(response, tmpFile10189);
                    
                    if (response.ToLower().Contains(publishDCAdapter.RODCFullName.ToLower()))
                    {
                        RODCRecordCorrect = RODCRecordCorrect + 1;
                    }
                }

                response = HelperClass.ExecuteCommand(
                    String.Concat("-type=srv _kerberos._tcp.Default-First-Site-Name._sites.", publishDCAdapter.PrimaryDomainDnsName));

                if (response.ToLower() != invalidParam)
                {
                    isRedirectionDone = HelperClass.AppendOutputToTextFile(response, tmpFile10189);
                    
                    if (response.ToLower().Contains(publishDCAdapter.RODCFullName.ToLower()))
                    {
                        RODCRecordCorrect = RODCRecordCorrect + 1;
                    }
                }

                response = HelperClass.ExecuteCommand(
                    String.Concat("-type=srv _kerberos._tcp.Default-First-Site-Name._sites.dc._msdcs.", publishDCAdapter.PrimaryDomainDnsName));

                if (response.ToLower() != invalidParam)
                {
                    isRedirectionDone = HelperClass.AppendOutputToTextFile(response, tmpFile10189);
                    
                    if (response.ToLower().Contains(publishDCAdapter.RODCFullName.ToLower()))
                    {
                        RODCRecordCorrect = RODCRecordCorrect + 1;
                    }
                }

                // Actual Validation
                // Here the main verification is the structure is created or not as per the TD.That we have done by 
                // Providing input to ExecuteCommand. Here we are verifying "svr hostname" variable because
                // It contais the SRV Records of a Domain Controller. Finally we will get Ping Information from DC.
                if (isRedirectionDone)
                {
                    // If all response have contained the "RODCName", the count parameter RODCRecordCorrect should equal 
                    // to 4.
                    //
                    // Verify MS-ADTS-PublishDC_R10189
                    //
                    Site.CaptureRequirementIfAreEqual<int>(
                        4,
                        RODCRecordCorrect,
                        10189,
                        @"[SRV Records Registered by a DC]If a server is an RODC with default NC X (and NC X's GUID is G)
                        in site Y and in forest Z, then it registers SRV records with Service.Proto.Name equal to the 
                        following: _ldap._tcp.Y._sites.X_ldap._tcp.Y._sites.dc._msdcs.X_kerberos._tcp.Y._sites.
                        X_kerberos._tcp.Y._sites.dc._msdcs.X");
                }
            }

            #endregion

            #region R190
           
            // We have provided the command as per the TD and we are able to see the corresponding SRV records.
            // Please refer the log file for corresponding SRV records. The following are the list of commands we passed
            // as an input to ExecuteCommand Method.
            // _ldap._tcp.gc._msdcs.Z
            // _ldap._tcp.Y._sites.gc._msdcs.Z
            // _gc._tcp.Z
            // _gc._tcp.Y._sites.Z
             
            // This requirement contains So many commands.So First command output we are redirecting to 190.txt.
            // After that we are appending the rest of the command outputs to 190.txt without writing them to different files
            // because all these commands are fall under single requirement.
             
            // Here we need to verify The record structure is created or not mentioned above(_ldap_tcp.gc._msdcs.Z).
            // If we receive some ping information 
            // then the structure is created otherwise not.
            response = HelperClass.ExecuteCommand(String.Concat("-type=srv _ldap._tcp.gc._msdcs.", publishDCAdapter.PrimaryDomainDnsName));
            
            if (response.ToLower() != "Invalid Parameter".ToLower())
            {
                isRedirectionDone = HelperClass.RedirectOutputToTextFile(response, "190.txt");
            }

            response = HelperClass.ExecuteCommand(
                String.Concat("-type=srv _ldap._tcp.Default-First-Site-Name._sites.gc._msdcs.", publishDCAdapter.PrimaryDomainDnsName));

            if (response.ToLower() != "Invalid Parameter".ToLower())
            {
                isRedirectionDone = HelperClass.AppendOutputToTextFile(response, "190.txt");
            }

            response = HelperClass.ExecuteCommand(String.Concat("-type=srv _gc._tcp.", publishDCAdapter.PrimaryDomainDnsName));
            
            if (response.ToLower() != "Invalid Parameter".ToLower())
            {
                isRedirectionDone = HelperClass.AppendOutputToTextFile(response, "190.txt");
            }

            response = HelperClass.ExecuteCommand
                (String.Concat("-type=srv _gc._tcp.Default-First-Site-Name._sites.", publishDCAdapter.PrimaryDomainDnsName));

            if (response.ToLower() != "Invalid Parameter".ToLower())
            {
                isRedirectionDone = HelperClass.AppendOutputToTextFile(response, "190.txt");
            }

            // Actual Validation
            // Here the main verification is the structure is created or not as per the TD.That we have done by 
            // Providing input to ExecuteCommand. Here we are verifying "svr hostname" variable because
            // It contais the SRV Records of a Domain Controller. Finally we will get Ping Information from DC.
            if (isRedirectionDone)
            {
                matchedLines = HelperClass.ReadLine(@"..\..\..\" + "190.txt", "svr hostname");
                
                foreach (string output in matchedLines)
                {
                    response = output.Split('=')[1].Trim();

                    //This validates that it is also a non-RODC GC server.
                    if (!response.ToLower().Contains(publishDCAdapter.RODCFullName.ToLower()))
                    {
                        Site.CaptureRequirementIfIsTrue(
                            (response.ToLower().Contains(publishDCAdapter.PDCFullName.ToLower())
                            || response.ToLower().Contains(publishDCAdapter.SDCFullName.ToLower())
                            || response.ToLower().Contains(publishDCAdapter.RODCFullName.ToLower())
                            || response.ToLower().Contains(publishDCAdapter.CDCFullName.ToLower())),
                            190, 
                            @"If the server is also a non-RODC GC server, then it registers SRV records with Service.
                             Proto.Name equal to the following:_ldap._tcp.gc._msdcs.Z _ldap._tcp.Y._sites.gc._msdcs.
                            Z _gc._tcp.Z _gc._tcp.Y._sites.Z");
                        break;
                    }
                }
            }

            #endregion

            #region R191

              // We have provided the command as per the TD and we are able to see the corresponding SRV records.
              // Please refer the log file for corresponding SRV records. The following are the list of commands we passed
              // as an input to ExecuteCommand Method.
              // _ldap._tcp.Y._sites.gc._msdcs.Z
              // _gc._tcp.Y._sites.Z
             
              // This requirement contains So many commands.So First command output we are redirecting to 191.txt.
              // After that we are appending the rest of the command outputs to 191.txt without writing them to different files
              // because all these commands are fall under single requirement.
            response = HelperClass.ExecuteCommand(
                String.Concat("-type=srv _ldap._tcp.Default-First-Site-Name._sites.gc._msdcs.", publishDCAdapter.PrimaryDomainDnsName));
            
            if (response.ToLower() != "Invalid Parameter".ToLower())
            {
                isRedirectionDone = HelperClass.RedirectOutputToTextFile(response, "191.txt");
            }

            response = HelperClass.ExecuteCommand(
                String.Concat("-type=srv _gc._tcp.Default-First-Site-Name._sites.", publishDCAdapter.PrimaryDomainDnsName));
            
            if (response.ToLower() != "Invalid Parameter".ToLower())
            {
                isRedirectionDone = HelperClass.AppendOutputToTextFile(response, "191.txt");
            }

            // Actual Validation
            // Here the main verification is the structure is created or not as per the TD.That we have done by 
            // providing input to ExecuteCommand. Here we are verifying "svr hostname" variable because
            // it contais the SRV Records of a Domain Controller. Finally we will get Ping Information from DC.
            if (isRedirectionDone)
            {
                matchedLines = HelperClass.ReadLine(@"..\..\..\" + "191.txt", "svr hostname");
                foreach (string output in matchedLines)
                {
                    response = output.Split('=')[1].Trim();
                    Site.CaptureRequirementIfIsTrue(
                        (response.ToLower().Contains(publishDCAdapter.PDCFullName.ToLower()) 
                        || response.ToLower().Contains(publishDCAdapter.SDCFullName.ToLower()) 
                        || response.ToLower().Contains(publishDCAdapter.RODCFullName.ToLower())
                        || response.ToLower().Contains(publishDCAdapter.CDCFullName.ToLower())), 
                        191, 
                        @"If the server is an RODC GC server, then it registers SRV records with Service.Proto.Name 
                        equal to the following:_ldap._tcp.Y._sites.gc._msdcs.Z _gc._tcp.Y._sites.Z");

                    break;
                }
            }

            #endregion

            #region R192

            // We have provided the command as per the TD and we are able to see the corresponding SRV records.
            // Please refer the log file for corresponding SRV records. The following are the list of commands we passed
            // as an input to ExecuteCommand Method.
            // _ldap._tcp.pdc._msdcs.X
            response = HelperClass.ExecuteCommand(String.Concat("-type=srv _ldap._tcp.pdc._msdcs.", publishDCAdapter.PrimaryDomainDnsName));
            
            if (response.ToLower() != "Invalid Parameter".ToLower())
            {
                isRedirectionDone = HelperClass.RedirectOutputToTextFile(response, "192.txt");

                // Actual Validation
                // Here the main verification is the structure is created or not as per the TD.That we have done by 
                // providing input to ExecuteCommand. Here we are verifying "svr hostname" variable because
                // it contais the SRV Records of a Domain Controller. Finally we will get Ping Information from DC.
                if (isRedirectionDone)
                {
                    matchedLines = HelperClass.ReadLine(@"..\..\..\192.txt", "svr hostname");
                   
                    foreach (string output in matchedLines)
                    {
                        response = output.Split('=')[1].Trim();
                        
                        Site.CaptureRequirementIfIsTrue(
                            response.ToLower().Contains(publishDCAdapter.PDCFullName.ToLower()), 
                            192, 
                            @"If the server is also the PDC of its default NC, then it registers SRV records with 
                            Service.Proto.Name equal to the following: _ldap._tcp.pdc._msdcs.X");

                        break;
                    }
                }
            }

            #endregion
        }

        
        /// <summary>
        /// Verify DNS discovery
        /// </summary>
        [TestMethod]
        [TestCategory("MS-ADTS-PublishDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("PDC")]
        public void PublishDC_DnsBasedDiscovery4()
        {
            #region R208

            // To the "ExecuteCommand" Method we have provided -type=srv and SRV Record format i.e _gc._tcp as 
            // an input which means nslookup will fetch SRV records registered by server along with their properties. 
            // Here the service name we are providing is gc instead of LDAP so that we will get GC Port: 3268
            Thread.Sleep(5000);
            response = HelperClass.ExecuteCommand(String.Concat("-type=srv _gc._tcp.", publishDCAdapter.PrimaryDomainDnsName));
            Thread.Sleep(5000);
            
            if (response.ToLower() != "Invalid Parameter".ToLower())
            {
                isRedirectionDone = HelperClass.RedirectOutputToTextFile(response, "208.txt");
                
                if (isRedirectionDone)
                {
                    matchedLines = HelperClass.ReadLine(@"..\..\..\208.txt", "Port");
                   
                    foreach (string output in matchedLines)
                    {
                        response = output.Split('=')[1].Trim();

                        Site.CaptureRequirementIfAreEqual<string>(
                            "3268",
                            response, 
                            208, 
                            "The Port field of SRV record registered by a server is set to 3268 for GC service.");

                        break;
                    }
                }
            }

            #endregion
        }


        /// <summary>
        /// Verify DNS discovery
        /// </summary>
        [TestMethod]
        [TestCategory("MS-ADTS-PublishDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("PDC")]
        public void PublishDC_DnsBasedDiscovery5()
        {
            #region R209

            // To the "ExecuteCommand" Method we have provided -type=srv and SRV Record format i.e _kerberos._tcp as 
            // an input which means nslookup will fetch SRV records registered by server along with their properties. 
            // Here the service name we are providing is _kerberos instead of LDAP,GC so that we will get a Kerberos 
            // service Port: 88
            Thread.Sleep(5000);
            response = HelperClass.ExecuteCommand(String.Concat("-type=srv _kerberos._tcp.", publishDCAdapter.PrimaryDomainDnsName));
            Thread.Sleep(10000);
            
            if (response.ToLower() != "Invalid Parameter".ToLower())
            {
                isRedirectionDone = HelperClass.RedirectOutputToTextFile(response, "209.txt");
                
                if (isRedirectionDone)
                {
                    matchedLines = HelperClass.ReadLine(@"..\..\..\209.txt", "Port");
                    
                    foreach (string output in matchedLines)
                    {
                        response = output.Split('=')[1].Trim();

                        Site.CaptureRequirementIfAreEqual<string>(
                            "88", 
                            response, 
                            209,
                            "The Port field of SRV record registered by a server is set to 88 for Kerberos KDC service.");

                        break;
                    }
                }
            }

            #endregion
        }


        /// <summary>
        /// Verify DNS discovery
        /// </summary>
        [TestMethod]
        [TestCategory("MS-ADTS-PublishDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("PDC")]
        public void PublishDC_DnsBasedDiscovery6()
        {
            #region R210

            // To the "ExecuteCommand" Method we have provided -type=srv and SRV Record format i.e _kpasswd._tcp as 
            // an input which means nslookup will fetch SRV records registered by server along with their properties. 
            // Here the service name we are providing is _kpasswd instead of LDAP,GC,_kerberos so that we will get
            // a Kerberos Password Change service Port: 464
            Thread.Sleep(5000);
            response = HelperClass.ExecuteCommand(String.Concat("-type=srv _kpasswd._tcp.", publishDCAdapter.PrimaryDomainDnsName));
            Thread.Sleep(5000);

            if (response.ToLower() != "Invalid Parameter".ToLower())
            {
                isRedirectionDone = HelperClass.RedirectOutputToTextFile(response, "210.txt");
                
                if (isRedirectionDone)
                {
                    matchedLines = HelperClass.ReadLine(@"..\..\..\210.txt", "Port");
                   
                    foreach (string output in matchedLines)
                    {
                        response = output.Split('=')[1].Trim();

                        Site.CaptureRequirementIfAreEqual<string>(
                            "464", 
                            response, 
                            210, 
                            @"The Port field of SRV record registered by a server is set to 464 for Kerberos Password
                            Change service.");

                        break;
                    }
                }
            }

            #endregion
        }

        
        /// <summary>
        /// Verify LDAP Ping
        /// </summary>
        [TestMethod]
        [TestCategory("MS-ADTS-PublishDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("PDC")]
        public void PublishDC_LDAPPing()
        {
            #region LocalVariables
            bool isLdapResultSuccess = false;
            #endregion

            //Connect and send an LDAP bind request to LDAP Server
            string connectStatus = ldapAdapter.ConnectAndBind(publishDCAdapter.PDCFullName,IPAddress.Parse(publishDCAdapter.PDCIPAddress),Int32.Parse(publishDCAdapter.ADDSPortNum),
                publishDCAdapter.DomainAdministratorName,publishDCAdapter.DomainUserPassword,publishDCAdapter.PrimaryDomainDnsName,AuthType.Kerberos);
            Assert.IsTrue(connectStatus.Contains("STATUS_SUCCESS"), "Bind response result should be LDAPResult_resultCode.success.");   

            #region MS-AD_LDAP_251,351,352,353
            response = ldapAdapter.SearchObject(publishDCAdapter.DomainNC,System.DirectoryServices.Protocols.SearchScope.Subtree,"(DnsDomain=\00)",
                new string[]{"Netlogon"}, null, out searchResponse);
            if (response.Contains("STATUS_SUCCESS"))
            {
                isLdapResultSuccess = true;
            }

            Site.CaptureRequirementIfIsTrue(
                (isLdapResultSuccess && searchResponse.Count == 0),
                251,
                @"Let reqDnsNC be the NC replica (full or partial) hosted by the server: If the filter includes 
                    the (DnsDomain=dnsDomain) clause, and if dnsDomain is empty, the server returns an LDAP 
                    SearchResultEntry with the following form: The ObjectName of the SearchResultEntry is NULL.
                    Attribute of SearchResultEntry is NULL. And LdapResult of SearchResultDone entry is set to 0 (success).");

            Site.CaptureRequirementIfIsTrue(
                (isLdapResultSuccess && searchResponse.Count == 0),
                351,
                @"If the filter is not syntactically valid for any of the cases , the server returns an LDAP 
                    SearchResultEntry with the ObjectName of the SearchResultEntry is NULL.");

            Site.CaptureRequirementIfIsTrue(
                (isLdapResultSuccess && searchResponse.Count == 0),
                352,
                @"If the filter is not syntactically valid for any of the cases , the server returns an LDAP 
                    SearchResultEntry with Attribute of SearchResultEntry is NULL.");

            Site.CaptureRequirementIfIsTrue(
                (isLdapResultSuccess && searchResponse.Count == 0),
                353,
                @"If the filter is not syntactically valid for any of the cases , the server returns an LDAP 
                    SearchResultEntry with LdapResult of SearchResultDone entry is set to 0 (success).");
            #endregion

            #region MS-AD_LDAP_252
            // Given the wrong DnsDomain name
            response = ldapAdapter.SearchObject(publishDCAdapter.DomainNC,System.DirectoryServices.Protocols.SearchScope.Subtree,"(DnsDomain=Wrong)",
                new string[]{"Netlogon"}, null, out searchResponse);
            if (response.Contains("STATUS_SUCCESS"))
            {
                isLdapResultSuccess = true;
            }

            Site.CaptureRequirementIfIsTrue(
                (isLdapResultSuccess && searchResponse.Count == 0),
                252,
                @"Let reqDnsNC be the NC replica (full or partial) hosted by the server: If the filter includes 
                    the (DnsDomain=dnsDomain) clause, and if there is no NC hosted by the server whose DNS name is 
                    dnsDomain, the server returns an LDAP SearchResultEntry with the following form: The ObjectName 
                    of the SearchResultEntry is NULL. Attribute of SearchResultEntry is NULL. And LdapResult of 
                    SearchResultDone entry is set to 0 (success).");
            #endregion

            #region MS-AD_LDAP_255
            response = ldapAdapter.SearchObject(publishDCAdapter.DomainNC,System.DirectoryServices.Protocols.SearchScope.Subtree,"(DomainGuid=Wrong)",
                new string[]{"Netlogon"}, null, out searchResponse);
            if (response.Contains("STATUS_SUCCESS"))
            {
                isLdapResultSuccess = true;
            }

            Site.CaptureRequirementIfIsTrue(
                (isLdapResultSuccess && searchResponse.Count == 0),
                    255,
                    @"Let reqGuidNC be the GUID of the NC replica (full or partial) hosted by the server: If the filter 
                    includes the (DomainGuid=domainGuid) clause, if domainGuid is not a valid GUID, the server returns 
                    an LDAP SearchResultEntry with the following form: The ObjectName of the SearchResultEntry is NULL. 
                    Attribute of SearchResultEntry is NULL. And LdapResult of SearchResultDone entry is set to 0 (success).");
            #endregion

            #region MS-AD_LDAP_256
            // Given the Correct Guid format But the guid is not the correct domain guid.
            response = ldapAdapter.SearchObject(publishDCAdapter.DomainNC,System.DirectoryServices.Protocols.SearchScope.Subtree,
                "(DomainGuid=\\3b\\b0\\21\\ca\\d3\\6d\\d1\\11\\8a\\7d\\b8\\df\\b1\\56\\87\\1f)",
                new string[]{"Netlogon"}, null, out searchResponse);
            if (response.Contains("STATUS_SUCCESS"))
            {
                isLdapResultSuccess = true;
            }

            Site.CaptureRequirementIfIsTrue(
                (isLdapResultSuccess && searchResponse.Count == 0),
                    256,
                    @"Let reqGuidNC be the GUID of the NC replica (full or partial) hosted by the server:
                    If the filter includes the (DomainGuid=domainGuid) clause, if there is no NC hosted by the server
                    whose Guid is domainGuid, the server returns an LDAP SearchResultEntry with the following form: 
                    The ObjectName of the SearchResultEntry is NULL. Attribute of SearchResultEntry is NULL. 
                    And LdapResult of SearchResultDone entry is set to 0 (success).");

            #endregion

            #region MS-AD_LDAP_259
            // Given the wrong Sid format. 
            response = ldapAdapter.SearchObject(publishDCAdapter.DomainNC,System.DirectoryServices.Protocols.SearchScope.Subtree,"(DomainSid=Wrong)",
                new string[]{"Netlogon"}, null, out searchResponse);
            if (response.Contains("STATUS_SUCCESS"))
            {
                isLdapResultSuccess = true;
            }

            Site.CaptureRequirementIfIsTrue(
                (isLdapResultSuccess && searchResponse.Count == 0),
                    259,
                    @"Let reqSidNC be the sid of the NC replica (full or partial) hosted by the server: 
                    If the filter includes the (DomainSid=domainSid) clause, if domainSid is not a valid sid, 
                    the server returns an LDAP SearchResultEntry with the following form: The ObjectName of the 
                    SearchResultEntry is NULL. Attribute of SearchResultEntry is NULL. And LdapResult of 
                    SearchResultDone entry is set to 0 (success).");
            #endregion

            #region MS-AD_LDAP_260
            // Given the correct Sid format but the Sid is wrong. 
            response = ldapAdapter.SearchObject(publishDCAdapter.DomainNC,System.DirectoryServices.Protocols.SearchScope.Subtree,
                "(DomainSid=S-1-5-21-4265316293-1957091001-45817728)",
                new string[]{"Netlogon"}, null, out searchResponse);
            if (response.Contains("STATUS_SUCCESS"))
            {
                isLdapResultSuccess = true;
            }

            Site.CaptureRequirementIfIsTrue(
                (isLdapResultSuccess && searchResponse.Count == 0),
                    260,
                    @"Let reqSidNC be the sid of the NC replica (full or partial) hosted by the server: 
                    If the filter includes the (DomainSid=domainSid) clause, if there is no NC hosted by the server 
                    whose Sid is domainSid, the server returns an LDAP SearchResultEntry with the following form: 
                    The ObjectName of the SearchResultEntry is NULL. Attribute of SearchResultEntry is NULL. 
                    And LdapResult of SearchResultDone entry is set to 0 (success).");
            #endregion

            #region MS-AD_LDAP_262
            // Given null values for reqGuidNC reqDnsNC
            response = ldapAdapter.SearchObject(publishDCAdapter.DomainNC, System.DirectoryServices.Protocols.SearchScope.Subtree, "(&(DnsDomain=\00)(DomainGuid=\00))",
                new string[]{"Netlogon"}, null, out searchResponse);
            if (response.Contains("STATUS_SUCCESS"))
            {
                isLdapResultSuccess = true;
            }

            Site.CaptureRequirementIfIsTrue(
                (isLdapResultSuccess && searchResponse.Count == 0),
                    262,
                    @"If both reqDnsNC and reqGuidNC are NULL, the server returns an LDAP SearchResultEntry with 
                    the following form:The ObjectName of the SearchResultEntry is NULL. Attribute of SearchResultEntry 
                    is NULL. And LdapResult of SearchResultDone entry is set to 0 (success).");
            #endregion

            #region MS-AD_LDAP_263
            string filterStr = String.Format("(&(DnsDomain={0})(DomainGuid=DomainGuid=\\3b\\b0\\21\\ca\\d3\\6d\\d1\\11\\8a\\7d\\b8\\df\\b1\\56\\87\\1f))",publishDCAdapter.PrimaryDomainDnsName);
            // Given reqGuidNC is not equal to the Guid of NC reqDnsNC. 
            response = ldapAdapter.SearchObject(publishDCAdapter.DomainNC,System.DirectoryServices.Protocols.SearchScope.Subtree,
                filterStr, new string[]{"Netlogon"}, null, out searchResponse);
            if (response.Contains("STATUS_SUCCESS"))
            {
                isLdapResultSuccess = true;
            }

            Site.CaptureRequirementIfIsTrue(
                (isLdapResultSuccess && searchResponse.Count == 0),
                    263,
                    @"When reqDnsNC and reqGuidNC are not NULL, if reqGuidNC is not equal to the Guid of NC reqDnsNC,
                    the server returns an LDAP SearchResultEntry with the following form:The ObjectName of 
                    the SearchResultEntry is NULL. Attribute of SearchResultEntry is NULL. And LdapResult of 
                    SearchResultDone entry is set to 0 (success).");
            #endregion

            #region MS-AD_LDAP_264
            filterStr = String.Format("(&(DnsDomain={0})(DomainSid=S-1-5-21-4265316293-1957091001-45817728))", publishDCAdapter.PrimaryDomainDnsName);
            response = ldapAdapter.SearchObject(publishDCAdapter.DomainNC,System.DirectoryServices.Protocols.SearchScope.Subtree,
                filterStr, new string[]{"Netlogon"}, null, out searchResponse);
            if (response.Contains("STATUS_SUCCESS"))
            {
                isLdapResultSuccess = true;
            }

            Site.CaptureRequirementIfIsTrue(
                (isLdapResultSuccess && searchResponse.Count == 0),
                    264,
                    @"When reqDnsNC and reqSidNC are not NULL, if reqSidNC is not equal to the Sid of NC reqDnsNC, 
                    the server returns an LDAP SearchResultEntry with the following form:The ObjectName of 
                    the SearchResultEntry is NULL. Attribute of SearchResultEntry is NULL. And LdapResult of 
                    SearchResultDone entry is set to 0 (success).");
            #endregion

            #region MS-AD_LDAP_265
            response = ldapAdapter.SearchObject(publishDCAdapter.DomainNC,System.DirectoryServices.Protocols.SearchScope.Subtree,
                "(&(DomainGuid=DomainGuid=\\3b\\b0\\21\\ca\\d3\\6d\\d1\\11\\8a\\7d\\b8\\df\\b1\\56\\87\\1f)(DomainSid=S-1-5-21-4265316293-1957091001-45817728))",
                new string[]{"Netlogon"}, null, out searchResponse);
            if (response.Contains("STATUS_SUCCESS"))
            {
                isLdapResultSuccess = true;
            }

            Site.CaptureRequirementIfIsTrue(
                (isLdapResultSuccess && searchResponse.Count == 0),
                    265,
                    @"When reqGuidNC and reqSidNC are not NULL, if reqSidNC is not equal to the Sid of NC reqGuidNC, 
                    the server returns an LDAP SearchResultEntry with the following form: The ObjectName 
                    of the SearchResultEntry is NULL. Attribute of SearchResultEntry is NULL. And LdapResult 
                    of SearchResultDone entry is set to 0 (success).");

            #endregion
        }

        public string GetResponseTriggerPath()
        {
            string baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
            string subPath = baseDirectory.Substring(0, baseDirectory.LastIndexOf("Batch"));
            string triggerPath = string.Format(@"{0}\Bin\ResponseTrigger.exe", subPath);
            if (!(new FileInfo(triggerPath)).Exists)
            {
                triggerPath = string.Format(@"C:\MicrosoftProtocolTests\ADFamily\Server-Endpoint\{0}\Bin\ResponseTrigger.exe", publishDCAdapter.GetInstalledTestSuiteVersion()); // use default path
            }

            return triggerPath;
        }
        #endregion
    }
}