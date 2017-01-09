// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Protocols.TestTools;
using Microsoft.Protocols.TestTools.StackSdk.ActiveDirectory.Drsr;

namespace Microsoft.Protocols.TestSuites.ActiveDirectory.Drsr
{
    [TestClass]
    public class DRSDCLocateTests : DrsrTestClassBase
    {
        #region Class Initialization and Cleanup
        [ClassInitialize]
        public static new void ClassInitialize(TestContext context)
        {
            DrsrTestClassBase.BaseInitialize(context);
        }

        [ClassCleanup]
        public static new void ClassCleanup()
        {
            DrsrTestClassBase.BaseCleanup();
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
        /// DRSDomainControllerInfo_InfoLevel_1_FQDN
        /// </summary>
        [BVT]
        [TestCategory("Win2003")]
        [ServerType(DcServerTypes.Any)]
        [SupportedADType(ADInstanceType.DS)]
        [Priority(0)]
        [Description("Verify the domain controller info of the given domain with info level 1. Domain name is a FQDN.")]
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-DRSR")]
        [TestMethod]
        public void DRSR_DRSDomainControllerInfo_InfoLevel_1_FQDN()
        {
            DrsrTestChecker.Check();
            uint ret = drsTestClient.DrsBind(EnvironmentConfig.Machine.WritableDC1, EnvironmentConfig.User.ParentDomainAdmin, DRS_EXTENSIONS_IN_FLAGS.DRS_EXT_BASE);
            BaseTestSite.Assert.AreEqual<uint>(0, ret, null);

            ret = drsTestClient.DrsDomainControllerInfo(EnvironmentConfig.Machine.WritableDC1, DRS_MSG_DCINFOREQ_Versions.V1, EnvironmentConfig.DomainStore[DomainEnum.PrimaryDomain].Name, DRS_MSG_DCINFOREQ_INFOLEVEL.LEVLE_1);
            BaseTestSite.Assert.AreEqual<uint>(0, ret, "IDL_DRSDomainControllerInfo: Checking return value - got: {0}, expect 0, should return 0 for a request for correct domain FQDN and info level 1");
        }

        /// <summary>
        /// DRSDomainControllerInfo_InfoLevel_2_NetBios
        /// </summary>
        [BVT]
        [TestCategory("Win2003")]
        [ServerType(DcServerTypes.Any)]
        [SupportedADType(ADInstanceType.DS)]
        [Priority(0)]
        [Description("Verify the domain controller info of the given domain with info level 2. Domain name is a NetBIOS.")]
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-DRSR")]
        [TestMethod]
        public void DRSR_DRSDomainControllerInfo_InfoLevel_2_NetBios()
        {
            DrsrTestChecker.Check();
            uint ret = drsTestClient.DrsBind(EnvironmentConfig.Machine.WritableDC1, EnvironmentConfig.User.ParentDomainAdmin, DRS_EXTENSIONS_IN_FLAGS.DRS_EXT_BASE);
            BaseTestSite.Assert.AreEqual<uint>(0, ret, null);

            ret = drsTestClient.DrsDomainControllerInfo(EnvironmentConfig.Machine.WritableDC1, DRS_MSG_DCINFOREQ_Versions.V1, EnvironmentConfig.DomainStore[DomainEnum.PrimaryDomain].NetbiosName, DRS_MSG_DCINFOREQ_INFOLEVEL.LEVEL_2);
            BaseTestSite.Assert.AreEqual<uint>(0, ret, "IDL_DRSDomainControllerInfo should return 0x0 for a request for correct domain Netbios name and info level 2");
        }

        /// <summary>
        /// DRSDomainControllerInfo_InfoLevel_3_DNSName
        /// </summary>
        [BVT]
        [TestCategory("Win2008")]
        [ServerType(DcServerTypes.Any)]
        [SupportedADType(ADInstanceType.DS)]
        [Priority(0)]
        [Description("Verify the domain controller info of the given domain with info level 3.Domain name is DNSName")]
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-DRSR")]
        [TestMethod]
        public void DRSR_DRSDomainControllerInfo_InfoLevel_3_DNSName()
        {
            DrsrTestChecker.Check();
            uint ret = drsTestClient.DrsBind(EnvironmentConfig.Machine.WritableDC1, EnvironmentConfig.User.ParentDomainAdmin, DRS_EXTENSIONS_IN_FLAGS.DRS_EXT_BASE);
            BaseTestSite.Assert.AreEqual<uint>(0, ret, null);

            ret = drsTestClient.DrsDomainControllerInfo(EnvironmentConfig.Machine.WritableDC1, DRS_MSG_DCINFOREQ_Versions.V1, EnvironmentConfig.DomainStore[DomainEnum.PrimaryDomain].DNSName, DRS_MSG_DCINFOREQ_INFOLEVEL.LEVEL_3);
            BaseTestSite.Assert.AreEqual<uint>(0, ret, "IDL_DRSDomainControllerInfo should return 0x0 for a request for correct domain DNS name and info level 3");
        }

        /// <summary>
        /// DRSDomainControllerInfo_InfoLevel_FFFFFFFF_FQDN
        /// </summary>
        [BVT]
        [TestCategory("Win2003")]
        [ServerType(DcServerTypes.Any)]
        [SupportedADType(ADInstanceType.DS)]
        [Priority(0)]
        [Description("Verify the domain controller info of the given domain with info level 0xffffffff. Domain name is a FQDN.")]
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-DRSR")]
        [TestMethod]
        public void DRSR_DRSDomainControllerInfo_InfoLevel_FFFFFFFF_FQDN()
        {
            DrsrTestChecker.Check();
            uint ret = drsTestClient.DrsBind(EnvironmentConfig.Machine.WritableDC1, EnvironmentConfig.User.ParentDomainAdmin, DRS_EXTENSIONS_IN_FLAGS.DRS_EXT_BASE);
            BaseTestSite.Assert.AreEqual<uint>(0, ret, null);

            ret = drsTestClient.DrsDomainControllerInfo(EnvironmentConfig.Machine.WritableDC1, DRS_MSG_DCINFOREQ_Versions.V1, EnvironmentConfig.DomainStore[DomainEnum.PrimaryDomain].Name, DRS_MSG_DCINFOREQ_INFOLEVEL.LEVEL_F);
            BaseTestSite.Assert.AreEqual<uint>(0, ret, "IDL_DRSDomainControllerInfo should return 0x0 for a request for correct domain FQDN and info level 0xffffffff");
        }

        [BVT]
        [TestCategory("Win2003")]
        [ServerType(DcServerTypes.Any)]
        [SupportedADType(ADInstanceType.DS)]
        [RequireDcPartner]
        [Priority(0)]
        [Description("Query cost from site1 to site2 with correct request")]
        [TestCategory("SDC")]
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-DRSR")]
        [TestMethod]
        public void DRSR_DRSQuerySitesByCost_From_Site1_To_Site2()
        {
            DrsrTestChecker.Check();
            DsSite[] dest = new DsSite[1];
            dest[0] = ((DsServer)EnvironmentConfig.MachineStore[EnvironmentConfig.Machine.WritableDC2]).Site;
            uint ret = drsTestClient.DrsBind(EnvironmentConfig.Machine.WritableDC1, EnvironmentConfig.User.ParentDomainAdmin, DRS_EXTENSIONS_IN_FLAGS.DRS_EXT_BASE);
            BaseTestSite.Assert.AreEqual<uint>(0, ret, null);

            ret = drsTestClient.DrsQuerySitesByCost(EnvironmentConfig.Machine.WritableDC1, DRS_MSG_QUERYSITESREQ_Versions.V1, ((DsServer)EnvironmentConfig.MachineStore[EnvironmentConfig.Machine.WritableDC1]).Site, dest);
            BaseTestSite.Assert.AreEqual<uint>(0, ret, "IDL_DRSQuerySItesByCost: Checking return value - got: {0}, expect: 0, should return 0x0 for a request for correct request", ret);
        }

        [BVT]
        [TestCategory("Win2003")]
        [ServerType(DcServerTypes.Any)]
        [SupportedADType(ADInstanceType.DS)]
        [Priority(0)]
        [RequireDcPartner]
        [Description("Query cost from site1 to site1 and site2 with correct request")]
        [TestCategory("SDC")]
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-DRSR")]
        [TestMethod]
        public void DRSR_DRSQuerySitesByCost_From_Site1_To_Site1_and_Site2()
        {
            DrsrTestChecker.Check();
            DsSite[] dest = new DsSite[2];
            dest[0] = ((DsServer)EnvironmentConfig.MachineStore[EnvironmentConfig.Machine.WritableDC1]).Site;
            dest[1] = ((DsServer)EnvironmentConfig.MachineStore[EnvironmentConfig.Machine.WritableDC2]).Site;
            uint ret = drsTestClient.DrsBind(EnvironmentConfig.Machine.WritableDC1, EnvironmentConfig.User.ParentDomainAdmin, DRS_EXTENSIONS_IN_FLAGS.DRS_EXT_BASE);
            BaseTestSite.Assert.AreEqual<uint>(0, ret, null);

            ret = drsTestClient.DrsQuerySitesByCost(EnvironmentConfig.Machine.WritableDC1, DRS_MSG_QUERYSITESREQ_Versions.V1, dest[0], dest);
            BaseTestSite.Assert.AreEqual<uint>(0, ret, "IDL_DRSQuerySItesByCost should return 0x0 for a request for correct request");
        }
    }


}
