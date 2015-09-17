// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.IO;
using Microsoft.Protocols.TestTools.StackSdk;
using Microsoft.Protocols.TestTools.StackSdk.Messages;

namespace Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Cifs
{
    /// <summary>
    /// Packets for SmbTransactionSecondary Request
    /// </summary>
    public class SmbTransactionSecondaryRequestPacket : SmbSingleRequestPacket
    {
        #region Fields

        private SMB_COM_TRANSACTION_SECONDARY_Request_SMB_Parameters smbParameters;
        private SMB_COM_TRANSACTION_SECONDARY_Request_SMB_Data smbData;

        #endregion


        #region Properties

        /// <summary>
        /// get or set the Smb_Parameters:SMB_COM_TRANSACTION_SECONDARY_Request_SMB_Parameters
        /// </summary>
        public SMB_COM_TRANSACTION_SECONDARY_Request_SMB_Parameters SmbParameters
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
        /// get or set the Smb_Data:SMB_COM_TRANSACTION_SECONDARY_Request_SMB_Data
        /// </summary>
        public SMB_COM_TRANSACTION_SECONDARY_Request_SMB_Data SmbData
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
        public SmbTransactionSecondaryRequestPacket()
            : base()
        {
            this.InitDefaultValue();
        }


        /// <summary>
        /// Constructor: Create a request directly from a buffer.
        /// </summary>
        public SmbTransactionSecondaryRequestPacket(byte[] data)
            : base(data)
        {
        }


        /// <summary>
        /// Deep copy constructor.
        /// </summary>
        public SmbTransactionSecondaryRequestPacket(SmbTransactionSecondaryRequestPacket packet)
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

            if (packet.smbData.Trans_Parameters != null)
            {
                this.smbData.Trans_Parameters = new byte[packet.smbData.Trans_Parameters.Length];
                Array.Copy(packet.smbData.Trans_Parameters,
                    this.smbData.Trans_Parameters, packet.smbData.Trans_Parameters.Length);
            }
            else
            {
                this.smbData.Trans_Parameters = new byte[0];
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

            if (packet.smbData.Trans_Data != null)
            {
                this.smbData.Trans_Data = new byte[packet.smbData.Trans_Data.Length];
                Array.Copy(packet.smbData.Trans_Data, this.smbData.Trans_Data, packet.smbData.Trans_Data.Length);
            }
            else
            {
                this.smbData.Trans_Data = new byte[0];
            }
        }

        #endregion


        #region Methods

        /// <summary>
        /// to update fields about size and offset automatly.
        /// </summary>
        internal void UpdateCountAndOffset()
        {
            int byteCount = 0;

            if (this.smbData.Trans_Parameters != null)
            {
                this.smbParameters.ParameterCount = (ushort)smbData.Trans_Parameters.Length;
                byteCount += this.smbData.Trans_Parameters.Length;
            }
            this.smbParameters.ParameterOffset = (ushort)(this.HeaderSize + this.smbParameters.WordCount * 2
                + SmbComTransactionPacket.SmbParametersWordCountLength + SmbComTransactionPacket.SmbDataByteCountLength);

            if (this.smbData.Pad1 != null)
            {
                this.smbParameters.ParameterOffset += (ushort)this.smbData.Pad1.Length;
                byteCount += this.smbData.Pad1.Length;
            }
            if (this.smbData.Trans_Data != null)
            {
                this.smbParameters.DataCount = (ushort)this.smbData.Trans_Data.Length;
                byteCount += this.smbData.Trans_Data.Length;
            }
            this.smbParameters.DataOffset = (ushort)(this.smbParameters.ParameterOffset +
                this.smbParameters.ParameterCount);

            if (this.smbData.Pad2 != null)
            {
                this.smbParameters.DataOffset += (ushort)this.smbData.Pad2.Length;
                byteCount += this.smbData.Pad2.Length;
            }
            this.smbData.ByteCount = (ushort)byteCount;
        }

        #endregion


        #region override methods

