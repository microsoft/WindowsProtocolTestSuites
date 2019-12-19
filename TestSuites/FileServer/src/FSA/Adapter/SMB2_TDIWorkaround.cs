// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Protocols.TestTools;
using Microsoft.Modeling;

namespace Microsoft.Protocols.TestSuites.FileSharing.FSA.Adapter
{
    internal static class SMB2_TDIWorkaround
    {
        internal static MessageStatus WorkaroundFsCtlSetZeroData(FileSystem fileSystem, BufferSize bufferSize, InputBuffer_FSCTL_SET_ZERO_DATA inputBuffer, bool isIsDeletedTrue, bool isConflictDetected, MessageStatus returnedStatus, ITestSite site)
        {
            if (fileSystem != FileSystem.NTFS && fileSystem != FileSystem.REFS) 
            {
                returnedStatus = FsaUtility.TransferExpectedResult<MessageStatus>(4335, MessageStatus.INVALID_DEVICE_REQUEST, returnedStatus, site);
            }
            else if (isIsDeletedTrue &&
                bufferSize == BufferSize.BufferSizeSuccess &&
                (inputBuffer == InputBuffer_FSCTL_SET_ZERO_DATA.BufferSuccess || inputBuffer == InputBuffer_FSCTL_SET_ZERO_DATA.FileOffsetGreatThanBeyondFinalZero) )
            {
                returnedStatus = FsaUtility.TransferExpectedResult<MessageStatus>(1309, MessageStatus.FILE_DELETED, returnedStatus, site);
            }
            else if (isConflictDetected &&
                bufferSize == BufferSize.BufferSizeSuccess &&
                (inputBuffer == InputBuffer_FSCTL_SET_ZERO_DATA.BufferSuccess || inputBuffer == InputBuffer_FSCTL_SET_ZERO_DATA.FileOffsetGreatThanBeyondFinalZero))
            {
                returnedStatus = FsaUtility.TransferExpectedResult<MessageStatus>(1316, MessageStatus.FILE_LOCK_CONFLICT, returnedStatus, site);
            }
            return returnedStatus;
        }

        internal static MessageStatus WorkaroundSetFileRenameInfo(InputBufferFileNameLength inputBufferFileNameLength, MessageStatus returnedStatus, ITestSite site)
        {
            if (inputBufferFileNameLength == InputBufferFileNameLength.Greater)
            {
                returnedStatus = FsaUtility.TransferExpectedResult<MessageStatus>(3025, MessageStatus.MEDIA_WRITE_PROTECTED, returnedStatus, site);
            }
            return returnedStatus;
        }

        internal static MessageStatus WorkaroundQueryFileInfoPart1(FileSystem fileSystem, FileInfoClass fileInfoClass, OutputBufferSize outputBufferSize, ref ByteCount byteCount, ref OutputBuffer outputBuffer, MessageStatus returnedStatus, ITestSite site)
        {
            if (fileInfoClass == FileInfoClass.NOT_DEFINED_IN_FSCC || fileInfoClass == FileInfoClass.FILE_BOTH_DIR_INFORMATION
                || fileInfoClass == FileInfoClass.FILE_DIRECTORY_INFORMATION || fileInfoClass == FileInfoClass.FILE_FULL_DIR_INFORMATION
                || fileInfoClass == FileInfoClass.FILE_LINKS_INFORMATION || fileInfoClass == FileInfoClass.FILE_ID_BOTH_DIR_INFORMATION
                || fileInfoClass == FileInfoClass.FILE_ID_FULL_DIR_INFORMATION || fileInfoClass == FileInfoClass.FILE_ID_GLOBAL_TX_DIR_INFORMATION
                || fileInfoClass == FileInfoClass.FILE_NAME_INFORMATION || fileInfoClass == FileInfoClass.FILE_NAMES_INFORMATION
                || fileInfoClass == FileInfoClass.FILE_OBJECTID_INFORMATION || fileInfoClass == FileInfoClass.FILE_QUOTA_INFORMATION
                || fileInfoClass == FileInfoClass.FILE_REPARSE_POINT_INFORMATION || fileInfoClass == FileInfoClass.FILE_SFIO_RESERVE_INFORMATION
                || fileInfoClass == FileInfoClass.FILE_STANDARD_LINK_INFORMATION)
            {
                returnedStatus = FsaUtility.TransferExpectedResult<MessageStatus>(2749, MessageStatus.INVALID_INFO_CLASS, returnedStatus, site);
            }
            else if (fileInfoClass == FileInfoClass.FILE_ACCESS_INFORMATION && outputBufferSize == OutputBufferSize.NotLessThan)
            {
                outputBuffer = FsaUtility.TransferExpectedResult<OutputBuffer>(1421, new OutputBuffer(), outputBuffer, site);
            }
            else if (fileInfoClass == FileInfoClass.FILE_FULLEA_INFORMATION && outputBufferSize == OutputBufferSize.LessThan)
            {
                returnedStatus = FsaUtility.TransferExpectedResult<MessageStatus>(3994, MessageStatus.BUFFER_TOO_SMALL, returnedStatus, site);
            }
            else if (fileInfoClass == FileInfoClass.FILE_FULLEA_INFORMATION && 
                returnedStatus == MessageStatus.NO_EAS_ON_FILE)
            {
                // For query FILE_FULLEA_INFORMATION, when server returns STATUS_NO_EAS_ON_FILE, this result is valid according to model design.
                // Transfer the return code and byteCount to make model test cases passed.
                byteCount = FsaUtility.TransferExpectedResult<ByteCount>(3992, ByteCount.SizeofFILE_FULL_EA_INFORMATION, byteCount, site);
                returnedStatus = FsaUtility.TransferExpectedResult<MessageStatus>(1187, MessageStatus.SUCCESS, returnedStatus, site);
            }
            else if (fileInfoClass == FileInfoClass.FILE_STREAM_INFORMATION &&
                fileSystem == FileSystem.FAT32) 
            {
                returnedStatus = FsaUtility.TransferExpectedResult<MessageStatus>(1421, MessageStatus.SUCCESS, returnedStatus, site);
            }
            else if (fileInfoClass == FileInfoClass.FILE_COMPRESSION_INFORMATION &&
                fileSystem == FileSystem.FAT32)
            {
                if (outputBufferSize == OutputBufferSize.NotLessThan) 
                {
                    returnedStatus = FsaUtility.TransferExpectedResult<MessageStatus>(1421, MessageStatus.SUCCESS, returnedStatus, site);
                    byteCount = FsaUtility.TransferExpectedResult<ByteCount>(1421, ByteCount.SizeofFILE_COMPRESSION_INFORMATION, byteCount, site);
                }
                else if (outputBufferSize == OutputBufferSize.LessThan)
                {
                    returnedStatus = FsaUtility.TransferExpectedResult<MessageStatus>(1489, MessageStatus.INFO_LENGTH_MISMATCH, returnedStatus, site);
                    byteCount = FsaUtility.TransferExpectedResult<ByteCount>(1489, ByteCount.NotSet, byteCount, site);
                }
            }
            else if (fileInfoClass == FileInfoClass.FILE_ATTRIBUTETAG_INFORMATION &&
                fileSystem == FileSystem.FAT32)
            {
                if (outputBufferSize == OutputBufferSize.NotLessThan) 
                {
                    returnedStatus = FsaUtility.TransferExpectedResult<MessageStatus>(1421, MessageStatus.SUCCESS, returnedStatus, site);
                    byteCount = FsaUtility.TransferExpectedResult<ByteCount>(1421, ByteCount.SizeofFILE_ATTRIBUTE_TAG_INFORMATION, byteCount, site);
                }
                else if (outputBufferSize == OutputBufferSize.LessThan)
                {
                    returnedStatus = FsaUtility.TransferExpectedResult<MessageStatus>(1445, MessageStatus.INFO_LENGTH_MISMATCH, returnedStatus, site);
                    byteCount = FsaUtility.TransferExpectedResult<ByteCount>(1445, ByteCount.NotSet, byteCount, site);
                }
            }
            else if (fileInfoClass == FileInfoClass.FILE_FULLEA_INFORMATION && 
                fileSystem != FileSystem.NTFS &&
                outputBufferSize == OutputBufferSize.NotLessThan)
            {
                // FILE_FULL_EA_INFORMATION is only supported in NTFS, will failed with STATUS_INVALID_DEVICE_REQUEST in other file systems.
                // Transfer the return code and byteCount to make model test cases passed.
                byteCount = FsaUtility.TransferExpectedResult<ByteCount>(3992, ByteCount.SizeofFILE_FULL_EA_INFORMATION, byteCount, site);
                returnedStatus = FsaUtility.TransferExpectedResult<MessageStatus>(1187, MessageStatus.SUCCESS, returnedStatus, site);
            }
            else if (fileInfoClass == FileInfoClass.FILE_ALTERNATENAME_INFORMATION &&
                fileSystem == FileSystem.REFS &&
                outputBufferSize == OutputBufferSize.NotLessThan)
            {
                // REFS file system does not support FILE_ALTERNATENAME_INFORMATION, will failed with STATUS_OBJECT_NAME_NOT_FOUND
                // Transfer the return code and byteCount to make model test cases passed.
                byteCount = FsaUtility.TransferExpectedResult<ByteCount>(3992, ByteCount.FieldOffsetFILE_NAME_INFORMATION_FileNameAddOutputBuffer_FileNameLength, byteCount, site);
                returnedStatus = FsaUtility.TransferExpectedResult<MessageStatus>(1187, MessageStatus.SUCCESS, returnedStatus, site);
            }
            return returnedStatus;
        }

