// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Threading;
using System.Diagnostics;
using Microsoft.Protocols.TestTools;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Protocols.TestSuites.FileSharing.Common.Adapter;
using Microsoft.Protocols.TestSuites.FileSharing.Common.TestSuite;
using Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Smb2;

namespace Microsoft.Protocols.TestSuites.FileSharing.SQOS.TestSuite
{
    [TestClass]
    public class SqosBasic : SqosTestBase
    {
        #region Test Initialize and Cleanup
        [ClassInitialize()]
        public static void ClassInitialize(TestContext testContext)
        {
            TestClassBase.Initialize(testContext);
        }

        [ClassCleanup()]
        public static void ClassCleanup()
        {
            TestClassBase.Cleanup();
        }
        #endregion

        [TestMethod]
        [TestCategory(TestCategories.Bvt)]
        [TestCategory(TestCategories.Sqos)]
        [Description("This test case is to test if server can handle an SQOS request to set policy to a logical flow")]
        public void BVT_Sqos_SetPolicy()
        {
            ConnectToVHD();
            Guid logicalFlowId = Guid.NewGuid();
            Guid initiatorId = Guid.NewGuid();
            AssociateOpenToLogicalFlow(logicalFlowId);
            SetOrProbePolicy(logicalFlowId, initiatorId, setPolicy:true);

            // The server needs a few seconds to build up the set of known clients and the related statistics.
            // Resend the request until the max and min rates are computed.
            BaseTestSite.Log.Add(LogEntryKind.TestStep, "Client sends an SQOS request to query status to a logical flow and expects success");
            DoUntilSucceed(
                () => GetStatus(initiatorId, logicalFlowId),
                TestConfig.Timeout,
                "Retry querying the logic flow status until succeed within timeout span");
        }

        [TestMethod]
        [TestCategory(TestCategories.Bvt)]
        [TestCategory(TestCategories.Sqos)]
        [Description("This test case is to test if server can handle an SQOS request to probe policy to a logical flow")]
        public void BVT_Sqos_ProbePolicy()
        {
            ConnectToVHD();
            Guid logicalFlowId = Guid.NewGuid();
            Guid initiatorId = Guid.NewGuid();
            SetOrProbePolicy(logicalFlowId, initiatorId, setPolicy: false);

            // The server needs a few seconds to build up the set of known clients and the related statistics.
            // Resend the request until the max and min rates are computed.
            BaseTestSite.Log.Add(LogEntryKind.TestStep, "Client sends an SQOS request to query status to a logical flow and expects success");
            DoUntilSucceed(
                () => GetStatus(initiatorId, logicalFlowId),
                TestConfig.Timeout,
                "Retry querying the logic flow status until succeed within timeout span");
        }

        [TestMethod]
        [TestCategory(TestCategories.Bvt)]
        [TestCategory(TestCategories.Sqos)]
        [Description("This test case is to test if server can handle an SQOS request to update counters to a logical flow")]
        public void BVT_Sqos_UpdateCounters()
        {
            ConnectToVHD();
            Guid logicalFlowId = Guid.NewGuid();
            Guid initiatorId = Guid.NewGuid();
            AssociateOpenToLogicalFlow(logicalFlowId);
            SetOrProbePolicy(logicalFlowId, initiatorId, setPolicy: true);

            byte[] payload;
            BaseTestSite.Log.Add(LogEntryKind.TestStep, "Client reads 8k bytes");

            // A single tick represents one hundred nanoseconds
            // So the tick unit is the same with latencyIncrement and lowerLatencyIncrement
            Stopwatch sw = Stopwatch.StartNew();
            sw.Start();
            client.Read(treeId, fileId, 0, TestConfig.SqosBaseIoSize, out payload);
            sw.Stop();

            // The difference of latencyIncrement and lowerLatencyIncrement is that if they include or exclude any delay accumulated 
            // by I/O requests in the initiator’s queues while waiting to be issued to lower layers
            // In this case, no delay in initiator queue since there's only one I/O request.
            // So the two value is the same.
            UpdateCounters(logicalFlowId, initiatorId, 1, 1, (ulong)sw.ElapsedTicks, (ulong)sw.ElapsedTicks);
        }

        private void AssociateOpenToLogicalFlow(Guid logicalFlowId)
        {
            BaseTestSite.Log.Add(LogEntryKind.TestStep, "Client sends an SQOS request to associate the Open to a logical flow and expects success");
            STORAGE_QOS_CONTROL_Response? sqosResponse;
            client.SendAndReceiveSqosPacket(
                treeId,
                fileId,
                SqosOptions_Values.STORAGE_QOS_CONTROL_FLAG_SET_LOGICAL_FLOW_ID,
                logicalFlowId,
                Guid.Empty,
                Guid.Empty,
                string.Empty,
                string.Empty,
                out sqosResponse);
        }

