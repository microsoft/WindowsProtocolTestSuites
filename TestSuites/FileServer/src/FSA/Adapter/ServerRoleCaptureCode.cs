// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Diagnostics.CodeAnalysis;

using Microsoft.Protocols.TestTools;
using Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Fscc;

namespace Microsoft.Protocols.TestSuites.FileSharing.FSA.Adapter
{
    /// <summary>
    /// Adapter requirements capture code for MS-FSA server role.
    /// </summary>
    public partial class FSAAdapter : ManagedAdapterBase, IFSAAdapter
    {
        /// <summary>
        /// Verify if FSCTL_CREATE_OR_GET_OBJECT_ID is implemented
        /// </summary>
        /// <param name="isImplemented"> A boolean value to verify whether FSCTL_CREATE_OR_GET_OBJECT_ID is implemented by the NTFS file system</param>
        private void VerifyFsctlCreateOrGetObjectId(bool isImplemented)
        {
            if (this.fileSystem == FileSystem.NTFS)
            {
                //
                // Add the debug information
                //
                Site.Log.Add(LogEntryKind.Debug,
                    "Verify MS-FSA_R57064 Actual filesystem: {0}, expected filesystem: {1}",
                    this.fileSystem,
                    FileSystem.NTFS);

                //
                // Verify MS-FSA requirement: MS-FSA_R57046
                //
                Site.CaptureRequirementIfIsTrue(
                    isImplemented,
                    57046,
                    @"[In FSCTL_CREATE_OR_GET_OBJECT_ID] <16> Section 3.1.5.9.1: This is implemented by the NTFS file system.");
            }
        }

        /// <summary>
        /// Verify if FSCTL_DELETE_OBJECT_ID is implemented
        /// </summary>
        /// <param name="isImplemented"> A boolean value to verify whether FSCTL_DELETE_OBJECT_ID is implemented by the NTFS file system</param>
        private void VerifyFsctlDeleteObjectId(bool isImplemented)
        {
            if (this.fileSystem == FileSystem.NTFS)
            {
                //
                // Add the debug information
                //
                Site.Log.Add(LogEntryKind.Debug,
                    "Verify MS-FSA_R57050 Actual filesystem: {0},expected filesystem: {1}",
                    this.fileSystem,
                    FileSystem.NTFS);

                //
                // Verify MS-FSA requirement: MS-FSA_R57050
                //
                Site.CaptureRequirementIfIsTrue(
                    isImplemented,
                    57050,
                    @"[In FSCTL_DELETE_OBJECT_ID ] <19> Section 3.1.5.9.2: This is implemented by the NTFS file system.");
            }
        }

        /// <summary>
        /// Verify if FSCTL_GET_COMPRESSION is implemented
        /// </summary>
        /// <param name="isImplemented"> A boolean value to verify whether FSCTL_GET_COMPRESSION is implemented by the NTFS file system</param>
        private void VerifyFsctlGetCompression(bool isImplemented)
        {
            if (this.fileSystem == FileSystem.NTFS)
            {
                //
                // Add the debug information
                //
                Site.Log.Add(LogEntryKind.Debug,
                    "Verify MS-FSA_R57061 Actual filesystem: {0},expected filesystem: {1}",
                    this.fileSystem, 
                    FileSystem.NTFS);

                //
                // Verify MS-FSA requirement: MS-FSA_R57061
                //
                Site.CaptureRequirementIfIsTrue(
                    isImplemented, 
                    57061, 
                    @"[In FSCTL_GET_COMPRESSION] <26> Section 3.1.5.9.6: This is implemented by the NTFS file system.");
            }
        }

        /// <summary>
        /// Verify if FSCTL_GET_NTFS_VOLUME_DATA is implemented
        /// </summary>
        /// <param name="isImplemented">A boolean value to verify whether FSCTL_GET_NTFS_VOLUME_DATA is implemented by the NTFS file system</param>
        private void VerifyFsclGetNtfsVolumeData(bool isImplemented)
        {
            if (this.fileSystem == FileSystem.NTFS && isWindows)
            {
                //
                // Add the debug information
                //
                Site.Log.Add(LogEntryKind.Debug,
                    "Verify MS-FSA_R57062 Actual filesystem: {0},expected filesystem: {1}",
                    this.fileSystem,
                    FileSystem.NTFS);

                //
                // Verify MS-FSA requirement: MS-FSA_R57062
                //
                Site.CaptureRequirementIfIsTrue(
                    isImplemented,
                    57062,
                    @"[In FSCTL_GET_NTFS_VOLUME_DATA] <27> Section 3.1.5.9.7: This is implemented by the NTFS file system.");
            }
        }

