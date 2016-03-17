// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Protocols.TestTools.StackSdk;

namespace Microsoft.Protocols.TestTools.StackSdk.RemoteDesktop.Rdpegfx
{
    /// <summary>
    /// It decodes byte stream into CLEARCODEC_RESIDUAL_DATA structure
    /// </summary>
    public class ClearCodecResidualDecoder : BasicTypeDecoder
    {
        #region method

        /// <summary>
        /// Constructor
        /// </summary>
        public ClearCodecResidualDecoder(byte[] Data)
            : base(Data)
        {
        }

        /// <summary>
        /// It decode byte stream into CLEARCODEC_RESIDUAL_DATA structure
        /// </summary>
        /// <param name = "residualSegs"> the structure that decode result save to </param>
        /// <return> true if decode success, otherwise return false </return>
        public bool Decode(ref CLEARCODEC_RESIDUAL_DATA residualSegs)
        {
            if (decodeData == null) return false;

            List<CLEARCODEC_RGB_RUN_SEGMENT> segList = new List<CLEARCODEC_RGB_RUN_SEGMENT>();

            while (offset < decodeData.Count())
            {
                CLEARCODEC_RGB_RUN_SEGMENT runSeg = new CLEARCODEC_RGB_RUN_SEGMENT();
                if (!DecodeByte(ref runSeg.buleValue)) return false;
                if (!DecodeByte(ref runSeg.greenValue)) return false;
                if (!DecodeByte(ref runSeg.redValue)) return false;
                // decode rlfactor
                if (!DecodeRulLengthFactor(ref runSeg.rlFactor)) return false;

                segList.Add(runSeg);
            }

            residualSegs.resRLSegArr = segList.ToArray();
            return true;
        }
        #endregion
    }
}
