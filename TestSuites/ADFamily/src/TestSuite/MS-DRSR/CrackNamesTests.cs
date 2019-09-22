// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.DirectoryServices.Protocols;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using Microsoft.Protocols.TestTools;
using Microsoft.Protocols.TestTools.StackSdk.ActiveDirectory.Drsr;
namespace Microsoft.Protocols.TestSuites.ActiveDirectory.Drsr
{
    [TestClass]
    public class CrackNamesTests : DrsrTestClassBase
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

        #region IDL_DRSCrackNames
        #region DRSCrackNames_With_GC
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-DRSR")]
        [TestMethod]
        [Priority(0)]
        [BVT]
        [SupportedADType(ADInstanceType.DS)]
        [TestCategory("Win2003")]
        [Description("Calls IDL_DRSCrackNames with DRS bound to a GC server and GC bit in dwFlags of DRS_MSG_CRACKREQ_V1 is set.")]
        public void DRSR_DRSCrackNames_With_GC()
        {
            DrsrTestChecker.Check();
            // Init the data.
            EnvironmentConfig.Machine srv = EnvironmentConfig.Machine.WritableDC1;
            DsServer server = (DsServer)EnvironmentConfig.MachineStore[srv];
            DsUser user = EnvironmentConfig.UserStore[EnvironmentConfig.User.ParentDomainAdmin];

            uint ret = 0;
            ret = drsTestClient.DrsBind(
                srv,
                EnvironmentConfig.User.ParentDomainAdmin,
                DRS_EXTENSIONS_IN_FLAGS.DRS_EXT_BASE
                );

            BaseTestSite.Assert.AreEqual<uint>(
                0,
                ret,
                "IDL_DRSBind: should return 0 with a success bind to DC");

            // Create the request
            // CrackNames doesn't allow empty names, so we create a dummy one.
            string[] dummyName = new string[] { "dummy" };

            ret = drsTestClient.DrsCrackNames(
                srv,
                DRS_MSG_CRACKREQ_FLAGS.DS_NAME_FLAG_GC_VERIFY,
                (uint)DS_NAME_FORMAT.DS_INVALID_NAME,
                (uint)DS_NAME_FORMAT.DS_INVALID_NAME,
                dummyName);

            BaseTestSite.Assert.AreEqual<uint>(
                0,
                ret,
                "IDL_DRSCrackNames: return value should be 0"
                );

            // Unbind
            ret = drsTestClient.DrsUnbind(srv);
            BaseTestSite.Assert.AreEqual<uint>(
                0,
                ret,
                "IDL_DRSUnbind: return value should be 0");
        }
        #endregion

        #region DRSCrackNames_List_All_Sites
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-DRSR")]
        [TestMethod]
        [Priority(0)]
        [BVT]
        [TestCategory("Win2003")]
        [Description("Calls IDL_DRSCrackNames to list all sites in the current forest. By `current', we mean the forest in which the server DC is located.")]
        public void DRSR_DRSCrackNames_List_All_Sites()
        {
            DrsrTestChecker.Check();
            // Init the data.
            EnvironmentConfig.Machine srv = EnvironmentConfig.Machine.WritableDC1;
            DsServer server = (DsServer)EnvironmentConfig.MachineStore[srv];
            DsUser user = EnvironmentConfig.UserStore[EnvironmentConfig.User.ParentDomainAdmin];

            uint ret = 0;
            ret = drsTestClient.DrsBind(
                srv,
                EnvironmentConfig.User.ParentDomainAdmin,
                DRS_EXTENSIONS_IN_FLAGS.DRS_EXT_BASE
                );

            BaseTestSite.Assert.AreEqual<uint>(
                0, ret,
                "IDL_DRSBind: Checking return value - got: {0}, expect: {1}, return value should always be 0 with a success bind to DC",
                ret, 0);
            // Create the request
            // CrackNames doesn't allow empty names, so we create a dummy one.
            string[] dummyName = new string[] { "dummy" };

            ret = drsTestClient.DrsCrackNames(
                srv,
                DRS_MSG_CRACKREQ_FLAGS.NONE,
                (uint)formatOffered_Values.DS_LIST_SITES,
                (uint)DS_NAME_FORMAT.DS_INVALID_NAME,
                dummyName);

            BaseTestSite.Assert.AreEqual<uint>(0, ret,
                "IDL_DRSCrackNames: Checking return value - got: {0}, expect: {1}, return value should always be 0",
                ret, 0);
            // Unbind
            ret = drsTestClient.DrsUnbind(srv);

            BaseTestSite.Assert.AreEqual<uint>(0, ret,
                "IDL_DRSUnbind: Checking return value - got: {0}, expect: {1}, return value should always be 0",
                ret, 0);
        }
        #endregion

        #region DRSCrackNames_List_Servers_In_Site
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-DRSR")]
        [TestMethod]
        [Priority(0)]
        [TestCategory("Win2003")]
        [Description("Calls IDL_DRSCrackNames to list all servers in a given site.")]
        public void DRSR_DRSCrackNames_List_Servers_In_Site()
        {
            DrsrTestChecker.Check();
            // Init the data.
            EnvironmentConfig.Machine srv = EnvironmentConfig.Machine.WritableDC1;
            DsServer server = (DsServer)EnvironmentConfig.MachineStore[srv];
            DsUser user = EnvironmentConfig.UserStore[EnvironmentConfig.User.ParentDomainAdmin];

            uint ret = 0;
            ret = drsTestClient.DrsBind(
                srv,
                EnvironmentConfig.User.ParentDomainAdmin,
                DRS_EXTENSIONS_IN_FLAGS.DRS_EXT_BASE
                );

            BaseTestSite.Assert.AreEqual<uint>(
                0,
                ret,
                "IDL_DRSBind: should return 0 with a success bind to DC");

            // Create the request
            // We list the servers in the site where the DrsBind-ed server is located.
            string siteDn = server.Site.DN;

            ret = drsTestClient.DrsCrackNames(
                srv,
                DRS_MSG_CRACKREQ_FLAGS.NONE,
                (uint)formatOffered_Values.DS_LIST_SERVERS_IN_SITE,
                (uint)DS_NAME_FORMAT.DS_INVALID_NAME,
                new string[] { siteDn });

            BaseTestSite.Assert.AreEqual<uint>(
                0,
                ret,
                "IDL_DRSCrackNames: return value should be 0"
                );

            // Unbind
            ret = drsTestClient.DrsUnbind(srv);
            BaseTestSite.Assert.AreEqual<uint>(
                0,
                ret,
                "IDL_DRSUnbind: return value should be 0");
        }
        #endregion

        #region DRSCrackNames_List_Domains_In_Site
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-DRSR")]
        [TestMethod]
        [Priority(0)]
        [TestCategory("Win2003")]
        [Description("Calls IDL_DRSCrackNames to list all domains in a given site.")]
        public void DRSR_DRSCrackNames_List_Domains_In_Site()
        {
            DrsrTestChecker.Check();
            // Init the data.
            EnvironmentConfig.Machine srv = EnvironmentConfig.Machine.WritableDC1;
            DsServer server = (DsServer)EnvironmentConfig.MachineStore[srv];
            DsUser user = EnvironmentConfig.UserStore[EnvironmentConfig.User.ParentDomainAdmin];

            uint ret = 0;
            ret = drsTestClient.DrsBind(
                srv,
                EnvironmentConfig.User.ParentDomainAdmin,
                DRS_EXTENSIONS_IN_FLAGS.DRS_EXT_BASE
                );

            BaseTestSite.Assert.AreEqual<uint>(
                0,
                ret,
                "IDL_DRSBind: should return 0 with a success bind to DC");

            // Create the request
            // We list the domains in the site where the DrsBind-ed server is located.
            string siteDn = server.Site.DN;

            ret = drsTestClient.DrsCrackNames(
                srv,
                DRS_MSG_CRACKREQ_FLAGS.NONE,
                (uint)formatOffered_Values.DS_LIST_DOMAINS_IN_SITE,
                (uint)DS_NAME_FORMAT.DS_INVALID_NAME,
                new string[] { siteDn });

            BaseTestSite.Assert.AreEqual<uint>(
                0,
                ret,
                "IDL_DRSCrackNames: return value should be 0"
                );

            // Unbind
            ret = drsTestClient.DrsUnbind(srv);
            BaseTestSite.Assert.AreEqual<uint>(
                0,
                ret,
                "IDL_DRSUnbind: return value should be 0");
        }
        #endregion

        #region DRSCrackNames_List_Servers_For_Domain_In_Site
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-DRSR")]
        [TestMethod]
        [Priority(0)]
        [SupportedADType(ADInstanceType.DS)]
        [TestCategory("Win2003")]
        [Description("Get all DCs of a specified domain in a given site.")]
        public void DRSR_DRSCrackNames_List_Servers_For_Domain_In_Site()
        {
            DrsrTestChecker.Check();
            // Init the data.
            EnvironmentConfig.Machine srv = EnvironmentConfig.Machine.WritableDC1;
            DsServer server = (DsServer)EnvironmentConfig.MachineStore[srv];
            DsUser user = EnvironmentConfig.UserStore[EnvironmentConfig.User.ParentDomainAdmin];

            uint ret = 0;
            ret = drsTestClient.DrsBind(
                srv,
                EnvironmentConfig.User.ParentDomainAdmin,
                DRS_EXTENSIONS_IN_FLAGS.DRS_EXT_BASE
                );

            BaseTestSite.Assert.AreEqual<uint>(
                0,
                ret,
                "IDL_DRSBind: should return 0 with a success bind to DC");

            // Create the request
            // We list the servers in the domain 
            // and the site where the DrsBind-ed server is located.
            string siteDn = server.Site.DN;
            string domainDn = server.Domain.Name;

            ret = drsTestClient.DrsCrackNames(
                srv,
                DRS_MSG_CRACKREQ_FLAGS.NONE,
                (uint)formatOffered_Values.DS_LIST_SERVERS_FOR_DOMAIN_IN_SITE,
                (uint)DS_NAME_FORMAT.DS_INVALID_NAME,
                new string[] { 
                    domainDn,
                    siteDn 
                });

            BaseTestSite.Assert.AreEqual<uint>(
                0,
                ret,
                "IDL_DRSCrackNames: return value should be 0"
                );

            // Unbind
            ret = drsTestClient.DrsUnbind(srv);
            BaseTestSite.Assert.AreEqual<uint>(
                0,
                ret,
                "IDL_DRSUnbind: return value should be 0");
        }
        #endregion

