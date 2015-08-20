// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Text;
using System.Collections.Generic;

using Microsoft.Protocols.TestTools.StackSdk.Messages;

namespace Microsoft.Protocols.TestTools.StackSdk
{
    /// <summary>
    /// A helper class to assist channel class
    /// </summary>
    public static class ChannelHelper
    {
        /// <summary>
        /// The size of Unicode character
        /// </summary>
        private const int UNICODE_CHAR_SIZE = 2;

        /// <summary>
        /// Read null terminated Unicode string buffer from the current position of channel
        /// </summary>
        /// <param name="channel">The PTF channel</param>
        /// <returns>The read string buffer, null terminator included</returns>
        /// <exception cref="System.InvalidOperationException">Thrown when can't get the string buffer</exception>
        public static byte[] ReadNtUnicodeStringBuffer(Channel channel)
        {
            List<ushort> ntStringList = new List<ushort>();

            while (channel.Stream.Position < channel.Stream.Length)
            {
                ushort temp = channel.Read<ushort>();
                ntStringList.Add(temp);
                if (temp == 0)
                {
                    break;
                }
            }

            byte[] stringBuffer = new byte[ntStringList.Count * UNICODE_CHAR_SIZE];

            for (int i = 0; i < ntStringList.Count; i++)
            {
                stringBuffer[i * 2] = (byte)(ntStringList[i] & 0xff);
                stringBuffer[i * 2 + 1] = (byte)((ntStringList[i] >> 8) & 0xff);
            }

            if (stringBuffer.Length > 1)
            {
                //check the last two bytes to make sure they are 0
                if (stringBuffer[stringBuffer.Length - 1] != 0 || stringBuffer[stringBuffer.Length - 2] != 0)
                {
                    throw new InvalidOperationException(
                        "Reach the end of the channel stream, but still can't find the terminator");
                }
                else
                {
                    return stringBuffer;
                }
            }
            else
            {
                throw new InvalidOperationException(
                    "Reach the end of the channel stream, but still can't find the terminator");
            }
        }


        /// <summary>
        /// Read null terminated ASCII string buffer from the current position of channel
        /// </summary>
        /// <param name="channel">The ptf channel</param>
        /// <returns>The read string buffer, null terminator included</returns>
        /// <exception cref="System.InvalidOperationException">Thrown when can't get the string buffer</exception>
        public static byte[] ReadNtAsciiStringBuffer(Channel channel)
        {
            List<byte> ntStringList = new List<byte>();

            while (channel.Stream.Position < channel.Stream.Length)
            {
                byte temp = channel.Read<byte>();
                ntStringList.Add(temp);
                if (temp == 0)
                {
                    return ntStringList.ToArray();
                }
            }

            throw new InvalidOperationException(
                "Reach the end of the channel stream, but still can't find the terminator");
        }


        /// <summary>
        /// Read null terminated Unicode string from the current position of channel
        /// </summary>
        /// <param name="channel">The ptf channel</param>
        /// <returns>The read string, when read from the channel, the null terminator will be consumed,
        /// but it will not be included in the return string</returns>
        /// <exception cref="System.InvalidOperationException">Thrown when can't get the string</exception>
        public static string ReadNtUnicodeString(Channel channel)
        {
            byte[] stringBuffer = ReadNtUnicodeStringBuffer(channel);

            // '\0' is not returned
            return Encoding.Unicode.GetString(stringBuffer).TrimEnd('\0');
        }


        /// <summary>
        /// Read null terminated ascii string from the current position of channel
        /// </summary>
        /// <param name="channel">The ptf channel</param>
        /// <returns>The read string, when read from the channel, the null terminator will be consumed,
        /// but it will not be included in the return string</returns>
        /// <exception cref="System.InvalidOperationException">Thrown when can't get the string</exception>
        public static string ReadNtAsciiString(Channel channel)
        {
            byte[] stringBuffer = ReadNtAsciiStringBuffer(channel);

            // '\0' is not returned
            return Encoding.ASCII.GetString(stringBuffer).TrimEnd('\0');
        }


        /// <summary>
        /// Verify if there is no data can be read in the channel
        /// </summary>
        /// <param name="channel">The ptf channel</param>
        /// <exception cref="System.InvalidOperationException">Thrown when there is data can be read</exception>
        public static void VerifyChannelEnd(Channel channel)
        {
            if (channel.Stream.Position != channel.Stream.Length)
            {
                throw new InvalidOperationException("The stream is not well formatted.");
            }
        }
    }
}
