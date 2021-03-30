// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Protocols.TestSuites.FileSharing.Common.Adapter;
using Microsoft.Protocols.TestSuites.FileSharing.FSA.Adapter;
using Microsoft.Protocols.TestTools;
using Microsoft.Protocols.TestTools.StackSdk;
using Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Fscc;
using Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Smb2;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Text;
using System;

namespace Microsoft.Protocols.TestSuites.FileSharing.FSA.TestSuite
{
    public partial class FileInfoTestCases : PtfTestClassBase
    {
        [TestMethod()]
        [TestCategory(TestCategories.Bvt)]
        [TestCategory(TestCategories.Fsa)]
        [TestCategory(TestCategories.QueryFileInformation)]
        [TestCategory(TestCategories.NonSmb)]
        [Description("The test case is used to query the buffer alignment required by the underlying device.")]
        public void FileInfo_Query_FileAlignmentInformation()
        {
            // Create a new file
            string fileName = this.fsaAdapter.ComposeRandomFileName(8, ".txt", CreateOptions.NON_DIRECTORY_FILE);

            BaseTestSite.Log.Add(LogEntryKind.TestStep, $"1. Create a file {fileName}");

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
            FileAlignmentInformation fileAlignmentInfo = new FileAlignmentInformation();
            uint outputBufferSize = (uint)TypeMarshal.ToBytes<FileAlignmentInformation>(fileAlignmentInfo).Length;

            BaseTestSite.Log.Add(LogEntryKind.TestStep, $"2. Query FileAlignmentInformation of the file {fileName}.");
            status = this.fsaAdapter.QueryFileInformation(
                FileInfoClass.FILE_ALIGNMENT_INFORMATION,
                outputBufferSize,
                out byteCount,
                out outputBuffer);

            BaseTestSite.Log.Add(LogEntryKind.TestStep, $"3. Verify the query status and outputbuffer.");
            this.TestSite.Assert.AreEqual(
             MessageStatus.SUCCESS,
             status,
             $"Query FileAlignmentInformation of file {fileName} is expected to succeed.");

            // [MS-FSCC] section 2.4.3 AlignmentRequirement (4 bytes):  A 32-bit unsigned integer that MUST contain one of the following values. 
            fileAlignmentInfo = TypeMarshal.ToStruct<FileAlignmentInformation>(outputBuffer);
            this.TestSite.Assert.AreEqual(
                Enum.IsDefined(typeof(AlignmentRequirement_Values), fileAlignmentInfo.AlignmentRequirement),
                true,
                $"AlignmentRequirement MUST contain one of AlignmentRequirement_Values. "
                );

            this.TestSite.Assert.AreEqual(
               byteCount,
               outputBufferSize,
               $"[MS-FSA] section 2.1.5.11.2: Upon successful completion of the operation, the object store MUST return: ByteCount set to sizeof(FileAlignmentInformation)."
               );
        }
    }
}
