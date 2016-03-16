// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using System;
using System.Diagnostics.CodeAnalysis;
using Microsoft.Protocols.TestTools.StackSdk.Messages.Marshaling;

namespace Microsoft.Protocols.TestTools.StackSdk.RemoteDesktop.Rdprfx
{
    /// <summary>
    /// Possible values of the version field.
    /// </summary>
    public enum version_Value : ushort
    {
        /// <summary>
        /// Version 0.9
        /// </summary>
        CLW_VERSION_0_9 = 0x0009,

        /// <summary>
        /// Version 1.0
        /// </summary>
        CLW_VERSION_1_0 = 0x0100,
    }

    /// <summary>
    /// Possible values of the entropyBits field.
    /// </summary>
    [Flags]
    public enum entropyBits_Value : byte
    {
        /// <summary>
        /// RLGR1 algorithm.
        /// </summary>
        CLW_ENTROPY_RLGR1 = 0x01,

        /// <summary>
        /// RLGR2 algorithm.
        /// </summary>
        CLW_ENTROPY_RLGR3 = 0x04,
    }

    /// <summary>
    /// The TS_RFX_ICAP structure specifies the set of codec properties that the decoder supports.
    /// </summary>
    public struct TS_RFX_ICAP
    {
        /// <summary>
        /// Specifies the codec version.
        /// </summary>
        public version_Value version;

        /// <summary>
        /// Specifies the width and height of a tile. This field MUST be set to CT_TILE_64x64 (0x0040),
        /// indicating that a tile is 64 x 64 pixels.
        /// </summary>
        public short tileSize;

        /// <summary>
        /// Specifies operational flags.
        /// </summary>
        public byte flags;

        /// <summary>
        /// Specifies the color conversion transform. This field MUST be set to CLW_COL_CONV_ICT (0x1)
        /// to specify the irreversible component transformation (ICT) defined by the equations
        /// in sections 3.1.8.1.3 and 3.1.8.2.5.
        /// </summary>
        public byte colConvBits;

        /// <summary>
        /// Specifies the DWT. This field MUST be set to CLW_XFORM_DWT_53_A (0x1), the DWT 
        /// transform given by equations in Figure 4 and Figure 10.
        /// </summary>
        public byte transformBits;

        /// <summary>
        /// Specifies the entropy algorithm.
        /// </summary>
        public entropyBits_Value entropyBits;

        /// <summary>
        /// Marshals this struct to managed byte array.
        /// </summary>
        /// <returns>Marshalled managed byte array.</returns>
        public byte[] ToBytes()
        {
            // This field MUST be set to CT_TILE_64x64 (0x0040),
            // indicating that a tile is 64 x 64 pixels.
            tileSize = 0x0040;
            // This field MUST be set to CLW_COL_CONV_ICT (0x1) to specify
            // the irreversible component transformation (ICT) defined by
            // the equations in sections 3.1.8.1.3 and 3.1.8.2.5.
            colConvBits = 0x1;
            // This field MUST be set to CLW_XFORM_DWT_53_A (0x1),
            // the DWT transform given by equations in Figure 4 and Figure 10.
            transformBits = 0x1;

            return TypeMarshal.ToBytes<TS_RFX_ICAP>(this);
        }
    }
}
