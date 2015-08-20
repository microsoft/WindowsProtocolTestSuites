// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;

namespace Microsoft.Protocols.TestTools.StackSdk.Transport
{
    /// <summary>
    /// This class is used to notify user that a special event occurs in transport.
    /// </summary>
    public class TransportEvent
    {
        #region Fields

        /// <summary>
        /// the type of the occurred event.
        /// </summary>
        private EventType eventType;

        /// <summary>
        /// the identity endpoint.
        /// </summary>
        private object identityEP;

        /// <summary>
        /// the local endpoint.
        /// </summary>
        private object localEP;

        /// <summary>
        /// the endPoint from which the event occurred.
        /// </summary>
        private object remoteEP;

        /// <summary>
        /// To contain some details of the occurred event.
        /// </summary>
        private object eventObject;

        #endregion

        #region Properties

        /// <summary>
        /// get an EventType enum that specifies the type of the occurred event.
        /// </summary>
        public EventType EventType
        {
            get
            {
                return this.eventType;
            }
        }


        /// <summary>
        /// get an object that specifies the identity of event.<para/>
        /// if TcpClient/Stream, equals to localEndPoint.<para/>
        /// if TcpServer, Netbios and Udp equals to remoteEndPoint.
        /// </summary>
        public object EndPoint
        {
            get
            {
                return this.identityEP;
            }
        }


        /// <summary>
        /// get an object that contains the endPoint from which the event occurred.<para/>
        /// if Tcp/Udp, it's an IPEndPoint object that specifies the endpoint of event.<para/>
        /// if Netbios, it's an int value that specifies the session id.<para/>
        /// if Stream, it's null.
        /// </summary>
        public object RemoteEndPoint
        {
            get
            {
                return this.remoteEP;
            }
        }


        /// <summary>
        /// get an object that contains the local endPoint.<para/>
        /// if Tcp/Udp, it's an IPEndPoint object that specifies the endpoint of event.<para/>
        /// if Netbios, it's the ncb_num that specifies the number for the local network name.<para/>
        /// if Stream, it's an int value that specifies the local identity.
        /// </summary>
        public object LocalEndPoint
        {
            get
            {
                return this.localEP;
            }
        }


        /// <summary>
        /// get an object that contains some details of the occurred event.<para/>
        /// for received packet, it's a StackPacket object that contains the received packet.<para/>
        /// for exception, it's an Exception object that contains the thrown exception.
        /// </summary>
        public object EventObject
        {
            get
            {
                return this.eventObject;
            }
        }

        #endregion

        #region Constructor

        /// <summary>
        /// Constructor.<para/>
        /// by default, the EndPoint will set to remoteEndPoint.
        /// </summary>
        /// <param name="type">
        /// an EventType enum that specifies the type of the occurred event.
        /// </param>
        /// <param name="remoteEndPoint">
        /// an object that contains the endPoint from which the event occurred.<para/>
        /// it's used to set the EndPoint and RemoteEndPoint.
        /// </param>
        /// <param name="detail">
        /// an object that contains some details of the occurred event.<para/>
        /// It may be null if no detail needed.</param>
        public TransportEvent(EventType type, object remoteEndPoint, object detail)
            : base()
        {
            this.eventType = type;
            this.remoteEP = remoteEndPoint;
            this.eventObject = detail;

            // normally, the endPoint equals to remoteEndPoint.
            this.identityEP = remoteEndPoint;
        }


        /// <summary>
        /// Constructor.<para/>
        /// by default, the EndPoint will set to remoteEndPoint.
        /// </summary>
        /// <param name="type">
        /// an EventType enum that specifies the type of the occurred event.
        /// </param>
        /// <param name="remoteEndPoint">
        /// an object that contains the endPoint from which the event occurred.<para/>
        /// it's used to set the EndPoint and RemoteEndPoint.
        /// </param>
        /// <param name="localEndPoint">
        /// an object that specifies the local endpoint of event.<para/>
        /// it's used to set the LocalEndPoint.
        /// </param>
        /// <param name="detail">
        /// an object that contains some details of the occurred event.<para/>
        /// It may be null if no detail needed.</param>
        public TransportEvent(EventType type, object remoteEndPoint, object localEndPoint, object detail)
            : this(type, remoteEndPoint, detail)
        {
            this.localEP = localEndPoint;
        }


        /// <summary>
        /// Constructor.<para/>
        /// used for TcpClient and Stream.
        /// </summary>
        /// <param name="type">
        /// an EventType enum that specifies the type of the occurred event.
        /// </param>
        /// <param name="endPoint">
        /// an object that specifies the identity of event.<para/>
        /// if TcpClient/Stream, equals to localEndPoint.
        /// </param>
        /// <param name="remoteEndPoint">
        /// an object that contains the endPoint from which the event occurred.
        /// </param>
        /// <param name="localEndPoint">
        /// an object that specifies the local endpoint of event.
        /// </param>
        /// <param name="detail">
        /// an object that contains some details of the occurred event.<para/>
        /// It may be null if no detail needed.</param>
        /// <exception cref="ArgumentNullException">
        /// thrown when endPoint is null.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// thrown when localEndPoint is null.
        /// </exception>
        /// <exception cref="InvalidOperationException">
        /// thrown when the endPoint does not equal to localEndPoint.
        /// </exception>
        internal TransportEvent(
            EventType type, object endPoint, object remoteEndPoint, object localEndPoint, object detail)
            : this(type, remoteEndPoint, localEndPoint, detail)
        {
            if (endPoint == null)
            {
                throw new ArgumentNullException("endPoint");
            }

            if (localEndPoint == null)
            {
                throw new ArgumentNullException("localEndPoint");
            }

            if (!endPoint.Equals(localEndPoint))
            {
                throw new InvalidOperationException("for TcpClient/Stream, the endPoint must equal to localEndPoint.");
            }

            // for TcpClient, the endpoint equals to localEndpoint.
            this.identityEP = endPoint;
        }

        #endregion
    }
}