        internal static MessageStatus WorkaroundFsCtlSetObjID(FsControlRequestType requestType, BufferSize bufferSize, MessageStatus returnedStatus, ITestSite site)
        {
            if (returnedStatus != MessageStatus.INVALID_DEVICE_REQUEST)
            {
                if (requestType == FsControlRequestType.SET_OBJECT_ID_EXTENDED && bufferSize == BufferSize.NotEqualFILE_OBJECTID_BUFFER)
                {
                    returnedStatus = FsaUtility.TransferExpectedResult<MessageStatus>(1206, MessageStatus.SUCCESS, returnedStatus, site);
                }
                else if ((requestType == FsControlRequestType.SET_OBJECT_ID || requestType == FsControlRequestType.SET_OBJECT_ID_EXTENDED)
                    && bufferSize == BufferSize.BufferSizeSuccess)
                {
                    returnedStatus = FsaUtility.TransferExpectedResult<MessageStatus>(1187, MessageStatus.SUCCESS, returnedStatus, site);
                }
            }
            return returnedStatus;
        }

        internal static MessageStatus WorkaroundFsCtlForEasyRequest(FileSystem fileSystem, FsControlRequestType requestType, BufferSize bufferSize, bool fileVolReadOnly, bool fileVolUsnAct, ref bool isBytesReturnedSet, ref bool isOutputBufferSizeReturn, MessageStatus returnedStatus, ITestSite site)
        {
            if (requestType == FsControlRequestType.RECALL_FILE)
            {
                returnedStatus = FsaUtility.TransferExpectedResult<MessageStatus>(1136, MessageStatus.SUCCESS, returnedStatus, site);
            }
            else if (requestType == FsControlRequestType.FSCTL_SET_SHORT_NAME_BEHAVIOR)
            {
                returnedStatus = FsaUtility.TransferExpectedResult<MessageStatus>(1274, MessageStatus.INVALID_DEVICE_REQUEST, returnedStatus, site);
            }
            else if (requestType == FsControlRequestType.WRITE_USN_CLOSE_RECORD && bufferSize == BufferSize.LessThanTwoBytes && !fileVolReadOnly && fileVolUsnAct)
            {
                isBytesReturnedSet = FsaUtility.TransferExpectedResult<bool>(3966, true, isBytesReturnedSet, site);
            }
            else if (requestType == FsControlRequestType.SET_ZERO_ON_DEALLOCATION && bufferSize == BufferSize.LessThanSizeofUsn)
            {
                isBytesReturnedSet = FsaUtility.TransferExpectedResult<bool>(1388, false, isBytesReturnedSet, site);
            }
            else if (requestType == FsControlRequestType.SET_OBJECT_ID && bufferSize == BufferSize.LessThan0x24 && !fileVolReadOnly && !fileVolUsnAct)
            {
                isBytesReturnedSet = FsaUtility.TransferExpectedResult<bool>(1068, true, isBytesReturnedSet, site);
            }
            else if (requestType == FsControlRequestType.QUERY_FAT_BPB && bufferSize == BufferSize.LessThanTotalSizeOfStatistics)
            {
                isBytesReturnedSet = FsaUtility.TransferExpectedResult<bool>(1121, true, isBytesReturnedSet, site);
            }
            else if (requestType == FsControlRequestType.QUERY_ON_DISK_VOLUME_INFO && bufferSize == BufferSize.LessThanFILE_QUERY_SPARING_BUFFER && fileVolReadOnly && fileVolUsnAct)
            {
                isBytesReturnedSet = FsaUtility.TransferExpectedResult<bool>(5522, true, isBytesReturnedSet, site);
            }
            else if (requestType == FsControlRequestType.WRITE_USN_CLOSE_RECORD && 
                returnedStatus == MessageStatus.JOURNAL_NOT_ACTIVE &&
                fileSystem == FileSystem.NTFS)
            {
                returnedStatus = FsaUtility.TransferExpectedResult<MessageStatus>(1274, MessageStatus.SUCCESS, returnedStatus, site);
                isBytesReturnedSet = FsaUtility.TransferExpectedResult<bool>(3966, true, isBytesReturnedSet, site);
                isOutputBufferSizeReturn = FsaUtility.TransferExpectedResult<bool>(3966, true, isOutputBufferSizeReturn, site);
            }
            else if (fileSystem != FileSystem.NTFS &&
                requestType == FsControlRequestType.SET_ZERO_ON_DEALLOCATION)
            {
                returnedStatus = FsaUtility.TransferExpectedResult<MessageStatus>(4721, MessageStatus.INVALID_DEVICE_REQUEST, returnedStatus, site);
                isBytesReturnedSet = FsaUtility.TransferExpectedResult<bool>(4721, false, isBytesReturnedSet, site);
                isOutputBufferSizeReturn = FsaUtility.TransferExpectedResult<bool>(4721, false, isOutputBufferSizeReturn, site);
            }
            else if (fileSystem == FileSystem.FAT32 &&
                requestType == FsControlRequestType.FSCTL_FILESYSTEM_GET_STATISTICS)
            {
                returnedStatus = FsaUtility.TransferExpectedResult<MessageStatus>(5803, MessageStatus.INVALID_DEVICE_REQUEST, returnedStatus, site);
                isBytesReturnedSet = FsaUtility.TransferExpectedResult<bool>(5803, false, isBytesReturnedSet, site);
                isOutputBufferSizeReturn = FsaUtility.TransferExpectedResult<bool>(5803, false, isOutputBufferSizeReturn, site);
            }
            return returnedStatus;
        }

