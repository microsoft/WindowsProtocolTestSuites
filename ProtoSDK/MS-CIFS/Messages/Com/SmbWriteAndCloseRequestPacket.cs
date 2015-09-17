// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using Microsoft.Protocols.TestTools.StackSdk;

namespace Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Cifs
{
    /// <summary>
    /// Packets for SmbWriteAndClose Request
    /// </summary>
    public class SmbWriteAndCloseRequestPacket : SmbSingleRequestPacket
    {
        #region Fields

        private SMB_COM_WRITE_AND_CLOSE_Request_SMB_Parameters smbParameters;
        private SMB_COM_WRITE_AND_CLOSE_Request_SMB_Data smbData;

        #endregion


        #region Properties

        /// <summary>
        /// get or set the Smb_Parameters:SMB_COM_WRITE_AND_CLOSE_Request_SMB_Parameters
        /// </summary>
        public SMB_COM_WRITE_AND_CLOSE_Request_SMB_Parameters SmbParameters
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
        /// get or set the Smb_Data:SMB_COM_WRITE_AND_CLOSE_Request_SMB_Data
        /// </summary>
        public SMB_COM_WRITE_AND_CLOSE_Request_SMB_Data SmbData
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
        public SmbWriteAndCloseRequestPacket()
            : base()
        {
            this.InitDefaultValue();
        }


        /// <summary>
        /// Constructor: Create a request directly from a buffer.
        /// </summary>
        public SmbWriteAndCloseRequestPacket(byte[] data)
            : base(data)
        {
        }


        /// <summary>
        /// Deep copy constructor.
        /// </summary>
        public SmbWriteAndCloseRequestPacket(SmbWriteAndCloseRequestPacket packet)
            : base(packet)
        {
            this.InitDefaultValue();

            this.smbParameters.WordCount = packet.SmbParameters.WordCount;
            this.smbParameters.FID = packet.SmbParameters.FID;
            this.smbParameters.CountOfBytesToWrite = packet.SmbParameters.CountOfBytesToWrite;
            this.smbParameters.WriteOffsetInBytes = packet.SmbParameters.WriteOffsetInBytes;
            this.smbParameters.LastWriteTime = new UTime();
            this.smbParameters.LastWriteTime.Time = packet.SmbParameters.LastWriteTime.Time;
            if (packet.smbParameters.Reserved != null)
            {
                this.smbParameters.Reserved = new uint[packet.smbParameters.Reserved.Length];
                Array.Copy(packet.smbParameters.Reserved,
                    this.smbParameters.Reserved, packet.smbParameters.Reserved.Length);
            }
            else
            {
                this.smbParameters.Reserved = new uint[0];
            }

            this.smbData.ByteCount = packet.SmbData.ByteCount;
            this.smbData.Pad = packet.SmbData.Pad;
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
            return new SmbWriteAndCloseRequestPacket(this);
        }


        /// <summary>
        /// Encode the struct of SMB_COM_WRITE_AND_CLOSE_Request_SMB_Parameters into the struct of SmbParameters
        /// </summary>
        protected override void EncodeParameters()
        {
            this.smbParametersBlock = TypeMarshal.ToStruct<SmbParameters>(
                CifsMessageUtils.ToBytes<SMB_COM_WRITE_AND_CLOSE_Request_SMB_Parameters>(this.smbParameters));
        }


        /// <summary>
        /// Encode the struct of SMB_COM_WRITE_AND_CLOSE_Request_SMB_Data into the struct of SmbData
        /// </summary>
        protected override void EncodeData()
        {
            this.smbDataBlock = TypeMarshal.ToStruct<SmbData>(
                CifsMessageUtils.ToBytes<SMB_COM_WRITE_AND_CLOSE_Request_SMB_Data>(this.SmbData));
        }


        /// <summary>
        /// to decode the smb parameters: from the general SmbParameters to the concrete Smb Parameters.
        /// </summary>
        protected override void DecodeParameters()
        {
            this.smbParameters = TypeMarshal.ToStruct<SMB_COM_WRITE_AND_CLOSE_Request_SMB_Parameters>(
                TypeMarshal.ToBytes(this.smbParametersBlock));
        }


        /// <summary>
        /// to decode the smb data: from the general SmbDada to the concrete Smb Data.
        /// </summary>
        protected override void DecodeData()
        {
            this.smbData = TypeMarshal.ToStruct<SMB_COM_WRITE_AND_CLOSE_Request_SMB_Data>(
                TypeMarshal.ToBytes(this.smbDataBlock));
        }

        #endregion


        #region initialize fields with default value

        /// <summary>
        /// init packet, set default field data
        /// </summary>
        private void InitDefaultValue()
        {
            this.smbData.Data = new byte[0];
        }

        #endregion
    }
}
