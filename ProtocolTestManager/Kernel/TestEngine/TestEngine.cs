﻿// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.IO;

namespace Microsoft.Protocols.TestManager.Kernel
{
    public class TestEngine
    {
        private string EnginePath;
        private Logger logger;

        public List<string> TestAssemblies { get; set; }
        public string TestSetting { get; set; }
        public string WorkingDirectory { get; set; }

        private List<TestCase> testcases;

        private List<TestCase> filteredTestcases;

        public TestEngine(string enginePath)
        {
            EnginePath = enginePath;
        }

        public void InitializeLogger(List<TestCase> testcases)
        {
            logger = new Logger();
            logger.Initialize(testcases);
            this.testcases = testcases;
        }

        /// <summary>
        /// Retrieves the Logger object.
        /// </summary>
        /// <returns></returns>
        public Logger GetLogger()
        {
            return logger;
        }

        /// <summary>
        /// Filters test cases by keyword in name.
        /// </summary>
        /// <param name="keyword">Keyword</param>
        public void FilterByKeyword(string keyword)
        {
            filteredTestcases = new List<TestCase>();
            foreach (TestCase t in testcases)
            {
                if (t.Name.ToUpper().Contains(keyword.ToUpper()))
                {
                    filteredTestcases.Add(t);
                }
            }
            logger.ApplyFilteredList(filteredTestcases);
        }

        /// <summary>
        /// Removes the filter.
        /// </summary>
        public void RemoveFilter()
        {
            logger.ApplyFilteredList(testcases);
        }

        /// <summary>
        /// Begins to run the test suite using the specified filter expression.
        /// </summary>
        /// <param name="filterExpression"></param>
        public void BeginRunByFilter(string filterExpression)
        {
            RunByFilterDelegate runbyfilter = new RunByFilterDelegate(RunByFilter);
            runbyfilter.BeginInvoke(filterExpression, null, null);
        }

        /// <summary>
        /// Begins to run the specified test cases in the test suite.
        /// </summary>
        /// <param name="caseStack">Test Cases</param>
        public void BeginRunByCase(Stack<TestCase> caseStack)
        {
            RunByCaseDelegate runner = new RunByCaseDelegate(RunByCase);
            IAsyncResult result = runner.BeginInvoke(caseStack, null, null);
        }

        Process vstestProcess = null;

        /// <summary>
        /// Build vstest arguments. If caseStack is null, build common arguments only.
        /// </summary>
        /// <param name="caseStack">Test cases to run.</param>
        /// <returns>A StringBuilder</returns>
        private StringBuilder ConstructVstestArgs(Stack<TestCase> caseStack = null)
        {
            StringBuilder args = new StringBuilder();
            Uri wd = new Uri(WorkingDirectory);
            foreach (string file in TestAssemblies)
            {
                args.AppendFormat("{0} ", wd.MakeRelativeUri(new Uri(file)).ToString().Replace('/', Path.DirectorySeparatorChar));
            }
            args.AppendFormat("/Settings:\"{0}\" ", TestSetting);
            args.AppendFormat("/logger:html ");
            if (caseStack != null)
            {
                args.Append("/TestCaseFilter:\"");
                TestCase testcase = caseStack.Pop();
                args.AppendFormat("Name={0}", testcase.Name);
                while (caseStack.Count > 0)
                {
                    TestCase test = caseStack.Peek();
                    if (args.Length + test.Name.Length + 9 + EnginePath.Length < 32000) //Max arg length for command line is 32699. For safety, use a shorter length, 32000.
                    {
                        test = caseStack.Pop();
                        args.AppendFormat("|Name={0}", test.Name);
                    }
                    else break;
                }
                args.Append("\"");
            }
            return args;
        }

        private string runArgs;
        HtmlResultChecker htmlResultChecker;

        private delegate void RunByCaseDelegate(Stack<TestCase> caseStack);

