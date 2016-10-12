// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Microsoft.Protocols.TestSuites.Pccrr
{
    using Microsoft.Protocols.TestTools;
    using Microsoft.Protocols.TestTools.StackSdk.BranchCache.Pccrr;

    /// <summary>
    /// This class is used to provide internal function.
    /// </summary>
    public static class Helper
    {
        /// <summary>
        /// Convert CryptoAlgoId_Values parameters to Adapter parameters.
        /// </summary>
        /// <param name="cryptoAlgoId">The CryptoAlgoId_Values value.</param>
        /// <returns>The CryptoAlgoIdValues value.</returns>
        public static CryptoAlgoIdValues ConvertFromStackCryptoAlgoIdValues(CryptoAlgoId_Values cryptoAlgoId)
        {
            CryptoAlgoIdValues crypAlgoId;

            crypAlgoId = (CryptoAlgoIdValues)cryptoAlgoId;

            return crypAlgoId;
        }

        /// <summary>
        /// Convert CryptoAlgoIdValues parameters to Stack parameters.
        /// </summary>
        /// <param name="cryptoAlgoId">The CryptoAlgoIdValues value.</param>
        /// <returns>The CryptoAlgoId_Values value.</returns>
        public static CryptoAlgoId_Values ConvertToStackCryptoAlgoIdValues(CryptoAlgoIdValues cryptoAlgoId)
        {
            CryptoAlgoId_Values crypAlgoId;

            crypAlgoId = (CryptoAlgoId_Values)cryptoAlgoId;

            return crypAlgoId;
        }

        /// <summary>
        /// Convert BLOCK_RANGE parameters to Adapter parameters.
        /// </summary>
        /// <param name="blockRanges">The array of BLOCK_RANGE.</param>
        /// <returns>The array of BLOCKRANGE.</returns>
        public static BLOCKRANGE[] ConvertFromStackBLOCKRANGEArray(BLOCK_RANGE[] blockRanges)
        {
            BLOCKRANGE[] blockRang = new BLOCKRANGE[blockRanges.Length];

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
        public static BLOCK_RANGE[] ConvertToStackBLOCKRANGEArray(BLOCKRANGE[] blockRanges)
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
