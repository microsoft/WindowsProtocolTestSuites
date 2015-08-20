// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Protocols.TestTools.StackSdk;
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
    /// LspRegistrationRequest message
    /// </summary>
    [StructLayoutAttribute(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
    public struct LspRegistrationRequestMsg
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
        /// IPEndPoint
        /// </summary>
        public IPEndPointMsg serviceEndPoint;
    }

    /// <summary>
    /// LspRegistrationRequest, sent by LspManagementClient
    /// </summary>
    public class LspRegistrationRequest : LspMessage
    {
        private ProtocolEndPoint protocolEndPoint;

        /// <summary>
        /// Listening IPEndPoint.
        /// </summary>
        public ProtocolEndPoint ProtocolEndPointRegistered
        {
            get
            {
                return this.protocolEndPoint;
            }
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="protocolEndPointRegistered">Protocol and IPEndPoint registered. </param>
        public LspRegistrationRequest(ProtocolEndPoint protocolEndPointRegistered)
            : base(LspMessageType.RegistrationRequest)
        {
            this.protocolEndPoint = protocolEndPointRegistered;
        }


        /// <summary>
        /// Decode the received raw data to strong-typed LspMessage.
        /// </summary>
        /// <param name="rawData">Received raw data. </param>
        /// <returns>Decoded strong-typed LspMessage.</returns>
        [SecurityPermission(SecurityAction.Demand)]
        public static LspRegistrationRequest Decode(byte[] rawData)
        {
            if (rawData == null || rawData.Length != Marshal.SizeOf(typeof(LspRegistrationRequestMsg)))
            {
                return null;
            }

            LspRegistrationRequestMsg msg = TypeMarshal.ToStruct<LspRegistrationRequestMsg>(rawData);

            ProtocolEndPoint endpoint;
            endpoint.protocolType = msg.transportType;
            endpoint.endPoint = msg.serviceEndPoint.ToIPEndPoint();
            if (endpoint.endPoint != null)
            {
                return new LspRegistrationRequest(endpoint);
            }
            else
            {
                return null;
            }
        }
    }
}
