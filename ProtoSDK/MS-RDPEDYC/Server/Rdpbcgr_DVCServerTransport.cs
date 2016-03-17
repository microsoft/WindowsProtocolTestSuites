// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Protocols.TestTools.StackSdk.RemoteDesktop.Rdpbcgr;

namespace Microsoft.Protocols.TestTools.StackSdk.RemoteDesktop.Rdpedyc
{
    public class Rdpbcgr_DVCServerTransport : IDVCTransport
    {
        #region Variables

        private RdpbcgrServerSessionContext sessionContext;
        private StaticVirtualChannel channel;

        private ServerDecodingPduBuilder decoder;
        private PduBuilder pduBuilder;

        #endregion Variables

        #region Properties

        /// <summary>
        /// Transport Type
        /// </summary>
        public DynamicVC_TransportType TransportType
        {
            get
            {
                return DynamicVC_TransportType.RDP_TCP;
            }
        }

        #endregion Properties

        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="context"></param>
        public Rdpbcgr_DVCServerTransport(RdpbcgrServerSessionContext context)
        {
            this.sessionContext = context;

            if (sessionContext.SVCManager == null)
            {
                throw new NotSupportedException("Cannot get SVC from RDPBCGR connection for RDPEDYC, this transport must be created after RDPBCGR connection established.");                
            }

            channel = sessionContext.SVCManager.GetChannelByName(StaticVirtualChannelName.RDPEDYC);
            if (channel == null)
            {
                throw new NotSupportedException("Cannot get SVC from RDPBCGR connection for RDPEDYC, the static virtual channel is not created."); 
            }

            channel.Received += ReceivedBytes;

            if (!sessionContext.SVCManager.IsRunning)
            {
                // Better start the SVC manager here, so as to make sure the first packet of RDPEDYC can be processed
                // it is not same restrict as client, since the first packet is sent by server
                sessionContext.SVCManager.Start();
            }

            decoder = new ServerDecodingPduBuilder();
            pduBuilder = new PduBuilder();
        }

        #endregion Constructor

        #region Public Methods

        /// <summary>
        /// Send a Dynamic virtual channel PDU
        /// </summary>
        /// <param name="pdu"></param>
        public void Send(DynamicVCPDU pdu)
        {
            channel.Send(pduBuilder.ToRawData(pdu));
        }

        /// <summary>
        /// Event called when receive a Dynamic Virtual channel PDU
        /// </summary>
        public event ReceivePacket Received;

        /// <summary>
        /// Dispose
        /// </summary>
        public void Dispose()
        {
            channel.Received -= ReceivedBytes;
            if (sessionContext.SVCManager != null)
            {
                sessionContext.SVCManager.Dispose();
            }
        }

        #endregion Public Methods

        #region Private Methods

        /// <summary>
        /// Process bytes received
        /// </summary>
        /// <param name="data"></param>
        private void ReceivedBytes(byte[] data)
        {
            DynamicVCPDU pdu = decoder.ToPdu(data);
            if (Received != null)
            {
                Received(pdu);
            }
        }
        #endregion Private Methods
    }
}
