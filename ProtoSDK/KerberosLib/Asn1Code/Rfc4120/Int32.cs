// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Protocols.TestTools.StackSdk.Asn1;
using System;

namespace Microsoft.Protocols.TestTools.StackSdk.Security.KerberosLib
{
    /*
    Int32           ::= INTEGER (-2147483648..2147483647)
    */
    public class KerbInt32 : Asn1Integer
    {
        protected override bool VerifyConstraints()
        {
            return this.Value >=-2147483648 && this.Value <= 2147483647;
        }

        public KerbInt32()
            : base()
        {
        }
        
        public KerbInt32(long? val)
            : base(val)
        {
        }

        private static int BytesCountForLong { get { return sizeof(long); } }

        protected override int ValueBerDecode(IAsn1DecodingBuffer buffer, int length)
        {
            byte[] result = buffer.ReadBytes(length);
            Value = IntegerDecoding(result);
            return length;
        }

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
    }
}

