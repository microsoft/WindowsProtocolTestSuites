// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Microsoft.Protocols.TestTools.StackSdk.Security.Pac
{
    public class PacDeviceInfo : PacInfoBuffer
    {
        public byte[] RawData;
        internal override void DecodeBuffer(byte[] buffer, int index, int count)
        {
            RawData = new byte[count];
            Array.ConstrainedCopy(buffer, index, RawData, 0, count);

        }

        internal override byte[] EncodeBuffer()
        {
            return RawData;
        }

        internal override int CalculateSize()
        {
            return EncodeBuffer().Length;
        }

        internal override PAC_INFO_BUFFER_Type_Values GetBufferInfoType()
        {
            return PAC_INFO_BUFFER_Type_Values.PacDeviceInfo;
        }
    }
}
