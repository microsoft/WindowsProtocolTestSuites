// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace GeneratedTests {
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Reflection;
    using Microsoft.SpecExplorer.Runtime.Testing;
    using Microsoft.Protocols.TestTools;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Microsoft.Protocols.TestSuites.MS_FRS2;
    
    [Microsoft.VisualStudio.TestTools.UnitTesting.TestClassAttribute()]
    public partial class TCtestScenario6 : PtfTestClassBase {
        
        public TCtestScenario6() {
            this.SetSwitch("graphtimeout", "12000");
            this.SetSwitch("codegenerationtimeout", "120000");
            this.SetSwitch("viewcollapselabels", "true");
            this.SetSwitch("displayrequirements", "false");
            this.SetSwitch("defaultparameterexpansion", "pairwise");
            this.SetSwitch("testclassbase", "ptf");
            this.SetSwitch("quiescencetimeout", "100000");
            this.SetSwitch("statebound", "30000");
            this.SetSwitch("stepbound", "60000");
            this.SetSwitch("pathbound", "30000");
            this.SetSwitch("stepsperstatebound", "1024");
        }
        
        #region Expect Delegates
        public delegate void InitializationDelegate1(FRS2Model.error_status_t @return);
        
        public delegate void EstablishConnectionDelegate1(FRS2Model.ProtocolVersionReturned upstreamProtocolVersion, FRS2Model.UpstreamFlagValueReturned upstreamFlags, FRS2Model.error_status_t @return);
        
        public delegate void EstablishSessionDelegate1(FRS2Model.error_status_t @return);
        
        public delegate void AsyncPollDelegate1(FRS2Model.error_status_t @return);
        
        public delegate void RequestVersionVectorDelegate1(FRS2Model.error_status_t @return);
        
        public delegate void AsyncPollResponseEventDelegate1(FRS2Model.VVGeneration vvGen);
        
        public delegate void RequestUpdatesDelegate1(FRS2Model.error_status_t @return);
        
        public delegate void RequestUpdatesEventDelegate1(FRS2Model.FilePresense present, FRS2Model.UPDATE_STATUS updateStatus);
        
        public delegate void UpdateCancelDelegate1(FRS2Model.error_status_t @return);
        
        public delegate void CheckConnectivityDelegate1(FRS2Model.error_status_t @return);
        
        public delegate void RequestRecordsDelegate1(FRS2Model.error_status_t @return);
        
        public delegate void InitializeFileTransferAsyncDelegate1(FRS2Model.error_status_t @return);
        
        public delegate void RawGetFileDataDelegate1(FRS2Model.error_status_t @return);
        
        public delegate void RdcGetSignaturesDelegate1(FRS2Model.error_status_t @return);
        
        public delegate void RdcPushSourceNeedsDelegate1(FRS2Model.error_status_t @return);
        
        public delegate void RdcGetFileDataDelegate1(FRS2Model.error_status_t @return);
        
        public delegate void RdcCloseDelegate1(FRS2Model.error_status_t @return);
        
        public delegate void RawGetFileDataAsyncDelegate1(FRS2Model.error_status_t @return);
        
        public delegate void RdcGetFileDataAsyncDelegate1(FRS2Model.error_status_t @return);
        
        public delegate void BkupFsccValidationDelegate1();
        
        public delegate void SetTraditionalTestFlagDelegate1();
        
        public delegate void SetRdcGetSigTestFlagDelegate1();
        
        public delegate void SetPushSourceNeedsTestFlagDelegate1();
        
        public delegate void SetRdcCloseTestFlagDelegate1();
        
        public delegate void SetRdcGetSigFailTestFlagDelegate1();
        
        public delegate void SetRdcGetSigLevelTestFlagDelegate1();
        
        public delegate void SetPushSourceNeedsTestFlagForNeedCountDelegate1();
        
        public delegate void Validate_FSCC_BKUP_RequirementsDelegate1();
        
        public delegate void InitializeFileTransferAsyncEventDelegate1(FRS2Model.ServerContext context, FRS2Model.RDC_Sig_Level rdcsigLevel, bool isEOF);
        
        public delegate void RawGetFileDataResponseEventDelegate1(bool isEOF);
        
        public delegate void RdcGetFileDataEventDelegate1(FRS2Model.SizeReturned sizeReturned);
        
        public delegate void RequestRecordsEventDelegate1(FRS2Model.RecordsStatus status);
        #endregion
        
        #region Event Metadata
        static System.Reflection.MethodBase InitializationInfo = TestManagerHelpers.GetMethodInfo(typeof(Microsoft.Protocols.TestSuites.MS_FRS2.IFRS2ManagedAdapter), "Initialization", typeof(FRS2Model.OSVersion), typeof(Microsoft.Modeling.Map<System.Int32,FRS2Model.connectionProperty>), typeof(Microsoft.Modeling.Map<System.Int32,FRS2Model.FromServer>), typeof(Microsoft.Modeling.Map<System.String,System.Int32>), typeof(Microsoft.Modeling.Map<System.String,FRS2Model.ReplicationGroupTypes>), typeof(Microsoft.Modeling.Map<System.Int32,System.String>), typeof(Microsoft.Modeling.Map<System.Int32,FRS2Model.accessLevels>), typeof(Microsoft.Modeling.Map<System.Int32,FRS2Model.connectionProperty>));
        
        static System.Reflection.MethodBase EstablishConnectionInfo = TestManagerHelpers.GetMethodInfo(typeof(Microsoft.Protocols.TestSuites.MS_FRS2.IFRS2ManagedAdapter), "EstablishConnection", typeof(string), typeof(int), typeof(FRS2Model.ProtocolVersion), typeof(FRS2Model.ProtocolVersionReturned).MakeByRefType(), typeof(FRS2Model.UpstreamFlagValueReturned).MakeByRefType());
        
        static System.Reflection.MethodBase EstablishSessionInfo = TestManagerHelpers.GetMethodInfo(typeof(Microsoft.Protocols.TestSuites.MS_FRS2.IFRS2ManagedAdapter), "EstablishSession", typeof(int), typeof(int));
        
        static System.Reflection.MethodBase AsyncPollInfo = TestManagerHelpers.GetMethodInfo(typeof(Microsoft.Protocols.TestSuites.MS_FRS2.IFRS2ManagedAdapter), "AsyncPoll", typeof(int));
        
        static System.Reflection.MethodBase RequestVersionVectorInfo = TestManagerHelpers.GetMethodInfo(typeof(Microsoft.Protocols.TestSuites.MS_FRS2.IFRS2ManagedAdapter), "RequestVersionVector", typeof(int), typeof(int), typeof(int), typeof(FRS2Model.VERSION_REQUEST_TYPE), typeof(FRS2Model.VERSION_CHANGE_TYPE), typeof(FRS2Model.VVGeneration));
        
        static System.Reflection.EventInfo AsyncPollResponseEventInfo = TestManagerHelpers.GetEventInfo(typeof(Microsoft.Protocols.TestSuites.MS_FRS2.IFRS2ManagedAdapter), "AsyncPollResponseEvent");
        
        static System.Reflection.MethodBase RequestUpdatesInfo = TestManagerHelpers.GetMethodInfo(typeof(Microsoft.Protocols.TestSuites.MS_FRS2.IFRS2ManagedAdapter), "RequestUpdates", typeof(int), typeof(int), typeof(FRS2Model.versionVectorDiff));
        
        static System.Reflection.EventInfo RequestUpdatesEventInfo = TestManagerHelpers.GetEventInfo(typeof(Microsoft.Protocols.TestSuites.MS_FRS2.IFRS2ManagedAdapter), "RequestUpdatesEvent");
        
        static System.Reflection.MethodBase UpdateCancelInfo = TestManagerHelpers.GetMethodInfo(typeof(Microsoft.Protocols.TestSuites.MS_FRS2.IFRS2ManagedAdapter), "UpdateCancel", typeof(int), typeof(FRS2Model.FRS_UPDATE_CANCEL_DATA), typeof(int));
        
        static System.Reflection.MethodBase CheckConnectivityInfo = TestManagerHelpers.GetMethodInfo(typeof(Microsoft.Protocols.TestSuites.MS_FRS2.IFRS2ManagedAdapter), "CheckConnectivity", typeof(string), typeof(int));
        
        static System.Reflection.MethodBase RequestRecordsInfo = TestManagerHelpers.GetMethodInfo(typeof(Microsoft.Protocols.TestSuites.MS_FRS2.IFRS2ManagedAdapter), "RequestRecords", typeof(int), typeof(int));
        
        static System.Reflection.MethodBase InitializeFileTransferAsyncInfo = TestManagerHelpers.GetMethodInfo(typeof(Microsoft.Protocols.TestSuites.MS_FRS2.IFRS2ManagedAdapter), "InitializeFileTransferAsync", typeof(int), typeof(int), typeof(bool));
        
        static System.Reflection.MethodBase RawGetFileDataInfo = TestManagerHelpers.GetMethodInfo(typeof(Microsoft.Protocols.TestSuites.MS_FRS2.IFRS2ManagedAdapter), "RawGetFileData");
        
        static System.Reflection.MethodBase RdcGetSignaturesInfo = TestManagerHelpers.GetMethodInfo(typeof(Microsoft.Protocols.TestSuites.MS_FRS2.IFRS2ManagedAdapter), "RdcGetSignatures", typeof(FRS2Model.offset));
        
        static System.Reflection.MethodBase RdcPushSourceNeedsInfo = TestManagerHelpers.GetMethodInfo(typeof(Microsoft.Protocols.TestSuites.MS_FRS2.IFRS2ManagedAdapter), "RdcPushSourceNeeds");
        
        static System.Reflection.MethodBase RdcGetFileDataInfo = TestManagerHelpers.GetMethodInfo(typeof(Microsoft.Protocols.TestSuites.MS_FRS2.IFRS2ManagedAdapter), "RdcGetFileData", typeof(FRS2Model.BufferSize));
        
        static System.Reflection.MethodBase RdcCloseInfo = TestManagerHelpers.GetMethodInfo(typeof(Microsoft.Protocols.TestSuites.MS_FRS2.IFRS2ManagedAdapter), "RdcClose");
        
        static System.Reflection.MethodBase RawGetFileDataAsyncInfo = TestManagerHelpers.GetMethodInfo(typeof(Microsoft.Protocols.TestSuites.MS_FRS2.IFRS2ManagedAdapter), "RawGetFileDataAsync");
        
        static System.Reflection.MethodBase RdcGetFileDataAsyncInfo = TestManagerHelpers.GetMethodInfo(typeof(Microsoft.Protocols.TestSuites.MS_FRS2.IFRS2ManagedAdapter), "RdcGetFileDataAsync");
        
        static System.Reflection.MethodBase BkupFsccValidationInfo = TestManagerHelpers.GetMethodInfo(typeof(Microsoft.Protocols.TestSuites.MS_FRS2.IFRS2ManagedAdapter), "BkupFsccValidation");
        
        static System.Reflection.MethodBase SetTraditionalTestFlagInfo = TestManagerHelpers.GetMethodInfo(typeof(Microsoft.Protocols.TestSuites.MS_FRS2.IFRS2ManagedAdapter), "SetTraditionalTestFlag");
        
        static System.Reflection.MethodBase SetRdcGetSigTestFlagInfo = TestManagerHelpers.GetMethodInfo(typeof(Microsoft.Protocols.TestSuites.MS_FRS2.IFRS2ManagedAdapter), "SetRdcGetSigTestFlag");
        
        static System.Reflection.MethodBase SetPushSourceNeedsTestFlagInfo = TestManagerHelpers.GetMethodInfo(typeof(Microsoft.Protocols.TestSuites.MS_FRS2.IFRS2ManagedAdapter), "SetPushSourceNeedsTestFlag");
        
        static System.Reflection.MethodBase SetRdcCloseTestFlagInfo = TestManagerHelpers.GetMethodInfo(typeof(Microsoft.Protocols.TestSuites.MS_FRS2.IFRS2ManagedAdapter), "SetRdcCloseTestFlag");
        
        static System.Reflection.MethodBase SetRdcGetSigFailTestFlagInfo = TestManagerHelpers.GetMethodInfo(typeof(Microsoft.Protocols.TestSuites.MS_FRS2.IFRS2ManagedAdapter), "SetRdcGetSigFailTestFlag");
        
        static System.Reflection.MethodBase SetRdcGetSigLevelTestFlagInfo = TestManagerHelpers.GetMethodInfo(typeof(Microsoft.Protocols.TestSuites.MS_FRS2.IFRS2ManagedAdapter), "SetRdcGetSigLevelTestFlag");
        
        static System.Reflection.MethodBase SetPushSourceNeedsTestFlagForNeedCountInfo = TestManagerHelpers.GetMethodInfo(typeof(Microsoft.Protocols.TestSuites.MS_FRS2.IFRS2ManagedAdapter), "SetPushSourceNeedsTestFlagForNeedCount");
        
        static System.Reflection.MethodBase Validate_FSCC_BKUP_RequirementsInfo = TestManagerHelpers.GetMethodInfo(typeof(Microsoft.Protocols.TestSuites.MS_FRS2.IFRS2ManagedAdapter), "Validate_FSCC_BKUP_Requirements", typeof(FileStreamDataParser.ReplicatedFileStructure), typeof(bool));
        
        static System.Reflection.EventInfo InitializeFileTransferAsyncEventInfo = TestManagerHelpers.GetEventInfo(typeof(Microsoft.Protocols.TestSuites.MS_FRS2.IFRS2ManagedAdapter), "InitializeFileTransferAsyncEvent");
        
        static System.Reflection.EventInfo RawGetFileDataResponseEventInfo = TestManagerHelpers.GetEventInfo(typeof(Microsoft.Protocols.TestSuites.MS_FRS2.IFRS2ManagedAdapter), "RawGetFileDataResponseEvent");
        
        static System.Reflection.EventInfo RdcGetFileDataEventInfo = TestManagerHelpers.GetEventInfo(typeof(Microsoft.Protocols.TestSuites.MS_FRS2.IFRS2ManagedAdapter), "RdcGetFileDataEvent");
        
        static System.Reflection.EventInfo RequestRecordsEventInfo = TestManagerHelpers.GetEventInfo(typeof(Microsoft.Protocols.TestSuites.MS_FRS2.IFRS2ManagedAdapter), "RequestRecordsEvent");
        #endregion
        
        #region Adapter Instances
        private Microsoft.Protocols.TestSuites.MS_FRS2.IFRS2ManagedAdapter IFRS2ManagedAdapterInstance;
        #endregion
        
        #region Class Initialization and Cleanup
        [Microsoft.VisualStudio.TestTools.UnitTesting.ClassInitializeAttribute()]
        public static void ClassInitialize(Microsoft.VisualStudio.TestTools.UnitTesting.TestContext context) {
            PtfTestClassBase.Initialize(context);
        }
        
        [Microsoft.VisualStudio.TestTools.UnitTesting.ClassCleanupAttribute()]
        public static void ClassCleanup() {
            PtfTestClassBase.Cleanup();
        }
        #endregion
        
        #region Test Initialization and Cleanup
        protected override void TestInitialize() {
            this.InitializeTestManager();
            this.IFRS2ManagedAdapterInstance = ((IFRS2ManagedAdapter)(this.Manager.GetAdapter(typeof(IFRS2ManagedAdapter))));
            FRS2ManagedAdapter.PreCheck();
            this.Manager.Subscribe(AsyncPollResponseEventInfo, this.IFRS2ManagedAdapterInstance);
            this.Manager.Subscribe(RequestUpdatesEventInfo, this.IFRS2ManagedAdapterInstance);
            this.Manager.Subscribe(InitializeFileTransferAsyncEventInfo, this.IFRS2ManagedAdapterInstance);
            this.Manager.Subscribe(RawGetFileDataResponseEventInfo, this.IFRS2ManagedAdapterInstance);
            this.Manager.Subscribe(RdcGetFileDataEventInfo, this.IFRS2ManagedAdapterInstance);
            this.Manager.Subscribe(RequestRecordsEventInfo, this.IFRS2ManagedAdapterInstance);
        }
        
        protected override void TestCleanup() {
            base.TestCleanup();
            this.CleanupTestManager();
        }
        #endregion
        
        #region Test Starting in S0
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        [Microsoft.VisualStudio.TestTools.UnitTesting.DescriptionAttribute(@"MS-FRS2_R37, MS-FRS2_R436, MS-FRS2_R439, MS-FRS2_R455, MS-FRS2_R458, MS-FRS2_R448, MS-FRS2_R549, MS-FRS2_R552, MS-FRS2_R517, MS-FRS2_R520, MS-FRS2_R466, MS-FRS2_R556, MS-FRS2_R1020, MS-FRS2_R555, MS-FRS2_R486, MS-FRS2_R489, MS-FRS2_R93, MS-FRS2_R607, MS-FRS2_R93, MS-FRS2_R498, MS-FRS2_R607, MS-FRS2_R93, MS-FRS2_R94, MS-FRS2_R607, MS-FRS2_R93, MS-FRS2_R94, MS-FRS2_R607, MS-FRS2_R556, MS-FRS2_R1020, MS-FRS2_R555")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestCategory("AD")]
        [TestCategory("MS-FRS2")]
        [TestCategory("SDC")]
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        
        public virtual void FRS2_TCtestScenario6S0()
        {
            this.Manager.BeginTest("TCtestScenario6S0");
            this.Manager.Comment("reaching state \'S0\'");
            FRS2Model.error_status_t temp0;
            this.Manager.Comment(@"executing step 'call Initialization(Windows7,Map{1=>enabled,2=>enabled,3=>enabled,4=>disabled},Map{1=>exists,2=>exists,3=>exists,4=>exists},Map{""A""=>1,""B""=>2,""C""=>3,""D""=>4},Map{""A""=>sysvol,""B""=>protection,""C""=>sysvol,""D""=>sysvol},Map{1=>""A"",2=>""B"",3=>""C"",4=>""D""},Map{1=>writeOnly,2=>readWrite,3=>readOnly,4=>readWrite},Map{1=>enabled,2=>disabled,3=>enabled,4=>enabled})'");
            temp0 = this.IFRS2ManagedAdapterInstance.Initialization(FRS2Model.OSVersion.Windows7, new Microsoft.Modeling.Map<System.Int32,FRS2Model.connectionProperty>().Add(1, FRS2Model.connectionProperty.enabled).Add(2, FRS2Model.connectionProperty.enabled).Add(3, FRS2Model.connectionProperty.enabled).Add(4, FRS2Model.connectionProperty.disabled), new Microsoft.Modeling.Map<System.Int32,FRS2Model.FromServer>().Add(1, FRS2Model.FromServer.exists).Add(2, FRS2Model.FromServer.exists).Add(3, FRS2Model.FromServer.exists).Add(4, FRS2Model.FromServer.exists), new Microsoft.Modeling.Map<System.String,System.Int32>().Add("A", 1).Add("B", 2).Add("C", 3).Add("D", 4), new Microsoft.Modeling.Map<System.String,FRS2Model.ReplicationGroupTypes>().Add("A", FRS2Model.ReplicationGroupTypes.sysvol).Add("B", FRS2Model.ReplicationGroupTypes.protection).Add("C", FRS2Model.ReplicationGroupTypes.sysvol).Add("D", FRS2Model.ReplicationGroupTypes.sysvol), new Microsoft.Modeling.Map<System.Int32,System.String>().Add(1, "A").Add(2, "B").Add(3, "C").Add(4, "D"), new Microsoft.Modeling.Map<System.Int32,FRS2Model.accessLevels>().Add(1, FRS2Model.accessLevels.writeOnly).Add(2, FRS2Model.accessLevels.readWrite).Add(3, FRS2Model.accessLevels.readOnly).Add(4, FRS2Model.accessLevels.readWrite), new Microsoft.Modeling.Map<System.Int32,FRS2Model.connectionProperty>().Add(1, FRS2Model.connectionProperty.enabled).Add(2, FRS2Model.connectionProperty.disabled).Add(3, FRS2Model.connectionProperty.enabled).Add(4, FRS2Model.connectionProperty.enabled));
            this.Manager.Comment("reaching state \'S1\'");
            this.Manager.Comment("checking step \'return Initialization/ERROR_SUCCESS\'");
            this.Manager.Assert((temp0 == FRS2Model.error_status_t.ERROR_SUCCESS), String.Format("expected \'FRS2Model.error_status_t.ERROR_SUCCESS\', actual \'{0}\' (return of Initia" +
                        "lization, state S1)", TestManagerHelpers.Describe(temp0)));
            this.Manager.Comment("reaching state \'S10\'");
            FRS2Model.ProtocolVersionReturned temp1;
            FRS2Model.UpstreamFlagValueReturned temp2;
            FRS2Model.error_status_t temp3;
            this.Manager.Comment("executing step \'call EstablishConnection(\"A\",1,FRS_COMMUNICATION_PROTOCOL_VERSION" +
                    "_LONGHORN_SERVER,out _,out _)\'");
            temp3 = this.IFRS2ManagedAdapterInstance.EstablishConnection("A", 1, FRS2Model.ProtocolVersion.FRS_COMMUNICATION_PROTOCOL_VERSION_LONGHORN_SERVER, out temp1, out temp2);
            this.Manager.Checkpoint("MS-FRS2_R37");
            this.Manager.Checkpoint("MS-FRS2_R436");
            this.Manager.Checkpoint("MS-FRS2_R439");
            this.Manager.Comment("reaching state \'S15\'");
            this.Manager.Comment("checking step \'return EstablishConnection/[out Valid,out Valid]:ERROR_SUCCESS\'");
            this.Manager.Assert((temp1 == FRS2Model.ProtocolVersionReturned.Valid), String.Format("expected \'FRS2Model.ProtocolVersionReturned.Valid\', actual \'{0}\' (upstreamProtoco" +
                        "lVersion of EstablishConnection, state S15)", TestManagerHelpers.Describe(temp1)));
            this.Manager.Assert((temp2 == FRS2Model.UpstreamFlagValueReturned.Valid), String.Format("expected \'FRS2Model.UpstreamFlagValueReturned.Valid\', actual \'{0}\' (upstreamFlags" +
                        " of EstablishConnection, state S15)", TestManagerHelpers.Describe(temp2)));
            this.Manager.Assert((temp3 == FRS2Model.error_status_t.ERROR_SUCCESS), String.Format("expected \'FRS2Model.error_status_t.ERROR_SUCCESS\', actual \'{0}\' (return of Establ" +
                        "ishConnection, state S15)", TestManagerHelpers.Describe(temp3)));
            this.Manager.Comment("reaching state \'S20\'");
            FRS2Model.error_status_t temp4;
            this.Manager.Comment("executing step \'call EstablishSession(1,1)\'");
            temp4 = this.IFRS2ManagedAdapterInstance.EstablishSession(1, 1);
            this.Manager.Checkpoint("MS-FRS2_R455");
            this.Manager.Checkpoint("MS-FRS2_R458");
            this.Manager.Checkpoint("MS-FRS2_R448");
            this.Manager.Comment("reaching state \'S25\'");
            this.Manager.Comment("checking step \'return EstablishSession/ERROR_SUCCESS\'");
            this.Manager.Assert((temp4 == FRS2Model.error_status_t.ERROR_SUCCESS), String.Format("expected \'FRS2Model.error_status_t.ERROR_SUCCESS\', actual \'{0}\' (return of Establ" +
                        "ishSession, state S25)", TestManagerHelpers.Describe(temp4)));
            this.Manager.Comment("reaching state \'S30\'");
            FRS2Model.error_status_t temp5;
            this.Manager.Comment("executing step \'call AsyncPoll(1)\'");
            temp5 = this.IFRS2ManagedAdapterInstance.AsyncPoll(1);
            this.Manager.Checkpoint("MS-FRS2_R549");
            this.Manager.Checkpoint("MS-FRS2_R552");
            this.Manager.Comment("reaching state \'S35\'");
            this.Manager.Comment("checking step \'return AsyncPoll/ERROR_SUCCESS\'");
            this.Manager.Assert((temp5 == FRS2Model.error_status_t.ERROR_SUCCESS), String.Format("expected \'FRS2Model.error_status_t.ERROR_SUCCESS\', actual \'{0}\' (return of AsyncP" +
                        "oll, state S35)", TestManagerHelpers.Describe(temp5)));
            this.Manager.Comment("reaching state \'S39\'");
            FRS2Model.error_status_t temp6;
            this.Manager.Comment("executing step \'call RequestVersionVector(1,1,1,REQUEST_NORMAL_SYNC,CHANGE_ALL,Va" +
                    "lidValue)\'");
            temp6 = this.IFRS2ManagedAdapterInstance.RequestVersionVector(1, 1, 1, FRS2Model.VERSION_REQUEST_TYPE.REQUEST_NORMAL_SYNC, FRS2Model.VERSION_CHANGE_TYPE.CHANGE_ALL, FRS2Model.VVGeneration.ValidValue);
            this.Manager.Checkpoint("MS-FRS2_R517");
            this.Manager.Checkpoint("MS-FRS2_R520");
            this.Manager.Checkpoint("MS-FRS2_R466");
            this.Manager.Comment("reaching state \'S43\'");
            this.Manager.Comment("checking step \'return RequestVersionVector/ERROR_SUCCESS\'");
            this.Manager.Assert((temp6 == FRS2Model.error_status_t.ERROR_SUCCESS), String.Format("expected \'FRS2Model.error_status_t.ERROR_SUCCESS\', actual \'{0}\' (return of Reques" +
                        "tVersionVector, state S43)", TestManagerHelpers.Describe(temp6)));
            this.Manager.Comment("reaching state \'S47\'");
            int temp13 = this.Manager.ExpectEvent(this.QuiescenceTimeout, true, new ExpectedEvent(TCtestScenario6.AsyncPollResponseEventInfo, null, new AsyncPollResponseEventDelegate1(this.TCtestScenario6S0AsyncPollResponseEventChecker)), new ExpectedEvent(TCtestScenario6.AsyncPollResponseEventInfo, null, new AsyncPollResponseEventDelegate1(this.TCtestScenario6S0AsyncPollResponseEventChecker1)));
            if ((temp13 == 0)) {
                this.Manager.Comment("reaching state \'S51\'");
                FRS2Model.error_status_t temp7;
                this.Manager.Comment("executing step \'call RequestUpdates(1,1,valid)\'");
                temp7 = this.IFRS2ManagedAdapterInstance.RequestUpdates(1, 1, FRS2Model.versionVectorDiff.valid);
                this.Manager.Checkpoint("MS-FRS2_R486");
                this.Manager.Checkpoint("MS-FRS2_R489");
                this.Manager.Comment("reaching state \'S59\'");
                this.Manager.Comment("checking step \'return RequestUpdates/ERROR_SUCCESS\'");
                this.Manager.Assert((temp7 == FRS2Model.error_status_t.ERROR_SUCCESS), String.Format("expected \'FRS2Model.error_status_t.ERROR_SUCCESS\', actual \'{0}\' (return of Reques" +
                            "tUpdates, state S59)", TestManagerHelpers.Describe(temp7)));
                this.Manager.Comment("reaching state \'S63\'");
                int temp12 = this.Manager.ExpectEvent(this.QuiescenceTimeout, true, new ExpectedEvent(TCtestScenario6.RequestUpdatesEventInfo, null, new RequestUpdatesEventDelegate1(this.TCtestScenario6S0RequestUpdatesEventChecker)), new ExpectedEvent(TCtestScenario6.RequestUpdatesEventInfo, null, new RequestUpdatesEventDelegate1(this.TCtestScenario6S0RequestUpdatesEventChecker1)), new ExpectedEvent(TCtestScenario6.RequestUpdatesEventInfo, null, new RequestUpdatesEventDelegate1(this.TCtestScenario6S0RequestUpdatesEventChecker2)), new ExpectedEvent(TCtestScenario6.RequestUpdatesEventInfo, null, new RequestUpdatesEventDelegate1(this.TCtestScenario6S0RequestUpdatesEventChecker3)));
                if ((temp12 == 0)) {
                    this.Manager.Comment("reaching state \'S67\'");
                    FRS2Model.error_status_t temp8;
                    this.Manager.Comment("executing step \'call UpdateCancel(1,valid,5)\'");
                    temp8 = this.IFRS2ManagedAdapterInstance.UpdateCancel(1, FRS2Model.FRS_UPDATE_CANCEL_DATA.valid, 5);
                    this.Manager.Checkpoint("MS-FRS2_R607");
                    this.Manager.AddReturn(UpdateCancelInfo, null, temp8);
                    TCtestScenario6S83();
                    goto label0;
                }
                if ((temp12 == 1)) {
                    this.Manager.Comment("reaching state \'S68\'");
                    FRS2Model.error_status_t temp9;
                    this.Manager.Comment("executing step \'call UpdateCancel(1,valid,5)\'");
                    temp9 = this.IFRS2ManagedAdapterInstance.UpdateCancel(1, FRS2Model.FRS_UPDATE_CANCEL_DATA.valid, 5);
                    this.Manager.Checkpoint("MS-FRS2_R607");
                    this.Manager.Comment("reaching state \'S84\'");
                    this.Manager.Comment("checking step \'return UpdateCancel/FRS_ERROR_CONTENTSET_NOT_FOUND\'");
                    this.Manager.Assert((temp9 == FRS2Model.error_status_t.FRS_ERROR_CONTENTSET_NOT_FOUND), String.Format("expected \'FRS2Model.error_status_t.FRS_ERROR_CONTENTSET_NOT_FOUND\', actual \'{0}\' " +
                                "(return of UpdateCancel, state S84)", TestManagerHelpers.Describe(temp9)));
                    TCtestScenario6S100();
                    goto label0;
                }
                if ((temp12 == 2)) {
                    this.Manager.Comment("reaching state \'S69\'");
                    FRS2Model.error_status_t temp10;
                    this.Manager.Comment("executing step \'call UpdateCancel(1,valid,5)\'");
                    temp10 = this.IFRS2ManagedAdapterInstance.UpdateCancel(1, FRS2Model.FRS_UPDATE_CANCEL_DATA.valid, 5);
                    this.Manager.Checkpoint("MS-FRS2_R607");
                    this.Manager.AddReturn(UpdateCancelInfo, null, temp10);
                    TCtestScenario6S83();
                    goto label0;
                }
                if ((temp12 == 3)) {
                    this.Manager.Comment("reaching state \'S70\'");
                    FRS2Model.error_status_t temp11;
                    this.Manager.Comment("executing step \'call UpdateCancel(1,valid,5)\'");
                    temp11 = this.IFRS2ManagedAdapterInstance.UpdateCancel(1, FRS2Model.FRS_UPDATE_CANCEL_DATA.valid, 5);
                    this.Manager.Checkpoint("MS-FRS2_R607");
                    this.Manager.Comment("reaching state \'S86\'");
                    this.Manager.Comment("checking step \'return UpdateCancel/FRS_ERROR_CONTENTSET_NOT_FOUND\'");
                    this.Manager.Assert((temp11 == FRS2Model.error_status_t.FRS_ERROR_CONTENTSET_NOT_FOUND), String.Format("expected \'FRS2Model.error_status_t.FRS_ERROR_CONTENTSET_NOT_FOUND\', actual \'{0}\' " +
                                "(return of UpdateCancel, state S86)", TestManagerHelpers.Describe(temp11)));
                    TCtestScenario6S101();
                    goto label0;
                }
                throw new InvalidOperationException("never reached");
            label0:
;
                goto label1;
            }
            if ((temp13 == 1)) {
                TCtestScenario6S52();
                goto label1;
            }
            throw new InvalidOperationException("never reached");
        label1:
;
            this.Manager.EndTest();
        }
        
        private void TCtestScenario6S0AsyncPollResponseEventChecker(FRS2Model.VVGeneration vvGen) {
            this.Manager.Comment("checking step \'event AsyncPollResponseEvent(ValidValue)\'");
            try {
                this.Manager.Assert((vvGen == FRS2Model.VVGeneration.ValidValue), String.Format("expected \'FRS2Model.VVGeneration.ValidValue\', actual \'{0}\' (vvGen of AsyncPollRes" +
                            "ponseEvent, state S47)", TestManagerHelpers.Describe(vvGen)));
            }
            catch (TransactionFailedException ) {
                this.Manager.Comment("This step would have covered MS-FRS2_R556, MS-FRS2_R1020, MS-FRS2_R555");
                throw;
            }
            this.Manager.Checkpoint("MS-FRS2_R556");
            this.Manager.Checkpoint("MS-FRS2_R1020");
            this.Manager.Checkpoint("MS-FRS2_R555");
        }
        
        private void TCtestScenario6S0RequestUpdatesEventChecker(FRS2Model.FilePresense present, FRS2Model.UPDATE_STATUS updateStatus) {
            this.Manager.Comment("checking step \'event RequestUpdatesEvent(fileDeleted,UPDATE_STATUS_MORE)\'");
            try {
                this.Manager.Assert((present == FRS2Model.FilePresense.fileDeleted), String.Format("expected \'FRS2Model.FilePresense.fileDeleted\', actual \'{0}\' (present of RequestUp" +
                            "datesEvent, state S63)", TestManagerHelpers.Describe(present)));
                this.Manager.Assert((updateStatus == FRS2Model.UPDATE_STATUS.UPDATE_STATUS_MORE), String.Format("expected \'FRS2Model.UPDATE_STATUS.UPDATE_STATUS_MORE\', actual \'{0}\' (updateStatus" +
                            " of RequestUpdatesEvent, state S63)", TestManagerHelpers.Describe(updateStatus)));
            }
            catch (TransactionFailedException ) {
                this.Manager.Comment("This step would have covered MS-FRS2_R93");
                throw;
            }
            this.Manager.Checkpoint("MS-FRS2_R93");
        }
        
        private void TCtestScenario6S83() {
            this.Manager.Comment("reaching state \'S83\'");
            this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(TCtestScenario6.UpdateCancelInfo, null, new UpdateCancelDelegate1(this.TCtestScenario6S0UpdateCancelChecker)));
            TCtestScenario6S99();
        }
        
        private void TCtestScenario6S0UpdateCancelChecker(FRS2Model.error_status_t @return) {
            this.Manager.Comment("checking step \'return UpdateCancel/FRS_ERROR_CONTENTSET_NOT_FOUND\'");
            this.Manager.Assert((@return == FRS2Model.error_status_t.FRS_ERROR_CONTENTSET_NOT_FOUND), String.Format("expected \'FRS2Model.error_status_t.FRS_ERROR_CONTENTSET_NOT_FOUND\', actual \'{0}\' " +
                        "(return of UpdateCancel, state S83)", TestManagerHelpers.Describe(@return)));
        }
        
        private void TCtestScenario6S99() {
            this.Manager.Comment("reaching state \'S99\'");
        }
        
        private void TCtestScenario6S0RequestUpdatesEventChecker1(FRS2Model.FilePresense present, FRS2Model.UPDATE_STATUS updateStatus) {
            this.Manager.Comment("checking step \'event RequestUpdatesEvent(fileExists,UPDATE_STATUS_MORE)\'");
            try {
                this.Manager.Assert((present == FRS2Model.FilePresense.fileExists), String.Format("expected \'FRS2Model.FilePresense.fileExists\', actual \'{0}\' (present of RequestUpd" +
                            "atesEvent, state S63)", TestManagerHelpers.Describe(present)));
                this.Manager.Assert((updateStatus == FRS2Model.UPDATE_STATUS.UPDATE_STATUS_MORE), String.Format("expected \'FRS2Model.UPDATE_STATUS.UPDATE_STATUS_MORE\', actual \'{0}\' (updateStatus" +
                            " of RequestUpdatesEvent, state S63)", TestManagerHelpers.Describe(updateStatus)));
            }
            catch (TransactionFailedException ) {
                this.Manager.Comment("This step would have covered MS-FRS2_R93, MS-FRS2_R498");
                throw;
            }
            this.Manager.Checkpoint("MS-FRS2_R93");
            this.Manager.Checkpoint("MS-FRS2_R498");
        }
        
        private void TCtestScenario6S100() {
            this.Manager.Comment("reaching state \'S100\'");
        }
        
        private void TCtestScenario6S0RequestUpdatesEventChecker2(FRS2Model.FilePresense present, FRS2Model.UPDATE_STATUS updateStatus) {
            this.Manager.Comment("checking step \'event RequestUpdatesEvent(fileDeleted,UPDATE_STATUS_DONE)\'");
            try {
                this.Manager.Assert((present == FRS2Model.FilePresense.fileDeleted), String.Format("expected \'FRS2Model.FilePresense.fileDeleted\', actual \'{0}\' (present of RequestUp" +
                            "datesEvent, state S63)", TestManagerHelpers.Describe(present)));
                this.Manager.Assert((updateStatus == FRS2Model.UPDATE_STATUS.UPDATE_STATUS_DONE), String.Format("expected \'FRS2Model.UPDATE_STATUS.UPDATE_STATUS_DONE\', actual \'{0}\' (updateStatus" +
                            " of RequestUpdatesEvent, state S63)", TestManagerHelpers.Describe(updateStatus)));
            }
            catch (TransactionFailedException ) {
                this.Manager.Comment("This step would have covered MS-FRS2_R93, MS-FRS2_R94");
                throw;
            }
            this.Manager.Checkpoint("MS-FRS2_R93");
            this.Manager.Checkpoint("MS-FRS2_R94");
        }
        
        private void TCtestScenario6S0RequestUpdatesEventChecker3(FRS2Model.FilePresense present, FRS2Model.UPDATE_STATUS updateStatus) {
            this.Manager.Comment("checking step \'event RequestUpdatesEvent(fileExists,UPDATE_STATUS_DONE)\'");
            try {
                this.Manager.Assert((present == FRS2Model.FilePresense.fileExists), String.Format("expected \'FRS2Model.FilePresense.fileExists\', actual \'{0}\' (present of RequestUpd" +
                            "atesEvent, state S63)", TestManagerHelpers.Describe(present)));
                this.Manager.Assert((updateStatus == FRS2Model.UPDATE_STATUS.UPDATE_STATUS_DONE), String.Format("expected \'FRS2Model.UPDATE_STATUS.UPDATE_STATUS_DONE\', actual \'{0}\' (updateStatus" +
                            " of RequestUpdatesEvent, state S63)", TestManagerHelpers.Describe(updateStatus)));
            }
            catch (TransactionFailedException ) {
                this.Manager.Comment("This step would have covered MS-FRS2_R93, MS-FRS2_R94");
                throw;
            }
            this.Manager.Checkpoint("MS-FRS2_R93");
            this.Manager.Checkpoint("MS-FRS2_R94");
        }
        
        private void TCtestScenario6S101() {
            this.Manager.Comment("reaching state \'S101\'");
        }
        
        private void TCtestScenario6S0AsyncPollResponseEventChecker1(FRS2Model.VVGeneration vvGen) {
            this.Manager.Comment("checking step \'event AsyncPollResponseEvent(InvalidValue)\'");
            try {
                this.Manager.Assert((vvGen == FRS2Model.VVGeneration.InvalidValue), String.Format("expected \'FRS2Model.VVGeneration.InvalidValue\', actual \'{0}\' (vvGen of AsyncPollR" +
                            "esponseEvent, state S47)", TestManagerHelpers.Describe(vvGen)));
            }
            catch (TransactionFailedException ) {
                this.Manager.Comment("This step would have covered MS-FRS2_R556, MS-FRS2_R1020, MS-FRS2_R555");
                throw;
            }
            this.Manager.Checkpoint("MS-FRS2_R556");
            this.Manager.Checkpoint("MS-FRS2_R1020");
            this.Manager.Checkpoint("MS-FRS2_R555");
        }
        
        private void TCtestScenario6S52() {
            this.Manager.Comment("reaching state \'S52\'");
        }
        #endregion
        
        #region Test Starting in S2
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        [Microsoft.VisualStudio.TestTools.UnitTesting.DescriptionAttribute(@"MS-FRS2_R37, MS-FRS2_R436, MS-FRS2_R439, MS-FRS2_R455, MS-FRS2_R458, MS-FRS2_R448, MS-FRS2_R549, MS-FRS2_R552, MS-FRS2_R517, MS-FRS2_R520, MS-FRS2_R466, MS-FRS2_R556, MS-FRS2_R1020, MS-FRS2_R555, MS-FRS2_R486, MS-FRS2_R489, MS-FRS2_R93, MS-FRS2_R94, MS-FRS2_R604, MS-FRS2_R611, MS-FRS2_R93, MS-FRS2_R94, MS-FRS2_R603, MS-FRS2_R606, MS-FRS2_R614, MS-FRS2_R93, MS-FRS2_R498, MS-FRS2_R604, MS-FRS2_R611, MS-FRS2_R93, MS-FRS2_R604, MS-FRS2_R611, MS-FRS2_R556, MS-FRS2_R1020, MS-FRS2_R555")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestCategory("AD")]
        [TestCategory("MS-FRS2")]
        [TestCategory("SDC")]
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        
        public virtual void FRS2_TCtestScenario6S2()
        {
            this.Manager.BeginTest("TCtestScenario6S2");
            this.Manager.Comment("reaching state \'S2\'");
            FRS2Model.error_status_t temp14;
            this.Manager.Comment(@"executing step 'call Initialization(Windows7,Map{1=>enabled,2=>enabled,3=>enabled,4=>disabled},Map{1=>exists,2=>exists,3=>exists,4=>exists},Map{""A""=>1,""B""=>2,""C""=>3,""D""=>4},Map{""A""=>sysvol,""B""=>protection,""C""=>sysvol,""D""=>sysvol},Map{1=>""A"",2=>""B"",3=>""C"",4=>""D""},Map{1=>writeOnly,2=>readWrite,3=>readOnly,4=>readWrite},Map{1=>enabled,2=>disabled,3=>enabled,4=>enabled})'");
            temp14 = this.IFRS2ManagedAdapterInstance.Initialization(FRS2Model.OSVersion.Windows7, new Microsoft.Modeling.Map<System.Int32,FRS2Model.connectionProperty>().Add(1, FRS2Model.connectionProperty.enabled).Add(2, FRS2Model.connectionProperty.enabled).Add(3, FRS2Model.connectionProperty.enabled).Add(4, FRS2Model.connectionProperty.disabled), new Microsoft.Modeling.Map<System.Int32,FRS2Model.FromServer>().Add(1, FRS2Model.FromServer.exists).Add(2, FRS2Model.FromServer.exists).Add(3, FRS2Model.FromServer.exists).Add(4, FRS2Model.FromServer.exists), new Microsoft.Modeling.Map<System.String,System.Int32>().Add("A", 1).Add("B", 2).Add("C", 3).Add("D", 4), new Microsoft.Modeling.Map<System.String,FRS2Model.ReplicationGroupTypes>().Add("A", FRS2Model.ReplicationGroupTypes.sysvol).Add("B", FRS2Model.ReplicationGroupTypes.protection).Add("C", FRS2Model.ReplicationGroupTypes.sysvol).Add("D", FRS2Model.ReplicationGroupTypes.sysvol), new Microsoft.Modeling.Map<System.Int32,System.String>().Add(1, "A").Add(2, "B").Add(3, "C").Add(4, "D"), new Microsoft.Modeling.Map<System.Int32,FRS2Model.accessLevels>().Add(1, FRS2Model.accessLevels.writeOnly).Add(2, FRS2Model.accessLevels.readWrite).Add(3, FRS2Model.accessLevels.readOnly).Add(4, FRS2Model.accessLevels.readWrite), new Microsoft.Modeling.Map<System.Int32,FRS2Model.connectionProperty>().Add(1, FRS2Model.connectionProperty.enabled).Add(2, FRS2Model.connectionProperty.disabled).Add(3, FRS2Model.connectionProperty.enabled).Add(4, FRS2Model.connectionProperty.enabled));
            this.Manager.Comment("reaching state \'S3\'");
            this.Manager.Comment("checking step \'return Initialization/ERROR_SUCCESS\'");
            this.Manager.Assert((temp14 == FRS2Model.error_status_t.ERROR_SUCCESS), String.Format("expected \'FRS2Model.error_status_t.ERROR_SUCCESS\', actual \'{0}\' (return of Initia" +
                        "lization, state S3)", TestManagerHelpers.Describe(temp14)));
            this.Manager.Comment("reaching state \'S11\'");
            FRS2Model.ProtocolVersionReturned temp15;
            FRS2Model.UpstreamFlagValueReturned temp16;
            FRS2Model.error_status_t temp17;
            this.Manager.Comment("executing step \'call EstablishConnection(\"A\",1,FRS_COMMUNICATION_PROTOCOL_VERSION" +
                    "_LONGHORN_SERVER,out _,out _)\'");
            temp17 = this.IFRS2ManagedAdapterInstance.EstablishConnection("A", 1, FRS2Model.ProtocolVersion.FRS_COMMUNICATION_PROTOCOL_VERSION_LONGHORN_SERVER, out temp15, out temp16);
            this.Manager.Checkpoint("MS-FRS2_R37");
            this.Manager.Checkpoint("MS-FRS2_R436");
            this.Manager.Checkpoint("MS-FRS2_R439");
            this.Manager.Comment("reaching state \'S16\'");
            this.Manager.Comment("checking step \'return EstablishConnection/[out Valid,out Valid]:ERROR_SUCCESS\'");
            this.Manager.Assert((temp15 == FRS2Model.ProtocolVersionReturned.Valid), String.Format("expected \'FRS2Model.ProtocolVersionReturned.Valid\', actual \'{0}\' (upstreamProtoco" +
                        "lVersion of EstablishConnection, state S16)", TestManagerHelpers.Describe(temp15)));
            this.Manager.Assert((temp16 == FRS2Model.UpstreamFlagValueReturned.Valid), String.Format("expected \'FRS2Model.UpstreamFlagValueReturned.Valid\', actual \'{0}\' (upstreamFlags" +
                        " of EstablishConnection, state S16)", TestManagerHelpers.Describe(temp16)));
            this.Manager.Assert((temp17 == FRS2Model.error_status_t.ERROR_SUCCESS), String.Format("expected \'FRS2Model.error_status_t.ERROR_SUCCESS\', actual \'{0}\' (return of Establ" +
                        "ishConnection, state S16)", TestManagerHelpers.Describe(temp17)));
            this.Manager.Comment("reaching state \'S21\'");
            FRS2Model.error_status_t temp18;
            this.Manager.Comment("executing step \'call EstablishSession(1,1)\'");
            temp18 = this.IFRS2ManagedAdapterInstance.EstablishSession(1, 1);
            this.Manager.Checkpoint("MS-FRS2_R455");
            this.Manager.Checkpoint("MS-FRS2_R458");
            this.Manager.Checkpoint("MS-FRS2_R448");
            this.Manager.Comment("reaching state \'S26\'");
            this.Manager.Comment("checking step \'return EstablishSession/ERROR_SUCCESS\'");
            this.Manager.Assert((temp18 == FRS2Model.error_status_t.ERROR_SUCCESS), String.Format("expected \'FRS2Model.error_status_t.ERROR_SUCCESS\', actual \'{0}\' (return of Establ" +
                        "ishSession, state S26)", TestManagerHelpers.Describe(temp18)));
            this.Manager.Comment("reaching state \'S31\'");
            FRS2Model.error_status_t temp19;
            this.Manager.Comment("executing step \'call AsyncPoll(1)\'");
            temp19 = this.IFRS2ManagedAdapterInstance.AsyncPoll(1);
            this.Manager.Checkpoint("MS-FRS2_R549");
            this.Manager.Checkpoint("MS-FRS2_R552");
            this.Manager.Comment("reaching state \'S36\'");
            this.Manager.Comment("checking step \'return AsyncPoll/ERROR_SUCCESS\'");
            this.Manager.Assert((temp19 == FRS2Model.error_status_t.ERROR_SUCCESS), String.Format("expected \'FRS2Model.error_status_t.ERROR_SUCCESS\', actual \'{0}\' (return of AsyncP" +
                        "oll, state S36)", TestManagerHelpers.Describe(temp19)));
            this.Manager.Comment("reaching state \'S40\'");
            FRS2Model.error_status_t temp20;
            this.Manager.Comment("executing step \'call RequestVersionVector(1,1,1,REQUEST_NORMAL_SYNC,CHANGE_ALL,Va" +
                    "lidValue)\'");
            temp20 = this.IFRS2ManagedAdapterInstance.RequestVersionVector(1, 1, 1, FRS2Model.VERSION_REQUEST_TYPE.REQUEST_NORMAL_SYNC, FRS2Model.VERSION_CHANGE_TYPE.CHANGE_ALL, FRS2Model.VVGeneration.ValidValue);
            this.Manager.Checkpoint("MS-FRS2_R517");
            this.Manager.Checkpoint("MS-FRS2_R520");
            this.Manager.Checkpoint("MS-FRS2_R466");
            this.Manager.Comment("reaching state \'S44\'");
            this.Manager.Comment("checking step \'return RequestVersionVector/ERROR_SUCCESS\'");
            this.Manager.Assert((temp20 == FRS2Model.error_status_t.ERROR_SUCCESS), String.Format("expected \'FRS2Model.error_status_t.ERROR_SUCCESS\', actual \'{0}\' (return of Reques" +
                        "tVersionVector, state S44)", TestManagerHelpers.Describe(temp20)));
            this.Manager.Comment("reaching state \'S48\'");
            int temp27 = this.Manager.ExpectEvent(this.QuiescenceTimeout, true, new ExpectedEvent(TCtestScenario6.AsyncPollResponseEventInfo, null, new AsyncPollResponseEventDelegate1(this.TCtestScenario6S2AsyncPollResponseEventChecker)), new ExpectedEvent(TCtestScenario6.AsyncPollResponseEventInfo, null, new AsyncPollResponseEventDelegate1(this.TCtestScenario6S2AsyncPollResponseEventChecker1)));
            if ((temp27 == 0)) {
                this.Manager.Comment("reaching state \'S53\'");
                FRS2Model.error_status_t temp21;
                this.Manager.Comment("executing step \'call RequestUpdates(1,1,valid)\'");
                temp21 = this.IFRS2ManagedAdapterInstance.RequestUpdates(1, 1, FRS2Model.versionVectorDiff.valid);
                this.Manager.Checkpoint("MS-FRS2_R486");
                this.Manager.Checkpoint("MS-FRS2_R489");
                this.Manager.Comment("reaching state \'S60\'");
                this.Manager.Comment("checking step \'return RequestUpdates/ERROR_SUCCESS\'");
                this.Manager.Assert((temp21 == FRS2Model.error_status_t.ERROR_SUCCESS), String.Format("expected \'FRS2Model.error_status_t.ERROR_SUCCESS\', actual \'{0}\' (return of Reques" +
                            "tUpdates, state S60)", TestManagerHelpers.Describe(temp21)));
                this.Manager.Comment("reaching state \'S64\'");
                int temp26 = this.Manager.ExpectEvent(this.QuiescenceTimeout, true, new ExpectedEvent(TCtestScenario6.RequestUpdatesEventInfo, null, new RequestUpdatesEventDelegate1(this.TCtestScenario6S2RequestUpdatesEventChecker)), new ExpectedEvent(TCtestScenario6.RequestUpdatesEventInfo, null, new RequestUpdatesEventDelegate1(this.TCtestScenario6S2RequestUpdatesEventChecker1)), new ExpectedEvent(TCtestScenario6.RequestUpdatesEventInfo, null, new RequestUpdatesEventDelegate1(this.TCtestScenario6S2RequestUpdatesEventChecker2)), new ExpectedEvent(TCtestScenario6.RequestUpdatesEventInfo, null, new RequestUpdatesEventDelegate1(this.TCtestScenario6S2RequestUpdatesEventChecker3)));
                if ((temp26 == 0)) {
                    this.Manager.Comment("reaching state \'S71\'");
                    FRS2Model.error_status_t temp22;
                    this.Manager.Comment("executing step \'call UpdateCancel(1,inValid,1)\'");
                    temp22 = this.IFRS2ManagedAdapterInstance.UpdateCancel(1, FRS2Model.FRS_UPDATE_CANCEL_DATA.inValid, 1);
                    this.Manager.Checkpoint("MS-FRS2_R604");
                    this.Manager.Checkpoint("MS-FRS2_R611");
                    this.Manager.Comment("reaching state \'S87\'");
                    this.Manager.Comment("checking step \'return UpdateCancel/ERROR_FAIL\'");
                    this.Manager.Assert((temp22 == FRS2Model.error_status_t.ERROR_FAIL), String.Format("expected \'FRS2Model.error_status_t.ERROR_FAIL\', actual \'{0}\' (return of UpdateCan" +
                                "cel, state S87)", TestManagerHelpers.Describe(temp22)));
                    TCtestScenario6S101();
                    goto label2;
                }
                if ((temp26 == 1)) {
                    this.Manager.Comment("reaching state \'S72\'");
                    FRS2Model.error_status_t temp23;
                    this.Manager.Comment("executing step \'call UpdateCancel(1,valid,1)\'");
                    temp23 = this.IFRS2ManagedAdapterInstance.UpdateCancel(1, FRS2Model.FRS_UPDATE_CANCEL_DATA.valid, 1);
                    this.Manager.Checkpoint("MS-FRS2_R603");
                    this.Manager.Checkpoint("MS-FRS2_R606");
                    this.Manager.Checkpoint("MS-FRS2_R614");
                    this.Manager.AddReturn(UpdateCancelInfo, null, temp23);
                    TCtestScenario6S88();
                    goto label2;
                }
                if ((temp26 == 2)) {
                    this.Manager.Comment("reaching state \'S73\'");
                    FRS2Model.error_status_t temp24;
                    this.Manager.Comment("executing step \'call UpdateCancel(1,inValid,1)\'");
                    temp24 = this.IFRS2ManagedAdapterInstance.UpdateCancel(1, FRS2Model.FRS_UPDATE_CANCEL_DATA.inValid, 1);
                    this.Manager.Checkpoint("MS-FRS2_R604");
                    this.Manager.Checkpoint("MS-FRS2_R611");
                    this.Manager.Comment("reaching state \'S89\'");
                    this.Manager.Comment("checking step \'return UpdateCancel/ERROR_FAIL\'");
                    this.Manager.Assert((temp24 == FRS2Model.error_status_t.ERROR_FAIL), String.Format("expected \'FRS2Model.error_status_t.ERROR_FAIL\', actual \'{0}\' (return of UpdateCan" +
                                "cel, state S89)", TestManagerHelpers.Describe(temp24)));
                    TCtestScenario6S100();
                    goto label2;
                }
                if ((temp26 == 3)) {
                    this.Manager.Comment("reaching state \'S74\'");
                    FRS2Model.error_status_t temp25;
                    this.Manager.Comment("executing step \'call UpdateCancel(1,inValid,1)\'");
                    temp25 = this.IFRS2ManagedAdapterInstance.UpdateCancel(1, FRS2Model.FRS_UPDATE_CANCEL_DATA.inValid, 1);
                    this.Manager.Checkpoint("MS-FRS2_R604");
                    this.Manager.Checkpoint("MS-FRS2_R611");
                    this.Manager.AddReturn(UpdateCancelInfo, null, temp25);
                    TCtestScenario6S90();
                    goto label2;
                }
                throw new InvalidOperationException("never reached");
            label2:
;
                goto label3;
            }
            if ((temp27 == 1)) {
                TCtestScenario6S52();
                goto label3;
            }
            throw new InvalidOperationException("never reached");
        label3:
;
            this.Manager.EndTest();
        }
        
        private void TCtestScenario6S2AsyncPollResponseEventChecker(FRS2Model.VVGeneration vvGen) {
            this.Manager.Comment("checking step \'event AsyncPollResponseEvent(ValidValue)\'");
            try {
                this.Manager.Assert((vvGen == FRS2Model.VVGeneration.ValidValue), String.Format("expected \'FRS2Model.VVGeneration.ValidValue\', actual \'{0}\' (vvGen of AsyncPollRes" +
                            "ponseEvent, state S48)", TestManagerHelpers.Describe(vvGen)));
            }
            catch (TransactionFailedException ) {
                this.Manager.Comment("This step would have covered MS-FRS2_R556, MS-FRS2_R1020, MS-FRS2_R555");
                throw;
            }
            this.Manager.Checkpoint("MS-FRS2_R556");
            this.Manager.Checkpoint("MS-FRS2_R1020");
            this.Manager.Checkpoint("MS-FRS2_R555");
        }
        
        private void TCtestScenario6S2RequestUpdatesEventChecker(FRS2Model.FilePresense present, FRS2Model.UPDATE_STATUS updateStatus) {
            this.Manager.Comment("checking step \'event RequestUpdatesEvent(fileExists,UPDATE_STATUS_DONE)\'");
            try {
                this.Manager.Assert((present == FRS2Model.FilePresense.fileExists), String.Format("expected \'FRS2Model.FilePresense.fileExists\', actual \'{0}\' (present of RequestUpd" +
                            "atesEvent, state S64)", TestManagerHelpers.Describe(present)));
                this.Manager.Assert((updateStatus == FRS2Model.UPDATE_STATUS.UPDATE_STATUS_DONE), String.Format("expected \'FRS2Model.UPDATE_STATUS.UPDATE_STATUS_DONE\', actual \'{0}\' (updateStatus" +
                            " of RequestUpdatesEvent, state S64)", TestManagerHelpers.Describe(updateStatus)));
            }
            catch (TransactionFailedException ) {
                this.Manager.Comment("This step would have covered MS-FRS2_R93, MS-FRS2_R94");
                throw;
            }
            this.Manager.Checkpoint("MS-FRS2_R93");
            this.Manager.Checkpoint("MS-FRS2_R94");
        }
        
        private void TCtestScenario6S2RequestUpdatesEventChecker1(FRS2Model.FilePresense present, FRS2Model.UPDATE_STATUS updateStatus) {
            this.Manager.Comment("checking step \'event RequestUpdatesEvent(fileDeleted,UPDATE_STATUS_DONE)\'");
            try {
                this.Manager.Assert((present == FRS2Model.FilePresense.fileDeleted), String.Format("expected \'FRS2Model.FilePresense.fileDeleted\', actual \'{0}\' (present of RequestUp" +
                            "datesEvent, state S64)", TestManagerHelpers.Describe(present)));
                this.Manager.Assert((updateStatus == FRS2Model.UPDATE_STATUS.UPDATE_STATUS_DONE), String.Format("expected \'FRS2Model.UPDATE_STATUS.UPDATE_STATUS_DONE\', actual \'{0}\' (updateStatus" +
                            " of RequestUpdatesEvent, state S64)", TestManagerHelpers.Describe(updateStatus)));
            }
            catch (TransactionFailedException ) {
                this.Manager.Comment("This step would have covered MS-FRS2_R93, MS-FRS2_R94");
                throw;
            }
            this.Manager.Checkpoint("MS-FRS2_R93");
            this.Manager.Checkpoint("MS-FRS2_R94");
        }
        
        private void TCtestScenario6S88() {
            this.Manager.Comment("reaching state \'S88\'");
            this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(TCtestScenario6.UpdateCancelInfo, null, new UpdateCancelDelegate1(this.TCtestScenario6S2UpdateCancelChecker)));
            this.Manager.Comment("reaching state \'S103\'");
        }
        
        private void TCtestScenario6S2UpdateCancelChecker(FRS2Model.error_status_t @return) {
            this.Manager.Comment("checking step \'return UpdateCancel/ERROR_SUCCESS\'");
            this.Manager.Assert((@return == FRS2Model.error_status_t.ERROR_SUCCESS), String.Format("expected \'FRS2Model.error_status_t.ERROR_SUCCESS\', actual \'{0}\' (return of Update" +
                        "Cancel, state S88)", TestManagerHelpers.Describe(@return)));
        }
        
        private void TCtestScenario6S2RequestUpdatesEventChecker2(FRS2Model.FilePresense present, FRS2Model.UPDATE_STATUS updateStatus) {
            this.Manager.Comment("checking step \'event RequestUpdatesEvent(fileExists,UPDATE_STATUS_MORE)\'");
            try {
                this.Manager.Assert((present == FRS2Model.FilePresense.fileExists), String.Format("expected \'FRS2Model.FilePresense.fileExists\', actual \'{0}\' (present of RequestUpd" +
                            "atesEvent, state S64)", TestManagerHelpers.Describe(present)));
                this.Manager.Assert((updateStatus == FRS2Model.UPDATE_STATUS.UPDATE_STATUS_MORE), String.Format("expected \'FRS2Model.UPDATE_STATUS.UPDATE_STATUS_MORE\', actual \'{0}\' (updateStatus" +
                            " of RequestUpdatesEvent, state S64)", TestManagerHelpers.Describe(updateStatus)));
            }
            catch (TransactionFailedException ) {
                this.Manager.Comment("This step would have covered MS-FRS2_R93, MS-FRS2_R498");
                throw;
            }
            this.Manager.Checkpoint("MS-FRS2_R93");
            this.Manager.Checkpoint("MS-FRS2_R498");
        }
        
        private void TCtestScenario6S2RequestUpdatesEventChecker3(FRS2Model.FilePresense present, FRS2Model.UPDATE_STATUS updateStatus) {
            this.Manager.Comment("checking step \'event RequestUpdatesEvent(fileDeleted,UPDATE_STATUS_MORE)\'");
            try {
                this.Manager.Assert((present == FRS2Model.FilePresense.fileDeleted), String.Format("expected \'FRS2Model.FilePresense.fileDeleted\', actual \'{0}\' (present of RequestUp" +
                            "datesEvent, state S64)", TestManagerHelpers.Describe(present)));
                this.Manager.Assert((updateStatus == FRS2Model.UPDATE_STATUS.UPDATE_STATUS_MORE), String.Format("expected \'FRS2Model.UPDATE_STATUS.UPDATE_STATUS_MORE\', actual \'{0}\' (updateStatus" +
                            " of RequestUpdatesEvent, state S64)", TestManagerHelpers.Describe(updateStatus)));
            }
            catch (TransactionFailedException ) {
                this.Manager.Comment("This step would have covered MS-FRS2_R93");
                throw;
            }
            this.Manager.Checkpoint("MS-FRS2_R93");
        }
        
        private void TCtestScenario6S90() {
            this.Manager.Comment("reaching state \'S90\'");
            this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(TCtestScenario6.UpdateCancelInfo, null, new UpdateCancelDelegate1(this.TCtestScenario6S2UpdateCancelChecker1)));
            TCtestScenario6S99();
        }
        
        private void TCtestScenario6S2UpdateCancelChecker1(FRS2Model.error_status_t @return) {
            this.Manager.Comment("checking step \'return UpdateCancel/ERROR_FAIL\'");
            this.Manager.Assert((@return == FRS2Model.error_status_t.ERROR_FAIL), String.Format("expected \'FRS2Model.error_status_t.ERROR_FAIL\', actual \'{0}\' (return of UpdateCan" +
                        "cel, state S90)", TestManagerHelpers.Describe(@return)));
        }
        
        private void TCtestScenario6S2AsyncPollResponseEventChecker1(FRS2Model.VVGeneration vvGen) {
            this.Manager.Comment("checking step \'event AsyncPollResponseEvent(InvalidValue)\'");
            try {
                this.Manager.Assert((vvGen == FRS2Model.VVGeneration.InvalidValue), String.Format("expected \'FRS2Model.VVGeneration.InvalidValue\', actual \'{0}\' (vvGen of AsyncPollR" +
                            "esponseEvent, state S48)", TestManagerHelpers.Describe(vvGen)));
            }
            catch (TransactionFailedException ) {
                this.Manager.Comment("This step would have covered MS-FRS2_R556, MS-FRS2_R1020, MS-FRS2_R555");
                throw;
            }
            this.Manager.Checkpoint("MS-FRS2_R556");
            this.Manager.Checkpoint("MS-FRS2_R1020");
            this.Manager.Checkpoint("MS-FRS2_R555");
        }
        #endregion
        
        #region Test Starting in S4
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        [Microsoft.VisualStudio.TestTools.UnitTesting.DescriptionAttribute(@"MS-FRS2_R37, MS-FRS2_R436, MS-FRS2_R439, MS-FRS2_R455, MS-FRS2_R458, MS-FRS2_R448, MS-FRS2_R549, MS-FRS2_R552, MS-FRS2_R517, MS-FRS2_R520, MS-FRS2_R466, MS-FRS2_R556, MS-FRS2_R1020, MS-FRS2_R555, MS-FRS2_R486, MS-FRS2_R489, MS-FRS2_R93, MS-FRS2_R94, MS-FRS2_R603, MS-FRS2_R606, MS-FRS2_R614, MS-FRS2_R93, MS-FRS2_R94, MS-FRS2_R604, MS-FRS2_R611, MS-FRS2_R93, MS-FRS2_R498, MS-FRS2_R603, MS-FRS2_R606, MS-FRS2_R614, MS-FRS2_R93, MS-FRS2_R603, MS-FRS2_R606, MS-FRS2_R614, MS-FRS2_R556, MS-FRS2_R1020, MS-FRS2_R555")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestCategory("AD")]
        [TestCategory("MS-FRS2")]
        [TestCategory("SDC")]
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        
        public virtual void FRS2_TCtestScenario6S4()
        {
            this.Manager.BeginTest("TCtestScenario6S4");
            this.Manager.Comment("reaching state \'S4\'");
            FRS2Model.error_status_t temp28;
            this.Manager.Comment(@"executing step 'call Initialization(Windows7,Map{1=>enabled,2=>enabled,3=>enabled,4=>disabled},Map{1=>exists,2=>exists,3=>exists,4=>exists},Map{""A""=>1,""B""=>2,""C""=>3,""D""=>4},Map{""A""=>sysvol,""B""=>protection,""C""=>sysvol,""D""=>sysvol},Map{1=>""A"",2=>""B"",3=>""C"",4=>""D""},Map{1=>writeOnly,2=>readWrite,3=>readOnly,4=>readWrite},Map{1=>enabled,2=>disabled,3=>enabled,4=>enabled})'");
            temp28 = this.IFRS2ManagedAdapterInstance.Initialization(FRS2Model.OSVersion.Windows7, new Microsoft.Modeling.Map<System.Int32,FRS2Model.connectionProperty>().Add(1, FRS2Model.connectionProperty.enabled).Add(2, FRS2Model.connectionProperty.enabled).Add(3, FRS2Model.connectionProperty.enabled).Add(4, FRS2Model.connectionProperty.disabled), new Microsoft.Modeling.Map<System.Int32,FRS2Model.FromServer>().Add(1, FRS2Model.FromServer.exists).Add(2, FRS2Model.FromServer.exists).Add(3, FRS2Model.FromServer.exists).Add(4, FRS2Model.FromServer.exists), new Microsoft.Modeling.Map<System.String,System.Int32>().Add("A", 1).Add("B", 2).Add("C", 3).Add("D", 4), new Microsoft.Modeling.Map<System.String,FRS2Model.ReplicationGroupTypes>().Add("A", FRS2Model.ReplicationGroupTypes.sysvol).Add("B", FRS2Model.ReplicationGroupTypes.protection).Add("C", FRS2Model.ReplicationGroupTypes.sysvol).Add("D", FRS2Model.ReplicationGroupTypes.sysvol), new Microsoft.Modeling.Map<System.Int32,System.String>().Add(1, "A").Add(2, "B").Add(3, "C").Add(4, "D"), new Microsoft.Modeling.Map<System.Int32,FRS2Model.accessLevels>().Add(1, FRS2Model.accessLevels.writeOnly).Add(2, FRS2Model.accessLevels.readWrite).Add(3, FRS2Model.accessLevels.readOnly).Add(4, FRS2Model.accessLevels.readWrite), new Microsoft.Modeling.Map<System.Int32,FRS2Model.connectionProperty>().Add(1, FRS2Model.connectionProperty.enabled).Add(2, FRS2Model.connectionProperty.disabled).Add(3, FRS2Model.connectionProperty.enabled).Add(4, FRS2Model.connectionProperty.enabled));
            this.Manager.Comment("reaching state \'S5\'");
            this.Manager.Comment("checking step \'return Initialization/ERROR_SUCCESS\'");
            this.Manager.Assert((temp28 == FRS2Model.error_status_t.ERROR_SUCCESS), String.Format("expected \'FRS2Model.error_status_t.ERROR_SUCCESS\', actual \'{0}\' (return of Initia" +
                        "lization, state S5)", TestManagerHelpers.Describe(temp28)));
            this.Manager.Comment("reaching state \'S12\'");
            FRS2Model.ProtocolVersionReturned temp29;
            FRS2Model.UpstreamFlagValueReturned temp30;
            FRS2Model.error_status_t temp31;
            this.Manager.Comment("executing step \'call EstablishConnection(\"A\",1,FRS_COMMUNICATION_PROTOCOL_VERSION" +
                    "_LONGHORN_SERVER,out _,out _)\'");
            temp31 = this.IFRS2ManagedAdapterInstance.EstablishConnection("A", 1, FRS2Model.ProtocolVersion.FRS_COMMUNICATION_PROTOCOL_VERSION_LONGHORN_SERVER, out temp29, out temp30);
            this.Manager.Checkpoint("MS-FRS2_R37");
            this.Manager.Checkpoint("MS-FRS2_R436");
            this.Manager.Checkpoint("MS-FRS2_R439");
            this.Manager.Comment("reaching state \'S17\'");
            this.Manager.Comment("checking step \'return EstablishConnection/[out Valid,out Valid]:ERROR_SUCCESS\'");
            this.Manager.Assert((temp29 == FRS2Model.ProtocolVersionReturned.Valid), String.Format("expected \'FRS2Model.ProtocolVersionReturned.Valid\', actual \'{0}\' (upstreamProtoco" +
                        "lVersion of EstablishConnection, state S17)", TestManagerHelpers.Describe(temp29)));
            this.Manager.Assert((temp30 == FRS2Model.UpstreamFlagValueReturned.Valid), String.Format("expected \'FRS2Model.UpstreamFlagValueReturned.Valid\', actual \'{0}\' (upstreamFlags" +
                        " of EstablishConnection, state S17)", TestManagerHelpers.Describe(temp30)));
            this.Manager.Assert((temp31 == FRS2Model.error_status_t.ERROR_SUCCESS), String.Format("expected \'FRS2Model.error_status_t.ERROR_SUCCESS\', actual \'{0}\' (return of Establ" +
                        "ishConnection, state S17)", TestManagerHelpers.Describe(temp31)));
            this.Manager.Comment("reaching state \'S22\'");
            FRS2Model.error_status_t temp32;
            this.Manager.Comment("executing step \'call EstablishSession(1,1)\'");
            temp32 = this.IFRS2ManagedAdapterInstance.EstablishSession(1, 1);
            this.Manager.Checkpoint("MS-FRS2_R455");
            this.Manager.Checkpoint("MS-FRS2_R458");
            this.Manager.Checkpoint("MS-FRS2_R448");
            this.Manager.Comment("reaching state \'S27\'");
            this.Manager.Comment("checking step \'return EstablishSession/ERROR_SUCCESS\'");
            this.Manager.Assert((temp32 == FRS2Model.error_status_t.ERROR_SUCCESS), String.Format("expected \'FRS2Model.error_status_t.ERROR_SUCCESS\', actual \'{0}\' (return of Establ" +
                        "ishSession, state S27)", TestManagerHelpers.Describe(temp32)));
            this.Manager.Comment("reaching state \'S32\'");
            FRS2Model.error_status_t temp33;
            this.Manager.Comment("executing step \'call AsyncPoll(1)\'");
            temp33 = this.IFRS2ManagedAdapterInstance.AsyncPoll(1);
            this.Manager.Checkpoint("MS-FRS2_R549");
            this.Manager.Checkpoint("MS-FRS2_R552");
            this.Manager.Comment("reaching state \'S37\'");
            this.Manager.Comment("checking step \'return AsyncPoll/ERROR_SUCCESS\'");
            this.Manager.Assert((temp33 == FRS2Model.error_status_t.ERROR_SUCCESS), String.Format("expected \'FRS2Model.error_status_t.ERROR_SUCCESS\', actual \'{0}\' (return of AsyncP" +
                        "oll, state S37)", TestManagerHelpers.Describe(temp33)));
            this.Manager.Comment("reaching state \'S41\'");
            FRS2Model.error_status_t temp34;
            this.Manager.Comment("executing step \'call RequestVersionVector(1,1,1,REQUEST_NORMAL_SYNC,CHANGE_ALL,Va" +
                    "lidValue)\'");
            temp34 = this.IFRS2ManagedAdapterInstance.RequestVersionVector(1, 1, 1, FRS2Model.VERSION_REQUEST_TYPE.REQUEST_NORMAL_SYNC, FRS2Model.VERSION_CHANGE_TYPE.CHANGE_ALL, FRS2Model.VVGeneration.ValidValue);
            this.Manager.Checkpoint("MS-FRS2_R517");
            this.Manager.Checkpoint("MS-FRS2_R520");
            this.Manager.Checkpoint("MS-FRS2_R466");
            this.Manager.Comment("reaching state \'S45\'");
            this.Manager.Comment("checking step \'return RequestVersionVector/ERROR_SUCCESS\'");
            this.Manager.Assert((temp34 == FRS2Model.error_status_t.ERROR_SUCCESS), String.Format("expected \'FRS2Model.error_status_t.ERROR_SUCCESS\', actual \'{0}\' (return of Reques" +
                        "tVersionVector, state S45)", TestManagerHelpers.Describe(temp34)));
            this.Manager.Comment("reaching state \'S49\'");
            int temp41 = this.Manager.ExpectEvent(this.QuiescenceTimeout, true, new ExpectedEvent(TCtestScenario6.AsyncPollResponseEventInfo, null, new AsyncPollResponseEventDelegate1(this.TCtestScenario6S4AsyncPollResponseEventChecker)), new ExpectedEvent(TCtestScenario6.AsyncPollResponseEventInfo, null, new AsyncPollResponseEventDelegate1(this.TCtestScenario6S4AsyncPollResponseEventChecker1)));
            if ((temp41 == 0)) {
                this.Manager.Comment("reaching state \'S55\'");
                FRS2Model.error_status_t temp35;
                this.Manager.Comment("executing step \'call RequestUpdates(1,1,valid)\'");
                temp35 = this.IFRS2ManagedAdapterInstance.RequestUpdates(1, 1, FRS2Model.versionVectorDiff.valid);
                this.Manager.Checkpoint("MS-FRS2_R486");
                this.Manager.Checkpoint("MS-FRS2_R489");
                this.Manager.Comment("reaching state \'S61\'");
                this.Manager.Comment("checking step \'return RequestUpdates/ERROR_SUCCESS\'");
                this.Manager.Assert((temp35 == FRS2Model.error_status_t.ERROR_SUCCESS), String.Format("expected \'FRS2Model.error_status_t.ERROR_SUCCESS\', actual \'{0}\' (return of Reques" +
                            "tUpdates, state S61)", TestManagerHelpers.Describe(temp35)));
                this.Manager.Comment("reaching state \'S65\'");
                int temp40 = this.Manager.ExpectEvent(this.QuiescenceTimeout, true, new ExpectedEvent(TCtestScenario6.RequestUpdatesEventInfo, null, new RequestUpdatesEventDelegate1(this.TCtestScenario6S4RequestUpdatesEventChecker)), new ExpectedEvent(TCtestScenario6.RequestUpdatesEventInfo, null, new RequestUpdatesEventDelegate1(this.TCtestScenario6S4RequestUpdatesEventChecker1)), new ExpectedEvent(TCtestScenario6.RequestUpdatesEventInfo, null, new RequestUpdatesEventDelegate1(this.TCtestScenario6S4RequestUpdatesEventChecker2)), new ExpectedEvent(TCtestScenario6.RequestUpdatesEventInfo, null, new RequestUpdatesEventDelegate1(this.TCtestScenario6S4RequestUpdatesEventChecker3)));
                if ((temp40 == 0)) {
                    this.Manager.Comment("reaching state \'S75\'");
                    FRS2Model.error_status_t temp36;
                    this.Manager.Comment("executing step \'call UpdateCancel(1,valid,1)\'");
                    temp36 = this.IFRS2ManagedAdapterInstance.UpdateCancel(1, FRS2Model.FRS_UPDATE_CANCEL_DATA.valid, 1);
                    this.Manager.Checkpoint("MS-FRS2_R603");
                    this.Manager.Checkpoint("MS-FRS2_R606");
                    this.Manager.Checkpoint("MS-FRS2_R614");
                    this.Manager.Comment("reaching state \'S91\'");
                    this.Manager.Comment("checking step \'return UpdateCancel/ERROR_SUCCESS\'");
                    this.Manager.Assert((temp36 == FRS2Model.error_status_t.ERROR_SUCCESS), String.Format("expected \'FRS2Model.error_status_t.ERROR_SUCCESS\', actual \'{0}\' (return of Update" +
                                "Cancel, state S91)", TestManagerHelpers.Describe(temp36)));
                    this.Manager.Comment("reaching state \'S106\'");
                    goto label4;
                }
                if ((temp40 == 1)) {
                    this.Manager.Comment("reaching state \'S76\'");
                    FRS2Model.error_status_t temp37;
                    this.Manager.Comment("executing step \'call UpdateCancel(1,inValid,1)\'");
                    temp37 = this.IFRS2ManagedAdapterInstance.UpdateCancel(1, FRS2Model.FRS_UPDATE_CANCEL_DATA.inValid, 1);
                    this.Manager.Checkpoint("MS-FRS2_R604");
                    this.Manager.Checkpoint("MS-FRS2_R611");
                    this.Manager.AddReturn(UpdateCancelInfo, null, temp37);
                    TCtestScenario6S90();
                    goto label4;
                }
                if ((temp40 == 2)) {
                    this.Manager.Comment("reaching state \'S77\'");
                    FRS2Model.error_status_t temp38;
                    this.Manager.Comment("executing step \'call UpdateCancel(1,valid,1)\'");
                    temp38 = this.IFRS2ManagedAdapterInstance.UpdateCancel(1, FRS2Model.FRS_UPDATE_CANCEL_DATA.valid, 1);
                    this.Manager.Checkpoint("MS-FRS2_R603");
                    this.Manager.Checkpoint("MS-FRS2_R606");
                    this.Manager.Checkpoint("MS-FRS2_R614");
                    this.Manager.Comment("reaching state \'S93\'");
                    this.Manager.Comment("checking step \'return UpdateCancel/ERROR_SUCCESS\'");
                    this.Manager.Assert((temp38 == FRS2Model.error_status_t.ERROR_SUCCESS), String.Format("expected \'FRS2Model.error_status_t.ERROR_SUCCESS\', actual \'{0}\' (return of Update" +
                                "Cancel, state S93)", TestManagerHelpers.Describe(temp38)));
                    this.Manager.Comment("reaching state \'S107\'");
                    goto label4;
                }
                if ((temp40 == 3)) {
                    this.Manager.Comment("reaching state \'S78\'");
                    FRS2Model.error_status_t temp39;
                    this.Manager.Comment("executing step \'call UpdateCancel(1,valid,1)\'");
                    temp39 = this.IFRS2ManagedAdapterInstance.UpdateCancel(1, FRS2Model.FRS_UPDATE_CANCEL_DATA.valid, 1);
                    this.Manager.Checkpoint("MS-FRS2_R603");
                    this.Manager.Checkpoint("MS-FRS2_R606");
                    this.Manager.Checkpoint("MS-FRS2_R614");
                    this.Manager.AddReturn(UpdateCancelInfo, null, temp39);
                    TCtestScenario6S88();
                    goto label4;
                }
                throw new InvalidOperationException("never reached");
            label4:
;
                goto label5;
            }
            if ((temp41 == 1)) {
                TCtestScenario6S52();
                goto label5;
            }
            throw new InvalidOperationException("never reached");
        label5:
;
            this.Manager.EndTest();
        }
        
        private void TCtestScenario6S4AsyncPollResponseEventChecker(FRS2Model.VVGeneration vvGen) {
            this.Manager.Comment("checking step \'event AsyncPollResponseEvent(ValidValue)\'");
            try {
                this.Manager.Assert((vvGen == FRS2Model.VVGeneration.ValidValue), String.Format("expected \'FRS2Model.VVGeneration.ValidValue\', actual \'{0}\' (vvGen of AsyncPollRes" +
                            "ponseEvent, state S49)", TestManagerHelpers.Describe(vvGen)));
            }
            catch (TransactionFailedException ) {
                this.Manager.Comment("This step would have covered MS-FRS2_R556, MS-FRS2_R1020, MS-FRS2_R555");
                throw;
            }
            this.Manager.Checkpoint("MS-FRS2_R556");
            this.Manager.Checkpoint("MS-FRS2_R1020");
            this.Manager.Checkpoint("MS-FRS2_R555");
        }
        
        private void TCtestScenario6S4RequestUpdatesEventChecker(FRS2Model.FilePresense present, FRS2Model.UPDATE_STATUS updateStatus) {
            this.Manager.Comment("checking step \'event RequestUpdatesEvent(fileExists,UPDATE_STATUS_DONE)\'");
            try {
                this.Manager.Assert((present == FRS2Model.FilePresense.fileExists), String.Format("expected \'FRS2Model.FilePresense.fileExists\', actual \'{0}\' (present of RequestUpd" +
                            "atesEvent, state S65)", TestManagerHelpers.Describe(present)));
                this.Manager.Assert((updateStatus == FRS2Model.UPDATE_STATUS.UPDATE_STATUS_DONE), String.Format("expected \'FRS2Model.UPDATE_STATUS.UPDATE_STATUS_DONE\', actual \'{0}\' (updateStatus" +
                            " of RequestUpdatesEvent, state S65)", TestManagerHelpers.Describe(updateStatus)));
            }
            catch (TransactionFailedException ) {
                this.Manager.Comment("This step would have covered MS-FRS2_R93, MS-FRS2_R94");
                throw;
            }
            this.Manager.Checkpoint("MS-FRS2_R93");
            this.Manager.Checkpoint("MS-FRS2_R94");
        }
        
        private void TCtestScenario6S4RequestUpdatesEventChecker1(FRS2Model.FilePresense present, FRS2Model.UPDATE_STATUS updateStatus) {
            this.Manager.Comment("checking step \'event RequestUpdatesEvent(fileDeleted,UPDATE_STATUS_DONE)\'");
            try {
                this.Manager.Assert((present == FRS2Model.FilePresense.fileDeleted), String.Format("expected \'FRS2Model.FilePresense.fileDeleted\', actual \'{0}\' (present of RequestUp" +
                            "datesEvent, state S65)", TestManagerHelpers.Describe(present)));
                this.Manager.Assert((updateStatus == FRS2Model.UPDATE_STATUS.UPDATE_STATUS_DONE), String.Format("expected \'FRS2Model.UPDATE_STATUS.UPDATE_STATUS_DONE\', actual \'{0}\' (updateStatus" +
                            " of RequestUpdatesEvent, state S65)", TestManagerHelpers.Describe(updateStatus)));
            }
            catch (TransactionFailedException ) {
                this.Manager.Comment("This step would have covered MS-FRS2_R93, MS-FRS2_R94");
                throw;
            }
            this.Manager.Checkpoint("MS-FRS2_R93");
            this.Manager.Checkpoint("MS-FRS2_R94");
        }
        
        private void TCtestScenario6S4RequestUpdatesEventChecker2(FRS2Model.FilePresense present, FRS2Model.UPDATE_STATUS updateStatus) {
            this.Manager.Comment("checking step \'event RequestUpdatesEvent(fileExists,UPDATE_STATUS_MORE)\'");
            try {
                this.Manager.Assert((present == FRS2Model.FilePresense.fileExists), String.Format("expected \'FRS2Model.FilePresense.fileExists\', actual \'{0}\' (present of RequestUpd" +
                            "atesEvent, state S65)", TestManagerHelpers.Describe(present)));
                this.Manager.Assert((updateStatus == FRS2Model.UPDATE_STATUS.UPDATE_STATUS_MORE), String.Format("expected \'FRS2Model.UPDATE_STATUS.UPDATE_STATUS_MORE\', actual \'{0}\' (updateStatus" +
                            " of RequestUpdatesEvent, state S65)", TestManagerHelpers.Describe(updateStatus)));
            }
            catch (TransactionFailedException ) {
                this.Manager.Comment("This step would have covered MS-FRS2_R93, MS-FRS2_R498");
                throw;
            }
            this.Manager.Checkpoint("MS-FRS2_R93");
            this.Manager.Checkpoint("MS-FRS2_R498");
        }
        
        private void TCtestScenario6S4RequestUpdatesEventChecker3(FRS2Model.FilePresense present, FRS2Model.UPDATE_STATUS updateStatus) {
            this.Manager.Comment("checking step \'event RequestUpdatesEvent(fileDeleted,UPDATE_STATUS_MORE)\'");
            try {
                this.Manager.Assert((present == FRS2Model.FilePresense.fileDeleted), String.Format("expected \'FRS2Model.FilePresense.fileDeleted\', actual \'{0}\' (present of RequestUp" +
                            "datesEvent, state S65)", TestManagerHelpers.Describe(present)));
                this.Manager.Assert((updateStatus == FRS2Model.UPDATE_STATUS.UPDATE_STATUS_MORE), String.Format("expected \'FRS2Model.UPDATE_STATUS.UPDATE_STATUS_MORE\', actual \'{0}\' (updateStatus" +
                            " of RequestUpdatesEvent, state S65)", TestManagerHelpers.Describe(updateStatus)));
            }
            catch (TransactionFailedException ) {
                this.Manager.Comment("This step would have covered MS-FRS2_R93");
                throw;
            }
            this.Manager.Checkpoint("MS-FRS2_R93");
        }
        
        private void TCtestScenario6S4AsyncPollResponseEventChecker1(FRS2Model.VVGeneration vvGen) {
            this.Manager.Comment("checking step \'event AsyncPollResponseEvent(InvalidValue)\'");
            try {
                this.Manager.Assert((vvGen == FRS2Model.VVGeneration.InvalidValue), String.Format("expected \'FRS2Model.VVGeneration.InvalidValue\', actual \'{0}\' (vvGen of AsyncPollR" +
                            "esponseEvent, state S49)", TestManagerHelpers.Describe(vvGen)));
            }
            catch (TransactionFailedException ) {
                this.Manager.Comment("This step would have covered MS-FRS2_R556, MS-FRS2_R1020, MS-FRS2_R555");
                throw;
            }
            this.Manager.Checkpoint("MS-FRS2_R556");
            this.Manager.Checkpoint("MS-FRS2_R1020");
            this.Manager.Checkpoint("MS-FRS2_R555");
        }
        #endregion
        
        #region Test Starting in S6
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        [Microsoft.VisualStudio.TestTools.UnitTesting.DescriptionAttribute(@"MS-FRS2_R37, MS-FRS2_R436, MS-FRS2_R439, MS-FRS2_R455, MS-FRS2_R458, MS-FRS2_R448, MS-FRS2_R549, MS-FRS2_R552, MS-FRS2_R517, MS-FRS2_R520, MS-FRS2_R466, MS-FRS2_R556, MS-FRS2_R1020, MS-FRS2_R555, MS-FRS2_R486, MS-FRS2_R489, MS-FRS2_R93, MS-FRS2_R94, MS-FRS2_R604, MS-FRS2_R609, MS-FRS2_R93, MS-FRS2_R94, MS-FRS2_R604, MS-FRS2_R609, MS-FRS2_R93, MS-FRS2_R498, MS-FRS2_R604, MS-FRS2_R609, MS-FRS2_R93, MS-FRS2_R604, MS-FRS2_R609, MS-FRS2_R556, MS-FRS2_R1020, MS-FRS2_R555")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestCategory("AD")]
        [TestCategory("MS-FRS2")]
        [TestCategory("SDC")]
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        
        public virtual void FRS2_TCtestScenario6S6()
        {
            this.Manager.BeginTest("TCtestScenario6S6");
            this.Manager.Comment("reaching state \'S6\'");
            FRS2Model.error_status_t temp42;
            this.Manager.Comment(@"executing step 'call Initialization(Windows7,Map{1=>enabled,2=>enabled,3=>enabled,4=>disabled},Map{1=>exists,2=>exists,3=>exists,4=>exists},Map{""A""=>1,""B""=>2,""C""=>3,""D""=>4},Map{""A""=>sysvol,""B""=>protection,""C""=>sysvol,""D""=>sysvol},Map{1=>""A"",2=>""B"",3=>""C"",4=>""D""},Map{1=>writeOnly,2=>readWrite,3=>readOnly,4=>readWrite},Map{1=>enabled,2=>disabled,3=>enabled,4=>enabled})'");
            temp42 = this.IFRS2ManagedAdapterInstance.Initialization(FRS2Model.OSVersion.Windows7, new Microsoft.Modeling.Map<System.Int32,FRS2Model.connectionProperty>().Add(1, FRS2Model.connectionProperty.enabled).Add(2, FRS2Model.connectionProperty.enabled).Add(3, FRS2Model.connectionProperty.enabled).Add(4, FRS2Model.connectionProperty.disabled), new Microsoft.Modeling.Map<System.Int32,FRS2Model.FromServer>().Add(1, FRS2Model.FromServer.exists).Add(2, FRS2Model.FromServer.exists).Add(3, FRS2Model.FromServer.exists).Add(4, FRS2Model.FromServer.exists), new Microsoft.Modeling.Map<System.String,System.Int32>().Add("A", 1).Add("B", 2).Add("C", 3).Add("D", 4), new Microsoft.Modeling.Map<System.String,FRS2Model.ReplicationGroupTypes>().Add("A", FRS2Model.ReplicationGroupTypes.sysvol).Add("B", FRS2Model.ReplicationGroupTypes.protection).Add("C", FRS2Model.ReplicationGroupTypes.sysvol).Add("D", FRS2Model.ReplicationGroupTypes.sysvol), new Microsoft.Modeling.Map<System.Int32,System.String>().Add(1, "A").Add(2, "B").Add(3, "C").Add(4, "D"), new Microsoft.Modeling.Map<System.Int32,FRS2Model.accessLevels>().Add(1, FRS2Model.accessLevels.writeOnly).Add(2, FRS2Model.accessLevels.readWrite).Add(3, FRS2Model.accessLevels.readOnly).Add(4, FRS2Model.accessLevels.readWrite), new Microsoft.Modeling.Map<System.Int32,FRS2Model.connectionProperty>().Add(1, FRS2Model.connectionProperty.enabled).Add(2, FRS2Model.connectionProperty.disabled).Add(3, FRS2Model.connectionProperty.enabled).Add(4, FRS2Model.connectionProperty.enabled));
            this.Manager.Comment("reaching state \'S7\'");
            this.Manager.Comment("checking step \'return Initialization/ERROR_SUCCESS\'");
            this.Manager.Assert((temp42 == FRS2Model.error_status_t.ERROR_SUCCESS), String.Format("expected \'FRS2Model.error_status_t.ERROR_SUCCESS\', actual \'{0}\' (return of Initia" +
                        "lization, state S7)", TestManagerHelpers.Describe(temp42)));
            this.Manager.Comment("reaching state \'S13\'");
            FRS2Model.ProtocolVersionReturned temp43;
            FRS2Model.UpstreamFlagValueReturned temp44;
            FRS2Model.error_status_t temp45;
            this.Manager.Comment("executing step \'call EstablishConnection(\"A\",1,FRS_COMMUNICATION_PROTOCOL_VERSION" +
                    "_LONGHORN_SERVER,out _,out _)\'");
            temp45 = this.IFRS2ManagedAdapterInstance.EstablishConnection("A", 1, FRS2Model.ProtocolVersion.FRS_COMMUNICATION_PROTOCOL_VERSION_LONGHORN_SERVER, out temp43, out temp44);
            this.Manager.Checkpoint("MS-FRS2_R37");
            this.Manager.Checkpoint("MS-FRS2_R436");
            this.Manager.Checkpoint("MS-FRS2_R439");
            this.Manager.Comment("reaching state \'S18\'");
            this.Manager.Comment("checking step \'return EstablishConnection/[out Valid,out Valid]:ERROR_SUCCESS\'");
            this.Manager.Assert((temp43 == FRS2Model.ProtocolVersionReturned.Valid), String.Format("expected \'FRS2Model.ProtocolVersionReturned.Valid\', actual \'{0}\' (upstreamProtoco" +
                        "lVersion of EstablishConnection, state S18)", TestManagerHelpers.Describe(temp43)));
            this.Manager.Assert((temp44 == FRS2Model.UpstreamFlagValueReturned.Valid), String.Format("expected \'FRS2Model.UpstreamFlagValueReturned.Valid\', actual \'{0}\' (upstreamFlags" +
                        " of EstablishConnection, state S18)", TestManagerHelpers.Describe(temp44)));
            this.Manager.Assert((temp45 == FRS2Model.error_status_t.ERROR_SUCCESS), String.Format("expected \'FRS2Model.error_status_t.ERROR_SUCCESS\', actual \'{0}\' (return of Establ" +
                        "ishConnection, state S18)", TestManagerHelpers.Describe(temp45)));
            this.Manager.Comment("reaching state \'S23\'");
            FRS2Model.error_status_t temp46;
            this.Manager.Comment("executing step \'call EstablishSession(1,1)\'");
            temp46 = this.IFRS2ManagedAdapterInstance.EstablishSession(1, 1);
            this.Manager.Checkpoint("MS-FRS2_R455");
            this.Manager.Checkpoint("MS-FRS2_R458");
            this.Manager.Checkpoint("MS-FRS2_R448");
            this.Manager.Comment("reaching state \'S28\'");
            this.Manager.Comment("checking step \'return EstablishSession/ERROR_SUCCESS\'");
            this.Manager.Assert((temp46 == FRS2Model.error_status_t.ERROR_SUCCESS), String.Format("expected \'FRS2Model.error_status_t.ERROR_SUCCESS\', actual \'{0}\' (return of Establ" +
                        "ishSession, state S28)", TestManagerHelpers.Describe(temp46)));
            this.Manager.Comment("reaching state \'S33\'");
            FRS2Model.error_status_t temp47;
            this.Manager.Comment("executing step \'call AsyncPoll(1)\'");
            temp47 = this.IFRS2ManagedAdapterInstance.AsyncPoll(1);
            this.Manager.Checkpoint("MS-FRS2_R549");
            this.Manager.Checkpoint("MS-FRS2_R552");
            this.Manager.Comment("reaching state \'S38\'");
            this.Manager.Comment("checking step \'return AsyncPoll/ERROR_SUCCESS\'");
            this.Manager.Assert((temp47 == FRS2Model.error_status_t.ERROR_SUCCESS), String.Format("expected \'FRS2Model.error_status_t.ERROR_SUCCESS\', actual \'{0}\' (return of AsyncP" +
                        "oll, state S38)", TestManagerHelpers.Describe(temp47)));
            this.Manager.Comment("reaching state \'S42\'");
            FRS2Model.error_status_t temp48;
            this.Manager.Comment("executing step \'call RequestVersionVector(1,1,1,REQUEST_NORMAL_SYNC,CHANGE_ALL,Va" +
                    "lidValue)\'");
            temp48 = this.IFRS2ManagedAdapterInstance.RequestVersionVector(1, 1, 1, FRS2Model.VERSION_REQUEST_TYPE.REQUEST_NORMAL_SYNC, FRS2Model.VERSION_CHANGE_TYPE.CHANGE_ALL, FRS2Model.VVGeneration.ValidValue);
            this.Manager.Checkpoint("MS-FRS2_R517");
            this.Manager.Checkpoint("MS-FRS2_R520");
            this.Manager.Checkpoint("MS-FRS2_R466");
            this.Manager.Comment("reaching state \'S46\'");
            this.Manager.Comment("checking step \'return RequestVersionVector/ERROR_SUCCESS\'");
            this.Manager.Assert((temp48 == FRS2Model.error_status_t.ERROR_SUCCESS), String.Format("expected \'FRS2Model.error_status_t.ERROR_SUCCESS\', actual \'{0}\' (return of Reques" +
                        "tVersionVector, state S46)", TestManagerHelpers.Describe(temp48)));
            this.Manager.Comment("reaching state \'S50\'");
            int temp55 = this.Manager.ExpectEvent(this.QuiescenceTimeout, true, new ExpectedEvent(TCtestScenario6.AsyncPollResponseEventInfo, null, new AsyncPollResponseEventDelegate1(this.TCtestScenario6S6AsyncPollResponseEventChecker)), new ExpectedEvent(TCtestScenario6.AsyncPollResponseEventInfo, null, new AsyncPollResponseEventDelegate1(this.TCtestScenario6S6AsyncPollResponseEventChecker1)));
            if ((temp55 == 0)) {
                this.Manager.Comment("reaching state \'S57\'");
                FRS2Model.error_status_t temp49;
                this.Manager.Comment("executing step \'call RequestUpdates(1,1,valid)\'");
                temp49 = this.IFRS2ManagedAdapterInstance.RequestUpdates(1, 1, FRS2Model.versionVectorDiff.valid);
                this.Manager.Checkpoint("MS-FRS2_R486");
                this.Manager.Checkpoint("MS-FRS2_R489");
                this.Manager.Comment("reaching state \'S62\'");
                this.Manager.Comment("checking step \'return RequestUpdates/ERROR_SUCCESS\'");
                this.Manager.Assert((temp49 == FRS2Model.error_status_t.ERROR_SUCCESS), String.Format("expected \'FRS2Model.error_status_t.ERROR_SUCCESS\', actual \'{0}\' (return of Reques" +
                            "tUpdates, state S62)", TestManagerHelpers.Describe(temp49)));
                this.Manager.Comment("reaching state \'S66\'");
                int temp54 = this.Manager.ExpectEvent(this.QuiescenceTimeout, true, new ExpectedEvent(TCtestScenario6.RequestUpdatesEventInfo, null, new RequestUpdatesEventDelegate1(this.TCtestScenario6S6RequestUpdatesEventChecker)), new ExpectedEvent(TCtestScenario6.RequestUpdatesEventInfo, null, new RequestUpdatesEventDelegate1(this.TCtestScenario6S6RequestUpdatesEventChecker1)), new ExpectedEvent(TCtestScenario6.RequestUpdatesEventInfo, null, new RequestUpdatesEventDelegate1(this.TCtestScenario6S6RequestUpdatesEventChecker2)), new ExpectedEvent(TCtestScenario6.RequestUpdatesEventInfo, null, new RequestUpdatesEventDelegate1(this.TCtestScenario6S6RequestUpdatesEventChecker3)));
                if ((temp54 == 0)) {
                    this.Manager.Comment("reaching state \'S79\'");
                    FRS2Model.error_status_t temp50;
                    this.Manager.Comment("executing step \'call UpdateCancel(4,valid,1)\'");
                    temp50 = this.IFRS2ManagedAdapterInstance.UpdateCancel(4, FRS2Model.FRS_UPDATE_CANCEL_DATA.valid, 1);
                    this.Manager.Checkpoint("MS-FRS2_R604");
                    this.Manager.Checkpoint("MS-FRS2_R609");
                    this.Manager.Comment("reaching state \'S95\'");
                    this.Manager.Comment("checking step \'return UpdateCancel/ERROR_FAIL\'");
                    this.Manager.Assert((temp50 == FRS2Model.error_status_t.ERROR_FAIL), String.Format("expected \'FRS2Model.error_status_t.ERROR_FAIL\', actual \'{0}\' (return of UpdateCan" +
                                "cel, state S95)", TestManagerHelpers.Describe(temp50)));
                    TCtestScenario6S101();
                    goto label6;
                }
                if ((temp54 == 1)) {
                    this.Manager.Comment("reaching state \'S80\'");
                    FRS2Model.error_status_t temp51;
                    this.Manager.Comment("executing step \'call UpdateCancel(4,valid,1)\'");
                    temp51 = this.IFRS2ManagedAdapterInstance.UpdateCancel(4, FRS2Model.FRS_UPDATE_CANCEL_DATA.valid, 1);
                    this.Manager.Checkpoint("MS-FRS2_R604");
                    this.Manager.Checkpoint("MS-FRS2_R609");
                    this.Manager.AddReturn(UpdateCancelInfo, null, temp51);
                    TCtestScenario6S96();
                    goto label6;
                }
                if ((temp54 == 2)) {
                    this.Manager.Comment("reaching state \'S81\'");
                    FRS2Model.error_status_t temp52;
                    this.Manager.Comment("executing step \'call UpdateCancel(4,valid,1)\'");
                    temp52 = this.IFRS2ManagedAdapterInstance.UpdateCancel(4, FRS2Model.FRS_UPDATE_CANCEL_DATA.valid, 1);
                    this.Manager.Checkpoint("MS-FRS2_R604");
                    this.Manager.Checkpoint("MS-FRS2_R609");
                    this.Manager.Comment("reaching state \'S97\'");
                    this.Manager.Comment("checking step \'return UpdateCancel/ERROR_FAIL\'");
                    this.Manager.Assert((temp52 == FRS2Model.error_status_t.ERROR_FAIL), String.Format("expected \'FRS2Model.error_status_t.ERROR_FAIL\', actual \'{0}\' (return of UpdateCan" +
                                "cel, state S97)", TestManagerHelpers.Describe(temp52)));
                    TCtestScenario6S100();
                    goto label6;
                }
                if ((temp54 == 3)) {
                    this.Manager.Comment("reaching state \'S82\'");
                    FRS2Model.error_status_t temp53;
                    this.Manager.Comment("executing step \'call UpdateCancel(4,valid,1)\'");
                    temp53 = this.IFRS2ManagedAdapterInstance.UpdateCancel(4, FRS2Model.FRS_UPDATE_CANCEL_DATA.valid, 1);
                    this.Manager.Checkpoint("MS-FRS2_R604");
                    this.Manager.Checkpoint("MS-FRS2_R609");
                    this.Manager.AddReturn(UpdateCancelInfo, null, temp53);
                    TCtestScenario6S96();
                    goto label6;
                }
                throw new InvalidOperationException("never reached");
            label6:
;
                goto label7;
            }
            if ((temp55 == 1)) {
                TCtestScenario6S52();
                goto label7;
            }
            throw new InvalidOperationException("never reached");
        label7:
;
            this.Manager.EndTest();
        }
        
        private void TCtestScenario6S6AsyncPollResponseEventChecker(FRS2Model.VVGeneration vvGen) {
            this.Manager.Comment("checking step \'event AsyncPollResponseEvent(ValidValue)\'");
            try {
                this.Manager.Assert((vvGen == FRS2Model.VVGeneration.ValidValue), String.Format("expected \'FRS2Model.VVGeneration.ValidValue\', actual \'{0}\' (vvGen of AsyncPollRes" +
                            "ponseEvent, state S50)", TestManagerHelpers.Describe(vvGen)));
            }
            catch (TransactionFailedException ) {
                this.Manager.Comment("This step would have covered MS-FRS2_R556, MS-FRS2_R1020, MS-FRS2_R555");
                throw;
            }
            this.Manager.Checkpoint("MS-FRS2_R556");
            this.Manager.Checkpoint("MS-FRS2_R1020");
            this.Manager.Checkpoint("MS-FRS2_R555");
        }
        
        private void TCtestScenario6S6RequestUpdatesEventChecker(FRS2Model.FilePresense present, FRS2Model.UPDATE_STATUS updateStatus) {
            this.Manager.Comment("checking step \'event RequestUpdatesEvent(fileExists,UPDATE_STATUS_DONE)\'");
            try {
                this.Manager.Assert((present == FRS2Model.FilePresense.fileExists), String.Format("expected \'FRS2Model.FilePresense.fileExists\', actual \'{0}\' (present of RequestUpd" +
                            "atesEvent, state S66)", TestManagerHelpers.Describe(present)));
                this.Manager.Assert((updateStatus == FRS2Model.UPDATE_STATUS.UPDATE_STATUS_DONE), String.Format("expected \'FRS2Model.UPDATE_STATUS.UPDATE_STATUS_DONE\', actual \'{0}\' (updateStatus" +
                            " of RequestUpdatesEvent, state S66)", TestManagerHelpers.Describe(updateStatus)));
            }
            catch (TransactionFailedException ) {
                this.Manager.Comment("This step would have covered MS-FRS2_R93, MS-FRS2_R94");
                throw;
            }
            this.Manager.Checkpoint("MS-FRS2_R93");
            this.Manager.Checkpoint("MS-FRS2_R94");
        }
        
        private void TCtestScenario6S6RequestUpdatesEventChecker1(FRS2Model.FilePresense present, FRS2Model.UPDATE_STATUS updateStatus) {
            this.Manager.Comment("checking step \'event RequestUpdatesEvent(fileDeleted,UPDATE_STATUS_DONE)\'");
            try {
                this.Manager.Assert((present == FRS2Model.FilePresense.fileDeleted), String.Format("expected \'FRS2Model.FilePresense.fileDeleted\', actual \'{0}\' (present of RequestUp" +
                            "datesEvent, state S66)", TestManagerHelpers.Describe(present)));
                this.Manager.Assert((updateStatus == FRS2Model.UPDATE_STATUS.UPDATE_STATUS_DONE), String.Format("expected \'FRS2Model.UPDATE_STATUS.UPDATE_STATUS_DONE\', actual \'{0}\' (updateStatus" +
                            " of RequestUpdatesEvent, state S66)", TestManagerHelpers.Describe(updateStatus)));
            }
            catch (TransactionFailedException ) {
                this.Manager.Comment("This step would have covered MS-FRS2_R93, MS-FRS2_R94");
                throw;
            }
            this.Manager.Checkpoint("MS-FRS2_R93");
            this.Manager.Checkpoint("MS-FRS2_R94");
        }
        
        private void TCtestScenario6S96() {
            this.Manager.Comment("reaching state \'S96\'");
            this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(TCtestScenario6.UpdateCancelInfo, null, new UpdateCancelDelegate1(this.TCtestScenario6S6UpdateCancelChecker)));
            TCtestScenario6S99();
        }
        
        private void TCtestScenario6S6UpdateCancelChecker(FRS2Model.error_status_t @return) {
            this.Manager.Comment("checking step \'return UpdateCancel/ERROR_FAIL\'");
            this.Manager.Assert((@return == FRS2Model.error_status_t.ERROR_FAIL), String.Format("expected \'FRS2Model.error_status_t.ERROR_FAIL\', actual \'{0}\' (return of UpdateCan" +
                        "cel, state S96)", TestManagerHelpers.Describe(@return)));
        }
        
        private void TCtestScenario6S6RequestUpdatesEventChecker2(FRS2Model.FilePresense present, FRS2Model.UPDATE_STATUS updateStatus) {
            this.Manager.Comment("checking step \'event RequestUpdatesEvent(fileExists,UPDATE_STATUS_MORE)\'");
            try {
                this.Manager.Assert((present == FRS2Model.FilePresense.fileExists), String.Format("expected \'FRS2Model.FilePresense.fileExists\', actual \'{0}\' (present of RequestUpd" +
                            "atesEvent, state S66)", TestManagerHelpers.Describe(present)));
                this.Manager.Assert((updateStatus == FRS2Model.UPDATE_STATUS.UPDATE_STATUS_MORE), String.Format("expected \'FRS2Model.UPDATE_STATUS.UPDATE_STATUS_MORE\', actual \'{0}\' (updateStatus" +
                            " of RequestUpdatesEvent, state S66)", TestManagerHelpers.Describe(updateStatus)));
            }
            catch (TransactionFailedException ) {
                this.Manager.Comment("This step would have covered MS-FRS2_R93, MS-FRS2_R498");
                throw;
            }
            this.Manager.Checkpoint("MS-FRS2_R93");
            this.Manager.Checkpoint("MS-FRS2_R498");
        }
        
        private void TCtestScenario6S6RequestUpdatesEventChecker3(FRS2Model.FilePresense present, FRS2Model.UPDATE_STATUS updateStatus) {
            this.Manager.Comment("checking step \'event RequestUpdatesEvent(fileDeleted,UPDATE_STATUS_MORE)\'");
            try {
                this.Manager.Assert((present == FRS2Model.FilePresense.fileDeleted), String.Format("expected \'FRS2Model.FilePresense.fileDeleted\', actual \'{0}\' (present of RequestUp" +
                            "datesEvent, state S66)", TestManagerHelpers.Describe(present)));
                this.Manager.Assert((updateStatus == FRS2Model.UPDATE_STATUS.UPDATE_STATUS_MORE), String.Format("expected \'FRS2Model.UPDATE_STATUS.UPDATE_STATUS_MORE\', actual \'{0}\' (updateStatus" +
                            " of RequestUpdatesEvent, state S66)", TestManagerHelpers.Describe(updateStatus)));
            }
            catch (TransactionFailedException ) {
                this.Manager.Comment("This step would have covered MS-FRS2_R93");
                throw;
            }
            this.Manager.Checkpoint("MS-FRS2_R93");
        }
        
        private void TCtestScenario6S6AsyncPollResponseEventChecker1(FRS2Model.VVGeneration vvGen) {
            this.Manager.Comment("checking step \'event AsyncPollResponseEvent(InvalidValue)\'");
            try {
                this.Manager.Assert((vvGen == FRS2Model.VVGeneration.InvalidValue), String.Format("expected \'FRS2Model.VVGeneration.InvalidValue\', actual \'{0}\' (vvGen of AsyncPollR" +
                            "esponseEvent, state S50)", TestManagerHelpers.Describe(vvGen)));
            }
            catch (TransactionFailedException ) {
                this.Manager.Comment("This step would have covered MS-FRS2_R556, MS-FRS2_R1020, MS-FRS2_R555");
                throw;
            }
            this.Manager.Checkpoint("MS-FRS2_R556");
            this.Manager.Checkpoint("MS-FRS2_R1020");
            this.Manager.Checkpoint("MS-FRS2_R555");
        }
        #endregion
        
        #region Test Starting in S8
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        [Microsoft.VisualStudio.TestTools.UnitTesting.DescriptionAttribute("MS-FRS2_R37, MS-FRS2_R436, MS-FRS2_R439, MS-FRS2_R549, MS-FRS2_R552")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestCategory("AD")]
        [TestCategory("MS-FRS2")]
        [TestCategory("SDC")]
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        
        public virtual void FRS2_TCtestScenario6S8()
        {
            this.Manager.BeginTest("TCtestScenario6S8");
            this.Manager.Comment("reaching state \'S8\'");
            FRS2Model.error_status_t temp56;
            this.Manager.Comment(@"executing step 'call Initialization(Windows7,Map{1=>enabled,2=>enabled,3=>enabled,4=>disabled},Map{1=>exists,2=>exists,3=>exists,4=>exists},Map{""A""=>1,""B""=>2,""C""=>3,""D""=>4},Map{""A""=>sysvol,""B""=>protection,""C""=>sysvol,""D""=>sysvol},Map{1=>""A"",2=>""B"",3=>""C"",4=>""D""},Map{1=>writeOnly,2=>readWrite,3=>readOnly,4=>readWrite},Map{1=>enabled,2=>disabled,3=>enabled,4=>enabled})'");
            temp56 = this.IFRS2ManagedAdapterInstance.Initialization(FRS2Model.OSVersion.Windows7, new Microsoft.Modeling.Map<System.Int32,FRS2Model.connectionProperty>().Add(1, FRS2Model.connectionProperty.enabled).Add(2, FRS2Model.connectionProperty.enabled).Add(3, FRS2Model.connectionProperty.enabled).Add(4, FRS2Model.connectionProperty.disabled), new Microsoft.Modeling.Map<System.Int32,FRS2Model.FromServer>().Add(1, FRS2Model.FromServer.exists).Add(2, FRS2Model.FromServer.exists).Add(3, FRS2Model.FromServer.exists).Add(4, FRS2Model.FromServer.exists), new Microsoft.Modeling.Map<System.String,System.Int32>().Add("A", 1).Add("B", 2).Add("C", 3).Add("D", 4), new Microsoft.Modeling.Map<System.String,FRS2Model.ReplicationGroupTypes>().Add("A", FRS2Model.ReplicationGroupTypes.sysvol).Add("B", FRS2Model.ReplicationGroupTypes.protection).Add("C", FRS2Model.ReplicationGroupTypes.sysvol).Add("D", FRS2Model.ReplicationGroupTypes.sysvol), new Microsoft.Modeling.Map<System.Int32,System.String>().Add(1, "A").Add(2, "B").Add(3, "C").Add(4, "D"), new Microsoft.Modeling.Map<System.Int32,FRS2Model.accessLevels>().Add(1, FRS2Model.accessLevels.writeOnly).Add(2, FRS2Model.accessLevels.readWrite).Add(3, FRS2Model.accessLevels.readOnly).Add(4, FRS2Model.accessLevels.readWrite), new Microsoft.Modeling.Map<System.Int32,FRS2Model.connectionProperty>().Add(1, FRS2Model.connectionProperty.enabled).Add(2, FRS2Model.connectionProperty.disabled).Add(3, FRS2Model.connectionProperty.enabled).Add(4, FRS2Model.connectionProperty.enabled));
            this.Manager.Comment("reaching state \'S9\'");
            this.Manager.Comment("checking step \'return Initialization/ERROR_SUCCESS\'");
            this.Manager.Assert((temp56 == FRS2Model.error_status_t.ERROR_SUCCESS), String.Format("expected \'FRS2Model.error_status_t.ERROR_SUCCESS\', actual \'{0}\' (return of Initia" +
                        "lization, state S9)", TestManagerHelpers.Describe(temp56)));
            this.Manager.Comment("reaching state \'S14\'");
            FRS2Model.ProtocolVersionReturned temp57;
            FRS2Model.UpstreamFlagValueReturned temp58;
            FRS2Model.error_status_t temp59;
            this.Manager.Comment("executing step \'call EstablishConnection(\"A\",1,FRS_COMMUNICATION_PROTOCOL_VERSION" +
                    "_LONGHORN_SERVER,out _,out _)\'");
            temp59 = this.IFRS2ManagedAdapterInstance.EstablishConnection("A", 1, FRS2Model.ProtocolVersion.FRS_COMMUNICATION_PROTOCOL_VERSION_LONGHORN_SERVER, out temp57, out temp58);
            this.Manager.Checkpoint("MS-FRS2_R37");
            this.Manager.Checkpoint("MS-FRS2_R436");
            this.Manager.Checkpoint("MS-FRS2_R439");
            this.Manager.Comment("reaching state \'S19\'");
            this.Manager.Comment("checking step \'return EstablishConnection/[out Valid,out Valid]:ERROR_SUCCESS\'");
            this.Manager.Assert((temp57 == FRS2Model.ProtocolVersionReturned.Valid), String.Format("expected \'FRS2Model.ProtocolVersionReturned.Valid\', actual \'{0}\' (upstreamProtoco" +
                        "lVersion of EstablishConnection, state S19)", TestManagerHelpers.Describe(temp57)));
            this.Manager.Assert((temp58 == FRS2Model.UpstreamFlagValueReturned.Valid), String.Format("expected \'FRS2Model.UpstreamFlagValueReturned.Valid\', actual \'{0}\' (upstreamFlags" +
                        " of EstablishConnection, state S19)", TestManagerHelpers.Describe(temp58)));
            this.Manager.Assert((temp59 == FRS2Model.error_status_t.ERROR_SUCCESS), String.Format("expected \'FRS2Model.error_status_t.ERROR_SUCCESS\', actual \'{0}\' (return of Establ" +
                        "ishConnection, state S19)", TestManagerHelpers.Describe(temp59)));
            this.Manager.Comment("reaching state \'S24\'");
            FRS2Model.error_status_t temp60;
            this.Manager.Comment("executing step \'call AsyncPoll(1)\'");
            temp60 = this.IFRS2ManagedAdapterInstance.AsyncPoll(1);
            this.Manager.Checkpoint("MS-FRS2_R549");
            this.Manager.Checkpoint("MS-FRS2_R552");
            this.Manager.Comment("reaching state \'S29\'");
            this.Manager.Comment("checking step \'return AsyncPoll/ERROR_SUCCESS\'");
            this.Manager.Assert((temp60 == FRS2Model.error_status_t.ERROR_SUCCESS), String.Format("expected \'FRS2Model.error_status_t.ERROR_SUCCESS\', actual \'{0}\' (return of AsyncP" +
                        "oll, state S29)", TestManagerHelpers.Describe(temp60)));
            this.Manager.Comment("reaching state \'S34\'");
            this.Manager.EndTest();
        }
        #endregion
    }
}
