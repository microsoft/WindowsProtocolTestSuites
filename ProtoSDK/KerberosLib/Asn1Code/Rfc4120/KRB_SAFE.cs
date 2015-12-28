// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Protocols.TestTools.StackSdk.Asn1;

namespace Microsoft.Protocols.TestTools.StackSdk.Security.KerberosLib
{
    /*
    KRB-SAFE        ::= [APPLICATION 20] SEQUENCE {
        pvno            [0] INTEGER (5),
        msg-type        [1] INTEGER (20),
        safe-body       [2] KRB-SAFE-BODY,
        cksum           [3] Checksum
    }
    */
    [Asn1Tag(Asn1TagType.Application, 20)]
    public class KRB_SAFE : Asn1Sequence
    {
        [Asn1Field(0), Asn1Tag(Asn1TagType.Context, 0)]
        public Asn1Integer pvno { get; set; }
        
        [Asn1Field(1), Asn1Tag(Asn1TagType.Context, 1)]
        public Asn1Integer msg_type { get; set; }
        
        [Asn1Field(2), Asn1Tag(Asn1TagType.Context, 2)]
        public KRB_SAFE_BODY safe_body { get; set; }
        
        [Asn1Field(3), Asn1Tag(Asn1TagType.Context, 3)]
        public Checksum cksum { get; set; }
        
        public KRB_SAFE()
        {
            this.pvno = null;
            this.msg_type = null;
            this.safe_body = null;
            this.cksum = null;
        }
        
        public KRB_SAFE(
         Asn1Integer param0,
         Asn1Integer param1,
         KRB_SAFE_BODY param2,
         Checksum param3)
        {
            this.pvno = param0;
            this.msg_type = param1;
            this.safe_body = param2;
            this.cksum = param3;
        }

        protected override bool VerifyConstraints()
        {
            return pvno != null && pvno.Value == 5 &&
                msg_type != null && msg_type.Value == 20;
        }
    }
}

