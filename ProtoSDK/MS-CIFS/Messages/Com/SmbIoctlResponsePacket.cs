// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.IO;
using Microsoft.Protocols.TestTools.StackSdk;
using Microsoft.Protocols.TestTools.StackSdk.Messages;

namespace Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Cifs
{
    /// <summary>
    /// Packets for SmbIoctl Response
    /// </summary>
    public class SmbIoctlResponsePacket : SmbSingleResponsePacket
    {
        #region Fields

        private SMB_COM_IOCTL_Response_SMB_Parameters smbParameters;
        private SMB_COM_IOCTL_Response_SMB_Data smbData;

        #endregion


        #region Properties

        /// <summary>
        /// get or set the Smb_Parameters:SMB_COM_IOCTL_Response_SMB_Parameters
        /// </summary>
        public SMB_COM_IOCTL_Response_SMB_Parameters SmbParameters
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
        /// get or set the Smb_Data:SMB_COM_IOCTL_Response_SMB_Data
        /// </summary>
        public SMB_COM_IOCTL_Response_SMB_Data SmbData
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
        public SmbIoctlResponsePacket()
            : base()
        {
            this.InitDefaultValue();
        }


        /// <summary>
        /// Constructor: Create a request directly from a buffer.
        /// </summary>
        public SmbIoctlResponsePacket(byte[] data)
            : base(data)
        {
        }


        /// <summary>
        /// Deep copy constructor.
        /// </summary>
        public SmbIoctlResponsePacket(SmbIoctlResponsePacket packet)
            : base(packet)
        {
            this.InitDefaultValue();

            this.smbParameters.WordCount = packet.SmbParameters.WordCount;
            this.smbParameters.TotalParameterCount = packet.SmbParameters.TotalParameterCount;
            this.smbParameters.TotalDataCount = packet.SmbParameters.TotalDataCount;
            this.smbParameters.ParameterCount = packet.SmbParameters.ParameterCount;
            this.smbParameters.ParameterOffset = packet.SmbParameters.ParameterOffset;
            this.smbParameters.ParameterDisplacement = packet.SmbParameters.ParameterDisplacement;
            this.smbParameters.DataCount = packet.SmbParameters.DataCount;
            this.smbParameters.DataOffset = packet.SmbParameters.DataOffset;
            this.smbParameters.DataDisplacement = packet.SmbParameters.DataDisplacement;

            this.smbData.ByteCount = packet.SmbData.ByteCount;

            if (packet.smbData.Pad1 != null)
            {
                this.smbData.Pad1 = new byte[packet.smbData.Pad1.Length];
                Array.Copy(packet.smbData.Pad1, this.smbData.Pad1, packet.smbData.Pad1.Length);
            }
            else
            {
                this.smbData.Pad1 = new byte[0];
            }

            if (packet.smbData.Parameters != null)
            {
                this.smbData.Parameters = new byte[packet.smbData.Parameters.Length];
                Array.Copy(packet.smbData.Parameters, this.smbData.Parameters, packet.smbData.Parameters.Length);
            }
            else
            {
                this.smbData.Parameters = new byte[0];
            }

            if (packet.smbData.Pad2 != null)
            {
                this.smbData.Pad2 = new byte[packet.smbData.Pad2.Length];
                Array.Copy(packet.smbData.Pad2, this.smbData.Pad2, packet.smbData.Pad2.Length);
            }
            else
            {
                this.smbData.Pad2 = new byte[0];
            }

            if (packet.smbData.Data != null)
            {
                this.smbData.Data = new byte[packet.smbData.Data.Length];
                Array.Copy(packet.smbData.Data, this.smbData.Data, packet.smbData.Data.Length);
            }
            else
            {
                this.smbData.Data = new byte[0];
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
            return new SmbIoctlResponsePacket(this);
        }


        /// <summary>
        /// Encode the struct of SMB_COM_IOCTL_Response_SMB_Parameters into the struct of SmbParameters
        /// </summary>
        protected override void EncodeParameters()
        {
            this.smbParametersBlock = TypeMarshal.ToStruct<SmbParameters>(
                CifsMessageUtils.ToBytes<SMB_COM_IOCTL_Response_SMB_Parameters>(this.SmbParameters));
        }


        /// <summary>
        /// Encode the struct of SMB_COM_IOCTL_Response_SMB_Data into the struct of SmbData
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
            if (this.smbParametersBlock.WordCount > 0)
            {
                this.smbParameters = TypeMarshal.ToStruct<SMB_COM_IOCTL_Response_SMB_Parameters>(
                     TypeMarshal.ToBytes(this.smbParametersBlock));
            }
        }


        /// <summary>
        /// to decode the smb data: from the general SmbDada to the concrete Smb Data.
        /// </summary>
        protected override void DecodeData()
        {
            this.smbData.ByteCount = this.smbDataBlock.ByteCount;
            using (MemoryStream memoryStream = new MemoryStream(this.smbDataBlock.Bytes))
            {
                using (Channel channel = new Channel(null, memoryStream))
                {
                    byte wordCountLength = 1;
                    byte byteCountLength = 2;
                    int smbParameterslength = this.smbParameters.WordCount * 2 + wordCountLength;

                    if (this.smbParameters.ParameterOffset >= (this.HeaderSize + smbParameterslength + byteCountLength))
                    {
                        this.smbData.Pad1 = channel.ReadBytes(this.smbParameters.ParameterOffset -
                            (this.HeaderSize + smbParameterslength + byteCountLength));
                        this.smbData.Parameters = channel.ReadBytes(this.smbParameters.ParameterCount);
                    }
                    else
                    {
                        this.smbData.Pad1 = new byte[0];
                        this.smbData.Parameters = new byte[0];
                    }

                    if (this.smbParameters.DataOffset
                        >= (this.HeaderSize + smbParameterslength + byteCountLength + this.smbData.Pad1.Length +
                        this.smbData.Parameters.Length))
                    {
                        this.smbData.Pad2 = channel.ReadBytes(this.smbParameters.DataOffset
                            - (this.HeaderSize + smbParameterslength + byteCountLength + this.smbData.Pad1.Length +
                            this.smbData.Parameters.Length));
                        this.smbData.Data = channel.ReadBytes(this.smbParameters.DataCount);
                    }
                    else
                    {
                        this.smbData.Pad2 = new byte[0];
                        this.smbData.Data = new byte[0];
                    }
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
