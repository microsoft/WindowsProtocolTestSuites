// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using System;
using System.Collections.Generic;
using Microsoft.Protocols.TestTools.StackSdk.RemoteDesktop.Rdprfx;

namespace Microsoft.Protocols.TestTools.StackSdk.RemoteDesktop.Rdpegfx
{
    /// <summary>
    /// The RFX_PROGRESSIVE_CODEC_QUANT structure specifies a progressive quantization table for compressing a tile.
    /// </summary>
    public class RFX_PROGRESSIVE_CODEC_QUANT
    {
        /// <summary>
        /// An 8-bit unsigned integer that specifies the quality associated with 
        /// the progressive stage as a value between 0 (0x00) and 100 (0x64), where 100 (0x64) indicates 
        /// that the tile will reach its final target quality. This value SHOULD be ignored by the decoder.
        /// </summary>
        public byte quality;

        /// <summary>
        /// A RFX_COMPONMENT_CODEC_QUANT ([MS-RDPEGFX] section 2.2.4.2.1.5.2) structure that contains the progressive quantization table for the Luma (Y) component.
        /// </summary>
        public RFX_COMPONMENT_CODEC_QUANT yQuantValues;

        /// <summary>
        /// A RFX_COMPONMENT_CODEC_QUANT structure that contains the progressive quantization table for the Chroma Blue (Cb) component.
        /// </summary>
        public RFX_COMPONMENT_CODEC_QUANT cbQuantValues;

        /// <summary>
        /// A RFX_COMPONMENT_CODEC_QUANT structure that contains the progressive quantization table for the Chroma Red (Cr) component.
        /// </summary>
        public RFX_COMPONMENT_CODEC_QUANT crQuantValues;
    }

    /// <summary>
    /// The RFX_COMPONMENT_CODEC_QUANT structure holds the scalar quantization values for
    /// the ten sub-bands in the 3-level DWT decomposition of RDPEGFX.
    /// </summary>
    public struct RFX_COMPONMENT_CODEC_QUANT
    {
        /// <summary>
        /// LL3 (4 bits): A 4-bit, unsigned integer. The LL quantization factor for the level-3 DWT sub-band.
        /// HL3 (4 bits): A 4-bit, unsigned integer. The HL quantization factors for the level-3 DWT sub-band.
        /// </summary>
        public byte LL3_HL3;

        /// <summary>
        /// LH3 (4 bits): A 4-bit, unsigned integer. The LH quantization factor for the level-3 DWT sub-band.
        /// HH3 (4 bits): A 4-bit, unsigned integer. The HH quantization factors for the level-3 DWT sub-band.
        /// </summary>
        public byte LH3_HH3;

        /// <summary>
        /// HL2 (4 bits): A 4-bit, unsigned integer. The HL quantization factor for the level-2 DWT sub-band.
        /// LH2 (4 bits): A 4-bit, unsigned integer. The LH quantization factor for the level-2 DWT sub-band.
        /// </summary>
        public byte HL2_LH2;

        /// <summary>
        /// HH2 (4 bits): A 4-bit, unsigned integer. The HH quantization factor for the level-2 DWT sub-band.
        /// HL1 (4 bits): A 4-bit, unsigned integer. The HL quantization factors for the level-1 DWT sub-band.
        /// </summary>
        public byte HH2_HL1;

        /// <summary>
        /// LH1 (4 bits): A 4-bit, unsigned integer. The LH quantization factors for the level-1 DWT sub-band.
        /// HH1 (4 bits): A 4-bit, unsigned integer. The HH quantization factor for the level-1 DWT sub-band.
        /// </summary>
        public byte LH1_HH1;
    }
}
