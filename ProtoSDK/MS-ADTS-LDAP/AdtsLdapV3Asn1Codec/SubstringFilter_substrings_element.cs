// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Protocols.TestTools.StackSdk.Asn1;

namespace Microsoft.Protocols.TestTools.StackSdk.ActiveDirectory.Adts.Asn1CodecV3
{
    /*
    	SubstringFilter_substrings_element ::= CHOICE {
                        initial [0] LDAPString,
                        any     [1] LDAPString,
                        final   [2] LDAPString }
    */
    public class SubstringFilter_substrings_element : Asn1Choice
    {
        [Asn1ChoiceIndex]
        public const long initial = 1;
        [Asn1ChoiceElement(initial), Asn1Tag(Asn1TagType.Context, 0)]
        protected LDAPString field0 { get; set; }
        
        [Asn1ChoiceIndex]
        public const long any = 2;
        [Asn1ChoiceElement(any), Asn1Tag(Asn1TagType.Context, 1)]
        protected LDAPString field1 { get; set; }
        
        [Asn1ChoiceIndex]
        public const long final = 3;
        [Asn1ChoiceElement(final), Asn1Tag(Asn1TagType.Context, 2)]
        protected LDAPString field2 { get; set; }
        
        public SubstringFilter_substrings_element()
            : base()
        {
        }
        
        public SubstringFilter_substrings_element(long? choiceIndex, Asn1Object obj)
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
                    allLen += field0.BerEncodeWithoutUnisersalTag(buffer);
                    contextTag = new Asn1Tag(Asn1TagType.Context, 0) { EncodingWay = EncodingWay.Primitive };
                    allLen += TagBerEncode(buffer, contextTag);
                    break;
                case 2:
                    allLen += field1.BerEncodeWithoutUnisersalTag(buffer);
                    contextTag = new Asn1Tag(Asn1TagType.Context, 1) { EncodingWay = EncodingWay.Primitive };
                    allLen += TagBerEncode(buffer, contextTag);
                    break;
                case 3:
                    allLen += field2.BerEncodeWithoutUnisersalTag(buffer);
                    contextTag = new Asn1Tag(Asn1TagType.Context, 2) { EncodingWay = EncodingWay.Primitive };
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
            switch (contextTag.TagValue)
            {
                case 0:
                    field0 = new LDAPString();
                    allLen += field0.BerDecodeWithoutUnisersalTag(buffer);
                    SetData(1, field0);
                    break;
                case 1:
                    field1 = new LDAPString();
                    allLen += field1.BerDecodeWithoutUnisersalTag(buffer);
                    SetData(2, field1);
                    break;
                case 2:
                    field2 = new LDAPString();
                    allLen += field2.BerDecodeWithoutUnisersalTag(buffer);
                    SetData(3, field2);
                    break;
                default:
                    throw new Asn1DecodingUnexpectedData(ExceptionMessages.DecodingUnexpectedData + " AuthenticationChoice");
            }
            return allLen;
        }
    }
}

