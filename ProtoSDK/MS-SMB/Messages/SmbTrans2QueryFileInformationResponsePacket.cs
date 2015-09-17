// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;

using Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Cifs;

namespace Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Smb
{
    /// <summary>
    /// Packets for SmbTrans2QueryFileInformation Response
    /// </summary>
    public class SmbTrans2QueryFileInformationResponsePacket : Cifs.SmbTrans2QueryFileInformationFinalResponsePacket
    {
        #region Convert from base class

        /// <summary>
        /// initialize packet from base packet. 
        /// </summary>
        /// <param name = "packet">the base packet to initialize this packet. </param>
        public SmbTrans2QueryFileInformationResponsePacket(Cifs.SmbTrans2QueryFileInformationFinalResponsePacket packet)
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
            return new SmbTrans2QueryFileInformationResponsePacket(this);
        }


        /// <summary>
        /// Constructor. 
        /// </summary>
        public SmbTrans2QueryFileInformationResponsePacket()
            : base()
        {
        }


        /// <summary>
        /// Constructor: Create a request directly from a buffer. 
        /// </summary>
        public SmbTrans2QueryFileInformationResponsePacket(byte[] data)
            : base(data)
        {
        }


        /// <summary>
        /// Constructor: Create a request directly from a buffer.
        /// </summary>
        public SmbTrans2QueryFileInformationResponsePacket(QueryInformationLevel informationLevel)
            : base(informationLevel)
        {
        }


        #endregion

        #region Methods

        /// <summary>
        /// no trans2 parameters for query fs information.
        /// </summary>
        protected override void EncodeTrans2Parameters()
        {
            this.smbData.Trans2_Parameters = new byte[0];
        }

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

            TRANS2_QUERY_PATH_INFORMATION_Response_Trans2_Data trans2Data = this.Trans2Data;

            trans2Data.Data = this.smbData.Trans2_Data;

            this.Trans2Data = trans2Data;
        }


        #endregion
    }
}
