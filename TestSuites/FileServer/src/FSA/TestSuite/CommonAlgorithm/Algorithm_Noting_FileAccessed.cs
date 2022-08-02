// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Protocols.TestSuites.FileSharing.FSA.Adapter;
using Microsoft.Protocols.TestSuites.FileSharing.Common.Adapter;
using Microsoft.Protocols.TestTools;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Fscc;
using Microsoft.Protocols.TestTools.StackSdk;
using System;
using System.Threading;
using Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Smb2;

namespace Microsoft.Protocols.TestSuites.FileSharing.FSA.TestSuite
{
    public partial class CommonAlgorithmTestCases : PtfTestClassBase
    {
        #region Test Cases

        [TestMethod()]
        [TestCategory(TestCategories.Bvt)]
        [TestCategory(TestCategories.Fsa)]
        [TestCategory(TestCategories.CommonAlgorithm)]
        [TestCategory(TestCategories.NonSmb)]
        [TestCategory(TestCategories.Positive)]
        [Description("Try to read file and check if LastAccessTime is updated")]
        public void Algorithm_NotingFileAccessed_File_LastAccessTime()
        {
            FileAttributes attributesBeforeFileAccessed;
            FileAttributes attributesAfterFileAccessed;
            Algorithm_Noting_FileAccessed(FileType.DataFile, out attributesBeforeFileAccessed, out attributesAfterFileAccessed);
            TestLastAccessTime(attributesBeforeFileAccessed.lastAccessTime, attributesAfterFileAccessed.lastAccessTime);
        }

        [TestMethod()]
        [TestCategory(TestCategories.Bvt)]
        [TestCategory(TestCategories.Fsa)]
        [TestCategory(TestCategories.CommonAlgorithm)]
        [TestCategory(TestCategories.NonSmb)]
        [TestCategory(TestCategories.Positive)]
        [Description("Try to read directory and check if LastAccessTime is updated")]
        public void Algorithm_NotingFileAccessed_Dir_LastAccessTime()
        {
            FileAttributes attributesBeforeFileModified;
            FileAttributes attributesAfterFileAccessed;
            Algorithm_Noting_FileAccessed(FileType.DirectoryFile, out attributesBeforeFileModified, out attributesAfterFileAccessed);
            TestLastAccessTime(attributesBeforeFileModified.lastAccessTime, attributesAfterFileAccessed.lastAccessTime);
        }

        #endregion

        #region Test Case Utility

        private void Algorithm_Noting_FileAccessed(FileType fileType, out FileAttributes attributesBeforeFileAccessed,
            out FileAttributes attributesAfterFileAccessed)
        {
            BaseTestSite.Log.Add(LogEntryKind.TestStep, "Test case steps:");

            //Step 1: Create file
            BaseTestSite.Log.Add(LogEntryKind.TestStep, "1. Create " + fileType.ToString());

            FILEID dirFileId = FILEID.Zero;
            uint treeId = 0;
            ulong sessionId = 0;
            string fileName = this.fsaAdapter.ComposeRandomFileName(8);

            if (fileType == FileType.DataFile)
            {
                CreateFile(fileType, fileName);

                //Step 2: Write to file
                BaseTestSite.Log.Add(LogEntryKind.TestStep, "2. Write to file");

                //Write to file
                WriteToFile();
            }
            else
            {
                CreateDirectory(fileName, out dirFileId, out treeId, out sessionId);

                //Step 2: Create file in the directory
                BaseTestSite.Log.Add(LogEntryKind.TestStep, "2. Create file in the directory");

                string innerFileName = this.fsaAdapter.ComposeRandomFileName(8);
                CreateFile(FileType.DataFile, fileName + "\\" + innerFileName);

                //Return to directory
                OpenFile(fileType, fileName);
            }

            //Step 3: Query file basic information
            BaseTestSite.Log.Add(LogEntryKind.TestStep, "3. Query FILE_BASIC_INFORMATION for timestamps");

            QueryFileBasicInformation(out attributesBeforeFileAccessed);
            DelayNextStep();

            if (fileType == FileType.DataFile)
            {
                //Step 4: Read file
                BaseTestSite.Log.Add(LogEntryKind.TestStep, "4. Read file");

                //Read file content
                ReadFile();
            }
            else
            {
                //Step 4: Query files in directory
                BaseTestSite.Log.Add(LogEntryKind.TestStep, "4. Query the directory");

                QueryDirectory(dirFileId, treeId, sessionId, FileInfoClass.FILE_BOTH_DIR_INFORMATION, out _);
            }

            //Step 5: Close and open file
            BaseTestSite.Log.Add(LogEntryKind.TestStep, "5. Close and open file to give significant time a file system can defer processing Section 2.1.4.17");

            //<42> Section 2.1.4.17: File systems may choose to defer processing for a file that has been modified 
            // to a later time, favoring performance over accuracy.
            this.fsaAdapter.CloseOpen();
            OpenFile(fileType, fileName);

            //Step 6: Query FileBasicInformation
            BaseTestSite.Log.Add(LogEntryKind.TestStep, "6. Query FILE_BASIC_INFORMATION for file attributes");

            QueryFileBasicInformation(out attributesAfterFileAccessed);

            //Step 7: Verify that FileBasicInformation attributes are updated appropriately
            BaseTestSite.Log.Add(LogEntryKind.TestStep, "7. Verify that FILE_BASIC_INFORMATION attributes are updated appropriately");

            string creationTimeDateBeforeFileAccessed = attributesBeforeFileAccessed.creationTime.ToString();
            string creationTimeDateAfterFileAccessed = attributesAfterFileAccessed.creationTime.ToString();

            BaseTestSite.Assert.AreEqual(creationTimeDateBeforeFileAccessed, creationTimeDateAfterFileAccessed,
                "File creation time is never updated in response to file system calls such as read and write.");
        }

        public void ReadFile()
        {
            int byteSize = 2 * 1024 * (int)this.fsaAdapter.ClusterSizeInKB;
            MessageStatus status = this.fsaAdapter.ReadFile(0, byteSize, out _);

            this.fsaAdapter.AssertAreEqual(this.Manager, MessageStatus.SUCCESS, status,
                    "Read data from file should succeed");
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
            MessageStatus status = this.fsaAdapter.QueryDirectory(dirFileId, treeId, sessionId, 
                searchPattern, fileInfoClass, false, true, out outputBuffer);
        }

        #endregion
    }
}
