// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Protocols.TestSuites.FileSharing.Common.Adapter;
using Microsoft.Protocols.TestSuites.FileSharing.FSA.Adapter;
using Microsoft.Protocols.TestTools;
using Microsoft.Protocols.TestTools.StackSdk;
using Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Fscc;
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
        [TestCategory(TestCategories.UnexpectedFields)]
        [Description("Set file basic information on data file and check file system responds according to [MS-FSA] 2.1.5.14.2")]
        public void FileInfo_Set_FileBasicInformation_File_Negative()
        {
            FileInfo_Set_FileBasicInformation_Negative(FileType.DataFile);
        }

        [TestMethod()]
        [TestCategory(TestCategories.Bvt)]
        [TestCategory(TestCategories.Fsa)]
        [TestCategory(TestCategories.SetFileInformation)]
        [TestCategory(TestCategories.NonSmb)]
        [TestCategory(TestCategories.Positive)]
        [Description("Set file basic information on data file and check file system responds according to [MS-FSA] 2.1.5.14.2")]
        public void FileInfo_Set_FileBasicInformation_File_LastWriteTime()
        {
            FileInfo_Set_FileBasicInformation_Positive(FileType.DataFile, TimestampType.LastWriteTime);
        }

        [TestMethod()]
        [TestCategory(TestCategories.Bvt)]
        [TestCategory(TestCategories.Fsa)]
        [TestCategory(TestCategories.SetFileInformation)]
        [TestCategory(TestCategories.NonSmb)]
        [TestCategory(TestCategories.Positive)]
        [Description("Set file basic information on data file and check file system responds according to [MS-FSA] 2.1.5.14.2")]
        public void FileInfo_Set_FileBasicInformation_File_CreationTime()
        {
            FileInfo_Set_FileBasicInformation_Positive(FileType.DataFile, TimestampType.CreationTime);
        }

        [TestMethod()]
        [TestCategory(TestCategories.Bvt)]
        [TestCategory(TestCategories.Fsa)]
        [TestCategory(TestCategories.SetFileInformation)]
        [TestCategory(TestCategories.NonSmb)]
        [TestCategory(TestCategories.Positive)]
        [Description("Set file basic information on data file and check file system responds according to [MS-FSA] 2.1.5.14.2")]
        public void FileInfo_Set_FileBasicInformation_File_ChangeTime()
        {
            if (this.fsaAdapter.FileSystem != FileSystem.NTFS && this.fsaAdapter.FileSystem != FileSystem.REFS)
            {
                this.TestSite.Assume.Inconclusive("<153> Section 2.1.5.14.2: The FAT32 file system doesn’t process the ChangeTime field.");
            }
            else
            {
                FileInfo_Set_FileBasicInformation_Positive(FileType.DataFile, TimestampType.ChangeTime);
            }
        }

        [TestMethod()]
        [TestCategory(TestCategories.Bvt)]
        [TestCategory(TestCategories.Fsa)]
        [TestCategory(TestCategories.SetFileInformation)]
        [TestCategory(TestCategories.NonSmb)]
        [TestCategory(TestCategories.Positive)]
        [Description("Set file basic information on data file and check file system responds according to [MS-FSA] 2.1.5.14.2")]
        public void FileInfo_Set_FileBasicInformation_File_LastAccessTime()
        {
            if (this.fsaAdapter.FileSystem == FileSystem.FAT32)
            {
                this.TestSite.Assume.Inconclusive("The FAT32 file system is inconclusive for Open.File.LastAccessTime.");
            }
            else
            {
                FileInfo_Set_FileBasicInformation_Positive(FileType.DataFile, TimestampType.LastAccessTime);
            }
        }

        [TestMethod()]
        [TestCategory(TestCategories.Bvt)]
        [TestCategory(TestCategories.Fsa)]
        [TestCategory(TestCategories.SetFileInformation)]
        [TestCategory(TestCategories.NonSmb)]
        [TestCategory(TestCategories.UnexpectedFields)]
        [Description("Set file basic information on directory and check file system responds according to [MS-FSA] 2.1.5.14.2")]
        public void FileInfo_Set_FileBasicInformation_Dir_Negative()
        {
            FileInfo_Set_FileBasicInformation_Negative(FileType.DirectoryFile);
        }

        [TestMethod()]
        [TestCategory(TestCategories.Bvt)]
        [TestCategory(TestCategories.Fsa)]
        [TestCategory(TestCategories.SetFileInformation)]
        [TestCategory(TestCategories.NonSmb)]
        [TestCategory(TestCategories.Positive)]
        [Description("Set file basic information on directory and check file system responds according to [MS-FSA] 2.1.5.14.2")]
        public void FileInfo_Set_FileBasicInformation_Dir_LastWriteTime()
        {
            FileInfo_Set_FileBasicInformation_Positive(FileType.DirectoryFile, TimestampType.LastWriteTime);
        }

        [TestMethod()]
        [TestCategory(TestCategories.Bvt)]
        [TestCategory(TestCategories.Fsa)]
        [TestCategory(TestCategories.SetFileInformation)]
        [TestCategory(TestCategories.NonSmb)]
        [TestCategory(TestCategories.Positive)]
        [Description("Set file basic information on directory and check file system responds according to [MS-FSA] 2.1.5.14.2")]
        public void FileInfo_Set_FileBasicInformation_Dir_CreationTime()
        {
            FileInfo_Set_FileBasicInformation_Positive(FileType.DirectoryFile, TimestampType.CreationTime);
        }

        [TestMethod()]
        [TestCategory(TestCategories.Bvt)]
        [TestCategory(TestCategories.Fsa)]
        [TestCategory(TestCategories.SetFileInformation)]
        [TestCategory(TestCategories.NonSmb)]
        [TestCategory(TestCategories.Positive)]
        [Description("Set file basic information on directory and check file system responds according to [MS-FSA] 2.1.5.14.2")]
        public void FileInfo_Set_FileBasicInformation_Dir_ChangeTime()
        {
            if (this.fsaAdapter.FileSystem != FileSystem.NTFS && this.fsaAdapter.FileSystem != FileSystem.REFS)
            {
                this.TestSite.Assume.Inconclusive("<153> Section 2.1.5.14.2: The FAT32 file system doesn’t process the ChangeTime field.");
            }
            else
            {
                FileInfo_Set_FileBasicInformation_Positive(FileType.DirectoryFile, TimestampType.ChangeTime);
            }
        }

        [TestMethod()]
        [TestCategory(TestCategories.Bvt)]
        [TestCategory(TestCategories.Fsa)]
        [TestCategory(TestCategories.SetFileInformation)]
        [TestCategory(TestCategories.NonSmb)]
        [TestCategory(TestCategories.Positive)]
        [Description("Set file basic information on directory and check file system responds according to [MS-FSA] 2.1.5.14.2")]
        public void FileInfo_Set_FileBasicInformation_Dir_LastAccessTime()
        {
            if (this.fsaAdapter.FileSystem == FileSystem.FAT32)
            {
                this.TestSite.Assume.Inconclusive("The FAT32 file system is inconclusive for Open.File.LastAccessTime.");
            }
            else
            {
                FileInfo_Set_FileBasicInformation_Positive(FileType.DirectoryFile, TimestampType.LastAccessTime);
            }
        }

        #endregion

        #region Test Case Utility

        private void FileInfo_Set_FileBasicInformation_Negative(FileType fileType)
        {
            BaseTestSite.Log.Add(LogEntryKind.TestStep, "Test case steps:");

            //Step 1: Create File
            BaseTestSite.Log.Add(LogEntryKind.TestStep, "1. Create " + fileType.ToString() + " with FileAccess.FILE_WRITE_ATTRIBUTES");

            CreateFile(fileType);

            //Step 2: Set file basic information with invalid inputBuffer
            BaseTestSite.Log.Add(LogEntryKind.TestStep, "2. SetFileInformation with FileInfoClass.FILE_BASIC_INFORMATION having invalid inputBuffer and verify NTSTATUS");

            byte[] inputBuffer = new byte[1];
            MessageStatus status = this.fsaAdapter.SetFileInformation(FileInfoClass.FILE_BASIC_INFORMATION, inputBuffer);

            this.fsaAdapter.AssertAreEqual(this.Manager, MessageStatus.INFO_LENGTH_MISMATCH, status,
                    "If InputBufferSize is less than sizeof(FILE_BASIC_INFORMATION), the operation MUST be failed with STATUS_INFO_LENGTH_MISMATCH.");

            //Step 3: Set file basic information to invalid values
            BaseTestSite.Log.Add(LogEntryKind.TestStep, "3. SetFileInformation with FileInfoClass.FILE_BASIC_INFORMATION having invalid attribute values and verify NTSTATUS");

            FileBasicInformation fileBasicInformation = new FileBasicInformation();
            fileBasicInformation.FileAttributes = fileType == FileType.DataFile ? (uint)(FileAttribute.DIRECTORY | FileAttribute.NORMAL)
                : (uint)(FileAttribute.TEMPORARY | FileAttribute.NORMAL);

            TestFileAttributes(fileType, fileBasicInformation);
        }

        private void FileInfo_Set_FileBasicInformation_Positive(FileType fileType, TimestampType timestampType)
        {
            BaseTestSite.Log.Add(LogEntryKind.TestStep, "Test case steps:");

            //Step 1: SetFileInformation with FileInfoClass.FILE_BASIC_INFORMATION and verify that file timestamp is updated
            BaseTestSite.Log.Add(LogEntryKind.TestStep, "1. SetFileInformation with FileInfoClass.FILE_BASIC_INFORMATION having valid timestamp and verify that file timestamp is updated");

            TestSetTimestamp(fileType, timestampType);
        }

        private void TestSetTimestamp(FileType fileType, TimestampType timestampType)
        {
            //Create File
            CreateFile(fileType);

            //Set FileBasicInformation with tested timestamp equal to 01/05/2008 8:00:00
            //FSBO Section 6, FAT32 processess LastAccessTime in 1 day resolution (usually 8:00:00)
            DateTime date = new DateTime(2009, 4, 1, 8, 0, 0);
            long fileTime = date.ToFileTime();
            string inputDate = date.ToString();

            SetTimestampUnderTest(timestampType, fileTime);

            //Query FileBasicInformation 
            long creationTime;
            long changeTime;
            long lastAccessTime;
            long lastWriteTime;

            QueryFileBasicInformation(out changeTime, out creationTime, out lastAccessTime, out lastWriteTime);

            //Verify file timestamp was updated
            long timestampUnderTest = timestampType switch
            {
                TimestampType.CreationTime => creationTime,
                TimestampType.ChangeTime => changeTime,
                TimestampType.LastAccessTime => lastAccessTime,
                TimestampType.LastWriteTime => lastWriteTime,
            };
            string underTestTimestamp = DateTime.FromFileTime(timestampUnderTest).ToString();
            string openFileParameter = timestampType.Equals(TimestampType.ChangeTime) ? "LastChangeTime" : timestampType.ToString();

            this.fsaAdapter.AssertAreEqual(this.Manager, inputDate, underTestTimestamp,
                "The object store MUST set Open.File." + openFileParameter + " to InputBuffer." + timestampType + ".");
            
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

        private void SetTimestampUnderTest(TimestampType timestampType, long value)
        {
            switch (timestampType)
            {
                case TimestampType.CreationTime:
                    SetCreationTime(value);
                    break;
                case TimestampType.ChangeTime:
                    SetChangeTime(value);
                    break;
                case TimestampType.LastAccessTime:
                    SetLastAccessTime(value);
                    break;
                case TimestampType.LastWriteTime:
                    SetLastWriteTime(value);
                    break;
            }
        }

        private enum TimestampType
        {
            ChangeTime,
            CreationTime,
            LastAccessTime,
            LastWriteTime
        }

        private void SetChangeTime(long changeTime)
        {
            FileBasicInformation fileBasicInformation = new FileBasicInformation();
            fileBasicInformation.ChangeTime.dwHighDateTime = (uint)(changeTime >> 32);
            fileBasicInformation.ChangeTime.dwLowDateTime = (uint)(changeTime & 0xFFFFFFFF);
            fileBasicInformation.Reserved = 0;
            fileBasicInformation.FileAttributes = (uint)FileAttribute.NORMAL;
            byte[] inputBuffer = TypeMarshal.ToBytes(fileBasicInformation);
            MessageStatus status = this.fsaAdapter.SetFileInformation(FileInfoClass.FILE_BASIC_INFORMATION, inputBuffer);

            if(changeTime < -2)
            {
                this.fsaAdapter.AssertAreEqual(this.Manager, MessageStatus.INVALID_PARAMETER, status,
                        "If ChangeTime is less than -2, the operation MUST be failed with STATUS_INVALID_PARAMETER");
            }
            else
            {
                this.fsaAdapter.AssertAreEqual(this.Manager, MessageStatus.SUCCESS, status,
                        "Set file basic information should succeed");
            }
        }

        private void SetLastWriteTime(long lastWriteTime)
        {
            FileBasicInformation fileBasicInformation = new FileBasicInformation();
            fileBasicInformation.LastWriteTime.dwHighDateTime = (uint)(lastWriteTime >> 32);
            fileBasicInformation.LastWriteTime.dwLowDateTime = (uint)(lastWriteTime & 0xFFFFFFFF);
            fileBasicInformation.Reserved = 0;
            fileBasicInformation.FileAttributes = (uint)FileAttribute.NORMAL;
            byte[] inputBuffer = TypeMarshal.ToBytes(fileBasicInformation);
            MessageStatus status = this.fsaAdapter.SetFileInformation(FileInfoClass.FILE_BASIC_INFORMATION, inputBuffer);

            if (lastWriteTime < -2)
            {
                this.fsaAdapter.AssertAreEqual(this.Manager, MessageStatus.INVALID_PARAMETER, status,
                        "If LastWriteTime is less than -2, the operation MUST be failed with STATUS_INVALID_PARAMETER");
            }
            else
            {
                this.fsaAdapter.AssertAreEqual(this.Manager, MessageStatus.SUCCESS, status,
                        "Set file information should succeed");
            }
        }

        private void SetLastAccessTime(long lastAccessTime)
        {
            FileBasicInformation fileBasicInformation = new FileBasicInformation();
            fileBasicInformation.LastAccessTime.dwHighDateTime = (uint)(lastAccessTime >> 32);
            fileBasicInformation.LastAccessTime.dwLowDateTime = (uint)(lastAccessTime & 0xFFFFFFFF);
            fileBasicInformation.Reserved = 0;
            fileBasicInformation.FileAttributes = (uint)FileAttribute.NORMAL;
            byte[] inputBuffer = TypeMarshal.ToBytes(fileBasicInformation);
            MessageStatus status = this.fsaAdapter.SetFileInformation(FileInfoClass.FILE_BASIC_INFORMATION, inputBuffer);

            if (lastAccessTime < -2)
            {
                this.fsaAdapter.AssertAreEqual(this.Manager, MessageStatus.INVALID_PARAMETER, status,
                        "If LastAccessTime is less than -2, the operation MUST be failed with STATUS_INVALID_PARAMETER");
            }
            else
            {
                this.fsaAdapter.AssertAreEqual(this.Manager, MessageStatus.SUCCESS, status,
                        "Set file information should succeed");
            }
        }

        private void SetCreationTime(long creationTime)
        {
            FileBasicInformation fileBasicInformation = new FileBasicInformation();
            fileBasicInformation.CreationTime.dwHighDateTime = (uint)(creationTime >> 32);
            fileBasicInformation.CreationTime.dwLowDateTime = (uint)(creationTime & 0xFFFFFFFF);
            fileBasicInformation.Reserved = 0;
            fileBasicInformation.FileAttributes = (uint)FileAttribute.NORMAL;
            byte[] inputBuffer = TypeMarshal.ToBytes(fileBasicInformation);
            MessageStatus status = this.fsaAdapter.SetFileInformation(FileInfoClass.FILE_BASIC_INFORMATION, inputBuffer);

            if (creationTime < -2)
            {
                this.fsaAdapter.AssertAreEqual(this.Manager, MessageStatus.INVALID_PARAMETER, status,
                        "If CreationTime is less than -2, the operation MUST be failed with STATUS_INVALID_PARAMETER");
            }
            else
            {
                this.fsaAdapter.AssertAreEqual(this.Manager, MessageStatus.SUCCESS, status,
                        "Set file information should succeed");
            }
        }

        private void TestFileAttributes(FileType fileType, FileBasicInformation fileBasicInformation)
        {
            byte[] inputBuffer = TypeMarshal.ToBytes<FileBasicInformation>(fileBasicInformation);
            MessageStatus status = this.fsaAdapter.SetFileInformation(FileInfoClass.FILE_BASIC_INFORMATION, inputBuffer);
            bool isDirectory = (fileBasicInformation.FileAttributes & (uint)FileAttribute.DIRECTORY) == (uint)FileAttribute.DIRECTORY;
            bool isTemporary = (fileBasicInformation.FileAttributes & (uint)FileAttribute.TEMPORARY) == (uint)FileAttribute.TEMPORARY;
            
            if (isDirectory && fileType == FileType.DataFile)
            {
                this.fsaAdapter.AssertAreEqual(this.Manager, MessageStatus.INVALID_PARAMETER, status,
                    "If InputBuffer.FileAttributes.FILE_ATTRIBUTE_DIRECTORY is TRUE and Open.Stream.StreamType is DataStream., the operation MUST be failed with STATUS_INVALID_PARAMETER");
            }
            else if (isTemporary && fileType == FileType.DirectoryFile)
            {
                this.fsaAdapter.AssertAreEqual(this.Manager, MessageStatus.INVALID_PARAMETER, status,
                    "If InputBuffer.FileAttributes.FILE_ATTRIBUTE_TEMPORARY is TRUE and Open.File.FileType is DirectoryFile, the operation MUST be failed with STATUS_INVALID_PARAMETER");
            }
        }

        private void QueryFileBasicInformation(out long changeTime, out long creationTime
            , out long lastAccessTime, out long lastWriteTime)
        {
            FileBasicInformation fileBasicInformation = new FileBasicInformation();
            uint outputBufferSize = (uint)TypeMarshal.ToBytes(fileBasicInformation).Length;
            byte[] outputBuffer;
            this.fsaAdapter.QueryFileInformation(FileInfoClass.FILE_BASIC_INFORMATION, outputBufferSize, out _, out outputBuffer);

            fileBasicInformation = TypeMarshal.ToStruct<FileBasicInformation>(outputBuffer);
            changeTime = FiletimeToLong(fileBasicInformation.ChangeTime);
            creationTime = FiletimeToLong(fileBasicInformation.CreationTime);
            lastAccessTime = FiletimeToLong(fileBasicInformation.LastAccessTime);
            lastWriteTime = FiletimeToLong(fileBasicInformation.LastWriteTime);
        }

        private long FiletimeToLong(FILETIME time)
        {
            return ((((long)time.dwHighDateTime) << 32) | time.dwLowDateTime) << 0;
        }

        #endregion
    }

}