        /// <summary>
        /// Verify if FSCTL_QUERY_ALLOCATED_RANGES is implemented
        /// </summary>
        /// <param name="isImplemented"> A boolean value to verify whether FSCTL_QUERY_ALLOCATED_RANGES is implemented by the NTFS file system</param>
        private void VerifyFsctlQueryAllocatedRanges(bool isImplemented)
        {
            if (this.fileSystem == FileSystem.NTFS)
            {
                //
                // Add the debug information
                //
                Site.Log.Add(LogEntryKind.Debug,
                    "Verify MS-FSA_R57069 Actual filesystem: {0},expected filesystem: {1}",
                    this.fileSystem,
                    FileSystem.NTFS);

                //
                // Verify MS-FSA requirement: MS-FSA_R57069
                //
                Site.CaptureRequirementIfIsTrue(
                    isImplemented,
                    57069,
                    @"[In FSCTL_QUERY_ALLOCATED_RANGES]<33> Section 3.1.5.9.15: This is implemented by the NTFS file system.");
            }
        }

        /// <summary>
        /// Verify if FSCTL_READ_FILE_USN_DATA is implemented
        /// </summary>
        /// <param name="isImplemented"> A boolean value to verify whether FSCTL_READ_FILE_USN_DATA is implemented by the NTFS file system</param>
        private void VerifyFsctlReadFileUsnData(bool isImplemented)
        {
            if (this.fileSystem == FileSystem.NTFS)
            {
                //
                // Add the debug information
                //
                Site.Log.Add(LogEntryKind.Debug,
                    "Verify MS-FSA_R48002 Actual fileSystem: {0}, expected fileSystem: {1}",
                    this.fileSystem, 
                    FileSystem.NTFS);

                //
                // Verify MS-FSA requirement: MS-FSA_R48002
                //
                Site.CaptureRequirementIfIsTrue(
                    isImplemented, 
                    48002, 
                    @"[In FSCTL_READ_FILE_USN_DATA] <36> Section 3.1.5.9.18: This is implemented by the NTFS file system.");
            }
        }

        /// <summary>
        /// Verify if FSCTL_SET_SPARSE is implemented
        /// </summary>
        /// <param name="verifiedFileSystem">Verified file system</param>
        /// <param name="isImplemented"> A boolean value to verify whether FSCTL_SET_SPARSE is implemented by the NTFS file system</param>
        public void VerifyFsctlSetSparse(FileSystem verifiedFileSystem, bool isImplemented)
        {
            if (verifiedFileSystem == FileSystem.NTFS && isWindows)
            {
                //
                // Add the debug information
                //
                Site.Log.Add(LogEntryKind.Debug,
                    "Verify MS-FSA_R46021");

                //
                // Verify MS-FSA requirement: MS-FSA_R46021
                //
                Site.CaptureRequirementIfIsTrue(
                    isImplemented, 
                    46021, 
                    @"[In FSCTL_SET_SPARSE] <49> Section 3.1.5.9.27: This is implemented by the NTFS file system.");
            }
        }

        /// <summary>
        /// Verify if FSCTL_SET_ZERO_DATA is implemented
        /// </summary>
        /// <param name="isImplemented"> A boolean value to verify whether FSCTL_SET_ZERO_DATA is implemented by the NTFS file system</param>
        private void VerifyFsctlSetZeroData(bool isImplemented)
        {
            if (this.fileSystem == FileSystem.NTFS)
            {
                //
                // Add the debug information
                //
                Site.Log.Add(LogEntryKind.Debug,
                    "Verify MS-FSA_R46022 Actual fileSystem: {0}, expected fileSystem: {1}",
                    this.fileSystem, 
                    FileSystem.NTFS);

                //
                // Verify MS-FSA requirement: MS-FSA_R46022
                //
                Site.CaptureRequirementIfIsTrue(
                    isImplemented, 
                    46022, 
                    @"[In FSCTL_SET_ZERO_DATA]  <50> Section 3.1.5.9.28: This is implemented by the NTFS file system.");
            }
        }

