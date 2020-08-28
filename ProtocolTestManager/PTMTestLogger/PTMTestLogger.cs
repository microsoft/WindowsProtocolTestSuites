// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Client;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Logging;

namespace Microsoft.Protocols.TestTools
{
    [ExtensionUri("logger://PTMTestLogger")]
    [FriendlyName("PTM")]
    public class PTMTestLogger : ITestLoggerWithParameters
    {
        private string reportFolderPath; //The path to the report folder
        private string txtResultFolderPath; //The path to the result folder which stores all the txt files
        private string htmlResultFolderPath; //The path to the result folder which stores all the html files
        private string jsFolderPath; //The path to the result folder which stores all the js files
        private string captureFolderPath; //The path to the result folder which stores all the capture files

        private DateTimeOffset testRunStartTime = DateTimeOffset.MaxValue.ToLocalTime(); //The start time of the test run
        private DateTimeOffset testRunEndTime = DateTimeOffset.MinValue.ToLocalTime(); //The end time of the test run
        private TxtToJSON txtToJSON = new TxtToJSON();

        private ConcurrentDictionary<string, DataType.TestCaseDetail> results = new ConcurrentDictionary<string, DataType.TestCaseDetail>(); //All the test results

        private const string categoryPropertyKey = "MSTestDiscoverer.TestCategory";
        private const string scriptNode = "<script language=\"javascript\" type=\"text/javascript\">";
        private const string jsFileName_Functions = "functions.js";
        private const string indexHtmlName = "index.html";
        private const string txtResultFolderName = "Txt";
        private const string htmlResultFolderName = "Html";
        private const string jsFolderName = "js";
        private const string captureFolderName = "Captures";

        private const string outputFolderKey = "OutputFolder";

        private Dictionary<string, string> parametersDictionary;
        private string testResultsDirPath;

        /// <summary>
        /// Initializes the Test Logger.
        /// </summary>
        /// <param name="events">Events which can be registered for.</param>
        /// <param name="testResultsDirPath">Test Results Directory</param>
        public void Initialize(TestLoggerEvents events, string testResultsDirPath)
        {
            if (events == null)
            {
                throw new ArgumentNullException(nameof(events));
            }

            if (string.IsNullOrEmpty(testResultsDirPath))
            {
                throw new ArgumentNullException(nameof(testResultsDirPath));
            }

            // Register for the events.
            events.TestRunMessage += TestMessageHandler;
            events.TestResult += TestResultHandler;
            events.TestRunComplete += TestRunCompleteHandler;

            this.testResultsDirPath = testResultsDirPath;
            CreateReportFolder();
        }

        /// <summary>
        /// Initializes the Test Logger.
        /// </summary>
        /// <param name="events">Events which can be registered for.</param>
        /// <param name="parameters">Collection of parameters</param>
        public void Initialize(TestLoggerEvents events, Dictionary<string, string> parameters)
        {
            if (parameters == null)
            {
                throw new ArgumentNullException(nameof(parameters));
            }

            if (parameters.Count == 0)
            {
                throw new ArgumentException("No default parameters added", nameof(parameters));
            }

            this.parametersDictionary = parameters;
            this.Initialize(events, this.parametersDictionary[DefaultLoggerParameterNames.TestRunDirectory]);
        }

        #region Implement three events
        /// <summary>
        /// Called when a test message is received.
        /// </summary>
        private void TestMessageHandler(object sender, TestRunMessageEventArgs e)
        {
            switch (e.Level)
            {
                case TestMessageLevel.Informational:
                    break;

                case TestMessageLevel.Warning:
                    break;

                case TestMessageLevel.Error:
                    break;

                default:
                    break;
            }
        }

        /// <summary>
        /// Called when a test result is received.
        /// </summary>
        private void TestResultHandler(object sender, TestResultEventArgs e)
        {
            if (e.Result.Outcome == TestOutcome.NotFound)
            {
                return;
            }

            DataType.TestCaseDetail caseDetail = ConvertToTestCase(e.Result);
            results.TryAdd(caseDetail.Name, caseDetail);

            // Generate txt log file
            string txtFileName = Path.Combine(
                txtResultFolderPath,
                string.Format("{0}_{1}_{2}.txt",
                              caseDetail.StartTime.ToLocalTime().ToString("yyyy-MM-dd-HH-mm-ss"),
                              caseDetail.Result,
                              caseDetail.Name)
            );
            File.WriteAllText(txtFileName, ConstructCaseTxtReport(caseDetail));

            // Generate html log file
            string htmlFileName = Path.Combine(htmlResultFolderPath, $"{caseDetail.Name}.html");
            File.WriteAllText(htmlFileName, ConstructCaseHtml(caseDetail));
        }

