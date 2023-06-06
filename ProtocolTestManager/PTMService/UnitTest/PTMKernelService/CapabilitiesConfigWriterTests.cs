// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Protocols.TestManager.Common;
using Microsoft.Protocols.TestManager.Kernel;
using Microsoft.Protocols.TestManager.PTMService.Abstractions.Kernel;
using Microsoft.Protocols.TestManager.PTMService.PTMKernelService;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Nodes;
using System.Threading.Tasks;

namespace Microsoft.Protocols.TestManager.PTMService.UnitTest.PTMKernelService
{
    [TestClass]
    public class CapabilitiesConfigWriterTests
    {
        [TestInitialize]
        public void TestInitialize()
        {
        }

        [TestCleanup]
        public void TestCleanup()
        {
        }

        [TestMethod]
        public void Create_WithValidTestSuiteId_ReturnsJsonNode()
        {
            var testSuiteId = 123;
            var testSuiteName = "TestSuiteName";
            var testSuiteVersion = "4.29.22.1";
            var testCaseInfo = new TestCaseInfo[] 
            { 
                new TestCaseInfo() { Name = "test 1" },
                new TestCaseInfo() { Name = "test 2" },
                new TestCaseInfo() { Name = "test 3" }
            };

            // Arrange
            var testSuiteMock = new Mock<ITestSuite>();
            testSuiteMock.Setup(t => t.GetTestCases(It.IsAny<string>()))
                                        .Returns(() => testCaseInfo);
            testSuiteMock.Setup(t => t.Name).Returns(testSuiteName);
            testSuiteMock.Setup(t => t.Version).Returns(testSuiteVersion);
            testSuiteMock.Setup(t => t.Id).Returns(testSuiteId);
            testSuiteMock.Setup(t => t.Removed).Returns(false);

            var ptmKernelServiceMock = new Mock<IPTMKernelService>();
            ptmKernelServiceMock.Setup(p => p.GetTestSuite(It.IsAny<int>()))
                                .Returns(() => testSuiteMock.Object);

            // Act
            var json = CapabilitiesConfigWriter.Create(ptmKernelServiceMock.Object, testSuiteId);

            // Assert
            Assert.IsNotNull(json);
            Assert.IsNotNull(json["capabilities"]["metadata"]["testsuite"]);
            Assert.AreEqual(testSuiteName, json["capabilities"]["metadata"]["testsuite"].ToString());
            Assert.IsNotNull(json["capabilities"]["metadata"]["version"]);
            Assert.AreEqual(testSuiteVersion, json["capabilities"]["metadata"]["version"].ToString());
            Assert.IsNotNull(json["capabilities"]["groups"]);
            Assert.AreEqual(new JsonArray().ToJsonString(), json["capabilities"]["groups"].ToJsonString());

            var testCaseNodes = json["capabilities"]["testcases"] as JsonArray;
            Assert.IsNotNull(testCaseNodes);
            for (int i = 0; i < testCaseNodes.Count; i++)
            {
                var node = testCaseNodes[i];
                Assert.IsNotNull(node);
                Assert.IsNotNull(node["name"]);
                Assert.IsNotNull(node["categories"]);
                Assert.AreEqual(new JsonArray().ToJsonString(), node["categories"].ToJsonString());
                Assert.IsTrue(node["name"].GetValue<string>() == testCaseInfo[i].Name);
            }
        }

        [TestMethod]
        public void Create_WithRemovedTestSuite_ThrowsInvalidOperationException()
        {
            // Arrange
            var testSuiteId = 123;
            var testSuiteName = "TestSuiteName";
            var testSuiteVersion = "4.29.22.1";
            var testCaseInfo = new TestCaseInfo[]
            {
                new TestCaseInfo() { Name = "test 1" },
                new TestCaseInfo() { Name = "test 2" },
                new TestCaseInfo() { Name = "test 3" }
            };

            var testSuiteMock = new Mock<ITestSuite>();
            testSuiteMock.Setup(t => t.GetTestCases(It.IsAny<string>()))
                                        .Returns(() => testCaseInfo);
            testSuiteMock.Setup(t => t.Name).Returns(testSuiteName);
            testSuiteMock.Setup(t => t.Version).Returns(testSuiteVersion);
            testSuiteMock.Setup(t => t.Id).Returns(testSuiteId);
            testSuiteMock.Setup(t => t.Removed).Returns(true);

            var ptmKernelServiceMock = new Mock<IPTMKernelService>();
            ptmKernelServiceMock.Setup(p => p.GetTestSuite(It.IsAny<int>()))
                                .Returns(() => testSuiteMock.Object);

            // Act + Assert
            var ex = Assert.ThrowsException<InvalidOperationException>(() =>
                                    CapabilitiesConfigWriter.Create(ptmKernelServiceMock.Object, testSuiteId));
            Assert.AreEqual(CapabilitiesConfigWriter.RemovedOrNullTestSuiteMessage(testSuiteId).ToLowerInvariant(), ex.Message.ToLowerInvariant());
        }

        [TestMethod]
        public void Create_WithNullTestSuite_ThrowsInvalidOperationException()
        {
            // Arrange
            var testSuiteId = 123;

            var ptmKernelServiceMock = new Mock<IPTMKernelService>();
            ptmKernelServiceMock.Setup(p => p.GetTestSuite(It.IsAny<int>()))
                                .Returns(() => null);

            // Act + Assert
            var ex = Assert.ThrowsException<InvalidOperationException>(() =>
                                    CapabilitiesConfigWriter.Create(ptmKernelServiceMock.Object, testSuiteId));
            Assert.AreEqual(CapabilitiesConfigWriter.RemovedOrNullTestSuiteMessage(testSuiteId).ToLowerInvariant(), ex.Message.ToLowerInvariant());
        }
    }
}
