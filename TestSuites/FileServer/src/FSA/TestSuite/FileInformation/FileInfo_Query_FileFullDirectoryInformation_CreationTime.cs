// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Protocols.TestSuites.FileSharing.Common.Adapter;
using Microsoft.Protocols.TestSuites.FileSharing.FSA.Adapter;
using Microsoft.Protocols.TestTools;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Fscc;
using Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Smb2;
using System.Text;

namespace Microsoft.Protocols.TestSuites.FileSharing.FSA.TestSuite
{
    public partial class FileInfoTestCases : PtfTestClassBase
    {
        #region Test Cases

        [TestMethod()]
        [TestCategory(TestCategories.Bvt)]
        [TestCategory(TestCategories.Fsa)]
        [TestCategory(TestCategories.QueryFileInformation)]
        [TestCategory(TestCategories.NonSmb)]
        [TestCategory(TestCategories.Positive)]
        [Description("Query FileFullDirectoryInformation on directory and check if creation time is valid")]
        public void FileInfo_Query_FileFullDirectoryInformation_Dir_CreationTime()
        {
            FileInfo_Query_FileFullDirectoryInformation_CreationTime();
        }

        #endregion

        #region Test Case Utility

        private void FileInfo_Query_FileFullDirectoryInformation_CreationTime()
        {
            FileType fileType = FileType.DirectoryFile;

            //Step 1: Create File
            BaseTestSite.Log.Add(LogEntryKind.TestStep, "1. Create " + fileType.ToString() + " with FileAccess.FILE_WRITE_ATTRIBUTES");

            FILEID dirFileId;
            uint treeId;
            ulong sessionId;
            string dirName = this.fsaAdapter.ComposeRandomFileName(8);

            CreateDirectory(dirName, out dirFileId, out treeId, out sessionId);

            //Query FileBothDirectoryInformation 
            byte[] outputBuffer;
            long creationTimeBeforeIO;

            QueryDirectory(dirFileId, treeId, sessionId, FileInfoClass.FILE_FULL_DIR_INFORMATION, out outputBuffer);
            QueryFileFullDirectoryInformationTimestamp(outputBuffer, 2, out _, out creationTimeBeforeIO, out _, out _);

            //Perform I/O operation
            //Step 3: Create file in the directory
            BaseTestSite.Log.Add(LogEntryKind.TestStep, "3. Create file in the directory");

            string innerFileName = this.fsaAdapter.ComposeRandomFileName(8);

            CreateFile(FileType.DataFile, dirName + "\\" + innerFileName);

            //Query FileBasicInformation
            long creationTimeAfterIO;

            QueryDirectory(dirFileId, treeId, sessionId, FileInfoClass.FILE_FULL_DIR_INFORMATION, out outputBuffer);
            QueryFileFullDirectoryInformationTimestamp(outputBuffer, 3, out _, out creationTimeAfterIO, out _, out _);

            //Verify CreationTime has not changed
            BaseTestSite.Assert.AreEqual(creationTimeBeforeIO, creationTimeAfterIO,
                "File creation time is never updated in response to file system calls such as read and write.");
        }

        private void QueryFileFullDirectoryInformationTimestamp(
            byte[] informationBuffer, int expectedEntryCount,
            out long changeTime, out long creationTime,
            out long lastAccessTime, out long lastWriteTime)
        {
            string fileName = ".";
            FileFullDirectoryInformation[] directoryInformation = FsaUtility.UnmarshalFileInformationArray<FileFullDirectoryInformation>(informationBuffer);

            Site.Assert.AreEqual(expectedEntryCount, directoryInformation.Length, $"The buffer should contain {expectedEntryCount} entries of FileFullDirectoryInformation.");
            Site.Assert.AreEqual(fileName, Encoding.Unicode.GetString(directoryInformation[0].FileName), $"FileName should be {fileName}.");

            changeTime = (((long)directoryInformation[0].FileCommonDirectoryInformation.ChangeTime.dwHighDateTime) << 32)
                + directoryInformation[0].FileCommonDirectoryInformation.ChangeTime.dwLowDateTime;
            creationTime = (((long)directoryInformation[0].FileCommonDirectoryInformation.CreationTime.dwHighDateTime) << 32)
                + directoryInformation[0].FileCommonDirectoryInformation.CreationTime.dwLowDateTime;
            lastAccessTime = (((long)directoryInformation[0].FileCommonDirectoryInformation.LastAccessTime.dwHighDateTime) << 32)
                + directoryInformation[0].FileCommonDirectoryInformation.LastAccessTime.dwLowDateTime;
            lastWriteTime = (((long)directoryInformation[0].FileCommonDirectoryInformation.LastWriteTime.dwHighDateTime) << 32)
                + directoryInformation[0].FileCommonDirectoryInformation.LastWriteTime.dwLowDateTime;
        }
        #endregion
    }

}
