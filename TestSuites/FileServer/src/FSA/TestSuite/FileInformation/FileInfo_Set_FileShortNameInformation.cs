// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Protocols.TestSuites.FileSharing.Common.Adapter;
using Microsoft.Protocols.TestSuites.FileSharing.FSA.Adapter;
using Microsoft.Protocols.TestTools;
using Microsoft.Protocols.TestTools.StackSdk;
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
        [Description("Try to set FileShortNameInfo to a file and check if ShortName is supported.")]
        public void FileInfo_Set_FileShortNameInfo_File_IsShortNameSupported()
        {
            FileInfo_Set_FileShortNameInfo_IsShortNameSupported(FileType.DataFile);
        }

        [TestMethod()]
        [TestCategory(TestCategories.Fsa)]
        [TestCategory(TestCategories.SetFileInformation)]
        [TestCategory(TestCategories.NonSmb)]
        [TestCategory(TestCategories.Positive)]
        [Description("Try to set FileShortNameInfo to a directory and check if ShortName is supported.")]
        public void FileInfo_Set_FileShortNameInfo_Dir_IsShortNameSupported()
        {
            FileInfo_Set_FileShortNameInfo_IsShortNameSupported(FileType.DirectoryFile);
        }

        #endregion

        #region Test Case Utility

        private void FileInfo_Set_FileShortNameInfo_IsShortNameSupported(FileType fileType)
        {
            BaseTestSite.Log.Add(LogEntryKind.TestStep, "Test case steps:");
            MessageStatus status;

            //Step 1: Create file
            BaseTestSite.Log.Add(LogEntryKind.TestStep, "1. Create " + fileType.ToString() + " with Open.HasRestoreAccess set to TRUE.");
            string fileName = this.fsaAdapter.ComposeRandomFileName(8);
            CreateOptions createFileType = (fileType == FileType.DataFile ? CreateOptions.NON_DIRECTORY_FILE : CreateOptions.DIRECTORY_FILE);
            CreateOptions restoreAccess = CreateOptions.OPEN_FOR_BACKUP_INTENT;

            status = this.fsaAdapter.CreateFile(
                        fileName,
                        FileAttribute.NORMAL,
                        createFileType | restoreAccess, //Open.HasRestoreAccess set to TRUE
                        FileAccess.GENERIC_ALL,
                        ShareAccess.FILE_SHARE_READ | ShareAccess.FILE_SHARE_WRITE | ShareAccess.FILE_SHARE_DELETE,
                        CreateDisposition.OPEN_IF);

            //Step 2: Set FILE_SHORTNAME_INFORMATION
            FileShortNameInformation shortNameInfo = new FileShortNameInformation();

            string shortName = this.fsaAdapter.ComposeRandomFileName(8);
            shortNameInfo.FileName = Encoding.Unicode.GetBytes(shortName);
            shortNameInfo.FileNameLength = (uint)shortNameInfo.FileName.Length;

            byte[] inputBuffer = TypeMarshal.ToBytes<FileShortNameInformation>(shortNameInfo);

            BaseTestSite.Log.Add(LogEntryKind.TestStep, "2. SetFileInformation with FileInfoClass.FILE_SHORTNAME_INFORMATION.");
            status = this.fsaAdapter.SetFileInformation(FileInfoClass.FILE_SHORTNAME_INFORMATION, inputBuffer);

            //Step 3: Verify test result
            BaseTestSite.Log.Add(LogEntryKind.TestStep, "3. Verify returned NTSTATUS code.");
            if (this.fsaAdapter.IsShortNameSupported == false)
            {
                BaseTestSite.Log.Add(LogEntryKind.Debug, "FileShortNameInformation is not supported.");
                this.fsaAdapter.AssertAreEqual(this.Manager, MessageStatus.INVALID_PARAMETER, status,
                    "If a file system does not support a specific File Information Class, STATUS_INVALID_PARAMETER MUST be returned.");
            }
            else
            {
                if (status == MessageStatus.SHORT_NAMES_NOT_ENABLED_ON_VOLUME)
                {
                    BaseTestSite.Log.Add(LogEntryKind.Debug, "If Open.File.Volume.GenerateShortNames is FALSE, the operation MUST be failed with STATUS_SHORT_NAMES_NOT_ENABLED_ON_VOLUME.");
                    BaseTestSite.Log.Add(LogEntryKind.Debug, "To enable short name in specific volume, such as volume with driver letter N:, use command: fsutil 8dot3name set N: 0.");
                }
                else
                {
                    this.fsaAdapter.AssertAreEqual(this.Manager, MessageStatus.SUCCESS, status,
                        "FileShortNameInformation is supported, status set to STATUS_SUCCESS.");
                }
            }
        }

        #endregion
    }
}
