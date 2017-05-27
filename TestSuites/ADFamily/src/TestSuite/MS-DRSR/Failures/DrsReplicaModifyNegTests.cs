// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Protocols.TestTools;
using Microsoft.Protocols.TestTools.StackSdk.ActiveDirectory.Drsr;
using Microsoft.Protocols.TestTools.StackSdk;
using System.Security.Principal;
using Microsoft.Protocols.TestSuites.ActiveDirectory.Common;

namespace Microsoft.Protocols.TestSuites.ActiveDirectory.Drsr.Failures
{
    [TestClass]
    public class DrsReplicaModifyNegTests : DrsrFailureTestClassBase
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

        [TestCategory("Win2003")]
        [ServerType(DcServerTypes.Any)]
        [SupportedADType(ADInstanceType.Both)]
        [Priority(2)]
        [Description("Modify a replication source with pszSourceDRA = null and uuidSourceDRA = null")]
        [TestCategory("SDC")]
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-DRSR")]
        [TestMethod]
        public void DRSR_DRSReplicaModify_Psz_Uuid_Null()
        {
            DrsrTestChecker.Check();
            NeedRepSourceUpdate need = new NeedRepSourceUpdate(EnvironmentConfig.Machine.WritableDC1, EnvironmentConfig.Machine.WritableDC2, NamingContext.ConfigNC);

            UpdatesStorage.GetInstance().PushUpdate(need);
            BaseTestSite.Assert.IsTrue(drsTestClient.SyncDCs(EnvironmentConfig.Machine.WritableDC1, EnvironmentConfig.Machine.WritableDC2),
                "need to sync DCs firstly");

            uint ret = drsTestClient.DrsBind(EnvironmentConfig.Machine.WritableDC1, EnvironmentConfig.User.ParentDomainAdmin, DRS_EXTENSIONS_IN_FLAGS.DRS_EXT_BASE);

            #region create request
            DRS_MSG_REPMOD req = drsTestClient.createDRS_MSG_REPMOD_Request(
                EnvironmentConfig.Machine.WritableDC1,
                (DsServer)EnvironmentConfig.MachineStore[EnvironmentConfig.Machine.WritableDC2],
                DRS_OPTIONS.DRS_WRIT_REP, DRS_MSG_REPMOD_FIELDS.DRS_UPDATE_ADDRESS | DRS_MSG_REPMOD_FIELDS.DRS_UPDATE_FLAGS | DRS_MSG_REPMOD_FIELDS.DRS_UPDATE_SCHEDULE,
                0
            );

            req.V1.pszSourceDRA = null;         //Invalid value
            req.V1.uuidSourceDRA = Guid.Empty;  //Invalid value
            #endregion

            #region execute and verify
            BaseTestSite.Log.Add(LogEntryKind.Checkpoint, "Begin to call IDL_DRSReplicaModify");
            ret = drsTestClient.DRSClient.DrsReplicaModify(EnvironmentConfig.DrsContextStore[EnvironmentConfig.Machine.WritableDC1], 1, req);

            BaseTestSite.Log.Add(LogEntryKind.Checkpoint, "End call IDL_DRSReplicaModify with return value " + ret);
            BaseTestSite.Assert.AreEqual<uint>((uint)Win32ErrorCode_32.ERROR_DS_DRA_INVALID_PARAMETER, ret, "IDL_DRSReplicaModify should return ERROR_DS_DRA_INVALID_PARAMETER");
            #endregion
        }

