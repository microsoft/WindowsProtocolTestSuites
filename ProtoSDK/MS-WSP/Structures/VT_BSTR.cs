// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;

namespace Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Wsp
{
    public struct VT_BSTR : IWspStructure
    {
        #region Fields
        /// <summary>
        /// A 32-bit unsigned integer.
        /// </summary>
        public uint cbSize;

        /// <summary>
        /// MUST be of length cbSize in bytes.
        /// </summary>
        public byte[] blobData;
        #endregion

        #region Constructors
        public VT_BSTR(string val)
        {
            var buffer = new WspBuffer();

            buffer.AddUnicodeString(val);

            cbSize = (uint)buffer.WriteOffset;

            blobData = buffer.GetBytes();
        }
        #endregion

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
