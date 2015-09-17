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
    /// Packets for SmbIoctl Request
    /// </summary>
    public class SmbIoctlRequestPacket : SmbSingleRequestPacket
    {
        #region Fields

        private SMB_COM_IOCTL_Request_SMB_Parameters smbParameters;
        private SMB_COM_IOCTL_Request_SMB_Data smbData;

        #endregion


        #region Properties

        /// <summary>
        /// get or set the Smb_Parameters:SMB_COM_IOCTL_Request_SMB_Parameters
        /// </summary>
        public SMB_COM_IOCTL_Request_SMB_Parameters SmbParameters
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
        /// get or set the Smb_Data:SMB_COM_IOCTL_Request_SMB_Data
        /// </summary>
        public SMB_COM_IOCTL_Request_SMB_Data SmbData
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
        public SmbIoctlRequestPacket()
            : base()
        {
            this.InitDefaultValue();
        }


        /// <summary>
        /// Constructor: Create a request directly from a buffer.
        /// </summary>
        public SmbIoctlRequestPacket(byte[] data)
            : base(data)
        {
        }


        /// <summary>
        /// Deep copy constructor.
        /// </summary>
        public SmbIoctlRequestPacket(SmbIoctlRequestPacket packet)
            : base(packet)
        {
            this.InitDefaultValue();

            this.smbParameters.WordCount = packet.SmbParameters.WordCount;
            this.smbParameters.FID = packet.SmbParameters.FID;
            this.smbParameters.Category = packet.SmbParameters.Category;
            this.smbParameters.Function = packet.SmbParameters.Function;
            this.smbParameters.TotalParameterCount = packet.SmbParameters.TotalParameterCount;
            this.smbParameters.TotalDataCount = packet.SmbParameters.TotalDataCount;
            this.smbParameters.MaxParameterCount = packet.SmbParameters.MaxParameterCount;
            this.smbParameters.MaxDataCount = packet.SmbParameters.MaxDataCount;
            this.smbParameters.Timeout = packet.SmbParameters.Timeout;
            this.smbParameters.Reserved = packet.SmbParameters.Reserved;
            this.smbParameters.ParameterCount = packet.SmbParameters.ParameterCount;
            this.smbParameters.ParameterOffset = packet.SmbParameters.ParameterOffset;
            this.smbParameters.DataCount = packet.SmbParameters.DataCount;
            this.smbParameters.DataOffset = packet.SmbParameters.DataOffset;
            this.smbData.ByteCount = packet.SmbData.ByteCount;

            if (packet.smbData.Pad1 != null)
            {
                this.smbData.Pad1 = new byte[packet.smbData.Pad1.Length];
                Array.Copy(packet.smbData.Pad1, this.smbData.Pad1, packet.smbData.Pad1.Length);
            }
            if (packet.smbData.Parameters != null)
            {
                this.smbData.Parameters = new byte[packet.smbData.Parameters.Length];
                Array.Copy(packet.smbData.Parameters, this.smbData.Parameters, packet.smbData.Parameters.Length);
            }
            if (packet.smbData.Pad2 != null)
            {
                this.smbData.Pad2 = new byte[packet.smbData.Pad2.Length];
                Array.Copy(packet.smbData.Pad2, this.smbData.Pad2, packet.smbData.Pad2.Length);
            }
            if (packet.smbData.Data != null)
            {
                this.smbData.Data = new byte[packet.smbData.Data.Length];
                Array.Copy(packet.smbData.Data, this.smbData.Data, packet.smbData.Data.Length);
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
            return new SmbIoctlRequestPacket(this);
        }


        /// <summary>
        /// Encode the struct of SMB_COM_IOCTL_Request_SMB_Parameters into the struct of SmbParameters
        /// </summary>
        protected override void EncodeParameters()
        {
            this.smbParametersBlock = TypeMarshal.ToStruct<SmbParameters>(
                CifsMessageUtils.ToBytes<SMB_COM_IOCTL_Request_SMB_Parameters>(this.smbParameters));
        }


        /// <summary>
        /// Encode the struct of SMB_COM_IOCTL_Request_SMB_Data into the struct of SmbData
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
                    if (this.smbData.Pad1 != null)
                    {
                        channel.WriteBytes(this.smbData.Pad1);
                    }
                    if (this.smbData.Parameters != null)
                    {
                        channel.WriteBytes(this.smbData.Parameters);
                    }
                    if (this.smbData.Pad2 != null)
                    {
                        channel.WriteBytes(this.smbData.Pad2);
                    }
                    if (this.smbData.Data != null)
                    {
                        channel.WriteBytes(this.smbData.Data);
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
            this.smbParameters = TypeMarshal.ToStruct<SMB_COM_IOCTL_Request_SMB_Parameters>(
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
                    int pad1Len = this.SmbParameters.ParameterOffset - Marshal.SizeOf(this.SmbHeader)
                        - Marshal.SizeOf(this.smbData.ByteCount) - Marshal.SizeOf(this.SmbParameters);

                    if (pad1Len > 0)
                    {
                        this.smbData.Pad1 = channel.ReadBytes(pad1Len);
                    }
                    else
                    {
                        this.smbData.Pad1 = new byte[0];
                    }

                    this.smbData.Parameters = channel.ReadBytes(this.SmbParameters.ParameterCount);
                    int pad2Len = this.SmbParameters.DataOffset - this.SmbParameters.ParameterOffset
                        - this.SmbParameters.ParameterCount;

                    if (pad2Len > 0)
                    {
                        this.smbData.Pad2 = channel.ReadBytes(pad2Len);
                    }
                    this.smbData.Data = channel.ReadBytes(this.SmbParameters.DataCount);
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
            this.smbData.Pad1 = new byte[0];
            this.smbData.Pad2 = new byte[0];
            this.smbData.Data = new byte[0];
            this.smbData.Parameters = new byte[0];
        }

        #endregion
    }
}
