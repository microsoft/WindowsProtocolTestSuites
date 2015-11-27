// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Protocols.TestTools.StackSdk.Asn1;

namespace Microsoft.Protocols.TestTools.StackSdk.Security.Spng
{
    /*
    NegTokenInit2 ::= SEQUENCE {
 mechTypes[0] MechTypeList OPTIONAL,
 reqFlags [1] ContextFlags OPTIONAL,
 mechToken [2] OCTET STRING OPTIONAL,
 negHints [3] NegHints OPTIONAL,
 mechListMIC [4] OCTET STRING OPTIONAL,
 ...
}
    */
    public class NegTokenInit2 : Asn1Sequence
    {
        [Asn1Field(0, Optional = true), Asn1Tag(Asn1TagType.Context, 0)]
        public MechTypeList mechTypes { get; set; }
        
        [Asn1Field(1, Optional = true), Asn1Tag(Asn1TagType.Context, 1)]
        public ContextFlags reqFlags { get; set; }
        
        [Asn1Field(2, Optional = true), Asn1Tag(Asn1TagType.Context, 2)]
        public Asn1OctetString mechToken { get; set; }
        
        [Asn1Field(3, Optional = true), Asn1Tag(Asn1TagType.Context, 3)]
        public NegHints negHints { get; set; }
        
        [Asn1Field(4, Optional = true), Asn1Tag(Asn1TagType.Context, 4)]
        public Asn1OctetString mechListMIC { get; set; }
        
        public NegTokenInit2()
        {
            this.mechTypes = null;
            this.reqFlags = null;
            this.mechToken = null;
            this.negHints = null;
            this.mechListMIC = null;
        }
        
        public NegTokenInit2(
         MechTypeList mechTypes,
         ContextFlags reqFlags,
         Asn1OctetString mechToken,
         NegHints negHints,
         Asn1OctetString mechListMIC)
        {
            this.mechTypes = mechTypes;
            this.reqFlags = reqFlags;
            this.mechToken = mechToken;
            this.negHints = negHints;
            this.mechListMIC = mechListMIC;
        }
    }
}

