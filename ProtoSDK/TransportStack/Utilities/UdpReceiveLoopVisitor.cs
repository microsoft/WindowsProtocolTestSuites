// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Net;
using System.Text;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Diagnostics.CodeAnalysis;

namespace Microsoft.Protocols.TestTools.StackSdk.Transport
{
    /// <summary>
    /// the interfaces of tcp receiver implementation.<para/>
    /// such as UdpClient and UdpServerListener.<para/>
    /// the format of name of interface: IVisitor{FunctionName}.<para/>
    /// the format of method of interface: Visitor{FunctionUsage}.
    /// </summary>
    internal interface IVisitorUdpReceiveLoop
    {
        /// <summary>
        /// an IPEndPoint object that specifies the local endpoint.<para/>
        /// if LSP hooked, return the required local endpoint.<para/>
        /// otherwise, return the actual listened local endpoint.
        /// </summary>
        IPEndPoint LspHookedLocalEP
        {
            get;
        }


        /// <summary>
        /// add the received data to buffer.
        /// </summary>
        /// <param name="data">
        /// a bytes array that contains the received data.
        /// </param>
        /// <param name="localEP">
        /// an IPEndPoint object that specifies the local endpoint.
        /// </param>
        /// <param name="remoteEP">
        /// an IPEndPoint object that specifies the remote endpoint.
        /// </param>
        void VisitorAddReceivedData(byte[] data, IPEndPoint localEP, IPEndPoint remoteEP);
    }

    /// <summary>
    /// this class is used for tcp receive loop.<para/>
    /// such as UdpClient and UdpServerListener.<para/>
    /// the format of name of class: {Function}[AddtionalInfomation]Visitor.
    /// the AddtionalInfomation is optional.
    /// </summary>
    internal class UdpReceiveLoopVisitor
    {
        /// <summary>
        /// the received loop for tcp connection.
        /// </summary>
        /// <param name="host">
        /// an IVisitorUdpReceiveLoop interface that specifies the host of visitor.
        /// </param>
        /// <param name="server">
        /// an ITransport object that provides AddEvent, it must be UdpClient or UdpServerListener
        /// </param>
        /// <param name="udpClient">
        /// a UdpClient object that provides the udp client stream.
        /// </param>
        /// <param name="thread">
        /// a ThreadManager object that specifies the received thread.
        /// </param>
        /// <param name="isLspHooked">
        /// a bool value that indicates whether using LSP hook.
        /// </param>
        [SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
        public static void Visit(
            IVisitorUdpReceiveLoop host, ITransport server,
            UdpClient udpClient, ThreadManager thread, bool isLspHooked)
        {
            byte[] data = null;

            // get the endpoint of tcp client.
            IPEndPoint localEP = host.LspHookedLocalEP;

            while (!thread.ShouldExit)
            {
                try
                {
                    // received data from server.
                    IPEndPoint remoteEP = null;

                    data = udpClient.Receive(ref remoteEP);

                    if (data == null)
                    {
                        break;
                    }

                    if(isLspHooked)
                    {
                        int numBytesReceived = data.Length;

                        if (numBytesReceived < Marshal.SizeOf(typeof(LspUdpHeader)))
                        {
                            throw new FormatException("Invalid LSP udp packet received");
                        }

                        //Get udp header
                        byte[] udpHeaderBuffer = ArrayUtility.SubArray(data, 0, Marshal.SizeOf(typeof(LspUdpHeader)));

                        IntPtr p = Marshal.AllocHGlobal(udpHeaderBuffer.Length);
                        Marshal.Copy(udpHeaderBuffer, 0, p, udpHeaderBuffer.Length);

                        LspUdpHeader udpHeader = (LspUdpHeader)Marshal.PtrToStructure(p, typeof(LspUdpHeader));
                        Marshal.FreeHGlobal(p);

                        //get address
                        byte[] srcAddressArray = ArrayUtility.SubArray<byte>(data,
                            Marshal.SizeOf(typeof(LspUdpHeader)), udpHeader.HeaderLength -
                            Marshal.SizeOf(typeof(LspUdpHeader)));
                        string srcAddress = Encoding.ASCII.GetString(srcAddressArray);

                        //replacement
                        numBytesReceived = numBytesReceived - udpHeader.HeaderLength;
                        byte[] msgBody = new byte[numBytesReceived];
                        Array.Copy(data, udpHeader.HeaderLength, msgBody, 0, numBytesReceived);

                        //endPoint is real remote client endpoint, remoteEP is LspDll's endpoint
                        IPEndPoint endPoint = new IPEndPoint(IPAddress.Parse(srcAddress), udpHeader.Port);

                        //map from the real endpoint to the lsp dll endpoint
                        //when sending response to the real endpoint, response will be sent to the lsp dll endpoint
                        //and the lsp dll will relay it to the real endpoint
                        LspConsole.Instance.SetMappedIPEndPoint(
                            (IPEndPoint)udpClient.Client.LocalEndPoint, (IPEndPoint)endPoint,
                            (IPEndPoint)remoteEP, StackTransportType.Udp);

                        host.VisitorAddReceivedData(msgBody, localEP, (IPEndPoint)endPoint);
                    }
                    else
                    {
                        host.VisitorAddReceivedData(data, localEP, remoteEP);
                    }
                }
                catch (Exception ex)
                {
                    // handle exception event, return.
                    server.AddEvent(new TransportEvent(EventType.Exception, localEP, ex));

                    break;
                }
            }
        }
    }
}
