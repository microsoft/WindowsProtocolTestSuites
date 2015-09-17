// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.IO;
using Microsoft.Protocols.TestTools.StackSdk;
using Microsoft.Protocols.TestTools.StackSdk.Messages;

namespace Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Cifs
{
    /// <summary>
    /// Packets for SmbNtTransactSecondary Request
    /// </summary>
    public class SmbNtTransactSecondaryRequestPacket : SmbSingleRequestPacket
    {
        #region Fields

        private SMB_COM_NT_TRANSACT_SECONDARY_Request_SMB_Parameters smbParameters;
        private SMB_COM_NT_TRANSACT_SECONDARY_Request_SMB_Data smbData;

        #endregion


        #region Properties

        /// <summary>
        /// get or set the Smb_Parameters:SMB_COM_NT_TRANSACT_SECONDARY_Request_SMB_Parameters
        /// </summary>
        public SMB_COM_NT_TRANSACT_SECONDARY_Request_SMB_Parameters SmbParameters
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
        /// get or set the Smb_Data:SMB_COM_NT_TRANSACT_SECONDARY_Request_SMB_Data
        /// </summary>
        public SMB_COM_NT_TRANSACT_SECONDARY_Request_SMB_Data SmbData
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
        public SmbNtTransactSecondaryRequestPacket()
            : base()
        {
            this.InitDefaultValue();
        }


        /// <summary>
        /// Constructor: Create a request directly from a buffer.
        /// </summary>
        public SmbNtTransactSecondaryRequestPacket(byte[] data)
            : base(data)
        {
        }


