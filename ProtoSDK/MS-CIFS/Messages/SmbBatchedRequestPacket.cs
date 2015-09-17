// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.IO;

using Microsoft.Protocols.TestTools.StackSdk.Messages;

namespace Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Cifs
{
    /// <summary>
    /// the basic packet of SMB batched request.
    /// defined common method of all SMB batched request packets
    /// </summary>
    public abstract class SmbBatchedRequestPacket : SmbPacket
    {
        #region fields

        /// <summary>
        /// the smb andx Packet
        /// </summary>
        protected SmbPacket andxPacket;

        #endregion


        #region properties

        /// <summary>
        /// get or set the andx packet.
        /// </summary>
        public SmbPacket AndxPacket
        {
            get
            {
                return this.andxPacket;
            }
            set
            {
                this.andxPacket = value;
            }
        }


        /// <summary>
        /// Get the type of the packet: Request or Response, and Single or Compounded.
        /// </summary>
        /// <returns>the type of the packet.</returns>
        public override SmbPacketType PacketType
        {
            get
            {
                return SmbPacketType.BatchedRequest;
            }
        }


        /// <summary>
        /// the SmbCommand of the andx packet.
        /// </summary>
        protected abstract SmbCommand AndxCommand
        {
            get;
        }


        /// <summary>
        /// Used to set the AndXOffset field of child class
        /// </summary>
        protected abstract ushort AndXOffset
        {
            get;
            set;
        }

        #endregion


        #region constructor and destructor

        /// <summary>
        /// Constructor.
        /// </summary>
        protected SmbBatchedRequestPacket()
            : base()
        {
            this.AndxPacket = null;
        }


        /// <summary>
        /// Constructor.
        /// </summary>
        protected SmbBatchedRequestPacket(byte[] data)
            : base(data)
        {
        }


        /// <summary>
        /// Deep copy constructor.
        /// </summary>
        protected SmbBatchedRequestPacket(SmbBatchedRequestPacket packet)
            : base(packet)
        {
            lock (packet)
            {
                if (packet.andxPacket != null)
                {
                    this.andxPacket = packet.AndxPacket.Clone() as SmbPacket;
                }
            }
        }

        #endregion


        #region encoding

        /// <summary>
        /// to encode the SmbParameters and SmbDada into bytes.
        /// </summary>
        /// <returns>the bytes array of SmbParameters, SmbDada, and AndX if existed.</returns>
        protected byte[] GetBytesWithoutHeader(bool isHeaderRequest, ushort andXOffset)
        {
            this.AndXOffset = (ushort)(this.ParametersSize + this.DataSize + andXOffset);

            if (isHeaderRequest)
            {
                this.AndXOffset += (ushort)(this.HeaderSize);
            }

            // get bytes of andx:
            byte[] andxBytes = new byte[0];

            if (this.andxPacket != null)
            {
                if (andxPacket as SmbBatchedRequestPacket != null)
                {
                    andxBytes = ((SmbBatchedRequestPacket)this.andxPacket).GetBytesWithoutHeader(false, this.AndXOffset);
                }
                else
                {
                    andxBytes = this.andxPacket.GetBytesWithoutHeader();
                }
            }

            // get the total CIFS packet size:
            int size = this.ParametersSize + this.DataSize + andxBytes.Length;

            // init packet bytes:
            byte[] packetBytes = new byte[size];
            using (MemoryStream memoryStream = new MemoryStream(packetBytes))
            {
                using (Channel channel = new Channel(null, memoryStream))
                {
                    channel.BeginWriteGroup();
                    channel.Write<SmbParameters>(this.smbParametersBlock);
                    channel.Write<SmbData>(this.smbDataBlock);
                    channel.WriteBytes(andxBytes);
                    channel.EndWriteGroup();
                }
            }
            return packetBytes;
        }


        /// <summary>
        /// to encode the SmbParameters and SmbDada into bytes.
        /// </summary>
        /// <returns>the bytes array of SmbParameters, SmbDada, and AndX if existed.</returns>
        protected internal override byte[] GetBytesWithoutHeader()
        {
            return GetBytesWithoutHeader(true, 0);
        }

        #endregion


        #region decoding

        /// <summary>
        /// to decode the smb parameters: from the general SmbParameters to the concrete Smb Parameters.
        /// </summary>
        protected abstract override void DecodeParameters();


        /// <summary>
        /// to decode the smb data: from the general SmbDada to the concrete Smb Data.
        /// </summary>
        protected abstract override void DecodeData();


        /// <summary>
        /// to read the andx packet from the channel
        /// </summary>
        /// <param name="channel">the channel started with the SmbParameters of the andx.</param>
        /// <returns>the size in bytes of the SmbParameters, SmbData and andx if existed of the andx.</returns>
        internal int ReadAndxFromChannel(Channel channel)
        {
            int consumedLen = 0;
            if (this.AndxCommand != SmbCommand.SMB_COM_NO_ANDX_COMMAND)
            {
                SmbHeader andxHeader = this.SmbHeader;
                andxHeader.Protocol = CifsMessageUtils.SMB_PROTOCOL_ANDXPACKET;
                andxHeader.Command = this.AndxCommand;
                this.andxPacket = CifsMessageUtils.CreateSmbRequestPacket(andxHeader, channel);
                this.andxPacket.SmbHeader = andxHeader;
                consumedLen += this.andxPacket.ReadParametersFromChannel(channel);
                consumedLen += this.andxPacket.ReadDataFromChannel(channel);
                SmbBatchedRequestPacket batchedRequest = this.andxPacket as SmbBatchedRequestPacket;
                if (batchedRequest != null)
                {
                    consumedLen += batchedRequest.ReadAndxFromChannel(channel);
                }
            }
            return consumedLen;
        }

        #endregion
    }
}