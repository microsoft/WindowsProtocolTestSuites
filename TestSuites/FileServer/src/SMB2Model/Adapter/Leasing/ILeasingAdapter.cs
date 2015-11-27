// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Linq;
using System.Text;
using Microsoft.Modeling;
using System.Collections.Generic;
using Microsoft.Protocols.TestTools;
using Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Smb2;

namespace Microsoft.Protocols.TestSuites.FileSharing.SMB2Model.Adapter.Leasing
{
    public delegate void CreateResponseEventHandler(ModelSmb2Status status, ReturnLeaseContextType returnLeaseContextType, uint leaseState, 
            LeaseFlagsValues leaseFlags, LeasingConfig c);

    public delegate void OnLeaseBreakNotificationEventHandler(ushort newEpoch, LEASE_BREAK_Notification_Packet_Flags_Values flags,
            uint currentLeaseState, uint newLeaseState);

    public delegate void LeaseBreakResponseEventHandler(ModelSmb2Status status, uint leaseState);

    public interface ILeasingAdapter : IAdapter
    {
        event CreateResponseEventHandler CreateResponse;

        event OnLeaseBreakNotificationEventHandler OnLeaseBreakNotification;

        event LeaseBreakResponseEventHandler LeaseBreakResponse;

        void ReadConfig(out LeasingConfig c);

        void SetupConnection(ModelDialectRevision dialect, ClientSupportDirectoryLeasingType clientSupportDirectoryLeasingType);

        void CreateRequest(ConnectTargetType connectTargetType, LeaseContextType leaseContextType,
            LeaseKeyType leaseKey, uint leaseState, LeaseFlagsValues leaseFlags,
            ParentLeaseKeyType parentLeaseKey);

        void LeaseBreakAcknowledgmentRequest(ModelLeaseKeyType modelLeaseKeyType, uint leaseState);

        void FileOperationToBreakLeaseRequest(FileOperation operation, OperatorType operatorType, ModelDialectRevision dialect, out LeasingConfig c);

        void FileOperationToBreakLeaseResponse();
    }
}
