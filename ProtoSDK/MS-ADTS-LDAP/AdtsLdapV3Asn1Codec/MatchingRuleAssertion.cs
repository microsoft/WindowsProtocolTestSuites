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
            this.dnAttributes = new Asn1Boolean(false);
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

        /// <summary>
        /// Encodes the object by BER.
        /// </summary>
        /// <param name="buffer">A buffer that stores the BER encoding result.</param>
        /// <param name="explicitTag">Indicates whether the tags should be encoded explicitly. In our Test Suites, it will always be true.</param>
        /// <returns>The length of the encoding result of this object.</returns>
        /// <exception cref="Asn1ConstraintsNotSatisfied">
        /// Thrown when the constraints are not satisfied before encoding.
        /// </exception>
        /// <remarks>Override this method in a user-defined class only if the procedure is not applicable in some special scenarios.</remarks>
        public override int BerEncode(IAsn1BerEncodingBuffer buffer, bool explicitTag = true)
        {
            if (!VerifyConstraints())
            {
                throw new Asn1ConstraintsNotSatisfied(ExceptionMessages.ConstraintsNotSatisfied
                    + " Encode " + this.GetType().Name + ".");
            }
            Asn1Tag valueTag;
            int resultLen = 0;

            resultLen += dnAttributes.BerEncodeWithoutUnisersalTag(buffer);
            valueTag = new Asn1Tag(Asn1TagType.Context, 4) { EncodingWay = EncodingWay.Primitive };
            resultLen += TagBerEncode(buffer, valueTag);

            resultLen += matchValue.BerEncodeWithoutUnisersalTag(buffer);
            valueTag = new Asn1Tag(Asn1TagType.Context, 3) { EncodingWay = EncodingWay.Primitive };
            resultLen += TagBerEncode(buffer, valueTag);

            if (type != null)
            {
                resultLen += type.BerEncodeWithoutUnisersalTag(buffer);
                valueTag = new Asn1Tag(Asn1TagType.Context, 2) { EncodingWay = EncodingWay.Primitive };
                resultLen += TagBerEncode(buffer, valueTag);
            }

            if (matchingRule != null)
            {
                resultLen += matchingRule.BerEncodeWithoutUnisersalTag(buffer);
                valueTag = new Asn1Tag(Asn1TagType.Context, 1) { EncodingWay = EncodingWay.Primitive };
                resultLen += TagBerEncode(buffer, valueTag);
            }

            if (explicitTag)
            {
                resultLen += LengthBerEncode(buffer, resultLen);
                //Add the encoding result of the top most tag (in most cases it's Application Class Tag) to the front of buffer if it is defined.
                resultLen += TagBerEncode(buffer, this.TopTag);
            }

            return resultLen;
        }

        /// <summary>
        /// Decodes the object by BER.
        /// </summary>
        /// <param name="buffer">A buffer that contains a BER encoding result.</param>
        /// <param name="explicitTag">Indicates whether the tags should be encoded explicitly. In our Test Suites, it will always be true.</param>
        /// <returns>The number of the bytes consumed in the buffer to decode this object.</returns>
        /// <exception cref="Asn1ConstraintsNotSatisfied">
        /// Thrown when the constraints are not satisfied after decoding.
        /// </exception>
        /// <exception cref="Asn1DecodingUnexpectedData">
        /// Thrown when the data in the buffer can not be properly decoded.
        /// </exception>
        /// <remarks>Override this method in a user-defined class only if the procedure is not applicable in some special scenarios.</remarks>
        public override int BerDecode(IAsn1DecodingBuffer buffer, bool explicitTag = true)
        {
            int headLen = 0, valueLen = 0;

            if (explicitTag)
            {
                Asn1Tag seqTag;
                headLen += TagBerDecode(buffer, out seqTag);
                headLen += LengthBerDecode(buffer, out valueLen);
            }

            int valueLenDecode = 0;
            Asn1Tag valueTag;

            // Decode referral
            valueLenDecode += TagBerDecode(buffer, out valueTag);
            this.matchingRule = new MatchingRuleId();
            valueLenDecode += this.matchingRule.BerDecodeWithoutUnisersalTag(buffer);

            // Decode type
            valueLenDecode += TagBerDecode(buffer, out valueTag);
            this.type = new AttributeDescription();
            valueLenDecode += this.type.BerDecodeWithoutUnisersalTag(buffer);

            // Decode matchValue
            valueLenDecode += TagBerDecode(buffer, out valueTag);
            this.matchValue = new AssertionValue();
            valueLenDecode += this.matchValue.BerDecodeWithoutUnisersalTag(buffer);

            // Decode dnAttributes
            valueLenDecode += TagBerDecode(buffer, out valueTag);
            this.dnAttributes = new Asn1Boolean();
            valueLenDecode += this.dnAttributes.BerDecodeWithoutUnisersalTag(buffer);

            if (!VerifyConstraints())
            {
                throw new Asn1ConstraintsNotSatisfied(ExceptionMessages.ConstraintsNotSatisfied
                    + " Decode " + this.GetType().Name + ".");
            }

            return headLen + valueLen;
        }
    }
}

