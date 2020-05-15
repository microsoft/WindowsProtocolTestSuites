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
        [TestCategory(TestCategories.NonSmb)]
        [TestCategory(TestCategories.Positive)]
        [Description("Query FileCompressionInformation from an Alternate Data Stream on a DataFile.")]
        public void AlternateDataStream_Query_FileCompressionInformation_File()
        {
            AlternateDataStream_CreateStream(FileType.DataFile);

            AlternateDataStream_Query_FileCompressionInformation(FileType.DataFile);
        }

        [TestMethod()]
        [TestCategory(TestCategories.Fsa)]
        [TestCategory(TestCategories.QueryFileInformation)]
        [TestCategory(TestCategories.AlternateDataStream)]
        [TestCategory(TestCategories.NonSmb)]
        [TestCategory(TestCategories.Positive)]
        [Description("Query FileCompressionInformation from an Alternate Data Stream on a DirectoryFile.")]
        public void AlternateDataStream_Query_FileCompressionInformation_Dir()
        {
            AlternateDataStream_CreateStream(FileType.DirectoryFile);

            AlternateDataStream_Query_FileCompressionInformation(FileType.DirectoryFile);
        }

        #endregion

        #region Test Case Utility

        private void AlternateDataStream_Query_FileCompressionInformation(FileType fileType)
        {
            //Prerequisites: Create streams on a newly created file

            //Step 1: Set compression by FSCTL_SET_COMPRESSION
            FSCTL_SET_COMPRESSION_Request setCompressionRequest = new FSCTL_SET_COMPRESSION_Request();
            setCompressionRequest.CompressionState = FSCTL_SET_COMPRESSION_Request_CompressionState_Values.COMPRESSION_FORMAT_LZNT1;
            uint inputBufferSize = (uint)TypeMarshal.ToBytes<FSCTL_SET_COMPRESSION_Request>(setCompressionRequest).Length;

            BaseTestSite.Log.Add(LogEntryKind.TestStep, "{0}. FSCTL request with FSCTL_SET_COMPRESSION", ++testStep);
            status = this.fsaAdapter.FsCtlSetCompression(setCompressionRequest, inputBufferSize);
            this.fsaAdapter.AssertIfNotSuccess(status, "FSCTL request with FSCTL_SET_COMPRESSION operation failed.");

            //Step 2: Query FILE_COMPRESSION_INFORMATION
            FileCompressionInformation fileCompressionInfo = new FileCompressionInformation() { Reserved = new byte[3] };
            long byteCount;
            byte[] outputBuffer = new byte[0];

            uint outputBufferSize = (uint)TypeMarshal.ToBytes<FileCompressionInformation>(fileCompressionInfo).Length;

            BaseTestSite.Log.Add(LogEntryKind.TestStep, "{0}. QueryFileInformation with FileInfoClass.FILE_COMPRESSION_INFORMATION", ++testStep);
            status = this.fsaAdapter.QueryFileInformation(FileInfoClass.FILE_COMPRESSION_INFORMATION, outputBufferSize, out byteCount, out outputBuffer);
            if (this.fsaAdapter.FileSystem == FileSystem.FAT32)
            {
                this.fsaAdapter.AssertAreEqual(this.Manager, MessageStatus.INVALID_PARAMETER, status,
                    "If a file system does not support a specific File Information Class, STATUS_INVALID_PARAMETER MUST be returned.");
                return;
            }
            
            //Step 3: Verify outputBuffer.CompressionFormat
            fileCompressionInfo = TypeMarshal.ToStruct<FileCompressionInformation>(outputBuffer);
            bool isCompressionFormatLZNT1 = (fileCompressionInfo.CompressionFormat & CompressionFormat_Values.COMPRESSION_FORMAT_LZNT1) == CompressionFormat_Values.COMPRESSION_FORMAT_LZNT1;

            BaseTestSite.Log.Add(LogEntryKind.TestStep, "{0}. Verify outputBuffer.CompressionFormat", ++testStep);
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