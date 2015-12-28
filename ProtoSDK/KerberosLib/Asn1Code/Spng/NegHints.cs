// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Protocols.TestTools.StackSdk.Asn1;

namespace Microsoft.Protocols.TestTools.StackSdk.Security.Spng
{
    /*
    NegHints ::= SEQUENCE {
        hintName[0] GeneralString OPTIONAL,
        hintAddress[1] OCTET STRING OPTIONAL
    }
    */
    public class NegHints : Asn1Sequence
    {
        [Asn1Field(0, Optional = true), Asn1Tag(Asn1TagType.Context, 0)]
        public Asn1GeneralString hintName { get; set; }
        
        [Asn1Field(1, Optional = true), Asn1Tag(Asn1TagType.Context, 1)]
        public Asn1OctetString hintAddress { get; set; }
        
        public NegHints()
        {
            this.hintName = null;
            this.hintAddress = null;
        }
        
        public NegHints(
         Asn1GeneralString hintName,
         Asn1OctetString hintAddress)
        {
            this.hintName = hintName;
            this.hintAddress = hintAddress;
        }
    }
}

