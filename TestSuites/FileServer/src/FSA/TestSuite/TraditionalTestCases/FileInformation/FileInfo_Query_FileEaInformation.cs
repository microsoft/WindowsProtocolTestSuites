// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Protocols.TestSuites.FileSharing.Common.Adapter;
using System.Text;
using Microsoft.Protocols.TestSuites.FileSharing.FSA.Adapter;
using Microsoft.Protocols.TestTools;
using Microsoft.Protocols.TestTools.StackSdk;
using Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Fscc;
using Microsoft.SpecExplorer.Runtime.Testing;
using Microsoft.VisualStudio.TestTools.UnitTesting;

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
        [Description("Query FileEaInformation from a file and check if ExtendedAttributes is supported.")]
        public void FileInfo_Query_FileEaInformation_File_IsEASupported()
        {
            FileInfo_Query_FileEaInformation_IsEASupported(FileType.DataFile);
        }

        [TestMethod()]
        [TestCategory(TestCategories.Fsa)]
        [TestCategory(TestCategories.QueryFileInformation)]
        [TestCategory(TestCategories.NonSmb)]
        [TestCategory(TestCategories.Positive)]
        [Description("Query FileEaInformation from a directory and check if ExtendedAttributes is supported.")]
        public void FileInfo_Query_FileEaInformation_Dir_IsEASupported()
        {
            FileInfo_Query_FileEaInformation_IsEASupported(FileType.DirectoryFile);
        }

        #endregion

        #region Test Case Utility

        private void FileInfo_Query_FileEaInformation_IsEASupported(FileType fileType)
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

            byte[] inputBuffer = TypeMarshal.ToBytes<FileFullEaInformation>(fileFullEaInfo);

            BaseTestSite.Log.Add(LogEntryKind.TestStep, "2. SetFileInformation with FileInfoClass.FILE_FULLEA_INFORMATION.");
            status = this.fsaAdapter.SetFileFullEaInformation(fileFullEaInfo);

            //Step 3: Query FILE_EA_INFORMATION
            long byteCount;
            byte[] outputBuffer;

            FileEaInformation fileEaInfo = new FileEaInformation();
            uint outputBufferSize = (uint)TypeMarshal.ToBytes<FileEaInformation>(fileEaInfo).Length;

            BaseTestSite.Log.Add(LogEntryKind.TestStep, "3. QueryFileInformation with FileInfoClass.FILE_EA_INFORMATION");
            status = this.fsaAdapter.QueryFileInformation(FileInfoClass.FILE_EA_INFORMATION, outputBufferSize, out byteCount, out outputBuffer);

            //Step 4: Verify test result
            fileEaInfo = TypeMarshal.ToStruct<FileEaInformation>(outputBuffer);
            BaseTestSite.Log.Add(LogEntryKind.TestStep, "4. Verify outputBuffer.EaSize");
            if (this.fsaAdapter.IsExtendedAttributeSupported == false)
            {
                this.fsaAdapter.AssertAreEqual(this.Manager, (uint)0, fileEaInfo.EaSize, 
                    "ExtendedAttribute is not supported, OutputBuffer.EaSize should be 0.");
            }
            else
            {
                this.fsaAdapter.AssertAreEqual(this.Manager, true, fileEaInfo.EaSize > 0,
                    "ExtendedAttribute is supported, OutputBuffer.EaSize should be greater than 0.");
            }
        }
        #endregion
    }
}
