// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Dfsc
{
    /// <summary>
    /// REQ_GET_DFS_REFERRAL_EX_packet
    /// </summary>
    public class DfscReferralRequestEXPacket
    {
        #region Fields

        private REQ_GET_DFS_REFERRAL_EX referralRequestEX;

        private const byte sizeofMaxReferralLevel = 2;

        private const byte sizeofRequestFlags = 2;

        private const byte sizeofRequestDataLength = 4;

        private const byte sizeofRequestFileNameLength = 2;

        #endregion

        #region Properties

        /// <summary>
        /// <return> REQ_GET_DFS_REFERRAL_EX structure </return>
        /// </summary>
        public REQ_GET_DFS_REFERRAL_EX ReferralRequest
        {
            get
            {
                return this.referralRequestEX;
            }
            set
            {
                referralRequestEX = value;
            }
        }

        #endregion

        #region methods

        /// <summary>
        /// Return REQ_GET_DFS_REFERRAL_EX structure in byte array
        /// Zero length RequestFileName is treated normally with preceding two bytes indicating a length of 0
        /// Zero SiteName is treated normally with preceding two bytes indicating a length of 0
        /// </summary>
        /// <returns>REQ_GET_DFS_REFERRAL_EX structure in byte array</returns>
        public byte[] ToBytes()
        {
            List<byte> payload = new List<byte>();

            payload.AddRange(TypeMarshal.ToBytes(this.referralRequestEX.MaxReferralLevel));
            payload.AddRange(TypeMarshal.ToBytes(this.referralRequestEX.RequestFlags));
            payload.AddRange(TypeMarshal.ToBytes(this.referralRequestEX.RequestDataLength));
            payload.AddRange(TypeMarshal.ToBytes(this.referralRequestEX.RequestData.RequestFileNameLength));
            payload.AddRange(this.referralRequestEX.RequestData.RequestFileName);

            if (this.referralRequestEX.RequestFlags == REQ_GET_DFS_REFERRAL_RequestFlags.SiteName)
            {
                payload.AddRange(TypeMarshal.ToBytes(this.referralRequestEX.RequestData.SiteNameLength));
                payload.AddRange(this.referralRequestEX.RequestData.SiteName);
            }

            return payload.ToArray();
        }
        #endregion
    }

}
