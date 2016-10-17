// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Protocols.TestTools.StackSdk.Compression.Mppc;
using Microsoft.Protocols.TestTools.ExtendedLogging;

namespace Microsoft.Protocols.TestTools.StackSdk.RemoteDesktop.Rdpbcgr
{
    public class StaticVirtualChannelName
    {
        public static string RDPEDYC = "DRDYNVC";
    }

    /// <summary>
    /// Send SVC Data
    /// </summary>
    /// <param name="channelId">Static virtual channel ID</param>
    /// <param name="channelHeader">Channel_PDU_Header</param>
    /// <param name="SVCData">Static virtual channel data to be sent</param>
    public delegate void SendSVCData(UInt16 channelId, CHANNEL_PDU_HEADER channelHeader, byte[] SVCData);

    /// <summary>
    /// Process received data
    /// </summary>
    /// <param name="data"></param>
    public delegate void ReceiveData(byte[] data);

    /// <summary>
    /// Base class for Static Virtual Channel
    /// </summary>
    public abstract class StaticVirtualChannel
    {
        #region Variables

        /// <summary>
        /// Channel ID of this static virtual channel
        /// </summary>
        protected UInt16 channelId;

        /// <summary>
        /// Channel Name of this static virtual channel
        /// </summary>
        protected string channelName;

        /// <summary>
        /// Channel options
        /// </summary>
        protected Channel_Options channelOptions;
                
        protected Compressor mppcCompressor;
        protected Decompressor mppcDecompressor;

        /// <summary>
        /// Method used to send SVC data
        /// </summary>
        protected SendSVCData Sender;

        /// <summary>
        /// How much data could be put into a chunk
        /// </summary>
        protected uint maxChunkSize;

        /// <summary>
        /// Contains the decompressed data.
        /// </summary>
        protected List<byte> decompressedBuffer;

        #endregion Variables

        #region Properties

        /// <summary>
        /// Channel ID
        /// </summary>
        public UInt16 ChannelId
        {
            get
            {
                return channelId;
            }
        }

        /// <summary>
        /// Channel Name
        /// </summary>
        public string ChannelName
        {
            get
            {
                return channelName;
            }
        }

        /// <summary>
        /// Channel Options
        /// </summary>
        public Channel_Options ChannelOptions
        {
            get
            {
                return channelOptions;
            }
        }

        #endregion Properties

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
        public  StaticVirtualChannel(UInt16 id, 
                                string name,
                                Channel_Options options,
                                uint chunkSize,
                                CompressionType compressType,
                                CompressionType decompressType,
                                SendSVCData sender)
        {
            this.channelId = id;
            this.channelName = name;
            this.channelOptions = options;
            this.maxChunkSize = chunkSize;
            this.decompressedBuffer = new List<byte>();

            if (compressType != CompressionType.PACKET_COMPR_TYPE_NONE)
            {
                mppcCompressor = new Compressor((SlidingWindowSize)compressType);
            }

            if (decompressType != CompressionType.PACKET_COMPR_TYPE_NONE)
            {
                mppcDecompressor = new Decompressor((SlidingWindowSize)decompressType);
            }
            this.Sender = sender;
        }

        #endregion Constructor

        #region Public methods

        /// <summary>
        /// Send data from this channel
        /// </summary>
        /// <param name="data"></param>
        public void Send(byte[] data)
        {
            if (data == null || data.Length == 0)
            {
                return;
            }

            ChannelChunk[] chunks = SplitToChunks(data);
            foreach (ChannelChunk chunk in chunks)
            {
                Sender(ChannelId, chunk.channelPduHeader, chunk.chunkData);
            }

            // ETW Provider Dump message
            string messageName = "RDPBCGR:SVC Sent Data";
            ExtendedLogger.DumpMessage(messageName, RdpbcgrUtility.DumpLevel_Layer1, "RDPBCGR: Static Virtual Channel Sent Data", data);
        }

        /// <summary>
        /// Event called when received SVC data
        /// </summary>
        public event ReceiveData Received;

        /// <summary>
        /// Process received static virtual channel packet
        /// </summary>
        /// <param name="packet"></param>
        public abstract void ReceivePackets(StackPacket packet);

        #endregion Public methods

        #region Protected Methods

        /// <summary>
        /// Process Static virtual channel data
        /// </summary>
        /// <param name="data"></param>
        protected void ProcessSVCData(byte[] data)
        {
            if (this.Received != null)
            {
                Received(data);
            }

            // ETW Provider Dump message
            string messageName = "RDPBCGR:SVC Received Data";
            ExtendedLogger.DumpMessage(messageName, RdpbcgrUtility.DumpLevel_Layer1, "RDPBCGR: Static Virtual Channel Received Data", data);
        }

        #endregion Protected Methods

        #region Internal Methods

        /// <summary>
        /// Split and compress the complete virtual channel data into chunk data.
        /// </summary>
        /// <param name="completeData">The compete virtual channel data. This argument can be null.</param>
        /// <returns>The splitted chunk data.</returns>
        internal ChannelChunk[] SplitToChunks(byte[] completeData, int maxBit  =16)
        {
            if (completeData == null || completeData.Length <= 0)
            {
                return null;
            }

            // calculate the number of the chunks
            int chunkNum = (int)(completeData.Length / maxChunkSize);
            if ((completeData.Length % maxChunkSize) != 0)
            {
                chunkNum++;
            }

            ChannelChunk[] chunks = new ChannelChunk[chunkNum];
            
            uint chunkSize = maxChunkSize;

            // fill the chunks except the last one
            for (int i = 0; i < chunkNum; i++)
            {
                if (i == chunkNum - 1)   // the last chunk
                {
                    chunkSize = (uint)(completeData.Length - maxChunkSize * i);
                }

                byte[] chunkData = new byte[chunkSize];
                Array.Copy(completeData, maxChunkSize * i, chunkData, 0, chunkSize);
                chunks[i].channelPduHeader.length = (uint)completeData.Length;
                if (mppcCompressor != null && channelOptions == Channel_Options.COMPRESS)   // has compression
                {
                    CompressMode flag;
                    chunks[i].chunkData = mppcCompressor.Compress(chunkData, out flag);

                    if ((flag & CompressMode.Compressed) == CompressMode.Compressed)
                    {
                        chunks[i].channelPduHeader.flags |= CHANNEL_PDU_HEADER_flags_Values.CHANNEL_PACKET_COMPRESSED;
                    }

                    if ((flag & CompressMode.Flush) == CompressMode.Flush)
                    {
                        chunks[i].channelPduHeader.flags |= CHANNEL_PDU_HEADER_flags_Values.CHANNEL_PACKET_FLUSHED;
                    }

                    if ((flag & CompressMode.SetToFront) == CompressMode.SetToFront)
                    {
                        chunks[i].channelPduHeader.flags |= CHANNEL_PDU_HEADER_flags_Values.CHANNEL_PACKET_AT_FRONT;
                    }
                }
                else                          // no compression
                {
                    chunks[i].chunkData = chunkData;
                }
            }
            
            // set the first chunk CHANNEL_FLAG_FIRST
            chunks[0].channelPduHeader.flags |= CHANNEL_PDU_HEADER_flags_Values.CHANNEL_FLAG_FIRST;
            // the last chunk
            chunks[chunkNum - 1].channelPduHeader.flags |= CHANNEL_PDU_HEADER_flags_Values.CHANNEL_FLAG_LAST;
            

            return chunks;
        }

        #endregion Internal Methods

    }
}
