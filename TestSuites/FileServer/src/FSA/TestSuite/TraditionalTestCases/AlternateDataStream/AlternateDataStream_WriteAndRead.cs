// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Protocols.TestSuites.FileSharing.Common.Adapter;
using Microsoft.Protocols.TestSuites.FileSharing.FSA.Adapter;
using Microsoft.Protocols.TestTools;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace Microsoft.Protocols.TestSuites.FileSharing.FSA.TestSuite
{
    public partial class AlternateDataStreamTestCases : PtfTestClassBase
    {
        #region Test Cases

        [TestMethod()]
        [TestCategory(TestCategories.Bvt)]
        [TestCategory(TestCategories.Fsa)]
        [TestCategory(TestCategories.AlternateDataStream)]
        [TestCategory(TestCategories.NonSmb)]
        [Description("Write and Read from an Alternate Data Stream on a DataFile.")]
        public void BVT_AlternateDataStream_WriteAndRead_File()
        {
            AlternateDataStream_WriteAndRead(FileType.DataFile);
        }

        [TestMethod()]
        [TestCategory(TestCategories.Bvt)]
        [TestCategory(TestCategories.Fsa)]
        [TestCategory(TestCategories.AlternateDataStream)]
        [TestCategory(TestCategories.NonSmb)]
        [Description("Write and Read from an Alternate Data Stream on a DirectoryFile.")]
        public void BVT_AlternateDataStream_WriteAndRead_Dir()
        {
            AlternateDataStream_WriteAndRead(FileType.DirectoryFile);
        }

        #endregion

        #region Test Case Utility

        private void AlternateDataStream_WriteAndRead(FileType fileType)
        {
            BaseTestSite.Log.Add(LogEntryKind.TestStep, "Test case steps:");
            MessageStatus status = MessageStatus.SUCCESS;
            Dictionary<string, long> streamList = new Dictionary<string, long>();
            long bytesToWrite = 0;
            long bytesWritten = 0;
            int bytesToRead = 0;
            long bytesRead = 0;

            //Step 1: Create a new File, it could be a DataFile or a DirectoryFile
            string fileName = this.fsaAdapter.ComposeRandomFileName(8);
            BaseTestSite.Log.Add(LogEntryKind.TestStep, "{0}. Create a file with type: " + fileType.ToString() + " and name: " + fileName, ++testStep);
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

                BaseTestSite.Log.Add(LogEntryKind.TestStep, "{0}. Write the file with " + bytesToWrite + " bytes data.", ++testStep);
                status = this.fsaAdapter.WriteFile(0, bytesToWrite, out bytesWritten);
                this.fsaAdapter.AssertIfNotSuccess(status, "Write data to file operation failed.");
            }
            else
            {
                //Do not write data into DirectoryFile.
                bytesToWrite = 0;
                BaseTestSite.Log.Add(LogEntryKind.TestStep, "{0}. Do not write data into DirectoryFile.", ++testStep);
            }

            //Step 3: Create an Alternate Data Stream <Stream1> in the newly created file
            string streamName1 = this.fsaAdapter.ComposeRandomFileName(8);

            BaseTestSite.Log.Add(LogEntryKind.TestStep, "{0}. Create an Alternate Data Stream with name: " + streamName1 + " on this file.", ++testStep);
            status = this.fsaAdapter.CreateFile(
                        fileName + ":" + streamName1 + ":$DATA",
                        FileAttribute.NORMAL,
                        CreateOptions.NON_DIRECTORY_FILE,
                        FileAccess.GENERIC_ALL,
                        ShareAccess.FILE_SHARE_READ | ShareAccess.FILE_SHARE_WRITE | ShareAccess.FILE_SHARE_DELETE,
                        CreateDisposition.OPEN_IF);
            this.fsaAdapter.AssertIfNotSuccess(status, "Create Alternate Data Stream operation failed");

            //Step 4: Write some bytes into the Alternate Data Stream <Stream1> in the file
            bytesToWrite = 2048;
            bytesWritten = 0;
            streamList.Add(":" + streamName1 + ":$DATA", bytesToWrite);

            BaseTestSite.Log.Add(LogEntryKind.TestStep, "{0}. Write the stream with " + bytesToWrite + " bytes data.", ++testStep);
            status = this.fsaAdapter.WriteFile(0, bytesToWrite, out bytesWritten);
            this.fsaAdapter.AssertIfNotSuccess(status, "Write data to stream operation failed.");

            //Step 5: Read some bytes from the Alternate Data Stream <Stream1> in the file
            bytesToRead = (int)bytesWritten;
            BaseTestSite.Log.Add(LogEntryKind.TestStep, "{0}. Read the stream from offset: 0 and length: " + bytesToRead.ToString(), ++testStep);
            status = this.fsaAdapter.ReadFile(0, bytesToRead, out bytesRead);
            this.fsaAdapter.AssertAreEqual(this.Manager, MessageStatus.INVALID_PARAMETER, status, "If IsUnbuffered is TRUE & (ByteOffset >= 0), the operation MUST be failed with STATUS_INVALID_PARAMETER under any of the following conditions: (1) (ByteOffset % Open.File.Volume.LogicalBytesPerSector) is not zero. (2) (ByteCount % Open.File.Volume.LogicalBytesPerSector) is not zero.");

            //Step 6: Create another Alternate Data Stream <Stream2> in the newly created file
            string streamName2 = this.fsaAdapter.ComposeRandomFileName(8);

            BaseTestSite.Log.Add(LogEntryKind.TestStep, "{0}. Create an Alternate Data Stream with name: " + streamName2 + " on this file.", ++testStep);
            status = this.fsaAdapter.CreateFile(
                        fileName + ":" + streamName2 + ":$DATA",
                        FileAttribute.NORMAL,
                        CreateOptions.NON_DIRECTORY_FILE,
                        FileAccess.GENERIC_ALL,
                        ShareAccess.FILE_SHARE_READ | ShareAccess.FILE_SHARE_WRITE | ShareAccess.FILE_SHARE_DELETE,
                        CreateDisposition.OPEN_IF);
            this.fsaAdapter.AssertIfNotSuccess(status, "Create Alternate Data Stream operation failed");

            //Step 7: Write some bytes into the Alternate Data Stream <Stream2> in the file
            bytesToWrite = 4096;
            bytesWritten = 0;
            streamList.Add(":" + streamName2 + ":$DATA", bytesToWrite);

            BaseTestSite.Log.Add(LogEntryKind.TestStep, "{0}. Write the stream with " + bytesToWrite + " bytes data.", ++testStep);
            status = this.fsaAdapter.WriteFile(0, bytesToWrite, out bytesWritten);
            this.fsaAdapter.AssertIfNotSuccess(status, "Write data to stream operation failed.");

            //Step 8: Read some bytes from the Alternate Data Stream <Stream2> in the file
            bytesToRead = (int)bytesWritten;
            BaseTestSite.Log.Add(LogEntryKind.TestStep, "{0}. Read the stream from offset: 0 and length: " + bytesToRead.ToString(), ++testStep);
            status = this.fsaAdapter.ReadFile(0, bytesToRead, out bytesRead);
            this.fsaAdapter.AssertIfNotSuccess(status, "Read data from stream operation failed.");
        }

        #endregion

    }
}
