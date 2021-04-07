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
        [Description("Query FileBothDirectoryInformation on directory file and check if creation time is valid")]
        public void FileInfo_Query_FileBothDirectoryInformation_Dir_CreationTime()
        {
            FileInfo_Query_FileBothDirectoryInformation_CreationTime();
        }

        #endregion

        #region Test Case Utility

        private void FileInfo_Query_FileBothDirectoryInformation_CreationTime()
        {
            FileType fileType = FileType.DirectoryFile;

            //Step 1: Create File
            BaseTestSite.Log.Add(LogEntryKind.TestStep, "1. Create " + fileType.ToString());

            FILEID dirFileId;
            uint treeId;
            ulong sessionId;
            string dirName = this.fsaAdapter.ComposeRandomFileName(8);

            CreateDirectory(dirName, out dirFileId, out treeId, out sessionId);

            //Query FileBothDirectoryInformation 
            long creationTimeBeforeIO;
            byte[] outputBuffer;

            QueryDirectory(dirFileId, treeId, sessionId, FileInfoClass.FILE_BOTH_DIR_INFORMATION, out outputBuffer);
            QueryFileBothDirectoryInformationTimestamp(outputBuffer, 2, out _, out creationTimeBeforeIO, out _, out _);
            DelayNextStep();

            //Perform I/O operation
            //Step 3: Create file in the directory
            BaseTestSite.Log.Add(LogEntryKind.TestStep, "3. Create file in the directory");

            string innerFileName = this.fsaAdapter.ComposeRandomFileName(8);

            CreateFile(FileType.DataFile, dirName + "\\" + innerFileName);

            //Query FileBasicInformation
            long creationTimeAfterIO;

            QueryDirectory(dirFileId, treeId, sessionId, FileInfoClass.FILE_BOTH_DIR_INFORMATION, out outputBuffer);
            QueryFileBothDirectoryInformationTimestamp(outputBuffer, 3, out _, out creationTimeAfterIO, out _, out _);

            //Verify CreationTime has not changed
            BaseTestSite.Assert.AreEqual(creationTimeBeforeIO, creationTimeAfterIO, 
                "File creation time is never updated in response to file system calls such as read and write.");
        }

        private void QueryFileBothDirectoryInformationTimestamp(
            byte[] informationBuffer, int expectedEntryCount,
            out long changeTime, out long creationTime,
            out long lastAccessTime, out long lastWriteTime)
        {
            string fileName = ".";
            FileBothDirectoryInformation[] directoryInformation = FsaUtility.UnmarshalFileInformationArray<FileBothDirectoryInformation>(informationBuffer);
            
            Site.Assert.AreEqual(expectedEntryCount, directoryInformation.Length, $"The buffer should contain {expectedEntryCount} entries of FileBothDirectoryInformation.");
            Site.Assert.AreEqual(fileName, Encoding.Unicode.GetString(directoryInformation[0].FileName), $"FileName should be {fileName}.");

            changeTime = FileTimeToLong(directoryInformation[0].FileCommonDirectoryInformation.ChangeTime);
            creationTime = FileTimeToLong(directoryInformation[0].FileCommonDirectoryInformation.CreationTime);
            lastAccessTime = FileTimeToLong(directoryInformation[0].FileCommonDirectoryInformation.LastAccessTime);
            lastWriteTime = FileTimeToLong(directoryInformation[0].FileCommonDirectoryInformation.LastWriteTime);
        }

        private void CreateDirectory(
        string dirName,
        out FILEID fileId,
        out uint treeId,
        out ulong sessionId)
        {
            MessageStatus status = this.fsaAdapter.CreateFile(
                        dirName,
                        FileAttribute.DIRECTORY,
                        CreateOptions.DIRECTORY_FILE,
                        (FileAccess.GENERIC_READ | FileAccess.GENERIC_WRITE),
                        (ShareAccess.FILE_SHARE_READ | ShareAccess.FILE_SHARE_WRITE),
                        CreateDisposition.CREATE,
                        out fileId,
                        out treeId,
                        out sessionId
                        );

            BaseTestSite.Assert.AreEqual(MessageStatus.SUCCESS, status, "Create directory should succeed.");
        }

        private void QueryDirectory(
            FILEID dirFileId,
            uint treeId,
            ulong sessionId,
            FileInfoClass fileInfoClass,
            out byte[] outputBuffer,
            string searchPattern = "*"
            )
        {
            MessageStatus status = this.fsaAdapter.QueryDirectory(dirFileId, treeId, sessionId, searchPattern, fileInfoClass, false, true, out outputBuffer);

            BaseTestSite.Assert.AreEqual(MessageStatus.SUCCESS, status, "Query directory should succeed.");
        }
        #endregion
    }

}
