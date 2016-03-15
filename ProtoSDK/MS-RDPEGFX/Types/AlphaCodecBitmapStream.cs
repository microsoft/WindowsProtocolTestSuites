// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using Microsoft.Protocols.TestTools.StackSdk;

namespace Microsoft.Protocols.TestTools.StackSdk.RemoteDesktop.Rdpegfx
{
    /// <summary>
    /// The ALPHACODEC_BITMAP_STREAM structure specifies the opacity of each pixel in the encoded bitmap. 
    /// The number of pixels encoded in the segments field MUST equal the area of the original image when decoded.
    /// </summary>
    public struct ALPHACODEC_BITMAP_STREAM
    {
        /// <summary>
        /// A 16-bit unsigned integer. This field MUST contain the value 16,716 (0x414C).
        /// </summary>
        public ushort alphaSig;

        /// <summary>
        /// A 16-bit unsigned integer. If this field equals 0x0000, the segments field contains the alpha channel values, encoded in raw format, one after the other, in top-left to bottom-right order.
        /// If this field is nonzero, the segments field contains one or more CLEARCODEC_ALPHA_RLE_SEGMENT structures.
        /// </summary>
        public ushort compressed;

        /// <summary>
        /// An optional variable-length array of bytes or CLEARCODEC_ALPHA_RLE_SEGMENT structures,
        /// depending on the value of the compressed field.
        /// </summary>
        public byte[] segments;
    }
}
