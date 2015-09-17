// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;

using Microsoft.Protocols.TestTools.StackSdk.Messages;
using Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Cifs;

namespace Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Smb
{
    /// <summary>
     /// Packets for SmbOpenAndx Response 
     /// </summary>
    public class SmbOpenAndxResponsePacket : Cifs.SmbOpenAndxResponsePacket
    {
        #region Fields

        private SMB_COM_OPEN_ANDX_Response_SMB_Parameters smbParameters;

        #endregion


        #region Properties

        /// <summary>
        /// get or set the Smb_Parameters:SMB_COM_OPEN_ANDX_Response_SMB_Parameters 
        /// </summary>
        public new SMB_COM_OPEN_ANDX_Response_SMB_Parameters SmbParameters
        {
            get
            {
                return this.smbParameters;
            }
            set
            {
                this.smbParameters = value;

                // update the smbParameters of base class.
                Cifs.SMB_COM_OPEN_ANDX_Response_SMB_Parameters param = 
                    new Cifs.SMB_COM_OPEN_ANDX_Response_SMB_Parameters();

                param.FID = this.smbParameters.FID;
                param.WordCount = this.smbParameters.WordCount;
                param.AndXCommand = this.smbParameters.AndXCommand;
                param.AndXReserved = this.smbParameters.AndXReserved;
                param.AndXOffset = this.smbParameters.AndXOffset;
                param.FID = this.smbParameters.FID;
                param.FileAttrs = this.smbParameters.FileAttrs;
                param.LastWriteTime = this.smbParameters.LastWriteTime;
                param.FileDataSize = this.smbParameters.DataSize;
                param.AccessRights = this.smbParameters.GrantedAccess;
                param.ResourceType = this.smbParameters.FileType;
                param.NMPipeStatus = this.smbParameters.DeviceState;
                param.OpenResults = this.smbParameters.Action;
                param.Reserved = new ushort[3];

                base.SmbParameters = param;
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
        public SmbOpenAndxResponsePacket()
            : base()
        {
            this.InitDefaultValue();
        }


        /// <summary>
        /// Constructor: Create a request directly from a buffer. 
        /// </summary>
        public SmbOpenAndxResponsePacket(byte[] data)
            : base(data)
        {
        }


        /// <summary>
        /// Deep copy constructor. 
        /// </summary>
        public SmbOpenAndxResponsePacket(SmbOpenAndxResponsePacket packet)
            : base(packet)
        {
            this.InitDefaultValue();

            this.smbParameters.WordCount = packet.SmbParameters.WordCount;
            this.smbParameters.AndXCommand = packet.SmbParameters.AndXCommand;
            this.smbParameters.AndXReserved = packet.SmbParameters.AndXReserved;
            this.smbParameters.AndXOffset = packet.SmbParameters.AndXOffset;
            this.smbParameters.FID = packet.SmbParameters.FID;
            this.smbParameters.FileAttrs = packet.SmbParameters.FileAttrs;
            this.smbParameters.LastWriteTime = packet.SmbParameters.LastWriteTime;
            this.smbParameters.DataSize = packet.SmbParameters.DataSize;
            this.smbParameters.GrantedAccess = packet.SmbParameters.GrantedAccess;
            this.smbParameters.FileType = packet.SmbParameters.FileType;
            this.smbParameters.DeviceState = packet.SmbParameters.DeviceState;
            this.smbParameters.Action = packet.SmbParameters.Action;
            this.smbParameters.ServerFid = packet.SmbParameters.ServerFid;
            this.smbParameters.Reserved = packet.SmbParameters.Reserved;
            this.smbParameters.MaximalAccessRights = packet.SmbParameters.MaximalAccessRights;
            this.smbParameters.GuestMaximalAccessRights = packet.SmbParameters.GuestMaximalAccessRights;
        }


        #endregion


        #region override methods

        /// <summary>
        /// to create an instance of the StackPacket class that is identical to the current StackPacket. 
        /// </summary>
        /// <returns>a new Packet cloned from this. </returns>
        public override StackPacket Clone()
        {
            return new SmbOpenAndxResponsePacket(this);
        }


        /// <summary>
        /// Encode the struct of SMB_COM_OPEN_ANDX_Response_SMB_Parameters into the struct of SmbParameters 
        /// </summary>
        protected override void EncodeParameters()
        {
            this.smbParametersBlock = CifsMessageUtils.ToStuct<SmbParameters>(
                CifsMessageUtils.ToBytes<SMB_COM_OPEN_ANDX_Response_SMB_Parameters>(this.smbParameters));
        }


        /// <summary>
        /// Encode the struct of SMB_COM_OPEN_ANDX_Response_SMB_Data into the struct of SmbData
        /// </summary>
        protected override void EncodeData()
        {
            this.smbDataBlock.ByteCount = 0x00;
        }


        /// <summary>
        /// to decode the smb parameters: from the general SmbParameters to the concrete Smb Parameters. 
        /// </summary>
        protected override void DecodeParameters()
        {
            if (this.SmbParametersBlock.WordCount > 0)
            {
                this.SmbParameters = CifsMessageUtils.ToStuct<SMB_COM_OPEN_ANDX_Response_SMB_Parameters>(
                    CifsMessageUtils.ToBytes<SmbParameters>(this.smbParametersBlock));
            }
            else
            {
                this.smbParameters.WordCount = this.SmbParametersBlock.WordCount;
            }
        }


        /// <summary>
        /// to decode the smb data: from the general SmbDada to the concrete Smb Data. 
        /// </summary>
        protected override void DecodeData()
        {
            SMB_COM_OPEN_ANDX_Response_SMB_Data smbData = base.SmbData;

            smbData.ByteCount = this.smbDataBlock.ByteCount;

            base.SmbData = smbData;
        }


        /// <summary>
        /// to unmarshal the SmbDada struct from a channel. the open response need to parse specially, because its 
        /// data only contains a ByteCount. 
        /// </summary>
        /// <param name = "channel">the channel started with SmbDada. </param>
        /// <returns>the size in bytes of the SmbDada. </returns>
        protected override int ReadDataFromChannel(Channel channel)
        {
            this.smbDataBlock.ByteCount = channel.Read<ushort>();

            this.DecodeData();

            return 2;
        }


        /// <summary>
        /// to encode the SmbParameters and SmbDada into bytes.
        /// </summary>
        /// <returns>the bytes array of SmbParameters, SmbDada, and AndX if existed.</returns>
        protected override byte[] GetBytesWithoutHeader()
        {
            byte[] bytes = base.GetBytesWithoutHeader();

            // set the byte count of open.
            // Windows-based servers may return a response where the ByteCount field is not initialized to 0.
            int byteCountIndex = this.smbParametersBlock.WordCount * SmbCapability.NUM_BYTES_OF_WORD + SmbCapability.NUM_BYTES_OF_BYTE;
            bytes[byteCountIndex] = (byte)(this.SmbData.ByteCount); // low bits
            bytes[byteCountIndex + 1] = (byte)(this.SmbData.ByteCount >> 8); // high bits

            return bytes;
        }


        #endregion


        #region initialize fields with default value

        /// <summary>
        /// init packet, set default field data 
        /// </summary>
        private void InitDefaultValue()
        {
            this.smbParameters.AndXCommand = SmbCommand.SMB_COM_NO_ANDX_COMMAND;
        }


        #endregion
    }
}
