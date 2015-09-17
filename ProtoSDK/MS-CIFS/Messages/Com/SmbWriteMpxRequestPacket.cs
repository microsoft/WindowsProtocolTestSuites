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
    /// Packets for SmbWriteMpx Request
    /// </summary>
    public class SmbWriteMpxRequestPacket : SmbSingleRequestPacket
    {
        #region Fields

        private SMB_COM_WRITE_MPX_Request_SMB_Parameters smbParameters;
        private SMB_COM_WRITE_MPX_Request_SMB_Data smbData;

        #endregion


        #region Properties

        /// <summary>
        /// get or set the Smb_Parameters:SMB_COM_WRITE_MPX_Request_SMB_Parameters
        /// </summary>
        public SMB_COM_WRITE_MPX_Request_SMB_Parameters SmbParameters
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
        /// get or set the Smb_Data:SMB_COM_WRITE_MPX_Request_SMB_Data
        /// </summary>
        public SMB_COM_WRITE_MPX_Request_SMB_Data SmbData
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
        public SmbWriteMpxRequestPacket()
            : base()
        {
            this.InitDefaultValue();
        }


        /// <summary>
        /// Constructor: Create a request directly from a buffer.
        /// </summary>
        public SmbWriteMpxRequestPacket(byte[] data)
            : base(data)
        {
        }


        /// <summary>
        /// Deep copy constructor.
        /// </summary>
        public SmbWriteMpxRequestPacket(SmbWriteMpxRequestPacket packet)
            : base(packet)
        {
            this.InitDefaultValue();

            this.smbParameters.WordCount = packet.SmbParameters.WordCount;
            this.smbParameters.FID = packet.SmbParameters.FID;
            this.smbParameters.TotalByteCount = packet.SmbParameters.TotalByteCount;
            this.smbParameters.Reserved = packet.SmbParameters.Reserved;
            this.smbParameters.ByteOffsetToBeginWrite = packet.SmbParameters.ByteOffsetToBeginWrite;
            this.smbParameters.Timeout = packet.SmbParameters.Timeout;
            this.smbParameters.WriteMode = packet.SmbParameters.WriteMode;
            this.smbParameters.RequestMask = packet.SmbParameters.RequestMask;
            this.smbParameters.DataLength = packet.SmbParameters.DataLength;
            this.smbParameters.DataOffset = packet.SmbParameters.DataOffset;

            this.smbData.ByteCount = packet.SmbData.ByteCount;
            if (packet.smbData.Pad != null)
            {
                this.smbData.Pad = new byte[packet.smbData.Pad.Length];
                Array.Copy(packet.smbData.Pad, this.smbData.Pad, packet.smbData.Pad.Length);
            }
            else
            {
                this.smbData.Pad = new byte[0];
            }

            if (packet.smbData.Buffer != null)
            {
                this.smbData.Buffer = new byte[packet.smbData.Buffer.Length];
                Array.Copy(packet.smbData.Buffer, this.smbData.Buffer, packet.smbData.Buffer.Length);
            }
            else
            {
                this.smbData.Buffer = new byte[0];
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
            return new SmbWriteMpxRequestPacket(this);
        }


        /// <summary>
        /// Encode the struct of SMB_COM_WRITE_MPX_Request_SMB_Parameters into the struct of SmbParameters
        /// </summary>
        protected override void EncodeParameters()
        {
            this.smbParametersBlock = CifsMessageUtils.ToStuct<SmbParameters>(
                CifsMessageUtils.ToBytes<SMB_COM_WRITE_MPX_Request_SMB_Parameters>(this.smbParameters));
        }


        /// <summary>
        /// Encode the struct of SMB_COM_WRITE_MPX_Request_SMB_Data into the struct of SmbData
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
                    if (this.smbData.Pad != null)
                    {
                        channel.WriteBytes(this.smbData.Pad);
                    }
                    if (this.smbData.Buffer != null)
                    {
                        channel.WriteBytes(this.smbData.Buffer);
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
            this.smbParameters = TypeMarshal.ToStruct<SMB_COM_WRITE_MPX_Request_SMB_Parameters>(
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
                    // pad:
                    if ((Marshal.SizeOf(this.SmbHeader) + Marshal.SizeOf(this.SmbParameters)
                        + Marshal.SizeOf(this.smbData.ByteCount)) % 2 != 0)
                    {
                        this.smbData.Pad = channel.ReadBytes(1);
                    }
                    else
                    {
                        this.smbData.Pad = new byte[0];
                    }
                    this.smbData.Buffer = channel.ReadBytes(this.smbData.ByteCount - this.smbData.Pad.Length);
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
        }

        #endregion
    }
}
