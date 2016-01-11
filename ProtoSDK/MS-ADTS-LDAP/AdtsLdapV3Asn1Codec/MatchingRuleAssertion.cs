// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Protocols.TestTools.StackSdk.Asn1;

namespace Microsoft.Protocols.TestTools.StackSdk.ActiveDirectory.Adts.Asn1CodecV3
{
    /*
     MatchingRuleAssertion ::= SEQUENCE {
                matchingRule    [1] MatchingRuleId OPTIONAL,
                type            [2] AttributeDescription OPTIONAL,
                matchValue      [3] AssertionValue,
                dnAttributes    [4] BOOLEAN DEFAULT FALSE}
    */
    public class MatchingRuleAssertion : Asn1Sequence
    {
        [Asn1Field(0, Optional = true), Asn1Tag(Asn1TagType.Context, 1)]
        public MatchingRuleId matchingRule { get; set; }
        
        [Asn1Field(1, Optional = true), Asn1Tag(Asn1TagType.Context, 2)]
        public AttributeDescription type { get; set; }
        
        [Asn1Field(2), Asn1Tag(Asn1TagType.Context, 3)]
        public AssertionValue matchValue { get; set; }
        
        [Asn1Field(3), Asn1Tag(Asn1TagType.Context, 4)]
        public Asn1Boolean dnAttributes { get; set; }
        
        public MatchingRuleAssertion()
        {
            this.matchingRule = null;
            this.type = null;
            this.matchValue = null;
            this.dnAttributes = null;
        }
        
        public MatchingRuleAssertion(
         MatchingRuleId matchingRule,
         AttributeDescription type,
         AssertionValue matchValue,
         Asn1Boolean dnAttributes)
        {
            this.matchingRule = matchingRule;
            this.type = type;
            this.matchValue = matchValue;
            this.dnAttributes = dnAttributes;
        }
    }
}

