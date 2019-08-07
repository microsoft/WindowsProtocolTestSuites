// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Protocols.TestTools.StackSdk.Messages.Marshaling;
using System;

namespace Microsoft.Protocols.TestTools.StackSdk.FileAccessService.WSP
{
    public struct VT_BLOB_OBJECT : IWSPObject
    {
        /// <summary>
        /// A 32-bit unsigned integer.
        /// </summary>
        public UInt32 cbSize;

        /// <summary>
        /// MUST be of length cbSize in bytes.
        /// </summary>
        [Size("cbSize")]
        public byte[] blobData;

        public void ToBytes(WSPBuffer buffer)
        {
            buffer.Add(this);
        }
    }
}
