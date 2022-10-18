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
        [TestCategory(TestCategories.Bvt)]
        [TestCategory(TestCategories.Fsa)]
        [TestCategory(TestCategories.SetFileInformation)]
        [TestCategory(TestCategories.NonSmb)]
        [TestCategory(TestCategories.Positive)]
        [Description("Set file basic information on data file and check if file system supports -2 timestamp for ChangeTime")]
        public void FileInfo_Set_FileBasicInformation_Timestamp_MinusTwo_File_ChangeTime()
        {
            if (this.fsaAdapter.FileSystem == FileSystem.REFS || this.fsaAdapter.TestConfig.Platform == Platform.WindowsServer2012R2)
            {
                this.TestSite.Assume.Inconclusive("-2 timestamp on ChangeTime is inconlusive if file system is ReFS or platform is Windows Server 2012 R2");
            }
            else
            {
                FileInfo_Set_FileBasicInformation_Timestamp_MinusTwo(FileType.DataFile, TimestampType.ChangeTime);
            }
        }

        [TestMethod()]
        [TestCategory(TestCategories.Bvt)]
        [TestCategory(TestCategories.Fsa)]
        [TestCategory(TestCategories.SetFileInformation)]
        [TestCategory(TestCategories.NonSmb)]
        [Description("Set file basic information on directory and check if file system supports -2 timestamp for ChangeTime")]
        public void FileInfo_Set_FileBasicInformation_Timestamp_MinusTwo_Dir_ChangeTime()
        {
            FileInfo_Set_FileBasicInformation_Timestamp_MinusTwo(FileType.DirectoryFile,TimestampType.ChangeTime);
        }

        [TestMethod()]
        [TestCategory(TestCategories.Bvt)]
        [TestCategory(TestCategories.Fsa)]
        [TestCategory(TestCategories.SetFileInformation)]
        [TestCategory(TestCategories.NonSmb)]
        [Description("Set file basic information on data file and check if file system supports -2 timestamp for LastWriteTime")]
        public void FileInfo_Set_FileBasicInformation_Timestamp_MinusTwo_File_LastWriteTime()
        {
            if (this.fsaAdapter.FileSystem == FileSystem.REFS || this.fsaAdapter.TestConfig.Platform == Platform.WindowsServer2012R2)
            {
                this.TestSite.Assume.Inconclusive("-2 timestamp on lastWriteTime is inconlusive if file system is ReFS or platform is Windows Server 2012 R2");
            }
            else
            {
                FileInfo_Set_FileBasicInformation_Timestamp_MinusTwo(FileType.DataFile, TimestampType.LastWriteTime);
            }
        }

        [TestMethod()]
        [TestCategory(TestCategories.Bvt)]
        [TestCategory(TestCategories.Fsa)]
        [TestCategory(TestCategories.SetFileInformation)]
        [TestCategory(TestCategories.NonSmb)]
        [Description("Set file basic information on directory and check if file system supports -2 timestamp for LastWriteTime")]
        public void FileInfo_Set_FileBasicInformation_Timestamp_MinusTwo_Dir_LastWriteTime()
        {
            FileInfo_Set_FileBasicInformation_Timestamp_MinusTwo(FileType.DirectoryFile,TimestampType.LastWriteTime);
        }

        [TestMethod()]
        [TestCategory(TestCategories.Bvt)]
        [TestCategory(TestCategories.Fsa)]
        [TestCategory(TestCategories.SetFileInformation)]
        [TestCategory(TestCategories.NonSmb)]
        [Description("Set file basic information on data file and check if file system supports -2 timestamp for LastAccessTime")]
        public void FileInfo_Set_FileBasicInformation_Timestamp_MinusTwo_File_LastAccessTime()
        {
            if (this.fsaAdapter.FileSystem == FileSystem.REFS || this.fsaAdapter.TestConfig.Platform == Platform.WindowsServer2012R2)
            {
                this.TestSite.Assume.Inconclusive("-2 timestamp on LastAccessTime is inconlusive if file system is ReFS or platform is Windows Server 2012 R2");
            }
            else
            {
                FileInfo_Set_FileBasicInformation_Timestamp_MinusTwo(FileType.DataFile, TimestampType.LastAccessTime);
            }
        }

        [TestMethod()]
        [TestCategory(TestCategories.Bvt)]
        [TestCategory(TestCategories.Fsa)]
        [TestCategory(TestCategories.SetFileInformation)]
        [TestCategory(TestCategories.NonSmb)]
        [Description("Set file basic information on data file and check if file system supports -2 timestamp for CreationTime")]
        public void FileInfo_Set_FileBasicInformation_Timestamp_MinusTwo_File_CreationTime()
        {
            FileInfo_Set_FileBasicInformation_Timestamp_MinusTwo(FileType.DataFile, TimestampType.CreationTime);
        }

        [TestMethod()]
        [TestCategory(TestCategories.Bvt)]
        [TestCategory(TestCategories.Fsa)]
        [TestCategory(TestCategories.SetFileInformation)]
        [TestCategory(TestCategories.NonSmb)]
        [Description("Set file basic information on directory and check if file system supports -2 timestamp for CreationTime")]
        public void FileInfo_Set_FileBasicInformation_Timestamp_MinusTwo_Dir_CreationTime()
        {
            FileInfo_Set_FileBasicInformation_Timestamp_MinusTwo(FileType.DirectoryFile, TimestampType.CreationTime);
        }

        [TestMethod()]
        [TestCategory(TestCategories.Fsa)]
        [TestCategory(TestCategories.SetFileInformation)]
        [TestCategory(TestCategories.NonSmb)]
        [TestCategory(TestCategories.UnexpectedFields)]
        [Description("Set file basic information on data file to value less than -2 for LastAccessTime")]
        public void FileInfo_Set_FileBasicInformation_Timestamp_Negative_File_LastAccessTime()
        {
            FileInfo_Set_FileBasicInformation_Timestamp_Negative(FileType.DataFile,TimestampType.LastAccessTime);
        }

        [TestMethod()]
        [TestCategory(TestCategories.Fsa)]
        [TestCategory(TestCategories.SetFileInformation)]
        [TestCategory(TestCategories.NonSmb)]
        [TestCategory(TestCategories.UnexpectedFields)]
        [Description("Set file basic information on directory to value less than -2 for LastAccessTime")]
        public void FileInfo_Set_FileBasicInformation_Timestamp_Negative_Dir_LastAccessTime()
        {
            FileInfo_Set_FileBasicInformation_Timestamp_Negative(FileType.DirectoryFile,TimestampType.LastAccessTime);
        }
        
        [TestMethod()]
        [TestCategory(TestCategories.Fsa)]
        [TestCategory(TestCategories.SetFileInformation)]
        [TestCategory(TestCategories.NonSmb)]
        [TestCategory(TestCategories.UnexpectedFields)]
        [Description("Set file basic information on data file to value less than -2 for LastWriteTime")]
        public void FileInfo_Set_FileBasicInformation_Timestamp_Negative_File_LastWriteTime()
        {
            FileInfo_Set_FileBasicInformation_Timestamp_Negative(FileType.DataFile,TimestampType.LastWriteTime);
        }

        [TestMethod()]
        [TestCategory(TestCategories.Fsa)]
        [TestCategory(TestCategories.SetFileInformation)]
        [TestCategory(TestCategories.NonSmb)]
        [TestCategory(TestCategories.UnexpectedFields)]
        [Description("Set file basic information on directory to value less than -2 for LastWriteTime")]
        public void FileInfo_Set_FileBasicInformation_Timestamp_Negative_Dir_LastWriteTime()
        {
            FileInfo_Set_FileBasicInformation_Timestamp_Negative(FileType.DirectoryFile,TimestampType.LastWriteTime);
        }
        
        [TestMethod()]
        [TestCategory(TestCategories.Fsa)]
        [TestCategory(TestCategories.SetFileInformation)]
        [TestCategory(TestCategories.NonSmb)]
        [TestCategory(TestCategories.UnexpectedFields)]
        [Description("Set file basic information on data file to value less than -2 for ChangeTime")]
        public void FileInfo_Set_FileBasicInformation_Timestamp_Negative_File_ChangeTime()
        {
            FileInfo_Set_FileBasicInformation_Timestamp_Negative(FileType.DataFile,TimestampType.ChangeTime);
        }

        [TestMethod()]
        [TestCategory(TestCategories.Fsa)]
        [TestCategory(TestCategories.SetFileInformation)]
        [TestCategory(TestCategories.NonSmb)]
        [TestCategory(TestCategories.UnexpectedFields)]
        [Description("Set file basic information on directory to value less than -2 for ChangeTime")]
        public void FileInfo_Set_FileBasicInformation_Timestamp_Negative_Dir_ChangeTime()
        {
            FileInfo_Set_FileBasicInformation_Timestamp_Negative(FileType.DirectoryFile,TimestampType.ChangeTime);
        }
        
        [TestMethod()]
        [TestCategory(TestCategories.Fsa)]
        [TestCategory(TestCategories.SetFileInformation)]
        [TestCategory(TestCategories.NonSmb)]
        [TestCategory(TestCategories.UnexpectedFields)]
        [Description("Set file basic information on data file to value less than -2 for CreationTime")]
        public void FileInfo_Set_FileBasicInformation_Timestamp_Negative_File_CreationTime()
        {
            FileInfo_Set_FileBasicInformation_Timestamp_Negative(FileType.DataFile,TimestampType.CreationTime);
        }

        [TestMethod()]
        [TestCategory(TestCategories.Fsa)]
        [TestCategory(TestCategories.SetFileInformation)]
        [TestCategory(TestCategories.NonSmb)]
        [TestCategory(TestCategories.UnexpectedFields)]
        [Description("Set file basic information on directory to value less than -2 for CreationTime")]
        public void FileInfo_Set_FileBasicInformation_Timestamp_Negative_Dir_CreationTime()
        {
            FileInfo_Set_FileBasicInformation_Timestamp_Negative(FileType.DirectoryFile,TimestampType.CreationTime);
        }
        #endregion

        #region Test Case Utility

        private void FileInfo_Set_FileBasicInformation_Timestamp_Negative(FileType fileType, TimestampType timestampType)
        {
            BaseTestSite.Log.Add(LogEntryKind.TestStep, "Test case steps:");

            //Step 1: Create File
            BaseTestSite.Log.Add(LogEntryKind.TestStep, "1. Create " + fileType.ToString() + " with FileAccess.FILE_WRITE_ATTRIBUTES");

            CreateFile(fileType);

            //Step 2: Set file basic information to timestamp values less than -2
            BaseTestSite.Log.Add(LogEntryKind.TestStep, "2. SetFileInformation with FileInfoClass.FILE_BASIC_INFORMATION having timestamp less than -2 and verify NTSTATUS");

            //Testing file system behavior to -2 timestamp value
            //[MS-FSCC] 6 Appendix B: Product Behavior <97>,<98>,<99>,<100>
            string operatingSystem = this.fsaAdapter.TestConfig.Platform.ToString();

            if (this.fsaAdapter.FileSystem != FileSystem.NTFS
                && this.fsaAdapter.FileSystem != FileSystem.REFS)
            {
                this.TestSite.Assume.Inconclusive("Value -2 for FileBasicInformation timestamps is only supported by NTFS and ReFS.");
            }
            else if (timestampType == TimestampType.ChangeTime &&
                (!Enum.IsDefined(typeof(OS_MinusTwo_NotSupported_NTFS), operatingSystem) && this.fsaAdapter.FileSystem == FileSystem.NTFS)
                || (!Enum.IsDefined(typeof(OS_MinusTwo_NotSupported_REFS), operatingSystem) && this.fsaAdapter.FileSystem == FileSystem.REFS))
            {
                SetChangeTime(-3);
            }
            else if (timestampType == TimestampType.LastAccessTime &&
                (!Enum.IsDefined(typeof(OS_MinusTwo_NotSupported_NTFS), operatingSystem) && this.fsaAdapter.FileSystem == FileSystem.NTFS)
                || (!Enum.IsDefined(typeof(OS_MinusTwo_NotSupported_REFS), operatingSystem) && this.fsaAdapter.FileSystem == FileSystem.REFS))
            {
                SetLastAccessTime(-3);
            }
            else if (timestampType == TimestampType.LastWriteTime &&
                (!Enum.IsDefined(typeof(OS_MinusTwo_NotSupported_NTFS), operatingSystem) && this.fsaAdapter.FileSystem == FileSystem.NTFS)
                || (!Enum.IsDefined(typeof(OS_MinusTwo_NotSupported_REFS), operatingSystem) && this.fsaAdapter.FileSystem == FileSystem.REFS))
            {
                SetLastWriteTime(-3);
            }
            else if (timestampType == TimestampType.CreationTime &&
                (!Enum.IsDefined(typeof(OS_MinusTwo_NotSupported_NTFS), operatingSystem) && this.fsaAdapter.FileSystem == FileSystem.NTFS)
                || (!Enum.IsDefined(typeof(OS_MinusTwo_NotSupported_REFS), operatingSystem) && this.fsaAdapter.FileSystem == FileSystem.REFS))
            {
                SetCreationTime(-3);
            }
            else
            {
                this.TestSite.Assume.Inconclusive(operatingSystem + " does not support -2 timestamp for " + this.fsaAdapter.FileSystem);
            }
        }

        private void FileInfo_Set_FileBasicInformation_Timestamp_MinusTwo(FileType fileType, TimestampType timestampType)
        {
            BaseTestSite.Log.Add(LogEntryKind.TestStep, "Test case steps:");

            //Testing file system behavior to -2 timestamp value
            //[MS-FSCC] 6 Appendix B: Product Behavior <96>,<97>,<98>,<99>
            string operatingSystem = this.fsaAdapter.TestConfig.Platform.ToString();
            
            if (this.fsaAdapter.FileSystem != FileSystem.NTFS
                && this.fsaAdapter.FileSystem != FileSystem.REFS)
            {
                this.TestSite.Assume.Inconclusive("Value -2 for FileBasicInformation timestamps is only supported by NTFS and ReFS.");
            }
            else if ((!Enum.IsDefined(typeof(OS_MinusTwo_NotSupported_NTFS), operatingSystem) && this.fsaAdapter.FileSystem == FileSystem.NTFS)
                || (!Enum.IsDefined(typeof(OS_MinusTwo_NotSupported_REFS), operatingSystem) && this.fsaAdapter.FileSystem == FileSystem.REFS))
            {
                TestMinusTwoTimestamp(fileType, timestampType);
            }
            else
            {
                this.TestSite.Assume.Inconclusive(operatingSystem + " does not support -2 timestamp for "+ this.fsaAdapter.FileSystem);
            }
        }

        private void TestMinusTwoTimestamp(FileType fileType, TimestampType timestampType)
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

            if (fileType == FileType.DirectoryFile)
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

                if (timestampType == TimestampType.ChangeTime)
                {
                    underTestTimestampOnCreationHolder = underTestTimestampOnCreation;
                }
            }

            //Step 2: Set FileBasicInformation with -1 and verify system response
            BaseTestSite.Log.Add(LogEntryKind.TestStep, "{0}. SetFileInformation with FileInfoClass.FILE_BASIC_INFORMATION having values -1 and verify system response", ++testStep);

            //SetFileInformation with FileInfoClass.FILE_BASIC_INFORMATION having timestamp equals -1
            SetTimestampUnderTest(timestampType, -1);

            DelayNextStep();

            //Write to file and verify file system response
            BaseTestSite.Log.Add(LogEntryKind.TestStep, "Write data to file as IO operation");
            if (fileType == FileType.DataFile)
            {
                WriteToFile(fileName);
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

            //Step 3: Set FileBasicInformation with -2 and verify system response
            BaseTestSite.Log.Add(LogEntryKind.TestStep, "{0}. SetFileInformation with FileInfoClass.FILE_BASIC_INFORMATION having values -2 and verify system response", ++testStep);

            //SetFileInformation with FileInfoClass.FILE_BASIC_INFORMATION having timestamp equals to -2
            SetTimestampUnderTest(timestampType, -2);
            DelayNextStep();

            BaseTestSite.Log.Add(LogEntryKind.TestStep, "{0}. Write data to file", ++testStep);
            //Write to file and verify file system response
            if (fileType == FileType.DataFile)
            {
                WriteToFile(fileName);
            }
            else
            {
                PerformReadWrite();
            }

            QueryFileBasicInformation(out changeTime, out creationTime, out lastAccessTime, out lastWriteTime);
            timestampUnderTest = timestampType switch
            {
                TimestampType.CreationTime => creationTime,
                TimestampType.ChangeTime => changeTime,
                TimestampType.LastAccessTime => lastAccessTime,
                TimestampType.LastWriteTime => lastWriteTime,
            };
            underTestTimestamp = DateTime.FromFileTime(timestampUnderTest).ToString();

            if (timestampType == TimestampType.CreationTime)
            {
                this.fsaAdapter.AssertAreEqual(this.Manager, underTestTimestampOnCreation, underTestTimestamp,
                    "Even though -1 and -2 can be used with the CreationTime field, they have no effect"
                    + " because file creation time is never updated in response to file system calls such as read and write");
            }
            else
            {
                BaseTestSite.Assert.AreNotEqual(underTestTimestampOnCreation, underTestTimestamp,
                    "If " + timestampType + " is -2, MUST change " + timestampType + " attribute for all subsequent operations");
            }            

            //Verify timestamp for directory updated
            if (fileType == FileType.DirectoryFile && timestampType != TimestampType.CreationTime)
            {
                OpenFile(fileType, fileName);
                QueryFileBasicInformation(out changeTime, out creationTime, out lastAccessTime, out lastWriteTime);
                timestampUnderTest = timestampType switch
                {
                    TimestampType.CreationTime => creationTime,
                    TimestampType.ChangeTime => changeTime,
                    TimestampType.LastAccessTime => lastAccessTime,
                    TimestampType.LastWriteTime => lastWriteTime,
                };
                underTestTimestamp = DateTime.FromFileTime(timestampUnderTest).ToString();

                BaseTestSite.Assert.AreNotEqual(underTestTimestampOnCreationHolder, underTestTimestamp,
                    "Directory: If " + timestampType + " is -2, MUST change " + timestampType + " attribute for all subsequent operations");
            }

        }

        private MessageStatus WriteNewFile(long byteCount, out long bytesWritten)
        {
            var status = this.fsaAdapter.SetFileInformation(FileInfoClass.FILE_ENDOFFILE_INFORMATION, BitConverter.GetBytes(byteCount));
            this.fsaAdapter.AssertIfNotSuccess(status, "Set new EOF of the file operation failed.");

            status = this.fsaAdapter.WriteFile(0, byteCount, out bytesWritten);
            this.fsaAdapter.AssertIfNotSuccess(status, "Actually write data to file operation failed.");

            return status;
        }

        private void WriteToFile(string fileName)
        {
            long byteSize = (uint)2 * 1024 * this.fsaAdapter.ClusterSizeInKB;
            MessageStatus status = this.fsaAdapter.WriteFile(0, byteSize, out _);

            this.fsaAdapter.AssertAreEqual(this.Manager, MessageStatus.SUCCESS, status,
                    "Write data to file should succeed");
        }
        #endregion
    }
}