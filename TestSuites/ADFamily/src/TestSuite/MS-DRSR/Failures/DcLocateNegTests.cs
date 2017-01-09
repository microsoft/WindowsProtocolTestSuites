// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Protocols.TestTools;
using Microsoft.Protocols.TestTools.StackSdk;
using Microsoft.Protocols.TestTools.StackSdk.ActiveDirectory.Drsr;
namespace Microsoft.Protocols.TestSuites.ActiveDirectory.Drsr
{
    [TestClass]
    public class DcLocateNegTests : DrsrFailureTestClassBase
    {
        #region Class Initialization and Cleanup
        [ClassInitialize]
        public static new void ClassInitialize(TestContext context)
        {
            DrsrFailureTestClassBase.BaseInitialize(context);
        }

        [ClassCleanup]
        public static new void ClassCleanup()
        {
            DrsrFailureTestClassBase.BaseCleanup();
        }
        #endregion

        #region Test Initialization and Cleanup
        protected override void TestInitialize()
        {
            base.TestInitialize();
        }

        protected override void TestCleanup()
        {
            base.TestCleanup();
        }
        #endregion
        /// <summary>
        /// Test DRSQuerySitesByCost by setting cToSites to 1 and rgszToSites to NULL
        /// </summary>
        [TestCategory("Win2003")]
        [ServerType(DcServerTypes.Any)]
        [SupportedADType(ADInstanceType.DS)]
        [RequireDcPartner]
        [Priority(2)]
        [Description("send a request with cToSites = 1, rgszToSites = null to the server, and the server will response ERROR_INVALID_PARAMETER ")]
        [TestCategory("SDC")]
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-DRSR")]
        [TestMethod]
        public void DRSR_DRSQuerySitesByCost_cToSites_1_And_rgszToSites_NULL()
        {
            DrsrTestChecker.Check();
            DsSite[] dest = new DsSite[1];
            dest[0] = ((DsServer)EnvironmentConfig.MachineStore[EnvironmentConfig.Machine.WritableDC2]).Site;
            uint ret = drsTestClient.DrsBind(EnvironmentConfig.Machine.WritableDC1, EnvironmentConfig.User.ParentDomainAdmin, DRS_EXTENSIONS_IN_FLAGS.DRS_EXT_BASE);
            BaseTestSite.Assert.AreEqual<uint>(0, ret, null);

            DRS_MSG_QUERYSITESREQ request = drsTestClient.CreateDRS_MSG_QUERYSITESREQ(
                ((DsServer)EnvironmentConfig.MachineStore[EnvironmentConfig.Machine.WritableDC1]).Site.CN,
                1,       //cToSites: Invalid value
                null     //rgszToSites: Invalid value
                );

            uint? outVer;
            DRS_MSG_QUERYSITESREPLY? reply = null;

            BaseTestSite.Log.Add(LogEntryKind.Checkpoint, "Begin to call IDL_DRSQuerySitesByCost");

            ret = drsTestClient.DRSClient.DrsQuerySitesByCost(
                EnvironmentConfig.DrsContextStore[EnvironmentConfig.Machine.WritableDC1],
                (uint)DRS_MSG_QUERYSITESREQ_Versions.V1,
                request,
                out outVer,
                out reply);

            BaseTestSite.Log.Add(LogEntryKind.Checkpoint, "End call IDL_DRSQuerySitesByCost with return value " + ret);
            BaseTestSite.Assert.AreEqual<uint>((uint)Win32ErrorCode_32.ERROR_INVALID_PARAMETER, ret, "IDL_DRSQuerySitesByCost should return ERROR_INVALID_PARAMETER for a request with cToSites = 1, rgszToSites = null");
        }

