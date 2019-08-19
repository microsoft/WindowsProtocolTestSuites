// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;

namespace Microsoft.Protocols.TestTools.StackSdk.FileAccessService.WSP
{
    /// <summary>
    /// The CRowSeekNext structure contains the number of rows to skip in a CPMGetRowsIn message.
    /// </summary>
    public struct CRowSeekNext : IWspStructure
    {
        /// <summary>
        /// A 32-bit unsigned integer representing the number of rows to skip in the rowset.
        /// </summary>
        public UInt32 _cskip;

        public void FromBytes(WspBuffer buffer)
        {
            throw new NotImplementedException();
        }

        public void ToBytes(WspBuffer buffer)
        {
            buffer.Add(this);
        }
    }
}
