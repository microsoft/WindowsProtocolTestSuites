// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;

namespace Microsoft.Protocols.TestTools
{
    /// <summary>
    /// Represents all data type used in TxtToJSON.cs
    /// </summary>
    public class DataType
    {
        /// <summary>
        /// Represents summary for all test cases
        /// </summary>
        public class TestCasesSummary
        {
            /// <summary>
            /// Test cases information
            /// </summary>
            public List<TestCase> TestCases { get; set; }

            /// <summary>
            /// Test cases categories
            /// </summary>
            public List<string> TestCasesCategories { get; set; }

            /// <summary>
            /// Test cases classes
            /// </summary>
            public List<string> TestCasesClasses { get; set; }
        }

        /// <summary>
        /// Represents a test case
        /// </summary>
        public class TestCase
        {
            /// <summary>
            /// The name of the test case
            /// </summary>
            public string Name { get; set; }

            /// <summary>
            /// The result of test case
            /// </summary>
            public string Result { get; set; }

            /// <summary>
            /// The test class of the test case
            /// </summary>
            public string ClassType { get; set; }

            /// <summary>
            /// The categories of the test case
            /// </summary>
            public List<string> Category { get; set; }
        }

        /// <summary>
        /// Represents a detailed StandardOut log
        /// </summary>
        public class StandardOutDetail
        {
            /// <summary>
            /// The type of the StandardOut log
            /// </summary>
            public string Type { get; set; }

            /// <summary>
            /// The content of the StandardOut log
            /// </summary>
            public string Content { get; set; }
        }

        /// <summary>
        /// Represents detailed test case information
        /// </summary>
        public class TestCaseDetail
        {
            /// <summary>
            /// The name of the test case
            /// </summary>
            public string Name { get; set; }

            /// <summary>
            /// The start time of the test case
            /// </summary>
            public DateTimeOffset StartTime { get; set; }

            /// <summary>
            /// The end time of the test case
            /// </summary>
            public DateTimeOffset EndTime { get; set; }
            /// <summary>
            /// The result of the test case
            /// </summary>
            public string Result { get; set; }

            /// <summary>
            /// The source assembly of the test case
            /// </summary>
            public string Source { get; set; }
            /// <summary>
            /// The test class of the test case
            /// </summary>
            public string ClassType { get; set; }

            /// <summary>
            /// The Categories of the test case
            /// </summary>
            public List<string> Categories { get; set; }

            /// <summary>
            /// The ErrorStackTrace log of the test case
            /// </summary>
            public List<string> ErrorStackTrace { get; set; }
            /// <summary>
            /// The ErrorMessage log of the test case
            /// </summary>
            public List<string> ErrorMessage { get; set; }

            /// <summary>
            /// The StandardOut log of the test case
            /// </summary>
            public List<StandardOutDetail> StandardOut { get; set; }

            /// <summary>
            /// The Types in StandardOut log 
            /// </summary>
            public List<string> StandardOutTypes { get; set; }

            /// <summary>
            /// The path of the capture file if any
            /// </summary>
            public string CapturePath { get; set; }

            /// <summary>
            /// Set default value
            /// </summary>
            /// <param name="name">Test case name</param>
            /// <param name="startTime">Start time of the test case</param>
            /// <param name="endTime">End time of the test case</param>
            /// <param name="result">Result of the test case</param>
            /// <param name="source">Assembly of the test case</param>
            /// <param name="classType">Class of the test case</param>
            /// <param name="categories">Categories of the test case</param>
            public TestCaseDetail(string name, DateTimeOffset startTime, DateTimeOffset endTime, string result, string source, string classType, List<string> categories)
            {
                this.Name = name;
                this.StartTime = startTime;
                this.EndTime = endTime;
                this.Result = result;
                this.Source = source;
                this.ClassType = classType;
                this.Categories = categories;
                this.ErrorStackTrace = new List<string>();
                this.ErrorMessage = new List<string>();
                this.StandardOut = new List<StandardOutDetail>();
                this.StandardOutTypes = new List<string>();
                this.CapturePath = null;
            }
        }

        /// <summary>
        /// Represents detailed test case information for JSON.
        /// </summary>
        public class TestCaseDetailForJson
        {
            /// <summary>
            /// The name of the test case
            /// </summary>
            public string Name { get; set; }

            /// <summary>
            /// The start time of the test case
            /// </summary>
            public string StartTime { get; set; }

            /// <summary>
            /// The end time of the test case
            /// </summary>
            public string EndTime { get; set; }
            /// <summary>
            /// The result of the test case
            /// </summary>
            public string Result { get; set; }

            /// <summary>
            /// The source assembly of the test case
            /// </summary>
            public string Source { get; set; }
            /// <summary>
            /// The test class of the test case
            /// </summary>
            public string ClassType { get; set; }

            /// <summary>
            /// The Categories of the test case
            /// </summary>
            public List<string> Categories { get; set; }

            /// <summary>
            /// The ErrorStackTrace log of the test case
            /// </summary>
            public List<string> ErrorStackTrace { get; set; }
            /// <summary>
            /// The ErrorMessage log of the test case
            /// </summary>
            public List<string> ErrorMessage { get; set; }

            /// <summary>
            /// The StandardOut log of the test case
            /// </summary>
            public List<StandardOutDetail> StandardOut { get; set; }

            /// <summary>
            /// The Types in StandardOut log 
            /// </summary>
            public List<string> StandardOutTypes { get; set; }

            /// <summary>
            /// The path of the capture file if any
            /// </summary>
            public string CapturePath { get; set; }

            /// <summary>
            /// Update fields by provided test case detail.
            /// </summary>
            /// <param name="detail">Test case detail.</param>
            public void Update(TestCaseDetail detail)
            {
                Name = detail.Name;
                StartTime = ParseDateTimeOffset(detail.StartTime);
                EndTime = ParseDateTimeOffset(detail.EndTime);
                Result = detail.Result;
                Source = detail.Source;
                ClassType = detail.ClassType;
                Categories = detail.Categories;
                ErrorStackTrace = detail.ErrorStackTrace;
                ErrorMessage = detail.ErrorMessage;
                StandardOut = detail.StandardOut;
                StandardOutTypes = detail.StandardOutTypes;
                CapturePath = detail.CapturePath;
            }

            private string ParseDateTimeOffset(DateTimeOffset value)
            {
                return $"{value.UtcDateTime.ToString("yyyy-MM-dd HH:mm:ss")}.{value.Millisecond:d03}";
            }
        }
    }
}
