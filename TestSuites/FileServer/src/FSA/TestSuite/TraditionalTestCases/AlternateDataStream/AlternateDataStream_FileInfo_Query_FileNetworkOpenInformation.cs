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
        [Description("Query FileNetworkOpenInformation from an Alternate Data Stream on a DataFile.")]
        public void AlternateDataStream_Query_FileNetworkOpenInformation_File()
        {
            AlternateDataStream_Query_FileNetworkOpenInformation(FileType.DataFile);
        }

        [TestMethod()]
        [TestCategory(TestCategories.Fsa)]
        [TestCategory(TestCategories.QueryFileInformation)]
        [TestCategory(TestCategories.AlternateDataStream)]
        [Description("Query FileNetworkOpenInformation from an Alternate Data Stream on a DirectoryFile.")]
        public void AlternateDataStream_Query_FileNetworkOpenInformation_Dir()
        {
            AlternateDataStream_Query_FileNetworkOpenInformation(FileType.DirectoryFile);
        }

        #endregion

        #region Test Case Utility

        private void AlternateDataStream_Query_FileNetworkOpenInformation(FileType fileType)
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

            //Step 5: Query FILE_NETWORKOPEN_INFORMATION
            long byteCount;
            byte[] outputBuffer;
            FileNetworkOpenInformation fileNetworkOpenInfo = new FileNetworkOpenInformation();
            uint outputBufferSize = (uint)TypeMarshal.ToBytes<FileNetworkOpenInformation>(fileNetworkOpenInfo).Length;

            BaseTestSite.Log.Add(LogEntryKind.TestStep, "5. QueryFileInformation with FileInfoClass.FILE_NETWORKOPEN_INFORMATION");
            status = this.fsaAdapter.QueryFileInformation(FileInfoClass.FILE_NETWORKOPEN_INFORMATION, outputBufferSize, out byteCount, out outputBuffer);

            // Step 6: Verify FILE_NETWORKOPEN_INFORMATION
            fileNetworkOpenInfo = TypeMarshal.ToStruct<FileNetworkOpenInformation>(outputBuffer);
            bool isIntegrityStreamSet = (fileNetworkOpenInfo.FileAttributes & (uint)FileAttribute.INTEGRITY_STREAM) == (uint)FileAttribute.INTEGRITY_STREAM;

            BaseTestSite.Log.Add(LogEntryKind.TestStep, "6. Verify outputBuffer.FileAttributes.FILE_ATTRIBUTE_INTEGRITY_STREAM");
            if (this.fsaAdapter.IsIntegritySupported == true)
            {
                this.fsaAdapter.AssertAreEqual(this.Manager, true, isIntegrityStreamSet,
                    "If integrity is supported, the object store MUST set FILE_ATTRIBUTE_INTEGRITY_STREAM in OutputBuffer.FileAttributes.");
            }
            else
            {
                this.fsaAdapter.AssertAreEqual(this.Manager, false, isIntegrityStreamSet, "Integrity is not supported, FILE_ATTRIBUTE_INTEGRITY_STREAM MUST NOT set.");
            }
        }

        #endregion

    }
}