// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Protocols.TestTools.StackSdk.Asn1;

namespace Microsoft.Protocols.TestTools.StackSdk.ActiveDirectory.Adts.Asn1CodecV3
{
    /*
    	AuthenticationChoice ::= CHOICE {
	    simple                 [0]    OCTET STRING,
	    sasl                   [3]    SaslCredentials
	    sicilyPackageDiscovery [9]    OCTET STRING
	    sicilyNegotiate        [10]   OCTET STRING
	    sicilyResponse         [11]   OCTET STRING  }

    */
    public class AuthenticationChoice : Asn1Choice
    {
        [Asn1ChoiceIndex]
        public const long simple = 1;
        [Asn1ChoiceElement(simple), Asn1Tag(Asn1TagType.Context, 0)]
        protected Asn1OctetString field0 { get; set; }
        
        [Asn1ChoiceIndex]
        public const long sasl = 2;
        [Asn1ChoiceElement(sasl), Asn1Tag(Asn1TagType.Context, 3)]
        protected SaslCredentials field1 { get; set; }
        
        [Asn1ChoiceIndex]
        public const long sicilyPackageDiscovery = 3;
        [Asn1ChoiceElement(sicilyPackageDiscovery), Asn1Tag(Asn1TagType.Context, 9)]
        protected Asn1OctetString field2 { get; set; }
        
        [Asn1ChoiceIndex]
        public const long sicilyNegotiate = 4;
        [Asn1ChoiceElement(sicilyNegotiate), Asn1Tag(Asn1TagType.Context, 10)]
        protected Asn1OctetString field3 { get; set; }
        
        [Asn1ChoiceIndex]
        public const long sicilyResponse = 5;
        [Asn1ChoiceElement(sicilyResponse), Asn1Tag(Asn1TagType.Context, 11)]
        protected Asn1OctetString field4 { get; set; }
        
        public AuthenticationChoice()
            : base()
        {
        }
        
        public AuthenticationChoice(long? choiceIndex, Asn1Object obj)
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
                    contextTag = new Asn1Tag(Asn1TagType.Context, 3) { EncodingWay = EncodingWay.Constructed };
                    allLen += TagBerEncode(buffer, contextTag);
                    break;
                case 3:
                    allLen += field2.BerEncodeWithoutUnisersalTag(buffer);
                    contextTag = new Asn1Tag(Asn1TagType.Context, 9) { EncodingWay = EncodingWay.Primitive };
                    allLen += TagBerEncode(buffer, contextTag);
                    break;
                case 4:
                    allLen += field3.BerEncodeWithoutUnisersalTag(buffer);
                    contextTag = new Asn1Tag(Asn1TagType.Context, 10) { EncodingWay = EncodingWay.Primitive };
                    allLen += TagBerEncode(buffer, contextTag);
                    break;
                case 5:
                    allLen += field4.BerEncodeWithoutUnisersalTag(buffer);
                    contextTag = new Asn1Tag(Asn1TagType.Context, 11) { EncodingWay = EncodingWay.Primitive };
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
                    field0 = new Asn1OctetString();
                    allLen += field0.BerDecodeWithoutUnisersalTag(buffer);
                    SetData(1, field0);
                    break;
                case 3:
                    field1 = new SaslCredentials();
                    allLen += field1.BerDecodeWithoutUnisersalTag(buffer);
                    SetData(2, field1);
                    break;
                case 9:
                    field2 = new Asn1OctetString();
                    allLen += field2.BerDecodeWithoutUnisersalTag(buffer);
                    SetData(3, field2);
                    break;
                case 10:
                    field3 = new Asn1OctetString();
                    allLen += field3.BerDecodeWithoutUnisersalTag(buffer);
                    SetData(4, field3);
                    break;
                case 11:
                    field4 = new Asn1OctetString();
                    allLen += field4.BerDecodeWithoutUnisersalTag(buffer);
                    SetData(5, field4);
                    break;
                default:
                    throw new Asn1DecodingUnexpectedData(ExceptionMessages.DecodingUnexpectedData + " AuthenticationChoice");
            }
            return allLen;
        }
    }
}

