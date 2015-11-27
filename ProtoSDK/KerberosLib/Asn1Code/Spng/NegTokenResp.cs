// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Protocols.TestTools.StackSdk.Asn1;

namespace Microsoft.Protocols.TestTools.StackSdk.Security.Spng
{
    /*
    NegTokenResp ::= SEQUENCE {
       negState       [0] ENUMERATED {
           accept-completed    (0),
           accept-incomplete   (1),
           reject              (2),
           request-mic         (3)
       }                                 OPTIONAL,
         -- REQUIRED in the first reply from the target
       supportedMech   [1] MechType      OPTIONAL,
         -- present only in the first reply from the target
       responseToken   [2] OCTET STRING  OPTIONAL,
       mechListMIC     [3] OCTET STRING  OPTIONAL,
       ...
   }
    */
    public class NegTokenResp : Asn1Sequence
    {
        [Asn1Field(0, Optional = true), Asn1Tag(Asn1TagType.Context, 0)]
        public NegState negState { get; set; }
        
        [Asn1Field(1, Optional = true), Asn1Tag(Asn1TagType.Context, 1)]
        public MechType supportedMech { get; set; }
        
        [Asn1Field(2, Optional = true), Asn1Tag(Asn1TagType.Context, 2)]
        public Asn1OctetString responseToken { get; set; }
        
        [Asn1Field(3, Optional = true), Asn1Tag(Asn1TagType.Context, 3)]
        public Asn1OctetString mechListMIC { get; set; }
        
        public NegTokenResp()
        {
            this.negState = null;
            this.supportedMech = null;
            this.responseToken = null;
            this.mechListMIC = null;
        }
        
        public NegTokenResp(
         NegState negState,
         MechType supportedMech,
         Asn1OctetString responseToken,
         Asn1OctetString mechListMIC)
        {
            this.negState = negState;
            this.supportedMech = supportedMech;
            this.responseToken = responseToken;
            this.mechListMIC = mechListMIC;
        }
    }
}

