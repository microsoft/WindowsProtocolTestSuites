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
        [Description("Query FileStandardInformation from an Alternate Data Stream on a DataFile.")]
        public void AlternateDataStream_Query_FileStandardInformation_File()
        {
            AlternateDataStream_CreateStream(FileType.DataFile);

            AlternateDataStream_Query_FileStandardInformation(FileType.DataFile);
        }

        [TestMethod()]
        [TestCategory(TestCategories.Fsa)]
        [TestCategory(TestCategories.QueryFileInformation)]
        [TestCategory(TestCategories.AlternateDataStream)]
        [TestCategory(TestCategories.NonSmb)]
        [TestCategory(TestCategories.Positive)]
        [Description("Query FileStandardInformation from an Alternate Data Stream on a DirectoryFile.")]
        public void AlternateDataStream_Query_FileStandardInformation_Dir()
        {
            AlternateDataStream_CreateStream(FileType.DirectoryFile);

            AlternateDataStream_Query_FileStandardInformation(FileType.DirectoryFile);
        }

        #endregion

        #region Test Case Utility

        private void AlternateDataStream_Query_FileStandardInformation(FileType fileType)
        {
            //Prerequisites: Create streams on a newly created file

            //Step 1: Query FILE_STANDARD_INFORMATION
            long byteCount;
            byte[] outputBuffer;
            FileStandardInformation fileStandardInfo = new FileStandardInformation();
            fileStandardInfo.Reserved = new byte[2] { 0x00, 0x00 };
            uint outputBufferSize = (uint)TypeMarshal.ToBytes<FileStandardInformation>(fileStandardInfo).Length;

            BaseTestSite.Log.Add(LogEntryKind.TestStep, "{0}. QueryFileInformation with FileInfoClass.FILE_STANDARD_INFORMATION", ++testStep);
            status = this.fsaAdapter.QueryFileInformation(FileInfoClass.FILE_STANDARD_INFORMATION, outputBufferSize, out byteCount, out outputBuffer);
            this.fsaAdapter.AssertIfNotSuccess(status, "QueryFileInformation with FileInfoClass.FILE_STANDARD_INFORMATION operation failed.");

            // Step 6: Verify FILE_STANDARD_INFORMATION
            fileStandardInfo = TypeMarshal.ToStruct<FileStandardInformation>(outputBuffer);
            BaseTestSite.Log.Add(LogEntryKind.TestStep, "{0}. Verify outputBuffer.EndOfFile", ++testStep);
            this.fsaAdapter.AssertAreEqual(this.Manager, dataStreamList[":" + dataStreamName1 + ":$DATA"], fileStandardInfo.EndOfFile,
                "EndOfFile specifies the offset to the byte immediately following the last valid byte in the file. Because this value is zero-based, it actually refers to the first free byte in the file. That is, it is the offset from the beginning of the file at which new bytes appended to the file will be written.");
        }

        #endregion

    }
}