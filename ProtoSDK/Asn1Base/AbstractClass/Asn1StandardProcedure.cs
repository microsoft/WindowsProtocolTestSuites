// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections;
using System.Collections.Generic;

namespace Microsoft.Protocols.TestTools.StackSdk.Asn1
{
    /// <summary>
    /// Contains some common standard BER encoding/decoding procedures.
    /// </summary>
    public static class Asn1StandardProcedure
    {
        /// <summary>
        /// Encodes a length to the buffer.
        /// </summary>
        /// <param name="buffer"></param>
        /// <param name="length">The length to be written to the buffer.</param>
        /// <returns>The length of "the encoded length's encoding result".</returns>
        public static int LengthBerEncode(IAsn1BerEncodingBuffer buffer, int length)
        {
            if (length < 0)
            {
                throw new Asn1InvalidArgument(ExceptionMessages.LengthNegative);
            }
            if (length < 128)
            {
                buffer.WriteByte((byte)length);
                return 1;
            }
            else
            {
                //The encoding result of a length that no less than 128 will be stored in at least two bytes.
                //The first byte:  1xxxxxxx, xxxxxxx is the length of the following encoding result.
                //The next bytes:  the length's encoding result, big endian.
                //Ref. X.690: 8.1.3
                byte[] bytes = BitConverter.GetBytes(length);
                //ensure bytes is big endian
                if (BitConverter.IsLittleEndian)
                {
                    Array.Reverse(bytes);
                }

                //delete the redundant zeros
                int zeroNum = 0;
                for (int i = 0; i < bytes.Length; i++)
                {
                    if (bytes[i] == 0)
                    {
                        zeroNum++;
                    }
                    else
                    {
                        break;
                    }
                }
                //zeroNum won't be equal to bytes.Length since length>=128.

                //Encode data first, then encode length of the data's encoding result since buffer is reversed.
                buffer.WriteBytes(bytes, zeroNum, bytes.Length - zeroNum);
                buffer.WriteByte((byte)((bytes.Length - zeroNum) | (1 << 7)));//set the first bit to 1
                return 1 + bytes.Length - zeroNum;
            }
        }

        /// <summary>
        /// Reads/decodes a length from the buffer.
        /// </summary>
        /// <param name="buffer">A buffer that stores a BER encoding result.</param>
        /// <param name="decodedLength">The decoded length will be retrieved by this param.</param>
        /// <returns>The number of the bytes consumed in the buffer to decode the length.</returns>
        public static int LengthBerDecode(IAsn1DecodingBuffer buffer, out int decodedLength)
        {
            byte firstByte = buffer.ReadByte();
            if (firstByte < 128)
            {
                decodedLength = firstByte;
                return 1;
            }
            else
            {
                int length = (firstByte & ((1 << 7) - 1));//set the first bit to 0
                if (length == 0)
                {
                    //Check X.690: 8.1.3.6
                    throw new NotImplementedException("Indefinite form of length is not implemented. Check X.690: 8.1.3.6.");
                }
                byte[] bigEndianLengthCode = buffer.ReadBytes(length);
                byte[] allContent = new byte[sizeof(int)];//no need to use long since all of the Length property is int
                Array.Copy(bigEndianLengthCode, 0,
                    allContent, sizeof(int) - bigEndianLengthCode.Length, bigEndianLengthCode.Length);
                //allContent is big endian
                if (BitConverter.IsLittleEndian)
                {
                    Array.Reverse(allContent);
                }
                decodedLength = BitConverter.ToInt32(allContent, 0);
                return 1 + length;
            }
        }

