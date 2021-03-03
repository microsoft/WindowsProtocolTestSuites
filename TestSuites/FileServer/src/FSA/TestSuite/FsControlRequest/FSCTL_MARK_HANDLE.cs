// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Protocols.TestSuites.FileSharing.Common.Adapter;
using Microsoft.Protocols.TestSuites.FileSharing.FSA.Adapter;
using Microsoft.Protocols.TestTools;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Protocols.TestSuites.FileSharing.FSA.TestSuite
{
    public partial class FsCtlTestCases : PtfTestClassBase
    {
        #region Test cases

        #region FsCtl_MarkHandleRequest_IsSupported

        [TestMethod()]
        [TestCategory(TestCategories.Fsa)]
        [TestCategory(TestCategories.IoCtlRequest)]
        [TestCategory(TestCategories.Positive)]
        [Description("Send FSCTL_MARK_HANDLE request to a file.")]
        public void FsCtl_MarkHandle_File_IsSupported()
        {
            FsCtl_MarkHandle_Test(FileType.DataFile);
        }

        [TestMethod()]
        [TestCategory(TestCategories.Fsa)]
        [TestCategory(TestCategories.IoCtlRequest)]
        [Description("Send FSCTL_MARK_HANDLE request to a directory.")]
        public void FsCtl_MarkHandle_Dir_NotSupported()
        {
            FsCtl_MarkHandle_Test(FileType.DirectoryFile);
        }

        #endregion

        #region Fsctl_MarkHandle_IsBuffered

        [TestMethod()]
        [TestCategory(TestCategories.Fsa)]
        [TestCategory(TestCategories.IoCtlRequest)]
        [Description("Send FSCTL_MARK_HANDLE request to a file opened with intermediate buffering.")]
        public void Fsctl_MarkHandle_File_BufferedNotSupported()
        {
            FsCtl_MarkHandle_Test(FileType.DataFile, isBufferDisabled: false);
        }

        [TestMethod()]
        [TestCategory(TestCategories.Fsa)]
        [TestCategory(TestCategories.IoCtlRequest)]
        [Description("Send FSCTL_MARK_HANDLE request to a directory opened with intermediate buffering.")]
        public void Fsctl_MarkHandle_Dir_BufferedNotSupported()
        {
            FsCtl_MarkHandle_Test(FileType.DirectoryFile, isBufferDisabled: false);
        }

        #endregion

        #region Fsctl_MarkHandle_Redundant_Media

        [TestMethod()]
        [TestCategory(TestCategories.Fsa)]
        [TestCategory(TestCategories.IoCtlRequest)]
        [TestCategory(TestCategories.Positive)]
        [TestCategory(TestCategories.IsRedundantMedia)]
        [Description("Send FSCTL_MARK_HANDLE request to a file on redundant media with valid copyNumber.")]
        public void Fsctl_MarkHandle_File_RedundantMedia_CopyNumberLessThanDataCopies()
        {
            FsCtl_MarkHandle_Test(fileType: FileType.DataFile,
                isBufferDisabled: true,
                copyNumber: this.fsaAdapter.NumberOfDataCopies - 1,
                shouldReadCopy: true);
        }

        [TestMethod()]
        [TestCategory(TestCategories.Fsa)]
        [TestCategory(TestCategories.IoCtlRequest)]
        [TestCategory(TestCategories.IsRedundantMedia)]
        [Description("Send FSCTL_MARK_HANDLE request to a file on redundant media with invalid copyNumber")]
        public void Fsctl_MarkHandle_File_RedundantMedia_CopyNumberGreaterThanDataCopies()
        {
            FsCtl_MarkHandle_Test(fileType: FileType.DataFile,
                isBufferDisabled: true,
                copyNumber: this.fsaAdapter.NumberOfDataCopies + 1,
                shouldReadCopy: true);
        }

        [TestMethod()]
        [TestCategory(TestCategories.Fsa)]
        [TestCategory(TestCategories.IoCtlRequest)]
        [TestCategory(TestCategories.IsRedundantMedia)]
        [Description("Send FSCTL_MARK_HANDLE request to a directory on redundant media")]
        public void Fsctl_MarkHandle_Dir_RedundantMediaNotSupported()
        {
            FsCtl_MarkHandle_Test(fileType: FileType.DirectoryFile,
                isBufferDisabled: true,
                shouldReadCopy: true);
        }

        #endregion

        #endregion

        #region Test Case Utility

        private void FsCtl_MarkHandle_Test(FileType fileType,
            bool isBufferDisabled = true,
            uint copyNumber = 0,
            bool shouldReadCopy = false)
        {
            BaseTestSite.Log.Add(LogEntryKind.TestStep, "Test case steps:");
            MessageStatus status;

            //Step 1: Create file
            BaseTestSite.Log.Add(LogEntryKind.TestStep, $"1. Create {fileType.ToString()}");
            status = this.fsaAdapter.CreateFile(fileType, false, isBufferDisabled);
            BaseTestSite.Assert.AreEqual(MessageStatus.SUCCESS, status, "Create should succeed.");

            //Step 2: Send FSCTL_MARK_HANDLE code
            BaseTestSite.Log.Add(LogEntryKind.TestStep, $"2. FSCTL_MARK_HANDLE request with copyNumber: {copyNumber}, and shouldReadCopy: {shouldReadCopy}");
            status = this.fsaAdapter.FsCtlMarkHandle(copyNumber, shouldReadCopy);

            //Step 3: Verify test result
            BaseTestSite.Log.Add(LogEntryKind.TestStep, "3. Verify returned NTSTATUS code.");

            if (!this.fsaAdapter.IsMarkHandleSupported)
            {
                this.fsaAdapter.AssertAreEqual(this.Manager,
                    MessageStatus.INVALID_DEVICE_REQUEST,
                    status,
                    "If the object store does not implement this functionality, the operation MUST be failed with STATUS_INVALID_DEVICE_REQUEST.");
            }
            else {
                if (fileType == FileType.DirectoryFile)
                {
                    this.fsaAdapter.AssertAreEqual(this.Manager, MessageStatus.STATUS_DIRECTORY_NOT_SUPPORTED, status,
                            "If Open.Stream.StreamType == DirectoryStream, the operation MUST be failed with STATUS_DIRECTORY_NOT_SUPPORTED.");
                }
                else
                {
                    if (shouldReadCopy && (this.fsaAdapter.NumberOfDataCopies < 2 || !this.fsaAdapter.IsRedundantMedia))
                    {
                        this.fsaAdapter.AssertAreEqual(this.Manager, MessageStatus.STATUS_NOT_REDUNDANT_STORAGE, status,
                                "If Open.File.Volume.NumberOfDataCopies < 2, the operation MUST be failed with STATUS_NOT_REDUNDANT_STORAGE.");
                    }
                    else if (!isBufferDisabled || (shouldReadCopy && copyNumber >= this.fsaAdapter.NumberOfDataCopies))
                    {
                        this.fsaAdapter.AssertAreEqual(this.Manager, MessageStatus.INVALID_PARAMETER, status,
                            "STATUS_INVALID_PARAMETER is returned if Open.Mode.FILE_NO_INTERMEDIATE_BUFFERING was not specified at open time OR " +
                            "InputBuffer.CopyNumber > (Open.File.Volume.NumberOfDataCopies – 1) OR Open.Stream.StreamType != DataStream.");
                    }
                    else
                    {
                        this.fsaAdapter.AssertAreEqual(this.Manager, MessageStatus.SUCCESS, status,
                            "Upon successful completion of the operation, the object store MUST return: Status set to STATUS_SUCCESS");
                    }
                }
            }
        }

        #endregion
    }
}
