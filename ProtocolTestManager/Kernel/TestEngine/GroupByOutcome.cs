// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Microsoft.Protocols.TestManager.Kernel
{
    /// <summary>
    /// Test cases grouped by execution outcome.
    /// </summary>
    public class GroupByOutcome
    {
        private object locker = new object();
        Dictionary<string, TestCaseGroup> testcasemap;

        /// <summary>
        /// Get the current page testcase name list.
        /// </summary>
        public List<string> TestCaseNameList
        {
            get
            {
                if (testcasemap == null)
                    return null;
                else
                    return testcasemap.Keys.ToList();
            }
        }

        public void SetTestCaseList(List<TestCase> testcases)
        {
            NotRunTestCases = new TestCaseGroup("Not Run", testcases.Count);
            PassedTestCases = new TestCaseGroup("Passed", testcases.Count);
            FailedTestCases = new TestCaseGroup("Failed", testcases.Count);
            OtherTestCases = new TestCaseGroup("Inconclusive", testcases.Count);
            testcasemap = new Dictionary<string, TestCaseGroup>();
            foreach (TestCase testcase in testcases)
            {
                switch (testcase.Status)
                {
                    case TestCaseStatus.NotRun:
                        NotRunTestCases.AddTestCase(testcase);
                        lock (locker)
                        {
                            testcasemap.Add(testcase.FullName, NotRunTestCases);
                        }
                        break;
                    case TestCaseStatus.Passed:
                        PassedTestCases.AddTestCase(testcase);
                        lock (locker)
                        {
                            testcasemap.Add(testcase.FullName, PassedTestCases);
                        }
                        break;
                    case TestCaseStatus.Failed:
                        FailedTestCases.AddTestCase(testcase);
                        lock (locker)
                        {
                            testcasemap.Add(testcase.FullName, FailedTestCases);
                        }
                        break;
                    case TestCaseStatus.Other:
                        OtherTestCases.AddTestCase(testcase);
                        lock (locker)
                        {
                            testcasemap.Add(testcase.FullName, OtherTestCases);
                        }
                        break;
                    case TestCaseStatus.Running:
                        RunningTestCase = testcase;
                        break;
                }
            }
            groupList = null;
        }

        public TestCaseGroup NotRunTestCases { get; set; }
        public TestCaseGroup PassedTestCases { get; set; }
        public TestCaseGroup FailedTestCases { get; set; }
        public TestCaseGroup OtherTestCases { get; set; }

        public TestCase RunningTestCase { get; set; }

        public bool IsAborted { get; set; }

        private List<TestCaseGroup> groupList = null;
        public List<TestCaseGroup> GetList()
        {
            if (groupList == null)
            {
                groupList = new List<TestCaseGroup>();
                groupList.Add(PassedTestCases);
                groupList.Add(FailedTestCases);
                groupList.Add(OtherTestCases);
                groupList.Add(NotRunTestCases);
                IsAborted = false;
            }
            return groupList;
        }

        public void ChangeStatus(string testCaseName, TestCaseStatus status)
        {
            lock (locker)
            {
                if (!testcasemap.ContainsKey(testCaseName))
                {
                    Utility.LogException(new List<Exception>
                    {
                        new KeyNotFoundException($"The test case name \"{testCaseName}\" was not found as a key of the \"{nameof(testcasemap)}\".")
                    });
                    return;
                }

                TestCaseGroup from = testcasemap[testCaseName];
                TestCaseGroup to = OtherTestCases;
                if (from == null) return;
                TestCase testcase = from.TestCaseList.FirstOrDefault(c => c.FullName == testCaseName);
                // If changed to Running/Waiting status, no need to change group.

                if (status == TestCaseStatus.Running)
                {
                    if (RunningTestCase != null)
                    {
                        if (RunningTestCase.Status == TestCaseStatus.Running)
                        {
                            if (UpdateTestCaseStatus != null)
                            {
                                UpdateTestCaseStatus(null, null, RunningTestCase, TestCaseStatus.Waiting);
                            }
                            else
                            {
                                RunningTestCase.Status = TestCaseStatus.Waiting;
                            }
                        }
                    }
                    RunningTestCase = testcase;
                    if (UpdateTestCaseStatus != null)
                    {
                        if(RunningTestCase != null) UpdateTestCaseStatus(null, null, RunningTestCase, status);
                    }
                    else
                    {
                        RunningTestCase.Status = status;
                    }
                    if (UpdateTestCaseList != null && RunningTestCase != null)
                    {
                        UpdateTestCaseList(from, RunningTestCase);
                    }
                    SetActualStatusesForRunningWaitingTestCases();
                    return;
                }
                if (status == TestCaseStatus.Waiting)
                {
                    if (testcase.Status == TestCaseStatus.Running)
                    {
                        if (UpdateTestCaseStatus != null)
                        {
                            UpdateTestCaseStatus(null, null, testcase, status);
                        }
                        else
                        {
                            testcase.Status = status;
                        }
                        SetActualStatusesForRunningWaitingTestCases();
                        return;
                    }
                }
                switch (status)
                {
                    case TestCaseStatus.Passed:
                        to = PassedTestCases;
                        break;
                    case TestCaseStatus.Failed:
                        to = FailedTestCases;
                        break;
                    case TestCaseStatus.Other:
                        to = OtherTestCases;
                        break;
                    case TestCaseStatus.NotRun:
                        to = NotRunTestCases;
                        break;
                }

                if (UpdateTestCaseStatus != null)
                {
                    UpdateTestCaseStatus(from, to, testcase, status);
                }
                else
                {
                    testcase.Status = status;
                    from.RemoveTestCase(testcase);
                    to.AddTestCase(testcase);
                }
                testcasemap[testCaseName] = to;
                SetActualStatusesForRunningWaitingTestCases();
            }
        }

        private void SetActualStatusesForRunningWaitingTestCases()
        {
            // When it is aborted, we need to set the statuses of test cases whose current statuses are Running or Waiting to their actual statuses.
            if (IsAborted)
            {
                var casesNeedUpdateToNotRun = new List<TestCase>();
                foreach (var group in groupList)
                {
                    var runningTestCases = group.TestCaseList.Where(c => c.Status == TestCaseStatus.Running);
                    foreach (var testcase in runningTestCases)
                    {
                        casesNeedUpdateToNotRun.Add(testcase);
                    }
                    var waitingTestCases = group.TestCaseList.Where(c => c.Status == TestCaseStatus.Waiting);
                    foreach (var testcase in waitingTestCases)
                    {
                        casesNeedUpdateToNotRun.Add(testcase);
                    }
                }
                for (int i = 0; i < casesNeedUpdateToNotRun.Count; i++)
                {
                    SetTestCaseToExpectedStatusAndGroup(casesNeedUpdateToNotRun[i]);
                }
            }
        }

        private void SetTestCaseToExpectedStatusAndGroup(TestCase testcase)
        {
            string testCaseName = testcase.FullName;
            var originalGroup = testcasemap[testCaseName];
            TestCaseStatus status = TestCaseStatus.NotRun;
            TestCaseDetail caseDetail;
            if (testcase.LogUri != null && System.IO.File.Exists(testcase.LogUri.AbsolutePath))
            {
                Utility.ParseFileGetStatus(testcase.LogUri.AbsolutePath, out status, out caseDetail);
            }
            string expectedGroupName = NotRunTestCases.Name;
            if (status == TestCaseStatus.Passed) expectedGroupName = PassedTestCases.Name;
            if (status == TestCaseStatus.Failed) expectedGroupName = FailedTestCases.Name;
            if (status == TestCaseStatus.Other) expectedGroupName = OtherTestCases.Name;
            TestCaseGroup expectedGroup = GetList().Where(i => i.Name == expectedGroupName).FirstOrDefault();

            // If original group name is different with expected group name, then we need to change both test case group and status.
            // otherwise we only need to change test case status.
            if (originalGroup.Name != expectedGroupName)
            {
                if (UpdateTestCaseStatus != null)
                {
                    UpdateTestCaseStatus(originalGroup, expectedGroup, testcase, status);
                }
                else
                {
                    testcase.Status = status;
                    originalGroup.RemoveTestCase(testcase);
                    expectedGroup.AddTestCase(testcase);
                }
                testcasemap[testCaseName] = expectedGroup;
            }
            else
            {
                if (UpdateTestCaseStatus != null)
                {
                    UpdateTestCaseStatus(null, null, testcase, status);
                }
                else
                {
                    testcase.Status = status;
                }
            }
        }

        public UpdateTestCaseStatusCallback UpdateTestCaseStatus;
        public UpdateTestCaseListCallback UpdateTestCaseList;
    }
}