        #region DRSCrackNames_List_Info_For_Server
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-DRSR")]
        [TestMethod]
        [Priority(0)]
        [SupportedADType(ADInstanceType.DS)]
        [TestCategory("Win2003")]
        [Description("Get DNS host name and server reference for a given DC.")]
        public void DRSR_DRSCrackNames_List_Info_For_Server()
        {
            DrsrTestChecker.Check();
            // Init the data.
            EnvironmentConfig.Machine srv = EnvironmentConfig.Machine.WritableDC1;
            DsServer server = (DsServer)EnvironmentConfig.MachineStore[srv];
            DsUser user = EnvironmentConfig.UserStore[EnvironmentConfig.User.ParentDomainAdmin];

            uint ret = 0;
            ret = drsTestClient.DrsBind(
                srv,
                EnvironmentConfig.User.ParentDomainAdmin,
                DRS_EXTENSIONS_IN_FLAGS.DRS_EXT_BASE
                );

            BaseTestSite.Assert.AreEqual<uint>(
                0,
                ret,
                "IDL_DRSBind: should return 0 with a success bind to DC");

            // Create the request
            string serverDn = server.ServerObjectName;

            ret = drsTestClient.DrsCrackNames(
                srv,
                DRS_MSG_CRACKREQ_FLAGS.NONE,
                (uint)formatOffered_Values.DS_LIST_INFO_FOR_SERVER,
                (uint)DS_NAME_FORMAT.DS_INVALID_NAME,
                new string[] { 
                    serverDn
                });

            BaseTestSite.Assert.AreEqual<uint>(
                0,
                ret,
                "IDL_DRSCrackNames: return value should be 0"
                );

            // Unbind
            ret = drsTestClient.DrsUnbind(srv);
            BaseTestSite.Assert.AreEqual<uint>(
                0,
                ret,
                "IDL_DRSUnbind: return value should be 0");
        }
        #endregion

        #region DRSCrackNames_List_Roles_DS
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-DRSR")]
        [TestMethod]
        [Priority(0)]
        [BVT]
        [SupportedADType(ADInstanceType.DS)]
        [TestCategory("Win2003")]
        [Description("Get FSMO role owners of a DS server.")]
        public void DRSR_DRSCrackNames_List_Roles_DS()
        {
            DrsrTestChecker.Check();
            // Init the data.
            EnvironmentConfig.Machine srv = EnvironmentConfig.Machine.WritableDC1;
            DsServer server = (DsServer)EnvironmentConfig.MachineStore[srv];
            DsUser user = EnvironmentConfig.UserStore[EnvironmentConfig.User.ParentDomainAdmin];

            uint ret = 0;
            ret = drsTestClient.DrsBind(
                srv,
                EnvironmentConfig.User.ParentDomainAdmin,
                DRS_EXTENSIONS_IN_FLAGS.DRS_EXT_BASE
                );

            BaseTestSite.Assert.AreEqual<uint>(0, ret,
                "IDL_DRSBind: Checking return value - got: {0}, expect: {1}, return value should always be 0 with a success bind to DC",
                ret, 0);

            // Create the request
            // CrackNames doesn't allow empty names, so we create a dummy one.
            string[] dummyName = new string[] { "dummy" };

            ret = drsTestClient.DrsCrackNames(
                srv,
                DRS_MSG_CRACKREQ_FLAGS.NONE,
                (uint)formatOffered_Values.DS_LIST_ROLES,
                (uint)DS_NAME_FORMAT.DS_INVALID_NAME,
                dummyName);

            BaseTestSite.Assert.AreEqual<uint>(0, ret,
                "IDL_DRSCrackNames: Checking return value - got: {0}, expect: {1}, return value should always be 0",
                ret, 0);

            // Unbind
            ret = drsTestClient.DrsUnbind(srv);
            BaseTestSite.Assert.AreEqual<uint>(0, ret,
                "IDL_DRSUnbind: Checking return value - got: {0}, expect: {1}, return value should always be 0",
                ret, 0);
        }
        #endregion

        #region DRSCrackNames_List_Roles_LDS
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-DRSR")]
        [TestMethod]
        [Priority(0)]
        [BVT]
        [Ignore]
        [SupportedADType(ADInstanceType.LDS)]
        [TestCategory("Win2003")]
        [Description("Get FSMO role owners of a LDS server.")]
        public void DRSR_DRSCrackNames_List_Roles_LDS()
        {
            DrsrTestChecker.Check();
            // Init the data.
            EnvironmentConfig.Machine srv = EnvironmentConfig.Machine.MainDC;
            DsServer server = (DsServer)EnvironmentConfig.MachineStore[srv];
            DsUser user = EnvironmentConfig.UserStore[EnvironmentConfig.User.LDSDomainAdmin];

            uint ret = 0;
            ret = drsTestClient.DrsBind(
                srv,
                EnvironmentConfig.User.LDSDomainAdmin,
                DRS_EXTENSIONS_IN_FLAGS.DRS_EXT_BASE
                );

            BaseTestSite.Assert.AreEqual<uint>(
                0,
                ret,
                "IDL_DRSBind: should return 0 with a success bind to LDS DC");

            // Create the request
            // CrackNames doesn't allow empty names, so we create a dummy one.
            string[] dummyName = new string[] { "dummy" };

            ret = drsTestClient.DrsCrackNames(
                srv,
                DRS_MSG_CRACKREQ_FLAGS.NONE,
                (uint)formatOffered_Values.DS_LIST_ROLES,
                (uint)DS_NAME_FORMAT.DS_INVALID_NAME,
                dummyName);

            BaseTestSite.Assert.AreEqual<uint>(
                0,
                ret,
                "IDL_DRSCrackNames: return value should be 0"
                );

            // Unbind
            ret = drsTestClient.DrsUnbind(srv);
            BaseTestSite.Assert.AreEqual<uint>(
                0,
                ret,
                "IDL_DRSUnbind: return value should be 0");
        }
        #endregion

        #region DRSCrackNames_List_Domains
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-DRSR")]
        [TestMethod]
        [Priority(0)]
        [BVT]
        [TestCategory("Win2003")]
        [Description("Get all domains in the forest.")]
        public void DRSR_DRSCrackNames_List_Domains()
        {
            DrsrTestChecker.Check();
            // Init the data.
            EnvironmentConfig.Machine srv = EnvironmentConfig.Machine.WritableDC1;
            DsServer server = (DsServer)EnvironmentConfig.MachineStore[srv];
            DsUser user = EnvironmentConfig.UserStore[EnvironmentConfig.User.ParentDomainAdmin];

            uint ret = 0;
            ret = drsTestClient.DrsBind(
                srv,
                EnvironmentConfig.User.ParentDomainAdmin,
                DRS_EXTENSIONS_IN_FLAGS.DRS_EXT_BASE
                );

            BaseTestSite.Assert.AreEqual<uint>(0, ret,
                "IDL_DRSBind: Checking return value - got: {0}, expect: {1}, return value should always be 0 with a success bind to DC",
                ret, 0);
            // Create the request
            // CrackNames doesn't allow empty names, so we create a dummy one.
            string[] dummyName = new string[] { "dummy" };

            ret = drsTestClient.DrsCrackNames(
                srv,
                DRS_MSG_CRACKREQ_FLAGS.NONE,
                (uint)formatOffered_Values.DS_LIST_DOMAINS,
                (uint)DS_NAME_FORMAT.DS_INVALID_NAME,
                dummyName);

            BaseTestSite.Assert.AreEqual<uint>(0, ret,
            "IDL_DRSCrackNames: Checking return value - got: {0}, expect: {1}, return value should always be 0",
            ret, 0);
            // Unbind
            ret = drsTestClient.DrsUnbind(srv);
            BaseTestSite.Assert.AreEqual<uint>(0, ret,
            "IDL_DRSUnbind: Checking return value - got: {0}, expect: {1}, return value should always be 0",
            ret, 0);
        }
        #endregion

