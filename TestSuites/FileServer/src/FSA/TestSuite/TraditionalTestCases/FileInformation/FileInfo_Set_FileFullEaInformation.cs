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
        [Description("Try to set FileFullEaInformation to a file and check if ExtendedAttributes is supported.")]
        public void FileInfo_Set_FileFullEaInformation_File_IsEASupported()
        {
            FileInfo_Set_FileFullEaInformation_IsEASupported(FileType.DataFile);
        }

        [TestMethod()]
        [TestCategory(TestCategories.Fsa)]
        [TestCategory(TestCategories.SetFileInformation)]
        [TestCategory(TestCategories.NonSmb)]
        [TestCategory(TestCategories.Positive)]
        [Description("Try to set FileFullEaInformation to a directory and check if ExtendedAttributes is supported.")]
        public void FileInfo_Set_FileFullEaInformation_Dir_IsEASupported()
        {
            FileInfo_Set_FileFullEaInformation_IsEASupported(FileType.DirectoryFile);
        }

        #endregion

        #region Test Case Utility

        private void FileInfo_Set_FileFullEaInformation_IsEASupported(FileType fileType)
        {
            BaseTestSite.Log.Add(LogEntryKind.TestStep, "Test case steps:");
            MessageStatus status;

            //Step 1: Create file
            BaseTestSite.Log.Add(LogEntryKind.TestStep, "1. Create " + fileType.ToString());
            status = this.fsaAdapter.CreateFile(fileType);

            //Step 2: Set FILE_FULLEA_INFORMATION
            string eaName = this.fsaAdapter.ComposeRandomFileName(8);
            string eaValue = this.fsaAdapter.ComposeRandomFileName(8);
            FileFullEaInformation fileFullEaInfo = new FileFullEaInformation();
            fileFullEaInfo.NextEntryOffset = 0;
            fileFullEaInfo.Flags = FILE_FULL_EA_INFORMATION_FLAGS.NONE;
            fileFullEaInfo.EaNameLength = (byte)eaName.Length;
            fileFullEaInfo.EaValueLength = (ushort)eaValue.Length;
            fileFullEaInfo.EaName = Encoding.ASCII.GetBytes(eaName + "\0");
            fileFullEaInfo.EaValue = Encoding.ASCII.GetBytes(eaValue);            

            BaseTestSite.Log.Add(LogEntryKind.TestStep, "2. SetFileInformation with FileInfoClass.FILE_FULLEA_INFORMATION.");
            status = this.fsaAdapter.SetFileFullEaInformation(fileFullEaInfo);

            //Step 3: Verify test result
            BaseTestSite.Log.Add(LogEntryKind.TestStep, "3. Verify returned NTSTATUS code.");
            if (this.fsaAdapter.IsExtendedAttributeSupported == false)
            {
                this.fsaAdapter.AssertAreEqual(this.Manager, MessageStatus.INVALID_DEVICE_REQUEST, status,
                    "[MS-FSA] Undocumented error code STATUS_INVALID_DEVICE_REQUEST for query FileFullEaInformation in unsupported file system " + this.fsaAdapter.FileSystem.ToString());
            }
            else
            {
                this.fsaAdapter.AssertAreEqual(this.Manager, MessageStatus.SUCCESS, status,
                    "STATUS_SUCCESS when one or more entries were returned from Open.File.ExtendedAttributes and there are no more entries to return.");
            }
        }
        #endregion
    }
}
