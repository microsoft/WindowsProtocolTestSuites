// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using System;
using System.Collections.Generic;

namespace Microsoft.Protocols.TestManager.Kernel
{
    /// <summary>
    /// This class defines the structure of a test suite.
    /// </summary>
    public class TestSuite
    {

        private List<TestCase> _testCaseList;
        public List<TestCase> TestCaseList
        {
            get { return _testCaseList; }
            set { _testCaseList = value; }
        }

        public TestSuite() { }

        /// <summary>
        /// Loads the test suite assembly
        /// </summary>
        /// <param name="enginePath">The test engine path.</param>
        /// <param name="testSuitePath">Test suite path.</param>
        /// <param name="dllPath">A list of assemblies.</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Reliability", "CA2001:AvoidCallingProblematicMethods", MessageId = "System.Reflection.Assembly.LoadFrom")]
        public void LoadFrom(string enginePath, string testSuitePath, List<string> dllPath)
        {
            var testEngine = new TestEngine(enginePath)
            {
                TestAssemblies = dllPath,
                WorkingDirectory = testSuitePath,
            };

            _testCaseList = testEngine.LoadTestCases();
        }

        /// <summary>
        /// Append test case categories from config.xml
        /// </summary>
        /// <param name="testCategory">TestCategory of AppConfig.</param>
        public void AppendCategoryByConfigFile(AppConfigTestCategory testCategory)
        {
            List<AppConfigTestCase> testCaseListForSearch = testCategory.TestCases;
            for (int i = 0; i < TestCaseList.Count; i++)
            {
                string testCaseName = TestCaseList[i].Name;
                for (int j = 0; j < testCaseListForSearch.Count; j++)
                {
                    if (testCaseListForSearch[j].Name == testCaseName)
                    {
                        List<string> testCategories = testCaseListForSearch[j].Categories;
                        foreach (string category in testCategories)
                        {
                            if (!TestCaseList[i].Category.Contains(category))
                            {
                                TestCaseList[i].Category.Add(category);
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
