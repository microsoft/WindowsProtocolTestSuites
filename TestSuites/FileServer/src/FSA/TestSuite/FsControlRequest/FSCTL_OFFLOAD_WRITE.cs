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
        [TestCategory(TestCategories.Bvt)]
        [TestCategory(TestCategories.Fsa)]
        [TestCategory(TestCategories.IoCtlRequest)]
        [TestCategory(TestCategories.NonSmb)]
        [Description("Send FSCTL_OFFLOAD_WRITE request to a file and check if copy offload is supported.")]
        public void FsCtl_Offload_Write_File_IsOffloadSupported()
        {
            BaseTestSite.Log.Add(LogEntryKind.TestStep, "Test case steps:");
            MessageStatus status;

            //Step 1: Create file
            BaseTestSite.Log.Add(LogEntryKind.TestStep, "1. Create Data File.");
            status = this.fsaAdapter.CreateFile(FileType.DataFile);

            //Step 2: FSCTL request with FSCTL_OFFLOAD_WRITE
            long bytesReturned;
            byte[] outputBuffer = new byte[0];
            FSCTL_OFFLOAD_WRITE_INPUT offloadWriteInput = new FSCTL_OFFLOAD_WRITE_INPUT();

            //TokenId is static size of 504, set it here to make TypeMarshal works correctly.
            offloadWriteInput.Token = new STORAGE_OFFLOAD_TOKEN() { TokenId = new byte[504] };
            offloadWriteInput.Size = (uint)TypeMarshal.ToBytes<FSCTL_OFFLOAD_WRITE_INPUT>(offloadWriteInput).Length;

            FSCTL_OFFLOAD_WRITE_OUTPUT offloadWriteOutput = new FSCTL_OFFLOAD_WRITE_OUTPUT();
            uint outputBufferSize = (uint)TypeMarshal.ToBytes<FSCTL_OFFLOAD_WRITE_OUTPUT>(offloadWriteOutput).Length;
            BaseTestSite.Log.Add(LogEntryKind.TestStep, "2. FSCTL request with FSCTL_OFFLOAD_WRITE.");
            status = this.fsaAdapter.FsCtlOffloadWrite(offloadWriteInput, outputBufferSize, out bytesReturned, out outputBuffer);

            if (!IsCurrentTransportSupportCopyOffload(status)) return;

            //Step 3: Verify test result
            BaseTestSite.Log.Add(LogEntryKind.TestStep, "3. Verify returned NTStatus code.");
            if (this.fsaAdapter.IsOffloadImplemented == false)
            {
                this.fsaAdapter.AssertAreEqual(this.Manager, MessageStatus.INVALID_DEVICE_REQUEST, status,
                    "If the object store does not implement this functionality, the operation MUST be failed with STATUS_INVALID_DEVICE_REQUEST.");
            }
            else if (status == MessageStatus.NOT_SUPPORTED)
            {
                this.fsaAdapter.AssertAreEqual(this.Manager, MessageStatus.NOT_SUPPORTED, status,
                    "If Open.File.Volume.IsOffloadWriteSupported is FALSE, the operation MUST be failed with STATUS_NOT_SUPPORTED.");
            }
            else if (status == MessageStatus.STATUS_OFFLOAD_WRITE_FLT_NOT_SUPPORTED)
            {
                this.fsaAdapter.AssertAreEqual(this.Manager, MessageStatus.STATUS_OFFLOAD_WRITE_FLT_NOT_SUPPORTED, status,
                    "A file system filter on the server has not opted in for Offload Write support.");
            }
            else if (status == MessageStatus.STATUS_OFFLOAD_WRITE_FILE_NOT_SUPPORTED)
            {
                this.fsaAdapter.AssertAreEqual(this.Manager, MessageStatus.STATUS_OFFLOAD_WRITE_FILE_NOT_SUPPORTED, status,
                    @"Offload write operations cannot be performed on: Compressed files, Sparse files, Encrypted files, File system metadata files");
            }
            else
            {
                this.fsaAdapter.AssertAreEqual(this.Manager, MessageStatus.SUCCESS, status, "Status set to STATUS_SUCCESS.");
            }
        }
        #endregion
    }
}
