// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Protocols.TestManager.PTMService.Abstractions;
using System.Collections.Generic;
using System.Linq;
using System.IO;

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
            DirectoryCopy(extractNode.AbsolutePath, AbsolutePath, true);

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

        private static void DirectoryCopy(string sourceDirName, string destDirName, bool copySubDirs)
        {
            // Get the subdirectories for the specified directory.
            DirectoryInfo dir = new DirectoryInfo(sourceDirName);

            if (!dir.Exists)
            {
                throw new DirectoryNotFoundException(
                    "Source directory does not exist or could not be found: "
                    + sourceDirName);
            }

            DirectoryInfo[] dirs = dir.GetDirectories();

            // If the destination directory doesn't exist, create it.       
            Directory.CreateDirectory(destDirName);

            // Get the files in the directory and copy them to the new location.
            FileInfo[] files = dir.GetFiles();
            foreach (FileInfo file in files)
            {
                string tempPath = Path.Combine(destDirName, file.Name);
                file.CopyTo(tempPath, true);
            }

            // If copying subdirectories, copy them and their contents to new location.
            if (copySubDirs)
            {
                foreach (DirectoryInfo subdir in dirs)
                {
                    string tempPath = Path.Combine(destDirName, subdir.Name);
                    DirectoryCopy(subdir.FullName, tempPath, copySubDirs);
                }
            }
        }
    }
}
