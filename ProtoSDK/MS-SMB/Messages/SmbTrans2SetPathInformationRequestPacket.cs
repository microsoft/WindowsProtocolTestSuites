// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;

namespace Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Smb
{
    /// <summary>
    /// Packets for SmbTrans2SetPathInformation Request 
    /// </summary>
    public class SmbTrans2SetPathInformationRequestPacket : Cifs.SmbTrans2SetPathInformationRequestPacket
    {
        #region Fields

        /// <summary>
        /// the SMB_Parameters 
        /// </summary>
        protected new SMB_COM_TRANSACTION2_Request_SMB_Parameters smbParameters;

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


        #endregion

        #region Convert from base class

        /// <summary>
        /// initialize packet from base packet. 
        /// </summary>
        /// <param name = "packet">the base packet to initialize this packet. </param>
        public SmbTrans2SetPathInformationRequestPacket(Cifs.SmbTrans2SetPathInformationRequestPacket packet)
            : base(packet)
        {
            this.smbParameters = SmbMessageUtils.ConvertTransaction2PacketPayload(packet.SmbParameters);
        }


        #endregion

        #region Constructor

        /// <summary>
        /// Constructor. 
        /// </summary>
        public SmbTrans2SetPathInformationRequestPacket()
            : base()
        {
        }


        /// <summary>
        /// Constructor: Create a request directly from a buffer. 
        /// </summary>
        public SmbTrans2SetPathInformationRequestPacket(byte[] data)
            : base(data)
        {
        }


        /// <summary>
        /// Deep copy constructor. 
        /// </summary>
        public SmbTrans2SetPathInformationRequestPacket(SmbTrans2SetPathInformationRequestPacket packet)
            : base(packet)
        {
        }


        #endregion

        #region override methods

        /// <summary>
        /// Encode the struct of Trans2Data into the byte array in SmbData.Trans2_Data 
        /// </summary>
        protected override void EncodeTrans2Data()
        {
            this.smbData.Trans2_Data = (byte[])this.Trans2Data.Data;
        }


        #endregion
    }
}
