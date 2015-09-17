// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;

using Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Cifs;

namespace Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Smb
{
    /// <summary>
    ///  Packets for SmbTrans2QueryFsInformation Response
    /// </summary>
    public class SmbTrans2QueryFsInformationResponsePacket : Cifs.SmbTrans2QueryFsInformationFinalResponsePacket
    {
        #region Convert from base class

        /// <summary>
        /// initialize packet from base packet. 
        /// </summary>
        /// <param name = "packet">the base packet to initialize this packet. </param>
        public SmbTrans2QueryFsInformationResponsePacket(
            Cifs.SmbTrans2QueryFsInformationFinalResponsePacket packet)
            : base(packet)
        {
        }


        #endregion

        #region Constructor

        /// <summary>
        /// to create an instance of the StackPacket class that is identical to the current StackPacket. 
        /// </summary>
        /// <returns>a new Packet cloned from this.</returns>
        public override StackPacket Clone()
        {
            return new SmbTrans2QueryFsInformationResponsePacket(this);
        }


        /// <summary>
        /// Constructor. 
        /// </summary>
        public SmbTrans2QueryFsInformationResponsePacket()
            : base()
        {
        }


        /// <summary>
        /// Deep copy constructor.
        /// </summary>
        public SmbTrans2QueryFsInformationResponsePacket(QueryFSInformationLevel informationLevel)
            : base((QueryFSInformationLevel)informationLevel)
        {
        }


        /// <summary>
        /// Constructor: Create a request directly from a buffer. 
        /// </summary>
        public SmbTrans2QueryFsInformationResponsePacket(byte[] data)
            : base(data)
        {
        }


        #endregion

        #region Methods

        /// <summary>
        /// to decode the Trans2 data: from the general Trans2Dada to the concrete Trans2 Data.
        /// </summary>
        protected override void DecodeTrans2Data()
        {
            if ((ushort)this.informationLevel < SmbCapability.CONST_SMB_INFO_PASSTHROUGH)
            {
                base.DecodeTrans2Data();
                return;
            }

            TRANS2_QUERY_FS_INFORMATION_Response_Trans2_Data trans2Data = this.Trans2Data;

            trans2Data.Data = this.smbData.Trans2_Data;

            this.Trans2Data = trans2Data;
        }


        #endregion
    }
}
