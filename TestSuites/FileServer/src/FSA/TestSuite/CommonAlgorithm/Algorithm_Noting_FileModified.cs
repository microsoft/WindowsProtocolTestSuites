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
        [TestCategory(TestCategories.SetFileInformation)]
        [TestCategory(TestCategories.NonSmb)]
        [TestCategory(TestCategories.Positive)]
        [Description("Try to set ")]
        public void Algorithm_Noting_FileModified_Positive()
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
            
            CreateFile(fileType);

            //Query FileBasicInformation 
            long oldCreationTime;
            long oldChangeTime;
            long oldLastAccessTime;
            long oldLastWriteTime;

            QueryFileBasicInformation(out _, out oldChangeTime, out oldLastAccessTime, out oldLastWriteTime, out _);

            //Write to file
            WriteToFile();

            //Query FileBasicInformation
            long creationTime;
            long changeTime;
            long lastAccessTime;
            long lastWriteTime;
            bool isArchive;

            QueryFileBasicInformation(out changeTime, out creationTime, out lastAccessTime, out lastWriteTime, out isArchive);

            //Verify timestamp updated appropriately
            string oldChangeTimeDate = DateTime.FromFileTime(oldChangeTime).ToString();
            string changeTimeDate = DateTime.FromFileTime(changeTime).ToString();

            BaseTestSite.Assert.AreNotEqual(oldChangeTimeDate, changeTimeDate,
                "The object store SHOULD set Open.File.LastChangeTime to the current system time.");

            string oldLastWriteTimeDate = DateTime.FromFileTime(oldLastWriteTime).ToString();
            string lastWriteTimeDate = DateTime.FromFileTime(lastWriteTime).ToString();

            BaseTestSite.Assert.AreNotEqual(oldLastWriteTimeDate, lastWriteTimeDate,
                "The object store SHOULD set Open.File.LastModificationTime to the current system time.");

            string oldLastAccessTimeDate = DateTime.FromFileTime(oldLastAccessTime).ToString();
            string lastAccessTimeDate = DateTime.FromFileTime(lastAccessTime).ToString();

            BaseTestSite.Assert.AreNotEqual(oldLastAccessTimeDate, lastAccessTimeDate,
                "The object store SHOULD set Open.File.LastAccessTime to the current system time.");

            //Verify FILE_ATTRIBUTE_ARCHIVE is set to TRUE
            BaseTestSite.Assert.IsTrue(isArchive,
                "The object store SHOULD set Open.File.LastAccessTime to the current system time.");
            
        }

        private void CreateFile(FileType fileType)
        {
            string fileName = this.fsaAdapter.ComposeRandomFileName(8);
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
