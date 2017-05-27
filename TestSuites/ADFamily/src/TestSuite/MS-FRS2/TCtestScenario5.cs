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
    public partial class TCtestScenario5 : PtfTestClassBase {
        
        public TCtestScenario5() {
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
        
        public delegate void RequestRecordsDelegate1(FRS2Model.error_status_t @return);
        
        public delegate void RequestRecordsEventDelegate1(FRS2Model.RecordsStatus status);
        
        public delegate void CheckConnectivityDelegate1(FRS2Model.error_status_t @return);
        
        public delegate void RequestUpdatesDelegate1(FRS2Model.error_status_t @return);
        
        public delegate void UpdateCancelDelegate1(FRS2Model.error_status_t @return);
        
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
        
        public delegate void RequestUpdatesEventDelegate1(FRS2Model.FilePresense present, FRS2Model.UPDATE_STATUS updateStatus);
        
        public delegate void RawGetFileDataResponseEventDelegate1(bool isEOF);
        
        public delegate void RdcGetFileDataEventDelegate1(FRS2Model.SizeReturned sizeReturned);
        #endregion
        
        #region Event Metadata
        static System.Reflection.MethodBase InitializationInfo = TestManagerHelpers.GetMethodInfo(typeof(Microsoft.Protocols.TestSuites.MS_FRS2.IFRS2ManagedAdapter), "Initialization", typeof(FRS2Model.OSVersion), typeof(Microsoft.Modeling.Map<System.Int32,FRS2Model.connectionProperty>), typeof(Microsoft.Modeling.Map<System.Int32,FRS2Model.FromServer>), typeof(Microsoft.Modeling.Map<System.String,System.Int32>), typeof(Microsoft.Modeling.Map<System.String,FRS2Model.ReplicationGroupTypes>), typeof(Microsoft.Modeling.Map<System.Int32,System.String>), typeof(Microsoft.Modeling.Map<System.Int32,FRS2Model.accessLevels>), typeof(Microsoft.Modeling.Map<System.Int32,FRS2Model.connectionProperty>));
        
        static System.Reflection.MethodBase EstablishConnectionInfo = TestManagerHelpers.GetMethodInfo(typeof(Microsoft.Protocols.TestSuites.MS_FRS2.IFRS2ManagedAdapter), "EstablishConnection", typeof(string), typeof(int), typeof(FRS2Model.ProtocolVersion), typeof(FRS2Model.ProtocolVersionReturned).MakeByRefType(), typeof(FRS2Model.UpstreamFlagValueReturned).MakeByRefType());
        
        static System.Reflection.MethodBase EstablishSessionInfo = TestManagerHelpers.GetMethodInfo(typeof(Microsoft.Protocols.TestSuites.MS_FRS2.IFRS2ManagedAdapter), "EstablishSession", typeof(int), typeof(int));
        
        static System.Reflection.MethodBase AsyncPollInfo = TestManagerHelpers.GetMethodInfo(typeof(Microsoft.Protocols.TestSuites.MS_FRS2.IFRS2ManagedAdapter), "AsyncPoll", typeof(int));
        
        static System.Reflection.MethodBase RequestVersionVectorInfo = TestManagerHelpers.GetMethodInfo(typeof(Microsoft.Protocols.TestSuites.MS_FRS2.IFRS2ManagedAdapter), "RequestVersionVector", typeof(int), typeof(int), typeof(int), typeof(FRS2Model.VERSION_REQUEST_TYPE), typeof(FRS2Model.VERSION_CHANGE_TYPE), typeof(FRS2Model.VVGeneration));
        
        static System.Reflection.EventInfo AsyncPollResponseEventInfo = TestManagerHelpers.GetEventInfo(typeof(Microsoft.Protocols.TestSuites.MS_FRS2.IFRS2ManagedAdapter), "AsyncPollResponseEvent");
        
        static System.Reflection.MethodBase RequestRecordsInfo = TestManagerHelpers.GetMethodInfo(typeof(Microsoft.Protocols.TestSuites.MS_FRS2.IFRS2ManagedAdapter), "RequestRecords", typeof(int), typeof(int));
        
        static System.Reflection.EventInfo RequestRecordsEventInfo = TestManagerHelpers.GetEventInfo(typeof(Microsoft.Protocols.TestSuites.MS_FRS2.IFRS2ManagedAdapter), "RequestRecordsEvent");
        
        static System.Reflection.MethodBase CheckConnectivityInfo = TestManagerHelpers.GetMethodInfo(typeof(Microsoft.Protocols.TestSuites.MS_FRS2.IFRS2ManagedAdapter), "CheckConnectivity", typeof(string), typeof(int));
        
        static System.Reflection.MethodBase RequestUpdatesInfo = TestManagerHelpers.GetMethodInfo(typeof(Microsoft.Protocols.TestSuites.MS_FRS2.IFRS2ManagedAdapter), "RequestUpdates", typeof(int), typeof(int), typeof(FRS2Model.versionVectorDiff));
        
        static System.Reflection.MethodBase UpdateCancelInfo = TestManagerHelpers.GetMethodInfo(typeof(Microsoft.Protocols.TestSuites.MS_FRS2.IFRS2ManagedAdapter), "UpdateCancel", typeof(int), typeof(FRS2Model.FRS_UPDATE_CANCEL_DATA), typeof(int));
        
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
        
        static System.Reflection.EventInfo RequestUpdatesEventInfo = TestManagerHelpers.GetEventInfo(typeof(Microsoft.Protocols.TestSuites.MS_FRS2.IFRS2ManagedAdapter), "RequestUpdatesEvent");
        
        static System.Reflection.EventInfo RawGetFileDataResponseEventInfo = TestManagerHelpers.GetEventInfo(typeof(Microsoft.Protocols.TestSuites.MS_FRS2.IFRS2ManagedAdapter), "RawGetFileDataResponseEvent");
        
        static System.Reflection.EventInfo RdcGetFileDataEventInfo = TestManagerHelpers.GetEventInfo(typeof(Microsoft.Protocols.TestSuites.MS_FRS2.IFRS2ManagedAdapter), "RdcGetFileDataEvent");
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
            this.Manager.Subscribe(RequestRecordsEventInfo, this.IFRS2ManagedAdapterInstance);
            this.Manager.Subscribe(InitializeFileTransferAsyncEventInfo, this.IFRS2ManagedAdapterInstance);
            this.Manager.Subscribe(RequestUpdatesEventInfo, this.IFRS2ManagedAdapterInstance);
            this.Manager.Subscribe(RawGetFileDataResponseEventInfo, this.IFRS2ManagedAdapterInstance);
            this.Manager.Subscribe(RdcGetFileDataEventInfo, this.IFRS2ManagedAdapterInstance);
        }
        
        protected override void TestCleanup() {
            base.TestCleanup();
            this.CleanupTestManager();
        }
        #endregion
        
        #region Test Starting in S0
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        [Microsoft.VisualStudio.TestTools.UnitTesting.DescriptionAttribute("MS-FRS2_R440, MS-FRS2_R447")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestCategory("AD")]
        [TestCategory("MS-FRS2")]
        [TestCategory("SDC")]
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]       
        public virtual void FRS2_TCtestScenario5S0()
        {
            this.Manager.BeginTest("TCtestScenario5S0");
            this.Manager.Comment("reaching state \'S0\'");
            FRS2Model.error_status_t temp0;
            this.Manager.Comment(@"executing step 'call Initialization(Windows7,Map{1=>enabled,2=>enabled,3=>enabled,4=>disabled},Map{1=>exists,2=>exists,3=>exists,4=>exists},Map{""A""=>1,""B""=>2,""C""=>3,""D""=>4},Map{""A""=>sysvol,""B""=>protection,""C""=>sysvol,""D""=>sysvol},Map{1=>""A"",2=>""B"",3=>""C"",4=>""D""},Map{1=>writeOnly,2=>readWrite,3=>readOnly,4=>readWrite},Map{1=>enabled,2=>disabled,3=>enabled,4=>enabled})'");
            temp0 = this.IFRS2ManagedAdapterInstance.Initialization(FRS2Model.OSVersion.Windows7, new Microsoft.Modeling.Map<System.Int32,FRS2Model.connectionProperty>().Add(1, FRS2Model.connectionProperty.enabled).Add(2, FRS2Model.connectionProperty.enabled).Add(3, FRS2Model.connectionProperty.enabled).Add(4, FRS2Model.connectionProperty.disabled), new Microsoft.Modeling.Map<System.Int32,FRS2Model.FromServer>().Add(1, FRS2Model.FromServer.exists).Add(2, FRS2Model.FromServer.exists).Add(3, FRS2Model.FromServer.exists).Add(4, FRS2Model.FromServer.exists), new Microsoft.Modeling.Map<System.String,System.Int32>().Add("A", 1).Add("B", 2).Add("C", 3).Add("D", 4), new Microsoft.Modeling.Map<System.String,FRS2Model.ReplicationGroupTypes>().Add("A", FRS2Model.ReplicationGroupTypes.sysvol).Add("B", FRS2Model.ReplicationGroupTypes.protection).Add("C", FRS2Model.ReplicationGroupTypes.sysvol).Add("D", FRS2Model.ReplicationGroupTypes.sysvol), new Microsoft.Modeling.Map<System.Int32,System.String>().Add(1, "A").Add(2, "B").Add(3, "C").Add(4, "D"), new Microsoft.Modeling.Map<System.Int32,FRS2Model.accessLevels>().Add(1, FRS2Model.accessLevels.writeOnly).Add(2, FRS2Model.accessLevels.readWrite).Add(3, FRS2Model.accessLevels.readOnly).Add(4, FRS2Model.accessLevels.readWrite), new Microsoft.Modeling.Map<System.Int32,FRS2Model.connectionProperty>().Add(1, FRS2Model.connectionProperty.enabled).Add(2, FRS2Model.connectionProperty.disabled).Add(3, FRS2Model.connectionProperty.enabled).Add(4, FRS2Model.connectionProperty.enabled));
            this.Manager.Comment("reaching state \'S1\'");
            this.Manager.Comment("checking step \'return Initialization/ERROR_SUCCESS\'");
            this.Manager.Assert((temp0 == FRS2Model.error_status_t.ERROR_SUCCESS), String.Format("expected \'FRS2Model.error_status_t.ERROR_SUCCESS\', actual \'{0}\' (return of Initia" +
                        "lization, state S1)", TestManagerHelpers.Describe(temp0)));
            this.Manager.Comment("reaching state \'S28\'");
            FRS2Model.ProtocolVersionReturned temp1;
            FRS2Model.UpstreamFlagValueReturned temp2;
            FRS2Model.error_status_t temp3;
            this.Manager.Comment("executing step \'call EstablishConnection(\"A\",1,inValid,out _,out _)\'");
            temp3 = this.IFRS2ManagedAdapterInstance.EstablishConnection("A", 1, FRS2Model.ProtocolVersion.inValid, out temp1, out temp2);
            this.Manager.Checkpoint("MS-FRS2_R440");
            this.Manager.Checkpoint("MS-FRS2_R447");
            this.Manager.Comment("reaching state \'S42\'");
            this.Manager.Comment("checking step \'return EstablishConnection/[out Invalid,out Invalid]:FRS_ERROR_INC" +
                    "OMPATIBLE_VERSION\'");
            this.Manager.Assert((temp1 == FRS2Model.ProtocolVersionReturned.Invalid), String.Format("expected \'FRS2Model.ProtocolVersionReturned.Invalid\', actual \'{0}\' (upstreamProto" +
                        "colVersion of EstablishConnection, state S42)", TestManagerHelpers.Describe(temp1)));
            this.Manager.Assert((temp2 == FRS2Model.UpstreamFlagValueReturned.Invalid), String.Format("expected \'FRS2Model.UpstreamFlagValueReturned.Invalid\', actual \'{0}\' (upstreamFla" +
                        "gs of EstablishConnection, state S42)", TestManagerHelpers.Describe(temp2)));
            this.Manager.Assert((temp3 == FRS2Model.error_status_t.FRS_ERROR_INCOMPATIBLE_VERSION), String.Format("expected \'FRS2Model.error_status_t.FRS_ERROR_INCOMPATIBLE_VERSION\', actual \'{0}\' " +
                        "(return of EstablishConnection, state S42)", TestManagerHelpers.Describe(temp3)));
            TCtestScenario5S56();
            this.Manager.EndTest();
        }
        
        private void TCtestScenario5S56() {
            this.Manager.Comment("reaching state \'S56\'");
        }
        #endregion
        
        #region Test Starting in S2
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        [Microsoft.VisualStudio.TestTools.UnitTesting.DescriptionAttribute(@"MS-FRS2_R37, MS-FRS2_R436, MS-FRS2_R439, MS-FRS2_R455, MS-FRS2_R458, MS-FRS2_R448, MS-FRS2_R549, MS-FRS2_R552, MS-FRS2_R517, MS-FRS2_R520, MS-FRS2_R466, MS-FRS2_R556, MS-FRS2_R1020, MS-FRS2_R555, MS-FRS2_R581, MS-FRS2_R584, MS-FRS2_R99, MS-FRS2_R581, MS-FRS2_R584, MS-FRS2_R100, MS-FRS2_R99, MS-FRS2_R100, MS-FRS2_R99, MS-FRS2_R100, MS-FRS2_R581, MS-FRS2_R584, MS-FRS2_R99, MS-FRS2_R100, MS-FRS2_R99, MS-FRS2_R100, MS-FRS2_R99, MS-FRS2_R100, MS-FRS2_R581, MS-FRS2_R584, MS-FRS2_R100, MS-FRS2_R99, MS-FRS2_R556, MS-FRS2_R1020, MS-FRS2_R555")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestCategory("AD")]
        [TestCategory("MS-FRS2")]
        [TestCategory("SDC")]
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]      
        public virtual void FRS2_TCtestScenario5S2()
        {
            this.Manager.BeginTest("TCtestScenario5S2");
            this.Manager.Comment("reaching state \'S2\'");
            FRS2Model.error_status_t temp4;
            this.Manager.Comment(@"executing step 'call Initialization(Windows7,Map{1=>enabled,2=>enabled,3=>enabled,4=>disabled},Map{1=>exists,2=>exists,3=>exists,4=>exists},Map{""A""=>1,""B""=>2,""C""=>3,""D""=>4},Map{""A""=>sysvol,""B""=>protection,""C""=>sysvol,""D""=>sysvol},Map{1=>""A"",2=>""B"",3=>""C"",4=>""D""},Map{1=>writeOnly,2=>readWrite,3=>readOnly,4=>readWrite},Map{1=>enabled,2=>disabled,3=>enabled,4=>enabled})'");
            temp4 = this.IFRS2ManagedAdapterInstance.Initialization(FRS2Model.OSVersion.Windows7, new Microsoft.Modeling.Map<System.Int32,FRS2Model.connectionProperty>().Add(1, FRS2Model.connectionProperty.enabled).Add(2, FRS2Model.connectionProperty.enabled).Add(3, FRS2Model.connectionProperty.enabled).Add(4, FRS2Model.connectionProperty.disabled), new Microsoft.Modeling.Map<System.Int32,FRS2Model.FromServer>().Add(1, FRS2Model.FromServer.exists).Add(2, FRS2Model.FromServer.exists).Add(3, FRS2Model.FromServer.exists).Add(4, FRS2Model.FromServer.exists), new Microsoft.Modeling.Map<System.String,System.Int32>().Add("A", 1).Add("B", 2).Add("C", 3).Add("D", 4), new Microsoft.Modeling.Map<System.String,FRS2Model.ReplicationGroupTypes>().Add("A", FRS2Model.ReplicationGroupTypes.sysvol).Add("B", FRS2Model.ReplicationGroupTypes.protection).Add("C", FRS2Model.ReplicationGroupTypes.sysvol).Add("D", FRS2Model.ReplicationGroupTypes.sysvol), new Microsoft.Modeling.Map<System.Int32,System.String>().Add(1, "A").Add(2, "B").Add(3, "C").Add(4, "D"), new Microsoft.Modeling.Map<System.Int32,FRS2Model.accessLevels>().Add(1, FRS2Model.accessLevels.writeOnly).Add(2, FRS2Model.accessLevels.readWrite).Add(3, FRS2Model.accessLevels.readOnly).Add(4, FRS2Model.accessLevels.readWrite), new Microsoft.Modeling.Map<System.Int32,FRS2Model.connectionProperty>().Add(1, FRS2Model.connectionProperty.enabled).Add(2, FRS2Model.connectionProperty.disabled).Add(3, FRS2Model.connectionProperty.enabled).Add(4, FRS2Model.connectionProperty.enabled));
            this.Manager.Comment("reaching state \'S3\'");
            this.Manager.Comment("checking step \'return Initialization/ERROR_SUCCESS\'");
            this.Manager.Assert((temp4 == FRS2Model.error_status_t.ERROR_SUCCESS), String.Format("expected \'FRS2Model.error_status_t.ERROR_SUCCESS\', actual \'{0}\' (return of Initia" +
                        "lization, state S3)", TestManagerHelpers.Describe(temp4)));
            this.Manager.Comment("reaching state \'S29\'");
            FRS2Model.ProtocolVersionReturned temp5;
            FRS2Model.UpstreamFlagValueReturned temp6;
            FRS2Model.error_status_t temp7;
            this.Manager.Comment("executing step \'call EstablishConnection(\"A\",1,FRS_COMMUNICATION_PROTOCOL_VERSION" +
                    "_LONGHORN_SERVER,out _,out _)\'");
            temp7 = this.IFRS2ManagedAdapterInstance.EstablishConnection("A", 1, FRS2Model.ProtocolVersion.FRS_COMMUNICATION_PROTOCOL_VERSION_LONGHORN_SERVER, out temp5, out temp6);
            this.Manager.Checkpoint("MS-FRS2_R37");
            this.Manager.Checkpoint("MS-FRS2_R436");
            this.Manager.Checkpoint("MS-FRS2_R439");
            this.Manager.Comment("reaching state \'S43\'");
            this.Manager.Comment("checking step \'return EstablishConnection/[out Valid,out Valid]:ERROR_SUCCESS\'");
            this.Manager.Assert((temp5 == FRS2Model.ProtocolVersionReturned.Valid), String.Format("expected \'FRS2Model.ProtocolVersionReturned.Valid\', actual \'{0}\' (upstreamProtoco" +
                        "lVersion of EstablishConnection, state S43)", TestManagerHelpers.Describe(temp5)));
            this.Manager.Assert((temp6 == FRS2Model.UpstreamFlagValueReturned.Valid), String.Format("expected \'FRS2Model.UpstreamFlagValueReturned.Valid\', actual \'{0}\' (upstreamFlags" +
                        " of EstablishConnection, state S43)", TestManagerHelpers.Describe(temp6)));
            this.Manager.Assert((temp7 == FRS2Model.error_status_t.ERROR_SUCCESS), String.Format("expected \'FRS2Model.error_status_t.ERROR_SUCCESS\', actual \'{0}\' (return of Establ" +
                        "ishConnection, state S43)", TestManagerHelpers.Describe(temp7)));
            this.Manager.Comment("reaching state \'S57\'");
            FRS2Model.error_status_t temp8;
            this.Manager.Comment("executing step \'call EstablishSession(1,1)\'");
            temp8 = this.IFRS2ManagedAdapterInstance.EstablishSession(1, 1);
            this.Manager.Checkpoint("MS-FRS2_R455");
            this.Manager.Checkpoint("MS-FRS2_R458");
            this.Manager.Checkpoint("MS-FRS2_R448");
            this.Manager.Comment("reaching state \'S70\'");
            this.Manager.Comment("checking step \'return EstablishSession/ERROR_SUCCESS\'");
            this.Manager.Assert((temp8 == FRS2Model.error_status_t.ERROR_SUCCESS), String.Format("expected \'FRS2Model.error_status_t.ERROR_SUCCESS\', actual \'{0}\' (return of Establ" +
                        "ishSession, state S70)", TestManagerHelpers.Describe(temp8)));
            this.Manager.Comment("reaching state \'S82\'");
            FRS2Model.error_status_t temp9;
            this.Manager.Comment("executing step \'call AsyncPoll(1)\'");
            temp9 = this.IFRS2ManagedAdapterInstance.AsyncPoll(1);
            this.Manager.Checkpoint("MS-FRS2_R549");
            this.Manager.Checkpoint("MS-FRS2_R552");
            this.Manager.Comment("reaching state \'S94\'");
            this.Manager.Comment("checking step \'return AsyncPoll/ERROR_SUCCESS\'");
            this.Manager.Assert((temp9 == FRS2Model.error_status_t.ERROR_SUCCESS), String.Format("expected \'FRS2Model.error_status_t.ERROR_SUCCESS\', actual \'{0}\' (return of AsyncP" +
                        "oll, state S94)", TestManagerHelpers.Describe(temp9)));
            this.Manager.Comment("reaching state \'S105\'");
            FRS2Model.error_status_t temp10;
            this.Manager.Comment("executing step \'call RequestVersionVector(1,1,1,REQUEST_SLOW_SYNC,CHANGE_ALL,Vali" +
                    "dValue)\'");
            temp10 = this.IFRS2ManagedAdapterInstance.RequestVersionVector(1, 1, 1, FRS2Model.VERSION_REQUEST_TYPE.REQUEST_SLOW_SYNC, FRS2Model.VERSION_CHANGE_TYPE.CHANGE_ALL, FRS2Model.VVGeneration.ValidValue);
            this.Manager.Checkpoint("MS-FRS2_R517");
            this.Manager.Checkpoint("MS-FRS2_R520");
            this.Manager.Checkpoint("MS-FRS2_R466");
            this.Manager.Comment("reaching state \'S116\'");
            this.Manager.Comment("checking step \'return RequestVersionVector/ERROR_SUCCESS\'");
            this.Manager.Assert((temp10 == FRS2Model.error_status_t.ERROR_SUCCESS), String.Format("expected \'FRS2Model.error_status_t.ERROR_SUCCESS\', actual \'{0}\' (return of Reques" +
                        "tVersionVector, state S116)", TestManagerHelpers.Describe(temp10)));
            this.Manager.Comment("reaching state \'S124\'");
            int temp21 = this.Manager.ExpectEvent(this.QuiescenceTimeout, true, new ExpectedEvent(TCtestScenario5.AsyncPollResponseEventInfo, null, new AsyncPollResponseEventDelegate1(this.TCtestScenario5S2AsyncPollResponseEventChecker)), new ExpectedEvent(TCtestScenario5.AsyncPollResponseEventInfo, null, new AsyncPollResponseEventDelegate1(this.TCtestScenario5S2AsyncPollResponseEventChecker1)));
            if ((temp21 == 0)) {
                this.Manager.Comment("reaching state \'S132\'");
                FRS2Model.error_status_t temp11;
                this.Manager.Comment("executing step \'call RequestRecords(1,1)\'");
                temp11 = this.IFRS2ManagedAdapterInstance.RequestRecords(1, 1);
                this.Manager.Checkpoint("MS-FRS2_R581");
                this.Manager.Checkpoint("MS-FRS2_R584");
                this.Manager.Comment("reaching state \'S142\'");
                this.Manager.Comment("checking step \'return RequestRecords/ERROR_SUCCESS\'");
                this.Manager.Assert((temp11 == FRS2Model.error_status_t.ERROR_SUCCESS), String.Format("expected \'FRS2Model.error_status_t.ERROR_SUCCESS\', actual \'{0}\' (return of Reques" +
                            "tRecords, state S142)", TestManagerHelpers.Describe(temp11)));
                this.Manager.Comment("reaching state \'S147\'");
                int temp20 = this.Manager.ExpectEvent(this.ProceedControlTimeout, false, new ExpectedEvent(TCtestScenario5.RequestRecordsEventInfo, null, new RequestRecordsEventDelegate1(this.TCtestScenario5S2RequestRecordsEventChecker)), new ExpectedEvent(TCtestScenario5.RequestRecordsEventInfo, null, new RequestRecordsEventDelegate1(this.TCtestScenario5S2RequestRecordsEventChecker5)), new ExpectedEvent(TCtestScenario5.RequestRecordsEventInfo, null, new RequestRecordsEventDelegate1(this.TCtestScenario5S2RequestRecordsEventChecker10)), new ExpectedEvent(TCtestScenario5.RequestRecordsEventInfo, null, new RequestRecordsEventDelegate1(this.TCtestScenario5S2RequestRecordsEventChecker11)));
                if ((temp20 == 0)) {
                    this.Manager.Comment("reaching state \'S152\'");
                    int temp14 = this.Manager.ExpectEvent(this.ProceedControlTimeout, false, new ExpectedEvent(TCtestScenario5.RequestRecordsEventInfo, null, new RequestRecordsEventDelegate1(this.TCtestScenario5S2RequestRecordsEventChecker1)), new ExpectedEvent(TCtestScenario5.RequestRecordsEventInfo, null, new RequestRecordsEventDelegate1(this.TCtestScenario5S2RequestRecordsEventChecker2)));
                    if ((temp14 == 0)) {
                        TCtestScenario5S166();
                        goto label1;
                    }
                    if ((temp14 == 1)) {
                        TCtestScenario5S167();
                        goto label1;
                    }
                    FRS2Model.error_status_t temp12;
                    this.Manager.Comment("executing step \'call RequestRecords(1,1)\'");
                    temp12 = this.IFRS2ManagedAdapterInstance.RequestRecords(1, 1);
                    this.Manager.Checkpoint("MS-FRS2_R581");
                    this.Manager.Checkpoint("MS-FRS2_R584");
                    this.Manager.AddReturn(RequestRecordsInfo, null, temp12);
                    TCtestScenario5S165();
                label1:
;
                    goto label5;
                }
                if ((temp20 == 1)) {
                    this.Manager.Comment("reaching state \'S153\'");
                    int temp17 = this.Manager.ExpectEvent(this.ProceedControlTimeout, false, new ExpectedEvent(TCtestScenario5.RequestRecordsEventInfo, null, new RequestRecordsEventDelegate1(this.TCtestScenario5S2RequestRecordsEventChecker6)), new ExpectedEvent(TCtestScenario5.RequestRecordsEventInfo, null, new RequestRecordsEventDelegate1(this.TCtestScenario5S2RequestRecordsEventChecker7)));
                    if ((temp17 == 0)) {
                        TCtestScenario5S169();
                        goto label3;
                    }
                    if ((temp17 == 1)) {
                        TCtestScenario5S170();
                        goto label3;
                    }
                    FRS2Model.error_status_t temp15;
                    this.Manager.Comment("executing step \'call RequestRecords(1,1)\'");
                    temp15 = this.IFRS2ManagedAdapterInstance.RequestRecords(1, 1);
                    this.Manager.Checkpoint("MS-FRS2_R581");
                    this.Manager.Checkpoint("MS-FRS2_R584");
                    this.Manager.AddReturn(RequestRecordsInfo, null, temp15);
                    TCtestScenario5S168();
                label3:
;
                    goto label5;
                }
                if ((temp20 == 2)) {
                    TCtestScenario5S154();
                    goto label5;
                }
                if ((temp20 == 3)) {
                    TCtestScenario5S155();
                    goto label5;
                }
                FRS2Model.error_status_t temp18;
                this.Manager.Comment("executing step \'call RequestRecords(1,1)\'");
                temp18 = this.IFRS2ManagedAdapterInstance.RequestRecords(1, 1);
                this.Manager.Checkpoint("MS-FRS2_R581");
                this.Manager.Checkpoint("MS-FRS2_R584");
                this.Manager.AddReturn(RequestRecordsInfo, null, temp18);
                TCtestScenario5S143();
            label5:
;
                goto label6;
            }
            if ((temp21 == 1)) {
                TCtestScenario5S133();
                goto label6;
            }
            throw new InvalidOperationException("never reached");
        label6:
;
            this.Manager.EndTest();
        }
        
        private void TCtestScenario5S2AsyncPollResponseEventChecker(FRS2Model.VVGeneration vvGen) {
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
        
        private void TCtestScenario5S2RequestRecordsEventChecker(FRS2Model.RecordsStatus status) {
            this.Manager.Comment("checking step \'event RequestRecordsEvent(RECORDS_STATUS_DONE)\'");
            try {
                this.Manager.Assert((status == FRS2Model.RecordsStatus.RECORDS_STATUS_DONE), String.Format("expected \'FRS2Model.RecordsStatus.RECORDS_STATUS_DONE\', actual \'{0}\' (status of R" +
                            "equestRecordsEvent, state S147)", TestManagerHelpers.Describe(status)));
            }
            catch (TransactionFailedException ) {
                this.Manager.Comment("This step would have covered MS-FRS2_R99");
                throw;
            }
            this.Manager.Checkpoint("MS-FRS2_R99");
        }
        
        private void TCtestScenario5S2RequestRecordsEventChecker1(FRS2Model.RecordsStatus status) {
            this.Manager.Comment("checking step \'event RequestRecordsEvent(RECORDS_STATUS_MORE)\'");
            try {
                this.Manager.Assert((status == FRS2Model.RecordsStatus.RECORDS_STATUS_MORE), String.Format("expected \'FRS2Model.RecordsStatus.RECORDS_STATUS_MORE\', actual \'{0}\' (status of R" +
                            "equestRecordsEvent, state S152)", TestManagerHelpers.Describe(status)));
            }
            catch (TransactionFailedException ) {
                this.Manager.Comment("This step would have covered MS-FRS2_R100");
                throw;
            }
            this.Manager.Checkpoint("MS-FRS2_R100");
        }
        
        private void TCtestScenario5S166() {
            this.Manager.Comment("reaching state \'S166\'");
        }
        
        private void TCtestScenario5S2RequestRecordsEventChecker2(FRS2Model.RecordsStatus status) {
            this.Manager.Comment("checking step \'event RequestRecordsEvent(RECORDS_STATUS_DONE)\'");
            try {
                this.Manager.Assert((status == FRS2Model.RecordsStatus.RECORDS_STATUS_DONE), String.Format("expected \'FRS2Model.RecordsStatus.RECORDS_STATUS_DONE\', actual \'{0}\' (status of R" +
                            "equestRecordsEvent, state S152)", TestManagerHelpers.Describe(status)));
            }
            catch (TransactionFailedException ) {
                this.Manager.Comment("This step would have covered MS-FRS2_R99");
                throw;
            }
            this.Manager.Checkpoint("MS-FRS2_R99");
        }
        
        private void TCtestScenario5S167() {
            this.Manager.Comment("reaching state \'S167\'");
        }
        
        private void TCtestScenario5S165() {
            this.Manager.Comment("reaching state \'S165\'");
            this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(TCtestScenario5.RequestRecordsInfo, null, new RequestRecordsDelegate1(this.TCtestScenario5S2RequestRecordsChecker)));
            this.Manager.Comment("reaching state \'S177\'");
            int temp13 = this.Manager.ExpectEvent(this.QuiescenceTimeout, true, new ExpectedEvent(TCtestScenario5.RequestRecordsEventInfo, null, new RequestRecordsEventDelegate1(this.TCtestScenario5S2RequestRecordsEventChecker3)), new ExpectedEvent(TCtestScenario5.RequestRecordsEventInfo, null, new RequestRecordsEventDelegate1(this.TCtestScenario5S2RequestRecordsEventChecker4)));
            if ((temp13 == 0)) {
                this.Manager.Comment("reaching state \'S179\'");
                goto label0;
            }
            if ((temp13 == 1)) {
                this.Manager.Comment("reaching state \'S180\'");
                goto label0;
            }
            throw new InvalidOperationException("never reached");
        label0:
;
        }
        
        private void TCtestScenario5S2RequestRecordsChecker(FRS2Model.error_status_t @return) {
            this.Manager.Comment("checking step \'return RequestRecords/ERROR_SUCCESS\'");
            this.Manager.Assert((@return == FRS2Model.error_status_t.ERROR_SUCCESS), String.Format("expected \'FRS2Model.error_status_t.ERROR_SUCCESS\', actual \'{0}\' (return of Reques" +
                        "tRecords, state S165)", TestManagerHelpers.Describe(@return)));
        }
        
        private void TCtestScenario5S2RequestRecordsEventChecker3(FRS2Model.RecordsStatus status) {
            this.Manager.Comment("checking step \'event RequestRecordsEvent(RECORDS_STATUS_MORE)\'");
            try {
                this.Manager.Assert((status == FRS2Model.RecordsStatus.RECORDS_STATUS_MORE), String.Format("expected \'FRS2Model.RecordsStatus.RECORDS_STATUS_MORE\', actual \'{0}\' (status of R" +
                            "equestRecordsEvent, state S177)", TestManagerHelpers.Describe(status)));
            }
            catch (TransactionFailedException ) {
                this.Manager.Comment("This step would have covered MS-FRS2_R100");
                throw;
            }
            this.Manager.Checkpoint("MS-FRS2_R100");
        }
        
        private void TCtestScenario5S2RequestRecordsEventChecker4(FRS2Model.RecordsStatus status) {
            this.Manager.Comment("checking step \'event RequestRecordsEvent(RECORDS_STATUS_DONE)\'");
            try {
                this.Manager.Assert((status == FRS2Model.RecordsStatus.RECORDS_STATUS_DONE), String.Format("expected \'FRS2Model.RecordsStatus.RECORDS_STATUS_DONE\', actual \'{0}\' (status of R" +
                            "equestRecordsEvent, state S177)", TestManagerHelpers.Describe(status)));
            }
            catch (TransactionFailedException ) {
                this.Manager.Comment("This step would have covered MS-FRS2_R99");
                throw;
            }
            this.Manager.Checkpoint("MS-FRS2_R99");
        }
        
        private void TCtestScenario5S2RequestRecordsEventChecker5(FRS2Model.RecordsStatus status) {
            this.Manager.Comment("checking step \'event RequestRecordsEvent(RECORDS_STATUS_MORE)\'");
            try {
                this.Manager.Assert((status == FRS2Model.RecordsStatus.RECORDS_STATUS_MORE), String.Format("expected \'FRS2Model.RecordsStatus.RECORDS_STATUS_MORE\', actual \'{0}\' (status of R" +
                            "equestRecordsEvent, state S147)", TestManagerHelpers.Describe(status)));
            }
            catch (TransactionFailedException ) {
                this.Manager.Comment("This step would have covered MS-FRS2_R100");
                throw;
            }
            this.Manager.Checkpoint("MS-FRS2_R100");
        }
        
        private void TCtestScenario5S2RequestRecordsEventChecker6(FRS2Model.RecordsStatus status) {
            this.Manager.Comment("checking step \'event RequestRecordsEvent(RECORDS_STATUS_DONE)\'");
            try {
                this.Manager.Assert((status == FRS2Model.RecordsStatus.RECORDS_STATUS_DONE), String.Format("expected \'FRS2Model.RecordsStatus.RECORDS_STATUS_DONE\', actual \'{0}\' (status of R" +
                            "equestRecordsEvent, state S153)", TestManagerHelpers.Describe(status)));
            }
            catch (TransactionFailedException ) {
                this.Manager.Comment("This step would have covered MS-FRS2_R99");
                throw;
            }
            this.Manager.Checkpoint("MS-FRS2_R99");
        }
        
        private void TCtestScenario5S169() {
            this.Manager.Comment("reaching state \'S169\'");
        }
        
        private void TCtestScenario5S2RequestRecordsEventChecker7(FRS2Model.RecordsStatus status) {
            this.Manager.Comment("checking step \'event RequestRecordsEvent(RECORDS_STATUS_MORE)\'");
            try {
                this.Manager.Assert((status == FRS2Model.RecordsStatus.RECORDS_STATUS_MORE), String.Format("expected \'FRS2Model.RecordsStatus.RECORDS_STATUS_MORE\', actual \'{0}\' (status of R" +
                            "equestRecordsEvent, state S153)", TestManagerHelpers.Describe(status)));
            }
            catch (TransactionFailedException ) {
                this.Manager.Comment("This step would have covered MS-FRS2_R100");
                throw;
            }
            this.Manager.Checkpoint("MS-FRS2_R100");
        }
        
        private void TCtestScenario5S170() {
            this.Manager.Comment("reaching state \'S170\'");
        }
        
        private void TCtestScenario5S168() {
            this.Manager.Comment("reaching state \'S168\'");
            this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(TCtestScenario5.RequestRecordsInfo, null, new RequestRecordsDelegate1(this.TCtestScenario5S2RequestRecordsChecker1)));
            this.Manager.Comment("reaching state \'S178\'");
            int temp16 = this.Manager.ExpectEvent(this.QuiescenceTimeout, true, new ExpectedEvent(TCtestScenario5.RequestRecordsEventInfo, null, new RequestRecordsEventDelegate1(this.TCtestScenario5S2RequestRecordsEventChecker8)), new ExpectedEvent(TCtestScenario5.RequestRecordsEventInfo, null, new RequestRecordsEventDelegate1(this.TCtestScenario5S2RequestRecordsEventChecker9)));
            if ((temp16 == 0)) {
                this.Manager.Comment("reaching state \'S181\'");
                goto label2;
            }
            if ((temp16 == 1)) {
                this.Manager.Comment("reaching state \'S182\'");
                goto label2;
            }
            throw new InvalidOperationException("never reached");
        label2:
