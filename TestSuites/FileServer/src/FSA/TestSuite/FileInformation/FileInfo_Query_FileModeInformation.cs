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

        [TestMethod()]
        [TestCategory(TestCategories.Bvt)]
        [TestCategory(TestCategories.Fsa)]
        [TestCategory(TestCategories.QueryFileInformation)]
        [TestCategory(TestCategories.NonSmb)]
        [Description("The case is used to query the mode of the file.")]
        public void FileInfo_Query_FileModeInformation()
        {
            if (fsaAdapter.Transport != Transport.SMB3)
            {
                TestSite.Assert.Inconclusive("FSA Transport must be set to SMB3 in order to test query FileModeInformation.");
            }

            BaseTestSite.Log.Add(LogEntryKind.TestStep, "Test case steps:");
            MessageStatus status;

            //Step 1: Create File
            string fileName = this.fsaAdapter.ComposeRandomFileName(8, ".txt", CreateOptions.NON_DIRECTORY_FILE);

            BaseTestSite.Log.Add(LogEntryKind.TestStep, $"1. Create a file {fileName}");

            FILEID fileId;
            uint treeId = 0;
            ulong sessionId = 0;
            CreateOptions createOptions = CreateOptions.NO_INTERMEDIATE_BUFFERING | CreateOptions.WRITE_THROUGH;
            status = this.fsaAdapter.CreateFile(
               fileName,
               (FileAttribute)0,
               CreateOptions.NON_DIRECTORY_FILE | createOptions,
               (FileAccess.GENERIC_READ | FileAccess.GENERIC_WRITE),
               (ShareAccess.FILE_SHARE_READ | ShareAccess.FILE_SHARE_WRITE | ShareAccess.FILE_SHARE_DELETE),
               CreateDisposition.OPEN_IF,
               out fileId,
               out treeId,
               out sessionId);

            this.fsaAdapter.AssertAreEqual(this.Manager,
                MessageStatus.SUCCESS,
                status,
                $"Create file with name {fileName} with options CreateOptions.NO_INTERMEDIATE_BUFFERING | CreateOptions.WRITE_THROUGH is expected to succeed.");

            //Step 2: Query FileModeInformation 
            long byteCount;

            uint outputBufferSize = 4 + 64 * 2; // FileNameInformation([MS-FSCC] 2.1.7): length + 64 Unicode characters which is long enough to hold random file name
            byte[] outputBuffer = new byte[outputBufferSize];

            BaseTestSite.Log.Add(LogEntryKind.TestStep, "2. Query FileModeInformation with FileInfoClass.");
            status = this.fsaAdapter.QueryFileInformation(FileInfoClass.FILE_MODE_INFORMATION, outputBufferSize, out byteCount, out outputBuffer);

            //Step 3: Verify test result
            //[MS-FSCC] section 2.4.24           
            TestSite.Assert.AreEqual(status, MessageStatus.SUCCESS, "[MS-FSCC] section 2.4.24: Upon success, the status code returned by the function that processes this file information class is STATUS_SUCCESS. .");

            BaseTestSite.Log.Add(LogEntryKind.TestStep, "3. Print mode of FileModeInformation .");
            var fileModeInformation = TypeMarshal.ToStruct<FileModeInformation>(outputBuffer);
            BaseTestSite.Log.Add(LogEntryKind.Comment, $"The file was created with FileModeInformation {fileModeInformation.Mode}");

            this.TestSite.Assert.AreEqual((Mode_Values)createOptions, fileModeInformation.Mode, $"The mode of the file is expected to be {createOptions}.");

        }

    }
}
