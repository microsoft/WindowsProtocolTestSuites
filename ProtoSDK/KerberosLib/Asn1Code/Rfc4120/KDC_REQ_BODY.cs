// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Protocols.TestTools.StackSdk.Asn1;

namespace Microsoft.Protocols.TestTools.StackSdk.Security.KerberosLib
{
    /*
    KDC-REQ-BODY    ::= SEQUENCE {
        kdc-options             [0] KDCOptions,

        cname                   [1] PrincipalName OPTIONAL
                                    -- Used only in AS-REQ --,
        realm                   [2] Realm
                                    -- Server's realm
                                    -- Also client's in AS-REQ --,
        sname                   [3] PrincipalName OPTIONAL,
        from                    [4] KerberosTime OPTIONAL,
        till                    [5] KerberosTime,
        rtime                   [6] KerberosTime OPTIONAL,
        nonce                   [7] UInt32,
        etype                   [8] SEQUENCE OF Int32 -- EncryptionType
                                    -- in preference order --,
        addresses               [9] HostAddresses OPTIONAL,
        enc-authorization-data  [10] EncryptedData OPTIONAL
                                    -- AuthorizationData --,
        additional-tickets      [11] SEQUENCE OF Ticket OPTIONAL
                                        -- NOTE: not empty
    }
    */
    public class KDC_REQ_BODY : Asn1Sequence
    {
        
        [Asn1Field(0), Asn1Tag(Asn1TagType.Context, 0)]
        public KDCOptions kdc_options { get; set; }
        
        [Asn1Field(1, Optional = true), Asn1Tag(Asn1TagType.Context, 1)]
        public PrincipalName cname { get; set; }
        
        [Asn1Field(2), Asn1Tag(Asn1TagType.Context, 2)]
        public Realm realm { get; set; }
        
        [Asn1Field(3, Optional = true), Asn1Tag(Asn1TagType.Context, 3)]
        public PrincipalName sname { get; set; }
        
        [Asn1Field(4, Optional = true), Asn1Tag(Asn1TagType.Context, 4)]
        public KerberosTime from { get; set; }
        
        [Asn1Field(5), Asn1Tag(Asn1TagType.Context, 5)]
        public KerberosTime till { get; set; }
        
        [Asn1Field(6, Optional = true), Asn1Tag(Asn1TagType.Context, 6)]
        public KerberosTime rtime { get; set; }
        
        [Asn1Field(7), Asn1Tag(Asn1TagType.Context, 7)]
        public KerbUInt32 nonce { get; set; }
        
        [Asn1Field(8), Asn1Tag(Asn1TagType.Context, 8)]
        public Asn1SequenceOf<KerbInt32> etype { get; set; }
        
        [Asn1Field(9, Optional = true), Asn1Tag(Asn1TagType.Context, 9)]
        public HostAddresses addresses { get; set; }
        
        [Asn1Field(10, Optional = true), Asn1Tag(Asn1TagType.Context, 10)]
        public EncryptedData enc_authorization_data { get; set; }
        
        [Asn1Field(11, Optional = true), Asn1Tag(Asn1TagType.Context, 11)]
        public Asn1SequenceOf<Ticket> additional_tickets { get; set; }
        
        public KDC_REQ_BODY()
        {
            this.kdc_options = null;
            this.cname = null;
            this.realm = null;
            this.sname = null;
            this.from = null;
            this.till = null;
            this.rtime = null;
            this.nonce = null;
            this.etype = null;
            this.addresses = null;
            this.enc_authorization_data = null;
            this.additional_tickets = null;
        }
        
        public KDC_REQ_BODY(
         KDCOptions param0,
         PrincipalName param1,
         Realm param2,
         PrincipalName param3,
         KerberosTime param4,
         KerberosTime param5,
         KerberosTime param6,
         KerbUInt32 param7,
         Asn1SequenceOf<KerbInt32> param8,
         HostAddresses param9,
         EncryptedData param10,
         Asn1SequenceOf<Ticket> param11)
        {
            this.kdc_options = param0;
            this.cname = param1;
            this.realm = param2;
            this.sname = param3;
            this.from = param4;
            this.till = param5;
            this.rtime = param6;
            this.nonce = param7;
            this.etype = param8;
            this.addresses = param9;
            this.enc_authorization_data = param10;
            this.additional_tickets = param11;
        }
    }
}

