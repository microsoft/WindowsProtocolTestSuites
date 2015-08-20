// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;

using Microsoft.Protocols.TestTools.StackSdk.Messages.Marshaling;

namespace Microsoft.Protocols.TestTools.StackSdk.Dtyp
{
    /// <summary>
    /// Represents an object tree for AccessCheck algorithm.
    /// </summary>
    public class AccessCheckObjectTree
    {
        private Guid? root;

        //Only include the node which has descendent node.
        private Dictionary<Guid, List<Guid>> childNodeMap;

        //Only include the node which has parent node.
        private Dictionary<Guid, Guid> parentNodeMap;

        //Include all the nodes in the tree.
        private Dictionary<Guid, uint> dataMap;

        /// <summary>
        /// The root of the tree.
        /// </summary>
        public Guid? Root
        {
            get
            {
                return root;
            }
        }


        /// <summary>
        /// Initializes a new instance of the Tree class.
        /// </summary>
        public AccessCheckObjectTree()
        {
            root = null;
            childNodeMap = new Dictionary<Guid, List<Guid>>();
            parentNodeMap = new Dictionary<Guid, Guid>();
            dataMap = new Dictionary<Guid, uint>();
        }


        /// <summary>
        /// Add a new node into the tree. 
        /// </summary>
        /// <param name="parentNodeGuid">The GUID of the parent node. If the new node is the root, it can be set to null.</param>
        /// <param name="newNodeGuid">The GUID of the new node to be inserted.</param>
        /// <exception cref="ArgumentException">
        /// Thrown when parent node or the new node is invalid.
        /// </exception>
        public void AddTreeNode(Guid? parentNodeGuid, Guid newNodeGuid)
        {
            if (parentNodeGuid == null)
            {
                if (root != null)
                {
                    parentNodeMap.Add(root.Value, newNodeGuid);
                    childNodeMap.Add(newNodeGuid, new List<Guid>());
                    childNodeMap[newNodeGuid].Add(root.Value);
                }
                root = newNodeGuid;
            }
            else
            {
                if (!dataMap.ContainsKey(parentNodeGuid.Value))
                {
                    throw new ArgumentException("Parent node doesn't exist.", "parentNodeGuid");
                }
                else if (dataMap.ContainsKey(newNodeGuid))
                {
                    throw new ArgumentException("The new node has existed.", "newNodeGuid");
                }
                else
                {
                    parentNodeMap.Add(newNodeGuid, parentNodeGuid.Value);

                    if (!childNodeMap.ContainsKey(parentNodeGuid.Value))
                    {
                        childNodeMap.Add(parentNodeGuid.Value, new List<Guid>());
                    }
                    childNodeMap[parentNodeGuid.Value].Add(newNodeGuid);
                }
            }

            dataMap.Add(newNodeGuid, 0);
        }

        /// <summary>
        /// Clear all the nodes in the tree.
        /// </summary>
        public void ClearAll()
        {
            root = null;
            childNodeMap.Clear();
            parentNodeMap.Clear();
            dataMap.Clear();
        }

        /// <summary>
        /// Set a tree node data.
        /// </summary>
        /// <param name="nodeGuid">Guid of the node to be changed.</param>
        /// <param name="newNodeData">New node data.</param>
        /// <exception cref="ArgumentException">
        /// Thrown when the given node doesn't exist.
        /// </exception>
        internal void SetTreeNodeData(Guid nodeGuid, uint newNodeData)
        {
            if (!dataMap.ContainsKey(nodeGuid))
            {
                throw new ArgumentException("The given node doesn't exist.", "nodeGuid");
            }

            dataMap[nodeGuid] = newNodeData;
        }


        /// <summary>
        /// Get a tree node data.
        /// </summary>
        /// <param name="nodeGuid">Guid of the node whose data is to be retrieved.</param>
        /// <returns>Node data.</returns>
        /// <exception cref="ArgumentException">
        /// Thrown when the given node doesn't exist.
        /// </exception>
        internal uint GetTreeNodeData(Guid nodeGuid)
        {
            if (!dataMap.ContainsKey(nodeGuid))
            {
                throw new ArgumentException("The given node doesn't exist.", "nodeGuid");
            }

            return dataMap[nodeGuid];
        }