        [TestCategory("Win2003")]
        [ServerType(DcServerTypes.Any)]
        [SupportedADType(ADInstanceType.Both)]
        [Priority(2)]
        [Description("Modify a replication source with DRS_UPDATE_ADDRESS set and pszSourceDRA = null")]
        [TestCategory("SDC")]
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-DRSR")]
        [TestMethod]
        public void DRSR_DRSReplicaModify_ModifyFieldSet_Psz_Null()
        {
            DrsrTestChecker.Check();
            NeedRepSourceUpdate need = new NeedRepSourceUpdate(EnvironmentConfig.Machine.WritableDC1, EnvironmentConfig.Machine.WritableDC2, NamingContext.ConfigNC);

            UpdatesStorage.GetInstance().PushUpdate(need);
            BaseTestSite.Assert.IsTrue(drsTestClient.SyncDCs(EnvironmentConfig.Machine.WritableDC1, EnvironmentConfig.Machine.WritableDC2),
                "need to sync DCs firstly");

            uint ret = drsTestClient.DrsBind(EnvironmentConfig.Machine.WritableDC1, EnvironmentConfig.User.ParentDomainAdmin, DRS_EXTENSIONS_IN_FLAGS.DRS_EXT_BASE);

            #region create request
            DRS_MSG_REPMOD req = drsTestClient.createDRS_MSG_REPMOD_Request(
                EnvironmentConfig.Machine.WritableDC1,
                (DsServer)EnvironmentConfig.MachineStore[EnvironmentConfig.Machine.WritableDC2],
                DRS_OPTIONS.DRS_WRIT_REP, DRS_MSG_REPMOD_FIELDS.DRS_UPDATE_ADDRESS | DRS_MSG_REPMOD_FIELDS.DRS_UPDATE_FLAGS | DRS_MSG_REPMOD_FIELDS.DRS_UPDATE_SCHEDULE,
                0
            );
            req.V1.pszSourceDRA = null; //Invalid value
            #endregion

            #region execute and verify
            BaseTestSite.Log.Add(LogEntryKind.Checkpoint, "Begin to call IDL_DRSReplicaModify");
            ret = drsTestClient.DRSClient.DrsReplicaModify(EnvironmentConfig.DrsContextStore[EnvironmentConfig.Machine.WritableDC1], 1, req);

            BaseTestSite.Log.Add(LogEntryKind.Checkpoint, "End call IDL_DRSReplicaModify with return value " + ret);
            BaseTestSite.Assert.AreEqual<uint>((uint)Win32ErrorCode_32.ERROR_DS_DRA_INVALID_PARAMETER, ret, "IDL_DRSReplicaModify should return ERROR_DS_DRA_INVALID_PARAMETER");
            #endregion
        }

        [TestCategory("Win2003")]
        [ServerType(DcServerTypes.Any)]
        [SupportedADType(ADInstanceType.Both)]
        [Priority(2)]
        [Description("Modify a replication source with ulModifyFields = 0")]
        [TestCategory("SDC")]
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-DRSR")]
        [TestMethod]
        public void DRSR_DRSReplicaModify_ulModifyField_Zero()
        {
            DrsrTestChecker.Check();
            NeedRepSourceUpdate need = new NeedRepSourceUpdate(EnvironmentConfig.Machine.WritableDC1, EnvironmentConfig.Machine.WritableDC2, NamingContext.ConfigNC);

            UpdatesStorage.GetInstance().PushUpdate(need);
            BaseTestSite.Assert.IsTrue(drsTestClient.SyncDCs(EnvironmentConfig.Machine.WritableDC1, EnvironmentConfig.Machine.WritableDC2),
                "need to sync DCs firstly");

            uint ret = drsTestClient.DrsBind(EnvironmentConfig.Machine.WritableDC1, EnvironmentConfig.User.ParentDomainAdmin, DRS_EXTENSIONS_IN_FLAGS.DRS_EXT_BASE);

            #region create request
            DRS_MSG_REPMOD req = drsTestClient.createDRS_MSG_REPMOD_Request(
                EnvironmentConfig.Machine.WritableDC1,
                (DsServer)EnvironmentConfig.MachineStore[EnvironmentConfig.Machine.WritableDC2],
                DRS_OPTIONS.DRS_WRIT_REP, DRS_MSG_REPMOD_FIELDS.DRS_UPDATE_ADDRESS | DRS_MSG_REPMOD_FIELDS.DRS_UPDATE_FLAGS | DRS_MSG_REPMOD_FIELDS.DRS_UPDATE_SCHEDULE,
                0
            );

            req.V1.ulModifyFields = 0;  //Invalid value
            #endregion

            #region execute and verify
            BaseTestSite.Log.Add(LogEntryKind.Checkpoint, "Begin to call IDL_DRSReplicaModify");
            ret = drsTestClient.DRSClient.DrsReplicaModify(EnvironmentConfig.DrsContextStore[EnvironmentConfig.Machine.WritableDC1], 1, req);

            BaseTestSite.Log.Add(LogEntryKind.Checkpoint, "End call IDL_DRSReplicaModify with return value " + ret);
            BaseTestSite.Assert.AreEqual<uint>((uint)Win32ErrorCode_32.ERROR_DS_DRA_INVALID_PARAMETER, ret, "IDL_DRSReplicaModify should return ERROR_DS_DRA_INVALID_PARAMETER");
            #endregion
        }

