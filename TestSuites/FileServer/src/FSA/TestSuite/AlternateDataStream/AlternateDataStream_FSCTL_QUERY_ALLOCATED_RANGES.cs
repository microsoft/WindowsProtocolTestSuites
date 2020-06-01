// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Protocols.TestSuites.FileSharing.Common.Adapter;
using Microsoft.Protocols.TestSuites.FileSharing.FSA.Adapter;
using Microsoft.Protocols.TestTools;
using Microsoft.Protocols.TestTools.StackSdk;
using Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Fscc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace Microsoft.Protocols.TestSuites.FileSharing.FSA.TestSuite
{
    public partial class AlternateDataStreamTestCases : PtfTestClassBase
    {
        #region Test Cases

        [TestMethod()]
        [TestCategory(TestCategories.Fsa)]
        [TestCategory(TestCategories.IoCtlRequest)]
        [TestCategory(TestCategories.AlternateDataStream)]
        [TestCategory(TestCategories.NonSmb)]
        [TestCategory(TestCategories.Positive)]
        [Description("FSCTL_QUERY_ALLOCATED_RANGES from an Alternate Data Stream on a DataFile.")]
        public void AlternateDataStream_FsCtl_Query_AllocatedRanges_File()
        {
            AlternateDataStream_CreateStream(FileType.DataFile);

            AlternateDataStream_FsCtl_Query_AllocatedRanges(FileType.DataFile);
        }

        [TestMethod()]
        [TestCategory(TestCategories.Fsa)]
        [TestCategory(TestCategories.IoCtlRequest)]
        [TestCategory(TestCategories.AlternateDataStream)]
        [TestCategory(TestCategories.NonSmb)]
        [TestCategory(TestCategories.Positive)]
        [Description("FSCTL_QUERY_ALLOCATED_RANGES from an Alternate Data Stream on a DirectoryFile.")]
        public void AlternateDataStream_FsCtl_Query_AllocatedRanges_Dir()
        {
            AlternateDataStream_CreateStream(FileType.DirectoryFile);

            AlternateDataStream_FsCtl_Query_AllocatedRanges(FileType.DirectoryFile);
        }

        #endregion

        #region Test Case Utility

        private void AlternateDataStream_FsCtl_Query_AllocatedRanges(FileType fileType)
        {
            //Prerequisites: Create streams on a newly created file

            //Step 1: FSCTL request with FSCTL_QUERY_ALLOCATED_RANGES
            long byteCount;
            byte[] outputBuffer;
            FSCTL_QUERY_ALLOCATED_RANGES_Request queryAllocatedRangesRequest = new FSCTL_QUERY_ALLOCATED_RANGES_Request();
            queryAllocatedRangesRequest.FileOffset = 0;
            queryAllocatedRangesRequest.Length = dataStreamList[":" + dataStreamName1 + ":$DATA"];
            uint outputBufferSize = (uint)TypeMarshal.ToBytes<FSCTL_QUERY_ALLOCATED_RANGES_Request>(queryAllocatedRangesRequest).Length;

            BaseTestSite.Log.Add(LogEntryKind.TestStep, "{0}. FSCTL request with FSCTL_QUERY_ALLOCATED_RANGES", ++testStep);
            status = this.fsaAdapter.FsCtlQueryAllocatedRanges(queryAllocatedRangesRequest, outputBufferSize, out byteCount, out outputBuffer);

            //Step 2: Verify test result
            BaseTestSite.Log.Add(LogEntryKind.TestStep, "{0}. Verify returned NTStatus code.", ++testStep);
            if (this.fsaAdapter.IsQueryAllocatedRangesSupported == false)
            {
                this.fsaAdapter.AssertAreEqual(this.Manager, MessageStatus.INVALID_DEVICE_REQUEST, status,
                    "If the object store does not implement this functionality, the operation MUST be failed with STATUS_INVALID_DEVICE_REQUEST.");
            }
            else
            {
                FSCTL_QUERY_ALLOCATED_RANGES_Reply queryAllocatedRangesReply = TypeMarshal.ToStruct<FSCTL_QUERY_ALLOCATED_RANGES_Reply>(outputBuffer);

                this.fsaAdapter.AssertAreEqual(this.Manager, dataStreamList[":" + dataStreamName1 + ":$DATA"], queryAllocatedRangesReply.Length,
                    "AllocatedRanges.Length is expected to same as the length of written data.");

                this.fsaAdapter.AssertAreEqual(this.Manager, MessageStatus.SUCCESS, status,
                    "FSCTL_QUERY_ALLOCATED_RANGES is supported, status set to STATUS_SUCCESS.");
            }
        }

        #endregion

    }
}