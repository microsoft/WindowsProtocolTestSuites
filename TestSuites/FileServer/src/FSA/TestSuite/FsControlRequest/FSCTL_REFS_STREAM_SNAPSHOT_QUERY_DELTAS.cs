// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Protocols.TestSuites.FileSharing.Common.Adapter;
using Microsoft.Protocols.TestSuites.FileSharing.FSA.Adapter;
using Microsoft.Protocols.TestTools;
using Microsoft.Protocols.TestTools.StackSdk;
using Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Fscc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Text;

namespace Microsoft.Protocols.TestSuites.FileSharing.FSA.TestSuite
{
    public partial class FsCtlTestCases : PtfTestClassBase
    {
        #region Test cases
        [TestMethod()]
        [TestCategory(TestCategories.Bvt)]
        [TestCategory(TestCategories.Fsa)]
        [TestCategory(TestCategories.IoCtlRequest)]
        [TestCategory(TestCategories.NonSmb)]
        [Description("Send FSCTL_REFS_STREAM_SNAPSHOT_MANAGEMENT request with REFS_STREAM_SNAPSHOT_OPERATION_QUERY_DELTAS to modified data file and verify extent returned.")]
        public void BVT_FsCtl_RefsStreamSnapshotOperation_QueryDeltas_Positive()
        {
            Fsctl_Refs_Stream_Snapshot_Operation_QueryDeltas(true);
        }

        [TestMethod()]
        [TestCategory(TestCategories.Fsa)]
        [TestCategory(TestCategories.IoCtlRequest)]
        [TestCategory(TestCategories.NonSmb)]
        [TestCategory(TestCategories.UnexpectedFields)]
        [Description("Send FSCTL_REFS_STREAM_SNAPSHOT_MANAGEMENT request with REFS_STREAM_SNAPSHOT_OPERATION_QUERY_DELTAS to unmodified data file and verify no extent returned.")]
        public void FsCtl_RefsStreamSnapshotOperation_QueryDeltas_Negative()
        {
            Fsctl_Refs_Stream_Snapshot_Operation_QueryDeltas(false);
        }
        #endregion

        #region Utility
        public void Fsctl_Refs_Stream_Snapshot_Operation_QueryDeltas(bool modifyFile)
        {
            BaseTestSite.Log.Add(LogEntryKind.TestStep, "Test case steps:");
            string fileName = this.fsaAdapter.ComposeRandomFileName((int)Test_Lengths.ALIGN);

            //Step 1: Create file
            BaseTestSite.Log.Add(LogEntryKind.TestStep, "1. Create Data File.");
            CreateFile(fileName);

            //Step 2: Create snapshot
            BaseTestSite.Log.Add(LogEntryKind.TestStep, "2. Create snapshot");
            string snapshotName = "snapshotBaseName";
            CreateRefsStreamSnapshot(snapshotName, MessageStatus.SUCCESS);

            //Step 3: Modify file
            BaseTestSite.Log.Add(LogEntryKind.TestStep, "3. Modify file if required");
            if (modifyFile)
            {
                WriteDataToFile();
            }

            //Step 4: Query and verify extent metadata.
            BaseTestSite.Log.Add(LogEntryKind.TestStep, "4. Query and verify extent metadata.");

            byte[] snapshotNameByte = Encoding.Unicode.GetBytes(snapshotName);
            ushort snapshotNameByteLength = (ushort)snapshotNameByte.Length;

            REFS_STREAM_SNAPSHOT_QUERY_DELTAS_INPUT_BUFFER refsStreamSnapshotQueryDeltasInputBuffer = GetQueryDeltasInputBuffer();
            byte[] queryDetalsInputBuffer = TypeMarshal.ToBytes(refsStreamSnapshotQueryDeltasInputBuffer);
            ushort queryDetalsInputBufferLength = (ushort)queryDetalsInputBuffer.Length;

            byte[] nameAndInputBuffer = new byte[snapshotNameByteLength + queryDetalsInputBufferLength];
            Buffer.BlockCopy(snapshotNameByte, 0, nameAndInputBuffer, 0, snapshotNameByteLength);
            Buffer.BlockCopy(queryDetalsInputBuffer, 0, nameAndInputBuffer, snapshotNameByteLength, queryDetalsInputBufferLength);
            REFS_STREAM_SNAPSHOT_MANAGEMENT_INPUT_BUFFER refsStreamSnapshotManagementInput =
                GetRefsStreamSnapshotManagement(RefsStreamSnapshotOperation_Values.REFS_STREAM_SNAPSHOT_OPERATION_QUERY_DELTAS,
                snapshotNameLength: snapshotNameByteLength, operationInputBufferLength: queryDetalsInputBufferLength, nameAndInputBuffer: nameAndInputBuffer);

            uint outputBufferSize = this.fsaAdapter.transBufferSize;
            byte[] outputBuffer;

            MessageStatus status = this.fsaAdapter.FsCtlRefsStreamSnapshotManagement(refsStreamSnapshotManagementInput,
                outputBufferSize, out _, out outputBuffer);

            //MS-SMB2 <353> Windows 10 v21H1 and later and Windows Server 2022 and later allow the additional CtlCode value, 
            //as specified in [MS-FSCC].
            if (this.fsaAdapter.TestConfig.Platform < Platform.Windows10V21H1)
            {
                this.fsaAdapter.AssertAreEqual(this.Manager, MessageStatus.NOT_SUPPORTED, status,
                    "The operation as requested is not supported, or the file system does not support snapshot operations.");
            }
            else if (this.fsaAdapter.IsStreamSnapshotManagementImplemented == false)
            {
                this.fsaAdapter.AssertAreEqual(this.Manager, MessageStatus.INVALID_DEVICE_REQUEST, status,
                    "If the object store does not implement this functionality, the operation MUST be failed with STATUS_INVALID_DEVICE_REQUEST.");
            }
            else
            {
                //Verify that QUERY DELTAS operation succeeds
                this.fsaAdapter.AssertAreEqual(this.Manager, MessageStatus.SUCCESS, status, "Snapshot Operation query deltas should succeed");

                //Verify that meta detas are returned
                uint defaultOutputBufferSize = 16;
                ulong extentsCount = 0;
                ulong zeroExtentReturned = 0;

                if (outputBuffer != null && outputBuffer.Length > defaultOutputBufferSize)
                {
                    REFS_STREAM_SNAPSHOT_QUERY_DELTAS_OUTPUT_BUFFER queryDeltasOutpuBuffer = TypeMarshal.ToStruct<REFS_STREAM_SNAPSHOT_QUERY_DELTAS_OUTPUT_BUFFER>(outputBuffer);
                    extentsCount = queryDeltasOutpuBuffer.Extents.Length;
                }

                if (modifyFile)
                {
                    BaseTestSite.Assert.AreNotEqual(zeroExtentReturned, extentsCount, "Number of extent should be greater than zero");
                }
                else
                {
                    BaseTestSite.Assert.AreEqual(zeroExtentReturned, extentsCount, $"Number of extent should be zero");
                }
            }
        }

        #endregion
    }
}
