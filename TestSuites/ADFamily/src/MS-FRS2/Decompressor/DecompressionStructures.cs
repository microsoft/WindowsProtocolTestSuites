// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Text;

namespace Decompressor
{
    /// <summary>
    /// PrefixCodeNode structure.
    /// </summary>
    public class PrefixCodeNode
    {
        /// <summary>
        /// Symbol
        /// </summary>
        public UInt16 symbol;

        /// <summary>
        /// bool variable indicates whether the Leaf exists or not.
        /// </summary>
        public bool leaf;

        /// <summary>
        /// Left and Right Child nodes can exist for each node.
        /// </summary>
        public PrefixCodeNode[] Child;

        /// <summary>
        /// ctor which initializes the child nodes.
        /// </summary>
        public PrefixCodeNode()
        {
            Child = new PrefixCodeNode[2];
            Child[0] = null;
            Child[1] = null;
        }
    }

    /// <summary>
    /// PrefixCodeSymbol structure
    /// </summary>
    public struct PrefixCodeSymbol
    {
        /// <summary>
        /// Symbol value.
        /// </summary>
        public UInt16 symbol;

        /// <summary>
        /// Length
        /// </summary>
        public UInt16 length;
    }

    /// <summary>
    /// BITSTRING structure.
    /// </summary>
    public struct BITSTRING
    {
        /// <summary>
        /// source array
        /// </summary>
        public byte[] source;

        /// <summary>
        /// Index
        /// </summary>
        public UInt32 index;

        /// <summary>
        /// Mask value
        /// </summary>
        public UInt32 mask;

        /// <summary>
        /// bits to mask
        /// </summary>
        public Int32 bits;
    }
}
