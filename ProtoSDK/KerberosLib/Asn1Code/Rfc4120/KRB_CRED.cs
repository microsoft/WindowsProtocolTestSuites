// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Protocols.TestTools.StackSdk.Asn1;

namespace Microsoft.Protocols.TestTools.StackSdk.Security.KerberosLib
{
    /*
    KRB-CRED        ::= [APPLICATION 22] SEQUENCE {
        pvno            [0] INTEGER (5),
        msg-type        [1] INTEGER (22),
        tickets         [2] SEQUENCE OF Ticket,
        enc-part        [3] EncryptedData -- EncKrbCredPart
    }
    */
    [Asn1Tag(Asn1TagType.Application, 22)]
    public class KRB_CRED : Asn1Sequence
    {
        [Asn1Field(0), Asn1Tag(Asn1TagType.Context, 0)]
        public Asn1Integer pvno { get; set; }
        
        [Asn1Field(1), Asn1Tag(Asn1TagType.Context, 1)]
        public Asn1Integer msg_type { get; set; }
        
        [Asn1Field(2), Asn1Tag(Asn1TagType.Context, 2)]
        public Asn1SequenceOf<Ticket> tickets { get; set; }
        
        [Asn1Field(3), Asn1Tag(Asn1TagType.Context, 3)]
        public EncryptedData enc_part { get; set; }
        
        public KRB_CRED()
        {
            this.pvno = null;
            this.msg_type = null;
            this.tickets = null;
            this.enc_part = null;
        }
        
        public KRB_CRED(
         Asn1Integer param0,
         Asn1Integer param1,
         Asn1SequenceOf<Ticket> param2,
         EncryptedData param3)
        {
            this.pvno = param0;
            this.msg_type = param1;
            this.tickets = param2;
            this.enc_part = param3;
        }

        protected override bool VerifyConstraints()
        {
            return pvno != null && pvno.Value == 5 &&
                msg_type != null && msg_type.Value == 22;
        }
    }
}

