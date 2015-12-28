// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Protocols.TestTools.StackSdk.Asn1;

namespace Microsoft.Protocols.TestTools.StackSdk.Security.Spng
{
    /*
    NegotiationToken ::= CHOICE {
       negTokenInit    [0] NegTokenInit,
       negTokenResp    [1] NegTokenResp,
       negTokenInit2    [2] NegTokenInit2
   }
    */
    public class NegotiationToken : Asn1Choice
    {
        [Asn1ChoiceIndex]
        public const long negTokenInit = 0;
        [Asn1ChoiceElement(negTokenInit), Asn1Tag(Asn1TagType.Context, 0)]
        protected NegTokenInit field0 { get; set; }
        
        [Asn1ChoiceIndex]
        public const long negTokenResp = 1;
        [Asn1ChoiceElement(negTokenResp), Asn1Tag(Asn1TagType.Context, 1)]
        protected NegTokenResp field1 { get; set; }
        
        [Asn1ChoiceIndex]
        public const long negTokenInit2 = 2;
        [Asn1ChoiceElement(negTokenInit2), Asn1Tag(Asn1TagType.Context, 2)]
        protected NegTokenInit2 field2 { get; set; }
        
        public NegotiationToken()
            : base()
        {
        }
        
        public NegotiationToken(long? choiceIndex, Asn1Object obj)
            : base(choiceIndex, obj)
        {
        }
    }
}

