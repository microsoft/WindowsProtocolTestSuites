// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Microsoft.Protocols.TestManager.Kernel
{
    /// <summary>
    /// Test cases grouped by category.
    /// </summary>
    public class GroupByCategory
    {
        public void SetTestCaseList(List<TestCase> allTestCases)
        {
            Dictionary<string, TestCaseGroup> groups = new Dictionary<string, TestCaseGroup>();
            foreach (TestCase testcase in allTestCases)
            {
                foreach (string category in testcase.Category)
                {
                    if (!groups.ContainsKey(category)) groups.Add(category, new TestCaseGroup(category, allTestCases.Count));
                    groups[category].AddTestCase(testcase);
                }
            }
            groupList = new List<TestCaseGroup>();
            foreach (KeyValuePair<string, TestCaseGroup> item in groups.OrderBy(key => key.Key))
            {
                groupList.Add(item.Value);
            }
        }

        private List<TestCaseGroup> groupList;
        public List<TestCaseGroup> GetList()
        {
            return groupList;
        }
    }
}
