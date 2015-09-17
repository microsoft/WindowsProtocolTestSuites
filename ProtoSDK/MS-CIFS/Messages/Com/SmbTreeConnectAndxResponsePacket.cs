// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Protocols.TestTools.StackSdk;
using Microsoft.Protocols.TestTools.StackSdk.Messages;

namespace Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Cifs
{
    /// <summary>
    /// Packets for SmbTreeConnectAndx Response
    /// </summary>
    public class SmbTreeConnectAndxResponsePacket : SmbBatchedResponsePacket
    {
        #region Fields

        private SMB_COM_TREE_CONNECT_ANDX_Response_SMB_Parameters smbParameters;
        private SMB_COM_TREE_CONNECT_ANDX_Response_SMB_Data smbData;

        #endregion


        #region Properties

        /// <summary>
        /// get or set the Smb_Parameters:SMB_COM_TREE_CONNECT_ANDX_Response_SMB_Parameters
        /// </summary>
        public SMB_COM_TREE_CONNECT_ANDX_Response_SMB_Parameters SmbParameters
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
        /// get or set the Smb_Data:SMB_COM_TREE_CONNECT_ANDX_Response_SMB_Data
        /// </summary>
        public SMB_COM_TREE_CONNECT_ANDX_Response_SMB_Data SmbData
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
        public SmbTreeConnectAndxResponsePacket()
            : base()
        {
            this.InitDefaultValue();
        }


        /// <summary>
        /// Constructor: Create a request directly from a buffer.
        /// </summary>
        public SmbTreeConnectAndxResponsePacket(byte[] data)
            : base(data)
        {
        }


        /// <summary>
        /// Deep copy constructor.
        /// </summary>
        public SmbTreeConnectAndxResponsePacket(SmbTreeConnectAndxResponsePacket packet)
            : base(packet)
        {
            this.InitDefaultValue();

            this.smbParameters.WordCount = packet.SmbParameters.WordCount;
            this.smbParameters.AndXCommand = packet.SmbParameters.AndXCommand;
            this.smbParameters.AndXReserved = packet.SmbParameters.AndXReserved;
            this.smbParameters.AndXOffset = packet.SmbParameters.AndXOffset;
            this.smbParameters.OptionalSupport = packet.SmbParameters.OptionalSupport;

            this.smbData.ByteCount = packet.SmbData.ByteCount;
            if (packet.smbData.Service != null)
            {
                this.smbData.Service = new byte[packet.smbData.Service.Length];
                Array.Copy(packet.smbData.Service, this.smbData.Service, packet.smbData.Service.Length);
            }
            else
            {
                this.smbData.Service = new byte[0];
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

            if (packet.smbData.NativeFileSystem != null)
            {
                this.smbData.NativeFileSystem = new byte[packet.smbData.NativeFileSystem.Length];
                Array.Copy(packet.smbData.NativeFileSystem,
                    this.smbData.NativeFileSystem, packet.smbData.NativeFileSystem.Length);
            }
            else
            {
                this.smbData.NativeFileSystem = new byte[0];
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
            return new SmbTreeConnectAndxResponsePacket(this);
        }


        /// <summary>
        /// Encode the struct of SMB_COM_TREE_CONNECT_ANDX_Response_SMB_Parameters into the struct of SmbParameters
        /// </summary>
        protected override void EncodeParameters()
        {
            this.smbParametersBlock = TypeMarshal.ToStruct<SmbParameters>(
                CifsMessageUtils.ToBytes<SMB_COM_TREE_CONNECT_ANDX_Response_SMB_Parameters>(this.smbParameters));
        }


        /// <summary>
        /// Encode the struct of SMB_COM_TREE_CONNECT_ANDX_Response_SMB_Data into the struct of SmbData
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
                    if (this.smbData.Service != null)
                    {
                        channel.WriteBytes(this.smbData.Service);
                    }
                    if (this.smbData.Pad != null)
                    {
                        channel.WriteBytes(this.smbData.Pad);
                    }
                    if (this.smbData.NativeFileSystem != null)
                    {
                        channel.WriteBytes(this.smbData.NativeFileSystem);
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
            if (this.smbParametersBlock.WordCount > 0)
            {
                this.smbParameters = TypeMarshal.ToStruct<SMB_COM_TREE_CONNECT_ANDX_Response_SMB_Parameters>(
                     TypeMarshal.ToBytes(this.smbParametersBlock));
            }
        }


        /// <summary>
        /// to decode the smb data: from the general SmbDada to the concrete Smb Data.
        /// </summary>
        protected override void DecodeData()
        {
            this.smbData.ByteCount = this.smbDataBlock.ByteCount;

            if (this.smbData.ByteCount > 0)
            {
                List<byte> data = new List<byte>(this.smbDataBlock.Bytes);
                int length = data.IndexOf(0) + 1;
                this.smbData.Service = data.GetRange(0, length).ToArray();

                if ((this.SmbHeader.Flags2 & SmbFlags2.SMB_FLAGS2_UNICODE) == SmbFlags2.SMB_FLAGS2_UNICODE)
                {
                    // The start of Service field is not aligned on a 16-bit boundary.
                    // To ensure that the NativeFileSystem string is aligned on a 16-bit boundary.
                    if (this.smbData.Service.Length % 2 == 0)
                    {
                        this.smbData.Pad = new byte[1];
                        length++;
                    }
                    else
                    {
                        this.smbData.Pad = new byte[0];
                    }
                }
                this.smbData.NativeFileSystem = new byte[this.SmbDataBlock.Bytes.Length - length];
                Array.Copy(this.SmbDataBlock.Bytes, length,
                    this.smbData.NativeFileSystem, 0, this.SmbDataBlock.Bytes.Length - length);
            }
        }

        #endregion


        #region initialize fields with default value

        /// <summary>
        /// init packet, set default field data
        /// </summary>
        private void InitDefaultValue()
        {
            this.smbParameters.AndXCommand = SmbCommand.SMB_COM_NO_ANDX_COMMAND;
            this.smbData.NativeFileSystem = new byte[0];
            this.smbData.Service = new byte[0];
        }

        #endregion
    }
}
