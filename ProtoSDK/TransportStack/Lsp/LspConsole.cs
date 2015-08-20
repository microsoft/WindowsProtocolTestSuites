// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Protocols.TestTools.StackSdk;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Net;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using System.Threading;

namespace Microsoft.Protocols.TestTools.StackSdk.Transport
{
    /// <summary>
    /// Lsp control mode 
    /// </summary>
    public enum LspControlMode
    {
        /// <summary>
        /// intercept mode 
        /// </summary>
        Interception,
        /// <summary>
        /// blocking mode 
        /// </summary>
        Blocking
    }

    /// <summary>
    /// A structure containing a collection of lsp related information 
    /// </summary>
    internal class LspSessionInfoCollection
    {
        /// <summary>
        /// lsp session
        /// </summary>
        internal LspSession lspSession;

        /// <summary>
        /// corresponding endpoints
        /// for tcp: map from lsp dll tcp endpoint to the real tcp endpoint
        /// for udp: map from the real udp endpoint to lsp dll udp endpoint
        /// </summary>
        internal Dictionary<string, IPEndPoint> endPoints;
    }

    /// <summary>
    /// Singleton instance;
    /// LspConsole is to pass the control message to the Lsp SPI dll;
    /// All the operations in LspConsole are synchronized. i.e. when an LspConsole operation returns, 
    /// the underlayer LSP MUST have done the internal management already.  
    /// </summary>
    internal sealed class LspConsole : IDisposable
    {
        private static readonly LspConsole instance = new LspConsole();
        private bool disposed;
        private Socket socket;
        private const int LspServiceDefaultPort = 63093;
        //map from local sdk listenning endpoint to lsp session
        private Dictionary<string, LspSessionInfoCollection> sessionMap;

        //key: transport type + endpoint(of windows service), Value: isBlocking
        //this dictionary contains all the endpoints to be hooked
        //when users call intercepttraffic or blocktraffic, intercepted endpoint will be added to this 
        //dictionary first.
        private Dictionary<string, bool> endPointsToHook;

        private bool isConnected;

        /// <summary>
        /// Map the intercepted IPEndPoint to the LspSession.
        /// </summary>
        /// <param name="index">Intercepted IPEndPoint. </param>
        /// <param name="transportType">Tcp or Udp</param>
        /// <returns>Mapped LspSession</returns>
        private LspSessionInfoCollection this[IPEndPoint index, StackTransportType transportType]
        {
            get
            {
                string strKey = transportType.ToString() + index.Serialize().ToString();
                if (sessionMap.ContainsKey(strKey))
                {
                    return sessionMap[strKey];
                }
                else
                {
                    return null;
                }
            }
            set
            {
                string strKey = transportType.ToString() + index.Serialize().ToString();
                sessionMap[strKey] = value;
            }
        }


        #region constructor and dispose

        /// <summary>
        /// Constructor. To create the singleton instance.
        /// Explicit static constructor to tell C# compiler not to mark type as beforefieldinit.
        /// </summary>
        static LspConsole()
        {
        }


        /// <summary>
        /// Constructor.
        /// </summary>
        private LspConsole()
        {
            sessionMap = new Dictionary<string, LspSessionInfoCollection>();
            endPointsToHook = new Dictionary<string, bool>();
        }


        /// <summary>
        /// Singleton instance property.
        /// </summary>
        public static LspConsole Instance
        {
            get
            {
                return instance;
            }
        }


        /// <summary>
        /// Release the managed and unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            this.Dispose(true);

            //Take this object out of the finalization queue of the GC:
            GC.SuppressFinalize(this);
        }


        /// <summary>
        /// Release resources.
        /// </summary>
        /// <param name="disposing">If disposing equals true, Managed and unmanaged resources are disposed.
        /// if false, Only unmanaged resources can be disposed . </param>
        public void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                // If disposing equals true, dispose all managed and unmanaged resources.
                if (disposing)
                {
                    // Free managed resources & other reference types:
                }