;
        }
        
        private void TCtestScenario5S2RequestRecordsChecker1(FRS2Model.error_status_t @return) {
            this.Manager.Comment("checking step \'return RequestRecords/ERROR_SUCCESS\'");
            this.Manager.Assert((@return == FRS2Model.error_status_t.ERROR_SUCCESS), String.Format("expected \'FRS2Model.error_status_t.ERROR_SUCCESS\', actual \'{0}\' (return of Reques" +
                        "tRecords, state S168)", TestManagerHelpers.Describe(@return)));
        }
        
        private void TCtestScenario5S2RequestRecordsEventChecker8(FRS2Model.RecordsStatus status) {
            this.Manager.Comment("checking step \'event RequestRecordsEvent(RECORDS_STATUS_DONE)\'");
            try {
                this.Manager.Assert((status == FRS2Model.RecordsStatus.RECORDS_STATUS_DONE), String.Format("expected \'FRS2Model.RecordsStatus.RECORDS_STATUS_DONE\', actual \'{0}\' (status of R" +
                            "equestRecordsEvent, state S178)", TestManagerHelpers.Describe(status)));
            }
            catch (TransactionFailedException ) {
                this.Manager.Comment("This step would have covered MS-FRS2_R99");
                throw;
            }
            this.Manager.Checkpoint("MS-FRS2_R99");
        }
        
        private void TCtestScenario5S2RequestRecordsEventChecker9(FRS2Model.RecordsStatus status) {
            this.Manager.Comment("checking step \'event RequestRecordsEvent(RECORDS_STATUS_MORE)\'");
            try {
                this.Manager.Assert((status == FRS2Model.RecordsStatus.RECORDS_STATUS_MORE), String.Format("expected \'FRS2Model.RecordsStatus.RECORDS_STATUS_MORE\', actual \'{0}\' (status of R" +
                            "equestRecordsEvent, state S178)", TestManagerHelpers.Describe(status)));
            }
            catch (TransactionFailedException ) {
                this.Manager.Comment("This step would have covered MS-FRS2_R100");
                throw;
            }
            this.Manager.Checkpoint("MS-FRS2_R100");
        }
        
        private void TCtestScenario5S2RequestRecordsEventChecker10(FRS2Model.RecordsStatus status) {
            this.Manager.Comment("checking step \'event RequestRecordsEvent(RECORDS_STATUS_DONE)\'");
            try {
                this.Manager.Assert((status == FRS2Model.RecordsStatus.RECORDS_STATUS_DONE), String.Format("expected \'FRS2Model.RecordsStatus.RECORDS_STATUS_DONE\', actual \'{0}\' (status of R" +
                            "equestRecordsEvent, state S147)", TestManagerHelpers.Describe(status)));
            }
            catch (TransactionFailedException ) {
                this.Manager.Comment("This step would have covered MS-FRS2_R99");
                throw;
            }
            this.Manager.Checkpoint("MS-FRS2_R99");
        }
        
        private void TCtestScenario5S154() {
            this.Manager.Comment("reaching state \'S154\'");
        }
        
        private void TCtestScenario5S2RequestRecordsEventChecker11(FRS2Model.RecordsStatus status) {
            this.Manager.Comment("checking step \'event RequestRecordsEvent(RECORDS_STATUS_MORE)\'");
            try {
                this.Manager.Assert((status == FRS2Model.RecordsStatus.RECORDS_STATUS_MORE), String.Format("expected \'FRS2Model.RecordsStatus.RECORDS_STATUS_MORE\', actual \'{0}\' (status of R" +
                            "equestRecordsEvent, state S147)", TestManagerHelpers.Describe(status)));
            }
            catch (TransactionFailedException ) {
                this.Manager.Comment("This step would have covered MS-FRS2_R100");
                throw;
            }
            this.Manager.Checkpoint("MS-FRS2_R100");
        }
        
        private void TCtestScenario5S155() {
            this.Manager.Comment("reaching state \'S155\'");
        }
        
        private void TCtestScenario5S143() {
            this.Manager.Comment("reaching state \'S143\'");
            this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(TCtestScenario5.RequestRecordsInfo, null, new RequestRecordsDelegate1(this.TCtestScenario5S2RequestRecordsChecker2)));
            this.Manager.Comment("reaching state \'S148\'");
            int temp19 = this.Manager.ExpectEvent(this.QuiescenceTimeout, true, new ExpectedEvent(TCtestScenario5.RequestRecordsEventInfo, null, new RequestRecordsEventDelegate1(this.TCtestScenario5S2RequestRecordsEventChecker12)), new ExpectedEvent(TCtestScenario5.RequestRecordsEventInfo, null, new RequestRecordsEventDelegate1(this.TCtestScenario5S2RequestRecordsEventChecker13)));
            if ((temp19 == 0)) {
                this.Manager.Comment("reaching state \'S157\'");
                goto label4;
            }
            if ((temp19 == 1)) {
                this.Manager.Comment("reaching state \'S158\'");
                goto label4;
            }
            throw new InvalidOperationException("never reached");
        label4:
