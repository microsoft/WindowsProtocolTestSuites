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
        [Description("Send FSCTL_SET_SPARSE request to a file and check if SparseFile is supported.")]
        public void FsCtl_Set_Sparse_File_IsSparseFileSupported()
        {
            FsCtl_Set_Sparse_IsSparseFileSupported(FileType.DataFile);
        }

        [TestMethod()]
        [TestCategory(TestCategories.Fsa)]
        [TestCategory(TestCategories.IoCtlRequest)]
        [TestCategory(TestCategories.NonSmb)]
        [TestCategory(TestCategories.Positive)]
        [Description("Send FSCTL_SET_SPARSE request to a directory and check if SparseFile is supported.")]
        public void FsCtl_Set_Sparse_Dir_IsSparseFileSupported()
        {
            FsCtl_Set_Sparse_IsSparseFileSupported(FileType.DirectoryFile);
        }

        #endregion

        #region Test Case Utility

        private void FsCtl_Set_Sparse_IsSparseFileSupported(FileType fileType)
        {
            BaseTestSite.Log.Add(LogEntryKind.TestStep, "Test case steps:");
            MessageStatus status;

            //Step 1: Create file
            BaseTestSite.Log.Add(LogEntryKind.TestStep, "1. Create " + fileType.ToString());
            status = this.fsaAdapter.CreateFile(fileType);

            //Step 2: Write data to file
            if (fileType == FileType.DataFile)
            {
                long bytesToWrite = 1024;
                long bytesWritten = 0;
                BaseTestSite.Log.Add(LogEntryKind.TestStep, "2. Write the file with " + bytesToWrite + " bytes.");
                status = this.fsaAdapter.WriteFile(0, bytesToWrite, out bytesWritten);
            }
            else
            {
                BaseTestSite.Log.Add(LogEntryKind.TestStep, "2. Do not write data to directory file.");
            }

            //Step 3: FSCTL request with FSCTL_SET_SPARSE
            BaseTestSite.Log.Add(LogEntryKind.TestStep, "3. FSCTL request with FSCTL_SET_SPARSE");
            status = this.fsaAdapter.FsCtlSetSparse(true);

            //Step 4: Verify test result
            BaseTestSite.Log.Add(LogEntryKind.TestStep, "4. Verify returned NTSTATUS code.");
            if (this.fsaAdapter.FileSystem == FileSystem.FAT32)
            {
                this.fsaAdapter.AssertAreEqual(this.Manager, MessageStatus.INVALID_DEVICE_REQUEST, status,
                "If the object store does not implement this functionality, the operation MUST be failed with STATUS_INVALID_DEVIeCE_REQUEST.");
                return;
            }

            if (fileType == FileType.DirectoryFile)
            {
                if (this.fsaAdapter.FileSystem == FileSystem.CSVFS)
                {
                    this.fsaAdapter.AssertAreEqual(this.Manager, MessageStatus.STATUS_NOT_IMPLEMENTED, status,
                        "If the Open is a directory on a Cluster Shared Volume File System (CSVFS), the operation MUST be failed with STATUS_NOT_IMPLEMENTED.");
                }
                else
                {
                    this.fsaAdapter.AssertAreEqual(this.Manager, MessageStatus.INVALID_PARAMETER, status,
                        "If the file is not a Data file, the operation MUST be failed with STATUS_INVALID_PARAMETER.");
                }
                return;
            }

            if (this.fsaAdapter.IsSparseFileSupported == false)
            {
                this.fsaAdapter.AssertAreEqual(this.Manager, MessageStatus.INVALID_DEVICE_REQUEST, status,
                    "If the object store does not implement this functionality, the operation MUST be failed with STATUS_INVALID_DEVICE_REQUEST.");
            }
            else
            {
                this.fsaAdapter.AssertAreEqual(this.Manager, MessageStatus.SUCCESS, status, "FSCTL_SET_SPARSE is supported, status set to STATUS_SUCCESS.");
            }
        }
        
        #endregion
    }
}
