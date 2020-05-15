// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Protocols.TestSuites.FileSharing.Common.Adapter;
using Microsoft.Protocols.TestSuites.FileSharing.FSA.Adapter;
using Microsoft.Protocols.TestTools;
using Microsoft.Protocols.TestTools.StackSdk;
using Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Fscc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace Microsoft.Protocols.TestSuites.FileSharing.FSA.TestSuite
{
    public partial class AlternateDataStreamTestCases : PtfTestClassBase
    {
        #region Test Cases

        [TestMethod()]
        [TestCategory(TestCategories.Fsa)]
        [TestCategory(TestCategories.IoCtlRequest)]
        [TestCategory(TestCategories.AlternateDataStream)]
        [TestCategory(TestCategories.NonSmb)]
        [TestCategory(TestCategories.Positive)]
        [Description("FSCTL_SET_ZERO_DATA on an Alternate Data Stream on a DataFile.")]
        public void AlternateDataStream_FsCtl_Set_ZeroData_File()
        {
            AlternateDataStream_CreateStream(FileType.DataFile);

            AlternateDataStream_FsCtl_Set_ZeroData(FileType.DataFile);
        }

        [TestMethod()]
        [TestCategory(TestCategories.Fsa)]
        [TestCategory(TestCategories.IoCtlRequest)]
        [TestCategory(TestCategories.AlternateDataStream)]
        [TestCategory(TestCategories.NonSmb)]
        [TestCategory(TestCategories.Positive)]
        [Description("FSCTL_SET_ZERO_DATA on an Alternate Data Stream on a DirectoryFile.")]
        public void AlternateDataStream_FsCtl_Set_ZeroData_Dir()
        {
            AlternateDataStream_CreateStream(FileType.DirectoryFile);

            AlternateDataStream_FsCtl_Set_ZeroData(FileType.DirectoryFile);
        }

        #endregion

        #region Test Case Utility

        private void AlternateDataStream_FsCtl_Set_ZeroData(FileType fileType)
        {
            //Prerequisites: Create streams on a newly created file

            //Step 1: FSCTL request with FSCTL_SET_ZERO_DATA
            FSCTL_SET_ZERO_DATA_Request setZeroDataRequest = new FSCTL_SET_ZERO_DATA_Request();
            setZeroDataRequest.FileOffset = 0;
            setZeroDataRequest.BeyondFinalZero = 0;

            uint inputBufferSize = (uint)TypeMarshal.ToBytes<FSCTL_SET_ZERO_DATA_Request>(setZeroDataRequest).Length;

            BaseTestSite.Log.Add(LogEntryKind.TestStep, "{0}. FSCTL request with FSCTL_SET_ZERO_DATA", ++testStep);
            status = this.fsaAdapter.FsCtlSetZeroData(setZeroDataRequest, inputBufferSize);

            //Step 2: Verify test result
            BaseTestSite.Log.Add(LogEntryKind.TestStep, "{0}. Verify returned NTSTATUS code.", ++testStep);
            if (this.fsaAdapter.IsSetZeroDataSupported == false)
            {
                this.fsaAdapter.AssertAreEqual(this.Manager, MessageStatus.INVALID_DEVICE_REQUEST, status,
                    "If the object store does not implement this functionality, the operation MUST be failed with STATUS_INVALID_DEVICE_REQUEST.");
            }
            else
            {
                this.fsaAdapter.AssertAreEqual(this.Manager, MessageStatus.SUCCESS, status,
                        "FSCTL_SET_ZERO_DATA is supported, status set to STATUS_SUCCESS.");
            }
        }

        #endregion

    }
}