        internal static MessageStatus WorkaroundFsctlSisCopy(FileSystem fileSystem, BufferSize bufferSize, InputBufferFSCTL_SIS_COPYFILE inputBuffer, bool isCOPYFILE_SIS_LINKTrue, bool isIsEncryptedTrue, MessageStatus returnedStatus, ITestSite site)
        {
            if (fileSystem == FileSystem.FAT32) 
            {
                returnedStatus = FsaUtility.TransferExpectedResult<MessageStatus>(4732, MessageStatus.INVALID_DEVICE_REQUEST, returnedStatus, site);
            }
            else if (bufferSize == BufferSize.BufferSizeSuccess && 
                inputBuffer == InputBufferFSCTL_SIS_COPYFILE.Initial && 
                isCOPYFILE_SIS_LINKTrue == false && 
                isIsEncryptedTrue == false)
            {
                returnedStatus = FsaUtility.TransferExpectedResult<MessageStatus>(4773, MessageStatus.SUCCESS, returnedStatus, site);
            }
            else if ((bufferSize == BufferSize.LessThanSI_COPYFILE) || 
                (inputBuffer == InputBufferFSCTL_SIS_COPYFILE.FlagsNotContainCOPYFILE_SIS_LINKAndCOPYFILE_SIS_REPLACE) || 
                (inputBuffer == InputBufferFSCTL_SIS_COPYFILE.DestinationFileNameLengthLessThanZero) || 
                (inputBuffer == InputBufferFSCTL_SIS_COPYFILE.DestinationFileNameLengthLargeThanMAXUSHORT) || 
                (inputBuffer == InputBufferFSCTL_SIS_COPYFILE.InputBufferSizeLessThanOtherPlus))
            {
                returnedStatus = FsaUtility.TransferExpectedResult<MessageStatus>(4734, MessageStatus.INVALID_PARAMETER, returnedStatus, site);
            }
            else if (isCOPYFILE_SIS_LINKTrue || isIsEncryptedTrue)
            {
                returnedStatus = FsaUtility.TransferExpectedResult<MessageStatus>(4755, MessageStatus.OBJECT_TYPE_MISMATCH, returnedStatus, site);
            }
            return returnedStatus;
        }

        internal static MessageStatus WorkaroundSetFileShortNameInfo(InputBufferFileName inputBufferFileName, MessageStatus returnedstatus, ITestSite site)
        {
            if (inputBufferFileName == InputBufferFileName.StartWithBackSlash)
            {
                returnedstatus = FsaUtility.TransferExpectedResult<MessageStatus>(3173, MessageStatus.INVALID_PARAMETER, returnedstatus, site);
            }
            else if (inputBufferFileName == InputBufferFileName.NotValid)
            {
                returnedstatus = FsaUtility.TransferExpectedResult<MessageStatus>(3176, MessageStatus.INVALID_PARAMETER, returnedstatus, site);
            }
            else if (inputBufferFileName == InputBufferFileName.Empty)
            {
                returnedstatus = FsaUtility.TransferExpectedResult<MessageStatus>(3180, MessageStatus.ACCESS_DENIED, returnedstatus, site);
            }

            return returnedstatus;
        }

        internal static MessageStatus WorkaroundStreamRename(FileSystem fileSystem, InputBufferFileName NewStreamName, InputBufferFileName StreamTypeName, bool ReplaceIfExists, MessageStatus returnedStatus, ITestSite site)
        {
            if (fileSystem == FileSystem.REFS || fileSystem == FileSystem.FAT32)
            {
                returnedStatus = FsaUtility.TransferExpectedResult<MessageStatus>(3146, MessageStatus.NOT_SUPPORTED, returnedStatus, site);
            }
            else if (NewStreamName == InputBufferFileName.ContainsWildcard)
            {
                returnedStatus = FsaUtility.TransferExpectedResult<MessageStatus>(3146, MessageStatus.INVALID_PARAMETER, returnedStatus, site);
            }
            return returnedStatus;
        }

