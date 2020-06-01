// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Modeling;
using Microsoft.Protocols.TestSuites.FileSharing.FSA.Adapter;

namespace Microsoft.Protocols.TestSuites.FileSharing.FSA.Model
{
    /// <summary>
    /// MS-FSA model program
    /// </summary>
    public static partial class ModelProgram
    {
        #region 3.1.5.2  Read file in section

        /// <summary>
        /// Application Requests a Read
        /// </summary>
        /// <param name="byteOffset">The absolute byte offset in the stream from which to read data.</param>
        /// <param name="byteCount">The desired number of bytes to read.</param>
        /// <param name="byteRead">The number of bytes that were read.</param>
        /// <returns>An NTSTATUS code that specifies the result</returns>
        [Rule]
        public static MessageStatus ReadFile(
            long byteOffset,
            int byteCount,
            out long byteRead)
        {
            byteRead = 0;
            //If Open.Mode.FILE_NO_INTERMEDIATE_BUFFERING is true & (ByteOffset >= 0)
            if ((gOpenMode & CreateOptions.NO_INTERMEDIATE_BUFFERING) != 0 && byteOffset >= 0)
            {
                //(ByteOffset % Open.File.Volume.SectorSize) is not zero
                if ((byteOffset % gOpenFileVolumeSize) != 0)
                {
                    Helper.CaptureRequirement(668, @"[In Server Requests a Read]If Open.Mode.FILE_NO_INTERMEDIATE_BUFFERING is TRUE 
                        & (ByteOffset >= 0), the operation MUST be failed with STATUS_INVALID_PARAMETER under any of the following conditions:
                       (ByteOffset % Open.File.Volume.SectorSize) is not zero.");
                    return MessageStatus.INVALID_PARAMETER;
                }

                //(ByteCount % Open.File.Volume.SectorSize) is not zero
                if ((byteCount % gOpenFileVolumeSize) != 0)
                {
                    Helper.CaptureRequirement(669, @"[In Server Requests a Read]If Open.Mode.FILE_NO_INTERMEDIATE_BUFFERING is TRUE 
                        & (ByteOffset >= 0), the operation MUST be failed with STATUS_INVALID_PARAMETER under any of the following conditions:
                       (ByteCount % Open.File.Volume.SectorSize) is not zero.");
                    return MessageStatus.INVALID_PARAMETER;
                }
            }

            //If ByteOffset is negative
            if (byteOffset < 0)
            {
                Helper.CaptureRequirement(671, @"[In Server Requests a Read]If ByteOffset is negative, 
                    then the operation MUST be failed with STATUS_INVALID_PARAMETER.");
                return MessageStatus.INVALID_PARAMETER;
            }

            // If (ByteOffset + ByteCount) is larger than MAXLONGLONG (0x7fffffffffffffff) 
            if ((byteOffset + byteCount) > long.MaxValue)
            {
                Helper.CaptureRequirement(672, @"[In Server Requests a Read]If (ByteOffset + ByteCount) is larger than MAXLONGLONG 
                    (0x7fffffffffffffff), the operation MUST be failed with STATUS_INVALID_PARAMETER.");
                return MessageStatus.INVALID_PARAMETER;
            }

            //If ByteCount is zero
            if (byteCount == 0)
            {
                Helper.CaptureRequirement(674, @"[In Server Requests a Read]If ByteCount is zero, the object store MUST return:BytesRead set to zero.");
                byteRead = 0;

                Helper.CaptureRequirement(675, @"[In Server Requests a Read,If ByteCount is zero,]the object store MUST return:Status set to STATUS_SUCCESS.");
                return MessageStatus.SUCCESS;
            }

            // Set RequestedByteCount to ByteCount
            long requestedByteCount = byteCount;

            //If Open.Mode.FILE_NO_INTERMEDIATE_BUFFERING is true
            if ((gOpenMode & CreateOptions.NO_INTERMEDIATE_BUFFERING) != 0)
            {
                //If Open.Mode.FILE_SYNCHRONOUS_IO_ALERT is true or 
                //Open.Mode.FILE_SYNCHRONOUS_IO_NONALERT is true
                if (((gOpenMode & CreateOptions.SYNCHRONOUS_IO_ALERT) != 0) ||
                    (gOpenMode & CreateOptions.SYNCHRONOUS_IO_NONALERT) != 0)
                {
                    gOpenCurrentOffset = byteOffset + requestedByteCount;
                }

                Helper.CaptureRequirement(692, @"[In Server Requests a Read,If IsNonCached is TRUE]Upon successful completion of the operation, 
                    the object store MUST return:BytesRead set to RequestedByteCount.");
                byteRead = requestedByteCount;

                Helper.CaptureRequirement(693, @"[In Server Requests a Read,If IsNonCached is TRUE]Upon successful completion of the operation, 
                    the object store MUST return:Status set to STATUS_SUCCESS.");
                return MessageStatus.SUCCESS;
            }
            else
            {
                //If Open.Mode.FILE_SYNCHRONOUS_IO_ALERT is true or 
                //Open.Mode.FILE_SYNCHRONOUS_IO_NONALERT is true
                if (((gOpenMode & CreateOptions.SYNCHRONOUS_IO_ALERT) != 0) ||
                    (gOpenMode & CreateOptions.SYNCHRONOUS_IO_NONALERT) != 0)
                {
                    gOpenCurrentOffset = byteOffset + byteCount;
                }

                Helper.CaptureRequirement(696, @"[In Server Requests a Read,else If IsNonCached is FASLE]Upon successful completion of the operation, 
                    the object store MUST return:BytesRead set to ByteCount.");
                byteRead = byteCount;

                Helper.CaptureRequirement(697, @"[In Server Requests a Read,If IsNonCached is FASLE]Upon successful completion of the operation, 
                    the object store MUST return:Status set to STATUS_SUCCESS.");
                return MessageStatus.SUCCESS;
            }
        }

        #endregion
    }
}
