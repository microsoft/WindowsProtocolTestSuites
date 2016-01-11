// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Net;

namespace Microsoft.Protocols.TestTools.StackSdk.ActiveDirectory.Adts
{
    internal abstract class AdtsLdapDecoderCallbackBase
    {
        /// <summary>
        /// Decodes LDAP packets.
        /// </summary>
        /// <param name="endPoint">The end point of the client.</param>
        /// <param name="messageBytes">The message bytes that contains the packet data.</param>
        /// <param name="consumedLength">Consumed length.</param>
        /// <param name="expectedLength">
        /// Indicates expected length if the message bytes doesn't contain all packet data.
        /// </param>
        /// <returns>Decoded packets.</returns>
        internal abstract StackPacket[] DecodeLdapPacketCallBack(
            object endPoint,
            byte[] messageBytes,
            out int consumedLength,
            out int expectedLength);


        /// <summary>
        /// Gets packet data for a single packet.
        /// </summary>
        /// <param name="messageBytes">The message bytes that contains the packet data.</param>
        /// <param name="consumedLength">Consumed length.</param>
        /// <param name="expectedLength">
        /// Indicates expected length if the message bytes doesn't contain all packet data.
        /// </param>
        /// <returns>
        /// The single packet data. If the messageBytes doesn't contain enough data, null is returned.
        /// </returns>
        internal byte[] GetSinglePacketData(
            byte[] messageBytes,
            out int consumedLength,
            out int expectedLength)
        {
            // The flag for T-L-V form data.
            const byte TlvFlag = 0x80;
            // The flag which gets "lengthOfLength" in "L" of T-L-V form data.
            const byte LengthFlag = 0x0f;
            // Min length of packet, at least length-related data should be contained.
            const int MinLength = 2;

            if (messageBytes == null || messageBytes.Length < MinLength)
            {
                consumedLength = 0;
                expectedLength = MinLength;
                return null;
            }

            // Gets the length of packet length data in the TLV data.
            int lengthOfPacketLen = messageBytes[1];
            int wholePacketLength = 0;

            // The first byte is the asn1 start tag, ignored.
            if ((messageBytes[1] & TlvFlag) == TlvFlag)
            {
                lengthOfPacketLen = messageBytes[1] & LengthFlag;
                // For Asn1Integer tag, the length data should not exceed sizeof(int) bytes.
                if (lengthOfPacketLen > sizeof(int))
                {
                    throw new StackException("Invalid length in T-L-V data.");
                }
                int tlvLength = MinLength + lengthOfPacketLen;
                if (messageBytes.Length < tlvLength)
                {
                    consumedLength = 0;
                    expectedLength = tlvLength;
                    return null;
                }

                // get the packet length
                byte[] lengthBytes = ArrayUtility.SubArray<byte>(messageBytes, MinLength, lengthOfPacketLen);
                // if the bytes does not contains the enough bytes of int, padding with zero.
                if(lengthBytes == null || lengthBytes.Length < sizeof(int))
                {
                    byte[] standardInt32Bytes = new byte[sizeof(int)];
                    Array.Copy(lengthBytes, 0,
                        standardInt32Bytes, standardInt32Bytes.Length - lengthBytes.Length,
                        lengthBytes.Length);
                    lengthBytes = standardInt32Bytes;
                }
                int networkOrderLength = BitConverter.ToInt32(lengthBytes, 0);

                // Packet length without header and TLV data.
                int remainingLength = IPAddress.NetworkToHostOrder(networkOrderLength);
                wholePacketLength = MinLength + lengthOfPacketLen + remainingLength;
            }
            else
            {
                // Non-TLV
                wholePacketLength = MinLength + lengthOfPacketLen;
            }

            // Packet data is not adequate, wait for whole packet data.
            if (messageBytes.Length < wholePacketLength)
            {
                consumedLength = 0;
                expectedLength = wholePacketLength;
                return null;
            }

            byte[] packetData = messageBytes;
            if (messageBytes.Length > wholePacketLength)
            {
                packetData = ArrayUtility.SubArray<byte>(messageBytes, 0, wholePacketLength);
            }
            consumedLength = wholePacketLength;
            expectedLength = 0;

            return packetData;
        }

    }
}
