// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Protocols.TestTools.StackSdk.Asn1;

namespace Microsoft.Protocols.TestTools.StackSdk.ActiveDirectory.Adts.Asn1CodecV3
{
    /*
    ModifyDNRequest ::= [APPLICATION 12] SEQUENCE {
                entry           LDAPDN,
                newrdn          RelativeLDAPDN,
                deleteoldrdn    BOOLEAN,
                newSuperior     [0] LDAPDN OPTIONAL }
    */
    [Asn1Tag(Asn1TagType.Application, 12, EncodingWay = EncodingWay.Constructed)]
    public class ModifyDNRequest : Asn1Sequence
    {
        [Asn1Field(0)]
        public LDAPDN entry { get; set; }

        [Asn1Field(1)]
        public RelativeLDAPDN newrdn { get; set; }

        [Asn1Field(2)]
        public Asn1Boolean deleteoldrdn { get; set; }

        [Asn1Field(3, Optional = true), Asn1Tag(Asn1TagType.Context, 0)]
        public LDAPDN newSuperior { get; set; }

        public ModifyDNRequest()
        {
            this.entry = null;
            this.newrdn = null;
            this.deleteoldrdn = null;
            this.newSuperior = null;
        }

        public ModifyDNRequest(
         LDAPDN entry,
         RelativeLDAPDN newrdn,
         Asn1Boolean deleteoldrdn,
         LDAPDN newSuperior)
        {
            this.entry = entry;
            this.newrdn = newrdn;
            this.deleteoldrdn = deleteoldrdn;
            this.newSuperior = newSuperior;
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

            // Encode newSuperior
            int resultLen = 0;
            if (newSuperior != null)
            {
                resultLen = newSuperior.BerEncodeWithoutUnisersalTag(buffer);
                resultLen += TagBerEncode(buffer, new Asn1Tag(Asn1TagType.Context, 0) { EncodingWay = EncodingWay.Primitive });
            }

            // Encode deleteoldrdn
            resultLen += deleteoldrdn.BerEncode(buffer, true);

            // Encode newrdn
            resultLen += newrdn.BerEncode(buffer, true);

            // Encode entry
            resultLen += entry.BerEncode(buffer, true);

            // Encode Length
            resultLen += LengthBerEncode(buffer, resultLen);

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
            //Decode the top most tag and universal class tag
            int headLen = 0;
            Asn1Tag seqTag;
            headLen += TagBerDecode(buffer, out seqTag);
            int valueLen;
            headLen += LengthBerDecode(buffer, out valueLen);

            int valueLenDecode = 0;
            this.entry = new LDAPDN();
            valueLenDecode += this.entry.BerDecode(buffer, true);

            this.newrdn = new RelativeLDAPDN();
            valueLenDecode += this.newrdn.BerDecode(buffer, true);

            this.deleteoldrdn = new Asn1Boolean();
            valueLenDecode += this.deleteoldrdn.BerDecode(buffer, true);

            if(valueLenDecode == valueLen)
            {
                newSuperior = null;
            }
            else
            {
                Asn1Tag contextTag;
                valueLenDecode += TagBerDecode(buffer, out contextTag);
                newSuperior = new LDAPDN();
                valueLenDecode += newSuperior.BerDecodeWithoutUnisersalTag(buffer);
            }

            if (valueLen != valueLenDecode)
            {
                throw new Asn1DecodingUnexpectedData(ExceptionMessages.DecodingUnexpectedData + " ModifyDNRequest.");
            }
            return headLen + valueLen;
        }
    }
}

