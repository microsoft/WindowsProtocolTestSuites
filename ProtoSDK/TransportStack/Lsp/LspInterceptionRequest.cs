// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Protocols.TestTools.StackSdk;
using Microsoft.Protocols.TestTools.StackSdk.Messages.Marshaling;
using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using System.Text;

namespace Microsoft.Protocols.TestTools.StackSdk.Transport
{
    /// <summary>
    /// LspInterceptionRequest message
    /// </summary>
    [StructLayoutAttribute(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
    public struct LspInterceptionRequestMsg
    {
        /// <summary>
        /// Header
        /// </summary>
        public LspMessageHeader header;

        /// <summary>
        /// Transport type
        /// </summary>
        public StackTransportType transportType;

        /// <summary>
        /// Session id
        /// </summary>
        public int sessionId;

        /// <summary>
        /// Service IPEndPoint
        /// </summary>
        public IPEndPointMsg serviceEndPoint;

        /// <summary>
        /// Sdk IPEndPoint
        /// </summary>
        public IPEndPointMsg sdkEndPoint;

        /// <summary>
        /// IsBlocking
        /// Marshal will add padding up to 4-byte length automatically
        /// </summary>
        public bool isBlocking;
    }

    /// <summary>
    /// LspInterceptionRequest, the first message sent from LspConsole
    /// </summary>
    public class LspInterceptionRequest : LspMessage
    {
        private static int currentSessionId = 1;
        private static Object sessionLock = new Object();

        private int sessionId;
        private StackTransportType transportType;
        private ProtocolEndPoint serviceEndPoint;
        private IPEndPoint sdkEndPoint;
        private bool isBlocking;

        /// <summary>
        /// Generate a new session ID for each interception request
        /// </summary>
        /// <returns>session ID</returns>
        public void GenerateNewSessionId()
        {
            lock (LspInterceptionRequest.sessionLock)
            {
                //generate session id, begin from 1
                this.sessionId = LspInterceptionRequest.currentSessionId++;
            }
        }

        /// <summary>
        /// SessionId property
        /// </summary>
        public int SessionId
        {
            get
            {
                return this.sessionId;
            }
        }

        /// <summary>
        /// Transport type: TCP/UDP?
        /// </summary>
        public StackTransportType TransportType
        {
            get
            {
                return this.transportType;
            }
        }

        /// <summary>
        /// Intercepted IPEndPoint.
        /// </summary>
        public ProtocolEndPoint InterceptedEndPoint
        {
            get
            {
                return this.serviceEndPoint;
            }
        }

        /// <summary>
        /// Sdk IPEndPoint
        /// </summary>
        public IPEndPoint SdkEndPoint
        {
            get
            {
                return this.sdkEndPoint;
            }
        }

        /// <summary>
        /// Whether this request is also a blocking request
        /// </summary>
        public bool IsBlocking
        {
            get
            {
                return this.isBlocking;
            }
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="transportType">Transport type: TCP/UDP?</param>
        /// <param name="isBlocking">Blocking mode or not</param>
        /// <param name="serviceEndPoint">Intercepted windows service IPEndPoint. </param>
        /// <param name="sdkEndPoint">Listening SDK's IPEndPoint. </param>
        public LspInterceptionRequest(StackTransportType transportType, bool isBlocking,
            IPEndPoint serviceEndPoint, IPEndPoint sdkEndPoint)
            : base(LspMessageType.InterceptionRequest)
        {
            if (StackTransportType.Tcp != transportType && StackTransportType.Udp != transportType)
            {
                throw new NotSupportedException("Not only TCP and UDP are supported");
            }

            this.sessionId = -1;
            this.transportType = transportType;
            this.isBlocking = isBlocking;
            this.serviceEndPoint.endPoint = serviceEndPoint;
            this.serviceEndPoint.protocolType = transportType;
            this.sdkEndPoint = sdkEndPoint;
        }


        /// <summary>
        /// Encode the Lsp message into byte array.
        /// </summary>
        /// <returns>Encoded byte array.</returns>
        [SecurityPermission(SecurityAction.Demand)]
        public override byte[] Encode()
        {
            LspInterceptionRequestMsg msg;
            msg.header.messageType = LspMessageType.InterceptionRequest;
            msg.header.messageLen = Marshal.SizeOf(typeof(LspInterceptionRequestMsg));
            msg.transportType = this.transportType;
            msg.sessionId = this.sessionId;
            msg.serviceEndPoint = new IPEndPointMsg(this.serviceEndPoint.endPoint);
            msg.sdkEndPoint = new IPEndPointMsg(this.sdkEndPoint);
            msg.isBlocking = this.isBlocking;

            return TypeMarshal.ToBytes<LspInterceptionRequestMsg>(msg);
        }


        /// <summary>
        /// Decode the raw data into the strong type LspMessage.
        /// </summary>
        /// <param name="rawData">Raw data(byte array) to be decoded.</param>
        /// <returns>Strong type LspMessage.</returns>
        [SecurityPermission(SecurityAction.Demand)]
        public static LspInterceptionRequest Decode(byte[] rawData)
        {
            if (rawData == null || rawData.Length != Marshal.SizeOf(typeof(LspInterceptionRequestMsg)))
            {
                return null;
            }

            LspInterceptionRequestMsg msg = TypeMarshal.ToStruct<LspInterceptionRequestMsg>(rawData);
            IPEndPoint serviceEndPoint = msg.serviceEndPoint.ToIPEndPoint();
            IPEndPoint sdkEndPoint = msg.sdkEndPoint.ToIPEndPoint();
            if (serviceEndPoint != null && sdkEndPoint != null)
            {
                LspInterceptionRequest request = new LspInterceptionRequest(
                    msg.transportType,
                    msg.isBlocking,
                    serviceEndPoint,
                    sdkEndPoint);
                request.sessionId = msg.sessionId;
                return request;
            }
            else
            {
                return null;
            }
        }
    }
}
