// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;

namespace Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Wsp
{
    public struct SAFEARRAY<T> : IWspStructure where T : struct
    {
        /// <summary>
        /// Unsigned 16-bit integer, indicating the number of dimensions of the multidimensional array.
        /// </summary>
        public ushort cDims;

        /// <summary>
        /// A 16-bit bitfield. The values represent features defined by upper-layer applications and MUST be ignored.
        /// </summary>
        public ushort fFeatures;

        /// <summary>
        /// A 32-bit unsigned integer specifying the size of each element of the array.
        /// </summary>
        public uint cbElements;

        /// <summary>
        /// An array that contains one SAFEARRAYBOUND structure per dimension in the SAFEARRAY.
        /// This array has the left-most dimension first and the right-most dimension last.
        /// </summary>
        public SAFEARRAYBOUND[] Rgsabound;

        public T[] vData;

        public void FromBytes(WspBuffer buffer)
        {
            throw new NotImplementedException();
        }

        public void ToBytes(WspBuffer buffer)
        {
            buffer.Add(cDims);

            buffer.Add(fFeatures);

            buffer.Add(cbElements);

            foreach (var bound in Rgsabound)
            {
                bound.ToBytes(buffer);
            }

            foreach (var data in vData)
            {
                if (data is IWspStructure)
                {
                    buffer.AlignWrite(4);
                    (data as IWspStructure).ToBytes(buffer);
                }
                else
                {
                    buffer.Add(data, 4);
                }
            }
        }
    }
}
