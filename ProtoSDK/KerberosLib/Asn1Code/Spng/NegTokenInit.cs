// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Protocols.TestTools.StackSdk.Asn1;

namespace Microsoft.Protocols.TestTools.StackSdk.Security.Spng
{
    /*
    NegTokenInit ::= SEQUENCE {
       mechTypes       [0] MechTypeList,
       reqFlags        [1] ContextFlags  OPTIONAL,
         -- inherited from RFC 2478 for backward compatibility,
         -- RECOMMENDED to be left out
       mechToken       [2] OCTET STRING  OPTIONAL,
       mechListMIC     [3] OCTET STRING  OPTIONAL,
       ...
   }
    */
    public class NegTokenInit : Asn1Sequence
    {
        [Asn1Field(0), Asn1Tag(Asn1TagType.Context, 0)]
        public MechTypeList mechTypes { get; set; }
        
        [Asn1Field(1, Optional = true), Asn1Tag(Asn1TagType.Context, 1)]
        public ContextFlags reqFlags { get; set; }
        
        [Asn1Field(2, Optional = true), Asn1Tag(Asn1TagType.Context, 2)]
        public Asn1OctetString mechToken { get; set; }
        
        [Asn1Field(3, Optional = true), Asn1Tag(Asn1TagType.Context, 3)]
        public Asn1OctetString mechListMIC { get; set; }
        
        public NegTokenInit()
        {
            this.mechTypes = null;
            this.reqFlags = null;
            this.mechToken = null;
            this.mechListMIC = null;
        }
        
        public NegTokenInit(
         MechTypeList mechTypes,
         ContextFlags reqFlags,
         Asn1OctetString mechToken,
         Asn1OctetString mechListMIC)
        {
            this.mechTypes = mechTypes;
            this.reqFlags = reqFlags;
            this.mechToken = mechToken;
            this.mechListMIC = mechListMIC;
        }
    }
}

