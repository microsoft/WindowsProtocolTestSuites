// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.IO;
using System.Runtime.InteropServices;
using Microsoft.Protocols.TestTools.StackSdk;
using Microsoft.Protocols.TestTools.StackSdk.Messages;

namespace Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Cifs
{
    /// <summary>
    /// Packets for SmbTreeConnectAndx Request
    /// </summary>
    public class SmbTreeConnectAndxRequestPacket : SmbBatchedRequestPacket
    {
        #region Fields

        private SMB_COM_TREE_CONNECT_ANDX_Request_SMB_Parameters smbParameters;
        private SMB_COM_TREE_CONNECT_ANDX_Request_SMB_Data smbData;

        #endregion


        #region Properties

        /// <summary>
        /// get or set the Smb_Parameters:SMB_COM_TREE_CONNECT_ANDX_Request_SMB_Parameters
        /// </summary>
        public SMB_COM_TREE_CONNECT_ANDX_Request_SMB_Parameters SmbParameters
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
        /// get or set the Smb_Data:SMB_COM_TREE_CONNECT_ANDX_Request_SMB_Data
        /// </summary>
        public SMB_COM_TREE_CONNECT_ANDX_Request_SMB_Data SmbData
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


        /// <summary>
        /// the SmbCommand of the andx packet.
        /// </summary>
        protected override SmbCommand AndxCommand
        {
            get
            {
                return this.SmbParameters.AndXCommand;
            }
        }


        /// <summary>
        /// Set the AndXOffset from batched request
        /// </summary>
        protected override ushort AndXOffset
        {
            get
            {
                return this.smbParameters.AndXOffset;
            }
            set
            {
                this.smbParameters.AndXOffset = value;
            }
        }

        #endregion


        #region Constructor

        /// <summary>
        /// Constructor.
        /// </summary>
        public SmbTreeConnectAndxRequestPacket()
            : base()
        {
            this.InitDefaultValue();
        }


        /// <summary>
        /// Constructor: Create a request directly from a buffer.
        /// </summary>
        public SmbTreeConnectAndxRequestPacket(byte[] data)
            : base(data)
        {
        }


        /// <summary>
        /// Deep copy constructor.
        /// </summary>
        public SmbTreeConnectAndxRequestPacket(SmbTreeConnectAndxRequestPacket packet)
            : base(packet)
        {
            this.InitDefaultValue();

            this.smbParameters.WordCount = packet.SmbParameters.WordCount;
            this.smbParameters.AndXCommand = packet.SmbParameters.AndXCommand;
            this.smbParameters.AndXReserved = packet.SmbParameters.AndXReserved;
            this.smbParameters.AndXOffset = packet.SmbParameters.AndXOffset;
            this.smbParameters.Flags = packet.SmbParameters.Flags;
            this.smbParameters.PasswordLength = packet.SmbParameters.PasswordLength;

            this.smbData.ByteCount = packet.SmbData.ByteCount;
            if (packet.smbData.Password != null)
            {
                this.smbData.Password = new byte[packet.smbData.Password.Length];
                Array.Copy(packet.smbData.Password, this.smbData.Password, packet.smbParameters.PasswordLength);
            }
            else
            {
                this.smbData.Password = new byte[0];
            }

            if (packet.smbData.Pad != null)
            {
                this.smbData.Pad = new byte[packet.smbData.Pad.Length];
                Array.Copy(packet.smbData.Pad, this.smbData.Pad, packet.smbData.Pad.Length);
            }
            else
            {
                this.smbData.Pad = new byte[0];
            }

            if (packet.smbData.Path != null)
            {
                this.smbData.Path = new byte[packet.smbData.Path.Length];
                Array.Copy(packet.smbData.Path, this.smbData.Path, packet.smbData.Path.Length);
            }
            else
            {
                this.smbData.Path = new byte[0];
            }

            if (packet.smbData.Service != null)
            {
                this.smbData.Service = new byte[packet.smbData.Service.Length];
                Array.Copy(packet.smbData.Service, this.smbData.Service, packet.smbData.Service.Length);
            }
            else
            {
                this.smbData.Service = new byte[0];
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
            return new SmbTreeConnectAndxRequestPacket(this);
        }


        /// <summary>
        /// Encode the struct of SMB_COM_TREE_CONNECT_ANDX_Request_SMB_Parameters into the struct of SmbParameters
        /// </summary>
        protected override void EncodeParameters()
        {
            this.smbParametersBlock = CifsMessageUtils.ToStuct<SmbParameters>(
                CifsMessageUtils.ToBytes<SMB_COM_TREE_CONNECT_ANDX_Request_SMB_Parameters>(this.smbParameters));
        }


        /// <summary>
        /// Encode the struct of SMB_COM_TREE_CONNECT_ANDX_Request_SMB_Data into the struct of SmbData
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
                    if (this.SmbData.Password != null)
                    {
                        channel.WriteBytes(this.SmbData.Password);
                    }
                    if (this.SmbData.Pad != null)
                    {
                        channel.WriteBytes(this.SmbData.Pad);
                    }
                    if (this.SmbData.Path != null)
                    {
                        channel.WriteBytes(this.SmbData.Path);
                    }
                    if (this.SmbData.Service != null)
                    {
                        channel.WriteBytes(this.SmbData.Service);
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
            this.smbParameters = TypeMarshal.ToStruct<SMB_COM_TREE_CONNECT_ANDX_Request_SMB_Parameters>(
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
                    this.smbData.Password = channel.ReadBytes(this.SmbParameters.PasswordLength);

                    // pad:
                    if ((Marshal.SizeOf(this.SmbHeader) + Marshal.SizeOf(this.SmbParameters)
                        + Marshal.SizeOf(this.smbData.ByteCount) + this.SmbParameters.PasswordLength) % 2 != 0)
                    {
                        this.smbData.Pad = channel.ReadBytes(1);
                    }
                    else
                    {
                        this.smbData.Pad = new byte[0];
                    }

                    // Path:
                    this.smbData.Path = CifsMessageUtils.ReadNullTerminatedString(channel,
                        (this.smbHeader.Flags2 & SmbFlags2.SMB_FLAGS2_UNICODE) == SmbFlags2.SMB_FLAGS2_UNICODE);

                    // Service:
                    this.smbData.Service = channel.ReadBytes(this.smbData.ByteCount - this.smbData.Password.Length
                        - this.smbData.Pad.Length - this.smbData.Path.Length);
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
            this.smbData.Pad = new byte[0];
            this.smbData.Password = new byte[0];
            this.smbData.Path = new byte[0];
            this.smbData.Service = new byte[0];
        }

        #endregion
    }
}
