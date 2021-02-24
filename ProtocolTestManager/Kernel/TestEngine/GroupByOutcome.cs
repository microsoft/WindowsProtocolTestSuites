// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace Microsoft.Protocols.TestManager.Kernel
{
    /// <summary>
    /// Test cases grouped by execution outcome.
    /// </summary>
    public class GroupByOutcome
    {
        private ReaderWriterLockSlim locker = new ReaderWriterLockSlim();
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
                locker.EnterReadLock();
                try
                {
                    switch (testcase.Status)
                    {
                        case TestCaseStatus.NotRun:
                            NotRunTestCases.AddTestCase(testcase);
                            testcasemap.Add(testcase.FullName, NotRunTestCases);
                            break;
                        case TestCaseStatus.Passed:
                            PassedTestCases.AddTestCase(testcase);
                            testcasemap.Add(testcase.FullName, PassedTestCases);
                            break;
                        case TestCaseStatus.Failed:
                            FailedTestCases.AddTestCase(testcase);
                            testcasemap.Add(testcase.FullName, FailedTestCases);
                            break;
                        case TestCaseStatus.Other:
                            OtherTestCases.AddTestCase(testcase);
                            testcasemap.Add(testcase.FullName, OtherTestCases);
                            break;
                        case TestCaseStatus.Running:
                            RunningTestCase = testcase;
                            break;
                    }
                }
                finally
                {
                    locker.ExitReadLock();
                }
            }
            groupList = null;
        }

        public TestCaseGroup NotRunTestCases { get; set; }
        public TestCaseGroup PassedTestCases { get; set; }
        public TestCaseGroup FailedTestCases { get; set; }
        public TestCaseGroup OtherTestCases { get; set; }

        public TestCase RunningTestCase { get; set; }

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
            }
            return groupList;
        }

        public void ChangeStatus(string testCaseName, TestCaseStatus status)
        {
            locker.EnterReadLock();
            try
            {
                TestCaseGroup from = testcasemap[testCaseName];
                TestCaseGroup to = OtherTestCases;
                if (from == null) return;
                TestCase testcase = from.TestCaseList.FirstOrDefault(c => c.FullName == testCaseName);
                // If changed to Running/Waiting status, no need to change group.

                if (RunningTestCase != null)
                {
                    if (RunningTestCase.Status == TestCaseStatus.Running)
                        RunningTestCase.Status = TestCaseStatus.Waiting;
                }
                RunningTestCase = testcase;
                RunningTestCase.Status = status;
                if (UpdateTestCaseList != null)
                {
                    UpdateTestCaseList(from, RunningTestCase);
                }
                return;
            }
            finally
            {
                locker.ExitReadLock();
            }
        }


        public UpdateTestCaseStatusCallback UpdateTestCaseStatus;
        public UpdateTestCaseListCallback UpdateTestCaseList;
    }
}
