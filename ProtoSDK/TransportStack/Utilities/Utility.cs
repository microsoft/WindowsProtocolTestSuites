// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Net;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;

namespace Microsoft.Protocols.TestTools.StackSdk.Transport
{
    /// <summary>
    /// Header of Lsp udp packets
    /// </summary>
    internal struct LspUdpHeader
    {
        /// <summary>
        /// an int value that specifies the length of header
        /// </summary>
        public int HeaderLength;

        /// <summary>
        /// an int value that specifies the address family, it's IPv4 or IPv6
        /// </summary>
        public int AddressFamily;

        /// <summary>
        /// an int value that specifies the source Port
        /// </summary>
        public int Port;
    }

    /// <summary>
    /// the utility for transport.
    /// </summary>
    internal static class Utility
    {
        /// <summary>
        /// an int value that indicates the milli-seconds to wait for data is available.
        /// </summary>
        public const int MilliSecondsToWaitStreamDataAvailable = 1;

        /// <summary>
        /// a delegate that specifies the safely operation call back.
        /// </summary>
        public delegate void SafelyOperationCallback();

        /// <summary>
        /// safely complete the operation, do not arise any exception.<para/>
        /// the work thread will invoke this method, to make sure donot arise any exception.<para/>
        /// such as the ReceiveLoop methods of receiver.
        /// </summary>
        /// <param name="callback">
        /// a delegate that specifies the safely operation call back.
        /// </param>
        [SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
        public static void SafelyOperation(SafelyOperationCallback callback)
        {
            if (callback == null)
            {
                return;
            }

            try
            {
                callback();
            }
            catch (Exception)
            {
            }
        }


        /// <summary>
        /// safely disconnect the netbios connection, donot arise any exception.<para/>
        /// when disconnect netbios transport multiple times,<para/>
        /// it may throw exception. if no need to arise it, invoke this method.
        /// </summary>
        /// <param name="netbiosTransport">
        /// a NetbiosTransport object to disconnect the connection.
        /// </param>
        /// <param name="sessionId">
        /// an int value that specifies the session id to disconnect.
        /// </param>
        [SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
        public static void SafelyDisconnectNetbiosConnection(NetbiosTransport netbiosTransport, int sessionId)
        {
            if (netbiosTransport == null)
            {
                return;
            }

            try
            {
                // when disconnect netbios transport multiple times,
                // it may throw exception.
                netbiosTransport.Disconnect(sessionId);
            }
            catch (Exception)
            {
            }
        }


        /// <summary>
        /// get one packet from packet cache.<para/>
        /// if no packet, return null.
        /// </summary>
        /// <param name="packetCache">
        /// a SyncFilterQueue queue that stores the packet.
        /// </param>
        /// <param name="filter">
        /// the filter to get packet. if null, do not filter packet.
        /// </param>
        /// <returns>
        /// a packet specified by filter.
        /// </returns>
        public static PacketType GetOne<PacketType>(SyncFilterQueue<PacketType> packetCache, Filter<PacketType> filter)
        {
            PacketType packet = default(PacketType);

            if (packetCache.Count == 0)
            {
                return packet;
            }

            try
            {
                packet = packetCache.Dequeue(new TimeSpan(), filter);
            }
            catch (TimeoutException)
            {
            }

            return packet;
        }


        /// <summary>
        /// enqueue the receivied transport event.<para/>
        /// add the transport event to the event queue, for user to expect specified event directly.<para/>
        /// add the transport event to data sequence, for user to expect any event(include received packet).<para/>
        /// it's thread-safe.
        /// </summary>
        /// <param name="eventQueue">
        /// a SyncFilterQueue&lt;TransportEvent&gt; that specifies the queue to store event.
        /// </param>
        /// <param name="sequence">
        /// a DataSequence object that stores the received data or event sequence.
        /// </param>
        /// <param name="transportEvent">
        /// a TransportEvent object that specifies the received event.
        /// </param>
        public static void Enqueue(SyncFilterQueue<TransportEvent> eventQueue, DataSequence sequence, TransportEvent transportEvent)
        {
            lock (eventQueue)
            {
                sequence.Add(transportEvent, new byte[0], null);
                eventQueue.Enqueue(transportEvent);
            }
        }


        /// <summary>
        /// dequeue the received event from queue.
        /// </summary>
        /// <param name="eventQueue">
        /// a SyncFilterQueue&lt;TransportEvent&gt; that specifies the queue to store event.
        /// </param>
        /// <param name="sequence">
        /// a DataSequence object that stores the received data or event sequence.
        /// </param>
        /// <param name="timeout">
        /// a TimeSpan object that specifies the timeout to wait for event.
        /// </param>
        /// <param name="filter">
        /// a Filter&lt;TransportEvent&gt; that specifies the filter to get event.
        /// </param>
        /// <returns>
        /// return a TransportEvent object that stores the event.
        /// </returns>
        public static TransportEvent Dequeue(
            SyncFilterQueue<TransportEvent> eventQueue, DataSequence sequence,
            TimeSpan timeout, Filter<TransportEvent> filter)
        {
            TransportEvent transportEvent = eventQueue.Dequeue(timeout, filter);

            // remove the event from sequence.
            sequence.Remove(transportEvent);

            return transportEvent;
        }


        /// <summary>
        /// remove the specifies transport event from queue and data sequence.<para/>
        /// invoke this method when get event from the data sequence.
        /// </summary>
        /// <param name="eventQueue">
        /// a SyncFilterQueue&lt;TransportEvent&gt; that specifies the queue to store event.
        /// </param>
        /// <param name="transportEvent">
        /// a TransportEvent object that specifies the event to remove.
        /// </param>
        public static void Remove(
            SyncFilterQueue<TransportEvent> eventQueue, TransportEvent transportEvent)
        {
            lock (eventQueue)
            {
                TransportEvent removedTransportEvent = eventQueue.Dequeue(new TimeSpan());

                if (removedTransportEvent != transportEvent)
                {
                    throw new InvalidOperationException(
                        "the transport event stores in the event queue and data sequence are not consistence.");
                }
            }
        }


        /// <summary>
        /// Create a UDP header which will be added to the head of the udp payload
        /// </summary>
        /// <param name="realEndPoint">
        /// The real IP endpoint that udp payload should be sent to
        /// </param>
        /// <returns>
        /// bytes of the udp header
        /// </returns>
        public static byte[] CreateUdpHeader(IPEndPoint realEndPoint)
        {
            int headerStructLength = Marshal.SizeOf(typeof(LspUdpHeader));

            LspUdpHeader udpHeader = new LspUdpHeader();

            udpHeader.Port = realEndPoint.Port;
            udpHeader.AddressFamily = (int)realEndPoint.AddressFamily;

            string strAddress = realEndPoint.Address.ToString();
            byte[] addressBytes = System.Text.Encoding.ASCII.GetBytes(strAddress);
            udpHeader.HeaderLength = headerStructLength + addressBytes.Length;

            //write header and address to a byte array
            IntPtr p = Marshal.AllocHGlobal(headerStructLength);
            Marshal.StructureToPtr(udpHeader, p, true);

            byte[] ret = new byte[headerStructLength + addressBytes.Length];
            Marshal.Copy(p, ret, 0, headerStructLength);
            Marshal.FreeHGlobal(p);

            Array.Copy(addressBytes, 0, ret, headerStructLength, addressBytes.Length);

            return ret;
        }


        /// <summary>
        /// stop the transport on the specifies port.
        /// </summary>
        /// <param name="transport">
        /// an IStartable object that specifies the transport.
        /// </param>
        /// <param name="listenedLocalEndPoints">
        /// an ICollection&lt;IPEndPoint&gt; that contains the listened local endpoint.<para/>
        /// if null, stop the EndPoint(specifiedAddress, portToStop), it's used by UdpClient.
        /// </param>
        /// <param name="specifiedAddress">
        /// when listenedLocalEndPoints is null, using this address instead.<para/>
        /// if null, using IPAddress.Any.
        /// </param>
        /// <param name="localEndPoint">
        /// an object that specifies the port to stop.
        /// </param>
        public static void StopTransportByPort(IStartable transport,
            ICollection<IPEndPoint> listenedLocalEndPoints, IPAddress specifiedAddress, object localEndPoint)
        {
            int port;

            try
            {
                port = Convert.ToInt32(localEndPoint, CultureInfo.InvariantCulture);
            }
            catch (Exception)
            {
                throw new ArgumentException("localEndPoint is not an IPEndPoint/port.", "localEndPoint");
            }

            if (listenedLocalEndPoints == null)
            {
                if (specifiedAddress == null)
                {
                    specifiedAddress = IPAddress.Any;
                }

                transport.Stop(new IPEndPoint(specifiedAddress, port));

                return;
            }

            // get all endpoint to stop.
            IPEndPoint[] endpoints = new IPEndPoint[listenedLocalEndPoints.Count];

            listenedLocalEndPoints.CopyTo(endpoints, 0);

            // stop the specified endpoint.
            foreach (IPEndPoint ep in endpoints)
            {
                if (ep.Port == port)
                {
                    transport.Stop(ep);
                }
            }
        }


        /// <summary>
        /// get the endpoint to listen by the port.<para/>
        /// retrieve the address from socket config.
        /// </summary>
        /// <param name="socketConfig">
        /// a SocketTransportConfig object that contains the LocalAddress.
        /// </param>
        /// <param name="localEndPoint">
        /// an int value that specifies the port to start at.
        /// </param>
        /// <returns>
        /// an IPEndPoint object that specifies the endpoint to listen at.
        /// </returns>
        public static IPEndPoint GetEndPointByPort(SocketTransportConfig socketConfig, object localEndPoint)
        {
            int port;

            try
            {
                port = Convert.ToInt32(localEndPoint, CultureInfo.InvariantCulture);
            }
            catch (Exception)
            {
                throw new ArgumentException("localEndPoint is not an IPEndPoint/port.", "localEndPoint");
            }

            if (socketConfig.LocalIpAddress == null)
            {
                throw new InvalidOperationException(
                    "localEndPoint is an int port, but the LocalIpAddress is not configured.");
            }

            return new IPEndPoint(socketConfig.LocalIpAddress, port);
        }
    }
}
