// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Protocols.TestTools;
using Microsoft.Protocols.TestTools.StackSdk.ActiveDirectory.Drsr;
using Microsoft.Protocols.TestSuites.ActiveDirectory.Common;

namespace Microsoft.Protocols.TestSuites.ActiveDirectory.Drsr
{
    [TestClass]
    public class DrsrKCCTests : DrsrTestClassBase
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

        #region Test Cases
        /// <summary>
        /// test ExecuteKCC
        /// </summary>
        [BVT]
        [TestCategory("Win2003")]
        [ServerType(DcServerTypes.Any)]
        [SupportedADType(ADInstanceType.Both)]
        [Priority(0)]
        [Description("This test case call IDL_DRSExecuteKCC to trigger DC to execute KCC synchronously")]
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-DRSR")]
        [TestMethod]
        public void DRSR_DRSExecuteKCC_Success_KCC_Sync()
        {
            DrsrTestChecker.Check();
            drsTestClient.DrsBind(EnvironmentConfig.Machine.WritableDC1, EnvironmentConfig.User.ParentDomainAdmin, DRS_EXTENSIONS_IN_FLAGS.DRS_EXT_BASE);

            uint ret = drsTestClient.DrsExecuteKCC(EnvironmentConfig.Machine.WritableDC1, false);
        }

        /// <summary>
        /// test ExecuteKCC
        /// </summary>
        [TestCategory("Win2003")]
        [ServerType(DcServerTypes.Any)]
        [SupportedADType(ADInstanceType.Both)]
        [Priority(0)]
        [Description("This test case call IDL_DRSExecuteKCC to trigger DC to execute KCC asynchronously")]
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-DRSR")]
        [TestMethod]
        public void DRSR_DRSExecuteKCC_Success_KCC_Async()
        {
            DrsrTestChecker.Check();
            drsTestClient.DrsBind(EnvironmentConfig.Machine.WritableDC1, EnvironmentConfig.User.ParentDomainAdmin, DRS_EXTENSIONS_IN_FLAGS.DRS_EXT_BASE);

            uint ret = drsTestClient.DrsExecuteKCC(EnvironmentConfig.Machine.WritableDC1, true);
        }

        [BVT]
        [TestCategory("Win2003")]
        [ServerType(DcServerTypes.Any)]
        [SupportedADType(ADInstanceType.Both)]
        [Priority(0)]
        [Description("Delete a replication source and add it back with Async flag")]
        [TestCategory("SDC")]
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-DRSR")]
        [TestMethod]
        public void DRSR_DRSReplicaAdd_V1_Success_WithAsyncFlag()
        {
            DrsrTestChecker.Check();
            DRSReplicaAdd_Success_WithAsyncFlag(DRS_MSG_REPADD_Versions.V1);
        }

        [BVT]
        [TestCategory("Win2003")]
        [ServerType(DcServerTypes.Any)]
        [SupportedADType(ADInstanceType.Both)]
        [Priority(0)]
        [Description("Delete a replication source and add it back with Async flag")]
        [TestCategory("SDC")]
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-DRSR")]
        [TestMethod]
        public void DRSR_DRSReplicaAdd_V2_Success_WithAsyncFlag()
        {
            DrsrTestChecker.Check();
            DRSReplicaAdd_Success_WithAsyncFlag(DRS_MSG_REPADD_Versions.V2);
        }

        [BVT]
        [TestCategory("Winv1803")]
        [ServerType(DcServerTypes.Any)]
        [SupportedADType(ADInstanceType.Both)]
        [Priority(0)]
        [Description("Delete a replication source and add it back with Async flag")]
        [TestCategory("SDC")]
        [TestCategory("PDC")]
        [TestCategory("DomainWinV1803")]
        [TestCategory("MS-DRSR")]
        [TestMethod]
        public void DRSR_DRSReplicaAdd_V3_Success_WithAsyncFlag()
        {
            DrsrTestChecker.Check();
            DRSReplicaAdd_Success_WithAsyncFlag(DRS_MSG_REPADD_Versions.V3);
        }