        #region DRSCrackNames_List_NCs
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-DRSR")]
        [TestMethod]
        [Priority(0)]
        [TestCategory("Win2003")]
        [Description("Get all NCs in the forest.")]
        public void DRSR_DRSCrackNames_List_NCs()
        {
            DrsrTestChecker.Check();
            // Init the data.
            EnvironmentConfig.Machine srv = EnvironmentConfig.Machine.WritableDC1;
            DsServer server = (DsServer)EnvironmentConfig.MachineStore[srv];
            DsUser user = EnvironmentConfig.UserStore[EnvironmentConfig.User.ParentDomainAdmin];

            uint ret = 0;
            ret = drsTestClient.DrsBind(
                srv,
                EnvironmentConfig.User.ParentDomainAdmin,
                DRS_EXTENSIONS_IN_FLAGS.DRS_EXT_BASE
                );

            BaseTestSite.Assert.AreEqual<uint>(0, ret,
                "IDL_DRSBind: Checking return value - got: {0}, expect: {1}, return value should always be 0 with a success bind to DC",
                ret, 0);

            // Create the request
            // CrackNames doesn't allow empty names, so we create a dummy one.
            string[] dummyName = new string[] { "dummy" };

            ret = drsTestClient.DrsCrackNames(
                srv,
                DRS_MSG_CRACKREQ_FLAGS.NONE,
                (uint)formatOffered_Values.DS_LIST_NCS,
                (uint)DS_NAME_FORMAT.DS_INVALID_NAME,
                dummyName);

            BaseTestSite.Assert.AreEqual<uint>(0, ret,
                "IDL_DRSCrackNames: Checking return value - got: {0}, expect: {1}, return value should always be 0",
                ret, 0);
            // Unbind
            ret = drsTestClient.DrsUnbind(srv);
            BaseTestSite.Assert.AreEqual<uint>(0, ret,
                "IDL_DRSUnbind: Checking return value - got: {0}, expect: {1}, return value should always be 0",
                ret, 0);
        }
        #endregion

        #region DRSCrackNames_List_Servers_With_DCs_In_Site
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-DRSR")]
        [TestMethod]
        [Priority(0)]
        [TestCategory("Win2003")]
        [Description("Get all DCs in a given site.")]
        public void DRSR_DRSCrackNames_List_Servers_With_DCs_In_Site()
        {
            DrsrTestChecker.Check();
            // Init the data.
            EnvironmentConfig.Machine srv = EnvironmentConfig.Machine.WritableDC1;
            DsServer server = (DsServer)EnvironmentConfig.MachineStore[srv];
            DsUser user = EnvironmentConfig.UserStore[EnvironmentConfig.User.ParentDomainAdmin];

            uint ret = 0;
            ret = drsTestClient.DrsBind(
                srv,
                EnvironmentConfig.User.ParentDomainAdmin,
                DRS_EXTENSIONS_IN_FLAGS.DRS_EXT_BASE
                );

            BaseTestSite.Assert.AreEqual<uint>(
                0,
                ret,
                "IDL_DRSBind: should return 0 with a success bind to DC");

            // Create the request
            // We use the site where the server is located in.
            string siteDn = server.Site.DN;

            ret = drsTestClient.DrsCrackNames(
                srv,
                DRS_MSG_CRACKREQ_FLAGS.NONE,
                (uint)formatOffered_Values.DS_LIST_SERVERS_WITH_DCS_IN_SITE,
                (uint)DS_NAME_FORMAT.DS_INVALID_NAME,
                new string[] { siteDn });

            BaseTestSite.Assert.AreEqual<uint>(
                0,
                ret,
                "IDL_DRSCrackNames: return value should be 0"
                );

            // Unbind
            ret = drsTestClient.DrsUnbind(srv);
            BaseTestSite.Assert.AreEqual<uint>(
                0,
                ret,
                "IDL_DRSUnbind: return value should be 0");
        }
        #endregion


        #region DRSCrackNames_FQDN_1779_To_NT4_Account_Name
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-DRSR")]
        [TestMethod]
        [Priority(0)]
        [BVT]
        [SupportedADType(ADInstanceType.DS)]
        [TestCategory("Win2003")]
        [Description("Find the NT4 Account Name of a given FQDN 1779 name")]
        public void DRSR_DRSCrackNames_FQDN_1779_To_NT4_Account_Name()
        {
            DrsrTestChecker.Check();
            // Init the data.
            EnvironmentConfig.Machine srv = EnvironmentConfig.Machine.WritableDC1;
            DsServer server = (DsServer)EnvironmentConfig.MachineStore[srv];
            DsUser user = EnvironmentConfig.UserStore[EnvironmentConfig.User.ParentDomainAdmin];
            string userDn = ldapAdapter.GetUserDn(server, user);

            uint ret = 0;
            ret = drsTestClient.DrsBind(
                srv,
                EnvironmentConfig.User.ParentDomainAdmin,
                DRS_EXTENSIONS_IN_FLAGS.DRS_EXT_BASE
                );

            BaseTestSite.Assert.AreEqual<uint>(
                0,
                ret,
                "IDL_DRSBind: should return 0 with a success bind to DC");

            // Actual DRSR call.
            ret = drsTestClient.DrsCrackNames(
                srv,
                DRS_MSG_CRACKREQ_FLAGS.NONE,
                (uint)DS_NAME_FORMAT.DS_FQDN_1779_NAME,
                (uint)DS_NAME_FORMAT.DS_NT4_ACCOUNT_NAME,
                new string[] { userDn }
                );

            BaseTestSite.Assert.AreEqual<uint>(
                0,
                ret,
                "IDL_DRSCrackNames: return value should be 0"
                );

            // Unbind
            ret = drsTestClient.DrsUnbind(srv);
            BaseTestSite.Assert.AreEqual<uint>(
                0,
                ret,
                "IDL_DRSUnbind: return value should be 0");
        }
        #endregion

        #region DRSCrackNames_FQDN_1779_To_Display_Name
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-DRSR")]
        [TestMethod]
        [Priority(0)]
        [TestCategory("Win2003")]
        [Description("Find the Display Name of a given FQDN 1779 name")]
        public void DRSR_DRSCrackNames_FQDN_1779_To_Display_Name()
        {
            DrsrTestChecker.Check();
            // Init the data.
            EnvironmentConfig.Machine srv = EnvironmentConfig.Machine.WritableDC1;
            DsServer server = (DsServer)EnvironmentConfig.MachineStore[srv];

            RootDSE rootDse = LdapUtility.GetRootDSE(server);
            string dn = DRSTestData.DrsCrackNames_DisplayNameRDN + "," + rootDse.configurationNamingContext;

            uint ret = 0;
            ret = drsTestClient.DrsBind(
                srv,
                EnvironmentConfig.User.ParentDomainAdmin,
                DRS_EXTENSIONS_IN_FLAGS.DRS_EXT_BASE
                );

            BaseTestSite.Assert.AreEqual<uint>(
                0,
                ret,
                "IDL_DRSBind: should return 0 with a success bind to DC");

            // Actual DRSR call.
            ret = drsTestClient.DrsCrackNames(
                srv,
                DRS_MSG_CRACKREQ_FLAGS.NONE,
                (uint)DS_NAME_FORMAT.DS_FQDN_1779_NAME,
                (uint)DS_NAME_FORMAT.DS_DISPLAY_NAME,
                new string[] { dn }
                );

            BaseTestSite.Assert.AreEqual<uint>(
                0,
                ret,
                "IDL_DRSCrackNames: return value should be 0"
                );

            // Unbind
            ret = drsTestClient.DrsUnbind(srv);
            BaseTestSite.Assert.AreEqual<uint>(
                0,
                ret,
                "IDL_DRSUnbind: return value should be 0");
        }
        #endregion

        #region DRSCrackNames_FQDN_1779_To_Unique_ID_Name
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-DRSR")]
        [TestMethod]
        [Priority(0)]
        [TestCategory("Win2003")]
        [Description("Find the Unique ID of a given FQDN 1779 name")]
        public void DRSR_DRSCrackNames_FQDN_1779_To_Unique_ID_Name()
        {
            DrsrTestChecker.Check();
            // Init the data.
            EnvironmentConfig.Machine srv = EnvironmentConfig.Machine.WritableDC1;
            DsServer server = (DsServer)EnvironmentConfig.MachineStore[srv];
            RootDSE rootDse = LdapUtility.GetRootDSE(server);
            string dn = DRSTestData.DrsCrackNames_DisplayNameRDN + "," + rootDse.configurationNamingContext;

            uint ret = 0;
            ret = drsTestClient.DrsBind(
                srv,
                EnvironmentConfig.User.ParentDomainAdmin,
                DRS_EXTENSIONS_IN_FLAGS.DRS_EXT_BASE
                );

            BaseTestSite.Assert.AreEqual<uint>(
                0,
                ret,
                "IDL_DRSBind: should return 0 with a success bind to DC");

            // Actual DRSR call.
            ret = drsTestClient.DrsCrackNames(
                srv,
                DRS_MSG_CRACKREQ_FLAGS.NONE,
                (uint)DS_NAME_FORMAT.DS_FQDN_1779_NAME,
                (uint)DS_NAME_FORMAT.DS_UNIQUE_ID_NAME,
                new string[] { dn }
                );

            BaseTestSite.Assert.AreEqual<uint>(
                0,
                ret,
                "IDL_DRSCrackNames: return value should be 0"
                );

            // Unbind
            ret = drsTestClient.DrsUnbind(srv);
            BaseTestSite.Assert.AreEqual<uint>(
                0,
                ret,
                "IDL_DRSUnbind: return value should be 0");
        }
        #endregion

