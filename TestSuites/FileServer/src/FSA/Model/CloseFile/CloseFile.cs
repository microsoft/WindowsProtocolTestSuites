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
        #region 3.1.5.4    Application Requests Closing an Open

        /// <summary>
        /// Application Requests Closing an Open
        /// </summary>
        /// <returns>An NTSTATUS code that specifies the result</returns>
        [Rule]
        public static MessageStatus CloseOpen()
        {
            Helper.CaptureRequirement(5729, @"[In Per File] <10> Section 3.1.1.3: It is updated when the file is closed.");
            Helper.CaptureRequirement(746, @"[In Server Requests Closing an Open]On completion, the object store MUST return:Status : 
                An NTSTATUS code that specifies the result.");
            // Upon successful completion of this operation, the object store MUST return
            Helper.CaptureRequirement(4710, @"[In Server Requests Closing an Open,Pseudocode for this notification is as follows:
                Phase 10 - Update Timestamps:] Upon successful completion of this operation, the object store MUST return:Status set to STATUS_SUCCESS.");
            return MessageStatus.SUCCESS;
        }

        #endregion
    }
}
