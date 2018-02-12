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
        [TestCategory(TestCategories.SetFileInformation)]
        [TestCategory(TestCategories.NonSmb)]
        [TestCategory(TestCategories.Positive)]
        [Description("Try to set FileValidDataLengthInfo to a file and check if valid data length information is supported.")]
        public void FileInfo_Set_FileValidDataLengthInfo_File_IsSupported()
        {
            FileInfo_Set_FileValidDataLengthInformation_IsSupported(FileType.DataFile);
        }

        [TestMethod()]
        [TestCategory(TestCategories.Fsa)]
        [TestCategory(TestCategories.SetFileInformation)]
        [TestCategory(TestCategories.NonSmb)]
        [TestCategory(TestCategories.Positive)]
        [Description("Try to set FileValidDataLengthInfo to a directory and check if valid data length information is supported.")]
        public void FileInfo_Set_FileValidDataLengthInfo_Dir_IsSupported()
        {
            FileInfo_Set_FileValidDataLengthInformation_IsSupported(FileType.DirectoryFile);
        }

        #endregion

        #region Test Case Utility

        private void FileInfo_Set_FileValidDataLengthInformation_IsSupported(FileType fileType)
        {
            BaseTestSite.Log.Add(LogEntryKind.TestStep, "Test case steps:");
            MessageStatus status;

            //Step 1: Create file
            BaseTestSite.Log.Add(LogEntryKind.TestStep, "1. Create " + fileType.ToString());
            status = this.fsaAdapter.CreateFile(fileType);

            //Step 2: Write some bytes into the file
            long bytesToWrite;
            if (fileType == FileType.DataFile)
            {
                //Write some bytes into the DataFile.
                bytesToWrite = 1024;
                long bytesWritten = 0;
                BaseTestSite.Log.Add(LogEntryKind.TestStep, "2. Write the file with " + bytesToWrite + " bytes data.");
                status = this.fsaAdapter.WriteFile(0, bytesToWrite, out bytesWritten);
            }
            else
            {
                BaseTestSite.Log.Add(LogEntryKind.TestStep, "2. Do not write data into DirectoryFile.");
                bytesToWrite = 0;
            }

            //Step 3: Set FILE_VALIDDATALENGTH_INFORMATION
            BaseTestSite.Log.Add(LogEntryKind.TestStep, "3. SetFileInformation with FileInfoClass.FILE_VALIDDATALENGTH_INFORMATION.");
            FileValidDataLengthInformation fileValidDataLengthInfo = new FileValidDataLengthInformation();

            BaseTestSite.Log.Add(LogEntryKind.Debug, "Parameter: Set ValidDataLength to " + bytesToWrite);
            fileValidDataLengthInfo.ValidDataLength = bytesToWrite;

            byte[] inputBuffer = TypeMarshal.ToBytes<FileValidDataLengthInformation>(fileValidDataLengthInfo);
            
            status = this.fsaAdapter.SetFileInformation(FileInfoClass.FILE_VALIDDATALENGTH_INFORMATION, inputBuffer);

            //Step 4: Verify test result
            BaseTestSite.Log.Add(LogEntryKind.TestStep, "4. Verify returned NTSTATUS code.");
            if (fileType == FileType.DataFile)
            {
                this.fsaAdapter.AssertAreEqual(this.Manager, MessageStatus.SUCCESS, status, "Status set to STATUS_SUCCESS.");
            }
            else
            {
                this.fsaAdapter.AssertAreEqual(this.Manager, MessageStatus.INVALID_PARAMETER, status,
                    "The operation MUST be failed with STATUS_INVALID_PARAMETER if Open.File.FileType is DirectoryFile.");
            }
        }

        #endregion
    }
}
