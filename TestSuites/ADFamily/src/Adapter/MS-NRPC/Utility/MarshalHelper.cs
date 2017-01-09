// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc
{
    using System;
    using System.Globalization;
    using System.Collections.Generic;
    using System.Text;

    /// <summary>
    /// This class is used to marshal simple types.
    /// </summary>
    public static partial class MarshalHelper
    {
        /// <summary>
        /// Reverse the bytes if it's not big endian.
        /// </summary>
        /// <param name="isBigEndian">Order of bytes. If it's big endian, set it to true, else set it to false.</param>
        /// <param name="byteTemp">The bytes will be re-order</param>
        public static void ReverseByFlag(bool isBigEndian, byte[] byteTemp)
        {
            if (!isBigEndian)
            {
                Array.Reverse(byteTemp);
            }
        }

        /// <summary>
        /// Convert value to bytes
        /// </summary>
        /// <param name="value">A UInt16 value</param>
        /// <param name="isBigEndian">Order of bytes. If it's big endian, set it to true, else set it to false.</param>
        /// <returns>Result of bytes</returns>
        public static byte[] GetBytes(ushort value, bool isBigEndian)
        {
            byte[] byteTemp = BitConverter.GetBytes(value);
            ReverseByFlag(isBigEndian, byteTemp);
            return byteTemp;
        }

        /// <summary>
        /// Convert value to bytes
        /// </summary>
        /// <param name="value">A UInt32 value</param>
        /// <param name="isBigEndian">Order of bytes. If it's big endian, set it to true, else set it to false.</param>
        /// <returns>Result of bytes</returns>
        public static byte[] GetBytes(uint value, bool isBigEndian)
        {
            byte[] byteTemp = BitConverter.GetBytes(value);
            ReverseByFlag(isBigEndian, byteTemp);
            return byteTemp;
        }

        /// <summary>
        /// Convert value to bytes
        /// </summary>
        /// <param name="value">A UInt64 value</param>
        /// <param name="isBigEndian">Order of bytes. If it's big endian, set it to true, else set it to false.</param>
        /// <returns>Result of bytes</returns>
        public static byte[] GetBytes(ulong value, bool isBigEndian)
        {
            byte[] byteTemp = BitConverter.GetBytes(value);
            ReverseByFlag(isBigEndian, byteTemp);
            return byteTemp;
        }

        /// <summary>
        /// Convert value to bytes
        /// </summary>
        /// <param name="shortArray">A ushort array</param>
        /// <param name="isBigEndian">Order of bytes. If it's big endian, set it to true, else set it to false.</param>
        /// <returns>Result of bytes</returns>
        public static byte[] GetBytes(ushort[] shortArray, bool isBigEndian)
        {            
            List<byte> tempBuffer = new List<byte>();

            for (int i = 0; i < shortArray.Length; i++)
            {
                tempBuffer.AddRange(GetBytes(shortArray[i], isBigEndian));
            }

            return tempBuffer.ToArray();
        }

        /// <summary>
        /// Convert value to bytes
        /// </summary>
        /// <param name="value">A UInt32 array</param>
        /// <param name="isBigEndian">Order of bytes. If it's big endian, set it to true, else set it to false.</param>
        /// <returns>Result of bytes</returns>
        public static byte[] GetBytes(uint[] value, bool isBigEndian)
        {
            List<byte> tempBuffer = new List<byte>();

            for (int i = 0; i < value.Length; i++)
            {
                tempBuffer.AddRange(GetBytes(value[i], isBigEndian));
            }

            return tempBuffer.ToArray();
        }

        /// <summary>
        /// Convert the ushort array to Unicode string
        /// </summary>
        /// <param name="shortArray">The ushort array to be convert</param>
        /// <param name="isBigEndian">Order of bytes. If it's big endian, set it to true, else set it to false.</param>
        /// <returns>The string from the ushort array</returns>
        public static string ShortArrayToString(ushort[] shortArray, bool isBigEndian)
        {
            string strRet = string.Empty;
            List<byte> tempBuffer = new List<byte>();
            for (int i = 0; i < shortArray.Length; i++)
            {
                tempBuffer.AddRange(GetBytes(shortArray[i], isBigEndian));
            }

            byte[] tempBytes = tempBuffer.ToArray();
            strRet = Encoding.Unicode.GetString(tempBytes);
            strRet = strRet.Trim(new char[] { Convert.ToChar(0x00) });
            return strRet;
        }

        /// <summary>
        /// String to byte array with static size
        /// </summary>
        /// <param name="str">The string to be convert</param>
        /// <param name="staticSize">The static size of the byte array</param>
        /// <returns>Byte array of staticSize length</returns>
        public static byte[] GetBytes(string str, int staticSize)
        {
            byte[] byteRet = new byte[staticSize];
            byte[] tempBytes = Encoding.Unicode.GetBytes(str);
            Array.Copy(tempBytes, 0, byteRet, 0, tempBytes.Length);
            return byteRet;
        }

        /// <summary>
        /// Convert a string to ushort array.
        /// </summary>
        /// <param name="input">The byte[] to be convert</param>
        /// <param name="isBigEndian">Order of bytes. If it's big endian, set it to true, else set it to false.</param>
        /// <returns>The ushort array from string</returns>
        public static ushort[] GetUshortArray(byte[] input, bool isBigEndian)
        {
            List<ushort> ushortReturn = new List<ushort>();

            for (int i = 0; i < input.Length / 2; i++)
            {
                if (isBigEndian)
                {
                    ushortReturn.Add((ushort)((input[i * 2] * 0x100) + input[(i * 2) + 1]));
                }
                else
                {
                    ushortReturn.Add((ushort)(input[i * 2] + (input[(i * 2) + 1] * 0x100)));
                }
            }

            return ushortReturn.ToArray();
        }        

        /// <summary>
        /// Parse UInt16 from bytes
        /// </summary>
        /// <param name="buffer">The buffer stores the value.</param>
        /// <param name="index">The index of start to parse</param>
        /// <param name="isBigEndian">Order of bytes. If it's big endian, set it to true, else set it to false.</param>
        /// <returns>Parsed UInt16 value</returns>
        public static ushort GetUInt16(byte[] buffer, ref int index, bool isBigEndian)
        {
            ushort ushortRet = 0;
            byte[] byteTemp = BitConverter.GetBytes(ushortRet);

            Array.Copy(buffer, index, byteTemp, 0, byteTemp.Length);
            ReverseByFlag(isBigEndian, byteTemp);
            ushortRet = BitConverter.ToUInt16(byteTemp, 0);
            index += byteTemp.Length;

            return ushortRet;
        }

        /// <summary>
        /// Parse UInt32 from bytes
        /// </summary>
        /// <param name="buffer">The buffer stores the value.</param>
        /// <param name="index">The index of start to parse</param>
        /// <param name="isBigEndian">Order of bytes. If it's big endian, set it to true, else set it to false.</param>
        /// <returns>Parsed UInt32 value</returns>
        public static uint GetUInt32(byte[] buffer, ref int index, bool isBigEndian)
        {
            uint uintRet = 0;
            byte[] byteTemp = BitConverter.GetBytes(uintRet);

            Array.Copy(buffer, index, byteTemp, 0, byteTemp.Length);
            ReverseByFlag(isBigEndian, byteTemp);
            uintRet = BitConverter.ToUInt32(byteTemp, 0);
            index += byteTemp.Length;

            return uintRet;
        }

        /// <summary>
        /// Convert a string to ushort array.
        /// </summary>
        /// <param name="str">The string to be convert</param>
        /// <param name="isBigEndian">Order of bytes. If it's big endian, set it to true, else set it to false.</param>
        /// <returns>The ushort array from string</returns>
        public static ushort[] GetUshortArray(string str, bool isBigEndian)
        {
            List<ushort> list = new List<ushort>();
            byte[] tempBytes = Encoding.Unicode.GetBytes(str);
            int index = 0;
            while (index != tempBytes.Length)
            {
                list.Add(GetUInt16(tempBytes, ref index, isBigEndian));
            }

            return list.ToArray();
        }        

        /// <summary>
        /// Get the sub bytes of byte array.
        /// </summary>
        /// <param name="src">The src array</param>
        /// <param name="start">The start index to sub bytes</param>
        /// <param name="size">The size of sub bytes</param>
        /// <returns>The sub bytes</returns>
        /// <exception cref="ArgumentNullException">The src must not be null</exception>
        /// <exception cref="ArgumentException">The start must not greater than the Length of src array</exception>
        /// <exception cref="ArgumentException">The sub array index is overflow!</exception>
        /// <exception cref="ArgumentException">Size should not be negative</exception>
        /// <exception cref="ArgumentException">Start should not be negative</exception>
        public static byte[] SubBytes(
            byte[] src,
            int start,
            int size)
        {
            if (src == null)
            {
                throw new ArgumentNullException("src");
            }

            if (src.Length <= start)
            {
                throw new ArgumentException("the start must not greater than the Length of src array", "start");
            }

            if (src.Length < start + size)
            {
                throw new ArgumentException(
                    string.Format(CultureInfo.InvariantCulture,
                    "the sub array index is overflow! total is {0}, start is {1}, size is {2}",
                    src.Length,
                    start,
                    size), 
                    "src");
            }

            if (size < 0)
            {
                throw new ArgumentException("size should not be negative", "size");
            }

            if (start < 0)
            {
                throw new ArgumentException("start should not be negative", "start");
            }

            byte[] ret = new byte[size];
            Array.Copy(src, start, ret, 0, size);
            
            return ret;
        }
    }    
}