// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.Generic;

namespace Microsoft.Protocols.TestManager.PTMService.Abstractions
{
    /// <summary>
    /// Interface of storage pool.
    /// </summary>
    public interface IStoragePool
    {
        /// <summary>
        /// Get the storage node.
        /// </summary>
        /// <param name="name">The name of storage node.</param>
        /// <returns>The storage node.</returns>
        IStorageNode GetNode(string name);

        /// <summary>
        /// Enumerate all storage nodes.
        /// </summary>
        /// <returns>The name of all storage nodes.</returns>
        IEnumerable<string> EnumerateAll();
    }
}
