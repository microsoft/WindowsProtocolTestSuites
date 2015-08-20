// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace Microsoft.Protocols.TestTools.StackSdk.Messages.Marshaling
{
    /// <summary>
    /// The Base Node
    /// </summary>
    public abstract class BaseNode : INode
    {
        private IList<INode> children = new List<INode>();

        /// <summary>
        /// Constructor
        /// </summary>
        protected BaseNode()
        {
        }

        /// <summary>
        /// Children count of the base node
        /// </summary>
        public virtual int ChildCount
        {
            get
            {
                if (children == null)
                {
                    return 0;
                }
                return children.Count;
            }
        }

        /// <summary>
        /// Get a child node by index
        /// </summary>
        /// <param name="childIndex">The specific index</param>
        /// <returns>The child node</returns>
        public virtual INode GetChild(int childIndex)
        {
            if (children == null || childIndex >= children.Count)
            {
                return null;
            }

            return children[childIndex];
        }

        /// <summary>
        /// Token type
        /// </summary>
        public abstract TokenType Type
        {
            get;
        }

        /// <summary>
        /// Node text
        /// </summary>
        public abstract string Text
        {
            get;
        }

        /// <summary>
        /// Add a child node
        /// </summary>
        /// <param name="child">The child node</param>
        public virtual void AddChild(INode child)
        {
            if (child == null)
            {
                return;
            }

            children.Add(child);
        }

        /// <summary>
        /// A String that represents the current Object.
        /// </summary>
        /// <returns>The string representation</returns>
        public override abstract string ToString();

        /// <summary>
        /// Convert the tree (based on the base node) to a string expression
        /// </summary>
        /// <returns>The tree's string expression</returns>
        public virtual string DumpTree()
        {
            if (children == null || children.Count == 0)
            {
                return this.ToString();
            }

            StringBuilder buf = new StringBuilder();
            buf.Append("(");
            buf.Append(this.ToString());
            buf.Append(' ');

            for (int i = 0; children != null && i < children.Count; i++)
            {
                BaseNode node = (BaseNode)children[i];
                if (i > 0)
                {
                    buf.Append(' ');
                }
                buf.Append(node.DumpTree());
            }

            buf.Append(")");

            return buf.ToString();
        }
    }
}
