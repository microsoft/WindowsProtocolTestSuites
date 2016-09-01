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
        [TestCategory(TestCategories.QueryFileInformation)]
        [TestCategory(TestCategories.AlternateDataStream)]
        [Description("Query FileCompressionInformation from an Alternate Data Stream on a DataFile.")]
        public void AlternateDataStream_Query_FileCompressionInformation_File()
        {
            AlternateDataStream_Query_FileCompressionInformation(FileType.DataFile);
        }

        [TestMethod()]
        [TestCategory(TestCategories.Fsa)]
        [TestCategory(TestCategories.QueryFileInformation)]
        [TestCategory(TestCategories.AlternateDataStream)]
        [Description("Query FileCompressionInformation from an Alternate Data Stream on a DirectoryFile.")]
        public void AlternateDataStream_Query_FileCompressionInformation_Dir()
        {
            AlternateDataStream_Query_FileCompressionInformation(FileType.DirectoryFile);
        }

        #endregion

        #region Test Case Utility

        private void AlternateDataStream_Query_FileCompressionInformation(FileType fileType)
        {
            BaseTestSite.Log.Add(LogEntryKind.TestStep, "Test case steps:");
            MessageStatus status = MessageStatus.SUCCESS;
            Dictionary<string, long> streamList = new Dictionary<string, long>();
            long bytesToWrite = 0;
            long bytesWritten = 0;

            //Step 1: Create a new File, it could be a DataFile or a DirectoryFile
            string fileName = this.fsaAdapter.ComposeRandomFileName(8);
            BaseTestSite.Log.Add(LogEntryKind.TestStep, "1. Create a file with type: " + fileType.ToString() + " and name: " + fileName);
            CreateOptions createFileType = (fileType == FileType.DataFile ? CreateOptions.NON_DIRECTORY_FILE : CreateOptions.DIRECTORY_FILE);
            status = this.fsaAdapter.CreateFile(
                        fileName,
                        FileAttribute.NORMAL,
                        createFileType,
                        FileAccess.GENERIC_ALL,
                        ShareAccess.FILE_SHARE_READ | ShareAccess.FILE_SHARE_WRITE | ShareAccess.FILE_SHARE_DELETE,
                        CreateDisposition.OPEN_IF);
            this.fsaAdapter.AssertIfNotSuccess(status, "Create file operation failed");

            //Step 2: Write some bytes into the Unnamed Data Stream in the newly created file
            if (fileType == FileType.DataFile)
            {
                //Write some bytes into the DataFile.
                bytesToWrite = 1024;
                bytesWritten = 0;
                streamList.Add("::$DATA", bytesToWrite);

                BaseTestSite.Log.Add(LogEntryKind.TestStep, "2. Write the file with " + bytesToWrite + " bytes data.");
                status = this.fsaAdapter.WriteFile(0, bytesToWrite, out bytesWritten);
                this.fsaAdapter.AssertIfNotSuccess(status, "Write data to file operation failed.");
            }
            else
            {
                //Do not write data into DirectoryFile.
                bytesToWrite = 0;
                BaseTestSite.Log.Add(LogEntryKind.TestStep, "2. Do not write data into DirectoryFile.");
            }

            //Step 3: Create an Alternate Data Stream <Stream1> in the newly created file
            string streamName1 = this.fsaAdapter.ComposeRandomFileName(8);

            BaseTestSite.Log.Add(LogEntryKind.TestStep, "3. Create an Alternate Data Stream with name: " + streamName1 + "on this file.");
            status = this.fsaAdapter.CreateFile(
                        fileName + ":" + streamName1 + ":$DATA",
                        FileAttribute.NORMAL | FileAttribute.INTEGRITY_STREAM, // Set Integrity field
                        CreateOptions.NON_DIRECTORY_FILE,
                        FileAccess.GENERIC_ALL,
                        ShareAccess.FILE_SHARE_READ | ShareAccess.FILE_SHARE_WRITE | ShareAccess.FILE_SHARE_DELETE,
                        CreateDisposition.OPEN_IF);
            this.fsaAdapter.AssertIfNotSuccess(status, "Create Alternate Data Stream operation failed");

            //Step 4: Write some bytes into the Alternate Data Stream <Stream1> in the file
            bytesToWrite = 2048;
            bytesWritten = 0;
            streamList.Add(":" + streamName1 + ":$DATA", bytesToWrite);

            BaseTestSite.Log.Add(LogEntryKind.TestStep, "4. Write the stream with " + bytesToWrite + " bytes data.");
            status = this.fsaAdapter.WriteFile(0, bytesToWrite, out bytesWritten);
            this.fsaAdapter.AssertIfNotSuccess(status, "Write data to stream operation failed.");

            //Step 5: Set compression
            FSCTL_SET_COMPRESSION_Request setCompressionRequest = new FSCTL_SET_COMPRESSION_Request();
            setCompressionRequest.CompressionState = FSCTL_SET_COMPRESSION_Request_CompressionState_Values.COMPRESSION_FORMAT_LZNT1;
            uint inputBufferSize = (uint)TypeMarshal.ToBytes<FSCTL_SET_COMPRESSION_Request>(setCompressionRequest).Length;

            BaseTestSite.Log.Add(LogEntryKind.TestStep, "5. FSCTL request with FSCTL_SET_COMPRESSION");
            status = this.fsaAdapter.FsCtlSetCompression(setCompressionRequest, inputBufferSize);
            this.fsaAdapter.AssertIfNotSuccess(status, "FSCTL request with FSCTL_SET_COMPRESSION operation failed.");

            //Step 6: Query FILE_COMPRESSION_INFORMATION
            FileCompressionInformation fileCompressionInfo = new FileCompressionInformation() { Reserved = new byte[3] };
            long byteCount;
            byte[] outputBuffer = new byte[0];

            uint outputBufferSize = (uint)TypeMarshal.ToBytes<FileCompressionInformation>(fileCompressionInfo).Length;

            BaseTestSite.Log.Add(LogEntryKind.TestStep, "6. QueryFileInformation with FileInfoClass.FILE_COMPRESSION_INFORMATION");
            status = this.fsaAdapter.QueryFileInformation(FileInfoClass.FILE_COMPRESSION_INFORMATION, outputBufferSize, out byteCount, out outputBuffer);
            if (this.fsaAdapter.FileSystem == FileSystem.FAT32)
            {
                this.fsaAdapter.AssertAreEqual(this.Manager, MessageStatus.INVALID_PARAMETER, status,
                    "If a file system does not support a specific File Information Class, STATUS_INVALID_PARAMETER MUST be returned.");
                return;
            }
            
            //Step 7: Verify outputBuffer.CompressionFormat
            fileCompressionInfo = TypeMarshal.ToStruct<FileCompressionInformation>(outputBuffer);
            bool isCompressionFormatLZNT1 = (fileCompressionInfo.CompressionFormat & CompressionFormat_Values.COMPRESSION_FORMAT_LZNT1) == CompressionFormat_Values.COMPRESSION_FORMAT_LZNT1;

            BaseTestSite.Log.Add(LogEntryKind.TestStep, "7. Verify outputBuffer.CompressionFormat");
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