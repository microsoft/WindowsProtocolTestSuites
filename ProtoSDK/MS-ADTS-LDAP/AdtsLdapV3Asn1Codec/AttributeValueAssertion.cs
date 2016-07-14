// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Protocols.TestTools.StackSdk.Asn1;

namespace Microsoft.Protocols.TestTools.StackSdk.ActiveDirectory.Adts.Asn1CodecV3
{
    /*
     AttributeValueAssertion ::= SEQUENCE {
                attributeDesc   AttributeDescription,
                assertionValue  AssertionValue }
    */
    public class AttributeValueAssertion : Asn1Sequence
    {
        [Asn1Field(0)]
        public AttributeDescription attributeDesc { get; set; }
        
        [Asn1Field(1)]
        public AssertionValue assertionValue { get; set; }
        
        public AttributeValueAssertion()
        {
            this.attributeDesc = null;
            this.assertionValue = null;
        }
        
        public AttributeValueAssertion(
         AttributeDescription attributeDesc,
         AssertionValue assertionValue)
        {
            this.attributeDesc = attributeDesc;
            this.assertionValue = assertionValue;
        }
    }
}

