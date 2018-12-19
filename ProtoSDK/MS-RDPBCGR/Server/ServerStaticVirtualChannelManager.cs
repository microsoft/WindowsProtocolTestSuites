// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Microsoft.Protocols.TestTools.StackSdk.RemoteDesktop.Rdpbcgr
{
    /// <summary>
    /// Static virtual channel manager for RDP Server
    /// </summary>
    public class ServerStaticVirtualChannelManager : StaticVirtualChannelManager
    {
        #region Variables

        /// <summary>
        /// RDPBCGR Server session context
        /// </summary>
        private RdpbcgrServerSessionContext context;

        #endregion Variables

        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="context"></param>
        public ServerStaticVirtualChannelManager(RdpbcgrServerSessionContext context)
        {
            this.context = context;
            this.channelDicById = new Dictionary<ushort, StaticVirtualChannel>();
            this.channelDicByName = new Dictionary<string, StaticVirtualChannel>();
            ushort[] virtualChannelIds = context.VirtualChannelIdStore;
            CHANNEL_DEF[] virtualChannelDefines = context.VirtualChannelDefines;
            if (virtualChannelIds != null && virtualChannelDefines != null
                && virtualChannelIds.Length == virtualChannelDefines.Length)
            {
                CompressionType compressType = context.VirtualChannelSCCompressionType;
                if (context.CompressionTypeSupported == CompressionType.PACKET_COMPR_TYPE_NONE)
                {
                    compressType = CompressionType.PACKET_COMPR_TYPE_NONE;
                }
                else if (context.CompressionTypeSupported == CompressionType.PACKET_COMPR_TYPE_RDP6 ||
                      context.CompressionTypeSupported == CompressionType.PACKET_COMPR_TYPE_RDP61)
                {
                    compressType = CompressionType.PACKET_COMPR_TYPE_64K;
                }

                for (int i = 0; i < virtualChannelIds.Length; ++i)
                {
                    string name = virtualChannelDefines[i].name;
                    if (name != null)
                    {
                        name = name.Replace("\0", string.Empty).ToUpper();
                    }
                    StaticVirtualChannel channel = new ServerStaticVirtualChannel(virtualChannelIds[i],
                                                                name,
                                                                virtualChannelDefines[i].options,
                                                                context.VCChunkSize,
                                                                compressType,
                                                                context.VirtualChannelCSCompressionType,
                                                                SendPacket);
                    channelDicById.Add(virtualChannelIds[i], channel);
                    channelDicByName.Add(name, channel);
                }
            }
        }

        #endregion Constructor

        #region Public Methods

        /// <summary>
        /// Send static virtual channel data
        /// </summary>
        /// <param name="channelId">Channel ID</param>
        /// <param name="channelPduHeader">Channel PDU Header</param>
        /// <param name="SVCData"></param>
        public void SendPacket(UInt16 channelId, CHANNEL_PDU_HEADER channelPduHeader, byte[] SVCData)
        {
            Virtual_Channel_RAW_Server_Pdu rawPdu = new Virtual_Channel_RAW_Server_Pdu(context);
            rawPdu.channelPduHeader = channelPduHeader;
            rawPdu.virtualChannelData = SVCData;

            RdpbcgrUtility.FillCommonHeader(ref rawPdu.commonHeader,
                                     TS_SECURITY_HEADER_flags_Values.SEC_IGNORE_SEQNO
                                     | TS_SECURITY_HEADER_flags_Values.SEC_RESET_SEQNO,
                                     context,
                                     channelId);

            context.Server.SendPdu(context, rawPdu);
        }

        /// <summary>
        /// Expect a static virtual channel Packet
        /// </summary>
        /// <param name="timeout"></param>
        /// <param name="channelId"></param>
        /// <returns></returns>
        public override StackPacket ExpectPacket(TimeSpan timeout, out UInt16 channelId)
        {
            DateTime endTime = DateTime.Now + timeout;
            channelId = 0;            
            while (DateTime.Now < endTime)
            {
                RdpbcgrClientPdu pdu = context.Server.ExpectChannelPdu(context, endTime-DateTime.Now);
                if (pdu is Virtual_Channel_RAW_Pdu)
                {
                    channelId = (pdu as Virtual_Channel_RAW_Pdu).commonHeader.channelId;
                    return pdu;
                }
            }
            return null;
        }

        #endregion Public Methods

        #region Internal Methods

        /// <summary>
        /// Reasemble Chunk Data
        /// </summary>
        /// <param name="pdu"></param>
        /// <returns></returns>
        internal Virtual_Channel_Complete_Pdu ReassembleChunkData(Virtual_Channel_RAW_Pdu pdu)
        {
            if (!channelDicById.ContainsKey(pdu.commonHeader.channelId))
            {
                // drop the pdu if the channel id does not exist
                //return null;
                throw new ArgumentException("The channel id of the pdu does not exist!");
            }
            ServerStaticVirtualChannel channel = (ServerStaticVirtualChannel)channelDicById[pdu.commonHeader.channelId];
            return channel.ReassembleChunkData(pdu);
        }

        #endregion Internal Methods

    }
}
