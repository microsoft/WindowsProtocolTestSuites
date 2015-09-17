// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.IO;
using Microsoft.Protocols.TestTools.StackSdk;
using Microsoft.Protocols.TestTools.StackSdk.Messages;

namespace Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Cifs
{
    /// <summary>
    /// Packets for SmbTrans2GetDfsReferal Request
    /// </summary>
    public class SmbTrans2GetDfsReferalRequestPacket : SmbTransaction2RequestPacket
    {
        #region Fields

        private TRANS2_GET_DFS_REFERRAL_Request_Trans2_Parameters trans2Parameters;
        private const ushort referralLevelLength = 2;

        #endregion


        #region Properties

        /// <summary>
        /// get or set the Trans2_Parameters:TRANS2_GET_DFS_REFERRAL_Request_Trans2_Parameters
        /// </summary>
        public TRANS2_GET_DFS_REFERRAL_Request_Trans2_Parameters Trans2Parameters
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


        /// <summary>
        /// get the FID of Trans2_Parameters
        /// </summary>
        internal override ushort FID
        {
            get
            {
                return INVALID_FID;
            }
        }

        #endregion


        #region Constructor

        /// <summary>
        /// Constructor.
        /// </summary>
        public SmbTrans2GetDfsReferalRequestPacket()
            : base()
        {
            this.InitDefaultValue();
        }


        /// <summary>
        /// Constructor: Create a request directly from a buffer.
        /// </summary>
        public SmbTrans2GetDfsReferalRequestPacket(byte[] data)
            : base(data)
        {
        }


        /// <summary>
        /// Deep copy constructor.
        /// </summary>
        public SmbTrans2GetDfsReferalRequestPacket(SmbTrans2GetDfsReferalRequestPacket packet)
            : base(packet)
        {
            this.InitDefaultValue();

            this.trans2Parameters.ReferralRequest = packet.trans2Parameters.ReferralRequest;
        }

        #endregion


        #region override methods

        /// <summary>
        /// to create an instance of the StackPacket class that is identical to the current StackPacket. 
        /// </summary>
        /// <returns>a new Packet cloned from this.</returns>
        public override StackPacket Clone()
        {
            return new SmbTrans2GetDfsReferalRequestPacket(this);
        }


        /// <summary>
        /// Encode the struct of Trans2Parameters into the byte array in SmbData.Trans2_Parameters
        /// </summary>
        protected override void EncodeTrans2Parameters()
        {
            int trans2ParametersCount = referralLevelLength;

            if (this.trans2Parameters.ReferralRequest.RequestFileName != null)
            {
                trans2ParametersCount += this.trans2Parameters.ReferralRequest.RequestFileName.Length;
            }
            this.smbData.Trans2_Parameters = new byte[trans2ParametersCount];
            using (MemoryStream memoryStream = new MemoryStream(this.smbData.Trans2_Parameters))
            {
                using (Channel channel = new Channel(null, memoryStream))
                {
                    channel.BeginWriteGroup();
                    channel.Write<ushort>(this.trans2Parameters.ReferralRequest.MaxReferralLevel);

                    if (this.trans2Parameters.ReferralRequest.RequestFileName != null)
                    {
                        channel.WriteBytes(this.trans2Parameters.ReferralRequest.RequestFileName);
                    }
                    channel.EndWriteGroup();
                }
            }
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
            if (this.smbData.Trans2_Parameters != null)
            {
                using (MemoryStream memoryStream = new MemoryStream(this.smbData.Trans2_Parameters))
                {
                    using (Channel channel = new Channel(null, memoryStream))
                    {
                        this.trans2Parameters.ReferralRequest.MaxReferralLevel = channel.Read<ushort>();
                        this.trans2Parameters.ReferralRequest.RequestFileName = channel.ReadBytes(
                            this.smbParameters.ParameterCount - referralLevelLength);
                    }
                }
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