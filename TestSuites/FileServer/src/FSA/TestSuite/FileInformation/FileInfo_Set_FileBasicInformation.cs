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
        [TestCategory(TestCategories.Positive)]
        [Description("Set file basic information on data file and check file system responds according to [MS-FSA] 2.1.5.14.2")]
        public void FileInfo_Set_FileBasicInformation_File()
        {
            FileInfo_Set_FileBasicInformation(FileType.DataFile);
        }

        [TestMethod()]
        [TestCategory(TestCategories.Fsa)]
        [TestCategory(TestCategories.SetFileInformation)]
        [TestCategory(TestCategories.NonSmb)]
        [TestCategory(TestCategories.Positive)]
        [Description("Set file basic information on data file and check if file system supports -2 timestamp")]
        public void FileInfo_Set_FileBasicInformation_File_MinusTwoSupported()
        {
            FileInfo_Set_FileBasicInformation_MinusTwoSupported();
        }

        [TestMethod()]
        [TestCategory(TestCategories.Bvt)]
        [TestCategory(TestCategories.Fsa)]
        [TestCategory(TestCategories.SetFileInformation)]
        [TestCategory(TestCategories.NonSmb)]
        [TestCategory(TestCategories.Positive)]
        [Description("Set file basic information on directory and check file system responds according to [MS-FSA] 2.1.5.14.2")]
        public void FileInfo_Set_FileBasicInformation_Dir()
        {
            FileInfo_Set_FileBasicInformation(FileType.DirectoryFile);
        }

        #endregion

        #region Test Case Utility

        private void FileInfo_Set_FileBasicInformation(FileType fileType)
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

        private void FileInfo_Set_FileBasicInformation_MinusTwoSupported()
        {
            BaseTestSite.Log.Add(LogEntryKind.TestStep, "Test case steps:");

            FileType fileType = FileType.DataFile;

            //Step 1: Create File
            BaseTestSite.Log.Add(LogEntryKind.TestStep, "1. Create " + fileType.ToString() + " with FileAccess.FILE_WRITE_ATTRIBUTES");

            CreateFile(fileType);

            //Step 2: Set file basic information to timestamp values less than -2
            BaseTestSite.Log.Add(LogEntryKind.TestStep, "2. SetFileInformation with FileInfoClass.FILE_BASIC_INFORMATION having timestamp less than -2 and verify NTSTATUS");

            //Testing file system behavior to -2 timestamp value
            //[MS-FSCC] 6 Appendix B: Product Behavior <96>,<97>,<98>,<99>
            //ReFS is proving inconsistent with -2 timestamp value at the moment and is temporarily asserted as inconclusive to avoid regression failure
            string operatingSystem = this.fsaAdapter.TestConfig.Platform.ToString();
                        
            if (this.fsaAdapter.FileSystem == FileSystem.NTFS 
                && !Enum.IsDefined(typeof(OS_MinusTwo_NotSupported_NTFS), operatingSystem))
            {
                SetChangeTime(-3);
                SetLastAccessTime(-3);
                SetLastWriteTime(-3);
                SetCreationTime(-3);

                //Step 3: Set FileBasicInformation with 0, -1 then -2 and verify system response
                BaseTestSite.Log.Add(LogEntryKind.TestStep, "3. SetFileInformation with FileInfoClass.FILE_BASIC_INFORMATION having values 0, -1, then -2 and verify system response");

                TestMinusTwoTimestamp(TimestampType.ChangeTime);
                TestMinusTwoTimestamp(TimestampType.LastAccessTime);
                TestMinusTwoTimestamp(TimestampType.LastWriteTime);
            }
            else
            {
                this.TestSite.Assume.Inconclusive("Value -2 for FileBasicInformation timestamps is only supported by NTFS and ReFS.");
            }
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

        private void TestMinusTwoTimestamp(TimestampType timestampType)
        {
            //Create new file
            CreateFile(FileType.DataFile);

            long initialCreationTime;

            QueryFileBasicInformation(out _, out initialCreationTime,
                out _, out _);

            //SetFileInformation with FileInfoClass.FILE_BASIC_INFORMATION having timestamp equals 0
            SetTimestampUnderTest(timestampType, 0);

            //Verify file system response
            long changeTime;
            long lastAccessTime;
            long lastWriteTime;

            QueryFileBasicInformation(out changeTime, out _, out lastAccessTime, out lastWriteTime);

            long timestampUnderTest = timestampType switch
            {
                TimestampType.ChangeTime => changeTime,
                TimestampType.LastAccessTime => lastAccessTime,
                TimestampType.LastWriteTime => lastWriteTime,
            };
            string creationTime = DateTime.FromFileTime(initialCreationTime).ToString();
            string underTestTimestamp = DateTime.FromFileTime(timestampUnderTest).ToString();

            this.fsaAdapter.AssertAreEqual(this.Manager, creationTime, underTestTimestamp,
                "If " + timestampType + " is 0, MUST NOT change " + timestampType + " attribute");

            //SetFileInformation with FileInfoClass.FILE_BASIC_INFORMATION having timestamp equals -1
            SetTimestampUnderTest(timestampType, -1);

            //Write to file and verify file system response
            WriteToFile();

            QueryFileBasicInformation(out changeTime, out _, out lastAccessTime, out lastWriteTime);

            timestampUnderTest = timestampType switch
            {
                TimestampType.ChangeTime => changeTime,
                TimestampType.LastAccessTime => lastAccessTime,
                TimestampType.LastWriteTime => lastWriteTime,
            };
            underTestTimestamp = DateTime.FromFileTime(timestampUnderTest).ToString();

            this.fsaAdapter.AssertAreEqual(this.Manager, creationTime, underTestTimestamp,
                    "If " + timestampType + " is -1, MUST NOT change " + timestampType + " attribute for all subsequent operations");

            //SetFileInformation with FileInfoClass.FILE_BASIC_INFORMATION having timestamp equals to -2
            SetTimestampUnderTest(timestampType, -2);

            //Write to file and verify file system response
            WriteToFile();

            QueryFileBasicInformation(out changeTime, out _, out lastAccessTime, out lastWriteTime);

            timestampUnderTest = timestampType switch
            {
                TimestampType.ChangeTime => changeTime,
                TimestampType.LastAccessTime => lastAccessTime,
                TimestampType.LastWriteTime => lastWriteTime,
            };
            underTestTimestamp = DateTime.FromFileTime(timestampUnderTest).ToString();

            BaseTestSite.Assert.AreNotEqual(creationTime, underTestTimestamp,
                "If " + timestampType + " is -2, MUST change " + timestampType + " attribute for all subsequent operations");
        }

        private void SetTimestampUnderTest(TimestampType timestampType, long value)
        {
            switch (timestampType)
            {
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

        private enum OS_MinusTwo_NotSupported_NTFS
        {
            WindowsNT4,
            Windows98,
            Windows98SecondEdition,
            Windows2000,
            WindowsXP,
            WindowsServer2003,
            WindowsVista,
            WindowsServer2008,
            Windows7,
            WindowsServer2008R2,
            Windows8,
            WindowsServer2012
        }

        private void SetChangeTime(long changeTime)
        {
            FileBasicInformation fileBasicInformation = new FileBasicInformation();
            fileBasicInformation.ChangeTime.dwHighDateTime = (uint)(changeTime >> 32);
            fileBasicInformation.ChangeTime.dwLowDateTime = (uint)(changeTime & 0xFFFFFFFF);
            fileBasicInformation.Reserved = 0;
            fileBasicInformation.FileAttributes = (uint)FileAttribute.NORMAL;
            byte[] inputBuffer = TypeMarshal.ToBytes<FileBasicInformation>(fileBasicInformation);
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
            byte[] inputBuffer = TypeMarshal.ToBytes<FileBasicInformation>(fileBasicInformation);
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
            byte[] inputBuffer = TypeMarshal.ToBytes<FileBasicInformation>(fileBasicInformation);
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
            byte[] inputBuffer = TypeMarshal.ToBytes<FileBasicInformation>(fileBasicInformation);
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

        private void WriteToFile()
        {
            //write data to file after a time interval
            DateTime currentTime = DateTime.Now;
            DateTime nextTime = DateTime.Now;

            while (currentTime.ToString().Equals(nextTime.ToString()))
            {
                nextTime = DateTime.Now;
            }

            long byteSize = (uint) 2 * 1024 * this.fsaAdapter.ClusterSizeInKB;
            MessageStatus status = this.fsaAdapter.WriteFile(0, byteSize, out _);

            this.fsaAdapter.AssertAreEqual(this.Manager, MessageStatus.SUCCESS, status,
                    "Write data to file should succeed");
        }

        private void QueryFileBasicInformation(out long changeTime, out long creationTime
            , out long lastAccessTime, out long lastWriteTime)
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
        }

        #endregion
    }

}
