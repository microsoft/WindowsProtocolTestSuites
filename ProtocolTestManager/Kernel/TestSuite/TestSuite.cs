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
        /// <param name="dllPath">A list of assemblies.</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Reliability", "CA2001:AvoidCallingProblematicMethods", MessageId = "System.Reflection.Assembly.LoadFrom")]
        public void LoadFrom(List<string> dllPath)
        {
            _testCaseList = new List<TestCase>();
            foreach (string DllFileName in dllPath)
            {
                Assembly assembly = Assembly.LoadFrom(DllFileName);
                Type[] types = assembly.GetTypes();

                foreach (Type type in types)
                {
                    //search for class, ont interfaces and other type
                    if (type.IsClass)
                    {
                        MethodInfo[] methods = type.GetMethods();
                        foreach (MethodInfo method in methods)
                        {
                            //methods loop, serch for methods with TestMethodAttribute
                            object[] objs = method.GetCustomAttributes(false);
                            bool isTestMethod = false;
                            bool isIgnored = false;
                            foreach (object attribute in method.GetCustomAttributes(false))
                            {
                                if (attribute.GetType().Name == "TestMethodAttribute") isTestMethod = true;
                                if (attribute.GetType().Name == "IgnoreAttribute") isIgnored = true;
                            }
                            if (isTestMethod && !isIgnored)
                            {
                                //GetCategory
                                List<string> categories = new List<string>();
                                string caseFullName = method.DeclaringType.FullName + "." + method.Name;
                                foreach (object attribute in objs)
                                {
                                    //record TestCategories
                                    if (attribute.GetType().Name == "TestCategoryAttribute")
                                    {
                                        PropertyInfo property = attribute.GetType().GetProperty("TestCategories");
                                        object category = property.GetValue(attribute, null);
                                        foreach (string str in (System.Collections.ObjectModel.ReadOnlyCollection<string>)category)
                                        {
                                            categories.Add(str);
                                        }
                                    }
                                }
                                TestCase testcase = new TestCase()
                                {
                                    FullName = caseFullName,
                                    Category = categories,
                                    Name = method.Name
                                };
                                testcase.ToolTipOnUI = testcase.Name + Environment.NewLine + "Category:";
                                foreach (string category in testcase.Category)
                                {
                                    testcase.ToolTipOnUI += Environment.NewLine + category;
                                }
                                _testCaseList.Add(testcase);
                            }
                        }
                    }
                }

            }
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
