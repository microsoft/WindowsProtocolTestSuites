// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Protocols.TestTools.StackSdk.Asn1;

namespace Microsoft.Protocols.TestTools.StackSdk.Security.KerberosLib
{
    /*
    HostAddress     ::= SEQUENCE  {
        addr-type       [0] Int32,
        address         [1] OCTET STRING
    }
    */
    public class HostAddress : Asn1Sequence
    {
        [Asn1Field(0), Asn1Tag(Asn1TagType.Context, 0)]
        public KerbInt32 addr_type { get; set; }

        [Asn1Field(1), Asn1Tag(Asn1TagType.Context, 1)]
        public Asn1OctetString address { get; set; }

        public HostAddress()
        {
            this.addr_type = null;
            this.address = null;
        }

        public HostAddress(
         KerbInt32 param0,
         Asn1OctetString param1)
        {
            this.addr_type = param0;
            this.address = param1;
        }
    }
}

