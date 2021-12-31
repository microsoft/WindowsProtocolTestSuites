// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;

namespace Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Wsp
{
    /// <summary>
    /// The CNodeRestriction structure contains an array of command tree restriction nodes for constraining the results of a query.
    /// </summary>
    public struct CNodeRestriction : IWspRestriction
    {
        /// <summary>
        /// A 32-bit unsigned integer specifying the number of CRestriction structures contained in the _paNode field.
        /// </summary>
        public uint _cNode;

        /// <summary>
        /// An array of CRestriction structures.
        /// Structures in the array MUST be separated by 0 to 3 padding bytes such that each structure begins at an offset that is a multiple of 4 bytes from the beginning of the message that contains this array.
        /// If padding bytes are present, the value they contain is arbitrary. The content of the padding bytes MUST be ignored by the receiver.
        /// </summary>
        public CRestriction[] _paNode;

        public void FromBytes(WspBuffer buffer)
        {
            throw new NotImplementedException();
        }

        public void ToBytes(WspBuffer buffer)
        {
            buffer.Add(_cNode);

            foreach (var node in _paNode)
            {
                buffer.AlignWrite(4);
                node.ToBytes(buffer);
            }
        }
    }
}