        internal static MessageStatus WorkaroundQueryDirectoryInfo(FileNamePattern fileNamePattern, bool isNoRecordsReturned, bool isOutBufferSizeLess, MessageStatus returnedStatus, ITestSite site)
        {
            if (isOutBufferSizeLess)
            {
                returnedStatus = FsaUtility.TransferExpectedResult<MessageStatus>(4836, MessageStatus.INFO_LENGTH_MISMATCH, returnedStatus, site);
            }
            else if (fileNamePattern == FileNamePattern.NotValidFilenameComponent)
            {
                returnedStatus = FsaUtility.TransferExpectedResult<MessageStatus>(849, MessageStatus.OBJECT_NAME_INVALID, returnedStatus, site);
            }
            else if (isNoRecordsReturned)
            {
                returnedStatus = FsaUtility.TransferExpectedResult<MessageStatus>(865, MessageStatus.NO_SUCH_FILE, returnedStatus, site);
            }
            return returnedStatus;
        }

        internal static MessageStatus WorkaroundSetFileFullEaInfo(EainInputBuffer eAValidate, MessageStatus returnedStatus, ITestSite site)
        {
            returnedStatus = FsaUtility.TransferExpectedResult<MessageStatus>(2853, MessageStatus.SUCCESS, returnedStatus, site);

            if (eAValidate == EainInputBuffer.EaNameExistinOpenFileExtendedAttribute)
            {
                returnedStatus = FsaUtility.TransferExpectedResult<MessageStatus>(2853, MessageStatus.SUCCESS, returnedStatus, site);
            }
            else if (eAValidate == EainInputBuffer.EaNameNotWellForm || eAValidate == EainInputBuffer.EaFlagsInvalid)
            {
                returnedStatus = FsaUtility.TransferExpectedResult<MessageStatus>(2927, MessageStatus.INVALID_EA_NAME, returnedStatus, site);
            }
            return returnedStatus;
        }

        internal static bool WorkaroundFsCtlGetReparsePoint(BufferSize bufferSize, ReparseTag openFileReparseTag, bool isBytesReturnedSet, ref MessageStatus returnedStatus, ITestSite site)
        {
            if ((bufferSize == BufferSize.BufferSizeSuccess && openFileReparseTag == ReparseTag.NON_MICROSOFT_RANGE_TAG) ||
                (bufferSize == BufferSize.LessThanREPARSE_DATA_BUFFER && openFileReparseTag == ReparseTag.NON_MICROSOFT_RANGE_TAG) || (bufferSize == BufferSize.BufferSizeSuccess && openFileReparseTag == ReparseTag.IO_REPARSE_TAG_RESERVED_ZERO))
            {
                isBytesReturnedSet = FsaUtility.TransferExpectedResult<bool>(1091, true, isBytesReturnedSet, site);
                if (returnedStatus == MessageStatus.NOT_A_REPARSE_POINT)
                {
                    // If the open file is not a reparse point, SMB2 server will return STATUS_NOT_A_REPARSE_POINT
                    // This is acceptable in model and expect as STATUS_SUCCESS.
                    returnedStatus = FsaUtility.TransferExpectedResult<MessageStatus>(2853, MessageStatus.SUCCESS, returnedStatus, site);
                }
            }
            else if (openFileReparseTag == ReparseTag.EMPTY)
            {
                return isBytesReturnedSet;
            }
            else if (((openFileReparseTag != ReparseTag.NON_MICROSOFT_RANGE_TAG) && (BufferSize.LessThanREPARSE_DATA_BUFFER == bufferSize)) ||
                ((openFileReparseTag == ReparseTag.NON_MICROSOFT_RANGE_TAG) && (BufferSize.LessThanREPARSE_GUID_DATA_BUFFER == bufferSize)))
            {
                returnedStatus = FsaUtility.TransferExpectedResult<MessageStatus>(1083, MessageStatus.BUFFER_TOO_SMALL, returnedStatus, site);
            }
            else if (bufferSize == BufferSize.LessThanREPARSE_GUID_DATA_BUFFER && openFileReparseTag == ReparseTag.IO_REPARSE_TAG_RESERVED_ZERO)
            {
                isBytesReturnedSet = FsaUtility.TransferExpectedResult<bool>(1091, true, isBytesReturnedSet, site);
            }
            return isBytesReturnedSet;
        }

        internal static MessageStatus WorkaroundFsCtlSetReparsePoint(FileSystem fileSystem, ReparseTag inputReparseTag, BufferSize bufferSize, bool isReparseGUIDNotEqual, bool isFileReparseTagNotEqualInputBufferReparseTag, MessageStatus returnedStatus, ITestSite site)
        {
            if (fileSystem != FileSystem.NTFS && fileSystem != FileSystem.REFS) 
            {
                returnedStatus = FsaUtility.TransferExpectedResult<MessageStatus>(4331, MessageStatus.INVALID_DEVICE_REQUEST, returnedStatus, site);
            }
            else if ((inputReparseTag == ReparseTag.SYMLINK))
            {
                returnedStatus = FsaUtility.TransferExpectedResult<MessageStatus>(1231, MessageStatus.ACCESS_DENIED, returnedStatus, site);
            }
            else if (returnedStatus == MessageStatus.INVALID_PARAMETER)
            {
                if (inputReparseTag != ReparseTag.EMPTY)
                {
                    if (isFileReparseTagNotEqualInputBufferReparseTag)
                    {
                        returnedStatus = FsaUtility.TransferExpectedResult<MessageStatus>(1237, MessageStatus.IO_REPARSE_TAG_MISMATCH, returnedStatus, site);
                    }
                    else if ((inputReparseTag == ReparseTag.NON_MICROSOFT_RANGE_TAG) && (!isReparseGUIDNotEqual))
                    {
                        returnedStatus = FsaUtility.TransferExpectedResult<MessageStatus>(1238, MessageStatus.REPARSE_ATTRIBUTE_CONFLICT, returnedStatus, site);
                    }
                }
                else
                {
                    returnedStatus = FsaUtility.TransferExpectedResult<MessageStatus>(1246, MessageStatus.SUCCESS, returnedStatus, site);
                }
            }

            return returnedStatus;
        }