        private void SetOrProbePolicy(Guid logicalFlowId, Guid initiatorId, bool setPolicy)
        {
            BaseTestSite.Log.Add(LogEntryKind.TestStep, "Client sends an SQOS request to {0} policy to a logical flow and expects success", setPolicy ? "set" : "probe");
            STORAGE_QOS_CONTROL_Response? sqosResponse;
            client.SendAndReceiveSqosPacket(
                treeId,
                fileId,
                setPolicy ? SqosOptions_Values.STORAGE_QOS_CONTROL_FLAG_SET_POLICY : SqosOptions_Values.STORAGE_QOS_CONTROL_FLAG_PROBE_POLICY,
                logicalFlowId,
                TestConfig.SqosPolicyId,
                initiatorId,
                TestConfig.SqosInitiatorName,
                TestConfig.SqosInitiatorNodeName,
                out sqosResponse);
        }

        private void UpdateCounters(
            Guid logicalFlowId,
            Guid initiatorId,
            ulong ioCountIncrement,
            ulong normalizedIoCountIncrement,
            ulong latencyIncrement,
            ulong lowerLatencyIncrement)
        {
            BaseTestSite.Log.Add(LogEntryKind.TestStep, "Client sends an SQOS request to update counters to a logical flow and expects success");
            STORAGE_QOS_CONTROL_Response? sqosResponse;
            client.SendAndReceiveSqosPacket(
                treeId,
                fileId,
                SqosOptions_Values.STORAGE_QOS_CONTROL_FLAG_UPDATE_COUNTERS,
                logicalFlowId,
                TestConfig.SqosPolicyId,
                initiatorId,
                TestConfig.SqosInitiatorName,
                TestConfig.SqosInitiatorNodeName,
                out sqosResponse,
                ioCountIncrement,
                normalizedIoCountIncrement,
                latencyIncrement,
                lowerLatencyIncrement);
        }

        private void GetStatus(Guid initiatorId, Guid logicalFlowId)
        {
            STORAGE_QOS_CONTROL_Response? sqosResponse;
            client.SendAndReceiveSqosPacket(
                treeId,
                fileId,
                SqosOptions_Values.STORAGE_QOS_CONTROL_FLAG_GET_STATUS,
                logicalFlowId,
                TestConfig.SqosPolicyId,
                initiatorId,
                TestConfig.SqosInitiatorName,
                TestConfig.SqosInitiatorNodeName,
                out sqosResponse);

            if (sqosResponse.Value.MaximumIoRate != TestConfig.SqosMaximumIoRate)
            {
                throw new Exception(String.Format("MaximumRate should be {0}, not {1}, retry querying logical flow status in case the server is not ready.", 
                    TestConfig.SqosMaximumIoRate, sqosResponse.Value.MaximumIoRate));
            }

            BaseTestSite.Assert.IsNotNull(sqosResponse,
                "Server should return STORAGE_QOS_CONTROL_RESPONSE when Request.Options includes the STORAGE_QOS_CONTROL_GET_STATUS flag");

            BaseTestSite.Assert.AreEqual(SqosConst.STORAGE_QOS_VERSION_1, sqosResponse.Value.ProtocolVersion, "ProtocolVersion must be set to 0x0100");
            BaseTestSite.Assert.AreEqual(SqosOptions_Values.STORAGE_QOS_CONTROL_FLAG_NONE, sqosResponse.Value.Options, "Options must be set to 0");
            BaseTestSite.Assert.AreEqual(logicalFlowId, sqosResponse.Value.LogicalFlowID, "LogicalFlowID MUST be set to {0}", logicalFlowId);
            BaseTestSite.Assert.AreEqual(TestConfig.SqosPolicyId, sqosResponse.Value.PolicyID, "PolicyID MUST be set to {0}", TestConfig.SqosPolicyId);
            BaseTestSite.Assert.AreEqual(initiatorId, sqosResponse.Value.InitiatorID, "InitiatorID MUST be set to {0}", initiatorId);
            BaseTestSite.Assert.AreNotEqual((uint)0, sqosResponse.Value.TimeToLive, "TimeToLive MUST be set to a positive value");
            BaseTestSite.Assert.AreEqual(LogicalFlowStatus.StorageQoSStatusOk, sqosResponse.Value.Status, "Status MUST be StorageQoSStatusOk");
            BaseTestSite.Assert.AreEqual(TestConfig.SqosMaximumIoRate, sqosResponse.Value.MaximumIoRate, "MaximumRate MUST be {0}", TestConfig.SqosMaximumIoRate);
            BaseTestSite.Assert.AreEqual(TestConfig.SqosMinimumIoRate, sqosResponse.Value.MinimumIoRate, "MinimumIoRate MUST be {0}", TestConfig.SqosMinimumIoRate);
            BaseTestSite.Assert.AreEqual(TestConfig.SqosBaseIoSize, sqosResponse.Value.BaseIoSize, "BaseIoSize MUST be {0}", TestConfig.SqosBaseIoSize);
        }
    }
}