// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Protocols.TestManager.Kernel;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace Microsoft.Protocols.TestManager.PTMService.UnitTest.Kernel
{
    [TestClass]
    public class TestEngineTests
    {
        [TestMethod]
        public void GetTestLogs_InterleavedPipeConnections_ReturnsLogsForRequestedFullName()
        {
            const string firstTest = "Tests.First.SharedMethod";
            const string secondTest = "Tests.Second.SharedMethod";

            var engine = new TestEngine("dotnet");
            engine.InitializeLogger(new List<TestCase>
            {
                new TestCase { Name = "SharedMethod", FullName = firstTest },
                new TestCase { Name = "SharedMethod", FullName = secondTest },
            });

            var parseLogMessage = typeof(TestEngine).GetMethod(
                "ParseLogMessage",
                BindingFlags.Instance | BindingFlags.NonPublic);

            Assert.IsNotNull(parseLogMessage);

            var firstConnection = Guid.NewGuid();
            var secondConnection = Guid.NewGuid();

            Parse(parseLogMessage, engine, firstConnection, $"[TestInProgress] {firstTest}");
            Parse(parseLogMessage, engine, secondConnection, $"[TestInProgress] {secondTest}");
            Parse(parseLogMessage, engine, firstConnection, "first-only-line");
            Parse(parseLogMessage, engine, secondConnection, "second-only-line");

            string firstLogs = engine.GetTestLogs(firstTest);
            string secondLogs = engine.GetTestLogs(secondTest);

            StringAssert.Contains(firstLogs, "first-only-line");
            Assert.IsFalse(firstLogs.Contains("second-only-line"));
            StringAssert.Contains(secondLogs, "second-only-line");
            Assert.IsFalse(secondLogs.Contains("first-only-line"));
        }

        private static void Parse(MethodInfo method, TestEngine engine, Guid connectionId, string message)
        {
            method.Invoke(engine, new object[] { connectionId, message });
        }
    }
}
