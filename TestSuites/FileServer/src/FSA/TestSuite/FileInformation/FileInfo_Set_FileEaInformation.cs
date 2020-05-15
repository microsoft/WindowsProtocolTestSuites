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
    public partial class FileInfoTestCases : PtfTestClassBase
    {
        #region Test Cases

        [TestMethod()]
        [TestCategory(TestCategories.Fsa)]
        [TestCategory(TestCategories.SetFileInformation)]
        [TestCategory(TestCategories.NonSmb)]
        [TestCategory(TestCategories.Positive)]
        [Description("Try to set FileEaInformation to a file and check if ExtendedAttributes is supported.")]
        public void FileInfo_Set_FileEaInformation_File_IsEASupported()
        {
            FileInfo_Set_FileEaInformation_IsEASupported(FileType.DataFile);
        }

        [TestMethod()]
        [TestCategory(TestCategories.Fsa)]
        [TestCategory(TestCategories.SetFileInformation)]
        [TestCategory(TestCategories.NonSmb)]
        [TestCategory(TestCategories.Positive)]
        [Description("Try to set FileEaInformation to a directory and check if ExtendedAttributes is supported.")]
        public void FileInfo_Set_FileEaInformation_Dir_IsEASupported()
        {
            FileInfo_Set_FileEaInformation_IsEASupported(FileType.DirectoryFile);
        }

        #endregion

        #region Test Case Utility

        private void FileInfo_Set_FileEaInformation_IsEASupported(FileType fileType)
        {
            BaseTestSite.Log.Add(LogEntryKind.TestStep, "Test case steps:");
            MessageStatus status;

            //Step 1: Create file
            BaseTestSite.Log.Add(LogEntryKind.TestStep, "1. Create " + fileType.ToString());
            status = this.fsaAdapter.CreateFile(FileType.DataFile);

            //Step 2: Set FILE_EA_INFORMATION
            FileEaInformation fileEaInfo = new FileEaInformation();
            fileEaInfo.EaSize = 1024;
            byte[] inputBuffer = TypeMarshal.ToBytes<FileEaInformation>(fileEaInfo);

            BaseTestSite.Log.Add(LogEntryKind.TestStep, "2. SetFileInformation with FileInfoClass.FILE_EA_INFORMATION.");
            status = this.fsaAdapter.SetFileInformation(FileInfoClass.FILE_EA_INFORMATION, inputBuffer);

            //Step 3: Verify test result
            BaseTestSite.Log.Add(LogEntryKind.TestStep, "3. Verify returned NTSTATUS code.");
            this.fsaAdapter.AssertAreEqual(this.Manager, MessageStatus.INVALID_INFO_CLASS, status,
                "This operation is not supported and MUST be failed with STATUS_ INVALID_INFO_CLASS.");
        }
        #endregion
    }
}
