// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Microsoft.Protocols.TestTools.StackSdk.Asn1
{
    /// <summary>
    /// Contains some common encoding/decoding behaviors for specific strings.
    /// </summary>
    /// <remarks>
    /// This class is the base class of OCTET STRING, GeneralString and NumericString since they have same encoding/decoding behavior.
    /// Same behavior in BER: the encoding result of each char in the string is stored in one byte.
    /// </remarks>
    public abstract class Asn1ByteString : Asn1String
    {
        /// <summary>
        /// Gets or sets the data of the object in string form.
        /// </summary>
        public override string Value
        {
            get
            {
                return ConvertByteArrayToString(ByteArrayValue);
            }
            set
            {
                ByteArrayValue = ConvertStringToByteArray(value);
            }
        }

        /// <summary>
        /// Gets or sets the data of the object in byte array form.
        /// </summary>
        public byte[] ByteArrayValue { get; set; }

        /// <summary>
        /// Gets the built in charset of the string type, in string form or regex form.
        /// </summary>
        protected override string TypeBuiltInCharSet
        {
            get { return @"\x00-\xFF"; }
        }

        #region overrode methods from System.Object

        /// <summary>
        /// Overrode method from System.Object.
        /// </summary>
        /// <returns>A string that represents this instance.</returns>
        public override string ToString()
        {
            return Value;
        }

        #endregion overrode methods from System.Object

        #region BER

        /// <summary>
        /// Coverts a string to a byte array.
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        private static byte[] ConvertStringToByteArray(string s)
        {
            if (s == null)
            {
                return null;
            }
            byte[] result = new byte[s.Length];
            for (int i = 0; i < s.Length; i++)
            {
                result[i] = (byte)s[i];
            }
            return result;
        }

        /// <summary>
        /// Coverts a byte array to a string.
        /// </summary>
        /// <param name="bytes"></param>
        /// <returns></returns>
        private static string ConvertByteArrayToString(byte[] bytes)
        {
            if (bytes == null)
            {
                return null;
            }
            char[] s = new char[bytes.Length];
            for (int i = 0; i < bytes.Length; i++)
            {
                s[i] = (char)bytes[i];
            }
            return new string(s);
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
            buffer.WriteBytes(ByteArrayValue);
            return ByteArrayValue == null ? 0 : ByteArrayValue.Length;
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
            ByteArrayValue = result;
            return length;
        }

        #endregion BER

        #region PER

        #endregion PER
    }
}
