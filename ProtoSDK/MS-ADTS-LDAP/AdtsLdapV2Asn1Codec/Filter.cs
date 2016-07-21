// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Protocols.TestTools.StackSdk.Asn1;

namespace Microsoft.Protocols.TestTools.StackSdk.ActiveDirectory.Adts.Asn1CodecV2
{
    /*
    Filter ::=
    CHOICE {
        and            [0] SET OF Filter,
        or             [1] SET OF Filter,
        not            [2] Filter,
        equalityMatch  [3] AttributeValueAssertion,
        substrings     [4] SubstringFilter,
        greaterOrEqual [5] AttributeValueAssertion,
        lessOrEqual    [6] AttributeValueAssertion,
        present        [7] AttributeType,
        approxMatch    [8] AttributeValueAssertion
    }
    */
    public class Filter : Asn1Choice
    {
        [Asn1ChoiceIndex]
        public const long and = 0;
        [Asn1ChoiceElement(and), Asn1Tag(Asn1TagType.Context, 0)]
        protected Asn1SetOf<Filter> field0 { get; set; }
        
        [Asn1ChoiceIndex]
        public const long or = 1;
        [Asn1ChoiceElement(or), Asn1Tag(Asn1TagType.Context, 1)]
        protected Asn1SetOf<Filter> field1 { get; set; }
        
        [Asn1ChoiceIndex]
        public const long not = 2;
        [Asn1ChoiceElement(not), Asn1Tag(Asn1TagType.Context, 2)]
        protected Filter field2 { get; set; }
        
        [Asn1ChoiceIndex]
        public const long equalityMatch = 3;
        [Asn1ChoiceElement(equalityMatch), Asn1Tag(Asn1TagType.Context, 3)]
        protected AttributeValueAssertion field3 { get; set; }
        
        [Asn1ChoiceIndex]
        public const long substrings = 4;
        [Asn1ChoiceElement(substrings), Asn1Tag(Asn1TagType.Context, 4)]
        protected SubstringFilter field4 { get; set; }
        
        [Asn1ChoiceIndex]
        public const long greaterOrEqual = 5;
        [Asn1ChoiceElement(greaterOrEqual), Asn1Tag(Asn1TagType.Context, 5)]
        protected AttributeValueAssertion field5 { get; set; }
        
        [Asn1ChoiceIndex]
        public const long lessOrEqual = 6;
        [Asn1ChoiceElement(lessOrEqual), Asn1Tag(Asn1TagType.Context, 6)]
        protected AttributeValueAssertion field6 { get; set; }
        
        [Asn1ChoiceIndex]
        public const long present = 7;
        [Asn1ChoiceElement(present), Asn1Tag(Asn1TagType.Context, 7)]
        protected AttributeType field7 { get; set; }
        
        [Asn1ChoiceIndex]
        public const long approxMatch = 8;
        [Asn1ChoiceElement(approxMatch), Asn1Tag(Asn1TagType.Context, 8)]
        protected AttributeValueAssertion field8 { get; set; }
        
        public Filter()
            : base()
        {
        }
        
        public Filter(long? choiceIndex, Asn1Object obj)
            : base(choiceIndex, obj)
        {
        }
    }
}

