// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Text;
using System;

namespace Microsoft.Protocols.TestTools.StackSdk.Asn1
{
    /// <summary>
    /// Represents an UniversalString in ASN.1 Definition
    /// </summary>
    [Asn1Tag(Asn1TagType.Universal, Asn1TagValue.UniversalString)]
    public class Asn1UniversalString : Asn1String
    {
        /// <summary>
        /// Initializes a new instance of the Asn1UniversalString class with an empty string.
        /// </summary>
        public Asn1UniversalString()
        {
            this.Value = String.Empty;
        }

        /// <summary>
        /// Initializes a new instance of the Asn1UniversalString class with a given string.
        /// </summary>
        /// <param name="s"></param>
        public Asn1UniversalString(string s)
        {
            this.Value = s;
        }

        //Constraint is not needed for primitive type UniversalString.
        //Therefore no need to override VerifyConstraints method.

        #region Encode & Decode Functionality

        /// <summary>
        /// Provides encoding functionality in UniversalString way for a string.
        /// </summary>
        /// <param name="str"></param>
        /// <returns>The encoding result, in byte array form, big endian.</returns>
        private static byte[] UniversalStringEncode(string str)
        {
            UTF32Encoding be = new UTF32Encoding(true, false);
            return be.GetBytes(str);
        }

        /// <summary>
        /// Provides decoding functionality in UniversalString way for a string.
        /// </summary>
        /// <param name="bytes">A BER encoding result of UniversalString</param>
        /// <returns>The decoding result.</returns>
        private static string UniversalStringDecode(byte[] bytes)
        {
            if (bytes == null || bytes.Length % 4 != 0)
            {
                throw new Asn1DecodingUnexpectedData(ExceptionMessages.DecodingUnexpectedData);
            }
            UTF32Encoding be = new UTF32Encoding(true, false);
            return be.GetString(bytes);
        }

        #endregion Encode & Decode Functionality

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
            byte[] result = UniversalStringEncode(this.Value);
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
            this.Value = UniversalStringDecode(result);
            return length;
        }


        #endregion BER

        #region PER

        #endregion PER
    }
}

