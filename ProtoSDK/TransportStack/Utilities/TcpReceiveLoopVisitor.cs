// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.IO;
using System.Net.Sockets;

namespace Microsoft.Protocols.TestTools.StackSdk.Transport
{
    /// <summary>
    /// the interfaces of tcp receiver implementation.<para/>
    /// such as TcpClient and TcpServerConnection, these classes received data from tcp client or sslstream.<para/>
    /// the format of name of interface: IVisitor{FunctionName}.<para/>
    /// the format of method of interface: Visitor{FunctionUsage}.
    /// </summary>
    internal interface IVisitorTcpReceiveLoop : IVisitorNetbiosReceiveLoop
    {
        /// <summary>
        /// create a transport event.<para/>
        /// TcpClient uses localEndPoint as EndPoint.<para/>
        /// while TcpServer uses remoteEndPointas EndPoint.
        /// </summary>
        /// <param name="type">
        /// an EventType enum that specifies the event type.
        /// </param>
        /// <param name="detail">
        /// an object that contains the event object.
        /// </param>
        /// <returns>
        /// a TransportEvent object that specifies the created event.
        /// </returns>
        TransportEvent VisitorCreateTransportEvent(EventType type, object detail);
    }

    /// <summary>
    /// this class is used for tcp receive loop.<para/>
    /// such as TcpClient and TcpServerConnection, that received data from tcpClient or sslStream.
    /// </summary>
    internal class TcpReceiveLoopVisitor
    {
        /// <summary>
        /// the received loop for tcp connection.
        /// </summary>
        /// <param name="host">
        /// an IVisitorTcpReceiveLoop interface that specifies the host of visitor.
        /// </param>
        /// <param name="server">
        /// an ITransport object that provides AddEvent, it must be TcpClient or TcpServerConnection.
        /// </param>
        /// <param name="stream">
        /// a Stream object that specifies the underlayer transport stream.<para/>
        /// if DirectTcp, it's the stream of TcpClient.GetStream().<para/>
        /// if Tcp over Ssl, it's SslStream.
        /// </param>
        /// <param name="thread">
        /// a ThreadManager object that specifies the received thread.
        /// </param>
        /// <param name="bufferSize">
        /// an int value that specifies the max buffer size.
        /// </param>
        public static void Visit(
            IVisitorTcpReceiveLoop host, ITransport server,
            Stream stream, ThreadManager thread, int bufferSize)
        {
            byte[] data = new byte[bufferSize];

            while (!thread.ShouldExit)
            {
                int receivedLength = 0;

                try
                {
                    // received data from server.
                    receivedLength = stream.Read(data, 0, data.Length);

                    // if the server close the socket, return.
                    if (receivedLength == 0)
                    {
                        server.AddEvent(host.VisitorCreateTransportEvent(EventType.Disconnected, null));

                        break;
                    }

                    host.VisitorAddReceivedData(ArrayUtility.SubArray<byte>(data, 0, receivedLength));
                }
                catch (IOException ex)
                {
                    SocketException socketException = ex.InnerException as SocketException;

                    // if the server disconnect the socket.
                    if (socketException != null
                        && (socketException.SocketErrorCode == SocketError.ConnectionReset
                        || socketException.SocketErrorCode == SocketError.Interrupted
                        || socketException.SocketErrorCode == SocketError.ConnectionAborted))
                    {
                        // handle the disconnected event, return.
                        server.AddEvent(host.VisitorCreateTransportEvent(EventType.Disconnected, ex));
                    }
                    else
                    {
                        // handle exception event, return.
                        server.AddEvent(host.VisitorCreateTransportEvent(EventType.Exception, ex));
                    }

                    break;
                }
            }
        }
    }
}
