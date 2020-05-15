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
    public partial class FileInfoTestCases : PtfTestClassBase
    {
        #region Test Cases

        [TestMethod()]
        [TestCategory(TestCategories.Fsa)]
        [TestCategory(TestCategories.QueryFileInformation)]
        [TestCategory(TestCategories.NonSmb)]
        [TestCategory(TestCategories.Positive)]
        [Description("Query FileCompressionInfo from a file and check if Compression is supported.")]
        public void FileInfo_Query_FileCompressionInfo_File_IsCompressionSupported() 
        {
            FileInfo_Query_FileCompressionInfo_IsCompressionSupported(FileType.DataFile);
        }

        [TestMethod()]
        [TestCategory(TestCategories.Fsa)]
        [TestCategory(TestCategories.QueryFileInformation)]
        [TestCategory(TestCategories.NonSmb)]
        [TestCategory(TestCategories.Positive)]
        [Description("Query FileCompressionInfo from a directory and check if Compression is supported.")]
        public void FileInfo_Query_FileCompressionInfo_Dir_IsCompressionSupported()
        {
            FileInfo_Query_FileCompressionInfo_IsCompressionSupported(FileType.DirectoryFile);
        }

        #endregion

        #region Test Case Utility

        private void FileInfo_Query_FileCompressionInfo_IsCompressionSupported(FileType fileType)
        {
            BaseTestSite.Log.Add(LogEntryKind.TestStep, "Test case steps:");
            MessageStatus status;

            //Step 1: Create File
            BaseTestSite.Log.Add(LogEntryKind.TestStep, "1. Create  " + fileType.ToString());
            status = this.fsaAdapter.CreateFile(fileType);

            //Step 2: Set compression
            FSCTL_SET_COMPRESSION_Request setCompressionRequest = new FSCTL_SET_COMPRESSION_Request();
            setCompressionRequest.CompressionState = FSCTL_SET_COMPRESSION_Request_CompressionState_Values.COMPRESSION_FORMAT_LZNT1;
            uint inputBufferSize = (uint)TypeMarshal.ToBytes<FSCTL_SET_COMPRESSION_Request>(setCompressionRequest).Length;

            BaseTestSite.Log.Add(LogEntryKind.TestStep, "2. FSCTL request with FSCTL_SET_COMPRESSION");
            status = this.fsaAdapter.FsCtlSetCompression(setCompressionRequest, inputBufferSize);

            //Step 3: Query FILE_COMPRESSION_INFORMATION
            FileCompressionInformation fileCompressionInfo = new FileCompressionInformation() { Reserved = new byte[3] };
            long byteCount;
            byte[] outputBuffer = new byte[0];

            uint outputBufferSize = (uint)TypeMarshal.ToBytes<FileCompressionInformation>(fileCompressionInfo).Length;

            BaseTestSite.Log.Add(LogEntryKind.TestStep, "3. QueryFileInformation with FileInfoClass.FILE_COMPRESSION_INFORMATION");
            status = this.fsaAdapter.QueryFileInformation(FileInfoClass.FILE_COMPRESSION_INFORMATION, outputBufferSize, out byteCount, out outputBuffer);

            //Step 4: Verify test result
            if (this.fsaAdapter.FileSystem == FileSystem.FAT32)
            {
                this.fsaAdapter.AssertAreEqual(this.Manager, MessageStatus.INVALID_PARAMETER, status,
                    "If a file system does not support a specific File Information Class, STATUS_INVALID_PARAMETER MUST be returned.");
                return;
            }

            fileCompressionInfo = TypeMarshal.ToStruct<FileCompressionInformation>(outputBuffer);
            bool isCompressionFormatLZNT1 = (fileCompressionInfo.CompressionFormat & CompressionFormat_Values.COMPRESSION_FORMAT_LZNT1) == CompressionFormat_Values.COMPRESSION_FORMAT_LZNT1;

            BaseTestSite.Log.Add(LogEntryKind.TestStep, "4. Verify outputBuffer.CompressionFormat");
            if (this.fsaAdapter.IsCompressionSupported == true)
            {
                if (fileType == FileType.DirectoryFile && this.fsaAdapter.FileSystem == FileSystem.CSVFS)
                {
                    this.fsaAdapter.AssertAreEqual(this.Manager, false, isCompressionFormatLZNT1, "CSVFS does not support setting compressed attribute on the folders, the compressionFormat should be NONE.");
                }
                else
                {
                    this.fsaAdapter.AssertAreEqual(this.Manager, true, isCompressionFormatLZNT1, "Compression is supported, the object store MUST set OutputBuffer.CompressionState to COMPRESSION_FORMAT_LZNT1.");
                }
            }
            else
            {
                this.fsaAdapter.AssertAreEqual(this.Manager, false, isCompressionFormatLZNT1, "Compression is NOT supported, the object store MUST NOT set OutputBuffer.CompressionState to COMPRESSION_FORMAT_LZNT1.");
            }
        }

        #endregion
    }
}
