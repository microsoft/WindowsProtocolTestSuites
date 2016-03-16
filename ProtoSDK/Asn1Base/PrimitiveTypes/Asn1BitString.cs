// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Linq;

namespace Microsoft.Protocols.TestTools.StackSdk.Asn1
{
    /// <summary>
    /// Represents a BIT STRING in ASN.1 Definition.
    /// </summary>
    [Asn1Tag(Asn1TagType.Universal, Asn1TagValue.BitString)]
    public class Asn1BitString : Asn1Object
    {
        /// <summary>
        /// Stores the data of the object.
        /// </summary>
        private byte[] data;

        /// <summary>
        /// Indicates the number of the bits that are not used in the last byte in data.
        /// </summary>
        private byte unusedBitsInLastByte;

        /// <summary>
        /// Gets or sets the data of the object in byte array form.
        /// </summary>
        public byte[] ByteArrayValue
        {
            get
            {
                return data;
            }
            set
            {
                data = value;
                unusedBitsInLastByte = 0;
            }
        }

        /// <summary>
        /// Returns the number of bits in the data of the object.
        /// </summary>
        public int Length
        {
            get { return 8 * data.Length - unusedBitsInLastByte; }
        }

        /// <summary>
        /// Initializes a new instance of the Asn1BitString class with empty data.
        /// </summary>
        public Asn1BitString()
        {
            this.ByteArrayValue = null;
        }

        /// <summary>
        /// Initializes a new instance of the Asn1BitString class with a given string.
        /// </summary>
        /// <remarks>
        /// Notation(format) for the strings: Ref. X680:22 and G.2.5. 
        /// Bstrings and hstrings: X.680: 12.10 and 12.12.
        /// </remarks>
        public Asn1BitString(string s)
        {
            List<byte> list = new List<byte>();
            if (s.Length >= 3 && s[0] == '\'' && s[s.Length - 2] == '\'')
            {
                //Legal
                if (s[s.Length - 1] == 'H')
                {
                    //Hex string: 
                    //example: "'0fa56920014abc'H"
                    //Two chars in the string form a byte. In the above example,
                    //0f->first byte: 0*16+15
                    //a5->second byte: 10*16+5
                    //....
                    //If the number of the chars between two ' is odd,
                    //The last 4 bits in last byte will not be used.
                    //Hex string: Ref. X.690:12.12
                    //Unusedbits: Ref. X.690:8.6.2.2
                    int index = 1;
                    while (index <= s.Length - 3)
                    {
                        byte val = (byte)(Convert.ToByte("" + s[index++], 16) << 4);
                        if (index <= s.Length - 3)
                        {
                            val += Convert.ToByte("" + s[index++], 16);
                        }
                        else
                        {
                            unusedBitsInLastByte = 4;
                        }
                        list.Add(val);
                    }
                    data = list.ToArray();
                }
                else if (s[s.Length - 1] == 'B')
                {
                    //Binary string:    "'11010010111001'B"
                    throw new NotImplementedException("BIT STRING constructor with Binary string is not implemented yet.");
                    //Ref. X.680:22
                }
                else
                {
                    throw new Asn1InvalidArgument("Parameter for BIT STRING is illegal.");
                }
            }
            else
            {
                throw new NotImplementedException("BIT STRING constructor with char string is not implemented yet.");
            }
        }

        /// <summary>
        /// Initializes a new instance of the Asn1BitString class with a given byte array.
        /// </summary>
        /// <param name="bytes">The given byte to initial the BIT STRING.</param>
        public Asn1BitString(byte[] bytes)
        {
            this.ByteArrayValue = bytes;
        }

        #region overrode methods from System.Object

        /// <summary>
        /// Overrode method from System.Object.
        /// </summary>
        /// <param name="obj">The object to be compared.</param>
        /// <returns>True if obj has same data with this instance. False if not.</returns>
        public override bool Equals(System.Object obj)
        {
            // If parameter is null return false.
            if (obj == null)
            {
                return false;
            }

            // If parameter cannot be cast to Asn1String return false.
            Asn1BitString p = obj as Asn1BitString;
            if ((System.Object)p == null)
            {
                return false;
            }

            // Return true if the fields match.
            return ByteArrayValue.SequenceEqual<byte>(p.ByteArrayValue) &&
                this.unusedBitsInLastByte == p.unusedBitsInLastByte;
        }

        /// <summary>
        /// Returns the hash code of the instance.
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            throw new NotImplementedException();
        }

        #endregion overrode methods from System.Object

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
            //Ref. X.680: 8.6.2.2
            buffer.WriteBytes(ByteArrayValue);
            buffer.WriteByte(unusedBitsInLastByte);
            return ByteArrayValue.Length + 1;
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
            this.unusedBitsInLastByte = buffer.ReadByte();
            this.data = buffer.ReadBytes(length - 1);
            return length;
        }

        #endregion BER

        #region PER

        #endregion PER
    }
}

