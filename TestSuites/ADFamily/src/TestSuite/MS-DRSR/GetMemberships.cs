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
using Microsoft.Protocols.TestSuites.ActiveDirectory.Common;

namespace Microsoft.Protocols.TestSuites.ActiveDirectory.Drsr
{
    [TestClass]
    public class GetMemberships : DrsrTestClassBase
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

        #region IDL_DRSGetMemberships

        #region DRSGetMemberships_Get_Groups_For_User
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-DRSR")]
        [TestMethod]
        [Priority(0)]
        [BVT]
        [SupportedADType(ADInstanceType.DS)]
        [TestCategory("Win2003")]
        [Description("Get non-transitive membership in groups for given objects that are confined to a given domain, excluding built-in groups and domain-local groups.")]
        public void DRSR_DRSGetMemberships_Get_Groups_For_User()
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

            DSNAME dsUser = ldapAdapter.GetDsName(
                server,
                ldapAdapter.GetUserDn(
                    server,
                    user
                )
            ).Value;

            ret = drsTestClient.DrsGetMemberships(
                srv,
                dwInVersion_Values.V1,
                dsUser,
                false,
                REVERSE_MEMBERSHIP_OPERATION_TYPE.RevMembGetGroupsForUser,
                ((AddsDomain)(server.Domain)).DomainNC);

