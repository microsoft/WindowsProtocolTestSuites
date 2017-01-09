// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Protocols.TestTools.StackSdk.Asn1;

namespace Microsoft.Protocols.TestTools.StackSdk.ActiveDirectory.Adts.Asn1CodecV3
{
    /*
    Filter ::= CHOICE {
                and             [0] SET OF Filter,
                or              [1] SET OF Filter,
                not             [2] Filter,
                equalityMatch   [3] AttributeValueAssertion,
                substrings      [4] SubstringFilter,
                greaterOrEqual  [5] AttributeValueAssertion,
                lessOrEqual     [6] AttributeValueAssertion,
                present         [7] AttributeDescription,
                approxMatch     [8] AttributeValueAssertion,
                extensibleMatch [9] MatchingRuleAssertion }
    */
    public class Filter : Asn1Choice
    {
        [Asn1ChoiceIndex]
        public const long and = 1;
        [Asn1ChoiceElement(and), Asn1Tag(Asn1TagType.Context, 0)]
        protected Asn1SetOf<Filter> field0 { get; set; }

        [Asn1ChoiceIndex]
        public const long or = 2;
        [Asn1ChoiceElement(or), Asn1Tag(Asn1TagType.Context, 1)]
        protected Asn1SetOf<Filter> field1 { get; set; }

        [Asn1ChoiceIndex]
        public const long not = 3;
        [Asn1ChoiceElement(not), Asn1Tag(Asn1TagType.Context, 2)]
        protected Filter field2 { get; set; }

        [Asn1ChoiceIndex]
        public const long equalityMatch = 4;
        [Asn1ChoiceElement(equalityMatch), Asn1Tag(Asn1TagType.Context, 3)]
        protected AttributeValueAssertion field3 { get; set; }

        [Asn1ChoiceIndex]
        public const long substrings = 5;
        [Asn1ChoiceElement(substrings), Asn1Tag(Asn1TagType.Context, 4)]
        protected SubstringFilter field4 { get; set; }

        [Asn1ChoiceIndex]
        public const long greaterOrEqual = 6;
        [Asn1ChoiceElement(greaterOrEqual), Asn1Tag(Asn1TagType.Context, 5)]
        protected AttributeValueAssertion field5 { get; set; }

        [Asn1ChoiceIndex]
        public const long lessOrEqual = 7;
        [Asn1ChoiceElement(lessOrEqual), Asn1Tag(Asn1TagType.Context, 6)]
        protected AttributeValueAssertion field6 { get; set; }

        [Asn1ChoiceIndex]
        public const long present = 8;
        [Asn1ChoiceElement(present), Asn1Tag(Asn1TagType.Context, 7)]
        protected AttributeDescription field7 { get; set; }

        [Asn1ChoiceIndex]
        public const long approxMatch = 9;
        [Asn1ChoiceElement(approxMatch), Asn1Tag(Asn1TagType.Context, 8)]
        protected AttributeValueAssertion field8 { get; set; }

        [Asn1ChoiceIndex]
        public const long extensibleMatch = 10;
        [Asn1ChoiceElement(extensibleMatch), Asn1Tag(Asn1TagType.Context, 9)]
        protected MatchingRuleAssertion field9 { get; set; }

        public Filter()
            : base()
        {
        }

        public Filter(long? choiceIndex, Asn1Object obj)
            : base(choiceIndex, obj)
        {
        }

