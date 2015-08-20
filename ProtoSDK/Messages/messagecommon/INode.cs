// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace Microsoft.Protocols.TestTools.StackSdk.Messages.Marshaling
{
    /// <summary>
    /// Interface for all types of nodes
    /// </summary>
    public interface INode
    {
        /// <summary>
        /// Count of its children nodes
        /// </summary>
        int ChildCount { get; }

        /// <summary>
        /// Token type
        /// </summary>
        TokenType Type { get; }

        /// <summary>
        /// Node text
        /// </summary>
        string Text { get; }

        /// <summary>
        /// Add a child node
        /// </summary>
        /// <param name="child">A child node</param>
        void AddChild(INode child);

        /// <summary>
        /// Get a child node by index
        /// </summary>
        /// <param name="childIndex">The child node index</param>
        /// <returns>The child node</returns>
        INode GetChild(int childIndex);

        /// <summary>
        /// Dump the tree to a string expression
        /// </summary>
        /// <returns>The string expression</returns>
        string DumpTree();

        /// <summary>
        /// A String that represents the current Object.
        /// </summary>
        /// <returns>The string representation</returns>
        string ToString();
    }
}
