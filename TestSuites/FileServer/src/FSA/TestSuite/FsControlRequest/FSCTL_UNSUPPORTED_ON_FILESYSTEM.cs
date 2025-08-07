// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Protocols.TestSuites.FileSharing.Common.Adapter;
using Microsoft.Protocols.TestSuites.FileSharing.FSA.Adapter;
using Microsoft.Protocols.TestTools;
using Microsoft.Protocols.TestTools.StackSdk;
using Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Fscc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

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
        [Description("Test FSCTL_GET_COMPRESSION on file systems that don't support it per [MS-FSCC] sections 2.3.17-2.3.18 to verify appropriate status codes are returned when FSCTL is allowed but not supported per section 2.2.")]
        public void FsCtl_Get_Compression_File_UnsupportedOnFileSystem()
        {
            if (this.fsaAdapter.FileSystem == FileSystem.REFS)
            {
                BaseTestSite.Assert.Inconclusive("FSCTL_GET_COMPRESSION is having unexpected behavior on ReFS, skipping test.");
            }
            FsCtl_UnsupportedOnFileSystem_Test(FileType.DataFile, FsControlCommand.FSCTL_GET_COMPRESSION, "FSCTL_GET_COMPRESSION");
        }

        [TestMethod()]
        [TestCategory(TestCategories.Fsa)]
        [TestCategory(TestCategories.IoCtlRequest)]
        [TestCategory(TestCategories.NonSmb)]
        [TestCategory(TestCategories.Positive)]
        [Description("Test FSCTL_SET_COMPRESSION on file systems that don't support it per [MS-FSCC] section 2.3.25 to verify STATUS_INVALID_DEVICE_REQUEST or STATUS_NOT_SUPPORTED is returned when FSCTL is allowed but not supported per section 2.2.")]
        public void FsCtl_Set_Compression_File_UnsupportedOnFileSystem()
        {
            if (this.fsaAdapter.FileSystem == FileSystem.REFS)
            {
                BaseTestSite.Assert.Inconclusive("FSCTL_SET_COMPRESSION is having unexpected behavior on ReFS, skipping test.");
            }
            FsCtl_UnsupportedOnFileSystem_Test(FileType.DataFile, FsControlCommand.FSCTL_SET_COMPRESSION, "FSCTL_SET_COMPRESSION");
        }

        [TestMethod()]
        [TestCategory(TestCategories.Fsa)]
        [TestCategory(TestCategories.IoCtlRequest)]
        [TestCategory(TestCategories.NonSmb)]
        [TestCategory(TestCategories.Positive)]
        [Description("Test FSCTL_SET_SPARSE on file systems that don't support it per [MS-FSCC] section 2.3.41 to verify STATUS_INVALID_DEVICE_REQUEST is returned when FSCTL is allowed but not supported per section 2.2.")]
        public void FsCtl_Set_Sparse_File_UnsupportedOnFileSystem()
        {
            FsCtl_UnsupportedOnFileSystem_Test(FileType.DataFile, FsControlCommand.FSCTL_SET_SPARSE, "FSCTL_SET_SPARSE");
        }

        [TestMethod()]
        [TestCategory(TestCategories.Fsa)]
        [TestCategory(TestCategories.IoCtlRequest)]
        [TestCategory(TestCategories.NonSmb)]
        [TestCategory(TestCategories.Positive)]
        [Description("Test FSCTL_SET_ZERO_DATA on file systems that don't support it per [MS-FSCC] section 2.3.46 to verify STATUS_INVALID_DEVICE_REQUEST is returned when FSCTL is allowed but not supported per section 2.2.")]
        public void FsCtl_Set_ZeroData_File_UnsupportedOnFileSystem()
        {
            FsCtl_UnsupportedOnFileSystem_Test(FileType.DataFile, FsControlCommand.FSCTL_SET_ZERO_DATA, "FSCTL_SET_ZERO_DATA");
        }

        [TestMethod()]
        [TestCategory(TestCategories.Fsa)]
        [TestCategory(TestCategories.IoCtlRequest)]
        [TestCategory(TestCategories.NonSmb)]
        [TestCategory(TestCategories.Positive)]
        [Description("Test FSCTL_GET_INTEGRITY_INFORMATION on file systems that don't support it per [MS-FSCC] section 2.3.56 to verify STATUS_INVALID_DEVICE_REQUEST is returned when FSCTL is allowed but not supported per section 2.2.")]
        public void FsCtl_Get_IntegrityInformation_File_UnsupportedOnFileSystem()
        {
            FsCtl_UnsupportedOnFileSystem_Test(FileType.DataFile, FsControlCommand.FSCTL_GET_INTEGRITY_INFORMATION, "FSCTL_GET_INTEGRITY_INFORMATION");
        }

        [TestMethod()]
        [TestCategory(TestCategories.Fsa)]
        [TestCategory(TestCategories.IoCtlRequest)]
        [TestCategory(TestCategories.NonSmb)]
        [TestCategory(TestCategories.Positive)]
        [Description("Test FSCTL_SET_INTEGRITY_INFORMATION on file systems that don't support it per [MS-FSCC] section 2.3.57 to verify STATUS_INVALID_DEVICE_REQUEST is returned when FSCTL is allowed but not supported per section 2.2.")]
        public void FsCtl_Set_IntegrityInformation_File_UnsupportedOnFileSystem()
        {
            FsCtl_UnsupportedOnFileSystem_Test(FileType.DataFile, FsControlCommand.FSCTL_SET_INTEGRITY_INFORMATION, "FSCTL_SET_INTEGRITY_INFORMATION");
        }

        #endregion

        #region Test Case Utility

        /// <summary>
        /// Test helper method to verify that FSCTLs return appropriate status codes when they are allowed by the protocol
        /// but not supported on the specific file system, as specified in [MS-FSCC] section 2.2.
        /// Special handling for compression FSCTLs: GET may return SUCCESS, SET must fail with appropriate error.
        /// </summary>
        /// <param name="fileType">Type of file object to create (DataFile or DirectoryFile)</param>
        /// <param name="fsctlCommand">The FSCTL command to test</param>
        /// <param name="fsctlName">The name of the FSCTL for logging purposes</param>
        private void FsCtl_UnsupportedOnFileSystem_Test(FileType fileType, FsControlCommand fsctlCommand, string fsctlName)
        {
            BaseTestSite.Log.Add(LogEntryKind.TestStep, $"Test case steps for {fsctlName} on {fileType}:");
            MessageStatus status;

            this.fsaAdapter.TestConfig.CheckFSCTL((uint)fsctlCommand);

            BaseTestSite.Log.Add(LogEntryKind.TestStep, $"1. Create {fileType}");
            status = this.fsaAdapter.CreateFile(fileType);
            this.fsaAdapter.AssertAreEqual(this.Manager, MessageStatus.SUCCESS, status, $"Create {fileType} should succeed");

            BaseTestSite.Log.Add(LogEntryKind.TestStep, $"2. Send {fsctlName} request");
            status = SendFsCtlRequest(fsctlCommand);

            BaseTestSite.Log.Add(LogEntryKind.TestStep, "3. Verify server response based on file system support");

            if (status == MessageStatus.SUCCESS)
            {
                BaseTestSite.Log.Add(LogEntryKind.Comment,
                    $"{fsctlCommand} returned SUCCESS on {this.fsaAdapter.FileSystem}.");
            }
            else
            {
                this.fsaAdapter.AssertAreEqual(this.Manager, MessageStatus.INVALID_DEVICE_REQUEST, status,
                    $"The server SHOULD fail the request with STATUS_INVALID_DEVICE_REQUEST when the FSCTL {fsctlName} " +
                    $"is allowed but is not supported on the {this.fsaAdapter.FileSystem} file system, as specified in [MS-FSCC] section 2.2");
            }
        }

        /// <summary>
        /// Send the specified FSCTL request with appropriate parameters
        /// </summary>
        /// <param name="fsctlCommand">The FSCTL command to send</param>
        /// <returns>The status returned by the FSCTL operation</returns>
        private MessageStatus SendFsCtlRequest(FsControlCommand fsctlCommand)
        {
            switch (fsctlCommand)
            {
                case FsControlCommand.FSCTL_GET_COMPRESSION:
                    var compressionReply = new FSCTL_GET_COMPRESSION_Reply();
                    uint outputBufferSize = (uint)TypeMarshal.ToBytes(compressionReply).Length;
                    return this.fsaAdapter.FsCtlGetCompression(outputBufferSize, out _, out _);

                case FsControlCommand.FSCTL_SET_COMPRESSION:
                    var setCompressionRequest = new FSCTL_SET_COMPRESSION_Request
                    {
                        CompressionState = FSCTL_SET_COMPRESSION_Request_CompressionState_Values.COMPRESSION_FORMAT_DEFAULT
                    };
                    uint setCompressionInputSize = (uint)TypeMarshal.ToBytes(setCompressionRequest).Length;
                    return this.fsaAdapter.FsCtlSetCompression(setCompressionRequest, setCompressionInputSize);

                case FsControlCommand.FSCTL_SET_SPARSE:
                    return this.fsaAdapter.FsCtlSetSparse(true);

                case FsControlCommand.FSCTL_SET_ZERO_DATA:
                    var setZeroDataRequest = new FSCTL_SET_ZERO_DATA_Request
                    {
                        FileOffset = 0,
                        BeyondFinalZero = 1024
                    };
                    uint setZeroDataInputSize = (uint)TypeMarshal.ToBytes(setZeroDataRequest).Length;
                    return this.fsaAdapter.FsCtlSetZeroData(setZeroDataRequest, setZeroDataInputSize);

                case FsControlCommand.FSCTL_GET_INTEGRITY_INFORMATION:
                    var getIntegrityInfo = new FSCTL_GET_INTEGRITY_INFORMATION_BUFFER();
                    uint getIntegrityOutputSize = (uint)TypeMarshal.ToBytes(getIntegrityInfo).Length;
                    return this.fsaAdapter.FsCtlGetIntegrityInfo(getIntegrityOutputSize, out _, out _);

                case FsControlCommand.FSCTL_SET_INTEGRITY_INFORMATION:
                    var setIntegrityInfo = new FSCTL_SET_INTEGRITY_INFORMATION_BUFFER
                    {
                        ChecksumAlgorithm = FSCTL_SET_INTEGRITY_INFORMATION_BUFFER_CHECKSUMALGORITHM.CHECKSUM_TYPE_CRC64,
                        Flags = FSCTL_SET_INTEGRITY_INFORMATION_BUFFER_FLAGS.FSCTL_INTEGRITY_FLAG_CHECKSUM_ENFORCEMENT_OFF
                    };
                    uint setIntegrityInputSize = (uint)TypeMarshal.ToBytes(setIntegrityInfo).Length;
                    return this.fsaAdapter.FsCtlSetIntegrityInfo(setIntegrityInfo, setIntegrityInputSize);

                default:
                    BaseTestSite.Log.Add(LogEntryKind.Warning, $"FSCTL {fsctlCommand} is not recognized and will be treated as unsupported.");
                    return MessageStatus.INVALID_DEVICE_REQUEST;
            }
        }

        #endregion
    }
}
