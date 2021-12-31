// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;

namespace Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Wsp
{
    public struct VT_BLOB : IWspStructure
    {
        /// <summary>
        /// A 32-bit unsigned integer.
        /// </summary>
        public uint cbSize;

        /// <summary>
        /// MUST be of length cbSize in bytes.
        /// </summary>
        public byte[] blobData;

        public void FromBytes(WspBuffer buffer)
        {
            throw new NotImplementedException();
        }

        public void ToBytes(WspBuffer buffer)
        {
            buffer.Add(cbSize);

            buffer.AddRange(blobData);
        }
    }
}