        /// <summary>
        /// Test DRSQuerySitesByCost with an invalid fromSite
        /// </summary>
        [TestCategory("Win2003")]
        [ServerType(DcServerTypes.Any)]
        [SupportedADType(ADInstanceType.DS)]
        [RequireDcPartner]
        [Priority(2)]
        [Description("send a request with fromSite invalid to the server, and the server will response ERROR_DS_OBJ_NOT_FOUND")]
        [TestCategory("SDC")]
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-DRSR")]
        [TestMethod]
        public void DRSR_DRSQuerySitesByCost_fromSite_Invalid()
        {
            DrsrTestChecker.Check();
            DsSite[] dest = new DsSite[1];
            dest[0] = ((DsServer)EnvironmentConfig.MachineStore[EnvironmentConfig.Machine.WritableDC2]).Site;
            uint ret = drsTestClient.DrsBind(EnvironmentConfig.Machine.WritableDC1, EnvironmentConfig.User.ParentDomainAdmin, DRS_EXTENSIONS_IN_FLAGS.DRS_EXT_BASE);
            BaseTestSite.Assert.AreEqual<uint>(0, ret, null);

            string[] sites = new string[dest.Length];
            for (int i = 0; i < dest.Length; i++)
            {
                sites[i] = dest[i].CN;
            }
            DRS_MSG_QUERYSITESREQ request = drsTestClient.CreateDRS_MSG_QUERYSITESREQ(
               "InvalidFromSite",    //fromSite: Invalid value
               (uint)dest.Length,
               sites
               );

            uint? outVer;
            DRS_MSG_QUERYSITESREPLY? reply = null;

            BaseTestSite.Log.Add(LogEntryKind.Checkpoint, "Begin to call IDL_DRSQuerySitesByCost");

            ret = drsTestClient.DRSClient.DrsQuerySitesByCost(
                EnvironmentConfig.DrsContextStore[EnvironmentConfig.Machine.WritableDC1],
                (uint)DRS_MSG_QUERYSITESREQ_Versions.V1,
                request,
                out outVer,
                out reply);

            BaseTestSite.Log.Add(LogEntryKind.Checkpoint, "End call IDL_DRSQuerySitesByCost with return value " + ret);
            BaseTestSite.Assert.AreEqual<uint>((uint)Win32ErrorCode_32.ERROR_DS_OBJ_NOT_FOUND, ret, "IDL_DRSQuerySitesByCost should return ERROR_DS_OBJ_NOT_FOUND for a request with fromSite invalid");
        }
        /// <summary>
        /// Test DRSQuerySitesByCost with toSite Invalid
        /// </summary>
        [TestCategory("Win2003")]
        [ServerType(DcServerTypes.Any)]
        [SupportedADType(ADInstanceType.DS)]
        [RequireDcPartner]
        [Priority(2)]
        [Description("Test DRSQuerySitesByCost with toSite Invalid and the server will response ERROR_DS_OBJ_NOT_FOUND")]
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-DRSR")]
        [TestMethod]
        public void DRSR_DRSQuerySitesByCost_toSite_Invalid()
        {
            DrsrTestChecker.Check();

            uint ret = drsTestClient.DrsBind(EnvironmentConfig.Machine.WritableDC1, EnvironmentConfig.User.ParentDomainAdmin, DRS_EXTENSIONS_IN_FLAGS.DRS_EXT_BASE);
            BaseTestSite.Assert.AreEqual<uint>(0, ret, null);
            string[] destStr = { "abc", "ddd" }; //toSite: Invalid Site
            DRS_MSG_QUERYSITESREQ request = drsTestClient.CreateDRS_MSG_QUERYSITESREQ(
               ((DsServer)EnvironmentConfig.MachineStore[EnvironmentConfig.Machine.WritableDC1]).Site.CN,
               (uint)destStr.Length,
               destStr
               );

            uint? outVer;
            DRS_MSG_QUERYSITESREPLY? reply = null;

            BaseTestSite.Log.Add(LogEntryKind.Checkpoint, "Begin to call IDL_DRSQuerySitesByCost");

            ret = drsTestClient.DRSClient.DrsQuerySitesByCost(
                EnvironmentConfig.DrsContextStore[EnvironmentConfig.Machine.WritableDC1],
                (uint)DRS_MSG_QUERYSITESREQ_Versions.V1,
                request,
                out outVer,
                out reply);

            BaseTestSite.Log.Add(LogEntryKind.Checkpoint, "End call IDL_DRSQuerySitesByCost with return value " + ret);

            BaseTestSite.Assert.AreEqual<uint>(0, ret, null);
            if (ret == 0)
            {
                if (reply.Value.V1.rgCostInfo != null)
                {
                    foreach (DRS_MSG_QUERYSITESREPLYELEMENT_V1 ele in reply.Value.V1.rgCostInfo)
                    {   // chech whether there are error in a success response
                        BaseTestSite.Assert.AreEqual<uint>((uint)Win32ErrorCode_32.ERROR_DS_OBJ_NOT_FOUND, ele.dwErrorCode, "IDL_DRSQuerySitesByCost should return ERROR_DS_OBJ_NOT_FOUND for a request with toSite invalid and dwErrorCode in DRS_MSG_QUERYSITESREPLYELEMENT_V1 should be set 0");
                    }
                }
            }
        }
        /// <summary>
        /// Verify the domain controller info of the given domain with info level 0xFFFFFFFF and not administrator account credential. 
        /// </summary>
        [TestCategory("Win2003")]
        [ServerType(DcServerTypes.Any)]
        [SupportedADType(ADInstanceType.DS)]
        [Priority(2)]
        [Description("Verify the domain controller info of the given domain with info level 0xFFFFFFFF and not administrator account credential. Domain name is a FQDN.")]
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-DRSR")]
        [TestMethod]
        public void DRSR_DRSDomainControllerInfo_InfoLevel_FFFFFFFF_NonAdmin_FQDN()
        {
            DrsrTestChecker.Check();
            // Use EnvironmentConfig.User.ParentDomainUser that is a normal user without admin rights
            uint ret = drsTestClient.DrsBind(EnvironmentConfig.Machine.WritableDC1, EnvironmentConfig.User.ParentDomainUser, DRS_EXTENSIONS_IN_FLAGS.DRS_EXT_BASE);
            BaseTestSite.Assert.AreEqual<uint>(0, ret, null);

            ret = drsTestClient.DrsDomainControllerInfo(EnvironmentConfig.Machine.WritableDC1, DRS_MSG_DCINFOREQ_Versions.V1, EnvironmentConfig.DomainStore[DomainEnum.PrimaryDomain].Name, DRS_MSG_DCINFOREQ_INFOLEVEL.LEVEL_F);
            BaseTestSite.Assert.AreEqual<uint>((uint)Win32ErrorCode_32.ERROR_ACCESS_DENIED, ret, "IDL_DRSDomainControllerInfo should return ERROR_ACCESS_DENIED for a request for correct domain FQDN  0xFFFFFFFF and not administrator account credential");
        }
        /// <summary>
        /// Verify a request for a response of an invalid format domain name
        /// </summary>
        [TestCategory("Win2003")]
        [ServerType(DcServerTypes.Any)]
        [SupportedADType(ADInstanceType.DS)]
        [Priority(2)]
        [Description("Verify a request for a response of an invalid format domain name")]
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-DRSR")]
        [TestMethod]
        public void DRSR_DRSDomainControllerInfo_InfoLevel_1_Invalid_Format_DN()
        {
            DrsrTestChecker.Check();
            uint ret = drsTestClient.DrsBind(EnvironmentConfig.Machine.WritableDC1, EnvironmentConfig.User.ParentDomainAdmin, DRS_EXTENSIONS_IN_FLAGS.DRS_EXT_BASE);
            BaseTestSite.Assert.AreEqual<uint>(0, ret, null);

            String invalidFormatDN = null;  // invalid format domain name
            ret = drsTestClient.DrsDomainControllerInfo(EnvironmentConfig.Machine.WritableDC1, DRS_MSG_DCINFOREQ_Versions.V1, invalidFormatDN, DRS_MSG_DCINFOREQ_INFOLEVEL.LEVLE_1);
            BaseTestSite.Assert.AreEqual<uint>((uint)Win32ErrorCode_32.ERROR_INVALID_PARAMETER, ret, "IDL_DRSDomainControllerInfo should return ERROR_INVALID_PARAMETER for a request for invalid format domain name");
        }
        /// <summary>
        /// Verify a request for a response of an invalid domain name
        /// </summary>
        [TestCategory("Win2003")]
        [ServerType(DcServerTypes.Any)]
        [SupportedADType(ADInstanceType.DS)]
        [Priority(2)]
        [Description("Verify a request for a response of an invalid domain name")]
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-DRSR")]
        [TestMethod]
        public void DRSR_DRSDomainControllerInfo_InfoLevel_1_Invalid_DN()
        {
            DrsrTestChecker.Check();
            uint ret = drsTestClient.DrsBind(EnvironmentConfig.Machine.WritableDC1, EnvironmentConfig.User.ParentDomainAdmin, DRS_EXTENSIONS_IN_FLAGS.DRS_EXT_BASE);
            BaseTestSite.Assert.AreEqual<uint>(0, ret, null);

            String invalidDN = EnvironmentConfig.InvalidDomainDSDNSName; //an invalid domain name
            ret = drsTestClient.DrsDomainControllerInfo(EnvironmentConfig.Machine.WritableDC1, DRS_MSG_DCINFOREQ_Versions.V1, invalidDN, DRS_MSG_DCINFOREQ_INFOLEVEL.LEVLE_1);
            BaseTestSite.Assert.AreEqual<uint>((uint)Win32ErrorCode_32.ERROR_DS_OBJ_NOT_FOUND, ret, "IDL_DRSDomainControllerInfo should return ERROR_DS_OBJ_NOT_FOUND for a request for invalid domain name");
        }
        /// <summary>
        /// child domain name when call parent domain DC, the server will response ERROR_INVALID_PARAMETER 
        /// </summary>
        [TestCategory("Win2003")]
        [ServerType(DcServerTypes.Any)]
        [SupportedADType(ADInstanceType.DS)]
        [Ignore]
        [Priority(2)]
        [Description("child domain name when call parent domain DC, the server will response ERROR_INVALID_PARAMETER ")]
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-DRSR")]
        [TestMethod]
        public void DRSR_DRSDomainControllerInfo_InfoLevel_1_ChildDN_FQDN()
        {
            DrsrTestChecker.Check();
            uint ret = drsTestClient.DrsBind(EnvironmentConfig.Machine.WritableDC1, EnvironmentConfig.User.ParentDomainAdmin, DRS_EXTENSIONS_IN_FLAGS.DRS_EXT_BASE);
            BaseTestSite.Assert.AreEqual<uint>(0, ret, null);
            // child domain name: EnvironmentConfig.DomainStore[DomainEnum.ChildDomain].Name and parent domain DC: EnvironmentConfig.Machine.WritableDC1
            ret = drsTestClient.DrsDomainControllerInfo(EnvironmentConfig.Machine.WritableDC1, DRS_MSG_DCINFOREQ_Versions.V1, EnvironmentConfig.DomainStore[DomainEnum.ChildDomain].Name, DRS_MSG_DCINFOREQ_INFOLEVEL.LEVLE_1);
            BaseTestSite.Assert.AreEqual<uint>((uint)Win32ErrorCode_32.ERROR_INVALID_PARAMETER, ret, "IDL_DRSDomainControllerInfo should return ERROR_INVALID_PARAMETER for a request for correct domain FQDN and info level 1");
        }
    }
}
