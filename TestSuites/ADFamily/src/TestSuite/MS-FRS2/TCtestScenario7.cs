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
    public partial class TCtestScenario7 : PtfTestClassBase {
        
        public TCtestScenario7() {
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
        
        public delegate void CheckConnectivityDelegate1(FRS2Model.error_status_t @return);
        
        public delegate void EstablishConnectionDelegate1(FRS2Model.ProtocolVersionReturned upstreamProtocolVersion, FRS2Model.UpstreamFlagValueReturned upstreamFlags, FRS2Model.error_status_t @return);
        
        public delegate void EstablishSessionDelegate1(FRS2Model.error_status_t @return);
        
        public delegate void AsyncPollDelegate1(FRS2Model.error_status_t @return);
        
        public delegate void RequestRecordsDelegate1(FRS2Model.error_status_t @return);
        
        public delegate void RequestUpdatesDelegate1(FRS2Model.error_status_t @return);
        
        public delegate void RequestVersionVectorDelegate1(FRS2Model.error_status_t @return);
        
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
        
        public delegate void AsyncPollResponseEventDelegate1(FRS2Model.VVGeneration vvGen);
        
        public delegate void InitializeFileTransferAsyncEventDelegate1(FRS2Model.ServerContext context, FRS2Model.RDC_Sig_Level rdcsigLevel, bool isEOF);
        
        public delegate void RequestUpdatesEventDelegate1(FRS2Model.FilePresense present, FRS2Model.UPDATE_STATUS updateStatus);
        
        public delegate void RawGetFileDataResponseEventDelegate1(bool isEOF);
        
        public delegate void RdcGetFileDataEventDelegate1(FRS2Model.SizeReturned sizeReturned);
        
        public delegate void RequestRecordsEventDelegate1(FRS2Model.RecordsStatus status);
        #endregion
        
        #region Event Metadata
        static System.Reflection.MethodBase InitializationInfo = TestManagerHelpers.GetMethodInfo(typeof(Microsoft.Protocols.TestSuites.MS_FRS2.IFRS2ManagedAdapter), "Initialization", typeof(FRS2Model.OSVersion), typeof(Microsoft.Modeling.Map<System.Int32,FRS2Model.connectionProperty>), typeof(Microsoft.Modeling.Map<System.Int32,FRS2Model.FromServer>), typeof(Microsoft.Modeling.Map<System.String,System.Int32>), typeof(Microsoft.Modeling.Map<System.String,FRS2Model.ReplicationGroupTypes>), typeof(Microsoft.Modeling.Map<System.Int32,System.String>), typeof(Microsoft.Modeling.Map<System.Int32,FRS2Model.accessLevels>), typeof(Microsoft.Modeling.Map<System.Int32,FRS2Model.connectionProperty>));
        
        static System.Reflection.MethodBase CheckConnectivityInfo = TestManagerHelpers.GetMethodInfo(typeof(Microsoft.Protocols.TestSuites.MS_FRS2.IFRS2ManagedAdapter), "CheckConnectivity", typeof(string), typeof(int));
        
        static System.Reflection.MethodBase EstablishConnectionInfo = TestManagerHelpers.GetMethodInfo(typeof(Microsoft.Protocols.TestSuites.MS_FRS2.IFRS2ManagedAdapter), "EstablishConnection", typeof(string), typeof(int), typeof(FRS2Model.ProtocolVersion), typeof(FRS2Model.ProtocolVersionReturned).MakeByRefType(), typeof(FRS2Model.UpstreamFlagValueReturned).MakeByRefType());
        
        static System.Reflection.MethodBase EstablishSessionInfo = TestManagerHelpers.GetMethodInfo(typeof(Microsoft.Protocols.TestSuites.MS_FRS2.IFRS2ManagedAdapter), "EstablishSession", typeof(int), typeof(int));
        
        static System.Reflection.MethodBase AsyncPollInfo = TestManagerHelpers.GetMethodInfo(typeof(Microsoft.Protocols.TestSuites.MS_FRS2.IFRS2ManagedAdapter), "AsyncPoll", typeof(int));
        
        static System.Reflection.MethodBase RequestRecordsInfo = TestManagerHelpers.GetMethodInfo(typeof(Microsoft.Protocols.TestSuites.MS_FRS2.IFRS2ManagedAdapter), "RequestRecords", typeof(int), typeof(int));
        
        static System.Reflection.MethodBase RequestUpdatesInfo = TestManagerHelpers.GetMethodInfo(typeof(Microsoft.Protocols.TestSuites.MS_FRS2.IFRS2ManagedAdapter), "RequestUpdates", typeof(int), typeof(int), typeof(FRS2Model.versionVectorDiff));
        
        static System.Reflection.MethodBase RequestVersionVectorInfo = TestManagerHelpers.GetMethodInfo(typeof(Microsoft.Protocols.TestSuites.MS_FRS2.IFRS2ManagedAdapter), "RequestVersionVector", typeof(int), typeof(int), typeof(int), typeof(FRS2Model.VERSION_REQUEST_TYPE), typeof(FRS2Model.VERSION_CHANGE_TYPE), typeof(FRS2Model.VVGeneration));
        
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
        
        static System.Reflection.EventInfo AsyncPollResponseEventInfo = TestManagerHelpers.GetEventInfo(typeof(Microsoft.Protocols.TestSuites.MS_FRS2.IFRS2ManagedAdapter), "AsyncPollResponseEvent");
        
        static System.Reflection.EventInfo InitializeFileTransferAsyncEventInfo = TestManagerHelpers.GetEventInfo(typeof(Microsoft.Protocols.TestSuites.MS_FRS2.IFRS2ManagedAdapter), "InitializeFileTransferAsyncEvent");
        
        static System.Reflection.EventInfo RequestUpdatesEventInfo = TestManagerHelpers.GetEventInfo(typeof(Microsoft.Protocols.TestSuites.MS_FRS2.IFRS2ManagedAdapter), "RequestUpdatesEvent");
        
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
            this.Manager.Subscribe(InitializeFileTransferAsyncEventInfo, this.IFRS2ManagedAdapterInstance);
            this.Manager.Subscribe(RequestUpdatesEventInfo, this.IFRS2ManagedAdapterInstance);
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
        [Microsoft.VisualStudio.TestTools.UnitTesting.DescriptionAttribute("MS-FRS2_R408, MS-FRS2_R413, MS-FRS2_R415, MS-FRS2_R414, MS-FRS2_R440, MS-FRS2_R44" +
            "7")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestCategory("AD")]
        [TestCategory("MS-FRS2")]
        [TestCategory("SDC")]
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        
        public virtual void FRS2_TCtestScenario7S0()
        {
            this.Manager.BeginTest("TCtestScenario7S0");
            this.Manager.Comment("reaching state \'S0\'");
            FRS2Model.error_status_t temp0;
            this.Manager.Comment(@"executing step 'call Initialization(Windows7,Map{1=>enabled,2=>enabled,3=>enabled,4=>disabled},Map{1=>exists,2=>exists,3=>exists,4=>exists},Map{""A""=>1,""B""=>2,""C""=>3,""D""=>4},Map{""A""=>sysvol,""B""=>protection,""C""=>sysvol,""D""=>sysvol},Map{1=>""A"",2=>""B"",3=>""C"",4=>""D""},Map{1=>writeOnly,2=>readWrite,3=>readOnly,4=>readWrite},Map{1=>enabled,2=>disabled,3=>enabled,4=>enabled})'");
            temp0 = this.IFRS2ManagedAdapterInstance.Initialization(FRS2Model.OSVersion.Windows7, new Microsoft.Modeling.Map<System.Int32,FRS2Model.connectionProperty>().Add(1, FRS2Model.connectionProperty.enabled).Add(2, FRS2Model.connectionProperty.enabled).Add(3, FRS2Model.connectionProperty.enabled).Add(4, FRS2Model.connectionProperty.disabled), new Microsoft.Modeling.Map<System.Int32,FRS2Model.FromServer>().Add(1, FRS2Model.FromServer.exists).Add(2, FRS2Model.FromServer.exists).Add(3, FRS2Model.FromServer.exists).Add(4, FRS2Model.FromServer.exists), new Microsoft.Modeling.Map<System.String,System.Int32>().Add("A", 1).Add("B", 2).Add("C", 3).Add("D", 4), new Microsoft.Modeling.Map<System.String,FRS2Model.ReplicationGroupTypes>().Add("A", FRS2Model.ReplicationGroupTypes.sysvol).Add("B", FRS2Model.ReplicationGroupTypes.protection).Add("C", FRS2Model.ReplicationGroupTypes.sysvol).Add("D", FRS2Model.ReplicationGroupTypes.sysvol), new Microsoft.Modeling.Map<System.Int32,System.String>().Add(1, "A").Add(2, "B").Add(3, "C").Add(4, "D"), new Microsoft.Modeling.Map<System.Int32,FRS2Model.accessLevels>().Add(1, FRS2Model.accessLevels.writeOnly).Add(2, FRS2Model.accessLevels.readWrite).Add(3, FRS2Model.accessLevels.readOnly).Add(4, FRS2Model.accessLevels.readWrite), new Microsoft.Modeling.Map<System.Int32,FRS2Model.connectionProperty>().Add(1, FRS2Model.connectionProperty.enabled).Add(2, FRS2Model.connectionProperty.disabled).Add(3, FRS2Model.connectionProperty.enabled).Add(4, FRS2Model.connectionProperty.enabled));
            this.Manager.Comment("reaching state \'S1\'");
            this.Manager.Comment("checking step \'return Initialization/ERROR_SUCCESS\'");
            this.Manager.Assert((temp0 == FRS2Model.error_status_t.ERROR_SUCCESS), String.Format("expected \'FRS2Model.error_status_t.ERROR_SUCCESS\', actual \'{0}\' (return of Initia" +
                        "lization, state S1)", TestManagerHelpers.Describe(temp0)));
            this.Manager.Comment("reaching state \'S56\'");
            FRS2Model.error_status_t temp1;
            this.Manager.Comment("executing step \'call CheckConnectivity(\"D\",4)\'");
            temp1 = this.IFRS2ManagedAdapterInstance.CheckConnectivity("D", 4);
            this.Manager.Checkpoint("MS-FRS2_R408");
            this.Manager.Checkpoint("MS-FRS2_R413");
            this.Manager.Checkpoint("MS-FRS2_R415");
            this.Manager.Checkpoint("MS-FRS2_R414");
            this.Manager.Comment("reaching state \'S84\'");
            this.Manager.Comment("checking step \'return CheckConnectivity/ERROR_FAIL\'");
            this.Manager.Assert((temp1 == FRS2Model.error_status_t.ERROR_FAIL), String.Format("expected \'FRS2Model.error_status_t.ERROR_FAIL\', actual \'{0}\' (return of CheckConn" +
                        "ectivity, state S84)", TestManagerHelpers.Describe(temp1)));
            this.Manager.Comment("reaching state \'S112\'");
            FRS2Model.ProtocolVersionReturned temp2;
            FRS2Model.UpstreamFlagValueReturned temp3;
            FRS2Model.error_status_t temp4;
            this.Manager.Comment("executing step \'call EstablishConnection(\"A\",1,inValid,out _,out _)\'");
            temp4 = this.IFRS2ManagedAdapterInstance.EstablishConnection("A", 1, FRS2Model.ProtocolVersion.inValid, out temp2, out temp3);
            this.Manager.Checkpoint("MS-FRS2_R440");
            this.Manager.Checkpoint("MS-FRS2_R447");
            this.Manager.Comment("reaching state \'S140\'");
            this.Manager.Comment("checking step \'return EstablishConnection/[out Invalid,out Invalid]:FRS_ERROR_INC" +
                    "OMPATIBLE_VERSION\'");
            this.Manager.Assert((temp2 == FRS2Model.ProtocolVersionReturned.Invalid), String.Format("expected \'FRS2Model.ProtocolVersionReturned.Invalid\', actual \'{0}\' (upstreamProto" +
                        "colVersion of EstablishConnection, state S140)", TestManagerHelpers.Describe(temp2)));
            this.Manager.Assert((temp3 == FRS2Model.UpstreamFlagValueReturned.Invalid), String.Format("expected \'FRS2Model.UpstreamFlagValueReturned.Invalid\', actual \'{0}\' (upstreamFla" +
                        "gs of EstablishConnection, state S140)", TestManagerHelpers.Describe(temp3)));
            this.Manager.Assert((temp4 == FRS2Model.error_status_t.FRS_ERROR_INCOMPATIBLE_VERSION), String.Format("expected \'FRS2Model.error_status_t.FRS_ERROR_INCOMPATIBLE_VERSION\', actual \'{0}\' " +
                        "(return of EstablishConnection, state S140)", TestManagerHelpers.Describe(temp4)));
            TCtestScenario7S168();
            this.Manager.EndTest();
        }
        
        private void TCtestScenario7S168() {
            this.Manager.Comment("reaching state \'S168\'");
        }
        #endregion
        
        #region Test Starting in S2
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        [Microsoft.VisualStudio.TestTools.UnitTesting.DescriptionAttribute("MS-FRS2_R408, MS-FRS2_R413, MS-FRS2_R415, MS-FRS2_R414, MS-FRS2_R37, MS-FRS2_R436" +
            ", MS-FRS2_R439, MS-FRS2_R455, MS-FRS2_R458, MS-FRS2_R448")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestCategory("AD")]
        [TestCategory("MS-FRS2")]
        [TestCategory("SDC")]
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        
        public virtual void FRS2_TCtestScenario7S2()
        {
            this.Manager.BeginTest("TCtestScenario7S2");
            this.Manager.Comment("reaching state \'S2\'");
            FRS2Model.error_status_t temp5;
            this.Manager.Comment(@"executing step 'call Initialization(Windows7,Map{1=>enabled,2=>enabled,3=>enabled,4=>disabled},Map{1=>exists,2=>exists,3=>exists,4=>exists},Map{""A""=>1,""B""=>2,""C""=>3,""D""=>4},Map{""A""=>sysvol,""B""=>protection,""C""=>sysvol,""D""=>sysvol},Map{1=>""A"",2=>""B"",3=>""C"",4=>""D""},Map{1=>writeOnly,2=>readWrite,3=>readOnly,4=>readWrite},Map{1=>enabled,2=>disabled,3=>enabled,4=>enabled})'");
            temp5 = this.IFRS2ManagedAdapterInstance.Initialization(FRS2Model.OSVersion.Windows7, new Microsoft.Modeling.Map<System.Int32,FRS2Model.connectionProperty>().Add(1, FRS2Model.connectionProperty.enabled).Add(2, FRS2Model.connectionProperty.enabled).Add(3, FRS2Model.connectionProperty.enabled).Add(4, FRS2Model.connectionProperty.disabled), new Microsoft.Modeling.Map<System.Int32,FRS2Model.FromServer>().Add(1, FRS2Model.FromServer.exists).Add(2, FRS2Model.FromServer.exists).Add(3, FRS2Model.FromServer.exists).Add(4, FRS2Model.FromServer.exists), new Microsoft.Modeling.Map<System.String,System.Int32>().Add("A", 1).Add("B", 2).Add("C", 3).Add("D", 4), new Microsoft.Modeling.Map<System.String,FRS2Model.ReplicationGroupTypes>().Add("A", FRS2Model.ReplicationGroupTypes.sysvol).Add("B", FRS2Model.ReplicationGroupTypes.protection).Add("C", FRS2Model.ReplicationGroupTypes.sysvol).Add("D", FRS2Model.ReplicationGroupTypes.sysvol), new Microsoft.Modeling.Map<System.Int32,System.String>().Add(1, "A").Add(2, "B").Add(3, "C").Add(4, "D"), new Microsoft.Modeling.Map<System.Int32,FRS2Model.accessLevels>().Add(1, FRS2Model.accessLevels.writeOnly).Add(2, FRS2Model.accessLevels.readWrite).Add(3, FRS2Model.accessLevels.readOnly).Add(4, FRS2Model.accessLevels.readWrite), new Microsoft.Modeling.Map<System.Int32,FRS2Model.connectionProperty>().Add(1, FRS2Model.connectionProperty.enabled).Add(2, FRS2Model.connectionProperty.disabled).Add(3, FRS2Model.connectionProperty.enabled).Add(4, FRS2Model.connectionProperty.enabled));
            this.Manager.Comment("reaching state \'S3\'");
            this.Manager.Comment("checking step \'return Initialization/ERROR_SUCCESS\'");
            this.Manager.Assert((temp5 == FRS2Model.error_status_t.ERROR_SUCCESS), String.Format("expected \'FRS2Model.error_status_t.ERROR_SUCCESS\', actual \'{0}\' (return of Initia" +
                        "lization, state S3)", TestManagerHelpers.Describe(temp5)));
            this.Manager.Comment("reaching state \'S57\'");
            FRS2Model.error_status_t temp6;
            this.Manager.Comment("executing step \'call CheckConnectivity(\"D\",4)\'");
            temp6 = this.IFRS2ManagedAdapterInstance.CheckConnectivity("D", 4);
            this.Manager.Checkpoint("MS-FRS2_R408");
            this.Manager.Checkpoint("MS-FRS2_R413");
            this.Manager.Checkpoint("MS-FRS2_R415");
            this.Manager.Checkpoint("MS-FRS2_R414");
            this.Manager.Comment("reaching state \'S85\'");
            this.Manager.Comment("checking step \'return CheckConnectivity/ERROR_FAIL\'");
            this.Manager.Assert((temp6 == FRS2Model.error_status_t.ERROR_FAIL), String.Format("expected \'FRS2Model.error_status_t.ERROR_FAIL\', actual \'{0}\' (return of CheckConn" +
                        "ectivity, state S85)", TestManagerHelpers.Describe(temp6)));
            this.Manager.Comment("reaching state \'S113\'");
            FRS2Model.ProtocolVersionReturned temp7;
            FRS2Model.UpstreamFlagValueReturned temp8;
            FRS2Model.error_status_t temp9;
            this.Manager.Comment("executing step \'call EstablishConnection(\"C\",3,FRS_COMMUNICATION_PROTOCOL_VERSION" +
                    "_LONGHORN_SERVER,out _,out _)\'");
            temp9 = this.IFRS2ManagedAdapterInstance.EstablishConnection("C", 3, FRS2Model.ProtocolVersion.FRS_COMMUNICATION_PROTOCOL_VERSION_LONGHORN_SERVER, out temp7, out temp8);
            this.Manager.Checkpoint("MS-FRS2_R37");
            this.Manager.Checkpoint("MS-FRS2_R436");
            this.Manager.Checkpoint("MS-FRS2_R439");
            this.Manager.Comment("reaching state \'S141\'");
            this.Manager.Comment("checking step \'return EstablishConnection/[out Valid,out Valid]:ERROR_SUCCESS\'");
            this.Manager.Assert((temp7 == FRS2Model.ProtocolVersionReturned.Valid), String.Format("expected \'FRS2Model.ProtocolVersionReturned.Valid\', actual \'{0}\' (upstreamProtoco" +
                        "lVersion of EstablishConnection, state S141)", TestManagerHelpers.Describe(temp7)));
            this.Manager.Assert((temp8 == FRS2Model.UpstreamFlagValueReturned.Valid), String.Format("expected \'FRS2Model.UpstreamFlagValueReturned.Valid\', actual \'{0}\' (upstreamFlags" +
                        " of EstablishConnection, state S141)", TestManagerHelpers.Describe(temp8)));
            this.Manager.Assert((temp9 == FRS2Model.error_status_t.ERROR_SUCCESS), String.Format("expected \'FRS2Model.error_status_t.ERROR_SUCCESS\', actual \'{0}\' (return of Establ" +
                        "ishConnection, state S141)", TestManagerHelpers.Describe(temp9)));
            this.Manager.Comment("reaching state \'S169\'");
            FRS2Model.error_status_t temp10;
            this.Manager.Comment("executing step \'call EstablishSession(1,1)\'");
            temp10 = this.IFRS2ManagedAdapterInstance.EstablishSession(1, 1);
            this.Manager.Checkpoint("MS-FRS2_R455");
            this.Manager.Checkpoint("MS-FRS2_R458");
            this.Manager.Checkpoint("MS-FRS2_R448");
            this.Manager.Comment("reaching state \'S196\'");
            this.Manager.Comment("checking step \'return EstablishSession/ERROR_SUCCESS\'");
            this.Manager.Assert((temp10 == FRS2Model.error_status_t.ERROR_SUCCESS), String.Format("expected \'FRS2Model.error_status_t.ERROR_SUCCESS\', actual \'{0}\' (return of Establ" +
                        "ishSession, state S196)", TestManagerHelpers.Describe(temp10)));
            this.Manager.Comment("reaching state \'S216\'");
            this.Manager.EndTest();
        }
        #endregion
        
        #region Test Starting in S4
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        [Microsoft.VisualStudio.TestTools.UnitTesting.DescriptionAttribute("MS-FRS2_R408, MS-FRS2_R413, MS-FRS2_R415, MS-FRS2_R414, MS-FRS2_R37, MS-FRS2_R436" +
            ", MS-FRS2_R439, MS-FRS2_R456, MS-FRS2_R459, MS-FRS2_R462")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestCategory("AD")]
        [TestCategory("MS-FRS2")]
        [TestCategory("SDC")]
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        
        public virtual void FRS2_TCtestScenario7S4()
        {
            this.Manager.BeginTest("TCtestScenario7S4");
            this.Manager.Comment("reaching state \'S4\'");
            FRS2Model.error_status_t temp11;
            this.Manager.Comment(@"executing step 'call Initialization(Windows7,Map{1=>enabled,2=>enabled,3=>enabled,4=>disabled},Map{1=>exists,2=>exists,3=>exists,4=>exists},Map{""A""=>1,""B""=>2,""C""=>3,""D""=>4},Map{""A""=>sysvol,""B""=>protection,""C""=>sysvol,""D""=>sysvol},Map{1=>""A"",2=>""B"",3=>""C"",4=>""D""},Map{1=>writeOnly,2=>readWrite,3=>readOnly,4=>readWrite},Map{1=>enabled,2=>disabled,3=>enabled,4=>enabled})'");
            temp11 = this.IFRS2ManagedAdapterInstance.Initialization(FRS2Model.OSVersion.Windows7, new Microsoft.Modeling.Map<System.Int32,FRS2Model.connectionProperty>().Add(1, FRS2Model.connectionProperty.enabled).Add(2, FRS2Model.connectionProperty.enabled).Add(3, FRS2Model.connectionProperty.enabled).Add(4, FRS2Model.connectionProperty.disabled), new Microsoft.Modeling.Map<System.Int32,FRS2Model.FromServer>().Add(1, FRS2Model.FromServer.exists).Add(2, FRS2Model.FromServer.exists).Add(3, FRS2Model.FromServer.exists).Add(4, FRS2Model.FromServer.exists), new Microsoft.Modeling.Map<System.String,System.Int32>().Add("A", 1).Add("B", 2).Add("C", 3).Add("D", 4), new Microsoft.Modeling.Map<System.String,FRS2Model.ReplicationGroupTypes>().Add("A", FRS2Model.ReplicationGroupTypes.sysvol).Add("B", FRS2Model.ReplicationGroupTypes.protection).Add("C", FRS2Model.ReplicationGroupTypes.sysvol).Add("D", FRS2Model.ReplicationGroupTypes.sysvol), new Microsoft.Modeling.Map<System.Int32,System.String>().Add(1, "A").Add(2, "B").Add(3, "C").Add(4, "D"), new Microsoft.Modeling.Map<System.Int32,FRS2Model.accessLevels>().Add(1, FRS2Model.accessLevels.writeOnly).Add(2, FRS2Model.accessLevels.readWrite).Add(3, FRS2Model.accessLevels.readOnly).Add(4, FRS2Model.accessLevels.readWrite), new Microsoft.Modeling.Map<System.Int32,FRS2Model.connectionProperty>().Add(1, FRS2Model.connectionProperty.enabled).Add(2, FRS2Model.connectionProperty.disabled).Add(3, FRS2Model.connectionProperty.enabled).Add(4, FRS2Model.connectionProperty.enabled));
            this.Manager.Comment("reaching state \'S5\'");
            this.Manager.Comment("checking step \'return Initialization/ERROR_SUCCESS\'");
            this.Manager.Assert((temp11 == FRS2Model.error_status_t.ERROR_SUCCESS), String.Format("expected \'FRS2Model.error_status_t.ERROR_SUCCESS\', actual \'{0}\' (return of Initia" +
                        "lization, state S5)", TestManagerHelpers.Describe(temp11)));
            this.Manager.Comment("reaching state \'S58\'");
            FRS2Model.error_status_t temp12;
            this.Manager.Comment("executing step \'call CheckConnectivity(\"D\",4)\'");
            temp12 = this.IFRS2ManagedAdapterInstance.CheckConnectivity("D", 4);
            this.Manager.Checkpoint("MS-FRS2_R408");
            this.Manager.Checkpoint("MS-FRS2_R413");
            this.Manager.Checkpoint("MS-FRS2_R415");
            this.Manager.Checkpoint("MS-FRS2_R414");
            this.Manager.Comment("reaching state \'S86\'");
            this.Manager.Comment("checking step \'return CheckConnectivity/ERROR_FAIL\'");
            this.Manager.Assert((temp12 == FRS2Model.error_status_t.ERROR_FAIL), String.Format("expected \'FRS2Model.error_status_t.ERROR_FAIL\', actual \'{0}\' (return of CheckConn" +
                        "ectivity, state S86)", TestManagerHelpers.Describe(temp12)));
            this.Manager.Comment("reaching state \'S114\'");
            FRS2Model.ProtocolVersionReturned temp13;
            FRS2Model.UpstreamFlagValueReturned temp14;
            FRS2Model.error_status_t temp15;
            this.Manager.Comment("executing step \'call EstablishConnection(\"C\",3,FRS_COMMUNICATION_PROTOCOL_VERSION" +
                    "_LONGHORN_SERVER,out _,out _)\'");
            temp15 = this.IFRS2ManagedAdapterInstance.EstablishConnection("C", 3, FRS2Model.ProtocolVersion.FRS_COMMUNICATION_PROTOCOL_VERSION_LONGHORN_SERVER, out temp13, out temp14);
            this.Manager.Checkpoint("MS-FRS2_R37");
            this.Manager.Checkpoint("MS-FRS2_R436");
            this.Manager.Checkpoint("MS-FRS2_R439");
            this.Manager.Comment("reaching state \'S142\'");
            this.Manager.Comment("checking step \'return EstablishConnection/[out Valid,out Valid]:ERROR_SUCCESS\'");
            this.Manager.Assert((temp13 == FRS2Model.ProtocolVersionReturned.Valid), String.Format("expected \'FRS2Model.ProtocolVersionReturned.Valid\', actual \'{0}\' (upstreamProtoco" +
                        "lVersion of EstablishConnection, state S142)", TestManagerHelpers.Describe(temp13)));
            this.Manager.Assert((temp14 == FRS2Model.UpstreamFlagValueReturned.Valid), String.Format("expected \'FRS2Model.UpstreamFlagValueReturned.Valid\', actual \'{0}\' (upstreamFlags" +
                        " of EstablishConnection, state S142)", TestManagerHelpers.Describe(temp14)));
            this.Manager.Assert((temp15 == FRS2Model.error_status_t.ERROR_SUCCESS), String.Format("expected \'FRS2Model.error_status_t.ERROR_SUCCESS\', actual \'{0}\' (return of Establ" +
                        "ishConnection, state S142)", TestManagerHelpers.Describe(temp15)));
            this.Manager.Comment("reaching state \'S170\'");
            FRS2Model.error_status_t temp16;
            this.Manager.Comment("executing step \'call EstablishSession(4,4)\'");
            temp16 = this.IFRS2ManagedAdapterInstance.EstablishSession(4, 4);
            this.Manager.Checkpoint("MS-FRS2_R456");
            this.Manager.Checkpoint("MS-FRS2_R459");
            this.Manager.Checkpoint("MS-FRS2_R462");
            this.Manager.Comment("reaching state \'S197\'");
            this.Manager.Comment("checking step \'return EstablishSession/FRS_ERROR_CONNECTION_INVALID\'");
            this.Manager.Assert((temp16 == FRS2Model.error_status_t.FRS_ERROR_CONNECTION_INVALID), String.Format("expected \'FRS2Model.error_status_t.FRS_ERROR_CONNECTION_INVALID\', actual \'{0}\' (r" +
                        "eturn of EstablishSession, state S197)", TestManagerHelpers.Describe(temp16)));
            TCtestScenario7S217();
            this.Manager.EndTest();
        }
        
        private void TCtestScenario7S217() {
            this.Manager.Comment("reaching state \'S217\'");
        }
        #endregion
        
        #region Test Starting in S6
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        [Microsoft.VisualStudio.TestTools.UnitTesting.DescriptionAttribute("MS-FRS2_R408, MS-FRS2_R413, MS-FRS2_R415, MS-FRS2_R414, MS-FRS2_R37, MS-FRS2_R436" +
            ", MS-FRS2_R439")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestCategory("AD")]
        [TestCategory("MS-FRS2")]
        [TestCategory("SDC")]
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        
        public virtual void FRS2_TCtestScenario7S6()
        {
            this.Manager.BeginTest("TCtestScenario7S6");
            this.Manager.Comment("reaching state \'S6\'");
            FRS2Model.error_status_t temp17;
            this.Manager.Comment(@"executing step 'call Initialization(Windows7,Map{1=>enabled,2=>enabled,3=>enabled,4=>disabled},Map{1=>exists,2=>exists,3=>exists,4=>exists},Map{""A""=>1,""B""=>2,""C""=>3,""D""=>4},Map{""A""=>sysvol,""B""=>protection,""C""=>sysvol,""D""=>sysvol},Map{1=>""A"",2=>""B"",3=>""C"",4=>""D""},Map{1=>writeOnly,2=>readWrite,3=>readOnly,4=>readWrite},Map{1=>enabled,2=>disabled,3=>enabled,4=>enabled})'");
            temp17 = this.IFRS2ManagedAdapterInstance.Initialization(FRS2Model.OSVersion.Windows7, new Microsoft.Modeling.Map<System.Int32,FRS2Model.connectionProperty>().Add(1, FRS2Model.connectionProperty.enabled).Add(2, FRS2Model.connectionProperty.enabled).Add(3, FRS2Model.connectionProperty.enabled).Add(4, FRS2Model.connectionProperty.disabled), new Microsoft.Modeling.Map<System.Int32,FRS2Model.FromServer>().Add(1, FRS2Model.FromServer.exists).Add(2, FRS2Model.FromServer.exists).Add(3, FRS2Model.FromServer.exists).Add(4, FRS2Model.FromServer.exists), new Microsoft.Modeling.Map<System.String,System.Int32>().Add("A", 1).Add("B", 2).Add("C", 3).Add("D", 4), new Microsoft.Modeling.Map<System.String,FRS2Model.ReplicationGroupTypes>().Add("A", FRS2Model.ReplicationGroupTypes.sysvol).Add("B", FRS2Model.ReplicationGroupTypes.protection).Add("C", FRS2Model.ReplicationGroupTypes.sysvol).Add("D", FRS2Model.ReplicationGroupTypes.sysvol), new Microsoft.Modeling.Map<System.Int32,System.String>().Add(1, "A").Add(2, "B").Add(3, "C").Add(4, "D"), new Microsoft.Modeling.Map<System.Int32,FRS2Model.accessLevels>().Add(1, FRS2Model.accessLevels.writeOnly).Add(2, FRS2Model.accessLevels.readWrite).Add(3, FRS2Model.accessLevels.readOnly).Add(4, FRS2Model.accessLevels.readWrite), new Microsoft.Modeling.Map<System.Int32,FRS2Model.connectionProperty>().Add(1, FRS2Model.connectionProperty.enabled).Add(2, FRS2Model.connectionProperty.disabled).Add(3, FRS2Model.connectionProperty.enabled).Add(4, FRS2Model.connectionProperty.enabled));
            this.Manager.Comment("reaching state \'S7\'");
            this.Manager.Comment("checking step \'return Initialization/ERROR_SUCCESS\'");
            this.Manager.Assert((temp17 == FRS2Model.error_status_t.ERROR_SUCCESS), String.Format("expected \'FRS2Model.error_status_t.ERROR_SUCCESS\', actual \'{0}\' (return of Initia" +
                        "lization, state S7)", TestManagerHelpers.Describe(temp17)));
            this.Manager.Comment("reaching state \'S59\'");
            FRS2Model.error_status_t temp18;
            this.Manager.Comment("executing step \'call CheckConnectivity(\"D\",4)\'");
            temp18 = this.IFRS2ManagedAdapterInstance.CheckConnectivity("D", 4);
            this.Manager.Checkpoint("MS-FRS2_R408");
            this.Manager.Checkpoint("MS-FRS2_R413");
            this.Manager.Checkpoint("MS-FRS2_R415");
            this.Manager.Checkpoint("MS-FRS2_R414");
            this.Manager.Comment("reaching state \'S87\'");
            this.Manager.Comment("checking step \'return CheckConnectivity/ERROR_FAIL\'");
            this.Manager.Assert((temp18 == FRS2Model.error_status_t.ERROR_FAIL), String.Format("expected \'FRS2Model.error_status_t.ERROR_FAIL\', actual \'{0}\' (return of CheckConn" +
                        "ectivity, state S87)", TestManagerHelpers.Describe(temp18)));
            this.Manager.Comment("reaching state \'S115\'");
            FRS2Model.ProtocolVersionReturned temp19;
            FRS2Model.UpstreamFlagValueReturned temp20;
            FRS2Model.error_status_t temp21;
            this.Manager.Comment("executing step \'call EstablishConnection(\"C\",3,FRS_COMMUNICATION_PROTOCOL_VERSION" +
                    "_LONGHORN_SERVER,out _,out _)\'");
            temp21 = this.IFRS2ManagedAdapterInstance.EstablishConnection("C", 3, FRS2Model.ProtocolVersion.FRS_COMMUNICATION_PROTOCOL_VERSION_LONGHORN_SERVER, out temp19, out temp20);
            this.Manager.Checkpoint("MS-FRS2_R37");
            this.Manager.Checkpoint("MS-FRS2_R436");
            this.Manager.Checkpoint("MS-FRS2_R439");
            this.Manager.Comment("reaching state \'S143\'");
            this.Manager.Comment("checking step \'return EstablishConnection/[out Valid,out Valid]:ERROR_SUCCESS\'");
            this.Manager.Assert((temp19 == FRS2Model.ProtocolVersionReturned.Valid), String.Format("expected \'FRS2Model.ProtocolVersionReturned.Valid\', actual \'{0}\' (upstreamProtoco" +
                        "lVersion of EstablishConnection, state S143)", TestManagerHelpers.Describe(temp19)));
            this.Manager.Assert((temp20 == FRS2Model.UpstreamFlagValueReturned.Valid), String.Format("expected \'FRS2Model.UpstreamFlagValueReturned.Valid\', actual \'{0}\' (upstreamFlags" +
                        " of EstablishConnection, state S143)", TestManagerHelpers.Describe(temp20)));
            this.Manager.Assert((temp21 == FRS2Model.error_status_t.ERROR_SUCCESS), String.Format("expected \'FRS2Model.error_status_t.ERROR_SUCCESS\', actual \'{0}\' (return of Establ" +
                        "ishConnection, state S143)", TestManagerHelpers.Describe(temp21)));
            this.Manager.Comment("reaching state \'S171\'");
            FRS2Model.error_status_t temp22;
            this.Manager.Comment("executing step \'call EstablishSession(2,2)\'");
            temp22 = this.IFRS2ManagedAdapterInstance.EstablishSession(2, 2);
            this.Manager.Comment("reaching state \'S198\'");
            this.Manager.Comment("checking step \'return EstablishSession/FRS_ERROR_CONNECTION_INVALID\'");
            this.Manager.Assert((temp22 == FRS2Model.error_status_t.FRS_ERROR_CONNECTION_INVALID), String.Format("expected \'FRS2Model.error_status_t.FRS_ERROR_CONNECTION_INVALID\', actual \'{0}\' (r" +
                        "eturn of EstablishSession, state S198)", TestManagerHelpers.Describe(temp22)));
            TCtestScenario7S217();
            this.Manager.EndTest();
        }
        #endregion
        
        #region Test Starting in S8
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        [Microsoft.VisualStudio.TestTools.UnitTesting.DescriptionAttribute("MS-FRS2_R408, MS-FRS2_R413, MS-FRS2_R415, MS-FRS2_R414, MS-FRS2_R37, MS-FRS2_R436" +
            ", MS-FRS2_R439")]
        public virtual void FRS2_TCtestScenario7S8()
        {
            this.Manager.BeginTest("TCtestScenario7S8");
            this.Manager.Comment("reaching state \'S8\'");
            FRS2Model.error_status_t temp23;
            this.Manager.Comment(@"executing step 'call Initialization(Windows7,Map{1=>enabled,2=>enabled,3=>enabled,4=>disabled},Map{1=>exists,2=>exists,3=>exists,4=>exists},Map{""A""=>1,""B""=>2,""C""=>3,""D""=>4},Map{""A""=>sysvol,""B""=>protection,""C""=>sysvol,""D""=>sysvol},Map{1=>""A"",2=>""B"",3=>""C"",4=>""D""},Map{1=>writeOnly,2=>readWrite,3=>readOnly,4=>readWrite},Map{1=>enabled,2=>disabled,3=>enabled,4=>enabled})'");
            temp23 = this.IFRS2ManagedAdapterInstance.Initialization(FRS2Model.OSVersion.Windows7, new Microsoft.Modeling.Map<System.Int32,FRS2Model.connectionProperty>().Add(1, FRS2Model.connectionProperty.enabled).Add(2, FRS2Model.connectionProperty.enabled).Add(3, FRS2Model.connectionProperty.enabled).Add(4, FRS2Model.connectionProperty.disabled), new Microsoft.Modeling.Map<System.Int32,FRS2Model.FromServer>().Add(1, FRS2Model.FromServer.exists).Add(2, FRS2Model.FromServer.exists).Add(3, FRS2Model.FromServer.exists).Add(4, FRS2Model.FromServer.exists), new Microsoft.Modeling.Map<System.String,System.Int32>().Add("A", 1).Add("B", 2).Add("C", 3).Add("D", 4), new Microsoft.Modeling.Map<System.String,FRS2Model.ReplicationGroupTypes>().Add("A", FRS2Model.ReplicationGroupTypes.sysvol).Add("B", FRS2Model.ReplicationGroupTypes.protection).Add("C", FRS2Model.ReplicationGroupTypes.sysvol).Add("D", FRS2Model.ReplicationGroupTypes.sysvol), new Microsoft.Modeling.Map<System.Int32,System.String>().Add(1, "A").Add(2, "B").Add(3, "C").Add(4, "D"), new Microsoft.Modeling.Map<System.Int32,FRS2Model.accessLevels>().Add(1, FRS2Model.accessLevels.writeOnly).Add(2, FRS2Model.accessLevels.readWrite).Add(3, FRS2Model.accessLevels.readOnly).Add(4, FRS2Model.accessLevels.readWrite), new Microsoft.Modeling.Map<System.Int32,FRS2Model.connectionProperty>().Add(1, FRS2Model.connectionProperty.enabled).Add(2, FRS2Model.connectionProperty.disabled).Add(3, FRS2Model.connectionProperty.enabled).Add(4, FRS2Model.connectionProperty.enabled));
            this.Manager.Comment("reaching state \'S9\'");
            this.Manager.Comment("checking step \'return Initialization/ERROR_SUCCESS\'");
            this.Manager.Assert((temp23 == FRS2Model.error_status_t.ERROR_SUCCESS), String.Format("expected \'FRS2Model.error_status_t.ERROR_SUCCESS\', actual \'{0}\' (return of Initia" +
                        "lization, state S9)", TestManagerHelpers.Describe(temp23)));
            this.Manager.Comment("reaching state \'S60\'");
            FRS2Model.error_status_t temp24;
            this.Manager.Comment("executing step \'call CheckConnectivity(\"D\",4)\'");
            temp24 = this.IFRS2ManagedAdapterInstance.CheckConnectivity("D", 4);
            this.Manager.Checkpoint("MS-FRS2_R408");
            this.Manager.Checkpoint("MS-FRS2_R413");
            this.Manager.Checkpoint("MS-FRS2_R415");
            this.Manager.Checkpoint("MS-FRS2_R414");
            this.Manager.Comment("reaching state \'S88\'");
            this.Manager.Comment("checking step \'return CheckConnectivity/ERROR_FAIL\'");
            this.Manager.Assert((temp24 == FRS2Model.error_status_t.ERROR_FAIL), String.Format("expected \'FRS2Model.error_status_t.ERROR_FAIL\', actual \'{0}\' (return of CheckConn" +
                        "ectivity, state S88)", TestManagerHelpers.Describe(temp24)));
            this.Manager.Comment("reaching state \'S116\'");
            FRS2Model.ProtocolVersionReturned temp25;
            FRS2Model.UpstreamFlagValueReturned temp26;
            FRS2Model.error_status_t temp27;
            this.Manager.Comment("executing step \'call EstablishConnection(\"C\",3,FRS_COMMUNICATION_PROTOCOL_VERSION" +
                    "_LONGHORN_SERVER,out _,out _)\'");
            temp27 = this.IFRS2ManagedAdapterInstance.EstablishConnection("C", 3, FRS2Model.ProtocolVersion.FRS_COMMUNICATION_PROTOCOL_VERSION_LONGHORN_SERVER, out temp25, out temp26);
            this.Manager.Checkpoint("MS-FRS2_R37");
            this.Manager.Checkpoint("MS-FRS2_R436");
            this.Manager.Checkpoint("MS-FRS2_R439");
            this.Manager.Comment("reaching state \'S144\'");
            this.Manager.Comment("checking step \'return EstablishConnection/[out Valid,out Valid]:ERROR_SUCCESS\'");
            this.Manager.Assert((temp25 == FRS2Model.ProtocolVersionReturned.Valid), String.Format("expected \'FRS2Model.ProtocolVersionReturned.Valid\', actual \'{0}\' (upstreamProtoco" +
                        "lVersion of EstablishConnection, state S144)", TestManagerHelpers.Describe(temp25)));
            this.Manager.Assert((temp26 == FRS2Model.UpstreamFlagValueReturned.Valid), String.Format("expected \'FRS2Model.UpstreamFlagValueReturned.Valid\', actual \'{0}\' (upstreamFlags" +
                        " of EstablishConnection, state S144)", TestManagerHelpers.Describe(temp26)));
            this.Manager.Assert((temp27 == FRS2Model.error_status_t.ERROR_SUCCESS), String.Format("expected \'FRS2Model.error_status_t.ERROR_SUCCESS\', actual \'{0}\' (return of Establ" +
                        "ishConnection, state S144)", TestManagerHelpers.Describe(temp27)));
            this.Manager.Comment("reaching state \'S172\'");
            FRS2Model.error_status_t temp28;
            this.Manager.Comment("executing step \'call EstablishSession(1,2)\'");
            temp28 = this.IFRS2ManagedAdapterInstance.EstablishSession(1, 2);
            this.Manager.Comment("reaching state \'S199\'");
            this.Manager.Comment("checking step \'return EstablishSession/ERROR_FAIL\'");
            this.Manager.Assert((temp28 == FRS2Model.error_status_t.ERROR_FAIL), String.Format("expected \'FRS2Model.error_status_t.ERROR_FAIL\', actual \'{0}\' (return of Establish" +
                        "Session, state S199)", TestManagerHelpers.Describe(temp28)));
            TCtestScenario7S217();
            this.Manager.EndTest();
        }
        #endregion
        
        #region Test Starting in S10
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        [Microsoft.VisualStudio.TestTools.UnitTesting.DescriptionAttribute("MS-FRS2_R408, MS-FRS2_R413, MS-FRS2_R415, MS-FRS2_R414, MS-FRS2_R37, MS-FRS2_R436" +
            ", MS-FRS2_R439, MS-FRS2_R455, MS-FRS2_R458, MS-FRS2_R448")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestCategory("AD")]
        [TestCategory("MS-FRS2")]
        [TestCategory("SDC")]
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        
        public virtual void FRS2_TCtestScenario7S10()
        {
            this.Manager.BeginTest("TCtestScenario7S10");
            this.Manager.Comment("reaching state \'S10\'");
            FRS2Model.error_status_t temp29;
            this.Manager.Comment(@"executing step 'call Initialization(Windows7,Map{1=>enabled,2=>enabled,3=>enabled,4=>disabled},Map{1=>exists,2=>exists,3=>exists,4=>exists},Map{""A""=>1,""B""=>2,""C""=>3,""D""=>4},Map{""A""=>sysvol,""B""=>protection,""C""=>sysvol,""D""=>sysvol},Map{1=>""A"",2=>""B"",3=>""C"",4=>""D""},Map{1=>writeOnly,2=>readWrite,3=>readOnly,4=>readWrite},Map{1=>enabled,2=>disabled,3=>enabled,4=>enabled})'");
            temp29 = this.IFRS2ManagedAdapterInstance.Initialization(FRS2Model.OSVersion.Windows7, new Microsoft.Modeling.Map<System.Int32,FRS2Model.connectionProperty>().Add(1, FRS2Model.connectionProperty.enabled).Add(2, FRS2Model.connectionProperty.enabled).Add(3, FRS2Model.connectionProperty.enabled).Add(4, FRS2Model.connectionProperty.disabled), new Microsoft.Modeling.Map<System.Int32,FRS2Model.FromServer>().Add(1, FRS2Model.FromServer.exists).Add(2, FRS2Model.FromServer.exists).Add(3, FRS2Model.FromServer.exists).Add(4, FRS2Model.FromServer.exists), new Microsoft.Modeling.Map<System.String,System.Int32>().Add("A", 1).Add("B", 2).Add("C", 3).Add("D", 4), new Microsoft.Modeling.Map<System.String,FRS2Model.ReplicationGroupTypes>().Add("A", FRS2Model.ReplicationGroupTypes.sysvol).Add("B", FRS2Model.ReplicationGroupTypes.protection).Add("C", FRS2Model.ReplicationGroupTypes.sysvol).Add("D", FRS2Model.ReplicationGroupTypes.sysvol), new Microsoft.Modeling.Map<System.Int32,System.String>().Add(1, "A").Add(2, "B").Add(3, "C").Add(4, "D"), new Microsoft.Modeling.Map<System.Int32,FRS2Model.accessLevels>().Add(1, FRS2Model.accessLevels.writeOnly).Add(2, FRS2Model.accessLevels.readWrite).Add(3, FRS2Model.accessLevels.readOnly).Add(4, FRS2Model.accessLevels.readWrite), new Microsoft.Modeling.Map<System.Int32,FRS2Model.connectionProperty>().Add(1, FRS2Model.connectionProperty.enabled).Add(2, FRS2Model.connectionProperty.disabled).Add(3, FRS2Model.connectionProperty.enabled).Add(4, FRS2Model.connectionProperty.enabled));
            this.Manager.Comment("reaching state \'S11\'");
            this.Manager.Comment("checking step \'return Initialization/ERROR_SUCCESS\'");
            this.Manager.Assert((temp29 == FRS2Model.error_status_t.ERROR_SUCCESS), String.Format("expected \'FRS2Model.error_status_t.ERROR_SUCCESS\', actual \'{0}\' (return of Initia" +
                        "lization, state S11)", TestManagerHelpers.Describe(temp29)));
            this.Manager.Comment("reaching state \'S61\'");
            FRS2Model.error_status_t temp30;
            this.Manager.Comment("executing step \'call CheckConnectivity(\"D\",4)\'");
            temp30 = this.IFRS2ManagedAdapterInstance.CheckConnectivity("D", 4);
            this.Manager.Checkpoint("MS-FRS2_R408");
            this.Manager.Checkpoint("MS-FRS2_R413");
            this.Manager.Checkpoint("MS-FRS2_R415");
            this.Manager.Checkpoint("MS-FRS2_R414");
            this.Manager.Comment("reaching state \'S89\'");
            this.Manager.Comment("checking step \'return CheckConnectivity/ERROR_FAIL\'");
            this.Manager.Assert((temp30 == FRS2Model.error_status_t.ERROR_FAIL), String.Format("expected \'FRS2Model.error_status_t.ERROR_FAIL\', actual \'{0}\' (return of CheckConn" +
                        "ectivity, state S89)", TestManagerHelpers.Describe(temp30)));
            this.Manager.Comment("reaching state \'S117\'");
            FRS2Model.ProtocolVersionReturned temp31;
            FRS2Model.UpstreamFlagValueReturned temp32;
            FRS2Model.error_status_t temp33;
            this.Manager.Comment("executing step \'call EstablishConnection(\"A\",1,FRS_COMMUNICATION_PROTOCOL_VERSION" +
                    "_LONGHORN_SERVER,out _,out _)\'");
            temp33 = this.IFRS2ManagedAdapterInstance.EstablishConnection("A", 1, FRS2Model.ProtocolVersion.FRS_COMMUNICATION_PROTOCOL_VERSION_LONGHORN_SERVER, out temp31, out temp32);
            this.Manager.Checkpoint("MS-FRS2_R37");
            this.Manager.Checkpoint("MS-FRS2_R436");
            this.Manager.Checkpoint("MS-FRS2_R439");
            this.Manager.Comment("reaching state \'S145\'");
            this.Manager.Comment("checking step \'return EstablishConnection/[out Valid,out Valid]:ERROR_SUCCESS\'");
            this.Manager.Assert((temp31 == FRS2Model.ProtocolVersionReturned.Valid), String.Format("expected \'FRS2Model.ProtocolVersionReturned.Valid\', actual \'{0}\' (upstreamProtoco" +
                        "lVersion of EstablishConnection, state S145)", TestManagerHelpers.Describe(temp31)));
            this.Manager.Assert((temp32 == FRS2Model.UpstreamFlagValueReturned.Valid), String.Format("expected \'FRS2Model.UpstreamFlagValueReturned.Valid\', actual \'{0}\' (upstreamFlags" +
                        " of EstablishConnection, state S145)", TestManagerHelpers.Describe(temp32)));
            this.Manager.Assert((temp33 == FRS2Model.error_status_t.ERROR_SUCCESS), String.Format("expected \'FRS2Model.error_status_t.ERROR_SUCCESS\', actual \'{0}\' (return of Establ" +
                        "ishConnection, state S145)", TestManagerHelpers.Describe(temp33)));
            this.Manager.Comment("reaching state \'S173\'");
            FRS2Model.error_status_t temp34;
            this.Manager.Comment("executing step \'call EstablishSession(1,1)\'");
            temp34 = this.IFRS2ManagedAdapterInstance.EstablishSession(1, 1);
            this.Manager.Checkpoint("MS-FRS2_R455");
            this.Manager.Checkpoint("MS-FRS2_R458");
            this.Manager.Checkpoint("MS-FRS2_R448");
            this.Manager.Comment("reaching state \'S200\'");
            this.Manager.Comment("checking step \'return EstablishSession/ERROR_SUCCESS\'");
            this.Manager.Assert((temp34 == FRS2Model.error_status_t.ERROR_SUCCESS), String.Format("expected \'FRS2Model.error_status_t.ERROR_SUCCESS\', actual \'{0}\' (return of Establ" +
                        "ishSession, state S200)", TestManagerHelpers.Describe(temp34)));
            this.Manager.Comment("reaching state \'S220\'");
            this.Manager.EndTest();
        }
        #endregion
        
        #region Test Starting in S12
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        [Microsoft.VisualStudio.TestTools.UnitTesting.DescriptionAttribute("MS-FRS2_R408, MS-FRS2_R413, MS-FRS2_R415, MS-FRS2_R414, MS-FRS2_R37, MS-FRS2_R436" +
            ", MS-FRS2_R439, MS-FRS2_R456, MS-FRS2_R459, MS-FRS2_R462")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestCategory("AD")]
        [TestCategory("MS-FRS2")]
        [TestCategory("SDC")]
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        
        public virtual void FRS2_TCtestScenario7S12()
        {
            this.Manager.BeginTest("TCtestScenario7S12");
            this.Manager.Comment("reaching state \'S12\'");
            FRS2Model.error_status_t temp35;
            this.Manager.Comment(@"executing step 'call Initialization(Windows7,Map{1=>enabled,2=>enabled,3=>enabled,4=>disabled},Map{1=>exists,2=>exists,3=>exists,4=>exists},Map{""A""=>1,""B""=>2,""C""=>3,""D""=>4},Map{""A""=>sysvol,""B""=>protection,""C""=>sysvol,""D""=>sysvol},Map{1=>""A"",2=>""B"",3=>""C"",4=>""D""},Map{1=>writeOnly,2=>readWrite,3=>readOnly,4=>readWrite},Map{1=>enabled,2=>disabled,3=>enabled,4=>enabled})'");
            temp35 = this.IFRS2ManagedAdapterInstance.Initialization(FRS2Model.OSVersion.Windows7, new Microsoft.Modeling.Map<System.Int32,FRS2Model.connectionProperty>().Add(1, FRS2Model.connectionProperty.enabled).Add(2, FRS2Model.connectionProperty.enabled).Add(3, FRS2Model.connectionProperty.enabled).Add(4, FRS2Model.connectionProperty.disabled), new Microsoft.Modeling.Map<System.Int32,FRS2Model.FromServer>().Add(1, FRS2Model.FromServer.exists).Add(2, FRS2Model.FromServer.exists).Add(3, FRS2Model.FromServer.exists).Add(4, FRS2Model.FromServer.exists), new Microsoft.Modeling.Map<System.String,System.Int32>().Add("A", 1).Add("B", 2).Add("C", 3).Add("D", 4), new Microsoft.Modeling.Map<System.String,FRS2Model.ReplicationGroupTypes>().Add("A", FRS2Model.ReplicationGroupTypes.sysvol).Add("B", FRS2Model.ReplicationGroupTypes.protection).Add("C", FRS2Model.ReplicationGroupTypes.sysvol).Add("D", FRS2Model.ReplicationGroupTypes.sysvol), new Microsoft.Modeling.Map<System.Int32,System.String>().Add(1, "A").Add(2, "B").Add(3, "C").Add(4, "D"), new Microsoft.Modeling.Map<System.Int32,FRS2Model.accessLevels>().Add(1, FRS2Model.accessLevels.writeOnly).Add(2, FRS2Model.accessLevels.readWrite).Add(3, FRS2Model.accessLevels.readOnly).Add(4, FRS2Model.accessLevels.readWrite), new Microsoft.Modeling.Map<System.Int32,FRS2Model.connectionProperty>().Add(1, FRS2Model.connectionProperty.enabled).Add(2, FRS2Model.connectionProperty.disabled).Add(3, FRS2Model.connectionProperty.enabled).Add(4, FRS2Model.connectionProperty.enabled));
            this.Manager.Comment("reaching state \'S13\'");
            this.Manager.Comment("checking step \'return Initialization/ERROR_SUCCESS\'");
            this.Manager.Assert((temp35 == FRS2Model.error_status_t.ERROR_SUCCESS), String.Format("expected \'FRS2Model.error_status_t.ERROR_SUCCESS\', actual \'{0}\' (return of Initia" +
                        "lization, state S13)", TestManagerHelpers.Describe(temp35)));
            this.Manager.Comment("reaching state \'S62\'");
            FRS2Model.error_status_t temp36;
            this.Manager.Comment("executing step \'call CheckConnectivity(\"D\",4)\'");
            temp36 = this.IFRS2ManagedAdapterInstance.CheckConnectivity("D", 4);
            this.Manager.Checkpoint("MS-FRS2_R408");
            this.Manager.Checkpoint("MS-FRS2_R413");
            this.Manager.Checkpoint("MS-FRS2_R415");
            this.Manager.Checkpoint("MS-FRS2_R414");
            this.Manager.Comment("reaching state \'S90\'");
            this.Manager.Comment("checking step \'return CheckConnectivity/ERROR_FAIL\'");
            this.Manager.Assert((temp36 == FRS2Model.error_status_t.ERROR_FAIL), String.Format("expected \'FRS2Model.error_status_t.ERROR_FAIL\', actual \'{0}\' (return of CheckConn" +
                        "ectivity, state S90)", TestManagerHelpers.Describe(temp36)));
            this.Manager.Comment("reaching state \'S118\'");
            FRS2Model.ProtocolVersionReturned temp37;
            FRS2Model.UpstreamFlagValueReturned temp38;
            FRS2Model.error_status_t temp39;
            this.Manager.Comment("executing step \'call EstablishConnection(\"A\",1,FRS_COMMUNICATION_PROTOCOL_VERSION" +
                    "_LONGHORN_SERVER,out _,out _)\'");
            temp39 = this.IFRS2ManagedAdapterInstance.EstablishConnection("A", 1, FRS2Model.ProtocolVersion.FRS_COMMUNICATION_PROTOCOL_VERSION_LONGHORN_SERVER, out temp37, out temp38);
            this.Manager.Checkpoint("MS-FRS2_R37");
            this.Manager.Checkpoint("MS-FRS2_R436");
            this.Manager.Checkpoint("MS-FRS2_R439");
            this.Manager.Comment("reaching state \'S146\'");
            this.Manager.Comment("checking step \'return EstablishConnection/[out Valid,out Valid]:ERROR_SUCCESS\'");
            this.Manager.Assert((temp37 == FRS2Model.ProtocolVersionReturned.Valid), String.Format("expected \'FRS2Model.ProtocolVersionReturned.Valid\', actual \'{0}\' (upstreamProtoco" +
                        "lVersion of EstablishConnection, state S146)", TestManagerHelpers.Describe(temp37)));
            this.Manager.Assert((temp38 == FRS2Model.UpstreamFlagValueReturned.Valid), String.Format("expected \'FRS2Model.UpstreamFlagValueReturned.Valid\', actual \'{0}\' (upstreamFlags" +
                        " of EstablishConnection, state S146)", TestManagerHelpers.Describe(temp38)));
            this.Manager.Assert((temp39 == FRS2Model.error_status_t.ERROR_SUCCESS), String.Format("expected \'FRS2Model.error_status_t.ERROR_SUCCESS\', actual \'{0}\' (return of Establ" +
                        "ishConnection, state S146)", TestManagerHelpers.Describe(temp39)));
            this.Manager.Comment("reaching state \'S174\'");
            FRS2Model.error_status_t temp40;
            this.Manager.Comment("executing step \'call EstablishSession(4,4)\'");
            temp40 = this.IFRS2ManagedAdapterInstance.EstablishSession(4, 4);
            this.Manager.Checkpoint("MS-FRS2_R456");
            this.Manager.Checkpoint("MS-FRS2_R459");
            this.Manager.Checkpoint("MS-FRS2_R462");
            this.Manager.Comment("reaching state \'S201\'");
            this.Manager.Comment("checking step \'return EstablishSession/FRS_ERROR_CONNECTION_INVALID\'");
            this.Manager.Assert((temp40 == FRS2Model.error_status_t.FRS_ERROR_CONNECTION_INVALID), String.Format("expected \'FRS2Model.error_status_t.FRS_ERROR_CONNECTION_INVALID\', actual \'{0}\' (r" +
                        "eturn of EstablishSession, state S201)", TestManagerHelpers.Describe(temp40)));
            TCtestScenario7S221();
            this.Manager.EndTest();
        }
        
        private void TCtestScenario7S221() {
            this.Manager.Comment("reaching state \'S221\'");
        }
        #endregion
        
        #region Test Starting in S14
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        [Microsoft.VisualStudio.TestTools.UnitTesting.DescriptionAttribute("MS-FRS2_R408, MS-FRS2_R413, MS-FRS2_R415, MS-FRS2_R414, MS-FRS2_R37, MS-FRS2_R436" +
            ", MS-FRS2_R439")]
        public virtual void FRS2_TCtestScenario7S14()
        {
            this.Manager.BeginTest("TCtestScenario7S14");
            this.Manager.Comment("reaching state \'S14\'");
            FRS2Model.error_status_t temp41;
            this.Manager.Comment(@"executing step 'call Initialization(Windows7,Map{1=>enabled,2=>enabled,3=>enabled,4=>disabled},Map{1=>exists,2=>exists,3=>exists,4=>exists},Map{""A""=>1,""B""=>2,""C""=>3,""D""=>4},Map{""A""=>sysvol,""B""=>protection,""C""=>sysvol,""D""=>sysvol},Map{1=>""A"",2=>""B"",3=>""C"",4=>""D""},Map{1=>writeOnly,2=>readWrite,3=>readOnly,4=>readWrite},Map{1=>enabled,2=>disabled,3=>enabled,4=>enabled})'");
            temp41 = this.IFRS2ManagedAdapterInstance.Initialization(FRS2Model.OSVersion.Windows7, new Microsoft.Modeling.Map<System.Int32,FRS2Model.connectionProperty>().Add(1, FRS2Model.connectionProperty.enabled).Add(2, FRS2Model.connectionProperty.enabled).Add(3, FRS2Model.connectionProperty.enabled).Add(4, FRS2Model.connectionProperty.disabled), new Microsoft.Modeling.Map<System.Int32,FRS2Model.FromServer>().Add(1, FRS2Model.FromServer.exists).Add(2, FRS2Model.FromServer.exists).Add(3, FRS2Model.FromServer.exists).Add(4, FRS2Model.FromServer.exists), new Microsoft.Modeling.Map<System.String,System.Int32>().Add("A", 1).Add("B", 2).Add("C", 3).Add("D", 4), new Microsoft.Modeling.Map<System.String,FRS2Model.ReplicationGroupTypes>().Add("A", FRS2Model.ReplicationGroupTypes.sysvol).Add("B", FRS2Model.ReplicationGroupTypes.protection).Add("C", FRS2Model.ReplicationGroupTypes.sysvol).Add("D", FRS2Model.ReplicationGroupTypes.sysvol), new Microsoft.Modeling.Map<System.Int32,System.String>().Add(1, "A").Add(2, "B").Add(3, "C").Add(4, "D"), new Microsoft.Modeling.Map<System.Int32,FRS2Model.accessLevels>().Add(1, FRS2Model.accessLevels.writeOnly).Add(2, FRS2Model.accessLevels.readWrite).Add(3, FRS2Model.accessLevels.readOnly).Add(4, FRS2Model.accessLevels.readWrite), new Microsoft.Modeling.Map<System.Int32,FRS2Model.connectionProperty>().Add(1, FRS2Model.connectionProperty.enabled).Add(2, FRS2Model.connectionProperty.disabled).Add(3, FRS2Model.connectionProperty.enabled).Add(4, FRS2Model.connectionProperty.enabled));
            this.Manager.Comment("reaching state \'S15\'");
            this.Manager.Comment("checking step \'return Initialization/ERROR_SUCCESS\'");
            this.Manager.Assert((temp41 == FRS2Model.error_status_t.ERROR_SUCCESS), String.Format("expected \'FRS2Model.error_status_t.ERROR_SUCCESS\', actual \'{0}\' (return of Initia" +
                        "lization, state S15)", TestManagerHelpers.Describe(temp41)));
            this.Manager.Comment("reaching state \'S63\'");
            FRS2Model.error_status_t temp42;
            this.Manager.Comment("executing step \'call CheckConnectivity(\"D\",4)\'");
            temp42 = this.IFRS2ManagedAdapterInstance.CheckConnectivity("D", 4);
            this.Manager.Checkpoint("MS-FRS2_R408");
            this.Manager.Checkpoint("MS-FRS2_R413");
            this.Manager.Checkpoint("MS-FRS2_R415");
            this.Manager.Checkpoint("MS-FRS2_R414");
            this.Manager.Comment("reaching state \'S91\'");
            this.Manager.Comment("checking step \'return CheckConnectivity/ERROR_FAIL\'");
            this.Manager.Assert((temp42 == FRS2Model.error_status_t.ERROR_FAIL), String.Format("expected \'FRS2Model.error_status_t.ERROR_FAIL\', actual \'{0}\' (return of CheckConn" +
                        "ectivity, state S91)", TestManagerHelpers.Describe(temp42)));
            this.Manager.Comment("reaching state \'S119\'");
            FRS2Model.ProtocolVersionReturned temp43;
            FRS2Model.UpstreamFlagValueReturned temp44;
            FRS2Model.error_status_t temp45;
            this.Manager.Comment("executing step \'call EstablishConnection(\"A\",1,FRS_COMMUNICATION_PROTOCOL_VERSION" +
                    "_LONGHORN_SERVER,out _,out _)\'");
            temp45 = this.IFRS2ManagedAdapterInstance.EstablishConnection("A", 1, FRS2Model.ProtocolVersion.FRS_COMMUNICATION_PROTOCOL_VERSION_LONGHORN_SERVER, out temp43, out temp44);
            this.Manager.Checkpoint("MS-FRS2_R37");
            this.Manager.Checkpoint("MS-FRS2_R436");
            this.Manager.Checkpoint("MS-FRS2_R439");
            this.Manager.Comment("reaching state \'S147\'");
            this.Manager.Comment("checking step \'return EstablishConnection/[out Valid,out Valid]:ERROR_SUCCESS\'");
            this.Manager.Assert((temp43 == FRS2Model.ProtocolVersionReturned.Valid), String.Format("expected \'FRS2Model.ProtocolVersionReturned.Valid\', actual \'{0}\' (upstreamProtoco" +
                        "lVersion of EstablishConnection, state S147)", TestManagerHelpers.Describe(temp43)));
            this.Manager.Assert((temp44 == FRS2Model.UpstreamFlagValueReturned.Valid), String.Format("expected \'FRS2Model.UpstreamFlagValueReturned.Valid\', actual \'{0}\' (upstreamFlags" +
                        " of EstablishConnection, state S147)", TestManagerHelpers.Describe(temp44)));
            this.Manager.Assert((temp45 == FRS2Model.error_status_t.ERROR_SUCCESS), String.Format("expected \'FRS2Model.error_status_t.ERROR_SUCCESS\', actual \'{0}\' (return of Establ" +
                        "ishConnection, state S147)", TestManagerHelpers.Describe(temp45)));
            this.Manager.Comment("reaching state \'S175\'");
            FRS2Model.error_status_t temp46;
            this.Manager.Comment("executing step \'call EstablishSession(1,2)\'");
            temp46 = this.IFRS2ManagedAdapterInstance.EstablishSession(1, 2);
            this.Manager.Comment("reaching state \'S202\'");
            this.Manager.Comment("checking step \'return EstablishSession/ERROR_FAIL\'");
            this.Manager.Assert((temp46 == FRS2Model.error_status_t.ERROR_FAIL), String.Format("expected \'FRS2Model.error_status_t.ERROR_FAIL\', actual \'{0}\' (return of Establish" +
                        "Session, state S202)", TestManagerHelpers.Describe(temp46)));
            TCtestScenario7S221();
            this.Manager.EndTest();
        }
        #endregion
        
        #region Test Starting in S16
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        [Microsoft.VisualStudio.TestTools.UnitTesting.DescriptionAttribute("MS-FRS2_R408, MS-FRS2_R413, MS-FRS2_R415, MS-FRS2_R414, MS-FRS2_R37, MS-FRS2_R436" +
            ", MS-FRS2_R439")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestCategory("AD")]
        [TestCategory("MS-FRS2")]
        [TestCategory("SDC")]
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        
        public virtual void FRS2_TCtestScenario7S16()
        {
            this.Manager.BeginTest("TCtestScenario7S16");
            this.Manager.Comment("reaching state \'S16\'");
            FRS2Model.error_status_t temp47;
            this.Manager.Comment(@"executing step 'call Initialization(Windows7,Map{1=>enabled,2=>enabled,3=>enabled,4=>disabled},Map{1=>exists,2=>exists,3=>exists,4=>exists},Map{""A""=>1,""B""=>2,""C""=>3,""D""=>4},Map{""A""=>sysvol,""B""=>protection,""C""=>sysvol,""D""=>sysvol},Map{1=>""A"",2=>""B"",3=>""C"",4=>""D""},Map{1=>writeOnly,2=>readWrite,3=>readOnly,4=>readWrite},Map{1=>enabled,2=>disabled,3=>enabled,4=>enabled})'");
            temp47 = this.IFRS2ManagedAdapterInstance.Initialization(FRS2Model.OSVersion.Windows7, new Microsoft.Modeling.Map<System.Int32,FRS2Model.connectionProperty>().Add(1, FRS2Model.connectionProperty.enabled).Add(2, FRS2Model.connectionProperty.enabled).Add(3, FRS2Model.connectionProperty.enabled).Add(4, FRS2Model.connectionProperty.disabled), new Microsoft.Modeling.Map<System.Int32,FRS2Model.FromServer>().Add(1, FRS2Model.FromServer.exists).Add(2, FRS2Model.FromServer.exists).Add(3, FRS2Model.FromServer.exists).Add(4, FRS2Model.FromServer.exists), new Microsoft.Modeling.Map<System.String,System.Int32>().Add("A", 1).Add("B", 2).Add("C", 3).Add("D", 4), new Microsoft.Modeling.Map<System.String,FRS2Model.ReplicationGroupTypes>().Add("A", FRS2Model.ReplicationGroupTypes.sysvol).Add("B", FRS2Model.ReplicationGroupTypes.protection).Add("C", FRS2Model.ReplicationGroupTypes.sysvol).Add("D", FRS2Model.ReplicationGroupTypes.sysvol), new Microsoft.Modeling.Map<System.Int32,System.String>().Add(1, "A").Add(2, "B").Add(3, "C").Add(4, "D"), new Microsoft.Modeling.Map<System.Int32,FRS2Model.accessLevels>().Add(1, FRS2Model.accessLevels.writeOnly).Add(2, FRS2Model.accessLevels.readWrite).Add(3, FRS2Model.accessLevels.readOnly).Add(4, FRS2Model.accessLevels.readWrite), new Microsoft.Modeling.Map<System.Int32,FRS2Model.connectionProperty>().Add(1, FRS2Model.connectionProperty.enabled).Add(2, FRS2Model.connectionProperty.disabled).Add(3, FRS2Model.connectionProperty.enabled).Add(4, FRS2Model.connectionProperty.enabled));
            this.Manager.Comment("reaching state \'S17\'");
            this.Manager.Comment("checking step \'return Initialization/ERROR_SUCCESS\'");
            this.Manager.Assert((temp47 == FRS2Model.error_status_t.ERROR_SUCCESS), String.Format("expected \'FRS2Model.error_status_t.ERROR_SUCCESS\', actual \'{0}\' (return of Initia" +
                        "lization, state S17)", TestManagerHelpers.Describe(temp47)));
            this.Manager.Comment("reaching state \'S64\'");
            FRS2Model.error_status_t temp48;
            this.Manager.Comment("executing step \'call CheckConnectivity(\"D\",4)\'");
            temp48 = this.IFRS2ManagedAdapterInstance.CheckConnectivity("D", 4);
            this.Manager.Checkpoint("MS-FRS2_R408");
            this.Manager.Checkpoint("MS-FRS2_R413");
            this.Manager.Checkpoint("MS-FRS2_R415");
            this.Manager.Checkpoint("MS-FRS2_R414");
            this.Manager.Comment("reaching state \'S92\'");
            this.Manager.Comment("checking step \'return CheckConnectivity/ERROR_FAIL\'");
            this.Manager.Assert((temp48 == FRS2Model.error_status_t.ERROR_FAIL), String.Format("expected \'FRS2Model.error_status_t.ERROR_FAIL\', actual \'{0}\' (return of CheckConn" +
                        "ectivity, state S92)", TestManagerHelpers.Describe(temp48)));
            this.Manager.Comment("reaching state \'S120\'");
            FRS2Model.ProtocolVersionReturned temp49;
            FRS2Model.UpstreamFlagValueReturned temp50;
            FRS2Model.error_status_t temp51;
            this.Manager.Comment("executing step \'call EstablishConnection(\"A\",1,FRS_COMMUNICATION_PROTOCOL_VERSION" +
                    "_LONGHORN_SERVER,out _,out _)\'");
            temp51 = this.IFRS2ManagedAdapterInstance.EstablishConnection("A", 1, FRS2Model.ProtocolVersion.FRS_COMMUNICATION_PROTOCOL_VERSION_LONGHORN_SERVER, out temp49, out temp50);
            this.Manager.Checkpoint("MS-FRS2_R37");
            this.Manager.Checkpoint("MS-FRS2_R436");
            this.Manager.Checkpoint("MS-FRS2_R439");
            this.Manager.Comment("reaching state \'S148\'");
            this.Manager.Comment("checking step \'return EstablishConnection/[out Valid,out Valid]:ERROR_SUCCESS\'");
            this.Manager.Assert((temp49 == FRS2Model.ProtocolVersionReturned.Valid), String.Format("expected \'FRS2Model.ProtocolVersionReturned.Valid\', actual \'{0}\' (upstreamProtoco" +
                        "lVersion of EstablishConnection, state S148)", TestManagerHelpers.Describe(temp49)));
            this.Manager.Assert((temp50 == FRS2Model.UpstreamFlagValueReturned.Valid), String.Format("expected \'FRS2Model.UpstreamFlagValueReturned.Valid\', actual \'{0}\' (upstreamFlags" +
                        " of EstablishConnection, state S148)", TestManagerHelpers.Describe(temp50)));
            this.Manager.Assert((temp51 == FRS2Model.error_status_t.ERROR_SUCCESS), String.Format("expected \'FRS2Model.error_status_t.ERROR_SUCCESS\', actual \'{0}\' (return of Establ" +
                        "ishConnection, state S148)", TestManagerHelpers.Describe(temp51)));
            this.Manager.Comment("reaching state \'S176\'");
            FRS2Model.error_status_t temp52;
            this.Manager.Comment("executing step \'call EstablishSession(2,2)\'");
            temp52 = this.IFRS2ManagedAdapterInstance.EstablishSession(2, 2);
            this.Manager.Comment("reaching state \'S203\'");
            this.Manager.Comment("checking step \'return EstablishSession/FRS_ERROR_CONNECTION_INVALID\'");
            this.Manager.Assert((temp52 == FRS2Model.error_status_t.FRS_ERROR_CONNECTION_INVALID), String.Format("expected \'FRS2Model.error_status_t.FRS_ERROR_CONNECTION_INVALID\', actual \'{0}\' (r" +
                        "eturn of EstablishSession, state S203)", TestManagerHelpers.Describe(temp52)));
            TCtestScenario7S221();
            this.Manager.EndTest();
        }
        #endregion
        
        #region Test Starting in S18
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        [Microsoft.VisualStudio.TestTools.UnitTesting.DescriptionAttribute("MS-FRS2_R407, MS-FRS2_R410, MS-FRS2_R417, MS-FRS2_R37, MS-FRS2_R436, MS-FRS2_R439" +
            ", MS-FRS2_R455, MS-FRS2_R458, MS-FRS2_R448")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestCategory("AD")]
        [TestCategory("MS-FRS2")]
        [TestCategory("SDC")]
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        
        public virtual void FRS2_TCtestScenario7S18()
        {
            this.Manager.BeginTest("TCtestScenario7S18");
            this.Manager.Comment("reaching state \'S18\'");
            FRS2Model.error_status_t temp53;
            this.Manager.Comment(@"executing step 'call Initialization(Windows7,Map{1=>enabled,2=>enabled,3=>enabled,4=>disabled},Map{1=>exists,2=>exists,3=>exists,4=>exists},Map{""A""=>1,""B""=>2,""C""=>3,""D""=>4},Map{""A""=>sysvol,""B""=>protection,""C""=>sysvol,""D""=>sysvol},Map{1=>""A"",2=>""B"",3=>""C"",4=>""D""},Map{1=>writeOnly,2=>readWrite,3=>readOnly,4=>readWrite},Map{1=>enabled,2=>disabled,3=>enabled,4=>enabled})'");
            temp53 = this.IFRS2ManagedAdapterInstance.Initialization(FRS2Model.OSVersion.Windows7, new Microsoft.Modeling.Map<System.Int32,FRS2Model.connectionProperty>().Add(1, FRS2Model.connectionProperty.enabled).Add(2, FRS2Model.connectionProperty.enabled).Add(3, FRS2Model.connectionProperty.enabled).Add(4, FRS2Model.connectionProperty.disabled), new Microsoft.Modeling.Map<System.Int32,FRS2Model.FromServer>().Add(1, FRS2Model.FromServer.exists).Add(2, FRS2Model.FromServer.exists).Add(3, FRS2Model.FromServer.exists).Add(4, FRS2Model.FromServer.exists), new Microsoft.Modeling.Map<System.String,System.Int32>().Add("A", 1).Add("B", 2).Add("C", 3).Add("D", 4), new Microsoft.Modeling.Map<System.String,FRS2Model.ReplicationGroupTypes>().Add("A", FRS2Model.ReplicationGroupTypes.sysvol).Add("B", FRS2Model.ReplicationGroupTypes.protection).Add("C", FRS2Model.ReplicationGroupTypes.sysvol).Add("D", FRS2Model.ReplicationGroupTypes.sysvol), new Microsoft.Modeling.Map<System.Int32,System.String>().Add(1, "A").Add(2, "B").Add(3, "C").Add(4, "D"), new Microsoft.Modeling.Map<System.Int32,FRS2Model.accessLevels>().Add(1, FRS2Model.accessLevels.writeOnly).Add(2, FRS2Model.accessLevels.readWrite).Add(3, FRS2Model.accessLevels.readOnly).Add(4, FRS2Model.accessLevels.readWrite), new Microsoft.Modeling.Map<System.Int32,FRS2Model.connectionProperty>().Add(1, FRS2Model.connectionProperty.enabled).Add(2, FRS2Model.connectionProperty.disabled).Add(3, FRS2Model.connectionProperty.enabled).Add(4, FRS2Model.connectionProperty.enabled));
            this.Manager.Comment("reaching state \'S19\'");
            this.Manager.Comment("checking step \'return Initialization/ERROR_SUCCESS\'");
            this.Manager.Assert((temp53 == FRS2Model.error_status_t.ERROR_SUCCESS), String.Format("expected \'FRS2Model.error_status_t.ERROR_SUCCESS\', actual \'{0}\' (return of Initia" +
                        "lization, state S19)", TestManagerHelpers.Describe(temp53)));
            this.Manager.Comment("reaching state \'S65\'");
            FRS2Model.error_status_t temp54;
            this.Manager.Comment("executing step \'call CheckConnectivity(\"A\",1)\'");
            temp54 = this.IFRS2ManagedAdapterInstance.CheckConnectivity("A", 1);
            this.Manager.Checkpoint("MS-FRS2_R407");
            this.Manager.Checkpoint("MS-FRS2_R410");
            this.Manager.Checkpoint("MS-FRS2_R417");
            this.Manager.Comment("reaching state \'S93\'");
            this.Manager.Comment("checking step \'return CheckConnectivity/ERROR_SUCCESS\'");
            this.Manager.Assert((temp54 == FRS2Model.error_status_t.ERROR_SUCCESS), String.Format("expected \'FRS2Model.error_status_t.ERROR_SUCCESS\', actual \'{0}\' (return of CheckC" +
                        "onnectivity, state S93)", TestManagerHelpers.Describe(temp54)));
            this.Manager.Comment("reaching state \'S121\'");
            FRS2Model.ProtocolVersionReturned temp55;
            FRS2Model.UpstreamFlagValueReturned temp56;
            FRS2Model.error_status_t temp57;
            this.Manager.Comment("executing step \'call EstablishConnection(\"C\",3,FRS_COMMUNICATION_PROTOCOL_VERSION" +
                    "_LONGHORN_SERVER,out _,out _)\'");
            temp57 = this.IFRS2ManagedAdapterInstance.EstablishConnection("C", 3, FRS2Model.ProtocolVersion.FRS_COMMUNICATION_PROTOCOL_VERSION_LONGHORN_SERVER, out temp55, out temp56);
            this.Manager.Checkpoint("MS-FRS2_R37");
            this.Manager.Checkpoint("MS-FRS2_R436");
            this.Manager.Checkpoint("MS-FRS2_R439");
            this.Manager.Comment("reaching state \'S149\'");
            this.Manager.Comment("checking step \'return EstablishConnection/[out Valid,out Valid]:ERROR_SUCCESS\'");
            this.Manager.Assert((temp55 == FRS2Model.ProtocolVersionReturned.Valid), String.Format("expected \'FRS2Model.ProtocolVersionReturned.Valid\', actual \'{0}\' (upstreamProtoco" +
                        "lVersion of EstablishConnection, state S149)", TestManagerHelpers.Describe(temp55)));
            this.Manager.Assert((temp56 == FRS2Model.UpstreamFlagValueReturned.Valid), String.Format("expected \'FRS2Model.UpstreamFlagValueReturned.Valid\', actual \'{0}\' (upstreamFlags" +
                        " of EstablishConnection, state S149)", TestManagerHelpers.Describe(temp56)));
            this.Manager.Assert((temp57 == FRS2Model.error_status_t.ERROR_SUCCESS), String.Format("expected \'FRS2Model.error_status_t.ERROR_SUCCESS\', actual \'{0}\' (return of Establ" +
                        "ishConnection, state S149)", TestManagerHelpers.Describe(temp57)));
            this.Manager.Comment("reaching state \'S177\'");
            FRS2Model.error_status_t temp58;
            this.Manager.Comment("executing step \'call EstablishSession(1,1)\'");
            temp58 = this.IFRS2ManagedAdapterInstance.EstablishSession(1, 1);
            this.Manager.Checkpoint("MS-FRS2_R455");
            this.Manager.Checkpoint("MS-FRS2_R458");
            this.Manager.Checkpoint("MS-FRS2_R448");
            this.Manager.Comment("reaching state \'S204\'");
            this.Manager.Comment("checking step \'return EstablishSession/ERROR_SUCCESS\'");
            this.Manager.Assert((temp58 == FRS2Model.error_status_t.ERROR_SUCCESS), String.Format("expected \'FRS2Model.error_status_t.ERROR_SUCCESS\', actual \'{0}\' (return of Establ" +
                        "ishSession, state S204)", TestManagerHelpers.Describe(temp58)));
            this.Manager.Comment("reaching state \'S224\'");
            this.Manager.EndTest();
        }
        #endregion
        
        #region Test Starting in S20
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        [Microsoft.VisualStudio.TestTools.UnitTesting.DescriptionAttribute("MS-FRS2_R407, MS-FRS2_R410, MS-FRS2_R417, MS-FRS2_R37, MS-FRS2_R436, MS-FRS2_R439" +
            ", MS-FRS2_R456, MS-FRS2_R459, MS-FRS2_R462")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestCategory("AD")]
        [TestCategory("MS-FRS2")]
        [TestCategory("SDC")]
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        
        public virtual void FRS2_TCtestScenario7S20()
        {
            this.Manager.BeginTest("TCtestScenario7S20");
            this.Manager.Comment("reaching state \'S20\'");
            FRS2Model.error_status_t temp59;
            this.Manager.Comment(@"executing step 'call Initialization(Windows7,Map{1=>enabled,2=>enabled,3=>enabled,4=>disabled},Map{1=>exists,2=>exists,3=>exists,4=>exists},Map{""A""=>1,""B""=>2,""C""=>3,""D""=>4},Map{""A""=>sysvol,""B""=>protection,""C""=>sysvol,""D""=>sysvol},Map{1=>""A"",2=>""B"",3=>""C"",4=>""D""},Map{1=>writeOnly,2=>readWrite,3=>readOnly,4=>readWrite},Map{1=>enabled,2=>disabled,3=>enabled,4=>enabled})'");
            temp59 = this.IFRS2ManagedAdapterInstance.Initialization(FRS2Model.OSVersion.Windows7, new Microsoft.Modeling.Map<System.Int32,FRS2Model.connectionProperty>().Add(1, FRS2Model.connectionProperty.enabled).Add(2, FRS2Model.connectionProperty.enabled).Add(3, FRS2Model.connectionProperty.enabled).Add(4, FRS2Model.connectionProperty.disabled), new Microsoft.Modeling.Map<System.Int32,FRS2Model.FromServer>().Add(1, FRS2Model.FromServer.exists).Add(2, FRS2Model.FromServer.exists).Add(3, FRS2Model.FromServer.exists).Add(4, FRS2Model.FromServer.exists), new Microsoft.Modeling.Map<System.String,System.Int32>().Add("A", 1).Add("B", 2).Add("C", 3).Add("D", 4), new Microsoft.Modeling.Map<System.String,FRS2Model.ReplicationGroupTypes>().Add("A", FRS2Model.ReplicationGroupTypes.sysvol).Add("B", FRS2Model.ReplicationGroupTypes.protection).Add("C", FRS2Model.ReplicationGroupTypes.sysvol).Add("D", FRS2Model.ReplicationGroupTypes.sysvol), new Microsoft.Modeling.Map<System.Int32,System.String>().Add(1, "A").Add(2, "B").Add(3, "C").Add(4, "D"), new Microsoft.Modeling.Map<System.Int32,FRS2Model.accessLevels>().Add(1, FRS2Model.accessLevels.writeOnly).Add(2, FRS2Model.accessLevels.readWrite).Add(3, FRS2Model.accessLevels.readOnly).Add(4, FRS2Model.accessLevels.readWrite), new Microsoft.Modeling.Map<System.Int32,FRS2Model.connectionProperty>().Add(1, FRS2Model.connectionProperty.enabled).Add(2, FRS2Model.connectionProperty.disabled).Add(3, FRS2Model.connectionProperty.enabled).Add(4, FRS2Model.connectionProperty.enabled));
            this.Manager.Comment("reaching state \'S21\'");
            this.Manager.Comment("checking step \'return Initialization/ERROR_SUCCESS\'");
            this.Manager.Assert((temp59 == FRS2Model.error_status_t.ERROR_SUCCESS), String.Format("expected \'FRS2Model.error_status_t.ERROR_SUCCESS\', actual \'{0}\' (return of Initia" +
                        "lization, state S21)", TestManagerHelpers.Describe(temp59)));
            this.Manager.Comment("reaching state \'S66\'");
            FRS2Model.error_status_t temp60;
            this.Manager.Comment("executing step \'call CheckConnectivity(\"A\",1)\'");
            temp60 = this.IFRS2ManagedAdapterInstance.CheckConnectivity("A", 1);
            this.Manager.Checkpoint("MS-FRS2_R407");
            this.Manager.Checkpoint("MS-FRS2_R410");
            this.Manager.Checkpoint("MS-FRS2_R417");
            this.Manager.Comment("reaching state \'S94\'");
            this.Manager.Comment("checking step \'return CheckConnectivity/ERROR_SUCCESS\'");
            this.Manager.Assert((temp60 == FRS2Model.error_status_t.ERROR_SUCCESS), String.Format("expected \'FRS2Model.error_status_t.ERROR_SUCCESS\', actual \'{0}\' (return of CheckC" +
                        "onnectivity, state S94)", TestManagerHelpers.Describe(temp60)));
            this.Manager.Comment("reaching state \'S122\'");
            FRS2Model.ProtocolVersionReturned temp61;
            FRS2Model.UpstreamFlagValueReturned temp62;
            FRS2Model.error_status_t temp63;
            this.Manager.Comment("executing step \'call EstablishConnection(\"C\",3,FRS_COMMUNICATION_PROTOCOL_VERSION" +
                    "_LONGHORN_SERVER,out _,out _)\'");
            temp63 = this.IFRS2ManagedAdapterInstance.EstablishConnection("C", 3, FRS2Model.ProtocolVersion.FRS_COMMUNICATION_PROTOCOL_VERSION_LONGHORN_SERVER, out temp61, out temp62);
            this.Manager.Checkpoint("MS-FRS2_R37");
            this.Manager.Checkpoint("MS-FRS2_R436");
            this.Manager.Checkpoint("MS-FRS2_R439");
            this.Manager.Comment("reaching state \'S150\'");
            this.Manager.Comment("checking step \'return EstablishConnection/[out Valid,out Valid]:ERROR_SUCCESS\'");
            this.Manager.Assert((temp61 == FRS2Model.ProtocolVersionReturned.Valid), String.Format("expected \'FRS2Model.ProtocolVersionReturned.Valid\', actual \'{0}\' (upstreamProtoco" +
                        "lVersion of EstablishConnection, state S150)", TestManagerHelpers.Describe(temp61)));
            this.Manager.Assert((temp62 == FRS2Model.UpstreamFlagValueReturned.Valid), String.Format("expected \'FRS2Model.UpstreamFlagValueReturned.Valid\', actual \'{0}\' (upstreamFlags" +
                        " of EstablishConnection, state S150)", TestManagerHelpers.Describe(temp62)));
            this.Manager.Assert((temp63 == FRS2Model.error_status_t.ERROR_SUCCESS), String.Format("expected \'FRS2Model.error_status_t.ERROR_SUCCESS\', actual \'{0}\' (return of Establ" +
                        "ishConnection, state S150)", TestManagerHelpers.Describe(temp63)));
            this.Manager.Comment("reaching state \'S178\'");
            FRS2Model.error_status_t temp64;
            this.Manager.Comment("executing step \'call EstablishSession(4,4)\'");
            temp64 = this.IFRS2ManagedAdapterInstance.EstablishSession(4, 4);
            this.Manager.Checkpoint("MS-FRS2_R456");
            this.Manager.Checkpoint("MS-FRS2_R459");
            this.Manager.Checkpoint("MS-FRS2_R462");
            this.Manager.Comment("reaching state \'S205\'");
            this.Manager.Comment("checking step \'return EstablishSession/FRS_ERROR_CONNECTION_INVALID\'");
            this.Manager.Assert((temp64 == FRS2Model.error_status_t.FRS_ERROR_CONNECTION_INVALID), String.Format("expected \'FRS2Model.error_status_t.FRS_ERROR_CONNECTION_INVALID\', actual \'{0}\' (r" +
                        "eturn of EstablishSession, state S205)", TestManagerHelpers.Describe(temp64)));
            TCtestScenario7S225();
            this.Manager.EndTest();
        }
        
        private void TCtestScenario7S225() {
            this.Manager.Comment("reaching state \'S225\'");
        }
        #endregion
        
        #region Test Starting in S22
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        [Microsoft.VisualStudio.TestTools.UnitTesting.DescriptionAttribute("MS-FRS2_R407, MS-FRS2_R410, MS-FRS2_R417, MS-FRS2_R37, MS-FRS2_R436, MS-FRS2_R439" +
            "")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestCategory("AD")]
        [TestCategory("MS-FRS2")]
        [TestCategory("SDC")]
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        
        public virtual void FRS2_TCtestScenario7S22()
        {
            this.Manager.BeginTest("TCtestScenario7S22");
            this.Manager.Comment("reaching state \'S22\'");
            FRS2Model.error_status_t temp65;
            this.Manager.Comment(@"executing step 'call Initialization(Windows7,Map{1=>enabled,2=>enabled,3=>enabled,4=>disabled},Map{1=>exists,2=>exists,3=>exists,4=>exists},Map{""A""=>1,""B""=>2,""C""=>3,""D""=>4},Map{""A""=>sysvol,""B""=>protection,""C""=>sysvol,""D""=>sysvol},Map{1=>""A"",2=>""B"",3=>""C"",4=>""D""},Map{1=>writeOnly,2=>readWrite,3=>readOnly,4=>readWrite},Map{1=>enabled,2=>disabled,3=>enabled,4=>enabled})'");
            temp65 = this.IFRS2ManagedAdapterInstance.Initialization(FRS2Model.OSVersion.Windows7, new Microsoft.Modeling.Map<System.Int32,FRS2Model.connectionProperty>().Add(1, FRS2Model.connectionProperty.enabled).Add(2, FRS2Model.connectionProperty.enabled).Add(3, FRS2Model.connectionProperty.enabled).Add(4, FRS2Model.connectionProperty.disabled), new Microsoft.Modeling.Map<System.Int32,FRS2Model.FromServer>().Add(1, FRS2Model.FromServer.exists).Add(2, FRS2Model.FromServer.exists).Add(3, FRS2Model.FromServer.exists).Add(4, FRS2Model.FromServer.exists), new Microsoft.Modeling.Map<System.String,System.Int32>().Add("A", 1).Add("B", 2).Add("C", 3).Add("D", 4), new Microsoft.Modeling.Map<System.String,FRS2Model.ReplicationGroupTypes>().Add("A", FRS2Model.ReplicationGroupTypes.sysvol).Add("B", FRS2Model.ReplicationGroupTypes.protection).Add("C", FRS2Model.ReplicationGroupTypes.sysvol).Add("D", FRS2Model.ReplicationGroupTypes.sysvol), new Microsoft.Modeling.Map<System.Int32,System.String>().Add(1, "A").Add(2, "B").Add(3, "C").Add(4, "D"), new Microsoft.Modeling.Map<System.Int32,FRS2Model.accessLevels>().Add(1, FRS2Model.accessLevels.writeOnly).Add(2, FRS2Model.accessLevels.readWrite).Add(3, FRS2Model.accessLevels.readOnly).Add(4, FRS2Model.accessLevels.readWrite), new Microsoft.Modeling.Map<System.Int32,FRS2Model.connectionProperty>().Add(1, FRS2Model.connectionProperty.enabled).Add(2, FRS2Model.connectionProperty.disabled).Add(3, FRS2Model.connectionProperty.enabled).Add(4, FRS2Model.connectionProperty.enabled));
            this.Manager.Comment("reaching state \'S23\'");
            this.Manager.Comment("checking step \'return Initialization/ERROR_SUCCESS\'");
            this.Manager.Assert((temp65 == FRS2Model.error_status_t.ERROR_SUCCESS), String.Format("expected \'FRS2Model.error_status_t.ERROR_SUCCESS\', actual \'{0}\' (return of Initia" +
                        "lization, state S23)", TestManagerHelpers.Describe(temp65)));
            this.Manager.Comment("reaching state \'S67\'");
            FRS2Model.error_status_t temp66;
            this.Manager.Comment("executing step \'call CheckConnectivity(\"A\",1)\'");
            temp66 = this.IFRS2ManagedAdapterInstance.CheckConnectivity("A", 1);
            this.Manager.Checkpoint("MS-FRS2_R407");
            this.Manager.Checkpoint("MS-FRS2_R410");
            this.Manager.Checkpoint("MS-FRS2_R417");
            this.Manager.Comment("reaching state \'S95\'");
            this.Manager.Comment("checking step \'return CheckConnectivity/ERROR_SUCCESS\'");
            this.Manager.Assert((temp66 == FRS2Model.error_status_t.ERROR_SUCCESS), String.Format("expected \'FRS2Model.error_status_t.ERROR_SUCCESS\', actual \'{0}\' (return of CheckC" +
                        "onnectivity, state S95)", TestManagerHelpers.Describe(temp66)));
            this.Manager.Comment("reaching state \'S123\'");
            FRS2Model.ProtocolVersionReturned temp67;
            FRS2Model.UpstreamFlagValueReturned temp68;
            FRS2Model.error_status_t temp69;
            this.Manager.Comment("executing step \'call EstablishConnection(\"C\",3,FRS_COMMUNICATION_PROTOCOL_VERSION" +
                    "_LONGHORN_SERVER,out _,out _)\'");
            temp69 = this.IFRS2ManagedAdapterInstance.EstablishConnection("C", 3, FRS2Model.ProtocolVersion.FRS_COMMUNICATION_PROTOCOL_VERSION_LONGHORN_SERVER, out temp67, out temp68);
            this.Manager.Checkpoint("MS-FRS2_R37");
            this.Manager.Checkpoint("MS-FRS2_R436");
            this.Manager.Checkpoint("MS-FRS2_R439");
            this.Manager.Comment("reaching state \'S151\'");
            this.Manager.Comment("checking step \'return EstablishConnection/[out Valid,out Valid]:ERROR_SUCCESS\'");
            this.Manager.Assert((temp67 == FRS2Model.ProtocolVersionReturned.Valid), String.Format("expected \'FRS2Model.ProtocolVersionReturned.Valid\', actual \'{0}\' (upstreamProtoco" +
                        "lVersion of EstablishConnection, state S151)", TestManagerHelpers.Describe(temp67)));
            this.Manager.Assert((temp68 == FRS2Model.UpstreamFlagValueReturned.Valid), String.Format("expected \'FRS2Model.UpstreamFlagValueReturned.Valid\', actual \'{0}\' (upstreamFlags" +
                        " of EstablishConnection, state S151)", TestManagerHelpers.Describe(temp68)));
            this.Manager.Assert((temp69 == FRS2Model.error_status_t.ERROR_SUCCESS), String.Format("expected \'FRS2Model.error_status_t.ERROR_SUCCESS\', actual \'{0}\' (return of Establ" +
                        "ishConnection, state S151)", TestManagerHelpers.Describe(temp69)));
            this.Manager.Comment("reaching state \'S179\'");
            FRS2Model.error_status_t temp70;
            this.Manager.Comment("executing step \'call EstablishSession(2,2)\'");
            temp70 = this.IFRS2ManagedAdapterInstance.EstablishSession(2, 2);
            this.Manager.Comment("reaching state \'S206\'");
            this.Manager.Comment("checking step \'return EstablishSession/FRS_ERROR_CONNECTION_INVALID\'");
            this.Manager.Assert((temp70 == FRS2Model.error_status_t.FRS_ERROR_CONNECTION_INVALID), String.Format("expected \'FRS2Model.error_status_t.FRS_ERROR_CONNECTION_INVALID\', actual \'{0}\' (r" +
                        "eturn of EstablishSession, state S206)", TestManagerHelpers.Describe(temp70)));
            TCtestScenario7S225();
            this.Manager.EndTest();
        }
        #endregion
        
        #region Test Starting in S24
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        [Microsoft.VisualStudio.TestTools.UnitTesting.DescriptionAttribute("MS-FRS2_R407, MS-FRS2_R410, MS-FRS2_R417, MS-FRS2_R37, MS-FRS2_R436, MS-FRS2_R439" +
            "")]
        public virtual void FRS2_TCtestScenario7S24()
        {
            this.Manager.BeginTest("TCtestScenario7S24");
            this.Manager.Comment("reaching state \'S24\'");
            FRS2Model.error_status_t temp71;
            this.Manager.Comment(@"executing step 'call Initialization(Windows7,Map{1=>enabled,2=>enabled,3=>enabled,4=>disabled},Map{1=>exists,2=>exists,3=>exists,4=>exists},Map{""A""=>1,""B""=>2,""C""=>3,""D""=>4},Map{""A""=>sysvol,""B""=>protection,""C""=>sysvol,""D""=>sysvol},Map{1=>""A"",2=>""B"",3=>""C"",4=>""D""},Map{1=>writeOnly,2=>readWrite,3=>readOnly,4=>readWrite},Map{1=>enabled,2=>disabled,3=>enabled,4=>enabled})'");
            temp71 = this.IFRS2ManagedAdapterInstance.Initialization(FRS2Model.OSVersion.Windows7, new Microsoft.Modeling.Map<System.Int32,FRS2Model.connectionProperty>().Add(1, FRS2Model.connectionProperty.enabled).Add(2, FRS2Model.connectionProperty.enabled).Add(3, FRS2Model.connectionProperty.enabled).Add(4, FRS2Model.connectionProperty.disabled), new Microsoft.Modeling.Map<System.Int32,FRS2Model.FromServer>().Add(1, FRS2Model.FromServer.exists).Add(2, FRS2Model.FromServer.exists).Add(3, FRS2Model.FromServer.exists).Add(4, FRS2Model.FromServer.exists), new Microsoft.Modeling.Map<System.String,System.Int32>().Add("A", 1).Add("B", 2).Add("C", 3).Add("D", 4), new Microsoft.Modeling.Map<System.String,FRS2Model.ReplicationGroupTypes>().Add("A", FRS2Model.ReplicationGroupTypes.sysvol).Add("B", FRS2Model.ReplicationGroupTypes.protection).Add("C", FRS2Model.ReplicationGroupTypes.sysvol).Add("D", FRS2Model.ReplicationGroupTypes.sysvol), new Microsoft.Modeling.Map<System.Int32,System.String>().Add(1, "A").Add(2, "B").Add(3, "C").Add(4, "D"), new Microsoft.Modeling.Map<System.Int32,FRS2Model.accessLevels>().Add(1, FRS2Model.accessLevels.writeOnly).Add(2, FRS2Model.accessLevels.readWrite).Add(3, FRS2Model.accessLevels.readOnly).Add(4, FRS2Model.accessLevels.readWrite), new Microsoft.Modeling.Map<System.Int32,FRS2Model.connectionProperty>().Add(1, FRS2Model.connectionProperty.enabled).Add(2, FRS2Model.connectionProperty.disabled).Add(3, FRS2Model.connectionProperty.enabled).Add(4, FRS2Model.connectionProperty.enabled));
            this.Manager.Comment("reaching state \'S25\'");
            this.Manager.Comment("checking step \'return Initialization/ERROR_SUCCESS\'");
            this.Manager.Assert((temp71 == FRS2Model.error_status_t.ERROR_SUCCESS), String.Format("expected \'FRS2Model.error_status_t.ERROR_SUCCESS\', actual \'{0}\' (return of Initia" +
                        "lization, state S25)", TestManagerHelpers.Describe(temp71)));
            this.Manager.Comment("reaching state \'S68\'");
            FRS2Model.error_status_t temp72;
            this.Manager.Comment("executing step \'call CheckConnectivity(\"A\",1)\'");
            temp72 = this.IFRS2ManagedAdapterInstance.CheckConnectivity("A", 1);
            this.Manager.Checkpoint("MS-FRS2_R407");
            this.Manager.Checkpoint("MS-FRS2_R410");
            this.Manager.Checkpoint("MS-FRS2_R417");
            this.Manager.Comment("reaching state \'S96\'");
            this.Manager.Comment("checking step \'return CheckConnectivity/ERROR_SUCCESS\'");
            this.Manager.Assert((temp72 == FRS2Model.error_status_t.ERROR_SUCCESS), String.Format("expected \'FRS2Model.error_status_t.ERROR_SUCCESS\', actual \'{0}\' (return of CheckC" +
                        "onnectivity, state S96)", TestManagerHelpers.Describe(temp72)));
            this.Manager.Comment("reaching state \'S124\'");
            FRS2Model.ProtocolVersionReturned temp73;
            FRS2Model.UpstreamFlagValueReturned temp74;
            FRS2Model.error_status_t temp75;
            this.Manager.Comment("executing step \'call EstablishConnection(\"C\",3,FRS_COMMUNICATION_PROTOCOL_VERSION" +
                    "_LONGHORN_SERVER,out _,out _)\'");
            temp75 = this.IFRS2ManagedAdapterInstance.EstablishConnection("C", 3, FRS2Model.ProtocolVersion.FRS_COMMUNICATION_PROTOCOL_VERSION_LONGHORN_SERVER, out temp73, out temp74);
            this.Manager.Checkpoint("MS-FRS2_R37");
            this.Manager.Checkpoint("MS-FRS2_R436");
            this.Manager.Checkpoint("MS-FRS2_R439");
            this.Manager.Comment("reaching state \'S152\'");
            this.Manager.Comment("checking step \'return EstablishConnection/[out Valid,out Valid]:ERROR_SUCCESS\'");
            this.Manager.Assert((temp73 == FRS2Model.ProtocolVersionReturned.Valid), String.Format("expected \'FRS2Model.ProtocolVersionReturned.Valid\', actual \'{0}\' (upstreamProtoco" +
                        "lVersion of EstablishConnection, state S152)", TestManagerHelpers.Describe(temp73)));
            this.Manager.Assert((temp74 == FRS2Model.UpstreamFlagValueReturned.Valid), String.Format("expected \'FRS2Model.UpstreamFlagValueReturned.Valid\', actual \'{0}\' (upstreamFlags" +
                        " of EstablishConnection, state S152)", TestManagerHelpers.Describe(temp74)));
            this.Manager.Assert((temp75 == FRS2Model.error_status_t.ERROR_SUCCESS), String.Format("expected \'FRS2Model.error_status_t.ERROR_SUCCESS\', actual \'{0}\' (return of Establ" +
                        "ishConnection, state S152)", TestManagerHelpers.Describe(temp75)));
            this.Manager.Comment("reaching state \'S180\'");
            FRS2Model.error_status_t temp76;
            this.Manager.Comment("executing step \'call EstablishSession(1,2)\'");
            temp76 = this.IFRS2ManagedAdapterInstance.EstablishSession(1, 2);
            this.Manager.Comment("reaching state \'S207\'");
            this.Manager.Comment("checking step \'return EstablishSession/ERROR_FAIL\'");
            this.Manager.Assert((temp76 == FRS2Model.error_status_t.ERROR_FAIL), String.Format("expected \'FRS2Model.error_status_t.ERROR_FAIL\', actual \'{0}\' (return of Establish" +
                        "Session, state S207)", TestManagerHelpers.Describe(temp76)));
            TCtestScenario7S225();
            this.Manager.EndTest();
        }
        #endregion
        
        #region Test Starting in S26
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        [Microsoft.VisualStudio.TestTools.UnitTesting.DescriptionAttribute("MS-FRS2_R407, MS-FRS2_R410, MS-FRS2_R417, MS-FRS2_R37, MS-FRS2_R436, MS-FRS2_R439" +
            ", MS-FRS2_R455, MS-FRS2_R458, MS-FRS2_R448")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestCategory("AD")]
        [TestCategory("MS-FRS2")]
        [TestCategory("SDC")]
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        
        public virtual void FRS2_TCtestScenario7S26()
        {
            this.Manager.BeginTest("TCtestScenario7S26");
            this.Manager.Comment("reaching state \'S26\'");
            FRS2Model.error_status_t temp77;
            this.Manager.Comment(@"executing step 'call Initialization(Windows7,Map{1=>enabled,2=>enabled,3=>enabled,4=>disabled},Map{1=>exists,2=>exists,3=>exists,4=>exists},Map{""A""=>1,""B""=>2,""C""=>3,""D""=>4},Map{""A""=>sysvol,""B""=>protection,""C""=>sysvol,""D""=>sysvol},Map{1=>""A"",2=>""B"",3=>""C"",4=>""D""},Map{1=>writeOnly,2=>readWrite,3=>readOnly,4=>readWrite},Map{1=>enabled,2=>disabled,3=>enabled,4=>enabled})'");
            temp77 = this.IFRS2ManagedAdapterInstance.Initialization(FRS2Model.OSVersion.Windows7, new Microsoft.Modeling.Map<System.Int32,FRS2Model.connectionProperty>().Add(1, FRS2Model.connectionProperty.enabled).Add(2, FRS2Model.connectionProperty.enabled).Add(3, FRS2Model.connectionProperty.enabled).Add(4, FRS2Model.connectionProperty.disabled), new Microsoft.Modeling.Map<System.Int32,FRS2Model.FromServer>().Add(1, FRS2Model.FromServer.exists).Add(2, FRS2Model.FromServer.exists).Add(3, FRS2Model.FromServer.exists).Add(4, FRS2Model.FromServer.exists), new Microsoft.Modeling.Map<System.String,System.Int32>().Add("A", 1).Add("B", 2).Add("C", 3).Add("D", 4), new Microsoft.Modeling.Map<System.String,FRS2Model.ReplicationGroupTypes>().Add("A", FRS2Model.ReplicationGroupTypes.sysvol).Add("B", FRS2Model.ReplicationGroupTypes.protection).Add("C", FRS2Model.ReplicationGroupTypes.sysvol).Add("D", FRS2Model.ReplicationGroupTypes.sysvol), new Microsoft.Modeling.Map<System.Int32,System.String>().Add(1, "A").Add(2, "B").Add(3, "C").Add(4, "D"), new Microsoft.Modeling.Map<System.Int32,FRS2Model.accessLevels>().Add(1, FRS2Model.accessLevels.writeOnly).Add(2, FRS2Model.accessLevels.readWrite).Add(3, FRS2Model.accessLevels.readOnly).Add(4, FRS2Model.accessLevels.readWrite), new Microsoft.Modeling.Map<System.Int32,FRS2Model.connectionProperty>().Add(1, FRS2Model.connectionProperty.enabled).Add(2, FRS2Model.connectionProperty.disabled).Add(3, FRS2Model.connectionProperty.enabled).Add(4, FRS2Model.connectionProperty.enabled));
            this.Manager.Comment("reaching state \'S27\'");
            this.Manager.Comment("checking step \'return Initialization/ERROR_SUCCESS\'");
            this.Manager.Assert((temp77 == FRS2Model.error_status_t.ERROR_SUCCESS), String.Format("expected \'FRS2Model.error_status_t.ERROR_SUCCESS\', actual \'{0}\' (return of Initia" +
                        "lization, state S27)", TestManagerHelpers.Describe(temp77)));
            this.Manager.Comment("reaching state \'S69\'");
            FRS2Model.error_status_t temp78;
            this.Manager.Comment("executing step \'call CheckConnectivity(\"A\",1)\'");
            temp78 = this.IFRS2ManagedAdapterInstance.CheckConnectivity("A", 1);
            this.Manager.Checkpoint("MS-FRS2_R407");
            this.Manager.Checkpoint("MS-FRS2_R410");
            this.Manager.Checkpoint("MS-FRS2_R417");
            this.Manager.Comment("reaching state \'S97\'");
            this.Manager.Comment("checking step \'return CheckConnectivity/ERROR_SUCCESS\'");
            this.Manager.Assert((temp78 == FRS2Model.error_status_t.ERROR_SUCCESS), String.Format("expected \'FRS2Model.error_status_t.ERROR_SUCCESS\', actual \'{0}\' (return of CheckC" +
                        "onnectivity, state S97)", TestManagerHelpers.Describe(temp78)));
            this.Manager.Comment("reaching state \'S125\'");
            FRS2Model.ProtocolVersionReturned temp79;
            FRS2Model.UpstreamFlagValueReturned temp80;
            FRS2Model.error_status_t temp81;
            this.Manager.Comment("executing step \'call EstablishConnection(\"A\",1,FRS_COMMUNICATION_PROTOCOL_VERSION" +
                    "_LONGHORN_SERVER,out _,out _)\'");
            temp81 = this.IFRS2ManagedAdapterInstance.EstablishConnection("A", 1, FRS2Model.ProtocolVersion.FRS_COMMUNICATION_PROTOCOL_VERSION_LONGHORN_SERVER, out temp79, out temp80);
            this.Manager.Checkpoint("MS-FRS2_R37");
            this.Manager.Checkpoint("MS-FRS2_R436");
            this.Manager.Checkpoint("MS-FRS2_R439");
            this.Manager.Comment("reaching state \'S153\'");
            this.Manager.Comment("checking step \'return EstablishConnection/[out Valid,out Valid]:ERROR_SUCCESS\'");
            this.Manager.Assert((temp79 == FRS2Model.ProtocolVersionReturned.Valid), String.Format("expected \'FRS2Model.ProtocolVersionReturned.Valid\', actual \'{0}\' (upstreamProtoco" +
                        "lVersion of EstablishConnection, state S153)", TestManagerHelpers.Describe(temp79)));
            this.Manager.Assert((temp80 == FRS2Model.UpstreamFlagValueReturned.Valid), String.Format("expected \'FRS2Model.UpstreamFlagValueReturned.Valid\', actual \'{0}\' (upstreamFlags" +
                        " of EstablishConnection, state S153)", TestManagerHelpers.Describe(temp80)));
            this.Manager.Assert((temp81 == FRS2Model.error_status_t.ERROR_SUCCESS), String.Format("expected \'FRS2Model.error_status_t.ERROR_SUCCESS\', actual \'{0}\' (return of Establ" +
                        "ishConnection, state S153)", TestManagerHelpers.Describe(temp81)));
            this.Manager.Comment("reaching state \'S181\'");
            FRS2Model.error_status_t temp82;
            this.Manager.Comment("executing step \'call EstablishSession(1,1)\'");
            temp82 = this.IFRS2ManagedAdapterInstance.EstablishSession(1, 1);
            this.Manager.Checkpoint("MS-FRS2_R455");
            this.Manager.Checkpoint("MS-FRS2_R458");
            this.Manager.Checkpoint("MS-FRS2_R448");
            this.Manager.Comment("reaching state \'S208\'");
            this.Manager.Comment("checking step \'return EstablishSession/ERROR_SUCCESS\'");
            this.Manager.Assert((temp82 == FRS2Model.error_status_t.ERROR_SUCCESS), String.Format("expected \'FRS2Model.error_status_t.ERROR_SUCCESS\', actual \'{0}\' (return of Establ" +
                        "ishSession, state S208)", TestManagerHelpers.Describe(temp82)));
            this.Manager.Comment("reaching state \'S228\'");
            this.Manager.EndTest();
        }
        #endregion
        
        #region Test Starting in S28
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        [Microsoft.VisualStudio.TestTools.UnitTesting.DescriptionAttribute("MS-FRS2_R407, MS-FRS2_R410, MS-FRS2_R417, MS-FRS2_R37, MS-FRS2_R436, MS-FRS2_R439" +
            ", MS-FRS2_R456, MS-FRS2_R459, MS-FRS2_R462")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestCategory("AD")]
        [TestCategory("MS-FRS2")]
        [TestCategory("SDC")]
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        
        public virtual void FRS2_TCtestScenario7S28()
        {
            this.Manager.BeginTest("TCtestScenario7S28");
            this.Manager.Comment("reaching state \'S28\'");
            FRS2Model.error_status_t temp83;
            this.Manager.Comment(@"executing step 'call Initialization(Windows7,Map{1=>enabled,2=>enabled,3=>enabled,4=>disabled},Map{1=>exists,2=>exists,3=>exists,4=>exists},Map{""A""=>1,""B""=>2,""C""=>3,""D""=>4},Map{""A""=>sysvol,""B""=>protection,""C""=>sysvol,""D""=>sysvol},Map{1=>""A"",2=>""B"",3=>""C"",4=>""D""},Map{1=>writeOnly,2=>readWrite,3=>readOnly,4=>readWrite},Map{1=>enabled,2=>disabled,3=>enabled,4=>enabled})'");
            temp83 = this.IFRS2ManagedAdapterInstance.Initialization(FRS2Model.OSVersion.Windows7, new Microsoft.Modeling.Map<System.Int32,FRS2Model.connectionProperty>().Add(1, FRS2Model.connectionProperty.enabled).Add(2, FRS2Model.connectionProperty.enabled).Add(3, FRS2Model.connectionProperty.enabled).Add(4, FRS2Model.connectionProperty.disabled), new Microsoft.Modeling.Map<System.Int32,FRS2Model.FromServer>().Add(1, FRS2Model.FromServer.exists).Add(2, FRS2Model.FromServer.exists).Add(3, FRS2Model.FromServer.exists).Add(4, FRS2Model.FromServer.exists), new Microsoft.Modeling.Map<System.String,System.Int32>().Add("A", 1).Add("B", 2).Add("C", 3).Add("D", 4), new Microsoft.Modeling.Map<System.String,FRS2Model.ReplicationGroupTypes>().Add("A", FRS2Model.ReplicationGroupTypes.sysvol).Add("B", FRS2Model.ReplicationGroupTypes.protection).Add("C", FRS2Model.ReplicationGroupTypes.sysvol).Add("D", FRS2Model.ReplicationGroupTypes.sysvol), new Microsoft.Modeling.Map<System.Int32,System.String>().Add(1, "A").Add(2, "B").Add(3, "C").Add(4, "D"), new Microsoft.Modeling.Map<System.Int32,FRS2Model.accessLevels>().Add(1, FRS2Model.accessLevels.writeOnly).Add(2, FRS2Model.accessLevels.readWrite).Add(3, FRS2Model.accessLevels.readOnly).Add(4, FRS2Model.accessLevels.readWrite), new Microsoft.Modeling.Map<System.Int32,FRS2Model.connectionProperty>().Add(1, FRS2Model.connectionProperty.enabled).Add(2, FRS2Model.connectionProperty.disabled).Add(3, FRS2Model.connectionProperty.enabled).Add(4, FRS2Model.connectionProperty.enabled));
            this.Manager.Comment("reaching state \'S29\'");
            this.Manager.Comment("checking step \'return Initialization/ERROR_SUCCESS\'");
            this.Manager.Assert((temp83 == FRS2Model.error_status_t.ERROR_SUCCESS), String.Format("expected \'FRS2Model.error_status_t.ERROR_SUCCESS\', actual \'{0}\' (return of Initia" +
                        "lization, state S29)", TestManagerHelpers.Describe(temp83)));
            this.Manager.Comment("reaching state \'S70\'");
            FRS2Model.error_status_t temp84;
            this.Manager.Comment("executing step \'call CheckConnectivity(\"A\",1)\'");
            temp84 = this.IFRS2ManagedAdapterInstance.CheckConnectivity("A", 1);
            this.Manager.Checkpoint("MS-FRS2_R407");
            this.Manager.Checkpoint("MS-FRS2_R410");
            this.Manager.Checkpoint("MS-FRS2_R417");
            this.Manager.Comment("reaching state \'S98\'");
            this.Manager.Comment("checking step \'return CheckConnectivity/ERROR_SUCCESS\'");
            this.Manager.Assert((temp84 == FRS2Model.error_status_t.ERROR_SUCCESS), String.Format("expected \'FRS2Model.error_status_t.ERROR_SUCCESS\', actual \'{0}\' (return of CheckC" +
                        "onnectivity, state S98)", TestManagerHelpers.Describe(temp84)));
            this.Manager.Comment("reaching state \'S126\'");
            FRS2Model.ProtocolVersionReturned temp85;
            FRS2Model.UpstreamFlagValueReturned temp86;
            FRS2Model.error_status_t temp87;
            this.Manager.Comment("executing step \'call EstablishConnection(\"A\",1,FRS_COMMUNICATION_PROTOCOL_VERSION" +
                    "_LONGHORN_SERVER,out _,out _)\'");
            temp87 = this.IFRS2ManagedAdapterInstance.EstablishConnection("A", 1, FRS2Model.ProtocolVersion.FRS_COMMUNICATION_PROTOCOL_VERSION_LONGHORN_SERVER, out temp85, out temp86);
            this.Manager.Checkpoint("MS-FRS2_R37");
            this.Manager.Checkpoint("MS-FRS2_R436");
            this.Manager.Checkpoint("MS-FRS2_R439");
            this.Manager.Comment("reaching state \'S154\'");
            this.Manager.Comment("checking step \'return EstablishConnection/[out Valid,out Valid]:ERROR_SUCCESS\'");
            this.Manager.Assert((temp85 == FRS2Model.ProtocolVersionReturned.Valid), String.Format("expected \'FRS2Model.ProtocolVersionReturned.Valid\', actual \'{0}\' (upstreamProtoco" +
                        "lVersion of EstablishConnection, state S154)", TestManagerHelpers.Describe(temp85)));
            this.Manager.Assert((temp86 == FRS2Model.UpstreamFlagValueReturned.Valid), String.Format("expected \'FRS2Model.UpstreamFlagValueReturned.Valid\', actual \'{0}\' (upstreamFlags" +
                        " of EstablishConnection, state S154)", TestManagerHelpers.Describe(temp86)));
            this.Manager.Assert((temp87 == FRS2Model.error_status_t.ERROR_SUCCESS), String.Format("expected \'FRS2Model.error_status_t.ERROR_SUCCESS\', actual \'{0}\' (return of Establ" +
                        "ishConnection, state S154)", TestManagerHelpers.Describe(temp87)));
            this.Manager.Comment("reaching state \'S182\'");
            FRS2Model.error_status_t temp88;
            this.Manager.Comment("executing step \'call EstablishSession(4,4)\'");
            temp88 = this.IFRS2ManagedAdapterInstance.EstablishSession(4, 4);
            this.Manager.Checkpoint("MS-FRS2_R456");
            this.Manager.Checkpoint("MS-FRS2_R459");
            this.Manager.Checkpoint("MS-FRS2_R462");
            this.Manager.Comment("reaching state \'S209\'");
            this.Manager.Comment("checking step \'return EstablishSession/FRS_ERROR_CONNECTION_INVALID\'");
            this.Manager.Assert((temp88 == FRS2Model.error_status_t.FRS_ERROR_CONNECTION_INVALID), String.Format("expected \'FRS2Model.error_status_t.FRS_ERROR_CONNECTION_INVALID\', actual \'{0}\' (r" +
                        "eturn of EstablishSession, state S209)", TestManagerHelpers.Describe(temp88)));
            TCtestScenario7S229();
            this.Manager.EndTest();
        }
        
        private void TCtestScenario7S229() {
            this.Manager.Comment("reaching state \'S229\'");
        }
        #endregion
        
        #region Test Starting in S30
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        [Microsoft.VisualStudio.TestTools.UnitTesting.DescriptionAttribute("MS-FRS2_R407, MS-FRS2_R410, MS-FRS2_R417, MS-FRS2_R37, MS-FRS2_R436, MS-FRS2_R439" +
            "")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestCategory("AD")]
        [TestCategory("MS-FRS2")]
        [TestCategory("SDC")]
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        
        public virtual void FRS2_TCtestScenario7S30()
        {
            this.Manager.BeginTest("TCtestScenario7S30");
            this.Manager.Comment("reaching state \'S30\'");
            FRS2Model.error_status_t temp89;
            this.Manager.Comment(@"executing step 'call Initialization(Windows7,Map{1=>enabled,2=>enabled,3=>enabled,4=>disabled},Map{1=>exists,2=>exists,3=>exists,4=>exists},Map{""A""=>1,""B""=>2,""C""=>3,""D""=>4},Map{""A""=>sysvol,""B""=>protection,""C""=>sysvol,""D""=>sysvol},Map{1=>""A"",2=>""B"",3=>""C"",4=>""D""},Map{1=>writeOnly,2=>readWrite,3=>readOnly,4=>readWrite},Map{1=>enabled,2=>disabled,3=>enabled,4=>enabled})'");
            temp89 = this.IFRS2ManagedAdapterInstance.Initialization(FRS2Model.OSVersion.Windows7, new Microsoft.Modeling.Map<System.Int32,FRS2Model.connectionProperty>().Add(1, FRS2Model.connectionProperty.enabled).Add(2, FRS2Model.connectionProperty.enabled).Add(3, FRS2Model.connectionProperty.enabled).Add(4, FRS2Model.connectionProperty.disabled), new Microsoft.Modeling.Map<System.Int32,FRS2Model.FromServer>().Add(1, FRS2Model.FromServer.exists).Add(2, FRS2Model.FromServer.exists).Add(3, FRS2Model.FromServer.exists).Add(4, FRS2Model.FromServer.exists), new Microsoft.Modeling.Map<System.String,System.Int32>().Add("A", 1).Add("B", 2).Add("C", 3).Add("D", 4), new Microsoft.Modeling.Map<System.String,FRS2Model.ReplicationGroupTypes>().Add("A", FRS2Model.ReplicationGroupTypes.sysvol).Add("B", FRS2Model.ReplicationGroupTypes.protection).Add("C", FRS2Model.ReplicationGroupTypes.sysvol).Add("D", FRS2Model.ReplicationGroupTypes.sysvol), new Microsoft.Modeling.Map<System.Int32,System.String>().Add(1, "A").Add(2, "B").Add(3, "C").Add(4, "D"), new Microsoft.Modeling.Map<System.Int32,FRS2Model.accessLevels>().Add(1, FRS2Model.accessLevels.writeOnly).Add(2, FRS2Model.accessLevels.readWrite).Add(3, FRS2Model.accessLevels.readOnly).Add(4, FRS2Model.accessLevels.readWrite), new Microsoft.Modeling.Map<System.Int32,FRS2Model.connectionProperty>().Add(1, FRS2Model.connectionProperty.enabled).Add(2, FRS2Model.connectionProperty.disabled).Add(3, FRS2Model.connectionProperty.enabled).Add(4, FRS2Model.connectionProperty.enabled));
            this.Manager.Comment("reaching state \'S31\'");
            this.Manager.Comment("checking step \'return Initialization/ERROR_SUCCESS\'");
            this.Manager.Assert((temp89 == FRS2Model.error_status_t.ERROR_SUCCESS), String.Format("expected \'FRS2Model.error_status_t.ERROR_SUCCESS\', actual \'{0}\' (return of Initia" +
                        "lization, state S31)", TestManagerHelpers.Describe(temp89)));
            this.Manager.Comment("reaching state \'S71\'");
            FRS2Model.error_status_t temp90;
            this.Manager.Comment("executing step \'call CheckConnectivity(\"A\",1)\'");
            temp90 = this.IFRS2ManagedAdapterInstance.CheckConnectivity("A", 1);
            this.Manager.Checkpoint("MS-FRS2_R407");
            this.Manager.Checkpoint("MS-FRS2_R410");
            this.Manager.Checkpoint("MS-FRS2_R417");
            this.Manager.Comment("reaching state \'S99\'");
            this.Manager.Comment("checking step \'return CheckConnectivity/ERROR_SUCCESS\'");
            this.Manager.Assert((temp90 == FRS2Model.error_status_t.ERROR_SUCCESS), String.Format("expected \'FRS2Model.error_status_t.ERROR_SUCCESS\', actual \'{0}\' (return of CheckC" +
                        "onnectivity, state S99)", TestManagerHelpers.Describe(temp90)));
            this.Manager.Comment("reaching state \'S127\'");
            FRS2Model.ProtocolVersionReturned temp91;
            FRS2Model.UpstreamFlagValueReturned temp92;
            FRS2Model.error_status_t temp93;
            this.Manager.Comment("executing step \'call EstablishConnection(\"A\",1,FRS_COMMUNICATION_PROTOCOL_VERSION" +
                    "_LONGHORN_SERVER,out _,out _)\'");
            temp93 = this.IFRS2ManagedAdapterInstance.EstablishConnection("A", 1, FRS2Model.ProtocolVersion.FRS_COMMUNICATION_PROTOCOL_VERSION_LONGHORN_SERVER, out temp91, out temp92);
            this.Manager.Checkpoint("MS-FRS2_R37");
            this.Manager.Checkpoint("MS-FRS2_R436");
            this.Manager.Checkpoint("MS-FRS2_R439");
            this.Manager.Comment("reaching state \'S155\'");
            this.Manager.Comment("checking step \'return EstablishConnection/[out Valid,out Valid]:ERROR_SUCCESS\'");
            this.Manager.Assert((temp91 == FRS2Model.ProtocolVersionReturned.Valid), String.Format("expected \'FRS2Model.ProtocolVersionReturned.Valid\', actual \'{0}\' (upstreamProtoco" +
                        "lVersion of EstablishConnection, state S155)", TestManagerHelpers.Describe(temp91)));
            this.Manager.Assert((temp92 == FRS2Model.UpstreamFlagValueReturned.Valid), String.Format("expected \'FRS2Model.UpstreamFlagValueReturned.Valid\', actual \'{0}\' (upstreamFlags" +
                        " of EstablishConnection, state S155)", TestManagerHelpers.Describe(temp92)));
            this.Manager.Assert((temp93 == FRS2Model.error_status_t.ERROR_SUCCESS), String.Format("expected \'FRS2Model.error_status_t.ERROR_SUCCESS\', actual \'{0}\' (return of Establ" +
                        "ishConnection, state S155)", TestManagerHelpers.Describe(temp93)));
            this.Manager.Comment("reaching state \'S183\'");
            FRS2Model.error_status_t temp94;
            this.Manager.Comment("executing step \'call EstablishSession(2,2)\'");
            temp94 = this.IFRS2ManagedAdapterInstance.EstablishSession(2, 2);
            this.Manager.Comment("reaching state \'S210\'");
            this.Manager.Comment("checking step \'return EstablishSession/FRS_ERROR_CONNECTION_INVALID\'");
            this.Manager.Assert((temp94 == FRS2Model.error_status_t.FRS_ERROR_CONNECTION_INVALID), String.Format("expected \'FRS2Model.error_status_t.FRS_ERROR_CONNECTION_INVALID\', actual \'{0}\' (r" +
                        "eturn of EstablishSession, state S210)", TestManagerHelpers.Describe(temp94)));
            TCtestScenario7S229();
            this.Manager.EndTest();
        }
        #endregion
        
        #region Test Starting in S32
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        [Microsoft.VisualStudio.TestTools.UnitTesting.DescriptionAttribute("MS-FRS2_R407, MS-FRS2_R410, MS-FRS2_R417, MS-FRS2_R37, MS-FRS2_R436, MS-FRS2_R439" +
            "")]
        public virtual void FRS2_TCtestScenario7S32()
        {
            this.Manager.BeginTest("TCtestScenario7S32");
            this.Manager.Comment("reaching state \'S32\'");
            FRS2Model.error_status_t temp95;
            this.Manager.Comment(@"executing step 'call Initialization(Windows7,Map{1=>enabled,2=>enabled,3=>enabled,4=>disabled},Map{1=>exists,2=>exists,3=>exists,4=>exists},Map{""A""=>1,""B""=>2,""C""=>3,""D""=>4},Map{""A""=>sysvol,""B""=>protection,""C""=>sysvol,""D""=>sysvol},Map{1=>""A"",2=>""B"",3=>""C"",4=>""D""},Map{1=>writeOnly,2=>readWrite,3=>readOnly,4=>readWrite},Map{1=>enabled,2=>disabled,3=>enabled,4=>enabled})'");
            temp95 = this.IFRS2ManagedAdapterInstance.Initialization(FRS2Model.OSVersion.Windows7, new Microsoft.Modeling.Map<System.Int32,FRS2Model.connectionProperty>().Add(1, FRS2Model.connectionProperty.enabled).Add(2, FRS2Model.connectionProperty.enabled).Add(3, FRS2Model.connectionProperty.enabled).Add(4, FRS2Model.connectionProperty.disabled), new Microsoft.Modeling.Map<System.Int32,FRS2Model.FromServer>().Add(1, FRS2Model.FromServer.exists).Add(2, FRS2Model.FromServer.exists).Add(3, FRS2Model.FromServer.exists).Add(4, FRS2Model.FromServer.exists), new Microsoft.Modeling.Map<System.String,System.Int32>().Add("A", 1).Add("B", 2).Add("C", 3).Add("D", 4), new Microsoft.Modeling.Map<System.String,FRS2Model.ReplicationGroupTypes>().Add("A", FRS2Model.ReplicationGroupTypes.sysvol).Add("B", FRS2Model.ReplicationGroupTypes.protection).Add("C", FRS2Model.ReplicationGroupTypes.sysvol).Add("D", FRS2Model.ReplicationGroupTypes.sysvol), new Microsoft.Modeling.Map<System.Int32,System.String>().Add(1, "A").Add(2, "B").Add(3, "C").Add(4, "D"), new Microsoft.Modeling.Map<System.Int32,FRS2Model.accessLevels>().Add(1, FRS2Model.accessLevels.writeOnly).Add(2, FRS2Model.accessLevels.readWrite).Add(3, FRS2Model.accessLevels.readOnly).Add(4, FRS2Model.accessLevels.readWrite), new Microsoft.Modeling.Map<System.Int32,FRS2Model.connectionProperty>().Add(1, FRS2Model.connectionProperty.enabled).Add(2, FRS2Model.connectionProperty.disabled).Add(3, FRS2Model.connectionProperty.enabled).Add(4, FRS2Model.connectionProperty.enabled));
            this.Manager.Comment("reaching state \'S33\'");
            this.Manager.Comment("checking step \'return Initialization/ERROR_SUCCESS\'");
            this.Manager.Assert((temp95 == FRS2Model.error_status_t.ERROR_SUCCESS), String.Format("expected \'FRS2Model.error_status_t.ERROR_SUCCESS\', actual \'{0}\' (return of Initia" +
                        "lization, state S33)", TestManagerHelpers.Describe(temp95)));
            this.Manager.Comment("reaching state \'S72\'");
            FRS2Model.error_status_t temp96;
            this.Manager.Comment("executing step \'call CheckConnectivity(\"A\",1)\'");
            temp96 = this.IFRS2ManagedAdapterInstance.CheckConnectivity("A", 1);
            this.Manager.Checkpoint("MS-FRS2_R407");
            this.Manager.Checkpoint("MS-FRS2_R410");
            this.Manager.Checkpoint("MS-FRS2_R417");
            this.Manager.Comment("reaching state \'S100\'");
            this.Manager.Comment("checking step \'return CheckConnectivity/ERROR_SUCCESS\'");
            this.Manager.Assert((temp96 == FRS2Model.error_status_t.ERROR_SUCCESS), String.Format("expected \'FRS2Model.error_status_t.ERROR_SUCCESS\', actual \'{0}\' (return of CheckC" +
                        "onnectivity, state S100)", TestManagerHelpers.Describe(temp96)));
            this.Manager.Comment("reaching state \'S128\'");
            FRS2Model.ProtocolVersionReturned temp97;
            FRS2Model.UpstreamFlagValueReturned temp98;
            FRS2Model.error_status_t temp99;
            this.Manager.Comment("executing step \'call EstablishConnection(\"A\",1,FRS_COMMUNICATION_PROTOCOL_VERSION" +
                    "_LONGHORN_SERVER,out _,out _)\'");
            temp99 = this.IFRS2ManagedAdapterInstance.EstablishConnection("A", 1, FRS2Model.ProtocolVersion.FRS_COMMUNICATION_PROTOCOL_VERSION_LONGHORN_SERVER, out temp97, out temp98);
            this.Manager.Checkpoint("MS-FRS2_R37");
            this.Manager.Checkpoint("MS-FRS2_R436");
            this.Manager.Checkpoint("MS-FRS2_R439");
            this.Manager.Comment("reaching state \'S156\'");
            this.Manager.Comment("checking step \'return EstablishConnection/[out Valid,out Valid]:ERROR_SUCCESS\'");
            this.Manager.Assert((temp97 == FRS2Model.ProtocolVersionReturned.Valid), String.Format("expected \'FRS2Model.ProtocolVersionReturned.Valid\', actual \'{0}\' (upstreamProtoco" +
                        "lVersion of EstablishConnection, state S156)", TestManagerHelpers.Describe(temp97)));
            this.Manager.Assert((temp98 == FRS2Model.UpstreamFlagValueReturned.Valid), String.Format("expected \'FRS2Model.UpstreamFlagValueReturned.Valid\', actual \'{0}\' (upstreamFlags" +
                        " of EstablishConnection, state S156)", TestManagerHelpers.Describe(temp98)));
            this.Manager.Assert((temp99 == FRS2Model.error_status_t.ERROR_SUCCESS), String.Format("expected \'FRS2Model.error_status_t.ERROR_SUCCESS\', actual \'{0}\' (return of Establ" +
                        "ishConnection, state S156)", TestManagerHelpers.Describe(temp99)));
            this.Manager.Comment("reaching state \'S184\'");
            FRS2Model.error_status_t temp100;
            this.Manager.Comment("executing step \'call EstablishSession(1,2)\'");
            temp100 = this.IFRS2ManagedAdapterInstance.EstablishSession(1, 2);
            this.Manager.Comment("reaching state \'S211\'");
            this.Manager.Comment("checking step \'return EstablishSession/ERROR_FAIL\'");
            this.Manager.Assert((temp100 == FRS2Model.error_status_t.ERROR_FAIL), String.Format("expected \'FRS2Model.error_status_t.ERROR_FAIL\', actual \'{0}\' (return of Establish" +
                        "Session, state S211)", TestManagerHelpers.Describe(temp100)));
            TCtestScenario7S229();
            this.Manager.EndTest();
        }
        #endregion
        
        #region Test Starting in S34
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        [Microsoft.VisualStudio.TestTools.UnitTesting.DescriptionAttribute("MS-FRS2_R407, MS-FRS2_R410, MS-FRS2_R417, MS-FRS2_R437, MS-FRS2_R443")]
        public virtual void FRS2_TCtestScenario7S34()
        {
            this.Manager.BeginTest("TCtestScenario7S34");
            this.Manager.Comment("reaching state \'S34\'");
            FRS2Model.error_status_t temp101;
            this.Manager.Comment(@"executing step 'call Initialization(Windows7,Map{1=>enabled,2=>enabled,3=>enabled,4=>disabled},Map{1=>exists,2=>exists,3=>exists,4=>exists},Map{""A""=>1,""B""=>2,""C""=>3,""D""=>4},Map{""A""=>sysvol,""B""=>protection,""C""=>sysvol,""D""=>sysvol},Map{1=>""A"",2=>""B"",3=>""C"",4=>""D""},Map{1=>writeOnly,2=>readWrite,3=>readOnly,4=>readWrite},Map{1=>enabled,2=>disabled,3=>enabled,4=>enabled})'");
            temp101 = this.IFRS2ManagedAdapterInstance.Initialization(FRS2Model.OSVersion.Windows7, new Microsoft.Modeling.Map<System.Int32,FRS2Model.connectionProperty>().Add(1, FRS2Model.connectionProperty.enabled).Add(2, FRS2Model.connectionProperty.enabled).Add(3, FRS2Model.connectionProperty.enabled).Add(4, FRS2Model.connectionProperty.disabled), new Microsoft.Modeling.Map<System.Int32,FRS2Model.FromServer>().Add(1, FRS2Model.FromServer.exists).Add(2, FRS2Model.FromServer.exists).Add(3, FRS2Model.FromServer.exists).Add(4, FRS2Model.FromServer.exists), new Microsoft.Modeling.Map<System.String,System.Int32>().Add("A", 1).Add("B", 2).Add("C", 3).Add("D", 4), new Microsoft.Modeling.Map<System.String,FRS2Model.ReplicationGroupTypes>().Add("A", FRS2Model.ReplicationGroupTypes.sysvol).Add("B", FRS2Model.ReplicationGroupTypes.protection).Add("C", FRS2Model.ReplicationGroupTypes.sysvol).Add("D", FRS2Model.ReplicationGroupTypes.sysvol), new Microsoft.Modeling.Map<System.Int32,System.String>().Add(1, "A").Add(2, "B").Add(3, "C").Add(4, "D"), new Microsoft.Modeling.Map<System.Int32,FRS2Model.accessLevels>().Add(1, FRS2Model.accessLevels.writeOnly).Add(2, FRS2Model.accessLevels.readWrite).Add(3, FRS2Model.accessLevels.readOnly).Add(4, FRS2Model.accessLevels.readWrite), new Microsoft.Modeling.Map<System.Int32,FRS2Model.connectionProperty>().Add(1, FRS2Model.connectionProperty.enabled).Add(2, FRS2Model.connectionProperty.disabled).Add(3, FRS2Model.connectionProperty.enabled).Add(4, FRS2Model.connectionProperty.enabled));
            this.Manager.Comment("reaching state \'S35\'");
            this.Manager.Comment("checking step \'return Initialization/ERROR_SUCCESS\'");
            this.Manager.Assert((temp101 == FRS2Model.error_status_t.ERROR_SUCCESS), String.Format("expected \'FRS2Model.error_status_t.ERROR_SUCCESS\', actual \'{0}\' (return of Initia" +
                        "lization, state S35)", TestManagerHelpers.Describe(temp101)));
            this.Manager.Comment("reaching state \'S73\'");
            FRS2Model.error_status_t temp102;
            this.Manager.Comment("executing step \'call CheckConnectivity(\"A\",1)\'");
            temp102 = this.IFRS2ManagedAdapterInstance.CheckConnectivity("A", 1);
            this.Manager.Checkpoint("MS-FRS2_R407");
            this.Manager.Checkpoint("MS-FRS2_R410");
            this.Manager.Checkpoint("MS-FRS2_R417");
            this.Manager.Comment("reaching state \'S101\'");
            this.Manager.Comment("checking step \'return CheckConnectivity/ERROR_SUCCESS\'");
            this.Manager.Assert((temp102 == FRS2Model.error_status_t.ERROR_SUCCESS), String.Format("expected \'FRS2Model.error_status_t.ERROR_SUCCESS\', actual \'{0}\' (return of CheckC" +
                        "onnectivity, state S101)", TestManagerHelpers.Describe(temp102)));
            this.Manager.Comment("reaching state \'S129\'");
            FRS2Model.ProtocolVersionReturned temp103;
            FRS2Model.UpstreamFlagValueReturned temp104;
            FRS2Model.error_status_t temp105;
            this.Manager.Comment("executing step \'call EstablishConnection(\"B\",2,FRS_COMMUNICATION_PROTOCOL_VERSION" +
                    "_LONGHORN_SERVER,out _,out _)\'");
            temp105 = this.IFRS2ManagedAdapterInstance.EstablishConnection("B", 2, FRS2Model.ProtocolVersion.FRS_COMMUNICATION_PROTOCOL_VERSION_LONGHORN_SERVER, out temp103, out temp104);
            this.Manager.Checkpoint("MS-FRS2_R437");
            this.Manager.Checkpoint("MS-FRS2_R443");
            this.Manager.Comment("reaching state \'S157\'");
            this.Manager.Comment("checking step \'return EstablishConnection/[out Invalid,out Invalid]:ERROR_FAIL\'");
            this.Manager.Assert((temp103 == FRS2Model.ProtocolVersionReturned.Invalid), String.Format("expected \'FRS2Model.ProtocolVersionReturned.Invalid\', actual \'{0}\' (upstreamProto" +
                        "colVersion of EstablishConnection, state S157)", TestManagerHelpers.Describe(temp103)));
            this.Manager.Assert((temp104 == FRS2Model.UpstreamFlagValueReturned.Invalid), String.Format("expected \'FRS2Model.UpstreamFlagValueReturned.Invalid\', actual \'{0}\' (upstreamFla" +
                        "gs of EstablishConnection, state S157)", TestManagerHelpers.Describe(temp104)));
            this.Manager.Assert((temp105 == FRS2Model.error_status_t.ERROR_FAIL), String.Format("expected \'FRS2Model.error_status_t.ERROR_FAIL\', actual \'{0}\' (return of Establish" +
                        "Connection, state S157)", TestManagerHelpers.Describe(temp105)));
            TCtestScenario7S185();
            this.Manager.EndTest();
        }
        
        private void TCtestScenario7S185() {
            this.Manager.Comment("reaching state \'S185\'");
        }
        #endregion
        
        #region Test Starting in S36
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        [Microsoft.VisualStudio.TestTools.UnitTesting.DescriptionAttribute("MS-FRS2_R407, MS-FRS2_R410, MS-FRS2_R417, MS-FRS2_R37, MS-FRS2_R436, MS-FRS2_R439" +
            ", MS-FRS2_R464, MS-FRS2_R460")]
        public virtual void FRS2_TCtestScenario7S36() {
            this.Manager.BeginTest("TCtestScenario7S36");
            this.Manager.Comment("reaching state \'S36\'");
            FRS2Model.error_status_t temp106;
            this.Manager.Comment(@"executing step 'call Initialization(Windows7,Map{1=>enabled,2=>enabled,3=>enabled,4=>disabled},Map{1=>exists,2=>exists,3=>exists,4=>exists},Map{""A""=>1,""B""=>2,""C""=>3,""D""=>4},Map{""A""=>sysvol,""B""=>protection,""C""=>sysvol,""D""=>sysvol},Map{1=>""A"",2=>""B"",3=>""C"",4=>""D""},Map{1=>writeOnly,2=>readWrite,3=>readOnly,4=>readWrite},Map{1=>enabled,2=>disabled,3=>enabled,4=>enabled})'");
            temp106 = this.IFRS2ManagedAdapterInstance.Initialization(FRS2Model.OSVersion.Windows7, new Microsoft.Modeling.Map<System.Int32,FRS2Model.connectionProperty>().Add(1, FRS2Model.connectionProperty.enabled).Add(2, FRS2Model.connectionProperty.enabled).Add(3, FRS2Model.connectionProperty.enabled).Add(4, FRS2Model.connectionProperty.disabled), new Microsoft.Modeling.Map<System.Int32,FRS2Model.FromServer>().Add(1, FRS2Model.FromServer.exists).Add(2, FRS2Model.FromServer.exists).Add(3, FRS2Model.FromServer.exists).Add(4, FRS2Model.FromServer.exists), new Microsoft.Modeling.Map<System.String,System.Int32>().Add("A", 1).Add("B", 2).Add("C", 3).Add("D", 4), new Microsoft.Modeling.Map<System.String,FRS2Model.ReplicationGroupTypes>().Add("A", FRS2Model.ReplicationGroupTypes.sysvol).Add("B", FRS2Model.ReplicationGroupTypes.protection).Add("C", FRS2Model.ReplicationGroupTypes.sysvol).Add("D", FRS2Model.ReplicationGroupTypes.sysvol), new Microsoft.Modeling.Map<System.Int32,System.String>().Add(1, "A").Add(2, "B").Add(3, "C").Add(4, "D"), new Microsoft.Modeling.Map<System.Int32,FRS2Model.accessLevels>().Add(1, FRS2Model.accessLevels.writeOnly).Add(2, FRS2Model.accessLevels.readWrite).Add(3, FRS2Model.accessLevels.readOnly).Add(4, FRS2Model.accessLevels.readWrite), new Microsoft.Modeling.Map<System.Int32,FRS2Model.connectionProperty>().Add(1, FRS2Model.connectionProperty.enabled).Add(2, FRS2Model.connectionProperty.disabled).Add(3, FRS2Model.connectionProperty.enabled).Add(4, FRS2Model.connectionProperty.enabled));
            this.Manager.Comment("reaching state \'S37\'");
            this.Manager.Comment("checking step \'return Initialization/ERROR_SUCCESS\'");
            this.Manager.Assert((temp106 == FRS2Model.error_status_t.ERROR_SUCCESS), String.Format("expected \'FRS2Model.error_status_t.ERROR_SUCCESS\', actual \'{0}\' (return of Initia" +
                        "lization, state S37)", TestManagerHelpers.Describe(temp106)));
            this.Manager.Comment("reaching state \'S74\'");
            FRS2Model.error_status_t temp107;
            this.Manager.Comment("executing step \'call CheckConnectivity(\"A\",1)\'");
            temp107 = this.IFRS2ManagedAdapterInstance.CheckConnectivity("A", 1);
            this.Manager.Checkpoint("MS-FRS2_R407");
            this.Manager.Checkpoint("MS-FRS2_R410");
            this.Manager.Checkpoint("MS-FRS2_R417");
            this.Manager.Comment("reaching state \'S102\'");
            this.Manager.Comment("checking step \'return CheckConnectivity/ERROR_SUCCESS\'");
            this.Manager.Assert((temp107 == FRS2Model.error_status_t.ERROR_SUCCESS), String.Format("expected \'FRS2Model.error_status_t.ERROR_SUCCESS\', actual \'{0}\' (return of CheckC" +
                        "onnectivity, state S102)", TestManagerHelpers.Describe(temp107)));
            this.Manager.Comment("reaching state \'S130\'");
            FRS2Model.ProtocolVersionReturned temp108;
            FRS2Model.UpstreamFlagValueReturned temp109;
            FRS2Model.error_status_t temp110;
            this.Manager.Comment("executing step \'call EstablishConnection(\"C\",3,FRS_COMMUNICATION_PROTOCOL_VERSION" +
                    "_LONGHORN_SERVER,out _,out _)\'");
            temp110 = this.IFRS2ManagedAdapterInstance.EstablishConnection("C", 3, FRS2Model.ProtocolVersion.FRS_COMMUNICATION_PROTOCOL_VERSION_LONGHORN_SERVER, out temp108, out temp109);
            this.Manager.Checkpoint("MS-FRS2_R37");
            this.Manager.Checkpoint("MS-FRS2_R436");
            this.Manager.Checkpoint("MS-FRS2_R439");
            this.Manager.Comment("reaching state \'S158\'");
            this.Manager.Comment("checking step \'return EstablishConnection/[out Valid,out Valid]:ERROR_SUCCESS\'");
            this.Manager.Assert((temp108 == FRS2Model.ProtocolVersionReturned.Valid), String.Format("expected \'FRS2Model.ProtocolVersionReturned.Valid\', actual \'{0}\' (upstreamProtoco" +
                        "lVersion of EstablishConnection, state S158)", TestManagerHelpers.Describe(temp108)));
            this.Manager.Assert((temp109 == FRS2Model.UpstreamFlagValueReturned.Valid), String.Format("expected \'FRS2Model.UpstreamFlagValueReturned.Valid\', actual \'{0}\' (upstreamFlags" +
                        " of EstablishConnection, state S158)", TestManagerHelpers.Describe(temp109)));
            this.Manager.Assert((temp110 == FRS2Model.error_status_t.ERROR_SUCCESS), String.Format("expected \'FRS2Model.error_status_t.ERROR_SUCCESS\', actual \'{0}\' (return of Establ" +
                        "ishConnection, state S158)", TestManagerHelpers.Describe(temp110)));
            this.Manager.Comment("reaching state \'S186\'");
            FRS2Model.error_status_t temp111;
            this.Manager.Comment("executing step \'call EstablishSession(3,3)\'");
            temp111 = this.IFRS2ManagedAdapterInstance.EstablishSession(3, 3);
            this.Manager.Checkpoint("MS-FRS2_R464");
            this.Manager.Checkpoint("MS-FRS2_R460");
            this.Manager.Comment("reaching state \'S212\'");
            this.Manager.Comment("checking step \'return EstablishSession/FRS_ERROR_CONTENTSET_READ_ONLY\'");
            this.Manager.Assert((temp111 == FRS2Model.error_status_t.FRS_ERROR_CONTENTSET_READ_ONLY), String.Format("expected \'FRS2Model.error_status_t.FRS_ERROR_CONTENTSET_READ_ONLY\', actual \'{0}\' " +
                        "(return of EstablishSession, state S212)", TestManagerHelpers.Describe(temp111)));
            TCtestScenario7S225();
            this.Manager.EndTest();
        }
        #endregion
        
        #region Test Starting in S38
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        [Microsoft.VisualStudio.TestTools.UnitTesting.DescriptionAttribute("MS-FRS2_R407, MS-FRS2_R410, MS-FRS2_R417, MS-FRS2_R37, MS-FRS2_R436, MS-FRS2_R439" +
            ", MS-FRS2_R464, MS-FRS2_R460")]
        public virtual void FRS2_TCtestScenario7S38() {
            this.Manager.BeginTest("TCtestScenario7S38");
            this.Manager.Comment("reaching state \'S38\'");
            FRS2Model.error_status_t temp112;
            this.Manager.Comment(@"executing step 'call Initialization(Windows7,Map{1=>enabled,2=>enabled,3=>enabled,4=>disabled},Map{1=>exists,2=>exists,3=>exists,4=>exists},Map{""A""=>1,""B""=>2,""C""=>3,""D""=>4},Map{""A""=>sysvol,""B""=>protection,""C""=>sysvol,""D""=>sysvol},Map{1=>""A"",2=>""B"",3=>""C"",4=>""D""},Map{1=>writeOnly,2=>readWrite,3=>readOnly,4=>readWrite},Map{1=>enabled,2=>disabled,3=>enabled,4=>enabled})'");
            temp112 = this.IFRS2ManagedAdapterInstance.Initialization(FRS2Model.OSVersion.Windows7, new Microsoft.Modeling.Map<System.Int32,FRS2Model.connectionProperty>().Add(1, FRS2Model.connectionProperty.enabled).Add(2, FRS2Model.connectionProperty.enabled).Add(3, FRS2Model.connectionProperty.enabled).Add(4, FRS2Model.connectionProperty.disabled), new Microsoft.Modeling.Map<System.Int32,FRS2Model.FromServer>().Add(1, FRS2Model.FromServer.exists).Add(2, FRS2Model.FromServer.exists).Add(3, FRS2Model.FromServer.exists).Add(4, FRS2Model.FromServer.exists), new Microsoft.Modeling.Map<System.String,System.Int32>().Add("A", 1).Add("B", 2).Add("C", 3).Add("D", 4), new Microsoft.Modeling.Map<System.String,FRS2Model.ReplicationGroupTypes>().Add("A", FRS2Model.ReplicationGroupTypes.sysvol).Add("B", FRS2Model.ReplicationGroupTypes.protection).Add("C", FRS2Model.ReplicationGroupTypes.sysvol).Add("D", FRS2Model.ReplicationGroupTypes.sysvol), new Microsoft.Modeling.Map<System.Int32,System.String>().Add(1, "A").Add(2, "B").Add(3, "C").Add(4, "D"), new Microsoft.Modeling.Map<System.Int32,FRS2Model.accessLevels>().Add(1, FRS2Model.accessLevels.writeOnly).Add(2, FRS2Model.accessLevels.readWrite).Add(3, FRS2Model.accessLevels.readOnly).Add(4, FRS2Model.accessLevels.readWrite), new Microsoft.Modeling.Map<System.Int32,FRS2Model.connectionProperty>().Add(1, FRS2Model.connectionProperty.enabled).Add(2, FRS2Model.connectionProperty.disabled).Add(3, FRS2Model.connectionProperty.enabled).Add(4, FRS2Model.connectionProperty.enabled));
            this.Manager.Comment("reaching state \'S39\'");
            this.Manager.Comment("checking step \'return Initialization/ERROR_SUCCESS\'");
            this.Manager.Assert((temp112 == FRS2Model.error_status_t.ERROR_SUCCESS), String.Format("expected \'FRS2Model.error_status_t.ERROR_SUCCESS\', actual \'{0}\' (return of Initia" +
                        "lization, state S39)", TestManagerHelpers.Describe(temp112)));
            this.Manager.Comment("reaching state \'S75\'");
            FRS2Model.error_status_t temp113;
            this.Manager.Comment("executing step \'call CheckConnectivity(\"A\",1)\'");
            temp113 = this.IFRS2ManagedAdapterInstance.CheckConnectivity("A", 1);
            this.Manager.Checkpoint("MS-FRS2_R407");
            this.Manager.Checkpoint("MS-FRS2_R410");
            this.Manager.Checkpoint("MS-FRS2_R417");
            this.Manager.Comment("reaching state \'S103\'");
            this.Manager.Comment("checking step \'return CheckConnectivity/ERROR_SUCCESS\'");
            this.Manager.Assert((temp113 == FRS2Model.error_status_t.ERROR_SUCCESS), String.Format("expected \'FRS2Model.error_status_t.ERROR_SUCCESS\', actual \'{0}\' (return of CheckC" +
                        "onnectivity, state S103)", TestManagerHelpers.Describe(temp113)));
            this.Manager.Comment("reaching state \'S131\'");
            FRS2Model.ProtocolVersionReturned temp114;
            FRS2Model.UpstreamFlagValueReturned temp115;
            FRS2Model.error_status_t temp116;
            this.Manager.Comment("executing step \'call EstablishConnection(\"A\",1,FRS_COMMUNICATION_PROTOCOL_VERSION" +
                    "_LONGHORN_SERVER,out _,out _)\'");
            temp116 = this.IFRS2ManagedAdapterInstance.EstablishConnection("A", 1, FRS2Model.ProtocolVersion.FRS_COMMUNICATION_PROTOCOL_VERSION_LONGHORN_SERVER, out temp114, out temp115);
            this.Manager.Checkpoint("MS-FRS2_R37");
            this.Manager.Checkpoint("MS-FRS2_R436");
            this.Manager.Checkpoint("MS-FRS2_R439");
            this.Manager.Comment("reaching state \'S159\'");
            this.Manager.Comment("checking step \'return EstablishConnection/[out Valid,out Valid]:ERROR_SUCCESS\'");
            this.Manager.Assert((temp114 == FRS2Model.ProtocolVersionReturned.Valid), String.Format("expected \'FRS2Model.ProtocolVersionReturned.Valid\', actual \'{0}\' (upstreamProtoco" +
                        "lVersion of EstablishConnection, state S159)", TestManagerHelpers.Describe(temp114)));
            this.Manager.Assert((temp115 == FRS2Model.UpstreamFlagValueReturned.Valid), String.Format("expected \'FRS2Model.UpstreamFlagValueReturned.Valid\', actual \'{0}\' (upstreamFlags" +
                        " of EstablishConnection, state S159)", TestManagerHelpers.Describe(temp115)));
            this.Manager.Assert((temp116 == FRS2Model.error_status_t.ERROR_SUCCESS), String.Format("expected \'FRS2Model.error_status_t.ERROR_SUCCESS\', actual \'{0}\' (return of Establ" +
                        "ishConnection, state S159)", TestManagerHelpers.Describe(temp116)));
            this.Manager.Comment("reaching state \'S187\'");
            FRS2Model.error_status_t temp117;
            this.Manager.Comment("executing step \'call EstablishSession(3,3)\'");
            temp117 = this.IFRS2ManagedAdapterInstance.EstablishSession(3, 3);
            this.Manager.Checkpoint("MS-FRS2_R464");
            this.Manager.Checkpoint("MS-FRS2_R460");
            this.Manager.Comment("reaching state \'S213\'");
            this.Manager.Comment("checking step \'return EstablishSession/FRS_ERROR_CONTENTSET_READ_ONLY\'");
            this.Manager.Assert((temp117 == FRS2Model.error_status_t.FRS_ERROR_CONTENTSET_READ_ONLY), String.Format("expected \'FRS2Model.error_status_t.FRS_ERROR_CONTENTSET_READ_ONLY\', actual \'{0}\' " +
                        "(return of EstablishSession, state S213)", TestManagerHelpers.Describe(temp117)));
            TCtestScenario7S229();
            this.Manager.EndTest();
        }
        #endregion
        
        #region Test Starting in S40
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        [Microsoft.VisualStudio.TestTools.UnitTesting.DescriptionAttribute("MS-FRS2_R407, MS-FRS2_R410, MS-FRS2_R417, MS-FRS2_R437, MS-FRS2_R445")]
        public virtual void FRS2_TCtestScenario7S40() {
            this.Manager.BeginTest("TCtestScenario7S40");
            this.Manager.Comment("reaching state \'S40\'");
            FRS2Model.error_status_t temp118;
            this.Manager.Comment(@"executing step 'call Initialization(Windows7,Map{1=>enabled,2=>enabled,3=>enabled,4=>disabled},Map{1=>exists,2=>exists,3=>exists,4=>exists},Map{""A""=>1,""B""=>2,""C""=>3,""D""=>4},Map{""A""=>sysvol,""B""=>protection,""C""=>sysvol,""D""=>sysvol},Map{1=>""A"",2=>""B"",3=>""C"",4=>""D""},Map{1=>writeOnly,2=>readWrite,3=>readOnly,4=>readWrite},Map{1=>enabled,2=>disabled,3=>enabled,4=>enabled})'");
            temp118 = this.IFRS2ManagedAdapterInstance.Initialization(FRS2Model.OSVersion.Windows7, new Microsoft.Modeling.Map<System.Int32,FRS2Model.connectionProperty>().Add(1, FRS2Model.connectionProperty.enabled).Add(2, FRS2Model.connectionProperty.enabled).Add(3, FRS2Model.connectionProperty.enabled).Add(4, FRS2Model.connectionProperty.disabled), new Microsoft.Modeling.Map<System.Int32,FRS2Model.FromServer>().Add(1, FRS2Model.FromServer.exists).Add(2, FRS2Model.FromServer.exists).Add(3, FRS2Model.FromServer.exists).Add(4, FRS2Model.FromServer.exists), new Microsoft.Modeling.Map<System.String,System.Int32>().Add("A", 1).Add("B", 2).Add("C", 3).Add("D", 4), new Microsoft.Modeling.Map<System.String,FRS2Model.ReplicationGroupTypes>().Add("A", FRS2Model.ReplicationGroupTypes.sysvol).Add("B", FRS2Model.ReplicationGroupTypes.protection).Add("C", FRS2Model.ReplicationGroupTypes.sysvol).Add("D", FRS2Model.ReplicationGroupTypes.sysvol), new Microsoft.Modeling.Map<System.Int32,System.String>().Add(1, "A").Add(2, "B").Add(3, "C").Add(4, "D"), new Microsoft.Modeling.Map<System.Int32,FRS2Model.accessLevels>().Add(1, FRS2Model.accessLevels.writeOnly).Add(2, FRS2Model.accessLevels.readWrite).Add(3, FRS2Model.accessLevels.readOnly).Add(4, FRS2Model.accessLevels.readWrite), new Microsoft.Modeling.Map<System.Int32,FRS2Model.connectionProperty>().Add(1, FRS2Model.connectionProperty.enabled).Add(2, FRS2Model.connectionProperty.disabled).Add(3, FRS2Model.connectionProperty.enabled).Add(4, FRS2Model.connectionProperty.enabled));
            this.Manager.Comment("reaching state \'S41\'");
            this.Manager.Comment("checking step \'return Initialization/ERROR_SUCCESS\'");
            this.Manager.Assert((temp118 == FRS2Model.error_status_t.ERROR_SUCCESS), String.Format("expected \'FRS2Model.error_status_t.ERROR_SUCCESS\', actual \'{0}\' (return of Initia" +
                        "lization, state S41)", TestManagerHelpers.Describe(temp118)));
            this.Manager.Comment("reaching state \'S76\'");
            FRS2Model.error_status_t temp119;
            this.Manager.Comment("executing step \'call CheckConnectivity(\"A\",1)\'");
            temp119 = this.IFRS2ManagedAdapterInstance.CheckConnectivity("A", 1);
            this.Manager.Checkpoint("MS-FRS2_R407");
            this.Manager.Checkpoint("MS-FRS2_R410");
            this.Manager.Checkpoint("MS-FRS2_R417");
            this.Manager.Comment("reaching state \'S104\'");
            this.Manager.Comment("checking step \'return CheckConnectivity/ERROR_SUCCESS\'");
            this.Manager.Assert((temp119 == FRS2Model.error_status_t.ERROR_SUCCESS), String.Format("expected \'FRS2Model.error_status_t.ERROR_SUCCESS\', actual \'{0}\' (return of CheckC" +
                        "onnectivity, state S104)", TestManagerHelpers.Describe(temp119)));
            this.Manager.Comment("reaching state \'S132\'");
            FRS2Model.ProtocolVersionReturned temp120;
            FRS2Model.UpstreamFlagValueReturned temp121;
            FRS2Model.error_status_t temp122;
            this.Manager.Comment("executing step \'call EstablishConnection(\"D\",4,FRS_COMMUNICATION_PROTOCOL_VERSION" +
                    "_LONGHORN_SERVER,out _,out _)\'");
            temp122 = this.IFRS2ManagedAdapterInstance.EstablishConnection("D", 4, FRS2Model.ProtocolVersion.FRS_COMMUNICATION_PROTOCOL_VERSION_LONGHORN_SERVER, out temp120, out temp121);
            this.Manager.Checkpoint("MS-FRS2_R437");
            this.Manager.Checkpoint("MS-FRS2_R445");
            this.Manager.Comment("reaching state \'S160\'");
            this.Manager.Comment("checking step \'return EstablishConnection/[out Invalid,out Invalid]:ERROR_FAIL\'");
            this.Manager.Assert((temp120 == FRS2Model.ProtocolVersionReturned.Invalid), String.Format("expected \'FRS2Model.ProtocolVersionReturned.Invalid\', actual \'{0}\' (upstreamProto" +
                        "colVersion of EstablishConnection, state S160)", TestManagerHelpers.Describe(temp120)));
            this.Manager.Assert((temp121 == FRS2Model.UpstreamFlagValueReturned.Invalid), String.Format("expected \'FRS2Model.UpstreamFlagValueReturned.Invalid\', actual \'{0}\' (upstreamFla" +
                        "gs of EstablishConnection, state S160)", TestManagerHelpers.Describe(temp121)));
            this.Manager.Assert((temp122 == FRS2Model.error_status_t.ERROR_FAIL), String.Format("expected \'FRS2Model.error_status_t.ERROR_FAIL\', actual \'{0}\' (return of Establish" +
                        "Connection, state S160)", TestManagerHelpers.Describe(temp122)));
            TCtestScenario7S185();
            this.Manager.EndTest();
        }
        #endregion
        
        #region Test Starting in S42
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        [Microsoft.VisualStudio.TestTools.UnitTesting.DescriptionAttribute("MS-FRS2_R408, MS-FRS2_R413, MS-FRS2_R415, MS-FRS2_R414, MS-FRS2_R437, MS-FRS2_R44" +
            "2")]
        public virtual void FRS2_TCtestScenario7S42() {
            this.Manager.BeginTest("TCtestScenario7S42");
            this.Manager.Comment("reaching state \'S42\'");
            FRS2Model.error_status_t temp123;
            this.Manager.Comment(@"executing step 'call Initialization(Windows7,Map{1=>enabled,2=>enabled,3=>enabled,4=>disabled},Map{1=>exists,2=>exists,3=>exists,4=>exists},Map{""A""=>1,""B""=>2,""C""=>3,""D""=>4},Map{""A""=>sysvol,""B""=>protection,""C""=>sysvol,""D""=>sysvol},Map{1=>""A"",2=>""B"",3=>""C"",4=>""D""},Map{1=>writeOnly,2=>readWrite,3=>readOnly,4=>readWrite},Map{1=>enabled,2=>disabled,3=>enabled,4=>enabled})'");
            temp123 = this.IFRS2ManagedAdapterInstance.Initialization(FRS2Model.OSVersion.Windows7, new Microsoft.Modeling.Map<System.Int32,FRS2Model.connectionProperty>().Add(1, FRS2Model.connectionProperty.enabled).Add(2, FRS2Model.connectionProperty.enabled).Add(3, FRS2Model.connectionProperty.enabled).Add(4, FRS2Model.connectionProperty.disabled), new Microsoft.Modeling.Map<System.Int32,FRS2Model.FromServer>().Add(1, FRS2Model.FromServer.exists).Add(2, FRS2Model.FromServer.exists).Add(3, FRS2Model.FromServer.exists).Add(4, FRS2Model.FromServer.exists), new Microsoft.Modeling.Map<System.String,System.Int32>().Add("A", 1).Add("B", 2).Add("C", 3).Add("D", 4), new Microsoft.Modeling.Map<System.String,FRS2Model.ReplicationGroupTypes>().Add("A", FRS2Model.ReplicationGroupTypes.sysvol).Add("B", FRS2Model.ReplicationGroupTypes.protection).Add("C", FRS2Model.ReplicationGroupTypes.sysvol).Add("D", FRS2Model.ReplicationGroupTypes.sysvol), new Microsoft.Modeling.Map<System.Int32,System.String>().Add(1, "A").Add(2, "B").Add(3, "C").Add(4, "D"), new Microsoft.Modeling.Map<System.Int32,FRS2Model.accessLevels>().Add(1, FRS2Model.accessLevels.writeOnly).Add(2, FRS2Model.accessLevels.readWrite).Add(3, FRS2Model.accessLevels.readOnly).Add(4, FRS2Model.accessLevels.readWrite), new Microsoft.Modeling.Map<System.Int32,FRS2Model.connectionProperty>().Add(1, FRS2Model.connectionProperty.enabled).Add(2, FRS2Model.connectionProperty.disabled).Add(3, FRS2Model.connectionProperty.enabled).Add(4, FRS2Model.connectionProperty.enabled));
            this.Manager.Comment("reaching state \'S43\'");
            this.Manager.Comment("checking step \'return Initialization/ERROR_SUCCESS\'");
            this.Manager.Assert((temp123 == FRS2Model.error_status_t.ERROR_SUCCESS), String.Format("expected \'FRS2Model.error_status_t.ERROR_SUCCESS\', actual \'{0}\' (return of Initia" +
                        "lization, state S43)", TestManagerHelpers.Describe(temp123)));
            this.Manager.Comment("reaching state \'S77\'");
            FRS2Model.error_status_t temp124;
            this.Manager.Comment("executing step \'call CheckConnectivity(\"D\",4)\'");
            temp124 = this.IFRS2ManagedAdapterInstance.CheckConnectivity("D", 4);
            this.Manager.Checkpoint("MS-FRS2_R408");
            this.Manager.Checkpoint("MS-FRS2_R413");
            this.Manager.Checkpoint("MS-FRS2_R415");
            this.Manager.Checkpoint("MS-FRS2_R414");
            this.Manager.Comment("reaching state \'S105\'");
            this.Manager.Comment("checking step \'return CheckConnectivity/ERROR_FAIL\'");
            this.Manager.Assert((temp124 == FRS2Model.error_status_t.ERROR_FAIL), String.Format("expected \'FRS2Model.error_status_t.ERROR_FAIL\', actual \'{0}\' (return of CheckConn" +
                        "ectivity, state S105)", TestManagerHelpers.Describe(temp124)));
            this.Manager.Comment("reaching state \'S133\'");
            FRS2Model.ProtocolVersionReturned temp125;
            FRS2Model.UpstreamFlagValueReturned temp126;
            FRS2Model.error_status_t temp127;
            this.Manager.Comment("executing step \'call EstablishConnection(\"A\",2,FRS_COMMUNICATION_PROTOCOL_VERSION" +
                    "_LONGHORN_SERVER,out _,out _)\'");
            temp127 = this.IFRS2ManagedAdapterInstance.EstablishConnection("A", 2, FRS2Model.ProtocolVersion.FRS_COMMUNICATION_PROTOCOL_VERSION_LONGHORN_SERVER, out temp125, out temp126);
            this.Manager.Checkpoint("MS-FRS2_R437");
            this.Manager.Checkpoint("MS-FRS2_R442");
            this.Manager.Comment("reaching state \'S161\'");
            this.Manager.Comment("checking step \'return EstablishConnection/[out Invalid,out Invalid]:ERROR_FAIL\'");
            this.Manager.Assert((temp125 == FRS2Model.ProtocolVersionReturned.Invalid), String.Format("expected \'FRS2Model.ProtocolVersionReturned.Invalid\', actual \'{0}\' (upstreamProto" +
                        "colVersion of EstablishConnection, state S161)", TestManagerHelpers.Describe(temp125)));
            this.Manager.Assert((temp126 == FRS2Model.UpstreamFlagValueReturned.Invalid), String.Format("expected \'FRS2Model.UpstreamFlagValueReturned.Invalid\', actual \'{0}\' (upstreamFla" +
                        "gs of EstablishConnection, state S161)", TestManagerHelpers.Describe(temp126)));
            this.Manager.Assert((temp127 == FRS2Model.error_status_t.ERROR_FAIL), String.Format("expected \'FRS2Model.error_status_t.ERROR_FAIL\', actual \'{0}\' (return of Establish" +
                        "Connection, state S161)", TestManagerHelpers.Describe(temp127)));
            TCtestScenario7S168();
            this.Manager.EndTest();
        }
        #endregion
        
        #region Test Starting in S44
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        [Microsoft.VisualStudio.TestTools.UnitTesting.DescriptionAttribute("MS-FRS2_R408, MS-FRS2_R413, MS-FRS2_R415, MS-FRS2_R414, MS-FRS2_R37, MS-FRS2_R436" +
            ", MS-FRS2_R439, MS-FRS2_R464, MS-FRS2_R460")]
        public virtual void FRS2_TCtestScenario7S44() {
            this.Manager.BeginTest("TCtestScenario7S44");
            this.Manager.Comment("reaching state \'S44\'");
            FRS2Model.error_status_t temp128;
            this.Manager.Comment(@"executing step 'call Initialization(Windows7,Map{1=>enabled,2=>enabled,3=>enabled,4=>disabled},Map{1=>exists,2=>exists,3=>exists,4=>exists},Map{""A""=>1,""B""=>2,""C""=>3,""D""=>4},Map{""A""=>sysvol,""B""=>protection,""C""=>sysvol,""D""=>sysvol},Map{1=>""A"",2=>""B"",3=>""C"",4=>""D""},Map{1=>writeOnly,2=>readWrite,3=>readOnly,4=>readWrite},Map{1=>enabled,2=>disabled,3=>enabled,4=>enabled})'");
            temp128 = this.IFRS2ManagedAdapterInstance.Initialization(FRS2Model.OSVersion.Windows7, new Microsoft.Modeling.Map<System.Int32,FRS2Model.connectionProperty>().Add(1, FRS2Model.connectionProperty.enabled).Add(2, FRS2Model.connectionProperty.enabled).Add(3, FRS2Model.connectionProperty.enabled).Add(4, FRS2Model.connectionProperty.disabled), new Microsoft.Modeling.Map<System.Int32,FRS2Model.FromServer>().Add(1, FRS2Model.FromServer.exists).Add(2, FRS2Model.FromServer.exists).Add(3, FRS2Model.FromServer.exists).Add(4, FRS2Model.FromServer.exists), new Microsoft.Modeling.Map<System.String,System.Int32>().Add("A", 1).Add("B", 2).Add("C", 3).Add("D", 4), new Microsoft.Modeling.Map<System.String,FRS2Model.ReplicationGroupTypes>().Add("A", FRS2Model.ReplicationGroupTypes.sysvol).Add("B", FRS2Model.ReplicationGroupTypes.protection).Add("C", FRS2Model.ReplicationGroupTypes.sysvol).Add("D", FRS2Model.ReplicationGroupTypes.sysvol), new Microsoft.Modeling.Map<System.Int32,System.String>().Add(1, "A").Add(2, "B").Add(3, "C").Add(4, "D"), new Microsoft.Modeling.Map<System.Int32,FRS2Model.accessLevels>().Add(1, FRS2Model.accessLevels.writeOnly).Add(2, FRS2Model.accessLevels.readWrite).Add(3, FRS2Model.accessLevels.readOnly).Add(4, FRS2Model.accessLevels.readWrite), new Microsoft.Modeling.Map<System.Int32,FRS2Model.connectionProperty>().Add(1, FRS2Model.connectionProperty.enabled).Add(2, FRS2Model.connectionProperty.disabled).Add(3, FRS2Model.connectionProperty.enabled).Add(4, FRS2Model.connectionProperty.enabled));
            this.Manager.Comment("reaching state \'S45\'");
            this.Manager.Comment("checking step \'return Initialization/ERROR_SUCCESS\'");
            this.Manager.Assert((temp128 == FRS2Model.error_status_t.ERROR_SUCCESS), String.Format("expected \'FRS2Model.error_status_t.ERROR_SUCCESS\', actual \'{0}\' (return of Initia" +
                        "lization, state S45)", TestManagerHelpers.Describe(temp128)));
            this.Manager.Comment("reaching state \'S78\'");
            FRS2Model.error_status_t temp129;
            this.Manager.Comment("executing step \'call CheckConnectivity(\"D\",4)\'");
            temp129 = this.IFRS2ManagedAdapterInstance.CheckConnectivity("D", 4);
            this.Manager.Checkpoint("MS-FRS2_R408");
            this.Manager.Checkpoint("MS-FRS2_R413");
            this.Manager.Checkpoint("MS-FRS2_R415");
            this.Manager.Checkpoint("MS-FRS2_R414");
            this.Manager.Comment("reaching state \'S106\'");
            this.Manager.Comment("checking step \'return CheckConnectivity/ERROR_FAIL\'");
            this.Manager.Assert((temp129 == FRS2Model.error_status_t.ERROR_FAIL), String.Format("expected \'FRS2Model.error_status_t.ERROR_FAIL\', actual \'{0}\' (return of CheckConn" +
                        "ectivity, state S106)", TestManagerHelpers.Describe(temp129)));
            this.Manager.Comment("reaching state \'S134\'");
            FRS2Model.ProtocolVersionReturned temp130;
            FRS2Model.UpstreamFlagValueReturned temp131;
            FRS2Model.error_status_t temp132;
            this.Manager.Comment("executing step \'call EstablishConnection(\"C\",3,FRS_COMMUNICATION_PROTOCOL_VERSION" +
                    "_LONGHORN_SERVER,out _,out _)\'");
            temp132 = this.IFRS2ManagedAdapterInstance.EstablishConnection("C", 3, FRS2Model.ProtocolVersion.FRS_COMMUNICATION_PROTOCOL_VERSION_LONGHORN_SERVER, out temp130, out temp131);
            this.Manager.Checkpoint("MS-FRS2_R37");
            this.Manager.Checkpoint("MS-FRS2_R436");
            this.Manager.Checkpoint("MS-FRS2_R439");
            this.Manager.Comment("reaching state \'S162\'");
            this.Manager.Comment("checking step \'return EstablishConnection/[out Valid,out Valid]:ERROR_SUCCESS\'");
            this.Manager.Assert((temp130 == FRS2Model.ProtocolVersionReturned.Valid), String.Format("expected \'FRS2Model.ProtocolVersionReturned.Valid\', actual \'{0}\' (upstreamProtoco" +
                        "lVersion of EstablishConnection, state S162)", TestManagerHelpers.Describe(temp130)));
            this.Manager.Assert((temp131 == FRS2Model.UpstreamFlagValueReturned.Valid), String.Format("expected \'FRS2Model.UpstreamFlagValueReturned.Valid\', actual \'{0}\' (upstreamFlags" +
                        " of EstablishConnection, state S162)", TestManagerHelpers.Describe(temp131)));
            this.Manager.Assert((temp132 == FRS2Model.error_status_t.ERROR_SUCCESS), String.Format("expected \'FRS2Model.error_status_t.ERROR_SUCCESS\', actual \'{0}\' (return of Establ" +
                        "ishConnection, state S162)", TestManagerHelpers.Describe(temp132)));
            this.Manager.Comment("reaching state \'S190\'");
            FRS2Model.error_status_t temp133;
            this.Manager.Comment("executing step \'call EstablishSession(3,3)\'");
            temp133 = this.IFRS2ManagedAdapterInstance.EstablishSession(3, 3);
            this.Manager.Checkpoint("MS-FRS2_R464");
            this.Manager.Checkpoint("MS-FRS2_R460");
            this.Manager.Comment("reaching state \'S214\'");
            this.Manager.Comment("checking step \'return EstablishSession/FRS_ERROR_CONTENTSET_READ_ONLY\'");
            this.Manager.Assert((temp133 == FRS2Model.error_status_t.FRS_ERROR_CONTENTSET_READ_ONLY), String.Format("expected \'FRS2Model.error_status_t.FRS_ERROR_CONTENTSET_READ_ONLY\', actual \'{0}\' " +
                        "(return of EstablishSession, state S214)", TestManagerHelpers.Describe(temp133)));
            TCtestScenario7S217();
            this.Manager.EndTest();
        }
        #endregion
        
        #region Test Starting in S46
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        [Microsoft.VisualStudio.TestTools.UnitTesting.DescriptionAttribute("MS-FRS2_R408, MS-FRS2_R413, MS-FRS2_R415, MS-FRS2_R414, MS-FRS2_R37, MS-FRS2_R436" +
            ", MS-FRS2_R439, MS-FRS2_R464, MS-FRS2_R460")]
        public virtual void FRS2_TCtestScenario7S46() {
            this.Manager.BeginTest("TCtestScenario7S46");
            this.Manager.Comment("reaching state \'S46\'");
            FRS2Model.error_status_t temp134;
            this.Manager.Comment(@"executing step 'call Initialization(Windows7,Map{1=>enabled,2=>enabled,3=>enabled,4=>disabled},Map{1=>exists,2=>exists,3=>exists,4=>exists},Map{""A""=>1,""B""=>2,""C""=>3,""D""=>4},Map{""A""=>sysvol,""B""=>protection,""C""=>sysvol,""D""=>sysvol},Map{1=>""A"",2=>""B"",3=>""C"",4=>""D""},Map{1=>writeOnly,2=>readWrite,3=>readOnly,4=>readWrite},Map{1=>enabled,2=>disabled,3=>enabled,4=>enabled})'");
            temp134 = this.IFRS2ManagedAdapterInstance.Initialization(FRS2Model.OSVersion.Windows7, new Microsoft.Modeling.Map<System.Int32,FRS2Model.connectionProperty>().Add(1, FRS2Model.connectionProperty.enabled).Add(2, FRS2Model.connectionProperty.enabled).Add(3, FRS2Model.connectionProperty.enabled).Add(4, FRS2Model.connectionProperty.disabled), new Microsoft.Modeling.Map<System.Int32,FRS2Model.FromServer>().Add(1, FRS2Model.FromServer.exists).Add(2, FRS2Model.FromServer.exists).Add(3, FRS2Model.FromServer.exists).Add(4, FRS2Model.FromServer.exists), new Microsoft.Modeling.Map<System.String,System.Int32>().Add("A", 1).Add("B", 2).Add("C", 3).Add("D", 4), new Microsoft.Modeling.Map<System.String,FRS2Model.ReplicationGroupTypes>().Add("A", FRS2Model.ReplicationGroupTypes.sysvol).Add("B", FRS2Model.ReplicationGroupTypes.protection).Add("C", FRS2Model.ReplicationGroupTypes.sysvol).Add("D", FRS2Model.ReplicationGroupTypes.sysvol), new Microsoft.Modeling.Map<System.Int32,System.String>().Add(1, "A").Add(2, "B").Add(3, "C").Add(4, "D"), new Microsoft.Modeling.Map<System.Int32,FRS2Model.accessLevels>().Add(1, FRS2Model.accessLevels.writeOnly).Add(2, FRS2Model.accessLevels.readWrite).Add(3, FRS2Model.accessLevels.readOnly).Add(4, FRS2Model.accessLevels.readWrite), new Microsoft.Modeling.Map<System.Int32,FRS2Model.connectionProperty>().Add(1, FRS2Model.connectionProperty.enabled).Add(2, FRS2Model.connectionProperty.disabled).Add(3, FRS2Model.connectionProperty.enabled).Add(4, FRS2Model.connectionProperty.enabled));
            this.Manager.Comment("reaching state \'S47\'");
            this.Manager.Comment("checking step \'return Initialization/ERROR_SUCCESS\'");
            this.Manager.Assert((temp134 == FRS2Model.error_status_t.ERROR_SUCCESS), String.Format("expected \'FRS2Model.error_status_t.ERROR_SUCCESS\', actual \'{0}\' (return of Initia" +
                        "lization, state S47)", TestManagerHelpers.Describe(temp134)));
            this.Manager.Comment("reaching state \'S79\'");
            FRS2Model.error_status_t temp135;
            this.Manager.Comment("executing step \'call CheckConnectivity(\"D\",4)\'");
            temp135 = this.IFRS2ManagedAdapterInstance.CheckConnectivity("D", 4);
            this.Manager.Checkpoint("MS-FRS2_R408");
            this.Manager.Checkpoint("MS-FRS2_R413");
            this.Manager.Checkpoint("MS-FRS2_R415");
            this.Manager.Checkpoint("MS-FRS2_R414");
            this.Manager.Comment("reaching state \'S107\'");
            this.Manager.Comment("checking step \'return CheckConnectivity/ERROR_FAIL\'");
            this.Manager.Assert((temp135 == FRS2Model.error_status_t.ERROR_FAIL), String.Format("expected \'FRS2Model.error_status_t.ERROR_FAIL\', actual \'{0}\' (return of CheckConn" +
                        "ectivity, state S107)", TestManagerHelpers.Describe(temp135)));
            this.Manager.Comment("reaching state \'S135\'");
            FRS2Model.ProtocolVersionReturned temp136;
            FRS2Model.UpstreamFlagValueReturned temp137;
            FRS2Model.error_status_t temp138;
            this.Manager.Comment("executing step \'call EstablishConnection(\"A\",1,FRS_COMMUNICATION_PROTOCOL_VERSION" +
                    "_LONGHORN_SERVER,out _,out _)\'");
            temp138 = this.IFRS2ManagedAdapterInstance.EstablishConnection("A", 1, FRS2Model.ProtocolVersion.FRS_COMMUNICATION_PROTOCOL_VERSION_LONGHORN_SERVER, out temp136, out temp137);
            this.Manager.Checkpoint("MS-FRS2_R37");
            this.Manager.Checkpoint("MS-FRS2_R436");
            this.Manager.Checkpoint("MS-FRS2_R439");
            this.Manager.Comment("reaching state \'S163\'");
            this.Manager.Comment("checking step \'return EstablishConnection/[out Valid,out Valid]:ERROR_SUCCESS\'");
            this.Manager.Assert((temp136 == FRS2Model.ProtocolVersionReturned.Valid), String.Format("expected \'FRS2Model.ProtocolVersionReturned.Valid\', actual \'{0}\' (upstreamProtoco" +
                        "lVersion of EstablishConnection, state S163)", TestManagerHelpers.Describe(temp136)));
            this.Manager.Assert((temp137 == FRS2Model.UpstreamFlagValueReturned.Valid), String.Format("expected \'FRS2Model.UpstreamFlagValueReturned.Valid\', actual \'{0}\' (upstreamFlags" +
                        " of EstablishConnection, state S163)", TestManagerHelpers.Describe(temp137)));
            this.Manager.Assert((temp138 == FRS2Model.error_status_t.ERROR_SUCCESS), String.Format("expected \'FRS2Model.error_status_t.ERROR_SUCCESS\', actual \'{0}\' (return of Establ" +
                        "ishConnection, state S163)", TestManagerHelpers.Describe(temp138)));
            this.Manager.Comment("reaching state \'S191\'");
            FRS2Model.error_status_t temp139;
            this.Manager.Comment("executing step \'call EstablishSession(3,3)\'");
            temp139 = this.IFRS2ManagedAdapterInstance.EstablishSession(3, 3);
            this.Manager.Checkpoint("MS-FRS2_R464");
            this.Manager.Checkpoint("MS-FRS2_R460");
            this.Manager.Comment("reaching state \'S215\'");
            this.Manager.Comment("checking step \'return EstablishSession/FRS_ERROR_CONTENTSET_READ_ONLY\'");
            this.Manager.Assert((temp139 == FRS2Model.error_status_t.FRS_ERROR_CONTENTSET_READ_ONLY), String.Format("expected \'FRS2Model.error_status_t.FRS_ERROR_CONTENTSET_READ_ONLY\', actual \'{0}\' " +
                        "(return of EstablishSession, state S215)", TestManagerHelpers.Describe(temp139)));
            TCtestScenario7S221();
            this.Manager.EndTest();
        }
        #endregion
        
        #region Test Starting in S48
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        [Microsoft.VisualStudio.TestTools.UnitTesting.DescriptionAttribute("MS-FRS2_R408, MS-FRS2_R413, MS-FRS2_R415, MS-FRS2_R414, MS-FRS2_R437, MS-FRS2_R44" +
            "5")]
        public virtual void FRS2_TCtestScenario7S48() {
            this.Manager.BeginTest("TCtestScenario7S48");
            this.Manager.Comment("reaching state \'S48\'");
            FRS2Model.error_status_t temp140;
            this.Manager.Comment(@"executing step 'call Initialization(Windows7,Map{1=>enabled,2=>enabled,3=>enabled,4=>disabled},Map{1=>exists,2=>exists,3=>exists,4=>exists},Map{""A""=>1,""B""=>2,""C""=>3,""D""=>4},Map{""A""=>sysvol,""B""=>protection,""C""=>sysvol,""D""=>sysvol},Map{1=>""A"",2=>""B"",3=>""C"",4=>""D""},Map{1=>writeOnly,2=>readWrite,3=>readOnly,4=>readWrite},Map{1=>enabled,2=>disabled,3=>enabled,4=>enabled})'");
            temp140 = this.IFRS2ManagedAdapterInstance.Initialization(FRS2Model.OSVersion.Windows7, new Microsoft.Modeling.Map<System.Int32,FRS2Model.connectionProperty>().Add(1, FRS2Model.connectionProperty.enabled).Add(2, FRS2Model.connectionProperty.enabled).Add(3, FRS2Model.connectionProperty.enabled).Add(4, FRS2Model.connectionProperty.disabled), new Microsoft.Modeling.Map<System.Int32,FRS2Model.FromServer>().Add(1, FRS2Model.FromServer.exists).Add(2, FRS2Model.FromServer.exists).Add(3, FRS2Model.FromServer.exists).Add(4, FRS2Model.FromServer.exists), new Microsoft.Modeling.Map<System.String,System.Int32>().Add("A", 1).Add("B", 2).Add("C", 3).Add("D", 4), new Microsoft.Modeling.Map<System.String,FRS2Model.ReplicationGroupTypes>().Add("A", FRS2Model.ReplicationGroupTypes.sysvol).Add("B", FRS2Model.ReplicationGroupTypes.protection).Add("C", FRS2Model.ReplicationGroupTypes.sysvol).Add("D", FRS2Model.ReplicationGroupTypes.sysvol), new Microsoft.Modeling.Map<System.Int32,System.String>().Add(1, "A").Add(2, "B").Add(3, "C").Add(4, "D"), new Microsoft.Modeling.Map<System.Int32,FRS2Model.accessLevels>().Add(1, FRS2Model.accessLevels.writeOnly).Add(2, FRS2Model.accessLevels.readWrite).Add(3, FRS2Model.accessLevels.readOnly).Add(4, FRS2Model.accessLevels.readWrite), new Microsoft.Modeling.Map<System.Int32,FRS2Model.connectionProperty>().Add(1, FRS2Model.connectionProperty.enabled).Add(2, FRS2Model.connectionProperty.disabled).Add(3, FRS2Model.connectionProperty.enabled).Add(4, FRS2Model.connectionProperty.enabled));
            this.Manager.Comment("reaching state \'S49\'");
            this.Manager.Comment("checking step \'return Initialization/ERROR_SUCCESS\'");
            this.Manager.Assert((temp140 == FRS2Model.error_status_t.ERROR_SUCCESS), String.Format("expected \'FRS2Model.error_status_t.ERROR_SUCCESS\', actual \'{0}\' (return of Initia" +
                        "lization, state S49)", TestManagerHelpers.Describe(temp140)));
            this.Manager.Comment("reaching state \'S80\'");
            FRS2Model.error_status_t temp141;
            this.Manager.Comment("executing step \'call CheckConnectivity(\"D\",4)\'");
            temp141 = this.IFRS2ManagedAdapterInstance.CheckConnectivity("D", 4);
            this.Manager.Checkpoint("MS-FRS2_R408");
            this.Manager.Checkpoint("MS-FRS2_R413");
            this.Manager.Checkpoint("MS-FRS2_R415");
            this.Manager.Checkpoint("MS-FRS2_R414");
            this.Manager.Comment("reaching state \'S108\'");
            this.Manager.Comment("checking step \'return CheckConnectivity/ERROR_FAIL\'");
            this.Manager.Assert((temp141 == FRS2Model.error_status_t.ERROR_FAIL), String.Format("expected \'FRS2Model.error_status_t.ERROR_FAIL\', actual \'{0}\' (return of CheckConn" +
                        "ectivity, state S108)", TestManagerHelpers.Describe(temp141)));
            this.Manager.Comment("reaching state \'S136\'");
            FRS2Model.ProtocolVersionReturned temp142;
            FRS2Model.UpstreamFlagValueReturned temp143;
            FRS2Model.error_status_t temp144;
            this.Manager.Comment("executing step \'call EstablishConnection(\"D\",4,FRS_COMMUNICATION_PROTOCOL_VERSION" +
                    "_LONGHORN_SERVER,out _,out _)\'");
            temp144 = this.IFRS2ManagedAdapterInstance.EstablishConnection("D", 4, FRS2Model.ProtocolVersion.FRS_COMMUNICATION_PROTOCOL_VERSION_LONGHORN_SERVER, out temp142, out temp143);
            this.Manager.Checkpoint("MS-FRS2_R437");
            this.Manager.Checkpoint("MS-FRS2_R445");
            this.Manager.Comment("reaching state \'S164\'");
            this.Manager.Comment("checking step \'return EstablishConnection/[out Invalid,out Invalid]:ERROR_FAIL\'");
            this.Manager.Assert((temp142 == FRS2Model.ProtocolVersionReturned.Invalid), String.Format("expected \'FRS2Model.ProtocolVersionReturned.Invalid\', actual \'{0}\' (upstreamProto" +
                        "colVersion of EstablishConnection, state S164)", TestManagerHelpers.Describe(temp142)));
            this.Manager.Assert((temp143 == FRS2Model.UpstreamFlagValueReturned.Invalid), String.Format("expected \'FRS2Model.UpstreamFlagValueReturned.Invalid\', actual \'{0}\' (upstreamFla" +
                        "gs of EstablishConnection, state S164)", TestManagerHelpers.Describe(temp143)));
            this.Manager.Assert((temp144 == FRS2Model.error_status_t.ERROR_FAIL), String.Format("expected \'FRS2Model.error_status_t.ERROR_FAIL\', actual \'{0}\' (return of Establish" +
                        "Connection, state S164)", TestManagerHelpers.Describe(temp144)));
            TCtestScenario7S168();
            this.Manager.EndTest();
        }
        #endregion
        
        #region Test Starting in S50
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        [Microsoft.VisualStudio.TestTools.UnitTesting.DescriptionAttribute("MS-FRS2_R408, MS-FRS2_R412, MS-FRS2_R437, MS-FRS2_R443")]
        public virtual void FRS2_TCtestScenario7S50() {
            this.Manager.BeginTest("TCtestScenario7S50");
            this.Manager.Comment("reaching state \'S50\'");
            FRS2Model.error_status_t temp145;
            this.Manager.Comment(@"executing step 'call Initialization(Windows7,Map{1=>enabled,2=>enabled,3=>enabled,4=>disabled},Map{1=>exists,2=>exists,3=>exists,4=>exists},Map{""A""=>1,""B""=>2,""C""=>3,""D""=>4},Map{""A""=>sysvol,""B""=>protection,""C""=>sysvol,""D""=>sysvol},Map{1=>""A"",2=>""B"",3=>""C"",4=>""D""},Map{1=>writeOnly,2=>readWrite,3=>readOnly,4=>readWrite},Map{1=>enabled,2=>disabled,3=>enabled,4=>enabled})'");
            temp145 = this.IFRS2ManagedAdapterInstance.Initialization(FRS2Model.OSVersion.Windows7, new Microsoft.Modeling.Map<System.Int32,FRS2Model.connectionProperty>().Add(1, FRS2Model.connectionProperty.enabled).Add(2, FRS2Model.connectionProperty.enabled).Add(3, FRS2Model.connectionProperty.enabled).Add(4, FRS2Model.connectionProperty.disabled), new Microsoft.Modeling.Map<System.Int32,FRS2Model.FromServer>().Add(1, FRS2Model.FromServer.exists).Add(2, FRS2Model.FromServer.exists).Add(3, FRS2Model.FromServer.exists).Add(4, FRS2Model.FromServer.exists), new Microsoft.Modeling.Map<System.String,System.Int32>().Add("A", 1).Add("B", 2).Add("C", 3).Add("D", 4), new Microsoft.Modeling.Map<System.String,FRS2Model.ReplicationGroupTypes>().Add("A", FRS2Model.ReplicationGroupTypes.sysvol).Add("B", FRS2Model.ReplicationGroupTypes.protection).Add("C", FRS2Model.ReplicationGroupTypes.sysvol).Add("D", FRS2Model.ReplicationGroupTypes.sysvol), new Microsoft.Modeling.Map<System.Int32,System.String>().Add(1, "A").Add(2, "B").Add(3, "C").Add(4, "D"), new Microsoft.Modeling.Map<System.Int32,FRS2Model.accessLevels>().Add(1, FRS2Model.accessLevels.writeOnly).Add(2, FRS2Model.accessLevels.readWrite).Add(3, FRS2Model.accessLevels.readOnly).Add(4, FRS2Model.accessLevels.readWrite), new Microsoft.Modeling.Map<System.Int32,FRS2Model.connectionProperty>().Add(1, FRS2Model.connectionProperty.enabled).Add(2, FRS2Model.connectionProperty.disabled).Add(3, FRS2Model.connectionProperty.enabled).Add(4, FRS2Model.connectionProperty.enabled));
            this.Manager.Comment("reaching state \'S51\'");
            this.Manager.Comment("checking step \'return Initialization/ERROR_SUCCESS\'");
            this.Manager.Assert((temp145 == FRS2Model.error_status_t.ERROR_SUCCESS), String.Format("expected \'FRS2Model.error_status_t.ERROR_SUCCESS\', actual \'{0}\' (return of Initia" +
                        "lization, state S51)", TestManagerHelpers.Describe(temp145)));
            this.Manager.Comment("reaching state \'S81\'");
            FRS2Model.error_status_t temp146;
            this.Manager.Comment("executing step \'call CheckConnectivity(\"A\",2)\'");
            temp146 = this.IFRS2ManagedAdapterInstance.CheckConnectivity("A", 2);
            this.Manager.Checkpoint("MS-FRS2_R408");
            this.Manager.Checkpoint("MS-FRS2_R412");
            this.Manager.Comment("reaching state \'S109\'");
            this.Manager.Comment("checking step \'return CheckConnectivity/ERROR_FAIL\'");
            this.Manager.Assert((temp146 == FRS2Model.error_status_t.ERROR_FAIL), String.Format("expected \'FRS2Model.error_status_t.ERROR_FAIL\', actual \'{0}\' (return of CheckConn" +
                        "ectivity, state S109)", TestManagerHelpers.Describe(temp146)));
            this.Manager.Comment("reaching state \'S137\'");
            FRS2Model.ProtocolVersionReturned temp147;
            FRS2Model.UpstreamFlagValueReturned temp148;
            FRS2Model.error_status_t temp149;
            this.Manager.Comment("executing step \'call EstablishConnection(\"B\",2,FRS_COMMUNICATION_PROTOCOL_VERSION" +
                    "_LONGHORN_SERVER,out _,out _)\'");
            temp149 = this.IFRS2ManagedAdapterInstance.EstablishConnection("B", 2, FRS2Model.ProtocolVersion.FRS_COMMUNICATION_PROTOCOL_VERSION_LONGHORN_SERVER, out temp147, out temp148);
            this.Manager.Checkpoint("MS-FRS2_R437");
            this.Manager.Checkpoint("MS-FRS2_R443");
            this.Manager.Comment("reaching state \'S165\'");
            this.Manager.Comment("checking step \'return EstablishConnection/[out Invalid,out Invalid]:ERROR_FAIL\'");
            this.Manager.Assert((temp147 == FRS2Model.ProtocolVersionReturned.Invalid), String.Format("expected \'FRS2Model.ProtocolVersionReturned.Invalid\', actual \'{0}\' (upstreamProto" +
                        "colVersion of EstablishConnection, state S165)", TestManagerHelpers.Describe(temp147)));
            this.Manager.Assert((temp148 == FRS2Model.UpstreamFlagValueReturned.Invalid), String.Format("expected \'FRS2Model.UpstreamFlagValueReturned.Invalid\', actual \'{0}\' (upstreamFla" +
                        "gs of EstablishConnection, state S165)", TestManagerHelpers.Describe(temp148)));
            this.Manager.Assert((temp149 == FRS2Model.error_status_t.ERROR_FAIL), String.Format("expected \'FRS2Model.error_status_t.ERROR_FAIL\', actual \'{0}\' (return of Establish" +
                        "Connection, state S165)", TestManagerHelpers.Describe(temp149)));
            TCtestScenario7S168();
            this.Manager.EndTest();
        }
        #endregion
        
        #region Test Starting in S52
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        [Microsoft.VisualStudio.TestTools.UnitTesting.DescriptionAttribute("MS-FRS2_R407, MS-FRS2_R410, MS-FRS2_R417, MS-FRS2_R437, MS-FRS2_R442")]
        public virtual void FRS2_TCtestScenario7S52() {
            this.Manager.BeginTest("TCtestScenario7S52");
            this.Manager.Comment("reaching state \'S52\'");
            FRS2Model.error_status_t temp150;
            this.Manager.Comment(@"executing step 'call Initialization(Windows7,Map{1=>enabled,2=>enabled,3=>enabled,4=>disabled},Map{1=>exists,2=>exists,3=>exists,4=>exists},Map{""A""=>1,""B""=>2,""C""=>3,""D""=>4},Map{""A""=>sysvol,""B""=>protection,""C""=>sysvol,""D""=>sysvol},Map{1=>""A"",2=>""B"",3=>""C"",4=>""D""},Map{1=>writeOnly,2=>readWrite,3=>readOnly,4=>readWrite},Map{1=>enabled,2=>disabled,3=>enabled,4=>enabled})'");
            temp150 = this.IFRS2ManagedAdapterInstance.Initialization(FRS2Model.OSVersion.Windows7, new Microsoft.Modeling.Map<System.Int32,FRS2Model.connectionProperty>().Add(1, FRS2Model.connectionProperty.enabled).Add(2, FRS2Model.connectionProperty.enabled).Add(3, FRS2Model.connectionProperty.enabled).Add(4, FRS2Model.connectionProperty.disabled), new Microsoft.Modeling.Map<System.Int32,FRS2Model.FromServer>().Add(1, FRS2Model.FromServer.exists).Add(2, FRS2Model.FromServer.exists).Add(3, FRS2Model.FromServer.exists).Add(4, FRS2Model.FromServer.exists), new Microsoft.Modeling.Map<System.String,System.Int32>().Add("A", 1).Add("B", 2).Add("C", 3).Add("D", 4), new Microsoft.Modeling.Map<System.String,FRS2Model.ReplicationGroupTypes>().Add("A", FRS2Model.ReplicationGroupTypes.sysvol).Add("B", FRS2Model.ReplicationGroupTypes.protection).Add("C", FRS2Model.ReplicationGroupTypes.sysvol).Add("D", FRS2Model.ReplicationGroupTypes.sysvol), new Microsoft.Modeling.Map<System.Int32,System.String>().Add(1, "A").Add(2, "B").Add(3, "C").Add(4, "D"), new Microsoft.Modeling.Map<System.Int32,FRS2Model.accessLevels>().Add(1, FRS2Model.accessLevels.writeOnly).Add(2, FRS2Model.accessLevels.readWrite).Add(3, FRS2Model.accessLevels.readOnly).Add(4, FRS2Model.accessLevels.readWrite), new Microsoft.Modeling.Map<System.Int32,FRS2Model.connectionProperty>().Add(1, FRS2Model.connectionProperty.enabled).Add(2, FRS2Model.connectionProperty.disabled).Add(3, FRS2Model.connectionProperty.enabled).Add(4, FRS2Model.connectionProperty.enabled));
            this.Manager.Comment("reaching state \'S53\'");
            this.Manager.Comment("checking step \'return Initialization/ERROR_SUCCESS\'");
            this.Manager.Assert((temp150 == FRS2Model.error_status_t.ERROR_SUCCESS), String.Format("expected \'FRS2Model.error_status_t.ERROR_SUCCESS\', actual \'{0}\' (return of Initia" +
                        "lization, state S53)", TestManagerHelpers.Describe(temp150)));
            this.Manager.Comment("reaching state \'S82\'");
            FRS2Model.error_status_t temp151;
            this.Manager.Comment("executing step \'call CheckConnectivity(\"C\",3)\'");
            temp151 = this.IFRS2ManagedAdapterInstance.CheckConnectivity("C", 3);
            this.Manager.Checkpoint("MS-FRS2_R407");
            this.Manager.Checkpoint("MS-FRS2_R410");
            this.Manager.Checkpoint("MS-FRS2_R417");
            this.Manager.Comment("reaching state \'S110\'");
            this.Manager.Comment("checking step \'return CheckConnectivity/ERROR_SUCCESS\'");
            this.Manager.Assert((temp151 == FRS2Model.error_status_t.ERROR_SUCCESS), String.Format("expected \'FRS2Model.error_status_t.ERROR_SUCCESS\', actual \'{0}\' (return of CheckC" +
                        "onnectivity, state S110)", TestManagerHelpers.Describe(temp151)));
            this.Manager.Comment("reaching state \'S138\'");
            FRS2Model.ProtocolVersionReturned temp152;
            FRS2Model.UpstreamFlagValueReturned temp153;
            FRS2Model.error_status_t temp154;
            this.Manager.Comment("executing step \'call EstablishConnection(\"A\",2,FRS_COMMUNICATION_PROTOCOL_VERSION" +
                    "_LONGHORN_SERVER,out _,out _)\'");
            temp154 = this.IFRS2ManagedAdapterInstance.EstablishConnection("A", 2, FRS2Model.ProtocolVersion.FRS_COMMUNICATION_PROTOCOL_VERSION_LONGHORN_SERVER, out temp152, out temp153);
            this.Manager.Checkpoint("MS-FRS2_R437");
            this.Manager.Checkpoint("MS-FRS2_R442");
            this.Manager.Comment("reaching state \'S166\'");
            this.Manager.Comment("checking step \'return EstablishConnection/[out Invalid,out Invalid]:ERROR_FAIL\'");
            this.Manager.Assert((temp152 == FRS2Model.ProtocolVersionReturned.Invalid), String.Format("expected \'FRS2Model.ProtocolVersionReturned.Invalid\', actual \'{0}\' (upstreamProto" +
                        "colVersion of EstablishConnection, state S166)", TestManagerHelpers.Describe(temp152)));
            this.Manager.Assert((temp153 == FRS2Model.UpstreamFlagValueReturned.Invalid), String.Format("expected \'FRS2Model.UpstreamFlagValueReturned.Invalid\', actual \'{0}\' (upstreamFla" +
                        "gs of EstablishConnection, state S166)", TestManagerHelpers.Describe(temp153)));
            this.Manager.Assert((temp154 == FRS2Model.error_status_t.ERROR_FAIL), String.Format("expected \'FRS2Model.error_status_t.ERROR_FAIL\', actual \'{0}\' (return of Establish" +
                        "Connection, state S166)", TestManagerHelpers.Describe(temp154)));
            TCtestScenario7S185();
            this.Manager.EndTest();
        }
        #endregion
        
        #region Test Starting in S54
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        [Microsoft.VisualStudio.TestTools.UnitTesting.DescriptionAttribute("MS-FRS2_R407, MS-FRS2_R410, MS-FRS2_R417, MS-FRS2_R440, MS-FRS2_R447")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestCategory("AD")]
        [TestCategory("MS-FRS2")]
        [TestCategory("SDC")]
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        
        public virtual void FRS2_TCtestScenario7S54()
        {
            this.Manager.BeginTest("TCtestScenario7S54");
            this.Manager.Comment("reaching state \'S54\'");
            FRS2Model.error_status_t temp155;
            this.Manager.Comment(@"executing step 'call Initialization(Windows7,Map{1=>enabled,2=>enabled,3=>enabled,4=>disabled},Map{1=>exists,2=>exists,3=>exists,4=>exists},Map{""A""=>1,""B""=>2,""C""=>3,""D""=>4},Map{""A""=>sysvol,""B""=>protection,""C""=>sysvol,""D""=>sysvol},Map{1=>""A"",2=>""B"",3=>""C"",4=>""D""},Map{1=>writeOnly,2=>readWrite,3=>readOnly,4=>readWrite},Map{1=>enabled,2=>disabled,3=>enabled,4=>enabled})'");
            temp155 = this.IFRS2ManagedAdapterInstance.Initialization(FRS2Model.OSVersion.Windows7, new Microsoft.Modeling.Map<System.Int32,FRS2Model.connectionProperty>().Add(1, FRS2Model.connectionProperty.enabled).Add(2, FRS2Model.connectionProperty.enabled).Add(3, FRS2Model.connectionProperty.enabled).Add(4, FRS2Model.connectionProperty.disabled), new Microsoft.Modeling.Map<System.Int32,FRS2Model.FromServer>().Add(1, FRS2Model.FromServer.exists).Add(2, FRS2Model.FromServer.exists).Add(3, FRS2Model.FromServer.exists).Add(4, FRS2Model.FromServer.exists), new Microsoft.Modeling.Map<System.String,System.Int32>().Add("A", 1).Add("B", 2).Add("C", 3).Add("D", 4), new Microsoft.Modeling.Map<System.String,FRS2Model.ReplicationGroupTypes>().Add("A", FRS2Model.ReplicationGroupTypes.sysvol).Add("B", FRS2Model.ReplicationGroupTypes.protection).Add("C", FRS2Model.ReplicationGroupTypes.sysvol).Add("D", FRS2Model.ReplicationGroupTypes.sysvol), new Microsoft.Modeling.Map<System.Int32,System.String>().Add(1, "A").Add(2, "B").Add(3, "C").Add(4, "D"), new Microsoft.Modeling.Map<System.Int32,FRS2Model.accessLevels>().Add(1, FRS2Model.accessLevels.writeOnly).Add(2, FRS2Model.accessLevels.readWrite).Add(3, FRS2Model.accessLevels.readOnly).Add(4, FRS2Model.accessLevels.readWrite), new Microsoft.Modeling.Map<System.Int32,FRS2Model.connectionProperty>().Add(1, FRS2Model.connectionProperty.enabled).Add(2, FRS2Model.connectionProperty.disabled).Add(3, FRS2Model.connectionProperty.enabled).Add(4, FRS2Model.connectionProperty.enabled));
            this.Manager.Comment("reaching state \'S55\'");
            this.Manager.Comment("checking step \'return Initialization/ERROR_SUCCESS\'");
            this.Manager.Assert((temp155 == FRS2Model.error_status_t.ERROR_SUCCESS), String.Format("expected \'FRS2Model.error_status_t.ERROR_SUCCESS\', actual \'{0}\' (return of Initia" +
                        "lization, state S55)", TestManagerHelpers.Describe(temp155)));
            this.Manager.Comment("reaching state \'S83\'");
            FRS2Model.error_status_t temp156;
            this.Manager.Comment("executing step \'call CheckConnectivity(\"A\",1)\'");
            temp156 = this.IFRS2ManagedAdapterInstance.CheckConnectivity("A", 1);
            this.Manager.Checkpoint("MS-FRS2_R407");
            this.Manager.Checkpoint("MS-FRS2_R410");
            this.Manager.Checkpoint("MS-FRS2_R417");
            this.Manager.Comment("reaching state \'S111\'");
            this.Manager.Comment("checking step \'return CheckConnectivity/ERROR_SUCCESS\'");
            this.Manager.Assert((temp156 == FRS2Model.error_status_t.ERROR_SUCCESS), String.Format("expected \'FRS2Model.error_status_t.ERROR_SUCCESS\', actual \'{0}\' (return of CheckC" +
                        "onnectivity, state S111)", TestManagerHelpers.Describe(temp156)));
            this.Manager.Comment("reaching state \'S139\'");
            FRS2Model.ProtocolVersionReturned temp157;
            FRS2Model.UpstreamFlagValueReturned temp158;
            FRS2Model.error_status_t temp159;
            this.Manager.Comment("executing step \'call EstablishConnection(\"A\",1,inValid,out _,out _)\'");
            temp159 = this.IFRS2ManagedAdapterInstance.EstablishConnection("A", 1, FRS2Model.ProtocolVersion.inValid, out temp157, out temp158);
            this.Manager.Checkpoint("MS-FRS2_R440");
            this.Manager.Checkpoint("MS-FRS2_R447");
            this.Manager.Comment("reaching state \'S167\'");
            this.Manager.Comment("checking step \'return EstablishConnection/[out Invalid,out Invalid]:FRS_ERROR_INC" +
                    "OMPATIBLE_VERSION\'");
            this.Manager.Assert((temp157 == FRS2Model.ProtocolVersionReturned.Invalid), String.Format("expected \'FRS2Model.ProtocolVersionReturned.Invalid\', actual \'{0}\' (upstreamProto" +
                        "colVersion of EstablishConnection, state S167)", TestManagerHelpers.Describe(temp157)));
            this.Manager.Assert((temp158 == FRS2Model.UpstreamFlagValueReturned.Invalid), String.Format("expected \'FRS2Model.UpstreamFlagValueReturned.Invalid\', actual \'{0}\' (upstreamFla" +
                        "gs of EstablishConnection, state S167)", TestManagerHelpers.Describe(temp158)));
            this.Manager.Assert((temp159 == FRS2Model.error_status_t.FRS_ERROR_INCOMPATIBLE_VERSION), String.Format("expected \'FRS2Model.error_status_t.FRS_ERROR_INCOMPATIBLE_VERSION\', actual \'{0}\' " +
                        "(return of EstablishConnection, state S167)", TestManagerHelpers.Describe(temp159)));
            TCtestScenario7S185();
            this.Manager.EndTest();
        }
        #endregion
    }
}
