// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Protocols.TestTools.StackSdk.Asn1;

namespace Microsoft.Protocols.TestTools.StackSdk.Security.KerberosLib
{
    /*
    ETYPE-INFO2-ENTRY       ::= SEQUENCE {
        etype           [0] Int32,
        salt            [1] KerberosString OPTIONAL,
        s2kparams       [2] OCTET STRING OPTIONAL
    }
    */
    public class ETYPE_INFO2_ENTRY : Asn1Sequence
    {
        [Asn1Field(0), Asn1Tag(Asn1TagType.Context, 0)]
        public KerbInt32 etype { get; set; }
        
        [Asn1Field(1, Optional = true), Asn1Tag(Asn1TagType.Context, 1)]
        public KerberosString salt { get; set; }
        
        [Asn1Field(2, Optional = true), Asn1Tag(Asn1TagType.Context, 2)]
        public Asn1OctetString s2kparams { get; set; }
        
        public ETYPE_INFO2_ENTRY()
        {
            this.etype = null;
            this.salt = null;
            this.s2kparams = null;
        }
        
        public ETYPE_INFO2_ENTRY(
         KerbInt32 param0,
         KerberosString param1,
         Asn1OctetString param2)
        {
            this.etype = param0;
            this.salt = param1;
            this.s2kparams = param2;
        }
    }
}

