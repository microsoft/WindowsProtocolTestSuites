// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.SpecExplorer.Runtime;
using System.Reflection;
using Microsoft.SpecExplorer;
using FRS2Model;

using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Protocols.TestTools;

namespace Microsoft.Protocols.TestSuites.MS_FRS2
{
    [TestClass]
    public class TraditionalTestCase : TestClassBase
    {

        #region Variables

        IFRS2ManagedAdapter iAdapter;

        #endregion

        #region Test Suite Initialization

        [ClassInitialize]
        public static void ClassInitialize(TestContext testContext)
        {
            TestClassBase.Initialize(testContext);
            ITestSite testSite = TestClassBase.BaseTestSite;
        }

        [ClassCleanup]
        public static void ClassCleanup()
        {
            TestClassBase.Cleanup();
        }

        #endregion

        #region Test Case Initialization

        protected override void TestInitialize()
        {
            // put here code which shall be run before every test case execution,
            // e.g. initialization of adapters:
            // protocolAdapter = Site.GetAdapter<IProtocolAdapter>();
            iAdapter = Site.GetAdapter<IFRS2ManagedAdapter>();
            FRS2ManagedAdapter.PreCheck();
            iAdapter.GeneralInitialize();
        }

        protected override void TestCleanup()
        {
            // put here code which shall be run after every test case execution,
            // e.g. reseting of adapters:
            // protocolAdapter.Reset(); v 
            iAdapter.Reset();
        }

        #endregion

        #region Test Cases

