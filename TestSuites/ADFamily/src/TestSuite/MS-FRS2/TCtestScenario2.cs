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
    public partial class TCtestScenario2 : PtfTestClassBase {
        
        public TCtestScenario2() {
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
        
        public delegate void RawGetFileDataAsyncDelegate1(FRS2Model.error_status_t @return);
        
        public delegate void RdcCloseDelegate1(FRS2Model.error_status_t @return);
        
        public delegate void CheckConnectivityDelegate1(FRS2Model.error_status_t @return);
        
        public delegate void RequestRecordsDelegate1(FRS2Model.error_status_t @return);
        
        public delegate void UpdateCancelDelegate1(FRS2Model.error_status_t @return);
        
        public delegate void RawGetFileDataDelegate1(FRS2Model.error_status_t @return);
        
        public delegate void RdcGetSignaturesDelegate1(FRS2Model.error_status_t @return);
        
        public delegate void RdcPushSourceNeedsDelegate1(FRS2Model.error_status_t @return);
        
        public delegate void RdcGetFileDataDelegate1(FRS2Model.error_status_t @return);
        
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
        
        static System.Reflection.MethodBase RawGetFileDataAsyncInfo = TestManagerHelpers.GetMethodInfo(typeof(Microsoft.Protocols.TestSuites.MS_FRS2.IFRS2ManagedAdapter), "RawGetFileDataAsync");
        
        static System.Reflection.MethodBase RdcCloseInfo = TestManagerHelpers.GetMethodInfo(typeof(Microsoft.Protocols.TestSuites.MS_FRS2.IFRS2ManagedAdapter), "RdcClose");
        
        static System.Reflection.MethodBase CheckConnectivityInfo = TestManagerHelpers.GetMethodInfo(typeof(Microsoft.Protocols.TestSuites.MS_FRS2.IFRS2ManagedAdapter), "CheckConnectivity", typeof(string), typeof(int));
        
        static System.Reflection.MethodBase RequestRecordsInfo = TestManagerHelpers.GetMethodInfo(typeof(Microsoft.Protocols.TestSuites.MS_FRS2.IFRS2ManagedAdapter), "RequestRecords", typeof(int), typeof(int));
        
        static System.Reflection.MethodBase UpdateCancelInfo = TestManagerHelpers.GetMethodInfo(typeof(Microsoft.Protocols.TestSuites.MS_FRS2.IFRS2ManagedAdapter), "UpdateCancel", typeof(int), typeof(FRS2Model.FRS_UPDATE_CANCEL_DATA), typeof(int));
        
        static System.Reflection.MethodBase RawGetFileDataInfo = TestManagerHelpers.GetMethodInfo(typeof(Microsoft.Protocols.TestSuites.MS_FRS2.IFRS2ManagedAdapter), "RawGetFileData");
        
        static System.Reflection.MethodBase RdcGetSignaturesInfo = TestManagerHelpers.GetMethodInfo(typeof(Microsoft.Protocols.TestSuites.MS_FRS2.IFRS2ManagedAdapter), "RdcGetSignatures", typeof(FRS2Model.offset));
        
        static System.Reflection.MethodBase RdcPushSourceNeedsInfo = TestManagerHelpers.GetMethodInfo(typeof(Microsoft.Protocols.TestSuites.MS_FRS2.IFRS2ManagedAdapter), "RdcPushSourceNeeds");
        
        static System.Reflection.MethodBase RdcGetFileDataInfo = TestManagerHelpers.GetMethodInfo(typeof(Microsoft.Protocols.TestSuites.MS_FRS2.IFRS2ManagedAdapter), "RdcGetFileData", typeof(FRS2Model.BufferSize));
        
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
            ", MS-FRS2_R518, MS-FRS2_R522, MS-FRS2_R523, MS-FRS2_R530")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestCategory("AD")]
        [TestCategory("MS-FRS2")]
        [TestCategory("SDC")]
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        
        public virtual void FRS2_TCtestScenario2S0()
        {
            this.Manager.BeginTest("TCtestScenario2S0");
            this.Manager.Comment("reaching state \'S0\'");
            FRS2Model.error_status_t temp0;
            this.Manager.Comment(@"executing step 'call Initialization(Windows7,Map{1=>enabled,2=>enabled,3=>enabled,4=>disabled},Map{1=>exists,2=>exists,3=>exists,4=>exists},Map{""A""=>1,""B""=>2,""C""=>3,""D""=>4},Map{""A""=>sysvol,""B""=>protection,""C""=>sysvol,""D""=>sysvol},Map{1=>""A"",2=>""B"",3=>""C"",4=>""D""},Map{1=>writeOnly,2=>readWrite,3=>readOnly,4=>readWrite},Map{1=>enabled,2=>disabled,3=>enabled,4=>enabled})'");
            temp0 = this.IFRS2ManagedAdapterInstance.Initialization(FRS2Model.OSVersion.Windows7, new Microsoft.Modeling.Map<System.Int32,FRS2Model.connectionProperty>().Add(1, FRS2Model.connectionProperty.enabled).Add(2, FRS2Model.connectionProperty.enabled).Add(3, FRS2Model.connectionProperty.enabled).Add(4, FRS2Model.connectionProperty.disabled), new Microsoft.Modeling.Map<System.Int32,FRS2Model.FromServer>().Add(1, FRS2Model.FromServer.exists).Add(2, FRS2Model.FromServer.exists).Add(3, FRS2Model.FromServer.exists).Add(4, FRS2Model.FromServer.exists), new Microsoft.Modeling.Map<System.String,System.Int32>().Add("A", 1).Add("B", 2).Add("C", 3).Add("D", 4), new Microsoft.Modeling.Map<System.String,FRS2Model.ReplicationGroupTypes>().Add("A", FRS2Model.ReplicationGroupTypes.sysvol).Add("B", FRS2Model.ReplicationGroupTypes.protection).Add("C", FRS2Model.ReplicationGroupTypes.sysvol).Add("D", FRS2Model.ReplicationGroupTypes.sysvol), new Microsoft.Modeling.Map<System.Int32,System.String>().Add(1, "A").Add(2, "B").Add(3, "C").Add(4, "D"), new Microsoft.Modeling.Map<System.Int32,FRS2Model.accessLevels>().Add(1, FRS2Model.accessLevels.writeOnly).Add(2, FRS2Model.accessLevels.readWrite).Add(3, FRS2Model.accessLevels.readOnly).Add(4, FRS2Model.accessLevels.readWrite), new Microsoft.Modeling.Map<System.Int32,FRS2Model.connectionProperty>().Add(1, FRS2Model.connectionProperty.enabled).Add(2, FRS2Model.connectionProperty.disabled).Add(3, FRS2Model.connectionProperty.enabled).Add(4, FRS2Model.connectionProperty.enabled));
            this.Manager.Comment("reaching state \'S1\'");
            this.Manager.Comment("checking step \'return Initialization/ERROR_SUCCESS\'");
            this.Manager.Assert((temp0 == FRS2Model.error_status_t.ERROR_SUCCESS), String.Format("expected \'FRS2Model.error_status_t.ERROR_SUCCESS\', actual \'{0}\' (return of Initia" +
                        "lization, state S1)", TestManagerHelpers.Describe(temp0)));
            this.Manager.Comment("reaching state \'S26\'");
            FRS2Model.ProtocolVersionReturned temp1;
            FRS2Model.UpstreamFlagValueReturned temp2;
            FRS2Model.error_status_t temp3;
            this.Manager.Comment("executing step \'call EstablishConnection(\"A\",1,FRS_COMMUNICATION_PROTOCOL_VERSION" +
                    "_LONGHORN_SERVER,out _,out _)\'");
            temp3 = this.IFRS2ManagedAdapterInstance.EstablishConnection("A", 1, FRS2Model.ProtocolVersion.FRS_COMMUNICATION_PROTOCOL_VERSION_LONGHORN_SERVER, out temp1, out temp2);
            this.Manager.Checkpoint("MS-FRS2_R37");
            this.Manager.Checkpoint("MS-FRS2_R436");
            this.Manager.Checkpoint("MS-FRS2_R439");
            this.Manager.Comment("reaching state \'S39\'");
            this.Manager.Comment("checking step \'return EstablishConnection/[out Valid,out Valid]:ERROR_SUCCESS\'");
            this.Manager.Assert((temp1 == FRS2Model.ProtocolVersionReturned.Valid), String.Format("expected \'FRS2Model.ProtocolVersionReturned.Valid\', actual \'{0}\' (upstreamProtoco" +
                        "lVersion of EstablishConnection, state S39)", TestManagerHelpers.Describe(temp1)));
            this.Manager.Assert((temp2 == FRS2Model.UpstreamFlagValueReturned.Valid), String.Format("expected \'FRS2Model.UpstreamFlagValueReturned.Valid\', actual \'{0}\' (upstreamFlags" +
                        " of EstablishConnection, state S39)", TestManagerHelpers.Describe(temp2)));
            this.Manager.Assert((temp3 == FRS2Model.error_status_t.ERROR_SUCCESS), String.Format("expected \'FRS2Model.error_status_t.ERROR_SUCCESS\', actual \'{0}\' (return of Establ" +
                        "ishConnection, state S39)", TestManagerHelpers.Describe(temp3)));
            this.Manager.Comment("reaching state \'S52\'");
            FRS2Model.error_status_t temp4;
            this.Manager.Comment("executing step \'call EstablishSession(1,1)\'");
            temp4 = this.IFRS2ManagedAdapterInstance.EstablishSession(1, 1);
            this.Manager.Checkpoint("MS-FRS2_R455");
            this.Manager.Checkpoint("MS-FRS2_R458");
            this.Manager.Checkpoint("MS-FRS2_R448");
            this.Manager.Comment("reaching state \'S65\'");
            this.Manager.Comment("checking step \'return EstablishSession/ERROR_SUCCESS\'");
            this.Manager.Assert((temp4 == FRS2Model.error_status_t.ERROR_SUCCESS), String.Format("expected \'FRS2Model.error_status_t.ERROR_SUCCESS\', actual \'{0}\' (return of Establ" +
                        "ishSession, state S65)", TestManagerHelpers.Describe(temp4)));
            this.Manager.Comment("reaching state \'S78\'");
            FRS2Model.error_status_t temp5;
            this.Manager.Comment("executing step \'call RequestVersionVector(1,4,5,REQUEST_NORMAL_SYNC,CHANGE_NOTIFY" +
                    ",ValidValue)\'");
            temp5 = this.IFRS2ManagedAdapterInstance.RequestVersionVector(1, 4, 5, FRS2Model.VERSION_REQUEST_TYPE.REQUEST_NORMAL_SYNC, FRS2Model.VERSION_CHANGE_TYPE.CHANGE_NOTIFY, FRS2Model.VVGeneration.ValidValue);
            this.Manager.Checkpoint("MS-FRS2_R518");
            this.Manager.Checkpoint("MS-FRS2_R522");
            this.Manager.Checkpoint("MS-FRS2_R523");
            this.Manager.Checkpoint("MS-FRS2_R530");
            this.Manager.Comment("reaching state \'S91\'");
            this.Manager.Comment("checking step \'return RequestVersionVector/ERROR_FAIL\'");
            this.Manager.Assert((temp5 == FRS2Model.error_status_t.ERROR_FAIL), String.Format("expected \'FRS2Model.error_status_t.ERROR_FAIL\', actual \'{0}\' (return of RequestVe" +
                        "rsionVector, state S91)", TestManagerHelpers.Describe(temp5)));
            TCtestScenario2S103();
            this.Manager.EndTest();
        }
        
        private void TCtestScenario2S103() {
            this.Manager.Comment("reaching state \'S103\'");
        }
        #endregion
        
        #region Test Starting in S2
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        [Microsoft.VisualStudio.TestTools.UnitTesting.DescriptionAttribute(@"MS-FRS2_R37, MS-FRS2_R436, MS-FRS2_R439, MS-FRS2_R455, MS-FRS2_R458, MS-FRS2_R448, MS-FRS2_R549, MS-FRS2_R552, MS-FRS2_R517, MS-FRS2_R520, MS-FRS2_R466, MS-FRS2_R556, MS-FRS2_R1020, MS-FRS2_R555, MS-FRS2_R486, MS-FRS2_R489, MS-FRS2_R93, MS-FRS2_R778, MS-FRS2_R783, MS-FRS2_R93, MS-FRS2_R94, MS-FRS2_R778, MS-FRS2_R783, MS-FRS2_R93, MS-FRS2_R94, MS-FRS2_R778, MS-FRS2_R783, MS-FRS2_R93, MS-FRS2_R498, MS-FRS2_R778, MS-FRS2_R783, MS-FRS2_R556, MS-FRS2_R1020, MS-FRS2_R555")]
        public virtual void FRS2_TCtestScenario2S2() {
            this.Manager.BeginTest("TCtestScenario2S2");
            this.Manager.Comment("reaching state \'S2\'");
            FRS2Model.error_status_t temp6;
            this.Manager.Comment(@"executing step 'call Initialization(Windows7,Map{1=>enabled,2=>enabled,3=>enabled,4=>disabled},Map{1=>exists,2=>exists,3=>exists,4=>exists},Map{""A""=>1,""B""=>2,""C""=>3,""D""=>4},Map{""A""=>sysvol,""B""=>protection,""C""=>sysvol,""D""=>sysvol},Map{1=>""A"",2=>""B"",3=>""C"",4=>""D""},Map{1=>writeOnly,2=>readWrite,3=>readOnly,4=>readWrite},Map{1=>enabled,2=>disabled,3=>enabled,4=>enabled})'");
            temp6 = this.IFRS2ManagedAdapterInstance.Initialization(FRS2Model.OSVersion.Windows7, new Microsoft.Modeling.Map<System.Int32,FRS2Model.connectionProperty>().Add(1, FRS2Model.connectionProperty.enabled).Add(2, FRS2Model.connectionProperty.enabled).Add(3, FRS2Model.connectionProperty.enabled).Add(4, FRS2Model.connectionProperty.disabled), new Microsoft.Modeling.Map<System.Int32,FRS2Model.FromServer>().Add(1, FRS2Model.FromServer.exists).Add(2, FRS2Model.FromServer.exists).Add(3, FRS2Model.FromServer.exists).Add(4, FRS2Model.FromServer.exists), new Microsoft.Modeling.Map<System.String,System.Int32>().Add("A", 1).Add("B", 2).Add("C", 3).Add("D", 4), new Microsoft.Modeling.Map<System.String,FRS2Model.ReplicationGroupTypes>().Add("A", FRS2Model.ReplicationGroupTypes.sysvol).Add("B", FRS2Model.ReplicationGroupTypes.protection).Add("C", FRS2Model.ReplicationGroupTypes.sysvol).Add("D", FRS2Model.ReplicationGroupTypes.sysvol), new Microsoft.Modeling.Map<System.Int32,System.String>().Add(1, "A").Add(2, "B").Add(3, "C").Add(4, "D"), new Microsoft.Modeling.Map<System.Int32,FRS2Model.accessLevels>().Add(1, FRS2Model.accessLevels.writeOnly).Add(2, FRS2Model.accessLevels.readWrite).Add(3, FRS2Model.accessLevels.readOnly).Add(4, FRS2Model.accessLevels.readWrite), new Microsoft.Modeling.Map<System.Int32,FRS2Model.connectionProperty>().Add(1, FRS2Model.connectionProperty.enabled).Add(2, FRS2Model.connectionProperty.disabled).Add(3, FRS2Model.connectionProperty.enabled).Add(4, FRS2Model.connectionProperty.enabled));
            this.Manager.Comment("reaching state \'S3\'");
            this.Manager.Comment("checking step \'return Initialization/ERROR_SUCCESS\'");
            this.Manager.Assert((temp6 == FRS2Model.error_status_t.ERROR_SUCCESS), String.Format("expected \'FRS2Model.error_status_t.ERROR_SUCCESS\', actual \'{0}\' (return of Initia" +
                        "lization, state S3)", TestManagerHelpers.Describe(temp6)));
            this.Manager.Comment("reaching state \'S27\'");
            FRS2Model.ProtocolVersionReturned temp7;
            FRS2Model.UpstreamFlagValueReturned temp8;
            FRS2Model.error_status_t temp9;
            this.Manager.Comment("executing step \'call EstablishConnection(\"A\",1,FRS_COMMUNICATION_PROTOCOL_VERSION" +
                    "_LONGHORN_SERVER,out _,out _)\'");
            temp9 = this.IFRS2ManagedAdapterInstance.EstablishConnection("A", 1, FRS2Model.ProtocolVersion.FRS_COMMUNICATION_PROTOCOL_VERSION_LONGHORN_SERVER, out temp7, out temp8);
            this.Manager.Checkpoint("MS-FRS2_R37");
            this.Manager.Checkpoint("MS-FRS2_R436");
            this.Manager.Checkpoint("MS-FRS2_R439");
            this.Manager.Comment("reaching state \'S40\'");
            this.Manager.Comment("checking step \'return EstablishConnection/[out Valid,out Valid]:ERROR_SUCCESS\'");
            this.Manager.Assert((temp7 == FRS2Model.ProtocolVersionReturned.Valid), String.Format("expected \'FRS2Model.ProtocolVersionReturned.Valid\', actual \'{0}\' (upstreamProtoco" +
                        "lVersion of EstablishConnection, state S40)", TestManagerHelpers.Describe(temp7)));
            this.Manager.Assert((temp8 == FRS2Model.UpstreamFlagValueReturned.Valid), String.Format("expected \'FRS2Model.UpstreamFlagValueReturned.Valid\', actual \'{0}\' (upstreamFlags" +
                        " of EstablishConnection, state S40)", TestManagerHelpers.Describe(temp8)));
            this.Manager.Assert((temp9 == FRS2Model.error_status_t.ERROR_SUCCESS), String.Format("expected \'FRS2Model.error_status_t.ERROR_SUCCESS\', actual \'{0}\' (return of Establ" +
                        "ishConnection, state S40)", TestManagerHelpers.Describe(temp9)));
            this.Manager.Comment("reaching state \'S53\'");
            FRS2Model.error_status_t temp10;
            this.Manager.Comment("executing step \'call EstablishSession(1,1)\'");
            temp10 = this.IFRS2ManagedAdapterInstance.EstablishSession(1, 1);
            this.Manager.Checkpoint("MS-FRS2_R455");
            this.Manager.Checkpoint("MS-FRS2_R458");
            this.Manager.Checkpoint("MS-FRS2_R448");
            this.Manager.Comment("reaching state \'S66\'");
            this.Manager.Comment("checking step \'return EstablishSession/ERROR_SUCCESS\'");
            this.Manager.Assert((temp10 == FRS2Model.error_status_t.ERROR_SUCCESS), String.Format("expected \'FRS2Model.error_status_t.ERROR_SUCCESS\', actual \'{0}\' (return of Establ" +
                        "ishSession, state S66)", TestManagerHelpers.Describe(temp10)));
            this.Manager.Comment("reaching state \'S79\'");
            FRS2Model.error_status_t temp11;
            this.Manager.Comment("executing step \'call AsyncPoll(1)\'");
            temp11 = this.IFRS2ManagedAdapterInstance.AsyncPoll(1);
            this.Manager.Checkpoint("MS-FRS2_R549");
            this.Manager.Checkpoint("MS-FRS2_R552");
            this.Manager.Comment("reaching state \'S92\'");
            this.Manager.Comment("checking step \'return AsyncPoll/ERROR_SUCCESS\'");
            this.Manager.Assert((temp11 == FRS2Model.error_status_t.ERROR_SUCCESS), String.Format("expected \'FRS2Model.error_status_t.ERROR_SUCCESS\', actual \'{0}\' (return of AsyncP" +
                        "oll, state S92)", TestManagerHelpers.Describe(temp11)));
            this.Manager.Comment("reaching state \'S104\'");
            FRS2Model.error_status_t temp12;
            this.Manager.Comment("executing step \'call RequestVersionVector(1,1,1,REQUEST_NORMAL_SYNC,CHANGE_NOTIFY" +
                    ",ValidValue)\'");
            temp12 = this.IFRS2ManagedAdapterInstance.RequestVersionVector(1, 1, 1, FRS2Model.VERSION_REQUEST_TYPE.REQUEST_NORMAL_SYNC, FRS2Model.VERSION_CHANGE_TYPE.CHANGE_NOTIFY, FRS2Model.VVGeneration.ValidValue);
            this.Manager.Checkpoint("MS-FRS2_R517");
            this.Manager.Checkpoint("MS-FRS2_R520");
            this.Manager.Checkpoint("MS-FRS2_R466");
            this.Manager.Comment("reaching state \'S115\'");
            this.Manager.Comment("checking step \'return RequestVersionVector/ERROR_SUCCESS\'");
            this.Manager.Assert((temp12 == FRS2Model.error_status_t.ERROR_SUCCESS), String.Format("expected \'FRS2Model.error_status_t.ERROR_SUCCESS\', actual \'{0}\' (return of Reques" +
                        "tVersionVector, state S115)", TestManagerHelpers.Describe(temp12)));
            this.Manager.Comment("reaching state \'S123\'");
            int temp19 = this.Manager.ExpectEvent(this.QuiescenceTimeout, true, new ExpectedEvent(TCtestScenario2.AsyncPollResponseEventInfo, null, new AsyncPollResponseEventDelegate1(this.TCtestScenario2S2AsyncPollResponseEventChecker)), new ExpectedEvent(TCtestScenario2.AsyncPollResponseEventInfo, null, new AsyncPollResponseEventDelegate1(this.TCtestScenario2S2AsyncPollResponseEventChecker1)));
            if ((temp19 == 0)) {
                this.Manager.Comment("reaching state \'S131\'");
                FRS2Model.error_status_t temp13;
                this.Manager.Comment("executing step \'call RequestUpdates(1,1,valid)\'");
                temp13 = this.IFRS2ManagedAdapterInstance.RequestUpdates(1, 1, FRS2Model.versionVectorDiff.valid);
                this.Manager.Checkpoint("MS-FRS2_R486");
                this.Manager.Checkpoint("MS-FRS2_R489");
                this.Manager.Comment("reaching state \'S139\'");
                this.Manager.Comment("checking step \'return RequestUpdates/ERROR_SUCCESS\'");
                this.Manager.Assert((temp13 == FRS2Model.error_status_t.ERROR_SUCCESS), String.Format("expected \'FRS2Model.error_status_t.ERROR_SUCCESS\', actual \'{0}\' (return of Reques" +
                            "tUpdates, state S139)", TestManagerHelpers.Describe(temp13)));
                this.Manager.Comment("reaching state \'S143\'");
                int temp18 = this.Manager.ExpectEvent(this.QuiescenceTimeout, true, new ExpectedEvent(TCtestScenario2.RequestUpdatesEventInfo, null, new RequestUpdatesEventDelegate1(this.TCtestScenario2S2RequestUpdatesEventChecker)), new ExpectedEvent(TCtestScenario2.RequestUpdatesEventInfo, null, new RequestUpdatesEventDelegate1(this.TCtestScenario2S2RequestUpdatesEventChecker1)), new ExpectedEvent(TCtestScenario2.RequestUpdatesEventInfo, null, new RequestUpdatesEventDelegate1(this.TCtestScenario2S2RequestUpdatesEventChecker2)), new ExpectedEvent(TCtestScenario2.RequestUpdatesEventInfo, null, new RequestUpdatesEventDelegate1(this.TCtestScenario2S2RequestUpdatesEventChecker3)));
                if ((temp18 == 0)) {
                    this.Manager.Comment("reaching state \'S147\'");
                    FRS2Model.error_status_t temp14;
                    this.Manager.Comment("executing step \'call InitializeFileTransferAsync(4,1,False)\'");
                    temp14 = this.IFRS2ManagedAdapterInstance.InitializeFileTransferAsync(4, 1, false);
                    this.Manager.Checkpoint("MS-FRS2_R778");
                    this.Manager.Checkpoint("MS-FRS2_R783");
                    this.Manager.AddReturn(InitializeFileTransferAsyncInfo, null, temp14);
                    TCtestScenario2S159();
                    goto label0;
                }
                if ((temp18 == 1)) {
                    this.Manager.Comment("reaching state \'S148\'");
                    FRS2Model.error_status_t temp15;
                    this.Manager.Comment("executing step \'call InitializeFileTransferAsync(4,1,False)\'");
                    temp15 = this.IFRS2ManagedAdapterInstance.InitializeFileTransferAsync(4, 1, false);
                    this.Manager.Checkpoint("MS-FRS2_R778");
                    this.Manager.Checkpoint("MS-FRS2_R783");
                    this.Manager.AddReturn(InitializeFileTransferAsyncInfo, null, temp15);
                    TCtestScenario2S159();
                    goto label0;
                }
                if ((temp18 == 2)) {
                    this.Manager.Comment("reaching state \'S149\'");
                    FRS2Model.error_status_t temp16;
                    this.Manager.Comment("executing step \'call InitializeFileTransferAsync(4,1,False)\'");
                    temp16 = this.IFRS2ManagedAdapterInstance.InitializeFileTransferAsync(4, 1, false);
                    this.Manager.Checkpoint("MS-FRS2_R778");
                    this.Manager.Checkpoint("MS-FRS2_R783");
                    this.Manager.Comment("reaching state \'S161\'");
                    this.Manager.Comment("checking step \'return InitializeFileTransferAsync/FRS_ERROR_CONNECTION_INVALID\'");
                    this.Manager.Assert((temp16 == FRS2Model.error_status_t.FRS_ERROR_CONNECTION_INVALID), String.Format("expected \'FRS2Model.error_status_t.FRS_ERROR_CONNECTION_INVALID\', actual \'{0}\' (r" +
                                "eturn of InitializeFileTransferAsync, state S161)", TestManagerHelpers.Describe(temp16)));
                    TCtestScenario2S172();
                    goto label0;
                }
                if ((temp18 == 3)) {
                    this.Manager.Comment("reaching state \'S150\'");
                    FRS2Model.error_status_t temp17;
                    this.Manager.Comment("executing step \'call InitializeFileTransferAsync(4,1,False)\'");
                    temp17 = this.IFRS2ManagedAdapterInstance.InitializeFileTransferAsync(4, 1, false);
                    this.Manager.Checkpoint("MS-FRS2_R778");
                    this.Manager.Checkpoint("MS-FRS2_R783");
                    this.Manager.Comment("reaching state \'S162\'");
                    this.Manager.Comment("checking step \'return InitializeFileTransferAsync/FRS_ERROR_CONNECTION_INVALID\'");
                    this.Manager.Assert((temp17 == FRS2Model.error_status_t.FRS_ERROR_CONNECTION_INVALID), String.Format("expected \'FRS2Model.error_status_t.FRS_ERROR_CONNECTION_INVALID\', actual \'{0}\' (r" +
                                "eturn of InitializeFileTransferAsync, state S162)", TestManagerHelpers.Describe(temp17)));
                    TCtestScenario2S173();
                    goto label0;
                }
                throw new InvalidOperationException("never reached");
            label0:
;
                goto label1;
            }
            if ((temp19 == 1)) {
                TCtestScenario2S132();
                goto label1;
            }
            throw new InvalidOperationException("never reached");
        label1:
;
            this.Manager.EndTest();
        }
        
        private void TCtestScenario2S2AsyncPollResponseEventChecker(FRS2Model.VVGeneration vvGen) {
            this.Manager.Comment("checking step \'event AsyncPollResponseEvent(ValidValue)\'");
            try {
                this.Manager.Assert((vvGen == FRS2Model.VVGeneration.ValidValue), String.Format("expected \'FRS2Model.VVGeneration.ValidValue\', actual \'{0}\' (vvGen of AsyncPollRes" +
                            "ponseEvent, state S123)", TestManagerHelpers.Describe(vvGen)));
            }
            catch (TransactionFailedException ) {
                this.Manager.Comment("This step would have covered MS-FRS2_R556, MS-FRS2_R1020, MS-FRS2_R555");
                throw;
            }
            this.Manager.Checkpoint("MS-FRS2_R556");
            this.Manager.Checkpoint("MS-FRS2_R1020");
            this.Manager.Checkpoint("MS-FRS2_R555");
        }
        
        private void TCtestScenario2S2RequestUpdatesEventChecker(FRS2Model.FilePresense present, FRS2Model.UPDATE_STATUS updateStatus) {
            this.Manager.Comment("checking step \'event RequestUpdatesEvent(fileDeleted,UPDATE_STATUS_MORE)\'");
            try {
                this.Manager.Assert((present == FRS2Model.FilePresense.fileDeleted), String.Format("expected \'FRS2Model.FilePresense.fileDeleted\', actual \'{0}\' (present of RequestUp" +
                            "datesEvent, state S143)", TestManagerHelpers.Describe(present)));
                this.Manager.Assert((updateStatus == FRS2Model.UPDATE_STATUS.UPDATE_STATUS_MORE), String.Format("expected \'FRS2Model.UPDATE_STATUS.UPDATE_STATUS_MORE\', actual \'{0}\' (updateStatus" +
                            " of RequestUpdatesEvent, state S143)", TestManagerHelpers.Describe(updateStatus)));
            }
            catch (TransactionFailedException ) {
                this.Manager.Comment("This step would have covered MS-FRS2_R93");
                throw;
            }
            this.Manager.Checkpoint("MS-FRS2_R93");
        }
        
        private void TCtestScenario2S159() {
            this.Manager.Comment("reaching state \'S159\'");
            this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(TCtestScenario2.InitializeFileTransferAsyncInfo, null, new InitializeFileTransferAsyncDelegate1(this.TCtestScenario2S2InitializeFileTransferAsyncChecker)));
            TCtestScenario2S171();
        }
        
        private void TCtestScenario2S2InitializeFileTransferAsyncChecker(FRS2Model.error_status_t @return) {
            this.Manager.Comment("checking step \'return InitializeFileTransferAsync/FRS_ERROR_CONNECTION_INVALID\'");
            this.Manager.Assert((@return == FRS2Model.error_status_t.FRS_ERROR_CONNECTION_INVALID), String.Format("expected \'FRS2Model.error_status_t.FRS_ERROR_CONNECTION_INVALID\', actual \'{0}\' (r" +
                        "eturn of InitializeFileTransferAsync, state S159)", TestManagerHelpers.Describe(@return)));
        }
        
        private void TCtestScenario2S171() {
            this.Manager.Comment("reaching state \'S171\'");
        }
        
        private void TCtestScenario2S2RequestUpdatesEventChecker1(FRS2Model.FilePresense present, FRS2Model.UPDATE_STATUS updateStatus) {
            this.Manager.Comment("checking step \'event RequestUpdatesEvent(fileDeleted,UPDATE_STATUS_DONE)\'");
            try {
                this.Manager.Assert((present == FRS2Model.FilePresense.fileDeleted), String.Format("expected \'FRS2Model.FilePresense.fileDeleted\', actual \'{0}\' (present of RequestUp" +
                            "datesEvent, state S143)", TestManagerHelpers.Describe(present)));
                this.Manager.Assert((updateStatus == FRS2Model.UPDATE_STATUS.UPDATE_STATUS_DONE), String.Format("expected \'FRS2Model.UPDATE_STATUS.UPDATE_STATUS_DONE\', actual \'{0}\' (updateStatus" +
                            " of RequestUpdatesEvent, state S143)", TestManagerHelpers.Describe(updateStatus)));
            }
            catch (TransactionFailedException ) {
                this.Manager.Comment("This step would have covered MS-FRS2_R93, MS-FRS2_R94");
                throw;
            }
            this.Manager.Checkpoint("MS-FRS2_R93");
            this.Manager.Checkpoint("MS-FRS2_R94");
        }
        
        private void TCtestScenario2S2RequestUpdatesEventChecker2(FRS2Model.FilePresense present, FRS2Model.UPDATE_STATUS updateStatus) {
            this.Manager.Comment("checking step \'event RequestUpdatesEvent(fileExists,UPDATE_STATUS_DONE)\'");
            try {
                this.Manager.Assert((present == FRS2Model.FilePresense.fileExists), String.Format("expected \'FRS2Model.FilePresense.fileExists\', actual \'{0}\' (present of RequestUpd" +
                            "atesEvent, state S143)", TestManagerHelpers.Describe(present)));
                this.Manager.Assert((updateStatus == FRS2Model.UPDATE_STATUS.UPDATE_STATUS_DONE), String.Format("expected \'FRS2Model.UPDATE_STATUS.UPDATE_STATUS_DONE\', actual \'{0}\' (updateStatus" +
                            " of RequestUpdatesEvent, state S143)", TestManagerHelpers.Describe(updateStatus)));
            }
            catch (TransactionFailedException ) {
                this.Manager.Comment("This step would have covered MS-FRS2_R93, MS-FRS2_R94");
                throw;
            }
            this.Manager.Checkpoint("MS-FRS2_R93");
            this.Manager.Checkpoint("MS-FRS2_R94");
        }
        
        private void TCtestScenario2S172() {
            this.Manager.Comment("reaching state \'S172\'");
        }
        
        private void TCtestScenario2S2RequestUpdatesEventChecker3(FRS2Model.FilePresense present, FRS2Model.UPDATE_STATUS updateStatus) {
            this.Manager.Comment("checking step \'event RequestUpdatesEvent(fileExists,UPDATE_STATUS_MORE)\'");
            try {
                this.Manager.Assert((present == FRS2Model.FilePresense.fileExists), String.Format("expected \'FRS2Model.FilePresense.fileExists\', actual \'{0}\' (present of RequestUpd" +
                            "atesEvent, state S143)", TestManagerHelpers.Describe(present)));
                this.Manager.Assert((updateStatus == FRS2Model.UPDATE_STATUS.UPDATE_STATUS_MORE), String.Format("expected \'FRS2Model.UPDATE_STATUS.UPDATE_STATUS_MORE\', actual \'{0}\' (updateStatus" +
                            " of RequestUpdatesEvent, state S143)", TestManagerHelpers.Describe(updateStatus)));
            }
            catch (TransactionFailedException ) {
                this.Manager.Comment("This step would have covered MS-FRS2_R93, MS-FRS2_R498");
                throw;
            }
            this.Manager.Checkpoint("MS-FRS2_R93");
            this.Manager.Checkpoint("MS-FRS2_R498");
        }
        
        private void TCtestScenario2S173() {
            this.Manager.Comment("reaching state \'S173\'");
        }
        
        private void TCtestScenario2S2AsyncPollResponseEventChecker1(FRS2Model.VVGeneration vvGen) {
            this.Manager.Comment("checking step \'event AsyncPollResponseEvent(InvalidValue)\'");
            try {
                this.Manager.Assert((vvGen == FRS2Model.VVGeneration.InvalidValue), String.Format("expected \'FRS2Model.VVGeneration.InvalidValue\', actual \'{0}\' (vvGen of AsyncPollR" +
                            "esponseEvent, state S123)", TestManagerHelpers.Describe(vvGen)));
            }
            catch (TransactionFailedException ) {
                this.Manager.Comment("This step would have covered MS-FRS2_R556, MS-FRS2_R1020, MS-FRS2_R555");
                throw;
            }
            this.Manager.Checkpoint("MS-FRS2_R556");
            this.Manager.Checkpoint("MS-FRS2_R1020");
            this.Manager.Checkpoint("MS-FRS2_R555");
        }
        
        private void TCtestScenario2S132() {
            this.Manager.Comment("reaching state \'S132\'");
        }
        #endregion
        
        #region Test Starting in S4
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        [Microsoft.VisualStudio.TestTools.UnitTesting.DescriptionAttribute("MS-FRS2_R37, MS-FRS2_R436, MS-FRS2_R439, MS-FRS2_R455, MS-FRS2_R458, MS-FRS2_R448" +
            ", MS-FRS2_R549, MS-FRS2_R552, MS-FRS2_R517, MS-FRS2_R520, MS-FRS2_R466, MS-FRS2_" +
            "R556, MS-FRS2_R1020, MS-FRS2_R555, MS-FRS2_R486, MS-FRS2_R489, MS-FRS2_R93, MS-F" +
            "RS2_R775, MS-FRS2_R779, MS-FRS2_R784, MS-FRS2_R93, MS-FRS2_R94, MS-FRS2_R774, MS" +
            "-FRS2_R777, MS-FRS2_R793, MS-FRS2_R865, MS-FRS2_R868, MS-FRS2_R737, MS-FRS2_R739" +
            ", MS-FRS2_R866, MS-FRS2_R872, MS-FRS2_R873, MS-FRS2_R808, MS-FRS2_R866, MS-FRS2_" +
            "R872, MS-FRS2_R873, MS-FRS2_R808, MS-FRS2_R866, MS-FRS2_R808, MS-FRS2_R866, MS-F" +
            "RS2_R872, MS-FRS2_R873, MS-FRS2_R808, MS-FRS2_R866, MS-FRS2_R808, MS-FRS2_R866, " +
            "MS-FRS2_R872, MS-FRS2_R873, MS-FRS2_R808, MS-FRS2_R866, MS-FRS2_R808, MS-FRS2_R9" +
            "3, MS-FRS2_R94, MS-FRS2_R774, MS-FRS2_R777, MS-FRS2_R793, MS-FRS2_R866, MS-FRS2_" +
            "R808, MS-FRS2_R866, MS-FRS2_R872, MS-FRS2_R873, MS-FRS2_R808, MS-FRS2_R866, MS-F" +
            "RS2_R872, MS-FRS2_R873, MS-FRS2_R808, MS-FRS2_R866, MS-FRS2_R808, MS-FRS2_R866, " +
            "MS-FRS2_R872, MS-FRS2_R873, MS-FRS2_R808, MS-FRS2_R866, MS-FRS2_R808, MS-FRS2_R8" +
            "65, MS-FRS2_R868, MS-FRS2_R737, MS-FRS2_R739, MS-FRS2_R866, MS-FRS2_R872, MS-FRS" +
            "2_R873, MS-FRS2_R808, MS-FRS2_R93, MS-FRS2_R498, MS-FRS2_R774, MS-FRS2_R777, MS-" +
            "FRS2_R793, MS-FRS2_R866, MS-FRS2_R808, MS-FRS2_R866, MS-FRS2_R872, MS-FRS2_R873," +
            " MS-FRS2_R808, MS-FRS2_R866, MS-FRS2_R872, MS-FRS2_R873, MS-FRS2_R808, MS-FRS2_R" +
            "866, MS-FRS2_R808, MS-FRS2_R866, MS-FRS2_R872, MS-FRS2_R873, MS-FRS2_R808, MS-FR" +
            "S2_R866, MS-FRS2_R808, MS-FRS2_R865, MS-FRS2_R868, MS-FRS2_R737, MS-FRS2_R739, M" +
            "S-FRS2_R866, MS-FRS2_R872, MS-FRS2_R873, MS-FRS2_R808, MS-FRS2_R556, MS-FRS2_R10" +
            "20, MS-FRS2_R555")]
        public virtual void FRS2_TCtestScenario2S4() {
            this.Manager.BeginTest("TCtestScenario2S4");
            this.Manager.Comment("reaching state \'S4\'");
            FRS2Model.error_status_t temp20;
            this.Manager.Comment(@"executing step 'call Initialization(Windows7,Map{1=>enabled,2=>enabled,3=>enabled,4=>disabled},Map{1=>exists,2=>exists,3=>exists,4=>exists},Map{""A""=>1,""B""=>2,""C""=>3,""D""=>4},Map{""A""=>sysvol,""B""=>protection,""C""=>sysvol,""D""=>sysvol},Map{1=>""A"",2=>""B"",3=>""C"",4=>""D""},Map{1=>writeOnly,2=>readWrite,3=>readOnly,4=>readWrite},Map{1=>enabled,2=>disabled,3=>enabled,4=>enabled})'");
            temp20 = this.IFRS2ManagedAdapterInstance.Initialization(FRS2Model.OSVersion.Windows7, new Microsoft.Modeling.Map<System.Int32,FRS2Model.connectionProperty>().Add(1, FRS2Model.connectionProperty.enabled).Add(2, FRS2Model.connectionProperty.enabled).Add(3, FRS2Model.connectionProperty.enabled).Add(4, FRS2Model.connectionProperty.disabled), new Microsoft.Modeling.Map<System.Int32,FRS2Model.FromServer>().Add(1, FRS2Model.FromServer.exists).Add(2, FRS2Model.FromServer.exists).Add(3, FRS2Model.FromServer.exists).Add(4, FRS2Model.FromServer.exists), new Microsoft.Modeling.Map<System.String,System.Int32>().Add("A", 1).Add("B", 2).Add("C", 3).Add("D", 4), new Microsoft.Modeling.Map<System.String,FRS2Model.ReplicationGroupTypes>().Add("A", FRS2Model.ReplicationGroupTypes.sysvol).Add("B", FRS2Model.ReplicationGroupTypes.protection).Add("C", FRS2Model.ReplicationGroupTypes.sysvol).Add("D", FRS2Model.ReplicationGroupTypes.sysvol), new Microsoft.Modeling.Map<System.Int32,System.String>().Add(1, "A").Add(2, "B").Add(3, "C").Add(4, "D"), new Microsoft.Modeling.Map<System.Int32,FRS2Model.accessLevels>().Add(1, FRS2Model.accessLevels.writeOnly).Add(2, FRS2Model.accessLevels.readWrite).Add(3, FRS2Model.accessLevels.readOnly).Add(4, FRS2Model.accessLevels.readWrite), new Microsoft.Modeling.Map<System.Int32,FRS2Model.connectionProperty>().Add(1, FRS2Model.connectionProperty.enabled).Add(2, FRS2Model.connectionProperty.disabled).Add(3, FRS2Model.connectionProperty.enabled).Add(4, FRS2Model.connectionProperty.enabled));
            this.Manager.Comment("reaching state \'S5\'");
            this.Manager.Comment("checking step \'return Initialization/ERROR_SUCCESS\'");
            this.Manager.Assert((temp20 == FRS2Model.error_status_t.ERROR_SUCCESS), String.Format("expected \'FRS2Model.error_status_t.ERROR_SUCCESS\', actual \'{0}\' (return of Initia" +
                        "lization, state S5)", TestManagerHelpers.Describe(temp20)));
            this.Manager.Comment("reaching state \'S28\'");
            FRS2Model.ProtocolVersionReturned temp21;
            FRS2Model.UpstreamFlagValueReturned temp22;
            FRS2Model.error_status_t temp23;
            this.Manager.Comment("executing step \'call EstablishConnection(\"A\",1,FRS_COMMUNICATION_PROTOCOL_VERSION" +
                    "_LONGHORN_SERVER,out _,out _)\'");
            temp23 = this.IFRS2ManagedAdapterInstance.EstablishConnection("A", 1, FRS2Model.ProtocolVersion.FRS_COMMUNICATION_PROTOCOL_VERSION_LONGHORN_SERVER, out temp21, out temp22);
            this.Manager.Checkpoint("MS-FRS2_R37");
            this.Manager.Checkpoint("MS-FRS2_R436");
            this.Manager.Checkpoint("MS-FRS2_R439");
            this.Manager.Comment("reaching state \'S41\'");
            this.Manager.Comment("checking step \'return EstablishConnection/[out Valid,out Valid]:ERROR_SUCCESS\'");
            this.Manager.Assert((temp21 == FRS2Model.ProtocolVersionReturned.Valid), String.Format("expected \'FRS2Model.ProtocolVersionReturned.Valid\', actual \'{0}\' (upstreamProtoco" +
                        "lVersion of EstablishConnection, state S41)", TestManagerHelpers.Describe(temp21)));
            this.Manager.Assert((temp22 == FRS2Model.UpstreamFlagValueReturned.Valid), String.Format("expected \'FRS2Model.UpstreamFlagValueReturned.Valid\', actual \'{0}\' (upstreamFlags" +
                        " of EstablishConnection, state S41)", TestManagerHelpers.Describe(temp22)));
            this.Manager.Assert((temp23 == FRS2Model.error_status_t.ERROR_SUCCESS), String.Format("expected \'FRS2Model.error_status_t.ERROR_SUCCESS\', actual \'{0}\' (return of Establ" +
                        "ishConnection, state S41)", TestManagerHelpers.Describe(temp23)));
            this.Manager.Comment("reaching state \'S54\'");
            FRS2Model.error_status_t temp24;
            this.Manager.Comment("executing step \'call EstablishSession(1,1)\'");
            temp24 = this.IFRS2ManagedAdapterInstance.EstablishSession(1, 1);
            this.Manager.Checkpoint("MS-FRS2_R455");
            this.Manager.Checkpoint("MS-FRS2_R458");
            this.Manager.Checkpoint("MS-FRS2_R448");
            this.Manager.Comment("reaching state \'S67\'");
            this.Manager.Comment("checking step \'return EstablishSession/ERROR_SUCCESS\'");
            this.Manager.Assert((temp24 == FRS2Model.error_status_t.ERROR_SUCCESS), String.Format("expected \'FRS2Model.error_status_t.ERROR_SUCCESS\', actual \'{0}\' (return of Establ" +
                        "ishSession, state S67)", TestManagerHelpers.Describe(temp24)));
            this.Manager.Comment("reaching state \'S80\'");
            FRS2Model.error_status_t temp25;
            this.Manager.Comment("executing step \'call AsyncPoll(1)\'");
            temp25 = this.IFRS2ManagedAdapterInstance.AsyncPoll(1);
            this.Manager.Checkpoint("MS-FRS2_R549");
            this.Manager.Checkpoint("MS-FRS2_R552");
            this.Manager.Comment("reaching state \'S93\'");
            this.Manager.Comment("checking step \'return AsyncPoll/ERROR_SUCCESS\'");
            this.Manager.Assert((temp25 == FRS2Model.error_status_t.ERROR_SUCCESS), String.Format("expected \'FRS2Model.error_status_t.ERROR_SUCCESS\', actual \'{0}\' (return of AsyncP" +
                        "oll, state S93)", TestManagerHelpers.Describe(temp25)));
            this.Manager.Comment("reaching state \'S105\'");
            FRS2Model.error_status_t temp26;
            this.Manager.Comment("executing step \'call RequestVersionVector(1,1,1,REQUEST_NORMAL_SYNC,CHANGE_NOTIFY" +
                    ",ValidValue)\'");
            temp26 = this.IFRS2ManagedAdapterInstance.RequestVersionVector(1, 1, 1, FRS2Model.VERSION_REQUEST_TYPE.REQUEST_NORMAL_SYNC, FRS2Model.VERSION_CHANGE_TYPE.CHANGE_NOTIFY, FRS2Model.VVGeneration.ValidValue);
            this.Manager.Checkpoint("MS-FRS2_R517");
            this.Manager.Checkpoint("MS-FRS2_R520");
            this.Manager.Checkpoint("MS-FRS2_R466");
            this.Manager.Comment("reaching state \'S116\'");
            this.Manager.Comment("checking step \'return RequestVersionVector/ERROR_SUCCESS\'");
            this.Manager.Assert((temp26 == FRS2Model.error_status_t.ERROR_SUCCESS), String.Format("expected \'FRS2Model.error_status_t.ERROR_SUCCESS\', actual \'{0}\' (return of Reques" +
                        "tVersionVector, state S116)", TestManagerHelpers.Describe(temp26)));
            this.Manager.Comment("reaching state \'S124\'");
            int temp63 = this.Manager.ExpectEvent(this.QuiescenceTimeout, true, new ExpectedEvent(TCtestScenario2.AsyncPollResponseEventInfo, null, new AsyncPollResponseEventDelegate1(this.TCtestScenario2S4AsyncPollResponseEventChecker)), new ExpectedEvent(TCtestScenario2.AsyncPollResponseEventInfo, null, new AsyncPollResponseEventDelegate1(this.TCtestScenario2S4AsyncPollResponseEventChecker1)));
            if ((temp63 == 0)) {
                this.Manager.Comment("reaching state \'S133\'");
                FRS2Model.error_status_t temp27;
                this.Manager.Comment("executing step \'call RequestUpdates(1,1,valid)\'");
                temp27 = this.IFRS2ManagedAdapterInstance.RequestUpdates(1, 1, FRS2Model.versionVectorDiff.valid);
                this.Manager.Checkpoint("MS-FRS2_R486");
                this.Manager.Checkpoint("MS-FRS2_R489");
                this.Manager.Comment("reaching state \'S140\'");
                this.Manager.Comment("checking step \'return RequestUpdates/ERROR_SUCCESS\'");
                this.Manager.Assert((temp27 == FRS2Model.error_status_t.ERROR_SUCCESS), String.Format("expected \'FRS2Model.error_status_t.ERROR_SUCCESS\', actual \'{0}\' (return of Reques" +
                            "tUpdates, state S140)", TestManagerHelpers.Describe(temp27)));
                this.Manager.Comment("reaching state \'S144\'");
                int temp62 = this.Manager.ExpectEvent(this.QuiescenceTimeout, true, new ExpectedEvent(TCtestScenario2.RequestUpdatesEventInfo, null, new RequestUpdatesEventDelegate1(this.TCtestScenario2S4RequestUpdatesEventChecker)), new ExpectedEvent(TCtestScenario2.RequestUpdatesEventInfo, null, new RequestUpdatesEventDelegate1(this.TCtestScenario2S4RequestUpdatesEventChecker1)), new ExpectedEvent(TCtestScenario2.RequestUpdatesEventInfo, null, new RequestUpdatesEventDelegate1(this.TCtestScenario2S4RequestUpdatesEventChecker2)), new ExpectedEvent(TCtestScenario2.RequestUpdatesEventInfo, null, new RequestUpdatesEventDelegate1(this.TCtestScenario2S4RequestUpdatesEventChecker3)));
                if ((temp62 == 0)) {
                    this.Manager.Comment("reaching state \'S151\'");
                    FRS2Model.error_status_t temp28;
                    this.Manager.Comment("executing step \'call InitializeFileTransferAsync(1,5,False)\'");
                    temp28 = this.IFRS2ManagedAdapterInstance.InitializeFileTransferAsync(1, 5, false);
                    this.Manager.Checkpoint("MS-FRS2_R775");
                    this.Manager.Checkpoint("MS-FRS2_R779");
                    this.Manager.Checkpoint("MS-FRS2_R784");
                    this.Manager.AddReturn(InitializeFileTransferAsyncInfo, null, temp28);
                    TCtestScenario2S163();
                    goto label5;
                }
                if ((temp62 == 1)) {
                    this.Manager.Comment("reaching state \'S152\'");
                    FRS2Model.error_status_t temp29;
                    this.Manager.Comment("executing step \'call InitializeFileTransferAsync(1,1,False)\'");
                    temp29 = this.IFRS2ManagedAdapterInstance.InitializeFileTransferAsync(1, 1, false);
                    this.Manager.Checkpoint("MS-FRS2_R774");
                    this.Manager.Checkpoint("MS-FRS2_R777");
                    this.Manager.Checkpoint("MS-FRS2_R793");
                    this.Manager.AddReturn(InitializeFileTransferAsyncInfo, null, temp29);
                    TCtestScenario2S164();
                    goto label5;
                }
                if ((temp62 == 2)) {
                    this.Manager.Comment("reaching state \'S153\'");
                    FRS2Model.error_status_t temp40;
                    this.Manager.Comment("executing step \'call InitializeFileTransferAsync(1,1,False)\'");
                    temp40 = this.IFRS2ManagedAdapterInstance.InitializeFileTransferAsync(1, 1, false);
                    this.Manager.Checkpoint("MS-FRS2_R774");
                    this.Manager.Checkpoint("MS-FRS2_R777");
                    this.Manager.Checkpoint("MS-FRS2_R793");
                    this.Manager.Comment("reaching state \'S165\'");
                    this.Manager.Comment("checking step \'return InitializeFileTransferAsync/ERROR_SUCCESS\'");
                    this.Manager.Assert((temp40 == FRS2Model.error_status_t.ERROR_SUCCESS), String.Format("expected \'FRS2Model.error_status_t.ERROR_SUCCESS\', actual \'{0}\' (return of Initia" +
                                "lizeFileTransferAsync, state S165)", TestManagerHelpers.Describe(temp40)));
                    this.Manager.Comment("reaching state \'S176\'");
                    int temp50 = this.Manager.ExpectEvent(this.QuiescenceTimeout, true, new ExpectedEvent(TCtestScenario2.InitializeFileTransferAsyncEventInfo, null, new InitializeFileTransferAsyncEventDelegate1(this.TCtestScenario2S4InitializeFileTransferAsyncEventChecker8)), new ExpectedEvent(TCtestScenario2.InitializeFileTransferAsyncEventInfo, null, new InitializeFileTransferAsyncEventDelegate1(this.TCtestScenario2S4InitializeFileTransferAsyncEventChecker9)), new ExpectedEvent(TCtestScenario2.InitializeFileTransferAsyncEventInfo, null, new InitializeFileTransferAsyncEventDelegate1(this.TCtestScenario2S4InitializeFileTransferAsyncEventChecker10)), new ExpectedEvent(TCtestScenario2.InitializeFileTransferAsyncEventInfo, null, new InitializeFileTransferAsyncEventDelegate1(this.TCtestScenario2S4InitializeFileTransferAsyncEventChecker11)), new ExpectedEvent(TCtestScenario2.InitializeFileTransferAsyncEventInfo, null, new InitializeFileTransferAsyncEventDelegate1(this.TCtestScenario2S4InitializeFileTransferAsyncEventChecker12)), new ExpectedEvent(TCtestScenario2.InitializeFileTransferAsyncEventInfo, null, new InitializeFileTransferAsyncEventDelegate1(this.TCtestScenario2S4InitializeFileTransferAsyncEventChecker13)), new ExpectedEvent(TCtestScenario2.InitializeFileTransferAsyncEventInfo, null, new InitializeFileTransferAsyncEventDelegate1(this.TCtestScenario2S4InitializeFileTransferAsyncEventChecker14)), new ExpectedEvent(TCtestScenario2.InitializeFileTransferAsyncEventInfo, null, new InitializeFileTransferAsyncEventDelegate1(this.TCtestScenario2S4InitializeFileTransferAsyncEventChecker15)));
                    if ((temp50 == 0)) {
                        this.Manager.Comment("reaching state \'S188\'");
                        FRS2Model.error_status_t temp41;
                        this.Manager.Comment("executing step \'call RawGetFileDataAsync()\'");
                        temp41 = this.IFRS2ManagedAdapterInstance.RawGetFileDataAsync();
                        this.Manager.Checkpoint("MS-FRS2_R866");
                        this.Manager.Checkpoint("MS-FRS2_R808");
                        this.Manager.Comment("reaching state \'S212\'");
                        this.Manager.Comment("checking step \'return RawGetFileDataAsync/ERROR_FAIL\'");
                        this.Manager.Assert((temp41 == FRS2Model.error_status_t.ERROR_FAIL), String.Format("expected \'FRS2Model.error_status_t.ERROR_FAIL\', actual \'{0}\' (return of RawGetFil" +
                                    "eDataAsync, state S212)", TestManagerHelpers.Describe(temp41)));
                        this.Manager.Comment("reaching state \'S236\'");
                        goto label3;
                    }
                    if ((temp50 == 1)) {
                        this.Manager.Comment("reaching state \'S189\'");
                        FRS2Model.error_status_t temp42;
                        this.Manager.Comment("executing step \'call RawGetFileDataAsync()\'");
                        temp42 = this.IFRS2ManagedAdapterInstance.RawGetFileDataAsync();
                        this.Manager.Checkpoint("MS-FRS2_R866");
                        this.Manager.Checkpoint("MS-FRS2_R872");
                        this.Manager.Checkpoint("MS-FRS2_R873");
                        this.Manager.Checkpoint("MS-FRS2_R808");
                        this.Manager.Comment("reaching state \'S213\'");
                        this.Manager.Comment("checking step \'return RawGetFileDataAsync/ERROR_FAIL\'");
                        this.Manager.Assert((temp42 == FRS2Model.error_status_t.ERROR_FAIL), String.Format("expected \'FRS2Model.error_status_t.ERROR_FAIL\', actual \'{0}\' (return of RawGetFil" +
                                    "eDataAsync, state S213)", TestManagerHelpers.Describe(temp42)));
                        this.Manager.Comment("reaching state \'S237\'");
                        goto label3;
                    }
                    if ((temp50 == 2)) {
                        this.Manager.Comment("reaching state \'S190\'");
                        FRS2Model.error_status_t temp43;
                        this.Manager.Comment("executing step \'call RawGetFileDataAsync()\'");
                        temp43 = this.IFRS2ManagedAdapterInstance.RawGetFileDataAsync();
                        this.Manager.Checkpoint("MS-FRS2_R866");
                        this.Manager.Checkpoint("MS-FRS2_R872");
                        this.Manager.Checkpoint("MS-FRS2_R873");
                        this.Manager.Checkpoint("MS-FRS2_R808");
                        this.Manager.Comment("reaching state \'S214\'");
                        this.Manager.Comment("checking step \'return RawGetFileDataAsync/ERROR_FAIL\'");
                        this.Manager.Assert((temp43 == FRS2Model.error_status_t.ERROR_FAIL), String.Format("expected \'FRS2Model.error_status_t.ERROR_FAIL\', actual \'{0}\' (return of RawGetFil" +
                                    "eDataAsync, state S214)", TestManagerHelpers.Describe(temp43)));
                        this.Manager.Comment("reaching state \'S238\'");
                        goto label3;
                    }
                    if ((temp50 == 3)) {
                        this.Manager.Comment("reaching state \'S191\'");
                        FRS2Model.error_status_t temp44;
                        this.Manager.Comment("executing step \'call RawGetFileDataAsync()\'");
                        temp44 = this.IFRS2ManagedAdapterInstance.RawGetFileDataAsync();
                        this.Manager.Checkpoint("MS-FRS2_R866");
                        this.Manager.Checkpoint("MS-FRS2_R808");
                        this.Manager.Comment("reaching state \'S215\'");
                        this.Manager.Comment("checking step \'return RawGetFileDataAsync/ERROR_FAIL\'");
                        this.Manager.Assert((temp44 == FRS2Model.error_status_t.ERROR_FAIL), String.Format("expected \'FRS2Model.error_status_t.ERROR_FAIL\', actual \'{0}\' (return of RawGetFil" +
                                    "eDataAsync, state S215)", TestManagerHelpers.Describe(temp44)));
                        this.Manager.Comment("reaching state \'S239\'");
                        goto label3;
                    }
                    if ((temp50 == 4)) {
                        this.Manager.Comment("reaching state \'S192\'");
                        FRS2Model.error_status_t temp45;
                        this.Manager.Comment("executing step \'call RawGetFileDataAsync()\'");
                        temp45 = this.IFRS2ManagedAdapterInstance.RawGetFileDataAsync();
                        this.Manager.Checkpoint("MS-FRS2_R866");
                        this.Manager.Checkpoint("MS-FRS2_R872");
                        this.Manager.Checkpoint("MS-FRS2_R873");
                        this.Manager.Checkpoint("MS-FRS2_R808");
                        this.Manager.Comment("reaching state \'S216\'");
                        this.Manager.Comment("checking step \'return RawGetFileDataAsync/ERROR_FAIL\'");
                        this.Manager.Assert((temp45 == FRS2Model.error_status_t.ERROR_FAIL), String.Format("expected \'FRS2Model.error_status_t.ERROR_FAIL\', actual \'{0}\' (return of RawGetFil" +
                                    "eDataAsync, state S216)", TestManagerHelpers.Describe(temp45)));
                        this.Manager.Comment("reaching state \'S240\'");
                        goto label3;
                    }
                    if ((temp50 == 5)) {
                        this.Manager.Comment("reaching state \'S193\'");
                        FRS2Model.error_status_t temp46;
                        this.Manager.Comment("executing step \'call RawGetFileDataAsync()\'");
                        temp46 = this.IFRS2ManagedAdapterInstance.RawGetFileDataAsync();
                        this.Manager.Checkpoint("MS-FRS2_R866");
                        this.Manager.Checkpoint("MS-FRS2_R808");
                        this.Manager.Comment("reaching state \'S217\'");
                        this.Manager.Comment("checking step \'return RawGetFileDataAsync/ERROR_FAIL\'");
                        this.Manager.Assert((temp46 == FRS2Model.error_status_t.ERROR_FAIL), String.Format("expected \'FRS2Model.error_status_t.ERROR_FAIL\', actual \'{0}\' (return of RawGetFil" +
                                    "eDataAsync, state S217)", TestManagerHelpers.Describe(temp46)));
                        this.Manager.Comment("reaching state \'S241\'");
                        goto label3;
                    }
                    if ((temp50 == 6)) {
                        this.Manager.Comment("reaching state \'S194\'");
                        FRS2Model.error_status_t temp47;
                        this.Manager.Comment("executing step \'call RawGetFileDataAsync()\'");
                        temp47 = this.IFRS2ManagedAdapterInstance.RawGetFileDataAsync();
                        this.Manager.Checkpoint("MS-FRS2_R865");
                        this.Manager.Checkpoint("MS-FRS2_R868");
                        this.Manager.Comment("reaching state \'S218\'");
                        this.Manager.Comment("checking step \'return RawGetFileDataAsync/ERROR_SUCCESS\'");
                        this.Manager.Assert((temp47 == FRS2Model.error_status_t.ERROR_SUCCESS), String.Format("expected \'FRS2Model.error_status_t.ERROR_SUCCESS\', actual \'{0}\' (return of RawGet" +
                                    "FileDataAsync, state S218)", TestManagerHelpers.Describe(temp47)));
                        this.Manager.Comment("reaching state \'S242\'");
                        FRS2Model.error_status_t temp48;
                        this.Manager.Comment("executing step \'call RdcClose()\'");
                        temp48 = this.IFRS2ManagedAdapterInstance.RdcClose();
                        this.Manager.Checkpoint("MS-FRS2_R737");
                        this.Manager.Checkpoint("MS-FRS2_R739");
                        this.Manager.Comment("reaching state \'S253\'");
                        this.Manager.Comment("checking step \'return RdcClose/ERROR_SUCCESS\'");
                        this.Manager.Assert((temp48 == FRS2Model.error_status_t.ERROR_SUCCESS), String.Format("expected \'FRS2Model.error_status_t.ERROR_SUCCESS\', actual \'{0}\' (return of RdcClo" +
                                    "se, state S253)", TestManagerHelpers.Describe(temp48)));
                        this.Manager.Comment("reaching state \'S256\'");
                        goto label3;
                    }
                    if ((temp50 == 7)) {
                        this.Manager.Comment("reaching state \'S195\'");
                        FRS2Model.error_status_t temp49;
                        this.Manager.Comment("executing step \'call RawGetFileDataAsync()\'");
                        temp49 = this.IFRS2ManagedAdapterInstance.RawGetFileDataAsync();
                        this.Manager.Checkpoint("MS-FRS2_R866");
                        this.Manager.Checkpoint("MS-FRS2_R872");
                        this.Manager.Checkpoint("MS-FRS2_R873");
                        this.Manager.Checkpoint("MS-FRS2_R808");
                        this.Manager.Comment("reaching state \'S219\'");
                        this.Manager.Comment("checking step \'return RawGetFileDataAsync/ERROR_FAIL\'");
                        this.Manager.Assert((temp49 == FRS2Model.error_status_t.ERROR_FAIL), String.Format("expected \'FRS2Model.error_status_t.ERROR_FAIL\', actual \'{0}\' (return of RawGetFil" +
                                    "eDataAsync, state S219)", TestManagerHelpers.Describe(temp49)));
                        this.Manager.Comment("reaching state \'S243\'");
                        goto label3;
                    }
                    throw new InvalidOperationException("never reached");
                label3:
;
                    goto label5;
                }
                if ((temp62 == 3)) {
                    this.Manager.Comment("reaching state \'S154\'");
                    FRS2Model.error_status_t temp51;
                    this.Manager.Comment("executing step \'call InitializeFileTransferAsync(1,1,False)\'");
                    temp51 = this.IFRS2ManagedAdapterInstance.InitializeFileTransferAsync(1, 1, false);
                    this.Manager.Checkpoint("MS-FRS2_R774");
                    this.Manager.Checkpoint("MS-FRS2_R777");
                    this.Manager.Checkpoint("MS-FRS2_R793");
                    this.Manager.Comment("reaching state \'S166\'");
                    this.Manager.Comment("checking step \'return InitializeFileTransferAsync/ERROR_SUCCESS\'");
                    this.Manager.Assert((temp51 == FRS2Model.error_status_t.ERROR_SUCCESS), String.Format("expected \'FRS2Model.error_status_t.ERROR_SUCCESS\', actual \'{0}\' (return of Initia" +
                                "lizeFileTransferAsync, state S166)", TestManagerHelpers.Describe(temp51)));
                    this.Manager.Comment("reaching state \'S177\'");
                    int temp61 = this.Manager.ExpectEvent(this.QuiescenceTimeout, true, new ExpectedEvent(TCtestScenario2.InitializeFileTransferAsyncEventInfo, null, new InitializeFileTransferAsyncEventDelegate1(this.TCtestScenario2S4InitializeFileTransferAsyncEventChecker16)), new ExpectedEvent(TCtestScenario2.InitializeFileTransferAsyncEventInfo, null, new InitializeFileTransferAsyncEventDelegate1(this.TCtestScenario2S4InitializeFileTransferAsyncEventChecker17)), new ExpectedEvent(TCtestScenario2.InitializeFileTransferAsyncEventInfo, null, new InitializeFileTransferAsyncEventDelegate1(this.TCtestScenario2S4InitializeFileTransferAsyncEventChecker18)), new ExpectedEvent(TCtestScenario2.InitializeFileTransferAsyncEventInfo, null, new InitializeFileTransferAsyncEventDelegate1(this.TCtestScenario2S4InitializeFileTransferAsyncEventChecker19)), new ExpectedEvent(TCtestScenario2.InitializeFileTransferAsyncEventInfo, null, new InitializeFileTransferAsyncEventDelegate1(this.TCtestScenario2S4InitializeFileTransferAsyncEventChecker20)), new ExpectedEvent(TCtestScenario2.InitializeFileTransferAsyncEventInfo, null, new InitializeFileTransferAsyncEventDelegate1(this.TCtestScenario2S4InitializeFileTransferAsyncEventChecker21)), new ExpectedEvent(TCtestScenario2.InitializeFileTransferAsyncEventInfo, null, new InitializeFileTransferAsyncEventDelegate1(this.TCtestScenario2S4InitializeFileTransferAsyncEventChecker22)), new ExpectedEvent(TCtestScenario2.InitializeFileTransferAsyncEventInfo, null, new InitializeFileTransferAsyncEventDelegate1(this.TCtestScenario2S4InitializeFileTransferAsyncEventChecker23)));
                    if ((temp61 == 0)) {
                        this.Manager.Comment("reaching state \'S196\'");
                        FRS2Model.error_status_t temp52;
                        this.Manager.Comment("executing step \'call RawGetFileDataAsync()\'");
                        temp52 = this.IFRS2ManagedAdapterInstance.RawGetFileDataAsync();
                        this.Manager.Checkpoint("MS-FRS2_R866");
                        this.Manager.Checkpoint("MS-FRS2_R808");
                        this.Manager.Comment("reaching state \'S220\'");
                        this.Manager.Comment("checking step \'return RawGetFileDataAsync/ERROR_FAIL\'");
                        this.Manager.Assert((temp52 == FRS2Model.error_status_t.ERROR_FAIL), String.Format("expected \'FRS2Model.error_status_t.ERROR_FAIL\', actual \'{0}\' (return of RawGetFil" +
                                    "eDataAsync, state S220)", TestManagerHelpers.Describe(temp52)));
                        this.Manager.Comment("reaching state \'S244\'");
                        goto label4;
                    }
                    if ((temp61 == 1)) {
                        this.Manager.Comment("reaching state \'S197\'");
                        FRS2Model.error_status_t temp53;
                        this.Manager.Comment("executing step \'call RawGetFileDataAsync()\'");
                        temp53 = this.IFRS2ManagedAdapterInstance.RawGetFileDataAsync();
                        this.Manager.Checkpoint("MS-FRS2_R866");
                        this.Manager.Checkpoint("MS-FRS2_R872");
                        this.Manager.Checkpoint("MS-FRS2_R873");
                        this.Manager.Checkpoint("MS-FRS2_R808");
                        this.Manager.Comment("reaching state \'S221\'");
                        this.Manager.Comment("checking step \'return RawGetFileDataAsync/ERROR_FAIL\'");
                        this.Manager.Assert((temp53 == FRS2Model.error_status_t.ERROR_FAIL), String.Format("expected \'FRS2Model.error_status_t.ERROR_FAIL\', actual \'{0}\' (return of RawGetFil" +
                                    "eDataAsync, state S221)", TestManagerHelpers.Describe(temp53)));
                        this.Manager.Comment("reaching state \'S245\'");
                        goto label4;
                    }
                    if ((temp61 == 2)) {
                        this.Manager.Comment("reaching state \'S198\'");
                        FRS2Model.error_status_t temp54;
                        this.Manager.Comment("executing step \'call RawGetFileDataAsync()\'");
                        temp54 = this.IFRS2ManagedAdapterInstance.RawGetFileDataAsync();
                        this.Manager.Checkpoint("MS-FRS2_R866");
                        this.Manager.Checkpoint("MS-FRS2_R872");
                        this.Manager.Checkpoint("MS-FRS2_R873");
                        this.Manager.Checkpoint("MS-FRS2_R808");
                        this.Manager.Comment("reaching state \'S222\'");
                        this.Manager.Comment("checking step \'return RawGetFileDataAsync/ERROR_FAIL\'");
                        this.Manager.Assert((temp54 == FRS2Model.error_status_t.ERROR_FAIL), String.Format("expected \'FRS2Model.error_status_t.ERROR_FAIL\', actual \'{0}\' (return of RawGetFil" +
                                    "eDataAsync, state S222)", TestManagerHelpers.Describe(temp54)));
                        this.Manager.Comment("reaching state \'S246\'");
                        goto label4;
                    }
                    if ((temp61 == 3)) {
                        this.Manager.Comment("reaching state \'S199\'");
                        FRS2Model.error_status_t temp55;
                        this.Manager.Comment("executing step \'call RawGetFileDataAsync()\'");
                        temp55 = this.IFRS2ManagedAdapterInstance.RawGetFileDataAsync();
                        this.Manager.Checkpoint("MS-FRS2_R866");
                        this.Manager.Checkpoint("MS-FRS2_R808");
                        this.Manager.Comment("reaching state \'S223\'");
                        this.Manager.Comment("checking step \'return RawGetFileDataAsync/ERROR_FAIL\'");
                        this.Manager.Assert((temp55 == FRS2Model.error_status_t.ERROR_FAIL), String.Format("expected \'FRS2Model.error_status_t.ERROR_FAIL\', actual \'{0}\' (return of RawGetFil" +
                                    "eDataAsync, state S223)", TestManagerHelpers.Describe(temp55)));
                        this.Manager.Comment("reaching state \'S247\'");
                        goto label4;
                    }
                    if ((temp61 == 4)) {
                        this.Manager.Comment("reaching state \'S200\'");
                        FRS2Model.error_status_t temp56;
                        this.Manager.Comment("executing step \'call RawGetFileDataAsync()\'");
                        temp56 = this.IFRS2ManagedAdapterInstance.RawGetFileDataAsync();
                        this.Manager.Checkpoint("MS-FRS2_R866");
                        this.Manager.Checkpoint("MS-FRS2_R872");
                        this.Manager.Checkpoint("MS-FRS2_R873");
                        this.Manager.Checkpoint("MS-FRS2_R808");
                        this.Manager.Comment("reaching state \'S224\'");
                        this.Manager.Comment("checking step \'return RawGetFileDataAsync/ERROR_FAIL\'");
                        this.Manager.Assert((temp56 == FRS2Model.error_status_t.ERROR_FAIL), String.Format("expected \'FRS2Model.error_status_t.ERROR_FAIL\', actual \'{0}\' (return of RawGetFil" +
                                    "eDataAsync, state S224)", TestManagerHelpers.Describe(temp56)));
                        this.Manager.Comment("reaching state \'S248\'");
                        goto label4;
                    }
                    if ((temp61 == 5)) {
                        this.Manager.Comment("reaching state \'S201\'");
                        FRS2Model.error_status_t temp57;
                        this.Manager.Comment("executing step \'call RawGetFileDataAsync()\'");
                        temp57 = this.IFRS2ManagedAdapterInstance.RawGetFileDataAsync();
                        this.Manager.Checkpoint("MS-FRS2_R866");
                        this.Manager.Checkpoint("MS-FRS2_R808");
                        this.Manager.Comment("reaching state \'S225\'");
                        this.Manager.Comment("checking step \'return RawGetFileDataAsync/ERROR_FAIL\'");
                        this.Manager.Assert((temp57 == FRS2Model.error_status_t.ERROR_FAIL), String.Format("expected \'FRS2Model.error_status_t.ERROR_FAIL\', actual \'{0}\' (return of RawGetFil" +
                                    "eDataAsync, state S225)", TestManagerHelpers.Describe(temp57)));
                        this.Manager.Comment("reaching state \'S249\'");
                        goto label4;
                    }
                    if ((temp61 == 6)) {
                        this.Manager.Comment("reaching state \'S202\'");
                        FRS2Model.error_status_t temp58;
                        this.Manager.Comment("executing step \'call RawGetFileDataAsync()\'");
                        temp58 = this.IFRS2ManagedAdapterInstance.RawGetFileDataAsync();
                        this.Manager.Checkpoint("MS-FRS2_R865");
                        this.Manager.Checkpoint("MS-FRS2_R868");
                        this.Manager.Comment("reaching state \'S226\'");
                        this.Manager.Comment("checking step \'return RawGetFileDataAsync/ERROR_SUCCESS\'");
                        this.Manager.Assert((temp58 == FRS2Model.error_status_t.ERROR_SUCCESS), String.Format("expected \'FRS2Model.error_status_t.ERROR_SUCCESS\', actual \'{0}\' (return of RawGet" +
                                    "FileDataAsync, state S226)", TestManagerHelpers.Describe(temp58)));
                        this.Manager.Comment("reaching state \'S250\'");
                        FRS2Model.error_status_t temp59;
                        this.Manager.Comment("executing step \'call RdcClose()\'");
                        temp59 = this.IFRS2ManagedAdapterInstance.RdcClose();
                        this.Manager.Checkpoint("MS-FRS2_R737");
                        this.Manager.Checkpoint("MS-FRS2_R739");
                        this.Manager.Comment("reaching state \'S254\'");
                        this.Manager.Comment("checking step \'return RdcClose/ERROR_SUCCESS\'");
                        this.Manager.Assert((temp59 == FRS2Model.error_status_t.ERROR_SUCCESS), String.Format("expected \'FRS2Model.error_status_t.ERROR_SUCCESS\', actual \'{0}\' (return of RdcClo" +
                                    "se, state S254)", TestManagerHelpers.Describe(temp59)));
                        this.Manager.Comment("reaching state \'S257\'");
                        goto label4;
                    }
                    if ((temp61 == 7)) {
                        this.Manager.Comment("reaching state \'S203\'");
                        FRS2Model.error_status_t temp60;
                        this.Manager.Comment("executing step \'call RawGetFileDataAsync()\'");
                        temp60 = this.IFRS2ManagedAdapterInstance.RawGetFileDataAsync();
                        this.Manager.Checkpoint("MS-FRS2_R866");
                        this.Manager.Checkpoint("MS-FRS2_R872");
                        this.Manager.Checkpoint("MS-FRS2_R873");
                        this.Manager.Checkpoint("MS-FRS2_R808");
                        this.Manager.Comment("reaching state \'S227\'");
                        this.Manager.Comment("checking step \'return RawGetFileDataAsync/ERROR_FAIL\'");
                        this.Manager.Assert((temp60 == FRS2Model.error_status_t.ERROR_FAIL), String.Format("expected \'FRS2Model.error_status_t.ERROR_FAIL\', actual \'{0}\' (return of RawGetFil" +
                                    "eDataAsync, state S227)", TestManagerHelpers.Describe(temp60)));
                        this.Manager.Comment("reaching state \'S251\'");
                        goto label4;
                    }
                    throw new InvalidOperationException("never reached");
                label4:
;
                    goto label5;
                }
                throw new InvalidOperationException("never reached");
            label5:
;
                goto label6;
            }
            if ((temp63 == 1)) {
                TCtestScenario2S132();
                goto label6;
            }
            throw new InvalidOperationException("never reached");
        label6:
;
            this.Manager.EndTest();
        }
        
        private void TCtestScenario2S4AsyncPollResponseEventChecker(FRS2Model.VVGeneration vvGen) {
            this.Manager.Comment("checking step \'event AsyncPollResponseEvent(ValidValue)\'");
            try {
                this.Manager.Assert((vvGen == FRS2Model.VVGeneration.ValidValue), String.Format("expected \'FRS2Model.VVGeneration.ValidValue\', actual \'{0}\' (vvGen of AsyncPollRes" +
                            "ponseEvent, state S124)", TestManagerHelpers.Describe(vvGen)));
            }
            catch (TransactionFailedException ) {
                this.Manager.Comment("This step would have covered MS-FRS2_R556, MS-FRS2_R1020, MS-FRS2_R555");
                throw;
            }
            this.Manager.Checkpoint("MS-FRS2_R556");
            this.Manager.Checkpoint("MS-FRS2_R1020");
            this.Manager.Checkpoint("MS-FRS2_R555");
        }
        
        private void TCtestScenario2S4RequestUpdatesEventChecker(FRS2Model.FilePresense present, FRS2Model.UPDATE_STATUS updateStatus) {
            this.Manager.Comment("checking step \'event RequestUpdatesEvent(fileDeleted,UPDATE_STATUS_MORE)\'");
            try {
                this.Manager.Assert((present == FRS2Model.FilePresense.fileDeleted), String.Format("expected \'FRS2Model.FilePresense.fileDeleted\', actual \'{0}\' (present of RequestUp" +
                            "datesEvent, state S144)", TestManagerHelpers.Describe(present)));
                this.Manager.Assert((updateStatus == FRS2Model.UPDATE_STATUS.UPDATE_STATUS_MORE), String.Format("expected \'FRS2Model.UPDATE_STATUS.UPDATE_STATUS_MORE\', actual \'{0}\' (updateStatus" +
                            " of RequestUpdatesEvent, state S144)", TestManagerHelpers.Describe(updateStatus)));
            }
            catch (TransactionFailedException ) {
                this.Manager.Comment("This step would have covered MS-FRS2_R93");
                throw;
            }
            this.Manager.Checkpoint("MS-FRS2_R93");
        }
        
        private void TCtestScenario2S163() {
            this.Manager.Comment("reaching state \'S163\'");
            this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(TCtestScenario2.InitializeFileTransferAsyncInfo, null, new InitializeFileTransferAsyncDelegate1(this.TCtestScenario2S4InitializeFileTransferAsyncChecker)));
            TCtestScenario2S171();
        }
        
        private void TCtestScenario2S4InitializeFileTransferAsyncChecker(FRS2Model.error_status_t @return) {
            this.Manager.Comment("checking step \'return InitializeFileTransferAsync/FRS_ERROR_CONTENTSET_NOT_FOUND\'" +
                    "");
            this.Manager.Assert((@return == FRS2Model.error_status_t.FRS_ERROR_CONTENTSET_NOT_FOUND), String.Format("expected \'FRS2Model.error_status_t.FRS_ERROR_CONTENTSET_NOT_FOUND\', actual \'{0}\' " +
                        "(return of InitializeFileTransferAsync, state S163)", TestManagerHelpers.Describe(@return)));
        }
        
        private void TCtestScenario2S4RequestUpdatesEventChecker1(FRS2Model.FilePresense present, FRS2Model.UPDATE_STATUS updateStatus) {
            this.Manager.Comment("checking step \'event RequestUpdatesEvent(fileDeleted,UPDATE_STATUS_DONE)\'");
            try {
                this.Manager.Assert((present == FRS2Model.FilePresense.fileDeleted), String.Format("expected \'FRS2Model.FilePresense.fileDeleted\', actual \'{0}\' (present of RequestUp" +
                            "datesEvent, state S144)", TestManagerHelpers.Describe(present)));
                this.Manager.Assert((updateStatus == FRS2Model.UPDATE_STATUS.UPDATE_STATUS_DONE), String.Format("expected \'FRS2Model.UPDATE_STATUS.UPDATE_STATUS_DONE\', actual \'{0}\' (updateStatus" +
                            " of RequestUpdatesEvent, state S144)", TestManagerHelpers.Describe(updateStatus)));
            }
            catch (TransactionFailedException ) {
                this.Manager.Comment("This step would have covered MS-FRS2_R93, MS-FRS2_R94");
                throw;
            }
            this.Manager.Checkpoint("MS-FRS2_R93");
            this.Manager.Checkpoint("MS-FRS2_R94");
        }
        
        private void TCtestScenario2S164() {
            this.Manager.Comment("reaching state \'S164\'");
            this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(TCtestScenario2.InitializeFileTransferAsyncInfo, null, new InitializeFileTransferAsyncDelegate1(this.TCtestScenario2S4InitializeFileTransferAsyncChecker1)));
            this.Manager.Comment("reaching state \'S175\'");
            int temp39 = this.Manager.ExpectEvent(this.QuiescenceTimeout, true, new ExpectedEvent(TCtestScenario2.InitializeFileTransferAsyncEventInfo, null, new InitializeFileTransferAsyncEventDelegate1(this.TCtestScenario2S4InitializeFileTransferAsyncEventChecker)), new ExpectedEvent(TCtestScenario2.InitializeFileTransferAsyncEventInfo, null, new InitializeFileTransferAsyncEventDelegate1(this.TCtestScenario2S4InitializeFileTransferAsyncEventChecker1)), new ExpectedEvent(TCtestScenario2.InitializeFileTransferAsyncEventInfo, null, new InitializeFileTransferAsyncEventDelegate1(this.TCtestScenario2S4InitializeFileTransferAsyncEventChecker2)), new ExpectedEvent(TCtestScenario2.InitializeFileTransferAsyncEventInfo, null, new InitializeFileTransferAsyncEventDelegate1(this.TCtestScenario2S4InitializeFileTransferAsyncEventChecker3)), new ExpectedEvent(TCtestScenario2.InitializeFileTransferAsyncEventInfo, null, new InitializeFileTransferAsyncEventDelegate1(this.TCtestScenario2S4InitializeFileTransferAsyncEventChecker4)), new ExpectedEvent(TCtestScenario2.InitializeFileTransferAsyncEventInfo, null, new InitializeFileTransferAsyncEventDelegate1(this.TCtestScenario2S4InitializeFileTransferAsyncEventChecker5)), new ExpectedEvent(TCtestScenario2.InitializeFileTransferAsyncEventInfo, null, new InitializeFileTransferAsyncEventDelegate1(this.TCtestScenario2S4InitializeFileTransferAsyncEventChecker6)), new ExpectedEvent(TCtestScenario2.InitializeFileTransferAsyncEventInfo, null, new InitializeFileTransferAsyncEventDelegate1(this.TCtestScenario2S4InitializeFileTransferAsyncEventChecker7)));
            if ((temp39 == 0)) {
                this.Manager.Comment("reaching state \'S180\'");
                FRS2Model.error_status_t temp30;
                this.Manager.Comment("executing step \'call RawGetFileDataAsync()\'");
                temp30 = this.IFRS2ManagedAdapterInstance.RawGetFileDataAsync();
                this.Manager.Checkpoint("MS-FRS2_R865");
                this.Manager.Checkpoint("MS-FRS2_R868");
                this.Manager.Comment("reaching state \'S204\'");
                this.Manager.Comment("checking step \'return RawGetFileDataAsync/ERROR_SUCCESS\'");
                this.Manager.Assert((temp30 == FRS2Model.error_status_t.ERROR_SUCCESS), String.Format("expected \'FRS2Model.error_status_t.ERROR_SUCCESS\', actual \'{0}\' (return of RawGet" +
                            "FileDataAsync, state S204)", TestManagerHelpers.Describe(temp30)));
                this.Manager.Comment("reaching state \'S228\'");
                FRS2Model.error_status_t temp31;
                this.Manager.Comment("executing step \'call RdcClose()\'");
                temp31 = this.IFRS2ManagedAdapterInstance.RdcClose();
                this.Manager.Checkpoint("MS-FRS2_R737");
                this.Manager.Checkpoint("MS-FRS2_R739");
                this.Manager.Comment("reaching state \'S252\'");
                this.Manager.Comment("checking step \'return RdcClose/ERROR_SUCCESS\'");
                this.Manager.Assert((temp31 == FRS2Model.error_status_t.ERROR_SUCCESS), String.Format("expected \'FRS2Model.error_status_t.ERROR_SUCCESS\', actual \'{0}\' (return of RdcClo" +
                            "se, state S252)", TestManagerHelpers.Describe(temp31)));
                this.Manager.Comment("reaching state \'S255\'");
                goto label2;
            }
            if ((temp39 == 1)) {
                this.Manager.Comment("reaching state \'S181\'");
                FRS2Model.error_status_t temp32;
                this.Manager.Comment("executing step \'call RawGetFileDataAsync()\'");
                temp32 = this.IFRS2ManagedAdapterInstance.RawGetFileDataAsync();
                this.Manager.Checkpoint("MS-FRS2_R866");
                this.Manager.Checkpoint("MS-FRS2_R872");
                this.Manager.Checkpoint("MS-FRS2_R873");
                this.Manager.Checkpoint("MS-FRS2_R808");
                this.Manager.Comment("reaching state \'S205\'");
                this.Manager.Comment("checking step \'return RawGetFileDataAsync/ERROR_FAIL\'");
                this.Manager.Assert((temp32 == FRS2Model.error_status_t.ERROR_FAIL), String.Format("expected \'FRS2Model.error_status_t.ERROR_FAIL\', actual \'{0}\' (return of RawGetFil" +
                            "eDataAsync, state S205)", TestManagerHelpers.Describe(temp32)));
                this.Manager.Comment("reaching state \'S229\'");
                goto label2;
            }
            if ((temp39 == 2)) {
                this.Manager.Comment("reaching state \'S182\'");
                FRS2Model.error_status_t temp33;
                this.Manager.Comment("executing step \'call RawGetFileDataAsync()\'");
                temp33 = this.IFRS2ManagedAdapterInstance.RawGetFileDataAsync();
                this.Manager.Checkpoint("MS-FRS2_R866");
                this.Manager.Checkpoint("MS-FRS2_R872");
                this.Manager.Checkpoint("MS-FRS2_R873");
                this.Manager.Checkpoint("MS-FRS2_R808");
                this.Manager.Comment("reaching state \'S206\'");
                this.Manager.Comment("checking step \'return RawGetFileDataAsync/ERROR_FAIL\'");
                this.Manager.Assert((temp33 == FRS2Model.error_status_t.ERROR_FAIL), String.Format("expected \'FRS2Model.error_status_t.ERROR_FAIL\', actual \'{0}\' (return of RawGetFil" +
                            "eDataAsync, state S206)", TestManagerHelpers.Describe(temp33)));
                this.Manager.Comment("reaching state \'S230\'");
                goto label2;
            }
            if ((temp39 == 3)) {
                this.Manager.Comment("reaching state \'S183\'");
                FRS2Model.error_status_t temp34;
                this.Manager.Comment("executing step \'call RawGetFileDataAsync()\'");
                temp34 = this.IFRS2ManagedAdapterInstance.RawGetFileDataAsync();
                this.Manager.Checkpoint("MS-FRS2_R866");
                this.Manager.Checkpoint("MS-FRS2_R808");
                this.Manager.Comment("reaching state \'S207\'");
                this.Manager.Comment("checking step \'return RawGetFileDataAsync/ERROR_FAIL\'");
                this.Manager.Assert((temp34 == FRS2Model.error_status_t.ERROR_FAIL), String.Format("expected \'FRS2Model.error_status_t.ERROR_FAIL\', actual \'{0}\' (return of RawGetFil" +
                            "eDataAsync, state S207)", TestManagerHelpers.Describe(temp34)));
                this.Manager.Comment("reaching state \'S231\'");
                goto label2;
            }
            if ((temp39 == 4)) {
                this.Manager.Comment("reaching state \'S184\'");
                FRS2Model.error_status_t temp35;
                this.Manager.Comment("executing step \'call RawGetFileDataAsync()\'");
                temp35 = this.IFRS2ManagedAdapterInstance.RawGetFileDataAsync();
                this.Manager.Checkpoint("MS-FRS2_R866");
                this.Manager.Checkpoint("MS-FRS2_R872");
                this.Manager.Checkpoint("MS-FRS2_R873");
                this.Manager.Checkpoint("MS-FRS2_R808");
                this.Manager.Comment("reaching state \'S208\'");
                this.Manager.Comment("checking step \'return RawGetFileDataAsync/ERROR_FAIL\'");
                this.Manager.Assert((temp35 == FRS2Model.error_status_t.ERROR_FAIL), String.Format("expected \'FRS2Model.error_status_t.ERROR_FAIL\', actual \'{0}\' (return of RawGetFil" +
                            "eDataAsync, state S208)", TestManagerHelpers.Describe(temp35)));
                this.Manager.Comment("reaching state \'S232\'");
                goto label2;
            }
            if ((temp39 == 5)) {
                this.Manager.Comment("reaching state \'S185\'");
                FRS2Model.error_status_t temp36;
                this.Manager.Comment("executing step \'call RawGetFileDataAsync()\'");
                temp36 = this.IFRS2ManagedAdapterInstance.RawGetFileDataAsync();
                this.Manager.Checkpoint("MS-FRS2_R866");
                this.Manager.Checkpoint("MS-FRS2_R808");
                this.Manager.Comment("reaching state \'S209\'");
                this.Manager.Comment("checking step \'return RawGetFileDataAsync/ERROR_FAIL\'");
                this.Manager.Assert((temp36 == FRS2Model.error_status_t.ERROR_FAIL), String.Format("expected \'FRS2Model.error_status_t.ERROR_FAIL\', actual \'{0}\' (return of RawGetFil" +
                            "eDataAsync, state S209)", TestManagerHelpers.Describe(temp36)));
                this.Manager.Comment("reaching state \'S233\'");
                goto label2;
            }
            if ((temp39 == 6)) {
                this.Manager.Comment("reaching state \'S186\'");
                FRS2Model.error_status_t temp37;
                this.Manager.Comment("executing step \'call RawGetFileDataAsync()\'");
                temp37 = this.IFRS2ManagedAdapterInstance.RawGetFileDataAsync();
                this.Manager.Checkpoint("MS-FRS2_R866");
                this.Manager.Checkpoint("MS-FRS2_R872");
                this.Manager.Checkpoint("MS-FRS2_R873");
                this.Manager.Checkpoint("MS-FRS2_R808");
                this.Manager.Comment("reaching state \'S210\'");
                this.Manager.Comment("checking step \'return RawGetFileDataAsync/ERROR_FAIL\'");
                this.Manager.Assert((temp37 == FRS2Model.error_status_t.ERROR_FAIL), String.Format("expected \'FRS2Model.error_status_t.ERROR_FAIL\', actual \'{0}\' (return of RawGetFil" +
                            "eDataAsync, state S210)", TestManagerHelpers.Describe(temp37)));
                this.Manager.Comment("reaching state \'S234\'");
                goto label2;
            }
            if ((temp39 == 7)) {
                this.Manager.Comment("reaching state \'S187\'");
                FRS2Model.error_status_t temp38;
                this.Manager.Comment("executing step \'call RawGetFileDataAsync()\'");
                temp38 = this.IFRS2ManagedAdapterInstance.RawGetFileDataAsync();
                this.Manager.Checkpoint("MS-FRS2_R866");
                this.Manager.Checkpoint("MS-FRS2_R808");
                this.Manager.Comment("reaching state \'S211\'");
                this.Manager.Comment("checking step \'return RawGetFileDataAsync/ERROR_FAIL\'");
                this.Manager.Assert((temp38 == FRS2Model.error_status_t.ERROR_FAIL), String.Format("expected \'FRS2Model.error_status_t.ERROR_FAIL\', actual \'{0}\' (return of RawGetFil" +
                            "eDataAsync, state S211)", TestManagerHelpers.Describe(temp38)));
                this.Manager.Comment("reaching state \'S235\'");
                goto label2;
            }
            throw new InvalidOperationException("never reached");
        label2:
