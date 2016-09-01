// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Protocols.TestSuites.FileSharing.Common.Adapter;
using Microsoft.Protocols.TestSuites.FileSharing.FSA.Adapter;
using Microsoft.Protocols.TestTools;
using Microsoft.Protocols.TestTools.StackSdk;
using Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Fscc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System;

namespace Microsoft.Protocols.TestSuites.FileSharing.FSA.TestSuite
{
    public partial class AlternateDataStreamTestCases : PtfTestClassBase
    {
        #region Test Cases

        [TestMethod()]
        [TestCategory(TestCategories.Bvt)]
        [TestCategory(TestCategories.Fsa)]
        [TestCategory(TestCategories.AlternateDataStream)]
        [Description("List all the Data Streams on a DataFile.")]
        public void AlternateDataStream_ListStreams_File()
        {
            AlternateDataStream_ListStreams(FileType.DataFile);
        }

        [TestMethod()]
        [TestCategory(TestCategories.Bvt)]
        [TestCategory(TestCategories.Fsa)]
        [TestCategory(TestCategories.AlternateDataStream)]
        [Description("List all the Data Streams on a DirectoryFile.")]
        public void AlternateDataStream_ListStreams_Dir()
        {
            AlternateDataStream_ListStreams(FileType.DirectoryFile);
        }

        #endregion

        #region Test Case Utility

        private void AlternateDataStream_ListStreams(FileType fileType)
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

            BaseTestSite.Log.Add(LogEntryKind.TestStep, "4. Write the stream with " + bytesToWrite + " bytes data.");
            status = this.fsaAdapter.WriteFile(0, bytesToWrite, out bytesWritten);
            this.fsaAdapter.AssertIfNotSuccess(status, "Write data to stream operation failed.");

            //Step 5: Create another Alternate Data Stream <Stream2> in the newly created file
            string streamName2 = this.fsaAdapter.ComposeRandomFileName(8);

            BaseTestSite.Log.Add(LogEntryKind.TestStep, "5. Create an Alternate Data Stream with name: " + streamName2 + "on this file.");
            status = this.fsaAdapter.CreateFile(
                        fileName + ":" + streamName2 + ":$DATA",
                        FileAttribute.NORMAL,
                        CreateOptions.NON_DIRECTORY_FILE,
                        FileAccess.GENERIC_ALL,
                        ShareAccess.FILE_SHARE_READ | ShareAccess.FILE_SHARE_WRITE | ShareAccess.FILE_SHARE_DELETE,
                        CreateDisposition.OPEN_IF);
            this.fsaAdapter.AssertIfNotSuccess(status, "Create Alternate Data Stream operation failed");

            //Step 6: Write some bytes into the Alternate Data Stream <Stream2> in the file
            bytesToWrite = 4096;
            bytesWritten = 0;
            streamList.Add(":" + streamName2 + ":$DATA", bytesToWrite);

            BaseTestSite.Log.Add(LogEntryKind.TestStep, "6. Write the stream with " + bytesToWrite + " bytes data.");
            status = this.fsaAdapter.WriteFile(0, bytesToWrite, out bytesWritten);
            this.fsaAdapter.AssertIfNotSuccess(status, "Write data to stream operation failed.");

            // Step 7: List the Alternate Data Streams by querying FileStreamInformation
            long byteCount;
            byte[] outputBuffer;
            uint outputBufferSize = 150;

            BaseTestSite.Log.Add(LogEntryKind.TestStep, "7. List the Alternate Data Streams on this file.");
            status = this.fsaAdapter.QueryFileInformation(
                FileInfoClass.FILE_STREAM_INFORMATION,
                outputBufferSize,
                out byteCount,
                out outputBuffer);
            this.fsaAdapter.AssertIfNotSuccess(status, "List the Alternate Data Stream operation failed");

            // Step 8: Verify the FileStreamInformation entry list from the outputBuffer returned by the query
            List<FileStreamInformation> fileStreamInformations = ParseFileStreamInformations(outputBuffer);

            BaseTestSite.Log.Add(LogEntryKind.TestStep, "8. Verify fields in each FileStreamInformation entry.");
            foreach (FileStreamInformation fileStreamInformation in fileStreamInformations)
            {
                string streamName = Encoding.Unicode.GetString(fileStreamInformation.StreamName);
                KeyValuePair<string, long> streamListElement = streamList.SingleOrDefault(x => x.Key.Equals(streamName));
                BaseTestSite.Assert.IsNotNull(streamListElement, "The stream with name {0} is found.", streamListElement.Key);
                this.fsaAdapter.AssertAreEqual(this.Manager, streamListElement.Value, (long)fileStreamInformation.StreamSize,
                    "The StreamSize field of each of the returned FILE_STREAM_INFORMATION data elements should match the size of bytes written to each data stream.");
            }
            this.fsaAdapter.AssertAreEqual(this.Manager, streamList.Count, fileStreamInformations.Count,
                "The total number of the returned FILE_STREAM_INFORMATION data elements should be equal the total streams that has been added to the file.");
        }

        private List<FileStreamInformation> ParseFileStreamInformations(byte[] outputBuffer)
        {
            List<FileStreamInformation> fileStreamInformations = new List<FileStreamInformation>();
            FileStreamInformation fileStreamInformation = new FileStreamInformation();
            int offset = 0;
            fileStreamInformation = TypeMarshal.ToStruct<FileStreamInformation>(outputBuffer);
            fileStreamInformations.Add(fileStreamInformation);
            while (fileStreamInformation.NextEntryOffset != 0)
            {
                offset += (int)(fileStreamInformation.NextEntryOffset);
                int temp = offset;
                fileStreamInformation = TypeMarshal.ToStruct<FileStreamInformation>(outputBuffer, ref temp);
                fileStreamInformations.Add(fileStreamInformation);
            }

            return fileStreamInformations;
        }

        #endregion
    }
}
