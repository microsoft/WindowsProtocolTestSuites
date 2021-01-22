// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Protocols.TestSuites.FileSharing.Common.Adapter;
using Microsoft.Protocols.TestSuites.FileSharing.FSA.Adapter;
using Microsoft.Protocols.TestTools;
using Microsoft.Protocols.TestTools.StackSdk;
using Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Fscc;
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
            MessageStatus status;
            string fileName = this.fsaAdapter.ComposeRandomFileName(8);

            //Step 1: Create File
            BaseTestSite.Log.Add(LogEntryKind.TestStep, "1. Create " + fileType.ToString() + " with FileAccess.FILE_WRITE_ATTRIBUTES");

            status = this.fsaAdapter.CreateFile(
                        FileAttribute.NORMAL,
                        fileType == FileType.DataFile ? CreateOptions.NON_DIRECTORY_FILE : CreateOptions.DIRECTORY_FILE,
                        fileType == FileType.DataFile ? StreamTypeNameToOpen.DATA : StreamTypeNameToOpen.INDEX_ALLOCATION,
                        FileAccess.GENERIC_READ | FileAccess.GENERIC_WRITE | FileAccess.FILE_WRITE_DATA | FileAccess.FILE_WRITE_ATTRIBUTES,
                        ShareAccess.FILE_SHARE_READ | ShareAccess.FILE_SHARE_WRITE,
                        CreateDisposition.OPEN_IF,
                        StreamFoundType.StreamIsFound,
                        SymbolicLinkType.IsNotSymbolicLink,
                        fileType,
                        FileNameStatus.PathNameValid);

            BaseTestSite.Assert.AreEqual(MessageStatus.SUCCESS, status, "Create should succeed.");

            //Step 2: Set file basic information with invalid inputBuffer
            BaseTestSite.Log.Add(LogEntryKind.TestStep, "2. Set FileBasicInformation with invalid inputBuffer and verify NTSTATUS");

            byte[] inputBuffer = new byte[1];
            status = this.fsaAdapter.SetFileInformation(FileInfoClass.FILE_BASIC_INFORMATION, inputBuffer);

            this.fsaAdapter.AssertAreEqual(this.Manager, MessageStatus.INFO_LENGTH_MISMATCH, status,
                    "If InputBufferSize is less than sizeof(FILE_BASIC_INFORMATION), the operation MUST be failed with STATUS_INFO_LENGTH_MISMATCH.");

            //Step 3: Set file basic information time stamp values less than -2
            BaseTestSite.Log.Add(LogEntryKind.TestStep, "3. Set FileBasicInformation with invalid attribute values and verify NTSTATUS");
            SetChangeTime(-3);
            SetLastAccessTime(-3);
            SetLastWriteTime(-3);
            SetCreationTime(-3);

            FileBasicInformation fileBasicInformation = new FileBasicInformation();
            fileBasicInformation.FileAttributes = (uint)(FileAttribute.DIRECTORY | FileAttribute.NORMAL);

            TestFileAttributes(fileType, fileBasicInformation);

            fileBasicInformation.FileAttributes = (uint)(FileAttribute.TEMPORARY | FileAttribute.NORMAL);

            TestFileAttributes(fileType, fileBasicInformation);

            //Set FileBasicInformation with -1 then -2 and verify system response
            if (fileType == FileType.DataFile 
                && (this.fsaAdapter.FileSystem == FileSystem.NTFS || this.fsaAdapter.FileSystem == FileSystem.REFS))
            {
                TestMinusTwoTimestamp();
            }
            else if(fileType != FileType.DirectoryFile)
            {
                this.TestSite.Assume.Inconclusive("Value -2 for FileBasicInformation timestamps is only supported by NTFS and ReFS.");
            }
        }

        private void TestMinusTwoTimestamp()
        {
            //Step 4: Set file basic information time stamp to -1 
            BaseTestSite.Log.Add(LogEntryKind.TestStep, "4. Set FileBasicInformation timestamp to -1");

            long initialCreationTime;

            QueryFileBasicInformation(out _, out initialCreationTime,
                out _, out _);

            SetLastWriteTime(-1);

            //Step 5: Verify file system response
            BaseTestSite.Log.Add(LogEntryKind.TestStep, "5. Write to file and verify file system response");

            WriteToFile();

            long newLastWriteTime;
            QueryFileBasicInformation(out _, out _, out _, out newLastWriteTime);
            BaseTestSite.Log.Add(LogEntryKind.TestStep, DateTime.FromFileTime(initialCreationTime) + " " + DateTime.FromFileTime(newLastWriteTime));
            this.fsaAdapter.AssertAreEqual(this.Manager, initialCreationTime, newLastWriteTime,
                    "If LastWriteTime is -1, don’t update with implicit value for subsequent I/O operations");

            //Step 6: Set file basic information time stamp to -2
            BaseTestSite.Log.Add(LogEntryKind.TestStep, "6. Set FileBasicInformation timestamp to -2");

            SetLastWriteTime(-2);

            //Step 7: Verify file system response
            BaseTestSite.Log.Add(LogEntryKind.TestStep, "7. Write to file and verify file system response");
            WriteToFile();

            long newLastAccessTime;
            long newChangeTime;
            QueryFileBasicInformation(out newChangeTime, out _, out newLastAccessTime, out newLastWriteTime);

            BaseTestSite.Assert.AreNotEqual(initialCreationTime, newLastAccessTime,
                "The file system updates the values of the LastAccessTime, LastWriteTime, and ChangeTime members as appropriate after an I / O operation is performed on a file");
            
            BaseTestSite.Assert.AreEqual(newLastAccessTime, newChangeTime,
                "The file system updates the values of the LastAccessTime, LastWriteTime, and ChangeTime members as appropriate after an I / O operation is performed on a file");
            
            this.fsaAdapter.AssertAreEqual(this.Manager, newLastAccessTime, newLastWriteTime,
                    "If LastWriteTime is -2, resume setting timestamp implicitly for subsequent I/O operations");
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
            //write data to file with 1 second time interval
            Thread.Sleep(1000);

            long byteSize = (uint) 2 * 1024 * this.fsaAdapter.ClusterSizeInKB;
            MessageStatus status = this.fsaAdapter.WriteFile(0, byteSize, out _);
            this.fsaAdapter.AssertAreEqual(this.Manager, MessageStatus.SUCCESS, status,
                    "Write data to file should succeed");
        }

        private void QueryFileBasicInformation(out long changeTime, out long creationTime
            , out long lastAccessTime, out long lastWriteTime)
        {
            FileBasicInformation fileBasicInformation = new FileBasicInformation();
            byte[] outputBuffer;
            uint outputBufferSize = (uint)TypeMarshal.ToBytes<FileBasicInformation>(fileBasicInformation).Length;
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
