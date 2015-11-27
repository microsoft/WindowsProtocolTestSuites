// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Protocols.TestTools.StackSdk.Asn1;

namespace Microsoft.Protocols.TestTools.StackSdk.Security.KerberosLib
{
    /*
    ETYPE-INFO-ENTRY        ::= SEQUENCE {
        etype           [0] Int32,
        salt            [1] OCTET STRING OPTIONAL
    }
    */
    public class ETYPE_INFO_ENTRY : Asn1Sequence
    {
        [Asn1Field(0), Asn1Tag(Asn1TagType.Context, 0)]
        public KerbInt32 etype { get; set; }
        
        [Asn1Field(1, Optional = true), Asn1Tag(Asn1TagType.Context, 1)]
        public Asn1OctetString salt { get; set; }
        
        public ETYPE_INFO_ENTRY()
        {
            this.etype = null;
            this.salt = null;
        }
        
        public ETYPE_INFO_ENTRY(
         KerbInt32 param0,
         Asn1OctetString param1)
        {
            this.etype = param0;
            this.salt = param1;
        }
    }
}