        [TestCategory("Win2003")]
        [ServerType(DcServerTypes.Any)]
        [SupportedADType(ADInstanceType.Both)]
        [Priority(2)]
        [Description("Modify a replication source with ulOptions set to DRS_SYNC_ALL")]
        [TestCategory("SDC")]
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-DRSR")]
        [TestMethod]
        public void DRSR_DRSReplicaModify_ulOptions_DRS_SYNC_ALL()
        {
            DrsrTestChecker.Check();
            NeedRepSourceUpdate need = new NeedRepSourceUpdate(EnvironmentConfig.Machine.WritableDC1, EnvironmentConfig.Machine.WritableDC2, NamingContext.ConfigNC);

            UpdatesStorage.GetInstance().PushUpdate(need);
            BaseTestSite.Assert.IsTrue(drsTestClient.SyncDCs(EnvironmentConfig.Machine.WritableDC1, EnvironmentConfig.Machine.WritableDC2),
                "need to sync DCs firstly");

            uint ret = drsTestClient.DrsBind(EnvironmentConfig.Machine.WritableDC1, EnvironmentConfig.User.ParentDomainAdmin, DRS_EXTENSIONS_IN_FLAGS.DRS_EXT_BASE);

            #region create request
            DRS_MSG_REPMOD req = drsTestClient.createDRS_MSG_REPMOD_Request(
                EnvironmentConfig.Machine.WritableDC1,
                (DsServer)EnvironmentConfig.MachineStore[EnvironmentConfig.Machine.WritableDC2],
                DRS_OPTIONS.DRS_WRIT_REP, DRS_MSG_REPMOD_FIELDS.DRS_UPDATE_ADDRESS | DRS_MSG_REPMOD_FIELDS.DRS_UPDATE_FLAGS | DRS_MSG_REPMOD_FIELDS.DRS_UPDATE_SCHEDULE,
                0
            );
            req.V1.ulOptions = (uint)DRS_OPTIONS.DRS_SYNC_ALL; //Invalid Field
            #endregion

            #region execute and verify
            BaseTestSite.Log.Add(LogEntryKind.Checkpoint, "Begin to call IDL_DRSReplicaModify");
            ret = drsTestClient.DRSClient.DrsReplicaModify(EnvironmentConfig.DrsContextStore[EnvironmentConfig.Machine.WritableDC1], 1, req);

            BaseTestSite.Log.Add(LogEntryKind.Checkpoint, "End call IDL_DRSReplicaModify with return value " + ret);
            BaseTestSite.Assert.AreEqual<uint>((uint)Win32ErrorCode_32.ERROR_DS_DRA_INVALID_PARAMETER, ret, "IDL_DRSReplicaModify should return ERROR_DS_DRA_INVALID_PARAMETER");
            #endregion
        }

