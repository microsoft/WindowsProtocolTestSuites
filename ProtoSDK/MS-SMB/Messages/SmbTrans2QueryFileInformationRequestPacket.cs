// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;

namespace Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Smb
{
    /// <summary>
    /// Packets for SmbTrans2QueryFileInformation Request 
    /// </summary>
    public class SmbTrans2QueryFileInformationRequestPacket : Cifs.SmbTrans2QueryFileInformationRequestPacket
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
        public SmbTrans2QueryFileInformationRequestPacket(Cifs.SmbTrans2QueryFileInformationRequestPacket packet)
            : base(packet)
        {
            this.smbParameters = SmbMessageUtils.ConvertTransaction2PacketPayload(packet.SmbParameters);
        }


        #endregion

        #region Constructor

        /// <summary>
        /// to create an instance of the StackPacket class that is identical to the current StackPacket. 
        /// </summary>
        /// <returns>a new Packet cloned from this.</returns>
        public override StackPacket Clone()
        {
            return new SmbTrans2QueryFileInformationRequestPacket(this);
        }


        /// <summary>
        /// Constructor. 
        /// </summary>
        public SmbTrans2QueryFileInformationRequestPacket()
            : base()
        {
        }


        /// <summary>
        /// Constructor: Create a request directly from a buffer. 
        /// </summary>
        public SmbTrans2QueryFileInformationRequestPacket(byte[] data)
            : base(data)
        {
        }


        #endregion
    }
}