        internal static MessageStatus WorkaroundSetFileLinkInfo(bool inputNameInvalid, bool replaceIfExist, MessageStatus returnedStatus, ITestSite site)
        {
            if (inputNameInvalid == false && replaceIfExist == true)
            {
                returnedStatus = FsaUtility.TransferExpectedResult<MessageStatus>(2974, MessageStatus.SUCCESS, returnedStatus, site);
            }
            else if (inputNameInvalid)
            {
                returnedStatus = FsaUtility.TransferExpectedResult<MessageStatus>(2941, MessageStatus.OBJECT_NAME_INVALID, returnedStatus, site);
            }
            else if (!replaceIfExist)
            {
                returnedStatus = FsaUtility.TransferExpectedResult<MessageStatus>(2954, MessageStatus.OBJECT_NAME_COLLISION, returnedStatus, site);
            }
            return returnedStatus;
        }

        internal static MessageStatus WorkAroundSetFilePositionInfo(InputBufferSize inputBufferSize, InputBufferCurrentByteOffset currentByteOffset, MessageStatus returnedStatus, ITestSite site)
        {
            if (inputBufferSize != InputBufferSize.LessThan && currentByteOffset == InputBufferCurrentByteOffset.NotValid)
            {
                // [MS-SMB2] Section 3.3.5.20.1   Handling SMB2_0_INFO_FILE
                // If the request is for the FilePositionInformation information class, the SMB2 server SHOULD (348) set the CurrentByteOffset field to zero. 
                // (348) Section 3.3.5.20.1: Windows-based SMB2 servers will set CurrentByteOffset to any value.
                // Per tested with Win2012/Win2012R2 SMB2 server, they return STATUS_SUCCESS.
                // To keep same model behavior according to MS-FSA, transfer the error code to STATUS_INVALID_PARAMETER
                returnedStatus = FsaUtility.TransferExpectedResult<MessageStatus>(3004, MessageStatus.INVALID_PARAMETER, returnedStatus, site);
            }
            return returnedStatus;
        }

        internal static MessageStatus WorkaroundCreateFile(FileNameStatus fileNameStatus, CreateOptions createOption, FileAccess desiredAccess, FileType openFileType, FileAttribute desiredFileAttribute, MessageStatus returnedStatus, ITestSite site)
        {
            if (createOption == CreateOptions.SYNCHRONOUS_IO_ALERT
               && desiredAccess == FileAccess.FILE_READ_DATA)
            {
                returnedStatus = FsaUtility.TransferExpectedResult<MessageStatus>(369, MessageStatus.INVALID_PARAMETER, returnedStatus, site);
            }
            else if (createOption == CreateOptions.SYNCHRONOUS_IO_NONALERT
               && desiredAccess == FileAccess.FILE_READ_DATA)
            {
                returnedStatus = FsaUtility.TransferExpectedResult<MessageStatus>(2373, MessageStatus.INVALID_PARAMETER, returnedStatus, site);
            }
            else if (createOption == CreateOptions.DELETE_ON_CLOSE &&
                (desiredAccess == FileAccess.ACCESS_SYSTEM_SECURITY))
            {
                returnedStatus = FsaUtility.TransferExpectedResult<MessageStatus>(371, MessageStatus.INVALID_PARAMETER, returnedStatus, site);
            }
            else if (createOption == (CreateOptions.SYNCHRONOUS_IO_NONALERT | CreateOptions.SYNCHRONOUS_IO_ALERT))
            {
                returnedStatus = FsaUtility.TransferExpectedResult<MessageStatus>(373, MessageStatus.INVALID_PARAMETER, returnedStatus, site);
            }
            else if (createOption == (CreateOptions.COMPLETE_IF_OPLOCKED | CreateOptions.RESERVE_OPFILTER))
            {
                returnedStatus = FsaUtility.TransferExpectedResult<MessageStatus>(375, MessageStatus.INVALID_PARAMETER, returnedStatus, site);
            }
            else if (fileNameStatus == FileNameStatus.StreamTypeNameIsINDEX_ALLOCATION)
            {
                returnedStatus = FsaUtility.TransferExpectedResult<MessageStatus>(507, MessageStatus.INVALID_PARAMETER, returnedStatus, site);
            }
            else if (createOption == CreateOptions.NO_INTERMEDIATE_BUFFERING &&
                (desiredAccess == FileAccess.FILE_APPEND_DATA || desiredAccess == FileAccess.FILE_ADD_SUBDIRECTORY))
            {
                returnedStatus = FsaUtility.TransferExpectedResult<MessageStatus>(376, MessageStatus.INVALID_PARAMETER, returnedStatus, site);
            }
            return returnedStatus;
        }

