// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using Microsoft.Protocols.TestTools.StackSdk.Asn1;

namespace Microsoft.Protocols.TestTools.StackSdk.RemoteDesktop.Rdpbcgr.Mcs
{
    /*
    Reason ::= ENUMERATED -- in DisconnectProviderUltimatum, DetachUserRequest, DetachUserIndication
    {
        rn-domain-disconnected (0),
        rn-provider-initiated (1),
        rn-token-purged (2),
        rn-user-requested (3),
        rn-channel-purged (4)
    }
    */
    public class Reason : Asn1Enumerated
    {
        [Asn1EnumeratedElement]
        public const long rn_domain_disconnected = 0;
        
        [Asn1EnumeratedElement]
        public const long rn_provider_initiated = 1;
        
        [Asn1EnumeratedElement]
        public const long rn_token_purged = 2;
        
        [Asn1EnumeratedElement]
        public const long rn_user_requested = 3;
        
        [Asn1EnumeratedElement]
        public const long rn_channel_purged = 4;
        
        public Reason()
            : base()
        {
        }
        
        public Reason(long val)
            : base(val)
        {
        }
    }
}

