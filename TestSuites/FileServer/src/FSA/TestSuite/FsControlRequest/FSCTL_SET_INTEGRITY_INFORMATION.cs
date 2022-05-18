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

        #region FsCtl_Set_IntegrityInformation_IsSupported

        [TestMethod()]
        [TestCategory(TestCategories.Bvt)]
        [TestCategory(TestCategories.Fsa)]
        [TestCategory(TestCategories.IoCtlRequest)]
        [TestCategory(TestCategories.NonSmb)]
        [Description("Send FSCTL_SET_INTEGRITY_INFORMATION request to a file and check if Integrity is supported.")]
        public void FsCtl_Set_IntegrityInformation_File_IsIntegritySupported()
        {
            FsCtl_Set_IntegrityInformation_IsIntegritySupported(FileType.DataFile);
        }

        [TestMethod()]
        [TestCategory(TestCategories.Bvt)]
        [TestCategory(TestCategories.Fsa)]
        [TestCategory(TestCategories.IoCtlRequest)]
        [TestCategory(TestCategories.NonSmb)]
        [Description("Send FSCTL_SET_INTEGRITY_INFORMATION request to a directory and check if Integrity is supported.")]
        public void FsCtl_Set_IntegrityInformation_Dir_IsIntegritySupported()
        {
            FsCtl_Set_IntegrityInformation_IsIntegritySupported(FileType.DirectoryFile);
        }

        #endregion

        #region FsCtl_Set_IntegrityInformation_InputBufferSizeLessThanIntegrityBuffer

        [TestMethod()]
        [TestCategory(TestCategories.Fsa)]
        [TestCategory(TestCategories.IoCtlRequest)]
        [TestCategory(TestCategories.NonSmb)]
        [TestCategory(TestCategories.Positive)]
        [Description("Send FSCTL_SET_INTEGRITY_INFORMATION request to a file and check if server responds correctly when the input buffer length is less than the size, in bytes, of the FSCTL_SET_INTEGRITY_INFORMATION_BUFFER element.")]
        public void FsCtl_Set_IntegrityInformation_File_InputBufferSizeLessThanIntegrityBuffer()
        {
            FsCtl_Set_IntegrityInformation_InputBufferSizeLessThanIntegrityBuffer(FileType.DataFile);
        }

        [TestMethod()]
        [TestCategory(TestCategories.Fsa)]
        [TestCategory(TestCategories.IoCtlRequest)]
        [TestCategory(TestCategories.NonSmb)]
        [TestCategory(TestCategories.Positive)]
        [Description("Send FSCTL_SET_INTEGRITY_INFORMATION request to a directory and check if server responds correctly when the input buffer length is less than the size, in bytes, of the FSCTL_SET_INTEGRITY_INFORMATION_BUFFER element.")]
        public void FsCtl_Set_IntegrityInformation_Dir_InputBufferSizeLessThanIntegrityBuffer()
        {
            FsCtl_Set_IntegrityInformation_InputBufferSizeLessThanIntegrityBuffer(FileType.DirectoryFile);
        }

        #endregion

        #region FsCtl_Set_IntegrityInformation_InvalidParameter_NonEmptyFile

        [TestMethod()]
        [TestCategory(TestCategories.Fsa)]
        [TestCategory(TestCategories.IoCtlRequest)]
        [TestCategory(TestCategories.NonSmb)]
        [TestCategory(TestCategories.Positive)]
        [Description("Send FSCTL_SET_INTEGRITY_INFORMATION request to a file with invalid parameter and check if server responses correctly.")]
        public void FsCtl_Set_IntegrityInformation_InvalidParameter_NonEmptyFile()
        {
            BaseTestSite.Log.Add(LogEntryKind.TestStep, "Test case steps:");
            MessageStatus status;

            //Step 1: Create file
            BaseTestSite.Log.Add(LogEntryKind.TestStep, "1. Create DataFile");
            status = fsaAdapter.CreateFile(FileType.DataFile);
            fsaAdapter.AssertAreEqual(Manager, MessageStatus.SUCCESS, status, "Failed to create the file.");

            //Step 2: FSCTL request FSCTL_SET_INTEGRITY_INFORMATION
            FSCTL_SET_INTEGRITY_INFORMATION_BUFFER integrityInfo = new()
            {
                ChecksumAlgorithm = FSCTL_SET_INTEGRITY_INFORMATION_BUFFER_CHECKSUMALGORITHM.CHECKSUM_TYPE_CRC64,
                Flags = FSCTL_SET_INTEGRITY_INFORMATION_BUFFER_FLAGS.FSCTL_INTEGRITY_FLAG_CHECKSUM_ENFORCEMENT_OFF
            };
            uint inputBufferSize = (uint)TypeMarshal.ToBytes(integrityInfo).Length;

            BaseTestSite.Log.Add(LogEntryKind.TestStep, "2. FSCTL request FSCTL_SET_INTEGRITY_INFORMATION.");
            status = fsaAdapter.FsCtlSetIntegrityInfo(integrityInfo, inputBufferSize);

            // Check if Integrity is supported
            if (!IsCurrentTransportSupportIntegrity(status)) return;
            if (fsaAdapter.IsIntegritySupported == false)
            {
                fsaAdapter.AssertAreEqual(Manager, MessageStatus.INVALID_DEVICE_REQUEST, status, "If the object store does not implement this functionality, the operation MUST be failed with STATUS_INVALID_DEVICE_REQUEST.");
                return;
            }

            //Step 3: FSCTL request FSCTL_GET_INTEGRITY_INFORMATION
            FSCTL_GET_INTEGRITY_INFORMATION_BUFFER getIntegrityInfo = new();
            uint outputBufferSize = (uint)TypeMarshal.ToBytes(getIntegrityInfo).Length;

            BaseTestSite.Log.Add(LogEntryKind.TestStep, "3. FSCTL request FSCTL_GET_INTEGRITY_INFORMATION.");
            status = fsaAdapter.FsCtlGetIntegrityInfo(outputBufferSize, out _, out byte[] outputBuffer);
            fsaAdapter.AssertAreEqual(Manager, MessageStatus.SUCCESS, status, "Failed to get integrity info of the file.");

            //Step 4: Verify ChecksumAlgorithm is correctly set
            //NOTE: on ReFS v1, only CRC64 is applicable; on ReFS v2 the actual checksum configured will not depend on the request parameter; it's CRC32 if cluster size is 4KB and CRC64 if cluster size is 64KB.
            getIntegrityInfo = TypeMarshal.ToStruct<FSCTL_GET_INTEGRITY_INFORMATION_BUFFER>(outputBuffer);
            if (fsaAdapter.ReFSVersion == 1)
            {
                bool isChecksumTypeCRC64 = (getIntegrityInfo.ChecksumAlgorithm == FSCTL_GET_INTEGRITY_INFORMATION_BUFFER_CHECKSUMALGORITHM.CHECKSUM_TYPE_CRC64);
                BaseTestSite.Log.Add(LogEntryKind.TestStep, "4. Verify ChecksumAlgorithm is correctly set.");
                fsaAdapter.AssertAreEqual(Manager, true, isChecksumTypeCRC64, "ChecksumAlgorithm should be CHECKSUM_TYPE_CRC64.");
            }
            else
            {
                bool isChecksumSet = (getIntegrityInfo.ChecksumAlgorithm != FSCTL_GET_INTEGRITY_INFORMATION_BUFFER_CHECKSUMALGORITHM.CHECKSUM_TYPE_NONE);
                BaseTestSite.Log.Add(LogEntryKind.TestStep, "4. Verify ChecksumAlgorithm is correctly set.");
                fsaAdapter.AssertAreEqual(Manager, true, isChecksumSet, "ChecksumAlgorithm should not be CHECKSUM_TYPE_NONE.");
            }
            BaseTestSite.Log.Add(LogEntryKind.TestStep, "5. Write some data so that the file is not empty.");

            //Step 5: Write some data so that the file is not empty
            status = fsaAdapter.WriteFile(0, 10240, out _);
            fsaAdapter.AssertAreEqual(Manager, MessageStatus.SUCCESS, status, "Failed to write to the file.");

            //Step 6: FSCTL request FSCTL_SET_INTEGRITY_INFORMATION
            integrityInfo.ChecksumAlgorithm = FSCTL_SET_INTEGRITY_INFORMATION_BUFFER_CHECKSUMALGORITHM.CHECKSUM_TYPE_NONE;
            integrityInfo.Flags = FSCTL_SET_INTEGRITY_INFORMATION_BUFFER_FLAGS.NONE;
            BaseTestSite.Log.Add(LogEntryKind.TestStep, "6. FSCTL request FSCTL_SET_INTEGRITY_INFORMATION.");
            status = fsaAdapter.FsCtlSetIntegrityInfo(integrityInfo, inputBufferSize);

            //Step 7: Verify test result
            BaseTestSite.Log.Add(LogEntryKind.TestStep, "7. Verify returned NTStatus code.");
            fsaAdapter.AssertAreEqual(Manager, MessageStatus.SUCCESS, status,
                "FSCTL_SET_INTEGRITY_INFORMATION request should succeed when change the checksum state of a non-empty file.");

        }

        #endregion

        #region FsCtl_Set_IntegrityInformation_UndefinedChecksumAlgorithm

        [TestMethod()]
        [TestCategory(TestCategories.Fsa)]
        [TestCategory(TestCategories.IoCtlRequest)]
        [TestCategory(TestCategories.NonSmb)]
        [TestCategory(TestCategories.Positive)]
        [Description("Send FSCTL_SET_INTEGRITY_INFORMATION request to a file with Undefined Checksum Algorithm and check if server responses correctly.")]
        public void FsCtl_Set_IntegrityInformation_File_UndefinedChecksumAlgorithm()
        {
            FsCtl_Set_IntegrityInformation_UndefinedChecksumAlgorithm(FileType.DataFile);
        }

        [TestMethod()]
        [TestCategory(TestCategories.Fsa)]
        [TestCategory(TestCategories.IoCtlRequest)]
        [TestCategory(TestCategories.NonSmb)]
        [TestCategory(TestCategories.Positive)]
        [Description("Send FSCTL_SET_INTEGRITY_INFORMATION request to a directory with Undefined Checksum Algorithm and check if server responses correctly.")]
        public void FsCtl_Set_IntegrityInformation_Dir_UndefinedChecksumAlgorithm()
        {
            FsCtl_Set_IntegrityInformation_UndefinedChecksumAlgorithm(FileType.DirectoryFile);
        }

        #endregion

        #region FsCtl_Set_IntegrityInformation_WriteProtected

        [TestMethod()]
        [TestCategory(TestCategories.Fsa)]
        [TestCategory(TestCategories.IoCtlRequest)]
        [TestCategory(TestCategories.NonSmb)]
        [TestCategory(TestCategories.Positive)]
        [Description("Send FSCTL_SET_INTEGRITY_INFORMATION request to a file which is write protected and check if server responses correctly.")]
        public void FsCtl_Set_IntegrityInformation_File_WriteProtected()
        {
            FsCtl_Set_IntegrityInformation_WriteProtected(FileType.DataFile);
        }

        [TestMethod()]
        [TestCategory(TestCategories.Fsa)]
        [TestCategory(TestCategories.IoCtlRequest)]
        [TestCategory(TestCategories.NonSmb)]
        [TestCategory(TestCategories.Positive)]
        [Description("Send FSCTL_SET_INTEGRITY_INFORMATION request to a directory which is write protected and check if server responses correctly.")]
        public void FsCtl_Set_IntegrityInformation_Dir_WriteProtected()
        {
            FsCtl_Set_IntegrityInformation_WriteProtected(FileType.DirectoryFile);
        }

        #endregion

        #region FsCtl_Set_IntegrityInformation_ChecksumTypeNoneAndUnchanged

        [TestMethod()]
        [TestCategory(TestCategories.Fsa)]
        [TestCategory(TestCategories.IoCtlRequest)]
        [TestCategory(TestCategories.NonSmb)]
        [TestCategory(TestCategories.Positive)]
        [Description("Send FSCTL_SET_INTEGRITY_INFORMATION request to a file with checksum type CHECKSUM_TYPE_NONE, then send another request with CHECKSUM_TYPE_UNCHANGED, check if server responses correctly.")]
        public void FsCtl_Set_IntegrityInformation_File_ChecksumTypeNoneAndUnchanged()
        {
            FsCtl_Set_IntegrityInformation_ChecksumTypeNoneAndUnchanged(FileType.DataFile);
        }

        [TestMethod()]
        [TestCategory(TestCategories.Fsa)]
        [TestCategory(TestCategories.IoCtlRequest)]
        [TestCategory(TestCategories.NonSmb)]
        [TestCategory(TestCategories.Positive)]
        [Description("Send FSCTL_SET_INTEGRITY_INFORMATION request to a directory with checksum type CHECKSUM_TYPE_NONE, then send another request with CHECKSUM_TYPE_UNCHANGED, check if server responses correctly.")]
        public void FsCtl_Set_IntegrityInformation_Dir_ChecksumTypeNoneAndUnchanged()
        {
            FsCtl_Set_IntegrityInformation_ChecksumTypeNoneAndUnchanged(FileType.DirectoryFile);
        }

        #endregion

        #region FsCtl_Set_IntegrityInformation_ChecksumTypeCrc64AndUnchanged

        [TestMethod()]
        [TestCategory(TestCategories.Fsa)]
        [TestCategory(TestCategories.IoCtlRequest)]
        [TestCategory(TestCategories.NonSmb)]
        [TestCategory(TestCategories.Positive)]
        [Description("Send FSCTL_SET_INTEGRITY_INFORMATION request to a file with checksum type CHECKSUM_TYPE_CRC64, then send another request with CHECKSUM_TYPE_UNCHANGED, check if server responses correctly.")]
        public void FsCtl_Set_IntegrityInformation_File_ChecksumTypeCrc64AndUnchanged()
        {
            FsCtl_Set_IntegrityInformation_ChecksumTypeCrc64AndUnchanged(FileType.DataFile);
        }

        [TestMethod()]
        [TestCategory(TestCategories.Fsa)]
        [TestCategory(TestCategories.IoCtlRequest)]
        [TestCategory(TestCategories.NonSmb)]
        [TestCategory(TestCategories.Positive)]
        [Description("Send FSCTL_SET_INTEGRITY_INFORMATION request to a directory with checksum type CHECKSUM_TYPE_CRC64, then send another request with CHECKSUM_TYPE_UNCHANGED, check if server responses correctly.")]
        public void FsCtl_Set_IntegrityInformation_Dir_ChecksumTypeCrc64AndUnchanged()
        {
            FsCtl_Set_IntegrityInformation_ChecksumTypeCrc64AndUnchanged(FileType.DirectoryFile);
        }

        #endregion

        #endregion

        #region Test Case Utility

        private void FsCtl_Set_IntegrityInformation_IsIntegritySupported(FileType fileType)
        {
            BaseTestSite.Log.Add(LogEntryKind.TestStep, "Test case steps:");
            MessageStatus status;

            //Step 1: Create file
            BaseTestSite.Log.Add(LogEntryKind.TestStep, "1. Create " + fileType.ToString());
            status = fsaAdapter.CreateFile(fileType);
            fsaAdapter.AssertAreEqual(Manager, MessageStatus.SUCCESS, status, "Failed to create the file.");

            //Step 2: FSCTL request FSCTL_SET_INTEGRITY_INFORMATION
            FSCTL_SET_INTEGRITY_INFORMATION_BUFFER integrityInfo = new()
            {
                ChecksumAlgorithm = FSCTL_SET_INTEGRITY_INFORMATION_BUFFER_CHECKSUMALGORITHM.CHECKSUM_TYPE_CRC64,
                Flags = FSCTL_SET_INTEGRITY_INFORMATION_BUFFER_FLAGS.FSCTL_INTEGRITY_FLAG_CHECKSUM_ENFORCEMENT_OFF
            };

            uint inputBufferSize = (uint)TypeMarshal.ToBytes(integrityInfo).Length;

            BaseTestSite.Log.Add(LogEntryKind.TestStep, "2. FSCTL request FSCTL_SET_INTEGRITY_INFORMATION.");
            status = fsaAdapter.FsCtlSetIntegrityInfo(integrityInfo, inputBufferSize);

            //Step 3: Verify test result
            BaseTestSite.Log.Add(LogEntryKind.TestStep, "3. Verify returned NTStatus code.");
            if (!IsCurrentTransportSupportIntegrity(status)) return;

            if (fsaAdapter.IsIntegritySupported == false)
            {
                fsaAdapter.AssertAreEqual(Manager, MessageStatus.INVALID_DEVICE_REQUEST, status, "If the object store does not implement this functionality, the operation MUST be failed with STATUS_INVALID_DEVICE_REQUEST.");
            }
            else
            {
                fsaAdapter.AssertAreEqual(Manager, MessageStatus.SUCCESS, status, "Integrity is supported, status set to STATUS_SUCCESS.");
            }
        }

        private void FsCtl_Set_IntegrityInformation_InputBufferSizeLessThanIntegrityBuffer(FileType fileType)
        {
            BaseTestSite.Log.Add(LogEntryKind.TestStep, "Test case steps:");
            MessageStatus status;

            //Step 1: Create file
            BaseTestSite.Log.Add(LogEntryKind.TestStep, "1. Create " + fileType.ToString());
            status = fsaAdapter.CreateFile(fileType);
            fsaAdapter.AssertAreEqual(Manager, MessageStatus.SUCCESS, status, "Failed to create the file.");

            //Step 2: FSCTL request FSCTL_SET_INTEGRITY_INFORMATION
            FSCTL_SET_INTEGRITY_INFORMATION_BUFFER integrityInfo = new()
            {
                ChecksumAlgorithm = FSCTL_SET_INTEGRITY_INFORMATION_BUFFER_CHECKSUMALGORITHM.CHECKSUM_TYPE_CRC64,
                Flags = FSCTL_SET_INTEGRITY_INFORMATION_BUFFER_FLAGS.FSCTL_INTEGRITY_FLAG_CHECKSUM_ENFORCEMENT_OFF
            };
            uint inputBufferSize = (uint)TypeMarshal.ToBytes(integrityInfo).Length - 1;

            BaseTestSite.Log.Add(LogEntryKind.TestStep, "2. FSCTL request FSCTL_SET_INTEGRITY_INFORMATION.");
            status = fsaAdapter.FsCtlSetIntegrityInfo(integrityInfo, inputBufferSize);

            //Step 3: Verify the test result
            BaseTestSite.Log.Add(LogEntryKind.TestStep, "3. Verify returned NTStatus code.");

            if (!IsCurrentTransportSupportIntegrity(status)) return;

            if (fsaAdapter.IsIntegritySupported == false)
            {
                fsaAdapter.AssertAreEqual(Manager, MessageStatus.INVALID_DEVICE_REQUEST, status, "If the object store does not implement this functionality, the operation MUST be failed with STATUS_INVALID_DEVICE_REQUEST.");
            }
            else
            {
                fsaAdapter.AssertAreEqual(Manager, MessageStatus.INVALID_PARAMETER, status,
                    "The operation MUST be failed with STATUS_INVALID_PARAMETER if InputBufferSize is less than sizeof(FILE_INTEGRITY_STREAM_INFORMATION).");
            }
        }

        private void FsCtl_Set_IntegrityInformation_UndefinedChecksumAlgorithm(FileType fileType)
        {
            BaseTestSite.Log.Add(LogEntryKind.TestStep, "Test case steps:");
            MessageStatus status;

            //Step 1: Create file
            BaseTestSite.Log.Add(LogEntryKind.TestStep, "1. Create " + fileType.ToString());
            status = fsaAdapter.CreateFile(fileType);
            fsaAdapter.AssertAreEqual(Manager, MessageStatus.SUCCESS, status, "Failed to create the file.");

            //Step 2: FSCTL request FSCTL_SET_INTEGRITY_INFORMATION
            FSCTL_SET_INTEGRITY_INFORMATION_BUFFER integrityInfo = new()
            {
                ChecksumAlgorithm = (FSCTL_SET_INTEGRITY_INFORMATION_BUFFER_CHECKSUMALGORITHM)0x0003,
                Flags = FSCTL_SET_INTEGRITY_INFORMATION_BUFFER_FLAGS.FSCTL_INTEGRITY_FLAG_CHECKSUM_ENFORCEMENT_OFF
            };

            uint inputBufferSize = (uint)TypeMarshal.ToBytes(integrityInfo).Length;

            BaseTestSite.Log.Add(LogEntryKind.TestStep, "2. FSCTL request FSCTL_SET_INTEGRITY_INFORMATION with undefined checksum algorithm 0x003.");
            status = fsaAdapter.FsCtlSetIntegrityInfo(integrityInfo, inputBufferSize);

            //Step 3: Verify test result
            BaseTestSite.Log.Add(LogEntryKind.TestStep, "3. Verify returned NTStatus code.");
            if (!IsCurrentTransportSupportIntegrity(status)) return;

            if (fsaAdapter.IsIntegritySupported == false)
            {
                fsaAdapter.AssertAreEqual(Manager, MessageStatus.INVALID_DEVICE_REQUEST, status, "If the object store does not implement this functionality, the operation MUST be failed with STATUS_INVALID_DEVICE_REQUEST.");
            }
            else
            {
                if (fsaAdapter.ReFSVersion == 2)
                {
                    fsaAdapter.AssertAreEqual(Manager, MessageStatus.SUCCESS, status,
                        "[MS-FSCC] section 2.3.57: for ReFS v2 any value except CHECKSUM_TYPE_NONE or CHECKSUM_TYPE_UNCHANGED will set the integrity value to a file-system-selected integrity mechanism and is not guaranteed to use the user specified checksum value.");

                }
                else
                {
                    fsaAdapter.AssertAreEqual(Manager, MessageStatus.INVALID_PARAMETER, status,
                        "The operation MUST be failed with STATUS_INVALID_PARAMETER if InputBuffer.ChecksumAlgorithm is not one of the predefined values in [MS-FSCC] section 2.3.51.");
                }
            }
        }

        private void FsCtl_Set_IntegrityInformation_WriteProtected(FileType fileType)
        {
            BaseTestSite.Log.Add(LogEntryKind.TestStep, "Test case steps:");
            MessageStatus status;

            //Step 1: Open file
            string fileToOpen = fileType == FileType.DataFile ? "ExistingFile.txt" : "ExistingFolder";
            CreateOptions fileCreateOption = fileType == FileType.DataFile ? CreateOptions.NON_DIRECTORY_FILE : CreateOptions.DIRECTORY_FILE;

            BaseTestSite.Log.Add(LogEntryKind.TestStep, "1. Open " + fileToOpen);
            status = fsaAdapter.CreateFile(
                        fileToOpen,
                        FileAttribute.NORMAL,
                        fileCreateOption,
                        FileAccess.GENERIC_READ | FileAccess.GENERIC_WRITE,
                        ShareAccess.FILE_SHARE_READ | ShareAccess.FILE_SHARE_WRITE,
                        CreateDisposition.OPEN);

            string comment = string.Format("Open {0} is expected to success.", fileToOpen);
            fsaAdapter.AssertAreEqual(Manager, MessageStatus.SUCCESS, status, comment);

            //Step 2: FSCTL request FSCTL_SET_INTEGRITY_INFORMATION
            FSCTL_SET_INTEGRITY_INFORMATION_BUFFER integrityInfo = new()
            {
                ChecksumAlgorithm = FSCTL_SET_INTEGRITY_INFORMATION_BUFFER_CHECKSUMALGORITHM.CHECKSUM_TYPE_CRC64
            };

            uint inputBufferSize = (uint)TypeMarshal.ToBytes(integrityInfo).Length;

            BaseTestSite.Log.Add(LogEntryKind.TestStep, "2. FSCTL request FSCTL_SET_INTEGRITY_INFORMATION.");
            status = fsaAdapter.FsCtlSetIntegrityInfo(integrityInfo, inputBufferSize);

            //Step 3: Verify test result
            BaseTestSite.Log.Add(LogEntryKind.TestStep, "3. Verify returned NTStatus code.");

            if (!IsCurrentTransportSupportIntegrity(status)) return;
            if (fsaAdapter.IsIntegritySupported == false)
            {
                fsaAdapter.AssertAreEqual(Manager, MessageStatus.INVALID_DEVICE_REQUEST, status, "If the object store does not implement this functionality, the operation MUST be failed with STATUS_INVALID_DEVICE_REQUEST.");
            }
            else
            {
                if (fsaAdapter.IsVolumeReadonly == true)
                {
                    fsaAdapter.AssertAreEqual(Manager, MessageStatus.MEDIA_WRITE_PROTECTED, status,
                        "If Open.File.Volume.IsReadOnly is TRUE, the operation MUST be failed with STATUS_MEDIA_WRITE_PROTECTED.");
                }
                else
                {
                    fsaAdapter.AssertAreEqual(Manager, MessageStatus.SUCCESS, status, "Status set to STATUS_SUCCESS.");
                }
            }
        }

        private void FsCtl_Set_IntegrityInformation_ChecksumTypeNoneAndUnchanged(FileType fileType)
        {
            BaseTestSite.Log.Add(LogEntryKind.TestStep, "Test case steps:");
            MessageStatus status;

            //Step 1: Create file
            BaseTestSite.Log.Add(LogEntryKind.TestStep, "1. Create " + fileType.ToString());
            status = fsaAdapter.CreateFile(fileType);
            fsaAdapter.AssertAreEqual(Manager, MessageStatus.SUCCESS, status, "Failed to create the file.");

            //Step 2: FSCTL request FSCTL_SET_INTEGRITY_INFORMATION with CHECKSUM_TYPE_NONE
            FSCTL_SET_INTEGRITY_INFORMATION_BUFFER integrityInfo = new()
            {
                ChecksumAlgorithm = FSCTL_SET_INTEGRITY_INFORMATION_BUFFER_CHECKSUMALGORITHM.CHECKSUM_TYPE_NONE
            };
            uint inputBufferSize = (uint)TypeMarshal.ToBytes(integrityInfo).Length;

            BaseTestSite.Log.Add(LogEntryKind.TestStep, "2. FSCTL request FSCTL_SET_INTEGRITY_INFORMATION with CHECKSUM_TYPE_NONE.");
            status = fsaAdapter.FsCtlSetIntegrityInfo(integrityInfo, inputBufferSize);

            // Check if Integrity is supported
            if (!IsCurrentTransportSupportIntegrity(status)) return;
            if (fsaAdapter.IsIntegritySupported == false)
            {
                fsaAdapter.AssertAreEqual(Manager, MessageStatus.INVALID_DEVICE_REQUEST, status, "If the object store does not implement this functionality, the operation MUST be failed with STATUS_INVALID_DEVICE_REQUEST.");
                return;
            }

            //Step 3: FSCTL request FSCTL_GET_INTEGRITY_INFORMATION
            FSCTL_GET_INTEGRITY_INFORMATION_BUFFER getIntegrityInfo = new();
            uint outputBufferSize = (uint)TypeMarshal.ToBytes(getIntegrityInfo).Length;

            BaseTestSite.Log.Add(LogEntryKind.TestStep, "3. FSCTL request FSCTL_GET_INTEGRITY_INFORMATION.");
            status = fsaAdapter.FsCtlGetIntegrityInfo(outputBufferSize, out _, out byte[] outputBuffer);
            fsaAdapter.AssertAreEqual(Manager, MessageStatus.SUCCESS, status, "Failed to get integrity info of the file.");

            //Step 4: Verify ChecksumAlgorithm
            getIntegrityInfo = TypeMarshal.ToStruct<FSCTL_GET_INTEGRITY_INFORMATION_BUFFER>(outputBuffer);

            bool isChecksumTypeNone = (getIntegrityInfo.ChecksumAlgorithm == FSCTL_GET_INTEGRITY_INFORMATION_BUFFER_CHECKSUMALGORITHM.CHECKSUM_TYPE_NONE);
            BaseTestSite.Log.Add(LogEntryKind.TestStep, "4. Verify ChecksumAlgorithm.");
            fsaAdapter.AssertAreEqual(Manager, true, isChecksumTypeNone, "ChecksumAlgorithm is CHECKSUM_TYPE_NONE.");

            //Step 5: FSCTL request FSCTL_SET_INTEGRITY_INFORMATION
            integrityInfo.ChecksumAlgorithm = FSCTL_SET_INTEGRITY_INFORMATION_BUFFER_CHECKSUMALGORITHM.CHECKSUM_TYPE_UNCHANGED;
            inputBufferSize = (uint)TypeMarshal.ToBytes(integrityInfo).Length;

            BaseTestSite.Log.Add(LogEntryKind.TestStep, "5. FSCTL request FSCTL_SET_INTEGRITY_INFORMATION with CHECKSUM_TYPE_UNCHANGED.");
            status = fsaAdapter.FsCtlSetIntegrityInfo(integrityInfo, inputBufferSize);

            // Check if Integrity is supported
            if (!IsCurrentTransportSupportIntegrity(status)) return;
            if (fsaAdapter.IsIntegritySupported == false)
            {
                fsaAdapter.AssertAreEqual(Manager, MessageStatus.INVALID_DEVICE_REQUEST, status, "If the object store does not implement this functionality, the operation MUST be failed with STATUS_INVALID_DEVICE_REQUEST.");
                return;
            }

            //Step 6: FSCTL request FSCTL_GET_INTEGRITY_INFORMATION
            BaseTestSite.Log.Add(LogEntryKind.TestStep, "6. FSCTL request FSCTL_GET_INTEGRITY_INFORMATION.");
            status = fsaAdapter.FsCtlGetIntegrityInfo(outputBufferSize, out _, out outputBuffer);
            fsaAdapter.AssertAreEqual(Manager, MessageStatus.SUCCESS, status, "Failed to get integrity info of the file.");

            //Step 7: Verify ChecksumAlgorithm
            getIntegrityInfo = TypeMarshal.ToStruct<FSCTL_GET_INTEGRITY_INFORMATION_BUFFER>(outputBuffer);
            isChecksumTypeNone = (getIntegrityInfo.ChecksumAlgorithm == FSCTL_GET_INTEGRITY_INFORMATION_BUFFER_CHECKSUMALGORITHM.CHECKSUM_TYPE_NONE);
            BaseTestSite.Log.Add(LogEntryKind.TestStep, "7. Verify ChecksumAlgorithm.");
            fsaAdapter.AssertAreEqual(Manager, true, isChecksumTypeNone, "ChecksumAlgorithm is CHECKSUM_TYPE_NONE.");

        }

        private void FsCtl_Set_IntegrityInformation_ChecksumTypeCrc64AndUnchanged(FileType fileType)
        {
            BaseTestSite.Log.Add(LogEntryKind.TestStep, "Test case steps:");
            MessageStatus status;

            //Step 1: Create file
            BaseTestSite.Log.Add(LogEntryKind.TestStep, "1. Create " + fileType.ToString());
            status = fsaAdapter.CreateFile(fileType);
            fsaAdapter.AssertAreEqual(Manager, MessageStatus.SUCCESS, status, "Failed to create the file.");

            //Step 2: FSCTL request FSCTL_SET_INTEGRITY_INFORMATION
            FSCTL_SET_INTEGRITY_INFORMATION_BUFFER integrityInfo = new()
            {
                ChecksumAlgorithm = FSCTL_SET_INTEGRITY_INFORMATION_BUFFER_CHECKSUMALGORITHM.CHECKSUM_TYPE_CRC64
            };
            uint inputBufferSize = (uint)TypeMarshal.ToBytes(integrityInfo).Length;

            BaseTestSite.Log.Add(LogEntryKind.TestStep, "2. FSCTL request FSCTL_SET_INTEGRITY_INFORMATION with CHECKSUM_TYPE_CRC64.");
            status = fsaAdapter.FsCtlSetIntegrityInfo(integrityInfo, inputBufferSize);

            // Check if Integrity is supported
            if (!IsCurrentTransportSupportIntegrity(status)) return;
            if (fsaAdapter.IsIntegritySupported == false)
            {
                fsaAdapter.AssertAreEqual(Manager, MessageStatus.INVALID_DEVICE_REQUEST, status, "If the object store does not implement this functionality, the operation MUST be failed with STATUS_INVALID_DEVICE_REQUEST.");
                return;
            }

            //Step 3: FSCTL request FSCTL_GET_INTEGRITY_INFORMATION
            FSCTL_GET_INTEGRITY_INFORMATION_BUFFER getIntegrityInfo = new();
            uint outputBufferSize = (uint)TypeMarshal.ToBytes(getIntegrityInfo).Length;

            BaseTestSite.Log.Add(LogEntryKind.TestStep, "3. FSCTL request FSCTL_GET_INTEGRITY_INFORMATION.");
            status = fsaAdapter.FsCtlGetIntegrityInfo(outputBufferSize, out _, out byte[] outputBuffer);
            fsaAdapter.AssertAreEqual(Manager, MessageStatus.SUCCESS, status, "Failed to get integrity info of the file.");

            //Step 4: Verify ChecksumAlgorithm
            //NOTE: on ReFS v1, only CRC64 is applicable; on ReFS v2 the actual checksum configured will not depend on the request parameter; it's CRC32 if cluster size is 4KB and CRC64 if cluster size is 64KB.
            getIntegrityInfo = TypeMarshal.ToStruct<FSCTL_GET_INTEGRITY_INFORMATION_BUFFER>(outputBuffer);
            if (fsaAdapter.ReFSVersion == 1)
            {
                bool isChecksumTypeCRC64 = (getIntegrityInfo.ChecksumAlgorithm == FSCTL_GET_INTEGRITY_INFORMATION_BUFFER_CHECKSUMALGORITHM.CHECKSUM_TYPE_CRC64);
                BaseTestSite.Log.Add(LogEntryKind.TestStep, "4. Verify ChecksumAlgorithm is correctly set.");
                fsaAdapter.AssertAreEqual(Manager, true, isChecksumTypeCRC64, "ChecksumAlgorithm should be CHECKSUM_TYPE_CRC64.");
            }
            else
            {
                bool isChecksumSet = (getIntegrityInfo.ChecksumAlgorithm != FSCTL_GET_INTEGRITY_INFORMATION_BUFFER_CHECKSUMALGORITHM.CHECKSUM_TYPE_NONE);
                BaseTestSite.Log.Add(LogEntryKind.TestStep, "4. Verify ChecksumAlgorithm is correctly set.");
                fsaAdapter.AssertAreEqual(Manager, true, isChecksumSet, "ChecksumAlgorithm should not be CHECKSUM_TYPE_NONE.");
            }

            //Step 5: FSCTL request FSCTL_SET_INTEGRITY_INFORMATION
            integrityInfo.ChecksumAlgorithm = FSCTL_SET_INTEGRITY_INFORMATION_BUFFER_CHECKSUMALGORITHM.CHECKSUM_TYPE_UNCHANGED;
            inputBufferSize = (uint)TypeMarshal.ToBytes(integrityInfo).Length;

            BaseTestSite.Log.Add(LogEntryKind.TestStep, "5. FSCTL request FSCTL_SET_INTEGRITY_INFORMATION with CHECKSUM_TYPE_UNCHANGED.");
            status = fsaAdapter.FsCtlSetIntegrityInfo(integrityInfo, inputBufferSize);

            // Check if Integrity is supported
            if (!IsCurrentTransportSupportIntegrity(status)) return;
            if (fsaAdapter.IsIntegritySupported == false)
            {
                fsaAdapter.AssertAreEqual(Manager, MessageStatus.INVALID_DEVICE_REQUEST, status, "If the object store does not implement this functionality, the operation MUST be failed with STATUS_INVALID_DEVICE_REQUEST.");
                return;
            }

            //Step 6: FSCTL request FSCTL_GET_INTEGRITY_INFORMATION
            BaseTestSite.Log.Add(LogEntryKind.TestStep, "6. FSCTL request FSCTL_GET_INTEGRITY_INFORMATION.");
            status = fsaAdapter.FsCtlGetIntegrityInfo(outputBufferSize, out _, out outputBuffer);
            fsaAdapter.AssertAreEqual(Manager, MessageStatus.SUCCESS, status, "Failed to get integrity info of the file.");

            //Step 7: Verify ChecksumAlgorithm
            getIntegrityInfo = TypeMarshal.ToStruct<FSCTL_GET_INTEGRITY_INFORMATION_BUFFER>(outputBuffer);
            if (fsaAdapter.ReFSVersion == 1)
            {
                bool isChecksumTypeCRC64 = (getIntegrityInfo.ChecksumAlgorithm == FSCTL_GET_INTEGRITY_INFORMATION_BUFFER_CHECKSUMALGORITHM.CHECKSUM_TYPE_CRC64);
                BaseTestSite.Log.Add(LogEntryKind.TestStep, "7. Verify ChecksumAlgorithm is correctly set.");
                fsaAdapter.AssertAreEqual(Manager, true, isChecksumTypeCRC64, "ChecksumAlgorithm should be CHECKSUM_TYPE_CRC64.");
            }
            else
            {
                bool isChecksumSet = (getIntegrityInfo.ChecksumAlgorithm != FSCTL_GET_INTEGRITY_INFORMATION_BUFFER_CHECKSUMALGORITHM.CHECKSUM_TYPE_NONE);
                BaseTestSite.Log.Add(LogEntryKind.TestStep, "7. Verify ChecksumAlgorithm is correctly set.");
                fsaAdapter.AssertAreEqual(Manager, true, isChecksumSet, "ChecksumAlgorithm should not be CHECKSUM_TYPE_NONE.");
            }
        }

        #endregion
    }
}
