// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Text;
using Microsoft.Modeling;
using Microsoft.Protocols.TestTools;
using Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Smb2;

namespace Microsoft.Protocols.TestSuites.FileSharing.SMB2Model.Adapter.MixedOplockLease
{
    public delegate void VerificationEventHandler(ModelBreakType breakType, OplockLevel_Values grantedOplockType, ModelLeaseStateType grantedLeaseType);

    public interface IMixedOplockLeaseAdapter: IAdapter
    {
        void Preparation();
        void RequestOplock(OplockLevel_Values oplockType);
        void RequestLease(ModelLeaseStateType leaseType);

        event VerificationEventHandler Verification;
    }
}