        #region DRSCrackNames_FQDN_1779_To_Canonical_Name
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-DRSR")]
        [TestMethod]
        [Priority(0)]
        [SupportedADType(ADInstanceType.DS)]
        [TestCategory("Win2003")]
        [Description("Find the Canonical Name of a given FQDN 1779 name")]
        public void DRSR_DRSCrackNames_FQDN_1779_To_Canonical_Name()
        {
            DrsrTestChecker.Check();
            // Init the data.
            EnvironmentConfig.Machine srv = EnvironmentConfig.Machine.WritableDC1;
            DsServer server = (DsServer)EnvironmentConfig.MachineStore[srv];
            RootDSE rootDse = LdapUtility.GetRootDSE(server);
            string dn = DRSTestData.DrsCrackNames_DisplayNameRDN + "," + rootDse.configurationNamingContext;

            uint ret = 0;
            ret = drsTestClient.DrsBind(
                srv,
                EnvironmentConfig.User.ParentDomainAdmin,
                DRS_EXTENSIONS_IN_FLAGS.DRS_EXT_BASE
                );

            BaseTestSite.Assert.AreEqual<uint>(
                0,
                ret,
                "IDL_DRSBind: should return 0 with a success bind to DC");

            // Actual DRSR call.
            ret = drsTestClient.DrsCrackNames(
                srv,
                DRS_MSG_CRACKREQ_FLAGS.NONE,
                (uint)DS_NAME_FORMAT.DS_FQDN_1779_NAME,
                (uint)DS_NAME_FORMAT.DS_CANONICAL_NAME,
                new string[] { dn }
                );

            BaseTestSite.Assert.AreEqual<uint>(
                0,
                ret,
                "IDL_DRSCrackNames: return value should be 0"
                );

            // Unbind
            ret = drsTestClient.DrsUnbind(srv);
            BaseTestSite.Assert.AreEqual<uint>(
                0,
                ret,
                "IDL_DRSUnbind: return value should be 0");
        }
        #endregion

        #region DRSCrackNames_FQDN_1779_To_User_Principal_Name
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-DRSR")]
        [TestMethod]
        [Priority(0)]
        [SupportedADType(ADInstanceType.DS)]
        [TestCategory("Win2003")]
        [Description("Find the User Principal Name of a given FQDN 1779 name")]
        public void DRSR_DRSCrackNames_FQDN_1779_To_User_Principal_Name()
        {
            DrsrTestChecker.Check();
            // Init the data.
            EnvironmentConfig.Machine srv = EnvironmentConfig.Machine.WritableDC1;
            DsServer server = (DsServer)EnvironmentConfig.MachineStore[srv];
            DsUser user = EnvironmentConfig.UserStore[EnvironmentConfig.User.ParentDomainAdmin];
            string userDn = ldapAdapter.GetUserDn(server, user);

            uint ret = 0;
            ret = drsTestClient.DrsBind(
                srv,
                EnvironmentConfig.User.ParentDomainAdmin,
                DRS_EXTENSIONS_IN_FLAGS.DRS_EXT_BASE
                );

            BaseTestSite.Assert.AreEqual<uint>(
                0,
                ret,
                "IDL_DRSBind: should return 0 with a success bind to DC");

            // Actual DRSR call.
            ret = drsTestClient.DrsCrackNames(
                srv,
                DRS_MSG_CRACKREQ_FLAGS.NONE,
                (uint)DS_NAME_FORMAT.DS_FQDN_1779_NAME,
                (uint)DS_NAME_FORMAT.DS_USER_PRINCIPAL_NAME,
                new string[] { userDn }
                );

            BaseTestSite.Assert.AreEqual<uint>(
                0,
                ret,
                "IDL_DRSCrackNames: return value should be 0"
                );

            // Unbind
            ret = drsTestClient.DrsUnbind(srv);
            BaseTestSite.Assert.AreEqual<uint>(
                0,
                ret,
                "IDL_DRSUnbind: return value should be 0");
        }
        #endregion

        #region DRSCrackNames_FQDN_1779_To_Canonical_Name_Ex
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-DRSR")]
        [TestMethod]
        [Priority(0)]
        [SupportedADType(ADInstanceType.DS)]
        [TestCategory("Win2003")]
        [Description("Find the Canonical Name of a given FQDN 1779 name, with newline character ('\n') replacing the rightmost forward slash ('/')")]
        public void DRSR_DRSCrackNames_FQDN_1779_To_Canonical_Name_Ex()
        {
            DrsrTestChecker.Check();
            // Init the data.
            EnvironmentConfig.Machine srv = EnvironmentConfig.Machine.WritableDC1;
            DsServer server = (DsServer)EnvironmentConfig.MachineStore[srv];
            DsUser user = EnvironmentConfig.UserStore[EnvironmentConfig.User.ParentDomainAdmin];
            string userDn = ldapAdapter.GetUserDn(server, user);

            uint ret = 0;
            ret = drsTestClient.DrsBind(
                srv,
                EnvironmentConfig.User.ParentDomainAdmin,
                DRS_EXTENSIONS_IN_FLAGS.DRS_EXT_BASE
                );

            BaseTestSite.Assert.AreEqual<uint>(
                0,
                ret,
                "IDL_DRSBind: should return 0 with a success bind to DC");

            // Actual DRSR call.
            ret = drsTestClient.DrsCrackNames(
                srv,
                DRS_MSG_CRACKREQ_FLAGS.NONE,
                (uint)DS_NAME_FORMAT.DS_FQDN_1779_NAME,
                (uint)DS_NAME_FORMAT.DS_CANONICAL_NAME_EX,
                new string[] { userDn }
                );

            BaseTestSite.Assert.AreEqual<uint>(
                0,
                ret,
                "IDL_DRSCrackNames: return value should be 0"
                );

            // Unbind
            ret = drsTestClient.DrsUnbind(srv);
            BaseTestSite.Assert.AreEqual<uint>(
                0,
                ret,
                "IDL_DRSUnbind: return value should be 0");
        }
        #endregion

        #region DRSCrackNames_FQDN_1779_To_Service_Principal_Name
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-DRSR")]
        [TestMethod]
        [Priority(0)]
        [SupportedADType(ADInstanceType.DS)]
        [TestCategory("Win2003")]
        [Description("Find the Service Principal Name of a given FQDN 1779 name")]
        public void DRSR_DRSCrackNames_FQDN_1779_To_Service_Principal_Name()
        {
            DrsrTestChecker.Check();
            // Init the data.
            EnvironmentConfig.Machine srv = EnvironmentConfig.Machine.WritableDC1;
            DsServer server = (DsServer)EnvironmentConfig.MachineStore[srv];
            DsUser user = EnvironmentConfig.UserStore[EnvironmentConfig.User.ParentDomainAdmin];
            string userDn = ldapAdapter.GetUserDn(server, user);

            uint ret = 0;
            ret = drsTestClient.DrsBind(
                srv,
                EnvironmentConfig.User.ParentDomainAdmin,
                DRS_EXTENSIONS_IN_FLAGS.DRS_EXT_BASE
                );

            BaseTestSite.Assert.AreEqual<uint>(
                0,
                ret,
                "IDL_DRSBind: should return 0 with a success bind to DC");

            // Actual DRSR call.
            ret = drsTestClient.DrsCrackNames(
                srv,
                DRS_MSG_CRACKREQ_FLAGS.NONE,
                (uint)DS_NAME_FORMAT.DS_FQDN_1779_NAME,
                (uint)DS_NAME_FORMAT.DS_SERVICE_PRINCIPAL_NAME,
                new string[] { userDn }
                );

            BaseTestSite.Assert.AreEqual<uint>(
                0,
                ret,
                "IDL_DRSCrackNames: return value should be 0"
                );

            // Unbind
            ret = drsTestClient.DrsUnbind(srv);
            BaseTestSite.Assert.AreEqual<uint>(
                0,
                ret,
                "IDL_DRSUnbind: return value should be 0");
        }
        #endregion

        #region DRSCrackNames_FQDN_1779_To_SID_Or_SID_History_Name
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-DRSR")]
        [TestMethod]
        [Priority(0)]
        [TestCategory("Win2003")]
        [Description("Find the SID or SID History name of a given FQDN 1779 name")]
        public void DRSR_DRSCrackNames_FQDN_1779_To_SID_Or_SID_History_Name()
        {
            DrsrTestChecker.Check();
            // Init the data.
            EnvironmentConfig.Machine srv = EnvironmentConfig.Machine.WritableDC1;
            DsServer server = (DsServer)EnvironmentConfig.MachineStore[srv];

            RootDSE rootDse = LdapUtility.GetRootDSE(server);
            string dn = DRSTestData.DrsCrackNames_DisplayNameRDN + "," + rootDse.configurationNamingContext;

            uint ret = 0;
            ret = drsTestClient.DrsBind(
                srv,
                EnvironmentConfig.User.ParentDomainAdmin,
                DRS_EXTENSIONS_IN_FLAGS.DRS_EXT_BASE
                );

            BaseTestSite.Assert.AreEqual<uint>(
                0,
                ret,
                "IDL_DRSBind: should return 0 with a success bind to DC");

            // Actual DRSR call.
            ret = drsTestClient.DrsCrackNames(
                srv,
                DRS_MSG_CRACKREQ_FLAGS.NONE,
                (uint)DS_NAME_FORMAT.DS_FQDN_1779_NAME,
                (uint)DS_NAME_FORMAT.DS_SID_OR_SID_HISTORY_NAME,
                new string[] { dn }
                );

            BaseTestSite.Assert.AreEqual<uint>(
                0,
                ret,
                "IDL_DRSCrackNames: return value should be 0"
                );

            // Unbind
            ret = drsTestClient.DrsUnbind(srv);
            BaseTestSite.Assert.AreEqual<uint>(
                0,
                ret,
                "IDL_DRSUnbind: return value should be 0");
        }
        #endregion