        public override int BerEncode(IAsn1BerEncodingBuffer buffer, bool explicitTag = true)
        {
            int allLen = 0;
            Asn1Tag contextTag;
            switch (SelectedChoice)
            {
                case 1:
                    allLen += field0.BerEncode(buffer, false);
                    allLen += LengthBerEncode(buffer, allLen);
                    contextTag = new Asn1Tag(Asn1TagType.Context, 0) { EncodingWay = EncodingWay.Constructed };
                    allLen += TagBerEncode(buffer, contextTag);
                    break;
                case 2:
                    allLen += field1.BerEncode(buffer, false);
                    allLen += LengthBerEncode(buffer, allLen);
                    contextTag = new Asn1Tag(Asn1TagType.Context, 1) { EncodingWay = EncodingWay.Constructed };
                    allLen += TagBerEncode(buffer, contextTag);
                    break;
                case 3:
                    allLen += field2.BerEncode(buffer, false);
                    allLen += LengthBerEncode(buffer, allLen);
                    contextTag = new Asn1Tag(Asn1TagType.Context, 2) { EncodingWay = EncodingWay.Constructed };
                    allLen += TagBerEncode(buffer, contextTag);
                    break;
                case 4:
                    allLen += field3.BerEncode(buffer, false);
                    allLen += LengthBerEncode(buffer, allLen);
                    contextTag = new Asn1Tag(Asn1TagType.Context, 3) { EncodingWay = EncodingWay.Constructed };
                    allLen += TagBerEncode(buffer, contextTag);
                    break;
                case 5:
                    allLen += field4.BerEncode(buffer, false);
                    allLen += LengthBerEncode(buffer, allLen);
                    contextTag = new Asn1Tag(Asn1TagType.Context, 4) { EncodingWay = EncodingWay.Constructed };
                    allLen += TagBerEncode(buffer, contextTag);
                    break;
                case 6:
                    allLen += field5.BerEncode(buffer, false);
                    allLen += LengthBerEncode(buffer, allLen);
                    contextTag = new Asn1Tag(Asn1TagType.Context, 5) { EncodingWay = EncodingWay.Constructed };
                    allLen += TagBerEncode(buffer, contextTag);
                    break;
                case 7:
                    allLen += field6.BerEncode(buffer, false);
                    allLen += LengthBerEncode(buffer, allLen);
                    contextTag = new Asn1Tag(Asn1TagType.Context, 6) { EncodingWay = EncodingWay.Constructed };
                    allLen += TagBerEncode(buffer, contextTag);
                    break;
                case 8:
                    allLen += field7.BerEncode(buffer, false);
                    allLen += LengthBerEncode(buffer, allLen);
                    contextTag = new Asn1Tag(Asn1TagType.Context, 7) { EncodingWay = EncodingWay.Primitive };
                    allLen += TagBerEncode(buffer, contextTag);
                    break;
                case 9:
                    allLen += field8.BerEncode(buffer, false);
                    allLen += LengthBerEncode(buffer, allLen);
                    contextTag = new Asn1Tag(Asn1TagType.Context, 8) { EncodingWay = EncodingWay.Constructed };
                    allLen += TagBerEncode(buffer, contextTag);
                    break;
                case 10:
                    allLen += field9.BerEncode(buffer, false);
                    allLen += LengthBerEncode(buffer, allLen);
                    contextTag = new Asn1Tag(Asn1TagType.Context, 9) { EncodingWay = EncodingWay.Constructed };
                    allLen += TagBerEncode(buffer, contextTag);
                    break;
                default:
                    throw new Asn1ConstraintsNotSatisfied(ExceptionMessages.InvalidChoiceIndex + " AuthenticationChoice");
            }
            return allLen;
        }

        public override int BerDecode(IAsn1DecodingBuffer buffer, bool explicitTag = true)
        {
            int allLen = 0;
            Asn1Tag contextTag;
            allLen += TagBerDecode(buffer, out contextTag);

            int valueLen;
            allLen += LengthBerDecode(buffer, out valueLen);

            switch (contextTag.TagValue)
            {
                case 0:
                    field0 = new Asn1SetOf<Filter>();
                    allLen += field0.BerDecode(buffer, false);
                    SetData(1, field0);
                    break;
                case 1:
                    field1 = new Asn1SetOf<Filter>();
                    allLen += field1.BerDecode(buffer, false);
                    SetData(2, field1);
                    break;
                case 2:
                    field2 = new Filter();
                    allLen += field2.BerDecode(buffer, false);
                    SetData(3, field2);
                    break;
                case 3:
                    field3 = new AttributeValueAssertion();
                    allLen += field3.BerDecode(buffer, false);
                    SetData(4, field3);
                    break;
                case 4:
                    field4 = new SubstringFilter();
                    allLen += field4.BerDecode(buffer, false);
                    SetData(5, field4);
                    break;
                case 5:
                    field5 = new AttributeValueAssertion();
                    allLen += field5.BerDecode(buffer, false);
                    SetData(6, field5);
                    break;
                case 6:
                    field6 = new AttributeValueAssertion();
                    allLen += field6.BerDecode(buffer, false);
                    SetData(7, field6);
                    break;
                case 7:
                    field7 = new AttributeDescription();
                    allLen += field7.BerDecode(buffer, false);
                    SetData(8, field7);
                    break;
                case 8:
                    field8 = new AttributeValueAssertion();
                    allLen += field8.BerDecode(buffer, false);
                    SetData(9, field8);
                    break;
                case 9:
                    field9 = new MatchingRuleAssertion();
                    allLen += field9.BerDecode(buffer, false);
                    SetData(10, field9);
                    break;
                default:
                    throw new Asn1DecodingUnexpectedData(ExceptionMessages.DecodingUnexpectedData + " AuthenticationChoice");
            }
            return allLen;
        }
    }
}

