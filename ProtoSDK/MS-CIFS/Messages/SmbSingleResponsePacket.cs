// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.IO;

using Microsoft.Protocols.TestTools.StackSdk.Messages;

namespace Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Cifs
{
    /// <summary>
    /// the basic packet of SMB single response.
    /// defined common method of all SMB single response packets
    /// </summary>
    public abstract class SmbSingleResponsePacket : SmbPacket
    {
        #region fields

        // none 

        #endregion


        #region properties

        /// <summary>
        /// Get the type of the packet: Request or Response, and Single or Compounded.
        /// </summary>
        /// <returns>the type of the packet.</returns>
        public override SmbPacketType PacketType
        {
            get
            {
                return SmbPacketType.SingleResponse;
            }
        }

        #endregion


        #region constructor and destructor

        /// <summary>
        /// Constructor.
        /// </summary>
        protected SmbSingleResponsePacket()
            : base()
        {
        }


        /// <summary>
        /// Constructor.
        /// </summary>
        protected SmbSingleResponsePacket(byte[] data)
            : base(data)
        {
        }


        /// <summary>
        /// Deep copy constructor.
        /// </summary>
        protected SmbSingleResponsePacket(SmbSingleResponsePacket packet)
            : base(packet)
        {
        }

        #endregion


        #region encoding

        /// <summary>
        /// to encode the SmbParameters and SmbDada into bytes.
        /// </summary>
        /// <returns>the bytes array of SmbParameters, SmbDada.</returns>
        protected internal override byte[] GetBytesWithoutHeader()
        {
            if (this.smbHeader.Status == 0)
            {
                // get the total CIFS packet size:
                int size = this.ParametersSize + this.DataSize;

                // init packet bytes:
                byte[] packetBytes = new byte[size];
                using (MemoryStream memoryStream = new MemoryStream(packetBytes))
                {
                    using (Channel channel = new Channel(null, memoryStream))
                    {
                        channel.BeginWriteGroup();
                        channel.Write<SmbParameters>(this.smbParametersBlock);
                        channel.Write<SmbData>(this.smbDataBlock);
                        channel.EndWriteGroup();
                    }
                }
                return packetBytes;
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

        #endregion
    }
}