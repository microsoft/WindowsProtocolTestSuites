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
                fileName,
                FileAttribute.NORMAL,
                fileType == FileType.DataFile ? CreateOptions.NON_DIRECTORY_FILE : CreateOptions.DIRECTORY_FILE,
                FileAccess.GENERIC_READ | FileAccess.GENERIC_WRITE | FileAccess.FILE_WRITE_DATA | FileAccess.FILE_WRITE_ATTRIBUTES,
                ShareAccess.FILE_SHARE_READ | ShareAccess.FILE_SHARE_WRITE,
                CreateDisposition.CREATE);

            BaseTestSite.Assert.AreEqual(MessageStatus.SUCCESS, status, "Create should succeed.");

            //Step 2: Set file basic information time stamp less than -2
            BaseTestSite.Log.Add(LogEntryKind.TestStep, "2. Set FileBasicInformation time stamp to -3 and expect INVALID_PARAMETER");
            SetChangeTime(-3);

            if(this.fsaAdapter.FileSystem == FileSystem.NTFS || this.fsaAdapter.FileSystem == FileSystem.REFS)
            {
                if(fileType == FileType.DataFile)
                {
                    CheckWriteOperation();
                }
                CheckReadOperation();
            }
        }

        private void CheckWriteOperation()
        {
            //Step 3: Set file basic information time stamp to -1 
            BaseTestSite.Log.Add(LogEntryKind.TestStep, "3. Set FileBasicInformation timestamp to -1");

            long initialChangeTime;
            long initialCreationTime;
            long initialLastAccessTime;
            long initialLastWriteTime;

            QueryFileBasicInformation(out initialChangeTime, out initialCreationTime,
                out initialLastAccessTime, out initialLastWriteTime);

            SetChangeTime(-1);

            //Step 4: Verify file system response
            BaseTestSite.Log.Add(LogEntryKind.TestStep, "4. Write to file and verify file system response");

            WriteToFile();

            long newChangeTime;
            QueryFileBasicInformation(out newChangeTime, out _, out _, out _);
            BaseTestSite.Log.Add(LogEntryKind.TestStep, DateTime.FromFileTime(initialCreationTime) + " " + DateTime.FromFileTime(newChangeTime));
            this.fsaAdapter.AssertAreEqual(this.Manager, initialCreationTime, newChangeTime,
                    "If ChangeTime is -1, don’t update with implicit value for subsequent I/O operations");

            //Step 5: Set file basic information time stamp to -2
            BaseTestSite.Log.Add(LogEntryKind.TestStep, "5. Set FileBasicInformation timestamp to -2");

            SetChangeTime(-2);

            //Step 6: Verify file system response
            BaseTestSite.Log.Add(LogEntryKind.TestStep, "6. Write to file and verify file system response");
            WriteToFile();

            long newLastWriteTime;
            QueryFileBasicInformation(out newChangeTime, out _, out _, out newLastWriteTime);
            BaseTestSite.Log.Add(LogEntryKind.TestStep, DateTime.FromFileTime(newLastWriteTime) + " " + DateTime.FromFileTime(newChangeTime));
            this.fsaAdapter.AssertAreEqual(this.Manager, newLastWriteTime, newChangeTime,
                    "If ChangeTime is -2, resume setting timestamp implicitly for subsequent I/O operations");
        }

        private void CheckReadOperation()
        {
            //Step 7: Set file basic information time stamp to -1 
            BaseTestSite.Log.Add(LogEntryKind.TestStep, "7. Set FileBasicInformation timestamp to -1");

            long initialChangeTime;
            long initialCreationTime;
            long initialLastAccessTime;
            long initialLastWriteTime;

            QueryFileBasicInformation(out initialChangeTime, out initialCreationTime,
                out initialLastAccessTime, out initialLastWriteTime);

            SetChangeTime(-1);
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
                        "Set file information must succeed");
            }
        }

        private void SetFileBasicInformationLastWriteTime(long lastWriteTime)
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
                        "Set file information must succeed");
            }
        }

        private void SetFileBasicInformationLastAccessTime(long lastAccessTime)
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
                        "Set file information must succeed");
            }
        }

        private void WriteToFile()
        {
            //write data to file with 1 second time interval
            Thread.Sleep(1000);
            MessageStatus status = this.fsaAdapter.WriteFile(0, 4, out _);
            this.fsaAdapter.AssertAreEqual(this.Manager, MessageStatus.SUCCESS, status,
                    "Write data to file must succeed");
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
