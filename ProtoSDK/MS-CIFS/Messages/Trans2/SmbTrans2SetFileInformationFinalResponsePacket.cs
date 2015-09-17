// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using Microsoft.Protocols.TestTools.StackSdk;

namespace Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Cifs
{
    /// <summary>
    /// Packets for SmbTrans2SetFileInformationFinal Response
    /// </summary>
    public class SmbTrans2SetFileInformationFinalResponsePacket : SmbTransaction2FinalResponsePacket
    {
        #region Fields

        private TRANS2_SET_PATH_INFORMATION_Response_Trans2_Parameters trans2Parameters;

        #endregion


        #region Properties

        /// <summary>
        /// get or set the Trans2_Parameters:TRANS2_SET_PATH_INFORMATION_Response_Trans2_Parameters
        /// </summary>
        public TRANS2_SET_PATH_INFORMATION_Response_Trans2_Parameters Trans2Parameters
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

        #endregion


        #region Constructor

        /// <summary>
        /// Constructor.
        /// </summary>
        public SmbTrans2SetFileInformationFinalResponsePacket()
            : base()
        {
            this.InitDefaultValue();
        }


        /// <summary>
        /// Constructor: Create a request directly from a buffer. 
        /// </summary>
        public SmbTrans2SetFileInformationFinalResponsePacket(byte[] data)
            : base(data)
        {
        }


        /// <summary>
        /// Deep copy constructor.
        /// </summary>
        public SmbTrans2SetFileInformationFinalResponsePacket(SmbTrans2SetFileInformationFinalResponsePacket packet)
            : base(packet)
        {
            this.InitDefaultValue();
            this.trans2Parameters.EaErrorOffset = packet.trans2Parameters.EaErrorOffset;
        }

        #endregion


        #region override methods

        /// <summary>
        /// to create an instance of the StackPacket class that is identical to the current StackPacket. 
        /// </summary>
        /// <returns>a new Packet cloned from this.</returns>
        public override StackPacket Clone()
        {
            return new SmbTrans2SetFileInformationFinalResponsePacket(this);
        }


        /// <summary>
        /// Encode the struct of Trans2Parameters into the byte array in SmbData.Trans2_Parameters
        /// </summary>
        protected override void EncodeTrans2Parameters()
        {
            this.smbData.Trans2_Parameters = CifsMessageUtils.ToBytes<
                TRANS2_SET_PATH_INFORMATION_Response_Trans2_Parameters>(this.trans2Parameters);
        }


        /// <summary>
        /// Encode the struct of Trans2Data into the byte array in SmbData.Trans2_Data
        /// </summary>
        protected override void EncodeTrans2Data()
        {
        }


        /// <summary>
        /// to decode the Trans2 parameters: from the general Trans2Parameters to the concrete Trans2 Parameters.
        /// </summary>
        protected override void DecodeTrans2Parameters()
        {
            if (this.smbData.Trans2_Parameters != null && this.smbData.Trans2_Parameters.Length > 0)
            {
                this.trans2Parameters = CifsMessageUtils.ToStuct<
                    TRANS2_SET_PATH_INFORMATION_Response_Trans2_Parameters>(this.smbData.Trans2_Parameters);
            }
        }


        /// <summary>
        /// to decode the Trans2 data: from the general Trans2Dada to the concrete Trans2 Data.
        /// </summary>
        protected override void DecodeTrans2Data()
        {
        }

        #endregion


        #region initialize fields with default value

        /// <summary>
        /// init packet, set default field data
        /// </summary>
        private void InitDefaultValue()
        {
        }

        #endregion
    }
}