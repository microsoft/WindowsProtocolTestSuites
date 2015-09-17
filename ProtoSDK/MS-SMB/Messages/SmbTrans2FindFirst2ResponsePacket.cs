// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;

using Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Cifs;

namespace Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Smb
{
    /// <summary>
    /// Packets for SmbTrans2FindFirst2 Response
    /// </summary>
    public class SmbTrans2FindFirst2ResponsePacket : Cifs.SmbTrans2FindFirst2FinalResponsePacket
    {
        #region Convert from base class

        /// <summary>
        /// initialize packet from base packet. 
        /// </summary>
        /// <param name = "packet">the base packet to initialize this packet. </param>
        public SmbTrans2FindFirst2ResponsePacket(Cifs.SmbTrans2FindFirst2FinalResponsePacket packet)
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
            return new SmbTrans2FindFirst2ResponsePacket(this);
        }


        /// <summary>
        /// Constructor. 
        /// </summary>
        public SmbTrans2FindFirst2ResponsePacket()
            : base()
        {
        }


        /// <summary>
        /// Constructor: Create a request directly from a buffer. 
        /// </summary>
        public SmbTrans2FindFirst2ResponsePacket(byte[] data)
            : base(data)
        {
        }


        /// <summary>
        /// Deep copy constructor. 
        /// Constructor: Create a request directly from a buffer.
        /// </summary>
        public SmbTrans2FindFirst2ResponsePacket(
            Cifs.FindInformationLevel informationLevel,
            bool isResumeKeyExisted)
            : base((Cifs.FindInformationLevel)informationLevel, isResumeKeyExisted)
        {
        }


        #endregion

        #region override methods

        /// <summary>
        /// to decode the Trans2 data: from the general Trans2Dada to the concrete Trans2 Data.
        /// </summary>
        protected override void DecodeTrans2Data()
        {
            if (this.smbData.Trans2_Data != null && this.smbData.Trans2_Data.Length > 0)
            {
                TRANS2_FIND_FIRST2_Response_Trans2_Data trans2Data;

                trans2Data.Data = SmbMessageUtils.UnmarshalSmbFindInformationLevelPayloads(
                    (FindInformationLevel)this.informationLevel,
                    this.Trans2Parameters.SearchCount,
                    this.smbData.Trans2_Data);

                if (trans2Data.Data != null)
                {
                    this.Trans2Data = trans2Data;
                    return;
                }
            }

            base.DecodeTrans2Data();
        }


        #endregion
    }
}
