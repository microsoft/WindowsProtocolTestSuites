// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using System;
using System.Collections.Generic;
using System.Text;

namespace Microsoft.Protocols.TestTools.StackSdk.RemoteDesktop.Rdprfx
{
    /// <summary>
    /// 
    /// </summary>
    public struct TS_RFX_SRVR_CAPS_CONTAINER
    {
        ///
        ///A variable-sized array of bytes. All the bytes in this field MUST be set to 0. The size of the field is given by the corresponding codecPropertiesLength field of the parent TS_BITMAPCODEC, as specified in [MS-RDPBCGR] section 2.2.7.2.10.1.1 Bitmap Codecs Capability Set.
        ///
        public byte[] reserved;
    }
}
