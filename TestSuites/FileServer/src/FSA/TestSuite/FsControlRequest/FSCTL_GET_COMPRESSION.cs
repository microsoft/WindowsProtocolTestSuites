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
        #region Test cases

        [TestMethod()]
        [TestCategory(TestCategories.Fsa)]
        [TestCategory(TestCategories.IoCtlRequest)]
        [TestCategory(TestCategories.NonSmb)]
        [TestCategory(TestCategories.Positive)]
        [Description("Send FSCTL_GET_COMPRESSION request to a file and check if Compression is supported.")]
        public void FsCtl_Get_Compression_File_IsCompressionSupported()
        {
            FsCtl_Get_Compression_IsCompressionSupported(FileType.DataFile);       
        }

        [TestMethod()]
        [TestCategory(TestCategories.Fsa)]
        [TestCategory(TestCategories.IoCtlRequest)]
        [TestCategory(TestCategories.NonSmb)]
        [TestCategory(TestCategories.Positive)]
        [Description("Send FSCTL_GET_COMPRESSION request to a directory and check if Compression is supported.")]
        public void FsCtl_Get_Compression_Dir_IsCompressionSupported()
        {
            FsCtl_Get_Compression_IsCompressionSupported(FileType.DirectoryFile);
        }

        #endregion

        #region Test Case Utility

        private void FsCtl_Get_Compression_IsCompressionSupported(FileType fileType)
        {
            BaseTestSite.Log.Add(LogEntryKind.TestStep, "Test case steps:");
            MessageStatus status;

            //Step 1: Create file
            BaseTestSite.Log.Add(LogEntryKind.TestStep, "1. Create " + fileType.ToString());
            status = this.fsaAdapter.CreateFile(fileType);

            //Step 2: FSCTL request with FSCTL_GET_COMPRESSION
            FSCTL_GET_COMPRESSION_Reply compressionReply = new FSCTL_GET_COMPRESSION_Reply();
            uint outputBufferSize = (uint)TypeMarshal.ToBytes<FSCTL_GET_COMPRESSION_Reply>(compressionReply).Length;

            long bytesReturned;
            byte[] outputBuffer = new byte[0];

            BaseTestSite.Log.Add(LogEntryKind.TestStep, "2. FSCTL request with FSCTL_GET_COMPRESSION");
            status = this.fsaAdapter.FsCtlGetCompression(outputBufferSize, out bytesReturned, out outputBuffer);

            //Step 3: Verify test result
            BaseTestSite.Log.Add(LogEntryKind.TestStep, "3. Verify returned NTSTATUS code.");
            // 2.1.5.9.7   FSCTL_GET_COMPRESSION
            // <64> Section 2.1.5.9.7: This is only implemented by the NTFS and ReFS file systems.
            if (this.fsaAdapter.FileSystem == FileSystem.NTFS || this.fsaAdapter.FileSystem == FileSystem.REFS)
            {
                this.fsaAdapter.AssertAreEqual(this.Manager, MessageStatus.SUCCESS, status, "FSCTL_GET_COMPRESSION is supported, status set to STATUS_SUCCESS.");
            }
            else
            {
                this.fsaAdapter.AssertAreEqual(this.Manager, MessageStatus.INVALID_DEVICE_REQUEST, status,
                        "If the object store does not implement this functionality, the operation MUST be failed with STATUS_INVALID_DEVICE_REQUEST.");
            }
        }

        #endregion
    }
}
