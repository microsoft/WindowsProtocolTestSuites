// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Protocols.TestTools.StackSdk.Asn1;

namespace Microsoft.Protocols.TestTools.StackSdk.Security.KerberosLib
{
    /*
    KDC-PROXY-MESSAGE::= SEQUENCE {
    kerb-message [0] OCTET STRING,
    target-domain [1] KerberosString OPTIONAL,
    dclocator-hint [2] INTEGER OPTIONAL
    }
    */
    public class KDC_PROXY_MESSAGE : Asn1Sequence
    {
        [Asn1Field(0), Asn1Tag(Asn1TagType.Context, 0)]
        public Asn1OctetString kerb_message { get; set; }
        
        [Asn1Field(1, Optional = true), Asn1Tag(Asn1TagType.Context, 1)]
        public KerberosString target_domain { get; set; }
        
        [Asn1Field(2, Optional = true), Asn1Tag(Asn1TagType.Context, 2)]
        public Asn1Integer dclocator_hint { get; set; }
        
        public KDC_PROXY_MESSAGE()
        {
            this.kerb_message = null;
            this.target_domain = null;
            this.dclocator_hint = null;
        }
        
        public KDC_PROXY_MESSAGE(
         Asn1OctetString param0,
         KerberosString param1,
         Asn1Integer param2)
        {
            this.kerb_message = param0;
            this.target_domain = param1;
            this.dclocator_hint = param2;
        }
    }
}

