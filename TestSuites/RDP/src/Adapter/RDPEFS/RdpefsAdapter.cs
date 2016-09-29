// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using Microsoft.Protocols.TestSuites.Rdpbcgr;
using Microsoft.Protocols.TestTools;
using Microsoft.Protocols.TestTools.StackSdk.RemoteDesktop.Rdpedyc;
using Microsoft.Protocols.TestTools.StackSdk.RemoteDesktop.Rdpefs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Microsoft.Protocols.TestSuites.Rdp.Rdpefs
{
    public class RdpefsAdapter : ManagedAdapterBase, IRdpefsAdapter
    {
        #region Private Variables
        private const string RdpefsChannelName = "rdpdr";

        RdpefsServer rdpefsServer;
        IRdpbcgrAdapter rdpbcgrAdapter;
        TimeSpan waitTime;
        #endregion 
        
        #region IAdapter Members
        public override void Initialize(ITestSite testSite)
        {
            base.Initialize(testSite);
            rdpbcgrAdapter = null;
            this.rdpefsServer = new RdpefsServer();
            waitTime = new TimeSpan(0, 0, 40);

            #region WaitTime
            string strWaitTime = Site.Properties["WaitTime"];
            if (strWaitTime != null)
            {
                int waitSeconds = Int32.Parse(strWaitTime);
                waitTime = new TimeSpan(0, 0, waitSeconds);
            }
            else
            {
                waitTime = new TimeSpan(0, 0, 40);
            }
            #endregion 
        }

        public override void Reset()
        {
            base.Reset();
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
           
        }
        #endregion 

        #region Constructor
        //public RdpefsAdapter() 
        //{
        //    rdpefsServer = new RdpefsServer();
        //    waitTime = new TimeSpan(0, 0, 40); 
        //}
        #endregion 

        #region IRdpefsAdapter Methods
        /// <summary>
        /// Attach a RdpbcgrAdapter object
        /// </summary>
        /// <param name="rdpbcgrAdapter">the source RdpbcgrAdapter object</param>
        public void AttachRdpbcgrAdapter(IRdpbcgrAdapter rdpbcgrAdapter)
        {
            this.rdpbcgrAdapter = rdpbcgrAdapter;
        }

        /// <summary>
        /// Initialize this protocol with create control and data channels.
        /// </summary>
        /// <param name="rdpedycServer">RDPEDYC Server instance</param>
        /// <param name="transportType">selected transport type for created channels</param>
        /// <returns>true if client supports this protocol; otherwise, return false.</returns>
        public bool ProtocolInitialize(RdpedycServer rdpedycServer, DynamicVC_TransportType transportType = DynamicVC_TransportType.RDP_UDP_Reliable)
        {
            if (!rdpedycServer.IsMultipleTransportCreated(transportType))
            {
                rdpedycServer.CreateMultipleTransport(transportType);
            }

            this.rdpefsServer = new RdpefsServer(rdpedycServer);
            bool success = false;

            // Create RDPEFS channel
            try
            {
                uint channelId = DynamicVirtualChannel.NewChannelId();
                Dictionary<TunnelType_Value, List<uint>> channelListDic = new Dictionary<TunnelType_Value, List<uint>>();
                List<uint> list = new List<uint>();
                list.Add(channelId);
                channelListDic.Add(TunnelType_Value.TUNNELTYPE_UDPFECR, list);
                rdpedycServer.SoftSyncNegotiate(waitTime, channelListDic);

                success = rdpefsServer.CreateRdpefsDvc(waitTime, channelId);                
            }
            catch (Exception e)
            {
                Site.Log.Add(LogEntryKind.Comment, "Exception occurred when creating RDPEFS channels: {1}", e.Message);
            }

            return success;
        }

        /// <summary>
        /// Generate static virtual channel data messages for test.
        /// MS-RDPEFS is used to generated virtual channel traffics.
        /// </summary>
        /// <param name="RDPDR_ChannelId">Static Channel Id for RDPDR</param>
        /// <param name="invalidType">Invalid Type</param>
        public void GenerateStaticVirtualChannelTraffics(StaticVirtualChannel_InvalidType invalidType = StaticVirtualChannel_InvalidType.None)
        {
            /*
             * MS-RDPEFS protocol Initialization.
             */

            byte[] receivedData = null;
            uint clientId = 0;

            UInt16 channelId = this.rdpbcgrAdapter.RDPDRChannelId;

            if (invalidType == StaticVirtualChannel_InvalidType.None)
            {
                //Sending Server Announce Request.
                byte[] data = rdpefsServer.EncodeServerPdu<DR_CORE_SERVER_ANNOUNCE_REQ>(CreateServerAnnounceReqest());
                this.rdpbcgrAdapter.SendVirtualChannelPDU(channelId, data, StaticVirtualChannel_InvalidType.None);

                //Expecting  Client Announce Reply.
                this.rdpbcgrAdapter.WaitForVirtualChannelPdu(channelId, out receivedData, waitTime);
                DR_CORE_CLIENT_ANNOUNCE_RSP reply = new DR_CORE_CLIENT_ANNOUNCE_RSP();
                bool fsuccess = rdpefsServer.DecodeClientPdu<DR_CORE_CLIENT_ANNOUNCE_RSP>(receivedData, reply);
                clientId = reply.ClientId;

                //Expecting Client Name Request.
                this.rdpbcgrAdapter.WaitForVirtualChannelPdu(channelId, out receivedData, waitTime);
                DR_CORE_CLIENT_NAME_REQ req = new DR_CORE_CLIENT_NAME_REQ();
                fsuccess = rdpefsServer.DecodeClientPdu<DR_CORE_CLIENT_NAME_REQ>(receivedData, req);

                //Sending Server Core Capability Request.
                data = rdpefsServer.EncodeServerPdu<DR_CORE_CAPABILITY_REQ>(CreateServerCoreCapabilityRequest());
                this.rdpbcgrAdapter.SendVirtualChannelPDU(channelId, data, StaticVirtualChannel_InvalidType.None);

                //Sending Server Client ID Confirm.
                data = rdpefsServer.EncodeServerPdu<DR_CORE_SERVER_CLIENTID_CONFIRM>(CreateServerClientIdConfirm(clientId));
                this.rdpbcgrAdapter.SendVirtualChannelPDU(channelId, data, StaticVirtualChannel_InvalidType.None);

                //Expecting Client Core Capability Response.
                this.rdpbcgrAdapter.WaitForVirtualChannelPdu(channelId, out receivedData, waitTime);
                DR_CORE_CAPABILITY_RSP capRsp = new DR_CORE_CAPABILITY_RSP();
                fsuccess = rdpefsServer.DecodeClientPdu<DR_CORE_CAPABILITY_RSP>(receivedData, capRsp);

                bool supportUserLogonPacket = false;
                foreach (CAPABILITY_SET capSet in capRsp.CapabilityMessage)
                {
                    if (capSet is GENERAL_CAPS_SET)
                    {
                        if (((GENERAL_CAPS_SET)capSet).extendedPDU.HasFlag(extendedPDU_Values.RDPDR_USER_LOGGEDON_PDU))
                        {
                            supportUserLogonPacket = true;
                        }
                    }
                }

                if (supportUserLogonPacket)
                {
                    // Send Server User logged on packet
                    data = rdpefsServer.EncodeServerPdu<DR_CORE_USER_LOGGEDON>(new DR_CORE_USER_LOGGEDON());
                    this.rdpbcgrAdapter.SendVirtualChannelPDU(channelId, data, StaticVirtualChannel_InvalidType.None);
                }

                //Expecting Client Device List.
                this.rdpbcgrAdapter.WaitForVirtualChannelPdu(channelId, out receivedData, waitTime);
                DR_CORE_DEVICELIST_ANNOUNCE_REQ announceReq = new DR_CORE_DEVICELIST_ANNOUNCE_REQ();
                fsuccess = rdpefsServer.DecodeClientPdu<DR_CORE_DEVICELIST_ANNOUNCE_REQ>(receivedData, announceReq);
            }
            else
            {
                //Sending Server Announce Request.
                byte[] data = rdpefsServer.EncodeServerPdu<DR_CORE_SERVER_ANNOUNCE_REQ>(CreateServerAnnounceReqest());
                this.rdpbcgrAdapter.SendVirtualChannelPDU(channelId, data, invalidType);
                //this.rdpbcgrAdapter.WaitForVirtualChannelPdu(channelId, out receivedData, timeout);
            }
        }

        /// <summary>
        /// Send and receive MS-RDPEFS data over DVC.
        /// </summary>
        public void EfsInitializationSequenceOverDVC()
        {
            //Sending Server Announce Request.
            SendServerPDUOverDVC(PacketId_Values.PAKID_CORE_SERVER_ANNOUNCE);

            //Expecting  Client Announce Reply.
            DR_CORE_CLIENT_ANNOUNCE_RSP AnnounceRsp = ExpectClientPduOverDVC<DR_CORE_CLIENT_ANNOUNCE_RSP>();
            uint clientId = AnnounceRsp.ClientId;

            //Expecting Client Name Request.
            DR_CORE_CLIENT_NAME_REQ NameReq = ExpectClientPduOverDVC<DR_CORE_CLIENT_NAME_REQ>();

            //Sending Server Core Capability Request.
            SendServerPDUOverDVC(PacketId_Values.PAKID_CORE_SERVER_CAPABILITY);

            //Sending Server Client ID Confirm.
            SendServerPDUOverDVC(PacketId_Values.PAKID_CORE_CLIENTID_CONFIRM, clientId);

            //Expecting Client Core Capability Response.
            DR_CORE_CAPABILITY_RSP capRsp = ExpectClientPduOverDVC<DR_CORE_CAPABILITY_RSP>();

        }
        #endregion

        #region private Create PDU methods
        // Sending Server Announce Request.
        private void SendServerPDUOverDVC(PacketId_Values packetType, uint? clientId = null)
        {
            RdpefsPDU pdu = null;
            switch (packetType)
            {
                case PacketId_Values.PAKID_CORE_SERVER_ANNOUNCE:
                    pdu = CreateServerAnnounceReqest();
                    break;
                case PacketId_Values.PAKID_CORE_SERVER_CAPABILITY:
                    pdu = CreateServerCoreCapabilityRequest();
                    break;
                case PacketId_Values.PAKID_CORE_CLIENTID_CONFIRM:
                    pdu = CreateServerClientIdConfirm(clientId);
                    break;
                case PacketId_Values.PAKID_CORE_USER_LOGGEDON:
                    pdu = new DR_CORE_USER_LOGGEDON();
                    break;
            }

            if (pdu != null && rdpefsServer != null)
            {
                rdpefsServer.SendRdpefsPdu(pdu);
            }
            else
            {
                throw new InvalidOperationException("The sending pdu type is null: " + packetType.ToString());
            }
        }

        private T ExpectClientPduOverDVC<T>() where T : RdpefsPDU
        {
            T pdu = rdpefsServer.ExpectRdpefsPdu<T>(waitTime);
            return pdu;
        }

        private DR_CORE_SERVER_ANNOUNCE_REQ CreateServerAnnounceReqest()
        {
            DR_CORE_SERVER_ANNOUNCE_REQ req = new DR_CORE_SERVER_ANNOUNCE_REQ();
            req.VersionMinor = VersionMinor_Values.V1;
            req.ClientId = 1;
            return req;
        }

        private DR_CORE_CAPABILITY_REQ CreateServerCoreCapabilityRequest()
        {
            DR_CORE_CAPABILITY_REQ req = new DR_CORE_CAPABILITY_REQ();
            req.numCapabilities = 5;
            List<CAPABILITY_SET> capabilitySet = new List<CAPABILITY_SET>();

            GENERAL_CAPS_SET generalCapability = new GENERAL_CAPS_SET(CAPABILITY_VERSION.V1);
            generalCapability.osType = osType_Values.OS_TYPE_UNKNOWN;
            generalCapability.osVersion = osVersion_Values.V1;
            generalCapability.protocolMinorVersion = DR_CORE_SERVER_CLIENTID_CONFIRM_VersionMinor_Values.V1;
            generalCapability.ioCode1 = (ioCode1_Values)0x0000FFFF;
            generalCapability.extendedPDU = extendedPDU_Values.RDPDR_DEVICE_REMOVE_PDUS | extendedPDU_Values.RDPDR_CLIENT_DISPLAY_NAME_PDU;
            generalCapability.SpecialTypeDeviceCap = 2;
            capabilitySet.Add(generalCapability);

            capabilitySet.Add(new PRINTER_CAPS_SET());

            capabilitySet.Add(new PORT_CAPS_SET());

            capabilitySet.Add(new DRIVE_CAPS_SET());

            capabilitySet.Add(new SMARTCARD_CAPS_SET());

            req.CapabilityMessage = capabilitySet.ToArray();
            return req;
        }

        private DR_CORE_SERVER_CLIENTID_CONFIRM CreateServerClientIdConfirm(uint? clientId = null)
        {
            if (clientId == null)
                throw new InvalidOperationException("ClientId is null.");

            DR_CORE_SERVER_CLIENTID_CONFIRM req = new DR_CORE_SERVER_CLIENTID_CONFIRM();
            req.VersionMinor = DR_CORE_SERVER_CLIENTID_CONFIRM_VersionMinor_Values.V1;
            req.ClientId = (uint)clientId;
            return req;
        }

        #endregion 

    }
}
