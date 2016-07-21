// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Protocols.TestTools.StackSdk.Asn1;

namespace Microsoft.Protocols.TestTools.StackSdk.ActiveDirectory.Adts.Asn1CodecV2
{
    /*
    AttributeValueAssertion ::=
    SEQUENCE {
        attributeType        AttributeType,
        attributeValue       AttributeValue
    }
    */
    public class AttributeValueAssertion : Asn1Sequence
    {
        [Asn1Field(0)]
        public AttributeType attributeType { get; set; }
        
        [Asn1Field(1)]
        public AttributeValue attributeValue { get; set; }
        
        public AttributeValueAssertion()
        {
            this.attributeType = null;
            this.attributeValue = null;
        }
        
        public AttributeValueAssertion(
         AttributeType attributeType,
         AttributeValue attributeValue)
        {
            this.attributeType = attributeType;
            this.attributeValue = attributeValue;
        }
    }
}

