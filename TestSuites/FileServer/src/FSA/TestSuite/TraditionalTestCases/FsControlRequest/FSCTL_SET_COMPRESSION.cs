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
        [Description("Send FSCTL_SET_COMPRESSION request to a file and check if Compression is supported.")]
        public void FsCtl_Set_Compression_File_IsCompressionSupported()
        {
            FsCtl_Set_Compression_IsCompressionSupported(FileType.DataFile);
        }

        [TestMethod()]
        [TestCategory(TestCategories.Fsa)]
        [TestCategory(TestCategories.IoCtlRequest)]
        [TestCategory(TestCategories.NonSmb)]
        [TestCategory(TestCategories.Positive)]
        [Description("Send FSCTL_SET_COMPRESSION request to a directory and check if Compression is supported.")]
        public void FsCtl_Set_Compression_Dir_IsCompressionSupported()
        {
            FsCtl_Set_Compression_IsCompressionSupported(FileType.DirectoryFile);
        }

        #endregion

        #region Test Case Utility

        private void FsCtl_Set_Compression_IsCompressionSupported(FileType fileType)
        {
            BaseTestSite.Log.Add(LogEntryKind.TestStep, "Test case steps:");
            MessageStatus status;

            //Step 1: Create file
            BaseTestSite.Log.Add(LogEntryKind.TestStep, "1. Create " + fileType.ToString());
            status = this.fsaAdapter.CreateFile(fileType);

            //Step 2: FSCTL request with FSCTL_SET_COMPRESSION
            FSCTL_SET_COMPRESSION_Request setCompressionRequest = new FSCTL_SET_COMPRESSION_Request();
            setCompressionRequest.CompressionState = FSCTL_SET_COMPRESSION_Request_CompressionState_Values.COMPRESSION_FORMAT_DEFAULT;
            uint inputBufferSize = (uint)TypeMarshal.ToBytes<FSCTL_SET_COMPRESSION_Request>(setCompressionRequest).Length;

            BaseTestSite.Log.Add(LogEntryKind.TestStep, "2. FSCTL request with FSCTL_SET_COMPRESSION");
            status = this.fsaAdapter.FsCtlSetCompression(setCompressionRequest, inputBufferSize);

            //Step 3: Verify test result
            BaseTestSite.Log.Add(LogEntryKind.TestStep, "3. Verify returned NTSTATUS code.");
            if (this.fsaAdapter.IsCompressionSupported == false)
            {
                if (this.fsaAdapter.FileSystem == FileSystem.REFS)
                {
                    this.fsaAdapter.AssertAreEqual(this.Manager, MessageStatus.NOT_SUPPORTED, status,
                        ("For ReFS, it is only supported and returns STATUS_SUCCESS when CompressionState is set to COMPRESSION_FORMAT_NONE. " + 
                         "The method fails with STATUS_NOT_SUPPORTED for any other value of CompressionState."));
                }
                else
                {
                    this.fsaAdapter.AssertAreEqual(this.Manager, MessageStatus.INVALID_DEVICE_REQUEST, status, 
                        "If the object store does not implement this functionality, the operation MUST be failed with STATUS_INVALID_DEVICE_REQUEST.");
                }
            }
            else
            {
                if (fileType == FileType.DirectoryFile && this.fsaAdapter.FileSystem == FileSystem.CSVFS)
                {
                    this.fsaAdapter.AssertAreEqual(this.Manager, MessageStatus.ACCESS_DENIED, status, "CSVFS does not support setting compressed attribute on the folders.");
                }
                else
                {
                    this.fsaAdapter.AssertAreEqual(this.Manager, MessageStatus.SUCCESS, status, "COMPRESSION is supported, status set to STATUS_SUCCESS.");
                }
            }
        }

        #endregion
    }
}
