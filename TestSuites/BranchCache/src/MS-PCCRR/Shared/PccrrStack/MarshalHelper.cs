// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Microsoft.Protocols.TestTools.StackSdk.BranchCache.Pccrr
{
    using System;
    using System.Text;

    /// <summary>
    /// This class is used to marshal simple types.
    /// </summary>
    public static class MarshalHelper
    {
        /// <summary>
        /// convert value to bytes
        /// </summary>
        /// <param name="value">a UInt16 value</param>
        /// <param name="isBigEndian">order of bytes. If it's big endian, set it to true, else set it to false.</param>
        /// <returns>result of bytes</returns>
        public static byte[] GetBytes(ushort value, bool isBigEndian)
        {
            byte[] ret = BitConverter.GetBytes(value);
            ReverseByFlag(isBigEndian, ret);
            return ret;
        }

        /// <summary>
        /// convert value to bytes
        /// </summary>
        /// <param name="value">a UInt32 value</param>
        /// <param name="isBigEndian">order of bytes. If it's big endian, set it to true, else set it to false.</param>
        /// <returns>result of bytes</returns>
        public static byte[] GetBytes(uint value, bool isBigEndian)
        {
            byte[] ret = BitConverter.GetBytes(value);
            ReverseByFlag(isBigEndian, ret);
            return ret;
        }

        /// <summary>
        /// convert DateTime to bytes, the encoding is the seconds from 19700101
        /// </summary>
        /// <param name="value">a DateTime value</param>
        /// <param name="isBigEndian">order of bytes. If it's big endian, set it to true, else set it to false.</param>
        /// <returns>result of bytes</returns>
        public static byte[] GetBytes(DateTime value, bool isBigEndian)
        {
            DateTime startDt = new DateTime(1970, 1, 1);
            byte[] ret = BitConverter.GetBytes((uint)(value - startDt).TotalSeconds);
            ReverseByFlag(isBigEndian, ret);
            return ret;
        }

        /// <summary>
        /// reverse the bytes if it's not big endian.
        /// </summary>
        /// <param name="isBigEndian">order of bytes. If it's big endian, set it to true, else set it to false.</param>
        /// <param name="ret">the bytes will be re-order</param>
        public static void ReverseByFlag(bool isBigEndian, byte[] ret)
        {
            if (!isBigEndian)
            {
                Array.Reverse(ret);
            }
        }

        /// <summary>
        /// parse UInt16 from bytes
        /// </summary>
        /// <param name="buffer">the buffer stores the value.</param>
        /// <param name="index">the index of start to parse</param>
        /// <param name="isBigEndian">order of bytes. If it's big endian, set it to true, else set it to false.</param>
        /// <returns>Parsed UInt16 value</returns>
        public static ushort GetUInt16(byte[] buffer, ref int index, bool isBigEndian)
        {
            ushort ushRet = 0;
            byte[] ret = BitConverter.GetBytes(ushRet);

            Array.Copy(buffer, index, ret, 0, ret.Length);
            ReverseByFlag(isBigEndian, ret);
            ushRet = BitConverter.ToUInt16(ret, 0);
            index += ret.Length;

            return ushRet;
        }

        /// <summary>
        /// parse UInt32 from bytes
        /// </summary>
        /// <param name="buffer">the buffer stores the value.</param>
        /// <param name="index">the index of start to parse</param>
        /// <param name="isBigEndian">order of bytes. If it's big endian, set it to true, else set it to false.</param>
        /// <returns>Parsed UInt32 value</returns>
        public static uint GetUInt32(byte[] buffer, ref int index, bool isBigEndian)
        {
            uint ret = 0;
            byte[] temp = BitConverter.GetBytes(ret);

            Array.Copy(buffer, index, temp, 0, temp.Length);
            ReverseByFlag(isBigEndian, temp);
            ret = BitConverter.ToUInt32(temp, 0);
            index += temp.Length;

            return ret;
        }

        /// <summary>
        /// parse UInt32 from bytes
        /// </summary>
        /// <param name="buffer">the buffer stores the value.</param>
        /// <param name="index">the index of start to parse</param>
        /// <param name="isBigEndian">order of bytes. If it's big endian, set it to true, else set it to false.</param>
        /// <returns>Parsed UInt32 value</returns>
        public static uint GetUInt32(byte[] buffer, int index, bool isBigEndian)
        {
            uint ret = 0;
            byte[] temp = BitConverter.GetBytes(ret);

            Array.Copy(buffer, index, temp, 0, temp.Length);
            ReverseByFlag(isBigEndian, temp);
            ret = BitConverter.ToUInt32(temp, 0);

            return ret;
        }

        /// <summary>
        /// parse UInt64 from bytes
        /// </summary>
        /// <param name="buffer">the buffer stores the value.</param>
        /// <param name="index">the index of start to parse</param>
        /// <param name="isBigEndian">order of bytes. If it's big endian, set it to true, else set it to false.</param>
        /// <returns>Parsed UInt32 value</returns>
        public static ulong GetUInt64(byte[] buffer, ref int index, bool isBigEndian)
        {
            ulong ret = 0;
            byte[] temp = BitConverter.GetBytes(ret);

            Array.Copy(buffer, index, temp, 0, temp.Length);
            ReverseByFlag(isBigEndian, temp);
            ret = BitConverter.ToUInt64(temp, 0);
            index += temp.Length;

            return ret;
        }

        /// <summary>
        /// Transfer a byte array to a binary string.
        /// </summary>
        /// <param name="buffer">The input byte array</param>
        /// <returns>Binary String from the buffer</returns>
        public static string ByteArrayToString(byte[] buffer)
        {
            StringBuilder sb = new StringBuilder();
            string tempString = string.Empty;
            for (int i = 0; i < buffer.Length; i++)
            {
                tempString = Convert.ToString(buffer[i], 16).ToUpper();

                if (buffer[i] < 16)
                {
                    tempString = "0" + tempString;
                }

                sb.Append(tempString);
            }

            return sb.ToString();
        }

        /// <summary>
        /// Get UInt32 data from buffer, and transfer it to DateTime
        /// </summary>
        /// <param name="buffer">the buffer stores the value.</param>
        /// <param name="index">the index of start to parse</param>
        /// <param name="isBigEndian">order of bytes. If it's big endian, set it to true, else set it to false.</param>
        /// <returns>DateTime value</returns>
        public static DateTime GetDateTime(byte[] buffer, ref int index, bool isBigEndian)
        {
            DateTime ret = new DateTime(1970, 1, 1);
            byte[] temp = new byte[4];
            uint seconds;

            Array.Copy(buffer, index, temp, 0, temp.Length);
            ReverseByFlag(isBigEndian, temp);
            seconds = BitConverter.ToUInt32(temp, 0);
            ret = ret.AddSeconds(seconds);

            index += 4;

            return ret;
        }

        /// <summary>
        /// get bytes from bytes
        /// </summary>
        /// <param name="buffer">the buffer stores the value.</param>
        /// <param name="index">the index of start to parse</param>
        /// <param name="count">count of bytes</param>
        /// <returns>Parsed UInt16 value</returns>
        public static byte[] GetBytes(byte[] buffer, ref int index, int count)
        {
            byte[] ret = null;

            if (count > 0)
            {
                ret = new byte[count];

                Array.Copy(buffer, index, ret, 0, ret.Length);
                index += ret.Length;
            }

            return ret;
        }
    }
}
