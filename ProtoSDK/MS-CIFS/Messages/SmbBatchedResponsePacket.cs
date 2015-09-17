// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.IO;

using Microsoft.Protocols.TestTools.StackSdk.Messages;

namespace Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Cifs
{
    /// <summary>
    /// the basic packet of SMB batched response.
    /// defined common method of all SMB batched response packets
    /// </summary>
    public abstract class SmbBatchedResponsePacket : SmbPacket
    {
        #region fields

        /// <summary>
        /// the Smb andx Packet
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
                return SmbPacketType.BatchedResponse;
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
        protected SmbBatchedResponsePacket()
            : base()
        {
            this.AndxPacket = null;
        }


        /// <summary>
        /// Constructor.
        /// </summary>
        protected SmbBatchedResponsePacket(byte[] data)
            : base(data)
        {
        }


        /// <summary>
        /// Deep copy constructor.
        /// </summary>
        protected SmbBatchedResponsePacket(SmbBatchedResponsePacket packet)
            : base(packet)
        {
            lock (packet)
            {
                if (packet.AndxPacket != null)
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
                if (andxPacket as SmbBatchedResponsePacket != null)
                {
                    andxBytes = ((SmbBatchedResponsePacket)this.andxPacket).GetBytesWithoutHeader(false, this.AndXOffset);
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
                    this.WriteSmbParameter(channel);
                    channel.Write<SmbData>(this.smbDataBlock);
                    channel.WriteBytes(andxBytes);
                    channel.EndWriteGroup();
                }
            }
            return packetBytes;
        }


        /// <summary>
        /// write the smbparameter to channel.
        /// </summary>
        /// <param name="channel">
        /// a Channel to write data to.
        /// </param>
        protected virtual void WriteSmbParameter(Channel channel)
        {
            channel.Write<SmbParameters>(this.smbParametersBlock);
        }


        /// <summary>
        /// to encode the SmbParameters and SmbDada into bytes.
        /// </summary>
        /// <returns>the bytes array of SmbParameters, SmbDada, and AndX if existed.</returns>
        protected internal override byte[] GetBytesWithoutHeader()
        {
            if (smbHeader.Status == (uint)NtStatus.STATUS_SUCCESS
                || smbHeader.Status == (uint)NtStatus.STATUS_MORE_PROCESSING_REQUIRED)
            {
                return GetBytesWithoutHeader(true, 0);
            }
            else
            {
                return new byte[3];
            }
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
        /// <param name="smbRequest">the request of the response.</param>
        /// <param name="channel">the channel started with the SmbParameters of the andx.</param>
        /// <returns>the size in bytes of the SmbParameters, SmbData and andx if existed of the andx.</returns>
        internal int ReadAndxFromChannel(SmbPacket smbRequest, Channel channel)
        {
            int consumedLen = 0;
            if (this.AndxCommand != SmbCommand.SMB_COM_NO_ANDX_COMMAND)
            {
                SmbHeader andxHeader = this.SmbHeader;
                andxHeader.Command = this.AndxCommand;
                andxHeader.Protocol = CifsMessageUtils.SMB_PROTOCOL_ANDXPACKET;
                this.andxPacket = CifsMessageUtils.CreateSmbResponsePacket(smbRequest, andxHeader, channel);
                this.andxPacket.SmbHeader = andxHeader;
                consumedLen += this.andxPacket.ReadParametersFromChannel(channel);
                consumedLen += this.andxPacket.ReadDataFromChannel(channel);
                SmbBatchedResponsePacket batchedResponse = this.andxPacket as SmbBatchedResponsePacket;
                if (batchedResponse != null)
                {
                    consumedLen += batchedResponse.ReadAndxFromChannel(smbRequest, channel);
                }
            }
            return consumedLen;
        }

        #endregion

        #region help methods

        /// <summary>
        /// Update the andx packet header and all the its following batched packet's headers.
        /// </summary>
        public void UpdateHeader()
        {
            SmbHeader smbHeader = this.SmbHeader;
            SmbPacket nextPacket = this.AndxPacket;
   
            //update this header
            while (nextPacket != null)
            {
                smbHeader.Flags |= nextPacket.SmbHeader.Flags;
                smbHeader.Flags2 |= nextPacket.SmbHeader.Flags2;

                if (smbHeader.Status == 0)
                {
                    smbHeader.Status = nextPacket.SmbHeader.Status;
                }
                if (nextPacket.SmbHeader.Uid != 0)
                {
                    smbHeader.Uid = nextPacket.SmbHeader.Uid;
                }
                if (nextPacket.SmbHeader.Tid != 0)
                {
                    smbHeader.Tid = nextPacket.SmbHeader.Tid;
                }

                nextPacket = nextPacket as SmbBatchedResponsePacket;
                if (nextPacket != null)
                {
                    nextPacket = (nextPacket as SmbBatchedResponsePacket).AndxPacket;
                }
            }
            
            this.SmbHeader = smbHeader;

            //update andx packet header
            nextPacket = this.AndxPacket;
            while (nextPacket != null)
            {
                smbHeader = nextPacket.SmbHeader;
                smbHeader.Flags = this.SmbHeader.Flags;
                smbHeader.Flags2 = this.SmbHeader.Flags2;

                smbHeader.Uid = this.SmbHeader.Uid;
                smbHeader.Tid = this.SmbHeader.Tid;
                nextPacket.SmbHeader = smbHeader;

                nextPacket = nextPacket as SmbBatchedResponsePacket;
                if (nextPacket != null)
                {
                    nextPacket = (nextPacket as SmbBatchedResponsePacket).AndxPacket;
                }
            }
        }

        #endregion
    }
}