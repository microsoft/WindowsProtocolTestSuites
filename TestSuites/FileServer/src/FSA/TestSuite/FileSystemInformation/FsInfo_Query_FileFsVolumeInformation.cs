// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using Microsoft.Protocols.TestSuites.FileSharing.Common.Adapter;
using Microsoft.Protocols.TestSuites.FileSharing.FSA.Adapter;
using Microsoft.Protocols.TestTools;
using Microsoft.Protocols.TestTools.StackSdk;
using Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Fscc;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Protocols.TestSuites.FileSharing.FSA.TestSuite
{
    public partial class FsInfoTestCases : PtfTestClassBase
    {
        private const uint FileFsVolumeInformationAlignment = 8;

        #region Test Cases

        [TestMethod()]
        [TestCategory(TestCategories.Fsa)]
        [TestCategory(TestCategories.QueryFileSystemInformation)]
        [TestCategory(TestCategories.NonSmb)]
        [TestCategory(TestCategories.Positive)]
        [Description("Query FileFsVolumeInformation from a file with an output buffer smaller than the aligned fixed portion.")]
        public void FsInfo_Query_FileFsVolumeInformation_File_OutputBufferSizeLessThanMinimum()
        {
            FsInfo_Query_FileFsVolumeInformation_OutputBufferSizeLessThanMinimum(FileType.DataFile);
        }

        [TestMethod()]
        [TestCategory(TestCategories.Fsa)]
        [TestCategory(TestCategories.QueryFileSystemInformation)]
        [TestCategory(TestCategories.NonSmb)]
        [TestCategory(TestCategories.Positive)]
        [Description("Query FileFsVolumeInformation from a directory with an output buffer smaller than the aligned fixed portion.")]
        public void FsInfo_Query_FileFsVolumeInformation_Dir_OutputBufferSizeLessThanMinimum()
        {
            FsInfo_Query_FileFsVolumeInformation_OutputBufferSizeLessThanMinimum(FileType.DirectoryFile);
        }

        [TestMethod()]
        [TestCategory(TestCategories.Fsa)]
        [TestCategory(TestCategories.QueryFileSystemInformation)]
        [TestCategory(TestCategories.NonSmb)]
        [TestCategory(TestCategories.Positive)]
        [Description("Query FileFsVolumeInformation from a file with space for only part of the volume label.")]
        public void FsInfo_Query_FileFsVolumeInformation_File_OutputBufferOverflow()
        {
            FsInfo_Query_FileFsVolumeInformation_OutputBufferOverflow(FileType.DataFile);
        }

        [TestMethod()]
        [TestCategory(TestCategories.Fsa)]
        [TestCategory(TestCategories.QueryFileSystemInformation)]
        [TestCategory(TestCategories.NonSmb)]
        [TestCategory(TestCategories.Positive)]
        [Description("Query FileFsVolumeInformation from a directory with space for only part of the volume label.")]
        public void FsInfo_Query_FileFsVolumeInformation_Dir_OutputBufferOverflow()
        {
            FsInfo_Query_FileFsVolumeInformation_OutputBufferOverflow(FileType.DirectoryFile);
        }

        [TestMethod()]
        [TestCategory(TestCategories.Bvt)]
        [TestCategory(TestCategories.Fsa)]
        [TestCategory(TestCategories.QueryFileSystemInformation)]
        [TestCategory(TestCategories.NonSmb)]
        [Description("Query FileFsVolumeInformation from a file and verify the complete response.")]
        public void FsInfo_Query_FileFsVolumeInformation_File_OutputValues()
        {
            FsInfo_Query_FileFsVolumeInformation_OutputValues(FileType.DataFile);
        }

        [TestMethod()]
        [TestCategory(TestCategories.Fsa)]
        [TestCategory(TestCategories.QueryFileSystemInformation)]
        [TestCategory(TestCategories.NonSmb)]
        [TestCategory(TestCategories.Positive)]
        [Description("Query FileFsVolumeInformation from a directory and verify the complete response.")]
        public void FsInfo_Query_FileFsVolumeInformation_Dir_OutputValues()
        {
            FsInfo_Query_FileFsVolumeInformation_OutputValues(FileType.DirectoryFile);
        }

        #endregion

        #region Test Case Utility

        private void FsInfo_Query_FileFsVolumeInformation_OutputBufferSizeLessThanMinimum(FileType fileType)
        {
            BaseTestSite.Log.Add(LogEntryKind.TestStep, "Test case steps:");

            BaseTestSite.Log.Add(LogEntryKind.TestStep, "1. Create " + fileType.ToString());
            MessageStatus status = this.fsaAdapter.CreateFile(fileType);
            this.fsaAdapter.AssertAreEqual(
                this.Manager,
                MessageStatus.SUCCESS,
                status,
                "CreateFile is expected to succeed.");

            uint volumeLabelOffset = GetFileFsVolumeInformationVolumeLabelOffset();
            uint minimumOutputBufferSize = BlockAlign(volumeLabelOffset, FileFsVolumeInformationAlignment);

            BaseTestSite.Log.Add(
                LogEntryKind.TestStep,
                "2. Query FileFsVolumeInformation with OutputBufferSize smaller than BlockAlign(FieldOffset(VolumeLabel), 8).");
            status = this.fsaAdapter.QueryFileSystemInformation(
                FileSystemInfoClass.File_FsVolumeInformation,
                minimumOutputBufferSize - 1,
                out _,
                out _);

            BaseTestSite.Log.Add(LogEntryKind.TestStep, "3. Verify STATUS_INFO_LENGTH_MISMATCH is returned.");
            this.fsaAdapter.AssertAreEqual(
                this.Manager,
                MessageStatus.INFO_LENGTH_MISMATCH,
                status,
                "[MS-FSA 2.1.5.13.1] An OutputBufferSize smaller than BlockAlign(FieldOffset(FILE_FS_VOLUME_INFORMATION.VolumeLabel), 8) MUST fail with STATUS_INFO_LENGTH_MISMATCH.");
        }

        private void FsInfo_Query_FileFsVolumeInformation_OutputBufferOverflow(FileType fileType)
        {
            BaseTestSite.Log.Add(LogEntryKind.TestStep, "Test case steps:");

            BaseTestSite.Log.Add(LogEntryKind.TestStep, "1. Create " + fileType.ToString());
            MessageStatus status = this.fsaAdapter.CreateFile(fileType);
            this.fsaAdapter.AssertAreEqual(
                this.Manager,
                MessageStatus.SUCCESS,
                status,
                "CreateFile is expected to succeed.");

            BaseTestSite.Log.Add(LogEntryKind.TestStep, "2. Query the complete FileFsVolumeInformation response.");
            FileFsVolumeInformation completeVolumeInformation = QueryCompleteFileFsVolumeInformation(
                out long completeByteCount,
                out byte[] completeOutputBuffer);

            uint volumeLabelOffset = GetFileFsVolumeInformationVolumeLabelOffset();
            uint minimumOutputBufferSize = BlockAlign(volumeLabelOffset, FileFsVolumeInformationAlignment);
            uint completeResponseSize = volumeLabelOffset + completeVolumeInformation.VolumeLabelLength;

            this.fsaAdapter.AssertAreEqual(
                this.Manager,
                (long)completeResponseSize,
                completeByteCount,
                "[MS-FSA 2.1.5.13.1] ByteCount MUST equal FieldOffset(FILE_FS_VOLUME_INFORMATION.VolumeLabel) plus BytesToCopy.");
            this.fsaAdapter.AssertAreEqual(
                this.Manager,
                0u,
                completeVolumeInformation.VolumeLabelLength % sizeof(char),
                "[MS-FSCC 2.5.9] VolumeLabel is a Unicode field, so VolumeLabelLength is expected to contain complete UTF-16 code units.");

            BaseTestSite.Assume.IsTrue(
                completeResponseSize > minimumOutputBufferSize,
                "The configured volume label must be longer than {0} bytes to exercise STATUS_BUFFER_OVERFLOW.",
                minimumOutputBufferSize - volumeLabelOffset);

            uint truncatedOutputBufferSize = completeResponseSize - sizeof(char);

            BaseTestSite.Log.Add(LogEntryKind.TestStep, "3. Query FileFsVolumeInformation with space for only part of VolumeLabel.");
            status = this.fsaAdapter.QueryFileSystemInformation(
                FileSystemInfoClass.File_FsVolumeInformation,
                truncatedOutputBufferSize,
                out long truncatedByteCount,
                out byte[] truncatedOutputBuffer);

            BaseTestSite.Log.Add(LogEntryKind.TestStep, "4. Verify the partial response, ByteCount, and STATUS_BUFFER_OVERFLOW.");
            this.fsaAdapter.AssertAreEqual(
                this.Manager,
                MessageStatus.BUFFER_OVERFLOW,
                status,
                "[MS-FSA 2.1.5.13.1] The status MUST be STATUS_BUFFER_OVERFLOW when BytesToCopy is less than VolumeLabelLength.");
            BaseTestSite.Assert.IsNotNull(
                truncatedOutputBuffer,
                "The partial FILE_FS_VOLUME_INFORMATION response must be returned with STATUS_BUFFER_OVERFLOW.");
            this.fsaAdapter.AssertAreEqual(
                this.Manager,
                (long)truncatedOutputBufferSize,
                truncatedByteCount,
                "[MS-FSA 2.1.5.13.1] ByteCount MUST equal FieldOffset(FILE_FS_VOLUME_INFORMATION.VolumeLabel) plus BytesToCopy.");
            this.fsaAdapter.AssertAreEqual(
                this.Manager,
                (int)truncatedOutputBufferSize,
                truncatedOutputBuffer.Length,
                "The returned buffer length must match ByteCount.");

            int fixedFieldLength = (int)volumeLabelOffset;
            int bytesToCopy = (int)(truncatedOutputBufferSize - volumeLabelOffset);
            this.fsaAdapter.AssertAreEqual(
                this.Manager,
                true,
                completeOutputBuffer.AsSpan(0, fixedFieldLength).SequenceEqual(
                    truncatedOutputBuffer.AsSpan(0, fixedFieldLength)),
                "[MS-FSA 2.1.5.13.1] The fixed FILE_FS_VOLUME_INFORMATION fields MUST describe the open file's volume.");
            this.fsaAdapter.AssertAreEqual(
                this.Manager,
                true,
                completeOutputBuffer.AsSpan(fixedFieldLength, bytesToCopy).SequenceEqual(
                    truncatedOutputBuffer.AsSpan(fixedFieldLength, bytesToCopy)),
                "[MS-FSA 2.1.5.13.1] The returned VolumeLabel MUST contain the first BytesToCopy bytes of the volume label.");
        }

        private void FsInfo_Query_FileFsVolumeInformation_OutputValues(FileType fileType)
        {
            BaseTestSite.Log.Add(LogEntryKind.TestStep, "Test case steps:");

            BaseTestSite.Log.Add(LogEntryKind.TestStep, "1. Create " + fileType.ToString());
            MessageStatus status = this.fsaAdapter.CreateFile(fileType);
            this.fsaAdapter.AssertAreEqual(
                this.Manager,
                MessageStatus.SUCCESS,
                status,
                "CreateFile is expected to succeed.");

            BaseTestSite.Log.Add(LogEntryKind.TestStep, "2. Query the complete FileFsVolumeInformation response.");
            FileFsVolumeInformation volumeInformation = QueryCompleteFileFsVolumeInformation(
                out long byteCount,
                out byte[] outputBuffer);

            uint volumeLabelOffset = GetFileFsVolumeInformationVolumeLabelOffset();
            int volumeLabelLength = volumeInformation.VolumeLabel == null ? 0 : volumeInformation.VolumeLabel.Length;
            long expectedByteCount = volumeLabelOffset + volumeInformation.VolumeLabelLength;

            BaseTestSite.Log.Add(LogEntryKind.TestStep, "3. Verify the FILE_FS_VOLUME_INFORMATION fields and ByteCount.");
            this.fsaAdapter.AssertAreEqual(
                this.Manager,
                true,
                volumeInformation.VolumeCreationTime >= 0,
                "[MS-FSCC 2.5.9] VolumeCreationTime MUST be greater than or equal to zero.");
            this.fsaAdapter.AssertAreEqual(
                this.Manager,
                (uint)volumeLabelLength,
                volumeInformation.VolumeLabelLength,
                "[MS-FSA 2.1.5.13.1] VolumeLabelLength MUST equal the length, in bytes, of the volume label.");
            this.fsaAdapter.AssertAreEqual(
                this.Manager,
                0u,
                volumeInformation.VolumeLabelLength % sizeof(char),
                "[MS-FSCC 2.5.9] VolumeLabel is a Unicode field, so VolumeLabelLength is expected to contain complete UTF-16 code units.");
            this.fsaAdapter.AssertAreEqual(
                this.Manager,
                SupportsObjects_Values.V1,
                volumeInformation.SupportsObjects,
                "[MS-FSA 2.1.5.13.1] SupportsObjects MUST be set to TRUE.");
            this.fsaAdapter.AssertAreEqual(
                this.Manager,
                (byte)0,
                volumeInformation.Reserved,
                "[MS-FSCC 2.5.9] Reserved MUST be set to zero.");
            this.fsaAdapter.AssertAreEqual(
                this.Manager,
                expectedByteCount,
                byteCount,
                "[MS-FSA 2.1.5.13.1] ByteCount MUST equal FieldOffset(FILE_FS_VOLUME_INFORMATION.VolumeLabel) plus BytesToCopy.");
            this.fsaAdapter.AssertAreEqual(
                this.Manager,
                expectedByteCount,
                (long)outputBuffer.Length,
                "The returned buffer length must match ByteCount.");

            uint minimumOutputBufferSize = BlockAlign(volumeLabelOffset, FileFsVolumeInformationAlignment);
            uint exactOutputBufferSize = Math.Max(minimumOutputBufferSize, (uint)expectedByteCount);

            BaseTestSite.Log.Add(LogEntryKind.TestStep, "4. Query again with exactly enough space for the complete response.");
            status = this.fsaAdapter.QueryFileSystemInformation(
                FileSystemInfoClass.File_FsVolumeInformation,
                exactOutputBufferSize,
                out long exactByteCount,
                out byte[] exactOutputBuffer);

            BaseTestSite.Log.Add(LogEntryKind.TestStep, "5. Verify STATUS_SUCCESS and the complete volume information.");
            this.fsaAdapter.AssertAreEqual(
                this.Manager,
                MessageStatus.SUCCESS,
                status,
                "[MS-FSA 2.1.5.13.1] The status MUST be STATUS_SUCCESS when BytesToCopy equals VolumeLabelLength.");
            BaseTestSite.Assert.IsNotNull(
                exactOutputBuffer,
                "The complete FILE_FS_VOLUME_INFORMATION response must be returned with STATUS_SUCCESS.");
            this.fsaAdapter.AssertAreEqual(
                this.Manager,
                expectedByteCount,
                exactByteCount,
                "[MS-FSA 2.1.5.13.1] ByteCount MUST equal FieldOffset(FILE_FS_VOLUME_INFORMATION.VolumeLabel) plus BytesToCopy.");
            this.fsaAdapter.AssertAreEqual(
                this.Manager,
                true,
                outputBuffer.AsSpan().SequenceEqual(exactOutputBuffer),
                "[MS-FSA 2.1.5.13.1] VolumeCreationTime, VolumeSerialNumber, VolumeLabelLength, SupportsObjects, and VolumeLabel MUST describe the same open file volume.");
        }

        private FileFsVolumeInformation QueryCompleteFileFsVolumeInformation(
            out long byteCount,
            out byte[] outputBuffer)
        {
            MessageStatus status = this.fsaAdapter.QueryFileSystemInformation(
                FileSystemInfoClass.File_FsVolumeInformation,
                this.fsaAdapter.transBufferSize,
                out byteCount,
                out outputBuffer);

            this.fsaAdapter.AssertAreEqual(
                this.Manager,
                MessageStatus.SUCCESS,
                status,
                "Querying FileFsVolumeInformation with a complete output buffer is expected to succeed.");
            BaseTestSite.Assert.IsNotNull(
                outputBuffer,
                "The complete FILE_FS_VOLUME_INFORMATION response must not be null.");

            return TypeMarshal.ToStruct<FileFsVolumeInformation>(outputBuffer);
        }

        private static uint GetFileFsVolumeInformationVolumeLabelOffset()
        {
            FileFsVolumeInformation volumeInformation = new FileFsVolumeInformation
            {
                VolumeLabel = Array.Empty<byte>()
            };

            return (uint)TypeMarshal.ToBytes(volumeInformation).Length;
        }

        private static uint BlockAlign(uint value, uint alignment)
        {
            return ((value + alignment - 1) / alignment) * alignment;
        }

        #endregion
    }
}
