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
        #region 3.1.5.3  Write file in section

        /// <summary>
        ///  Application Requests a Write
        /// </summary>
        /// <param name="byteOffset">The absolute byte offset in the stream where data should be written. </param>
        /// <param name="byteCount">Indicate byteCount</param>
        /// <param name="BytesWritten">Indicate BytesWritten </param>
        /// <returns>An NTSTATUS code that specifies the result.</returns>
        [Rule]
        public static MessageStatus WriteFile(
            long byteOffset,
            long byteCount,
            out long BytesWritten)
        {
            // FSA adapter only support byteoffset > 0 and byteCount > 0
            Condition.IsTrue(byteOffset > 0 && byteOffset < 100);
            Condition.IsTrue(byteCount > 0 && byteCount < 100);
            BytesWritten = 0;

            //If Open.Mode.FILE_NO_INTERMEDIATE_BUFFERING is true and (ByteOffset >= 0)
            if ((gOpenMode & CreateOptions.NO_INTERMEDIATE_BUFFERING) != 0 && (byteOffset >= 0))
            {
                if ((byteOffset % gOpenFileVolumeSize) != 0)
                {
                    Helper.CaptureRequirement(712, @"[In Server Requests a Write]Pseudocode for the operation is as follows: 
                        If Open.Mode.FILE_NO_INTERMEDIATE_BUFFERING is TRUE and (ByteOffset >= 0), the operation MUST be failed with STATUS_INVALID_PARAMETER 
                        under any of the following conditions:If (ByteOffset % Open.File.Volume.SectorSize) is not zero.");
                    return MessageStatus.INVALID_PARAMETER;
                }

                if ((byteCount % gOpenFileVolumeSize) != 0)
                {
                    Helper.CaptureRequirement(713, @"[In Server Requests a Write,Pseudocode for the operation is as follows:]
                        If Open.Mode.FILE_NO_INTERMEDIATE_BUFFERING is TRUE and (ByteOffset >= 0), the operation MUST be failed with 
                        STATUS_INVALID_PARAMETER under any of the following conditions:If (ByteCount % Open.File.Volume.SectorSize) is not zero.");
                    return MessageStatus.INVALID_PARAMETER;
                }

            }

            // If ByteOffset equals -2, then set ByteOffset to Open.CurrentByteOffset
            if (byteOffset == -2)
            {
                byteOffset = gOpenCurrentOffset;
            }

            // If Open.File.Volume.IsReadOnly
            if (isFileVolumeReadOnly)
            {
                Helper.CaptureRequirement(715, @"[In Server Requests a Write,Pseudocode for the operation is as follows:]
                    If Open.File.Volume.IsReadOnly, the operation MUST be failed with STATUS_MEDIA_WRITE_PROTECTED.");
                return MessageStatus.MEDIA_WRITE_PROTECTED;
            }
            if (byteCount == 0)
            {
                Helper.CaptureRequirement(718, @"[In Server Requests a Write,Pseudocode for the operation is as follows:]
                    If ByteCount is zero, the object store MUST return:BytesWritten set to 0.");
                BytesWritten = 0;

                Helper.CaptureRequirement(719, @"[In Server Requests a Write,Pseudocode for the operation is as follows:]
                    If ByteCount is zero, the object store MUST return:Status set to STATUS_SUCCESS.");
                return MessageStatus.SUCCESS;
            }

            // If Open.Mode.FILE_SYNCHRONOUS_IO_ALERT is true or Open.Mode.FILE_SYNCHRONOUS_IO_NONALERT is true, 
            // the object store MUST set Open.CurrentByteOffset to (ByteOffset + ByteCount).
            if ((gOpenMode & CreateOptions.SYNCHRONOUS_IO_ALERT) != 0 ||
                (gOpenMode & CreateOptions.SYNCHRONOUS_IO_NONALERT) != 0)
            {
                gOpenCurrentOffset = byteOffset + byteCount;
            }

            BytesWritten = byteCount;

            Helper.CaptureRequirement(742, @"[In Server Requests a Write,Pseudocode for the operation is as follows:]
                Upon successful completion of the operation, the object store MUST Set: Status to STATUS_SUCCESS.");
            return MessageStatus.SUCCESS;
        }

        #endregion
    }
}