;
        }
        
        private void TCtestScenario5S2RequestRecordsChecker2(FRS2Model.error_status_t @return) {
            this.Manager.Comment("checking step \'return RequestRecords/ERROR_SUCCESS\'");
            this.Manager.Assert((@return == FRS2Model.error_status_t.ERROR_SUCCESS), String.Format("expected \'FRS2Model.error_status_t.ERROR_SUCCESS\', actual \'{0}\' (return of Reques" +
                        "tRecords, state S143)", TestManagerHelpers.Describe(@return)));
        }
        
        private void TCtestScenario5S2RequestRecordsEventChecker12(FRS2Model.RecordsStatus status) {
            this.Manager.Comment("checking step \'event RequestRecordsEvent(RECORDS_STATUS_MORE)\'");
            try {
                this.Manager.Assert((status == FRS2Model.RecordsStatus.RECORDS_STATUS_MORE), String.Format("expected \'FRS2Model.RecordsStatus.RECORDS_STATUS_MORE\', actual \'{0}\' (status of R" +
                            "equestRecordsEvent, state S148)", TestManagerHelpers.Describe(status)));
            }
            catch (TransactionFailedException ) {
                this.Manager.Comment("This step would have covered MS-FRS2_R100");
                throw;
            }
            this.Manager.Checkpoint("MS-FRS2_R100");
        }
        
        private void TCtestScenario5S2RequestRecordsEventChecker13(FRS2Model.RecordsStatus status) {
            this.Manager.Comment("checking step \'event RequestRecordsEvent(RECORDS_STATUS_DONE)\'");
            try {
                this.Manager.Assert((status == FRS2Model.RecordsStatus.RECORDS_STATUS_DONE), String.Format("expected \'FRS2Model.RecordsStatus.RECORDS_STATUS_DONE\', actual \'{0}\' (status of R" +
                            "equestRecordsEvent, state S148)", TestManagerHelpers.Describe(status)));
            }
            catch (TransactionFailedException ) {
                this.Manager.Comment("This step would have covered MS-FRS2_R99");
                throw;
            }
            this.Manager.Checkpoint("MS-FRS2_R99");
        }
        
        private void TCtestScenario5S2AsyncPollResponseEventChecker1(FRS2Model.VVGeneration vvGen) {
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
        
        private void TCtestScenario5S133() {
            this.Manager.Comment("reaching state \'S133\'");
        }
        #endregion
        
        #region Test Starting in S4
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        [Microsoft.VisualStudio.TestTools.UnitTesting.DescriptionAttribute(@"MS-FRS2_R37, MS-FRS2_R436, MS-FRS2_R439, MS-FRS2_R455, MS-FRS2_R458, MS-FRS2_R448, MS-FRS2_R549, MS-FRS2_R552, MS-FRS2_R517, MS-FRS2_R520, MS-FRS2_R466, MS-FRS2_R556, MS-FRS2_R1020, MS-FRS2_R555, MS-FRS2_R581, MS-FRS2_R584, MS-FRS2_R556, MS-FRS2_R1020, MS-FRS2_R555")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestCategory("AD")]
        [TestCategory("MS-FRS2")]
        [TestCategory("SDC")]
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]    
        public virtual void FRS2_TCtestScenario5S4()
        {
            this.Manager.BeginTest("TCtestScenario5S4");
            this.Manager.Comment("reaching state \'S4\'");
            FRS2Model.error_status_t temp22;
            this.Manager.Comment(@"executing step 'call Initialization(Windows7,Map{1=>enabled,2=>enabled,3=>enabled,4=>disabled},Map{1=>exists,2=>exists,3=>exists,4=>exists},Map{""A""=>1,""B""=>2,""C""=>3,""D""=>4},Map{""A""=>sysvol,""B""=>protection,""C""=>sysvol,""D""=>sysvol},Map{1=>""A"",2=>""B"",3=>""C"",4=>""D""},Map{1=>writeOnly,2=>readWrite,3=>readOnly,4=>readWrite},Map{1=>enabled,2=>disabled,3=>enabled,4=>enabled})'");
            temp22 = this.IFRS2ManagedAdapterInstance.Initialization(FRS2Model.OSVersion.Windows7, new Microsoft.Modeling.Map<System.Int32,FRS2Model.connectionProperty>().Add(1, FRS2Model.connectionProperty.enabled).Add(2, FRS2Model.connectionProperty.enabled).Add(3, FRS2Model.connectionProperty.enabled).Add(4, FRS2Model.connectionProperty.disabled), new Microsoft.Modeling.Map<System.Int32,FRS2Model.FromServer>().Add(1, FRS2Model.FromServer.exists).Add(2, FRS2Model.FromServer.exists).Add(3, FRS2Model.FromServer.exists).Add(4, FRS2Model.FromServer.exists), new Microsoft.Modeling.Map<System.String,System.Int32>().Add("A", 1).Add("B", 2).Add("C", 3).Add("D", 4), new Microsoft.Modeling.Map<System.String,FRS2Model.ReplicationGroupTypes>().Add("A", FRS2Model.ReplicationGroupTypes.sysvol).Add("B", FRS2Model.ReplicationGroupTypes.protection).Add("C", FRS2Model.ReplicationGroupTypes.sysvol).Add("D", FRS2Model.ReplicationGroupTypes.sysvol), new Microsoft.Modeling.Map<System.Int32,System.String>().Add(1, "A").Add(2, "B").Add(3, "C").Add(4, "D"), new Microsoft.Modeling.Map<System.Int32,FRS2Model.accessLevels>().Add(1, FRS2Model.accessLevels.writeOnly).Add(2, FRS2Model.accessLevels.readWrite).Add(3, FRS2Model.accessLevels.readOnly).Add(4, FRS2Model.accessLevels.readWrite), new Microsoft.Modeling.Map<System.Int32,FRS2Model.connectionProperty>().Add(1, FRS2Model.connectionProperty.enabled).Add(2, FRS2Model.connectionProperty.disabled).Add(3, FRS2Model.connectionProperty.enabled).Add(4, FRS2Model.connectionProperty.enabled));
            this.Manager.Comment("reaching state \'S5\'");
            this.Manager.Comment("checking step \'return Initialization/ERROR_SUCCESS\'");
            this.Manager.Assert((temp22 == FRS2Model.error_status_t.ERROR_SUCCESS), String.Format("expected \'FRS2Model.error_status_t.ERROR_SUCCESS\', actual \'{0}\' (return of Initia" +
                        "lization, state S5)", TestManagerHelpers.Describe(temp22)));
            this.Manager.Comment("reaching state \'S30\'");
            FRS2Model.ProtocolVersionReturned temp23;
            FRS2Model.UpstreamFlagValueReturned temp24;
            FRS2Model.error_status_t temp25;
            this.Manager.Comment("executing step \'call EstablishConnection(\"A\",1,FRS_COMMUNICATION_PROTOCOL_VERSION" +
                    "_LONGHORN_SERVER,out _,out _)\'");
            temp25 = this.IFRS2ManagedAdapterInstance.EstablishConnection("A", 1, FRS2Model.ProtocolVersion.FRS_COMMUNICATION_PROTOCOL_VERSION_LONGHORN_SERVER, out temp23, out temp24);
            this.Manager.Checkpoint("MS-FRS2_R37");
            this.Manager.Checkpoint("MS-FRS2_R436");
            this.Manager.Checkpoint("MS-FRS2_R439");
            this.Manager.Comment("reaching state \'S44\'");
            this.Manager.Comment("checking step \'return EstablishConnection/[out Valid,out Valid]:ERROR_SUCCESS\'");
            this.Manager.Assert((temp23 == FRS2Model.ProtocolVersionReturned.Valid), String.Format("expected \'FRS2Model.ProtocolVersionReturned.Valid\', actual \'{0}\' (upstreamProtoco" +
                        "lVersion of EstablishConnection, state S44)", TestManagerHelpers.Describe(temp23)));
            this.Manager.Assert((temp24 == FRS2Model.UpstreamFlagValueReturned.Valid), String.Format("expected \'FRS2Model.UpstreamFlagValueReturned.Valid\', actual \'{0}\' (upstreamFlags" +
                        " of EstablishConnection, state S44)", TestManagerHelpers.Describe(temp24)));
            this.Manager.Assert((temp25 == FRS2Model.error_status_t.ERROR_SUCCESS), String.Format("expected \'FRS2Model.error_status_t.ERROR_SUCCESS\', actual \'{0}\' (return of Establ" +
                        "ishConnection, state S44)", TestManagerHelpers.Describe(temp25)));
            this.Manager.Comment("reaching state \'S58\'");
            FRS2Model.error_status_t temp26;
            this.Manager.Comment("executing step \'call EstablishSession(1,1)\'");
            temp26 = this.IFRS2ManagedAdapterInstance.EstablishSession(1, 1);
            this.Manager.Checkpoint("MS-FRS2_R455");
            this.Manager.Checkpoint("MS-FRS2_R458");
            this.Manager.Checkpoint("MS-FRS2_R448");
            this.Manager.Comment("reaching state \'S71\'");
            this.Manager.Comment("checking step \'return EstablishSession/ERROR_SUCCESS\'");
            this.Manager.Assert((temp26 == FRS2Model.error_status_t.ERROR_SUCCESS), String.Format("expected \'FRS2Model.error_status_t.ERROR_SUCCESS\', actual \'{0}\' (return of Establ" +
                        "ishSession, state S71)", TestManagerHelpers.Describe(temp26)));
            this.Manager.Comment("reaching state \'S83\'");
            FRS2Model.error_status_t temp27;
            this.Manager.Comment("executing step \'call AsyncPoll(1)\'");
            temp27 = this.IFRS2ManagedAdapterInstance.AsyncPoll(1);
            this.Manager.Checkpoint("MS-FRS2_R549");
            this.Manager.Checkpoint("MS-FRS2_R552");
            this.Manager.Comment("reaching state \'S95\'");
            this.Manager.Comment("checking step \'return AsyncPoll/ERROR_SUCCESS\'");
            this.Manager.Assert((temp27 == FRS2Model.error_status_t.ERROR_SUCCESS), String.Format("expected \'FRS2Model.error_status_t.ERROR_SUCCESS\', actual \'{0}\' (return of AsyncP" +
                        "oll, state S95)", TestManagerHelpers.Describe(temp27)));
            this.Manager.Comment("reaching state \'S106\'");
            FRS2Model.error_status_t temp28;
            this.Manager.Comment("executing step \'call RequestVersionVector(1,1,1,REQUEST_SLOW_SYNC,CHANGE_ALL,Vali" +
                    "dValue)\'");
            temp28 = this.IFRS2ManagedAdapterInstance.RequestVersionVector(1, 1, 1, FRS2Model.VERSION_REQUEST_TYPE.REQUEST_SLOW_SYNC, FRS2Model.VERSION_CHANGE_TYPE.CHANGE_ALL, FRS2Model.VVGeneration.ValidValue);
            this.Manager.Checkpoint("MS-FRS2_R517");
            this.Manager.Checkpoint("MS-FRS2_R520");
            this.Manager.Checkpoint("MS-FRS2_R466");
            this.Manager.Comment("reaching state \'S117\'");
            this.Manager.Comment("checking step \'return RequestVersionVector/ERROR_SUCCESS\'");
            this.Manager.Assert((temp28 == FRS2Model.error_status_t.ERROR_SUCCESS), String.Format("expected \'FRS2Model.error_status_t.ERROR_SUCCESS\', actual \'{0}\' (return of Reques" +
                        "tVersionVector, state S117)", TestManagerHelpers.Describe(temp28)));
            this.Manager.Comment("reaching state \'S125\'");
            int temp30 = this.Manager.ExpectEvent(this.QuiescenceTimeout, true, new ExpectedEvent(TCtestScenario5.AsyncPollResponseEventInfo, null, new AsyncPollResponseEventDelegate1(this.TCtestScenario5S4AsyncPollResponseEventChecker)), new ExpectedEvent(TCtestScenario5.AsyncPollResponseEventInfo, null, new AsyncPollResponseEventDelegate1(this.TCtestScenario5S4AsyncPollResponseEventChecker1)));
            if ((temp30 == 0)) {
                this.Manager.Comment("reaching state \'S134\'");
                FRS2Model.error_status_t temp29;
                this.Manager.Comment("executing step \'call RequestRecords(1,1)\'");
                temp29 = this.IFRS2ManagedAdapterInstance.RequestRecords(1, 1);
                this.Manager.Checkpoint("MS-FRS2_R581");
                this.Manager.Checkpoint("MS-FRS2_R584");
                this.Manager.AddReturn(RequestRecordsInfo, null, temp29);
                TCtestScenario5S143();
                goto label7;
            }
            if ((temp30 == 1)) {
                TCtestScenario5S133();
                goto label7;
            }
            throw new InvalidOperationException("never reached");
        label7:
;
            this.Manager.EndTest();
        }
        
        private void TCtestScenario5S4AsyncPollResponseEventChecker(FRS2Model.VVGeneration vvGen) {
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
        
        private void TCtestScenario5S4AsyncPollResponseEventChecker1(FRS2Model.VVGeneration vvGen) {
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
        
        #region Test Starting in S6
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        [Microsoft.VisualStudio.TestTools.UnitTesting.DescriptionAttribute(@"MS-FRS2_R37, MS-FRS2_R436, MS-FRS2_R439, MS-FRS2_R455, MS-FRS2_R458, MS-FRS2_R448, MS-FRS2_R549, MS-FRS2_R552, MS-FRS2_R517, MS-FRS2_R520, MS-FRS2_R466, MS-FRS2_R556, MS-FRS2_R1020, MS-FRS2_R555, MS-FRS2_R581, MS-FRS2_R584, MS-FRS2_R100, MS-FRS2_R100, MS-FRS2_R100, MS-FRS2_R99, MS-FRS2_R581, MS-FRS2_R584, MS-FRS2_R99, MS-FRS2_R99, MS-FRS2_R99, MS-FRS2_R100, MS-FRS2_R581, MS-FRS2_R584, MS-FRS2_R581, MS-FRS2_R584, MS-FRS2_R556, MS-FRS2_R1020, MS-FRS2_R555")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestCategory("AD")]
        [TestCategory("MS-FRS2")]
        [TestCategory("SDC")]
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]        
        public virtual void FRS2_TCtestScenario5S6()
        {
            this.Manager.BeginTest("TCtestScenario5S6");
            this.Manager.Comment("reaching state \'S6\'");
            FRS2Model.error_status_t temp31;
            this.Manager.Comment(@"executing step 'call Initialization(Windows7,Map{1=>enabled,2=>enabled,3=>enabled,4=>disabled},Map{1=>exists,2=>exists,3=>exists,4=>exists},Map{""A""=>1,""B""=>2,""C""=>3,""D""=>4},Map{""A""=>sysvol,""B""=>protection,""C""=>sysvol,""D""=>sysvol},Map{1=>""A"",2=>""B"",3=>""C"",4=>""D""},Map{1=>writeOnly,2=>readWrite,3=>readOnly,4=>readWrite},Map{1=>enabled,2=>disabled,3=>enabled,4=>enabled})'");
            temp31 = this.IFRS2ManagedAdapterInstance.Initialization(FRS2Model.OSVersion.Windows7, new Microsoft.Modeling.Map<System.Int32,FRS2Model.connectionProperty>().Add(1, FRS2Model.connectionProperty.enabled).Add(2, FRS2Model.connectionProperty.enabled).Add(3, FRS2Model.connectionProperty.enabled).Add(4, FRS2Model.connectionProperty.disabled), new Microsoft.Modeling.Map<System.Int32,FRS2Model.FromServer>().Add(1, FRS2Model.FromServer.exists).Add(2, FRS2Model.FromServer.exists).Add(3, FRS2Model.FromServer.exists).Add(4, FRS2Model.FromServer.exists), new Microsoft.Modeling.Map<System.String,System.Int32>().Add("A", 1).Add("B", 2).Add("C", 3).Add("D", 4), new Microsoft.Modeling.Map<System.String,FRS2Model.ReplicationGroupTypes>().Add("A", FRS2Model.ReplicationGroupTypes.sysvol).Add("B", FRS2Model.ReplicationGroupTypes.protection).Add("C", FRS2Model.ReplicationGroupTypes.sysvol).Add("D", FRS2Model.ReplicationGroupTypes.sysvol), new Microsoft.Modeling.Map<System.Int32,System.String>().Add(1, "A").Add(2, "B").Add(3, "C").Add(4, "D"), new Microsoft.Modeling.Map<System.Int32,FRS2Model.accessLevels>().Add(1, FRS2Model.accessLevels.writeOnly).Add(2, FRS2Model.accessLevels.readWrite).Add(3, FRS2Model.accessLevels.readOnly).Add(4, FRS2Model.accessLevels.readWrite), new Microsoft.Modeling.Map<System.Int32,FRS2Model.connectionProperty>().Add(1, FRS2Model.connectionProperty.enabled).Add(2, FRS2Model.connectionProperty.disabled).Add(3, FRS2Model.connectionProperty.enabled).Add(4, FRS2Model.connectionProperty.enabled));
            this.Manager.Comment("reaching state \'S7\'");
            this.Manager.Comment("checking step \'return Initialization/ERROR_SUCCESS\'");
            this.Manager.Assert((temp31 == FRS2Model.error_status_t.ERROR_SUCCESS), String.Format("expected \'FRS2Model.error_status_t.ERROR_SUCCESS\', actual \'{0}\' (return of Initia" +
                        "lization, state S7)", TestManagerHelpers.Describe(temp31)));
            this.Manager.Comment("reaching state \'S31\'");
            FRS2Model.ProtocolVersionReturned temp32;
            FRS2Model.UpstreamFlagValueReturned temp33;
            FRS2Model.error_status_t temp34;
            this.Manager.Comment("executing step \'call EstablishConnection(\"A\",1,FRS_COMMUNICATION_PROTOCOL_VERSION" +
                    "_LONGHORN_SERVER,out _,out _)\'");
            temp34 = this.IFRS2ManagedAdapterInstance.EstablishConnection("A", 1, FRS2Model.ProtocolVersion.FRS_COMMUNICATION_PROTOCOL_VERSION_LONGHORN_SERVER, out temp32, out temp33);
            this.Manager.Checkpoint("MS-FRS2_R37");
            this.Manager.Checkpoint("MS-FRS2_R436");
            this.Manager.Checkpoint("MS-FRS2_R439");
            this.Manager.Comment("reaching state \'S45\'");
            this.Manager.Comment("checking step \'return EstablishConnection/[out Valid,out Valid]:ERROR_SUCCESS\'");
            this.Manager.Assert((temp32 == FRS2Model.ProtocolVersionReturned.Valid), String.Format("expected \'FRS2Model.ProtocolVersionReturned.Valid\', actual \'{0}\' (upstreamProtoco" +
                        "lVersion of EstablishConnection, state S45)", TestManagerHelpers.Describe(temp32)));
            this.Manager.Assert((temp33 == FRS2Model.UpstreamFlagValueReturned.Valid), String.Format("expected \'FRS2Model.UpstreamFlagValueReturned.Valid\', actual \'{0}\' (upstreamFlags" +
                        " of EstablishConnection, state S45)", TestManagerHelpers.Describe(temp33)));
            this.Manager.Assert((temp34 == FRS2Model.error_status_t.ERROR_SUCCESS), String.Format("expected \'FRS2Model.error_status_t.ERROR_SUCCESS\', actual \'{0}\' (return of Establ" +
                        "ishConnection, state S45)", TestManagerHelpers.Describe(temp34)));
            this.Manager.Comment("reaching state \'S59\'");
            FRS2Model.error_status_t temp35;
            this.Manager.Comment("executing step \'call EstablishSession(1,1)\'");
            temp35 = this.IFRS2ManagedAdapterInstance.EstablishSession(1, 1);
            this.Manager.Checkpoint("MS-FRS2_R455");
            this.Manager.Checkpoint("MS-FRS2_R458");
            this.Manager.Checkpoint("MS-FRS2_R448");
            this.Manager.Comment("reaching state \'S72\'");
            this.Manager.Comment("checking step \'return EstablishSession/ERROR_SUCCESS\'");
            this.Manager.Assert((temp35 == FRS2Model.error_status_t.ERROR_SUCCESS), String.Format("expected \'FRS2Model.error_status_t.ERROR_SUCCESS\', actual \'{0}\' (return of Establ" +
                        "ishSession, state S72)", TestManagerHelpers.Describe(temp35)));
            this.Manager.Comment("reaching state \'S84\'");
            FRS2Model.error_status_t temp36;
            this.Manager.Comment("executing step \'call AsyncPoll(1)\'");
            temp36 = this.IFRS2ManagedAdapterInstance.AsyncPoll(1);
            this.Manager.Checkpoint("MS-FRS2_R549");
            this.Manager.Checkpoint("MS-FRS2_R552");
            this.Manager.Comment("reaching state \'S96\'");
            this.Manager.Comment("checking step \'return AsyncPoll/ERROR_SUCCESS\'");
            this.Manager.Assert((temp36 == FRS2Model.error_status_t.ERROR_SUCCESS), String.Format("expected \'FRS2Model.error_status_t.ERROR_SUCCESS\', actual \'{0}\' (return of AsyncP" +
                        "oll, state S96)", TestManagerHelpers.Describe(temp36)));
            this.Manager.Comment("reaching state \'S107\'");
            FRS2Model.error_status_t temp37;
            this.Manager.Comment("executing step \'call RequestVersionVector(1,1,1,REQUEST_SLOW_SYNC,CHANGE_ALL,Vali" +
                    "dValue)\'");
            temp37 = this.IFRS2ManagedAdapterInstance.RequestVersionVector(1, 1, 1, FRS2Model.VERSION_REQUEST_TYPE.REQUEST_SLOW_SYNC, FRS2Model.VERSION_CHANGE_TYPE.CHANGE_ALL, FRS2Model.VVGeneration.ValidValue);
            this.Manager.Checkpoint("MS-FRS2_R517");
            this.Manager.Checkpoint("MS-FRS2_R520");
            this.Manager.Checkpoint("MS-FRS2_R466");
            this.Manager.Comment("reaching state \'S118\'");
            this.Manager.Comment("checking step \'return RequestVersionVector/ERROR_SUCCESS\'");
            this.Manager.Assert((temp37 == FRS2Model.error_status_t.ERROR_SUCCESS), String.Format("expected \'FRS2Model.error_status_t.ERROR_SUCCESS\', actual \'{0}\' (return of Reques" +
                        "tVersionVector, state S118)", TestManagerHelpers.Describe(temp37)));
            this.Manager.Comment("reaching state \'S126\'");
            int temp45 = this.Manager.ExpectEvent(this.QuiescenceTimeout, true, new ExpectedEvent(TCtestScenario5.AsyncPollResponseEventInfo, null, new AsyncPollResponseEventDelegate1(this.TCtestScenario5S6AsyncPollResponseEventChecker)), new ExpectedEvent(TCtestScenario5.AsyncPollResponseEventInfo, null, new AsyncPollResponseEventDelegate1(this.TCtestScenario5S6AsyncPollResponseEventChecker1)));
            if ((temp45 == 0)) {
                this.Manager.Comment("reaching state \'S136\'");
                FRS2Model.error_status_t temp38;
                this.Manager.Comment("executing step \'call RequestRecords(1,1)\'");
                temp38 = this.IFRS2ManagedAdapterInstance.RequestRecords(1, 1);
                this.Manager.Checkpoint("MS-FRS2_R581");
                this.Manager.Checkpoint("MS-FRS2_R584");
                this.Manager.Comment("reaching state \'S144\'");
                this.Manager.Comment("checking step \'return RequestRecords/ERROR_SUCCESS\'");
                this.Manager.Assert((temp38 == FRS2Model.error_status_t.ERROR_SUCCESS), String.Format("expected \'FRS2Model.error_status_t.ERROR_SUCCESS\', actual \'{0}\' (return of Reques" +
                            "tRecords, state S144)", TestManagerHelpers.Describe(temp38)));
                this.Manager.Comment("reaching state \'S149\'");
                int temp44 = this.Manager.ExpectEvent(this.ProceedControlTimeout, false, new ExpectedEvent(TCtestScenario5.RequestRecordsEventInfo, null, new RequestRecordsEventDelegate1(this.TCtestScenario5S6RequestRecordsEventChecker)), new ExpectedEvent(TCtestScenario5.RequestRecordsEventInfo, null, new RequestRecordsEventDelegate1(this.TCtestScenario5S6RequestRecordsEventChecker1)), new ExpectedEvent(TCtestScenario5.RequestRecordsEventInfo, null, new RequestRecordsEventDelegate1(this.TCtestScenario5S6RequestRecordsEventChecker4)), new ExpectedEvent(TCtestScenario5.RequestRecordsEventInfo, null, new RequestRecordsEventDelegate1(this.TCtestScenario5S6RequestRecordsEventChecker5)));
                if ((temp44 == 0)) {
                    TCtestScenario5S155();
                    goto label10;
                }
                if ((temp44 == 1)) {
                    this.Manager.Comment("reaching state \'S160\'");
                    int temp40 = this.Manager.ExpectEvent(this.ProceedControlTimeout, false, new ExpectedEvent(TCtestScenario5.RequestRecordsEventInfo, null, new RequestRecordsEventDelegate1(this.TCtestScenario5S6RequestRecordsEventChecker2)), new ExpectedEvent(TCtestScenario5.RequestRecordsEventInfo, null, new RequestRecordsEventDelegate1(this.TCtestScenario5S6RequestRecordsEventChecker3)));
                    if ((temp40 == 0)) {
                        TCtestScenario5S170();
                        goto label8;
                    }
                    if ((temp40 == 1)) {
                        TCtestScenario5S169();
                        goto label8;
                    }
                    FRS2Model.error_status_t temp39;
                    this.Manager.Comment("executing step \'call RequestRecords(1,1)\'");
                    temp39 = this.IFRS2ManagedAdapterInstance.RequestRecords(1, 1);
                    this.Manager.Checkpoint("MS-FRS2_R581");
                    this.Manager.Checkpoint("MS-FRS2_R584");
                    this.Manager.AddReturn(RequestRecordsInfo, null, temp39);
                    TCtestScenario5S168();
                label8:
;
                    goto label10;
                }
                if ((temp44 == 2)) {
                    TCtestScenario5S154();
                    goto label10;
                }
                if ((temp44 == 3)) {
                    this.Manager.Comment("reaching state \'S162\'");
                    int temp42 = this.Manager.ExpectEvent(this.ProceedControlTimeout, false, new ExpectedEvent(TCtestScenario5.RequestRecordsEventInfo, null, new RequestRecordsEventDelegate1(this.TCtestScenario5S6RequestRecordsEventChecker6)), new ExpectedEvent(TCtestScenario5.RequestRecordsEventInfo, null, new RequestRecordsEventDelegate1(this.TCtestScenario5S6RequestRecordsEventChecker7)));
                    if ((temp42 == 0)) {
                        TCtestScenario5S167();
                        goto label9;
                    }
                    if ((temp42 == 1)) {
                        TCtestScenario5S166();
                        goto label9;
                    }
                    FRS2Model.error_status_t temp41;
                    this.Manager.Comment("executing step \'call RequestRecords(1,1)\'");
                    temp41 = this.IFRS2ManagedAdapterInstance.RequestRecords(1, 1);
                    this.Manager.Checkpoint("MS-FRS2_R581");
                    this.Manager.Checkpoint("MS-FRS2_R584");
                    this.Manager.AddReturn(RequestRecordsInfo, null, temp41);
                    TCtestScenario5S165();
                label9:
;
                    goto label10;
                }
                FRS2Model.error_status_t temp43;
                this.Manager.Comment("executing step \'call RequestRecords(1,1)\'");
                temp43 = this.IFRS2ManagedAdapterInstance.RequestRecords(1, 1);
                this.Manager.Checkpoint("MS-FRS2_R581");
                this.Manager.Checkpoint("MS-FRS2_R584");
                this.Manager.AddReturn(RequestRecordsInfo, null, temp43);
                TCtestScenario5S143();
            label10:
;
                goto label11;
            }
            if ((temp45 == 1)) {
                TCtestScenario5S133();
                goto label11;
            }
            throw new InvalidOperationException("never reached");
        label11:
;
            this.Manager.EndTest();
        }
        
        private void TCtestScenario5S6AsyncPollResponseEventChecker(FRS2Model.VVGeneration vvGen) {
            this.Manager.Comment("checking step \'event AsyncPollResponseEvent(ValidValue)\'");
            try {
                this.Manager.Assert((vvGen == FRS2Model.VVGeneration.ValidValue), String.Format("expected \'FRS2Model.VVGeneration.ValidValue\', actual \'{0}\' (vvGen of AsyncPollRes" +
                            "ponseEvent, state S126)", TestManagerHelpers.Describe(vvGen)));
            }
            catch (TransactionFailedException ) {
                this.Manager.Comment("This step would have covered MS-FRS2_R556, MS-FRS2_R1020, MS-FRS2_R555");
                throw;
            }
            this.Manager.Checkpoint("MS-FRS2_R556");
            this.Manager.Checkpoint("MS-FRS2_R1020");
            this.Manager.Checkpoint("MS-FRS2_R555");
        }
        
        private void TCtestScenario5S6RequestRecordsEventChecker(FRS2Model.RecordsStatus status) {
            this.Manager.Comment("checking step \'event RequestRecordsEvent(RECORDS_STATUS_MORE)\'");
            try {
                this.Manager.Assert((status == FRS2Model.RecordsStatus.RECORDS_STATUS_MORE), String.Format("expected \'FRS2Model.RecordsStatus.RECORDS_STATUS_MORE\', actual \'{0}\' (status of R" +
                            "equestRecordsEvent, state S149)", TestManagerHelpers.Describe(status)));
            }
            catch (TransactionFailedException ) {
                this.Manager.Comment("This step would have covered MS-FRS2_R100");
                throw;
            }
            this.Manager.Checkpoint("MS-FRS2_R100");
        }
        
        private void TCtestScenario5S6RequestRecordsEventChecker1(FRS2Model.RecordsStatus status) {
            this.Manager.Comment("checking step \'event RequestRecordsEvent(RECORDS_STATUS_MORE)\'");
            try {
                this.Manager.Assert((status == FRS2Model.RecordsStatus.RECORDS_STATUS_MORE), String.Format("expected \'FRS2Model.RecordsStatus.RECORDS_STATUS_MORE\', actual \'{0}\' (status of R" +
                            "equestRecordsEvent, state S149)", TestManagerHelpers.Describe(status)));
            }
            catch (TransactionFailedException ) {
                this.Manager.Comment("This step would have covered MS-FRS2_R100");
                throw;
            }
            this.Manager.Checkpoint("MS-FRS2_R100");
        }
        
        private void TCtestScenario5S6RequestRecordsEventChecker2(FRS2Model.RecordsStatus status) {
            this.Manager.Comment("checking step \'event RequestRecordsEvent(RECORDS_STATUS_MORE)\'");
            try {
                this.Manager.Assert((status == FRS2Model.RecordsStatus.RECORDS_STATUS_MORE), String.Format("expected \'FRS2Model.RecordsStatus.RECORDS_STATUS_MORE\', actual \'{0}\' (status of R" +
                            "equestRecordsEvent, state S160)", TestManagerHelpers.Describe(status)));
            }
            catch (TransactionFailedException ) {
                this.Manager.Comment("This step would have covered MS-FRS2_R100");
                throw;
            }
            this.Manager.Checkpoint("MS-FRS2_R100");
        }
        
        private void TCtestScenario5S6RequestRecordsEventChecker3(FRS2Model.RecordsStatus status) {
            this.Manager.Comment("checking step \'event RequestRecordsEvent(RECORDS_STATUS_DONE)\'");
            try {
                this.Manager.Assert((status == FRS2Model.RecordsStatus.RECORDS_STATUS_DONE), String.Format("expected \'FRS2Model.RecordsStatus.RECORDS_STATUS_DONE\', actual \'{0}\' (status of R" +
                            "equestRecordsEvent, state S160)", TestManagerHelpers.Describe(status)));
            }
            catch (TransactionFailedException ) {
                this.Manager.Comment("This step would have covered MS-FRS2_R99");
                throw;
            }
            this.Manager.Checkpoint("MS-FRS2_R99");
        }
        
        private void TCtestScenario5S6RequestRecordsEventChecker4(FRS2Model.RecordsStatus status) {
            this.Manager.Comment("checking step \'event RequestRecordsEvent(RECORDS_STATUS_DONE)\'");
            try {
                this.Manager.Assert((status == FRS2Model.RecordsStatus.RECORDS_STATUS_DONE), String.Format("expected \'FRS2Model.RecordsStatus.RECORDS_STATUS_DONE\', actual \'{0}\' (status of R" +
                            "equestRecordsEvent, state S149)", TestManagerHelpers.Describe(status)));
            }
            catch (TransactionFailedException ) {
                this.Manager.Comment("This step would have covered MS-FRS2_R99");
                throw;
            }
            this.Manager.Checkpoint("MS-FRS2_R99");
        }
        
        private void TCtestScenario5S6RequestRecordsEventChecker5(FRS2Model.RecordsStatus status) {
            this.Manager.Comment("checking step \'event RequestRecordsEvent(RECORDS_STATUS_DONE)\'");
            try {
                this.Manager.Assert((status == FRS2Model.RecordsStatus.RECORDS_STATUS_DONE), String.Format("expected \'FRS2Model.RecordsStatus.RECORDS_STATUS_DONE\', actual \'{0}\' (status of R" +
                            "equestRecordsEvent, state S149)", TestManagerHelpers.Describe(status)));
            }
            catch (TransactionFailedException ) {
                this.Manager.Comment("This step would have covered MS-FRS2_R99");
                throw;
            }
            this.Manager.Checkpoint("MS-FRS2_R99");
        }
        
        private void TCtestScenario5S6RequestRecordsEventChecker6(FRS2Model.RecordsStatus status) {
            this.Manager.Comment("checking step \'event RequestRecordsEvent(RECORDS_STATUS_DONE)\'");
            try {
                this.Manager.Assert((status == FRS2Model.RecordsStatus.RECORDS_STATUS_DONE), String.Format("expected \'FRS2Model.RecordsStatus.RECORDS_STATUS_DONE\', actual \'{0}\' (status of R" +
                            "equestRecordsEvent, state S162)", TestManagerHelpers.Describe(status)));
            }
            catch (TransactionFailedException ) {
                this.Manager.Comment("This step would have covered MS-FRS2_R99");
                throw;
            }
            this.Manager.Checkpoint("MS-FRS2_R99");
        }
        
        private void TCtestScenario5S6RequestRecordsEventChecker7(FRS2Model.RecordsStatus status) {
            this.Manager.Comment("checking step \'event RequestRecordsEvent(RECORDS_STATUS_MORE)\'");
            try {
                this.Manager.Assert((status == FRS2Model.RecordsStatus.RECORDS_STATUS_MORE), String.Format("expected \'FRS2Model.RecordsStatus.RECORDS_STATUS_MORE\', actual \'{0}\' (status of R" +
                            "equestRecordsEvent, state S162)", TestManagerHelpers.Describe(status)));
            }
            catch (TransactionFailedException ) {
                this.Manager.Comment("This step would have covered MS-FRS2_R100");
                throw;
            }
            this.Manager.Checkpoint("MS-FRS2_R100");
        }
        
        private void TCtestScenario5S6AsyncPollResponseEventChecker1(FRS2Model.VVGeneration vvGen) {
            this.Manager.Comment("checking step \'event AsyncPollResponseEvent(InvalidValue)\'");
            try {
                this.Manager.Assert((vvGen == FRS2Model.VVGeneration.InvalidValue), String.Format("expected \'FRS2Model.VVGeneration.InvalidValue\', actual \'{0}\' (vvGen of AsyncPollR" +
                            "esponseEvent, state S126)", TestManagerHelpers.Describe(vvGen)));
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
        [Microsoft.VisualStudio.TestTools.UnitTesting.DescriptionAttribute(@"MS-FRS2_R37, MS-FRS2_R436, MS-FRS2_R439, MS-FRS2_R455, MS-FRS2_R458, MS-FRS2_R448, MS-FRS2_R549, MS-FRS2_R552, MS-FRS2_R517, MS-FRS2_R520, MS-FRS2_R466, MS-FRS2_R556, MS-FRS2_R1020, MS-FRS2_R555, MS-FRS2_R582, MS-FRS2_R588, MS-FRS2_R581, MS-FRS2_R584, MS-FRS2_R556, MS-FRS2_R1020, MS-FRS2_R555")]
        public virtual void FRS2_TCtestScenario5S8() {
            this.Manager.BeginTest("TCtestScenario5S8");
            this.Manager.Comment("reaching state \'S8\'");
            FRS2Model.error_status_t temp46;
            this.Manager.Comment(@"executing step 'call Initialization(Windows7,Map{1=>enabled,2=>enabled,3=>enabled,4=>disabled},Map{1=>exists,2=>exists,3=>exists,4=>exists},Map{""A""=>1,""B""=>2,""C""=>3,""D""=>4},Map{""A""=>sysvol,""B""=>protection,""C""=>sysvol,""D""=>sysvol},Map{1=>""A"",2=>""B"",3=>""C"",4=>""D""},Map{1=>writeOnly,2=>readWrite,3=>readOnly,4=>readWrite},Map{1=>enabled,2=>disabled,3=>enabled,4=>enabled})'");
            temp46 = this.IFRS2ManagedAdapterInstance.Initialization(FRS2Model.OSVersion.Windows7, new Microsoft.Modeling.Map<System.Int32,FRS2Model.connectionProperty>().Add(1, FRS2Model.connectionProperty.enabled).Add(2, FRS2Model.connectionProperty.enabled).Add(3, FRS2Model.connectionProperty.enabled).Add(4, FRS2Model.connectionProperty.disabled), new Microsoft.Modeling.Map<System.Int32,FRS2Model.FromServer>().Add(1, FRS2Model.FromServer.exists).Add(2, FRS2Model.FromServer.exists).Add(3, FRS2Model.FromServer.exists).Add(4, FRS2Model.FromServer.exists), new Microsoft.Modeling.Map<System.String,System.Int32>().Add("A", 1).Add("B", 2).Add("C", 3).Add("D", 4), new Microsoft.Modeling.Map<System.String,FRS2Model.ReplicationGroupTypes>().Add("A", FRS2Model.ReplicationGroupTypes.sysvol).Add("B", FRS2Model.ReplicationGroupTypes.protection).Add("C", FRS2Model.ReplicationGroupTypes.sysvol).Add("D", FRS2Model.ReplicationGroupTypes.sysvol), new Microsoft.Modeling.Map<System.Int32,System.String>().Add(1, "A").Add(2, "B").Add(3, "C").Add(4, "D"), new Microsoft.Modeling.Map<System.Int32,FRS2Model.accessLevels>().Add(1, FRS2Model.accessLevels.writeOnly).Add(2, FRS2Model.accessLevels.readWrite).Add(3, FRS2Model.accessLevels.readOnly).Add(4, FRS2Model.accessLevels.readWrite), new Microsoft.Modeling.Map<System.Int32,FRS2Model.connectionProperty>().Add(1, FRS2Model.connectionProperty.enabled).Add(2, FRS2Model.connectionProperty.disabled).Add(3, FRS2Model.connectionProperty.enabled).Add(4, FRS2Model.connectionProperty.enabled));
            this.Manager.Comment("reaching state \'S9\'");
            this.Manager.Comment("checking step \'return Initialization/ERROR_SUCCESS\'");
            this.Manager.Assert((temp46 == FRS2Model.error_status_t.ERROR_SUCCESS), String.Format("expected \'FRS2Model.error_status_t.ERROR_SUCCESS\', actual \'{0}\' (return of Initia" +
                        "lization, state S9)", TestManagerHelpers.Describe(temp46)));
            this.Manager.Comment("reaching state \'S32\'");
            FRS2Model.ProtocolVersionReturned temp47;
            FRS2Model.UpstreamFlagValueReturned temp48;
            FRS2Model.error_status_t temp49;
            this.Manager.Comment("executing step \'call EstablishConnection(\"A\",1,FRS_COMMUNICATION_PROTOCOL_VERSION" +
                    "_LONGHORN_SERVER,out _,out _)\'");
            temp49 = this.IFRS2ManagedAdapterInstance.EstablishConnection("A", 1, FRS2Model.ProtocolVersion.FRS_COMMUNICATION_PROTOCOL_VERSION_LONGHORN_SERVER, out temp47, out temp48);
            this.Manager.Checkpoint("MS-FRS2_R37");
            this.Manager.Checkpoint("MS-FRS2_R436");
            this.Manager.Checkpoint("MS-FRS2_R439");
            this.Manager.Comment("reaching state \'S46\'");
            this.Manager.Comment("checking step \'return EstablishConnection/[out Valid,out Valid]:ERROR_SUCCESS\'");
            this.Manager.Assert((temp47 == FRS2Model.ProtocolVersionReturned.Valid), String.Format("expected \'FRS2Model.ProtocolVersionReturned.Valid\', actual \'{0}\' (upstreamProtoco" +
                        "lVersion of EstablishConnection, state S46)", TestManagerHelpers.Describe(temp47)));
            this.Manager.Assert((temp48 == FRS2Model.UpstreamFlagValueReturned.Valid), String.Format("expected \'FRS2Model.UpstreamFlagValueReturned.Valid\', actual \'{0}\' (upstreamFlags" +
                        " of EstablishConnection, state S46)", TestManagerHelpers.Describe(temp48)));
            this.Manager.Assert((temp49 == FRS2Model.error_status_t.ERROR_SUCCESS), String.Format("expected \'FRS2Model.error_status_t.ERROR_SUCCESS\', actual \'{0}\' (return of Establ" +
                        "ishConnection, state S46)", TestManagerHelpers.Describe(temp49)));
            this.Manager.Comment("reaching state \'S60\'");
            FRS2Model.error_status_t temp50;
            this.Manager.Comment("executing step \'call EstablishSession(1,1)\'");
            temp50 = this.IFRS2ManagedAdapterInstance.EstablishSession(1, 1);
            this.Manager.Checkpoint("MS-FRS2_R455");
            this.Manager.Checkpoint("MS-FRS2_R458");
            this.Manager.Checkpoint("MS-FRS2_R448");
            this.Manager.Comment("reaching state \'S73\'");
            this.Manager.Comment("checking step \'return EstablishSession/ERROR_SUCCESS\'");
            this.Manager.Assert((temp50 == FRS2Model.error_status_t.ERROR_SUCCESS), String.Format("expected \'FRS2Model.error_status_t.ERROR_SUCCESS\', actual \'{0}\' (return of Establ" +
                        "ishSession, state S73)", TestManagerHelpers.Describe(temp50)));
            this.Manager.Comment("reaching state \'S85\'");
            FRS2Model.error_status_t temp51;
            this.Manager.Comment("executing step \'call AsyncPoll(1)\'");
            temp51 = this.IFRS2ManagedAdapterInstance.AsyncPoll(1);
            this.Manager.Checkpoint("MS-FRS2_R549");
            this.Manager.Checkpoint("MS-FRS2_R552");
            this.Manager.Comment("reaching state \'S97\'");
            this.Manager.Comment("checking step \'return AsyncPoll/ERROR_SUCCESS\'");
            this.Manager.Assert((temp51 == FRS2Model.error_status_t.ERROR_SUCCESS), String.Format("expected \'FRS2Model.error_status_t.ERROR_SUCCESS\', actual \'{0}\' (return of AsyncP" +
                        "oll, state S97)", TestManagerHelpers.Describe(temp51)));
            this.Manager.Comment("reaching state \'S108\'");
            FRS2Model.error_status_t temp52;
            this.Manager.Comment("executing step \'call RequestVersionVector(1,1,1,REQUEST_SLOW_SYNC,CHANGE_ALL,Vali" +
                    "dValue)\'");
            temp52 = this.IFRS2ManagedAdapterInstance.RequestVersionVector(1, 1, 1, FRS2Model.VERSION_REQUEST_TYPE.REQUEST_SLOW_SYNC, FRS2Model.VERSION_CHANGE_TYPE.CHANGE_ALL, FRS2Model.VVGeneration.ValidValue);
            this.Manager.Checkpoint("MS-FRS2_R517");
            this.Manager.Checkpoint("MS-FRS2_R520");
            this.Manager.Checkpoint("MS-FRS2_R466");
            this.Manager.Comment("reaching state \'S119\'");
            this.Manager.Comment("checking step \'return RequestVersionVector/ERROR_SUCCESS\'");
            this.Manager.Assert((temp52 == FRS2Model.error_status_t.ERROR_SUCCESS), String.Format("expected \'FRS2Model.error_status_t.ERROR_SUCCESS\', actual \'{0}\' (return of Reques" +
                        "tVersionVector, state S119)", TestManagerHelpers.Describe(temp52)));
            this.Manager.Comment("reaching state \'S127\'");
            int temp55 = this.Manager.ExpectEvent(this.QuiescenceTimeout, true, new ExpectedEvent(TCtestScenario5.AsyncPollResponseEventInfo, null, new AsyncPollResponseEventDelegate1(this.TCtestScenario5S8AsyncPollResponseEventChecker)), new ExpectedEvent(TCtestScenario5.AsyncPollResponseEventInfo, null, new AsyncPollResponseEventDelegate1(this.TCtestScenario5S8AsyncPollResponseEventChecker1)));
            if ((temp55 == 0)) {
                this.Manager.Comment("reaching state \'S138\'");
                FRS2Model.error_status_t temp53;
                this.Manager.Comment("executing step \'call RequestRecords(4,1)\'");
                temp53 = this.IFRS2ManagedAdapterInstance.RequestRecords(4, 1);
                this.Manager.Checkpoint("MS-FRS2_R582");
                this.Manager.Checkpoint("MS-FRS2_R588");
                this.Manager.Comment("reaching state \'S145\'");
                this.Manager.Comment("checking step \'return RequestRecords/ERROR_FAIL\'");
                this.Manager.Assert((temp53 == FRS2Model.error_status_t.ERROR_FAIL), String.Format("expected \'FRS2Model.error_status_t.ERROR_FAIL\', actual \'{0}\' (return of RequestRe" +
                            "cords, state S145)", TestManagerHelpers.Describe(temp53)));
                TCtestScenario5S150();
                goto label12;
            }
            if ((temp55 == 1)) {
                TCtestScenario5S133();
                goto label12;
            }
            throw new InvalidOperationException("never reached");
        label12:
;
            this.Manager.EndTest();
        }
        
        private void TCtestScenario5S8AsyncPollResponseEventChecker(FRS2Model.VVGeneration vvGen) {
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
        
        private void TCtestScenario5S150() {
            this.Manager.Comment("reaching state \'S150\'");
            FRS2Model.error_status_t temp54;
            this.Manager.Comment("executing step \'call RequestRecords(1,1)\'");
            temp54 = this.IFRS2ManagedAdapterInstance.RequestRecords(1, 1);
            this.Manager.Checkpoint("MS-FRS2_R581");
            this.Manager.Checkpoint("MS-FRS2_R584");
            this.Manager.AddReturn(RequestRecordsInfo, null, temp54);
            TCtestScenario5S143();
        }
        
        private void TCtestScenario5S8AsyncPollResponseEventChecker1(FRS2Model.VVGeneration vvGen) {
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
        
        #region Test Starting in S10
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        [Microsoft.VisualStudio.TestTools.UnitTesting.DescriptionAttribute("MS-FRS2_R37, MS-FRS2_R436, MS-FRS2_R439, MS-FRS2_R455, MS-FRS2_R458, MS-FRS2_R448" +
            ", MS-FRS2_R549, MS-FRS2_R552, MS-FRS2_R518")]
        public virtual void FRS2_TCtestScenario5S10() {
            this.Manager.BeginTest("TCtestScenario5S10");
            this.Manager.Comment("reaching state \'S10\'");
            FRS2Model.error_status_t temp56;
            this.Manager.Comment(@"executing step 'call Initialization(Windows7,Map{1=>enabled,2=>enabled,3=>enabled,4=>disabled},Map{1=>exists,2=>exists,3=>exists,4=>exists},Map{""A""=>1,""B""=>2,""C""=>3,""D""=>4},Map{""A""=>sysvol,""B""=>protection,""C""=>sysvol,""D""=>sysvol},Map{1=>""A"",2=>""B"",3=>""C"",4=>""D""},Map{1=>writeOnly,2=>readWrite,3=>readOnly,4=>readWrite},Map{1=>enabled,2=>disabled,3=>enabled,4=>enabled})'");
            temp56 = this.IFRS2ManagedAdapterInstance.Initialization(FRS2Model.OSVersion.Windows7, new Microsoft.Modeling.Map<System.Int32,FRS2Model.connectionProperty>().Add(1, FRS2Model.connectionProperty.enabled).Add(2, FRS2Model.connectionProperty.enabled).Add(3, FRS2Model.connectionProperty.enabled).Add(4, FRS2Model.connectionProperty.disabled), new Microsoft.Modeling.Map<System.Int32,FRS2Model.FromServer>().Add(1, FRS2Model.FromServer.exists).Add(2, FRS2Model.FromServer.exists).Add(3, FRS2Model.FromServer.exists).Add(4, FRS2Model.FromServer.exists), new Microsoft.Modeling.Map<System.String,System.Int32>().Add("A", 1).Add("B", 2).Add("C", 3).Add("D", 4), new Microsoft.Modeling.Map<System.String,FRS2Model.ReplicationGroupTypes>().Add("A", FRS2Model.ReplicationGroupTypes.sysvol).Add("B", FRS2Model.ReplicationGroupTypes.protection).Add("C", FRS2Model.ReplicationGroupTypes.sysvol).Add("D", FRS2Model.ReplicationGroupTypes.sysvol), new Microsoft.Modeling.Map<System.Int32,System.String>().Add(1, "A").Add(2, "B").Add(3, "C").Add(4, "D"), new Microsoft.Modeling.Map<System.Int32,FRS2Model.accessLevels>().Add(1, FRS2Model.accessLevels.writeOnly).Add(2, FRS2Model.accessLevels.readWrite).Add(3, FRS2Model.accessLevels.readOnly).Add(4, FRS2Model.accessLevels.readWrite), new Microsoft.Modeling.Map<System.Int32,FRS2Model.connectionProperty>().Add(1, FRS2Model.connectionProperty.enabled).Add(2, FRS2Model.connectionProperty.disabled).Add(3, FRS2Model.connectionProperty.enabled).Add(4, FRS2Model.connectionProperty.enabled));
            this.Manager.Comment("reaching state \'S11\'");
            this.Manager.Comment("checking step \'return Initialization/ERROR_SUCCESS\'");
            this.Manager.Assert((temp56 == FRS2Model.error_status_t.ERROR_SUCCESS), String.Format("expected \'FRS2Model.error_status_t.ERROR_SUCCESS\', actual \'{0}\' (return of Initia" +
                        "lization, state S11)", TestManagerHelpers.Describe(temp56)));
            this.Manager.Comment("reaching state \'S33\'");
            FRS2Model.ProtocolVersionReturned temp57;
            FRS2Model.UpstreamFlagValueReturned temp58;
            FRS2Model.error_status_t temp59;
            this.Manager.Comment("executing step \'call EstablishConnection(\"A\",1,FRS_COMMUNICATION_PROTOCOL_VERSION" +
                    "_LONGHORN_SERVER,out _,out _)\'");
            temp59 = this.IFRS2ManagedAdapterInstance.EstablishConnection("A", 1, FRS2Model.ProtocolVersion.FRS_COMMUNICATION_PROTOCOL_VERSION_LONGHORN_SERVER, out temp57, out temp58);
            this.Manager.Checkpoint("MS-FRS2_R37");
            this.Manager.Checkpoint("MS-FRS2_R436");
            this.Manager.Checkpoint("MS-FRS2_R439");
            this.Manager.Comment("reaching state \'S47\'");
            this.Manager.Comment("checking step \'return EstablishConnection/[out Valid,out Valid]:ERROR_SUCCESS\'");
            this.Manager.Assert((temp57 == FRS2Model.ProtocolVersionReturned.Valid), String.Format("expected \'FRS2Model.ProtocolVersionReturned.Valid\', actual \'{0}\' (upstreamProtoco" +
                        "lVersion of EstablishConnection, state S47)", TestManagerHelpers.Describe(temp57)));
            this.Manager.Assert((temp58 == FRS2Model.UpstreamFlagValueReturned.Valid), String.Format("expected \'FRS2Model.UpstreamFlagValueReturned.Valid\', actual \'{0}\' (upstreamFlags" +
                        " of EstablishConnection, state S47)", TestManagerHelpers.Describe(temp58)));
            this.Manager.Assert((temp59 == FRS2Model.error_status_t.ERROR_SUCCESS), String.Format("expected \'FRS2Model.error_status_t.ERROR_SUCCESS\', actual \'{0}\' (return of Establ" +
                        "ishConnection, state S47)", TestManagerHelpers.Describe(temp59)));
            this.Manager.Comment("reaching state \'S61\'");
            FRS2Model.error_status_t temp60;
            this.Manager.Comment("executing step \'call EstablishSession(1,1)\'");
            temp60 = this.IFRS2ManagedAdapterInstance.EstablishSession(1, 1);
            this.Manager.Checkpoint("MS-FRS2_R455");
            this.Manager.Checkpoint("MS-FRS2_R458");
            this.Manager.Checkpoint("MS-FRS2_R448");
            this.Manager.Comment("reaching state \'S74\'");
            this.Manager.Comment("checking step \'return EstablishSession/ERROR_SUCCESS\'");
            this.Manager.Assert((temp60 == FRS2Model.error_status_t.ERROR_SUCCESS), String.Format("expected \'FRS2Model.error_status_t.ERROR_SUCCESS\', actual \'{0}\' (return of Establ" +
                        "ishSession, state S74)", TestManagerHelpers.Describe(temp60)));
            this.Manager.Comment("reaching state \'S86\'");
            FRS2Model.error_status_t temp61;
            this.Manager.Comment("executing step \'call AsyncPoll(1)\'");
            temp61 = this.IFRS2ManagedAdapterInstance.AsyncPoll(1);
            this.Manager.Checkpoint("MS-FRS2_R549");
            this.Manager.Checkpoint("MS-FRS2_R552");
            this.Manager.Comment("reaching state \'S98\'");
            this.Manager.Comment("checking step \'return AsyncPoll/ERROR_SUCCESS\'");
            this.Manager.Assert((temp61 == FRS2Model.error_status_t.ERROR_SUCCESS), String.Format("expected \'FRS2Model.error_status_t.ERROR_SUCCESS\', actual \'{0}\' (return of AsyncP" +
                        "oll, state S98)", TestManagerHelpers.Describe(temp61)));
            this.Manager.Comment("reaching state \'S109\'");
            FRS2Model.error_status_t temp62;
            this.Manager.Comment("executing step \'call RequestVersionVector(1,1,1,REQUEST_SLOW_SYNC,CHANGE_NOTIFY,I" +
                    "nvalidValue)\'");
            temp62 = this.IFRS2ManagedAdapterInstance.RequestVersionVector(1, 1, 1, FRS2Model.VERSION_REQUEST_TYPE.REQUEST_SLOW_SYNC, FRS2Model.VERSION_CHANGE_TYPE.CHANGE_NOTIFY, FRS2Model.VVGeneration.InvalidValue);
            this.Manager.Checkpoint("MS-FRS2_R518");
            this.Manager.Comment("reaching state \'S120\'");
            this.Manager.Comment("checking step \'return RequestVersionVector/ERROR_SUCCESS\'");
            this.Manager.Assert((temp62 == FRS2Model.error_status_t.ERROR_SUCCESS), String.Format("expected \'FRS2Model.error_status_t.ERROR_SUCCESS\', actual \'{0}\' (return of Reques" +
                        "tVersionVector, state S120)", TestManagerHelpers.Describe(temp62)));
            TCtestScenario5S128();
            this.Manager.EndTest();
        }
        
        private void TCtestScenario5S128() {
            this.Manager.Comment("reaching state \'S128\'");
        }
        #endregion
        
        #region Test Starting in S12
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        [Microsoft.VisualStudio.TestTools.UnitTesting.DescriptionAttribute(@"MS-FRS2_R37, MS-FRS2_R436, MS-FRS2_R439, MS-FRS2_R455, MS-FRS2_R458, MS-FRS2_R448, MS-FRS2_R549, MS-FRS2_R552, MS-FRS2_R517, MS-FRS2_R520, MS-FRS2_R466, MS-FRS2_R556, MS-FRS2_R1020, MS-FRS2_R555, MS-FRS2_R585, MS-FRS2_R589, MS-FRS2_R556, MS-FRS2_R1020, MS-FRS2_R555")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestCategory("AD")]
        [TestCategory("MS-FRS2")]
        [TestCategory("SDC")]
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        
        public virtual void FRS2_TCtestScenario5S12()
        {
            this.Manager.BeginTest("TCtestScenario5S12");
            this.Manager.Comment("reaching state \'S12\'");
            FRS2Model.error_status_t temp63;
            this.Manager.Comment(@"executing step 'call Initialization(Windows7,Map{1=>enabled,2=>enabled,3=>enabled,4=>disabled},Map{1=>exists,2=>exists,3=>exists,4=>exists},Map{""A""=>1,""B""=>2,""C""=>3,""D""=>4},Map{""A""=>sysvol,""B""=>protection,""C""=>sysvol,""D""=>sysvol},Map{1=>""A"",2=>""B"",3=>""C"",4=>""D""},Map{1=>writeOnly,2=>readWrite,3=>readOnly,4=>readWrite},Map{1=>enabled,2=>disabled,3=>enabled,4=>enabled})'");
            temp63 = this.IFRS2ManagedAdapterInstance.Initialization(FRS2Model.OSVersion.Windows7, new Microsoft.Modeling.Map<System.Int32,FRS2Model.connectionProperty>().Add(1, FRS2Model.connectionProperty.enabled).Add(2, FRS2Model.connectionProperty.enabled).Add(3, FRS2Model.connectionProperty.enabled).Add(4, FRS2Model.connectionProperty.disabled), new Microsoft.Modeling.Map<System.Int32,FRS2Model.FromServer>().Add(1, FRS2Model.FromServer.exists).Add(2, FRS2Model.FromServer.exists).Add(3, FRS2Model.FromServer.exists).Add(4, FRS2Model.FromServer.exists), new Microsoft.Modeling.Map<System.String,System.Int32>().Add("A", 1).Add("B", 2).Add("C", 3).Add("D", 4), new Microsoft.Modeling.Map<System.String,FRS2Model.ReplicationGroupTypes>().Add("A", FRS2Model.ReplicationGroupTypes.sysvol).Add("B", FRS2Model.ReplicationGroupTypes.protection).Add("C", FRS2Model.ReplicationGroupTypes.sysvol).Add("D", FRS2Model.ReplicationGroupTypes.sysvol), new Microsoft.Modeling.Map<System.Int32,System.String>().Add(1, "A").Add(2, "B").Add(3, "C").Add(4, "D"), new Microsoft.Modeling.Map<System.Int32,FRS2Model.accessLevels>().Add(1, FRS2Model.accessLevels.writeOnly).Add(2, FRS2Model.accessLevels.readWrite).Add(3, FRS2Model.accessLevels.readOnly).Add(4, FRS2Model.accessLevels.readWrite), new Microsoft.Modeling.Map<System.Int32,FRS2Model.connectionProperty>().Add(1, FRS2Model.connectionProperty.enabled).Add(2, FRS2Model.connectionProperty.disabled).Add(3, FRS2Model.connectionProperty.enabled).Add(4, FRS2Model.connectionProperty.enabled));
            this.Manager.Comment("reaching state \'S13\'");
            this.Manager.Comment("checking step \'return Initialization/ERROR_SUCCESS\'");
            this.Manager.Assert((temp63 == FRS2Model.error_status_t.ERROR_SUCCESS), String.Format("expected \'FRS2Model.error_status_t.ERROR_SUCCESS\', actual \'{0}\' (return of Initia" +
                        "lization, state S13)", TestManagerHelpers.Describe(temp63)));
            this.Manager.Comment("reaching state \'S34\'");
            FRS2Model.ProtocolVersionReturned temp64;
            FRS2Model.UpstreamFlagValueReturned temp65;
            FRS2Model.error_status_t temp66;
            this.Manager.Comment("executing step \'call EstablishConnection(\"A\",1,FRS_COMMUNICATION_PROTOCOL_VERSION" +
                    "_LONGHORN_SERVER,out _,out _)\'");
            temp66 = this.IFRS2ManagedAdapterInstance.EstablishConnection("A", 1, FRS2Model.ProtocolVersion.FRS_COMMUNICATION_PROTOCOL_VERSION_LONGHORN_SERVER, out temp64, out temp65);
            this.Manager.Checkpoint("MS-FRS2_R37");
            this.Manager.Checkpoint("MS-FRS2_R436");
            this.Manager.Checkpoint("MS-FRS2_R439");
            this.Manager.Comment("reaching state \'S48\'");
            this.Manager.Comment("checking step \'return EstablishConnection/[out Valid,out Valid]:ERROR_SUCCESS\'");
            this.Manager.Assert((temp64 == FRS2Model.ProtocolVersionReturned.Valid), String.Format("expected \'FRS2Model.ProtocolVersionReturned.Valid\', actual \'{0}\' (upstreamProtoco" +
                        "lVersion of EstablishConnection, state S48)", TestManagerHelpers.Describe(temp64)));
            this.Manager.Assert((temp65 == FRS2Model.UpstreamFlagValueReturned.Valid), String.Format("expected \'FRS2Model.UpstreamFlagValueReturned.Valid\', actual \'{0}\' (upstreamFlags" +
                        " of EstablishConnection, state S48)", TestManagerHelpers.Describe(temp65)));
            this.Manager.Assert((temp66 == FRS2Model.error_status_t.ERROR_SUCCESS), String.Format("expected \'FRS2Model.error_status_t.ERROR_SUCCESS\', actual \'{0}\' (return of Establ" +
                        "ishConnection, state S48)", TestManagerHelpers.Describe(temp66)));
            this.Manager.Comment("reaching state \'S62\'");
            FRS2Model.error_status_t temp67;
            this.Manager.Comment("executing step \'call EstablishSession(1,1)\'");
            temp67 = this.IFRS2ManagedAdapterInstance.EstablishSession(1, 1);
            this.Manager.Checkpoint("MS-FRS2_R455");
            this.Manager.Checkpoint("MS-FRS2_R458");
            this.Manager.Checkpoint("MS-FRS2_R448");
            this.Manager.Comment("reaching state \'S75\'");
            this.Manager.Comment("checking step \'return EstablishSession/ERROR_SUCCESS\'");
            this.Manager.Assert((temp67 == FRS2Model.error_status_t.ERROR_SUCCESS), String.Format("expected \'FRS2Model.error_status_t.ERROR_SUCCESS\', actual \'{0}\' (return of Establ" +
                        "ishSession, state S75)", TestManagerHelpers.Describe(temp67)));
            this.Manager.Comment("reaching state \'S87\'");
            FRS2Model.error_status_t temp68;
            this.Manager.Comment("executing step \'call AsyncPoll(1)\'");
            temp68 = this.IFRS2ManagedAdapterInstance.AsyncPoll(1);
            this.Manager.Checkpoint("MS-FRS2_R549");
            this.Manager.Checkpoint("MS-FRS2_R552");
            this.Manager.Comment("reaching state \'S99\'");
            this.Manager.Comment("checking step \'return AsyncPoll/ERROR_SUCCESS\'");
            this.Manager.Assert((temp68 == FRS2Model.error_status_t.ERROR_SUCCESS), String.Format("expected \'FRS2Model.error_status_t.ERROR_SUCCESS\', actual \'{0}\' (return of AsyncP" +
                        "oll, state S99)", TestManagerHelpers.Describe(temp68)));
            this.Manager.Comment("reaching state \'S110\'");
            FRS2Model.error_status_t temp69;
            this.Manager.Comment("executing step \'call RequestVersionVector(1,1,1,REQUEST_SLOW_SYNC,CHANGE_ALL,Vali" +
                    "dValue)\'");
            temp69 = this.IFRS2ManagedAdapterInstance.RequestVersionVector(1, 1, 1, FRS2Model.VERSION_REQUEST_TYPE.REQUEST_SLOW_SYNC, FRS2Model.VERSION_CHANGE_TYPE.CHANGE_ALL, FRS2Model.VVGeneration.ValidValue);
            this.Manager.Checkpoint("MS-FRS2_R517");
            this.Manager.Checkpoint("MS-FRS2_R520");
            this.Manager.Checkpoint("MS-FRS2_R466");
            this.Manager.Comment("reaching state \'S121\'");
            this.Manager.Comment("checking step \'return RequestVersionVector/ERROR_SUCCESS\'");
            this.Manager.Assert((temp69 == FRS2Model.error_status_t.ERROR_SUCCESS), String.Format("expected \'FRS2Model.error_status_t.ERROR_SUCCESS\', actual \'{0}\' (return of Reques" +
                        "tVersionVector, state S121)", TestManagerHelpers.Describe(temp69)));
            this.Manager.Comment("reaching state \'S129\'");
            int temp71 = this.Manager.ExpectEvent(this.QuiescenceTimeout, true, new ExpectedEvent(TCtestScenario5.AsyncPollResponseEventInfo, null, new AsyncPollResponseEventDelegate1(this.TCtestScenario5S12AsyncPollResponseEventChecker)), new ExpectedEvent(TCtestScenario5.AsyncPollResponseEventInfo, null, new AsyncPollResponseEventDelegate1(this.TCtestScenario5S12AsyncPollResponseEventChecker1)));
            if ((temp71 == 0)) {
                this.Manager.Comment("reaching state \'S140\'");
                FRS2Model.error_status_t temp70;
                this.Manager.Comment("executing step \'call RequestRecords(1,5)\'");
                temp70 = this.IFRS2ManagedAdapterInstance.RequestRecords(1, 5);
                this.Manager.Checkpoint("MS-FRS2_R585");
                this.Manager.Checkpoint("MS-FRS2_R589");
                this.Manager.Comment("reaching state \'S146\'");
                this.Manager.Comment("checking step \'return RequestRecords/FRS_ERROR_CONTENTSET_NOT_FOUND\'");
                this.Manager.Assert((temp70 == FRS2Model.error_status_t.FRS_ERROR_CONTENTSET_NOT_FOUND), String.Format("expected \'FRS2Model.error_status_t.FRS_ERROR_CONTENTSET_NOT_FOUND\', actual \'{0}\' " +
                            "(return of RequestRecords, state S146)", TestManagerHelpers.Describe(temp70)));
                TCtestScenario5S150();
                goto label13;
            }
            if ((temp71 == 1)) {
                TCtestScenario5S133();
                goto label13;
            }
            throw new InvalidOperationException("never reached");
        label13:
;
            this.Manager.EndTest();
        }
        
        private void TCtestScenario5S12AsyncPollResponseEventChecker(FRS2Model.VVGeneration vvGen) {
            this.Manager.Comment("checking step \'event AsyncPollResponseEvent(ValidValue)\'");
            try {
                this.Manager.Assert((vvGen == FRS2Model.VVGeneration.ValidValue), String.Format("expected \'FRS2Model.VVGeneration.ValidValue\', actual \'{0}\' (vvGen of AsyncPollRes" +
                            "ponseEvent, state S129)", TestManagerHelpers.Describe(vvGen)));
            }
            catch (TransactionFailedException ) {
                this.Manager.Comment("This step would have covered MS-FRS2_R556, MS-FRS2_R1020, MS-FRS2_R555");
                throw;
            }
            this.Manager.Checkpoint("MS-FRS2_R556");
            this.Manager.Checkpoint("MS-FRS2_R1020");
            this.Manager.Checkpoint("MS-FRS2_R555");
        }
        
        private void TCtestScenario5S12AsyncPollResponseEventChecker1(FRS2Model.VVGeneration vvGen) {
            this.Manager.Comment("checking step \'event AsyncPollResponseEvent(InvalidValue)\'");
            try {
                this.Manager.Assert((vvGen == FRS2Model.VVGeneration.InvalidValue), String.Format("expected \'FRS2Model.VVGeneration.InvalidValue\', actual \'{0}\' (vvGen of AsyncPollR" +
                            "esponseEvent, state S129)", TestManagerHelpers.Describe(vvGen)));
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
        [Microsoft.VisualStudio.TestTools.UnitTesting.DescriptionAttribute("MS-FRS2_R37, MS-FRS2_R436, MS-FRS2_R439, MS-FRS2_R455, MS-FRS2_R458, MS-FRS2_R448" +
            ", MS-FRS2_R549, MS-FRS2_R552, MS-FRS2_R518")]
        public virtual void FRS2_TCtestScenario5S14() {
            this.Manager.BeginTest("TCtestScenario5S14");
            this.Manager.Comment("reaching state \'S14\'");
            FRS2Model.error_status_t temp72;
            this.Manager.Comment(@"executing step 'call Initialization(Windows7,Map{1=>enabled,2=>enabled,3=>enabled,4=>disabled},Map{1=>exists,2=>exists,3=>exists,4=>exists},Map{""A""=>1,""B""=>2,""C""=>3,""D""=>4},Map{""A""=>sysvol,""B""=>protection,""C""=>sysvol,""D""=>sysvol},Map{1=>""A"",2=>""B"",3=>""C"",4=>""D""},Map{1=>writeOnly,2=>readWrite,3=>readOnly,4=>readWrite},Map{1=>enabled,2=>disabled,3=>enabled,4=>enabled})'");
            temp72 = this.IFRS2ManagedAdapterInstance.Initialization(FRS2Model.OSVersion.Windows7, new Microsoft.Modeling.Map<System.Int32,FRS2Model.connectionProperty>().Add(1, FRS2Model.connectionProperty.enabled).Add(2, FRS2Model.connectionProperty.enabled).Add(3, FRS2Model.connectionProperty.enabled).Add(4, FRS2Model.connectionProperty.disabled), new Microsoft.Modeling.Map<System.Int32,FRS2Model.FromServer>().Add(1, FRS2Model.FromServer.exists).Add(2, FRS2Model.FromServer.exists).Add(3, FRS2Model.FromServer.exists).Add(4, FRS2Model.FromServer.exists), new Microsoft.Modeling.Map<System.String,System.Int32>().Add("A", 1).Add("B", 2).Add("C", 3).Add("D", 4), new Microsoft.Modeling.Map<System.String,FRS2Model.ReplicationGroupTypes>().Add("A", FRS2Model.ReplicationGroupTypes.sysvol).Add("B", FRS2Model.ReplicationGroupTypes.protection).Add("C", FRS2Model.ReplicationGroupTypes.sysvol).Add("D", FRS2Model.ReplicationGroupTypes.sysvol), new Microsoft.Modeling.Map<System.Int32,System.String>().Add(1, "A").Add(2, "B").Add(3, "C").Add(4, "D"), new Microsoft.Modeling.Map<System.Int32,FRS2Model.accessLevels>().Add(1, FRS2Model.accessLevels.writeOnly).Add(2, FRS2Model.accessLevels.readWrite).Add(3, FRS2Model.accessLevels.readOnly).Add(4, FRS2Model.accessLevels.readWrite), new Microsoft.Modeling.Map<System.Int32,FRS2Model.connectionProperty>().Add(1, FRS2Model.connectionProperty.enabled).Add(2, FRS2Model.connectionProperty.disabled).Add(3, FRS2Model.connectionProperty.enabled).Add(4, FRS2Model.connectionProperty.enabled));
            this.Manager.Comment("reaching state \'S15\'");
            this.Manager.Comment("checking step \'return Initialization/ERROR_SUCCESS\'");
            this.Manager.Assert((temp72 == FRS2Model.error_status_t.ERROR_SUCCESS), String.Format("expected \'FRS2Model.error_status_t.ERROR_SUCCESS\', actual \'{0}\' (return of Initia" +
                        "lization, state S15)", TestManagerHelpers.Describe(temp72)));
            this.Manager.Comment("reaching state \'S35\'");
            FRS2Model.ProtocolVersionReturned temp73;
            FRS2Model.UpstreamFlagValueReturned temp74;
            FRS2Model.error_status_t temp75;
            this.Manager.Comment("executing step \'call EstablishConnection(\"A\",1,FRS_COMMUNICATION_PROTOCOL_VERSION" +
                    "_LONGHORN_SERVER,out _,out _)\'");
            temp75 = this.IFRS2ManagedAdapterInstance.EstablishConnection("A", 1, FRS2Model.ProtocolVersion.FRS_COMMUNICATION_PROTOCOL_VERSION_LONGHORN_SERVER, out temp73, out temp74);
            this.Manager.Checkpoint("MS-FRS2_R37");
            this.Manager.Checkpoint("MS-FRS2_R436");
            this.Manager.Checkpoint("MS-FRS2_R439");
            this.Manager.Comment("reaching state \'S49\'");
            this.Manager.Comment("checking step \'return EstablishConnection/[out Valid,out Valid]:ERROR_SUCCESS\'");
            this.Manager.Assert((temp73 == FRS2Model.ProtocolVersionReturned.Valid), String.Format("expected \'FRS2Model.ProtocolVersionReturned.Valid\', actual \'{0}\' (upstreamProtoco" +
                        "lVersion of EstablishConnection, state S49)", TestManagerHelpers.Describe(temp73)));
            this.Manager.Assert((temp74 == FRS2Model.UpstreamFlagValueReturned.Valid), String.Format("expected \'FRS2Model.UpstreamFlagValueReturned.Valid\', actual \'{0}\' (upstreamFlags" +
                        " of EstablishConnection, state S49)", TestManagerHelpers.Describe(temp74)));
            this.Manager.Assert((temp75 == FRS2Model.error_status_t.ERROR_SUCCESS), String.Format("expected \'FRS2Model.error_status_t.ERROR_SUCCESS\', actual \'{0}\' (return of Establ" +
                        "ishConnection, state S49)", TestManagerHelpers.Describe(temp75)));
            this.Manager.Comment("reaching state \'S63\'");
            FRS2Model.error_status_t temp76;
            this.Manager.Comment("executing step \'call EstablishSession(1,1)\'");
            temp76 = this.IFRS2ManagedAdapterInstance.EstablishSession(1, 1);
            this.Manager.Checkpoint("MS-FRS2_R455");
            this.Manager.Checkpoint("MS-FRS2_R458");
            this.Manager.Checkpoint("MS-FRS2_R448");
            this.Manager.Comment("reaching state \'S76\'");
            this.Manager.Comment("checking step \'return EstablishSession/ERROR_SUCCESS\'");
            this.Manager.Assert((temp76 == FRS2Model.error_status_t.ERROR_SUCCESS), String.Format("expected \'FRS2Model.error_status_t.ERROR_SUCCESS\', actual \'{0}\' (return of Establ" +
                        "ishSession, state S76)", TestManagerHelpers.Describe(temp76)));
            this.Manager.Comment("reaching state \'S88\'");
            FRS2Model.error_status_t temp77;
            this.Manager.Comment("executing step \'call AsyncPoll(1)\'");
            temp77 = this.IFRS2ManagedAdapterInstance.AsyncPoll(1);
            this.Manager.Checkpoint("MS-FRS2_R549");
            this.Manager.Checkpoint("MS-FRS2_R552");
            this.Manager.Comment("reaching state \'S100\'");
            this.Manager.Comment("checking step \'return AsyncPoll/ERROR_SUCCESS\'");
            this.Manager.Assert((temp77 == FRS2Model.error_status_t.ERROR_SUCCESS), String.Format("expected \'FRS2Model.error_status_t.ERROR_SUCCESS\', actual \'{0}\' (return of AsyncP" +
                        "oll, state S100)", TestManagerHelpers.Describe(temp77)));
            this.Manager.Comment("reaching state \'S111\'");
            FRS2Model.error_status_t temp78;
            this.Manager.Comment("executing step \'call RequestVersionVector(1,1,1,REQUEST_SLOW_SYNC,CHANGE_ALL,Inva" +
                    "lidValue)\'");
            temp78 = this.IFRS2ManagedAdapterInstance.RequestVersionVector(1, 1, 1, FRS2Model.VERSION_REQUEST_TYPE.REQUEST_SLOW_SYNC, FRS2Model.VERSION_CHANGE_TYPE.CHANGE_ALL, FRS2Model.VVGeneration.InvalidValue);
            this.Manager.Checkpoint("MS-FRS2_R518");
            this.Manager.Comment("reaching state \'S122\'");
            this.Manager.Comment("checking step \'return RequestVersionVector/ERROR_SUCCESS\'");
            this.Manager.Assert((temp78 == FRS2Model.error_status_t.ERROR_SUCCESS), String.Format("expected \'FRS2Model.error_status_t.ERROR_SUCCESS\', actual \'{0}\' (return of Reques" +
                        "tVersionVector, state S122)", TestManagerHelpers.Describe(temp78)));
            TCtestScenario5S128();
            this.Manager.EndTest();
        }
        #endregion
        
        #region Test Starting in S16
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        [Microsoft.VisualStudio.TestTools.UnitTesting.DescriptionAttribute("MS-FRS2_R37, MS-FRS2_R436, MS-FRS2_R439, MS-FRS2_R455, MS-FRS2_R458, MS-FRS2_R448" +
            ", MS-FRS2_R518")]
        public virtual void FRS2_TCtestScenario5S16() {
            this.Manager.BeginTest("TCtestScenario5S16");
            this.Manager.Comment("reaching state \'S16\'");
            FRS2Model.error_status_t temp79;
            this.Manager.Comment(@"executing step 'call Initialization(Windows7,Map{1=>enabled,2=>enabled,3=>enabled,4=>disabled},Map{1=>exists,2=>exists,3=>exists,4=>exists},Map{""A""=>1,""B""=>2,""C""=>3,""D""=>4},Map{""A""=>sysvol,""B""=>protection,""C""=>sysvol,""D""=>sysvol},Map{1=>""A"",2=>""B"",3=>""C"",4=>""D""},Map{1=>writeOnly,2=>readWrite,3=>readOnly,4=>readWrite},Map{1=>enabled,2=>disabled,3=>enabled,4=>enabled})'");
            temp79 = this.IFRS2ManagedAdapterInstance.Initialization(FRS2Model.OSVersion.Windows7, new Microsoft.Modeling.Map<System.Int32,FRS2Model.connectionProperty>().Add(1, FRS2Model.connectionProperty.enabled).Add(2, FRS2Model.connectionProperty.enabled).Add(3, FRS2Model.connectionProperty.enabled).Add(4, FRS2Model.connectionProperty.disabled), new Microsoft.Modeling.Map<System.Int32,FRS2Model.FromServer>().Add(1, FRS2Model.FromServer.exists).Add(2, FRS2Model.FromServer.exists).Add(3, FRS2Model.FromServer.exists).Add(4, FRS2Model.FromServer.exists), new Microsoft.Modeling.Map<System.String,System.Int32>().Add("A", 1).Add("B", 2).Add("C", 3).Add("D", 4), new Microsoft.Modeling.Map<System.String,FRS2Model.ReplicationGroupTypes>().Add("A", FRS2Model.ReplicationGroupTypes.sysvol).Add("B", FRS2Model.ReplicationGroupTypes.protection).Add("C", FRS2Model.ReplicationGroupTypes.sysvol).Add("D", FRS2Model.ReplicationGroupTypes.sysvol), new Microsoft.Modeling.Map<System.Int32,System.String>().Add(1, "A").Add(2, "B").Add(3, "C").Add(4, "D"), new Microsoft.Modeling.Map<System.Int32,FRS2Model.accessLevels>().Add(1, FRS2Model.accessLevels.writeOnly).Add(2, FRS2Model.accessLevels.readWrite).Add(3, FRS2Model.accessLevels.readOnly).Add(4, FRS2Model.accessLevels.readWrite), new Microsoft.Modeling.Map<System.Int32,FRS2Model.connectionProperty>().Add(1, FRS2Model.connectionProperty.enabled).Add(2, FRS2Model.connectionProperty.disabled).Add(3, FRS2Model.connectionProperty.enabled).Add(4, FRS2Model.connectionProperty.enabled));
            this.Manager.Comment("reaching state \'S17\'");
            this.Manager.Comment("checking step \'return Initialization/ERROR_SUCCESS\'");
            this.Manager.Assert((temp79 == FRS2Model.error_status_t.ERROR_SUCCESS), String.Format("expected \'FRS2Model.error_status_t.ERROR_SUCCESS\', actual \'{0}\' (return of Initia" +
                        "lization, state S17)", TestManagerHelpers.Describe(temp79)));
            this.Manager.Comment("reaching state \'S36\'");
            FRS2Model.ProtocolVersionReturned temp80;
            FRS2Model.UpstreamFlagValueReturned temp81;
            FRS2Model.error_status_t temp82;
            this.Manager.Comment("executing step \'call EstablishConnection(\"A\",1,FRS_COMMUNICATION_PROTOCOL_VERSION" +
                    "_LONGHORN_SERVER,out _,out _)\'");
            temp82 = this.IFRS2ManagedAdapterInstance.EstablishConnection("A", 1, FRS2Model.ProtocolVersion.FRS_COMMUNICATION_PROTOCOL_VERSION_LONGHORN_SERVER, out temp80, out temp81);
            this.Manager.Checkpoint("MS-FRS2_R37");
            this.Manager.Checkpoint("MS-FRS2_R436");
            this.Manager.Checkpoint("MS-FRS2_R439");
            this.Manager.Comment("reaching state \'S50\'");
            this.Manager.Comment("checking step \'return EstablishConnection/[out Valid,out Valid]:ERROR_SUCCESS\'");
            this.Manager.Assert((temp80 == FRS2Model.ProtocolVersionReturned.Valid), String.Format("expected \'FRS2Model.ProtocolVersionReturned.Valid\', actual \'{0}\' (upstreamProtoco" +
                        "lVersion of EstablishConnection, state S50)", TestManagerHelpers.Describe(temp80)));
            this.Manager.Assert((temp81 == FRS2Model.UpstreamFlagValueReturned.Valid), String.Format("expected \'FRS2Model.UpstreamFlagValueReturned.Valid\', actual \'{0}\' (upstreamFlags" +
                        " of EstablishConnection, state S50)", TestManagerHelpers.Describe(temp81)));
            this.Manager.Assert((temp82 == FRS2Model.error_status_t.ERROR_SUCCESS), String.Format("expected \'FRS2Model.error_status_t.ERROR_SUCCESS\', actual \'{0}\' (return of Establ" +
                        "ishConnection, state S50)", TestManagerHelpers.Describe(temp82)));
            this.Manager.Comment("reaching state \'S64\'");
            FRS2Model.error_status_t temp83;
            this.Manager.Comment("executing step \'call EstablishSession(1,1)\'");
            temp83 = this.IFRS2ManagedAdapterInstance.EstablishSession(1, 1);
            this.Manager.Checkpoint("MS-FRS2_R455");
            this.Manager.Checkpoint("MS-FRS2_R458");
            this.Manager.Checkpoint("MS-FRS2_R448");
            this.Manager.Comment("reaching state \'S77\'");
            this.Manager.Comment("checking step \'return EstablishSession/ERROR_SUCCESS\'");
            this.Manager.Assert((temp83 == FRS2Model.error_status_t.ERROR_SUCCESS), String.Format("expected \'FRS2Model.error_status_t.ERROR_SUCCESS\', actual \'{0}\' (return of Establ" +
                        "ishSession, state S77)", TestManagerHelpers.Describe(temp83)));
            this.Manager.Comment("reaching state \'S89\'");
            FRS2Model.error_status_t temp84;
            this.Manager.Comment("executing step \'call RequestVersionVector(1,1,1,REQUEST_SLOW_SYNC,CHANGE_NOTIFY,I" +
                    "nvalidValue)\'");
            temp84 = this.IFRS2ManagedAdapterInstance.RequestVersionVector(1, 1, 1, FRS2Model.VERSION_REQUEST_TYPE.REQUEST_SLOW_SYNC, FRS2Model.VERSION_CHANGE_TYPE.CHANGE_NOTIFY, FRS2Model.VVGeneration.InvalidValue);
            this.Manager.Checkpoint("MS-FRS2_R518");
            this.Manager.Comment("reaching state \'S101\'");
            this.Manager.Comment("checking step \'return RequestVersionVector/ERROR_SUCCESS\'");
            this.Manager.Assert((temp84 == FRS2Model.error_status_t.ERROR_SUCCESS), String.Format("expected \'FRS2Model.error_status_t.ERROR_SUCCESS\', actual \'{0}\' (return of Reques" +
                        "tVersionVector, state S101)", TestManagerHelpers.Describe(temp84)));
            TCtestScenario5S112();
            this.Manager.EndTest();
        }
        
        private void TCtestScenario5S112() {
            this.Manager.Comment("reaching state \'S112\'");
        }
        #endregion
        
        #region Test Starting in S18
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        [Microsoft.VisualStudio.TestTools.UnitTesting.DescriptionAttribute("MS-FRS2_R37, MS-FRS2_R436, MS-FRS2_R439, MS-FRS2_R455, MS-FRS2_R458, MS-FRS2_R448" +
            ", MS-FRS2_R549, MS-FRS2_R552, MS-FRS2_R518")]
        public virtual void FRS2_TCtestScenario5S18() {
            this.Manager.BeginTest("TCtestScenario5S18");
            this.Manager.Comment("reaching state \'S18\'");
            FRS2Model.error_status_t temp85;
            this.Manager.Comment(@"executing step 'call Initialization(Windows7,Map{1=>enabled,2=>enabled,3=>enabled,4=>disabled},Map{1=>exists,2=>exists,3=>exists,4=>exists},Map{""A""=>1,""B""=>2,""C""=>3,""D""=>4},Map{""A""=>sysvol,""B""=>protection,""C""=>sysvol,""D""=>sysvol},Map{1=>""A"",2=>""B"",3=>""C"",4=>""D""},Map{1=>writeOnly,2=>readWrite,3=>readOnly,4=>readWrite},Map{1=>enabled,2=>disabled,3=>enabled,4=>enabled})'");
            temp85 = this.IFRS2ManagedAdapterInstance.Initialization(FRS2Model.OSVersion.Windows7, new Microsoft.Modeling.Map<System.Int32,FRS2Model.connectionProperty>().Add(1, FRS2Model.connectionProperty.enabled).Add(2, FRS2Model.connectionProperty.enabled).Add(3, FRS2Model.connectionProperty.enabled).Add(4, FRS2Model.connectionProperty.disabled), new Microsoft.Modeling.Map<System.Int32,FRS2Model.FromServer>().Add(1, FRS2Model.FromServer.exists).Add(2, FRS2Model.FromServer.exists).Add(3, FRS2Model.FromServer.exists).Add(4, FRS2Model.FromServer.exists), new Microsoft.Modeling.Map<System.String,System.Int32>().Add("A", 1).Add("B", 2).Add("C", 3).Add("D", 4), new Microsoft.Modeling.Map<System.String,FRS2Model.ReplicationGroupTypes>().Add("A", FRS2Model.ReplicationGroupTypes.sysvol).Add("B", FRS2Model.ReplicationGroupTypes.protection).Add("C", FRS2Model.ReplicationGroupTypes.sysvol).Add("D", FRS2Model.ReplicationGroupTypes.sysvol), new Microsoft.Modeling.Map<System.Int32,System.String>().Add(1, "A").Add(2, "B").Add(3, "C").Add(4, "D"), new Microsoft.Modeling.Map<System.Int32,FRS2Model.accessLevels>().Add(1, FRS2Model.accessLevels.writeOnly).Add(2, FRS2Model.accessLevels.readWrite).Add(3, FRS2Model.accessLevels.readOnly).Add(4, FRS2Model.accessLevels.readWrite), new Microsoft.Modeling.Map<System.Int32,FRS2Model.connectionProperty>().Add(1, FRS2Model.connectionProperty.enabled).Add(2, FRS2Model.connectionProperty.disabled).Add(3, FRS2Model.connectionProperty.enabled).Add(4, FRS2Model.connectionProperty.enabled));
            this.Manager.Comment("reaching state \'S19\'");
            this.Manager.Comment("checking step \'return Initialization/ERROR_SUCCESS\'");
            this.Manager.Assert((temp85 == FRS2Model.error_status_t.ERROR_SUCCESS), String.Format("expected \'FRS2Model.error_status_t.ERROR_SUCCESS\', actual \'{0}\' (return of Initia" +
                        "lization, state S19)", TestManagerHelpers.Describe(temp85)));
            this.Manager.Comment("reaching state \'S37\'");
            FRS2Model.ProtocolVersionReturned temp86;
            FRS2Model.UpstreamFlagValueReturned temp87;
            FRS2Model.error_status_t temp88;
            this.Manager.Comment("executing step \'call EstablishConnection(\"A\",1,FRS_COMMUNICATION_PROTOCOL_VERSION" +
                    "_LONGHORN_SERVER,out _,out _)\'");
            temp88 = this.IFRS2ManagedAdapterInstance.EstablishConnection("A", 1, FRS2Model.ProtocolVersion.FRS_COMMUNICATION_PROTOCOL_VERSION_LONGHORN_SERVER, out temp86, out temp87);
            this.Manager.Checkpoint("MS-FRS2_R37");
            this.Manager.Checkpoint("MS-FRS2_R436");
            this.Manager.Checkpoint("MS-FRS2_R439");
            this.Manager.Comment("reaching state \'S51\'");
            this.Manager.Comment("checking step \'return EstablishConnection/[out Valid,out Valid]:ERROR_SUCCESS\'");
            this.Manager.Assert((temp86 == FRS2Model.ProtocolVersionReturned.Valid), String.Format("expected \'FRS2Model.ProtocolVersionReturned.Valid\', actual \'{0}\' (upstreamProtoco" +
                        "lVersion of EstablishConnection, state S51)", TestManagerHelpers.Describe(temp86)));
            this.Manager.Assert((temp87 == FRS2Model.UpstreamFlagValueReturned.Valid), String.Format("expected \'FRS2Model.UpstreamFlagValueReturned.Valid\', actual \'{0}\' (upstreamFlags" +
                        " of EstablishConnection, state S51)", TestManagerHelpers.Describe(temp87)));
            this.Manager.Assert((temp88 == FRS2Model.error_status_t.ERROR_SUCCESS), String.Format("expected \'FRS2Model.error_status_t.ERROR_SUCCESS\', actual \'{0}\' (return of Establ" +
                        "ishConnection, state S51)", TestManagerHelpers.Describe(temp88)));
            this.Manager.Comment("reaching state \'S65\'");
            FRS2Model.error_status_t temp89;
            this.Manager.Comment("executing step \'call EstablishSession(1,1)\'");
            temp89 = this.IFRS2ManagedAdapterInstance.EstablishSession(1, 1);
            this.Manager.Checkpoint("MS-FRS2_R455");
            this.Manager.Checkpoint("MS-FRS2_R458");
            this.Manager.Checkpoint("MS-FRS2_R448");
            this.Manager.Comment("reaching state \'S78\'");
            this.Manager.Comment("checking step \'return EstablishSession/ERROR_SUCCESS\'");
            this.Manager.Assert((temp89 == FRS2Model.error_status_t.ERROR_SUCCESS), String.Format("expected \'FRS2Model.error_status_t.ERROR_SUCCESS\', actual \'{0}\' (return of Establ" +
                        "ishSession, state S78)", TestManagerHelpers.Describe(temp89)));
            this.Manager.Comment("reaching state \'S90\'");
            FRS2Model.error_status_t temp90;
            this.Manager.Comment("executing step \'call AsyncPoll(1)\'");
            temp90 = this.IFRS2ManagedAdapterInstance.AsyncPoll(1);
            this.Manager.Checkpoint("MS-FRS2_R549");
            this.Manager.Checkpoint("MS-FRS2_R552");
            this.Manager.Comment("reaching state \'S102\'");
            this.Manager.Comment("checking step \'return AsyncPoll/ERROR_SUCCESS\'");
            this.Manager.Assert((temp90 == FRS2Model.error_status_t.ERROR_SUCCESS), String.Format("expected \'FRS2Model.error_status_t.ERROR_SUCCESS\', actual \'{0}\' (return of AsyncP" +
                        "oll, state S102)", TestManagerHelpers.Describe(temp90)));
            this.Manager.Comment("reaching state \'S113\'");
            FRS2Model.error_status_t temp91;
            this.Manager.Comment("executing step \'call RequestVersionVector(1,1,1,REQUEST_SLOW_SYNC,CHANGE_NOTIFY,V" +
                    "alidValue)\'");
            temp91 = this.IFRS2ManagedAdapterInstance.RequestVersionVector(1, 1, 1, FRS2Model.VERSION_REQUEST_TYPE.REQUEST_SLOW_SYNC, FRS2Model.VERSION_CHANGE_TYPE.CHANGE_NOTIFY, FRS2Model.VVGeneration.ValidValue);
            this.Manager.Checkpoint("MS-FRS2_R518");
            this.Manager.Comment("reaching state \'S123\'");
            this.Manager.Comment("checking step \'return RequestVersionVector/ERROR_SUCCESS\'");
            this.Manager.Assert((temp91 == FRS2Model.error_status_t.ERROR_SUCCESS), String.Format("expected \'FRS2Model.error_status_t.ERROR_SUCCESS\', actual \'{0}\' (return of Reques" +
                        "tVersionVector, state S123)", TestManagerHelpers.Describe(temp91)));
            TCtestScenario5S128();
            this.Manager.EndTest();
        }
        #endregion
        
        #region Test Starting in S20
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        [Microsoft.VisualStudio.TestTools.UnitTesting.DescriptionAttribute("MS-FRS2_R37, MS-FRS2_R436, MS-FRS2_R439, MS-FRS2_R455, MS-FRS2_R458, MS-FRS2_R448" +
            ", MS-FRS2_R518")]
        public virtual void FRS2_TCtestScenario5S20() {
            this.Manager.BeginTest("TCtestScenario5S20");
            this.Manager.Comment("reaching state \'S20\'");
            FRS2Model.error_status_t temp92;
            this.Manager.Comment(@"executing step 'call Initialization(Windows7,Map{1=>enabled,2=>enabled,3=>enabled,4=>disabled},Map{1=>exists,2=>exists,3=>exists,4=>exists},Map{""A""=>1,""B""=>2,""C""=>3,""D""=>4},Map{""A""=>sysvol,""B""=>protection,""C""=>sysvol,""D""=>sysvol},Map{1=>""A"",2=>""B"",3=>""C"",4=>""D""},Map{1=>writeOnly,2=>readWrite,3=>readOnly,4=>readWrite},Map{1=>enabled,2=>disabled,3=>enabled,4=>enabled})'");
            temp92 = this.IFRS2ManagedAdapterInstance.Initialization(FRS2Model.OSVersion.Windows7, new Microsoft.Modeling.Map<System.Int32,FRS2Model.connectionProperty>().Add(1, FRS2Model.connectionProperty.enabled).Add(2, FRS2Model.connectionProperty.enabled).Add(3, FRS2Model.connectionProperty.enabled).Add(4, FRS2Model.connectionProperty.disabled), new Microsoft.Modeling.Map<System.Int32,FRS2Model.FromServer>().Add(1, FRS2Model.FromServer.exists).Add(2, FRS2Model.FromServer.exists).Add(3, FRS2Model.FromServer.exists).Add(4, FRS2Model.FromServer.exists), new Microsoft.Modeling.Map<System.String,System.Int32>().Add("A", 1).Add("B", 2).Add("C", 3).Add("D", 4), new Microsoft.Modeling.Map<System.String,FRS2Model.ReplicationGroupTypes>().Add("A", FRS2Model.ReplicationGroupTypes.sysvol).Add("B", FRS2Model.ReplicationGroupTypes.protection).Add("C", FRS2Model.ReplicationGroupTypes.sysvol).Add("D", FRS2Model.ReplicationGroupTypes.sysvol), new Microsoft.Modeling.Map<System.Int32,System.String>().Add(1, "A").Add(2, "B").Add(3, "C").Add(4, "D"), new Microsoft.Modeling.Map<System.Int32,FRS2Model.accessLevels>().Add(1, FRS2Model.accessLevels.writeOnly).Add(2, FRS2Model.accessLevels.readWrite).Add(3, FRS2Model.accessLevels.readOnly).Add(4, FRS2Model.accessLevels.readWrite), new Microsoft.Modeling.Map<System.Int32,FRS2Model.connectionProperty>().Add(1, FRS2Model.connectionProperty.enabled).Add(2, FRS2Model.connectionProperty.disabled).Add(3, FRS2Model.connectionProperty.enabled).Add(4, FRS2Model.connectionProperty.enabled));
            this.Manager.Comment("reaching state \'S21\'");
            this.Manager.Comment("checking step \'return Initialization/ERROR_SUCCESS\'");
            this.Manager.Assert((temp92 == FRS2Model.error_status_t.ERROR_SUCCESS), String.Format("expected \'FRS2Model.error_status_t.ERROR_SUCCESS\', actual \'{0}\' (return of Initia" +
                        "lization, state S21)", TestManagerHelpers.Describe(temp92)));
            this.Manager.Comment("reaching state \'S38\'");
            FRS2Model.ProtocolVersionReturned temp93;
            FRS2Model.UpstreamFlagValueReturned temp94;
            FRS2Model.error_status_t temp95;
            this.Manager.Comment("executing step \'call EstablishConnection(\"A\",1,FRS_COMMUNICATION_PROTOCOL_VERSION" +
                    "_LONGHORN_SERVER,out _,out _)\'");
            temp95 = this.IFRS2ManagedAdapterInstance.EstablishConnection("A", 1, FRS2Model.ProtocolVersion.FRS_COMMUNICATION_PROTOCOL_VERSION_LONGHORN_SERVER, out temp93, out temp94);
            this.Manager.Checkpoint("MS-FRS2_R37");
            this.Manager.Checkpoint("MS-FRS2_R436");
            this.Manager.Checkpoint("MS-FRS2_R439");
            this.Manager.Comment("reaching state \'S52\'");
            this.Manager.Comment("checking step \'return EstablishConnection/[out Valid,out Valid]:ERROR_SUCCESS\'");
            this.Manager.Assert((temp93 == FRS2Model.ProtocolVersionReturned.Valid), String.Format("expected \'FRS2Model.ProtocolVersionReturned.Valid\', actual \'{0}\' (upstreamProtoco" +
                        "lVersion of EstablishConnection, state S52)", TestManagerHelpers.Describe(temp93)));
            this.Manager.Assert((temp94 == FRS2Model.UpstreamFlagValueReturned.Valid), String.Format("expected \'FRS2Model.UpstreamFlagValueReturned.Valid\', actual \'{0}\' (upstreamFlags" +
                        " of EstablishConnection, state S52)", TestManagerHelpers.Describe(temp94)));
            this.Manager.Assert((temp95 == FRS2Model.error_status_t.ERROR_SUCCESS), String.Format("expected \'FRS2Model.error_status_t.ERROR_SUCCESS\', actual \'{0}\' (return of Establ" +
                        "ishConnection, state S52)", TestManagerHelpers.Describe(temp95)));
            this.Manager.Comment("reaching state \'S66\'");
            FRS2Model.error_status_t temp96;
            this.Manager.Comment("executing step \'call EstablishSession(1,1)\'");
            temp96 = this.IFRS2ManagedAdapterInstance.EstablishSession(1, 1);
            this.Manager.Checkpoint("MS-FRS2_R455");
            this.Manager.Checkpoint("MS-FRS2_R458");
            this.Manager.Checkpoint("MS-FRS2_R448");
            this.Manager.Comment("reaching state \'S79\'");
            this.Manager.Comment("checking step \'return EstablishSession/ERROR_SUCCESS\'");
            this.Manager.Assert((temp96 == FRS2Model.error_status_t.ERROR_SUCCESS), String.Format("expected \'FRS2Model.error_status_t.ERROR_SUCCESS\', actual \'{0}\' (return of Establ" +
                        "ishSession, state S79)", TestManagerHelpers.Describe(temp96)));
            this.Manager.Comment("reaching state \'S91\'");
            FRS2Model.error_status_t temp97;
            this.Manager.Comment("executing step \'call RequestVersionVector(1,1,1,REQUEST_SLOW_SYNC,CHANGE_ALL,Inva" +
                    "lidValue)\'");
            temp97 = this.IFRS2ManagedAdapterInstance.RequestVersionVector(1, 1, 1, FRS2Model.VERSION_REQUEST_TYPE.REQUEST_SLOW_SYNC, FRS2Model.VERSION_CHANGE_TYPE.CHANGE_ALL, FRS2Model.VVGeneration.InvalidValue);
            this.Manager.Checkpoint("MS-FRS2_R518");
            this.Manager.Comment("reaching state \'S103\'");
            this.Manager.Comment("checking step \'return RequestVersionVector/ERROR_SUCCESS\'");
            this.Manager.Assert((temp97 == FRS2Model.error_status_t.ERROR_SUCCESS), String.Format("expected \'FRS2Model.error_status_t.ERROR_SUCCESS\', actual \'{0}\' (return of Reques" +
                        "tVersionVector, state S103)", TestManagerHelpers.Describe(temp97)));
            TCtestScenario5S112();
            this.Manager.EndTest();
        }
        #endregion
        
        #region Test Starting in S22
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        [Microsoft.VisualStudio.TestTools.UnitTesting.DescriptionAttribute("MS-FRS2_R37, MS-FRS2_R436, MS-FRS2_R439, MS-FRS2_R549, MS-FRS2_R552")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestCategory("AD")]
        [TestCategory("MS-FRS2")]
        [TestCategory("SDC")]
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        
        public virtual void FRS2_TCtestScenario5S22()
        {
            this.Manager.BeginTest("TCtestScenario5S22");
            this.Manager.Comment("reaching state \'S22\'");
            FRS2Model.error_status_t temp98;
            this.Manager.Comment(@"executing step 'call Initialization(Windows7,Map{1=>enabled,2=>enabled,3=>enabled,4=>disabled},Map{1=>exists,2=>exists,3=>exists,4=>exists},Map{""A""=>1,""B""=>2,""C""=>3,""D""=>4},Map{""A""=>sysvol,""B""=>protection,""C""=>sysvol,""D""=>sysvol},Map{1=>""A"",2=>""B"",3=>""C"",4=>""D""},Map{1=>writeOnly,2=>readWrite,3=>readOnly,4=>readWrite},Map{1=>enabled,2=>disabled,3=>enabled,4=>enabled})'");
            temp98 = this.IFRS2ManagedAdapterInstance.Initialization(FRS2Model.OSVersion.Windows7, new Microsoft.Modeling.Map<System.Int32,FRS2Model.connectionProperty>().Add(1, FRS2Model.connectionProperty.enabled).Add(2, FRS2Model.connectionProperty.enabled).Add(3, FRS2Model.connectionProperty.enabled).Add(4, FRS2Model.connectionProperty.disabled), new Microsoft.Modeling.Map<System.Int32,FRS2Model.FromServer>().Add(1, FRS2Model.FromServer.exists).Add(2, FRS2Model.FromServer.exists).Add(3, FRS2Model.FromServer.exists).Add(4, FRS2Model.FromServer.exists), new Microsoft.Modeling.Map<System.String,System.Int32>().Add("A", 1).Add("B", 2).Add("C", 3).Add("D", 4), new Microsoft.Modeling.Map<System.String,FRS2Model.ReplicationGroupTypes>().Add("A", FRS2Model.ReplicationGroupTypes.sysvol).Add("B", FRS2Model.ReplicationGroupTypes.protection).Add("C", FRS2Model.ReplicationGroupTypes.sysvol).Add("D", FRS2Model.ReplicationGroupTypes.sysvol), new Microsoft.Modeling.Map<System.Int32,System.String>().Add(1, "A").Add(2, "B").Add(3, "C").Add(4, "D"), new Microsoft.Modeling.Map<System.Int32,FRS2Model.accessLevels>().Add(1, FRS2Model.accessLevels.writeOnly).Add(2, FRS2Model.accessLevels.readWrite).Add(3, FRS2Model.accessLevels.readOnly).Add(4, FRS2Model.accessLevels.readWrite), new Microsoft.Modeling.Map<System.Int32,FRS2Model.connectionProperty>().Add(1, FRS2Model.connectionProperty.enabled).Add(2, FRS2Model.connectionProperty.disabled).Add(3, FRS2Model.connectionProperty.enabled).Add(4, FRS2Model.connectionProperty.enabled));
            this.Manager.Comment("reaching state \'S23\'");
            this.Manager.Comment("checking step \'return Initialization/ERROR_SUCCESS\'");
            this.Manager.Assert((temp98 == FRS2Model.error_status_t.ERROR_SUCCESS), String.Format("expected \'FRS2Model.error_status_t.ERROR_SUCCESS\', actual \'{0}\' (return of Initia" +
                        "lization, state S23)", TestManagerHelpers.Describe(temp98)));
            this.Manager.Comment("reaching state \'S39\'");
            FRS2Model.ProtocolVersionReturned temp99;
            FRS2Model.UpstreamFlagValueReturned temp100;
            FRS2Model.error_status_t temp101;
            this.Manager.Comment("executing step \'call EstablishConnection(\"A\",1,FRS_COMMUNICATION_PROTOCOL_VERSION" +
                    "_LONGHORN_SERVER,out _,out _)\'");
            temp101 = this.IFRS2ManagedAdapterInstance.EstablishConnection("A", 1, FRS2Model.ProtocolVersion.FRS_COMMUNICATION_PROTOCOL_VERSION_LONGHORN_SERVER, out temp99, out temp100);
            this.Manager.Checkpoint("MS-FRS2_R37");
            this.Manager.Checkpoint("MS-FRS2_R436");
            this.Manager.Checkpoint("MS-FRS2_R439");
            this.Manager.Comment("reaching state \'S53\'");
            this.Manager.Comment("checking step \'return EstablishConnection/[out Valid,out Valid]:ERROR_SUCCESS\'");
            this.Manager.Assert((temp99 == FRS2Model.ProtocolVersionReturned.Valid), String.Format("expected \'FRS2Model.ProtocolVersionReturned.Valid\', actual \'{0}\' (upstreamProtoco" +
                        "lVersion of EstablishConnection, state S53)", TestManagerHelpers.Describe(temp99)));
            this.Manager.Assert((temp100 == FRS2Model.UpstreamFlagValueReturned.Valid), String.Format("expected \'FRS2Model.UpstreamFlagValueReturned.Valid\', actual \'{0}\' (upstreamFlags" +
                        " of EstablishConnection, state S53)", TestManagerHelpers.Describe(temp100)));
            this.Manager.Assert((temp101 == FRS2Model.error_status_t.ERROR_SUCCESS), String.Format("expected \'FRS2Model.error_status_t.ERROR_SUCCESS\', actual \'{0}\' (return of Establ" +
                        "ishConnection, state S53)", TestManagerHelpers.Describe(temp101)));
            this.Manager.Comment("reaching state \'S67\'");
            FRS2Model.error_status_t temp102;
            this.Manager.Comment("executing step \'call AsyncPoll(1)\'");
            temp102 = this.IFRS2ManagedAdapterInstance.AsyncPoll(1);
            this.Manager.Checkpoint("MS-FRS2_R549");
            this.Manager.Checkpoint("MS-FRS2_R552");
            this.Manager.Comment("reaching state \'S80\'");
            this.Manager.Comment("checking step \'return AsyncPoll/ERROR_SUCCESS\'");
            this.Manager.Assert((temp102 == FRS2Model.error_status_t.ERROR_SUCCESS), String.Format("expected \'FRS2Model.error_status_t.ERROR_SUCCESS\', actual \'{0}\' (return of AsyncP" +
                        "oll, state S80)", TestManagerHelpers.Describe(temp102)));
            this.Manager.Comment("reaching state \'S92\'");
            this.Manager.EndTest();
        }
        #endregion
        
        #region Test Starting in S24
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        [Microsoft.VisualStudio.TestTools.UnitTesting.DescriptionAttribute("MS-FRS2_R440, MS-FRS2_R447")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestCategory("AD")]
        [TestCategory("MS-FRS2")]
        [TestCategory("SDC")]
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        public virtual void FRS2_TCtestScenario5S24()
        {
            this.Manager.BeginTest("TCtestScenario5S24");
            this.Manager.Comment("reaching state \'S24\'");
            FRS2Model.error_status_t temp103;
            this.Manager.Comment(@"executing step 'call Initialization(Windows7,Map{1=>enabled,2=>enabled,3=>enabled,4=>disabled},Map{1=>exists,2=>exists,3=>exists,4=>exists},Map{""A""=>1,""B""=>2,""C""=>3,""D""=>4},Map{""A""=>sysvol,""B""=>protection,""C""=>sysvol,""D""=>sysvol},Map{1=>""A"",2=>""B"",3=>""C"",4=>""D""},Map{1=>writeOnly,2=>readWrite,3=>readOnly,4=>readWrite},Map{1=>enabled,2=>disabled,3=>enabled,4=>enabled})'");
            temp103 = this.IFRS2ManagedAdapterInstance.Initialization(FRS2Model.OSVersion.Windows7, new Microsoft.Modeling.Map<System.Int32,FRS2Model.connectionProperty>().Add(1, FRS2Model.connectionProperty.enabled).Add(2, FRS2Model.connectionProperty.enabled).Add(3, FRS2Model.connectionProperty.enabled).Add(4, FRS2Model.connectionProperty.disabled), new Microsoft.Modeling.Map<System.Int32,FRS2Model.FromServer>().Add(1, FRS2Model.FromServer.exists).Add(2, FRS2Model.FromServer.exists).Add(3, FRS2Model.FromServer.exists).Add(4, FRS2Model.FromServer.exists), new Microsoft.Modeling.Map<System.String,System.Int32>().Add("A", 1).Add("B", 2).Add("C", 3).Add("D", 4), new Microsoft.Modeling.Map<System.String,FRS2Model.ReplicationGroupTypes>().Add("A", FRS2Model.ReplicationGroupTypes.sysvol).Add("B", FRS2Model.ReplicationGroupTypes.protection).Add("C", FRS2Model.ReplicationGroupTypes.sysvol).Add("D", FRS2Model.ReplicationGroupTypes.sysvol), new Microsoft.Modeling.Map<System.Int32,System.String>().Add(1, "A").Add(2, "B").Add(3, "C").Add(4, "D"), new Microsoft.Modeling.Map<System.Int32,FRS2Model.accessLevels>().Add(1, FRS2Model.accessLevels.writeOnly).Add(2, FRS2Model.accessLevels.readWrite).Add(3, FRS2Model.accessLevels.readOnly).Add(4, FRS2Model.accessLevels.readWrite), new Microsoft.Modeling.Map<System.Int32,FRS2Model.connectionProperty>().Add(1, FRS2Model.connectionProperty.enabled).Add(2, FRS2Model.connectionProperty.disabled).Add(3, FRS2Model.connectionProperty.enabled).Add(4, FRS2Model.connectionProperty.enabled));
            this.Manager.Comment("reaching state \'S25\'");
            this.Manager.Comment("checking step \'return Initialization/ERROR_SUCCESS\'");
            this.Manager.Assert((temp103 == FRS2Model.error_status_t.ERROR_SUCCESS), String.Format("expected \'FRS2Model.error_status_t.ERROR_SUCCESS\', actual \'{0}\' (return of Initia" +
                        "lization, state S25)", TestManagerHelpers.Describe(temp103)));
            this.Manager.Comment("reaching state \'S40\'");
            FRS2Model.ProtocolVersionReturned temp104;
            FRS2Model.UpstreamFlagValueReturned temp105;
            FRS2Model.error_status_t temp106;
            this.Manager.Comment("executing step \'call EstablishConnection(\"A\",1,FRS_COMMUNICATION_PROTOCOL_VERSION" +
                    "_VISTA,out _,out _)\'");
            temp106 = this.IFRS2ManagedAdapterInstance.EstablishConnection("A", 1, FRS2Model.ProtocolVersion.FRS_COMMUNICATION_PROTOCOL_VERSION_VISTA, out temp104, out temp105);
            this.Manager.Checkpoint("MS-FRS2_R440");
            this.Manager.Checkpoint("MS-FRS2_R447");
            this.Manager.Comment("reaching state \'S54\'");
            this.Manager.Comment("checking step \'return EstablishConnection/[out Invalid,out Invalid]:FRS_ERROR_INC" +
                    "OMPATIBLE_VERSION\'");
            this.Manager.Assert((temp104 == FRS2Model.ProtocolVersionReturned.Invalid), String.Format("expected \'FRS2Model.ProtocolVersionReturned.Invalid\', actual \'{0}\' (upstreamProto" +
                        "colVersion of EstablishConnection, state S54)", TestManagerHelpers.Describe(temp104)));
            this.Manager.Assert((temp105 == FRS2Model.UpstreamFlagValueReturned.Invalid), String.Format("expected \'FRS2Model.UpstreamFlagValueReturned.Invalid\', actual \'{0}\' (upstreamFla" +
                        "gs of EstablishConnection, state S54)", TestManagerHelpers.Describe(temp105)));
            this.Manager.Assert((temp106 == FRS2Model.error_status_t.FRS_ERROR_INCOMPATIBLE_VERSION), String.Format("expected \'FRS2Model.error_status_t.FRS_ERROR_INCOMPATIBLE_VERSION\', actual \'{0}\' " +
                        "(return of EstablishConnection, state S54)", TestManagerHelpers.Describe(temp106)));
            TCtestScenario5S56();
            this.Manager.EndTest();
        }
        #endregion
        
        #region Test Starting in S26
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        [Microsoft.VisualStudio.TestTools.UnitTesting.DescriptionAttribute("MS-FRS2_R37, MS-FRS2_R436, MS-FRS2_R439, MS-FRS2_R455, MS-FRS2_R458, MS-FRS2_R448" +
            ", MS-FRS2_R518")]
        public virtual void FRS2_TCtestScenario5S26() {
            this.Manager.BeginTest("TCtestScenario5S26");
            this.Manager.Comment("reaching state \'S26\'");
            FRS2Model.error_status_t temp107;
            this.Manager.Comment(@"executing step 'call Initialization(Windows7,Map{1=>enabled,2=>enabled,3=>enabled,4=>disabled},Map{1=>exists,2=>exists,3=>exists,4=>exists},Map{""A""=>1,""B""=>2,""C""=>3,""D""=>4},Map{""A""=>sysvol,""B""=>protection,""C""=>sysvol,""D""=>sysvol},Map{1=>""A"",2=>""B"",3=>""C"",4=>""D""},Map{1=>writeOnly,2=>readWrite,3=>readOnly,4=>readWrite},Map{1=>enabled,2=>disabled,3=>enabled,4=>enabled})'");
            temp107 = this.IFRS2ManagedAdapterInstance.Initialization(FRS2Model.OSVersion.Windows7, new Microsoft.Modeling.Map<System.Int32,FRS2Model.connectionProperty>().Add(1, FRS2Model.connectionProperty.enabled).Add(2, FRS2Model.connectionProperty.enabled).Add(3, FRS2Model.connectionProperty.enabled).Add(4, FRS2Model.connectionProperty.disabled), new Microsoft.Modeling.Map<System.Int32,FRS2Model.FromServer>().Add(1, FRS2Model.FromServer.exists).Add(2, FRS2Model.FromServer.exists).Add(3, FRS2Model.FromServer.exists).Add(4, FRS2Model.FromServer.exists), new Microsoft.Modeling.Map<System.String,System.Int32>().Add("A", 1).Add("B", 2).Add("C", 3).Add("D", 4), new Microsoft.Modeling.Map<System.String,FRS2Model.ReplicationGroupTypes>().Add("A", FRS2Model.ReplicationGroupTypes.sysvol).Add("B", FRS2Model.ReplicationGroupTypes.protection).Add("C", FRS2Model.ReplicationGroupTypes.sysvol).Add("D", FRS2Model.ReplicationGroupTypes.sysvol), new Microsoft.Modeling.Map<System.Int32,System.String>().Add(1, "A").Add(2, "B").Add(3, "C").Add(4, "D"), new Microsoft.Modeling.Map<System.Int32,FRS2Model.accessLevels>().Add(1, FRS2Model.accessLevels.writeOnly).Add(2, FRS2Model.accessLevels.readWrite).Add(3, FRS2Model.accessLevels.readOnly).Add(4, FRS2Model.accessLevels.readWrite), new Microsoft.Modeling.Map<System.Int32,FRS2Model.connectionProperty>().Add(1, FRS2Model.connectionProperty.enabled).Add(2, FRS2Model.connectionProperty.disabled).Add(3, FRS2Model.connectionProperty.enabled).Add(4, FRS2Model.connectionProperty.enabled));
            this.Manager.Comment("reaching state \'S27\'");
            this.Manager.Comment("checking step \'return Initialization/ERROR_SUCCESS\'");
            this.Manager.Assert((temp107 == FRS2Model.error_status_t.ERROR_SUCCESS), String.Format("expected \'FRS2Model.error_status_t.ERROR_SUCCESS\', actual \'{0}\' (return of Initia" +
                        "lization, state S27)", TestManagerHelpers.Describe(temp107)));
            this.Manager.Comment("reaching state \'S41\'");
            FRS2Model.ProtocolVersionReturned temp108;
            FRS2Model.UpstreamFlagValueReturned temp109;
            FRS2Model.error_status_t temp110;
            this.Manager.Comment("executing step \'call EstablishConnection(\"A\",1,FRS_COMMUNICATION_PROTOCOL_VERSION" +
                    "_LONGHORN_SERVER,out _,out _)\'");
            temp110 = this.IFRS2ManagedAdapterInstance.EstablishConnection("A", 1, FRS2Model.ProtocolVersion.FRS_COMMUNICATION_PROTOCOL_VERSION_LONGHORN_SERVER, out temp108, out temp109);
            this.Manager.Checkpoint("MS-FRS2_R37");
            this.Manager.Checkpoint("MS-FRS2_R436");
            this.Manager.Checkpoint("MS-FRS2_R439");
            this.Manager.Comment("reaching state \'S55\'");
            this.Manager.Comment("checking step \'return EstablishConnection/[out Valid,out Valid]:ERROR_SUCCESS\'");
            this.Manager.Assert((temp108 == FRS2Model.ProtocolVersionReturned.Valid), String.Format("expected \'FRS2Model.ProtocolVersionReturned.Valid\', actual \'{0}\' (upstreamProtoco" +
                        "lVersion of EstablishConnection, state S55)", TestManagerHelpers.Describe(temp108)));
            this.Manager.Assert((temp109 == FRS2Model.UpstreamFlagValueReturned.Valid), String.Format("expected \'FRS2Model.UpstreamFlagValueReturned.Valid\', actual \'{0}\' (upstreamFlags" +
                        " of EstablishConnection, state S55)", TestManagerHelpers.Describe(temp109)));
            this.Manager.Assert((temp110 == FRS2Model.error_status_t.ERROR_SUCCESS), String.Format("expected \'FRS2Model.error_status_t.ERROR_SUCCESS\', actual \'{0}\' (return of Establ" +
                        "ishConnection, state S55)", TestManagerHelpers.Describe(temp110)));
            this.Manager.Comment("reaching state \'S69\'");
            FRS2Model.error_status_t temp111;
            this.Manager.Comment("executing step \'call EstablishSession(1,1)\'");
            temp111 = this.IFRS2ManagedAdapterInstance.EstablishSession(1, 1);
            this.Manager.Checkpoint("MS-FRS2_R455");
            this.Manager.Checkpoint("MS-FRS2_R458");
            this.Manager.Checkpoint("MS-FRS2_R448");
            this.Manager.Comment("reaching state \'S81\'");
            this.Manager.Comment("checking step \'return EstablishSession/ERROR_SUCCESS\'");
            this.Manager.Assert((temp111 == FRS2Model.error_status_t.ERROR_SUCCESS), String.Format("expected \'FRS2Model.error_status_t.ERROR_SUCCESS\', actual \'{0}\' (return of Establ" +
                        "ishSession, state S81)", TestManagerHelpers.Describe(temp111)));
            this.Manager.Comment("reaching state \'S93\'");
            FRS2Model.error_status_t temp112;
            this.Manager.Comment("executing step \'call RequestVersionVector(1,1,1,REQUEST_SLOW_SYNC,CHANGE_NOTIFY,V" +
                    "alidValue)\'");
            temp112 = this.IFRS2ManagedAdapterInstance.RequestVersionVector(1, 1, 1, FRS2Model.VERSION_REQUEST_TYPE.REQUEST_SLOW_SYNC, FRS2Model.VERSION_CHANGE_TYPE.CHANGE_NOTIFY, FRS2Model.VVGeneration.ValidValue);
            this.Manager.Checkpoint("MS-FRS2_R518");
            this.Manager.Comment("reaching state \'S104\'");
            this.Manager.Comment("checking step \'return RequestVersionVector/ERROR_SUCCESS\'");
            this.Manager.Assert((temp112 == FRS2Model.error_status_t.ERROR_SUCCESS), String.Format("expected \'FRS2Model.error_status_t.ERROR_SUCCESS\', actual \'{0}\' (return of Reques" +
                        "tVersionVector, state S104)", TestManagerHelpers.Describe(temp112)));
            TCtestScenario5S112();
            this.Manager.EndTest();
        }
        #endregion
    }
}
