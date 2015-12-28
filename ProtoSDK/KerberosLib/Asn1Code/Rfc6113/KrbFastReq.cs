// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Protocols.TestTools.StackSdk.Asn1;
using Microsoft.Protocols.TestTools.StackSdk.Security.KerberosLib;

namespace Microsoft.Protocols.TestTools.StackSdk.Security.KerberosV5.Preauth 
{
    /*
    KrbFastReq ::= SEQUENCE {
          fast-options [0] FastOptions,
              -- Additional options.
          padata       [1] SEQUENCE OF PA-DATA,
              -- padata typed holes.
          req-body     [2] KDC-REQ-BODY,
              -- Contains the KDC request body as defined in Section
              -- 5.4.1 of [RFC4120].
              -- This req-body field is preferred over the outer field
              -- in the KDC request.
           ...
      }
    */
    public class KrbFastReq : Asn1Sequence
    {
        [Asn1Field(0), Asn1Tag(Asn1TagType.Context, 0)]
        public FastOptions fast_options { get; set; }
        
        [Asn1Field(1), Asn1Tag(Asn1TagType.Context, 1)]
        public Asn1SequenceOf<PA_DATA> padata { get; set; }
        
        [Asn1Field(2), Asn1Tag(Asn1TagType.Context, 2)]
        public KDC_REQ_BODY req_body { get; set; }
        
        public KrbFastReq()
        {
            this.fast_options = null;
            this.padata = null;
            this.req_body = null;
        }
        
        public KrbFastReq(
         FastOptions param0,
         Asn1SequenceOf<PA_DATA> param1,
         KDC_REQ_BODY param2)
        {
            this.fast_options = param0;
            this.padata = param1;
            this.req_body = param2;
        }
    }
}

