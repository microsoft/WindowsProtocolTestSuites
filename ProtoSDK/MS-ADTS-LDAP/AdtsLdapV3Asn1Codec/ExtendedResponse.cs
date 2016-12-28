// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Protocols.TestTools.StackSdk.Asn1;

namespace Microsoft.Protocols.TestTools.StackSdk.ActiveDirectory.Adts.Asn1CodecV3
{
    /*
    ExtendedResponse ::= [APPLICATION 24] SEQUENCE {
                COMPONENTS OF LDAPResult,
                responseName     [10] LDAPOID OPTIONAL,
                response         [11] OCTET STRING OPTIONAL }
    */
    [Asn1Tag(Asn1TagType.Application, 24)]
    public class ExtendedResponse : Asn1Sequence
    {
        [Asn1Field(0)]
        public LDAPResult_resultCode resultCode { get; set; }

        [Asn1Field(1)]
        public LDAPDN matchedDN { get; set; }

        [Asn1Field(2)]
        public LDAPString errorMessage { get; set; }

        [Asn1Field(3, Optional = true), Asn1Tag(Asn1TagType.Context, 3)]
        public Referral referral { get; set; }

        [Asn1Field(4, Optional = true), Asn1Tag(Asn1TagType.Context, 10)]
        public LDAPOID responseName { get; set; }

        [Asn1Field(5, Optional = true), Asn1Tag(Asn1TagType.Context, 11)]
        public Asn1OctetString response { get; set; }

        public ExtendedResponse()
        {
            this.resultCode = null;
            this.matchedDN = null;
            this.errorMessage = null;
            this.referral = null;
            this.responseName = null;
            this.response = null;
        }

        public ExtendedResponse(
         LDAPResult_resultCode resultCode,
         LDAPDN matchedDN,
         LDAPString errorMessage,
         Referral referral)
        {
            this.resultCode = resultCode;
            this.matchedDN = matchedDN;
            this.errorMessage = errorMessage;
            this.referral = referral;
            this.responseName = null;
            this.response = null;
        }

        public ExtendedResponse(
         LDAPResult_resultCode resultCode,
         LDAPDN matchedDN,
         LDAPString errorMessage,
         Referral referral,
         LDAPOID responseName,
         Asn1OctetString response)
        {
            this.resultCode = resultCode;
            this.matchedDN = matchedDN;
            this.errorMessage = errorMessage;
            this.referral = referral;
            this.responseName = responseName;
            this.response = response;
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

            if (response != null)
            {
                resultLen += response.BerEncodeWithoutUnisersalTag(buffer);
                valueTag = new Asn1Tag(Asn1TagType.Context, 11) { EncodingWay = EncodingWay.Primitive };
                resultLen += TagBerEncode(buffer, valueTag);
            }

            if (responseName != null)
            {
                resultLen += responseName.BerEncodeWithoutUnisersalTag(buffer);
                valueTag = new Asn1Tag(Asn1TagType.Context, 10) { EncodingWay = EncodingWay.Primitive };
                resultLen += TagBerEncode(buffer, valueTag);
            }

            if (referral != null)
            {
                resultLen += referral.BerEncodeWithoutUnisersalTag(buffer);
                valueTag = new Asn1Tag(Asn1TagType.Context, 3) { EncodingWay = EncodingWay.Constructed };
                resultLen += TagBerEncode(buffer, valueTag);
            }

            resultLen += errorMessage.BerEncode(buffer, true);
            resultLen += matchedDN.BerEncode(buffer, true);
            resultLen += resultCode.BerEncode(buffer, true);

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
            int returnVal = 0, lengthAfterUniTag = 0, tagLength = 0;
            if (explicitTag)
            {
                //Decode the top most tag and universal class tag
                Asn1Tag topTag;
                returnVal += TagBerDecode(buffer, out topTag);
            }

            returnVal += LengthBerDecode(buffer, out lengthAfterUniTag);
            Asn1Tag valueTag;

            //Decode data
            valueTag = new Asn1Tag(Asn1TagType.Universal, 10) { EncodingWay = EncodingWay.Primitive };
            if (IsTagMatch(buffer, valueTag, out tagLength))
            {
                resultCode = new LDAPResult_resultCode();
                returnVal += resultCode.BerDecode(buffer, true);
            }

            valueTag = new Asn1Tag(Asn1TagType.Universal, 4) { EncodingWay = EncodingWay.Primitive };
            if (IsTagMatch(buffer, valueTag, out tagLength))
            {
                matchedDN = new LDAPDN();
                returnVal += matchedDN.BerDecode(buffer, true);
            }

            // decode errorMessage
            valueTag = new Asn1Tag(Asn1TagType.Universal, 4) { EncodingWay = EncodingWay.Primitive };
            if (IsTagMatch(buffer, valueTag, out tagLength))
            {
                errorMessage = new LDAPString();
                returnVal += errorMessage.BerDecode(buffer, true);
            }

            // decode referral
            valueTag = new Asn1Tag(Asn1TagType.Context, 3) { EncodingWay = EncodingWay.Constructed };
            if (IsTagMatch(buffer, valueTag, out tagLength))
            {
                referral = new Referral();
                returnVal += referral.BerDecode(buffer, false);
            }

            // decode responseName
            valueTag = new Asn1Tag(Asn1TagType.Context, 10) { EncodingWay = EncodingWay.Primitive };
            if (IsTagMatch(buffer, valueTag, out tagLength, true))
            {
                responseName = new LDAPOID();
                returnVal += responseName.BerDecode(buffer, false);
            }

            // decode response
            valueTag = new Asn1Tag(Asn1TagType.Context, 11) { EncodingWay = EncodingWay.Primitive };
            if (IsTagMatch(buffer, valueTag, out tagLength, true))
            {
                response = new Asn1OctetString();
                returnVal += response.BerDecode(buffer, false);
            }

            return returnVal;
        }
    }
}

