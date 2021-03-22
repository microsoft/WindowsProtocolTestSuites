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
        [Description("Create file with longer name and trigger the alternate name and then query file alternate name info.")]
        public void FileInfo_Query_FileAlternateNameInformation()
        {
            if (this.fsaAdapter.FileSystem != FileSystem.NTFS)
            {
                this.TestSite.Assume.Inconclusive("File name with alternate name is supported by NTFS.");
            }

            if (fsaAdapter.Transport != Transport.SMB3)
            {
                TestSite.Assert.Inconclusive("FSA Transport must be set to SMB3 in order to test query FileAlternateNameInformation.");
            }

            BaseTestSite.Log.Add(LogEntryKind.TestStep, "Test case steps:");
            MessageStatus status;

            //Step 1: Create File with alternate name
            // See MS-FSCC note <95>: NTFS assigns an alternate name to a file whose full name is not compliant with restrictions for file names under MS-DOS and 16-bit Windows
            // unless the system has been configured through a registry entry to not generate these names to improve performance.
            // Create a new file with name longger than 8 on NTFS system will bring an alternate name to the file
            string fileName = this.fsaAdapter.ComposeRandomFileName(10, ".txt", CreateOptions.NON_DIRECTORY_FILE);

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

            //Step 2: Query 2FileAlternateNameInformation
            long byteCount;
            
            uint outputBufferSize = 4 + 64 * 2; // FileNameInformation([MS-FSCC] 2.1.7): length + 64 Unicode characters which is long enough to hold random file name
            byte[] outputBuffer = new byte[outputBufferSize];

            BaseTestSite.Log.Add(LogEntryKind.TestStep, "2. QueryFileInformation with FileInfoClass.FileAlternateNameInformation");

            //This operation returns a status code as specified in section 2.2.Upon success, the status code returned by the function that processes this file information class is STATUS_SUCCESS.            
            //STATUS_INFO_LENGTH_MISMATCH  0xC0000004	The specified information record length does not match the length that is required for the specified information class.
            //STATUS_OBJECT_NAME_NOT_FOUND 0xC0000034	The object name is not found or is empty.
            //STATUS_BUFFER_OVERFLOW 0x80000005	The output buffer was filled before the complete name could be returned.
            status = this.fsaAdapter.QueryFileInformation(FileInfoClass.FILE_ALTERNATENAME_INFORMATION, outputBufferSize, out byteCount, out outputBuffer);

            //Step 3: Verify test result
            BaseTestSite.Log.Add(LogEntryKind.TestStep, "3. Verify return status.");

            if (status != MessageStatus.SUCCESS)
            {
                if (status == (MessageStatus) NtStatus.STATUS_INFO_LENGTH_MISMATCH)
                {
                    TestSite.Assert.Fail("Query FILE_ALIGNMENT_INFORMATION with status {0}.The specified information record length does not match the length that is required for the specified information class.", status);
                }
                else if (status == (MessageStatus)NtStatus.STATUS_OBJECT_NAME_NOT_FOUND)
                {
                    TestSite.Assert.Fail("Query FILE_ALIGNMENT_INFORMATION with status {0}.The object name is not found or is empty.", status);
                }
                else if (status == (MessageStatus)NtStatus.STATUS_BUFFER_OVERFLOW)
                {
                    TestSite.Assert.Fail("Query FILE_ALIGNMENT_INFORMATION with status {0}.The output buffer was filled before the complete name could be returned.", status);
                }
                else
                {
                    TestSite.Assert.Fail("Query FILE_ALIGNMENT_INFORMATION with status {0}. The query failed with unknown error.", status);
                }
            }

            BaseTestSite.Log.Add(LogEntryKind.TestStep, "4. Verify outputBuffer as FileAlternateNameInformation.");
            var fileAlternatenameInfo= TypeMarshal.ToStruct<FileAlternateNameInformation>(outputBuffer);

            string returnedFileAlternateName = Encoding.Unicode.GetString(fileAlternatenameInfo.FileName);
            // Alternate name will be compliant with the 8.3 format name.
            // Example: TextFile.Mine.txt becomes TEXTFI~1.TXT(or TEXTFI~2.TXT, should TEXTFI~1.TXT already exist).
            string fileAlternateName = fileName.Substring(0, 6).ToUpper() + @"~1.TXT";

            TestSite.Assert.AreEqual(fileAlternateName.ToUpper(), returnedFileAlternateName, "If the information class is FileAlternateNameInformation, a FILE_NAME_INFORMATION (section 2.1.7) data element containing an 8.3 file name (section 2.1.5.2.1) is returned by the server.");
        }
        #endregion
    }
}
