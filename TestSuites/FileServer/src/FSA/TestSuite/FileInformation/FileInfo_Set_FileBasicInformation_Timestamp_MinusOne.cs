// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Protocols.TestSuites.FileSharing.Common.Adapter;
using Microsoft.Protocols.TestSuites.FileSharing.FSA.Adapter;
using Microsoft.Protocols.TestTools;
using Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Smb2;
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
        [Description("Set file basic information on data file and check if file system supports -1 timestamp for ChangeTime")]
        public void FileInfo_Set_FileBasicInformation_Timestamp_MinusOne_File_ChangeTime()
        {
            FileInfo_Set_FileBasicInformation_Timestamp_MinusOne(FileType.DataFile, TimestampType.ChangeTime);
        }

        [TestMethod()]
        [TestCategory(TestCategories.Fsa)]
        [TestCategory(TestCategories.SetFileInformation)]
        [TestCategory(TestCategories.NonSmb)]
        [TestCategory(TestCategories.Positive)]
        [Description("Set file basic information on directory and check if file system supports -1 timestamp for ChangeTime")]
        public void FileInfo_Set_FileBasicInformation_Timestamp_MinusOne_Dir_ChangeTime()
        {
            FileInfo_Set_FileBasicInformation_Timestamp_MinusOne(FileType.DirectoryFile, TimestampType.ChangeTime);
        }

        [TestMethod()]
        [TestCategory(TestCategories.Fsa)]
        [TestCategory(TestCategories.SetFileInformation)]
        [TestCategory(TestCategories.NonSmb)]
        [TestCategory(TestCategories.Positive)]
        [Description("Set file basic information on data file and check if file system supports -1 timestamp for LastWriteTime")]
        public void FileInfo_Set_FileBasicInformation_Timestamp_MinusOne_File_LastWriteTime()
        {
            FileInfo_Set_FileBasicInformation_Timestamp_MinusOne(FileType.DataFile, TimestampType.LastWriteTime);
        }

        [TestMethod()]
        [TestCategory(TestCategories.Fsa)]
        [TestCategory(TestCategories.SetFileInformation)]
        [TestCategory(TestCategories.NonSmb)]
        [TestCategory(TestCategories.Positive)]
        [Description("Set file basic information on directory and check if file system supports -1 timestamp for LastWriteTime")]
        public void FileInfo_Set_FileBasicInformation_Timestamp_MinusOne_Dir_LastWriteTime()
        {
            FileInfo_Set_FileBasicInformation_Timestamp_MinusOne(FileType.DirectoryFile, TimestampType.LastWriteTime);
        }

        [TestMethod()]
        [TestCategory(TestCategories.Fsa)]
        [TestCategory(TestCategories.SetFileInformation)]
        [TestCategory(TestCategories.NonSmb)]
        [TestCategory(TestCategories.Positive)]
        [Description("Set file basic information on data file and check if file system supports -1 timestamp for LastAccessTime")]
        public void FileInfo_Set_FileBasicInformation_Timestamp_MinusOne_File_LastAccessTime()
        {
            FileInfo_Set_FileBasicInformation_Timestamp_MinusOne(FileType.DataFile, TimestampType.LastAccessTime);
        }

        [TestMethod()]
        [TestCategory(TestCategories.Fsa)]
        [TestCategory(TestCategories.SetFileInformation)]
        [TestCategory(TestCategories.NonSmb)]
        [TestCategory(TestCategories.Positive)]
        [Description("Set file basic information on directory and check if file system supports -1 timestamp for LastAccessTime")]
        public void FileInfo_Set_FileBasicInformation_Timestamp_MinusOne_Dir_LastAccessTime()
        {
            FileInfo_Set_FileBasicInformation_Timestamp_MinusOne(FileType.DirectoryFile, TimestampType.LastAccessTime);
        }

        [TestMethod()]
        [TestCategory(TestCategories.Fsa)]
        [TestCategory(TestCategories.SetFileInformation)]
        [TestCategory(TestCategories.NonSmb)]
        [TestCategory(TestCategories.Positive)]
        [Description("Set file basic information on data file and check if file system supports -1 timestamp for CreationTime")]
        public void FileInfo_Set_FileBasicInformation_Timestamp_MinusOne_File_CreationTime()
        {
            FileInfo_Set_FileBasicInformation_Timestamp_MinusOne(FileType.DataFile, TimestampType.CreationTime);
        }

        [TestMethod()]
        [TestCategory(TestCategories.Fsa)]
        [TestCategory(TestCategories.SetFileInformation)]
        [TestCategory(TestCategories.NonSmb)]
        [TestCategory(TestCategories.Positive)]
        [Description("Set file basic information on directory and check if file system supports -1 timestamp for CreationTime")]
        public void FileInfo_Set_FileBasicInformation_Timestamp_MinusOne_Dir_CreationTime()
        {
            FileInfo_Set_FileBasicInformation_Timestamp_MinusOne(FileType.DirectoryFile, TimestampType.CreationTime);
        }

        #endregion

        #region Test Case Utility

        private void FileInfo_Set_FileBasicInformation_Timestamp_MinusOne(FileType fileType, TimestampType timestampType)
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
                TestMinusOneTimestamp(fileType, timestampType);
            }
            else
            {
                this.TestSite.Assume.Inconclusive(operatingSystem + " does not support -2 timestamp for " + this.fsaAdapter.FileSystem);
            }
        }

        private void TestMinusOneTimestamp(FileType fileType, TimestampType timestampType)
        {
            int testStep = 0;

            //Step 1: Create file
            BaseTestSite.Log.Add(LogEntryKind.TestStep, "{0}. Create file and QueryFileInformation with FileInfoClass.FILE_BASIC_INFORMATION", ++testStep);

            string fileName = this.fsaAdapter.ComposeRandomFileName(8);

            long creationTime;
            long changeTime;
            long lastAccessTime;
            long lastWriteTime;

            //Create new file
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
            string underTestTimestampOnCreationHolder = underTestTimestampOnCreation;

            DelayNextStep();

            if(fileType == FileType.DirectoryFile)
            {
                //Create an Alternate Data Stream in the newly created directory
                string streamName = this.fsaAdapter.ComposeRandomFileName(8);

                BaseTestSite.Log.Add(LogEntryKind.TestStep, "{0}. Create an Alternate Data Stream with name: " + streamName + " on this file.", ++testStep);
                CreateFile(FileType.DataFile, fileName + ":" + streamName + ":$DATA");
                QueryFileBasicInformation(out changeTime, out creationTime, out lastAccessTime, out lastWriteTime);

                timestampUnderTestOnCreation = timestampType switch
                {
                    TimestampType.CreationTime => creationTime,
                    TimestampType.ChangeTime => changeTime,
                    TimestampType.LastAccessTime => lastAccessTime,
                    TimestampType.LastWriteTime => lastWriteTime,
                };
                underTestTimestampOnCreation = DateTime.FromFileTime(timestampUnderTestOnCreation).ToString();

                if(timestampType == TimestampType.ChangeTime)
                {
                    underTestTimestampOnCreationHolder = underTestTimestampOnCreation;
                }
            }

            //Step 2: Set FileBasicInformation with -1 and verify system response
            BaseTestSite.Log.Add(LogEntryKind.TestStep, "{0}. SetFileInformation with FileInfoClass.FILE_BASIC_INFORMATION having values -1 and verify system response", ++testStep);

            //SetFileInformation with FileInfoClass.FILE_BASIC_INFORMATION having timestamp equals -1
            SetTimestampUnderTest(timestampType, -1);
            DelayNextStep();

            //Step 3: Write to file and verify file system response
            BaseTestSite.Log.Add(LogEntryKind.TestStep, "{0}. Write data to file as IO operation", ++testStep);
            if (fileType == FileType.DataFile)
            {
                WriteDataToFile(fileName);
            }
            else
            {
                PerformReadWrite();
            }

            QueryFileBasicInformation(out changeTime, out creationTime, out lastAccessTime, out lastWriteTime);

            long timestampUnderTest = timestampType switch
            {
                TimestampType.CreationTime => creationTime,
                TimestampType.ChangeTime => changeTime,
                TimestampType.LastAccessTime => lastAccessTime,
                TimestampType.LastWriteTime => lastWriteTime
            };
            string underTestTimestamp = DateTime.FromFileTime(timestampUnderTest).ToString();

            this.fsaAdapter.AssertAreEqual(this.Manager, underTestTimestampOnCreation, underTestTimestamp,
                    "If " + timestampType + " is -1, MUST NOT change " + timestampType + " attribute for all subsequent operations");

            if (fileType == FileType.DirectoryFile)
            {
                OpenFile(fileType, fileName);
                QueryFileBasicInformation(out changeTime, out creationTime,
                    out lastAccessTime, out lastWriteTime);
                timestampUnderTest = timestampType switch
                {
                    TimestampType.CreationTime => creationTime,
                    TimestampType.ChangeTime => changeTime,
                    TimestampType.LastAccessTime => lastAccessTime,
                    TimestampType.LastWriteTime => lastWriteTime,
                };
                underTestTimestamp = DateTime.FromFileTime(timestampUnderTest).ToString();
                this.fsaAdapter.AssertAreEqual(this.Manager, underTestTimestampOnCreationHolder, underTestTimestamp,
                        "Directory: If " + timestampType + " is -1, MUST NOT change " + timestampType + " attribute for all subsequent operations");
            }
        }

        private void PerformReadWrite()
        {
            long bytesToWrite = 4096;
            long bytesWritten = 0;
            MessageStatus status = WriteNewFile(bytesToWrite, out bytesWritten);
            this.fsaAdapter.AssertIfNotSuccess(status, "Write data to stream operation failed.");

            //Read some bytes from the Alternate Data Stream <Stream2> in the file
            int bytesToRead = (int)bytesWritten;
            status = this.fsaAdapter.ReadFile(0, bytesToRead, out _);
            this.fsaAdapter.AssertIfNotSuccess(status, "Read data from stream operation failed.");
        }
        #endregion
    }
}