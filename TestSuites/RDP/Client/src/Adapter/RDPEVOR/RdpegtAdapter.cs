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
    public class RdpegtAdapter : ManagedAdapterBase, IRdpegtAdapter
    {
        private const string RdpegtChannelName = "Microsoft::Windows::RDS::Geometry::v08.01";        
        RdpegtServer rdpegtServer;
        TimeSpan waitTime;
        

        #region IAdapter Members

        public override void Initialize(ITestSite testSite)
        {
            base.Initialize(testSite);

            #region WaitTime
            string strWaitTime = Site.Properties["WaitTime"];
            if (strWaitTime != null)
            {
                int waitSeconds = Int32.Parse(strWaitTime);
                waitTime = new TimeSpan(0, 0, waitSeconds);
            }
            else
            {
                waitTime = new TimeSpan(0, 0, 20);
            }
            #endregion
            
        }

        public override void Reset()
        {
            base.Reset();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                //Do Something.
            }

            base.Dispose(disposing);
        }

        #endregion


        #region IRdpegtAdapter Members
        
        /// <summary>
        /// Initialize this protocol with create control and data channels.
        /// </summary>
        /// <param name="rdpedycServer">RDPEDYC Server instance</param>
        /// <param name="transportType">selected transport type for created channels</param>
        /// <returns>true if client supports this protocol; otherwise, return false.</returns>
        public bool ProtocolInitialize(RdpedycServer rdpedycServer, DynamicVC_TransportType transportType = DynamicVC_TransportType.RDP_TCP)
        {
            if (!rdpedycServer.IsMultipleTransportCreated(transportType))
            {
                rdpedycServer.CreateMultipleTransport(transportType);
            }

            this.rdpegtServer = new RdpegtServer(rdpedycServer);

            bool success = false;

            // Create RDPEGT Channel
            try
            {
                success = rdpegtServer.CreateRdpegtDvc(waitTime);
            }
            catch (Exception e)
            {
                Site.Log.Add(LogEntryKind.Comment, "Exception occurred when creating RDPEGT channels: {1}", e.Message);
            }
            
            return success;
        }

        /// <summary>
        /// Send a MAPPED_GEOMETRY_PACKET to client to create a geometry mapping to a rect.
        /// </summary>
        /// <param name="mappingId">The id for the geometry mapping.</param>
        /// <param name="updateType">The update type.</param>
        /// <param name="left">The position to left bound.</param>
        /// <param name="top">The position to right bound.</param>
        /// <param name="width">The width of the rect.</param>
        /// <param name="height">The height of the rect.</param>
        public void SendMappedGeometryPacket(UInt64 mappingId, UpdateTypeValues updateType, uint left, uint top, uint width, uint height)
        {
            MAPPED_GEOMETRY_PACKET geometry = rdpegtServer.CreateMappedGeometryPacket(mappingId, updateType, left, top, width, height);
            rdpegtServer.SendRdpegtPdu(geometry);
        }

        #endregion

    }
}
