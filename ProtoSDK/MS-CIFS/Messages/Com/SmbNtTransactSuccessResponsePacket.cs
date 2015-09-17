// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using Microsoft.Protocols.TestTools.StackSdk;
using Microsoft.Protocols.TestTools.StackSdk.Messages;

namespace Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Cifs
{
    /// <summary>
    /// Packets for SmbNtTransactSuccess Response
    /// </summary>
    public abstract class SmbNtTransactSuccessResponsePacket : SmbSingleResponsePacket
    {
        #region Fields

        /// <summary>
        /// the SMB_Parameters
        /// </summary>
        protected SMB_COM_NT_TRANSACT_SuccessResponse_SMB_Parameters smbParameters;

        /// <summary>
        /// the SMB_Data
        /// </summary>
        protected SMB_COM_NT_TRANSACT_SuccessResponse_SMB_Data smbData;

        #endregion


        #region Properties

        /// <summary>
        /// get or set the Smb_Parameters:SMB_COM_NT_TRANSACT_SuccessResponse_SMB_Parameters
        /// </summary>
        public SMB_COM_NT_TRANSACT_SuccessResponse_SMB_Parameters SmbParameters
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
        /// get or set the Smb_Data:SMB_COM_NT_TRANSACT_SuccessResponse_SMB_Data
        /// </summary>
        public SMB_COM_NT_TRANSACT_SuccessResponse_SMB_Data SmbData
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
        protected SmbNtTransactSuccessResponsePacket()
            : base()
        {
            this.InitDefaultValue();
        }


        /// <summary>
        /// Constructor: Create a request directly from a buffer.
        /// </summary>
        protected SmbNtTransactSuccessResponsePacket(byte[] data)
            : base(data)
        {
        }


        /// <summary>
        /// Deep copy constructor.
        /// </summary>
        protected SmbNtTransactSuccessResponsePacket(SmbNtTransactSuccessResponsePacket packet)
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
            this.smbParameters.SetupCount = packet.SmbParameters.SetupCount;
            this.smbParameters.Setup = new ushort[packet.smbParameters.SetupCount];

            if (packet.smbParameters.Setup != null)
            {
                Array.Copy(packet.smbParameters.Setup, this.smbParameters.Setup, packet.smbParameters.SetupCount);
            }
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

            this.smbData.Parameters = new byte[packet.smbParameters.ParameterCount];
            if (packet.smbData.Parameters != null)
            {
                Array.Copy(packet.smbData.Parameters, this.smbData.Parameters,
                    packet.smbParameters.ParameterCount);
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
        }

        #endregion


        #region Methods

        /// <summary>
        /// to update SmbParameters and SmbData automatically.
        /// </summary>
        public virtual void UpdateCountAndOffset()
        {
            this.EncodeNtTransParameters();
            this.EncodeNtTransData();

            this.smbParameters.ParameterCount = (ushort)this.smbData.Parameters.Length;

            //If TotalParameterCount is less than ParameterCount(not correct). So update it.
            //If TotalParameterCount is greater than ParameterCount, should be setup by user. Then keep it.
            if (this.smbParameters.TotalParameterCount < this.smbParameters.ParameterCount)
            {
                this.smbParameters.TotalParameterCount = this.smbParameters.ParameterCount;
            }

            this.smbParameters.ParameterOffset = (ushort)(this.HeaderSize + 
                TypeMarshal.GetBlockMemorySize(this.smbParameters) + sizeof(ushort));
            this.smbData.Pad1 = new byte[(this.smbParameters.ParameterOffset + 3) & ~3
                - this.smbParameters.ParameterOffset];
            this.smbParameters.ParameterOffset += (ushort)this.smbData.Pad1.Length;
            
            this.smbParameters.DataCount = (ushort)this.smbData.Data.Length;

            //If TotalDataCount is less than DataCount(which is not correct). So update it.
            //If TotalDataCount is greater than DataCount, should be setup by user. Then keep it.
            if (this.smbParameters.TotalDataCount < this.smbParameters.DataCount)
            {
                this.smbParameters.TotalDataCount = this.smbParameters.DataCount;
            }
            this.smbParameters.DataOffset = (ushort)(this.smbParameters.ParameterOffset
                + this.smbData.Parameters.Length);

            this.smbData.Pad2 = new byte[(this.smbParameters.DataOffset + 3) & ~3
                - this.smbParameters.DataOffset];
            this.smbParameters.DataOffset += (ushort)this.smbData.Pad2.Length;

            this.smbData.ByteCount = (ushort)(this.smbData.Pad1.Length + this.smbData.Parameters.Length
                + this.smbData.Pad2.Length + this.smbData.Data.Length);
        }

