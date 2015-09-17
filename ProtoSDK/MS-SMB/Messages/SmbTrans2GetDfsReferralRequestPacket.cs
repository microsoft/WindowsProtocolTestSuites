// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;

namespace Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Smb
{
    /// <summary>
    /// Packets for SmbTrans2GetDfsReferral Request 
    /// </summary>
    public class SmbTrans2GetDfsReferralRequestPacket : Cifs.SmbTrans2GetDfsReferalRequestPacket
    {
        #region Fields

        /// <summary>
        /// the SMB_Parameters 
        /// </summary>
        protected new SMB_COM_TRANSACTION2_Request_SMB_Parameters smbParameters;
        
        #endregion

        #region Convert from base class

        /// <summary>
        /// initialize packet from base packet. 
        /// </summary>
        /// <param name = "packet">the base packet to initialize this packet. </param>
        public SmbTrans2GetDfsReferralRequestPacket(Cifs.SmbTrans2GetDfsReferalRequestPacket packet)
            : base(packet)
        {
            this.smbParameters = SmbMessageUtils.ConvertTransaction2PacketPayload(packet.SmbParameters);
        }


        #endregion

        #region Constructor

        /// <summary>
        /// Constructor. 
        /// </summary>
        public SmbTrans2GetDfsReferralRequestPacket()
            : base()
        {
        }


        /// <summary>
        /// Constructor: Create a request directly from a buffer. 
        /// </summary>
        public SmbTrans2GetDfsReferralRequestPacket(byte[] data)
            : base(data)
        {
        }


        /// <summary>
        /// Deep copy constructor. 
        /// </summary>
        public SmbTrans2GetDfsReferralRequestPacket(SmbTrans2GetDfsReferralRequestPacket packet)
            : base(packet)
        {
        }


        #endregion
    }
}
