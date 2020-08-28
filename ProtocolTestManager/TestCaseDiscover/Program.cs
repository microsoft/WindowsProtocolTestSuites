// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Protocols.TestManager.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.Json;

namespace Microsoft.Protocols.TestManager.TestCaseDiscover
{
    class Program
    {
        static void Main(string[] args)
        {
            string inputFile = args[0];

            var inputContent = File.ReadAllText(inputFile);

            var dllPath = JsonSerializer.Deserialize<string[]>(inputContent);

            var testCaseInfos = Load(dllPath);

            var outputContent = JsonSerializer.Serialize(testCaseInfos.ToArray());

            string outputFile = args[1];

            File.WriteAllText(outputFile, outputContent);
        }

        /// <summary>
        /// Load test case infos of given dll files.
        /// </summary>
        /// <param name="dllPath">The dll path.</param>
        /// <returns>The loaded test case infos.</returns>
        public static IEnumerable<TestCaseInfo> Load(string[] dllPath)
        {
            var _testCaseList = new List<TestCaseInfo>();

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
                                string name = attribute.GetType().Name;
                                // Break the loop when "IgnoreAttribute" is found
                                if (name == "IgnoreAttribute")
                                {
                                    isIgnored = true;
                                    break;
                                }

                                // Do not break the loop when "TestMethodAttribute" is found
                                // It's possible to have "IgnoreAttribute" after "TestMethodAttribute"
                                if (name == "TestMethodAttribute")
                                {
                                    isTestMethod = true;
                                }

                                // Ignore test case with TestCategory "Disabled"
                                if (name == "TestCategoryAttribute")
                                {
                                    PropertyInfo property = attribute.GetType().GetProperty("TestCategories");
                                    var category = property.GetValue(attribute, null) as List<string>;
                                    foreach (string str in category)
                                    {
                                        if (str == "Disabled")
                                        {
                                            isIgnored = true;
                                            break;
                                        }
                                    }
                                }
                            }
                            if (isTestMethod && !isIgnored)
                            {
                                // Get categories and description
                                List<string> categories = new List<string>();
                                string description = null;
                                string caseFullName = method.DeclaringType.FullName + "." + method.Name;
                                foreach (object attribute in attributes)
                                {
                                    // Record TestCategories
                                    if (attribute.GetType().Name == "TestCategoryAttribute")
                                    {
                                        PropertyInfo property = attribute.GetType().GetProperty("TestCategories");
                                        var category = property.GetValue(attribute, null) as List<string>;
                                        foreach (string str in category)
                                        {
                                            categories.Add(str);
                                        }
                                    }

                                    // Record Description
                                    if (attribute.GetType().Name == "DescriptionAttribute")
                                    {
                                        var descriptionProp = attribute.GetType().GetProperty("Description");
                                        description = descriptionProp.GetValue(attribute, null) as string;
                                    }
                                }

                                var testcase = new TestCaseInfo()
                                {
                                    FullName = caseFullName,
                                    Category = categories.ToArray(),
                                    Description = description,
                                    Name = method.Name
                                };

                                var testcaseToolTipBuilder = new StringBuilder();
                                testcaseToolTipBuilder.Append(testcase.Name);
                                if (testcase.Category.Any())
                                {
                                    testcaseToolTipBuilder.Append(Environment.NewLine + "Category:");
                                    foreach (var category in testcase.Category)
                                    {
                                        testcaseToolTipBuilder.Append(Environment.NewLine + "  " + category);
                                    }
                                }
                                if (!string.IsNullOrEmpty(testcase.Description))
                                {
                                    testcaseToolTipBuilder.Append(Environment.NewLine + "Description:");
                                    testcaseToolTipBuilder.Append(Environment.NewLine + "  " + testcase.Description);
                                }
                                testcase.ToolTipOnUI = testcaseToolTipBuilder.ToString();

                                _testCaseList.Add(testcase);
                            }
                        }
                    }
                }

            }

            return _testCaseList;
        }
    }
}
