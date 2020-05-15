// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Protocols.TestSuites.FileSharing.Common.Adapter;
using Microsoft.Protocols.TestSuites.FileSharing.FSA.Adapter;
using Microsoft.Protocols.TestTools;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Protocols.TestSuites.FileSharing.FSA.TestSuite
{
    public partial class FsCtlTestCases : PtfTestClassBase
    {
        #region Test Cases

        [TestMethod()]
        [TestCategory(TestCategories.Fsa)]
        [TestCategory(TestCategories.IoCtlRequest)]
        [TestCategory(TestCategories.NonSmb)]
        [TestCategory(TestCategories.Positive)]
        [Description("Send FSCTL_GET_REPARSE_POINT request to a ReparsePoint file and check if ReparsePoint is supported.")]
        public void FsCtl_Get_ReparsePoint_IsReparsePointSupported()
        {
            BaseTestSite.Log.Add(LogEntryKind.TestStep, "Test case steps:");
            MessageStatus status;

            //Step 1: Open ReparsePointFile
            BaseTestSite.Log.Add(LogEntryKind.TestStep, "1. Open ReparsePointFile: " + this.fsaAdapter.UncSharePath + "\\" +  this.fsaAdapter.ReparsePointFile);
            string fileName = this.fsaAdapter.ComposeRandomFileName(8);
            status = this.fsaAdapter.CreateFile(
                        this.fsaAdapter.ReparsePointFile,
                        FileAttribute.NORMAL | FileAttribute.REPARSE_POINT,
                        CreateOptions.DIRECTORY_FILE | CreateOptions.OPEN_REPARSE_POINT,
                        FileAccess.FILE_READ_DATA | FileAccess.FILE_READ_ATTRIBUTES | FileAccess.READ_CONTROL | FileAccess.SYNCHRONIZE,
                        ShareAccess.FILE_SHARE_READ | ShareAccess.FILE_SHARE_WRITE,
                        CreateDisposition.OPEN_IF);

            BaseTestSite.Assert.AreEqual(MessageStatus.SUCCESS, status, "Open ReparsePointFile failed with " + status.ToString());

            //Step 2: FSCTL request with FSCTL_GET_REPARSE_POINT
            long byteCount;
            byte[] outputBuffer;
            BaseTestSite.Log.Add(LogEntryKind.TestStep, "2. FSCTL request with FSCTL_GET_REPARSE_POINT.");
            status = this.fsaAdapter.FsCtlGetReparsePoint(2048, out byteCount, out outputBuffer);            

            //Step 3: Verify test result
            BaseTestSite.Log.Add(LogEntryKind.TestStep, "3. Verify returned NTSTATUS code.");
            if (this.fsaAdapter.IsReparsePointSupported == false)
            {
                this.fsaAdapter.AssertAreEqual(this.Manager, MessageStatus.INVALID_DEVICE_REQUEST, status,
                    "If the object store does not implement this functionality, the operation MUST be failed with STATUS_INVALID_DEVICE_REQUEST.");
            }
            else
            {
                if (status == MessageStatus.NOT_A_REPARSE_POINT)
                {
                    BaseTestSite.Log.Add(LogEntryKind.Debug, "The target file is not a reparse point.");
                }
                else
                {
                    this.fsaAdapter.AssertAreEqual(this.Manager, MessageStatus.SUCCESS, status,
                        "FSCTL_GET_REPARSE_POINT is supported, status set to STATUS_SUCCESS.");
                }
            }
        }

        #endregion
    }
}