        /// <summary>
        /// to create an instance of the StackPacket class that is identical to the current StackPacket. 
        /// </summary>
        /// <returns>a new Packet cloned from this.</returns>
        public override StackPacket Clone()
        {
            return new SmbTransactionSecondaryRequestPacket(this);
        }


        /// <summary>
        /// Encode the struct of SMB_COM_TRANSACTION_SECONDARY_Request_SMB_Parameters
        /// into the struct of SmbParameters
        /// </summary>
        protected override void EncodeParameters()
        {
            this.smbParametersBlock = TypeMarshal.ToStruct<SmbParameters>(
                CifsMessageUtils.ToBytes<SMB_COM_TRANSACTION_SECONDARY_Request_SMB_Parameters>(this.smbParameters));
        }


        /// <summary>
        /// Encode the struct of SMB_COM_TRANSACTION_SECONDARY_Request_SMB_Data into the struct of SmbData
        /// </summary>
        protected override void EncodeData()
        {
            int byteCount = 0;

            if (this.smbData.Pad1 != null)
            {
                byteCount += this.smbData.Pad1.Length;
            }
            if (this.smbData.Trans_Parameters != null)
            {
                byteCount += this.smbData.Trans_Parameters.Length;
            }
            if (this.smbData.Pad2 != null)
            {
                byteCount += this.smbData.Pad2.Length;
            }
            if (this.smbData.Trans_Data != null)
            {
                byteCount += this.smbData.Trans_Data.Length;
            }
            this.smbDataBlock.ByteCount = this.SmbData.ByteCount;
            this.smbDataBlock.Bytes = new byte[byteCount];
            using (MemoryStream memoryStream = new MemoryStream(this.smbDataBlock.Bytes))
            {
                using (Channel channel = new Channel(null, memoryStream))
                {
                    channel.BeginWriteGroup();
                    if (this.SmbData.Pad1 != null)
                    {
                        channel.WriteBytes(this.SmbData.Pad1);
                    }
                    if (this.SmbData.Trans_Parameters != null)
                    {
                        channel.WriteBytes(this.SmbData.Trans_Parameters);
                    }
                    if (this.SmbData.Pad2 != null)
                    {
                        channel.WriteBytes(this.SmbData.Pad2);
                    }
                    if (this.SmbData.Trans_Data != null)
                    {
                        channel.WriteBytes(this.SmbData.Trans_Data);
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
            this.smbParameters = TypeMarshal.ToStruct<SMB_COM_TRANSACTION_SECONDARY_Request_SMB_Parameters>(
                TypeMarshal.ToBytes(this.smbParametersBlock));
        }


        /// <summary>
        /// to decode the smb data: from the general SmbDada to the concrete Smb Data.
        /// </summary>
        protected override void DecodeData()
        {
            this.smbData = new SMB_COM_TRANSACTION_SECONDARY_Request_SMB_Data();
            using (MemoryStream memoryStream = new MemoryStream(CifsMessageUtils.ToBytes<SmbData>(this.smbDataBlock)))
            {
                using (Channel channel = new Channel(null, memoryStream))
                {
                    this.smbData.ByteCount = channel.Read<ushort>();
                    this.smbData.Pad1 = channel.ReadBytes((int)(this.smbParameters.ParameterOffset - this.HeaderSize
                        - this.smbParameters.WordCount * 2 - SmbComTransactionPacket.SmbParametersWordCountLength
                        - SmbComTransactionPacket.SmbDataByteCountLength));
                    this.smbData.Trans_Parameters = channel.ReadBytes((int)this.smbParameters.ParameterCount);
                    this.smbData.Pad2 = channel.ReadBytes((int)(this.smbParameters.DataOffset
                        - this.smbParameters.ParameterOffset - this.smbParameters.ParameterCount));
                    this.smbData.Trans_Data = channel.ReadBytes((int)this.smbParameters.DataCount);
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
            this.smbData.Trans_Data = new byte[0];
            this.smbData.Trans_Parameters = new byte[0];
        }

        #endregion
    }
}
