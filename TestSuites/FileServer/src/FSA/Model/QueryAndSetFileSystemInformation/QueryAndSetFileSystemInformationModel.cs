// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Modeling;
using Microsoft.Protocols.TestSuites.FileSharing.FSA.Adapter;
using System.Diagnostics.CodeAnalysis;

namespace Microsoft.Protocols.TestSuites.FileSharing.FSA.Model
{
    /// <summary>
    /// MS-FSA model program
    /// </summary>
    public static partial class ModelProgram
    {
        /// <summary>
        /// If the object store implements Query FileFsControlInformation
        /// </summary>
        /// Disable warning CA2211, because this action confuses with the actual model design if modify it.
        [SuppressMessage("Microsoft.Usage", "CA2211:NonConstantFieldsShouldNotBeVisible")]
        public static bool gIsImplementQueryFileFsControlInformation;

        /// <summary>
        /// If the object store implements Query FileFsObjectIdInformation
        /// </summary>
        /// Disable warning CA2211, because this action confuses with the actual model design if modify it.
        [SuppressMessage("Microsoft.Usage", "CA2211:NonConstantFieldsShouldNotBeVisible")]
        public static bool gIsImplementQueryFileFsObjectIdInformation;


        #region Get if the object store implements Query FileFsControlInformation

        /// <summary>
        /// The call part of the method GetIfImplementQueryFileFsControlInformation which is used to get if object store implements this function.
        /// This is a call-return pattern
        /// </summary>
        [Rule(Action = "call GetIfImplementQueryFileFsControlInformation(out _)")]
        public static void CallGetIfImplementQueryFileFsControlInformation()
        {
        }

        /// <summary>
        /// The return part of the method GetIfImplementQueryFileFsControlInformation which is used to get if this function is implemented.
        /// This is a call-returns pattern. 
        /// </summary>
        /// <param name="isImplemented">true: If the object store implements this functionality.</param>
        [Rule(Action = "return GetIfImplementQueryFileFsControlInformation(out isImplemented)")]
        public static void ReturnGetIfImplementQueryFileFsControlInformation(bool isImplemented)
        {
            gIsImplementQueryFileFsControlInformation = isImplemented;
        }

        #endregion

        #region Get if the object store implements Query FileFsObjectIdInformation

        /// <summary>
        /// The call part of the method GetIfImplementQueryFileFsObjectIdInformation which is used to get if object store implements this function.
        /// This is a call-return pattern
        /// </summary>
        [Rule(Action = "call GetIfImplementQueryFileFsObjectIdInformation(out _)")]
        public static void CallGetIfImplementQueryFileFsObjectIdInformation()
        {
        }

        /// <summary>
        /// The return part of the method GetIfImplementQueryFileFsObjectIdInformation which is used to get if this function is implemented.
        /// This is a call-returns pattern. 
        /// </summary>
        /// <param name="isImplemented">true: If the object store implements this functionality.</param>
        [Rule(Action = "return GetIfImplementQueryFileFsObjectIdInformation(out isImplemented)")]
        public static void ReturnGetIfImplementQueryFileFsObjectIdInformation(bool isImplemented)
        {
            gIsImplementQueryFileFsObjectIdInformation = isImplemented;
        }

        #endregion

        #region 3.1.5.12    Application Requests a Query of File System Information

