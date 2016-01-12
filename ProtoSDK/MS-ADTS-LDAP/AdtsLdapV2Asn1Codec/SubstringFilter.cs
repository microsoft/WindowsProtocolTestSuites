// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Protocols.TestTools.StackSdk.Asn1;

namespace Microsoft.Protocols.TestTools.StackSdk.ActiveDirectory.Adts.Asn1CodecV2
{
    /*
    SubstringFilter ::= SEQUENCE {
                type            AttributeType,
                -- at least one must be present
                substrings      SEQUENCE OF SubstringFilter_substrings }

    */
    public class SubstringFilter : Asn1Sequence
    {
        [Asn1Field(0)]
        public AttributeType type { get; set; }
        
        [Asn1Field(1)]
        public Asn1SequenceOf<SubstringFilter_substrings_element> substrings { get; set; }
        
        public SubstringFilter()
        {
            this.type = null;
            this.substrings = null;
        }
        
        public SubstringFilter(
         AttributeType type,
         Asn1SequenceOf<SubstringFilter_substrings_element> substrings)
        {
            this.type = type;
            this.substrings = substrings;
        }
    }
}

