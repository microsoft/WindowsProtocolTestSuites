// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Protocols.TestManager.Common;
using Microsoft.Protocols.TestManager.Kernel;
using Microsoft.Protocols.TestManager.PTMService.Abstractions;
using Microsoft.Protocols.TestManager.PTMService.Abstractions.Kernel;
using Microsoft.Protocols.TestManager.PTMService.Common.Entities;
using Microsoft.Protocols.TestManager.PTMService.Common.Types;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
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

        public TestResultState State { get; private set; }

        public int? Total { get; private set; }

        public int? NotRun { get; private set; }

        public int? Passed { get; private set; }

        public int? Failed { get; private set; }

        public int? Inconclusive { get; private set; }

        private record TestCaseInfo(TestCaseState Status, TestCaseResult Detail, Task Task);

        private ConcurrentDictionary<string, TestCaseInfo> runningTestResult;

        private TestRun(string testEnginePath, TestResult testResult, IConfiguration configuration, IStorageNode storageRoot, TestResultUpdateDelegate update)
        {
            TestEnginePath = testEnginePath;

            Id = testResult.Id;

            Configuration = configuration;

            StorageRoot = storageRoot;

            State = testResult.State;

            Total = testResult.Total;

            Total = testResult.NotRun;

            Passed = testResult.Passed;

            Failed = testResult.Failed;

            Inconclusive = testResult.Inconclusive;

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

            using var stream = new MemoryStream();

            using var sw = new StreamWriter(stream);

            string content = JsonSerializer.Serialize(tests.Select(t => t.FullName).ToArray());

            sw.Write(content);

            sw.Flush();

            stream.Seek(0, SeekOrigin.Begin);

            StorageRoot.CreateFile(TestRunConsts.TestCaseListFile, stream);

            runningTestResult = new ConcurrentDictionary<string, TestCaseInfo>(tests.Select(t => new KeyValuePair<string, TestCaseInfo>(t.FullName, new TestCaseInfo(TestCaseState.NotRun, null, null))));

            testEngine.InitializeLogger(tests.ToList());

            testEngine.GetTestSuiteLogManager().GroupByOutcome.UpdateTestCaseList = (_, testCase) =>
            {
                try
                {
                    UpdateTestCase(testCase);
                }
                catch (Exception ex)
                {
                    // Add exception log.
                }
            };

            RunTask = Task.Run(() =>
            {
                try
                {
                    State = TestResultState.Running;

                    UpdateItem();

                    testEngine.RunByCase(new Stack<TestCase>(tests), CancellationTokenSource.Token);

                    State = TestResultState.Finished;

                    Total = runningTestResult.Count();

                    NotRun = runningTestResult.Values.Where(info => info.Status == TestCaseState.NotRun || info.Status == TestCaseState.Running).Count();

                    Passed = runningTestResult.Values.Where(info => info.Status == TestCaseState.Passed).Count();

                    Failed = runningTestResult.Values.Where(info => info.Status == TestCaseState.Failed).Count();

                    Inconclusive = runningTestResult.Values.Where(info => info.Status == TestCaseState.Inconclusive).Count();

                    UpdateItem();
                }
                catch (Exception ex)
                {
                    State = TestResultState.Failed;

                    Total = runningTestResult.Count();

                    NotRun = runningTestResult.Values.Where(info => info.Status == TestCaseState.NotRun || info.Status == TestCaseState.Running).Count();

                    Passed = runningTestResult.Values.Where(info => info.Status == TestCaseState.Passed).Count();

                    Failed = runningTestResult.Values.Where(info => info.Status == TestCaseState.Failed).Count();

                    Inconclusive = runningTestResult.Values.Where(info => info.Status == TestCaseState.Inconclusive).Count();

                    UpdateItem();
                }
            });
        }

        public void Abort()
        {
            CancellationTokenSource.Cancel();

            RunTask.Wait();
        }

        public IDictionary<string, TestCaseOverview> GetRunningStatus()
        {
            if (runningTestResult != null)
            {
                var result = runningTestResult.ToDictionary(info => info.Key, info =>
                {
                    return new TestCaseOverview
                    {
                        FullName = info.Key,
                        State = info.Value.Status,
                    };
                });

                return result;
            }
            else
            {
                using var stream = StorageRoot.ReadFile(TestRunConsts.TestCaseListFile);

                using var sr = new StreamReader(stream);

                string content = sr.ReadToEnd();

                var list = JsonSerializer.Deserialize<string[]>(content);

                var result = list.ToDictionary(test => test, test =>
                {
                    try
                    {
                        var testCaseResult = GetTestCaseResult(test);

                        return testCaseResult.Overview;
                    }
                    catch (Exception ex)
                    {
                        return new TestCaseOverview
                        {
                            FullName = test,
                            State = TestCaseState.NotRun,
                        };
                    }
                });

                return result;
            }
        }

        public TestCaseResult GetTestCaseDetail(string name)
        {
            if (runningTestResult != null)
            {
                var info = runningTestResult[name];

                if (info.Task != null)
                {
                    info.Task.Wait();

                    info = runningTestResult[name];
                }

                var result = new TestCaseResult
                {
                    Overview = new TestCaseOverview
                    {
                        FullName = name,
                        State = info.Status,
                    },
                    StartTime = info.Detail?.StartTime,
                    EndTime = info.Detail?.EndTime,
                    Output = info.Detail?.Output,
                };

                return result;
            }
            else
            {
                try
                {
                    var testCaseResult = GetTestCaseResult(name);

                    return testCaseResult;
                }
                catch
                {
                    return new TestCaseResult
                    {
                        Overview = new TestCaseOverview
                        {
                            FullName = name,
                            State = TestCaseState.NotRun,
                        },
                    };
                }
            }
        }

        private void UpdateItem()
        {
            Update(Id, testResult =>
            {
                testResult.State = State;

                testResult.Total = Total;

                testResult.NotRun = NotRun;

                testResult.Passed = Passed;

                testResult.Failed = Failed;

                testResult.Inconclusive = Inconclusive;
            });
        }

        private void UpdateTestCase(TestCase testCase)
        {
            var state = testCase.Status switch
            {
                TestCaseStatus.Passed => TestCaseState.Passed,
                TestCaseStatus.Failed => TestCaseState.Failed,
                TestCaseStatus.Other => TestCaseState.Inconclusive,
                _ => TestCaseState.Running,
            };

            if (state != TestCaseState.Running)
            {
                var task = Task.Run(() =>
                {
                    // Need to wait before HTML file is generated.
                    Task.Delay(5000).Wait();

                    var testCaseResult = GetTestCaseResult(testCase.FullName);

                    var info = new TestCaseInfo(state, testCaseResult, null);

                    runningTestResult.AddOrUpdate(testCase.FullName, info, (k, old) => info);
                });

                var info = new TestCaseInfo(state, null, task);

                runningTestResult.AddOrUpdate(testCase.FullName, info, (k, old) => info);
            }
            else
            {
                var info = new TestCaseInfo(state, null, null);

                runningTestResult.AddOrUpdate(testCase.FullName, info, (k, old) => info);
            }
        }

        private TestCaseResult GetTestCaseResult(string test)
        {
            string filePath = Directory.EnumerateFiles(StorageRoot.AbsolutePath, $"{test}.html", SearchOption.AllDirectories).First();

            var content = File.ReadAllLines(filePath);

            var detailLine = content.Where(l => l.StartsWith(AppConfig.DetailKeyword));
            string detailStr;

            detailStr = detailLine.First();

            int startIndex = AppConfig.DetailKeyword.Length;
            int endIndex = detailStr.Length - 1;
            string detailJson = detailStr.Substring(startIndex, endIndex - startIndex);

            var detail = JsonSerializer.Deserialize<TestCaseDetail>(detailJson);

            var state = detail.Result switch
            {
                "Passed" => TestCaseState.Passed,
                "Failed" => TestCaseState.Failed,
                "Inconclusive" => TestCaseState.Inconclusive,
                _ => TestCaseState.NotRun,
            };

            var result = new TestCaseResult
            {
                Overview = new TestCaseOverview
                {
                    FullName = test,
                    State = state,
                },
                StartTime = detail.StartTime,
                EndTime = detail.EndTime,
                Output = String.Join("\n", detail.StandardOut.Select(output => output.Content)),
            };

            return result;
        }
    }
}