        /// <summary>
        /// Encodes a tag to the buffer.
        /// </summary>
        /// <param name="buffer"></param>
        /// <param name="tag"></param>
        /// <returns>The length of the encoding result.</returns>
        public static int TagBerEncode(IAsn1BerEncodingBuffer buffer, Asn1Tag tag)
        {
            //Ref. X.690: 8.1.2.2, there are four kinds of tags
            byte prefix = 0;
            switch (tag.TagType)
            {
                case Asn1TagType.Universal:
                    {
                        prefix = 0;//00
                    } break;
                case Asn1TagType.Application:
                    {
                        prefix = 1;//01
                    } break;
                case Asn1TagType.Context:
                    {
                        prefix = 2;//10
                    } break;
                case Asn1TagType.Private:
                    {
                        prefix = 3;//11
                        //Ref. X.680: G.2.12.4
                        throw new Asn1UnreachableExcpetion(ExceptionMessages.Unreachable);
                    };
            }
            prefix <<= 6;
            if (tag.EncodingWay == EncodingWay.Constructed)
            {
                prefix |= (1 << 5);//Set the sixth bit to 1 if it is encoded in constructed way. Ref. X.690: 8.1.2.3
            }
            if (tag.TagValue <= 30)//Use one byte to store the encoding result
            //Ref. X.690: 8.1.2.3
            {
                prefix |= (byte)tag.TagValue;
                buffer.WriteByte(prefix);
                return 1;
            }
            else//Use more than one bytes to store.
            {
                //Ref. X.690: 8.1.2.4.3
                throw new NotImplementedException("Case tag > 30 is not implemented. Check X.690: 8.1.2.4.3.");
            }
        }


        /// <summary>
        /// Decodes a tag from the buffer.
        /// </summary>
        /// <param name="buffer"></param>
        /// <param name="tag"></param>
        /// <returns>The number of the bytes consumed in the buffer to decode the tag.</returns>
        public static int TagBerDecode(IAsn1DecodingBuffer buffer, out Asn1Tag tag)
        {
            byte prefix = buffer.ReadByte();
            int firstTwoBits = (prefix >> 6);
            Asn1TagType tagType = Asn1TagType.Private;
            long tagValue;
            switch (firstTwoBits)
            {
                case 0:
                    {
                        tagType = Asn1TagType.Universal;
                    } break;
                case 1:
                    {
                        tagType = Asn1TagType.Application;
                    } break;
                case 2:
                    {
                        tagType = Asn1TagType.Context;
                    } break;
                case 3:
                    {
                        throw new NotImplementedException(ExceptionMessages.Unreachable);
                    };
            }
            tagValue = prefix & ((byte)(1 << 5) - 1);
            if (tagValue <= 30)
            {
                tag = new Asn1Tag(tagType, tagValue);
                if ((prefix & (1 << 5)) != 0)
                {
                    tag.EncodingWay = EncodingWay.Constructed;
                }
                else
                {
                    tag.EncodingWay = EncodingWay.Primitive;
                }
                return 1;
            }
            else
            {
                throw new NotImplementedException("Case tag > 30 is not implemented. Check X.690: 8.1.2.4.3.");
            }
        }

        public static byte GetEncodeTag(Asn1Tag tag)
        {
            //Ref. X.690: 8.1.2.2, there are four kinds of tags
            byte prefix = 0;
            switch (tag.TagType)
            {
                case Asn1TagType.Universal:
                    {
                        prefix = 0;//00
                    } break;
                case Asn1TagType.Application:
                    {
                        prefix = 1;//01
                    } break;
                case Asn1TagType.Context:
                    {
                        prefix = 2;//10
                    } break;
                case Asn1TagType.Private:
                    {
                        prefix = 3;//11
                        //Ref. X.680: G.2.12.4
                        throw new Asn1UnreachableExcpetion(ExceptionMessages.Unreachable);
                    };
            }
            prefix <<= 6;
            if (tag.EncodingWay == EncodingWay.Constructed)
            {
                prefix |= (1 << 5);//Set the sixth bit to 1 if it is encoded in constructed way. Ref. X.690: 8.1.2.3
            }
            if (tag.TagValue <= 30)//Use one byte to store the encoding result
            //Ref. X.690: 8.1.2.3
            {
                prefix |= (byte)tag.TagValue;
                return prefix;
            }
            else//Use more than one bytes to store.
            {
                //Ref. X.690: 8.1.2.4.3
                throw new NotImplementedException("Case tag > 30 is not implemented. Check X.690: 8.1.2.4.3.");
            }
        }

