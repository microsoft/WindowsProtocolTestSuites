// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Protocols.TestTools.StackSdk.Asn1;

namespace Microsoft.Protocols.TestTools.StackSdk.Security.KerberosLib
{
    /*
    -- Encrypted part of ticket
    EncTicketPart   ::= [APPLICATION 3] SEQUENCE {
        flags                   [0] TicketFlags,
        key                     [1] EncryptionKey,
        crealm                  [2] Realm,
        cname                   [3] PrincipalName,
        transited               [4] TransitedEncoding,
        authtime                [5] KerberosTime,
        starttime               [6] KerberosTime OPTIONAL,
        endtime                 [7] KerberosTime,
        renew-till              [8] KerberosTime OPTIONAL,
        caddr                   [9] HostAddresses OPTIONAL,
        authorization-data      [10] AuthorizationData OPTIONAL
    }
    */
    [Asn1Tag(Asn1TagType.Application, 3)]
    public class EncTicketPart : Asn1Sequence
    {
        [Asn1Field(0), Asn1Tag(Asn1TagType.Context, 0)]
        public TicketFlags flags { get; set; }
        
        [Asn1Field(1), Asn1Tag(Asn1TagType.Context, 1)]
        public EncryptionKey key { get; set; }
        
        [Asn1Field(2), Asn1Tag(Asn1TagType.Context, 2)]
        public Realm crealm { get; set; }
        
        [Asn1Field(3), Asn1Tag(Asn1TagType.Context, 3)]
        public PrincipalName cname { get; set; }
        
        [Asn1Field(4), Asn1Tag(Asn1TagType.Context, 4)]
        public TransitedEncoding transited { get; set; }
        
        [Asn1Field(5), Asn1Tag(Asn1TagType.Context, 5)]
        public KerberosTime authtime { get; set; }
        
        [Asn1Field(6, Optional = true), Asn1Tag(Asn1TagType.Context, 6)]
        public KerberosTime starttime { get; set; }
        
        [Asn1Field(7), Asn1Tag(Asn1TagType.Context, 7)]
        public KerberosTime endtime { get; set; }
        
        [Asn1Field(8, Optional = true), Asn1Tag(Asn1TagType.Context, 8)]
        public KerberosTime renew_till { get; set; }
        
        [Asn1Field(9, Optional = true), Asn1Tag(Asn1TagType.Context, 9)]
        public HostAddresses caddr { get; set; }
        
        [Asn1Field(10, Optional = true), Asn1Tag(Asn1TagType.Context, 10)]
        public AuthorizationData authorization_data { get; set; }
        
        public EncTicketPart()
        {
            this.flags = null;
            this.key = null;
            this.crealm = null;
            this.cname = null;
            this.transited = null;
            this.authtime = null;
            this.starttime = null;
            this.endtime = null;
            this.renew_till = null;
            this.caddr = null;
            this.authorization_data = null;
        }
        
        public EncTicketPart(
         TicketFlags param0,
         EncryptionKey param1,
         Realm param2,
         PrincipalName param3,
         TransitedEncoding param4,
         KerberosTime param5,
         KerberosTime param6,
         KerberosTime param7,
         KerberosTime param8,
         HostAddresses param9,
         AuthorizationData param10)
        {
            this.flags = param0;
            this.key = param1;
            this.crealm = param2;
            this.cname = param3;
            this.transited = param4;
            this.authtime = param5;
            this.starttime = param6;
            this.endtime = param7;
            this.renew_till = param8;
            this.caddr = param9;
            this.authorization_data = param10;
        }
    }
}

