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
        #region 3.1.5.6    Application Requests Flushing Cached Data

        /// <summary>
        /// 3.1.5.6    Application Requests Flushing Cached Data
        /// </summary>
        /// <returns>An NTSTATUS code that specifies the result.</returns>
        [Rule]
        public static MessageStatus FlushCachedData()
        {
            // If Open.File.Volume.IsReadOnly is true, the operation MUST be failed 
            //with STATUS_MEDIA_WRITE_PROTECTED
            if (isFileVolumeReadOnly)
            {
                Helper.CaptureRequirement(881, @"[In Server Requests Flushing Cached Data]On completion, the object store MUST return:Status: An NTSTATUS code that specifies the result.");
                Helper.CaptureRequirement(883, @"[In Server Requests Flushing Cached Data]If Open.File.Volume.IsReadOnly is TRUE, the operation MUST be failed with STATUS_MEDIA_WRITE_PROTECTED.");
                return MessageStatus.MEDIA_WRITE_PROTECTED;
            }

            //Ensure that the directory structure is persisted to stable storage. This behavior is only 
            //implemented by the NTFS file system. Other file systems return STATUS_SUCCESS and perform 
            Helper.CaptureRequirement(881, @"[In Server Requests Flushing Cached Data]On completion, the object store MUST return:Status: An NTSTATUS code that specifies the result.");
            Helper.CaptureRequirement(933, @"[In Server Requests Flushing Cached Data]Other[except  NTFS file system] file systems return STATUS_SUCCESS .");
            Helper.CaptureRequirement(881, @"[In Server Requests Flushing Cached Data]On completion, the object store MUST return:Status: An NTSTATUS code that specifies the result.");
            return MessageStatus.SUCCESS;
        }

        #endregion     
    }
}
