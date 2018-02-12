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
        [TestCategory(TestCategories.QueryFileInformation)]
        [TestCategory(TestCategories.NonSmb)]
        [TestCategory(TestCategories.Positive)]
        [Description("Query FileFullEaInformation from a file and check if ExtendedAttributes is supported.")]
        public void FileInfo_Query_FileFullEaInformation_File_IsEASupported()
        {
            FileInfo_Query_FileFullEaInformation_IsEASupported(FileType.DataFile);
        }

        [TestMethod()]
        [TestCategory(TestCategories.Fsa)]
        [TestCategory(TestCategories.QueryFileInformation)]
        [TestCategory(TestCategories.NonSmb)]
        [TestCategory(TestCategories.Positive)]
        [Description("Query FileFullEaInformation from a directory and check if ExtendedAttributes is supported.")]
        public void FileInfo_Query_FileFullEaInformation_Dir_IsEASupported()
        {
            FileInfo_Query_FileFullEaInformation_IsEASupported(FileType.DirectoryFile);
        }

        #endregion

        #region Test Case Utility

        private void FileInfo_Query_FileFullEaInformation_IsEASupported(FileType fileType)
        {
            BaseTestSite.Log.Add(LogEntryKind.TestStep, "Test case steps:");
            MessageStatus status;

            //Step 1: Create File
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

            //Step 3: Query FILE_FULLEA_INFORMATION
            long byteCount;
            byte[] outputBuffer;          
            uint outputBufferSize = (uint)TypeMarshal.ToBytes<FileFullEaInformation>(fileFullEaInfo).Length;

            BaseTestSite.Log.Add(LogEntryKind.TestStep, "3. QueryFileInformation with FileInfoClass.FILE_FULLEA_INFORMATION");
            status = this.fsaAdapter.QueryFileFullEaInformation(outputBufferSize, out byteCount, out outputBuffer);

            //Step 4: Verify test result
            BaseTestSite.Log.Add(LogEntryKind.TestStep, "4. Verify returned NTSTATUS code.");
            if (this.fsaAdapter.Transport == Transport.SMB)
            {
                this.fsaAdapter.AssertAreEqual(this.Manager, MessageStatus.SUCCESS, status,
                    "STATUS_SUCCESS when one or more entries were returned from Open.File.ExtendedAttributes and there are no more entries to return.");
                return;
            }
            if (this.fsaAdapter.IsExtendedAttributeSupported == false)
            {
                this.fsaAdapter.AssertAreEqual(this.Manager, MessageStatus.INVALID_DEVICE_REQUEST, status, 
                    "If the object store does not implement this functionality, the operation MUST be failed with STATUS_INVALID_DEVICE_REQUEST.");
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
