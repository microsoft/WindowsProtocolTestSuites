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
        /// If Query FileObjectIdInformation is implemented
        /// </summary>
        /// Disable warning CA2211, because this action confuses with the actual model design if modify it.
        [SuppressMessage("Microsoft.Usage", "CA2211:NonConstantFieldsShouldNotBeVisible")]
        public static bool isImplementQueryFileObjectIdInformation;

        /// <summary>
        /// If Query FileReparsePointInformation is implemented
        /// </summary>
        /// Disable warning CA2211, because this action confuses with the actual model design if modify it.
        [SuppressMessage("Microsoft.Usage", "CA2211:NonConstantFieldsShouldNotBeVisible")]
        public static bool isImplementQueryFileReparsePointInformation;

        #region Get if Query FileObjectIdInformation is implemented

        /// <summary>
        /// The call part of the method GetIfImplementQueryFileObjectIdInformation which is used to
        /// get if Query FileObjectIdInformation is implemented
        /// This is a call-return pattern
        /// </summary>
        [Rule(Action = "call GetIfImplementQueryFileObjectIdInformation(out _)")]
        public static void CallGetIfImplementQueryFileObjectIdInformation()
        {
        }

        /// <summary>
        /// The return part of the method GetIfImplementQueryFileObjectIdInformation which is used to 
        /// get if Query FileObjectIdInformation is implemented
        /// This is a call-returns pattern. 
        /// </summary>
        /// <param name="isImplemented">true: If the object store implements this functionality.</param>
        [Rule(Action = "return GetIfImplementQueryFileObjectIdInformation(out isImplemented)")]
        public static void ReturnGetIfImplementQueryFileObjectIdInformation(bool isImplemented)
        {
            isImplementQueryFileObjectIdInformation = isImplemented;
        }

        #endregion

        #region Get if Query FileReparsePointInformation is implemented

        /// <summary>
        /// The call part of the method GetIfImplementQueryFileReparsePointInformation which is used to
        /// get if Query FileReparsePointInformation is implemented
        /// This is a call-return pattern
        /// </summary>
        [Rule(Action = "call GetIfImplementQueryFileReparsePointInformation(out _)")]
        public static void CallGetIfImplementQueryFileReparsePointInformation()
        {
        }

        /// <summary>
        /// The return part of the method GetIfImplementQueryFileReparsePointInformation which is used to 
        /// get if Query FileReparsePointInformation is implemented
        /// This is a call-returns pattern. 
        /// </summary>
        /// <param name="isImplemented">true: If the object store implements this functionality.</param>
        [Rule(Action = "return GetIfImplementQueryFileReparsePointInformation(out isImplemented)")]
        public static void ReturnGetIfImplementQueryFileReparsePointInformation(bool isImplemented)
        {
            isImplementQueryFileReparsePointInformation = isImplemented;
        }

        #endregion

        #region 3.1.5.5    Application Requests Querying a Directory

        #region 3.1.5.5.1    FileObjectIdInformation

        /// <summary>
        /// 3.1.5.5.1    FileObjectIdInformation
        /// </summary>
        /// <param name="fileNamePattern"> A Unicode string containing the file name pattern to match. </param>
        /// <param name="queryDirectoryScanType">Indicate whether the enumeration should be restarted.</param>
        /// <param name="queryDirectoryFileNameMatchType">The object store MUST search the volume for Files having File.ObjectId matching FileNamePattern.
        /// This parameter indicates if matches the file by FileNamePattern.</param>
        /// <param name="queryDirectoryOutputBufferType">Indicate if OutputBuffer is large enough to hold the first matching entry.</param>
        /// <returns>An NTSTATUS code that specifies the result</returns>
        [Rule]
        public static MessageStatus QueryFileObjectIdInfo(
            FileNamePattern fileNamePattern,
            QueryDirectoryScanType queryDirectoryScanType,
            QueryDirectoryFileNameMatchType queryDirectoryFileNameMatchType,
            QueryDirectoryOutputBufferType queryDirectoryOutputBufferType)
        {
            Condition.IsTrue(fileNamePattern == FileNamePattern.NotEmpty_LengthIsNotAMultipleOf4 ||
                fileNamePattern == FileNamePattern.Empty ||
                fileNamePattern == FileNamePattern.NotEmpty);

            bool emptyPattern = false;

            if (!isImplementQueryFileObjectIdInformation)
            {
                Helper.CaptureRequirement(4817, @"[In FileObjectIdInformation ] If the object store does not implement this functionality, 
                    the operation MUST be failed with STATUS_INVALID_DEVICE_REQUEST.");
                return MessageStatus.INVALID_DEVICE_REQUEST;
            }

            //If FileNamePattern is not empty and FileNamePattern.Length (0 is a valid length) 
            //is not a multiple of 4
            if (fileNamePattern == FileNamePattern.NotEmpty_LengthIsNotAMultipleOf4)
            {
                Helper.CaptureRequirement(819, @"[In FileObjectIdInformation,Pseudocode for the operation is as follows:] 
                    If FileNamePattern is not empty and FileNamePattern.Length (0 is a valid length) is not a multiple of 4, 
                    the operation MUST be failed with STATUS_INVALID_PARAMETER.");
                return MessageStatus.INVALID_PARAMETER;
            }

            // If FileNamePattern is empty, then the object store MUST set EmptyPattern to 
            // True; otherwise it MUST set EmptyPattern to FALSE.
            if (fileNamePattern == FileNamePattern.Empty)
            {
                emptyPattern = true;
            }
            else
            {
                emptyPattern = false;
            }

            // If RestartScan is FALSE and EmptyPattern is true and there is no match, 
            //the operation MUST be failed with STATUS_NO_MORE_FILES.
            if ((queryDirectoryScanType == QueryDirectoryScanType.NotRestartScan) && 
                (emptyPattern) &&
                (queryDirectoryFileNameMatchType == QueryDirectoryFileNameMatchType.FileNamePatternNotMatched) &&
                queryDirectoryOutputBufferType != QueryDirectoryOutputBufferType.OutputBufferIsNotEnough)
            {
                Helper.CaptureRequirement(826, @"[In FileObjectIdInformation,Pseudocode for the operation is as follows:]
                    If RestartScan is FALSE and EmptyPattern is TRUE and there is no match[Any comparison where the ObjectId chunk is greater than 
                    or equal to the FileNamePattern.Buffer chunk], the operation MUST be failed with STATUS_NO_MORE_FILES.");
                return MessageStatus.NO_MORE_FILES;
            }

            // EmptyPattern is FALSE and there is no match.
            if ((!emptyPattern) && (queryDirectoryFileNameMatchType == QueryDirectoryFileNameMatchType.FileNamePatternNotMatched) &&
                queryDirectoryOutputBufferType != QueryDirectoryOutputBufferType.OutputBufferIsNotEnough)
            {
                Helper.CaptureRequirement(828, @"[In FileObjectIdInformation,Pseudocode for the operation is as follows:]The operation MUST fail with 
                    STATUS_NO_SUCH_FILE under any of the following conditions:EmptyPattern is FALSE and there is no match
                    [the volume for Files having File.ObjectId matching FileNamePattern].");
                return MessageStatus.NO_SUCH_FILE;
            }

            // EmptyPattern is true and RestartScan is true and there is no match.
            if ((emptyPattern) && 
                (queryDirectoryScanType == QueryDirectoryScanType.RestartScan) &&
                (queryDirectoryFileNameMatchType == QueryDirectoryFileNameMatchType.FileNamePatternNotMatched) &&
                queryDirectoryOutputBufferType != QueryDirectoryOutputBufferType.OutputBufferIsNotEnough)
            {
                Helper.CaptureRequirement(829, @"[In FileObjectIdInformation,Pseudocode for the operation is as follows:]The operation MUST fail with 
                    STATUS_NO_SUCH_FILE under any of the following conditions:EmptyPattern is TRUE and RestartScan is TRUE 
                    and there is no match[the volume for Files having File.ObjectId matching FileNamePattern].");
                return MessageStatus.NO_SUCH_FILE;
            }

            //The operation MUST fail with STATUS_BUFFER_OVERFLOW if OutputBuffer is not large enough to hold the first matching entry.
            if (queryDirectoryOutputBufferType == QueryDirectoryOutputBufferType.OutputBufferIsNotEnough)
            {
                Helper.CaptureRequirement(830, @"[In FileObjectIdInformation,Pseudocode for the operation is as follows:]The operation MUST fail with 
                    STATUS_BUFFER_OVERFLOW if OutputBuffer is not large enough to hold the first matching entry.");
                return MessageStatus.INFO_LENGTH_MISMATCH;
            }
            
            Helper.CaptureRequirement(813, @"[In Server Requests Querying a Directory]On completion, the object store MUST return:
                [Status,OutputBuffer,BytesReturned ].");
            Helper.CaptureRequirement(833, @"[In FileObjectIdInformation, Pseudocode for the operation is as follows:
                If there is at least one match]The object store MUST return:Status set to STATUS_SUCCESS.");
            return MessageStatus.SUCCESS;
        }

        #endregion

        #region 3.1.5.5.3    FileReparsePointInformation

        /// <summary>
        /// 3.1.5.5.3 FileReparsePointInformation
        /// </summary>
        /// <param name="fileNamePattern"> A Unicode string containing the file name pattern to match. </param>
        /// <param name="queryDirectoryScanType">Indicate whether the enumeration should be restarted.</param>
        /// <param name="queryDirectoryFileNameMatchType">The object store MUST search the volume for Files having File.ObjectId matching FileNamePattern.
        /// This parameter indicates if matches the file by FileNamePattern.</param>
        /// <param name="queryDirectoryOutputBufferType">Indicate if OutputBuffer is large enough to hold the first matching entry.</param>
        /// <returns>An NTSTATUS code that specifies the result</returns>
        [Rule]
        public static MessageStatus QueryFileReparsePointInformation(
            FileNamePattern fileNamePattern,
            QueryDirectoryScanType queryDirectoryScanType,
            QueryDirectoryFileNameMatchType queryDirectoryFileNameMatchType,
            QueryDirectoryOutputBufferType queryDirectoryOutputBufferType)
        {
            bool EmptyPattern = false;

            //If the object store does not implement this functionality, 
            //the operation MUST be failed with STATUS_INVALID_DEVICE_REQUEST
            if (!isImplementQueryFileReparsePointInformation)
            {
                Helper.CaptureRequirement(6276, @"[In FileReparsePointInformation]If the object store does not implement this functionality, 
                    the operation MUST be failed with STATUS_INVALID_DEVICE_REQUEST.");
                return MessageStatus.INVALID_DEVICE_REQUEST;
            }

            if (fileNamePattern == FileNamePattern.NotEmpty_LengthIsNotAMultipleOf4)
            {
                Helper.CaptureRequirement(6280, @"[In FileReparsePointInformation]Pseudocode for the operation is as follows:
                    If FileNamePattern is not empty and FileNamePattern.Length (0 is a valid length) is not a multiple of 4, the operation 
                    MUST be failed with STATUS_INVALID_PARAMETER.");
                return MessageStatus.INVALID_PARAMETER;
            }

            if (fileNamePattern == FileNamePattern.Empty)
            {
                EmptyPattern = true;
            }
            else
            {
                EmptyPattern = false;
            }

            if ((queryDirectoryScanType == QueryDirectoryScanType.NotRestartScan) && 
                EmptyPattern &&
                (queryDirectoryFileNameMatchType == QueryDirectoryFileNameMatchType.FileNamePatternNotMatched))
            {
                Helper.CaptureRequirement(6286, @"[In FileReparsePointInformation]If RestartScan is FALSE and EmptyPattern is TRUE 
                    and there is no match, the operation MUST be failed with STATUS_NO_MORE_FILES.");
                return MessageStatus.NO_MORE_FILES;
            }

            if (!EmptyPattern 
                && (queryDirectoryFileNameMatchType == QueryDirectoryFileNameMatchType.FileNamePatternNotMatched))
            {
                Helper.CaptureRequirement(6287, @"[In FileReparsePointInformation]The operation MUST fail with STATUS_NO_SUCH_FILE under 
                    any of the following conditions:EmptyPattern is FALSE and there is no match.");
                return MessageStatus.NO_SUCH_FILE;
            }

            if (EmptyPattern && (queryDirectoryScanType == QueryDirectoryScanType.RestartScan) && 
                (queryDirectoryFileNameMatchType == QueryDirectoryFileNameMatchType.FileNamePatternNotMatched))
            {
                Helper.CaptureRequirement(6288, @"[In FileReparsePointInformation]The operation MUST fail with STATUS_NO_SUCH_FILE 
                    under any of the following conditions:EmptyPattern is TRUE and RestartScan is TRUE and there is no match.");
                return MessageStatus.NO_SUCH_FILE;
            }

            //If OutputBuffer is not large enough to hold the first matching entry
            if (queryDirectoryOutputBufferType == QueryDirectoryOutputBufferType.OutputBufferIsNotEnough)
            {
                Helper.CaptureRequirement(6289, @"[In FileReparsePointInformation]The operation MUST fail with STATUS_BUFFER_OVERFLOW 
                    if OutputBuffer is not large enough to hold the first matching entry.");
                return MessageStatus.INFO_LENGTH_MISMATCH;
            }

            //If there is at least one match, the operation is considered successful
            if (queryDirectoryFileNameMatchType == QueryDirectoryFileNameMatchType.FileNamePatternMatched)
            {
                Helper.CaptureRequirement(6290, @"[In FileReparsePointInformation]If there is at least one match, the operation is considered successful. 
                    The object store MUST return:[Status, OutputBuffer,ByteCount].");
                Helper.CaptureRequirement(6291, @"[In FileReparsePointInformation]Status set to STATUS_SUCCESS.");

                return MessageStatus.SUCCESS;
            }

            Helper.CaptureRequirement(6290, @"[In FileReparsePointInformation]If there is at least one match, the operation is considered successful. 
                The object store MUST return:[Status, OutputBuffer,ByteCount].");
            Helper.CaptureRequirement(6291, @"[In FileReparsePointInformation]Status set to STATUS_SUCCESS.");
            return MessageStatus.SUCCESS;
        }

        #endregion

        #region 3.1.5.5.4    Directory Information Queries

        static bool firstQuery = true;

        /// <summary>
        /// 3.1.5.5.4    Directory Information Queries
        /// </summary>
        /// <param name="fileNamePattern">FileNamePattern</param>
        /// <param name="restartScan">A boolean indicating whether the enumeration should be restarted.</param>
        /// <param name="isNoRecordsReturned">true: If no records are being returned</param>
        /// <param name="isOutBufferSizeLess">true if OutputBufferSize is less than the size needed to return a single entry</param>
        /// <param name="outBufferSize">the state of OutBufferSize in subsection 
        /// of section 3.1.5.5.4</param>
        /// <returns>An NTSTATUS code that specifies the result</returns>
        /// Disable warning CA1801, because the parameter of "capabilities" is used for extend the model logic, 
        /// which will affect the implementation of the model if it is removed.
        [SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters")]
        [Rule]
        public static MessageStatus QueryDirectoryInfo(
            FileNamePattern fileNamePattern,
            bool restartScan,
            bool isNoRecordsReturned,
            bool isOutBufferSizeLess,
            OutBufferSmall outBufferSize)
        {
            MessageStatus returnStatus = MessageStatus.SUCCESS;
            //If OutputBufferSize is less than the size needed to return a single entry
            if (isOutBufferSizeLess)
            {
                switch (outBufferSize)
                {
                    case OutBufferSmall.FileBothDirectoryInformation:
                        {
                            Helper.CaptureRequirement(4881, @"[In FileBothDirectoryInformation] Pseudocode for the operation is as follows:
                                If OutputBufferSize is smaller than FieldOffset( FILE_BOTH_DIR_INFORMATION.FileName ), 
                                the operation MUST be failed with STATUS_INFO_LENGTH_MISMATCH.");
                            return MessageStatus.INFO_LENGTH_MISMATCH;
                        }
                    case OutBufferSmall.FileDirectoryInformation:
                        {
                            Helper.CaptureRequirement(4904, @"[In FileDirectoryInformation] Pseudocode for the operation is as follows:
                                If OutputBufferSize is smaller than FieldOffset( FILE_DIRECTORY_INFORMATION.FileName ), 
                                the operation MUST be failed with STATUS_INFO_LENGTH_MISMATCH.");
                            return MessageStatus.INFO_LENGTH_MISMATCH;
                        }
                    case OutBufferSmall.FileFullDirectoryInformation:
                        {
                            Helper.CaptureRequirement(4920, @"[In FileFullDirectoryInformation] Pseudocode for the operation is as follows:
                                If OutputBufferSize is smaller than FieldOffset( FILE_FULL_DIR_INFORMATION.FileName ), 
                                the operation MUST be failed with STATUS_INFO_LENGTH_MISMATCH.");
                            return MessageStatus.INFO_LENGTH_MISMATCH;
                        }
                    case OutBufferSmall.FileIdBothDirectoryInformation:
                        {
                            Helper.CaptureRequirement(4939, @"[In FileIdBothDirectoryInformation] Pseudocode for the operation is as follows:
                                If OutputBufferSize is smaller than FieldOffset( FILE_ID_BOTH_DIR_INFORMATION.FileName ), 
                                the operation MUST be failed with STATUS_INFO_LENGTH_MISMATCH.");
                            return MessageStatus.INFO_LENGTH_MISMATCH;
                        }
                    case OutBufferSmall.FileIdFullDirectoryInformation:
                        {
                            return MessageStatus.INFO_LENGTH_MISMATCH;
                        }
                    case OutBufferSmall.FileNamesInformation:
                        {
                            Helper.CaptureRequirement(4983, @"[In FileNamesInformation] Pseudocode for the operation is as follows:
                                If OutputBufferSize is smaller than FieldOffset( FILE_NAMES_INFORMATION.FileName ), 
                                the operation MUST be failed with STATUS_INFO_LENGTH_MISMATCH.");
                            return MessageStatus.INFO_LENGTH_MISMATCH;
                        }
                    default:
                        break;
                }

                Helper.CaptureRequirement(4836, @"[In Directory Information Queries] Pseudocode for the algorithm is as follows:
                    If OutputBufferSize is less than the size needed to return a single entry, 
                    the operation MUST be failed with STATUS_INFO_LENGTH_MISMATCH.");
                return MessageStatus.INFO_LENGTH_MISMATCH;
            }

            if (fileNamePattern == FileNamePattern.Empty)
            {
                fileNamePattern = FileNamePattern.IndicateAll;
            }
            else
            {
                //If FileNamePattern is not a valid filename component as described in [MS-FSCC] section 2.1.5, with the exceptions that wildcard characters described in section 3.1.4.3 are permitted and the strings "." and ".." are permitted
                if (fileNamePattern == FileNamePattern.NotValidFilenameComponent)
                {
                    Helper.CaptureRequirement(849, @"[In Directory Information Queries ,Pseudocode for the algorithm is as follows: 
                        If Open.QueryPattern is empty] else [if FileNamePattern is not empty]If FileNamePattern is not a valid filename component 
                        as described in [MS-FSCC] section 2.1.5, with the exceptions that wildcard characters described in section 3.1.4.3 
                        are permitted and the strings \"".\"" and \""..\"" are permitted, the operation MUST be failed with STATUS_OBJECT_NAME_INVALID.");
                    return MessageStatus.OBJECT_NAME_INVALID;
                }
            }

            //If no records are being returned
            if (isNoRecordsReturned)
            {
                //If FirstQuery is true
                if (firstQuery)
                {
                    firstQuery = false;
                    Helper.CaptureRequirement(4875, @"[In Directory Information Queries,Pseudocode for the algorithm is as follows:]
                        If no records are being returned:If FirstQuery is TRUE:Set StatusToReturn to STATUS_NO_SUCH_FILE, which means no files were found 
                        in this directory that match the given wildcard pattern.");
                    returnStatus = MessageStatus.NO_SUCH_FILE;
                }
                else
                {
                    Helper.CaptureRequirement(4876, @"[In Directory Information Queries,Pseudocode for the algorithm is as follows:
                        If no records are being returned:] Else[If FirstQuery is FALSE]:Set StatusToReturn to STATUS_NO_MORE_FILES, 
                        which means no more files were found in this directory that match the given wildcard pattern.");
                    returnStatus = MessageStatus.NO_MORE_FILES;
                }
            }

            Helper.CaptureRequirement(865, @"[In Directory Information Queries,Pseudocode for the algorithm is as follows:Add a context,
                if the operation succeeds ]The object store MUST return:Status set to StatusToReturn.");
            return returnStatus;
        }

        #endregion

        #endregion
    }
}