        /// <summary>
        /// Encapsulates a method that encode an object to a buffer.
        /// </summary>
        /// <typeparam name="T">Type of the object.</typeparam>
        /// <param name="buffer">A buffer that will store the encoding result for obj.</param>
        /// <param name="obj">The object to be encoded.</param>
        public delegate void PerEncodeSingleObject<T>(IAsn1PerEncodingBuffer buffer, T obj);

        /// <summary>
        /// Encapsulates a method that decode an object from a buffer.
        /// </summary>
        /// <typeparam name="T">The type of the object.</typeparam>
        /// <param name="buffer">A buffer that stores the encoding result of the object.</param>
        /// <returns>The decoded object.</returns>
        public delegate T PerDecodeSingleObject<T>(IAsn1DecodingBuffer buffer);

        /// <summary>
        /// Encodes an array of objects by PER.
        /// </summary>
        /// <typeparam name="T">The type of the objects.</typeparam>
        /// <param name="buffer">A buffer that will store the encoding result of the objects.</param>
        /// <param name="objs">The array of the objects.</param>
        /// <param name="encoder">A method that provide the functionality of encoding a single object.</param>
        /// <param name="minSize">Minimal size of the array.</param>
        /// <param name="maxSize">Maximal size of the array.</param>
        /// <param name="alignAfterEncodeLength">Indicates whether align after the length is encoded.</param>
        public static void PerEncodeArray<T>(IAsn1PerEncodingBuffer buffer, T[] objs, PerEncodeSingleObject<T> encoder,
            long? minSize = null, long? maxSize = null, bool alignAfterEncodeLength = false)
        {
            if (minSize == null && maxSize == null)
            {
                bool len16KMultiple = false;
                int len = objs.Length;
                if (len == 0)
                {
                    buffer.WriteByte(0);
                    return;
                }
                int curIndex = 0;
                while (curIndex != len)
                {
                    int writeContentNum = 0;
                    int rest = len - curIndex;
                    if (rest < 128) //X.691: 10.9 a)
                    {
                        buffer.WriteByte((byte)rest);
                        writeContentNum = rest;
                        len16KMultiple = false;
                    }
                    else if (rest < 16 * 1024) //X.691: 10.9 b) 16K
                    {
                        buffer.WriteByte((byte)(128 | (rest >> 8)));
                        buffer.WriteByte((byte)(rest & 255));
                        writeContentNum = rest;
                        len16KMultiple = false;
                    }
                    else
                    {
                        int fragmentNum = rest / (16 * 1024);//number of 16K items, X.691: 10.9 c)
                        if (fragmentNum > 4)
                        {
                            fragmentNum = 4;
                        }
                        buffer.WriteByte((byte)(192 + fragmentNum));
                        writeContentNum = fragmentNum * 16 * 1024;
                        len16KMultiple = true;
                    }
                    for (int i = 0; i < writeContentNum; i++, curIndex++)
                    {
                        encoder(buffer, objs[curIndex]);
                    }
                }
                if (len16KMultiple)
                {
                    //throw new Asn1InvalidArgument("Objsys bug");
                    buffer.WriteByte(0);
                }
            }
            else if (minSize != null && maxSize != null)
            {
                if (minSize < 0 || maxSize < minSize)
                {
                    throw new Asn1UserDefinedTypeInconsistent(ExceptionMessages.UserDefinedTypeInconsistent + " Size constraints illegal.");
                }
                long range = (long)(maxSize - minSize + 1);
                if (range > 256L * 256)
                {
                    throw new NotImplementedException("Array with big size constraints are not implemented yet.");
                }
                //range < 256 * 256
                if (maxSize == 0) //Ref. X.691: 16.5
                {
                    return;
                }
                if (maxSize != minSize) //Ref. X.691: 16.8
                {
                    //Encode length 
                    Asn1Integer ai = new Asn1Integer(objs.Length, minSize, maxSize);
                    ai.PerEncode(buffer);
                    if (alignAfterEncodeLength)
                    {
                        buffer.AlignData();
                    }
                }
                foreach (T curObj in objs)
                {
                    encoder(buffer, curObj);
                }
            }
            else
            {
                throw new NotImplementedException("Array with semi size constraints are not implemented yet.");
            }
        }

