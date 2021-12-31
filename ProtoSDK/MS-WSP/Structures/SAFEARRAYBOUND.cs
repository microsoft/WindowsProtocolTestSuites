// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;

namespace Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Wsp
{
    /// <summary>
    /// The SAFEARRAYBOUND structure represents the bounds of one dimension of a SAFEARRAY or SAFEARRAY2.
    /// </summary>
    public struct SAFEARRAYBOUND : IWspStructure
    {
        /// <summary>
        /// A 32-bit unsigned integer, specifying the number of elements in the dimension.
        /// </summary>
        public uint cElements;

        /// <summary>
        /// A 32-bit unsigned integer, specifying the lower bound of the dimension.
        /// </summary>
        public uint lLbound;

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
