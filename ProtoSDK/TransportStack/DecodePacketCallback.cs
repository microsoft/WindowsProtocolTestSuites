// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Microsoft.Protocols.TestTools.StackSdk.Transport
{
    /// <summary>
    /// to decode stack packet from the received message bytes.
    /// </summary>
    /// <param name="endPoint">
    /// an object that specifies the endpoint of message.<para/>
    /// if TcpClient, it's an IPEndPoint that specifies the local endpoint.<para/>
    /// if TcpServer, it's an IPEndPoint that specifies the remote endpoint.<para/>
    /// if Netbios, it's an int value that specifies the remote endpoint.<para/>
    /// if Udp, it's an IPEndPoint that specifies the remote endpoint.<para/>
    /// if Stream, it's an int value that identity the stream.
    /// </param>
    /// <param name="messageBytes">
    /// a bytes array that contains the received message bytes to be decoded.
    /// </param>
    /// <param name="consumedLength">
    /// return an int value that specifies the length of message bytes consumed by decoder.
    /// </param>
    /// <param name="expectedLength">
    /// return an int value that specifies the length of message bytes the decoder expects to receive.
    /// </param>
    /// <returns>
    /// a StackPacket array that contains the stack packets decoded from the received message bytes.<para/>
    /// if no packet is decoded, return either null or StackPacket[0].<para/>
    /// otherwise, return more than one packet in array.
    /// </returns>
    public delegate StackPacket[] DecodePacketCallback(
        object endPoint, byte[] messageBytes, out int consumedLength, out int expectedLength);
}
