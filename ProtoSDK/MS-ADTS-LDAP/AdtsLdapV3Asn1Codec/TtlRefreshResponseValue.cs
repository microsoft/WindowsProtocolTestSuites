// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Protocols.TestTools.StackSdk.Asn1;

namespace Microsoft.Protocols.TestTools.StackSdk.ActiveDirectory.Adts.Asn1CodecV3
{

    public class TtlRefreshResponseValue : Asn1Sequence
    {
        [Asn1Field(0)]
        public Asn1Integer responseTtl { get; set; }

        public TtlRefreshResponseValue()
        {
            responseTtl = null;
        }

        /// <summary>
        /// This constructor sets all elements to references to the 
        /// given objects
        /// </summary>
        public TtlRefreshResponseValue(Asn1Integer responseTtl_)
        {
            responseTtl = responseTtl_;
        }

        /// <summary>
        /// This constructor allows primitive data to be passed for all 
        /// primitive elements.  It will create new object wrappers for 
        /// the primitive data and set other elements to references to 
        /// the given objects 
        /// </summary>
        public TtlRefreshResponseValue(long responseTtl_)
        {
            responseTtl = new Asn1Integer(responseTtl_);
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

            int resultLen = 0;
            resultLen += responseTtl.BerEncodeWithoutUnisersalTag(buffer);
            Asn1Tag valueTag = new Asn1Tag(Asn1TagType.Context, 1) { EncodingWay = EncodingWay.Primitive };
            resultLen += TagBerEncode(buffer, valueTag);

            resultLen += LengthBerEncode(buffer, resultLen);
            //Add the encoding result of the top most tag (in most cases it's Application Class Tag) to the front of buffer if it is defined.
            resultLen += TagBerEncode(buffer, this.TopTag);
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
            int headLen = 0;
            Asn1Tag seqTag;
            headLen += TagBerDecode(buffer, out seqTag);
            int valueLen;
            headLen += LengthBerDecode(buffer, out valueLen);

            // Decode Request Name
            int valueLenDecode = 0;

            // Decode Request Value
            Asn1Tag valueTag;
            valueLenDecode += TagBerDecode(buffer, out valueTag);
            this.responseTtl = new Asn1Integer();
            valueLenDecode += this.responseTtl.BerDecodeWithoutUnisersalTag(buffer);

            if (!VerifyConstraints())
            {
                throw new Asn1ConstraintsNotSatisfied(ExceptionMessages.ConstraintsNotSatisfied
                    + " Decode " + this.GetType().Name + ".");
            }

            return headLen + valueLen;
        }
    }
}
