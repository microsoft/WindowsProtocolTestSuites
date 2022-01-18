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
        [Description("The FILE_INTERNAL_INFORMATION structure is used to query for the file system's 8-byte file reference number for a file.")]
        public void FileInfo_Query_FileInternalInformation()
        {            
            if (fsaAdapter.Transport != Transport.SMB3)
            {
                TestSite.Assert.Inconclusive("FSA Transport must be set to SMB3 in order to test query FileInternalInformation.");
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

            //Step 2: Query FileInternalInformation
            long byteCount;
            
            uint outputBufferSize = 4 + 64 * 2; // FileNameInformation([MS-FSCC] 2.1.7): length + 64 Unicode characters which is long enough to hold random file name
            byte[] outputBuffer = new byte[outputBufferSize];

            BaseTestSite.Log.Add(LogEntryKind.TestStep, "2. QueryFileInformation with FileInfoClass.FILE_INTERNAL_INFORMATION");
            status = this.fsaAdapter.QueryFileInformation(FileInfoClass.FILE_INTERNAL_INFORMATION, outputBufferSize, out byteCount, out outputBuffer);

            //Step 3: Verify return status
            BaseTestSite.Log.Add(LogEntryKind.TestStep, "3. Verify return status.");
            if (status != MessageStatus.SUCCESS)
            {
                if (status == (MessageStatus)NtStatus.STATUS_INFO_LENGTH_MISMATCH)
                {
                    TestSite.Assert.Fail("Query FILE_INTERNAL_INFORMATION with status {0}. [MS-FSA] Section 2.1.5.11.17: If OutputBufferSize is smaller than sizeof(FILE_INTERNAL_INFORMATION), the operation MUST be failed with STATUS_INFO_LENGTH_MISMATCH.", status);
                }
                else
                {
                    TestSite.Assert.Fail("Query FILE_INTERNAL_INFORMATION with status {0}. The query failed with unknown error.", status);
                }
            }

            TestSite.Assert.AreEqual(status, MessageStatus.SUCCESS, "[MS-FSCC] section 2.4.20: Upon success, the status code returned by the function that processes this file information class is STATUS_SUCCESS. .");

            //Step 4: Verify return result
            BaseTestSite.Log.Add(LogEntryKind.TestStep, "4. Print IndexNumber of FileInternalInformation.");
            var fileInternalInformation = TypeMarshal.ToStruct<FileInternalInformation>(outputBuffer);
            BaseTestSite.Log.Add(LogEntryKind.Comment, string.Format("The file was created with FileInternalInformation {0}", fileInternalInformation.IndexNumber));

            //For file systems that do not support a 64-bit file ID, this field MUST be set to 0, and MUST be ignored.
            //See [MS-FSCC] section <10> for the supportted file system list.
            if (this.fsaAdapter.Is64bitFileIdSupported)
            {
                Site.Assert.AreNotEqual(0, fileInternalInformation.IndexNumber, "FileId of the entry should not be 0.");
            }
            else
            {
                //For file systems that do not support a 64 - bit file ID, this field MUST be set to 0, and MUST be ignored. 
                Site.Assert.AreEqual(0, fileInternalInformation.IndexNumber, "FileId of the entry should be 0 if the file system does not support a 64-bit file ID.");
            }
        }
        
        #endregion
    }
}
