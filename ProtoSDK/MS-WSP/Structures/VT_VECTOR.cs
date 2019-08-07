// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;

namespace Microsoft.Protocols.TestTools.StackSdk.FileAccessService.WSP
{
    public struct VT_VECTOR<T> : IWSPObject where T : struct
    {
        /// <summary>
        /// Unsigned 32-bit integer, indicating the number of elements in the vVectorData field.
        /// </summary>
        public UInt32 vVectorElements;

        public T[] vVectorData;

        public void ToBytes(WSPBuffer buffer)
        {
            buffer.Add(vVectorElements);

            foreach (var data in vVectorData)
            {
                if (data is IWSPObject)
                {
                    buffer.Align(4);
                    ((IWSPObject)data).ToBytes(buffer);
                }
                else
                {
                    buffer.Add(data, 4);
                }
            }
        }
    }
}
