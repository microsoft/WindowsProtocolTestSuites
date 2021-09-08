// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Extensions.Options;
using Microsoft.Protocols.TestManager.PTMService.Abstractions;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Microsoft.Protocols.TestManager.PTMService.Storage
{
    internal class StoragePool : IStoragePool
    {
        private IDictionary<string, StorageNode> Pool { get; init; }

        public StoragePool(IOptions<StoragePoolOptions> options)
        {
            Pool = new Dictionary<string, StorageNode>();

            foreach (var kvp in options.Value.Nodes)
            {
                Pool.Add(kvp.Key, new StorageNode(kvp.Key, kvp.Value));
            };
        }

        public IStorageNode GetKnownNode(string name)
        {
            return Pool[name];
        }

        public IStorageNode OpenNode(string path)
        {
            var pathComponents = GetPathComponents(path);

            IStorageNode ancestorNode = Pool.Values.FirstOrDefault(node =>
            {
                var nodePathComponents = GetPathComponents(node.AbsolutePath);

                return Enumerable.SequenceEqual(pathComponents.Take(nodePathComponents.Count()), nodePathComponents);
            });

            if (ancestorNode == null)
            {
                return new StorageNode(string.Empty, path);
            }
            else
            {
                var nodePathComponents = GetPathComponents(ancestorNode.AbsolutePath);

                var relativeComponents = pathComponents.Skip(nodePathComponents.Count());

                var result = relativeComponents.Aggregate(ancestorNode, (node, component) => node.GetNode(component));

                return result;
            }
        }

        public IEnumerable<string> EnumerateAll()
        {
            return Pool.Keys.AsEnumerable();
        }

        private static IEnumerable<string> GetPathComponents(string path)
        {
            var pathComponents = new Stack<string>();

            var current = new DirectoryInfo(path);

            while (current != null)
            {
                pathComponents.Push(current.Name);

                current = current.Parent;
            }

            return pathComponents.AsEnumerable();
        }
    }
}
