// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Protocols.TestSuites.FileSharing.Common.Adapter;
using Microsoft.Protocols.TestSuites.FileSharing.FSA.Adapter;
using Microsoft.Protocols.TestTools;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Threading;

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
        [Description("Query FileBasicInformation on data file and check if creation time is valid")]
        public void FileInfo_Query_FileBasicInformation_File_CreationTime()
        {
            FileInfo_Query_FileBasicInformation_CreationTime(TestType.IO, FileType.DataFile);
        }

        [TestMethod()]
        [TestCategory(TestCategories.Fsa)]
        [TestCategory(TestCategories.QueryFileInformation)]
        [TestCategory(TestCategories.NonSmb)]
        [TestCategory(TestCategories.UnexpectedFields)]
        [Description("Query FileBasicInformation on directory file and check if creation time is valid")]
        public void FileInfo_Query_FileBasicInformation_File_CreationTime_Negative()
        {
            TestCreationTimeNegative(FileType.DataFile);
        }

        [TestMethod()]
        [TestCategory(TestCategories.Bvt)]
        [TestCategory(TestCategories.Fsa)]
        [TestCategory(TestCategories.QueryFileInformation)]
        [TestCategory(TestCategories.NonSmb)]
        [TestCategory(TestCategories.Positive)]
        [Description("Query FileBasicInformation on directory file and check if creation time is valid")]
        public void FileInfo_Query_FileBasicInformation_Dir_CreationTime()
        {
            FileInfo_Query_FileBasicInformation_CreationTime(TestType.IO, FileType.DirectoryFile);
        }

        [TestMethod()]
        [TestCategory(TestCategories.Fsa)]
        [TestCategory(TestCategories.QueryFileInformation)]
        [TestCategory(TestCategories.NonSmb)]
        [TestCategory(TestCategories.UnexpectedFields)]
        [Description("Query FileBasicInformation on data file and check if creation time is valid")]
        public void FileInfo_Query_FileBasicInformation_File_CreationTime_Zero()
        {
            FileInfo_Query_FileBasicInformation_CreationTime(TestType.Zero, FileType.DataFile);
        }

        [TestMethod()]
        [TestCategory(TestCategories.Fsa)]
        [TestCategory(TestCategories.QueryFileInformation)]
        [TestCategory(TestCategories.NonSmb)]
        [TestCategory(TestCategories.UnexpectedFields)]
        [Description("Query FileBasicInformation on directory file and check if creation time is valid")]
        public void FileInfo_Query_FileBasicInformation_Dir_CreationTime_Zero()
        {
            FileInfo_Query_FileBasicInformation_CreationTime(TestType.Zero, FileType.DirectoryFile);
        }

        [TestMethod()]
        [TestCategory(TestCategories.Fsa)]
        [TestCategory(TestCategories.QueryFileInformation)]
        [TestCategory(TestCategories.NonSmb)]
        [TestCategory(TestCategories.UnexpectedFields)]
        [Description("Query FileBasicInformation on data file and check if creation time is valid")]
        public void FileInfo_Query_FileBasicInformation_File_CreationTime_MinusTwo()
        {
            TestCreationTimeMinusTwo(FileType.DataFile);
        }

        [TestMethod()]
        [TestCategory(TestCategories.Fsa)]
        [TestCategory(TestCategories.QueryFileInformation)]
        [TestCategory(TestCategories.NonSmb)]
        [TestCategory(TestCategories.UnexpectedFields)]
        [Description("Query FileBasicInformation on directory file and check if creation time is valid")]
        public void FileInfo_Query_FileBasicInformation_Dir_CreationTime_MinusTwo()
        {
            TestCreationTimeMinusTwo(FileType.DirectoryFile);
        }

        [TestMethod()]
        [TestCategory(TestCategories.Fsa)]
        [TestCategory(TestCategories.QueryFileInformation)]
        [TestCategory(TestCategories.NonSmb)]
        [TestCategory(TestCategories.UnexpectedFields)]
        [Description("Query FileBasicInformation on directory file and check if creation time is valid")]
        public void FileInfo_Query_FileBasicInformation_Dir_CreationTime_Negative()
        {
            TestCreationTimeNegative(FileType.DirectoryFile);
        }

        #endregion

        #region Test Case Utility

        private void FileInfo_Query_FileBasicInformation_CreationTime(TestType testType, FileType fileType)
        {
            BaseTestSite.Log.Add(LogEntryKind.TestStep, "Test case steps:");

            //Step 1: Create File
            BaseTestSite.Log.Add(LogEntryKind.TestStep, "1. Create " + fileType.ToString() + " with FileAccess.FILE_WRITE_ATTRIBUTES");

            string fileName = this.fsaAdapter.ComposeRandomFileName(8);
            CreateFile(fileType, fileName);

            //Query FileBasicInformation 
            BaseTestSite.Log.Add(LogEntryKind.TestStep, "2. Query CreationTime before operation");
            long creationTimeBeforeIO;

            QueryFileBasicInformation(out _, out creationTimeBeforeIO, out _, out _);
            DelayNextStep();

            if(testType == TestType.Zero)
            {
                BaseTestSite.Log.Add(LogEntryKind.TestStep, "3. Set CreationTime to 0");
                SetCreationTime(0);
            }
            else
            {
                //Perform I/O operation
                BaseTestSite.Log.Add(LogEntryKind.TestStep, "3. Perform I/O operation");
                PerformIO(fileType, fileName);
            }

            //Query FileBasicInformation 
            BaseTestSite.Log.Add(LogEntryKind.TestStep, "4. Query CreationTime after operation");
            long creationTimeAfterIO;

            QueryFileBasicInformation(out _, out creationTimeAfterIO, out _, out _);

            //Verify CreationTime has not changed
            BaseTestSite.Assert.AreEqual(creationTimeBeforeIO, creationTimeAfterIO, "File creation time is never updated in response to file system calls such as read and write.");
        }

        private void PerformIO(FileType fileType, string fileName)
        {
            if (fileType == FileType.DataFile)
            {
                //Write to file
                WriteDataToFile(fileName);
            }
            else
            {
                //Create file in the directory
                string innerFileName = this.fsaAdapter.ComposeRandomFileName(8);

                CreateFile(FileType.DataFile, fileName + "\\" + innerFileName);

                this.fsaAdapter.CloseOpen();
                OpenFile(fileType, fileName);
            }
        }

        enum TestType
        {
            IO,
            Zero
        }

        private void TestCreationTimeMinusTwo(FileType fileType)
        {
            //Testing file system behavior to -2 timestamp value
            //[MS-FSCC] 6 Appendix B: Product Behavior <96>,<97>,<98>,<99>
            string operatingSystem = this.fsaAdapter.TestConfig.Platform.ToString();
            if ((this.fsaAdapter.FileSystem != FileSystem.NTFS
                   || Enum.IsDefined(typeof(OS_MinusTwo_NotSupported_NTFS), operatingSystem))
                && (this.fsaAdapter.FileSystem != FileSystem.REFS
                   || Enum.IsDefined(typeof(OS_MinusTwo_NotSupported_REFS), operatingSystem)))
            {
                this.TestSite.Assume.Inconclusive("Value -2 for FileBasicInformation timestamps is only supported by NTFS and ReFS. See FSCC 96> Section 2.4.7 for supported Operating System");
            }

            BaseTestSite.Log.Add(LogEntryKind.TestStep, "Test case steps:");

            //Step 1: Create File
            BaseTestSite.Log.Add(LogEntryKind.TestStep, "1. Create " + fileType.ToString() + " with FileAccess.FILE_WRITE_ATTRIBUTES");

            string fileName = this.fsaAdapter.ComposeRandomFileName(8);
            CreateFile(fileType, fileName);

            long initialCreationTime;

            BaseTestSite.Log.Add(LogEntryKind.TestStep, "2. Query FileBasicInformation for CreationTime ");

            QueryFileBasicInformation(out _, out initialCreationTime,
                out _, out _);
            DelayNextStep();

            BaseTestSite.Log.Add(LogEntryKind.TestStep, "3. Set CreationTime to -1");
            //SetFileInformation with FileInfoClass.FILE_BASIC_INFORMATION having timestamp equals -1
            SetCreationTime(-1);

            BaseTestSite.Log.Add(LogEntryKind.TestStep, "4. Perform I/O operation");
            //Write to file and verify file system response
            PerformIO(fileType, fileName);

            long creationTimeAfterIO;

            BaseTestSite.Log.Add(LogEntryKind.TestStep, "5. Query FileBasicInformation for current CreationTime ");
            QueryFileBasicInformation(out _, out creationTimeAfterIO, out _, out _);

            this.fsaAdapter.AssertAreEqual(this.Manager, initialCreationTime, creationTimeAfterIO,
                    "Creation time is never updated in response to file system calls such as read and write.");

            BaseTestSite.Log.Add(LogEntryKind.TestStep, "6. Set CreationTime to -2");
            //SetFileInformation with FileInfoClass.FILE_BASIC_INFORMATION having timestamp equals to -2
            SetCreationTime(-2);

            BaseTestSite.Log.Add(LogEntryKind.TestStep, "7. Perform I/O operation");
            //Write to file and verify file system response
            PerformIO(fileType, fileName);

            BaseTestSite.Log.Add(LogEntryKind.TestStep, "8. Query FileBasicInformation for current CreationTime ");
            QueryFileBasicInformation(out _, out creationTimeAfterIO, out _, out _);

            BaseTestSite.Assert.AreEqual(initialCreationTime, creationTimeAfterIO,
                "Creation time is never updated in response to file system calls such as read and write.");
        }

        private void TestCreationTimeNegative(FileType fileType)
        {
            BaseTestSite.Log.Add(LogEntryKind.TestStep, "Test case steps:");

            //Step 1: Create File
            BaseTestSite.Log.Add(LogEntryKind.TestStep, "1. Create " + fileType.ToString() + " with FileAccess.FILE_WRITE_ATTRIBUTES");

            string fileName = this.fsaAdapter.ComposeRandomFileName(8);
            CreateFile(fileType, fileName);

            //Set value less than -2
            BaseTestSite.Log.Add(LogEntryKind.TestStep, "1. Set value to -3");
            SetCreationTime(-3);
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

        private void WriteDataToFile(string fileName)
        {
            long byteSize = (uint)2 * 1024 * this.fsaAdapter.ClusterSizeInKB;
            MessageStatus status = this.fsaAdapter.WriteFile(0, byteSize, out _);

            this.fsaAdapter.AssertAreEqual(this.Manager, MessageStatus.SUCCESS, status,
                    "Write data to file should succeed");

            this.fsaAdapter.CloseOpen();
            OpenFile(FileType.DataFile, fileName);
        }

        private int GetDelaTimeByFileSystem(FileSystem fileSystem)
        {
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
