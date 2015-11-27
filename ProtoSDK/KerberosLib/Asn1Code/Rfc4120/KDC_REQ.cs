// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Protocols.TestTools.StackSdk.Asn1;

namespace Microsoft.Protocols.TestTools.StackSdk.Security.KerberosLib
{
    /*
    KDC-REQ         ::= SEQUENCE {
        -- NOTE: first tag is [1], not [0]
        pvno            [1] INTEGER (5) ,
        msg-type        [2] INTEGER (10 -- AS -- | 12 -- TGS --),
        padata          [3] SEQUENCE OF PA-DATA OPTIONAL
                            -- NOTE: not empty --,
        req-body        [4] KDC-REQ-BODY
    }
    */
    public class KDC_REQ : Asn1Sequence
    {
        [Asn1Field(0), Asn1Tag(Asn1TagType.Context, 1)]
        public Asn1Integer pvno { get; set; }
        
        [Asn1Field(1), Asn1Tag(Asn1TagType.Context, 2)]
        public Asn1Integer msg_type { get; set; }
        
        [Asn1Field(2, Optional = true), Asn1Tag(Asn1TagType.Context, 3)]
        public Asn1SequenceOf<PA_DATA> padata { get; set; }
        
        [Asn1Field(3), Asn1Tag(Asn1TagType.Context, 4)]
        public KDC_REQ_BODY req_body { get; set; }
        
        public KDC_REQ()
        {
            this.pvno = null;
            this.msg_type = null;
            this.padata = null;
            this.req_body = null;
        }
        
        public KDC_REQ(
         Asn1Integer param0,
         Asn1Integer param1,
         Asn1SequenceOf<PA_DATA> param2,
         KDC_REQ_BODY param3)
        {
            this.pvno = param0;
            this.msg_type = param1;
            this.padata = param2;
            this.req_body = param3;
        }
    }
}

