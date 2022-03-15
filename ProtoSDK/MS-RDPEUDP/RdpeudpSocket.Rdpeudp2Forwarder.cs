// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Protocols.TestTools.StackSdk.RemoteDesktop.Rdpeudp2;
using Microsoft.Protocols.TestTools.StackSdk.RemoteDesktop.Rdpeudp2.Types;
using System.Collections.Generic;
using System.Threading;

namespace Microsoft.Protocols.TestTools.StackSdk.RemoteDesktop.Rdpeudp
{
    public partial class RdpeudpSocket
    {
        // Indicates whether the current socket is upgraded to RDPEUDP2.
        protected bool upgradedToRdpedup2;

        public bool UpgradedToRdpeudp2 => upgradedToRdpedup2;

        // The handler to handle RDPEUDP2 packets.
        protected Rdpeudp2ProtocolHandler rdpeudp2Handler;

        public Rdpeudp2ProtocolHandler Rdpeudp2Handler => rdpeudp2Handler;

        // The buffer to cache unprocessed packets during the upgrade to RDPEUDP2.
        protected List<StackPacket> rdpeudp2UpgradeBuffer = new List<StackPacket>();

        /// <summary>
        /// Receive a RDPEUDP2 packet from the underlaying UDP transport.
        /// </summary>
        /// <param name="eudp2Packet">The packet to be received.</param>
        public void ReceivePacket(Rdpeudp2Packet eudp2Packet)
        {
            rdpeudp2Handler.ReceivePacket(eudp2Packet);
        }

        /// <summary>
        /// Send a RDPEUDP2 packet to the underlaying UDP transport.
        /// </summary>
        /// <param name="eudp2Packet">The packet to be sent.</param>
        public void SendPacket(Rdpeudp2Packet eudp2Packet)
        {
            rdpeudp2Handler.SendPacket(eudp2Packet);
        }

        /// <summary>
        /// The higher layer protocol is receiving data from RDPEUDP2 layer.
        /// </summary>
        /// <param name="data">The data to be sent from RDPEUDP2 layer to higher layer.</param>
        public void ReceiveDataOnHigherLayer(byte[] data)
        {
            SpinWait.SpinUntil(() => Received != null);

            Received?.Invoke(data);
        }
    }
}
