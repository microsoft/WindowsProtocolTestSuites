// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Protocols.TestTools.StackSdk.Asn1;

namespace Microsoft.Protocols.TestTools.StackSdk.Security.KerberosLib
{
    /*
    EncKrbCredPart  ::= [APPLICATION 29] SEQUENCE {
        ticket-info     [0] SEQUENCE OF KrbCredInfo,
        nonce           [1] UInt32 OPTIONAL,
        timestamp       [2] KerberosTime OPTIONAL,
        usec            [3] Microseconds OPTIONAL,
        s-address       [4] HostAddress OPTIONAL,
        r-address       [5] HostAddress OPTIONAL
    }
    */
    [Asn1Tag(Asn1TagType.Application, 29)]
    public class EncKrbCredPart : Asn1Sequence
    {
        [Asn1Field(0), Asn1Tag(Asn1TagType.Context, 0)]
        public Asn1SequenceOf<KrbCredInfo> ticket_info { get; set; }
        
        [Asn1Field(1, Optional = true), Asn1Tag(Asn1TagType.Context, 1)]
        public KerbUInt32 nonce { get; set; }
        
        [Asn1Field(2, Optional = true), Asn1Tag(Asn1TagType.Context, 2)]
        public KerberosTime timestamp { get; set; }
        
        [Asn1Field(3, Optional = true), Asn1Tag(Asn1TagType.Context, 3)]
        public Microseconds usec { get; set; }
        
        [Asn1Field(4, Optional = true), Asn1Tag(Asn1TagType.Context, 4)]
        public HostAddress s_address { get; set; }
        
        [Asn1Field(5, Optional = true), Asn1Tag(Asn1TagType.Context, 5)]
        public HostAddress r_address { get; set; }
        
        public EncKrbCredPart()
        {
            this.ticket_info = null;
            this.nonce = null;
            this.timestamp = null;
            this.usec = null;
            this.s_address = null;
            this.r_address = null;
        }
        
        public EncKrbCredPart(
         Asn1SequenceOf<KrbCredInfo> param0,
         KerbUInt32 param1,
         KerberosTime param2,
         Microseconds param3,
         HostAddress param4,
         HostAddress param5)
        {
            this.ticket_info = param0;
            this.nonce = param1;
            this.timestamp = param2;
            this.usec = param3;
            this.s_address = param4;
            this.r_address = param5;
        }
    }
}

