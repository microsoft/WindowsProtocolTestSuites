// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Protocols.TestSuites.FileSharing.Common.Adapter;
using Microsoft.Protocols.TestSuites.FileSharing.Common.TestSuite;
using Microsoft.Protocols.TestTools;
using Microsoft.Protocols.TestTools.StackSdk;
using Microsoft.Protocols.TestTools.StackSdk.FileAccessService;
using Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Rsvd;
using Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Smb2;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Text;

namespace Microsoft.Protocols.TestSuites.FileSharing.RSVD.TestSuite
{
    [TestClass]
    public class ChangeTracking : RSVDTestBase
    {
        #region Test Suite Initialization

        // Use ClassInitialize to run code before running the first test in the class
        [ClassInitialize()]
        public static void ClassInitialize(TestContext testContext)
        {
            TestClassBase.Initialize(testContext);
        }

        // Use ClassCleanup to run code after all tests in a class have run
        [ClassCleanup()]
        public static void ClassCleanup()
        {
            TestClassBase.Cleanup();
        }

        #endregion

        [TestMethod]
        [TestCategory(TestCategories.Bvt)]
        [TestCategory(TestCategories.RsvdVersion2)]
        [TestCategory(TestCategories.NonSmb)]
        [Description("Check if server supports handling change tracking.")]
        public void BVT_ChangeTracking()
        {
            // The log file used in change tracking request must be an existing file.
            // So create one file before running the case.
            string logFileName = string.Format("log_{0}.txt", System.Guid.NewGuid());
            sutProtocolController.CreateFile(TestConfig.FullPathShareContainingSharedVHD, logFileName, "");

            BaseTestSite.Log.Add(LogEntryKind.TestStep, "1.	Client opens a shared virtual disk file and expects success.");
            OpenSharedVHD(TestConfig.NameOfSharedVHDS, RSVD_PROTOCOL_VERSION.RSVD_PROTOCOL_VERSION_2);

            BaseTestSite.Log.Add(LogEntryKind.TestStep, "2.	Client sends the tunnel operation SVHDX_CHANGE_TRACKING_START_REQUEST to start change tracking.");
            SVHDX_CHANGE_TRACKING_START_REQUEST request = new SVHDX_CHANGE_TRACKING_START_REQUEST();
            request.TransactionId = System.Guid.NewGuid();
            request.LogFileId = System.Guid.NewGuid();
            request.LogFileNameOffset = 56; 
            request.LogFileName = logFileName; 
            request.LogFileNameLength = (uint)(request.LogFileName.Length + 1) * 2;
            byte[] payload = client.CreateChangeTrackingStartRequest(request);
            SVHDX_TUNNEL_OPERATION_HEADER? header;
            SVHDX_TUNNEL_OPERATION_HEADER? response;
            //For RSVD_TUNNEL_META_OPERATION_START operation code, the IOCTL code should be FSCTL_SVHDX_ASYNC_TUNNEL_REQUEST
            uint status = client.TunnelOperation<SVHDX_TUNNEL_OPERATION_HEADER>(
                false,//true for Async operation, false for non-async operation
                RSVD_TUNNEL_OPERATION_CODE.RSVD_TUNNEL_CHANGE_TRACKING_START,
                ++RequestIdentifier,
                payload,
                out header,
                out response);
            BaseTestSite.Assert.AreEqual(
                (uint)Smb2Status.STATUS_SUCCESS,
                status,
                "Ioctl should succeed, actual status: {0}",
                GetStatus(status));
            VerifyTunnelOperationHeader(header.Value, RSVD_TUNNEL_OPERATION_CODE.RSVD_TUNNEL_CHANGE_TRACKING_START, (uint)RsvdStatus.STATUS_SVHDX_SUCCESS, RequestIdentifier);

            BaseTestSite.Log.Add(LogEntryKind.TestStep, "3.	Client sends Write request to change the vhds file.");
            payload = Smb2Utility.CreateRandomByteArray(512);
            status = client.Write(0, payload);

            BaseTestSite.Log.Add(LogEntryKind.TestStep, "4.	Client sends the tunnel operation RSVD_TUNNEL_CHANGE_TRACKING_GET_PARAMETERS to get the change tracking status.");
            SVHDX_CHANGE_TRACKING_GET_PARAMETERS_RESPONSE? getParamResponse;
            status = client.TunnelOperation<SVHDX_CHANGE_TRACKING_GET_PARAMETERS_RESPONSE>(
                false,//true for Async operation, false for non-async operation
                RSVD_TUNNEL_OPERATION_CODE.RSVD_TUNNEL_CHANGE_TRACKING_GET_PARAMETERS,
                ++RequestIdentifier,
                null,
                out header,
                out getParamResponse);

            VerifyTunnelOperationHeader(header.Value, RSVD_TUNNEL_OPERATION_CODE.RSVD_TUNNEL_CHANGE_TRACKING_GET_PARAMETERS, (uint)RsvdStatus.STATUS_SVHDX_SUCCESS, RequestIdentifier);
            BaseTestSite.Assert.AreEqual(
                RsvdStatus.STATUS_SVHDX_SUCCESS, 
                (RsvdStatus)getParamResponse.Value.ChangeTrackingStatus,
                "Status of change tracking should be STATUS_SVHDX_SUCCESS.");

            BaseTestSite.Log.Add(LogEntryKind.TestStep, "5.	Client sends the tunnel operation SVHDX_CHANGE_TRACKING_STOP_REQUEST to stop change tracking.");
            status = client.TunnelOperation<SVHDX_TUNNEL_OPERATION_HEADER>(
                false,//true for Async operation, false for non-async operation
                RSVD_TUNNEL_OPERATION_CODE.RSVD_TUNNEL_CHANGE_TRACKING_STOP,
                ++RequestIdentifier,
                null,
                out header,
                out response);
            VerifyTunnelOperationHeader(header.Value, RSVD_TUNNEL_OPERATION_CODE.RSVD_TUNNEL_CHANGE_TRACKING_STOP, (uint)RsvdStatus.STATUS_SVHDX_SUCCESS, RequestIdentifier);

            BaseTestSite.Log.Add(LogEntryKind.TestStep, "6.	Client closes the file.");
            client.CloseSharedVirtualDisk();
        }
    }
}
