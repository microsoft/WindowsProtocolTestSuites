// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Text;

namespace Microsoft.Protocols.TestTools.StackSdk.Asn1
{
    /// <summary>
    /// Represents a BMPString in ASN.1 Definition
    /// </summary>
    [Asn1Tag(Asn1TagType.Universal, Asn1TagValue.BmpString)]
    public class Asn1BmpString : Asn1String
    {
        /// <summary>
        /// Initializes a new instance of the Asn1BmpString class with an empty string.
        /// </summary>
        public Asn1BmpString()
        {
            this.Value = String.Empty;
        }

        /// <summary>
        /// Initializes a new instance of the Asn1BmpString class with a given string.
        /// </summary>
        /// <param name="s"></param>
        public Asn1BmpString(string s)
        {
            this.Value = s;
        }

        //Constraint is not needed for primitive type UniversalString.
        //Therefore no need to override VerifyConstraints method.

        #region BER

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
            if (this.Value == null)
            {
                throw new Asn1EmptyDataException(ExceptionMessages.EmptyData);
            }
            byte[] result = Encoding.BigEndianUnicode.GetBytes(this.Value);
            buffer.WriteBytes(result);
            return result.Length;
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
            byte[] result = buffer.ReadBytes(length);
            this.Value = Encoding.BigEndianUnicode.GetString(result);
            return length;
        }

        #endregion BER

        #region PER

        /// <summary>
        /// Encodes the content of the object by PER.
        /// </summary>
        /// <param name="buffer">A buffer to which the encoding result will be written.</param>
        /// <remarks>Length is included.</remarks>
        protected override void ValuePerEncode(IAsn1PerEncodingBuffer buffer)
        {
            Asn1StandardProcedure.PerEncodeArray(buffer, Value.ToCharArray(),
                (encodingBuffer, b) =>
                {
                    byte[] result = Encoding.BigEndianUnicode.GetBytes("" + b);
                    encodingBuffer.WriteBytes(result);
                }
            , Constraint != null && Constraint.HasMinSize ? Constraint.MinSize : (long?)null, Constraint != null && Constraint.HasMaxSize ? Constraint.MaxSize : (long?)null,
            true);
        }

        /// <summary>
        /// Decodes the object by PER.
        /// </summary>
        /// <param name="buffer">A buffer that contains a PER encoding result.</param>
        /// <param name="aligned">Indicating whether the PER decoding is aligned.</param>
        /// <remarks>Length is included.</remarks>
        protected override void ValuePerDecode(IAsn1DecodingBuffer buffer, bool aligned = true)
        {
            char[] result = Asn1StandardProcedure.PerDecodeArray<char>(buffer,
                decodingBuffer =>
                {
                    byte[] encodingResult = buffer.ReadBytes(2);
                    string s = Encoding.BigEndianUnicode.GetString(encodingResult);
                    return s[0];
                },
            Constraint != null && Constraint.HasMinSize ? Constraint.MinSize : (long?)null, Constraint != null && Constraint.HasMaxSize ? Constraint.MaxSize : (long?)null,
            true);
            Value = new string(result);
        }

        #endregion PER
    }
}

