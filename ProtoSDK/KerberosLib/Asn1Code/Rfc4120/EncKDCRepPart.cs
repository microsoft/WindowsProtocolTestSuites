// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Protocols.TestTools.StackSdk.Asn1;

namespace Microsoft.Protocols.TestTools.StackSdk.Security.KerberosLib
{
    /*
    EncKDCRepPart   ::= SEQUENCE {
           key             [0] EncryptionKey,
           last-req        [1] LastReq,
           nonce           [2] UInt32,
           key-expiration  [3] KerberosTime OPTIONAL,
           flags           [4] TicketFlags,
           authtime        [5] KerberosTime,
           starttime       [6] KerberosTime OPTIONAL,
           endtime         [7] KerberosTime,
           renew-till      [8] KerberosTime OPTIONAL,
           srealm          [9] Realm,
           sname           [10] PrincipalName,
           caddr           [11] HostAddresses OPTIONAL
           pa_datas     [12] SEQUENCE OF PA-DATA OPTIONAL
    }
    */
    public class EncKDCRepPart : Asn1Sequence
    {
        [Asn1Field(0), Asn1Tag(Asn1TagType.Context, 0)]
        public EncryptionKey key { get; set; }
        
        [Asn1Field(1), Asn1Tag(Asn1TagType.Context, 1)]
        public LastReq last_req { get; set; }
        
        [Asn1Field(2), Asn1Tag(Asn1TagType.Context, 2)]
        public KerbUInt32 nonce { get; set; }
        
        [Asn1Field(3, Optional = true), Asn1Tag(Asn1TagType.Context, 3)]
        public KerberosTime key_expiration { get; set; }
        
        [Asn1Field(4), Asn1Tag(Asn1TagType.Context, 4)]
        public TicketFlags flags { get; set; }
        
        [Asn1Field(5), Asn1Tag(Asn1TagType.Context, 5)]
        public KerberosTime authtime { get; set; }
        
        [Asn1Field(6, Optional = true), Asn1Tag(Asn1TagType.Context, 6)]
        public KerberosTime starttime { get; set; }
        
        [Asn1Field(7), Asn1Tag(Asn1TagType.Context, 7)]
        public KerberosTime endtime { get; set; }
        
        [Asn1Field(8, Optional = true), Asn1Tag(Asn1TagType.Context, 8)]
        public KerberosTime renew_till { get; set; }
        
        [Asn1Field(9), Asn1Tag(Asn1TagType.Context, 9)]
        public Realm srealm { get; set; }
        
        [Asn1Field(10), Asn1Tag(Asn1TagType.Context, 10)]
        public PrincipalName sname { get; set; }
        
        [Asn1Field(11, Optional = true), Asn1Tag(Asn1TagType.Context, 11)]
        public HostAddresses caddr { get; set; }
        
        [Asn1Field(12, Optional = true), Asn1Tag(Asn1TagType.Context, 12)]
        public Asn1SequenceOf<PA_DATA> pa_datas { get; set; }
        
        public EncKDCRepPart()
        {
            this.key = null;
            this.last_req = null;
            this.nonce = null;
            this.key_expiration = null;
            this.flags = null;
            this.authtime = null;
            this.starttime = null;
            this.endtime = null;
            this.renew_till = null;
            this.srealm = null;
            this.sname = null;
            this.caddr = null;
            this.pa_datas = null;
        }
        
        public EncKDCRepPart(
         EncryptionKey param0,
         LastReq param1,
         KerbUInt32 param2,
         KerberosTime param3,
         TicketFlags param4,
         KerberosTime param5,
         KerberosTime param6,
         KerberosTime param7,
         KerberosTime param8,
         Realm param9,
         PrincipalName param10,
         HostAddresses param11,
         Asn1SequenceOf<PA_DATA> param12)
        {
            this.key = param0;
            this.last_req = param1;
            this.nonce = param2;
            this.key_expiration = param3;
            this.flags = param4;
            this.authtime = param5;
            this.starttime = param6;
            this.endtime = param7;
            this.renew_till = param8;
            this.srealm = param9;
            this.sname = param10;
            this.caddr = param11;
            this.pa_datas = param12;
        }
    }
}

