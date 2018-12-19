// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Protocols.TestSuites.FileSharing.Common.Adapter;
using Microsoft.Protocols.TestSuites.FileSharing.FSA.Adapter;
using Microsoft.Protocols.TestTools;
using Microsoft.Protocols.TestTools.StackSdk;
using Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Fscc;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Protocols.TestSuites.FileSharing.FSA.TestSuite
{
    public partial class FsInfoTestCases : PtfTestClassBase
    {
        #region Test Cases

        [TestMethod()]
        [TestCategory(TestCategories.Fsa)]
        [TestCategory(TestCategories.SetFileSystemInformation)]
        [TestCategory(TestCategories.NonSmb)]
        [TestCategory(TestCategories.Positive)]
        [Description("Try to set FileFsSectorSizeInformation to a file and expect failed with STATUS_INVALID_INFO_CLASS.")]
        public void FsInfo_Set_FileFsSectorSizeInformation_File_InvalidInfoClass()
        {
            FsInfo_Set_FileFsSectorSizeInformation_InvalidInfoClass(FileType.DataFile);
        }

        [TestMethod()]
        [TestCategory(TestCategories.Fsa)]
        [TestCategory(TestCategories.SetFileSystemInformation)]
        [TestCategory(TestCategories.NonSmb)]
        [TestCategory(TestCategories.Positive)]
        [Description("Try to set FileFsSectorSizeInformation to a directory and expect failed with STATUS_INVALID_INFO_CLASS.")]
        public void FsInfo_Set_FileFsSectorSizeInformation_Dir_InvalidInfoClass()
        {
            FsInfo_Set_FileFsSectorSizeInformation_InvalidInfoClass(FileType.DirectoryFile);
        }

        #endregion

        #region Test Case Utility
        private void FsInfo_Set_FileFsSectorSizeInformation_InvalidInfoClass(FileType fileType)
        {
            BaseTestSite.Log.Add(LogEntryKind.TestStep, "Test case steps:");
            MessageStatus status;

            //Step 1: Create file
            BaseTestSite.Log.Add(LogEntryKind.TestStep, "1. Create " + fileType.ToString());
            status = this.fsaAdapter.CreateFile(fileType);

            //Step 2: Set File_FsSectorSizeInformation
            FILE_FS_SECTOR_SIZE_INFORMATION fileFsSectorSizeInfo = new FILE_FS_SECTOR_SIZE_INFORMATION();
            byte[] inputBuffer = TypeMarshal.ToBytes<FILE_FS_SECTOR_SIZE_INFORMATION>(fileFsSectorSizeInfo);

            BaseTestSite.Log.Add(LogEntryKind.TestStep, "2. Set File_FsSectorSizeInformation.");
            status = this.fsaAdapter.SetFileSystemInformation((uint)FileSystemInfoClass.File_FsSectorSizeInformation, inputBuffer);

            //Step 3: Verify test result
            BaseTestSite.Log.Add(LogEntryKind.TestStep, "3. Verify returned NTStatus code.");
            this.fsaAdapter.AssertAreEqual(this.Manager, MessageStatus.INVALID_INFO_CLASS, status,
                "This operation is not supported and MUST be failed with STATUS_INVALID_INFO_CLASS.");
        }
        #endregion
    }
}