                if (socket != null)
                {
                    this.socket.Close();
                    this.socket = null;
                }
                // Call the appropriate methods to clean up unmanaged resources.
                // If disposing is false, only the following code is executed:
                this.disposed = true;
            }
        }


        /// <summary>
        /// This destructor will get called only from the finalization queue.
        /// </summary>
        ~LspConsole()
        {
            this.Dispose(false);
        }
        #endregion


        #region Operations with LspService 
        /// <summary>
        /// Connect to LspService.
        /// </summary>
        private void Connect()
        {
            if (disposed)
            {
                return;
            }

            if (!isConnected)
            {
                IPEndPoint lspServiceEndPoint = new IPEndPoint(IPAddress.Loopback, LspServiceDefaultPort);

                this.socket = new Socket(lspServiceEndPoint.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

                try
                {
                    this.socket.Connect(lspServiceEndPoint);
                }
                catch (SocketException)
                {
                    throw new InvalidOperationException("Cannot connect to LspService");
                }

                sessionMap = new Dictionary<string, LspSessionInfoCollection>();

                isConnected = true;
            }
        }


        /// <summary>
        /// Send message to LspService, and re-connect if connection is broken
        /// </summary>
        /// <param name="msg">message content</param>
        private void InternalSend(byte[] msg)
        {
            if (this.socket == null)
            {
                throw new InvalidOperationException("Must connect before send message");
            }

            try
            {
                this.socket.Send(msg);
            }
            catch (SocketException)
            {
                if (!this.socket.Connected)
                {
                    this.socket.Close();
                    isConnected = false;

                    //re-connect and send again
                    Connect();
                    this.socket.Send(msg);
                }
                else
                {
                    throw;
                }
            }
        }


        /// <summary>
        /// Intercept the ip traffic in a designated address. All the traffic will be sent to sdkListeningIpAddress. 
        /// </summary>
        /// <param name="transportType">TCP or UDP . </param>
        /// <param name="isBlocking">Whether this request is a blocking request.</param>
        /// <param name="interceptedEndPoint">The intercepted IP/Port of the windows service . </param>
        internal void InterceptTraffic(StackTransportType transportType, bool isBlocking, IPEndPoint interceptedEndPoint)
        {
            if (disposed)
            {
                return;
            }

            string strKey = transportType.ToString() + interceptedEndPoint.Serialize().ToString();
            endPointsToHook[strKey] = isBlocking;
        }


        /// <summary>
        /// Intercept the ip traffic in a designated address. All the traffic will be sent to sdkListeningIpAddress. 
        /// </summary>
        /// <param name="transportType">TCP or UDP . </param>
        /// <param name="isBlocking">Whether this request is a blocking request.</param>
        /// <param name="interceptedEndPoint">The intercepted IP/Port of the windows service . </param>
        /// <param name="sdkLocalListeningEndPoint">The IP/Port listened by SDK . </param>
        /// <returns>LspSession, which is corresponding to each intercepted endpoint. Return null if failed.</returns>
        internal void InterceptTraffic(StackTransportType transportType, bool isBlocking, IPEndPoint interceptedEndPoint,
            IPEndPoint sdkLocalListeningEndPoint)
        {
            if (disposed)
            {
                return;
            }

            if (!isConnected)
            {
                Connect();
            }

            if (sdkLocalListeningEndPoint == null)
            {
                throw new ArgumentNullException("sdkLocalListeningEndPoint");
            }

            if (sdkLocalListeningEndPoint.AddressFamily == AddressFamily.InterNetwork &&
                sdkLocalListeningEndPoint.Address == IPAddress.Any)
            {
                sdkLocalListeningEndPoint.Address = IPAddress.Loopback;
            }
            else if (sdkLocalListeningEndPoint.AddressFamily == AddressFamily.InterNetworkV6 &&
                sdkLocalListeningEndPoint.Address == IPAddress.IPv6Any)
            {
                sdkLocalListeningEndPoint.Address = IPAddress.IPv6Loopback;
            }

            LspInterceptionRequest request = new LspInterceptionRequest(transportType, isBlocking,
                interceptedEndPoint, sdkLocalListeningEndPoint);
            InternalSend(request.Encode());


            byte[] recvBuf;
            int recvLen = LspMessage.ReceiveWholeMessage(this.socket, Marshal.SizeOf(typeof(LspInterceptionResponseMsg)), out recvBuf);
            if (recvLen != Marshal.SizeOf(typeof(LspInterceptionResponseMsg)))
            {
                if (isBlocking)
                {
                    throw new InvalidOperationException("BlockTraffic failed");
                }
                else
                {
                    throw new InvalidOperationException("InterceptTraffic failed");
                }
            }

            LspInterceptionResponse response = LspInterceptionResponse.Decode(recvBuf) as LspInterceptionResponse;

            if (response != null && response.Status == 0) //success
            {
                LspSessionInfoCollection sessionInfoCollection = new LspSessionInfoCollection();
                sessionInfoCollection.lspSession = response.Session;
                sessionInfoCollection.lspSession.InterceptedEndPoint = response.InterceptedEndPoint;
                sessionInfoCollection.endPoints = new Dictionary<string, IPEndPoint>();
                this[sdkLocalListeningEndPoint, transportType] = sessionInfoCollection;
            }
            else
            {
                if (isBlocking)
                {
                    throw new InvalidOperationException("BlockTraffic failed");
                }
                else
                {
                    throw new InvalidOperationException("InterceptTraffic failed");
                }
            }
        }


        /// <summary>
        /// Change a hooked endpoint from intercepted mode to blocking mode
        /// </summary>
        /// <param name="localServerEndpoint">the lsp hooked server endpoint</param>
        /// <param name="transportType">Tcp or Udp</param>
        [SecurityPermission(SecurityAction.Demand)]
        internal void ChangeToBlockingMode(IPEndPoint localServerEndpoint, StackTransportType transportType)
        {
            if (disposed)
            {
                return;
            }

            if (!isConnected)
            {
                Connect();
            }

            foreach (KeyValuePair<string, LspSessionInfoCollection> kvp in sessionMap)
            {
                //sessionMap key is local listening endpoint
                //sessionMap value is remote intertecpted endpoint
                if(kvp.Value.lspSession.InterceptedEndPoint.protocolType == transportType
                    && kvp.Value.lspSession.InterceptedEndPoint.endPoint.Equals(localServerEndpoint))
                {
                    LspSession session = kvp.Value.lspSession;
                    LspBlockRequest request = new LspBlockRequest(session);
                    InternalSend(request.Encode());
                    byte[] recvBuf;
                    int recvLen = LspMessage.ReceiveWholeMessage(this.socket, Marshal.SizeOf(typeof(LspBlockResponseMsg)), out recvBuf);
                    if (recvLen != Marshal.SizeOf(typeof(LspBlockResponseMsg)))
                    {
                        throw new InvalidOperationException("BlockTraffic failed");
                    }

                    LspBlockResponse response = LspBlockResponse.Decode(recvBuf) as LspBlockResponse;

                    if (response == null || response.Status != 0)
                    {
                        throw new InvalidOperationException("BlockTraffic failed");
                    }

                    return;
                }
            }

            throw new InvalidOperationException("The specified endpoint isn't in intercepted mode");
        }

        /// <summary>
        /// Retrieve the real remote endpoint address that replaced by the LspClient endpoint.
        /// </summary>
        /// <param name="socketToRetrieve">Socket of Local LspClient. </param>
        /// <param name="destinationEndpoint">destination endpoint of the connection. </param>
        /// <returns>Local IPEndPoint.</returns>
        internal IPEndPoint RetrieveRemoteEndPoint(Socket socketToRetrieve, out IPEndPoint destinationEndpoint)
        {
            destinationEndpoint = null;

            if (disposed)
            {
                return null;
            }

            if (!isConnected)
            {
                Connect();
            }

            IPEndPoint remoteEndPoint = socketToRetrieve.RemoteEndPoint as IPEndPoint;
            //find the session by EndPoint
            if (this[socketToRetrieve.LocalEndPoint as IPEndPoint, StackTransportType.Tcp] != null)
            {
                remoteEndPoint = GetMappedIPEndPoint((IPEndPoint)socketToRetrieve.LocalEndPoint,
                    socketToRetrieve.RemoteEndPoint as IPEndPoint,
                    StackTransportType.Tcp);

                if (remoteEndPoint == null)
                {
                    LspSession session = this[socketToRetrieve.LocalEndPoint as IPEndPoint,
                        StackTransportType.Tcp].lspSession;
                    IPEndPoint lspClientEndPoint = socketToRetrieve.RemoteEndPoint as IPEndPoint;

                    //retrieve the remote EndPoint
                    LspRetrieveEndPointRequest request = new LspRetrieveEndPointRequest(session, lspClientEndPoint);
                    InternalSend(request.Encode());

                    byte[] recvBuf;
                    int recvLen = LspMessage.ReceiveWholeMessage(this.socket, Marshal.SizeOf(typeof(LspRetrieveEndPointResponseMsg)), out recvBuf);
                    if (recvLen != Marshal.SizeOf(typeof(LspRetrieveEndPointResponseMsg)))
                    {
                        throw new InvalidOperationException("Failed to retrieve remote endpoint");
                    }

                    LspRetrieveEndPointResponse response = LspRetrieveEndPointResponse.Decode(recvBuf)
                        as LspRetrieveEndPointResponse;

                    if (response == null || response.Status != 0)
                    {
                        throw new InvalidOperationException("Failed to retrieve remote endpoint");
                    }

                    //map from local endpoint to real endpoint
                    this.SetMappedIPEndPoint((IPEndPoint)socketToRetrieve.LocalEndPoint, 
                        lspClientEndPoint, response.RemoteClientEndPoint, StackTransportType.Tcp);

                    remoteEndPoint = response.RemoteClientEndPoint;
                    destinationEndpoint = response.DestinationEndPoint;
                }
            }

            return remoteEndPoint;
        }


        /// <summary>
        /// Get the local IPEndPoint by remote IPEndPoint. The key is remote IPEndPoint.
        /// for tcp: map from lsp dll tcp endpoint to the real tcp endpoint
        /// for udp: map from the real udp endpoint to lsp dll udp endpoint
        /// </summary>
        /// <param name="localEndpoint">Local server endpoint of sdk</param>
        /// <param name="srcEndPoint">source endpoint</param>
        /// <param name="transportType">tcp or udp</param>
        /// <returns>Local LspClient socket</returns>
        internal IPEndPoint GetMappedIPEndPoint(IPEndPoint localEndpoint, IPEndPoint srcEndPoint, StackTransportType transportType)
        {
            if (disposed)
            {
                return null;
            }

            Dictionary<string, IPEndPoint> endPointMap;

            IPEndPoint connectableEndpoint = GetConnectableEndpoint(localEndpoint);

            string strKey = transportType.ToString() + connectableEndpoint.Serialize().ToString();
            if (sessionMap.ContainsKey(strKey))
            {
                endPointMap = sessionMap[strKey].endPoints;
            }
            else
            {
                return null;
            }

            strKey = srcEndPoint.Serialize().ToString();
            if (endPointMap.ContainsKey(strKey))
            {
                return endPointMap[strKey];
            }
            else
            {
                return null;
            }
        }


        /// <summary>
        /// Bind the local and remote IPEndPoint.
        /// for tcp: map from lsp dll tcp endpoint to the real tcp endpoint
        /// for udp: map from the real udp endpoint to lsp dll udp endpoint
        /// </summary>
        /// <param name="localEndpoint">Local server endpoint of sdk</param>
        /// <param name="srcEndPoint">the tcp/udp endpoint</param>
        /// <param name="mappedEndPoint">the tcp/udp endpoint </param>
        /// <param name="transportType">Tcp or Udp </param>
        internal void SetMappedIPEndPoint(IPEndPoint localEndpoint, IPEndPoint srcEndPoint,
            IPEndPoint mappedEndPoint, StackTransportType transportType)
        {
            if (disposed)
            {
                return;
            }

            Dictionary<string, IPEndPoint> endPointMap;

            IPEndPoint connectableEndpoint = GetConnectableEndpoint(localEndpoint);

            string strKey = transportType.ToString() + connectableEndpoint.Serialize().ToString();
            if (sessionMap.ContainsKey(strKey))
            {
                endPointMap = sessionMap[strKey].endPoints;
            }
            else
            {
                return;
            }

            strKey = srcEndPoint.Serialize().ToString();
            endPointMap[strKey] = mappedEndPoint;

            //For tcp, each endpoint-to-endpoint pair has two records in the dictionary.
            if (transportType == StackTransportType.Tcp)
            {
                strKey = mappedEndPoint.Serialize().ToString();
                endPointMap[strKey] = srcEndPoint;
            }
        }


        /// <summary>
        /// Disconnect the remote EndPoint, and clean up the resources.
        /// </summary>
        /// <param name="localEndpoint">SDK listening endpoint</param>
        /// <param name="remoteEndpoint">the endpoint of real remote client</param>
        /// <param name="transportType">Tcp or Udp</param>
        internal void Disconnect(IPEndPoint localEndpoint, IPEndPoint remoteEndpoint, 
            StackTransportType transportType)
        {
            if (disposed)
            {
                return;
            }

            IPEndPoint connectableEndpoint = GetConnectableEndpoint(localEndpoint);

            string strKey = transportType.ToString() + connectableEndpoint.Serialize().ToString();
            if (sessionMap.ContainsKey(strKey))
            {
                Dictionary<string, IPEndPoint> endPointMap =
                    sessionMap[strKey].endPoints;
                strKey = remoteEndpoint.Serialize().ToString();
                if (transportType == StackTransportType.Udp)
                {
                    endPointMap.Remove(strKey);
                }
                else
                {
                    //For tcp, each endpoint-to-endpoint pair has two records in the dictionary.
                    IPEndPoint mappedEndpoint = endPointMap[strKey];
                    endPointMap.Remove(strKey);
                    endPointMap.Remove(mappedEndpoint.Serialize().ToString());
                }
            }
        }


        /// <summary>
        /// If the input ip endpoint is on an IPv4/IPv6 any address, replace it with loopback address
        /// </summary>
        /// <param name="localEndpoint">Input endpoint</param>
        /// <returns>The replaced IP endpoint</returns>
        private static IPEndPoint GetConnectableEndpoint(IPEndPoint localEndpoint)
        {
            IPEndPoint connectableEndpoint = localEndpoint;
            if (connectableEndpoint.AddressFamily == AddressFamily.InterNetwork &&
                connectableEndpoint.Address.ToString() == IPAddress.Any.ToString())
            {
                connectableEndpoint.Address = IPAddress.Loopback;
            }
            else if (connectableEndpoint.AddressFamily == AddressFamily.InterNetworkV6 &&
                connectableEndpoint.Address.ToString() == IPAddress.IPv6Any.ToString())
            {
                connectableEndpoint.Address = IPAddress.IPv6Loopback;
            }
            return connectableEndpoint;
        }


        /// <summary>
        /// When a lsp hooked server is released, this method should be called to release related resources
        /// </summary>
        /// <param name="localServerEndpoint">the lsp hooked server endpoint</param>
        /// <param name="transportType">Tcp or Udp</param>
        [SecurityPermission(SecurityAction.Demand)]
        internal void UnblockTraffic(IPEndPoint localServerEndpoint, StackTransportType transportType)
        {
            if (disposed)
            {
                return;
            }

            String strKey = null;
            foreach (KeyValuePair<string, LspSessionInfoCollection> kvp in sessionMap)
            {
                //sessionMap key is local listening endpoint
                //sessionMap value is remote intertecpted endpoint
                if(kvp.Value.lspSession.InterceptedEndPoint.protocolType == transportType
                    && kvp.Value.lspSession.InterceptedEndPoint.endPoint.Equals(localServerEndpoint))
                {
                    LspSession session = kvp.Value.lspSession;

                    if (!isConnected)
                    {
                        Connect();
                    }

                    LspUnblockRequest request = new LspUnblockRequest(session);
                    InternalSend(request.Encode());

                    byte[] recvBuf;
                    int recvLen = LspMessage.ReceiveWholeMessage(this.socket, Marshal.SizeOf(typeof(LspUnblockResponseMsg)), out recvBuf);
                    if (recvLen != Marshal.SizeOf(typeof(LspUnblockResponseMsg)))
                    {
                        throw new InvalidOperationException("UnBlockTraffic failed");
                    }

                    LspUnblockResponse response = LspUnblockResponse.Decode(recvBuf) as LspUnblockResponse;

                    strKey = kvp.Key;

                    if (response == null || response.Status != 0)
                    {
                        throw new InvalidOperationException("UnBlockTraffic failed");
                    }

                    break;
                }
            }

            //remove it from session map
            if (strKey != null)
            {
                sessionMap.Remove(strKey);
            }
        }


        /// <summary>
        /// get a replaced endpoint.<para/>
        /// if the localEndPoint (that specifies the server to listen at) is not available, 
        /// LSP will give another endpoint to listen at, and intercept the traffic.<para/>
        /// if the output bool value isLspHooked is set to true, user must invoke the InterceptTraffic.
        /// </summary>
        /// <param name="transportType">
        /// a StackTransportType that specifies the type of transport. it must be Tcp or Udp.
        /// </param>
        /// <param name="localEndPoint">
        /// an IPEndPoint object that specifies the local endpoint for server to listen at.
        /// </param>
        /// <param name="isLspHooked">
        /// output a bool value that specifies whether LSP work.<para/>
        /// if true, must invoke InterceptTraffic.
        /// </param>
        /// <param name="isBlocking">
        /// output a bool value that specifies whether LSP is block mode.<para/>
        /// it's used to pass to InterceptTraffic.
        /// </param>
        /// <returns>
        /// return an IPEndPoint object that specifies the replaced endpoint.
        /// </returns>
        internal IPEndPoint GetReplacedEndPoint(
            StackTransportType transportType, IPEndPoint localEndPoint, out bool isLspHooked, out bool isBlocking)
        {
            if (disposed)
            {
                isLspHooked = false;
                isBlocking = false;

                return localEndPoint;
            }

            IPEndPoint replacedEndpoint;

            isBlocking = false;

            string strKey = transportType.ToString() + localEndPoint.Serialize().ToString();
            if (endPointsToHook.ContainsKey(strKey))
            {
                replacedEndpoint = new IPEndPoint(localEndPoint.Address, 0);
                //sdk local listening endpoint address is fixed as loopback address
                if (replacedEndpoint.AddressFamily == AddressFamily.InterNetwork)
                {
                    replacedEndpoint.Address = IPAddress.Loopback;
                }
                else
                {
                    replacedEndpoint.Address = IPAddress.IPv6Loopback;
                }
                isBlocking = endPointsToHook[strKey];
                endPointsToHook.Remove(strKey);
                isLspHooked = true;
            }
            else
            {
                replacedEndpoint = localEndPoint;
                isLspHooked = false;
            }

            return replacedEndpoint;
        }

        #endregion
    }
}
