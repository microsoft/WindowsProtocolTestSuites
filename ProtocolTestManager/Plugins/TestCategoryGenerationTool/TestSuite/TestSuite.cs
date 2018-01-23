// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Xml;
using System.IO;

namespace Microsoft.Protocols.TestManager.TestCategoryGenerationTool
{
    /// <summary>
    /// This class defines the structure of a test suite.
    /// </summary>
    public class TestSuite
    {
        private List<TestCase> _testCaseList;

        /// <summary>
        /// Test case list
        /// </summary>
        public List<TestCase> TestCaseList
        {
            get { return _testCaseList; }
            set { _testCaseList = value; }
        }

        /// <summary>
        /// Constructor of TestSuite
        /// </summary>
        public TestSuite() { }

        /// <summary>
        /// Load the test suite assembly
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
                    // Search for class, interfaces and other type
                    if (type.IsClass)
                    {
                        MethodInfo[] methods = type.GetMethods();
                        foreach (MethodInfo method in methods)
                        {
                            // Search for methods with TestMethodAttribute
                            object[] attributes = method.GetCustomAttributes(false);
                            bool isTestMethod = false;
                            bool isIgnored = false;
                            foreach (object attribute in attributes)
                            {
                                // Break the loop when "IgnoreAttribute" is found
                                if (attribute.GetType().Name == "IgnoreAttribute")
                                {
                                    isIgnored = true;
                                    break;
                                }

                                // Do not break the loop when "TestMethodAttribute" is found
                                // It's possible to have "IgnoreAttribute" after "TestMethodAttribute"
                                if (attribute.GetType().Name == "TestMethodAttribute")
                                {
                                    isTestMethod = true;
                                }
                            }
                            if (isTestMethod && !isIgnored)
                            {
                                // GetCategory
                                List<string> categories = new List<string>();
                                foreach (object attribute in attributes)
                                {
                                    // Record TestCategories
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
                                    Category = categories,
                                    Name = method.Name
                                };
                                _testCaseList.Add(testcase);
                            }
                        }
                    }
                }

            }
        }

        /// <summary>
        /// Append categories for FileServer test cases in TestCategories.xml
        ///   1. Add "Positive" for model-based cases without the following test categories in Priority Filter.
        ///        BVT, Positive, UnexpectedFields, InvalidIdentifier, OutOfBoundary, Compatibility, UnexpectedContext
        ///   2. Add "NonSmb" to FSA model-based cases.
        /// </summary>
        public void AppendCategoryForFileServerCases()
        {
            string testCategoriesXmlPath = "TestCategories.xml";
            if (File.Exists(testCategoriesXmlPath))
            {
                File.Delete(testCategoriesXmlPath);
            }
            using (XmlTextWriter xml = new XmlTextWriter(testCategoriesXmlPath, null))
            {
                xml.WriteStartElement("TestCategories");
                xml.WriteWhitespace("\n");
                for (int i = 0; i < TestCaseList.Count; i++)
                {
                    bool isFsa = false;
                    bool isModel = false;
                    TestCase currentCase = TestCaseList[i];
                    List<string> categories = currentCase.Category;

                    // Model-based case
                    if (categories.Contains("Model"))
                    {
                        // TestCase which does not have any category in Priority Filter
                        if (!categories.Contains("BVT") &&
                            !categories.Contains("Positive") &&
                            !categories.Contains("UnexpectedFields") &&
                            !categories.Contains("InvalidIdentifier") &&
                            !categories.Contains("OutOfBoundary") &&
                            !categories.Contains("Compatibility") &&
                            !categories.Contains("UnexpectedContext"))
                        {
                            isModel = true;
                        }

                        // FSA case
                        if (categories.Contains("FSA"))
                        {
                            if (!categories.Contains("NonSmb"))
                            {
                                isFsa = true;
                            }
                        }

                        if (isFsa || isModel)
                        {
                            xml.WriteWhitespace("  ");
                            xml.WriteStartElement("TestCase");
                            xml.WriteAttributeString("name", currentCase.Name);
                            xml.WriteWhitespace("\n");
                            if (isFsa)
                            {
                                xml.WriteWhitespace("    ");
                                xml.WriteStartElement("Category");
                                xml.WriteAttributeString("name", "NonSmb");
                                xml.WriteEndElement();
                                xml.WriteWhitespace("\n");
                            }
                            if (isModel)
                            {
                                xml.WriteWhitespace("    ");
                                xml.WriteStartElement("Category");
                                xml.WriteAttributeString("name", "Positive");
                                xml.WriteEndElement();
                                xml.WriteWhitespace("\n");
                            }
                            xml.WriteWhitespace("  ");
                            xml.WriteEndElement();
                            xml.WriteWhitespace("\n");
                        }
                        isFsa = false;
                        isModel = false;
                    }
                }
                xml.WriteEndElement();
                xml.WriteWhitespace("\n");
                xml.Close();
            }
        }
    }
}
