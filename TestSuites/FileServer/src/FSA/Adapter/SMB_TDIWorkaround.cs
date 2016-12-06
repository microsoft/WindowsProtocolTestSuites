// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Protocols.TestTools;

namespace Microsoft.Protocols.TestSuites.FileSharing.FSA.Adapter
{
    internal static class SMB_TDIWorkaround
    {
        internal static MessageStatus WorkaroundMethodSetFileAllocOrObjIdInfo(FileInfoClass fileInfoClass, bool isInputBufAllocGreater, MessageStatus returnedStatus, ITestSite site)
        {
            switch (fileInfoClass)
            {
                case FileInfoClass.FILE_OBJECTID_INFORMATION:
                    {
                        returnedStatus = FsaUtility.TransferExpectedResult<MessageStatus>(3007, MessageStatus.NOT_SUPPORTED, returnedStatus, site);
                        break;
                    }
                case FileInfoClass.FILE_SFIO_RESERVE_INFORMATION:
                    {
                        returnedStatus = FsaUtility.TransferExpectedResult<MessageStatus>(3166, MessageStatus.NOT_SUPPORTED, returnedStatus, site);
                        break;
                    }
                default:
                    break;
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

        internal static MessageStatus WorkAroundFsCtlSetObjID(FsControlRequestType requestType, BufferSize bufferSize, MessageStatus returnedStatus, ITestSite site)
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

        internal static MessageStatus WorkAroundFsCtlForEasyRequest(FileSystem fileSystem, FsControlRequestType requestType, BufferSize bufferSize, bool fileVolReadOnly, bool fileVolUsnAct, MessageStatus returnedStatus, ITestSite site)
        {
            switch (requestType)
            {
                case FsControlRequestType.FSCTL_GET_COMPRESSION:
                    {
                        if (bufferSize != BufferSize.LessThanTwoBytes)
                        {
                            returnedStatus = FsaUtility.TransferExpectedResult<MessageStatus>(1136, MessageStatus.SUCCESS, returnedStatus, site);
                        }
                        else
                        {
                            returnedStatus = FsaUtility.TransferExpectedResult<MessageStatus>(3851, MessageStatus.INVALID_PARAMETER, returnedStatus, site);
                        }
                    }
                    break;
                case FsControlRequestType.QUERY_SPARING_INFO:
                    {
                        if (bufferSize != BufferSize.LessThanFILE_QUERY_SPARING_BUFFER)
                        {
                            returnedStatus = FsaUtility.TransferExpectedResult<MessageStatus>(1136, MessageStatus.SUCCESS, returnedStatus, site);
                        }
                        else
                        {
                            returnedStatus = FsaUtility.TransferExpectedResult<MessageStatus>(3851, MessageStatus.INVALID_PARAMETER, returnedStatus, site);
                        }
                    }
                    break;

                case FsControlRequestType.RECALL_FILE:
                    returnedStatus = FsaUtility.TransferExpectedResult<MessageStatus>(1136, MessageStatus.SUCCESS, returnedStatus, site);
                    break;

                case FsControlRequestType.SET_OBJECT_ID:
                    {
                        if (bufferSize != BufferSize.LessThanFILE_OBJECTID_BUFFER)
                        {
                            returnedStatus = FsaUtility.TransferExpectedResult<MessageStatus>(1136, MessageStatus.SUCCESS, returnedStatus, site);
                        }
                        else
                        {
                            returnedStatus = FsaUtility.TransferExpectedResult<MessageStatus>(3851, MessageStatus.INVALID_PARAMETER, returnedStatus, site);
                        }
                    }
                    break;

                case FsControlRequestType.FSCTL_SET_SHORT_NAME_BEHAVIOR:
                    {
                        returnedStatus = FsaUtility.TransferExpectedResult<MessageStatus>(1136, MessageStatus.INVALID_DEVICE_REQUEST, returnedStatus, site);
                        break;
                    }
                case FsControlRequestType.WRITE_USN_CLOSE_RECORD:
                    {
                        if ((!fileVolReadOnly && bufferSize != BufferSize.LessThanSizeofUsn && fileVolUsnAct) ||
                            (returnedStatus == MessageStatus.JOURNAL_NOT_ACTIVE && fileSystem == FileSystem.NTFS))
                        {
                            returnedStatus = FsaUtility.TransferExpectedResult<MessageStatus>(1136, MessageStatus.SUCCESS, returnedStatus, site);
                        }
                        else if (bufferSize == BufferSize.LessThanSizeofUsn)
                        {
                            returnedStatus = FsaUtility.TransferExpectedResult<MessageStatus>(3851, MessageStatus.INVALID_PARAMETER, returnedStatus, site);
                        }
                        else if (fileVolReadOnly)
                        {
                            returnedStatus = FsaUtility.TransferExpectedResult<MessageStatus>(3960, MessageStatus.MEDIA_WRITE_PROTECTED, returnedStatus, site);
                        }
                    }
                    break;

                case FsControlRequestType.SET_ZERO_ON_DEALLOCATION:
                    { 
                        if (fileSystem != FileSystem.NTFS)
                        {
                            returnedStatus = FsaUtility.TransferExpectedResult<MessageStatus>(4721, MessageStatus.INVALID_DEVICE_REQUEST, returnedStatus, site);
                        }
                    }
                    break;
                case FsControlRequestType.FSCTL_FILESYSTEM_GET_STATISTICS:
                    {
                        if (fileSystem == FileSystem.FAT32)
                        {
                            returnedStatus = FsaUtility.TransferExpectedResult<MessageStatus>(5803, MessageStatus.INVALID_DEVICE_REQUEST, returnedStatus, site);
                        }
                    }
                    break;

                default:                    
                    break;
            }
            return returnedStatus;
        }

        internal static MessageStatus WorkAroundFsCtlSetZeroData(FileSystem fileSystem, BufferSize bufferSize, InputBuffer_FSCTL_SET_ZERO_DATA inputBuffer, bool isIsDeletedTrue, bool isConflictDetected, MessageStatus returnedStatus, ITestSite site)
        {
            if (fileSystem != FileSystem.NTFS && fileSystem != FileSystem.REFS)
            {
                returnedStatus = FsaUtility.TransferExpectedResult<MessageStatus>(4335, MessageStatus.INVALID_DEVICE_REQUEST, returnedStatus, site);
            }
            else if (isIsDeletedTrue &&
                bufferSize == BufferSize.BufferSizeSuccess &&
                (inputBuffer == InputBuffer_FSCTL_SET_ZERO_DATA.BufferSuccess || inputBuffer == InputBuffer_FSCTL_SET_ZERO_DATA.FileOffsetGreatThanBeyondFinalZero))
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

        internal static MessageStatus WorkAroundFsctlSisCopyFile(FileSystem fileSystem, BufferSize bufferSize, InputBufferFSCTL_SIS_COPYFILE inputBuffer, bool isCOPYFILE_SIS_LINKTrue, bool isIsEncryptedTrue, MessageStatus returnedStatus, ITestSite site)
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

        internal static MessageStatus WorkArondSetFileShortNameInfo(InputBufferFileName inputBufferFileName, MessageStatus returnedstatus, ITestSite site)
        {
            if (inputBufferFileName == InputBufferFileName.StartWithBackSlash)
            {
                returnedstatus = FsaUtility.TransferExpectedResult<MessageStatus>(3173, MessageStatus.INVALID_PARAMETER, returnedstatus, site);
            }
            else if (inputBufferFileName == InputBufferFileName.NotValid)
            {
                returnedstatus = FsaUtility.TransferExpectedResult<MessageStatus>(3176, MessageStatus.INVALID_PARAMETER, returnedstatus, site);
            }
            return returnedstatus;
        }

        internal static MessageStatus WrokAroundStreamRename(FileSystem fileSystem, InputBufferFileName NewStreamName, InputBufferFileName StreamTypeName, bool ReplaceIfExists, MessageStatus returnedStatus, ITestSite site)
        {
            if (fileSystem == FileSystem.REFS || fileSystem == FileSystem.FAT32)
            {
                returnedStatus = FsaUtility.TransferExpectedResult<MessageStatus>(3146, MessageStatus.NOT_SUPPORTED, returnedStatus, site);
            }
            else if (NewStreamName == InputBufferFileName.ContainsWildcard ||
                NewStreamName == InputBufferFileName.ContainsInvalid)
            {
                returnedStatus = FsaUtility.TransferExpectedResult<MessageStatus>(3146, MessageStatus.INVALID_PARAMETER, returnedStatus, site);
            }
            return returnedStatus;
        }

        internal static MessageStatus WorkAroundQueryDirectoryInfo(bool isNoRecordsReturned, bool isOutBufferSizeLess, MessageStatus returnedStatus, ITestSite site)
        {
            if (isNoRecordsReturned)
            {
                returnedStatus = FsaUtility.TransferExpectedResult<MessageStatus>(865, MessageStatus.NO_SUCH_FILE, returnedStatus, site);
            }

            if (isOutBufferSizeLess)
            {
                returnedStatus = FsaUtility.TransferExpectedResult<MessageStatus>(4836, MessageStatus.INFO_LENGTH_MISMATCH, returnedStatus, site);
            }
            return returnedStatus;
        }

        internal static MessageStatus WorkAroundSetFileFullEaInfo(EainInputBuffer eAValidate, MessageStatus returnedStatus, ITestSite site)
        {
            if (eAValidate == EainInputBuffer.EaNameNotWellForm || eAValidate == EainInputBuffer.EaFlagsInvalid)
            {
                returnedStatus = FsaUtility.TransferExpectedResult<MessageStatus>(2927, MessageStatus.INVALID_EA_NAME, returnedStatus, site);
            }
            else
            {
                returnedStatus = FsaUtility.TransferExpectedResult<MessageStatus>(2853, MessageStatus.SUCCESS, returnedStatus, site);
            }
            return returnedStatus;
        }

        internal static bool WorkAroundFsCtlGetReparsePoint(BufferSize bufferSize, ReparseTag openFileReparseTag, bool isBytesReturnedSet, ITestSite site)
        {
            if (!((openFileReparseTag == ReparseTag.EMPTY)
                || ((openFileReparseTag != ReparseTag.NON_MICROSOFT_RANGE_TAG) && (BufferSize.LessThanREPARSE_DATA_BUFFER == bufferSize))
                || ((openFileReparseTag == ReparseTag.NON_MICROSOFT_RANGE_TAG) && (BufferSize.LessThanREPARSE_GUID_DATA_BUFFER == bufferSize))))
            {
                isBytesReturnedSet = FsaUtility.TransferExpectedResult<bool>(1091, true, isBytesReturnedSet, site);
            }
            return isBytesReturnedSet;
        }

        internal static MessageStatus WorkAroundQueryFileInfoPart1(FileSystem fileSystem, FileInfoClass fileInfoClass, OutputBufferSize outputBufferSize, ref ByteCount byteCount, ref OutputBuffer outputBuffer, MessageStatus returnedStatus, ITestSite site)
        {
            if (fileInfoClass == FileInfoClass.NOT_DEFINED_IN_FSCC)
            {
                returnedStatus = FsaUtility.TransferExpectedResult<MessageStatus>(1426, MessageStatus.INVALID_INFO_CLASS, returnedStatus, site);
            }
            else
            {
                switch (fileInfoClass)
                {
                    case FileInfoClass.FILE_STANDARD_LINK_INFORMATION:
                        {
                            returnedStatus = FsaUtility.TransferExpectedResult<MessageStatus>(2749, MessageStatus.INVALID_INFO_CLASS, returnedStatus, site);
                            break;
                        }
                    case FileInfoClass.FILE_LINKS_INFORMATION:
                        {
                            returnedStatus = FsaUtility.TransferExpectedResult<MessageStatus>(1593, MessageStatus.INVALID_INFO_CLASS, returnedStatus, site);
                            break;
                        }
                    case FileInfoClass.FILE_QUOTA_INFORMATION:
                        {
                            returnedStatus = FsaUtility.TransferExpectedResult<MessageStatus>(2524, MessageStatus.INVALID_INFO_CLASS, returnedStatus, site);
                            break;
                        }
                    case FileInfoClass.FILE_SFIO_RESERVE_INFORMATION:
                        {
                            returnedStatus = FsaUtility.TransferExpectedResult<MessageStatus>(2734, MessageStatus.INVALID_INFO_CLASS, returnedStatus, site);
                            break;
                        }
                    case FileInfoClass.FILE_OBJECTID_INFORMATION:
                        {
                            returnedStatus = FsaUtility.TransferExpectedResult<MessageStatus>(1585, MessageStatus.INVALID_INFO_CLASS, returnedStatus, site);
                            break;
                        }
                    case FileInfoClass.FILE_REPARSE_POINT_INFORMATION:
                        {
                            returnedStatus = FsaUtility.TransferExpectedResult<MessageStatus>(2558, MessageStatus.INVALID_INFO_CLASS, returnedStatus, site);
                            break;
                        }
                    case FileInfoClass.FILE_NAME_INFORMATION:
                        {
                            returnedStatus = FsaUtility.TransferExpectedResult<MessageStatus>(1591, MessageStatus.INVALID_INFO_CLASS, returnedStatus, site);
                            break;
                        }
                    case FileInfoClass.FILE_FULLEA_INFORMATION:
                        {
                            // SMB server does not suport this operation, transfer return code to keep same model behavior
                            if (returnedStatus == MessageStatus.INVALID_INFO_CLASS)
                            {
                                if (outputBufferSize == OutputBufferSize.LessThan)
                                {
                                    returnedStatus = FsaUtility.TransferExpectedResult<MessageStatus>(3899, MessageStatus.BUFFER_TOO_SMALL, returnedStatus, site);
                                }
                                else
                                {
                                    byteCount = FsaUtility.TransferExpectedResult<ByteCount>(3992, ByteCount.SizeofFILE_FULL_EA_INFORMATION, byteCount, site);
                                    returnedStatus = FsaUtility.TransferExpectedResult<MessageStatus>(3994, MessageStatus.SUCCESS, returnedStatus, site);
                                }
                            }
                            break;
                        }
                    case FileInfoClass.FILE_ALTERNATENAME_INFORMATION:
                        {
                            if (fileSystem == FileSystem.REFS && outputBufferSize == OutputBufferSize.NotLessThan)
                            {
                                // REFS file system does not support FILE_ALTERNATENAME_INFORMATION, will failed with STATUS_OBJECT_NAME_NOT_FOUND
                                // Transfer the return code and byteCount to make model test cases passed.
                                byteCount = FsaUtility.TransferExpectedResult<ByteCount>(3992, ByteCount.FieldOffsetFILE_NAME_INFORMATION_FileNameAddOutputBuffer_FileNameLength, byteCount, site);
                                returnedStatus = FsaUtility.TransferExpectedResult<MessageStatus>(1187, MessageStatus.SUCCESS, returnedStatus, site);
                            }
                            break;
                        }
                    case FileInfoClass.FILE_STREAM_INFORMATION:
                        {
                            if (fileSystem == FileSystem.FAT32)
                            {
                                returnedStatus = FsaUtility.TransferExpectedResult<MessageStatus>(1421, MessageStatus.SUCCESS, returnedStatus, site);
                            }
                        }
                        break;
                    case FileInfoClass.FILE_COMPRESSION_INFORMATION:
                        {
                            if (fileSystem == FileSystem.FAT32)
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
                        }
                        break;
                    case FileInfoClass.FILE_ATTRIBUTETAG_INFORMATION:
                        {
                            if (fileSystem == FileSystem.FAT32)
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
                        }
                        break;
                    default:
                        break;
                }
            }
            return returnedStatus;
        }

        internal static MessageStatus WorkAroundReadFile(long byteOffset, int byteCount, MessageStatus returnedStatus, ITestSite site)
        {
            if (returnedStatus == MessageStatus.INVALID_DEVICE_REQUEST)
            {
                returnedStatus = FsaUtility.TransferExpectedResult<MessageStatus>(1103, MessageStatus.INVALID_PARAMETER, returnedStatus, site);
            }
            else if (byteOffset < 0 && returnedStatus == MessageStatus.SUCCESS)
            {
                // If byteOffset < 0, SMB transport adapter will covert to 0, and return STATUS_SUCCESS.
                // To keep same model behavior, transfer the return code to STATUS_INVALID_PARAMETER.
                returnedStatus = FsaUtility.TransferExpectedResult<MessageStatus>(1103, MessageStatus.INVALID_PARAMETER, returnedStatus, site);
            }
            else
            {
                long gOpenFileVolumeSize = long.Parse(site.Properties["FSA.OpenFileVolumeSize"]);
                if ((byteCount % gOpenFileVolumeSize) != 0)
                {
                    returnedStatus = FsaUtility.TransferExpectedResult<MessageStatus>(669, MessageStatus.INVALID_PARAMETER, returnedStatus, site);
                }
            }
            return returnedStatus;
        }

        internal static MessageStatus WorkAroundWriteFile(CreateOptions openMode, MessageStatus returnedStatus, ITestSite site)
        {
            if (returnedStatus == MessageStatus.DISK_FULL)
            {
                returnedStatus = FsaUtility.TransferExpectedResult<MessageStatus>(715, MessageStatus.MEDIA_WRITE_PROTECTED, MessageStatus.DISK_FULL, site);
            }
            else if (openMode == CreateOptions.NO_INTERMEDIATE_BUFFERING && returnedStatus == MessageStatus.SUCCESS)
            {
                // MS-FSA is going to verify below requirement of 2.1.5.3   Server Requests a Write 
                // If Open.Mode.FILE_NO_INTERMEDIATE_BUFFERING is TRUE and (ByteOffset >= 0), the operation MUST be failed with STATUS_INVALID_PARAMETER 
                // under any of the following conditions:If (ByteOffset % Open.File.Volume.SectorSize) is not zero.
                // 
                // But SMB server does not fail on such condition. 
                // To keep same model behavior, transfer the return code to STATUS_INVALID_PARAMETER
                returnedStatus = FsaUtility.TransferExpectedResult<MessageStatus>(1103, MessageStatus.INVALID_PARAMETER, returnedStatus, site);
            }
            return returnedStatus;
        }

        internal static long WorkAroundWriteFileOut(long byteOffset, bool isOpenVolumeReadOnly, long byteCount, long bytesWritten, ITestSite site)
        {
            if (byteOffset < 0 && !isOpenVolumeReadOnly && byteCount != 0)
            {
                bytesWritten = FsaUtility.TransferExpectedResult<long>(742, 2, bytesWritten, site);
            }
            return bytesWritten;
        }

        internal static MessageStatus WorkAroundFlushCachedData(bool isOpenVolumeReadOnly, MessageStatus rtnstatus, ITestSite site)
        {
            // Flush Cache Data is not supported by SMB.
            // Below transfer the error code so as to keep same model behavior.
            if (isOpenVolumeReadOnly)
            {
                rtnstatus = FsaUtility.TransferExpectedResult<MessageStatus>(883, MessageStatus.MEDIA_WRITE_PROTECTED, rtnstatus, site);
            }
            else
            {
                rtnstatus = FsaUtility.TransferExpectedResult<MessageStatus>(881, MessageStatus.SUCCESS, rtnstatus, site);
            }
            return rtnstatus;
        }

        internal static bool WorkAroundFsCtlGetRetrivalPoints(BufferSize bufferSize, bool isStartingVcnNegative, bool isStartingVcnGreatThanAllocationSize, bool isElementsNotAllCopied, bool isBytesReturnedSet, ITestSite site)
        {
            if (!((bufferSize == BufferSize.LessThanSTARTING_VCN_INPUT_BUFFER)
                || (bufferSize == BufferSize.LessThanRETRIEVAL_POINTERS_BUFFER)
                || (isStartingVcnNegative)
                || (isStartingVcnGreatThanAllocationSize)
                || (isElementsNotAllCopied)))
            {
                isBytesReturnedSet = FsaUtility.TransferExpectedResult<bool>(5041, true, isBytesReturnedSet, site);
            }
            return isBytesReturnedSet;
        }

        internal static MessageStatus WorkAroundFsCtlGetRetrivalPointsStatus(BufferSize bufferSize, bool isStartingVcnNegative, bool isStartingVcnGreatThanAllocationSize, MessageStatus returnedStatus, ITestSite site)
        {
            if ((bufferSize == BufferSize.LessThanSTARTING_VCN_INPUT_BUFFER)
                || (isStartingVcnNegative && !isStartingVcnGreatThanAllocationSize))
            {
                returnedStatus = FsaUtility.TransferExpectedResult<MessageStatus>(1103, MessageStatus.INVALID_PARAMETER, returnedStatus, site);
            }
            else if (returnedStatus == MessageStatus.NOT_SUPPORTED)
            {
                // [MS-SMB] (footnote 136) Section 3.3.5.11.1: 
                // The following FSCTL is explicitly blocked by Windows 8 and Windows Server 2012 and is failed with STATUS_NOT_SUPPORTED.
                // FSCTL_GET_RETRIEVAL_POINTERS 0x00090073
                // Transfer the return code to keep same model behavior
                if (bufferSize == BufferSize.LessThanRETRIEVAL_POINTERS_BUFFER)
                {
                    returnedStatus = FsaUtility.TransferExpectedResult<MessageStatus>(1083, MessageStatus.BUFFER_TOO_SMALL, returnedStatus, site);
                }
                else if (bufferSize == BufferSize.BufferSizeSuccess)
                {
                    if (isStartingVcnGreatThanAllocationSize)
                    {
                        returnedStatus = FsaUtility.TransferExpectedResult<MessageStatus>(1083, MessageStatus.END_OF_FILE, returnedStatus, site);
                    }
                    else
                    {
                        returnedStatus = FsaUtility.TransferExpectedResult<MessageStatus>(2853, MessageStatus.SUCCESS, returnedStatus, site);
                    }
                }
            }
            return returnedStatus;
        }

        internal static MessageStatus WorkaroundQuerySecurityInfo(bool isByteCountGreater, MessageStatus returnedStatus, ITestSite site)
        {
            if (isByteCountGreater && returnedStatus == MessageStatus.SUCCESS)
            {
                // SMB transport adapter does not support setting ByteCount greater than InputBufferLength.
                // So it will return STATUS_SUCCESS instead of STATUS_BUFFER_OVERFLOW.
                // To keep same model behavior, transfer the return code to STATUS_BUFFER_OVERFLOW.
                returnedStatus = FsaUtility.TransferExpectedResult<MessageStatus>(2799, MessageStatus.BUFFER_OVERFLOW, returnedStatus, site);
            }
            return returnedStatus;
        }

        internal static MessageStatus WorkAroundFsCtlReadFileUSNData(BufferSize bufferSize, MessageStatus returnedStatus, ITestSite site)
        {
            if (bufferSize == BufferSize.LessThanRecordLength)
            {
                returnedStatus = FsaUtility.TransferExpectedResult<MessageStatus>(3873, MessageStatus.INFO_LENGTH_MISMATCH, returnedStatus, site);
            }
            return returnedStatus;
        }

        internal static MessageStatus WorkAroundFsCtlSetEncryption(FileSystem fileSystem, bool isIsCompressedTrue, EncryptionOperation encryptionOpteration, BufferSize bufferSize, MessageStatus returnedStatus, ITestSite site)
        {
            if (fileSystem == FileSystem.FAT32)
            {
                returnedStatus = FsaUtility.TransferExpectedResult<MessageStatus>(3891, MessageStatus.INVALID_DEVICE_REQUEST, returnedStatus, site);
            }
            else if (returnedStatus == MessageStatus.NOT_SUPPORTED)
            {
                if (bufferSize == BufferSize.LessThanENCRYPTION_BUFFER)
                {
                    returnedStatus = FsaUtility.TransferExpectedResult<MessageStatus>(3899, MessageStatus.BUFFER_TOO_SMALL, returnedStatus, site);
                }
                else if ((encryptionOpteration == EncryptionOperation.NOT_VALID_IN_FSCC) || ((encryptionOpteration == EncryptionOperation.STREAM_SET_ENCRYPTION) && isIsCompressedTrue))
                {
                    returnedStatus = FsaUtility.TransferExpectedResult<MessageStatus>(3901, MessageStatus.INVALID_PARAMETER, returnedStatus, site);
                }
                else
                {
                    returnedStatus = FsaUtility.TransferExpectedResult<MessageStatus>(3919, MessageStatus.SUCCESS, returnedStatus, site);
                }
            }
            return returnedStatus;
        }

        internal static MessageStatus WorkAroundFsCtlSetReparsePoint(FileSystem fileSystem, ReparseTag inputReparseTag, bool isReparseGUIDNotEqual, bool isFileReparseTagNotEqualInputBufferReparseTag, MessageStatus returnedStatus, ITestSite site)
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

        internal static MessageStatus WorkAroundFsCtlDeleteReparsePoint(FileSystem fileSystem, ReparseTag reparseTag, bool reparseGuidEqualOpenGuid, MessageStatus returnedStatus, ITestSite site)
        {
            if (fileSystem != FileSystem.FAT32)
            {
                if (returnedStatus == MessageStatus.INVALID_PARAMETER)
                {
                    if ((reparseTag == ReparseTag.NON_MICROSOFT_RANGE_TAG) && (!reparseGuidEqualOpenGuid))
                    {
                        returnedStatus = FsaUtility.TransferExpectedResult<MessageStatus>(990, MessageStatus.REPARSE_ATTRIBUTE_CONFLICT, returnedStatus, site);
                    }
                    else if ((reparseTag == ReparseTag.IO_REPARSE_TAG_RESERVED_ZERO) || (reparseTag == ReparseTag.IO_REPARSE_TAG_RESERVED_ONE))
                    {
                        returnedStatus = FsaUtility.TransferExpectedResult<MessageStatus>(984, MessageStatus.IO_REPARSE_TAG_INVALID, returnedStatus, site);
                    }
                    else if (reparseTag == ReparseTag.NotEqualOpenFileReparseTag)
                    {
                        returnedStatus = FsaUtility.TransferExpectedResult<MessageStatus>(989, MessageStatus.IO_REPARSE_TAG_MISMATCH, returnedStatus, site);
                    }
                    else if (reparseTag == ReparseTag.NON_MICROSOFT_RANGE_TAG && reparseGuidEqualOpenGuid)
                    {
                        returnedStatus = FsaUtility.TransferExpectedResult<MessageStatus>(1257, MessageStatus.IO_REPARSE_DATA_INVALID, returnedStatus, site);
                    }
                    else
                    {
                        returnedStatus = FsaUtility.TransferExpectedResult<MessageStatus>(5001, MessageStatus.SUCCESS, returnedStatus, site);
                    }
                }
            }
            return returnedStatus;
        }

        internal static MessageStatus WorkAroundFsCtlGetReparsePointStatus(BufferSize bufferSize, ReparseTag openFileReparseTag, MessageStatus returnedStatus, ITestSite site)
        {
            if (openFileReparseTag == ReparseTag.EMPTY)
            {
                return returnedStatus;
            }
            else if ((openFileReparseTag != ReparseTag.NON_MICROSOFT_RANGE_TAG) && (BufferSize.LessThanREPARSE_DATA_BUFFER == bufferSize))
            {
                returnedStatus = FsaUtility.TransferExpectedResult<MessageStatus>(1080, MessageStatus.BUFFER_TOO_SMALL, returnedStatus, site);
            }
            else if ((openFileReparseTag == ReparseTag.NON_MICROSOFT_RANGE_TAG) && (BufferSize.LessThanREPARSE_GUID_DATA_BUFFER == bufferSize))
            {
                returnedStatus = FsaUtility.TransferExpectedResult<MessageStatus>(1083, MessageStatus.BUFFER_TOO_SMALL, returnedStatus, site);
            }
            else if (returnedStatus == MessageStatus.NOT_A_REPARSE_POINT && 
                (bufferSize == BufferSize.BufferSizeSuccess || (bufferSize == BufferSize.LessThanREPARSE_DATA_BUFFER && openFileReparseTag == ReparseTag.NON_MICROSOFT_RANGE_TAG) ))
            {
                // If the open file is not a reparse point, SMB server will return STATUS_NOT_A_REPARSE_POINT
                // This is acceptable in model and expect as STATUS_SUCCESS.
                returnedStatus = FsaUtility.TransferExpectedResult<MessageStatus>(2853, MessageStatus.SUCCESS, returnedStatus, site);
            }
            return returnedStatus;
        }

        internal static bool WorkAroundFsCtlForEasyRequestBool(FileSystem fileSystem, FsControlRequestType requestType, BufferSize bufferSize, bool fileVolReadOnly, bool fileVolUsnAct, bool isBytesReturnedSet, ref bool isOutputBufferSizeReturn, MessageStatus returnedStatus, ITestSite site)
        {
            switch (requestType)
            {
                case FsControlRequestType.FSCTL_FILESYSTEM_GET_STATISTICS:
                    {
                        if (fileSystem == FileSystem.FAT32) {
                            isBytesReturnedSet = FsaUtility.TransferExpectedResult<bool>(5803, false, isBytesReturnedSet, site);
                            isOutputBufferSizeReturn = FsaUtility.TransferExpectedResult<bool>(5803, false, isOutputBufferSizeReturn, site);
                        }
                        else if (bufferSize != BufferSize.LessThanTotalSizeOfStatistics && bufferSize != BufferSize.LessThanSizeOf_FILESYSTEM_STATISTICS)
                        {
                            isBytesReturnedSet = FsaUtility.TransferExpectedResult<bool>(5522, true, isBytesReturnedSet, site);
                        }
                    }
                    break;

                case FsControlRequestType.FSCTL_GET_COMPRESSION:
                    {
                        if (bufferSize != BufferSize.LessThanTwoBytes)
                        {
                            isBytesReturnedSet = FsaUtility.TransferExpectedResult<bool>(5522, true, isBytesReturnedSet, site);
                        }
                    }
                    break;
                case FsControlRequestType.SET_OBJECT_ID:
                    {
                        if (bufferSize != BufferSize.LessThanFILE_OBJECTID_BUFFER)
                        {
                            isBytesReturnedSet = FsaUtility.TransferExpectedResult<bool>(5522, true, isBytesReturnedSet, site);
                        }
                    }
                    break;

                case FsControlRequestType.QUERY_FAT_BPB:
                    {
                        if (bufferSize != BufferSize.LessThan0x24)
                        {
                            isBytesReturnedSet = FsaUtility.TransferExpectedResult<bool>(5522, true, isBytesReturnedSet, site);
                        }
                    }
                    break;

                case FsControlRequestType.QUERY_ON_DISK_VOLUME_INFO:
                    {
                        if (bufferSize != BufferSize.LessThanFILE_QUERY_ON_DISK_VOL_INFO_BUFFER)
                        {
                            isBytesReturnedSet = FsaUtility.TransferExpectedResult<bool>(5522, true, isBytesReturnedSet, site);
                        }
                    }
                    break;

                case FsControlRequestType.QUERY_SPARING_INFO:
                    {
                        if (bufferSize != BufferSize.LessThanFILE_QUERY_SPARING_BUFFER)
                        {
                            isBytesReturnedSet = FsaUtility.TransferExpectedResult<bool>(5522, true, isBytesReturnedSet, site);
                        }
                    }
                    break;
                case FsControlRequestType.FSCTL_SET_SHORT_NAME_BEHAVIOR:
                    {
                        isBytesReturnedSet = FsaUtility.TransferExpectedResult<bool>(5522, false, isBytesReturnedSet, site);
                        break;
                    }
                case FsControlRequestType.WRITE_USN_CLOSE_RECORD:
                    {
                        if ((!fileVolReadOnly && bufferSize != BufferSize.LessThanSizeofUsn && fileVolUsnAct) ||
                           ((returnedStatus == MessageStatus.JOURNAL_NOT_ACTIVE && fileSystem == FileSystem.NTFS) || returnedStatus == MessageStatus.SUCCESS))
                        {
                            isBytesReturnedSet = FsaUtility.TransferExpectedResult<bool>(5522, true, isBytesReturnedSet, site);
                            isOutputBufferSizeReturn = FsaUtility.TransferExpectedResult<bool>(5522, true, isOutputBufferSizeReturn, site);
                        }
                    }
                    break;
                case FsControlRequestType.SET_ZERO_ON_DEALLOCATION:
                    {
                        if (fileSystem != FileSystem.NTFS) {
                            isBytesReturnedSet = FsaUtility.TransferExpectedResult<bool>(4721, false, isBytesReturnedSet, site);
                            isOutputBufferSizeReturn = FsaUtility.TransferExpectedResult<bool>(4721, false, isOutputBufferSizeReturn, site);
                        }
                    }
                    break;
                default:
                    break;
            }
            return isBytesReturnedSet;
        }

        internal static MessageStatus WorkAroundChangeNotificationForDir(bool allEntriesFitBufSize, MessageStatus returnedStatus, ITestSite site)
        {
            if (allEntriesFitBufSize)
            {
                returnedStatus = FsaUtility.TransferExpectedResult<MessageStatus>(1399, MessageStatus.SUCCESS, returnedStatus, site);
            }
            else
            {
                returnedStatus = FsaUtility.TransferExpectedResult<MessageStatus>(1399, MessageStatus.NOTIFY_ENUM_DIR, returnedStatus, site);
            }
            return returnedStatus;
        }

        internal static MessageStatus WorkAroundQueryFileSystemInfo(FileSystemInfoClass fileInfoClass, OutputBufferSize outBufSmall, MessageStatus returnedStatus, ref FsInfoByteCount byteCount, ITestSite site)
        {
            if (fileInfoClass == FileSystemInfoClass.File_FsDriverPath_Information && outBufSmall == OutputBufferSize.NotLessThan)
            {
                // According to MS-FSA, it is expected to get error code STATUS_NOT_SUPPORTED
                // In SMB2 server, it returns STATUS_NOT_SUPPORTED correctly.
                // But SMB1 server responses with STATUS_INVALID_PARAMETER.
                // To keep same model behavior, transfer the error code to STATUS_NOT_SUPPORTED.
                returnedStatus = FsaUtility.TransferExpectedResult<MessageStatus>(2674, MessageStatus.NOT_SUPPORTED, returnedStatus, site);
            }
            else if (fileInfoClass == FileSystemInfoClass.NOT_DEFINED_IN_FSCC && outBufSmall == OutputBufferSize.NotLessThan)
            {
                // For undefined information class, it is expected to get error code STATUS_NOT_SUPPORTED
                // In SMB2 server, it returns STATUS_NOT_SUPPORTED correctly.
                // But SMB1 server responses with STATUS_SUCCESS, and with "WarningErrorInfo" structure in query info outBuffer.
                // To keep same model behavior, transfer the error code to STATUS_NOT_SUPPORTED.
                returnedStatus = FsaUtility.TransferExpectedResult<MessageStatus>(2674, MessageStatus.NOT_SUPPORTED, returnedStatus, site);
            }
            else if (fileInfoClass == FileSystemInfoClass.File_FsObjectId_Information && returnedStatus == MessageStatus.OBJECT_NAME_NOT_FOUND)
            {
                returnedStatus = FsaUtility.TransferExpectedResult<MessageStatus>(2674, MessageStatus.SUCCESS, returnedStatus, site);
                byteCount = FsInfoByteCount.SizeOf_FILE_FS_OBJECTID_INFORMATION;
            }
            return returnedStatus;
        }

        internal static MessageStatus WorkAroundSetFileLinkInfo(bool inputNameInvalid, bool replaceIfExist, MessageStatus returnedStatus, ITestSite site)
        {
            if (inputNameInvalid)
            {
                returnedStatus = FsaUtility.TransferExpectedResult<MessageStatus>(2941, MessageStatus.OBJECT_NAME_INVALID, returnedStatus, site);
            }
            else if (!replaceIfExist)
            {
                returnedStatus = FsaUtility.TransferExpectedResult<MessageStatus>(2954, MessageStatus.OBJECT_NAME_COLLISION, returnedStatus, site);
            }
            else
            {
                returnedStatus = FsaUtility.TransferExpectedResult<MessageStatus>(2974, MessageStatus.SUCCESS, returnedStatus, site);
            }
            return returnedStatus;
        }

        internal static MessageStatus WorkAroundSetFilePositionInfo(InputBufferSize inputBufferSize, InputBufferCurrentByteOffset currentByteOffset, MessageStatus returnedStatus, ITestSite site)
        {
            if (inputBufferSize != InputBufferSize.LessThan && currentByteOffset == InputBufferCurrentByteOffset.NotValid)
            {
                // For SMB server, when InputBufferCurrentByteOffset is not valid, it does not failed with STATUS_INVALID_PARAMETER.
                // Instead, it return with STATUS_SUCCESS, and setting "EaErrorOffset" warning in the response buffer to tell user the offset is not correct.
                // To keep same model behavior according to MS-FSA, transfer the error code to STATUS_INVALID_PARAMETER
                returnedStatus = FsaUtility.TransferExpectedResult<MessageStatus>(3004, MessageStatus.INVALID_PARAMETER, returnedStatus, site);
            }
            return returnedStatus;
        }

        internal static MessageStatus WorkaroundCreateFile(FileSystem fileSystem, FileNameStatus fileNameStatus, CreateOptions createOption, FileAccess desiredAccess, FileType openFileType, FileAttribute desiredFileAttribute, MessageStatus returnedStatus, ITestSite site)
        {
            if (desiredAccess == FileAccess.None || desiredAccess == FileAccess.INVALID_ACCESS_MASK)
            {
                returnedStatus = FsaUtility.TransferExpectedResult<MessageStatus>(366, MessageStatus.ACCESS_DENIED, returnedStatus, site);
            }
            else if (fileSystem == FileSystem.FAT32 &&
                fileNameStatus == FileNameStatus.BackslashName &&
                returnedStatus == MessageStatus.SUCCESS)
            {
                returnedStatus = FsaUtility.TransferExpectedResult<MessageStatus>(380, MessageStatus.OBJECT_NAME_INVALID, returnedStatus, site);
            }
            else if (createOption == CreateOptions.DELETE_ON_CLOSE &&
                (desiredAccess != FileAccess.DELETE))
            {
                returnedStatus = FsaUtility.TransferExpectedResult<MessageStatus>(371, MessageStatus.INVALID_PARAMETER, returnedStatus, site);
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
            // SMB1 server return ACCESS_DENIED instead of INVALID_PARAMETER when "CreateOptions.DELETE_ON_CLOS && !DesiredAccess.DELETE"
            if (createOption == CreateOptions.DELETE_ON_CLOSE &&
                desiredAccess != FileAccess.DELETE)
            {
                returnedStatus = FsaUtility.TransferExpectedResult<MessageStatus>(371, MessageStatus.INVALID_PARAMETER, returnedStatus, site);
            }
            else if (desiredAccess == FileAccess.None || desiredAccess == FileAccess.INVALID_ACCESS_MASK)
            {
                returnedStatus = FsaUtility.TransferExpectedResult<MessageStatus>(377, MessageStatus.ACCESS_DENIED, returnedStatus, site);
            }
            else if (createOption == CreateOptions.NO_INTERMEDIATE_BUFFERING &&
                (desiredAccess == FileAccess.FILE_APPEND_DATA || desiredAccess == FileAccess.FILE_ADD_SUBDIRECTORY))
            {
                returnedStatus = FsaUtility.TransferExpectedResult<MessageStatus>(376, MessageStatus.INVALID_PARAMETER, returnedStatus, site);
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

        internal static MessageStatus WorkaroundByteRangeLock(StreamType streamType, bool isConflicted, MessageStatus returnedStatus, ITestSite site)
        {
            if (streamType == StreamType.DirectoryStream)
            {
                returnedStatus = FsaUtility.TransferExpectedResult<MessageStatus>(376, MessageStatus.INVALID_PARAMETER, returnedStatus, site);
            }
            else if (isConflicted)
            {
                returnedStatus = FsaUtility.TransferExpectedResult<MessageStatus>(376, MessageStatus.LOCK_NOT_GRANTED, returnedStatus, site);
            }
            else
            {
                returnedStatus = FsaUtility.TransferExpectedResult<MessageStatus>(376, MessageStatus.SUCCESS, returnedStatus, site);
            }

            return returnedStatus;
        }

        internal static MessageStatus WorkaroundByteRangeUnlock(StreamType streamType, bool isConflicted, MessageStatus returnedStatus, ITestSite site)
        {
            if (streamType == StreamType.DirectoryStream)
            {
                returnedStatus = FsaUtility.TransferExpectedResult<MessageStatus>(376, MessageStatus.INVALID_PARAMETER, returnedStatus, site);
            }
            else if (isConflicted)
            {
                returnedStatus = FsaUtility.TransferExpectedResult<MessageStatus>(376, MessageStatus.RANGE_NOT_LOCKED, returnedStatus, site);
            }
            else
            {
                returnedStatus = FsaUtility.TransferExpectedResult<MessageStatus>(376, MessageStatus.SUCCESS, returnedStatus, site);
            }

            return returnedStatus;
        }
    }
}