        #region DRSCrackNames_FQDN_1779_To_Unknown_Name
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-DRSR")]
        [TestMethod]
        [Priority(0)]
        [SupportedADType(ADInstanceType.DS)]
        [TestCategory("Win2003")]
        [Description("Use the LookupUnknownName method to resolve a FQDN name.")]
        public void DRSR_DRSCrackNames_FQDN_1779_To_Unknown_Name()
        {
            DrsrTestChecker.Check();
            // Init the data.
            EnvironmentConfig.Machine srv = EnvironmentConfig.Machine.WritableDC1;
            DsServer server = (DsServer)EnvironmentConfig.MachineStore[srv];
            DsUser user = EnvironmentConfig.UserStore[EnvironmentConfig.User.ParentDomainAdmin];
            string userDn = ldapAdapter.GetUserDn(server, user);

            uint ret = 0;
            ret = drsTestClient.DrsBind(
                srv,
                EnvironmentConfig.User.ParentDomainAdmin,
                DRS_EXTENSIONS_IN_FLAGS.DRS_EXT_BASE
                );

            BaseTestSite.Assert.AreEqual<uint>(
                0,
                ret,
                "IDL_DRSBind: should return 0 with a success bind to DC");

            // Actual DRSR call.
            ret = drsTestClient.DrsCrackNames(
                srv,
                DRS_MSG_CRACKREQ_FLAGS.NONE,
                (uint)DS_NAME_FORMAT.DS_FQDN_1779_NAME,
                (uint)DS_NAME_FORMAT.DS_UNKNOWN_NAME,
                new string[] { userDn }
                );

            BaseTestSite.Assert.AreEqual<uint>(
                0,
                ret,
                "IDL_DRSCrackNames: return value should be 0"
                );

            // Unbind
            ret = drsTestClient.DrsUnbind(srv);
            BaseTestSite.Assert.AreEqual<uint>(
                0,
                ret,
                "IDL_DRSUnbind: return value should be 0");
        }
        #endregion

        #region DRSCrackNames_FQDN_1779_To_String_SID_Name
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-DRSR")]
        [TestMethod]
        [Priority(0)]
        [TestCategory("Win2003")]
        [Description("Find the String SID name of a given FQDN name.")]
        public void DRSR_DRSCrackNames_FQDN_1779_To_String_SID_Name()
        {
            DrsrTestChecker.Check();
            // Init the data.
            EnvironmentConfig.Machine srv = EnvironmentConfig.Machine.WritableDC1;
            DsServer server = (DsServer)EnvironmentConfig.MachineStore[srv];

            RootDSE rootDse = LdapUtility.GetRootDSE(server);
            string dn = DRSTestData.DrsCrackNames_DisplayNameRDN + "," + rootDse.configurationNamingContext;

            uint ret = 0;
            ret = drsTestClient.DrsBind(
                srv,
                EnvironmentConfig.User.ParentDomainAdmin,
                DRS_EXTENSIONS_IN_FLAGS.DRS_EXT_BASE
                );

            BaseTestSite.Assert.AreEqual<uint>(
                0,
                ret,
                "IDL_DRSBind: should return 0 with a success bind to DC");

            // Actual DRSR call.
            ret = drsTestClient.DrsCrackNames(
                srv,
                DRS_MSG_CRACKREQ_FLAGS.NONE,
                (uint)DS_NAME_FORMAT.DS_FQDN_1779_NAME,
                (uint)formatDesired_Values.DS_STRING_SID_NAME,
                new string[] { dn }
                );

            BaseTestSite.Assert.AreEqual<uint>(
                0,
                ret,
                "IDL_DRSCrackNames: return value should be 0"
                );

            // Unbind
            ret = drsTestClient.DrsUnbind(srv);
            BaseTestSite.Assert.AreEqual<uint>(
                0,
                ret,
                "IDL_DRSUnbind: return value should be 0");
        }
        #endregion

        #region DRSCrackNames_FQDN_1779_To_User_Principal_Name_For_Logon
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-DRSR")]
        [TestMethod]
        [Priority(0)]
        [SupportedADType(ADInstanceType.DS)]
        [TestCategory("Win2003")]
        [Description("Find the UPN for logon of a given FQDN name.")]
        public void DRSR_DRSCrackNames_FQDN_1779_To_User_Principal_Name_For_Logon()
        {
            DrsrTestChecker.Check();
            // Init the data.
            EnvironmentConfig.Machine srv = EnvironmentConfig.Machine.WritableDC1;
            DsServer server = (DsServer)EnvironmentConfig.MachineStore[srv];
            DsUser user = EnvironmentConfig.UserStore[EnvironmentConfig.User.ParentDomainAdmin];
            string userDn = ldapAdapter.GetUserDn(server, user);

            uint ret = 0;
            ret = drsTestClient.DrsBind(
                srv,
                EnvironmentConfig.User.ParentDomainAdmin,
                DRS_EXTENSIONS_IN_FLAGS.DRS_EXT_BASE
                );

            BaseTestSite.Assert.AreEqual<uint>(
                0,
                ret,
                "IDL_DRSBind: should return 0 with a success bind to DC");

            // Actual DRSR call.
            ret = drsTestClient.DrsCrackNames(
                srv,
                DRS_MSG_CRACKREQ_FLAGS.NONE,
                (uint)DS_NAME_FORMAT.DS_FQDN_1779_NAME,
                (uint)formatDesired_Values.DS_USER_PRINCIPAL_NAME_FOR_LOGON,
                new string[] { userDn }
                );

            BaseTestSite.Assert.AreEqual<uint>(
                0,
                ret,
                "IDL_DRSCrackNames: return value should be 0"
                );

            // Unbind
            ret = drsTestClient.DrsUnbind(srv);
            BaseTestSite.Assert.AreEqual<uint>(
                0,
                ret,
                "IDL_DRSUnbind: return value should be 0");
        }
        #endregion

        #region DRSCrackNames_FQDN_1779_From_NT4_Account_Name
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-DRSR")]
        [TestMethod]
        [Priority(0)]
        [BVT]
        [SupportedADType(ADInstanceType.DS)]
        [TestCategory("Win2003")]
        [Description("Find the FQDN of a given NT4 account name.")]
        public void DRSR_DRSCrackNames_FQDN_1779_From_NT4_Account_Name()
        {
            DrsrTestChecker.Check();
            // Init the data.
            EnvironmentConfig.Machine srv = EnvironmentConfig.Machine.WritableDC1;
            DsServer server = (DsServer)EnvironmentConfig.MachineStore[srv];
            DsUser user = EnvironmentConfig.UserStore[EnvironmentConfig.User.ParentDomainUser];
            string userDn = ldapAdapter.GetUserDn(server, user);
            string nt4Acc = server.Domain.NetbiosName + "\\" +
                ldapAdapter.GetAttributeValueInString(server, userDn, "sAMAccountName");

            uint ret = 0;
            ret = drsTestClient.DrsBind(
                srv,
                EnvironmentConfig.User.ParentDomainAdmin,
                DRS_EXTENSIONS_IN_FLAGS.DRS_EXT_BASE
                );

            BaseTestSite.Assert.AreEqual<uint>(0, ret,
                "IDL_DRSBind: Checking return value - got: {0}, expect: {1}, return value should always be 0 with a success bind to DC",
                ret, 0);
            // Actual DRSR call.
            ret = drsTestClient.DrsCrackNames(
                srv,
                DRS_MSG_CRACKREQ_FLAGS.NONE,
                (uint)DS_NAME_FORMAT.DS_NT4_ACCOUNT_NAME,
                (uint)DS_NAME_FORMAT.DS_FQDN_1779_NAME,
                new string[] { nt4Acc }
                );

            BaseTestSite.Assert.AreEqual<uint>(0, ret,
                "IDL_DRSCrackNames: Checking return value - got: {0}, expect: {1}, return value should always be 0",
                ret, 0);
            // Unbind
            ret = drsTestClient.DrsUnbind(srv);

            BaseTestSite.Assert.AreEqual<uint>(0, ret,
                "IDL_DRSUnbind: Checking return value - got: {0}, expect: {1}, return value should always be 0",
            ret, 0);
        }
        #endregion

        #region DRSCrackNames_FQDN_1779_From_Display_Name
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-DRSR")]
        [TestMethod]
        [Priority(0)]
        [TestCategory("Win2003")]
        [Description("Find the FQDN of a given display name.")]
        public void DRSR_DRSCrackNames_FQDN_1779_From_Display_Name()
        {
            DrsrTestChecker.Check();
            // Init the data.
            EnvironmentConfig.Machine srv = EnvironmentConfig.Machine.WritableDC1;
            DsServer server = (DsServer)EnvironmentConfig.MachineStore[srv];

            RootDSE rootDse = LdapUtility.GetRootDSE(server);
            string dn = DRSTestData.DrsCrackNames_DisplayNameRDN + "," + rootDse.configurationNamingContext;

            string displayName = ldapAdapter.GetAttributeValueInString(server, dn, "displayName");

            uint ret = 0;
            ret = drsTestClient.DrsBind(
                srv,
                EnvironmentConfig.User.ParentDomainAdmin,
                DRS_EXTENSIONS_IN_FLAGS.DRS_EXT_BASE
                );

            BaseTestSite.Assert.AreEqual<uint>(
                0,
                ret,
                "IDL_DRSBind: should return 0 with a success bind to DC");

            // Actual DRSR call.
            ret = drsTestClient.DrsCrackNames(
                srv,
                DRS_MSG_CRACKREQ_FLAGS.NONE,
                (uint)DS_NAME_FORMAT.DS_DISPLAY_NAME,
                (uint)DS_NAME_FORMAT.DS_FQDN_1779_NAME,
                new string[] { displayName }
                );

            BaseTestSite.Assert.AreEqual<uint>(
                0,
                ret,
                "IDL_DRSCrackNames: return value should be 0"
                );

            // Unbind
            ret = drsTestClient.DrsUnbind(srv);
            BaseTestSite.Assert.AreEqual<uint>(
                0,
                ret,
                "IDL_DRSUnbind: return value should be 0");
        }
        #endregion