        /// <summary>
        /// Verify file standard information 
        /// </summary>
        /// <param name="info">An instance of FileStandardInformation</param>
        /// <param name="status">An instance of MessageStatus</param>
        private void VerifyFileStandardInformation(FileStandardInformation info, MessageStatus status)
        {
            if (this.fileSystem == FileSystem.NTFS)
            {
                //
                // Add the debug information
                //
                Site.Log.Add(LogEntryKind.Debug, "Verify MS-FSA_R46036 Actual FileSystem: {0}", fileSystem);

                //
                // Verify MS-FSA requirement: MS-FSA_R46036
                //
                Site.CaptureRequirementIfAreEqual <MessageStatus>(
                    MessageStatus.SUCCESS,
                    status,
                    46036,
                    @"[In FileStandardInformation] <58> Section 3.1.5.11.27: This algorithm is implemented by NTFS. ");
            }

            if (this.fileSystem == FileSystem.FAT
                || this.fileSystem == FileSystem.EXFAT
                || this.fileSystem == FileSystem.CDFS
                || this.fileSystem == FileSystem.UDFS)
            {
                //
                // Add the debug information
                //
                Site.Log.Add(LogEntryKind.Debug, "Verify MS-FSA_R4637");

                //
                // Verify MS-FSA requirement: MS-FSA_R4637
                //
                Site.CaptureRequirementIfAreEqual<uint>(
                    1,
                    info.NumberOfLinks,
                    4637,
                    @"[In FileStandardInformation] <58> Section 3.1.5.11.27:The FAT, EXFAT, CDFS, and UDFS file systems always return 1.");
            }

            if (info.NumberOfLinks == 0)
            {
                //
                // Add the debug information
                //
                Site.Log.Add(LogEntryKind.Debug, "Verify MS-FSA_R2746");

                //
                // Verify MS-FSA requirement: MS-FSA_R2746
                //
                Site.CaptureRequirementIfAreEqual<byte>(
                    1,
                    info.DeletePending,
                    2746,
                    @"[In FileStandardInformation,Pseudocode for the operation is as follows:]If alignmentInfo.NumberOfLinks is 0, 
                    set alignmentInfo.DeletePending to 1.");
            }
        }

        /// <summary>
        /// Verify server request of setting File Information
        /// </summary>
        /// <param name="isReturnStatus">A boolean value to verify whether the requests of setting File Information store return:[Status]</param>
        private void VerifyServerSetFsInfo(bool isReturnStatus)
        {
            //
            // Add the debug information
            //
            Site.Log.Add(LogEntryKind.Debug, "Verify MS-FSA_R2853");

            //
            // Verify MS-FSA requirement: MS-FSA_R2853
            //
            Site.CaptureRequirementIfIsTrue(
                isReturnStatus,
                2853,
                @"[In Server Requests Setting of File Information ]The object store MUST return:[Status].");
        }

        /// <summary>
        /// Verify server requests to read
        /// </summary>
        /// <param name="isReturned">A boolean value to verify whether the request of server to read object store returns:[Status,alignmentInfo,BytesRead]</param>
        private void VerifyServerRequestsRead(bool isReturned)
        {
            //
            // Add the debug information
            //
            Site.Log.Add(LogEntryKind.Debug, "Verify MS-FSA_R663");

            //
            // Verify MS-FSA requirement: MS-FSA_R663
            //
            Site.CaptureRequirementIfIsTrue(
                isReturned,
                663,
                @"[In Server Requests a Read]On completion, the object store MUST return:[Status,alignmentInfo,BytesRead].");
        }

        /// <summary>
        /// Verify file alignment information
        /// </summary>
        /// <param name="alignmentInfo">Alignment information</param>
        private void VerifyFileAlignmentInformation(AlignmentRequirement alignmentInfo)
        {
            //
            // Add the debug information
            //
            Site.Log.Add(LogEntryKind.Debug, "Verify MS-FSA_R1434 Actual alignmentInfo: {0}", alignmentInfo);

            //
            // Verify MS-FSA requirement: MS-FSA_R1434
            //
            // To verify whether alignmentInfo defines AlignmentRequirement
            Site.CaptureRequirementIfIsTrue(
                (alignmentInfo == AlignmentRequirement.FileByteAlignment |
                alignmentInfo == AlignmentRequirement.FileWordAlignment |
                alignmentInfo == AlignmentRequirement.FileLongAlignment |
                alignmentInfo == AlignmentRequirement.FileQuadAlignment |
                alignmentInfo == AlignmentRequirement.FileOctaAlignment |
                alignmentInfo == AlignmentRequirement.File32ByteAlignment |
                alignmentInfo == AlignmentRequirement.File64ByteAlignment |
                alignmentInfo == AlignmentRequirement.File128ByteAlignment |
                alignmentInfo == AlignmentRequirement.File256ByteAlignment |
                alignmentInfo == AlignmentRequirement.File512ByteAlignment),
                1434,
                @"[In FileAlignmentInformation,Pseudocode for the operation is as follows:]alignmentInfo MUST be filled out as follows:
                alignmentInfo.AlignmentRequirement set to one of the alignment requirement values specified in [MS-FSCC] section 2.4.3 
                based on the characteristics of the device on which the File is stored.");
        }
    }
}