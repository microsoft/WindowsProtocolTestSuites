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
        #region Test cases

        #region FsCtl_Get_IntegrityInformation_IsIntegritySupported

        [TestMethod()]
        [TestCategory(TestCategories.Bvt)]
        [TestCategory(TestCategories.Fsa)]
        [TestCategory(TestCategories.IoCtlRequest)]
        [TestCategory(TestCategories.NonSmb)]
        [Description("Send FSCTL_GET_INTEGRITY_INFORMATION request to a file and check if Integrity is supported.")]
        public void FsCtl_Get_IntegrityInformation_File_IsIntegritySupported()
        {
            FsCtl_Get_IntegrityInformation_IsIntegritySupported(FileType.DataFile);
        }

        [TestMethod()]
        [TestCategory(TestCategories.Bvt)]
        [TestCategory(TestCategories.Fsa)]
        [TestCategory(TestCategories.IoCtlRequest)]
        [TestCategory(TestCategories.NonSmb)]
        [Description("Send FSCTL_GET_INTEGRITY_INFORMATION request to a directory and check if Integrity is supported.")]
        public void FsCtl_Get_IntegrityInformation_Dir_IsIntegritySupported()
        {
            FsCtl_Get_IntegrityInformation_IsIntegritySupported(FileType.DirectoryFile);
        }
        #endregion

        #region FsCtl_Get_IntegrityInformation_OutputBufferSizeLessThanIntegrityBuffer

        [TestMethod()]
        [TestCategory(TestCategories.Fsa)]
        [TestCategory(TestCategories.IoCtlRequest)]
        [TestCategory(TestCategories.NonSmb)]
        [TestCategory(TestCategories.Positive)]
        [Description("Send FSCTL_GET_INTEGRITY_INFORMATION request to a file and check if server responses correctly when OutputBufferSize is less than IntegrityBuffer.")]
        public void FsCtl_Get_IntegrityInformation_File_OutputBufferSizeLessThanIntegrityBuffer()
        {
            FsCtl_Get_IntegrityInformation_OutputBufferSizeLessThanIntegrityBuffer(FileType.DataFile);
        }

        [TestMethod()]
        [TestCategory(TestCategories.Fsa)]
        [TestCategory(TestCategories.IoCtlRequest)]
        [TestCategory(TestCategories.NonSmb)]
        [TestCategory(TestCategories.Positive)]
        [Description("Send FSCTL_GET_INTEGRITY_INFORMATION request to a directory and check if server responses correctly when OutputBufferSize is less than IntegrityBuffer.")]
        public void FsCtl_Get_IntegrityInformation_Dir_OutputBufferSizeLessThanIntegrityBuffer()
        {
            FsCtl_Get_IntegrityInformation_OutputBufferSizeLessThanIntegrityBuffer(FileType.DirectoryFile);
        }

        #endregion

        #region FsCtl_Get_IntegrityInformation_SystemFile

        [TestMethod()]
        [TestCategory(TestCategories.Fsa)]
        [TestCategory(TestCategories.IoCtlRequest)]
        [TestCategory(TestCategories.NonSmb)]
        [TestCategory(TestCategories.Positive)]
        [Description("Send FSCTL_GET_INTEGRITY_INFORMATION request to a system file and check if server responses correctly.")]
        public void FsCtl_Get_IntegrityInformation_File_SystemFile()
        {
            FsCtl_Get_IntegrityInformation_SystemFile(FileType.DataFile);
        }

        [TestMethod()]
        [TestCategory(TestCategories.Fsa)]
        [TestCategory(TestCategories.IoCtlRequest)]
        [TestCategory(TestCategories.NonSmb)]
        [TestCategory(TestCategories.Positive)]
        [Description("Send FSCTL_GET_INTEGRITY_INFORMATION request to a system directory and check if server responses correctly.")]
        public void FsCtl_Get_IntegrityInformation_Dir_SystemFile()
        {
            FsCtl_Get_IntegrityInformation_SystemFile(FileType.DirectoryFile);
        }
        #endregion

        #region FsCtl_GetIntegrityInformation_OutputValue_Common

        [TestMethod()]
        [TestCategory(TestCategories.Fsa)]
        [TestCategory(TestCategories.IoCtlRequest)]
        [TestCategory(TestCategories.NonSmb)]
        [TestCategory(TestCategories.Positive)]
        [Description("Send FSCTL_GET_INTEGRITY_INFORMATION request to a file and check if OutputValue of server response is correct.")]
        public void FsCtl_Get_IntegrityInformation_File_OutputValue_Common()
        {
            FsCtl_Get_IntegrityInformation_OutputValue_Common(FileType.DataFile);
        }

        [TestMethod()]
        [TestCategory(TestCategories.Fsa)]
        [TestCategory(TestCategories.IoCtlRequest)]
        [TestCategory(TestCategories.NonSmb)]
        [TestCategory(TestCategories.Positive)]
        [Description("Send FSCTL_GET_INTEGRITY_INFORMATION request to a directory and check if OutputValue of server response is correct.")]
        public void FsCtl_Get_IntegrityInformation_Dir_OutputValue_Common()
        {
            FsCtl_Get_IntegrityInformation_OutputValue_Common(FileType.DirectoryFile);
        }

        #endregion

        #region FsCtl_Get_IntegrityInformation_File_OutputValue_ChecksumEnforcement

        [TestMethod()]
        [TestCategory(TestCategories.Fsa)]
        [TestCategory(TestCategories.IoCtlRequest)]
        [TestCategory(TestCategories.NonSmb)]
        [TestCategory(TestCategories.Positive)]
        [Description("Send FSCTL_GET_INTEGRITY_INFORMATION request to a file and check if Checksum of server response is correct.")]
        public void FsCtl_Get_IntegrityInformation_File_OutputValue_ChecksumEnforcement()
        {
            FsCtl_Get_IntegrityInformation_OutputValue_ChecksumEnforcement(FileType.DataFile);
        }

        [TestMethod()]
        [TestCategory(TestCategories.Fsa)]
        [TestCategory(TestCategories.IoCtlRequest)]
        [TestCategory(TestCategories.NonSmb)]
        [TestCategory(TestCategories.Positive)]
        [Description("Send FSCTL_GET_INTEGRITY_INFORMATION request to a directory and check if Checksum of server response is correct.")]
        public void FsCtl_Get_IntegrityInformation_Dir_OutputValue_ChecksumEnforcement()
        {
            FsCtl_Get_IntegrityInformation_OutputValue_ChecksumEnforcement(FileType.DirectoryFile);
        }

        #endregion

        #endregion

        #region Test Case Utility

        private void FsCtl_Get_IntegrityInformation_IsIntegritySupported(FileType fileType)
        {
            BaseTestSite.Log.Add(LogEntryKind.TestStep, "Test case steps:");
            MessageStatus status;

            //Step 1: Create file
            BaseTestSite.Log.Add(LogEntryKind.TestStep, "1. Create " + fileType.ToString());            
            status = this.fsaAdapter.CreateFile(fileType);

            //Step 2: FSCTL Request with FSCTL_GET_INTEGRITY_INFORMATION
            FSCTL_GET_INTEGRITY_INFORMATION_BUFFER integrityInfo = new FSCTL_GET_INTEGRITY_INFORMATION_BUFFER();
            uint outputBufferSize = (uint)TypeMarshal.ToBytes<FSCTL_GET_INTEGRITY_INFORMATION_BUFFER>(integrityInfo).Length;

            long bytesReturned;
            byte[] outputBuffer = new byte[0];
            BaseTestSite.Log.Add(LogEntryKind.TestStep, "2. FSCTL Request with FSCTL_GET_INTEGRITY_INFORMATION.");
            status = this.fsaAdapter.FsCtlGetIntegrityInfo(outputBufferSize, out bytesReturned, out outputBuffer);

            //Step 3: Verify test result
            BaseTestSite.Log.Add(LogEntryKind.TestStep, "3. Verify returned NTStatus code.");
            if (!IsCurrentTransportSupportIntegrity(status)) return;

            if (this.fsaAdapter.IsIntegritySupported == false)
            {
                this.fsaAdapter.AssertAreEqual(this.Manager, MessageStatus.INVALID_DEVICE_REQUEST, status, "If the object store does not implement this functionality, the operation MUST be failed with STATUS_INVALID_DEVICE_REQUEST.");
            }
            else
            {
                this.fsaAdapter.AssertAreEqual(this.Manager, MessageStatus.SUCCESS, status, "Integrity is supported, status set to STATUS_SUCCESS.");
            }
        }

        private void FsCtl_Get_IntegrityInformation_OutputBufferSizeLessThanIntegrityBuffer(FileType fileType)
        {
            BaseTestSite.Log.Add(LogEntryKind.TestStep, "Test case steps:");
            MessageStatus status;

            //Step 1: Create file
            BaseTestSite.Log.Add(LogEntryKind.TestStep, "1. Create " + fileType.ToString());
            status = this.fsaAdapter.CreateFile(fileType);

            //Step 2: FSCTL Request with FSCTL_GET_INTEGRITY_INFORMATION
            FSCTL_GET_INTEGRITY_INFORMATION_BUFFER integrityInfo = new FSCTL_GET_INTEGRITY_INFORMATION_BUFFER();
            uint outputBufferSize = (uint)TypeMarshal.ToBytes<FSCTL_GET_INTEGRITY_INFORMATION_BUFFER>(integrityInfo).Length - 1;

            long bytesReturned;
            byte[] outputBuffer = new byte[0];
            BaseTestSite.Log.Add(LogEntryKind.TestStep, "2. FSCTL Request with FSCTL_GET_INTEGRITY_INFORMATION.");

            BaseTestSite.Log.Add(LogEntryKind.TestStep, "Parameter: outputBufferSize is sizeof(FSCTL_GET_INTEGRITY_INFORMATION_BUFFER) - 1");
            status = this.fsaAdapter.FsCtlGetIntegrityInfo(outputBufferSize, out bytesReturned, out outputBuffer);

            //Step 3: Verify test result
            BaseTestSite.Log.Add(LogEntryKind.TestStep, "3. Verify returned NTStatus code.");
            if (!IsCurrentTransportSupportIntegrity(status)) return;

            if (this.fsaAdapter.IsIntegritySupported == false)
            {
                this.fsaAdapter.AssertAreEqual(this.Manager, MessageStatus.INVALID_DEVICE_REQUEST, status, "If the object store does not implement this functionality, the operation MUST be failed with STATUS_INVALID_DEVICE_REQUEST.");
            }
            else
            {
                this.fsaAdapter.AssertAreEqual(this.Manager, MessageStatus.INVALID_PARAMETER, status,
                    "The operation MUST be failed with STATUS_INVALID_PARAMETER if OutputBufferSize is less than sizeof(FSCTL_GET_INTEGRITY_INFORMATION_BUFFER).");
            }
        }

        private void FsCtl_Get_IntegrityInformation_SystemFile(FileType fileType)
        {
            BaseTestSite.Log.Add(LogEntryKind.TestStep, "Test case steps:");
            MessageStatus status;

            //Step 1: Create file
            BaseTestSite.Log.Add(LogEntryKind.TestStep, "1. Create " + fileType.ToString());
            BaseTestSite.Log.Add(LogEntryKind.Debug, "Parameter: Set FileAttribute to SYSTEM.");
            
            status = this.fsaAdapter.CreateFile(
                        FileAttribute.SYSTEM,
                        fileType == FileType.DataFile ? CreateOptions.NON_DIRECTORY_FILE : CreateOptions.DIRECTORY_FILE,
                        StreamTypeNameToOpen.NULL,
                        FileAccess.GENERIC_READ | FileAccess.GENERIC_WRITE,
                        ShareAccess.FILE_SHARE_READ | ShareAccess.FILE_SHARE_WRITE,
                        CreateDisposition.OPEN_IF,
                        StreamFoundType.StreamIsFound,
                        SymbolicLinkType.IsNotSymbolicLink,
                        fileType,
                        FileNameStatus.PathNameValid);

            //Step 2: FSCTL request with FSCTL_GET_INTEGRITY_INFORMATION
            FSCTL_GET_INTEGRITY_INFORMATION_BUFFER integrityInfo = new FSCTL_GET_INTEGRITY_INFORMATION_BUFFER();
            uint outputBufferSize = (uint)TypeMarshal.ToBytes<FSCTL_GET_INTEGRITY_INFORMATION_BUFFER>(integrityInfo).Length;

            long bytesReturned;
            byte[] outputBuffer = new byte[0];
            BaseTestSite.Log.Add(LogEntryKind.TestStep, "2. FSCTL request with FSCTL_GET_INTEGRITY_INFORMATION.");
            status = this.fsaAdapter.FsCtlGetIntegrityInfo(outputBufferSize, out bytesReturned, out outputBuffer);

            //Step 3: Verify test result
            BaseTestSite.Log.Add(LogEntryKind.TestStep, "3. Verify returned NTStatus code.");
            if (!IsCurrentTransportSupportIntegrity(status)) return;

            if (this.fsaAdapter.IsIntegritySupported == false)
            {
                this.fsaAdapter.AssertAreEqual(this.Manager, MessageStatus.INVALID_DEVICE_REQUEST, status, "If the object store does not implement this functionality, the operation MUST be failed with STATUS_INVALID_DEVICE_REQUEST.");
            }
            else
            {
                this.fsaAdapter.AssertAreEqual(this.Manager, MessageStatus.SUCCESS, status, "Integrity is supported, status set to STATUS_SUCCESS.");
            }
        }

        private void FsCtl_Get_IntegrityInformation_OutputValue_Common(FileType fileType)
        {
            BaseTestSite.Log.Add(LogEntryKind.TestStep, "Test case steps:");
            MessageStatus status;

            //Step 1: Create file
            BaseTestSite.Log.Add(LogEntryKind.TestStep, "1. Create " + fileType.ToString());            
            status = this.fsaAdapter.CreateFile(fileType);

            //Step 2: FSCTL Request with FSCTL_GET_INTEGRITY_INFORMATION
            FSCTL_GET_INTEGRITY_INFORMATION_BUFFER integrityInfo = new FSCTL_GET_INTEGRITY_INFORMATION_BUFFER();
            uint outputBufferSize = (uint)TypeMarshal.ToBytes<FSCTL_GET_INTEGRITY_INFORMATION_BUFFER>(integrityInfo).Length;

            long bytesReturned;
            byte[] outputBuffer = new byte[0];
            BaseTestSite.Log.Add(LogEntryKind.TestStep, "2. FSCTL Request with FSCTL_GET_INTEGRITY_INFORMATION.");
            status = this.fsaAdapter.FsCtlGetIntegrityInfo(outputBufferSize, out bytesReturned, out outputBuffer);

            //Step 3: Verify test result
            BaseTestSite.Log.Add(LogEntryKind.TestStep, "3. Verify test result.");
            // Check if Integrity is supported
            if (!IsCurrentTransportSupportIntegrity(status)) return;

            if (this.fsaAdapter.IsIntegritySupported == false)
            {
                this.fsaAdapter.AssertAreEqual(this.Manager, MessageStatus.INVALID_DEVICE_REQUEST, status, "If the object store does not implement this functionality, the operation MUST be failed with STATUS_INVALID_DEVICE_REQUEST.");
                return;
            }

            // Check output result
            integrityInfo = TypeMarshal.ToStruct<FSCTL_GET_INTEGRITY_INFORMATION_BUFFER>(outputBuffer);

            BaseTestSite.Log.Add(LogEntryKind.Debug, "3.1. Verify OutputBuffer.CheckSumAlgorithm.");
            bool isCheckSumAlgorithmCorrect = FsaUtility.IsOneOfExpectedChecksumAlgorithm(integrityInfo.ChecksumAlgorithm);
            this.fsaAdapter.AssertAreEqual(this.Manager, true, isCheckSumAlgorithmCorrect,
                "The object store MUST set OutputBuffer.CheckSumAlgorithm to one of the values for ChecksumAlgorithm.");

            BaseTestSite.Log.Add(LogEntryKind.Debug, "3.2. Verify OutputBuffer.ClusterSizeInBytes.");
            BaseTestSite.Log.Add(LogEntryKind.Debug, "Expected ClusterSizeInBytes: " + (this.fsaAdapter.ClusterSizeInKB * 1024));
            BaseTestSite.Log.Add(LogEntryKind.Debug, "Actual ClusterSizeInBytes: " + integrityInfo.ClusterSizeInBytes);
            string comment = string.Format("The default clusterSize for {0} is {1} KB.", this.fsaAdapter.FileSystem.ToString(), this.fsaAdapter.ClusterSizeInKB.ToString());
            this.fsaAdapter.AssertAreEqual(this.Manager, this.fsaAdapter.ClusterSizeInKB * 1024, integrityInfo.ClusterSizeInBytes, comment);

            BaseTestSite.Log.Add(LogEntryKind.Debug, "3.3. Verify CHECKSUM_ENFORCEMENT_OFF flag.");
            bool isChecksumEnforcementSet = ((integrityInfo.Flags & FSCTL_GET_INTEGRITY_INFORMATION_BUFFER_FLAGS.FSCTL_INTEGRITY_FLAG_CHECKSUM_ENFORCEMENT_OFF) != FSCTL_GET_INTEGRITY_INFORMATION_BUFFER_FLAGS.FSCTL_INTEGRITY_FLAG_CHECKSUM_ENFORCEMENT_OFF);
            this.fsaAdapter.AssertAreEqual(this.Manager, true, isChecksumEnforcementSet,
                "If Open.Stream.StreamType is not data stream, the object store should not set OutputBuffer.Flags to CHECKSUM_ENFORCEMENT_OFF.");
        }

        private void FsCtl_Get_IntegrityInformation_OutputValue_ChecksumEnforcement(FileType fileType)
        {
            BaseTestSite.Log.Add(LogEntryKind.TestStep, "Test case steps:");
            MessageStatus status;

            //Step 1: Create file
            BaseTestSite.Log.Add(LogEntryKind.TestStep, "1. Create " + fileType.ToString());            
            status = this.fsaAdapter.CreateFile(fileType, true);

            //Step 2: FsCtlSetIntegrityInfo with CHECKSUM_ENFORCEMENT_OFF flag
            FSCTL_SET_INTEGRITY_INFORMATION_BUFFER setIntegrityInfo = new FSCTL_SET_INTEGRITY_INFORMATION_BUFFER();
            setIntegrityInfo.Flags = FSCTL_SET_INTEGRITY_INFORMATION_BUFFER_FLAGS.FSCTL_INTEGRITY_FLAG_CHECKSUM_ENFORCEMENT_OFF;
            setIntegrityInfo.ChecksumAlgorithm = FSCTL_SET_INTEGRITY_INFORMATION_BUFFER_CHECKSUMALGORITHM.CHECKSUM_TYPE_CRC64;
            uint inputBufferSize = (uint)TypeMarshal.ToBytes<FSCTL_SET_INTEGRITY_INFORMATION_BUFFER>(setIntegrityInfo).Length;

            BaseTestSite.Log.Add(LogEntryKind.TestStep, "2. FsCtlSetIntegrityInfo with CHECKSUM_ENFORCEMENT_OFF flag.");
            status = this.fsaAdapter.FsCtlSetIntegrityInfo(setIntegrityInfo, inputBufferSize);

            if (!IsCurrentTransportSupportIntegrity(status)) return;

            if (this.fsaAdapter.IsIntegritySupported == false)
            {
                this.fsaAdapter.AssertAreEqual(this.Manager, MessageStatus.INVALID_DEVICE_REQUEST, status, "If the object store does not implement this functionality, the operation MUST be failed with STATUS_INVALID_DEVICE_REQUEST.");
                return;
            }

            //Step 3: FSCTL request with FSCTL_GET_INTEGRITY_INFORMATION
            FSCTL_GET_INTEGRITY_INFORMATION_BUFFER integrityInfo = new FSCTL_GET_INTEGRITY_INFORMATION_BUFFER();
            uint outputBufferSize = (uint)TypeMarshal.ToBytes<FSCTL_GET_INTEGRITY_INFORMATION_BUFFER>(integrityInfo).Length;

            long bytesReturned;
            byte[] outputBuffer = new byte[0];
            BaseTestSite.Log.Add(LogEntryKind.TestStep, "3. FSCTL request with FSCTL_GET_INTEGRITY_INFORMATION.");
            status = this.fsaAdapter.FsCtlGetIntegrityInfo(outputBufferSize, out bytesReturned, out outputBuffer);

            //Step 4: Verify test result
            integrityInfo = TypeMarshal.ToStruct<FSCTL_GET_INTEGRITY_INFORMATION_BUFFER>(outputBuffer);

            BaseTestSite.Log.Add(LogEntryKind.TestStep, "4. Verify CHECKSUM_ENFORCEMENT_OFF flag.");
            bool isChecksumEnforcementSet = ((integrityInfo.Flags & FSCTL_GET_INTEGRITY_INFORMATION_BUFFER_FLAGS.FSCTL_INTEGRITY_FLAG_CHECKSUM_ENFORCEMENT_OFF) == FSCTL_GET_INTEGRITY_INFORMATION_BUFFER_FLAGS.FSCTL_INTEGRITY_FLAG_CHECKSUM_ENFORCEMENT_OFF);

            string streamType = (fileType == FileType.DataFile ? "DataStream" : "DirectoryStream");
            string comment = string.Format("If Open.Stream.StreamType is {0} and Open.Stream.ChecksumEnforcementOff is TRUE, then the object store MUST set OutputBuffer.Flags to CHECKSUM_ENFORCEMENT_OFF.", streamType);
            this.fsaAdapter.AssertAreEqual(this.Manager, true, isChecksumEnforcementSet, comment);
        }

        #endregion
    }
}
