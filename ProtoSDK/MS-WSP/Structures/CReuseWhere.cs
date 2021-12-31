// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;

namespace Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Wsp
{
    /// <summary>
    /// The CReuseWhere restriction packet contains a WHEREID that refers to the restriction array used to construct a currently open rowset.
    /// A rowset is open as long as there is still a cursor returned by CPMCreateQueryOut that has not been freed using CPMFreeCursorIn.
    /// A server can use this information to share evaluation of a restriction across multiple queries.
    /// </summary>
    public struct CReuseWhere : IWspRestriction
    {
        /// <summary>
        /// A 32-bit unsigned integer defining a unique WHEREID for referring to the CRestrictionArray.
        /// </summary>
        public uint whereID;

        public void FromBytes(WspBuffer buffer)
        {
            throw new NotImplementedException();
        }

        public void ToBytes(WspBuffer buffer)
        {
            buffer.Add(this);
        }
    }
}
