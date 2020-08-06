// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Microsoft.Protocols.TestTools.StackSdk.Security.Spng
{
    /// <summary>
    /// Negotiation state of both client role and server role
    /// </summary>
    public enum SpngNegotiationState
    {
        AcceptCompleted,
        AcceptIncomplete,
        Reject,
        RequestMic,
        Initial,
        SspiNegotiation
    }

    /// <summary>
    /// An enum parameter, indicates the inner payload type of NegotiationToken, 
    /// e.g. PayloadType.NegInit is for NegTokenInit
    /// </summary>
    public enum SpngPayloadType
    {
        None,
        NegInit = 1,
        NegResp = 2,
        NegInit2 = 3
    }

    /// <summary>
    /// Authentication mechanisms
    /// </summary>
    public enum SpngAuthMech
    {
        Unknown,
        MsKerberos,
        Kerberos,
        Negoex,
        NLMP,
        Schannel
    }
}
