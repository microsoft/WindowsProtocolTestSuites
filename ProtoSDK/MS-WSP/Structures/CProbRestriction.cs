// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;

namespace Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Wsp
{
    /// <summary>
    /// A CProbRestriction structure contains parameters for probabilistic ranking.
    /// </summary>
    public struct CProbRestriction : IWspRestriction
    {
        /// <summary>
        /// A CFullPropSpec structure, indicating which property to use for probabilistic ranking or the columns' group full property specification (which corresponds to _groupPid field in the CColumnGroup structure).
        /// In the latter case, CFullPropSpec MUST have the _guidPropSet field set to zero, the ulKind field set to PRSPEC_LPWSTR and the Property Name field set to the name of the referenced group property.
        /// </summary>
        public CFullPropSpec _Property;

        /// <summary>
        /// An IEEE 32-bit floating point number [IEEE754] that indicates parameter k1 in formula [1], specified below.
        /// </summary>
        public float _flK1;

        /// <summary>
        /// An IEEE 32-bit floating point number.
        /// Note MUST be set to 0.0.
        /// </summary>
        public float _flK2;

        /// <summary>
        /// An IEEE 32-bit floating point number that indicates parameter k3 in formula [1].
        /// </summary>
        public float _flK3;

        /// <summary>
        /// An IEEE 32-bit floating point number that indicates parameter b in formula [1] below.
        /// </summary>
        public float _flB;

        /// <summary>
        /// A 32-bit unsigned integer specifying the count of relevant documents.
        /// </summary>
        public uint _cFeedbackDoc;

        /// <summary>
        /// A 32-bit unsigned integer.
        /// Note Reserved.MUST be set to 0x00000000.
        /// </summary>
        public uint _ProbQueryPid;

        public void FromBytes(WspBuffer buffer)
        {
            throw new NotImplementedException();
        }

        public void ToBytes(WspBuffer buffer)
        {
            _Property.ToBytes(buffer);

            buffer.Add(_flK1, 4);

            buffer.Add(_flK2);

            buffer.Add(_flK3);

            buffer.Add(_flB);

            buffer.Add(_cFeedbackDoc);

            buffer.Add(_ProbQueryPid);
        }
    }
}