        #region DRSCrackNames_FQDN_1779_From_Unique_ID_Name
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-DRSR")]
        [TestMethod]
        [Priority(0)]
        [TestCategory("Win2003")]
        [Description("Find the FQDN of a given unique ID name.")]
        public void DRSR_DRSCrackNames_FQDN_1779_From_Unique_ID_Name()
        {
            DrsrTestChecker.Check();
            // Init the data.
            EnvironmentConfig.Machine srv = EnvironmentConfig.Machine.WritableDC1;
            DsServer server = (DsServer)EnvironmentConfig.MachineStore[srv];

            RootDSE rootDse = LdapUtility.GetRootDSE(server);
            string dn = DRSTestData.DrsCrackNames_DisplayNameRDN + "," + rootDse.configurationNamingContext;

            Guid? guid = LdapUtility.GetObjectGuid(server, dn);

            BaseTestSite.Assert.IsNotNull(
                guid,
                "IDL_DRSCrackNames: cannot read GUID of the object {0}",
                dn);

            uint ret = 0;
            ret = drsTestClient.DrsBind(
                srv,
                EnvironmentConfig.User.ParentDomainAdmin,
                DRS_EXTENSIONS_IN_FLAGS.DRS_EXT_BASE
                );

            BaseTestSite.Assert.AreEqual<uint>(
                0,
                ret,
                "IDL_DRSBind: should return 0 with a success bind to DC");

            // Actual DRSR call.
            ret = drsTestClient.DrsCrackNames(
                srv,
                DRS_MSG_CRACKREQ_FLAGS.NONE,
                (uint)DS_NAME_FORMAT.DS_UNIQUE_ID_NAME,
                (uint)DS_NAME_FORMAT.DS_FQDN_1779_NAME,
                new string[] { "{" + guid.Value.ToString() + "}" }
                );

            BaseTestSite.Assert.AreEqual<uint>(
                0,
                ret,
                "IDL_DRSCrackNames: return value should be 0"
                );

            // Unbind
            ret = drsTestClient.DrsUnbind(srv);
            BaseTestSite.Assert.AreEqual<uint>(
                0,
                ret,
                "IDL_DRSUnbind: return value should be 0");
        }
        #endregion

        #region DRSCrackNames_FQDN_1779_From_Canonical_Name
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-DRSR")]
        [TestMethod]
        [Priority(0)]
        [SupportedADType(ADInstanceType.DS)]
        [TestCategory("Win2003")]
        [Description("Find the FQDN of a given canonical name.")]
        public void DRSR_DRSCrackNames_FQDN_1779_From_Canonical_Name()
        {
            DrsrTestChecker.Check();
            // Init the data.
            EnvironmentConfig.Machine srv = EnvironmentConfig.Machine.WritableDC1;
            DsServer server = (DsServer)EnvironmentConfig.MachineStore[srv];
            DsUser user = EnvironmentConfig.UserStore[EnvironmentConfig.User.ParentDomainAdmin];
            string userDn = ldapAdapter.GetUserDn(server, user);

            string userContainer =
                ldapAdapter.GetParentObjectDn(userDn)
                    .Split(',')[0]
                    .Replace("CN=", "");

            string cn = server.Domain.DNSName + "/"
                + userContainer + "/"
                + userDn.Split(',')[0]
                    .Replace("CN=", "");

            uint ret = 0;
            ret = drsTestClient.DrsBind(
                srv,
                EnvironmentConfig.User.ParentDomainAdmin,
                DRS_EXTENSIONS_IN_FLAGS.DRS_EXT_BASE
                );

            BaseTestSite.Assert.AreEqual<uint>(
                0,
                ret,
                "IDL_DRSBind: should return 0 with a success bind to DC");

            // Actual DRSR call.
            ret = drsTestClient.DrsCrackNames(
                srv,
                DRS_MSG_CRACKREQ_FLAGS.NONE,
                (uint)DS_NAME_FORMAT.DS_CANONICAL_NAME,
                (uint)DS_NAME_FORMAT.DS_FQDN_1779_NAME,
                new string[] { cn }
                );

            BaseTestSite.Assert.AreEqual<uint>(
                0,
                ret,
                "IDL_DRSCrackNames: return value should be 0"
                );

            // Unbind
            ret = drsTestClient.DrsUnbind(srv);
            BaseTestSite.Assert.AreEqual<uint>(
                0,
                ret,
                "IDL_DRSUnbind: return value should be 0");
        }
        #endregion

        #region DRSCrackNames_FQDN_1779_From_User_Principal_Name
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-DRSR")]
        [TestMethod]
        [Priority(0)]
        [SupportedADType(ADInstanceType.DS)]
        [TestCategory("Win2003")]
        [Description("Find the FQDN of a given user principal name.")]
        public void DRSR_DRSCrackNames_FQDN_1779_From_User_Principal_Name()
        {
            DrsrTestChecker.Check();
            // Init the data.
            EnvironmentConfig.Machine srv = EnvironmentConfig.Machine.WritableDC1;
            DsServer server = (DsServer)EnvironmentConfig.MachineStore[srv];
            DsUser user = EnvironmentConfig.UserStore[EnvironmentConfig.User.ParentDomainUser];
            string userDn = ldapAdapter.GetUserDn(server, user);

            string upn = "drsr_testObj@" + EnvironmentConfig.DomainStore[DomainEnum.PrimaryDomain].DNSName;

            ResultCode r = ldapAdapter.ModifyAttribute(
                server,
                userDn,
                new DirectoryAttribute("userPrincipalName", upn)
                );

            BaseTestSite.Assert.AreEqual<ResultCode>(
                ResultCode.Success,
                r,
                "IDL_DRSCrackNames: modify the userPrincipalName of " + userDn + "attribute to " + upn);

            uint ret = 0;
            ret = drsTestClient.DrsBind(
                srv,
                EnvironmentConfig.User.ParentDomainAdmin,
                DRS_EXTENSIONS_IN_FLAGS.DRS_EXT_BASE
                );

            BaseTestSite.Assert.AreEqual<uint>(
                0,
                ret,
                "IDL_DRSBind: should return 0 with a success bind to DC");

            // Actual DRSR call.
            ret = drsTestClient.DrsCrackNames(
                srv,
                DRS_MSG_CRACKREQ_FLAGS.NONE,
                (uint)DS_NAME_FORMAT.DS_USER_PRINCIPAL_NAME,
                (uint)DS_NAME_FORMAT.DS_FQDN_1779_NAME,
                new string[] { upn }
                );

            BaseTestSite.Assert.AreEqual<uint>(
                0,
                ret,
                "IDL_DRSCrackNames: return value should be 0"
                );

            // Unbind
            ret = drsTestClient.DrsUnbind(srv);
            BaseTestSite.Assert.AreEqual<uint>(
                0,
                ret,
                "IDL_DRSUnbind: return value should be 0");
        }
        #endregion

        #region DRSCrackNames_FQDN_1779_From_Canonical_Name_Ex
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-DRSR")]
        [TestMethod]
        [Priority(0)]
        [SupportedADType(ADInstanceType.DS)]
        [TestCategory("Win2003")]
        [Description("Find the FQDN of a given canonical name. The canonical name is the same as in  DRSCrackNames_FQDN_1779_From_Canonical_Name only that the rightmost forward slash ('/') is replaced with new line ('\n')")]
        public void DRSR_DRSCrackNames_FQDN_1779_From_Canonical_Name_Ex()
        {
            DrsrTestChecker.Check();
            // Init the data.
            EnvironmentConfig.Machine srv = EnvironmentConfig.Machine.WritableDC1;
            DsServer server = (DsServer)EnvironmentConfig.MachineStore[srv];
            DsUser user = EnvironmentConfig.UserStore[EnvironmentConfig.User.ParentDomainAdmin];
            string userDn = ldapAdapter.GetUserDn(server, user);
            string userContainer =
                ldapAdapter.GetParentObjectDn(userDn)
                    .Split(',')[0]
                    .Replace("CN=", "");

            string cn = server.Domain.DNSName + "/"
                + userContainer + "\n"
                + userDn.Split(',')[0]
                    .Replace("CN=", "");

            uint ret = 0;
            ret = drsTestClient.DrsBind(
                srv,
                EnvironmentConfig.User.ParentDomainAdmin,
                DRS_EXTENSIONS_IN_FLAGS.DRS_EXT_BASE
                );

            BaseTestSite.Assert.AreEqual<uint>(
                0,
                ret,
                "IDL_DRSBind: should return 0 with a success bind to DC");

            // Actual DRSR call.
            ret = drsTestClient.DrsCrackNames(
                srv,
                DRS_MSG_CRACKREQ_FLAGS.NONE,
                (uint)DS_NAME_FORMAT.DS_CANONICAL_NAME_EX,
                (uint)DS_NAME_FORMAT.DS_FQDN_1779_NAME,
                new string[] { cn }
                );

            BaseTestSite.Assert.AreEqual<uint>(
                0,
                ret,
                "IDL_DRSCrackNames: return value should be 0"
                );

            // Unbind
            ret = drsTestClient.DrsUnbind(srv);
            BaseTestSite.Assert.AreEqual<uint>(
                0,
                ret,
                "IDL_DRSUnbind: return value should be 0");
        }
        #endregion

