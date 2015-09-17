// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;

using Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Cifs;

namespace Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Smb
{
    /// <summary>
    ///   Packets for SmbTransMailslotWrite Response
    /// </summary>
    public class SmbTransMailslotWriteResponsePacket : Cifs.SmbTransMailslotWriteSuccessResponsePacket
    {
        #region Fields

        // trans data
        private TRANS_MAILSLOT_WRITE_Response_Trans_Data transData;

        #endregion

        #region Properties

        /// <summary>
        /// the trans data
        /// </summary>
        public TRANS_MAILSLOT_WRITE_Response_Trans_Data TransData
        {
            get
            {
                return this.transData;
            }
            set
            {
                this.transData = value;
            }
        }


        #endregion

        #region Convert from base class

        /// <summary>
        /// initialize packet from base packet. 
        /// </summary>
        /// <param name = "packet">the base packet to initialize this packet. </param>
        public SmbTransMailslotWriteResponsePacket(Cifs.SmbTransMailslotWriteSuccessResponsePacket packet)
            : base(packet)
        {
        }


        #endregion

        #region override methods

        /// <summary>
        /// to create an instance of the StackPacket class that is identical to the current StackPacket. 
        /// </summary>
        /// <returns>a new Packet cloned from this.</returns>
        public override StackPacket Clone()
        {
            return new SmbTransMailslotWriteResponsePacket(this);
        }


        /// <summary>
        /// Encode the struct of TransData into the byte array in SmbData.Trans_Data
        /// </summary>
        protected override void EncodeTransData()
        {
            this.smbData.Trans_Data = CifsMessageUtils.ToBytes<ushort>(this.transData.OperationStatus);
        }


        /// <summary>
        /// to decode the Trans data: from the general TransDada to the concrete Trans Data.
        /// </summary>
        protected override void DecodeTransData()
        {
            if (this.smbData.Trans_Data != null && this.smbData.Trans_Data.Length > 0)
            {
                this.transData.OperationStatus = TypeMarshal.ToStruct<ushort>(this.smbData.Trans_Data);
            }
        }


        #endregion
        
        #region Constructor

        /// <summary>
        /// Constructor. 
        /// </summary>
        public SmbTransMailslotWriteResponsePacket()
            : base()
        {
        }


        /// <summary>
        /// Constructor: Create a request directly from a buffer. 
        /// </summary>
        public SmbTransMailslotWriteResponsePacket(byte[] data)
            : base(data)
        {
        }


        /// <summary>
        /// Deep copy constructor. 
        /// </summary>
        public SmbTransMailslotWriteResponsePacket(SmbTransMailslotWriteResponsePacket packet)
            : base(packet)
        {
        }


        #endregion
    }
}
