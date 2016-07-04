// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Diagnostics;

namespace Microsoft.Protocols.TestTools.StackSdk.Asn1
{
    /// <summary>
    /// Represents an INTEGER in ASN.1 Definition.
    /// </summary>
    [Asn1Tag(Asn1TagType.Universal, Asn1TagValue.Integer)]
    public class Asn1Integer : Asn1Object
    {
        /// <summary>
        /// Gets or sets the data of this object.
        /// </summary>
        public long? Value
        {
            get;
            set;
        }

        /// <summary>
        /// Constraints will be extracted from attributes in constructor.
        /// </summary>
        protected internal Asn1IntegerBound Constraints;

        /// <summary>
        /// Range = upperBound - lowerBound + 1, Ref. X.691: 10.5.3.
        /// </summary>
        /// <remarks>
        /// If upperBound is null or lowerBound is null, range is set to null.
        /// </remarks>
        protected long? Range
        {
            get
            {
                if (Constraints != null && Constraints.HasMax && Constraints.HasMin)
                {
                    if (Constraints.Max < Constraints.Min)
                    {
                        throw new Asn1InvalidArgument(ExceptionMessages.UserDefinedTypeInconsistent +
                                                      " Max should be not less than Min.");
                    }
                    return Constraints.Max - Constraints.Min + 1;
                }
                else
                {
                    return null;
                }
            }
        }

        /// <summary>
        /// Initializes a new instance of the Asn1Integer class with the default value 0.
        /// </summary>
        public Asn1Integer()
            : this(0)
        {

        }

        /// <summary>
        /// Initializes a new instance of the Asn1Integer class with a given value and bounds.
        /// </summary>
        /// <param name="val"></param>
        /// <param name="min"></param>
        /// <param name="max"></param>
        protected internal Asn1Integer(long? val, long? min, long? max)
        {
            Value = val;
            if (Constraints == null)
            {
                Constraints = new Asn1IntegerBound();
                if (min != null)
                {
                    Constraints.Min = (long)min;
                }
                if (max != null)
                {
                    Constraints.Max = (long)max;
                }
            }
        }

        /// <summary>
        /// Initializes a new instance of the Asn1Integer class with a given value.
        /// </summary>
        /// <param name="val"></param>
        public Asn1Integer(long? val)
        {
            Value = val;
            Constraints = null;

            //Gets the upper and lower bound for the structure.
            object[] allAttributes = GetType().GetCustomAttributes(true);
            foreach (object o in allAttributes)
            {
                if (o is Asn1IntegerBound)
                {
                    Constraints = o as Asn1IntegerBound;
                    break;
                }
            }
        }

        /// <summary>
        /// Ensures Value is not greater than upper bound and not less than lower bound. 
        /// </summary>
        /// <returns>True if Value is between lower bound and upper bound, false if not.</returns>
        protected override bool VerifyConstraints()
        {
            return Value != null && (Constraints == null ||
                ((!Constraints.HasMax || Value <= Constraints.Max) && (!Constraints.HasMin || Value >= Constraints.Min)));
        }

        #region overrode methods from System.Object

