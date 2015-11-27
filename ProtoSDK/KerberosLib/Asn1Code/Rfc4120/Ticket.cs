// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Protocols.TestTools.StackSdk.Asn1;

namespace Microsoft.Protocols.TestTools.StackSdk.Security.KerberosLib
{
    /*
    Ticket          ::= [APPLICATION 1] SEQUENCE {
        tkt-vno         [0] INTEGER (5),
        realm           [1] Realm,
        sname           [2] PrincipalName,
        enc-part        [3] EncryptedData -- EncTicketPart
    }
    */
    [Asn1Tag(Asn1TagType.Application, 1)]
    public class Ticket : Asn1Sequence
    {
        [Asn1Field(0), Asn1Tag(Asn1TagType.Context, 0)]
        public Asn1Integer tkt_vno { get; set; }
        
        [Asn1Field(1), Asn1Tag(Asn1TagType.Context, 1)]
        public Realm realm { get; set; }
        
        [Asn1Field(2), Asn1Tag(Asn1TagType.Context, 2)]
        public PrincipalName sname { get; set; }
        
        [Asn1Field(3), Asn1Tag(Asn1TagType.Context, 3)]
        public EncryptedData enc_part { get; set; }
        
        public Ticket()
        {
            this.tkt_vno = null;
            this.realm = null;
            this.sname = null;
            this.enc_part = null;
        }
        
        public Ticket(
         Asn1Integer param0,
         Realm param1,
         PrincipalName param2,
         EncryptedData param3)
        {
            this.tkt_vno = param0;
            this.realm = param1;
            this.sname = param2;
            this.enc_part = param3;
        }

        protected override bool VerifyConstraints()
        {
            return (tkt_vno != null && tkt_vno.Value == 5);
        }
    }
}

