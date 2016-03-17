// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using Microsoft.Protocols.TestTools.StackSdk.Asn1;

namespace Microsoft.Protocols.TestTools.StackSdk.RemoteDesktop.Rdpbcgr.Mcs
{
    /*
    ErectDomainRequest ::= [APPLICATION 1] IMPLICIT SEQUENCE
    {
        subHeight INTEGER (0..MAX),
        -- height in domain of the MCSPDU transmitter
        subInterval INTEGER (0..MAX)
        -- its throughput enforcement interval in milliseconds
    }
    */
    [Asn1Tag(Asn1TagType.Application, 1)]
    public class ErectDomainRequest : Asn1Sequence
    {
        [Asn1Field(0), Asn1IntegerBound(Min = 0)]
        public Asn1Integer subHeight { get; set; }
        
        [Asn1Field(1), Asn1IntegerBound(Min = 0)]
        public Asn1Integer subInterval { get; set; }
        
        public ErectDomainRequest()
        {
            this.subHeight = null;
            this.subInterval = null;
        }
        
        public ErectDomainRequest(
         Asn1Integer subHeight,
         Asn1Integer subInterval)
        {
            this.subHeight = subHeight;
            this.subInterval = subInterval;
        }
    }
}

