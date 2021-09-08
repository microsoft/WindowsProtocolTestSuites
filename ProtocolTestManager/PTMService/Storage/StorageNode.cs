// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Protocols.TestManager.PTMService.Abstractions;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using Microsoft.Protocols.TestManager.Kernel;

namespace Microsoft.Protocols.TestManager.PTMService.Storage
{
    internal class StorageNode : IStorageNode
    {
        public string Name { get; private init; }

        public string AbsolutePath { get; private init; }

        public IStorageNode Parent { get; private init; }

        private IDictionary<string, IStorageNode> Nodes { get; init; }

        private StorageNode()
        {
            Nodes = new Dictionary<string, IStorageNode>();
        }

        public StorageNode(string name, string path)
            : this()
        {
            Parent = null;

            Name = name;

            AbsolutePath = path;
        }

        public StorageNode(StorageNode parent, string name)
            : this()
        {
            Parent = parent;

            Name = name;

            AbsolutePath = Path.Combine(parent.AbsolutePath, name);
        }

        public IStorageNode CreateNode(string name)
        {
            string path = Path.Combine(AbsolutePath, name);

            Directory.CreateDirectory(path);

            var result = new StorageNode(this, name);

            Nodes.Add(name, result);

            return result;
        }

        public bool NodeExists(string name)
        {
            string path = Path.Combine(AbsolutePath, name);
            
            return Directory.Exists(path);
        }

        public void RemoveNode(string name)
        {
            string path = Path.Combine(AbsolutePath, name);

            Directory.Delete(path, true);

            Nodes.Remove(name);
        }

        public IStorageNode GetNode(string name)
        {
            if (!Nodes.ContainsKey(name))
            {
                Nodes.Add(name, new StorageNode(this, name));
            }

            return Nodes[name];
        }

        public IEnumerable<string> GetNodes()
        {
            return Nodes.Values.Select(n => n.AbsolutePath);
        }

        public void RemoveFile(string name)
        {
            string path = Path.Combine(AbsolutePath, name);

            File.Delete(path);
        }

        Stream IStorageNode.ReadFile(string name)
        {
            string path = Path.Combine(AbsolutePath, name);

            using var fs = File.OpenRead(path);

            var stream = new MemoryStream();

            fs.CopyTo(stream);

            stream.Seek(0, SeekOrigin.Begin);

            return stream;
        }

        public void CreateFile(string name, Stream content)
        {
            string path = Path.Combine(AbsolutePath, name);

            using var fs = File.Open(path, FileMode.CreateNew);

            content.CopyTo(fs);

            fs.Close();
        }

        public void CopyFromNode(IStorageNode extractNode, bool deleteSource = false)
        {
            Utility.DirectoryCopy(extractNode.AbsolutePath, AbsolutePath, true);

            try
            {
                if (deleteSource)
                {
                    Directory.Delete(extractNode.AbsolutePath, true);
                }
            }
            catch
            {
            }
        }

        public IEnumerable<string> GetFiles()
        {
            return Directory.EnumerateFiles(AbsolutePath);
        }

        public void DeleteNode()
        {
            Directory.Delete(AbsolutePath, true);
        }        
    }
}