        #endregion


        #region override methods

        /// <summary>
        /// to create an instance of the StackPacket class that is identical to the current StackPacket. 
        /// </summary>
        /// <returns>a new Packet cloned from this.</returns>
        public override abstract StackPacket Clone();

        /// <summary>
        /// Encode the struct of SMB_COM_NT_TRANSACT_SuccessResponse_SMB_Parameters into the struct of SmbParameters
        /// </summary>
        protected override void EncodeParameters()
        {
            this.smbParametersBlock = TypeMarshal.ToStruct<SmbParameters>(
                CifsMessageUtils.ToBytes<SMB_COM_NT_TRANSACT_SuccessResponse_SMB_Parameters>(this.smbParameters));
        }


        /// <summary>
        /// Encode the struct of SMB_COM_NT_TRANSACT_SuccessResponse_SMB_Data into the struct of SmbData
        /// </summary>
        protected override void EncodeData()
        {
            this.EncodeNtTransParameters();
            this.EncodeNtTransData();
            int byteCount = 0;

            if (this.smbData.Pad1 != null)
            {
                byteCount += this.smbData.Pad1.Length;
            }
            if (this.smbData.Parameters != null)
            {
                byteCount += this.smbData.Parameters.Length;
            }
            if (this.smbData.Pad2 != null)
            {
                byteCount += this.smbData.Pad2.Length;
            }
            if (this.smbData.Data != null)
            {
                byteCount += this.smbData.Data.Length;
            }
            this.smbDataBlock.ByteCount = this.smbData.ByteCount;
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

                    if (this.SmbData.Parameters != null)
                    {
                        channel.WriteBytes(this.SmbData.Parameters);
                    }

                    if (this.SmbData.Pad2 != null)
                    {
                        channel.WriteBytes(this.SmbData.Pad2);
                    }

                    if (this.SmbData.Data != null)
                    {
                        channel.WriteBytes(this.SmbData.Data);
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
                this.smbParameters = TypeMarshal.ToStruct<SMB_COM_NT_TRANSACT_SuccessResponse_SMB_Parameters>(
                     TypeMarshal.ToBytes(this.smbParametersBlock));
            }
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
                    int pad1Length = this.smbParameters.ParameterOffset > 0 ?
                        (int)(this.smbParameters.ParameterOffset - this.HeaderSize
                        - SmbComTransactionPacket.SmbParametersWordCountLength - this.smbParameters.WordCount * 2
                        - SmbComTransactionPacket.SmbDataByteCountLength) : 0;

                    this.smbData.Pad1 = channel.ReadBytes(pad1Length);
                    this.smbData.Parameters = channel.ReadBytes((int)this.smbParameters.ParameterCount);
                    this.smbData.Pad2 = channel.ReadBytes((int)(this.smbParameters.DataOffset - this.smbParameters.ParameterOffset
                        - this.smbParameters.ParameterCount));
                    this.smbData.Data = channel.ReadBytes((int)this.smbParameters.DataCount);
                }
                this.DecodeNtTransParameters();
                this.DecodeNtTransData();
            }
        }


        /// <summary>
        /// Encode the struct of NtTransParameters into the byte array in SmbData.NT_Trans_Parameters
        /// </summary>
        protected abstract void EncodeNtTransParameters();

        /// <summary>
        /// Encode the struct of NtTransData into the byte array in SmbData.NT_Trans_Data
        /// </summary>
        protected abstract void EncodeNtTransData();

        /// <summary>
        /// to decode the NtTrans parameters: from the general NtTransParameters to the concrete NtTrans Parameters.
        /// </summary>
        protected abstract void DecodeNtTransParameters();

        /// <summary>
        /// to decode the NtTrans data: from the general NtTransDada to the concrete NtTrans Data.
        /// </summary>
        protected abstract void DecodeNtTransData();

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
            this.smbParameters.Reserved1 = new byte[3];
            this.smbParameters.SetupCount = 0;
            this.smbParameters.Setup = new ushort[0];
            this.smbParameters.WordCount = 0x12;
        }

        #endregion
    }
}
