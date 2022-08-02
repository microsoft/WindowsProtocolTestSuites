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
        [Description("Try to write to file and check if file system consistent with [MS-FSA] section 2.1.4.17")]
        public void Algorithm_NotingFileModified_File_LastModificationTime()
        {
            FileAttributes attributesBeforeFileModified;
            FileAttributes attributesAfterFileModified;
            Algorithm_Noting_FileModified(FileType.DataFile, out attributesBeforeFileModified, out attributesAfterFileModified);
            TestLastWriteTime(attributesBeforeFileModified.lastWriteTime, attributesAfterFileModified.lastWriteTime);
        }

        [TestMethod()]
        [TestCategory(TestCategories.Bvt)]
        [TestCategory(TestCategories.Fsa)]
        [TestCategory(TestCategories.CommonAlgorithm)]
        [TestCategory(TestCategories.NonSmb)]
        [TestCategory(TestCategories.Positive)]
        [Description("Try to write to file and check if file system consistent with [MS-FSA] section 2.1.4.17")]
        public void Algorithm_NotingFileModified_File_LastAccessTime()
        {
            FileAttributes attributesBeforeFileModified;
            FileAttributes attributesAfterFileModified;
            Algorithm_Noting_FileModified(FileType.DataFile, out attributesBeforeFileModified, out attributesAfterFileModified);
            TestLastAccessTime(attributesBeforeFileModified.lastAccessTime, attributesAfterFileModified.lastAccessTime);
        }

        [TestMethod()]
        [TestCategory(TestCategories.Bvt)]
        [TestCategory(TestCategories.Fsa)]
        [TestCategory(TestCategories.CommonAlgorithm)]
        [TestCategory(TestCategories.NonSmb)]
        [TestCategory(TestCategories.Positive)]
        [Description("Try to write to file and check if file system consistent with [MS-FSA] section 2.1.4.17")]
        public void Algorithm_NotingFileModified_File_LastChangeTime()
        {
            FileAttributes attributesBeforeFileModified;
            FileAttributes attributesAfterFileModified;
            Algorithm_Noting_FileModified(FileType.DataFile, out attributesBeforeFileModified, out attributesAfterFileModified);
            TestChangeTime(attributesBeforeFileModified.changeTime, attributesAfterFileModified.changeTime);
        }

        [TestMethod()]
        [TestCategory(TestCategories.Bvt)]
        [TestCategory(TestCategories.Fsa)]
        [TestCategory(TestCategories.CommonAlgorithm)]
        [TestCategory(TestCategories.NonSmb)]
        [TestCategory(TestCategories.Positive)]
        [Description("Try to write to file and check if file system consistent with [MS-FSA] section 2.1.4.17")]
        public void Algorithm_NotingFileModified_File_Archive()
        {
            FileAttributes attributesAfterFileModified;
            Algorithm_Noting_FileModified(FileType.DataFile, out _, out attributesAfterFileModified);
            TestArchive(attributesAfterFileModified.isArchive);            
        }

        [TestMethod()]
        [TestCategory(TestCategories.Bvt)]
        [TestCategory(TestCategories.Fsa)]
        [TestCategory(TestCategories.CommonAlgorithm)]
        [TestCategory(TestCategories.NonSmb)]
        [TestCategory(TestCategories.Positive)]
        [Description("Try to create file in directory and check if file system consistent with [MS-FSA] section 2.1.4.17")]
        public void Algorithm_NotingFileModified_Dir_LastModificationTime()
        {
            FileAttributes attributesBeforeFileModified;
            FileAttributes attributesAfterFileModified;
            Algorithm_Noting_FileModified(FileType.DirectoryFile, out attributesBeforeFileModified, out attributesAfterFileModified);

            if (this.fsaAdapter.FileSystem == FileSystem.FAT32)
            {
                BaseTestSite.Assert.Inconclusive("FAT32 is inconclusive for Open.File.LastWriteTime with directory");
            }
            else
            {
                TestLastWriteTime(attributesBeforeFileModified.lastWriteTime, attributesAfterFileModified.lastWriteTime);
            }
        }

        [TestMethod()]
        [TestCategory(TestCategories.Bvt)]
        [TestCategory(TestCategories.Fsa)]
        [TestCategory(TestCategories.CommonAlgorithm)]
        [TestCategory(TestCategories.NonSmb)]
        [TestCategory(TestCategories.Positive)]
        [Description("Try to create file in directory and check if file system consistent with [MS-FSA] section 2.1.4.17")]
        public void Algorithm_NotingFileModified_Dir_LastChangeTime()
        {
            FileAttributes attributesBeforeFileModified;
            FileAttributes attributesAfterFileModified;
            Algorithm_Noting_FileModified(FileType.DirectoryFile, out attributesBeforeFileModified, out attributesAfterFileModified);
            TestChangeTime(attributesBeforeFileModified.changeTime, attributesAfterFileModified.changeTime);
        }

        [TestMethod()]
        [TestCategory(TestCategories.Bvt)]
        [TestCategory(TestCategories.Fsa)]
        [TestCategory(TestCategories.CommonAlgorithm)]
        [TestCategory(TestCategories.NonSmb)]
        [TestCategory(TestCategories.Positive)]
        [Description("Try to create file in directory and check if file system consistent with [MS-FSA] section 2.1.4.17")]
        public void Algorithm_NotingFileModified_Dir_LastAccessTime()
        {
            FileAttributes attributesBeforeFileModified;
            FileAttributes attributesAfterFileModified;
            Algorithm_Noting_FileModified(FileType.DirectoryFile, out attributesBeforeFileModified, out attributesAfterFileModified);
            TestLastAccessTime(attributesBeforeFileModified.lastAccessTime, attributesAfterFileModified.lastAccessTime);
            
        }

        [TestMethod()]
        [TestCategory(TestCategories.Bvt)]
        [TestCategory(TestCategories.Fsa)]
        [TestCategory(TestCategories.CommonAlgorithm)]
        [TestCategory(TestCategories.NonSmb)]
        [TestCategory(TestCategories.Positive)]
        [Description("Try to create file in directory and check if file system consistent with [MS-FSA] section 2.1.4.17")]
        public void Algorithm_NotingFileModified_Dir_Archive()
        {
            FileAttributes attributesAfterFileModified;
            Algorithm_Noting_FileModified(FileType.DirectoryFile, out _, out attributesAfterFileModified);

            if (this.fsaAdapter.FileSystem == FileSystem.NTFS
                || this.fsaAdapter.FileSystem == FileSystem.REFS
                || this.fsaAdapter.FileSystem == FileSystem.FAT32)
            {
                BaseTestSite.Assert.Inconclusive("NTFS, REFS and FAT32 are inconclusive for Open.File.FileAttributes.FILE_ATTRIBUTE_ARCHIVE on dirctory");
            }
            else
            {
                TestArchive(attributesAfterFileModified.isArchive);
            }
        }

        #endregion

        #region Test Case Utility

        private void Algorithm_Noting_FileModified(FileType fileType, out FileAttributes attributesBeforeFileModified, 
            out FileAttributes attributesAfterFileModified)
        {
            BaseTestSite.Log.Add(LogEntryKind.TestStep, "Test case steps:");
            
            //Step 1: Create file
            BaseTestSite.Log.Add(LogEntryKind.TestStep, "1. Create " + fileType.ToString());

            string fileName = this.fsaAdapter.ComposeRandomFileName(8);
            CreateFile(fileType, fileName);

            //Step 2: Query file basic information
            BaseTestSite.Log.Add(LogEntryKind.TestStep, "2. Query FILE_BASIC_INFORMATION for timestamps");

            QueryFileBasicInformation(out attributesBeforeFileModified);
            DelayNextStep();

            if(fileType == FileType.DataFile)
            {
                //Step 3: Write to file
                BaseTestSite.Log.Add(LogEntryKind.TestStep, "3. Write to file");

                //Write to file
                WriteToFile();
            }
            else
            {
                //Step 3: Create file in the directory
                BaseTestSite.Log.Add(LogEntryKind.TestStep, "3. Create file in the directory");

                string innerFileName = this.fsaAdapter.ComposeRandomFileName(8);

                CreateFile(FileType.DataFile, fileName + "\\" + innerFileName);
            }

            //Step 4: Close and open file
            BaseTestSite.Log.Add(LogEntryKind.TestStep, "4. Close and open file to give significant time a file system can defer processing Section 2.1.4.17");

            //<42> Section 2.1.4.17: File systems may choose to defer processing for a file that has been modified 
            // to a later time, favoring performance over accuracy.
            this.fsaAdapter.CloseOpen();
            OpenFile(fileType ,fileName);

            //Step 5: Query FileBasicInformation
            BaseTestSite.Log.Add(LogEntryKind.TestStep, "5. Query FILE_BASIC_INFORMATION for file attributes");

            QueryFileBasicInformation(out attributesAfterFileModified);

            //Step 6: Verify that FileBasicInformation attributes are updated appropriately
            BaseTestSite.Log.Add(LogEntryKind.TestStep, "6. Verify that FILE_BASIC_INFORMATION attributes are updated appropriately");

            string creationTimeDateBeforeFileModified = attributesBeforeFileModified.creationTime.ToString();
            string creationTimeDateAfterFileModified = attributesAfterFileModified.creationTime.ToString();

            BaseTestSite.Assert.AreEqual(creationTimeDateBeforeFileModified, creationTimeDateAfterFileModified,
                "File creation time is never updated in response to file system calls such as read and write.");
        }

        private void TestLastWriteTime(DateTime lastWriteTimeDateBeforeFileModified, DateTime lastWriteTimeDateAfterFileModified)
        {
            if (lastWriteTimeDateBeforeFileModified.CompareTo(lastWriteTimeDateAfterFileModified) >= 0)
            {
                BaseTestSite.Assert.Fail("The object store SHOULD set Open.File.LastModificationTime to the current system time.");
            }
            else
            {
                BaseTestSite.Assert.Pass("The object store SHOULD set Open.File.LastModificationTime to the current system time.");
            }
        }

        private void TestLastAccessTime(DateTime lastAccessTimeDateBeforeFileModified, DateTime lastAccessTimeDateAfterFileModified)
        {
            if (this.fsaAdapter.FileSystem == FileSystem.FAT32)
            {
                BaseTestSite.Assert.Inconclusive("FAT32 is inconclusive for Open.File.LastAccessTime");
            }
            else if (lastAccessTimeDateBeforeFileModified.CompareTo(lastAccessTimeDateAfterFileModified) >= 0)
            {
                BaseTestSite.Assert.Fail("The object store SHOULD set Open.File.LastAccessTime to the current system time.");
            }
            else
            {
                BaseTestSite.Assert.Pass("The object store SHOULD set Open.File.LastAccessTime to the current system time.");
            }
        }

        private void TestChangeTime(DateTime changeTimeDateBeforeFileModified, DateTime changeTimeDateAferFileModified)
        {            
            if (this.fsaAdapter.FileSystem == FileSystem.FAT32)
            {
                this.TestSite.Assume.Inconclusive("<153> Section 2.1.5.14.2: The FAT32 file system doesn’t process the ChangeTime field.");
            }
            else if (changeTimeDateBeforeFileModified.CompareTo(changeTimeDateAferFileModified) >= 0)
            {
                BaseTestSite.Assert.Fail("The object store SHOULD set Open.File.LastChangeTime to the current system time.");
            }
            else
            {
                BaseTestSite.Assert.Pass("The object store SHOULD set Open.File.LastChangeTime to the current system time.");
            }
        }

        private void TestArchive(bool isArchive)
        {
            //Verify FILE_ATTRIBUTE_ARCHIVE is set to TRUE
            if (isArchive == false)
            {
                BaseTestSite.Assert.Fail("The object store SHOULD set Open.File.FileAttributes.FILE_ATTRIBUTE_ARCHIVE to TRUE.");
            }
            else
            {
                BaseTestSite.Assert.Pass("The object store SHOULD set Open.File.FileAttributes.FILE_ATTRIBUTE_ARCHIVE to TRUE.");
            }
        }

        private void OpenFile(FileType fileType, string fileName)
        {
            MessageStatus status = this.fsaAdapter.CreateFile(
                fileName,
                FileAttribute.NORMAL,
                fileType == FileType.DataFile ? CreateOptions.NON_DIRECTORY_FILE : CreateOptions.DIRECTORY_FILE,
                FileAccess.GENERIC_READ | FileAccess.GENERIC_WRITE | FileAccess.FILE_WRITE_DATA | FileAccess.FILE_WRITE_ATTRIBUTES,
                ShareAccess.FILE_SHARE_READ | ShareAccess.FILE_SHARE_WRITE,
                CreateDisposition.OPEN);

            BaseTestSite.Assert.AreEqual(MessageStatus.SUCCESS, status, "Open should succeed.");
        }

        private void CreateFile(FileType fileType, string fileName)
        {
            MessageStatus status = this.fsaAdapter.CreateFile(
                fileName,
                FileAttribute.NORMAL,
                fileType == FileType.DataFile ? CreateOptions.NON_DIRECTORY_FILE : CreateOptions.DIRECTORY_FILE,
                FileAccess.GENERIC_READ | FileAccess.GENERIC_WRITE | FileAccess.FILE_WRITE_DATA | FileAccess.FILE_WRITE_ATTRIBUTES,
                ShareAccess.FILE_SHARE_READ | ShareAccess.FILE_SHARE_WRITE,
                CreateDisposition.CREATE);

            BaseTestSite.Assert.AreEqual(MessageStatus.SUCCESS, status, "Create should succeed.");
        }

        private void QueryFileBasicInformation(out FileAttributes fileAttributes)
        {
            FileBasicInformation fileBasicInformation = new FileBasicInformation();
            uint outputBufferSize = (uint)TypeMarshal.ToBytes<FileBasicInformation>(fileBasicInformation).Length;
            byte[] outputBuffer;
            this.fsaAdapter.QueryFileInformation(FileInfoClass.FILE_BASIC_INFORMATION, outputBufferSize, out _, out outputBuffer);

            fileBasicInformation = TypeMarshal.ToStruct<FileBasicInformation>(outputBuffer);
            fileAttributes.changeTime = DateTime.FromFileTime((((long)fileBasicInformation.ChangeTime.dwHighDateTime) << 32) + fileBasicInformation.ChangeTime.dwLowDateTime);
            fileAttributes.creationTime = DateTime.FromFileTime((((long)fileBasicInformation.CreationTime.dwHighDateTime) << 32) + fileBasicInformation.CreationTime.dwLowDateTime);
            fileAttributes.lastAccessTime = DateTime.FromFileTime((((long)fileBasicInformation.LastAccessTime.dwHighDateTime) << 32) + fileBasicInformation.LastAccessTime.dwLowDateTime);
            fileAttributes.lastWriteTime = DateTime.FromFileTime((((long)fileBasicInformation.LastWriteTime.dwHighDateTime) << 32) + fileBasicInformation.LastWriteTime.dwLowDateTime);
            fileAttributes.isArchive = (fileBasicInformation.FileAttributes & (uint)FileAttribute.ARCHIVE) == (uint)FileAttribute.ARCHIVE;
        }

        private struct FileAttributes
        {
            public DateTime changeTime;
            public DateTime creationTime;
            public DateTime lastAccessTime;
            public DateTime lastWriteTime;
            public bool isArchive;
        }

        private void WriteToFile()
        {
            long byteSize = (uint)2 * 1024 * this.fsaAdapter.ClusterSizeInKB;
            MessageStatus status = this.fsaAdapter.WriteFile(0, byteSize, out _);

            this.fsaAdapter.AssertAreEqual(this.Manager, MessageStatus.SUCCESS, status,
                    "Write data to file should succeed");
        }

        private int GetDelaTimeByFileSystem(FileSystem fileSystem)
        {
            //Applying delay time considering File System resolution time MS-FSCC 2.1.1 FSBO Section 6 
            switch (fileSystem)
            {
                case FileSystem.NTFS:
                    return 1000;
                case FileSystem.FAT32:
                    return 3000;
                default:
                    return 3000;
            }
        }

        private void DelayNextStep()
        {
            int delayTime = GetDelaTimeByFileSystem(this.fsaAdapter.FileSystem);
            Thread.Sleep(delayTime);
        }
        #endregion
    }
}
