// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;

namespace Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Wsp
{
    public struct VT_LPSTR : IWspStructure
    {
        /// <summary>
        /// A 32-bit unsigned integer, indicating the size of the string field including the terminating null. 
        /// </summary>
        public uint cLen;

        /// <summary>
        /// Null-terminated string.
        /// </summary>
        public string _string;

        public void FromBytes(WspBuffer buffer)
        {
            throw new NotImplementedException();
        }

        public void ToBytes(WspBuffer buffer)
        {
            buffer.Add(cLen);

            if (cLen != 0)
            {
                buffer.AddUnicodeString(_string);
            }
        }
    }
}