        /// <summary>
        /// Called when a test run is completed.
        /// </summary>
        private void TestRunCompleteHandler(object sender, TestRunCompleteEventArgs e)
        {
            // Insert the necessary info used in index.html and copy it to report folder.
            File.WriteAllText(Path.Combine(reportFolderPath, indexHtmlName), ConstructIndexHtml(e));
        }

        #endregion

        /// <summary>
        /// Creates the report folders
        /// </summary>
        private void CreateReportFolder()
        {
            if (this.parametersDictionary != null)
            {
                var isoutputFolderParameterExists = this.parametersDictionary.TryGetValue(outputFolderKey, out string outputFolderValue);
                if (isoutputFolderParameterExists && !string.IsNullOrWhiteSpace(outputFolderValue))
                {
                    reportFolderPath = Path.Combine(testResultsDirPath, outputFolderValue);
                }
                else
                {
                    reportFolderPath = Path.Combine(testResultsDirPath, DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss"));
                }
            }
            else
            {
                reportFolderPath = Path.Combine(testResultsDirPath, DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss"));
            }

            Directory.CreateDirectory(reportFolderPath);

            txtResultFolderPath = Path.Combine(reportFolderPath, txtResultFolderName);
            Directory.CreateDirectory(txtResultFolderPath);

            htmlResultFolderPath = Path.Combine(reportFolderPath, htmlResultFolderName);
            Directory.CreateDirectory(htmlResultFolderPath);

            jsFolderPath = Path.Combine(reportFolderPath, jsFolderName);
            Directory.CreateDirectory(jsFolderPath);

            captureFolderPath = Path.Combine(reportFolderPath, captureFolderName);
            Directory.CreateDirectory(captureFolderPath);

            // Copy the two .js files to report folder, the two files don't need to be changed.
            File.WriteAllText(Path.Combine(jsFolderPath, jsFileName_Functions), Properties.Resources.functions);
        }

        /// <summary>
        /// Convert a vstest TestResult object to TestCaseDetail
        /// </summary>
        private DataType.TestCaseDetail ConvertToTestCase(TestResult result)
        {
            var eolSeparators = new char[] { '\r', '\n' };

            string caseName = !string.IsNullOrEmpty(result.TestCase.DisplayName) ? result.TestCase.DisplayName : result.TestCase.FullyQualifiedName.Split('.').Last();
            string outcome = result.Outcome == TestOutcome.Skipped ? "Inconclusive" : result.Outcome.ToString();
            string classType = result.TestCase.FullyQualifiedName.Split('.').Reverse().ElementAtOrDefault(1);
            List<string> testCategories = GetCustomPropertyValueFromTestCase(result.TestCase, categoryPropertyKey);

            var ret = new DataType.TestCaseDetail(caseName, result.StartTime, result.EndTime, outcome, result.TestCase.Source, classType, testCategories);

            if (!String.IsNullOrEmpty(result.ErrorStackTrace))
            {
                ret.ErrorStackTrace.AddRange(result.ErrorStackTrace.Split(eolSeparators, StringSplitOptions.RemoveEmptyEntries));
            }

            if (!String.IsNullOrEmpty(result.ErrorMessage))
            {
                ret.ErrorMessage.AddRange(result.ErrorMessage.Split(eolSeparators, StringSplitOptions.RemoveEmptyEntries));
            }

            var stdout = new List<string>();
            foreach (TestResultMessage m in result.Messages)
            {
                if (m.Category == TestResultMessage.StandardOutCategory && !String.IsNullOrEmpty(m.Text))
                {
                    stdout.AddRange(m.Text.Split(eolSeparators, StringSplitOptions.RemoveEmptyEntries));
                }
            }
            foreach (string line in stdout)
            {
                string pattern = @"\d{4}-\d{2}-\d{2} \d{2}:\d{2}:\d{2}.\d{3} \[(\w+)\] ";
                Regex r = new Regex(pattern, RegexOptions.IgnoreCase);

                Match m = r.Match(line);
                if (m.Success)
                {
                    string type = m.Groups[1].Value;
                    ret.StandardOut.Add(new DataType.StandardOutDetail()
                    {
                        Content = line,
                        Type = type
                    });
                }
                else
                {
                    // There must be at least one record in the list.
                    // Just to make the logger robust, let's do a check here.
                    // (But it won't happen)
                    int stdoutCount = ret.StandardOut.Count;
                    if (stdoutCount == 0)
                    {
                        continue;
                    }
                    else
                    {
                        var lastOutput = ret.StandardOut[stdoutCount - 1];
                        lastOutput.Content = lastOutput.Content + '\n' + line;
                    }
                }
            }

            ret.StandardOutTypes = ret.StandardOut.Select(output => output.Type).Distinct().ToList();

            return ret;
        }

        /// <summary>
        ///  Get Custom property values from test cases.
        /// </summary>
        /// <param name="testCase">TestCase object extracted from the TestResult</param>
        /// <param name="categoryID">Property Name from the list of properties in TestCase</param>
        /// <returns> list of properties</returns>
        public List<string> GetCustomPropertyValueFromTestCase(TestCase testCase, string categoryID)
        {
            var customProperty = testCase.Properties.FirstOrDefault(t => t.Id.Equals(categoryID));

            if (customProperty != null)
            {
                var cateogryValues = (string[])testCase.GetPropertyValue(customProperty);
                if (cateogryValues != null)
                {
                    return cateogryValues.ToList();
                }
                else
                {
                    return Enumerable.Empty<String>().ToList();
                }
            }

            return Enumerable.Empty<String>().ToList();
        }

        /// <summary>
        /// Inserts the corresponding script to the template html and generates the [testcase].html 
        /// </summary>
        private string ConstructCaseTxtReport(DataType.TestCaseDetail caseDetail)
        {
            StringBuilder sb = new StringBuilder();

            if (DateTimeOffset.Compare(testRunStartTime, caseDetail.StartTime.ToLocalTime()) > 0)
                testRunStartTime = caseDetail.StartTime.ToLocalTime();
            if (DateTimeOffset.Compare(testRunEndTime, caseDetail.EndTime.ToLocalTime()) < 0)
                testRunEndTime = caseDetail.EndTime.ToLocalTime();

            sb.AppendLine(caseDetail.Name);
            sb.AppendLine("Start Time: " + caseDetail.StartTime.ToLocalTime().ToString("MM/dd/yyyy HH:mm:ss"));
            sb.AppendLine("End Time: " + caseDetail.EndTime.ToLocalTime().ToString("MM/dd/yyyy HH:mm:ss"));
            sb.AppendLine("Result: " + caseDetail.Result);
            sb.AppendLine(caseDetail.Source);
            if (caseDetail.ErrorStackTrace.Count() > 0)
            {
                sb.AppendLine("===========ErrorStackTrace===========");
                caseDetail.ErrorStackTrace.ForEach(line => sb.AppendLine(line));
            }
            if (caseDetail.ErrorMessage.Count() > 0)
            {
                sb.AppendLine("===========ErrorMessage==============");
                caseDetail.ErrorMessage.ForEach(line => sb.AppendLine(line));
            }
            if (caseDetail.StandardOut.Count() > 0)
            {
                sb.AppendLine("===========StandardOut===============");
                caseDetail.StandardOut.ForEach(stdout => sb.AppendLine(stdout.Content));
            }

            return sb.ToString();
        }

        /// <summary>
        /// Constructs detailObj used in each [caseName].html
        /// </summary>
        private string ConstructDetailObj(DataType.TestCaseDetail caseDetail)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine();
            sb.Append("var detailObj=");
            sb.Append(txtToJSON.ConstructCaseDetail(caseDetail, captureFolderPath));
            sb.AppendLine(";");

            return sb.ToString();
        }

        /// <summary>
        /// Constructs listObj used in functions.js
        /// </summary>
        private string ConstructListAndSummaryObj(TestRunCompleteEventArgs e)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine();
            sb.AppendLine("var listObj = " + txtToJSON.TestCasesString(txtResultFolderPath, captureFolderPath, results) + ";");

            return sb.ToString();
        }

