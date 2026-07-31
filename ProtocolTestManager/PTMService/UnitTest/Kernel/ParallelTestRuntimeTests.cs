// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Protocols.TestManager.Kernel;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.IO.Pipes;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace Microsoft.Protocols.TestManager.PTMService.UnitTest.Kernel
{
    [TestClass]
    [DoNotParallelize]
    public class ParallelTestRuntimeTests
    {
        [TestMethod]
        public void ChangeStatus_ConcurrentRunningTests_RemainRunning()
        {
            var first = CreateTest("Namespace.Class.First");
            var second = CreateTest("Namespace.Class.Second");
            var group = new GroupByOutcome { ConcurrentExecution = true };
            group.SetTestCaseList(new List<TestCase> { first, second });

            Parallel.Invoke(
                () => group.ChangeStatus(first.FullName, TestCaseStatus.Running),
                () => group.ChangeStatus(second.FullName, TestCaseStatus.Running));

            Assert.AreEqual(TestCaseStatus.Running, first.Status);
            Assert.AreEqual(TestCaseStatus.Running, second.Status);
        }

        [TestMethod]
        public void ChangeStatus_SequentialExecution_DemotesPreviousRunningTest()
        {
            var first = CreateTest("Namespace.Class.First");
            var second = CreateTest("Namespace.Class.Second");
            var group = new GroupByOutcome();
            group.SetTestCaseList(new List<TestCase> { first, second });

            group.ChangeStatus(first.FullName, TestCaseStatus.Running);
            group.ChangeStatus(second.FullName, TestCaseStatus.Running);

            Assert.AreEqual(TestCaseStatus.Waiting, first.Status, "The previously running case falls back to Waiting when execution is sequential.");
            Assert.AreEqual(TestCaseStatus.Running, second.Status);
        }

        [TestMethod]
        public void ChangeStatus_SequentialExecution_DoesNotHoldWriteLockDuringCallback()
        {
            var test = CreateTest("Namespace.Class.First");
            var group = new GroupByOutcome();
            group.SetTestCaseList(new List<TestCase> { test });

            bool concurrentReaderCompleted = false;
            group.UpdateTestCaseList = (_, __) =>
            {
                // Mimics the WPF UI thread reading the case list while a log message is processed.
                concurrentReaderCompleted = Task.Run(() => group.TestCaseNameList != null).Wait(TimeSpan.FromSeconds(5));
            };

            group.ChangeStatus(test.FullName, TestCaseStatus.Running);

            Assert.IsTrue(concurrentReaderCompleted, "Sequential execution must not block concurrent readers while the update callback runs.");
        }

        [TestMethod]
        public void ChangeStatus_UnknownTestName_IsIgnored()
        {
            var test = CreateTest("Namespace.Class.First");
            var group = new GroupByOutcome();
            group.SetTestCaseList(new List<TestCase> { test });

            group.ChangeStatus("Namespace.Class.DoesNotExist", TestCaseStatus.Passed);

            Assert.AreEqual(TestCaseStatus.NotRun, test.Status);
        }

        [TestMethod]
        public void PipeSinkServer_TwoConcurrentClients_ReceivesBothMessages()
        {
            string pipeName = $"PTMService-{Guid.NewGuid():N}";
            var messages = new ConcurrentBag<string>();
            using var received = new CountdownEvent(2);
            PipeSinkServer.ParseLogMessage = (_, message) =>
            {
                messages.Add(message);
                received.Signal();
            };

            PipeSinkServer.Start(pipeName);
            try
            {
                using var first = Connect(pipeName);
                using var second = Connect(pipeName);
                using var firstWriter = new StreamWriter(first) { AutoFlush = true };
                using var secondWriter = new StreamWriter(second) { AutoFlush = true };

                firstWriter.WriteLine("first");
                secondWriter.WriteLine("second");

                Assert.IsTrue(received.Wait(TimeSpan.FromSeconds(5)));
                CollectionAssert.AreEquivalent(
                    new[] { "first", "second" },
                    messages.ToArray());
            }
            finally
            {
                PipeSinkServer.Stop();
                PipeSinkServer.ParseLogMessage = null;
            }
        }

        [TestMethod]
        public void PipeSinkServer_BurstOfConcurrentClients_ReceivesAllMessages()
        {
            const int clientCount = 5;
            string pipeName = $"PTMService-{Guid.NewGuid():N}";
            var messages = new ConcurrentBag<string>();
            using var received = new CountdownEvent(clientCount);
            using var start = new ManualResetEventSlim();
            PipeSinkServer.ParseLogMessage = (_, message) =>
            {
                messages.Add(message);
                received.Signal();
            };

            PipeSinkServer.Start(pipeName);
            try
            {
                var clients = Enumerable.Range(0, clientCount)
                    .Select(index => Task.Run(() =>
                    {
                        start.Wait();
                        using var client = Connect(pipeName, 1000);
                        using var writer = new StreamWriter(client) { AutoFlush = true };
                        writer.WriteLine($"client-{index}");
                    }))
                    .ToArray();

                start.Set();

                Assert.IsTrue(received.Wait(TimeSpan.FromSeconds(5)));
                Task.WaitAll(clients);
                CollectionAssert.AreEquivalent(
                    Enumerable.Range(0, clientCount).Select(index => $"client-{index}").ToArray(),
                    messages.ToArray());
            }
            finally
            {
                PipeSinkServer.Stop();
                PipeSinkServer.ParseLogMessage = null;
            }
        }

        [TestMethod]
        public void PipeSinkServer_FailedStartup_AllowsSubsequentStart()
        {
            Assert.ThrowsException<ArgumentException>(() => PipeSinkServer.Start(string.Empty));

            string pipeName = $"PTMService-{Guid.NewGuid():N}";
            PipeSinkServer.Start(pipeName);
            PipeSinkServer.Stop();
        }

        [TestMethod]
        public void RunExecutionPlan_FailedPipeStartup_ResetsConcurrentState()
        {
            var test = CreateTest("Namespace.Class.Test");
            var engine = new TestEngine("dotnet")
            {
                PipeName = string.Empty,
            };
            engine.InitializeLogger(new List<TestCase> { test });
            var plan = new TestExecutionPlan(Array.Empty<TestExecutionStage>(), 1);

            TestExecutionPlanResult result = engine.RunExecutionPlan(plan, CancellationToken.None);

            Assert.IsFalse(result.Succeeded);
            Assert.IsFalse(engine.GetTestSuiteLogManager().GroupByOutcome.ConcurrentExecution);

            string pipeName = $"PTMService-{Guid.NewGuid():N}";
            PipeSinkServer.Start(pipeName);
            PipeSinkServer.Stop();
        }

        [TestMethod]
        public void EscapeFilterValue_SpecialCharacters_ReturnsVstestSafeValue()
        {
            var escapeFilterValue = typeof(TestEngine).GetMethod(
                "EscapeFilterValue",
                BindingFlags.Static | BindingFlags.NonPublic);

            Assert.IsNotNull(escapeFilterValue);

            string escaped = (string)escapeFilterValue.Invoke(
                null,
                new object[] { "Namespace.Generic<A,B>.Test(A&B|C=1!~2)\\\"quoted\"" });

            Assert.AreEqual(
                "Namespace.Generic<A%2CB>.Test\\(A\\&B\\|C\\=1\\!\\~2\\)\\\\%22quoted%22",
                escaped);
        }

        [TestMethod]
        public void RunExecutionPlan_CancelledRun_PropagatesCancellationAndStopsPipeSink()
        {
            string pipeName = $"PTMService-{Guid.NewGuid():N}";
            var test = CreateTest("Namespace.Class.Cancelled");
            var plan = new TestExecutionPlan(
                new[]
                {
                    new TestExecutionStage(
                        "cancelled",
                        new[]
                        {
                            new TestExecutionUnit(
                                "cancelled",
                                new[] { "cancelled.dll" },
                                new[] { test }),
                        }),
                },
                1);
            var engine = new TestEngine("dotnet")
            {
                PipeName = pipeName,
            };
            engine.InitializeLogger(new List<TestCase> { test });
            using var cancellation = new CancellationTokenSource();
            cancellation.Cancel();

            Assert.ThrowsException<OperationCanceledException>(
                () => engine.RunExecutionPlan(plan, cancellation.Token));

            PipeSinkServer.Start(pipeName);
            PipeSinkServer.Stop();
        }

        [TestMethod]
        public void RunExecutionPlan_TestHostFailureWithoutResults_ReportsExecutionFailure()
        {
            string root = Path.Combine(Path.GetTempPath(), $"PTMService-{Guid.NewGuid():N}");
            Directory.CreateDirectory(root);

            try
            {
                string pipeName = $"PTMService-{Guid.NewGuid():N}";
                var test = CreateTest("Namespace.Class.NotExecuted");
                string missingAssembly = Path.Combine(root, "missing-test-assembly.dll");
                var plan = new TestExecutionPlan(
                    new[]
                    {
                        new TestExecutionStage(
                            "infrastructure",
                            new[]
                            {
                                new TestExecutionUnit(
                                    "missing-test-host",
                                    new[] { missingAssembly },
                                    new[] { test }),
                            }),
                    },
                    1);
                var engine = new TestEngine("dotnet")
                {
                    PipeName = pipeName,
                    WorkingDirectory = root + Path.DirectorySeparatorChar,
                    ResultOutputFolder = root,
                    PtfConfigDirectory = root,
                };
                engine.InitializeLogger(new List<TestCase> { test });

                TestExecutionPlanResult result = engine.RunExecutionPlan(plan, CancellationToken.None);

                Assert.IsFalse(result.Succeeded);
                Assert.AreEqual(1, result.Failures.Count);
                StringAssert.Contains(result.Failures[0].Exception.Message, "before every selected test case reported a final status");
                Assert.AreEqual(TestCaseStatus.NotRun, test.Status);
            }
            finally
            {
                Directory.Delete(root, recursive: true);
            }
        }

        [TestMethod]
        public void RunExecutionPlan_NonzeroExitWithTerminalTestStatus_DoesNotReportInfrastructureFailure()
        {
            string root = Path.Combine(Path.GetTempPath(), $"PTMService-{Guid.NewGuid():N}");
            Directory.CreateDirectory(root);

            try
            {
                string pipeName = $"PTMService-{Guid.NewGuid():N}";
                var test = CreateTest("Namespace.Class.FailedNormally");
                test.Status = TestCaseStatus.Failed;
                string missingAssembly = Path.Combine(root, "missing-test-assembly.dll");
                var plan = new TestExecutionPlan(
                    new[]
                    {
                        new TestExecutionStage(
                            "test-failure",
                            new[]
                            {
                                new TestExecutionUnit(
                                    "ordinary-test-failure",
                                    new[] { missingAssembly },
                                    new[] { test }),
                            }),
                    },
                    1);
                var engine = new TestEngine("dotnet")
                {
                    PipeName = pipeName,
                    WorkingDirectory = root + Path.DirectorySeparatorChar,
                    ResultOutputFolder = root,
                    PtfConfigDirectory = root,
                };
                engine.InitializeLogger(new List<TestCase> { test });

                TestExecutionPlanResult result = engine.RunExecutionPlan(plan, CancellationToken.None);

                Assert.IsTrue(result.Succeeded);
                Assert.AreEqual(TestCaseStatus.Failed, test.Status);
            }
            finally
            {
                Directory.Delete(root, recursive: true);
            }
        }

        [TestMethod]
        public void RunExecutionPlan_NonzeroExitWithPartiallyCompletedBatch_ReportsExecutionFailure()
        {
            string root = Path.Combine(Path.GetTempPath(), $"PTMService-{Guid.NewGuid():N}");
            Directory.CreateDirectory(root);

            try
            {
                string pipeName = $"PTMService-{Guid.NewGuid():N}";
                var completedTest = CreateTest("Namespace.Class.Completed");
                completedTest.Status = TestCaseStatus.Passed;
                var skippedTest = CreateTest("Namespace.Class.Skipped");
                string missingAssembly = Path.Combine(root, "missing-test-assembly.dll");
                var plan = new TestExecutionPlan(
                    new[]
                    {
                        new TestExecutionStage(
                            "partial-test-host-crash",
                            new[]
                            {
                                new TestExecutionUnit(
                                    "partial-test-host-crash",
                                    new[] { missingAssembly },
                                    new[] { completedTest, skippedTest }),
                            }),
                    },
                    1);
                var engine = new TestEngine("dotnet")
                {
                    PipeName = pipeName,
                    WorkingDirectory = root + Path.DirectorySeparatorChar,
                    ResultOutputFolder = root,
                    PtfConfigDirectory = root,
                };
                engine.InitializeLogger(new List<TestCase> { completedTest, skippedTest });

                TestExecutionPlanResult result = engine.RunExecutionPlan(plan, CancellationToken.None);

                Assert.IsFalse(result.Succeeded);
                Assert.AreEqual(1, result.Failures.Count);
                Assert.AreEqual(TestCaseStatus.Passed, completedTest.Status);
                Assert.AreEqual(TestCaseStatus.NotRun, skippedTest.Status);
            }
            finally
            {
                Directory.Delete(root, recursive: true);
            }
        }

        private static NamedPipeClientStream Connect(string pipeName)
        {
            return Connect(pipeName, 5000);
        }

        private static NamedPipeClientStream Connect(string pipeName, int timeout)
        {
            var client = new NamedPipeClientStream(
                ".",
                pipeName,
                PipeDirection.Out,
                PipeOptions.Asynchronous);
            client.Connect(timeout);
            return client;
        }

        private static TestCase CreateTest(string fullName)
        {
            return new TestCase
            {
                FullName = fullName,
                Name = fullName.Substring(fullName.LastIndexOf('.') + 1),
            };
        }
    }
}
