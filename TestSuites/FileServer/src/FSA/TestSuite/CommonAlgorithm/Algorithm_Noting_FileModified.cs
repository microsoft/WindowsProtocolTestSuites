// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Protocols.TestSuites.FileSharing.FSA.Adapter;
using Microsoft.Protocols.TestSuites.FileSharing.Common.Adapter;
using Microsoft.Protocols.TestTools;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Fscc;
using Microsoft.Protocols.TestTools.StackSdk;
using System;

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
        public void CommonAlgorithm_Noting_FileModified()
        {
            Algorithm_Noting_FileModified(FileType.DataFile);
        }

        #endregion

        #region Test Case Utility

        private void Algorithm_Noting_FileModified(FileType fileType)
        {
            BaseTestSite.Log.Add(LogEntryKind.TestStep, "Test case steps:");

            //Step 1: Create file
            BaseTestSite.Log.Add(LogEntryKind.TestStep, "1. Create " + fileType.ToString());

            string fileName;
            CreateFile(fileType, out fileName);

            //Step 2: Query file basic information
            BaseTestSite.Log.Add(LogEntryKind.TestStep, "2. Query FILE_BASIC_INFORMATION for timestamps");

            long oldCreationTime;
            long oldChangeTime;
            long oldLastAccessTime;
            long oldLastWriteTime;

            QueryFileBasicInformation(out oldChangeTime, out oldCreationTime, out oldLastAccessTime, out oldLastWriteTime, out _);

            //Step 3: Write to file
            BaseTestSite.Log.Add(LogEntryKind.TestStep, "3. Write to file");

            //Write to file
            WriteToFile();

            //Step 4: Close and open file
            BaseTestSite.Log.Add(LogEntryKind.TestStep, "4. Close and open file to give significant time a file system can defer processing Section 2.1.4.17");

            //<42> Section 2.1.4.17: File systems may choose to defer processing for a file that has been modified 
            // to a later time, favoring performance over accuracy.
            this.fsaAdapter.CloseOpen();
            OpenFile(fileName);

            //Step 5: Query FileBasicInformation
            BaseTestSite.Log.Add(LogEntryKind.TestStep, "5. Query FILE_BASIC_INFORMATION for file attributes");

            long creationTime;
            long changeTime;
            long lastAccessTime;
            long lastWriteTime;
            bool isArchive;

            QueryFileBasicInformation(out changeTime, out creationTime, out lastAccessTime, out lastWriteTime, out isArchive);

            //Step 6: Verify that FileBasicInformation attributes are updated appropriately
            BaseTestSite.Log.Add(LogEntryKind.TestStep, "6. Verify that FILE_BASIC_INFORMATION attributes are updated appropriately");

            string oldCreationTimeDate = DateTime.FromFileTime(oldCreationTime).ToString();
            string creationTimeDate = DateTime.FromFileTime(creationTime).ToString();

            BaseTestSite.Assert.AreEqual(oldCreationTimeDate, creationTimeDate,
                "File creation time is never updated in response to file system calls such as read and write.");

            DateTime oldLastWriteTimeDate = DateTime.FromFileTime(oldLastWriteTime);
            DateTime lastWriteTimeDate = DateTime.FromFileTime(lastWriteTime);

            DateTime oldChangeTimeDate = DateTime.FromFileTime(oldChangeTime);
            DateTime changeTimeDate = DateTime.FromFileTime(changeTime);

            DateTime oldLastAccessTimeDate = DateTime.FromFileTime(oldLastAccessTime);
            DateTime lastAccessTimeDate = DateTime.FromFileTime(lastAccessTime);

            if (oldLastWriteTimeDate.CompareTo(lastWriteTimeDate) >= 0)
            {
                BaseTestSite.Assert.Inconclusive("The object store SHOULD set Open.File.LastModificationTime to the current system time.");
            }
            else if (oldLastAccessTimeDate.CompareTo(lastAccessTimeDate) >= 0)
            {
                BaseTestSite.Assert.Pass("The object store SHOULD set Open.File.LastModificationTime to the current system time.");
                BaseTestSite.Assert.Inconclusive("The object store SHOULD set Open.File.LastAccessTime to the current system time.");
            }
            else if (oldChangeTimeDate.CompareTo(changeTimeDate) >= 0)
            {
                BaseTestSite.Assert.Pass("The object store SHOULD set Open.File.LastModificationTime to the current system time.");
                BaseTestSite.Assert.Pass("The object store SHOULD set Open.File.LastAccessTime to the current system time.");
                BaseTestSite.Assert.Inconclusive("The object store SHOULD set Open.File.LastChangeTime to the current system time.");
            }
            else if (isArchive == false)
            {
                BaseTestSite.Assert.Pass("The object store SHOULD set Open.File.LastModificationTime to the current system time.");
                BaseTestSite.Assert.Pass("The object store SHOULD set Open.File.LastAccessTime to the current system time.");
                BaseTestSite.Assert.Pass("The object store SHOULD set Open.File.LastChangeTime to the current system time.");

                //Verify FILE_ATTRIBUTE_ARCHIVE is set to TRUE
                BaseTestSite.Assert.Inconclusive("The object store SHOULD set Open.File.FileAttributes.FILE_ATTRIBUTE_ARCHIVE to TRUE.");
            }
            else
            {
                BaseTestSite.Assert.Pass("The object store SHOULD set Open.File.LastModificationTime to the current system time.");
                BaseTestSite.Assert.Pass("The object store SHOULD set Open.File.LastChangeTime to the current system time.");
                BaseTestSite.Assert.Pass("The object store SHOULD set Open.File.LastAccessTime to the current system time.");
                BaseTestSite.Assert.Pass("The object store SHOULD set Open.File.FileAttributes.FILE_ATTRIBUTE_ARCHIVE to TRUE.");
            }
        }

        private void OpenFile(string fileName)
        {
            //open file after a time interval
            DateTime currentTime = DateTime.Now;
            DateTime nextTime = DateTime.Now;

            while (currentTime.ToString().Equals(nextTime.ToString()))
            {
                nextTime = DateTime.Now;
            }

            MessageStatus status = this.fsaAdapter.CreateFile(
                fileName,
                FileAttribute.NORMAL,
                CreateOptions.NON_DIRECTORY_FILE,
                FileAccess.GENERIC_READ | FileAccess.GENERIC_WRITE | FileAccess.FILE_WRITE_DATA | FileAccess.FILE_WRITE_ATTRIBUTES,
                ShareAccess.FILE_SHARE_READ | ShareAccess.FILE_SHARE_WRITE,
                CreateDisposition.OPEN);

            BaseTestSite.Assert.AreEqual(MessageStatus.SUCCESS, status, "Open should succeed.");
        }

        private void CreateFile(FileType fileType, out string fileName)
        {
            fileName = this.fsaAdapter.ComposeRandomFileName(8);
            MessageStatus status = this.fsaAdapter.CreateFile(
                fileName,
                FileAttribute.NORMAL,
                fileType == FileType.DataFile ? CreateOptions.NON_DIRECTORY_FILE : CreateOptions.DIRECTORY_FILE,
                FileAccess.GENERIC_READ | FileAccess.GENERIC_WRITE | FileAccess.FILE_WRITE_DATA | FileAccess.FILE_WRITE_ATTRIBUTES,
                ShareAccess.FILE_SHARE_READ | ShareAccess.FILE_SHARE_WRITE,
                CreateDisposition.CREATE);

            BaseTestSite.Assert.AreEqual(MessageStatus.SUCCESS, status, "Create should succeed.");
        }

        private void QueryFileBasicInformation(out long changeTime, out long creationTime
            , out long lastAccessTime, out long lastWriteTime, out bool isArchive)
        {
            FileBasicInformation fileBasicInformation = new FileBasicInformation();
            uint outputBufferSize = (uint)TypeMarshal.ToBytes<FileBasicInformation>(fileBasicInformation).Length;
            byte[] outputBuffer;
            this.fsaAdapter.QueryFileInformation(FileInfoClass.FILE_BASIC_INFORMATION, outputBufferSize, out _, out outputBuffer);

            fileBasicInformation = TypeMarshal.ToStruct<FileBasicInformation>(outputBuffer);
            changeTime = (((long)fileBasicInformation.ChangeTime.dwHighDateTime) << 32) + fileBasicInformation.ChangeTime.dwLowDateTime;
            creationTime = (((long)fileBasicInformation.CreationTime.dwHighDateTime) << 32) + fileBasicInformation.CreationTime.dwLowDateTime;
            lastAccessTime = (((long)fileBasicInformation.LastAccessTime.dwHighDateTime) << 32) + fileBasicInformation.LastAccessTime.dwLowDateTime;
            lastWriteTime = (((long)fileBasicInformation.LastWriteTime.dwHighDateTime) << 32) + fileBasicInformation.LastWriteTime.dwLowDateTime;
            isArchive = (fileBasicInformation.FileAttributes & (uint)FileAttribute.ARCHIVE) == (uint)FileAttribute.ARCHIVE;
        }

        private void WriteToFile()
        {
            //write data to file after a time interval
            DateTime currentTime = DateTime.Now;
            DateTime nextTime = DateTime.Now;

            while (currentTime.ToString().Equals(nextTime.ToString()))
            {
                nextTime = DateTime.Now;
            }

            long byteSize = (uint)2 * 1024 * this.fsaAdapter.ClusterSizeInKB;
            MessageStatus status = this.fsaAdapter.WriteFile(0, byteSize, out _);

            this.fsaAdapter.AssertAreEqual(this.Manager, MessageStatus.SUCCESS, status,
                    "Write data to file should succeed");
        }

        #endregion
    }
}