        internal static MessageStatus WorkaroundOpenExistingFile(ShareAccess shareAccess, FileAccess desiredAccess,
            bool streamFound, bool isSymbolicLink, FileType openFileType, FileNameStatus fileNameStatus,
            CreateOptions existingOpenModeCreateOption, ShareAccess existOpenShareModeShareAccess,
            FileAccess existOpenDesiredAccess, CreateOptions createOption, CreateDisposition createDisposition,
            StreamTypeNameToOpen streamTypeNameToOPen, FileAttribute fileAttribute,
            FileAttribute desiredFileAttribute, MessageStatus returnedStatus, ITestSite site)
        {
            if (shareAccess == ShareAccess.FILE_SHARE_READ && desiredAccess == FileAccess.FILE_ADD_SUBDIRECTORY
                && !streamFound && !isSymbolicLink && openFileType == FileType.DataFile && fileNameStatus == FileNameStatus.Normal
                && existingOpenModeCreateOption == CreateOptions.NON_DIRECTORY_FILE && existOpenShareModeShareAccess == ShareAccess.FILE_SHARE_READ
                && existOpenDesiredAccess == FileAccess.FILE_LIST_DIRECTORY && createOption == CreateOptions.NO_INTERMEDIATE_BUFFERING
                && createDisposition == CreateDisposition.OPEN_IF && streamTypeNameToOPen == StreamTypeNameToOpen.NULL
                && fileAttribute == FileAttribute.NORMAL && desiredFileAttribute == FileAttribute.NORMAL)
            {
                returnedStatus = FsaUtility.TransferExpectedResult<MessageStatus>(376, MessageStatus.INVALID_PARAMETER, returnedStatus, site);
            }
            else if (createOption == (CreateOptions.SYNCHRONOUS_IO_NONALERT | CreateOptions.SYNCHRONOUS_IO_ALERT))
            {
                returnedStatus = FsaUtility.TransferExpectedResult<MessageStatus>(373, MessageStatus.INVALID_PARAMETER, returnedStatus, site);
            }
            else if (createOption == CreateOptions.SYNCHRONOUS_IO_ALERT
               && desiredAccess == FileAccess.FILE_READ_DATA)
            {
                returnedStatus = FsaUtility.TransferExpectedResult<MessageStatus>(369, MessageStatus.INVALID_PARAMETER, returnedStatus, site);
            }
            else if (createOption == CreateOptions.SYNCHRONOUS_IO_NONALERT 
                && desiredAccess == FileAccess.FILE_READ_DATA)
            {
                returnedStatus = FsaUtility.TransferExpectedResult<MessageStatus>(2373, MessageStatus.INVALID_PARAMETER, returnedStatus, site);
            }
            else if (createOption == CreateOptions.DELETE_ON_CLOSE &&
                (desiredAccess == FileAccess.ACCESS_SYSTEM_SECURITY))
            {
                returnedStatus = FsaUtility.TransferExpectedResult<MessageStatus>(371, MessageStatus.INVALID_PARAMETER, returnedStatus, site);
            }
            else if (createOption == (CreateOptions.COMPLETE_IF_OPLOCKED | CreateOptions.RESERVE_OPFILTER))
            {
                returnedStatus = FsaUtility.TransferExpectedResult<MessageStatus>(375, MessageStatus.INVALID_PARAMETER, returnedStatus, site);
            }
            else if (streamFound && !isSymbolicLink && openFileType == FileType.DataFile &&
                existingOpenModeCreateOption == CreateOptions.DIRECTORY_FILE &&
                existOpenDesiredAccess == FileAccess.FILE_LIST_DIRECTORY && 
                createOption == CreateOptions.DIRECTORY_FILE)
            {
                returnedStatus = FsaUtility.TransferExpectedResult<MessageStatus>(375, MessageStatus.ACCESS_VIOLATION, returnedStatus, site);
            }
            else if (!streamFound && !isSymbolicLink && openFileType == FileType.DataFile &&
                existingOpenModeCreateOption == CreateOptions.NON_DIRECTORY_FILE &&
                existOpenDesiredAccess == FileAccess.FILE_LIST_DIRECTORY &&
                createOption == CreateOptions.NON_DIRECTORY_FILE &&
                fileAttribute == FileAttribute.READONLY)
            {
                returnedStatus = FsaUtility.TransferExpectedResult<MessageStatus>(375, MessageStatus.ACCESS_DENIED, returnedStatus, site);
            }
            return returnedStatus;
        }

        internal static MessageStatus WorkaroundQueryFileObjectIdInfo(bool isObjectIDsSupported, FileNamePattern fileNamePattern,
            bool restartScan, 
            bool isDirectoryNotRight, 
            bool isOutPutBufferNotEnough, 
            MessageStatus returnedStatus, 
            ITestSite site)
        {
            if (!isObjectIDsSupported)
            {
                returnedStatus = FsaUtility.TransferExpectedResult<MessageStatus>(830, MessageStatus.INVALID_DEVICE_REQUEST, returnedStatus, site);
            }
            else if (fileNamePattern != FileNamePattern.NotEmpty_LengthIsNotAMultipleOf4 && 
                restartScan && !isDirectoryNotRight && !isOutPutBufferNotEnough)
            {
                returnedStatus = FsaUtility.TransferExpectedResult<MessageStatus>(830, MessageStatus.SUCCESS, returnedStatus, site);
            }
            else if ((!restartScan) && (fileNamePattern == FileNamePattern.Empty) && (isDirectoryNotRight))
            {
                returnedStatus = FsaUtility.TransferExpectedResult<MessageStatus>(830, MessageStatus.NO_MORE_FILES, returnedStatus, site);
            }

            else if ((fileNamePattern == FileNamePattern.NotEmpty_LengthIsNotAMultipleOf4)
                && (isDirectoryNotRight)
                && (!isOutPutBufferNotEnough))
            {
                returnedStatus = FsaUtility.TransferExpectedResult<MessageStatus>(830, MessageStatus.INVALID_PARAMETER, returnedStatus, site);
            }

            // EmptyPattern is FALSE and there is no match.
            else if ((!(fileNamePattern == FileNamePattern.Empty)) && (isDirectoryNotRight))
            {
                returnedStatus = FsaUtility.TransferExpectedResult<MessageStatus>(830, MessageStatus.NO_SUCH_FILE, returnedStatus, site);
            }

            // EmptyPattern is true and RestartScan is false and there is no match and output Buffer is not enough.
            else if ((fileNamePattern == FileNamePattern.Empty)
                && (!restartScan)
                && (!isDirectoryNotRight)
                && (!isOutPutBufferNotEnough))
            {
                returnedStatus = FsaUtility.TransferExpectedResult<MessageStatus>(830, MessageStatus.SUCCESS, returnedStatus, site);
            }

            // EmptyPattern is true and RestartScan is true and there is no match.
            else if ((fileNamePattern == FileNamePattern.Empty) && (restartScan) && (isDirectoryNotRight) && (!isOutPutBufferNotEnough))
            {
                returnedStatus = FsaUtility.TransferExpectedResult<MessageStatus>(830, MessageStatus.NO_SUCH_FILE, returnedStatus, site);
            }

            else if (isOutPutBufferNotEnough)
            {
                returnedStatus = FsaUtility.TransferExpectedResult<MessageStatus>(830, MessageStatus.INFO_LENGTH_MISMATCH, returnedStatus, site);
            }

            return returnedStatus;
        }

        internal static MessageStatus WorkaroundQueryFileReparsePointInfo(FileSystem fileSystem, FileNamePattern fileNamePattern,
            bool restartScan,
            bool isDirectoryNotRight,
            bool isOutPutBufferNotEnough,
            MessageStatus returnedStatus,
            ITestSite site)
        {
            bool EmptyPattern = false;
            if (fileNamePattern == FileNamePattern.Empty)
            {
                EmptyPattern = true;
            }
            else
            {
                EmptyPattern = false;
            }

            if (fileSystem != FileSystem.NTFS)
            {
                returnedStatus = FsaUtility.TransferExpectedResult<MessageStatus>(830, MessageStatus.INVALID_DEVICE_REQUEST, returnedStatus, site);
            }

            else if (fileNamePattern == FileNamePattern.NotEmpty_LengthIsNotAMultipleOf4)
            {
                returnedStatus = FsaUtility.TransferExpectedResult<MessageStatus>(830, MessageStatus.INVALID_PARAMETER, returnedStatus, site);
            }

            else if (!restartScan && EmptyPattern && isDirectoryNotRight)
            {                
                returnedStatus = FsaUtility.TransferExpectedResult<MessageStatus>(830, MessageStatus.NO_MORE_FILES, returnedStatus, site);
            }

            else if ((!EmptyPattern && isDirectoryNotRight) ||
                (EmptyPattern && restartScan && isDirectoryNotRight))
            {
                returnedStatus = FsaUtility.TransferExpectedResult<MessageStatus>(830, MessageStatus.NO_SUCH_FILE, returnedStatus, site);
            }

            else if (isOutPutBufferNotEnough)
            {
                returnedStatus = FsaUtility.TransferExpectedResult<MessageStatus>(830, MessageStatus.INFO_LENGTH_MISMATCH, returnedStatus, site);
            }

            //If there is at least one match, the operation is considered successful
            else if (!isDirectoryNotRight)
            {
                returnedStatus = FsaUtility.TransferExpectedResult<MessageStatus>(830, MessageStatus.SUCCESS, returnedStatus, site);
            }

            return returnedStatus;
        }

