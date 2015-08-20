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
    internal interface IVisitorGetAnyData
    {
        /// <summary>
        /// get the endpoint from the security sequence item owner.<para/>
        /// this delegate is used by TcpServer and NetbiosServer that manages the data sequence.
        /// </summary>
        /// <param name="owner">
        /// an object that specifies the owner of item.
        /// </param>
        /// <param name="remoteEndPoint">
        /// an object that specifies the remote endpoint expected packet.
        /// </param>
        /// <param name="localEndPoint">
        /// an object that indicates the local endpoint received packet at.
        /// </param>
        void VisitorGetEndPoint(object owner, out object remoteEndPoint, out object localEndPoint);


        /// <summary>
        /// decode packet from the security sequence item owner<para/>
        /// this delegate is used by TcpServer and NetbiosServer that manages the data sequence.
        /// </summary>
        /// <param name="owner">
        /// an object that specifies the owner of item.
        /// </param>
        /// <param name="consumedLength">
        /// an int value that specifies the consumed length of decoder.
        /// </param>
        /// <param name="remoteEndPoint">
        /// an object that specifies the remote endpoint expected packet.
        /// </param>
        /// <param name="localEndPoint">
        /// an object that indicates the local endpoint received packet at.
        /// </param>
        /// <returns>
        /// the decoded packet.
        /// </returns>
        StackPacket VisitorDecodePackets(
            object owner, object remoteEndPoint, object localEndPoint, out int consumedLength);
    }

    /// <summary>
    /// this class is used to decode the data buffer according to the data sequence of server,<para/>
    /// that communicates with multiple clients, such as TcpServer and NetbiosServer.<para/>
    /// the format of name of class: {Function}[AddtionalInfomation]Visitor.
    /// the AddtionalInfomation is optional.
    /// </summary>
    internal class GetAnyDataVisitor
    {
        /// <summary>
        /// expect packet from transport.<para/>
        /// the transport must be a TcpServer or NetbiosServer.
        /// </summary>
        /// <param name="host">
        /// an IVisitorGetAnyPacket interface that specifies the host of visitor.
        /// </param>
        /// <param name="eventQueue">
        /// a SyncFilterQueue&lt;TransportEvent&gt; that specifies the queue to store event.
        /// </param>
        /// <param name="sequence">
        /// a DataSequence object that manages the sequence information of multiple clients.
        /// </param>
        /// <param name="timeout">
        /// a TimeSpan object that indicates the timeout to expect event.
        /// </param>
        /// <param name="skipEvent">
        /// a bool value that specifies whether skip the event.<para/>
        /// if true, just wait for packet coming; otherwise, both data and event will return.
        /// </param>
        /// <returns>
        /// a StackPacket object that specifies the received packet.<para/>
        /// if all buffer is closed in this while, and required to return if all buffer is closed, return null.<para/>
        /// otherwise never return null, if no packets coming in timespan, throw exception.
        /// </returns>
        public static TransportEvent Visit(
            IVisitorGetAnyData host,
            SyncFilterQueue<TransportEvent> eventQueue, DataSequence sequence, TimeSpan timeout, bool skipEvent)
        {
            // the end time for operation.
            DateTime endTime = DateTime.Now + timeout;
            TimeSpan currentTimeout = timeout;

            while (true)
            {
                sequence.Reset();

                // try to decode packet from all clients in sequence.
                while (true)
                {
                    SequenceItem item = sequence.Next(TimeSpan.MinValue);

                    // all item in the sequences returned
                    if (item == null)
                    {
                        break;
                    }

                    TransportEvent transportEvent = item.Source as TransportEvent;

                    // if event arrived and donot skip the event, return the event directly.
                    if (transportEvent != null)
                    {
                        if (skipEvent)
                        {
                            continue;
                        }

                        sequence.Remove(transportEvent);
                        Utility.Remove(eventQueue, transportEvent);

                        return transportEvent;
                    }

                    object remoteEndPoint;
                    object localEndPoint;

                    host.VisitorGetEndPoint(item.Source, out remoteEndPoint, out localEndPoint);

                    int consumedLength = 0;
                    StackPacket packet = null;

                    try
                    {
                        // set timeout to zero, must not wait for more data.
                        // if timeout, process next.
                        packet = host.VisitorDecodePackets(
                            item.Source, remoteEndPoint, localEndPoint, out consumedLength);

                        // remove the sequence information in data sequence.
                        sequence.Consume(item.Source, consumedLength);

                        if (packet != null)
                        {
                            TcpServerConnection connection = item.Source as TcpServerConnection;

                            if (connection != null)
                            {
                                return connection.VisitorCreateTransportEvent(EventType.ReceivedPacket, packet);
                            }
                            else
                            {
                                return new TransportEvent(EventType.ReceivedPacket, remoteEndPoint, localEndPoint, packet);
                            }
                        }
                    }
                    // skip timeout of any host.
                    catch (TimeoutException)
                    {
                    }
                }

                // waiting for next data coming.
                sequence.Next(currentTimeout);
                currentTimeout = endTime - DateTime.Now;
            }
        }
    }
}