        #region DRSCrackNames_FQDN_1779_From_SID_Or_SID_History_Name
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-DRSR")]
        [TestMethod]
        [Priority(0)]
        [SupportedADType(ADInstanceType.DS)]
        [TestCategory("Win2003")]
        [Description("Find the FQDN of a given SID name.")]
        public void DRSR_DRSCrackNames_FQDN_1779_From_SID_Or_SID_History_Name()
        {
            DrsrTestChecker.Check();
            // Init the data.
            EnvironmentConfig.Machine srv = EnvironmentConfig.Machine.WritableDC1;
            DsServer server = (DsServer)EnvironmentConfig.MachineStore[srv];
            DsUser user = (DsUser)EnvironmentConfig.UserStore[EnvironmentConfig.User.ParentDomainAdmin];

            string dn = ldapAdapter.GetUserDn(server, user);

            string sid = LdapUtility.GetObjectStringSid(server, dn);

            BaseTestSite.Assert.IsNotNull(
                sid,
                "IDL_DRSCrackNames: cannot get SID of the object: {0}",
                dn);

            uint ret = 0;
            ret = drsTestClient.DrsBind(
                srv,
                EnvironmentConfig.User.ParentDomainAdmin,
                DRS_EXTENSIONS_IN_FLAGS.DRS_EXT_BASE
                );

            BaseTestSite.Assert.AreEqual<uint>(
                0,
                ret,
                "IDL_DRSBind: should return 0 with a success bind to DC");

            // Actual DRSR call.
            ret = drsTestClient.DrsCrackNames(
                srv,
                DRS_MSG_CRACKREQ_FLAGS.NONE,
                (uint)DS_NAME_FORMAT.DS_SID_OR_SID_HISTORY_NAME,
                (uint)DS_NAME_FORMAT.DS_FQDN_1779_NAME,
                new string[] { sid }
                );

            BaseTestSite.Assert.AreEqual<uint>(
                0,
                ret,
                "IDL_DRSCrackNames: return value should be 0"
                );

            // Unbind
            ret = drsTestClient.DrsUnbind(srv);
            BaseTestSite.Assert.AreEqual<uint>(
                0,
                ret,
                "IDL_DRSUnbind: return value should be 0");
        }
        #endregion

        #region DRSCrackNames_FQDN_1779_From_Unknown_Name
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-DRSR")]
        [TestMethod]
        [Priority(0)]
        [SupportedADType(ADInstanceType.DS)]
        [TestCategory("Win2003")]
        [Description("Find the FQDN using a pre-defined search order, to guess the input format of the name.")]
        public void DRSR_DRSCrackNames_FQDN_1779_From_Unknown_Name()
        {
            DrsrTestChecker.Check();
            // Init the data.
            EnvironmentConfig.Machine srv = EnvironmentConfig.Machine.WritableDC1;
            DsServer server = (DsServer)EnvironmentConfig.MachineStore[srv];
            DsUser user = EnvironmentConfig.UserStore[EnvironmentConfig.User.ParentDomainAdmin];
            string userDn = ldapAdapter.GetUserDn(server, user);

            Guid? guid = LdapUtility.GetObjectGuid(server, userDn);

            BaseTestSite.Assert.IsNotNull(
                guid,
                "IDL_DRSCrackNames: cannot get GUID of the object: {0}",
                userDn);

            uint ret = 0;
            ret = drsTestClient.DrsBind(
                srv,
                EnvironmentConfig.User.ParentDomainAdmin,
                DRS_EXTENSIONS_IN_FLAGS.DRS_EXT_BASE
                );

            BaseTestSite.Assert.AreEqual<uint>(
                0,
                ret,
                "IDL_DRSBind: should return 0 with a success bind to DC");

            // Actual DRSR call.
            ret = drsTestClient.DrsCrackNames(
                srv,
                DRS_MSG_CRACKREQ_FLAGS.NONE,
                (uint)DS_NAME_FORMAT.DS_UNKNOWN_NAME,
                (uint)DS_NAME_FORMAT.DS_FQDN_1779_NAME,
                new string[] { "{" + guid.Value.ToString() + "}" }
                );

            BaseTestSite.Assert.AreEqual<uint>(
                0,
                ret,
                "IDL_DRSCrackNames: return value should be 0"
                );

            // Unbind
            ret = drsTestClient.DrsUnbind(srv);
            BaseTestSite.Assert.AreEqual<uint>(
                0,
                ret,
                "IDL_DRSUnbind: return value should be 0");
        }
        #endregion

        #region DRSCrackNames_FQDN_1779_From_NT4_Account_Name_Sans_Domain
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-DRSR")]
        [TestMethod]
        [Priority(0)]
        [SupportedADType(ADInstanceType.DS)]
        [TestCategory("Win2003")]
        [Description("Find the FQDN using a sAMAccountName")]
        public void DRSR_DRSCrackNames_FQDN_1779_From_NT4_Account_Name_Sans_Domain()
        {
            DrsrTestChecker.Check();
            // Init the data.
            EnvironmentConfig.Machine srv = EnvironmentConfig.Machine.WritableDC1;
            DsServer server = (DsServer)EnvironmentConfig.MachineStore[srv];
            DsUser user = EnvironmentConfig.UserStore[EnvironmentConfig.User.ParentDomainUser];
            string userDn = ldapAdapter.GetUserDn(server, user);

            string samAcc = ldapAdapter.GetAttributeValueInString(server, userDn, "sAMAccountName");

            BaseTestSite.Assert.IsNotNull(
                samAcc,
                "IDL_DRSCrackNames: cannot get sAMAccountName attribute of the object: {0}",
                userDn);

            uint ret = 0;
            ret = drsTestClient.DrsBind(
                srv,
                EnvironmentConfig.User.ParentDomainAdmin,
                DRS_EXTENSIONS_IN_FLAGS.DRS_EXT_BASE
                );

            BaseTestSite.Assert.AreEqual<uint>(
                0,
                ret,
                "IDL_DRSBind: should return 0 with a success bind to DC");

            // Actual DRSR call.
            ret = drsTestClient.DrsCrackNames(
                srv,
                DRS_MSG_CRACKREQ_FLAGS.NONE,
                (uint)formatOffered_Values.DS_NT4_ACCOUNT_NAME_SANS_DOMAIN,
                (uint)DS_NAME_FORMAT.DS_FQDN_1779_NAME,
                new string[] { samAcc }
                );

            BaseTestSite.Assert.AreEqual<uint>(
                0,
                ret,
                "IDL_DRSCrackNames: return value should be 0"
                );

            // Unbind
            ret = drsTestClient.DrsUnbind(srv);
            BaseTestSite.Assert.AreEqual<uint>(
                0,
                ret,
                "IDL_DRSUnbind: return value should be 0");
        }
        #endregion

        #region DRSCrackNames_FQDN_1779_From_Alt_Security_Identities_Name
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-DRSR")]
        [TestMethod]
        [Priority(0)]
        [SupportedADType(ADInstanceType.DS)]
        [TestCategory("Win2003")]
        [Description("Get the FQDN name from the altSecurityIdentities Name")]
        public void DRSR_DRSCrackNames_FQDN_1779_From_Alt_Security_Identities_Name()
        {
            DrsrTestChecker.Check();
            // Init the data.
            EnvironmentConfig.Machine srv = EnvironmentConfig.Machine.WritableDC1;
            DsServer server = (DsServer)EnvironmentConfig.MachineStore[srv];
            DsUser user = EnvironmentConfig.UserStore[EnvironmentConfig.User.ParentDomainAdmin];
            string userDn = ldapAdapter.GetUserDn(server, user);

            // Add some attributes.
            ResultCode r = ldapAdapter.ModifyAttribute(
                server,
                userDn,
                new DirectoryAttribute("altSecurityIdentities", "admin_1" + EnvironmentConfig.DomainStore[DomainEnum.PrimaryDomain].DNSName)
                );

            BaseTestSite.Assert.IsTrue(
                r == ResultCode.Success,
                "IDL_DRSCrackNames: cannot modify altSecurityIdentities attribute of the object: {0}",
                userDn);

            string altSec = ldapAdapter.GetAttributeValueInString(
                server,
                userDn,
                "altSecurityIdentities"
                );

            BaseTestSite.Assert.IsNotNull(
                altSec,
                "IDL_DRSCrackNames: cannot get altSecurityIdentities attribute of the object: {0}",
                userDn);

            uint ret = 0;
            ret = drsTestClient.DrsBind(
                srv,
                EnvironmentConfig.User.ParentDomainAdmin,
                DRS_EXTENSIONS_IN_FLAGS.DRS_EXT_BASE
                );

            BaseTestSite.Assert.AreEqual<uint>(
                0,
                ret,
                "IDL_DRSBind: should return 0 with a success bind to DC");

            // Actual DRSR call.
            ret = drsTestClient.DrsCrackNames(
                srv,
                DRS_MSG_CRACKREQ_FLAGS.NONE,
                (uint)formatOffered_Values.DS_ALT_SECURITY_IDENTITIES_NAME,
                (uint)DS_NAME_FORMAT.DS_FQDN_1779_NAME,
                new string[] { altSec }
                );

            BaseTestSite.Assert.AreEqual<uint>(
                0,
                ret,
                "IDL_DRSCrackNames: return value should be 0"
                );

            // Unbind
            ret = drsTestClient.DrsUnbind(srv);
            BaseTestSite.Assert.AreEqual<uint>(
                0,
                ret,
                "IDL_DRSUnbind: return value should be 0");
        }
        #endregion

