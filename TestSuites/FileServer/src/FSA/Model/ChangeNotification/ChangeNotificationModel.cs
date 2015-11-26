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
        #region 3.1.5.10    Application Requests Change Notifications for a Directory

        /// <summary>
        /// 3.1.5.10    Application Requests Change Notifications for a Directory
        /// </summary>
        /// <param name="changeNotifyEntryType">ChangeNotifyEntryType to indicate if all entries from ChangeNotifyEntry.NotifyEventList
        /// fit in OutputBufferSize bytes</param>
        /// <returns>An NTSTATUS code that specifies the result</returns>
        [Rule]
        public static MessageStatus ChangeNotificationForDir(ChangeNotifyEntryType changeNotifyEntryType)
        {
            #region 3.1.5.10.1    Waiting for Change Notification to be Reported

            if (changeNotifyEntryType == ChangeNotifyEntryType.AllEntriesFitInOutputBufferSize)
            {
                Helper.CaptureRequirement(1399, @"[In Server Requests Change Notifications for a Directory]On completion, 
                    the object store MUST return:[Status,OutputBuffer,ByteCount].");
                return MessageStatus.SUCCESS;
            }
            else
            {
                Helper.CaptureRequirement(1399, @"[In Server Requests Change Notifications for a Directory]On completion, 
                    the object store MUST return:[Status,OutputBuffer,ByteCount].");
                return MessageStatus.NOTIFY_ENUM_DIR;
            }

            #endregion
        }

        #endregion
    }
}