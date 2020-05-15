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
        /// If stream rename is supported.
        /// </summary>
        /// Disable warning CA2211, because this action confuses with the actual model design if modify it.
        [SuppressMessage("Microsoft.Usage", "CA2211:NonConstantFieldsShouldNotBeVisible")]
        public static bool IsStreamRenameSupported;

        /// <summary>
        /// The call part of the method GetIfStreamRenameIsSupported.
        /// This is a call-return pattern
        /// </summary>
        [Rule(Action = "call GetIfStreamRenameIsSupported(out _)")]
        public static void CallGetIfStreamRenameIsSupported()
        {
        }

        /// <summary>
        /// The return part of the method GetIfStreamRenameIsSupported.
        /// This is a call-returns pattern. 
        /// </summary>
        /// <param name="isSupported">true: if stream rename is supported.</param>
        [Rule(Action = "return GetIfStreamRenameIsSupported(out isSupported)")]
        public static void ReturnGetIfStreamRenameIsSupported(bool isSupported)
        {
            IsStreamRenameSupported = isSupported;
        }


        #region 3.1.5.11    Application Requests a Query of File Information

        #region QueryFileInfoPart1
        /// <summary>
        /// 3.1.5.11.1     FileAccessInformation
        /// 3.1.5.11.2     FileAlignmentInformation
        /// 3.1.5.11.3     FileAllInformation
        /// 3.1.5.11.4     FileAlternateNameInformation
        /// 3.1.5.11.5     FileAttributeTagInformation
        /// 3.1.5.11.6     FileBasicInformation
        /// 3.1.5.11.7     FileBothDirectoryInformation
        /// 3.1.5.11.8     FileCompressionInformation
        /// 3.1.5.11.9     FileDirectoryInformation
        /// 3.1.5.11.10    FileEaInformation
        /// 3.1.5.11.11    FileFullDirectoryInformation
        /// 3.1.5.11.12    FileFullEaInformation
        /// 3.1.5.11.13    FileHardLinkInformation
        /// 3.1.5.11.14    FileIdBothDirectoryInformation
        /// 3.1.5.11.15    FileIdFullDirectoryInformation
        /// 3.1.5.11.16    FileIdGlobalTxDirectoryInformation
        /// 3.1.5.11.19    FileNameInformation
        /// 3.1.5.11.20    FileNamesInformation
        /// 3.1.5.11.22    FileObjectIdInformation
        /// 3.1.5.11.26    FileSfioReserveInformation
        /// 3.1.5.11.28    FileStandardLinkInformation
        /// 3.1.5.11.17    FileInternalInformation
        /// 3.1.5.11.18    FileModeInformation
        /// 3.1.5.11.21    FileNetworkOpenInformation
        /// 3.1.5.11.23    FilePositionInformation
        /// </summary>
        /// <param name="byteCount">To indicate ByteCount</param>
        /// <param name="fileInfoClass">To indicate FileInfoClass</param>
        /// <param name="outputBuffer">To indicate OutputBuffer</param>
        /// <param name="outputBufferSize">To indicate outputBuffer size</param>
        /// <returns>An NTSTATUS code that specifies the result</returns>
        /// Disable warning CA1502, because this action confuses with the actual model design if modify it.
        [SuppressMessage("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        // Disable warning CA1505, because this action confuses with the actual model design if modify it.
        [SuppressMessage("Microsoft.Maintainability", "CA1505:AvoidUnmaintainableCode")]
        [Rule]
        public static MessageStatus QueryFileInfoPart1(
            FileInfoClass fileInfoClass,
            OutputBufferSize outputBufferSize,
            out ByteCount byteCount,
            out OutputBuffer outputBuffer
            )
        {
            byteCount = ByteCount.NotSet;
            outputBuffer = new OutputBuffer();
            if (fileInfoClass == FileInfoClass.NOT_DEFINED_IN_FSCC)
            {
                Helper.CaptureRequirement(1426, @"[In Server Requests a Query of File Information ]
                    If FileInformationClass is not defined in [MS-FSCC] section 2.4, the operation MUST be failed with STATUS_INVALID_INFO_CLASS.");
                return MessageStatus.INVALID_INFO_CLASS;
            }

            switch (fileInfoClass)
            {
                #region  3.1.5.11.1    FileAccessInformation

                case (FileInfoClass.FILE_ACCESS_INFORMATION):
                    {
                        if (outputBufferSize == OutputBufferSize.LessThan)
                        {
                            Helper.CaptureRequirement(1428, @"[In FileAccessInformation]Pseudocode for the operation is as follows:
                                If OutputBufferSize is smaller than sizeof( FILE_ACCESS_INFORMATION ), 
                                the operation MUST be failed with STATUS_INFO_LENGTH_MISMATCH.");
                            return MessageStatus.INFO_LENGTH_MISMATCH;
                        }

                        outputBuffer.AccessFlags = gOpenGrantedAccess;
                        Helper.CaptureRequirement(1430, @"[In FileAccessInformation,Pseudocode for the operation is as follows:]
                            OutputBuffer MUST be filled out as follows:OutputBuffer.AccessFlags set to Open.GrantedAccess.");

                        byteCount = ByteCount.SizeofFILE_ACCESS_INFORMATION;
                        Helper.CaptureRequirement(3967, @"[In FileAccessInformation]Upon successful completion of the operation, 
                            the object store MUST return: ByteCount set to sizeof( FILE_ACCESS_INFORMATION ).");
                        Helper.CaptureRequirement(3968, @"[In FileAccessInformation]Upon successful completion of the operation, 
                            the object store MUST return:Status set to STATUS_SUCCESS.");
                        Helper.CaptureRequirement(1421, @"[In Server Requests a Query of File Information ]On completion, 
                            the object store MUST return:[Status,OutputBuffer,ByteCount].");
                        return MessageStatus.SUCCESS;
                    }

                #endregion

                #region  3.1.5.11.2    FileAlignmentInformation

                case (FileInfoClass.FILE_ALIGNMENT_INFORMATION):
                    {
                        if (outputBufferSize == OutputBufferSize.LessThan)
                        {
                            Helper.CaptureRequirement(1433, @"[In FileAlignmentInformation]Pseudocode for the operation is as follows:
                                If OutputBufferSize is smaller than sizeof( FILE_ALIGNMENT_INFORMATION ), 
                                the operation MUST be failed with Status STATUS_INFO_LENGTH_MISMATCH.");
                            return MessageStatus.INFO_LENGTH_MISMATCH;
                        }
                        byteCount = ByteCount.SizeofFILE_ALIGNMENT_INFORMATION;
                        Helper.CaptureRequirement(1435, @"[In FileAlignmentInformation,Pseudocode for the operation is as follows:]
                            Upon successful completion of the operation, the object store MUST return:ByteCount set to sizeof( FILE_ALIGNMENT INFORMATION ).");
                        Helper.CaptureRequirement(1436, @"[In FileAlignmentInformation,Pseudocode for the operation is as follows:]
                            Upon successful completion of the operation, the object store MUST return:Status set to STATUS_SUCCESS.");
                        Helper.CaptureRequirement(1421, @"[In Server Requests a Query of File Information ]On completion, 
                            the object store MUST return:[Status,OutputBuffer,ByteCount].");
                        return MessageStatus.SUCCESS;
                    }

                #endregion

                #region  3.1.5.11.3    FileAllInformation

                case (FileInfoClass.FILE_ALL_INFORMATION):
                    {
                        if (outputBufferSize == OutputBufferSize.LessThan)
                        {
                            Helper.CaptureRequirement(3970, @"[In FileAllInformation] Pseudocode for the operation is as follows:
                                If OutputBufferSize is smaller than BlockAlign( FieldOffset( FILE_ALL_INFORMATION.NameInformation.FileName ) + 2, 8 ),
                                the operation MUST be failed with STATUS_INFO_LENGTH_MISMATCH.");
                            return MessageStatus.INFO_LENGTH_MISMATCH;
                        }
                        byteCount = ByteCount.FieldOffsetFILE_ALL_INFORMATION_NameInformationAddNameInformationLength;
                        Helper.CaptureRequirement(3980, @"[In FileAllInformation,Pseudocode for the operation is as follows:] 
                            Upon successful completion of the operation, the object store MUST return:
                            ByteCount set to FieldOffset( FILE_ALL_INFORMATION.NameInformation ) + NameInformationLength.");
                        Helper.CaptureRequirement(1421, @"[In Server Requests a Query of File Information ]On completion, 
                            the object store MUST return:[Status,OutputBuffer,ByteCount].");
                        return MessageStatus.SUCCESS;
                    }

                #endregion

                #region  3.1.5.11.4    FileAlternateNameInformation

                case (FileInfoClass.FILE_ALTERNATENAME_INFORMATION):
                    {

                        if (outputBufferSize == OutputBufferSize.LessThan)
                        {
                            Helper.CaptureRequirement(1438, @"[In FileAlternateNameInformation]Pseudocode for the operation is as follows:
                                If OutputBufferSize is smaller than BlockAlign( FieldOffset( FILE_NAME_INFORMATION.FileName ) + 2, 4 ), 
                                the operation MUST be failed with STATUS_INFO_LENGTH_MISMATCH.");
                            return MessageStatus.INFO_LENGTH_MISMATCH;
                        }

                        byteCount = ByteCount.FieldOffsetFILE_NAME_INFORMATION_FileNameAddOutputBuffer_FileNameLength;
                        Helper.CaptureRequirement(1442, @"[In FileAlternateNameInformation,Pseudocode for the operation is as follows:]
                            Upon successful completion of the operation, the object store MUST return:
                            ByteCount set to FieldOffset( FILE_NAME_INFORMATION.FileName ) + OutputBuffer.FileNameLength.");
                        Helper.CaptureRequirement(1443, @"[In FileAlternateNameInformation,Pseudocode for the operation is as follows:]
                            Upon successful completion of the operation, the object store MUST return:Status set to STATUS_SUCCESS.");
                        Helper.CaptureRequirement(1421, @"[In Server Requests a Query of File Information ]On completion, 
                            the object store MUST return:[Status,OutputBuffer,ByteCount].");
                        return MessageStatus.SUCCESS;
                    }

                #endregion

                #region  3.1.5.11.5    FileAttributeTagInformation

                case (FileInfoClass.FILE_ATTRIBUTETAG_INFORMATION):
                    {

                        if (outputBufferSize == OutputBufferSize.LessThan)
                        {
                            Helper.CaptureRequirement(1445, @"[In FileAttributeTagInformation,Pseudocode for the operation is as follows:]
                                If OutputBufferSize is smaller than sizeof( FILE_ATTRIBUTE_TAG_INFORMATION ), the operation MUST be failed with STATUS_INFO_LENGTH_MISMATCH.");
                            return MessageStatus.INFO_LENGTH_MISMATCH;
                        }

                        byteCount = ByteCount.SizeofFILE_ATTRIBUTE_TAG_INFORMATION;
                        Helper.CaptureRequirement(1464, @"[In FileAttributeTagInformation,Pseudocode for the operation is as follows:]
                            Upon successful completion of the operation, the object store MUST return:ByteCount set to sizeof(FILE_ATTRIBUTE_TAG_INFORMATION ).");
                        Helper.CaptureRequirement(1465, @"[In FileAttributeTagInformation,Pseudocode for the operation is as follows:]
                            Upon successful completion of the operation, the object store MUST return:Status set to STATUS_SUCCESS.");
                        Helper.CaptureRequirement(1421, @"[In Server Requests a Query of File Information ]On completion, 
                            the object store MUST return:[Status,OutputBuffer,ByteCount].");
                        return MessageStatus.SUCCESS;
                    }

                #endregion

                #region  3.1.5.11.6    FileBasicInformation

                case (FileInfoClass.FILE_BASIC_INFORMATION):
                    {
                        if (outputBufferSize == OutputBufferSize.LessThan)
                        {
                            Helper.CaptureRequirement(1467, @"[In FileBasicInformation]Pseudocode for the operation is as follows:If OutputBufferSize is smaller than BlockAlign( sizeof( FILE_BASIC_INFORMATION ), 8 ), the operation MUST be failed with STATUS_INFO_LENGTH_MISMATCH.");
                            return MessageStatus.INFO_LENGTH_MISMATCH;
                        }

                        byteCount = ByteCount.SizeofFILE_BASIC_INFORMATION;
                        Helper.CaptureRequirement(3982, @"[In FileBasicInformation]Upon successful completion of the operation, 
                            the object store MUST return:ByteCount set to sizeof( FILE_BASIC_INFORMATION ).");
                        Helper.CaptureRequirement(3983, @"[In FileBasicInformation]Upon successful completion of the operation, 
                            the object store MUST return:Status set to STATUS_SUCCESS.");
                        Helper.CaptureRequirement(1421, @"[In Server Requests a Query of File Information ]On completion, 
                            the object store MUST return:[Status,OutputBuffer,ByteCount].");
                        return MessageStatus.SUCCESS;
                    }

                #endregion

                #region  3.1.5.11.7    FileBothDirectoryInformation

                case (FileInfoClass.FILE_BOTH_DIR_INFORMATION):
                    {
                        Condition.IsTrue(outputBufferSize == OutputBufferSize.NotLessThan);
                        // This operation is not supported and MUST be failed with STATUS_NOT_SUPPORTED.
                        Helper.CaptureRequirement(1605, @"[In FileBothDirectoryInformation, This operation]MUST be failed with STATUS_NOT_SUPPORTED.");
                        //return MessageStatus.NOT_SUPPORTED;
                        //this is a TD issue ,so change the return value to return MessageStatus.INVALID_INFO_CLASS;
                        return MessageStatus.INVALID_INFO_CLASS;
                    }

                #endregion

                #region  3.1.5.11.8    FileCompressionInformation

                case (FileInfoClass.FILE_COMPRESSION_INFORMATION):
                    {
                        if (outputBufferSize == OutputBufferSize.LessThan)
                        {
                            Helper.CaptureRequirement(1489, @"[In FileCompressionInformation]Pseudocode for the operation is as follows:
                                If OutputBufferSize is smaller than sizeof( FILE_COMPRESSION_INFORMATION ), 
                                the operation MUST be failed with STATUS_INFO_LENGTH_MISMATCH.");
                            return MessageStatus.INFO_LENGTH_MISMATCH;
                        }
                        byteCount = ByteCount.SizeofFILE_COMPRESSION_INFORMATION;
                        Helper.CaptureRequirement(3984, @"[In FileCompressionInformation]Upon successful completion of the operation, 
                            the object store MUST return:ByteCount set to sizeof(FILE_COMPRESSION_INFORMATION ).");
                        Helper.CaptureRequirement(3985, @"[In FileCompressionInformation]Upon successful completion of the operation, 
                            the object store MUST return:Status set to STATUS_SUCCESS.");
                        Helper.CaptureRequirement(1421, @"[In Server Requests a Query of File Information ]On completion, 
                            the object store MUST return:[Status,OutputBuffer,ByteCount].");
                        return MessageStatus.SUCCESS;
                    }

                #endregion

                #region  3.1.5.11.9    FileDirectoryInformation

                case (FileInfoClass.FILE_DIRECTORY_INFORMATION):
                    {
                        Condition.IsTrue(outputBufferSize == OutputBufferSize.NotLessThan);
                        // This operation is not supported and MUST be failed with STATUS_NOT_SUPPORTED.
                        Helper.CaptureRequirement(1603, @"[In FileDirectoryInformation,This operation ]MUST be failed with STATUS_NOT_SUPPORTED.");
                        //return MessageStatus.NOT_SUPPORTED;
                        //this is a TD issue ,so change the return value to return MessageStatus.INVALID_INFO_CLASS;
                        return MessageStatus.INVALID_INFO_CLASS;
                    }

                #endregion

                #region  3.1.5.11.10    FileEaInformation

                case (FileInfoClass.FILE_EA_INFORMATION):
                    {
                        if (outputBufferSize == OutputBufferSize.LessThan)
                        {
                            Helper.CaptureRequirement(3986, @"[In FileEaInformation]Pseudocode for the operation is as follows:
                                If OutputBufferSize is smaller than sizeof( FILE_EA_INFORMATION ), the operation MUST be failed with STATUS_INFO_LENGTH_MISMATCH.");
                            return MessageStatus.INFO_LENGTH_MISMATCH;
                        }
                        byteCount = ByteCount.SizeofFILE_EA_INFORMATION;
                        Helper.CaptureRequirement(3987, @"[In FileEaInformation,Pseudocode for the operation is as follows:]
                            Upon successful completion of the operation, the object store MUST return:ByteCount set to sizeof( FILE_EA_INFORMATION ).");
                        Helper.CaptureRequirement(3988, @"[In FileEaInformation,Pseudocode for the operation is as follows:]
                            Upon successful completion of the operation, the object store MUST return:Status set to STATUS_SUCCESS.");
                        Helper.CaptureRequirement(1421, @"[In Server Requests a Query of File Information ]On completion, 
                            the object store MUST return:[Status,OutputBuffer,ByteCount].");
                        return MessageStatus.SUCCESS;
                    }

                #endregion

                #region  3.1.5.11.11    FileFullDirectoryInformation

                case (FileInfoClass.FILE_FULL_DIR_INFORMATION):
                    {
                        Condition.IsTrue(outputBufferSize == OutputBufferSize.NotLessThan);
                        // This operation is not supported and MUST be failed with STATUS_NOT_SUPPORTED.
                        Helper.CaptureRequirement(1601, @"[In FileFullDirectoryInformation,This operation ]MUST be failed with STATUS_NOT_SUPPORTED.");
                        //return MessageStatus.NOT_SUPPORTED;
                        //this is a TD issue ,so change the return value to return MessageStatus.INVALID_INFO_CLASS, acctually ,it return INVALID_INFO_CLASS
                        return MessageStatus.INVALID_INFO_CLASS;
                    }

                #endregion

                #region  3.1.5.11.12    FileFullEaInformation

                case (FileInfoClass.FILE_FULLEA_INFORMATION):
                    {
                        if (outputBufferSize == OutputBufferSize.LessThan)
                        {
                            Helper.CaptureRequirement(3994, @"[In FileFullEaInformation,Pseudocode for the operation is as follows:]
                                Upon successful completion of the operation, the object store MUST return:Status set to:STATUS_BUFFER_TOO_SMALL 
                                if OutputBufferSize is too small to hold Open.NextEaEntry.  No entries are returned.");
                            return MessageStatus.BUFFER_TOO_SMALL;
                        }
                        byteCount = ByteCount.SizeofFILE_FULL_EA_INFORMATION;
                        Helper.CaptureRequirement(3992, @"[In FileFullEaInformation,Pseudocode for the operation is as follows:]
                            Upon successful completion of the operation, the object store MUST return:ByteCount set to the size, in bytes, 
                            of all FILE_FULL_EA_INFORMATION entries returned.");
                        return MessageStatus.SUCCESS;

                    }

                #endregion

                #region 3.1.5.11.13    FileHardLinkInformation

                case (FileInfoClass.FILE_LINKS_INFORMATION):
                    {
                        Condition.IsTrue(outputBufferSize == OutputBufferSize.NotLessThan);
                        // This operation is not supported and MUST be failed with STATUS_NOT_SUPPORTED.
                        Helper.CaptureRequirement(1593, @"[In FileHardLinkInformation,This operation] MUST be failed with STATUS_NOT_SUPPORTED.");
                        //return MessageStatus.NOT_SUPPORTED;
                        //this is a TD issue ,so change the return value to return MessageStatus.INVALID_INFO_CLASS;
                        return MessageStatus.INVALID_INFO_CLASS;
                    }

                #endregion

                #region 3.1.5.11.14  FileIdBothDirectoryInformation

                case (FileInfoClass.FILE_ID_BOTH_DIR_INFORMATION):
                    {
                        Condition.IsTrue(outputBufferSize == OutputBufferSize.NotLessThan);
                        // This operation is not supported and MUST be failed with STATUS_NOT_SUPPORTED.
                        Helper.CaptureRequirement(1595, @"[In FileIdBothDirectoryInformation,This operation] MUST be failed with STATUS_NOT_SUPPORTED.");
                        //return MessageStatus.NOT_SUPPORTED;
                        //this is a TD issue ,so change the return value to return MessageStatus.INVALID_INFO_CLASS;
                        return MessageStatus.INVALID_INFO_CLASS;
                    }

                #endregion

                #region 3.1.5.11.15  FileIdFullDirectoryInformation

                case (FileInfoClass.FILE_ID_FULL_DIR_INFORMATION):
                    {
                        Condition.IsTrue(outputBufferSize == OutputBufferSize.NotLessThan);
                        // This operation is not supported and MUST be failed with STATUS_NOT_SUPPORTED.
                        Helper.CaptureRequirement(1597, @"[In FileIdFullDirectoryInformation,This operation]MUST be failed with STATUS_NOT_SUPPORTED.");
                        //return MessageStatus.NOT_SUPPORTED;
                        //this is a TD issue ,so change the return value to return MessageStatus.INVALID_INFO_CLASS;
                        return MessageStatus.INVALID_INFO_CLASS;
                    }

                #endregion

                #region 3.1.5.11.16  FileIdGlobalTxDirectoryInformation

                case (FileInfoClass.FILE_ID_GLOBAL_TX_DIR_INFORMATION):
                    {
                        Condition.IsTrue(outputBufferSize == OutputBufferSize.NotLessThan);
                        // This operation is not supported and MUST be failed with STATUS_NOT_SUPPORTED.
                        Helper.CaptureRequirement(1599, @"[In FileIdGlobalTxDirectoryInformation,This operation] MUST be failed with STATUS_NOT_SUPPORTED.");
                        //return MessageStatus.NOT_SUPPORTED;
                        //this is a TD issue ,so change the return value to return MessageStatus.INVALID_INFO_CLASS;
                        return MessageStatus.INVALID_INFO_CLASS;
                    }

                #endregion

                #region 3.1.5.11.17    FileInternalInformation

                case (FileInfoClass.FILE_INTERNAL_INFORMATION):
                    {
                        if (outputBufferSize == OutputBufferSize.LessThan)
                        {
                            Helper.CaptureRequirement(1524, @"[In FileInternalInformation]Pseudocode for the operation is as follows:
                                If OutputBufferSize is smaller than sizeof( FILE_INTERNAL_INFORMATION ), 
                                the operation MUST be failed with STATUS_INFO_LENGTH_MISMATCH.");
                            return MessageStatus.INFO_LENGTH_MISMATCH;
                        }
                        byteCount = ByteCount.SizeofFILE_INTERNAL_INFORMATION;
                        Helper.CaptureRequirement(3997, @"[In FileInternalInformation]Upon successful completion of the operation, 
                            the object store MUST return: ByteCount set to sizeof( FILE_INTERNAL_INFORMATION ).");
                        Helper.CaptureRequirement(3998, @"[In FileInternalInformation]Upon successful completion of the operation, 
                            the object store MUST return:Status set to STATUS_SUCCESS.");
                        Helper.CaptureRequirement(1421, @"[In Server Requests a Query of File Information ]On completion, 
                            the object store MUST return:[Status,OutputBuffer,ByteCount].");
                        return MessageStatus.SUCCESS;
                    }

                #endregion

                #region 3.1.5.11.18    FileModeInformation

                case (FileInfoClass.FILE_MODE_INFORMATION):
                    {
                        if (outputBufferSize == OutputBufferSize.LessThan)
                        {
                            Helper.CaptureRequirement(1529, @"[In FileModeInformation]Pseudocode for the operation is as follows:
                                If OutputBufferSize is smaller than sizeof(FILE_MODE_INFORMATION ), the operation MUST be failed with STATUS_INFO_LENGTH_MISMATCH.");
                            return MessageStatus.INFO_LENGTH_MISMATCH;
                        }
                        byteCount = ByteCount.SizeofFILE_MODE_INFORMATION;
                        Helper.CaptureRequirement(4000, @"[In FileModeInformation,Pseudocode for the operation is as follows:]
                            Upon successful completion of the operation, the object store MUST return:ByteCount set to sizeof( FILE_MODE_INFORMATION ).");
                        Helper.CaptureRequirement(4001, @"[In FileModeInformation,Pseudocode for the operation is as follows:]
                            Upon successful completion of the operation, the object store MUST return:Status set to STATUS_SUCCESS.");
                        Helper.CaptureRequirement(1421, @"[In Server Requests a Query of File Information ]On completion, 
                            the object store MUST return:[Status,OutputBuffer,ByteCount].");
                        return MessageStatus.SUCCESS;
                    }

                #endregion

                #region 3.1.5.11.19  FileNameInformation

                case (FileInfoClass.FILE_NAME_INFORMATION):
                    {
                        Condition.IsTrue(outputBufferSize == OutputBufferSize.NotLessThan);
                        // This operation is not supported and MUST be failed with STATUS_NOT_SUPPORTED.
                        Helper.CaptureRequirement(1591, @"[In FileNameInformation]This operation MUST be failed with STATUS_NOT_SUPPORTED.");
                        //return MessageStatus.NOT_SUPPORTED;
                        //this is a TD issue ,so change the return value to return MessageStatus.INVALID_INFO_CLASS;
                        return MessageStatus.INVALID_INFO_CLASS;
                    }

                #endregion

                #region 3.1.5.11.20  FileNamesInformation

                case (FileInfoClass.FILE_NAMES_INFORMATION):
                    {
                        Condition.IsTrue(outputBufferSize == OutputBufferSize.NotLessThan);
                        // This operation is not supported and MUST be failed with STATUS_NOT_SUPPORTED.
                        Helper.CaptureRequirement(1587, @"[In FileNamesInformation]If used to query file information STATUS_NOT_SUPPORTED MUST be returned.");
                        //return MessageStatus.NOT_SUPPORTED;
                        //this is a TD issue ,so change the return value to return MessageStatus.INVALID_INFO_CLASS;
                        return MessageStatus.INVALID_INFO_CLASS;
                    }

                #endregion

                #region 3.1.5.11.21    FileNetworkOpenInformation

                case (FileInfoClass.FILE_NETWORKOPEN_INFORMATION):
                    {
                        if (outputBufferSize == OutputBufferSize.LessThan)
                        {
                            Helper.CaptureRequirement(4002, @"[In FileNetworkOpenInformation]Pseudocode for the operation is as follows:
                                If OutputBufferSize is smaller than sizeof( FILE_NETWORK_OPEN_INFORMATION ), 
                                the operation MUST be failed with STATUS_INFO_LENGTH_MISMATCH.");
                            return MessageStatus.INFO_LENGTH_MISMATCH;
                        }

                        byteCount = ByteCount.SizeofFILE_NETWORK_OPEN_INFORMATION;
                        Helper.CaptureRequirement(4004, @"[In FileNetworkOpenInformation,Pseudocode for the operation is as follows:]
                            Upon successful completion of the operation, the object store MUST return:ByteCount set to sizeof( FILE_NETWORK_OPEN_INFORMATION ).");
                        Helper.CaptureRequirement(4005, @"[In FileNetworkOpenInformation,Pseudocode for the operation is as follows:]U
                            pon successful completion of the operation, the object store MUST return:Status set to STATUS_SUCCESS.");
                        Helper.CaptureRequirement(1421, @"[In Server Requests a Query of File Information ]On completion, 
                            the object store MUST return:[Status,OutputBuffer,ByteCount].");
                        return MessageStatus.SUCCESS;
                    }

                #endregion

                #region 3.1.5.11.22  FileObjectIdInformation

                case (FileInfoClass.FILE_OBJECTID_INFORMATION):
                    {

                        Condition.IsTrue(outputBufferSize == OutputBufferSize.NotLessThan);
                        // This operation is not supported and MUST be failed with STATUS_NOT_SUPPORTED.
                        Helper.CaptureRequirement(1585, @"[In FileObjectIdInformation,This operation]MUST be failed with STATUS_NOT_SUPPORTED.");
                        //return MessageStatus.NOT_SUPPORTED;
                        //this is a TD issue ,so change the return value to return MessageStatus.INVALID_INFO_CLASS;
                        return MessageStatus.INVALID_INFO_CLASS;
                    }

                #endregion

                #region 3.1.5.11.23    FilePositionInformation

                case (FileInfoClass.FILE_POSITION_INFORMATION):
                    {
                        if (outputBufferSize == OutputBufferSize.LessThan)
                        {
                            Helper.CaptureRequirement(1561, @"[In FilePositionInformation]Pseudocode for the operation is as follows:
                                If OutputBufferSize is less than the size, in bytes, of the FILE_POSITION_INFORMATION structure, 
                                the operation MUST be failed with STATUS_INFO_LENGTH_MISMATCH.");
                            return MessageStatus.INFO_LENGTH_MISMATCH;
                        }
                        Helper.CaptureRequirement(1563, @"[In FilePositionInformation,Pseudocode for the operation is as follows:]
                            The operation returns STATUS_SUCCESS.<57>");
                        Helper.CaptureRequirement(1421, @"[In Server Requests a Query of File Information ]On completion, 
                            the object store MUST return:[Status,OutputBuffer,ByteCount].");
                        return MessageStatus.SUCCESS;
                    }

                #endregion

                #region 3.1.5.11.24  FileQuotaInformation

                case (FileInfoClass.FILE_QUOTA_INFORMATION):
                    {
                        Condition.IsTrue(outputBufferSize == OutputBufferSize.NotLessThan);
                        // This operation is not supported and MUST be failed with STATUS_NOT_SUPPORTED.
                        Helper.CaptureRequirement(2525, @"[In FileQuotaInformation]If used to query file information STATUS_NOT_SUPPORTED MUST be returned.");
                        Helper.CaptureRequirement(2524, @"[In FileQuotaInformation]This operation is not supported as a file information class, 
                            it is only supported as a directory enumeration class, see section 3.1.5.5.2.");
                        //return MessageStatus.NOT_SUPPORTED;
                        //this is a TD issue ,so change the return value to return MessageStatus.INVALID_INFO_CLASS;
                        return MessageStatus.INVALID_INFO_CLASS;
                    }

                #endregion

                #region 3.1.5.11.25  FileReparsePointInformation

                case (FileInfoClass.FILE_REPARSE_POINT_INFORMATION):
                    {
                        Condition.IsTrue(outputBufferSize == OutputBufferSize.NotLessThan);
                        // This operation is not supported and MUST be failed with STATUS_NOT_SUPPORTED.
                        Helper.CaptureRequirement(2559, @"[In FileReparsePointInformation ]  
                           If used to query file information STATUS_NOT_SUPPORTED MUST be returned.");
                        Helper.CaptureRequirement(2558, @"[In FileReparsePointInformation ] This operation is not supported as a file information class, 
                            it is only supported as a directory enumeration class, see section 3.1.5.5.3.");
                        //return MessageStatus.NOT_SUPPORTED;
                        //this is a TD issue ,so change the return value to return MessageStatus.INVALID_INFO_CLASS;
                        return MessageStatus.INVALID_INFO_CLASS;
                    }

                #endregion

                #region 3.1.5.11.26  FileSfioReserveInformation

                case (FileInfoClass.FILE_SFIO_RESERVE_INFORMATION):
                    {
                        Condition.IsTrue(outputBufferSize == OutputBufferSize.NotLessThan);
                        // This operation is not supported and MUST be failed with STATUS_NOT_SUPPORTED.
                        Helper.CaptureRequirement(2734, @"[In FileSfioReserveInformation] This operation MUST be failed with STATUS_NOT_SUPPORTED.");
                        //return MessageStatus.NOT_SUPPORTED;
                        //this is a TD issue ,so change the return value to return MessageStatus.INVALID_INFO_CLASS;
                        return MessageStatus.INVALID_INFO_CLASS;
                    }

                #endregion

                #region 3.1.5.11.27    FileStandardInformation

                case (FileInfoClass.FILE_STANDARD_INFORMATION):
                    {
                        if (outputBufferSize == OutputBufferSize.LessThan)
                        {
                            Helper.CaptureRequirement(4006, @"[In FileStandardInformation]Pseudocode for the operation is as follows:
                                If OutputBufferSize is smaller than sizeof( FILE_STANDARD_INFORMATION ), 
                                the operation MUST be failed with STATUS_INFO_LENGTH_MISMATCH.");
                            return MessageStatus.INFO_LENGTH_MISMATCH;
                        }
                        byteCount = ByteCount.SizeofFILE_STANDARD_INFORMATION;
                        Helper.CaptureRequirement(4007, @"[In FileStandardInformation,Pseudocode for the operation is as follows:]
                            Upon successful completion of the operation, the object store MUST return:ByteCount set to sizeof( FILE_STANDARD_INFORMATION ).");
                        Helper.CaptureRequirement(4008, @"[In FileStandardInformation,Pseudocode for the operation is as follows:]
                            Upon successful completion of the operation, the object store MUST return:Status set to STATUS_SUCCESS.");
                        Helper.CaptureRequirement(1421, @"[In Server Requests a Query of File Information ]On completion, 
                            the object store MUST return:[Status,OutputBuffer,ByteCount].");
                        return MessageStatus.SUCCESS;
                    }

                #endregion

                #region 3.1.5.11.28  FileStandardLinkInformation

                case (FileInfoClass.FILE_STANDARD_LINK_INFORMATION):
                    {
                        Condition.IsTrue(outputBufferSize == OutputBufferSize.NotLessThan);
                        // This operation is not supported and MUST be failed with STATUS_NOT_SUPPORTED.
                        Helper.CaptureRequirement(2749, @"[In FileStandardLinkInformation]This operation MUST be failed with STATUS_NOT_SUPPORTED.");
                        //return MessageStatus.NOT_SUPPORTED;
                        //this is a TD issue ,so change the return value to return MessageStatus.INVALID_INFO_CLASS;
                        return MessageStatus.INVALID_INFO_CLASS;
                    }

                #endregion

                #region  3.1.5.11.29    FileStreamInformation
                case (FileInfoClass.FILE_STREAM_INFORMATION):
                    {
                        Condition.IsTrue(outputBufferSize == OutputBufferSize.NotLessThan);
                        Helper.CaptureRequirement(2625, @"[In FileStreamInformation,Pseudocode for the operation is as follows:] 
                            The operation returns STATUS_SUCCESS.");
                        Helper.CaptureRequirement(1421, @"[In Server Requests a Query of File Information ]On completion, 
                            the object store MUST return:[Status,OutputBuffer,ByteCount].");
                        return MessageStatus.SUCCESS;
                    }

                #endregion
            }
            Helper.CaptureRequirement(1421, @"[In Server Requests a Query of File Information ]On completion, the object store MUST return:[Status,OutputBuffer,ByteCount].");
            return MessageStatus.SUCCESS;
        }
        #endregion QueryFileInfoPart1

        #endregion

        #region 3.1.5.14    Application Requests Setting of File Information

        #region 3.1.5.14.1    FileAllocationInformation \\3.1.5.14.8    FileObjectIdInformation\\3.1.5.14.12    FileSfioReserveInformation \\3.1.5.14.10 FileQuotaInformation

        /// <summary>
        /// 3.1.5.14.1    FileAllocationInformation 
        /// 3.1.5.14.8    FileObjectIdInformation
        /// 3.1.5.14.12    FileSfioReserveInformation
        /// </summary>
        /// <param name="fileInfoClass">The type of information being applied, as specified in [MS-FSCC] section 2.4.</param>
        /// <param name="allocationSizeType">Indicates if InputBuffer.AllocationSize is greater than the maximum file size allowed by the object store.</param>
        /// <returns>An NTSTATUS code that specifies the result</returns>
        [Rule]
        public static MessageStatus SetFileAllocOrObjIdInfo(FileInfoClass fileInfoClass, AllocationSizeType allocationSizeType)
        {
            switch (fileInfoClass)
            {
                #region 3.1.5.14.1    FileAllocationInformation
                case FileInfoClass.FILE_ALLOCATION_INFORMATION:
                    {
                        if (gStreamType == StreamType.DirectoryStream)
                        {
                            Helper.CaptureRequirement(2858, @"[In FileAllocationInformation]This operation MUST be failed with STATUS_INVALID_PARAMETER 
                                under any of the following conditions:If Open.Stream.StreamType is DirectoryStream.");
                            return MessageStatus.INVALID_PARAMETER;
                        }
                        //If InputBuffer.AllocationSize is greater than the maximum file size allowed by the object store
                        if (allocationSizeType == AllocationSizeType.AllocationSizeIsGreaterThanMaximum)
                        {
                            Helper.CaptureRequirement(2859, @"[In FileAllocationInformation]This operation MUST be failed with STATUS_INVALID_PARAMETER 
                                under any of the following conditions:If InputBuffer.AllocationSize is greater than the maximum file size 
                                allowed by the object store.<61>");
                            return MessageStatus.INVALID_PARAMETER;
                        }
                        //If Open.GrantedAccess does not contain FILE_WRITE_DATA
                        if ((gOpenGrantedAccess != FileAccess.None) && (gOpenGrantedAccess & FileAccess.FILE_WRITE_DATA) == 0)
                        {
                            Helper.CaptureRequirement(2860, @"[In FileAllocationInformation]Pseudocode for the operation is as follows:
                                If Open.GrantedAccess does not contain FILE_WRITE_DATA, the operation MUST be failed with STATUS_ACCESS_DENIED.");
                            return MessageStatus.ACCESS_DENIED;
                        }
                        Helper.CaptureRequirement(2869, @"[In FileAllocationInformation,Pseudocode for the operation is as follows:]
                            The operation returns STATUS_SUCCESS.");
                        return MessageStatus.SUCCESS;
                    }
                #endregion

                #region 3.1.5.14.8    FileObjectIdInformation
                //This operation is not supported and MUST be failed with STATUS_NOT_SUPPORTED.
                case FileInfoClass.FILE_OBJECTID_INFORMATION:
                    {
                        Helper.CaptureRequirement(3000, @"[In FileObjectIdInformation]This operation MUST be failed with STATUS_NOT_SUPPORTED.");
                        return MessageStatus.NOT_SUPPORTED;
                    }
                #endregion

                #region 3.1.5.14.12    FileSfioReserveInformation
                //This operation is not supported and MUST be failed with STATUS_NOT_SUPPORTED.
                case FileInfoClass.FILE_SFIO_RESERVE_INFORMATION:
                    {
                        Helper.CaptureRequirement(3166, @"[In FileSfioReserveInformation]This operation MUST be failed with STATUS_NOT_SUPPORTED.");
                        return MessageStatus.NOT_SUPPORTED;
                    }
                #endregion

            }
            return MessageStatus.SUCCESS;

        }

        #endregion

        #region 3.1.5.14.2    FileBasicInformation

        /// <summary>
        /// 3.1.5.14.2    FileBasicInformation
        /// </summary>
        /// <param name="inputBufferSize">If InputBufferSize is less than the size of the FILE_BASIC_INFORMATION structure</param>
        /// <param name="inputBufferTime"></param>
        /// <returns>An NTSTATUS code that specifies the result</returns>
        [Rule]
        public static MessageStatus SetFileBasicInfo(InputBufferSize inputBufferSize, InputBufferTime inputBufferTime)
        {
            // If InputBufferSize is less than the size of the FILE_BASIC_INFORMATION structure
            if (inputBufferSize == InputBufferSize.LessThan)
            {
                Helper.CaptureRequirement(2871, @"[In FileBasicInformation]Pseudocode for the operation is as follows:
                    If InputBufferSize is less than sizeof( FILE_BASIC_INFORMATION ), the operation MUST be failed with STATUS_INFO_LENGTH_MISMATCH.");
                return MessageStatus.INFO_LENGTH_MISMATCH;
            }
            if ((inputBufferTime == InputBufferTime.CreationTimeLessthanM1) || (inputBufferTime == InputBufferTime.LastAccessTimeLessthanM1) || (inputBufferTime == InputBufferTime.LastWriteTimeLessthanM1) || (inputBufferTime == InputBufferTime.ChangeTimeLessthanM1))
            {
                Helper.CaptureRequirement(2872, @"[In FileBasicInformation,Pseudocode for the operation is as follows:]
                    The operation MUST be failed with STATUS_INVALID_PARAMETER under any of the following conditions:
                    If InputBuffer.CreationTime is less than -1.");
                Helper.CaptureRequirement(2873, @"[In FileBasicInformation,Pseudocode for the operation is as follows:]
                    The operation MUST be failed with STATUS_INVALID_PARAMETER under any of the following conditions:
                    If InputBuffer.LastAccessTime is less than -1.");
                Helper.CaptureRequirement(2874, @"[In FileBasicInformation,Pseudocode for the operation is as follows:]
                    The operation MUST be failed with STATUS_INVALID_PARAMETER under any of the following conditions:
                    If InputBuffer.LastWriteTime is less than -1.");
                Helper.CaptureRequirement(2875, @"[In FileBasicInformation,Pseudocode for the operation is as follows:]
                    The operation MUST be failed with STATUS_INVALID_PARAMETER under any of the following conditions:
                    If InputBuffer.ChangeTime is less than -1.");
                return MessageStatus.INVALID_PARAMETER;
            }
            //If InputBuffer.FileAttributes.FILE_ATTRIBUTE_DIRECTORY is true and Open.Stream.StreamType is DataStream
            if (((gFileAttribute & FileAttribute.DIRECTORY) != 0) && (gStreamType == StreamType.DataStream))
            {
                Helper.CaptureRequirement(2876, @"[In FileBasicInformation,Pseudocode for the operation is as follows:]
                    The operation MUST be failed with STATUS_INVALID_PARAMETER under any of the following conditions:
                    If InputBuffer.FileAttributes.FILE_ATTRIBUTE_DIRECTORY is TRUE and Open.Stream.StreamType is DataStream.");
                return MessageStatus.INVALID_PARAMETER;
            }
            //If InputBuffer.FileAttributes.FILE_ATTRIBUTE_TEMPORARY is true and Open.File.FileType is DirectoryFile.
            if (((gFileAttribute & FileAttribute.TEMPORARY) != 0) && (gStreamType == StreamType.DirectoryStream))
            {
                Helper.CaptureRequirement(2877, @"[In FileBasicInformation,Pseudocode for the operation is as follows:]
                   The operation MUST be failed with STATUS_INVALID_PARAMETER under any of the following conditions:
                   If InputBuffer.FileAttributes.FILE_ATTRIBUTE_TEMPORARY is TRUE and Open.File.FileType is DirectoryFile.");
                return MessageStatus.INVALID_PARAMETER;
            }
            Helper.CaptureRequirement(2910, @"[In FileBasicInformation,Pseudocode for the operation is as follows:]The operation returns STATUS_SUCCESS.");
            return MessageStatus.SUCCESS;
        }

        #endregion

        #region 3.1.5.14.3    FileDispositionInformation

        /// <summary>
        /// 3.1.5.14.3    FileDispositionInformation
        /// </summary>
        /// <param name="fileDispositionType">Indicates if the file is set to delete pending.</param>
        /// <returns>An NTSTATUS code that specifies the result</returns>
        [Rule]
        public static MessageStatus SetFileDispositionInfo(FileDispositionType fileDispositionType)
        {
            //If Open.GrantedAccess does not contain DELETE
            if ((gOpenGrantedAccess != FileAccess.None) && (gOpenGrantedAccess & FileAccess.DELETE) == 0)
            {
                Helper.CaptureRequirement(4012, @"[In FileDispositionInformation]Pseudocode for the operation is as follows:
                   If Open.GrantedAccess does not contain DELETE, the operation MUST be failed with STATUS_ACCESS_DENIED.");
                return MessageStatus.ACCESS_DENIED;
            }
            //If InputBuffer.DeletePending is true
            if (fileDispositionType == FileDispositionType.IsDeletePending)
            {
                //If File.FileAttributes.FILE_ATTRIBUTE_READONLY is true
                if ((gFileAttribute & FileAttribute.READONLY) != 0)
                {
                    Helper.CaptureRequirement(4013, @"[In FileDispositionInformation,Pseudocode for the operation is as follows:]
                        If InputBuffer.DeletePending is TRUE:If File.FileAttributes.FILE_ATTRIBUTE_READONLY is TRUE, the operation MUST be failed with STATUS_CANNOT_DELETE.");
                    return MessageStatus.CANNOT_DELETE;
                }
            }
            Helper.CaptureRequirement(4021, @"[In FileDispositionInformation,Pseudocode for the operation is as follows:]The operation returns STATUS_SUCCESS.");
            return MessageStatus.SUCCESS;
        }

        #endregion

        #region 3.1.5.14.4    FileEndOfFileInformation

        /// <summary>
        /// 3.1.5.14.4    FileEndOfFileInformation
        /// </summary>
        /// <param name="isEndOfFileGreatThanMaxSize">If InputBuffer.EndOfFile is greater than the maximum file size allowed by the object store</param>
        /// <param name="isSizeEqualEndOfFile">If Open.Stream.Size is equal to InputBuffer.EndOfFile </param>
        /// <returns>An NTSTATUS code that specifies the result</returns>
        [Rule]
        public static MessageStatus SetFileEndOfFileInfo(bool isEndOfFileGreatThanMaxSize, bool isSizeEqualEndOfFile)
        {
            if (gStreamType == StreamType.DirectoryStream)
            {
                Helper.CaptureRequirement(2912, @"[In FileEndOfFileInformation] Pseudocode for the operation is as follows:
                    The operation MUST be failed with STATUS_INVALID_PARAMETER under any of the following conditions:If Open.Stream.StreamType is DirectoryStream.");
                return MessageStatus.INVALID_PARAMETER;
            }

            // If InputBuffer.EndOfFile is greater than the maximum file size allowed by the object store
            if (isEndOfFileGreatThanMaxSize)
            {
                Helper.CaptureRequirement(2913, @"[In FileEndOfFileInformation,Pseudocode for the operation is as follows:] 
                    The operation MUST be failed with STATUS_INVALID_PARAMETER under any of the following conditions:
                    If InputBuffer.EndOfFile is greater than the maximum file size allowed by the object store.<62>");
                return MessageStatus.INVALID_PARAMETER;
            }

            //If Open.GrantedAccess does not contain FILE_WRITE_DATA
            if ((gOpenGrantedAccess != FileAccess.None) && (gOpenGrantedAccess & FileAccess.FILE_WRITE_DATA) == 0)
            {
                Helper.CaptureRequirement(2914, @"[In FileEndOfFileInformation,Pseudocode for the operation is as follows:] 
                    If Open.GrantedAccess does not contain FILE_WRITE_DATA, the operation MUST be failed with STATUS_ACCESS_DENIED.");
                return MessageStatus.ACCESS_DENIED;
            }

            //If Open.Stream.Size is equal to InputBuffer.EndOfFile
            if (isSizeEqualEndOfFile)
            {
                Helper.CaptureRequirement(2917, @"[In FileEndOfFileInformation,Pseudocode for the operation is as follows:] 
                    If Open.Stream.Size is equal to InputBuffer.EndOfFile, the operation MUST immediately return STATUS_SUCCESS.");
                return MessageStatus.SUCCESS;
            }

            Helper.CaptureRequirement(2923, @"[In FileEndOfFileInformation] The operation returns STATUS_SUCCESS.");
            return MessageStatus.SUCCESS;
        }

        #endregion

        #region 3.1.5.14.5    FileFullEaInformation

        /// <summary>
        /// 3.1.5.14.5    FileFullEaInformation
        /// </summary>
        /// <param name="eAValidate">Indicate EainInputBuffer</param>
        /// <returns>An NTSTATUS code that specifies the result</returns>
        [Rule]
        public static MessageStatus SetFileFullEaInfo(EainInputBuffer eAValidate)
        {
            if ((gFileAttribute & FileAttribute.REPARSE_POINT) != 0)
            {
                Helper.CaptureRequirement(2925, @"[In FileFullEaInformation]Pseudocode for the operation is as follows:
                    If Open.File.FileAttributes.FILE_ATTRIBUTE_REPARSE_POINT is TRUE the object store MUST fail the operation with STATUS_EAS_NOT_SUPPORTED.");
                return MessageStatus.EAS_NOT_SUPPORTED;
            }
            //For each Ea in InputBuffer:
            //If Ea.EaName is not well-formed as per [MS-FSCC] 2.4.15
            //If Ea.Flags does not contain a valid set of flags as per [MS-FSCC] 2.4.15
            if (eAValidate == EainInputBuffer.EaNameNotWellForm || eAValidate == EainInputBuffer.EaFlagsInvalid)
            {
                Helper.CaptureRequirement(2926, @"[In FileFullEaInformation,Pseudocode for the operation is as follows:]
                    For each Ea in InputBuffer:If Ea.EaName is not well-formed as per [MS-FSCC] 2.4.15, the operation MUST be failed with STATUS_INVALID_EA_NAME.");
                Helper.CaptureRequirement(2927, @"[In FileFullEaInformation, Pseudocode for the operation is as follows:
                    For each Ea in InputBuffer:] If Ea.Flags does not contain a valid set of flags as per [MS-FSCC] 2.4.15, 
                    the operation MUST be failed with STATUS_INVALID_EA_NAME.");
                return MessageStatus.INVALID_EA_NAME;
            }

            return MessageStatus.SUCCESS;
        }

        #endregion

        #region 3.1.5.14.6    FileLinkInformation

        /// <summary>
        /// 3.1.5.14.6    FileLinkInformation
        /// </summary>
        /// <param name="inputNameInvalid">True: if the inputName is an invalid value</param>
        /// <param name="replaceIfExist">A boolean value. If true and the target stream exists and the operation is successful, the target stream MUST be replaced. If FALSE and the target stream exists, the operation MUST fail.</param>
        /// <returns>An NTSTATUS code that specifies the result</returns>
        [Rule]
        public static MessageStatus SetFileLinkInfo(bool inputNameInvalid, bool replaceIfExist)
        {
            // If Open.File.FileType is DirectoryFile
            if (gfileTypeToOpen == FileType.DirectoryFile)
            {
                Helper.CaptureRequirement(2939, @"[In FileLinkInformation,Pseudocode for the operation is as follows:Pseudocode for the operation is 
                    as follows:]If Open.File.FileType is DirectoryFile, the operation MUST be failed with STATUS_FILE_IS_A_DIRECTORY.");
                return MessageStatus.FILE_IS_A_DIRECTORY;
            }

            // If InputBuffer.FileName is not valid as specified in [MS-FSCC] section 2.1.5s
            if (inputNameInvalid)
            {
                Helper.CaptureRequirement(2941, @"[In FileLinkInformation,Pseudocode for the operation is as follows:]
                    If InputBuffer.FileName is not valid as specified in [MS-FSCC] section 2.1.5, the operation MUST be failed with STATUS_OBJECT_NAME_INVALID.");
                return MessageStatus.OBJECT_NAME_INVALID;
            }

            //This request requires that the caller has FILE_ADD_FILE access on the DestinationDirectory ?if not, the store MUST fail with STATUS_ACCESS_DENIED.
            if ((gOpenGrantedAccess != FileAccess.None) && (gOpenGrantedAccess & FileAccess.FILE_ADD_FILE) == 0)
            {
                Helper.CaptureRequirement(2947, @"[In FileLinkInformation,Pseudocode for the operation is as follows:]
                    This request requires that the caller has FILE_ADD_FILE access on the DestinationDirectory -- if not, the store MUST fail with STATUS_ACCESS_DENIED.");
                return MessageStatus.ACCESS_DENIED;
            }

            //Search DestinationDirectory.File.DirectoryList for an ExistingLink where ExistingLink.Name 
            //or ExistingLink.ShortName matches FileName using case-sensitivity according to Open.IsCaseSensitive
            //If such a link is found:
            if (gIsLinkFound)
            {
                //If InputBuffer.ReplaceIfExists is true:
                if (!replaceIfExist)
                {
                    Helper.CaptureRequirement(2954, @"[In FileLinkInformation,Pseudocode for the operation is as follows: 
                        If such a link is found:]Else[If InputBuffer.ReplaceIfExists is FALSE]The operation MUST be failed with STATUS_OBJECT_NAME_COLLISION.");
                    return MessageStatus.OBJECT_NAME_COLLISION;
                }
            }
            Helper.CaptureRequirement(2974, @"[In FileLinkInformation,Pseudocode for the operation is as follows:]The operation returns STATUS_SUCCESS.");
            return MessageStatus.SUCCESS;
        }

        #endregion

        #region 3.1.5.14.7    FileModeInformation

        /// <summary>
        /// 3.1.5.14.7    FileModeInformation
        /// </summary>
        /// <param name="inputMode">The mode flags for this Open as specified in [MS-FSCC] section 2.4.24. </param>
        /// <returns>An NTSTATUS code that specifies the result</returns>
        [Rule]
        public static MessageStatus SetFileModeInfo(FileMode inputMode)
        {
            //The operation MUST be failed with STATUS_INVALID_PARAMETER under any of the following conditions:
            //InputBuffer.Mode contains any flag, as defined in [MS-FSCC] section 2.4.24, other than the following:
            //FILE_WRITE_THROUGH,FILE_SEQUENTIAL_ONLY , FILE_SYNCHRONOUS_IO_ALERT, FILE_SYNCHRONOUS_IO_NONALERT.
            if (!(inputMode == FileMode.WRITE_THROUGH || inputMode == FileMode.SEQUENTIAL_ONLY || inputMode == FileMode.SYNCHRONOUS_IO_ALERT || inputMode == FileMode.SYNCHRONOUS_IO_NONALERT))
            {
                Helper.CaptureRequirement(2980, @"[In FileModeInformation] Pseudocode for the operation is as follows:
                    The operation MUST be failed with STATUS_INVALID_PARAMETER under any of the following conditions:
                    InputBuffer.Mode contains any flag, as defined in [MS-FSCC] section 2.4.24, 
                    other than the following:FILE_WRITE_THROUGHFILE_SEQUENTIAL_ONLYFILE_SYNCHRONOUS_IO_ALERTFILE_SYNCHRONOUS_IO_NONALERT.");
                return MessageStatus.INVALID_PARAMETER;
            }
            // InputBuffer.Mode contains either FILE_SYNCHRONOUS_IO_ALERT or FILE_SYNCHRONOUS_IO_NONALERT, but Open.Mode contains neither FILE_SYNCHRONOUS_IO_ALERT nor FILE_SYNCHRONOUS_IO_NONALERT
            if (((inputMode == FileMode.SYNCHRONOUS_IO_ALERT) || (inputMode == FileMode.SYNCHRONOUS_IO_NONALERT))
                && ((gOpenMode != CreateOptions.SYNCHRONOUS_IO_ALERT) && (gOpenMode != CreateOptions.SYNCHRONOUS_IO_NONALERT)))
            {
                Helper.CaptureRequirement(2981, @"[In FileModeInformation, Pseudocode for the operation is as follows:] 
                    The operation MUST be failed with STATUS_INVALID_PARAMETER under any of the following conditions: 
                    InputBuffer.Mode contains FILE_SYNCHRONOUS_IO_ALERT , but Open.Mode contains neither FILE_SYNCHRONOUS_IO_ALERT nor FILE_SYNCHRONOUS_IO_NONALERT.");
                Helper.CaptureRequirement(2982, @"[In FileModeInformation, Pseudocode for the operation is as follows:] 
                    The operation MUST be failed with STATUS_INVALID_PARAMETER under any of the following conditions: 
                    InputBuffer.Mode contains FILE_SYNCHRONOUS_IO_NONALERT , but Open.Mode contains neither FILE_SYNCHRONOUS_IO_ALERT 
                    nor FILE_SYNCHRONOUS_IO_NONALERT.");
                return MessageStatus.INVALID_PARAMETER;
            }
            // Open.Mode contains either FILE_SYNCHRONOUS_IO_ALERT or FILE_SYNCHRONOUS_IO_NONALERT, but InputBuffer.Mode contains neither the FILE_SYNCHRONOUS_IO_ALERT nor FILE_SYNCHRONOUS_IO_NONALERT flags.
            if (((inputMode != FileMode.SYNCHRONOUS_IO_ALERT) && (inputMode != FileMode.SYNCHRONOUS_IO_NONALERT))
                 && ((gOpenMode == CreateOptions.SYNCHRONOUS_IO_ALERT) || (gOpenMode == CreateOptions.SYNCHRONOUS_IO_NONALERT)))
            {
                Helper.CaptureRequirement(2983, @"[In FileModeInformation, Pseudocode for the operation is as follows:] 
                    The operation MUST be failed with STATUS_INVALID_PARAMETER under any of the following conditions:
                    Open.Mode contains FILE_SYNCHRONOUS_IO_ALERT , but InputBuffer.Mode contains neither the FILE_SYNCHRONOUS_IO_ALERT 
                    nor FILE_SYNCHRONOUS_IO_NONALERT flags.");
                Helper.CaptureRequirement(2984, @"[In FileModeInformation, Pseudocode for the operation is as follows:] 
                    The operation MUST be failed with STATUS_INVALID_PARAMETER under any of the following conditions: 
                    Open.Mode contains FILE_SYNCHRONOUS_IO_NONALERT, but InputBuffer.Mode contains neither the FILE_SYNCHRONOUS_IO_ALERT 
                    nor FILE_SYNCHRONOUS_IO_NONALERT flags.");
                return MessageStatus.INVALID_PARAMETER;
            }
            //InputBuffer.Mode contains both FILE_SYNCHRONOUS_IO_ALERT and FILE_SYNCHRONOUS_IO_NONALERT.
            if (((inputMode & FileMode.SYNCHRONOUS_IO_NONALERT) != 0) && ((inputMode & FileMode.SYNCHRONOUS_IO_ALERT) != 0))
            {
                Helper.CaptureRequirement(2985, @"[In FileModeInformation, Pseudocode for the operation is as follows:] 
                     The operation MUST be failed with STATUS_INVALID_PARAMETER under any of the following conditions:InputBuffer.Mode contains 
                     both FILE_SYNCHRONOUS_IO_ALERT and FILE_SYNCHRONOUS_IO_NONALERT.");
                return MessageStatus.INVALID_PARAMETER;
            }
            Helper.CaptureRequirement(2998, @"[In FileModeInformation, Pseudocode for the operation is as follows:] The operation returns STATUS_SUCCESS.");
            return MessageStatus.SUCCESS;
        }

        #endregion

        #region 3.1.5.14.9    FilePositionInformation

        /// <summary>
        /// 3.1.5.14.9    FilePositionInformation
        /// </summary>
        /// <param name="inputBufferSize">If InputBufferSize is less than the size, in bytes, of the FILE_POSITION_INFORMATION structure</param>
        /// <param name="currentByteOffset ">NotValid: if InputBuffer.CurrentByteOffset is not an integer multiple of Open.File.Volume.SectorSize,
        /// LessThanZero:InputBuffer.CurrentByteOffset is less than 0</param>
        /// <returns>An NTSTATUS code that specifies the result</returns>
        [Rule]
        public static MessageStatus SetFilePositionInfo(InputBufferSize inputBufferSize, InputBufferCurrentByteOffset currentByteOffset)
        {
            // If InputBufferSize is less than the size, in bytes, of the FILE_POSITION_INFORMATION structure
            if (inputBufferSize == InputBufferSize.LessThan)
            {
                Helper.CaptureRequirement(3002, @"[In FilePositionInformation]Pseudocode for the operation is as follows:
                    If InputBufferSize is less than the size, in bytes, of the FILE_POSITION_INFORMATION structure, 
                    the operation MUST be failed with STATUS_INFO_LENGTH_MISMATCH.");
                return MessageStatus.INFO_LENGTH_MISMATCH;
            }
            //InputBuffer.CurrentByteOffset is less than 0.
            if (currentByteOffset == InputBufferCurrentByteOffset.LessThanZero)
            {
                Helper.CaptureRequirement(3003, @"[In FilePositionInformation,Pseudocode for the operation is as follows:]The operation MUST be failed with STATUS_INVALID_PARAMETER under either of the following conditions:InputBuffer.CurrentByteOffset is less than 0.");
                return MessageStatus.INVALID_PARAMETER;
            }
            //Open.Mode contains FILE_NO_INTERMEDIATE_BUFFERING and InputBuffer.CurrentByteOffset is not an integer multiple of Open.File.Volume.SectorSize.
            if (((gOpenMode & CreateOptions.NO_INTERMEDIATE_BUFFERING) != 0) && (currentByteOffset == InputBufferCurrentByteOffset.NotValid))
            {
                Helper.CaptureRequirement(3004, @"[In FilePositionInformation,Pseudocode for the operation is as follows:]
                    The operation MUST be failed with STATUS_INVALID_PARAMETER under either of the following conditions:
                    Open.Mode contains FILE_NO_INTERMEDIATE_BUFFERING and InputBuffer.CurrentByteOffset is not an integer multiple of Open.File.Volume.SectorSize.");
                return MessageStatus.INVALID_PARAMETER;
            }
            Helper.CaptureRequirement(3006, @"[In FilePositionInformation,Pseudocode for the operation is as follows:]The operation returns STATUS_SUCCESS.<63>");
            return MessageStatus.SUCCESS;
        }

        #endregion

        #region 3.1.5.14.11    FileRenameInformation

        /// <summary>
        /// 3.1.5.14.11 set file rename information
        /// </summary>
        /// <param name="inputBufferFileNameLength">InputBufferFileNameLength</param>
        /// <param name="inputBufferFileName">InputbBufferFileName</param>
        /// <param name="directoryVolumeType">Indicate if DestinationDirectory.Volume is equal to Open.File.Volume.</param>
        /// <param name="destinationDirectoryType">Indicate if DestinationDirectory is the same as Open.Link.ParentFile.</param>
        /// <param name="newLinkNameFormatType">Indicate if NewLinkName is case-sensitive</param>
        /// <param name="newLinkNameMatchType">Indicate if NewLinkName matched TargetLink.ShortName</param>
        /// <param name="replacementType">Indicate if replace target file if exists.</param>
        /// <param name="targetLinkDeleteType">Indicate if TargetLink is deleted or not.</param>
        /// <param name="oplockBreakStatusType">Indicate if there is an oplock to be broken</param>
        /// <param name="targetLinkFileOpenListType">Indicate if TargetLink.File.OpenList contains an Open with a Stream matching the current Stream.</param>
        /// <returns>An NTSTATUS code indicating the result of the operation.</returns>
        /// Disable warning CA1502, because this action confuses with the actual model design if modify it.
        [SuppressMessage("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        [Rule]
        public static MessageStatus SetFileRenameInfo(
            InputBufferFileNameLength inputBufferFileNameLength,
            InputBufferFileName inputBufferFileName,
            DirectoryVolumeType directoryVolumeType,
            DestinationDirectoryType destinationDirectoryType,
            NewLinkNameFormatType newLinkNameFormatType,
            NewLinkNameMatchType newLinkNameMatchType,
            ReplacementType replacementType,
            TargetLinkDeleteType targetLinkDeleteType,
            OplockBreakStatusType oplockBreakStatusType,
            TargetLinkFileOpenListType targetLinkFileOpenListType
            )
        {
            bool RemoveTargetLink = false;
            bool TargetExistsSameFile = false;

            //Boolean values (initialized to true): ActivelyRemoveSourceLink, RemoveSourceLink, AddTargetLink
            bool AddTargetLink = true;
            //If InputBuffer.FileNameLength is equal to zero.
            if (inputBufferFileNameLength == InputBufferFileNameLength.EqualTo_Zero)
            {
                Helper.CaptureRequirement(3023, @"[In FileRenameInformation,Pseudocode for the operation is as follows:]
                    The operation MUST be failed with STATUS_INVALID_PARAMETER under any of the following conditions:
                    If InputBuffer.FileNameLength is equal to zero.");
                return MessageStatus.INVALID_PARAMETER;
            }

            // inputNameLengthValidate == 1:If InputBuffer.FileNameLength is an odd number
            if (inputBufferFileNameLength == InputBufferFileNameLength.OddNumber)
            {
                Helper.CaptureRequirement(3024, @"[In FileRenameInformation,Pseudocode for the operation is as follows:]
                    The operation MUST be failed with STATUS_INVALID_PARAMETER under any of the following conditions:
                    If InputBuffer.FileNameLength is an odd number.");
                return MessageStatus.INVALID_PARAMETER;
            }

            //If InputBuffer.FileNameLength is greater than 
            //InputBufferLength minus the byte offset into the FILE_RENAME_INFORMATION InputBuffer 
            //of the InputBuffer.FileName field (that is, the total length of InputBuffer as given 
            //in InputBufferLength is insufficient to contain the fixed-size fields of InputBuffer 
            //plus the length of InputBuffer.FileName)
            if (inputBufferFileNameLength == InputBufferFileNameLength.Greater)
            {
                Helper.CaptureRequirement(3025, @"[In FileRenameInformation,Pseudocode for the operation is as follows:]
                    The operation MUST be failed with STATUS_INVALID_PARAMETER under any of the following conditions:
                    If InputBuffer.FileNameLength is greater than InputBufferLength minus the byte offset into the FILE_RENAME_INFORMATION InputBuffer 
                    of the InputBuffer.FileName field (that is, the total length of InputBuffer as given in InputBufferLength 
is insufficient to contain the fixed-size fields of InputBuffer plus the length of InputBuffer.FileName).");
                return MessageStatus.MEDIA_WRITE_PROTECTED;
            }

            //If Open.GrantedAccess does not contain DELETE, as defined in [MS-SMB2] section 2.2.13.1
            if ((gOpenGrantedAccess != FileAccess.None) && (gOpenGrantedAccess & FileAccess.DELETE) == 0)
            {
                Helper.CaptureRequirement(3026, @"[In FileRenameInformation,Pseudocode for the operation is as follows:]
                    If Open.GrantedAccess does not contain DELETE, as defined in [MS-SMB2] section 2.2.13.1, 
                    the operation MUST be failed with STATUS_ACCESS_DENIED.");
                return MessageStatus.ACCESS_DENIED;
            }

            // If the first character of InputBuffer.FileName is '\'
            if (inputBufferFileName == InputBufferFileName.StartWithBackSlash)
            {

                //if DestinationDirectory.Volume is not equal to Open.File.Volume
                if (directoryVolumeType == DirectoryVolumeType.DestDirVolumeNotEqualToOpenFileVolume)
                {
                    Helper.CaptureRequirement(3034, @"[In FileRenameInformation,Pseudocode for the operation is as follows:
                        If the first character of InputBuffer.FileName is '\']Else if DestinationDirectory.Volume is not equal 
                        to Open.File.Volume:The operation MUST be failed with STATUS_NOT_SAME_DEVICE.");
                    return MessageStatus.NOT_SAME_DEVICE;
                }
            }

            // If the first character of InputBuffer.FileName is ':'
            if (inputBufferFileName == InputBufferFileName.StartWithColon)
            {
                Helper.CaptureRequirement(3042, @"[In FileRenameInformation,Pseudocode for the operation is as follows: 
                    If the first character of InputBuffer.FileName is ':']The operation immediately returns STATUS_SUCCESS.");
                return MessageStatus.SUCCESS;
            }

            //If Open.File contains open files, the operation MUST be failed with STATUS_ACCESS_DENIED
            //if (isOpenFileContain)
            if (gIsOpenListContains)
            {
                Helper.CaptureRequirement(3044, @"[In FileRenameInformation,Pseudocode for the operation is as follows:]
                    If Open.File contains open files, the operation MUST be failed with STATUS_ACCESS_DENIED.");
                return MessageStatus.ACCESS_DENIED;
            }

            //If InputBuffer.FileName is not valid as specified in [MS-FSCC] section 2.1.5
            if (inputBufferFileName == InputBufferFileName.NotValid)
            {
                Helper.CaptureRequirement(3045, @"[In FileRenameInformation,Pseudocode for the operation is as follows:]
                    If InputBuffer.FileName is not valid as specified in [MS-FSCC] section 2.1.5, 
                    the operation MUST be failed with STATUS_OBJECT_NAME_INVALID.");
                return MessageStatus.OBJECT_NAME_INVALID;
            }

            //If DestinationDirectory is the same as Open.Link.ParentFile:
            if (destinationDirectoryType == DestinationDirectoryType.DestDirIsSameAsOpenLinkParentFile)
            {
                //If NewLinkName is a case-sensitive exact match with Open.Link.Name
                if (newLinkNameFormatType == NewLinkNameFormatType.NewLinkNameIsCaseSensitive)
                {
                    Helper.CaptureRequirement(3046, @"[In FileRenameInformation,Pseudocode for the operation is as follows:] 
                        If DestinationDirectory is the same as Open.Link.ParentFile:If NewLinkName is a case-sensitive exact match with Open.Link.Name, 
                        the operation MUST immediately return STATUS_SUCCESS.");
                    return MessageStatus.SUCCESS;
                }
            }

            //If NewLinkName matches the Name or ShortName of any Link in DestinationDirectory.
            //DirectoryList using case-sensitivity according to Open.IsCaseInsensitive:
            if (newLinkNameMatchType == NewLinkNameMatchType.NewLinkNameMatchTargetLinkShortName)
            {
                //Set FoundLink to true.
                //Set TargetLink to the existing Link found in DestinationDirectory.DirectoryList. Because the name may have been found using a case-insensitive search (if Open.IsCaseInsensitive is true), this preserves the case of the found name.
                //If NewLinkName matched TargetLink.ShortName, set MatchedShortName to true.
                //Set RemoveTargetLink to true.
                RemoveTargetLink = true;
                //If TargetLink.File.FileID equals Open.File.FileID, set TargetExistsSameFile to true. This detects a rename to another existing link to the same file.

                //If (TargetLink.Name is a case-sensitive exact match with NewLinkName) or MatchedShortName is true and TargetLink.ShortName is a case-sensitive exact match with NewLinkName):
                //Set ExactCaseMatch to true.

                //If RemoveTargetLink is true:
                if (RemoveTargetLink)
                {
                    //If TargetExistsSameFile is FALSE and InputBuffer.ReplaceIfExists is FALSE
                    if (!TargetExistsSameFile && replacementType == ReplacementType.NotReplaceIfExists)
                    {
                        Helper.CaptureRequirement(3067, @"[In FileRenameInformation,Pseudocode for the operation is as follows:] 
                            If RemoveTargetLink is TRUE:If TargetExistsSameFile is FALSE and InputBuffer.ReplaceIfExists is FALSE, 
                            the operation MUST be failed with STATUS_OBJECT_NAME_COLLISION.");
                        return MessageStatus.OBJECT_NAME_COLLISION;
                    }

                    //If TargetExistsSameFile is FALSE
                    if (!TargetExistsSameFile)
                    {
                        if (gfileTypeToOpen == FileType.DirectoryFile)
                        {
                            Helper.CaptureRequirement(3069, @"[In FileRenameInformation,Pseudocode for the operation is as follows:
                                If RemoveTargetLink is TRUE:] If TargetExistsSameFile is FALSE:The operation MUST be failed with TATUS_ACCESS_DENIED 
                                under any of the following conditions: If TargetLink.File.FileType is DirectoryFile.");
                            return MessageStatus.ACCESS_DENIED;
                        }

                        if ((gFileAttribute & FileAttribute.READONLY) != 0)
                        {
                            Helper.CaptureRequirement(3071, @"[In FileRenameInformation,Pseudocode for the operation is as follows:
                                If RemoveTargetLink is TRUE: If TargetExistsSameFile is FALSE:The operation MUST be failed with TATUS_ACCESS_DENIED 
                                under any of the following conditions:]If TargetLink.File.FileAttributes.FILE_ATTRIBUTE_READONLY is TRUE.");
                            return MessageStatus.ACCESS_DENIED;
                        }

                        //If TargetLink.IsDeleted is true
                        if (targetLinkDeleteType == TargetLinkDeleteType.TargetLinkIsDeleted)
                        {
                            Helper.CaptureRequirement(3072, @"[In FileRenameInformation,Pseudocode for the operation is as follows:If RemoveTargetLink is TRUE: 
                                If TargetExistsSameFile is FALSE: ]If TargetLink.IsDeleted is TRUE, the operation MUST be failed with STATUS_DELETE_PENDING.");
                            return MessageStatus.DELETE_PENDING;
                        }

                        //If the caller does not have DELETE access to TargetLink.File
                        if ((gOpenGrantedAccess != FileAccess.None) && (gOpenGrantedAccess & FileAccess.DELETE) == 0)
                        {
                            //If the caller does not have FILE_DELETE_CHILD access to DestinationDirectory
                            if ((gOpenGrantedAccess != FileAccess.None) && (gOpenGrantedAccess & FileAccess.FILE_DELETE_CHILD) == 0)
                            {
                                Helper.CaptureRequirement(3073, @"[In FileRenameInformation,Pseudocode for the operation is as follows: 
                                    If RemoveTargetLink is TRUE: If TargetExistsSameFile is FALSE:] If the caller does not have DELETE access to 
                                    TargetLink.File:If the caller does not have FILE_DELETE_CHILD access to DestinationDirectory:
                                    The operation MUST be failed with STATUS_ACCESS_DENIED.");
                                return MessageStatus.ACCESS_DENIED;
                            }
                        }

                        //If there was not an oplock to be broken and TargetLink.File.OpenList contains 
                        //an Open with a Stream matching the current Stream
                        if (oplockBreakStatusType == OplockBreakStatusType.HasNoOplockBreak && 
                            targetLinkFileOpenListType == TargetLinkFileOpenListType.TargetLinkFileOpenListContainMatchedOpen)
                        {
                            Helper.CaptureRequirement(3075, @"[In FileRenameInformation,Pseudocode for the operation is as follows: 
                                If RemoveTargetLink is TRUE: If TargetExistsSameFile is FALSE: For each Stream on TargetLink.File ]
                                If there was not an oplock to be broken and TargetLink.File.OpenList contains an Open with a Stream matching the current Stream, 
                                the operation MUST be failed with STATUS_ACCESS_DENIED.");
                            return MessageStatus.ACCESS_DENIED;
                        }
                    }
                }
            }

            //If AddTargetLink is true:
            if (AddTargetLink)
            {
                //The operation must be failed with STATUS_ACCESS_DENIED
                //if either of the following conditions are true: Open.File.FileType is DirectoryFile 
                //and the caller does not have FILE_ADD_SUBDIRECTORY access on DestinationDirectory.
                if ((gfileTypeToOpen == FileType.DirectoryFile) && ((gOpenGrantedAccess != FileAccess.None) && (gOpenGrantedAccess & FileAccess.FILE_ADD_SUBDIRECTORY) == 0))
                {
                    Helper.CaptureRequirement(3086, @"[In FileRenameInformation,Pseudocode for the operation is as follows:] If AddTargetLink is TRUE:
                        The operation must be failed with STATUS_ACCESS_DENIED if either of the following conditions are true:
                        Open.File.FileType is DirectoryFile and the caller does not have FILE_ADD_SUBDIRECTORY access on DestinationDirectory.");
                    return MessageStatus.ACCESS_DENIED;
                }

                //The operation MUST be failed with STATUS_ACCESS_DENIED if either of the following conditions are true: ]
                //Open.File.FileType is DataFile and the caller does not have FILE_ADD_FILE access on DestinationDirectory.
                if (gfileTypeToOpen == FileType.DataFile && ((gOpenGrantedAccess & FileAccess.FILE_ADD_FILE) == 0))
                {
                    Helper.CaptureRequirement(3087, @"[In FileRenameInformation,Pseudocode for the operation is as follows: If AddTargetLink is TRUE:
                        The operation MUST be failed with STATUS_ACCESS_DENIED if either of the following conditions are true: ]Open.File.FileType is DataFile 
                        and the caller does not have FILE_ADD_FILE access on DestinationDirectory.");
                    return MessageStatus.ACCESS_DENIED;
                }
            }
            Helper.CaptureRequirement(3126, @"[In FileRenameInformation,Pseudocode for the operation is as follows:] The operation returns STATUS_SUCCESS.");
            return MessageStatus.SUCCESS;
        }

        #endregion

        #region 3.1.5.14.11.1 Algorithm for Performing Stream Rename

        /// <summary>
        /// 3.1.5.14.11.1 Algorithm for Performing Stream Rename
        /// </summary>
        /// <param name="newStreamNameFormat">The format of NewStreamName</param>
        /// <param name="streamTypeNameFormat">The format of StreamType</param>
        /// <param name="replacementType">Indicate if replace target file if exists.</param>
        /// <returns>An NTSTATUS code indicating the result of the operation</returns>
        [Rule]
        public static MessageStatus StreamRename(
            InputBufferFileName newStreamNameFormat,
            InputBufferFileName streamTypeNameFormat,
            ReplacementType replacementType)
        {
            if (!IsStreamRenameSupported)
            {
                Requirement.Capture("[TestInfo] If stream rename is not supported by the object store, it SHOULD return STATUS_NOT_SUPPORTED.");
                return MessageStatus.NOT_SUPPORTED;
            }

            if (newStreamNameFormat == InputBufferFileName.EndWithColon)
            {
                Helper.CaptureRequirement(3136, @"[In Algorithm for Performing Stream Rename,Pseudocode for the algorithm is as follows:]
                    The operation MUST be failed with STATUS_INVALID_PARAMETER under any of the following conditions:The last character of NewStreamName is \"":"".");
                return MessageStatus.INVALID_PARAMETER;
            }

            if (newStreamNameFormat == InputBufferFileName.ColonOccurMoreThanThreeTimes)
            {
                Helper.CaptureRequirement(3137, @"[In Algorithm for Performing Stream Rename,Pseudocode for the algorithm is as follows:]
                    The operation MUST be failed with STATUS_INVALID_PARAMETER under any of the following conditions:
                    The character \"":"" occurs more than three times in NewStreamName.");
                return MessageStatus.INVALID_PARAMETER;
            }

            if (newStreamNameFormat == InputBufferFileName.ContainsInvalid)
            {
                Helper.CaptureRequirement(3265, @"[In Algorithm for Performing Stream Rename,Pseudocode for the algorithm is as follows:]The operation MUST be failed with STATUS_INVALID_PARAMETER under any of the following conditions:If StreamName contains any characters invalid for a streamname as specified in [MS-FSCC] section 2.1.5  .");
                return MessageStatus.INVALID_PARAMETER;
            }

            if (newStreamNameFormat == InputBufferFileName.ContainsWildcard)
            {
                Helper.CaptureRequirement(3266, @"[In Algorithm for Performing Stream Rename]The operation MUST be failed with STATUS_INVALID_PARAMETER 
                    under any of the following conditions:If StreamName contains  any wildcard characters as defined in section 3.1.4.3  .");
                return MessageStatus.INVALID_PARAMETER;
            }

            //if (streamNameZeroLength && StreamTypeNameZeroLength)
            if ((newStreamNameFormat == InputBufferFileName.LengthZero) && (streamTypeNameFormat == InputBufferFileName.LengthZero))
            {
                Helper.CaptureRequirement(3140, @"[In Algorithm for Performing Stream Rename,Pseudocode for the algorithm is as follows:]
                    The operation MUST be failed with STATUS_INVALID_PARAMETER under any of the following conditions:Both StreamName and StreamTypeName are zero-length.");
                return MessageStatus.INVALID_PARAMETER;
            }

            //if (streamNameIsMore255Unicode)
            if (newStreamNameFormat == InputBufferFileName.IsMore255Unicode)
            {
                Helper.CaptureRequirement(3141, @"[In Algorithm for Performing Stream Rename,Pseudocode for the algorithm is as follows:]
                    The operation MUST be failed with STATUS_INVALID_PARAMETER under any of the following conditions:StreamName is more than 255 Unicode characters in length.");
                return MessageStatus.INVALID_PARAMETER;
            }

            //if (streamType == StreamType.DataStream && StreamTypeNameisData)
            if ((gStreamType == StreamType.DataStream && streamTypeNameFormat == InputBufferFileName.isData) ||
                (newStreamNameFormat == InputBufferFileName.LengthZero && streamTypeNameFormat == InputBufferFileName.isIndexAllocation))
            {
                Helper.CaptureRequirement(3143, @"[In Algorithm for Performing Stream Rename,Pseudocode for the algorithm is as follows:]
                    The operation MUST be failed with STATUS_OBJECT_TYPE_MISMATCH if either of the following conditions are true:
                    Open.Stream.StreamType is DataStream and StreamTypeName is not the Unicode string \""$DATA"".");
                return MessageStatus.OBJECT_TYPE_MISMATCH;
            }

            //if (streamType == StreamType.DirectoryStream && StreamTypeNameisIndexAllocation)
            if ((gStreamType == StreamType.DirectoryStream) && (streamTypeNameFormat == InputBufferFileName.isIndexAllocation))
            {
                Helper.CaptureRequirement(3144, @"[In Algorithm for Performing Stream Rename,Pseudocode for the algorithm is as follows:]
                    The operation MUST be failed with STATUS_OBJECT_TYPE_MISMATCH if either of the following conditions are true:Open.Stream.StreamType 
                    is DirectoryStream and StreamTypeName is not the Unicode string \""$INDEX_ALLOCATION"".");
                return MessageStatus.OBJECT_TYPE_MISMATCH;
            }

            //If Open.Stream.StreamType is DirectoryStream
            if (gStreamType == StreamType.DirectoryStream)
            {
                Helper.CaptureRequirement(3145, @"[In Algorithm for Performing Stream Rename,Pseudocode for the algorithm is as follows:]
                    If Open.Stream.StreamType is DirectoryStream, the operation MUST be failed with STATUS_INVALID_PARAMETER.");
                return MessageStatus.INVALID_PARAMETER;
            }

            //If StreamName is a case-insensitive match with Open.Stream.Name
            if (newStreamNameFormat == InputBufferFileName.IsCaseInsensitiveMatch && streamTypeNameFormat == InputBufferFileName.isData)
            {
                Helper.CaptureRequirement(3146, @"[In Algorithm for Performing Stream Rename,Pseudocode for the algorithm is as follows:]
                    If StreamName is a case-insensitive match with Open.Stream.Name, the operation MUST immediately return STATUS_SUCCESS.");
                return MessageStatus.SUCCESS;
            }

            //If TargetStream is found
            if (gIsTargetStreamFound)
            {
                if (replacementType != ReplacementType.ReplaceIfExists)
                {
                    Helper.CaptureRequirement(3148, @"[In Algorithm for Performing Stream Rename,Pseudocode for the algorithm is as follows:]
                        If TargetStream is found:If ReplaceIfExists is FALSE, the operation MUST be failed with STATUS_OBJECT_NAME_COLLISION.");
                    return MessageStatus.OBJECT_NAME_COLLISION;
                }
                //If TargetStream.File.OpenList contains any Opens to TargetStream
                if (gIsOpenListContains)
                {
                    Helper.CaptureRequirement(3149, @"[In Algorithm for Performing Stream Rename,Pseudocode for the algorithm is as follows:
                        If TargetStream is found:]If TargetStream.File.OpenList contains any Opens to TargetStream, 
                        the operation MUST be failed with STATUS_INVALID_PARAMETER.");
                    return MessageStatus.INVALID_PARAMETER;
                }
                //if (TargetStreamSizeNotZero)
                if (gIsTargetStreamSizeNotZero)
                {
                    Helper.CaptureRequirement(3150, @"[In Algorithm for Performing Stream Rename,Pseudocode for the algorithm is as follows:
                        If TargetStream is found:]If TargetStream.Size is not 0, the operation MUST be failed with STATUS_INVALID_PARAMETER.");
                    return MessageStatus.INVALID_PARAMETER;
                }
            }
            return MessageStatus.SUCCESS;
        }

        #endregion

        #region 3.1.5.14.13    FileShortNameInformation

        /// <summary>
        /// 3.1.5.14.13    FileShortNameInformation
        /// </summary>
        /// <param name="inputBufferFileName">Indicate inputBufferFileName</param>
        /// <returns>An NTSTATUS code that specifies the result</returns>
        [Rule]
        public static MessageStatus SetFileShortNameInfo(InputBufferFileName inputBufferFileName)
        {
            // If Open.FileName is not "\$Extend\$Quota" and SecurityContext does not have the SE_MANAGE_VOLUME_ACCESS privilege
            if (inputBufferFileName == InputBufferFileName.StartWithBackSlash)
            {
                Helper.CaptureRequirement(3173, @"[In FileShortNameInformation,Pseudocode for the algorithm is as follows:]
                    The operation MUST be failed with STATUS_INVALID_PARAMETER under any of the following conditions:If InputBuffer.FileName starts with '\'.");
                return MessageStatus.INVALID_PARAMETER;
            }

            //inputFileName == "InvalidName": If InputBuffer.FileName is not a valid 8.3 name as described in [MS-FSCC] section 
            if (inputBufferFileName == InputBufferFileName.NotValid)
            {
                Helper.CaptureRequirement(3176, @"[In FileShortNameInformation,Pseudocode for the algorithm is as follows:]
                    The operation MUST be failed with STATUS_INVALID_PARAMETER under any of the following conditions:If InputBuffer.FileName is not a valid 8.3 name as described in [MS-FSCC] section 2.1.5.2.1.");
                return MessageStatus.INVALID_PARAMETER;
            }

            //If Open.GrantedAccess contains neither FILE_WRITE_DATA nor FILE_WRITE_ATTRIBUTES as defined in [MS-SMB2] section 2.2.13.1.
            if ((gOpenGrantedAccess != FileAccess.None) && (gOpenGrantedAccess & (FileAccess.FILE_WRITE_DATA | FileAccess.FILE_WRITE_ATTRIBUTES)) == 0)
            {
                Helper.CaptureRequirement(3178, @"[In FileShortNameInformation,Pseudocode for the algorithm is as follows:]
                    The operation MUST be failed with STATUS_ACCESS_DENIED under any of the following conditions:
                    If Open.GrantedAccess contains neither FILE_WRITE_DATA nor FILE_WRITE_ATTRIBUTES as defined in [MS-SMB2] section 2.2.13.1.");
                return MessageStatus.ACCESS_DENIED;
            }
            if ((gOpenMode & CreateOptions.DELETE_ON_CLOSE) != 0)
            {
                Helper.CaptureRequirement(3180, @"[In FileShortNameInformation,Pseudocode for the algorithm is as follows:]
                    The operation MUST be failed with STATUS_ACCESS_DENIED under any of the following conditions:If Open.Mode.FILE_DELETE_ON_CLOSE is TRUE.");
                return MessageStatus.ACCESS_DENIED;
            }
            // If Open.HasRestorePrivilege is FALSE
            if (!openHasRestoreAcces)
            {
                Helper.CaptureRequirement(3181, @"[In FileShortNameInformation,Pseudocode for the algorithm is as follows:]
                    If Open.HasRestoreAccess is FALSE, the operation MUST be failed with STATUS_PRIVILEGE_NOT_HELD.");
                return MessageStatus.PRIVILEGE_NOT_HELD;
            }
            // If Open.File.Volume.GenerateShortNames is FALSE
            if (!gOpenGenerateShortNames)
            {
                Helper.CaptureRequirement(3182, @"[In FileShortNameInformation,Pseudocode for the algorithm is as follows:]
                    If Open.File.Volume.GenerateShortNames is FALSE, the operation MUST be failed with STATUS_SHORT_NAMES_NOT_ENABLED_ON_VOLUME.");
                return MessageStatus.SHORT_NAMES_NOT_ENABLED_ON_VOLUME;
            }
            // If Open.File contains open files as per section 3.1.4.2
            if (gIsOpenListContains)
            {
                Helper.CaptureRequirement(3183, @"[In FileShortNameInformation,Pseudocode for the algorithm is as follows:]
                    If Open.File contains open files, the operation MUST be failed with STATUS_ACCESS_DENIED.");
                return MessageStatus.ACCESS_DENIED;
            }

            //If InputBuffer.FileName is empty:
            if (inputBufferFileName == InputBufferFileName.Empty)
            {
                Helper.CaptureRequirement(3189, @"[In FileShortNameInformation,Pseudocode for the algorithm is as follows:,
                    If InputBuffer.FileName is empty:]Return STATUS_SUCCESS.");
                Helper.CaptureRequirement(3200, @"[In FileShortNameInformation,Pseudocode for the algorithm is as follows:] Return STATUS_SUCCESS.");
                return MessageStatus.SUCCESS;
            }

            //if (inputFileName == linkShortName)
            if (inputBufferFileName == InputBufferFileName.EqualLinkShortName)
            {
                Helper.CaptureRequirement(3190, @"[In FileShortNameInformation,Pseudocode for the algorithm is as follows:]
                    If InputBuffer.FileName equals Open.Link.ShortName, return STATUS_SUCCESS.");
                Helper.CaptureRequirement(3200, @"[In FileShortNameInformation,Pseudocode for the algorithm is as follows:] Return STATUS_SUCCESS.");
                return MessageStatus.SUCCESS;
            }
            Helper.CaptureRequirement(3200, @"[In FileShortNameInformation,Pseudocode for the algorithm is as follows:] Return STATUS_SUCCESS.");
            return MessageStatus.SUCCESS;
        }

        #endregion

        #region 3.1.5.14.14    FileValidDataLengthInformation

        ///<summary>
        ///3.1.5.14.14    FileValidDataLengthInformation
        ///</summary>
        ///<param name="isStreamValidLengthGreater">True: if Open.Stream.ValidDataLength is greater than InputBuffer.ValidDataLength</param>
        ///<returns>An NTSTATUS code that specifies the result</returns>
        [Rule]
        public static MessageStatus SetFileValidDataLengthInfo(bool isStreamValidLengthGreater)
        {
            //If Open.HasManageVolumeAccess is FALSE, the operation MUST be failed with STATUS_PRIVILEGE_NOT_HELD.
            if (!isOpenHasManageVolumeAccess)
            {
                Helper.CaptureRequirement(3203, @"[In FileValidDataLengthInformation,Pseudocode for the operation is as follows:]
                    If Open.HasManageVolumeAccess is FALSE, the operation MUST be failed with STATUS_PRIVILEGE_NOT_HELD.");
                return MessageStatus.PRIVILEGE_NOT_HELD;
            }
            //If Open.Stream.ValidDataLength is greater than InputBuffer.ValidDataLength.
            //If Open.Stream.IsCompressed is true.
            //If Open.Stream.IsSparse is true.
            if (isStreamValidLengthGreater || gOpenStreamIsCompressed || gOpenStreamIsSparse)
            {
                Helper.CaptureRequirement(3204, @"[In FileValidDataLengthInformation,Pseudocode for the operation is as follows:]
                    The operation MUST be failed with STATUS_INVALID_PARAMETER under any of the following conditions:
                    If Open.Stream.ValidDataLength is greater than InputBuffer.ValidDataLength.");
                Helper.CaptureRequirement(3205, @"[In FileValidDataLengthInformation,Pseudocode for the operation is as follows:]
                     The operation MUST be failed with STATUS_INVALID_PARAMETER under any of the following conditions:If Open.Stream.IsCompressed is TRUE.");
                Helper.CaptureRequirement(3206, @"[In FileValidDataLengthInformation,Pseudocode for the operation is as follows:]
                     The operation MUST be failed with STATUS_INVALID_PARAMETER under any of the following conditions:If Open.Stream.IsSparse is TRUE.");
                return MessageStatus.INVALID_PARAMETER;
            }
            Helper.CaptureRequirement(3210, "[In FileValidDataLengthInformation,Pseudocode for the operation is as follows:]Return STATUS_SUCCESS.");
            return MessageStatus.SUCCESS;
        }

        #endregion

        #endregion
    }
}
