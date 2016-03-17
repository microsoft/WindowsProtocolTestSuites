// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using System;
using System.Collections.Generic;

namespace Microsoft.Protocols.TestTools.StackSdk.RemoteDesktop.Rdpegfx
{
    /// <summary>
    /// This class provides the operation for bit stream 
    /// </summary>
    public class BitStream
    {
        List<bool> bitList = new List<bool>();

        /// <summary>
        /// Write one bit into stream
        /// </summary>
        /// <param name="bit">write 1 if this value is true, otherwise 0.</param>
        public void WriteBit(bool bit)
        {
            bitList.Add(bit);
        }

        /// <summary>
        /// Write bits into stream.
        /// </summary>
        /// <param name="count">The count of bits to be written in.</param>
        /// <param name="bit">write 1 if this value is true, otherwise 0.</param>
        public void WriteBits(int count, bool bit)
        {
            bool[] bits = new bool[count];
            if (bit)
            {
                for (int i = 0; i < count; i++) bits[i] = bit;
            }
            bitList.AddRange(bits);
        }
        
        /// <summary>
        /// Convert an int value to bits and write into this stream.
        /// </summary>
        /// <param name="bitCount">The count of bits to be written in.</param>
        /// <param name="value">The specified int value.</param>
        public void WriteBits(int bitCount, int value)
        {
            if (bitCount == 0) return;
            value = Math.Abs(value);
            for (int pos = bitCount - 1; pos >= 0; pos--)
            {
                int mask = 1 << pos;
                int bitV = mask & value;
                WriteBit(bitV > 0 ? true : false);
            }
        }

        /// <summary>
        /// Covert this stream to byte array.
        /// </summary>
        /// <returns>The result byte array.</returns>
        public byte[] ToBytes()
        {
            if (bitList.Count == 0) return null;
            int bCount = bitList.Count;
            int byteCount = (bCount - 1) / 8 + 1;
            int leftBitCount = bCount % 8;
            if (leftBitCount != 0)
            {
                WriteBits(8 - leftBitCount, false);
            }
            byte[] bytes = new byte[byteCount];
            for (int i = 0; i < byteCount; i++)
            {
                bool[] byteBits = bitList.GetRange(i * 8, 8).ToArray();
                bytes[i] =
                    (byte)
                    (
                        (byteBits[0] ? 1 << 7 : 0) |
                        (byteBits[1] ? 1 << 6 : 0) |
                        (byteBits[2] ? 1 << 5 : 0) |
                        (byteBits[3] ? 1 << 4 : 0) |
                        (byteBits[4] ? 1 << 3 : 0) |
                        (byteBits[5] ? 1 << 2 : 0) |
                        (byteBits[6] ? 1 << 1 : 0) |
                        (byteBits[7] ? 1 : 0)
                    );
            }
            return bytes;

        }

        private int toInt(bool bValue)
        {
            return bValue ? 1 : 0;
        }
         
        /// <summary>
        /// Create a bit stream from a given byte array 
        /// </summary>
        /// <param name="input">The given byte array.</param>
        /// <returns>A bit stream created from the given byte array.</returns>
        public static BitStream GetFromBytes(byte[] input)
        {
            BitStream bitStream = new BitStream();
            if (input == null) return bitStream;

            foreach(byte b in input)
            {
                bitStream.WriteBits(8, b);
            }
            return bitStream;
        }

        /// <summary>
        /// Read an int value from this bit stream.
        /// </summary>
        /// <param name="bitCount">The count of bits to be read.</param>
        /// <param name="output">The output int value.</param>
        /// <returns>Indicates if the read operation succeed.</returns>
        public bool ReadInt32(int bitCount, out int output)
        {
            output = 0;
            if (bitCount == 0) return false;
            if (bitCount > bitList.Count) return false;
            while (bitCount > 0)
            {
                output = output << 1;
                if (bitList[0]) output = output + 1;
                bitList.RemoveAt(0);
                bitCount--;
            }
            return true;
        }
    }
}
