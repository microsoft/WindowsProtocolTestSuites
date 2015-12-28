// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Protocols.TestTools.StackSdk.Asn1;

namespace Microsoft.Protocols.TestTools.StackSdk.Security.KerberosLib
{
    /*
    -- encoded Transited field
    TransitedEncoding       ::= SEQUENCE {
        tr-type         [0] Int32 -- must be registered --,
        contents        [1] OCTET STRING
    }
    */
    public class TransitedEncoding : Asn1Sequence
    {
        [Asn1Field(0), Asn1Tag(Asn1TagType.Context, 0)]
        public KerbInt32 tr_type { get; set; }
        
        [Asn1Field(1), Asn1Tag(Asn1TagType.Context, 1)]
        public Asn1OctetString contents { get; set; }
        
        public TransitedEncoding()
        {
            this.tr_type = null;
            this.contents = null;
        }
        
        public TransitedEncoding(
         KerbInt32 param0,
         Asn1OctetString param1)
        {
            this.tr_type = param0;
            this.contents = param1;
        }
    }
}

