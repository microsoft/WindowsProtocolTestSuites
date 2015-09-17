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
    /// Packets for SmbTransaction2 Request
    /// </summary>
    public abstract class SmbTransaction2RequestPacket : SmbSingleRequestPacket
    {
        #region Fields

        /// <summary>
        /// the invalid FID used for trans2 secondary request
        /// </summary>
        protected const ushort INVALID_FID = 0xFFFF;

        /// <summary>
        /// the SMB_Parameters
        /// </summary>
        protected SMB_COM_TRANSACTION2_Request_SMB_Parameters smbParameters;

        /// <summary>
        /// the SMB_Data
        /// </summary>
        protected SMB_COM_TRANSACTION2_Request_SMB_Data smbData;

        /// <summary>
        /// Specify whether the packet is sent out with following secondary packets.
        /// </summary>
        internal bool isDivided;

        #endregion


        #region Properties

        /// <summary>
        /// get or set the Smb_Parameters:SMB_COM_TRANSACTION2_Request_SMB_Parameters
        /// </summary>
        public virtual SMB_COM_TRANSACTION2_Request_SMB_Parameters SmbParameters
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
        /// get or set the Smb_Data:SMB_COM_TRANSACTION2_Request_SMB_Data
        /// </summary>
        public SMB_COM_TRANSACTION2_Request_SMB_Data SmbData
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
        /// FID used by Trans2 Secondary Request
        /// </summary>
        internal abstract ushort FID
        {
            get;
        }

        #endregion


        #region Constructor

        /// <summary>
        /// Constructor.
        /// </summary>
        protected SmbTransaction2RequestPacket()
            : base()
        {
            this.InitDefaultValue();
        }


        /// <summary>
        /// Constructor: Create a request directly from a buffer.
        /// </summary>
        protected SmbTransaction2RequestPacket(byte[] data)
            : base(data)
        {
        }


        /// <summary>
        /// Deep copy constructor.
        /// </summary>
        protected SmbTransaction2RequestPacket(SmbTransaction2RequestPacket packet)
            : base(packet)
        {
            this.InitDefaultValue();

            this.smbParameters.WordCount = packet.SmbParameters.WordCount;
            this.smbParameters.TotalParameterCount = packet.SmbParameters.TotalParameterCount;
            this.smbParameters.TotalDataCount = packet.SmbParameters.TotalDataCount;
            this.smbParameters.MaxParameterCount = packet.SmbParameters.MaxParameterCount;
            this.smbParameters.MaxDataCount = packet.SmbParameters.MaxDataCount;
            this.smbParameters.MaxSetupCount = packet.SmbParameters.MaxSetupCount;
            this.smbParameters.Reserved1 = packet.SmbParameters.Reserved1;
            this.smbParameters.Flags = packet.SmbParameters.Flags;
            this.smbParameters.Timeout = packet.SmbParameters.Timeout;
            this.smbParameters.Reserved2 = packet.SmbParameters.Reserved2;
            this.smbParameters.ParameterCount = packet.SmbParameters.ParameterCount;
            this.smbParameters.ParameterOffset = packet.SmbParameters.ParameterOffset;
            this.smbParameters.DataCount = packet.SmbParameters.DataCount;
            this.smbParameters.DataOffset = packet.SmbParameters.DataOffset;
            this.smbParameters.SetupCount = packet.SmbParameters.SetupCount;
            this.smbParameters.Reserved3 = packet.SmbParameters.Reserved3;

            if (packet.smbParameters.Setup != null)
            {
                this.smbParameters.Setup = new ushort[packet.smbParameters.Setup.Length];
                Array.Copy(packet.smbParameters.Setup, this.smbParameters.Setup, packet.smbParameters.Setup.Length);
            }
            else
            {
                this.smbParameters.Setup = new ushort[0];
            }
            this.smbData.ByteCount = packet.SmbData.ByteCount;
            this.smbData.Name = packet.SmbData.Name;

            if (packet.smbData.Pad1 != null)
            {
                this.smbData.Pad1 = new byte[packet.smbData.Pad1.Length];
                Array.Copy(packet.smbData.Pad1, this.smbData.Pad1, packet.smbData.Pad1.Length);
            }
            else
            {
                this.smbData.Pad1 = new byte[0];
            }

            if (packet.smbData.Trans2_Parameters != null)
            {
                this.smbData.Trans2_Parameters = new byte[packet.smbData.Trans2_Parameters.Length];
                Array.Copy(packet.smbData.Trans2_Parameters,
                    this.smbData.Trans2_Parameters, packet.smbData.Trans2_Parameters.Length);
            }
            else
            {
                this.smbData.Trans2_Parameters = new byte[0];
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

            if (packet.smbData.Trans2_Data != null)
            {
                this.smbData.Trans2_Data = new byte[packet.smbData.Trans2_Data.Length];
                Array.Copy(packet.smbData.Trans2_Data, this.smbData.Trans2_Data, packet.smbData.Trans2_Data.Length);
            }
            else
            {
                this.smbData.Trans2_Data = new byte[0];
            }
        }

        #endregion


        #region Methods

        /// <summary>
        /// to update fields about size and offset automatly.
        /// if the smb data is update, this method must be invoke to update the count and offset.
        /// </summary>
        public void UpdateCountAndOffset()
        {
            if (!this.isDivided)
            {
                this.EncodeTrans2Parameters();
                this.EncodeTrans2Data();
            }
            int byteCount = 0;

            if (this.SmbData.Name != null)
            {
                byteCount = this.smbData.Name.Length;
            }
            if (this.smbData.Trans2_Parameters != null)
            {
                this.smbParameters.ParameterCount = (ushort)smbData.Trans2_Parameters.Length;
                byteCount += this.smbData.Trans2_Parameters.Length;

                if (!this.isDivided)
                {
                    this.smbParameters.TotalParameterCount = (ushort)smbData.Trans2_Parameters.Length;
                }
            }
            this.smbParameters.ParameterOffset = (ushort)(this.HeaderSize + this.smbParameters.WordCount * 2
                + SmbComTransactionPacket.SmbParametersWordCountLength + SmbComTransactionPacket.SmbDataByteCountLength
                + this.smbData.Name.Length);

            if (this.smbData.Pad1 != null)
            {
                this.smbParameters.ParameterOffset += (ushort)this.smbData.Pad1.Length;
                byteCount += this.smbData.Pad1.Length;
            }
            if (this.smbData.Trans2_Data != null)
            {
                this.smbParameters.DataCount = (ushort)this.smbData.Trans2_Data.Length;
                byteCount += this.smbData.Trans2_Data.Length;

                if (!this.isDivided)
                {
                    this.smbParameters.TotalDataCount = (ushort)this.smbData.Trans2_Data.Length;
                }
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
        public override abstract StackPacket Clone();

        /// <summary>
        /// Encode the struct of SMB_COM_TRANSACTION2_Request_SMB_Parameters into the struct of SmbParameters
        /// </summary>
        protected override void EncodeParameters()
        {
            this.smbParametersBlock = TypeMarshal.ToStruct<SmbParameters>(
                CifsMessageUtils.ToBytes<SMB_COM_TRANSACTION2_Request_SMB_Parameters>(this.smbParameters));
        }


        /// <summary>
        /// Encode the struct of SMB_COM_TRANSACTION2_Request_SMB_Data into the struct of SmbData
        /// </summary>
        protected override void EncodeData()
        {
            if (!this.isDivided)
            {
                this.EncodeTrans2Parameters();
                this.EncodeTrans2Data();
            }
            int byteCount = 0;

            if (this.SmbData.Name != null)
            {
                byteCount += this.smbData.Name.Length;
            }
            if (this.smbData.Pad1 != null)
            {
                byteCount += this.smbData.Pad1.Length;
            }
            if (this.smbData.Trans2_Parameters != null)
            {
                byteCount += this.smbData.Trans2_Parameters.Length;
            }
            if (this.smbData.Pad2 != null)
            {
                byteCount += this.smbData.Pad2.Length;
            }
            if (this.smbData.Trans2_Data != null)
            {
                byteCount += this.smbData.Trans2_Data.Length;
            }
            this.smbDataBlock.ByteCount = this.SmbData.ByteCount;
            this.smbDataBlock.Bytes = new byte[byteCount];
            using (MemoryStream memoryStream = new MemoryStream(this.smbDataBlock.Bytes))
            {
            using (Channel channel = new Channel(null, memoryStream))
            {
                channel.BeginWriteGroup();
                if (this.SmbData.Name != null)
                {
                    channel.WriteBytes(this.SmbData.Name);
                }
                if (this.SmbData.Pad1 != null)
                {
                    channel.WriteBytes(this.SmbData.Pad1);
                }
                if (this.SmbData.Trans2_Parameters != null)
                {
                    channel.WriteBytes(this.SmbData.Trans2_Parameters);
                }
                if (this.SmbData.Pad2 != null)
                {
                    channel.WriteBytes(this.SmbData.Pad2);
                }
                if (this.SmbData.Trans2_Data != null)
                {
                    channel.WriteBytes(this.SmbData.Trans2_Data);
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
            this.smbParameters = TypeMarshal.ToStruct<SMB_COM_TRANSACTION2_Request_SMB_Parameters>(
                TypeMarshal.ToBytes(this.smbParametersBlock));
        }


        /// <summary>
        /// to decode the smb data: from the general SmbDada to the concrete Smb Data.
        /// </summary>
        protected override void DecodeData()
        {
            this.smbData = new SMB_COM_TRANSACTION2_Request_SMB_Data();
            using (MemoryStream memoryStream = new MemoryStream(CifsMessageUtils.ToBytes<SmbData>(this.smbDataBlock)))
            {
                using (Channel channel = new Channel(null, memoryStream))
                {
                    this.smbData.ByteCount = channel.Read<ushort>();

                    if ((this.smbHeader.Flags2 & SmbFlags2.SMB_FLAGS2_UNICODE) == SmbFlags2.SMB_FLAGS2_UNICODE)
                    {
                        List<ushort> array = new List<ushort>();
                        ushort letter;

                        do
                        {
                            letter = channel.Read<ushort>();
                            array.Add(letter);
                        }
                        while (letter != new ushort());
                        this.smbData.Name = CifsMessageUtils.ToBytesArray(array.ToArray());
                    }
                    else
                    {
                        List<byte> array = new List<byte>();
                        byte letter;

                        do
                        {
                            letter = channel.Read<byte>();
                            array.Add(letter);
                        }
                        while (letter != new byte());

                        this.smbData.Name = array.ToArray();
                    }

                    this.smbData.Pad1 = channel.ReadBytes(this.smbParameters.ParameterOffset - this.HeaderSize
                        - this.smbParameters.WordCount * 2 - SmbComTransactionPacket.SmbParametersWordCountLength
                        - SmbComTransactionPacket.SmbDataByteCountLength - this.smbData.Name.Length);
                    this.smbData.Trans2_Parameters = channel.ReadBytes(this.smbParameters.ParameterCount);
                    if (this.smbParameters.DataOffset > 0)
                    {
                        this.smbData.Pad2 = channel.ReadBytes(this.smbParameters.DataOffset
                            - this.smbParameters.ParameterOffset - this.smbParameters.ParameterCount);
                        this.smbData.Trans2_Data = channel.ReadBytes(this.smbParameters.DataCount);
                    }
                    else
                    {
                        this.smbData.Pad2 = new byte[0];
                        this.smbData.Trans2_Data = new byte[0];
                    }
                }
                this.DecodeTrans2Parameters();
                this.DecodeTrans2Data();
            }
        }


        /// <summary>
        /// Encode the struct of Trans2Parameters into the byte array in SmbData.Trans2_Parameters
        /// </summary>
        protected abstract void EncodeTrans2Parameters();

        /// <summary>
        /// Encode the struct of Trans2Data into the byte array in SmbData.Trans2_Data
        /// </summary>
        protected abstract void EncodeTrans2Data();

        /// <summary>
        /// to decode the Trans2 parameters: from the general Trans2Parameters to the concrete Trans2 Parameters.
        /// </summary>
        protected abstract void DecodeTrans2Parameters();

        /// <summary>
        /// to decode the Trans2 data: from the general Trans2Dada to the concrete Trans2 Data.
        /// </summary>
        protected abstract void DecodeTrans2Data();

        #endregion


        #region initialize fields with default value

        /// <summary>
        /// init packet, set default field data
        /// </summary>
        private void InitDefaultValue()
        {
            this.smbParameters.SetupCount = 0x1;
            this.smbParameters.Setup = new ushort[1];
            this.smbData.Name = new byte[0];
            this.smbData.Pad1 = new byte[0];
            this.smbData.Pad2 = new byte[0];
            this.smbData.Trans2_Data = new byte[0];
            this.smbData.Trans2_Parameters = new byte[0];
        }

        #endregion
    }
}