        #region DRSCrackNames_FQDN_1779_From_NT4_Account_Name_Sans_Domain_Ex
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-DRSR")]
        [TestMethod]
        [Priority(0)]
        [SupportedADType(ADInstanceType.DS)]
        [TestCategory("Win2003")]
        [Description("Find the FQDN using a sAMAccountName, return DS_NAME_ERROR_NOT_FOUND if account is invalid.")]
        public void DRSR_DRSCrackNames_FQDN_1779_From_NT4_Account_Name_Sans_Domain_Ex()
        {
            DrsrTestChecker.Check();
            // Init the data.
            EnvironmentConfig.Machine srv = EnvironmentConfig.Machine.WritableDC1;
            DsServer server = (DsServer)EnvironmentConfig.MachineStore[srv];
            DsUser user = EnvironmentConfig.UserStore[EnvironmentConfig.User.ParentDomainUser];
            string userDn = ldapAdapter.GetUserDn(server, user);

            string samAcc = ldapAdapter.GetAttributeValueInString(server, userDn, "sAMAccountName");

            /*
            CrackNameUpdate update = new CrackNameUpdate();
            update.ldapAdapter = ldapAdpter;
            update.server = server;
            update.Dn = userDn;

            updateStorage.PushUpdate(update);
            */

            BaseTestSite.Assert.IsNotNull(
                samAcc,
                "IDL_DRSCrackNames: cannot get sAMAccountName attribute of the object: {0}",
                userDn);

            uint ret = 0;
            ret = drsTestClient.DrsBind(
                srv,
                EnvironmentConfig.User.ParentDomainAdmin,
                DRS_EXTENSIONS_IN_FLAGS.DRS_EXT_BASE
                );

            BaseTestSite.Assert.AreEqual<uint>(
                0,
                ret,
                "IDL_DRSBind: should return 0 with a success bind to DC");

            // Actual DRSR call.
            ret = drsTestClient.DrsCrackNames(
                srv,
                DRS_MSG_CRACKREQ_FLAGS.NONE,
                (uint)formatOffered_Values.DS_NT4_ACCOUNT_NAME_SANS_DOMAIN_EX,
                (uint)DS_NAME_FORMAT.DS_FQDN_1779_NAME,
                new string[] { samAcc }
                );

            BaseTestSite.Assert.AreEqual<uint>(
                0,
                ret,
                "IDL_DRSCrackNames: return value should be 0"
                );

            // Unbind
            ret = drsTestClient.DrsUnbind(srv);
            BaseTestSite.Assert.AreEqual<uint>(
                0,
                ret,
                "IDL_DRSUnbind: return value should be 0");
        }
        #endregion

        #region DRSCrackNames_FQDN_1779_From_String_SID_Name
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-DRSR")]
        [TestMethod]
        [Priority(0)]
        [SupportedADType(ADInstanceType.DS)]
        [TestCategory("Win2003")]
        [Description("Find the FQDN using a string SID name.")]
        public void DRSR_DRSCrackNames_FQDN_1779_From_String_SID_Name()
        {
            DrsrTestChecker.Check();
            // Init the data.
            EnvironmentConfig.Machine srv = EnvironmentConfig.Machine.WritableDC1;
            DsServer server = (DsServer)EnvironmentConfig.MachineStore[srv];
            DsUser user = (DsUser)EnvironmentConfig.UserStore[EnvironmentConfig.User.ParentDomainAdmin];

            string dn = ldapAdapter.GetUserDn(server, user);

            string sid = LdapUtility.GetObjectStringSid(server, dn);

            BaseTestSite.Assert.IsNotNull(
                sid,
                "IDL_DRSCrackNames: cannot get objectSid attribute of the object: {0}",
                dn);

            uint ret = 0;
            ret = drsTestClient.DrsBind(
                srv,
                EnvironmentConfig.User.ParentDomainAdmin,
                DRS_EXTENSIONS_IN_FLAGS.DRS_EXT_BASE
                );

            BaseTestSite.Assert.AreEqual<uint>(
                0,
                ret,
                "IDL_DRSBind: should return 0 with a success bind to DC");

            // Actual DRSR call.
            ret = drsTestClient.DrsCrackNames(
                srv,
                DRS_MSG_CRACKREQ_FLAGS.NONE,
                (uint)formatOffered_Values.DS_STRING_SID_NAME,
                (uint)DS_NAME_FORMAT.DS_FQDN_1779_NAME,
                new string[] { sid }
                );

            BaseTestSite.Assert.AreEqual<uint>(
                0,
                ret,
                "IDL_DRSCrackNames: return value should be 0"
                );

            // Unbind
            ret = drsTestClient.DrsUnbind(srv);
            BaseTestSite.Assert.AreEqual<uint>(
                0,
                ret,
                "IDL_DRSUnbind: return value should be 0");
        }
        #endregion

        #region DRSCrackNames_FQDN_1779_From_User_Principal_Name_And_AltSecId
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-DRSR")]
        [TestMethod]
        [Priority(0)]
        [SupportedADType(ADInstanceType.DS)]
        [TestCategory("Win2003")]
        [Description("Get the FQDN name from the altSecurityIdentities and userPrincipalName Name")]
        public void DRSR_DRSCrackNames_FQDN_1779_From_User_Principal_Name_And_AltSecId()
        {
            DrsrTestChecker.Check();
            // Init the data.
            EnvironmentConfig.Machine srv = EnvironmentConfig.Machine.WritableDC1;
            DsServer server = (DsServer)EnvironmentConfig.MachineStore[srv];
            DsUser user = EnvironmentConfig.UserStore[EnvironmentConfig.User.ParentDomainUser];
            string userDn = ldapAdapter.GetUserDn(server, user);

            string upn = "drsr_testObj@" + EnvironmentConfig.DomainStore[DomainEnum.PrimaryDomain].DNSName;

            ResultCode r = ldapAdapter.ModifyAttribute(
                server,
                userDn,
                new DirectoryAttribute("userPrincipalName", upn)
                );

            BaseTestSite.Assert.AreEqual<ResultCode>(
                ResultCode.Success,
                r,
                "IDL_DRSCrackNames: cannot modify the userPrincipalName attribute");


            uint ret = 0;
            ret = drsTestClient.DrsBind(
                srv,
                EnvironmentConfig.User.ParentDomainAdmin,
                DRS_EXTENSIONS_IN_FLAGS.DRS_EXT_BASE
                );

            BaseTestSite.Assert.AreEqual<uint>(
                0,
                ret,
                "IDL_DRSBind: should return 0 with a success bind to DC");

            // Actual DRSR call.
            ret = drsTestClient.DrsCrackNames(
                srv,
                DRS_MSG_CRACKREQ_FLAGS.NONE,
                (uint)formatOffered_Values.DS_USER_PRINCIPAL_NAME_AND_ALTSECID,
                (uint)DS_NAME_FORMAT.DS_FQDN_1779_NAME,
                new string[] { upn }
                );

            BaseTestSite.Assert.AreEqual<uint>(
                0,
                ret,
                "IDL_DRSCrackNames: return value should be 0"
                );

            // Unbind
            ret = drsTestClient.DrsUnbind(srv);
            BaseTestSite.Assert.AreEqual<uint>(
                0,
                ret,
                "IDL_DRSUnbind: return value should be 0");
        }
        #endregion

        #region DRSCrackNames_LDAP_Display_Name_From_Schema_Guid
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-DRSR")]
        [TestMethod]
        [Priority(0)]
        [TestCategory("Win2003")]
        [Description("Get the LDAP display name from schema GUID.")]
        public void DRSR_DRSCrackNames_LDAP_Display_Name_From_Schema_Guid()
        {
            DrsrTestChecker.Check();
            // Init the data.
            EnvironmentConfig.Machine srv = EnvironmentConfig.Machine.WritableDC1;
            DsServer server = (DsServer)EnvironmentConfig.MachineStore[srv];

            RootDSE rootDse = LdapUtility.GetRootDSE(server);

            string schemaNc = rootDse.schemaNamingContext;

            // Try to look up CN=Site class.
            byte[] guidByte = ldapAdapter.GetAttributeValueInBytes(
                server,
                "CN=Site," + schemaNc,
                "schemaIDGUID");

            BaseTestSite.Assert.IsNotNull(
                guidByte,
                "IDL_DRSCrackNames: cannot read the schemaIDGUID attribute of object {0}",
                "CN=User," + schemaNc
                );

            Guid guid = new Guid(guidByte);

            uint ret = 0;
            ret = drsTestClient.DrsBind(
                srv,
                EnvironmentConfig.User.ParentDomainAdmin,
                DRS_EXTENSIONS_IN_FLAGS.DRS_EXT_BASE
                );

            BaseTestSite.Assert.AreEqual<uint>(
                0,
                ret,
                "IDL_DRSBind: should return 0 with a success bind to DC");

            // Actual DRSR call.
            ret = drsTestClient.DrsCrackNames(
                srv,
                DRS_MSG_CRACKREQ_FLAGS.NONE,
                (uint)formatOffered_Values.DS_MAP_SCHEMA_GUID,
                (uint)DS_NAME_FORMAT.DS_DISPLAY_NAME,
                new string[] { "{" + guid.ToString() + "}" }
                );

            BaseTestSite.Assert.AreEqual<uint>(
                0,
                ret,
                "IDL_DRSCrackNames: return value should be 0"
                );

            // Unbind
            ret = drsTestClient.DrsUnbind(srv);
            BaseTestSite.Assert.AreEqual<uint>(
                0,
                ret,
                "IDL_DRSUnbind: return value should be 0");
        }
        #endregion

        #endregion
    }
}
