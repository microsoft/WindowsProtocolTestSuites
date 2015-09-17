// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;

using Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Cifs;

namespace Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Smb
{
    /// <summary>
    /// Packets for SmbTrans2SetFsInformation Request 
    /// </summary>
    public class SmbTrans2SetFsInformationRequestPacket : Cifs.SmbTrans2SetFsInformationRequestPacket
    {
        #region Fields

        /// <summary>
        /// the SMB_Parameters 
        /// </summary>
        protected new SMB_COM_TRANSACTION2_Request_SMB_Parameters smbParameters;

        private TRANS2_SET_FILE_SYSTEM_INFORMATION_Request_Trans2_Parameters trans2Parameters;
        private TRANS2_SET_FILE_SYSTEM_INFORMATION_Request_Trans2_Data trans2Data;
        
        #endregion

        #region Properties

        /// <summary>
        /// get or set the Smb_Parameters:SMB_COM_TRANSACTION2_Request_SMB_Parameters 
        /// </summary>
        public override Cifs.SMB_COM_TRANSACTION2_Request_SMB_Parameters SmbParameters
        {
            get
            {
                return SmbMessageUtils.ConvertTransaction2PacketPayload(this.smbParameters);
            }
            set
            {
                this.smbParameters = SmbMessageUtils.ConvertTransaction2PacketPayload(value);
                base.smbParameters = value;
            }
        }


        /// <summary>
        /// get or set the Trans2_Data:TRANS2_SET_FILE_SYSTEM_INFORMATION_Request_Trans2_Parameters 
        /// </summary>
        public TRANS2_SET_FILE_SYSTEM_INFORMATION_Request_Trans2_Parameters Trans2Parameters
        {
            get
            {
                return this.trans2Parameters;
            }
            set
            {
                this.trans2Parameters = value;
            }
        }


        /// <summary>
        /// get or set the Trans2_Data:TRANS2_SET_FILE_SYSTEM_INFORMATION_Request_Trans2_Parameters 
        /// </summary>
        public TRANS2_SET_FILE_SYSTEM_INFORMATION_Request_Trans2_Data Trans2Data
        {
            get
            {
                return this.trans2Data;
            }
            set
            {
                this.trans2Data = value;
            }
        }


        #endregion

        #region Convert from base class

        /// <summary>
        /// initialize packet from base packet. 
        /// </summary>
        /// <param name = "packet">the base packet to initialize this packet. </param>
        public SmbTrans2SetFsInformationRequestPacket(Cifs.SmbTrans2SetFsInformationRequestPacket packet)
            : base(packet)
        {
            this.smbParameters = SmbMessageUtils.ConvertTransaction2PacketPayload(packet.SmbParameters);
        }


        #endregion

        #region Constructor

        /// <summary>
        /// Constructor. 
        /// </summary>
        public SmbTrans2SetFsInformationRequestPacket()
            : base()
        {
        }


        /// <summary>
        /// Constructor: Create a request directly from a buffer. 
        /// </summary>
        public SmbTrans2SetFsInformationRequestPacket(byte[] data)
            : base(data)
        {
        }


        /// <summary>
        /// Deep copy constructor. 
        /// </summary>
        public SmbTrans2SetFsInformationRequestPacket(SmbTrans2SetFsInformationRequestPacket packet)
            : base(packet)
        {
        }


        #endregion

        #region override methods

        /// <summary>
        /// to create an instance of the StackPacket class that is identical to the current StackPacket. 
        /// </summary>
        /// <returns>a new Packet cloned from this. </returns>
        public override StackPacket Clone()
        {
            return new SmbTrans2SetFsInformationRequestPacket(this);
        }


        /// <summary>
        /// Encode the struct of Trans2Parameters into the byte array in SmbData.Trans2_Parameters 
        /// </summary>
        protected override void EncodeTrans2Parameters()
        {
            this.smbData.Trans2_Parameters =
                CifsMessageUtils.ToBytes<TRANS2_SET_FILE_SYSTEM_INFORMATION_Request_Trans2_Parameters>(
                this.trans2Parameters);
        }


        /// <summary>
        /// Encode the struct of Trans2Data into the byte array in SmbData.Trans2_Data 
        /// </summary>
        protected override void EncodeTrans2Data()
        {
            this.smbData.Trans2_Data = (byte[])this.trans2Data.Data;
        }


        /// <summary>
        /// to decode the Trans2 parameters: from the general Trans2Parameters to the concrete Trans2 Parameters. 
        /// </summary>
        protected override void DecodeTrans2Parameters()
        {
            if (this.smbData.Trans2_Parameters != null)
            {
                this.trans2Parameters = 
                    CifsMessageUtils.ToStuct<TRANS2_SET_FILE_SYSTEM_INFORMATION_Request_Trans2_Parameters>(
                    this.smbData.Trans2_Parameters);
            }
        }


        /// <summary>
        /// to decode the Trans2 data: from the general Trans2Dada to the concrete Trans2 Data. 
        /// </summary>
        protected override void DecodeTrans2Data()
        {
            this.trans2Data.Data = this.smbData.Trans2_Data;
        }


        #endregion
    }
}