        [TestCategory("Win2003")]
        [ServerType(DcServerTypes.Any)]
        [SupportedADType(ADInstanceType.Both)]
        [Priority(2)]
        [Description("Modify a replication source with nc invalid ")]
        [TestCategory("SDC")]
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-DRSR")]
        [TestMethod]
        public void DRSR_DRSReplicaModify_NC_Invalid()
        {
            DrsrTestChecker.Check();
            NeedRepSourceUpdate need = new NeedRepSourceUpdate(EnvironmentConfig.Machine.WritableDC1, EnvironmentConfig.Machine.WritableDC2, NamingContext.ConfigNC);

            UpdatesStorage.GetInstance().PushUpdate(need);
            BaseTestSite.Assert.IsTrue(drsTestClient.SyncDCs(EnvironmentConfig.Machine.WritableDC1, EnvironmentConfig.Machine.WritableDC2),
                "need to sync DCs firstly");

            uint ret = drsTestClient.DrsBind(EnvironmentConfig.Machine.WritableDC1, EnvironmentConfig.User.ParentDomainAdmin, DRS_EXTENSIONS_IN_FLAGS.DRS_EXT_BASE);

            #region create request
            DRS_MSG_REPMOD req = drsTestClient.createDRS_MSG_REPMOD_Request(
                EnvironmentConfig.Machine.WritableDC1,
                (DsServer)EnvironmentConfig.MachineStore[EnvironmentConfig.Machine.WritableDC2],
                DRS_OPTIONS.DRS_WRIT_REP, DRS_MSG_REPMOD_FIELDS.DRS_UPDATE_ADDRESS | DRS_MSG_REPMOD_FIELDS.DRS_UPDATE_FLAGS | DRS_MSG_REPMOD_FIELDS.DRS_UPDATE_SCHEDULE,
                0
            );

            req.V1.pNC = DrsuapiClient.CreateDsName("InvalidNC", Guid.Empty, null); // Invalid NC is set
            #endregion

            #region execute and verify
            BaseTestSite.Log.Add(LogEntryKind.Checkpoint, "Begin to call IDL_DRSReplicaModify");
            ret = drsTestClient.DRSClient.DrsReplicaModify(EnvironmentConfig.DrsContextStore[EnvironmentConfig.Machine.WritableDC1], 1, req);

            BaseTestSite.Log.Add(LogEntryKind.Checkpoint, "End call IDL_DRSReplicaModify with return value " + ret);
            BaseTestSite.Assert.AreEqual<uint>((uint)Win32ErrorCode_32.ERROR_DS_DRA_BAD_NC, ret, "IDL_DRSReplicaModify should return ERROR_DS_DRA_BAD_NC");
            #endregion
        }

        [TestCategory("Win2003")]
        [ServerType(DcServerTypes.Any)]
        [SupportedADType(ADInstanceType.Both)]
        [Priority(2)]
        [Description("Modify a replication source with a domain user's privilege ")]
        [TestCategory("SDC")]
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-DRSR")]
        [TestMethod]
        public void DRSR_DRSReplicaModify_By_DomainUser()
        {
            DrsrTestChecker.Check();
            NeedRepSourceUpdate need = new NeedRepSourceUpdate(EnvironmentConfig.Machine.WritableDC1, EnvironmentConfig.Machine.WritableDC2, NamingContext.ConfigNC);

            UpdatesStorage.GetInstance().PushUpdate(need);
            BaseTestSite.Assert.IsTrue(drsTestClient.SyncDCs(EnvironmentConfig.Machine.WritableDC1, EnvironmentConfig.Machine.WritableDC2),
                "need to sync DCs firstly");
            // here cannot pass
            uint ret = drsTestClient.DrsBind(EnvironmentConfig.Machine.WritableDC1, EnvironmentConfig.User.ParentDomainUser, DRS_EXTENSIONS_IN_FLAGS.DRS_EXT_BASE); // use ParentDomainUser not admin

            #region create request
            DRS_MSG_REPMOD req = drsTestClient.createDRS_MSG_REPMOD_Request(
                EnvironmentConfig.Machine.WritableDC1,
                (DsServer)EnvironmentConfig.MachineStore[EnvironmentConfig.Machine.WritableDC2],
                DRS_OPTIONS.DRS_WRIT_REP, DRS_MSG_REPMOD_FIELDS.DRS_UPDATE_ADDRESS | DRS_MSG_REPMOD_FIELDS.DRS_UPDATE_FLAGS | DRS_MSG_REPMOD_FIELDS.DRS_UPDATE_SCHEDULE,
                0
            );
            #endregion

            #region execute and verify
            BaseTestSite.Log.Add(LogEntryKind.Checkpoint, "Begin to call IDL_DRSReplicaModify");
            ret = drsTestClient.DRSClient.DrsReplicaModify(EnvironmentConfig.DrsContextStore[EnvironmentConfig.Machine.WritableDC1], 1, req);

            BaseTestSite.Log.Add(LogEntryKind.Checkpoint, "End call IDL_DRSReplicaModify with return value " + ret);
            BaseTestSite.Assert.AreEqual<uint>((uint)Win32ErrorCode_32.ERROR_DS_DRA_ACCESS_DENIED, ret, "IDL_DRSReplicaModify should return ERROR_DS_DRA_ACCESS_DENIED");
            #endregion
        }

