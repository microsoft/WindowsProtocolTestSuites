// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Protocols.TestTools;
using Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Smb2;
using System.Collections.Generic;

namespace Microsoft.Protocols.TestSuites.FileSharing.SMB2Model.Adapter.Negotiate
{
    public delegate void NegotiateResponseEventHandler(ModelSmb2Status status, DialectRevision dialectRevision);

    public interface INegotiateAdapter : IAdapter
    {
        event NegotiateResponseEventHandler NegotiateResponse;

        void ReadConfig(out NegotiateServerConfig c);

        void SetupConnection();

        void ExpectDisconnect();

        void ComNegotiateRequest(List<string> dialects);

        void NegotiateRequest(List<DialectRevision> dialects);
    }
}
