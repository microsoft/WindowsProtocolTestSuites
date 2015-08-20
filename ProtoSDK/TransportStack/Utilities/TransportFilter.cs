// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Net;

namespace Microsoft.Protocols.TestTools.StackSdk.Transport
{
    /// <summary>
    /// filter for transport.
    /// </summary>
    internal class TransportFilter
    {
        #region Fields

        /// <summary>
        /// an object that specifies the endpoint.
        /// </summary>
        private object endpoint;

        #endregion

        #region Constructors

        /// <summary>
        /// constructor.
        /// </summary>
        public TransportFilter()
        {
        }


        /// <summary>
        /// constructor.
        /// </summary>
        /// <param name="objEndpoint">
        /// an object that specifies the endpoint.
        /// </param>
        public TransportFilter(object objEndpoint)
        {
            this.endpoint = objEndpoint;
        }

        #endregion

        #region Transport Event Filter

        /// <summary>
        /// the filter that gets all disconnected event.
        /// </summary>
        /// <param name="obj">
        /// a TransportEvent object that contains the event to filt.
        /// </param>
        /// <returns>
        /// return a bool value that indicates whether the event is disconnected event.
        /// if true, the transport event is disconnected event; otherwise, return false.
        /// </returns>
        public bool FilterDisconnected(TransportEvent obj)
        {
            if (obj != null && obj.EventType == EventType.Disconnected)
            {
                return true;
            }

            return false;
        }


        /// <summary>
        /// the filter that gets all connected event.
        /// </summary>
        /// <param name="obj">
        /// a TransportEvent object that contains the event to filt.
        /// </param>
        /// <returns>
        /// return a bool value that indicates whether the event is connected event.
        /// if true, the transport event is connected event; otherwise, return false.
        /// </returns>
        public bool FilterConnected(TransportEvent obj)
        {
            if (obj != null && obj.EventType == EventType.Connected)
            {
                return true;
            }

            return false;
        }


        /// <summary>
        /// filter the disconnected event from specified client.
        /// </summary>
        /// <param name="obj">
        /// a TransportEvent object that contains the event to filt.
        /// </param>
        /// <returns>
        /// if the disconnected event is from specified endpoint, return true; otherwise, false.
        /// </returns>
        public bool FilterEndPointDisconnected(TransportEvent obj)
        {
            if (obj == null || obj.EventType != EventType.Disconnected)
            {
                return false;
            }

            if (obj.EndPoint == null || !obj.EndPoint.Equals(this.endpoint))
            {
                return false;
            }

            return true;
        }

        #endregion

        #region IPEndPointStackPacket Filter

        /// <summary>
        /// filter the disconnected event from specified client.
        /// </summary>
        /// <param name="obj">
        /// a UdpReceivedBytes object that contains the event to filt.
        /// </param>
        /// <returns>
        /// if the received data is from the specified local endpoint, return true; otherwise, false.
        /// </returns>
        public bool FilterIPEndPoint(IPEndPointStackPacket obj)
        {
            if (obj == null)
            {
                return false;
            }

            // compare the endpoint.
            if (obj.RemoteEndPoint == null || !obj.RemoteEndPoint.Equals(this.endpoint))
            {
                return false;
            }

            return true;
        }

        #endregion

        #region NetbiosEndpointDataFilter Filter

        /// <summary>
        /// filter the disconnected event from specified client.
        /// </summary>
        /// <param name="obj">
        /// a UdpReceivedBytes object that contains the event to filt.
        /// </param>
        /// <returns>
        /// if the received data is from the specified local endpoint, return true; otherwise, false.
        /// </returns>
        public bool FilterNetbiosEndPoint(NetbiosServerStackPacket obj)
        {
            if (obj == null)
            {
                return false;
            }

            // compare the endpoint.
            if (!obj.RemoteEndPoint.Equals(this.endpoint))
            {
                return false;
            }

            return true;
        }

        #endregion

        #region UdpPacketOrBytes Filter

        /// <summary>
        /// filter the disconnected event from specified client.
        /// </summary>
        /// <param name="obj">
        /// a UdpReceivedBytes object that contains the event to filt.
        /// </param>
        /// <returns>
        /// if the received data is from the specified local endpoint, return true; otherwise, false.
        /// </returns>
        public bool FilterUdpPacketOrBytes(UdpReceivedBytes obj)
        {
            if (obj == null)
            {
                return false;
            }

            return this.CompareUdpPacketOrBytes(obj.LocalEndPoint);
        }

        /// <summary>
        /// filter the disconnected event from specified client.
        /// </summary>
        /// <param name="obj">
        /// a UdpReceivedBytes object that contains the event to filt.
        /// </param>
        /// <returns>
        /// if the received data is from the specified local endpoint, return true; otherwise, false.
        /// </returns>
        public bool FilterUdpPacketOrBytes(IPEndPointStackPacket obj)
        {
            if (obj == null)
            {
                return false;
            }

            return this.CompareUdpPacketOrBytes(obj.LocalEndPoint);
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// compare the endpoint.<para/>
        /// if endpoint address is null, compare the port.<para/>
        /// then, compare the address and port.
        /// </summary>
        /// <param name="ep">
        /// an IPEndPoint object to compare.
        /// </param>
        /// <returns>
        /// if equal, return true; otherwise, false.
        /// </returns>
        private bool CompareUdpPacketOrBytes(IPEndPoint ep)
        {
            IPEndPoint localEP = this.endpoint as IPEndPoint;

            // if the address is null, compare the port.
            if (localEP.Address == null && ep != null)
            {
                if (ep != null && ep.Port == localEP.Port)
                {
                    return true;
                }
            }

            // compare the endpoint.
            if (ep == null || !ep.Equals(this.endpoint))
            {
                return false;
            }

            return true;
        }

        #endregion
    }
}