        [BVT]
        [TestCategory("Win2003")]
        [ServerType(DcServerTypes.Any)]
        [SupportedADType(ADInstanceType.Both)]
        [Priority(0)]
        [Description("Delete a replication source and add it back with DRS_WRIT_REP flag")]
        [TestCategory("SDC")]
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-DRSR")]
        [TestMethod]
        public void DRSR_DRSReplicaAdd_V1_Success_With0x10Flag()
        {
            DrsrTestChecker.Check();
            DelReplicaSource delUpdate = new DelReplicaSource(EnvironmentConfig.Machine.WritableDC1, EnvironmentConfig.Machine.WritableDC2, DRS_OPTIONS.DRS_WRIT_REP, EnvironmentConfig.User.ParentDomainAdmin);

            BaseTestSite.Assert.IsTrue(UpdatesStorage.GetInstance().PushUpdate(delUpdate), "IDL_DRSReplicaAdd: Need to delete a replication source first");

            uint ret = drsTestClient.DrsBind(EnvironmentConfig.Machine.WritableDC1, EnvironmentConfig.User.ParentDomainAdmin, DRS_EXTENSIONS_IN_FLAGS.DRS_EXT_BASE);

            drsTestClient.DrsReplicaAdd(EnvironmentConfig.Machine.WritableDC1, DRS_MSG_REPADD_Versions.V1, (DsServer)EnvironmentConfig.MachineStore[EnvironmentConfig.Machine.WritableDC2], DRS_OPTIONS.DRS_WRIT_REP);
        }

        [TestCategory("Win2003")]
        [ServerType(DcServerTypes.Any)]
        [SupportedADType(ADInstanceType.Both)]
        [Priority(0)]
        [Description("Delete a replication source and add it back with DRS_WRIT_REP flag")]
        [TestCategory("SDC")]
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-DRSR")]
        [TestMethod]
        public void DRSR_DRSReplicaDel_V1_Async()
        {
            DrsrTestChecker.Check();
            RecoverReplicaSource delUpdate = new RecoverReplicaSource(EnvironmentConfig.Machine.WritableDC1, EnvironmentConfig.User.ParentDomainAdmin);

            UpdatesStorage.GetInstance().PushUpdate(delUpdate);

            uint ret = drsTestClient.DrsBind(EnvironmentConfig.Machine.WritableDC1, EnvironmentConfig.User.ParentDomainAdmin, DRS_EXTENSIONS_IN_FLAGS.DRS_EXT_BASE);

            drsTestClient.DrsReplicaDel(EnvironmentConfig.Machine.WritableDC1, (DsServer)EnvironmentConfig.MachineStore[EnvironmentConfig.Machine.WritableDC2], DRS_OPTIONS.DRS_ASYNC_OP);
        }


        [TestCategory("Win2003")]
        [ServerType(DcServerTypes.Any)]
        [SupportedADType(ADInstanceType.Both)]
        [Priority(0)]
        [Description("Delete a replication source and add it back with DRS_WRIT_REP flag")]
        [TestCategory("SDC")]
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-DRSR")]
        [TestMethod]
        public void DRSR_DRSReplicaDel_V1_With0x1000Flag()
        {
            DrsrTestChecker.Check();
            NeedRepSourceUpdate need = new NeedRepSourceUpdate(EnvironmentConfig.Machine.WritableDC1, EnvironmentConfig.Machine.WritableDC2, NamingContext.ConfigNC);

            UpdatesStorage.GetInstance().PushUpdate(need);

            drsTestClient.SyncDCs(EnvironmentConfig.Machine.WritableDC1, EnvironmentConfig.Machine.WritableDC2);

            uint ret = drsTestClient.DrsBind(EnvironmentConfig.Machine.WritableDC1, EnvironmentConfig.User.ParentDomainAdmin, DRS_EXTENSIONS_IN_FLAGS.DRS_EXT_BASE);

            drsTestClient.DrsReplicaDel(EnvironmentConfig.Machine.WritableDC1, (DsServer)EnvironmentConfig.MachineStore[EnvironmentConfig.Machine.WritableDC2], DRS_OPTIONS.DRS_LOCAL_ONLY);
        }

        [BVT]
        [TestCategory("Win2003")]
        [ServerType(DcServerTypes.Any)]
        [SupportedADType(ADInstanceType.Both)]
        [Priority(0)]
        [Description("Delete a replication source and add it back with DRS_WRIT_REP flag")]
        [TestCategory("SDC")]
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-DRSR")]
        [TestMethod]
        public void DRSR_DRSReplicaAdd_V2_Success_With0x10Flag()
        {
            DrsrTestChecker.Check();
            DRSR_DRSReplicaAdd_Success_With0x10Flag(DRS_MSG_REPADD_Versions.V2);
        }

