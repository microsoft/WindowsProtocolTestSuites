// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Protocols.TestTools.StackSdk.Compression.Mppc;

namespace Microsoft.Protocols.TestTools.StackSdk.RemoteDesktop.Rdpbcgr
{
    /// <summary>
    /// Static virtual channel for RDP client
    /// </summary>
    public class ClientStaticVirtualChannel : StaticVirtualChannel
    {
        #region Variables

        /// <summary>
        /// A structure contains completed SVC data, which are sent from RDP server
        /// This variable is used to combine SVC data from a sequence of SVC PDUs
        /// </summary>
        private Virtual_Channel_Complete_Server_Pdu completeServerPdu;
        
        #endregion Variables

        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="id">Channel ID</param>
        /// <param name="name">Channel Name</param>
        /// <param name="options">Channel Options</param>
        /// <param name="chunkSize">Max chunk size</param>
        /// <param name="compressType">Compress Type</param>
        /// <param name="decompressType">Decompress Type</param>
        /// <param name="sender">Method used to send packet</param>
        public ClientStaticVirtualChannel(UInt16 id, 
                                string name,
                                Channel_Options options,
                                uint chunkSize,
                                CompressionType compressType,
                                CompressionType decompressType,
                                SendSVCData sender)
            :base(id, name, options, chunkSize, compressType, decompressType, sender)
        {            
        }

        #endregion Constructor

        #region Public Methods
                
        /// <summary>
        /// Process a static virtual channel received
        /// </summary>
        /// <param name="packet"></param>
        public override void ReceivePackets(StackPacket packet)
        {
            if (packet is Virtual_Channel_RAW_Server_Pdu)
            {
                Virtual_Channel_Complete_Server_Pdu reassembledPacket = ReassembleChunkData(packet as Virtual_Channel_RAW_Server_Pdu);
                if (reassembledPacket != null)
                {
                    ProcessSVCData(reassembledPacket.virtualChannelData);
                }
            }
        }

        #endregion Public Methods

        #region Internal Methods

        /// <summary>
        /// Reassemble static virtual channel PDU
        /// </summary>
        /// <param name="pdu"></param>
        /// <returns></returns>
        internal Virtual_Channel_Complete_Server_Pdu ReassembleChunkData(Virtual_Channel_RAW_Server_Pdu pdu)
        {
            if (pdu == null || pdu.virtualChannelData == null)
            {
                return null;
            }

            // the first chunk
            if ((pdu.channelPduHeader.flags & CHANNEL_PDU_HEADER_flags_Values.CHANNEL_FLAG_FIRST)
                == CHANNEL_PDU_HEADER_flags_Values.CHANNEL_FLAG_FIRST)
            {
                completeServerPdu = new Virtual_Channel_Complete_Server_Pdu();
                completeServerPdu.rawPdus = new System.Collections.ObjectModel.Collection<Virtual_Channel_RAW_Server_Pdu>();
                completeServerPdu.channelId = channelId;
                decompressedBuffer.Clear();
            }



            byte[] decompressedData = pdu.virtualChannelData;

            if (mppcDecompressor != null)   // has compression
            {
                CompressMode flag = CompressMode.None;

                if ((pdu.channelPduHeader.flags & CHANNEL_PDU_HEADER_flags_Values.CHANNEL_PACKET_AT_FRONT)
                    == CHANNEL_PDU_HEADER_flags_Values.CHANNEL_PACKET_AT_FRONT)
                {
                    flag |= CompressMode.SetToFront;
                }

                if ((pdu.channelPduHeader.flags & CHANNEL_PDU_HEADER_flags_Values.CHANNEL_PACKET_COMPRESSED)
                    == CHANNEL_PDU_HEADER_flags_Values.CHANNEL_PACKET_COMPRESSED)
                {
                    flag |= CompressMode.Compressed;
                }

                if ((pdu.channelPduHeader.flags & CHANNEL_PDU_HEADER_flags_Values.CHANNEL_PACKET_FLUSHED)
                    == CHANNEL_PDU_HEADER_flags_Values.CHANNEL_PACKET_FLUSHED)
                {
                    flag |= CompressMode.Flush;
                }

                if (flag != CompressMode.None)
                {
                    decompressedData = mppcDecompressor.Decompress(pdu.virtualChannelData, flag);
                }
            }


            if (completeServerPdu != null)
            {
                completeServerPdu.rawPdus.Add(pdu);
                decompressedBuffer.AddRange(decompressedData);
            }
            else
            {
                // not need to reassemble
                Virtual_Channel_Complete_Server_Pdu returnPDU = new Virtual_Channel_Complete_Server_Pdu();
                returnPDU.rawPdus = new System.Collections.ObjectModel.Collection<Virtual_Channel_RAW_Server_Pdu>();
                returnPDU.channelId = channelId;
                returnPDU.rawPdus.Add(pdu);
                returnPDU.virtualChannelData = decompressedData;
                return returnPDU;
            }

            // the last chunk
            if ((pdu.channelPduHeader.flags & CHANNEL_PDU_HEADER_flags_Values.CHANNEL_FLAG_LAST)
                == CHANNEL_PDU_HEADER_flags_Values.CHANNEL_FLAG_LAST)
            {
                if (decompressedBuffer != null)
                {
                    completeServerPdu.virtualChannelData = decompressedBuffer.ToArray();
                }
                Virtual_Channel_Complete_Server_Pdu returnPDU = completeServerPdu;
                completeServerPdu = null;
                return returnPDU;
            }

            return null;
        }

        #endregion Internal Methods
    }
}
