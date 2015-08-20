// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;

namespace Microsoft.Protocols.TestTools.StackSdk.Transport
{
    /// <summary>
    /// this class is used to decode the data buffer of connection.<para/>
    /// such as TcpServerConnection, NetbiosServerConnection, Stream, TcpClient and NetbiosClient.<para/>
    /// the format of name of class: {Function}[AddtionalInfomation]Visitor.
    /// the AddtionalInfomation is optional.
    /// </summary>
    internal class ExpectBytesVisitor
    {
        /// <summary>
        /// to receive bytes from connection.
        /// </summary>
        /// <param name="buffer">
        /// a BytesBuffer object that contains the received data from endpoint.
        /// </param>
        /// <param name="timeout">
        /// a TimeSpan object that specifies the timeout for this operation.
        /// </param>
        /// <param name="maxCount">
        /// an int value that specifies the maximum count of expect bytes.
        /// </param>
        /// <returns>
        /// a bytes array that contains the received bytes.
        /// </returns>
        /// <exception cref="InvalidOperationException">
        /// thrown when the connection is closed, there is no data anymore.
        /// </exception>
        public static byte[] Visit(BytesBuffer buffer, TimeSpan timeout, int maxCount)
        {
            bool bufferClosed;
            // get specified length data in buffer, at-least one byte.
            byte[] data = buffer.Read(timeout, maxCount, 0, out bufferClosed);

            if (data.Length == 0)
            {
                throw new InvalidOperationException("the connection is closed, there is no data anymore.");
            }

            buffer.Remove(data.Length);

            return data;
        }
    }

    /// <summary>
    /// this class is used to decode the data buffer of connection.<para/>
    /// such as Stream, TcpClient and NetbiosClient.<para/>
    /// the format of name of class: {Function}[AddtionalInfomation]Visitor.
    /// the AddtionalInfomation is optional.
    /// </summary>
    internal class ExpectSinglePacketVisitor
    {
        /// <summary>
        /// expect packet from transport.
        /// </summary>
        /// <param name="buffer">
        /// a BytesBuffer object that contains the received data from endpoint.
        /// </param>
        /// <param name="decoder">
        /// a DecodePacketCallback delegate that is used to decode packet from buffer.
        /// </param>
        /// <param name="endpoint">
        /// an object that specifies the endpoint for decoder.<para/>
        /// remember: this endpoint must be the endpoint that is returned to the user,
        /// that is the endpoint return by Connect().
        /// </param>
        /// <param name="timeout">
        /// a TimeSpan object that indicates the timeout to expect event.
        /// </param>
        /// <param name="packetCache">
        /// a list that contains the stackpackets.
        /// </param>
        /// <returns>
        /// a StackPacket object that specifies the received packet.
        /// </returns>
        public static StackPacket Visit(
            BytesBuffer buffer, DecodePacketCallback decoder, object endpoint,
            TimeSpan timeout, SyncFilterQueue<StackPacket> packetCache)
        {
            // get the packet in packet list.
            if (packetCache.Count > 0)
            {
                return Utility.GetOne<StackPacket>(packetCache, null);
            }

            // the consumed length of decoder.
            int consumedLength = 0;

            // decode packet, return null or at least one packet.
            StackPacket[] packets = ExpectMultiPacketsVisitor.Visit(
                buffer, decoder, endpoint, timeout, out consumedLength);

            // packet is null, buffer is closed, no packet will come.
            if (packets == null)
            {
                return null;
            }

            // if packet arrived, add to packet list, and return the first.
            foreach (StackPacket packet in packets)
            {
                packetCache.Enqueue(packet);
            }

            return Utility.GetOne<StackPacket>(packetCache, null);
        }
    }

    /// <summary>
    /// this class is used to decode the data buffer of connection.<para/>
    /// such as TcpServerConnection and NetbiosServerConnection.<para/>
    /// the format of name of class: {Function}[AddtionalInfomation]Visitor.
    /// the AddtionalInfomation is optional.
    /// </summary>
    internal class ExpectMultiPacketsVisitor
    {
        /// <summary>
        /// expect packet from transport.
        /// </summary>
        /// <param name="buffer">
        /// a BytesBuffer object that contains the received data from endpoint.
        /// </param>
        /// <param name="decoder">
        /// a DecodePacketCallback delegate that is used to decode packet from buffer.
        /// </param>
        /// <param name="endpoint">
        /// an object that specifies the endpoint for decoder.<para/>
        /// remember: this endpoint is the identity endpoint of tcp/netbios connection,
        /// that is the remote endpoint of connection.
        /// </param>
        /// <param name="timeout">
        /// a TimeSpan object that indicates the timeout to expect event.
        /// </param>
        /// <param name="consumedLength">
        /// return an int value that specifies the consumed length.
        /// </param>
        /// <returns>
        /// a StackPacket object that specifies the received packet.
        /// </returns>
        public static StackPacket[] Visit(
            BytesBuffer buffer, DecodePacketCallback decoder, object endpoint,
            TimeSpan timeout, out int consumedLength)
        {
            bool bufferClosed;
            // get all data in buffer, at-least one byte.
            byte[] data = buffer.Read(timeout, BytesBuffer.MaxCount, 0, out bufferClosed);

            // the end time for operation.
            DateTime endTime = DateTime.Now + timeout;

            while (true)
            {
                // decode packets using data in buffer.
                int expectedLength = 0;

                // decode data.
                if (data.Length > 0)
                {
                    StackPacket[] packets = decoder(endpoint, data, out consumedLength, out expectedLength);

                    buffer.Remove(consumedLength);

                    // if packet arrived, add to packet list, and return the first.
                    if (packets != null && packets.Length > 0)
                    {
                        return packets;
                    }
                }

                // if buffer is closed, no data will come, return null.
                if (bufferClosed)
                {
                    consumedLength = 0;
                    return null;
                }

                // wait for the next data coming.
                data = buffer.Read(endTime - DateTime.Now, BytesBuffer.MaxCount, data.Length, out bufferClosed);
            }
        }
    }
}
