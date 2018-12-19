// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using System;
using System.Collections.Generic;
using Microsoft.Protocols.TestTools;
using Microsoft.Protocols.TestTools.StackSdk;
using Microsoft.Protocols.TestTools.StackSdk.RemoteDesktop.Rdpedyc;
using Microsoft.Protocols.TestTools.StackSdk.RemoteDesktop.Rdpegt;

namespace Microsoft.Protocols.TestSuites.Rdpegt
{
    public interface IRdpegtAdapter : IAdapter
    {
        /// <summary>
        /// Initialize this protocol with create control and data channels.
        /// </summary>
        /// <param name="rdpedycServer">RDPEDYC Server instance</param>
        /// <param name="transportType">selected transport type for created channels</param>
        /// <returns>true if client supports this protocol; otherwise, return false.</returns>
        bool ProtocolInitialize(RdpedycServer rdpedycServer, DynamicVC_TransportType transportType = DynamicVC_TransportType.RDP_TCP);

        /// <summary>
        /// Send a MAPPED_GEOMETRY_PACKET to client to create a geometry mapping to a rect.
        /// </summary>
        /// <param name="mappingId">The id for the geometry mapping.</param>
        /// <param name="updateType">The update type.</param>
        /// <param name="left">The position to left bound.</param>
        /// <param name="top">The position to right bound.</param>
        /// <param name="width">The width of the rect.</param>
        /// <param name="height">The height of the rect.</param>
        void SendMappedGeometryPacket(UInt64 mappingId, UpdateTypeValues updateType, uint left, uint top, uint width, uint height);
    }
}