        /// <summary>
        /// Determines whether the tree contains the specific node.
        /// </summary>
        /// <param name="nodeGuid">The given node to be determined.</param>
        /// <returns>Return true if the given node is in the tree; otherwise, false.</returns>
        public bool ContainsTreeNode(Guid nodeGuid)
        {
            return dataMap.ContainsKey(nodeGuid);
        }


        /// <summary>
        /// Get the descendent nodes of the given subtree root node.
        /// </summary>
        /// <param name="nodeGuid">Node guid.</param>
        /// <returns>The guid array of the descendent nodes.</returns>
        /// <exception cref="ArgumentException">
        /// Thrown when the given node doesn't exist.
        /// </exception>
        internal Guid[] GetDescendentNodes(Guid nodeGuid)
        {
            if (!dataMap.ContainsKey(nodeGuid))
            {
                throw new ArgumentException("The given node doesn't exist.", "nodeGuid");
            }

            if (!childNodeMap.ContainsKey(nodeGuid))
            {
                return new Guid[] { };
            }

            List<Guid> tmpList = new List<Guid>();
            Queue<Guid> pendingNodeQueue = new Queue<Guid>();
            pendingNodeQueue.Enqueue(nodeGuid);

            Guid currentNode;
            while (pendingNodeQueue.Count > 0)
            {
                currentNode = pendingNodeQueue.Dequeue();

                if (childNodeMap.ContainsKey(currentNode))
                {
                    foreach (Guid item in childNodeMap[currentNode])
                    {
                        pendingNodeQueue.Enqueue(item);
                        tmpList.Add(item);
                    }
                }
            }

            return tmpList.ToArray();
        }


        /// <summary>
        /// Get the ancestor nodes of the given tree node.
        /// </summary>
        /// <param name="nodeGuid">Tree node guid.</param>
        /// <returns>
        /// Return the guid array of the ancestor nodes, whose order is from the parent node to root node.
        /// </returns>
        /// <exception cref="ArgumentException">
        /// Thrown when the given node doesn't exist.
        /// </exception>
        internal Guid[] GetAncestorNodes(Guid nodeGuid)
        {
            if (!dataMap.ContainsKey(nodeGuid))
            {
                throw new ArgumentException("The given node doesn't exist.", "nodeGuid");
            }

            if (ObjectUtility.DeepCompare(root.Value, nodeGuid))
            {
                return new Guid[] { };
            }

            List<Guid> tmpList = new List<Guid>();

            Guid currentNode = nodeGuid;
            while (parentNodeMap.ContainsKey(currentNode))
            {
                currentNode = parentNodeMap[currentNode];
                tmpList.Add(currentNode);
            }

            return tmpList.ToArray();
        }


        /// <summary>
        /// Get the sibling nodes of the given tree node.
        /// </summary>
        /// <param name="nodeGuid">Tree node guid.</param>
        /// <returns>
        /// Return the guid array of the sibling nodes.
        /// </returns>
        /// <exception cref="ArgumentException">
        /// Thrown when the given node doesn't exist.
        /// </exception>
        internal Guid[] GetSiblingNodes(Guid nodeGuid)
        {
            if (!dataMap.ContainsKey(nodeGuid))
            {
                throw new ArgumentException("The given node doesn't exist.", "nodeGuid");
            }

            if (!parentNodeMap.ContainsKey(nodeGuid))
            {
                return new Guid[] { };
            }

            List<Guid> tmpList = new List<Guid>();
            Guid parent = parentNodeMap[nodeGuid];
            foreach (Guid currentNode in childNodeMap[parent])
            {
                if (ObjectUtility.DeepCompare(currentNode, nodeGuid))
                {
                    continue;
                }
                tmpList.Add(currentNode);
            }

            return tmpList.ToArray();
        }
    }
}
