// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using System.Text;

namespace Microsoft.Protocols.TestTools.StackSdk.Transport
{
    /// <summary>
    /// LspRegistrationRequest message
    /// </summary>
    [StructLayoutAttribute(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
    public struct LspUnregistrationRequestMsg
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
    /// LspUnregistrationRequest, sent by LspManagementClient
    /// </summary>
    public class LspUnregistrationRequest : LspMessage
    {
        #region fields

        private ProtocolEndPoint protocolEndPoint;

        #endregion


        #region properties

        /// <summary>
        /// Property of protocolEndPoint.
        /// </summary>
        public ProtocolEndPoint ProtocolEndpointUnregistered
        {
            get
            {
                return this.protocolEndPoint;
            }
        }

        #endregion


        #region Constructor

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="protocolEndPointUnregistered">Protocol and IPEndPoint unregistered. </param>
        public LspUnregistrationRequest(ProtocolEndPoint protocolEndPointUnregistered)
            : base(LspMessageType.UnregistrationRequest)
        {
            this.protocolEndPoint = protocolEndPointUnregistered;
        }

        #endregion


        #region methods

        /// <summary>
        /// Decode the received raw data to strong-typed LspMessage.
        /// </summary>
        /// <param name="rawData">Received raw data. </param>
        /// <returns>Decoded strong-typed LspMessage.</returns>
        [SecurityPermission(SecurityAction.Demand)]
        public static LspUnregistrationRequest Decode(byte[] rawData)
        {
            if (rawData == null || rawData.Length != Marshal.SizeOf(typeof(LspUnregistrationRequestMsg)))
            {
                return null;
            }

            LspUnregistrationRequestMsg msg = TypeMarshal.ToStruct<LspUnregistrationRequestMsg>(rawData);

            ProtocolEndPoint endpoint;
            endpoint.protocolType = msg.transportType;
            endpoint.endPoint = msg.serviceEndPoint.ToIPEndPoint();
            if (endpoint.endPoint != null)
            {
                return new LspUnregistrationRequest(endpoint);
            }
            else
            {
                return null;
            }
        }

        #endregion
    }
}
