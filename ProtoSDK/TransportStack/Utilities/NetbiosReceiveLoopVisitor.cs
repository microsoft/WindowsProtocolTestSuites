// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Diagnostics.CodeAnalysis;

namespace Microsoft.Protocols.TestTools.StackSdk.Transport
{
    /// <summary>
    /// the interfaces of tcp receiver implementation.<para/>
    /// such as NetbiosClient and NetbiosServerConnection.<para/>
    /// the format of name of interface: IVisitor{FunctionName}.<para/>
    /// the format of method of interface: Visitor{FunctionUsage}.
    /// </summary>
    internal interface IVisitorNetbiosReceiveLoop
    {
        /// <summary>
        /// add the received data to buffer.
        /// </summary>
        /// <param name="data">
        /// a bytes array that contains the received data.
        /// </param>
        void VisitorAddReceivedData(byte[] data);
    }

    /// <summary>
    /// this class is used for tcp receive loop.<para/>
    /// such as NetbiosClient and NetbiosServerConnection.<para/>
    /// the format of name of class: {Function}[AddtionalInfomation]Visitor.
    /// the AddtionalInfomation is optional.
    /// </summary>
    internal class NetbiosReceiveLoopVisitor
    {
        /// <summary>
        /// the received loop for netbios connection.
        /// </summary>
        /// <param name="host">
        /// an IVisitorNetbiosReceiveLoop interface that specifies the host of visitor.
        /// </param>
        /// <param name="server">
        /// an ITransport object that provides AddEvent, it must be NetbiosClient or NetbiosServerConnection.
        /// </param>
        /// <param name="transport">
        /// a NetbiosTransport object that specifies the underlayer transport.
        /// </param>
        /// <param name="localEP">
        /// an int value that specifies the local endpoint.
        /// </param>
        /// <param name="remoteEP">
        /// an int value that specifies the remote endpoint.
        /// </param>
        /// <param name="thread">
        /// a ThreadManager object that specifies the received thread.
        /// </param>
        [SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
        public static void Visit(
            IVisitorNetbiosReceiveLoop host, ITransport server,
            NetbiosTransport transport, int localEP, int remoteEP, ThreadManager thread)
        {
            byte[] data = null;

            while (!thread.ShouldExit)
            {
                try
                {
                    // received data from server.
                    data = transport.Receive(remoteEP);

                    // if the server close the socket, return.
                    if (data == null)
                    {
                        server.AddEvent(new TransportEvent(EventType.Disconnected, remoteEP, localEP, null));

                        break;
                    }

                    host.VisitorAddReceivedData(ArrayUtility.SubArray<byte>(data, 0));
                }
                // the connection is disconnected.
                catch (InvalidOperationException ex)
                {
                    server.AddEvent(new TransportEvent(EventType.Disconnected, remoteEP, localEP, ex));

                    break;
                }
                catch (Exception ex)
                {
                    // handle exception event, return.
                    server.AddEvent(new TransportEvent(EventType.Exception, remoteEP, localEP, ex));

                    break;
                }
            }
        }
    }
}
