// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Net.Quic;

namespace Microsoft.Protocols.TestTools.StackSdk.Transport
{
    internal interface IVisitorQuicReceiveLoop : IVisitorNetbiosReceiveLoop
    {
        TransportEvent VisitorCreateTransportEvent(EventType type, object detail);
    }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Interoperability", "CA1416:Validate platform compatibility")]
    internal class QuicReceiveLoopVisitor
    {
        public static void Visit(
            IVisitorQuicReceiveLoop host, ITransport server,
            QuicStream stream, ThreadManager thread, int bufferSize)
        {
            byte[] receivedData = new byte[bufferSize];            

            while (!thread.ShouldExit)
            {
                int receivedLength;

                try
                {
                    receivedLength = stream.Read(receivedData, 0, bufferSize);

                    if (receivedLength == 0)
                    { 
                        server.AddEvent(host.VisitorCreateTransportEvent(EventType.Disconnected, null));
                        break;
                    }
                    else
                    {
                        host.VisitorAddReceivedData(ArrayUtility.SubArray<byte>(receivedData, 0, receivedLength));
                    }

                }
                catch (QuicException ex)
                {
                    server.AddEvent(new TransportEvent(EventType.Disconnected, null, ex));
                    break;
                }
            }
        }

    }
}
