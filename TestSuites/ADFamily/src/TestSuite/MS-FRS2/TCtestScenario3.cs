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
    public partial class TCtestScenario3 : PtfTestClassBase {
        
        public TCtestScenario3() {
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
        
        public delegate void InitializeFileTransferAsyncDelegate1(FRS2Model.error_status_t @return);
        
        public delegate void InitializeFileTransferAsyncEventDelegate1(FRS2Model.ServerContext context, FRS2Model.RDC_Sig_Level rdcsigLevel, bool isEOF);
        
        public delegate void RdcGetSignaturesDelegate1(FRS2Model.error_status_t @return);
        
        public delegate void RdcGetFileDataDelegate1(FRS2Model.error_status_t @return);
        
        public delegate void RdcPushSourceNeedsDelegate1(FRS2Model.error_status_t @return);
        
        public delegate void RdcCloseDelegate1(FRS2Model.error_status_t @return);
        
        public delegate void CheckConnectivityDelegate1(FRS2Model.error_status_t @return);
        
        public delegate void RequestRecordsDelegate1(FRS2Model.error_status_t @return);
        
        public delegate void UpdateCancelDelegate1(FRS2Model.error_status_t @return);
        
        public delegate void RawGetFileDataDelegate1(FRS2Model.error_status_t @return);
        
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
        
        static System.Reflection.MethodBase InitializeFileTransferAsyncInfo = TestManagerHelpers.GetMethodInfo(typeof(Microsoft.Protocols.TestSuites.MS_FRS2.IFRS2ManagedAdapter), "InitializeFileTransferAsync", typeof(int), typeof(int), typeof(bool));
        
        static System.Reflection.EventInfo InitializeFileTransferAsyncEventInfo = TestManagerHelpers.GetEventInfo(typeof(Microsoft.Protocols.TestSuites.MS_FRS2.IFRS2ManagedAdapter), "InitializeFileTransferAsyncEvent");
        
        static System.Reflection.MethodBase RdcGetSignaturesInfo = TestManagerHelpers.GetMethodInfo(typeof(Microsoft.Protocols.TestSuites.MS_FRS2.IFRS2ManagedAdapter), "RdcGetSignatures", typeof(FRS2Model.offset));
        
        static System.Reflection.MethodBase RdcGetFileDataInfo = TestManagerHelpers.GetMethodInfo(typeof(Microsoft.Protocols.TestSuites.MS_FRS2.IFRS2ManagedAdapter), "RdcGetFileData", typeof(FRS2Model.BufferSize));
        
        static System.Reflection.MethodBase RdcPushSourceNeedsInfo = TestManagerHelpers.GetMethodInfo(typeof(Microsoft.Protocols.TestSuites.MS_FRS2.IFRS2ManagedAdapter), "RdcPushSourceNeeds");
        
        static System.Reflection.MethodBase RdcCloseInfo = TestManagerHelpers.GetMethodInfo(typeof(Microsoft.Protocols.TestSuites.MS_FRS2.IFRS2ManagedAdapter), "RdcClose");
        
        static System.Reflection.MethodBase CheckConnectivityInfo = TestManagerHelpers.GetMethodInfo(typeof(Microsoft.Protocols.TestSuites.MS_FRS2.IFRS2ManagedAdapter), "CheckConnectivity", typeof(string), typeof(int));
        
        static System.Reflection.MethodBase RequestRecordsInfo = TestManagerHelpers.GetMethodInfo(typeof(Microsoft.Protocols.TestSuites.MS_FRS2.IFRS2ManagedAdapter), "RequestRecords", typeof(int), typeof(int));
        
        static System.Reflection.MethodBase UpdateCancelInfo = TestManagerHelpers.GetMethodInfo(typeof(Microsoft.Protocols.TestSuites.MS_FRS2.IFRS2ManagedAdapter), "UpdateCancel", typeof(int), typeof(FRS2Model.FRS_UPDATE_CANCEL_DATA), typeof(int));
        
        static System.Reflection.MethodBase RawGetFileDataInfo = TestManagerHelpers.GetMethodInfo(typeof(Microsoft.Protocols.TestSuites.MS_FRS2.IFRS2ManagedAdapter), "RawGetFileData");
        
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
        [Microsoft.VisualStudio.TestTools.UnitTesting.DescriptionAttribute("MS-FRS2_R37, MS-FRS2_R436, MS-FRS2_R439, MS-FRS2_R455, MS-FRS2_R458, MS-FRS2_R448" +
            ", MS-FRS2_R549, MS-FRS2_R552, MS-FRS2_R517, MS-FRS2_R520, MS-FRS2_R466, MS-FRS2_" +
            "R556, MS-FRS2_R1020, MS-FRS2_R555, MS-FRS2_R486, MS-FRS2_R489, MS-FRS2_R93, MS-F" +
            "RS2_R774, MS-FRS2_R777, MS-FRS2_R793, MS-FRS2_R647, MS-FRS2_R661, MS-FRS2_R805, " +
            "MS-FRS2_R647, MS-FRS2_R655, MS-FRS2_R656, MS-FRS2_R657, MS-FRS2_R661, MS-FRS2_R8" +
            "05, MS-FRS2_R647, MS-FRS2_R658, MS-FRS2_R661, MS-FRS2_R805, MS-FRS2_R647, MS-FRS" +
            "2_R655, MS-FRS2_R656, MS-FRS2_R657, MS-FRS2_R658, MS-FRS2_R661, MS-FRS2_R805, MS" +
            "-FRS2_R646, MS-FRS2_R649, MS-FRS2_R650, MS-FRS2_R702, MS-FRS2_R737, MS-FRS2_R739" +
            ", MS-FRS2_R93, MS-FRS2_R94, MS-FRS2_R774, MS-FRS2_R777, MS-FRS2_R793, MS-FRS2_R6" +
            "47, MS-FRS2_R658, MS-FRS2_R661, MS-FRS2_R805, MS-FRS2_R647, MS-FRS2_R655, MS-FRS" +
            "2_R656, MS-FRS2_R657, MS-FRS2_R658, MS-FRS2_R661, MS-FRS2_R805, MS-FRS2_R647, MS" +
            "-FRS2_R661, MS-FRS2_R805, MS-FRS2_R647, MS-FRS2_R655, MS-FRS2_R656, MS-FRS2_R657" +
            ", MS-FRS2_R661, MS-FRS2_R805, MS-FRS2_R646, MS-FRS2_R649, MS-FRS2_R650, MS-FRS2_" +
            "R702, MS-FRS2_R737, MS-FRS2_R739, MS-FRS2_R93, MS-FRS2_R498, MS-FRS2_R774, MS-FR" +
            "S2_R777, MS-FRS2_R793, MS-FRS2_R647, MS-FRS2_R658, MS-FRS2_R661, MS-FRS2_R805, M" +
            "S-FRS2_R647, MS-FRS2_R655, MS-FRS2_R656, MS-FRS2_R657, MS-FRS2_R658, MS-FRS2_R66" +
            "1, MS-FRS2_R805, MS-FRS2_R647, MS-FRS2_R661, MS-FRS2_R805, MS-FRS2_R646, MS-FRS2" +
            "_R649, MS-FRS2_R650, MS-FRS2_R702, MS-FRS2_R737, MS-FRS2_R739, MS-FRS2_R647, MS-" +
            "FRS2_R655, MS-FRS2_R656, MS-FRS2_R657, MS-FRS2_R661, MS-FRS2_R805, MS-FRS2_R93, " +
            "MS-FRS2_R94, MS-FRS2_R774, MS-FRS2_R777, MS-FRS2_R793, MS-FRS2_R556, MS-FRS2_R10" +
            "20, MS-FRS2_R555")]
        public virtual void FRS2_TCtestScenario3S0() {
            this.Manager.BeginTest("TCtestScenario3S0");
            this.Manager.Comment("reaching state \'S0\'");
            FRS2Model.error_status_t temp0;
            this.Manager.Comment(@"executing step 'call Initialization(Windows7,Map{1=>enabled,2=>enabled,3=>enabled,4=>disabled},Map{1=>exists,2=>exists,3=>exists,4=>exists},Map{""A""=>1,""B""=>2,""C""=>3,""D""=>4},Map{""A""=>sysvol,""B""=>protection,""C""=>sysvol,""D""=>sysvol},Map{1=>""A"",2=>""B"",3=>""C"",4=>""D""},Map{1=>writeOnly,2=>readWrite,3=>readOnly,4=>readWrite},Map{1=>enabled,2=>disabled,3=>enabled,4=>enabled})'");
            temp0 = this.IFRS2ManagedAdapterInstance.Initialization(FRS2Model.OSVersion.Windows7, new Microsoft.Modeling.Map<System.Int32,FRS2Model.connectionProperty>().Add(1, FRS2Model.connectionProperty.enabled).Add(2, FRS2Model.connectionProperty.enabled).Add(3, FRS2Model.connectionProperty.enabled).Add(4, FRS2Model.connectionProperty.disabled), new Microsoft.Modeling.Map<System.Int32,FRS2Model.FromServer>().Add(1, FRS2Model.FromServer.exists).Add(2, FRS2Model.FromServer.exists).Add(3, FRS2Model.FromServer.exists).Add(4, FRS2Model.FromServer.exists), new Microsoft.Modeling.Map<System.String,System.Int32>().Add("A", 1).Add("B", 2).Add("C", 3).Add("D", 4), new Microsoft.Modeling.Map<System.String,FRS2Model.ReplicationGroupTypes>().Add("A", FRS2Model.ReplicationGroupTypes.sysvol).Add("B", FRS2Model.ReplicationGroupTypes.protection).Add("C", FRS2Model.ReplicationGroupTypes.sysvol).Add("D", FRS2Model.ReplicationGroupTypes.sysvol), new Microsoft.Modeling.Map<System.Int32,System.String>().Add(1, "A").Add(2, "B").Add(3, "C").Add(4, "D"), new Microsoft.Modeling.Map<System.Int32,FRS2Model.accessLevels>().Add(1, FRS2Model.accessLevels.writeOnly).Add(2, FRS2Model.accessLevels.readWrite).Add(3, FRS2Model.accessLevels.readOnly).Add(4, FRS2Model.accessLevels.readWrite), new Microsoft.Modeling.Map<System.Int32,FRS2Model.connectionProperty>().Add(1, FRS2Model.connectionProperty.enabled).Add(2, FRS2Model.connectionProperty.disabled).Add(3, FRS2Model.connectionProperty.enabled).Add(4, FRS2Model.connectionProperty.enabled));
            this.Manager.Comment("reaching state \'S1\'");
            this.Manager.Comment("checking step \'return Initialization/ERROR_SUCCESS\'");
            this.Manager.Assert((temp0 == FRS2Model.error_status_t.ERROR_SUCCESS), String.Format("expected \'FRS2Model.error_status_t.ERROR_SUCCESS\', actual \'{0}\' (return of Initia" +
                        "lization, state S1)", TestManagerHelpers.Describe(temp0)));
            this.Manager.Comment("reaching state \'S18\'");
            FRS2Model.ProtocolVersionReturned temp1;
            FRS2Model.UpstreamFlagValueReturned temp2;
            FRS2Model.error_status_t temp3;
            this.Manager.Comment("executing step \'call EstablishConnection(\"A\",1,FRS_COMMUNICATION_PROTOCOL_VERSION" +
                    "_LONGHORN_SERVER,out _,out _)\'");
            temp3 = this.IFRS2ManagedAdapterInstance.EstablishConnection("A", 1, FRS2Model.ProtocolVersion.FRS_COMMUNICATION_PROTOCOL_VERSION_LONGHORN_SERVER, out temp1, out temp2);
            this.Manager.Checkpoint("MS-FRS2_R37");
            this.Manager.Checkpoint("MS-FRS2_R436");
            this.Manager.Checkpoint("MS-FRS2_R439");
            this.Manager.Comment("reaching state \'S27\'");
            this.Manager.Comment("checking step \'return EstablishConnection/[out Valid,out Valid]:ERROR_SUCCESS\'");
            this.Manager.Assert((temp1 == FRS2Model.ProtocolVersionReturned.Valid), String.Format("expected \'FRS2Model.ProtocolVersionReturned.Valid\', actual \'{0}\' (upstreamProtoco" +
                        "lVersion of EstablishConnection, state S27)", TestManagerHelpers.Describe(temp1)));
            this.Manager.Assert((temp2 == FRS2Model.UpstreamFlagValueReturned.Valid), String.Format("expected \'FRS2Model.UpstreamFlagValueReturned.Valid\', actual \'{0}\' (upstreamFlags" +
                        " of EstablishConnection, state S27)", TestManagerHelpers.Describe(temp2)));
            this.Manager.Assert((temp3 == FRS2Model.error_status_t.ERROR_SUCCESS), String.Format("expected \'FRS2Model.error_status_t.ERROR_SUCCESS\', actual \'{0}\' (return of Establ" +
                        "ishConnection, state S27)", TestManagerHelpers.Describe(temp3)));
            this.Manager.Comment("reaching state \'S36\'");
            FRS2Model.error_status_t temp4;
            this.Manager.Comment("executing step \'call EstablishSession(1,1)\'");
            temp4 = this.IFRS2ManagedAdapterInstance.EstablishSession(1, 1);
            this.Manager.Checkpoint("MS-FRS2_R455");
            this.Manager.Checkpoint("MS-FRS2_R458");
            this.Manager.Checkpoint("MS-FRS2_R448");
            this.Manager.Comment("reaching state \'S45\'");
            this.Manager.Comment("checking step \'return EstablishSession/ERROR_SUCCESS\'");
            this.Manager.Assert((temp4 == FRS2Model.error_status_t.ERROR_SUCCESS), String.Format("expected \'FRS2Model.error_status_t.ERROR_SUCCESS\', actual \'{0}\' (return of Establ" +
                        "ishSession, state S45)", TestManagerHelpers.Describe(temp4)));
            this.Manager.Comment("reaching state \'S54\'");
            FRS2Model.error_status_t temp5;
            this.Manager.Comment("executing step \'call AsyncPoll(1)\'");
            temp5 = this.IFRS2ManagedAdapterInstance.AsyncPoll(1);
            this.Manager.Checkpoint("MS-FRS2_R549");
            this.Manager.Checkpoint("MS-FRS2_R552");
            this.Manager.Comment("reaching state \'S63\'");
            this.Manager.Comment("checking step \'return AsyncPoll/ERROR_SUCCESS\'");
            this.Manager.Assert((temp5 == FRS2Model.error_status_t.ERROR_SUCCESS), String.Format("expected \'FRS2Model.error_status_t.ERROR_SUCCESS\', actual \'{0}\' (return of AsyncP" +
                        "oll, state S63)", TestManagerHelpers.Describe(temp5)));
            this.Manager.Comment("reaching state \'S71\'");
            FRS2Model.error_status_t temp6;
            this.Manager.Comment("executing step \'call RequestVersionVector(1,1,1,REQUEST_NORMAL_SYNC,CHANGE_ALL,Va" +
                    "lidValue)\'");
            temp6 = this.IFRS2ManagedAdapterInstance.RequestVersionVector(1, 1, 1, FRS2Model.VERSION_REQUEST_TYPE.REQUEST_NORMAL_SYNC, FRS2Model.VERSION_CHANGE_TYPE.CHANGE_ALL, FRS2Model.VVGeneration.ValidValue);
            this.Manager.Checkpoint("MS-FRS2_R517");
            this.Manager.Checkpoint("MS-FRS2_R520");
            this.Manager.Checkpoint("MS-FRS2_R466");
            this.Manager.Comment("reaching state \'S79\'");
            this.Manager.Comment("checking step \'return RequestVersionVector/ERROR_SUCCESS\'");
            this.Manager.Assert((temp6 == FRS2Model.error_status_t.ERROR_SUCCESS), String.Format("expected \'FRS2Model.error_status_t.ERROR_SUCCESS\', actual \'{0}\' (return of Reques" +
                        "tVersionVector, state S79)", TestManagerHelpers.Describe(temp6)));
            this.Manager.Comment("reaching state \'S87\'");
            int temp37 = this.Manager.ExpectEvent(this.QuiescenceTimeout, true, new ExpectedEvent(TCtestScenario3.AsyncPollResponseEventInfo, null, new AsyncPollResponseEventDelegate1(this.TCtestScenario3S0AsyncPollResponseEventChecker)), new ExpectedEvent(TCtestScenario3.AsyncPollResponseEventInfo, null, new AsyncPollResponseEventDelegate1(this.TCtestScenario3S0AsyncPollResponseEventChecker1)));
            if ((temp37 == 0)) {
                this.Manager.Comment("reaching state \'S95\'");
                FRS2Model.error_status_t temp7;
                this.Manager.Comment("executing step \'call RequestUpdates(1,1,valid)\'");
                temp7 = this.IFRS2ManagedAdapterInstance.RequestUpdates(1, 1, FRS2Model.versionVectorDiff.valid);
                this.Manager.Checkpoint("MS-FRS2_R486");
                this.Manager.Checkpoint("MS-FRS2_R489");
                this.Manager.Comment("reaching state \'S111\'");
                this.Manager.Comment("checking step \'return RequestUpdates/ERROR_SUCCESS\'");
                this.Manager.Assert((temp7 == FRS2Model.error_status_t.ERROR_SUCCESS), String.Format("expected \'FRS2Model.error_status_t.ERROR_SUCCESS\', actual \'{0}\' (return of Reques" +
                            "tUpdates, state S111)", TestManagerHelpers.Describe(temp7)));
                this.Manager.Comment("reaching state \'S119\'");
                int temp36 = this.Manager.ExpectEvent(this.QuiescenceTimeout, true, new ExpectedEvent(TCtestScenario3.RequestUpdatesEventInfo, null, new RequestUpdatesEventDelegate1(this.TCtestScenario3S0RequestUpdatesEventChecker)), new ExpectedEvent(TCtestScenario3.RequestUpdatesEventInfo, null, new RequestUpdatesEventDelegate1(this.TCtestScenario3S0RequestUpdatesEventChecker1)), new ExpectedEvent(TCtestScenario3.RequestUpdatesEventInfo, null, new RequestUpdatesEventDelegate1(this.TCtestScenario3S0RequestUpdatesEventChecker2)), new ExpectedEvent(TCtestScenario3.RequestUpdatesEventInfo, null, new RequestUpdatesEventDelegate1(this.TCtestScenario3S0RequestUpdatesEventChecker3)));
                if ((temp36 == 0)) {
                    TCtestScenario3S127();
                    goto label3;
                }
                if ((temp36 == 1)) {
                    this.Manager.Comment("reaching state \'S128\'");
                    FRS2Model.error_status_t temp17;
                    this.Manager.Comment("executing step \'call InitializeFileTransferAsync(1,1,True)\'");
                    temp17 = this.IFRS2ManagedAdapterInstance.InitializeFileTransferAsync(1, 1, true);
                    this.Manager.Checkpoint("MS-FRS2_R774");
                    this.Manager.Checkpoint("MS-FRS2_R777");
                    this.Manager.Checkpoint("MS-FRS2_R793");
                    this.Manager.Comment("reaching state \'S156\'");
                    this.Manager.Comment("checking step \'return InitializeFileTransferAsync/ERROR_SUCCESS\'");
                    this.Manager.Assert((temp17 == FRS2Model.error_status_t.ERROR_SUCCESS), String.Format("expected \'FRS2Model.error_status_t.ERROR_SUCCESS\', actual \'{0}\' (return of Initia" +
                                "lizeFileTransferAsync, state S156)", TestManagerHelpers.Describe(temp17)));
                    this.Manager.Comment("reaching state \'S175\'");
                    int temp25 = this.Manager.ExpectEvent(this.QuiescenceTimeout, true, new ExpectedEvent(TCtestScenario3.InitializeFileTransferAsyncEventInfo, null, new InitializeFileTransferAsyncEventDelegate1(this.TCtestScenario3S0InitializeFileTransferAsyncEventChecker8)), new ExpectedEvent(TCtestScenario3.InitializeFileTransferAsyncEventInfo, null, new InitializeFileTransferAsyncEventDelegate1(this.TCtestScenario3S0InitializeFileTransferAsyncEventChecker9)), new ExpectedEvent(TCtestScenario3.InitializeFileTransferAsyncEventInfo, null, new InitializeFileTransferAsyncEventDelegate1(this.TCtestScenario3S0InitializeFileTransferAsyncEventChecker10)), new ExpectedEvent(TCtestScenario3.InitializeFileTransferAsyncEventInfo, null, new InitializeFileTransferAsyncEventDelegate1(this.TCtestScenario3S0InitializeFileTransferAsyncEventChecker11)), new ExpectedEvent(TCtestScenario3.InitializeFileTransferAsyncEventInfo, null, new InitializeFileTransferAsyncEventDelegate1(this.TCtestScenario3S0InitializeFileTransferAsyncEventChecker12)), new ExpectedEvent(TCtestScenario3.InitializeFileTransferAsyncEventInfo, null, new InitializeFileTransferAsyncEventDelegate1(this.TCtestScenario3S0InitializeFileTransferAsyncEventChecker13)), new ExpectedEvent(TCtestScenario3.InitializeFileTransferAsyncEventInfo, null, new InitializeFileTransferAsyncEventDelegate1(this.TCtestScenario3S0InitializeFileTransferAsyncEventChecker14)), new ExpectedEvent(TCtestScenario3.InitializeFileTransferAsyncEventInfo, null, new InitializeFileTransferAsyncEventDelegate1(this.TCtestScenario3S0InitializeFileTransferAsyncEventChecker15)));
                    if ((temp25 == 0)) {
                        this.Manager.Comment("reaching state \'S200\'");
                        FRS2Model.error_status_t temp18;
                        this.Manager.Comment("executing step \'call RdcGetSignatures(inValid)\'");
                        temp18 = this.IFRS2ManagedAdapterInstance.RdcGetSignatures(FRS2Model.offset.inValid);
                        this.Manager.Checkpoint("MS-FRS2_R647");
                        this.Manager.Checkpoint("MS-FRS2_R658");
                        this.Manager.Checkpoint("MS-FRS2_R661");
                        this.Manager.Checkpoint("MS-FRS2_R805");
                        this.Manager.Comment("reaching state \'S341\'");
                        this.Manager.Comment("checking step \'return RdcGetSignatures/ERROR_FAIL\'");
                        this.Manager.Assert((temp18 == FRS2Model.error_status_t.ERROR_FAIL), String.Format("expected \'FRS2Model.error_status_t.ERROR_FAIL\', actual \'{0}\' (return of RdcGetSig" +
                                    "natures, state S341)", TestManagerHelpers.Describe(temp18)));
                        TCtestScenario3S374();
                        goto label1;
                    }
                    if ((temp25 == 1)) {
                        this.Manager.Comment("reaching state \'S201\'");
                        FRS2Model.error_status_t temp19;
                        this.Manager.Comment("executing step \'call RdcGetSignatures(inValid)\'");
                        temp19 = this.IFRS2ManagedAdapterInstance.RdcGetSignatures(FRS2Model.offset.inValid);
                        this.Manager.Checkpoint("MS-FRS2_R647");
                        this.Manager.Checkpoint("MS-FRS2_R655");
                        this.Manager.Checkpoint("MS-FRS2_R656");
                        this.Manager.Checkpoint("MS-FRS2_R657");
                        this.Manager.Checkpoint("MS-FRS2_R658");
                        this.Manager.Checkpoint("MS-FRS2_R661");
                        this.Manager.Checkpoint("MS-FRS2_R805");
                        this.Manager.Comment("reaching state \'S342\'");
                        this.Manager.Comment("checking step \'return RdcGetSignatures/ERROR_FAIL\'");
                        this.Manager.Assert((temp19 == FRS2Model.error_status_t.ERROR_FAIL), String.Format("expected \'FRS2Model.error_status_t.ERROR_FAIL\', actual \'{0}\' (return of RdcGetSig" +
                                    "natures, state S342)", TestManagerHelpers.Describe(temp19)));
                        TCtestScenario3S375();
                        goto label1;
                    }
                    if ((temp25 == 2)) {
                        TCtestScenario3S202();
                        goto label1;
                    }
                    if ((temp25 == 3)) {
                        TCtestScenario3S203();
                        goto label1;
                    }
                    if ((temp25 == 4)) {
                        this.Manager.Comment("reaching state \'S204\'");
                        FRS2Model.error_status_t temp21;
                        this.Manager.Comment("executing step \'call RdcGetSignatures(inValid)\'");
                        temp21 = this.IFRS2ManagedAdapterInstance.RdcGetSignatures(FRS2Model.offset.inValid);
                        this.Manager.Checkpoint("MS-FRS2_R647");
                        this.Manager.Checkpoint("MS-FRS2_R655");
                        this.Manager.Checkpoint("MS-FRS2_R656");
                        this.Manager.Checkpoint("MS-FRS2_R657");
                        this.Manager.Checkpoint("MS-FRS2_R661");
                        this.Manager.Checkpoint("MS-FRS2_R805");
                        this.Manager.Comment("reaching state \'S344\'");
                        this.Manager.Comment("checking step \'return RdcGetSignatures/ERROR_FAIL\'");
                        this.Manager.Assert((temp21 == FRS2Model.error_status_t.ERROR_FAIL), String.Format("expected \'FRS2Model.error_status_t.ERROR_FAIL\', actual \'{0}\' (return of RdcGetSig" +
                                    "natures, state S344)", TestManagerHelpers.Describe(temp21)));
                        TCtestScenario3S377();
                        goto label1;
                    }
                    if ((temp25 == 5)) {
                        this.Manager.Comment("reaching state \'S205\'");
                        FRS2Model.error_status_t temp22;
                        this.Manager.Comment("executing step \'call RdcGetSignatures(valid)\'");
                        temp22 = this.IFRS2ManagedAdapterInstance.RdcGetSignatures(FRS2Model.offset.valid);
                        this.Manager.Checkpoint("MS-FRS2_R646");
                        this.Manager.Checkpoint("MS-FRS2_R649");
                        this.Manager.Checkpoint("MS-FRS2_R650");
                        this.Manager.Comment("reaching state \'S345\'");
                        this.Manager.Comment("checking step \'return RdcGetSignatures/ERROR_SUCCESS\'");
                        this.Manager.Assert((temp22 == FRS2Model.error_status_t.ERROR_SUCCESS), String.Format("expected \'FRS2Model.error_status_t.ERROR_SUCCESS\', actual \'{0}\' (return of RdcGet" +
                                    "Signatures, state S345)", TestManagerHelpers.Describe(temp22)));
                        this.Manager.Comment("reaching state \'S378\'");
                        FRS2Model.error_status_t temp23;
                        this.Manager.Comment("executing step \'call RdcGetFileData(validBufSize)\'");
                        temp23 = this.IFRS2ManagedAdapterInstance.RdcGetFileData(FRS2Model.BufferSize.validBufSize);
                        this.Manager.Checkpoint("MS-FRS2_R702");
                        this.Manager.Comment("reaching state \'S403\'");
                        this.Manager.Comment("checking step \'return RdcGetFileData/ERROR_SUCCESS\'");
                        this.Manager.Assert((temp23 == FRS2Model.error_status_t.ERROR_SUCCESS), String.Format("expected \'FRS2Model.error_status_t.ERROR_SUCCESS\', actual \'{0}\' (return of RdcGet" +
                                    "FileData, state S403)", TestManagerHelpers.Describe(temp23)));
                        this.Manager.Comment("reaching state \'S415\'");
                        FRS2Model.error_status_t temp24;
                        this.Manager.Comment("executing step \'call RdcClose()\'");
                        temp24 = this.IFRS2ManagedAdapterInstance.RdcClose();
                        this.Manager.Checkpoint("MS-FRS2_R737");
                        this.Manager.Checkpoint("MS-FRS2_R739");
                        this.Manager.Comment("reaching state \'S427\'");
                        this.Manager.Comment("checking step \'return RdcClose/ERROR_SUCCESS\'");
                        this.Manager.Assert((temp24 == FRS2Model.error_status_t.ERROR_SUCCESS), String.Format("expected \'FRS2Model.error_status_t.ERROR_SUCCESS\', actual \'{0}\' (return of RdcClo" +
                                    "se, state S427)", TestManagerHelpers.Describe(temp24)));
                        this.Manager.Comment("reaching state \'S436\'");
                        goto label1;
                    }
                    if ((temp25 == 6)) {
                        TCtestScenario3S206();
                        goto label1;
                    }
                    if ((temp25 == 7)) {
                        TCtestScenario3S207();
                        goto label1;
                    }
                    throw new InvalidOperationException("never reached");
                label1:
;
                    goto label3;
                }
                if ((temp36 == 2)) {
                    this.Manager.Comment("reaching state \'S129\'");
                    FRS2Model.error_status_t temp26;
                    this.Manager.Comment("executing step \'call InitializeFileTransferAsync(1,1,True)\'");
                    temp26 = this.IFRS2ManagedAdapterInstance.InitializeFileTransferAsync(1, 1, true);
                    this.Manager.Checkpoint("MS-FRS2_R774");
                    this.Manager.Checkpoint("MS-FRS2_R777");
                    this.Manager.Checkpoint("MS-FRS2_R793");
                    this.Manager.Comment("reaching state \'S157\'");
                    this.Manager.Comment("checking step \'return InitializeFileTransferAsync/ERROR_SUCCESS\'");
                    this.Manager.Assert((temp26 == FRS2Model.error_status_t.ERROR_SUCCESS), String.Format("expected \'FRS2Model.error_status_t.ERROR_SUCCESS\', actual \'{0}\' (return of Initia" +
                                "lizeFileTransferAsync, state S157)", TestManagerHelpers.Describe(temp26)));
                    this.Manager.Comment("reaching state \'S176\'");
                    int temp34 = this.Manager.ExpectEvent(this.QuiescenceTimeout, true, new ExpectedEvent(TCtestScenario3.InitializeFileTransferAsyncEventInfo, null, new InitializeFileTransferAsyncEventDelegate1(this.TCtestScenario3S0InitializeFileTransferAsyncEventChecker16)), new ExpectedEvent(TCtestScenario3.InitializeFileTransferAsyncEventInfo, null, new InitializeFileTransferAsyncEventDelegate1(this.TCtestScenario3S0InitializeFileTransferAsyncEventChecker17)), new ExpectedEvent(TCtestScenario3.InitializeFileTransferAsyncEventInfo, null, new InitializeFileTransferAsyncEventDelegate1(this.TCtestScenario3S0InitializeFileTransferAsyncEventChecker18)), new ExpectedEvent(TCtestScenario3.InitializeFileTransferAsyncEventInfo, null, new InitializeFileTransferAsyncEventDelegate1(this.TCtestScenario3S0InitializeFileTransferAsyncEventChecker19)), new ExpectedEvent(TCtestScenario3.InitializeFileTransferAsyncEventInfo, null, new InitializeFileTransferAsyncEventDelegate1(this.TCtestScenario3S0InitializeFileTransferAsyncEventChecker20)), new ExpectedEvent(TCtestScenario3.InitializeFileTransferAsyncEventInfo, null, new InitializeFileTransferAsyncEventDelegate1(this.TCtestScenario3S0InitializeFileTransferAsyncEventChecker21)), new ExpectedEvent(TCtestScenario3.InitializeFileTransferAsyncEventInfo, null, new InitializeFileTransferAsyncEventDelegate1(this.TCtestScenario3S0InitializeFileTransferAsyncEventChecker22)), new ExpectedEvent(TCtestScenario3.InitializeFileTransferAsyncEventInfo, null, new InitializeFileTransferAsyncEventDelegate1(this.TCtestScenario3S0InitializeFileTransferAsyncEventChecker23)));
                    if ((temp34 == 0)) {
                        this.Manager.Comment("reaching state \'S208\'");
                        FRS2Model.error_status_t temp27;
                        this.Manager.Comment("executing step \'call RdcGetSignatures(inValid)\'");
                        temp27 = this.IFRS2ManagedAdapterInstance.RdcGetSignatures(FRS2Model.offset.inValid);
                        this.Manager.Checkpoint("MS-FRS2_R647");
                        this.Manager.Checkpoint("MS-FRS2_R658");
                        this.Manager.Checkpoint("MS-FRS2_R661");
                        this.Manager.Checkpoint("MS-FRS2_R805");
                        this.Manager.Comment("reaching state \'S346\'");
                        this.Manager.Comment("checking step \'return RdcGetSignatures/ERROR_FAIL\'");
                        this.Manager.Assert((temp27 == FRS2Model.error_status_t.ERROR_FAIL), String.Format("expected \'FRS2Model.error_status_t.ERROR_FAIL\', actual \'{0}\' (return of RdcGetSig" +
                                    "natures, state S346)", TestManagerHelpers.Describe(temp27)));
                        TCtestScenario3S379();
                        goto label2;
                    }
                    if ((temp34 == 1)) {
                        this.Manager.Comment("reaching state \'S209\'");
                        FRS2Model.error_status_t temp28;
                        this.Manager.Comment("executing step \'call RdcGetSignatures(inValid)\'");
                        temp28 = this.IFRS2ManagedAdapterInstance.RdcGetSignatures(FRS2Model.offset.inValid);
                        this.Manager.Checkpoint("MS-FRS2_R647");
                        this.Manager.Checkpoint("MS-FRS2_R655");
                        this.Manager.Checkpoint("MS-FRS2_R656");
                        this.Manager.Checkpoint("MS-FRS2_R657");
                        this.Manager.Checkpoint("MS-FRS2_R658");
                        this.Manager.Checkpoint("MS-FRS2_R661");
                        this.Manager.Checkpoint("MS-FRS2_R805");
                        this.Manager.Comment("reaching state \'S347\'");
                        this.Manager.Comment("checking step \'return RdcGetSignatures/ERROR_FAIL\'");
                        this.Manager.Assert((temp28 == FRS2Model.error_status_t.ERROR_FAIL), String.Format("expected \'FRS2Model.error_status_t.ERROR_FAIL\', actual \'{0}\' (return of RdcGetSig" +
                                    "natures, state S347)", TestManagerHelpers.Describe(temp28)));
                        TCtestScenario3S380();
                        goto label2;
                    }
                    if ((temp34 == 2)) {
                        TCtestScenario3S210();
                        goto label2;
                    }
                    if ((temp34 == 3)) {
                        TCtestScenario3S211();
                        goto label2;
                    }
                    if ((temp34 == 4)) {
                        this.Manager.Comment("reaching state \'S212\'");
                        FRS2Model.error_status_t temp30;
                        this.Manager.Comment("executing step \'call RdcGetSignatures(valid)\'");
                        temp30 = this.IFRS2ManagedAdapterInstance.RdcGetSignatures(FRS2Model.offset.valid);
                        this.Manager.Checkpoint("MS-FRS2_R646");
                        this.Manager.Checkpoint("MS-FRS2_R649");
                        this.Manager.Checkpoint("MS-FRS2_R650");
                        this.Manager.Comment("reaching state \'S349\'");
                        this.Manager.Comment("checking step \'return RdcGetSignatures/ERROR_SUCCESS\'");
                        this.Manager.Assert((temp30 == FRS2Model.error_status_t.ERROR_SUCCESS), String.Format("expected \'FRS2Model.error_status_t.ERROR_SUCCESS\', actual \'{0}\' (return of RdcGet" +
                                    "Signatures, state S349)", TestManagerHelpers.Describe(temp30)));
                        this.Manager.Comment("reaching state \'S382\'");
                        FRS2Model.error_status_t temp31;
                        this.Manager.Comment("executing step \'call RdcGetFileData(validBufSize)\'");
                        temp31 = this.IFRS2ManagedAdapterInstance.RdcGetFileData(FRS2Model.BufferSize.validBufSize);
                        this.Manager.Checkpoint("MS-FRS2_R702");
                        this.Manager.Comment("reaching state \'S404\'");
                        this.Manager.Comment("checking step \'return RdcGetFileData/ERROR_SUCCESS\'");
                        this.Manager.Assert((temp31 == FRS2Model.error_status_t.ERROR_SUCCESS), String.Format("expected \'FRS2Model.error_status_t.ERROR_SUCCESS\', actual \'{0}\' (return of RdcGet" +
                                    "FileData, state S404)", TestManagerHelpers.Describe(temp31)));
                        this.Manager.Comment("reaching state \'S416\'");
                        FRS2Model.error_status_t temp32;
                        this.Manager.Comment("executing step \'call RdcClose()\'");
                        temp32 = this.IFRS2ManagedAdapterInstance.RdcClose();
                        this.Manager.Checkpoint("MS-FRS2_R737");
                        this.Manager.Checkpoint("MS-FRS2_R739");
                        this.Manager.Comment("reaching state \'S428\'");
                        this.Manager.Comment("checking step \'return RdcClose/ERROR_SUCCESS\'");
                        this.Manager.Assert((temp32 == FRS2Model.error_status_t.ERROR_SUCCESS), String.Format("expected \'FRS2Model.error_status_t.ERROR_SUCCESS\', actual \'{0}\' (return of RdcClo" +
                                    "se, state S428)", TestManagerHelpers.Describe(temp32)));
                        this.Manager.Comment("reaching state \'S437\'");
                        goto label2;
                    }
                    if ((temp34 == 5)) {
                        TCtestScenario3S213();
                        goto label2;
                    }
                    if ((temp34 == 6)) {
                        TCtestScenario3S214();
                        goto label2;
                    }
                    if ((temp34 == 7)) {
                        this.Manager.Comment("reaching state \'S215\'");
                        FRS2Model.error_status_t temp33;
                        this.Manager.Comment("executing step \'call RdcGetSignatures(inValid)\'");
                        temp33 = this.IFRS2ManagedAdapterInstance.RdcGetSignatures(FRS2Model.offset.inValid);
                        this.Manager.Checkpoint("MS-FRS2_R647");
                        this.Manager.Checkpoint("MS-FRS2_R655");
                        this.Manager.Checkpoint("MS-FRS2_R656");
                        this.Manager.Checkpoint("MS-FRS2_R657");
                        this.Manager.Checkpoint("MS-FRS2_R661");
                        this.Manager.Checkpoint("MS-FRS2_R805");
                        this.Manager.Comment("reaching state \'S350\'");
                        this.Manager.Comment("checking step \'return RdcGetSignatures/ERROR_FAIL\'");
                        this.Manager.Assert((temp33 == FRS2Model.error_status_t.ERROR_FAIL), String.Format("expected \'FRS2Model.error_status_t.ERROR_FAIL\', actual \'{0}\' (return of RdcGetSig" +
                                    "natures, state S350)", TestManagerHelpers.Describe(temp33)));
                        TCtestScenario3S383();
                        goto label2;
                    }
                    throw new InvalidOperationException("never reached");
                label2:
;
                    goto label3;
                }
                if ((temp36 == 3)) {
                    this.Manager.Comment("reaching state \'S130\'");
                    FRS2Model.error_status_t temp35;
                    this.Manager.Comment("executing step \'call InitializeFileTransferAsync(1,1,True)\'");
                    temp35 = this.IFRS2ManagedAdapterInstance.InitializeFileTransferAsync(1, 1, true);
                    this.Manager.Checkpoint("MS-FRS2_R774");
                    this.Manager.Checkpoint("MS-FRS2_R777");
                    this.Manager.Checkpoint("MS-FRS2_R793");
                    this.Manager.AddReturn(InitializeFileTransferAsyncInfo, null, temp35);
                    TCtestScenario3S155();
                    goto label3;
                }
                throw new InvalidOperationException("never reached");
            label3:
;
                goto label4;
            }
            if ((temp37 == 1)) {
                TCtestScenario3S96();
                goto label4;
            }
            throw new InvalidOperationException("never reached");
        label4:
;
            this.Manager.EndTest();
        }
        
        private void TCtestScenario3S0AsyncPollResponseEventChecker(FRS2Model.VVGeneration vvGen) {
            this.Manager.Comment("checking step \'event AsyncPollResponseEvent(ValidValue)\'");
            try {
                this.Manager.Assert((vvGen == FRS2Model.VVGeneration.ValidValue), String.Format("expected \'FRS2Model.VVGeneration.ValidValue\', actual \'{0}\' (vvGen of AsyncPollRes" +
                            "ponseEvent, state S87)", TestManagerHelpers.Describe(vvGen)));
            }
            catch (TransactionFailedException ) {
                this.Manager.Comment("This step would have covered MS-FRS2_R556, MS-FRS2_R1020, MS-FRS2_R555");
                throw;
            }
            this.Manager.Checkpoint("MS-FRS2_R556");
            this.Manager.Checkpoint("MS-FRS2_R1020");
            this.Manager.Checkpoint("MS-FRS2_R555");
        }
        
        private void TCtestScenario3S0RequestUpdatesEventChecker(FRS2Model.FilePresense present, FRS2Model.UPDATE_STATUS updateStatus) {
            this.Manager.Comment("checking step \'event RequestUpdatesEvent(fileDeleted,UPDATE_STATUS_MORE)\'");
            try {
                this.Manager.Assert((present == FRS2Model.FilePresense.fileDeleted), String.Format("expected \'FRS2Model.FilePresense.fileDeleted\', actual \'{0}\' (present of RequestUp" +
                            "datesEvent, state S119)", TestManagerHelpers.Describe(present)));
                this.Manager.Assert((updateStatus == FRS2Model.UPDATE_STATUS.UPDATE_STATUS_MORE), String.Format("expected \'FRS2Model.UPDATE_STATUS.UPDATE_STATUS_MORE\', actual \'{0}\' (updateStatus" +
                            " of RequestUpdatesEvent, state S119)", TestManagerHelpers.Describe(updateStatus)));
            }
            catch (TransactionFailedException ) {
                this.Manager.Comment("This step would have covered MS-FRS2_R93");
                throw;
            }
            this.Manager.Checkpoint("MS-FRS2_R93");
        }
        
        private void TCtestScenario3S127() {
            this.Manager.Comment("reaching state \'S127\'");
            FRS2Model.error_status_t temp8;
            this.Manager.Comment("executing step \'call InitializeFileTransferAsync(1,1,True)\'");
            temp8 = this.IFRS2ManagedAdapterInstance.InitializeFileTransferAsync(1, 1, true);
            this.Manager.Checkpoint("MS-FRS2_R774");
            this.Manager.Checkpoint("MS-FRS2_R777");
            this.Manager.Checkpoint("MS-FRS2_R793");
            this.Manager.AddReturn(InitializeFileTransferAsyncInfo, null, temp8);
            TCtestScenario3S155();
        }
        
        private void TCtestScenario3S155() {
            this.Manager.Comment("reaching state \'S155\'");
            this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(TCtestScenario3.InitializeFileTransferAsyncInfo, null, new InitializeFileTransferAsyncDelegate1(this.TCtestScenario3S0InitializeFileTransferAsyncChecker)));
            this.Manager.Comment("reaching state \'S174\'");
            int temp16 = this.Manager.ExpectEvent(this.QuiescenceTimeout, true, new ExpectedEvent(TCtestScenario3.InitializeFileTransferAsyncEventInfo, null, new InitializeFileTransferAsyncEventDelegate1(this.TCtestScenario3S0InitializeFileTransferAsyncEventChecker)), new ExpectedEvent(TCtestScenario3.InitializeFileTransferAsyncEventInfo, null, new InitializeFileTransferAsyncEventDelegate1(this.TCtestScenario3S0InitializeFileTransferAsyncEventChecker1)), new ExpectedEvent(TCtestScenario3.InitializeFileTransferAsyncEventInfo, null, new InitializeFileTransferAsyncEventDelegate1(this.TCtestScenario3S0InitializeFileTransferAsyncEventChecker2)), new ExpectedEvent(TCtestScenario3.InitializeFileTransferAsyncEventInfo, null, new InitializeFileTransferAsyncEventDelegate1(this.TCtestScenario3S0InitializeFileTransferAsyncEventChecker3)), new ExpectedEvent(TCtestScenario3.InitializeFileTransferAsyncEventInfo, null, new InitializeFileTransferAsyncEventDelegate1(this.TCtestScenario3S0InitializeFileTransferAsyncEventChecker4)), new ExpectedEvent(TCtestScenario3.InitializeFileTransferAsyncEventInfo, null, new InitializeFileTransferAsyncEventDelegate1(this.TCtestScenario3S0InitializeFileTransferAsyncEventChecker5)), new ExpectedEvent(TCtestScenario3.InitializeFileTransferAsyncEventInfo, null, new InitializeFileTransferAsyncEventDelegate1(this.TCtestScenario3S0InitializeFileTransferAsyncEventChecker6)), new ExpectedEvent(TCtestScenario3.InitializeFileTransferAsyncEventInfo, null, new InitializeFileTransferAsyncEventDelegate1(this.TCtestScenario3S0InitializeFileTransferAsyncEventChecker7)));
            if ((temp16 == 0)) {
                TCtestScenario3S192();
                goto label0;
            }
            if ((temp16 == 1)) {
                this.Manager.Comment("reaching state \'S193\'");
                FRS2Model.error_status_t temp10;
                this.Manager.Comment("executing step \'call RdcGetSignatures(inValid)\'");
                temp10 = this.IFRS2ManagedAdapterInstance.RdcGetSignatures(FRS2Model.offset.inValid);
                this.Manager.Checkpoint("MS-FRS2_R647");
                this.Manager.Checkpoint("MS-FRS2_R655");
                this.Manager.Checkpoint("MS-FRS2_R656");
                this.Manager.Checkpoint("MS-FRS2_R657");
                this.Manager.Checkpoint("MS-FRS2_R661");
                this.Manager.Checkpoint("MS-FRS2_R805");
                this.Manager.Comment("reaching state \'S337\'");
                this.Manager.Comment("checking step \'return RdcGetSignatures/ERROR_FAIL\'");
                this.Manager.Assert((temp10 == FRS2Model.error_status_t.ERROR_FAIL), String.Format("expected \'FRS2Model.error_status_t.ERROR_FAIL\', actual \'{0}\' (return of RdcGetSig" +
                            "natures, state S337)", TestManagerHelpers.Describe(temp10)));
                TCtestScenario3S370();
                goto label0;
            }
            if ((temp16 == 2)) {
                TCtestScenario3S194();
                goto label0;
            }
            if ((temp16 == 3)) {
                this.Manager.Comment("reaching state \'S195\'");
                FRS2Model.error_status_t temp11;
                this.Manager.Comment("executing step \'call RdcGetSignatures(inValid)\'");
                temp11 = this.IFRS2ManagedAdapterInstance.RdcGetSignatures(FRS2Model.offset.inValid);
                this.Manager.Checkpoint("MS-FRS2_R647");
                this.Manager.Checkpoint("MS-FRS2_R658");
                this.Manager.Checkpoint("MS-FRS2_R661");
                this.Manager.Checkpoint("MS-FRS2_R805");
                this.Manager.Comment("reaching state \'S338\'");
                this.Manager.Comment("checking step \'return RdcGetSignatures/ERROR_FAIL\'");
                this.Manager.Assert((temp11 == FRS2Model.error_status_t.ERROR_FAIL), String.Format("expected \'FRS2Model.error_status_t.ERROR_FAIL\', actual \'{0}\' (return of RdcGetSig" +
                            "natures, state S338)", TestManagerHelpers.Describe(temp11)));
                TCtestScenario3S371();
                goto label0;
            }
            if ((temp16 == 4)) {
                this.Manager.Comment("reaching state \'S196\'");
                FRS2Model.error_status_t temp12;
                this.Manager.Comment("executing step \'call RdcGetSignatures(inValid)\'");
                temp12 = this.IFRS2ManagedAdapterInstance.RdcGetSignatures(FRS2Model.offset.inValid);
                this.Manager.Checkpoint("MS-FRS2_R647");
                this.Manager.Checkpoint("MS-FRS2_R655");
                this.Manager.Checkpoint("MS-FRS2_R656");
                this.Manager.Checkpoint("MS-FRS2_R657");
                this.Manager.Checkpoint("MS-FRS2_R658");
                this.Manager.Checkpoint("MS-FRS2_R661");
                this.Manager.Checkpoint("MS-FRS2_R805");
                this.Manager.Comment("reaching state \'S339\'");
                this.Manager.Comment("checking step \'return RdcGetSignatures/ERROR_FAIL\'");
                this.Manager.Assert((temp12 == FRS2Model.error_status_t.ERROR_FAIL), String.Format("expected \'FRS2Model.error_status_t.ERROR_FAIL\', actual \'{0}\' (return of RdcGetSig" +
                            "natures, state S339)", TestManagerHelpers.Describe(temp12)));
                TCtestScenario3S372();
                goto label0;
            }
            if ((temp16 == 5)) {
                TCtestScenario3S197();
                goto label0;
            }
            if ((temp16 == 6)) {
                this.Manager.Comment("reaching state \'S198\'");
                FRS2Model.error_status_t temp13;
                this.Manager.Comment("executing step \'call RdcGetSignatures(valid)\'");
                temp13 = this.IFRS2ManagedAdapterInstance.RdcGetSignatures(FRS2Model.offset.valid);
                this.Manager.Checkpoint("MS-FRS2_R646");
                this.Manager.Checkpoint("MS-FRS2_R649");
                this.Manager.Checkpoint("MS-FRS2_R650");
                this.Manager.Comment("reaching state \'S340\'");
                this.Manager.Comment("checking step \'return RdcGetSignatures/ERROR_SUCCESS\'");
                this.Manager.Assert((temp13 == FRS2Model.error_status_t.ERROR_SUCCESS), String.Format("expected \'FRS2Model.error_status_t.ERROR_SUCCESS\', actual \'{0}\' (return of RdcGet" +
                            "Signatures, state S340)", TestManagerHelpers.Describe(temp13)));
                this.Manager.Comment("reaching state \'S373\'");
                FRS2Model.error_status_t temp14;
                this.Manager.Comment("executing step \'call RdcGetFileData(validBufSize)\'");
                temp14 = this.IFRS2ManagedAdapterInstance.RdcGetFileData(FRS2Model.BufferSize.validBufSize);
                this.Manager.Checkpoint("MS-FRS2_R702");
                this.Manager.Comment("reaching state \'S402\'");
                this.Manager.Comment("checking step \'return RdcGetFileData/ERROR_SUCCESS\'");
                this.Manager.Assert((temp14 == FRS2Model.error_status_t.ERROR_SUCCESS), String.Format("expected \'FRS2Model.error_status_t.ERROR_SUCCESS\', actual \'{0}\' (return of RdcGet" +
                            "FileData, state S402)", TestManagerHelpers.Describe(temp14)));
                this.Manager.Comment("reaching state \'S414\'");
                FRS2Model.error_status_t temp15;
                this.Manager.Comment("executing step \'call RdcClose()\'");
                temp15 = this.IFRS2ManagedAdapterInstance.RdcClose();
                this.Manager.Checkpoint("MS-FRS2_R737");
                this.Manager.Checkpoint("MS-FRS2_R739");
                this.Manager.Comment("reaching state \'S426\'");
                this.Manager.Comment("checking step \'return RdcClose/ERROR_SUCCESS\'");
                this.Manager.Assert((temp15 == FRS2Model.error_status_t.ERROR_SUCCESS), String.Format("expected \'FRS2Model.error_status_t.ERROR_SUCCESS\', actual \'{0}\' (return of RdcClo" +
                            "se, state S426)", TestManagerHelpers.Describe(temp15)));
                this.Manager.Comment("reaching state \'S435\'");
                goto label0;
            }
            if ((temp16 == 7)) {
                TCtestScenario3S199();
                goto label0;
            }
            throw new InvalidOperationException("never reached");
        label0:
;
        }
        
        private void TCtestScenario3S0InitializeFileTransferAsyncChecker(FRS2Model.error_status_t @return) {
            this.Manager.Comment("checking step \'return InitializeFileTransferAsync/ERROR_SUCCESS\'");
            this.Manager.Assert((@return == FRS2Model.error_status_t.ERROR_SUCCESS), String.Format("expected \'FRS2Model.error_status_t.ERROR_SUCCESS\', actual \'{0}\' (return of Initia" +
                        "lizeFileTransferAsync, state S155)", TestManagerHelpers.Describe(@return)));
        }
        
        private void TCtestScenario3S0InitializeFileTransferAsyncEventChecker(FRS2Model.ServerContext context, FRS2Model.RDC_Sig_Level rdcsigLevel, bool isEOF) {
            this.Manager.Comment("checking step \'event InitializeFileTransferAsyncEvent(ValidContext,forRDCFileLeve" +
                    "l,True)\'");
            this.Manager.Assert((context == FRS2Model.ServerContext.ValidContext), String.Format("expected \'FRS2Model.ServerContext.ValidContext\', actual \'{0}\' (context of Initial" +
                        "izeFileTransferAsyncEvent, state S174)", TestManagerHelpers.Describe(context)));
            this.Manager.Assert((rdcsigLevel == FRS2Model.RDC_Sig_Level.forRDCFileLevel), String.Format("expected \'FRS2Model.RDC_Sig_Level.forRDCFileLevel\', actual \'{0}\' (rdcsigLevel of " +
                        "InitializeFileTransferAsyncEvent, state S174)", TestManagerHelpers.Describe(rdcsigLevel)));
            this.Manager.Assert((isEOF == true), String.Format("expected \'true\', actual \'{0}\' (isEOF of InitializeFileTransferAsyncEvent, state S" +
                        "174)", TestManagerHelpers.Describe(isEOF)));
        }
        
        private void TCtestScenario3S192() {
            this.Manager.Comment("reaching state \'S192\'");
            FRS2Model.error_status_t temp9;
            this.Manager.Comment("executing step \'call RdcGetSignatures(inValid)\'");
            temp9 = this.IFRS2ManagedAdapterInstance.RdcGetSignatures(FRS2Model.offset.inValid);
            this.Manager.Checkpoint("MS-FRS2_R647");
            this.Manager.Checkpoint("MS-FRS2_R661");
            this.Manager.Checkpoint("MS-FRS2_R805");
            this.Manager.Comment("reaching state \'S336\'");
            this.Manager.Comment("checking step \'return RdcGetSignatures/ERROR_FAIL\'");
            this.Manager.Assert((temp9 == FRS2Model.error_status_t.ERROR_FAIL), String.Format("expected \'FRS2Model.error_status_t.ERROR_FAIL\', actual \'{0}\' (return of RdcGetSig" +
                        "natures, state S336)", TestManagerHelpers.Describe(temp9)));
            this.Manager.Comment("reaching state \'S369\'");
        }
        
        private void TCtestScenario3S0InitializeFileTransferAsyncEventChecker1(FRS2Model.ServerContext context, FRS2Model.RDC_Sig_Level rdcsigLevel, bool isEOF) {
            this.Manager.Comment("checking step \'event InitializeFileTransferAsyncEvent(InvalidContext,forRDCFileLe" +
                    "vel,True)\'");
            this.Manager.Assert((context == FRS2Model.ServerContext.InvalidContext), String.Format("expected \'FRS2Model.ServerContext.InvalidContext\', actual \'{0}\' (context of Initi" +
                        "alizeFileTransferAsyncEvent, state S174)", TestManagerHelpers.Describe(context)));
            this.Manager.Assert((rdcsigLevel == FRS2Model.RDC_Sig_Level.forRDCFileLevel), String.Format("expected \'FRS2Model.RDC_Sig_Level.forRDCFileLevel\', actual \'{0}\' (rdcsigLevel of " +
                        "InitializeFileTransferAsyncEvent, state S174)", TestManagerHelpers.Describe(rdcsigLevel)));
            this.Manager.Assert((isEOF == true), String.Format("expected \'true\', actual \'{0}\' (isEOF of InitializeFileTransferAsyncEvent, state S" +
                        "174)", TestManagerHelpers.Describe(isEOF)));
        }
        
        private void TCtestScenario3S370() {
            this.Manager.Comment("reaching state \'S370\'");
        }
        
        private void TCtestScenario3S0InitializeFileTransferAsyncEventChecker2(FRS2Model.ServerContext context, FRS2Model.RDC_Sig_Level rdcsigLevel, bool isEOF) {
            this.Manager.Comment("checking step \'event InitializeFileTransferAsyncEvent(InvalidContext,forRAWFileLe" +
                    "vel,False)\'");
            this.Manager.Assert((context == FRS2Model.ServerContext.InvalidContext), String.Format("expected \'FRS2Model.ServerContext.InvalidContext\', actual \'{0}\' (context of Initi" +
                        "alizeFileTransferAsyncEvent, state S174)", TestManagerHelpers.Describe(context)));
            this.Manager.Assert((rdcsigLevel == FRS2Model.RDC_Sig_Level.forRAWFileLevel), String.Format("expected \'FRS2Model.RDC_Sig_Level.forRAWFileLevel\', actual \'{0}\' (rdcsigLevel of " +
                        "InitializeFileTransferAsyncEvent, state S174)", TestManagerHelpers.Describe(rdcsigLevel)));
            this.Manager.Assert((isEOF == false), String.Format("expected \'false\', actual \'{0}\' (isEOF of InitializeFileTransferAsyncEvent, state " +
                        "S174)", TestManagerHelpers.Describe(isEOF)));
        }
        
        private void TCtestScenario3S194() {
            this.Manager.Comment("reaching state \'S194\'");
        }
        
        private void TCtestScenario3S0InitializeFileTransferAsyncEventChecker3(FRS2Model.ServerContext context, FRS2Model.RDC_Sig_Level rdcsigLevel, bool isEOF) {
            this.Manager.Comment("checking step \'event InitializeFileTransferAsyncEvent(ValidContext,forRAWFileLeve" +
                    "l,True)\'");
            this.Manager.Assert((context == FRS2Model.ServerContext.ValidContext), String.Format("expected \'FRS2Model.ServerContext.ValidContext\', actual \'{0}\' (context of Initial" +
                        "izeFileTransferAsyncEvent, state S174)", TestManagerHelpers.Describe(context)));
            this.Manager.Assert((rdcsigLevel == FRS2Model.RDC_Sig_Level.forRAWFileLevel), String.Format("expected \'FRS2Model.RDC_Sig_Level.forRAWFileLevel\', actual \'{0}\' (rdcsigLevel of " +
                        "InitializeFileTransferAsyncEvent, state S174)", TestManagerHelpers.Describe(rdcsigLevel)));
            this.Manager.Assert((isEOF == true), String.Format("expected \'true\', actual \'{0}\' (isEOF of InitializeFileTransferAsyncEvent, state S" +
                        "174)", TestManagerHelpers.Describe(isEOF)));
        }
        
        private void TCtestScenario3S371() {
            this.Manager.Comment("reaching state \'S371\'");
        }
        
        private void TCtestScenario3S0InitializeFileTransferAsyncEventChecker4(FRS2Model.ServerContext context, FRS2Model.RDC_Sig_Level rdcsigLevel, bool isEOF) {
            this.Manager.Comment("checking step \'event InitializeFileTransferAsyncEvent(InvalidContext,forRAWFileLe" +
                    "vel,True)\'");
            this.Manager.Assert((context == FRS2Model.ServerContext.InvalidContext), String.Format("expected \'FRS2Model.ServerContext.InvalidContext\', actual \'{0}\' (context of Initi" +
                        "alizeFileTransferAsyncEvent, state S174)", TestManagerHelpers.Describe(context)));
            this.Manager.Assert((rdcsigLevel == FRS2Model.RDC_Sig_Level.forRAWFileLevel), String.Format("expected \'FRS2Model.RDC_Sig_Level.forRAWFileLevel\', actual \'{0}\' (rdcsigLevel of " +
                        "InitializeFileTransferAsyncEvent, state S174)", TestManagerHelpers.Describe(rdcsigLevel)));
            this.Manager.Assert((isEOF == true), String.Format("expected \'true\', actual \'{0}\' (isEOF of InitializeFileTransferAsyncEvent, state S" +
                        "174)", TestManagerHelpers.Describe(isEOF)));
        }
        
        private void TCtestScenario3S372() {
            this.Manager.Comment("reaching state \'S372\'");
        }
        
        private void TCtestScenario3S0InitializeFileTransferAsyncEventChecker5(FRS2Model.ServerContext context, FRS2Model.RDC_Sig_Level rdcsigLevel, bool isEOF) {
            this.Manager.Comment("checking step \'event InitializeFileTransferAsyncEvent(ValidContext,forRAWFileLeve" +
                    "l,False)\'");
            this.Manager.Assert((context == FRS2Model.ServerContext.ValidContext), String.Format("expected \'FRS2Model.ServerContext.ValidContext\', actual \'{0}\' (context of Initial" +
                        "izeFileTransferAsyncEvent, state S174)", TestManagerHelpers.Describe(context)));
            this.Manager.Assert((rdcsigLevel == FRS2Model.RDC_Sig_Level.forRAWFileLevel), String.Format("expected \'FRS2Model.RDC_Sig_Level.forRAWFileLevel\', actual \'{0}\' (rdcsigLevel of " +
                        "InitializeFileTransferAsyncEvent, state S174)", TestManagerHelpers.Describe(rdcsigLevel)));
            this.Manager.Assert((isEOF == false), String.Format("expected \'false\', actual \'{0}\' (isEOF of InitializeFileTransferAsyncEvent, state " +
                        "S174)", TestManagerHelpers.Describe(isEOF)));
        }
        
        private void TCtestScenario3S197() {
            this.Manager.Comment("reaching state \'S197\'");
        }
        
        private void TCtestScenario3S0InitializeFileTransferAsyncEventChecker6(FRS2Model.ServerContext context, FRS2Model.RDC_Sig_Level rdcsigLevel, bool isEOF) {
            this.Manager.Comment("checking step \'event InitializeFileTransferAsyncEvent(ValidContext,forRDCFileLeve" +
                    "l,False)\'");
            this.Manager.Assert((context == FRS2Model.ServerContext.ValidContext), String.Format("expected \'FRS2Model.ServerContext.ValidContext\', actual \'{0}\' (context of Initial" +
                        "izeFileTransferAsyncEvent, state S174)", TestManagerHelpers.Describe(context)));
            this.Manager.Assert((rdcsigLevel == FRS2Model.RDC_Sig_Level.forRDCFileLevel), String.Format("expected \'FRS2Model.RDC_Sig_Level.forRDCFileLevel\', actual \'{0}\' (rdcsigLevel of " +
                        "InitializeFileTransferAsyncEvent, state S174)", TestManagerHelpers.Describe(rdcsigLevel)));
            this.Manager.Assert((isEOF == false), String.Format("expected \'false\', actual \'{0}\' (isEOF of InitializeFileTransferAsyncEvent, state " +
                        "S174)", TestManagerHelpers.Describe(isEOF)));
        }
        
        private void TCtestScenario3S0InitializeFileTransferAsyncEventChecker7(FRS2Model.ServerContext context, FRS2Model.RDC_Sig_Level rdcsigLevel, bool isEOF) {
            this.Manager.Comment("checking step \'event InitializeFileTransferAsyncEvent(InvalidContext,forRDCFileLe" +
                    "vel,False)\'");
            this.Manager.Assert((context == FRS2Model.ServerContext.InvalidContext), String.Format("expected \'FRS2Model.ServerContext.InvalidContext\', actual \'{0}\' (context of Initi" +
                        "alizeFileTransferAsyncEvent, state S174)", TestManagerHelpers.Describe(context)));
            this.Manager.Assert((rdcsigLevel == FRS2Model.RDC_Sig_Level.forRDCFileLevel), String.Format("expected \'FRS2Model.RDC_Sig_Level.forRDCFileLevel\', actual \'{0}\' (rdcsigLevel of " +
                        "InitializeFileTransferAsyncEvent, state S174)", TestManagerHelpers.Describe(rdcsigLevel)));
            this.Manager.Assert((isEOF == false), String.Format("expected \'false\', actual \'{0}\' (isEOF of InitializeFileTransferAsyncEvent, state " +
                        "S174)", TestManagerHelpers.Describe(isEOF)));
        }
        
        private void TCtestScenario3S199() {
            this.Manager.Comment("reaching state \'S199\'");
        }
        
        private void TCtestScenario3S0RequestUpdatesEventChecker1(FRS2Model.FilePresense present, FRS2Model.UPDATE_STATUS updateStatus) {
            this.Manager.Comment("checking step \'event RequestUpdatesEvent(fileExists,UPDATE_STATUS_DONE)\'");
            try {
                this.Manager.Assert((present == FRS2Model.FilePresense.fileExists), String.Format("expected \'FRS2Model.FilePresense.fileExists\', actual \'{0}\' (present of RequestUpd" +
                            "atesEvent, state S119)", TestManagerHelpers.Describe(present)));
                this.Manager.Assert((updateStatus == FRS2Model.UPDATE_STATUS.UPDATE_STATUS_DONE), String.Format("expected \'FRS2Model.UPDATE_STATUS.UPDATE_STATUS_DONE\', actual \'{0}\' (updateStatus" +
                            " of RequestUpdatesEvent, state S119)", TestManagerHelpers.Describe(updateStatus)));
            }
            catch (TransactionFailedException ) {
                this.Manager.Comment("This step would have covered MS-FRS2_R93, MS-FRS2_R94");
                throw;
            }
            this.Manager.Checkpoint("MS-FRS2_R93");
            this.Manager.Checkpoint("MS-FRS2_R94");
        }
        
        private void TCtestScenario3S0InitializeFileTransferAsyncEventChecker8(FRS2Model.ServerContext context, FRS2Model.RDC_Sig_Level rdcsigLevel, bool isEOF) {
            this.Manager.Comment("checking step \'event InitializeFileTransferAsyncEvent(ValidContext,forRAWFileLeve" +
                    "l,True)\'");
            this.Manager.Assert((context == FRS2Model.ServerContext.ValidContext), String.Format("expected \'FRS2Model.ServerContext.ValidContext\', actual \'{0}\' (context of Initial" +
                        "izeFileTransferAsyncEvent, state S175)", TestManagerHelpers.Describe(context)));
            this.Manager.Assert((rdcsigLevel == FRS2Model.RDC_Sig_Level.forRAWFileLevel), String.Format("expected \'FRS2Model.RDC_Sig_Level.forRAWFileLevel\', actual \'{0}\' (rdcsigLevel of " +
                        "InitializeFileTransferAsyncEvent, state S175)", TestManagerHelpers.Describe(rdcsigLevel)));
            this.Manager.Assert((isEOF == true), String.Format("expected \'true\', actual \'{0}\' (isEOF of InitializeFileTransferAsyncEvent, state S" +
                        "175)", TestManagerHelpers.Describe(isEOF)));
        }
        
        private void TCtestScenario3S374() {
            this.Manager.Comment("reaching state \'S374\'");
        }
        
        private void TCtestScenario3S0InitializeFileTransferAsyncEventChecker9(FRS2Model.ServerContext context, FRS2Model.RDC_Sig_Level rdcsigLevel, bool isEOF) {
            this.Manager.Comment("checking step \'event InitializeFileTransferAsyncEvent(InvalidContext,forRAWFileLe" +
                    "vel,True)\'");
            this.Manager.Assert((context == FRS2Model.ServerContext.InvalidContext), String.Format("expected \'FRS2Model.ServerContext.InvalidContext\', actual \'{0}\' (context of Initi" +
                        "alizeFileTransferAsyncEvent, state S175)", TestManagerHelpers.Describe(context)));
            this.Manager.Assert((rdcsigLevel == FRS2Model.RDC_Sig_Level.forRAWFileLevel), String.Format("expected \'FRS2Model.RDC_Sig_Level.forRAWFileLevel\', actual \'{0}\' (rdcsigLevel of " +
                        "InitializeFileTransferAsyncEvent, state S175)", TestManagerHelpers.Describe(rdcsigLevel)));
            this.Manager.Assert((isEOF == true), String.Format("expected \'true\', actual \'{0}\' (isEOF of InitializeFileTransferAsyncEvent, state S" +
                        "175)", TestManagerHelpers.Describe(isEOF)));
        }
        
        private void TCtestScenario3S375() {
            this.Manager.Comment("reaching state \'S375\'");
        }
        
        private void TCtestScenario3S0InitializeFileTransferAsyncEventChecker10(FRS2Model.ServerContext context, FRS2Model.RDC_Sig_Level rdcsigLevel, bool isEOF) {
            this.Manager.Comment("checking step \'event InitializeFileTransferAsyncEvent(InvalidContext,forRDCFileLe" +
                    "vel,False)\'");
            this.Manager.Assert((context == FRS2Model.ServerContext.InvalidContext), String.Format("expected \'FRS2Model.ServerContext.InvalidContext\', actual \'{0}\' (context of Initi" +
                        "alizeFileTransferAsyncEvent, state S175)", TestManagerHelpers.Describe(context)));
            this.Manager.Assert((rdcsigLevel == FRS2Model.RDC_Sig_Level.forRDCFileLevel), String.Format("expected \'FRS2Model.RDC_Sig_Level.forRDCFileLevel\', actual \'{0}\' (rdcsigLevel of " +
                        "InitializeFileTransferAsyncEvent, state S175)", TestManagerHelpers.Describe(rdcsigLevel)));
            this.Manager.Assert((isEOF == false), String.Format("expected \'false\', actual \'{0}\' (isEOF of InitializeFileTransferAsyncEvent, state " +
                        "S175)", TestManagerHelpers.Describe(isEOF)));
        }
        
        private void TCtestScenario3S202() {
            this.Manager.Comment("reaching state \'S202\'");
        }
        
        private void TCtestScenario3S0InitializeFileTransferAsyncEventChecker11(FRS2Model.ServerContext context, FRS2Model.RDC_Sig_Level rdcsigLevel, bool isEOF) {
            this.Manager.Comment("checking step \'event InitializeFileTransferAsyncEvent(ValidContext,forRDCFileLeve" +
                    "l,True)\'");
            this.Manager.Assert((context == FRS2Model.ServerContext.ValidContext), String.Format("expected \'FRS2Model.ServerContext.ValidContext\', actual \'{0}\' (context of Initial" +
                        "izeFileTransferAsyncEvent, state S175)", TestManagerHelpers.Describe(context)));
            this.Manager.Assert((rdcsigLevel == FRS2Model.RDC_Sig_Level.forRDCFileLevel), String.Format("expected \'FRS2Model.RDC_Sig_Level.forRDCFileLevel\', actual \'{0}\' (rdcsigLevel of " +
                        "InitializeFileTransferAsyncEvent, state S175)", TestManagerHelpers.Describe(rdcsigLevel)));
            this.Manager.Assert((isEOF == true), String.Format("expected \'true\', actual \'{0}\' (isEOF of InitializeFileTransferAsyncEvent, state S" +
                        "175)", TestManagerHelpers.Describe(isEOF)));
        }
        
        private void TCtestScenario3S203() {
            this.Manager.Comment("reaching state \'S203\'");
            FRS2Model.error_status_t temp20;
            this.Manager.Comment("executing step \'call RdcGetSignatures(inValid)\'");
            temp20 = this.IFRS2ManagedAdapterInstance.RdcGetSignatures(FRS2Model.offset.inValid);
            this.Manager.Checkpoint("MS-FRS2_R647");
            this.Manager.Checkpoint("MS-FRS2_R661");
            this.Manager.Checkpoint("MS-FRS2_R805");
            this.Manager.Comment("reaching state \'S343\'");
            this.Manager.Comment("checking step \'return RdcGetSignatures/ERROR_FAIL\'");
            this.Manager.Assert((temp20 == FRS2Model.error_status_t.ERROR_FAIL), String.Format("expected \'FRS2Model.error_status_t.ERROR_FAIL\', actual \'{0}\' (return of RdcGetSig" +
                        "natures, state S343)", TestManagerHelpers.Describe(temp20)));
            this.Manager.Comment("reaching state \'S376\'");
        }
        
        private void TCtestScenario3S0InitializeFileTransferAsyncEventChecker12(FRS2Model.ServerContext context, FRS2Model.RDC_Sig_Level rdcsigLevel, bool isEOF) {
            this.Manager.Comment("checking step \'event InitializeFileTransferAsyncEvent(InvalidContext,forRDCFileLe" +
                    "vel,True)\'");
            this.Manager.Assert((context == FRS2Model.ServerContext.InvalidContext), String.Format("expected \'FRS2Model.ServerContext.InvalidContext\', actual \'{0}\' (context of Initi" +
                        "alizeFileTransferAsyncEvent, state S175)", TestManagerHelpers.Describe(context)));
            this.Manager.Assert((rdcsigLevel == FRS2Model.RDC_Sig_Level.forRDCFileLevel), String.Format("expected \'FRS2Model.RDC_Sig_Level.forRDCFileLevel\', actual \'{0}\' (rdcsigLevel of " +
                        "InitializeFileTransferAsyncEvent, state S175)", TestManagerHelpers.Describe(rdcsigLevel)));
            this.Manager.Assert((isEOF == true), String.Format("expected \'true\', actual \'{0}\' (isEOF of InitializeFileTransferAsyncEvent, state S" +
                        "175)", TestManagerHelpers.Describe(isEOF)));
        }
        
        private void TCtestScenario3S377() {
            this.Manager.Comment("reaching state \'S377\'");
        }
        
        private void TCtestScenario3S0InitializeFileTransferAsyncEventChecker13(FRS2Model.ServerContext context, FRS2Model.RDC_Sig_Level rdcsigLevel, bool isEOF) {
            this.Manager.Comment("checking step \'event InitializeFileTransferAsyncEvent(ValidContext,forRDCFileLeve" +
                    "l,False)\'");
            this.Manager.Assert((context == FRS2Model.ServerContext.ValidContext), String.Format("expected \'FRS2Model.ServerContext.ValidContext\', actual \'{0}\' (context of Initial" +
                        "izeFileTransferAsyncEvent, state S175)", TestManagerHelpers.Describe(context)));
            this.Manager.Assert((rdcsigLevel == FRS2Model.RDC_Sig_Level.forRDCFileLevel), String.Format("expected \'FRS2Model.RDC_Sig_Level.forRDCFileLevel\', actual \'{0}\' (rdcsigLevel of " +
                        "InitializeFileTransferAsyncEvent, state S175)", TestManagerHelpers.Describe(rdcsigLevel)));
            this.Manager.Assert((isEOF == false), String.Format("expected \'false\', actual \'{0}\' (isEOF of InitializeFileTransferAsyncEvent, state " +
                        "S175)", TestManagerHelpers.Describe(isEOF)));
        }
        
        private void TCtestScenario3S0InitializeFileTransferAsyncEventChecker14(FRS2Model.ServerContext context, FRS2Model.RDC_Sig_Level rdcsigLevel, bool isEOF) {
            this.Manager.Comment("checking step \'event InitializeFileTransferAsyncEvent(InvalidContext,forRAWFileLe" +
                    "vel,False)\'");
            this.Manager.Assert((context == FRS2Model.ServerContext.InvalidContext), String.Format("expected \'FRS2Model.ServerContext.InvalidContext\', actual \'{0}\' (context of Initi" +
                        "alizeFileTransferAsyncEvent, state S175)", TestManagerHelpers.Describe(context)));
            this.Manager.Assert((rdcsigLevel == FRS2Model.RDC_Sig_Level.forRAWFileLevel), String.Format("expected \'FRS2Model.RDC_Sig_Level.forRAWFileLevel\', actual \'{0}\' (rdcsigLevel of " +
                        "InitializeFileTransferAsyncEvent, state S175)", TestManagerHelpers.Describe(rdcsigLevel)));
            this.Manager.Assert((isEOF == false), String.Format("expected \'false\', actual \'{0}\' (isEOF of InitializeFileTransferAsyncEvent, state " +
                        "S175)", TestManagerHelpers.Describe(isEOF)));
        }
        
        private void TCtestScenario3S206() {
            this.Manager.Comment("reaching state \'S206\'");
        }
        
        private void TCtestScenario3S0InitializeFileTransferAsyncEventChecker15(FRS2Model.ServerContext context, FRS2Model.RDC_Sig_Level rdcsigLevel, bool isEOF) {
            this.Manager.Comment("checking step \'event InitializeFileTransferAsyncEvent(ValidContext,forRAWFileLeve" +
                    "l,False)\'");
            this.Manager.Assert((context == FRS2Model.ServerContext.ValidContext), String.Format("expected \'FRS2Model.ServerContext.ValidContext\', actual \'{0}\' (context of Initial" +
                        "izeFileTransferAsyncEvent, state S175)", TestManagerHelpers.Describe(context)));
            this.Manager.Assert((rdcsigLevel == FRS2Model.RDC_Sig_Level.forRAWFileLevel), String.Format("expected \'FRS2Model.RDC_Sig_Level.forRAWFileLevel\', actual \'{0}\' (rdcsigLevel of " +
                        "InitializeFileTransferAsyncEvent, state S175)", TestManagerHelpers.Describe(rdcsigLevel)));
            this.Manager.Assert((isEOF == false), String.Format("expected \'false\', actual \'{0}\' (isEOF of InitializeFileTransferAsyncEvent, state " +
                        "S175)", TestManagerHelpers.Describe(isEOF)));
        }
        
        private void TCtestScenario3S207() {
            this.Manager.Comment("reaching state \'S207\'");
        }
        
        private void TCtestScenario3S0RequestUpdatesEventChecker2(FRS2Model.FilePresense present, FRS2Model.UPDATE_STATUS updateStatus) {
            this.Manager.Comment("checking step \'event RequestUpdatesEvent(fileExists,UPDATE_STATUS_MORE)\'");
            try {
                this.Manager.Assert((present == FRS2Model.FilePresense.fileExists), String.Format("expected \'FRS2Model.FilePresense.fileExists\', actual \'{0}\' (present of RequestUpd" +
                            "atesEvent, state S119)", TestManagerHelpers.Describe(present)));
                this.Manager.Assert((updateStatus == FRS2Model.UPDATE_STATUS.UPDATE_STATUS_MORE), String.Format("expected \'FRS2Model.UPDATE_STATUS.UPDATE_STATUS_MORE\', actual \'{0}\' (updateStatus" +
                            " of RequestUpdatesEvent, state S119)", TestManagerHelpers.Describe(updateStatus)));
            }
            catch (TransactionFailedException ) {
                this.Manager.Comment("This step would have covered MS-FRS2_R93, MS-FRS2_R498");
                throw;
            }
            this.Manager.Checkpoint("MS-FRS2_R93");
            this.Manager.Checkpoint("MS-FRS2_R498");
        }
        
        private void TCtestScenario3S0InitializeFileTransferAsyncEventChecker16(FRS2Model.ServerContext context, FRS2Model.RDC_Sig_Level rdcsigLevel, bool isEOF) {
            this.Manager.Comment("checking step \'event InitializeFileTransferAsyncEvent(ValidContext,forRAWFileLeve" +
                    "l,True)\'");
            this.Manager.Assert((context == FRS2Model.ServerContext.ValidContext), String.Format("expected \'FRS2Model.ServerContext.ValidContext\', actual \'{0}\' (context of Initial" +
                        "izeFileTransferAsyncEvent, state S176)", TestManagerHelpers.Describe(context)));
            this.Manager.Assert((rdcsigLevel == FRS2Model.RDC_Sig_Level.forRAWFileLevel), String.Format("expected \'FRS2Model.RDC_Sig_Level.forRAWFileLevel\', actual \'{0}\' (rdcsigLevel of " +
                        "InitializeFileTransferAsyncEvent, state S176)", TestManagerHelpers.Describe(rdcsigLevel)));
            this.Manager.Assert((isEOF == true), String.Format("expected \'true\', actual \'{0}\' (isEOF of InitializeFileTransferAsyncEvent, state S" +
                        "176)", TestManagerHelpers.Describe(isEOF)));
        }
        
        private void TCtestScenario3S379() {
            this.Manager.Comment("reaching state \'S379\'");
        }
        
        private void TCtestScenario3S0InitializeFileTransferAsyncEventChecker17(FRS2Model.ServerContext context, FRS2Model.RDC_Sig_Level rdcsigLevel, bool isEOF) {
            this.Manager.Comment("checking step \'event InitializeFileTransferAsyncEvent(InvalidContext,forRAWFileLe" +
                    "vel,True)\'");
            this.Manager.Assert((context == FRS2Model.ServerContext.InvalidContext), String.Format("expected \'FRS2Model.ServerContext.InvalidContext\', actual \'{0}\' (context of Initi" +
                        "alizeFileTransferAsyncEvent, state S176)", TestManagerHelpers.Describe(context)));
            this.Manager.Assert((rdcsigLevel == FRS2Model.RDC_Sig_Level.forRAWFileLevel), String.Format("expected \'FRS2Model.RDC_Sig_Level.forRAWFileLevel\', actual \'{0}\' (rdcsigLevel of " +
                        "InitializeFileTransferAsyncEvent, state S176)", TestManagerHelpers.Describe(rdcsigLevel)));
            this.Manager.Assert((isEOF == true), String.Format("expected \'true\', actual \'{0}\' (isEOF of InitializeFileTransferAsyncEvent, state S" +
                        "176)", TestManagerHelpers.Describe(isEOF)));
        }
        
        private void TCtestScenario3S380() {
            this.Manager.Comment("reaching state \'S380\'");
        }
        
        private void TCtestScenario3S0InitializeFileTransferAsyncEventChecker18(FRS2Model.ServerContext context, FRS2Model.RDC_Sig_Level rdcsigLevel, bool isEOF) {
            this.Manager.Comment("checking step \'event InitializeFileTransferAsyncEvent(InvalidContext,forRDCFileLe" +
                    "vel,False)\'");
            this.Manager.Assert((context == FRS2Model.ServerContext.InvalidContext), String.Format("expected \'FRS2Model.ServerContext.InvalidContext\', actual \'{0}\' (context of Initi" +
                        "alizeFileTransferAsyncEvent, state S176)", TestManagerHelpers.Describe(context)));
            this.Manager.Assert((rdcsigLevel == FRS2Model.RDC_Sig_Level.forRDCFileLevel), String.Format("expected \'FRS2Model.RDC_Sig_Level.forRDCFileLevel\', actual \'{0}\' (rdcsigLevel of " +
                        "InitializeFileTransferAsyncEvent, state S176)", TestManagerHelpers.Describe(rdcsigLevel)));
            this.Manager.Assert((isEOF == false), String.Format("expected \'false\', actual \'{0}\' (isEOF of InitializeFileTransferAsyncEvent, state " +
                        "S176)", TestManagerHelpers.Describe(isEOF)));
        }
        
        private void TCtestScenario3S210() {
            this.Manager.Comment("reaching state \'S210\'");
        }
        
        private void TCtestScenario3S0InitializeFileTransferAsyncEventChecker19(FRS2Model.ServerContext context, FRS2Model.RDC_Sig_Level rdcsigLevel, bool isEOF) {
            this.Manager.Comment("checking step \'event InitializeFileTransferAsyncEvent(ValidContext,forRDCFileLeve" +
                    "l,True)\'");
            this.Manager.Assert((context == FRS2Model.ServerContext.ValidContext), String.Format("expected \'FRS2Model.ServerContext.ValidContext\', actual \'{0}\' (context of Initial" +
                        "izeFileTransferAsyncEvent, state S176)", TestManagerHelpers.Describe(context)));
            this.Manager.Assert((rdcsigLevel == FRS2Model.RDC_Sig_Level.forRDCFileLevel), String.Format("expected \'FRS2Model.RDC_Sig_Level.forRDCFileLevel\', actual \'{0}\' (rdcsigLevel of " +
                        "InitializeFileTransferAsyncEvent, state S176)", TestManagerHelpers.Describe(rdcsigLevel)));
            this.Manager.Assert((isEOF == true), String.Format("expected \'true\', actual \'{0}\' (isEOF of InitializeFileTransferAsyncEvent, state S" +
                        "176)", TestManagerHelpers.Describe(isEOF)));
        }
        
        private void TCtestScenario3S211() {
            this.Manager.Comment("reaching state \'S211\'");
            FRS2Model.error_status_t temp29;
            this.Manager.Comment("executing step \'call RdcGetSignatures(inValid)\'");
            temp29 = this.IFRS2ManagedAdapterInstance.RdcGetSignatures(FRS2Model.offset.inValid);
            this.Manager.Checkpoint("MS-FRS2_R647");
            this.Manager.Checkpoint("MS-FRS2_R661");
            this.Manager.Checkpoint("MS-FRS2_R805");
            this.Manager.Comment("reaching state \'S348\'");
            this.Manager.Comment("checking step \'return RdcGetSignatures/ERROR_FAIL\'");
            this.Manager.Assert((temp29 == FRS2Model.error_status_t.ERROR_FAIL), String.Format("expected \'FRS2Model.error_status_t.ERROR_FAIL\', actual \'{0}\' (return of RdcGetSig" +
                        "natures, state S348)", TestManagerHelpers.Describe(temp29)));
            this.Manager.Comment("reaching state \'S381\'");
        }
        
        private void TCtestScenario3S0InitializeFileTransferAsyncEventChecker20(FRS2Model.ServerContext context, FRS2Model.RDC_Sig_Level rdcsigLevel, bool isEOF) {
            this.Manager.Comment("checking step \'event InitializeFileTransferAsyncEvent(ValidContext,forRDCFileLeve" +
                    "l,False)\'");
            this.Manager.Assert((context == FRS2Model.ServerContext.ValidContext), String.Format("expected \'FRS2Model.ServerContext.ValidContext\', actual \'{0}\' (context of Initial" +
                        "izeFileTransferAsyncEvent, state S176)", TestManagerHelpers.Describe(context)));
            this.Manager.Assert((rdcsigLevel == FRS2Model.RDC_Sig_Level.forRDCFileLevel), String.Format("expected \'FRS2Model.RDC_Sig_Level.forRDCFileLevel\', actual \'{0}\' (rdcsigLevel of " +
                        "InitializeFileTransferAsyncEvent, state S176)", TestManagerHelpers.Describe(rdcsigLevel)));
            this.Manager.Assert((isEOF == false), String.Format("expected \'false\', actual \'{0}\' (isEOF of InitializeFileTransferAsyncEvent, state " +
                        "S176)", TestManagerHelpers.Describe(isEOF)));
        }
        
        private void TCtestScenario3S0InitializeFileTransferAsyncEventChecker21(FRS2Model.ServerContext context, FRS2Model.RDC_Sig_Level rdcsigLevel, bool isEOF) {
            this.Manager.Comment("checking step \'event InitializeFileTransferAsyncEvent(InvalidContext,forRAWFileLe" +
                    "vel,False)\'");
            this.Manager.Assert((context == FRS2Model.ServerContext.InvalidContext), String.Format("expected \'FRS2Model.ServerContext.InvalidContext\', actual \'{0}\' (context of Initi" +
                        "alizeFileTransferAsyncEvent, state S176)", TestManagerHelpers.Describe(context)));
            this.Manager.Assert((rdcsigLevel == FRS2Model.RDC_Sig_Level.forRAWFileLevel), String.Format("expected \'FRS2Model.RDC_Sig_Level.forRAWFileLevel\', actual \'{0}\' (rdcsigLevel of " +
                        "InitializeFileTransferAsyncEvent, state S176)", TestManagerHelpers.Describe(rdcsigLevel)));
            this.Manager.Assert((isEOF == false), String.Format("expected \'false\', actual \'{0}\' (isEOF of InitializeFileTransferAsyncEvent, state " +
                        "S176)", TestManagerHelpers.Describe(isEOF)));
        }
        
        private void TCtestScenario3S213() {
            this.Manager.Comment("reaching state \'S213\'");
        }
        
        private void TCtestScenario3S0InitializeFileTransferAsyncEventChecker22(FRS2Model.ServerContext context, FRS2Model.RDC_Sig_Level rdcsigLevel, bool isEOF) {
            this.Manager.Comment("checking step \'event InitializeFileTransferAsyncEvent(ValidContext,forRAWFileLeve" +
                    "l,False)\'");
            this.Manager.Assert((context == FRS2Model.ServerContext.ValidContext), String.Format("expected \'FRS2Model.ServerContext.ValidContext\', actual \'{0}\' (context of Initial" +
                        "izeFileTransferAsyncEvent, state S176)", TestManagerHelpers.Describe(context)));
            this.Manager.Assert((rdcsigLevel == FRS2Model.RDC_Sig_Level.forRAWFileLevel), String.Format("expected \'FRS2Model.RDC_Sig_Level.forRAWFileLevel\', actual \'{0}\' (rdcsigLevel of " +
                        "InitializeFileTransferAsyncEvent, state S176)", TestManagerHelpers.Describe(rdcsigLevel)));
            this.Manager.Assert((isEOF == false), String.Format("expected \'false\', actual \'{0}\' (isEOF of InitializeFileTransferAsyncEvent, state " +
                        "S176)", TestManagerHelpers.Describe(isEOF)));
        }
        
        private void TCtestScenario3S214() {
            this.Manager.Comment("reaching state \'S214\'");
        }
        
        private void TCtestScenario3S0InitializeFileTransferAsyncEventChecker23(FRS2Model.ServerContext context, FRS2Model.RDC_Sig_Level rdcsigLevel, bool isEOF) {
            this.Manager.Comment("checking step \'event InitializeFileTransferAsyncEvent(InvalidContext,forRDCFileLe" +
                    "vel,True)\'");
            this.Manager.Assert((context == FRS2Model.ServerContext.InvalidContext), String.Format("expected \'FRS2Model.ServerContext.InvalidContext\', actual \'{0}\' (context of Initi" +
                        "alizeFileTransferAsyncEvent, state S176)", TestManagerHelpers.Describe(context)));
            this.Manager.Assert((rdcsigLevel == FRS2Model.RDC_Sig_Level.forRDCFileLevel), String.Format("expected \'FRS2Model.RDC_Sig_Level.forRDCFileLevel\', actual \'{0}\' (rdcsigLevel of " +
                        "InitializeFileTransferAsyncEvent, state S176)", TestManagerHelpers.Describe(rdcsigLevel)));
            this.Manager.Assert((isEOF == true), String.Format("expected \'true\', actual \'{0}\' (isEOF of InitializeFileTransferAsyncEvent, state S" +
                        "176)", TestManagerHelpers.Describe(isEOF)));
        }
        
        private void TCtestScenario3S383() {
            this.Manager.Comment("reaching state \'S383\'");
        }
        
        private void TCtestScenario3S0RequestUpdatesEventChecker3(FRS2Model.FilePresense present, FRS2Model.UPDATE_STATUS updateStatus) {
            this.Manager.Comment("checking step \'event RequestUpdatesEvent(fileDeleted,UPDATE_STATUS_DONE)\'");
            try {
                this.Manager.Assert((present == FRS2Model.FilePresense.fileDeleted), String.Format("expected \'FRS2Model.FilePresense.fileDeleted\', actual \'{0}\' (present of RequestUp" +
                            "datesEvent, state S119)", TestManagerHelpers.Describe(present)));
                this.Manager.Assert((updateStatus == FRS2Model.UPDATE_STATUS.UPDATE_STATUS_DONE), String.Format("expected \'FRS2Model.UPDATE_STATUS.UPDATE_STATUS_DONE\', actual \'{0}\' (updateStatus" +
                            " of RequestUpdatesEvent, state S119)", TestManagerHelpers.Describe(updateStatus)));
            }
            catch (TransactionFailedException ) {
                this.Manager.Comment("This step would have covered MS-FRS2_R93, MS-FRS2_R94");
                throw;
            }
            this.Manager.Checkpoint("MS-FRS2_R93");
            this.Manager.Checkpoint("MS-FRS2_R94");
        }
        
        private void TCtestScenario3S0AsyncPollResponseEventChecker1(FRS2Model.VVGeneration vvGen) {
            this.Manager.Comment("checking step \'event AsyncPollResponseEvent(InvalidValue)\'");
            try {
                this.Manager.Assert((vvGen == FRS2Model.VVGeneration.InvalidValue), String.Format("expected \'FRS2Model.VVGeneration.InvalidValue\', actual \'{0}\' (vvGen of AsyncPollR" +
                            "esponseEvent, state S87)", TestManagerHelpers.Describe(vvGen)));
            }
            catch (TransactionFailedException ) {
                this.Manager.Comment("This step would have covered MS-FRS2_R556, MS-FRS2_R1020, MS-FRS2_R555");
                throw;
            }
            this.Manager.Checkpoint("MS-FRS2_R556");
            this.Manager.Checkpoint("MS-FRS2_R1020");
            this.Manager.Checkpoint("MS-FRS2_R555");
        }
        
        private void TCtestScenario3S96() {
            this.Manager.Comment("reaching state \'S96\'");
        }
        #endregion
        
        #region Test Starting in S2
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        [Microsoft.VisualStudio.TestTools.UnitTesting.DescriptionAttribute("MS-FRS2_R37, MS-FRS2_R436, MS-FRS2_R439, MS-FRS2_R455, MS-FRS2_R458, MS-FRS2_R448" +
            ", MS-FRS2_R549, MS-FRS2_R552, MS-FRS2_R517, MS-FRS2_R520, MS-FRS2_R466, MS-FRS2_" +
            "R556, MS-FRS2_R1020, MS-FRS2_R555, MS-FRS2_R486, MS-FRS2_R489, MS-FRS2_R93, MS-F" +
            "RS2_R498, MS-FRS2_R774, MS-FRS2_R777, MS-FRS2_R793, MS-FRS2_R647, MS-FRS2_R655, " +
            "MS-FRS2_R656, MS-FRS2_R657, MS-FRS2_R805, MS-FRS2_R646, MS-FRS2_R649, MS-FRS2_R6" +
            "50, MS-FRS2_R671, MS-FRS2_R674, MS-FRS2_R693, MS-FRS2_R694, MS-FRS2_R683, MS-FRS" +
            "2_R691, MS-FRS2_R692, MS-FRS2_R675, MS-FRS2_R647, MS-FRS2_R655, MS-FRS2_R656, MS" +
            "-FRS2_R657, MS-FRS2_R658, MS-FRS2_R805, MS-FRS2_R647, MS-FRS2_R658, MS-FRS2_R805" +
            ", MS-FRS2_R93, MS-FRS2_R94, MS-FRS2_R774, MS-FRS2_R777, MS-FRS2_R793, MS-FRS2_R6" +
            "46, MS-FRS2_R649, MS-FRS2_R650, MS-FRS2_R671, MS-FRS2_R674, MS-FRS2_R693, MS-FRS" +
            "2_R694, MS-FRS2_R683, MS-FRS2_R691, MS-FRS2_R692, MS-FRS2_R675, MS-FRS2_R647, MS" +
            "-FRS2_R655, MS-FRS2_R656, MS-FRS2_R657, MS-FRS2_R658, MS-FRS2_R805, MS-FRS2_R647" +
            ", MS-FRS2_R658, MS-FRS2_R805, MS-FRS2_R647, MS-FRS2_R655, MS-FRS2_R656, MS-FRS2_" +
            "R657, MS-FRS2_R805, MS-FRS2_R93, MS-FRS2_R94, MS-FRS2_R774, MS-FRS2_R777, MS-FRS" +
            "2_R793, MS-FRS2_R646, MS-FRS2_R649, MS-FRS2_R650, MS-FRS2_R671, MS-FRS2_R674, MS" +
            "-FRS2_R693, MS-FRS2_R694, MS-FRS2_R683, MS-FRS2_R691, MS-FRS2_R692, MS-FRS2_R675" +
            ", MS-FRS2_R647, MS-FRS2_R655, MS-FRS2_R656, MS-FRS2_R657, MS-FRS2_R805, MS-FRS2_" +
            "R647, MS-FRS2_R655, MS-FRS2_R656, MS-FRS2_R657, MS-FRS2_R658, MS-FRS2_R805, MS-F" +
            "RS2_R647, MS-FRS2_R658, MS-FRS2_R805, MS-FRS2_R93, MS-FRS2_R556, MS-FRS2_R1020, " +
            "MS-FRS2_R555")]
        public virtual void FRS2_TCtestScenario3S2() {
            this.Manager.BeginTest("TCtestScenario3S2");
            this.Manager.Comment("reaching state \'S2\'");
            FRS2Model.error_status_t temp38;
            this.Manager.Comment(@"executing step 'call Initialization(Windows7,Map{1=>enabled,2=>enabled,3=>enabled,4=>disabled},Map{1=>exists,2=>exists,3=>exists,4=>exists},Map{""A""=>1,""B""=>2,""C""=>3,""D""=>4},Map{""A""=>sysvol,""B""=>protection,""C""=>sysvol,""D""=>sysvol},Map{1=>""A"",2=>""B"",3=>""C"",4=>""D""},Map{1=>writeOnly,2=>readWrite,3=>readOnly,4=>readWrite},Map{1=>enabled,2=>disabled,3=>enabled,4=>enabled})'");
            temp38 = this.IFRS2ManagedAdapterInstance.Initialization(FRS2Model.OSVersion.Windows7, new Microsoft.Modeling.Map<System.Int32,FRS2Model.connectionProperty>().Add(1, FRS2Model.connectionProperty.enabled).Add(2, FRS2Model.connectionProperty.enabled).Add(3, FRS2Model.connectionProperty.enabled).Add(4, FRS2Model.connectionProperty.disabled), new Microsoft.Modeling.Map<System.Int32,FRS2Model.FromServer>().Add(1, FRS2Model.FromServer.exists).Add(2, FRS2Model.FromServer.exists).Add(3, FRS2Model.FromServer.exists).Add(4, FRS2Model.FromServer.exists), new Microsoft.Modeling.Map<System.String,System.Int32>().Add("A", 1).Add("B", 2).Add("C", 3).Add("D", 4), new Microsoft.Modeling.Map<System.String,FRS2Model.ReplicationGroupTypes>().Add("A", FRS2Model.ReplicationGroupTypes.sysvol).Add("B", FRS2Model.ReplicationGroupTypes.protection).Add("C", FRS2Model.ReplicationGroupTypes.sysvol).Add("D", FRS2Model.ReplicationGroupTypes.sysvol), new Microsoft.Modeling.Map<System.Int32,System.String>().Add(1, "A").Add(2, "B").Add(3, "C").Add(4, "D"), new Microsoft.Modeling.Map<System.Int32,FRS2Model.accessLevels>().Add(1, FRS2Model.accessLevels.writeOnly).Add(2, FRS2Model.accessLevels.readWrite).Add(3, FRS2Model.accessLevels.readOnly).Add(4, FRS2Model.accessLevels.readWrite), new Microsoft.Modeling.Map<System.Int32,FRS2Model.connectionProperty>().Add(1, FRS2Model.connectionProperty.enabled).Add(2, FRS2Model.connectionProperty.disabled).Add(3, FRS2Model.connectionProperty.enabled).Add(4, FRS2Model.connectionProperty.enabled));
            this.Manager.Comment("reaching state \'S3\'");
            this.Manager.Comment("checking step \'return Initialization/ERROR_SUCCESS\'");
            this.Manager.Assert((temp38 == FRS2Model.error_status_t.ERROR_SUCCESS), String.Format("expected \'FRS2Model.error_status_t.ERROR_SUCCESS\', actual \'{0}\' (return of Initia" +
                        "lization, state S3)", TestManagerHelpers.Describe(temp38)));
            this.Manager.Comment("reaching state \'S19\'");
            FRS2Model.ProtocolVersionReturned temp39;
            FRS2Model.UpstreamFlagValueReturned temp40;
            FRS2Model.error_status_t temp41;
            this.Manager.Comment("executing step \'call EstablishConnection(\"A\",1,FRS_COMMUNICATION_PROTOCOL_VERSION" +
                    "_LONGHORN_SERVER,out _,out _)\'");
            temp41 = this.IFRS2ManagedAdapterInstance.EstablishConnection("A", 1, FRS2Model.ProtocolVersion.FRS_COMMUNICATION_PROTOCOL_VERSION_LONGHORN_SERVER, out temp39, out temp40);
            this.Manager.Checkpoint("MS-FRS2_R37");
            this.Manager.Checkpoint("MS-FRS2_R436");
            this.Manager.Checkpoint("MS-FRS2_R439");
            this.Manager.Comment("reaching state \'S28\'");
            this.Manager.Comment("checking step \'return EstablishConnection/[out Valid,out Valid]:ERROR_SUCCESS\'");
            this.Manager.Assert((temp39 == FRS2Model.ProtocolVersionReturned.Valid), String.Format("expected \'FRS2Model.ProtocolVersionReturned.Valid\', actual \'{0}\' (upstreamProtoco" +
                        "lVersion of EstablishConnection, state S28)", TestManagerHelpers.Describe(temp39)));
            this.Manager.Assert((temp40 == FRS2Model.UpstreamFlagValueReturned.Valid), String.Format("expected \'FRS2Model.UpstreamFlagValueReturned.Valid\', actual \'{0}\' (upstreamFlags" +
                        " of EstablishConnection, state S28)", TestManagerHelpers.Describe(temp40)));
            this.Manager.Assert((temp41 == FRS2Model.error_status_t.ERROR_SUCCESS), String.Format("expected \'FRS2Model.error_status_t.ERROR_SUCCESS\', actual \'{0}\' (return of Establ" +
                        "ishConnection, state S28)", TestManagerHelpers.Describe(temp41)));
            this.Manager.Comment("reaching state \'S37\'");
            FRS2Model.error_status_t temp42;
            this.Manager.Comment("executing step \'call EstablishSession(1,1)\'");
            temp42 = this.IFRS2ManagedAdapterInstance.EstablishSession(1, 1);
            this.Manager.Checkpoint("MS-FRS2_R455");
            this.Manager.Checkpoint("MS-FRS2_R458");
            this.Manager.Checkpoint("MS-FRS2_R448");
            this.Manager.Comment("reaching state \'S46\'");
            this.Manager.Comment("checking step \'return EstablishSession/ERROR_SUCCESS\'");
            this.Manager.Assert((temp42 == FRS2Model.error_status_t.ERROR_SUCCESS), String.Format("expected \'FRS2Model.error_status_t.ERROR_SUCCESS\', actual \'{0}\' (return of Establ" +
                        "ishSession, state S46)", TestManagerHelpers.Describe(temp42)));
            this.Manager.Comment("reaching state \'S55\'");
            FRS2Model.error_status_t temp43;
            this.Manager.Comment("executing step \'call AsyncPoll(1)\'");
            temp43 = this.IFRS2ManagedAdapterInstance.AsyncPoll(1);
            this.Manager.Checkpoint("MS-FRS2_R549");
            this.Manager.Checkpoint("MS-FRS2_R552");
            this.Manager.Comment("reaching state \'S64\'");
            this.Manager.Comment("checking step \'return AsyncPoll/ERROR_SUCCESS\'");
            this.Manager.Assert((temp43 == FRS2Model.error_status_t.ERROR_SUCCESS), String.Format("expected \'FRS2Model.error_status_t.ERROR_SUCCESS\', actual \'{0}\' (return of AsyncP" +
                        "oll, state S64)", TestManagerHelpers.Describe(temp43)));
            this.Manager.Comment("reaching state \'S72\'");
            FRS2Model.error_status_t temp44;
            this.Manager.Comment("executing step \'call RequestVersionVector(1,1,1,REQUEST_NORMAL_SYNC,CHANGE_ALL,Va" +
                    "lidValue)\'");
            temp44 = this.IFRS2ManagedAdapterInstance.RequestVersionVector(1, 1, 1, FRS2Model.VERSION_REQUEST_TYPE.REQUEST_NORMAL_SYNC, FRS2Model.VERSION_CHANGE_TYPE.CHANGE_ALL, FRS2Model.VVGeneration.ValidValue);
            this.Manager.Checkpoint("MS-FRS2_R517");
            this.Manager.Checkpoint("MS-FRS2_R520");
            this.Manager.Checkpoint("MS-FRS2_R466");
            this.Manager.Comment("reaching state \'S80\'");
            this.Manager.Comment("checking step \'return RequestVersionVector/ERROR_SUCCESS\'");
            this.Manager.Assert((temp44 == FRS2Model.error_status_t.ERROR_SUCCESS), String.Format("expected \'FRS2Model.error_status_t.ERROR_SUCCESS\', actual \'{0}\' (return of Reques" +
                        "tVersionVector, state S80)", TestManagerHelpers.Describe(temp44)));
            this.Manager.Comment("reaching state \'S88\'");
            int temp71 = this.Manager.ExpectEvent(this.QuiescenceTimeout, true, new ExpectedEvent(TCtestScenario3.AsyncPollResponseEventInfo, null, new AsyncPollResponseEventDelegate1(this.TCtestScenario3S2AsyncPollResponseEventChecker)), new ExpectedEvent(TCtestScenario3.AsyncPollResponseEventInfo, null, new AsyncPollResponseEventDelegate1(this.TCtestScenario3S2AsyncPollResponseEventChecker1)));
            if ((temp71 == 0)) {
                this.Manager.Comment("reaching state \'S97\'");
                FRS2Model.error_status_t temp45;
                this.Manager.Comment("executing step \'call RequestUpdates(1,1,valid)\'");
                temp45 = this.IFRS2ManagedAdapterInstance.RequestUpdates(1, 1, FRS2Model.versionVectorDiff.valid);
                this.Manager.Checkpoint("MS-FRS2_R486");
                this.Manager.Checkpoint("MS-FRS2_R489");
                this.Manager.Comment("reaching state \'S112\'");
                this.Manager.Comment("checking step \'return RequestUpdates/ERROR_SUCCESS\'");
                this.Manager.Assert((temp45 == FRS2Model.error_status_t.ERROR_SUCCESS), String.Format("expected \'FRS2Model.error_status_t.ERROR_SUCCESS\', actual \'{0}\' (return of Reques" +
                            "tUpdates, state S112)", TestManagerHelpers.Describe(temp45)));
                this.Manager.Comment("reaching state \'S120\'");
                int temp70 = this.Manager.ExpectEvent(this.QuiescenceTimeout, true, new ExpectedEvent(TCtestScenario3.RequestUpdatesEventInfo, null, new RequestUpdatesEventDelegate1(this.TCtestScenario3S2RequestUpdatesEventChecker)), new ExpectedEvent(TCtestScenario3.RequestUpdatesEventInfo, null, new RequestUpdatesEventDelegate1(this.TCtestScenario3S2RequestUpdatesEventChecker1)), new ExpectedEvent(TCtestScenario3.RequestUpdatesEventInfo, null, new RequestUpdatesEventDelegate1(this.TCtestScenario3S2RequestUpdatesEventChecker2)), new ExpectedEvent(TCtestScenario3.RequestUpdatesEventInfo, null, new RequestUpdatesEventDelegate1(this.TCtestScenario3S2RequestUpdatesEventChecker3)));
                if ((temp70 == 0)) {
                    this.Manager.Comment("reaching state \'S131\'");
                    FRS2Model.error_status_t temp46;
                    this.Manager.Comment("executing step \'call InitializeFileTransferAsync(1,1,True)\'");
                    temp46 = this.IFRS2ManagedAdapterInstance.InitializeFileTransferAsync(1, 1, true);
                    this.Manager.Checkpoint("MS-FRS2_R774");
                    this.Manager.Checkpoint("MS-FRS2_R777");
                    this.Manager.Checkpoint("MS-FRS2_R793");
                    this.Manager.Comment("reaching state \'S159\'");
                    this.Manager.Comment("checking step \'return InitializeFileTransferAsync/ERROR_SUCCESS\'");
                    this.Manager.Assert((temp46 == FRS2Model.error_status_t.ERROR_SUCCESS), String.Format("expected \'FRS2Model.error_status_t.ERROR_SUCCESS\', actual \'{0}\' (return of Initia" +
                                "lizeFileTransferAsync, state S159)", TestManagerHelpers.Describe(temp46)));
                    this.Manager.Comment("reaching state \'S177\'");
                    int temp53 = this.Manager.ExpectEvent(this.QuiescenceTimeout, true, new ExpectedEvent(TCtestScenario3.InitializeFileTransferAsyncEventInfo, null, new InitializeFileTransferAsyncEventDelegate1(this.TCtestScenario3S2InitializeFileTransferAsyncEventChecker)), new ExpectedEvent(TCtestScenario3.InitializeFileTransferAsyncEventInfo, null, new InitializeFileTransferAsyncEventDelegate1(this.TCtestScenario3S2InitializeFileTransferAsyncEventChecker1)), new ExpectedEvent(TCtestScenario3.InitializeFileTransferAsyncEventInfo, null, new InitializeFileTransferAsyncEventDelegate1(this.TCtestScenario3S2InitializeFileTransferAsyncEventChecker2)), new ExpectedEvent(TCtestScenario3.InitializeFileTransferAsyncEventInfo, null, new InitializeFileTransferAsyncEventDelegate1(this.TCtestScenario3S2InitializeFileTransferAsyncEventChecker3)), new ExpectedEvent(TCtestScenario3.InitializeFileTransferAsyncEventInfo, null, new InitializeFileTransferAsyncEventDelegate1(this.TCtestScenario3S2InitializeFileTransferAsyncEventChecker4)), new ExpectedEvent(TCtestScenario3.InitializeFileTransferAsyncEventInfo, null, new InitializeFileTransferAsyncEventDelegate1(this.TCtestScenario3S2InitializeFileTransferAsyncEventChecker5)), new ExpectedEvent(TCtestScenario3.InitializeFileTransferAsyncEventInfo, null, new InitializeFileTransferAsyncEventDelegate1(this.TCtestScenario3S2InitializeFileTransferAsyncEventChecker6)), new ExpectedEvent(TCtestScenario3.InitializeFileTransferAsyncEventInfo, null, new InitializeFileTransferAsyncEventDelegate1(this.TCtestScenario3S2InitializeFileTransferAsyncEventChecker7)));
                    if ((temp53 == 0)) {
                        TCtestScenario3S216();
                        goto label5;
                    }
                    if ((temp53 == 1)) {
                        TCtestScenario3S214();
                        goto label5;
                    }
                    if ((temp53 == 2)) {
                        TCtestScenario3S213();
                        goto label5;
                    }
                    if ((temp53 == 3)) {
                        this.Manager.Comment("reaching state \'S219\'");
                        FRS2Model.error_status_t temp48;
                        this.Manager.Comment("executing step \'call RdcGetSignatures(valid)\'");
                        temp48 = this.IFRS2ManagedAdapterInstance.RdcGetSignatures(FRS2Model.offset.valid);
                        this.Manager.Checkpoint("MS-FRS2_R646");
                        this.Manager.Checkpoint("MS-FRS2_R649");
                        this.Manager.Checkpoint("MS-FRS2_R650");
                        this.Manager.Comment("reaching state \'S352\'");
                        this.Manager.Comment("checking step \'return RdcGetSignatures/ERROR_SUCCESS\'");
                        this.Manager.Assert((temp48 == FRS2Model.error_status_t.ERROR_SUCCESS), String.Format("expected \'FRS2Model.error_status_t.ERROR_SUCCESS\', actual \'{0}\' (return of RdcGet" +
                                    "Signatures, state S352)", TestManagerHelpers.Describe(temp48)));
                        this.Manager.Comment("reaching state \'S385\'");
                        FRS2Model.error_status_t temp49;
                        this.Manager.Comment("executing step \'call RdcPushSourceNeeds()\'");
                        temp49 = this.IFRS2ManagedAdapterInstance.RdcPushSourceNeeds();
                        this.Manager.Checkpoint("MS-FRS2_R671");
                        this.Manager.Checkpoint("MS-FRS2_R674");
                        this.Manager.Checkpoint("MS-FRS2_R693");
                        this.Manager.Checkpoint("MS-FRS2_R694");
                        this.Manager.Checkpoint("MS-FRS2_R683");
                        this.Manager.Checkpoint("MS-FRS2_R691");
                        this.Manager.Checkpoint("MS-FRS2_R692");
                        this.Manager.Checkpoint("MS-FRS2_R675");
                        this.Manager.Comment("reaching state \'S405\'");
                        this.Manager.Comment("checking step \'return RdcPushSourceNeeds/ERROR_SUCCESS\'");
                        this.Manager.Assert((temp49 == FRS2Model.error_status_t.ERROR_SUCCESS), String.Format("expected \'FRS2Model.error_status_t.ERROR_SUCCESS\', actual \'{0}\' (return of RdcPus" +
                                    "hSourceNeeds, state S405)", TestManagerHelpers.Describe(temp49)));
                        this.Manager.Comment("reaching state \'S417\'");
                        FRS2Model.error_status_t temp50;
                        this.Manager.Comment("executing step \'call RdcGetFileData(inValidBufSize)\'");
                        temp50 = this.IFRS2ManagedAdapterInstance.RdcGetFileData(FRS2Model.BufferSize.inValidBufSize);
                        this.Manager.Comment("reaching state \'S429\'");
                        this.Manager.Comment("checking step \'return RdcGetFileData/ERROR_FAIL\'");
                        this.Manager.Assert((temp50 == FRS2Model.error_status_t.ERROR_FAIL), String.Format("expected \'FRS2Model.error_status_t.ERROR_FAIL\', actual \'{0}\' (return of RdcGetFil" +
                                    "eData, state S429)", TestManagerHelpers.Describe(temp50)));
                        this.Manager.Comment("reaching state \'S438\'");
                        goto label5;
                    }
                    if ((temp53 == 4)) {
                        TCtestScenario3S211();
                        goto label5;
                    }
                    if ((temp53 == 5)) {
                        TCtestScenario3S210();
                        goto label5;
                    }
                    if ((temp53 == 6)) {
                        TCtestScenario3S222();
                        goto label5;
                    }
                    if ((temp53 == 7)) {
                        TCtestScenario3S223();
                        goto label5;
                    }
                    throw new InvalidOperationException("never reached");
                label5:
;
                    goto label8;
                }
                if ((temp70 == 1)) {
                    this.Manager.Comment("reaching state \'S132\'");
                    FRS2Model.error_status_t temp54;
                    this.Manager.Comment("executing step \'call InitializeFileTransferAsync(1,1,True)\'");
                    temp54 = this.IFRS2ManagedAdapterInstance.InitializeFileTransferAsync(1, 1, true);
                    this.Manager.Checkpoint("MS-FRS2_R774");
                    this.Manager.Checkpoint("MS-FRS2_R777");
                    this.Manager.Checkpoint("MS-FRS2_R793");
                    this.Manager.Comment("reaching state \'S160\'");
                    this.Manager.Comment("checking step \'return InitializeFileTransferAsync/ERROR_SUCCESS\'");
                    this.Manager.Assert((temp54 == FRS2Model.error_status_t.ERROR_SUCCESS), String.Format("expected \'FRS2Model.error_status_t.ERROR_SUCCESS\', actual \'{0}\' (return of Initia" +
                                "lizeFileTransferAsync, state S160)", TestManagerHelpers.Describe(temp54)));
                    this.Manager.Comment("reaching state \'S178\'");
                    int temp61 = this.Manager.ExpectEvent(this.QuiescenceTimeout, true, new ExpectedEvent(TCtestScenario3.InitializeFileTransferAsyncEventInfo, null, new InitializeFileTransferAsyncEventDelegate1(this.TCtestScenario3S2InitializeFileTransferAsyncEventChecker8)), new ExpectedEvent(TCtestScenario3.InitializeFileTransferAsyncEventInfo, null, new InitializeFileTransferAsyncEventDelegate1(this.TCtestScenario3S2InitializeFileTransferAsyncEventChecker9)), new ExpectedEvent(TCtestScenario3.InitializeFileTransferAsyncEventInfo, null, new InitializeFileTransferAsyncEventDelegate1(this.TCtestScenario3S2InitializeFileTransferAsyncEventChecker10)), new ExpectedEvent(TCtestScenario3.InitializeFileTransferAsyncEventInfo, null, new InitializeFileTransferAsyncEventDelegate1(this.TCtestScenario3S2InitializeFileTransferAsyncEventChecker11)), new ExpectedEvent(TCtestScenario3.InitializeFileTransferAsyncEventInfo, null, new InitializeFileTransferAsyncEventDelegate1(this.TCtestScenario3S2InitializeFileTransferAsyncEventChecker12)), new ExpectedEvent(TCtestScenario3.InitializeFileTransferAsyncEventInfo, null, new InitializeFileTransferAsyncEventDelegate1(this.TCtestScenario3S2InitializeFileTransferAsyncEventChecker13)), new ExpectedEvent(TCtestScenario3.InitializeFileTransferAsyncEventInfo, null, new InitializeFileTransferAsyncEventDelegate1(this.TCtestScenario3S2InitializeFileTransferAsyncEventChecker14)), new ExpectedEvent(TCtestScenario3.InitializeFileTransferAsyncEventInfo, null, new InitializeFileTransferAsyncEventDelegate1(this.TCtestScenario3S2InitializeFileTransferAsyncEventChecker15)));
                    if ((temp61 == 0)) {
                        this.Manager.Comment("reaching state \'S224\'");
                        FRS2Model.error_status_t temp55;
                        this.Manager.Comment("executing step \'call RdcGetSignatures(valid)\'");
                        temp55 = this.IFRS2ManagedAdapterInstance.RdcGetSignatures(FRS2Model.offset.valid);
                        this.Manager.Checkpoint("MS-FRS2_R646");
                        this.Manager.Checkpoint("MS-FRS2_R649");
                        this.Manager.Checkpoint("MS-FRS2_R650");
                        this.Manager.Comment("reaching state \'S355\'");
                        this.Manager.Comment("checking step \'return RdcGetSignatures/ERROR_SUCCESS\'");
                        this.Manager.Assert((temp55 == FRS2Model.error_status_t.ERROR_SUCCESS), String.Format("expected \'FRS2Model.error_status_t.ERROR_SUCCESS\', actual \'{0}\' (return of RdcGet" +
                                    "Signatures, state S355)", TestManagerHelpers.Describe(temp55)));
                        this.Manager.Comment("reaching state \'S388\'");
                        FRS2Model.error_status_t temp56;
                        this.Manager.Comment("executing step \'call RdcPushSourceNeeds()\'");
                        temp56 = this.IFRS2ManagedAdapterInstance.RdcPushSourceNeeds();
                        this.Manager.Checkpoint("MS-FRS2_R671");
                        this.Manager.Checkpoint("MS-FRS2_R674");
                        this.Manager.Checkpoint("MS-FRS2_R693");
                        this.Manager.Checkpoint("MS-FRS2_R694");
                        this.Manager.Checkpoint("MS-FRS2_R683");
                        this.Manager.Checkpoint("MS-FRS2_R691");
                        this.Manager.Checkpoint("MS-FRS2_R692");
                        this.Manager.Checkpoint("MS-FRS2_R675");
                        this.Manager.Comment("reaching state \'S406\'");
                        this.Manager.Comment("checking step \'return RdcPushSourceNeeds/ERROR_SUCCESS\'");
                        this.Manager.Assert((temp56 == FRS2Model.error_status_t.ERROR_SUCCESS), String.Format("expected \'FRS2Model.error_status_t.ERROR_SUCCESS\', actual \'{0}\' (return of RdcPus" +
                                    "hSourceNeeds, state S406)", TestManagerHelpers.Describe(temp56)));
                        this.Manager.Comment("reaching state \'S418\'");
                        FRS2Model.error_status_t temp57;
                        this.Manager.Comment("executing step \'call RdcGetFileData(inValidBufSize)\'");
                        temp57 = this.IFRS2ManagedAdapterInstance.RdcGetFileData(FRS2Model.BufferSize.inValidBufSize);
                        this.Manager.Comment("reaching state \'S430\'");
                        this.Manager.Comment("checking step \'return RdcGetFileData/ERROR_FAIL\'");
                        this.Manager.Assert((temp57 == FRS2Model.error_status_t.ERROR_FAIL), String.Format("expected \'FRS2Model.error_status_t.ERROR_FAIL\', actual \'{0}\' (return of RdcGetFil" +
                                    "eData, state S430)", TestManagerHelpers.Describe(temp57)));
                        this.Manager.Comment("reaching state \'S439\'");
                        goto label6;
                    }
                    if ((temp61 == 1)) {
                        TCtestScenario3S199();
                        goto label6;
                    }
                    if ((temp61 == 2)) {
                        TCtestScenario3S197();
                        goto label6;
                    }
                    if ((temp61 == 3)) {
                        TCtestScenario3S227();
                        goto label6;
                    }
                    if ((temp61 == 4)) {
                        TCtestScenario3S228();
                        goto label6;
                    }
                    if ((temp61 == 5)) {
                        TCtestScenario3S194();
                        goto label6;
                    }
                    if ((temp61 == 6)) {
                        TCtestScenario3S230();
                        goto label6;
                    }
                    if ((temp61 == 7)) {
                        TCtestScenario3S192();
                        goto label6;
                    }
                    throw new InvalidOperationException("never reached");
                label6:
;
                    goto label8;
                }
                if ((temp70 == 2)) {
                    this.Manager.Comment("reaching state \'S133\'");
                    FRS2Model.error_status_t temp62;
                    this.Manager.Comment("executing step \'call InitializeFileTransferAsync(1,1,True)\'");
                    temp62 = this.IFRS2ManagedAdapterInstance.InitializeFileTransferAsync(1, 1, true);
                    this.Manager.Checkpoint("MS-FRS2_R774");
                    this.Manager.Checkpoint("MS-FRS2_R777");
                    this.Manager.Checkpoint("MS-FRS2_R793");
                    this.Manager.Comment("reaching state \'S161\'");
                    this.Manager.Comment("checking step \'return InitializeFileTransferAsync/ERROR_SUCCESS\'");
                    this.Manager.Assert((temp62 == FRS2Model.error_status_t.ERROR_SUCCESS), String.Format("expected \'FRS2Model.error_status_t.ERROR_SUCCESS\', actual \'{0}\' (return of Initia" +
                                "lizeFileTransferAsync, state S161)", TestManagerHelpers.Describe(temp62)));
                    this.Manager.Comment("reaching state \'S179\'");
                    int temp69 = this.Manager.ExpectEvent(this.QuiescenceTimeout, true, new ExpectedEvent(TCtestScenario3.InitializeFileTransferAsyncEventInfo, null, new InitializeFileTransferAsyncEventDelegate1(this.TCtestScenario3S2InitializeFileTransferAsyncEventChecker16)), new ExpectedEvent(TCtestScenario3.InitializeFileTransferAsyncEventInfo, null, new InitializeFileTransferAsyncEventDelegate1(this.TCtestScenario3S2InitializeFileTransferAsyncEventChecker17)), new ExpectedEvent(TCtestScenario3.InitializeFileTransferAsyncEventInfo, null, new InitializeFileTransferAsyncEventDelegate1(this.TCtestScenario3S2InitializeFileTransferAsyncEventChecker18)), new ExpectedEvent(TCtestScenario3.InitializeFileTransferAsyncEventInfo, null, new InitializeFileTransferAsyncEventDelegate1(this.TCtestScenario3S2InitializeFileTransferAsyncEventChecker19)), new ExpectedEvent(TCtestScenario3.InitializeFileTransferAsyncEventInfo, null, new InitializeFileTransferAsyncEventDelegate1(this.TCtestScenario3S2InitializeFileTransferAsyncEventChecker20)), new ExpectedEvent(TCtestScenario3.InitializeFileTransferAsyncEventInfo, null, new InitializeFileTransferAsyncEventDelegate1(this.TCtestScenario3S2InitializeFileTransferAsyncEventChecker21)), new ExpectedEvent(TCtestScenario3.InitializeFileTransferAsyncEventInfo, null, new InitializeFileTransferAsyncEventDelegate1(this.TCtestScenario3S2InitializeFileTransferAsyncEventChecker22)), new ExpectedEvent(TCtestScenario3.InitializeFileTransferAsyncEventInfo, null, new InitializeFileTransferAsyncEventDelegate1(this.TCtestScenario3S2InitializeFileTransferAsyncEventChecker23)));
                    if ((temp69 == 0)) {
                        this.Manager.Comment("reaching state \'S232\'");
                        FRS2Model.error_status_t temp63;
                        this.Manager.Comment("executing step \'call RdcGetSignatures(valid)\'");
                        temp63 = this.IFRS2ManagedAdapterInstance.RdcGetSignatures(FRS2Model.offset.valid);
                        this.Manager.Checkpoint("MS-FRS2_R646");
                        this.Manager.Checkpoint("MS-FRS2_R649");
                        this.Manager.Checkpoint("MS-FRS2_R650");
                        this.Manager.Comment("reaching state \'S359\'");
                        this.Manager.Comment("checking step \'return RdcGetSignatures/ERROR_SUCCESS\'");
                        this.Manager.Assert((temp63 == FRS2Model.error_status_t.ERROR_SUCCESS), String.Format("expected \'FRS2Model.error_status_t.ERROR_SUCCESS\', actual \'{0}\' (return of RdcGet" +
                                    "Signatures, state S359)", TestManagerHelpers.Describe(temp63)));
                        this.Manager.Comment("reaching state \'S392\'");
                        FRS2Model.error_status_t temp64;
                        this.Manager.Comment("executing step \'call RdcPushSourceNeeds()\'");
                        temp64 = this.IFRS2ManagedAdapterInstance.RdcPushSourceNeeds();
                        this.Manager.Checkpoint("MS-FRS2_R671");
                        this.Manager.Checkpoint("MS-FRS2_R674");
                        this.Manager.Checkpoint("MS-FRS2_R693");
                        this.Manager.Checkpoint("MS-FRS2_R694");
                        this.Manager.Checkpoint("MS-FRS2_R683");
                        this.Manager.Checkpoint("MS-FRS2_R691");
                        this.Manager.Checkpoint("MS-FRS2_R692");
                        this.Manager.Checkpoint("MS-FRS2_R675");
                        this.Manager.Comment("reaching state \'S407\'");
                        this.Manager.Comment("checking step \'return RdcPushSourceNeeds/ERROR_SUCCESS\'");
                        this.Manager.Assert((temp64 == FRS2Model.error_status_t.ERROR_SUCCESS), String.Format("expected \'FRS2Model.error_status_t.ERROR_SUCCESS\', actual \'{0}\' (return of RdcPus" +
                                    "hSourceNeeds, state S407)", TestManagerHelpers.Describe(temp64)));
                        this.Manager.Comment("reaching state \'S419\'");
                        FRS2Model.error_status_t temp65;
                        this.Manager.Comment("executing step \'call RdcGetFileData(inValidBufSize)\'");
                        temp65 = this.IFRS2ManagedAdapterInstance.RdcGetFileData(FRS2Model.BufferSize.inValidBufSize);
                        this.Manager.Comment("reaching state \'S431\'");
                        this.Manager.Comment("checking step \'return RdcGetFileData/ERROR_FAIL\'");
                        this.Manager.Assert((temp65 == FRS2Model.error_status_t.ERROR_FAIL), String.Format("expected \'FRS2Model.error_status_t.ERROR_FAIL\', actual \'{0}\' (return of RdcGetFil" +
                                    "eData, state S431)", TestManagerHelpers.Describe(temp65)));
                        this.Manager.Comment("reaching state \'S440\'");
                        goto label7;
                    }
                    if ((temp69 == 1)) {
                        TCtestScenario3S207();
                        goto label7;
                    }
                    if ((temp69 == 2)) {
                        TCtestScenario3S206();
                        goto label7;
                    }
                    if ((temp69 == 3)) {
                        TCtestScenario3S235();
                        goto label7;
                    }
                    if ((temp69 == 4)) {
                        TCtestScenario3S203();
                        goto label7;
                    }
                    if ((temp69 == 5)) {
                        TCtestScenario3S202();
                        goto label7;
                    }
                    if ((temp69 == 6)) {
                        TCtestScenario3S238();
                        goto label7;
                    }
                    if ((temp69 == 7)) {
                        TCtestScenario3S239();
                        goto label7;
                    }
                    throw new InvalidOperationException("never reached");
                label7:
;
                    goto label8;
                }
                if ((temp70 == 3)) {
                    TCtestScenario3S127();
                    goto label8;
                }
                throw new InvalidOperationException("never reached");
            label8:
;
                goto label9;
            }
            if ((temp71 == 1)) {
                TCtestScenario3S96();
                goto label9;
            }
            throw new InvalidOperationException("never reached");
        label9:
;
            this.Manager.EndTest();
        }
        
        private void TCtestScenario3S2AsyncPollResponseEventChecker(FRS2Model.VVGeneration vvGen) {
            this.Manager.Comment("checking step \'event AsyncPollResponseEvent(ValidValue)\'");
            try {
                this.Manager.Assert((vvGen == FRS2Model.VVGeneration.ValidValue), String.Format("expected \'FRS2Model.VVGeneration.ValidValue\', actual \'{0}\' (vvGen of AsyncPollRes" +
                            "ponseEvent, state S88)", TestManagerHelpers.Describe(vvGen)));
            }
            catch (TransactionFailedException ) {
                this.Manager.Comment("This step would have covered MS-FRS2_R556, MS-FRS2_R1020, MS-FRS2_R555");
                throw;
            }
            this.Manager.Checkpoint("MS-FRS2_R556");
            this.Manager.Checkpoint("MS-FRS2_R1020");
            this.Manager.Checkpoint("MS-FRS2_R555");
        }
        
        private void TCtestScenario3S2RequestUpdatesEventChecker(FRS2Model.FilePresense present, FRS2Model.UPDATE_STATUS updateStatus) {
            this.Manager.Comment("checking step \'event RequestUpdatesEvent(fileExists,UPDATE_STATUS_MORE)\'");
            try {
                this.Manager.Assert((present == FRS2Model.FilePresense.fileExists), String.Format("expected \'FRS2Model.FilePresense.fileExists\', actual \'{0}\' (present of RequestUpd" +
                            "atesEvent, state S120)", TestManagerHelpers.Describe(present)));
                this.Manager.Assert((updateStatus == FRS2Model.UPDATE_STATUS.UPDATE_STATUS_MORE), String.Format("expected \'FRS2Model.UPDATE_STATUS.UPDATE_STATUS_MORE\', actual \'{0}\' (updateStatus" +
                            " of RequestUpdatesEvent, state S120)", TestManagerHelpers.Describe(updateStatus)));
            }
            catch (TransactionFailedException ) {
                this.Manager.Comment("This step would have covered MS-FRS2_R93, MS-FRS2_R498");
                throw;
            }
            this.Manager.Checkpoint("MS-FRS2_R93");
            this.Manager.Checkpoint("MS-FRS2_R498");
        }
        
        private void TCtestScenario3S2InitializeFileTransferAsyncEventChecker(FRS2Model.ServerContext context, FRS2Model.RDC_Sig_Level rdcsigLevel, bool isEOF) {
            this.Manager.Comment("checking step \'event InitializeFileTransferAsyncEvent(InvalidContext,forRDCFileLe" +
                    "vel,True)\'");
            this.Manager.Assert((context == FRS2Model.ServerContext.InvalidContext), String.Format("expected \'FRS2Model.ServerContext.InvalidContext\', actual \'{0}\' (context of Initi" +
                        "alizeFileTransferAsyncEvent, state S177)", TestManagerHelpers.Describe(context)));
            this.Manager.Assert((rdcsigLevel == FRS2Model.RDC_Sig_Level.forRDCFileLevel), String.Format("expected \'FRS2Model.RDC_Sig_Level.forRDCFileLevel\', actual \'{0}\' (rdcsigLevel of " +
                        "InitializeFileTransferAsyncEvent, state S177)", TestManagerHelpers.Describe(rdcsigLevel)));
            this.Manager.Assert((isEOF == true), String.Format("expected \'true\', actual \'{0}\' (isEOF of InitializeFileTransferAsyncEvent, state S" +
                        "177)", TestManagerHelpers.Describe(isEOF)));
        }
        
        private void TCtestScenario3S216() {
            this.Manager.Comment("reaching state \'S216\'");
            FRS2Model.error_status_t temp47;
            this.Manager.Comment("executing step \'call RdcGetSignatures(valid)\'");
            temp47 = this.IFRS2ManagedAdapterInstance.RdcGetSignatures(FRS2Model.offset.valid);
            this.Manager.Checkpoint("MS-FRS2_R647");
            this.Manager.Checkpoint("MS-FRS2_R655");
            this.Manager.Checkpoint("MS-FRS2_R656");
            this.Manager.Checkpoint("MS-FRS2_R657");
            this.Manager.Checkpoint("MS-FRS2_R805");
            this.Manager.Comment("reaching state \'S351\'");
            this.Manager.Comment("checking step \'return RdcGetSignatures/ERROR_FAIL\'");
            this.Manager.Assert((temp47 == FRS2Model.error_status_t.ERROR_FAIL), String.Format("expected \'FRS2Model.error_status_t.ERROR_FAIL\', actual \'{0}\' (return of RdcGetSig" +
                        "natures, state S351)", TestManagerHelpers.Describe(temp47)));
            TCtestScenario3S383();
        }
        
        private void TCtestScenario3S2InitializeFileTransferAsyncEventChecker1(FRS2Model.ServerContext context, FRS2Model.RDC_Sig_Level rdcsigLevel, bool isEOF) {
            this.Manager.Comment("checking step \'event InitializeFileTransferAsyncEvent(ValidContext,forRAWFileLeve" +
                    "l,False)\'");
            this.Manager.Assert((context == FRS2Model.ServerContext.ValidContext), String.Format("expected \'FRS2Model.ServerContext.ValidContext\', actual \'{0}\' (context of Initial" +
                        "izeFileTransferAsyncEvent, state S177)", TestManagerHelpers.Describe(context)));
            this.Manager.Assert((rdcsigLevel == FRS2Model.RDC_Sig_Level.forRAWFileLevel), String.Format("expected \'FRS2Model.RDC_Sig_Level.forRAWFileLevel\', actual \'{0}\' (rdcsigLevel of " +
                        "InitializeFileTransferAsyncEvent, state S177)", TestManagerHelpers.Describe(rdcsigLevel)));
            this.Manager.Assert((isEOF == false), String.Format("expected \'false\', actual \'{0}\' (isEOF of InitializeFileTransferAsyncEvent, state " +
                        "S177)", TestManagerHelpers.Describe(isEOF)));
        }
        
        private void TCtestScenario3S2InitializeFileTransferAsyncEventChecker2(FRS2Model.ServerContext context, FRS2Model.RDC_Sig_Level rdcsigLevel, bool isEOF) {
            this.Manager.Comment("checking step \'event InitializeFileTransferAsyncEvent(InvalidContext,forRAWFileLe" +
                    "vel,False)\'");
            this.Manager.Assert((context == FRS2Model.ServerContext.InvalidContext), String.Format("expected \'FRS2Model.ServerContext.InvalidContext\', actual \'{0}\' (context of Initi" +
                        "alizeFileTransferAsyncEvent, state S177)", TestManagerHelpers.Describe(context)));
            this.Manager.Assert((rdcsigLevel == FRS2Model.RDC_Sig_Level.forRAWFileLevel), String.Format("expected \'FRS2Model.RDC_Sig_Level.forRAWFileLevel\', actual \'{0}\' (rdcsigLevel of " +
                        "InitializeFileTransferAsyncEvent, state S177)", TestManagerHelpers.Describe(rdcsigLevel)));
            this.Manager.Assert((isEOF == false), String.Format("expected \'false\', actual \'{0}\' (isEOF of InitializeFileTransferAsyncEvent, state " +
                        "S177)", TestManagerHelpers.Describe(isEOF)));
        }
        
        private void TCtestScenario3S2InitializeFileTransferAsyncEventChecker3(FRS2Model.ServerContext context, FRS2Model.RDC_Sig_Level rdcsigLevel, bool isEOF) {
            this.Manager.Comment("checking step \'event InitializeFileTransferAsyncEvent(ValidContext,forRDCFileLeve" +
                    "l,False)\'");
            this.Manager.Assert((context == FRS2Model.ServerContext.ValidContext), String.Format("expected \'FRS2Model.ServerContext.ValidContext\', actual \'{0}\' (context of Initial" +
                        "izeFileTransferAsyncEvent, state S177)", TestManagerHelpers.Describe(context)));
            this.Manager.Assert((rdcsigLevel == FRS2Model.RDC_Sig_Level.forRDCFileLevel), String.Format("expected \'FRS2Model.RDC_Sig_Level.forRDCFileLevel\', actual \'{0}\' (rdcsigLevel of " +
                        "InitializeFileTransferAsyncEvent, state S177)", TestManagerHelpers.Describe(rdcsigLevel)));
            this.Manager.Assert((isEOF == false), String.Format("expected \'false\', actual \'{0}\' (isEOF of InitializeFileTransferAsyncEvent, state " +
                        "S177)", TestManagerHelpers.Describe(isEOF)));
        }
        
        private void TCtestScenario3S2InitializeFileTransferAsyncEventChecker4(FRS2Model.ServerContext context, FRS2Model.RDC_Sig_Level rdcsigLevel, bool isEOF) {
            this.Manager.Comment("checking step \'event InitializeFileTransferAsyncEvent(ValidContext,forRDCFileLeve" +
                    "l,True)\'");
            this.Manager.Assert((context == FRS2Model.ServerContext.ValidContext), String.Format("expected \'FRS2Model.ServerContext.ValidContext\', actual \'{0}\' (context of Initial" +
                        "izeFileTransferAsyncEvent, state S177)", TestManagerHelpers.Describe(context)));
            this.Manager.Assert((rdcsigLevel == FRS2Model.RDC_Sig_Level.forRDCFileLevel), String.Format("expected \'FRS2Model.RDC_Sig_Level.forRDCFileLevel\', actual \'{0}\' (rdcsigLevel of " +
                        "InitializeFileTransferAsyncEvent, state S177)", TestManagerHelpers.Describe(rdcsigLevel)));
            this.Manager.Assert((isEOF == true), String.Format("expected \'true\', actual \'{0}\' (isEOF of InitializeFileTransferAsyncEvent, state S" +
                        "177)", TestManagerHelpers.Describe(isEOF)));
        }
        
        private void TCtestScenario3S2InitializeFileTransferAsyncEventChecker5(FRS2Model.ServerContext context, FRS2Model.RDC_Sig_Level rdcsigLevel, bool isEOF) {
            this.Manager.Comment("checking step \'event InitializeFileTransferAsyncEvent(InvalidContext,forRDCFileLe" +
                    "vel,False)\'");
            this.Manager.Assert((context == FRS2Model.ServerContext.InvalidContext), String.Format("expected \'FRS2Model.ServerContext.InvalidContext\', actual \'{0}\' (context of Initi" +
                        "alizeFileTransferAsyncEvent, state S177)", TestManagerHelpers.Describe(context)));
            this.Manager.Assert((rdcsigLevel == FRS2Model.RDC_Sig_Level.forRDCFileLevel), String.Format("expected \'FRS2Model.RDC_Sig_Level.forRDCFileLevel\', actual \'{0}\' (rdcsigLevel of " +
                        "InitializeFileTransferAsyncEvent, state S177)", TestManagerHelpers.Describe(rdcsigLevel)));
            this.Manager.Assert((isEOF == false), String.Format("expected \'false\', actual \'{0}\' (isEOF of InitializeFileTransferAsyncEvent, state " +
                        "S177)", TestManagerHelpers.Describe(isEOF)));
        }
        
        private void TCtestScenario3S2InitializeFileTransferAsyncEventChecker6(FRS2Model.ServerContext context, FRS2Model.RDC_Sig_Level rdcsigLevel, bool isEOF) {
            this.Manager.Comment("checking step \'event InitializeFileTransferAsyncEvent(InvalidContext,forRAWFileLe" +
                    "vel,True)\'");
            this.Manager.Assert((context == FRS2Model.ServerContext.InvalidContext), String.Format("expected \'FRS2Model.ServerContext.InvalidContext\', actual \'{0}\' (context of Initi" +
                        "alizeFileTransferAsyncEvent, state S177)", TestManagerHelpers.Describe(context)));
            this.Manager.Assert((rdcsigLevel == FRS2Model.RDC_Sig_Level.forRAWFileLevel), String.Format("expected \'FRS2Model.RDC_Sig_Level.forRAWFileLevel\', actual \'{0}\' (rdcsigLevel of " +
                        "InitializeFileTransferAsyncEvent, state S177)", TestManagerHelpers.Describe(rdcsigLevel)));
            this.Manager.Assert((isEOF == true), String.Format("expected \'true\', actual \'{0}\' (isEOF of InitializeFileTransferAsyncEvent, state S" +
                        "177)", TestManagerHelpers.Describe(isEOF)));
        }
        
        private void TCtestScenario3S222() {
            this.Manager.Comment("reaching state \'S222\'");
            FRS2Model.error_status_t temp51;
            this.Manager.Comment("executing step \'call RdcGetSignatures(valid)\'");
            temp51 = this.IFRS2ManagedAdapterInstance.RdcGetSignatures(FRS2Model.offset.valid);
            this.Manager.Checkpoint("MS-FRS2_R647");
            this.Manager.Checkpoint("MS-FRS2_R655");
            this.Manager.Checkpoint("MS-FRS2_R656");
            this.Manager.Checkpoint("MS-FRS2_R657");
            this.Manager.Checkpoint("MS-FRS2_R658");
            this.Manager.Checkpoint("MS-FRS2_R805");
            this.Manager.Comment("reaching state \'S353\'");
            this.Manager.Comment("checking step \'return RdcGetSignatures/ERROR_FAIL\'");
            this.Manager.Assert((temp51 == FRS2Model.error_status_t.ERROR_FAIL), String.Format("expected \'FRS2Model.error_status_t.ERROR_FAIL\', actual \'{0}\' (return of RdcGetSig" +
                        "natures, state S353)", TestManagerHelpers.Describe(temp51)));
            TCtestScenario3S380();
        }
        
        private void TCtestScenario3S2InitializeFileTransferAsyncEventChecker7(FRS2Model.ServerContext context, FRS2Model.RDC_Sig_Level rdcsigLevel, bool isEOF) {
            this.Manager.Comment("checking step \'event InitializeFileTransferAsyncEvent(ValidContext,forRAWFileLeve" +
                    "l,True)\'");
            this.Manager.Assert((context == FRS2Model.ServerContext.ValidContext), String.Format("expected \'FRS2Model.ServerContext.ValidContext\', actual \'{0}\' (context of Initial" +
                        "izeFileTransferAsyncEvent, state S177)", TestManagerHelpers.Describe(context)));
            this.Manager.Assert((rdcsigLevel == FRS2Model.RDC_Sig_Level.forRAWFileLevel), String.Format("expected \'FRS2Model.RDC_Sig_Level.forRAWFileLevel\', actual \'{0}\' (rdcsigLevel of " +
                        "InitializeFileTransferAsyncEvent, state S177)", TestManagerHelpers.Describe(rdcsigLevel)));
            this.Manager.Assert((isEOF == true), String.Format("expected \'true\', actual \'{0}\' (isEOF of InitializeFileTransferAsyncEvent, state S" +
                        "177)", TestManagerHelpers.Describe(isEOF)));
        }
        
        private void TCtestScenario3S223() {
            this.Manager.Comment("reaching state \'S223\'");
            FRS2Model.error_status_t temp52;
            this.Manager.Comment("executing step \'call RdcGetSignatures(valid)\'");
            temp52 = this.IFRS2ManagedAdapterInstance.RdcGetSignatures(FRS2Model.offset.valid);
            this.Manager.Checkpoint("MS-FRS2_R647");
            this.Manager.Checkpoint("MS-FRS2_R658");
            this.Manager.Checkpoint("MS-FRS2_R805");
            this.Manager.Comment("reaching state \'S354\'");
            this.Manager.Comment("checking step \'return RdcGetSignatures/ERROR_FAIL\'");
            this.Manager.Assert((temp52 == FRS2Model.error_status_t.ERROR_FAIL), String.Format("expected \'FRS2Model.error_status_t.ERROR_FAIL\', actual \'{0}\' (return of RdcGetSig" +
                        "natures, state S354)", TestManagerHelpers.Describe(temp52)));
            TCtestScenario3S379();
        }
        
        private void TCtestScenario3S2RequestUpdatesEventChecker1(FRS2Model.FilePresense present, FRS2Model.UPDATE_STATUS updateStatus) {
            this.Manager.Comment("checking step \'event RequestUpdatesEvent(fileDeleted,UPDATE_STATUS_DONE)\'");
            try {
                this.Manager.Assert((present == FRS2Model.FilePresense.fileDeleted), String.Format("expected \'FRS2Model.FilePresense.fileDeleted\', actual \'{0}\' (present of RequestUp" +
                            "datesEvent, state S120)", TestManagerHelpers.Describe(present)));
                this.Manager.Assert((updateStatus == FRS2Model.UPDATE_STATUS.UPDATE_STATUS_DONE), String.Format("expected \'FRS2Model.UPDATE_STATUS.UPDATE_STATUS_DONE\', actual \'{0}\' (updateStatus" +
                            " of RequestUpdatesEvent, state S120)", TestManagerHelpers.Describe(updateStatus)));
            }
            catch (TransactionFailedException ) {
                this.Manager.Comment("This step would have covered MS-FRS2_R93, MS-FRS2_R94");
                throw;
            }
            this.Manager.Checkpoint("MS-FRS2_R93");
            this.Manager.Checkpoint("MS-FRS2_R94");
        }
        
        private void TCtestScenario3S2InitializeFileTransferAsyncEventChecker8(FRS2Model.ServerContext context, FRS2Model.RDC_Sig_Level rdcsigLevel, bool isEOF) {
            this.Manager.Comment("checking step \'event InitializeFileTransferAsyncEvent(ValidContext,forRDCFileLeve" +
                    "l,False)\'");
            this.Manager.Assert((context == FRS2Model.ServerContext.ValidContext), String.Format("expected \'FRS2Model.ServerContext.ValidContext\', actual \'{0}\' (context of Initial" +
                        "izeFileTransferAsyncEvent, state S178)", TestManagerHelpers.Describe(context)));
            this.Manager.Assert((rdcsigLevel == FRS2Model.RDC_Sig_Level.forRDCFileLevel), String.Format("expected \'FRS2Model.RDC_Sig_Level.forRDCFileLevel\', actual \'{0}\' (rdcsigLevel of " +
                        "InitializeFileTransferAsyncEvent, state S178)", TestManagerHelpers.Describe(rdcsigLevel)));
            this.Manager.Assert((isEOF == false), String.Format("expected \'false\', actual \'{0}\' (isEOF of InitializeFileTransferAsyncEvent, state " +
                        "S178)", TestManagerHelpers.Describe(isEOF)));
        }
        
        private void TCtestScenario3S2InitializeFileTransferAsyncEventChecker9(FRS2Model.ServerContext context, FRS2Model.RDC_Sig_Level rdcsigLevel, bool isEOF) {
            this.Manager.Comment("checking step \'event InitializeFileTransferAsyncEvent(InvalidContext,forRDCFileLe" +
                    "vel,False)\'");
            this.Manager.Assert((context == FRS2Model.ServerContext.InvalidContext), String.Format("expected \'FRS2Model.ServerContext.InvalidContext\', actual \'{0}\' (context of Initi" +
                        "alizeFileTransferAsyncEvent, state S178)", TestManagerHelpers.Describe(context)));
            this.Manager.Assert((rdcsigLevel == FRS2Model.RDC_Sig_Level.forRDCFileLevel), String.Format("expected \'FRS2Model.RDC_Sig_Level.forRDCFileLevel\', actual \'{0}\' (rdcsigLevel of " +
                        "InitializeFileTransferAsyncEvent, state S178)", TestManagerHelpers.Describe(rdcsigLevel)));
            this.Manager.Assert((isEOF == false), String.Format("expected \'false\', actual \'{0}\' (isEOF of InitializeFileTransferAsyncEvent, state " +
                        "S178)", TestManagerHelpers.Describe(isEOF)));
        }
        
        private void TCtestScenario3S2InitializeFileTransferAsyncEventChecker10(FRS2Model.ServerContext context, FRS2Model.RDC_Sig_Level rdcsigLevel, bool isEOF) {
            this.Manager.Comment("checking step \'event InitializeFileTransferAsyncEvent(ValidContext,forRAWFileLeve" +
                    "l,False)\'");
            this.Manager.Assert((context == FRS2Model.ServerContext.ValidContext), String.Format("expected \'FRS2Model.ServerContext.ValidContext\', actual \'{0}\' (context of Initial" +
                        "izeFileTransferAsyncEvent, state S178)", TestManagerHelpers.Describe(context)));
            this.Manager.Assert((rdcsigLevel == FRS2Model.RDC_Sig_Level.forRAWFileLevel), String.Format("expected \'FRS2Model.RDC_Sig_Level.forRAWFileLevel\', actual \'{0}\' (rdcsigLevel of " +
                        "InitializeFileTransferAsyncEvent, state S178)", TestManagerHelpers.Describe(rdcsigLevel)));
            this.Manager.Assert((isEOF == false), String.Format("expected \'false\', actual \'{0}\' (isEOF of InitializeFileTransferAsyncEvent, state " +
                        "S178)", TestManagerHelpers.Describe(isEOF)));
        }
        
        private void TCtestScenario3S2InitializeFileTransferAsyncEventChecker11(FRS2Model.ServerContext context, FRS2Model.RDC_Sig_Level rdcsigLevel, bool isEOF) {
            this.Manager.Comment("checking step \'event InitializeFileTransferAsyncEvent(InvalidContext,forRAWFileLe" +
                    "vel,True)\'");
            this.Manager.Assert((context == FRS2Model.ServerContext.InvalidContext), String.Format("expected \'FRS2Model.ServerContext.InvalidContext\', actual \'{0}\' (context of Initi" +
                        "alizeFileTransferAsyncEvent, state S178)", TestManagerHelpers.Describe(context)));
            this.Manager.Assert((rdcsigLevel == FRS2Model.RDC_Sig_Level.forRAWFileLevel), String.Format("expected \'FRS2Model.RDC_Sig_Level.forRAWFileLevel\', actual \'{0}\' (rdcsigLevel of " +
                        "InitializeFileTransferAsyncEvent, state S178)", TestManagerHelpers.Describe(rdcsigLevel)));
            this.Manager.Assert((isEOF == true), String.Format("expected \'true\', actual \'{0}\' (isEOF of InitializeFileTransferAsyncEvent, state S" +
                        "178)", TestManagerHelpers.Describe(isEOF)));
        }
        
        private void TCtestScenario3S227() {
            this.Manager.Comment("reaching state \'S227\'");
            FRS2Model.error_status_t temp58;
            this.Manager.Comment("executing step \'call RdcGetSignatures(valid)\'");
            temp58 = this.IFRS2ManagedAdapterInstance.RdcGetSignatures(FRS2Model.offset.valid);
            this.Manager.Checkpoint("MS-FRS2_R647");
            this.Manager.Checkpoint("MS-FRS2_R655");
            this.Manager.Checkpoint("MS-FRS2_R656");
            this.Manager.Checkpoint("MS-FRS2_R657");
            this.Manager.Checkpoint("MS-FRS2_R658");
            this.Manager.Checkpoint("MS-FRS2_R805");
            this.Manager.Comment("reaching state \'S356\'");
            this.Manager.Comment("checking step \'return RdcGetSignatures/ERROR_FAIL\'");
            this.Manager.Assert((temp58 == FRS2Model.error_status_t.ERROR_FAIL), String.Format("expected \'FRS2Model.error_status_t.ERROR_FAIL\', actual \'{0}\' (return of RdcGetSig" +
                        "natures, state S356)", TestManagerHelpers.Describe(temp58)));
            TCtestScenario3S372();
        }
        
        private void TCtestScenario3S2InitializeFileTransferAsyncEventChecker12(FRS2Model.ServerContext context, FRS2Model.RDC_Sig_Level rdcsigLevel, bool isEOF) {
            this.Manager.Comment("checking step \'event InitializeFileTransferAsyncEvent(ValidContext,forRAWFileLeve" +
                    "l,True)\'");
            this.Manager.Assert((context == FRS2Model.ServerContext.ValidContext), String.Format("expected \'FRS2Model.ServerContext.ValidContext\', actual \'{0}\' (context of Initial" +
                        "izeFileTransferAsyncEvent, state S178)", TestManagerHelpers.Describe(context)));
            this.Manager.Assert((rdcsigLevel == FRS2Model.RDC_Sig_Level.forRAWFileLevel), String.Format("expected \'FRS2Model.RDC_Sig_Level.forRAWFileLevel\', actual \'{0}\' (rdcsigLevel of " +
                        "InitializeFileTransferAsyncEvent, state S178)", TestManagerHelpers.Describe(rdcsigLevel)));
            this.Manager.Assert((isEOF == true), String.Format("expected \'true\', actual \'{0}\' (isEOF of InitializeFileTransferAsyncEvent, state S" +
                        "178)", TestManagerHelpers.Describe(isEOF)));
        }
        
        private void TCtestScenario3S228() {
            this.Manager.Comment("reaching state \'S228\'");
            FRS2Model.error_status_t temp59;
            this.Manager.Comment("executing step \'call RdcGetSignatures(valid)\'");
            temp59 = this.IFRS2ManagedAdapterInstance.RdcGetSignatures(FRS2Model.offset.valid);
            this.Manager.Checkpoint("MS-FRS2_R647");
            this.Manager.Checkpoint("MS-FRS2_R658");
            this.Manager.Checkpoint("MS-FRS2_R805");
            this.Manager.Comment("reaching state \'S357\'");
            this.Manager.Comment("checking step \'return RdcGetSignatures/ERROR_FAIL\'");
            this.Manager.Assert((temp59 == FRS2Model.error_status_t.ERROR_FAIL), String.Format("expected \'FRS2Model.error_status_t.ERROR_FAIL\', actual \'{0}\' (return of RdcGetSig" +
                        "natures, state S357)", TestManagerHelpers.Describe(temp59)));
            TCtestScenario3S371();
        }
        
        private void TCtestScenario3S2InitializeFileTransferAsyncEventChecker13(FRS2Model.ServerContext context, FRS2Model.RDC_Sig_Level rdcsigLevel, bool isEOF) {
            this.Manager.Comment("checking step \'event InitializeFileTransferAsyncEvent(InvalidContext,forRAWFileLe" +
                    "vel,False)\'");
            this.Manager.Assert((context == FRS2Model.ServerContext.InvalidContext), String.Format("expected \'FRS2Model.ServerContext.InvalidContext\', actual \'{0}\' (context of Initi" +
                        "alizeFileTransferAsyncEvent, state S178)", TestManagerHelpers.Describe(context)));
            this.Manager.Assert((rdcsigLevel == FRS2Model.RDC_Sig_Level.forRAWFileLevel), String.Format("expected \'FRS2Model.RDC_Sig_Level.forRAWFileLevel\', actual \'{0}\' (rdcsigLevel of " +
                        "InitializeFileTransferAsyncEvent, state S178)", TestManagerHelpers.Describe(rdcsigLevel)));
            this.Manager.Assert((isEOF == false), String.Format("expected \'false\', actual \'{0}\' (isEOF of InitializeFileTransferAsyncEvent, state " +
                        "S178)", TestManagerHelpers.Describe(isEOF)));
        }
        
        private void TCtestScenario3S2InitializeFileTransferAsyncEventChecker14(FRS2Model.ServerContext context, FRS2Model.RDC_Sig_Level rdcsigLevel, bool isEOF) {
            this.Manager.Comment("checking step \'event InitializeFileTransferAsyncEvent(InvalidContext,forRDCFileLe" +
                    "vel,True)\'");
            this.Manager.Assert((context == FRS2Model.ServerContext.InvalidContext), String.Format("expected \'FRS2Model.ServerContext.InvalidContext\', actual \'{0}\' (context of Initi" +
                        "alizeFileTransferAsyncEvent, state S178)", TestManagerHelpers.Describe(context)));
            this.Manager.Assert((rdcsigLevel == FRS2Model.RDC_Sig_Level.forRDCFileLevel), String.Format("expected \'FRS2Model.RDC_Sig_Level.forRDCFileLevel\', actual \'{0}\' (rdcsigLevel of " +
                        "InitializeFileTransferAsyncEvent, state S178)", TestManagerHelpers.Describe(rdcsigLevel)));
            this.Manager.Assert((isEOF == true), String.Format("expected \'true\', actual \'{0}\' (isEOF of InitializeFileTransferAsyncEvent, state S" +
                        "178)", TestManagerHelpers.Describe(isEOF)));
        }
        
        private void TCtestScenario3S230() {
            this.Manager.Comment("reaching state \'S230\'");
            FRS2Model.error_status_t temp60;
            this.Manager.Comment("executing step \'call RdcGetSignatures(valid)\'");
            temp60 = this.IFRS2ManagedAdapterInstance.RdcGetSignatures(FRS2Model.offset.valid);
            this.Manager.Checkpoint("MS-FRS2_R647");
            this.Manager.Checkpoint("MS-FRS2_R655");
            this.Manager.Checkpoint("MS-FRS2_R656");
            this.Manager.Checkpoint("MS-FRS2_R657");
            this.Manager.Checkpoint("MS-FRS2_R805");
            this.Manager.Comment("reaching state \'S358\'");
            this.Manager.Comment("checking step \'return RdcGetSignatures/ERROR_FAIL\'");
            this.Manager.Assert((temp60 == FRS2Model.error_status_t.ERROR_FAIL), String.Format("expected \'FRS2Model.error_status_t.ERROR_FAIL\', actual \'{0}\' (return of RdcGetSig" +
                        "natures, state S358)", TestManagerHelpers.Describe(temp60)));
            TCtestScenario3S370();
        }
        
        private void TCtestScenario3S2InitializeFileTransferAsyncEventChecker15(FRS2Model.ServerContext context, FRS2Model.RDC_Sig_Level rdcsigLevel, bool isEOF) {
            this.Manager.Comment("checking step \'event InitializeFileTransferAsyncEvent(ValidContext,forRDCFileLeve" +
                    "l,True)\'");
            this.Manager.Assert((context == FRS2Model.ServerContext.ValidContext), String.Format("expected \'FRS2Model.ServerContext.ValidContext\', actual \'{0}\' (context of Initial" +
                        "izeFileTransferAsyncEvent, state S178)", TestManagerHelpers.Describe(context)));
            this.Manager.Assert((rdcsigLevel == FRS2Model.RDC_Sig_Level.forRDCFileLevel), String.Format("expected \'FRS2Model.RDC_Sig_Level.forRDCFileLevel\', actual \'{0}\' (rdcsigLevel of " +
                        "InitializeFileTransferAsyncEvent, state S178)", TestManagerHelpers.Describe(rdcsigLevel)));
            this.Manager.Assert((isEOF == true), String.Format("expected \'true\', actual \'{0}\' (isEOF of InitializeFileTransferAsyncEvent, state S" +
                        "178)", TestManagerHelpers.Describe(isEOF)));
        }
        
        private void TCtestScenario3S2RequestUpdatesEventChecker2(FRS2Model.FilePresense present, FRS2Model.UPDATE_STATUS updateStatus) {
            this.Manager.Comment("checking step \'event RequestUpdatesEvent(fileExists,UPDATE_STATUS_DONE)\'");
            try {
                this.Manager.Assert((present == FRS2Model.FilePresense.fileExists), String.Format("expected \'FRS2Model.FilePresense.fileExists\', actual \'{0}\' (present of RequestUpd" +
                            "atesEvent, state S120)", TestManagerHelpers.Describe(present)));
                this.Manager.Assert((updateStatus == FRS2Model.UPDATE_STATUS.UPDATE_STATUS_DONE), String.Format("expected \'FRS2Model.UPDATE_STATUS.UPDATE_STATUS_DONE\', actual \'{0}\' (updateStatus" +
                            " of RequestUpdatesEvent, state S120)", TestManagerHelpers.Describe(updateStatus)));
            }
            catch (TransactionFailedException ) {
                this.Manager.Comment("This step would have covered MS-FRS2_R93, MS-FRS2_R94");
                throw;
            }
            this.Manager.Checkpoint("MS-FRS2_R93");
            this.Manager.Checkpoint("MS-FRS2_R94");
        }
        
        private void TCtestScenario3S2InitializeFileTransferAsyncEventChecker16(FRS2Model.ServerContext context, FRS2Model.RDC_Sig_Level rdcsigLevel, bool isEOF) {
            this.Manager.Comment("checking step \'event InitializeFileTransferAsyncEvent(ValidContext,forRDCFileLeve" +
                    "l,False)\'");
            this.Manager.Assert((context == FRS2Model.ServerContext.ValidContext), String.Format("expected \'FRS2Model.ServerContext.ValidContext\', actual \'{0}\' (context of Initial" +
                        "izeFileTransferAsyncEvent, state S179)", TestManagerHelpers.Describe(context)));
            this.Manager.Assert((rdcsigLevel == FRS2Model.RDC_Sig_Level.forRDCFileLevel), String.Format("expected \'FRS2Model.RDC_Sig_Level.forRDCFileLevel\', actual \'{0}\' (rdcsigLevel of " +
                        "InitializeFileTransferAsyncEvent, state S179)", TestManagerHelpers.Describe(rdcsigLevel)));
            this.Manager.Assert((isEOF == false), String.Format("expected \'false\', actual \'{0}\' (isEOF of InitializeFileTransferAsyncEvent, state " +
                        "S179)", TestManagerHelpers.Describe(isEOF)));
        }
        
        private void TCtestScenario3S2InitializeFileTransferAsyncEventChecker17(FRS2Model.ServerContext context, FRS2Model.RDC_Sig_Level rdcsigLevel, bool isEOF) {
            this.Manager.Comment("checking step \'event InitializeFileTransferAsyncEvent(ValidContext,forRAWFileLeve" +
                    "l,False)\'");
            this.Manager.Assert((context == FRS2Model.ServerContext.ValidContext), String.Format("expected \'FRS2Model.ServerContext.ValidContext\', actual \'{0}\' (context of Initial" +
                        "izeFileTransferAsyncEvent, state S179)", TestManagerHelpers.Describe(context)));
            this.Manager.Assert((rdcsigLevel == FRS2Model.RDC_Sig_Level.forRAWFileLevel), String.Format("expected \'FRS2Model.RDC_Sig_Level.forRAWFileLevel\', actual \'{0}\' (rdcsigLevel of " +
                        "InitializeFileTransferAsyncEvent, state S179)", TestManagerHelpers.Describe(rdcsigLevel)));
            this.Manager.Assert((isEOF == false), String.Format("expected \'false\', actual \'{0}\' (isEOF of InitializeFileTransferAsyncEvent, state " +
                        "S179)", TestManagerHelpers.Describe(isEOF)));
        }
        
        private void TCtestScenario3S2InitializeFileTransferAsyncEventChecker18(FRS2Model.ServerContext context, FRS2Model.RDC_Sig_Level rdcsigLevel, bool isEOF) {
            this.Manager.Comment("checking step \'event InitializeFileTransferAsyncEvent(InvalidContext,forRAWFileLe" +
                    "vel,False)\'");
            this.Manager.Assert((context == FRS2Model.ServerContext.InvalidContext), String.Format("expected \'FRS2Model.ServerContext.InvalidContext\', actual \'{0}\' (context of Initi" +
                        "alizeFileTransferAsyncEvent, state S179)", TestManagerHelpers.Describe(context)));
            this.Manager.Assert((rdcsigLevel == FRS2Model.RDC_Sig_Level.forRAWFileLevel), String.Format("expected \'FRS2Model.RDC_Sig_Level.forRAWFileLevel\', actual \'{0}\' (rdcsigLevel of " +
                        "InitializeFileTransferAsyncEvent, state S179)", TestManagerHelpers.Describe(rdcsigLevel)));
            this.Manager.Assert((isEOF == false), String.Format("expected \'false\', actual \'{0}\' (isEOF of InitializeFileTransferAsyncEvent, state " +
                        "S179)", TestManagerHelpers.Describe(isEOF)));
        }
        
        private void TCtestScenario3S2InitializeFileTransferAsyncEventChecker19(FRS2Model.ServerContext context, FRS2Model.RDC_Sig_Level rdcsigLevel, bool isEOF) {
            this.Manager.Comment("checking step \'event InitializeFileTransferAsyncEvent(InvalidContext,forRDCFileLe" +
                    "vel,True)\'");
            this.Manager.Assert((context == FRS2Model.ServerContext.InvalidContext), String.Format("expected \'FRS2Model.ServerContext.InvalidContext\', actual \'{0}\' (context of Initi" +
                        "alizeFileTransferAsyncEvent, state S179)", TestManagerHelpers.Describe(context)));
            this.Manager.Assert((rdcsigLevel == FRS2Model.RDC_Sig_Level.forRDCFileLevel), String.Format("expected \'FRS2Model.RDC_Sig_Level.forRDCFileLevel\', actual \'{0}\' (rdcsigLevel of " +
                        "InitializeFileTransferAsyncEvent, state S179)", TestManagerHelpers.Describe(rdcsigLevel)));
            this.Manager.Assert((isEOF == true), String.Format("expected \'true\', actual \'{0}\' (isEOF of InitializeFileTransferAsyncEvent, state S" +
                        "179)", TestManagerHelpers.Describe(isEOF)));
        }
        
        private void TCtestScenario3S235() {
            this.Manager.Comment("reaching state \'S235\'");
            FRS2Model.error_status_t temp66;
            this.Manager.Comment("executing step \'call RdcGetSignatures(valid)\'");
            temp66 = this.IFRS2ManagedAdapterInstance.RdcGetSignatures(FRS2Model.offset.valid);
            this.Manager.Checkpoint("MS-FRS2_R647");
            this.Manager.Checkpoint("MS-FRS2_R655");
            this.Manager.Checkpoint("MS-FRS2_R656");
            this.Manager.Checkpoint("MS-FRS2_R657");
            this.Manager.Checkpoint("MS-FRS2_R805");
            this.Manager.Comment("reaching state \'S360\'");
            this.Manager.Comment("checking step \'return RdcGetSignatures/ERROR_FAIL\'");
            this.Manager.Assert((temp66 == FRS2Model.error_status_t.ERROR_FAIL), String.Format("expected \'FRS2Model.error_status_t.ERROR_FAIL\', actual \'{0}\' (return of RdcGetSig" +
                        "natures, state S360)", TestManagerHelpers.Describe(temp66)));
            TCtestScenario3S377();
        }
        
        private void TCtestScenario3S2InitializeFileTransferAsyncEventChecker20(FRS2Model.ServerContext context, FRS2Model.RDC_Sig_Level rdcsigLevel, bool isEOF) {
            this.Manager.Comment("checking step \'event InitializeFileTransferAsyncEvent(ValidContext,forRDCFileLeve" +
                    "l,True)\'");
            this.Manager.Assert((context == FRS2Model.ServerContext.ValidContext), String.Format("expected \'FRS2Model.ServerContext.ValidContext\', actual \'{0}\' (context of Initial" +
                        "izeFileTransferAsyncEvent, state S179)", TestManagerHelpers.Describe(context)));
            this.Manager.Assert((rdcsigLevel == FRS2Model.RDC_Sig_Level.forRDCFileLevel), String.Format("expected \'FRS2Model.RDC_Sig_Level.forRDCFileLevel\', actual \'{0}\' (rdcsigLevel of " +
                        "InitializeFileTransferAsyncEvent, state S179)", TestManagerHelpers.Describe(rdcsigLevel)));
            this.Manager.Assert((isEOF == true), String.Format("expected \'true\', actual \'{0}\' (isEOF of InitializeFileTransferAsyncEvent, state S" +
                        "179)", TestManagerHelpers.Describe(isEOF)));
        }
        
        private void TCtestScenario3S2InitializeFileTransferAsyncEventChecker21(FRS2Model.ServerContext context, FRS2Model.RDC_Sig_Level rdcsigLevel, bool isEOF) {
            this.Manager.Comment("checking step \'event InitializeFileTransferAsyncEvent(InvalidContext,forRDCFileLe" +
                    "vel,False)\'");
            this.Manager.Assert((context == FRS2Model.ServerContext.InvalidContext), String.Format("expected \'FRS2Model.ServerContext.InvalidContext\', actual \'{0}\' (context of Initi" +
                        "alizeFileTransferAsyncEvent, state S179)", TestManagerHelpers.Describe(context)));
            this.Manager.Assert((rdcsigLevel == FRS2Model.RDC_Sig_Level.forRDCFileLevel), String.Format("expected \'FRS2Model.RDC_Sig_Level.forRDCFileLevel\', actual \'{0}\' (rdcsigLevel of " +
                        "InitializeFileTransferAsyncEvent, state S179)", TestManagerHelpers.Describe(rdcsigLevel)));
            this.Manager.Assert((isEOF == false), String.Format("expected \'false\', actual \'{0}\' (isEOF of InitializeFileTransferAsyncEvent, state " +
                        "S179)", TestManagerHelpers.Describe(isEOF)));
        }
        
        private void TCtestScenario3S2InitializeFileTransferAsyncEventChecker22(FRS2Model.ServerContext context, FRS2Model.RDC_Sig_Level rdcsigLevel, bool isEOF) {
            this.Manager.Comment("checking step \'event InitializeFileTransferAsyncEvent(InvalidContext,forRAWFileLe" +
                    "vel,True)\'");
            this.Manager.Assert((context == FRS2Model.ServerContext.InvalidContext), String.Format("expected \'FRS2Model.ServerContext.InvalidContext\', actual \'{0}\' (context of Initi" +
                        "alizeFileTransferAsyncEvent, state S179)", TestManagerHelpers.Describe(context)));
            this.Manager.Assert((rdcsigLevel == FRS2Model.RDC_Sig_Level.forRAWFileLevel), String.Format("expected \'FRS2Model.RDC_Sig_Level.forRAWFileLevel\', actual \'{0}\' (rdcsigLevel of " +
                        "InitializeFileTransferAsyncEvent, state S179)", TestManagerHelpers.Describe(rdcsigLevel)));
            this.Manager.Assert((isEOF == true), String.Format("expected \'true\', actual \'{0}\' (isEOF of InitializeFileTransferAsyncEvent, state S" +
                        "179)", TestManagerHelpers.Describe(isEOF)));
        }
        
        private void TCtestScenario3S238() {
            this.Manager.Comment("reaching state \'S238\'");
            FRS2Model.error_status_t temp67;
            this.Manager.Comment("executing step \'call RdcGetSignatures(valid)\'");
            temp67 = this.IFRS2ManagedAdapterInstance.RdcGetSignatures(FRS2Model.offset.valid);
            this.Manager.Checkpoint("MS-FRS2_R647");
            this.Manager.Checkpoint("MS-FRS2_R655");
            this.Manager.Checkpoint("MS-FRS2_R656");
            this.Manager.Checkpoint("MS-FRS2_R657");
            this.Manager.Checkpoint("MS-FRS2_R658");
            this.Manager.Checkpoint("MS-FRS2_R805");
            this.Manager.Comment("reaching state \'S361\'");
            this.Manager.Comment("checking step \'return RdcGetSignatures/ERROR_FAIL\'");
            this.Manager.Assert((temp67 == FRS2Model.error_status_t.ERROR_FAIL), String.Format("expected \'FRS2Model.error_status_t.ERROR_FAIL\', actual \'{0}\' (return of RdcGetSig" +
                        "natures, state S361)", TestManagerHelpers.Describe(temp67)));
            TCtestScenario3S375();
        }
        
        private void TCtestScenario3S2InitializeFileTransferAsyncEventChecker23(FRS2Model.ServerContext context, FRS2Model.RDC_Sig_Level rdcsigLevel, bool isEOF) {
            this.Manager.Comment("checking step \'event InitializeFileTransferAsyncEvent(ValidContext,forRAWFileLeve" +
                    "l,True)\'");
            this.Manager.Assert((context == FRS2Model.ServerContext.ValidContext), String.Format("expected \'FRS2Model.ServerContext.ValidContext\', actual \'{0}\' (context of Initial" +
                        "izeFileTransferAsyncEvent, state S179)", TestManagerHelpers.Describe(context)));
            this.Manager.Assert((rdcsigLevel == FRS2Model.RDC_Sig_Level.forRAWFileLevel), String.Format("expected \'FRS2Model.RDC_Sig_Level.forRAWFileLevel\', actual \'{0}\' (rdcsigLevel of " +
                        "InitializeFileTransferAsyncEvent, state S179)", TestManagerHelpers.Describe(rdcsigLevel)));
            this.Manager.Assert((isEOF == true), String.Format("expected \'true\', actual \'{0}\' (isEOF of InitializeFileTransferAsyncEvent, state S" +
                        "179)", TestManagerHelpers.Describe(isEOF)));
        }
        
        private void TCtestScenario3S239() {
            this.Manager.Comment("reaching state \'S239\'");
            FRS2Model.error_status_t temp68;
            this.Manager.Comment("executing step \'call RdcGetSignatures(valid)\'");
            temp68 = this.IFRS2ManagedAdapterInstance.RdcGetSignatures(FRS2Model.offset.valid);
            this.Manager.Checkpoint("MS-FRS2_R647");
            this.Manager.Checkpoint("MS-FRS2_R658");
            this.Manager.Checkpoint("MS-FRS2_R805");
            this.Manager.Comment("reaching state \'S362\'");
            this.Manager.Comment("checking step \'return RdcGetSignatures/ERROR_FAIL\'");
            this.Manager.Assert((temp68 == FRS2Model.error_status_t.ERROR_FAIL), String.Format("expected \'FRS2Model.error_status_t.ERROR_FAIL\', actual \'{0}\' (return of RdcGetSig" +
                        "natures, state S362)", TestManagerHelpers.Describe(temp68)));
            TCtestScenario3S374();
        }
        
        private void TCtestScenario3S2RequestUpdatesEventChecker3(FRS2Model.FilePresense present, FRS2Model.UPDATE_STATUS updateStatus) {
            this.Manager.Comment("checking step \'event RequestUpdatesEvent(fileDeleted,UPDATE_STATUS_MORE)\'");
            try {
                this.Manager.Assert((present == FRS2Model.FilePresense.fileDeleted), String.Format("expected \'FRS2Model.FilePresense.fileDeleted\', actual \'{0}\' (present of RequestUp" +
                            "datesEvent, state S120)", TestManagerHelpers.Describe(present)));
                this.Manager.Assert((updateStatus == FRS2Model.UPDATE_STATUS.UPDATE_STATUS_MORE), String.Format("expected \'FRS2Model.UPDATE_STATUS.UPDATE_STATUS_MORE\', actual \'{0}\' (updateStatus" +
                            " of RequestUpdatesEvent, state S120)", TestManagerHelpers.Describe(updateStatus)));
            }
            catch (TransactionFailedException ) {
                this.Manager.Comment("This step would have covered MS-FRS2_R93");
                throw;
            }
            this.Manager.Checkpoint("MS-FRS2_R93");
        }
        
        private void TCtestScenario3S2AsyncPollResponseEventChecker1(FRS2Model.VVGeneration vvGen) {
            this.Manager.Comment("checking step \'event AsyncPollResponseEvent(InvalidValue)\'");
            try {
                this.Manager.Assert((vvGen == FRS2Model.VVGeneration.InvalidValue), String.Format("expected \'FRS2Model.VVGeneration.InvalidValue\', actual \'{0}\' (vvGen of AsyncPollR" +
                            "esponseEvent, state S88)", TestManagerHelpers.Describe(vvGen)));
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
        [Microsoft.VisualStudio.TestTools.UnitTesting.DescriptionAttribute(@"MS-FRS2_R37, MS-FRS2_R436, MS-FRS2_R439, MS-FRS2_R455, MS-FRS2_R458, MS-FRS2_R448, MS-FRS2_R549, MS-FRS2_R552, MS-FRS2_R517, MS-FRS2_R520, MS-FRS2_R466, MS-FRS2_R556, MS-FRS2_R1020, MS-FRS2_R555, MS-FRS2_R486, MS-FRS2_R489, MS-FRS2_R93, MS-FRS2_R94, MS-FRS2_R774, MS-FRS2_R777, MS-FRS2_R793, MS-FRS2_R646, MS-FRS2_R649, MS-FRS2_R650, MS-FRS2_R93, MS-FRS2_R94, MS-FRS2_R774, MS-FRS2_R777, MS-FRS2_R793, MS-FRS2_R646, MS-FRS2_R649, MS-FRS2_R650, MS-FRS2_R93, MS-FRS2_R498, MS-FRS2_R774, MS-FRS2_R777, MS-FRS2_R793, MS-FRS2_R646, MS-FRS2_R649, MS-FRS2_R650, MS-FRS2_R93, MS-FRS2_R556, MS-FRS2_R1020, MS-FRS2_R555")]
        public virtual void FRS2_TCtestScenario3S4() {
            this.Manager.BeginTest("TCtestScenario3S4");
            this.Manager.Comment("reaching state \'S4\'");
            FRS2Model.error_status_t temp72;
            this.Manager.Comment(@"executing step 'call Initialization(Windows7,Map{1=>enabled,2=>enabled,3=>enabled,4=>disabled},Map{1=>exists,2=>exists,3=>exists,4=>exists},Map{""A""=>1,""B""=>2,""C""=>3,""D""=>4},Map{""A""=>sysvol,""B""=>protection,""C""=>sysvol,""D""=>sysvol},Map{1=>""A"",2=>""B"",3=>""C"",4=>""D""},Map{1=>writeOnly,2=>readWrite,3=>readOnly,4=>readWrite},Map{1=>enabled,2=>disabled,3=>enabled,4=>enabled})'");
            temp72 = this.IFRS2ManagedAdapterInstance.Initialization(FRS2Model.OSVersion.Windows7, new Microsoft.Modeling.Map<System.Int32,FRS2Model.connectionProperty>().Add(1, FRS2Model.connectionProperty.enabled).Add(2, FRS2Model.connectionProperty.enabled).Add(3, FRS2Model.connectionProperty.enabled).Add(4, FRS2Model.connectionProperty.disabled), new Microsoft.Modeling.Map<System.Int32,FRS2Model.FromServer>().Add(1, FRS2Model.FromServer.exists).Add(2, FRS2Model.FromServer.exists).Add(3, FRS2Model.FromServer.exists).Add(4, FRS2Model.FromServer.exists), new Microsoft.Modeling.Map<System.String,System.Int32>().Add("A", 1).Add("B", 2).Add("C", 3).Add("D", 4), new Microsoft.Modeling.Map<System.String,FRS2Model.ReplicationGroupTypes>().Add("A", FRS2Model.ReplicationGroupTypes.sysvol).Add("B", FRS2Model.ReplicationGroupTypes.protection).Add("C", FRS2Model.ReplicationGroupTypes.sysvol).Add("D", FRS2Model.ReplicationGroupTypes.sysvol), new Microsoft.Modeling.Map<System.Int32,System.String>().Add(1, "A").Add(2, "B").Add(3, "C").Add(4, "D"), new Microsoft.Modeling.Map<System.Int32,FRS2Model.accessLevels>().Add(1, FRS2Model.accessLevels.writeOnly).Add(2, FRS2Model.accessLevels.readWrite).Add(3, FRS2Model.accessLevels.readOnly).Add(4, FRS2Model.accessLevels.readWrite), new Microsoft.Modeling.Map<System.Int32,FRS2Model.connectionProperty>().Add(1, FRS2Model.connectionProperty.enabled).Add(2, FRS2Model.connectionProperty.disabled).Add(3, FRS2Model.connectionProperty.enabled).Add(4, FRS2Model.connectionProperty.enabled));
            this.Manager.Comment("reaching state \'S5\'");
            this.Manager.Comment("checking step \'return Initialization/ERROR_SUCCESS\'");
            this.Manager.Assert((temp72 == FRS2Model.error_status_t.ERROR_SUCCESS), String.Format("expected \'FRS2Model.error_status_t.ERROR_SUCCESS\', actual \'{0}\' (return of Initia" +
                        "lization, state S5)", TestManagerHelpers.Describe(temp72)));
            this.Manager.Comment("reaching state \'S20\'");
            FRS2Model.ProtocolVersionReturned temp73;
            FRS2Model.UpstreamFlagValueReturned temp74;
            FRS2Model.error_status_t temp75;
            this.Manager.Comment("executing step \'call EstablishConnection(\"A\",1,FRS_COMMUNICATION_PROTOCOL_VERSION" +
                    "_LONGHORN_SERVER,out _,out _)\'");
            temp75 = this.IFRS2ManagedAdapterInstance.EstablishConnection("A", 1, FRS2Model.ProtocolVersion.FRS_COMMUNICATION_PROTOCOL_VERSION_LONGHORN_SERVER, out temp73, out temp74);
            this.Manager.Checkpoint("MS-FRS2_R37");
            this.Manager.Checkpoint("MS-FRS2_R436");
            this.Manager.Checkpoint("MS-FRS2_R439");
            this.Manager.Comment("reaching state \'S29\'");
            this.Manager.Comment("checking step \'return EstablishConnection/[out Valid,out Valid]:ERROR_SUCCESS\'");
            this.Manager.Assert((temp73 == FRS2Model.ProtocolVersionReturned.Valid), String.Format("expected \'FRS2Model.ProtocolVersionReturned.Valid\', actual \'{0}\' (upstreamProtoco" +
                        "lVersion of EstablishConnection, state S29)", TestManagerHelpers.Describe(temp73)));
            this.Manager.Assert((temp74 == FRS2Model.UpstreamFlagValueReturned.Valid), String.Format("expected \'FRS2Model.UpstreamFlagValueReturned.Valid\', actual \'{0}\' (upstreamFlags" +
                        " of EstablishConnection, state S29)", TestManagerHelpers.Describe(temp74)));
            this.Manager.Assert((temp75 == FRS2Model.error_status_t.ERROR_SUCCESS), String.Format("expected \'FRS2Model.error_status_t.ERROR_SUCCESS\', actual \'{0}\' (return of Establ" +
                        "ishConnection, state S29)", TestManagerHelpers.Describe(temp75)));
            this.Manager.Comment("reaching state \'S38\'");
            FRS2Model.error_status_t temp76;
            this.Manager.Comment("executing step \'call EstablishSession(1,1)\'");
            temp76 = this.IFRS2ManagedAdapterInstance.EstablishSession(1, 1);
            this.Manager.Checkpoint("MS-FRS2_R455");
            this.Manager.Checkpoint("MS-FRS2_R458");
            this.Manager.Checkpoint("MS-FRS2_R448");
            this.Manager.Comment("reaching state \'S47\'");
            this.Manager.Comment("checking step \'return EstablishSession/ERROR_SUCCESS\'");
            this.Manager.Assert((temp76 == FRS2Model.error_status_t.ERROR_SUCCESS), String.Format("expected \'FRS2Model.error_status_t.ERROR_SUCCESS\', actual \'{0}\' (return of Establ" +
                        "ishSession, state S47)", TestManagerHelpers.Describe(temp76)));
            this.Manager.Comment("reaching state \'S56\'");
            FRS2Model.error_status_t temp77;
            this.Manager.Comment("executing step \'call AsyncPoll(1)\'");
            temp77 = this.IFRS2ManagedAdapterInstance.AsyncPoll(1);
            this.Manager.Checkpoint("MS-FRS2_R549");
            this.Manager.Checkpoint("MS-FRS2_R552");
            this.Manager.Comment("reaching state \'S65\'");
            this.Manager.Comment("checking step \'return AsyncPoll/ERROR_SUCCESS\'");
            this.Manager.Assert((temp77 == FRS2Model.error_status_t.ERROR_SUCCESS), String.Format("expected \'FRS2Model.error_status_t.ERROR_SUCCESS\', actual \'{0}\' (return of AsyncP" +
                        "oll, state S65)", TestManagerHelpers.Describe(temp77)));
            this.Manager.Comment("reaching state \'S73\'");
            FRS2Model.error_status_t temp78;
            this.Manager.Comment("executing step \'call RequestVersionVector(1,1,1,REQUEST_NORMAL_SYNC,CHANGE_ALL,Va" +
                    "lidValue)\'");
            temp78 = this.IFRS2ManagedAdapterInstance.RequestVersionVector(1, 1, 1, FRS2Model.VERSION_REQUEST_TYPE.REQUEST_NORMAL_SYNC, FRS2Model.VERSION_CHANGE_TYPE.CHANGE_ALL, FRS2Model.VVGeneration.ValidValue);
            this.Manager.Checkpoint("MS-FRS2_R517");
            this.Manager.Checkpoint("MS-FRS2_R520");
            this.Manager.Checkpoint("MS-FRS2_R466");
            this.Manager.Comment("reaching state \'S81\'");
            this.Manager.Comment("checking step \'return RequestVersionVector/ERROR_SUCCESS\'");
            this.Manager.Assert((temp78 == FRS2Model.error_status_t.ERROR_SUCCESS), String.Format("expected \'FRS2Model.error_status_t.ERROR_SUCCESS\', actual \'{0}\' (return of Reques" +
                        "tVersionVector, state S81)", TestManagerHelpers.Describe(temp78)));
            this.Manager.Comment("reaching state \'S89\'");
            int temp93 = this.Manager.ExpectEvent(this.QuiescenceTimeout, true, new ExpectedEvent(TCtestScenario3.AsyncPollResponseEventInfo, null, new AsyncPollResponseEventDelegate1(this.TCtestScenario3S4AsyncPollResponseEventChecker)), new ExpectedEvent(TCtestScenario3.AsyncPollResponseEventInfo, null, new AsyncPollResponseEventDelegate1(this.TCtestScenario3S4AsyncPollResponseEventChecker1)));
            if ((temp93 == 0)) {
                this.Manager.Comment("reaching state \'S99\'");
                FRS2Model.error_status_t temp79;
                this.Manager.Comment("executing step \'call RequestUpdates(1,1,valid)\'");
                temp79 = this.IFRS2ManagedAdapterInstance.RequestUpdates(1, 1, FRS2Model.versionVectorDiff.valid);
                this.Manager.Checkpoint("MS-FRS2_R486");
                this.Manager.Checkpoint("MS-FRS2_R489");
                this.Manager.Comment("reaching state \'S113\'");
                this.Manager.Comment("checking step \'return RequestUpdates/ERROR_SUCCESS\'");
                this.Manager.Assert((temp79 == FRS2Model.error_status_t.ERROR_SUCCESS), String.Format("expected \'FRS2Model.error_status_t.ERROR_SUCCESS\', actual \'{0}\' (return of Reques" +
                            "tUpdates, state S113)", TestManagerHelpers.Describe(temp79)));
                this.Manager.Comment("reaching state \'S121\'");
                int temp92 = this.Manager.ExpectEvent(this.QuiescenceTimeout, true, new ExpectedEvent(TCtestScenario3.RequestUpdatesEventInfo, null, new RequestUpdatesEventDelegate1(this.TCtestScenario3S4RequestUpdatesEventChecker)), new ExpectedEvent(TCtestScenario3.RequestUpdatesEventInfo, null, new RequestUpdatesEventDelegate1(this.TCtestScenario3S4RequestUpdatesEventChecker1)), new ExpectedEvent(TCtestScenario3.RequestUpdatesEventInfo, null, new RequestUpdatesEventDelegate1(this.TCtestScenario3S4RequestUpdatesEventChecker2)), new ExpectedEvent(TCtestScenario3.RequestUpdatesEventInfo, null, new RequestUpdatesEventDelegate1(this.TCtestScenario3S4RequestUpdatesEventChecker3)));
                if ((temp92 == 0)) {
                    this.Manager.Comment("reaching state \'S135\'");
                    FRS2Model.error_status_t temp80;
                    this.Manager.Comment("executing step \'call InitializeFileTransferAsync(1,1,True)\'");
                    temp80 = this.IFRS2ManagedAdapterInstance.InitializeFileTransferAsync(1, 1, true);
                    this.Manager.Checkpoint("MS-FRS2_R774");
                    this.Manager.Checkpoint("MS-FRS2_R777");
                    this.Manager.Checkpoint("MS-FRS2_R793");
                    this.Manager.Comment("reaching state \'S162\'");
                    this.Manager.Comment("checking step \'return InitializeFileTransferAsync/ERROR_SUCCESS\'");
                    this.Manager.Assert((temp80 == FRS2Model.error_status_t.ERROR_SUCCESS), String.Format("expected \'FRS2Model.error_status_t.ERROR_SUCCESS\', actual \'{0}\' (return of Initia" +
                                "lizeFileTransferAsync, state S162)", TestManagerHelpers.Describe(temp80)));
                    this.Manager.Comment("reaching state \'S180\'");
                    int temp83 = this.Manager.ExpectEvent(this.QuiescenceTimeout, true, new ExpectedEvent(TCtestScenario3.InitializeFileTransferAsyncEventInfo, null, new InitializeFileTransferAsyncEventDelegate1(this.TCtestScenario3S4InitializeFileTransferAsyncEventChecker)), new ExpectedEvent(TCtestScenario3.InitializeFileTransferAsyncEventInfo, null, new InitializeFileTransferAsyncEventDelegate1(this.TCtestScenario3S4InitializeFileTransferAsyncEventChecker1)), new ExpectedEvent(TCtestScenario3.InitializeFileTransferAsyncEventInfo, null, new InitializeFileTransferAsyncEventDelegate1(this.TCtestScenario3S4InitializeFileTransferAsyncEventChecker2)), new ExpectedEvent(TCtestScenario3.InitializeFileTransferAsyncEventInfo, null, new InitializeFileTransferAsyncEventDelegate1(this.TCtestScenario3S4InitializeFileTransferAsyncEventChecker3)), new ExpectedEvent(TCtestScenario3.InitializeFileTransferAsyncEventInfo, null, new InitializeFileTransferAsyncEventDelegate1(this.TCtestScenario3S4InitializeFileTransferAsyncEventChecker4)), new ExpectedEvent(TCtestScenario3.InitializeFileTransferAsyncEventInfo, null, new InitializeFileTransferAsyncEventDelegate1(this.TCtestScenario3S4InitializeFileTransferAsyncEventChecker5)), new ExpectedEvent(TCtestScenario3.InitializeFileTransferAsyncEventInfo, null, new InitializeFileTransferAsyncEventDelegate1(this.TCtestScenario3S4InitializeFileTransferAsyncEventChecker6)), new ExpectedEvent(TCtestScenario3.InitializeFileTransferAsyncEventInfo, null, new InitializeFileTransferAsyncEventDelegate1(this.TCtestScenario3S4InitializeFileTransferAsyncEventChecker7)));
                    if ((temp83 == 0)) {
                        this.Manager.Comment("reaching state \'S240\'");
                        FRS2Model.error_status_t temp81;
                        this.Manager.Comment("executing step \'call RdcGetSignatures(valid)\'");
                        temp81 = this.IFRS2ManagedAdapterInstance.RdcGetSignatures(FRS2Model.offset.valid);
                        this.Manager.Checkpoint("MS-FRS2_R646");
                        this.Manager.Checkpoint("MS-FRS2_R649");
                        this.Manager.Checkpoint("MS-FRS2_R650");
                        this.Manager.Comment("reaching state \'S363\'");
                        this.Manager.Comment("checking step \'return RdcGetSignatures/ERROR_SUCCESS\'");
                        this.Manager.Assert((temp81 == FRS2Model.error_status_t.ERROR_SUCCESS), String.Format("expected \'FRS2Model.error_status_t.ERROR_SUCCESS\', actual \'{0}\' (return of RdcGet" +
                                    "Signatures, state S363)", TestManagerHelpers.Describe(temp81)));
                        this.Manager.Comment("reaching state \'S396\'");
                        FRS2Model.error_status_t temp82;
                        this.Manager.Comment("executing step \'call RdcGetFileData(inValidBufSize)\'");
                        temp82 = this.IFRS2ManagedAdapterInstance.RdcGetFileData(FRS2Model.BufferSize.inValidBufSize);
                        this.Manager.Comment("reaching state \'S408\'");
                        this.Manager.Comment("checking step \'return RdcGetFileData/ERROR_FAIL\'");
                        this.Manager.Assert((temp82 == FRS2Model.error_status_t.ERROR_FAIL), String.Format("expected \'FRS2Model.error_status_t.ERROR_FAIL\', actual \'{0}\' (return of RdcGetFil" +
                                    "eData, state S408)", TestManagerHelpers.Describe(temp82)));
                        this.Manager.Comment("reaching state \'S420\'");
                        goto label10;
                    }
                    if ((temp83 == 1)) {
                        TCtestScenario3S199();
                        goto label10;
                    }
                    if ((temp83 == 2)) {
                        TCtestScenario3S197();
                        goto label10;
                    }
                    if ((temp83 == 3)) {
                        TCtestScenario3S227();
                        goto label10;
                    }
                    if ((temp83 == 4)) {
                        TCtestScenario3S228();
                        goto label10;
                    }
                    if ((temp83 == 5)) {
                        TCtestScenario3S194();
                        goto label10;
                    }
                    if ((temp83 == 6)) {
                        TCtestScenario3S230();
                        goto label10;
                    }
                    if ((temp83 == 7)) {
                        TCtestScenario3S192();
                        goto label10;
                    }
                    throw new InvalidOperationException("never reached");
                label10:
;
                    goto label13;
                }
                if ((temp92 == 1)) {
                    this.Manager.Comment("reaching state \'S136\'");
                    FRS2Model.error_status_t temp84;
                    this.Manager.Comment("executing step \'call InitializeFileTransferAsync(1,1,True)\'");
                    temp84 = this.IFRS2ManagedAdapterInstance.InitializeFileTransferAsync(1, 1, true);
                    this.Manager.Checkpoint("MS-FRS2_R774");
                    this.Manager.Checkpoint("MS-FRS2_R777");
                    this.Manager.Checkpoint("MS-FRS2_R793");
                    this.Manager.Comment("reaching state \'S163\'");
                    this.Manager.Comment("checking step \'return InitializeFileTransferAsync/ERROR_SUCCESS\'");
                    this.Manager.Assert((temp84 == FRS2Model.error_status_t.ERROR_SUCCESS), String.Format("expected \'FRS2Model.error_status_t.ERROR_SUCCESS\', actual \'{0}\' (return of Initia" +
                                "lizeFileTransferAsync, state S163)", TestManagerHelpers.Describe(temp84)));
                    this.Manager.Comment("reaching state \'S181\'");
                    int temp87 = this.Manager.ExpectEvent(this.QuiescenceTimeout, true, new ExpectedEvent(TCtestScenario3.InitializeFileTransferAsyncEventInfo, null, new InitializeFileTransferAsyncEventDelegate1(this.TCtestScenario3S4InitializeFileTransferAsyncEventChecker8)), new ExpectedEvent(TCtestScenario3.InitializeFileTransferAsyncEventInfo, null, new InitializeFileTransferAsyncEventDelegate1(this.TCtestScenario3S4InitializeFileTransferAsyncEventChecker9)), new ExpectedEvent(TCtestScenario3.InitializeFileTransferAsyncEventInfo, null, new InitializeFileTransferAsyncEventDelegate1(this.TCtestScenario3S4InitializeFileTransferAsyncEventChecker10)), new ExpectedEvent(TCtestScenario3.InitializeFileTransferAsyncEventInfo, null, new InitializeFileTransferAsyncEventDelegate1(this.TCtestScenario3S4InitializeFileTransferAsyncEventChecker11)), new ExpectedEvent(TCtestScenario3.InitializeFileTransferAsyncEventInfo, null, new InitializeFileTransferAsyncEventDelegate1(this.TCtestScenario3S4InitializeFileTransferAsyncEventChecker12)), new ExpectedEvent(TCtestScenario3.InitializeFileTransferAsyncEventInfo, null, new InitializeFileTransferAsyncEventDelegate1(this.TCtestScenario3S4InitializeFileTransferAsyncEventChecker13)), new ExpectedEvent(TCtestScenario3.InitializeFileTransferAsyncEventInfo, null, new InitializeFileTransferAsyncEventDelegate1(this.TCtestScenario3S4InitializeFileTransferAsyncEventChecker14)), new ExpectedEvent(TCtestScenario3.InitializeFileTransferAsyncEventInfo, null, new InitializeFileTransferAsyncEventDelegate1(this.TCtestScenario3S4InitializeFileTransferAsyncEventChecker15)));
                    if ((temp87 == 0)) {
                        this.Manager.Comment("reaching state \'S248\'");
                        FRS2Model.error_status_t temp85;
                        this.Manager.Comment("executing step \'call RdcGetSignatures(valid)\'");
                        temp85 = this.IFRS2ManagedAdapterInstance.RdcGetSignatures(FRS2Model.offset.valid);
                        this.Manager.Checkpoint("MS-FRS2_R646");
                        this.Manager.Checkpoint("MS-FRS2_R649");
                        this.Manager.Checkpoint("MS-FRS2_R650");
                        this.Manager.Comment("reaching state \'S364\'");
                        this.Manager.Comment("checking step \'return RdcGetSignatures/ERROR_SUCCESS\'");
                        this.Manager.Assert((temp85 == FRS2Model.error_status_t.ERROR_SUCCESS), String.Format("expected \'FRS2Model.error_status_t.ERROR_SUCCESS\', actual \'{0}\' (return of RdcGet" +
                                    "Signatures, state S364)", TestManagerHelpers.Describe(temp85)));
                        this.Manager.Comment("reaching state \'S397\'");
                        FRS2Model.error_status_t temp86;
                        this.Manager.Comment("executing step \'call RdcGetFileData(inValidBufSize)\'");
                        temp86 = this.IFRS2ManagedAdapterInstance.RdcGetFileData(FRS2Model.BufferSize.inValidBufSize);
                        this.Manager.Comment("reaching state \'S409\'");
                        this.Manager.Comment("checking step \'return RdcGetFileData/ERROR_FAIL\'");
                        this.Manager.Assert((temp86 == FRS2Model.error_status_t.ERROR_FAIL), String.Format("expected \'FRS2Model.error_status_t.ERROR_FAIL\', actual \'{0}\' (return of RdcGetFil" +
                                    "eData, state S409)", TestManagerHelpers.Describe(temp86)));
                        this.Manager.Comment("reaching state \'S421\'");
                        goto label11;
                    }
                    if ((temp87 == 1)) {
                        TCtestScenario3S207();
                        goto label11;
                    }
                    if ((temp87 == 2)) {
                        TCtestScenario3S206();
                        goto label11;
                    }
                    if ((temp87 == 3)) {
                        TCtestScenario3S235();
                        goto label11;
                    }
                    if ((temp87 == 4)) {
                        TCtestScenario3S203();
                        goto label11;
                    }
                    if ((temp87 == 5)) {
                        TCtestScenario3S202();
                        goto label11;
                    }
                    if ((temp87 == 6)) {
                        TCtestScenario3S238();
                        goto label11;
                    }
                    if ((temp87 == 7)) {
                        TCtestScenario3S239();
                        goto label11;
                    }
                    throw new InvalidOperationException("never reached");
                label11:
;
                    goto label13;
                }
                if ((temp92 == 2)) {
                    this.Manager.Comment("reaching state \'S137\'");
                    FRS2Model.error_status_t temp88;
                    this.Manager.Comment("executing step \'call InitializeFileTransferAsync(1,1,True)\'");
                    temp88 = this.IFRS2ManagedAdapterInstance.InitializeFileTransferAsync(1, 1, true);
                    this.Manager.Checkpoint("MS-FRS2_R774");
                    this.Manager.Checkpoint("MS-FRS2_R777");
                    this.Manager.Checkpoint("MS-FRS2_R793");
                    this.Manager.Comment("reaching state \'S164\'");
                    this.Manager.Comment("checking step \'return InitializeFileTransferAsync/ERROR_SUCCESS\'");
                    this.Manager.Assert((temp88 == FRS2Model.error_status_t.ERROR_SUCCESS), String.Format("expected \'FRS2Model.error_status_t.ERROR_SUCCESS\', actual \'{0}\' (return of Initia" +
                                "lizeFileTransferAsync, state S164)", TestManagerHelpers.Describe(temp88)));
                    this.Manager.Comment("reaching state \'S182\'");
                    int temp91 = this.Manager.ExpectEvent(this.QuiescenceTimeout, true, new ExpectedEvent(TCtestScenario3.InitializeFileTransferAsyncEventInfo, null, new InitializeFileTransferAsyncEventDelegate1(this.TCtestScenario3S4InitializeFileTransferAsyncEventChecker16)), new ExpectedEvent(TCtestScenario3.InitializeFileTransferAsyncEventInfo, null, new InitializeFileTransferAsyncEventDelegate1(this.TCtestScenario3S4InitializeFileTransferAsyncEventChecker17)), new ExpectedEvent(TCtestScenario3.InitializeFileTransferAsyncEventInfo, null, new InitializeFileTransferAsyncEventDelegate1(this.TCtestScenario3S4InitializeFileTransferAsyncEventChecker18)), new ExpectedEvent(TCtestScenario3.InitializeFileTransferAsyncEventInfo, null, new InitializeFileTransferAsyncEventDelegate1(this.TCtestScenario3S4InitializeFileTransferAsyncEventChecker19)), new ExpectedEvent(TCtestScenario3.InitializeFileTransferAsyncEventInfo, null, new InitializeFileTransferAsyncEventDelegate1(this.TCtestScenario3S4InitializeFileTransferAsyncEventChecker20)), new ExpectedEvent(TCtestScenario3.InitializeFileTransferAsyncEventInfo, null, new InitializeFileTransferAsyncEventDelegate1(this.TCtestScenario3S4InitializeFileTransferAsyncEventChecker21)), new ExpectedEvent(TCtestScenario3.InitializeFileTransferAsyncEventInfo, null, new InitializeFileTransferAsyncEventDelegate1(this.TCtestScenario3S4InitializeFileTransferAsyncEventChecker22)), new ExpectedEvent(TCtestScenario3.InitializeFileTransferAsyncEventInfo, null, new InitializeFileTransferAsyncEventDelegate1(this.TCtestScenario3S4InitializeFileTransferAsyncEventChecker23)));
                    if ((temp91 == 0)) {
                        this.Manager.Comment("reaching state \'S256\'");
                        FRS2Model.error_status_t temp89;
                        this.Manager.Comment("executing step \'call RdcGetSignatures(valid)\'");
                        temp89 = this.IFRS2ManagedAdapterInstance.RdcGetSignatures(FRS2Model.offset.valid);
                        this.Manager.Checkpoint("MS-FRS2_R646");
                        this.Manager.Checkpoint("MS-FRS2_R649");
                        this.Manager.Checkpoint("MS-FRS2_R650");
                        this.Manager.Comment("reaching state \'S365\'");
                        this.Manager.Comment("checking step \'return RdcGetSignatures/ERROR_SUCCESS\'");
                        this.Manager.Assert((temp89 == FRS2Model.error_status_t.ERROR_SUCCESS), String.Format("expected \'FRS2Model.error_status_t.ERROR_SUCCESS\', actual \'{0}\' (return of RdcGet" +
                                    "Signatures, state S365)", TestManagerHelpers.Describe(temp89)));
                        this.Manager.Comment("reaching state \'S398\'");
                        FRS2Model.error_status_t temp90;
                        this.Manager.Comment("executing step \'call RdcGetFileData(inValidBufSize)\'");
                        temp90 = this.IFRS2ManagedAdapterInstance.RdcGetFileData(FRS2Model.BufferSize.inValidBufSize);
                        this.Manager.Comment("reaching state \'S410\'");
                        this.Manager.Comment("checking step \'return RdcGetFileData/ERROR_FAIL\'");
                        this.Manager.Assert((temp90 == FRS2Model.error_status_t.ERROR_FAIL), String.Format("expected \'FRS2Model.error_status_t.ERROR_FAIL\', actual \'{0}\' (return of RdcGetFil" +
                                    "eData, state S410)", TestManagerHelpers.Describe(temp90)));
                        this.Manager.Comment("reaching state \'S422\'");
                        goto label12;
                    }
                    if ((temp91 == 1)) {
                        TCtestScenario3S214();
                        goto label12;
                    }
                    if ((temp91 == 2)) {
                        TCtestScenario3S213();
                        goto label12;
                    }
                    if ((temp91 == 3)) {
                        TCtestScenario3S216();
                        goto label12;
                    }
                    if ((temp91 == 4)) {
                        TCtestScenario3S211();
                        goto label12;
                    }
                    if ((temp91 == 5)) {
                        TCtestScenario3S210();
                        goto label12;
                    }
                    if ((temp91 == 6)) {
                        TCtestScenario3S222();
                        goto label12;
                    }
                    if ((temp91 == 7)) {
                        TCtestScenario3S223();
                        goto label12;
                    }
                    throw new InvalidOperationException("never reached");
                label12:
;
                    goto label13;
                }
                if ((temp92 == 3)) {
                    TCtestScenario3S127();
                    goto label13;
                }
                throw new InvalidOperationException("never reached");
            label13:
;
                goto label14;
            }
            if ((temp93 == 1)) {
                TCtestScenario3S96();
                goto label14;
            }
            throw new InvalidOperationException("never reached");
        label14:
;
            this.Manager.EndTest();
        }
        
        private void TCtestScenario3S4AsyncPollResponseEventChecker(FRS2Model.VVGeneration vvGen) {
            this.Manager.Comment("checking step \'event AsyncPollResponseEvent(ValidValue)\'");
            try {
                this.Manager.Assert((vvGen == FRS2Model.VVGeneration.ValidValue), String.Format("expected \'FRS2Model.VVGeneration.ValidValue\', actual \'{0}\' (vvGen of AsyncPollRes" +
                            "ponseEvent, state S89)", TestManagerHelpers.Describe(vvGen)));
            }
            catch (TransactionFailedException ) {
                this.Manager.Comment("This step would have covered MS-FRS2_R556, MS-FRS2_R1020, MS-FRS2_R555");
                throw;
            }
            this.Manager.Checkpoint("MS-FRS2_R556");
            this.Manager.Checkpoint("MS-FRS2_R1020");
            this.Manager.Checkpoint("MS-FRS2_R555");
        }
        
        private void TCtestScenario3S4RequestUpdatesEventChecker(FRS2Model.FilePresense present, FRS2Model.UPDATE_STATUS updateStatus) {
            this.Manager.Comment("checking step \'event RequestUpdatesEvent(fileDeleted,UPDATE_STATUS_DONE)\'");
            try {
                this.Manager.Assert((present == FRS2Model.FilePresense.fileDeleted), String.Format("expected \'FRS2Model.FilePresense.fileDeleted\', actual \'{0}\' (present of RequestUp" +
                            "datesEvent, state S121)", TestManagerHelpers.Describe(present)));
                this.Manager.Assert((updateStatus == FRS2Model.UPDATE_STATUS.UPDATE_STATUS_DONE), String.Format("expected \'FRS2Model.UPDATE_STATUS.UPDATE_STATUS_DONE\', actual \'{0}\' (updateStatus" +
                            " of RequestUpdatesEvent, state S121)", TestManagerHelpers.Describe(updateStatus)));
            }
            catch (TransactionFailedException ) {
                this.Manager.Comment("This step would have covered MS-FRS2_R93, MS-FRS2_R94");
                throw;
            }
            this.Manager.Checkpoint("MS-FRS2_R93");
            this.Manager.Checkpoint("MS-FRS2_R94");
        }
        
        private void TCtestScenario3S4InitializeFileTransferAsyncEventChecker(FRS2Model.ServerContext context, FRS2Model.RDC_Sig_Level rdcsigLevel, bool isEOF) {
            this.Manager.Comment("checking step \'event InitializeFileTransferAsyncEvent(ValidContext,forRDCFileLeve" +
                    "l,False)\'");
            this.Manager.Assert((context == FRS2Model.ServerContext.ValidContext), String.Format("expected \'FRS2Model.ServerContext.ValidContext\', actual \'{0}\' (context of Initial" +
                        "izeFileTransferAsyncEvent, state S180)", TestManagerHelpers.Describe(context)));
            this.Manager.Assert((rdcsigLevel == FRS2Model.RDC_Sig_Level.forRDCFileLevel), String.Format("expected \'FRS2Model.RDC_Sig_Level.forRDCFileLevel\', actual \'{0}\' (rdcsigLevel of " +
                        "InitializeFileTransferAsyncEvent, state S180)", TestManagerHelpers.Describe(rdcsigLevel)));
            this.Manager.Assert((isEOF == false), String.Format("expected \'false\', actual \'{0}\' (isEOF of InitializeFileTransferAsyncEvent, state " +
                        "S180)", TestManagerHelpers.Describe(isEOF)));
        }
        
        private void TCtestScenario3S4InitializeFileTransferAsyncEventChecker1(FRS2Model.ServerContext context, FRS2Model.RDC_Sig_Level rdcsigLevel, bool isEOF) {
            this.Manager.Comment("checking step \'event InitializeFileTransferAsyncEvent(InvalidContext,forRDCFileLe" +
                    "vel,False)\'");
            this.Manager.Assert((context == FRS2Model.ServerContext.InvalidContext), String.Format("expected \'FRS2Model.ServerContext.InvalidContext\', actual \'{0}\' (context of Initi" +
                        "alizeFileTransferAsyncEvent, state S180)", TestManagerHelpers.Describe(context)));
            this.Manager.Assert((rdcsigLevel == FRS2Model.RDC_Sig_Level.forRDCFileLevel), String.Format("expected \'FRS2Model.RDC_Sig_Level.forRDCFileLevel\', actual \'{0}\' (rdcsigLevel of " +
                        "InitializeFileTransferAsyncEvent, state S180)", TestManagerHelpers.Describe(rdcsigLevel)));
            this.Manager.Assert((isEOF == false), String.Format("expected \'false\', actual \'{0}\' (isEOF of InitializeFileTransferAsyncEvent, state " +
                        "S180)", TestManagerHelpers.Describe(isEOF)));
        }
        
        private void TCtestScenario3S4InitializeFileTransferAsyncEventChecker2(FRS2Model.ServerContext context, FRS2Model.RDC_Sig_Level rdcsigLevel, bool isEOF) {
            this.Manager.Comment("checking step \'event InitializeFileTransferAsyncEvent(ValidContext,forRAWFileLeve" +
                    "l,False)\'");
            this.Manager.Assert((context == FRS2Model.ServerContext.ValidContext), String.Format("expected \'FRS2Model.ServerContext.ValidContext\', actual \'{0}\' (context of Initial" +
                        "izeFileTransferAsyncEvent, state S180)", TestManagerHelpers.Describe(context)));
            this.Manager.Assert((rdcsigLevel == FRS2Model.RDC_Sig_Level.forRAWFileLevel), String.Format("expected \'FRS2Model.RDC_Sig_Level.forRAWFileLevel\', actual \'{0}\' (rdcsigLevel of " +
                        "InitializeFileTransferAsyncEvent, state S180)", TestManagerHelpers.Describe(rdcsigLevel)));
            this.Manager.Assert((isEOF == false), String.Format("expected \'false\', actual \'{0}\' (isEOF of InitializeFileTransferAsyncEvent, state " +
                        "S180)", TestManagerHelpers.Describe(isEOF)));
        }
        
        private void TCtestScenario3S4InitializeFileTransferAsyncEventChecker3(FRS2Model.ServerContext context, FRS2Model.RDC_Sig_Level rdcsigLevel, bool isEOF) {
            this.Manager.Comment("checking step \'event InitializeFileTransferAsyncEvent(InvalidContext,forRAWFileLe" +
                    "vel,True)\'");
            this.Manager.Assert((context == FRS2Model.ServerContext.InvalidContext), String.Format("expected \'FRS2Model.ServerContext.InvalidContext\', actual \'{0}\' (context of Initi" +
                        "alizeFileTransferAsyncEvent, state S180)", TestManagerHelpers.Describe(context)));
            this.Manager.Assert((rdcsigLevel == FRS2Model.RDC_Sig_Level.forRAWFileLevel), String.Format("expected \'FRS2Model.RDC_Sig_Level.forRAWFileLevel\', actual \'{0}\' (rdcsigLevel of " +
                        "InitializeFileTransferAsyncEvent, state S180)", TestManagerHelpers.Describe(rdcsigLevel)));
            this.Manager.Assert((isEOF == true), String.Format("expected \'true\', actual \'{0}\' (isEOF of InitializeFileTransferAsyncEvent, state S" +
                        "180)", TestManagerHelpers.Describe(isEOF)));
        }
        
        private void TCtestScenario3S4InitializeFileTransferAsyncEventChecker4(FRS2Model.ServerContext context, FRS2Model.RDC_Sig_Level rdcsigLevel, bool isEOF) {
            this.Manager.Comment("checking step \'event InitializeFileTransferAsyncEvent(ValidContext,forRAWFileLeve" +
                    "l,True)\'");
            this.Manager.Assert((context == FRS2Model.ServerContext.ValidContext), String.Format("expected \'FRS2Model.ServerContext.ValidContext\', actual \'{0}\' (context of Initial" +
                        "izeFileTransferAsyncEvent, state S180)", TestManagerHelpers.Describe(context)));
            this.Manager.Assert((rdcsigLevel == FRS2Model.RDC_Sig_Level.forRAWFileLevel), String.Format("expected \'FRS2Model.RDC_Sig_Level.forRAWFileLevel\', actual \'{0}\' (rdcsigLevel of " +
                        "InitializeFileTransferAsyncEvent, state S180)", TestManagerHelpers.Describe(rdcsigLevel)));
            this.Manager.Assert((isEOF == true), String.Format("expected \'true\', actual \'{0}\' (isEOF of InitializeFileTransferAsyncEvent, state S" +
                        "180)", TestManagerHelpers.Describe(isEOF)));
        }
        
        private void TCtestScenario3S4InitializeFileTransferAsyncEventChecker5(FRS2Model.ServerContext context, FRS2Model.RDC_Sig_Level rdcsigLevel, bool isEOF) {
            this.Manager.Comment("checking step \'event InitializeFileTransferAsyncEvent(InvalidContext,forRAWFileLe" +
                    "vel,False)\'");
            this.Manager.Assert((context == FRS2Model.ServerContext.InvalidContext), String.Format("expected \'FRS2Model.ServerContext.InvalidContext\', actual \'{0}\' (context of Initi" +
                        "alizeFileTransferAsyncEvent, state S180)", TestManagerHelpers.Describe(context)));
            this.Manager.Assert((rdcsigLevel == FRS2Model.RDC_Sig_Level.forRAWFileLevel), String.Format("expected \'FRS2Model.RDC_Sig_Level.forRAWFileLevel\', actual \'{0}\' (rdcsigLevel of " +
                        "InitializeFileTransferAsyncEvent, state S180)", TestManagerHelpers.Describe(rdcsigLevel)));
            this.Manager.Assert((isEOF == false), String.Format("expected \'false\', actual \'{0}\' (isEOF of InitializeFileTransferAsyncEvent, state " +
                        "S180)", TestManagerHelpers.Describe(isEOF)));
        }
        
        private void TCtestScenario3S4InitializeFileTransferAsyncEventChecker6(FRS2Model.ServerContext context, FRS2Model.RDC_Sig_Level rdcsigLevel, bool isEOF) {
            this.Manager.Comment("checking step \'event InitializeFileTransferAsyncEvent(InvalidContext,forRDCFileLe" +
                    "vel,True)\'");
            this.Manager.Assert((context == FRS2Model.ServerContext.InvalidContext), String.Format("expected \'FRS2Model.ServerContext.InvalidContext\', actual \'{0}\' (context of Initi" +
                        "alizeFileTransferAsyncEvent, state S180)", TestManagerHelpers.Describe(context)));
            this.Manager.Assert((rdcsigLevel == FRS2Model.RDC_Sig_Level.forRDCFileLevel), String.Format("expected \'FRS2Model.RDC_Sig_Level.forRDCFileLevel\', actual \'{0}\' (rdcsigLevel of " +
                        "InitializeFileTransferAsyncEvent, state S180)", TestManagerHelpers.Describe(rdcsigLevel)));
            this.Manager.Assert((isEOF == true), String.Format("expected \'true\', actual \'{0}\' (isEOF of InitializeFileTransferAsyncEvent, state S" +
                        "180)", TestManagerHelpers.Describe(isEOF)));
        }
        
        private void TCtestScenario3S4InitializeFileTransferAsyncEventChecker7(FRS2Model.ServerContext context, FRS2Model.RDC_Sig_Level rdcsigLevel, bool isEOF) {
            this.Manager.Comment("checking step \'event InitializeFileTransferAsyncEvent(ValidContext,forRDCFileLeve" +
                    "l,True)\'");
            this.Manager.Assert((context == FRS2Model.ServerContext.ValidContext), String.Format("expected \'FRS2Model.ServerContext.ValidContext\', actual \'{0}\' (context of Initial" +
                        "izeFileTransferAsyncEvent, state S180)", TestManagerHelpers.Describe(context)));
            this.Manager.Assert((rdcsigLevel == FRS2Model.RDC_Sig_Level.forRDCFileLevel), String.Format("expected \'FRS2Model.RDC_Sig_Level.forRDCFileLevel\', actual \'{0}\' (rdcsigLevel of " +
                        "InitializeFileTransferAsyncEvent, state S180)", TestManagerHelpers.Describe(rdcsigLevel)));
            this.Manager.Assert((isEOF == true), String.Format("expected \'true\', actual \'{0}\' (isEOF of InitializeFileTransferAsyncEvent, state S" +
                        "180)", TestManagerHelpers.Describe(isEOF)));
        }
        
        private void TCtestScenario3S4RequestUpdatesEventChecker1(FRS2Model.FilePresense present, FRS2Model.UPDATE_STATUS updateStatus) {
            this.Manager.Comment("checking step \'event RequestUpdatesEvent(fileExists,UPDATE_STATUS_DONE)\'");
            try {
                this.Manager.Assert((present == FRS2Model.FilePresense.fileExists), String.Format("expected \'FRS2Model.FilePresense.fileExists\', actual \'{0}\' (present of RequestUpd" +
                            "atesEvent, state S121)", TestManagerHelpers.Describe(present)));
                this.Manager.Assert((updateStatus == FRS2Model.UPDATE_STATUS.UPDATE_STATUS_DONE), String.Format("expected \'FRS2Model.UPDATE_STATUS.UPDATE_STATUS_DONE\', actual \'{0}\' (updateStatus" +
                            " of RequestUpdatesEvent, state S121)", TestManagerHelpers.Describe(updateStatus)));
            }
            catch (TransactionFailedException ) {
                this.Manager.Comment("This step would have covered MS-FRS2_R93, MS-FRS2_R94");
                throw;
            }
            this.Manager.Checkpoint("MS-FRS2_R93");
            this.Manager.Checkpoint("MS-FRS2_R94");
        }
        
        private void TCtestScenario3S4InitializeFileTransferAsyncEventChecker8(FRS2Model.ServerContext context, FRS2Model.RDC_Sig_Level rdcsigLevel, bool isEOF) {
            this.Manager.Comment("checking step \'event InitializeFileTransferAsyncEvent(ValidContext,forRDCFileLeve" +
                    "l,False)\'");
            this.Manager.Assert((context == FRS2Model.ServerContext.ValidContext), String.Format("expected \'FRS2Model.ServerContext.ValidContext\', actual \'{0}\' (context of Initial" +
                        "izeFileTransferAsyncEvent, state S181)", TestManagerHelpers.Describe(context)));
            this.Manager.Assert((rdcsigLevel == FRS2Model.RDC_Sig_Level.forRDCFileLevel), String.Format("expected \'FRS2Model.RDC_Sig_Level.forRDCFileLevel\', actual \'{0}\' (rdcsigLevel of " +
                        "InitializeFileTransferAsyncEvent, state S181)", TestManagerHelpers.Describe(rdcsigLevel)));
            this.Manager.Assert((isEOF == false), String.Format("expected \'false\', actual \'{0}\' (isEOF of InitializeFileTransferAsyncEvent, state " +
                        "S181)", TestManagerHelpers.Describe(isEOF)));
        }
        
        private void TCtestScenario3S4InitializeFileTransferAsyncEventChecker9(FRS2Model.ServerContext context, FRS2Model.RDC_Sig_Level rdcsigLevel, bool isEOF) {
            this.Manager.Comment("checking step \'event InitializeFileTransferAsyncEvent(ValidContext,forRAWFileLeve" +
                    "l,False)\'");
            this.Manager.Assert((context == FRS2Model.ServerContext.ValidContext), String.Format("expected \'FRS2Model.ServerContext.ValidContext\', actual \'{0}\' (context of Initial" +
                        "izeFileTransferAsyncEvent, state S181)", TestManagerHelpers.Describe(context)));
            this.Manager.Assert((rdcsigLevel == FRS2Model.RDC_Sig_Level.forRAWFileLevel), String.Format("expected \'FRS2Model.RDC_Sig_Level.forRAWFileLevel\', actual \'{0}\' (rdcsigLevel of " +
                        "InitializeFileTransferAsyncEvent, state S181)", TestManagerHelpers.Describe(rdcsigLevel)));
            this.Manager.Assert((isEOF == false), String.Format("expected \'false\', actual \'{0}\' (isEOF of InitializeFileTransferAsyncEvent, state " +
                        "S181)", TestManagerHelpers.Describe(isEOF)));
        }
        
        private void TCtestScenario3S4InitializeFileTransferAsyncEventChecker10(FRS2Model.ServerContext context, FRS2Model.RDC_Sig_Level rdcsigLevel, bool isEOF) {
            this.Manager.Comment("checking step \'event InitializeFileTransferAsyncEvent(InvalidContext,forRAWFileLe" +
                    "vel,False)\'");
            this.Manager.Assert((context == FRS2Model.ServerContext.InvalidContext), String.Format("expected \'FRS2Model.ServerContext.InvalidContext\', actual \'{0}\' (context of Initi" +
                        "alizeFileTransferAsyncEvent, state S181)", TestManagerHelpers.Describe(context)));
            this.Manager.Assert((rdcsigLevel == FRS2Model.RDC_Sig_Level.forRAWFileLevel), String.Format("expected \'FRS2Model.RDC_Sig_Level.forRAWFileLevel\', actual \'{0}\' (rdcsigLevel of " +
                        "InitializeFileTransferAsyncEvent, state S181)", TestManagerHelpers.Describe(rdcsigLevel)));
            this.Manager.Assert((isEOF == false), String.Format("expected \'false\', actual \'{0}\' (isEOF of InitializeFileTransferAsyncEvent, state " +
                        "S181)", TestManagerHelpers.Describe(isEOF)));
        }
        
        private void TCtestScenario3S4InitializeFileTransferAsyncEventChecker11(FRS2Model.ServerContext context, FRS2Model.RDC_Sig_Level rdcsigLevel, bool isEOF) {
            this.Manager.Comment("checking step \'event InitializeFileTransferAsyncEvent(InvalidContext,forRDCFileLe" +
                    "vel,True)\'");
            this.Manager.Assert((context == FRS2Model.ServerContext.InvalidContext), String.Format("expected \'FRS2Model.ServerContext.InvalidContext\', actual \'{0}\' (context of Initi" +
                        "alizeFileTransferAsyncEvent, state S181)", TestManagerHelpers.Describe(context)));
            this.Manager.Assert((rdcsigLevel == FRS2Model.RDC_Sig_Level.forRDCFileLevel), String.Format("expected \'FRS2Model.RDC_Sig_Level.forRDCFileLevel\', actual \'{0}\' (rdcsigLevel of " +
                        "InitializeFileTransferAsyncEvent, state S181)", TestManagerHelpers.Describe(rdcsigLevel)));
            this.Manager.Assert((isEOF == true), String.Format("expected \'true\', actual \'{0}\' (isEOF of InitializeFileTransferAsyncEvent, state S" +
                        "181)", TestManagerHelpers.Describe(isEOF)));
        }
        
        private void TCtestScenario3S4InitializeFileTransferAsyncEventChecker12(FRS2Model.ServerContext context, FRS2Model.RDC_Sig_Level rdcsigLevel, bool isEOF) {
            this.Manager.Comment("checking step \'event InitializeFileTransferAsyncEvent(ValidContext,forRDCFileLeve" +
                    "l,True)\'");
            this.Manager.Assert((context == FRS2Model.ServerContext.ValidContext), String.Format("expected \'FRS2Model.ServerContext.ValidContext\', actual \'{0}\' (context of Initial" +
                        "izeFileTransferAsyncEvent, state S181)", TestManagerHelpers.Describe(context)));
            this.Manager.Assert((rdcsigLevel == FRS2Model.RDC_Sig_Level.forRDCFileLevel), String.Format("expected \'FRS2Model.RDC_Sig_Level.forRDCFileLevel\', actual \'{0}\' (rdcsigLevel of " +
                        "InitializeFileTransferAsyncEvent, state S181)", TestManagerHelpers.Describe(rdcsigLevel)));
            this.Manager.Assert((isEOF == true), String.Format("expected \'true\', actual \'{0}\' (isEOF of InitializeFileTransferAsyncEvent, state S" +
                        "181)", TestManagerHelpers.Describe(isEOF)));
        }
        
        private void TCtestScenario3S4InitializeFileTransferAsyncEventChecker13(FRS2Model.ServerContext context, FRS2Model.RDC_Sig_Level rdcsigLevel, bool isEOF) {
            this.Manager.Comment("checking step \'event InitializeFileTransferAsyncEvent(InvalidContext,forRDCFileLe" +
                    "vel,False)\'");
            this.Manager.Assert((context == FRS2Model.ServerContext.InvalidContext), String.Format("expected \'FRS2Model.ServerContext.InvalidContext\', actual \'{0}\' (context of Initi" +
                        "alizeFileTransferAsyncEvent, state S181)", TestManagerHelpers.Describe(context)));
            this.Manager.Assert((rdcsigLevel == FRS2Model.RDC_Sig_Level.forRDCFileLevel), String.Format("expected \'FRS2Model.RDC_Sig_Level.forRDCFileLevel\', actual \'{0}\' (rdcsigLevel of " +
                        "InitializeFileTransferAsyncEvent, state S181)", TestManagerHelpers.Describe(rdcsigLevel)));
            this.Manager.Assert((isEOF == false), String.Format("expected \'false\', actual \'{0}\' (isEOF of InitializeFileTransferAsyncEvent, state " +
                        "S181)", TestManagerHelpers.Describe(isEOF)));
        }
        
        private void TCtestScenario3S4InitializeFileTransferAsyncEventChecker14(FRS2Model.ServerContext context, FRS2Model.RDC_Sig_Level rdcsigLevel, bool isEOF) {
            this.Manager.Comment("checking step \'event InitializeFileTransferAsyncEvent(InvalidContext,forRAWFileLe" +
                    "vel,True)\'");
            this.Manager.Assert((context == FRS2Model.ServerContext.InvalidContext), String.Format("expected \'FRS2Model.ServerContext.InvalidContext\', actual \'{0}\' (context of Initi" +
                        "alizeFileTransferAsyncEvent, state S181)", TestManagerHelpers.Describe(context)));
            this.Manager.Assert((rdcsigLevel == FRS2Model.RDC_Sig_Level.forRAWFileLevel), String.Format("expected \'FRS2Model.RDC_Sig_Level.forRAWFileLevel\', actual \'{0}\' (rdcsigLevel of " +
                        "InitializeFileTransferAsyncEvent, state S181)", TestManagerHelpers.Describe(rdcsigLevel)));
            this.Manager.Assert((isEOF == true), String.Format("expected \'true\', actual \'{0}\' (isEOF of InitializeFileTransferAsyncEvent, state S" +
                        "181)", TestManagerHelpers.Describe(isEOF)));
        }
        
        private void TCtestScenario3S4InitializeFileTransferAsyncEventChecker15(FRS2Model.ServerContext context, FRS2Model.RDC_Sig_Level rdcsigLevel, bool isEOF) {
            this.Manager.Comment("checking step \'event InitializeFileTransferAsyncEvent(ValidContext,forRAWFileLeve" +
                    "l,True)\'");
            this.Manager.Assert((context == FRS2Model.ServerContext.ValidContext), String.Format("expected \'FRS2Model.ServerContext.ValidContext\', actual \'{0}\' (context of Initial" +
                        "izeFileTransferAsyncEvent, state S181)", TestManagerHelpers.Describe(context)));
            this.Manager.Assert((rdcsigLevel == FRS2Model.RDC_Sig_Level.forRAWFileLevel), String.Format("expected \'FRS2Model.RDC_Sig_Level.forRAWFileLevel\', actual \'{0}\' (rdcsigLevel of " +
                        "InitializeFileTransferAsyncEvent, state S181)", TestManagerHelpers.Describe(rdcsigLevel)));
            this.Manager.Assert((isEOF == true), String.Format("expected \'true\', actual \'{0}\' (isEOF of InitializeFileTransferAsyncEvent, state S" +
                        "181)", TestManagerHelpers.Describe(isEOF)));
        }
        
        private void TCtestScenario3S4RequestUpdatesEventChecker2(FRS2Model.FilePresense present, FRS2Model.UPDATE_STATUS updateStatus) {
            this.Manager.Comment("checking step \'event RequestUpdatesEvent(fileExists,UPDATE_STATUS_MORE)\'");
            try {
                this.Manager.Assert((present == FRS2Model.FilePresense.fileExists), String.Format("expected \'FRS2Model.FilePresense.fileExists\', actual \'{0}\' (present of RequestUpd" +
                            "atesEvent, state S121)", TestManagerHelpers.Describe(present)));
                this.Manager.Assert((updateStatus == FRS2Model.UPDATE_STATUS.UPDATE_STATUS_MORE), String.Format("expected \'FRS2Model.UPDATE_STATUS.UPDATE_STATUS_MORE\', actual \'{0}\' (updateStatus" +
                            " of RequestUpdatesEvent, state S121)", TestManagerHelpers.Describe(updateStatus)));
            }
            catch (TransactionFailedException ) {
                this.Manager.Comment("This step would have covered MS-FRS2_R93, MS-FRS2_R498");
                throw;
            }
            this.Manager.Checkpoint("MS-FRS2_R93");
            this.Manager.Checkpoint("MS-FRS2_R498");
        }
        
        private void TCtestScenario3S4InitializeFileTransferAsyncEventChecker16(FRS2Model.ServerContext context, FRS2Model.RDC_Sig_Level rdcsigLevel, bool isEOF) {
            this.Manager.Comment("checking step \'event InitializeFileTransferAsyncEvent(ValidContext,forRDCFileLeve" +
                    "l,False)\'");
            this.Manager.Assert((context == FRS2Model.ServerContext.ValidContext), String.Format("expected \'FRS2Model.ServerContext.ValidContext\', actual \'{0}\' (context of Initial" +
                        "izeFileTransferAsyncEvent, state S182)", TestManagerHelpers.Describe(context)));
            this.Manager.Assert((rdcsigLevel == FRS2Model.RDC_Sig_Level.forRDCFileLevel), String.Format("expected \'FRS2Model.RDC_Sig_Level.forRDCFileLevel\', actual \'{0}\' (rdcsigLevel of " +
                        "InitializeFileTransferAsyncEvent, state S182)", TestManagerHelpers.Describe(rdcsigLevel)));
            this.Manager.Assert((isEOF == false), String.Format("expected \'false\', actual \'{0}\' (isEOF of InitializeFileTransferAsyncEvent, state " +
                        "S182)", TestManagerHelpers.Describe(isEOF)));
        }
        
        private void TCtestScenario3S4InitializeFileTransferAsyncEventChecker17(FRS2Model.ServerContext context, FRS2Model.RDC_Sig_Level rdcsigLevel, bool isEOF) {
            this.Manager.Comment("checking step \'event InitializeFileTransferAsyncEvent(ValidContext,forRAWFileLeve" +
                    "l,False)\'");
            this.Manager.Assert((context == FRS2Model.ServerContext.ValidContext), String.Format("expected \'FRS2Model.ServerContext.ValidContext\', actual \'{0}\' (context of Initial" +
                        "izeFileTransferAsyncEvent, state S182)", TestManagerHelpers.Describe(context)));
            this.Manager.Assert((rdcsigLevel == FRS2Model.RDC_Sig_Level.forRAWFileLevel), String.Format("expected \'FRS2Model.RDC_Sig_Level.forRAWFileLevel\', actual \'{0}\' (rdcsigLevel of " +
                        "InitializeFileTransferAsyncEvent, state S182)", TestManagerHelpers.Describe(rdcsigLevel)));
            this.Manager.Assert((isEOF == false), String.Format("expected \'false\', actual \'{0}\' (isEOF of InitializeFileTransferAsyncEvent, state " +
                        "S182)", TestManagerHelpers.Describe(isEOF)));
        }
        
        private void TCtestScenario3S4InitializeFileTransferAsyncEventChecker18(FRS2Model.ServerContext context, FRS2Model.RDC_Sig_Level rdcsigLevel, bool isEOF) {
            this.Manager.Comment("checking step \'event InitializeFileTransferAsyncEvent(InvalidContext,forRAWFileLe" +
                    "vel,False)\'");
            this.Manager.Assert((context == FRS2Model.ServerContext.InvalidContext), String.Format("expected \'FRS2Model.ServerContext.InvalidContext\', actual \'{0}\' (context of Initi" +
                        "alizeFileTransferAsyncEvent, state S182)", TestManagerHelpers.Describe(context)));
            this.Manager.Assert((rdcsigLevel == FRS2Model.RDC_Sig_Level.forRAWFileLevel), String.Format("expected \'FRS2Model.RDC_Sig_Level.forRAWFileLevel\', actual \'{0}\' (rdcsigLevel of " +
                        "InitializeFileTransferAsyncEvent, state S182)", TestManagerHelpers.Describe(rdcsigLevel)));
            this.Manager.Assert((isEOF == false), String.Format("expected \'false\', actual \'{0}\' (isEOF of InitializeFileTransferAsyncEvent, state " +
                        "S182)", TestManagerHelpers.Describe(isEOF)));
        }
        
        private void TCtestScenario3S4InitializeFileTransferAsyncEventChecker19(FRS2Model.ServerContext context, FRS2Model.RDC_Sig_Level rdcsigLevel, bool isEOF) {
            this.Manager.Comment("checking step \'event InitializeFileTransferAsyncEvent(InvalidContext,forRDCFileLe" +
                    "vel,True)\'");
            this.Manager.Assert((context == FRS2Model.ServerContext.InvalidContext), String.Format("expected \'FRS2Model.ServerContext.InvalidContext\', actual \'{0}\' (context of Initi" +
                        "alizeFileTransferAsyncEvent, state S182)", TestManagerHelpers.Describe(context)));
            this.Manager.Assert((rdcsigLevel == FRS2Model.RDC_Sig_Level.forRDCFileLevel), String.Format("expected \'FRS2Model.RDC_Sig_Level.forRDCFileLevel\', actual \'{0}\' (rdcsigLevel of " +
                        "InitializeFileTransferAsyncEvent, state S182)", TestManagerHelpers.Describe(rdcsigLevel)));
            this.Manager.Assert((isEOF == true), String.Format("expected \'true\', actual \'{0}\' (isEOF of InitializeFileTransferAsyncEvent, state S" +
                        "182)", TestManagerHelpers.Describe(isEOF)));
        }
        
        private void TCtestScenario3S4InitializeFileTransferAsyncEventChecker20(FRS2Model.ServerContext context, FRS2Model.RDC_Sig_Level rdcsigLevel, bool isEOF) {
            this.Manager.Comment("checking step \'event InitializeFileTransferAsyncEvent(ValidContext,forRDCFileLeve" +
                    "l,True)\'");
            this.Manager.Assert((context == FRS2Model.ServerContext.ValidContext), String.Format("expected \'FRS2Model.ServerContext.ValidContext\', actual \'{0}\' (context of Initial" +
                        "izeFileTransferAsyncEvent, state S182)", TestManagerHelpers.Describe(context)));
            this.Manager.Assert((rdcsigLevel == FRS2Model.RDC_Sig_Level.forRDCFileLevel), String.Format("expected \'FRS2Model.RDC_Sig_Level.forRDCFileLevel\', actual \'{0}\' (rdcsigLevel of " +
                        "InitializeFileTransferAsyncEvent, state S182)", TestManagerHelpers.Describe(rdcsigLevel)));
            this.Manager.Assert((isEOF == true), String.Format("expected \'true\', actual \'{0}\' (isEOF of InitializeFileTransferAsyncEvent, state S" +
                        "182)", TestManagerHelpers.Describe(isEOF)));
        }
        
        private void TCtestScenario3S4InitializeFileTransferAsyncEventChecker21(FRS2Model.ServerContext context, FRS2Model.RDC_Sig_Level rdcsigLevel, bool isEOF) {
            this.Manager.Comment("checking step \'event InitializeFileTransferAsyncEvent(InvalidContext,forRDCFileLe" +
                    "vel,False)\'");
            this.Manager.Assert((context == FRS2Model.ServerContext.InvalidContext), String.Format("expected \'FRS2Model.ServerContext.InvalidContext\', actual \'{0}\' (context of Initi" +
                        "alizeFileTransferAsyncEvent, state S182)", TestManagerHelpers.Describe(context)));
            this.Manager.Assert((rdcsigLevel == FRS2Model.RDC_Sig_Level.forRDCFileLevel), String.Format("expected \'FRS2Model.RDC_Sig_Level.forRDCFileLevel\', actual \'{0}\' (rdcsigLevel of " +
                        "InitializeFileTransferAsyncEvent, state S182)", TestManagerHelpers.Describe(rdcsigLevel)));
            this.Manager.Assert((isEOF == false), String.Format("expected \'false\', actual \'{0}\' (isEOF of InitializeFileTransferAsyncEvent, state " +
                        "S182)", TestManagerHelpers.Describe(isEOF)));
        }
        
        private void TCtestScenario3S4InitializeFileTransferAsyncEventChecker22(FRS2Model.ServerContext context, FRS2Model.RDC_Sig_Level rdcsigLevel, bool isEOF) {
            this.Manager.Comment("checking step \'event InitializeFileTransferAsyncEvent(InvalidContext,forRAWFileLe" +
                    "vel,True)\'");
            this.Manager.Assert((context == FRS2Model.ServerContext.InvalidContext), String.Format("expected \'FRS2Model.ServerContext.InvalidContext\', actual \'{0}\' (context of Initi" +
                        "alizeFileTransferAsyncEvent, state S182)", TestManagerHelpers.Describe(context)));
            this.Manager.Assert((rdcsigLevel == FRS2Model.RDC_Sig_Level.forRAWFileLevel), String.Format("expected \'FRS2Model.RDC_Sig_Level.forRAWFileLevel\', actual \'{0}\' (rdcsigLevel of " +
                        "InitializeFileTransferAsyncEvent, state S182)", TestManagerHelpers.Describe(rdcsigLevel)));
            this.Manager.Assert((isEOF == true), String.Format("expected \'true\', actual \'{0}\' (isEOF of InitializeFileTransferAsyncEvent, state S" +
                        "182)", TestManagerHelpers.Describe(isEOF)));
        }
        
        private void TCtestScenario3S4InitializeFileTransferAsyncEventChecker23(FRS2Model.ServerContext context, FRS2Model.RDC_Sig_Level rdcsigLevel, bool isEOF) {
            this.Manager.Comment("checking step \'event InitializeFileTransferAsyncEvent(ValidContext,forRAWFileLeve" +
                    "l,True)\'");
            this.Manager.Assert((context == FRS2Model.ServerContext.ValidContext), String.Format("expected \'FRS2Model.ServerContext.ValidContext\', actual \'{0}\' (context of Initial" +
                        "izeFileTransferAsyncEvent, state S182)", TestManagerHelpers.Describe(context)));
            this.Manager.Assert((rdcsigLevel == FRS2Model.RDC_Sig_Level.forRAWFileLevel), String.Format("expected \'FRS2Model.RDC_Sig_Level.forRAWFileLevel\', actual \'{0}\' (rdcsigLevel of " +
                        "InitializeFileTransferAsyncEvent, state S182)", TestManagerHelpers.Describe(rdcsigLevel)));
            this.Manager.Assert((isEOF == true), String.Format("expected \'true\', actual \'{0}\' (isEOF of InitializeFileTransferAsyncEvent, state S" +
                        "182)", TestManagerHelpers.Describe(isEOF)));
        }
        
        private void TCtestScenario3S4RequestUpdatesEventChecker3(FRS2Model.FilePresense present, FRS2Model.UPDATE_STATUS updateStatus) {
            this.Manager.Comment("checking step \'event RequestUpdatesEvent(fileDeleted,UPDATE_STATUS_MORE)\'");
            try {
                this.Manager.Assert((present == FRS2Model.FilePresense.fileDeleted), String.Format("expected \'FRS2Model.FilePresense.fileDeleted\', actual \'{0}\' (present of RequestUp" +
                            "datesEvent, state S121)", TestManagerHelpers.Describe(present)));
                this.Manager.Assert((updateStatus == FRS2Model.UPDATE_STATUS.UPDATE_STATUS_MORE), String.Format("expected \'FRS2Model.UPDATE_STATUS.UPDATE_STATUS_MORE\', actual \'{0}\' (updateStatus" +
                            " of RequestUpdatesEvent, state S121)", TestManagerHelpers.Describe(updateStatus)));
            }
            catch (TransactionFailedException ) {
                this.Manager.Comment("This step would have covered MS-FRS2_R93");
                throw;
            }
            this.Manager.Checkpoint("MS-FRS2_R93");
        }
        
        private void TCtestScenario3S4AsyncPollResponseEventChecker1(FRS2Model.VVGeneration vvGen) {
            this.Manager.Comment("checking step \'event AsyncPollResponseEvent(InvalidValue)\'");
            try {
                this.Manager.Assert((vvGen == FRS2Model.VVGeneration.InvalidValue), String.Format("expected \'FRS2Model.VVGeneration.InvalidValue\', actual \'{0}\' (vvGen of AsyncPollR" +
                            "esponseEvent, state S89)", TestManagerHelpers.Describe(vvGen)));
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
        [Microsoft.VisualStudio.TestTools.UnitTesting.DescriptionAttribute(@"MS-FRS2_R37, MS-FRS2_R436, MS-FRS2_R439, MS-FRS2_R455, MS-FRS2_R458, MS-FRS2_R448, MS-FRS2_R549, MS-FRS2_R552, MS-FRS2_R517, MS-FRS2_R520, MS-FRS2_R466, MS-FRS2_R556, MS-FRS2_R1020, MS-FRS2_R555, MS-FRS2_R486, MS-FRS2_R489, MS-FRS2_R93, MS-FRS2_R94, MS-FRS2_R774, MS-FRS2_R777, MS-FRS2_R793, MS-FRS2_R646, MS-FRS2_R649, MS-FRS2_R650, MS-FRS2_R671, MS-FRS2_R674, MS-FRS2_R693, MS-FRS2_R694, MS-FRS2_R683, MS-FRS2_R691, MS-FRS2_R692, MS-FRS2_R675, MS-FRS2_R702, MS-FRS2_R737, MS-FRS2_R739, MS-FRS2_R93, MS-FRS2_R94, MS-FRS2_R774, MS-FRS2_R777, MS-FRS2_R793, MS-FRS2_R646, MS-FRS2_R649, MS-FRS2_R650, MS-FRS2_R671, MS-FRS2_R674, MS-FRS2_R693, MS-FRS2_R694, MS-FRS2_R683, MS-FRS2_R691, MS-FRS2_R692, MS-FRS2_R675, MS-FRS2_R702, MS-FRS2_R737, MS-FRS2_R739, MS-FRS2_R93, MS-FRS2_R498, MS-FRS2_R774, MS-FRS2_R777, MS-FRS2_R793, MS-FRS2_R646, MS-FRS2_R649, MS-FRS2_R650, MS-FRS2_R671, MS-FRS2_R674, MS-FRS2_R693, MS-FRS2_R694, MS-FRS2_R683, MS-FRS2_R691, MS-FRS2_R692, MS-FRS2_R675, MS-FRS2_R702, MS-FRS2_R737, MS-FRS2_R739, MS-FRS2_R93, MS-FRS2_R556, MS-FRS2_R1020, MS-FRS2_R555")]
        public virtual void FRS2_TCtestScenario3S6() {
            this.Manager.BeginTest("TCtestScenario3S6");
            this.Manager.Comment("reaching state \'S6\'");
            FRS2Model.error_status_t temp94;
            this.Manager.Comment(@"executing step 'call Initialization(Windows7,Map{1=>enabled,2=>enabled,3=>enabled,4=>disabled},Map{1=>exists,2=>exists,3=>exists,4=>exists},Map{""A""=>1,""B""=>2,""C""=>3,""D""=>4},Map{""A""=>sysvol,""B""=>protection,""C""=>sysvol,""D""=>sysvol},Map{1=>""A"",2=>""B"",3=>""C"",4=>""D""},Map{1=>writeOnly,2=>readWrite,3=>readOnly,4=>readWrite},Map{1=>enabled,2=>disabled,3=>enabled,4=>enabled})'");
            temp94 = this.IFRS2ManagedAdapterInstance.Initialization(FRS2Model.OSVersion.Windows7, new Microsoft.Modeling.Map<System.Int32,FRS2Model.connectionProperty>().Add(1, FRS2Model.connectionProperty.enabled).Add(2, FRS2Model.connectionProperty.enabled).Add(3, FRS2Model.connectionProperty.enabled).Add(4, FRS2Model.connectionProperty.disabled), new Microsoft.Modeling.Map<System.Int32,FRS2Model.FromServer>().Add(1, FRS2Model.FromServer.exists).Add(2, FRS2Model.FromServer.exists).Add(3, FRS2Model.FromServer.exists).Add(4, FRS2Model.FromServer.exists), new Microsoft.Modeling.Map<System.String,System.Int32>().Add("A", 1).Add("B", 2).Add("C", 3).Add("D", 4), new Microsoft.Modeling.Map<System.String,FRS2Model.ReplicationGroupTypes>().Add("A", FRS2Model.ReplicationGroupTypes.sysvol).Add("B", FRS2Model.ReplicationGroupTypes.protection).Add("C", FRS2Model.ReplicationGroupTypes.sysvol).Add("D", FRS2Model.ReplicationGroupTypes.sysvol), new Microsoft.Modeling.Map<System.Int32,System.String>().Add(1, "A").Add(2, "B").Add(3, "C").Add(4, "D"), new Microsoft.Modeling.Map<System.Int32,FRS2Model.accessLevels>().Add(1, FRS2Model.accessLevels.writeOnly).Add(2, FRS2Model.accessLevels.readWrite).Add(3, FRS2Model.accessLevels.readOnly).Add(4, FRS2Model.accessLevels.readWrite), new Microsoft.Modeling.Map<System.Int32,FRS2Model.connectionProperty>().Add(1, FRS2Model.connectionProperty.enabled).Add(2, FRS2Model.connectionProperty.disabled).Add(3, FRS2Model.connectionProperty.enabled).Add(4, FRS2Model.connectionProperty.enabled));
            this.Manager.Comment("reaching state \'S7\'");
            this.Manager.Comment("checking step \'return Initialization/ERROR_SUCCESS\'");
            this.Manager.Assert((temp94 == FRS2Model.error_status_t.ERROR_SUCCESS), String.Format("expected \'FRS2Model.error_status_t.ERROR_SUCCESS\', actual \'{0}\' (return of Initia" +
                        "lization, state S7)", TestManagerHelpers.Describe(temp94)));
            this.Manager.Comment("reaching state \'S21\'");
            FRS2Model.ProtocolVersionReturned temp95;
            FRS2Model.UpstreamFlagValueReturned temp96;
            FRS2Model.error_status_t temp97;
            this.Manager.Comment("executing step \'call EstablishConnection(\"A\",1,FRS_COMMUNICATION_PROTOCOL_VERSION" +
                    "_LONGHORN_SERVER,out _,out _)\'");
            temp97 = this.IFRS2ManagedAdapterInstance.EstablishConnection("A", 1, FRS2Model.ProtocolVersion.FRS_COMMUNICATION_PROTOCOL_VERSION_LONGHORN_SERVER, out temp95, out temp96);
            this.Manager.Checkpoint("MS-FRS2_R37");
            this.Manager.Checkpoint("MS-FRS2_R436");
            this.Manager.Checkpoint("MS-FRS2_R439");
            this.Manager.Comment("reaching state \'S30\'");
            this.Manager.Comment("checking step \'return EstablishConnection/[out Valid,out Valid]:ERROR_SUCCESS\'");
            this.Manager.Assert((temp95 == FRS2Model.ProtocolVersionReturned.Valid), String.Format("expected \'FRS2Model.ProtocolVersionReturned.Valid\', actual \'{0}\' (upstreamProtoco" +
                        "lVersion of EstablishConnection, state S30)", TestManagerHelpers.Describe(temp95)));
            this.Manager.Assert((temp96 == FRS2Model.UpstreamFlagValueReturned.Valid), String.Format("expected \'FRS2Model.UpstreamFlagValueReturned.Valid\', actual \'{0}\' (upstreamFlags" +
                        " of EstablishConnection, state S30)", TestManagerHelpers.Describe(temp96)));
            this.Manager.Assert((temp97 == FRS2Model.error_status_t.ERROR_SUCCESS), String.Format("expected \'FRS2Model.error_status_t.ERROR_SUCCESS\', actual \'{0}\' (return of Establ" +
                        "ishConnection, state S30)", TestManagerHelpers.Describe(temp97)));
            this.Manager.Comment("reaching state \'S39\'");
            FRS2Model.error_status_t temp98;
            this.Manager.Comment("executing step \'call EstablishSession(1,1)\'");
            temp98 = this.IFRS2ManagedAdapterInstance.EstablishSession(1, 1);
            this.Manager.Checkpoint("MS-FRS2_R455");
            this.Manager.Checkpoint("MS-FRS2_R458");
            this.Manager.Checkpoint("MS-FRS2_R448");
            this.Manager.Comment("reaching state \'S48\'");
            this.Manager.Comment("checking step \'return EstablishSession/ERROR_SUCCESS\'");
            this.Manager.Assert((temp98 == FRS2Model.error_status_t.ERROR_SUCCESS), String.Format("expected \'FRS2Model.error_status_t.ERROR_SUCCESS\', actual \'{0}\' (return of Establ" +
                        "ishSession, state S48)", TestManagerHelpers.Describe(temp98)));
            this.Manager.Comment("reaching state \'S57\'");
            FRS2Model.error_status_t temp99;
            this.Manager.Comment("executing step \'call AsyncPoll(1)\'");
            temp99 = this.IFRS2ManagedAdapterInstance.AsyncPoll(1);
            this.Manager.Checkpoint("MS-FRS2_R549");
            this.Manager.Checkpoint("MS-FRS2_R552");
            this.Manager.Comment("reaching state \'S66\'");
            this.Manager.Comment("checking step \'return AsyncPoll/ERROR_SUCCESS\'");
            this.Manager.Assert((temp99 == FRS2Model.error_status_t.ERROR_SUCCESS), String.Format("expected \'FRS2Model.error_status_t.ERROR_SUCCESS\', actual \'{0}\' (return of AsyncP" +
                        "oll, state S66)", TestManagerHelpers.Describe(temp99)));
            this.Manager.Comment("reaching state \'S74\'");
            FRS2Model.error_status_t temp100;
            this.Manager.Comment("executing step \'call RequestVersionVector(1,1,1,REQUEST_NORMAL_SYNC,CHANGE_ALL,Va" +
                    "lidValue)\'");
            temp100 = this.IFRS2ManagedAdapterInstance.RequestVersionVector(1, 1, 1, FRS2Model.VERSION_REQUEST_TYPE.REQUEST_NORMAL_SYNC, FRS2Model.VERSION_CHANGE_TYPE.CHANGE_ALL, FRS2Model.VVGeneration.ValidValue);
            this.Manager.Checkpoint("MS-FRS2_R517");
            this.Manager.Checkpoint("MS-FRS2_R520");
            this.Manager.Checkpoint("MS-FRS2_R466");
            this.Manager.Comment("reaching state \'S82\'");
            this.Manager.Comment("checking step \'return RequestVersionVector/ERROR_SUCCESS\'");
            this.Manager.Assert((temp100 == FRS2Model.error_status_t.ERROR_SUCCESS), String.Format("expected \'FRS2Model.error_status_t.ERROR_SUCCESS\', actual \'{0}\' (return of Reques" +
                        "tVersionVector, state S82)", TestManagerHelpers.Describe(temp100)));
            this.Manager.Comment("reaching state \'S90\'");
            int temp121 = this.Manager.ExpectEvent(this.QuiescenceTimeout, true, new ExpectedEvent(TCtestScenario3.AsyncPollResponseEventInfo, null, new AsyncPollResponseEventDelegate1(this.TCtestScenario3S6AsyncPollResponseEventChecker)), new ExpectedEvent(TCtestScenario3.AsyncPollResponseEventInfo, null, new AsyncPollResponseEventDelegate1(this.TCtestScenario3S6AsyncPollResponseEventChecker1)));
            if ((temp121 == 0)) {
                this.Manager.Comment("reaching state \'S101\'");
                FRS2Model.error_status_t temp101;
                this.Manager.Comment("executing step \'call RequestUpdates(1,1,valid)\'");
                temp101 = this.IFRS2ManagedAdapterInstance.RequestUpdates(1, 1, FRS2Model.versionVectorDiff.valid);
                this.Manager.Checkpoint("MS-FRS2_R486");
                this.Manager.Checkpoint("MS-FRS2_R489");
                this.Manager.Comment("reaching state \'S114\'");
                this.Manager.Comment("checking step \'return RequestUpdates/ERROR_SUCCESS\'");
                this.Manager.Assert((temp101 == FRS2Model.error_status_t.ERROR_SUCCESS), String.Format("expected \'FRS2Model.error_status_t.ERROR_SUCCESS\', actual \'{0}\' (return of Reques" +
                            "tUpdates, state S114)", TestManagerHelpers.Describe(temp101)));
                this.Manager.Comment("reaching state \'S122\'");
                int temp120 = this.Manager.ExpectEvent(this.QuiescenceTimeout, true, new ExpectedEvent(TCtestScenario3.RequestUpdatesEventInfo, null, new RequestUpdatesEventDelegate1(this.TCtestScenario3S6RequestUpdatesEventChecker)), new ExpectedEvent(TCtestScenario3.RequestUpdatesEventInfo, null, new RequestUpdatesEventDelegate1(this.TCtestScenario3S6RequestUpdatesEventChecker1)), new ExpectedEvent(TCtestScenario3.RequestUpdatesEventInfo, null, new RequestUpdatesEventDelegate1(this.TCtestScenario3S6RequestUpdatesEventChecker2)), new ExpectedEvent(TCtestScenario3.RequestUpdatesEventInfo, null, new RequestUpdatesEventDelegate1(this.TCtestScenario3S6RequestUpdatesEventChecker3)));
                if ((temp120 == 0)) {
                    this.Manager.Comment("reaching state \'S139\'");
                    FRS2Model.error_status_t temp102;
                    this.Manager.Comment("executing step \'call InitializeFileTransferAsync(1,1,True)\'");
                    temp102 = this.IFRS2ManagedAdapterInstance.InitializeFileTransferAsync(1, 1, true);
                    this.Manager.Checkpoint("MS-FRS2_R774");
                    this.Manager.Checkpoint("MS-FRS2_R777");
                    this.Manager.Checkpoint("MS-FRS2_R793");
                    this.Manager.Comment("reaching state \'S165\'");
                    this.Manager.Comment("checking step \'return InitializeFileTransferAsync/ERROR_SUCCESS\'");
                    this.Manager.Assert((temp102 == FRS2Model.error_status_t.ERROR_SUCCESS), String.Format("expected \'FRS2Model.error_status_t.ERROR_SUCCESS\', actual \'{0}\' (return of Initia" +
                                "lizeFileTransferAsync, state S165)", TestManagerHelpers.Describe(temp102)));
                    this.Manager.Comment("reaching state \'S183\'");
                    int temp107 = this.Manager.ExpectEvent(this.QuiescenceTimeout, true, new ExpectedEvent(TCtestScenario3.InitializeFileTransferAsyncEventInfo, null, new InitializeFileTransferAsyncEventDelegate1(this.TCtestScenario3S6InitializeFileTransferAsyncEventChecker)), new ExpectedEvent(TCtestScenario3.InitializeFileTransferAsyncEventInfo, null, new InitializeFileTransferAsyncEventDelegate1(this.TCtestScenario3S6InitializeFileTransferAsyncEventChecker1)), new ExpectedEvent(TCtestScenario3.InitializeFileTransferAsyncEventInfo, null, new InitializeFileTransferAsyncEventDelegate1(this.TCtestScenario3S6InitializeFileTransferAsyncEventChecker2)), new ExpectedEvent(TCtestScenario3.InitializeFileTransferAsyncEventInfo, null, new InitializeFileTransferAsyncEventDelegate1(this.TCtestScenario3S6InitializeFileTransferAsyncEventChecker3)), new ExpectedEvent(TCtestScenario3.InitializeFileTransferAsyncEventInfo, null, new InitializeFileTransferAsyncEventDelegate1(this.TCtestScenario3S6InitializeFileTransferAsyncEventChecker4)), new ExpectedEvent(TCtestScenario3.InitializeFileTransferAsyncEventInfo, null, new InitializeFileTransferAsyncEventDelegate1(this.TCtestScenario3S6InitializeFileTransferAsyncEventChecker5)), new ExpectedEvent(TCtestScenario3.InitializeFileTransferAsyncEventInfo, null, new InitializeFileTransferAsyncEventDelegate1(this.TCtestScenario3S6InitializeFileTransferAsyncEventChecker6)), new ExpectedEvent(TCtestScenario3.InitializeFileTransferAsyncEventInfo, null, new InitializeFileTransferAsyncEventDelegate1(this.TCtestScenario3S6InitializeFileTransferAsyncEventChecker7)));
                    if ((temp107 == 0)) {
                        TCtestScenario3S264();
                        goto label15;
                    }
                    if ((temp107 == 1)) {
                        TCtestScenario3S199();
                        goto label15;
                    }
                    if ((temp107 == 2)) {
                        TCtestScenario3S197();
                        goto label15;
                    }
                    if ((temp107 == 3)) {
                        TCtestScenario3S227();
                        goto label15;
                    }
                    if ((temp107 == 4)) {
                        TCtestScenario3S228();
                        goto label15;
                    }
                    if ((temp107 == 5)) {
                        TCtestScenario3S194();
                        goto label15;
                    }
                    if ((temp107 == 6)) {
                        TCtestScenario3S230();
                        goto label15;
                    }
                    if ((temp107 == 7)) {
                        TCtestScenario3S192();
                        goto label15;
                    }
                    throw new InvalidOperationException("never reached");
                label15:
;
                    goto label18;
                }
                if ((temp120 == 1)) {
                    this.Manager.Comment("reaching state \'S140\'");
                    FRS2Model.error_status_t temp108;
                    this.Manager.Comment("executing step \'call InitializeFileTransferAsync(1,1,True)\'");
                    temp108 = this.IFRS2ManagedAdapterInstance.InitializeFileTransferAsync(1, 1, true);
                    this.Manager.Checkpoint("MS-FRS2_R774");
                    this.Manager.Checkpoint("MS-FRS2_R777");
                    this.Manager.Checkpoint("MS-FRS2_R793");
                    this.Manager.Comment("reaching state \'S166\'");
                    this.Manager.Comment("checking step \'return InitializeFileTransferAsync/ERROR_SUCCESS\'");
                    this.Manager.Assert((temp108 == FRS2Model.error_status_t.ERROR_SUCCESS), String.Format("expected \'FRS2Model.error_status_t.ERROR_SUCCESS\', actual \'{0}\' (return of Initia" +
                                "lizeFileTransferAsync, state S166)", TestManagerHelpers.Describe(temp108)));
                    this.Manager.Comment("reaching state \'S184\'");
                    int temp113 = this.Manager.ExpectEvent(this.QuiescenceTimeout, true, new ExpectedEvent(TCtestScenario3.InitializeFileTransferAsyncEventInfo, null, new InitializeFileTransferAsyncEventDelegate1(this.TCtestScenario3S6InitializeFileTransferAsyncEventChecker8)), new ExpectedEvent(TCtestScenario3.InitializeFileTransferAsyncEventInfo, null, new InitializeFileTransferAsyncEventDelegate1(this.TCtestScenario3S6InitializeFileTransferAsyncEventChecker9)), new ExpectedEvent(TCtestScenario3.InitializeFileTransferAsyncEventInfo, null, new InitializeFileTransferAsyncEventDelegate1(this.TCtestScenario3S6InitializeFileTransferAsyncEventChecker10)), new ExpectedEvent(TCtestScenario3.InitializeFileTransferAsyncEventInfo, null, new InitializeFileTransferAsyncEventDelegate1(this.TCtestScenario3S6InitializeFileTransferAsyncEventChecker11)), new ExpectedEvent(TCtestScenario3.InitializeFileTransferAsyncEventInfo, null, new InitializeFileTransferAsyncEventDelegate1(this.TCtestScenario3S6InitializeFileTransferAsyncEventChecker12)), new ExpectedEvent(TCtestScenario3.InitializeFileTransferAsyncEventInfo, null, new InitializeFileTransferAsyncEventDelegate1(this.TCtestScenario3S6InitializeFileTransferAsyncEventChecker13)), new ExpectedEvent(TCtestScenario3.InitializeFileTransferAsyncEventInfo, null, new InitializeFileTransferAsyncEventDelegate1(this.TCtestScenario3S6InitializeFileTransferAsyncEventChecker14)), new ExpectedEvent(TCtestScenario3.InitializeFileTransferAsyncEventInfo, null, new InitializeFileTransferAsyncEventDelegate1(this.TCtestScenario3S6InitializeFileTransferAsyncEventChecker15)));
                    if ((temp113 == 0)) {
                        TCtestScenario3S272();
                        goto label16;
                    }
                    if ((temp113 == 1)) {
                        TCtestScenario3S207();
                        goto label16;
                    }
                    if ((temp113 == 2)) {
                        TCtestScenario3S206();
                        goto label16;
                    }
                    if ((temp113 == 3)) {
                        TCtestScenario3S235();
                        goto label16;
                    }
                    if ((temp113 == 4)) {
                        TCtestScenario3S203();
                        goto label16;
                    }
                    if ((temp113 == 5)) {
                        TCtestScenario3S202();
                        goto label16;
                    }
                    if ((temp113 == 6)) {
                        TCtestScenario3S238();
                        goto label16;
                    }
                    if ((temp113 == 7)) {
                        TCtestScenario3S239();
                        goto label16;
                    }
                    throw new InvalidOperationException("never reached");
                label16:
;
                    goto label18;
                }
                if ((temp120 == 2)) {
                    TCtestScenario3S141();
                    goto label18;
                }
                if ((temp120 == 3)) {
                    TCtestScenario3S127();
                    goto label18;
                }
                throw new InvalidOperationException("never reached");
            label18:
;
                goto label19;
            }
            if ((temp121 == 1)) {
                TCtestScenario3S96();
                goto label19;
            }
            throw new InvalidOperationException("never reached");
        label19:
;
            this.Manager.EndTest();
        }
        
        private void TCtestScenario3S6AsyncPollResponseEventChecker(FRS2Model.VVGeneration vvGen) {
            this.Manager.Comment("checking step \'event AsyncPollResponseEvent(ValidValue)\'");
            try {
                this.Manager.Assert((vvGen == FRS2Model.VVGeneration.ValidValue), String.Format("expected \'FRS2Model.VVGeneration.ValidValue\', actual \'{0}\' (vvGen of AsyncPollRes" +
                            "ponseEvent, state S90)", TestManagerHelpers.Describe(vvGen)));
            }
            catch (TransactionFailedException ) {
                this.Manager.Comment("This step would have covered MS-FRS2_R556, MS-FRS2_R1020, MS-FRS2_R555");
                throw;
            }
            this.Manager.Checkpoint("MS-FRS2_R556");
            this.Manager.Checkpoint("MS-FRS2_R1020");
            this.Manager.Checkpoint("MS-FRS2_R555");
        }
        
        private void TCtestScenario3S6RequestUpdatesEventChecker(FRS2Model.FilePresense present, FRS2Model.UPDATE_STATUS updateStatus) {
            this.Manager.Comment("checking step \'event RequestUpdatesEvent(fileDeleted,UPDATE_STATUS_DONE)\'");
            try {
                this.Manager.Assert((present == FRS2Model.FilePresense.fileDeleted), String.Format("expected \'FRS2Model.FilePresense.fileDeleted\', actual \'{0}\' (present of RequestUp" +
                            "datesEvent, state S122)", TestManagerHelpers.Describe(present)));
                this.Manager.Assert((updateStatus == FRS2Model.UPDATE_STATUS.UPDATE_STATUS_DONE), String.Format("expected \'FRS2Model.UPDATE_STATUS.UPDATE_STATUS_DONE\', actual \'{0}\' (updateStatus" +
                            " of RequestUpdatesEvent, state S122)", TestManagerHelpers.Describe(updateStatus)));
            }
            catch (TransactionFailedException ) {
                this.Manager.Comment("This step would have covered MS-FRS2_R93, MS-FRS2_R94");
                throw;
            }
            this.Manager.Checkpoint("MS-FRS2_R93");
            this.Manager.Checkpoint("MS-FRS2_R94");
        }
        
        private void TCtestScenario3S6InitializeFileTransferAsyncEventChecker(FRS2Model.ServerContext context, FRS2Model.RDC_Sig_Level rdcsigLevel, bool isEOF) {
            this.Manager.Comment("checking step \'event InitializeFileTransferAsyncEvent(ValidContext,forRDCFileLeve" +
                    "l,False)\'");
            this.Manager.Assert((context == FRS2Model.ServerContext.ValidContext), String.Format("expected \'FRS2Model.ServerContext.ValidContext\', actual \'{0}\' (context of Initial" +
                        "izeFileTransferAsyncEvent, state S183)", TestManagerHelpers.Describe(context)));
            this.Manager.Assert((rdcsigLevel == FRS2Model.RDC_Sig_Level.forRDCFileLevel), String.Format("expected \'FRS2Model.RDC_Sig_Level.forRDCFileLevel\', actual \'{0}\' (rdcsigLevel of " +
                        "InitializeFileTransferAsyncEvent, state S183)", TestManagerHelpers.Describe(rdcsigLevel)));
            this.Manager.Assert((isEOF == false), String.Format("expected \'false\', actual \'{0}\' (isEOF of InitializeFileTransferAsyncEvent, state " +
                        "S183)", TestManagerHelpers.Describe(isEOF)));
        }
        
        private void TCtestScenario3S264() {
            this.Manager.Comment("reaching state \'S264\'");
            FRS2Model.error_status_t temp103;
            this.Manager.Comment("executing step \'call RdcGetSignatures(valid)\'");
            temp103 = this.IFRS2ManagedAdapterInstance.RdcGetSignatures(FRS2Model.offset.valid);
            this.Manager.Checkpoint("MS-FRS2_R646");
            this.Manager.Checkpoint("MS-FRS2_R649");
            this.Manager.Checkpoint("MS-FRS2_R650");
            this.Manager.Comment("reaching state \'S366\'");
            this.Manager.Comment("checking step \'return RdcGetSignatures/ERROR_SUCCESS\'");
            this.Manager.Assert((temp103 == FRS2Model.error_status_t.ERROR_SUCCESS), String.Format("expected \'FRS2Model.error_status_t.ERROR_SUCCESS\', actual \'{0}\' (return of RdcGet" +
                        "Signatures, state S366)", TestManagerHelpers.Describe(temp103)));
            this.Manager.Comment("reaching state \'S399\'");
            FRS2Model.error_status_t temp104;
            this.Manager.Comment("executing step \'call RdcPushSourceNeeds()\'");
            temp104 = this.IFRS2ManagedAdapterInstance.RdcPushSourceNeeds();
            this.Manager.Checkpoint("MS-FRS2_R671");
            this.Manager.Checkpoint("MS-FRS2_R674");
            this.Manager.Checkpoint("MS-FRS2_R693");
            this.Manager.Checkpoint("MS-FRS2_R694");
            this.Manager.Checkpoint("MS-FRS2_R683");
            this.Manager.Checkpoint("MS-FRS2_R691");
            this.Manager.Checkpoint("MS-FRS2_R692");
            this.Manager.Checkpoint("MS-FRS2_R675");
            this.Manager.Comment("reaching state \'S411\'");
            this.Manager.Comment("checking step \'return RdcPushSourceNeeds/ERROR_SUCCESS\'");
            this.Manager.Assert((temp104 == FRS2Model.error_status_t.ERROR_SUCCESS), String.Format("expected \'FRS2Model.error_status_t.ERROR_SUCCESS\', actual \'{0}\' (return of RdcPus" +
                        "hSourceNeeds, state S411)", TestManagerHelpers.Describe(temp104)));
            this.Manager.Comment("reaching state \'S423\'");
            FRS2Model.error_status_t temp105;
            this.Manager.Comment("executing step \'call RdcGetFileData(validBufSize)\'");
            temp105 = this.IFRS2ManagedAdapterInstance.RdcGetFileData(FRS2Model.BufferSize.validBufSize);
            this.Manager.Checkpoint("MS-FRS2_R702");
            this.Manager.Comment("reaching state \'S432\'");
            this.Manager.Comment("checking step \'return RdcGetFileData/ERROR_SUCCESS\'");
            this.Manager.Assert((temp105 == FRS2Model.error_status_t.ERROR_SUCCESS), String.Format("expected \'FRS2Model.error_status_t.ERROR_SUCCESS\', actual \'{0}\' (return of RdcGet" +
                        "FileData, state S432)", TestManagerHelpers.Describe(temp105)));
            this.Manager.Comment("reaching state \'S441\'");
            FRS2Model.error_status_t temp106;
            this.Manager.Comment("executing step \'call RdcClose()\'");
            temp106 = this.IFRS2ManagedAdapterInstance.RdcClose();
            this.Manager.Checkpoint("MS-FRS2_R737");
            this.Manager.Checkpoint("MS-FRS2_R739");
            this.Manager.Comment("reaching state \'S444\'");
            this.Manager.Comment("checking step \'return RdcClose/ERROR_SUCCESS\'");
            this.Manager.Assert((temp106 == FRS2Model.error_status_t.ERROR_SUCCESS), String.Format("expected \'FRS2Model.error_status_t.ERROR_SUCCESS\', actual \'{0}\' (return of RdcClo" +
                        "se, state S444)", TestManagerHelpers.Describe(temp106)));
            this.Manager.Comment("reaching state \'S447\'");
        }
        
        private void TCtestScenario3S6InitializeFileTransferAsyncEventChecker1(FRS2Model.ServerContext context, FRS2Model.RDC_Sig_Level rdcsigLevel, bool isEOF) {
            this.Manager.Comment("checking step \'event InitializeFileTransferAsyncEvent(InvalidContext,forRDCFileLe" +
                    "vel,False)\'");
            this.Manager.Assert((context == FRS2Model.ServerContext.InvalidContext), String.Format("expected \'FRS2Model.ServerContext.InvalidContext\', actual \'{0}\' (context of Initi" +
                        "alizeFileTransferAsyncEvent, state S183)", TestManagerHelpers.Describe(context)));
            this.Manager.Assert((rdcsigLevel == FRS2Model.RDC_Sig_Level.forRDCFileLevel), String.Format("expected \'FRS2Model.RDC_Sig_Level.forRDCFileLevel\', actual \'{0}\' (rdcsigLevel of " +
                        "InitializeFileTransferAsyncEvent, state S183)", TestManagerHelpers.Describe(rdcsigLevel)));
            this.Manager.Assert((isEOF == false), String.Format("expected \'false\', actual \'{0}\' (isEOF of InitializeFileTransferAsyncEvent, state " +
                        "S183)", TestManagerHelpers.Describe(isEOF)));
        }
        
        private void TCtestScenario3S6InitializeFileTransferAsyncEventChecker2(FRS2Model.ServerContext context, FRS2Model.RDC_Sig_Level rdcsigLevel, bool isEOF) {
            this.Manager.Comment("checking step \'event InitializeFileTransferAsyncEvent(ValidContext,forRAWFileLeve" +
                    "l,False)\'");
            this.Manager.Assert((context == FRS2Model.ServerContext.ValidContext), String.Format("expected \'FRS2Model.ServerContext.ValidContext\', actual \'{0}\' (context of Initial" +
                        "izeFileTransferAsyncEvent, state S183)", TestManagerHelpers.Describe(context)));
            this.Manager.Assert((rdcsigLevel == FRS2Model.RDC_Sig_Level.forRAWFileLevel), String.Format("expected \'FRS2Model.RDC_Sig_Level.forRAWFileLevel\', actual \'{0}\' (rdcsigLevel of " +
                        "InitializeFileTransferAsyncEvent, state S183)", TestManagerHelpers.Describe(rdcsigLevel)));
            this.Manager.Assert((isEOF == false), String.Format("expected \'false\', actual \'{0}\' (isEOF of InitializeFileTransferAsyncEvent, state " +
                        "S183)", TestManagerHelpers.Describe(isEOF)));
        }
        
        private void TCtestScenario3S6InitializeFileTransferAsyncEventChecker3(FRS2Model.ServerContext context, FRS2Model.RDC_Sig_Level rdcsigLevel, bool isEOF) {
            this.Manager.Comment("checking step \'event InitializeFileTransferAsyncEvent(InvalidContext,forRAWFileLe" +
                    "vel,True)\'");
            this.Manager.Assert((context == FRS2Model.ServerContext.InvalidContext), String.Format("expected \'FRS2Model.ServerContext.InvalidContext\', actual \'{0}\' (context of Initi" +
                        "alizeFileTransferAsyncEvent, state S183)", TestManagerHelpers.Describe(context)));
            this.Manager.Assert((rdcsigLevel == FRS2Model.RDC_Sig_Level.forRAWFileLevel), String.Format("expected \'FRS2Model.RDC_Sig_Level.forRAWFileLevel\', actual \'{0}\' (rdcsigLevel of " +
                        "InitializeFileTransferAsyncEvent, state S183)", TestManagerHelpers.Describe(rdcsigLevel)));
            this.Manager.Assert((isEOF == true), String.Format("expected \'true\', actual \'{0}\' (isEOF of InitializeFileTransferAsyncEvent, state S" +
                        "183)", TestManagerHelpers.Describe(isEOF)));
        }
        
        private void TCtestScenario3S6InitializeFileTransferAsyncEventChecker4(FRS2Model.ServerContext context, FRS2Model.RDC_Sig_Level rdcsigLevel, bool isEOF) {
            this.Manager.Comment("checking step \'event InitializeFileTransferAsyncEvent(ValidContext,forRAWFileLeve" +
                    "l,True)\'");
            this.Manager.Assert((context == FRS2Model.ServerContext.ValidContext), String.Format("expected \'FRS2Model.ServerContext.ValidContext\', actual \'{0}\' (context of Initial" +
                        "izeFileTransferAsyncEvent, state S183)", TestManagerHelpers.Describe(context)));
            this.Manager.Assert((rdcsigLevel == FRS2Model.RDC_Sig_Level.forRAWFileLevel), String.Format("expected \'FRS2Model.RDC_Sig_Level.forRAWFileLevel\', actual \'{0}\' (rdcsigLevel of " +
                        "InitializeFileTransferAsyncEvent, state S183)", TestManagerHelpers.Describe(rdcsigLevel)));
            this.Manager.Assert((isEOF == true), String.Format("expected \'true\', actual \'{0}\' (isEOF of InitializeFileTransferAsyncEvent, state S" +
                        "183)", TestManagerHelpers.Describe(isEOF)));
        }
        
        private void TCtestScenario3S6InitializeFileTransferAsyncEventChecker5(FRS2Model.ServerContext context, FRS2Model.RDC_Sig_Level rdcsigLevel, bool isEOF) {
            this.Manager.Comment("checking step \'event InitializeFileTransferAsyncEvent(InvalidContext,forRAWFileLe" +
                    "vel,False)\'");
            this.Manager.Assert((context == FRS2Model.ServerContext.InvalidContext), String.Format("expected \'FRS2Model.ServerContext.InvalidContext\', actual \'{0}\' (context of Initi" +
                        "alizeFileTransferAsyncEvent, state S183)", TestManagerHelpers.Describe(context)));
            this.Manager.Assert((rdcsigLevel == FRS2Model.RDC_Sig_Level.forRAWFileLevel), String.Format("expected \'FRS2Model.RDC_Sig_Level.forRAWFileLevel\', actual \'{0}\' (rdcsigLevel of " +
                        "InitializeFileTransferAsyncEvent, state S183)", TestManagerHelpers.Describe(rdcsigLevel)));
            this.Manager.Assert((isEOF == false), String.Format("expected \'false\', actual \'{0}\' (isEOF of InitializeFileTransferAsyncEvent, state " +
                        "S183)", TestManagerHelpers.Describe(isEOF)));
        }
        
        private void TCtestScenario3S6InitializeFileTransferAsyncEventChecker6(FRS2Model.ServerContext context, FRS2Model.RDC_Sig_Level rdcsigLevel, bool isEOF) {
            this.Manager.Comment("checking step \'event InitializeFileTransferAsyncEvent(InvalidContext,forRDCFileLe" +
                    "vel,True)\'");
            this.Manager.Assert((context == FRS2Model.ServerContext.InvalidContext), String.Format("expected \'FRS2Model.ServerContext.InvalidContext\', actual \'{0}\' (context of Initi" +
                        "alizeFileTransferAsyncEvent, state S183)", TestManagerHelpers.Describe(context)));
            this.Manager.Assert((rdcsigLevel == FRS2Model.RDC_Sig_Level.forRDCFileLevel), String.Format("expected \'FRS2Model.RDC_Sig_Level.forRDCFileLevel\', actual \'{0}\' (rdcsigLevel of " +
                        "InitializeFileTransferAsyncEvent, state S183)", TestManagerHelpers.Describe(rdcsigLevel)));
            this.Manager.Assert((isEOF == true), String.Format("expected \'true\', actual \'{0}\' (isEOF of InitializeFileTransferAsyncEvent, state S" +
                        "183)", TestManagerHelpers.Describe(isEOF)));
        }
        
        private void TCtestScenario3S6InitializeFileTransferAsyncEventChecker7(FRS2Model.ServerContext context, FRS2Model.RDC_Sig_Level rdcsigLevel, bool isEOF) {
            this.Manager.Comment("checking step \'event InitializeFileTransferAsyncEvent(ValidContext,forRDCFileLeve" +
                    "l,True)\'");
            this.Manager.Assert((context == FRS2Model.ServerContext.ValidContext), String.Format("expected \'FRS2Model.ServerContext.ValidContext\', actual \'{0}\' (context of Initial" +
                        "izeFileTransferAsyncEvent, state S183)", TestManagerHelpers.Describe(context)));
            this.Manager.Assert((rdcsigLevel == FRS2Model.RDC_Sig_Level.forRDCFileLevel), String.Format("expected \'FRS2Model.RDC_Sig_Level.forRDCFileLevel\', actual \'{0}\' (rdcsigLevel of " +
                        "InitializeFileTransferAsyncEvent, state S183)", TestManagerHelpers.Describe(rdcsigLevel)));
            this.Manager.Assert((isEOF == true), String.Format("expected \'true\', actual \'{0}\' (isEOF of InitializeFileTransferAsyncEvent, state S" +
                        "183)", TestManagerHelpers.Describe(isEOF)));
        }
        
        private void TCtestScenario3S6RequestUpdatesEventChecker1(FRS2Model.FilePresense present, FRS2Model.UPDATE_STATUS updateStatus) {
            this.Manager.Comment("checking step \'event RequestUpdatesEvent(fileExists,UPDATE_STATUS_DONE)\'");
            try {
                this.Manager.Assert((present == FRS2Model.FilePresense.fileExists), String.Format("expected \'FRS2Model.FilePresense.fileExists\', actual \'{0}\' (present of RequestUpd" +
                            "atesEvent, state S122)", TestManagerHelpers.Describe(present)));
                this.Manager.Assert((updateStatus == FRS2Model.UPDATE_STATUS.UPDATE_STATUS_DONE), String.Format("expected \'FRS2Model.UPDATE_STATUS.UPDATE_STATUS_DONE\', actual \'{0}\' (updateStatus" +
                            " of RequestUpdatesEvent, state S122)", TestManagerHelpers.Describe(updateStatus)));
            }
            catch (TransactionFailedException ) {
                this.Manager.Comment("This step would have covered MS-FRS2_R93, MS-FRS2_R94");
                throw;
            }
            this.Manager.Checkpoint("MS-FRS2_R93");
            this.Manager.Checkpoint("MS-FRS2_R94");
        }
        
        private void TCtestScenario3S6InitializeFileTransferAsyncEventChecker8(FRS2Model.ServerContext context, FRS2Model.RDC_Sig_Level rdcsigLevel, bool isEOF) {
            this.Manager.Comment("checking step \'event InitializeFileTransferAsyncEvent(ValidContext,forRDCFileLeve" +
                    "l,False)\'");
            this.Manager.Assert((context == FRS2Model.ServerContext.ValidContext), String.Format("expected \'FRS2Model.ServerContext.ValidContext\', actual \'{0}\' (context of Initial" +
                        "izeFileTransferAsyncEvent, state S184)", TestManagerHelpers.Describe(context)));
            this.Manager.Assert((rdcsigLevel == FRS2Model.RDC_Sig_Level.forRDCFileLevel), String.Format("expected \'FRS2Model.RDC_Sig_Level.forRDCFileLevel\', actual \'{0}\' (rdcsigLevel of " +
                        "InitializeFileTransferAsyncEvent, state S184)", TestManagerHelpers.Describe(rdcsigLevel)));
            this.Manager.Assert((isEOF == false), String.Format("expected \'false\', actual \'{0}\' (isEOF of InitializeFileTransferAsyncEvent, state " +
                        "S184)", TestManagerHelpers.Describe(isEOF)));
        }
        
        private void TCtestScenario3S272() {
            this.Manager.Comment("reaching state \'S272\'");
            FRS2Model.error_status_t temp109;
            this.Manager.Comment("executing step \'call RdcGetSignatures(valid)\'");
            temp109 = this.IFRS2ManagedAdapterInstance.RdcGetSignatures(FRS2Model.offset.valid);
            this.Manager.Checkpoint("MS-FRS2_R646");
            this.Manager.Checkpoint("MS-FRS2_R649");
            this.Manager.Checkpoint("MS-FRS2_R650");
            this.Manager.Comment("reaching state \'S367\'");
            this.Manager.Comment("checking step \'return RdcGetSignatures/ERROR_SUCCESS\'");
            this.Manager.Assert((temp109 == FRS2Model.error_status_t.ERROR_SUCCESS), String.Format("expected \'FRS2Model.error_status_t.ERROR_SUCCESS\', actual \'{0}\' (return of RdcGet" +
                        "Signatures, state S367)", TestManagerHelpers.Describe(temp109)));
            this.Manager.Comment("reaching state \'S400\'");
            FRS2Model.error_status_t temp110;
            this.Manager.Comment("executing step \'call RdcPushSourceNeeds()\'");
            temp110 = this.IFRS2ManagedAdapterInstance.RdcPushSourceNeeds();
            this.Manager.Checkpoint("MS-FRS2_R671");
            this.Manager.Checkpoint("MS-FRS2_R674");
            this.Manager.Checkpoint("MS-FRS2_R693");
            this.Manager.Checkpoint("MS-FRS2_R694");
            this.Manager.Checkpoint("MS-FRS2_R683");
            this.Manager.Checkpoint("MS-FRS2_R691");
            this.Manager.Checkpoint("MS-FRS2_R692");
            this.Manager.Checkpoint("MS-FRS2_R675");
            this.Manager.Comment("reaching state \'S412\'");
            this.Manager.Comment("checking step \'return RdcPushSourceNeeds/ERROR_SUCCESS\'");
            this.Manager.Assert((temp110 == FRS2Model.error_status_t.ERROR_SUCCESS), String.Format("expected \'FRS2Model.error_status_t.ERROR_SUCCESS\', actual \'{0}\' (return of RdcPus" +
                        "hSourceNeeds, state S412)", TestManagerHelpers.Describe(temp110)));
            this.Manager.Comment("reaching state \'S424\'");
            FRS2Model.error_status_t temp111;
            this.Manager.Comment("executing step \'call RdcGetFileData(validBufSize)\'");
            temp111 = this.IFRS2ManagedAdapterInstance.RdcGetFileData(FRS2Model.BufferSize.validBufSize);
            this.Manager.Checkpoint("MS-FRS2_R702");
            this.Manager.Comment("reaching state \'S433\'");
            this.Manager.Comment("checking step \'return RdcGetFileData/ERROR_SUCCESS\'");
            this.Manager.Assert((temp111 == FRS2Model.error_status_t.ERROR_SUCCESS), String.Format("expected \'FRS2Model.error_status_t.ERROR_SUCCESS\', actual \'{0}\' (return of RdcGet" +
                        "FileData, state S433)", TestManagerHelpers.Describe(temp111)));
            this.Manager.Comment("reaching state \'S442\'");
            FRS2Model.error_status_t temp112;
            this.Manager.Comment("executing step \'call RdcClose()\'");
            temp112 = this.IFRS2ManagedAdapterInstance.RdcClose();
            this.Manager.Checkpoint("MS-FRS2_R737");
            this.Manager.Checkpoint("MS-FRS2_R739");
            this.Manager.Comment("reaching state \'S445\'");
            this.Manager.Comment("checking step \'return RdcClose/ERROR_SUCCESS\'");
            this.Manager.Assert((temp112 == FRS2Model.error_status_t.ERROR_SUCCESS), String.Format("expected \'FRS2Model.error_status_t.ERROR_SUCCESS\', actual \'{0}\' (return of RdcClo" +
                        "se, state S445)", TestManagerHelpers.Describe(temp112)));
            this.Manager.Comment("reaching state \'S448\'");
        }
        
        private void TCtestScenario3S6InitializeFileTransferAsyncEventChecker9(FRS2Model.ServerContext context, FRS2Model.RDC_Sig_Level rdcsigLevel, bool isEOF) {
            this.Manager.Comment("checking step \'event InitializeFileTransferAsyncEvent(ValidContext,forRAWFileLeve" +
                    "l,False)\'");
            this.Manager.Assert((context == FRS2Model.ServerContext.ValidContext), String.Format("expected \'FRS2Model.ServerContext.ValidContext\', actual \'{0}\' (context of Initial" +
                        "izeFileTransferAsyncEvent, state S184)", TestManagerHelpers.Describe(context)));
            this.Manager.Assert((rdcsigLevel == FRS2Model.RDC_Sig_Level.forRAWFileLevel), String.Format("expected \'FRS2Model.RDC_Sig_Level.forRAWFileLevel\', actual \'{0}\' (rdcsigLevel of " +
                        "InitializeFileTransferAsyncEvent, state S184)", TestManagerHelpers.Describe(rdcsigLevel)));
            this.Manager.Assert((isEOF == false), String.Format("expected \'false\', actual \'{0}\' (isEOF of InitializeFileTransferAsyncEvent, state " +
                        "S184)", TestManagerHelpers.Describe(isEOF)));
        }
        
        private void TCtestScenario3S6InitializeFileTransferAsyncEventChecker10(FRS2Model.ServerContext context, FRS2Model.RDC_Sig_Level rdcsigLevel, bool isEOF) {
            this.Manager.Comment("checking step \'event InitializeFileTransferAsyncEvent(InvalidContext,forRAWFileLe" +
                    "vel,False)\'");
            this.Manager.Assert((context == FRS2Model.ServerContext.InvalidContext), String.Format("expected \'FRS2Model.ServerContext.InvalidContext\', actual \'{0}\' (context of Initi" +
                        "alizeFileTransferAsyncEvent, state S184)", TestManagerHelpers.Describe(context)));
            this.Manager.Assert((rdcsigLevel == FRS2Model.RDC_Sig_Level.forRAWFileLevel), String.Format("expected \'FRS2Model.RDC_Sig_Level.forRAWFileLevel\', actual \'{0}\' (rdcsigLevel of " +
                        "InitializeFileTransferAsyncEvent, state S184)", TestManagerHelpers.Describe(rdcsigLevel)));
            this.Manager.Assert((isEOF == false), String.Format("expected \'false\', actual \'{0}\' (isEOF of InitializeFileTransferAsyncEvent, state " +
                        "S184)", TestManagerHelpers.Describe(isEOF)));
        }
        
        private void TCtestScenario3S6InitializeFileTransferAsyncEventChecker11(FRS2Model.ServerContext context, FRS2Model.RDC_Sig_Level rdcsigLevel, bool isEOF) {
            this.Manager.Comment("checking step \'event InitializeFileTransferAsyncEvent(InvalidContext,forRDCFileLe" +
                    "vel,True)\'");
            this.Manager.Assert((context == FRS2Model.ServerContext.InvalidContext), String.Format("expected \'FRS2Model.ServerContext.InvalidContext\', actual \'{0}\' (context of Initi" +
                        "alizeFileTransferAsyncEvent, state S184)", TestManagerHelpers.Describe(context)));
            this.Manager.Assert((rdcsigLevel == FRS2Model.RDC_Sig_Level.forRDCFileLevel), String.Format("expected \'FRS2Model.RDC_Sig_Level.forRDCFileLevel\', actual \'{0}\' (rdcsigLevel of " +
                        "InitializeFileTransferAsyncEvent, state S184)", TestManagerHelpers.Describe(rdcsigLevel)));
            this.Manager.Assert((isEOF == true), String.Format("expected \'true\', actual \'{0}\' (isEOF of InitializeFileTransferAsyncEvent, state S" +
                        "184)", TestManagerHelpers.Describe(isEOF)));
        }
        
        private void TCtestScenario3S6InitializeFileTransferAsyncEventChecker12(FRS2Model.ServerContext context, FRS2Model.RDC_Sig_Level rdcsigLevel, bool isEOF) {
            this.Manager.Comment("checking step \'event InitializeFileTransferAsyncEvent(ValidContext,forRDCFileLeve" +
                    "l,True)\'");
            this.Manager.Assert((context == FRS2Model.ServerContext.ValidContext), String.Format("expected \'FRS2Model.ServerContext.ValidContext\', actual \'{0}\' (context of Initial" +
                        "izeFileTransferAsyncEvent, state S184)", TestManagerHelpers.Describe(context)));
            this.Manager.Assert((rdcsigLevel == FRS2Model.RDC_Sig_Level.forRDCFileLevel), String.Format("expected \'FRS2Model.RDC_Sig_Level.forRDCFileLevel\', actual \'{0}\' (rdcsigLevel of " +
                        "InitializeFileTransferAsyncEvent, state S184)", TestManagerHelpers.Describe(rdcsigLevel)));
            this.Manager.Assert((isEOF == true), String.Format("expected \'true\', actual \'{0}\' (isEOF of InitializeFileTransferAsyncEvent, state S" +
                        "184)", TestManagerHelpers.Describe(isEOF)));
        }
        
        private void TCtestScenario3S6InitializeFileTransferAsyncEventChecker13(FRS2Model.ServerContext context, FRS2Model.RDC_Sig_Level rdcsigLevel, bool isEOF) {
            this.Manager.Comment("checking step \'event InitializeFileTransferAsyncEvent(InvalidContext,forRDCFileLe" +
                    "vel,False)\'");
            this.Manager.Assert((context == FRS2Model.ServerContext.InvalidContext), String.Format("expected \'FRS2Model.ServerContext.InvalidContext\', actual \'{0}\' (context of Initi" +
                        "alizeFileTransferAsyncEvent, state S184)", TestManagerHelpers.Describe(context)));
            this.Manager.Assert((rdcsigLevel == FRS2Model.RDC_Sig_Level.forRDCFileLevel), String.Format("expected \'FRS2Model.RDC_Sig_Level.forRDCFileLevel\', actual \'{0}\' (rdcsigLevel of " +
                        "InitializeFileTransferAsyncEvent, state S184)", TestManagerHelpers.Describe(rdcsigLevel)));
            this.Manager.Assert((isEOF == false), String.Format("expected \'false\', actual \'{0}\' (isEOF of InitializeFileTransferAsyncEvent, state " +
                        "S184)", TestManagerHelpers.Describe(isEOF)));
        }
        
        private void TCtestScenario3S6InitializeFileTransferAsyncEventChecker14(FRS2Model.ServerContext context, FRS2Model.RDC_Sig_Level rdcsigLevel, bool isEOF) {
            this.Manager.Comment("checking step \'event InitializeFileTransferAsyncEvent(InvalidContext,forRAWFileLe" +
                    "vel,True)\'");
            this.Manager.Assert((context == FRS2Model.ServerContext.InvalidContext), String.Format("expected \'FRS2Model.ServerContext.InvalidContext\', actual \'{0}\' (context of Initi" +
                        "alizeFileTransferAsyncEvent, state S184)", TestManagerHelpers.Describe(context)));
            this.Manager.Assert((rdcsigLevel == FRS2Model.RDC_Sig_Level.forRAWFileLevel), String.Format("expected \'FRS2Model.RDC_Sig_Level.forRAWFileLevel\', actual \'{0}\' (rdcsigLevel of " +
                        "InitializeFileTransferAsyncEvent, state S184)", TestManagerHelpers.Describe(rdcsigLevel)));
            this.Manager.Assert((isEOF == true), String.Format("expected \'true\', actual \'{0}\' (isEOF of InitializeFileTransferAsyncEvent, state S" +
                        "184)", TestManagerHelpers.Describe(isEOF)));
        }
        
        private void TCtestScenario3S6InitializeFileTransferAsyncEventChecker15(FRS2Model.ServerContext context, FRS2Model.RDC_Sig_Level rdcsigLevel, bool isEOF) {
            this.Manager.Comment("checking step \'event InitializeFileTransferAsyncEvent(ValidContext,forRAWFileLeve" +
                    "l,True)\'");
            this.Manager.Assert((context == FRS2Model.ServerContext.ValidContext), String.Format("expected \'FRS2Model.ServerContext.ValidContext\', actual \'{0}\' (context of Initial" +
                        "izeFileTransferAsyncEvent, state S184)", TestManagerHelpers.Describe(context)));
            this.Manager.Assert((rdcsigLevel == FRS2Model.RDC_Sig_Level.forRAWFileLevel), String.Format("expected \'FRS2Model.RDC_Sig_Level.forRAWFileLevel\', actual \'{0}\' (rdcsigLevel of " +
                        "InitializeFileTransferAsyncEvent, state S184)", TestManagerHelpers.Describe(rdcsigLevel)));
            this.Manager.Assert((isEOF == true), String.Format("expected \'true\', actual \'{0}\' (isEOF of InitializeFileTransferAsyncEvent, state S" +
                        "184)", TestManagerHelpers.Describe(isEOF)));
        }
        
        private void TCtestScenario3S6RequestUpdatesEventChecker2(FRS2Model.FilePresense present, FRS2Model.UPDATE_STATUS updateStatus) {
            this.Manager.Comment("checking step \'event RequestUpdatesEvent(fileExists,UPDATE_STATUS_MORE)\'");
            try {
                this.Manager.Assert((present == FRS2Model.FilePresense.fileExists), String.Format("expected \'FRS2Model.FilePresense.fileExists\', actual \'{0}\' (present of RequestUpd" +
                            "atesEvent, state S122)", TestManagerHelpers.Describe(present)));
                this.Manager.Assert((updateStatus == FRS2Model.UPDATE_STATUS.UPDATE_STATUS_MORE), String.Format("expected \'FRS2Model.UPDATE_STATUS.UPDATE_STATUS_MORE\', actual \'{0}\' (updateStatus" +
                            " of RequestUpdatesEvent, state S122)", TestManagerHelpers.Describe(updateStatus)));
            }
            catch (TransactionFailedException ) {
                this.Manager.Comment("This step would have covered MS-FRS2_R93, MS-FRS2_R498");
                throw;
            }
            this.Manager.Checkpoint("MS-FRS2_R93");
            this.Manager.Checkpoint("MS-FRS2_R498");
        }
        
        private void TCtestScenario3S141() {
            this.Manager.Comment("reaching state \'S141\'");
            FRS2Model.error_status_t temp114;
            this.Manager.Comment("executing step \'call InitializeFileTransferAsync(1,1,True)\'");
            temp114 = this.IFRS2ManagedAdapterInstance.InitializeFileTransferAsync(1, 1, true);
            this.Manager.Checkpoint("MS-FRS2_R774");
            this.Manager.Checkpoint("MS-FRS2_R777");
            this.Manager.Checkpoint("MS-FRS2_R793");
            this.Manager.Comment("reaching state \'S167\'");
            this.Manager.Comment("checking step \'return InitializeFileTransferAsync/ERROR_SUCCESS\'");
            this.Manager.Assert((temp114 == FRS2Model.error_status_t.ERROR_SUCCESS), String.Format("expected \'FRS2Model.error_status_t.ERROR_SUCCESS\', actual \'{0}\' (return of Initia" +
                        "lizeFileTransferAsync, state S167)", TestManagerHelpers.Describe(temp114)));
            this.Manager.Comment("reaching state \'S185\'");
            int temp119 = this.Manager.ExpectEvent(this.QuiescenceTimeout, true, new ExpectedEvent(TCtestScenario3.InitializeFileTransferAsyncEventInfo, null, new InitializeFileTransferAsyncEventDelegate1(this.TCtestScenario3S6InitializeFileTransferAsyncEventChecker16)), new ExpectedEvent(TCtestScenario3.InitializeFileTransferAsyncEventInfo, null, new InitializeFileTransferAsyncEventDelegate1(this.TCtestScenario3S6InitializeFileTransferAsyncEventChecker17)), new ExpectedEvent(TCtestScenario3.InitializeFileTransferAsyncEventInfo, null, new InitializeFileTransferAsyncEventDelegate1(this.TCtestScenario3S6InitializeFileTransferAsyncEventChecker18)), new ExpectedEvent(TCtestScenario3.InitializeFileTransferAsyncEventInfo, null, new InitializeFileTransferAsyncEventDelegate1(this.TCtestScenario3S6InitializeFileTransferAsyncEventChecker19)), new ExpectedEvent(TCtestScenario3.InitializeFileTransferAsyncEventInfo, null, new InitializeFileTransferAsyncEventDelegate1(this.TCtestScenario3S6InitializeFileTransferAsyncEventChecker20)), new ExpectedEvent(TCtestScenario3.InitializeFileTransferAsyncEventInfo, null, new InitializeFileTransferAsyncEventDelegate1(this.TCtestScenario3S6InitializeFileTransferAsyncEventChecker21)), new ExpectedEvent(TCtestScenario3.InitializeFileTransferAsyncEventInfo, null, new InitializeFileTransferAsyncEventDelegate1(this.TCtestScenario3S6InitializeFileTransferAsyncEventChecker22)), new ExpectedEvent(TCtestScenario3.InitializeFileTransferAsyncEventInfo, null, new InitializeFileTransferAsyncEventDelegate1(this.TCtestScenario3S6InitializeFileTransferAsyncEventChecker23)));
            if ((temp119 == 0)) {
                this.Manager.Comment("reaching state \'S280\'");
                FRS2Model.error_status_t temp115;
                this.Manager.Comment("executing step \'call RdcGetSignatures(valid)\'");
                temp115 = this.IFRS2ManagedAdapterInstance.RdcGetSignatures(FRS2Model.offset.valid);
                this.Manager.Checkpoint("MS-FRS2_R646");
                this.Manager.Checkpoint("MS-FRS2_R649");
                this.Manager.Checkpoint("MS-FRS2_R650");
                this.Manager.Comment("reaching state \'S368\'");
                this.Manager.Comment("checking step \'return RdcGetSignatures/ERROR_SUCCESS\'");
                this.Manager.Assert((temp115 == FRS2Model.error_status_t.ERROR_SUCCESS), String.Format("expected \'FRS2Model.error_status_t.ERROR_SUCCESS\', actual \'{0}\' (return of RdcGet" +
                            "Signatures, state S368)", TestManagerHelpers.Describe(temp115)));
                this.Manager.Comment("reaching state \'S401\'");
                FRS2Model.error_status_t temp116;
                this.Manager.Comment("executing step \'call RdcPushSourceNeeds()\'");
                temp116 = this.IFRS2ManagedAdapterInstance.RdcPushSourceNeeds();
                this.Manager.Checkpoint("MS-FRS2_R671");
                this.Manager.Checkpoint("MS-FRS2_R674");
                this.Manager.Checkpoint("MS-FRS2_R693");
                this.Manager.Checkpoint("MS-FRS2_R694");
                this.Manager.Checkpoint("MS-FRS2_R683");
                this.Manager.Checkpoint("MS-FRS2_R691");
                this.Manager.Checkpoint("MS-FRS2_R692");
                this.Manager.Checkpoint("MS-FRS2_R675");
                this.Manager.Comment("reaching state \'S413\'");
                this.Manager.Comment("checking step \'return RdcPushSourceNeeds/ERROR_SUCCESS\'");
                this.Manager.Assert((temp116 == FRS2Model.error_status_t.ERROR_SUCCESS), String.Format("expected \'FRS2Model.error_status_t.ERROR_SUCCESS\', actual \'{0}\' (return of RdcPus" +
                            "hSourceNeeds, state S413)", TestManagerHelpers.Describe(temp116)));
                this.Manager.Comment("reaching state \'S425\'");
                FRS2Model.error_status_t temp117;
                this.Manager.Comment("executing step \'call RdcGetFileData(validBufSize)\'");
                temp117 = this.IFRS2ManagedAdapterInstance.RdcGetFileData(FRS2Model.BufferSize.validBufSize);
                this.Manager.Checkpoint("MS-FRS2_R702");
                this.Manager.Comment("reaching state \'S434\'");
                this.Manager.Comment("checking step \'return RdcGetFileData/ERROR_SUCCESS\'");
                this.Manager.Assert((temp117 == FRS2Model.error_status_t.ERROR_SUCCESS), String.Format("expected \'FRS2Model.error_status_t.ERROR_SUCCESS\', actual \'{0}\' (return of RdcGet" +
                            "FileData, state S434)", TestManagerHelpers.Describe(temp117)));
                this.Manager.Comment("reaching state \'S443\'");
                FRS2Model.error_status_t temp118;
                this.Manager.Comment("executing step \'call RdcClose()\'");
                temp118 = this.IFRS2ManagedAdapterInstance.RdcClose();
                this.Manager.Checkpoint("MS-FRS2_R737");
                this.Manager.Checkpoint("MS-FRS2_R739");
                this.Manager.Comment("reaching state \'S446\'");
                this.Manager.Comment("checking step \'return RdcClose/ERROR_SUCCESS\'");
                this.Manager.Assert((temp118 == FRS2Model.error_status_t.ERROR_SUCCESS), String.Format("expected \'FRS2Model.error_status_t.ERROR_SUCCESS\', actual \'{0}\' (return of RdcClo" +
                            "se, state S446)", TestManagerHelpers.Describe(temp118)));
                this.Manager.Comment("reaching state \'S449\'");
                goto label17;
            }
            if ((temp119 == 1)) {
                TCtestScenario3S214();
                goto label17;
            }
            if ((temp119 == 2)) {
                TCtestScenario3S213();
                goto label17;
            }
            if ((temp119 == 3)) {
                TCtestScenario3S216();
                goto label17;
            }
            if ((temp119 == 4)) {
                TCtestScenario3S211();
                goto label17;
            }
            if ((temp119 == 5)) {
                TCtestScenario3S210();
                goto label17;
            }
            if ((temp119 == 6)) {
                TCtestScenario3S222();
                goto label17;
            }
            if ((temp119 == 7)) {
                TCtestScenario3S223();
                goto label17;
            }
            throw new InvalidOperationException("never reached");
        label17:
;
        }
        
        private void TCtestScenario3S6InitializeFileTransferAsyncEventChecker16(FRS2Model.ServerContext context, FRS2Model.RDC_Sig_Level rdcsigLevel, bool isEOF) {
            this.Manager.Comment("checking step \'event InitializeFileTransferAsyncEvent(ValidContext,forRDCFileLeve" +
                    "l,False)\'");
            this.Manager.Assert((context == FRS2Model.ServerContext.ValidContext), String.Format("expected \'FRS2Model.ServerContext.ValidContext\', actual \'{0}\' (context of Initial" +
                        "izeFileTransferAsyncEvent, state S185)", TestManagerHelpers.Describe(context)));
            this.Manager.Assert((rdcsigLevel == FRS2Model.RDC_Sig_Level.forRDCFileLevel), String.Format("expected \'FRS2Model.RDC_Sig_Level.forRDCFileLevel\', actual \'{0}\' (rdcsigLevel of " +
                        "InitializeFileTransferAsyncEvent, state S185)", TestManagerHelpers.Describe(rdcsigLevel)));
            this.Manager.Assert((isEOF == false), String.Format("expected \'false\', actual \'{0}\' (isEOF of InitializeFileTransferAsyncEvent, state " +
                        "S185)", TestManagerHelpers.Describe(isEOF)));
        }
        
        private void TCtestScenario3S6InitializeFileTransferAsyncEventChecker17(FRS2Model.ServerContext context, FRS2Model.RDC_Sig_Level rdcsigLevel, bool isEOF) {
            this.Manager.Comment("checking step \'event InitializeFileTransferAsyncEvent(ValidContext,forRAWFileLeve" +
                    "l,False)\'");
            this.Manager.Assert((context == FRS2Model.ServerContext.ValidContext), String.Format("expected \'FRS2Model.ServerContext.ValidContext\', actual \'{0}\' (context of Initial" +
                        "izeFileTransferAsyncEvent, state S185)", TestManagerHelpers.Describe(context)));
            this.Manager.Assert((rdcsigLevel == FRS2Model.RDC_Sig_Level.forRAWFileLevel), String.Format("expected \'FRS2Model.RDC_Sig_Level.forRAWFileLevel\', actual \'{0}\' (rdcsigLevel of " +
                        "InitializeFileTransferAsyncEvent, state S185)", TestManagerHelpers.Describe(rdcsigLevel)));
            this.Manager.Assert((isEOF == false), String.Format("expected \'false\', actual \'{0}\' (isEOF of InitializeFileTransferAsyncEvent, state " +
                        "S185)", TestManagerHelpers.Describe(isEOF)));
        }
        
        private void TCtestScenario3S6InitializeFileTransferAsyncEventChecker18(FRS2Model.ServerContext context, FRS2Model.RDC_Sig_Level rdcsigLevel, bool isEOF) {
            this.Manager.Comment("checking step \'event InitializeFileTransferAsyncEvent(InvalidContext,forRAWFileLe" +
                    "vel,False)\'");
            this.Manager.Assert((context == FRS2Model.ServerContext.InvalidContext), String.Format("expected \'FRS2Model.ServerContext.InvalidContext\', actual \'{0}\' (context of Initi" +
                        "alizeFileTransferAsyncEvent, state S185)", TestManagerHelpers.Describe(context)));
            this.Manager.Assert((rdcsigLevel == FRS2Model.RDC_Sig_Level.forRAWFileLevel), String.Format("expected \'FRS2Model.RDC_Sig_Level.forRAWFileLevel\', actual \'{0}\' (rdcsigLevel of " +
                        "InitializeFileTransferAsyncEvent, state S185)", TestManagerHelpers.Describe(rdcsigLevel)));
            this.Manager.Assert((isEOF == false), String.Format("expected \'false\', actual \'{0}\' (isEOF of InitializeFileTransferAsyncEvent, state " +
                        "S185)", TestManagerHelpers.Describe(isEOF)));
        }
        
        private void TCtestScenario3S6InitializeFileTransferAsyncEventChecker19(FRS2Model.ServerContext context, FRS2Model.RDC_Sig_Level rdcsigLevel, bool isEOF) {
            this.Manager.Comment("checking step \'event InitializeFileTransferAsyncEvent(InvalidContext,forRDCFileLe" +
                    "vel,True)\'");
            this.Manager.Assert((context == FRS2Model.ServerContext.InvalidContext), String.Format("expected \'FRS2Model.ServerContext.InvalidContext\', actual \'{0}\' (context of Initi" +
                        "alizeFileTransferAsyncEvent, state S185)", TestManagerHelpers.Describe(context)));
            this.Manager.Assert((rdcsigLevel == FRS2Model.RDC_Sig_Level.forRDCFileLevel), String.Format("expected \'FRS2Model.RDC_Sig_Level.forRDCFileLevel\', actual \'{0}\' (rdcsigLevel of " +
                        "InitializeFileTransferAsyncEvent, state S185)", TestManagerHelpers.Describe(rdcsigLevel)));
            this.Manager.Assert((isEOF == true), String.Format("expected \'true\', actual \'{0}\' (isEOF of InitializeFileTransferAsyncEvent, state S" +
                        "185)", TestManagerHelpers.Describe(isEOF)));
        }
        
        private void TCtestScenario3S6InitializeFileTransferAsyncEventChecker20(FRS2Model.ServerContext context, FRS2Model.RDC_Sig_Level rdcsigLevel, bool isEOF) {
            this.Manager.Comment("checking step \'event InitializeFileTransferAsyncEvent(ValidContext,forRDCFileLeve" +
                    "l,True)\'");
            this.Manager.Assert((context == FRS2Model.ServerContext.ValidContext), String.Format("expected \'FRS2Model.ServerContext.ValidContext\', actual \'{0}\' (context of Initial" +
                        "izeFileTransferAsyncEvent, state S185)", TestManagerHelpers.Describe(context)));
            this.Manager.Assert((rdcsigLevel == FRS2Model.RDC_Sig_Level.forRDCFileLevel), String.Format("expected \'FRS2Model.RDC_Sig_Level.forRDCFileLevel\', actual \'{0}\' (rdcsigLevel of " +
                        "InitializeFileTransferAsyncEvent, state S185)", TestManagerHelpers.Describe(rdcsigLevel)));
            this.Manager.Assert((isEOF == true), String.Format("expected \'true\', actual \'{0}\' (isEOF of InitializeFileTransferAsyncEvent, state S" +
                        "185)", TestManagerHelpers.Describe(isEOF)));
        }
        
        private void TCtestScenario3S6InitializeFileTransferAsyncEventChecker21(FRS2Model.ServerContext context, FRS2Model.RDC_Sig_Level rdcsigLevel, bool isEOF) {
            this.Manager.Comment("checking step \'event InitializeFileTransferAsyncEvent(InvalidContext,forRDCFileLe" +
                    "vel,False)\'");
            this.Manager.Assert((context == FRS2Model.ServerContext.InvalidContext), String.Format("expected \'FRS2Model.ServerContext.InvalidContext\', actual \'{0}\' (context of Initi" +
                        "alizeFileTransferAsyncEvent, state S185)", TestManagerHelpers.Describe(context)));
            this.Manager.Assert((rdcsigLevel == FRS2Model.RDC_Sig_Level.forRDCFileLevel), String.Format("expected \'FRS2Model.RDC_Sig_Level.forRDCFileLevel\', actual \'{0}\' (rdcsigLevel of " +
                        "InitializeFileTransferAsyncEvent, state S185)", TestManagerHelpers.Describe(rdcsigLevel)));
            this.Manager.Assert((isEOF == false), String.Format("expected \'false\', actual \'{0}\' (isEOF of InitializeFileTransferAsyncEvent, state " +
                        "S185)", TestManagerHelpers.Describe(isEOF)));
        }
        
        private void TCtestScenario3S6InitializeFileTransferAsyncEventChecker22(FRS2Model.ServerContext context, FRS2Model.RDC_Sig_Level rdcsigLevel, bool isEOF) {
            this.Manager.Comment("checking step \'event InitializeFileTransferAsyncEvent(InvalidContext,forRAWFileLe" +
                    "vel,True)\'");
            this.Manager.Assert((context == FRS2Model.ServerContext.InvalidContext), String.Format("expected \'FRS2Model.ServerContext.InvalidContext\', actual \'{0}\' (context of Initi" +
                        "alizeFileTransferAsyncEvent, state S185)", TestManagerHelpers.Describe(context)));
            this.Manager.Assert((rdcsigLevel == FRS2Model.RDC_Sig_Level.forRAWFileLevel), String.Format("expected \'FRS2Model.RDC_Sig_Level.forRAWFileLevel\', actual \'{0}\' (rdcsigLevel of " +
                        "InitializeFileTransferAsyncEvent, state S185)", TestManagerHelpers.Describe(rdcsigLevel)));
            this.Manager.Assert((isEOF == true), String.Format("expected \'true\', actual \'{0}\' (isEOF of InitializeFileTransferAsyncEvent, state S" +
                        "185)", TestManagerHelpers.Describe(isEOF)));
        }
        
        private void TCtestScenario3S6InitializeFileTransferAsyncEventChecker23(FRS2Model.ServerContext context, FRS2Model.RDC_Sig_Level rdcsigLevel, bool isEOF) {
            this.Manager.Comment("checking step \'event InitializeFileTransferAsyncEvent(ValidContext,forRAWFileLeve" +
                    "l,True)\'");
            this.Manager.Assert((context == FRS2Model.ServerContext.ValidContext), String.Format("expected \'FRS2Model.ServerContext.ValidContext\', actual \'{0}\' (context of Initial" +
                        "izeFileTransferAsyncEvent, state S185)", TestManagerHelpers.Describe(context)));
            this.Manager.Assert((rdcsigLevel == FRS2Model.RDC_Sig_Level.forRAWFileLevel), String.Format("expected \'FRS2Model.RDC_Sig_Level.forRAWFileLevel\', actual \'{0}\' (rdcsigLevel of " +
                        "InitializeFileTransferAsyncEvent, state S185)", TestManagerHelpers.Describe(rdcsigLevel)));
            this.Manager.Assert((isEOF == true), String.Format("expected \'true\', actual \'{0}\' (isEOF of InitializeFileTransferAsyncEvent, state S" +
                        "185)", TestManagerHelpers.Describe(isEOF)));
        }
        
        private void TCtestScenario3S6RequestUpdatesEventChecker3(FRS2Model.FilePresense present, FRS2Model.UPDATE_STATUS updateStatus) {
            this.Manager.Comment("checking step \'event RequestUpdatesEvent(fileDeleted,UPDATE_STATUS_MORE)\'");
            try {
                this.Manager.Assert((present == FRS2Model.FilePresense.fileDeleted), String.Format("expected \'FRS2Model.FilePresense.fileDeleted\', actual \'{0}\' (present of RequestUp" +
                            "datesEvent, state S122)", TestManagerHelpers.Describe(present)));
                this.Manager.Assert((updateStatus == FRS2Model.UPDATE_STATUS.UPDATE_STATUS_MORE), String.Format("expected \'FRS2Model.UPDATE_STATUS.UPDATE_STATUS_MORE\', actual \'{0}\' (updateStatus" +
                            " of RequestUpdatesEvent, state S122)", TestManagerHelpers.Describe(updateStatus)));
            }
            catch (TransactionFailedException ) {
                this.Manager.Comment("This step would have covered MS-FRS2_R93");
                throw;
            }
            this.Manager.Checkpoint("MS-FRS2_R93");
        }
        
        private void TCtestScenario3S6AsyncPollResponseEventChecker1(FRS2Model.VVGeneration vvGen) {
            this.Manager.Comment("checking step \'event AsyncPollResponseEvent(InvalidValue)\'");
            try {
                this.Manager.Assert((vvGen == FRS2Model.VVGeneration.InvalidValue), String.Format("expected \'FRS2Model.VVGeneration.InvalidValue\', actual \'{0}\' (vvGen of AsyncPollR" +
                            "esponseEvent, state S90)", TestManagerHelpers.Describe(vvGen)));
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
        [Microsoft.VisualStudio.TestTools.UnitTesting.DescriptionAttribute(@"MS-FRS2_R37, MS-FRS2_R436, MS-FRS2_R439, MS-FRS2_R455, MS-FRS2_R458, MS-FRS2_R448, MS-FRS2_R549, MS-FRS2_R552, MS-FRS2_R517, MS-FRS2_R520, MS-FRS2_R466, MS-FRS2_R556, MS-FRS2_R1020, MS-FRS2_R555, MS-FRS2_R486, MS-FRS2_R489, MS-FRS2_R93, MS-FRS2_R94, MS-FRS2_R774, MS-FRS2_R777, MS-FRS2_R793, MS-FRS2_R93, MS-FRS2_R94, MS-FRS2_R774, MS-FRS2_R777, MS-FRS2_R793, MS-FRS2_R93, MS-FRS2_R498, MS-FRS2_R93, MS-FRS2_R556, MS-FRS2_R1020, MS-FRS2_R555")]
        public virtual void FRS2_TCtestScenario3S8() {
            this.Manager.BeginTest("TCtestScenario3S8");
            this.Manager.Comment("reaching state \'S8\'");
            FRS2Model.error_status_t temp122;
            this.Manager.Comment(@"executing step 'call Initialization(Windows7,Map{1=>enabled,2=>enabled,3=>enabled,4=>disabled},Map{1=>exists,2=>exists,3=>exists,4=>exists},Map{""A""=>1,""B""=>2,""C""=>3,""D""=>4},Map{""A""=>sysvol,""B""=>protection,""C""=>sysvol,""D""=>sysvol},Map{1=>""A"",2=>""B"",3=>""C"",4=>""D""},Map{1=>writeOnly,2=>readWrite,3=>readOnly,4=>readWrite},Map{1=>enabled,2=>disabled,3=>enabled,4=>enabled})'");
            temp122 = this.IFRS2ManagedAdapterInstance.Initialization(FRS2Model.OSVersion.Windows7, new Microsoft.Modeling.Map<System.Int32,FRS2Model.connectionProperty>().Add(1, FRS2Model.connectionProperty.enabled).Add(2, FRS2Model.connectionProperty.enabled).Add(3, FRS2Model.connectionProperty.enabled).Add(4, FRS2Model.connectionProperty.disabled), new Microsoft.Modeling.Map<System.Int32,FRS2Model.FromServer>().Add(1, FRS2Model.FromServer.exists).Add(2, FRS2Model.FromServer.exists).Add(3, FRS2Model.FromServer.exists).Add(4, FRS2Model.FromServer.exists), new Microsoft.Modeling.Map<System.String,System.Int32>().Add("A", 1).Add("B", 2).Add("C", 3).Add("D", 4), new Microsoft.Modeling.Map<System.String,FRS2Model.ReplicationGroupTypes>().Add("A", FRS2Model.ReplicationGroupTypes.sysvol).Add("B", FRS2Model.ReplicationGroupTypes.protection).Add("C", FRS2Model.ReplicationGroupTypes.sysvol).Add("D", FRS2Model.ReplicationGroupTypes.sysvol), new Microsoft.Modeling.Map<System.Int32,System.String>().Add(1, "A").Add(2, "B").Add(3, "C").Add(4, "D"), new Microsoft.Modeling.Map<System.Int32,FRS2Model.accessLevels>().Add(1, FRS2Model.accessLevels.writeOnly).Add(2, FRS2Model.accessLevels.readWrite).Add(3, FRS2Model.accessLevels.readOnly).Add(4, FRS2Model.accessLevels.readWrite), new Microsoft.Modeling.Map<System.Int32,FRS2Model.connectionProperty>().Add(1, FRS2Model.connectionProperty.enabled).Add(2, FRS2Model.connectionProperty.disabled).Add(3, FRS2Model.connectionProperty.enabled).Add(4, FRS2Model.connectionProperty.enabled));
            this.Manager.Comment("reaching state \'S9\'");
            this.Manager.Comment("checking step \'return Initialization/ERROR_SUCCESS\'");
            this.Manager.Assert((temp122 == FRS2Model.error_status_t.ERROR_SUCCESS), String.Format("expected \'FRS2Model.error_status_t.ERROR_SUCCESS\', actual \'{0}\' (return of Initia" +
                        "lization, state S9)", TestManagerHelpers.Describe(temp122)));
            this.Manager.Comment("reaching state \'S22\'");
            FRS2Model.ProtocolVersionReturned temp123;
            FRS2Model.UpstreamFlagValueReturned temp124;
            FRS2Model.error_status_t temp125;
            this.Manager.Comment("executing step \'call EstablishConnection(\"A\",1,FRS_COMMUNICATION_PROTOCOL_VERSION" +
                    "_LONGHORN_SERVER,out _,out _)\'");
            temp125 = this.IFRS2ManagedAdapterInstance.EstablishConnection("A", 1, FRS2Model.ProtocolVersion.FRS_COMMUNICATION_PROTOCOL_VERSION_LONGHORN_SERVER, out temp123, out temp124);
            this.Manager.Checkpoint("MS-FRS2_R37");
            this.Manager.Checkpoint("MS-FRS2_R436");
            this.Manager.Checkpoint("MS-FRS2_R439");
            this.Manager.Comment("reaching state \'S31\'");
            this.Manager.Comment("checking step \'return EstablishConnection/[out Valid,out Valid]:ERROR_SUCCESS\'");
            this.Manager.Assert((temp123 == FRS2Model.ProtocolVersionReturned.Valid), String.Format("expected \'FRS2Model.ProtocolVersionReturned.Valid\', actual \'{0}\' (upstreamProtoco" +
                        "lVersion of EstablishConnection, state S31)", TestManagerHelpers.Describe(temp123)));
            this.Manager.Assert((temp124 == FRS2Model.UpstreamFlagValueReturned.Valid), String.Format("expected \'FRS2Model.UpstreamFlagValueReturned.Valid\', actual \'{0}\' (upstreamFlags" +
                        " of EstablishConnection, state S31)", TestManagerHelpers.Describe(temp124)));
            this.Manager.Assert((temp125 == FRS2Model.error_status_t.ERROR_SUCCESS), String.Format("expected \'FRS2Model.error_status_t.ERROR_SUCCESS\', actual \'{0}\' (return of Establ" +
                        "ishConnection, state S31)", TestManagerHelpers.Describe(temp125)));
            this.Manager.Comment("reaching state \'S40\'");
            FRS2Model.error_status_t temp126;
            this.Manager.Comment("executing step \'call EstablishSession(1,1)\'");
            temp126 = this.IFRS2ManagedAdapterInstance.EstablishSession(1, 1);
            this.Manager.Checkpoint("MS-FRS2_R455");
            this.Manager.Checkpoint("MS-FRS2_R458");
            this.Manager.Checkpoint("MS-FRS2_R448");
            this.Manager.Comment("reaching state \'S49\'");
            this.Manager.Comment("checking step \'return EstablishSession/ERROR_SUCCESS\'");
            this.Manager.Assert((temp126 == FRS2Model.error_status_t.ERROR_SUCCESS), String.Format("expected \'FRS2Model.error_status_t.ERROR_SUCCESS\', actual \'{0}\' (return of Establ" +
                        "ishSession, state S49)", TestManagerHelpers.Describe(temp126)));
            this.Manager.Comment("reaching state \'S58\'");
            FRS2Model.error_status_t temp127;
            this.Manager.Comment("executing step \'call AsyncPoll(1)\'");
            temp127 = this.IFRS2ManagedAdapterInstance.AsyncPoll(1);
            this.Manager.Checkpoint("MS-FRS2_R549");
            this.Manager.Checkpoint("MS-FRS2_R552");
            this.Manager.Comment("reaching state \'S67\'");
            this.Manager.Comment("checking step \'return AsyncPoll/ERROR_SUCCESS\'");
            this.Manager.Assert((temp127 == FRS2Model.error_status_t.ERROR_SUCCESS), String.Format("expected \'FRS2Model.error_status_t.ERROR_SUCCESS\', actual \'{0}\' (return of AsyncP" +
                        "oll, state S67)", TestManagerHelpers.Describe(temp127)));
            this.Manager.Comment("reaching state \'S75\'");
            FRS2Model.error_status_t temp128;
            this.Manager.Comment("executing step \'call RequestVersionVector(1,1,1,REQUEST_NORMAL_SYNC,CHANGE_ALL,Va" +
                    "lidValue)\'");
            temp128 = this.IFRS2ManagedAdapterInstance.RequestVersionVector(1, 1, 1, FRS2Model.VERSION_REQUEST_TYPE.REQUEST_NORMAL_SYNC, FRS2Model.VERSION_CHANGE_TYPE.CHANGE_ALL, FRS2Model.VVGeneration.ValidValue);
            this.Manager.Checkpoint("MS-FRS2_R517");
            this.Manager.Checkpoint("MS-FRS2_R520");
            this.Manager.Checkpoint("MS-FRS2_R466");
            this.Manager.Comment("reaching state \'S83\'");
            this.Manager.Comment("checking step \'return RequestVersionVector/ERROR_SUCCESS\'");
            this.Manager.Assert((temp128 == FRS2Model.error_status_t.ERROR_SUCCESS), String.Format("expected \'FRS2Model.error_status_t.ERROR_SUCCESS\', actual \'{0}\' (return of Reques" +
                        "tVersionVector, state S83)", TestManagerHelpers.Describe(temp128)));
            this.Manager.Comment("reaching state \'S91\'");
            int temp135 = this.Manager.ExpectEvent(this.QuiescenceTimeout, true, new ExpectedEvent(TCtestScenario3.AsyncPollResponseEventInfo, null, new AsyncPollResponseEventDelegate1(this.TCtestScenario3S8AsyncPollResponseEventChecker)), new ExpectedEvent(TCtestScenario3.AsyncPollResponseEventInfo, null, new AsyncPollResponseEventDelegate1(this.TCtestScenario3S8AsyncPollResponseEventChecker1)));
            if ((temp135 == 0)) {
                this.Manager.Comment("reaching state \'S103\'");
                FRS2Model.error_status_t temp129;
                this.Manager.Comment("executing step \'call RequestUpdates(1,1,valid)\'");
                temp129 = this.IFRS2ManagedAdapterInstance.RequestUpdates(1, 1, FRS2Model.versionVectorDiff.valid);
                this.Manager.Checkpoint("MS-FRS2_R486");
                this.Manager.Checkpoint("MS-FRS2_R489");
                this.Manager.Comment("reaching state \'S115\'");
                this.Manager.Comment("checking step \'return RequestUpdates/ERROR_SUCCESS\'");
                this.Manager.Assert((temp129 == FRS2Model.error_status_t.ERROR_SUCCESS), String.Format("expected \'FRS2Model.error_status_t.ERROR_SUCCESS\', actual \'{0}\' (return of Reques" +
                            "tUpdates, state S115)", TestManagerHelpers.Describe(temp129)));
                this.Manager.Comment("reaching state \'S123\'");
                int temp134 = this.Manager.ExpectEvent(this.QuiescenceTimeout, true, new ExpectedEvent(TCtestScenario3.RequestUpdatesEventInfo, null, new RequestUpdatesEventDelegate1(this.TCtestScenario3S8RequestUpdatesEventChecker)), new ExpectedEvent(TCtestScenario3.RequestUpdatesEventInfo, null, new RequestUpdatesEventDelegate1(this.TCtestScenario3S8RequestUpdatesEventChecker1)), new ExpectedEvent(TCtestScenario3.RequestUpdatesEventInfo, null, new RequestUpdatesEventDelegate1(this.TCtestScenario3S8RequestUpdatesEventChecker2)), new ExpectedEvent(TCtestScenario3.RequestUpdatesEventInfo, null, new RequestUpdatesEventDelegate1(this.TCtestScenario3S8RequestUpdatesEventChecker3)));
                if ((temp134 == 0)) {
                    this.Manager.Comment("reaching state \'S143\'");
                    FRS2Model.error_status_t temp130;
                    this.Manager.Comment("executing step \'call InitializeFileTransferAsync(1,1,True)\'");
                    temp130 = this.IFRS2ManagedAdapterInstance.InitializeFileTransferAsync(1, 1, true);
                    this.Manager.Checkpoint("MS-FRS2_R774");
                    this.Manager.Checkpoint("MS-FRS2_R777");
                    this.Manager.Checkpoint("MS-FRS2_R793");
                    this.Manager.Comment("reaching state \'S168\'");
                    this.Manager.Comment("checking step \'return InitializeFileTransferAsync/ERROR_SUCCESS\'");
                    this.Manager.Assert((temp130 == FRS2Model.error_status_t.ERROR_SUCCESS), String.Format("expected \'FRS2Model.error_status_t.ERROR_SUCCESS\', actual \'{0}\' (return of Initia" +
                                "lizeFileTransferAsync, state S168)", TestManagerHelpers.Describe(temp130)));
                    this.Manager.Comment("reaching state \'S186\'");
                    int temp131 = this.Manager.ExpectEvent(this.QuiescenceTimeout, true, new ExpectedEvent(TCtestScenario3.InitializeFileTransferAsyncEventInfo, null, new InitializeFileTransferAsyncEventDelegate1(this.TCtestScenario3S8InitializeFileTransferAsyncEventChecker)), new ExpectedEvent(TCtestScenario3.InitializeFileTransferAsyncEventInfo, null, new InitializeFileTransferAsyncEventDelegate1(this.TCtestScenario3S8InitializeFileTransferAsyncEventChecker1)), new ExpectedEvent(TCtestScenario3.InitializeFileTransferAsyncEventInfo, null, new InitializeFileTransferAsyncEventDelegate1(this.TCtestScenario3S8InitializeFileTransferAsyncEventChecker2)), new ExpectedEvent(TCtestScenario3.InitializeFileTransferAsyncEventInfo, null, new InitializeFileTransferAsyncEventDelegate1(this.TCtestScenario3S8InitializeFileTransferAsyncEventChecker3)), new ExpectedEvent(TCtestScenario3.InitializeFileTransferAsyncEventInfo, null, new InitializeFileTransferAsyncEventDelegate1(this.TCtestScenario3S8InitializeFileTransferAsyncEventChecker4)), new ExpectedEvent(TCtestScenario3.InitializeFileTransferAsyncEventInfo, null, new InitializeFileTransferAsyncEventDelegate1(this.TCtestScenario3S8InitializeFileTransferAsyncEventChecker5)), new ExpectedEvent(TCtestScenario3.InitializeFileTransferAsyncEventInfo, null, new InitializeFileTransferAsyncEventDelegate1(this.TCtestScenario3S8InitializeFileTransferAsyncEventChecker6)), new ExpectedEvent(TCtestScenario3.InitializeFileTransferAsyncEventInfo, null, new InitializeFileTransferAsyncEventDelegate1(this.TCtestScenario3S8InitializeFileTransferAsyncEventChecker7)));
                    if ((temp131 == 0)) {
                        TCtestScenario3S227();
                        goto label20;
                    }
                    if ((temp131 == 1)) {
                        TCtestScenario3S264();
                        goto label20;
                    }
                    if ((temp131 == 2)) {
                        TCtestScenario3S199();
                        goto label20;
                    }
                    if ((temp131 == 3)) {
                        TCtestScenario3S197();
                        goto label20;
                    }
                    if ((temp131 == 4)) {
                        TCtestScenario3S228();
                        goto label20;
                    }
                    if ((temp131 == 5)) {
                        TCtestScenario3S194();
                        goto label20;
                    }
                    if ((temp131 == 6)) {
                        TCtestScenario3S230();
                        goto label20;
                    }
                    if ((temp131 == 7)) {
                        TCtestScenario3S192();
                        goto label20;
                    }
                    throw new InvalidOperationException("never reached");
                label20:
;
                    goto label22;
                }
                if ((temp134 == 1)) {
                    this.Manager.Comment("reaching state \'S144\'");
                    FRS2Model.error_status_t temp132;
                    this.Manager.Comment("executing step \'call InitializeFileTransferAsync(1,1,True)\'");
                    temp132 = this.IFRS2ManagedAdapterInstance.InitializeFileTransferAsync(1, 1, true);
                    this.Manager.Checkpoint("MS-FRS2_R774");
                    this.Manager.Checkpoint("MS-FRS2_R777");
                    this.Manager.Checkpoint("MS-FRS2_R793");
                    this.Manager.Comment("reaching state \'S169\'");
                    this.Manager.Comment("checking step \'return InitializeFileTransferAsync/ERROR_SUCCESS\'");
                    this.Manager.Assert((temp132 == FRS2Model.error_status_t.ERROR_SUCCESS), String.Format("expected \'FRS2Model.error_status_t.ERROR_SUCCESS\', actual \'{0}\' (return of Initia" +
                                "lizeFileTransferAsync, state S169)", TestManagerHelpers.Describe(temp132)));
                    this.Manager.Comment("reaching state \'S187\'");
                    int temp133 = this.Manager.ExpectEvent(this.QuiescenceTimeout, true, new ExpectedEvent(TCtestScenario3.InitializeFileTransferAsyncEventInfo, null, new InitializeFileTransferAsyncEventDelegate1(this.TCtestScenario3S8InitializeFileTransferAsyncEventChecker8)), new ExpectedEvent(TCtestScenario3.InitializeFileTransferAsyncEventInfo, null, new InitializeFileTransferAsyncEventDelegate1(this.TCtestScenario3S8InitializeFileTransferAsyncEventChecker9)), new ExpectedEvent(TCtestScenario3.InitializeFileTransferAsyncEventInfo, null, new InitializeFileTransferAsyncEventDelegate1(this.TCtestScenario3S8InitializeFileTransferAsyncEventChecker10)), new ExpectedEvent(TCtestScenario3.InitializeFileTransferAsyncEventInfo, null, new InitializeFileTransferAsyncEventDelegate1(this.TCtestScenario3S8InitializeFileTransferAsyncEventChecker11)), new ExpectedEvent(TCtestScenario3.InitializeFileTransferAsyncEventInfo, null, new InitializeFileTransferAsyncEventDelegate1(this.TCtestScenario3S8InitializeFileTransferAsyncEventChecker12)), new ExpectedEvent(TCtestScenario3.InitializeFileTransferAsyncEventInfo, null, new InitializeFileTransferAsyncEventDelegate1(this.TCtestScenario3S8InitializeFileTransferAsyncEventChecker13)), new ExpectedEvent(TCtestScenario3.InitializeFileTransferAsyncEventInfo, null, new InitializeFileTransferAsyncEventDelegate1(this.TCtestScenario3S8InitializeFileTransferAsyncEventChecker14)), new ExpectedEvent(TCtestScenario3.InitializeFileTransferAsyncEventInfo, null, new InitializeFileTransferAsyncEventDelegate1(this.TCtestScenario3S8InitializeFileTransferAsyncEventChecker15)));
                    if ((temp133 == 0)) {
                        TCtestScenario3S235();
                        goto label21;
                    }
                    if ((temp133 == 1)) {
                        TCtestScenario3S207();
                        goto label21;
                    }
                    if ((temp133 == 2)) {
                        TCtestScenario3S206();
                        goto label21;
                    }
                    if ((temp133 == 3)) {
                        TCtestScenario3S272();
                        goto label21;
                    }
                    if ((temp133 == 4)) {
                        TCtestScenario3S203();
                        goto label21;
                    }
                    if ((temp133 == 5)) {
                        TCtestScenario3S202();
                        goto label21;
                    }
                    if ((temp133 == 6)) {
                        TCtestScenario3S238();
                        goto label21;
                    }
                    if ((temp133 == 7)) {
                        TCtestScenario3S239();
                        goto label21;
                    }
                    throw new InvalidOperationException("never reached");
                label21:
;
                    goto label22;
                }
                if ((temp134 == 2)) {
                    TCtestScenario3S141();
                    goto label22;
                }
                if ((temp134 == 3)) {
                    TCtestScenario3S127();
                    goto label22;
                }
                throw new InvalidOperationException("never reached");
            label22:
;
                goto label23;
            }
            if ((temp135 == 1)) {
                TCtestScenario3S96();
                goto label23;
            }
            throw new InvalidOperationException("never reached");
        label23:
;
            this.Manager.EndTest();
        }
        
        private void TCtestScenario3S8AsyncPollResponseEventChecker(FRS2Model.VVGeneration vvGen) {
            this.Manager.Comment("checking step \'event AsyncPollResponseEvent(ValidValue)\'");
            try {
                this.Manager.Assert((vvGen == FRS2Model.VVGeneration.ValidValue), String.Format("expected \'FRS2Model.VVGeneration.ValidValue\', actual \'{0}\' (vvGen of AsyncPollRes" +
                            "ponseEvent, state S91)", TestManagerHelpers.Describe(vvGen)));
            }
            catch (TransactionFailedException ) {
                this.Manager.Comment("This step would have covered MS-FRS2_R556, MS-FRS2_R1020, MS-FRS2_R555");
                throw;
            }
            this.Manager.Checkpoint("MS-FRS2_R556");
            this.Manager.Checkpoint("MS-FRS2_R1020");
            this.Manager.Checkpoint("MS-FRS2_R555");
        }
        
        private void TCtestScenario3S8RequestUpdatesEventChecker(FRS2Model.FilePresense present, FRS2Model.UPDATE_STATUS updateStatus) {
            this.Manager.Comment("checking step \'event RequestUpdatesEvent(fileDeleted,UPDATE_STATUS_DONE)\'");
            try {
                this.Manager.Assert((present == FRS2Model.FilePresense.fileDeleted), String.Format("expected \'FRS2Model.FilePresense.fileDeleted\', actual \'{0}\' (present of RequestUp" +
                            "datesEvent, state S123)", TestManagerHelpers.Describe(present)));
                this.Manager.Assert((updateStatus == FRS2Model.UPDATE_STATUS.UPDATE_STATUS_DONE), String.Format("expected \'FRS2Model.UPDATE_STATUS.UPDATE_STATUS_DONE\', actual \'{0}\' (updateStatus" +
                            " of RequestUpdatesEvent, state S123)", TestManagerHelpers.Describe(updateStatus)));
            }
            catch (TransactionFailedException ) {
                this.Manager.Comment("This step would have covered MS-FRS2_R93, MS-FRS2_R94");
                throw;
            }
            this.Manager.Checkpoint("MS-FRS2_R93");
            this.Manager.Checkpoint("MS-FRS2_R94");
        }
        
        private void TCtestScenario3S8InitializeFileTransferAsyncEventChecker(FRS2Model.ServerContext context, FRS2Model.RDC_Sig_Level rdcsigLevel, bool isEOF) {
            this.Manager.Comment("checking step \'event InitializeFileTransferAsyncEvent(InvalidContext,forRAWFileLe" +
                    "vel,True)\'");
            this.Manager.Assert((context == FRS2Model.ServerContext.InvalidContext), String.Format("expected \'FRS2Model.ServerContext.InvalidContext\', actual \'{0}\' (context of Initi" +
                        "alizeFileTransferAsyncEvent, state S186)", TestManagerHelpers.Describe(context)));
            this.Manager.Assert((rdcsigLevel == FRS2Model.RDC_Sig_Level.forRAWFileLevel), String.Format("expected \'FRS2Model.RDC_Sig_Level.forRAWFileLevel\', actual \'{0}\' (rdcsigLevel of " +
                        "InitializeFileTransferAsyncEvent, state S186)", TestManagerHelpers.Describe(rdcsigLevel)));
            this.Manager.Assert((isEOF == true), String.Format("expected \'true\', actual \'{0}\' (isEOF of InitializeFileTransferAsyncEvent, state S" +
                        "186)", TestManagerHelpers.Describe(isEOF)));
        }
        
        private void TCtestScenario3S8InitializeFileTransferAsyncEventChecker1(FRS2Model.ServerContext context, FRS2Model.RDC_Sig_Level rdcsigLevel, bool isEOF) {
            this.Manager.Comment("checking step \'event InitializeFileTransferAsyncEvent(ValidContext,forRDCFileLeve" +
                    "l,False)\'");
            this.Manager.Assert((context == FRS2Model.ServerContext.ValidContext), String.Format("expected \'FRS2Model.ServerContext.ValidContext\', actual \'{0}\' (context of Initial" +
                        "izeFileTransferAsyncEvent, state S186)", TestManagerHelpers.Describe(context)));
            this.Manager.Assert((rdcsigLevel == FRS2Model.RDC_Sig_Level.forRDCFileLevel), String.Format("expected \'FRS2Model.RDC_Sig_Level.forRDCFileLevel\', actual \'{0}\' (rdcsigLevel of " +
                        "InitializeFileTransferAsyncEvent, state S186)", TestManagerHelpers.Describe(rdcsigLevel)));
            this.Manager.Assert((isEOF == false), String.Format("expected \'false\', actual \'{0}\' (isEOF of InitializeFileTransferAsyncEvent, state " +
                        "S186)", TestManagerHelpers.Describe(isEOF)));
        }
        
        private void TCtestScenario3S8InitializeFileTransferAsyncEventChecker2(FRS2Model.ServerContext context, FRS2Model.RDC_Sig_Level rdcsigLevel, bool isEOF) {
            this.Manager.Comment("checking step \'event InitializeFileTransferAsyncEvent(InvalidContext,forRDCFileLe" +
                    "vel,False)\'");
            this.Manager.Assert((context == FRS2Model.ServerContext.InvalidContext), String.Format("expected \'FRS2Model.ServerContext.InvalidContext\', actual \'{0}\' (context of Initi" +
                        "alizeFileTransferAsyncEvent, state S186)", TestManagerHelpers.Describe(context)));
            this.Manager.Assert((rdcsigLevel == FRS2Model.RDC_Sig_Level.forRDCFileLevel), String.Format("expected \'FRS2Model.RDC_Sig_Level.forRDCFileLevel\', actual \'{0}\' (rdcsigLevel of " +
                        "InitializeFileTransferAsyncEvent, state S186)", TestManagerHelpers.Describe(rdcsigLevel)));
            this.Manager.Assert((isEOF == false), String.Format("expected \'false\', actual \'{0}\' (isEOF of InitializeFileTransferAsyncEvent, state " +
                        "S186)", TestManagerHelpers.Describe(isEOF)));
        }
        
        private void TCtestScenario3S8InitializeFileTransferAsyncEventChecker3(FRS2Model.ServerContext context, FRS2Model.RDC_Sig_Level rdcsigLevel, bool isEOF) {
            this.Manager.Comment("checking step \'event InitializeFileTransferAsyncEvent(ValidContext,forRAWFileLeve" +
                    "l,False)\'");
            this.Manager.Assert((context == FRS2Model.ServerContext.ValidContext), String.Format("expected \'FRS2Model.ServerContext.ValidContext\', actual \'{0}\' (context of Initial" +
                        "izeFileTransferAsyncEvent, state S186)", TestManagerHelpers.Describe(context)));
            this.Manager.Assert((rdcsigLevel == FRS2Model.RDC_Sig_Level.forRAWFileLevel), String.Format("expected \'FRS2Model.RDC_Sig_Level.forRAWFileLevel\', actual \'{0}\' (rdcsigLevel of " +
                        "InitializeFileTransferAsyncEvent, state S186)", TestManagerHelpers.Describe(rdcsigLevel)));
            this.Manager.Assert((isEOF == false), String.Format("expected \'false\', actual \'{0}\' (isEOF of InitializeFileTransferAsyncEvent, state " +
                        "S186)", TestManagerHelpers.Describe(isEOF)));
        }
        
        private void TCtestScenario3S8InitializeFileTransferAsyncEventChecker4(FRS2Model.ServerContext context, FRS2Model.RDC_Sig_Level rdcsigLevel, bool isEOF) {
            this.Manager.Comment("checking step \'event InitializeFileTransferAsyncEvent(ValidContext,forRAWFileLeve" +
                    "l,True)\'");
            this.Manager.Assert((context == FRS2Model.ServerContext.ValidContext), String.Format("expected \'FRS2Model.ServerContext.ValidContext\', actual \'{0}\' (context of Initial" +
                        "izeFileTransferAsyncEvent, state S186)", TestManagerHelpers.Describe(context)));
            this.Manager.Assert((rdcsigLevel == FRS2Model.RDC_Sig_Level.forRAWFileLevel), String.Format("expected \'FRS2Model.RDC_Sig_Level.forRAWFileLevel\', actual \'{0}\' (rdcsigLevel of " +
                        "InitializeFileTransferAsyncEvent, state S186)", TestManagerHelpers.Describe(rdcsigLevel)));
            this.Manager.Assert((isEOF == true), String.Format("expected \'true\', actual \'{0}\' (isEOF of InitializeFileTransferAsyncEvent, state S" +
                        "186)", TestManagerHelpers.Describe(isEOF)));
        }
        
        private void TCtestScenario3S8InitializeFileTransferAsyncEventChecker5(FRS2Model.ServerContext context, FRS2Model.RDC_Sig_Level rdcsigLevel, bool isEOF) {
            this.Manager.Comment("checking step \'event InitializeFileTransferAsyncEvent(InvalidContext,forRAWFileLe" +
                    "vel,False)\'");
            this.Manager.Assert((context == FRS2Model.ServerContext.InvalidContext), String.Format("expected \'FRS2Model.ServerContext.InvalidContext\', actual \'{0}\' (context of Initi" +
                        "alizeFileTransferAsyncEvent, state S186)", TestManagerHelpers.Describe(context)));
            this.Manager.Assert((rdcsigLevel == FRS2Model.RDC_Sig_Level.forRAWFileLevel), String.Format("expected \'FRS2Model.RDC_Sig_Level.forRAWFileLevel\', actual \'{0}\' (rdcsigLevel of " +
                        "InitializeFileTransferAsyncEvent, state S186)", TestManagerHelpers.Describe(rdcsigLevel)));
            this.Manager.Assert((isEOF == false), String.Format("expected \'false\', actual \'{0}\' (isEOF of InitializeFileTransferAsyncEvent, state " +
                        "S186)", TestManagerHelpers.Describe(isEOF)));
        }
        
        private void TCtestScenario3S8InitializeFileTransferAsyncEventChecker6(FRS2Model.ServerContext context, FRS2Model.RDC_Sig_Level rdcsigLevel, bool isEOF) {
            this.Manager.Comment("checking step \'event InitializeFileTransferAsyncEvent(InvalidContext,forRDCFileLe" +
                    "vel,True)\'");
            this.Manager.Assert((context == FRS2Model.ServerContext.InvalidContext), String.Format("expected \'FRS2Model.ServerContext.InvalidContext\', actual \'{0}\' (context of Initi" +
                        "alizeFileTransferAsyncEvent, state S186)", TestManagerHelpers.Describe(context)));
            this.Manager.Assert((rdcsigLevel == FRS2Model.RDC_Sig_Level.forRDCFileLevel), String.Format("expected \'FRS2Model.RDC_Sig_Level.forRDCFileLevel\', actual \'{0}\' (rdcsigLevel of " +
                        "InitializeFileTransferAsyncEvent, state S186)", TestManagerHelpers.Describe(rdcsigLevel)));
            this.Manager.Assert((isEOF == true), String.Format("expected \'true\', actual \'{0}\' (isEOF of InitializeFileTransferAsyncEvent, state S" +
                        "186)", TestManagerHelpers.Describe(isEOF)));
        }
        
        private void TCtestScenario3S8InitializeFileTransferAsyncEventChecker7(FRS2Model.ServerContext context, FRS2Model.RDC_Sig_Level rdcsigLevel, bool isEOF) {
            this.Manager.Comment("checking step \'event InitializeFileTransferAsyncEvent(ValidContext,forRDCFileLeve" +
                    "l,True)\'");
            this.Manager.Assert((context == FRS2Model.ServerContext.ValidContext), String.Format("expected \'FRS2Model.ServerContext.ValidContext\', actual \'{0}\' (context of Initial" +
                        "izeFileTransferAsyncEvent, state S186)", TestManagerHelpers.Describe(context)));
            this.Manager.Assert((rdcsigLevel == FRS2Model.RDC_Sig_Level.forRDCFileLevel), String.Format("expected \'FRS2Model.RDC_Sig_Level.forRDCFileLevel\', actual \'{0}\' (rdcsigLevel of " +
                        "InitializeFileTransferAsyncEvent, state S186)", TestManagerHelpers.Describe(rdcsigLevel)));
            this.Manager.Assert((isEOF == true), String.Format("expected \'true\', actual \'{0}\' (isEOF of InitializeFileTransferAsyncEvent, state S" +
                        "186)", TestManagerHelpers.Describe(isEOF)));
        }
        
        private void TCtestScenario3S8RequestUpdatesEventChecker1(FRS2Model.FilePresense present, FRS2Model.UPDATE_STATUS updateStatus) {
            this.Manager.Comment("checking step \'event RequestUpdatesEvent(fileExists,UPDATE_STATUS_DONE)\'");
            try {
                this.Manager.Assert((present == FRS2Model.FilePresense.fileExists), String.Format("expected \'FRS2Model.FilePresense.fileExists\', actual \'{0}\' (present of RequestUpd" +
                            "atesEvent, state S123)", TestManagerHelpers.Describe(present)));
                this.Manager.Assert((updateStatus == FRS2Model.UPDATE_STATUS.UPDATE_STATUS_DONE), String.Format("expected \'FRS2Model.UPDATE_STATUS.UPDATE_STATUS_DONE\', actual \'{0}\' (updateStatus" +
                            " of RequestUpdatesEvent, state S123)", TestManagerHelpers.Describe(updateStatus)));
            }
            catch (TransactionFailedException ) {
                this.Manager.Comment("This step would have covered MS-FRS2_R93, MS-FRS2_R94");
                throw;
            }
            this.Manager.Checkpoint("MS-FRS2_R93");
            this.Manager.Checkpoint("MS-FRS2_R94");
        }
        
        private void TCtestScenario3S8InitializeFileTransferAsyncEventChecker8(FRS2Model.ServerContext context, FRS2Model.RDC_Sig_Level rdcsigLevel, bool isEOF) {
            this.Manager.Comment("checking step \'event InitializeFileTransferAsyncEvent(InvalidContext,forRDCFileLe" +
                    "vel,True)\'");
            this.Manager.Assert((context == FRS2Model.ServerContext.InvalidContext), String.Format("expected \'FRS2Model.ServerContext.InvalidContext\', actual \'{0}\' (context of Initi" +
                        "alizeFileTransferAsyncEvent, state S187)", TestManagerHelpers.Describe(context)));
            this.Manager.Assert((rdcsigLevel == FRS2Model.RDC_Sig_Level.forRDCFileLevel), String.Format("expected \'FRS2Model.RDC_Sig_Level.forRDCFileLevel\', actual \'{0}\' (rdcsigLevel of " +
                        "InitializeFileTransferAsyncEvent, state S187)", TestManagerHelpers.Describe(rdcsigLevel)));
            this.Manager.Assert((isEOF == true), String.Format("expected \'true\', actual \'{0}\' (isEOF of InitializeFileTransferAsyncEvent, state S" +
                        "187)", TestManagerHelpers.Describe(isEOF)));
        }
        
        private void TCtestScenario3S8InitializeFileTransferAsyncEventChecker9(FRS2Model.ServerContext context, FRS2Model.RDC_Sig_Level rdcsigLevel, bool isEOF) {
            this.Manager.Comment("checking step \'event InitializeFileTransferAsyncEvent(ValidContext,forRAWFileLeve" +
                    "l,False)\'");
            this.Manager.Assert((context == FRS2Model.ServerContext.ValidContext), String.Format("expected \'FRS2Model.ServerContext.ValidContext\', actual \'{0}\' (context of Initial" +
                        "izeFileTransferAsyncEvent, state S187)", TestManagerHelpers.Describe(context)));
            this.Manager.Assert((rdcsigLevel == FRS2Model.RDC_Sig_Level.forRAWFileLevel), String.Format("expected \'FRS2Model.RDC_Sig_Level.forRAWFileLevel\', actual \'{0}\' (rdcsigLevel of " +
                        "InitializeFileTransferAsyncEvent, state S187)", TestManagerHelpers.Describe(rdcsigLevel)));
            this.Manager.Assert((isEOF == false), String.Format("expected \'false\', actual \'{0}\' (isEOF of InitializeFileTransferAsyncEvent, state " +
                        "S187)", TestManagerHelpers.Describe(isEOF)));
        }
        
        private void TCtestScenario3S8InitializeFileTransferAsyncEventChecker10(FRS2Model.ServerContext context, FRS2Model.RDC_Sig_Level rdcsigLevel, bool isEOF) {
            this.Manager.Comment("checking step \'event InitializeFileTransferAsyncEvent(InvalidContext,forRAWFileLe" +
                    "vel,False)\'");
            this.Manager.Assert((context == FRS2Model.ServerContext.InvalidContext), String.Format("expected \'FRS2Model.ServerContext.InvalidContext\', actual \'{0}\' (context of Initi" +
                        "alizeFileTransferAsyncEvent, state S187)", TestManagerHelpers.Describe(context)));
            this.Manager.Assert((rdcsigLevel == FRS2Model.RDC_Sig_Level.forRAWFileLevel), String.Format("expected \'FRS2Model.RDC_Sig_Level.forRAWFileLevel\', actual \'{0}\' (rdcsigLevel of " +
                        "InitializeFileTransferAsyncEvent, state S187)", TestManagerHelpers.Describe(rdcsigLevel)));
            this.Manager.Assert((isEOF == false), String.Format("expected \'false\', actual \'{0}\' (isEOF of InitializeFileTransferAsyncEvent, state " +
                        "S187)", TestManagerHelpers.Describe(isEOF)));
        }
        
        private void TCtestScenario3S8InitializeFileTransferAsyncEventChecker11(FRS2Model.ServerContext context, FRS2Model.RDC_Sig_Level rdcsigLevel, bool isEOF) {
            this.Manager.Comment("checking step \'event InitializeFileTransferAsyncEvent(ValidContext,forRDCFileLeve" +
                    "l,False)\'");
            this.Manager.Assert((context == FRS2Model.ServerContext.ValidContext), String.Format("expected \'FRS2Model.ServerContext.ValidContext\', actual \'{0}\' (context of Initial" +
                        "izeFileTransferAsyncEvent, state S187)", TestManagerHelpers.Describe(context)));
            this.Manager.Assert((rdcsigLevel == FRS2Model.RDC_Sig_Level.forRDCFileLevel), String.Format("expected \'FRS2Model.RDC_Sig_Level.forRDCFileLevel\', actual \'{0}\' (rdcsigLevel of " +
                        "InitializeFileTransferAsyncEvent, state S187)", TestManagerHelpers.Describe(rdcsigLevel)));
            this.Manager.Assert((isEOF == false), String.Format("expected \'false\', actual \'{0}\' (isEOF of InitializeFileTransferAsyncEvent, state " +
                        "S187)", TestManagerHelpers.Describe(isEOF)));
        }
        
        private void TCtestScenario3S8InitializeFileTransferAsyncEventChecker12(FRS2Model.ServerContext context, FRS2Model.RDC_Sig_Level rdcsigLevel, bool isEOF) {
            this.Manager.Comment("checking step \'event InitializeFileTransferAsyncEvent(ValidContext,forRDCFileLeve" +
                    "l,True)\'");
            this.Manager.Assert((context == FRS2Model.ServerContext.ValidContext), String.Format("expected \'FRS2Model.ServerContext.ValidContext\', actual \'{0}\' (context of Initial" +
                        "izeFileTransferAsyncEvent, state S187)", TestManagerHelpers.Describe(context)));
            this.Manager.Assert((rdcsigLevel == FRS2Model.RDC_Sig_Level.forRDCFileLevel), String.Format("expected \'FRS2Model.RDC_Sig_Level.forRDCFileLevel\', actual \'{0}\' (rdcsigLevel of " +
                        "InitializeFileTransferAsyncEvent, state S187)", TestManagerHelpers.Describe(rdcsigLevel)));
            this.Manager.Assert((isEOF == true), String.Format("expected \'true\', actual \'{0}\' (isEOF of InitializeFileTransferAsyncEvent, state S" +
                        "187)", TestManagerHelpers.Describe(isEOF)));
        }
        
        private void TCtestScenario3S8InitializeFileTransferAsyncEventChecker13(FRS2Model.ServerContext context, FRS2Model.RDC_Sig_Level rdcsigLevel, bool isEOF) {
            this.Manager.Comment("checking step \'event InitializeFileTransferAsyncEvent(InvalidContext,forRDCFileLe" +
                    "vel,False)\'");
            this.Manager.Assert((context == FRS2Model.ServerContext.InvalidContext), String.Format("expected \'FRS2Model.ServerContext.InvalidContext\', actual \'{0}\' (context of Initi" +
                        "alizeFileTransferAsyncEvent, state S187)", TestManagerHelpers.Describe(context)));
            this.Manager.Assert((rdcsigLevel == FRS2Model.RDC_Sig_Level.forRDCFileLevel), String.Format("expected \'FRS2Model.RDC_Sig_Level.forRDCFileLevel\', actual \'{0}\' (rdcsigLevel of " +
                        "InitializeFileTransferAsyncEvent, state S187)", TestManagerHelpers.Describe(rdcsigLevel)));
            this.Manager.Assert((isEOF == false), String.Format("expected \'false\', actual \'{0}\' (isEOF of InitializeFileTransferAsyncEvent, state " +
                        "S187)", TestManagerHelpers.Describe(isEOF)));
        }
        
        private void TCtestScenario3S8InitializeFileTransferAsyncEventChecker14(FRS2Model.ServerContext context, FRS2Model.RDC_Sig_Level rdcsigLevel, bool isEOF) {
            this.Manager.Comment("checking step \'event InitializeFileTransferAsyncEvent(InvalidContext,forRAWFileLe" +
                    "vel,True)\'");
            this.Manager.Assert((context == FRS2Model.ServerContext.InvalidContext), String.Format("expected \'FRS2Model.ServerContext.InvalidContext\', actual \'{0}\' (context of Initi" +
                        "alizeFileTransferAsyncEvent, state S187)", TestManagerHelpers.Describe(context)));
            this.Manager.Assert((rdcsigLevel == FRS2Model.RDC_Sig_Level.forRAWFileLevel), String.Format("expected \'FRS2Model.RDC_Sig_Level.forRAWFileLevel\', actual \'{0}\' (rdcsigLevel of " +
                        "InitializeFileTransferAsyncEvent, state S187)", TestManagerHelpers.Describe(rdcsigLevel)));
            this.Manager.Assert((isEOF == true), String.Format("expected \'true\', actual \'{0}\' (isEOF of InitializeFileTransferAsyncEvent, state S" +
                        "187)", TestManagerHelpers.Describe(isEOF)));
        }
        
        private void TCtestScenario3S8InitializeFileTransferAsyncEventChecker15(FRS2Model.ServerContext context, FRS2Model.RDC_Sig_Level rdcsigLevel, bool isEOF) {
            this.Manager.Comment("checking step \'event InitializeFileTransferAsyncEvent(ValidContext,forRAWFileLeve" +
                    "l,True)\'");
            this.Manager.Assert((context == FRS2Model.ServerContext.ValidContext), String.Format("expected \'FRS2Model.ServerContext.ValidContext\', actual \'{0}\' (context of Initial" +
                        "izeFileTransferAsyncEvent, state S187)", TestManagerHelpers.Describe(context)));
            this.Manager.Assert((rdcsigLevel == FRS2Model.RDC_Sig_Level.forRAWFileLevel), String.Format("expected \'FRS2Model.RDC_Sig_Level.forRAWFileLevel\', actual \'{0}\' (rdcsigLevel of " +
                        "InitializeFileTransferAsyncEvent, state S187)", TestManagerHelpers.Describe(rdcsigLevel)));
            this.Manager.Assert((isEOF == true), String.Format("expected \'true\', actual \'{0}\' (isEOF of InitializeFileTransferAsyncEvent, state S" +
                        "187)", TestManagerHelpers.Describe(isEOF)));
        }
        
        private void TCtestScenario3S8RequestUpdatesEventChecker2(FRS2Model.FilePresense present, FRS2Model.UPDATE_STATUS updateStatus) {
            this.Manager.Comment("checking step \'event RequestUpdatesEvent(fileExists,UPDATE_STATUS_MORE)\'");
            try {
                this.Manager.Assert((present == FRS2Model.FilePresense.fileExists), String.Format("expected \'FRS2Model.FilePresense.fileExists\', actual \'{0}\' (present of RequestUpd" +
                            "atesEvent, state S123)", TestManagerHelpers.Describe(present)));
                this.Manager.Assert((updateStatus == FRS2Model.UPDATE_STATUS.UPDATE_STATUS_MORE), String.Format("expected \'FRS2Model.UPDATE_STATUS.UPDATE_STATUS_MORE\', actual \'{0}\' (updateStatus" +
                            " of RequestUpdatesEvent, state S123)", TestManagerHelpers.Describe(updateStatus)));
            }
            catch (TransactionFailedException ) {
                this.Manager.Comment("This step would have covered MS-FRS2_R93, MS-FRS2_R498");
                throw;
            }
            this.Manager.Checkpoint("MS-FRS2_R93");
            this.Manager.Checkpoint("MS-FRS2_R498");
        }
        
        private void TCtestScenario3S8RequestUpdatesEventChecker3(FRS2Model.FilePresense present, FRS2Model.UPDATE_STATUS updateStatus) {
            this.Manager.Comment("checking step \'event RequestUpdatesEvent(fileDeleted,UPDATE_STATUS_MORE)\'");
            try {
                this.Manager.Assert((present == FRS2Model.FilePresense.fileDeleted), String.Format("expected \'FRS2Model.FilePresense.fileDeleted\', actual \'{0}\' (present of RequestUp" +
                            "datesEvent, state S123)", TestManagerHelpers.Describe(present)));
                this.Manager.Assert((updateStatus == FRS2Model.UPDATE_STATUS.UPDATE_STATUS_MORE), String.Format("expected \'FRS2Model.UPDATE_STATUS.UPDATE_STATUS_MORE\', actual \'{0}\' (updateStatus" +
                            " of RequestUpdatesEvent, state S123)", TestManagerHelpers.Describe(updateStatus)));
            }
            catch (TransactionFailedException ) {
                this.Manager.Comment("This step would have covered MS-FRS2_R93");
                throw;
            }
            this.Manager.Checkpoint("MS-FRS2_R93");
        }
        
        private void TCtestScenario3S8AsyncPollResponseEventChecker1(FRS2Model.VVGeneration vvGen) {
            this.Manager.Comment("checking step \'event AsyncPollResponseEvent(InvalidValue)\'");
            try {
                this.Manager.Assert((vvGen == FRS2Model.VVGeneration.InvalidValue), String.Format("expected \'FRS2Model.VVGeneration.InvalidValue\', actual \'{0}\' (vvGen of AsyncPollR" +
                            "esponseEvent, state S91)", TestManagerHelpers.Describe(vvGen)));
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
        
        #region Test Starting in S10
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        [Microsoft.VisualStudio.TestTools.UnitTesting.DescriptionAttribute(@"MS-FRS2_R37, MS-FRS2_R436, MS-FRS2_R439, MS-FRS2_R455, MS-FRS2_R458, MS-FRS2_R448, MS-FRS2_R549, MS-FRS2_R552, MS-FRS2_R517, MS-FRS2_R520, MS-FRS2_R466, MS-FRS2_R556, MS-FRS2_R1020, MS-FRS2_R555, MS-FRS2_R486, MS-FRS2_R489, MS-FRS2_R93, MS-FRS2_R94, MS-FRS2_R774, MS-FRS2_R777, MS-FRS2_R793, MS-FRS2_R93, MS-FRS2_R94, MS-FRS2_R774, MS-FRS2_R777, MS-FRS2_R793, MS-FRS2_R93, MS-FRS2_R498, MS-FRS2_R93, MS-FRS2_R556, MS-FRS2_R1020, MS-FRS2_R555")]
        public virtual void FRS2_TCtestScenario3S10() {
            this.Manager.BeginTest("TCtestScenario3S10");
            this.Manager.Comment("reaching state \'S10\'");
            FRS2Model.error_status_t temp136;
            this.Manager.Comment(@"executing step 'call Initialization(Windows7,Map{1=>enabled,2=>enabled,3=>enabled,4=>disabled},Map{1=>exists,2=>exists,3=>exists,4=>exists},Map{""A""=>1,""B""=>2,""C""=>3,""D""=>4},Map{""A""=>sysvol,""B""=>protection,""C""=>sysvol,""D""=>sysvol},Map{1=>""A"",2=>""B"",3=>""C"",4=>""D""},Map{1=>writeOnly,2=>readWrite,3=>readOnly,4=>readWrite},Map{1=>enabled,2=>disabled,3=>enabled,4=>enabled})'");
            temp136 = this.IFRS2ManagedAdapterInstance.Initialization(FRS2Model.OSVersion.Windows7, new Microsoft.Modeling.Map<System.Int32,FRS2Model.connectionProperty>().Add(1, FRS2Model.connectionProperty.enabled).Add(2, FRS2Model.connectionProperty.enabled).Add(3, FRS2Model.connectionProperty.enabled).Add(4, FRS2Model.connectionProperty.disabled), new Microsoft.Modeling.Map<System.Int32,FRS2Model.FromServer>().Add(1, FRS2Model.FromServer.exists).Add(2, FRS2Model.FromServer.exists).Add(3, FRS2Model.FromServer.exists).Add(4, FRS2Model.FromServer.exists), new Microsoft.Modeling.Map<System.String,System.Int32>().Add("A", 1).Add("B", 2).Add("C", 3).Add("D", 4), new Microsoft.Modeling.Map<System.String,FRS2Model.ReplicationGroupTypes>().Add("A", FRS2Model.ReplicationGroupTypes.sysvol).Add("B", FRS2Model.ReplicationGroupTypes.protection).Add("C", FRS2Model.ReplicationGroupTypes.sysvol).Add("D", FRS2Model.ReplicationGroupTypes.sysvol), new Microsoft.Modeling.Map<System.Int32,System.String>().Add(1, "A").Add(2, "B").Add(3, "C").Add(4, "D"), new Microsoft.Modeling.Map<System.Int32,FRS2Model.accessLevels>().Add(1, FRS2Model.accessLevels.writeOnly).Add(2, FRS2Model.accessLevels.readWrite).Add(3, FRS2Model.accessLevels.readOnly).Add(4, FRS2Model.accessLevels.readWrite), new Microsoft.Modeling.Map<System.Int32,FRS2Model.connectionProperty>().Add(1, FRS2Model.connectionProperty.enabled).Add(2, FRS2Model.connectionProperty.disabled).Add(3, FRS2Model.connectionProperty.enabled).Add(4, FRS2Model.connectionProperty.enabled));
            this.Manager.Comment("reaching state \'S11\'");
            this.Manager.Comment("checking step \'return Initialization/ERROR_SUCCESS\'");
            this.Manager.Assert((temp136 == FRS2Model.error_status_t.ERROR_SUCCESS), String.Format("expected \'FRS2Model.error_status_t.ERROR_SUCCESS\', actual \'{0}\' (return of Initia" +
                        "lization, state S11)", TestManagerHelpers.Describe(temp136)));
            this.Manager.Comment("reaching state \'S23\'");
            FRS2Model.ProtocolVersionReturned temp137;
            FRS2Model.UpstreamFlagValueReturned temp138;
            FRS2Model.error_status_t temp139;
            this.Manager.Comment("executing step \'call EstablishConnection(\"A\",1,FRS_COMMUNICATION_PROTOCOL_VERSION" +
                    "_LONGHORN_SERVER,out _,out _)\'");
            temp139 = this.IFRS2ManagedAdapterInstance.EstablishConnection("A", 1, FRS2Model.ProtocolVersion.FRS_COMMUNICATION_PROTOCOL_VERSION_LONGHORN_SERVER, out temp137, out temp138);
            this.Manager.Checkpoint("MS-FRS2_R37");
            this.Manager.Checkpoint("MS-FRS2_R436");
            this.Manager.Checkpoint("MS-FRS2_R439");
            this.Manager.Comment("reaching state \'S32\'");
            this.Manager.Comment("checking step \'return EstablishConnection/[out Valid,out Valid]:ERROR_SUCCESS\'");
            this.Manager.Assert((temp137 == FRS2Model.ProtocolVersionReturned.Valid), String.Format("expected \'FRS2Model.ProtocolVersionReturned.Valid\', actual \'{0}\' (upstreamProtoco" +
                        "lVersion of EstablishConnection, state S32)", TestManagerHelpers.Describe(temp137)));
            this.Manager.Assert((temp138 == FRS2Model.UpstreamFlagValueReturned.Valid), String.Format("expected \'FRS2Model.UpstreamFlagValueReturned.Valid\', actual \'{0}\' (upstreamFlags" +
                        " of EstablishConnection, state S32)", TestManagerHelpers.Describe(temp138)));
            this.Manager.Assert((temp139 == FRS2Model.error_status_t.ERROR_SUCCESS), String.Format("expected \'FRS2Model.error_status_t.ERROR_SUCCESS\', actual \'{0}\' (return of Establ" +
                        "ishConnection, state S32)", TestManagerHelpers.Describe(temp139)));
            this.Manager.Comment("reaching state \'S41\'");
            FRS2Model.error_status_t temp140;
            this.Manager.Comment("executing step \'call EstablishSession(1,1)\'");
            temp140 = this.IFRS2ManagedAdapterInstance.EstablishSession(1, 1);
            this.Manager.Checkpoint("MS-FRS2_R455");
            this.Manager.Checkpoint("MS-FRS2_R458");
            this.Manager.Checkpoint("MS-FRS2_R448");
            this.Manager.Comment("reaching state \'S50\'");
            this.Manager.Comment("checking step \'return EstablishSession/ERROR_SUCCESS\'");
            this.Manager.Assert((temp140 == FRS2Model.error_status_t.ERROR_SUCCESS), String.Format("expected \'FRS2Model.error_status_t.ERROR_SUCCESS\', actual \'{0}\' (return of Establ" +
                        "ishSession, state S50)", TestManagerHelpers.Describe(temp140)));
            this.Manager.Comment("reaching state \'S59\'");
            FRS2Model.error_status_t temp141;
            this.Manager.Comment("executing step \'call AsyncPoll(1)\'");
            temp141 = this.IFRS2ManagedAdapterInstance.AsyncPoll(1);
            this.Manager.Checkpoint("MS-FRS2_R549");
            this.Manager.Checkpoint("MS-FRS2_R552");
            this.Manager.Comment("reaching state \'S68\'");
            this.Manager.Comment("checking step \'return AsyncPoll/ERROR_SUCCESS\'");
            this.Manager.Assert((temp141 == FRS2Model.error_status_t.ERROR_SUCCESS), String.Format("expected \'FRS2Model.error_status_t.ERROR_SUCCESS\', actual \'{0}\' (return of AsyncP" +
                        "oll, state S68)", TestManagerHelpers.Describe(temp141)));
            this.Manager.Comment("reaching state \'S76\'");
            FRS2Model.error_status_t temp142;
            this.Manager.Comment("executing step \'call RequestVersionVector(1,1,1,REQUEST_NORMAL_SYNC,CHANGE_ALL,Va" +
                    "lidValue)\'");
            temp142 = this.IFRS2ManagedAdapterInstance.RequestVersionVector(1, 1, 1, FRS2Model.VERSION_REQUEST_TYPE.REQUEST_NORMAL_SYNC, FRS2Model.VERSION_CHANGE_TYPE.CHANGE_ALL, FRS2Model.VVGeneration.ValidValue);
            this.Manager.Checkpoint("MS-FRS2_R517");
            this.Manager.Checkpoint("MS-FRS2_R520");
            this.Manager.Checkpoint("MS-FRS2_R466");
            this.Manager.Comment("reaching state \'S84\'");
            this.Manager.Comment("checking step \'return RequestVersionVector/ERROR_SUCCESS\'");
            this.Manager.Assert((temp142 == FRS2Model.error_status_t.ERROR_SUCCESS), String.Format("expected \'FRS2Model.error_status_t.ERROR_SUCCESS\', actual \'{0}\' (return of Reques" +
                        "tVersionVector, state S84)", TestManagerHelpers.Describe(temp142)));
            this.Manager.Comment("reaching state \'S92\'");
            int temp149 = this.Manager.ExpectEvent(this.QuiescenceTimeout, true, new ExpectedEvent(TCtestScenario3.AsyncPollResponseEventInfo, null, new AsyncPollResponseEventDelegate1(this.TCtestScenario3S10AsyncPollResponseEventChecker)), new ExpectedEvent(TCtestScenario3.AsyncPollResponseEventInfo, null, new AsyncPollResponseEventDelegate1(this.TCtestScenario3S10AsyncPollResponseEventChecker1)));
            if ((temp149 == 0)) {
                this.Manager.Comment("reaching state \'S105\'");
                FRS2Model.error_status_t temp143;
                this.Manager.Comment("executing step \'call RequestUpdates(1,1,valid)\'");
                temp143 = this.IFRS2ManagedAdapterInstance.RequestUpdates(1, 1, FRS2Model.versionVectorDiff.valid);
                this.Manager.Checkpoint("MS-FRS2_R486");
                this.Manager.Checkpoint("MS-FRS2_R489");
                this.Manager.Comment("reaching state \'S116\'");
                this.Manager.Comment("checking step \'return RequestUpdates/ERROR_SUCCESS\'");
                this.Manager.Assert((temp143 == FRS2Model.error_status_t.ERROR_SUCCESS), String.Format("expected \'FRS2Model.error_status_t.ERROR_SUCCESS\', actual \'{0}\' (return of Reques" +
                            "tUpdates, state S116)", TestManagerHelpers.Describe(temp143)));
                this.Manager.Comment("reaching state \'S124\'");
                int temp148 = this.Manager.ExpectEvent(this.QuiescenceTimeout, true, new ExpectedEvent(TCtestScenario3.RequestUpdatesEventInfo, null, new RequestUpdatesEventDelegate1(this.TCtestScenario3S10RequestUpdatesEventChecker)), new ExpectedEvent(TCtestScenario3.RequestUpdatesEventInfo, null, new RequestUpdatesEventDelegate1(this.TCtestScenario3S10RequestUpdatesEventChecker1)), new ExpectedEvent(TCtestScenario3.RequestUpdatesEventInfo, null, new RequestUpdatesEventDelegate1(this.TCtestScenario3S10RequestUpdatesEventChecker2)), new ExpectedEvent(TCtestScenario3.RequestUpdatesEventInfo, null, new RequestUpdatesEventDelegate1(this.TCtestScenario3S10RequestUpdatesEventChecker3)));
                if ((temp148 == 0)) {
                    this.Manager.Comment("reaching state \'S147\'");
                    FRS2Model.error_status_t temp144;
                    this.Manager.Comment("executing step \'call InitializeFileTransferAsync(1,1,True)\'");
                    temp144 = this.IFRS2ManagedAdapterInstance.InitializeFileTransferAsync(1, 1, true);
                    this.Manager.Checkpoint("MS-FRS2_R774");
                    this.Manager.Checkpoint("MS-FRS2_R777");
                    this.Manager.Checkpoint("MS-FRS2_R793");
                    this.Manager.Comment("reaching state \'S170\'");
                    this.Manager.Comment("checking step \'return InitializeFileTransferAsync/ERROR_SUCCESS\'");
                    this.Manager.Assert((temp144 == FRS2Model.error_status_t.ERROR_SUCCESS), String.Format("expected \'FRS2Model.error_status_t.ERROR_SUCCESS\', actual \'{0}\' (return of Initia" +
                                "lizeFileTransferAsync, state S170)", TestManagerHelpers.Describe(temp144)));
                    this.Manager.Comment("reaching state \'S188\'");
                    int temp145 = this.Manager.ExpectEvent(this.QuiescenceTimeout, true, new ExpectedEvent(TCtestScenario3.InitializeFileTransferAsyncEventInfo, null, new InitializeFileTransferAsyncEventDelegate1(this.TCtestScenario3S10InitializeFileTransferAsyncEventChecker)), new ExpectedEvent(TCtestScenario3.InitializeFileTransferAsyncEventInfo, null, new InitializeFileTransferAsyncEventDelegate1(this.TCtestScenario3S10InitializeFileTransferAsyncEventChecker1)), new ExpectedEvent(TCtestScenario3.InitializeFileTransferAsyncEventInfo, null, new InitializeFileTransferAsyncEventDelegate1(this.TCtestScenario3S10InitializeFileTransferAsyncEventChecker2)), new ExpectedEvent(TCtestScenario3.InitializeFileTransferAsyncEventInfo, null, new InitializeFileTransferAsyncEventDelegate1(this.TCtestScenario3S10InitializeFileTransferAsyncEventChecker3)), new ExpectedEvent(TCtestScenario3.InitializeFileTransferAsyncEventInfo, null, new InitializeFileTransferAsyncEventDelegate1(this.TCtestScenario3S10InitializeFileTransferAsyncEventChecker4)), new ExpectedEvent(TCtestScenario3.InitializeFileTransferAsyncEventInfo, null, new InitializeFileTransferAsyncEventDelegate1(this.TCtestScenario3S10InitializeFileTransferAsyncEventChecker5)), new ExpectedEvent(TCtestScenario3.InitializeFileTransferAsyncEventInfo, null, new InitializeFileTransferAsyncEventDelegate1(this.TCtestScenario3S10InitializeFileTransferAsyncEventChecker6)), new ExpectedEvent(TCtestScenario3.InitializeFileTransferAsyncEventInfo, null, new InitializeFileTransferAsyncEventDelegate1(this.TCtestScenario3S10InitializeFileTransferAsyncEventChecker7)));
                    if ((temp145 == 0)) {
                        TCtestScenario3S228();
                        goto label24;
                    }
                    if ((temp145 == 1)) {
                        TCtestScenario3S264();
                        goto label24;
                    }
                    if ((temp145 == 2)) {
                        TCtestScenario3S199();
                        goto label24;
                    }
                    if ((temp145 == 3)) {
                        TCtestScenario3S197();
                        goto label24;
                    }
                    if ((temp145 == 4)) {
                        TCtestScenario3S227();
                        goto label24;
                    }
                    if ((temp145 == 5)) {
                        TCtestScenario3S194();
                        goto label24;
                    }
                    if ((temp145 == 6)) {
                        TCtestScenario3S230();
                        goto label24;
                    }
                    if ((temp145 == 7)) {
                        TCtestScenario3S192();
                        goto label24;
                    }
                    throw new InvalidOperationException("never reached");
                label24:
;
                    goto label26;
                }
                if ((temp148 == 1)) {
                    this.Manager.Comment("reaching state \'S148\'");
                    FRS2Model.error_status_t temp146;
                    this.Manager.Comment("executing step \'call InitializeFileTransferAsync(1,1,True)\'");
                    temp146 = this.IFRS2ManagedAdapterInstance.InitializeFileTransferAsync(1, 1, true);
                    this.Manager.Checkpoint("MS-FRS2_R774");
                    this.Manager.Checkpoint("MS-FRS2_R777");
                    this.Manager.Checkpoint("MS-FRS2_R793");
                    this.Manager.Comment("reaching state \'S171\'");
                    this.Manager.Comment("checking step \'return InitializeFileTransferAsync/ERROR_SUCCESS\'");
                    this.Manager.Assert((temp146 == FRS2Model.error_status_t.ERROR_SUCCESS), String.Format("expected \'FRS2Model.error_status_t.ERROR_SUCCESS\', actual \'{0}\' (return of Initia" +
                                "lizeFileTransferAsync, state S171)", TestManagerHelpers.Describe(temp146)));
                    this.Manager.Comment("reaching state \'S189\'");
                    int temp147 = this.Manager.ExpectEvent(this.QuiescenceTimeout, true, new ExpectedEvent(TCtestScenario3.InitializeFileTransferAsyncEventInfo, null, new InitializeFileTransferAsyncEventDelegate1(this.TCtestScenario3S10InitializeFileTransferAsyncEventChecker8)), new ExpectedEvent(TCtestScenario3.InitializeFileTransferAsyncEventInfo, null, new InitializeFileTransferAsyncEventDelegate1(this.TCtestScenario3S10InitializeFileTransferAsyncEventChecker9)), new ExpectedEvent(TCtestScenario3.InitializeFileTransferAsyncEventInfo, null, new InitializeFileTransferAsyncEventDelegate1(this.TCtestScenario3S10InitializeFileTransferAsyncEventChecker10)), new ExpectedEvent(TCtestScenario3.InitializeFileTransferAsyncEventInfo, null, new InitializeFileTransferAsyncEventDelegate1(this.TCtestScenario3S10InitializeFileTransferAsyncEventChecker11)), new ExpectedEvent(TCtestScenario3.InitializeFileTransferAsyncEventInfo, null, new InitializeFileTransferAsyncEventDelegate1(this.TCtestScenario3S10InitializeFileTransferAsyncEventChecker12)), new ExpectedEvent(TCtestScenario3.InitializeFileTransferAsyncEventInfo, null, new InitializeFileTransferAsyncEventDelegate1(this.TCtestScenario3S10InitializeFileTransferAsyncEventChecker13)), new ExpectedEvent(TCtestScenario3.InitializeFileTransferAsyncEventInfo, null, new InitializeFileTransferAsyncEventDelegate1(this.TCtestScenario3S10InitializeFileTransferAsyncEventChecker14)), new ExpectedEvent(TCtestScenario3.InitializeFileTransferAsyncEventInfo, null, new InitializeFileTransferAsyncEventDelegate1(this.TCtestScenario3S10InitializeFileTransferAsyncEventChecker15)));
                    if ((temp147 == 0)) {
                        TCtestScenario3S238();
                        goto label25;
                    }
                    if ((temp147 == 1)) {
                        TCtestScenario3S207();
                        goto label25;
                    }
                    if ((temp147 == 2)) {
                        TCtestScenario3S206();
                        goto label25;
                    }
                    if ((temp147 == 3)) {
                        TCtestScenario3S272();
                        goto label25;
                    }
                    if ((temp147 == 4)) {
                        TCtestScenario3S235();
                        goto label25;
                    }
                    if ((temp147 == 5)) {
                        TCtestScenario3S203();
                        goto label25;
                    }
                    if ((temp147 == 6)) {
                        TCtestScenario3S202();
                        goto label25;
                    }
                    if ((temp147 == 7)) {
                        TCtestScenario3S239();
                        goto label25;
                    }
                    throw new InvalidOperationException("never reached");
                label25:
;
                    goto label26;
                }
                if ((temp148 == 2)) {
                    TCtestScenario3S141();
                    goto label26;
                }
                if ((temp148 == 3)) {
                    TCtestScenario3S127();
                    goto label26;
                }
                throw new InvalidOperationException("never reached");
            label26:
;
                goto label27;
            }
            if ((temp149 == 1)) {
                TCtestScenario3S96();
                goto label27;
            }
            throw new InvalidOperationException("never reached");
        label27:
;
            this.Manager.EndTest();
        }
        
        private void TCtestScenario3S10AsyncPollResponseEventChecker(FRS2Model.VVGeneration vvGen) {
            this.Manager.Comment("checking step \'event AsyncPollResponseEvent(ValidValue)\'");
            try {
                this.Manager.Assert((vvGen == FRS2Model.VVGeneration.ValidValue), String.Format("expected \'FRS2Model.VVGeneration.ValidValue\', actual \'{0}\' (vvGen of AsyncPollRes" +
                            "ponseEvent, state S92)", TestManagerHelpers.Describe(vvGen)));
            }
            catch (TransactionFailedException ) {
                this.Manager.Comment("This step would have covered MS-FRS2_R556, MS-FRS2_R1020, MS-FRS2_R555");
                throw;
            }
            this.Manager.Checkpoint("MS-FRS2_R556");
            this.Manager.Checkpoint("MS-FRS2_R1020");
            this.Manager.Checkpoint("MS-FRS2_R555");
        }
        
        private void TCtestScenario3S10RequestUpdatesEventChecker(FRS2Model.FilePresense present, FRS2Model.UPDATE_STATUS updateStatus) {
            this.Manager.Comment("checking step \'event RequestUpdatesEvent(fileDeleted,UPDATE_STATUS_DONE)\'");
            try {
                this.Manager.Assert((present == FRS2Model.FilePresense.fileDeleted), String.Format("expected \'FRS2Model.FilePresense.fileDeleted\', actual \'{0}\' (present of RequestUp" +
                            "datesEvent, state S124)", TestManagerHelpers.Describe(present)));
                this.Manager.Assert((updateStatus == FRS2Model.UPDATE_STATUS.UPDATE_STATUS_DONE), String.Format("expected \'FRS2Model.UPDATE_STATUS.UPDATE_STATUS_DONE\', actual \'{0}\' (updateStatus" +
                            " of RequestUpdatesEvent, state S124)", TestManagerHelpers.Describe(updateStatus)));
            }
            catch (TransactionFailedException ) {
                this.Manager.Comment("This step would have covered MS-FRS2_R93, MS-FRS2_R94");
                throw;
            }
            this.Manager.Checkpoint("MS-FRS2_R93");
            this.Manager.Checkpoint("MS-FRS2_R94");
        }
        
        private void TCtestScenario3S10InitializeFileTransferAsyncEventChecker(FRS2Model.ServerContext context, FRS2Model.RDC_Sig_Level rdcsigLevel, bool isEOF) {
            this.Manager.Comment("checking step \'event InitializeFileTransferAsyncEvent(ValidContext,forRAWFileLeve" +
                    "l,True)\'");
            this.Manager.Assert((context == FRS2Model.ServerContext.ValidContext), String.Format("expected \'FRS2Model.ServerContext.ValidContext\', actual \'{0}\' (context of Initial" +
                        "izeFileTransferAsyncEvent, state S188)", TestManagerHelpers.Describe(context)));
            this.Manager.Assert((rdcsigLevel == FRS2Model.RDC_Sig_Level.forRAWFileLevel), String.Format("expected \'FRS2Model.RDC_Sig_Level.forRAWFileLevel\', actual \'{0}\' (rdcsigLevel of " +
                        "InitializeFileTransferAsyncEvent, state S188)", TestManagerHelpers.Describe(rdcsigLevel)));
            this.Manager.Assert((isEOF == true), String.Format("expected \'true\', actual \'{0}\' (isEOF of InitializeFileTransferAsyncEvent, state S" +
                        "188)", TestManagerHelpers.Describe(isEOF)));
        }
        
        private void TCtestScenario3S10InitializeFileTransferAsyncEventChecker1(FRS2Model.ServerContext context, FRS2Model.RDC_Sig_Level rdcsigLevel, bool isEOF) {
            this.Manager.Comment("checking step \'event InitializeFileTransferAsyncEvent(ValidContext,forRDCFileLeve" +
                    "l,False)\'");
            this.Manager.Assert((context == FRS2Model.ServerContext.ValidContext), String.Format("expected \'FRS2Model.ServerContext.ValidContext\', actual \'{0}\' (context of Initial" +
                        "izeFileTransferAsyncEvent, state S188)", TestManagerHelpers.Describe(context)));
            this.Manager.Assert((rdcsigLevel == FRS2Model.RDC_Sig_Level.forRDCFileLevel), String.Format("expected \'FRS2Model.RDC_Sig_Level.forRDCFileLevel\', actual \'{0}\' (rdcsigLevel of " +
                        "InitializeFileTransferAsyncEvent, state S188)", TestManagerHelpers.Describe(rdcsigLevel)));
            this.Manager.Assert((isEOF == false), String.Format("expected \'false\', actual \'{0}\' (isEOF of InitializeFileTransferAsyncEvent, state " +
                        "S188)", TestManagerHelpers.Describe(isEOF)));
        }
        
        private void TCtestScenario3S10InitializeFileTransferAsyncEventChecker2(FRS2Model.ServerContext context, FRS2Model.RDC_Sig_Level rdcsigLevel, bool isEOF) {
            this.Manager.Comment("checking step \'event InitializeFileTransferAsyncEvent(InvalidContext,forRDCFileLe" +
                    "vel,False)\'");
            this.Manager.Assert((context == FRS2Model.ServerContext.InvalidContext), String.Format("expected \'FRS2Model.ServerContext.InvalidContext\', actual \'{0}\' (context of Initi" +
                        "alizeFileTransferAsyncEvent, state S188)", TestManagerHelpers.Describe(context)));
            this.Manager.Assert((rdcsigLevel == FRS2Model.RDC_Sig_Level.forRDCFileLevel), String.Format("expected \'FRS2Model.RDC_Sig_Level.forRDCFileLevel\', actual \'{0}\' (rdcsigLevel of " +
                        "InitializeFileTransferAsyncEvent, state S188)", TestManagerHelpers.Describe(rdcsigLevel)));
            this.Manager.Assert((isEOF == false), String.Format("expected \'false\', actual \'{0}\' (isEOF of InitializeFileTransferAsyncEvent, state " +
                        "S188)", TestManagerHelpers.Describe(isEOF)));
        }
        
        private void TCtestScenario3S10InitializeFileTransferAsyncEventChecker3(FRS2Model.ServerContext context, FRS2Model.RDC_Sig_Level rdcsigLevel, bool isEOF) {
            this.Manager.Comment("checking step \'event InitializeFileTransferAsyncEvent(ValidContext,forRAWFileLeve" +
                    "l,False)\'");
            this.Manager.Assert((context == FRS2Model.ServerContext.ValidContext), String.Format("expected \'FRS2Model.ServerContext.ValidContext\', actual \'{0}\' (context of Initial" +
                        "izeFileTransferAsyncEvent, state S188)", TestManagerHelpers.Describe(context)));
            this.Manager.Assert((rdcsigLevel == FRS2Model.RDC_Sig_Level.forRAWFileLevel), String.Format("expected \'FRS2Model.RDC_Sig_Level.forRAWFileLevel\', actual \'{0}\' (rdcsigLevel of " +
                        "InitializeFileTransferAsyncEvent, state S188)", TestManagerHelpers.Describe(rdcsigLevel)));
            this.Manager.Assert((isEOF == false), String.Format("expected \'false\', actual \'{0}\' (isEOF of InitializeFileTransferAsyncEvent, state " +
                        "S188)", TestManagerHelpers.Describe(isEOF)));
        }
        
        private void TCtestScenario3S10InitializeFileTransferAsyncEventChecker4(FRS2Model.ServerContext context, FRS2Model.RDC_Sig_Level rdcsigLevel, bool isEOF) {
            this.Manager.Comment("checking step \'event InitializeFileTransferAsyncEvent(InvalidContext,forRAWFileLe" +
                    "vel,True)\'");
            this.Manager.Assert((context == FRS2Model.ServerContext.InvalidContext), String.Format("expected \'FRS2Model.ServerContext.InvalidContext\', actual \'{0}\' (context of Initi" +
                        "alizeFileTransferAsyncEvent, state S188)", TestManagerHelpers.Describe(context)));
            this.Manager.Assert((rdcsigLevel == FRS2Model.RDC_Sig_Level.forRAWFileLevel), String.Format("expected \'FRS2Model.RDC_Sig_Level.forRAWFileLevel\', actual \'{0}\' (rdcsigLevel of " +
                        "InitializeFileTransferAsyncEvent, state S188)", TestManagerHelpers.Describe(rdcsigLevel)));
            this.Manager.Assert((isEOF == true), String.Format("expected \'true\', actual \'{0}\' (isEOF of InitializeFileTransferAsyncEvent, state S" +
                        "188)", TestManagerHelpers.Describe(isEOF)));
        }
        
        private void TCtestScenario3S10InitializeFileTransferAsyncEventChecker5(FRS2Model.ServerContext context, FRS2Model.RDC_Sig_Level rdcsigLevel, bool isEOF) {
            this.Manager.Comment("checking step \'event InitializeFileTransferAsyncEvent(InvalidContext,forRAWFileLe" +
                    "vel,False)\'");
            this.Manager.Assert((context == FRS2Model.ServerContext.InvalidContext), String.Format("expected \'FRS2Model.ServerContext.InvalidContext\', actual \'{0}\' (context of Initi" +
                        "alizeFileTransferAsyncEvent, state S188)", TestManagerHelpers.Describe(context)));
            this.Manager.Assert((rdcsigLevel == FRS2Model.RDC_Sig_Level.forRAWFileLevel), String.Format("expected \'FRS2Model.RDC_Sig_Level.forRAWFileLevel\', actual \'{0}\' (rdcsigLevel of " +
                        "InitializeFileTransferAsyncEvent, state S188)", TestManagerHelpers.Describe(rdcsigLevel)));
            this.Manager.Assert((isEOF == false), String.Format("expected \'false\', actual \'{0}\' (isEOF of InitializeFileTransferAsyncEvent, state " +
                        "S188)", TestManagerHelpers.Describe(isEOF)));
        }
        
        private void TCtestScenario3S10InitializeFileTransferAsyncEventChecker6(FRS2Model.ServerContext context, FRS2Model.RDC_Sig_Level rdcsigLevel, bool isEOF) {
            this.Manager.Comment("checking step \'event InitializeFileTransferAsyncEvent(InvalidContext,forRDCFileLe" +
                    "vel,True)\'");
            this.Manager.Assert((context == FRS2Model.ServerContext.InvalidContext), String.Format("expected \'FRS2Model.ServerContext.InvalidContext\', actual \'{0}\' (context of Initi" +
                        "alizeFileTransferAsyncEvent, state S188)", TestManagerHelpers.Describe(context)));
            this.Manager.Assert((rdcsigLevel == FRS2Model.RDC_Sig_Level.forRDCFileLevel), String.Format("expected \'FRS2Model.RDC_Sig_Level.forRDCFileLevel\', actual \'{0}\' (rdcsigLevel of " +
                        "InitializeFileTransferAsyncEvent, state S188)", TestManagerHelpers.Describe(rdcsigLevel)));
            this.Manager.Assert((isEOF == true), String.Format("expected \'true\', actual \'{0}\' (isEOF of InitializeFileTransferAsyncEvent, state S" +
                        "188)", TestManagerHelpers.Describe(isEOF)));
        }
        
        private void TCtestScenario3S10InitializeFileTransferAsyncEventChecker7(FRS2Model.ServerContext context, FRS2Model.RDC_Sig_Level rdcsigLevel, bool isEOF) {
            this.Manager.Comment("checking step \'event InitializeFileTransferAsyncEvent(ValidContext,forRDCFileLeve" +
                    "l,True)\'");
            this.Manager.Assert((context == FRS2Model.ServerContext.ValidContext), String.Format("expected \'FRS2Model.ServerContext.ValidContext\', actual \'{0}\' (context of Initial" +
                        "izeFileTransferAsyncEvent, state S188)", TestManagerHelpers.Describe(context)));
            this.Manager.Assert((rdcsigLevel == FRS2Model.RDC_Sig_Level.forRDCFileLevel), String.Format("expected \'FRS2Model.RDC_Sig_Level.forRDCFileLevel\', actual \'{0}\' (rdcsigLevel of " +
                        "InitializeFileTransferAsyncEvent, state S188)", TestManagerHelpers.Describe(rdcsigLevel)));
            this.Manager.Assert((isEOF == true), String.Format("expected \'true\', actual \'{0}\' (isEOF of InitializeFileTransferAsyncEvent, state S" +
                        "188)", TestManagerHelpers.Describe(isEOF)));
        }
        
        private void TCtestScenario3S10RequestUpdatesEventChecker1(FRS2Model.FilePresense present, FRS2Model.UPDATE_STATUS updateStatus) {
            this.Manager.Comment("checking step \'event RequestUpdatesEvent(fileExists,UPDATE_STATUS_DONE)\'");
            try {
                this.Manager.Assert((present == FRS2Model.FilePresense.fileExists), String.Format("expected \'FRS2Model.FilePresense.fileExists\', actual \'{0}\' (present of RequestUpd" +
                            "atesEvent, state S124)", TestManagerHelpers.Describe(present)));
                this.Manager.Assert((updateStatus == FRS2Model.UPDATE_STATUS.UPDATE_STATUS_DONE), String.Format("expected \'FRS2Model.UPDATE_STATUS.UPDATE_STATUS_DONE\', actual \'{0}\' (updateStatus" +
                            " of RequestUpdatesEvent, state S124)", TestManagerHelpers.Describe(updateStatus)));
            }
            catch (TransactionFailedException ) {
                this.Manager.Comment("This step would have covered MS-FRS2_R93, MS-FRS2_R94");
                throw;
            }
            this.Manager.Checkpoint("MS-FRS2_R93");
            this.Manager.Checkpoint("MS-FRS2_R94");
        }
        
        private void TCtestScenario3S10InitializeFileTransferAsyncEventChecker8(FRS2Model.ServerContext context, FRS2Model.RDC_Sig_Level rdcsigLevel, bool isEOF) {
            this.Manager.Comment("checking step \'event InitializeFileTransferAsyncEvent(InvalidContext,forRAWFileLe" +
                    "vel,True)\'");
            this.Manager.Assert((context == FRS2Model.ServerContext.InvalidContext), String.Format("expected \'FRS2Model.ServerContext.InvalidContext\', actual \'{0}\' (context of Initi" +
                        "alizeFileTransferAsyncEvent, state S189)", TestManagerHelpers.Describe(context)));
            this.Manager.Assert((rdcsigLevel == FRS2Model.RDC_Sig_Level.forRAWFileLevel), String.Format("expected \'FRS2Model.RDC_Sig_Level.forRAWFileLevel\', actual \'{0}\' (rdcsigLevel of " +
                        "InitializeFileTransferAsyncEvent, state S189)", TestManagerHelpers.Describe(rdcsigLevel)));
            this.Manager.Assert((isEOF == true), String.Format("expected \'true\', actual \'{0}\' (isEOF of InitializeFileTransferAsyncEvent, state S" +
                        "189)", TestManagerHelpers.Describe(isEOF)));
        }
        
        private void TCtestScenario3S10InitializeFileTransferAsyncEventChecker9(FRS2Model.ServerContext context, FRS2Model.RDC_Sig_Level rdcsigLevel, bool isEOF) {
            this.Manager.Comment("checking step \'event InitializeFileTransferAsyncEvent(ValidContext,forRAWFileLeve" +
                    "l,False)\'");
            this.Manager.Assert((context == FRS2Model.ServerContext.ValidContext), String.Format("expected \'FRS2Model.ServerContext.ValidContext\', actual \'{0}\' (context of Initial" +
                        "izeFileTransferAsyncEvent, state S189)", TestManagerHelpers.Describe(context)));
            this.Manager.Assert((rdcsigLevel == FRS2Model.RDC_Sig_Level.forRAWFileLevel), String.Format("expected \'FRS2Model.RDC_Sig_Level.forRAWFileLevel\', actual \'{0}\' (rdcsigLevel of " +
                        "InitializeFileTransferAsyncEvent, state S189)", TestManagerHelpers.Describe(rdcsigLevel)));
            this.Manager.Assert((isEOF == false), String.Format("expected \'false\', actual \'{0}\' (isEOF of InitializeFileTransferAsyncEvent, state " +
                        "S189)", TestManagerHelpers.Describe(isEOF)));
        }
        
        private void TCtestScenario3S10InitializeFileTransferAsyncEventChecker10(FRS2Model.ServerContext context, FRS2Model.RDC_Sig_Level rdcsigLevel, bool isEOF) {
            this.Manager.Comment("checking step \'event InitializeFileTransferAsyncEvent(InvalidContext,forRAWFileLe" +
                    "vel,False)\'");
            this.Manager.Assert((context == FRS2Model.ServerContext.InvalidContext), String.Format("expected \'FRS2Model.ServerContext.InvalidContext\', actual \'{0}\' (context of Initi" +
                        "alizeFileTransferAsyncEvent, state S189)", TestManagerHelpers.Describe(context)));
            this.Manager.Assert((rdcsigLevel == FRS2Model.RDC_Sig_Level.forRAWFileLevel), String.Format("expected \'FRS2Model.RDC_Sig_Level.forRAWFileLevel\', actual \'{0}\' (rdcsigLevel of " +
                        "InitializeFileTransferAsyncEvent, state S189)", TestManagerHelpers.Describe(rdcsigLevel)));
            this.Manager.Assert((isEOF == false), String.Format("expected \'false\', actual \'{0}\' (isEOF of InitializeFileTransferAsyncEvent, state " +
                        "S189)", TestManagerHelpers.Describe(isEOF)));
        }
        
        private void TCtestScenario3S10InitializeFileTransferAsyncEventChecker11(FRS2Model.ServerContext context, FRS2Model.RDC_Sig_Level rdcsigLevel, bool isEOF) {
            this.Manager.Comment("checking step \'event InitializeFileTransferAsyncEvent(ValidContext,forRDCFileLeve" +
                    "l,False)\'");
            this.Manager.Assert((context == FRS2Model.ServerContext.ValidContext), String.Format("expected \'FRS2Model.ServerContext.ValidContext\', actual \'{0}\' (context of Initial" +
                        "izeFileTransferAsyncEvent, state S189)", TestManagerHelpers.Describe(context)));
            this.Manager.Assert((rdcsigLevel == FRS2Model.RDC_Sig_Level.forRDCFileLevel), String.Format("expected \'FRS2Model.RDC_Sig_Level.forRDCFileLevel\', actual \'{0}\' (rdcsigLevel of " +
                        "InitializeFileTransferAsyncEvent, state S189)", TestManagerHelpers.Describe(rdcsigLevel)));
            this.Manager.Assert((isEOF == false), String.Format("expected \'false\', actual \'{0}\' (isEOF of InitializeFileTransferAsyncEvent, state " +
                        "S189)", TestManagerHelpers.Describe(isEOF)));
        }
        
        private void TCtestScenario3S10InitializeFileTransferAsyncEventChecker12(FRS2Model.ServerContext context, FRS2Model.RDC_Sig_Level rdcsigLevel, bool isEOF) {
            this.Manager.Comment("checking step \'event InitializeFileTransferAsyncEvent(InvalidContext,forRDCFileLe" +
                    "vel,True)\'");
            this.Manager.Assert((context == FRS2Model.ServerContext.InvalidContext), String.Format("expected \'FRS2Model.ServerContext.InvalidContext\', actual \'{0}\' (context of Initi" +
                        "alizeFileTransferAsyncEvent, state S189)", TestManagerHelpers.Describe(context)));
            this.Manager.Assert((rdcsigLevel == FRS2Model.RDC_Sig_Level.forRDCFileLevel), String.Format("expected \'FRS2Model.RDC_Sig_Level.forRDCFileLevel\', actual \'{0}\' (rdcsigLevel of " +
                        "InitializeFileTransferAsyncEvent, state S189)", TestManagerHelpers.Describe(rdcsigLevel)));
            this.Manager.Assert((isEOF == true), String.Format("expected \'true\', actual \'{0}\' (isEOF of InitializeFileTransferAsyncEvent, state S" +
                        "189)", TestManagerHelpers.Describe(isEOF)));
        }
        
        private void TCtestScenario3S10InitializeFileTransferAsyncEventChecker13(FRS2Model.ServerContext context, FRS2Model.RDC_Sig_Level rdcsigLevel, bool isEOF) {
            this.Manager.Comment("checking step \'event InitializeFileTransferAsyncEvent(ValidContext,forRDCFileLeve" +
                    "l,True)\'");
            this.Manager.Assert((context == FRS2Model.ServerContext.ValidContext), String.Format("expected \'FRS2Model.ServerContext.ValidContext\', actual \'{0}\' (context of Initial" +
                        "izeFileTransferAsyncEvent, state S189)", TestManagerHelpers.Describe(context)));
            this.Manager.Assert((rdcsigLevel == FRS2Model.RDC_Sig_Level.forRDCFileLevel), String.Format("expected \'FRS2Model.RDC_Sig_Level.forRDCFileLevel\', actual \'{0}\' (rdcsigLevel of " +
                        "InitializeFileTransferAsyncEvent, state S189)", TestManagerHelpers.Describe(rdcsigLevel)));
            this.Manager.Assert((isEOF == true), String.Format("expected \'true\', actual \'{0}\' (isEOF of InitializeFileTransferAsyncEvent, state S" +
                        "189)", TestManagerHelpers.Describe(isEOF)));
        }
        
        private void TCtestScenario3S10InitializeFileTransferAsyncEventChecker14(FRS2Model.ServerContext context, FRS2Model.RDC_Sig_Level rdcsigLevel, bool isEOF) {
            this.Manager.Comment("checking step \'event InitializeFileTransferAsyncEvent(InvalidContext,forRDCFileLe" +
                    "vel,False)\'");
            this.Manager.Assert((context == FRS2Model.ServerContext.InvalidContext), String.Format("expected \'FRS2Model.ServerContext.InvalidContext\', actual \'{0}\' (context of Initi" +
                        "alizeFileTransferAsyncEvent, state S189)", TestManagerHelpers.Describe(context)));
            this.Manager.Assert((rdcsigLevel == FRS2Model.RDC_Sig_Level.forRDCFileLevel), String.Format("expected \'FRS2Model.RDC_Sig_Level.forRDCFileLevel\', actual \'{0}\' (rdcsigLevel of " +
                        "InitializeFileTransferAsyncEvent, state S189)", TestManagerHelpers.Describe(rdcsigLevel)));
            this.Manager.Assert((isEOF == false), String.Format("expected \'false\', actual \'{0}\' (isEOF of InitializeFileTransferAsyncEvent, state " +
                        "S189)", TestManagerHelpers.Describe(isEOF)));
        }
        
        private void TCtestScenario3S10InitializeFileTransferAsyncEventChecker15(FRS2Model.ServerContext context, FRS2Model.RDC_Sig_Level rdcsigLevel, bool isEOF) {
            this.Manager.Comment("checking step \'event InitializeFileTransferAsyncEvent(ValidContext,forRAWFileLeve" +
                    "l,True)\'");
            this.Manager.Assert((context == FRS2Model.ServerContext.ValidContext), String.Format("expected \'FRS2Model.ServerContext.ValidContext\', actual \'{0}\' (context of Initial" +
                        "izeFileTransferAsyncEvent, state S189)", TestManagerHelpers.Describe(context)));
            this.Manager.Assert((rdcsigLevel == FRS2Model.RDC_Sig_Level.forRAWFileLevel), String.Format("expected \'FRS2Model.RDC_Sig_Level.forRAWFileLevel\', actual \'{0}\' (rdcsigLevel of " +
                        "InitializeFileTransferAsyncEvent, state S189)", TestManagerHelpers.Describe(rdcsigLevel)));
            this.Manager.Assert((isEOF == true), String.Format("expected \'true\', actual \'{0}\' (isEOF of InitializeFileTransferAsyncEvent, state S" +
                        "189)", TestManagerHelpers.Describe(isEOF)));
        }
        
        private void TCtestScenario3S10RequestUpdatesEventChecker2(FRS2Model.FilePresense present, FRS2Model.UPDATE_STATUS updateStatus) {
            this.Manager.Comment("checking step \'event RequestUpdatesEvent(fileExists,UPDATE_STATUS_MORE)\'");
            try {
                this.Manager.Assert((present == FRS2Model.FilePresense.fileExists), String.Format("expected \'FRS2Model.FilePresense.fileExists\', actual \'{0}\' (present of RequestUpd" +
                            "atesEvent, state S124)", TestManagerHelpers.Describe(present)));
                this.Manager.Assert((updateStatus == FRS2Model.UPDATE_STATUS.UPDATE_STATUS_MORE), String.Format("expected \'FRS2Model.UPDATE_STATUS.UPDATE_STATUS_MORE\', actual \'{0}\' (updateStatus" +
                            " of RequestUpdatesEvent, state S124)", TestManagerHelpers.Describe(updateStatus)));
            }
            catch (TransactionFailedException ) {
                this.Manager.Comment("This step would have covered MS-FRS2_R93, MS-FRS2_R498");
                throw;
            }
            this.Manager.Checkpoint("MS-FRS2_R93");
            this.Manager.Checkpoint("MS-FRS2_R498");
        }
        
        private void TCtestScenario3S10RequestUpdatesEventChecker3(FRS2Model.FilePresense present, FRS2Model.UPDATE_STATUS updateStatus) {
            this.Manager.Comment("checking step \'event RequestUpdatesEvent(fileDeleted,UPDATE_STATUS_MORE)\'");
            try {
                this.Manager.Assert((present == FRS2Model.FilePresense.fileDeleted), String.Format("expected \'FRS2Model.FilePresense.fileDeleted\', actual \'{0}\' (present of RequestUp" +
                            "datesEvent, state S124)", TestManagerHelpers.Describe(present)));
                this.Manager.Assert((updateStatus == FRS2Model.UPDATE_STATUS.UPDATE_STATUS_MORE), String.Format("expected \'FRS2Model.UPDATE_STATUS.UPDATE_STATUS_MORE\', actual \'{0}\' (updateStatus" +
                            " of RequestUpdatesEvent, state S124)", TestManagerHelpers.Describe(updateStatus)));
            }
            catch (TransactionFailedException ) {
                this.Manager.Comment("This step would have covered MS-FRS2_R93");
                throw;
            }
            this.Manager.Checkpoint("MS-FRS2_R93");
        }
        
        private void TCtestScenario3S10AsyncPollResponseEventChecker1(FRS2Model.VVGeneration vvGen) {
            this.Manager.Comment("checking step \'event AsyncPollResponseEvent(InvalidValue)\'");
            try {
                this.Manager.Assert((vvGen == FRS2Model.VVGeneration.InvalidValue), String.Format("expected \'FRS2Model.VVGeneration.InvalidValue\', actual \'{0}\' (vvGen of AsyncPollR" +
                            "esponseEvent, state S92)", TestManagerHelpers.Describe(vvGen)));
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
        
        #region Test Starting in S12
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        [Microsoft.VisualStudio.TestTools.UnitTesting.DescriptionAttribute(@"MS-FRS2_R37, MS-FRS2_R436, MS-FRS2_R439, MS-FRS2_R455, MS-FRS2_R458, MS-FRS2_R448, MS-FRS2_R549, MS-FRS2_R552, MS-FRS2_R517, MS-FRS2_R520, MS-FRS2_R466, MS-FRS2_R556, MS-FRS2_R1020, MS-FRS2_R555, MS-FRS2_R486, MS-FRS2_R489, MS-FRS2_R93, MS-FRS2_R94, MS-FRS2_R774, MS-FRS2_R777, MS-FRS2_R793, MS-FRS2_R93, MS-FRS2_R94, MS-FRS2_R774, MS-FRS2_R777, MS-FRS2_R793, MS-FRS2_R93, MS-FRS2_R498, MS-FRS2_R93, MS-FRS2_R556, MS-FRS2_R1020, MS-FRS2_R555")]
        public virtual void FRS2_TCtestScenario3S12() {
            this.Manager.BeginTest("TCtestScenario3S12");
            this.Manager.Comment("reaching state \'S12\'");
            FRS2Model.error_status_t temp150;
            this.Manager.Comment(@"executing step 'call Initialization(Windows7,Map{1=>enabled,2=>enabled,3=>enabled,4=>disabled},Map{1=>exists,2=>exists,3=>exists,4=>exists},Map{""A""=>1,""B""=>2,""C""=>3,""D""=>4},Map{""A""=>sysvol,""B""=>protection,""C""=>sysvol,""D""=>sysvol},Map{1=>""A"",2=>""B"",3=>""C"",4=>""D""},Map{1=>writeOnly,2=>readWrite,3=>readOnly,4=>readWrite},Map{1=>enabled,2=>disabled,3=>enabled,4=>enabled})'");
            temp150 = this.IFRS2ManagedAdapterInstance.Initialization(FRS2Model.OSVersion.Windows7, new Microsoft.Modeling.Map<System.Int32,FRS2Model.connectionProperty>().Add(1, FRS2Model.connectionProperty.enabled).Add(2, FRS2Model.connectionProperty.enabled).Add(3, FRS2Model.connectionProperty.enabled).Add(4, FRS2Model.connectionProperty.disabled), new Microsoft.Modeling.Map<System.Int32,FRS2Model.FromServer>().Add(1, FRS2Model.FromServer.exists).Add(2, FRS2Model.FromServer.exists).Add(3, FRS2Model.FromServer.exists).Add(4, FRS2Model.FromServer.exists), new Microsoft.Modeling.Map<System.String,System.Int32>().Add("A", 1).Add("B", 2).Add("C", 3).Add("D", 4), new Microsoft.Modeling.Map<System.String,FRS2Model.ReplicationGroupTypes>().Add("A", FRS2Model.ReplicationGroupTypes.sysvol).Add("B", FRS2Model.ReplicationGroupTypes.protection).Add("C", FRS2Model.ReplicationGroupTypes.sysvol).Add("D", FRS2Model.ReplicationGroupTypes.sysvol), new Microsoft.Modeling.Map<System.Int32,System.String>().Add(1, "A").Add(2, "B").Add(3, "C").Add(4, "D"), new Microsoft.Modeling.Map<System.Int32,FRS2Model.accessLevels>().Add(1, FRS2Model.accessLevels.writeOnly).Add(2, FRS2Model.accessLevels.readWrite).Add(3, FRS2Model.accessLevels.readOnly).Add(4, FRS2Model.accessLevels.readWrite), new Microsoft.Modeling.Map<System.Int32,FRS2Model.connectionProperty>().Add(1, FRS2Model.connectionProperty.enabled).Add(2, FRS2Model.connectionProperty.disabled).Add(3, FRS2Model.connectionProperty.enabled).Add(4, FRS2Model.connectionProperty.enabled));
            this.Manager.Comment("reaching state \'S13\'");
            this.Manager.Comment("checking step \'return Initialization/ERROR_SUCCESS\'");
            this.Manager.Assert((temp150 == FRS2Model.error_status_t.ERROR_SUCCESS), String.Format("expected \'FRS2Model.error_status_t.ERROR_SUCCESS\', actual \'{0}\' (return of Initia" +
                        "lization, state S13)", TestManagerHelpers.Describe(temp150)));
            this.Manager.Comment("reaching state \'S24\'");
            FRS2Model.ProtocolVersionReturned temp151;
            FRS2Model.UpstreamFlagValueReturned temp152;
            FRS2Model.error_status_t temp153;
            this.Manager.Comment("executing step \'call EstablishConnection(\"A\",1,FRS_COMMUNICATION_PROTOCOL_VERSION" +
                    "_LONGHORN_SERVER,out _,out _)\'");
            temp153 = this.IFRS2ManagedAdapterInstance.EstablishConnection("A", 1, FRS2Model.ProtocolVersion.FRS_COMMUNICATION_PROTOCOL_VERSION_LONGHORN_SERVER, out temp151, out temp152);
            this.Manager.Checkpoint("MS-FRS2_R37");
            this.Manager.Checkpoint("MS-FRS2_R436");
            this.Manager.Checkpoint("MS-FRS2_R439");
            this.Manager.Comment("reaching state \'S33\'");
            this.Manager.Comment("checking step \'return EstablishConnection/[out Valid,out Valid]:ERROR_SUCCESS\'");
            this.Manager.Assert((temp151 == FRS2Model.ProtocolVersionReturned.Valid), String.Format("expected \'FRS2Model.ProtocolVersionReturned.Valid\', actual \'{0}\' (upstreamProtoco" +
                        "lVersion of EstablishConnection, state S33)", TestManagerHelpers.Describe(temp151)));
            this.Manager.Assert((temp152 == FRS2Model.UpstreamFlagValueReturned.Valid), String.Format("expected \'FRS2Model.UpstreamFlagValueReturned.Valid\', actual \'{0}\' (upstreamFlags" +
                        " of EstablishConnection, state S33)", TestManagerHelpers.Describe(temp152)));
            this.Manager.Assert((temp153 == FRS2Model.error_status_t.ERROR_SUCCESS), String.Format("expected \'FRS2Model.error_status_t.ERROR_SUCCESS\', actual \'{0}\' (return of Establ" +
                        "ishConnection, state S33)", TestManagerHelpers.Describe(temp153)));
            this.Manager.Comment("reaching state \'S42\'");
            FRS2Model.error_status_t temp154;
            this.Manager.Comment("executing step \'call EstablishSession(1,1)\'");
            temp154 = this.IFRS2ManagedAdapterInstance.EstablishSession(1, 1);
            this.Manager.Checkpoint("MS-FRS2_R455");
            this.Manager.Checkpoint("MS-FRS2_R458");
            this.Manager.Checkpoint("MS-FRS2_R448");
            this.Manager.Comment("reaching state \'S51\'");
            this.Manager.Comment("checking step \'return EstablishSession/ERROR_SUCCESS\'");
            this.Manager.Assert((temp154 == FRS2Model.error_status_t.ERROR_SUCCESS), String.Format("expected \'FRS2Model.error_status_t.ERROR_SUCCESS\', actual \'{0}\' (return of Establ" +
                        "ishSession, state S51)", TestManagerHelpers.Describe(temp154)));
            this.Manager.Comment("reaching state \'S60\'");
            FRS2Model.error_status_t temp155;
            this.Manager.Comment("executing step \'call AsyncPoll(1)\'");
            temp155 = this.IFRS2ManagedAdapterInstance.AsyncPoll(1);
            this.Manager.Checkpoint("MS-FRS2_R549");
            this.Manager.Checkpoint("MS-FRS2_R552");
            this.Manager.Comment("reaching state \'S69\'");
            this.Manager.Comment("checking step \'return AsyncPoll/ERROR_SUCCESS\'");
            this.Manager.Assert((temp155 == FRS2Model.error_status_t.ERROR_SUCCESS), String.Format("expected \'FRS2Model.error_status_t.ERROR_SUCCESS\', actual \'{0}\' (return of AsyncP" +
                        "oll, state S69)", TestManagerHelpers.Describe(temp155)));
            this.Manager.Comment("reaching state \'S77\'");
            FRS2Model.error_status_t temp156;
            this.Manager.Comment("executing step \'call RequestVersionVector(1,1,1,REQUEST_NORMAL_SYNC,CHANGE_ALL,Va" +
                    "lidValue)\'");
            temp156 = this.IFRS2ManagedAdapterInstance.RequestVersionVector(1, 1, 1, FRS2Model.VERSION_REQUEST_TYPE.REQUEST_NORMAL_SYNC, FRS2Model.VERSION_CHANGE_TYPE.CHANGE_ALL, FRS2Model.VVGeneration.ValidValue);
            this.Manager.Checkpoint("MS-FRS2_R517");
            this.Manager.Checkpoint("MS-FRS2_R520");
            this.Manager.Checkpoint("MS-FRS2_R466");
            this.Manager.Comment("reaching state \'S85\'");
            this.Manager.Comment("checking step \'return RequestVersionVector/ERROR_SUCCESS\'");
            this.Manager.Assert((temp156 == FRS2Model.error_status_t.ERROR_SUCCESS), String.Format("expected \'FRS2Model.error_status_t.ERROR_SUCCESS\', actual \'{0}\' (return of Reques" +
                        "tVersionVector, state S85)", TestManagerHelpers.Describe(temp156)));
            this.Manager.Comment("reaching state \'S93\'");
            int temp163 = this.Manager.ExpectEvent(this.QuiescenceTimeout, true, new ExpectedEvent(TCtestScenario3.AsyncPollResponseEventInfo, null, new AsyncPollResponseEventDelegate1(this.TCtestScenario3S12AsyncPollResponseEventChecker)), new ExpectedEvent(TCtestScenario3.AsyncPollResponseEventInfo, null, new AsyncPollResponseEventDelegate1(this.TCtestScenario3S12AsyncPollResponseEventChecker1)));
            if ((temp163 == 0)) {
                this.Manager.Comment("reaching state \'S107\'");
                FRS2Model.error_status_t temp157;
                this.Manager.Comment("executing step \'call RequestUpdates(1,1,valid)\'");
                temp157 = this.IFRS2ManagedAdapterInstance.RequestUpdates(1, 1, FRS2Model.versionVectorDiff.valid);
                this.Manager.Checkpoint("MS-FRS2_R486");
                this.Manager.Checkpoint("MS-FRS2_R489");
                this.Manager.Comment("reaching state \'S117\'");
                this.Manager.Comment("checking step \'return RequestUpdates/ERROR_SUCCESS\'");
                this.Manager.Assert((temp157 == FRS2Model.error_status_t.ERROR_SUCCESS), String.Format("expected \'FRS2Model.error_status_t.ERROR_SUCCESS\', actual \'{0}\' (return of Reques" +
                            "tUpdates, state S117)", TestManagerHelpers.Describe(temp157)));
                this.Manager.Comment("reaching state \'S125\'");
                int temp162 = this.Manager.ExpectEvent(this.QuiescenceTimeout, true, new ExpectedEvent(TCtestScenario3.RequestUpdatesEventInfo, null, new RequestUpdatesEventDelegate1(this.TCtestScenario3S12RequestUpdatesEventChecker)), new ExpectedEvent(TCtestScenario3.RequestUpdatesEventInfo, null, new RequestUpdatesEventDelegate1(this.TCtestScenario3S12RequestUpdatesEventChecker1)), new ExpectedEvent(TCtestScenario3.RequestUpdatesEventInfo, null, new RequestUpdatesEventDelegate1(this.TCtestScenario3S12RequestUpdatesEventChecker2)), new ExpectedEvent(TCtestScenario3.RequestUpdatesEventInfo, null, new RequestUpdatesEventDelegate1(this.TCtestScenario3S12RequestUpdatesEventChecker3)));
                if ((temp162 == 0)) {
                    this.Manager.Comment("reaching state \'S151\'");
                    FRS2Model.error_status_t temp158;
                    this.Manager.Comment("executing step \'call InitializeFileTransferAsync(1,1,True)\'");
                    temp158 = this.IFRS2ManagedAdapterInstance.InitializeFileTransferAsync(1, 1, true);
                    this.Manager.Checkpoint("MS-FRS2_R774");
                    this.Manager.Checkpoint("MS-FRS2_R777");
                    this.Manager.Checkpoint("MS-FRS2_R793");
                    this.Manager.Comment("reaching state \'S172\'");
                    this.Manager.Comment("checking step \'return InitializeFileTransferAsync/ERROR_SUCCESS\'");
                    this.Manager.Assert((temp158 == FRS2Model.error_status_t.ERROR_SUCCESS), String.Format("expected \'FRS2Model.error_status_t.ERROR_SUCCESS\', actual \'{0}\' (return of Initia" +
                                "lizeFileTransferAsync, state S172)", TestManagerHelpers.Describe(temp158)));
                    this.Manager.Comment("reaching state \'S190\'");
                    int temp159 = this.Manager.ExpectEvent(this.QuiescenceTimeout, true, new ExpectedEvent(TCtestScenario3.InitializeFileTransferAsyncEventInfo, null, new InitializeFileTransferAsyncEventDelegate1(this.TCtestScenario3S12InitializeFileTransferAsyncEventChecker)), new ExpectedEvent(TCtestScenario3.InitializeFileTransferAsyncEventInfo, null, new InitializeFileTransferAsyncEventDelegate1(this.TCtestScenario3S12InitializeFileTransferAsyncEventChecker1)), new ExpectedEvent(TCtestScenario3.InitializeFileTransferAsyncEventInfo, null, new InitializeFileTransferAsyncEventDelegate1(this.TCtestScenario3S12InitializeFileTransferAsyncEventChecker2)), new ExpectedEvent(TCtestScenario3.InitializeFileTransferAsyncEventInfo, null, new InitializeFileTransferAsyncEventDelegate1(this.TCtestScenario3S12InitializeFileTransferAsyncEventChecker3)), new ExpectedEvent(TCtestScenario3.InitializeFileTransferAsyncEventInfo, null, new InitializeFileTransferAsyncEventDelegate1(this.TCtestScenario3S12InitializeFileTransferAsyncEventChecker4)), new ExpectedEvent(TCtestScenario3.InitializeFileTransferAsyncEventInfo, null, new InitializeFileTransferAsyncEventDelegate1(this.TCtestScenario3S12InitializeFileTransferAsyncEventChecker5)), new ExpectedEvent(TCtestScenario3.InitializeFileTransferAsyncEventInfo, null, new InitializeFileTransferAsyncEventDelegate1(this.TCtestScenario3S12InitializeFileTransferAsyncEventChecker6)), new ExpectedEvent(TCtestScenario3.InitializeFileTransferAsyncEventInfo, null, new InitializeFileTransferAsyncEventDelegate1(this.TCtestScenario3S12InitializeFileTransferAsyncEventChecker7)));
                    if ((temp159 == 0)) {
                        TCtestScenario3S230();
                        goto label28;
                    }
                    if ((temp159 == 1)) {
                        TCtestScenario3S264();
                        goto label28;
                    }
                    if ((temp159 == 2)) {
                        TCtestScenario3S199();
                        goto label28;
                    }
                    if ((temp159 == 3)) {
                        TCtestScenario3S197();
                        goto label28;
                    }
                    if ((temp159 == 4)) {
                        TCtestScenario3S227();
                        goto label28;
                    }
                    if ((temp159 == 5)) {
                        TCtestScenario3S228();
                        goto label28;
                    }
                    if ((temp159 == 6)) {
                        TCtestScenario3S194();
                        goto label28;
                    }
                    if ((temp159 == 7)) {
                        TCtestScenario3S192();
                        goto label28;
                    }
                    throw new InvalidOperationException("never reached");
                label28:
;
                    goto label30;
                }
                if ((temp162 == 1)) {
                    this.Manager.Comment("reaching state \'S152\'");
                    FRS2Model.error_status_t temp160;
                    this.Manager.Comment("executing step \'call InitializeFileTransferAsync(1,1,True)\'");
                    temp160 = this.IFRS2ManagedAdapterInstance.InitializeFileTransferAsync(1, 1, true);
                    this.Manager.Checkpoint("MS-FRS2_R774");
                    this.Manager.Checkpoint("MS-FRS2_R777");
                    this.Manager.Checkpoint("MS-FRS2_R793");
                    this.Manager.Comment("reaching state \'S173\'");
                    this.Manager.Comment("checking step \'return InitializeFileTransferAsync/ERROR_SUCCESS\'");
                    this.Manager.Assert((temp160 == FRS2Model.error_status_t.ERROR_SUCCESS), String.Format("expected \'FRS2Model.error_status_t.ERROR_SUCCESS\', actual \'{0}\' (return of Initia" +
                                "lizeFileTransferAsync, state S173)", TestManagerHelpers.Describe(temp160)));
                    this.Manager.Comment("reaching state \'S191\'");
                    int temp161 = this.Manager.ExpectEvent(this.QuiescenceTimeout, true, new ExpectedEvent(TCtestScenario3.InitializeFileTransferAsyncEventInfo, null, new InitializeFileTransferAsyncEventDelegate1(this.TCtestScenario3S12InitializeFileTransferAsyncEventChecker8)), new ExpectedEvent(TCtestScenario3.InitializeFileTransferAsyncEventInfo, null, new InitializeFileTransferAsyncEventDelegate1(this.TCtestScenario3S12InitializeFileTransferAsyncEventChecker9)), new ExpectedEvent(TCtestScenario3.InitializeFileTransferAsyncEventInfo, null, new InitializeFileTransferAsyncEventDelegate1(this.TCtestScenario3S12InitializeFileTransferAsyncEventChecker10)), new ExpectedEvent(TCtestScenario3.InitializeFileTransferAsyncEventInfo, null, new InitializeFileTransferAsyncEventDelegate1(this.TCtestScenario3S12InitializeFileTransferAsyncEventChecker11)), new ExpectedEvent(TCtestScenario3.InitializeFileTransferAsyncEventInfo, null, new InitializeFileTransferAsyncEventDelegate1(this.TCtestScenario3S12InitializeFileTransferAsyncEventChecker12)), new ExpectedEvent(TCtestScenario3.InitializeFileTransferAsyncEventInfo, null, new InitializeFileTransferAsyncEventDelegate1(this.TCtestScenario3S12InitializeFileTransferAsyncEventChecker13)), new ExpectedEvent(TCtestScenario3.InitializeFileTransferAsyncEventInfo, null, new InitializeFileTransferAsyncEventDelegate1(this.TCtestScenario3S12InitializeFileTransferAsyncEventChecker14)), new ExpectedEvent(TCtestScenario3.InitializeFileTransferAsyncEventInfo, null, new InitializeFileTransferAsyncEventDelegate1(this.TCtestScenario3S12InitializeFileTransferAsyncEventChecker15)));
                    if ((temp161 == 0)) {
                        TCtestScenario3S239();
                        goto label29;
                    }
                    if ((temp161 == 1)) {
                        TCtestScenario3S207();
                        goto label29;
                    }
                    if ((temp161 == 2)) {
                        TCtestScenario3S206();
                        goto label29;
                    }
                    if ((temp161 == 3)) {
                        TCtestScenario3S272();
                        goto label29;
                    }
                    if ((temp161 == 4)) {
                        TCtestScenario3S235();
                        goto label29;
                    }
                    if ((temp161 == 5)) {
                        TCtestScenario3S203();
                        goto label29;
                    }
                    if ((temp161 == 6)) {
                        TCtestScenario3S202();
                        goto label29;
                    }
                    if ((temp161 == 7)) {
                        TCtestScenario3S238();
                        goto label29;
                    }
                    throw new InvalidOperationException("never reached");
                label29:
;
                    goto label30;
                }
                if ((temp162 == 2)) {
                    TCtestScenario3S141();
                    goto label30;
                }
                if ((temp162 == 3)) {
                    TCtestScenario3S127();
                    goto label30;
                }
                throw new InvalidOperationException("never reached");
            label30:
;
                goto label31;
            }
            if ((temp163 == 1)) {
                TCtestScenario3S96();
                goto label31;
            }
            throw new InvalidOperationException("never reached");
        label31:
;
            this.Manager.EndTest();
        }
        
        private void TCtestScenario3S12AsyncPollResponseEventChecker(FRS2Model.VVGeneration vvGen) {
            this.Manager.Comment("checking step \'event AsyncPollResponseEvent(ValidValue)\'");
            try {
                this.Manager.Assert((vvGen == FRS2Model.VVGeneration.ValidValue), String.Format("expected \'FRS2Model.VVGeneration.ValidValue\', actual \'{0}\' (vvGen of AsyncPollRes" +
                            "ponseEvent, state S93)", TestManagerHelpers.Describe(vvGen)));
            }
            catch (TransactionFailedException ) {
                this.Manager.Comment("This step would have covered MS-FRS2_R556, MS-FRS2_R1020, MS-FRS2_R555");
                throw;
            }
            this.Manager.Checkpoint("MS-FRS2_R556");
            this.Manager.Checkpoint("MS-FRS2_R1020");
            this.Manager.Checkpoint("MS-FRS2_R555");
        }
        
        private void TCtestScenario3S12RequestUpdatesEventChecker(FRS2Model.FilePresense present, FRS2Model.UPDATE_STATUS updateStatus) {
            this.Manager.Comment("checking step \'event RequestUpdatesEvent(fileDeleted,UPDATE_STATUS_DONE)\'");
            try {
                this.Manager.Assert((present == FRS2Model.FilePresense.fileDeleted), String.Format("expected \'FRS2Model.FilePresense.fileDeleted\', actual \'{0}\' (present of RequestUp" +
                            "datesEvent, state S125)", TestManagerHelpers.Describe(present)));
                this.Manager.Assert((updateStatus == FRS2Model.UPDATE_STATUS.UPDATE_STATUS_DONE), String.Format("expected \'FRS2Model.UPDATE_STATUS.UPDATE_STATUS_DONE\', actual \'{0}\' (updateStatus" +
                            " of RequestUpdatesEvent, state S125)", TestManagerHelpers.Describe(updateStatus)));
            }
            catch (TransactionFailedException ) {
                this.Manager.Comment("This step would have covered MS-FRS2_R93, MS-FRS2_R94");
                throw;
            }
            this.Manager.Checkpoint("MS-FRS2_R93");
            this.Manager.Checkpoint("MS-FRS2_R94");
        }
        
        private void TCtestScenario3S12InitializeFileTransferAsyncEventChecker(FRS2Model.ServerContext context, FRS2Model.RDC_Sig_Level rdcsigLevel, bool isEOF) {
            this.Manager.Comment("checking step \'event InitializeFileTransferAsyncEvent(InvalidContext,forRDCFileLe" +
                    "vel,True)\'");
            this.Manager.Assert((context == FRS2Model.ServerContext.InvalidContext), String.Format("expected \'FRS2Model.ServerContext.InvalidContext\', actual \'{0}\' (context of Initi" +
                        "alizeFileTransferAsyncEvent, state S190)", TestManagerHelpers.Describe(context)));
            this.Manager.Assert((rdcsigLevel == FRS2Model.RDC_Sig_Level.forRDCFileLevel), String.Format("expected \'FRS2Model.RDC_Sig_Level.forRDCFileLevel\', actual \'{0}\' (rdcsigLevel of " +
                        "InitializeFileTransferAsyncEvent, state S190)", TestManagerHelpers.Describe(rdcsigLevel)));
            this.Manager.Assert((isEOF == true), String.Format("expected \'true\', actual \'{0}\' (isEOF of InitializeFileTransferAsyncEvent, state S" +
                        "190)", TestManagerHelpers.Describe(isEOF)));
        }
        
        private void TCtestScenario3S12InitializeFileTransferAsyncEventChecker1(FRS2Model.ServerContext context, FRS2Model.RDC_Sig_Level rdcsigLevel, bool isEOF) {
            this.Manager.Comment("checking step \'event InitializeFileTransferAsyncEvent(ValidContext,forRDCFileLeve" +
                    "l,False)\'");
            this.Manager.Assert((context == FRS2Model.ServerContext.ValidContext), String.Format("expected \'FRS2Model.ServerContext.ValidContext\', actual \'{0}\' (context of Initial" +
                        "izeFileTransferAsyncEvent, state S190)", TestManagerHelpers.Describe(context)));
            this.Manager.Assert((rdcsigLevel == FRS2Model.RDC_Sig_Level.forRDCFileLevel), String.Format("expected \'FRS2Model.RDC_Sig_Level.forRDCFileLevel\', actual \'{0}\' (rdcsigLevel of " +
                        "InitializeFileTransferAsyncEvent, state S190)", TestManagerHelpers.Describe(rdcsigLevel)));
            this.Manager.Assert((isEOF == false), String.Format("expected \'false\', actual \'{0}\' (isEOF of InitializeFileTransferAsyncEvent, state " +
                        "S190)", TestManagerHelpers.Describe(isEOF)));
        }
        
        private void TCtestScenario3S12InitializeFileTransferAsyncEventChecker2(FRS2Model.ServerContext context, FRS2Model.RDC_Sig_Level rdcsigLevel, bool isEOF) {
            this.Manager.Comment("checking step \'event InitializeFileTransferAsyncEvent(InvalidContext,forRDCFileLe" +
                    "vel,False)\'");
            this.Manager.Assert((context == FRS2Model.ServerContext.InvalidContext), String.Format("expected \'FRS2Model.ServerContext.InvalidContext\', actual \'{0}\' (context of Initi" +
                        "alizeFileTransferAsyncEvent, state S190)", TestManagerHelpers.Describe(context)));
            this.Manager.Assert((rdcsigLevel == FRS2Model.RDC_Sig_Level.forRDCFileLevel), String.Format("expected \'FRS2Model.RDC_Sig_Level.forRDCFileLevel\', actual \'{0}\' (rdcsigLevel of " +
                        "InitializeFileTransferAsyncEvent, state S190)", TestManagerHelpers.Describe(rdcsigLevel)));
            this.Manager.Assert((isEOF == false), String.Format("expected \'false\', actual \'{0}\' (isEOF of InitializeFileTransferAsyncEvent, state " +
                        "S190)", TestManagerHelpers.Describe(isEOF)));
        }
        
        private void TCtestScenario3S12InitializeFileTransferAsyncEventChecker3(FRS2Model.ServerContext context, FRS2Model.RDC_Sig_Level rdcsigLevel, bool isEOF) {
            this.Manager.Comment("checking step \'event InitializeFileTransferAsyncEvent(ValidContext,forRAWFileLeve" +
                    "l,False)\'");
            this.Manager.Assert((context == FRS2Model.ServerContext.ValidContext), String.Format("expected \'FRS2Model.ServerContext.ValidContext\', actual \'{0}\' (context of Initial" +
                        "izeFileTransferAsyncEvent, state S190)", TestManagerHelpers.Describe(context)));
            this.Manager.Assert((rdcsigLevel == FRS2Model.RDC_Sig_Level.forRAWFileLevel), String.Format("expected \'FRS2Model.RDC_Sig_Level.forRAWFileLevel\', actual \'{0}\' (rdcsigLevel of " +
                        "InitializeFileTransferAsyncEvent, state S190)", TestManagerHelpers.Describe(rdcsigLevel)));
            this.Manager.Assert((isEOF == false), String.Format("expected \'false\', actual \'{0}\' (isEOF of InitializeFileTransferAsyncEvent, state " +
                        "S190)", TestManagerHelpers.Describe(isEOF)));
        }
        
        private void TCtestScenario3S12InitializeFileTransferAsyncEventChecker4(FRS2Model.ServerContext context, FRS2Model.RDC_Sig_Level rdcsigLevel, bool isEOF) {
            this.Manager.Comment("checking step \'event InitializeFileTransferAsyncEvent(InvalidContext,forRAWFileLe" +
                    "vel,True)\'");
            this.Manager.Assert((context == FRS2Model.ServerContext.InvalidContext), String.Format("expected \'FRS2Model.ServerContext.InvalidContext\', actual \'{0}\' (context of Initi" +
                        "alizeFileTransferAsyncEvent, state S190)", TestManagerHelpers.Describe(context)));
            this.Manager.Assert((rdcsigLevel == FRS2Model.RDC_Sig_Level.forRAWFileLevel), String.Format("expected \'FRS2Model.RDC_Sig_Level.forRAWFileLevel\', actual \'{0}\' (rdcsigLevel of " +
                        "InitializeFileTransferAsyncEvent, state S190)", TestManagerHelpers.Describe(rdcsigLevel)));
            this.Manager.Assert((isEOF == true), String.Format("expected \'true\', actual \'{0}\' (isEOF of InitializeFileTransferAsyncEvent, state S" +
                        "190)", TestManagerHelpers.Describe(isEOF)));
        }
        
        private void TCtestScenario3S12InitializeFileTransferAsyncEventChecker5(FRS2Model.ServerContext context, FRS2Model.RDC_Sig_Level rdcsigLevel, bool isEOF) {
            this.Manager.Comment("checking step \'event InitializeFileTransferAsyncEvent(ValidContext,forRAWFileLeve" +
                    "l,True)\'");
            this.Manager.Assert((context == FRS2Model.ServerContext.ValidContext), String.Format("expected \'FRS2Model.ServerContext.ValidContext\', actual \'{0}\' (context of Initial" +
                        "izeFileTransferAsyncEvent, state S190)", TestManagerHelpers.Describe(context)));
            this.Manager.Assert((rdcsigLevel == FRS2Model.RDC_Sig_Level.forRAWFileLevel), String.Format("expected \'FRS2Model.RDC_Sig_Level.forRAWFileLevel\', actual \'{0}\' (rdcsigLevel of " +
                        "InitializeFileTransferAsyncEvent, state S190)", TestManagerHelpers.Describe(rdcsigLevel)));
            this.Manager.Assert((isEOF == true), String.Format("expected \'true\', actual \'{0}\' (isEOF of InitializeFileTransferAsyncEvent, state S" +
                        "190)", TestManagerHelpers.Describe(isEOF)));
        }
        
        private void TCtestScenario3S12InitializeFileTransferAsyncEventChecker6(FRS2Model.ServerContext context, FRS2Model.RDC_Sig_Level rdcsigLevel, bool isEOF) {
            this.Manager.Comment("checking step \'event InitializeFileTransferAsyncEvent(InvalidContext,forRAWFileLe" +
                    "vel,False)\'");
            this.Manager.Assert((context == FRS2Model.ServerContext.InvalidContext), String.Format("expected \'FRS2Model.ServerContext.InvalidContext\', actual \'{0}\' (context of Initi" +
                        "alizeFileTransferAsyncEvent, state S190)", TestManagerHelpers.Describe(context)));
            this.Manager.Assert((rdcsigLevel == FRS2Model.RDC_Sig_Level.forRAWFileLevel), String.Format("expected \'FRS2Model.RDC_Sig_Level.forRAWFileLevel\', actual \'{0}\' (rdcsigLevel of " +
                        "InitializeFileTransferAsyncEvent, state S190)", TestManagerHelpers.Describe(rdcsigLevel)));
            this.Manager.Assert((isEOF == false), String.Format("expected \'false\', actual \'{0}\' (isEOF of InitializeFileTransferAsyncEvent, state " +
                        "S190)", TestManagerHelpers.Describe(isEOF)));
        }
        
        private void TCtestScenario3S12InitializeFileTransferAsyncEventChecker7(FRS2Model.ServerContext context, FRS2Model.RDC_Sig_Level rdcsigLevel, bool isEOF) {
            this.Manager.Comment("checking step \'event InitializeFileTransferAsyncEvent(ValidContext,forRDCFileLeve" +
                    "l,True)\'");
            this.Manager.Assert((context == FRS2Model.ServerContext.ValidContext), String.Format("expected \'FRS2Model.ServerContext.ValidContext\', actual \'{0}\' (context of Initial" +
                        "izeFileTransferAsyncEvent, state S190)", TestManagerHelpers.Describe(context)));
            this.Manager.Assert((rdcsigLevel == FRS2Model.RDC_Sig_Level.forRDCFileLevel), String.Format("expected \'FRS2Model.RDC_Sig_Level.forRDCFileLevel\', actual \'{0}\' (rdcsigLevel of " +
                        "InitializeFileTransferAsyncEvent, state S190)", TestManagerHelpers.Describe(rdcsigLevel)));
            this.Manager.Assert((isEOF == true), String.Format("expected \'true\', actual \'{0}\' (isEOF of InitializeFileTransferAsyncEvent, state S" +
                        "190)", TestManagerHelpers.Describe(isEOF)));
        }
        
        private void TCtestScenario3S12RequestUpdatesEventChecker1(FRS2Model.FilePresense present, FRS2Model.UPDATE_STATUS updateStatus) {
            this.Manager.Comment("checking step \'event RequestUpdatesEvent(fileExists,UPDATE_STATUS_DONE)\'");
            try {
                this.Manager.Assert((present == FRS2Model.FilePresense.fileExists), String.Format("expected \'FRS2Model.FilePresense.fileExists\', actual \'{0}\' (present of RequestUpd" +
                            "atesEvent, state S125)", TestManagerHelpers.Describe(present)));
                this.Manager.Assert((updateStatus == FRS2Model.UPDATE_STATUS.UPDATE_STATUS_DONE), String.Format("expected \'FRS2Model.UPDATE_STATUS.UPDATE_STATUS_DONE\', actual \'{0}\' (updateStatus" +
                            " of RequestUpdatesEvent, state S125)", TestManagerHelpers.Describe(updateStatus)));
            }
            catch (TransactionFailedException ) {
                this.Manager.Comment("This step would have covered MS-FRS2_R93, MS-FRS2_R94");
                throw;
            }
            this.Manager.Checkpoint("MS-FRS2_R93");
            this.Manager.Checkpoint("MS-FRS2_R94");
        }
        
        private void TCtestScenario3S12InitializeFileTransferAsyncEventChecker8(FRS2Model.ServerContext context, FRS2Model.RDC_Sig_Level rdcsigLevel, bool isEOF) {
            this.Manager.Comment("checking step \'event InitializeFileTransferAsyncEvent(ValidContext,forRAWFileLeve" +
                    "l,True)\'");
            this.Manager.Assert((context == FRS2Model.ServerContext.ValidContext), String.Format("expected \'FRS2Model.ServerContext.ValidContext\', actual \'{0}\' (context of Initial" +
                        "izeFileTransferAsyncEvent, state S191)", TestManagerHelpers.Describe(context)));
            this.Manager.Assert((rdcsigLevel == FRS2Model.RDC_Sig_Level.forRAWFileLevel), String.Format("expected \'FRS2Model.RDC_Sig_Level.forRAWFileLevel\', actual \'{0}\' (rdcsigLevel of " +
                        "InitializeFileTransferAsyncEvent, state S191)", TestManagerHelpers.Describe(rdcsigLevel)));
            this.Manager.Assert((isEOF == true), String.Format("expected \'true\', actual \'{0}\' (isEOF of InitializeFileTransferAsyncEvent, state S" +
                        "191)", TestManagerHelpers.Describe(isEOF)));
        }
        
        private void TCtestScenario3S12InitializeFileTransferAsyncEventChecker9(FRS2Model.ServerContext context, FRS2Model.RDC_Sig_Level rdcsigLevel, bool isEOF) {
            this.Manager.Comment("checking step \'event InitializeFileTransferAsyncEvent(ValidContext,forRAWFileLeve" +
                    "l,False)\'");
            this.Manager.Assert((context == FRS2Model.ServerContext.ValidContext), String.Format("expected \'FRS2Model.ServerContext.ValidContext\', actual \'{0}\' (context of Initial" +
                        "izeFileTransferAsyncEvent, state S191)", TestManagerHelpers.Describe(context)));
            this.Manager.Assert((rdcsigLevel == FRS2Model.RDC_Sig_Level.forRAWFileLevel), String.Format("expected \'FRS2Model.RDC_Sig_Level.forRAWFileLevel\', actual \'{0}\' (rdcsigLevel of " +
                        "InitializeFileTransferAsyncEvent, state S191)", TestManagerHelpers.Describe(rdcsigLevel)));
            this.Manager.Assert((isEOF == false), String.Format("expected \'false\', actual \'{0}\' (isEOF of InitializeFileTransferAsyncEvent, state " +
                        "S191)", TestManagerHelpers.Describe(isEOF)));
        }
        
        private void TCtestScenario3S12InitializeFileTransferAsyncEventChecker10(FRS2Model.ServerContext context, FRS2Model.RDC_Sig_Level rdcsigLevel, bool isEOF) {
            this.Manager.Comment("checking step \'event InitializeFileTransferAsyncEvent(InvalidContext,forRAWFileLe" +
                    "vel,False)\'");
            this.Manager.Assert((context == FRS2Model.ServerContext.InvalidContext), String.Format("expected \'FRS2Model.ServerContext.InvalidContext\', actual \'{0}\' (context of Initi" +
                        "alizeFileTransferAsyncEvent, state S191)", TestManagerHelpers.Describe(context)));
            this.Manager.Assert((rdcsigLevel == FRS2Model.RDC_Sig_Level.forRAWFileLevel), String.Format("expected \'FRS2Model.RDC_Sig_Level.forRAWFileLevel\', actual \'{0}\' (rdcsigLevel of " +
                        "InitializeFileTransferAsyncEvent, state S191)", TestManagerHelpers.Describe(rdcsigLevel)));
            this.Manager.Assert((isEOF == false), String.Format("expected \'false\', actual \'{0}\' (isEOF of InitializeFileTransferAsyncEvent, state " +
                        "S191)", TestManagerHelpers.Describe(isEOF)));
        }
        
        private void TCtestScenario3S12InitializeFileTransferAsyncEventChecker11(FRS2Model.ServerContext context, FRS2Model.RDC_Sig_Level rdcsigLevel, bool isEOF) {
            this.Manager.Comment("checking step \'event InitializeFileTransferAsyncEvent(ValidContext,forRDCFileLeve" +
                    "l,False)\'");
            this.Manager.Assert((context == FRS2Model.ServerContext.ValidContext), String.Format("expected \'FRS2Model.ServerContext.ValidContext\', actual \'{0}\' (context of Initial" +
                        "izeFileTransferAsyncEvent, state S191)", TestManagerHelpers.Describe(context)));
            this.Manager.Assert((rdcsigLevel == FRS2Model.RDC_Sig_Level.forRDCFileLevel), String.Format("expected \'FRS2Model.RDC_Sig_Level.forRDCFileLevel\', actual \'{0}\' (rdcsigLevel of " +
                        "InitializeFileTransferAsyncEvent, state S191)", TestManagerHelpers.Describe(rdcsigLevel)));
            this.Manager.Assert((isEOF == false), String.Format("expected \'false\', actual \'{0}\' (isEOF of InitializeFileTransferAsyncEvent, state " +
                        "S191)", TestManagerHelpers.Describe(isEOF)));
        }
        
        private void TCtestScenario3S12InitializeFileTransferAsyncEventChecker12(FRS2Model.ServerContext context, FRS2Model.RDC_Sig_Level rdcsigLevel, bool isEOF) {
            this.Manager.Comment("checking step \'event InitializeFileTransferAsyncEvent(InvalidContext,forRDCFileLe" +
                    "vel,True)\'");
            this.Manager.Assert((context == FRS2Model.ServerContext.InvalidContext), String.Format("expected \'FRS2Model.ServerContext.InvalidContext\', actual \'{0}\' (context of Initi" +
                        "alizeFileTransferAsyncEvent, state S191)", TestManagerHelpers.Describe(context)));
            this.Manager.Assert((rdcsigLevel == FRS2Model.RDC_Sig_Level.forRDCFileLevel), String.Format("expected \'FRS2Model.RDC_Sig_Level.forRDCFileLevel\', actual \'{0}\' (rdcsigLevel of " +
                        "InitializeFileTransferAsyncEvent, state S191)", TestManagerHelpers.Describe(rdcsigLevel)));
            this.Manager.Assert((isEOF == true), String.Format("expected \'true\', actual \'{0}\' (isEOF of InitializeFileTransferAsyncEvent, state S" +
                        "191)", TestManagerHelpers.Describe(isEOF)));
        }
        
        private void TCtestScenario3S12InitializeFileTransferAsyncEventChecker13(FRS2Model.ServerContext context, FRS2Model.RDC_Sig_Level rdcsigLevel, bool isEOF) {
            this.Manager.Comment("checking step \'event InitializeFileTransferAsyncEvent(ValidContext,forRDCFileLeve" +
                    "l,True)\'");
            this.Manager.Assert((context == FRS2Model.ServerContext.ValidContext), String.Format("expected \'FRS2Model.ServerContext.ValidContext\', actual \'{0}\' (context of Initial" +
                        "izeFileTransferAsyncEvent, state S191)", TestManagerHelpers.Describe(context)));
            this.Manager.Assert((rdcsigLevel == FRS2Model.RDC_Sig_Level.forRDCFileLevel), String.Format("expected \'FRS2Model.RDC_Sig_Level.forRDCFileLevel\', actual \'{0}\' (rdcsigLevel of " +
                        "InitializeFileTransferAsyncEvent, state S191)", TestManagerHelpers.Describe(rdcsigLevel)));
            this.Manager.Assert((isEOF == true), String.Format("expected \'true\', actual \'{0}\' (isEOF of InitializeFileTransferAsyncEvent, state S" +
                        "191)", TestManagerHelpers.Describe(isEOF)));
        }
        
        private void TCtestScenario3S12InitializeFileTransferAsyncEventChecker14(FRS2Model.ServerContext context, FRS2Model.RDC_Sig_Level rdcsigLevel, bool isEOF) {
            this.Manager.Comment("checking step \'event InitializeFileTransferAsyncEvent(InvalidContext,forRDCFileLe" +
                    "vel,False)\'");
            this.Manager.Assert((context == FRS2Model.ServerContext.InvalidContext), String.Format("expected \'FRS2Model.ServerContext.InvalidContext\', actual \'{0}\' (context of Initi" +
                        "alizeFileTransferAsyncEvent, state S191)", TestManagerHelpers.Describe(context)));
            this.Manager.Assert((rdcsigLevel == FRS2Model.RDC_Sig_Level.forRDCFileLevel), String.Format("expected \'FRS2Model.RDC_Sig_Level.forRDCFileLevel\', actual \'{0}\' (rdcsigLevel of " +
                        "InitializeFileTransferAsyncEvent, state S191)", TestManagerHelpers.Describe(rdcsigLevel)));
            this.Manager.Assert((isEOF == false), String.Format("expected \'false\', actual \'{0}\' (isEOF of InitializeFileTransferAsyncEvent, state " +
                        "S191)", TestManagerHelpers.Describe(isEOF)));
        }
        
        private void TCtestScenario3S12InitializeFileTransferAsyncEventChecker15(FRS2Model.ServerContext context, FRS2Model.RDC_Sig_Level rdcsigLevel, bool isEOF) {
            this.Manager.Comment("checking step \'event InitializeFileTransferAsyncEvent(InvalidContext,forRAWFileLe" +
                    "vel,True)\'");
            this.Manager.Assert((context == FRS2Model.ServerContext.InvalidContext), String.Format("expected \'FRS2Model.ServerContext.InvalidContext\', actual \'{0}\' (context of Initi" +
                        "alizeFileTransferAsyncEvent, state S191)", TestManagerHelpers.Describe(context)));
            this.Manager.Assert((rdcsigLevel == FRS2Model.RDC_Sig_Level.forRAWFileLevel), String.Format("expected \'FRS2Model.RDC_Sig_Level.forRAWFileLevel\', actual \'{0}\' (rdcsigLevel of " +
                        "InitializeFileTransferAsyncEvent, state S191)", TestManagerHelpers.Describe(rdcsigLevel)));
            this.Manager.Assert((isEOF == true), String.Format("expected \'true\', actual \'{0}\' (isEOF of InitializeFileTransferAsyncEvent, state S" +
                        "191)", TestManagerHelpers.Describe(isEOF)));
        }
        
        private void TCtestScenario3S12RequestUpdatesEventChecker2(FRS2Model.FilePresense present, FRS2Model.UPDATE_STATUS updateStatus) {
            this.Manager.Comment("checking step \'event RequestUpdatesEvent(fileExists,UPDATE_STATUS_MORE)\'");
            try {
                this.Manager.Assert((present == FRS2Model.FilePresense.fileExists), String.Format("expected \'FRS2Model.FilePresense.fileExists\', actual \'{0}\' (present of RequestUpd" +
                            "atesEvent, state S125)", TestManagerHelpers.Describe(present)));
                this.Manager.Assert((updateStatus == FRS2Model.UPDATE_STATUS.UPDATE_STATUS_MORE), String.Format("expected \'FRS2Model.UPDATE_STATUS.UPDATE_STATUS_MORE\', actual \'{0}\' (updateStatus" +
                            " of RequestUpdatesEvent, state S125)", TestManagerHelpers.Describe(updateStatus)));
            }
            catch (TransactionFailedException ) {
                this.Manager.Comment("This step would have covered MS-FRS2_R93, MS-FRS2_R498");
                throw;
            }
            this.Manager.Checkpoint("MS-FRS2_R93");
            this.Manager.Checkpoint("MS-FRS2_R498");
        }
        
        private void TCtestScenario3S12RequestUpdatesEventChecker3(FRS2Model.FilePresense present, FRS2Model.UPDATE_STATUS updateStatus) {
            this.Manager.Comment("checking step \'event RequestUpdatesEvent(fileDeleted,UPDATE_STATUS_MORE)\'");
            try {
                this.Manager.Assert((present == FRS2Model.FilePresense.fileDeleted), String.Format("expected \'FRS2Model.FilePresense.fileDeleted\', actual \'{0}\' (present of RequestUp" +
                            "datesEvent, state S125)", TestManagerHelpers.Describe(present)));
                this.Manager.Assert((updateStatus == FRS2Model.UPDATE_STATUS.UPDATE_STATUS_MORE), String.Format("expected \'FRS2Model.UPDATE_STATUS.UPDATE_STATUS_MORE\', actual \'{0}\' (updateStatus" +
                            " of RequestUpdatesEvent, state S125)", TestManagerHelpers.Describe(updateStatus)));
            }
            catch (TransactionFailedException ) {
                this.Manager.Comment("This step would have covered MS-FRS2_R93");
                throw;
            }
            this.Manager.Checkpoint("MS-FRS2_R93");
        }
        
        private void TCtestScenario3S12AsyncPollResponseEventChecker1(FRS2Model.VVGeneration vvGen) {
            this.Manager.Comment("checking step \'event AsyncPollResponseEvent(InvalidValue)\'");
            try {
                this.Manager.Assert((vvGen == FRS2Model.VVGeneration.InvalidValue), String.Format("expected \'FRS2Model.VVGeneration.InvalidValue\', actual \'{0}\' (vvGen of AsyncPollR" +
                            "esponseEvent, state S93)", TestManagerHelpers.Describe(vvGen)));
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
        
        #region Test Starting in S14
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        [Microsoft.VisualStudio.TestTools.UnitTesting.DescriptionAttribute(@"MS-FRS2_R37, MS-FRS2_R436, MS-FRS2_R439, MS-FRS2_R455, MS-FRS2_R458, MS-FRS2_R448, MS-FRS2_R549, MS-FRS2_R552, MS-FRS2_R517, MS-FRS2_R520, MS-FRS2_R466, MS-FRS2_R556, MS-FRS2_R1020, MS-FRS2_R555, MS-FRS2_R487, MS-FRS2_R494, MS-FRS2_R556, MS-FRS2_R1020, MS-FRS2_R555")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestCategory("AD")]
        [TestCategory("MS-FRS2")]
        [TestCategory("SDC")]
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        
        public virtual void FRS2_TCtestScenario3S14()
        {
            this.Manager.BeginTest("TCtestScenario3S14");
            this.Manager.Comment("reaching state \'S14\'");
            FRS2Model.error_status_t temp164;
            this.Manager.Comment(@"executing step 'call Initialization(Windows7,Map{1=>enabled,2=>enabled,3=>enabled,4=>disabled},Map{1=>exists,2=>exists,3=>exists,4=>exists},Map{""A""=>1,""B""=>2,""C""=>3,""D""=>4},Map{""A""=>sysvol,""B""=>protection,""C""=>sysvol,""D""=>sysvol},Map{1=>""A"",2=>""B"",3=>""C"",4=>""D""},Map{1=>writeOnly,2=>readWrite,3=>readOnly,4=>readWrite},Map{1=>enabled,2=>disabled,3=>enabled,4=>enabled})'");
            temp164 = this.IFRS2ManagedAdapterInstance.Initialization(FRS2Model.OSVersion.Windows7, new Microsoft.Modeling.Map<System.Int32,FRS2Model.connectionProperty>().Add(1, FRS2Model.connectionProperty.enabled).Add(2, FRS2Model.connectionProperty.enabled).Add(3, FRS2Model.connectionProperty.enabled).Add(4, FRS2Model.connectionProperty.disabled), new Microsoft.Modeling.Map<System.Int32,FRS2Model.FromServer>().Add(1, FRS2Model.FromServer.exists).Add(2, FRS2Model.FromServer.exists).Add(3, FRS2Model.FromServer.exists).Add(4, FRS2Model.FromServer.exists), new Microsoft.Modeling.Map<System.String,System.Int32>().Add("A", 1).Add("B", 2).Add("C", 3).Add("D", 4), new Microsoft.Modeling.Map<System.String,FRS2Model.ReplicationGroupTypes>().Add("A", FRS2Model.ReplicationGroupTypes.sysvol).Add("B", FRS2Model.ReplicationGroupTypes.protection).Add("C", FRS2Model.ReplicationGroupTypes.sysvol).Add("D", FRS2Model.ReplicationGroupTypes.sysvol), new Microsoft.Modeling.Map<System.Int32,System.String>().Add(1, "A").Add(2, "B").Add(3, "C").Add(4, "D"), new Microsoft.Modeling.Map<System.Int32,FRS2Model.accessLevels>().Add(1, FRS2Model.accessLevels.writeOnly).Add(2, FRS2Model.accessLevels.readWrite).Add(3, FRS2Model.accessLevels.readOnly).Add(4, FRS2Model.accessLevels.readWrite), new Microsoft.Modeling.Map<System.Int32,FRS2Model.connectionProperty>().Add(1, FRS2Model.connectionProperty.enabled).Add(2, FRS2Model.connectionProperty.disabled).Add(3, FRS2Model.connectionProperty.enabled).Add(4, FRS2Model.connectionProperty.enabled));
            this.Manager.Comment("reaching state \'S15\'");
            this.Manager.Comment("checking step \'return Initialization/ERROR_SUCCESS\'");
            this.Manager.Assert((temp164 == FRS2Model.error_status_t.ERROR_SUCCESS), String.Format("expected \'FRS2Model.error_status_t.ERROR_SUCCESS\', actual \'{0}\' (return of Initia" +
                        "lization, state S15)", TestManagerHelpers.Describe(temp164)));
            this.Manager.Comment("reaching state \'S25\'");
            FRS2Model.ProtocolVersionReturned temp165;
            FRS2Model.UpstreamFlagValueReturned temp166;
            FRS2Model.error_status_t temp167;
            this.Manager.Comment("executing step \'call EstablishConnection(\"A\",1,FRS_COMMUNICATION_PROTOCOL_VERSION" +
                    "_LONGHORN_SERVER,out _,out _)\'");
            temp167 = this.IFRS2ManagedAdapterInstance.EstablishConnection("A", 1, FRS2Model.ProtocolVersion.FRS_COMMUNICATION_PROTOCOL_VERSION_LONGHORN_SERVER, out temp165, out temp166);
            this.Manager.Checkpoint("MS-FRS2_R37");
            this.Manager.Checkpoint("MS-FRS2_R436");
            this.Manager.Checkpoint("MS-FRS2_R439");
            this.Manager.Comment("reaching state \'S34\'");
            this.Manager.Comment("checking step \'return EstablishConnection/[out Valid,out Valid]:ERROR_SUCCESS\'");
            this.Manager.Assert((temp165 == FRS2Model.ProtocolVersionReturned.Valid), String.Format("expected \'FRS2Model.ProtocolVersionReturned.Valid\', actual \'{0}\' (upstreamProtoco" +
                        "lVersion of EstablishConnection, state S34)", TestManagerHelpers.Describe(temp165)));
            this.Manager.Assert((temp166 == FRS2Model.UpstreamFlagValueReturned.Valid), String.Format("expected \'FRS2Model.UpstreamFlagValueReturned.Valid\', actual \'{0}\' (upstreamFlags" +
                        " of EstablishConnection, state S34)", TestManagerHelpers.Describe(temp166)));
            this.Manager.Assert((temp167 == FRS2Model.error_status_t.ERROR_SUCCESS), String.Format("expected \'FRS2Model.error_status_t.ERROR_SUCCESS\', actual \'{0}\' (return of Establ" +
                        "ishConnection, state S34)", TestManagerHelpers.Describe(temp167)));
            this.Manager.Comment("reaching state \'S43\'");
            FRS2Model.error_status_t temp168;
            this.Manager.Comment("executing step \'call EstablishSession(1,1)\'");
            temp168 = this.IFRS2ManagedAdapterInstance.EstablishSession(1, 1);
            this.Manager.Checkpoint("MS-FRS2_R455");
            this.Manager.Checkpoint("MS-FRS2_R458");
            this.Manager.Checkpoint("MS-FRS2_R448");
            this.Manager.Comment("reaching state \'S52\'");
            this.Manager.Comment("checking step \'return EstablishSession/ERROR_SUCCESS\'");
            this.Manager.Assert((temp168 == FRS2Model.error_status_t.ERROR_SUCCESS), String.Format("expected \'FRS2Model.error_status_t.ERROR_SUCCESS\', actual \'{0}\' (return of Establ" +
                        "ishSession, state S52)", TestManagerHelpers.Describe(temp168)));
            this.Manager.Comment("reaching state \'S61\'");
            FRS2Model.error_status_t temp169;
            this.Manager.Comment("executing step \'call AsyncPoll(1)\'");
            temp169 = this.IFRS2ManagedAdapterInstance.AsyncPoll(1);
            this.Manager.Checkpoint("MS-FRS2_R549");
            this.Manager.Checkpoint("MS-FRS2_R552");
            this.Manager.Comment("reaching state \'S70\'");
            this.Manager.Comment("checking step \'return AsyncPoll/ERROR_SUCCESS\'");
            this.Manager.Assert((temp169 == FRS2Model.error_status_t.ERROR_SUCCESS), String.Format("expected \'FRS2Model.error_status_t.ERROR_SUCCESS\', actual \'{0}\' (return of AsyncP" +
                        "oll, state S70)", TestManagerHelpers.Describe(temp169)));
            this.Manager.Comment("reaching state \'S78\'");
            FRS2Model.error_status_t temp170;
            this.Manager.Comment("executing step \'call RequestVersionVector(1,1,1,REQUEST_NORMAL_SYNC,CHANGE_ALL,Va" +
                    "lidValue)\'");
            temp170 = this.IFRS2ManagedAdapterInstance.RequestVersionVector(1, 1, 1, FRS2Model.VERSION_REQUEST_TYPE.REQUEST_NORMAL_SYNC, FRS2Model.VERSION_CHANGE_TYPE.CHANGE_ALL, FRS2Model.VVGeneration.ValidValue);
            this.Manager.Checkpoint("MS-FRS2_R517");
            this.Manager.Checkpoint("MS-FRS2_R520");
            this.Manager.Checkpoint("MS-FRS2_R466");
            this.Manager.Comment("reaching state \'S86\'");
            this.Manager.Comment("checking step \'return RequestVersionVector/ERROR_SUCCESS\'");
            this.Manager.Assert((temp170 == FRS2Model.error_status_t.ERROR_SUCCESS), String.Format("expected \'FRS2Model.error_status_t.ERROR_SUCCESS\', actual \'{0}\' (return of Reques" +
                        "tVersionVector, state S86)", TestManagerHelpers.Describe(temp170)));
            this.Manager.Comment("reaching state \'S94\'");
            int temp172 = this.Manager.ExpectEvent(this.QuiescenceTimeout, true, new ExpectedEvent(TCtestScenario3.AsyncPollResponseEventInfo, null, new AsyncPollResponseEventDelegate1(this.TCtestScenario3S14AsyncPollResponseEventChecker)), new ExpectedEvent(TCtestScenario3.AsyncPollResponseEventInfo, null, new AsyncPollResponseEventDelegate1(this.TCtestScenario3S14AsyncPollResponseEventChecker1)));
            if ((temp172 == 0)) {
                this.Manager.Comment("reaching state \'S109\'");
                FRS2Model.error_status_t temp171;
                this.Manager.Comment("executing step \'call RequestUpdates(1,1,inValid)\'");
                temp171 = this.IFRS2ManagedAdapterInstance.RequestUpdates(1, 1, FRS2Model.versionVectorDiff.inValid);
                this.Manager.Checkpoint("MS-FRS2_R487");
                this.Manager.Checkpoint("MS-FRS2_R494");
                this.Manager.Comment("reaching state \'S118\'");
                this.Manager.Comment("checking step \'return RequestUpdates/ERROR_FAIL\'");
                this.Manager.Assert((temp171 == FRS2Model.error_status_t.ERROR_FAIL), String.Format("expected \'FRS2Model.error_status_t.ERROR_FAIL\', actual \'{0}\' (return of RequestUp" +
                            "dates, state S118)", TestManagerHelpers.Describe(temp171)));
                this.Manager.Comment("reaching state \'S126\'");
                goto label32;
            }
            if ((temp172 == 1)) {
                TCtestScenario3S96();
                goto label32;
            }
            throw new InvalidOperationException("never reached");
        label32:
;
            this.Manager.EndTest();
        }
        
        private void TCtestScenario3S14AsyncPollResponseEventChecker(FRS2Model.VVGeneration vvGen) {
            this.Manager.Comment("checking step \'event AsyncPollResponseEvent(ValidValue)\'");
            try {
                this.Manager.Assert((vvGen == FRS2Model.VVGeneration.ValidValue), String.Format("expected \'FRS2Model.VVGeneration.ValidValue\', actual \'{0}\' (vvGen of AsyncPollRes" +
                            "ponseEvent, state S94)", TestManagerHelpers.Describe(vvGen)));
            }
            catch (TransactionFailedException ) {
                this.Manager.Comment("This step would have covered MS-FRS2_R556, MS-FRS2_R1020, MS-FRS2_R555");
                throw;
            }
            this.Manager.Checkpoint("MS-FRS2_R556");
            this.Manager.Checkpoint("MS-FRS2_R1020");
            this.Manager.Checkpoint("MS-FRS2_R555");
        }
        
        private void TCtestScenario3S14AsyncPollResponseEventChecker1(FRS2Model.VVGeneration vvGen) {
            this.Manager.Comment("checking step \'event AsyncPollResponseEvent(InvalidValue)\'");
            try {
                this.Manager.Assert((vvGen == FRS2Model.VVGeneration.InvalidValue), String.Format("expected \'FRS2Model.VVGeneration.InvalidValue\', actual \'{0}\' (vvGen of AsyncPollR" +
                            "esponseEvent, state S94)", TestManagerHelpers.Describe(vvGen)));
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
        
        #region Test Starting in S16
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        [Microsoft.VisualStudio.TestTools.UnitTesting.DescriptionAttribute("MS-FRS2_R37, MS-FRS2_R436, MS-FRS2_R439, MS-FRS2_R549, MS-FRS2_R552")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestCategory("AD")]
        [TestCategory("MS-FRS2")]
        [TestCategory("SDC")]
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        
        public virtual void FRS2_TCtestScenario3S16()
        {
            this.Manager.BeginTest("TCtestScenario3S16");
            this.Manager.Comment("reaching state \'S16\'");
            FRS2Model.error_status_t temp173;
            this.Manager.Comment(@"executing step 'call Initialization(Windows7,Map{1=>enabled,2=>enabled,3=>enabled,4=>disabled},Map{1=>exists,2=>exists,3=>exists,4=>exists},Map{""A""=>1,""B""=>2,""C""=>3,""D""=>4},Map{""A""=>sysvol,""B""=>protection,""C""=>sysvol,""D""=>sysvol},Map{1=>""A"",2=>""B"",3=>""C"",4=>""D""},Map{1=>writeOnly,2=>readWrite,3=>readOnly,4=>readWrite},Map{1=>enabled,2=>disabled,3=>enabled,4=>enabled})'");
            temp173 = this.IFRS2ManagedAdapterInstance.Initialization(FRS2Model.OSVersion.Windows7, new Microsoft.Modeling.Map<System.Int32,FRS2Model.connectionProperty>().Add(1, FRS2Model.connectionProperty.enabled).Add(2, FRS2Model.connectionProperty.enabled).Add(3, FRS2Model.connectionProperty.enabled).Add(4, FRS2Model.connectionProperty.disabled), new Microsoft.Modeling.Map<System.Int32,FRS2Model.FromServer>().Add(1, FRS2Model.FromServer.exists).Add(2, FRS2Model.FromServer.exists).Add(3, FRS2Model.FromServer.exists).Add(4, FRS2Model.FromServer.exists), new Microsoft.Modeling.Map<System.String,System.Int32>().Add("A", 1).Add("B", 2).Add("C", 3).Add("D", 4), new Microsoft.Modeling.Map<System.String,FRS2Model.ReplicationGroupTypes>().Add("A", FRS2Model.ReplicationGroupTypes.sysvol).Add("B", FRS2Model.ReplicationGroupTypes.protection).Add("C", FRS2Model.ReplicationGroupTypes.sysvol).Add("D", FRS2Model.ReplicationGroupTypes.sysvol), new Microsoft.Modeling.Map<System.Int32,System.String>().Add(1, "A").Add(2, "B").Add(3, "C").Add(4, "D"), new Microsoft.Modeling.Map<System.Int32,FRS2Model.accessLevels>().Add(1, FRS2Model.accessLevels.writeOnly).Add(2, FRS2Model.accessLevels.readWrite).Add(3, FRS2Model.accessLevels.readOnly).Add(4, FRS2Model.accessLevels.readWrite), new Microsoft.Modeling.Map<System.Int32,FRS2Model.connectionProperty>().Add(1, FRS2Model.connectionProperty.enabled).Add(2, FRS2Model.connectionProperty.disabled).Add(3, FRS2Model.connectionProperty.enabled).Add(4, FRS2Model.connectionProperty.enabled));
            this.Manager.Comment("reaching state \'S17\'");
            this.Manager.Comment("checking step \'return Initialization/ERROR_SUCCESS\'");
            this.Manager.Assert((temp173 == FRS2Model.error_status_t.ERROR_SUCCESS), String.Format("expected \'FRS2Model.error_status_t.ERROR_SUCCESS\', actual \'{0}\' (return of Initia" +
                        "lization, state S17)", TestManagerHelpers.Describe(temp173)));
            this.Manager.Comment("reaching state \'S26\'");
            FRS2Model.ProtocolVersionReturned temp174;
            FRS2Model.UpstreamFlagValueReturned temp175;
            FRS2Model.error_status_t temp176;
            this.Manager.Comment("executing step \'call EstablishConnection(\"A\",1,FRS_COMMUNICATION_PROTOCOL_VERSION" +
                    "_LONGHORN_SERVER,out _,out _)\'");
            temp176 = this.IFRS2ManagedAdapterInstance.EstablishConnection("A", 1, FRS2Model.ProtocolVersion.FRS_COMMUNICATION_PROTOCOL_VERSION_LONGHORN_SERVER, out temp174, out temp175);
            this.Manager.Checkpoint("MS-FRS2_R37");
            this.Manager.Checkpoint("MS-FRS2_R436");
            this.Manager.Checkpoint("MS-FRS2_R439");
            this.Manager.Comment("reaching state \'S35\'");
            this.Manager.Comment("checking step \'return EstablishConnection/[out Valid,out Valid]:ERROR_SUCCESS\'");
            this.Manager.Assert((temp174 == FRS2Model.ProtocolVersionReturned.Valid), String.Format("expected \'FRS2Model.ProtocolVersionReturned.Valid\', actual \'{0}\' (upstreamProtoco" +
                        "lVersion of EstablishConnection, state S35)", TestManagerHelpers.Describe(temp174)));
            this.Manager.Assert((temp175 == FRS2Model.UpstreamFlagValueReturned.Valid), String.Format("expected \'FRS2Model.UpstreamFlagValueReturned.Valid\', actual \'{0}\' (upstreamFlags" +
                        " of EstablishConnection, state S35)", TestManagerHelpers.Describe(temp175)));
            this.Manager.Assert((temp176 == FRS2Model.error_status_t.ERROR_SUCCESS), String.Format("expected \'FRS2Model.error_status_t.ERROR_SUCCESS\', actual \'{0}\' (return of Establ" +
                        "ishConnection, state S35)", TestManagerHelpers.Describe(temp176)));
            this.Manager.Comment("reaching state \'S44\'");
            FRS2Model.error_status_t temp177;
            this.Manager.Comment("executing step \'call AsyncPoll(1)\'");
            temp177 = this.IFRS2ManagedAdapterInstance.AsyncPoll(1);
            this.Manager.Checkpoint("MS-FRS2_R549");
            this.Manager.Checkpoint("MS-FRS2_R552");
            this.Manager.Comment("reaching state \'S53\'");
            this.Manager.Comment("checking step \'return AsyncPoll/ERROR_SUCCESS\'");
            this.Manager.Assert((temp177 == FRS2Model.error_status_t.ERROR_SUCCESS), String.Format("expected \'FRS2Model.error_status_t.ERROR_SUCCESS\', actual \'{0}\' (return of AsyncP" +
                        "oll, state S53)", TestManagerHelpers.Describe(temp177)));
            this.Manager.Comment("reaching state \'S62\'");
            this.Manager.EndTest();
        }
        #endregion
    }
}