        /// <summary>
        /// Decodes an array of objects from a buffer.
        /// </summary>
        /// <typeparam name="T">Type of the objects.</typeparam>
        /// <param name="buffer">A buffer that stores the encoding result of the objects.</param>
        /// <param name="decoder">A method that provides the functionality of decoding a single object.</param>
        /// <param name="minSize">Minimal size of the array.</param>
        /// <param name="maxSize">Maximal size of the array.</param>
        /// <param name="alignAfterDecodeLength">Indicates whether align when the length is decoded.</param>
        /// <returns>An array of the objects that are decoded from the buffer.</returns>
        public static T[] PerDecodeArray<T>(IAsn1DecodingBuffer buffer, PerDecodeSingleObject<T> decoder,
            long? minSize = null, long? maxSize = null, bool alignAfterDecodeLength = false)
        {
            List<T> result = new List<T>();
            long decodeNum = 0;
            if (minSize == null && maxSize == null)
            {
                bool end = false;
                while (!end)
                {
                    byte lenHead = buffer.ReadByte();
                    if (lenHead > 196)
                    {
                        throw new Asn1DecodingUnexpectedData(ExceptionMessages.DecodingUnexpectedData + " Length in PER fragment illegal.");
                    }
                    else if (lenHead > 192)
                    {
                        long fraNum = lenHead - 192;
                        decodeNum = fraNum * 16 * 1024;
                        end = false;
                    }
                    else if (lenHead >= 128)
                    {
                        byte lenNext = buffer.ReadByte();
                        decodeNum = 256L * (lenHead-128) + lenNext;
                        end = true;
                    }
                    else //lenHead < 128
                    {
                        decodeNum = lenHead;
                        end = true;
                    }
                    for (long i = 0; i < decodeNum; i++)
                    {
                        T t = decoder(buffer);
                        result.Add(t);
                    }
                }
                return result.ToArray();
            }
            else if (minSize != null && maxSize != null)
            {
                if (minSize < 0 || maxSize < minSize)
                {
                    throw new Asn1UserDefinedTypeInconsistent(ExceptionMessages.UserDefinedTypeInconsistent + " Size constraints illegal.");
                }
                long range = (long)(maxSize - minSize + 1);
                if (range > 256L * 256)
                {
                    throw new NotImplementedException("Array with big size constraints are not implemented yet.");
                }
                //range < 256 * 256
                if (maxSize == 0) //Ref. X.691: 16.5
                {
                    return new T[0];
                }
                if (maxSize != minSize) //Ref. X.691: 16.8
                {
                    //Decode length 
                    Asn1Integer ai = new Asn1Integer(null, minSize, maxSize);
                    ai.PerDecode(buffer);
                    if (alignAfterDecodeLength)
                    {
                        buffer.AlignData();
                    } 
                    decodeNum = (long)ai.Value;
                }
                else
                {
                    decodeNum = minSize.Value;
                }
                for (long i = 0; i < decodeNum; i++)
                {
                    T t = decoder(buffer);
                    result.Add(t);
                }
                return result.ToArray();
            }
            else
            {
                throw new NotImplementedException("Array with semi size constraints are not implemented yet.");
            }

        }
    }
}
