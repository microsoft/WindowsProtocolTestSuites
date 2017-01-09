// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Protocols.TestTools.StackSdk.Asn1;

namespace Microsoft.Protocols.TestTools.StackSdk.ActiveDirectory.Adts.Asn1CodecV3
{
    /*
     SearchRequest ::= [APPLICATION 3] SEQUENCE {
                baseObject      LDAPDN,
                scope           SearchRequest_scope,
                derefAliases    SearchRequest_derefAliases,
                sizeLimit       INTEGER (0 .. maxInt),
                timeLimit       INTEGER (0 .. maxInt),
                typesOnly       BOOLEAN,
                filter          Filter,
                attributes      AttributeDescriptionList }
    */
    [Asn1Tag(Asn1TagType.Application, 3)]
    public class SearchRequest : Asn1Sequence
    {
        [Asn1Field(0)]
        public LDAPDN baseObject { get; set; }

        [Asn1Field(1)]
        public SearchRequest_scope scope { get; set; }

        [Asn1Field(2)]
        public SearchRequest_derefAliases derefAliases { get; set; }

        [Asn1IntegerBound(Max = 2147483647L, Min = 0)]
        [Asn1Field(3)]
        public Asn1Integer sizeLimit { get; set; }

        [Asn1IntegerBound(Max = 2147483647L, Min = 0)]
        [Asn1Field(4)]
        public Asn1Integer timeLimit { get; set; }

        [Asn1Field(5)]
        public Asn1Boolean typesOnly { get; set; }

        [Asn1Field(6)]
        public Filter filter { get; set; }

        [Asn1Field(7)]
        public AttributeDescriptionList attributes { get; set; }

        public SearchRequest()
        {
            this.baseObject = null;
            this.scope = null;
            this.derefAliases = null;
            this.sizeLimit = null;
            this.timeLimit = null;
            this.typesOnly = null;
            this.filter = null;
            this.attributes = null;
        }

        public SearchRequest(
         LDAPDN baseObject,
         SearchRequest_scope scope,
         SearchRequest_derefAliases derefAliases,
         Asn1Integer sizeLimit,
         Asn1Integer timeLimit,
         Asn1Boolean typesOnly,
         Filter filter,
         AttributeDescriptionList attributes)
        {
            this.baseObject = baseObject;
            this.scope = scope;
            this.derefAliases = derefAliases;
            this.sizeLimit = sizeLimit;
            this.timeLimit = timeLimit;
            this.typesOnly = typesOnly;
            this.filter = filter;
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

            //Add the encoding result of the top most tag (in most cases it's Application Class Tag) to the front of buffer if it is defined.
            Asn1Tag topTag = this.TopTag;
            if (topTag.TagType != Asn1TagType.Universal)
            {
                resultLen += LengthBerEncode(buffer, resultLen);
                resultLen += TagBerEncode(buffer, topTag);
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
            int returnVal = 0;

            Asn1Tag topTag;
            returnVal += TagBerDecode(buffer, out topTag);

            Asn1Tag topTagInDefinition = this.TopTag;
            if (!topTag.Equals(topTagInDefinition))
            {
                throw new Asn1DecodingUnexpectedData(ExceptionMessages.DecodingUnexpectedData + " Top Most Tag decoding fail.");
            }

            //Decode length
            int lengthAfterUniTag;
            returnVal += LengthBerDecode(buffer, out lengthAfterUniTag);
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

