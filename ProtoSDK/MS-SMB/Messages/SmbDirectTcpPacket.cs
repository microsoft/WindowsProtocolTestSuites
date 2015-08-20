// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;

using Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Cifs;

namespace Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Smb
{
    /// <summary>
    /// the packet for direct tcp transport. its ToBytes return 4 bytes contains the size. 
    /// </summary>
    internal class SmbDirectTcpPacket : StackPacket
    {
        /// <summary>
        /// the packet to send through TCP. 
        /// </summary>
        private SmbPacket packet;

        /// <summary>
        /// the transport 
        /// </summary>
        /// <param name = "packet">the packet to send through TCP </param>
        public SmbDirectTcpPacket(SmbPacket packet)
        {
            this.packet = packet;
        }


        /// <summary>
        /// to clone this packet 
        /// </summary>
        /// <returns>the cloned packet </returns>
        /// <exception cref="NotSupportedException">the tcp transport can not be clone!</exception>
        public override StackPacket Clone()
        {
            throw new NotSupportedException("the tcp transport can not be clone!");
        }


        /// <summary>
        /// get the length of tcp packet. decode the first 4 bytes to get the length. 
        /// </summary>
        /// <param name = "bytes">the bytes contains the tcp data </param>
        /// <returns>the length of the tcp packet. </returns>
        /// <exception cref="ArgumentNullException">bytes can not be null</exception>
        public int GetTcpPacketLength(byte[] bytes)
        {
            if (bytes == null)
            {
                throw new ArgumentNullException("bytes");
            }
            // the bit order of bytes need to reset.
            return BitConverter.ToInt32(new byte[] { bytes[3], bytes[2], bytes[1],  0 }, 0);
        }


        /// <summary>
        /// get the data of tcp packet. 
        /// </summary>
        /// <param name = "bytes">the bytes contains the transport header and data </param>
        /// <returns>byte array of transportation payload that over TCP layer.</returns>
        public byte[] GetTcpData(byte[] bytes)
        {
            byte[] data = new byte[bytes.Length - TcpTransportHeaderSize];
            Array.Copy(bytes, TcpTransportHeaderSize, data, 0, data.Length);

            return data;
        }


        /// <summary>
        /// If the packet is an invalid packet. If true, the packet must not be parsed. 
        /// </summary>
        /// <param name = "bytes">the bytes of packet </param>
        /// <returns>whether the tcp packet is invalid </returns>
        public bool IsPacketInvalid(byte[] bytes)
        {
            return bytes == null || bytes.Length < TcpTransportHeaderSize;
        }


        /// <summary>
        /// is the packet has a continuous packet. if true, the packet should not be parsed. 
        /// </summary>
        /// <param name = "bytes">the packet bytes. </param>
        /// <returns>whether the packet has a continuous packet. </returns>
        public bool HasContinuousPacket(byte[] bytes)
        {
            return bytes.Length < TcpTransportHeaderSize + GetTcpPacketLength(bytes);
        }


        /// <summary>
        /// get the size of tcp transport header. it must be 4. 
        /// </summary>
        public int TcpTransportHeaderSize
        {
            get
            {
                return 4;
            }
        }


        /// <summary>
        /// marshal the packet to bytes array.
        /// add TransportHeader(Zero StreamProtocolLength), if not set manually.
        /// </summary>
        /// <returns>the bytes of packet </returns>
        public override byte[] ToBytes()
        {
            byte[] packetBytes = packet.ToBytes();

            return ArrayUtility.ConcatenateArrays(CifsMessageUtils.ToBytes(packet.TransportHeader), packetBytes);
        }


    }
}
