// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;

using Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Cifs;

namespace Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Smb
{
    /// <summary>
    /// Packets for Smb Error Response.
    /// </summary>
    public class SmbErrorResponsePacket : Cifs.SmbSingleResponsePacket
    {
        #region Fields

        private SMB_COM_ErrorResponse_SMB_Parameters smbParameters;
        private SMB_COM_ErrorResponse_SMB_SMB_Data smbData;

        #endregion


        #region Properties

        /// <summary>
        /// get or set the Smb_Parameters:SMB_COM_ErrorResponse_SMB_Parameters 
        /// </summary>
        public SMB_COM_ErrorResponse_SMB_Parameters SmbParameters
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
        /// get or set the Smb_Data:SMB_COM_ErrorResponse_SMB_SMB_Data 
        /// </summary>
        public SMB_COM_ErrorResponse_SMB_SMB_Data SmbData
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
        public SmbErrorResponsePacket()
            : base()
        {
        }


        /// <summary>
        /// Constructor: Create a request directly from a buffer. 
        /// </summary>
        public SmbErrorResponsePacket(byte[] data)
            : base(data)
        {
        }


        /// <summary>
        /// Deep copy constructor. 
        /// </summary>
        public SmbErrorResponsePacket(SmbErrorResponsePacket packet)
            : base(packet)
        {
            this.smbParameters.WordCount = packet.SmbParameters.WordCount;
            this.smbData.ByteCount = packet.SmbData.ByteCount;
        }


        #endregion


        #region override methods

        /// <summary>
        /// to create an instance of the StackPacket class that is identical to the current StackPacket. 
        /// </summary>
        /// <returns>a new Packet cloned from this. </returns>
        public override StackPacket Clone()
        {
            return new SmbErrorResponsePacket(this);
        }


        /// <summary>
        /// Encode the struct of SMB_COM_NEGOTIATE_NtLanManagerResponse_SMB_Parameters into the struct of 
        /// SmbParameters 
        /// </summary>
        protected override void EncodeParameters()
        {
            this.smbParametersBlock = CifsMessageUtils.ToStuct<SmbParameters>(
                CifsMessageUtils.ToBytes<SMB_COM_ErrorResponse_SMB_Parameters>(this.smbParameters));
        }


        /// <summary>
        /// Encode the struct of SMB_COM_NEGOTIATE_NtLanManagerResponse_SMB_Data into the struct of SmbData 
        /// </summary>
        protected override void EncodeData()
        {
            this.smbDataBlock.ByteCount = this.SmbData.ByteCount;
            this.smbDataBlock.Bytes = new byte[this.smbDataBlock.ByteCount];
        }


        /// <summary>
        /// to decode the smb parameters: from the general SmbParameters to the concrete Smb Parameters. 
        /// </summary>
        protected override void DecodeParameters()
        {
            if (this.SmbParametersBlock.WordCount > 0)
            {
                this.smbParameters = CifsMessageUtils.ToStuct<SMB_COM_ErrorResponse_SMB_Parameters>(
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
            if (this.SmbDataBlock.ByteCount > 0)
            {
                this.smbData.ByteCount = this.smbDataBlock.ByteCount;
            }
            else
            {
                this.smbData.ByteCount = this.SmbDataBlock.ByteCount;
            }
        }


        #endregion
    }
}
