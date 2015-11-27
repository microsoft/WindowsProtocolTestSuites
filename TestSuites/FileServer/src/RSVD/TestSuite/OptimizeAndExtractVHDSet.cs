﻿// Copyright (c) Microsoft. All rights reserved.
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
    public class OptimizeAndExtractVHDSet : RSVDTestBase
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
        [Description("Check if server supports handling tunnel operation to extract the target VHD set file.")]
        public void BVT_Extract_VHDSet()
        {
            ulong requestId = 0;
            BaseTestSite.Log.Add(LogEntryKind.TestStep, "1.	Client opens a shared virtual disk file and expects success.");
            OpenSharedVHD(TestConfig.NameOfSharedVHDS, requestId);

            BaseTestSite.Log.Add(LogEntryKind.TestStep, "2.	Client sends tunnel operation SVHDX_META_OPERATION_START_REQUEST to server and expects success.");

            System.Guid snapshotId = CreateSnapshot(ref requestId, client);
            SVHDX_META_OPERATION_START_REQUEST startRequest = new SVHDX_META_OPERATION_START_REQUEST();
            startRequest.TransactionId = System.Guid.NewGuid();
            startRequest.OperationType = Operation_Type.SvhdxMetaOperationTypeExtractVHD;
            startRequest.Padding = new byte[4];
            SVHDX_META_OPERATION_EXTRACT extract = new SVHDX_META_OPERATION_EXTRACT();
            extract.snapshotType = Snapshot_Type.SvhdxSnapshotTypeVM;
            extract.Padding = new byte[4];
            extract.flags = ExtractSnapshot_Flags.SVHDX_EXTRACT_SNAPSHOTS_FLAG_ZERO;
            extract.SourceSnapshotId = snapshotId;
            extract.SourceLimitSnapshotId = snapshotId;
            extract.DestinationFileName = Encoding.Unicode.GetBytes(System.Guid.NewGuid().ToString() + "\0");
            extract.DestinationFileNameLength = (uint)extract.DestinationFileName.Length;
            extract.padding = new byte[4];
            
            byte[] payload = client.CreateTunnelMetaOperationStartExtractRequest(
                startRequest,
                extract);
      
            SVHDX_TUNNEL_OPERATION_HEADER? header;
            SVHDX_TUNNEL_OPERATION_HEADER? response;
            //For RSVD_TUNNEL_META_OPERATION_START operation code, the IOCTL code should be FSCTL_SVHDX_ASYNC_TUNNEL_REQUEST
            uint status = client.TunnelOperation<SVHDX_TUNNEL_OPERATION_HEADER>(
                true, //true for Async operation, false for non-async operation
                RSVD_TUNNEL_OPERATION_CODE.RSVD_TUNNEL_META_OPERATION_START,
                requestId,
                payload,
                out header,
                out response);
            BaseTestSite.Assert.AreEqual(
                (uint)Smb2Status.STATUS_SUCCESS,
                status,
                "Ioctl should succeed, actual status: {0}",
                GetStatus(status));
            VerifyTunnelOperationHeader(header.Value, RSVD_TUNNEL_OPERATION_CODE.RSVD_TUNNEL_META_OPERATION_START, (uint)RsvdStatus.STATUS_SVHDX_SUCCESS, requestId++);

            DeleteSnapshot(ref requestId, snapshotId, client);

            BaseTestSite.Log.Add(LogEntryKind.TestStep, "3.	Client closes the file.");
            client.CloseSharedVirtualDisk();

        }
    }
}
