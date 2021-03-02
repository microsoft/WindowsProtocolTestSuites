// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Protocols.TestManager.PTMService.Abstractions;
using System.Collections.Generic;
using System.Linq;

namespace Microsoft.Protocols.TestManager.PTMService.Storage
{
    internal class StoragePool : IStoragePool
    {
        private IDictionary<string, StorageNode> Pool { get; init; }

        public StoragePool(StoragePoolOptions options)
        {
            Pool = new Dictionary<string, StorageNode>();

            foreach (var kvp in options.Nodes)
            {
                Pool.Add(kvp.Key, new StorageNode(kvp.Key, kvp.Value));
            };
        }

        public IStorageNode GetNode(string name)
        {
            return Pool[name];
        }

        public IEnumerable<string> EnumerateAll()
        {
            return Pool.Keys.AsEnumerable();
        }
    }
}
