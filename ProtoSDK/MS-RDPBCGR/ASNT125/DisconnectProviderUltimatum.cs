// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using Microsoft.Protocols.TestTools.StackSdk.Asn1;

namespace Microsoft.Protocols.TestTools.StackSdk.RemoteDesktop.Rdpbcgr.Mcs
{
    /*
    DisconnectProviderUltimatum ::= [APPLICATION 8] IMPLICIT SEQUENCE
    {
        reason Reason
    }
    */
    [Asn1Tag(Asn1TagType.Application, 8)]
    public class DisconnectProviderUltimatum : Asn1Sequence
    {
        [Asn1Field(0)]
        public Reason reason { get; set; }
        
        public DisconnectProviderUltimatum()
        {
            this.reason = null;
        }
        
        public DisconnectProviderUltimatum(
         Reason reason)
        {
            this.reason = reason;
        }
    }
}

