// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using Microsoft.Protocols.TestSuites.Rdpbcgr;
using Microsoft.Protocols.TestTools;
using Microsoft.Protocols.TestTools.StackSdk.RemoteDesktop.Rdpedyc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Microsoft.Protocols.TestSuites.Rdp.Rdpefs
{
    public interface IRdpefsAdapter : IAdapter
    {
        /// <summary>
        /// Initialize this protocol with create control and data channels.
        /// </summary>
        /// <param name="rdpedycServer">RDPEDYC Server instance</param>
        /// <param name="transportType">selected transport type for created channels</param>
        /// <returns>true if client supports this protocol; otherwise, return false.</returns>
        bool ProtocolInitialize(RdpedycServer rdpedycServer, DynamicVC_TransportType transportType = DynamicVC_TransportType.RDP_UDP_Reliable);

        /// <summary>
        /// Generate static virtual channel data messages for test.
        /// MS-RDPEFS is used to generated virtual channel traffics.
        /// </summary>
        /// <param name="RDPDR_ChannelId">Static Channel Id for RDPDR</param>
        /// <param name="invalidType">Invalid Type</param>
        void GenerateStaticVirtualChannelTraffics(StaticVirtualChannel_InvalidType invalidType = StaticVirtualChannel_InvalidType.None);

        /// <summary>
        /// Attach a RdpbcgrAdapter object
        /// </summary>
        /// <param name="rdpbcgrAdapter">RDPBCGR adapter</param>
        void AttachRdpbcgrAdapter(IRdpbcgrAdapter rdpbcgrAdapter);

        /// <summary>
        /// EFS Initialization over DVC
        /// </summary>
        void EfsInitializationSequenceOverDVC();
    }
}