        [TestCategory("Win2003")]
        [ServerType(DcServerTypes.Any)]
        [SupportedADType(ADInstanceType.Both)]
        [Priority(2)]
        [Description("Modify a replication source with uuidSourceDRA invalid")]
        [TestCategory("SDC")]
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-DRSR")]
        [TestMethod]
        public void DRSR_DRSReplicaModify_Guid_Invalid()
        {
            DrsrTestChecker.Check();
            NeedRepSourceUpdate need = new NeedRepSourceUpdate(EnvironmentConfig.Machine.WritableDC1, EnvironmentConfig.Machine.WritableDC2, NamingContext.ConfigNC);

            UpdatesStorage.GetInstance().PushUpdate(need);
            BaseTestSite.Assert.IsTrue(drsTestClient.SyncDCs(EnvironmentConfig.Machine.WritableDC1, EnvironmentConfig.Machine.WritableDC2),
                "need to sync DCs firstly");

            uint ret = drsTestClient.DrsBind(EnvironmentConfig.Machine.WritableDC1, EnvironmentConfig.User.ParentDomainAdmin, DRS_EXTENSIONS_IN_FLAGS.DRS_EXT_BASE);

            #region create request
            DRS_MSG_REPMOD req = drsTestClient.createDRS_MSG_REPMOD_Request(
                EnvironmentConfig.Machine.WritableDC1,
                (DsServer)EnvironmentConfig.MachineStore[EnvironmentConfig.Machine.WritableDC2],
                DRS_OPTIONS.DRS_WRIT_REP, DRS_MSG_REPMOD_FIELDS.DRS_UPDATE_ADDRESS | DRS_MSG_REPMOD_FIELDS.DRS_UPDATE_FLAGS | DRS_MSG_REPMOD_FIELDS.DRS_UPDATE_SCHEDULE,
                0
            );

            req.V1.uuidSourceDRA = Guid.NewGuid(); // Invalid Guid

            #endregion

            #region execute and verify
            BaseTestSite.Log.Add(LogEntryKind.Checkpoint, "Begin to call IDL_DRSReplicaModify");
            ret = drsTestClient.DRSClient.DrsReplicaModify(EnvironmentConfig.DrsContextStore[EnvironmentConfig.Machine.WritableDC1], 1, req);

            BaseTestSite.Log.Add(LogEntryKind.Checkpoint, "End call IDL_DRSReplicaModify with return value " + ret);
            BaseTestSite.Assert.AreEqual<uint>((uint)Win32ErrorCode_32.ERROR_DS_DRA_NO_REPLICA, ret, "IDL_DRSReplicaModify should return ERROR_DS_DRA_NO_REPLICA");
            #endregion
        }


    }
}
