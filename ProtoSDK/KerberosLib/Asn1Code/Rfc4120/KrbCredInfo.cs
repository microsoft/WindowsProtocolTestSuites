// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Protocols.TestTools.StackSdk.Asn1;

namespace Microsoft.Protocols.TestTools.StackSdk.Security.KerberosLib
{
    /*
    KrbCredInfo     ::= SEQUENCE {
        key             [0] EncryptionKey,
        prealm          [1] Realm OPTIONAL,
        pname           [2] PrincipalName OPTIONAL,
        flags           [3] TicketFlags OPTIONAL,
        authtime        [4] KerberosTime OPTIONAL,
        starttime       [5] KerberosTime OPTIONAL,
        endtime         [6] KerberosTime OPTIONAL,
        renew-till      [7] KerberosTime OPTIONAL,
        srealm          [8] Realm OPTIONAL,
        sname           [9] PrincipalName OPTIONAL,
        caddr           [10] HostAddresses OPTIONAL
    }
    */
    public class KrbCredInfo : Asn1Sequence
    {
        [Asn1Field(0), Asn1Tag(Asn1TagType.Context, 0)]
        public EncryptionKey key { get; set; }
        
        [Asn1Field(1, Optional = true), Asn1Tag(Asn1TagType.Context, 1)]
        public Realm prealm { get; set; }
        
        [Asn1Field(2, Optional = true), Asn1Tag(Asn1TagType.Context, 2)]
        public PrincipalName pname { get; set; }
        
        [Asn1Field(3, Optional = true), Asn1Tag(Asn1TagType.Context, 3)]
        public TicketFlags flags { get; set; }
        
        [Asn1Field(4, Optional = true), Asn1Tag(Asn1TagType.Context, 4)]
        public KerberosTime authtime { get; set; }
        
        [Asn1Field(5, Optional = true), Asn1Tag(Asn1TagType.Context, 5)]
        public KerberosTime starttime { get; set; }
        
        [Asn1Field(6, Optional = true), Asn1Tag(Asn1TagType.Context, 6)]
        public KerberosTime endtime { get; set; }
        
        [Asn1Field(7, Optional = true), Asn1Tag(Asn1TagType.Context, 7)]
        public KerberosTime renew_till { get; set; }
        
        [Asn1Field(8, Optional = true), Asn1Tag(Asn1TagType.Context, 8)]
        public Realm srealm { get; set; }
        
        [Asn1Field(9, Optional = true), Asn1Tag(Asn1TagType.Context, 9)]
        public PrincipalName sname { get; set; }
        
        [Asn1Field(10, Optional = true), Asn1Tag(Asn1TagType.Context, 10)]
        public HostAddresses caddr { get; set; }
        
        public KrbCredInfo()
        {
            this.key = null;
            this.prealm = null;
            this.pname = null;
            this.flags = null;
            this.authtime = null;
            this.starttime = null;
            this.endtime = null;
            this.renew_till = null;
            this.srealm = null;
            this.sname = null;
            this.caddr = null;
        }
        
        public KrbCredInfo(
         EncryptionKey param0,
         Realm param1,
         PrincipalName param2,
         TicketFlags param3,
         KerberosTime param4,
         KerberosTime param5,
         KerberosTime param6,
         KerberosTime param7,
         Realm param8,
         PrincipalName param9,
         HostAddresses param10)
        {
            this.key = param0;
            this.prealm = param1;
            this.pname = param2;
            this.flags = param3;
            this.authtime = param4;
            this.starttime = param5;
            this.endtime = param6;
            this.renew_till = param7;
            this.srealm = param8;
            this.sname = param9;
            this.caddr = param10;
        }
    }
}

