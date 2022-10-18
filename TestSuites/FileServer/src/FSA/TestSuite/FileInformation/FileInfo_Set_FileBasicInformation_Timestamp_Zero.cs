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
        [TestCategory(TestCategories.Fsa)]
        [TestCategory(TestCategories.SetFileInformation)]
        [TestCategory(TestCategories.NonSmb)]
        [TestCategory(TestCategories.Positive)]
        [Description("Set file basic information on directory and check if file system supports 0 timestamp for ChangeTime")]
        public void FileInfo_Set_FileBasicInformation_Timestamp_Zero_Dir_ChangeTime()
        {
            if (this.fsaAdapter.FileSystem == FileSystem.REFS)
            {
                this.TestSite.Assume.Inconclusive("0 timestamp on ChangeTime is inconlusive for directory if file system is ReFS");
            }
            else
            {
                FileInfo_Set_FileBasicInformation_Timestamp_Zero(FileType.DirectoryFile, TimestampType.ChangeTime);
            }
        }

        [TestMethod()]
        [TestCategory(TestCategories.Fsa)]
        [TestCategory(TestCategories.SetFileInformation)]
        [TestCategory(TestCategories.NonSmb)]
        [TestCategory(TestCategories.Positive)]
        [Description("Set file basic information on data file and check if file system supports 0 timestamp for LastWriteTime")]
        public void FileInfo_Set_FileBasicInformation_Timestamp_Zero_File_LastWriteTime()
        {
            FileInfo_Set_FileBasicInformation_Timestamp_Zero(FileType.DataFile, TimestampType.LastWriteTime);
        }

        [TestMethod()]
        [TestCategory(TestCategories.Fsa)]
        [TestCategory(TestCategories.SetFileInformation)]
        [TestCategory(TestCategories.NonSmb)]
        [TestCategory(TestCategories.Positive)]
        [Description("Set file basic information on directory and check if file system supports 0 timestamp for LastWriteTime")]
        public void FileInfo_Set_FileBasicInformation_Timestamp_Zero_Dir_LastWriteTime()
        {
            FileInfo_Set_FileBasicInformation_Timestamp_Zero(FileType.DirectoryFile, TimestampType.LastWriteTime);
        }

        [TestMethod()]
        [TestCategory(TestCategories.Fsa)]
        [TestCategory(TestCategories.SetFileInformation)]
        [TestCategory(TestCategories.NonSmb)]
        [TestCategory(TestCategories.Positive)]
        [Description("Set file basic information on data file and check if file system supports 0 timestamp for LastAccessTime")]
        public void FileInfo_Set_FileBasicInformation_Timestamp_Zero_File_LastAccessTime()
        {
            FileInfo_Set_FileBasicInformation_Timestamp_Zero(FileType.DataFile, TimestampType.LastAccessTime);
        }

        [TestMethod()]
        [TestCategory(TestCategories.Fsa)]
        [TestCategory(TestCategories.SetFileInformation)]
        [TestCategory(TestCategories.NonSmb)]
        [TestCategory(TestCategories.Positive)]
        [Description("Set file basic information on directory and check if file system supports 0 timestamp for LastAccessTime")]
        public void FileInfo_Set_FileBasicInformation_Timestamp_Zero_Dir_LastAccessTime()
        {
            FileInfo_Set_FileBasicInformation_Timestamp_Zero(FileType.DirectoryFile, TimestampType.LastAccessTime);
        }

        [TestMethod()]
        [TestCategory(TestCategories.Fsa)]
        [TestCategory(TestCategories.SetFileInformation)]
        [TestCategory(TestCategories.NonSmb)]
        [TestCategory(TestCategories.Positive)]
        [Description("Set file basic information on data file and check if file system supports 0 timestamp for CreationTime")]
        public void FileInfo_Set_FileBasicInformation_Timestamp_Zero_File_CreationTime()
        {
            FileInfo_Set_FileBasicInformation_Timestamp_Zero(FileType.DataFile, TimestampType.CreationTime);
        }

        [TestMethod()]
        [TestCategory(TestCategories.Fsa)]
        [TestCategory(TestCategories.SetFileInformation)]
        [TestCategory(TestCategories.NonSmb)]
        [TestCategory(TestCategories.Positive)]
        [Description("Set file basic information on directory and check if file system supports 0 timestamp for CreationTime")]
        public void FileInfo_Set_FileBasicInformation_Timestamp_Zero_Dir_CreationTime()
        {
            FileInfo_Set_FileBasicInformation_Timestamp_Zero(FileType.DirectoryFile, TimestampType.CreationTime);
        }

        #endregion

        #region Test Case Utility
        private void FileInfo_Set_FileBasicInformation_Timestamp_Zero(FileType fileType, TimestampType timestampType)
        {
            BaseTestSite.Log.Add(LogEntryKind.TestStep, "Test case steps:");

            //Testing file system behavior to 0 timestamp value
            //[MS-FSCC] 6 Appendix B: Product Behavior <97>,<98>,<99>,<100>
            string operatingSystem = this.fsaAdapter.TestConfig.Platform.ToString();

            if (this.fsaAdapter.FileSystem != FileSystem.NTFS
                && this.fsaAdapter.FileSystem != FileSystem.REFS)
            {
                this.TestSite.Assume.Inconclusive("Value -2 for FileBasicInformation timestamps is only supported by NTFS and ReFS.");
            }
            else if ((!Enum.IsDefined(typeof(OS_MinusTwo_NotSupported_NTFS), operatingSystem) && this.fsaAdapter.FileSystem == FileSystem.NTFS)
                || (!Enum.IsDefined(typeof(OS_MinusTwo_NotSupported_REFS), operatingSystem) && this.fsaAdapter.FileSystem == FileSystem.REFS))
            {
                TestZeroTimestamp(fileType, timestampType);
            }
            else
            {
                this.TestSite.Assume.Inconclusive(operatingSystem + " does not support -2 timestamp for " + this.fsaAdapter.FileSystem);
            }
        }

        private void TestZeroTimestamp(FileType fileType, TimestampType timestampType)
        {
            //Step 1: Create file
            BaseTestSite.Log.Add(LogEntryKind.TestStep, "1. Create file and QueryFileInformation with FileInfoClass.FILE_BASIC_INFORMATION");

            //Create new file
            string fileName = this.fsaAdapter.ComposeRandomFileName(8);

            long creationTime;
            long changeTime;
            long lastAccessTime;
            long lastWriteTime;

            CreateFile(fileType, fileName);
            QueryFileBasicInformation(out changeTime, out creationTime, out lastAccessTime, out lastWriteTime);

            long timestampUnderTestOnCreation = timestampType switch
            {
                TimestampType.CreationTime => creationTime,
                TimestampType.ChangeTime => changeTime,
                TimestampType.LastAccessTime => lastAccessTime,
                TimestampType.LastWriteTime => lastWriteTime,
            };
            string underTestTimestampOnCreation = DateTime.FromFileTime(timestampUnderTestOnCreation).ToString();

            DelayNextStep();
            //Step 2: Set FileBasicInformation with 0 and verify system response
            BaseTestSite.Log.Add(LogEntryKind.TestStep, "2. SetFileInformation with FileInfoClass.FILE_BASIC_INFORMATION having values 0 and verify system response");

            //SetFileInformation with FileInfoClass.FILE_BASIC_INFORMATION having timestamp equals 0
            SetTimestampUnderTest(timestampType, 0);

            //Verify file system response
            QueryFileBasicInformation(out changeTime, out creationTime, out lastAccessTime, out lastWriteTime);
            long timestampUnderTest = timestampType switch
            {
                TimestampType.CreationTime => creationTime,
                TimestampType.ChangeTime => changeTime,
                TimestampType.LastAccessTime => lastAccessTime,
                TimestampType.LastWriteTime => lastWriteTime,
            };
            string underTestTimestamp = DateTime.FromFileTime(timestampUnderTest).ToString();

            this.fsaAdapter.AssertAreEqual(this.Manager, underTestTimestampOnCreation, underTestTimestamp,
                "If " + timestampType + " is 0, MUST NOT change " + timestampType + " attribute");
        }
        #endregion
    }
}