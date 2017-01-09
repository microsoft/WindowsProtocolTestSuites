// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Protocols.TestTools.StackSdk.Asn1;

namespace Microsoft.Protocols.TestTools.StackSdk.ActiveDirectory.Adts.Asn1CodecV3
{
    /*
    AddRequest ::= [APPLICATION 8] SEQUENCE {
                entry           LDAPDN,
                attributes      AttributeList }
    */
    [Asn1Tag(Asn1TagType.Application, 8)]
    public class AddRequest : Asn1Sequence
    {
        [Asn1Field(0)]
        public LDAPDN entry { get; set; }

        [Asn1Field(1)]
        public AttributeList attributes { get; set; }

        public AddRequest()
        {
            this.entry = null;
            this.attributes = null;
        }

        public AddRequest(
         LDAPDN entry,
         AttributeList attributes)
        {
            this.entry = entry;
            this.attributes = attributes;
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

            //Encoding inversely since buffer is reversed.

            //Add the encoding result of Value to the front of buffer.
            int resultLen = ValueBerEncode(buffer);

            if (explicitTag)
            {
                //Add the encoding result of the top most tag (in most cases it's Application Class Tag) to the front of buffer if it is defined.
                Asn1Tag topTag = this.TopTag;
                if (topTag.TagType != Asn1TagType.Universal)
                {
                    resultLen += LengthBerEncode(buffer, resultLen);
                    resultLen += TagBerEncode(buffer, topTag);
                }
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
            int returnVal = 0, lengthAfterUniTag = 0;
            //Decode the top most tag and universal class tag
            if (explicitTag)
            {
                Asn1Tag topTag;
                returnVal += TagBerDecode(buffer, out topTag);

                Asn1Tag topTagInDefinition = this.TopTag;
                if (!topTag.Equals(topTagInDefinition))
                {
                    throw new Asn1DecodingUnexpectedData(ExceptionMessages.DecodingUnexpectedData + " Top Most Tag decoding fail.");
                }

                //Decode length
                returnVal += LengthBerDecode(buffer, out lengthAfterUniTag);
            }

            //Decode data
            returnVal += ValueBerDecode(buffer, lengthAfterUniTag);

            if (!VerifyConstraints())
            {
                throw new Asn1ConstraintsNotSatisfied(ExceptionMessages.ConstraintsNotSatisfied
                    + " Decode " + this.GetType().Name + ".");
            }

            return returnVal;
        }
    }
}

