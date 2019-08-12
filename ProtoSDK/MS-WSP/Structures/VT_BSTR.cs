// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Protocols.TestTools.StackSdk.Messages.Marshaling;
using System;

namespace Microsoft.Protocols.TestTools.StackSdk.FileAccessService.WSP
{
    public struct VT_BSTR : IWSPObject
    {
        #region Fields
        /// <summary>
        /// A 32-bit unsigned integer.
        /// </summary>
        public UInt32 cbSize;

        /// <summary>
        /// MUST be of length cbSize in bytes.
        /// </summary>
        [Size("cbSize")]
        public byte[] blobData;
        #endregion

        #region Constructors
        public VT_BSTR(string val)
        {
            var buffer = new WSPBuffer();

            buffer.AddUnicodeString(val);

            cbSize = (UInt32)buffer.Offset;

            blobData = buffer.GetBytes();
        }
        #endregion

        public void ToBytes(WSPBuffer buffer)
        {
            buffer.Add(this);
        }
    }
}
