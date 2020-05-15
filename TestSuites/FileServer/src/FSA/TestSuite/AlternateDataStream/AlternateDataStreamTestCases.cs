// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Protocols.TestSuites.FileSharing.FSA.Adapter;
using Microsoft.Protocols.TestTools;
using Microsoft.Protocols.TestTools.StackSdk;
using Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Fscc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Microsoft.Protocols.TestSuites.FileSharing.FSA.TestSuite
{
    [TestClassAttribute()]
    public partial class AlternateDataStreamTestCases : PtfTestClassBase
    {
        #region Variables
        private FSAAdapter fsaAdapter;
        private int testStep;
        private string fileName;
        private CreateOptions createFileType;
        private string dataStreamName1;
        private string dataStreamName2;
        private string dataStreamName3;
        private Dictionary<string, long> dataStreamList;
        private MessageStatus status;
        #endregion

        #region Class Initialization and Cleanup
        [ClassInitializeAttribute()]
        public static void ClassInitialize(TestContext context)
        {
            PtfTestClassBase.Initialize(context);
        }

        [ClassCleanupAttribute()]
        public static void ClassCleanup()
        {
            PtfTestClassBase.Cleanup();
        }
        #endregion

        #region Test Initialization and Cleanup
        protected override void TestInitialize()
        {
            this.InitializeTestManager();
            this.fsaAdapter = new FSAAdapter();
            this.fsaAdapter.Initialize(BaseTestSite);
            this.fsaAdapter.LogTestCaseDescription(BaseTestSite);
            BaseTestSite.Log.Add(LogEntryKind.Comment, "Test environment:");
            BaseTestSite.Log.Add(LogEntryKind.Comment, "\t 1. File System: " + this.fsaAdapter.FileSystem.ToString());
            BaseTestSite.Log.Add(LogEntryKind.Comment, "\t 2. Transport: " + this.fsaAdapter.Transport.ToString());
            BaseTestSite.Log.Add(LogEntryKind.Comment, "\t 3. Share Path: " + this.fsaAdapter.UncSharePath);

            BaseTestSite.Assume.AreEqual(FileSystem.NTFS, this.fsaAdapter.FileSystem, "Alternate Data Stream is only supported by NTFS file system.");

            this.fsaAdapter.FsaInitial();

            BaseTestSite.Log.Add(LogEntryKind.TestStep, "Test case steps:");
            testStep = 0;
        }

        protected override void TestCleanup()
        {
            this.fsaAdapter.Dispose();
            base.TestCleanup();
            this.CleanupTestManager();
        }
        #endregion

        #region Test Case Utility

        /// <summary>
        /// Create one alternate data stream on the newly created file
        /// </summary>
        /// <param name="fileType">The newly created file type: DataFile, DirectoryFile</param>
        private void AlternateDataStream_CreateStream(FileType fileType)
        {   
            dataStreamList = new Dictionary<string, long>();
            long bytesToWrite = 0;
            long bytesWritten = 0;

            //Step 1: Create a new File, it could be a DataFile or a DirectoryFile
            fileName = this.fsaAdapter.ComposeRandomFileName(8);
            createFileType = (fileType == FileType.DataFile ? CreateOptions.NON_DIRECTORY_FILE : CreateOptions.DIRECTORY_FILE);
            BaseTestSite.Log.Add(LogEntryKind.TestStep, "{0}. Create a file with type: " + fileType.ToString() + " and name: " + fileName, ++testStep);
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
                dataStreamList.Add("::$DATA", bytesToWrite);

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
            dataStreamName1 = this.fsaAdapter.ComposeRandomFileName(8);

            BaseTestSite.Log.Add(LogEntryKind.TestStep, "{0}. Create an Alternate Data Stream with name: " + dataStreamName1 + " on this file.", ++testStep);
            status = this.fsaAdapter.CreateFile(
                        fileName + ":" + dataStreamName1 + ":$DATA",
                        FileAttribute.NORMAL,
                        CreateOptions.NON_DIRECTORY_FILE,
                        FileAccess.GENERIC_ALL,
                        ShareAccess.FILE_SHARE_READ | ShareAccess.FILE_SHARE_WRITE | ShareAccess.FILE_SHARE_DELETE,
                        CreateDisposition.OPEN_IF);
            this.fsaAdapter.AssertIfNotSuccess(status, "Create Alternate Data Stream operation failed");

            //Step 4: Write some bytes into the Alternate Data Stream <Stream1> in the file
            bytesToWrite = 2048;
            bytesWritten = 0;
            dataStreamList.Add(":" + dataStreamName1 + ":$DATA", bytesToWrite);

            BaseTestSite.Log.Add(LogEntryKind.TestStep, "{0}. Write the stream with " + bytesToWrite + " bytes data.", ++testStep);
            status = this.fsaAdapter.WriteFile(0, bytesToWrite, out bytesWritten);
            this.fsaAdapter.AssertIfNotSuccess(status, "Write data to stream operation failed.");
        }

        /// <summary>
        /// Create two alternate data streams on the newly created file
        /// </summary>
        /// <param name="fileType">The newly created file type: DataFile, DirectoryFile</param>
        private void AlternateDataStream_CreateStreams(FileType fileType)
        {
            long bytesToWrite = 0;
            long bytesWritten = 0;

            //Step 1-4: Create a new file and a new Alternate Data Stream.
            AlternateDataStream_CreateStream(fileType);

            //Step 5: Create another Alternate Data Stream <Stream2> in the newly created file
            dataStreamName2 = this.fsaAdapter.ComposeRandomFileName(8);

            BaseTestSite.Log.Add(LogEntryKind.TestStep, "{0}. Create an Alternate Data Stream with name: " + dataStreamName2 + " on this file.", ++testStep);
            status = this.fsaAdapter.CreateFile(
                        fileName + ":" + dataStreamName2 + ":$DATA",
                        FileAttribute.NORMAL,
                        CreateOptions.NON_DIRECTORY_FILE,
                        FileAccess.GENERIC_ALL,
                        ShareAccess.FILE_SHARE_READ | ShareAccess.FILE_SHARE_WRITE | ShareAccess.FILE_SHARE_DELETE,
                        CreateDisposition.OPEN_IF);
            this.fsaAdapter.AssertIfNotSuccess(status, "Create Alternate Data Stream operation failed");

            //Step 6: Write some bytes into the Alternate Data Stream <Stream2> in the file
            bytesToWrite = 4096;
            bytesWritten = 0;
            dataStreamList.Add(":" + dataStreamName2 + ":$DATA", bytesToWrite);

            BaseTestSite.Log.Add(LogEntryKind.TestStep, "{0}. Write the stream with " + bytesToWrite + " bytes data.", ++testStep);
            status = this.fsaAdapter.WriteFile(0, bytesToWrite, out bytesWritten);
            this.fsaAdapter.AssertIfNotSuccess(status, "Write data to stream operation failed.");
        }

        private void AlternateDataStream_ListStreams(FileType fileType)
        {
            //Prerequisites: Create streams on a newly created file

            //Step 1: List the Alternate Data Streams by querying FileStreamInformation
            //FileStreamInformation not only returns the Alternate Data Streams, but also other data streams in this file
            long byteCount;
            byte[] outputBuffer;
            uint outputBufferSize = 150;

            BaseTestSite.Log.Add(LogEntryKind.TestStep, "{0}. List the Alternate Data Streams on this file.", ++testStep);
            status = this.fsaAdapter.QueryFileInformation(
                FileInfoClass.FILE_STREAM_INFORMATION,
                outputBufferSize,
                out byteCount,
                out outputBuffer);
            this.fsaAdapter.AssertIfNotSuccess(status, "List the Alternate Data Stream operation failed");

            // Step 2: Verify the FileStreamInformation entry list from the outputBuffer returned by the query
            List<FileStreamInformation> fileStreamInformations = ParseFileStreamInformations(outputBuffer);

            BaseTestSite.Log.Add(LogEntryKind.TestStep, "{0}. Verify fields in each FileStreamInformation entry.", ++testStep);
            VerifyFileStreamInformations(fileStreamInformations, dataStreamList);
        }

        private void AlternateDataStream_DeleteStream(FileType fileType)
        {
            //Prerequisites: Create streams on a newly created file

            // Step 1: Delete the Alternate Data Stream <Stream2>
            BaseTestSite.Log.Add(LogEntryKind.TestStep, "{0}. Delete the Alternate Data Stream with name: " + dataStreamName2, ++testStep);
            dataStreamList.Remove(":" + dataStreamName2 + ":$DATA");

            BaseTestSite.Log.Add(LogEntryKind.Debug, "Set FileDispositionInformation.DeletePending to 1.");
            FileDispositionInformation fileDispositionInfo = new FileDispositionInformation();
            fileDispositionInfo.DeletePending = 1;
            List<byte> byteList = new List<byte>();
            byteList.AddRange(BitConverter.GetBytes(fileDispositionInfo.DeletePending));

            status = this.fsaAdapter.SetFileInformation(
                FileInfoClass.FILE_DISPOSITION_INFORMATION,
                byteList.ToArray());
            this.fsaAdapter.AssertIfNotSuccess(status, "Set FileDispositionInformation.DeletePending operation failed");

            BaseTestSite.Log.Add(LogEntryKind.Debug, "Close the open to delete the stream.");
            status = this.fsaAdapter.CloseOpen();
            this.fsaAdapter.AssertIfNotSuccess(status, "Close open operation failed");

            this.fsaAdapter.AssertIfNotSuccess(status, "Delete the Alternate Data Stream operation failed");

            //Step 2: Create a new open for the File, it could be a DataFile or a DirectoryFile
            BaseTestSite.Log.Add(LogEntryKind.TestStep, "{0}. Create a new open for the file with type: " + fileType.ToString() + " and name: " + fileName, ++testStep);
            createFileType = (fileType == FileType.DataFile ? CreateOptions.NON_DIRECTORY_FILE : CreateOptions.DIRECTORY_FILE);
            status = this.fsaAdapter.CreateFile(
                        fileName,
                        FileAttribute.NORMAL,
                        createFileType,
                        FileAccess.GENERIC_ALL,
                        ShareAccess.FILE_SHARE_READ | ShareAccess.FILE_SHARE_WRITE | ShareAccess.FILE_SHARE_DELETE,
                        CreateDisposition.OPEN_IF);
            this.fsaAdapter.AssertIfNotSuccess(status, "Create file operation failed");
        }

        /// <summary>
        /// Parse the outputBuffer to a list of FileStreamInformation structures
        /// </summary>
        /// <param name="outputBuffer">The outputBuffer returned from server</param>
        /// <returns>A list of FileStreamInformation structures</returns>
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

        /// <summary>
        /// Verify file stream information
        /// </summary>
        /// <param name="fileStreamInformations">A list of FileStreamInformation structures</param>
        /// <param name="streamList">A dictionary of streamname and streamsize mapping</param>
        private void VerifyFileStreamInformations(List<FileStreamInformation> fileStreamInformations, Dictionary<string, long> streamList)
        {   
            // To verify whether the count of filestreaminformation data elements equals to the actual data streams counts
            BaseTestSite.Assert.AreEqual(streamList.Count, fileStreamInformations.Count,
                "The total number of the returned FILE_STREAM_INFORMATION data elements should be equal the total streams that has been added to the file.");

            // To verify whether each data element of filestreaminformation matches with the actual data streams in name and size
            foreach (FileStreamInformation fileStreamInformation in fileStreamInformations)
            {
                string streamName = Encoding.Unicode.GetString(fileStreamInformation.StreamName);
                bool foundMatchedStream = streamList.ContainsKey(streamName);
                BaseTestSite.Assert.IsTrue(foundMatchedStream, "The stream with name {0} is found.", streamName);
                BaseTestSite.Assert.AreEqual(streamList[streamName], (long)fileStreamInformation.StreamSize,
                    "The StreamSize field of each of the returned FILE_STREAM_INFORMATION data elements should match the size of bytes written to each data stream.");
            }
        }

        #endregion
    }
}
