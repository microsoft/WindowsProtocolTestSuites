// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using Microsoft.Protocols.TestTools.StackSdk;

namespace Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Cifs
{
    /// <summary>
    ///  Packets for SmbSetInformation2 Request
    /// </summary>
    public class SmbSetInformation2RequestPacket : SmbSingleRequestPacket
    {
        #region Fields

        private SMB_COM_SET_INFORMATION2_Request_SMB_Parameters smbParameters;
        private SMB_COM_SET_INFORMATION2_Request_SMB_Data smbData;

        #endregion


        #region Properties

        /// <summary>
        /// get or set the Smb_Parameters:SMB_COM_SET_INFORMATION2_Request_SMB_Parameters
        /// </summary>
        public SMB_COM_SET_INFORMATION2_Request_SMB_Parameters SmbParameters
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
        /// get or set the Smb_Data:SMB_COM_SET_INFORMATION2_Request_SMB_Data
        /// </summary>
        public SMB_COM_SET_INFORMATION2_Request_SMB_Data SmbData
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
        public SmbSetInformation2RequestPacket()
            : base()
        {
            this.InitDefaultValue();
        }


        /// <summary>
        /// Constructor: Create a request directly from a buffer.
        /// </summary>
        public SmbSetInformation2RequestPacket(byte[] data)
            : base(data)
        {
        }


        /// <summary>
        /// Deep copy constructor.
        /// </summary>
        public SmbSetInformation2RequestPacket(SmbSetInformation2RequestPacket packet)
            : base(packet)
        {
            this.InitDefaultValue();

            this.smbParameters.WordCount = packet.SmbParameters.WordCount;
            this.smbParameters.FID = packet.SmbParameters.FID;
            this.smbParameters.CreateDate = new SmbDate();
            this.smbParameters.CreateDate.Year = packet.SmbParameters.CreateDate.Year;
            this.smbParameters.CreateDate.Month = packet.SmbParameters.CreateDate.Month;
            this.smbParameters.CreateDate.Day = packet.SmbParameters.CreateDate.Day;
            this.smbParameters.CreationTime = new SmbTime();
            this.smbParameters.CreationTime.Hour = packet.SmbParameters.CreationTime.Hour;
            this.smbParameters.CreationTime.Minutes = packet.SmbParameters.CreationTime.Minutes;
            this.smbParameters.CreationTime.Seconds = packet.SmbParameters.CreationTime.Seconds;
            this.smbParameters.LastAccessDate = new SmbDate();
            this.smbParameters.LastAccessDate.Year = packet.SmbParameters.LastAccessDate.Year;
            this.smbParameters.LastAccessDate.Month = packet.SmbParameters.LastAccessDate.Month;
            this.smbParameters.LastAccessDate.Day = packet.SmbParameters.LastAccessDate.Day;
            this.smbParameters.LastAccessTime = new SmbTime();
            this.smbParameters.LastAccessTime.Hour = packet.SmbParameters.LastAccessTime.Hour;
            this.smbParameters.LastAccessTime.Minutes = packet.SmbParameters.LastAccessTime.Minutes;
            this.smbParameters.LastAccessTime.Seconds = packet.SmbParameters.LastAccessTime.Seconds;
            this.smbParameters.LastWriteDate = new SmbDate();
            this.smbParameters.LastWriteDate.Year = packet.SmbParameters.LastWriteDate.Year;
            this.smbParameters.LastWriteDate.Month = packet.SmbParameters.LastWriteDate.Month;
            this.smbParameters.LastWriteDate.Day = packet.SmbParameters.LastWriteDate.Day;
            this.smbParameters.LastWriteTime = new SmbTime();
            this.smbParameters.LastWriteTime.Hour = packet.SmbParameters.LastWriteTime.Hour;
            this.smbParameters.LastWriteTime.Minutes = packet.SmbParameters.LastWriteTime.Minutes;
            this.smbParameters.LastWriteTime.Seconds = packet.SmbParameters.LastWriteTime.Seconds;

            this.smbData.ByteCount = packet.SmbData.ByteCount;
        }

        #endregion


        #region override methods

        /// <summary>
        /// to create an instance of the StackPacket class that is identical to the current StackPacket. 
        /// </summary>
        /// <returns>a new Packet cloned from this.</returns>
        public override StackPacket Clone()
        {
            return new SmbSetInformation2RequestPacket(this);
        }


        /// <summary>
        /// Encode the struct of SMB_COM_SET_INFORMATION2_Request_SMB_Parameters into the struct of SmbParameters
        /// </summary>
        protected override void EncodeParameters()
        {
            this.smbParametersBlock = TypeMarshal.ToStruct<SmbParameters>(
                CifsMessageUtils.ToBytes<SMB_COM_SET_INFORMATION2_Request_SMB_Parameters>(this.smbParameters));
        }


        /// <summary>
        /// Encode the struct of SMB_COM_SET_INFORMATION2_Request_SMB_Data into the struct of SmbData
        /// </summary>
        protected override void EncodeData()
        {
            this.smbDataBlock = TypeMarshal.ToStruct<SmbData>(
                CifsMessageUtils.ToBytes<SMB_COM_SET_INFORMATION2_Request_SMB_Data>(this.SmbData));
        }


        /// <summary>
        /// to decode the smb parameters: from the general SmbParameters to the concrete Smb Parameters.
        /// </summary>
        protected override void DecodeParameters()
        {
            this.smbParameters = TypeMarshal.ToStruct<SMB_COM_SET_INFORMATION2_Request_SMB_Parameters>(
                TypeMarshal.ToBytes(this.smbParametersBlock));
        }


        /// <summary>
        /// to decode the smb data: from the general SmbDada to the concrete Smb Data.
        /// </summary>
        protected override void DecodeData()
        {
            this.smbData = TypeMarshal.ToStruct<SMB_COM_SET_INFORMATION2_Request_SMB_Data>(
                TypeMarshal.ToBytes(this.smbDataBlock));
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
