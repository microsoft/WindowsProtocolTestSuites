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
        [Description("FSCTL_GET_INTEGRITY_INFORMATION on an Alternate Data Stream on a DataFile.")]
        public void AlternateDataStream_FsCtl_Get_IntegrityInformation_File()
        {
            AlternateDataStream_CreateStream(FileType.DataFile);

            AlternateDataStream_FsCtl_Get_IntegrityInformation(FileType.DataFile);
        }

        [TestMethod()]
        [TestCategory(TestCategories.Fsa)]
        [TestCategory(TestCategories.IoCtlRequest)]
        [TestCategory(TestCategories.AlternateDataStream)]
        [TestCategory(TestCategories.NonSmb)]
        [TestCategory(TestCategories.Positive)]
        [Description("FSCTL_GET_INTEGRITY_INFORMATION on an Alternate Data Stream on a DirectoryFile.")]
        public void AlternateDataStream_FsCtl_Get_IntegrityInformation_Dir()
        {
            AlternateDataStream_CreateStream(FileType.DirectoryFile);

            AlternateDataStream_FsCtl_Get_IntegrityInformation(FileType.DirectoryFile);
        }

        #endregion

        #region Test Case Utility

        private void AlternateDataStream_FsCtl_Get_IntegrityInformation(FileType fileType)
        {
            //Prerequisites: Create streams on a newly created file

            //Step 1: FSCTL Request with FSCTL_GET_INTEGRITY_INFORMATION
            FSCTL_GET_INTEGRITY_INFORMATION_BUFFER integrityInfo = new FSCTL_GET_INTEGRITY_INFORMATION_BUFFER();
            uint outputBufferSize = (uint)TypeMarshal.ToBytes<FSCTL_GET_INTEGRITY_INFORMATION_BUFFER>(integrityInfo).Length;

            long bytesReturned;
            byte[] outputBuffer = new byte[0];
            BaseTestSite.Log.Add(LogEntryKind.TestStep, "{0}. FSCTL Request with FSCTL_GET_INTEGRITY_INFORMATION.", ++testStep);
            status = this.fsaAdapter.FsCtlGetIntegrityInfo(outputBufferSize, out bytesReturned, out outputBuffer);

            //Step 2: Verify test result
            BaseTestSite.Log.Add(LogEntryKind.TestStep, "{0}. Verify returned NTStatus code.", ++testStep);
            if (this.fsaAdapter.Transport == Transport.SMB)
            {
                this.fsaAdapter.AssertAreEqual(this.Manager, MessageStatus.NOT_SUPPORTED, status,
                    "This operation is not supported for SMB transport.");
            }
            else
            {
                if (this.fsaAdapter.IsIntegritySupported == false)
                {
                    this.fsaAdapter.AssertAreEqual(this.Manager, MessageStatus.INVALID_DEVICE_REQUEST, status, "If the object store does not implement this functionality, the operation MUST be failed with STATUS_INVALID_DEVICE_REQUEST.");
                }
                else
                {
                    this.fsaAdapter.AssertAreEqual(this.Manager, MessageStatus.SUCCESS, status, "Integrity is supported, status set to STATUS_SUCCESS.");
                }
            }
        }

        #endregion

    }
}