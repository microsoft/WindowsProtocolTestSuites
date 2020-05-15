// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Protocols.TestSuites.FileSharing.Common.Adapter;
using Microsoft.Protocols.TestSuites.FileSharing.FSA.Adapter;
using Microsoft.Protocols.TestTools;
using Microsoft.Protocols.TestTools.StackSdk;
using Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Fscc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Smb2;

namespace Microsoft.Protocols.TestSuites.FileSharing.FSA.TestSuite
{
    public partial class FileInfoTestCases : PtfTestClassBase
    {
        #region Test Cases


        [TestMethod()]
        [TestCategory(TestCategories.Bvt)]
        [TestCategory(TestCategories.Fsa)]
        [TestCategory(TestCategories.QueryFileInformation)]
        [TestCategory(TestCategories.NonSmb)]
        [Description("Create file with ::$DATA as suffix and then query file access info.")]
        public void FileInfo_Query_FileAccessInformation_DataSuffix()
        {
            if (this.fsaAdapter.FileSystem == FileSystem.FAT32)
            {
                this.TestSite.Assume.Inconclusive("File name with stream type or stream data as suffix is not supported by FAT32.");
            }

            // Create a new file
            string fileName = this.fsaAdapter.ComposeRandomFileName(8, ".txt", CreateOptions.NON_DIRECTORY_FILE);
            fileName = $"{fileName}::$DATA";

            BaseTestSite.Log.Add(LogEntryKind.TestStep, $"Create a file {fileName}");

            FILEID fileId;
            uint treeId = 0;
            ulong sessionId = 0;
            MessageStatus status = this.fsaAdapter.CreateFile(
                fileName,
                (FileAttribute)0,
                CreateOptions.NON_DIRECTORY_FILE,
                (FileAccess.GENERIC_READ | FileAccess.GENERIC_WRITE),
                (ShareAccess.FILE_SHARE_READ | ShareAccess.FILE_SHARE_WRITE | ShareAccess.FILE_SHARE_DELETE),
                CreateDisposition.OPEN_IF,
                out fileId,
                out treeId,
                out sessionId);

            this.fsaAdapter.AssertAreEqual(this.Manager,
                MessageStatus.SUCCESS,
                status,
                $"Create file with name {fileName} is expected to succeed.");

            long byteCount;
            byte[] outputBuffer;
            FILE_ACCESS_INFORMATION fileAccessInfo = new FILE_ACCESS_INFORMATION();
            uint outputBufferSize = (uint)TypeMarshal.ToBytes<FILE_ACCESS_INFORMATION>(fileAccessInfo).Length;

            status = this.fsaAdapter.QueryFileInformation(
                FileInfoClass.FILE_ACCESS_INFORMATION,
                outputBufferSize,
                out byteCount,
                out outputBuffer);

            this.fsaAdapter.AssertAreEqual(this.Manager,
             MessageStatus.SUCCESS,
             status,
             $"Query access information of file {fileName} is expected to succeed.");
        }

        #endregion
    }
}