        /// <summary>
        /// Deep copy constructor.
        /// </summary>
        public SmbNtTransactSecondaryRequestPacket(SmbNtTransactSecondaryRequestPacket packet)
            : base(packet)
        {
            this.InitDefaultValue();
            this.smbParameters.WordCount = packet.SmbParameters.WordCount;

            if (packet.smbParameters.Reserved1 != null)
            {
                this.smbParameters.Reserved1 = new byte[packet.smbParameters.Reserved1.Length];
                Array.Copy(packet.smbParameters.Reserved1, this.smbParameters.Reserved1,
                    packet.smbParameters.Reserved1.Length);
            }
            else
            {
                this.smbParameters.Reserved1 = new byte[0];
            }
            this.smbParameters.TotalParameterCount = packet.SmbParameters.TotalParameterCount;
            this.smbParameters.TotalDataCount = packet.SmbParameters.TotalDataCount;
            this.smbParameters.ParameterCount = packet.SmbParameters.ParameterCount;
            this.smbParameters.ParameterOffset = packet.SmbParameters.ParameterOffset;
            this.smbParameters.ParameterDisplacement = packet.SmbParameters.ParameterDisplacement;
            this.smbParameters.DataCount = packet.SmbParameters.DataCount;
            this.smbParameters.DataOffset = packet.SmbParameters.DataOffset;
            this.smbParameters.DataDisplacement = packet.SmbParameters.DataDisplacement;
            this.smbParameters.Reserved2 = packet.SmbParameters.Reserved2;
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
            this.smbData.NT_Trans_Parameters = new byte[packet.smbParameters.ParameterCount];

            if (packet.smbData.NT_Trans_Parameters != null)
            {
                Array.Copy(packet.smbData.NT_Trans_Parameters,
                    this.smbData.NT_Trans_Parameters, packet.smbParameters.ParameterCount);
            }
            else
            {
                this.smbData.NT_Trans_Parameters = new byte[0];
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
            this.smbData.NT_Trans_Data = new byte[packet.smbParameters.DataCount];

            if (this.smbData.NT_Trans_Data != null)
            {
                Array.Copy(packet.smbData.NT_Trans_Data, this.smbData.NT_Trans_Data, packet.smbParameters.DataCount);
            }
            else
            {
                this.smbData.NT_Trans_Data = new byte[0];
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

            if (this.smbData.NT_Trans_Parameters != null)
            {
                this.smbParameters.ParameterCount = (ushort)smbData.NT_Trans_Parameters.Length;
                byteCount += this.smbData.NT_Trans_Parameters.Length;
            }
            this.smbParameters.ParameterOffset = (ushort)(this.HeaderSize + this.smbParameters.WordCount * 2
                + SmbComTransactionPacket.SmbParametersWordCountLength + SmbComTransactionPacket.SmbDataByteCountLength);

            if (this.smbData.Pad1 != null)
            {
                this.smbParameters.ParameterOffset += (ushort)this.smbData.Pad1.Length;
                byteCount += this.smbData.Pad1.Length;
            }
            if (this.smbData.NT_Trans_Data != null)
            {
                this.smbParameters.DataCount = (ushort)this.smbData.NT_Trans_Data.Length;
                byteCount += this.smbData.NT_Trans_Data.Length;
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
            return new SmbNtTransactSecondaryRequestPacket(this);
        }


        /// <summary>
        /// Encode the struct of SMB_COM_NT_TRANSACT_SECONDARY_Request_SMB_Parameters
        /// into the struct of SmbParameters
        /// </summary>
        protected override void EncodeParameters()
        {
            this.smbParametersBlock = TypeMarshal.ToStruct<SmbParameters>(
                CifsMessageUtils.ToBytes<SMB_COM_NT_TRANSACT_SECONDARY_Request_SMB_Parameters>(this.smbParameters));
        }


        /// <summary>
        /// Encode the struct of SMB_COM_NT_TRANSACT_SECONDARY_Request_SMB_Data into the struct of SmbData
        /// </summary>
        protected override void EncodeData()
        {
            int byteCount = 0;

            if (this.smbData.Pad1 != null)
            {
                byteCount += this.smbData.Pad1.Length;
            }
            if (this.smbData.NT_Trans_Parameters != null)
            {
                byteCount += this.smbData.NT_Trans_Parameters.Length;
            }
            if (this.smbData.Pad2 != null)
            {
                byteCount += this.smbData.Pad2.Length;
            }
            if (this.smbData.NT_Trans_Data != null)
            {
                byteCount += this.smbData.NT_Trans_Data.Length;
            }
            this.smbDataBlock.ByteCount = this.smbData.ByteCount;
            this.smbDataBlock.Bytes = new byte[byteCount];

            if (this.smbData.ByteCount > 0)
            {
                using (MemoryStream memoryStream = new MemoryStream(this.smbDataBlock.Bytes))
                {
                    using (Channel channel = new Channel(null, memoryStream))
                    {
                        channel.BeginWriteGroup();
                        if (this.SmbData.Pad1 != null)
                        {
                            channel.WriteBytes(this.SmbData.Pad1);
                        }
                        if (this.SmbData.NT_Trans_Parameters != null)
                        {
                            channel.WriteBytes(this.SmbData.NT_Trans_Parameters);
                        }
                        if (this.SmbData.Pad2 != null)
                        {
                            channel.WriteBytes(this.SmbData.Pad2);
                        }
                        if (this.SmbData.NT_Trans_Data != null)
                        {
                            channel.WriteBytes(this.SmbData.NT_Trans_Data);
                        }
                        channel.EndWriteGroup();
                    }
                }
            }
        }


        /// <summary>
        /// to decode the smb parameters: from the general SmbParameters to the concrete Smb Parameters.
        /// </summary>
        protected override void DecodeParameters()
        {
            this.smbParameters = TypeMarshal.ToStruct<SMB_COM_NT_TRANSACT_SECONDARY_Request_SMB_Parameters>(
                TypeMarshal.ToBytes(this.smbParametersBlock));
        }


        /// <summary>
        /// to decode the smb data: from the general SmbDada to the concrete Smb Data.
        /// </summary>
        protected override void DecodeData()
        {
            this.smbData = new SMB_COM_NT_TRANSACT_SECONDARY_Request_SMB_Data();
            using (MemoryStream memoryStream = new MemoryStream(CifsMessageUtils.ToBytes<SmbData>(this.smbDataBlock)))
            {
                using (Channel channel = new Channel(null, memoryStream))
                {
                    this.smbData.ByteCount = channel.Read<ushort>();
                    this.smbData.Pad1 = channel.ReadBytes((int)(this.smbParameters.ParameterOffset - this.HeaderSize
                        - this.smbParameters.WordCount * 2 - SmbComTransactionPacket.SmbParametersWordCountLength
                        - SmbComTransactionPacket.SmbDataByteCountLength));
                    this.smbData.NT_Trans_Parameters = channel.ReadBytes((int)this.smbParameters.ParameterCount);
                    this.smbData.Pad2 = channel.ReadBytes((int)(this.smbParameters.DataOffset
                        - this.smbParameters.ParameterOffset - this.smbParameters.ParameterCount));
                    this.smbData.NT_Trans_Data = channel.ReadBytes((int)this.smbParameters.DataCount);
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
            this.smbData.NT_Trans_Data = new byte[0];
            this.smbData.NT_Trans_Parameters = new byte[0];
            this.smbParameters.Reserved1 = new byte[0];
        }

        #endregion
    }
}