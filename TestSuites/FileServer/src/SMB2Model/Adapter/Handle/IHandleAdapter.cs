// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Modeling;
using Microsoft.Protocols.TestTools;
using Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Smb2;

namespace Microsoft.Protocols.TestSuites.FileSharing.SMB2Model.Adapter.Handle
{
    public delegate void ResponseEventHandler(
            ModelSmb2Status status,
            DurableHandleResponseContext durableHandleResponseContext, 
            LeaseResponseContext leaseResponseContext,
            HandleConfig c);

    public interface IHandleAdapter : IAdapter
    {
        event ResponseEventHandler OpenResponse;

        void ReadConfig(out HandleConfig c);

        void LogOff();

        void Disconnect();

        void PrepareOpen(
            ModelDialectRevision clientMaxDialect,
            PersistentBitType persistentBit,
            CAShareType connectToCAShare,
            ModelHandleType modelHandleType,
            OplockLeaseType oplockLeaseType);

        void ReconnectOpenRequest(
            DurableV1ReconnectContext durableV1ReconnectContext,
            DurableV2ReconnectContext durableV2ReconnectContext,
            OplockLeaseType oplockLeaseType,
            LeaseKeyDifferentialType leaseKeyDifferentialType,
            ClientIdType clientIdType,
            CreateGuidType createGuidType);

        void OpenRequest(
            ModelDialectRevision clientMaxDialect,
            PersistentBitType persistentBit,
            CAShareType connectToCAShare,
            OplockLeaseType oplockLeaseType,
            DurableV1RequestContext durableV1RequestContext,
            DurableV2RequestContext durableV2RequestContext,
            DurableV1ReconnectContext durableV1ReconnectContext,
            DurableV2ReconnectContext durableV2ReconnectContext);
    }
}
