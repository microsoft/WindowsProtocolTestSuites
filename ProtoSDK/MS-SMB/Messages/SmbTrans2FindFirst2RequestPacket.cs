// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;

namespace Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Smb
{
    /// <summary>
    /// Packets for SmbTrans2FindFirst2 Request 
    /// </summary>
    public class SmbTrans2FindFirst2RequestPacket : Cifs.SmbTrans2FindFirst2RequestPacket
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
        public SmbTrans2FindFirst2RequestPacket(Cifs.SmbTrans2FindFirst2RequestPacket packet)
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
            return new SmbTrans2FindFirst2RequestPacket(this);
        }


        /// <summary>
        /// Constructor. 
        /// </summary>
        public SmbTrans2FindFirst2RequestPacket()
            : base()
        {
        }


        /// <summary>
        /// Constructor: Create a request directly from a buffer. 
        /// </summary>
        public SmbTrans2FindFirst2RequestPacket(byte[] data)
            : base(data)
        {
        }


        #endregion
    }
}
