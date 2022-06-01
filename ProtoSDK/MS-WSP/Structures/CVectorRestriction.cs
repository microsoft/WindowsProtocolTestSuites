// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;

namespace Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Wsp
{
    public enum CVectorRestriction_ulRankMethod_Values : uint
    {
        /// <summary>
        /// Use the minimum algorithm, as specified in [SALTON].
        /// </summary>
        VECTOR_RANK_MIN = 0x00000000,

        /// <summary>
        /// Use the maximum algorithm, as specified in [SALTON].
        /// </summary>
        VECTOR_RANK_MAX = 0x00000001,

        /// <summary>
        /// Use the inner product algorithm, as specified in [SALTON].
        /// </summary>
        VECTOR_RANK_INNER = 0x00000002,

        /// <summary>
        /// Use the Dice coefficient algorithm, as specified in [SALTON].
        /// </summary>
        VECTOR_RANK_DICE = 0x00000003,

        /// <summary>
        /// Use the Jaccard coefficient algorithm, as specified in [SALTON].
        /// </summary>
        VECTOR_RANK_JACCARD = 0x00000004,
    }

    /// <summary>
    /// The CVectorRestriction structure contains a weighted OR operation over restriction nodes.
    /// Vector restrictions represent queries using the full text vector space model of ranking (see [SALTON] for details).
    /// In addition to the OR operation, they also compute a rank based on the ranking algorithm.
    /// </summary>
    public struct CVectorRestriction : IWspRestriction
    {
        /// <summary>
        /// A CNodeRestriction command tree upon which a ranked OR operation is to be performed.
        /// </summary>
        public CNodeRestriction _pres;

        /// <summary>
        /// A 32-bit unsigned integer specifying a ranking algorithm.
        /// </summary>
        public CVectorRestriction_ulRankMethod_Values _ulRankMethod;

        public void FromBytes(WspBuffer buffer)
        {
            throw new NotImplementedException();
        }

        public void ToBytes(WspBuffer buffer)
        {
            _pres.ToBytes(buffer);

            buffer.Add(_ulRankMethod);
        }
    }
}
