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
        /// If Query FileQuotaInformation is implemented
        /// </summary>
        /// Disable warning CA2211, because this action confuses with the actual model design if modify it.
        [SuppressMessage("Microsoft.Usage", "CA2211:NonConstantFieldsShouldNotBeVisible")]
        public static bool isImplementQueryFileQuotaInformation;

        #region Get if Query FileQuotaInformation is implemented

        /// <summary>
        /// The call part of the method GetIfImplementQueryFileQuotaInformation which is used to
        /// get if Query FileQuotaInformation is implemented
        /// This is a call-return pattern
        /// </summary>
        [Rule(Action = "call GetIfImplementQueryFileQuotaInformation(out _)")]
        public static void CallGetIfImplementQueryFileQuotaInformation()
        {
        }

        /// <summary>
        /// The return part of the method GetIfImplementQueryFileQuotaInformation which is used to 
        /// get if Query FileQuotaInformation is implemented
        /// This is a call-returns pattern. 
        /// </summary>
        /// <param name="isImplemented">true: If the object store implements this functionality.</param>
        [Rule(Action = "return GetIfImplementQueryFileQuotaInformation(out isImplemented)")]
        public static void ReturnGetIfImplementQueryFileQuotaInformation(bool isImplemented)
        {
            isImplementQueryFileQuotaInformation = isImplemented;
        }

        #endregion


        #region 3.1.5.5.2    FileQuotaInformation

        /// <summary>
        /// 3.1.5.5.2 FileQuotaInformation
        /// </summary>
        /// <param name="isDirectoryNotRight">The value is true if directory is not right</param>
        /// <param name="isOutBufferSizeLess">The value is true if out buffer size is less</param>
        /// <param name="RestartScan">Indicate restart scan</param>
        /// <param name="state">Indicate SidListState</param>
        /// <returns>An NTSTATUS code that specifies the result</returns>
        [Rule]
        public static MessageStatus QueryFileQuotaInformation(
            SidListState state,
            bool isOutBufferSizeLess,
            bool RestartScan,
            bool isDirectoryNotRight)
        {
            bool EmptyPattern = false;

            //If the object store does not implement this functionality, 
            //the operation MUST be failed with STATUS_INVALID_DEVICE_REQUEST
            if (!isImplementQueryFileQuotaInformation)
            {
                Helper.CaptureRequirement(6241, @"[In FileQuotaInformation]If the object store does not implement this functionality, 
                    the operation MUST be failed with STATUS_INVALID_DEVICE_REQUEST.");
                return MessageStatus.INVALID_DEVICE_REQUEST;
            }

            //If SidList is not empty and SidList.Length (0 is a valid length) is not a multiple of 4, 
            //the operation MUST be failed with STATUS_INVALID_PARAMETER.
            if (state == SidListState.NotEmpty_NotMultipleofFour)
            {
                Helper.CaptureRequirement(6249, @"[In FileQuotaInformation]Pseudocode for the operation is as follows:
                    If SidList is not empty and SidList.Length (0 is a valid length) is not a multiple of 4, the operation MUST be failed with 
                    STATUS_INVALID_PARAMETER.");
                return MessageStatus.INVALID_PARAMETER;
            }

            if (state == SidListState.Empty)
            {
                EmptyPattern = true;
            }
            else
            {
                EmptyPattern = false;
            }

            if (state == SidListState.HasMoreThanOneEntry)
            {
                //If OutputBufferSize is less than sizeof( FILE_QUOTA_INFORMATION ) multiplied
                //by the number of elements in SidList
                if (isOutBufferSizeLess)
                {
                    Helper.CaptureRequirement(6253, @"[In FileQuotaInformation]If SidList has more than one entry:If OutputBufferSize is less than 
                        sizeof( FILE_QUOTA_INFORMATION ) multiplied by the number of elements in SidList, 
                        the operation MUST be failed with STATUS_BUFFER_TOO_SMALL.");
                    return MessageStatus.BUFFER_TOO_SMALL;
                }
            }
            else if (state == SidListState.HasZeroorOneEntry || EmptyPattern)
            {
                if (isOutBufferSizeLess)
                {
                    Helper.CaptureRequirement(6260, @"[In FileQuotaInformation]else[If SidList does not have more than one entry:]
                        If OutputBufferSize is less than sizeof( FILE_QUOTA_INFORMATION ), the operation MUST be failed with STATUS_BUFFER_TOO_SMALL.");
                    return MessageStatus.BUFFER_TOO_SMALL;
                }

                //If RestartScan is FALSE and EmptyPattern is true and there is no match, 
                //the operation MUST be failed with STATUS_NO_MORE_FILES
                if (!RestartScan && EmptyPattern && isDirectoryNotRight)
                {
                    Helper.CaptureRequirement(6266, @"[In FileQuotaInformation,else If SidList does not have more than one entry:]
                        If RestartScan is FALSE and EmptyPattern is TRUE and there is no match, the operation MUST be failed with STATUS_NO_MORE_FILES.");
                    return MessageStatus.NO_MORE_FILES;
                }

                //The operation MUST fail with STATUS_NO_SUCH_FILE under any of 
                //the following conditions:
                //EmptyPattern is FALSE and there is no match
                if (!EmptyPattern && isDirectoryNotRight)
                {
                    Helper.CaptureRequirement(6267, @"[In FileQuotaInformation,else If SidList does not have more than one entry:]
                         The operation MUST fail with STATUS_NO_SUCH_FILE under any of the following conditions:EmptyPattern is FALSE and there is no match.");
                    return MessageStatus.NO_SUCH_FILE;
                }

                //EmptyPattern is true and RestartScan is true and there is no match
                if (EmptyPattern && RestartScan && isDirectoryNotRight)
                {
                    Helper.CaptureRequirement(6268, @"[In FileQuotaInformation,else If SidList does not have more than one entry:]
                    The operation MUST fail with STATUS_NO_SUCH_FILE under any of the following conditions:EmptyPattern is TRUE and 
                    RestartScan is TRUE and there is no match.");
                    return MessageStatus.NO_SUCH_FILE;
                }
            }

            Helper.CaptureRequirement(6272, @"[In FileQuotaInformation]Upon successful completion, the object store MUST return:Status set to STATUS_SUCCESS.");
            return MessageStatus.SUCCESS;
        }

        #endregion
    }
}