        internal static MessageStatus WorkaroundFsCtlDeleteReparsePoint(FileSystem fileSystem, ReparseTag reparseTag, bool reparseGuidEqualOpenGuid, MessageStatus returnedStatus, ITestSite site)
        {
            if (fileSystem != FileSystem.FAT32)
            {
                if (reparseTag == ReparseTag.EMPTY && reparseGuidEqualOpenGuid)
                {
                    returnedStatus = FsaUtility.TransferExpectedResult<MessageStatus>(5001, MessageStatus.SUCCESS, returnedStatus, site);
                }
                else if (reparseTag == ReparseTag.IO_REPARSE_TAG_RESERVED_ONE || reparseTag == ReparseTag.IO_REPARSE_TAG_RESERVED_ZERO)
                {
                    returnedStatus = FsaUtility.TransferExpectedResult<MessageStatus>(2437, MessageStatus.IO_REPARSE_TAG_INVALID, returnedStatus, site);
                }
                else if ((reparseTag == ReparseTag.NON_MICROSOFT_RANGE_TAG) && (!reparseGuidEqualOpenGuid))
                {
                    returnedStatus = FsaUtility.TransferExpectedResult<MessageStatus>(990, MessageStatus.REPARSE_ATTRIBUTE_CONFLICT, returnedStatus, site);
                }
                else if (reparseTag == ReparseTag.NON_MICROSOFT_RANGE_TAG && reparseGuidEqualOpenGuid)
                {
                    returnedStatus = FsaUtility.TransferExpectedResult<MessageStatus>(1257, MessageStatus.IO_REPARSE_DATA_INVALID, returnedStatus, site);
                }
                else if (reparseTag == ReparseTag.NotEqualOpenFileReparseTag)
                {
                    returnedStatus = FsaUtility.TransferExpectedResult<MessageStatus>(989, MessageStatus.IO_REPARSE_TAG_MISMATCH, returnedStatus, site);
                }
            }
            return returnedStatus;
        }

        internal static MessageStatus WorkaroundFsCtlGetRetrivalPoints(BufferSize bufferSize, bool isStartingVcnNegative, bool isStartingVcnGreatThanAllocationSize, bool isElementsNotAllCopied, ref bool isBytesReturnedSet, MessageStatus returnedStatus, ITestSite site)
        {
            if (bufferSize == BufferSize.LessThanSTARTING_VCN_INPUT_BUFFER || (isStartingVcnNegative && !isStartingVcnGreatThanAllocationSize))
            {
                returnedStatus = FsaUtility.TransferExpectedResult<MessageStatus>(1103, MessageStatus.INVALID_PARAMETER, returnedStatus, site);
            }
            else if (bufferSize == BufferSize.BufferSizeSuccess && !isStartingVcnNegative && !isStartingVcnGreatThanAllocationSize && !isElementsNotAllCopied)
            {
                returnedStatus = FsaUtility.TransferExpectedResult<MessageStatus>(5041, MessageStatus.SUCCESS, returnedStatus, site);
                isBytesReturnedSet = FsaUtility.TransferExpectedResult<bool>(1112, true, isBytesReturnedSet, site);
            }
            return returnedStatus;
        }

        internal static MessageStatus WorkaroundQuerySecurityInfo(bool isByteCountGreater, MessageStatus returnedStatus, ITestSite site)
        {
            if (isByteCountGreater && returnedStatus == MessageStatus.BUFFER_TOO_SMALL)
            {
                // TD description:
                // [MS-SMB2] 3.3.5.20.3   Handling SMB2_0_INFO_SECURITY
                // If the OutputBufferLength given in the client request is either zero or is insufficient to hold the information requested, 
                // the server MUST fail the request with STATUS_BUFFER_TOO_SMALL.
                // Note:
                // SMB2 server responses with STATUS_BUFFER_TOO_SMALL instead of STATUS_BUFFER_OVERFLOW
                // To keep same model behavior, transfer the error code to STATUS_BUFFER_OVERFLOW.
                returnedStatus = FsaUtility.TransferExpectedResult<MessageStatus>(2799, MessageStatus.BUFFER_OVERFLOW, returnedStatus, site);
            }
            return returnedStatus;
        }

        internal static MessageStatus WorkaroundFsCtlQueryAllocatedRanges(BufferSize bufferSize, MessageStatus returnedStatus, ref bool isBytesReturnedSet, ITestSite site)
        {
            if (bufferSize == BufferSize.OutLessThanFILE_ALLOCATED_RANGE_BUFFER)
            {
                isBytesReturnedSet = FsaUtility.TransferExpectedResult<bool>(3778, false, isBytesReturnedSet, site);
                returnedStatus = FsaUtility.TransferExpectedResult<MessageStatus>(3778, MessageStatus.BUFFER_TOO_SMALL, returnedStatus, site);
            }
            return returnedStatus;
        }

