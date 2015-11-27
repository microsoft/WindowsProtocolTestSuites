// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Text;

namespace Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Dfsc
{
    /// <summary>
    /// REQ_GET_DFS_REFERRAL packet
    /// </summary>
    public class DfscReferralRequestPacket
    {
        #region Fields

        private REQ_GET_DFS_REFERRAL referralRequest;

        private const byte sizeofMaxReferralLevel = 2;

        #endregion


        #region Properties

        /// <summary>
        /// REQ_GET_DFS_REFERRAL structure
        /// </summary>
        public REQ_GET_DFS_REFERRAL ReferralRequest
        {
            get
            {
                return this.referralRequest;
            }
            set
            {
                this.referralRequest = value;
            }
        }

        #endregion


        #region Methods

        /// <summary>
        /// Return REQ_GET_DFS_REFERRAL structure in byte array
        /// RequestFileName MUST NOT be null
        /// </summary>
        /// <returns>REQ_GET_DFS_REFERRAL structure in byte array</returns>
        public byte[] ToBytes()
        {
            byte[] fileName = this.referralRequest.RequestFileName;
            byte[] payload = new byte[fileName.Length + sizeofMaxReferralLevel];

            // Put the lower byte of MaxReferralLevel into payload[0]
            // Put the upper byte of MaxReferralLevel into payload[1]
            // Sample: MaxReferralLevel = 0x0A0B; 
            // payload[0] = 0x0B; payload[1] = 0x0A
            payload[0] = (byte)(this.referralRequest.MaxReferralLevel & 0xFF);
            payload[1] = (byte)(this.referralRequest.MaxReferralLevel >> 8);

            Array.Copy(fileName, 0, payload, sizeofMaxReferralLevel,
                fileName.Length);

            return payload;
        }
        #endregion
    }
}