        /// <summary>
        /// Overrode method from System.Object.
        /// </summary>
        /// <param name="obj">The object to be compared.</param>
        /// <returns>True if obj has same data with this instance. False if not.</returns>
        public override bool Equals(object obj)
        {
            // If parameter is null or cannot be cast to Asn1Integer return false.
            Asn1Integer p = obj as Asn1Integer;
            if (p == null)
            {
                return false;
            }

            // Return true if the fields match.
            return Value == p.Value;
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

        /// <summary>
        /// Gets the maximum number of the bytes that are used to store the data.
        /// </summary>
        private static int BytesCountForLong { get { return sizeof(long); } }

        //Constraint is not needed for primitive type INTEGER.
        //Therefore no need to override VerifyConstraints method.

        /// <summary>
        /// Provides the BER encoding functionality for INTEGER.
        /// </summary>
        /// <param name="val">The value to be encoded.</param>
        /// <returns>A byte array that contains the encoding result. No length, no Flag. Shrinked. Big Endian.</returns>
        /// <exception cref="Asn1EmptyDataException">
        /// Thrown when the object doesn't contain any data.
        /// </exception>
        private static byte[] IntegerEncoding(long? val)
        {
            if (val == null)
            {
                throw new Asn1EmptyDataException(ExceptionMessages.EmptyData);
            }
            byte[] bitConverterResult = BitConverter.GetBytes((long)val);
            //Ensure the bitConverterResult is big endian.
            //The ASN.1 encoding result is big endian.
            if (BitConverter.IsLittleEndian)
            {
                Array.Reverse(bitConverterResult);
            }

            //Ref. X.690: 8.3.2.
            //Shrink to ensure minimal number of bytes are used.
            //The first bit equals to 0 means it is a nonnegative numer.
            //In this case 00000000 0....... is not space saving since we could remove the first 8 bits.
            //The first bit equals to 1 means it is a negative number.
            //In this case 11111111 1....... is not space saving since we could also remove the first 8 bits.

            //Calculate the number of the bytes to be removed/shrinked.
            int deleteCount = GetDeleteCount(bitConverterResult, val < 0);

            //Delete the redundant bytes and adjust to big endian.
            byte[] result = new byte[BytesCountForLong - deleteCount];
            Array.Copy(bitConverterResult, deleteCount, result, 0, BytesCountForLong - deleteCount);
            return result;
        }

        /// <summary>
        /// Calculates the delete count of the bytes when shrinking the encoding result.
        /// </summary>
        /// <param name="bytesBigEndian">The entiry encoding result of a long value, big endian.</param>
        /// <param name="negative">Indicating whether the encoded long value is negative.</param>
        /// <returns>The number of the bytes to be shrinked.</returns>
        private static int GetDeleteCount(byte[] bytesBigEndian, bool negative)
        {
            int deleteCount = 0;
            if (!negative)
            {
                while (deleteCount < BytesCountForLong - 1 && //at most delete 7 bytes
                    bytesBigEndian[deleteCount] == 0 && //00000000
                    bytesBigEndian[deleteCount + 1] < 128) //0.......
                {
                    deleteCount++;
                }
            }
            else
            {
                //Value < 0
                while (deleteCount < BytesCountForLong - 1 && //at most delete 7 bytes
                    bytesBigEndian[deleteCount] == 255 && //11111111
                    bytesBigEndian[deleteCount + 1] >= 128) //1.......
                {
                    deleteCount++;
                }
            }
            return deleteCount;
        }

        /// <summary>
        /// Provide the BER decoding functionality for INTEGER.
        /// </summary>
        /// <param name="encodingResult">A byte array that stores an encoding result. No flag and length. Shrinked. Big Endian.</param>
        /// <returns>The decoding result.</returns>
        private static long IntegerDecoding(byte[] encodingResult)
        {
            int len = encodingResult.Length;
            if (len == 0)
            {
                throw new Asn1DecodingOutOfBufferRangeException(ExceptionMessages.DecodingOutOfRange);
            }
            byte[] fullContent = GetExpandedContent(encodingResult);//fullContent is little endian
            if (!BitConverter.IsLittleEndian)
            {
                //If the system is not little endian, then change fullcontent to big endian.
                Array.Reverse(fullContent);
            }
            return BitConverter.ToInt64(fullContent, 0);
        }

        /// <summary>
        /// Expands the encoding result.
        /// </summary>
        /// <param name="bigEndianEncodingResult">A shrinked big endian encoding result.</param>
        /// <returns>An expanded little endian result.</returns>
        private static byte[] GetExpandedContent(byte[] bigEndianEncodingResult)
        {
            byte[] fullContent = new byte[BytesCountForLong];
            //adjust to little endian and expand the array so that it could be processed by BitConverter.
            Array.Copy(bigEndianEncodingResult, fullContent, bigEndianEncodingResult.Length);
            Array.Reverse(fullContent, 0, bigEndianEncodingResult.Length);
            int curPos = bigEndianEncodingResult.Length;

            if (bigEndianEncodingResult[0] < 128) //it's a positive data's encoding result
            {
                while (curPos != sizeof(long))
                {
                    fullContent[curPos++] = 0;
                }
            }
            else//it's a negative data's encoding result
            {
                while (curPos != BytesCountForLong)
                {
                    fullContent[curPos++] = 255;
                }
            }
            return fullContent;
        }

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
            byte[] result = IntegerEncoding(Value);
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
            Value = IntegerDecoding(result);
            return length;
        }

        #endregion BER

        #region PER

        private byte[] NonNegativeBinaryIntegerPerEncode(long val)//Ref. X.691:10.3
        {
            byte[] bitConverterResult = BitConverter.GetBytes(val);
            if (BitConverter.IsLittleEndian)
            {
                Array.Reverse(bitConverterResult);
            }
            int notZeroIndex = 0;
            while (bitConverterResult[notZeroIndex] == 0 && notZeroIndex < 7)
            {
                notZeroIndex++;
            }
            byte[] result = new byte[8 - notZeroIndex];
            Array.Copy(bitConverterResult, notZeroIndex, result, 0, 8 - notZeroIndex);
            return result;
        }

        private long NonNegativeBinaryIntegerPerDeocde(byte[] result)
        {
            if (result.Length == 0)
            {
                throw new Asn1DecodingUnexpectedData(ExceptionMessages.DecodingUnexpectedData);
            }
            byte[] fullContent = new byte[BytesCountForLong];
            //adjust to little endian and expand the array so that it could be processed by BitConverter.
            Array.Copy(result, fullContent, result.Length);
            Array.Reverse(fullContent, 0, result.Length);
            int curPos = result.Length;

            while (curPos != BytesCountForLong)
            {
                fullContent[curPos++] = 0;
            }

            if (!BitConverter.IsLittleEndian)
            {
                //If the system is not little endian, then change fullcontent to big endian.
                Array.Reverse(fullContent);
            }
            return BitConverter.ToInt64(fullContent, 0);
        }