;
        }
        
        private void TCtestScenario2S4InitializeFileTransferAsyncChecker1(FRS2Model.error_status_t @return) {
            this.Manager.Comment("checking step \'return InitializeFileTransferAsync/ERROR_SUCCESS\'");
            this.Manager.Assert((@return == FRS2Model.error_status_t.ERROR_SUCCESS), String.Format("expected \'FRS2Model.error_status_t.ERROR_SUCCESS\', actual \'{0}\' (return of Initia" +
                        "lizeFileTransferAsync, state S164)", TestManagerHelpers.Describe(@return)));
        }
        
        private void TCtestScenario2S4InitializeFileTransferAsyncEventChecker(FRS2Model.ServerContext context, FRS2Model.RDC_Sig_Level rdcsigLevel, bool isEOF) {
            this.Manager.Comment("checking step \'event InitializeFileTransferAsyncEvent(ValidContext,forRAWFileLeve" +
                    "l,False)\'");
            this.Manager.Assert((context == FRS2Model.ServerContext.ValidContext), String.Format("expected \'FRS2Model.ServerContext.ValidContext\', actual \'{0}\' (context of Initial" +
                        "izeFileTransferAsyncEvent, state S175)", TestManagerHelpers.Describe(context)));
            this.Manager.Assert((rdcsigLevel == FRS2Model.RDC_Sig_Level.forRAWFileLevel), String.Format("expected \'FRS2Model.RDC_Sig_Level.forRAWFileLevel\', actual \'{0}\' (rdcsigLevel of " +
                        "InitializeFileTransferAsyncEvent, state S175)", TestManagerHelpers.Describe(rdcsigLevel)));
            this.Manager.Assert((isEOF == false), String.Format("expected \'false\', actual \'{0}\' (isEOF of InitializeFileTransferAsyncEvent, state " +
                        "S175)", TestManagerHelpers.Describe(isEOF)));
        }
        
        private void TCtestScenario2S4InitializeFileTransferAsyncEventChecker1(FRS2Model.ServerContext context, FRS2Model.RDC_Sig_Level rdcsigLevel, bool isEOF) {
            this.Manager.Comment("checking step \'event InitializeFileTransferAsyncEvent(InvalidContext,forRAWFileLe" +
                    "vel,False)\'");
            this.Manager.Assert((context == FRS2Model.ServerContext.InvalidContext), String.Format("expected \'FRS2Model.ServerContext.InvalidContext\', actual \'{0}\' (context of Initi" +
                        "alizeFileTransferAsyncEvent, state S175)", TestManagerHelpers.Describe(context)));
            this.Manager.Assert((rdcsigLevel == FRS2Model.RDC_Sig_Level.forRAWFileLevel), String.Format("expected \'FRS2Model.RDC_Sig_Level.forRAWFileLevel\', actual \'{0}\' (rdcsigLevel of " +
                        "InitializeFileTransferAsyncEvent, state S175)", TestManagerHelpers.Describe(rdcsigLevel)));
            this.Manager.Assert((isEOF == false), String.Format("expected \'false\', actual \'{0}\' (isEOF of InitializeFileTransferAsyncEvent, state " +
                        "S175)", TestManagerHelpers.Describe(isEOF)));
        }
        
        private void TCtestScenario2S4InitializeFileTransferAsyncEventChecker2(FRS2Model.ServerContext context, FRS2Model.RDC_Sig_Level rdcsigLevel, bool isEOF) {
            this.Manager.Comment("checking step \'event InitializeFileTransferAsyncEvent(InvalidContext,forRDCFileLe" +
                    "vel,True)\'");
            this.Manager.Assert((context == FRS2Model.ServerContext.InvalidContext), String.Format("expected \'FRS2Model.ServerContext.InvalidContext\', actual \'{0}\' (context of Initi" +
                        "alizeFileTransferAsyncEvent, state S175)", TestManagerHelpers.Describe(context)));
            this.Manager.Assert((rdcsigLevel == FRS2Model.RDC_Sig_Level.forRDCFileLevel), String.Format("expected \'FRS2Model.RDC_Sig_Level.forRDCFileLevel\', actual \'{0}\' (rdcsigLevel of " +
                        "InitializeFileTransferAsyncEvent, state S175)", TestManagerHelpers.Describe(rdcsigLevel)));
            this.Manager.Assert((isEOF == true), String.Format("expected \'true\', actual \'{0}\' (isEOF of InitializeFileTransferAsyncEvent, state S" +
                        "175)", TestManagerHelpers.Describe(isEOF)));
        }
        
        private void TCtestScenario2S4InitializeFileTransferAsyncEventChecker3(FRS2Model.ServerContext context, FRS2Model.RDC_Sig_Level rdcsigLevel, bool isEOF) {
            this.Manager.Comment("checking step \'event InitializeFileTransferAsyncEvent(ValidContext,forRDCFileLeve" +
                    "l,False)\'");
            this.Manager.Assert((context == FRS2Model.ServerContext.ValidContext), String.Format("expected \'FRS2Model.ServerContext.ValidContext\', actual \'{0}\' (context of Initial" +
                        "izeFileTransferAsyncEvent, state S175)", TestManagerHelpers.Describe(context)));
            this.Manager.Assert((rdcsigLevel == FRS2Model.RDC_Sig_Level.forRDCFileLevel), String.Format("expected \'FRS2Model.RDC_Sig_Level.forRDCFileLevel\', actual \'{0}\' (rdcsigLevel of " +
                        "InitializeFileTransferAsyncEvent, state S175)", TestManagerHelpers.Describe(rdcsigLevel)));
            this.Manager.Assert((isEOF == false), String.Format("expected \'false\', actual \'{0}\' (isEOF of InitializeFileTransferAsyncEvent, state " +
                        "S175)", TestManagerHelpers.Describe(isEOF)));
        }
        
        private void TCtestScenario2S4InitializeFileTransferAsyncEventChecker4(FRS2Model.ServerContext context, FRS2Model.RDC_Sig_Level rdcsigLevel, bool isEOF) {
            this.Manager.Comment("checking step \'event InitializeFileTransferAsyncEvent(InvalidContext,forRDCFileLe" +
                    "vel,False)\'");
            this.Manager.Assert((context == FRS2Model.ServerContext.InvalidContext), String.Format("expected \'FRS2Model.ServerContext.InvalidContext\', actual \'{0}\' (context of Initi" +
                        "alizeFileTransferAsyncEvent, state S175)", TestManagerHelpers.Describe(context)));
            this.Manager.Assert((rdcsigLevel == FRS2Model.RDC_Sig_Level.forRDCFileLevel), String.Format("expected \'FRS2Model.RDC_Sig_Level.forRDCFileLevel\', actual \'{0}\' (rdcsigLevel of " +
                        "InitializeFileTransferAsyncEvent, state S175)", TestManagerHelpers.Describe(rdcsigLevel)));
            this.Manager.Assert((isEOF == false), String.Format("expected \'false\', actual \'{0}\' (isEOF of InitializeFileTransferAsyncEvent, state " +
                        "S175)", TestManagerHelpers.Describe(isEOF)));
        }
        
        private void TCtestScenario2S4InitializeFileTransferAsyncEventChecker5(FRS2Model.ServerContext context, FRS2Model.RDC_Sig_Level rdcsigLevel, bool isEOF) {
            this.Manager.Comment("checking step \'event InitializeFileTransferAsyncEvent(ValidContext,forRDCFileLeve" +
                    "l,True)\'");
            this.Manager.Assert((context == FRS2Model.ServerContext.ValidContext), String.Format("expected \'FRS2Model.ServerContext.ValidContext\', actual \'{0}\' (context of Initial" +
                        "izeFileTransferAsyncEvent, state S175)", TestManagerHelpers.Describe(context)));
            this.Manager.Assert((rdcsigLevel == FRS2Model.RDC_Sig_Level.forRDCFileLevel), String.Format("expected \'FRS2Model.RDC_Sig_Level.forRDCFileLevel\', actual \'{0}\' (rdcsigLevel of " +
                        "InitializeFileTransferAsyncEvent, state S175)", TestManagerHelpers.Describe(rdcsigLevel)));
            this.Manager.Assert((isEOF == true), String.Format("expected \'true\', actual \'{0}\' (isEOF of InitializeFileTransferAsyncEvent, state S" +
                        "175)", TestManagerHelpers.Describe(isEOF)));
        }
        
        private void TCtestScenario2S4InitializeFileTransferAsyncEventChecker6(FRS2Model.ServerContext context, FRS2Model.RDC_Sig_Level rdcsigLevel, bool isEOF) {
            this.Manager.Comment("checking step \'event InitializeFileTransferAsyncEvent(InvalidContext,forRAWFileLe" +
                    "vel,True)\'");
            this.Manager.Assert((context == FRS2Model.ServerContext.InvalidContext), String.Format("expected \'FRS2Model.ServerContext.InvalidContext\', actual \'{0}\' (context of Initi" +
                        "alizeFileTransferAsyncEvent, state S175)", TestManagerHelpers.Describe(context)));
            this.Manager.Assert((rdcsigLevel == FRS2Model.RDC_Sig_Level.forRAWFileLevel), String.Format("expected \'FRS2Model.RDC_Sig_Level.forRAWFileLevel\', actual \'{0}\' (rdcsigLevel of " +
                        "InitializeFileTransferAsyncEvent, state S175)", TestManagerHelpers.Describe(rdcsigLevel)));
            this.Manager.Assert((isEOF == true), String.Format("expected \'true\', actual \'{0}\' (isEOF of InitializeFileTransferAsyncEvent, state S" +
                        "175)", TestManagerHelpers.Describe(isEOF)));
        }
        
        private void TCtestScenario2S4InitializeFileTransferAsyncEventChecker7(FRS2Model.ServerContext context, FRS2Model.RDC_Sig_Level rdcsigLevel, bool isEOF) {
            this.Manager.Comment("checking step \'event InitializeFileTransferAsyncEvent(ValidContext,forRAWFileLeve" +
                    "l,True)\'");
            this.Manager.Assert((context == FRS2Model.ServerContext.ValidContext), String.Format("expected \'FRS2Model.ServerContext.ValidContext\', actual \'{0}\' (context of Initial" +
                        "izeFileTransferAsyncEvent, state S175)", TestManagerHelpers.Describe(context)));
            this.Manager.Assert((rdcsigLevel == FRS2Model.RDC_Sig_Level.forRAWFileLevel), String.Format("expected \'FRS2Model.RDC_Sig_Level.forRAWFileLevel\', actual \'{0}\' (rdcsigLevel of " +
                        "InitializeFileTransferAsyncEvent, state S175)", TestManagerHelpers.Describe(rdcsigLevel)));
            this.Manager.Assert((isEOF == true), String.Format("expected \'true\', actual \'{0}\' (isEOF of InitializeFileTransferAsyncEvent, state S" +
                        "175)", TestManagerHelpers.Describe(isEOF)));
        }
        
        private void TCtestScenario2S4RequestUpdatesEventChecker2(FRS2Model.FilePresense present, FRS2Model.UPDATE_STATUS updateStatus) {
            this.Manager.Comment("checking step \'event RequestUpdatesEvent(fileExists,UPDATE_STATUS_DONE)\'");
            try {
                this.Manager.Assert((present == FRS2Model.FilePresense.fileExists), String.Format("expected \'FRS2Model.FilePresense.fileExists\', actual \'{0}\' (present of RequestUpd" +
                            "atesEvent, state S144)", TestManagerHelpers.Describe(present)));
                this.Manager.Assert((updateStatus == FRS2Model.UPDATE_STATUS.UPDATE_STATUS_DONE), String.Format("expected \'FRS2Model.UPDATE_STATUS.UPDATE_STATUS_DONE\', actual \'{0}\' (updateStatus" +
                            " of RequestUpdatesEvent, state S144)", TestManagerHelpers.Describe(updateStatus)));
            }
            catch (TransactionFailedException ) {
                this.Manager.Comment("This step would have covered MS-FRS2_R93, MS-FRS2_R94");
                throw;
            }
            this.Manager.Checkpoint("MS-FRS2_R93");
            this.Manager.Checkpoint("MS-FRS2_R94");
        }
        
        private void TCtestScenario2S4InitializeFileTransferAsyncEventChecker8(FRS2Model.ServerContext context, FRS2Model.RDC_Sig_Level rdcsigLevel, bool isEOF) {
            this.Manager.Comment("checking step \'event InitializeFileTransferAsyncEvent(ValidContext,forRAWFileLeve" +
                    "l,True)\'");
            this.Manager.Assert((context == FRS2Model.ServerContext.ValidContext), String.Format("expected \'FRS2Model.ServerContext.ValidContext\', actual \'{0}\' (context of Initial" +
                        "izeFileTransferAsyncEvent, state S176)", TestManagerHelpers.Describe(context)));
            this.Manager.Assert((rdcsigLevel == FRS2Model.RDC_Sig_Level.forRAWFileLevel), String.Format("expected \'FRS2Model.RDC_Sig_Level.forRAWFileLevel\', actual \'{0}\' (rdcsigLevel of " +
                        "InitializeFileTransferAsyncEvent, state S176)", TestManagerHelpers.Describe(rdcsigLevel)));
            this.Manager.Assert((isEOF == true), String.Format("expected \'true\', actual \'{0}\' (isEOF of InitializeFileTransferAsyncEvent, state S" +
                        "176)", TestManagerHelpers.Describe(isEOF)));
        }
        
        private void TCtestScenario2S4InitializeFileTransferAsyncEventChecker9(FRS2Model.ServerContext context, FRS2Model.RDC_Sig_Level rdcsigLevel, bool isEOF) {
            this.Manager.Comment("checking step \'event InitializeFileTransferAsyncEvent(InvalidContext,forRAWFileLe" +
                    "vel,True)\'");
            this.Manager.Assert((context == FRS2Model.ServerContext.InvalidContext), String.Format("expected \'FRS2Model.ServerContext.InvalidContext\', actual \'{0}\' (context of Initi" +
                        "alizeFileTransferAsyncEvent, state S176)", TestManagerHelpers.Describe(context)));
            this.Manager.Assert((rdcsigLevel == FRS2Model.RDC_Sig_Level.forRAWFileLevel), String.Format("expected \'FRS2Model.RDC_Sig_Level.forRAWFileLevel\', actual \'{0}\' (rdcsigLevel of " +
                        "InitializeFileTransferAsyncEvent, state S176)", TestManagerHelpers.Describe(rdcsigLevel)));
            this.Manager.Assert((isEOF == true), String.Format("expected \'true\', actual \'{0}\' (isEOF of InitializeFileTransferAsyncEvent, state S" +
                        "176)", TestManagerHelpers.Describe(isEOF)));
        }
        
        private void TCtestScenario2S4InitializeFileTransferAsyncEventChecker10(FRS2Model.ServerContext context, FRS2Model.RDC_Sig_Level rdcsigLevel, bool isEOF) {
            this.Manager.Comment("checking step \'event InitializeFileTransferAsyncEvent(InvalidContext,forRDCFileLe" +
                    "vel,False)\'");
            this.Manager.Assert((context == FRS2Model.ServerContext.InvalidContext), String.Format("expected \'FRS2Model.ServerContext.InvalidContext\', actual \'{0}\' (context of Initi" +
                        "alizeFileTransferAsyncEvent, state S176)", TestManagerHelpers.Describe(context)));
            this.Manager.Assert((rdcsigLevel == FRS2Model.RDC_Sig_Level.forRDCFileLevel), String.Format("expected \'FRS2Model.RDC_Sig_Level.forRDCFileLevel\', actual \'{0}\' (rdcsigLevel of " +
                        "InitializeFileTransferAsyncEvent, state S176)", TestManagerHelpers.Describe(rdcsigLevel)));
            this.Manager.Assert((isEOF == false), String.Format("expected \'false\', actual \'{0}\' (isEOF of InitializeFileTransferAsyncEvent, state " +
                        "S176)", TestManagerHelpers.Describe(isEOF)));
        }
        
        private void TCtestScenario2S4InitializeFileTransferAsyncEventChecker11(FRS2Model.ServerContext context, FRS2Model.RDC_Sig_Level rdcsigLevel, bool isEOF) {
            this.Manager.Comment("checking step \'event InitializeFileTransferAsyncEvent(ValidContext,forRDCFileLeve" +
                    "l,True)\'");
            this.Manager.Assert((context == FRS2Model.ServerContext.ValidContext), String.Format("expected \'FRS2Model.ServerContext.ValidContext\', actual \'{0}\' (context of Initial" +
                        "izeFileTransferAsyncEvent, state S176)", TestManagerHelpers.Describe(context)));
            this.Manager.Assert((rdcsigLevel == FRS2Model.RDC_Sig_Level.forRDCFileLevel), String.Format("expected \'FRS2Model.RDC_Sig_Level.forRDCFileLevel\', actual \'{0}\' (rdcsigLevel of " +
                        "InitializeFileTransferAsyncEvent, state S176)", TestManagerHelpers.Describe(rdcsigLevel)));
            this.Manager.Assert((isEOF == true), String.Format("expected \'true\', actual \'{0}\' (isEOF of InitializeFileTransferAsyncEvent, state S" +
                        "176)", TestManagerHelpers.Describe(isEOF)));
        }
        
        private void TCtestScenario2S4InitializeFileTransferAsyncEventChecker12(FRS2Model.ServerContext context, FRS2Model.RDC_Sig_Level rdcsigLevel, bool isEOF) {
            this.Manager.Comment("checking step \'event InitializeFileTransferAsyncEvent(InvalidContext,forRDCFileLe" +
                    "vel,True)\'");
            this.Manager.Assert((context == FRS2Model.ServerContext.InvalidContext), String.Format("expected \'FRS2Model.ServerContext.InvalidContext\', actual \'{0}\' (context of Initi" +
                        "alizeFileTransferAsyncEvent, state S176)", TestManagerHelpers.Describe(context)));
            this.Manager.Assert((rdcsigLevel == FRS2Model.RDC_Sig_Level.forRDCFileLevel), String.Format("expected \'FRS2Model.RDC_Sig_Level.forRDCFileLevel\', actual \'{0}\' (rdcsigLevel of " +
                        "InitializeFileTransferAsyncEvent, state S176)", TestManagerHelpers.Describe(rdcsigLevel)));
            this.Manager.Assert((isEOF == true), String.Format("expected \'true\', actual \'{0}\' (isEOF of InitializeFileTransferAsyncEvent, state S" +
                        "176)", TestManagerHelpers.Describe(isEOF)));
        }
        
        private void TCtestScenario2S4InitializeFileTransferAsyncEventChecker13(FRS2Model.ServerContext context, FRS2Model.RDC_Sig_Level rdcsigLevel, bool isEOF) {
            this.Manager.Comment("checking step \'event InitializeFileTransferAsyncEvent(ValidContext,forRDCFileLeve" +
                    "l,False)\'");
            this.Manager.Assert((context == FRS2Model.ServerContext.ValidContext), String.Format("expected \'FRS2Model.ServerContext.ValidContext\', actual \'{0}\' (context of Initial" +
                        "izeFileTransferAsyncEvent, state S176)", TestManagerHelpers.Describe(context)));
            this.Manager.Assert((rdcsigLevel == FRS2Model.RDC_Sig_Level.forRDCFileLevel), String.Format("expected \'FRS2Model.RDC_Sig_Level.forRDCFileLevel\', actual \'{0}\' (rdcsigLevel of " +
                        "InitializeFileTransferAsyncEvent, state S176)", TestManagerHelpers.Describe(rdcsigLevel)));
            this.Manager.Assert((isEOF == false), String.Format("expected \'false\', actual \'{0}\' (isEOF of InitializeFileTransferAsyncEvent, state " +
                        "S176)", TestManagerHelpers.Describe(isEOF)));
        }
        
        private void TCtestScenario2S4InitializeFileTransferAsyncEventChecker14(FRS2Model.ServerContext context, FRS2Model.RDC_Sig_Level rdcsigLevel, bool isEOF) {
            this.Manager.Comment("checking step \'event InitializeFileTransferAsyncEvent(ValidContext,forRAWFileLeve" +
                    "l,False)\'");
            this.Manager.Assert((context == FRS2Model.ServerContext.ValidContext), String.Format("expected \'FRS2Model.ServerContext.ValidContext\', actual \'{0}\' (context of Initial" +
                        "izeFileTransferAsyncEvent, state S176)", TestManagerHelpers.Describe(context)));
            this.Manager.Assert((rdcsigLevel == FRS2Model.RDC_Sig_Level.forRAWFileLevel), String.Format("expected \'FRS2Model.RDC_Sig_Level.forRAWFileLevel\', actual \'{0}\' (rdcsigLevel of " +
                        "InitializeFileTransferAsyncEvent, state S176)", TestManagerHelpers.Describe(rdcsigLevel)));
            this.Manager.Assert((isEOF == false), String.Format("expected \'false\', actual \'{0}\' (isEOF of InitializeFileTransferAsyncEvent, state " +
                        "S176)", TestManagerHelpers.Describe(isEOF)));
        }
        
        private void TCtestScenario2S4InitializeFileTransferAsyncEventChecker15(FRS2Model.ServerContext context, FRS2Model.RDC_Sig_Level rdcsigLevel, bool isEOF) {
            this.Manager.Comment("checking step \'event InitializeFileTransferAsyncEvent(InvalidContext,forRAWFileLe" +
                    "vel,False)\'");
            this.Manager.Assert((context == FRS2Model.ServerContext.InvalidContext), String.Format("expected \'FRS2Model.ServerContext.InvalidContext\', actual \'{0}\' (context of Initi" +
                        "alizeFileTransferAsyncEvent, state S176)", TestManagerHelpers.Describe(context)));
            this.Manager.Assert((rdcsigLevel == FRS2Model.RDC_Sig_Level.forRAWFileLevel), String.Format("expected \'FRS2Model.RDC_Sig_Level.forRAWFileLevel\', actual \'{0}\' (rdcsigLevel of " +
                        "InitializeFileTransferAsyncEvent, state S176)", TestManagerHelpers.Describe(rdcsigLevel)));
            this.Manager.Assert((isEOF == false), String.Format("expected \'false\', actual \'{0}\' (isEOF of InitializeFileTransferAsyncEvent, state " +
                        "S176)", TestManagerHelpers.Describe(isEOF)));
        }
        
        private void TCtestScenario2S4RequestUpdatesEventChecker3(FRS2Model.FilePresense present, FRS2Model.UPDATE_STATUS updateStatus) {
            this.Manager.Comment("checking step \'event RequestUpdatesEvent(fileExists,UPDATE_STATUS_MORE)\'");
            try {
                this.Manager.Assert((present == FRS2Model.FilePresense.fileExists), String.Format("expected \'FRS2Model.FilePresense.fileExists\', actual \'{0}\' (present of RequestUpd" +
                            "atesEvent, state S144)", TestManagerHelpers.Describe(present)));
                this.Manager.Assert((updateStatus == FRS2Model.UPDATE_STATUS.UPDATE_STATUS_MORE), String.Format("expected \'FRS2Model.UPDATE_STATUS.UPDATE_STATUS_MORE\', actual \'{0}\' (updateStatus" +
                            " of RequestUpdatesEvent, state S144)", TestManagerHelpers.Describe(updateStatus)));
            }
            catch (TransactionFailedException ) {
                this.Manager.Comment("This step would have covered MS-FRS2_R93, MS-FRS2_R498");
                throw;
            }
            this.Manager.Checkpoint("MS-FRS2_R93");
            this.Manager.Checkpoint("MS-FRS2_R498");
        }
        
        private void TCtestScenario2S4InitializeFileTransferAsyncEventChecker16(FRS2Model.ServerContext context, FRS2Model.RDC_Sig_Level rdcsigLevel, bool isEOF) {
            this.Manager.Comment("checking step \'event InitializeFileTransferAsyncEvent(ValidContext,forRAWFileLeve" +
                    "l,True)\'");
            this.Manager.Assert((context == FRS2Model.ServerContext.ValidContext), String.Format("expected \'FRS2Model.ServerContext.ValidContext\', actual \'{0}\' (context of Initial" +
                        "izeFileTransferAsyncEvent, state S177)", TestManagerHelpers.Describe(context)));
            this.Manager.Assert((rdcsigLevel == FRS2Model.RDC_Sig_Level.forRAWFileLevel), String.Format("expected \'FRS2Model.RDC_Sig_Level.forRAWFileLevel\', actual \'{0}\' (rdcsigLevel of " +
                        "InitializeFileTransferAsyncEvent, state S177)", TestManagerHelpers.Describe(rdcsigLevel)));
            this.Manager.Assert((isEOF == true), String.Format("expected \'true\', actual \'{0}\' (isEOF of InitializeFileTransferAsyncEvent, state S" +
                        "177)", TestManagerHelpers.Describe(isEOF)));
        }
        
        private void TCtestScenario2S4InitializeFileTransferAsyncEventChecker17(FRS2Model.ServerContext context, FRS2Model.RDC_Sig_Level rdcsigLevel, bool isEOF) {
            this.Manager.Comment("checking step \'event InitializeFileTransferAsyncEvent(InvalidContext,forRAWFileLe" +
                    "vel,True)\'");
            this.Manager.Assert((context == FRS2Model.ServerContext.InvalidContext), String.Format("expected \'FRS2Model.ServerContext.InvalidContext\', actual \'{0}\' (context of Initi" +
                        "alizeFileTransferAsyncEvent, state S177)", TestManagerHelpers.Describe(context)));
            this.Manager.Assert((rdcsigLevel == FRS2Model.RDC_Sig_Level.forRAWFileLevel), String.Format("expected \'FRS2Model.RDC_Sig_Level.forRAWFileLevel\', actual \'{0}\' (rdcsigLevel of " +
                        "InitializeFileTransferAsyncEvent, state S177)", TestManagerHelpers.Describe(rdcsigLevel)));
            this.Manager.Assert((isEOF == true), String.Format("expected \'true\', actual \'{0}\' (isEOF of InitializeFileTransferAsyncEvent, state S" +
                        "177)", TestManagerHelpers.Describe(isEOF)));
        }
        
        private void TCtestScenario2S4InitializeFileTransferAsyncEventChecker18(FRS2Model.ServerContext context, FRS2Model.RDC_Sig_Level rdcsigLevel, bool isEOF) {
            this.Manager.Comment("checking step \'event InitializeFileTransferAsyncEvent(InvalidContext,forRDCFileLe" +
                    "vel,False)\'");
            this.Manager.Assert((context == FRS2Model.ServerContext.InvalidContext), String.Format("expected \'FRS2Model.ServerContext.InvalidContext\', actual \'{0}\' (context of Initi" +
                        "alizeFileTransferAsyncEvent, state S177)", TestManagerHelpers.Describe(context)));
            this.Manager.Assert((rdcsigLevel == FRS2Model.RDC_Sig_Level.forRDCFileLevel), String.Format("expected \'FRS2Model.RDC_Sig_Level.forRDCFileLevel\', actual \'{0}\' (rdcsigLevel of " +
                        "InitializeFileTransferAsyncEvent, state S177)", TestManagerHelpers.Describe(rdcsigLevel)));
            this.Manager.Assert((isEOF == false), String.Format("expected \'false\', actual \'{0}\' (isEOF of InitializeFileTransferAsyncEvent, state " +
                        "S177)", TestManagerHelpers.Describe(isEOF)));
        }
        
        private void TCtestScenario2S4InitializeFileTransferAsyncEventChecker19(FRS2Model.ServerContext context, FRS2Model.RDC_Sig_Level rdcsigLevel, bool isEOF) {
            this.Manager.Comment("checking step \'event InitializeFileTransferAsyncEvent(ValidContext,forRDCFileLeve" +
                    "l,True)\'");
            this.Manager.Assert((context == FRS2Model.ServerContext.ValidContext), String.Format("expected \'FRS2Model.ServerContext.ValidContext\', actual \'{0}\' (context of Initial" +
                        "izeFileTransferAsyncEvent, state S177)", TestManagerHelpers.Describe(context)));
            this.Manager.Assert((rdcsigLevel == FRS2Model.RDC_Sig_Level.forRDCFileLevel), String.Format("expected \'FRS2Model.RDC_Sig_Level.forRDCFileLevel\', actual \'{0}\' (rdcsigLevel of " +
                        "InitializeFileTransferAsyncEvent, state S177)", TestManagerHelpers.Describe(rdcsigLevel)));
            this.Manager.Assert((isEOF == true), String.Format("expected \'true\', actual \'{0}\' (isEOF of InitializeFileTransferAsyncEvent, state S" +
                        "177)", TestManagerHelpers.Describe(isEOF)));
        }
        
        private void TCtestScenario2S4InitializeFileTransferAsyncEventChecker20(FRS2Model.ServerContext context, FRS2Model.RDC_Sig_Level rdcsigLevel, bool isEOF) {
            this.Manager.Comment("checking step \'event InitializeFileTransferAsyncEvent(InvalidContext,forRDCFileLe" +
                    "vel,True)\'");
            this.Manager.Assert((context == FRS2Model.ServerContext.InvalidContext), String.Format("expected \'FRS2Model.ServerContext.InvalidContext\', actual \'{0}\' (context of Initi" +
                        "alizeFileTransferAsyncEvent, state S177)", TestManagerHelpers.Describe(context)));
            this.Manager.Assert((rdcsigLevel == FRS2Model.RDC_Sig_Level.forRDCFileLevel), String.Format("expected \'FRS2Model.RDC_Sig_Level.forRDCFileLevel\', actual \'{0}\' (rdcsigLevel of " +
                        "InitializeFileTransferAsyncEvent, state S177)", TestManagerHelpers.Describe(rdcsigLevel)));
            this.Manager.Assert((isEOF == true), String.Format("expected \'true\', actual \'{0}\' (isEOF of InitializeFileTransferAsyncEvent, state S" +
                        "177)", TestManagerHelpers.Describe(isEOF)));
        }
        
        private void TCtestScenario2S4InitializeFileTransferAsyncEventChecker21(FRS2Model.ServerContext context, FRS2Model.RDC_Sig_Level rdcsigLevel, bool isEOF) {
            this.Manager.Comment("checking step \'event InitializeFileTransferAsyncEvent(ValidContext,forRDCFileLeve" +
                    "l,False)\'");
            this.Manager.Assert((context == FRS2Model.ServerContext.ValidContext), String.Format("expected \'FRS2Model.ServerContext.ValidContext\', actual \'{0}\' (context of Initial" +
                        "izeFileTransferAsyncEvent, state S177)", TestManagerHelpers.Describe(context)));
            this.Manager.Assert((rdcsigLevel == FRS2Model.RDC_Sig_Level.forRDCFileLevel), String.Format("expected \'FRS2Model.RDC_Sig_Level.forRDCFileLevel\', actual \'{0}\' (rdcsigLevel of " +
                        "InitializeFileTransferAsyncEvent, state S177)", TestManagerHelpers.Describe(rdcsigLevel)));
            this.Manager.Assert((isEOF == false), String.Format("expected \'false\', actual \'{0}\' (isEOF of InitializeFileTransferAsyncEvent, state " +
                        "S177)", TestManagerHelpers.Describe(isEOF)));
        }
        
        private void TCtestScenario2S4InitializeFileTransferAsyncEventChecker22(FRS2Model.ServerContext context, FRS2Model.RDC_Sig_Level rdcsigLevel, bool isEOF) {
            this.Manager.Comment("checking step \'event InitializeFileTransferAsyncEvent(ValidContext,forRAWFileLeve" +
                    "l,False)\'");
            this.Manager.Assert((context == FRS2Model.ServerContext.ValidContext), String.Format("expected \'FRS2Model.ServerContext.ValidContext\', actual \'{0}\' (context of Initial" +
                        "izeFileTransferAsyncEvent, state S177)", TestManagerHelpers.Describe(context)));
            this.Manager.Assert((rdcsigLevel == FRS2Model.RDC_Sig_Level.forRAWFileLevel), String.Format("expected \'FRS2Model.RDC_Sig_Level.forRAWFileLevel\', actual \'{0}\' (rdcsigLevel of " +
                        "InitializeFileTransferAsyncEvent, state S177)", TestManagerHelpers.Describe(rdcsigLevel)));
            this.Manager.Assert((isEOF == false), String.Format("expected \'false\', actual \'{0}\' (isEOF of InitializeFileTransferAsyncEvent, state " +
                        "S177)", TestManagerHelpers.Describe(isEOF)));
        }
        
        private void TCtestScenario2S4InitializeFileTransferAsyncEventChecker23(FRS2Model.ServerContext context, FRS2Model.RDC_Sig_Level rdcsigLevel, bool isEOF) {
            this.Manager.Comment("checking step \'event InitializeFileTransferAsyncEvent(InvalidContext,forRAWFileLe" +
                    "vel,False)\'");
            this.Manager.Assert((context == FRS2Model.ServerContext.InvalidContext), String.Format("expected \'FRS2Model.ServerContext.InvalidContext\', actual \'{0}\' (context of Initi" +
                        "alizeFileTransferAsyncEvent, state S177)", TestManagerHelpers.Describe(context)));
            this.Manager.Assert((rdcsigLevel == FRS2Model.RDC_Sig_Level.forRAWFileLevel), String.Format("expected \'FRS2Model.RDC_Sig_Level.forRAWFileLevel\', actual \'{0}\' (rdcsigLevel of " +
                        "InitializeFileTransferAsyncEvent, state S177)", TestManagerHelpers.Describe(rdcsigLevel)));
            this.Manager.Assert((isEOF == false), String.Format("expected \'false\', actual \'{0}\' (isEOF of InitializeFileTransferAsyncEvent, state " +
                        "S177)", TestManagerHelpers.Describe(isEOF)));
        }
        
        private void TCtestScenario2S4AsyncPollResponseEventChecker1(FRS2Model.VVGeneration vvGen) {
            this.Manager.Comment("checking step \'event AsyncPollResponseEvent(InvalidValue)\'");
            try {
                this.Manager.Assert((vvGen == FRS2Model.VVGeneration.InvalidValue), String.Format("expected \'FRS2Model.VVGeneration.InvalidValue\', actual \'{0}\' (vvGen of AsyncPollR" +
                            "esponseEvent, state S124)", TestManagerHelpers.Describe(vvGen)));
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
        [Microsoft.VisualStudio.TestTools.UnitTesting.DescriptionAttribute(@"MS-FRS2_R37, MS-FRS2_R436, MS-FRS2_R439, MS-FRS2_R455, MS-FRS2_R458, MS-FRS2_R448, MS-FRS2_R549, MS-FRS2_R552, MS-FRS2_R517, MS-FRS2_R520, MS-FRS2_R466, MS-FRS2_R556, MS-FRS2_R1020, MS-FRS2_R555, MS-FRS2_R487, MS-FRS2_R492, MS-FRS2_R494, MS-FRS2_R556, MS-FRS2_R1020, MS-FRS2_R555")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestCategory("AD")]
        [TestCategory("MS-FRS2")]
        [TestCategory("SDC")]
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        
        public virtual void FRS2_TCtestScenario2S6()
        {
            this.Manager.BeginTest("TCtestScenario2S6");
            this.Manager.Comment("reaching state \'S6\'");
            FRS2Model.error_status_t temp64;
            this.Manager.Comment(@"executing step 'call Initialization(Windows7,Map{1=>enabled,2=>enabled,3=>enabled,4=>disabled},Map{1=>exists,2=>exists,3=>exists,4=>exists},Map{""A""=>1,""B""=>2,""C""=>3,""D""=>4},Map{""A""=>sysvol,""B""=>protection,""C""=>sysvol,""D""=>sysvol},Map{1=>""A"",2=>""B"",3=>""C"",4=>""D""},Map{1=>writeOnly,2=>readWrite,3=>readOnly,4=>readWrite},Map{1=>enabled,2=>disabled,3=>enabled,4=>enabled})'");
            temp64 = this.IFRS2ManagedAdapterInstance.Initialization(FRS2Model.OSVersion.Windows7, new Microsoft.Modeling.Map<System.Int32,FRS2Model.connectionProperty>().Add(1, FRS2Model.connectionProperty.enabled).Add(2, FRS2Model.connectionProperty.enabled).Add(3, FRS2Model.connectionProperty.enabled).Add(4, FRS2Model.connectionProperty.disabled), new Microsoft.Modeling.Map<System.Int32,FRS2Model.FromServer>().Add(1, FRS2Model.FromServer.exists).Add(2, FRS2Model.FromServer.exists).Add(3, FRS2Model.FromServer.exists).Add(4, FRS2Model.FromServer.exists), new Microsoft.Modeling.Map<System.String,System.Int32>().Add("A", 1).Add("B", 2).Add("C", 3).Add("D", 4), new Microsoft.Modeling.Map<System.String,FRS2Model.ReplicationGroupTypes>().Add("A", FRS2Model.ReplicationGroupTypes.sysvol).Add("B", FRS2Model.ReplicationGroupTypes.protection).Add("C", FRS2Model.ReplicationGroupTypes.sysvol).Add("D", FRS2Model.ReplicationGroupTypes.sysvol), new Microsoft.Modeling.Map<System.Int32,System.String>().Add(1, "A").Add(2, "B").Add(3, "C").Add(4, "D"), new Microsoft.Modeling.Map<System.Int32,FRS2Model.accessLevels>().Add(1, FRS2Model.accessLevels.writeOnly).Add(2, FRS2Model.accessLevels.readWrite).Add(3, FRS2Model.accessLevels.readOnly).Add(4, FRS2Model.accessLevels.readWrite), new Microsoft.Modeling.Map<System.Int32,FRS2Model.connectionProperty>().Add(1, FRS2Model.connectionProperty.enabled).Add(2, FRS2Model.connectionProperty.disabled).Add(3, FRS2Model.connectionProperty.enabled).Add(4, FRS2Model.connectionProperty.enabled));
            this.Manager.Comment("reaching state \'S7\'");
            this.Manager.Comment("checking step \'return Initialization/ERROR_SUCCESS\'");
            this.Manager.Assert((temp64 == FRS2Model.error_status_t.ERROR_SUCCESS), String.Format("expected \'FRS2Model.error_status_t.ERROR_SUCCESS\', actual \'{0}\' (return of Initia" +
                        "lization, state S7)", TestManagerHelpers.Describe(temp64)));
            this.Manager.Comment("reaching state \'S29\'");
            FRS2Model.ProtocolVersionReturned temp65;
            FRS2Model.UpstreamFlagValueReturned temp66;
            FRS2Model.error_status_t temp67;
            this.Manager.Comment("executing step \'call EstablishConnection(\"A\",1,FRS_COMMUNICATION_PROTOCOL_VERSION" +
                    "_LONGHORN_SERVER,out _,out _)\'");
            temp67 = this.IFRS2ManagedAdapterInstance.EstablishConnection("A", 1, FRS2Model.ProtocolVersion.FRS_COMMUNICATION_PROTOCOL_VERSION_LONGHORN_SERVER, out temp65, out temp66);
            this.Manager.Checkpoint("MS-FRS2_R37");
            this.Manager.Checkpoint("MS-FRS2_R436");
            this.Manager.Checkpoint("MS-FRS2_R439");
            this.Manager.Comment("reaching state \'S42\'");
            this.Manager.Comment("checking step \'return EstablishConnection/[out Valid,out Valid]:ERROR_SUCCESS\'");
            this.Manager.Assert((temp65 == FRS2Model.ProtocolVersionReturned.Valid), String.Format("expected \'FRS2Model.ProtocolVersionReturned.Valid\', actual \'{0}\' (upstreamProtoco" +
                        "lVersion of EstablishConnection, state S42)", TestManagerHelpers.Describe(temp65)));
            this.Manager.Assert((temp66 == FRS2Model.UpstreamFlagValueReturned.Valid), String.Format("expected \'FRS2Model.UpstreamFlagValueReturned.Valid\', actual \'{0}\' (upstreamFlags" +
                        " of EstablishConnection, state S42)", TestManagerHelpers.Describe(temp66)));
            this.Manager.Assert((temp67 == FRS2Model.error_status_t.ERROR_SUCCESS), String.Format("expected \'FRS2Model.error_status_t.ERROR_SUCCESS\', actual \'{0}\' (return of Establ" +
                        "ishConnection, state S42)", TestManagerHelpers.Describe(temp67)));
            this.Manager.Comment("reaching state \'S55\'");
            FRS2Model.error_status_t temp68;
            this.Manager.Comment("executing step \'call EstablishSession(1,1)\'");
            temp68 = this.IFRS2ManagedAdapterInstance.EstablishSession(1, 1);
            this.Manager.Checkpoint("MS-FRS2_R455");
            this.Manager.Checkpoint("MS-FRS2_R458");
            this.Manager.Checkpoint("MS-FRS2_R448");
            this.Manager.Comment("reaching state \'S68\'");
            this.Manager.Comment("checking step \'return EstablishSession/ERROR_SUCCESS\'");
            this.Manager.Assert((temp68 == FRS2Model.error_status_t.ERROR_SUCCESS), String.Format("expected \'FRS2Model.error_status_t.ERROR_SUCCESS\', actual \'{0}\' (return of Establ" +
                        "ishSession, state S68)", TestManagerHelpers.Describe(temp68)));
            this.Manager.Comment("reaching state \'S81\'");
            FRS2Model.error_status_t temp69;
            this.Manager.Comment("executing step \'call AsyncPoll(1)\'");
            temp69 = this.IFRS2ManagedAdapterInstance.AsyncPoll(1);
            this.Manager.Checkpoint("MS-FRS2_R549");
            this.Manager.Checkpoint("MS-FRS2_R552");
            this.Manager.Comment("reaching state \'S94\'");
            this.Manager.Comment("checking step \'return AsyncPoll/ERROR_SUCCESS\'");
            this.Manager.Assert((temp69 == FRS2Model.error_status_t.ERROR_SUCCESS), String.Format("expected \'FRS2Model.error_status_t.ERROR_SUCCESS\', actual \'{0}\' (return of AsyncP" +
                        "oll, state S94)", TestManagerHelpers.Describe(temp69)));
            this.Manager.Comment("reaching state \'S106\'");
            FRS2Model.error_status_t temp70;
            this.Manager.Comment("executing step \'call RequestVersionVector(1,1,1,REQUEST_NORMAL_SYNC,CHANGE_NOTIFY" +
                    ",ValidValue)\'");
            temp70 = this.IFRS2ManagedAdapterInstance.RequestVersionVector(1, 1, 1, FRS2Model.VERSION_REQUEST_TYPE.REQUEST_NORMAL_SYNC, FRS2Model.VERSION_CHANGE_TYPE.CHANGE_NOTIFY, FRS2Model.VVGeneration.ValidValue);
            this.Manager.Checkpoint("MS-FRS2_R517");
            this.Manager.Checkpoint("MS-FRS2_R520");
            this.Manager.Checkpoint("MS-FRS2_R466");
            this.Manager.Comment("reaching state \'S117\'");
            this.Manager.Comment("checking step \'return RequestVersionVector/ERROR_SUCCESS\'");
            this.Manager.Assert((temp70 == FRS2Model.error_status_t.ERROR_SUCCESS), String.Format("expected \'FRS2Model.error_status_t.ERROR_SUCCESS\', actual \'{0}\' (return of Reques" +
                        "tVersionVector, state S117)", TestManagerHelpers.Describe(temp70)));
            this.Manager.Comment("reaching state \'S125\'");
            int temp72 = this.Manager.ExpectEvent(this.QuiescenceTimeout, true, new ExpectedEvent(TCtestScenario2.AsyncPollResponseEventInfo, null, new AsyncPollResponseEventDelegate1(this.TCtestScenario2S6AsyncPollResponseEventChecker)), new ExpectedEvent(TCtestScenario2.AsyncPollResponseEventInfo, null, new AsyncPollResponseEventDelegate1(this.TCtestScenario2S6AsyncPollResponseEventChecker1)));
            if ((temp72 == 0)) {
                this.Manager.Comment("reaching state \'S135\'");
                FRS2Model.error_status_t temp71;
                this.Manager.Comment("executing step \'call RequestUpdates(4,1,inValid)\'");
                temp71 = this.IFRS2ManagedAdapterInstance.RequestUpdates(4, 1, FRS2Model.versionVectorDiff.inValid);
                this.Manager.Checkpoint("MS-FRS2_R487");
                this.Manager.Checkpoint("MS-FRS2_R492");
                this.Manager.Checkpoint("MS-FRS2_R494");
                this.Manager.Comment("reaching state \'S141\'");
                this.Manager.Comment("checking step \'return RequestUpdates/ERROR_FAIL\'");
                this.Manager.Assert((temp71 == FRS2Model.error_status_t.ERROR_FAIL), String.Format("expected \'FRS2Model.error_status_t.ERROR_FAIL\', actual \'{0}\' (return of RequestUp" +
                            "dates, state S141)", TestManagerHelpers.Describe(temp71)));
                this.Manager.Comment("reaching state \'S145\'");
                goto label7;
            }
            if ((temp72 == 1)) {
                TCtestScenario2S132();
                goto label7;
            }
            throw new InvalidOperationException("never reached");
        label7:
;
            this.Manager.EndTest();
        }
        
        private void TCtestScenario2S6AsyncPollResponseEventChecker(FRS2Model.VVGeneration vvGen) {
            this.Manager.Comment("checking step \'event AsyncPollResponseEvent(ValidValue)\'");
            try {
                this.Manager.Assert((vvGen == FRS2Model.VVGeneration.ValidValue), String.Format("expected \'FRS2Model.VVGeneration.ValidValue\', actual \'{0}\' (vvGen of AsyncPollRes" +
                            "ponseEvent, state S125)", TestManagerHelpers.Describe(vvGen)));
            }
            catch (TransactionFailedException ) {
                this.Manager.Comment("This step would have covered MS-FRS2_R556, MS-FRS2_R1020, MS-FRS2_R555");
                throw;
            }
            this.Manager.Checkpoint("MS-FRS2_R556");
            this.Manager.Checkpoint("MS-FRS2_R1020");
            this.Manager.Checkpoint("MS-FRS2_R555");
        }
        
        private void TCtestScenario2S6AsyncPollResponseEventChecker1(FRS2Model.VVGeneration vvGen) {
            this.Manager.Comment("checking step \'event AsyncPollResponseEvent(InvalidValue)\'");
            try {
                this.Manager.Assert((vvGen == FRS2Model.VVGeneration.InvalidValue), String.Format("expected \'FRS2Model.VVGeneration.InvalidValue\', actual \'{0}\' (vvGen of AsyncPollR" +
                            "esponseEvent, state S125)", TestManagerHelpers.Describe(vvGen)));
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
        [Microsoft.VisualStudio.TestTools.UnitTesting.DescriptionAttribute("MS-FRS2_R37, MS-FRS2_R436, MS-FRS2_R439, MS-FRS2_R455, MS-FRS2_R458, MS-FRS2_R448" +
            ", MS-FRS2_R549, MS-FRS2_R552, MS-FRS2_R518, MS-FRS2_R522, MS-FRS2_R523, MS-FRS2_" +
            "R529")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestCategory("AD")]
        [TestCategory("MS-FRS2")]
        [TestCategory("SDC")]
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        
        public virtual void FRS2_TCtestScenario2S8()
        {
            this.Manager.BeginTest("TCtestScenario2S8");
            this.Manager.Comment("reaching state \'S8\'");
            FRS2Model.error_status_t temp73;
            this.Manager.Comment(@"executing step 'call Initialization(Windows7,Map{1=>enabled,2=>enabled,3=>enabled,4=>disabled},Map{1=>exists,2=>exists,3=>exists,4=>exists},Map{""A""=>1,""B""=>2,""C""=>3,""D""=>4},Map{""A""=>sysvol,""B""=>protection,""C""=>sysvol,""D""=>sysvol},Map{1=>""A"",2=>""B"",3=>""C"",4=>""D""},Map{1=>writeOnly,2=>readWrite,3=>readOnly,4=>readWrite},Map{1=>enabled,2=>disabled,3=>enabled,4=>enabled})'");
            temp73 = this.IFRS2ManagedAdapterInstance.Initialization(FRS2Model.OSVersion.Windows7, new Microsoft.Modeling.Map<System.Int32,FRS2Model.connectionProperty>().Add(1, FRS2Model.connectionProperty.enabled).Add(2, FRS2Model.connectionProperty.enabled).Add(3, FRS2Model.connectionProperty.enabled).Add(4, FRS2Model.connectionProperty.disabled), new Microsoft.Modeling.Map<System.Int32,FRS2Model.FromServer>().Add(1, FRS2Model.FromServer.exists).Add(2, FRS2Model.FromServer.exists).Add(3, FRS2Model.FromServer.exists).Add(4, FRS2Model.FromServer.exists), new Microsoft.Modeling.Map<System.String,System.Int32>().Add("A", 1).Add("B", 2).Add("C", 3).Add("D", 4), new Microsoft.Modeling.Map<System.String,FRS2Model.ReplicationGroupTypes>().Add("A", FRS2Model.ReplicationGroupTypes.sysvol).Add("B", FRS2Model.ReplicationGroupTypes.protection).Add("C", FRS2Model.ReplicationGroupTypes.sysvol).Add("D", FRS2Model.ReplicationGroupTypes.sysvol), new Microsoft.Modeling.Map<System.Int32,System.String>().Add(1, "A").Add(2, "B").Add(3, "C").Add(4, "D"), new Microsoft.Modeling.Map<System.Int32,FRS2Model.accessLevels>().Add(1, FRS2Model.accessLevels.writeOnly).Add(2, FRS2Model.accessLevels.readWrite).Add(3, FRS2Model.accessLevels.readOnly).Add(4, FRS2Model.accessLevels.readWrite), new Microsoft.Modeling.Map<System.Int32,FRS2Model.connectionProperty>().Add(1, FRS2Model.connectionProperty.enabled).Add(2, FRS2Model.connectionProperty.disabled).Add(3, FRS2Model.connectionProperty.enabled).Add(4, FRS2Model.connectionProperty.enabled));
            this.Manager.Comment("reaching state \'S9\'");
            this.Manager.Comment("checking step \'return Initialization/ERROR_SUCCESS\'");
            this.Manager.Assert((temp73 == FRS2Model.error_status_t.ERROR_SUCCESS), String.Format("expected \'FRS2Model.error_status_t.ERROR_SUCCESS\', actual \'{0}\' (return of Initia" +
                        "lization, state S9)", TestManagerHelpers.Describe(temp73)));
            this.Manager.Comment("reaching state \'S30\'");
            FRS2Model.ProtocolVersionReturned temp74;
            FRS2Model.UpstreamFlagValueReturned temp75;
            FRS2Model.error_status_t temp76;
            this.Manager.Comment("executing step \'call EstablishConnection(\"A\",1,FRS_COMMUNICATION_PROTOCOL_VERSION" +
                    "_LONGHORN_SERVER,out _,out _)\'");
            temp76 = this.IFRS2ManagedAdapterInstance.EstablishConnection("A", 1, FRS2Model.ProtocolVersion.FRS_COMMUNICATION_PROTOCOL_VERSION_LONGHORN_SERVER, out temp74, out temp75);
            this.Manager.Checkpoint("MS-FRS2_R37");
            this.Manager.Checkpoint("MS-FRS2_R436");
            this.Manager.Checkpoint("MS-FRS2_R439");
            this.Manager.Comment("reaching state \'S43\'");
            this.Manager.Comment("checking step \'return EstablishConnection/[out Valid,out Valid]:ERROR_SUCCESS\'");
            this.Manager.Assert((temp74 == FRS2Model.ProtocolVersionReturned.Valid), String.Format("expected \'FRS2Model.ProtocolVersionReturned.Valid\', actual \'{0}\' (upstreamProtoco" +
                        "lVersion of EstablishConnection, state S43)", TestManagerHelpers.Describe(temp74)));
            this.Manager.Assert((temp75 == FRS2Model.UpstreamFlagValueReturned.Valid), String.Format("expected \'FRS2Model.UpstreamFlagValueReturned.Valid\', actual \'{0}\' (upstreamFlags" +
                        " of EstablishConnection, state S43)", TestManagerHelpers.Describe(temp75)));
            this.Manager.Assert((temp76 == FRS2Model.error_status_t.ERROR_SUCCESS), String.Format("expected \'FRS2Model.error_status_t.ERROR_SUCCESS\', actual \'{0}\' (return of Establ" +
                        "ishConnection, state S43)", TestManagerHelpers.Describe(temp76)));
            this.Manager.Comment("reaching state \'S56\'");
            FRS2Model.error_status_t temp77;
            this.Manager.Comment("executing step \'call EstablishSession(1,1)\'");
            temp77 = this.IFRS2ManagedAdapterInstance.EstablishSession(1, 1);
            this.Manager.Checkpoint("MS-FRS2_R455");
            this.Manager.Checkpoint("MS-FRS2_R458");
            this.Manager.Checkpoint("MS-FRS2_R448");
            this.Manager.Comment("reaching state \'S69\'");
            this.Manager.Comment("checking step \'return EstablishSession/ERROR_SUCCESS\'");
            this.Manager.Assert((temp77 == FRS2Model.error_status_t.ERROR_SUCCESS), String.Format("expected \'FRS2Model.error_status_t.ERROR_SUCCESS\', actual \'{0}\' (return of Establ" +
                        "ishSession, state S69)", TestManagerHelpers.Describe(temp77)));
            this.Manager.Comment("reaching state \'S82\'");
            FRS2Model.error_status_t temp78;
            this.Manager.Comment("executing step \'call AsyncPoll(1)\'");
            temp78 = this.IFRS2ManagedAdapterInstance.AsyncPoll(1);
            this.Manager.Checkpoint("MS-FRS2_R549");
            this.Manager.Checkpoint("MS-FRS2_R552");
            this.Manager.Comment("reaching state \'S95\'");
            this.Manager.Comment("checking step \'return AsyncPoll/ERROR_SUCCESS\'");
            this.Manager.Assert((temp78 == FRS2Model.error_status_t.ERROR_SUCCESS), String.Format("expected \'FRS2Model.error_status_t.ERROR_SUCCESS\', actual \'{0}\' (return of AsyncP" +
                        "oll, state S95)", TestManagerHelpers.Describe(temp78)));
            this.Manager.Comment("reaching state \'S107\'");
            FRS2Model.error_status_t temp79;
            this.Manager.Comment("executing step \'call RequestVersionVector(1,4,5,REQUEST_NORMAL_SYNC,CHANGE_ALL,In" +
                    "validValue)\'");
            temp79 = this.IFRS2ManagedAdapterInstance.RequestVersionVector(1, 4, 5, FRS2Model.VERSION_REQUEST_TYPE.REQUEST_NORMAL_SYNC, FRS2Model.VERSION_CHANGE_TYPE.CHANGE_ALL, FRS2Model.VVGeneration.InvalidValue);
            this.Manager.Checkpoint("MS-FRS2_R518");
            this.Manager.Checkpoint("MS-FRS2_R522");
            this.Manager.Checkpoint("MS-FRS2_R523");
            this.Manager.Checkpoint("MS-FRS2_R529");
            this.Manager.Comment("reaching state \'S118\'");
            this.Manager.Comment("checking step \'return RequestVersionVector/ERROR_FAIL\'");
            this.Manager.Assert((temp79 == FRS2Model.error_status_t.ERROR_FAIL), String.Format("expected \'FRS2Model.error_status_t.ERROR_FAIL\', actual \'{0}\' (return of RequestVe" +
                        "rsionVector, state S118)", TestManagerHelpers.Describe(temp79)));
            TCtestScenario2S126();
            this.Manager.EndTest();
        }
        
        private void TCtestScenario2S126() {
            this.Manager.Comment("reaching state \'S126\'");
        }
        #endregion
        
        #region Test Starting in S10
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        [Microsoft.VisualStudio.TestTools.UnitTesting.DescriptionAttribute(@"MS-FRS2_R37, MS-FRS2_R436, MS-FRS2_R439, MS-FRS2_R455, MS-FRS2_R458, MS-FRS2_R448, MS-FRS2_R549, MS-FRS2_R552, MS-FRS2_R517, MS-FRS2_R520, MS-FRS2_R466, MS-FRS2_R556, MS-FRS2_R1020, MS-FRS2_R555, MS-FRS2_R486, MS-FRS2_R489, MS-FRS2_R93, MS-FRS2_R498, MS-FRS2_R775, MS-FRS2_R779, MS-FRS2_R784, MS-FRS2_R93, MS-FRS2_R94, MS-FRS2_R775, MS-FRS2_R779, MS-FRS2_R784, MS-FRS2_R93, MS-FRS2_R94, MS-FRS2_R775, MS-FRS2_R779, MS-FRS2_R784, MS-FRS2_R93, MS-FRS2_R774, MS-FRS2_R777, MS-FRS2_R793, MS-FRS2_R556, MS-FRS2_R1020, MS-FRS2_R555")]
        public virtual void FRS2_TCtestScenario2S10() {
            this.Manager.BeginTest("TCtestScenario2S10");
            this.Manager.Comment("reaching state \'S10\'");
            FRS2Model.error_status_t temp80;
            this.Manager.Comment(@"executing step 'call Initialization(Windows7,Map{1=>enabled,2=>enabled,3=>enabled,4=>disabled},Map{1=>exists,2=>exists,3=>exists,4=>exists},Map{""A""=>1,""B""=>2,""C""=>3,""D""=>4},Map{""A""=>sysvol,""B""=>protection,""C""=>sysvol,""D""=>sysvol},Map{1=>""A"",2=>""B"",3=>""C"",4=>""D""},Map{1=>writeOnly,2=>readWrite,3=>readOnly,4=>readWrite},Map{1=>enabled,2=>disabled,3=>enabled,4=>enabled})'");
            temp80 = this.IFRS2ManagedAdapterInstance.Initialization(FRS2Model.OSVersion.Windows7, new Microsoft.Modeling.Map<System.Int32,FRS2Model.connectionProperty>().Add(1, FRS2Model.connectionProperty.enabled).Add(2, FRS2Model.connectionProperty.enabled).Add(3, FRS2Model.connectionProperty.enabled).Add(4, FRS2Model.connectionProperty.disabled), new Microsoft.Modeling.Map<System.Int32,FRS2Model.FromServer>().Add(1, FRS2Model.FromServer.exists).Add(2, FRS2Model.FromServer.exists).Add(3, FRS2Model.FromServer.exists).Add(4, FRS2Model.FromServer.exists), new Microsoft.Modeling.Map<System.String,System.Int32>().Add("A", 1).Add("B", 2).Add("C", 3).Add("D", 4), new Microsoft.Modeling.Map<System.String,FRS2Model.ReplicationGroupTypes>().Add("A", FRS2Model.ReplicationGroupTypes.sysvol).Add("B", FRS2Model.ReplicationGroupTypes.protection).Add("C", FRS2Model.ReplicationGroupTypes.sysvol).Add("D", FRS2Model.ReplicationGroupTypes.sysvol), new Microsoft.Modeling.Map<System.Int32,System.String>().Add(1, "A").Add(2, "B").Add(3, "C").Add(4, "D"), new Microsoft.Modeling.Map<System.Int32,FRS2Model.accessLevels>().Add(1, FRS2Model.accessLevels.writeOnly).Add(2, FRS2Model.accessLevels.readWrite).Add(3, FRS2Model.accessLevels.readOnly).Add(4, FRS2Model.accessLevels.readWrite), new Microsoft.Modeling.Map<System.Int32,FRS2Model.connectionProperty>().Add(1, FRS2Model.connectionProperty.enabled).Add(2, FRS2Model.connectionProperty.disabled).Add(3, FRS2Model.connectionProperty.enabled).Add(4, FRS2Model.connectionProperty.enabled));
            this.Manager.Comment("reaching state \'S11\'");
            this.Manager.Comment("checking step \'return Initialization/ERROR_SUCCESS\'");
            this.Manager.Assert((temp80 == FRS2Model.error_status_t.ERROR_SUCCESS), String.Format("expected \'FRS2Model.error_status_t.ERROR_SUCCESS\', actual \'{0}\' (return of Initia" +
                        "lization, state S11)", TestManagerHelpers.Describe(temp80)));
            this.Manager.Comment("reaching state \'S31\'");
            FRS2Model.ProtocolVersionReturned temp81;
            FRS2Model.UpstreamFlagValueReturned temp82;
            FRS2Model.error_status_t temp83;
            this.Manager.Comment("executing step \'call EstablishConnection(\"A\",1,FRS_COMMUNICATION_PROTOCOL_VERSION" +
                    "_LONGHORN_SERVER,out _,out _)\'");
            temp83 = this.IFRS2ManagedAdapterInstance.EstablishConnection("A", 1, FRS2Model.ProtocolVersion.FRS_COMMUNICATION_PROTOCOL_VERSION_LONGHORN_SERVER, out temp81, out temp82);
            this.Manager.Checkpoint("MS-FRS2_R37");
            this.Manager.Checkpoint("MS-FRS2_R436");
            this.Manager.Checkpoint("MS-FRS2_R439");
            this.Manager.Comment("reaching state \'S44\'");
            this.Manager.Comment("checking step \'return EstablishConnection/[out Valid,out Valid]:ERROR_SUCCESS\'");
            this.Manager.Assert((temp81 == FRS2Model.ProtocolVersionReturned.Valid), String.Format("expected \'FRS2Model.ProtocolVersionReturned.Valid\', actual \'{0}\' (upstreamProtoco" +
                        "lVersion of EstablishConnection, state S44)", TestManagerHelpers.Describe(temp81)));
            this.Manager.Assert((temp82 == FRS2Model.UpstreamFlagValueReturned.Valid), String.Format("expected \'FRS2Model.UpstreamFlagValueReturned.Valid\', actual \'{0}\' (upstreamFlags" +
                        " of EstablishConnection, state S44)", TestManagerHelpers.Describe(temp82)));
            this.Manager.Assert((temp83 == FRS2Model.error_status_t.ERROR_SUCCESS), String.Format("expected \'FRS2Model.error_status_t.ERROR_SUCCESS\', actual \'{0}\' (return of Establ" +
                        "ishConnection, state S44)", TestManagerHelpers.Describe(temp83)));
            this.Manager.Comment("reaching state \'S57\'");
            FRS2Model.error_status_t temp84;
            this.Manager.Comment("executing step \'call EstablishSession(1,1)\'");
            temp84 = this.IFRS2ManagedAdapterInstance.EstablishSession(1, 1);
            this.Manager.Checkpoint("MS-FRS2_R455");
            this.Manager.Checkpoint("MS-FRS2_R458");
            this.Manager.Checkpoint("MS-FRS2_R448");
            this.Manager.Comment("reaching state \'S70\'");
            this.Manager.Comment("checking step \'return EstablishSession/ERROR_SUCCESS\'");
            this.Manager.Assert((temp84 == FRS2Model.error_status_t.ERROR_SUCCESS), String.Format("expected \'FRS2Model.error_status_t.ERROR_SUCCESS\', actual \'{0}\' (return of Establ" +
                        "ishSession, state S70)", TestManagerHelpers.Describe(temp84)));
            this.Manager.Comment("reaching state \'S83\'");
            FRS2Model.error_status_t temp85;
            this.Manager.Comment("executing step \'call AsyncPoll(1)\'");
            temp85 = this.IFRS2ManagedAdapterInstance.AsyncPoll(1);
            this.Manager.Checkpoint("MS-FRS2_R549");
            this.Manager.Checkpoint("MS-FRS2_R552");
            this.Manager.Comment("reaching state \'S96\'");
            this.Manager.Comment("checking step \'return AsyncPoll/ERROR_SUCCESS\'");
            this.Manager.Assert((temp85 == FRS2Model.error_status_t.ERROR_SUCCESS), String.Format("expected \'FRS2Model.error_status_t.ERROR_SUCCESS\', actual \'{0}\' (return of AsyncP" +
                        "oll, state S96)", TestManagerHelpers.Describe(temp85)));
            this.Manager.Comment("reaching state \'S108\'");
            FRS2Model.error_status_t temp86;
            this.Manager.Comment("executing step \'call RequestVersionVector(1,1,1,REQUEST_NORMAL_SYNC,CHANGE_NOTIFY" +
                    ",ValidValue)\'");
            temp86 = this.IFRS2ManagedAdapterInstance.RequestVersionVector(1, 1, 1, FRS2Model.VERSION_REQUEST_TYPE.REQUEST_NORMAL_SYNC, FRS2Model.VERSION_CHANGE_TYPE.CHANGE_NOTIFY, FRS2Model.VVGeneration.ValidValue);
            this.Manager.Checkpoint("MS-FRS2_R517");
            this.Manager.Checkpoint("MS-FRS2_R520");
            this.Manager.Checkpoint("MS-FRS2_R466");
            this.Manager.Comment("reaching state \'S119\'");
            this.Manager.Comment("checking step \'return RequestVersionVector/ERROR_SUCCESS\'");
            this.Manager.Assert((temp86 == FRS2Model.error_status_t.ERROR_SUCCESS), String.Format("expected \'FRS2Model.error_status_t.ERROR_SUCCESS\', actual \'{0}\' (return of Reques" +
                        "tVersionVector, state S119)", TestManagerHelpers.Describe(temp86)));
            this.Manager.Comment("reaching state \'S127\'");
            int temp93 = this.Manager.ExpectEvent(this.QuiescenceTimeout, true, new ExpectedEvent(TCtestScenario2.AsyncPollResponseEventInfo, null, new AsyncPollResponseEventDelegate1(this.TCtestScenario2S10AsyncPollResponseEventChecker)), new ExpectedEvent(TCtestScenario2.AsyncPollResponseEventInfo, null, new AsyncPollResponseEventDelegate1(this.TCtestScenario2S10AsyncPollResponseEventChecker1)));
            if ((temp93 == 0)) {
                this.Manager.Comment("reaching state \'S137\'");
                FRS2Model.error_status_t temp87;
                this.Manager.Comment("executing step \'call RequestUpdates(1,1,valid)\'");
                temp87 = this.IFRS2ManagedAdapterInstance.RequestUpdates(1, 1, FRS2Model.versionVectorDiff.valid);
                this.Manager.Checkpoint("MS-FRS2_R486");
                this.Manager.Checkpoint("MS-FRS2_R489");
                this.Manager.Comment("reaching state \'S142\'");
                this.Manager.Comment("checking step \'return RequestUpdates/ERROR_SUCCESS\'");
                this.Manager.Assert((temp87 == FRS2Model.error_status_t.ERROR_SUCCESS), String.Format("expected \'FRS2Model.error_status_t.ERROR_SUCCESS\', actual \'{0}\' (return of Reques" +
                            "tUpdates, state S142)", TestManagerHelpers.Describe(temp87)));
                this.Manager.Comment("reaching state \'S146\'");
                int temp92 = this.Manager.ExpectEvent(this.QuiescenceTimeout, true, new ExpectedEvent(TCtestScenario2.RequestUpdatesEventInfo, null, new RequestUpdatesEventDelegate1(this.TCtestScenario2S10RequestUpdatesEventChecker)), new ExpectedEvent(TCtestScenario2.RequestUpdatesEventInfo, null, new RequestUpdatesEventDelegate1(this.TCtestScenario2S10RequestUpdatesEventChecker1)), new ExpectedEvent(TCtestScenario2.RequestUpdatesEventInfo, null, new RequestUpdatesEventDelegate1(this.TCtestScenario2S10RequestUpdatesEventChecker2)), new ExpectedEvent(TCtestScenario2.RequestUpdatesEventInfo, null, new RequestUpdatesEventDelegate1(this.TCtestScenario2S10RequestUpdatesEventChecker3)));
                if ((temp92 == 0)) {
                    this.Manager.Comment("reaching state \'S155\'");
                    FRS2Model.error_status_t temp88;
                    this.Manager.Comment("executing step \'call InitializeFileTransferAsync(1,5,False)\'");
                    temp88 = this.IFRS2ManagedAdapterInstance.InitializeFileTransferAsync(1, 5, false);
                    this.Manager.Checkpoint("MS-FRS2_R775");
                    this.Manager.Checkpoint("MS-FRS2_R779");
                    this.Manager.Checkpoint("MS-FRS2_R784");
                    this.Manager.Comment("reaching state \'S167\'");
                    this.Manager.Comment("checking step \'return InitializeFileTransferAsync/FRS_ERROR_CONTENTSET_NOT_FOUND\'" +
                            "");
                    this.Manager.Assert((temp88 == FRS2Model.error_status_t.FRS_ERROR_CONTENTSET_NOT_FOUND), String.Format("expected \'FRS2Model.error_status_t.FRS_ERROR_CONTENTSET_NOT_FOUND\', actual \'{0}\' " +
                                "(return of InitializeFileTransferAsync, state S167)", TestManagerHelpers.Describe(temp88)));
                    TCtestScenario2S173();
                    goto label8;
                }
                if ((temp92 == 1)) {
                    this.Manager.Comment("reaching state \'S156\'");
                    FRS2Model.error_status_t temp89;
                    this.Manager.Comment("executing step \'call InitializeFileTransferAsync(1,5,False)\'");
                    temp89 = this.IFRS2ManagedAdapterInstance.InitializeFileTransferAsync(1, 5, false);
                    this.Manager.Checkpoint("MS-FRS2_R775");
                    this.Manager.Checkpoint("MS-FRS2_R779");
                    this.Manager.Checkpoint("MS-FRS2_R784");
                    this.Manager.Comment("reaching state \'S168\'");
                    this.Manager.Comment("checking step \'return InitializeFileTransferAsync/FRS_ERROR_CONTENTSET_NOT_FOUND\'" +
                            "");
                    this.Manager.Assert((temp89 == FRS2Model.error_status_t.FRS_ERROR_CONTENTSET_NOT_FOUND), String.Format("expected \'FRS2Model.error_status_t.FRS_ERROR_CONTENTSET_NOT_FOUND\', actual \'{0}\' " +
                                "(return of InitializeFileTransferAsync, state S168)", TestManagerHelpers.Describe(temp89)));
                    TCtestScenario2S172();
                    goto label8;
                }
                if ((temp92 == 2)) {
                    this.Manager.Comment("reaching state \'S157\'");
                    FRS2Model.error_status_t temp90;
                    this.Manager.Comment("executing step \'call InitializeFileTransferAsync(1,5,False)\'");
                    temp90 = this.IFRS2ManagedAdapterInstance.InitializeFileTransferAsync(1, 5, false);
                    this.Manager.Checkpoint("MS-FRS2_R775");
                    this.Manager.Checkpoint("MS-FRS2_R779");
                    this.Manager.Checkpoint("MS-FRS2_R784");
                    this.Manager.AddReturn(InitializeFileTransferAsyncInfo, null, temp90);
                    TCtestScenario2S163();
                    goto label8;
                }
                if ((temp92 == 3)) {
                    this.Manager.Comment("reaching state \'S158\'");
                    FRS2Model.error_status_t temp91;
                    this.Manager.Comment("executing step \'call InitializeFileTransferAsync(1,1,False)\'");
                    temp91 = this.IFRS2ManagedAdapterInstance.InitializeFileTransferAsync(1, 1, false);
                    this.Manager.Checkpoint("MS-FRS2_R774");
                    this.Manager.Checkpoint("MS-FRS2_R777");
                    this.Manager.Checkpoint("MS-FRS2_R793");
                    this.Manager.AddReturn(InitializeFileTransferAsyncInfo, null, temp91);
                    TCtestScenario2S164();
                    goto label8;
                }
                throw new InvalidOperationException("never reached");
            label8:
;
                goto label9;
            }
            if ((temp93 == 1)) {
                TCtestScenario2S132();
                goto label9;
            }
            throw new InvalidOperationException("never reached");
        label9:
;
            this.Manager.EndTest();
        }
        
        private void TCtestScenario2S10AsyncPollResponseEventChecker(FRS2Model.VVGeneration vvGen) {
            this.Manager.Comment("checking step \'event AsyncPollResponseEvent(ValidValue)\'");
            try {
                this.Manager.Assert((vvGen == FRS2Model.VVGeneration.ValidValue), String.Format("expected \'FRS2Model.VVGeneration.ValidValue\', actual \'{0}\' (vvGen of AsyncPollRes" +
                            "ponseEvent, state S127)", TestManagerHelpers.Describe(vvGen)));
            }
            catch (TransactionFailedException ) {
                this.Manager.Comment("This step would have covered MS-FRS2_R556, MS-FRS2_R1020, MS-FRS2_R555");
                throw;
            }
            this.Manager.Checkpoint("MS-FRS2_R556");
            this.Manager.Checkpoint("MS-FRS2_R1020");
            this.Manager.Checkpoint("MS-FRS2_R555");
        }
        
        private void TCtestScenario2S10RequestUpdatesEventChecker(FRS2Model.FilePresense present, FRS2Model.UPDATE_STATUS updateStatus) {
            this.Manager.Comment("checking step \'event RequestUpdatesEvent(fileExists,UPDATE_STATUS_MORE)\'");
            try {
                this.Manager.Assert((present == FRS2Model.FilePresense.fileExists), String.Format("expected \'FRS2Model.FilePresense.fileExists\', actual \'{0}\' (present of RequestUpd" +
                            "atesEvent, state S146)", TestManagerHelpers.Describe(present)));
                this.Manager.Assert((updateStatus == FRS2Model.UPDATE_STATUS.UPDATE_STATUS_MORE), String.Format("expected \'FRS2Model.UPDATE_STATUS.UPDATE_STATUS_MORE\', actual \'{0}\' (updateStatus" +
                            " of RequestUpdatesEvent, state S146)", TestManagerHelpers.Describe(updateStatus)));
            }
            catch (TransactionFailedException ) {
                this.Manager.Comment("This step would have covered MS-FRS2_R93, MS-FRS2_R498");
                throw;
            }
            this.Manager.Checkpoint("MS-FRS2_R93");
            this.Manager.Checkpoint("MS-FRS2_R498");
        }
        
        private void TCtestScenario2S10RequestUpdatesEventChecker1(FRS2Model.FilePresense present, FRS2Model.UPDATE_STATUS updateStatus) {
            this.Manager.Comment("checking step \'event RequestUpdatesEvent(fileExists,UPDATE_STATUS_DONE)\'");
            try {
                this.Manager.Assert((present == FRS2Model.FilePresense.fileExists), String.Format("expected \'FRS2Model.FilePresense.fileExists\', actual \'{0}\' (present of RequestUpd" +
                            "atesEvent, state S146)", TestManagerHelpers.Describe(present)));
                this.Manager.Assert((updateStatus == FRS2Model.UPDATE_STATUS.UPDATE_STATUS_DONE), String.Format("expected \'FRS2Model.UPDATE_STATUS.UPDATE_STATUS_DONE\', actual \'{0}\' (updateStatus" +
                            " of RequestUpdatesEvent, state S146)", TestManagerHelpers.Describe(updateStatus)));
            }
            catch (TransactionFailedException ) {
                this.Manager.Comment("This step would have covered MS-FRS2_R93, MS-FRS2_R94");
                throw;
            }
            this.Manager.Checkpoint("MS-FRS2_R93");
            this.Manager.Checkpoint("MS-FRS2_R94");
        }
        
        private void TCtestScenario2S10RequestUpdatesEventChecker2(FRS2Model.FilePresense present, FRS2Model.UPDATE_STATUS updateStatus) {
            this.Manager.Comment("checking step \'event RequestUpdatesEvent(fileDeleted,UPDATE_STATUS_DONE)\'");
            try {
                this.Manager.Assert((present == FRS2Model.FilePresense.fileDeleted), String.Format("expected \'FRS2Model.FilePresense.fileDeleted\', actual \'{0}\' (present of RequestUp" +
                            "datesEvent, state S146)", TestManagerHelpers.Describe(present)));
                this.Manager.Assert((updateStatus == FRS2Model.UPDATE_STATUS.UPDATE_STATUS_DONE), String.Format("expected \'FRS2Model.UPDATE_STATUS.UPDATE_STATUS_DONE\', actual \'{0}\' (updateStatus" +
                            " of RequestUpdatesEvent, state S146)", TestManagerHelpers.Describe(updateStatus)));
            }
            catch (TransactionFailedException ) {
                this.Manager.Comment("This step would have covered MS-FRS2_R93, MS-FRS2_R94");
                throw;
            }
            this.Manager.Checkpoint("MS-FRS2_R93");
            this.Manager.Checkpoint("MS-FRS2_R94");
        }
        
        private void TCtestScenario2S10RequestUpdatesEventChecker3(FRS2Model.FilePresense present, FRS2Model.UPDATE_STATUS updateStatus) {
            this.Manager.Comment("checking step \'event RequestUpdatesEvent(fileDeleted,UPDATE_STATUS_MORE)\'");
            try {
                this.Manager.Assert((present == FRS2Model.FilePresense.fileDeleted), String.Format("expected \'FRS2Model.FilePresense.fileDeleted\', actual \'{0}\' (present of RequestUp" +
                            "datesEvent, state S146)", TestManagerHelpers.Describe(present)));
                this.Manager.Assert((updateStatus == FRS2Model.UPDATE_STATUS.UPDATE_STATUS_MORE), String.Format("expected \'FRS2Model.UPDATE_STATUS.UPDATE_STATUS_MORE\', actual \'{0}\' (updateStatus" +
                            " of RequestUpdatesEvent, state S146)", TestManagerHelpers.Describe(updateStatus)));
            }
            catch (TransactionFailedException ) {
                this.Manager.Comment("This step would have covered MS-FRS2_R93");
                throw;
            }
            this.Manager.Checkpoint("MS-FRS2_R93");
        }
        
        private void TCtestScenario2S10AsyncPollResponseEventChecker1(FRS2Model.VVGeneration vvGen) {
            this.Manager.Comment("checking step \'event AsyncPollResponseEvent(InvalidValue)\'");
            try {
                this.Manager.Assert((vvGen == FRS2Model.VVGeneration.InvalidValue), String.Format("expected \'FRS2Model.VVGeneration.InvalidValue\', actual \'{0}\' (vvGen of AsyncPollR" +
                            "esponseEvent, state S127)", TestManagerHelpers.Describe(vvGen)));
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
        [Microsoft.VisualStudio.TestTools.UnitTesting.DescriptionAttribute("MS-FRS2_R37, MS-FRS2_R436, MS-FRS2_R439, MS-FRS2_R455, MS-FRS2_R458, MS-FRS2_R448" +
            ", MS-FRS2_R549, MS-FRS2_R552, MS-FRS2_R518, MS-FRS2_R522, MS-FRS2_R530")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestCategory("AD")]
        [TestCategory("MS-FRS2")]
        [TestCategory("SDC")]
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]      
        public virtual void FRS2_TCtestScenario2S12()
        {
            this.Manager.BeginTest("TCtestScenario2S12");
            this.Manager.Comment("reaching state \'S12\'");
            FRS2Model.error_status_t temp94;
            this.Manager.Comment(@"executing step 'call Initialization(Windows7,Map{1=>enabled,2=>enabled,3=>enabled,4=>disabled},Map{1=>exists,2=>exists,3=>exists,4=>exists},Map{""A""=>1,""B""=>2,""C""=>3,""D""=>4},Map{""A""=>sysvol,""B""=>protection,""C""=>sysvol,""D""=>sysvol},Map{1=>""A"",2=>""B"",3=>""C"",4=>""D""},Map{1=>writeOnly,2=>readWrite,3=>readOnly,4=>readWrite},Map{1=>enabled,2=>disabled,3=>enabled,4=>enabled})'");
            temp94 = this.IFRS2ManagedAdapterInstance.Initialization(FRS2Model.OSVersion.Windows7, new Microsoft.Modeling.Map<System.Int32,FRS2Model.connectionProperty>().Add(1, FRS2Model.connectionProperty.enabled).Add(2, FRS2Model.connectionProperty.enabled).Add(3, FRS2Model.connectionProperty.enabled).Add(4, FRS2Model.connectionProperty.disabled), new Microsoft.Modeling.Map<System.Int32,FRS2Model.FromServer>().Add(1, FRS2Model.FromServer.exists).Add(2, FRS2Model.FromServer.exists).Add(3, FRS2Model.FromServer.exists).Add(4, FRS2Model.FromServer.exists), new Microsoft.Modeling.Map<System.String,System.Int32>().Add("A", 1).Add("B", 2).Add("C", 3).Add("D", 4), new Microsoft.Modeling.Map<System.String,FRS2Model.ReplicationGroupTypes>().Add("A", FRS2Model.ReplicationGroupTypes.sysvol).Add("B", FRS2Model.ReplicationGroupTypes.protection).Add("C", FRS2Model.ReplicationGroupTypes.sysvol).Add("D", FRS2Model.ReplicationGroupTypes.sysvol), new Microsoft.Modeling.Map<System.Int32,System.String>().Add(1, "A").Add(2, "B").Add(3, "C").Add(4, "D"), new Microsoft.Modeling.Map<System.Int32,FRS2Model.accessLevels>().Add(1, FRS2Model.accessLevels.writeOnly).Add(2, FRS2Model.accessLevels.readWrite).Add(3, FRS2Model.accessLevels.readOnly).Add(4, FRS2Model.accessLevels.readWrite), new Microsoft.Modeling.Map<System.Int32,FRS2Model.connectionProperty>().Add(1, FRS2Model.connectionProperty.enabled).Add(2, FRS2Model.connectionProperty.disabled).Add(3, FRS2Model.connectionProperty.enabled).Add(4, FRS2Model.connectionProperty.enabled));
            this.Manager.Comment("reaching state \'S13\'");
            this.Manager.Comment("checking step \'return Initialization/ERROR_SUCCESS\'");
            this.Manager.Assert((temp94 == FRS2Model.error_status_t.ERROR_SUCCESS), String.Format("expected \'FRS2Model.error_status_t.ERROR_SUCCESS\', actual \'{0}\' (return of Initia" +
                        "lization, state S13)", TestManagerHelpers.Describe(temp94)));
            this.Manager.Comment("reaching state \'S32\'");
            FRS2Model.ProtocolVersionReturned temp95;
            FRS2Model.UpstreamFlagValueReturned temp96;
            FRS2Model.error_status_t temp97;
            this.Manager.Comment("executing step \'call EstablishConnection(\"A\",1,FRS_COMMUNICATION_PROTOCOL_VERSION" +
                    "_LONGHORN_SERVER,out _,out _)\'");
            temp97 = this.IFRS2ManagedAdapterInstance.EstablishConnection("A", 1, FRS2Model.ProtocolVersion.FRS_COMMUNICATION_PROTOCOL_VERSION_LONGHORN_SERVER, out temp95, out temp96);
            this.Manager.Checkpoint("MS-FRS2_R37");
            this.Manager.Checkpoint("MS-FRS2_R436");
            this.Manager.Checkpoint("MS-FRS2_R439");
            this.Manager.Comment("reaching state \'S45\'");
            this.Manager.Comment("checking step \'return EstablishConnection/[out Valid,out Valid]:ERROR_SUCCESS\'");
            this.Manager.Assert((temp95 == FRS2Model.ProtocolVersionReturned.Valid), String.Format("expected \'FRS2Model.ProtocolVersionReturned.Valid\', actual \'{0}\' (upstreamProtoco" +
                        "lVersion of EstablishConnection, state S45)", TestManagerHelpers.Describe(temp95)));
            this.Manager.Assert((temp96 == FRS2Model.UpstreamFlagValueReturned.Valid), String.Format("expected \'FRS2Model.UpstreamFlagValueReturned.Valid\', actual \'{0}\' (upstreamFlags" +
                        " of EstablishConnection, state S45)", TestManagerHelpers.Describe(temp96)));
            this.Manager.Assert((temp97 == FRS2Model.error_status_t.ERROR_SUCCESS), String.Format("expected \'FRS2Model.error_status_t.ERROR_SUCCESS\', actual \'{0}\' (return of Establ" +
                        "ishConnection, state S45)", TestManagerHelpers.Describe(temp97)));
            this.Manager.Comment("reaching state \'S58\'");
            FRS2Model.error_status_t temp98;
            this.Manager.Comment("executing step \'call EstablishSession(1,1)\'");
            temp98 = this.IFRS2ManagedAdapterInstance.EstablishSession(1, 1);
            this.Manager.Checkpoint("MS-FRS2_R455");
            this.Manager.Checkpoint("MS-FRS2_R458");
            this.Manager.Checkpoint("MS-FRS2_R448");
            this.Manager.Comment("reaching state \'S71\'");
            this.Manager.Comment("checking step \'return EstablishSession/ERROR_SUCCESS\'");
            this.Manager.Assert((temp98 == FRS2Model.error_status_t.ERROR_SUCCESS), String.Format("expected \'FRS2Model.error_status_t.ERROR_SUCCESS\', actual \'{0}\' (return of Establ" +
                        "ishSession, state S71)", TestManagerHelpers.Describe(temp98)));
            this.Manager.Comment("reaching state \'S84\'");
            FRS2Model.error_status_t temp99;
            this.Manager.Comment("executing step \'call AsyncPoll(1)\'");
            temp99 = this.IFRS2ManagedAdapterInstance.AsyncPoll(1);
            this.Manager.Checkpoint("MS-FRS2_R549");
            this.Manager.Checkpoint("MS-FRS2_R552");
            this.Manager.Comment("reaching state \'S97\'");
            this.Manager.Comment("checking step \'return AsyncPoll/ERROR_SUCCESS\'");
            this.Manager.Assert((temp99 == FRS2Model.error_status_t.ERROR_SUCCESS), String.Format("expected \'FRS2Model.error_status_t.ERROR_SUCCESS\', actual \'{0}\' (return of AsyncP" +
                        "oll, state S97)", TestManagerHelpers.Describe(temp99)));
            this.Manager.Comment("reaching state \'S109\'");
            FRS2Model.error_status_t temp100;
            this.Manager.Comment("executing step \'call RequestVersionVector(1,4,1,REQUEST_NORMAL_SYNC,CHANGE_NOTIFY" +
                    ",InvalidValue)\'");
            temp100 = this.IFRS2ManagedAdapterInstance.RequestVersionVector(1, 4, 1, FRS2Model.VERSION_REQUEST_TYPE.REQUEST_NORMAL_SYNC, FRS2Model.VERSION_CHANGE_TYPE.CHANGE_NOTIFY, FRS2Model.VVGeneration.InvalidValue);
            this.Manager.Checkpoint("MS-FRS2_R518");
            this.Manager.Checkpoint("MS-FRS2_R522");
            this.Manager.Checkpoint("MS-FRS2_R530");
            this.Manager.Comment("reaching state \'S120\'");
            this.Manager.Comment("checking step \'return RequestVersionVector/ERROR_FAIL\'");
            this.Manager.Assert((temp100 == FRS2Model.error_status_t.ERROR_FAIL), String.Format("expected \'FRS2Model.error_status_t.ERROR_FAIL\', actual \'{0}\' (return of RequestVe" +
                        "rsionVector, state S120)", TestManagerHelpers.Describe(temp100)));
            TCtestScenario2S126();
            this.Manager.EndTest();
        }
        #endregion
        
        #region Test Starting in S14
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        [Microsoft.VisualStudio.TestTools.UnitTesting.DescriptionAttribute("MS-FRS2_R37, MS-FRS2_R436, MS-FRS2_R439, MS-FRS2_R455, MS-FRS2_R458, MS-FRS2_R448" +
            ", MS-FRS2_R549, MS-FRS2_R552, MS-FRS2_R518, MS-FRS2_R523, MS-FRS2_R529")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestCategory("AD")]
        [TestCategory("MS-FRS2")]
        [TestCategory("SDC")]
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]      
        public virtual void FRS2_TCtestScenario2S14()
        {
            this.Manager.BeginTest("TCtestScenario2S14");
            this.Manager.Comment("reaching state \'S14\'");
            FRS2Model.error_status_t temp101;
            this.Manager.Comment(@"executing step 'call Initialization(Windows7,Map{1=>enabled,2=>enabled,3=>enabled,4=>disabled},Map{1=>exists,2=>exists,3=>exists,4=>exists},Map{""A""=>1,""B""=>2,""C""=>3,""D""=>4},Map{""A""=>sysvol,""B""=>protection,""C""=>sysvol,""D""=>sysvol},Map{1=>""A"",2=>""B"",3=>""C"",4=>""D""},Map{1=>writeOnly,2=>readWrite,3=>readOnly,4=>readWrite},Map{1=>enabled,2=>disabled,3=>enabled,4=>enabled})'");
            temp101 = this.IFRS2ManagedAdapterInstance.Initialization(FRS2Model.OSVersion.Windows7, new Microsoft.Modeling.Map<System.Int32,FRS2Model.connectionProperty>().Add(1, FRS2Model.connectionProperty.enabled).Add(2, FRS2Model.connectionProperty.enabled).Add(3, FRS2Model.connectionProperty.enabled).Add(4, FRS2Model.connectionProperty.disabled), new Microsoft.Modeling.Map<System.Int32,FRS2Model.FromServer>().Add(1, FRS2Model.FromServer.exists).Add(2, FRS2Model.FromServer.exists).Add(3, FRS2Model.FromServer.exists).Add(4, FRS2Model.FromServer.exists), new Microsoft.Modeling.Map<System.String,System.Int32>().Add("A", 1).Add("B", 2).Add("C", 3).Add("D", 4), new Microsoft.Modeling.Map<System.String,FRS2Model.ReplicationGroupTypes>().Add("A", FRS2Model.ReplicationGroupTypes.sysvol).Add("B", FRS2Model.ReplicationGroupTypes.protection).Add("C", FRS2Model.ReplicationGroupTypes.sysvol).Add("D", FRS2Model.ReplicationGroupTypes.sysvol), new Microsoft.Modeling.Map<System.Int32,System.String>().Add(1, "A").Add(2, "B").Add(3, "C").Add(4, "D"), new Microsoft.Modeling.Map<System.Int32,FRS2Model.accessLevels>().Add(1, FRS2Model.accessLevels.writeOnly).Add(2, FRS2Model.accessLevels.readWrite).Add(3, FRS2Model.accessLevels.readOnly).Add(4, FRS2Model.accessLevels.readWrite), new Microsoft.Modeling.Map<System.Int32,FRS2Model.connectionProperty>().Add(1, FRS2Model.connectionProperty.enabled).Add(2, FRS2Model.connectionProperty.disabled).Add(3, FRS2Model.connectionProperty.enabled).Add(4, FRS2Model.connectionProperty.enabled));
            this.Manager.Comment("reaching state \'S15\'");
            this.Manager.Comment("checking step \'return Initialization/ERROR_SUCCESS\'");
            this.Manager.Assert((temp101 == FRS2Model.error_status_t.ERROR_SUCCESS), String.Format("expected \'FRS2Model.error_status_t.ERROR_SUCCESS\', actual \'{0}\' (return of Initia" +
                        "lization, state S15)", TestManagerHelpers.Describe(temp101)));
            this.Manager.Comment("reaching state \'S33\'");
            FRS2Model.ProtocolVersionReturned temp102;
            FRS2Model.UpstreamFlagValueReturned temp103;
            FRS2Model.error_status_t temp104;
            this.Manager.Comment("executing step \'call EstablishConnection(\"A\",1,FRS_COMMUNICATION_PROTOCOL_VERSION" +
                    "_LONGHORN_SERVER,out _,out _)\'");
            temp104 = this.IFRS2ManagedAdapterInstance.EstablishConnection("A", 1, FRS2Model.ProtocolVersion.FRS_COMMUNICATION_PROTOCOL_VERSION_LONGHORN_SERVER, out temp102, out temp103);
            this.Manager.Checkpoint("MS-FRS2_R37");
            this.Manager.Checkpoint("MS-FRS2_R436");
            this.Manager.Checkpoint("MS-FRS2_R439");
            this.Manager.Comment("reaching state \'S46\'");
            this.Manager.Comment("checking step \'return EstablishConnection/[out Valid,out Valid]:ERROR_SUCCESS\'");
            this.Manager.Assert((temp102 == FRS2Model.ProtocolVersionReturned.Valid), String.Format("expected \'FRS2Model.ProtocolVersionReturned.Valid\', actual \'{0}\' (upstreamProtoco" +
                        "lVersion of EstablishConnection, state S46)", TestManagerHelpers.Describe(temp102)));
            this.Manager.Assert((temp103 == FRS2Model.UpstreamFlagValueReturned.Valid), String.Format("expected \'FRS2Model.UpstreamFlagValueReturned.Valid\', actual \'{0}\' (upstreamFlags" +
                        " of EstablishConnection, state S46)", TestManagerHelpers.Describe(temp103)));
            this.Manager.Assert((temp104 == FRS2Model.error_status_t.ERROR_SUCCESS), String.Format("expected \'FRS2Model.error_status_t.ERROR_SUCCESS\', actual \'{0}\' (return of Establ" +
                        "ishConnection, state S46)", TestManagerHelpers.Describe(temp104)));
            this.Manager.Comment("reaching state \'S59\'");
            FRS2Model.error_status_t temp105;
            this.Manager.Comment("executing step \'call EstablishSession(1,1)\'");
            temp105 = this.IFRS2ManagedAdapterInstance.EstablishSession(1, 1);
            this.Manager.Checkpoint("MS-FRS2_R455");
            this.Manager.Checkpoint("MS-FRS2_R458");
            this.Manager.Checkpoint("MS-FRS2_R448");
            this.Manager.Comment("reaching state \'S72\'");
            this.Manager.Comment("checking step \'return EstablishSession/ERROR_SUCCESS\'");
            this.Manager.Assert((temp105 == FRS2Model.error_status_t.ERROR_SUCCESS), String.Format("expected \'FRS2Model.error_status_t.ERROR_SUCCESS\', actual \'{0}\' (return of Establ" +
                        "ishSession, state S72)", TestManagerHelpers.Describe(temp105)));
            this.Manager.Comment("reaching state \'S85\'");
            FRS2Model.error_status_t temp106;
            this.Manager.Comment("executing step \'call AsyncPoll(1)\'");
            temp106 = this.IFRS2ManagedAdapterInstance.AsyncPoll(1);
            this.Manager.Checkpoint("MS-FRS2_R549");
            this.Manager.Checkpoint("MS-FRS2_R552");
            this.Manager.Comment("reaching state \'S98\'");
            this.Manager.Comment("checking step \'return AsyncPoll/ERROR_SUCCESS\'");
            this.Manager.Assert((temp106 == FRS2Model.error_status_t.ERROR_SUCCESS), String.Format("expected \'FRS2Model.error_status_t.ERROR_SUCCESS\', actual \'{0}\' (return of AsyncP" +
                        "oll, state S98)", TestManagerHelpers.Describe(temp106)));
            this.Manager.Comment("reaching state \'S110\'");
            FRS2Model.error_status_t temp107;
            this.Manager.Comment("executing step \'call RequestVersionVector(1,1,5,REQUEST_NORMAL_SYNC,CHANGE_ALL,Va" +
                    "lidValue)\'");
            temp107 = this.IFRS2ManagedAdapterInstance.RequestVersionVector(1, 1, 5, FRS2Model.VERSION_REQUEST_TYPE.REQUEST_NORMAL_SYNC, FRS2Model.VERSION_CHANGE_TYPE.CHANGE_ALL, FRS2Model.VVGeneration.ValidValue);
            this.Manager.Checkpoint("MS-FRS2_R518");
            this.Manager.Checkpoint("MS-FRS2_R523");
            this.Manager.Checkpoint("MS-FRS2_R529");
            this.Manager.Comment("reaching state \'S121\'");
            this.Manager.Comment("checking step \'return RequestVersionVector/ERROR_FAIL\'");
            this.Manager.Assert((temp107 == FRS2Model.error_status_t.ERROR_FAIL), String.Format("expected \'FRS2Model.error_status_t.ERROR_FAIL\', actual \'{0}\' (return of RequestVe" +
                        "rsionVector, state S121)", TestManagerHelpers.Describe(temp107)));
            TCtestScenario2S126();
            this.Manager.EndTest();
        }
        #endregion
        
        #region Test Starting in S16
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        [Microsoft.VisualStudio.TestTools.UnitTesting.DescriptionAttribute("MS-FRS2_R37, MS-FRS2_R436, MS-FRS2_R439, MS-FRS2_R455, MS-FRS2_R458, MS-FRS2_R448" +
            ", MS-FRS2_R518, MS-FRS2_R522, MS-FRS2_R523, MS-FRS2_R529")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestCategory("AD")]
        [TestCategory("MS-FRS2")]
        [TestCategory("SDC")]
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]       
        public virtual void FRS2_TCtestScenario2S16()
        {
            this.Manager.BeginTest("TCtestScenario2S16");
            this.Manager.Comment("reaching state \'S16\'");
            FRS2Model.error_status_t temp108;
            this.Manager.Comment(@"executing step 'call Initialization(Windows7,Map{1=>enabled,2=>enabled,3=>enabled,4=>disabled},Map{1=>exists,2=>exists,3=>exists,4=>exists},Map{""A""=>1,""B""=>2,""C""=>3,""D""=>4},Map{""A""=>sysvol,""B""=>protection,""C""=>sysvol,""D""=>sysvol},Map{1=>""A"",2=>""B"",3=>""C"",4=>""D""},Map{1=>writeOnly,2=>readWrite,3=>readOnly,4=>readWrite},Map{1=>enabled,2=>disabled,3=>enabled,4=>enabled})'");
            temp108 = this.IFRS2ManagedAdapterInstance.Initialization(FRS2Model.OSVersion.Windows7, new Microsoft.Modeling.Map<System.Int32,FRS2Model.connectionProperty>().Add(1, FRS2Model.connectionProperty.enabled).Add(2, FRS2Model.connectionProperty.enabled).Add(3, FRS2Model.connectionProperty.enabled).Add(4, FRS2Model.connectionProperty.disabled), new Microsoft.Modeling.Map<System.Int32,FRS2Model.FromServer>().Add(1, FRS2Model.FromServer.exists).Add(2, FRS2Model.FromServer.exists).Add(3, FRS2Model.FromServer.exists).Add(4, FRS2Model.FromServer.exists), new Microsoft.Modeling.Map<System.String,System.Int32>().Add("A", 1).Add("B", 2).Add("C", 3).Add("D", 4), new Microsoft.Modeling.Map<System.String,FRS2Model.ReplicationGroupTypes>().Add("A", FRS2Model.ReplicationGroupTypes.sysvol).Add("B", FRS2Model.ReplicationGroupTypes.protection).Add("C", FRS2Model.ReplicationGroupTypes.sysvol).Add("D", FRS2Model.ReplicationGroupTypes.sysvol), new Microsoft.Modeling.Map<System.Int32,System.String>().Add(1, "A").Add(2, "B").Add(3, "C").Add(4, "D"), new Microsoft.Modeling.Map<System.Int32,FRS2Model.accessLevels>().Add(1, FRS2Model.accessLevels.writeOnly).Add(2, FRS2Model.accessLevels.readWrite).Add(3, FRS2Model.accessLevels.readOnly).Add(4, FRS2Model.accessLevels.readWrite), new Microsoft.Modeling.Map<System.Int32,FRS2Model.connectionProperty>().Add(1, FRS2Model.connectionProperty.enabled).Add(2, FRS2Model.connectionProperty.disabled).Add(3, FRS2Model.connectionProperty.enabled).Add(4, FRS2Model.connectionProperty.enabled));
            this.Manager.Comment("reaching state \'S17\'");
            this.Manager.Comment("checking step \'return Initialization/ERROR_SUCCESS\'");
            this.Manager.Assert((temp108 == FRS2Model.error_status_t.ERROR_SUCCESS), String.Format("expected \'FRS2Model.error_status_t.ERROR_SUCCESS\', actual \'{0}\' (return of Initia" +
                        "lization, state S17)", TestManagerHelpers.Describe(temp108)));
            this.Manager.Comment("reaching state \'S34\'");
            FRS2Model.ProtocolVersionReturned temp109;
            FRS2Model.UpstreamFlagValueReturned temp110;
            FRS2Model.error_status_t temp111;
            this.Manager.Comment("executing step \'call EstablishConnection(\"A\",1,FRS_COMMUNICATION_PROTOCOL_VERSION" +
                    "_LONGHORN_SERVER,out _,out _)\'");
            temp111 = this.IFRS2ManagedAdapterInstance.EstablishConnection("A", 1, FRS2Model.ProtocolVersion.FRS_COMMUNICATION_PROTOCOL_VERSION_LONGHORN_SERVER, out temp109, out temp110);
            this.Manager.Checkpoint("MS-FRS2_R37");
            this.Manager.Checkpoint("MS-FRS2_R436");
            this.Manager.Checkpoint("MS-FRS2_R439");
            this.Manager.Comment("reaching state \'S47\'");
            this.Manager.Comment("checking step \'return EstablishConnection/[out Valid,out Valid]:ERROR_SUCCESS\'");
            this.Manager.Assert((temp109 == FRS2Model.ProtocolVersionReturned.Valid), String.Format("expected \'FRS2Model.ProtocolVersionReturned.Valid\', actual \'{0}\' (upstreamProtoco" +
                        "lVersion of EstablishConnection, state S47)", TestManagerHelpers.Describe(temp109)));
            this.Manager.Assert((temp110 == FRS2Model.UpstreamFlagValueReturned.Valid), String.Format("expected \'FRS2Model.UpstreamFlagValueReturned.Valid\', actual \'{0}\' (upstreamFlags" +
                        " of EstablishConnection, state S47)", TestManagerHelpers.Describe(temp110)));
            this.Manager.Assert((temp111 == FRS2Model.error_status_t.ERROR_SUCCESS), String.Format("expected \'FRS2Model.error_status_t.ERROR_SUCCESS\', actual \'{0}\' (return of Establ" +
                        "ishConnection, state S47)", TestManagerHelpers.Describe(temp111)));
            this.Manager.Comment("reaching state \'S60\'");
            FRS2Model.error_status_t temp112;
            this.Manager.Comment("executing step \'call EstablishSession(1,1)\'");
            temp112 = this.IFRS2ManagedAdapterInstance.EstablishSession(1, 1);
            this.Manager.Checkpoint("MS-FRS2_R455");
            this.Manager.Checkpoint("MS-FRS2_R458");
            this.Manager.Checkpoint("MS-FRS2_R448");
            this.Manager.Comment("reaching state \'S73\'");
            this.Manager.Comment("checking step \'return EstablishSession/ERROR_SUCCESS\'");
            this.Manager.Assert((temp112 == FRS2Model.error_status_t.ERROR_SUCCESS), String.Format("expected \'FRS2Model.error_status_t.ERROR_SUCCESS\', actual \'{0}\' (return of Establ" +
                        "ishSession, state S73)", TestManagerHelpers.Describe(temp112)));
            this.Manager.Comment("reaching state \'S86\'");
            FRS2Model.error_status_t temp113;
            this.Manager.Comment("executing step \'call RequestVersionVector(1,4,5,REQUEST_NORMAL_SYNC,CHANGE_ALL,In" +
                    "validValue)\'");
            temp113 = this.IFRS2ManagedAdapterInstance.RequestVersionVector(1, 4, 5, FRS2Model.VERSION_REQUEST_TYPE.REQUEST_NORMAL_SYNC, FRS2Model.VERSION_CHANGE_TYPE.CHANGE_ALL, FRS2Model.VVGeneration.InvalidValue);
            this.Manager.Checkpoint("MS-FRS2_R518");
            this.Manager.Checkpoint("MS-FRS2_R522");
            this.Manager.Checkpoint("MS-FRS2_R523");
            this.Manager.Checkpoint("MS-FRS2_R529");
            this.Manager.Comment("reaching state \'S99\'");
            this.Manager.Comment("checking step \'return RequestVersionVector/ERROR_FAIL\'");
            this.Manager.Assert((temp113 == FRS2Model.error_status_t.ERROR_FAIL), String.Format("expected \'FRS2Model.error_status_t.ERROR_FAIL\', actual \'{0}\' (return of RequestVe" +
                        "rsionVector, state S99)", TestManagerHelpers.Describe(temp113)));
            TCtestScenario2S103();
            this.Manager.EndTest();
        }
        #endregion
        
        #region Test Starting in S18
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        [Microsoft.VisualStudio.TestTools.UnitTesting.DescriptionAttribute("MS-FRS2_R37, MS-FRS2_R436, MS-FRS2_R439, MS-FRS2_R455, MS-FRS2_R458, MS-FRS2_R448" +
            ", MS-FRS2_R549, MS-FRS2_R552, MS-FRS2_R518, MS-FRS2_R522, MS-FRS2_R523, MS-FRS2_" +
            "R530")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestCategory("AD")]
        [TestCategory("MS-FRS2")]
        [TestCategory("SDC")]
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]       
        public virtual void FRS2_TCtestScenario2S18()
        {
            this.Manager.BeginTest("TCtestScenario2S18");
            this.Manager.Comment("reaching state \'S18\'");
            FRS2Model.error_status_t temp114;
            this.Manager.Comment(@"executing step 'call Initialization(Windows7,Map{1=>enabled,2=>enabled,3=>enabled,4=>disabled},Map{1=>exists,2=>exists,3=>exists,4=>exists},Map{""A""=>1,""B""=>2,""C""=>3,""D""=>4},Map{""A""=>sysvol,""B""=>protection,""C""=>sysvol,""D""=>sysvol},Map{1=>""A"",2=>""B"",3=>""C"",4=>""D""},Map{1=>writeOnly,2=>readWrite,3=>readOnly,4=>readWrite},Map{1=>enabled,2=>disabled,3=>enabled,4=>enabled})'");
            temp114 = this.IFRS2ManagedAdapterInstance.Initialization(FRS2Model.OSVersion.Windows7, new Microsoft.Modeling.Map<System.Int32,FRS2Model.connectionProperty>().Add(1, FRS2Model.connectionProperty.enabled).Add(2, FRS2Model.connectionProperty.enabled).Add(3, FRS2Model.connectionProperty.enabled).Add(4, FRS2Model.connectionProperty.disabled), new Microsoft.Modeling.Map<System.Int32,FRS2Model.FromServer>().Add(1, FRS2Model.FromServer.exists).Add(2, FRS2Model.FromServer.exists).Add(3, FRS2Model.FromServer.exists).Add(4, FRS2Model.FromServer.exists), new Microsoft.Modeling.Map<System.String,System.Int32>().Add("A", 1).Add("B", 2).Add("C", 3).Add("D", 4), new Microsoft.Modeling.Map<System.String,FRS2Model.ReplicationGroupTypes>().Add("A", FRS2Model.ReplicationGroupTypes.sysvol).Add("B", FRS2Model.ReplicationGroupTypes.protection).Add("C", FRS2Model.ReplicationGroupTypes.sysvol).Add("D", FRS2Model.ReplicationGroupTypes.sysvol), new Microsoft.Modeling.Map<System.Int32,System.String>().Add(1, "A").Add(2, "B").Add(3, "C").Add(4, "D"), new Microsoft.Modeling.Map<System.Int32,FRS2Model.accessLevels>().Add(1, FRS2Model.accessLevels.writeOnly).Add(2, FRS2Model.accessLevels.readWrite).Add(3, FRS2Model.accessLevels.readOnly).Add(4, FRS2Model.accessLevels.readWrite), new Microsoft.Modeling.Map<System.Int32,FRS2Model.connectionProperty>().Add(1, FRS2Model.connectionProperty.enabled).Add(2, FRS2Model.connectionProperty.disabled).Add(3, FRS2Model.connectionProperty.enabled).Add(4, FRS2Model.connectionProperty.enabled));
            this.Manager.Comment("reaching state \'S19\'");
            this.Manager.Comment("checking step \'return Initialization/ERROR_SUCCESS\'");
            this.Manager.Assert((temp114 == FRS2Model.error_status_t.ERROR_SUCCESS), String.Format("expected \'FRS2Model.error_status_t.ERROR_SUCCESS\', actual \'{0}\' (return of Initia" +
                        "lization, state S19)", TestManagerHelpers.Describe(temp114)));
            this.Manager.Comment("reaching state \'S35\'");
            FRS2Model.ProtocolVersionReturned temp115;
            FRS2Model.UpstreamFlagValueReturned temp116;
            FRS2Model.error_status_t temp117;
            this.Manager.Comment("executing step \'call EstablishConnection(\"A\",1,FRS_COMMUNICATION_PROTOCOL_VERSION" +
                    "_LONGHORN_SERVER,out _,out _)\'");
            temp117 = this.IFRS2ManagedAdapterInstance.EstablishConnection("A", 1, FRS2Model.ProtocolVersion.FRS_COMMUNICATION_PROTOCOL_VERSION_LONGHORN_SERVER, out temp115, out temp116);
            this.Manager.Checkpoint("MS-FRS2_R37");
            this.Manager.Checkpoint("MS-FRS2_R436");
            this.Manager.Checkpoint("MS-FRS2_R439");
            this.Manager.Comment("reaching state \'S48\'");
            this.Manager.Comment("checking step \'return EstablishConnection/[out Valid,out Valid]:ERROR_SUCCESS\'");
            this.Manager.Assert((temp115 == FRS2Model.ProtocolVersionReturned.Valid), String.Format("expected \'FRS2Model.ProtocolVersionReturned.Valid\', actual \'{0}\' (upstreamProtoco" +
                        "lVersion of EstablishConnection, state S48)", TestManagerHelpers.Describe(temp115)));
            this.Manager.Assert((temp116 == FRS2Model.UpstreamFlagValueReturned.Valid), String.Format("expected \'FRS2Model.UpstreamFlagValueReturned.Valid\', actual \'{0}\' (upstreamFlags" +
                        " of EstablishConnection, state S48)", TestManagerHelpers.Describe(temp116)));
            this.Manager.Assert((temp117 == FRS2Model.error_status_t.ERROR_SUCCESS), String.Format("expected \'FRS2Model.error_status_t.ERROR_SUCCESS\', actual \'{0}\' (return of Establ" +
                        "ishConnection, state S48)", TestManagerHelpers.Describe(temp117)));
            this.Manager.Comment("reaching state \'S61\'");
            FRS2Model.error_status_t temp118;
            this.Manager.Comment("executing step \'call EstablishSession(1,1)\'");
            temp118 = this.IFRS2ManagedAdapterInstance.EstablishSession(1, 1);
            this.Manager.Checkpoint("MS-FRS2_R455");
            this.Manager.Checkpoint("MS-FRS2_R458");
            this.Manager.Checkpoint("MS-FRS2_R448");
            this.Manager.Comment("reaching state \'S74\'");
            this.Manager.Comment("checking step \'return EstablishSession/ERROR_SUCCESS\'");
            this.Manager.Assert((temp118 == FRS2Model.error_status_t.ERROR_SUCCESS), String.Format("expected \'FRS2Model.error_status_t.ERROR_SUCCESS\', actual \'{0}\' (return of Establ" +
                        "ishSession, state S74)", TestManagerHelpers.Describe(temp118)));
            this.Manager.Comment("reaching state \'S87\'");
            FRS2Model.error_status_t temp119;
            this.Manager.Comment("executing step \'call AsyncPoll(1)\'");
            temp119 = this.IFRS2ManagedAdapterInstance.AsyncPoll(1);
            this.Manager.Checkpoint("MS-FRS2_R549");
            this.Manager.Checkpoint("MS-FRS2_R552");
            this.Manager.Comment("reaching state \'S100\'");
            this.Manager.Comment("checking step \'return AsyncPoll/ERROR_SUCCESS\'");
            this.Manager.Assert((temp119 == FRS2Model.error_status_t.ERROR_SUCCESS), String.Format("expected \'FRS2Model.error_status_t.ERROR_SUCCESS\', actual \'{0}\' (return of AsyncP" +
                        "oll, state S100)", TestManagerHelpers.Describe(temp119)));
            this.Manager.Comment("reaching state \'S112\'");
            FRS2Model.error_status_t temp120;
            this.Manager.Comment("executing step \'call RequestVersionVector(1,4,5,REQUEST_NORMAL_SYNC,CHANGE_NOTIFY" +
                    ",ValidValue)\'");
            temp120 = this.IFRS2ManagedAdapterInstance.RequestVersionVector(1, 4, 5, FRS2Model.VERSION_REQUEST_TYPE.REQUEST_NORMAL_SYNC, FRS2Model.VERSION_CHANGE_TYPE.CHANGE_NOTIFY, FRS2Model.VVGeneration.ValidValue);
            this.Manager.Checkpoint("MS-FRS2_R518");
            this.Manager.Checkpoint("MS-FRS2_R522");
            this.Manager.Checkpoint("MS-FRS2_R523");
            this.Manager.Checkpoint("MS-FRS2_R530");
            this.Manager.Comment("reaching state \'S122\'");
            this.Manager.Comment("checking step \'return RequestVersionVector/ERROR_FAIL\'");
            this.Manager.Assert((temp120 == FRS2Model.error_status_t.ERROR_FAIL), String.Format("expected \'FRS2Model.error_status_t.ERROR_FAIL\', actual \'{0}\' (return of RequestVe" +
                        "rsionVector, state S122)", TestManagerHelpers.Describe(temp120)));
            TCtestScenario2S126();
            this.Manager.EndTest();
        }
        #endregion
        
        #region Test Starting in S20
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        [Microsoft.VisualStudio.TestTools.UnitTesting.DescriptionAttribute("MS-FRS2_R37, MS-FRS2_R436, MS-FRS2_R439, MS-FRS2_R455, MS-FRS2_R458, MS-FRS2_R448" +
            ", MS-FRS2_R518, MS-FRS2_R522, MS-FRS2_R530")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestCategory("AD")]
        [TestCategory("MS-FRS2")]
        [TestCategory("SDC")]
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]       
        public virtual void FRS2_TCtestScenario2S20()
        {
            this.Manager.BeginTest("TCtestScenario2S20");
            this.Manager.Comment("reaching state \'S20\'");
            FRS2Model.error_status_t temp121;
            this.Manager.Comment(@"executing step 'call Initialization(Windows7,Map{1=>enabled,2=>enabled,3=>enabled,4=>disabled},Map{1=>exists,2=>exists,3=>exists,4=>exists},Map{""A""=>1,""B""=>2,""C""=>3,""D""=>4},Map{""A""=>sysvol,""B""=>protection,""C""=>sysvol,""D""=>sysvol},Map{1=>""A"",2=>""B"",3=>""C"",4=>""D""},Map{1=>writeOnly,2=>readWrite,3=>readOnly,4=>readWrite},Map{1=>enabled,2=>disabled,3=>enabled,4=>enabled})'");
            temp121 = this.IFRS2ManagedAdapterInstance.Initialization(FRS2Model.OSVersion.Windows7, new Microsoft.Modeling.Map<System.Int32,FRS2Model.connectionProperty>().Add(1, FRS2Model.connectionProperty.enabled).Add(2, FRS2Model.connectionProperty.enabled).Add(3, FRS2Model.connectionProperty.enabled).Add(4, FRS2Model.connectionProperty.disabled), new Microsoft.Modeling.Map<System.Int32,FRS2Model.FromServer>().Add(1, FRS2Model.FromServer.exists).Add(2, FRS2Model.FromServer.exists).Add(3, FRS2Model.FromServer.exists).Add(4, FRS2Model.FromServer.exists), new Microsoft.Modeling.Map<System.String,System.Int32>().Add("A", 1).Add("B", 2).Add("C", 3).Add("D", 4), new Microsoft.Modeling.Map<System.String,FRS2Model.ReplicationGroupTypes>().Add("A", FRS2Model.ReplicationGroupTypes.sysvol).Add("B", FRS2Model.ReplicationGroupTypes.protection).Add("C", FRS2Model.ReplicationGroupTypes.sysvol).Add("D", FRS2Model.ReplicationGroupTypes.sysvol), new Microsoft.Modeling.Map<System.Int32,System.String>().Add(1, "A").Add(2, "B").Add(3, "C").Add(4, "D"), new Microsoft.Modeling.Map<System.Int32,FRS2Model.accessLevels>().Add(1, FRS2Model.accessLevels.writeOnly).Add(2, FRS2Model.accessLevels.readWrite).Add(3, FRS2Model.accessLevels.readOnly).Add(4, FRS2Model.accessLevels.readWrite), new Microsoft.Modeling.Map<System.Int32,FRS2Model.connectionProperty>().Add(1, FRS2Model.connectionProperty.enabled).Add(2, FRS2Model.connectionProperty.disabled).Add(3, FRS2Model.connectionProperty.enabled).Add(4, FRS2Model.connectionProperty.enabled));
            this.Manager.Comment("reaching state \'S21\'");
            this.Manager.Comment("checking step \'return Initialization/ERROR_SUCCESS\'");
            this.Manager.Assert((temp121 == FRS2Model.error_status_t.ERROR_SUCCESS), String.Format("expected \'FRS2Model.error_status_t.ERROR_SUCCESS\', actual \'{0}\' (return of Initia" +
                        "lization, state S21)", TestManagerHelpers.Describe(temp121)));
            this.Manager.Comment("reaching state \'S36\'");
            FRS2Model.ProtocolVersionReturned temp122;
            FRS2Model.UpstreamFlagValueReturned temp123;
            FRS2Model.error_status_t temp124;
            this.Manager.Comment("executing step \'call EstablishConnection(\"A\",1,FRS_COMMUNICATION_PROTOCOL_VERSION" +
                    "_LONGHORN_SERVER,out _,out _)\'");
            temp124 = this.IFRS2ManagedAdapterInstance.EstablishConnection("A", 1, FRS2Model.ProtocolVersion.FRS_COMMUNICATION_PROTOCOL_VERSION_LONGHORN_SERVER, out temp122, out temp123);
            this.Manager.Checkpoint("MS-FRS2_R37");
            this.Manager.Checkpoint("MS-FRS2_R436");
            this.Manager.Checkpoint("MS-FRS2_R439");
            this.Manager.Comment("reaching state \'S49\'");
            this.Manager.Comment("checking step \'return EstablishConnection/[out Valid,out Valid]:ERROR_SUCCESS\'");
            this.Manager.Assert((temp122 == FRS2Model.ProtocolVersionReturned.Valid), String.Format("expected \'FRS2Model.ProtocolVersionReturned.Valid\', actual \'{0}\' (upstreamProtoco" +
                        "lVersion of EstablishConnection, state S49)", TestManagerHelpers.Describe(temp122)));
            this.Manager.Assert((temp123 == FRS2Model.UpstreamFlagValueReturned.Valid), String.Format("expected \'FRS2Model.UpstreamFlagValueReturned.Valid\', actual \'{0}\' (upstreamFlags" +
                        " of EstablishConnection, state S49)", TestManagerHelpers.Describe(temp123)));
            this.Manager.Assert((temp124 == FRS2Model.error_status_t.ERROR_SUCCESS), String.Format("expected \'FRS2Model.error_status_t.ERROR_SUCCESS\', actual \'{0}\' (return of Establ" +
                        "ishConnection, state S49)", TestManagerHelpers.Describe(temp124)));
            this.Manager.Comment("reaching state \'S62\'");
            FRS2Model.error_status_t temp125;
            this.Manager.Comment("executing step \'call EstablishSession(1,1)\'");
            temp125 = this.IFRS2ManagedAdapterInstance.EstablishSession(1, 1);
            this.Manager.Checkpoint("MS-FRS2_R455");
            this.Manager.Checkpoint("MS-FRS2_R458");
            this.Manager.Checkpoint("MS-FRS2_R448");
            this.Manager.Comment("reaching state \'S75\'");
            this.Manager.Comment("checking step \'return EstablishSession/ERROR_SUCCESS\'");
            this.Manager.Assert((temp125 == FRS2Model.error_status_t.ERROR_SUCCESS), String.Format("expected \'FRS2Model.error_status_t.ERROR_SUCCESS\', actual \'{0}\' (return of Establ" +
                        "ishSession, state S75)", TestManagerHelpers.Describe(temp125)));
            this.Manager.Comment("reaching state \'S88\'");
            FRS2Model.error_status_t temp126;
            this.Manager.Comment("executing step \'call RequestVersionVector(1,4,1,REQUEST_NORMAL_SYNC,CHANGE_NOTIFY" +
                    ",InvalidValue)\'");
            temp126 = this.IFRS2ManagedAdapterInstance.RequestVersionVector(1, 4, 1, FRS2Model.VERSION_REQUEST_TYPE.REQUEST_NORMAL_SYNC, FRS2Model.VERSION_CHANGE_TYPE.CHANGE_NOTIFY, FRS2Model.VVGeneration.InvalidValue);
            this.Manager.Checkpoint("MS-FRS2_R518");
            this.Manager.Checkpoint("MS-FRS2_R522");
            this.Manager.Checkpoint("MS-FRS2_R530");
            this.Manager.Comment("reaching state \'S101\'");
            this.Manager.Comment("checking step \'return RequestVersionVector/ERROR_FAIL\'");
            this.Manager.Assert((temp126 == FRS2Model.error_status_t.ERROR_FAIL), String.Format("expected \'FRS2Model.error_status_t.ERROR_FAIL\', actual \'{0}\' (return of RequestVe" +
                        "rsionVector, state S101)", TestManagerHelpers.Describe(temp126)));
            TCtestScenario2S103();
            this.Manager.EndTest();
        }
        #endregion
        
        #region Test Starting in S22
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        [Microsoft.VisualStudio.TestTools.UnitTesting.DescriptionAttribute("MS-FRS2_R37, MS-FRS2_R436, MS-FRS2_R439, MS-FRS2_R455, MS-FRS2_R458, MS-FRS2_R448" +
            ", MS-FRS2_R518, MS-FRS2_R523, MS-FRS2_R529")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestCategory("AD")]
        [TestCategory("MS-FRS2")]
        [TestCategory("SDC")]
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]      
        public virtual void FRS2_TCtestScenario2S22()
        {
            this.Manager.BeginTest("TCtestScenario2S22");
            this.Manager.Comment("reaching state \'S22\'");
            FRS2Model.error_status_t temp127;
            this.Manager.Comment(@"executing step 'call Initialization(Windows7,Map{1=>enabled,2=>enabled,3=>enabled,4=>disabled},Map{1=>exists,2=>exists,3=>exists,4=>exists},Map{""A""=>1,""B""=>2,""C""=>3,""D""=>4},Map{""A""=>sysvol,""B""=>protection,""C""=>sysvol,""D""=>sysvol},Map{1=>""A"",2=>""B"",3=>""C"",4=>""D""},Map{1=>writeOnly,2=>readWrite,3=>readOnly,4=>readWrite},Map{1=>enabled,2=>disabled,3=>enabled,4=>enabled})'");
            temp127 = this.IFRS2ManagedAdapterInstance.Initialization(FRS2Model.OSVersion.Windows7, new Microsoft.Modeling.Map<System.Int32,FRS2Model.connectionProperty>().Add(1, FRS2Model.connectionProperty.enabled).Add(2, FRS2Model.connectionProperty.enabled).Add(3, FRS2Model.connectionProperty.enabled).Add(4, FRS2Model.connectionProperty.disabled), new Microsoft.Modeling.Map<System.Int32,FRS2Model.FromServer>().Add(1, FRS2Model.FromServer.exists).Add(2, FRS2Model.FromServer.exists).Add(3, FRS2Model.FromServer.exists).Add(4, FRS2Model.FromServer.exists), new Microsoft.Modeling.Map<System.String,System.Int32>().Add("A", 1).Add("B", 2).Add("C", 3).Add("D", 4), new Microsoft.Modeling.Map<System.String,FRS2Model.ReplicationGroupTypes>().Add("A", FRS2Model.ReplicationGroupTypes.sysvol).Add("B", FRS2Model.ReplicationGroupTypes.protection).Add("C", FRS2Model.ReplicationGroupTypes.sysvol).Add("D", FRS2Model.ReplicationGroupTypes.sysvol), new Microsoft.Modeling.Map<System.Int32,System.String>().Add(1, "A").Add(2, "B").Add(3, "C").Add(4, "D"), new Microsoft.Modeling.Map<System.Int32,FRS2Model.accessLevels>().Add(1, FRS2Model.accessLevels.writeOnly).Add(2, FRS2Model.accessLevels.readWrite).Add(3, FRS2Model.accessLevels.readOnly).Add(4, FRS2Model.accessLevels.readWrite), new Microsoft.Modeling.Map<System.Int32,FRS2Model.connectionProperty>().Add(1, FRS2Model.connectionProperty.enabled).Add(2, FRS2Model.connectionProperty.disabled).Add(3, FRS2Model.connectionProperty.enabled).Add(4, FRS2Model.connectionProperty.enabled));
            this.Manager.Comment("reaching state \'S23\'");
            this.Manager.Comment("checking step \'return Initialization/ERROR_SUCCESS\'");
            this.Manager.Assert((temp127 == FRS2Model.error_status_t.ERROR_SUCCESS), String.Format("expected \'FRS2Model.error_status_t.ERROR_SUCCESS\', actual \'{0}\' (return of Initia" +
                        "lization, state S23)", TestManagerHelpers.Describe(temp127)));
            this.Manager.Comment("reaching state \'S37\'");
            FRS2Model.ProtocolVersionReturned temp128;
            FRS2Model.UpstreamFlagValueReturned temp129;
            FRS2Model.error_status_t temp130;
            this.Manager.Comment("executing step \'call EstablishConnection(\"A\",1,FRS_COMMUNICATION_PROTOCOL_VERSION" +
                    "_LONGHORN_SERVER,out _,out _)\'");
            temp130 = this.IFRS2ManagedAdapterInstance.EstablishConnection("A", 1, FRS2Model.ProtocolVersion.FRS_COMMUNICATION_PROTOCOL_VERSION_LONGHORN_SERVER, out temp128, out temp129);
            this.Manager.Checkpoint("MS-FRS2_R37");
            this.Manager.Checkpoint("MS-FRS2_R436");
            this.Manager.Checkpoint("MS-FRS2_R439");
            this.Manager.Comment("reaching state \'S50\'");
            this.Manager.Comment("checking step \'return EstablishConnection/[out Valid,out Valid]:ERROR_SUCCESS\'");
            this.Manager.Assert((temp128 == FRS2Model.ProtocolVersionReturned.Valid), String.Format("expected \'FRS2Model.ProtocolVersionReturned.Valid\', actual \'{0}\' (upstreamProtoco" +
                        "lVersion of EstablishConnection, state S50)", TestManagerHelpers.Describe(temp128)));
            this.Manager.Assert((temp129 == FRS2Model.UpstreamFlagValueReturned.Valid), String.Format("expected \'FRS2Model.UpstreamFlagValueReturned.Valid\', actual \'{0}\' (upstreamFlags" +
                        " of EstablishConnection, state S50)", TestManagerHelpers.Describe(temp129)));
            this.Manager.Assert((temp130 == FRS2Model.error_status_t.ERROR_SUCCESS), String.Format("expected \'FRS2Model.error_status_t.ERROR_SUCCESS\', actual \'{0}\' (return of Establ" +
                        "ishConnection, state S50)", TestManagerHelpers.Describe(temp130)));
            this.Manager.Comment("reaching state \'S63\'");
            FRS2Model.error_status_t temp131;
            this.Manager.Comment("executing step \'call EstablishSession(1,1)\'");
            temp131 = this.IFRS2ManagedAdapterInstance.EstablishSession(1, 1);
            this.Manager.Checkpoint("MS-FRS2_R455");
            this.Manager.Checkpoint("MS-FRS2_R458");
            this.Manager.Checkpoint("MS-FRS2_R448");
            this.Manager.Comment("reaching state \'S76\'");
            this.Manager.Comment("checking step \'return EstablishSession/ERROR_SUCCESS\'");
            this.Manager.Assert((temp131 == FRS2Model.error_status_t.ERROR_SUCCESS), String.Format("expected \'FRS2Model.error_status_t.ERROR_SUCCESS\', actual \'{0}\' (return of Establ" +
                        "ishSession, state S76)", TestManagerHelpers.Describe(temp131)));
            this.Manager.Comment("reaching state \'S89\'");
            FRS2Model.error_status_t temp132;
            this.Manager.Comment("executing step \'call RequestVersionVector(1,1,5,REQUEST_NORMAL_SYNC,CHANGE_ALL,Va" +
                    "lidValue)\'");
            temp132 = this.IFRS2ManagedAdapterInstance.RequestVersionVector(1, 1, 5, FRS2Model.VERSION_REQUEST_TYPE.REQUEST_NORMAL_SYNC, FRS2Model.VERSION_CHANGE_TYPE.CHANGE_ALL, FRS2Model.VVGeneration.ValidValue);
            this.Manager.Checkpoint("MS-FRS2_R518");
            this.Manager.Checkpoint("MS-FRS2_R523");
            this.Manager.Checkpoint("MS-FRS2_R529");
            this.Manager.Comment("reaching state \'S102\'");
            this.Manager.Comment("checking step \'return RequestVersionVector/ERROR_FAIL\'");
            this.Manager.Assert((temp132 == FRS2Model.error_status_t.ERROR_FAIL), String.Format("expected \'FRS2Model.error_status_t.ERROR_FAIL\', actual \'{0}\' (return of RequestVe" +
                        "rsionVector, state S102)", TestManagerHelpers.Describe(temp132)));
            TCtestScenario2S103();
            this.Manager.EndTest();
        }
        #endregion
        
        #region Test Starting in S24
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        [Microsoft.VisualStudio.TestTools.UnitTesting.DescriptionAttribute("MS-FRS2_R37, MS-FRS2_R436, MS-FRS2_R439, MS-FRS2_R549, MS-FRS2_R552")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestCategory("AD")]
        [TestCategory("MS-FRS2")]
        [TestCategory("SDC")]
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]    
        public virtual void FRS2_TCtestScenario2S24()
        {
            this.Manager.BeginTest("TCtestScenario2S24");
            this.Manager.Comment("reaching state \'S24\'");
            FRS2Model.error_status_t temp133;
            this.Manager.Comment(@"executing step 'call Initialization(Windows7,Map{1=>enabled,2=>enabled,3=>enabled,4=>disabled},Map{1=>exists,2=>exists,3=>exists,4=>exists},Map{""A""=>1,""B""=>2,""C""=>3,""D""=>4},Map{""A""=>sysvol,""B""=>protection,""C""=>sysvol,""D""=>sysvol},Map{1=>""A"",2=>""B"",3=>""C"",4=>""D""},Map{1=>writeOnly,2=>readWrite,3=>readOnly,4=>readWrite},Map{1=>enabled,2=>disabled,3=>enabled,4=>enabled})'");
            temp133 = this.IFRS2ManagedAdapterInstance.Initialization(FRS2Model.OSVersion.Windows7, new Microsoft.Modeling.Map<System.Int32,FRS2Model.connectionProperty>().Add(1, FRS2Model.connectionProperty.enabled).Add(2, FRS2Model.connectionProperty.enabled).Add(3, FRS2Model.connectionProperty.enabled).Add(4, FRS2Model.connectionProperty.disabled), new Microsoft.Modeling.Map<System.Int32,FRS2Model.FromServer>().Add(1, FRS2Model.FromServer.exists).Add(2, FRS2Model.FromServer.exists).Add(3, FRS2Model.FromServer.exists).Add(4, FRS2Model.FromServer.exists), new Microsoft.Modeling.Map<System.String,System.Int32>().Add("A", 1).Add("B", 2).Add("C", 3).Add("D", 4), new Microsoft.Modeling.Map<System.String,FRS2Model.ReplicationGroupTypes>().Add("A", FRS2Model.ReplicationGroupTypes.sysvol).Add("B", FRS2Model.ReplicationGroupTypes.protection).Add("C", FRS2Model.ReplicationGroupTypes.sysvol).Add("D", FRS2Model.ReplicationGroupTypes.sysvol), new Microsoft.Modeling.Map<System.Int32,System.String>().Add(1, "A").Add(2, "B").Add(3, "C").Add(4, "D"), new Microsoft.Modeling.Map<System.Int32,FRS2Model.accessLevels>().Add(1, FRS2Model.accessLevels.writeOnly).Add(2, FRS2Model.accessLevels.readWrite).Add(3, FRS2Model.accessLevels.readOnly).Add(4, FRS2Model.accessLevels.readWrite), new Microsoft.Modeling.Map<System.Int32,FRS2Model.connectionProperty>().Add(1, FRS2Model.connectionProperty.enabled).Add(2, FRS2Model.connectionProperty.disabled).Add(3, FRS2Model.connectionProperty.enabled).Add(4, FRS2Model.connectionProperty.enabled));
            this.Manager.Comment("reaching state \'S25\'");
            this.Manager.Comment("checking step \'return Initialization/ERROR_SUCCESS\'");
            this.Manager.Assert((temp133 == FRS2Model.error_status_t.ERROR_SUCCESS), String.Format("expected \'FRS2Model.error_status_t.ERROR_SUCCESS\', actual \'{0}\' (return of Initia" +
                        "lization, state S25)", TestManagerHelpers.Describe(temp133)));
            this.Manager.Comment("reaching state \'S38\'");
            FRS2Model.ProtocolVersionReturned temp134;
            FRS2Model.UpstreamFlagValueReturned temp135;
            FRS2Model.error_status_t temp136;
            this.Manager.Comment("executing step \'call EstablishConnection(\"A\",1,FRS_COMMUNICATION_PROTOCOL_VERSION" +
                    "_LONGHORN_SERVER,out _,out _)\'");
            temp136 = this.IFRS2ManagedAdapterInstance.EstablishConnection("A", 1, FRS2Model.ProtocolVersion.FRS_COMMUNICATION_PROTOCOL_VERSION_LONGHORN_SERVER, out temp134, out temp135);
            this.Manager.Checkpoint("MS-FRS2_R37");
            this.Manager.Checkpoint("MS-FRS2_R436");
            this.Manager.Checkpoint("MS-FRS2_R439");
            this.Manager.Comment("reaching state \'S51\'");
            this.Manager.Comment("checking step \'return EstablishConnection/[out Valid,out Valid]:ERROR_SUCCESS\'");
            this.Manager.Assert((temp134 == FRS2Model.ProtocolVersionReturned.Valid), String.Format("expected \'FRS2Model.ProtocolVersionReturned.Valid\', actual \'{0}\' (upstreamProtoco" +
                        "lVersion of EstablishConnection, state S51)", TestManagerHelpers.Describe(temp134)));
            this.Manager.Assert((temp135 == FRS2Model.UpstreamFlagValueReturned.Valid), String.Format("expected \'FRS2Model.UpstreamFlagValueReturned.Valid\', actual \'{0}\' (upstreamFlags" +
                        " of EstablishConnection, state S51)", TestManagerHelpers.Describe(temp135)));
            this.Manager.Assert((temp136 == FRS2Model.error_status_t.ERROR_SUCCESS), String.Format("expected \'FRS2Model.error_status_t.ERROR_SUCCESS\', actual \'{0}\' (return of Establ" +
                        "ishConnection, state S51)", TestManagerHelpers.Describe(temp136)));
            this.Manager.Comment("reaching state \'S64\'");
            FRS2Model.error_status_t temp137;
            this.Manager.Comment("executing step \'call AsyncPoll(1)\'");
            temp137 = this.IFRS2ManagedAdapterInstance.AsyncPoll(1);
            this.Manager.Checkpoint("MS-FRS2_R549");
            this.Manager.Checkpoint("MS-FRS2_R552");
            this.Manager.Comment("reaching state \'S77\'");
            this.Manager.Comment("checking step \'return AsyncPoll/ERROR_SUCCESS\'");
            this.Manager.Assert((temp137 == FRS2Model.error_status_t.ERROR_SUCCESS), String.Format("expected \'FRS2Model.error_status_t.ERROR_SUCCESS\', actual \'{0}\' (return of AsyncP" +
                        "oll, state S77)", TestManagerHelpers.Describe(temp137)));
            this.Manager.Comment("reaching state \'S90\'");
            this.Manager.EndTest();
        }
        #endregion
    }
}
