// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Wsp
{
    /// <summary>
    /// The CRowSeekNext structure contains the number of rows to skip in a CPMGetRowsIn message.
    /// </summary>
    public struct CRowSeekNext : IWspSeekDescription
    {
        /// <summary>
        /// A 32-bit unsigned integer representing the number of rows to skip in the rowset.
        /// </summary>
        public uint _cskip;

        public void FromBytes(WspBuffer buffer)
        {
            _cskip = buffer.ToStruct<uint>();
        }

        public void ToBytes(WspBuffer buffer)
        {
            buffer.Add(this);
        }
    }
}
