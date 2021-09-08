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

        #region FsCtl_Set_IntegrityInformationEx_IsSupported

        [TestMethod()]
        [TestCategory(TestCategories.Bvt)]
        [TestCategory(TestCategories.Fsa)]
        [TestCategory(TestCategories.IoCtlRequest)]
        [TestCategory(TestCategories.NonSmb)]
        [Description("Send FSCTL_SET_INTEGRITY_INFORMATION_EX request to a file and check if Integrity is supported.")]
        public void FsCtl_Set_IntegrityInformationEx_File_IsIntegritySupported()
        {
            FsCtl_Set_IntegrityInformationEx_IsIntegritySupported(FileType.DataFile);
        }

        [TestMethod()]
        [TestCategory(TestCategories.Bvt)]
        [TestCategory(TestCategories.Fsa)]
        [TestCategory(TestCategories.IoCtlRequest)]
        [TestCategory(TestCategories.NonSmb)]
        [Description("Send FSCTL_SET_INTEGRITY_INFORMATION request to a directory and check if Integrity is supported.")]
        public void FsCtl_Set_IntegrityInformationEx_Dir_IsIntegritySupported()
        {
            FsCtl_Set_IntegrityInformationEx_IsIntegritySupported(FileType.DirectoryFile);
        }

        #endregion

        #region FsCtl_Set_IntegrityInformationEx_InputBufferSizeLessThanIntegrityBuffer

        [TestMethod()]
        [TestCategory(TestCategories.Fsa)]
        [TestCategory(TestCategories.IoCtlRequest)]
        [TestCategory(TestCategories.NonSmb)]
        [TestCategory(TestCategories.Positive)]
        [Description("Send FSCTL_SET_INTEGRITY_INFORMATION_EX request to a file and check if server responds correctly when the input buffer length is less than the size, in bytes, of the FSCTL_SET_INTEGRITY_INFORMATION_BUFFER_EX element.")]
        public void FsCtl_Set_IntegrityInformationEx_File_InputBufferSizeLessThanIntegrityBuffer()
        {
            FsCtl_Set_IntegrityInformationEx_InputBufferSizeLessThanIntegrityBuffer(FileType.DataFile);
        }

        [TestMethod()]
        [TestCategory(TestCategories.Fsa)]
        [TestCategory(TestCategories.IoCtlRequest)]
        [TestCategory(TestCategories.NonSmb)]
        [TestCategory(TestCategories.Positive)]
        [Description("Send FSCTL_SET_INTEGRITY_INFORMATION_EX request to a directory and check if server responds correctly when the input buffer length is less than the size, in bytes, of the FSCTL_SET_INTEGRITY_INFORMATION_BUFFER_EX element.")]
        public void FsCtl_Set_IntegrityInformationEx_Dir_InputBufferSizeLessThanIntegrityBuffer()
        {
            FsCtl_Set_IntegrityInformationEx_InputBufferSizeLessThanIntegrityBuffer(FileType.DirectoryFile);
        }

        #endregion

        #region FsCtl_Set_IntegrityInformationEx_InvalidParameter_NonEmptyFile

        [TestMethod()]
        [TestCategory(TestCategories.Fsa)]
        [TestCategory(TestCategories.IoCtlRequest)]
        [TestCategory(TestCategories.NonSmb)]
        [TestCategory(TestCategories.Positive)]
        [Description("Send FSCTL_SET_INTEGRITY_INFORMATION_EX request to a file with invalid parameter and check if server responses correctly.")]
        public void FsCtl_Set_IntegrityInformationEx_InvalidParameter_NonEmptyFile()
        {
            BaseTestSite.Log.Add(LogEntryKind.TestStep, "Test case steps:");
            MessageStatus status;

            //Step 1: Create file
            BaseTestSite.Log.Add(LogEntryKind.TestStep, "1. Create DataFile");
            status = fsaAdapter.CreateFile(FileType.DataFile);
            fsaAdapter.AssertAreEqual(Manager, MessageStatus.SUCCESS, status, "Failed to create the file.");

            //Step 2: FSCTL request FSCTL_SET_INTEGRITY_INFORMATION_EX
            FSCTL_SET_INTEGRITY_INFORMATION_BUFFER_EX integrityInfo = new()
            {
                EnableIntegrity = FSCTL_SET_INTEGRITY_INFORMATION_BUFFER_EX_ENABLEINTEGRITY.ENABLE_INTEGRITY,
                KeepIntegrityStateUnchanged = FSCTL_SET_INTEGRITY_INFORMATION_BUFFER_EX_KEEPINTEGRITYSTATEUNCHANGED.CHANGE_INTEGRITY,
                Flags = FSCTL_SET_INTEGRITY_INFORMATION_BUFFER_EX_FLAGS.FSCTL_INTEGRITY_FLAG_CHECKSUM_ENFORCEMENT_OFF,
                Version = FSCTL_SET_INTEGRITY_INFORMATION_BUFFER_EX_VERSION.V1,
                Reserved2 = new byte[7],
            };
            uint inputBufferSize = (uint)TypeMarshal.ToBytes(integrityInfo).Length;

            BaseTestSite.Log.Add(LogEntryKind.TestStep, "2. FSCTL request FSCTL_SET_INTEGRITY_INFORMATION_EX.");
            status = fsaAdapter.FsCtlSetIntegrityInfoEx(integrityInfo, inputBufferSize);

            // Check if Integrity is supported
            if (!IsCurrentTransportSupportIntegrity(status)) return;
            if (fsaAdapter.IsIntegritySupported == false)
            {
                fsaAdapter.AssertAreEqual(Manager, MessageStatus.INVALID_DEVICE_REQUEST, status, "If the object store does not implement this functionality, the operation MUST be failed with STATUS_INVALID_DEVICE_REQUEST.");
                return;
            }
            else
            {
                fsaAdapter.AssertAreEqual(Manager, MessageStatus.SUCCESS, status, "Status should be STATUS_SUCCESS.");
            }

            //Step 3: FSCTL request FSCTL_GET_INTEGRITY_INFORMATION_EX
            FSCTL_GET_INTEGRITY_INFORMATION_BUFFER getIntegrityInfo = new();
            uint outputBufferSize = (uint)TypeMarshal.ToBytes(getIntegrityInfo).Length;

            BaseTestSite.Log.Add(LogEntryKind.TestStep, "3. FSCTL request FSCTL_GET_INTEGRITY_INFORMATION.");
            status = fsaAdapter.FsCtlGetIntegrityInfo(outputBufferSize, out _, out byte[] outputBuffer);
            fsaAdapter.AssertAreEqual(Manager, MessageStatus.SUCCESS, status, "Failed to get integrity info of the file.");

            //Step 4: Verify ChecksumAlgorithm is set to either CRC32 or CRC64
            getIntegrityInfo = TypeMarshal.ToStruct<FSCTL_GET_INTEGRITY_INFORMATION_BUFFER>(outputBuffer);
            bool isChecksumTypeSet = (getIntegrityInfo.ChecksumAlgorithm == FSCTL_GET_INTEGRITY_INFORMATION_BUFFER_CHECKSUMALGORITHM.CHECKSUM_TYPE_CRC32 || getIntegrityInfo.ChecksumAlgorithm == FSCTL_GET_INTEGRITY_INFORMATION_BUFFER_CHECKSUMALGORITHM.CHECKSUM_TYPE_CRC64);
            BaseTestSite.Log.Add(LogEntryKind.TestStep, "4. Verify ChecksumAlgorithm is correctly set.");
            fsaAdapter.AssertAreEqual(Manager, true, isChecksumTypeSet, "ChecksumAlgorithm should be either CRC32 or CRC64.");
            BaseTestSite.Log.Add(LogEntryKind.TestStep, "5. Write some data so that the file is not empty.");

            //Step 5: Write some data so that the file is not empty
            status = fsaAdapter.WriteFile(0, 10240, out _);
            fsaAdapter.AssertAreEqual(Manager, MessageStatus.SUCCESS, status, "Failed to write to the file.");

            //Step 6: FSCTL request FSCTL_SET_INTEGRITY_INFORMATION_EX
            integrityInfo.KeepIntegrityStateUnchanged = FSCTL_SET_INTEGRITY_INFORMATION_BUFFER_EX_KEEPINTEGRITYSTATEUNCHANGED.CHANGE_INTEGRITY;
            integrityInfo.EnableIntegrity = FSCTL_SET_INTEGRITY_INFORMATION_BUFFER_EX_ENABLEINTEGRITY.ENABLE_INTEGRITY;
            integrityInfo.Flags = FSCTL_SET_INTEGRITY_INFORMATION_BUFFER_EX_FLAGS.NONE;
            BaseTestSite.Log.Add(LogEntryKind.TestStep, "6. FSCTL request FSCTL_SET_INTEGRITY_INFORMATION.");
            status = fsaAdapter.FsCtlSetIntegrityInfoEx(integrityInfo, inputBufferSize);

            //Step 7: Verify test result
            BaseTestSite.Log.Add(LogEntryKind.TestStep, "7. Verify returned NTStatus code.");
            fsaAdapter.AssertAreEqual(Manager, MessageStatus.SUCCESS, status,
                "FSCTL_SET_INTEGRITY_INFORMATION request should succeed when change the checksum state of a non-empty file.");
        }

        #endregion

        #region FsCtl_Set_IntegrityInformationEx_UndefinedVersion

        [TestMethod()]
        [TestCategory(TestCategories.Fsa)]
        [TestCategory(TestCategories.IoCtlRequest)]
        [TestCategory(TestCategories.NonSmb)]
        [TestCategory(TestCategories.Positive)]
        [Description("Send FSCTL_SET_INTEGRITY_INFORMATION_EX request to a file with undefined version and check if server responses correctly.")]
        public void FsCtl_Set_IntegrityInformationEx_File_UndefinedVersion()
        {
            FsCtl_Set_IntegrityInformationEx_UndefinedVersion(FileType.DataFile);
        }

        [TestMethod()]
        [TestCategory(TestCategories.Fsa)]
        [TestCategory(TestCategories.IoCtlRequest)]
        [TestCategory(TestCategories.NonSmb)]
        [TestCategory(TestCategories.Positive)]
        [Description("Send FSCTL_SET_INTEGRITY_INFORMATION_EX request to a directory with undefined version and check if server responses correctly.")]
        public void FsCtl_Set_IntegrityInformationEx_Dir_UndefinedVersion()
        {
            FsCtl_Set_IntegrityInformationEx_UndefinedVersion(FileType.DirectoryFile);
        }

        #endregion

        #region FsCtl_Set_IntegrityInformationEx_WriteProtected

        [TestMethod()]
        [TestCategory(TestCategories.Fsa)]
        [TestCategory(TestCategories.IoCtlRequest)]
        [TestCategory(TestCategories.NonSmb)]
        [TestCategory(TestCategories.Positive)]
        [Description("Send FSCTL_SET_INTEGRITY_INFORMATION_EX request to a file which is write protected and check if server responses correctly.")]
        public void FsCtl_Set_IntegrityInformationEx_File_WriteProtected()
        {
            FsCtl_Set_IntegrityInformationEx_WriteProtected(FileType.DataFile);
        }

        [TestMethod()]
        [TestCategory(TestCategories.Fsa)]
        [TestCategory(TestCategories.IoCtlRequest)]
        [TestCategory(TestCategories.NonSmb)]
        [TestCategory(TestCategories.Positive)]
        [Description("Send FSCTL_SET_INTEGRITY_INFORMATION_EX request to a directory which is write protected and check if server responses correctly.")]
        public void FsCtl_Set_IntegrityInformationEx_Dir_WriteProtected()
        {
            FsCtl_Set_IntegrityInformationEx_WriteProtected(FileType.DirectoryFile);
        }

        #endregion

        #region FsCtl_Set_IntegrityInformationEx_EnableIntegrityAndUnchanged

        [TestMethod()]
        [TestCategory(TestCategories.Fsa)]
        [TestCategory(TestCategories.IoCtlRequest)]
        [TestCategory(TestCategories.NonSmb)]
        [TestCategory(TestCategories.Positive)]
        [Description("Send FSCTL_SET_INTEGRITY_INFORMATION_EX request to a file to enable integrity, then send another request with KeepIntegrityStateUnchanged, and check if server responds correctly.")]
        public void FsCtl_Set_IntegrityInformationEx_File_EnableIntegrityAndUnchanged()
        {
            FsCtl_Set_IntegrityInformationEx_SetIntegrityAndUnchanged(FileType.DataFile, FSCTL_SET_INTEGRITY_INFORMATION_BUFFER_EX_ENABLEINTEGRITY.ENABLE_INTEGRITY);
        }

        [TestMethod()]
        [TestCategory(TestCategories.Fsa)]
        [TestCategory(TestCategories.IoCtlRequest)]
        [TestCategory(TestCategories.NonSmb)]
        [TestCategory(TestCategories.Positive)]
        [Description("Send FSCTL_SET_INTEGRITY_INFORMATION_EX request to a directory to enable integrity, then send another request with KeepIntegrityStateUnchanged, and check if server responds correctly.")]
        public void FsCtl_Set_IntegrityInformationEx_Dir_EnableIntegrityAndUnchanged()
        {
            FsCtl_Set_IntegrityInformationEx_SetIntegrityAndUnchanged(FileType.DirectoryFile, FSCTL_SET_INTEGRITY_INFORMATION_BUFFER_EX_ENABLEINTEGRITY.ENABLE_INTEGRITY);
        }

        #endregion

        #region FsCtl_Set_IntegrityInformationEx_DisableIntegrityAndUnchanged

        [TestMethod()]
        [TestCategory(TestCategories.Fsa)]
        [TestCategory(TestCategories.IoCtlRequest)]
        [TestCategory(TestCategories.NonSmb)]
        [TestCategory(TestCategories.Positive)]
        [Description("Send FSCTL_SET_INTEGRITY_INFORMATION_EX request to a file to disable integrity, then send another request with KeepIntegrityStateUnchanged, and check if server responds correctly.")]
        public void FsCtl_Set_IntegrityInformationEx_File_DisableIntegrityAndUnchanged()
        {
            FsCtl_Set_IntegrityInformationEx_SetIntegrityAndUnchanged(FileType.DataFile, FSCTL_SET_INTEGRITY_INFORMATION_BUFFER_EX_ENABLEINTEGRITY.DISABLE_INTEGRITY);
        }

        [TestMethod()]
        [TestCategory(TestCategories.Fsa)]
        [TestCategory(TestCategories.IoCtlRequest)]
        [TestCategory(TestCategories.NonSmb)]
        [TestCategory(TestCategories.Positive)]
        [Description("Send FSCTL_SET_INTEGRITY_INFORMATION_EX request to a directory to disable integrity, then send another request with KeepIntegrityStateUnchanged, and check if server responds correctly.")]
        public void FsCtl_Set_IntegrityInformationEx_Dir_DisableIntegrityAndUnchanged()
        {
            FsCtl_Set_IntegrityInformationEx_SetIntegrityAndUnchanged(FileType.DirectoryFile, FSCTL_SET_INTEGRITY_INFORMATION_BUFFER_EX_ENABLEINTEGRITY.DISABLE_INTEGRITY);
        }

        #endregion

        #endregion

        #region Test Case Utility

        private void FsCtl_Set_IntegrityInformationEx_IsIntegritySupported(FileType fileType)
        {
            BaseTestSite.Log.Add(LogEntryKind.TestStep, "Test case steps:");
            MessageStatus status;

            //Step 1: Create file
            BaseTestSite.Log.Add(LogEntryKind.TestStep, "1. Create " + fileType.ToString());
            status = fsaAdapter.CreateFile(fileType);
            fsaAdapter.AssertAreEqual(Manager, MessageStatus.SUCCESS, status, "Failed to create the file.");

            //Step 2: FSCTL request FSCTL_SET_INTEGRITY_INFORMATION_EX
            FSCTL_SET_INTEGRITY_INFORMATION_BUFFER_EX integrityInfo = new()
            {
                EnableIntegrity = FSCTL_SET_INTEGRITY_INFORMATION_BUFFER_EX_ENABLEINTEGRITY.ENABLE_INTEGRITY,
                KeepIntegrityStateUnchanged = FSCTL_SET_INTEGRITY_INFORMATION_BUFFER_EX_KEEPINTEGRITYSTATEUNCHANGED.CHANGE_INTEGRITY,
                Version = FSCTL_SET_INTEGRITY_INFORMATION_BUFFER_EX_VERSION.V1,
                Reserved2 = new byte[7],
            };

            uint inputBufferSize = (uint)TypeMarshal.ToBytes(integrityInfo).Length;

            BaseTestSite.Log.Add(LogEntryKind.TestStep, "2. FSCTL request FSCTL_SET_INTEGRITY_INFORMATION_EX.");
            status = fsaAdapter.FsCtlSetIntegrityInfoEx(integrityInfo, inputBufferSize);

            //Step 3: Verify test result
            BaseTestSite.Log.Add(LogEntryKind.TestStep, "3. Verify returned NTStatus code.");
            if (!IsCurrentTransportSupportIntegrity(status)) return;

            if (fsaAdapter.IsIntegritySupported == false)
            {
                fsaAdapter.AssertAreEqual(Manager, MessageStatus.INVALID_DEVICE_REQUEST, status, "If the object store does not implement this functionality, the operation MUST be failed with STATUS_INVALID_DEVICE_REQUEST.");
            }
            else
            {
                fsaAdapter.AssertAreEqual(Manager, MessageStatus.SUCCESS, status, "Status should be STATUS_SUCCESS.");
            }
        }

        private void FsCtl_Set_IntegrityInformationEx_InputBufferSizeLessThanIntegrityBuffer(FileType fileType)
        {
            BaseTestSite.Log.Add(LogEntryKind.TestStep, "Test case steps:");
            MessageStatus status;

            //Step 1: Create file
            BaseTestSite.Log.Add(LogEntryKind.TestStep, "1. Create " + fileType.ToString());
            status = fsaAdapter.CreateFile(fileType);
            fsaAdapter.AssertAreEqual(Manager, MessageStatus.SUCCESS, status, "Failed to create the file.");

            //Step 2: FSCTL request FSCTL_SET_INTEGRITY_INFORMATION_EX
            FSCTL_SET_INTEGRITY_INFORMATION_BUFFER_EX integrityInfo = new()
            {
                KeepIntegrityStateUnchanged = FSCTL_SET_INTEGRITY_INFORMATION_BUFFER_EX_KEEPINTEGRITYSTATEUNCHANGED.KEEP_INTEGRITY,
                Flags = FSCTL_SET_INTEGRITY_INFORMATION_BUFFER_EX_FLAGS.FSCTL_INTEGRITY_FLAG_CHECKSUM_ENFORCEMENT_OFF,
                Version = FSCTL_SET_INTEGRITY_INFORMATION_BUFFER_EX_VERSION.V1,
                Reserved2 = new byte[7],
            };
            uint inputBufferSize = (uint)TypeMarshal.ToBytes(integrityInfo).Length - 1;

            BaseTestSite.Log.Add(LogEntryKind.TestStep, "2. FSCTL request FSCTL_SET_INTEGRITY_INFORMATION.");
            status = fsaAdapter.FsCtlSetIntegrityInfoEx(integrityInfo, inputBufferSize);

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

        private void FsCtl_Set_IntegrityInformationEx_UndefinedVersion(FileType fileType)
        {
            BaseTestSite.Log.Add(LogEntryKind.TestStep, "Test case steps:");
            MessageStatus status;

            //Step 1: Create file
            BaseTestSite.Log.Add(LogEntryKind.TestStep, "1. Create " + fileType.ToString());
            status = fsaAdapter.CreateFile(fileType);
            fsaAdapter.AssertAreEqual(Manager, MessageStatus.SUCCESS, status, "Failed to create the file.");

            //Step 2: FSCTL request FSCTL_SET_INTEGRITY_INFORMATION
            FSCTL_SET_INTEGRITY_INFORMATION_BUFFER_EX integrityInfo = new()
            {
                KeepIntegrityStateUnchanged = FSCTL_SET_INTEGRITY_INFORMATION_BUFFER_EX_KEEPINTEGRITYSTATEUNCHANGED.KEEP_INTEGRITY,
                Flags = FSCTL_SET_INTEGRITY_INFORMATION_BUFFER_EX_FLAGS.FSCTL_INTEGRITY_FLAG_CHECKSUM_ENFORCEMENT_OFF,
                Version = 0, // 1 is the correct value
                Reserved2 = new byte[7],
            };

            uint inputBufferSize = (uint)TypeMarshal.ToBytes(integrityInfo).Length;

            BaseTestSite.Log.Add(LogEntryKind.TestStep, "2. FSCTL request FSCTL_SET_INTEGRITY_INFORMATION_EX with undefined version 0.");
            status = fsaAdapter.FsCtlSetIntegrityInfoEx(integrityInfo, inputBufferSize);

            //Step 3: Verify test result
            BaseTestSite.Log.Add(LogEntryKind.TestStep, "3. Verify returned NTStatus code.");
            if (!IsCurrentTransportSupportIntegrity(status)) return;

            if (fsaAdapter.IsIntegritySupported == false)
            {
                fsaAdapter.AssertAreEqual(Manager, MessageStatus.INVALID_DEVICE_REQUEST, status, "If the object store does not implement this functionality, the operation MUST be failed with STATUS_INVALID_DEVICE_REQUEST.");
            }
            else
            {
                fsaAdapter.AssertAreEqual(Manager, MessageStatus.INVALID_PARAMETER, status, "The operation MUST be failed with STATUS_INVALID_PARAMETER if InputBuffer.Version != 1.");
            }
        }

        private void FsCtl_Set_IntegrityInformationEx_WriteProtected(FileType fileType)
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
            FSCTL_SET_INTEGRITY_INFORMATION_BUFFER_EX integrityInfo = new()
            {
                KeepIntegrityStateUnchanged = FSCTL_SET_INTEGRITY_INFORMATION_BUFFER_EX_KEEPINTEGRITYSTATEUNCHANGED.KEEP_INTEGRITY,
                Flags = FSCTL_SET_INTEGRITY_INFORMATION_BUFFER_EX_FLAGS.FSCTL_INTEGRITY_FLAG_CHECKSUM_ENFORCEMENT_OFF,
                Version = FSCTL_SET_INTEGRITY_INFORMATION_BUFFER_EX_VERSION.V1,
                Reserved2 = new byte[7],
            };

            uint inputBufferSize = (uint)TypeMarshal.ToBytes(integrityInfo).Length;

            BaseTestSite.Log.Add(LogEntryKind.TestStep, "2. FSCTL request FSCTL_SET_INTEGRITY_INFORMATION_EX.");
            status = fsaAdapter.FsCtlSetIntegrityInfoEx(integrityInfo, inputBufferSize);

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
                    fsaAdapter.AssertAreEqual(Manager, MessageStatus.SUCCESS, status, "Status should be STATUS_SUCCESS.");
                }
            }
        }

        private void FsCtl_Set_IntegrityInformationEx_SetIntegrityAndUnchanged(FileType fileType, FSCTL_SET_INTEGRITY_INFORMATION_BUFFER_EX_ENABLEINTEGRITY enableIntegrity)
        {
            BaseTestSite.Log.Add(LogEntryKind.TestStep, "Test case steps:");
            MessageStatus status;

            //Step 1: Create file
            BaseTestSite.Log.Add(LogEntryKind.TestStep, "1. Create " + fileType.ToString());
            status = fsaAdapter.CreateFile(fileType);
            fsaAdapter.AssertAreEqual(Manager, MessageStatus.SUCCESS, status, "Failed to create the file.");

            //Step 2: FSCTL request FSCTL_SET_INTEGRITY_INFORMATION_EX with KeepIntegrityStateUnchanged set to 1 (change) and enableIntegrity set to the parameter passed in.
            FSCTL_SET_INTEGRITY_INFORMATION_BUFFER_EX integrityInfo = new()
            {
                EnableIntegrity = enableIntegrity,
                KeepIntegrityStateUnchanged = FSCTL_SET_INTEGRITY_INFORMATION_BUFFER_EX_KEEPINTEGRITYSTATEUNCHANGED.CHANGE_INTEGRITY,
                Flags = FSCTL_SET_INTEGRITY_INFORMATION_BUFFER_EX_FLAGS.FSCTL_INTEGRITY_FLAG_CHECKSUM_ENFORCEMENT_OFF,
                Version = FSCTL_SET_INTEGRITY_INFORMATION_BUFFER_EX_VERSION.V1,
                Reserved2 = new byte[7],
            };
            uint inputBufferSize = (uint)TypeMarshal.ToBytes(integrityInfo).Length;

            BaseTestSite.Log.Add(LogEntryKind.TestStep, $"2. FSCTL request FSCTL_SET_INTEGRITY_INFORMATION with KeepIntegrityStateUnchanged set to 1 (change) and enableIntegrity set to {enableIntegrity}.");
            status = fsaAdapter.FsCtlSetIntegrityInfoEx(integrityInfo, inputBufferSize);

            // Check if Integrity is supported
            if (!IsCurrentTransportSupportIntegrity(status)) return;
            if (fsaAdapter.IsIntegritySupported == false)
            {
                fsaAdapter.AssertAreEqual(Manager, MessageStatus.INVALID_DEVICE_REQUEST, status, "If the object store does not implement this functionality, the operation MUST be failed with STATUS_INVALID_DEVICE_REQUEST.");
                return;
            }
            else
            {
                fsaAdapter.AssertAreEqual(Manager, MessageStatus.SUCCESS, status, "Status should be STATUS_SUCCESS.");
            }

            //Step 3: FSCTL request FSCTL_GET_INTEGRITY_INFORMATION
            FSCTL_GET_INTEGRITY_INFORMATION_BUFFER getIntegrityInfo = new();
            uint outputBufferSize = (uint)TypeMarshal.ToBytes(getIntegrityInfo).Length;

            BaseTestSite.Log.Add(LogEntryKind.TestStep, "3. FSCTL request FSCTL_GET_INTEGRITY_INFORMATION.");
            status = fsaAdapter.FsCtlGetIntegrityInfo(outputBufferSize, out _, out byte[] outputBuffer);
            fsaAdapter.AssertAreEqual(Manager, MessageStatus.SUCCESS, status, "Failed to get integrity info of the file.");

            //Step 4: Verify ChecksumAlgorithm
            getIntegrityInfo = TypeMarshal.ToStruct<FSCTL_GET_INTEGRITY_INFORMATION_BUFFER>(outputBuffer);

            bool isChecksumTypeCorrect(FSCTL_GET_INTEGRITY_INFORMATION_BUFFER_CHECKSUMALGORITHM checksumAlgorithm)
            {
                if (enableIntegrity == FSCTL_SET_INTEGRITY_INFORMATION_BUFFER_EX_ENABLEINTEGRITY.DISABLE_INTEGRITY)
                {
                    return checksumAlgorithm == FSCTL_GET_INTEGRITY_INFORMATION_BUFFER_CHECKSUMALGORITHM.CHECKSUM_TYPE_NONE;
                }
                else // ENABLE_INTEGRITY
                {
                    return (checksumAlgorithm == FSCTL_GET_INTEGRITY_INFORMATION_BUFFER_CHECKSUMALGORITHM.CHECKSUM_TYPE_CRC32 || checksumAlgorithm == FSCTL_GET_INTEGRITY_INFORMATION_BUFFER_CHECKSUMALGORITHM.CHECKSUM_TYPE_CRC64);
                }
            }

            BaseTestSite.Log.Add(LogEntryKind.TestStep, "4. Verify ChecksumAlgorithm.");
            fsaAdapter.AssertAreEqual(Manager, true, isChecksumTypeCorrect(getIntegrityInfo.ChecksumAlgorithm), $"ChecksumAlgorithm is {getIntegrityInfo.ChecksumAlgorithm}, which does not match EnableIntegrity {enableIntegrity}.");

            //Step 5: FSCTL request FSCTL_SET_INTEGRITY_INFORMATION_EX
            integrityInfo.KeepIntegrityStateUnchanged = FSCTL_SET_INTEGRITY_INFORMATION_BUFFER_EX_KEEPINTEGRITYSTATEUNCHANGED.KEEP_INTEGRITY;
            inputBufferSize = (uint)TypeMarshal.ToBytes(integrityInfo).Length;

            BaseTestSite.Log.Add(LogEntryKind.TestStep, "5. FSCTL request FSCTL_SET_INTEGRITY_INFORMATION_EX with KeepIntegrityStateUnchanged set to 0 (unchanged).");
            status = fsaAdapter.FsCtlSetIntegrityInfoEx(integrityInfo, inputBufferSize);

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
            BaseTestSite.Log.Add(LogEntryKind.TestStep, "7. Verify ChecksumAlgorithm.");
            fsaAdapter.AssertAreEqual(Manager, true, isChecksumTypeCorrect(getIntegrityInfo.ChecksumAlgorithm), $"ChecksumAlgorithm is {getIntegrityInfo.ChecksumAlgorithm}, which does not match EnableIntegrity {enableIntegrity}.");
        }

        #endregion
    }
}