        /// <summary>
        /// Encodes the content of the object by PER.
        /// </summary>
        /// <param name="buffer">A buffer to which the encoding result will be written.</param>
        protected override void ValuePerEncode(IAsn1PerEncodingBuffer buffer)
        {
            long offset;
            if (Constraints == null || !Constraints.HasMin)
            {
                offset = (long)Value;
            }
            else
            {
                offset = (long)(Value - Constraints.Min);
            }

            //Ref. X.691: 10.5.2
            if (Range == null)
            {
                if (offset == 0)
                {
                    buffer.WriteByte(0);
                    return;
                }
                //Ref. X.691: 10.3, 10.4
                byte[] result;
                if (Constraints == null || !Constraints.HasMin)
                {
                    result = IntegerEncoding(offset);
                }
                else
                {
                    //lowerBound != null && upperBound == null
                    result = NonNegativeBinaryIntegerPerEncode(offset);
                }
                int len = result.Length;
                buffer.WriteByte((byte)len);
                buffer.WriteBytes(result);
                return;
            }
            else
            {
                //lowerBound != null && upperBound != null, range != null, offset should be non-negative
                //Ref. X.691: 10.5
                if (Range == 1)
                {
                    //X.691: 10.5.4
                    return;
                }
                else if (Range <= 255) //10.5.7: a)
                {
                    int bitFieldSize = 1;
                    while (Range > (1 << (bitFieldSize)))
                    {
                        bitFieldSize++;
                    }
                    byte[] temp = new byte[] { (byte)offset };
                    buffer.WriteBits(temp, 8 - bitFieldSize, bitFieldSize);
                    return;
                }
                else if (Range == 256) //10.5.7: b)
                {
                    buffer.WriteByte((byte)offset);
                    return;
                }
                else if (Range <= 256 * 256) //10.5.7: c)
                {
                    byte[] bytes = NonNegativeBinaryIntegerPerEncode(offset);//Length is either be 1 or 2
                    if (bytes.Length == 1)
                    {
                        buffer.WriteByte(0);
                    }
                    buffer.WriteBytes(bytes);
                    return;
                }
                else //10.5.7: d)
                {
                    byte[] bytes = NonNegativeBinaryIntegerPerEncode(offset);
                    Asn1Integer len = new Asn1Integer(bytes.Length, 1, 4);
                    len.PerEncode(buffer);
                    buffer.WriteBytes(bytes);
                    return;
                }
            }
        }

        /// <summary>
        /// Decodes the object by PER.
        /// </summary>
        /// <param name="buffer">A buffer that contains a PER encoding result.</param>
        /// <param name="aligned">Indicating whether the PER decoding is aligned.</param>
        protected override void ValuePerDecode(IAsn1DecodingBuffer buffer, bool aligned = true)
        {
            long baseNum = (Constraints != null && Constraints.HasMin ? Constraints.Min : 0);
            //Ref. X.691: 10.5.2
            if (Range == null) //range is equal to null
            {
                byte length = buffer.ReadByte();
                if (length == 0)
                {
                    Value = baseNum;
                    return;
                }

                byte[] result = buffer.ReadBytes(length);
                if (Constraints == null || !Constraints.HasMin)
                {
                    //No base
                    Value = IntegerDecoding(result);
                }
                else
                {
                    Value = baseNum + NonNegativeBinaryIntegerPerDeocde(result);
                }
            }
            else
            {
                if (Range == 1)
                {
                    Value = Constraints.Min;
                }
                else if (Range <= 255) //10.5.7: a)
                {
                    int bitFieldSize = 1;
                    while (Range > (1 << (bitFieldSize)))
                    {
                        bitFieldSize++;
                    }
                    bool[] result = buffer.ReadBits(bitFieldSize);
                    byte offset = 0;
                    foreach (var b in result)
                    {
                        offset <<= 1;
                        offset += (byte)(b ? 1 : 0);
                    }
                    Value = baseNum + offset;
                }
                else if (Range == 256) //10.5.7: b)
                {
                    Value = baseNum + buffer.ReadByte();
                }
                else if (Range <= 256 * 256) //10.5.7: c)
                {
                    byte[] bytes = buffer.ReadBytes(2);
                    Value = baseNum + bytes[0] * 256 + bytes[1];
                }
                else //10.5.7: d)
                {
                    Asn1Integer len = new Asn1Integer { Constraints = new Asn1IntegerBound { Min = 1, Max = 4 } };
                    len.PerDecode(buffer);
                    byte[] bytes = buffer.ReadBytes((int)len.Value);
                    Value = baseNum + NonNegativeBinaryIntegerPerDeocde(bytes);
                }
            }
        }

        #endregion PER
    }
}