        [BVT]
        [TestCategory("Win2003")]
        [ServerType(DcServerTypes.Any)]
        [SupportedADType(ADInstanceType.Both)]
        [Priority(0)]
        [Description("Modify a replication source")]
        [TestCategory("SDC")]
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-DRSR")]
        [TestMethod]
        public void DRSR_DRSReplicaModify_V1_Success()
        {
            DrsrTestChecker.Check();
            NeedRepSourceUpdate need = new NeedRepSourceUpdate(EnvironmentConfig.Machine.WritableDC1, EnvironmentConfig.Machine.WritableDC2, NamingContext.ConfigNC);

            UpdatesStorage.GetInstance().PushUpdate(need);

            BaseTestSite.Assert.IsTrue(drsTestClient.SyncDCs(EnvironmentConfig.Machine.WritableDC1, EnvironmentConfig.Machine.WritableDC2),
                "IDL_DRSReplicaModify: need to sync DCs first");

            uint ret = drsTestClient.DrsBind(EnvironmentConfig.Machine.WritableDC1, EnvironmentConfig.User.ParentDomainAdmin, DRS_EXTENSIONS_IN_FLAGS.DRS_EXT_BASE);

            drsTestClient.DrsReplicaModify(EnvironmentConfig.Machine.WritableDC1, (DsServer)EnvironmentConfig.MachineStore[EnvironmentConfig.Machine.WritableDC2], DRS_OPTIONS.DRS_WRIT_REP, DRS_MSG_REPMOD_FIELDS.DRS_UPDATE_ADDRESS | DRS_MSG_REPMOD_FIELDS.DRS_UPDATE_FLAGS | DRS_MSG_REPMOD_FIELDS.DRS_UPDATE_SCHEDULE, 0);
        }



        [TestCategory("Win2003")]
        [ServerType(DcServerTypes.Any)]
        [SupportedADType(ADInstanceType.Both)]
        [Priority(0)]
        [Description("Modify a replication source")]
        [TestCategory("SDC")]
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-DRSR")]
        [TestMethod]
        public void DRSR_DRSReplicaModify_V1_Success_Async()
        {
            DrsrTestChecker.Check();

            uint ret = drsTestClient.DrsBind(EnvironmentConfig.Machine.WritableDC1, EnvironmentConfig.User.ParentDomainAdmin, DRS_EXTENSIONS_IN_FLAGS.DRS_EXT_BASE);

            drsTestClient.DrsReplicaModify(EnvironmentConfig.Machine.WritableDC1, (DsServer)EnvironmentConfig.MachineStore[EnvironmentConfig.Machine.WritableDC2], DRS_OPTIONS.DRS_WRIT_REP, DRS_MSG_REPMOD_FIELDS.DRS_UPDATE_ADDRESS | DRS_MSG_REPMOD_FIELDS.DRS_UPDATE_FLAGS | DRS_MSG_REPMOD_FIELDS.DRS_UPDATE_SCHEDULE, DRS_OPTIONS.DRS_ASYNC_OP);
        }

        [BVT]
        [TestCategory("Win2003")]
        [ServerType(DcServerTypes.Any)]
        [SupportedADType(ADInstanceType.Both)]
        [Priority(0)]
        [Description("Modify a replication source")]
        [TestCategory("SDC")]
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-DRSR")]
        [TestMethod]
        public void DRSR_DRSUpdateRefs_V1_Success_AddThenDel()
        {
            DrsrTestChecker.Check();
            DRSUpdateRefs_Success_AddThenDel(DrsUpdateRefs_Versions.V1);
        }

        [BVT]
        [TestCategory("Winv1803")]
        [ServerType(DcServerTypes.Any)]
        [SupportedADType(ADInstanceType.Both)]
        [Priority(0)]
        [Description("Modify a replication source")]
        [TestCategory("SDC")]
        [TestCategory("PDC")]
        [TestCategory("DomainWinV1803")]
        [TestCategory("MS-DRSR")]
        [TestMethod]
        public void DRSR_DRSUpdateRefs_V2_Success_AddThenDel()
        {
            DrsrTestChecker.Check();
            DRSUpdateRefs_Success_AddThenDel(DrsUpdateRefs_Versions.V2);
        }

        [TestCategory("Win2003,BreakEnvironment")]
        [ServerType(DcServerTypes.Any)]
        [SupportedADType(ADInstanceType.DS)]
        [RequireDcPartner]
        [Priority(0)]
        [Description("completely remove a nc from replication")]
        [Ignore]
        [BreakEnvironment]
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-DRSR")]
        [TestMethod]
        public void DRSR_DRSReplicaDel_V1_Success_With0x8000flag()
        {
            DrsrTestChecker.Check();
            Site.Log.Add(LogEntryKind.Checkpoint, "Remove replication relationship between DC1 and DC2 for AppNC");
            DelReplicaSource delUpdate = new DelReplicaSource(EnvironmentConfig.Machine.WritableDC1, EnvironmentConfig.Machine.WritableDC2, DRS_OPTIONS.NONE, EnvironmentConfig.User.ParentDomainAdmin, NamingContext.AppNC);

            drsTestClient.DrsBind(EnvironmentConfig.Machine.WritableDC2, EnvironmentConfig.User.ParentDomainAdmin, DRS_EXTENSIONS_IN_FLAGS.DRS_EXT_BASE);
            try
            {
                drsTestClient.DrsReplicaDel(EnvironmentConfig.Machine.WritableDC2, (DsServer)EnvironmentConfig.MachineStore[EnvironmentConfig.Machine.WritableDC1], DRS_OPTIONS.NONE, NamingContext.AppNC);
            }
            catch
            {
            }
            drsTestClient.DrsBind(EnvironmentConfig.Machine.WritableDC1, EnvironmentConfig.User.ParentDomainAdmin, DRS_EXTENSIONS_IN_FLAGS.DRS_EXT_BASE);

            Site.Log.Add(LogEntryKind.Checkpoint, "Fully remove replication on DC1 for AppNC with NO_SOURCE flag");
            drsTestClient.DrsReplicaDel(EnvironmentConfig.Machine.WritableDC1, (DsServer)EnvironmentConfig.MachineStore[EnvironmentConfig.Machine.WritableDC2], DRS_OPTIONS.DRS_NO_SOURCE, NamingContext.AppNC);
        }
        #endregion

