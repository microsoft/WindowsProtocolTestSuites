// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Protocols.TestSuites.FileSharing.Common.Adapter;
using Microsoft.Protocols.TestSuites.FileSharing.FSA.Adapter;
using Microsoft.Protocols.TestTools;
using Microsoft.Protocols.TestTools.StackSdk;
using Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Fscc;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Protocols.TestSuites.FileSharing.FSA.TestSuite
{
    public partial class FsCtlTestCases : PtfTestClassBase
    {
        #region Test Cases

        [TestMethod()]
        [TestCategory(TestCategories.Fsa)]
        [TestCategory(TestCategories.IoCtlRequest)]
        [TestCategory(TestCategories.NonSmb)]
        [TestCategory(TestCategories.Positive)]
        [Description("Send FSCTL_QUERY_ALLOCATED_RANGES request to a file and check if AllocatedRanges are supported.")]
        public void FsCtl_Query_AllocatedRanges_File_IsAllocatedRangesSupported()
        {
            FsCtl_Query_AllocatedRanges_IsAllocatedRangesSupported(FileType.DataFile);
        }

        [TestMethod()]
        [TestCategory(TestCategories.Fsa)]
        [TestCategory(TestCategories.IoCtlRequest)]
        [TestCategory(TestCategories.NonSmb)]
        [TestCategory(TestCategories.Positive)]
        [Description("Send FSCTL_QUERY_ALLOCATED_RANGES request to a directory and check if AllocatedRanges are supported.")]
        public void FsCtl_Query_AllocatedRanges_Dir_IsAllocatedRangesSupported()
        {
            FsCtl_Query_AllocatedRanges_IsAllocatedRangesSupported(FileType.DirectoryFile);
        }

        #endregion

        #region Test Case Utility

        private void FsCtl_Query_AllocatedRanges_IsAllocatedRangesSupported(FileType fileType)
        {
            BaseTestSite.Log.Add(LogEntryKind.TestStep, "Test case steps:");
            MessageStatus status;

            //Step 1: Create file
            BaseTestSite.Log.Add(LogEntryKind.TestStep, "1. Create " + fileType.ToString());
            status = this.fsaAdapter.CreateFile(fileType);

            //Step 2: Write file
            long bytesToWrite;
            if (fileType == FileType.DataFile)
            {
                //Write some bytes into the DataFile.
                bytesToWrite = 1024;
                long bytesWritten = 0;
                BaseTestSite.Log.Add(LogEntryKind.TestStep, "2. Write file with " + bytesToWrite + " bytes data.");
                status = this.fsaAdapter.WriteFile(0, bytesToWrite, out bytesWritten);
            }
            else
            {
                BaseTestSite.Log.Add(LogEntryKind.TestStep, "2. Do not write data into DirectoryFile.");
                bytesToWrite = 0;
            }

            //Step 3: FSCTL request with FSCTL_QUERY_ALLOCATED_RANGES
            long byteCount;
            byte[] outputBuffer;
            FSCTL_QUERY_ALLOCATED_RANGES_Request queryAllocatedRangesRequest = new FSCTL_QUERY_ALLOCATED_RANGES_Request();
            queryAllocatedRangesRequest.FileOffset = 0;
            queryAllocatedRangesRequest.Length = bytesToWrite;
            uint outputBufferSize = (uint)TypeMarshal.ToBytes<FSCTL_QUERY_ALLOCATED_RANGES_Request>(queryAllocatedRangesRequest).Length;

            BaseTestSite.Log.Add(LogEntryKind.TestStep, "3. FSCTL request with FSCTL_QUERY_ALLOCATED_RANGES ");
            status = this.fsaAdapter.FsCtlQueryAllocatedRanges(queryAllocatedRangesRequest, outputBufferSize, out byteCount, out outputBuffer);

            //Step 4: Verify test result
            BaseTestSite.Log.Add(LogEntryKind.TestStep, "4. Verify returned NTStatus code.");
            if (this.fsaAdapter.IsQueryAllocatedRangesSupported == false)
            {
                this.fsaAdapter.AssertAreEqual(this.Manager, MessageStatus.INVALID_DEVICE_REQUEST, status, 
                    "If the object store does not implement this functionality, the operation MUST be failed with STATUS_INVALID_DEVICE_REQUEST.");
            }
            else
            {
                if (fileType == FileType.DataFile)
                {
                    FSCTL_QUERY_ALLOCATED_RANGES_Reply queryAllocatedRangesReply = TypeMarshal.ToStruct<FSCTL_QUERY_ALLOCATED_RANGES_Reply>(outputBuffer);

                    this.fsaAdapter.AssertAreEqual(this.Manager, bytesToWrite, queryAllocatedRangesReply.Length, 
                        "AllocatedRanges.Length is expected to same as the length of written data.");

                    this.fsaAdapter.AssertAreEqual(this.Manager, MessageStatus.SUCCESS, status,
                        "FSCTL_QUERY_ALLOCATED_RANGES is supported, status set to STATUS_SUCCESS.");
                }
                else
                {
                    this.fsaAdapter.AssertAreEqual(this.Manager, MessageStatus.INVALID_PARAMETER, status, 
                        "If Open.Stream.StreamType is DirectoryStream, the operation MUST be failed with STATUS_INVALID_PARAMETER.");
                }
            }
        }

        #endregion
    }
}
