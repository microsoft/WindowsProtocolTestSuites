// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;

namespace Microsoft.Protocols.TestTools.StackSdk.FileAccessService.WSP
{
    /// <summary>
    /// The CRowSeekAtRatio structure identifies the point at which to begin retrieval for a CPMGetRowsIn message.
    /// </summary>
    public struct CRowSeekAtRatio : IWspStructure
    {
        /// <summary>
        /// A 32-bit unsigned integer representing the numerator of the ratio of rows in the chapter at which to begin retrieval.
        /// </summary>
        public UInt32 _ulNumerator;

        /// <summary>
        /// A 32-bit unsigned integer representing the denominator of the ratio of rows in the chapter at which to begin retrieval. This MUST be greater than zero.
        /// </summary>
        public UInt32 _ulDenominator;

        /// <summary>
        /// A 32-bit unsigned integer.
        /// Note This field MUST be set to 0x00000000 and MUST be ignored.
        /// </summary>
        public UInt32 _hRegion;

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
