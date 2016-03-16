// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Linq;

namespace Microsoft.Protocols.TestTools.StackSdk.Asn1
{
    /// <summary>
    /// Represents an Ojbect Identifier in ASN.1 Definition
    /// </summary>
    /// <remarks>
    /// Ref. X.680: 3.8.54, X.660
    /// </remarks>
    [Asn1Tag(Asn1TagType.Universal, Asn1TagValue.ObjectIdentifier)]
    public class Asn1ObjectIdentifier : Asn1Object
    {
        /// <summary>
        /// Gets or sets the data of the object.
        /// </summary>
        public int[] Value { get; set; }

        /// <summary>
        /// Initializes a new instance of the Asn1ObjectIdentifier class with empty data.
        /// </summary>
        public Asn1ObjectIdentifier()
        {
            Value = null;
        }

        /// <summary>
        /// Initializes a new instance of the Asn1ObjectIdentifier class with a given value.
        /// </summary>
        /// <param name="val"></param>
        public Asn1ObjectIdentifier(int[] val)
        {
            Value = val;
        }

        /// <summary>
        /// Checks the constraints for Object Identifier.
        /// </summary>
        /// <returns>True if constraints are satisfied; False if not satisfied.</returns>
        /// <remarks>
        /// Ref. X.660: 7.6
        /// a) the root arcs are restricted to three arcs with primary integer values 0 to 2; and
        /// b) the arcs beneath the root arcs 0 and 1 are restricted to forty arcs with primary integer values 0 to 39.
        /// </remarks>
        protected override bool VerifyConstraints()
        {
            //Ref. X.660: 7.6
            if (Value == null || Value.Length < 2 || Value[0] > 2)
            {
                return false;
            }
            if (Value[0] <= 1 && Value[1] > 39)
            {
                return false;
            }
            return true;
        }

        #region overrode methods from System.Object

        /// <summary>
        /// Overrode method from System.Object.
        /// </summary>
        /// <param name="obj">The object to be compared.</param>
        /// <returns>True if obj has same data with this instance. False if not.</returns>
        public override bool Equals(object obj)
        {
            // If parameter is null or cannot be cast to Asn1ObjectIdentifier return false.
            Asn1ObjectIdentifier p = obj as Asn1ObjectIdentifier;
            return p != null && Value.SequenceEqual(p.Value);
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

        #region Encode & Decode Functionality

        /// <summary>
        /// Encodes an integer value (subidentifier) to bytes.
        /// </summary>
        /// <param name="val"></param>
        /// <returns>A list which contains the encoding result.</returns>
        private static IEnumerable<byte> EncodeSubIdentifier(int val)
        {
            if (val < 0)
            {
                throw new NotImplementedException("Negative identifier.");
            }
            //Ref: X.690:8.19.2
            var res = new List<byte>();
            //Take the last 7 bits
            var last = (byte)(val & 127);//bit 8 is 0
            res.Add(last);
            val >>= 7;
            while (val != 0)
            {
                var cur = (byte)(128 + (val & 127));//bit 8 is 1
                res.Insert(0, cur);
                val >>= 7;
            }
            return res;
        }

        /// <summary>
        /// Encodes an array of integers to bytes.
        /// </summary>
        /// <param name="val"></param>
        /// <returns>An array of bytes which contains the encoding result.</returns>
        private static byte[] EncodeIdentifier(IList<int> val)
        {
            //Ref: X.690:8.19.4
            var first = val[0] * 40 + val[1];
            var all = new List<byte>();
            all.AddRange(EncodeSubIdentifier(first));
            for (var i = 2; i < val.Count; i++)
            {
                all.AddRange(EncodeSubIdentifier(val[i]));
            }
            return all.ToArray();
        }

        /// <summary>
        /// Decodes integers (subidentifiers) from a byte array by BER.
        /// </summary>
        /// <param name="val"></param>
        /// <returns>An array of integers that contains the decoding result. Length is not included in val.</returns>
        private static int[] DecodeIdentifiers(IList<byte> val)
        {
            var res = new List<int>();
            var first = true;
            var curPos = 0;
            while (curPos != val.Count)
            {
                var curIntVal = 0;
                do
                {
                    curIntVal <<= 7;
                    curIntVal |= (val[curPos] & 127);
                } while (val[curPos++] >= 128);
                if (first)
                {
                    if (curIntVal >= 128)
                    {
                        throw new NotImplementedException("Ojbect Identifier with first element greater than 127 is not implemented."); 
                    }
                    first = false;
                    var val1 = (curIntVal >= 80 ? 2 : curIntVal / 40);
                    var val2 = (curIntVal - val1 * 40);
                    res.Add(val1);
                    res.Add(val2);
                }
                else
                {
                    res.Add(curIntVal);
                }
            }
            return res.ToArray();
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
            var res = EncodeIdentifier(Value);
            buffer.WriteBytes(res);
            return res.Length;
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
            var all = buffer.ReadBytes(length);
            Value = DecodeIdentifiers(all);
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
            if (Value[0]*40 + Value[1] >= 128)
            {
                throw new NotImplementedException("Ojbect Identifier with first element greater than 127 is not implemented.");
                //Len will be minus 1 while encoding.
            }
            var res = EncodeIdentifier(Value);
            Asn1StandardProcedure.PerEncodeArray<byte>(buffer, res,
                (encodingBuffer, b) =>
                {
                    encodingBuffer.WriteByte(b);
                });
        }

        /// <summary>
        /// Decodes the content of the object by PER.
        /// </summary>
        /// <param name="buffer">A buffer that contains a PER encoding result.</param>
        /// <param name="aligned">Indicating whether the PER decoding is aligned.</param>
        /// <remarks>Length is included.</remarks>
        protected override void ValuePerDecode(IAsn1DecodingBuffer buffer, bool aligned = true)
        {
            byte[] res = Asn1StandardProcedure.PerDecodeArray(buffer, decodingBuffer => (decodingBuffer.ReadByte()));
            Value = DecodeIdentifiers(res);
        }

        #endregion PER
    }
}

