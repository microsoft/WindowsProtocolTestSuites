// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;

namespace Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Wsp
{
    public struct VT_VECTOR<T> : IWspStructure where T : struct
    {
        /// <summary>
        /// Unsigned 32-bit integer, indicating the number of elements in the vVectorData field.
        /// </summary>
        public uint vVectorElements;

        public T[] vVectorData;

        public void FromBytes(WspBuffer buffer)
        {
            throw new NotImplementedException();
        }

        public void ToBytes(WspBuffer buffer)
        {
            buffer.Add(vVectorElements);

            foreach (var data in vVectorData)
            {
                if (data is IWspStructure)
                {
                    buffer.AlignWrite(4);
                    ((IWspStructure)data).ToBytes(buffer);
                }
                else
                {
                    buffer.Add(data, 4);
                }
            }
        }
    }
}
