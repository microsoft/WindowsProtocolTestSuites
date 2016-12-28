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
    public class RemoveDsTest : DrsrTestClassBase
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

            // Take snapshot of the VMs.
            VMSnapshotUpdate update = new VMSnapshotUpdate();
            updateStorage.PushUpdate(update);
        }

        protected override void TestCleanup()
        {
            // Revert the VMs to the most recent snapshots.
            IUpdate update = null;
            updateStorage.PopUpdate(ref update);

            base.TestCleanup();
        }
        #endregion

        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-DRSR")]
        [TestMethod]
        [Priority(0)]
        [BVT]
        [SupportedADType(ADInstanceType.DS)]
        [TestCategory("Win2003")]
        [Description("Calling IDL_DRSRemoveDsServer to verify whether the DC server is the last DC in the domain.")]
        public void DRSR_DRSRemoveDsServer_Verify_LastDC()
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

            string serverDn = server.ServerObjectName;
            string domainDn = server.Domain.Name;

            ret = drsTestClient.DrsRemoveDsServer(
                srv,
                IDL_DRSRemoveDsServer_dwInVersion_Values.V1,
                serverDn,
                domainDn,
                false);

            BaseTestSite.Assert.AreEqual<uint>(0, ret,
                "IDL_DRSRemoveDsServer: Checking return value - got: {0}, expect: {1}, return value should always be 0",
                ret, 0);
            // Unbind
            ret = drsTestClient.DrsUnbind(srv);
            BaseTestSite.Assert.AreEqual<uint>(0, ret,
                "IDL_DRSUnbind: Checking return value - got: {0}, expect: {1}, return value should always be 0",
                ret, 0);

        }

        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-DRSR")]
        [TestMethod]
        [Priority(0)]
        [RequireDcPartner]
        [BreakEnvironment]
        [SupportedADType(ADInstanceType.DS)]
        [Ignore]
        [TestCategory("Win2003,BreakEnvironment")]
        [Description("Calling IDL_DRSRemoveDsServer to remove a DC server from domain.")]
        public void DRSR_DRSRemoveDsServer_Remove_DC()
        {
            DrsrTestChecker.Check();
            // Init the data.
            EnvironmentConfig.Machine srv = EnvironmentConfig.Machine.WritableDC1;
            EnvironmentConfig.Machine dst = EnvironmentConfig.Machine.CDC;

            DsServer server = (DsServer)EnvironmentConfig.MachineStore[srv];
            DsServer child = (DsServer)EnvironmentConfig.MachineStore[dst];

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

            string serverDn = child.ServerObjectName;
            string domainDn = child.Domain.Name;

            ret = drsTestClient.DrsRemoveDsServer(
                srv,
                IDL_DRSRemoveDsServer_dwInVersion_Values.V1,
                serverDn,
                domainDn,
                true);  // DANGEROUS: We're actually going to remove this server!

            BaseTestSite.Assert.AreEqual<uint>(
                ret,
                0,
                "IDL_DRSRemoveDsServer: return value should be 0");

            // Unbind
            ret = drsTestClient.DrsUnbind(srv);
            BaseTestSite.Assert.AreEqual<uint>(
                0,
                ret,
                "IDL_DRSUnbind: return value should be 0");

        }

        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-DRSR")]
        [TestMethod]
        [Priority(0)]
        [BVT]
        [BreakEnvironment]
        [RequireDcPartner]
        [Ignore]
        [SupportedADType(ADInstanceType.DS)]
        [TestCategory("Win2003,BreakEnvironment")]
        [Description("Calling IDL_DRSRemoveDsDomain to remove a DC server from domain.")]
        public void DRSR_DRSRemoveDsDomain_Remove_Domain()
        {
            DrsrTestChecker.Check();
            // Init the data.
            EnvironmentConfig.Machine srv = EnvironmentConfig.Machine.WritableDC1;

            // The destination server in an external forest. The external forest will be
            // removed after this call.
            EnvironmentConfig.Machine dst = EnvironmentConfig.Machine.CDC;

            DsServer server = (DsServer)EnvironmentConfig.MachineStore[srv];

            DsServer child = (DsServer)EnvironmentConfig.MachineStore[dst];

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

            string domainDn = child.Domain.Name;

            // We call RemoveDsServer to remove an actual 
            // DC in the external forest, so that a crossRef object 
            // pointing to a removed NC will exists.
            string serverDn = child.ServerObjectName;
            ret = drsTestClient.DrsRemoveDsServer(
                srv,
                IDL_DRSRemoveDsServer_dwInVersion_Values.V1,
                serverDn,
                domainDn,
                true);

            BaseTestSite.Assert.AreEqual<uint>(
                0,
                ret,
                "IDL_DRSRemoveDsDomain: should return 0 after removal of server");

            // Note that by far, the NTDS DSA object of this server should be removed already.
            ret = drsTestClient.DrsRemoveDsDomain(
                srv,
                IDL_DRSRemoveDsDomain_dwInVersion_Values.V1,
                domainDn
                );

            BaseTestSite.Assert.AreEqual<uint>(
                ret,
                0,
                "IDL_DRSRemoveDsDomain: return value should be 0");

            // Unbind
            ret = drsTestClient.DrsUnbind(srv);
            BaseTestSite.Assert.AreEqual<uint>(
                0,
                ret,
                "IDL_DRSUnbind: return value should be 0");

        }
    }
}
