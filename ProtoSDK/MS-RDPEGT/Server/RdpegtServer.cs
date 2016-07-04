// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Protocols.TestTools.StackSdk.RemoteDesktop.Rdpedyc;

namespace Microsoft.Protocols.TestTools.StackSdk.RemoteDesktop.Rdpegt
{
    /// <summary>
    /// RDPEGT Server
    /// Server role of RDPEGT protocol
    /// </summary>
    public class RdpegtServer
    {
        #region Variables

        private const string RdpegtChannelName = "Microsoft::Windows::RDS::Geometry::v08.01";
        private RdpedycServer rdpedycServer;
        private DynamicVirtualChannel rdpegtDVC;

        #endregion Variables

        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="rdpedycServer"></param>
        public RdpegtServer(RdpedycServer rdpedycServer)
        {
            this.rdpedycServer = rdpedycServer;
        }

        #endregion Constructor

        /// <summary>
        /// Create dynamic virtual channel.
        /// </summary>
        /// <param name="transportType">selected transport type for created channels</param>
        /// <param name="timeout">Timeout</param>
        /// <returns>true if client supports this protocol; otherwise, return false.</returns>
        public bool CreateRdpegtDvc(TimeSpan timeout, DynamicVC_TransportType transportType = DynamicVC_TransportType.RDP_TCP)
        {

            const ushort priority = 0;
            
            rdpegtDVC = rdpedycServer.CreateChannel(timeout, priority, RdpegtChannelName, transportType, null);
            
            if (rdpegtDVC != null)
            {
                return true;
            }
            return false;
        }

        #region Send/Receive Methods

        /// <summary>
        /// Send a RDPEGT Packet
        /// </summary>
        /// <param name="pdu"></param>
        public void SendRdpegtPdu(MAPPED_GEOMETRY_PACKET pdu)
        {
            byte[] data = PduMarshaler.Marshal(pdu);
            if (rdpegtDVC == null)
            {
                throw new InvalidOperationException("DVC instance of RDPEGT is null, Dynamic virtual channel must be created before sending data.");
            }
            rdpegtDVC.Send(data);
        }

        // RDPEGT only contains a server-to-client message, server receive nothing

        #endregion Send/Receive Methods

        #region Create Methods

        /// <summary>
        /// Create a MAPPED_GEOMETRY_PACKET
        /// </summary>
        /// <param name="mappingId">The id for the geometry mapping.</param>
        /// <param name="updateType">The update type.</param>
        /// <param name="left">The position to left bound.</param>
        /// <param name="top">The position to right bound.</param>
        /// <param name="width">The width of the rect.</param>
        /// <param name="height">The height of the rect.</param>
        /// <returns>MAPPED_GEOMETRY_PACKET created</returns>
        public MAPPED_GEOMETRY_PACKET CreateMappedGeometryPacket(UInt64 mappingId, UpdateTypeValues updateType, uint left, uint top, uint width, uint height)
        {
            MAPPED_GEOMETRY_PACKET geometry = new MAPPED_GEOMETRY_PACKET();

            if (updateType == UpdateTypeValues.GEOMETRY_UPDATE)
            {
                geometry.cbGeometryData = 0x79;
                geometry.Version = RdpegtVersionValues.RDP8;
                geometry.MappingId = mappingId;
                geometry.UpdateType = updateType;
                geometry.Flags = 0x0;
                geometry.TopLevelId = mappingId;
                geometry.Left = 0;
                geometry.Top = 0;
                geometry.Right = width;
                geometry.Bottom = height;
                geometry.TopLevelLeft = left;
                geometry.TopLevelTop = top;
                geometry.TopLevelRight = (uint)(left + width); ;
                geometry.TopLevelBottom = (uint)(top + height); ;
                geometry.GeometryType = GeometryTypeValues.RDP8;
                geometry.cbGeometryBuffer = 0x0030;
                geometry.pGeometryBuffer = new RGNDATA();
                geometry.pGeometryBuffer.rdh.dwSize = 32;
                geometry.pGeometryBuffer.rdh.iType = 0x0001;
                geometry.pGeometryBuffer.rdh.nCount = 0x0001;
                geometry.pGeometryBuffer.rdh.nRgnSize = 0;
                geometry.pGeometryBuffer.rdh.rcBound.left = (int)left;
                geometry.pGeometryBuffer.rdh.rcBound.top = (int)top;
                geometry.pGeometryBuffer.rdh.rcBound.right = (int)(left + width);
                geometry.pGeometryBuffer.rdh.rcBound.bottom = (int)(top + height);
                geometry.pGeometryBuffer.Buffer = new RECT[1];
                RECT rct = new RECT();
                rct.left = 0;
                rct.top = 0;
                rct.right = (int)width;
                rct.bottom = (int)height;
                geometry.pGeometryBuffer.Buffer[0] = rct;
                geometry.Reserved2 = 0;
            }
            else if (updateType == UpdateTypeValues.GEOMETRY_CLEAR)
            {
                geometry.cbGeometryData = 0x0049;
                geometry.Version = RdpegtVersionValues.RDP8;
                geometry.MappingId = mappingId;
                geometry.UpdateType = updateType;
                geometry.cbGeometryBuffer = 0;
                geometry.Reserved2 = 0;
            }
            
            return geometry;
        }
        #endregion Create Methods
    }
}
