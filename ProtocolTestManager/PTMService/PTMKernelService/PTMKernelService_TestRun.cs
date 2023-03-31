// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Protocols.TestManager.Kernel;
using Microsoft.Protocols.TestManager.PTMService.Abstractions;
using Microsoft.Protocols.TestManager.PTMService.Abstractions.Kernel;
using Microsoft.Protocols.TestManager.PTMService.Common.Entities;
using Microsoft.Protocols.TestManager.PTMService.Common.Types;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Microsoft.Protocols.TestManager.PTMService.PTMKernelService
{
    internal partial class PTMKernelService
    {
        public ITestRun[] QueryTestRuns(int pageSize, int pageIndex, Func<TestResult, bool> queryFunc, out int totalPage)
        {
            using var instance = ScopedServiceFactory.GetInstance();

            var pool = instance.ScopedServiceInstance;

            var repo = pool.Get<TestResult>();

            var all = repo.Get(q => q.Where(queryFunc).OrderByDescending(item => item.Id).AsQueryable()).Select(item => GetTestRunInternal(item.Id, item));

            int count = all.Count();

            if (count % pageSize == 0)
            {
                totalPage = count / pageSize;
            }
            else
            {
                totalPage = count / pageSize + 1;
            }

            var result = all.Skip(pageSize * pageIndex).Take(pageSize);

            return result.ToArray();
        }

        public ITestRun GetTestRun(int id)
        {
            return GetTestRunInternal(id, null);
        }

        public bool IsTestSuiteRunning()
        {
            using var instance = ScopedServiceFactory.GetInstance();

            var pool = instance.ScopedServiceInstance;

            var repo = pool.Get<TestResult>();

            return repo.Get(q => q.Where(r => r.State == TestResultState.Running)).Any();
        }

        private ITestRun GetTestRunInternal(int id, TestResult testResult)
        {
            if (!TestRunPool.ContainsKey(id))
            {
                if (testResult == null)
                {
                    using var instance = ScopedServiceFactory.GetInstance();

                    var pool = instance.ScopedServiceInstance;

                    var repo = pool.Get<TestResult>();

                    testResult = repo.Get(q => q.Where(item => item.Id == id)).First();
                }

                var configuration = GetConfiguration(testResult.TestSuiteConfigurationId);
                var testRun = TestRun.Open(Options.TestEnginePath, testResult, configuration, StoragePool, Update);

                TestRunPool.AddOrUpdate(id, _ => testRun, (_, _) => testRun);
            }

            return TestRunPool[id];
        }

        public int CreateTestRun(int configurationId, string[] selectedTestCases)
        {
            var configuration = GetConfiguration(configurationId);

            using var instance = ScopedServiceFactory.GetInstance();

            var pool = instance.ScopedServiceInstance;

            var repo = pool.Get<TestResult>();

            var testResult = new TestResult
            {
                TestSuiteConfigurationId = configurationId,
                State = TestResultState.Created,
            };

            repo.Insert(testResult);

            pool.Save().Wait();

            int id = testResult.Id;

            var testRun = TestRun.Create(Options.TestEnginePath, testResult, configuration, StoragePool, Update);

            repo.Update(testResult);

            pool.Save().Wait();

            testRun.Run(selectedTestCases);

            TestRunPool.AddOrUpdate(id, _ => testRun, (_, _) => testRun);

            return id;
        }

        private void Update(int id, Action<TestResult> updater)
        {
            using var instance = ScopedServiceFactory.GetInstance();

            var pool = instance.ScopedServiceInstance;

            var repo = pool.Get<TestResult>();

            var testResult = repo.Get(q => q.Where(item => item.Id == id)).First();

            updater(testResult);

            repo.Update(testResult);

            pool.Save().Wait();
        }

        public void RemoveTestRun(int id)
        {
            ITestRun testRun = default(ITestRun);
            if (TestRunPool.ContainsKey(id))
            {
                testRun = TestRunPool[id];
            }

            if (testRun != default(ITestRun) &&
                testRun.State != TestResultState.Running)
            {
                //If the test run is newly created and about to start, try aborting it first.
                if(testRun.State == TestResultState.Created)
                {
                    testRun.Abort();
                }

                TestRunPool.Remove(id, out ITestRun run);

                using var instance = ScopedServiceFactory.GetInstance();

                var pool = instance.ScopedServiceInstance;
                var repo = pool.Get<TestResult>();

                var testResult = repo.Get(q => q.Where(item => item.Id == id)).FirstOrDefault();
                if (testResult != null)
                {
                    repo.Remove(testResult);
                    pool.Save().Wait();

                    try
                    {
                        if (Directory.Exists(testResult.Path))
                        {
                            Directory.Delete(testResult.Path, true);
                        }
                    }
                    catch (Exception ex)
                    {
                        Logger.AddLog(LogLevel.Debug, $"Deleting {testResult.Path} failed with {ex.ToString()}");
                    }
                }
            }
        }

        public string GetTestRunReport(int testResultId, ReportFormat format, string[] testCases)
        {
            var testRun = GetTestRun(testResultId);
            var testCaseDetails = testCases.Select(testRun.GetTestCaseDetail).Where(e => e.found).Select(e => e.detail);

            Dictionary<string, string> descriptionDict = null;
            if (format == ReportFormat.Json)
            {
                if (DescriptionDictCache.ContainsKey(testRun.Configuration.TestSuite.Id))
                {
                    DescriptionDictCache.TryGetValue(testRun.Configuration.TestSuite.Id, out descriptionDict);
                }
                else
                {
                    var testSuite = GetTestSuite(testRun.Configuration.TestSuite.Id);
                    descriptionDict = testSuite.GetTestCases(null).GroupBy(info => info.FullName).ToDictionary(g => g.Key, g => g.Single().Description);

                    DescriptionDictCache.AddOrUpdate(testSuite.Id, _ => descriptionDict, (_, _) => descriptionDict);
                }
            }

            var kernelTestCases = testCaseDetails.Select(detail =>
            {
                var kernelTestCase = new TestCase
                {
                    Category = detail.Categories,
                    Name = detail.Name,
                    FullName = detail.FullyQualifiedName,
                    Assembly = detail.Source,
                    Status = detail.Result switch
                    {
                        "Passed" => TestCaseStatus.Passed,
                        "Failed" => TestCaseStatus.Failed,
                        "Inconclusive" => TestCaseStatus.Other,
                        _ => TestCaseStatus.NotRun,
                    },
                    StartTime = DateTimeOffset.Parse(detail.StartTime),
                    EndTime = DateTimeOffset.Parse(detail.EndTime),
                    StdOut = string.Join(Environment.NewLine, detail.StandardOut.Select(line => line.Content)),
                    ErrorStackTrace = string.Join(Environment.NewLine, detail.ErrorStackTrace),
                    ErrorMessage = string.Join(Environment.NewLine, detail.ErrorMessage)
                };

                if (format == ReportFormat.Json)
                {
                    if (descriptionDict.ContainsKey(kernelTestCase.FullName))
                    {
                        kernelTestCase.Description = descriptionDict[kernelTestCase.FullName];
                    }
                    else
                    {
                        kernelTestCase.Description = string.Empty;
                    }
                }

                return kernelTestCase;
            });

            var report = TestReport.GetInstance(format.ToString(), kernelTestCases.ToList());

            var tempNode = GetTestRunTempNode();
            var reportPath = Path.Combine(tempNode.AbsolutePath, $"Report_{testResultId}_{Guid.NewGuid()}.{report.FileExtension}");
            report.ExportReport(reportPath);

            return reportPath;
        }

        private IStorageNode GetTestRunTempNode()
        {
            var testResultNode = StoragePool.GetKnownNode(KnownStorageNodeNames.TestResult);

            lock (syncRoot)
            {
                var tempNodeName = "temp";
                var tempNodePath = testResultNode.GetNodes().Where(n => new DirectoryInfo(n).Name == tempNodeName).FirstOrDefault();

                if (tempNodePath != null)
                {
                    return testResultNode.GetNode(tempNodeName);
                }

                return testResultNode.CreateNode(tempNodeName);
            }
        }
    }
}
