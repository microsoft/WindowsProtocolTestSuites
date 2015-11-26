// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Protocols.TestTools.StackSdk.Asn1;

namespace Microsoft.Protocols.TestTools.StackSdk.Security.Spng
{
    /*
    NegotiationToken2 ::= CHOICE{
        negTokenInit2 [0] NegTokenInit2
    }
    */
    public class NegotiationToken2 : Asn1Choice
    {
        [Asn1ChoiceIndex]
        public const long negTokenInit2 = 0;
        [Asn1ChoiceElement(negTokenInit2), Asn1Tag(Asn1TagType.Context, 0)]
        protected NegTokenInit2 field0 { get; set; }
        
        public NegotiationToken2()
            : base()
        {
        }
        
        public NegotiationToken2(long? choiceIndex, Asn1Object obj)
            : base(choiceIndex, obj)
        {
        }
    }
}

