// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.IO;
using Microsoft.Protocols.TestTools.StackSdk;
using Microsoft.Protocols.TestTools.StackSdk.Messages;

namespace Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Cifs
{
    /// <summary>
    /// Packets for SmbRename Request
    /// </summary>
    public class SmbRenameRequestPacket : SmbSingleRequestPacket
    {
        #region Fields

        private SMB_COM_RENAME_Request_SMB_Parameters smbParameters;
        private SMB_COM_RENAME_Request_SMB_Data smbData;

        #endregion


        #region Properties

        /// <summary>
        /// get or set the Smb_Parameters:SMB_COM_RENAME_Request_SMB_Parameters
        /// </summary>
        public SMB_COM_RENAME_Request_SMB_Parameters SmbParameters
        {
            get
            {
                return this.smbParameters;
            }
            set
            {
                this.smbParameters = value;
            }
        }


        /// <summary>
        /// get or set the Smb_Data:SMB_COM_RENAME_Request_SMB_Data
        /// </summary>
        public SMB_COM_RENAME_Request_SMB_Data SmbData
        {
            get
            {
                return this.smbData;
            }
            set
            {
                this.smbData = value;
            }
        }

        #endregion


        #region Constructor

        /// <summary>
        /// Constructor.
        /// </summary>
        public SmbRenameRequestPacket()
            : base()
        {
            this.InitDefaultValue();
        }


        /// <summary>
        /// Constructor: Create a request directly from a buffer.
        /// </summary>
        public SmbRenameRequestPacket(byte[] data)
            : base(data)
        {
        }


        /// <summary>
        /// Deep copy constructor.
        /// </summary>
        public SmbRenameRequestPacket(SmbRenameRequestPacket packet)
            : base(packet)
        {
            this.InitDefaultValue();

            this.smbParameters.WordCount = packet.SmbParameters.WordCount;
            this.smbParameters.SearchAttributes = packet.SmbParameters.SearchAttributes;

            this.smbData.ByteCount = packet.SmbData.ByteCount;
            this.smbData.BufferFormat1 = packet.SmbData.BufferFormat1;

            if (packet.smbData.OldFileName != null)
            {
                this.smbData.OldFileName = new byte[packet.smbData.OldFileName.Length];
                Array.Copy(packet.smbData.OldFileName, this.smbData.OldFileName, packet.smbData.OldFileName.Length);
            }
            else
            {
                this.smbData.OldFileName = new byte[0];
            }
            this.smbData.BufferFormat2 = packet.SmbData.BufferFormat2;

            if (packet.smbData.NewFileName != null)
            {
                this.smbData.NewFileName = new byte[packet.smbData.NewFileName.Length];
                Array.Copy(packet.smbData.NewFileName, this.smbData.NewFileName, packet.smbData.NewFileName.Length);
            }
            else
            {
                this.smbData.NewFileName = new byte[0];
            }
        }

        #endregion


        #region override methods

        /// <summary>
        /// to create an instance of the StackPacket class that is identical to the current StackPacket. 
        /// </summary>
        /// <returns>a new Packet cloned from this.</returns>
        public override StackPacket Clone()
        {
            return new SmbRenameRequestPacket(this);
        }


        /// <summary>
        /// Encode the struct of SMB_COM_RENAME_Request_SMB_Parameters into the struct of SmbParameters
        /// </summary>
        protected override void EncodeParameters()
        {
            this.smbParametersBlock = TypeMarshal.ToStruct<SmbParameters>(
                CifsMessageUtils.ToBytes<SMB_COM_RENAME_Request_SMB_Parameters>(this.smbParameters));
        }


        /// <summary>
        /// Encode the struct of SMB_COM_RENAME_Request_SMB_Data into the struct of SmbData
        /// </summary>
        protected override void EncodeData()
        {
            this.smbDataBlock.ByteCount = this.SmbData.ByteCount;
            this.smbDataBlock.Bytes = new byte[this.smbDataBlock.ByteCount];
            using (MemoryStream memoryStream = new MemoryStream(this.smbDataBlock.Bytes))
            {
                using (Channel channel = new Channel(null, memoryStream))
                {
                    channel.BeginWriteGroup();
                    channel.Write<byte>(this.SmbData.BufferFormat1);

                    if (this.SmbData.OldFileName != null)
                    {
                        channel.WriteBytes(this.SmbData.OldFileName);
                    }
                    channel.Write<byte>(this.SmbData.BufferFormat2);

                    if (this.SmbData.NewFileName != null)
                    {
                        channel.WriteBytes(this.SmbData.NewFileName);
                    }
                    channel.EndWriteGroup();
                }
            }
        }


        /// <summary>
        /// to decode the smb parameters: from the general SmbParameters to the concrete Smb Parameters.
        /// </summary>
        protected override void DecodeParameters()
        {
            this.smbParameters = TypeMarshal.ToStruct<SMB_COM_RENAME_Request_SMB_Parameters>(
                TypeMarshal.ToBytes(this.smbParametersBlock));
        }


        /// <summary>
        /// to decode the smb data: from the general SmbDada to the concrete Smb Data.
        /// </summary>
        protected override void DecodeData()
        {
            using (MemoryStream memoryStream = new MemoryStream(CifsMessageUtils.ToBytes<SmbData>(this.smbDataBlock)))
            {
                using (Channel channel = new Channel(null, memoryStream))
                {
                    this.smbData.ByteCount = channel.Read<ushort>();
                    this.smbData.BufferFormat1 = channel.Read<byte>();
                    this.smbData.OldFileName = CifsMessageUtils.ReadNullTerminatedString(channel, 
                        (this.smbHeader.Flags2 & SmbFlags2.SMB_FLAGS2_UNICODE) == SmbFlags2.SMB_FLAGS2_UNICODE);
                    this.smbData.BufferFormat2 = channel.Read<byte>();
                    this.smbData.NewFileName = CifsMessageUtils.ReadNullTerminatedString(channel, 
                        (this.smbHeader.Flags2 & SmbFlags2.SMB_FLAGS2_UNICODE) == SmbFlags2.SMB_FLAGS2_UNICODE);
                }
            }
        }

        #endregion


        #region initialize fields with default value

        /// <summary>
        /// init packet, set default field data
        /// </summary>
        private void InitDefaultValue()
        {
            this.smbData.NewFileName = new byte[0];
            this.smbData.OldFileName = new byte[0];
        }

        #endregion
    }
}
