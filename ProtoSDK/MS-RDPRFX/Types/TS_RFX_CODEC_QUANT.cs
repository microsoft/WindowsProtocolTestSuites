// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using System;
using System.Diagnostics.CodeAnalysis;

namespace Microsoft.Protocols.TestTools.StackSdk.RemoteDesktop.Rdprfx
{
    /// <summary>
    /// The TS_RFX_CODEC_QUANT structure holds the scalar quantization values for
    /// the ten sub-bands in the 3-level DWT decomposition.
    /// </summary>
    public struct TS_RFX_CODEC_QUANT
    {
        /// <summary>
        /// LL3 (4 bits): A 4-bit, unsigned integer. The LL quantization factor for the level-3 DWT sub-band.
        /// LH3 (4 bits): A 4-bit, unsigned integer. The LH quantization factor for the level-3 DWT sub-band.
        /// </summary>
        public byte LL3_LH3;

        /// <summary>
        /// HL3 (4 bits): A 4-bit, unsigned integer. The HL quantization factors for the level-3 DWT sub-band.
        /// HH3 (4 bits): A 4-bit, unsigned integer. The HH quantization factors for the level-3 DWT sub-band.
        /// </summary>
        public byte HL3_HH3;

        /// <summary>
        /// LH2 (4 bits): A 4-bit, unsigned integer. The LH quantization factor for the level-2 DWT sub-band.
        /// HL2 (4 bits): A 4-bit, unsigned integer. The HL quantization factor for the level-2 DWT sub-band.
        /// </summary>
        public byte LH2_HL2;

        /// <summary>
        /// HH2 (4 bits): A 4-bit, unsigned integer. The HH quantization factor for the level-2 DWT sub-band.
        /// LH1 (4 bits): A 4-bit, unsigned integer. The LH quantization factors for the level-1 DWT sub-band.
        /// </summary>
        public byte HH2_LH1;

        /// <summary>
        /// HL1 (4 bits): A 4-bit, unsigned integer. The HL quantization factors for the level-1 DWT sub-band.
        /// HH1 (4 bits): A 4-bit, unsigned integer. The HH quantization factor for the level-1 DWT sub-band.
        /// </summary>
        public byte HL1_HH1;
    }
}
