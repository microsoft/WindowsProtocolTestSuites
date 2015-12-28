// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Protocols.TestTools;
using Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Smb2;

namespace Microsoft.Protocols.TestSuites.FileSharing.SMB2Model.Adapter.Oplock
{
    public delegate void OplockBreakNotificationEventHandler(OPLOCK_BREAK_Notification_Packet_OplockLevel_Values acceptableAckOplockLevel);

    public delegate void OplockBreakResponseEventHandler(ModelSmb2Status status, OPLOCK_BREAK_Response_OplockLevel_Values oplockLevel, OplockLevel_Values oplockLevelOnOpen);

    public delegate void RequestOplockAndOperateFileResponseEventHandler();

    public interface IOplockAdapter : IAdapter
    {
        event OplockBreakNotificationEventHandler OplockBreakNotification;

        event OplockBreakResponseEventHandler OplockBreakResponse;

        void ReadConfig(out OplockConfig c);

        void SetupConnection(ModelDialectRevision maxSmbVersionClientSupported, ModelShareFlag shareFlag, ModelShareType shareType);

        void RequestOplockAndOperateFileRequest(
            RequestedOplockLevel_Values requestedOplockLevel,
            OplockFileOperation fileOperation,
            out OplockLevel_Values grantedOplockLevel,
            out OplockConfig c);

        void OplockBreakAcknowledgementRequest(
            OplockVolatilePortion volatilePortion,
            OplockPersistentPortion persistentPortion,
            OplockLevel_Values oplockLevel);

    }
}
