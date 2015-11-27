// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Protocols.TestTools.StackSdk.Asn1;

namespace Microsoft.Protocols.TestTools.StackSdk.Security.KerberosLib
{
    /*
    KDC-REP         ::= SEQUENCE {
        pvno            [0] INTEGER (5),
        msg-type        [1] INTEGER (11 -- AS -- | 13 -- TGS --),
        padata          [2] SEQUENCE OF PA-DATA OPTIONAL
                                -- NOTE: not empty --,
        crealm          [3] Realm,
        cname           [4] PrincipalName,
        ticket          [5] Ticket,
        enc-part        [6] EncryptedData
                                -- EncASRepPart or EncTGSRepPart,
                                -- as appropriate
    }
    */
    public class KDC_REP : Asn1Sequence
    {
        [Asn1Field(0), Asn1Tag(Asn1TagType.Context, 0)]
        public Asn1Integer pvno { get; set; }
        
        [Asn1Field(1), Asn1Tag(Asn1TagType.Context, 1)]
        public Asn1Integer msg_type { get; set; }
        
        [Asn1Field(2, Optional = true), Asn1Tag(Asn1TagType.Context, 2)]
        public Asn1SequenceOf<PA_DATA> padata { get; set; }
        
        [Asn1Field(3), Asn1Tag(Asn1TagType.Context, 3)]
        public Realm crealm { get; set; }
        
        [Asn1Field(4), Asn1Tag(Asn1TagType.Context, 4)]
        public PrincipalName cname { get; set; }
        
        [Asn1Field(5), Asn1Tag(Asn1TagType.Context, 5)]
        public Ticket ticket { get; set; }
        
        [Asn1Field(6), Asn1Tag(Asn1TagType.Context, 6)]
        public EncryptedData enc_part { get; set; }
        
        public KDC_REP()
        {
            this.pvno = null;
            this.msg_type = null;
            this.padata = null;
            this.crealm = null;
            this.cname = null;
            this.ticket = null;
            this.enc_part = null;
        }
        
        public KDC_REP(
         Asn1Integer param0,
         Asn1Integer param1,
         Asn1SequenceOf<PA_DATA> param2,
         Realm param3,
         PrincipalName param4,
         Ticket param5,
         EncryptedData param6)
        {
            this.pvno = param0;
            this.msg_type = param1;
            this.padata = param2;
            this.crealm = param3;
            this.cname = param4;
            this.ticket = param5;
            this.enc_part = param6;
        }

        protected override bool VerifyConstraints()
        {
            return pvno != null && pvno.Value == 5 && msg_type != null &&
                (msg_type.Value == 11 || msg_type.Value == 13);
        }
    }
}

