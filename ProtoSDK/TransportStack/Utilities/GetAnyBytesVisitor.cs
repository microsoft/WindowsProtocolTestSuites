// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;

namespace Microsoft.Protocols.TestTools.StackSdk.Transport
{
    /// <summary>
    /// the interfaces of multiple connection implementation.<para/>
    /// such as TcpServerTransport and NetbiosServerTransport. 
    /// these classes manage multiple connection to get packet from any client.<para/>
    /// the format of name of interface: IVisitor{FunctionName}.<para/>
    /// the format of method of interface: Visitor{FunctionUsage}.
    /// </summary>
    internal interface IVisitorGetAnyBytes
    {
        /// <summary>
        /// get the endpoint from the security sequence item owner.<para/>
        /// this delegate is used by TcpServer and NetbiosServer that manages the data sequence.
        /// </summary>
        /// <param name="maxCount">
        /// an int value that specifies the maximum count of bytes to get.
        /// </param>
        /// <param name="source">
        /// an object that specifies the source of item.
        /// </param>
        /// <param name="remoteEndPoint">
        /// an object that specifies the remote endpoint expected packet.
        /// </param>
        /// <param name="localEndPoint">
        /// an object that indicates the local endpoint received packet at.
        /// </param>
        byte[] GetBytes(int maxCount, object source, out object remoteEndPoint, out object localEndPoint);
    }

    /// <summary>
    /// this class is used to decode the data buffer according to the data sequence of server,<para/>
    /// that communicates with multiple clients, such as TcpServer and NetbiosServer.<para/>
    /// the format of name of class: {Function}[AddtionalInfomation]Visitor.
    /// the AddtionalInfomation is optional.
    /// </summary>
    internal class GetAnyBytesVisitor
    {
        /// <summary>
        /// expect packet from transport.<para/>
        /// the transport must be a TcpServer or NetbiosServer.
        /// </summary>
        /// <param name="host">
        /// an IGetAnyBytesVisitor interface that specifies the host of visitor.
        /// </param>
        /// <param name="sequence">
        /// a DataSequence object that manages the sequence information of multiple clients.
        /// </param>
        /// <param name="timeout">
        /// a TimeSpan object that indicates the timeout to expect event.
        /// </param>
        /// <param name="maxCount">
        /// an int value that specifies the maximum count of bytes to get.
        /// </param>
        /// <param name="remoteEndPoint">
        /// an object that specifies the remote endpoint expected packet.
        /// </param>
        /// <param name="localEndPoint">
        /// an object that indicates the local endpoint received packet at.
        /// </param>
        /// <returns>
        /// a StackPacket object that specifies the received packet.<para/>
        /// if all buffer is closed in this while, and required to return if all buffer is closed, return null.<para/>
        /// otherwise never return null, if no packets coming in timespan, throw exception.
        /// </returns>
        public static byte[] Visit(
            IVisitorGetAnyBytes host, DataSequence sequence, TimeSpan timeout, int maxCount,
            out object remoteEndPoint, out object localEndPoint)
        {
            if (maxCount < 0)
            {
                throw new ArgumentException("max count must not be negative", "maxCount");
            }

            sequence.Reset();

            DateTime endTime = DateTime.Now + timeout;
            TimeSpan currentTimeout = timeout;

            while (true)
            {
                SequenceItem item = sequence.Next(currentTimeout);

                // skip event
                if (item.Source is TransportEvent)
                {
                    currentTimeout = endTime - DateTime.Now;
                    continue;
                }

                byte[] data = host.GetBytes(maxCount, item.Source, out remoteEndPoint, out localEndPoint);

                if (data == null)
                {
                    throw new InvalidOperationException("invalid data received from client.");
                }

                sequence.Consume(item.Source, data.Length);

                return data;
            }
        }
    }
}
