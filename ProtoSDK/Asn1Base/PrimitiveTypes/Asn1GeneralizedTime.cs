// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;

namespace Microsoft.Protocols.TestTools.StackSdk.Asn1
{
    /// <summary>
    /// Represents a GeneralizedTime in ASN.1 Definition
    /// </summary>
    /// <remarks>
    /// Ref. X.680:46.3: GeneralizedTime ::= [UNIVERSAL 24] IMPLICIT VisibleString
    /// </remarks>
    [Asn1Tag(Asn1TagType.Universal, Asn1TagValue.GeneralizedTime)]
    public class Asn1GeneralizedTime : Asn1ByteString
    {
        /// <summary>
        /// Initializes a new instance of the Asn1GeneralizedTime class that has the default data.
        /// </summary>
        public Asn1GeneralizedTime()
        {
            this.Value = String.Empty;
        }

        /// <summary>
        /// Initializes a new instance of the Asn1GeneralizedTime class with the given value.
        /// </summary>
        /// <param name="s"></param>
        public Asn1GeneralizedTime(string s)
        {
            this.Value = s;
        }

        /// <summary>
        /// Checks whether the data in the ASN.1 object meets the constraints.
        /// </summary>
        /// <returns>True if constraints are met. False if not.</returns>
        /// <remarks>
        /// Constraints include user defined and ASN.1 defined constraints.
        /// If there are constraints in the derived classes, this method should be overrode.
        /// </remarks>
        protected override bool VerifyConstraints()
        {
            //Ref. X.680: 46
            //Constraints may be verified by using REGEX, seems complicated.
            return base.VerifyConstraints();
        }

        /// <summary>
        /// Encodes the data of this object to the buffer.
        /// </summary>
        /// <param name="buffer">A buffer to which the encoding result will be written.</param>
        /// <returns>The length of the encoding result of the data.</returns>
        /// <remarks>
        /// For the TLV (Tag, Length, Value) triple in the encoding result,
        /// this method provides the functionality of encoding Value.
        /// The encoding for Tag and Length will be done in Asn1Object::BerEncode method.
        /// </remarks>
        protected override int ValueBerEncode(IAsn1BerEncodingBuffer buffer)
        {
            //Currently this works good only if there is no fractional portions in Value.
            //In kerberos test suite, the time shall not include any fractional portions of the seconds. Ref. RFC4120: 5.2.3.
            return base.ValueBerEncode(buffer);
        }

        /// <summary>
        /// Decodes the data from the buffer and stores the data in this object.
        /// </summary>
        /// <param name="buffer">A buffer that stores a BER encoding result.</param>
        /// <param name="length">The length of the encoding result of the data in the given buffer.</param>
        /// <returns>The number of the bytes consumed in the buffer to decode the data.</returns>
        /// <remarks>
        /// For the TLV (Tag, Length, Value) triple in the encoding result, 
        /// this method provides the functionality of decoding Value.
        /// The decoding for Tag and Length will be done in Asn1Object::BerDecode method.
        /// </remarks>
        protected override int ValueBerDecode(IAsn1DecodingBuffer buffer, int length)
        {
            //Check X.690: 11.7 for more decoding details.
            return base.ValueBerDecode(buffer, length);
        }

        //PER pending
    }
}
