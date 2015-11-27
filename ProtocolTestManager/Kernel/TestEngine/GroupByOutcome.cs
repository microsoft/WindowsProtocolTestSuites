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
        Dictionary<string, TestCaseGroup> testcasemap;

        public void SetTestCaseList(List<TestCase> testcases)
        {
            NotRunTestCases = new TestCaseGroup("Not Run", testcases.Count);
            InProgressTestCases = new TestCaseGroup("In Progress", testcases.Count);
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
                        testcasemap.Add(testcase.Name, NotRunTestCases);
                        break;
                    case TestCaseStatus.Passed:
                        PassedTestCases.AddTestCase(testcase);
                        testcasemap.Add(testcase.Name, PassedTestCases);
                        break;
                    case TestCaseStatus.Failed:
                        FailedTestCases.AddTestCase(testcase);
                        testcasemap.Add(testcase.Name, FailedTestCases);
                        break;
                    case TestCaseStatus.Other:
                        OtherTestCases.AddTestCase(testcase);
                        testcasemap.Add(testcase.Name, OtherTestCases);
                        break;
                    case TestCaseStatus.Running:
                        InProgressTestCases.AddTestCase(testcase);
                        testcasemap.Add(testcase.Name, InProgressTestCases);
                        break;
                }
            }
            groupList = null;
        }

        public TestCaseGroup NotRunTestCases { get; set; }
        public TestCaseGroup InProgressTestCases { get; set; }
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
                groupList.Add(InProgressTestCases);
                groupList.Add(PassedTestCases);
                groupList.Add(FailedTestCases);
                groupList.Add(OtherTestCases);
                groupList.Add(NotRunTestCases);
            }
            return groupList;
        }

        public void ChangeStatus(string testCaseName, TestCaseStatus status)
        {
            TestCaseGroup from = testcasemap[testCaseName];
            TestCaseGroup to = OtherTestCases;
            if (from == null) return;
            TestCase testcase = from.TestCaseList.FirstOrDefault(c => c.Name == testCaseName);
            if (status == TestCaseStatus.Running) RunningTestCase = testcase;
            switch (status)
            {
                case TestCaseStatus.Running:
                    to = InProgressTestCases;
                    break;
                case TestCaseStatus.Passed:
                    testcase.IsChecked = false;
                    to = PassedTestCases;
                    break;
                case TestCaseStatus.Failed:
                    testcase.IsChecked = true;
                    to = FailedTestCases;
                    break;
                case TestCaseStatus.Other:
                    to = OtherTestCases;
                    break;
                case TestCaseStatus.NotRun:
                    to = NotRunTestCases;
                    break;
            }

            testcase.Status = status;
            if (UpdateTestCaseStatus != null)
            {
                UpdateTestCaseStatus(from, to, testcase);
            }
            else
            {

                from.RemoveTestCase(testcase);
                to.AddTestCase(testcase);
            }
            testcasemap[testCaseName] = to;
        }


        public UpdateTestCaseStatusCallback UpdateTestCaseStatus;
    }
}