        /// <summary>
        /// Inserts the corresponding script to the template html and generates the [testcase].html 
        /// </summary>
        private string ConstructCaseHtml(DataType.TestCaseDetail caseDetail)
        {
            // Insert script to the template html (testcase.html)
            StringBuilder sb = new StringBuilder();
            sb.Append(ConstructDetailObj(caseDetail));
            sb.AppendLine("var titleObj = document.getElementById(\"right_sidebar_case_title\");");
            sb.Append(string.Format("CreateText(titleObj, \"{0}\");", caseDetail.Name));

            return InsertScriptToTemplate(Properties.Resources.testcase, sb.ToString());
        }

        /// <summary>
        /// Inserts the corresponding script to the template html and generates the index.html 
        /// </summary>
        private string ConstructIndexHtml(TestRunCompleteEventArgs e)
        {
            return InsertScriptToTemplate(Properties.Resources.index, ConstructListAndSummaryObj(e));
        }

        /// <summary>
        /// Inserts scripts to the template html files and returns the updated content
        /// </summary>
        private string InsertScriptToTemplate(string templateHtml, string scriptToInsert)
        {
            int posInsert = templateHtml.IndexOf(scriptNode);
            return templateHtml.Insert(posInsert + scriptNode.Length, scriptToInsert);       
        }
    }
}
