// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Modeling;
using Microsoft.Protocols.TestTools;
using Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Smb2;

namespace Microsoft.Protocols.TestSuites.FileSharing.SMB2Model.Adapter.Negotiate
{
    public delegate void NegotiateResponseEventHandler(ModelSmb2Status status, DialectRevision dialectRevision);

    public interface INegotiateAdapter : IAdapter
    {
        event NegotiateResponseEventHandler NegotiateResponse;

        void ReadConfig(out NegotiateServerConfig c);

        void SetupConnection();

        void ExpectDisconnect();

        void ComNegotiateRequest(Sequence<string> dialects);

        void NegotiateRequest(Sequence<DialectRevision> dialects);
    }
}