            BaseTestSite.Assert.AreEqual<uint>(0, ret,
                "IDL_DRSGetMemberships: Checking return value - got: {0}, expect: {1}, return value should always be 0",
                ret, 0);
            // Unbind
            ret = drsTestClient.DrsUnbind(srv);
            BaseTestSite.Assert.AreEqual<uint>(0, ret,
                "IDL_DRSUnbind: Checking return value - got: {0}, expect: {1}, return value should always be 0",
                ret, 0);
        }
        #endregion

        #region DRSGetMemberships_Get_Alias_Membership
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-DRSR")]
        [TestMethod]
        [Priority(0)]
        [BVT]
        [SupportedADType(ADInstanceType.DS)]
        [TestCategory("Win2003")]
        [Description("Get non-transitive membership of an object in domain-local groups that are confined to a given domain.")]
        public void DRSR_DRSGetMemberships_Get_Alias_Membership()
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

            DSNAME dsUser = ldapAdapter.GetDsName(
                server,
                ldapAdapter.GetUserDn(
                    server,
                    user
                )
            ).Value;

            ret = drsTestClient.DrsGetMemberships(
                srv,
                dwInVersion_Values.V1,
                dsUser,
                false,
                REVERSE_MEMBERSHIP_OPERATION_TYPE.RevMembGetAliasMembership,
                ((AddsDomain)(server.Domain)).DomainNC);

            BaseTestSite.Assert.AreEqual<uint>(
                0,
                ret,
                "IDL_DRSGetMemberships: return value should be 0");

            // Unbind
            ret = drsTestClient.DrsUnbind(srv);
            BaseTestSite.Assert.AreEqual<uint>(
                0,
                ret,
                "IDL_DRSUnbind: return value should be 0");
        }
        #endregion

        #region DRSGetMemberships_Get_Account_Groups
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-DRSR")]
        [TestMethod]
        [Priority(0)]
        [BVT]
        [SupportedADType(ADInstanceType.DS)]
        [TestCategory("Win2003")]
        [Description("Get transitive membership for an object in all account groups in a given domain, excluding built-in groups.")]
        public void DRSR_DRSGetMemberships_Get_Account_Groups()
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

            DSNAME dsUser = ldapAdapter.GetDsName(
                server,
                ldapAdapter.GetUserDn(
                    server,
                    user
                )
            ).Value;

            ret = drsTestClient.DrsGetMemberships(
                srv,
                dwInVersion_Values.V1,
                dsUser,
                false,
                REVERSE_MEMBERSHIP_OPERATION_TYPE.RevMembGetAccountGroups,
                ((AddsDomain)(server.Domain)).DomainNC);

            BaseTestSite.Assert.AreEqual<uint>(
                0,
                ret,
                "IDL_DRSGetMemberships: return value should be 0");

            // Unbind
            ret = drsTestClient.DrsUnbind(srv);
            BaseTestSite.Assert.AreEqual<uint>(
                0,
                ret,
                "IDL_DRSUnbind: return value should be 0");
        }
        #endregion

        #region DRSGetMemberships_Get_Resource_Groups
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-DRSR")]
        [TestMethod]
        [Priority(0)]
        [BVT]
        [SupportedADType(ADInstanceType.DS)]
        [TestCategory("Win2003")]
        [Description("Get transitive membership for an object in all domain-local groups in a given domain, excluding built-in groups.")]
        public void DRSR_DRSGetMemberships_Get_Resource_Groups()
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

            DSNAME dsUser = ldapAdapter.GetDsName(
                server,
                ldapAdapter.GetUserDn(
                    server,
                    user
                )
            ).Value;

            ret = drsTestClient.DrsGetMemberships(
                srv,
                dwInVersion_Values.V1,
                dsUser,
                false,
                REVERSE_MEMBERSHIP_OPERATION_TYPE.RevMembGetResourceGroups,
                ((AddsDomain)(server.Domain)).DomainNC);

            BaseTestSite.Assert.AreEqual<uint>(
                0,
                ret,
                "IDL_DRSGetMemberships: return value should be 0");

            // Unbind
            ret = drsTestClient.DrsUnbind(srv);
            BaseTestSite.Assert.AreEqual<uint>(
                0,
                ret,
                "IDL_DRSUnbind: return value should be 0");
        }
        #endregion

        #region DRSGetMemberships_Get_Universal_Groups
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-DRSR")]
        [TestMethod]
        [Priority(0)]
        [BVT]
        [SupportedADType(ADInstanceType.DS)]
        [TestCategory("Win2003")]
        [Description("Get transitive membership for an object in all universal groups, excluding built-in groups.")]
        public void DRSR_DRSGetMemberships_Get_Universal_Groups()
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

            DSNAME dsUser = ldapAdapter.GetDsName(
                server,
                ldapAdapter.GetUserDn(
                    server,
                    user
                )
            ).Value;

            ret = drsTestClient.DrsGetMemberships(
                srv,
                dwInVersion_Values.V1,
                dsUser,
                false,
                REVERSE_MEMBERSHIP_OPERATION_TYPE.RevMembGetUniversalGroups,
                null);

            BaseTestSite.Assert.AreEqual<uint>(
                0,
                ret,
                "IDL_DRSGetMemberships: return value should be 0");

            // Unbind
            ret = drsTestClient.DrsUnbind(srv);
            BaseTestSite.Assert.AreEqual<uint>(
                0,
                ret,
                "IDL_DRSUnbind: return value should be 0");
        }
        #endregion

        #region DRSGetMemberships_Get_Group_Members_Transitive
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-DRSR")]
        [TestMethod]
        [Priority(0)]
        [BVT]
        [SupportedADType(ADInstanceType.DS)]
        [TestCategory("Win2003")]
        [Description("Get membership closure of members of a group based on the information present in the server's NC replicas, including the primary group.")]
        public void DRSR_DRSGetMemberships_Get_Group_Members_Transitive()
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
                string groupDn = "CN=Domain Users,CN=Users," + DrsrHelper.GetDNFromFQDN(ADCommonServerAdapter.Instance(Site).PrimaryDomainDnsName);
                DSNAME dsGroup = LdapUtility.CreateDSNameForObject(
                    server,
                   groupDn
                    );

                ret = drsTestClient.DrsGetMemberships(
                    srv,
                    dwInVersion_Values.V1,
                    dsGroup,
                    false,
                    REVERSE_MEMBERSHIP_OPERATION_TYPE.GroupMembersTransitive,
                    null);

                BaseTestSite.Assert.AreEqual<uint>(
                    0,
                    ret,
                    "IDL_DRSGetMemberships: return value should be 0");

                // Unbind
                ret = drsTestClient.DrsUnbind(srv);
                BaseTestSite.Assert.AreEqual<uint>(
                    0,
                    ret,
                    "IDL_DRSUnbind: return value should be 0");
           
        }
        #endregion

        #region DRSGetMemberships_Get_Global_Groups_Non_Transitive
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-DRSR")]
        [TestMethod]
        [Priority(0)]
        [BVT]
        [SupportedADType(ADInstanceType.DS)]
        [TestCategory("Win2003")]
        [Description("Get non-transitive membership in global groups, excluding built-in groups.")]
        public void DRSR_DRSGetMemberships_Get_Global_Groups_Non_Transitive()
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

            DSNAME dsUser = ldapAdapter.GetDsName(
                server,
                ldapAdapter.GetUserDn(
                    server,
                    user
                )
            ).Value;

            ret = drsTestClient.DrsGetMemberships(
                srv,
                dwInVersion_Values.V1,
                dsUser,
                false,
                REVERSE_MEMBERSHIP_OPERATION_TYPE.RevMembGlobalGroupsNonTransitive,
                ((AddsDomain)(server.Domain)).DomainNC);

            BaseTestSite.Assert.AreEqual<uint>(
                0,
                ret,
                "IDL_DRSGetMemberships: return value should be 0");

            // Unbind
            ret = drsTestClient.DrsUnbind(srv);
            BaseTestSite.Assert.AreEqual<uint>(
                0,
                ret,
                "IDL_DRSUnbind: return value should be 0");
        }
        #endregion
        #endregion

        #region IDL_DRSGetMemberships2
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-DRSR")]
        [TestMethod]
        [Priority(0)]
        [BVT]
        [SupportedADType(ADInstanceType.DS)]
        [TestCategory("Win2003")]
        [Description("Retrieve the group memberships of a sequence of objects.")]
        public void DRSR_DRSGetMemberships2()
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


            DSNAME dsUser = ldapAdapter.GetDsName(
                server,
                ldapAdapter.GetUserDn(
                    server,
                    user
                )
            ).Value;

            ret = drsTestClient.DrsGetMemberships2(
                srv,
                dwInVersion_Values.V1,
                dsUser,
                false,
                REVERSE_MEMBERSHIP_OPERATION_TYPE.RevMembGetGroupsForUser,
                ((AddsDomain)(server.Domain)).DomainNC);

            BaseTestSite.Assert.AreEqual<uint>(0, ret,
                "IDL_DRSGetMemberships2: Checking return value - got: {0}, expect: {1}, return value should always be 0",
                ret, 0);
            // Unbind
            ret = drsTestClient.DrsUnbind(srv);
            BaseTestSite.Assert.AreEqual<uint>(0, ret,
                "IDL_DRSUnbind: Checking return value - got: {0}, expect: {1}, return value should always be 0",
                ret, 0);
        }

        #endregion
    }
}
