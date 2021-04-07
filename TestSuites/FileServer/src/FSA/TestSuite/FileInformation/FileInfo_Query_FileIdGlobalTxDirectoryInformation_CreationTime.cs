// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Protocols.TestSuites.FileSharing.Common.Adapter;
using Microsoft.Protocols.TestSuites.FileSharing.FSA.Adapter;
using Microsoft.Protocols.TestTools;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Text;
using Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Fscc;

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
        [Description("Query FileIdGlobalTxDirectoryInformation on directory file and check if creation time is valid")]
        public void FileInfo_Query_FileIdGlobalTxDirectoryInformation_Dir_CreationTime()
        {
            FileInfo_Query_FileIdGlobalTxDirectoryInformation_CreationTime();
        }

        #endregion

        #region Test Case Utility

        private void FileInfo_Query_FileIdGlobalTxDirectoryInformation_CreationTime()
        {
            FileType fileType = FileType.DirectoryFile;

            //Step 1: Create File
            BaseTestSite.Log.Add(LogEntryKind.TestStep, "1. Create " + fileType.ToString());

            TestTools.StackSdk.FileAccessService.Smb2.FILEID dirFileId;
            uint treeId;
            ulong sessionId;
            string dirName = this.fsaAdapter.ComposeRandomFileName(8);

            CreateDirectory(dirName, out dirFileId, out treeId, out sessionId);

            //Query FileBothDirectoryInformation 
            byte[] outputBuffer;
            long creationTimeBeforeIO;

            FileFsAttributeInformation fileFsAttributeInformation;
            this.fsaAdapter.QueryFileFsAttributeInformation(out fileFsAttributeInformation);

            bool supportTransactions = (fileFsAttributeInformation.FileSystemAttributes & 
                FileSystemAttributes_Values.FILE_SUPPORTS_TRANSACTIONS)
                == FileSystemAttributes_Values.FILE_SUPPORTS_TRANSACTIONS;
            
            if(!supportTransactions)
            {
                BaseTestSite.Assume.Inconclusive("FileIdGlobalTxDirectoryInformation MUST NOT be implemented for file systems that do not return the FILE_SUPPORTS_TRANSACTIONS flag.");
            }

            QueryDirectory(dirFileId, treeId, sessionId, FileInfoClass.FILE_ID_GLOBAL_TX_DIR_INFORMATION, out outputBuffer);
            QueryFileIdGlobalTxDirectoryInformationTimestamp(outputBuffer, 2, out _, out creationTimeBeforeIO, out _, out _);
            DelayNextStep();

            //Perform I/O operation
            //Step 3: Create file in the directory
            BaseTestSite.Log.Add(LogEntryKind.TestStep, "3. Create file in the directory");

            string innerFileName = this.fsaAdapter.ComposeRandomFileName(8);

            CreateFile(FileType.DataFile, dirName + "\\" + innerFileName);

            //Query FileBasicInformation
            long creationTimeAfterIO;

            QueryDirectory(dirFileId, treeId, sessionId, FileInfoClass.FILE_ID_GLOBAL_TX_DIR_INFORMATION, out outputBuffer);
            QueryFileIdGlobalTxDirectoryInformationTimestamp(outputBuffer, 2, out _, out creationTimeAfterIO, out _, out _);

            //Verify CreationTime has not changed
            BaseTestSite.Assert.AreEqual(creationTimeBeforeIO, creationTimeAfterIO,
                "File creation time is never updated in response to file system calls such as read and write.");
        }

        private void QueryFileIdGlobalTxDirectoryInformationTimestamp(
            byte[] informationBuffer, int expectedEntryCount,
            out long changeTime, out long creationTime,
            out long lastAccessTime, out long lastWriteTime)
        {
            string fileName = ".";
            FileIdGlobalTxDirectoryInformation[] directoryInformation = FsaUtility.UnmarshalFileInformationArray<FileIdGlobalTxDirectoryInformation>(informationBuffer);

            Site.Assert.AreEqual(expectedEntryCount, directoryInformation.Length, $"The buffer should contain {expectedEntryCount} entries of FileIdGlobalTxDirectoryInformation.");
            Site.Assert.AreEqual(fileName, Encoding.Unicode.GetString(directoryInformation[0].FileName), $"FileName should be {fileName}.");

            changeTime = FileTimeToLong(directoryInformation[0].FileCommonDirectoryInformation.ChangeTime);
            creationTime = FileTimeToLong(directoryInformation[0].FileCommonDirectoryInformation.CreationTime);
            lastAccessTime = FileTimeToLong(directoryInformation[0].FileCommonDirectoryInformation.LastAccessTime);
            lastWriteTime = FileTimeToLong(directoryInformation[0].FileCommonDirectoryInformation.LastWriteTime);
        }
        #endregion
    }

}
