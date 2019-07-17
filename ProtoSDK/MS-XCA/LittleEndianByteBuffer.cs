// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;

namespace Microsoft.Protocols.TestTools.StackSdk.Compression.Xpress
{
    /// <summary>
    /// Little-endian byte buffer.
    /// </summary>
    public class LittleEndianByteBuffer
    {
        public LittleEndianByteBuffer()
        {
            buffer = new List<byte>();
        }

        public LittleEndianByteBuffer(byte[] input)
        {
            buffer = new List<byte>(input);
        }

        private List<byte> buffer;

        public byte this[int index]
        {
            get
            {
                return buffer[index];
            }
        }

        public int Count
        {
            get
            {
                return buffer.Count;
            }
        }

        public byte[] GetBytes()
        {
            return buffer.ToArray();
        }

        public void WriteBytes(int offset, int value, int length)
        {
            var input = new byte[length];

            for (int i = 0; i < length; i++)
            {
                input[i] = (byte)(value >> (8 * i));
            }

            if (offset + length - 1 >= buffer.Count)
            {
                buffer.AddRange(new byte[offset + length - buffer.Count]);
            }

            for (int i = 0; i < length; i++)
            {
                buffer[offset + i] = input[i];
            }
        }

        public int ReadBytes(int offset, int count)
        {
            int result = 0;
            for (int i = 0; i < count; i++)
            {
                int location = offset + i;
                if (location < 0 || location >= buffer.Count)
                {
                    throw new ArgumentOutOfRangeException("Either offset or count is out of range!");
                }
                byte b = buffer[offset + i];
                result |= (b << (i * 8));
            }
            return result;
        }

        public LittleEndianByteBuffer SubCopy(int offset, int length)
        {
            if (offset + length > buffer.Count)
            {
                throw new ArgumentOutOfRangeException("Either offset or length is out of range!");
            }
            var result = new LittleEndianByteBuffer(buffer.GetRange(offset, length).ToArray());
            return result;
        }
    }
}
