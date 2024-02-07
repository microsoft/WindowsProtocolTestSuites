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
        [TestCategory(TestCategories.UnexpectedFields)]
        [TestCategory(TestCategories.Fsa)]
        [TestCategory(TestCategories.IoCtlRequest)]
        [TestCategory(TestCategories.NonSmb)]
        [Description("Send FSCTL_SET_INTEGRITY_INFORMATION_EX request to a file and check if Integrity is supported. No checksum algorithm and No integrity state change required.")]
        public void FsCtl_Set_IntegrityInformation_Ex_File_IsIntegritySupported_InvalidRequest()
        {
            FsCtl_Set_IntegrityInformation_Ex_Combined(FileType.DataFile, true);
        }

        [TestMethod()]
        [TestCategory(TestCategories.Bvt)]
        [TestCategory(TestCategories.Fsa)]
        [TestCategory(TestCategories.IoCtlRequest)]
        [TestCategory(TestCategories.NonSmb)]
        [Description("Send FSCTL_SET_INTEGRITY_INFORMATION_EX request to a file and check if Integrity is supported.")]
        public void FsCtl_Set_IntegrityInformation_Ex_File_IsIntegritySupported()
        {
            FsCtl_Set_IntegrityInformation_Ex_Combined(FileType.DataFile, false);
        }

        [TestMethod()]
        [TestCategory(TestCategories.UnexpectedFields)]
        [TestCategory(TestCategories.Fsa)]
        [TestCategory(TestCategories.IoCtlRequest)]
        [TestCategory(TestCategories.NonSmb)]
        [Description("Send FSCTL_SET_INTEGRITY_INFORMATION_EX request to a directory and check if Integrity is supported.  No checksum algorithm and No integrity state change required.")]
        public void FsCtl_Set_IntegrityInformation_Ex_DirectoryFile_IsIntegritySupported_InvalidRequest()
        {
            FsCtl_Set_IntegrityInformation_Ex_Combined(FileType.DirectoryFile, true);
        }

        [TestMethod()]
        [TestCategory(TestCategories.Bvt)]
        [TestCategory(TestCategories.Fsa)]
        [TestCategory(TestCategories.IoCtlRequest)]
        [TestCategory(TestCategories.NonSmb)]
        [Description("Send FSCTL_SET_INTEGRITY_INFORMATION_EX request to a directory and check if Integrity is supported.")]
        public void FsCtl_Set_IntegrityInformation_Ex_DirectoryFile_IsIntegritySupported()
        {
            FsCtl_Set_IntegrityInformation_Ex_Combined(FileType.DirectoryFile, false);
        }

        #endregion Test Cases

        #region Utility
        private void FsCtl_Set_IntegrityInformation_Ex_Combined(FileType fileType, bool IsTestingForInvalidParameter)
        {
            BaseTestSite.Log.Add(LogEntryKind.TestStep, "Test case steps:");
            MessageStatus status;

            //Step 1: Create file
            BaseTestSite.Log.Add(LogEntryKind.TestStep, "1. Create " + fileType.ToString());
            status = this.fsaAdapter.CreateFile(fileType);

            //Step 2: FSCTL request FSCTL_SET_INTEGRITY_INFORMATION
            FSCTL_SET_INTEGRITY_INFORMATION_EX_BUFFER integrityInfo = new FSCTL_SET_INTEGRITY_INFORMATION_EX_BUFFER();

            switch (IsTestingForInvalidParameter)
            {
                case true:
                    {
                        integrityInfo.EnableIntegrity = FSCTL_SET_INTEGRITY_INFORMATION_EX_BUFFER_CHECKSUMALGORITHM.CHECKSUM_TYPE_NONE;
                        integrityInfo.KeepIntegrityStateUnchanged = FSCTL_SET_INTEGRITY_INFORMATION_EX_BUFFER_INTEGRITY_STATE.INTEGRITY_STATE_NO_CHANGE;
                        integrityInfo.Flags = FSCTL_SET_INTEGRITY_INFORMATION_BUFFER_FLAGS.FSCTL_INTEGRITY_FLAG_CHECKSUM_ENFORCEMENT_OFF;
                        integrityInfo.Reserved = 0x000;
                        integrityInfo.Version = 1;
                        integrityInfo.Reserved2 = new byte[7] { 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 };

                    }
                    break;
                case false:
                    {
                        integrityInfo.EnableIntegrity = FSCTL_SET_INTEGRITY_INFORMATION_EX_BUFFER_CHECKSUMALGORITHM.CHECKSUM_TYPE_CRC32_OR_CRC64;
                        integrityInfo.KeepIntegrityStateUnchanged = FSCTL_SET_INTEGRITY_INFORMATION_EX_BUFFER_INTEGRITY_STATE.INTEGRITY_STATE_CHANGE;
                        integrityInfo.Flags = FSCTL_SET_INTEGRITY_INFORMATION_BUFFER_FLAGS.FSCTL_INTEGRITY_FLAG_CHECKSUM_ENFORCEMENT_OFF;
                        integrityInfo.Reserved = 0x000;
                        integrityInfo.Version = 1;
                        integrityInfo.Reserved2 = new byte[7] { 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 };
                    }
                    break;
            }

            uint inputBufferSize = (uint)TypeMarshal.ToBytes<FSCTL_SET_INTEGRITY_INFORMATION_EX_BUFFER>(integrityInfo).Length;

            BaseTestSite.Log.Add(LogEntryKind.TestStep, "2. FSCTL request FSCTL_SET_INTEGRITY_INFORMATION_EX.");
            status = this.fsaAdapter.FsCtlSetIntegrityInfo(integrityInfo, inputBufferSize);

            //Step 3: Verify test result
            BaseTestSite.Log.Add(LogEntryKind.TestStep, "3. Verify returned NTStatus code.");
            if (!IsCurrentTransportSupportIntegrity(status)) return;

            switch (IsTestingForInvalidParameter)
            {
                case true:
                    {
                        if (this.fsaAdapter.IsIntegritySupported == false)
                        {
                            this.fsaAdapter.AssertAreEqual(this.Manager, MessageStatus.INVALID_DEVICE_REQUEST, status, "If the object store does not implement this functionality, the operation MUST be failed with STATUS_INVALID_DEVICE_REQUEST.");
                        }
                        else
                        {
                            this.fsaAdapter.AssertAreEqual(this.Manager, MessageStatus.INVALID_PARAMETER, status,
                                "The input buffer length is less than the size, in bytes, of the FSCTL_SET_INTEGRITY_INFORMATION_BUFFER_EX element; the handle is not to a file or directory; or Version is not equal to 1.");
                        }
                    }
                    break;
                case false:
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
                    break;
            }

        }
        #endregion Utility
    }
}