        #region Private Methods
        private void DRSReplicaAdd_Success_WithAsyncFlag(DRS_MSG_REPADD_Versions ver)
        {
            DelReplicaSource delUpdate = new DelReplicaSource(EnvironmentConfig.Machine.WritableDC1, EnvironmentConfig.Machine.WritableDC2, DRS_OPTIONS.DRS_WRIT_REP, EnvironmentConfig.User.ParentDomainAdmin);

            BaseTestSite.Assert.IsTrue(UpdatesStorage.GetInstance().PushUpdate(delUpdate), "Need to delete a replication source firstly");

            uint ret = drsTestClient.DrsBind(EnvironmentConfig.Machine.WritableDC1, EnvironmentConfig.User.ParentDomainAdmin, DRS_EXTENSIONS_IN_FLAGS.DRS_EXT_BASE);

            drsTestClient.DrsReplicaAdd(
                EnvironmentConfig.Machine.WritableDC1,
                DRS_MSG_REPADD_Versions.V2,
                (DsServer)EnvironmentConfig.MachineStore[EnvironmentConfig.Machine.WritableDC2],
                DRS_OPTIONS.DRS_ASYNC_OP
                );
        }

        private void DRSR_DRSReplicaAdd_Success_With0x10Flag(DRS_MSG_REPADD_Versions ver)
        {
            DelReplicaSource delUpdate = new DelReplicaSource(EnvironmentConfig.Machine.WritableDC1, EnvironmentConfig.Machine.WritableDC2, DRS_OPTIONS.DRS_WRIT_REP, EnvironmentConfig.User.ParentDomainAdmin);

            BaseTestSite.Assert.IsTrue(UpdatesStorage.GetInstance().PushUpdate(delUpdate), "Need to delete a replication source firstly");

            uint ret = drsTestClient.DrsBind(EnvironmentConfig.Machine.WritableDC1, EnvironmentConfig.User.ParentDomainAdmin, DRS_EXTENSIONS_IN_FLAGS.DRS_EXT_BASE);

            drsTestClient.DrsReplicaAdd(
                EnvironmentConfig.Machine.WritableDC1,
                ver,
                (DsServer)EnvironmentConfig.MachineStore[EnvironmentConfig.Machine.WritableDC2],
                DRS_OPTIONS.DRS_WRIT_REP
                );
        }

        private void DRSUpdateRefs_Success_AddThenDel(DrsUpdateRefs_Versions ver)
        {
            uint ret = drsTestClient.DrsBind(EnvironmentConfig.Machine.WritableDC1, EnvironmentConfig.User.ParentDomainAdmin, DRS_EXTENSIONS_IN_FLAGS.DRS_EXT_BASE);

            try
            {
                BaseTestSite.Log.Add(LogEntryKind.Checkpoint, "Try to delete replication destination from repsTo for later testing. It MAY fails if there is no such record to delete");
                drsTestClient.DrsUpdateRefs(
                    EnvironmentConfig.Machine.WritableDC1,
                    ver,
                    (DsServer)EnvironmentConfig.MachineStore[EnvironmentConfig.Machine.WritableDC2],
                    DRS_OPTIONS.DRS_DEL_REF);
            }
            catch
            {
                BaseTestSite.Log.Add(LogEntryKind.Checkpoint, "No similar record to delete in repsTo. It's OK to continue");
            }

            drsTestClient.DrsUpdateRefs(
                EnvironmentConfig.Machine.WritableDC1,
                ver,
                (DsServer)EnvironmentConfig.MachineStore[EnvironmentConfig.Machine.WritableDC2],
                DRS_OPTIONS.DRS_ADD_REF);

            drsTestClient.DrsUpdateRefs(
                EnvironmentConfig.Machine.WritableDC1,
                ver,
                (DsServer)EnvironmentConfig.MachineStore[EnvironmentConfig.Machine.WritableDC2],
                DRS_OPTIONS.DRS_DEL_REF);
        }
        #endregion
    }
}