        /// <summary>
        /// 3.1.5.12    Application Requests a Query of File System Information
        /// </summary>
        /// <param name="fileInfoClass">The type of information being queried, as specified in [MS-FSCC] section 2.5.</param>
        /// <param name="outBufSmall">True: If OutputBufferSize is smaller than the requested size. </param>
        /// <param name="byteCount">The number of bytes stored in OutputBuffer.</param>
        /// <returns>An NTSTATUS code that specifies the result</returns>
        [Rule]
        public static MessageStatus QueryFileSystemInfo(
            FileSystemInfoClass fileInfoClass,
            OutputBufferSize outBufSmall,
            out FsInfoByteCount byteCount)
        {
            byteCount = FsInfoByteCount.Zero;

            switch (fileInfoClass)
            {
                #region 3.1.5.12.1    FileFsVolumeInformation

                case (FileSystemInfoClass.File_FsVolumeInformation):
                    {
                        // If OutputBufferSize is smaller than BlockAlign( FieldOffset( FILE_FS_VOLUME_INFORMATION.VolumeLabel ), 8 )
                        if (outBufSmall == OutputBufferSize.LessThan)
                        {
                            Helper.CaptureRequirement(2638, @"[In FileFsVolumeInformation]Pseudocode for the operation is as follows:
                                If OutputBufferSize is smaller than BlockAlign( FieldOffset( FILE_FS_VOLUME_INFORMATION.VolumeLabel ), 8 ), the operation MUST be failed with STATUS_INFO_LENGTH_MISMATCH.");
                            return MessageStatus.INFO_LENGTH_MISMATCH;
                        }

                        Requirement.Capture(@"[2.1.5.12.1 FileFsVolumeInformation, Pseudocode for the operation is as follows:]
                            Upon successful completion of the operation, the object store MUST return: ByteCount set to FieldOffset(FILE_FS_VOLUME_INFORMATION.VolumeLabel) + BytesToCopy.");
                        byteCount = FsInfoByteCount.FieldOffset_FILE_FS_VOLUME_INFORMATION_VolumeLabel_Add_BytesToCopy;

                        Requirement.Capture(@"[2.1.5.12.1 FileFsVolumeInformation, Pseudocode for the operation is as follows:]
                            Upon successful completion of the operation, the object store MUST return: Status set to STATUS_BUFFER_OVERFLOW if BytesToCopy < OutputBuffer.VolumeLabelLength else STATUS_SUCCESS.");
                        return MessageStatus.SUCCESS;
                    }

                #endregion

                #region 3.1.5.12.2    FileFsLabelInformation //3.1.5.12.9    FileFsDriverPathInformation

                case (FileSystemInfoClass.File_FsLabelInformation):
                    {
                        Helper.CaptureRequirement(2751, @"[MS-FSCC] section 2.5, FileFsLabelInformation is intended for local use only. Query request does not match the usage, STATUS_INVALID_INFO_CLASS is returned.");
                        return MessageStatus.INVALID_INFO_CLASS;
                    }

                case (FileSystemInfoClass.File_FsDriverPath_Information):
                    {
                        if (outBufSmall == OutputBufferSize.LessThan)
                        {
                            Requirement.Capture(@"[2.1.5.12.4   FileFsDeviceInformation, Pseudocode for the operation is as follows:]
                                If OutputBufferSize is smaller than sizeof(FILE_FS_DRIVER_INFORMATION), the operation MUST be failed with STATUS_INFO_LENGTH_MISMATCH.");
                            return MessageStatus.INFO_LENGTH_MISMATCH;
                        }

                        Helper.CaptureRequirement(2764, @"[In FileFsDriverPathInformation]This operation MUST be failed with STATUS_NOT_SUPPORTED.");                        
                        return MessageStatus.NOT_SUPPORTED;

                    }

                #endregion

                #region 3.1.5.12.3    FileFsSizeInformation //3.1.5.12.7    FileFsFullSizeInformation

                case (FileSystemInfoClass.File_FsSizeInformation):
                    {
                        if (outBufSmall == OutputBufferSize.LessThan)
                        {
                            Requirement.Capture(@"[2.1.5.12.3 FileFsSizeInformation] Pseudocode for the operation is as follows:
                                If OutputBufferSize is smaller than sizeof(FILE_FS_SIZE_INFORMATION), the operation MUST be failed with STATUS_INFO_LENGTH_MISMATCH.");
                            return MessageStatus.INFO_LENGTH_MISMATCH;
                        }

                        Helper.CaptureRequirement(2672, @"[In FileFsSizeInformation, Pseudocode for the operation is as follows:]
                            Upon successful completion of the operation, the object store MUST return:ByteCount MUST be set to sizeof(FILE_FS_SIZE_INFORMATION).");
                        byteCount = FsInfoByteCount.SizeOf_FILE_FS_SIZE_INFORMATION;

                        Helper.CaptureRequirement(2673, @"[In FileFsSizeInformation, Pseudocode for the operation is as follows:]
                            Upon successful completion of the operation, the object store MUST return:Status set to STATUS_SUCCESS.");
                        return MessageStatus.SUCCESS;
                    }

                case (FileSystemInfoClass.File_FsFullSize_Information):
                    {
                        if (outBufSmall == OutputBufferSize.LessThan)
                        {
                            Requirement.Capture(@"[2.1.5.12.7 FileFsFullSizeInformation] Pseudocode for the operation is as follows:
                                If OutputBufferSize is smaller than sizeof(FILE_FS_FULL_SIZE_INFORMATION), the operation MUST be failed with STATUS_INFO_LENGTH_MISMATCH.");
                            return MessageStatus.INFO_LENGTH_MISMATCH;
                        }

                        Helper.CaptureRequirement(2729, @"[In FileFsFullSizeInformation,Pseudocode for the operation is as follows:]
                            Upon successful completion of the operation, the object store MUST return:ByteCount set to sizeof( FILE_FS_FULL_SIZE_INFORMATION ).");
                        byteCount = FsInfoByteCount.SizeOf_FILE_FS_FULL_SIZE_INFORMATION;

                        Helper.CaptureRequirement(2730, @"[In FileFsFullSizeInformation,Pseudocode for the operation is as follows:]
                            Upon successful completion of the operation, the object store MUST return:Status set to STATUS_SUCCESS.");
                        return MessageStatus.SUCCESS;
                    }

                #endregion

                #region 3.1.5.12.4    FileFsDeviceInformation

                case (FileSystemInfoClass.File_FsDevice_Information):
                    {
                        // If OutputBufferSize is smaller than sizeof( FILE_FS_DEVICE_INFORMATION )
                        if (outBufSmall == OutputBufferSize.LessThan)
                        {
                            Requirement.Capture(@"[2.1.5.12.4 FileFsDeviceInformation] Pseudocode for the operation is as follows: 
                                If OutputBufferSize is smaller than sizeof(FILE_FS_DEVICE_INFORMATION), the operation MUST be failed with STATUS_INFO_LENGTH_MISMATCH.");
                            return MessageStatus.INFO_LENGTH_MISMATCH;
                        }

                        Helper.CaptureRequirement(2753, @"[In FileFsDeviceInformation,Pseudocode for the operation is as follows:]
                            Upon successful completion of the operation, the object store MUST return:ByteCount set to sizeof( FILE_FS_DEVICE_INFORMATION ).");
                        byteCount = FsInfoByteCount.SizeOf_FILE_FS_DEVICE_INFORMATION;

                        Helper.CaptureRequirement(2680, @"[In FileFsDeviceInformation,Pseudocode for the operation is as follows:]
                            Upon successful completion of the operation, the object store MUST return:Status set to STATUS_SUCCESS.");
                        return MessageStatus.SUCCESS;
                    }

                #endregion

                #region 3.1.5.12.5    FileFsAttributeInformation
                case (FileSystemInfoClass.File_FsAttribute_Information):
                    {
                        //If OutputBufferSize is smaller than BlockAlign( FieldOffset( FILE_FS_ATTRIBUTE_INFORMATION.FileSystemName ), 4 )
                        if (outBufSmall == OutputBufferSize.LessThan)
                        {
                            Helper.CaptureRequirement(2691, @"[In FileFsAttributeInformation]Pseudocode for the operation is as follows:
                                If OutputBufferSize is smaller than BlockAlign( FieldOffset( FILE_FS_ATTRIBUTE_INFORMATION.FileSystemName ), 4 ), 
                                the operation MUST be failed with STATUS_INFO_LENGTH_MISMATCH.");
                            return MessageStatus.INFO_LENGTH_MISMATCH;
                        }

                        Requirement.Capture(@"[2.1.5.12.5 FileFsAttributeInformation], Pseudocode for the operation is as follows:
                            Upon successful completion of the operation, the object store MUST return: ByteCount set to FieldOffset(FILE_FS_ATTRIBUTE_INFORMATION.FileSystemName) + BytesToCopy.");
                        byteCount = FsInfoByteCount.FieldOffset_FILE_FS_ATTRIBUTE_INFORMATION_FileSystemName_Add_BytesToCopy;

                        Requirement.Capture(@"[2.1.5.12.5 FileFsAttributeInformation], Pseudocode for the operation is as follows:
                            Upon successful completion of the operation, the object store MUST return: Status set to STATUS_BUFFER_OVERFLOW if BytesToCopy < OutputBuffer.FileSystemNameLength else STATUS_SUCCESS.");
                        return MessageStatus.SUCCESS;
                    }
                #endregion

                #region 3.1.5.12.6    FileFsControlInformation

                case (FileSystemInfoClass.File_FsControlInformation):
                    {
                        // If OutputBufferSize is smaller than BlockAlign( sizeof( FILE_FS_CONTROL_INFORMATION ), 8 ) 
                        if (outBufSmall == OutputBufferSize.LessThan)
                        {
                            Helper.CaptureRequirement(2707, @"[In FileFsControlInformation] Pseudocode for the operation is as follows: 
                                If OutputBufferSize is smaller than BlockAlign( sizeof( FILE_FS_CONTROL_INFORMATION ), 8 ) the operation MUST be failed with STATUS_INFO_LENGTH_MISMATCH.");
                            return MessageStatus.INFO_LENGTH_MISMATCH;
                        }

                        if (!gIsImplementQueryFileFsControlInformation)
                        {
                            Helper.CaptureRequirement(7802, @"[In FileFsControlInformation,Pseudocode for the operation is as follows:] 
                                If the object store does not implement this functionality, the operation MUST be failed with STATUS_INVALID_PARAMETER.");
                            return MessageStatus.INVALID_PARAMETER;
                        }

                        if (!gisQuotasSupported)
                        {
                            Helper.CaptureRequirement(7803, @"[In FileFsControlInformation,Pseudocode for the operation is as follows:] 
                                If Open.File.Volume.IsQuotasSupported is FALSE, the operation MUST be failed with STATUS_VOLUME_NOT_UPGRADED");
                            return MessageStatus.VOLUME_NOT_UPGRADED;
                        }

                        Helper.CaptureRequirement(2715, @"[In FileFsControlInformation,Pseudocode for the operation is as follows:] 
                            Upon successful completion of the operation, the object store MUST return:ByteCount set to sizeof(FILE_FS_CONTROL_INFORMATION).");
                        byteCount = FsInfoByteCount.SizeOf_FILE_FS_CONTROL_INFORMATION;

                        Helper.CaptureRequirement(2716, @"[In FileFsControlInformation,Pseudocode for the operation is as follows:]
                            Upon successful completion of the operation, the object store MUST return:Status set to STATUS_SUCCESS.");
                        return MessageStatus.SUCCESS;
                    }

                #endregion

                #region 3.1.5.12.8    FileFsObjectIdInformation

                case (FileSystemInfoClass.File_FsObjectId_Information):
                    {
                        // If OutputBufferSize is less than the size of FILE_FS_OBJECTID_INFORMATION
                        if (outBufSmall == OutputBufferSize.LessThan)
                        {
                            Helper.CaptureRequirement(2755, @"[In FileFsObjectIdInformation]Pseudocode for the operation is as follows:
                                If OutputBufferSize is less than sizeof( FILE_FS_OBJECTID_INFORMATION ), 
                                the operation MUST be failed with STATUS_INFO_LENGTH_MISMATCH.");
                            return MessageStatus.INFO_LENGTH_MISMATCH;
                        }

                        if (!gIsImplementQueryFileFsObjectIdInformation)
                        {
                            Helper.CaptureRequirement(4811, @"[In FileFsObjectIdInformation,Pseudocode for the operation is as follows:] 
                                If the object store does not implement this functionality, the operation MUST be failed with STATUS_INVALID_PARAMETER.");
                            return MessageStatus.INVALID_PARAMETER;
                        }

                        if (!gisObjectIDsSupported)
                        {
                            Helper.CaptureRequirement(4812, @"[In FileFsObjectIdInformation,Pseudocode for the operation is as follows:] 
                                If Open.File.Volume.IsObjectIDsSupported is FALSE, the operation MUST be failed with STATUS_VOLUME_NOT_UPGRADED.");
                            return MessageStatus.VOLUME_NOT_UPGRADED;
                        }

                        Helper.CaptureRequirement(2761, @"[In FileFsObjectIdInformation,Pseudocode for the operation is as follows:]
                            Upon successful completion of the operation, the object store MUST return:ByteCount set to sizeof( FILE_FS_OBJECTID_INFORMATION ).");
                        byteCount = FsInfoByteCount.SizeOf_FILE_FS_OBJECTID_INFORMATION;

                        Helper.CaptureRequirement(2762, @"[In FileFsObjectIdInformation,Pseudocode for the operation is as follows:]
                            Upon successful completion of the operation, the object store MUST return:Status set to STATUS_SUCCESS.");
                        return MessageStatus.SUCCESS;
                    }

                #endregion

                #region 2.1.5.12.10   FileFsSectorSizeInformation

                case (FileSystemInfoClass.File_FsSectorSizeInformation):
                    {
                        if (outBufSmall == OutputBufferSize.LessThan)
                        {
                            Requirement.Capture(@"[2.1.5.12.10 FileFsSectorSizeInformation], Pseudocode for the operation is as follows:
                                If OutputBufferSize is smaller than sizeof(FILE_FS_SECTOR_SIZE_INFORMATION), the operation MUST be failed with STATUS_INFO_LENGTH_MISMATCH");
                            return MessageStatus.INFO_LENGTH_MISMATCH;
                        }

                        Requirement.Capture(@"[2.1.5.12.10 FileFsSectorSizeInformation], Pseudocode for the operation is as follows:
                            Upon successful completion of the operation, the object store MUST return: ByteCount set to the size of the FILE_FS_SECTOR_SIZE_INFORMATION structure.");
                        byteCount = FsInfoByteCount.SizeOf_FILE_FS_SECTOR_SIZE_INFORMATION;

                        Requirement.Capture(@"[2.1.5.12.10 FileFsSectorSizeInformation], Pseudocode for the operation is as follows:
                            Upon successful completion of the operation, the object store MUST return: Status set to STATUS_SUCCESS.");
                        return MessageStatus.SUCCESS;
                    }

                #endregion

                #region Information Class which is not defined in MS-FSCC
                case FileSystemInfoClass.Zero:
                    {                        
                        Requirement.Capture(@"[MS-FSCC 2.5 File System Information Classes] If an Information Class is specified that does not match the usage in the above table, STATUS_INVALID_INFO_CLASS MUST be returned.");
                        return MessageStatus.INVALID_INFO_CLASS;
                    }
                case FileSystemInfoClass.NOT_DEFINED_IN_FSCC:
                    {
                        if (outBufSmall == OutputBufferSize.LessThan)
                        {
                            Requirement.Capture(@"Negative test with artificial FileSystemInfoClass.NOT_DEFINED_IN_FSCC:
                                If OutputBufferSize is smaller than sizeof(NOT_DEFINED_IN_FSCC), the operation MUST be failed with STATUS_INFO_LENGTH_MISMATCH");
                            return MessageStatus.INFO_LENGTH_MISMATCH;
                        }

                        Requirement.Capture(@"[MS-SMB2 section 3.3.5.20.2 Handling SMB2_0_INFO_FILESYSTEM] If the store does not support the data requested, the server MUST fail the request with STATUS_NOT_SUPPORTED.");
                        return MessageStatus.NOT_SUPPORTED;
                    }
                #endregion

                default:
                    break;
            }

            Helper.CaptureRequirement(2630, @"[In Application Requests a Query of File System Information]On completion, the object store MUST return:
                [Status,OutputBuffer,ByteCount].");
            return MessageStatus.SUCCESS;
        }

        #endregion

        #region 3.1.5.15    Application Requests Setting of File System Information

        /// <summary>
        /// 3.1.5.15    Application Requests Setting of File System Information
        /// </summary>
        /// <param name="fileInfoClass">The type of information being applied, as specified in [MS-FSCC] section 2.5.</param>
        /// <param name="inputBufferSize">This is a abstracted enum to indicate inputBufferSize </param>
        /// <returns>An NTSTATUS code that specifies the result</returns>
        [Rule]
        public static MessageStatus SetFileSysInfo(
            FileSystemInfoClass fileInfoClass,
            InputBufferSize inputBufferSize
            )
        {
            //serverSet = validateServerSet;
            switch (fileInfoClass)
            {
                case (FileSystemInfoClass.File_FsVolumeInformation):
                    {
                        Helper.CaptureRequirement(3219, @"[In FileFsVolumeInformation]This operation MUST be failed with STATUS_NOT_SUPPORTED.");
                        return MessageStatus.NOT_SUPPORTED;
                    }
                case (FileSystemInfoClass.File_FsLabelInformation):
                    {
                        Helper.CaptureRequirement(3221, @"[In FileFsLabelInformation]This operation MUST be failed with STATUS_NOT_SUPPORTED.");
                        return MessageStatus.NOT_SUPPORTED;
                    }
                case (FileSystemInfoClass.File_FsSizeInformation):
                    {
                        Helper.CaptureRequirement(3223, @"[In FileFsSizeInformation]This operation MUST be failed with STATUS_NOT_SUPPORTED.");
                        return MessageStatus.NOT_SUPPORTED;
                    }
                case (FileSystemInfoClass.File_FsDevice_Information):
                    {
                        Helper.CaptureRequirement(3225, @"[In FileFsDeviceInformation]This operation MUST be failed with STATUS_NOT_SUPPORTED.");
                        return MessageStatus.NOT_SUPPORTED;
                    }
                case (FileSystemInfoClass.File_FsAttribute_Information):
                    {
                        Helper.CaptureRequirement(3227, @"[In FileFsAttributeInformation] This operation MUST be failed with STATUS_NOT_SUPPORTED.");
                        return MessageStatus.NOT_SUPPORTED;
                    }
                case (FileSystemInfoClass.File_FsControlInformation):
                    {
                        //If InputBufferSize is smaller than BlockAlign( sizeof( FILE_FS_CONTROL_INFORMATION ), 8 ) the operation MUST be failed with STATUS_INVALID_INFO_CLASS.
                        if (inputBufferSize == InputBufferSize.LessThan)
                        {
                            Helper.CaptureRequirement(4568, @"[In FileFsControlInformation]Pseudocode for the operation is as follows:
                                If InputBufferSize is smaller than BlockAlign( sizeof( FILE_FS_CONTROL_INFORMATION ), 8 ) 
                                the operation MUST be failed with STATUS_INVALID_INFO_CLASS.");
                            return MessageStatus.INVALID_INFO_CLASS;
                        }
                        if (!isObjectImplementedFunctionality)
                        {
                            Helper.CaptureRequirement(4570, @"[In FileFsControlInformation,Pseudocode for the operation is as follows:]
                                If the object store does not implement this functionality, the operation MUST be failed with STATUS_INVALID_PARAMETER.");
                            return MessageStatus.INVALID_PARAMETER;
                        }
                        if (!IsQuotasSupported)
                        {
                            Helper.CaptureRequirement(4571, @"[In FileFsControlInformation,Pseudocode for the operation is as follows:]
                                If Open.File.Volume.IsQuotasSupported is FALSE, the operation MUST be failed with STATUS_VOLUME_NOT_UPGRADED.");
                            return MessageStatus.STATUS_VOLUME_NOT_UPGRADED;
                        }
                        Helper.CaptureRequirement(4576, @"[In FileFsControlInformation,Pseudocode for the operation is as follows:]
                            Upon successful completion of the operation, the object store MUST return:Status set to STATUS_SUCCESS.");
                        return MessageStatus.SUCCESS;
                    }
                case (FileSystemInfoClass.File_FsFullSize_Information):
                    {
                        Helper.CaptureRequirement(3229, @"[In FileFsFullSizeInformation]This operation MUST be failed with STATUS_NOT_SUPPORTED.");
                        return MessageStatus.NOT_SUPPORTED;
                    }
                case (FileSystemInfoClass.File_FsObjectId_Information):
                    {
                        //If InputBufferSize is less than sizeof( FILE_FS_OBJECTID_INFORMATION ), the operation MUST be failed with STATUS_INVALID_INFO_CLASS.
                        if (inputBufferSize == InputBufferSize.LessThan)
                        {
                            Helper.CaptureRequirement(4578, @"[In FileFsObjectIdInformation]Pseudocode for the operation is as follows:
                                If InputBufferSize is less than sizeof( FILE_FS_OBJECTID_INFORMATION ), the operation MUST be failed with STATUS_INVALID_INFO_CLASS.");
                            return MessageStatus.INVALID_INFO_CLASS;
                        }
                        if (!isObjectImplementedFunctionality)
                        {
                            Helper.CaptureRequirement(4579, @"[In FileFsObjectIdInformation,Pseudocode for the operation is as follows:]
                                Support for ObjectIDs is OPTIONAL. If the object store does not implement this functionality, 
                                the operation MUST be failed with STATUS_INVALID_PARAMETER.");
                            return MessageStatus.INVALID_PARAMETER;
                        }
                        if (!isObjectIDsSupportedTrue)
                        {
                            Helper.CaptureRequirement(4580, @"[In FileFsObjectIdInformation,Pseudocode for the operation is as follows:]
                                If Open.File.Volume.IsObjectIDsSupported is FALSE, the operation MUST be failed with STATUS_VOLUME_NOT_UPGRADED.");
                            return MessageStatus.STATUS_VOLUME_NOT_UPGRADED;
                        }
                        Helper.CaptureRequirement(4583, @"[In FileFsObjectIdInformation,Pseudocode for the operation is as follows:]
                            Upon successful completion of the operation, the object store MUST return:Status set to STATUS_SUCCESS.");
                        return MessageStatus.SUCCESS;
                    }
                case FileSystemInfoClass.File_FsDriverPath_Information:
                    {
                        Helper.CaptureRequirement(3231, @"[In FileFsDriverPathInformation]This operation MUST be failed with STATUS_NOT_SUPPORTED.");
                        return MessageStatus.NOT_SUPPORTED;
                    }
            }
            Helper.CaptureRequirement(3216, @"[In Server Requests Setting of File System Information]The object store MUST return:[Status].");
            return MessageStatus.SUCCESS;
        }

        #endregion
    }
}
