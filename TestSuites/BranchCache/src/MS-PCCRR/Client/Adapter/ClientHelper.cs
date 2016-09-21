// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Microsoft.Protocols.TestSuites.Pccrr
{
    using Microsoft.Protocols.TestTools;
    using Microsoft.Protocols.TestTools.StackSdk.BranchCache.Pccrr;

    /// <summary>
    /// This class is used to provide internal function.
    /// </summary>
    public static class ClientHelper
    {
        /// <summary>
        /// Convert BLOCK_RANGE parameters to Adapter parameters.
        /// </summary>
        /// <param name="blockRanges">The array of BLOCK_RANGE.</param>
        /// <returns>The array of BLOCKRANGE.</returns>
        public static _BLOCKRANGE[] ConvertFromStackBLOCKRANGEArray(BLOCK_RANGE[] blockRanges)
        {
            _BLOCKRANGE[] blockRang = new _BLOCKRANGE[blockRanges.Length];

            for (int i = 0; i < blockRanges.Length; i++)
            {
                blockRang[i].Index = blockRanges[i].Index;
                blockRang[i].Count = blockRanges[i].Count;
            }

            return blockRang;
        }

        /// <summary>
        /// Convert BLOCKRANGE parameters to Stack parameters.
        /// </summary>
        /// <param name="blockRanges">The array of BLOCKRANGE.</param>
        /// <returns>The array of BLOCK_RANGE.</returns>
        public static BLOCK_RANGE[] ConvertToStackBLOCKRANGEArray(_BLOCKRANGE[] blockRanges)
        {
            BLOCK_RANGE[] blockRang = new BLOCK_RANGE[blockRanges.Length];

            for (int i = 0; i < blockRanges.Length; i++)
            {
                blockRang[i].Index = blockRanges[i].Index;
                blockRang[i].Count = blockRanges[i].Count;
            }

            return blockRang;
        }
    }
}