        [TestMethod, Timeout(600000)]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestCategory("AD")]
        [TestCategory("MS-FRS2")]
        [TestCategory("SDC")]
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]      
        public void FRS2_TraditionalRawGetFileDataAsync()
        {
            do
            {
                try
                {
                    iAdapter.InShutdown = false;
                    iAdapter.SetTraditionalTestFlag();
                    iAdapter.CheckConnectivity("A", 1);
                    ProtocolVersionReturned protocolVersion;
                    UpstreamFlagValueReturned upStreamFlags;
                    iAdapter.EstablishConnection("A", 1, ProtocolVersion.FRS_COMMUNICATION_PROTOCOL_VERSION_LONGHORN_SERVER, out protocolVersion, out upStreamFlags);
                    iAdapter.EstablishSession(1, 1);
                    iAdapter.AsyncPoll(1);
                    iAdapter.RequestVersionVector(22, 1, 1, VERSION_REQUEST_TYPE.REQUEST_NORMAL_SYNC, VERSION_CHANGE_TYPE.CHANGE_ALL, VVGeneration.ValidValue);
                    iAdapter.AsyncPoll(1);
                    iAdapter.RequestUpdates(1, 1, versionVectorDiff.valid);
                    iAdapter.InitializeFileTransferAsync(1, 1, false);
                    iAdapter.RawGetFileDataAsync();
                }
                catch (Frs2TsException)
                {
                    System.Threading.Thread.Sleep(60000);
                }
                //Not know why shutdown happens, so just retry as short term solution
            } while (iAdapter.InShutdown);
        }

        #endregion

        #region Test Cases

        [TestMethod, Timeout(600000)]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestCategory("AD")]
        [TestCategory("MS-FRS2")]
        [TestCategory("SDC")]
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        public void FRS2_TraditionalRdcGetFileDataAsync()
        {
            do
            {
                try
                {
                    iAdapter.InShutdown = false;
                    iAdapter.SetTraditionalTestFlag();
                    iAdapter.CheckConnectivity("A", 1);
                    ProtocolVersionReturned protocolVersion;
                    UpstreamFlagValueReturned upStreamFlags;
                    iAdapter.EstablishConnection("A", 1, ProtocolVersion.FRS_COMMUNICATION_PROTOCOL_VERSION_LONGHORN_SERVER, out protocolVersion, out upStreamFlags);
                    iAdapter.EstablishSession(1, 1);
                    iAdapter.AsyncPoll(1);
                    iAdapter.RequestVersionVector(22, 1, 1, VERSION_REQUEST_TYPE.REQUEST_NORMAL_SYNC, VERSION_CHANGE_TYPE.CHANGE_ALL, VVGeneration.ValidValue);
                    iAdapter.AsyncPoll(1);
                    iAdapter.RequestUpdates(1, 1, versionVectorDiff.valid);
                    iAdapter.InitializeFileTransferAsync(1, 1, true);
                    iAdapter.RdcGetFileDataAsync();
                }
                catch (Frs2TsException)
                {
                    System.Threading.Thread.Sleep(60000);
                }
                //Not know why shutdown happens, so just retry as short term solution
            } while (iAdapter.InShutdown);
        }

        #endregion

        #region Test Cases

        [TestMethod, Timeout(600000)]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestCategory("AD")]
        [TestCategory("MS-FRS2")]
        [TestCategory("SDC")]
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        public void FRS2_TraditionalRawGetFileData()
        {
            do
            {
                try
                {
                    iAdapter.InShutdown = false;
                    iAdapter.SetTraditionalTestFlag();
                    iAdapter.CheckConnectivity("A", 1);
                    ProtocolVersionReturned protocolVersion;
                    UpstreamFlagValueReturned upStreamFlags;
                    iAdapter.EstablishConnection("A", 1, ProtocolVersion.FRS_COMMUNICATION_PROTOCOL_VERSION_LONGHORN_SERVER, out protocolVersion, out upStreamFlags);
                    iAdapter.EstablishSession(1, 1);
                    iAdapter.AsyncPoll(1);
                    iAdapter.RequestVersionVector(22, 1, 1, VERSION_REQUEST_TYPE.REQUEST_NORMAL_SYNC, VERSION_CHANGE_TYPE.CHANGE_ALL, VVGeneration.ValidValue);
                    iAdapter.AsyncPoll(1);
                    iAdapter.RequestUpdates(1, 1, versionVectorDiff.valid);
                    iAdapter.InitializeFileTransferAsync(1, 1, false);
                    iAdapter.RawGetFileData();
                }
                catch (Frs2TsException)
                {
                    System.Threading.Thread.Sleep(60000);
                }
                //Not know why shutdown happens, so just retry as short term solution
            } while (iAdapter.InShutdown);
        }

        #endregion

        #region Test Case RDCGetFileData

        [TestMethod]
        public void FRS2_TraditionalRdcGetFileData()
        {
            iAdapter.SetTraditionalTestFlag();
            iAdapter.CheckConnectivity("A", 1);
            ProtocolVersionReturned protocolVersion;
            UpstreamFlagValueReturned upStreamFlags;
            iAdapter.EstablishConnection("A", 1, ProtocolVersion.FRS_COMMUNICATION_PROTOCOL_VERSION_LONGHORN_SERVER, out protocolVersion, out upStreamFlags);
            iAdapter.EstablishSession(1, 1);
            iAdapter.AsyncPoll(1);
            iAdapter.RequestVersionVector(22, 1, 1, VERSION_REQUEST_TYPE.REQUEST_NORMAL_SYNC, VERSION_CHANGE_TYPE.CHANGE_ALL, VVGeneration.ValidValue);
            iAdapter.AsyncPoll(1);
            iAdapter.RequestUpdates(1, 1, versionVectorDiff.valid);
            iAdapter.InitializeFileTransferAsync(1, 1, true);
            iAdapter.RdcGetSignatures(offset.valid);
            iAdapter.RdcPushSourceNeeds();
            iAdapter.RdcGetFileData(BufferSize.validBufSize);
        }

        #endregion

        #region Test Case RDCGetSignatures

        [TestMethod]
        public void FRS2_TraditionalRdcGetSignatures()
        {
            iAdapter.SetRdcGetSigTestFlag();
            iAdapter.CheckConnectivity("A", 1);
            ProtocolVersionReturned protocolVersion;
            UpstreamFlagValueReturned upStreamFlags;
            iAdapter.EstablishConnection("A", 1, ProtocolVersion.FRS_COMMUNICATION_PROTOCOL_VERSION_LONGHORN_SERVER, out protocolVersion, out upStreamFlags);
            iAdapter.EstablishSession(1, 1);
            iAdapter.AsyncPoll(1);
            iAdapter.RequestVersionVector(22, 1, 1, VERSION_REQUEST_TYPE.REQUEST_NORMAL_SYNC, VERSION_CHANGE_TYPE.CHANGE_ALL, VVGeneration.ValidValue);
            iAdapter.AsyncPoll(1);
            iAdapter.RequestUpdates(1, 1, versionVectorDiff.valid);
            iAdapter.InitializeFileTransferAsync(1, 1, true);
            iAdapter.RdcGetSignatures(offset.valid);
        }

        #endregion

        #region Test Case RdcPushSourceNeeds

        [TestMethod]
        public void FRS2_TraditionalRdcPushSourceNeeds()
        {
            iAdapter.SetPushSourceNeedsTestFlag();
            iAdapter.CheckConnectivity("A", 1);
            ProtocolVersionReturned protocolVersion;
            UpstreamFlagValueReturned upStreamFlags;
            iAdapter.EstablishConnection("A", 1, ProtocolVersion.FRS_COMMUNICATION_PROTOCOL_VERSION_LONGHORN_SERVER, out protocolVersion, out upStreamFlags);
            iAdapter.EstablishSession(1, 1);
            iAdapter.AsyncPoll(1);
            iAdapter.RequestVersionVector(22, 1, 1, VERSION_REQUEST_TYPE.REQUEST_NORMAL_SYNC, VERSION_CHANGE_TYPE.CHANGE_ALL, VVGeneration.ValidValue);
            //iAdapter.AsyncPoll(1);
            iAdapter.RequestUpdates(1, 1, versionVectorDiff.valid);
            iAdapter.InitializeFileTransferAsync(1, 1, true);
            iAdapter.RdcGetSignatures(offset.valid);
            iAdapter.RdcPushSourceNeeds();
        }

        #endregion

        #region Test Case RdcClose

        [TestMethod]
        public void FRS2_TraditionalTestCaseRdcClose()
        {
            iAdapter.SetRdcCloseTestFlag();
            iAdapter.CheckConnectivity("A", 1);
            ProtocolVersionReturned protocolVersion;
            UpstreamFlagValueReturned upStreamFlags;
            iAdapter.EstablishConnection("A", 1, ProtocolVersion.FRS_COMMUNICATION_PROTOCOL_VERSION_LONGHORN_SERVER, out protocolVersion, out upStreamFlags);
            iAdapter.EstablishSession(1, 1);
            iAdapter.AsyncPoll(1);
            iAdapter.RequestVersionVector(22, 1, 1, VERSION_REQUEST_TYPE.REQUEST_NORMAL_SYNC, VERSION_CHANGE_TYPE.CHANGE_ALL, VVGeneration.ValidValue);
            iAdapter.AsyncPoll(1);
            iAdapter.RequestUpdates(1, 1, versionVectorDiff.valid);
            iAdapter.InitializeFileTransferAsync(1, 1, false);
            iAdapter.RdcClose();
        }

        #endregion

        #region Test Case RDCGetSignatureFailure

        [TestMethod]
        public void FRS2_TraditionalTestCaseRDCGetSignatureFail()
        {
            iAdapter.SetRdcGetSigFailTestFlag();
            iAdapter.CheckConnectivity("A", 1);
            ProtocolVersionReturned protocolVersion;
            UpstreamFlagValueReturned upStreamFlags;
            iAdapter.EstablishConnection("A", 1, ProtocolVersion.FRS_COMMUNICATION_PROTOCOL_VERSION_LONGHORN_SERVER, out protocolVersion, out upStreamFlags);
            iAdapter.EstablishSession(1, 1);
            iAdapter.AsyncPoll(1);
            iAdapter.RequestVersionVector(22, 1, 1, VERSION_REQUEST_TYPE.REQUEST_NORMAL_SYNC, VERSION_CHANGE_TYPE.CHANGE_ALL, VVGeneration.ValidValue);
            iAdapter.AsyncPoll(1);
            iAdapter.RequestUpdates(1, 1, versionVectorDiff.valid);
            iAdapter.InitializeFileTransferAsync(1, 1, false);
            iAdapter.RdcGetSignatures(offset.valid);
        }

        #endregion

        #region Test Case RDCGetSignatureFailureFofInvalidLevel

        [TestMethod]
        public void FRS2_TraditionalTestCaseRDCGetSignatureFailForLevel()
        {
            iAdapter.SetRdcGetSigLevelTestFlag();
            iAdapter.CheckConnectivity("A", 1);
            ProtocolVersionReturned protocolVersion;
            UpstreamFlagValueReturned upStreamFlags;
            iAdapter.EstablishConnection("A", 1, ProtocolVersion.FRS_COMMUNICATION_PROTOCOL_VERSION_LONGHORN_SERVER, out protocolVersion, out upStreamFlags);
            iAdapter.EstablishSession(1, 1);
            iAdapter.AsyncPoll(1);
            iAdapter.RequestVersionVector(22, 1, 1, VERSION_REQUEST_TYPE.REQUEST_NORMAL_SYNC, VERSION_CHANGE_TYPE.CHANGE_ALL, VVGeneration.ValidValue);
            iAdapter.AsyncPoll(1);
            iAdapter.RequestUpdates(1, 1, versionVectorDiff.valid);
            iAdapter.InitializeFileTransferAsync(1, 1, true);
            iAdapter.RdcGetSignatures(offset.valid);
        }

        #endregion

        #region Test Case RdcPushSourceNeeds

        [TestMethod]
        public void FRS2_TraditionalTestCaseRdcPushSourceNeedsForNeedCount()
        {
            iAdapter.SetPushSourceNeedsTestFlagForNeedCount();
            iAdapter.CheckConnectivity("A", 1);
            ProtocolVersionReturned protocolVersion;
            UpstreamFlagValueReturned upStreamFlags;
            iAdapter.EstablishConnection("A", 1, ProtocolVersion.FRS_COMMUNICATION_PROTOCOL_VERSION_LONGHORN_SERVER, out protocolVersion, out upStreamFlags);
            iAdapter.EstablishSession(1, 1);
            iAdapter.AsyncPoll(1);
            iAdapter.RequestVersionVector(22, 1, 1, VERSION_REQUEST_TYPE.REQUEST_NORMAL_SYNC, VERSION_CHANGE_TYPE.CHANGE_ALL, VVGeneration.ValidValue);
            //iAdapter.AsyncPoll(1);
            iAdapter.RequestUpdates(1, 1, versionVectorDiff.valid);
            iAdapter.InitializeFileTransferAsync(1, 1, true);
            iAdapter.RdcGetSignatures(offset.valid);
            iAdapter.RdcPushSourceNeeds();
        }

        #endregion

    }
}
