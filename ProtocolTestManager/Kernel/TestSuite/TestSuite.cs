// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Microsoft.Protocols.TestManager.Kernel
{
    /// <summary>
    /// This class defines the structure of a test suite.
    /// </summary>
    public abstract class TestSuite : ITestSuite
    {
        protected List<TestCase> _testCaseList;

        public IEnumerable<TestCase> TestCaseList
        {
            get { return _testCaseList; }
        }

        public TestSuite() { }

        /// <summary>
        /// Loads the test suite assembly
        /// </summary>
        /// <param name="dllPath">A list of assemblies.</param>
        public abstract void LoadFrom(IEnumerable<string> dllPath);

        /// <summary>
        /// Append test case categories from config.xml
        /// </summary>
        /// <param name="testCategory">TestCategory of AppConfig.</param>
        public void AppendCategoryByConfigFile(AppConfigTestCategory testCategory)
        {
            List<AppConfigTestCase> testCaseListForSearch = testCategory.TestCases;
            for (int i = 0; i < _testCaseList.Count; i++)
            {
                string testCaseName = _testCaseList[i].Name;
                for (int j = 0; j < testCaseListForSearch.Count; j++)
                {
                    if (testCaseListForSearch[j].Name == testCaseName)
                    {
                        List<string> testCategories = testCaseListForSearch[j].Categories;
                        foreach (string category in testCategories)
                        {
                            if (!_testCaseList[i].Category.Contains(category))
                            {
                                _testCaseList[i].Category.Add(category);
                            }
                        }

                        // Remove item to reduce search scope.
                        testCaseListForSearch.RemoveAt(j);
                        break;
                    }
                }
            }
        }
    }
}