        Stack<TestCase> runningCaseStack = null;
        /// <summary>
        /// Runs the specified test cases in the test suite.
        /// </summary>
        /// <param name="caseStack">Test Cases</param>
        public void RunByCase(Stack<TestCase> caseStack)
        {
            runningCaseStack = caseStack;
            htmlResultChecker = HtmlResultChecker.GetHtmlResultChecker();
            htmlResultChecker.UpdateCase = logger.UpdateCaseFromHtmlLog;
            htmlResultChecker.Start(this.WorkingDirectory);
            Exception exception = null;
            try
            {
                while (caseStack != null && caseStack.Count > 0)
                {
                    StringBuilder args = ConstructVstestArgs(caseStack);
                    runArgs = args.ToString();

                    vstestProcess = new Process()
                    {
                        StartInfo = new ProcessStartInfo()
                        {
                            WorkingDirectory = WorkingDirectory,
                            FileName = EnginePath,
                            UseShellExecute = false,
                            CreateNoWindow = false,
                            Arguments = runArgs
                        }
                    };
                    vstestProcess.Start();
                    vstestProcess.WaitForExit();
                }
            }
            catch (Exception e)
            {
                exception = e;
            }
            ExecutionFinished(exception);
        }

        private delegate void RunByFilterDelegate(string filterExpr);

        /// <summary>
        /// Runs the test suite using the given filter expression.
        /// </summary>
        /// <param name="filterExpr"></param>
        public void RunByFilter(string filterExpr)
        {
            Exception exception = null;
            try
            {
                htmlResultChecker = HtmlResultChecker.GetHtmlResultChecker();
                htmlResultChecker.UpdateCase = logger.UpdateCaseFromHtmlLog;
                htmlResultChecker.Start(this.WorkingDirectory);

                StringBuilder args = ConstructVstestArgs();
                args.AppendFormat("/TestCaseFilter:\"{0}\" ", filterExpr);
                runArgs = args.ToString();
                vstestProcess = new Process()
                {
                    StartInfo = new ProcessStartInfo()
                    {
                        WorkingDirectory = WorkingDirectory,
                        FileName = EnginePath,
                        UseShellExecute = false,
                        CreateNoWindow = false,
                        Arguments = runArgs
                    }
                };
                vstestProcess.Start();
                vstestProcess.WaitForExit();

            }
            catch (Exception e)
            {
                exception = e;
            }
            ExecutionFinished(exception);
        }

        private void ExecutionFinished(Exception e)
        {
            if (TestFinished != null)
            {
                TestFinished(this,
                    new TestFinishedEventArgs(
                        logger.GroupByOutcome.PassedTestCases.TestCaseList.Count,
                        logger.GroupByOutcome.FailedTestCases.TestCaseList.Count,
                        logger.GroupByOutcome.OtherTestCases.TestCaseList.Count,
                        e));
            }

            htmlResultChecker.Stop();
            logger.IndexHtmlFilePath = htmlResultChecker.IndexHtmlFilePath;
            logger.FinishTest();
            logger.GroupByOutcome.InProgressTestCases.Autohide = true;
        }

        /// <summary>
        /// Occurs when the test execution is finished.
        /// </summary>
        public event TestFinishedEvent TestFinished;

        /// <summary>
        /// Aborts the test execution.
        /// </summary>
        public void AbortExecution()
        {
            if (runningCaseStack != null) runningCaseStack.Clear();
            if (vstestProcess != null && !vstestProcess.HasExited) vstestProcess.Kill();
        }

    }

    public delegate void TestFinishedEvent(object sender, TestFinishedEventArgs args);

    public class TestFinishedEventArgs : EventArgs
    {
        public int Passed { get; set; }
        public int Failed { get; set; }
        public int Inconclusive { get; set; }
        public Exception Exception { get; set; }

        public TestFinishedEventArgs(int pass, int fail, int inconclusive, Exception e)
        {
            Passed = pass;
            Failed = fail;
            Inconclusive = inconclusive;
            Exception = e;
        }
    }
}
