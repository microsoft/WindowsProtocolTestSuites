// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Protocols.TestTools.StackSdk.Asn1;

namespace Microsoft.Protocols.TestTools.StackSdk.Security.KerberosLib
{
    /*
    Checksum        ::= SEQUENCE {
        cksumtype       [0] Int32,
        checksum        [1] OCTET STRING
    }
    */
    public class Checksum : Asn1Sequence
    {
        [Asn1Field(0), Asn1Tag(Asn1TagType.Context, 0)]
        public KerbInt32 cksumtype { get; set; }
        
        [Asn1Field(1), Asn1Tag(Asn1TagType.Context, 1)]
        public Asn1OctetString checksum { get; set; }
        
        public Checksum()
        {
            this.cksumtype = null;
            this.checksum = null;
        }
        
        public Checksum(
         KerbInt32 param0,
         Asn1OctetString param1)
        {
            this.cksumtype = param0;
            this.checksum = param1;
        }
    }
}

