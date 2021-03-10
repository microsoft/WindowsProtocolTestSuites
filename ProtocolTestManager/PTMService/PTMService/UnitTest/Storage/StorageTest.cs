// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Protocols.TestManager.PTMService.Abstractions;
using Microsoft.Protocols.TestManager.PTMService.Storage;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.IO;

namespace Microsoft.Protocols.TestManager.PTMService.UnitTest.Storage
{
    [TestClass]
    public class StorageTest
    {
        private const string testNodeName = "test";

        private string testPath;

        private IStoragePool pool;

        [TestInitialize]
        public void TestInitialize()
        {
            string tempPath = Path.GetTempPath();

            testPath = Path.Combine(tempPath, Guid.NewGuid().ToString());

            Directory.CreateDirectory(testPath);

            var services = new ServiceCollection();

            services.Configure<StoragePoolOptions>(options =>
            {
                options.Nodes = new Dictionary<string, string>
                {
                    [testNodeName] = testPath,
                };
            });

            services.AddStoragePool();

            var serviceProvider = services.BuildServiceProvider();

            pool = serviceProvider.GetService<IStoragePool>();
        }

        [TestCleanup]
        public void TestCleanup()
        {
            Directory.Delete(testPath, true);
        }

        [TestMethod]
        public void GetKnownNode()
        {
            var node = pool.GetKnownNode(testNodeName);

            Assert.IsNotNull(node);

            Assert.AreEqual(testNodeName, node.Name);
        }

        [TestMethod]
        public void OpenNode()
        {
            string subNodeName = "456";

            string subNodePath = Path.Combine(testPath, subNodeName);

            Directory.CreateDirectory(subNodePath);

            var node = pool.OpenNode(subNodePath);

            Assert.IsNotNull(node);

            Assert.AreEqual(subNodePath, node.AbsolutePath);

            Assert.AreEqual(subNodeName, node.Name);

            Assert.AreEqual(testNodeName, node.Parent.Name);
        }

        [TestMethod]
        public void CreateNode()
        {
            var node = pool.GetKnownNode(testNodeName);

            Assert.IsNotNull(node);

            Assert.AreEqual(testNodeName, node.Name);

            string nodeName = "123";

            var innerNode = node.CreateNode(nodeName);

            Assert.IsNotNull(node);

            string path = Path.Combine(node.AbsolutePath, nodeName);

            Assert.IsTrue(Directory.Exists(path));
        }

        [TestMethod]
        public void RemoveNode()
        {
            var node = pool.GetKnownNode(testNodeName);

            Assert.IsNotNull(node);

            Assert.AreEqual(testNodeName, node.Name);

            string nodeName = "123";

            var innerNode = node.CreateNode(nodeName);

            Assert.IsNotNull(node);

            string path = Path.Combine(node.AbsolutePath, nodeName);

            Assert.IsTrue(Directory.Exists(path));

            node.RemoveNode(nodeName);

            Assert.IsFalse(Directory.Exists(path));
        }

        [TestMethod]
        public void CreateFile()
        {
            var node = pool.GetKnownNode(testNodeName);

            Assert.IsNotNull(node);

            Assert.AreEqual(testNodeName, node.Name);

            using var stream = new MemoryStream();

            using var sw = new StreamWriter(stream);

            string testContent = "123";

            sw.Write(testContent);

            sw.Flush();

            stream.Seek(0, SeekOrigin.Begin);

            string testFileName = "test.txt";

            node.CreateFile(testFileName, stream);

            string path = Path.Combine(node.AbsolutePath, testFileName);

            string content = File.ReadAllText(path);

            Assert.AreEqual(testContent, content);
        }

        [TestMethod]
        public void ReadFile()
        {
            var node = pool.GetKnownNode(testNodeName);

            Assert.IsNotNull(node);

            Assert.AreEqual(testNodeName, node.Name);

            string testFileName = "test.txt";

            string path = Path.Combine(node.AbsolutePath, testFileName);

            string testContent = "123";

            File.WriteAllText(path, testContent);

            using var stream = node.ReadFile(testFileName);

            using var sr = new StreamReader(stream);

            string content = sr.ReadToEnd();

            Assert.AreEqual(testContent, content);
        }

        [TestMethod]
        public void RemoveFile()
        {
            var node = pool.GetKnownNode(testNodeName);

            Assert.IsNotNull(node);

            Assert.AreEqual(testNodeName, node.Name);

            string testFileName = "test.txt";

            string path = Path.Combine(node.AbsolutePath, testFileName);

            string testContent = "123";

            File.WriteAllText(path, testContent);

            Assert.IsTrue(File.Exists(path));

            node.RemoveFile(testFileName);

            Assert.IsFalse(File.Exists(path));
        }
    }
}
