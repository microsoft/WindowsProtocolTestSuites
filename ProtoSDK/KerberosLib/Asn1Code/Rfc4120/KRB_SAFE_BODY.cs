// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Protocols.TestTools.StackSdk.Asn1;

namespace Microsoft.Protocols.TestTools.StackSdk.Security.KerberosLib
{
    /*
    KRB-SAFE-BODY   ::= SEQUENCE {
        user-data       [0] OCTET STRING,
        timestamp       [1] KerberosTime OPTIONAL,
        usec            [2] Microseconds OPTIONAL,
        seq-number      [3] UInt32 OPTIONAL,
        s-address       [4] HostAddress,
        r-address       [5] HostAddress OPTIONAL
    }
    */
    public class KRB_SAFE_BODY : Asn1Sequence
    {
        [Asn1Field(0), Asn1Tag(Asn1TagType.Context, 0)]
        public Asn1OctetString user_data { get; set; }
        
        [Asn1Field(1, Optional = true), Asn1Tag(Asn1TagType.Context, 1)]
        public KerberosTime timestamp { get; set; }
        
        [Asn1Field(2, Optional = true), Asn1Tag(Asn1TagType.Context, 2)]
        public Microseconds usec { get; set; }
        
        [Asn1Field(3, Optional = true), Asn1Tag(Asn1TagType.Context, 3)]
        public KerbUInt32 seq_number { get; set; }
        
        [Asn1Field(4), Asn1Tag(Asn1TagType.Context, 4)]
        public HostAddress s_address { get; set; }
        
        [Asn1Field(5, Optional = true), Asn1Tag(Asn1TagType.Context, 5)]
        public HostAddress r_address { get; set; }
        
        public KRB_SAFE_BODY()
        {
            this.user_data = null;
            this.timestamp = null;
            this.usec = null;
            this.seq_number = null;
            this.s_address = null;
            this.r_address = null;
        }
        
        public KRB_SAFE_BODY(
         Asn1OctetString user_data,
         KerberosTime timestamp,
         Microseconds usec,
         KerbUInt32 seq_number,
         HostAddress s_address,
         HostAddress r_address)
        {
            this.user_data = user_data;
            this.timestamp = timestamp;
            this.usec = usec;
            this.seq_number = seq_number;
            this.s_address = s_address;
            this.r_address = r_address;
        }
    }
}