        internal static MessageStatus WorkaroundSetSecurityInfo(FileSystem fileSystem, SecurityInformation securityInformation, OwnerSid ownerSidEnum, MessageStatus returnedStatus, ITestSite site)
        {
            if (fileSystem == FileSystem.FAT32) 
            {
                returnedStatus = FsaUtility.TransferExpectedResult<MessageStatus>(3232, MessageStatus.INVALID_DEVICE_REQUEST, returnedStatus, site);
            }
            else if (((securityInformation == SecurityInformation.OWNER_SECURITY_INFORMATION || securityInformation == SecurityInformation.GROUP_SECURITY_INFORMATION || securityInformation == SecurityInformation.LABEL_SECURITY_INFORMATION)) || (securityInformation == SecurityInformation.DACL_SECURITY_INFORMATION) || (securityInformation == SecurityInformation.SACL_SECURITY_INFORMATION))
            {
                returnedStatus = FsaUtility.TransferExpectedResult<MessageStatus>(3239, MessageStatus.ACCESS_DENIED, returnedStatus, site);
            }
            else if ((securityInformation == SecurityInformation.OWNER_SECURITY_INFORMATION && ownerSidEnum == OwnerSid.InputBufferOwnerSidNotPresent) || (securityInformation == SecurityInformation.OWNER_SECURITY_INFORMATION && ownerSidEnum == OwnerSid.InputBufferOwnerSidNotValid) || (securityInformation != SecurityInformation.OWNER_SECURITY_INFORMATION && ownerSidEnum == OwnerSid.OpenFileSecDesOwnerIsNull))
            {
                returnedStatus = FsaUtility.TransferExpectedResult<MessageStatus>(3239, MessageStatus.INVALID_OWNER, returnedStatus, site);
            }
            return returnedStatus;
        }

        internal static MessageStatus WorkaroundFsCtlReadFileUSNData(BufferSize bufferSize, MessageStatus returnedStatus, ITestSite site)
        {
            if (bufferSize == BufferSize.LessThanRecordLength)
            {
                returnedStatus = FsaUtility.TransferExpectedResult<MessageStatus>(3873, MessageStatus.INFO_LENGTH_MISMATCH, returnedStatus, site);
            }
            return returnedStatus;
        }

        internal static MessageStatus WorkaroundFsCtlSetEncrypion(FileSystem fileSystem, bool isIsCompressedTrue, EncryptionOperation encryptionOpteration, BufferSize bufferSize, MessageStatus returnedStatus, ITestSite site)
        {
            if (fileSystem == FileSystem.FAT32) 
            {
                returnedStatus = FsaUtility.TransferExpectedResult<MessageStatus>(3891, MessageStatus.INVALID_DEVICE_REQUEST, returnedStatus, site);
            }
            else if (bufferSize == BufferSize.LessThanENCRYPTION_BUFFER)
            {
                returnedStatus = FsaUtility.TransferExpectedResult<MessageStatus>(3899, MessageStatus.BUFFER_TOO_SMALL, returnedStatus, site);
            }
            else if ((encryptionOpteration == EncryptionOperation.NOT_VALID_IN_FSCC) || ((encryptionOpteration == EncryptionOperation.STREAM_SET_ENCRYPTION) && isIsCompressedTrue))
            {
                returnedStatus = FsaUtility.TransferExpectedResult<MessageStatus>(3900, MessageStatus.INVALID_PARAMETER, returnedStatus, site);
            }
            else if (!isIsCompressedTrue && encryptionOpteration == EncryptionOperation.STREAM_SET_ENCRYPTION && bufferSize == BufferSize.BufferSizeSuccess)
            {
                returnedStatus = FsaUtility.TransferExpectedResult<MessageStatus>(3919, MessageStatus.SUCCESS, returnedStatus, site);
            }
            return returnedStatus;
        }

        internal static MessageStatus WorkaroundWriteFile(long byteOffset, bool isOpenVolumeReadOnly, long byteCount, ref long bytesWritten, MessageStatus returnedStatus, ITestSite site)
        {
            if (byteOffset == -2 && !isOpenVolumeReadOnly && byteCount == 2)
            {
                bytesWritten = FsaUtility.TransferExpectedResult<long>(742, 2, bytesWritten, site);
                returnedStatus = FsaUtility.TransferExpectedResult<MessageStatus>(715, MessageStatus.SUCCESS, returnedStatus, site);
            }
            return returnedStatus;
        }

        internal static MessageStatus WorkaroundChangeNotificationForDir(bool allEntriesFitBufSize, MessageStatus returnedStatus, ITestSite site)
        {
            if (!allEntriesFitBufSize)
            {
                returnedStatus = FsaUtility.TransferExpectedResult<MessageStatus>(1399, MessageStatus.NOTIFY_ENUM_DIR, returnedStatus, site);
            }
            else if (allEntriesFitBufSize)
            {
                returnedStatus = FsaUtility.TransferExpectedResult<MessageStatus>(1399, MessageStatus.SUCCESS, returnedStatus, site);
            }
            return returnedStatus;
        }

        internal static MessageStatus WorkAroundQueryFileSystemInfo(FileSystemInfoClass fileInfoClass, OutputBufferSize outBufSmall, MessageStatus returnedStatus, ref FsInfoByteCount byteCount, ITestSite site)
        {
            if (fileInfoClass == FileSystemInfoClass.File_FsObjectId_Information && returnedStatus == MessageStatus.OBJECT_NAME_NOT_FOUND)
            {
                returnedStatus = FsaUtility.TransferExpectedResult<MessageStatus>(2674, MessageStatus.SUCCESS, returnedStatus, site);
                byteCount = FsInfoByteCount.SizeOf_FILE_FS_OBJECTID_INFORMATION;
            }
            return returnedStatus;
        }

        internal static MessageStatus WorkaroundReadFile(CreateOptions gOpenMode, long byteCount, MessageStatus returnedStatus, ITestSite site)
        {
            long gOpenFileVolumeSize = long.Parse(site.Properties["FSA.OpenFileVolumeSize"]);
            if ((byteCount % gOpenFileVolumeSize) != 0)
            {
                returnedStatus = FsaUtility.TransferExpectedResult<MessageStatus>(669, MessageStatus.INVALID_PARAMETER, returnedStatus, site);
            }
            else if ((gOpenMode & CreateOptions.NO_INTERMEDIATE_BUFFERING) != 0)
            {
                returnedStatus = FsaUtility.TransferExpectedResult<MessageStatus>(693, MessageStatus.SUCCESS, returnedStatus, site);
            }
            return returnedStatus;
        }

        internal static long WorkaroundReadFileForByteRead(CreateOptions gOpenMode, long byteCount, long readCount, ITestSite site)
        {
            if ((gOpenMode & CreateOptions.NO_INTERMEDIATE_BUFFERING) != 0)
            {
                readCount = FsaUtility.TransferExpectedResult<long>(692, byteCount, readCount, site);
            }
            return readCount;
        }
    }
}
