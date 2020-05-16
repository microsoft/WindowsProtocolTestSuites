// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Protocols.TestSuites.FileSharing.Common.Adapter;
using Microsoft.Protocols.TestSuites.FileSharing.FSA.Adapter;
using Microsoft.Protocols.TestTools;
using Microsoft.Protocols.TestTools.StackSdk;
using Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Fscc;
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
        [Description("Send FSCTL_SET_ZERO_DATA request to a file and check if ZeroData is supported.")]
        public void FsCtl_Set_ZeroData_File_IsZeroDataSupported()
        {
            FsCtl_Set_ZeroData_IsZeroDataSupported(FileType.DataFile);
        }

        [TestMethod()]
        [TestCategory(TestCategories.Fsa)]
        [TestCategory(TestCategories.IoCtlRequest)]
        [TestCategory(TestCategories.NonSmb)]
        [TestCategory(TestCategories.Positive)]
        [Description("Send FSCTL_SET_ZERO_DATA request to a directory and check if ZeroData is supported.")]
        public void FsCtl_Set_ZeroData_Dir_IsZeroDataSupported()
        {
            FsCtl_Set_ZeroData_IsZeroDataSupported(FileType.DirectoryFile);
        }

        #endregion

        #region Test Case Utility

        private void FsCtl_Set_ZeroData_IsZeroDataSupported(FileType fileType)
        {
            BaseTestSite.Log.Add(LogEntryKind.TestStep, "Test case steps:");
            MessageStatus status;

            //Step 1: Create file
            BaseTestSite.Log.Add(LogEntryKind.TestStep, "1. Create " + fileType.ToString());
            status = this.fsaAdapter.CreateFile(fileType);

            //Step 2: FSCTL request with FSCTL_SET_ZERO_DATA
            FSCTL_SET_ZERO_DATA_Request setZeroDataRequest = new FSCTL_SET_ZERO_DATA_Request();
            setZeroDataRequest.FileOffset = 0;
            setZeroDataRequest.BeyondFinalZero = 0;

            uint inputBufferSize = (uint)TypeMarshal.ToBytes<FSCTL_SET_ZERO_DATA_Request>(setZeroDataRequest).Length;

            BaseTestSite.Log.Add(LogEntryKind.TestStep, "2. FSCTL request with FSCTL_SET_ZERO_DATA");
            status = this.fsaAdapter.FsCtlSetZeroData(setZeroDataRequest, inputBufferSize);

            //Step 3: Verify test result
            BaseTestSite.Log.Add(LogEntryKind.TestStep, "3. Verify returned NTSTATUS code.");
            if (this.fsaAdapter.IsSetZeroDataSupported == false)
            {
                this.fsaAdapter.AssertAreEqual(this.Manager, MessageStatus.INVALID_DEVICE_REQUEST, status,
                    "If the object store does not implement this functionality, the operation MUST be failed with STATUS_INVALID_DEVICE_REQUEST.");
            }
            else
            {
                if (fileType == FileType.DataFile)
                {
                    this.fsaAdapter.AssertAreEqual(this.Manager, MessageStatus.SUCCESS, status,
                        "FSCTL_SET_ZERO_DATA is supported, status set to STATUS_SUCCESS.");
                }
                else
                {
                    if (this.fsaAdapter.FileSystem == FileSystem.CSVFS)
                    {
                        this.fsaAdapter.AssertAreEqual(this.Manager, MessageStatus.STATUS_NOT_IMPLEMENTED, status,
                            "If the Open is a directory on a Cluster Shared Volume File System (CSVFS), the operation MUST be failed with STATUS_NOT_IMPLEMENTED.");
                    }
                    else
                    {
                        this.fsaAdapter.AssertAreEqual(this.Manager, MessageStatus.INVALID_PARAMETER, status,
                            "The operation MUST be failed with STATUS_INVALID_PARAMETER if Open.Stream.StreamType is not DataStream.");
                    }
                }
            }
        }

        #endregion
    }
}
