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
    public partial class TCtestScenario4 : PtfTestClassBase {
        
        public TCtestScenario4() {
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
        
        public delegate void RdcPushSourceNeedsDelegate1(FRS2Model.error_status_t @return);
        
        public delegate void RdcGetFileDataAsyncDelegate1(FRS2Model.error_status_t @return);
        
        public delegate void RdcCloseDelegate1(FRS2Model.error_status_t @return);
        
        public delegate void CheckConnectivityDelegate1(FRS2Model.error_status_t @return);
        
        public delegate void RequestRecordsDelegate1(FRS2Model.error_status_t @return);
        
        public delegate void UpdateCancelDelegate1(FRS2Model.error_status_t @return);
        
        public delegate void RawGetFileDataDelegate1(FRS2Model.error_status_t @return);
        
        public delegate void RdcGetFileDataDelegate1(FRS2Model.error_status_t @return);
        
        public delegate void RawGetFileDataAsyncDelegate1(FRS2Model.error_status_t @return);
        
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
        
        static System.Reflection.MethodBase RdcPushSourceNeedsInfo = TestManagerHelpers.GetMethodInfo(typeof(Microsoft.Protocols.TestSuites.MS_FRS2.IFRS2ManagedAdapter), "RdcPushSourceNeeds");
        
        static System.Reflection.MethodBase RdcGetFileDataAsyncInfo = TestManagerHelpers.GetMethodInfo(typeof(Microsoft.Protocols.TestSuites.MS_FRS2.IFRS2ManagedAdapter), "RdcGetFileDataAsync");
        
        static System.Reflection.MethodBase RdcCloseInfo = TestManagerHelpers.GetMethodInfo(typeof(Microsoft.Protocols.TestSuites.MS_FRS2.IFRS2ManagedAdapter), "RdcClose");
        
        static System.Reflection.MethodBase CheckConnectivityInfo = TestManagerHelpers.GetMethodInfo(typeof(Microsoft.Protocols.TestSuites.MS_FRS2.IFRS2ManagedAdapter), "CheckConnectivity", typeof(string), typeof(int));
        
        static System.Reflection.MethodBase RequestRecordsInfo = TestManagerHelpers.GetMethodInfo(typeof(Microsoft.Protocols.TestSuites.MS_FRS2.IFRS2ManagedAdapter), "RequestRecords", typeof(int), typeof(int));
        
        static System.Reflection.MethodBase UpdateCancelInfo = TestManagerHelpers.GetMethodInfo(typeof(Microsoft.Protocols.TestSuites.MS_FRS2.IFRS2ManagedAdapter), "UpdateCancel", typeof(int), typeof(FRS2Model.FRS_UPDATE_CANCEL_DATA), typeof(int));
        
        static System.Reflection.MethodBase RawGetFileDataInfo = TestManagerHelpers.GetMethodInfo(typeof(Microsoft.Protocols.TestSuites.MS_FRS2.IFRS2ManagedAdapter), "RawGetFileData");
        
        static System.Reflection.MethodBase RdcGetFileDataInfo = TestManagerHelpers.GetMethodInfo(typeof(Microsoft.Protocols.TestSuites.MS_FRS2.IFRS2ManagedAdapter), "RdcGetFileData", typeof(FRS2Model.BufferSize));
        
        static System.Reflection.MethodBase RawGetFileDataAsyncInfo = TestManagerHelpers.GetMethodInfo(typeof(Microsoft.Protocols.TestSuites.MS_FRS2.IFRS2ManagedAdapter), "RawGetFileDataAsync");
        
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
        [Microsoft.VisualStudio.TestTools.UnitTesting.DescriptionAttribute("MS-FRS2_R440, MS-FRS2_R447")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestCategory("AD")]
        [TestCategory("MS-FRS2")]
        [TestCategory("SDC")]
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]     
        public virtual void FRS2_TCtestScenario4S0()
        {
            this.Manager.BeginTest("TCtestScenario4S0");
            this.Manager.Comment("reaching state \'S0\'");
            FRS2Model.error_status_t temp0;
            this.Manager.Comment(@"executing step 'call Initialization(Windows7,Map{1=>enabled,2=>enabled,3=>enabled,4=>disabled},Map{1=>exists,2=>exists,3=>exists,4=>exists},Map{""A""=>1,""B""=>2,""C""=>3,""D""=>4},Map{""A""=>sysvol,""B""=>protection,""C""=>sysvol,""D""=>sysvol},Map{1=>""A"",2=>""B"",3=>""C"",4=>""D""},Map{1=>writeOnly,2=>readWrite,3=>readOnly,4=>readWrite},Map{1=>enabled,2=>disabled,3=>enabled,4=>enabled})'");
            temp0 = this.IFRS2ManagedAdapterInstance.Initialization(FRS2Model.OSVersion.Windows7, new Microsoft.Modeling.Map<System.Int32,FRS2Model.connectionProperty>().Add(1, FRS2Model.connectionProperty.enabled).Add(2, FRS2Model.connectionProperty.enabled).Add(3, FRS2Model.connectionProperty.enabled).Add(4, FRS2Model.connectionProperty.disabled), new Microsoft.Modeling.Map<System.Int32,FRS2Model.FromServer>().Add(1, FRS2Model.FromServer.exists).Add(2, FRS2Model.FromServer.exists).Add(3, FRS2Model.FromServer.exists).Add(4, FRS2Model.FromServer.exists), new Microsoft.Modeling.Map<System.String,System.Int32>().Add("A", 1).Add("B", 2).Add("C", 3).Add("D", 4), new Microsoft.Modeling.Map<System.String,FRS2Model.ReplicationGroupTypes>().Add("A", FRS2Model.ReplicationGroupTypes.sysvol).Add("B", FRS2Model.ReplicationGroupTypes.protection).Add("C", FRS2Model.ReplicationGroupTypes.sysvol).Add("D", FRS2Model.ReplicationGroupTypes.sysvol), new Microsoft.Modeling.Map<System.Int32,System.String>().Add(1, "A").Add(2, "B").Add(3, "C").Add(4, "D"), new Microsoft.Modeling.Map<System.Int32,FRS2Model.accessLevels>().Add(1, FRS2Model.accessLevels.writeOnly).Add(2, FRS2Model.accessLevels.readWrite).Add(3, FRS2Model.accessLevels.readOnly).Add(4, FRS2Model.accessLevels.readWrite), new Microsoft.Modeling.Map<System.Int32,FRS2Model.connectionProperty>().Add(1, FRS2Model.connectionProperty.enabled).Add(2, FRS2Model.connectionProperty.disabled).Add(3, FRS2Model.connectionProperty.enabled).Add(4, FRS2Model.connectionProperty.enabled));
            this.Manager.Comment("reaching state \'S1\'");
            this.Manager.Comment("checking step \'return Initialization/ERROR_SUCCESS\'");
            this.Manager.Assert((temp0 == FRS2Model.error_status_t.ERROR_SUCCESS), String.Format("expected \'FRS2Model.error_status_t.ERROR_SUCCESS\', actual \'{0}\' (return of Initia" +
                        "lization, state S1)", TestManagerHelpers.Describe(temp0)));
            this.Manager.Comment("reaching state \'S12\'");
            FRS2Model.ProtocolVersionReturned temp1;
            FRS2Model.UpstreamFlagValueReturned temp2;
            FRS2Model.error_status_t temp3;
            this.Manager.Comment("executing step \'call EstablishConnection(\"A\",1,inValid,out _,out _)\'");
            temp3 = this.IFRS2ManagedAdapterInstance.EstablishConnection("A", 1, FRS2Model.ProtocolVersion.inValid, out temp1, out temp2);
            this.Manager.Checkpoint("MS-FRS2_R440");
            this.Manager.Checkpoint("MS-FRS2_R447");
            this.Manager.Comment("reaching state \'S18\'");
            this.Manager.Comment("checking step \'return EstablishConnection/[out Invalid,out Invalid]:FRS_ERROR_INC" +
                    "OMPATIBLE_VERSION\'");
            this.Manager.Assert((temp1 == FRS2Model.ProtocolVersionReturned.Invalid), String.Format("expected \'FRS2Model.ProtocolVersionReturned.Invalid\', actual \'{0}\' (upstreamProto" +
                        "colVersion of EstablishConnection, state S18)", TestManagerHelpers.Describe(temp1)));
            this.Manager.Assert((temp2 == FRS2Model.UpstreamFlagValueReturned.Invalid), String.Format("expected \'FRS2Model.UpstreamFlagValueReturned.Invalid\', actual \'{0}\' (upstreamFla" +
                        "gs of EstablishConnection, state S18)", TestManagerHelpers.Describe(temp2)));
            this.Manager.Assert((temp3 == FRS2Model.error_status_t.FRS_ERROR_INCOMPATIBLE_VERSION), String.Format("expected \'FRS2Model.error_status_t.FRS_ERROR_INCOMPATIBLE_VERSION\', actual \'{0}\' " +
                        "(return of EstablishConnection, state S18)", TestManagerHelpers.Describe(temp3)));
            TCtestScenario4S24();
            this.Manager.EndTest();
        }
        
        private void TCtestScenario4S24() {
            this.Manager.Comment("reaching state \'S24\'");
        }
        #endregion
        
        #region Test Starting in S2
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        [Microsoft.VisualStudio.TestTools.UnitTesting.DescriptionAttribute(@"MS-FRS2_R37, MS-FRS2_R436, MS-FRS2_R439, MS-FRS2_R455, MS-FRS2_R458, MS-FRS2_R448, MS-FRS2_R549, MS-FRS2_R552, MS-FRS2_R517, MS-FRS2_R520, MS-FRS2_R466, MS-FRS2_R556, MS-FRS2_R1020, MS-FRS2_R555, MS-FRS2_R487, MS-FRS2_R494, MS-FRS2_R556, MS-FRS2_R1020, MS-FRS2_R555")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestCategory("AD")]
        [TestCategory("MS-FRS2")]
        [TestCategory("SDC")]
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        public virtual void FRS2_TCtestScenario4S2()
        {
            this.Manager.BeginTest("TCtestScenario4S2");
            this.Manager.Comment("reaching state \'S2\'");
            FRS2Model.error_status_t temp4;
            this.Manager.Comment(@"executing step 'call Initialization(Windows7,Map{1=>enabled,2=>enabled,3=>enabled,4=>disabled},Map{1=>exists,2=>exists,3=>exists,4=>exists},Map{""A""=>1,""B""=>2,""C""=>3,""D""=>4},Map{""A""=>sysvol,""B""=>protection,""C""=>sysvol,""D""=>sysvol},Map{1=>""A"",2=>""B"",3=>""C"",4=>""D""},Map{1=>writeOnly,2=>readWrite,3=>readOnly,4=>readWrite},Map{1=>enabled,2=>disabled,3=>enabled,4=>enabled})'");
            temp4 = this.IFRS2ManagedAdapterInstance.Initialization(FRS2Model.OSVersion.Windows7, new Microsoft.Modeling.Map<System.Int32,FRS2Model.connectionProperty>().Add(1, FRS2Model.connectionProperty.enabled).Add(2, FRS2Model.connectionProperty.enabled).Add(3, FRS2Model.connectionProperty.enabled).Add(4, FRS2Model.connectionProperty.disabled), new Microsoft.Modeling.Map<System.Int32,FRS2Model.FromServer>().Add(1, FRS2Model.FromServer.exists).Add(2, FRS2Model.FromServer.exists).Add(3, FRS2Model.FromServer.exists).Add(4, FRS2Model.FromServer.exists), new Microsoft.Modeling.Map<System.String,System.Int32>().Add("A", 1).Add("B", 2).Add("C", 3).Add("D", 4), new Microsoft.Modeling.Map<System.String,FRS2Model.ReplicationGroupTypes>().Add("A", FRS2Model.ReplicationGroupTypes.sysvol).Add("B", FRS2Model.ReplicationGroupTypes.protection).Add("C", FRS2Model.ReplicationGroupTypes.sysvol).Add("D", FRS2Model.ReplicationGroupTypes.sysvol), new Microsoft.Modeling.Map<System.Int32,System.String>().Add(1, "A").Add(2, "B").Add(3, "C").Add(4, "D"), new Microsoft.Modeling.Map<System.Int32,FRS2Model.accessLevels>().Add(1, FRS2Model.accessLevels.writeOnly).Add(2, FRS2Model.accessLevels.readWrite).Add(3, FRS2Model.accessLevels.readOnly).Add(4, FRS2Model.accessLevels.readWrite), new Microsoft.Modeling.Map<System.Int32,FRS2Model.connectionProperty>().Add(1, FRS2Model.connectionProperty.enabled).Add(2, FRS2Model.connectionProperty.disabled).Add(3, FRS2Model.connectionProperty.enabled).Add(4, FRS2Model.connectionProperty.enabled));
            this.Manager.Comment("reaching state \'S3\'");
            this.Manager.Comment("checking step \'return Initialization/ERROR_SUCCESS\'");
            this.Manager.Assert((temp4 == FRS2Model.error_status_t.ERROR_SUCCESS), String.Format("expected \'FRS2Model.error_status_t.ERROR_SUCCESS\', actual \'{0}\' (return of Initia" +
                        "lization, state S3)", TestManagerHelpers.Describe(temp4)));
            this.Manager.Comment("reaching state \'S13\'");
            FRS2Model.ProtocolVersionReturned temp5;
            FRS2Model.UpstreamFlagValueReturned temp6;
            FRS2Model.error_status_t temp7;
            this.Manager.Comment("executing step \'call EstablishConnection(\"A\",1,FRS_COMMUNICATION_PROTOCOL_VERSION" +
                    "_LONGHORN_SERVER,out _,out _)\'");
            temp7 = this.IFRS2ManagedAdapterInstance.EstablishConnection("A", 1, FRS2Model.ProtocolVersion.FRS_COMMUNICATION_PROTOCOL_VERSION_LONGHORN_SERVER, out temp5, out temp6);
            this.Manager.Checkpoint("MS-FRS2_R37");
            this.Manager.Checkpoint("MS-FRS2_R436");
            this.Manager.Checkpoint("MS-FRS2_R439");
            this.Manager.Comment("reaching state \'S19\'");
            this.Manager.Comment("checking step \'return EstablishConnection/[out Valid,out Valid]:ERROR_SUCCESS\'");
            this.Manager.Assert((temp5 == FRS2Model.ProtocolVersionReturned.Valid), String.Format("expected \'FRS2Model.ProtocolVersionReturned.Valid\', actual \'{0}\' (upstreamProtoco" +
                        "lVersion of EstablishConnection, state S19)", TestManagerHelpers.Describe(temp5)));
            this.Manager.Assert((temp6 == FRS2Model.UpstreamFlagValueReturned.Valid), String.Format("expected \'FRS2Model.UpstreamFlagValueReturned.Valid\', actual \'{0}\' (upstreamFlags" +
                        " of EstablishConnection, state S19)", TestManagerHelpers.Describe(temp6)));
            this.Manager.Assert((temp7 == FRS2Model.error_status_t.ERROR_SUCCESS), String.Format("expected \'FRS2Model.error_status_t.ERROR_SUCCESS\', actual \'{0}\' (return of Establ" +
                        "ishConnection, state S19)", TestManagerHelpers.Describe(temp7)));
            this.Manager.Comment("reaching state \'S25\'");
            FRS2Model.error_status_t temp8;
            this.Manager.Comment("executing step \'call EstablishSession(1,1)\'");
            temp8 = this.IFRS2ManagedAdapterInstance.EstablishSession(1, 1);
            this.Manager.Checkpoint("MS-FRS2_R455");
            this.Manager.Checkpoint("MS-FRS2_R458");
            this.Manager.Checkpoint("MS-FRS2_R448");
            this.Manager.Comment("reaching state \'S30\'");
            this.Manager.Comment("checking step \'return EstablishSession/ERROR_SUCCESS\'");
            this.Manager.Assert((temp8 == FRS2Model.error_status_t.ERROR_SUCCESS), String.Format("expected \'FRS2Model.error_status_t.ERROR_SUCCESS\', actual \'{0}\' (return of Establ" +
                        "ishSession, state S30)", TestManagerHelpers.Describe(temp8)));
            this.Manager.Comment("reaching state \'S34\'");
            FRS2Model.error_status_t temp9;
            this.Manager.Comment("executing step \'call AsyncPoll(1)\'");
            temp9 = this.IFRS2ManagedAdapterInstance.AsyncPoll(1);
            this.Manager.Checkpoint("MS-FRS2_R549");
            this.Manager.Checkpoint("MS-FRS2_R552");
            this.Manager.Comment("reaching state \'S38\'");
            this.Manager.Comment("checking step \'return AsyncPoll/ERROR_SUCCESS\'");
            this.Manager.Assert((temp9 == FRS2Model.error_status_t.ERROR_SUCCESS), String.Format("expected \'FRS2Model.error_status_t.ERROR_SUCCESS\', actual \'{0}\' (return of AsyncP" +
                        "oll, state S38)", TestManagerHelpers.Describe(temp9)));
            this.Manager.Comment("reaching state \'S41\'");
            FRS2Model.error_status_t temp10;
            this.Manager.Comment("executing step \'call RequestVersionVector(1,1,1,REQUEST_NORMAL_SYNC,CHANGE_NOTIFY" +
                    ",ValidValue)\'");
            temp10 = this.IFRS2ManagedAdapterInstance.RequestVersionVector(1, 1, 1, FRS2Model.VERSION_REQUEST_TYPE.REQUEST_NORMAL_SYNC, FRS2Model.VERSION_CHANGE_TYPE.CHANGE_NOTIFY, FRS2Model.VVGeneration.ValidValue);
            this.Manager.Checkpoint("MS-FRS2_R517");
            this.Manager.Checkpoint("MS-FRS2_R520");
            this.Manager.Checkpoint("MS-FRS2_R466");
            this.Manager.Comment("reaching state \'S44\'");
            this.Manager.Comment("checking step \'return RequestVersionVector/ERROR_SUCCESS\'");
            this.Manager.Assert((temp10 == FRS2Model.error_status_t.ERROR_SUCCESS), String.Format("expected \'FRS2Model.error_status_t.ERROR_SUCCESS\', actual \'{0}\' (return of Reques" +
                        "tVersionVector, state S44)", TestManagerHelpers.Describe(temp10)));
            this.Manager.Comment("reaching state \'S47\'");
            int temp12 = this.Manager.ExpectEvent(this.QuiescenceTimeout, true, new ExpectedEvent(TCtestScenario4.AsyncPollResponseEventInfo, null, new AsyncPollResponseEventDelegate1(this.TCtestScenario4S2AsyncPollResponseEventChecker)), new ExpectedEvent(TCtestScenario4.AsyncPollResponseEventInfo, null, new AsyncPollResponseEventDelegate1(this.TCtestScenario4S2AsyncPollResponseEventChecker1)));
            if ((temp12 == 0)) {
                this.Manager.Comment("reaching state \'S50\'");
                FRS2Model.error_status_t temp11;
                this.Manager.Comment("executing step \'call RequestUpdates(1,1,inValid)\'");
                temp11 = this.IFRS2ManagedAdapterInstance.RequestUpdates(1, 1, FRS2Model.versionVectorDiff.inValid);
                this.Manager.Checkpoint("MS-FRS2_R487");
                this.Manager.Checkpoint("MS-FRS2_R494");
                this.Manager.Comment("reaching state \'S54\'");
                this.Manager.Comment("checking step \'return RequestUpdates/ERROR_FAIL\'");
                this.Manager.Assert((temp11 == FRS2Model.error_status_t.ERROR_FAIL), String.Format("expected \'FRS2Model.error_status_t.ERROR_FAIL\', actual \'{0}\' (return of RequestUp" +
                            "dates, state S54)", TestManagerHelpers.Describe(temp11)));
                this.Manager.Comment("reaching state \'S56\'");
                goto label0;
            }
            if ((temp12 == 1)) {
                TCtestScenario4S51();
                goto label0;
            }
            throw new InvalidOperationException("never reached");
        label0:
;
            this.Manager.EndTest();
        }
        
        private void TCtestScenario4S2AsyncPollResponseEventChecker(FRS2Model.VVGeneration vvGen) {
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
        
        private void TCtestScenario4S2AsyncPollResponseEventChecker1(FRS2Model.VVGeneration vvGen) {
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
        
        private void TCtestScenario4S51() {
            this.Manager.Comment("reaching state \'S51\'");
        }
        #endregion
        
        #region Test Starting in S4
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        [Microsoft.VisualStudio.TestTools.UnitTesting.DescriptionAttribute("MS-FRS2_R37, MS-FRS2_R436, MS-FRS2_R439, MS-FRS2_R455, MS-FRS2_R458, MS-FRS2_R448" +
            ", MS-FRS2_R549, MS-FRS2_R552, MS-FRS2_R517, MS-FRS2_R520, MS-FRS2_R466, MS-FRS2_" +
            "R556, MS-FRS2_R1020, MS-FRS2_R555, MS-FRS2_R486, MS-FRS2_R489, MS-FRS2_R93, MS-F" +
            "RS2_R774, MS-FRS2_R777, MS-FRS2_R793, MS-FRS2_R646, MS-FRS2_R649, MS-FRS2_R650, " +
            "MS-FRS2_R671, MS-FRS2_R674, MS-FRS2_R693, MS-FRS2_R694, MS-FRS2_R683, MS-FRS2_R6" +
            "91, MS-FRS2_R692, MS-FRS2_R675, MS-FRS2_R882, MS-FRS2_R885, MS-FRS2_R896, MS-FRS" +
            "2_R886, MS-FRS2_R737, MS-FRS2_R739, MS-FRS2_R647, MS-FRS2_R655, MS-FRS2_R656, MS" +
            "-FRS2_R657, MS-FRS2_R805, MS-FRS2_R647, MS-FRS2_R658, MS-FRS2_R805, MS-FRS2_R647" +
            ", MS-FRS2_R655, MS-FRS2_R656, MS-FRS2_R657, MS-FRS2_R658, MS-FRS2_R805, MS-FRS2_" +
            "R93, MS-FRS2_R498, MS-FRS2_R774, MS-FRS2_R777, MS-FRS2_R793, MS-FRS2_R647, MS-FR" +
            "S2_R658, MS-FRS2_R805, MS-FRS2_R647, MS-FRS2_R655, MS-FRS2_R656, MS-FRS2_R657, M" +
            "S-FRS2_R658, MS-FRS2_R805, MS-FRS2_R647, MS-FRS2_R655, MS-FRS2_R656, MS-FRS2_R65" +
            "7, MS-FRS2_R805, MS-FRS2_R646, MS-FRS2_R649, MS-FRS2_R650, MS-FRS2_R671, MS-FRS2" +
            "_R674, MS-FRS2_R693, MS-FRS2_R694, MS-FRS2_R683, MS-FRS2_R691, MS-FRS2_R692, MS-" +
            "FRS2_R675, MS-FRS2_R882, MS-FRS2_R885, MS-FRS2_R896, MS-FRS2_R886, MS-FRS2_R737," +
            " MS-FRS2_R739, MS-FRS2_R93, MS-FRS2_R94, MS-FRS2_R774, MS-FRS2_R777, MS-FRS2_R79" +
            "3, MS-FRS2_R647, MS-FRS2_R658, MS-FRS2_R805, MS-FRS2_R647, MS-FRS2_R655, MS-FRS2" +
            "_R656, MS-FRS2_R657, MS-FRS2_R658, MS-FRS2_R805, MS-FRS2_R647, MS-FRS2_R655, MS-" +
            "FRS2_R656, MS-FRS2_R657, MS-FRS2_R805, MS-FRS2_R646, MS-FRS2_R649, MS-FRS2_R650," +
            " MS-FRS2_R671, MS-FRS2_R674, MS-FRS2_R693, MS-FRS2_R694, MS-FRS2_R683, MS-FRS2_R" +
            "691, MS-FRS2_R692, MS-FRS2_R675, MS-FRS2_R882, MS-FRS2_R885, MS-FRS2_R896, MS-FR" +
            "S2_R886, MS-FRS2_R737, MS-FRS2_R739, MS-FRS2_R93, MS-FRS2_R94, MS-FRS2_R774, MS-" +
            "FRS2_R777, MS-FRS2_R793, MS-FRS2_R556, MS-FRS2_R1020, MS-FRS2_R555")]
        public virtual void FRS2_TCtestScenario4S4() {
            this.Manager.BeginTest("TCtestScenario4S4");
            this.Manager.Comment("reaching state \'S4\'");
            FRS2Model.error_status_t temp13;
            this.Manager.Comment(@"executing step 'call Initialization(Windows7,Map{1=>enabled,2=>enabled,3=>enabled,4=>disabled},Map{1=>exists,2=>exists,3=>exists,4=>exists},Map{""A""=>1,""B""=>2,""C""=>3,""D""=>4},Map{""A""=>sysvol,""B""=>protection,""C""=>sysvol,""D""=>sysvol},Map{1=>""A"",2=>""B"",3=>""C"",4=>""D""},Map{1=>writeOnly,2=>readWrite,3=>readOnly,4=>readWrite},Map{1=>enabled,2=>disabled,3=>enabled,4=>enabled})'");
            temp13 = this.IFRS2ManagedAdapterInstance.Initialization(FRS2Model.OSVersion.Windows7, new Microsoft.Modeling.Map<System.Int32,FRS2Model.connectionProperty>().Add(1, FRS2Model.connectionProperty.enabled).Add(2, FRS2Model.connectionProperty.enabled).Add(3, FRS2Model.connectionProperty.enabled).Add(4, FRS2Model.connectionProperty.disabled), new Microsoft.Modeling.Map<System.Int32,FRS2Model.FromServer>().Add(1, FRS2Model.FromServer.exists).Add(2, FRS2Model.FromServer.exists).Add(3, FRS2Model.FromServer.exists).Add(4, FRS2Model.FromServer.exists), new Microsoft.Modeling.Map<System.String,System.Int32>().Add("A", 1).Add("B", 2).Add("C", 3).Add("D", 4), new Microsoft.Modeling.Map<System.String,FRS2Model.ReplicationGroupTypes>().Add("A", FRS2Model.ReplicationGroupTypes.sysvol).Add("B", FRS2Model.ReplicationGroupTypes.protection).Add("C", FRS2Model.ReplicationGroupTypes.sysvol).Add("D", FRS2Model.ReplicationGroupTypes.sysvol), new Microsoft.Modeling.Map<System.Int32,System.String>().Add(1, "A").Add(2, "B").Add(3, "C").Add(4, "D"), new Microsoft.Modeling.Map<System.Int32,FRS2Model.accessLevels>().Add(1, FRS2Model.accessLevels.writeOnly).Add(2, FRS2Model.accessLevels.readWrite).Add(3, FRS2Model.accessLevels.readOnly).Add(4, FRS2Model.accessLevels.readWrite), new Microsoft.Modeling.Map<System.Int32,FRS2Model.connectionProperty>().Add(1, FRS2Model.connectionProperty.enabled).Add(2, FRS2Model.connectionProperty.disabled).Add(3, FRS2Model.connectionProperty.enabled).Add(4, FRS2Model.connectionProperty.enabled));
            this.Manager.Comment("reaching state \'S5\'");
            this.Manager.Comment("checking step \'return Initialization/ERROR_SUCCESS\'");
            this.Manager.Assert((temp13 == FRS2Model.error_status_t.ERROR_SUCCESS), String.Format("expected \'FRS2Model.error_status_t.ERROR_SUCCESS\', actual \'{0}\' (return of Initia" +
                        "lization, state S5)", TestManagerHelpers.Describe(temp13)));
            this.Manager.Comment("reaching state \'S14\'");
            FRS2Model.ProtocolVersionReturned temp14;
            FRS2Model.UpstreamFlagValueReturned temp15;
            FRS2Model.error_status_t temp16;
            this.Manager.Comment("executing step \'call EstablishConnection(\"A\",1,FRS_COMMUNICATION_PROTOCOL_VERSION" +
                    "_LONGHORN_SERVER,out _,out _)\'");
            temp16 = this.IFRS2ManagedAdapterInstance.EstablishConnection("A", 1, FRS2Model.ProtocolVersion.FRS_COMMUNICATION_PROTOCOL_VERSION_LONGHORN_SERVER, out temp14, out temp15);
            this.Manager.Checkpoint("MS-FRS2_R37");
            this.Manager.Checkpoint("MS-FRS2_R436");
            this.Manager.Checkpoint("MS-FRS2_R439");
            this.Manager.Comment("reaching state \'S20\'");
            this.Manager.Comment("checking step \'return EstablishConnection/[out Valid,out Valid]:ERROR_SUCCESS\'");
            this.Manager.Assert((temp14 == FRS2Model.ProtocolVersionReturned.Valid), String.Format("expected \'FRS2Model.ProtocolVersionReturned.Valid\', actual \'{0}\' (upstreamProtoco" +
                        "lVersion of EstablishConnection, state S20)", TestManagerHelpers.Describe(temp14)));
            this.Manager.Assert((temp15 == FRS2Model.UpstreamFlagValueReturned.Valid), String.Format("expected \'FRS2Model.UpstreamFlagValueReturned.Valid\', actual \'{0}\' (upstreamFlags" +
                        " of EstablishConnection, state S20)", TestManagerHelpers.Describe(temp15)));
            this.Manager.Assert((temp16 == FRS2Model.error_status_t.ERROR_SUCCESS), String.Format("expected \'FRS2Model.error_status_t.ERROR_SUCCESS\', actual \'{0}\' (return of Establ" +
                        "ishConnection, state S20)", TestManagerHelpers.Describe(temp16)));
            this.Manager.Comment("reaching state \'S26\'");
            FRS2Model.error_status_t temp17;
            this.Manager.Comment("executing step \'call EstablishSession(1,1)\'");
            temp17 = this.IFRS2ManagedAdapterInstance.EstablishSession(1, 1);
            this.Manager.Checkpoint("MS-FRS2_R455");
            this.Manager.Checkpoint("MS-FRS2_R458");
            this.Manager.Checkpoint("MS-FRS2_R448");
            this.Manager.Comment("reaching state \'S31\'");
            this.Manager.Comment("checking step \'return EstablishSession/ERROR_SUCCESS\'");
            this.Manager.Assert((temp17 == FRS2Model.error_status_t.ERROR_SUCCESS), String.Format("expected \'FRS2Model.error_status_t.ERROR_SUCCESS\', actual \'{0}\' (return of Establ" +
                        "ishSession, state S31)", TestManagerHelpers.Describe(temp17)));
            this.Manager.Comment("reaching state \'S35\'");
            FRS2Model.error_status_t temp18;
            this.Manager.Comment("executing step \'call AsyncPoll(1)\'");
            temp18 = this.IFRS2ManagedAdapterInstance.AsyncPoll(1);
            this.Manager.Checkpoint("MS-FRS2_R549");
            this.Manager.Checkpoint("MS-FRS2_R552");
            this.Manager.Comment("reaching state \'S39\'");
            this.Manager.Comment("checking step \'return AsyncPoll/ERROR_SUCCESS\'");
            this.Manager.Assert((temp18 == FRS2Model.error_status_t.ERROR_SUCCESS), String.Format("expected \'FRS2Model.error_status_t.ERROR_SUCCESS\', actual \'{0}\' (return of AsyncP" +
                        "oll, state S39)", TestManagerHelpers.Describe(temp18)));
            this.Manager.Comment("reaching state \'S42\'");
            FRS2Model.error_status_t temp19;
            this.Manager.Comment("executing step \'call RequestVersionVector(1,1,1,REQUEST_NORMAL_SYNC,CHANGE_NOTIFY" +
                    ",ValidValue)\'");
            temp19 = this.IFRS2ManagedAdapterInstance.RequestVersionVector(1, 1, 1, FRS2Model.VERSION_REQUEST_TYPE.REQUEST_NORMAL_SYNC, FRS2Model.VERSION_CHANGE_TYPE.CHANGE_NOTIFY, FRS2Model.VVGeneration.ValidValue);
            this.Manager.Checkpoint("MS-FRS2_R517");
            this.Manager.Checkpoint("MS-FRS2_R520");
            this.Manager.Checkpoint("MS-FRS2_R466");
            this.Manager.Comment("reaching state \'S45\'");
            this.Manager.Comment("checking step \'return RequestVersionVector/ERROR_SUCCESS\'");
            this.Manager.Assert((temp19 == FRS2Model.error_status_t.ERROR_SUCCESS), String.Format("expected \'FRS2Model.error_status_t.ERROR_SUCCESS\', actual \'{0}\' (return of Reques" +
                        "tVersionVector, state S45)", TestManagerHelpers.Describe(temp19)));
            TCtestScenario4S48();
            this.Manager.EndTest();
        }
        
        private void TCtestScenario4S48() {
            this.Manager.Comment("reaching state \'S48\'");
            int temp50 = this.Manager.ExpectEvent(this.QuiescenceTimeout, true, new ExpectedEvent(TCtestScenario4.AsyncPollResponseEventInfo, null, new AsyncPollResponseEventDelegate1(this.TCtestScenario4S4AsyncPollResponseEventChecker)), new ExpectedEvent(TCtestScenario4.AsyncPollResponseEventInfo, null, new AsyncPollResponseEventDelegate1(this.TCtestScenario4S4AsyncPollResponseEventChecker1)));
            if ((temp50 == 0)) {
                this.Manager.Comment("reaching state \'S52\'");
                FRS2Model.error_status_t temp20;
                this.Manager.Comment("executing step \'call RequestUpdates(1,1,valid)\'");
                temp20 = this.IFRS2ManagedAdapterInstance.RequestUpdates(1, 1, FRS2Model.versionVectorDiff.valid);
                this.Manager.Checkpoint("MS-FRS2_R486");
                this.Manager.Checkpoint("MS-FRS2_R489");
                this.Manager.Comment("reaching state \'S55\'");
                this.Manager.Comment("checking step \'return RequestUpdates/ERROR_SUCCESS\'");
                this.Manager.Assert((temp20 == FRS2Model.error_status_t.ERROR_SUCCESS), String.Format("expected \'FRS2Model.error_status_t.ERROR_SUCCESS\', actual \'{0}\' (return of Reques" +
                            "tUpdates, state S55)", TestManagerHelpers.Describe(temp20)));
                this.Manager.Comment("reaching state \'S57\'");
                int temp49 = this.Manager.ExpectEvent(this.QuiescenceTimeout, true, new ExpectedEvent(TCtestScenario4.RequestUpdatesEventInfo, null, new RequestUpdatesEventDelegate1(this.TCtestScenario4S4RequestUpdatesEventChecker)), new ExpectedEvent(TCtestScenario4.RequestUpdatesEventInfo, null, new RequestUpdatesEventDelegate1(this.TCtestScenario4S4RequestUpdatesEventChecker1)), new ExpectedEvent(TCtestScenario4.RequestUpdatesEventInfo, null, new RequestUpdatesEventDelegate1(this.TCtestScenario4S4RequestUpdatesEventChecker2)), new ExpectedEvent(TCtestScenario4.RequestUpdatesEventInfo, null, new RequestUpdatesEventDelegate1(this.TCtestScenario4S4RequestUpdatesEventChecker3)));
                if ((temp49 == 0)) {
                    this.Manager.Comment("reaching state \'S58\'");
                    FRS2Model.error_status_t temp21;
                    this.Manager.Comment("executing step \'call InitializeFileTransferAsync(1,1,True)\'");
                    temp21 = this.IFRS2ManagedAdapterInstance.InitializeFileTransferAsync(1, 1, true);
                    this.Manager.Checkpoint("MS-FRS2_R774");
                    this.Manager.Checkpoint("MS-FRS2_R777");
                    this.Manager.Checkpoint("MS-FRS2_R793");
                    this.Manager.AddReturn(InitializeFileTransferAsyncInfo, null, temp21);
                    TCtestScenario4S62();
                    goto label4;
                }
                if ((temp49 == 1)) {
                    this.Manager.Comment("reaching state \'S59\'");
                    FRS2Model.error_status_t temp30;
                    this.Manager.Comment("executing step \'call InitializeFileTransferAsync(1,1,True)\'");
                    temp30 = this.IFRS2ManagedAdapterInstance.InitializeFileTransferAsync(1, 1, true);
                    this.Manager.Checkpoint("MS-FRS2_R774");
                    this.Manager.Checkpoint("MS-FRS2_R777");
                    this.Manager.Checkpoint("MS-FRS2_R793");
                    this.Manager.Comment("reaching state \'S63\'");
                    this.Manager.Comment("checking step \'return InitializeFileTransferAsync/ERROR_SUCCESS\'");
                    this.Manager.Assert((temp30 == FRS2Model.error_status_t.ERROR_SUCCESS), String.Format("expected \'FRS2Model.error_status_t.ERROR_SUCCESS\', actual \'{0}\' (return of Initia" +
                                "lizeFileTransferAsync, state S63)", TestManagerHelpers.Describe(temp30)));
                    this.Manager.Comment("reaching state \'S67\'");
                    int temp38 = this.Manager.ExpectEvent(this.QuiescenceTimeout, true, new ExpectedEvent(TCtestScenario4.InitializeFileTransferAsyncEventInfo, null, new InitializeFileTransferAsyncEventDelegate1(this.TCtestScenario4S4InitializeFileTransferAsyncEventChecker8)), new ExpectedEvent(TCtestScenario4.InitializeFileTransferAsyncEventInfo, null, new InitializeFileTransferAsyncEventDelegate1(this.TCtestScenario4S4InitializeFileTransferAsyncEventChecker9)), new ExpectedEvent(TCtestScenario4.InitializeFileTransferAsyncEventInfo, null, new InitializeFileTransferAsyncEventDelegate1(this.TCtestScenario4S4InitializeFileTransferAsyncEventChecker10)), new ExpectedEvent(TCtestScenario4.InitializeFileTransferAsyncEventInfo, null, new InitializeFileTransferAsyncEventDelegate1(this.TCtestScenario4S4InitializeFileTransferAsyncEventChecker11)), new ExpectedEvent(TCtestScenario4.InitializeFileTransferAsyncEventInfo, null, new InitializeFileTransferAsyncEventDelegate1(this.TCtestScenario4S4InitializeFileTransferAsyncEventChecker12)), new ExpectedEvent(TCtestScenario4.InitializeFileTransferAsyncEventInfo, null, new InitializeFileTransferAsyncEventDelegate1(this.TCtestScenario4S4InitializeFileTransferAsyncEventChecker13)), new ExpectedEvent(TCtestScenario4.InitializeFileTransferAsyncEventInfo, null, new InitializeFileTransferAsyncEventDelegate1(this.TCtestScenario4S4InitializeFileTransferAsyncEventChecker14)), new ExpectedEvent(TCtestScenario4.InitializeFileTransferAsyncEventInfo, null, new InitializeFileTransferAsyncEventDelegate1(this.TCtestScenario4S4InitializeFileTransferAsyncEventChecker15)));
                    if ((temp38 == 0)) {
                        this.Manager.Comment("reaching state \'S77\'");
                        FRS2Model.error_status_t temp31;
                        this.Manager.Comment("executing step \'call RdcGetSignatures(valid)\'");
                        temp31 = this.IFRS2ManagedAdapterInstance.RdcGetSignatures(FRS2Model.offset.valid);
                        this.Manager.Checkpoint("MS-FRS2_R647");
                        this.Manager.Checkpoint("MS-FRS2_R658");
                        this.Manager.Checkpoint("MS-FRS2_R805");
                        this.Manager.Comment("reaching state \'S97\'");
                        this.Manager.Comment("checking step \'return RdcGetSignatures/ERROR_FAIL\'");
                        this.Manager.Assert((temp31 == FRS2Model.error_status_t.ERROR_FAIL), String.Format("expected \'FRS2Model.error_status_t.ERROR_FAIL\', actual \'{0}\' (return of RdcGetSig" +
                                    "natures, state S97)", TestManagerHelpers.Describe(temp31)));
                        this.Manager.Comment("reaching state \'S109\'");
                        goto label2;
                    }
                    if ((temp38 == 1)) {
                        this.Manager.Comment("reaching state \'S78\'");
                        FRS2Model.error_status_t temp32;
                        this.Manager.Comment("executing step \'call RdcGetSignatures(valid)\'");
                        temp32 = this.IFRS2ManagedAdapterInstance.RdcGetSignatures(FRS2Model.offset.valid);
                        this.Manager.Checkpoint("MS-FRS2_R647");
                        this.Manager.Checkpoint("MS-FRS2_R655");
                        this.Manager.Checkpoint("MS-FRS2_R656");
                        this.Manager.Checkpoint("MS-FRS2_R657");
                        this.Manager.Checkpoint("MS-FRS2_R658");
                        this.Manager.Checkpoint("MS-FRS2_R805");
                        this.Manager.Comment("reaching state \'S98\'");
                        this.Manager.Comment("checking step \'return RdcGetSignatures/ERROR_FAIL\'");
                        this.Manager.Assert((temp32 == FRS2Model.error_status_t.ERROR_FAIL), String.Format("expected \'FRS2Model.error_status_t.ERROR_FAIL\', actual \'{0}\' (return of RdcGetSig" +
                                    "natures, state S98)", TestManagerHelpers.Describe(temp32)));
                        this.Manager.Comment("reaching state \'S110\'");
                        goto label2;
                    }
                    if ((temp38 == 2)) {
                        this.Manager.Comment("reaching state \'S79\'");
                        goto label2;
                    }
                    if ((temp38 == 3)) {
                        this.Manager.Comment("reaching state \'S80\'");
                        FRS2Model.error_status_t temp33;
                        this.Manager.Comment("executing step \'call RdcGetSignatures(valid)\'");
                        temp33 = this.IFRS2ManagedAdapterInstance.RdcGetSignatures(FRS2Model.offset.valid);
                        this.Manager.Checkpoint("MS-FRS2_R647");
                        this.Manager.Checkpoint("MS-FRS2_R655");
                        this.Manager.Checkpoint("MS-FRS2_R656");
                        this.Manager.Checkpoint("MS-FRS2_R657");
                        this.Manager.Checkpoint("MS-FRS2_R805");
                        this.Manager.Comment("reaching state \'S99\'");
                        this.Manager.Comment("checking step \'return RdcGetSignatures/ERROR_FAIL\'");
                        this.Manager.Assert((temp33 == FRS2Model.error_status_t.ERROR_FAIL), String.Format("expected \'FRS2Model.error_status_t.ERROR_FAIL\', actual \'{0}\' (return of RdcGetSig" +
                                    "natures, state S99)", TestManagerHelpers.Describe(temp33)));
                        this.Manager.Comment("reaching state \'S111\'");
                        goto label2;
                    }
                    if ((temp38 == 4)) {
                        this.Manager.Comment("reaching state \'S81\'");
                        FRS2Model.error_status_t temp34;
                        this.Manager.Comment("executing step \'call RdcGetSignatures(valid)\'");
                        temp34 = this.IFRS2ManagedAdapterInstance.RdcGetSignatures(FRS2Model.offset.valid);
                        this.Manager.Checkpoint("MS-FRS2_R646");
                        this.Manager.Checkpoint("MS-FRS2_R649");
                        this.Manager.Checkpoint("MS-FRS2_R650");
                        this.Manager.Comment("reaching state \'S100\'");
                        this.Manager.Comment("checking step \'return RdcGetSignatures/ERROR_SUCCESS\'");
                        this.Manager.Assert((temp34 == FRS2Model.error_status_t.ERROR_SUCCESS), String.Format("expected \'FRS2Model.error_status_t.ERROR_SUCCESS\', actual \'{0}\' (return of RdcGet" +
                                    "Signatures, state S100)", TestManagerHelpers.Describe(temp34)));
                        this.Manager.Comment("reaching state \'S112\'");
                        FRS2Model.error_status_t temp35;
                        this.Manager.Comment("executing step \'call RdcPushSourceNeeds()\'");
                        temp35 = this.IFRS2ManagedAdapterInstance.RdcPushSourceNeeds();
                        this.Manager.Checkpoint("MS-FRS2_R671");
                        this.Manager.Checkpoint("MS-FRS2_R674");
                        this.Manager.Checkpoint("MS-FRS2_R693");
                        this.Manager.Checkpoint("MS-FRS2_R694");
                        this.Manager.Checkpoint("MS-FRS2_R683");
                        this.Manager.Checkpoint("MS-FRS2_R691");
                        this.Manager.Checkpoint("MS-FRS2_R692");
                        this.Manager.Checkpoint("MS-FRS2_R675");
                        this.Manager.Comment("reaching state \'S118\'");
                        this.Manager.Comment("checking step \'return RdcPushSourceNeeds/ERROR_SUCCESS\'");
                        this.Manager.Assert((temp35 == FRS2Model.error_status_t.ERROR_SUCCESS), String.Format("expected \'FRS2Model.error_status_t.ERROR_SUCCESS\', actual \'{0}\' (return of RdcPus" +
                                    "hSourceNeeds, state S118)", TestManagerHelpers.Describe(temp35)));
                        this.Manager.Comment("reaching state \'S121\'");
                        FRS2Model.error_status_t temp36;
                        this.Manager.Comment("executing step \'call RdcGetFileDataAsync()\'");
                        temp36 = this.IFRS2ManagedAdapterInstance.RdcGetFileDataAsync();
                        this.Manager.Checkpoint("MS-FRS2_R882");
                        this.Manager.Checkpoint("MS-FRS2_R885");
                        this.Manager.Checkpoint("MS-FRS2_R896");
                        this.Manager.Checkpoint("MS-FRS2_R886");
                        this.Manager.Comment("reaching state \'S124\'");
                        this.Manager.Comment("checking step \'return RdcGetFileDataAsync/ERROR_SUCCESS\'");
                        this.Manager.Assert((temp36 == FRS2Model.error_status_t.ERROR_SUCCESS), String.Format("expected \'FRS2Model.error_status_t.ERROR_SUCCESS\', actual \'{0}\' (return of RdcGet" +
                                    "FileDataAsync, state S124)", TestManagerHelpers.Describe(temp36)));
                        this.Manager.Comment("reaching state \'S127\'");
                        FRS2Model.error_status_t temp37;
                        this.Manager.Comment("executing step \'call RdcClose()\'");
                        temp37 = this.IFRS2ManagedAdapterInstance.RdcClose();
                        this.Manager.Checkpoint("MS-FRS2_R737");
                        this.Manager.Checkpoint("MS-FRS2_R739");
                        this.Manager.Comment("reaching state \'S130\'");
                        this.Manager.Comment("checking step \'return RdcClose/ERROR_SUCCESS\'");
                        this.Manager.Assert((temp37 == FRS2Model.error_status_t.ERROR_SUCCESS), String.Format("expected \'FRS2Model.error_status_t.ERROR_SUCCESS\', actual \'{0}\' (return of RdcClo" +
                                    "se, state S130)", TestManagerHelpers.Describe(temp37)));
                        this.Manager.Comment("reaching state \'S133\'");
                        goto label2;
                    }
                    if ((temp38 == 5)) {
                        this.Manager.Comment("reaching state \'S82\'");
                        goto label2;
                    }
                    if ((temp38 == 6)) {
                        this.Manager.Comment("reaching state \'S83\'");
                        goto label2;
                    }
                    if ((temp38 == 7)) {
                        this.Manager.Comment("reaching state \'S84\'");
                        goto label2;
                    }
                    throw new InvalidOperationException("never reached");
                label2:
;
                    goto label4;
                }
                if ((temp49 == 2)) {
                    this.Manager.Comment("reaching state \'S60\'");
                    FRS2Model.error_status_t temp39;
                    this.Manager.Comment("executing step \'call InitializeFileTransferAsync(1,1,True)\'");
                    temp39 = this.IFRS2ManagedAdapterInstance.InitializeFileTransferAsync(1, 1, true);
                    this.Manager.Checkpoint("MS-FRS2_R774");
                    this.Manager.Checkpoint("MS-FRS2_R777");
                    this.Manager.Checkpoint("MS-FRS2_R793");
                    this.Manager.Comment("reaching state \'S64\'");
                    this.Manager.Comment("checking step \'return InitializeFileTransferAsync/ERROR_SUCCESS\'");
                    this.Manager.Assert((temp39 == FRS2Model.error_status_t.ERROR_SUCCESS), String.Format("expected \'FRS2Model.error_status_t.ERROR_SUCCESS\', actual \'{0}\' (return of Initia" +
                                "lizeFileTransferAsync, state S64)", TestManagerHelpers.Describe(temp39)));
                    this.Manager.Comment("reaching state \'S68\'");
                    int temp47 = this.Manager.ExpectEvent(this.QuiescenceTimeout, true, new ExpectedEvent(TCtestScenario4.InitializeFileTransferAsyncEventInfo, null, new InitializeFileTransferAsyncEventDelegate1(this.TCtestScenario4S4InitializeFileTransferAsyncEventChecker16)), new ExpectedEvent(TCtestScenario4.InitializeFileTransferAsyncEventInfo, null, new InitializeFileTransferAsyncEventDelegate1(this.TCtestScenario4S4InitializeFileTransferAsyncEventChecker17)), new ExpectedEvent(TCtestScenario4.InitializeFileTransferAsyncEventInfo, null, new InitializeFileTransferAsyncEventDelegate1(this.TCtestScenario4S4InitializeFileTransferAsyncEventChecker18)), new ExpectedEvent(TCtestScenario4.InitializeFileTransferAsyncEventInfo, null, new InitializeFileTransferAsyncEventDelegate1(this.TCtestScenario4S4InitializeFileTransferAsyncEventChecker19)), new ExpectedEvent(TCtestScenario4.InitializeFileTransferAsyncEventInfo, null, new InitializeFileTransferAsyncEventDelegate1(this.TCtestScenario4S4InitializeFileTransferAsyncEventChecker20)), new ExpectedEvent(TCtestScenario4.InitializeFileTransferAsyncEventInfo, null, new InitializeFileTransferAsyncEventDelegate1(this.TCtestScenario4S4InitializeFileTransferAsyncEventChecker21)), new ExpectedEvent(TCtestScenario4.InitializeFileTransferAsyncEventInfo, null, new InitializeFileTransferAsyncEventDelegate1(this.TCtestScenario4S4InitializeFileTransferAsyncEventChecker22)), new ExpectedEvent(TCtestScenario4.InitializeFileTransferAsyncEventInfo, null, new InitializeFileTransferAsyncEventDelegate1(this.TCtestScenario4S4InitializeFileTransferAsyncEventChecker23)));
                    if ((temp47 == 0)) {
                        this.Manager.Comment("reaching state \'S85\'");
                        FRS2Model.error_status_t temp40;
                        this.Manager.Comment("executing step \'call RdcGetSignatures(valid)\'");
                        temp40 = this.IFRS2ManagedAdapterInstance.RdcGetSignatures(FRS2Model.offset.valid);
                        this.Manager.Checkpoint("MS-FRS2_R647");
                        this.Manager.Checkpoint("MS-FRS2_R658");
                        this.Manager.Checkpoint("MS-FRS2_R805");
                        this.Manager.Comment("reaching state \'S101\'");
                        this.Manager.Comment("checking step \'return RdcGetSignatures/ERROR_FAIL\'");
                        this.Manager.Assert((temp40 == FRS2Model.error_status_t.ERROR_FAIL), String.Format("expected \'FRS2Model.error_status_t.ERROR_FAIL\', actual \'{0}\' (return of RdcGetSig" +
                                    "natures, state S101)", TestManagerHelpers.Describe(temp40)));
                        this.Manager.Comment("reaching state \'S113\'");
                        goto label3;
                    }
                    if ((temp47 == 1)) {
                        this.Manager.Comment("reaching state \'S86\'");
                        FRS2Model.error_status_t temp41;
                        this.Manager.Comment("executing step \'call RdcGetSignatures(valid)\'");
                        temp41 = this.IFRS2ManagedAdapterInstance.RdcGetSignatures(FRS2Model.offset.valid);
                        this.Manager.Checkpoint("MS-FRS2_R647");
                        this.Manager.Checkpoint("MS-FRS2_R655");
                        this.Manager.Checkpoint("MS-FRS2_R656");
                        this.Manager.Checkpoint("MS-FRS2_R657");
                        this.Manager.Checkpoint("MS-FRS2_R658");
                        this.Manager.Checkpoint("MS-FRS2_R805");
                        this.Manager.Comment("reaching state \'S102\'");
                        this.Manager.Comment("checking step \'return RdcGetSignatures/ERROR_FAIL\'");
                        this.Manager.Assert((temp41 == FRS2Model.error_status_t.ERROR_FAIL), String.Format("expected \'FRS2Model.error_status_t.ERROR_FAIL\', actual \'{0}\' (return of RdcGetSig" +
                                    "natures, state S102)", TestManagerHelpers.Describe(temp41)));
                        this.Manager.Comment("reaching state \'S114\'");
                        goto label3;
                    }
                    if ((temp47 == 2)) {
                        this.Manager.Comment("reaching state \'S87\'");
                        goto label3;
                    }
                    if ((temp47 == 3)) {
                        this.Manager.Comment("reaching state \'S88\'");
                        goto label3;
                    }
                    if ((temp47 == 4)) {
                        this.Manager.Comment("reaching state \'S89\'");
                        FRS2Model.error_status_t temp42;
                        this.Manager.Comment("executing step \'call RdcGetSignatures(valid)\'");
                        temp42 = this.IFRS2ManagedAdapterInstance.RdcGetSignatures(FRS2Model.offset.valid);
                        this.Manager.Checkpoint("MS-FRS2_R647");
                        this.Manager.Checkpoint("MS-FRS2_R655");
                        this.Manager.Checkpoint("MS-FRS2_R656");
                        this.Manager.Checkpoint("MS-FRS2_R657");
                        this.Manager.Checkpoint("MS-FRS2_R805");
                        this.Manager.Comment("reaching state \'S103\'");
                        this.Manager.Comment("checking step \'return RdcGetSignatures/ERROR_FAIL\'");
                        this.Manager.Assert((temp42 == FRS2Model.error_status_t.ERROR_FAIL), String.Format("expected \'FRS2Model.error_status_t.ERROR_FAIL\', actual \'{0}\' (return of RdcGetSig" +
                                    "natures, state S103)", TestManagerHelpers.Describe(temp42)));
                        this.Manager.Comment("reaching state \'S115\'");
                        goto label3;
                    }
                    if ((temp47 == 5)) {
                        this.Manager.Comment("reaching state \'S90\'");
                        FRS2Model.error_status_t temp43;
                        this.Manager.Comment("executing step \'call RdcGetSignatures(valid)\'");
                        temp43 = this.IFRS2ManagedAdapterInstance.RdcGetSignatures(FRS2Model.offset.valid);
                        this.Manager.Checkpoint("MS-FRS2_R646");
                        this.Manager.Checkpoint("MS-FRS2_R649");
                        this.Manager.Checkpoint("MS-FRS2_R650");
                        this.Manager.Comment("reaching state \'S104\'");
                        this.Manager.Comment("checking step \'return RdcGetSignatures/ERROR_SUCCESS\'");
                        this.Manager.Assert((temp43 == FRS2Model.error_status_t.ERROR_SUCCESS), String.Format("expected \'FRS2Model.error_status_t.ERROR_SUCCESS\', actual \'{0}\' (return of RdcGet" +
                                    "Signatures, state S104)", TestManagerHelpers.Describe(temp43)));
                        this.Manager.Comment("reaching state \'S116\'");
                        FRS2Model.error_status_t temp44;
                        this.Manager.Comment("executing step \'call RdcPushSourceNeeds()\'");
                        temp44 = this.IFRS2ManagedAdapterInstance.RdcPushSourceNeeds();
                        this.Manager.Checkpoint("MS-FRS2_R671");
                        this.Manager.Checkpoint("MS-FRS2_R674");
                        this.Manager.Checkpoint("MS-FRS2_R693");
                        this.Manager.Checkpoint("MS-FRS2_R694");
                        this.Manager.Checkpoint("MS-FRS2_R683");
                        this.Manager.Checkpoint("MS-FRS2_R691");
                        this.Manager.Checkpoint("MS-FRS2_R692");
                        this.Manager.Checkpoint("MS-FRS2_R675");
                        this.Manager.Comment("reaching state \'S119\'");
                        this.Manager.Comment("checking step \'return RdcPushSourceNeeds/ERROR_SUCCESS\'");
                        this.Manager.Assert((temp44 == FRS2Model.error_status_t.ERROR_SUCCESS), String.Format("expected \'FRS2Model.error_status_t.ERROR_SUCCESS\', actual \'{0}\' (return of RdcPus" +
                                    "hSourceNeeds, state S119)", TestManagerHelpers.Describe(temp44)));
                        this.Manager.Comment("reaching state \'S122\'");
                        FRS2Model.error_status_t temp45;
                        this.Manager.Comment("executing step \'call RdcGetFileDataAsync()\'");
                        temp45 = this.IFRS2ManagedAdapterInstance.RdcGetFileDataAsync();
                        this.Manager.Checkpoint("MS-FRS2_R882");
                        this.Manager.Checkpoint("MS-FRS2_R885");
                        this.Manager.Checkpoint("MS-FRS2_R896");
                        this.Manager.Checkpoint("MS-FRS2_R886");
                        this.Manager.Comment("reaching state \'S125\'");
                        this.Manager.Comment("checking step \'return RdcGetFileDataAsync/ERROR_SUCCESS\'");
                        this.Manager.Assert((temp45 == FRS2Model.error_status_t.ERROR_SUCCESS), String.Format("expected \'FRS2Model.error_status_t.ERROR_SUCCESS\', actual \'{0}\' (return of RdcGet" +
                                    "FileDataAsync, state S125)", TestManagerHelpers.Describe(temp45)));
                        this.Manager.Comment("reaching state \'S128\'");
                        FRS2Model.error_status_t temp46;
                        this.Manager.Comment("executing step \'call RdcClose()\'");
                        temp46 = this.IFRS2ManagedAdapterInstance.RdcClose();
                        this.Manager.Checkpoint("MS-FRS2_R737");
                        this.Manager.Checkpoint("MS-FRS2_R739");
                        this.Manager.Comment("reaching state \'S131\'");
                        this.Manager.Comment("checking step \'return RdcClose/ERROR_SUCCESS\'");
                        this.Manager.Assert((temp46 == FRS2Model.error_status_t.ERROR_SUCCESS), String.Format("expected \'FRS2Model.error_status_t.ERROR_SUCCESS\', actual \'{0}\' (return of RdcClo" +
                                    "se, state S131)", TestManagerHelpers.Describe(temp46)));
                        this.Manager.Comment("reaching state \'S134\'");
                        goto label3;
                    }
                    if ((temp47 == 6)) {
                        this.Manager.Comment("reaching state \'S91\'");
                        goto label3;
                    }
                    if ((temp47 == 7)) {
                        this.Manager.Comment("reaching state \'S92\'");
                        goto label3;
                    }
                    throw new InvalidOperationException("never reached");
                label3:
;
                    goto label4;
                }
                if ((temp49 == 3)) {
                    this.Manager.Comment("reaching state \'S61\'");
                    FRS2Model.error_status_t temp48;
                    this.Manager.Comment("executing step \'call InitializeFileTransferAsync(1,1,True)\'");
                    temp48 = this.IFRS2ManagedAdapterInstance.InitializeFileTransferAsync(1, 1, true);
                    this.Manager.Checkpoint("MS-FRS2_R774");
                    this.Manager.Checkpoint("MS-FRS2_R777");
                    this.Manager.Checkpoint("MS-FRS2_R793");
                    this.Manager.AddReturn(InitializeFileTransferAsyncInfo, null, temp48);
                    TCtestScenario4S62();
                    goto label4;
                }
                throw new InvalidOperationException("never reached");
            label4:
;
                goto label5;
            }
            if ((temp50 == 1)) {
                TCtestScenario4S51();
                goto label5;
            }
            throw new InvalidOperationException("never reached");
        label5:
;
        }
        
        private void TCtestScenario4S4AsyncPollResponseEventChecker(FRS2Model.VVGeneration vvGen) {
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
        
        private void TCtestScenario4S4RequestUpdatesEventChecker(FRS2Model.FilePresense present, FRS2Model.UPDATE_STATUS updateStatus) {
            this.Manager.Comment("checking step \'event RequestUpdatesEvent(fileDeleted,UPDATE_STATUS_MORE)\'");
            try {
                this.Manager.Assert((present == FRS2Model.FilePresense.fileDeleted), String.Format("expected \'FRS2Model.FilePresense.fileDeleted\', actual \'{0}\' (present of RequestUp" +
                            "datesEvent, state S57)", TestManagerHelpers.Describe(present)));
                this.Manager.Assert((updateStatus == FRS2Model.UPDATE_STATUS.UPDATE_STATUS_MORE), String.Format("expected \'FRS2Model.UPDATE_STATUS.UPDATE_STATUS_MORE\', actual \'{0}\' (updateStatus" +
                            " of RequestUpdatesEvent, state S57)", TestManagerHelpers.Describe(updateStatus)));
            }
            catch (TransactionFailedException ) {
                this.Manager.Comment("This step would have covered MS-FRS2_R93");
                throw;
            }
            this.Manager.Checkpoint("MS-FRS2_R93");
        }
        
        private void TCtestScenario4S62() {
            this.Manager.Comment("reaching state \'S62\'");
            this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(TCtestScenario4.InitializeFileTransferAsyncInfo, null, new InitializeFileTransferAsyncDelegate1(this.TCtestScenario4S4InitializeFileTransferAsyncChecker)));
            this.Manager.Comment("reaching state \'S66\'");
            int temp29 = this.Manager.ExpectEvent(this.QuiescenceTimeout, true, new ExpectedEvent(TCtestScenario4.InitializeFileTransferAsyncEventInfo, null, new InitializeFileTransferAsyncEventDelegate1(this.TCtestScenario4S4InitializeFileTransferAsyncEventChecker)), new ExpectedEvent(TCtestScenario4.InitializeFileTransferAsyncEventInfo, null, new InitializeFileTransferAsyncEventDelegate1(this.TCtestScenario4S4InitializeFileTransferAsyncEventChecker1)), new ExpectedEvent(TCtestScenario4.InitializeFileTransferAsyncEventInfo, null, new InitializeFileTransferAsyncEventDelegate1(this.TCtestScenario4S4InitializeFileTransferAsyncEventChecker2)), new ExpectedEvent(TCtestScenario4.InitializeFileTransferAsyncEventInfo, null, new InitializeFileTransferAsyncEventDelegate1(this.TCtestScenario4S4InitializeFileTransferAsyncEventChecker3)), new ExpectedEvent(TCtestScenario4.InitializeFileTransferAsyncEventInfo, null, new InitializeFileTransferAsyncEventDelegate1(this.TCtestScenario4S4InitializeFileTransferAsyncEventChecker4)), new ExpectedEvent(TCtestScenario4.InitializeFileTransferAsyncEventInfo, null, new InitializeFileTransferAsyncEventDelegate1(this.TCtestScenario4S4InitializeFileTransferAsyncEventChecker5)), new ExpectedEvent(TCtestScenario4.InitializeFileTransferAsyncEventInfo, null, new InitializeFileTransferAsyncEventDelegate1(this.TCtestScenario4S4InitializeFileTransferAsyncEventChecker6)), new ExpectedEvent(TCtestScenario4.InitializeFileTransferAsyncEventInfo, null, new InitializeFileTransferAsyncEventDelegate1(this.TCtestScenario4S4InitializeFileTransferAsyncEventChecker7)));
            if ((temp29 == 0)) {
                this.Manager.Comment("reaching state \'S69\'");
                goto label1;
            }
            if ((temp29 == 1)) {
                this.Manager.Comment("reaching state \'S70\'");
                goto label1;
            }
            if ((temp29 == 2)) {
                this.Manager.Comment("reaching state \'S71\'");
                goto label1;
            }
            if ((temp29 == 3)) {
                this.Manager.Comment("reaching state \'S72\'");
                FRS2Model.error_status_t temp22;
                this.Manager.Comment("executing step \'call RdcGetSignatures(valid)\'");
                temp22 = this.IFRS2ManagedAdapterInstance.RdcGetSignatures(FRS2Model.offset.valid);
                this.Manager.Checkpoint("MS-FRS2_R646");
                this.Manager.Checkpoint("MS-FRS2_R649");
                this.Manager.Checkpoint("MS-FRS2_R650");
                this.Manager.Comment("reaching state \'S93\'");
                this.Manager.Comment("checking step \'return RdcGetSignatures/ERROR_SUCCESS\'");
                this.Manager.Assert((temp22 == FRS2Model.error_status_t.ERROR_SUCCESS), String.Format("expected \'FRS2Model.error_status_t.ERROR_SUCCESS\', actual \'{0}\' (return of RdcGet" +
                            "Signatures, state S93)", TestManagerHelpers.Describe(temp22)));
                this.Manager.Comment("reaching state \'S105\'");
                FRS2Model.error_status_t temp23;
                this.Manager.Comment("executing step \'call RdcPushSourceNeeds()\'");
                temp23 = this.IFRS2ManagedAdapterInstance.RdcPushSourceNeeds();
                this.Manager.Checkpoint("MS-FRS2_R671");
                this.Manager.Checkpoint("MS-FRS2_R674");
                this.Manager.Checkpoint("MS-FRS2_R693");
                this.Manager.Checkpoint("MS-FRS2_R694");
                this.Manager.Checkpoint("MS-FRS2_R683");
                this.Manager.Checkpoint("MS-FRS2_R691");
                this.Manager.Checkpoint("MS-FRS2_R692");
                this.Manager.Checkpoint("MS-FRS2_R675");
                this.Manager.Comment("reaching state \'S117\'");
                this.Manager.Comment("checking step \'return RdcPushSourceNeeds/ERROR_SUCCESS\'");
                this.Manager.Assert((temp23 == FRS2Model.error_status_t.ERROR_SUCCESS), String.Format("expected \'FRS2Model.error_status_t.ERROR_SUCCESS\', actual \'{0}\' (return of RdcPus" +
                            "hSourceNeeds, state S117)", TestManagerHelpers.Describe(temp23)));
                this.Manager.Comment("reaching state \'S120\'");
                FRS2Model.error_status_t temp24;
                this.Manager.Comment("executing step \'call RdcGetFileDataAsync()\'");
                temp24 = this.IFRS2ManagedAdapterInstance.RdcGetFileDataAsync();
                this.Manager.Checkpoint("MS-FRS2_R882");
                this.Manager.Checkpoint("MS-FRS2_R885");
                this.Manager.Checkpoint("MS-FRS2_R896");
                this.Manager.Checkpoint("MS-FRS2_R886");
                this.Manager.Comment("reaching state \'S123\'");
                this.Manager.Comment("checking step \'return RdcGetFileDataAsync/ERROR_SUCCESS\'");
                this.Manager.Assert((temp24 == FRS2Model.error_status_t.ERROR_SUCCESS), String.Format("expected \'FRS2Model.error_status_t.ERROR_SUCCESS\', actual \'{0}\' (return of RdcGet" +
                            "FileDataAsync, state S123)", TestManagerHelpers.Describe(temp24)));
                this.Manager.Comment("reaching state \'S126\'");
                FRS2Model.error_status_t temp25;
                this.Manager.Comment("executing step \'call RdcClose()\'");
                temp25 = this.IFRS2ManagedAdapterInstance.RdcClose();
                this.Manager.Checkpoint("MS-FRS2_R737");
                this.Manager.Checkpoint("MS-FRS2_R739");
                this.Manager.Comment("reaching state \'S129\'");
                this.Manager.Comment("checking step \'return RdcClose/ERROR_SUCCESS\'");
                this.Manager.Assert((temp25 == FRS2Model.error_status_t.ERROR_SUCCESS), String.Format("expected \'FRS2Model.error_status_t.ERROR_SUCCESS\', actual \'{0}\' (return of RdcClo" +
                            "se, state S129)", TestManagerHelpers.Describe(temp25)));
                this.Manager.Comment("reaching state \'S132\'");
                goto label1;
            }
            if ((temp29 == 4)) {
                this.Manager.Comment("reaching state \'S73\'");
                FRS2Model.error_status_t temp26;
                this.Manager.Comment("executing step \'call RdcGetSignatures(valid)\'");
                temp26 = this.IFRS2ManagedAdapterInstance.RdcGetSignatures(FRS2Model.offset.valid);
                this.Manager.Checkpoint("MS-FRS2_R647");
                this.Manager.Checkpoint("MS-FRS2_R655");
                this.Manager.Checkpoint("MS-FRS2_R656");
                this.Manager.Checkpoint("MS-FRS2_R657");
                this.Manager.Checkpoint("MS-FRS2_R805");
                this.Manager.Comment("reaching state \'S94\'");
                this.Manager.Comment("checking step \'return RdcGetSignatures/ERROR_FAIL\'");
                this.Manager.Assert((temp26 == FRS2Model.error_status_t.ERROR_FAIL), String.Format("expected \'FRS2Model.error_status_t.ERROR_FAIL\', actual \'{0}\' (return of RdcGetSig" +
                            "natures, state S94)", TestManagerHelpers.Describe(temp26)));
                this.Manager.Comment("reaching state \'S106\'");
                goto label1;
            }
            if ((temp29 == 5)) {
                this.Manager.Comment("reaching state \'S74\'");
                FRS2Model.error_status_t temp27;
                this.Manager.Comment("executing step \'call RdcGetSignatures(valid)\'");
                temp27 = this.IFRS2ManagedAdapterInstance.RdcGetSignatures(FRS2Model.offset.valid);
                this.Manager.Checkpoint("MS-FRS2_R647");
                this.Manager.Checkpoint("MS-FRS2_R658");
                this.Manager.Checkpoint("MS-FRS2_R805");
                this.Manager.Comment("reaching state \'S95\'");
                this.Manager.Comment("checking step \'return RdcGetSignatures/ERROR_FAIL\'");
                this.Manager.Assert((temp27 == FRS2Model.error_status_t.ERROR_FAIL), String.Format("expected \'FRS2Model.error_status_t.ERROR_FAIL\', actual \'{0}\' (return of RdcGetSig" +
                            "natures, state S95)", TestManagerHelpers.Describe(temp27)));
                this.Manager.Comment("reaching state \'S107\'");
                goto label1;
            }
            if ((temp29 == 6)) {
                this.Manager.Comment("reaching state \'S75\'");
                FRS2Model.error_status_t temp28;
                this.Manager.Comment("executing step \'call RdcGetSignatures(valid)\'");
                temp28 = this.IFRS2ManagedAdapterInstance.RdcGetSignatures(FRS2Model.offset.valid);
                this.Manager.Checkpoint("MS-FRS2_R647");
                this.Manager.Checkpoint("MS-FRS2_R655");
                this.Manager.Checkpoint("MS-FRS2_R656");
                this.Manager.Checkpoint("MS-FRS2_R657");
                this.Manager.Checkpoint("MS-FRS2_R658");
                this.Manager.Checkpoint("MS-FRS2_R805");
                this.Manager.Comment("reaching state \'S96\'");
                this.Manager.Comment("checking step \'return RdcGetSignatures/ERROR_FAIL\'");
                this.Manager.Assert((temp28 == FRS2Model.error_status_t.ERROR_FAIL), String.Format("expected \'FRS2Model.error_status_t.ERROR_FAIL\', actual \'{0}\' (return of RdcGetSig" +
                            "natures, state S96)", TestManagerHelpers.Describe(temp28)));
                this.Manager.Comment("reaching state \'S108\'");
                goto label1;
            }
            if ((temp29 == 7)) {
                this.Manager.Comment("reaching state \'S76\'");
                goto label1;
            }
            throw new InvalidOperationException("never reached");
        label1:
;
        }
        
        private void TCtestScenario4S4InitializeFileTransferAsyncChecker(FRS2Model.error_status_t @return) {
            this.Manager.Comment("checking step \'return InitializeFileTransferAsync/ERROR_SUCCESS\'");
            this.Manager.Assert((@return == FRS2Model.error_status_t.ERROR_SUCCESS), String.Format("expected \'FRS2Model.error_status_t.ERROR_SUCCESS\', actual \'{0}\' (return of Initia" +
                        "lizeFileTransferAsync, state S62)", TestManagerHelpers.Describe(@return)));
        }
        
        private void TCtestScenario4S4InitializeFileTransferAsyncEventChecker(FRS2Model.ServerContext context, FRS2Model.RDC_Sig_Level rdcsigLevel, bool isEOF) {
            this.Manager.Comment("checking step \'event InitializeFileTransferAsyncEvent(InvalidContext,forRAWFileLe" +
                    "vel,False)\'");
            this.Manager.Assert((context == FRS2Model.ServerContext.InvalidContext), String.Format("expected \'FRS2Model.ServerContext.InvalidContext\', actual \'{0}\' (context of Initi" +
                        "alizeFileTransferAsyncEvent, state S66)", TestManagerHelpers.Describe(context)));
            this.Manager.Assert((rdcsigLevel == FRS2Model.RDC_Sig_Level.forRAWFileLevel), String.Format("expected \'FRS2Model.RDC_Sig_Level.forRAWFileLevel\', actual \'{0}\' (rdcsigLevel of " +
                        "InitializeFileTransferAsyncEvent, state S66)", TestManagerHelpers.Describe(rdcsigLevel)));
            this.Manager.Assert((isEOF == false), String.Format("expected \'false\', actual \'{0}\' (isEOF of InitializeFileTransferAsyncEvent, state " +
                        "S66)", TestManagerHelpers.Describe(isEOF)));
        }
        
        private void TCtestScenario4S4InitializeFileTransferAsyncEventChecker1(FRS2Model.ServerContext context, FRS2Model.RDC_Sig_Level rdcsigLevel, bool isEOF) {
            this.Manager.Comment("checking step \'event InitializeFileTransferAsyncEvent(ValidContext,forRAWFileLeve" +
                    "l,False)\'");
            this.Manager.Assert((context == FRS2Model.ServerContext.ValidContext), String.Format("expected \'FRS2Model.ServerContext.ValidContext\', actual \'{0}\' (context of Initial" +
                        "izeFileTransferAsyncEvent, state S66)", TestManagerHelpers.Describe(context)));
            this.Manager.Assert((rdcsigLevel == FRS2Model.RDC_Sig_Level.forRAWFileLevel), String.Format("expected \'FRS2Model.RDC_Sig_Level.forRAWFileLevel\', actual \'{0}\' (rdcsigLevel of " +
                        "InitializeFileTransferAsyncEvent, state S66)", TestManagerHelpers.Describe(rdcsigLevel)));
            this.Manager.Assert((isEOF == false), String.Format("expected \'false\', actual \'{0}\' (isEOF of InitializeFileTransferAsyncEvent, state " +
                        "S66)", TestManagerHelpers.Describe(isEOF)));
        }
        
        private void TCtestScenario4S4InitializeFileTransferAsyncEventChecker2(FRS2Model.ServerContext context, FRS2Model.RDC_Sig_Level rdcsigLevel, bool isEOF) {
            this.Manager.Comment("checking step \'event InitializeFileTransferAsyncEvent(ValidContext,forRDCFileLeve" +
                    "l,True)\'");
            this.Manager.Assert((context == FRS2Model.ServerContext.ValidContext), String.Format("expected \'FRS2Model.ServerContext.ValidContext\', actual \'{0}\' (context of Initial" +
                        "izeFileTransferAsyncEvent, state S66)", TestManagerHelpers.Describe(context)));
            this.Manager.Assert((rdcsigLevel == FRS2Model.RDC_Sig_Level.forRDCFileLevel), String.Format("expected \'FRS2Model.RDC_Sig_Level.forRDCFileLevel\', actual \'{0}\' (rdcsigLevel of " +
                        "InitializeFileTransferAsyncEvent, state S66)", TestManagerHelpers.Describe(rdcsigLevel)));
            this.Manager.Assert((isEOF == true), String.Format("expected \'true\', actual \'{0}\' (isEOF of InitializeFileTransferAsyncEvent, state S" +
                        "66)", TestManagerHelpers.Describe(isEOF)));
        }
        
        private void TCtestScenario4S4InitializeFileTransferAsyncEventChecker3(FRS2Model.ServerContext context, FRS2Model.RDC_Sig_Level rdcsigLevel, bool isEOF) {
            this.Manager.Comment("checking step \'event InitializeFileTransferAsyncEvent(ValidContext,forRDCFileLeve" +
                    "l,False)\'");
            this.Manager.Assert((context == FRS2Model.ServerContext.ValidContext), String.Format("expected \'FRS2Model.ServerContext.ValidContext\', actual \'{0}\' (context of Initial" +
                        "izeFileTransferAsyncEvent, state S66)", TestManagerHelpers.Describe(context)));
            this.Manager.Assert((rdcsigLevel == FRS2Model.RDC_Sig_Level.forRDCFileLevel), String.Format("expected \'FRS2Model.RDC_Sig_Level.forRDCFileLevel\', actual \'{0}\' (rdcsigLevel of " +
                        "InitializeFileTransferAsyncEvent, state S66)", TestManagerHelpers.Describe(rdcsigLevel)));
            this.Manager.Assert((isEOF == false), String.Format("expected \'false\', actual \'{0}\' (isEOF of InitializeFileTransferAsyncEvent, state " +
                        "S66)", TestManagerHelpers.Describe(isEOF)));
        }
        
        private void TCtestScenario4S4InitializeFileTransferAsyncEventChecker4(FRS2Model.ServerContext context, FRS2Model.RDC_Sig_Level rdcsigLevel, bool isEOF) {
            this.Manager.Comment("checking step \'event InitializeFileTransferAsyncEvent(InvalidContext,forRDCFileLe" +
                    "vel,True)\'");
            this.Manager.Assert((context == FRS2Model.ServerContext.InvalidContext), String.Format("expected \'FRS2Model.ServerContext.InvalidContext\', actual \'{0}\' (context of Initi" +
                        "alizeFileTransferAsyncEvent, state S66)", TestManagerHelpers.Describe(context)));
            this.Manager.Assert((rdcsigLevel == FRS2Model.RDC_Sig_Level.forRDCFileLevel), String.Format("expected \'FRS2Model.RDC_Sig_Level.forRDCFileLevel\', actual \'{0}\' (rdcsigLevel of " +
                        "InitializeFileTransferAsyncEvent, state S66)", TestManagerHelpers.Describe(rdcsigLevel)));
            this.Manager.Assert((isEOF == true), String.Format("expected \'true\', actual \'{0}\' (isEOF of InitializeFileTransferAsyncEvent, state S" +
                        "66)", TestManagerHelpers.Describe(isEOF)));
        }
        
        private void TCtestScenario4S4InitializeFileTransferAsyncEventChecker5(FRS2Model.ServerContext context, FRS2Model.RDC_Sig_Level rdcsigLevel, bool isEOF) {
            this.Manager.Comment("checking step \'event InitializeFileTransferAsyncEvent(ValidContext,forRAWFileLeve" +
                    "l,True)\'");
            this.Manager.Assert((context == FRS2Model.ServerContext.ValidContext), String.Format("expected \'FRS2Model.ServerContext.ValidContext\', actual \'{0}\' (context of Initial" +
                        "izeFileTransferAsyncEvent, state S66)", TestManagerHelpers.Describe(context)));
            this.Manager.Assert((rdcsigLevel == FRS2Model.RDC_Sig_Level.forRAWFileLevel), String.Format("expected \'FRS2Model.RDC_Sig_Level.forRAWFileLevel\', actual \'{0}\' (rdcsigLevel of " +
                        "InitializeFileTransferAsyncEvent, state S66)", TestManagerHelpers.Describe(rdcsigLevel)));
            this.Manager.Assert((isEOF == true), String.Format("expected \'true\', actual \'{0}\' (isEOF of InitializeFileTransferAsyncEvent, state S" +
                        "66)", TestManagerHelpers.Describe(isEOF)));
        }
        
        private void TCtestScenario4S4InitializeFileTransferAsyncEventChecker6(FRS2Model.ServerContext context, FRS2Model.RDC_Sig_Level rdcsigLevel, bool isEOF) {
            this.Manager.Comment("checking step \'event InitializeFileTransferAsyncEvent(InvalidContext,forRAWFileLe" +
                    "vel,True)\'");
            this.Manager.Assert((context == FRS2Model.ServerContext.InvalidContext), String.Format("expected \'FRS2Model.ServerContext.InvalidContext\', actual \'{0}\' (context of Initi" +
                        "alizeFileTransferAsyncEvent, state S66)", TestManagerHelpers.Describe(context)));
            this.Manager.Assert((rdcsigLevel == FRS2Model.RDC_Sig_Level.forRAWFileLevel), String.Format("expected \'FRS2Model.RDC_Sig_Level.forRAWFileLevel\', actual \'{0}\' (rdcsigLevel of " +
                        "InitializeFileTransferAsyncEvent, state S66)", TestManagerHelpers.Describe(rdcsigLevel)));
            this.Manager.Assert((isEOF == true), String.Format("expected \'true\', actual \'{0}\' (isEOF of InitializeFileTransferAsyncEvent, state S" +
                        "66)", TestManagerHelpers.Describe(isEOF)));
        }
        
        private void TCtestScenario4S4InitializeFileTransferAsyncEventChecker7(FRS2Model.ServerContext context, FRS2Model.RDC_Sig_Level rdcsigLevel, bool isEOF) {
            this.Manager.Comment("checking step \'event InitializeFileTransferAsyncEvent(InvalidContext,forRDCFileLe" +
                    "vel,False)\'");
            this.Manager.Assert((context == FRS2Model.ServerContext.InvalidContext), String.Format("expected \'FRS2Model.ServerContext.InvalidContext\', actual \'{0}\' (context of Initi" +
                        "alizeFileTransferAsyncEvent, state S66)", TestManagerHelpers.Describe(context)));
            this.Manager.Assert((rdcsigLevel == FRS2Model.RDC_Sig_Level.forRDCFileLevel), String.Format("expected \'FRS2Model.RDC_Sig_Level.forRDCFileLevel\', actual \'{0}\' (rdcsigLevel of " +
                        "InitializeFileTransferAsyncEvent, state S66)", TestManagerHelpers.Describe(rdcsigLevel)));
            this.Manager.Assert((isEOF == false), String.Format("expected \'false\', actual \'{0}\' (isEOF of InitializeFileTransferAsyncEvent, state " +
                        "S66)", TestManagerHelpers.Describe(isEOF)));
        }
        
        private void TCtestScenario4S4RequestUpdatesEventChecker1(FRS2Model.FilePresense present, FRS2Model.UPDATE_STATUS updateStatus) {
            this.Manager.Comment("checking step \'event RequestUpdatesEvent(fileExists,UPDATE_STATUS_MORE)\'");
            try {
                this.Manager.Assert((present == FRS2Model.FilePresense.fileExists), String.Format("expected \'FRS2Model.FilePresense.fileExists\', actual \'{0}\' (present of RequestUpd" +
                            "atesEvent, state S57)", TestManagerHelpers.Describe(present)));
                this.Manager.Assert((updateStatus == FRS2Model.UPDATE_STATUS.UPDATE_STATUS_MORE), String.Format("expected \'FRS2Model.UPDATE_STATUS.UPDATE_STATUS_MORE\', actual \'{0}\' (updateStatus" +
                            " of RequestUpdatesEvent, state S57)", TestManagerHelpers.Describe(updateStatus)));
            }
            catch (TransactionFailedException ) {
                this.Manager.Comment("This step would have covered MS-FRS2_R93, MS-FRS2_R498");
                throw;
            }
            this.Manager.Checkpoint("MS-FRS2_R93");
            this.Manager.Checkpoint("MS-FRS2_R498");
        }
        
        private void TCtestScenario4S4InitializeFileTransferAsyncEventChecker8(FRS2Model.ServerContext context, FRS2Model.RDC_Sig_Level rdcsigLevel, bool isEOF) {
            this.Manager.Comment("checking step \'event InitializeFileTransferAsyncEvent(ValidContext,forRAWFileLeve" +
                    "l,True)\'");
            this.Manager.Assert((context == FRS2Model.ServerContext.ValidContext), String.Format("expected \'FRS2Model.ServerContext.ValidContext\', actual \'{0}\' (context of Initial" +
                        "izeFileTransferAsyncEvent, state S67)", TestManagerHelpers.Describe(context)));
            this.Manager.Assert((rdcsigLevel == FRS2Model.RDC_Sig_Level.forRAWFileLevel), String.Format("expected \'FRS2Model.RDC_Sig_Level.forRAWFileLevel\', actual \'{0}\' (rdcsigLevel of " +
                        "InitializeFileTransferAsyncEvent, state S67)", TestManagerHelpers.Describe(rdcsigLevel)));
            this.Manager.Assert((isEOF == true), String.Format("expected \'true\', actual \'{0}\' (isEOF of InitializeFileTransferAsyncEvent, state S" +
                        "67)", TestManagerHelpers.Describe(isEOF)));
        }
        
        private void TCtestScenario4S4InitializeFileTransferAsyncEventChecker9(FRS2Model.ServerContext context, FRS2Model.RDC_Sig_Level rdcsigLevel, bool isEOF) {
            this.Manager.Comment("checking step \'event InitializeFileTransferAsyncEvent(InvalidContext,forRAWFileLe" +
                    "vel,True)\'");
            this.Manager.Assert((context == FRS2Model.ServerContext.InvalidContext), String.Format("expected \'FRS2Model.ServerContext.InvalidContext\', actual \'{0}\' (context of Initi" +
                        "alizeFileTransferAsyncEvent, state S67)", TestManagerHelpers.Describe(context)));
            this.Manager.Assert((rdcsigLevel == FRS2Model.RDC_Sig_Level.forRAWFileLevel), String.Format("expected \'FRS2Model.RDC_Sig_Level.forRAWFileLevel\', actual \'{0}\' (rdcsigLevel of " +
                        "InitializeFileTransferAsyncEvent, state S67)", TestManagerHelpers.Describe(rdcsigLevel)));
            this.Manager.Assert((isEOF == true), String.Format("expected \'true\', actual \'{0}\' (isEOF of InitializeFileTransferAsyncEvent, state S" +
                        "67)", TestManagerHelpers.Describe(isEOF)));
        }
        
        private void TCtestScenario4S4InitializeFileTransferAsyncEventChecker10(FRS2Model.ServerContext context, FRS2Model.RDC_Sig_Level rdcsigLevel, bool isEOF) {
            this.Manager.Comment("checking step \'event InitializeFileTransferAsyncEvent(InvalidContext,forRDCFileLe" +
                    "vel,False)\'");
            this.Manager.Assert((context == FRS2Model.ServerContext.InvalidContext), String.Format("expected \'FRS2Model.ServerContext.InvalidContext\', actual \'{0}\' (context of Initi" +
                        "alizeFileTransferAsyncEvent, state S67)", TestManagerHelpers.Describe(context)));
            this.Manager.Assert((rdcsigLevel == FRS2Model.RDC_Sig_Level.forRDCFileLevel), String.Format("expected \'FRS2Model.RDC_Sig_Level.forRDCFileLevel\', actual \'{0}\' (rdcsigLevel of " +
                        "InitializeFileTransferAsyncEvent, state S67)", TestManagerHelpers.Describe(rdcsigLevel)));
            this.Manager.Assert((isEOF == false), String.Format("expected \'false\', actual \'{0}\' (isEOF of InitializeFileTransferAsyncEvent, state " +
                        "S67)", TestManagerHelpers.Describe(isEOF)));
        }
        
        private void TCtestScenario4S4InitializeFileTransferAsyncEventChecker11(FRS2Model.ServerContext context, FRS2Model.RDC_Sig_Level rdcsigLevel, bool isEOF) {
            this.Manager.Comment("checking step \'event InitializeFileTransferAsyncEvent(InvalidContext,forRDCFileLe" +
                    "vel,True)\'");
            this.Manager.Assert((context == FRS2Model.ServerContext.InvalidContext), String.Format("expected \'FRS2Model.ServerContext.InvalidContext\', actual \'{0}\' (context of Initi" +
                        "alizeFileTransferAsyncEvent, state S67)", TestManagerHelpers.Describe(context)));
            this.Manager.Assert((rdcsigLevel == FRS2Model.RDC_Sig_Level.forRDCFileLevel), String.Format("expected \'FRS2Model.RDC_Sig_Level.forRDCFileLevel\', actual \'{0}\' (rdcsigLevel of " +
                        "InitializeFileTransferAsyncEvent, state S67)", TestManagerHelpers.Describe(rdcsigLevel)));
            this.Manager.Assert((isEOF == true), String.Format("expected \'true\', actual \'{0}\' (isEOF of InitializeFileTransferAsyncEvent, state S" +
                        "67)", TestManagerHelpers.Describe(isEOF)));
        }
        
        private void TCtestScenario4S4InitializeFileTransferAsyncEventChecker12(FRS2Model.ServerContext context, FRS2Model.RDC_Sig_Level rdcsigLevel, bool isEOF) {
            this.Manager.Comment("checking step \'event InitializeFileTransferAsyncEvent(ValidContext,forRDCFileLeve" +
                    "l,False)\'");
            this.Manager.Assert((context == FRS2Model.ServerContext.ValidContext), String.Format("expected \'FRS2Model.ServerContext.ValidContext\', actual \'{0}\' (context of Initial" +
                        "izeFileTransferAsyncEvent, state S67)", TestManagerHelpers.Describe(context)));
            this.Manager.Assert((rdcsigLevel == FRS2Model.RDC_Sig_Level.forRDCFileLevel), String.Format("expected \'FRS2Model.RDC_Sig_Level.forRDCFileLevel\', actual \'{0}\' (rdcsigLevel of " +
                        "InitializeFileTransferAsyncEvent, state S67)", TestManagerHelpers.Describe(rdcsigLevel)));
            this.Manager.Assert((isEOF == false), String.Format("expected \'false\', actual \'{0}\' (isEOF of InitializeFileTransferAsyncEvent, state " +
                        "S67)", TestManagerHelpers.Describe(isEOF)));
        }
        
        private void TCtestScenario4S4InitializeFileTransferAsyncEventChecker13(FRS2Model.ServerContext context, FRS2Model.RDC_Sig_Level rdcsigLevel, bool isEOF) {
            this.Manager.Comment("checking step \'event InitializeFileTransferAsyncEvent(InvalidContext,forRAWFileLe" +
                    "vel,False)\'");
            this.Manager.Assert((context == FRS2Model.ServerContext.InvalidContext), String.Format("expected \'FRS2Model.ServerContext.InvalidContext\', actual \'{0}\' (context of Initi" +
                        "alizeFileTransferAsyncEvent, state S67)", TestManagerHelpers.Describe(context)));
            this.Manager.Assert((rdcsigLevel == FRS2Model.RDC_Sig_Level.forRAWFileLevel), String.Format("expected \'FRS2Model.RDC_Sig_Level.forRAWFileLevel\', actual \'{0}\' (rdcsigLevel of " +
                        "InitializeFileTransferAsyncEvent, state S67)", TestManagerHelpers.Describe(rdcsigLevel)));
            this.Manager.Assert((isEOF == false), String.Format("expected \'false\', actual \'{0}\' (isEOF of InitializeFileTransferAsyncEvent, state " +
                        "S67)", TestManagerHelpers.Describe(isEOF)));
        }
        
        private void TCtestScenario4S4InitializeFileTransferAsyncEventChecker14(FRS2Model.ServerContext context, FRS2Model.RDC_Sig_Level rdcsigLevel, bool isEOF) {
            this.Manager.Comment("checking step \'event InitializeFileTransferAsyncEvent(ValidContext,forRAWFileLeve" +
                    "l,False)\'");
            this.Manager.Assert((context == FRS2Model.ServerContext.ValidContext), String.Format("expected \'FRS2Model.ServerContext.ValidContext\', actual \'{0}\' (context of Initial" +
                        "izeFileTransferAsyncEvent, state S67)", TestManagerHelpers.Describe(context)));
            this.Manager.Assert((rdcsigLevel == FRS2Model.RDC_Sig_Level.forRAWFileLevel), String.Format("expected \'FRS2Model.RDC_Sig_Level.forRAWFileLevel\', actual \'{0}\' (rdcsigLevel of " +
                        "InitializeFileTransferAsyncEvent, state S67)", TestManagerHelpers.Describe(rdcsigLevel)));
            this.Manager.Assert((isEOF == false), String.Format("expected \'false\', actual \'{0}\' (isEOF of InitializeFileTransferAsyncEvent, state " +
                        "S67)", TestManagerHelpers.Describe(isEOF)));
        }
        
        private void TCtestScenario4S4InitializeFileTransferAsyncEventChecker15(FRS2Model.ServerContext context, FRS2Model.RDC_Sig_Level rdcsigLevel, bool isEOF) {
            this.Manager.Comment("checking step \'event InitializeFileTransferAsyncEvent(ValidContext,forRDCFileLeve" +
                    "l,True)\'");
            this.Manager.Assert((context == FRS2Model.ServerContext.ValidContext), String.Format("expected \'FRS2Model.ServerContext.ValidContext\', actual \'{0}\' (context of Initial" +
                        "izeFileTransferAsyncEvent, state S67)", TestManagerHelpers.Describe(context)));
            this.Manager.Assert((rdcsigLevel == FRS2Model.RDC_Sig_Level.forRDCFileLevel), String.Format("expected \'FRS2Model.RDC_Sig_Level.forRDCFileLevel\', actual \'{0}\' (rdcsigLevel of " +
                        "InitializeFileTransferAsyncEvent, state S67)", TestManagerHelpers.Describe(rdcsigLevel)));
            this.Manager.Assert((isEOF == true), String.Format("expected \'true\', actual \'{0}\' (isEOF of InitializeFileTransferAsyncEvent, state S" +
                        "67)", TestManagerHelpers.Describe(isEOF)));
        }
        
        private void TCtestScenario4S4RequestUpdatesEventChecker2(FRS2Model.FilePresense present, FRS2Model.UPDATE_STATUS updateStatus) {
            this.Manager.Comment("checking step \'event RequestUpdatesEvent(fileExists,UPDATE_STATUS_DONE)\'");
            try {
                this.Manager.Assert((present == FRS2Model.FilePresense.fileExists), String.Format("expected \'FRS2Model.FilePresense.fileExists\', actual \'{0}\' (present of RequestUpd" +
                            "atesEvent, state S57)", TestManagerHelpers.Describe(present)));
                this.Manager.Assert((updateStatus == FRS2Model.UPDATE_STATUS.UPDATE_STATUS_DONE), String.Format("expected \'FRS2Model.UPDATE_STATUS.UPDATE_STATUS_DONE\', actual \'{0}\' (updateStatus" +
                            " of RequestUpdatesEvent, state S57)", TestManagerHelpers.Describe(updateStatus)));
            }
            catch (TransactionFailedException ) {
                this.Manager.Comment("This step would have covered MS-FRS2_R93, MS-FRS2_R94");
                throw;
            }
            this.Manager.Checkpoint("MS-FRS2_R93");
            this.Manager.Checkpoint("MS-FRS2_R94");
        }
        
        private void TCtestScenario4S4InitializeFileTransferAsyncEventChecker16(FRS2Model.ServerContext context, FRS2Model.RDC_Sig_Level rdcsigLevel, bool isEOF) {
            this.Manager.Comment("checking step \'event InitializeFileTransferAsyncEvent(ValidContext,forRAWFileLeve" +
                    "l,True)\'");
            this.Manager.Assert((context == FRS2Model.ServerContext.ValidContext), String.Format("expected \'FRS2Model.ServerContext.ValidContext\', actual \'{0}\' (context of Initial" +
                        "izeFileTransferAsyncEvent, state S68)", TestManagerHelpers.Describe(context)));
            this.Manager.Assert((rdcsigLevel == FRS2Model.RDC_Sig_Level.forRAWFileLevel), String.Format("expected \'FRS2Model.RDC_Sig_Level.forRAWFileLevel\', actual \'{0}\' (rdcsigLevel of " +
                        "InitializeFileTransferAsyncEvent, state S68)", TestManagerHelpers.Describe(rdcsigLevel)));
            this.Manager.Assert((isEOF == true), String.Format("expected \'true\', actual \'{0}\' (isEOF of InitializeFileTransferAsyncEvent, state S" +
                        "68)", TestManagerHelpers.Describe(isEOF)));
        }
        
        private void TCtestScenario4S4InitializeFileTransferAsyncEventChecker17(FRS2Model.ServerContext context, FRS2Model.RDC_Sig_Level rdcsigLevel, bool isEOF) {
            this.Manager.Comment("checking step \'event InitializeFileTransferAsyncEvent(InvalidContext,forRAWFileLe" +
                    "vel,True)\'");
            this.Manager.Assert((context == FRS2Model.ServerContext.InvalidContext), String.Format("expected \'FRS2Model.ServerContext.InvalidContext\', actual \'{0}\' (context of Initi" +
                        "alizeFileTransferAsyncEvent, state S68)", TestManagerHelpers.Describe(context)));
            this.Manager.Assert((rdcsigLevel == FRS2Model.RDC_Sig_Level.forRAWFileLevel), String.Format("expected \'FRS2Model.RDC_Sig_Level.forRAWFileLevel\', actual \'{0}\' (rdcsigLevel of " +
                        "InitializeFileTransferAsyncEvent, state S68)", TestManagerHelpers.Describe(rdcsigLevel)));
            this.Manager.Assert((isEOF == true), String.Format("expected \'true\', actual \'{0}\' (isEOF of InitializeFileTransferAsyncEvent, state S" +
                        "68)", TestManagerHelpers.Describe(isEOF)));
        }
        
        private void TCtestScenario4S4InitializeFileTransferAsyncEventChecker18(FRS2Model.ServerContext context, FRS2Model.RDC_Sig_Level rdcsigLevel, bool isEOF) {
            this.Manager.Comment("checking step \'event InitializeFileTransferAsyncEvent(InvalidContext,forRDCFileLe" +
                    "vel,False)\'");
            this.Manager.Assert((context == FRS2Model.ServerContext.InvalidContext), String.Format("expected \'FRS2Model.ServerContext.InvalidContext\', actual \'{0}\' (context of Initi" +
                        "alizeFileTransferAsyncEvent, state S68)", TestManagerHelpers.Describe(context)));
            this.Manager.Assert((rdcsigLevel == FRS2Model.RDC_Sig_Level.forRDCFileLevel), String.Format("expected \'FRS2Model.RDC_Sig_Level.forRDCFileLevel\', actual \'{0}\' (rdcsigLevel of " +
                        "InitializeFileTransferAsyncEvent, state S68)", TestManagerHelpers.Describe(rdcsigLevel)));
            this.Manager.Assert((isEOF == false), String.Format("expected \'false\', actual \'{0}\' (isEOF of InitializeFileTransferAsyncEvent, state " +
                        "S68)", TestManagerHelpers.Describe(isEOF)));
        }
        
        private void TCtestScenario4S4InitializeFileTransferAsyncEventChecker19(FRS2Model.ServerContext context, FRS2Model.RDC_Sig_Level rdcsigLevel, bool isEOF) {
            this.Manager.Comment("checking step \'event InitializeFileTransferAsyncEvent(ValidContext,forRDCFileLeve" +
                    "l,True)\'");
            this.Manager.Assert((context == FRS2Model.ServerContext.ValidContext), String.Format("expected \'FRS2Model.ServerContext.ValidContext\', actual \'{0}\' (context of Initial" +
                        "izeFileTransferAsyncEvent, state S68)", TestManagerHelpers.Describe(context)));
            this.Manager.Assert((rdcsigLevel == FRS2Model.RDC_Sig_Level.forRDCFileLevel), String.Format("expected \'FRS2Model.RDC_Sig_Level.forRDCFileLevel\', actual \'{0}\' (rdcsigLevel of " +
                        "InitializeFileTransferAsyncEvent, state S68)", TestManagerHelpers.Describe(rdcsigLevel)));
            this.Manager.Assert((isEOF == true), String.Format("expected \'true\', actual \'{0}\' (isEOF of InitializeFileTransferAsyncEvent, state S" +
                        "68)", TestManagerHelpers.Describe(isEOF)));
        }
        
        private void TCtestScenario4S4InitializeFileTransferAsyncEventChecker20(FRS2Model.ServerContext context, FRS2Model.RDC_Sig_Level rdcsigLevel, bool isEOF) {
            this.Manager.Comment("checking step \'event InitializeFileTransferAsyncEvent(InvalidContext,forRDCFileLe" +
                    "vel,True)\'");
            this.Manager.Assert((context == FRS2Model.ServerContext.InvalidContext), String.Format("expected \'FRS2Model.ServerContext.InvalidContext\', actual \'{0}\' (context of Initi" +
                        "alizeFileTransferAsyncEvent, state S68)", TestManagerHelpers.Describe(context)));
            this.Manager.Assert((rdcsigLevel == FRS2Model.RDC_Sig_Level.forRDCFileLevel), String.Format("expected \'FRS2Model.RDC_Sig_Level.forRDCFileLevel\', actual \'{0}\' (rdcsigLevel of " +
                        "InitializeFileTransferAsyncEvent, state S68)", TestManagerHelpers.Describe(rdcsigLevel)));
            this.Manager.Assert((isEOF == true), String.Format("expected \'true\', actual \'{0}\' (isEOF of InitializeFileTransferAsyncEvent, state S" +
                        "68)", TestManagerHelpers.Describe(isEOF)));
        }
        
        private void TCtestScenario4S4InitializeFileTransferAsyncEventChecker21(FRS2Model.ServerContext context, FRS2Model.RDC_Sig_Level rdcsigLevel, bool isEOF) {
            this.Manager.Comment("checking step \'event InitializeFileTransferAsyncEvent(ValidContext,forRDCFileLeve" +
                    "l,False)\'");
            this.Manager.Assert((context == FRS2Model.ServerContext.ValidContext), String.Format("expected \'FRS2Model.ServerContext.ValidContext\', actual \'{0}\' (context of Initial" +
                        "izeFileTransferAsyncEvent, state S68)", TestManagerHelpers.Describe(context)));
            this.Manager.Assert((rdcsigLevel == FRS2Model.RDC_Sig_Level.forRDCFileLevel), String.Format("expected \'FRS2Model.RDC_Sig_Level.forRDCFileLevel\', actual \'{0}\' (rdcsigLevel of " +
                        "InitializeFileTransferAsyncEvent, state S68)", TestManagerHelpers.Describe(rdcsigLevel)));
            this.Manager.Assert((isEOF == false), String.Format("expected \'false\', actual \'{0}\' (isEOF of InitializeFileTransferAsyncEvent, state " +
                        "S68)", TestManagerHelpers.Describe(isEOF)));
        }
        
        private void TCtestScenario4S4InitializeFileTransferAsyncEventChecker22(FRS2Model.ServerContext context, FRS2Model.RDC_Sig_Level rdcsigLevel, bool isEOF) {
            this.Manager.Comment("checking step \'event InitializeFileTransferAsyncEvent(InvalidContext,forRAWFileLe" +
                    "vel,False)\'");
            this.Manager.Assert((context == FRS2Model.ServerContext.InvalidContext), String.Format("expected \'FRS2Model.ServerContext.InvalidContext\', actual \'{0}\' (context of Initi" +
                        "alizeFileTransferAsyncEvent, state S68)", TestManagerHelpers.Describe(context)));
            this.Manager.Assert((rdcsigLevel == FRS2Model.RDC_Sig_Level.forRAWFileLevel), String.Format("expected \'FRS2Model.RDC_Sig_Level.forRAWFileLevel\', actual \'{0}\' (rdcsigLevel of " +
                        "InitializeFileTransferAsyncEvent, state S68)", TestManagerHelpers.Describe(rdcsigLevel)));
            this.Manager.Assert((isEOF == false), String.Format("expected \'false\', actual \'{0}\' (isEOF of InitializeFileTransferAsyncEvent, state " +
                        "S68)", TestManagerHelpers.Describe(isEOF)));
        }
        
        private void TCtestScenario4S4InitializeFileTransferAsyncEventChecker23(FRS2Model.ServerContext context, FRS2Model.RDC_Sig_Level rdcsigLevel, bool isEOF) {
            this.Manager.Comment("checking step \'event InitializeFileTransferAsyncEvent(ValidContext,forRAWFileLeve" +
                    "l,False)\'");
            this.Manager.Assert((context == FRS2Model.ServerContext.ValidContext), String.Format("expected \'FRS2Model.ServerContext.ValidContext\', actual \'{0}\' (context of Initial" +
                        "izeFileTransferAsyncEvent, state S68)", TestManagerHelpers.Describe(context)));
            this.Manager.Assert((rdcsigLevel == FRS2Model.RDC_Sig_Level.forRAWFileLevel), String.Format("expected \'FRS2Model.RDC_Sig_Level.forRAWFileLevel\', actual \'{0}\' (rdcsigLevel of " +
                        "InitializeFileTransferAsyncEvent, state S68)", TestManagerHelpers.Describe(rdcsigLevel)));
            this.Manager.Assert((isEOF == false), String.Format("expected \'false\', actual \'{0}\' (isEOF of InitializeFileTransferAsyncEvent, state " +
                        "S68)", TestManagerHelpers.Describe(isEOF)));
        }
        
        private void TCtestScenario4S4RequestUpdatesEventChecker3(FRS2Model.FilePresense present, FRS2Model.UPDATE_STATUS updateStatus) {
            this.Manager.Comment("checking step \'event RequestUpdatesEvent(fileDeleted,UPDATE_STATUS_DONE)\'");
            try {
                this.Manager.Assert((present == FRS2Model.FilePresense.fileDeleted), String.Format("expected \'FRS2Model.FilePresense.fileDeleted\', actual \'{0}\' (present of RequestUp" +
                            "datesEvent, state S57)", TestManagerHelpers.Describe(present)));
                this.Manager.Assert((updateStatus == FRS2Model.UPDATE_STATUS.UPDATE_STATUS_DONE), String.Format("expected \'FRS2Model.UPDATE_STATUS.UPDATE_STATUS_DONE\', actual \'{0}\' (updateStatus" +
                            " of RequestUpdatesEvent, state S57)", TestManagerHelpers.Describe(updateStatus)));
            }
            catch (TransactionFailedException ) {
                this.Manager.Comment("This step would have covered MS-FRS2_R93, MS-FRS2_R94");
                throw;
            }
            this.Manager.Checkpoint("MS-FRS2_R93");
            this.Manager.Checkpoint("MS-FRS2_R94");
        }
        
        private void TCtestScenario4S4AsyncPollResponseEventChecker1(FRS2Model.VVGeneration vvGen) {
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
        
        #region Test Starting in S6
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        [Microsoft.VisualStudio.TestTools.UnitTesting.DescriptionAttribute("MS-FRS2_R37, MS-FRS2_R436, MS-FRS2_R439, MS-FRS2_R549, MS-FRS2_R552")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestCategory("AD")]
        [TestCategory("MS-FRS2")]
        [TestCategory("SDC")]
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        
        public virtual void FRS2_TCtestScenario4S6()
        {
            this.Manager.BeginTest("TCtestScenario4S6");
            this.Manager.Comment("reaching state \'S6\'");
            FRS2Model.error_status_t temp51;
            this.Manager.Comment(@"executing step 'call Initialization(Windows7,Map{1=>enabled,2=>enabled,3=>enabled,4=>disabled},Map{1=>exists,2=>exists,3=>exists,4=>exists},Map{""A""=>1,""B""=>2,""C""=>3,""D""=>4},Map{""A""=>sysvol,""B""=>protection,""C""=>sysvol,""D""=>sysvol},Map{1=>""A"",2=>""B"",3=>""C"",4=>""D""},Map{1=>writeOnly,2=>readWrite,3=>readOnly,4=>readWrite},Map{1=>enabled,2=>disabled,3=>enabled,4=>enabled})'");
            temp51 = this.IFRS2ManagedAdapterInstance.Initialization(FRS2Model.OSVersion.Windows7, new Microsoft.Modeling.Map<System.Int32,FRS2Model.connectionProperty>().Add(1, FRS2Model.connectionProperty.enabled).Add(2, FRS2Model.connectionProperty.enabled).Add(3, FRS2Model.connectionProperty.enabled).Add(4, FRS2Model.connectionProperty.disabled), new Microsoft.Modeling.Map<System.Int32,FRS2Model.FromServer>().Add(1, FRS2Model.FromServer.exists).Add(2, FRS2Model.FromServer.exists).Add(3, FRS2Model.FromServer.exists).Add(4, FRS2Model.FromServer.exists), new Microsoft.Modeling.Map<System.String,System.Int32>().Add("A", 1).Add("B", 2).Add("C", 3).Add("D", 4), new Microsoft.Modeling.Map<System.String,FRS2Model.ReplicationGroupTypes>().Add("A", FRS2Model.ReplicationGroupTypes.sysvol).Add("B", FRS2Model.ReplicationGroupTypes.protection).Add("C", FRS2Model.ReplicationGroupTypes.sysvol).Add("D", FRS2Model.ReplicationGroupTypes.sysvol), new Microsoft.Modeling.Map<System.Int32,System.String>().Add(1, "A").Add(2, "B").Add(3, "C").Add(4, "D"), new Microsoft.Modeling.Map<System.Int32,FRS2Model.accessLevels>().Add(1, FRS2Model.accessLevels.writeOnly).Add(2, FRS2Model.accessLevels.readWrite).Add(3, FRS2Model.accessLevels.readOnly).Add(4, FRS2Model.accessLevels.readWrite), new Microsoft.Modeling.Map<System.Int32,FRS2Model.connectionProperty>().Add(1, FRS2Model.connectionProperty.enabled).Add(2, FRS2Model.connectionProperty.disabled).Add(3, FRS2Model.connectionProperty.enabled).Add(4, FRS2Model.connectionProperty.enabled));
            this.Manager.Comment("reaching state \'S7\'");
            this.Manager.Comment("checking step \'return Initialization/ERROR_SUCCESS\'");
            this.Manager.Assert((temp51 == FRS2Model.error_status_t.ERROR_SUCCESS), String.Format("expected \'FRS2Model.error_status_t.ERROR_SUCCESS\', actual \'{0}\' (return of Initia" +
                        "lization, state S7)", TestManagerHelpers.Describe(temp51)));
            this.Manager.Comment("reaching state \'S15\'");
            FRS2Model.ProtocolVersionReturned temp52;
            FRS2Model.UpstreamFlagValueReturned temp53;
            FRS2Model.error_status_t temp54;
            this.Manager.Comment("executing step \'call EstablishConnection(\"A\",1,FRS_COMMUNICATION_PROTOCOL_VERSION" +
                    "_LONGHORN_SERVER,out _,out _)\'");
            temp54 = this.IFRS2ManagedAdapterInstance.EstablishConnection("A", 1, FRS2Model.ProtocolVersion.FRS_COMMUNICATION_PROTOCOL_VERSION_LONGHORN_SERVER, out temp52, out temp53);
            this.Manager.Checkpoint("MS-FRS2_R37");
            this.Manager.Checkpoint("MS-FRS2_R436");
            this.Manager.Checkpoint("MS-FRS2_R439");
            this.Manager.Comment("reaching state \'S21\'");
            this.Manager.Comment("checking step \'return EstablishConnection/[out Valid,out Valid]:ERROR_SUCCESS\'");
            this.Manager.Assert((temp52 == FRS2Model.ProtocolVersionReturned.Valid), String.Format("expected \'FRS2Model.ProtocolVersionReturned.Valid\', actual \'{0}\' (upstreamProtoco" +
                        "lVersion of EstablishConnection, state S21)", TestManagerHelpers.Describe(temp52)));
            this.Manager.Assert((temp53 == FRS2Model.UpstreamFlagValueReturned.Valid), String.Format("expected \'FRS2Model.UpstreamFlagValueReturned.Valid\', actual \'{0}\' (upstreamFlags" +
                        " of EstablishConnection, state S21)", TestManagerHelpers.Describe(temp53)));
            this.Manager.Assert((temp54 == FRS2Model.error_status_t.ERROR_SUCCESS), String.Format("expected \'FRS2Model.error_status_t.ERROR_SUCCESS\', actual \'{0}\' (return of Establ" +
                        "ishConnection, state S21)", TestManagerHelpers.Describe(temp54)));
            this.Manager.Comment("reaching state \'S27\'");
            FRS2Model.error_status_t temp55;
            this.Manager.Comment("executing step \'call AsyncPoll(1)\'");
            temp55 = this.IFRS2ManagedAdapterInstance.AsyncPoll(1);
            this.Manager.Checkpoint("MS-FRS2_R549");
            this.Manager.Checkpoint("MS-FRS2_R552");
            this.Manager.Comment("reaching state \'S32\'");
            this.Manager.Comment("checking step \'return AsyncPoll/ERROR_SUCCESS\'");
            this.Manager.Assert((temp55 == FRS2Model.error_status_t.ERROR_SUCCESS), String.Format("expected \'FRS2Model.error_status_t.ERROR_SUCCESS\', actual \'{0}\' (return of AsyncP" +
                        "oll, state S32)", TestManagerHelpers.Describe(temp55)));
            this.Manager.Comment("reaching state \'S36\'");
            this.Manager.EndTest();
        }
        #endregion
        
        #region Test Starting in S8
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        [Microsoft.VisualStudio.TestTools.UnitTesting.DescriptionAttribute("MS-FRS2_R440, MS-FRS2_R447")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestCategory("AD")]
        [TestCategory("MS-FRS2")]
        [TestCategory("SDC")]
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        public virtual void FRS2_TCtestScenario4S8()
        {
            this.Manager.BeginTest("TCtestScenario4S8");
            this.Manager.Comment("reaching state \'S8\'");
            FRS2Model.error_status_t temp56;
            this.Manager.Comment(@"executing step 'call Initialization(Windows7,Map{1=>enabled,2=>enabled,3=>enabled,4=>disabled},Map{1=>exists,2=>exists,3=>exists,4=>exists},Map{""A""=>1,""B""=>2,""C""=>3,""D""=>4},Map{""A""=>sysvol,""B""=>protection,""C""=>sysvol,""D""=>sysvol},Map{1=>""A"",2=>""B"",3=>""C"",4=>""D""},Map{1=>writeOnly,2=>readWrite,3=>readOnly,4=>readWrite},Map{1=>enabled,2=>disabled,3=>enabled,4=>enabled})'");
            temp56 = this.IFRS2ManagedAdapterInstance.Initialization(FRS2Model.OSVersion.Windows7, new Microsoft.Modeling.Map<System.Int32,FRS2Model.connectionProperty>().Add(1, FRS2Model.connectionProperty.enabled).Add(2, FRS2Model.connectionProperty.enabled).Add(3, FRS2Model.connectionProperty.enabled).Add(4, FRS2Model.connectionProperty.disabled), new Microsoft.Modeling.Map<System.Int32,FRS2Model.FromServer>().Add(1, FRS2Model.FromServer.exists).Add(2, FRS2Model.FromServer.exists).Add(3, FRS2Model.FromServer.exists).Add(4, FRS2Model.FromServer.exists), new Microsoft.Modeling.Map<System.String,System.Int32>().Add("A", 1).Add("B", 2).Add("C", 3).Add("D", 4), new Microsoft.Modeling.Map<System.String,FRS2Model.ReplicationGroupTypes>().Add("A", FRS2Model.ReplicationGroupTypes.sysvol).Add("B", FRS2Model.ReplicationGroupTypes.protection).Add("C", FRS2Model.ReplicationGroupTypes.sysvol).Add("D", FRS2Model.ReplicationGroupTypes.sysvol), new Microsoft.Modeling.Map<System.Int32,System.String>().Add(1, "A").Add(2, "B").Add(3, "C").Add(4, "D"), new Microsoft.Modeling.Map<System.Int32,FRS2Model.accessLevels>().Add(1, FRS2Model.accessLevels.writeOnly).Add(2, FRS2Model.accessLevels.readWrite).Add(3, FRS2Model.accessLevels.readOnly).Add(4, FRS2Model.accessLevels.readWrite), new Microsoft.Modeling.Map<System.Int32,FRS2Model.connectionProperty>().Add(1, FRS2Model.connectionProperty.enabled).Add(2, FRS2Model.connectionProperty.disabled).Add(3, FRS2Model.connectionProperty.enabled).Add(4, FRS2Model.connectionProperty.enabled));
            this.Manager.Comment("reaching state \'S9\'");
            this.Manager.Comment("checking step \'return Initialization/ERROR_SUCCESS\'");
            this.Manager.Assert((temp56 == FRS2Model.error_status_t.ERROR_SUCCESS), String.Format("expected \'FRS2Model.error_status_t.ERROR_SUCCESS\', actual \'{0}\' (return of Initia" +
                        "lization, state S9)", TestManagerHelpers.Describe(temp56)));
            this.Manager.Comment("reaching state \'S16\'");
            FRS2Model.ProtocolVersionReturned temp57;
            FRS2Model.UpstreamFlagValueReturned temp58;
            FRS2Model.error_status_t temp59;
            this.Manager.Comment("executing step \'call EstablishConnection(\"A\",1,FRS_COMMUNICATION_PROTOCOL_VERSION" +
                    "_VISTA,out _,out _)\'");
            temp59 = this.IFRS2ManagedAdapterInstance.EstablishConnection("A", 1, FRS2Model.ProtocolVersion.FRS_COMMUNICATION_PROTOCOL_VERSION_VISTA, out temp57, out temp58);
            this.Manager.Checkpoint("MS-FRS2_R440");
            this.Manager.Checkpoint("MS-FRS2_R447");
            this.Manager.Comment("reaching state \'S22\'");
            this.Manager.Comment("checking step \'return EstablishConnection/[out Invalid,out Invalid]:FRS_ERROR_INC" +
                    "OMPATIBLE_VERSION\'");
            this.Manager.Assert((temp57 == FRS2Model.ProtocolVersionReturned.Invalid), String.Format("expected \'FRS2Model.ProtocolVersionReturned.Invalid\', actual \'{0}\' (upstreamProto" +
                        "colVersion of EstablishConnection, state S22)", TestManagerHelpers.Describe(temp57)));
            this.Manager.Assert((temp58 == FRS2Model.UpstreamFlagValueReturned.Invalid), String.Format("expected \'FRS2Model.UpstreamFlagValueReturned.Invalid\', actual \'{0}\' (upstreamFla" +
                        "gs of EstablishConnection, state S22)", TestManagerHelpers.Describe(temp58)));
            this.Manager.Assert((temp59 == FRS2Model.error_status_t.FRS_ERROR_INCOMPATIBLE_VERSION), String.Format("expected \'FRS2Model.error_status_t.FRS_ERROR_INCOMPATIBLE_VERSION\', actual \'{0}\' " +
                        "(return of EstablishConnection, state S22)", TestManagerHelpers.Describe(temp59)));
            TCtestScenario4S24();
            this.Manager.EndTest();
        }
        #endregion
        
        #region Test Starting in S10
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        [Microsoft.VisualStudio.TestTools.UnitTesting.DescriptionAttribute("MS-FRS2_R37, MS-FRS2_R436, MS-FRS2_R439, MS-FRS2_R455, MS-FRS2_R458, MS-FRS2_R448" +
            ", MS-FRS2_R549, MS-FRS2_R552, MS-FRS2_R517, MS-FRS2_R520, MS-FRS2_R466")]
        public virtual void FRS2_TCtestScenario4S10() {
            this.Manager.BeginTest("TCtestScenario4S10");
            this.Manager.Comment("reaching state \'S10\'");
            FRS2Model.error_status_t temp60;
            this.Manager.Comment(@"executing step 'call Initialization(Windows7,Map{1=>enabled,2=>enabled,3=>enabled,4=>disabled},Map{1=>exists,2=>exists,3=>exists,4=>exists},Map{""A""=>1,""B""=>2,""C""=>3,""D""=>4},Map{""A""=>sysvol,""B""=>protection,""C""=>sysvol,""D""=>sysvol},Map{1=>""A"",2=>""B"",3=>""C"",4=>""D""},Map{1=>writeOnly,2=>readWrite,3=>readOnly,4=>readWrite},Map{1=>enabled,2=>disabled,3=>enabled,4=>enabled})'");
            temp60 = this.IFRS2ManagedAdapterInstance.Initialization(FRS2Model.OSVersion.Windows7, new Microsoft.Modeling.Map<System.Int32,FRS2Model.connectionProperty>().Add(1, FRS2Model.connectionProperty.enabled).Add(2, FRS2Model.connectionProperty.enabled).Add(3, FRS2Model.connectionProperty.enabled).Add(4, FRS2Model.connectionProperty.disabled), new Microsoft.Modeling.Map<System.Int32,FRS2Model.FromServer>().Add(1, FRS2Model.FromServer.exists).Add(2, FRS2Model.FromServer.exists).Add(3, FRS2Model.FromServer.exists).Add(4, FRS2Model.FromServer.exists), new Microsoft.Modeling.Map<System.String,System.Int32>().Add("A", 1).Add("B", 2).Add("C", 3).Add("D", 4), new Microsoft.Modeling.Map<System.String,FRS2Model.ReplicationGroupTypes>().Add("A", FRS2Model.ReplicationGroupTypes.sysvol).Add("B", FRS2Model.ReplicationGroupTypes.protection).Add("C", FRS2Model.ReplicationGroupTypes.sysvol).Add("D", FRS2Model.ReplicationGroupTypes.sysvol), new Microsoft.Modeling.Map<System.Int32,System.String>().Add(1, "A").Add(2, "B").Add(3, "C").Add(4, "D"), new Microsoft.Modeling.Map<System.Int32,FRS2Model.accessLevels>().Add(1, FRS2Model.accessLevels.writeOnly).Add(2, FRS2Model.accessLevels.readWrite).Add(3, FRS2Model.accessLevels.readOnly).Add(4, FRS2Model.accessLevels.readWrite), new Microsoft.Modeling.Map<System.Int32,FRS2Model.connectionProperty>().Add(1, FRS2Model.connectionProperty.enabled).Add(2, FRS2Model.connectionProperty.disabled).Add(3, FRS2Model.connectionProperty.enabled).Add(4, FRS2Model.connectionProperty.enabled));
            this.Manager.Comment("reaching state \'S11\'");
            this.Manager.Comment("checking step \'return Initialization/ERROR_SUCCESS\'");
            this.Manager.Assert((temp60 == FRS2Model.error_status_t.ERROR_SUCCESS), String.Format("expected \'FRS2Model.error_status_t.ERROR_SUCCESS\', actual \'{0}\' (return of Initia" +
                        "lization, state S11)", TestManagerHelpers.Describe(temp60)));
            this.Manager.Comment("reaching state \'S17\'");
            FRS2Model.ProtocolVersionReturned temp61;
            FRS2Model.UpstreamFlagValueReturned temp62;
            FRS2Model.error_status_t temp63;
            this.Manager.Comment("executing step \'call EstablishConnection(\"A\",1,FRS_COMMUNICATION_PROTOCOL_VERSION" +
                    "_LONGHORN_SERVER,out _,out _)\'");
            temp63 = this.IFRS2ManagedAdapterInstance.EstablishConnection("A", 1, FRS2Model.ProtocolVersion.FRS_COMMUNICATION_PROTOCOL_VERSION_LONGHORN_SERVER, out temp61, out temp62);
            this.Manager.Checkpoint("MS-FRS2_R37");
            this.Manager.Checkpoint("MS-FRS2_R436");
            this.Manager.Checkpoint("MS-FRS2_R439");
            this.Manager.Comment("reaching state \'S23\'");
            this.Manager.Comment("checking step \'return EstablishConnection/[out Valid,out Valid]:ERROR_SUCCESS\'");
            this.Manager.Assert((temp61 == FRS2Model.ProtocolVersionReturned.Valid), String.Format("expected \'FRS2Model.ProtocolVersionReturned.Valid\', actual \'{0}\' (upstreamProtoco" +
                        "lVersion of EstablishConnection, state S23)", TestManagerHelpers.Describe(temp61)));
            this.Manager.Assert((temp62 == FRS2Model.UpstreamFlagValueReturned.Valid), String.Format("expected \'FRS2Model.UpstreamFlagValueReturned.Valid\', actual \'{0}\' (upstreamFlags" +
                        " of EstablishConnection, state S23)", TestManagerHelpers.Describe(temp62)));
            this.Manager.Assert((temp63 == FRS2Model.error_status_t.ERROR_SUCCESS), String.Format("expected \'FRS2Model.error_status_t.ERROR_SUCCESS\', actual \'{0}\' (return of Establ" +
                        "ishConnection, state S23)", TestManagerHelpers.Describe(temp63)));
            this.Manager.Comment("reaching state \'S29\'");
            FRS2Model.error_status_t temp64;
            this.Manager.Comment("executing step \'call EstablishSession(1,1)\'");
            temp64 = this.IFRS2ManagedAdapterInstance.EstablishSession(1, 1);
            this.Manager.Checkpoint("MS-FRS2_R455");
            this.Manager.Checkpoint("MS-FRS2_R458");
            this.Manager.Checkpoint("MS-FRS2_R448");
            this.Manager.Comment("reaching state \'S33\'");
            this.Manager.Comment("checking step \'return EstablishSession/ERROR_SUCCESS\'");
            this.Manager.Assert((temp64 == FRS2Model.error_status_t.ERROR_SUCCESS), String.Format("expected \'FRS2Model.error_status_t.ERROR_SUCCESS\', actual \'{0}\' (return of Establ" +
                        "ishSession, state S33)", TestManagerHelpers.Describe(temp64)));
            this.Manager.Comment("reaching state \'S37\'");
            FRS2Model.error_status_t temp65;
            this.Manager.Comment("executing step \'call AsyncPoll(1)\'");
            temp65 = this.IFRS2ManagedAdapterInstance.AsyncPoll(1);
            this.Manager.Checkpoint("MS-FRS2_R549");
            this.Manager.Checkpoint("MS-FRS2_R552");
            this.Manager.Comment("reaching state \'S40\'");
            this.Manager.Comment("checking step \'return AsyncPoll/ERROR_SUCCESS\'");
            this.Manager.Assert((temp65 == FRS2Model.error_status_t.ERROR_SUCCESS), String.Format("expected \'FRS2Model.error_status_t.ERROR_SUCCESS\', actual \'{0}\' (return of AsyncP" +
                        "oll, state S40)", TestManagerHelpers.Describe(temp65)));
            this.Manager.Comment("reaching state \'S43\'");
            FRS2Model.error_status_t temp66;
            this.Manager.Comment("executing step \'call RequestVersionVector(1,1,1,REQUEST_NORMAL_SYNC,CHANGE_ALL,Va" +
                    "lidValue)\'");
            temp66 = this.IFRS2ManagedAdapterInstance.RequestVersionVector(1, 1, 1, FRS2Model.VERSION_REQUEST_TYPE.REQUEST_NORMAL_SYNC, FRS2Model.VERSION_CHANGE_TYPE.CHANGE_ALL, FRS2Model.VVGeneration.ValidValue);
            this.Manager.Checkpoint("MS-FRS2_R517");
            this.Manager.Checkpoint("MS-FRS2_R520");
            this.Manager.Checkpoint("MS-FRS2_R466");
            this.Manager.Comment("reaching state \'S46\'");
            this.Manager.Comment("checking step \'return RequestVersionVector/ERROR_SUCCESS\'");
            this.Manager.Assert((temp66 == FRS2Model.error_status_t.ERROR_SUCCESS), String.Format("expected \'FRS2Model.error_status_t.ERROR_SUCCESS\', actual \'{0}\' (return of Reques" +
                        "tVersionVector, state S46)", TestManagerHelpers.Describe(temp66)));
            TCtestScenario4S48();
            this.Manager.EndTest();
        }
        #endregion
    }
}
