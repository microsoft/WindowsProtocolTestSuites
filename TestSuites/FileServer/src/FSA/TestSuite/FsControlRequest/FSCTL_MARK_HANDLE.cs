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

        #region FsCtl_MarkHandleRequest_IsSupported

        [TestMethod()]
        [TestCategory(TestCategories.Bvt)]
        [TestCategory(TestCategories.Fsa)]
        [TestCategory(TestCategories.IoCtlRequest)]
        [TestCategory(TestCategories.NonSmb)]
        [TestCategory(TestCategories.Positive)]
        [Description("Send FSCTL_MARK_HANDLE request to a file.")]
        public void BVT_FsCtl_MarkHandle_File_IsSupported()
        {
            FsCtl_MarkHandle_Test(FileType.DataFile);
        }

        [TestMethod()]
        [TestCategory(TestCategories.Fsa)]
        [TestCategory(TestCategories.IoCtlRequest)]
        [TestCategory(TestCategories.NonSmb)]
        [Description("Send FSCTL_MARK_HANDLE request to a directory.")]
        public void FsCtl_MarkHandle_Dir_NotSupported()
        {
            FsCtl_MarkHandle_Test(FileType.DirectoryFile);
        }

        #endregion

        #region FsCtl_MarkHandleRequest_IsFileEmpty

        [TestMethod()]
        [TestCategory(TestCategories.Fsa)]
        [TestCategory(TestCategories.IoCtlRequest)]
        [TestCategory(TestCategories.NonSmb)]
        [Description("Send FSCTL_MARK_HANDLE request to a file.")]
        public void FsCtl_MarkHandle_EmptyFile_IsSupported()
        {
            FsCtl_MarkHandle_Test(FileType.DataFile, isFileEmpty: true);
        }

        [TestMethod()]
        [TestCategory(TestCategories.Fsa)]
        [TestCategory(TestCategories.IoCtlRequest)]
        [TestCategory(TestCategories.NonSmb)]
        [Description("Send FSCTL_MARK_HANDLE request to a file.")]
        public void FsCtl_MarkHandle_NotEmptyFile_IsSupported()
        {
            FsCtl_MarkHandle_Test(FileType.DataFile, isFileEmpty: false);
        }

        #endregion

        #region FsCtl_MarkHandle_IsBuffered

        [TestMethod()]
        [TestCategory(TestCategories.Fsa)]
        [TestCategory(TestCategories.IoCtlRequest)]
        [TestCategory(TestCategories.NonSmb)]
        [Description("Send FSCTL_MARK_HANDLE request to a file opened with intermediate buffering.")]
        public void FsCtl_MarkHandle_File_BufferedNotSupported()
        {
            FsCtl_MarkHandle_Test(FileType.DataFile, isBufferDisabled: false);
        }

        [TestMethod()]
        [TestCategory(TestCategories.Fsa)]
        [TestCategory(TestCategories.IoCtlRequest)]
        [TestCategory(TestCategories.NonSmb)]
        [Description("Send FSCTL_MARK_HANDLE request to a directory opened with intermediate buffering.")]
        public void FsCtl_MarkHandle_Dir_BufferedNotSupported()
        {
            FsCtl_MarkHandle_Test(FileType.DirectoryFile, isBufferDisabled: false);
        }

        #endregion

        #region FsCtl_MarkHandle_IsCompressed

        [TestMethod()]
        [TestCategory(TestCategories.Fsa)]
        [TestCategory(TestCategories.IoCtlRequest)]
        [TestCategory(TestCategories.NonSmb)]
        [TestCategory(TestCategories.Compression)]
        [Description("Send FSCTL_MARK_HANDLE request to a file opened with intermediate buffering.")]
        public void FsCtl_MarkHandle_File_CompressedSupported()
        {
            FsCtl_MarkHandle_Test(FileType.DataFile, isCompressed: true, isFileEmpty: false);
        }

        [TestMethod()]
        [TestCategory(TestCategories.Fsa)]
        [TestCategory(TestCategories.IoCtlRequest)]
        [TestCategory(TestCategories.NonSmb)]
        [TestCategory(TestCategories.Compression)]
        [Description("Send FSCTL_MARK_HANDLE request to a directory opened with intermediate buffering.")]
        public void FsCtl_MarkHandle_Dir_CompressedNotSupported()
        {
            FsCtl_MarkHandle_Test(FileType.DirectoryFile, isCompressed: true);
        }

        #endregion

        #region FsCtl_MarkHandle_Redundant_Media

        [TestMethod()]
        [TestCategory(TestCategories.Fsa)]
        [TestCategory(TestCategories.IoCtlRequest)]
        [TestCategory(TestCategories.NonSmb)]
        [TestCategory(TestCategories.IsRedundantMedia)]
        [Description("Send FSCTL_MARK_HANDLE request to a file on redundant media with valid copyNumber.")]
        public void FsCtl_MarkHandle_File_RedundantMedia_CopyNumberLessThanDataCopies_EmptyFile()
        {
            FsCtl_MarkHandle_Test(fileType: FileType.DataFile,
                isBufferDisabled: true,
                copyNumber: this.fsaAdapter.NumberOfDataCopies - 1,
                shouldReadCopy: true);
        }

        [TestMethod()]
        [TestCategory(TestCategories.Fsa)]
        [TestCategory(TestCategories.IoCtlRequest)]
        [TestCategory(TestCategories.Positive)]
        [TestCategory(TestCategories.NonSmb)]
        [TestCategory(TestCategories.IsRedundantMedia)]
        [Description("Send FSCTL_MARK_HANDLE request to a file on redundant media with valid copyNumber.")]
        public void FsCtl_MarkHandle_File_RedundantMedia_CopyNumberLessThanDataCopies_NotEmptyFileSupported()
        {
            FsCtl_MarkHandle_Test(fileType: FileType.DataFile,
                isBufferDisabled: true,
                copyNumber: this.fsaAdapter.NumberOfDataCopies - 1,
                shouldReadCopy: true,
                isFileEmpty: false);
        }

        [TestMethod()]
        [TestCategory(TestCategories.Fsa)]
        [TestCategory(TestCategories.IoCtlRequest)]
        [TestCategory(TestCategories.NonSmb)]
        [TestCategory(TestCategories.Compression)]
        [TestCategory(TestCategories.IsRedundantMedia)]
        [Description("Send FSCTL_MARK_HANDLE request to a file opened with intermediate buffering.")]
        public void FsCtl_MarkHandle_File_RedundantMedia_CompressedNotSupported()
        {
            FsCtl_MarkHandle_Test(FileType.DataFile,
                copyNumber: this.fsaAdapter.NumberOfDataCopies - 1,
                isCompressed: true,
                shouldReadCopy: true,
                isFileEmpty: false);
        }

        [TestMethod()]
        [TestCategory(TestCategories.Fsa)]
        [TestCategory(TestCategories.IoCtlRequest)]
        [TestCategory(TestCategories.NonSmb)]
        [TestCategory(TestCategories.IsRedundantMedia)]
        [Description("Send FSCTL_MARK_HANDLE request to a file on redundant media with invalid copyNumber")]
        public void FsCtl_MarkHandle_File_RedundantMedia_CopyNumberGreaterThanDataCopies()
        {
            FsCtl_MarkHandle_Test(fileType: FileType.DataFile,
                isBufferDisabled: true,
                copyNumber: this.fsaAdapter.NumberOfDataCopies + 1,
                shouldReadCopy: true);
        }

        [TestMethod()]
        [TestCategory(TestCategories.Fsa)]
        [TestCategory(TestCategories.IoCtlRequest)]
        [TestCategory(TestCategories.NonSmb)]
        [TestCategory(TestCategories.IsRedundantMedia)]
        [Description("Send FSCTL_MARK_HANDLE request to a directory on redundant media")]
        public void FsCtl_MarkHandle_Dir_RedundantMediaNotSupported()
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
            bool shouldReadCopy = false,
            bool isFileEmpty = true,
            bool isCompressed = false)
        {
            this.fsaAdapter.TestConfig.CheckPlatform(Platform.WindowsServer2019);

            if (!this.fsaAdapter.IsMarkHandleSupported)
            {
                BaseTestSite.Assert.Inconclusive("MARK_HANDLE is not supported for this FileSystem");
            }

            MessageStatus status;
            int currentTestStep = 1;

            //Create file
            BaseTestSite.Log.Add(LogEntryKind.TestStep, $"{currentTestStep++}. Create {fileType}");
            status = this.fsaAdapter.CreateFile(fileType, false, isBufferDisabled);
            BaseTestSite.Assert.AreEqual(MessageStatus.SUCCESS, status, "Create should succeed.");

            if (!isFileEmpty)
            {
                BaseTestSite.Log.Add(LogEntryKind.TestStep, $"{currentTestStep++}. Write data to file");
                uint writeLength = 2 * 1024 * fsaAdapter.ClusterSizeInKB;

                status = fsaAdapter.WriteFile(0, writeLength, out long bytesWritten);

                BaseTestSite.Assert.AreEqual(MessageStatus.SUCCESS, status, $"{bytesWritten} bytes written to file should succeed.");
            }

            if (isCompressed)
            {
                if (this.fsaAdapter.IsCompressionSupported)
                {
                    FSCTL_SET_COMPRESSION_Request setCompressionRequest = new FSCTL_SET_COMPRESSION_Request();
                    setCompressionRequest.CompressionState = FSCTL_SET_COMPRESSION_Request_CompressionState_Values.COMPRESSION_FORMAT_LZNT1;
                    uint inputBufferSize = (uint)TypeMarshal.ToBytes<FSCTL_SET_COMPRESSION_Request>(setCompressionRequest).Length;

                    BaseTestSite.Log.Add(LogEntryKind.TestStep, $"{currentTestStep++}. FSCTL request with FSCTL_SET_COMPRESSION and state ==> {setCompressionRequest.CompressionState}");
                    status = this.fsaAdapter.FsCtlSetCompression(setCompressionRequest, inputBufferSize);

                    this.fsaAdapter.AssertAreEqual(this.Manager, MessageStatus.SUCCESS, status, "COMPRESSION is supported, status set to STATUS_SUCCESS.");
                }
                else
                {
                    BaseTestSite.Assert.Inconclusive("Compression is not supported for this FileSystem");
                }
            }

            //Send FSCTL_MARK_HANDLE code
            BaseTestSite.Log.Add(LogEntryKind.TestStep, $"{currentTestStep++}. FSCTL_MARK_HANDLE request with copyNumber: {copyNumber}, and shouldReadCopy: {shouldReadCopy}");
            status = this.fsaAdapter.FsCtlMarkHandle(copyNumber, shouldReadCopy);

            //Verify test result
            BaseTestSite.Log.Add(LogEntryKind.TestStep, $"{currentTestStep++}. Verify returned NTSTATUS code.");


            if (fileType == FileType.DirectoryFile)
            {
                if (this.fsaAdapter.FileSystem == FileSystem.REFS)
                {
                    this.fsaAdapter.AssertAreEqual(this.Manager, MessageStatus.INVALID_PARAMETER, status,
                            "The ReFS file system returns STATUS_INVALID_PARAMETER for directory files.");
                }
                else
                {
                    this.fsaAdapter.AssertAreEqual(this.Manager, MessageStatus.STATUS_DIRECTORY_NOT_SUPPORTED, status,
                            "If Open.Stream.StreamType == DirectoryStream, the operation MUST be failed with STATUS_DIRECTORY_NOT_SUPPORTED.");
                }
            }
            else
            {
                if (!isBufferDisabled)
                {
                    this.fsaAdapter.AssertAreEqual(this.Manager, MessageStatus.INVALID_PARAMETER, status,
                    "STATUS_INVALID_PARAMETER is returned if Open.Mode.FILE_NO_INTERMEDIATE_BUFFERING was not specified at open time OR " +
                    "InputBuffer.CopyNumber > (Open.File.Volume.NumberOfDataCopies – 1) OR Open.Stream.StreamType != DataStream.");
                }
                else if (shouldReadCopy)
                {
                    if (this.fsaAdapter.NumberOfDataCopies < 2)
                    {
                        this.fsaAdapter.AssertAreEqual(this.Manager, MessageStatus.STATUS_NOT_REDUNDANT_STORAGE, status,
                                "If Open.File.Volume.NumberOfDataCopies < 2, the operation MUST be failed with STATUS_NOT_REDUNDANT_STORAGE.");
                    }
                    else if (copyNumber >= this.fsaAdapter.NumberOfDataCopies)
                    {
                        this.fsaAdapter.AssertAreEqual(this.Manager, MessageStatus.INVALID_PARAMETER, status,
                        "STATUS_INVALID_PARAMETER is returned if InputBuffer.CopyNumber > (Open.File.Volume.NumberOfDataCopies – 1).");
                    }
                    else if (isCompressed && this.fsaAdapter.FileSystem == FileSystem.NTFS)
                    {
                        this.fsaAdapter.AssertAreEqual(this.Manager, MessageStatus.STATUS_COMPRESSED_FILE_NOT_SUPPORTED, status,
                                "If Open.Stream.IsCompressed is TRUE, the operation MUST be failed with STATUS_COMPRESSED_FILE_NOT_SUPPORTED.");
                    }
                    else if (this.fsaAdapter.FileSystem == FileSystem.NTFS && isFileEmpty)
                    {
                        this.fsaAdapter.AssertAreEqual(this.Manager, MessageStatus.STATUS_RESIDENT_FILE_NOT_SUPPORTED, status,
                            "If a file is resident the operation MUST be failed with STATUS_RESIDENT_FILE_NOT_SUPPORTED.");
                    }
                    else
                    {
                        this.fsaAdapter.AssertAreEqual(this.Manager, MessageStatus.SUCCESS, status,
                            "Upon successful completion of the operation, the object store MUST return: Status set to STATUS_SUCCESS");
                    }
                }
                else if (!shouldReadCopy && this.fsaAdapter.FileSystem == FileSystem.REFS)
                {
                    this.fsaAdapter.AssertAreEqual(this.Manager, MessageStatus.STATUS_NOT_REDUNDANT_STORAGE, status,
                            "For ReFS File System, if Open.File.Volume.NumberOfDataCopies < 2, the operation MUST be failed with STATUS_NOT_REDUNDANT_STORAGE.");
                }
                else
                {
                    this.fsaAdapter.AssertAreEqual(this.Manager, MessageStatus.SUCCESS, status,
                                "Upon successful completion of the operation, the object store MUST return: Status set to STATUS_SUCCESS");
                }
            }
        }

        #endregion
    }
}
