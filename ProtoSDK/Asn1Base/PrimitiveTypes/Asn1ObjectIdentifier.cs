// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Linq;
using System.Collections.Generic;

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
        public int[] Value{get; set;}

        /// <summary>
        /// Initializes a new instance of the Asn1ObjectIdentifier class with empty data.
        /// </summary>
        public Asn1ObjectIdentifier()
        {
            this.Value = null;
        }

        /// <summary>
        /// Initializes a new instance of the Asn1ObjectIdentifier class with a given value.
        /// </summary>
        /// <param name="val"></param>
        public Asn1ObjectIdentifier(int[] val)
        {
            this.Value = val;
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
            if (this.Value == null || this.Value.Length < 2 || this.Value[0] > 2)
            {
                return false;
            }
            if (this.Value[0] <= 1 && this.Value[1] > 39)
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
        public override bool Equals(System.Object obj)
        {
            // If parameter is null return false.
            if (obj == null)
            {
                return false;
            }

            // If parameter cannot be cast to Asn1ObjectIdentifier return false.
            Asn1ObjectIdentifier p = obj as Asn1ObjectIdentifier;
            if ((System.Object)p == null)
            {
                return false;
            }

            // Return true if the fields match.
            return Enumerable.SequenceEqual<int>(this.Value, p.Value);
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
        private List<byte> EncodeSubIdentifier(int val)
        {
            if (val < 0)
            {
                throw new NotImplementedException("Negative identifier.");
            }
            //Ref: X.690:8.19.2
            List<byte> res = new List<byte>();
            //Take the last 7 bits
            byte last = (byte)(val & 127);//bit 8 is 0
            res.Add(last);
            val >>= 7;
            while (val != 0)
            {
                byte cur = (byte)(128 + (val & 127));//bit 8 is 1
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
        private byte[] EncodeIdentifier(int[] val)
        {
            //Ref: X.690:8.19.4
            int first = val[0] * 40 + val[1];
            List<byte> all = new List<byte>();
            all.AddRange(EncodeSubIdentifier(first));
            for (int i = 2; i < val.Length; i++)
            {
                all.AddRange(EncodeSubIdentifier(val[i]));
            }
            return all.ToArray();
        }

        /// <summary>
        /// Decodes integers (subidentifiers) from a byte array.
        /// </summary>
        /// <param name="val"></param>
        /// <returns>An array of integers that contains the decoding result.</returns>
        private int[] DecodeSubidentifiers(byte[] val)
        {
            List<int> res = new List<int>();
            bool first = true;
            int curPos = 0;
            while (curPos != val.Length)
            {
                int curIntVal = 0;
                do
                {
                    curIntVal <<= 7;
                    curIntVal |= (val[curPos] & 127);
                } while (val[curPos++] >= 128);
                if (first)
                {
                    first = false;
                    int val1 = (curIntVal >= 80 ? 2 : curIntVal / 40);
                    int val2 = (curIntVal - val1 * 40);
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
            byte[] res = EncodeIdentifier(this.Value);
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
            byte[] all = buffer.ReadBytes(length);
            this.Value = DecodeSubidentifiers(all);
            return length;
        }

        #endregion BER

        #region PER

        #endregion PER
    }
}

