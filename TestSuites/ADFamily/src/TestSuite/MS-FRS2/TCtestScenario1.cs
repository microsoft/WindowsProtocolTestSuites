// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace GeneratedTests
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Reflection;
    using Microsoft.SpecExplorer.Runtime.Testing;
    using Microsoft.Protocols.TestTools;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Microsoft.Protocols.TestSuites.MS_FRS2;

    [Microsoft.VisualStudio.TestTools.UnitTesting.TestClassAttribute()]
    public partial class TCtestScenario1 : PtfTestClassBase
    {

        public TCtestScenario1()
        {
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

        public delegate void AsyncPollDelegate1(FRS2Model.error_status_t @return);

        public delegate void EstablishSessionDelegate1(FRS2Model.error_status_t @return);

        public delegate void RequestVersionVectorDelegate1(FRS2Model.error_status_t @return);

        public delegate void AsyncPollResponseEventDelegate1(FRS2Model.VVGeneration vvGen);

        public delegate void RequestUpdatesDelegate1(FRS2Model.error_status_t @return);

        public delegate void RequestUpdatesEventDelegate1(FRS2Model.FilePresense present, FRS2Model.UPDATE_STATUS updateStatus);

        public delegate void InitializeFileTransferAsyncDelegate1(FRS2Model.error_status_t @return);

        public delegate void InitializeFileTransferAsyncEventDelegate1(FRS2Model.ServerContext context, FRS2Model.RDC_Sig_Level rdcsigLevel, bool isEOF);

        public delegate void RawGetFileDataDelegate1(FRS2Model.error_status_t @return);

        public delegate void RawGetFileDataResponseEventDelegate1(bool isEOF);

        public delegate void RdcCloseDelegate1(FRS2Model.error_status_t @return);

        public delegate void CheckConnectivityDelegate1(FRS2Model.error_status_t @return);

        public delegate void RequestRecordsDelegate1(FRS2Model.error_status_t @return);

        public delegate void UpdateCancelDelegate1(FRS2Model.error_status_t @return);

        public delegate void RdcGetSignaturesDelegate1(FRS2Model.error_status_t @return);

        public delegate void RdcPushSourceNeedsDelegate1(FRS2Model.error_status_t @return);

        public delegate void RdcGetFileDataDelegate1(FRS2Model.error_status_t @return);

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

        public delegate void RdcGetFileDataEventDelegate1(FRS2Model.SizeReturned sizeReturned);

        public delegate void RequestRecordsEventDelegate1(FRS2Model.RecordsStatus status);
        #endregion

        #region Event Metadata
        static System.Reflection.MethodBase InitializationInfo = TestManagerHelpers.GetMethodInfo(typeof(Microsoft.Protocols.TestSuites.MS_FRS2.IFRS2ManagedAdapter), "Initialization", typeof(FRS2Model.OSVersion), typeof(Microsoft.Modeling.Map<System.Int32, FRS2Model.connectionProperty>), typeof(Microsoft.Modeling.Map<System.Int32, FRS2Model.FromServer>), typeof(Microsoft.Modeling.Map<System.String, System.Int32>), typeof(Microsoft.Modeling.Map<System.String, FRS2Model.ReplicationGroupTypes>), typeof(Microsoft.Modeling.Map<System.Int32, System.String>), typeof(Microsoft.Modeling.Map<System.Int32, FRS2Model.accessLevels>), typeof(Microsoft.Modeling.Map<System.Int32, FRS2Model.connectionProperty>));

        static System.Reflection.MethodBase EstablishConnectionInfo = TestManagerHelpers.GetMethodInfo(typeof(Microsoft.Protocols.TestSuites.MS_FRS2.IFRS2ManagedAdapter), "EstablishConnection", typeof(string), typeof(int), typeof(FRS2Model.ProtocolVersion), typeof(FRS2Model.ProtocolVersionReturned).MakeByRefType(), typeof(FRS2Model.UpstreamFlagValueReturned).MakeByRefType());

        static System.Reflection.MethodBase AsyncPollInfo = TestManagerHelpers.GetMethodInfo(typeof(Microsoft.Protocols.TestSuites.MS_FRS2.IFRS2ManagedAdapter), "AsyncPoll", typeof(int));

        static System.Reflection.MethodBase EstablishSessionInfo = TestManagerHelpers.GetMethodInfo(typeof(Microsoft.Protocols.TestSuites.MS_FRS2.IFRS2ManagedAdapter), "EstablishSession", typeof(int), typeof(int));

        static System.Reflection.MethodBase RequestVersionVectorInfo = TestManagerHelpers.GetMethodInfo(typeof(Microsoft.Protocols.TestSuites.MS_FRS2.IFRS2ManagedAdapter), "RequestVersionVector", typeof(int), typeof(int), typeof(int), typeof(FRS2Model.VERSION_REQUEST_TYPE), typeof(FRS2Model.VERSION_CHANGE_TYPE), typeof(FRS2Model.VVGeneration));

        static System.Reflection.EventInfo AsyncPollResponseEventInfo = TestManagerHelpers.GetEventInfo(typeof(Microsoft.Protocols.TestSuites.MS_FRS2.IFRS2ManagedAdapter), "AsyncPollResponseEvent");

        static System.Reflection.MethodBase RequestUpdatesInfo = TestManagerHelpers.GetMethodInfo(typeof(Microsoft.Protocols.TestSuites.MS_FRS2.IFRS2ManagedAdapter), "RequestUpdates", typeof(int), typeof(int), typeof(FRS2Model.versionVectorDiff));

        static System.Reflection.EventInfo RequestUpdatesEventInfo = TestManagerHelpers.GetEventInfo(typeof(Microsoft.Protocols.TestSuites.MS_FRS2.IFRS2ManagedAdapter), "RequestUpdatesEvent");

        static System.Reflection.MethodBase InitializeFileTransferAsyncInfo = TestManagerHelpers.GetMethodInfo(typeof(Microsoft.Protocols.TestSuites.MS_FRS2.IFRS2ManagedAdapter), "InitializeFileTransferAsync", typeof(int), typeof(int), typeof(bool));

        static System.Reflection.EventInfo InitializeFileTransferAsyncEventInfo = TestManagerHelpers.GetEventInfo(typeof(Microsoft.Protocols.TestSuites.MS_FRS2.IFRS2ManagedAdapter), "InitializeFileTransferAsyncEvent");

        static System.Reflection.MethodBase RawGetFileDataInfo = TestManagerHelpers.GetMethodInfo(typeof(Microsoft.Protocols.TestSuites.MS_FRS2.IFRS2ManagedAdapter), "RawGetFileData");

        static System.Reflection.EventInfo RawGetFileDataResponseEventInfo = TestManagerHelpers.GetEventInfo(typeof(Microsoft.Protocols.TestSuites.MS_FRS2.IFRS2ManagedAdapter), "RawGetFileDataResponseEvent");

        static System.Reflection.MethodBase RdcCloseInfo = TestManagerHelpers.GetMethodInfo(typeof(Microsoft.Protocols.TestSuites.MS_FRS2.IFRS2ManagedAdapter), "RdcClose");

        static System.Reflection.MethodBase CheckConnectivityInfo = TestManagerHelpers.GetMethodInfo(typeof(Microsoft.Protocols.TestSuites.MS_FRS2.IFRS2ManagedAdapter), "CheckConnectivity", typeof(string), typeof(int));

        static System.Reflection.MethodBase RequestRecordsInfo = TestManagerHelpers.GetMethodInfo(typeof(Microsoft.Protocols.TestSuites.MS_FRS2.IFRS2ManagedAdapter), "RequestRecords", typeof(int), typeof(int));

        static System.Reflection.MethodBase UpdateCancelInfo = TestManagerHelpers.GetMethodInfo(typeof(Microsoft.Protocols.TestSuites.MS_FRS2.IFRS2ManagedAdapter), "UpdateCancel", typeof(int), typeof(FRS2Model.FRS_UPDATE_CANCEL_DATA), typeof(int));

        static System.Reflection.MethodBase RdcGetSignaturesInfo = TestManagerHelpers.GetMethodInfo(typeof(Microsoft.Protocols.TestSuites.MS_FRS2.IFRS2ManagedAdapter), "RdcGetSignatures", typeof(FRS2Model.offset));

        static System.Reflection.MethodBase RdcPushSourceNeedsInfo = TestManagerHelpers.GetMethodInfo(typeof(Microsoft.Protocols.TestSuites.MS_FRS2.IFRS2ManagedAdapter), "RdcPushSourceNeeds");

        static System.Reflection.MethodBase RdcGetFileDataInfo = TestManagerHelpers.GetMethodInfo(typeof(Microsoft.Protocols.TestSuites.MS_FRS2.IFRS2ManagedAdapter), "RdcGetFileData", typeof(FRS2Model.BufferSize));

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

        static System.Reflection.EventInfo RdcGetFileDataEventInfo = TestManagerHelpers.GetEventInfo(typeof(Microsoft.Protocols.TestSuites.MS_FRS2.IFRS2ManagedAdapter), "RdcGetFileDataEvent");

        static System.Reflection.EventInfo RequestRecordsEventInfo = TestManagerHelpers.GetEventInfo(typeof(Microsoft.Protocols.TestSuites.MS_FRS2.IFRS2ManagedAdapter), "RequestRecordsEvent");
        #endregion

        #region Adapter Instances
        private Microsoft.Protocols.TestSuites.MS_FRS2.IFRS2ManagedAdapter IFRS2ManagedAdapterInstance;
        #endregion

        #region Class Initialization and Cleanup
        [Microsoft.VisualStudio.TestTools.UnitTesting.ClassInitializeAttribute()]
        public static void ClassInitialize(Microsoft.VisualStudio.TestTools.UnitTesting.TestContext context)
        {
            PtfTestClassBase.Initialize(context);
        }

        [Microsoft.VisualStudio.TestTools.UnitTesting.ClassCleanupAttribute()]
        public static void ClassCleanup()
        {
            PtfTestClassBase.Cleanup();
        }
        #endregion

        #region Test Initialization and Cleanup
        protected override void TestInitialize()
        {
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

        protected override void TestCleanup()
        {
            base.TestCleanup();
            this.CleanupTestManager();
        }
        #endregion

        #region Test Starting in S0
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        [Microsoft.VisualStudio.TestTools.UnitTesting.DescriptionAttribute("MS-FRS2_R37, MS-FRS2_R436, MS-FRS2_R439, MS-FRS2_R550, MS-FRS2_R554")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestCategory("AD")]
        [TestCategory("MS-FRS2")]
        [TestCategory("SDC")]
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        
        public virtual void FRS2_TCtestScenario1S0()
        {
            this.Manager.BeginTest("TCtestScenario1S0");
            this.Manager.Comment("reaching state \'S0\'");
            FRS2Model.error_status_t temp0;
            this.Manager.Comment(@"executing step 'call Initialization(Windows7,Map{1=>enabled,2=>enabled,3=>enabled,4=>disabled},Map{1=>exists,2=>exists,3=>exists,4=>exists},Map{""A""=>1,""B""=>2,""C""=>3,""D""=>4},Map{""A""=>sysvol,""B""=>protection,""C""=>sysvol,""D""=>sysvol},Map{1=>""A"",2=>""B"",3=>""C"",4=>""D""},Map{1=>writeOnly,2=>readWrite,3=>readOnly,4=>readWrite},Map{1=>enabled,2=>disabled,3=>enabled,4=>enabled})'");
            temp0 = this.IFRS2ManagedAdapterInstance.Initialization(FRS2Model.OSVersion.Windows7, new Microsoft.Modeling.Map<System.Int32, FRS2Model.connectionProperty>().Add(1, FRS2Model.connectionProperty.enabled).Add(2, FRS2Model.connectionProperty.enabled).Add(3, FRS2Model.connectionProperty.enabled).Add(4, FRS2Model.connectionProperty.disabled), new Microsoft.Modeling.Map<System.Int32, FRS2Model.FromServer>().Add(1, FRS2Model.FromServer.exists).Add(2, FRS2Model.FromServer.exists).Add(3, FRS2Model.FromServer.exists).Add(4, FRS2Model.FromServer.exists), new Microsoft.Modeling.Map<System.String, System.Int32>().Add("A", 1).Add("B", 2).Add("C", 3).Add("D", 4), new Microsoft.Modeling.Map<System.String, FRS2Model.ReplicationGroupTypes>().Add("A", FRS2Model.ReplicationGroupTypes.sysvol).Add("B", FRS2Model.ReplicationGroupTypes.protection).Add("C", FRS2Model.ReplicationGroupTypes.sysvol).Add("D", FRS2Model.ReplicationGroupTypes.sysvol), new Microsoft.Modeling.Map<System.Int32, System.String>().Add(1, "A").Add(2, "B").Add(3, "C").Add(4, "D"), new Microsoft.Modeling.Map<System.Int32, FRS2Model.accessLevels>().Add(1, FRS2Model.accessLevels.writeOnly).Add(2, FRS2Model.accessLevels.readWrite).Add(3, FRS2Model.accessLevels.readOnly).Add(4, FRS2Model.accessLevels.readWrite), new Microsoft.Modeling.Map<System.Int32, FRS2Model.connectionProperty>().Add(1, FRS2Model.connectionProperty.enabled).Add(2, FRS2Model.connectionProperty.disabled).Add(3, FRS2Model.connectionProperty.enabled).Add(4, FRS2Model.connectionProperty.enabled));
            this.Manager.Comment("reaching state \'S1\'");
            this.Manager.Comment("checking step \'return Initialization/ERROR_SUCCESS\'");
            this.Manager.Assert((temp0 == FRS2Model.error_status_t.ERROR_SUCCESS), String.Format("expected \'FRS2Model.error_status_t.ERROR_SUCCESS\', actual \'{0}\' (return of Initia" +
                        "lization, state S1)", TestManagerHelpers.Describe(temp0)));
            this.Manager.Comment("reaching state \'S36\'");
            FRS2Model.ProtocolVersionReturned temp1;
            FRS2Model.UpstreamFlagValueReturned temp2;
            FRS2Model.error_status_t temp3;
            this.Manager.Comment("executing step \'call EstablishConnection(\"A\",1,FRS_COMMUNICATION_PROTOCOL_VERSION" +
                    "_LONGHORN_SERVER,out _,out _)\'");
            temp3 = this.IFRS2ManagedAdapterInstance.EstablishConnection("A", 1, FRS2Model.ProtocolVersion.FRS_COMMUNICATION_PROTOCOL_VERSION_LONGHORN_SERVER, out temp1, out temp2);
            this.Manager.Checkpoint("MS-FRS2_R37");
            this.Manager.Checkpoint("MS-FRS2_R436");
            this.Manager.Checkpoint("MS-FRS2_R439");
            this.Manager.Comment("reaching state \'S54\'");
            this.Manager.Comment("checking step \'return EstablishConnection/[out Valid,out Valid]:ERROR_SUCCESS\'");
            this.Manager.Assert((temp1 == FRS2Model.ProtocolVersionReturned.Valid), String.Format("expected \'FRS2Model.ProtocolVersionReturned.Valid\', actual \'{0}\' (upstreamProtoco" +
                        "lVersion of EstablishConnection, state S54)", TestManagerHelpers.Describe(temp1)));
            this.Manager.Assert((temp2 == FRS2Model.UpstreamFlagValueReturned.Valid), String.Format("expected \'FRS2Model.UpstreamFlagValueReturned.Valid\', actual \'{0}\' (upstreamFlags" +
                        " of EstablishConnection, state S54)", TestManagerHelpers.Describe(temp2)));
            this.Manager.Assert((temp3 == FRS2Model.error_status_t.ERROR_SUCCESS), String.Format("expected \'FRS2Model.error_status_t.ERROR_SUCCESS\', actual \'{0}\' (return of Establ" +
                        "ishConnection, state S54)", TestManagerHelpers.Describe(temp3)));
            this.Manager.Comment("reaching state \'S72\'");
            FRS2Model.error_status_t temp4;
            this.Manager.Comment("executing step \'call AsyncPoll(2)\'");
            temp4 = this.IFRS2ManagedAdapterInstance.AsyncPoll(2);
            this.Manager.Checkpoint("MS-FRS2_R550");
            this.Manager.Checkpoint("MS-FRS2_R554");
            this.Manager.Comment("reaching state \'S90\'");
            this.Manager.Comment("checking step \'return AsyncPoll/ERROR_FAIL\'");
            this.Manager.Assert((temp4 == FRS2Model.error_status_t.ERROR_FAIL), String.Format("expected \'FRS2Model.error_status_t.ERROR_FAIL\', actual \'{0}\' (return of AsyncPoll" +
                        ", state S90)", TestManagerHelpers.Describe(temp4)));
            this.Manager.Comment("reaching state \'S108\'");
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S2
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        [Microsoft.VisualStudio.TestTools.UnitTesting.DescriptionAttribute(@"MS-FRS2_R37, MS-FRS2_R436, MS-FRS2_R439, MS-FRS2_R455, MS-FRS2_R458, MS-FRS2_R448, MS-FRS2_R549, MS-FRS2_R552, MS-FRS2_R517, MS-FRS2_R520, MS-FRS2_R466, MS-FRS2_R556, MS-FRS2_R1020, MS-FRS2_R555, MS-FRS2_R486, MS-FRS2_R489, MS-FRS2_R93, MS-FRS2_R774, MS-FRS2_R777, MS-FRS2_R793, MS-FRS2_R622, MS-FRS2_R625, MS-FRS2_R737, MS-FRS2_R739, MS-FRS2_R623, MS-FRS2_R629, MS-FRS2_R630, MS-FRS2_R804, MS-FRS2_R623, MS-FRS2_R629, MS-FRS2_R630, MS-FRS2_R804, MS-FRS2_R623, MS-FRS2_R804, MS-FRS2_R623, MS-FRS2_R629, MS-FRS2_R630, MS-FRS2_R804, MS-FRS2_R623, MS-FRS2_R804, MS-FRS2_R623, MS-FRS2_R629, MS-FRS2_R630, MS-FRS2_R804, MS-FRS2_R623, MS-FRS2_R804, MS-FRS2_R93, MS-FRS2_R94, MS-FRS2_R778, MS-FRS2_R783, MS-FRS2_R93, MS-FRS2_R94, MS-FRS2_R778, MS-FRS2_R783, MS-FRS2_R93, MS-FRS2_R498, MS-FRS2_R778, MS-FRS2_R783, MS-FRS2_R556, MS-FRS2_R1020, MS-FRS2_R555")]
        public virtual void FRS2_TCtestScenario1S2()
        {
            this.Manager.BeginTest("TCtestScenario1S2");
            this.Manager.Comment("reaching state \'S2\'");
            FRS2Model.error_status_t temp5;
            this.Manager.Comment(@"executing step 'call Initialization(Windows7,Map{1=>enabled,2=>enabled,3=>enabled,4=>disabled},Map{1=>exists,2=>exists,3=>exists,4=>exists},Map{""A""=>1,""B""=>2,""C""=>3,""D""=>4},Map{""A""=>sysvol,""B""=>protection,""C""=>sysvol,""D""=>sysvol},Map{1=>""A"",2=>""B"",3=>""C"",4=>""D""},Map{1=>writeOnly,2=>readWrite,3=>readOnly,4=>readWrite},Map{1=>enabled,2=>disabled,3=>enabled,4=>enabled})'");
            temp5 = this.IFRS2ManagedAdapterInstance.Initialization(FRS2Model.OSVersion.Windows7, new Microsoft.Modeling.Map<System.Int32, FRS2Model.connectionProperty>().Add(1, FRS2Model.connectionProperty.enabled).Add(2, FRS2Model.connectionProperty.enabled).Add(3, FRS2Model.connectionProperty.enabled).Add(4, FRS2Model.connectionProperty.disabled), new Microsoft.Modeling.Map<System.Int32, FRS2Model.FromServer>().Add(1, FRS2Model.FromServer.exists).Add(2, FRS2Model.FromServer.exists).Add(3, FRS2Model.FromServer.exists).Add(4, FRS2Model.FromServer.exists), new Microsoft.Modeling.Map<System.String, System.Int32>().Add("A", 1).Add("B", 2).Add("C", 3).Add("D", 4), new Microsoft.Modeling.Map<System.String, FRS2Model.ReplicationGroupTypes>().Add("A", FRS2Model.ReplicationGroupTypes.sysvol).Add("B", FRS2Model.ReplicationGroupTypes.protection).Add("C", FRS2Model.ReplicationGroupTypes.sysvol).Add("D", FRS2Model.ReplicationGroupTypes.sysvol), new Microsoft.Modeling.Map<System.Int32, System.String>().Add(1, "A").Add(2, "B").Add(3, "C").Add(4, "D"), new Microsoft.Modeling.Map<System.Int32, FRS2Model.accessLevels>().Add(1, FRS2Model.accessLevels.writeOnly).Add(2, FRS2Model.accessLevels.readWrite).Add(3, FRS2Model.accessLevels.readOnly).Add(4, FRS2Model.accessLevels.readWrite), new Microsoft.Modeling.Map<System.Int32, FRS2Model.connectionProperty>().Add(1, FRS2Model.connectionProperty.enabled).Add(2, FRS2Model.connectionProperty.disabled).Add(3, FRS2Model.connectionProperty.enabled).Add(4, FRS2Model.connectionProperty.enabled));
            this.Manager.Comment("reaching state \'S3\'");
            this.Manager.Comment("checking step \'return Initialization/ERROR_SUCCESS\'");
            this.Manager.Assert((temp5 == FRS2Model.error_status_t.ERROR_SUCCESS), String.Format("expected \'FRS2Model.error_status_t.ERROR_SUCCESS\', actual \'{0}\' (return of Initia" +
                        "lization, state S3)", TestManagerHelpers.Describe(temp5)));
            this.Manager.Comment("reaching state \'S37\'");
            FRS2Model.ProtocolVersionReturned temp6;
            FRS2Model.UpstreamFlagValueReturned temp7;
            FRS2Model.error_status_t temp8;
            this.Manager.Comment("executing step \'call EstablishConnection(\"A\",1,FRS_COMMUNICATION_PROTOCOL_VERSION" +
                    "_LONGHORN_SERVER,out _,out _)\'");
            temp8 = this.IFRS2ManagedAdapterInstance.EstablishConnection("A", 1, FRS2Model.ProtocolVersion.FRS_COMMUNICATION_PROTOCOL_VERSION_LONGHORN_SERVER, out temp6, out temp7);
            this.Manager.Checkpoint("MS-FRS2_R37");
            this.Manager.Checkpoint("MS-FRS2_R436");
            this.Manager.Checkpoint("MS-FRS2_R439");
            this.Manager.Comment("reaching state \'S55\'");
            this.Manager.Comment("checking step \'return EstablishConnection/[out Valid,out Valid]:ERROR_SUCCESS\'");
            this.Manager.Assert((temp6 == FRS2Model.ProtocolVersionReturned.Valid), String.Format("expected \'FRS2Model.ProtocolVersionReturned.Valid\', actual \'{0}\' (upstreamProtoco" +
                        "lVersion of EstablishConnection, state S55)", TestManagerHelpers.Describe(temp6)));
            this.Manager.Assert((temp7 == FRS2Model.UpstreamFlagValueReturned.Valid), String.Format("expected \'FRS2Model.UpstreamFlagValueReturned.Valid\', actual \'{0}\' (upstreamFlags" +
                        " of EstablishConnection, state S55)", TestManagerHelpers.Describe(temp7)));
            this.Manager.Assert((temp8 == FRS2Model.error_status_t.ERROR_SUCCESS), String.Format("expected \'FRS2Model.error_status_t.ERROR_SUCCESS\', actual \'{0}\' (return of Establ" +
                        "ishConnection, state S55)", TestManagerHelpers.Describe(temp8)));
            this.Manager.Comment("reaching state \'S73\'");
            FRS2Model.error_status_t temp9;
            this.Manager.Comment("executing step \'call EstablishSession(1,1)\'");
            temp9 = this.IFRS2ManagedAdapterInstance.EstablishSession(1, 1);
            this.Manager.Checkpoint("MS-FRS2_R455");
            this.Manager.Checkpoint("MS-FRS2_R458");
            this.Manager.Checkpoint("MS-FRS2_R448");
            this.Manager.Comment("reaching state \'S91\'");
            this.Manager.Comment("checking step \'return EstablishSession/ERROR_SUCCESS\'");
            this.Manager.Assert((temp9 == FRS2Model.error_status_t.ERROR_SUCCESS), String.Format("expected \'FRS2Model.error_status_t.ERROR_SUCCESS\', actual \'{0}\' (return of Establ" +
                        "ishSession, state S91)", TestManagerHelpers.Describe(temp9)));
            this.Manager.Comment("reaching state \'S109\'");
            FRS2Model.error_status_t temp10;
            this.Manager.Comment("executing step \'call AsyncPoll(1)\'");
            temp10 = this.IFRS2ManagedAdapterInstance.AsyncPoll(1);
            this.Manager.Checkpoint("MS-FRS2_R549");
            this.Manager.Checkpoint("MS-FRS2_R552");
            this.Manager.Comment("reaching state \'S126\'");
            this.Manager.Comment("checking step \'return AsyncPoll/ERROR_SUCCESS\'");
            this.Manager.Assert((temp10 == FRS2Model.error_status_t.ERROR_SUCCESS), String.Format("expected \'FRS2Model.error_status_t.ERROR_SUCCESS\', actual \'{0}\' (return of AsyncP" +
                        "oll, state S126)", TestManagerHelpers.Describe(temp10)));
            this.Manager.Comment("reaching state \'S142\'");
            FRS2Model.error_status_t temp11;
            this.Manager.Comment("executing step \'call RequestVersionVector(1,1,1,REQUEST_NORMAL_SYNC,CHANGE_NOTIFY" +
                    ",ValidValue)\'");
            temp11 = this.IFRS2ManagedAdapterInstance.RequestVersionVector(1, 1, 1, FRS2Model.VERSION_REQUEST_TYPE.REQUEST_NORMAL_SYNC, FRS2Model.VERSION_CHANGE_TYPE.CHANGE_NOTIFY, FRS2Model.VVGeneration.ValidValue);
            this.Manager.Checkpoint("MS-FRS2_R517");
            this.Manager.Checkpoint("MS-FRS2_R520");
            this.Manager.Checkpoint("MS-FRS2_R466");
            this.Manager.Comment("reaching state \'S158\'");
            this.Manager.Comment("checking step \'return RequestVersionVector/ERROR_SUCCESS\'");
            this.Manager.Assert((temp11 == FRS2Model.error_status_t.ERROR_SUCCESS), String.Format("expected \'FRS2Model.error_status_t.ERROR_SUCCESS\', actual \'{0}\' (return of Reques" +
                        "tVersionVector, state S158)", TestManagerHelpers.Describe(temp11)));
            this.Manager.Comment("reaching state \'S170\'");
            int temp29 = this.Manager.ExpectEvent(this.QuiescenceTimeout, true, new ExpectedEvent(TCtestScenario1.AsyncPollResponseEventInfo, null, new AsyncPollResponseEventDelegate1(this.TCtestScenario1S2AsyncPollResponseEventChecker)), new ExpectedEvent(TCtestScenario1.AsyncPollResponseEventInfo, null, new AsyncPollResponseEventDelegate1(this.TCtestScenario1S2AsyncPollResponseEventChecker1)));
            if ((temp29 == 0))
            {
                this.Manager.Comment("reaching state \'S178\'");
                FRS2Model.error_status_t temp12;
                this.Manager.Comment("executing step \'call RequestUpdates(1,1,valid)\'");
                temp12 = this.IFRS2ManagedAdapterInstance.RequestUpdates(1, 1, FRS2Model.versionVectorDiff.valid);
                this.Manager.Checkpoint("MS-FRS2_R486");
                this.Manager.Checkpoint("MS-FRS2_R489");
                this.Manager.Comment("reaching state \'S186\'");
                this.Manager.Comment("checking step \'return RequestUpdates/ERROR_SUCCESS\'");
                this.Manager.Assert((temp12 == FRS2Model.error_status_t.ERROR_SUCCESS), String.Format("expected \'FRS2Model.error_status_t.ERROR_SUCCESS\', actual \'{0}\' (return of Reques" +
                            "tUpdates, state S186)", TestManagerHelpers.Describe(temp12)));
                this.Manager.Comment("reaching state \'S190\'");
                int temp28 = this.Manager.ExpectEvent(this.QuiescenceTimeout, true, new ExpectedEvent(TCtestScenario1.RequestUpdatesEventInfo, null, new RequestUpdatesEventDelegate1(this.TCtestScenario1S2RequestUpdatesEventChecker)), new ExpectedEvent(TCtestScenario1.RequestUpdatesEventInfo, null, new RequestUpdatesEventDelegate1(this.TCtestScenario1S2RequestUpdatesEventChecker1)), new ExpectedEvent(TCtestScenario1.RequestUpdatesEventInfo, null, new RequestUpdatesEventDelegate1(this.TCtestScenario1S2RequestUpdatesEventChecker2)), new ExpectedEvent(TCtestScenario1.RequestUpdatesEventInfo, null, new RequestUpdatesEventDelegate1(this.TCtestScenario1S2RequestUpdatesEventChecker3)));
                if ((temp28 == 0))
                {
                    this.Manager.Comment("reaching state \'S194\'");
                    FRS2Model.error_status_t temp13;
                    this.Manager.Comment("executing step \'call InitializeFileTransferAsync(1,1,False)\'");
                    temp13 = this.IFRS2ManagedAdapterInstance.InitializeFileTransferAsync(1, 1, false);
                    this.Manager.Checkpoint("MS-FRS2_R774");
                    this.Manager.Checkpoint("MS-FRS2_R777");
                    this.Manager.Checkpoint("MS-FRS2_R793");
                    this.Manager.AddReturn(InitializeFileTransferAsyncInfo, null, temp13);
                    TCtestScenario1S206();
                    goto label2;
                }
                if ((temp28 == 1))
                {
                    this.Manager.Comment("reaching state \'S195\'");
                    FRS2Model.error_status_t temp25;
                    this.Manager.Comment("executing step \'call InitializeFileTransferAsync(4,1,False)\'");
                    temp25 = this.IFRS2ManagedAdapterInstance.InitializeFileTransferAsync(4, 1, false);
                    this.Manager.Checkpoint("MS-FRS2_R778");
                    this.Manager.Checkpoint("MS-FRS2_R783");
                    this.Manager.AddReturn(InitializeFileTransferAsyncInfo, null, temp25);
                    TCtestScenario1S207();
                    goto label2;
                }
                if ((temp28 == 2))
                {
                    this.Manager.Comment("reaching state \'S196\'");
                    FRS2Model.error_status_t temp26;
                    this.Manager.Comment("executing step \'call InitializeFileTransferAsync(4,1,False)\'");
                    temp26 = this.IFRS2ManagedAdapterInstance.InitializeFileTransferAsync(4, 1, false);
                    this.Manager.Checkpoint("MS-FRS2_R778");
                    this.Manager.Checkpoint("MS-FRS2_R783");
                    this.Manager.Comment("reaching state \'S208\'");
                    this.Manager.Comment("checking step \'return InitializeFileTransferAsync/FRS_ERROR_CONNECTION_INVALID\'");
                    this.Manager.Assert((temp26 == FRS2Model.error_status_t.FRS_ERROR_CONNECTION_INVALID), String.Format("expected \'FRS2Model.error_status_t.FRS_ERROR_CONNECTION_INVALID\', actual \'{0}\' (r" +
                                "eturn of InitializeFileTransferAsync, state S208)", TestManagerHelpers.Describe(temp26)));
                    TCtestScenario1S220();
                    goto label2;
                }
                if ((temp28 == 3))
                {
                    this.Manager.Comment("reaching state \'S197\'");
                    FRS2Model.error_status_t temp27;
                    this.Manager.Comment("executing step \'call InitializeFileTransferAsync(4,1,False)\'");
                    temp27 = this.IFRS2ManagedAdapterInstance.InitializeFileTransferAsync(4, 1, false);
                    this.Manager.Checkpoint("MS-FRS2_R778");
                    this.Manager.Checkpoint("MS-FRS2_R783");
                    this.Manager.Comment("reaching state \'S209\'");
                    this.Manager.Comment("checking step \'return InitializeFileTransferAsync/FRS_ERROR_CONNECTION_INVALID\'");
                    this.Manager.Assert((temp27 == FRS2Model.error_status_t.FRS_ERROR_CONNECTION_INVALID), String.Format("expected \'FRS2Model.error_status_t.FRS_ERROR_CONNECTION_INVALID\', actual \'{0}\' (r" +
                                "eturn of InitializeFileTransferAsync, state S209)", TestManagerHelpers.Describe(temp27)));
                    TCtestScenario1S221();
                    goto label2;
                }
                throw new InvalidOperationException("never reached");
            label2:
                ;
                goto label3;
            }
            if ((temp29 == 1))
            {
                TCtestScenario1S179();
                goto label3;
            }
            throw new InvalidOperationException("never reached");
        label3:
            ;
            this.Manager.EndTest();
        }

        private void TCtestScenario1S2AsyncPollResponseEventChecker(FRS2Model.VVGeneration vvGen)
        {
            this.Manager.Comment("checking step \'event AsyncPollResponseEvent(ValidValue)\'");
            try
            {
                this.Manager.Assert((vvGen == FRS2Model.VVGeneration.ValidValue), String.Format("expected \'FRS2Model.VVGeneration.ValidValue\', actual \'{0}\' (vvGen of AsyncPollRes" +
                            "ponseEvent, state S170)", TestManagerHelpers.Describe(vvGen)));
            }
            catch (TransactionFailedException)
            {
                this.Manager.Comment("This step would have covered MS-FRS2_R556, MS-FRS2_R1020, MS-FRS2_R555");
                throw;
            }
            this.Manager.Checkpoint("MS-FRS2_R556");
            this.Manager.Checkpoint("MS-FRS2_R1020");
            this.Manager.Checkpoint("MS-FRS2_R555");
        }

        private void TCtestScenario1S2RequestUpdatesEventChecker(FRS2Model.FilePresense present, FRS2Model.UPDATE_STATUS updateStatus)
        {
            this.Manager.Comment("checking step \'event RequestUpdatesEvent(fileDeleted,UPDATE_STATUS_MORE)\'");
            try
            {
                this.Manager.Assert((present == FRS2Model.FilePresense.fileDeleted), String.Format("expected \'FRS2Model.FilePresense.fileDeleted\', actual \'{0}\' (present of RequestUp" +
                            "datesEvent, state S190)", TestManagerHelpers.Describe(present)));
                this.Manager.Assert((updateStatus == FRS2Model.UPDATE_STATUS.UPDATE_STATUS_MORE), String.Format("expected \'FRS2Model.UPDATE_STATUS.UPDATE_STATUS_MORE\', actual \'{0}\' (updateStatus" +
                            " of RequestUpdatesEvent, state S190)", TestManagerHelpers.Describe(updateStatus)));
            }
            catch (TransactionFailedException)
            {
                this.Manager.Comment("This step would have covered MS-FRS2_R93");
                throw;
            }
            this.Manager.Checkpoint("MS-FRS2_R93");
        }

        private void TCtestScenario1S206()
        {
            this.Manager.Comment("reaching state \'S206\'");
            this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(TCtestScenario1.InitializeFileTransferAsyncInfo, null, new InitializeFileTransferAsyncDelegate1(this.TCtestScenario1S2InitializeFileTransferAsyncChecker)));
            this.Manager.Comment("reaching state \'S218\'");
            int temp24 = this.Manager.ExpectEvent(this.QuiescenceTimeout, true, new ExpectedEvent(TCtestScenario1.InitializeFileTransferAsyncEventInfo, null, new InitializeFileTransferAsyncEventDelegate1(this.TCtestScenario1S2InitializeFileTransferAsyncEventChecker)), new ExpectedEvent(TCtestScenario1.InitializeFileTransferAsyncEventInfo, null, new InitializeFileTransferAsyncEventDelegate1(this.TCtestScenario1S2InitializeFileTransferAsyncEventChecker1)), new ExpectedEvent(TCtestScenario1.InitializeFileTransferAsyncEventInfo, null, new InitializeFileTransferAsyncEventDelegate1(this.TCtestScenario1S2InitializeFileTransferAsyncEventChecker2)), new ExpectedEvent(TCtestScenario1.InitializeFileTransferAsyncEventInfo, null, new InitializeFileTransferAsyncEventDelegate1(this.TCtestScenario1S2InitializeFileTransferAsyncEventChecker3)), new ExpectedEvent(TCtestScenario1.InitializeFileTransferAsyncEventInfo, null, new InitializeFileTransferAsyncEventDelegate1(this.TCtestScenario1S2InitializeFileTransferAsyncEventChecker4)), new ExpectedEvent(TCtestScenario1.InitializeFileTransferAsyncEventInfo, null, new InitializeFileTransferAsyncEventDelegate1(this.TCtestScenario1S2InitializeFileTransferAsyncEventChecker5)), new ExpectedEvent(TCtestScenario1.InitializeFileTransferAsyncEventInfo, null, new InitializeFileTransferAsyncEventDelegate1(this.TCtestScenario1S2InitializeFileTransferAsyncEventChecker6)), new ExpectedEvent(TCtestScenario1.InitializeFileTransferAsyncEventInfo, null, new InitializeFileTransferAsyncEventDelegate1(this.TCtestScenario1S2InitializeFileTransferAsyncEventChecker7)));
            if ((temp24 == 0))
            {
                this.Manager.Comment("reaching state \'S227\'");
                FRS2Model.error_status_t temp14;
                this.Manager.Comment("executing step \'call RawGetFileData()\'");
                temp14 = this.IFRS2ManagedAdapterInstance.RawGetFileData();
                this.Manager.Checkpoint("MS-FRS2_R622");
                this.Manager.Checkpoint("MS-FRS2_R625");
                this.Manager.Comment("reaching state \'S251\'");
                this.Manager.Comment("checking step \'return RawGetFileData/ERROR_SUCCESS\'");
                this.Manager.Assert((temp14 == FRS2Model.error_status_t.ERROR_SUCCESS), String.Format("expected \'FRS2Model.error_status_t.ERROR_SUCCESS\', actual \'{0}\' (return of RawGet" +
                            "FileData, state S251)", TestManagerHelpers.Describe(temp14)));
                this.Manager.Comment("reaching state \'S275\'");
                int temp16 = this.Manager.ExpectEvent(this.QuiescenceTimeout, true, new ExpectedEvent(TCtestScenario1.RawGetFileDataResponseEventInfo, null, new RawGetFileDataResponseEventDelegate1(this.TCtestScenario1S2RawGetFileDataResponseEventChecker)), new ExpectedEvent(TCtestScenario1.RawGetFileDataResponseEventInfo, null, new RawGetFileDataResponseEventDelegate1(this.TCtestScenario1S2RawGetFileDataResponseEventChecker1)));
                if ((temp16 == 0))
                {
                    this.Manager.Comment("reaching state \'S299\'");
                    FRS2Model.error_status_t temp15;
                    this.Manager.Comment("executing step \'call RdcClose()\'");
                    temp15 = this.IFRS2ManagedAdapterInstance.RdcClose();
                    this.Manager.Checkpoint("MS-FRS2_R737");
                    this.Manager.Checkpoint("MS-FRS2_R739");
                    this.Manager.Comment("reaching state \'S305\'");
                    this.Manager.Comment("checking step \'return RdcClose/ERROR_SUCCESS\'");
                    this.Manager.Assert((temp15 == FRS2Model.error_status_t.ERROR_SUCCESS), String.Format("expected \'FRS2Model.error_status_t.ERROR_SUCCESS\', actual \'{0}\' (return of RdcClo" +
                                "se, state S305)", TestManagerHelpers.Describe(temp15)));
                    this.Manager.Comment("reaching state \'S308\'");
                    goto label0;
                }
                if ((temp16 == 1))
                {
                    this.Manager.Comment("reaching state \'S300\'");
                    goto label0;
                }
                throw new InvalidOperationException("never reached");
            label0:
                ;
                goto label1;
            }
            if ((temp24 == 1))
            {
                this.Manager.Comment("reaching state \'S228\'");
                FRS2Model.error_status_t temp17;
                this.Manager.Comment("executing step \'call RawGetFileData()\'");
                temp17 = this.IFRS2ManagedAdapterInstance.RawGetFileData();
                this.Manager.Checkpoint("MS-FRS2_R623");
                this.Manager.Checkpoint("MS-FRS2_R629");
                this.Manager.Checkpoint("MS-FRS2_R630");
                this.Manager.Checkpoint("MS-FRS2_R804");
                this.Manager.Comment("reaching state \'S252\'");
                this.Manager.Comment("checking step \'return RawGetFileData/ERROR_FAIL\'");
                this.Manager.Assert((temp17 == FRS2Model.error_status_t.ERROR_FAIL), String.Format("expected \'FRS2Model.error_status_t.ERROR_FAIL\', actual \'{0}\' (return of RawGetFil" +
                            "eData, state S252)", TestManagerHelpers.Describe(temp17)));
                this.Manager.Comment("reaching state \'S276\'");
                goto label1;
            }
            if ((temp24 == 2))
            {
                this.Manager.Comment("reaching state \'S229\'");
                FRS2Model.error_status_t temp18;
                this.Manager.Comment("executing step \'call RawGetFileData()\'");
                temp18 = this.IFRS2ManagedAdapterInstance.RawGetFileData();
                this.Manager.Checkpoint("MS-FRS2_R623");
                this.Manager.Checkpoint("MS-FRS2_R629");
                this.Manager.Checkpoint("MS-FRS2_R630");
                this.Manager.Checkpoint("MS-FRS2_R804");
                this.Manager.Comment("reaching state \'S253\'");
                this.Manager.Comment("checking step \'return RawGetFileData/ERROR_FAIL\'");
                this.Manager.Assert((temp18 == FRS2Model.error_status_t.ERROR_FAIL), String.Format("expected \'FRS2Model.error_status_t.ERROR_FAIL\', actual \'{0}\' (return of RawGetFil" +
                            "eData, state S253)", TestManagerHelpers.Describe(temp18)));
                this.Manager.Comment("reaching state \'S277\'");
                goto label1;
            }
            if ((temp24 == 3))
            {
                this.Manager.Comment("reaching state \'S230\'");
                FRS2Model.error_status_t temp19;
                this.Manager.Comment("executing step \'call RawGetFileData()\'");
                temp19 = this.IFRS2ManagedAdapterInstance.RawGetFileData();
                this.Manager.Checkpoint("MS-FRS2_R623");
                this.Manager.Checkpoint("MS-FRS2_R804");
                this.Manager.Comment("reaching state \'S254\'");
                this.Manager.Comment("checking step \'return RawGetFileData/ERROR_FAIL\'");
                this.Manager.Assert((temp19 == FRS2Model.error_status_t.ERROR_FAIL), String.Format("expected \'FRS2Model.error_status_t.ERROR_FAIL\', actual \'{0}\' (return of RawGetFil" +
                            "eData, state S254)", TestManagerHelpers.Describe(temp19)));
                this.Manager.Comment("reaching state \'S278\'");
                goto label1;
            }
            if ((temp24 == 4))
            {
                this.Manager.Comment("reaching state \'S231\'");
                FRS2Model.error_status_t temp20;
                this.Manager.Comment("executing step \'call RawGetFileData()\'");
                temp20 = this.IFRS2ManagedAdapterInstance.RawGetFileData();
                this.Manager.Checkpoint("MS-FRS2_R623");
                this.Manager.Checkpoint("MS-FRS2_R629");
                this.Manager.Checkpoint("MS-FRS2_R630");
                this.Manager.Checkpoint("MS-FRS2_R804");
                this.Manager.Comment("reaching state \'S255\'");
                this.Manager.Comment("checking step \'return RawGetFileData/ERROR_FAIL\'");
                this.Manager.Assert((temp20 == FRS2Model.error_status_t.ERROR_FAIL), String.Format("expected \'FRS2Model.error_status_t.ERROR_FAIL\', actual \'{0}\' (return of RawGetFil" +
                            "eData, state S255)", TestManagerHelpers.Describe(temp20)));
                this.Manager.Comment("reaching state \'S279\'");
                goto label1;
            }
            if ((temp24 == 5))
            {
                this.Manager.Comment("reaching state \'S232\'");
                FRS2Model.error_status_t temp21;
                this.Manager.Comment("executing step \'call RawGetFileData()\'");
                temp21 = this.IFRS2ManagedAdapterInstance.RawGetFileData();
                this.Manager.Checkpoint("MS-FRS2_R623");
                this.Manager.Checkpoint("MS-FRS2_R804");
                this.Manager.Comment("reaching state \'S256\'");
                this.Manager.Comment("checking step \'return RawGetFileData/ERROR_FAIL\'");
                this.Manager.Assert((temp21 == FRS2Model.error_status_t.ERROR_FAIL), String.Format("expected \'FRS2Model.error_status_t.ERROR_FAIL\', actual \'{0}\' (return of RawGetFil" +
                            "eData, state S256)", TestManagerHelpers.Describe(temp21)));
                this.Manager.Comment("reaching state \'S280\'");
                goto label1;
            }
            if ((temp24 == 6))
            {
                this.Manager.Comment("reaching state \'S233\'");
                FRS2Model.error_status_t temp22;
                this.Manager.Comment("executing step \'call RawGetFileData()\'");
                temp22 = this.IFRS2ManagedAdapterInstance.RawGetFileData();
                this.Manager.Checkpoint("MS-FRS2_R623");
                this.Manager.Checkpoint("MS-FRS2_R629");
                this.Manager.Checkpoint("MS-FRS2_R630");
                this.Manager.Checkpoint("MS-FRS2_R804");
                this.Manager.Comment("reaching state \'S257\'");
                this.Manager.Comment("checking step \'return RawGetFileData/ERROR_FAIL\'");
                this.Manager.Assert((temp22 == FRS2Model.error_status_t.ERROR_FAIL), String.Format("expected \'FRS2Model.error_status_t.ERROR_FAIL\', actual \'{0}\' (return of RawGetFil" +
                            "eData, state S257)", TestManagerHelpers.Describe(temp22)));
                this.Manager.Comment("reaching state \'S281\'");
                goto label1;
            }
            if ((temp24 == 7))
            {
                this.Manager.Comment("reaching state \'S234\'");
                FRS2Model.error_status_t temp23;
                this.Manager.Comment("executing step \'call RawGetFileData()\'");
                temp23 = this.IFRS2ManagedAdapterInstance.RawGetFileData();
                this.Manager.Checkpoint("MS-FRS2_R623");
                this.Manager.Checkpoint("MS-FRS2_R804");
                this.Manager.Comment("reaching state \'S258\'");
                this.Manager.Comment("checking step \'return RawGetFileData/ERROR_FAIL\'");
                this.Manager.Assert((temp23 == FRS2Model.error_status_t.ERROR_FAIL), String.Format("expected \'FRS2Model.error_status_t.ERROR_FAIL\', actual \'{0}\' (return of RawGetFil" +
                            "eData, state S258)", TestManagerHelpers.Describe(temp23)));
                this.Manager.Comment("reaching state \'S282\'");
                goto label1;
            }
            throw new InvalidOperationException("never reached");
        label1:
            ;
        }

        private void TCtestScenario1S2InitializeFileTransferAsyncChecker(FRS2Model.error_status_t @return)
        {
            this.Manager.Comment("checking step \'return InitializeFileTransferAsync/ERROR_SUCCESS\'");
            this.Manager.Assert((@return == FRS2Model.error_status_t.ERROR_SUCCESS), String.Format("expected \'FRS2Model.error_status_t.ERROR_SUCCESS\', actual \'{0}\' (return of Initia" +
                        "lizeFileTransferAsync, state S206)", TestManagerHelpers.Describe(@return)));
        }

        private void TCtestScenario1S2InitializeFileTransferAsyncEventChecker(FRS2Model.ServerContext context, FRS2Model.RDC_Sig_Level rdcsigLevel, bool isEOF)
        {
            this.Manager.Comment("checking step \'event InitializeFileTransferAsyncEvent(ValidContext,forRAWFileLeve" +
                    "l,False)\'");
            this.Manager.Assert((context == FRS2Model.ServerContext.ValidContext), String.Format("expected \'FRS2Model.ServerContext.ValidContext\', actual \'{0}\' (context of Initial" +
                        "izeFileTransferAsyncEvent, state S218)", TestManagerHelpers.Describe(context)));
            this.Manager.Assert((rdcsigLevel == FRS2Model.RDC_Sig_Level.forRAWFileLevel), String.Format("expected \'FRS2Model.RDC_Sig_Level.forRAWFileLevel\', actual \'{0}\' (rdcsigLevel of " +
                        "InitializeFileTransferAsyncEvent, state S218)", TestManagerHelpers.Describe(rdcsigLevel)));
            this.Manager.Assert((isEOF == false), String.Format("expected \'false\', actual \'{0}\' (isEOF of InitializeFileTransferAsyncEvent, state " +
                        "S218)", TestManagerHelpers.Describe(isEOF)));
        }

        private void TCtestScenario1S2RawGetFileDataResponseEventChecker(bool isEOF)
        {
            this.Manager.Comment("checking step \'event RawGetFileDataResponseEvent(True)\'");
            this.Manager.Assert((isEOF == true), String.Format("expected \'true\', actual \'{0}\' (isEOF of RawGetFileDataResponseEvent, state S275)", TestManagerHelpers.Describe(isEOF)));
        }

        private void TCtestScenario1S2RawGetFileDataResponseEventChecker1(bool isEOF)
        {
            this.Manager.Comment("checking step \'event RawGetFileDataResponseEvent(False)\'");
            this.Manager.Assert((isEOF == false), String.Format("expected \'false\', actual \'{0}\' (isEOF of RawGetFileDataResponseEvent, state S275)" +
                        "", TestManagerHelpers.Describe(isEOF)));
        }

        private void TCtestScenario1S2InitializeFileTransferAsyncEventChecker1(FRS2Model.ServerContext context, FRS2Model.RDC_Sig_Level rdcsigLevel, bool isEOF)
        {
            this.Manager.Comment("checking step \'event InitializeFileTransferAsyncEvent(InvalidContext,forRAWFileLe" +
                    "vel,False)\'");
            this.Manager.Assert((context == FRS2Model.ServerContext.InvalidContext), String.Format("expected \'FRS2Model.ServerContext.InvalidContext\', actual \'{0}\' (context of Initi" +
                        "alizeFileTransferAsyncEvent, state S218)", TestManagerHelpers.Describe(context)));
            this.Manager.Assert((rdcsigLevel == FRS2Model.RDC_Sig_Level.forRAWFileLevel), String.Format("expected \'FRS2Model.RDC_Sig_Level.forRAWFileLevel\', actual \'{0}\' (rdcsigLevel of " +
                        "InitializeFileTransferAsyncEvent, state S218)", TestManagerHelpers.Describe(rdcsigLevel)));
            this.Manager.Assert((isEOF == false), String.Format("expected \'false\', actual \'{0}\' (isEOF of InitializeFileTransferAsyncEvent, state " +
                        "S218)", TestManagerHelpers.Describe(isEOF)));
        }

        private void TCtestScenario1S2InitializeFileTransferAsyncEventChecker2(FRS2Model.ServerContext context, FRS2Model.RDC_Sig_Level rdcsigLevel, bool isEOF)
        {
            this.Manager.Comment("checking step \'event InitializeFileTransferAsyncEvent(InvalidContext,forRDCFileLe" +
                    "vel,True)\'");
            this.Manager.Assert((context == FRS2Model.ServerContext.InvalidContext), String.Format("expected \'FRS2Model.ServerContext.InvalidContext\', actual \'{0}\' (context of Initi" +
                        "alizeFileTransferAsyncEvent, state S218)", TestManagerHelpers.Describe(context)));
            this.Manager.Assert((rdcsigLevel == FRS2Model.RDC_Sig_Level.forRDCFileLevel), String.Format("expected \'FRS2Model.RDC_Sig_Level.forRDCFileLevel\', actual \'{0}\' (rdcsigLevel of " +
                        "InitializeFileTransferAsyncEvent, state S218)", TestManagerHelpers.Describe(rdcsigLevel)));
            this.Manager.Assert((isEOF == true), String.Format("expected \'true\', actual \'{0}\' (isEOF of InitializeFileTransferAsyncEvent, state S" +
                        "218)", TestManagerHelpers.Describe(isEOF)));
        }

        private void TCtestScenario1S2InitializeFileTransferAsyncEventChecker3(FRS2Model.ServerContext context, FRS2Model.RDC_Sig_Level rdcsigLevel, bool isEOF)
        {
            this.Manager.Comment("checking step \'event InitializeFileTransferAsyncEvent(ValidContext,forRDCFileLeve" +
                    "l,False)\'");
            this.Manager.Assert((context == FRS2Model.ServerContext.ValidContext), String.Format("expected \'FRS2Model.ServerContext.ValidContext\', actual \'{0}\' (context of Initial" +
                        "izeFileTransferAsyncEvent, state S218)", TestManagerHelpers.Describe(context)));
            this.Manager.Assert((rdcsigLevel == FRS2Model.RDC_Sig_Level.forRDCFileLevel), String.Format("expected \'FRS2Model.RDC_Sig_Level.forRDCFileLevel\', actual \'{0}\' (rdcsigLevel of " +
                        "InitializeFileTransferAsyncEvent, state S218)", TestManagerHelpers.Describe(rdcsigLevel)));
            this.Manager.Assert((isEOF == false), String.Format("expected \'false\', actual \'{0}\' (isEOF of InitializeFileTransferAsyncEvent, state " +
                        "S218)", TestManagerHelpers.Describe(isEOF)));
        }

        private void TCtestScenario1S2InitializeFileTransferAsyncEventChecker4(FRS2Model.ServerContext context, FRS2Model.RDC_Sig_Level rdcsigLevel, bool isEOF)
        {
            this.Manager.Comment("checking step \'event InitializeFileTransferAsyncEvent(InvalidContext,forRDCFileLe" +
                    "vel,False)\'");
            this.Manager.Assert((context == FRS2Model.ServerContext.InvalidContext), String.Format("expected \'FRS2Model.ServerContext.InvalidContext\', actual \'{0}\' (context of Initi" +
                        "alizeFileTransferAsyncEvent, state S218)", TestManagerHelpers.Describe(context)));
            this.Manager.Assert((rdcsigLevel == FRS2Model.RDC_Sig_Level.forRDCFileLevel), String.Format("expected \'FRS2Model.RDC_Sig_Level.forRDCFileLevel\', actual \'{0}\' (rdcsigLevel of " +
                        "InitializeFileTransferAsyncEvent, state S218)", TestManagerHelpers.Describe(rdcsigLevel)));
            this.Manager.Assert((isEOF == false), String.Format("expected \'false\', actual \'{0}\' (isEOF of InitializeFileTransferAsyncEvent, state " +
                        "S218)", TestManagerHelpers.Describe(isEOF)));
        }

        private void TCtestScenario1S2InitializeFileTransferAsyncEventChecker5(FRS2Model.ServerContext context, FRS2Model.RDC_Sig_Level rdcsigLevel, bool isEOF)
        {
            this.Manager.Comment("checking step \'event InitializeFileTransferAsyncEvent(ValidContext,forRDCFileLeve" +
                    "l,True)\'");
            this.Manager.Assert((context == FRS2Model.ServerContext.ValidContext), String.Format("expected \'FRS2Model.ServerContext.ValidContext\', actual \'{0}\' (context of Initial" +
                        "izeFileTransferAsyncEvent, state S218)", TestManagerHelpers.Describe(context)));
            this.Manager.Assert((rdcsigLevel == FRS2Model.RDC_Sig_Level.forRDCFileLevel), String.Format("expected \'FRS2Model.RDC_Sig_Level.forRDCFileLevel\', actual \'{0}\' (rdcsigLevel of " +
                        "InitializeFileTransferAsyncEvent, state S218)", TestManagerHelpers.Describe(rdcsigLevel)));
            this.Manager.Assert((isEOF == true), String.Format("expected \'true\', actual \'{0}\' (isEOF of InitializeFileTransferAsyncEvent, state S" +
                        "218)", TestManagerHelpers.Describe(isEOF)));
        }

        private void TCtestScenario1S2InitializeFileTransferAsyncEventChecker6(FRS2Model.ServerContext context, FRS2Model.RDC_Sig_Level rdcsigLevel, bool isEOF)
        {
            this.Manager.Comment("checking step \'event InitializeFileTransferAsyncEvent(InvalidContext,forRAWFileLe" +
                    "vel,True)\'");
            this.Manager.Assert((context == FRS2Model.ServerContext.InvalidContext), String.Format("expected \'FRS2Model.ServerContext.InvalidContext\', actual \'{0}\' (context of Initi" +
                        "alizeFileTransferAsyncEvent, state S218)", TestManagerHelpers.Describe(context)));
            this.Manager.Assert((rdcsigLevel == FRS2Model.RDC_Sig_Level.forRAWFileLevel), String.Format("expected \'FRS2Model.RDC_Sig_Level.forRAWFileLevel\', actual \'{0}\' (rdcsigLevel of " +
                        "InitializeFileTransferAsyncEvent, state S218)", TestManagerHelpers.Describe(rdcsigLevel)));
            this.Manager.Assert((isEOF == true), String.Format("expected \'true\', actual \'{0}\' (isEOF of InitializeFileTransferAsyncEvent, state S" +
                        "218)", TestManagerHelpers.Describe(isEOF)));
        }

        private void TCtestScenario1S2InitializeFileTransferAsyncEventChecker7(FRS2Model.ServerContext context, FRS2Model.RDC_Sig_Level rdcsigLevel, bool isEOF)
        {
            this.Manager.Comment("checking step \'event InitializeFileTransferAsyncEvent(ValidContext,forRAWFileLeve" +
                    "l,True)\'");
            this.Manager.Assert((context == FRS2Model.ServerContext.ValidContext), String.Format("expected \'FRS2Model.ServerContext.ValidContext\', actual \'{0}\' (context of Initial" +
                        "izeFileTransferAsyncEvent, state S218)", TestManagerHelpers.Describe(context)));
            this.Manager.Assert((rdcsigLevel == FRS2Model.RDC_Sig_Level.forRAWFileLevel), String.Format("expected \'FRS2Model.RDC_Sig_Level.forRAWFileLevel\', actual \'{0}\' (rdcsigLevel of " +
                        "InitializeFileTransferAsyncEvent, state S218)", TestManagerHelpers.Describe(rdcsigLevel)));
            this.Manager.Assert((isEOF == true), String.Format("expected \'true\', actual \'{0}\' (isEOF of InitializeFileTransferAsyncEvent, state S" +
                        "218)", TestManagerHelpers.Describe(isEOF)));
        }

        private void TCtestScenario1S2RequestUpdatesEventChecker1(FRS2Model.FilePresense present, FRS2Model.UPDATE_STATUS updateStatus)
        {
            this.Manager.Comment("checking step \'event RequestUpdatesEvent(fileDeleted,UPDATE_STATUS_DONE)\'");
            try
            {
                this.Manager.Assert((present == FRS2Model.FilePresense.fileDeleted), String.Format("expected \'FRS2Model.FilePresense.fileDeleted\', actual \'{0}\' (present of RequestUp" +
                            "datesEvent, state S190)", TestManagerHelpers.Describe(present)));
                this.Manager.Assert((updateStatus == FRS2Model.UPDATE_STATUS.UPDATE_STATUS_DONE), String.Format("expected \'FRS2Model.UPDATE_STATUS.UPDATE_STATUS_DONE\', actual \'{0}\' (updateStatus" +
                            " of RequestUpdatesEvent, state S190)", TestManagerHelpers.Describe(updateStatus)));
            }
            catch (TransactionFailedException)
            {
                this.Manager.Comment("This step would have covered MS-FRS2_R93, MS-FRS2_R94");
                throw;
            }
            this.Manager.Checkpoint("MS-FRS2_R93");
            this.Manager.Checkpoint("MS-FRS2_R94");
        }

        private void TCtestScenario1S207()
        {
            this.Manager.Comment("reaching state \'S207\'");
            this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(TCtestScenario1.InitializeFileTransferAsyncInfo, null, new InitializeFileTransferAsyncDelegate1(this.TCtestScenario1S2InitializeFileTransferAsyncChecker1)));
            TCtestScenario1S219();
        }

        private void TCtestScenario1S2InitializeFileTransferAsyncChecker1(FRS2Model.error_status_t @return)
        {
            this.Manager.Comment("checking step \'return InitializeFileTransferAsync/FRS_ERROR_CONNECTION_INVALID\'");
            this.Manager.Assert((@return == FRS2Model.error_status_t.FRS_ERROR_CONNECTION_INVALID), String.Format("expected \'FRS2Model.error_status_t.FRS_ERROR_CONNECTION_INVALID\', actual \'{0}\' (r" +
                        "eturn of InitializeFileTransferAsync, state S207)", TestManagerHelpers.Describe(@return)));
        }

        private void TCtestScenario1S219()
        {
            this.Manager.Comment("reaching state \'S219\'");
        }

        private void TCtestScenario1S2RequestUpdatesEventChecker2(FRS2Model.FilePresense present, FRS2Model.UPDATE_STATUS updateStatus)
        {
            this.Manager.Comment("checking step \'event RequestUpdatesEvent(fileExists,UPDATE_STATUS_DONE)\'");
            try
            {
                this.Manager.Assert((present == FRS2Model.FilePresense.fileExists), String.Format("expected \'FRS2Model.FilePresense.fileExists\', actual \'{0}\' (present of RequestUpd" +
                            "atesEvent, state S190)", TestManagerHelpers.Describe(present)));
                this.Manager.Assert((updateStatus == FRS2Model.UPDATE_STATUS.UPDATE_STATUS_DONE), String.Format("expected \'FRS2Model.UPDATE_STATUS.UPDATE_STATUS_DONE\', actual \'{0}\' (updateStatus" +
                            " of RequestUpdatesEvent, state S190)", TestManagerHelpers.Describe(updateStatus)));
            }
            catch (TransactionFailedException)
            {
                this.Manager.Comment("This step would have covered MS-FRS2_R93, MS-FRS2_R94");
                throw;
            }
            this.Manager.Checkpoint("MS-FRS2_R93");
            this.Manager.Checkpoint("MS-FRS2_R94");
        }

        private void TCtestScenario1S220()
        {
            this.Manager.Comment("reaching state \'S220\'");
        }

        private void TCtestScenario1S2RequestUpdatesEventChecker3(FRS2Model.FilePresense present, FRS2Model.UPDATE_STATUS updateStatus)
        {
            this.Manager.Comment("checking step \'event RequestUpdatesEvent(fileExists,UPDATE_STATUS_MORE)\'");
            try
            {
                this.Manager.Assert((present == FRS2Model.FilePresense.fileExists), String.Format("expected \'FRS2Model.FilePresense.fileExists\', actual \'{0}\' (present of RequestUpd" +
                            "atesEvent, state S190)", TestManagerHelpers.Describe(present)));
                this.Manager.Assert((updateStatus == FRS2Model.UPDATE_STATUS.UPDATE_STATUS_MORE), String.Format("expected \'FRS2Model.UPDATE_STATUS.UPDATE_STATUS_MORE\', actual \'{0}\' (updateStatus" +
                            " of RequestUpdatesEvent, state S190)", TestManagerHelpers.Describe(updateStatus)));
            }
            catch (TransactionFailedException)
            {
                this.Manager.Comment("This step would have covered MS-FRS2_R93, MS-FRS2_R498");
                throw;
            }
            this.Manager.Checkpoint("MS-FRS2_R93");
            this.Manager.Checkpoint("MS-FRS2_R498");
        }

        private void TCtestScenario1S221()
        {
            this.Manager.Comment("reaching state \'S221\'");
        }

        private void TCtestScenario1S2AsyncPollResponseEventChecker1(FRS2Model.VVGeneration vvGen)
        {
            this.Manager.Comment("checking step \'event AsyncPollResponseEvent(InvalidValue)\'");
            try
            {
                this.Manager.Assert((vvGen == FRS2Model.VVGeneration.InvalidValue), String.Format("expected \'FRS2Model.VVGeneration.InvalidValue\', actual \'{0}\' (vvGen of AsyncPollR" +
                            "esponseEvent, state S170)", TestManagerHelpers.Describe(vvGen)));
            }
            catch (TransactionFailedException)
            {
                this.Manager.Comment("This step would have covered MS-FRS2_R556, MS-FRS2_R1020, MS-FRS2_R555");
                throw;
            }
            this.Manager.Checkpoint("MS-FRS2_R556");
            this.Manager.Checkpoint("MS-FRS2_R1020");
            this.Manager.Checkpoint("MS-FRS2_R555");
        }

        private void TCtestScenario1S179()
        {
            this.Manager.Comment("reaching state \'S179\'");
        }
        #endregion

        #region Test Starting in S4
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        [Microsoft.VisualStudio.TestTools.UnitTesting.DescriptionAttribute(@"MS-FRS2_R37, MS-FRS2_R436, MS-FRS2_R439, MS-FRS2_R455, MS-FRS2_R458, MS-FRS2_R448, MS-FRS2_R549, MS-FRS2_R552, MS-FRS2_R517, MS-FRS2_R520, MS-FRS2_R466, MS-FRS2_R556, MS-FRS2_R1020, MS-FRS2_R555, MS-FRS2_R486, MS-FRS2_R489, MS-FRS2_R93, MS-FRS2_R778, MS-FRS2_R783, MS-FRS2_R93, MS-FRS2_R94, MS-FRS2_R774, MS-FRS2_R777, MS-FRS2_R793, MS-FRS2_R93, MS-FRS2_R94, MS-FRS2_R774, MS-FRS2_R777, MS-FRS2_R793, MS-FRS2_R622, MS-FRS2_R625, MS-FRS2_R737, MS-FRS2_R739, MS-FRS2_R623, MS-FRS2_R629, MS-FRS2_R630, MS-FRS2_R804, MS-FRS2_R623, MS-FRS2_R629, MS-FRS2_R630, MS-FRS2_R804, MS-FRS2_R623, MS-FRS2_R804, MS-FRS2_R623, MS-FRS2_R629, MS-FRS2_R630, MS-FRS2_R804, MS-FRS2_R623, MS-FRS2_R804, MS-FRS2_R623, MS-FRS2_R629, MS-FRS2_R630, MS-FRS2_R804, MS-FRS2_R623, MS-FRS2_R804, MS-FRS2_R93, MS-FRS2_R498, MS-FRS2_R774, MS-FRS2_R777, MS-FRS2_R793, MS-FRS2_R622, MS-FRS2_R625, MS-FRS2_R737, MS-FRS2_R739, MS-FRS2_R623, MS-FRS2_R629, MS-FRS2_R630, MS-FRS2_R804, MS-FRS2_R623, MS-FRS2_R629, MS-FRS2_R630, MS-FRS2_R804, MS-FRS2_R623, MS-FRS2_R804, MS-FRS2_R623, MS-FRS2_R629, MS-FRS2_R630, MS-FRS2_R804, MS-FRS2_R623, MS-FRS2_R804, MS-FRS2_R623, MS-FRS2_R629, MS-FRS2_R630, MS-FRS2_R804, MS-FRS2_R623, MS-FRS2_R804, MS-FRS2_R556, MS-FRS2_R1020, MS-FRS2_R555")]
        public virtual void FRS2_TCtestScenario1S4()
        {
            this.Manager.BeginTest("TCtestScenario1S4");
            this.Manager.Comment("reaching state \'S4\'");
            FRS2Model.error_status_t temp30;
            this.Manager.Comment(@"executing step 'call Initialization(Windows7,Map{1=>enabled,2=>enabled,3=>enabled,4=>disabled},Map{1=>exists,2=>exists,3=>exists,4=>exists},Map{""A""=>1,""B""=>2,""C""=>3,""D""=>4},Map{""A""=>sysvol,""B""=>protection,""C""=>sysvol,""D""=>sysvol},Map{1=>""A"",2=>""B"",3=>""C"",4=>""D""},Map{1=>writeOnly,2=>readWrite,3=>readOnly,4=>readWrite},Map{1=>enabled,2=>disabled,3=>enabled,4=>enabled})'");
            temp30 = this.IFRS2ManagedAdapterInstance.Initialization(FRS2Model.OSVersion.Windows7, new Microsoft.Modeling.Map<System.Int32, FRS2Model.connectionProperty>().Add(1, FRS2Model.connectionProperty.enabled).Add(2, FRS2Model.connectionProperty.enabled).Add(3, FRS2Model.connectionProperty.enabled).Add(4, FRS2Model.connectionProperty.disabled), new Microsoft.Modeling.Map<System.Int32, FRS2Model.FromServer>().Add(1, FRS2Model.FromServer.exists).Add(2, FRS2Model.FromServer.exists).Add(3, FRS2Model.FromServer.exists).Add(4, FRS2Model.FromServer.exists), new Microsoft.Modeling.Map<System.String, System.Int32>().Add("A", 1).Add("B", 2).Add("C", 3).Add("D", 4), new Microsoft.Modeling.Map<System.String, FRS2Model.ReplicationGroupTypes>().Add("A", FRS2Model.ReplicationGroupTypes.sysvol).Add("B", FRS2Model.ReplicationGroupTypes.protection).Add("C", FRS2Model.ReplicationGroupTypes.sysvol).Add("D", FRS2Model.ReplicationGroupTypes.sysvol), new Microsoft.Modeling.Map<System.Int32, System.String>().Add(1, "A").Add(2, "B").Add(3, "C").Add(4, "D"), new Microsoft.Modeling.Map<System.Int32, FRS2Model.accessLevels>().Add(1, FRS2Model.accessLevels.writeOnly).Add(2, FRS2Model.accessLevels.readWrite).Add(3, FRS2Model.accessLevels.readOnly).Add(4, FRS2Model.accessLevels.readWrite), new Microsoft.Modeling.Map<System.Int32, FRS2Model.connectionProperty>().Add(1, FRS2Model.connectionProperty.enabled).Add(2, FRS2Model.connectionProperty.disabled).Add(3, FRS2Model.connectionProperty.enabled).Add(4, FRS2Model.connectionProperty.enabled));
            this.Manager.Comment("reaching state \'S5\'");
            this.Manager.Comment("checking step \'return Initialization/ERROR_SUCCESS\'");
            this.Manager.Assert((temp30 == FRS2Model.error_status_t.ERROR_SUCCESS), String.Format("expected \'FRS2Model.error_status_t.ERROR_SUCCESS\', actual \'{0}\' (return of Initia" +
                        "lization, state S5)", TestManagerHelpers.Describe(temp30)));
            this.Manager.Comment("reaching state \'S38\'");
            FRS2Model.ProtocolVersionReturned temp31;
            FRS2Model.UpstreamFlagValueReturned temp32;
            FRS2Model.error_status_t temp33;
            this.Manager.Comment("executing step \'call EstablishConnection(\"A\",1,FRS_COMMUNICATION_PROTOCOL_VERSION" +
                    "_LONGHORN_SERVER,out _,out _)\'");
            temp33 = this.IFRS2ManagedAdapterInstance.EstablishConnection("A", 1, FRS2Model.ProtocolVersion.FRS_COMMUNICATION_PROTOCOL_VERSION_LONGHORN_SERVER, out temp31, out temp32);
            this.Manager.Checkpoint("MS-FRS2_R37");
            this.Manager.Checkpoint("MS-FRS2_R436");
            this.Manager.Checkpoint("MS-FRS2_R439");
            this.Manager.Comment("reaching state \'S56\'");
            this.Manager.Comment("checking step \'return EstablishConnection/[out Valid,out Valid]:ERROR_SUCCESS\'");
            this.Manager.Assert((temp31 == FRS2Model.ProtocolVersionReturned.Valid), String.Format("expected \'FRS2Model.ProtocolVersionReturned.Valid\', actual \'{0}\' (upstreamProtoco" +
                        "lVersion of EstablishConnection, state S56)", TestManagerHelpers.Describe(temp31)));
            this.Manager.Assert((temp32 == FRS2Model.UpstreamFlagValueReturned.Valid), String.Format("expected \'FRS2Model.UpstreamFlagValueReturned.Valid\', actual \'{0}\' (upstreamFlags" +
                        " of EstablishConnection, state S56)", TestManagerHelpers.Describe(temp32)));
            this.Manager.Assert((temp33 == FRS2Model.error_status_t.ERROR_SUCCESS), String.Format("expected \'FRS2Model.error_status_t.ERROR_SUCCESS\', actual \'{0}\' (return of Establ" +
                        "ishConnection, state S56)", TestManagerHelpers.Describe(temp33)));
            this.Manager.Comment("reaching state \'S74\'");
            FRS2Model.error_status_t temp34;
            this.Manager.Comment("executing step \'call EstablishSession(1,1)\'");
            temp34 = this.IFRS2ManagedAdapterInstance.EstablishSession(1, 1);
            this.Manager.Checkpoint("MS-FRS2_R455");
            this.Manager.Checkpoint("MS-FRS2_R458");
            this.Manager.Checkpoint("MS-FRS2_R448");
            this.Manager.Comment("reaching state \'S92\'");
            this.Manager.Comment("checking step \'return EstablishSession/ERROR_SUCCESS\'");
            this.Manager.Assert((temp34 == FRS2Model.error_status_t.ERROR_SUCCESS), String.Format("expected \'FRS2Model.error_status_t.ERROR_SUCCESS\', actual \'{0}\' (return of Establ" +
                        "ishSession, state S92)", TestManagerHelpers.Describe(temp34)));
            this.Manager.Comment("reaching state \'S110\'");
            FRS2Model.error_status_t temp35;
            this.Manager.Comment("executing step \'call AsyncPoll(1)\'");
            temp35 = this.IFRS2ManagedAdapterInstance.AsyncPoll(1);
            this.Manager.Checkpoint("MS-FRS2_R549");
            this.Manager.Checkpoint("MS-FRS2_R552");
            this.Manager.Comment("reaching state \'S127\'");
            this.Manager.Comment("checking step \'return AsyncPoll/ERROR_SUCCESS\'");
            this.Manager.Assert((temp35 == FRS2Model.error_status_t.ERROR_SUCCESS), String.Format("expected \'FRS2Model.error_status_t.ERROR_SUCCESS\', actual \'{0}\' (return of AsyncP" +
                        "oll, state S127)", TestManagerHelpers.Describe(temp35)));
            this.Manager.Comment("reaching state \'S143\'");
            FRS2Model.error_status_t temp36;
            this.Manager.Comment("executing step \'call RequestVersionVector(1,1,1,REQUEST_NORMAL_SYNC,CHANGE_NOTIFY" +
                    ",ValidValue)\'");
            temp36 = this.IFRS2ManagedAdapterInstance.RequestVersionVector(1, 1, 1, FRS2Model.VERSION_REQUEST_TYPE.REQUEST_NORMAL_SYNC, FRS2Model.VERSION_CHANGE_TYPE.CHANGE_NOTIFY, FRS2Model.VVGeneration.ValidValue);
            this.Manager.Checkpoint("MS-FRS2_R517");
            this.Manager.Checkpoint("MS-FRS2_R520");
            this.Manager.Checkpoint("MS-FRS2_R466");
            this.Manager.Comment("reaching state \'S159\'");
            this.Manager.Comment("checking step \'return RequestVersionVector/ERROR_SUCCESS\'");
            this.Manager.Assert((temp36 == FRS2Model.error_status_t.ERROR_SUCCESS), String.Format("expected \'FRS2Model.error_status_t.ERROR_SUCCESS\', actual \'{0}\' (return of Reques" +
                        "tVersionVector, state S159)", TestManagerHelpers.Describe(temp36)));
            this.Manager.Comment("reaching state \'S171\'");
            int temp65 = this.Manager.ExpectEvent(this.QuiescenceTimeout, true, new ExpectedEvent(TCtestScenario1.AsyncPollResponseEventInfo, null, new AsyncPollResponseEventDelegate1(this.TCtestScenario1S4AsyncPollResponseEventChecker)), new ExpectedEvent(TCtestScenario1.AsyncPollResponseEventInfo, null, new AsyncPollResponseEventDelegate1(this.TCtestScenario1S4AsyncPollResponseEventChecker1)));
            if ((temp65 == 0))
            {
                this.Manager.Comment("reaching state \'S180\'");
                FRS2Model.error_status_t temp37;
                this.Manager.Comment("executing step \'call RequestUpdates(1,1,valid)\'");
                temp37 = this.IFRS2ManagedAdapterInstance.RequestUpdates(1, 1, FRS2Model.versionVectorDiff.valid);
                this.Manager.Checkpoint("MS-FRS2_R486");
                this.Manager.Checkpoint("MS-FRS2_R489");
                this.Manager.Comment("reaching state \'S187\'");
                this.Manager.Comment("checking step \'return RequestUpdates/ERROR_SUCCESS\'");
                this.Manager.Assert((temp37 == FRS2Model.error_status_t.ERROR_SUCCESS), String.Format("expected \'FRS2Model.error_status_t.ERROR_SUCCESS\', actual \'{0}\' (return of Reques" +
                            "tUpdates, state S187)", TestManagerHelpers.Describe(temp37)));
                this.Manager.Comment("reaching state \'S191\'");
                int temp64 = this.Manager.ExpectEvent(this.QuiescenceTimeout, true, new ExpectedEvent(TCtestScenario1.RequestUpdatesEventInfo, null, new RequestUpdatesEventDelegate1(this.TCtestScenario1S4RequestUpdatesEventChecker)), new ExpectedEvent(TCtestScenario1.RequestUpdatesEventInfo, null, new RequestUpdatesEventDelegate1(this.TCtestScenario1S4RequestUpdatesEventChecker1)), new ExpectedEvent(TCtestScenario1.RequestUpdatesEventInfo, null, new RequestUpdatesEventDelegate1(this.TCtestScenario1S4RequestUpdatesEventChecker2)), new ExpectedEvent(TCtestScenario1.RequestUpdatesEventInfo, null, new RequestUpdatesEventDelegate1(this.TCtestScenario1S4RequestUpdatesEventChecker3)));
                if ((temp64 == 0))
                {
                    this.Manager.Comment("reaching state \'S198\'");
                    FRS2Model.error_status_t temp38;
                    this.Manager.Comment("executing step \'call InitializeFileTransferAsync(4,1,False)\'");
                    temp38 = this.IFRS2ManagedAdapterInstance.InitializeFileTransferAsync(4, 1, false);
                    this.Manager.Checkpoint("MS-FRS2_R778");
                    this.Manager.Checkpoint("MS-FRS2_R783");
                    this.Manager.AddReturn(InitializeFileTransferAsyncInfo, null, temp38);
                    TCtestScenario1S207();
                    goto label8;
                }
                if ((temp64 == 1))
                {
                    this.Manager.Comment("reaching state \'S199\'");
                    FRS2Model.error_status_t temp39;
                    this.Manager.Comment("executing step \'call InitializeFileTransferAsync(1,1,False)\'");
                    temp39 = this.IFRS2ManagedAdapterInstance.InitializeFileTransferAsync(1, 1, false);
                    this.Manager.Checkpoint("MS-FRS2_R774");
                    this.Manager.Checkpoint("MS-FRS2_R777");
                    this.Manager.Checkpoint("MS-FRS2_R793");
                    this.Manager.AddReturn(InitializeFileTransferAsyncInfo, null, temp39);
                    TCtestScenario1S206();
                    goto label8;
                }
                if ((temp64 == 2))
                {
                    this.Manager.Comment("reaching state \'S200\'");
                    FRS2Model.error_status_t temp40;
                    this.Manager.Comment("executing step \'call InitializeFileTransferAsync(1,1,False)\'");
                    temp40 = this.IFRS2ManagedAdapterInstance.InitializeFileTransferAsync(1, 1, false);
                    this.Manager.Checkpoint("MS-FRS2_R774");
                    this.Manager.Checkpoint("MS-FRS2_R777");
                    this.Manager.Checkpoint("MS-FRS2_R793");
                    this.Manager.Comment("reaching state \'S212\'");
                    this.Manager.Comment("checking step \'return InitializeFileTransferAsync/ERROR_SUCCESS\'");
                    this.Manager.Assert((temp40 == FRS2Model.error_status_t.ERROR_SUCCESS), String.Format("expected \'FRS2Model.error_status_t.ERROR_SUCCESS\', actual \'{0}\' (return of Initia" +
                                "lizeFileTransferAsync, state S212)", TestManagerHelpers.Describe(temp40)));
                    this.Manager.Comment("reaching state \'S222\'");
                    int temp51 = this.Manager.ExpectEvent(this.QuiescenceTimeout, true, new ExpectedEvent(TCtestScenario1.InitializeFileTransferAsyncEventInfo, null, new InitializeFileTransferAsyncEventDelegate1(this.TCtestScenario1S4InitializeFileTransferAsyncEventChecker)), new ExpectedEvent(TCtestScenario1.InitializeFileTransferAsyncEventInfo, null, new InitializeFileTransferAsyncEventDelegate1(this.TCtestScenario1S4InitializeFileTransferAsyncEventChecker1)), new ExpectedEvent(TCtestScenario1.InitializeFileTransferAsyncEventInfo, null, new InitializeFileTransferAsyncEventDelegate1(this.TCtestScenario1S4InitializeFileTransferAsyncEventChecker2)), new ExpectedEvent(TCtestScenario1.InitializeFileTransferAsyncEventInfo, null, new InitializeFileTransferAsyncEventDelegate1(this.TCtestScenario1S4InitializeFileTransferAsyncEventChecker3)), new ExpectedEvent(TCtestScenario1.InitializeFileTransferAsyncEventInfo, null, new InitializeFileTransferAsyncEventDelegate1(this.TCtestScenario1S4InitializeFileTransferAsyncEventChecker4)), new ExpectedEvent(TCtestScenario1.InitializeFileTransferAsyncEventInfo, null, new InitializeFileTransferAsyncEventDelegate1(this.TCtestScenario1S4InitializeFileTransferAsyncEventChecker5)), new ExpectedEvent(TCtestScenario1.InitializeFileTransferAsyncEventInfo, null, new InitializeFileTransferAsyncEventDelegate1(this.TCtestScenario1S4InitializeFileTransferAsyncEventChecker6)), new ExpectedEvent(TCtestScenario1.InitializeFileTransferAsyncEventInfo, null, new InitializeFileTransferAsyncEventDelegate1(this.TCtestScenario1S4InitializeFileTransferAsyncEventChecker7)));
                    if ((temp51 == 0))
                    {
                        this.Manager.Comment("reaching state \'S235\'");
                        FRS2Model.error_status_t temp41;
                        this.Manager.Comment("executing step \'call RawGetFileData()\'");
                        temp41 = this.IFRS2ManagedAdapterInstance.RawGetFileData();
                        this.Manager.Checkpoint("MS-FRS2_R622");
                        this.Manager.Checkpoint("MS-FRS2_R625");
                        this.Manager.Comment("reaching state \'S259\'");
                        this.Manager.Comment("checking step \'return RawGetFileData/ERROR_SUCCESS\'");
                        this.Manager.Assert((temp41 == FRS2Model.error_status_t.ERROR_SUCCESS), String.Format("expected \'FRS2Model.error_status_t.ERROR_SUCCESS\', actual \'{0}\' (return of RawGet" +
                                    "FileData, state S259)", TestManagerHelpers.Describe(temp41)));
                        this.Manager.Comment("reaching state \'S283\'");
                        int temp43 = this.Manager.ExpectEvent(this.QuiescenceTimeout, true, new ExpectedEvent(TCtestScenario1.RawGetFileDataResponseEventInfo, null, new RawGetFileDataResponseEventDelegate1(this.TCtestScenario1S4RawGetFileDataResponseEventChecker)), new ExpectedEvent(TCtestScenario1.RawGetFileDataResponseEventInfo, null, new RawGetFileDataResponseEventDelegate1(this.TCtestScenario1S4RawGetFileDataResponseEventChecker1)));
                        if ((temp43 == 0))
                        {
                            this.Manager.Comment("reaching state \'S301\'");
                            FRS2Model.error_status_t temp42;
                            this.Manager.Comment("executing step \'call RdcClose()\'");
                            temp42 = this.IFRS2ManagedAdapterInstance.RdcClose();
                            this.Manager.Checkpoint("MS-FRS2_R737");
                            this.Manager.Checkpoint("MS-FRS2_R739");
                            this.Manager.Comment("reaching state \'S306\'");
                            this.Manager.Comment("checking step \'return RdcClose/ERROR_SUCCESS\'");
                            this.Manager.Assert((temp42 == FRS2Model.error_status_t.ERROR_SUCCESS), String.Format("expected \'FRS2Model.error_status_t.ERROR_SUCCESS\', actual \'{0}\' (return of RdcClo" +
                                        "se, state S306)", TestManagerHelpers.Describe(temp42)));
                            this.Manager.Comment("reaching state \'S309\'");
                            goto label4;
                        }
                        if ((temp43 == 1))
                        {
                            this.Manager.Comment("reaching state \'S302\'");
                            goto label4;
                        }
                        throw new InvalidOperationException("never reached");
                    label4:
                        ;
                        goto label5;
                    }
                    if ((temp51 == 1))
                    {
                        this.Manager.Comment("reaching state \'S236\'");
                        FRS2Model.error_status_t temp44;
                        this.Manager.Comment("executing step \'call RawGetFileData()\'");
                        temp44 = this.IFRS2ManagedAdapterInstance.RawGetFileData();
                        this.Manager.Checkpoint("MS-FRS2_R623");
                        this.Manager.Checkpoint("MS-FRS2_R629");
                        this.Manager.Checkpoint("MS-FRS2_R630");
                        this.Manager.Checkpoint("MS-FRS2_R804");
                        this.Manager.Comment("reaching state \'S260\'");
                        this.Manager.Comment("checking step \'return RawGetFileData/ERROR_FAIL\'");
                        this.Manager.Assert((temp44 == FRS2Model.error_status_t.ERROR_FAIL), String.Format("expected \'FRS2Model.error_status_t.ERROR_FAIL\', actual \'{0}\' (return of RawGetFil" +
                                    "eData, state S260)", TestManagerHelpers.Describe(temp44)));
                        this.Manager.Comment("reaching state \'S284\'");
                        goto label5;
                    }
                    if ((temp51 == 2))
                    {
                        this.Manager.Comment("reaching state \'S237\'");
                        FRS2Model.error_status_t temp45;
                        this.Manager.Comment("executing step \'call RawGetFileData()\'");
                        temp45 = this.IFRS2ManagedAdapterInstance.RawGetFileData();
                        this.Manager.Checkpoint("MS-FRS2_R623");
                        this.Manager.Checkpoint("MS-FRS2_R629");
                        this.Manager.Checkpoint("MS-FRS2_R630");
                        this.Manager.Checkpoint("MS-FRS2_R804");
                        this.Manager.Comment("reaching state \'S261\'");
                        this.Manager.Comment("checking step \'return RawGetFileData/ERROR_FAIL\'");
                        this.Manager.Assert((temp45 == FRS2Model.error_status_t.ERROR_FAIL), String.Format("expected \'FRS2Model.error_status_t.ERROR_FAIL\', actual \'{0}\' (return of RawGetFil" +
                                    "eData, state S261)", TestManagerHelpers.Describe(temp45)));
                        this.Manager.Comment("reaching state \'S285\'");
                        goto label5;
                    }
                    if ((temp51 == 3))
                    {
                        this.Manager.Comment("reaching state \'S238\'");
                        FRS2Model.error_status_t temp46;
                        this.Manager.Comment("executing step \'call RawGetFileData()\'");
                        temp46 = this.IFRS2ManagedAdapterInstance.RawGetFileData();
                        this.Manager.Checkpoint("MS-FRS2_R623");
                        this.Manager.Checkpoint("MS-FRS2_R804");
                        this.Manager.Comment("reaching state \'S262\'");
                        this.Manager.Comment("checking step \'return RawGetFileData/ERROR_FAIL\'");
                        this.Manager.Assert((temp46 == FRS2Model.error_status_t.ERROR_FAIL), String.Format("expected \'FRS2Model.error_status_t.ERROR_FAIL\', actual \'{0}\' (return of RawGetFil" +
                                    "eData, state S262)", TestManagerHelpers.Describe(temp46)));
                        this.Manager.Comment("reaching state \'S286\'");
                        goto label5;
                    }
                    if ((temp51 == 4))
                    {
                        this.Manager.Comment("reaching state \'S239\'");
                        FRS2Model.error_status_t temp47;
                        this.Manager.Comment("executing step \'call RawGetFileData()\'");
                        temp47 = this.IFRS2ManagedAdapterInstance.RawGetFileData();
                        this.Manager.Checkpoint("MS-FRS2_R623");
                        this.Manager.Checkpoint("MS-FRS2_R629");
                        this.Manager.Checkpoint("MS-FRS2_R630");
                        this.Manager.Checkpoint("MS-FRS2_R804");
                        this.Manager.Comment("reaching state \'S263\'");
                        this.Manager.Comment("checking step \'return RawGetFileData/ERROR_FAIL\'");
                        this.Manager.Assert((temp47 == FRS2Model.error_status_t.ERROR_FAIL), String.Format("expected \'FRS2Model.error_status_t.ERROR_FAIL\', actual \'{0}\' (return of RawGetFil" +
                                    "eData, state S263)", TestManagerHelpers.Describe(temp47)));
                        this.Manager.Comment("reaching state \'S287\'");
                        goto label5;
                    }
                    if ((temp51 == 5))
                    {
                        this.Manager.Comment("reaching state \'S240\'");
                        FRS2Model.error_status_t temp48;
                        this.Manager.Comment("executing step \'call RawGetFileData()\'");
                        temp48 = this.IFRS2ManagedAdapterInstance.RawGetFileData();
                        this.Manager.Checkpoint("MS-FRS2_R623");
                        this.Manager.Checkpoint("MS-FRS2_R804");
                        this.Manager.Comment("reaching state \'S264\'");
                        this.Manager.Comment("checking step \'return RawGetFileData/ERROR_FAIL\'");
                        this.Manager.Assert((temp48 == FRS2Model.error_status_t.ERROR_FAIL), String.Format("expected \'FRS2Model.error_status_t.ERROR_FAIL\', actual \'{0}\' (return of RawGetFil" +
                                    "eData, state S264)", TestManagerHelpers.Describe(temp48)));
                        this.Manager.Comment("reaching state \'S288\'");
                        goto label5;
                    }
                    if ((temp51 == 6))
                    {
                        this.Manager.Comment("reaching state \'S241\'");
                        FRS2Model.error_status_t temp49;
                        this.Manager.Comment("executing step \'call RawGetFileData()\'");
                        temp49 = this.IFRS2ManagedAdapterInstance.RawGetFileData();
                        this.Manager.Checkpoint("MS-FRS2_R623");
                        this.Manager.Checkpoint("MS-FRS2_R629");
                        this.Manager.Checkpoint("MS-FRS2_R630");
                        this.Manager.Checkpoint("MS-FRS2_R804");
                        this.Manager.Comment("reaching state \'S265\'");
                        this.Manager.Comment("checking step \'return RawGetFileData/ERROR_FAIL\'");
                        this.Manager.Assert((temp49 == FRS2Model.error_status_t.ERROR_FAIL), String.Format("expected \'FRS2Model.error_status_t.ERROR_FAIL\', actual \'{0}\' (return of RawGetFil" +
                                    "eData, state S265)", TestManagerHelpers.Describe(temp49)));
                        this.Manager.Comment("reaching state \'S289\'");
                        goto label5;
                    }
                    if ((temp51 == 7))
                    {
                        this.Manager.Comment("reaching state \'S242\'");
                        FRS2Model.error_status_t temp50;
                        this.Manager.Comment("executing step \'call RawGetFileData()\'");
                        temp50 = this.IFRS2ManagedAdapterInstance.RawGetFileData();
                        this.Manager.Checkpoint("MS-FRS2_R623");
                        this.Manager.Checkpoint("MS-FRS2_R804");
                        this.Manager.Comment("reaching state \'S266\'");
                        this.Manager.Comment("checking step \'return RawGetFileData/ERROR_FAIL\'");
                        this.Manager.Assert((temp50 == FRS2Model.error_status_t.ERROR_FAIL), String.Format("expected \'FRS2Model.error_status_t.ERROR_FAIL\', actual \'{0}\' (return of RawGetFil" +
                                    "eData, state S266)", TestManagerHelpers.Describe(temp50)));
                        this.Manager.Comment("reaching state \'S290\'");
                        goto label5;
                    }
                    throw new InvalidOperationException("never reached");
                label5:
                    ;
                    goto label8;
                }
                if ((temp64 == 3))
                {
                    this.Manager.Comment("reaching state \'S201\'");
                    FRS2Model.error_status_t temp52;
                    this.Manager.Comment("executing step \'call InitializeFileTransferAsync(1,1,False)\'");
                    temp52 = this.IFRS2ManagedAdapterInstance.InitializeFileTransferAsync(1, 1, false);
                    this.Manager.Checkpoint("MS-FRS2_R774");
                    this.Manager.Checkpoint("MS-FRS2_R777");
                    this.Manager.Checkpoint("MS-FRS2_R793");
                    this.Manager.Comment("reaching state \'S213\'");
                    this.Manager.Comment("checking step \'return InitializeFileTransferAsync/ERROR_SUCCESS\'");
                    this.Manager.Assert((temp52 == FRS2Model.error_status_t.ERROR_SUCCESS), String.Format("expected \'FRS2Model.error_status_t.ERROR_SUCCESS\', actual \'{0}\' (return of Initia" +
                                "lizeFileTransferAsync, state S213)", TestManagerHelpers.Describe(temp52)));
                    this.Manager.Comment("reaching state \'S223\'");
                    int temp63 = this.Manager.ExpectEvent(this.QuiescenceTimeout, true, new ExpectedEvent(TCtestScenario1.InitializeFileTransferAsyncEventInfo, null, new InitializeFileTransferAsyncEventDelegate1(this.TCtestScenario1S4InitializeFileTransferAsyncEventChecker8)), new ExpectedEvent(TCtestScenario1.InitializeFileTransferAsyncEventInfo, null, new InitializeFileTransferAsyncEventDelegate1(this.TCtestScenario1S4InitializeFileTransferAsyncEventChecker9)), new ExpectedEvent(TCtestScenario1.InitializeFileTransferAsyncEventInfo, null, new InitializeFileTransferAsyncEventDelegate1(this.TCtestScenario1S4InitializeFileTransferAsyncEventChecker10)), new ExpectedEvent(TCtestScenario1.InitializeFileTransferAsyncEventInfo, null, new InitializeFileTransferAsyncEventDelegate1(this.TCtestScenario1S4InitializeFileTransferAsyncEventChecker11)), new ExpectedEvent(TCtestScenario1.InitializeFileTransferAsyncEventInfo, null, new InitializeFileTransferAsyncEventDelegate1(this.TCtestScenario1S4InitializeFileTransferAsyncEventChecker12)), new ExpectedEvent(TCtestScenario1.InitializeFileTransferAsyncEventInfo, null, new InitializeFileTransferAsyncEventDelegate1(this.TCtestScenario1S4InitializeFileTransferAsyncEventChecker13)), new ExpectedEvent(TCtestScenario1.InitializeFileTransferAsyncEventInfo, null, new InitializeFileTransferAsyncEventDelegate1(this.TCtestScenario1S4InitializeFileTransferAsyncEventChecker14)), new ExpectedEvent(TCtestScenario1.InitializeFileTransferAsyncEventInfo, null, new InitializeFileTransferAsyncEventDelegate1(this.TCtestScenario1S4InitializeFileTransferAsyncEventChecker15)));
                    if ((temp63 == 0))
                    {
                        this.Manager.Comment("reaching state \'S243\'");
                        FRS2Model.error_status_t temp53;
                        this.Manager.Comment("executing step \'call RawGetFileData()\'");
                        temp53 = this.IFRS2ManagedAdapterInstance.RawGetFileData();
                        this.Manager.Checkpoint("MS-FRS2_R622");
                        this.Manager.Checkpoint("MS-FRS2_R625");
                        this.Manager.Comment("reaching state \'S267\'");
                        this.Manager.Comment("checking step \'return RawGetFileData/ERROR_SUCCESS\'");
                        this.Manager.Assert((temp53 == FRS2Model.error_status_t.ERROR_SUCCESS), String.Format("expected \'FRS2Model.error_status_t.ERROR_SUCCESS\', actual \'{0}\' (return of RawGet" +
                                    "FileData, state S267)", TestManagerHelpers.Describe(temp53)));
                        this.Manager.Comment("reaching state \'S291\'");
                        int temp55 = this.Manager.ExpectEvent(this.QuiescenceTimeout, true, new ExpectedEvent(TCtestScenario1.RawGetFileDataResponseEventInfo, null, new RawGetFileDataResponseEventDelegate1(this.TCtestScenario1S4RawGetFileDataResponseEventChecker2)), new ExpectedEvent(TCtestScenario1.RawGetFileDataResponseEventInfo, null, new RawGetFileDataResponseEventDelegate1(this.TCtestScenario1S4RawGetFileDataResponseEventChecker3)));
                        if ((temp55 == 0))
                        {
                            this.Manager.Comment("reaching state \'S303\'");
                            FRS2Model.error_status_t temp54;
                            this.Manager.Comment("executing step \'call RdcClose()\'");
                            temp54 = this.IFRS2ManagedAdapterInstance.RdcClose();
                            this.Manager.Checkpoint("MS-FRS2_R737");
                            this.Manager.Checkpoint("MS-FRS2_R739");
                            this.Manager.Comment("reaching state \'S307\'");
                            this.Manager.Comment("checking step \'return RdcClose/ERROR_SUCCESS\'");
                            this.Manager.Assert((temp54 == FRS2Model.error_status_t.ERROR_SUCCESS), String.Format("expected \'FRS2Model.error_status_t.ERROR_SUCCESS\', actual \'{0}\' (return of RdcClo" +
                                        "se, state S307)", TestManagerHelpers.Describe(temp54)));
                            this.Manager.Comment("reaching state \'S310\'");
                            goto label6;
                        }
                        if ((temp55 == 1))
                        {
                            this.Manager.Comment("reaching state \'S304\'");
                            goto label6;
                        }
                        throw new InvalidOperationException("never reached");
                    label6:
                        ;
                        goto label7;
                    }
                    if ((temp63 == 1))
                    {
                        this.Manager.Comment("reaching state \'S244\'");
                        FRS2Model.error_status_t temp56;
                        this.Manager.Comment("executing step \'call RawGetFileData()\'");
                        temp56 = this.IFRS2ManagedAdapterInstance.RawGetFileData();
                        this.Manager.Checkpoint("MS-FRS2_R623");
                        this.Manager.Checkpoint("MS-FRS2_R629");
                        this.Manager.Checkpoint("MS-FRS2_R630");
                        this.Manager.Checkpoint("MS-FRS2_R804");
                        this.Manager.Comment("reaching state \'S268\'");
                        this.Manager.Comment("checking step \'return RawGetFileData/ERROR_FAIL\'");
                        this.Manager.Assert((temp56 == FRS2Model.error_status_t.ERROR_FAIL), String.Format("expected \'FRS2Model.error_status_t.ERROR_FAIL\', actual \'{0}\' (return of RawGetFil" +
                                    "eData, state S268)", TestManagerHelpers.Describe(temp56)));
                        this.Manager.Comment("reaching state \'S292\'");
                        goto label7;
                    }
                    if ((temp63 == 2))
                    {
                        this.Manager.Comment("reaching state \'S245\'");
                        FRS2Model.error_status_t temp57;
                        this.Manager.Comment("executing step \'call RawGetFileData()\'");
                        temp57 = this.IFRS2ManagedAdapterInstance.RawGetFileData();
                        this.Manager.Checkpoint("MS-FRS2_R623");
                        this.Manager.Checkpoint("MS-FRS2_R629");
                        this.Manager.Checkpoint("MS-FRS2_R630");
                        this.Manager.Checkpoint("MS-FRS2_R804");
                        this.Manager.Comment("reaching state \'S269\'");
                        this.Manager.Comment("checking step \'return RawGetFileData/ERROR_FAIL\'");
                        this.Manager.Assert((temp57 == FRS2Model.error_status_t.ERROR_FAIL), String.Format("expected \'FRS2Model.error_status_t.ERROR_FAIL\', actual \'{0}\' (return of RawGetFil" +
                                    "eData, state S269)", TestManagerHelpers.Describe(temp57)));
                        this.Manager.Comment("reaching state \'S293\'");
                        goto label7;
                    }
                    if ((temp63 == 3))
                    {
                        this.Manager.Comment("reaching state \'S246\'");
                        FRS2Model.error_status_t temp58;
                        this.Manager.Comment("executing step \'call RawGetFileData()\'");
                        temp58 = this.IFRS2ManagedAdapterInstance.RawGetFileData();
                        this.Manager.Checkpoint("MS-FRS2_R623");
                        this.Manager.Checkpoint("MS-FRS2_R804");
                        this.Manager.Comment("reaching state \'S270\'");
                        this.Manager.Comment("checking step \'return RawGetFileData/ERROR_FAIL\'");
                        this.Manager.Assert((temp58 == FRS2Model.error_status_t.ERROR_FAIL), String.Format("expected \'FRS2Model.error_status_t.ERROR_FAIL\', actual \'{0}\' (return of RawGetFil" +
                                    "eData, state S270)", TestManagerHelpers.Describe(temp58)));
                        this.Manager.Comment("reaching state \'S294\'");
                        goto label7;
                    }
                    if ((temp63 == 4))
                    {
                        this.Manager.Comment("reaching state \'S247\'");
                        FRS2Model.error_status_t temp59;
                        this.Manager.Comment("executing step \'call RawGetFileData()\'");
                        temp59 = this.IFRS2ManagedAdapterInstance.RawGetFileData();
                        this.Manager.Checkpoint("MS-FRS2_R623");
                        this.Manager.Checkpoint("MS-FRS2_R629");
                        this.Manager.Checkpoint("MS-FRS2_R630");
                        this.Manager.Checkpoint("MS-FRS2_R804");
                        this.Manager.Comment("reaching state \'S271\'");
                        this.Manager.Comment("checking step \'return RawGetFileData/ERROR_FAIL\'");
                        this.Manager.Assert((temp59 == FRS2Model.error_status_t.ERROR_FAIL), String.Format("expected \'FRS2Model.error_status_t.ERROR_FAIL\', actual \'{0}\' (return of RawGetFil" +
                                    "eData, state S271)", TestManagerHelpers.Describe(temp59)));
                        this.Manager.Comment("reaching state \'S295\'");
                        goto label7;
                    }
                    if ((temp63 == 5))
                    {
                        this.Manager.Comment("reaching state \'S248\'");
                        FRS2Model.error_status_t temp60;
                        this.Manager.Comment("executing step \'call RawGetFileData()\'");
                        temp60 = this.IFRS2ManagedAdapterInstance.RawGetFileData();
                        this.Manager.Checkpoint("MS-FRS2_R623");
                        this.Manager.Checkpoint("MS-FRS2_R804");
                        this.Manager.Comment("reaching state \'S272\'");
                        this.Manager.Comment("checking step \'return RawGetFileData/ERROR_FAIL\'");
                        this.Manager.Assert((temp60 == FRS2Model.error_status_t.ERROR_FAIL), String.Format("expected \'FRS2Model.error_status_t.ERROR_FAIL\', actual \'{0}\' (return of RawGetFil" +
                                    "eData, state S272)", TestManagerHelpers.Describe(temp60)));
                        this.Manager.Comment("reaching state \'S296\'");
                        goto label7;
                    }
                    if ((temp63 == 6))
                    {
                        this.Manager.Comment("reaching state \'S249\'");
                        FRS2Model.error_status_t temp61;
                        this.Manager.Comment("executing step \'call RawGetFileData()\'");
                        temp61 = this.IFRS2ManagedAdapterInstance.RawGetFileData();
                        this.Manager.Checkpoint("MS-FRS2_R623");
                        this.Manager.Checkpoint("MS-FRS2_R629");
                        this.Manager.Checkpoint("MS-FRS2_R630");
                        this.Manager.Checkpoint("MS-FRS2_R804");
                        this.Manager.Comment("reaching state \'S273\'");
                        this.Manager.Comment("checking step \'return RawGetFileData/ERROR_FAIL\'");
                        this.Manager.Assert((temp61 == FRS2Model.error_status_t.ERROR_FAIL), String.Format("expected \'FRS2Model.error_status_t.ERROR_FAIL\', actual \'{0}\' (return of RawGetFil" +
                                    "eData, state S273)", TestManagerHelpers.Describe(temp61)));
                        this.Manager.Comment("reaching state \'S297\'");
                        goto label7;
                    }
                    if ((temp63 == 7))
                    {
                        this.Manager.Comment("reaching state \'S250\'");
                        FRS2Model.error_status_t temp62;
                        this.Manager.Comment("executing step \'call RawGetFileData()\'");
                        temp62 = this.IFRS2ManagedAdapterInstance.RawGetFileData();
                        this.Manager.Checkpoint("MS-FRS2_R623");
                        this.Manager.Checkpoint("MS-FRS2_R804");
                        this.Manager.Comment("reaching state \'S274\'");
                        this.Manager.Comment("checking step \'return RawGetFileData/ERROR_FAIL\'");
                        this.Manager.Assert((temp62 == FRS2Model.error_status_t.ERROR_FAIL), String.Format("expected \'FRS2Model.error_status_t.ERROR_FAIL\', actual \'{0}\' (return of RawGetFil" +
                                    "eData, state S274)", TestManagerHelpers.Describe(temp62)));
                        this.Manager.Comment("reaching state \'S298\'");
                        goto label7;
                    }
                    throw new InvalidOperationException("never reached");
                label7:
                    ;
                    goto label8;
                }
                throw new InvalidOperationException("never reached");
            label8:
                ;
                goto label9;
            }
            if ((temp65 == 1))
            {
                TCtestScenario1S179();
                goto label9;
            }
            throw new InvalidOperationException("never reached");
        label9:
            ;
            this.Manager.EndTest();
        }

        private void TCtestScenario1S4AsyncPollResponseEventChecker(FRS2Model.VVGeneration vvGen)
        {
            this.Manager.Comment("checking step \'event AsyncPollResponseEvent(ValidValue)\'");
            try
            {
                this.Manager.Assert((vvGen == FRS2Model.VVGeneration.ValidValue), String.Format("expected \'FRS2Model.VVGeneration.ValidValue\', actual \'{0}\' (vvGen of AsyncPollRes" +
                            "ponseEvent, state S171)", TestManagerHelpers.Describe(vvGen)));
            }
            catch (TransactionFailedException)
            {
                this.Manager.Comment("This step would have covered MS-FRS2_R556, MS-FRS2_R1020, MS-FRS2_R555");
                throw;
            }
            this.Manager.Checkpoint("MS-FRS2_R556");
            this.Manager.Checkpoint("MS-FRS2_R1020");
            this.Manager.Checkpoint("MS-FRS2_R555");
        }

        private void TCtestScenario1S4RequestUpdatesEventChecker(FRS2Model.FilePresense present, FRS2Model.UPDATE_STATUS updateStatus)
        {
            this.Manager.Comment("checking step \'event RequestUpdatesEvent(fileDeleted,UPDATE_STATUS_MORE)\'");
            try
            {
                this.Manager.Assert((present == FRS2Model.FilePresense.fileDeleted), String.Format("expected \'FRS2Model.FilePresense.fileDeleted\', actual \'{0}\' (present of RequestUp" +
                            "datesEvent, state S191)", TestManagerHelpers.Describe(present)));
                this.Manager.Assert((updateStatus == FRS2Model.UPDATE_STATUS.UPDATE_STATUS_MORE), String.Format("expected \'FRS2Model.UPDATE_STATUS.UPDATE_STATUS_MORE\', actual \'{0}\' (updateStatus" +
                            " of RequestUpdatesEvent, state S191)", TestManagerHelpers.Describe(updateStatus)));
            }
            catch (TransactionFailedException)
            {
                this.Manager.Comment("This step would have covered MS-FRS2_R93");
                throw;
            }
            this.Manager.Checkpoint("MS-FRS2_R93");
        }

        private void TCtestScenario1S4RequestUpdatesEventChecker1(FRS2Model.FilePresense present, FRS2Model.UPDATE_STATUS updateStatus)
        {
            this.Manager.Comment("checking step \'event RequestUpdatesEvent(fileDeleted,UPDATE_STATUS_DONE)\'");
            try
            {
                this.Manager.Assert((present == FRS2Model.FilePresense.fileDeleted), String.Format("expected \'FRS2Model.FilePresense.fileDeleted\', actual \'{0}\' (present of RequestUp" +
                            "datesEvent, state S191)", TestManagerHelpers.Describe(present)));
                this.Manager.Assert((updateStatus == FRS2Model.UPDATE_STATUS.UPDATE_STATUS_DONE), String.Format("expected \'FRS2Model.UPDATE_STATUS.UPDATE_STATUS_DONE\', actual \'{0}\' (updateStatus" +
                            " of RequestUpdatesEvent, state S191)", TestManagerHelpers.Describe(updateStatus)));
            }
            catch (TransactionFailedException)
            {
                this.Manager.Comment("This step would have covered MS-FRS2_R93, MS-FRS2_R94");
                throw;
            }
            this.Manager.Checkpoint("MS-FRS2_R93");
            this.Manager.Checkpoint("MS-FRS2_R94");
        }

        private void TCtestScenario1S4RequestUpdatesEventChecker2(FRS2Model.FilePresense present, FRS2Model.UPDATE_STATUS updateStatus)
        {
            this.Manager.Comment("checking step \'event RequestUpdatesEvent(fileExists,UPDATE_STATUS_DONE)\'");
            try
            {
                this.Manager.Assert((present == FRS2Model.FilePresense.fileExists), String.Format("expected \'FRS2Model.FilePresense.fileExists\', actual \'{0}\' (present of RequestUpd" +
                            "atesEvent, state S191)", TestManagerHelpers.Describe(present)));
                this.Manager.Assert((updateStatus == FRS2Model.UPDATE_STATUS.UPDATE_STATUS_DONE), String.Format("expected \'FRS2Model.UPDATE_STATUS.UPDATE_STATUS_DONE\', actual \'{0}\' (updateStatus" +
                            " of RequestUpdatesEvent, state S191)", TestManagerHelpers.Describe(updateStatus)));
            }
            catch (TransactionFailedException)
            {
                this.Manager.Comment("This step would have covered MS-FRS2_R93, MS-FRS2_R94");
                throw;
            }
            this.Manager.Checkpoint("MS-FRS2_R93");
            this.Manager.Checkpoint("MS-FRS2_R94");
        }

        private void TCtestScenario1S4InitializeFileTransferAsyncEventChecker(FRS2Model.ServerContext context, FRS2Model.RDC_Sig_Level rdcsigLevel, bool isEOF)
        {
            this.Manager.Comment("checking step \'event InitializeFileTransferAsyncEvent(ValidContext,forRAWFileLeve" +
                    "l,False)\'");
            this.Manager.Assert((context == FRS2Model.ServerContext.ValidContext), String.Format("expected \'FRS2Model.ServerContext.ValidContext\', actual \'{0}\' (context of Initial" +
                        "izeFileTransferAsyncEvent, state S222)", TestManagerHelpers.Describe(context)));
            this.Manager.Assert((rdcsigLevel == FRS2Model.RDC_Sig_Level.forRAWFileLevel), String.Format("expected \'FRS2Model.RDC_Sig_Level.forRAWFileLevel\', actual \'{0}\' (rdcsigLevel of " +
                        "InitializeFileTransferAsyncEvent, state S222)", TestManagerHelpers.Describe(rdcsigLevel)));
            this.Manager.Assert((isEOF == false), String.Format("expected \'false\', actual \'{0}\' (isEOF of InitializeFileTransferAsyncEvent, state " +
                        "S222)", TestManagerHelpers.Describe(isEOF)));
        }

        private void TCtestScenario1S4RawGetFileDataResponseEventChecker(bool isEOF)
        {
            this.Manager.Comment("checking step \'event RawGetFileDataResponseEvent(True)\'");
            this.Manager.Assert((isEOF == true), String.Format("expected \'true\', actual \'{0}\' (isEOF of RawGetFileDataResponseEvent, state S283)", TestManagerHelpers.Describe(isEOF)));
        }

        private void TCtestScenario1S4RawGetFileDataResponseEventChecker1(bool isEOF)
        {
            this.Manager.Comment("checking step \'event RawGetFileDataResponseEvent(False)\'");
            this.Manager.Assert((isEOF == false), String.Format("expected \'false\', actual \'{0}\' (isEOF of RawGetFileDataResponseEvent, state S283)" +
                        "", TestManagerHelpers.Describe(isEOF)));
        }

        private void TCtestScenario1S4InitializeFileTransferAsyncEventChecker1(FRS2Model.ServerContext context, FRS2Model.RDC_Sig_Level rdcsigLevel, bool isEOF)
        {
            this.Manager.Comment("checking step \'event InitializeFileTransferAsyncEvent(InvalidContext,forRAWFileLe" +
                    "vel,False)\'");
            this.Manager.Assert((context == FRS2Model.ServerContext.InvalidContext), String.Format("expected \'FRS2Model.ServerContext.InvalidContext\', actual \'{0}\' (context of Initi" +
                        "alizeFileTransferAsyncEvent, state S222)", TestManagerHelpers.Describe(context)));
            this.Manager.Assert((rdcsigLevel == FRS2Model.RDC_Sig_Level.forRAWFileLevel), String.Format("expected \'FRS2Model.RDC_Sig_Level.forRAWFileLevel\', actual \'{0}\' (rdcsigLevel of " +
                        "InitializeFileTransferAsyncEvent, state S222)", TestManagerHelpers.Describe(rdcsigLevel)));
            this.Manager.Assert((isEOF == false), String.Format("expected \'false\', actual \'{0}\' (isEOF of InitializeFileTransferAsyncEvent, state " +
                        "S222)", TestManagerHelpers.Describe(isEOF)));
        }

        private void TCtestScenario1S4InitializeFileTransferAsyncEventChecker2(FRS2Model.ServerContext context, FRS2Model.RDC_Sig_Level rdcsigLevel, bool isEOF)
        {
            this.Manager.Comment("checking step \'event InitializeFileTransferAsyncEvent(InvalidContext,forRDCFileLe" +
                    "vel,True)\'");
            this.Manager.Assert((context == FRS2Model.ServerContext.InvalidContext), String.Format("expected \'FRS2Model.ServerContext.InvalidContext\', actual \'{0}\' (context of Initi" +
                        "alizeFileTransferAsyncEvent, state S222)", TestManagerHelpers.Describe(context)));
            this.Manager.Assert((rdcsigLevel == FRS2Model.RDC_Sig_Level.forRDCFileLevel), String.Format("expected \'FRS2Model.RDC_Sig_Level.forRDCFileLevel\', actual \'{0}\' (rdcsigLevel of " +
                        "InitializeFileTransferAsyncEvent, state S222)", TestManagerHelpers.Describe(rdcsigLevel)));
            this.Manager.Assert((isEOF == true), String.Format("expected \'true\', actual \'{0}\' (isEOF of InitializeFileTransferAsyncEvent, state S" +
                        "222)", TestManagerHelpers.Describe(isEOF)));
        }

        private void TCtestScenario1S4InitializeFileTransferAsyncEventChecker3(FRS2Model.ServerContext context, FRS2Model.RDC_Sig_Level rdcsigLevel, bool isEOF)
        {
            this.Manager.Comment("checking step \'event InitializeFileTransferAsyncEvent(ValidContext,forRDCFileLeve" +
                    "l,False)\'");
            this.Manager.Assert((context == FRS2Model.ServerContext.ValidContext), String.Format("expected \'FRS2Model.ServerContext.ValidContext\', actual \'{0}\' (context of Initial" +
                        "izeFileTransferAsyncEvent, state S222)", TestManagerHelpers.Describe(context)));
            this.Manager.Assert((rdcsigLevel == FRS2Model.RDC_Sig_Level.forRDCFileLevel), String.Format("expected \'FRS2Model.RDC_Sig_Level.forRDCFileLevel\', actual \'{0}\' (rdcsigLevel of " +
                        "InitializeFileTransferAsyncEvent, state S222)", TestManagerHelpers.Describe(rdcsigLevel)));
            this.Manager.Assert((isEOF == false), String.Format("expected \'false\', actual \'{0}\' (isEOF of InitializeFileTransferAsyncEvent, state " +
                        "S222)", TestManagerHelpers.Describe(isEOF)));
        }

        private void TCtestScenario1S4InitializeFileTransferAsyncEventChecker4(FRS2Model.ServerContext context, FRS2Model.RDC_Sig_Level rdcsigLevel, bool isEOF)
        {
            this.Manager.Comment("checking step \'event InitializeFileTransferAsyncEvent(InvalidContext,forRDCFileLe" +
                    "vel,False)\'");
            this.Manager.Assert((context == FRS2Model.ServerContext.InvalidContext), String.Format("expected \'FRS2Model.ServerContext.InvalidContext\', actual \'{0}\' (context of Initi" +
                        "alizeFileTransferAsyncEvent, state S222)", TestManagerHelpers.Describe(context)));
            this.Manager.Assert((rdcsigLevel == FRS2Model.RDC_Sig_Level.forRDCFileLevel), String.Format("expected \'FRS2Model.RDC_Sig_Level.forRDCFileLevel\', actual \'{0}\' (rdcsigLevel of " +
                        "InitializeFileTransferAsyncEvent, state S222)", TestManagerHelpers.Describe(rdcsigLevel)));
            this.Manager.Assert((isEOF == false), String.Format("expected \'false\', actual \'{0}\' (isEOF of InitializeFileTransferAsyncEvent, state " +
                        "S222)", TestManagerHelpers.Describe(isEOF)));
        }

        private void TCtestScenario1S4InitializeFileTransferAsyncEventChecker5(FRS2Model.ServerContext context, FRS2Model.RDC_Sig_Level rdcsigLevel, bool isEOF)
        {
            this.Manager.Comment("checking step \'event InitializeFileTransferAsyncEvent(ValidContext,forRDCFileLeve" +
                    "l,True)\'");
            this.Manager.Assert((context == FRS2Model.ServerContext.ValidContext), String.Format("expected \'FRS2Model.ServerContext.ValidContext\', actual \'{0}\' (context of Initial" +
                        "izeFileTransferAsyncEvent, state S222)", TestManagerHelpers.Describe(context)));
            this.Manager.Assert((rdcsigLevel == FRS2Model.RDC_Sig_Level.forRDCFileLevel), String.Format("expected \'FRS2Model.RDC_Sig_Level.forRDCFileLevel\', actual \'{0}\' (rdcsigLevel of " +
                        "InitializeFileTransferAsyncEvent, state S222)", TestManagerHelpers.Describe(rdcsigLevel)));
            this.Manager.Assert((isEOF == true), String.Format("expected \'true\', actual \'{0}\' (isEOF of InitializeFileTransferAsyncEvent, state S" +
                        "222)", TestManagerHelpers.Describe(isEOF)));
        }

        private void TCtestScenario1S4InitializeFileTransferAsyncEventChecker6(FRS2Model.ServerContext context, FRS2Model.RDC_Sig_Level rdcsigLevel, bool isEOF)
        {
            this.Manager.Comment("checking step \'event InitializeFileTransferAsyncEvent(InvalidContext,forRAWFileLe" +
                    "vel,True)\'");
            this.Manager.Assert((context == FRS2Model.ServerContext.InvalidContext), String.Format("expected \'FRS2Model.ServerContext.InvalidContext\', actual \'{0}\' (context of Initi" +
                        "alizeFileTransferAsyncEvent, state S222)", TestManagerHelpers.Describe(context)));
            this.Manager.Assert((rdcsigLevel == FRS2Model.RDC_Sig_Level.forRAWFileLevel), String.Format("expected \'FRS2Model.RDC_Sig_Level.forRAWFileLevel\', actual \'{0}\' (rdcsigLevel of " +
                        "InitializeFileTransferAsyncEvent, state S222)", TestManagerHelpers.Describe(rdcsigLevel)));
            this.Manager.Assert((isEOF == true), String.Format("expected \'true\', actual \'{0}\' (isEOF of InitializeFileTransferAsyncEvent, state S" +
                        "222)", TestManagerHelpers.Describe(isEOF)));
        }

        private void TCtestScenario1S4InitializeFileTransferAsyncEventChecker7(FRS2Model.ServerContext context, FRS2Model.RDC_Sig_Level rdcsigLevel, bool isEOF)
        {
            this.Manager.Comment("checking step \'event InitializeFileTransferAsyncEvent(ValidContext,forRAWFileLeve" +
                    "l,True)\'");
            this.Manager.Assert((context == FRS2Model.ServerContext.ValidContext), String.Format("expected \'FRS2Model.ServerContext.ValidContext\', actual \'{0}\' (context of Initial" +
                        "izeFileTransferAsyncEvent, state S222)", TestManagerHelpers.Describe(context)));
            this.Manager.Assert((rdcsigLevel == FRS2Model.RDC_Sig_Level.forRAWFileLevel), String.Format("expected \'FRS2Model.RDC_Sig_Level.forRAWFileLevel\', actual \'{0}\' (rdcsigLevel of " +
                        "InitializeFileTransferAsyncEvent, state S222)", TestManagerHelpers.Describe(rdcsigLevel)));
            this.Manager.Assert((isEOF == true), String.Format("expected \'true\', actual \'{0}\' (isEOF of InitializeFileTransferAsyncEvent, state S" +
                        "222)", TestManagerHelpers.Describe(isEOF)));
        }

        private void TCtestScenario1S4RequestUpdatesEventChecker3(FRS2Model.FilePresense present, FRS2Model.UPDATE_STATUS updateStatus)
        {
            this.Manager.Comment("checking step \'event RequestUpdatesEvent(fileExists,UPDATE_STATUS_MORE)\'");
            try
            {
                this.Manager.Assert((present == FRS2Model.FilePresense.fileExists), String.Format("expected \'FRS2Model.FilePresense.fileExists\', actual \'{0}\' (present of RequestUpd" +
                            "atesEvent, state S191)", TestManagerHelpers.Describe(present)));
                this.Manager.Assert((updateStatus == FRS2Model.UPDATE_STATUS.UPDATE_STATUS_MORE), String.Format("expected \'FRS2Model.UPDATE_STATUS.UPDATE_STATUS_MORE\', actual \'{0}\' (updateStatus" +
                            " of RequestUpdatesEvent, state S191)", TestManagerHelpers.Describe(updateStatus)));
            }
            catch (TransactionFailedException)
            {
                this.Manager.Comment("This step would have covered MS-FRS2_R93, MS-FRS2_R498");
                throw;
            }
            this.Manager.Checkpoint("MS-FRS2_R93");
            this.Manager.Checkpoint("MS-FRS2_R498");
        }

        private void TCtestScenario1S4InitializeFileTransferAsyncEventChecker8(FRS2Model.ServerContext context, FRS2Model.RDC_Sig_Level rdcsigLevel, bool isEOF)
        {
            this.Manager.Comment("checking step \'event InitializeFileTransferAsyncEvent(ValidContext,forRAWFileLeve" +
                    "l,False)\'");
            this.Manager.Assert((context == FRS2Model.ServerContext.ValidContext), String.Format("expected \'FRS2Model.ServerContext.ValidContext\', actual \'{0}\' (context of Initial" +
                        "izeFileTransferAsyncEvent, state S223)", TestManagerHelpers.Describe(context)));
            this.Manager.Assert((rdcsigLevel == FRS2Model.RDC_Sig_Level.forRAWFileLevel), String.Format("expected \'FRS2Model.RDC_Sig_Level.forRAWFileLevel\', actual \'{0}\' (rdcsigLevel of " +
                        "InitializeFileTransferAsyncEvent, state S223)", TestManagerHelpers.Describe(rdcsigLevel)));
            this.Manager.Assert((isEOF == false), String.Format("expected \'false\', actual \'{0}\' (isEOF of InitializeFileTransferAsyncEvent, state " +
                        "S223)", TestManagerHelpers.Describe(isEOF)));
        }

        private void TCtestScenario1S4RawGetFileDataResponseEventChecker2(bool isEOF)
        {
            this.Manager.Comment("checking step \'event RawGetFileDataResponseEvent(True)\'");
            this.Manager.Assert((isEOF == true), String.Format("expected \'true\', actual \'{0}\' (isEOF of RawGetFileDataResponseEvent, state S291)", TestManagerHelpers.Describe(isEOF)));
        }

        private void TCtestScenario1S4RawGetFileDataResponseEventChecker3(bool isEOF)
        {
            this.Manager.Comment("checking step \'event RawGetFileDataResponseEvent(False)\'");
            this.Manager.Assert((isEOF == false), String.Format("expected \'false\', actual \'{0}\' (isEOF of RawGetFileDataResponseEvent, state S291)" +
                        "", TestManagerHelpers.Describe(isEOF)));
        }

        private void TCtestScenario1S4InitializeFileTransferAsyncEventChecker9(FRS2Model.ServerContext context, FRS2Model.RDC_Sig_Level rdcsigLevel, bool isEOF)
        {
            this.Manager.Comment("checking step \'event InitializeFileTransferAsyncEvent(InvalidContext,forRAWFileLe" +
                    "vel,False)\'");
            this.Manager.Assert((context == FRS2Model.ServerContext.InvalidContext), String.Format("expected \'FRS2Model.ServerContext.InvalidContext\', actual \'{0}\' (context of Initi" +
                        "alizeFileTransferAsyncEvent, state S223)", TestManagerHelpers.Describe(context)));
            this.Manager.Assert((rdcsigLevel == FRS2Model.RDC_Sig_Level.forRAWFileLevel), String.Format("expected \'FRS2Model.RDC_Sig_Level.forRAWFileLevel\', actual \'{0}\' (rdcsigLevel of " +
                        "InitializeFileTransferAsyncEvent, state S223)", TestManagerHelpers.Describe(rdcsigLevel)));
            this.Manager.Assert((isEOF == false), String.Format("expected \'false\', actual \'{0}\' (isEOF of InitializeFileTransferAsyncEvent, state " +
                        "S223)", TestManagerHelpers.Describe(isEOF)));
        }

        private void TCtestScenario1S4InitializeFileTransferAsyncEventChecker10(FRS2Model.ServerContext context, FRS2Model.RDC_Sig_Level rdcsigLevel, bool isEOF)
        {
            this.Manager.Comment("checking step \'event InitializeFileTransferAsyncEvent(InvalidContext,forRDCFileLe" +
                    "vel,True)\'");
            this.Manager.Assert((context == FRS2Model.ServerContext.InvalidContext), String.Format("expected \'FRS2Model.ServerContext.InvalidContext\', actual \'{0}\' (context of Initi" +
                        "alizeFileTransferAsyncEvent, state S223)", TestManagerHelpers.Describe(context)));
            this.Manager.Assert((rdcsigLevel == FRS2Model.RDC_Sig_Level.forRDCFileLevel), String.Format("expected \'FRS2Model.RDC_Sig_Level.forRDCFileLevel\', actual \'{0}\' (rdcsigLevel of " +
                        "InitializeFileTransferAsyncEvent, state S223)", TestManagerHelpers.Describe(rdcsigLevel)));
            this.Manager.Assert((isEOF == true), String.Format("expected \'true\', actual \'{0}\' (isEOF of InitializeFileTransferAsyncEvent, state S" +
                        "223)", TestManagerHelpers.Describe(isEOF)));
        }

        private void TCtestScenario1S4InitializeFileTransferAsyncEventChecker11(FRS2Model.ServerContext context, FRS2Model.RDC_Sig_Level rdcsigLevel, bool isEOF)
        {
            this.Manager.Comment("checking step \'event InitializeFileTransferAsyncEvent(ValidContext,forRDCFileLeve" +
                    "l,False)\'");
            this.Manager.Assert((context == FRS2Model.ServerContext.ValidContext), String.Format("expected \'FRS2Model.ServerContext.ValidContext\', actual \'{0}\' (context of Initial" +
                        "izeFileTransferAsyncEvent, state S223)", TestManagerHelpers.Describe(context)));
            this.Manager.Assert((rdcsigLevel == FRS2Model.RDC_Sig_Level.forRDCFileLevel), String.Format("expected \'FRS2Model.RDC_Sig_Level.forRDCFileLevel\', actual \'{0}\' (rdcsigLevel of " +
                        "InitializeFileTransferAsyncEvent, state S223)", TestManagerHelpers.Describe(rdcsigLevel)));
            this.Manager.Assert((isEOF == false), String.Format("expected \'false\', actual \'{0}\' (isEOF of InitializeFileTransferAsyncEvent, state " +
                        "S223)", TestManagerHelpers.Describe(isEOF)));
        }

        private void TCtestScenario1S4InitializeFileTransferAsyncEventChecker12(FRS2Model.ServerContext context, FRS2Model.RDC_Sig_Level rdcsigLevel, bool isEOF)
        {
            this.Manager.Comment("checking step \'event InitializeFileTransferAsyncEvent(InvalidContext,forRDCFileLe" +
                    "vel,False)\'");
            this.Manager.Assert((context == FRS2Model.ServerContext.InvalidContext), String.Format("expected \'FRS2Model.ServerContext.InvalidContext\', actual \'{0}\' (context of Initi" +
                        "alizeFileTransferAsyncEvent, state S223)", TestManagerHelpers.Describe(context)));
            this.Manager.Assert((rdcsigLevel == FRS2Model.RDC_Sig_Level.forRDCFileLevel), String.Format("expected \'FRS2Model.RDC_Sig_Level.forRDCFileLevel\', actual \'{0}\' (rdcsigLevel of " +
                        "InitializeFileTransferAsyncEvent, state S223)", TestManagerHelpers.Describe(rdcsigLevel)));
            this.Manager.Assert((isEOF == false), String.Format("expected \'false\', actual \'{0}\' (isEOF of InitializeFileTransferAsyncEvent, state " +
                        "S223)", TestManagerHelpers.Describe(isEOF)));
        }

        private void TCtestScenario1S4InitializeFileTransferAsyncEventChecker13(FRS2Model.ServerContext context, FRS2Model.RDC_Sig_Level rdcsigLevel, bool isEOF)
        {
            this.Manager.Comment("checking step \'event InitializeFileTransferAsyncEvent(ValidContext,forRDCFileLeve" +
                    "l,True)\'");
            this.Manager.Assert((context == FRS2Model.ServerContext.ValidContext), String.Format("expected \'FRS2Model.ServerContext.ValidContext\', actual \'{0}\' (context of Initial" +
                        "izeFileTransferAsyncEvent, state S223)", TestManagerHelpers.Describe(context)));
            this.Manager.Assert((rdcsigLevel == FRS2Model.RDC_Sig_Level.forRDCFileLevel), String.Format("expected \'FRS2Model.RDC_Sig_Level.forRDCFileLevel\', actual \'{0}\' (rdcsigLevel of " +
                        "InitializeFileTransferAsyncEvent, state S223)", TestManagerHelpers.Describe(rdcsigLevel)));
            this.Manager.Assert((isEOF == true), String.Format("expected \'true\', actual \'{0}\' (isEOF of InitializeFileTransferAsyncEvent, state S" +
                        "223)", TestManagerHelpers.Describe(isEOF)));
        }

        private void TCtestScenario1S4InitializeFileTransferAsyncEventChecker14(FRS2Model.ServerContext context, FRS2Model.RDC_Sig_Level rdcsigLevel, bool isEOF)
        {
            this.Manager.Comment("checking step \'event InitializeFileTransferAsyncEvent(InvalidContext,forRAWFileLe" +
                    "vel,True)\'");
            this.Manager.Assert((context == FRS2Model.ServerContext.InvalidContext), String.Format("expected \'FRS2Model.ServerContext.InvalidContext\', actual \'{0}\' (context of Initi" +
                        "alizeFileTransferAsyncEvent, state S223)", TestManagerHelpers.Describe(context)));
            this.Manager.Assert((rdcsigLevel == FRS2Model.RDC_Sig_Level.forRAWFileLevel), String.Format("expected \'FRS2Model.RDC_Sig_Level.forRAWFileLevel\', actual \'{0}\' (rdcsigLevel of " +
                        "InitializeFileTransferAsyncEvent, state S223)", TestManagerHelpers.Describe(rdcsigLevel)));
            this.Manager.Assert((isEOF == true), String.Format("expected \'true\', actual \'{0}\' (isEOF of InitializeFileTransferAsyncEvent, state S" +
                        "223)", TestManagerHelpers.Describe(isEOF)));
        }

        private void TCtestScenario1S4InitializeFileTransferAsyncEventChecker15(FRS2Model.ServerContext context, FRS2Model.RDC_Sig_Level rdcsigLevel, bool isEOF)
        {
            this.Manager.Comment("checking step \'event InitializeFileTransferAsyncEvent(ValidContext,forRAWFileLeve" +
                    "l,True)\'");
            this.Manager.Assert((context == FRS2Model.ServerContext.ValidContext), String.Format("expected \'FRS2Model.ServerContext.ValidContext\', actual \'{0}\' (context of Initial" +
                        "izeFileTransferAsyncEvent, state S223)", TestManagerHelpers.Describe(context)));
            this.Manager.Assert((rdcsigLevel == FRS2Model.RDC_Sig_Level.forRAWFileLevel), String.Format("expected \'FRS2Model.RDC_Sig_Level.forRAWFileLevel\', actual \'{0}\' (rdcsigLevel of " +
                        "InitializeFileTransferAsyncEvent, state S223)", TestManagerHelpers.Describe(rdcsigLevel)));
            this.Manager.Assert((isEOF == true), String.Format("expected \'true\', actual \'{0}\' (isEOF of InitializeFileTransferAsyncEvent, state S" +
                        "223)", TestManagerHelpers.Describe(isEOF)));
        }

        private void TCtestScenario1S4AsyncPollResponseEventChecker1(FRS2Model.VVGeneration vvGen)
        {
            this.Manager.Comment("checking step \'event AsyncPollResponseEvent(InvalidValue)\'");
            try
            {
                this.Manager.Assert((vvGen == FRS2Model.VVGeneration.InvalidValue), String.Format("expected \'FRS2Model.VVGeneration.InvalidValue\', actual \'{0}\' (vvGen of AsyncPollR" +
                            "esponseEvent, state S171)", TestManagerHelpers.Describe(vvGen)));
            }
            catch (TransactionFailedException)
            {
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
        
        public virtual void FRS2_TCtestScenario1S6()
        {
            this.Manager.BeginTest("TCtestScenario1S6");
            this.Manager.Comment("reaching state \'S6\'");
            FRS2Model.error_status_t temp66;
            this.Manager.Comment(@"executing step 'call Initialization(Windows7,Map{1=>enabled,2=>enabled,3=>enabled,4=>disabled},Map{1=>exists,2=>exists,3=>exists,4=>exists},Map{""A""=>1,""B""=>2,""C""=>3,""D""=>4},Map{""A""=>sysvol,""B""=>protection,""C""=>sysvol,""D""=>sysvol},Map{1=>""A"",2=>""B"",3=>""C"",4=>""D""},Map{1=>writeOnly,2=>readWrite,3=>readOnly,4=>readWrite},Map{1=>enabled,2=>disabled,3=>enabled,4=>enabled})'");
            temp66 = this.IFRS2ManagedAdapterInstance.Initialization(FRS2Model.OSVersion.Windows7, new Microsoft.Modeling.Map<System.Int32, FRS2Model.connectionProperty>().Add(1, FRS2Model.connectionProperty.enabled).Add(2, FRS2Model.connectionProperty.enabled).Add(3, FRS2Model.connectionProperty.enabled).Add(4, FRS2Model.connectionProperty.disabled), new Microsoft.Modeling.Map<System.Int32, FRS2Model.FromServer>().Add(1, FRS2Model.FromServer.exists).Add(2, FRS2Model.FromServer.exists).Add(3, FRS2Model.FromServer.exists).Add(4, FRS2Model.FromServer.exists), new Microsoft.Modeling.Map<System.String, System.Int32>().Add("A", 1).Add("B", 2).Add("C", 3).Add("D", 4), new Microsoft.Modeling.Map<System.String, FRS2Model.ReplicationGroupTypes>().Add("A", FRS2Model.ReplicationGroupTypes.sysvol).Add("B", FRS2Model.ReplicationGroupTypes.protection).Add("C", FRS2Model.ReplicationGroupTypes.sysvol).Add("D", FRS2Model.ReplicationGroupTypes.sysvol), new Microsoft.Modeling.Map<System.Int32, System.String>().Add(1, "A").Add(2, "B").Add(3, "C").Add(4, "D"), new Microsoft.Modeling.Map<System.Int32, FRS2Model.accessLevels>().Add(1, FRS2Model.accessLevels.writeOnly).Add(2, FRS2Model.accessLevels.readWrite).Add(3, FRS2Model.accessLevels.readOnly).Add(4, FRS2Model.accessLevels.readWrite), new Microsoft.Modeling.Map<System.Int32, FRS2Model.connectionProperty>().Add(1, FRS2Model.connectionProperty.enabled).Add(2, FRS2Model.connectionProperty.disabled).Add(3, FRS2Model.connectionProperty.enabled).Add(4, FRS2Model.connectionProperty.enabled));
            this.Manager.Comment("reaching state \'S7\'");
            this.Manager.Comment("checking step \'return Initialization/ERROR_SUCCESS\'");
            this.Manager.Assert((temp66 == FRS2Model.error_status_t.ERROR_SUCCESS), String.Format("expected \'FRS2Model.error_status_t.ERROR_SUCCESS\', actual \'{0}\' (return of Initia" +
                        "lization, state S7)", TestManagerHelpers.Describe(temp66)));
            this.Manager.Comment("reaching state \'S39\'");
            FRS2Model.ProtocolVersionReturned temp67;
            FRS2Model.UpstreamFlagValueReturned temp68;
            FRS2Model.error_status_t temp69;
            this.Manager.Comment("executing step \'call EstablishConnection(\"A\",1,FRS_COMMUNICATION_PROTOCOL_VERSION" +
                    "_LONGHORN_SERVER,out _,out _)\'");
            temp69 = this.IFRS2ManagedAdapterInstance.EstablishConnection("A", 1, FRS2Model.ProtocolVersion.FRS_COMMUNICATION_PROTOCOL_VERSION_LONGHORN_SERVER, out temp67, out temp68);
            this.Manager.Checkpoint("MS-FRS2_R37");
            this.Manager.Checkpoint("MS-FRS2_R436");
            this.Manager.Checkpoint("MS-FRS2_R439");
            this.Manager.Comment("reaching state \'S57\'");
            this.Manager.Comment("checking step \'return EstablishConnection/[out Valid,out Valid]:ERROR_SUCCESS\'");
            this.Manager.Assert((temp67 == FRS2Model.ProtocolVersionReturned.Valid), String.Format("expected \'FRS2Model.ProtocolVersionReturned.Valid\', actual \'{0}\' (upstreamProtoco" +
                        "lVersion of EstablishConnection, state S57)", TestManagerHelpers.Describe(temp67)));
            this.Manager.Assert((temp68 == FRS2Model.UpstreamFlagValueReturned.Valid), String.Format("expected \'FRS2Model.UpstreamFlagValueReturned.Valid\', actual \'{0}\' (upstreamFlags" +
                        " of EstablishConnection, state S57)", TestManagerHelpers.Describe(temp68)));
            this.Manager.Assert((temp69 == FRS2Model.error_status_t.ERROR_SUCCESS), String.Format("expected \'FRS2Model.error_status_t.ERROR_SUCCESS\', actual \'{0}\' (return of Establ" +
                        "ishConnection, state S57)", TestManagerHelpers.Describe(temp69)));
            this.Manager.Comment("reaching state \'S75\'");
            FRS2Model.error_status_t temp70;
            this.Manager.Comment("executing step \'call EstablishSession(1,1)\'");
            temp70 = this.IFRS2ManagedAdapterInstance.EstablishSession(1, 1);
            this.Manager.Checkpoint("MS-FRS2_R455");
            this.Manager.Checkpoint("MS-FRS2_R458");
            this.Manager.Checkpoint("MS-FRS2_R448");
            this.Manager.Comment("reaching state \'S93\'");
            this.Manager.Comment("checking step \'return EstablishSession/ERROR_SUCCESS\'");
            this.Manager.Assert((temp70 == FRS2Model.error_status_t.ERROR_SUCCESS), String.Format("expected \'FRS2Model.error_status_t.ERROR_SUCCESS\', actual \'{0}\' (return of Establ" +
                        "ishSession, state S93)", TestManagerHelpers.Describe(temp70)));
            this.Manager.Comment("reaching state \'S111\'");
            FRS2Model.error_status_t temp71;
            this.Manager.Comment("executing step \'call AsyncPoll(1)\'");
            temp71 = this.IFRS2ManagedAdapterInstance.AsyncPoll(1);
            this.Manager.Checkpoint("MS-FRS2_R549");
            this.Manager.Checkpoint("MS-FRS2_R552");
            this.Manager.Comment("reaching state \'S128\'");
            this.Manager.Comment("checking step \'return AsyncPoll/ERROR_SUCCESS\'");
            this.Manager.Assert((temp71 == FRS2Model.error_status_t.ERROR_SUCCESS), String.Format("expected \'FRS2Model.error_status_t.ERROR_SUCCESS\', actual \'{0}\' (return of AsyncP" +
                        "oll, state S128)", TestManagerHelpers.Describe(temp71)));
            this.Manager.Comment("reaching state \'S144\'");
            FRS2Model.error_status_t temp72;
            this.Manager.Comment("executing step \'call RequestVersionVector(1,1,1,REQUEST_NORMAL_SYNC,CHANGE_NOTIFY" +
                    ",ValidValue)\'");
            temp72 = this.IFRS2ManagedAdapterInstance.RequestVersionVector(1, 1, 1, FRS2Model.VERSION_REQUEST_TYPE.REQUEST_NORMAL_SYNC, FRS2Model.VERSION_CHANGE_TYPE.CHANGE_NOTIFY, FRS2Model.VVGeneration.ValidValue);
            this.Manager.Checkpoint("MS-FRS2_R517");
            this.Manager.Checkpoint("MS-FRS2_R520");
            this.Manager.Checkpoint("MS-FRS2_R466");
            this.Manager.Comment("reaching state \'S160\'");
            this.Manager.Comment("checking step \'return RequestVersionVector/ERROR_SUCCESS\'");
            this.Manager.Assert((temp72 == FRS2Model.error_status_t.ERROR_SUCCESS), String.Format("expected \'FRS2Model.error_status_t.ERROR_SUCCESS\', actual \'{0}\' (return of Reques" +
                        "tVersionVector, state S160)", TestManagerHelpers.Describe(temp72)));
            this.Manager.Comment("reaching state \'S172\'");
            int temp74 = this.Manager.ExpectEvent(this.QuiescenceTimeout, true, new ExpectedEvent(TCtestScenario1.AsyncPollResponseEventInfo, null, new AsyncPollResponseEventDelegate1(this.TCtestScenario1S6AsyncPollResponseEventChecker)), new ExpectedEvent(TCtestScenario1.AsyncPollResponseEventInfo, null, new AsyncPollResponseEventDelegate1(this.TCtestScenario1S6AsyncPollResponseEventChecker1)));
            if ((temp74 == 0))
            {
                this.Manager.Comment("reaching state \'S182\'");
                FRS2Model.error_status_t temp73;
                this.Manager.Comment("executing step \'call RequestUpdates(4,1,inValid)\'");
                temp73 = this.IFRS2ManagedAdapterInstance.RequestUpdates(4, 1, FRS2Model.versionVectorDiff.inValid);
                this.Manager.Checkpoint("MS-FRS2_R487");
                this.Manager.Checkpoint("MS-FRS2_R492");
                this.Manager.Checkpoint("MS-FRS2_R494");
                this.Manager.Comment("reaching state \'S188\'");
                this.Manager.Comment("checking step \'return RequestUpdates/ERROR_FAIL\'");
                this.Manager.Assert((temp73 == FRS2Model.error_status_t.ERROR_FAIL), String.Format("expected \'FRS2Model.error_status_t.ERROR_FAIL\', actual \'{0}\' (return of RequestUp" +
                            "dates, state S188)", TestManagerHelpers.Describe(temp73)));
                this.Manager.Comment("reaching state \'S192\'");
                goto label10;
            }
            if ((temp74 == 1))
            {
                TCtestScenario1S179();
                goto label10;
            }
            throw new InvalidOperationException("never reached");
        label10:
            ;
            this.Manager.EndTest();
        }

        private void TCtestScenario1S6AsyncPollResponseEventChecker(FRS2Model.VVGeneration vvGen)
        {
            this.Manager.Comment("checking step \'event AsyncPollResponseEvent(ValidValue)\'");
            try
            {
                this.Manager.Assert((vvGen == FRS2Model.VVGeneration.ValidValue), String.Format("expected \'FRS2Model.VVGeneration.ValidValue\', actual \'{0}\' (vvGen of AsyncPollRes" +
                            "ponseEvent, state S172)", TestManagerHelpers.Describe(vvGen)));
            }
            catch (TransactionFailedException)
            {
                this.Manager.Comment("This step would have covered MS-FRS2_R556, MS-FRS2_R1020, MS-FRS2_R555");
                throw;
            }
            this.Manager.Checkpoint("MS-FRS2_R556");
            this.Manager.Checkpoint("MS-FRS2_R1020");
            this.Manager.Checkpoint("MS-FRS2_R555");
        }

        private void TCtestScenario1S6AsyncPollResponseEventChecker1(FRS2Model.VVGeneration vvGen)
        {
            this.Manager.Comment("checking step \'event AsyncPollResponseEvent(InvalidValue)\'");
            try
            {
                this.Manager.Assert((vvGen == FRS2Model.VVGeneration.InvalidValue), String.Format("expected \'FRS2Model.VVGeneration.InvalidValue\', actual \'{0}\' (vvGen of AsyncPollR" +
                            "esponseEvent, state S172)", TestManagerHelpers.Describe(vvGen)));
            }
            catch (TransactionFailedException)
            {
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
            ", MS-FRS2_R550, MS-FRS2_R554, MS-FRS2_R518, MS-FRS2_R522, MS-FRS2_R525, MS-FRS2_" +
            "R527, MS-FRS2_R530")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestCategory("AD")]
        [TestCategory("MS-FRS2")]
        [TestCategory("SDC")]
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]      
        public virtual void FRS2_TCtestScenario1S8()
        {
            this.Manager.BeginTest("TCtestScenario1S8");
            this.Manager.Comment("reaching state \'S8\'");
            FRS2Model.error_status_t temp75;
            this.Manager.Comment(@"executing step 'call Initialization(Windows7,Map{1=>enabled,2=>enabled,3=>enabled,4=>disabled},Map{1=>exists,2=>exists,3=>exists,4=>exists},Map{""A""=>1,""B""=>2,""C""=>3,""D""=>4},Map{""A""=>sysvol,""B""=>protection,""C""=>sysvol,""D""=>sysvol},Map{1=>""A"",2=>""B"",3=>""C"",4=>""D""},Map{1=>writeOnly,2=>readWrite,3=>readOnly,4=>readWrite},Map{1=>enabled,2=>disabled,3=>enabled,4=>enabled})'");
            temp75 = this.IFRS2ManagedAdapterInstance.Initialization(FRS2Model.OSVersion.Windows7, new Microsoft.Modeling.Map<System.Int32, FRS2Model.connectionProperty>().Add(1, FRS2Model.connectionProperty.enabled).Add(2, FRS2Model.connectionProperty.enabled).Add(3, FRS2Model.connectionProperty.enabled).Add(4, FRS2Model.connectionProperty.disabled), new Microsoft.Modeling.Map<System.Int32, FRS2Model.FromServer>().Add(1, FRS2Model.FromServer.exists).Add(2, FRS2Model.FromServer.exists).Add(3, FRS2Model.FromServer.exists).Add(4, FRS2Model.FromServer.exists), new Microsoft.Modeling.Map<System.String, System.Int32>().Add("A", 1).Add("B", 2).Add("C", 3).Add("D", 4), new Microsoft.Modeling.Map<System.String, FRS2Model.ReplicationGroupTypes>().Add("A", FRS2Model.ReplicationGroupTypes.sysvol).Add("B", FRS2Model.ReplicationGroupTypes.protection).Add("C", FRS2Model.ReplicationGroupTypes.sysvol).Add("D", FRS2Model.ReplicationGroupTypes.sysvol), new Microsoft.Modeling.Map<System.Int32, System.String>().Add(1, "A").Add(2, "B").Add(3, "C").Add(4, "D"), new Microsoft.Modeling.Map<System.Int32, FRS2Model.accessLevels>().Add(1, FRS2Model.accessLevels.writeOnly).Add(2, FRS2Model.accessLevels.readWrite).Add(3, FRS2Model.accessLevels.readOnly).Add(4, FRS2Model.accessLevels.readWrite), new Microsoft.Modeling.Map<System.Int32, FRS2Model.connectionProperty>().Add(1, FRS2Model.connectionProperty.enabled).Add(2, FRS2Model.connectionProperty.disabled).Add(3, FRS2Model.connectionProperty.enabled).Add(4, FRS2Model.connectionProperty.enabled));
            this.Manager.Comment("reaching state \'S9\'");
            this.Manager.Comment("checking step \'return Initialization/ERROR_SUCCESS\'");
            this.Manager.Assert((temp75 == FRS2Model.error_status_t.ERROR_SUCCESS), String.Format("expected \'FRS2Model.error_status_t.ERROR_SUCCESS\', actual \'{0}\' (return of Initia" +
                        "lization, state S9)", TestManagerHelpers.Describe(temp75)));
            this.Manager.Comment("reaching state \'S40\'");
            FRS2Model.ProtocolVersionReturned temp76;
            FRS2Model.UpstreamFlagValueReturned temp77;
            FRS2Model.error_status_t temp78;
            this.Manager.Comment("executing step \'call EstablishConnection(\"A\",1,FRS_COMMUNICATION_PROTOCOL_VERSION" +
                    "_LONGHORN_SERVER,out _,out _)\'");
            temp78 = this.IFRS2ManagedAdapterInstance.EstablishConnection("A", 1, FRS2Model.ProtocolVersion.FRS_COMMUNICATION_PROTOCOL_VERSION_LONGHORN_SERVER, out temp76, out temp77);
            this.Manager.Checkpoint("MS-FRS2_R37");
            this.Manager.Checkpoint("MS-FRS2_R436");
            this.Manager.Checkpoint("MS-FRS2_R439");
            this.Manager.Comment("reaching state \'S58\'");
            this.Manager.Comment("checking step \'return EstablishConnection/[out Valid,out Valid]:ERROR_SUCCESS\'");
            this.Manager.Assert((temp76 == FRS2Model.ProtocolVersionReturned.Valid), String.Format("expected \'FRS2Model.ProtocolVersionReturned.Valid\', actual \'{0}\' (upstreamProtoco" +
                        "lVersion of EstablishConnection, state S58)", TestManagerHelpers.Describe(temp76)));
            this.Manager.Assert((temp77 == FRS2Model.UpstreamFlagValueReturned.Valid), String.Format("expected \'FRS2Model.UpstreamFlagValueReturned.Valid\', actual \'{0}\' (upstreamFlags" +
                        " of EstablishConnection, state S58)", TestManagerHelpers.Describe(temp77)));
            this.Manager.Assert((temp78 == FRS2Model.error_status_t.ERROR_SUCCESS), String.Format("expected \'FRS2Model.error_status_t.ERROR_SUCCESS\', actual \'{0}\' (return of Establ" +
                        "ishConnection, state S58)", TestManagerHelpers.Describe(temp78)));
            this.Manager.Comment("reaching state \'S76\'");
            FRS2Model.error_status_t temp79;
            this.Manager.Comment("executing step \'call EstablishSession(1,1)\'");
            temp79 = this.IFRS2ManagedAdapterInstance.EstablishSession(1, 1);
            this.Manager.Checkpoint("MS-FRS2_R455");
            this.Manager.Checkpoint("MS-FRS2_R458");
            this.Manager.Checkpoint("MS-FRS2_R448");
            this.Manager.Comment("reaching state \'S94\'");
            this.Manager.Comment("checking step \'return EstablishSession/ERROR_SUCCESS\'");
            this.Manager.Assert((temp79 == FRS2Model.error_status_t.ERROR_SUCCESS), String.Format("expected \'FRS2Model.error_status_t.ERROR_SUCCESS\', actual \'{0}\' (return of Establ" +
                        "ishSession, state S94)", TestManagerHelpers.Describe(temp79)));
            this.Manager.Comment("reaching state \'S112\'");
            FRS2Model.error_status_t temp80;
            this.Manager.Comment("executing step \'call AsyncPoll(2)\'");
            temp80 = this.IFRS2ManagedAdapterInstance.AsyncPoll(2);
            this.Manager.Checkpoint("MS-FRS2_R550");
            this.Manager.Checkpoint("MS-FRS2_R554");
            this.Manager.Comment("reaching state \'S129\'");
            this.Manager.Comment("checking step \'return AsyncPoll/ERROR_FAIL\'");
            this.Manager.Assert((temp80 == FRS2Model.error_status_t.ERROR_FAIL), String.Format("expected \'FRS2Model.error_status_t.ERROR_FAIL\', actual \'{0}\' (return of AsyncPoll" +
                        ", state S129)", TestManagerHelpers.Describe(temp80)));
            this.Manager.Comment("reaching state \'S145\'");
            FRS2Model.error_status_t temp81;
            this.Manager.Comment("executing step \'call RequestVersionVector(1,4,1,REQUEST_SLAVE_SYNC,CHANGE_NOTIFY," +
                    "InvalidValue)\'");
            temp81 = this.IFRS2ManagedAdapterInstance.RequestVersionVector(1, 4, 1, FRS2Model.VERSION_REQUEST_TYPE.REQUEST_SLAVE_SYNC, FRS2Model.VERSION_CHANGE_TYPE.CHANGE_NOTIFY, FRS2Model.VVGeneration.InvalidValue);
            this.Manager.Checkpoint("MS-FRS2_R518");
            this.Manager.Checkpoint("MS-FRS2_R522");
            this.Manager.Checkpoint("MS-FRS2_R525");
            this.Manager.Checkpoint("MS-FRS2_R527");
            this.Manager.Checkpoint("MS-FRS2_R530");
            this.Manager.AddReturn(RequestVersionVectorInfo, null, temp81);
            TCtestScenario1S139();
            this.Manager.EndTest();
        }

        private void TCtestScenario1S139()
        {
            this.Manager.Comment("reaching state \'S139\'");
            this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(TCtestScenario1.RequestVersionVectorInfo, null, new RequestVersionVectorDelegate1(this.TCtestScenario1S8RequestVersionVectorChecker)));
            TCtestScenario1S152();
        }

        private void TCtestScenario1S8RequestVersionVectorChecker(FRS2Model.error_status_t @return)
        {
            this.Manager.Comment("checking step \'return RequestVersionVector/ERROR_FAIL\'");
            if (!Microsoft.Protocols.TestSuites.MS_FRS2.ConfigStore.IsTestSYSVOL)
                this.Manager.Assert((@return == FRS2Model.error_status_t.ERROR_FAIL), String.Format("expected \'FRS2Model.error_status_t.ERROR_FAIL\', actual \'{0}\' (return of RequestVe" +
                            "rsionVector, state S139)", TestManagerHelpers.Describe(@return)));
            else
                this.Manager.Assert((@return == FRS2Model.error_status_t.ERROR_INVALID_PARAMETER), String.Format("expected \'FRS2Model.error_status_t.ERROR_INVALID_PARAMETER\', actual \'{0}\' (return of RequestVe" +
                       "rsionVector, state S139)", TestManagerHelpers.Describe(@return)));
        }

        private void TCtestScenario1S152()
        {
            this.Manager.Comment("reaching state \'S152\'");
        }
        #endregion

        #region Test Starting in S10
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        [Microsoft.VisualStudio.TestTools.UnitTesting.DescriptionAttribute("MS-FRS2_R37, MS-FRS2_R436, MS-FRS2_R439, MS-FRS2_R455, MS-FRS2_R458, MS-FRS2_R448" +
            ", MS-FRS2_R550, MS-FRS2_R554, MS-FRS2_R518, MS-FRS2_R522, MS-FRS2_R523, MS-FRS2_" +
            "R529")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestCategory("AD")]
        [TestCategory("MS-FRS2")]
        [TestCategory("SDC")]
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]        
        public virtual void FRS2_TCtestScenario1S10()
        {
            this.Manager.BeginTest("TCtestScenario1S10");
            this.Manager.Comment("reaching state \'S10\'");
            FRS2Model.error_status_t temp82;
            this.Manager.Comment(@"executing step 'call Initialization(Windows7,Map{1=>enabled,2=>enabled,3=>enabled,4=>disabled},Map{1=>exists,2=>exists,3=>exists,4=>exists},Map{""A""=>1,""B""=>2,""C""=>3,""D""=>4},Map{""A""=>sysvol,""B""=>protection,""C""=>sysvol,""D""=>sysvol},Map{1=>""A"",2=>""B"",3=>""C"",4=>""D""},Map{1=>writeOnly,2=>readWrite,3=>readOnly,4=>readWrite},Map{1=>enabled,2=>disabled,3=>enabled,4=>enabled})'");
            temp82 = this.IFRS2ManagedAdapterInstance.Initialization(FRS2Model.OSVersion.Windows7, new Microsoft.Modeling.Map<System.Int32, FRS2Model.connectionProperty>().Add(1, FRS2Model.connectionProperty.enabled).Add(2, FRS2Model.connectionProperty.enabled).Add(3, FRS2Model.connectionProperty.enabled).Add(4, FRS2Model.connectionProperty.disabled), new Microsoft.Modeling.Map<System.Int32, FRS2Model.FromServer>().Add(1, FRS2Model.FromServer.exists).Add(2, FRS2Model.FromServer.exists).Add(3, FRS2Model.FromServer.exists).Add(4, FRS2Model.FromServer.exists), new Microsoft.Modeling.Map<System.String, System.Int32>().Add("A", 1).Add("B", 2).Add("C", 3).Add("D", 4), new Microsoft.Modeling.Map<System.String, FRS2Model.ReplicationGroupTypes>().Add("A", FRS2Model.ReplicationGroupTypes.sysvol).Add("B", FRS2Model.ReplicationGroupTypes.protection).Add("C", FRS2Model.ReplicationGroupTypes.sysvol).Add("D", FRS2Model.ReplicationGroupTypes.sysvol), new Microsoft.Modeling.Map<System.Int32, System.String>().Add(1, "A").Add(2, "B").Add(3, "C").Add(4, "D"), new Microsoft.Modeling.Map<System.Int32, FRS2Model.accessLevels>().Add(1, FRS2Model.accessLevels.writeOnly).Add(2, FRS2Model.accessLevels.readWrite).Add(3, FRS2Model.accessLevels.readOnly).Add(4, FRS2Model.accessLevels.readWrite), new Microsoft.Modeling.Map<System.Int32, FRS2Model.connectionProperty>().Add(1, FRS2Model.connectionProperty.enabled).Add(2, FRS2Model.connectionProperty.disabled).Add(3, FRS2Model.connectionProperty.enabled).Add(4, FRS2Model.connectionProperty.enabled));
            this.Manager.Comment("reaching state \'S11\'");
            this.Manager.Comment("checking step \'return Initialization/ERROR_SUCCESS\'");
            this.Manager.Assert((temp82 == FRS2Model.error_status_t.ERROR_SUCCESS), String.Format("expected \'FRS2Model.error_status_t.ERROR_SUCCESS\', actual \'{0}\' (return of Initia" +
                        "lization, state S11)", TestManagerHelpers.Describe(temp82)));
            this.Manager.Comment("reaching state \'S41\'");
            FRS2Model.ProtocolVersionReturned temp83;
            FRS2Model.UpstreamFlagValueReturned temp84;
            FRS2Model.error_status_t temp85;
            this.Manager.Comment("executing step \'call EstablishConnection(\"A\",1,FRS_COMMUNICATION_PROTOCOL_VERSION" +
                    "_LONGHORN_SERVER,out _,out _)\'");
            temp85 = this.IFRS2ManagedAdapterInstance.EstablishConnection("A", 1, FRS2Model.ProtocolVersion.FRS_COMMUNICATION_PROTOCOL_VERSION_LONGHORN_SERVER, out temp83, out temp84);
            this.Manager.Checkpoint("MS-FRS2_R37");
            this.Manager.Checkpoint("MS-FRS2_R436");
            this.Manager.Checkpoint("MS-FRS2_R439");
            this.Manager.Comment("reaching state \'S59\'");
            this.Manager.Comment("checking step \'return EstablishConnection/[out Valid,out Valid]:ERROR_SUCCESS\'");
            this.Manager.Assert((temp83 == FRS2Model.ProtocolVersionReturned.Valid), String.Format("expected \'FRS2Model.ProtocolVersionReturned.Valid\', actual \'{0}\' (upstreamProtoco" +
                        "lVersion of EstablishConnection, state S59)", TestManagerHelpers.Describe(temp83)));
            this.Manager.Assert((temp84 == FRS2Model.UpstreamFlagValueReturned.Valid), String.Format("expected \'FRS2Model.UpstreamFlagValueReturned.Valid\', actual \'{0}\' (upstreamFlags" +
                        " of EstablishConnection, state S59)", TestManagerHelpers.Describe(temp84)));
            this.Manager.Assert((temp85 == FRS2Model.error_status_t.ERROR_SUCCESS), String.Format("expected \'FRS2Model.error_status_t.ERROR_SUCCESS\', actual \'{0}\' (return of Establ" +
                        "ishConnection, state S59)", TestManagerHelpers.Describe(temp85)));
            this.Manager.Comment("reaching state \'S77\'");
            FRS2Model.error_status_t temp86;
            this.Manager.Comment("executing step \'call EstablishSession(1,1)\'");
            temp86 = this.IFRS2ManagedAdapterInstance.EstablishSession(1, 1);
            this.Manager.Checkpoint("MS-FRS2_R455");
            this.Manager.Checkpoint("MS-FRS2_R458");
            this.Manager.Checkpoint("MS-FRS2_R448");
            this.Manager.Comment("reaching state \'S95\'");
            this.Manager.Comment("checking step \'return EstablishSession/ERROR_SUCCESS\'");
            this.Manager.Assert((temp86 == FRS2Model.error_status_t.ERROR_SUCCESS), String.Format("expected \'FRS2Model.error_status_t.ERROR_SUCCESS\', actual \'{0}\' (return of Establ" +
                        "ishSession, state S95)", TestManagerHelpers.Describe(temp86)));
            this.Manager.Comment("reaching state \'S113\'");
            FRS2Model.error_status_t temp87;
            this.Manager.Comment("executing step \'call AsyncPoll(2)\'");
            temp87 = this.IFRS2ManagedAdapterInstance.AsyncPoll(2);
            this.Manager.Checkpoint("MS-FRS2_R550");
            this.Manager.Checkpoint("MS-FRS2_R554");
            this.Manager.Comment("reaching state \'S130\'");
            this.Manager.Comment("checking step \'return AsyncPoll/ERROR_FAIL\'");
            this.Manager.Assert((temp87 == FRS2Model.error_status_t.ERROR_FAIL), String.Format("expected \'FRS2Model.error_status_t.ERROR_FAIL\', actual \'{0}\' (return of AsyncPoll" +
                        ", state S130)", TestManagerHelpers.Describe(temp87)));
            this.Manager.Comment("reaching state \'S146\'");
            FRS2Model.error_status_t temp88;
            this.Manager.Comment("executing step \'call RequestVersionVector(1,4,5,REQUEST_NORMAL_SYNC,CHANGE_ALL,In" +
                    "validValue)\'");
            temp88 = this.IFRS2ManagedAdapterInstance.RequestVersionVector(1, 4, 5, FRS2Model.VERSION_REQUEST_TYPE.REQUEST_NORMAL_SYNC, FRS2Model.VERSION_CHANGE_TYPE.CHANGE_ALL, FRS2Model.VVGeneration.InvalidValue);
            this.Manager.Checkpoint("MS-FRS2_R518");
            this.Manager.Checkpoint("MS-FRS2_R522");
            this.Manager.Checkpoint("MS-FRS2_R523");
            this.Manager.Checkpoint("MS-FRS2_R529");
            this.Manager.AddReturn(RequestVersionVectorInfo, null, temp88);
            TCtestScenario1S136();
            this.Manager.EndTest();
        }

        private void TCtestScenario1S136()
        {
            this.Manager.Comment("reaching state \'S136\'");
            this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(TCtestScenario1.RequestVersionVectorInfo, null, new RequestVersionVectorDelegate1(this.TCtestScenario1S10RequestVersionVectorChecker)));
            TCtestScenario1S152();
        }

        private void TCtestScenario1S10RequestVersionVectorChecker(FRS2Model.error_status_t @return)
        {
            this.Manager.Comment("checking step \'return RequestVersionVector/ERROR_FAIL\'");
            this.Manager.Assert((@return == FRS2Model.error_status_t.ERROR_FAIL), String.Format("expected \'FRS2Model.error_status_t.ERROR_FAIL\', actual \'{0}\' (return of RequestVe" +
                        "rsionVector, state S136)", TestManagerHelpers.Describe(@return)));
        }
        #endregion

        #region Test Starting in S12
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        [Microsoft.VisualStudio.TestTools.UnitTesting.DescriptionAttribute("MS-FRS2_R37, MS-FRS2_R436, MS-FRS2_R439, MS-FRS2_R455, MS-FRS2_R458, MS-FRS2_R448" +
            ", MS-FRS2_R550, MS-FRS2_R554, MS-FRS2_R518, MS-FRS2_R523, MS-FRS2_R529")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestCategory("AD")]
        [TestCategory("MS-FRS2")]
        [TestCategory("SDC")]
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]       
        public virtual void FRS2_TCtestScenario1S12()
        {
            this.Manager.BeginTest("TCtestScenario1S12");
            this.Manager.Comment("reaching state \'S12\'");
            FRS2Model.error_status_t temp89;
            this.Manager.Comment(@"executing step 'call Initialization(Windows7,Map{1=>enabled,2=>enabled,3=>enabled,4=>disabled},Map{1=>exists,2=>exists,3=>exists,4=>exists},Map{""A""=>1,""B""=>2,""C""=>3,""D""=>4},Map{""A""=>sysvol,""B""=>protection,""C""=>sysvol,""D""=>sysvol},Map{1=>""A"",2=>""B"",3=>""C"",4=>""D""},Map{1=>writeOnly,2=>readWrite,3=>readOnly,4=>readWrite},Map{1=>enabled,2=>disabled,3=>enabled,4=>enabled})'");
            temp89 = this.IFRS2ManagedAdapterInstance.Initialization(FRS2Model.OSVersion.Windows7, new Microsoft.Modeling.Map<System.Int32, FRS2Model.connectionProperty>().Add(1, FRS2Model.connectionProperty.enabled).Add(2, FRS2Model.connectionProperty.enabled).Add(3, FRS2Model.connectionProperty.enabled).Add(4, FRS2Model.connectionProperty.disabled), new Microsoft.Modeling.Map<System.Int32, FRS2Model.FromServer>().Add(1, FRS2Model.FromServer.exists).Add(2, FRS2Model.FromServer.exists).Add(3, FRS2Model.FromServer.exists).Add(4, FRS2Model.FromServer.exists), new Microsoft.Modeling.Map<System.String, System.Int32>().Add("A", 1).Add("B", 2).Add("C", 3).Add("D", 4), new Microsoft.Modeling.Map<System.String, FRS2Model.ReplicationGroupTypes>().Add("A", FRS2Model.ReplicationGroupTypes.sysvol).Add("B", FRS2Model.ReplicationGroupTypes.protection).Add("C", FRS2Model.ReplicationGroupTypes.sysvol).Add("D", FRS2Model.ReplicationGroupTypes.sysvol), new Microsoft.Modeling.Map<System.Int32, System.String>().Add(1, "A").Add(2, "B").Add(3, "C").Add(4, "D"), new Microsoft.Modeling.Map<System.Int32, FRS2Model.accessLevels>().Add(1, FRS2Model.accessLevels.writeOnly).Add(2, FRS2Model.accessLevels.readWrite).Add(3, FRS2Model.accessLevels.readOnly).Add(4, FRS2Model.accessLevels.readWrite), new Microsoft.Modeling.Map<System.Int32, FRS2Model.connectionProperty>().Add(1, FRS2Model.connectionProperty.enabled).Add(2, FRS2Model.connectionProperty.disabled).Add(3, FRS2Model.connectionProperty.enabled).Add(4, FRS2Model.connectionProperty.enabled));
            this.Manager.Comment("reaching state \'S13\'");
            this.Manager.Comment("checking step \'return Initialization/ERROR_SUCCESS\'");
            this.Manager.Assert((temp89 == FRS2Model.error_status_t.ERROR_SUCCESS), String.Format("expected \'FRS2Model.error_status_t.ERROR_SUCCESS\', actual \'{0}\' (return of Initia" +
                        "lization, state S13)", TestManagerHelpers.Describe(temp89)));
            this.Manager.Comment("reaching state \'S42\'");
            FRS2Model.ProtocolVersionReturned temp90;
            FRS2Model.UpstreamFlagValueReturned temp91;
            FRS2Model.error_status_t temp92;
            this.Manager.Comment("executing step \'call EstablishConnection(\"A\",1,FRS_COMMUNICATION_PROTOCOL_VERSION" +
                    "_LONGHORN_SERVER,out _,out _)\'");
            temp92 = this.IFRS2ManagedAdapterInstance.EstablishConnection("A", 1, FRS2Model.ProtocolVersion.FRS_COMMUNICATION_PROTOCOL_VERSION_LONGHORN_SERVER, out temp90, out temp91);
            this.Manager.Checkpoint("MS-FRS2_R37");
            this.Manager.Checkpoint("MS-FRS2_R436");
            this.Manager.Checkpoint("MS-FRS2_R439");
            this.Manager.Comment("reaching state \'S60\'");
            this.Manager.Comment("checking step \'return EstablishConnection/[out Valid,out Valid]:ERROR_SUCCESS\'");
            this.Manager.Assert((temp90 == FRS2Model.ProtocolVersionReturned.Valid), String.Format("expected \'FRS2Model.ProtocolVersionReturned.Valid\', actual \'{0}\' (upstreamProtoco" +
                        "lVersion of EstablishConnection, state S60)", TestManagerHelpers.Describe(temp90)));
            this.Manager.Assert((temp91 == FRS2Model.UpstreamFlagValueReturned.Valid), String.Format("expected \'FRS2Model.UpstreamFlagValueReturned.Valid\', actual \'{0}\' (upstreamFlags" +
                        " of EstablishConnection, state S60)", TestManagerHelpers.Describe(temp91)));
            this.Manager.Assert((temp92 == FRS2Model.error_status_t.ERROR_SUCCESS), String.Format("expected \'FRS2Model.error_status_t.ERROR_SUCCESS\', actual \'{0}\' (return of Establ" +
                        "ishConnection, state S60)", TestManagerHelpers.Describe(temp92)));
            this.Manager.Comment("reaching state \'S78\'");
            FRS2Model.error_status_t temp93;
            this.Manager.Comment("executing step \'call EstablishSession(1,1)\'");
            temp93 = this.IFRS2ManagedAdapterInstance.EstablishSession(1, 1);
            this.Manager.Checkpoint("MS-FRS2_R455");
            this.Manager.Checkpoint("MS-FRS2_R458");
            this.Manager.Checkpoint("MS-FRS2_R448");
            this.Manager.Comment("reaching state \'S96\'");
            this.Manager.Comment("checking step \'return EstablishSession/ERROR_SUCCESS\'");
            this.Manager.Assert((temp93 == FRS2Model.error_status_t.ERROR_SUCCESS), String.Format("expected \'FRS2Model.error_status_t.ERROR_SUCCESS\', actual \'{0}\' (return of Establ" +
                        "ishSession, state S96)", TestManagerHelpers.Describe(temp93)));
            this.Manager.Comment("reaching state \'S114\'");
            FRS2Model.error_status_t temp94;
            this.Manager.Comment("executing step \'call AsyncPoll(2)\'");
            temp94 = this.IFRS2ManagedAdapterInstance.AsyncPoll(2);
            this.Manager.Checkpoint("MS-FRS2_R550");
            this.Manager.Checkpoint("MS-FRS2_R554");
            this.Manager.Comment("reaching state \'S131\'");
            this.Manager.Comment("checking step \'return AsyncPoll/ERROR_FAIL\'");
            this.Manager.Assert((temp94 == FRS2Model.error_status_t.ERROR_FAIL), String.Format("expected \'FRS2Model.error_status_t.ERROR_FAIL\', actual \'{0}\' (return of AsyncPoll" +
                        ", state S131)", TestManagerHelpers.Describe(temp94)));
            this.Manager.Comment("reaching state \'S147\'");
            FRS2Model.error_status_t temp95;
            this.Manager.Comment("executing step \'call RequestVersionVector(1,1,5,REQUEST_SLAVE_SYNC,CHANGE_ALL,Val" +
                    "idValue)\'");
            temp95 = this.IFRS2ManagedAdapterInstance.RequestVersionVector(1, 1, 5, FRS2Model.VERSION_REQUEST_TYPE.REQUEST_SLAVE_SYNC, FRS2Model.VERSION_CHANGE_TYPE.CHANGE_ALL, FRS2Model.VVGeneration.ValidValue);
            this.Manager.Checkpoint("MS-FRS2_R518");
            this.Manager.Checkpoint("MS-FRS2_R523");
            this.Manager.Checkpoint("MS-FRS2_R529");
            this.Manager.AddReturn(RequestVersionVectorInfo, null, temp95);
            TCtestScenario1S140();
            this.Manager.EndTest();
        }

        private void TCtestScenario1S140()
        {
            this.Manager.Comment("reaching state \'S140\'");
            this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(TCtestScenario1.RequestVersionVectorInfo, null, new RequestVersionVectorDelegate1(this.TCtestScenario1S12RequestVersionVectorChecker)));
            TCtestScenario1S152();
        }

        private void TCtestScenario1S12RequestVersionVectorChecker(FRS2Model.error_status_t @return)
        {
            this.Manager.Comment("checking step \'return RequestVersionVector/ERROR_FAIL\'");
            this.Manager.Assert((@return == FRS2Model.error_status_t.ERROR_FAIL), String.Format("expected \'FRS2Model.error_status_t.ERROR_FAIL\', actual \'{0}\' (return of RequestVe" +
                        "rsionVector, state S140)", TestManagerHelpers.Describe(@return)));
        }
        #endregion

        #region Test Starting in S14
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
        public virtual void FRS2_TCtestScenario1S14()
        {
            this.Manager.BeginTest("TCtestScenario1S14");
            this.Manager.Comment("reaching state \'S14\'");
            FRS2Model.error_status_t temp96;
            this.Manager.Comment(@"executing step 'call Initialization(Windows7,Map{1=>enabled,2=>enabled,3=>enabled,4=>disabled},Map{1=>exists,2=>exists,3=>exists,4=>exists},Map{""A""=>1,""B""=>2,""C""=>3,""D""=>4},Map{""A""=>sysvol,""B""=>protection,""C""=>sysvol,""D""=>sysvol},Map{1=>""A"",2=>""B"",3=>""C"",4=>""D""},Map{1=>writeOnly,2=>readWrite,3=>readOnly,4=>readWrite},Map{1=>enabled,2=>disabled,3=>enabled,4=>enabled})'");
            temp96 = this.IFRS2ManagedAdapterInstance.Initialization(FRS2Model.OSVersion.Windows7, new Microsoft.Modeling.Map<System.Int32, FRS2Model.connectionProperty>().Add(1, FRS2Model.connectionProperty.enabled).Add(2, FRS2Model.connectionProperty.enabled).Add(3, FRS2Model.connectionProperty.enabled).Add(4, FRS2Model.connectionProperty.disabled), new Microsoft.Modeling.Map<System.Int32, FRS2Model.FromServer>().Add(1, FRS2Model.FromServer.exists).Add(2, FRS2Model.FromServer.exists).Add(3, FRS2Model.FromServer.exists).Add(4, FRS2Model.FromServer.exists), new Microsoft.Modeling.Map<System.String, System.Int32>().Add("A", 1).Add("B", 2).Add("C", 3).Add("D", 4), new Microsoft.Modeling.Map<System.String, FRS2Model.ReplicationGroupTypes>().Add("A", FRS2Model.ReplicationGroupTypes.sysvol).Add("B", FRS2Model.ReplicationGroupTypes.protection).Add("C", FRS2Model.ReplicationGroupTypes.sysvol).Add("D", FRS2Model.ReplicationGroupTypes.sysvol), new Microsoft.Modeling.Map<System.Int32, System.String>().Add(1, "A").Add(2, "B").Add(3, "C").Add(4, "D"), new Microsoft.Modeling.Map<System.Int32, FRS2Model.accessLevels>().Add(1, FRS2Model.accessLevels.writeOnly).Add(2, FRS2Model.accessLevels.readWrite).Add(3, FRS2Model.accessLevels.readOnly).Add(4, FRS2Model.accessLevels.readWrite), new Microsoft.Modeling.Map<System.Int32, FRS2Model.connectionProperty>().Add(1, FRS2Model.connectionProperty.enabled).Add(2, FRS2Model.connectionProperty.disabled).Add(3, FRS2Model.connectionProperty.enabled).Add(4, FRS2Model.connectionProperty.enabled));
            this.Manager.Comment("reaching state \'S15\'");
            this.Manager.Comment("checking step \'return Initialization/ERROR_SUCCESS\'");
            this.Manager.Assert((temp96 == FRS2Model.error_status_t.ERROR_SUCCESS), String.Format("expected \'FRS2Model.error_status_t.ERROR_SUCCESS\', actual \'{0}\' (return of Initia" +
                        "lization, state S15)", TestManagerHelpers.Describe(temp96)));
            this.Manager.Comment("reaching state \'S43\'");
            FRS2Model.ProtocolVersionReturned temp97;
            FRS2Model.UpstreamFlagValueReturned temp98;
            FRS2Model.error_status_t temp99;
            this.Manager.Comment("executing step \'call EstablishConnection(\"A\",1,FRS_COMMUNICATION_PROTOCOL_VERSION" +
                    "_LONGHORN_SERVER,out _,out _)\'");
            temp99 = this.IFRS2ManagedAdapterInstance.EstablishConnection("A", 1, FRS2Model.ProtocolVersion.FRS_COMMUNICATION_PROTOCOL_VERSION_LONGHORN_SERVER, out temp97, out temp98);
            this.Manager.Checkpoint("MS-FRS2_R37");
            this.Manager.Checkpoint("MS-FRS2_R436");
            this.Manager.Checkpoint("MS-FRS2_R439");
            this.Manager.Comment("reaching state \'S61\'");
            this.Manager.Comment("checking step \'return EstablishConnection/[out Valid,out Valid]:ERROR_SUCCESS\'");
            this.Manager.Assert((temp97 == FRS2Model.ProtocolVersionReturned.Valid), String.Format("expected \'FRS2Model.ProtocolVersionReturned.Valid\', actual \'{0}\' (upstreamProtoco" +
                        "lVersion of EstablishConnection, state S61)", TestManagerHelpers.Describe(temp97)));
            this.Manager.Assert((temp98 == FRS2Model.UpstreamFlagValueReturned.Valid), String.Format("expected \'FRS2Model.UpstreamFlagValueReturned.Valid\', actual \'{0}\' (upstreamFlags" +
                        " of EstablishConnection, state S61)", TestManagerHelpers.Describe(temp98)));
            this.Manager.Assert((temp99 == FRS2Model.error_status_t.ERROR_SUCCESS), String.Format("expected \'FRS2Model.error_status_t.ERROR_SUCCESS\', actual \'{0}\' (return of Establ" +
                        "ishConnection, state S61)", TestManagerHelpers.Describe(temp99)));
            this.Manager.Comment("reaching state \'S79\'");
            FRS2Model.error_status_t temp100;
            this.Manager.Comment("executing step \'call EstablishSession(1,1)\'");
            temp100 = this.IFRS2ManagedAdapterInstance.EstablishSession(1, 1);
            this.Manager.Checkpoint("MS-FRS2_R455");
            this.Manager.Checkpoint("MS-FRS2_R458");
            this.Manager.Checkpoint("MS-FRS2_R448");
            this.Manager.Comment("reaching state \'S97\'");
            this.Manager.Comment("checking step \'return EstablishSession/ERROR_SUCCESS\'");
            this.Manager.Assert((temp100 == FRS2Model.error_status_t.ERROR_SUCCESS), String.Format("expected \'FRS2Model.error_status_t.ERROR_SUCCESS\', actual \'{0}\' (return of Establ" +
                        "ishSession, state S97)", TestManagerHelpers.Describe(temp100)));
            this.Manager.Comment("reaching state \'S115\'");
            FRS2Model.error_status_t temp101;
            this.Manager.Comment("executing step \'call AsyncPoll(1)\'");
            temp101 = this.IFRS2ManagedAdapterInstance.AsyncPoll(1);
            this.Manager.Checkpoint("MS-FRS2_R549");
            this.Manager.Checkpoint("MS-FRS2_R552");
            this.Manager.Comment("reaching state \'S132\'");
            this.Manager.Comment("checking step \'return AsyncPoll/ERROR_SUCCESS\'");
            this.Manager.Assert((temp101 == FRS2Model.error_status_t.ERROR_SUCCESS), String.Format("expected \'FRS2Model.error_status_t.ERROR_SUCCESS\', actual \'{0}\' (return of AsyncP" +
                        "oll, state S132)", TestManagerHelpers.Describe(temp101)));
            this.Manager.Comment("reaching state \'S148\'");
            FRS2Model.error_status_t temp102;
            this.Manager.Comment("executing step \'call RequestVersionVector(1,4,5,REQUEST_NORMAL_SYNC,CHANGE_ALL,In" +
                    "validValue)\'");
            temp102 = this.IFRS2ManagedAdapterInstance.RequestVersionVector(1, 4, 5, FRS2Model.VERSION_REQUEST_TYPE.REQUEST_NORMAL_SYNC, FRS2Model.VERSION_CHANGE_TYPE.CHANGE_ALL, FRS2Model.VVGeneration.InvalidValue);
            this.Manager.Checkpoint("MS-FRS2_R518");
            this.Manager.Checkpoint("MS-FRS2_R522");
            this.Manager.Checkpoint("MS-FRS2_R523");
            this.Manager.Checkpoint("MS-FRS2_R529");
            this.Manager.Comment("reaching state \'S164\'");
            this.Manager.Comment("checking step \'return RequestVersionVector/ERROR_FAIL\'");
            this.Manager.Assert((temp102 == FRS2Model.error_status_t.ERROR_FAIL), String.Format("expected \'FRS2Model.error_status_t.ERROR_FAIL\', actual \'{0}\' (return of RequestVe" +
                        "rsionVector, state S164)", TestManagerHelpers.Describe(temp102)));
            TCtestScenario1S173();
            this.Manager.EndTest();
        }

        private void TCtestScenario1S173()
        {
            this.Manager.Comment("reaching state \'S173\'");
        }
        #endregion

        #region Test Starting in S16
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        [Microsoft.VisualStudio.TestTools.UnitTesting.DescriptionAttribute(@"MS-FRS2_R37, MS-FRS2_R436, MS-FRS2_R439, MS-FRS2_R455, MS-FRS2_R458, MS-FRS2_R448, MS-FRS2_R549, MS-FRS2_R552, MS-FRS2_R517, MS-FRS2_R520, MS-FRS2_R466, MS-FRS2_R556, MS-FRS2_R1020, MS-FRS2_R555, MS-FRS2_R486, MS-FRS2_R489, MS-FRS2_R93, MS-FRS2_R498, MS-FRS2_R775, MS-FRS2_R779, MS-FRS2_R784, MS-FRS2_R93, MS-FRS2_R94, MS-FRS2_R775, MS-FRS2_R779, MS-FRS2_R784, MS-FRS2_R93, MS-FRS2_R94, MS-FRS2_R775, MS-FRS2_R779, MS-FRS2_R784, MS-FRS2_R93, MS-FRS2_R775, MS-FRS2_R779, MS-FRS2_R784, MS-FRS2_R556, MS-FRS2_R1020, MS-FRS2_R555")]
        public virtual void FRS2_TCtestScenario1S16()
        {
            this.Manager.BeginTest("TCtestScenario1S16");
            this.Manager.Comment("reaching state \'S16\'");
            FRS2Model.error_status_t temp103;
            this.Manager.Comment(@"executing step 'call Initialization(Windows7,Map{1=>enabled,2=>enabled,3=>enabled,4=>disabled},Map{1=>exists,2=>exists,3=>exists,4=>exists},Map{""A""=>1,""B""=>2,""C""=>3,""D""=>4},Map{""A""=>sysvol,""B""=>protection,""C""=>sysvol,""D""=>sysvol},Map{1=>""A"",2=>""B"",3=>""C"",4=>""D""},Map{1=>writeOnly,2=>readWrite,3=>readOnly,4=>readWrite},Map{1=>enabled,2=>disabled,3=>enabled,4=>enabled})'");
            temp103 = this.IFRS2ManagedAdapterInstance.Initialization(FRS2Model.OSVersion.Windows7, new Microsoft.Modeling.Map<System.Int32, FRS2Model.connectionProperty>().Add(1, FRS2Model.connectionProperty.enabled).Add(2, FRS2Model.connectionProperty.enabled).Add(3, FRS2Model.connectionProperty.enabled).Add(4, FRS2Model.connectionProperty.disabled), new Microsoft.Modeling.Map<System.Int32, FRS2Model.FromServer>().Add(1, FRS2Model.FromServer.exists).Add(2, FRS2Model.FromServer.exists).Add(3, FRS2Model.FromServer.exists).Add(4, FRS2Model.FromServer.exists), new Microsoft.Modeling.Map<System.String, System.Int32>().Add("A", 1).Add("B", 2).Add("C", 3).Add("D", 4), new Microsoft.Modeling.Map<System.String, FRS2Model.ReplicationGroupTypes>().Add("A", FRS2Model.ReplicationGroupTypes.sysvol).Add("B", FRS2Model.ReplicationGroupTypes.protection).Add("C", FRS2Model.ReplicationGroupTypes.sysvol).Add("D", FRS2Model.ReplicationGroupTypes.sysvol), new Microsoft.Modeling.Map<System.Int32, System.String>().Add(1, "A").Add(2, "B").Add(3, "C").Add(4, "D"), new Microsoft.Modeling.Map<System.Int32, FRS2Model.accessLevels>().Add(1, FRS2Model.accessLevels.writeOnly).Add(2, FRS2Model.accessLevels.readWrite).Add(3, FRS2Model.accessLevels.readOnly).Add(4, FRS2Model.accessLevels.readWrite), new Microsoft.Modeling.Map<System.Int32, FRS2Model.connectionProperty>().Add(1, FRS2Model.connectionProperty.enabled).Add(2, FRS2Model.connectionProperty.disabled).Add(3, FRS2Model.connectionProperty.enabled).Add(4, FRS2Model.connectionProperty.enabled));
            this.Manager.Comment("reaching state \'S17\'");
            this.Manager.Comment("checking step \'return Initialization/ERROR_SUCCESS\'");
            this.Manager.Assert((temp103 == FRS2Model.error_status_t.ERROR_SUCCESS), String.Format("expected \'FRS2Model.error_status_t.ERROR_SUCCESS\', actual \'{0}\' (return of Initia" +
                        "lization, state S17)", TestManagerHelpers.Describe(temp103)));
            this.Manager.Comment("reaching state \'S44\'");
            FRS2Model.ProtocolVersionReturned temp104;
            FRS2Model.UpstreamFlagValueReturned temp105;
            FRS2Model.error_status_t temp106;
            this.Manager.Comment("executing step \'call EstablishConnection(\"A\",1,FRS_COMMUNICATION_PROTOCOL_VERSION" +
                    "_LONGHORN_SERVER,out _,out _)\'");
            temp106 = this.IFRS2ManagedAdapterInstance.EstablishConnection("A", 1, FRS2Model.ProtocolVersion.FRS_COMMUNICATION_PROTOCOL_VERSION_LONGHORN_SERVER, out temp104, out temp105);
            this.Manager.Checkpoint("MS-FRS2_R37");
            this.Manager.Checkpoint("MS-FRS2_R436");
            this.Manager.Checkpoint("MS-FRS2_R439");
            this.Manager.Comment("reaching state \'S62\'");
            this.Manager.Comment("checking step \'return EstablishConnection/[out Valid,out Valid]:ERROR_SUCCESS\'");
            this.Manager.Assert((temp104 == FRS2Model.ProtocolVersionReturned.Valid), String.Format("expected \'FRS2Model.ProtocolVersionReturned.Valid\', actual \'{0}\' (upstreamProtoco" +
                        "lVersion of EstablishConnection, state S62)", TestManagerHelpers.Describe(temp104)));
            this.Manager.Assert((temp105 == FRS2Model.UpstreamFlagValueReturned.Valid), String.Format("expected \'FRS2Model.UpstreamFlagValueReturned.Valid\', actual \'{0}\' (upstreamFlags" +
                        " of EstablishConnection, state S62)", TestManagerHelpers.Describe(temp105)));
            this.Manager.Assert((temp106 == FRS2Model.error_status_t.ERROR_SUCCESS), String.Format("expected \'FRS2Model.error_status_t.ERROR_SUCCESS\', actual \'{0}\' (return of Establ" +
                        "ishConnection, state S62)", TestManagerHelpers.Describe(temp106)));
            this.Manager.Comment("reaching state \'S80\'");
            FRS2Model.error_status_t temp107;
            this.Manager.Comment("executing step \'call EstablishSession(1,1)\'");
            temp107 = this.IFRS2ManagedAdapterInstance.EstablishSession(1, 1);
            this.Manager.Checkpoint("MS-FRS2_R455");
            this.Manager.Checkpoint("MS-FRS2_R458");
            this.Manager.Checkpoint("MS-FRS2_R448");
            this.Manager.Comment("reaching state \'S98\'");
            this.Manager.Comment("checking step \'return EstablishSession/ERROR_SUCCESS\'");
            this.Manager.Assert((temp107 == FRS2Model.error_status_t.ERROR_SUCCESS), String.Format("expected \'FRS2Model.error_status_t.ERROR_SUCCESS\', actual \'{0}\' (return of Establ" +
                        "ishSession, state S98)", TestManagerHelpers.Describe(temp107)));
            this.Manager.Comment("reaching state \'S116\'");
            FRS2Model.error_status_t temp108;
            this.Manager.Comment("executing step \'call AsyncPoll(1)\'");
            temp108 = this.IFRS2ManagedAdapterInstance.AsyncPoll(1);
            this.Manager.Checkpoint("MS-FRS2_R549");
            this.Manager.Checkpoint("MS-FRS2_R552");
            this.Manager.Comment("reaching state \'S133\'");
            this.Manager.Comment("checking step \'return AsyncPoll/ERROR_SUCCESS\'");
            this.Manager.Assert((temp108 == FRS2Model.error_status_t.ERROR_SUCCESS), String.Format("expected \'FRS2Model.error_status_t.ERROR_SUCCESS\', actual \'{0}\' (return of AsyncP" +
                        "oll, state S133)", TestManagerHelpers.Describe(temp108)));
            this.Manager.Comment("reaching state \'S149\'");
            FRS2Model.error_status_t temp109;
            this.Manager.Comment("executing step \'call RequestVersionVector(1,1,1,REQUEST_NORMAL_SYNC,CHANGE_NOTIFY" +
                    ",ValidValue)\'");
            temp109 = this.IFRS2ManagedAdapterInstance.RequestVersionVector(1, 1, 1, FRS2Model.VERSION_REQUEST_TYPE.REQUEST_NORMAL_SYNC, FRS2Model.VERSION_CHANGE_TYPE.CHANGE_NOTIFY, FRS2Model.VVGeneration.ValidValue);
            this.Manager.Checkpoint("MS-FRS2_R517");
            this.Manager.Checkpoint("MS-FRS2_R520");
            this.Manager.Checkpoint("MS-FRS2_R466");
            this.Manager.Comment("reaching state \'S165\'");
            this.Manager.Comment("checking step \'return RequestVersionVector/ERROR_SUCCESS\'");
            this.Manager.Assert((temp109 == FRS2Model.error_status_t.ERROR_SUCCESS), String.Format("expected \'FRS2Model.error_status_t.ERROR_SUCCESS\', actual \'{0}\' (return of Reques" +
                        "tVersionVector, state S165)", TestManagerHelpers.Describe(temp109)));
            this.Manager.Comment("reaching state \'S174\'");
            int temp116 = this.Manager.ExpectEvent(this.QuiescenceTimeout, true, new ExpectedEvent(TCtestScenario1.AsyncPollResponseEventInfo, null, new AsyncPollResponseEventDelegate1(this.TCtestScenario1S16AsyncPollResponseEventChecker)), new ExpectedEvent(TCtestScenario1.AsyncPollResponseEventInfo, null, new AsyncPollResponseEventDelegate1(this.TCtestScenario1S16AsyncPollResponseEventChecker1)));
            if ((temp116 == 0))
            {
                this.Manager.Comment("reaching state \'S184\'");
                FRS2Model.error_status_t temp110;
                this.Manager.Comment("executing step \'call RequestUpdates(1,1,valid)\'");
                temp110 = this.IFRS2ManagedAdapterInstance.RequestUpdates(1, 1, FRS2Model.versionVectorDiff.valid);
                this.Manager.Checkpoint("MS-FRS2_R486");
                this.Manager.Checkpoint("MS-FRS2_R489");
                this.Manager.Comment("reaching state \'S189\'");
                this.Manager.Comment("checking step \'return RequestUpdates/ERROR_SUCCESS\'");
                this.Manager.Assert((temp110 == FRS2Model.error_status_t.ERROR_SUCCESS), String.Format("expected \'FRS2Model.error_status_t.ERROR_SUCCESS\', actual \'{0}\' (return of Reques" +
                            "tUpdates, state S189)", TestManagerHelpers.Describe(temp110)));
                this.Manager.Comment("reaching state \'S193\'");
                int temp115 = this.Manager.ExpectEvent(this.QuiescenceTimeout, true, new ExpectedEvent(TCtestScenario1.RequestUpdatesEventInfo, null, new RequestUpdatesEventDelegate1(this.TCtestScenario1S16RequestUpdatesEventChecker)), new ExpectedEvent(TCtestScenario1.RequestUpdatesEventInfo, null, new RequestUpdatesEventDelegate1(this.TCtestScenario1S16RequestUpdatesEventChecker1)), new ExpectedEvent(TCtestScenario1.RequestUpdatesEventInfo, null, new RequestUpdatesEventDelegate1(this.TCtestScenario1S16RequestUpdatesEventChecker2)), new ExpectedEvent(TCtestScenario1.RequestUpdatesEventInfo, null, new RequestUpdatesEventDelegate1(this.TCtestScenario1S16RequestUpdatesEventChecker3)));
                if ((temp115 == 0))
                {
                    this.Manager.Comment("reaching state \'S202\'");
                    FRS2Model.error_status_t temp111;
                    this.Manager.Comment("executing step \'call InitializeFileTransferAsync(1,5,False)\'");
                    temp111 = this.IFRS2ManagedAdapterInstance.InitializeFileTransferAsync(1, 5, false);
                    this.Manager.Checkpoint("MS-FRS2_R775");
                    this.Manager.Checkpoint("MS-FRS2_R779");
                    this.Manager.Checkpoint("MS-FRS2_R784");
                    this.Manager.Comment("reaching state \'S214\'");
                    this.Manager.Comment("checking step \'return InitializeFileTransferAsync/FRS_ERROR_CONTENTSET_NOT_FOUND\'" +
                            "");
                    this.Manager.Assert((temp111 == FRS2Model.error_status_t.FRS_ERROR_CONTENTSET_NOT_FOUND), String.Format("expected \'FRS2Model.error_status_t.FRS_ERROR_CONTENTSET_NOT_FOUND\', actual \'{0}\' " +
                                "(return of InitializeFileTransferAsync, state S214)", TestManagerHelpers.Describe(temp111)));
                    TCtestScenario1S221();
                    goto label11;
                }
                if ((temp115 == 1))
                {
                    this.Manager.Comment("reaching state \'S203\'");
                    FRS2Model.error_status_t temp112;
                    this.Manager.Comment("executing step \'call InitializeFileTransferAsync(1,5,False)\'");
                    temp112 = this.IFRS2ManagedAdapterInstance.InitializeFileTransferAsync(1, 5, false);
                    this.Manager.Checkpoint("MS-FRS2_R775");
                    this.Manager.Checkpoint("MS-FRS2_R779");
                    this.Manager.Checkpoint("MS-FRS2_R784");
                    this.Manager.Comment("reaching state \'S215\'");
                    this.Manager.Comment("checking step \'return InitializeFileTransferAsync/FRS_ERROR_CONTENTSET_NOT_FOUND\'" +
                            "");
                    this.Manager.Assert((temp112 == FRS2Model.error_status_t.FRS_ERROR_CONTENTSET_NOT_FOUND), String.Format("expected \'FRS2Model.error_status_t.FRS_ERROR_CONTENTSET_NOT_FOUND\', actual \'{0}\' " +
                                "(return of InitializeFileTransferAsync, state S215)", TestManagerHelpers.Describe(temp112)));
                    TCtestScenario1S220();
                    goto label11;
                }
                if ((temp115 == 2))
                {
                    this.Manager.Comment("reaching state \'S204\'");
                    FRS2Model.error_status_t temp113;
                    this.Manager.Comment("executing step \'call InitializeFileTransferAsync(1,5,False)\'");
                    temp113 = this.IFRS2ManagedAdapterInstance.InitializeFileTransferAsync(1, 5, false);
                    this.Manager.Checkpoint("MS-FRS2_R775");
                    this.Manager.Checkpoint("MS-FRS2_R779");
                    this.Manager.Checkpoint("MS-FRS2_R784");
                    this.Manager.AddReturn(InitializeFileTransferAsyncInfo, null, temp113);
                    TCtestScenario1S216();
                    goto label11;
                }
                if ((temp115 == 3))
                {
                    this.Manager.Comment("reaching state \'S205\'");
                    FRS2Model.error_status_t temp114;
                    this.Manager.Comment("executing step \'call InitializeFileTransferAsync(1,5,False)\'");
                    temp114 = this.IFRS2ManagedAdapterInstance.InitializeFileTransferAsync(1, 5, false);
                    this.Manager.Checkpoint("MS-FRS2_R775");
                    this.Manager.Checkpoint("MS-FRS2_R779");
                    this.Manager.Checkpoint("MS-FRS2_R784");
                    this.Manager.AddReturn(InitializeFileTransferAsyncInfo, null, temp114);
                    TCtestScenario1S216();
                    goto label11;
                }
                throw new InvalidOperationException("never reached");
            label11:
                ;
                goto label12;
            }
            if ((temp116 == 1))
            {
                TCtestScenario1S179();
                goto label12;
            }
            throw new InvalidOperationException("never reached");
        label12:
            ;
            this.Manager.EndTest();
        }

        private void TCtestScenario1S16AsyncPollResponseEventChecker(FRS2Model.VVGeneration vvGen)
        {
            this.Manager.Comment("checking step \'event AsyncPollResponseEvent(ValidValue)\'");
            try
            {
                this.Manager.Assert((vvGen == FRS2Model.VVGeneration.ValidValue), String.Format("expected \'FRS2Model.VVGeneration.ValidValue\', actual \'{0}\' (vvGen of AsyncPollRes" +
                            "ponseEvent, state S174)", TestManagerHelpers.Describe(vvGen)));
            }
            catch (TransactionFailedException)
            {
                this.Manager.Comment("This step would have covered MS-FRS2_R556, MS-FRS2_R1020, MS-FRS2_R555");
                throw;
            }
            this.Manager.Checkpoint("MS-FRS2_R556");
            this.Manager.Checkpoint("MS-FRS2_R1020");
            this.Manager.Checkpoint("MS-FRS2_R555");
        }

        private void TCtestScenario1S16RequestUpdatesEventChecker(FRS2Model.FilePresense present, FRS2Model.UPDATE_STATUS updateStatus)
        {
            this.Manager.Comment("checking step \'event RequestUpdatesEvent(fileExists,UPDATE_STATUS_MORE)\'");
            try
            {
                this.Manager.Assert((present == FRS2Model.FilePresense.fileExists), String.Format("expected \'FRS2Model.FilePresense.fileExists\', actual \'{0}\' (present of RequestUpd" +
                            "atesEvent, state S193)", TestManagerHelpers.Describe(present)));
                this.Manager.Assert((updateStatus == FRS2Model.UPDATE_STATUS.UPDATE_STATUS_MORE), String.Format("expected \'FRS2Model.UPDATE_STATUS.UPDATE_STATUS_MORE\', actual \'{0}\' (updateStatus" +
                            " of RequestUpdatesEvent, state S193)", TestManagerHelpers.Describe(updateStatus)));
            }
            catch (TransactionFailedException)
            {
                this.Manager.Comment("This step would have covered MS-FRS2_R93, MS-FRS2_R498");
                throw;
            }
            this.Manager.Checkpoint("MS-FRS2_R93");
            this.Manager.Checkpoint("MS-FRS2_R498");
        }

        private void TCtestScenario1S16RequestUpdatesEventChecker1(FRS2Model.FilePresense present, FRS2Model.UPDATE_STATUS updateStatus)
        {
            this.Manager.Comment("checking step \'event RequestUpdatesEvent(fileExists,UPDATE_STATUS_DONE)\'");
            try
            {
                this.Manager.Assert((present == FRS2Model.FilePresense.fileExists), String.Format("expected \'FRS2Model.FilePresense.fileExists\', actual \'{0}\' (present of RequestUpd" +
                            "atesEvent, state S193)", TestManagerHelpers.Describe(present)));
                this.Manager.Assert((updateStatus == FRS2Model.UPDATE_STATUS.UPDATE_STATUS_DONE), String.Format("expected \'FRS2Model.UPDATE_STATUS.UPDATE_STATUS_DONE\', actual \'{0}\' (updateStatus" +
                            " of RequestUpdatesEvent, state S193)", TestManagerHelpers.Describe(updateStatus)));
            }
            catch (TransactionFailedException)
            {
                this.Manager.Comment("This step would have covered MS-FRS2_R93, MS-FRS2_R94");
                throw;
            }
            this.Manager.Checkpoint("MS-FRS2_R93");
            this.Manager.Checkpoint("MS-FRS2_R94");
        }

        private void TCtestScenario1S16RequestUpdatesEventChecker2(FRS2Model.FilePresense present, FRS2Model.UPDATE_STATUS updateStatus)
        {
            this.Manager.Comment("checking step \'event RequestUpdatesEvent(fileDeleted,UPDATE_STATUS_DONE)\'");
            try
            {
                this.Manager.Assert((present == FRS2Model.FilePresense.fileDeleted), String.Format("expected \'FRS2Model.FilePresense.fileDeleted\', actual \'{0}\' (present of RequestUp" +
                            "datesEvent, state S193)", TestManagerHelpers.Describe(present)));
                this.Manager.Assert((updateStatus == FRS2Model.UPDATE_STATUS.UPDATE_STATUS_DONE), String.Format("expected \'FRS2Model.UPDATE_STATUS.UPDATE_STATUS_DONE\', actual \'{0}\' (updateStatus" +
                            " of RequestUpdatesEvent, state S193)", TestManagerHelpers.Describe(updateStatus)));
            }
            catch (TransactionFailedException)
            {
                this.Manager.Comment("This step would have covered MS-FRS2_R93, MS-FRS2_R94");
                throw;
            }
            this.Manager.Checkpoint("MS-FRS2_R93");
            this.Manager.Checkpoint("MS-FRS2_R94");
        }

        private void TCtestScenario1S216()
        {
            this.Manager.Comment("reaching state \'S216\'");
            this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(TCtestScenario1.InitializeFileTransferAsyncInfo, null, new InitializeFileTransferAsyncDelegate1(this.TCtestScenario1S16InitializeFileTransferAsyncChecker)));
            TCtestScenario1S219();
        }

        private void TCtestScenario1S16InitializeFileTransferAsyncChecker(FRS2Model.error_status_t @return)
        {
            this.Manager.Comment("checking step \'return InitializeFileTransferAsync/FRS_ERROR_CONTENTSET_NOT_FOUND\'" +
                    "");
            this.Manager.Assert((@return == FRS2Model.error_status_t.FRS_ERROR_CONTENTSET_NOT_FOUND), String.Format("expected \'FRS2Model.error_status_t.FRS_ERROR_CONTENTSET_NOT_FOUND\', actual \'{0}\' " +
                        "(return of InitializeFileTransferAsync, state S216)", TestManagerHelpers.Describe(@return)));
        }

        private void TCtestScenario1S16RequestUpdatesEventChecker3(FRS2Model.FilePresense present, FRS2Model.UPDATE_STATUS updateStatus)
        {
            this.Manager.Comment("checking step \'event RequestUpdatesEvent(fileDeleted,UPDATE_STATUS_MORE)\'");
            try
            {
                this.Manager.Assert((present == FRS2Model.FilePresense.fileDeleted), String.Format("expected \'FRS2Model.FilePresense.fileDeleted\', actual \'{0}\' (present of RequestUp" +
                            "datesEvent, state S193)", TestManagerHelpers.Describe(present)));
                this.Manager.Assert((updateStatus == FRS2Model.UPDATE_STATUS.UPDATE_STATUS_MORE), String.Format("expected \'FRS2Model.UPDATE_STATUS.UPDATE_STATUS_MORE\', actual \'{0}\' (updateStatus" +
                            " of RequestUpdatesEvent, state S193)", TestManagerHelpers.Describe(updateStatus)));
            }
            catch (TransactionFailedException)
            {
                this.Manager.Comment("This step would have covered MS-FRS2_R93");
                throw;
            }
            this.Manager.Checkpoint("MS-FRS2_R93");
        }

        private void TCtestScenario1S16AsyncPollResponseEventChecker1(FRS2Model.VVGeneration vvGen)
        {
            this.Manager.Comment("checking step \'event AsyncPollResponseEvent(InvalidValue)\'");
            try
            {
                this.Manager.Assert((vvGen == FRS2Model.VVGeneration.InvalidValue), String.Format("expected \'FRS2Model.VVGeneration.InvalidValue\', actual \'{0}\' (vvGen of AsyncPollR" +
                            "esponseEvent, state S174)", TestManagerHelpers.Describe(vvGen)));
            }
            catch (TransactionFailedException)
            {
                this.Manager.Comment("This step would have covered MS-FRS2_R556, MS-FRS2_R1020, MS-FRS2_R555");
                throw;
            }
            this.Manager.Checkpoint("MS-FRS2_R556");
            this.Manager.Checkpoint("MS-FRS2_R1020");
            this.Manager.Checkpoint("MS-FRS2_R555");
        }
        #endregion

        #region Test Starting in S18
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        [Microsoft.VisualStudio.TestTools.UnitTesting.DescriptionAttribute("MS-FRS2_R37, MS-FRS2_R436, MS-FRS2_R439, MS-FRS2_R455, MS-FRS2_R458, MS-FRS2_R448" +
            ", MS-FRS2_R549, MS-FRS2_R552, MS-FRS2_R518, MS-FRS2_R522, MS-FRS2_R525, MS-FRS2_" +
            "R527, MS-FRS2_R530")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestCategory("AD")]
        [TestCategory("MS-FRS2")]
        [TestCategory("SDC")]
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]      
        public virtual void FRS2_TCtestScenario1S18()
        {
            this.Manager.BeginTest("TCtestScenario1S18");
            this.Manager.Comment("reaching state \'S18\'");
            FRS2Model.error_status_t temp117;
            this.Manager.Comment(@"executing step 'call Initialization(Windows7,Map{1=>enabled,2=>enabled,3=>enabled,4=>disabled},Map{1=>exists,2=>exists,3=>exists,4=>exists},Map{""A""=>1,""B""=>2,""C""=>3,""D""=>4},Map{""A""=>sysvol,""B""=>protection,""C""=>sysvol,""D""=>sysvol},Map{1=>""A"",2=>""B"",3=>""C"",4=>""D""},Map{1=>writeOnly,2=>readWrite,3=>readOnly,4=>readWrite},Map{1=>enabled,2=>disabled,3=>enabled,4=>enabled})'");
            temp117 = this.IFRS2ManagedAdapterInstance.Initialization(FRS2Model.OSVersion.Windows7, new Microsoft.Modeling.Map<System.Int32, FRS2Model.connectionProperty>().Add(1, FRS2Model.connectionProperty.enabled).Add(2, FRS2Model.connectionProperty.enabled).Add(3, FRS2Model.connectionProperty.enabled).Add(4, FRS2Model.connectionProperty.disabled), new Microsoft.Modeling.Map<System.Int32, FRS2Model.FromServer>().Add(1, FRS2Model.FromServer.exists).Add(2, FRS2Model.FromServer.exists).Add(3, FRS2Model.FromServer.exists).Add(4, FRS2Model.FromServer.exists), new Microsoft.Modeling.Map<System.String, System.Int32>().Add("A", 1).Add("B", 2).Add("C", 3).Add("D", 4), new Microsoft.Modeling.Map<System.String, FRS2Model.ReplicationGroupTypes>().Add("A", FRS2Model.ReplicationGroupTypes.sysvol).Add("B", FRS2Model.ReplicationGroupTypes.protection).Add("C", FRS2Model.ReplicationGroupTypes.sysvol).Add("D", FRS2Model.ReplicationGroupTypes.sysvol), new Microsoft.Modeling.Map<System.Int32, System.String>().Add(1, "A").Add(2, "B").Add(3, "C").Add(4, "D"), new Microsoft.Modeling.Map<System.Int32, FRS2Model.accessLevels>().Add(1, FRS2Model.accessLevels.writeOnly).Add(2, FRS2Model.accessLevels.readWrite).Add(3, FRS2Model.accessLevels.readOnly).Add(4, FRS2Model.accessLevels.readWrite), new Microsoft.Modeling.Map<System.Int32, FRS2Model.connectionProperty>().Add(1, FRS2Model.connectionProperty.enabled).Add(2, FRS2Model.connectionProperty.disabled).Add(3, FRS2Model.connectionProperty.enabled).Add(4, FRS2Model.connectionProperty.enabled));
            this.Manager.Comment("reaching state \'S19\'");
            this.Manager.Comment("checking step \'return Initialization/ERROR_SUCCESS\'");
            this.Manager.Assert((temp117 == FRS2Model.error_status_t.ERROR_SUCCESS), String.Format("expected \'FRS2Model.error_status_t.ERROR_SUCCESS\', actual \'{0}\' (return of Initia" +
                        "lization, state S19)", TestManagerHelpers.Describe(temp117)));
            this.Manager.Comment("reaching state \'S45\'");
            FRS2Model.ProtocolVersionReturned temp118;
            FRS2Model.UpstreamFlagValueReturned temp119;
            FRS2Model.error_status_t temp120;
            this.Manager.Comment("executing step \'call EstablishConnection(\"A\",1,FRS_COMMUNICATION_PROTOCOL_VERSION" +
                    "_LONGHORN_SERVER,out _,out _)\'");
            temp120 = this.IFRS2ManagedAdapterInstance.EstablishConnection("A", 1, FRS2Model.ProtocolVersion.FRS_COMMUNICATION_PROTOCOL_VERSION_LONGHORN_SERVER, out temp118, out temp119);
            this.Manager.Checkpoint("MS-FRS2_R37");
            this.Manager.Checkpoint("MS-FRS2_R436");
            this.Manager.Checkpoint("MS-FRS2_R439");
            this.Manager.Comment("reaching state \'S63\'");
            this.Manager.Comment("checking step \'return EstablishConnection/[out Valid,out Valid]:ERROR_SUCCESS\'");
            this.Manager.Assert((temp118 == FRS2Model.ProtocolVersionReturned.Valid), String.Format("expected \'FRS2Model.ProtocolVersionReturned.Valid\', actual \'{0}\' (upstreamProtoco" +
                        "lVersion of EstablishConnection, state S63)", TestManagerHelpers.Describe(temp118)));
            this.Manager.Assert((temp119 == FRS2Model.UpstreamFlagValueReturned.Valid), String.Format("expected \'FRS2Model.UpstreamFlagValueReturned.Valid\', actual \'{0}\' (upstreamFlags" +
                        " of EstablishConnection, state S63)", TestManagerHelpers.Describe(temp119)));
            this.Manager.Assert((temp120 == FRS2Model.error_status_t.ERROR_SUCCESS), String.Format("expected \'FRS2Model.error_status_t.ERROR_SUCCESS\', actual \'{0}\' (return of Establ" +
                        "ishConnection, state S63)", TestManagerHelpers.Describe(temp120)));
            this.Manager.Comment("reaching state \'S81\'");
            FRS2Model.error_status_t temp121;
            this.Manager.Comment("executing step \'call EstablishSession(1,1)\'");
            temp121 = this.IFRS2ManagedAdapterInstance.EstablishSession(1, 1);
            this.Manager.Checkpoint("MS-FRS2_R455");
            this.Manager.Checkpoint("MS-FRS2_R458");
            this.Manager.Checkpoint("MS-FRS2_R448");
            this.Manager.Comment("reaching state \'S99\'");
            this.Manager.Comment("checking step \'return EstablishSession/ERROR_SUCCESS\'");
            this.Manager.Assert((temp121 == FRS2Model.error_status_t.ERROR_SUCCESS), String.Format("expected \'FRS2Model.error_status_t.ERROR_SUCCESS\', actual \'{0}\' (return of Establ" +
                        "ishSession, state S99)", TestManagerHelpers.Describe(temp121)));
            this.Manager.Comment("reaching state \'S117\'");
            FRS2Model.error_status_t temp122;
            this.Manager.Comment("executing step \'call AsyncPoll(1)\'");
            temp122 = this.IFRS2ManagedAdapterInstance.AsyncPoll(1);
            this.Manager.Checkpoint("MS-FRS2_R549");
            this.Manager.Checkpoint("MS-FRS2_R552");
            this.Manager.Comment("reaching state \'S134\'");
            this.Manager.Comment("checking step \'return AsyncPoll/ERROR_SUCCESS\'");
            this.Manager.Assert((temp122 == FRS2Model.error_status_t.ERROR_SUCCESS), String.Format("expected \'FRS2Model.error_status_t.ERROR_SUCCESS\', actual \'{0}\' (return of AsyncP" +
                        "oll, state S134)", TestManagerHelpers.Describe(temp122)));
            this.Manager.Comment("reaching state \'S150\'");
            FRS2Model.error_status_t temp123;
            this.Manager.Comment("executing step \'call RequestVersionVector(1,4,1,REQUEST_SLAVE_SYNC,CHANGE_NOTIFY," +
                    "InvalidValue)\'");
            temp123 = this.IFRS2ManagedAdapterInstance.RequestVersionVector(1, 4, 1, FRS2Model.VERSION_REQUEST_TYPE.REQUEST_SLAVE_SYNC, FRS2Model.VERSION_CHANGE_TYPE.CHANGE_NOTIFY, FRS2Model.VVGeneration.InvalidValue);
            this.Manager.Checkpoint("MS-FRS2_R518");
            this.Manager.Checkpoint("MS-FRS2_R522");
            this.Manager.Checkpoint("MS-FRS2_R525");
            this.Manager.Checkpoint("MS-FRS2_R527");
            this.Manager.Checkpoint("MS-FRS2_R530");
            this.Manager.Comment("reaching state \'S166\'");
            this.Manager.Comment("checking step \'return RequestVersionVector/ERROR_FAIL\'");
            if (!Microsoft.Protocols.TestSuites.MS_FRS2.ConfigStore.IsTestSYSVOL)
                this.Manager.Assert((temp123 == FRS2Model.error_status_t.ERROR_FAIL), String.Format("expected \'FRS2Model.error_status_t.ERROR_FAIL\', actual \'{0}\' (return of RequestVe" +
                            "rsionVector, state S166)", TestManagerHelpers.Describe(temp123)));
            else
                this.Manager.Assert((temp123 == FRS2Model.error_status_t.ERROR_INVALID_PARAMETER), String.Format("expected \'FRS2Model.error_status_t.ERROR_INVALID_PARAMETER\', actual \'{0}\' (return of RequestVe" +
                        "rsionVector, state S166)", TestManagerHelpers.Describe(temp123)));
            TCtestScenario1S173();
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S20
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        [Microsoft.VisualStudio.TestTools.UnitTesting.DescriptionAttribute("MS-FRS2_R37, MS-FRS2_R436, MS-FRS2_R439, MS-FRS2_R455, MS-FRS2_R458, MS-FRS2_R448" +
            ", MS-FRS2_R549, MS-FRS2_R552, MS-FRS2_R518, MS-FRS2_R523, MS-FRS2_R529")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestCategory("AD")]
        [TestCategory("MS-FRS2")]
        [TestCategory("SDC")]
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]       
        public virtual void FRS2_TCtestScenario1S20()
        {
            this.Manager.BeginTest("TCtestScenario1S20");
            this.Manager.Comment("reaching state \'S20\'");
            FRS2Model.error_status_t temp124;
            this.Manager.Comment(@"executing step 'call Initialization(Windows7,Map{1=>enabled,2=>enabled,3=>enabled,4=>disabled},Map{1=>exists,2=>exists,3=>exists,4=>exists},Map{""A""=>1,""B""=>2,""C""=>3,""D""=>4},Map{""A""=>sysvol,""B""=>protection,""C""=>sysvol,""D""=>sysvol},Map{1=>""A"",2=>""B"",3=>""C"",4=>""D""},Map{1=>writeOnly,2=>readWrite,3=>readOnly,4=>readWrite},Map{1=>enabled,2=>disabled,3=>enabled,4=>enabled})'");
            temp124 = this.IFRS2ManagedAdapterInstance.Initialization(FRS2Model.OSVersion.Windows7, new Microsoft.Modeling.Map<System.Int32, FRS2Model.connectionProperty>().Add(1, FRS2Model.connectionProperty.enabled).Add(2, FRS2Model.connectionProperty.enabled).Add(3, FRS2Model.connectionProperty.enabled).Add(4, FRS2Model.connectionProperty.disabled), new Microsoft.Modeling.Map<System.Int32, FRS2Model.FromServer>().Add(1, FRS2Model.FromServer.exists).Add(2, FRS2Model.FromServer.exists).Add(3, FRS2Model.FromServer.exists).Add(4, FRS2Model.FromServer.exists), new Microsoft.Modeling.Map<System.String, System.Int32>().Add("A", 1).Add("B", 2).Add("C", 3).Add("D", 4), new Microsoft.Modeling.Map<System.String, FRS2Model.ReplicationGroupTypes>().Add("A", FRS2Model.ReplicationGroupTypes.sysvol).Add("B", FRS2Model.ReplicationGroupTypes.protection).Add("C", FRS2Model.ReplicationGroupTypes.sysvol).Add("D", FRS2Model.ReplicationGroupTypes.sysvol), new Microsoft.Modeling.Map<System.Int32, System.String>().Add(1, "A").Add(2, "B").Add(3, "C").Add(4, "D"), new Microsoft.Modeling.Map<System.Int32, FRS2Model.accessLevels>().Add(1, FRS2Model.accessLevels.writeOnly).Add(2, FRS2Model.accessLevels.readWrite).Add(3, FRS2Model.accessLevels.readOnly).Add(4, FRS2Model.accessLevels.readWrite), new Microsoft.Modeling.Map<System.Int32, FRS2Model.connectionProperty>().Add(1, FRS2Model.connectionProperty.enabled).Add(2, FRS2Model.connectionProperty.disabled).Add(3, FRS2Model.connectionProperty.enabled).Add(4, FRS2Model.connectionProperty.enabled));
            this.Manager.Comment("reaching state \'S21\'");
            this.Manager.Comment("checking step \'return Initialization/ERROR_SUCCESS\'");
            this.Manager.Assert((temp124 == FRS2Model.error_status_t.ERROR_SUCCESS), String.Format("expected \'FRS2Model.error_status_t.ERROR_SUCCESS\', actual \'{0}\' (return of Initia" +
                        "lization, state S21)", TestManagerHelpers.Describe(temp124)));
            this.Manager.Comment("reaching state \'S46\'");
            FRS2Model.ProtocolVersionReturned temp125;
            FRS2Model.UpstreamFlagValueReturned temp126;
            FRS2Model.error_status_t temp127;
            this.Manager.Comment("executing step \'call EstablishConnection(\"A\",1,FRS_COMMUNICATION_PROTOCOL_VERSION" +
                    "_LONGHORN_SERVER,out _,out _)\'");
            temp127 = this.IFRS2ManagedAdapterInstance.EstablishConnection("A", 1, FRS2Model.ProtocolVersion.FRS_COMMUNICATION_PROTOCOL_VERSION_LONGHORN_SERVER, out temp125, out temp126);
            this.Manager.Checkpoint("MS-FRS2_R37");
            this.Manager.Checkpoint("MS-FRS2_R436");
            this.Manager.Checkpoint("MS-FRS2_R439");
            this.Manager.Comment("reaching state \'S64\'");
            this.Manager.Comment("checking step \'return EstablishConnection/[out Valid,out Valid]:ERROR_SUCCESS\'");
            this.Manager.Assert((temp125 == FRS2Model.ProtocolVersionReturned.Valid), String.Format("expected \'FRS2Model.ProtocolVersionReturned.Valid\', actual \'{0}\' (upstreamProtoco" +
                        "lVersion of EstablishConnection, state S64)", TestManagerHelpers.Describe(temp125)));
            this.Manager.Assert((temp126 == FRS2Model.UpstreamFlagValueReturned.Valid), String.Format("expected \'FRS2Model.UpstreamFlagValueReturned.Valid\', actual \'{0}\' (upstreamFlags" +
                        " of EstablishConnection, state S64)", TestManagerHelpers.Describe(temp126)));
            this.Manager.Assert((temp127 == FRS2Model.error_status_t.ERROR_SUCCESS), String.Format("expected \'FRS2Model.error_status_t.ERROR_SUCCESS\', actual \'{0}\' (return of Establ" +
                        "ishConnection, state S64)", TestManagerHelpers.Describe(temp127)));
            this.Manager.Comment("reaching state \'S82\'");
            FRS2Model.error_status_t temp128;
            this.Manager.Comment("executing step \'call EstablishSession(1,1)\'");
            temp128 = this.IFRS2ManagedAdapterInstance.EstablishSession(1, 1);
            this.Manager.Checkpoint("MS-FRS2_R455");
            this.Manager.Checkpoint("MS-FRS2_R458");
            this.Manager.Checkpoint("MS-FRS2_R448");
            this.Manager.Comment("reaching state \'S100\'");
            this.Manager.Comment("checking step \'return EstablishSession/ERROR_SUCCESS\'");
            this.Manager.Assert((temp128 == FRS2Model.error_status_t.ERROR_SUCCESS), String.Format("expected \'FRS2Model.error_status_t.ERROR_SUCCESS\', actual \'{0}\' (return of Establ" +
                        "ishSession, state S100)", TestManagerHelpers.Describe(temp128)));
            this.Manager.Comment("reaching state \'S118\'");
            FRS2Model.error_status_t temp129;
            this.Manager.Comment("executing step \'call AsyncPoll(1)\'");
            temp129 = this.IFRS2ManagedAdapterInstance.AsyncPoll(1);
            this.Manager.Checkpoint("MS-FRS2_R549");
            this.Manager.Checkpoint("MS-FRS2_R552");
            this.Manager.Comment("reaching state \'S135\'");
            this.Manager.Comment("checking step \'return AsyncPoll/ERROR_SUCCESS\'");
            this.Manager.Assert((temp129 == FRS2Model.error_status_t.ERROR_SUCCESS), String.Format("expected \'FRS2Model.error_status_t.ERROR_SUCCESS\', actual \'{0}\' (return of AsyncP" +
                        "oll, state S135)", TestManagerHelpers.Describe(temp129)));
            this.Manager.Comment("reaching state \'S151\'");
            FRS2Model.error_status_t temp130;
            this.Manager.Comment("executing step \'call RequestVersionVector(1,1,5,REQUEST_SLAVE_SYNC,CHANGE_ALL,Val" +
                    "idValue)\'");
            temp130 = this.IFRS2ManagedAdapterInstance.RequestVersionVector(1, 1, 5, FRS2Model.VERSION_REQUEST_TYPE.REQUEST_SLAVE_SYNC, FRS2Model.VERSION_CHANGE_TYPE.CHANGE_ALL, FRS2Model.VVGeneration.ValidValue);
            this.Manager.Checkpoint("MS-FRS2_R518");
            this.Manager.Checkpoint("MS-FRS2_R523");
            this.Manager.Checkpoint("MS-FRS2_R529");
            this.Manager.Comment("reaching state \'S167\'");
            this.Manager.Comment("checking step \'return RequestVersionVector/ERROR_FAIL\'");
            this.Manager.Assert((temp130 == FRS2Model.error_status_t.ERROR_FAIL), String.Format("expected \'FRS2Model.error_status_t.ERROR_FAIL\', actual \'{0}\' (return of RequestVe" +
                        "rsionVector, state S167)", TestManagerHelpers.Describe(temp130)));
            TCtestScenario1S173();
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S22
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        [Microsoft.VisualStudio.TestTools.UnitTesting.DescriptionAttribute("MS-FRS2_R37, MS-FRS2_R436, MS-FRS2_R439, MS-FRS2_R455, MS-FRS2_R458, MS-FRS2_R448" +
            ", MS-FRS2_R518, MS-FRS2_R522, MS-FRS2_R523, MS-FRS2_R529")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestCategory("AD")]
        [TestCategory("MS-FRS2")]
        [TestCategory("SDC")]
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]       
        public virtual void FRS2_TCtestScenario1S22()
        {
            this.Manager.BeginTest("TCtestScenario1S22");
            this.Manager.Comment("reaching state \'S22\'");
            FRS2Model.error_status_t temp131;
            this.Manager.Comment(@"executing step 'call Initialization(Windows7,Map{1=>enabled,2=>enabled,3=>enabled,4=>disabled},Map{1=>exists,2=>exists,3=>exists,4=>exists},Map{""A""=>1,""B""=>2,""C""=>3,""D""=>4},Map{""A""=>sysvol,""B""=>protection,""C""=>sysvol,""D""=>sysvol},Map{1=>""A"",2=>""B"",3=>""C"",4=>""D""},Map{1=>writeOnly,2=>readWrite,3=>readOnly,4=>readWrite},Map{1=>enabled,2=>disabled,3=>enabled,4=>enabled})'");
            temp131 = this.IFRS2ManagedAdapterInstance.Initialization(FRS2Model.OSVersion.Windows7, new Microsoft.Modeling.Map<System.Int32, FRS2Model.connectionProperty>().Add(1, FRS2Model.connectionProperty.enabled).Add(2, FRS2Model.connectionProperty.enabled).Add(3, FRS2Model.connectionProperty.enabled).Add(4, FRS2Model.connectionProperty.disabled), new Microsoft.Modeling.Map<System.Int32, FRS2Model.FromServer>().Add(1, FRS2Model.FromServer.exists).Add(2, FRS2Model.FromServer.exists).Add(3, FRS2Model.FromServer.exists).Add(4, FRS2Model.FromServer.exists), new Microsoft.Modeling.Map<System.String, System.Int32>().Add("A", 1).Add("B", 2).Add("C", 3).Add("D", 4), new Microsoft.Modeling.Map<System.String, FRS2Model.ReplicationGroupTypes>().Add("A", FRS2Model.ReplicationGroupTypes.sysvol).Add("B", FRS2Model.ReplicationGroupTypes.protection).Add("C", FRS2Model.ReplicationGroupTypes.sysvol).Add("D", FRS2Model.ReplicationGroupTypes.sysvol), new Microsoft.Modeling.Map<System.Int32, System.String>().Add(1, "A").Add(2, "B").Add(3, "C").Add(4, "D"), new Microsoft.Modeling.Map<System.Int32, FRS2Model.accessLevels>().Add(1, FRS2Model.accessLevels.writeOnly).Add(2, FRS2Model.accessLevels.readWrite).Add(3, FRS2Model.accessLevels.readOnly).Add(4, FRS2Model.accessLevels.readWrite), new Microsoft.Modeling.Map<System.Int32, FRS2Model.connectionProperty>().Add(1, FRS2Model.connectionProperty.enabled).Add(2, FRS2Model.connectionProperty.disabled).Add(3, FRS2Model.connectionProperty.enabled).Add(4, FRS2Model.connectionProperty.enabled));
            this.Manager.Comment("reaching state \'S23\'");
            this.Manager.Comment("checking step \'return Initialization/ERROR_SUCCESS\'");
            this.Manager.Assert((temp131 == FRS2Model.error_status_t.ERROR_SUCCESS), String.Format("expected \'FRS2Model.error_status_t.ERROR_SUCCESS\', actual \'{0}\' (return of Initia" +
                        "lization, state S23)", TestManagerHelpers.Describe(temp131)));
            this.Manager.Comment("reaching state \'S47\'");
            FRS2Model.ProtocolVersionReturned temp132;
            FRS2Model.UpstreamFlagValueReturned temp133;
            FRS2Model.error_status_t temp134;
            this.Manager.Comment("executing step \'call EstablishConnection(\"A\",1,FRS_COMMUNICATION_PROTOCOL_VERSION" +
                    "_LONGHORN_SERVER,out _,out _)\'");
            temp134 = this.IFRS2ManagedAdapterInstance.EstablishConnection("A", 1, FRS2Model.ProtocolVersion.FRS_COMMUNICATION_PROTOCOL_VERSION_LONGHORN_SERVER, out temp132, out temp133);
            this.Manager.Checkpoint("MS-FRS2_R37");
            this.Manager.Checkpoint("MS-FRS2_R436");
            this.Manager.Checkpoint("MS-FRS2_R439");
            this.Manager.Comment("reaching state \'S65\'");
            this.Manager.Comment("checking step \'return EstablishConnection/[out Valid,out Valid]:ERROR_SUCCESS\'");
            this.Manager.Assert((temp132 == FRS2Model.ProtocolVersionReturned.Valid), String.Format("expected \'FRS2Model.ProtocolVersionReturned.Valid\', actual \'{0}\' (upstreamProtoco" +
                        "lVersion of EstablishConnection, state S65)", TestManagerHelpers.Describe(temp132)));
            this.Manager.Assert((temp133 == FRS2Model.UpstreamFlagValueReturned.Valid), String.Format("expected \'FRS2Model.UpstreamFlagValueReturned.Valid\', actual \'{0}\' (upstreamFlags" +
                        " of EstablishConnection, state S65)", TestManagerHelpers.Describe(temp133)));
            this.Manager.Assert((temp134 == FRS2Model.error_status_t.ERROR_SUCCESS), String.Format("expected \'FRS2Model.error_status_t.ERROR_SUCCESS\', actual \'{0}\' (return of Establ" +
                        "ishConnection, state S65)", TestManagerHelpers.Describe(temp134)));
            this.Manager.Comment("reaching state \'S83\'");
            FRS2Model.error_status_t temp135;
            this.Manager.Comment("executing step \'call EstablishSession(1,1)\'");
            temp135 = this.IFRS2ManagedAdapterInstance.EstablishSession(1, 1);
            this.Manager.Checkpoint("MS-FRS2_R455");
            this.Manager.Checkpoint("MS-FRS2_R458");
            this.Manager.Checkpoint("MS-FRS2_R448");
            this.Manager.Comment("reaching state \'S101\'");
            this.Manager.Comment("checking step \'return EstablishSession/ERROR_SUCCESS\'");
            this.Manager.Assert((temp135 == FRS2Model.error_status_t.ERROR_SUCCESS), String.Format("expected \'FRS2Model.error_status_t.ERROR_SUCCESS\', actual \'{0}\' (return of Establ" +
                        "ishSession, state S101)", TestManagerHelpers.Describe(temp135)));
            this.Manager.Comment("reaching state \'S119\'");
            FRS2Model.error_status_t temp136;
            this.Manager.Comment("executing step \'call RequestVersionVector(1,4,5,REQUEST_NORMAL_SYNC,CHANGE_ALL,In" +
                    "validValue)\'");
            temp136 = this.IFRS2ManagedAdapterInstance.RequestVersionVector(1, 4, 5, FRS2Model.VERSION_REQUEST_TYPE.REQUEST_NORMAL_SYNC, FRS2Model.VERSION_CHANGE_TYPE.CHANGE_ALL, FRS2Model.VVGeneration.InvalidValue);
            this.Manager.Checkpoint("MS-FRS2_R518");
            this.Manager.Checkpoint("MS-FRS2_R522");
            this.Manager.Checkpoint("MS-FRS2_R523");
            this.Manager.Checkpoint("MS-FRS2_R529");
            this.Manager.AddReturn(RequestVersionVectorInfo, null, temp136);
            TCtestScenario1S136();
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S24
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        [Microsoft.VisualStudio.TestTools.UnitTesting.DescriptionAttribute("MS-FRS2_R37, MS-FRS2_R436, MS-FRS2_R439, MS-FRS2_R455, MS-FRS2_R458, MS-FRS2_R448" +
            ", MS-FRS2_R550, MS-FRS2_R554, MS-FRS2_R518, MS-FRS2_R522, MS-FRS2_R523, MS-FRS2_" +
            "R527, MS-FRS2_R530")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestCategory("AD")]
        [TestCategory("MS-FRS2")]
        [TestCategory("SDC")]
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        public virtual void FRS2_TCtestScenario1S24()
        {
            this.Manager.BeginTest("TCtestScenario1S24");
            this.Manager.Comment("reaching state \'S24\'");
            FRS2Model.error_status_t temp137;
            this.Manager.Comment(@"executing step 'call Initialization(Windows7,Map{1=>enabled,2=>enabled,3=>enabled,4=>disabled},Map{1=>exists,2=>exists,3=>exists,4=>exists},Map{""A""=>1,""B""=>2,""C""=>3,""D""=>4},Map{""A""=>sysvol,""B""=>protection,""C""=>sysvol,""D""=>sysvol},Map{1=>""A"",2=>""B"",3=>""C"",4=>""D""},Map{1=>writeOnly,2=>readWrite,3=>readOnly,4=>readWrite},Map{1=>enabled,2=>disabled,3=>enabled,4=>enabled})'");
            temp137 = this.IFRS2ManagedAdapterInstance.Initialization(FRS2Model.OSVersion.Windows7, new Microsoft.Modeling.Map<System.Int32, FRS2Model.connectionProperty>().Add(1, FRS2Model.connectionProperty.enabled).Add(2, FRS2Model.connectionProperty.enabled).Add(3, FRS2Model.connectionProperty.enabled).Add(4, FRS2Model.connectionProperty.disabled), new Microsoft.Modeling.Map<System.Int32, FRS2Model.FromServer>().Add(1, FRS2Model.FromServer.exists).Add(2, FRS2Model.FromServer.exists).Add(3, FRS2Model.FromServer.exists).Add(4, FRS2Model.FromServer.exists), new Microsoft.Modeling.Map<System.String, System.Int32>().Add("A", 1).Add("B", 2).Add("C", 3).Add("D", 4), new Microsoft.Modeling.Map<System.String, FRS2Model.ReplicationGroupTypes>().Add("A", FRS2Model.ReplicationGroupTypes.sysvol).Add("B", FRS2Model.ReplicationGroupTypes.protection).Add("C", FRS2Model.ReplicationGroupTypes.sysvol).Add("D", FRS2Model.ReplicationGroupTypes.sysvol), new Microsoft.Modeling.Map<System.Int32, System.String>().Add(1, "A").Add(2, "B").Add(3, "C").Add(4, "D"), new Microsoft.Modeling.Map<System.Int32, FRS2Model.accessLevels>().Add(1, FRS2Model.accessLevels.writeOnly).Add(2, FRS2Model.accessLevels.readWrite).Add(3, FRS2Model.accessLevels.readOnly).Add(4, FRS2Model.accessLevels.readWrite), new Microsoft.Modeling.Map<System.Int32, FRS2Model.connectionProperty>().Add(1, FRS2Model.connectionProperty.enabled).Add(2, FRS2Model.connectionProperty.disabled).Add(3, FRS2Model.connectionProperty.enabled).Add(4, FRS2Model.connectionProperty.enabled));
            this.Manager.Comment("reaching state \'S25\'");
            this.Manager.Comment("checking step \'return Initialization/ERROR_SUCCESS\'");
            this.Manager.Assert((temp137 == FRS2Model.error_status_t.ERROR_SUCCESS), String.Format("expected \'FRS2Model.error_status_t.ERROR_SUCCESS\', actual \'{0}\' (return of Initia" +
                        "lization, state S25)", TestManagerHelpers.Describe(temp137)));
            this.Manager.Comment("reaching state \'S48\'");
            FRS2Model.ProtocolVersionReturned temp138;
            FRS2Model.UpstreamFlagValueReturned temp139;
            FRS2Model.error_status_t temp140;
            this.Manager.Comment("executing step \'call EstablishConnection(\"A\",1,FRS_COMMUNICATION_PROTOCOL_VERSION" +
                    "_LONGHORN_SERVER,out _,out _)\'");
            temp140 = this.IFRS2ManagedAdapterInstance.EstablishConnection("A", 1, FRS2Model.ProtocolVersion.FRS_COMMUNICATION_PROTOCOL_VERSION_LONGHORN_SERVER, out temp138, out temp139);
            this.Manager.Checkpoint("MS-FRS2_R37");
            this.Manager.Checkpoint("MS-FRS2_R436");
            this.Manager.Checkpoint("MS-FRS2_R439");
            this.Manager.Comment("reaching state \'S66\'");
            this.Manager.Comment("checking step \'return EstablishConnection/[out Valid,out Valid]:ERROR_SUCCESS\'");
            this.Manager.Assert((temp138 == FRS2Model.ProtocolVersionReturned.Valid), String.Format("expected \'FRS2Model.ProtocolVersionReturned.Valid\', actual \'{0}\' (upstreamProtoco" +
                        "lVersion of EstablishConnection, state S66)", TestManagerHelpers.Describe(temp138)));
            this.Manager.Assert((temp139 == FRS2Model.UpstreamFlagValueReturned.Valid), String.Format("expected \'FRS2Model.UpstreamFlagValueReturned.Valid\', actual \'{0}\' (upstreamFlags" +
                        " of EstablishConnection, state S66)", TestManagerHelpers.Describe(temp139)));
            this.Manager.Assert((temp140 == FRS2Model.error_status_t.ERROR_SUCCESS), String.Format("expected \'FRS2Model.error_status_t.ERROR_SUCCESS\', actual \'{0}\' (return of Establ" +
                        "ishConnection, state S66)", TestManagerHelpers.Describe(temp140)));
            this.Manager.Comment("reaching state \'S84\'");
            FRS2Model.error_status_t temp141;
            this.Manager.Comment("executing step \'call EstablishSession(1,1)\'");
            temp141 = this.IFRS2ManagedAdapterInstance.EstablishSession(1, 1);
            this.Manager.Checkpoint("MS-FRS2_R455");
            this.Manager.Checkpoint("MS-FRS2_R458");
            this.Manager.Checkpoint("MS-FRS2_R448");
            this.Manager.Comment("reaching state \'S102\'");
            this.Manager.Comment("checking step \'return EstablishSession/ERROR_SUCCESS\'");
            this.Manager.Assert((temp141 == FRS2Model.error_status_t.ERROR_SUCCESS), String.Format("expected \'FRS2Model.error_status_t.ERROR_SUCCESS\', actual \'{0}\' (return of Establ" +
                        "ishSession, state S102)", TestManagerHelpers.Describe(temp141)));
            this.Manager.Comment("reaching state \'S120\'");
            FRS2Model.error_status_t temp142;
            this.Manager.Comment("executing step \'call AsyncPoll(2)\'");
            temp142 = this.IFRS2ManagedAdapterInstance.AsyncPoll(2);
            this.Manager.Checkpoint("MS-FRS2_R550");
            this.Manager.Checkpoint("MS-FRS2_R554");
            this.Manager.Comment("reaching state \'S137\'");
            this.Manager.Comment("checking step \'return AsyncPoll/ERROR_FAIL\'");
            this.Manager.Assert((temp142 == FRS2Model.error_status_t.ERROR_FAIL), String.Format("expected \'FRS2Model.error_status_t.ERROR_FAIL\', actual \'{0}\' (return of AsyncPoll" +
                        ", state S137)", TestManagerHelpers.Describe(temp142)));
            this.Manager.Comment("reaching state \'S153\'");
            FRS2Model.error_status_t temp143;
            this.Manager.Comment("executing step \'call RequestVersionVector(1,4,5,REQUEST_SLAVE_SYNC,CHANGE_NOTIFY," +
                    "ValidValue)\'");
            temp143 = this.IFRS2ManagedAdapterInstance.RequestVersionVector(1, 4, 5, FRS2Model.VERSION_REQUEST_TYPE.REQUEST_SLAVE_SYNC, FRS2Model.VERSION_CHANGE_TYPE.CHANGE_NOTIFY, FRS2Model.VVGeneration.ValidValue);
            this.Manager.Checkpoint("MS-FRS2_R518");
            this.Manager.Checkpoint("MS-FRS2_R522");
            this.Manager.Checkpoint("MS-FRS2_R523");
            this.Manager.Checkpoint("MS-FRS2_R527");
            this.Manager.Checkpoint("MS-FRS2_R530");
            this.Manager.AddReturn(RequestVersionVectorInfo, null, temp143);
            TCtestScenario1S141();
            this.Manager.EndTest();
        }

        private void TCtestScenario1S141()
        {
            this.Manager.Comment("reaching state \'S141\'");
            this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(TCtestScenario1.RequestVersionVectorInfo, null, new RequestVersionVectorDelegate1(this.TCtestScenario1S24RequestVersionVectorChecker)));
            TCtestScenario1S152();
        }

        private void TCtestScenario1S24RequestVersionVectorChecker(FRS2Model.error_status_t @return)
        {
            this.Manager.Comment("checking step \'return RequestVersionVector/ERROR_FAIL\'");
            if (!Microsoft.Protocols.TestSuites.MS_FRS2.ConfigStore.IsTestSYSVOL)
                this.Manager.Assert((@return == FRS2Model.error_status_t.ERROR_FAIL), String.Format("expected \'FRS2Model.error_status_t.ERROR_FAIL\', actual \'{0}\' (return of RequestVe" +
                            "rsionVector, state S141)", TestManagerHelpers.Describe(@return)));
            else
                this.Manager.Assert((@return == FRS2Model.error_status_t.ERROR_INVALID_PARAMETER), String.Format("expected \'FRS2Model.error_status_t.ERROR_INVALID_PARAMETER\', actual \'{0}\' (return of RequestVe" +
                       "rsionVector, state S141)", TestManagerHelpers.Describe(@return)));
        }
        #endregion

        #region Test Starting in S26
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        [Microsoft.VisualStudio.TestTools.UnitTesting.DescriptionAttribute("MS-FRS2_R37, MS-FRS2_R436, MS-FRS2_R439, MS-FRS2_R455, MS-FRS2_R458, MS-FRS2_R448" +
            ", MS-FRS2_R549, MS-FRS2_R552, MS-FRS2_R518, MS-FRS2_R522, MS-FRS2_R523, MS-FRS2_" +
            "R527, MS-FRS2_R530")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestCategory("AD")]
        [TestCategory("MS-FRS2")]
        [TestCategory("SDC")]
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]       
        public virtual void FRS2_TCtestScenario1S26()
        {
            this.Manager.BeginTest("TCtestScenario1S26");
            this.Manager.Comment("reaching state \'S26\'");
            FRS2Model.error_status_t temp144;
            this.Manager.Comment(@"executing step 'call Initialization(Windows7,Map{1=>enabled,2=>enabled,3=>enabled,4=>disabled},Map{1=>exists,2=>exists,3=>exists,4=>exists},Map{""A""=>1,""B""=>2,""C""=>3,""D""=>4},Map{""A""=>sysvol,""B""=>protection,""C""=>sysvol,""D""=>sysvol},Map{1=>""A"",2=>""B"",3=>""C"",4=>""D""},Map{1=>writeOnly,2=>readWrite,3=>readOnly,4=>readWrite},Map{1=>enabled,2=>disabled,3=>enabled,4=>enabled})'");
            temp144 = this.IFRS2ManagedAdapterInstance.Initialization(FRS2Model.OSVersion.Windows7, new Microsoft.Modeling.Map<System.Int32, FRS2Model.connectionProperty>().Add(1, FRS2Model.connectionProperty.enabled).Add(2, FRS2Model.connectionProperty.enabled).Add(3, FRS2Model.connectionProperty.enabled).Add(4, FRS2Model.connectionProperty.disabled), new Microsoft.Modeling.Map<System.Int32, FRS2Model.FromServer>().Add(1, FRS2Model.FromServer.exists).Add(2, FRS2Model.FromServer.exists).Add(3, FRS2Model.FromServer.exists).Add(4, FRS2Model.FromServer.exists), new Microsoft.Modeling.Map<System.String, System.Int32>().Add("A", 1).Add("B", 2).Add("C", 3).Add("D", 4), new Microsoft.Modeling.Map<System.String, FRS2Model.ReplicationGroupTypes>().Add("A", FRS2Model.ReplicationGroupTypes.sysvol).Add("B", FRS2Model.ReplicationGroupTypes.protection).Add("C", FRS2Model.ReplicationGroupTypes.sysvol).Add("D", FRS2Model.ReplicationGroupTypes.sysvol), new Microsoft.Modeling.Map<System.Int32, System.String>().Add(1, "A").Add(2, "B").Add(3, "C").Add(4, "D"), new Microsoft.Modeling.Map<System.Int32, FRS2Model.accessLevels>().Add(1, FRS2Model.accessLevels.writeOnly).Add(2, FRS2Model.accessLevels.readWrite).Add(3, FRS2Model.accessLevels.readOnly).Add(4, FRS2Model.accessLevels.readWrite), new Microsoft.Modeling.Map<System.Int32, FRS2Model.connectionProperty>().Add(1, FRS2Model.connectionProperty.enabled).Add(2, FRS2Model.connectionProperty.disabled).Add(3, FRS2Model.connectionProperty.enabled).Add(4, FRS2Model.connectionProperty.enabled));
            this.Manager.Comment("reaching state \'S27\'");
            this.Manager.Comment("checking step \'return Initialization/ERROR_SUCCESS\'");
            this.Manager.Assert((temp144 == FRS2Model.error_status_t.ERROR_SUCCESS), String.Format("expected \'FRS2Model.error_status_t.ERROR_SUCCESS\', actual \'{0}\' (return of Initia" +
                        "lization, state S27)", TestManagerHelpers.Describe(temp144)));
            this.Manager.Comment("reaching state \'S49\'");
            FRS2Model.ProtocolVersionReturned temp145;
            FRS2Model.UpstreamFlagValueReturned temp146;
            FRS2Model.error_status_t temp147;
            this.Manager.Comment("executing step \'call EstablishConnection(\"A\",1,FRS_COMMUNICATION_PROTOCOL_VERSION" +
                    "_LONGHORN_SERVER,out _,out _)\'");
            temp147 = this.IFRS2ManagedAdapterInstance.EstablishConnection("A", 1, FRS2Model.ProtocolVersion.FRS_COMMUNICATION_PROTOCOL_VERSION_LONGHORN_SERVER, out temp145, out temp146);
            this.Manager.Checkpoint("MS-FRS2_R37");
            this.Manager.Checkpoint("MS-FRS2_R436");
            this.Manager.Checkpoint("MS-FRS2_R439");
            this.Manager.Comment("reaching state \'S67\'");
            this.Manager.Comment("checking step \'return EstablishConnection/[out Valid,out Valid]:ERROR_SUCCESS\'");
            this.Manager.Assert((temp145 == FRS2Model.ProtocolVersionReturned.Valid), String.Format("expected \'FRS2Model.ProtocolVersionReturned.Valid\', actual \'{0}\' (upstreamProtoco" +
                        "lVersion of EstablishConnection, state S67)", TestManagerHelpers.Describe(temp145)));
            this.Manager.Assert((temp146 == FRS2Model.UpstreamFlagValueReturned.Valid), String.Format("expected \'FRS2Model.UpstreamFlagValueReturned.Valid\', actual \'{0}\' (upstreamFlags" +
                        " of EstablishConnection, state S67)", TestManagerHelpers.Describe(temp146)));
            this.Manager.Assert((temp147 == FRS2Model.error_status_t.ERROR_SUCCESS), String.Format("expected \'FRS2Model.error_status_t.ERROR_SUCCESS\', actual \'{0}\' (return of Establ" +
                        "ishConnection, state S67)", TestManagerHelpers.Describe(temp147)));
            this.Manager.Comment("reaching state \'S85\'");
            FRS2Model.error_status_t temp148;
            this.Manager.Comment("executing step \'call EstablishSession(1,1)\'");
            temp148 = this.IFRS2ManagedAdapterInstance.EstablishSession(1, 1);
            this.Manager.Checkpoint("MS-FRS2_R455");
            this.Manager.Checkpoint("MS-FRS2_R458");
            this.Manager.Checkpoint("MS-FRS2_R448");
            this.Manager.Comment("reaching state \'S103\'");
            this.Manager.Comment("checking step \'return EstablishSession/ERROR_SUCCESS\'");
            this.Manager.Assert((temp148 == FRS2Model.error_status_t.ERROR_SUCCESS), String.Format("expected \'FRS2Model.error_status_t.ERROR_SUCCESS\', actual \'{0}\' (return of Establ" +
                        "ishSession, state S103)", TestManagerHelpers.Describe(temp148)));
            this.Manager.Comment("reaching state \'S121\'");
            FRS2Model.error_status_t temp149;
            this.Manager.Comment("executing step \'call AsyncPoll(1)\'");
            temp149 = this.IFRS2ManagedAdapterInstance.AsyncPoll(1);
            this.Manager.Checkpoint("MS-FRS2_R549");
            this.Manager.Checkpoint("MS-FRS2_R552");
            this.Manager.Comment("reaching state \'S138\'");
            this.Manager.Comment("checking step \'return AsyncPoll/ERROR_SUCCESS\'");
            this.Manager.Assert((temp149 == FRS2Model.error_status_t.ERROR_SUCCESS), String.Format("expected \'FRS2Model.error_status_t.ERROR_SUCCESS\', actual \'{0}\' (return of AsyncP" +
                        "oll, state S138)", TestManagerHelpers.Describe(temp149)));
            this.Manager.Comment("reaching state \'S154\'");
            FRS2Model.error_status_t temp150;
            this.Manager.Comment("executing step \'call RequestVersionVector(1,4,5,REQUEST_SLAVE_SYNC,CHANGE_NOTIFY," +
                    "ValidValue)\'");
            temp150 = this.IFRS2ManagedAdapterInstance.RequestVersionVector(1, 4, 5, FRS2Model.VERSION_REQUEST_TYPE.REQUEST_SLAVE_SYNC, FRS2Model.VERSION_CHANGE_TYPE.CHANGE_NOTIFY, FRS2Model.VVGeneration.ValidValue);
            this.Manager.Checkpoint("MS-FRS2_R518");
            this.Manager.Checkpoint("MS-FRS2_R522");
            this.Manager.Checkpoint("MS-FRS2_R523");
            this.Manager.Checkpoint("MS-FRS2_R527");
            this.Manager.Checkpoint("MS-FRS2_R530");
            this.Manager.Comment("reaching state \'S169\'");
            this.Manager.Comment("checking step \'return RequestVersionVector/ERROR_FAIL\'");
            if (!Microsoft.Protocols.TestSuites.MS_FRS2.ConfigStore.IsTestSYSVOL)
                this.Manager.Assert((temp150 == FRS2Model.error_status_t.ERROR_FAIL), String.Format("expected \'FRS2Model.error_status_t.ERROR_FAIL\', actual \'{0}\' (return of RequestVe" +
                            "rsionVector, state S169)", TestManagerHelpers.Describe(temp150)));
            else
                this.Manager.Assert((temp150 == FRS2Model.error_status_t.ERROR_INVALID_PARAMETER), String.Format("expected \'FRS2Model.error_status_t.ERROR_INVALID_PARAMETER\', actual \'{0}\' (return of RequestVe" +
                     "rsionVector, state S169)", TestManagerHelpers.Describe(temp150)));
            TCtestScenario1S173();
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S28
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        [Microsoft.VisualStudio.TestTools.UnitTesting.DescriptionAttribute("MS-FRS2_R37, MS-FRS2_R436, MS-FRS2_R439, MS-FRS2_R455, MS-FRS2_R458, MS-FRS2_R448" +
            ", MS-FRS2_R518, MS-FRS2_R522, MS-FRS2_R525, MS-FRS2_R527, MS-FRS2_R530")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestCategory("AD")]
        [TestCategory("MS-FRS2")]
        [TestCategory("SDC")]
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]        
        public virtual void FRS2_TCtestScenario1S28()
        {
            this.Manager.BeginTest("TCtestScenario1S28");
            this.Manager.Comment("reaching state \'S28\'");
            FRS2Model.error_status_t temp151;
            this.Manager.Comment(@"executing step 'call Initialization(Windows7,Map{1=>enabled,2=>enabled,3=>enabled,4=>disabled},Map{1=>exists,2=>exists,3=>exists,4=>exists},Map{""A""=>1,""B""=>2,""C""=>3,""D""=>4},Map{""A""=>sysvol,""B""=>protection,""C""=>sysvol,""D""=>sysvol},Map{1=>""A"",2=>""B"",3=>""C"",4=>""D""},Map{1=>writeOnly,2=>readWrite,3=>readOnly,4=>readWrite},Map{1=>enabled,2=>disabled,3=>enabled,4=>enabled})'");
            temp151 = this.IFRS2ManagedAdapterInstance.Initialization(FRS2Model.OSVersion.Windows7, new Microsoft.Modeling.Map<System.Int32, FRS2Model.connectionProperty>().Add(1, FRS2Model.connectionProperty.enabled).Add(2, FRS2Model.connectionProperty.enabled).Add(3, FRS2Model.connectionProperty.enabled).Add(4, FRS2Model.connectionProperty.disabled), new Microsoft.Modeling.Map<System.Int32, FRS2Model.FromServer>().Add(1, FRS2Model.FromServer.exists).Add(2, FRS2Model.FromServer.exists).Add(3, FRS2Model.FromServer.exists).Add(4, FRS2Model.FromServer.exists), new Microsoft.Modeling.Map<System.String, System.Int32>().Add("A", 1).Add("B", 2).Add("C", 3).Add("D", 4), new Microsoft.Modeling.Map<System.String, FRS2Model.ReplicationGroupTypes>().Add("A", FRS2Model.ReplicationGroupTypes.sysvol).Add("B", FRS2Model.ReplicationGroupTypes.protection).Add("C", FRS2Model.ReplicationGroupTypes.sysvol).Add("D", FRS2Model.ReplicationGroupTypes.sysvol), new Microsoft.Modeling.Map<System.Int32, System.String>().Add(1, "A").Add(2, "B").Add(3, "C").Add(4, "D"), new Microsoft.Modeling.Map<System.Int32, FRS2Model.accessLevels>().Add(1, FRS2Model.accessLevels.writeOnly).Add(2, FRS2Model.accessLevels.readWrite).Add(3, FRS2Model.accessLevels.readOnly).Add(4, FRS2Model.accessLevels.readWrite), new Microsoft.Modeling.Map<System.Int32, FRS2Model.connectionProperty>().Add(1, FRS2Model.connectionProperty.enabled).Add(2, FRS2Model.connectionProperty.disabled).Add(3, FRS2Model.connectionProperty.enabled).Add(4, FRS2Model.connectionProperty.enabled));
            this.Manager.Comment("reaching state \'S29\'");
            this.Manager.Comment("checking step \'return Initialization/ERROR_SUCCESS\'");
            this.Manager.Assert((temp151 == FRS2Model.error_status_t.ERROR_SUCCESS), String.Format("expected \'FRS2Model.error_status_t.ERROR_SUCCESS\', actual \'{0}\' (return of Initia" +
                        "lization, state S29)", TestManagerHelpers.Describe(temp151)));
            this.Manager.Comment("reaching state \'S50\'");
            FRS2Model.ProtocolVersionReturned temp152;
            FRS2Model.UpstreamFlagValueReturned temp153;
            FRS2Model.error_status_t temp154;
            this.Manager.Comment("executing step \'call EstablishConnection(\"A\",1,FRS_COMMUNICATION_PROTOCOL_VERSION" +
                    "_LONGHORN_SERVER,out _,out _)\'");
            temp154 = this.IFRS2ManagedAdapterInstance.EstablishConnection("A", 1, FRS2Model.ProtocolVersion.FRS_COMMUNICATION_PROTOCOL_VERSION_LONGHORN_SERVER, out temp152, out temp153);
            this.Manager.Checkpoint("MS-FRS2_R37");
            this.Manager.Checkpoint("MS-FRS2_R436");
            this.Manager.Checkpoint("MS-FRS2_R439");
            this.Manager.Comment("reaching state \'S68\'");
            this.Manager.Comment("checking step \'return EstablishConnection/[out Valid,out Valid]:ERROR_SUCCESS\'");
            this.Manager.Assert((temp152 == FRS2Model.ProtocolVersionReturned.Valid), String.Format("expected \'FRS2Model.ProtocolVersionReturned.Valid\', actual \'{0}\' (upstreamProtoco" +
                        "lVersion of EstablishConnection, state S68)", TestManagerHelpers.Describe(temp152)));
            this.Manager.Assert((temp153 == FRS2Model.UpstreamFlagValueReturned.Valid), String.Format("expected \'FRS2Model.UpstreamFlagValueReturned.Valid\', actual \'{0}\' (upstreamFlags" +
                        " of EstablishConnection, state S68)", TestManagerHelpers.Describe(temp153)));
            this.Manager.Assert((temp154 == FRS2Model.error_status_t.ERROR_SUCCESS), String.Format("expected \'FRS2Model.error_status_t.ERROR_SUCCESS\', actual \'{0}\' (return of Establ" +
                        "ishConnection, state S68)", TestManagerHelpers.Describe(temp154)));
            this.Manager.Comment("reaching state \'S86\'");
            FRS2Model.error_status_t temp155;
            this.Manager.Comment("executing step \'call EstablishSession(1,1)\'");
            temp155 = this.IFRS2ManagedAdapterInstance.EstablishSession(1, 1);
            this.Manager.Checkpoint("MS-FRS2_R455");
            this.Manager.Checkpoint("MS-FRS2_R458");
            this.Manager.Checkpoint("MS-FRS2_R448");
            this.Manager.Comment("reaching state \'S104\'");
            this.Manager.Comment("checking step \'return EstablishSession/ERROR_SUCCESS\'");
            this.Manager.Assert((temp155 == FRS2Model.error_status_t.ERROR_SUCCESS), String.Format("expected \'FRS2Model.error_status_t.ERROR_SUCCESS\', actual \'{0}\' (return of Establ" +
                        "ishSession, state S104)", TestManagerHelpers.Describe(temp155)));
            this.Manager.Comment("reaching state \'S122\'");
            FRS2Model.error_status_t temp156;
            this.Manager.Comment("executing step \'call RequestVersionVector(1,4,1,REQUEST_SLAVE_SYNC,CHANGE_NOTIFY," +
                    "InvalidValue)\'");
            temp156 = this.IFRS2ManagedAdapterInstance.RequestVersionVector(1, 4, 1, FRS2Model.VERSION_REQUEST_TYPE.REQUEST_SLAVE_SYNC, FRS2Model.VERSION_CHANGE_TYPE.CHANGE_NOTIFY, FRS2Model.VVGeneration.InvalidValue);
            this.Manager.Checkpoint("MS-FRS2_R518");
            this.Manager.Checkpoint("MS-FRS2_R522");
            this.Manager.Checkpoint("MS-FRS2_R525");
            this.Manager.Checkpoint("MS-FRS2_R527");
            this.Manager.Checkpoint("MS-FRS2_R530");
            this.Manager.AddReturn(RequestVersionVectorInfo, null, temp156);
            TCtestScenario1S139();
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S30
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        [Microsoft.VisualStudio.TestTools.UnitTesting.DescriptionAttribute("MS-FRS2_R37, MS-FRS2_R436, MS-FRS2_R439, MS-FRS2_R455, MS-FRS2_R458, MS-FRS2_R448" +
            ", MS-FRS2_R518, MS-FRS2_R523, MS-FRS2_R529")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestCategory("AD")]
        [TestCategory("MS-FRS2")]
        [TestCategory("SDC")]
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]       
        public virtual void FRS2_TCtestScenario1S30()
        {
            this.Manager.BeginTest("TCtestScenario1S30");
            this.Manager.Comment("reaching state \'S30\'");
            FRS2Model.error_status_t temp157;
            this.Manager.Comment(@"executing step 'call Initialization(Windows7,Map{1=>enabled,2=>enabled,3=>enabled,4=>disabled},Map{1=>exists,2=>exists,3=>exists,4=>exists},Map{""A""=>1,""B""=>2,""C""=>3,""D""=>4},Map{""A""=>sysvol,""B""=>protection,""C""=>sysvol,""D""=>sysvol},Map{1=>""A"",2=>""B"",3=>""C"",4=>""D""},Map{1=>writeOnly,2=>readWrite,3=>readOnly,4=>readWrite},Map{1=>enabled,2=>disabled,3=>enabled,4=>enabled})'");
            temp157 = this.IFRS2ManagedAdapterInstance.Initialization(FRS2Model.OSVersion.Windows7, new Microsoft.Modeling.Map<System.Int32, FRS2Model.connectionProperty>().Add(1, FRS2Model.connectionProperty.enabled).Add(2, FRS2Model.connectionProperty.enabled).Add(3, FRS2Model.connectionProperty.enabled).Add(4, FRS2Model.connectionProperty.disabled), new Microsoft.Modeling.Map<System.Int32, FRS2Model.FromServer>().Add(1, FRS2Model.FromServer.exists).Add(2, FRS2Model.FromServer.exists).Add(3, FRS2Model.FromServer.exists).Add(4, FRS2Model.FromServer.exists), new Microsoft.Modeling.Map<System.String, System.Int32>().Add("A", 1).Add("B", 2).Add("C", 3).Add("D", 4), new Microsoft.Modeling.Map<System.String, FRS2Model.ReplicationGroupTypes>().Add("A", FRS2Model.ReplicationGroupTypes.sysvol).Add("B", FRS2Model.ReplicationGroupTypes.protection).Add("C", FRS2Model.ReplicationGroupTypes.sysvol).Add("D", FRS2Model.ReplicationGroupTypes.sysvol), new Microsoft.Modeling.Map<System.Int32, System.String>().Add(1, "A").Add(2, "B").Add(3, "C").Add(4, "D"), new Microsoft.Modeling.Map<System.Int32, FRS2Model.accessLevels>().Add(1, FRS2Model.accessLevels.writeOnly).Add(2, FRS2Model.accessLevels.readWrite).Add(3, FRS2Model.accessLevels.readOnly).Add(4, FRS2Model.accessLevels.readWrite), new Microsoft.Modeling.Map<System.Int32, FRS2Model.connectionProperty>().Add(1, FRS2Model.connectionProperty.enabled).Add(2, FRS2Model.connectionProperty.disabled).Add(3, FRS2Model.connectionProperty.enabled).Add(4, FRS2Model.connectionProperty.enabled));
            this.Manager.Comment("reaching state \'S31\'");
            this.Manager.Comment("checking step \'return Initialization/ERROR_SUCCESS\'");
            this.Manager.Assert((temp157 == FRS2Model.error_status_t.ERROR_SUCCESS), String.Format("expected \'FRS2Model.error_status_t.ERROR_SUCCESS\', actual \'{0}\' (return of Initia" +
                        "lization, state S31)", TestManagerHelpers.Describe(temp157)));
            this.Manager.Comment("reaching state \'S51\'");
            FRS2Model.ProtocolVersionReturned temp158;
            FRS2Model.UpstreamFlagValueReturned temp159;
            FRS2Model.error_status_t temp160;
            this.Manager.Comment("executing step \'call EstablishConnection(\"A\",1,FRS_COMMUNICATION_PROTOCOL_VERSION" +
                    "_LONGHORN_SERVER,out _,out _)\'");
            temp160 = this.IFRS2ManagedAdapterInstance.EstablishConnection("A", 1, FRS2Model.ProtocolVersion.FRS_COMMUNICATION_PROTOCOL_VERSION_LONGHORN_SERVER, out temp158, out temp159);
            this.Manager.Checkpoint("MS-FRS2_R37");
            this.Manager.Checkpoint("MS-FRS2_R436");
            this.Manager.Checkpoint("MS-FRS2_R439");
            this.Manager.Comment("reaching state \'S69\'");
            this.Manager.Comment("checking step \'return EstablishConnection/[out Valid,out Valid]:ERROR_SUCCESS\'");
            this.Manager.Assert((temp158 == FRS2Model.ProtocolVersionReturned.Valid), String.Format("expected \'FRS2Model.ProtocolVersionReturned.Valid\', actual \'{0}\' (upstreamProtoco" +
                        "lVersion of EstablishConnection, state S69)", TestManagerHelpers.Describe(temp158)));
            this.Manager.Assert((temp159 == FRS2Model.UpstreamFlagValueReturned.Valid), String.Format("expected \'FRS2Model.UpstreamFlagValueReturned.Valid\', actual \'{0}\' (upstreamFlags" +
                        " of EstablishConnection, state S69)", TestManagerHelpers.Describe(temp159)));
            this.Manager.Assert((temp160 == FRS2Model.error_status_t.ERROR_SUCCESS), String.Format("expected \'FRS2Model.error_status_t.ERROR_SUCCESS\', actual \'{0}\' (return of Establ" +
                        "ishConnection, state S69)", TestManagerHelpers.Describe(temp160)));
            this.Manager.Comment("reaching state \'S87\'");
            FRS2Model.error_status_t temp161;
            this.Manager.Comment("executing step \'call EstablishSession(1,1)\'");
            temp161 = this.IFRS2ManagedAdapterInstance.EstablishSession(1, 1);
            this.Manager.Checkpoint("MS-FRS2_R455");
            this.Manager.Checkpoint("MS-FRS2_R458");
            this.Manager.Checkpoint("MS-FRS2_R448");
            this.Manager.Comment("reaching state \'S105\'");
            this.Manager.Comment("checking step \'return EstablishSession/ERROR_SUCCESS\'");
            this.Manager.Assert((temp161 == FRS2Model.error_status_t.ERROR_SUCCESS), String.Format("expected \'FRS2Model.error_status_t.ERROR_SUCCESS\', actual \'{0}\' (return of Establ" +
                        "ishSession, state S105)", TestManagerHelpers.Describe(temp161)));
            this.Manager.Comment("reaching state \'S123\'");
            FRS2Model.error_status_t temp162;
            this.Manager.Comment("executing step \'call RequestVersionVector(1,1,5,REQUEST_SLAVE_SYNC,CHANGE_ALL,Val" +
                    "idValue)\'");
            temp162 = this.IFRS2ManagedAdapterInstance.RequestVersionVector(1, 1, 5, FRS2Model.VERSION_REQUEST_TYPE.REQUEST_SLAVE_SYNC, FRS2Model.VERSION_CHANGE_TYPE.CHANGE_ALL, FRS2Model.VVGeneration.ValidValue);
            this.Manager.Checkpoint("MS-FRS2_R518");
            this.Manager.Checkpoint("MS-FRS2_R523");
            this.Manager.Checkpoint("MS-FRS2_R529");
            this.Manager.AddReturn(RequestVersionVectorInfo, null, temp162);
            TCtestScenario1S140();
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S32
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        [Microsoft.VisualStudio.TestTools.UnitTesting.DescriptionAttribute("MS-FRS2_R37, MS-FRS2_R436, MS-FRS2_R439, MS-FRS2_R549, MS-FRS2_R552")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestCategory("AD")]
        [TestCategory("MS-FRS2")]
        [TestCategory("SDC")]
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]      
        public virtual void FRS2_TCtestScenario1S32()
        {
            this.Manager.BeginTest("TCtestScenario1S32");
            this.Manager.Comment("reaching state \'S32\'");
            FRS2Model.error_status_t temp163;
            this.Manager.Comment(@"executing step 'call Initialization(Windows7,Map{1=>enabled,2=>enabled,3=>enabled,4=>disabled},Map{1=>exists,2=>exists,3=>exists,4=>exists},Map{""A""=>1,""B""=>2,""C""=>3,""D""=>4},Map{""A""=>sysvol,""B""=>protection,""C""=>sysvol,""D""=>sysvol},Map{1=>""A"",2=>""B"",3=>""C"",4=>""D""},Map{1=>writeOnly,2=>readWrite,3=>readOnly,4=>readWrite},Map{1=>enabled,2=>disabled,3=>enabled,4=>enabled})'");
            temp163 = this.IFRS2ManagedAdapterInstance.Initialization(FRS2Model.OSVersion.Windows7, new Microsoft.Modeling.Map<System.Int32, FRS2Model.connectionProperty>().Add(1, FRS2Model.connectionProperty.enabled).Add(2, FRS2Model.connectionProperty.enabled).Add(3, FRS2Model.connectionProperty.enabled).Add(4, FRS2Model.connectionProperty.disabled), new Microsoft.Modeling.Map<System.Int32, FRS2Model.FromServer>().Add(1, FRS2Model.FromServer.exists).Add(2, FRS2Model.FromServer.exists).Add(3, FRS2Model.FromServer.exists).Add(4, FRS2Model.FromServer.exists), new Microsoft.Modeling.Map<System.String, System.Int32>().Add("A", 1).Add("B", 2).Add("C", 3).Add("D", 4), new Microsoft.Modeling.Map<System.String, FRS2Model.ReplicationGroupTypes>().Add("A", FRS2Model.ReplicationGroupTypes.sysvol).Add("B", FRS2Model.ReplicationGroupTypes.protection).Add("C", FRS2Model.ReplicationGroupTypes.sysvol).Add("D", FRS2Model.ReplicationGroupTypes.sysvol), new Microsoft.Modeling.Map<System.Int32, System.String>().Add(1, "A").Add(2, "B").Add(3, "C").Add(4, "D"), new Microsoft.Modeling.Map<System.Int32, FRS2Model.accessLevels>().Add(1, FRS2Model.accessLevels.writeOnly).Add(2, FRS2Model.accessLevels.readWrite).Add(3, FRS2Model.accessLevels.readOnly).Add(4, FRS2Model.accessLevels.readWrite), new Microsoft.Modeling.Map<System.Int32, FRS2Model.connectionProperty>().Add(1, FRS2Model.connectionProperty.enabled).Add(2, FRS2Model.connectionProperty.disabled).Add(3, FRS2Model.connectionProperty.enabled).Add(4, FRS2Model.connectionProperty.enabled));
            this.Manager.Comment("reaching state \'S33\'");
            this.Manager.Comment("checking step \'return Initialization/ERROR_SUCCESS\'");
            this.Manager.Assert((temp163 == FRS2Model.error_status_t.ERROR_SUCCESS), String.Format("expected \'FRS2Model.error_status_t.ERROR_SUCCESS\', actual \'{0}\' (return of Initia" +
                        "lization, state S33)", TestManagerHelpers.Describe(temp163)));
            this.Manager.Comment("reaching state \'S52\'");
            FRS2Model.ProtocolVersionReturned temp164;
            FRS2Model.UpstreamFlagValueReturned temp165;
            FRS2Model.error_status_t temp166;
            this.Manager.Comment("executing step \'call EstablishConnection(\"A\",1,FRS_COMMUNICATION_PROTOCOL_VERSION" +
                    "_LONGHORN_SERVER,out _,out _)\'");
            temp166 = this.IFRS2ManagedAdapterInstance.EstablishConnection("A", 1, FRS2Model.ProtocolVersion.FRS_COMMUNICATION_PROTOCOL_VERSION_LONGHORN_SERVER, out temp164, out temp165);
            this.Manager.Checkpoint("MS-FRS2_R37");
            this.Manager.Checkpoint("MS-FRS2_R436");
            this.Manager.Checkpoint("MS-FRS2_R439");
            this.Manager.Comment("reaching state \'S70\'");
            this.Manager.Comment("checking step \'return EstablishConnection/[out Valid,out Valid]:ERROR_SUCCESS\'");
            this.Manager.Assert((temp164 == FRS2Model.ProtocolVersionReturned.Valid), String.Format("expected \'FRS2Model.ProtocolVersionReturned.Valid\', actual \'{0}\' (upstreamProtoco" +
                        "lVersion of EstablishConnection, state S70)", TestManagerHelpers.Describe(temp164)));
            this.Manager.Assert((temp165 == FRS2Model.UpstreamFlagValueReturned.Valid), String.Format("expected \'FRS2Model.UpstreamFlagValueReturned.Valid\', actual \'{0}\' (upstreamFlags" +
                        " of EstablishConnection, state S70)", TestManagerHelpers.Describe(temp165)));
            this.Manager.Assert((temp166 == FRS2Model.error_status_t.ERROR_SUCCESS), String.Format("expected \'FRS2Model.error_status_t.ERROR_SUCCESS\', actual \'{0}\' (return of Establ" +
                        "ishConnection, state S70)", TestManagerHelpers.Describe(temp166)));
            this.Manager.Comment("reaching state \'S88\'");
            FRS2Model.error_status_t temp167;
            this.Manager.Comment("executing step \'call AsyncPoll(1)\'");
            temp167 = this.IFRS2ManagedAdapterInstance.AsyncPoll(1);
            this.Manager.Checkpoint("MS-FRS2_R549");
            this.Manager.Checkpoint("MS-FRS2_R552");
            this.Manager.Comment("reaching state \'S106\'");
            this.Manager.Comment("checking step \'return AsyncPoll/ERROR_SUCCESS\'");
            this.Manager.Assert((temp167 == FRS2Model.error_status_t.ERROR_SUCCESS), String.Format("expected \'FRS2Model.error_status_t.ERROR_SUCCESS\', actual \'{0}\' (return of AsyncP" +
                        "oll, state S106)", TestManagerHelpers.Describe(temp167)));
            this.Manager.Comment("reaching state \'S124\'");
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S34
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        [Microsoft.VisualStudio.TestTools.UnitTesting.DescriptionAttribute("MS-FRS2_R37, MS-FRS2_R436, MS-FRS2_R439, MS-FRS2_R455, MS-FRS2_R458, MS-FRS2_R448" +
            ", MS-FRS2_R518, MS-FRS2_R522, MS-FRS2_R523, MS-FRS2_R527, MS-FRS2_R530")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestCategory("AD")]
        [TestCategory("MS-FRS2")]
        [TestCategory("SDC")]
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]      
        public virtual void FRS2_TCtestScenario1S34()
        {
            this.Manager.BeginTest("TCtestScenario1S34");
            this.Manager.Comment("reaching state \'S34\'");
            FRS2Model.error_status_t temp168;
            this.Manager.Comment(@"executing step 'call Initialization(Windows7,Map{1=>enabled,2=>enabled,3=>enabled,4=>disabled},Map{1=>exists,2=>exists,3=>exists,4=>exists},Map{""A""=>1,""B""=>2,""C""=>3,""D""=>4},Map{""A""=>sysvol,""B""=>protection,""C""=>sysvol,""D""=>sysvol},Map{1=>""A"",2=>""B"",3=>""C"",4=>""D""},Map{1=>writeOnly,2=>readWrite,3=>readOnly,4=>readWrite},Map{1=>enabled,2=>disabled,3=>enabled,4=>enabled})'");
            temp168 = this.IFRS2ManagedAdapterInstance.Initialization(FRS2Model.OSVersion.Windows7, new Microsoft.Modeling.Map<System.Int32, FRS2Model.connectionProperty>().Add(1, FRS2Model.connectionProperty.enabled).Add(2, FRS2Model.connectionProperty.enabled).Add(3, FRS2Model.connectionProperty.enabled).Add(4, FRS2Model.connectionProperty.disabled), new Microsoft.Modeling.Map<System.Int32, FRS2Model.FromServer>().Add(1, FRS2Model.FromServer.exists).Add(2, FRS2Model.FromServer.exists).Add(3, FRS2Model.FromServer.exists).Add(4, FRS2Model.FromServer.exists), new Microsoft.Modeling.Map<System.String, System.Int32>().Add("A", 1).Add("B", 2).Add("C", 3).Add("D", 4), new Microsoft.Modeling.Map<System.String, FRS2Model.ReplicationGroupTypes>().Add("A", FRS2Model.ReplicationGroupTypes.sysvol).Add("B", FRS2Model.ReplicationGroupTypes.protection).Add("C", FRS2Model.ReplicationGroupTypes.sysvol).Add("D", FRS2Model.ReplicationGroupTypes.sysvol), new Microsoft.Modeling.Map<System.Int32, System.String>().Add(1, "A").Add(2, "B").Add(3, "C").Add(4, "D"), new Microsoft.Modeling.Map<System.Int32, FRS2Model.accessLevels>().Add(1, FRS2Model.accessLevels.writeOnly).Add(2, FRS2Model.accessLevels.readWrite).Add(3, FRS2Model.accessLevels.readOnly).Add(4, FRS2Model.accessLevels.readWrite), new Microsoft.Modeling.Map<System.Int32, FRS2Model.connectionProperty>().Add(1, FRS2Model.connectionProperty.enabled).Add(2, FRS2Model.connectionProperty.disabled).Add(3, FRS2Model.connectionProperty.enabled).Add(4, FRS2Model.connectionProperty.enabled));
            this.Manager.Comment("reaching state \'S35\'");
            this.Manager.Comment("checking step \'return Initialization/ERROR_SUCCESS\'");
            this.Manager.Assert((temp168 == FRS2Model.error_status_t.ERROR_SUCCESS), String.Format("expected \'FRS2Model.error_status_t.ERROR_SUCCESS\', actual \'{0}\' (return of Initia" +
                        "lization, state S35)", TestManagerHelpers.Describe(temp168)));
            this.Manager.Comment("reaching state \'S53\'");
            FRS2Model.ProtocolVersionReturned temp169;
            FRS2Model.UpstreamFlagValueReturned temp170;
            FRS2Model.error_status_t temp171;
            this.Manager.Comment("executing step \'call EstablishConnection(\"A\",1,FRS_COMMUNICATION_PROTOCOL_VERSION" +
                    "_LONGHORN_SERVER,out _,out _)\'");
            temp171 = this.IFRS2ManagedAdapterInstance.EstablishConnection("A", 1, FRS2Model.ProtocolVersion.FRS_COMMUNICATION_PROTOCOL_VERSION_LONGHORN_SERVER, out temp169, out temp170);
            this.Manager.Checkpoint("MS-FRS2_R37");
            this.Manager.Checkpoint("MS-FRS2_R436");
            this.Manager.Checkpoint("MS-FRS2_R439");
            this.Manager.Comment("reaching state \'S71\'");
            this.Manager.Comment("checking step \'return EstablishConnection/[out Valid,out Valid]:ERROR_SUCCESS\'");
            this.Manager.Assert((temp169 == FRS2Model.ProtocolVersionReturned.Valid), String.Format("expected \'FRS2Model.ProtocolVersionReturned.Valid\', actual \'{0}\' (upstreamProtoco" +
                        "lVersion of EstablishConnection, state S71)", TestManagerHelpers.Describe(temp169)));
            this.Manager.Assert((temp170 == FRS2Model.UpstreamFlagValueReturned.Valid), String.Format("expected \'FRS2Model.UpstreamFlagValueReturned.Valid\', actual \'{0}\' (upstreamFlags" +
                        " of EstablishConnection, state S71)", TestManagerHelpers.Describe(temp170)));
            this.Manager.Assert((temp171 == FRS2Model.error_status_t.ERROR_SUCCESS), String.Format("expected \'FRS2Model.error_status_t.ERROR_SUCCESS\', actual \'{0}\' (return of Establ" +
                        "ishConnection, state S71)", TestManagerHelpers.Describe(temp171)));
            this.Manager.Comment("reaching state \'S89\'");
            FRS2Model.error_status_t temp172;
            this.Manager.Comment("executing step \'call EstablishSession(1,1)\'");
            temp172 = this.IFRS2ManagedAdapterInstance.EstablishSession(1, 1);
            this.Manager.Checkpoint("MS-FRS2_R455");
            this.Manager.Checkpoint("MS-FRS2_R458");
            this.Manager.Checkpoint("MS-FRS2_R448");
            this.Manager.Comment("reaching state \'S107\'");
            this.Manager.Comment("checking step \'return EstablishSession/ERROR_SUCCESS\'");
            this.Manager.Assert((temp172 == FRS2Model.error_status_t.ERROR_SUCCESS), String.Format("expected \'FRS2Model.error_status_t.ERROR_SUCCESS\', actual \'{0}\' (return of Establ" +
                        "ishSession, state S107)", TestManagerHelpers.Describe(temp172)));
            this.Manager.Comment("reaching state \'S125\'");
            FRS2Model.error_status_t temp173;
            this.Manager.Comment("executing step \'call RequestVersionVector(1,4,5,REQUEST_SLAVE_SYNC,CHANGE_NOTIFY," +
                    "ValidValue)\'");
            temp173 = this.IFRS2ManagedAdapterInstance.RequestVersionVector(1, 4, 5, FRS2Model.VERSION_REQUEST_TYPE.REQUEST_SLAVE_SYNC, FRS2Model.VERSION_CHANGE_TYPE.CHANGE_NOTIFY, FRS2Model.VVGeneration.ValidValue);
            this.Manager.Checkpoint("MS-FRS2_R518");
            this.Manager.Checkpoint("MS-FRS2_R522");
            this.Manager.Checkpoint("MS-FRS2_R523");
            this.Manager.Checkpoint("MS-FRS2_R527");
            this.Manager.Checkpoint("MS-FRS2_R530");
            this.Manager.AddReturn(RequestVersionVectorInfo, null, temp173);
            TCtestScenario1S141();
            this.Manager.EndTest();
        }
        #endregion
    }
}
