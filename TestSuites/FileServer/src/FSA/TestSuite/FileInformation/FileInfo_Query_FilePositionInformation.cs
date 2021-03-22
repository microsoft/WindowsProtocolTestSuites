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
        [Description("The case is used to query the position of the file pointer within a file.")]
        public void FileInfo_Query_FilePositionInformation()
        {
            if (fsaAdapter.Transport != Transport.SMB3)
            {
                TestSite.Assert.Inconclusive("FSA Transport must be set to SMB3 in order to test query FilePositionInformatio.");
            }

            BaseTestSite.Log.Add(LogEntryKind.TestStep, "Test case steps:");
            MessageStatus status;

            //Step 1: Create File
            string fileName = this.fsaAdapter.ComposeRandomFileName(8, ".txt", CreateOptions.NON_DIRECTORY_FILE);

            BaseTestSite.Log.Add(LogEntryKind.TestStep, $"1. Create a file {fileName}");

            FILEID fileId;
            uint treeId = 0;
            ulong sessionId = 0;             

            status = this.fsaAdapter.CreateFile(
               fileName,
               Microsoft.Protocols.TestSuites.FileSharing.FSA.Adapter.FileAttribute.NORMAL,
               CreateOptions.NON_DIRECTORY_FILE | CreateOptions.NO_INTERMEDIATE_BUFFERING,
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

            //Step 2: Query FilePositionInformatio
            long byteCount;

            uint outputBufferSize = 4 + 64 * 2; // FileNameInformation([MS-FSCC] 2.1.7): length + 64 Unicode characters which is long enough to hold random file name
            byte[] outputBuffer = new byte[outputBufferSize];

            BaseTestSite.Log.Add(LogEntryKind.TestStep, "2. SetFilePositionInfo to file on server and verify the return value.");
            status =  this.fsaAdapter.SetFilePositionInfo(InputBufferSize.LessThan, InputBufferCurrentByteOffset.NotValid);
            this.TestSite.Assert.AreEqual(MessageStatus.INFO_LENGTH_MISMATCH, status, $"[MS-FSA] section 2.1.5.11.23: If OutputBufferSize is less than the size, in bytes, of the FILE_POSITION_INFORMATION structure, the operation MUST be failed with STATUS_INFO_LENGTH_MISMATCH.");

            status = this.fsaAdapter.SetFilePositionInfo(InputBufferSize.NotLessThan, InputBufferCurrentByteOffset.Valid);
            this.TestSite.Assert.AreEqual(MessageStatus.SUCCESS, status, $"[MS-FSA] section 2.1.5.11.23: In FilePositionInformation,Pseudocode for the operation is as follows: The operation returns STATUS_SUCCESS.");

            //Step 3: Verify the result
            BaseTestSite.Log.Add(LogEntryKind.TestStep, "3. QueryFileInformation with FileInfoClass.FILE_POSITION_INFORMATION and verify the return status.");
            status = this.fsaAdapter.QueryFileInformation(FileInfoClass.FILE_POSITION_INFORMATION, outputBufferSize, out byteCount, out outputBuffer);
            TestSite.Assert.AreEqual(status, MessageStatus.SUCCESS, "[MS-FSCC] section 2.4.32: Upon success, the status code returned by the function that processes this file information class is STATUS_SUCCESS.");
            
        }

        #endregion
    }
}
