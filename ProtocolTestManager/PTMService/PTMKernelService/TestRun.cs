// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Protocols.TestManager.Kernel;
using Microsoft.Protocols.TestManager.PTMService.Abstractions;
using Microsoft.Protocols.TestManager.PTMService.Abstractions.Kernel;
using Microsoft.Protocols.TestManager.PTMService.Common.Entities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Microsoft.Protocols.TestManager.PTMService.PTMKernelService
{
    internal delegate void TestResultUpdateDelegate(int id, Action<TestResult> updater);

    internal class TestRun : ITestRun
    {
        private string TestEnginePath { get; init; }

        public IConfiguration Configuration { get; init; }

        public int Id { get; init; }

        public IStorageNode StorageRoot { get; init; }

        private CancellationTokenSource CancellationTokenSource { get; init; }

        private TestResultUpdateDelegate Update { get; init; }

        private Task RunTask { get; set; }

        private TestRun(string testEnginePath, TestResult testResult, IConfiguration configuration, IStorageNode storageRoot, TestResultUpdateDelegate update)
        {
            TestEnginePath = testEnginePath;

            Id = configuration.Id;

            Configuration = configuration;

            StorageRoot = storageRoot;

            Update = update;

            CancellationTokenSource = new CancellationTokenSource();
        }

        public static TestRun Create(string testEnginePath, TestResult testResult, IConfiguration configuration, IStoragePool storagePool, TestResultUpdateDelegate update)
        {
            var testResultNode = storagePool.GetKnownNode(KnownStorageNodeNames.TestResult).CreateNode(testResult.Id.ToString());

            testResult.Path = testResultNode.AbsolutePath;

            testResult.State = TestResultState.Created;

            var result = new TestRun(testEnginePath, testResult, configuration, testResultNode, update);

            return result;
        }

        public static TestRun Open(string testEnginePath, TestResult testResult, IConfiguration configuration, IStoragePool storagePool, TestResultUpdateDelegate update)
        {
            var testResultNode = storagePool.OpenNode(testResult.Path);

            var result = new TestRun(testEnginePath, testResult, configuration, testResultNode, update);

            return result;
        }

        public void Run(IEnumerable<string> selectedTestCases)
        {
            RunTask = Task.Run(() =>
            {
                try
                {
                    var testEngine = new TestEngine(TestEnginePath)
                    {
                        WorkingDirectory = $"{Configuration.TestSuite.StorageRoot.AbsolutePath}{Path.DirectorySeparatorChar}",
                        TestAssemblies = Configuration.TestSuite.GetTestAssemblies().ToList(),
                        ResultOutputFolder = StorageRoot.AbsolutePath,
                        PtfConfigDirectory = Configuration.StorageRoot.GetNode(ConfigurationConsts.PtfConfig).AbsolutePath,
                        RunSettingsPath = Path.Combine(StorageRoot.AbsolutePath, $"{Configuration.Name}.runsettings"),
                        PipeName = "PTFToolPipe",
                    };

                    if (selectedTestCases == null)
                    {
                        selectedTestCases = Configuration.GetApplicableTestCases();
                    }

                    var tests = testEngine.LoadTestCases().Join(selectedTestCases, o => o.FullName, i => i, (o, i) => o);

                    testEngine.InitializeLogger(tests.ToList());

                    testEngine.RunByCase(new Stack<TestCase>(tests), CancellationTokenSource.Token);

                    Update(Id, testResult =>
                    {
                        testResult.State = TestResultState.Finished;
                    });
                }
                catch (Exception ex)
                {
                    Update(Id, testResult =>
                    {
                        testResult.State = TestResultState.Failed;
                    });
                }
            });
        }

        public void Abort()
        {
            CancellationTokenSource.Cancel();

            RunTask.Wait();
        }
    }
}
