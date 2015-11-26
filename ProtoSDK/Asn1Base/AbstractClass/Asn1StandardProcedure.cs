// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;

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
    }
}
