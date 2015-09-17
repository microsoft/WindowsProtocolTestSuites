// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;

using Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Cifs;

namespace Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Smb
{
    /// <summary>
    /// Packets for SmbTransTransactNmpipe Request 
    /// </summary>
    public class SmbTransTransactNmpipeRequestPacket : Cifs.SmbTransTransactNmpipeRequestPacket
    {
        #region Properties

        /// <summary>
        /// get or set the Smb_Parameters:SMB_COM_TRANSACTION_Request_SMB_Parameters 
        /// </summary>
        public new SMB_COM_TRANSACTION_Request_SMB_Parameters_File SmbParameters
        {
            get
            {
                return SmbMessageUtils.ConvertTransactionFilePacketPayload(this.smbParameters);
            }
            set
            {
                this.smbParameters = SmbMessageUtils.ConvertTransactionFilePacketPayload(value);
            }
        }


        #endregion
        
        #region Convert from base class

        /// <summary>
        /// initialize packet from base packet. 
        /// </summary>
        /// <param name = "packet">the base packet to initialize this packet. </param>
        public SmbTransTransactNmpipeRequestPacket(Cifs.SmbTransTransactNmpipeRequestPacket packet)
            : base(packet)
        {
        }


        #endregion

        #region Constructor

        /// <summary>
        /// Constructor. 
        /// </summary>
        public SmbTransTransactNmpipeRequestPacket()
            : base()
        {
        }


        /// <summary>
        /// Constructor: Create a request directly from a buffer. 
        /// </summary>
        public SmbTransTransactNmpipeRequestPacket(byte[] data)
            : base(data)
        {
        }


        /// <summary>
        /// Deep copy constructor. 
        /// </summary>
        public SmbTransTransactNmpipeRequestPacket(SmbTransTransactNmpipeRequestPacket packet)
            : base(packet)
        {
        }


        #endregion
    }
}
