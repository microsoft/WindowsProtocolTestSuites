// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Protocols.TestTools;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Microsoft.Protocol.TestSuites.Smbd.TestSuite
{
    public class SmbdUtilities
    {
        /// <summary>
        /// Create file name
        /// </summary>
        /// <returns></returns>
        public static string CreateRandomFileName()
        {
            return string.Format("{0}.txt", Guid.NewGuid());
        }

        /// <summary>
        /// Compare two byte arrays
        /// </summary>
        /// <param name="array1">Byte array 1.</param>
        /// <param name="array2">Byte array 2.</param>
        /// <returns>Return True if these two byte arrays are the same, otherwise return False.</returns>
        public static bool CompareByteArray(byte[] array1, byte[] array2)
        {
            if (array1.Length != array2.Length)
            {
                return false;
            }
            for (int i = 0; i < array1.Length; i++)
            {
                if (array1[i] != array2[i])
                {
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// Add test case description to log
        /// </summary>
        /// <param name="testSite">TestSite interface which provides logging, assertions, and SUT adapters for test.</param>
        public static void LogTestCaseDescription(ITestSite testSite)
        {
            var testcase = (string)testSite.TestProperties["CurrentTestCaseName"];
            int lastDotIndex = testcase.LastIndexOf('.');
            string typeName = testcase.Substring(0, lastDotIndex);
            string methodName = testcase.Substring(lastDotIndex + 1);

            Assembly testcaseAssembly = Assembly.GetExecutingAssembly();
            var type = testcaseAssembly.GetType(typeName);
            if (type == null)
            {
                testSite.Assert.Fail(String.Format("Test case type name {0} does not exist in test case assembly {1}.", typeName, testcaseAssembly.FullName));
            }
            else
            {
                var method = type.GetMethod(methodName);
                var attributes = method.GetCustomAttributes(typeof(DescriptionAttribute), false);
                if (attributes == null)
                {
                    testSite.Assert.Fail("No description is provided for this case.");
                }
                else
                {
                    foreach (DescriptionAttribute attribute in attributes)
                    {
                        testSite.Log.Add(LogEntryKind.Comment, "[TestPurpose]" + attribute.Description);
                    }
                }
            }

        }
    }
}
