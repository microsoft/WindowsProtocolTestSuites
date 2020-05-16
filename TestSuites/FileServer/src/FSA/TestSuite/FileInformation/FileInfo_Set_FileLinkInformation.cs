// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Protocols.TestSuites.FileSharing.Common.Adapter;
using Microsoft.Protocols.TestSuites.FileSharing.FSA.Adapter;
using Microsoft.Protocols.TestTools;
using Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Fscc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Text;

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
        [Description("Try to set FileLinkInfo to a file and check if FileLink is supported.")]
        public void FileInfo_Set_FileLinkInfo_File_IsFileLinkInfoSupported()
        {
            FileInfo_Set_FileLinkInfo_IsFileLinkInfoSupported(FileType.DataFile);
        }

        [TestMethod()]
        [TestCategory(TestCategories.Fsa)]
        [TestCategory(TestCategories.SetFileInformation)]
        [TestCategory(TestCategories.NonSmb)]
        [TestCategory(TestCategories.Positive)]
        [Description("Try to set FileLinkInfo to a directory and check if FileLink is supported.")]
        public void FileInfo_Set_FileLinkInfo_Dir_IsFileLinkInfoSupported()
        {
            FileInfo_Set_FileLinkInfo_IsFileLinkInfoSupported(FileType.DirectoryFile);
        }

        #endregion

        #region Test Case Utility

        private void FileInfo_Set_FileLinkInfo_IsFileLinkInfoSupported(FileType fileType)
        {
            BaseTestSite.Log.Add(LogEntryKind.TestStep, "Test case steps:");
            MessageStatus status;

            //Step 1: Create file
            BaseTestSite.Log.Add(LogEntryKind.TestStep, "1. Create " + fileType.ToString());
            status = this.fsaAdapter.CreateFile(fileType);

            //Step 2: Set FILE_LINK_INFORMATION
            string fileName = this.fsaAdapter.ComposeRandomFileName(8);
            BaseTestSite.Log.Add(LogEntryKind.TestStep, "2. SetFileInformation with FileInfoClass.FILE_LINK_INFORMATION");
            if (this.fsaAdapter.Transport == Transport.SMB)
            {
                FILE_LINK_INFORMATION_TYPE_SMB fileLinkInfo = new FILE_LINK_INFORMATION_TYPE_SMB();
                fileLinkInfo.ReplaceIfExists = 1;
                fileLinkInfo.RootDirectory = 0;
                fileLinkInfo.FileName = Encoding.Unicode.GetBytes(fileName);
                fileLinkInfo.FileNameLength = (uint)fileLinkInfo.FileName.Length;

                status = this.fsaAdapter.SetFileLinkInformation(fileLinkInfo);
                this.fsaAdapter.AssertAreEqual(this.Manager, MessageStatus.NOT_SUPPORTED, status,
                       "[MS-SMB]: Windows-based servers do not support setting FileLinkInformation via the pass-through Information Level mechanism.");
                return;
            }
            else
            {
                FILE_LINK_INFORMATION_TYPE_SMB2 fileLinkInfo = new FILE_LINK_INFORMATION_TYPE_SMB2();
                fileLinkInfo.ReplaceIfExists = 1;
                fileLinkInfo.RootDirectory = 0;
                fileLinkInfo.FileName = Encoding.Unicode.GetBytes(fileName);
                fileLinkInfo.FileNameLength = (uint)fileLinkInfo.FileName.Length;

                status = this.fsaAdapter.SetFileLinkInformation(fileLinkInfo);
            }

            //Step 3: Verify test result
            BaseTestSite.Log.Add(LogEntryKind.TestStep, "3. Verify returned NTStatus code.");
            if (this.fsaAdapter.FileSystem == FileSystem.FAT32)
            {
                this.fsaAdapter.AssertAreEqual(this.Manager, MessageStatus.INVALID_DEVICE_REQUEST, status,
                "If the object store does not implement this functionality, the operation MUST be failed with STATUS_INVALID_DEVIeCE_REQUEST.");
                return;
            }

            if (fileType == FileType.DirectoryFile)
            {
                this.fsaAdapter.AssertAreEqual(this.Manager, MessageStatus.FILE_IS_A_DIRECTORY, status,
                       "If Open.File.FileType is DirectoryFile, the operation MUST be failed with STATUS_FILE_IS_A_DIRECTORY.");
                return;
            }

            if (this.fsaAdapter.IsFileLinkInfoSupported == false)
            {
                this.fsaAdapter.AssertAreEqual(this.Manager, MessageStatus.NOT_SUPPORTED, status, "FileLinkInfo is not supported.");
            }
            else
            {

                this.fsaAdapter.AssertAreEqual(this.Manager, MessageStatus.SUCCESS, status, "The operation returns STATUS_SUCCESS.");
            }
        }

        #endregion
    }
}
