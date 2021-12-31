// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Wsp
{
    /// <summary>
    /// The CRowSeekAt structure contains the offset at which to retrieve rows in a CPMGetRowsIn message.
    /// </summary>
    public struct CRowSeekAt : IWspSeekDescription
    {
        /// <summary>
        /// A 32-bit value representing the handle of the bookmark indicating the starting position from which to skip the number of rows specified in _cskip, before beginning retrieval.
        /// </summary>
        public uint _bmkOffset;

        /// <summary>
        /// A 32-bit unsigned integer containing the number of rows to skip in the rowset.
        /// </summary>
        public uint _cskip;

        /// <summary>
        /// A 32-bit unsigned integer.
        /// Note This field MUST be set to 0x00000000 and MUST be ignored.
        /// </summary>
        public uint _hRegion;

        public void FromBytes(WspBuffer buffer)
        {
            _bmkOffset = buffer.ToStruct<uint>();
            _cskip = buffer.ToStruct<uint>();
            _hRegion = buffer.ToStruct<uint>();
        }

        public void ToBytes(WspBuffer buffer)
        {
            buffer.Add(this);
        }
    }
}
