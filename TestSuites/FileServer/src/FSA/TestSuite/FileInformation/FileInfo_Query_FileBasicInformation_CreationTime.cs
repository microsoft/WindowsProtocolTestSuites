// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Protocols.TestSuites.FileSharing.Common.Adapter;
using Microsoft.Protocols.TestSuites.FileSharing.FSA.Adapter;
using Microsoft.Protocols.TestTools;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

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
            FileInfo_Query_FileBasicInformation_CreationTime(FileType.DataFile);
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
            FileInfo_Query_FileBasicInformation_CreationTime(FileType.DirectoryFile);
        }

        #endregion

        #region Test Case Utility

        private void FileInfo_Query_FileBasicInformation_CreationTime(FileType fileType)
        {
            BaseTestSite.Log.Add(LogEntryKind.TestStep, "Test case steps:");

            //Step 1: Create File
            BaseTestSite.Log.Add(LogEntryKind.TestStep, "1. Create " + fileType.ToString() + " with FileAccess.FILE_WRITE_ATTRIBUTES");

            string fileName = this.fsaAdapter.ComposeRandomFileName(8);
            CreateFile(fileType, fileName);

            //Query FileBasicInformation 
            long creationTimeBeforeIO;

            QueryFileBasicInformation(out _, out creationTimeBeforeIO, out _, out _);
            DelayNextStep();

            //Perform I/O operation

            if (fileType == FileType.DataFile)
            {
                //Step 3: Write to file
                BaseTestSite.Log.Add(LogEntryKind.TestStep, "3. Write to file");

                //Write to file
                WriteDataToFile();
            }
            else
            {
                //Step 3: Create file in the directory
                BaseTestSite.Log.Add(LogEntryKind.TestStep, "3. Create file in the directory");

                string innerFileName = this.fsaAdapter.ComposeRandomFileName(8);

                CreateFile(FileType.DataFile, fileName + "\\" + innerFileName);
            }

            //Query FileBasicInformation 
            long creationTimeAfterIO;

            QueryFileBasicInformation(out _, out creationTimeAfterIO, out _, out _);

            //Verify CreationTime has not changed
            BaseTestSite.Assert.AreEqual(creationTimeBeforeIO, creationTimeAfterIO, "File creation time is never updated in response to file system calls such as read and write.");
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
        private void WriteDataToFile()
        {
            long byteSize = (uint)2 * 1024 * this.fsaAdapter.ClusterSizeInKB;
            MessageStatus status = this.fsaAdapter.WriteFile(0, byteSize, out _);

            this.fsaAdapter.AssertAreEqual(this.Manager, MessageStatus.SUCCESS, status,
                    "Write data to file should succeed");
        }

        private void DelayNextStep()
        {
            //delay next step for recognizable milliseconds change
            DateTime currentTime = GetCurrentSystemTime();
            DateTime nextTime = GetCurrentSystemTime();

            while (currentTime.ToString().Equals(nextTime.ToString()))
            {
                nextTime = GetCurrentSystemTime();
            }
        }

        private DateTime GetCurrentSystemTime()
        {
            return DateTime.Now;
        }
        #endregion
    }

}
