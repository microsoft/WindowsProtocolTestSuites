// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Management;
using System.Text;

namespace Microsoft.Protocols.TestManager.Kernel
{
    public abstract class TestEngine
    {
        protected string EnginePath;
        private Logger logger;
        public string PipeName { get; set; }

        public List<string> TestAssemblies { get; set; }
        public string TestSetting { get; set; }
        public string WorkingDirectory { get; set; }
        public string ResultOutputFolder { get; set; }

        private List<TestCase> testcases;

        private List<TestCase> filteredTestcases;

        private bool ShowConsole;

        public TestEngine(string enginePath, bool showConsole)
        {
            EnginePath = enginePath;

            ShowConsole = showConsole;
        }

        public void InitializeLogger(List<TestCase> testcases)
        {
            logger = CreateLogger();
            logger.Initialize(testcases);
            this.testcases = testcases;
        }

        /// <summary>
        /// Create a logger.
        /// </summary>
        /// <returns>The logger.</returns>
        protected abstract Logger CreateLogger();

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
        /// Build vstest arguments by test cases.
        /// </summary>
        /// <param name="caseStack">Test cases to run.</param>
        /// <returns>The constructed args.</returns>
        abstract protected string ConstructVstestArgs(Stack<TestCase> caseStack);

        /// <summary>
        /// Build vstest arguments by filter.
        /// </summary>
        /// <param name="filterExpr">Filter expression</param>
        /// <returns>The constructed args.</returns>
        abstract protected string ConstructVstestArgs(string filterExpr);

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

            htmlResultChecker = GetHtmlResultChecker();
            htmlResultChecker.UpdateCase = logger.UpdateCaseFromHtmlLog;
            htmlResultChecker.Start(this.WorkingDirectory);

            var exception = new List<Exception>();
            try
            {
                while (caseStack != null && caseStack.Count > 0)
                {
                    string args = ConstructVstestArgs(caseStack);
                    var innerException = Run(args);
                    if (innerException != null)
                    {
                        exception.Add(innerException);
                    }
                }
            }
            catch (Exception e)
            {
                exception.Add(e);
            }
            ExecutionFinished(exception);
        }

        private Exception Run(string runArgs)
        {
            logger.GroupByOutcome.IsAborted = false;
            try
            {
                vstestProcess = new Process()
                {
                    StartInfo = new ProcessStartInfo()
                    {
                        WorkingDirectory = WorkingDirectory,
                        FileName = EnginePath,
                        UseShellExecute = false,
                        CreateNoWindow = !ShowConsole,
                        Arguments = runArgs
                    }
                };

                PipeSinkServer.ParseLogMessage = ParseLogMessage;
                PipeSinkServer.Start(PipeName);

                vstestProcess.Start();
                vstestProcess.WaitForExit();
            }
            catch (Exception exception)
            {
                PipeSinkServer.Stop();
                return exception;
            }
            return null;
        }

        private void ParseLogMessage(string message)
        {

            if (message.IndexOf(StringResource.InprogressTag) != -1 ||
                message.IndexOf(StringResource.PassedTag) != -1 ||
                message.IndexOf(StringResource.FailedTag) != -1 ||
                message.IndexOf(StringResource.InconclusiveTag) != -1)
            {
                string[] strings = message.Split(' ');
                string testCaseName = strings[strings.Length - 1];

                if (String.IsNullOrEmpty(testCaseName))
                {
                    return;
                }

                if (message.IndexOf(StringResource.InprogressTag) != -1)
                {
                    logger.GroupByOutcome.ChangeStatus(testCaseName, TestCaseStatus.Running);
                }
                else
                {
                    // Case status from Running -> Waiting.
                    // Waiting QT close or Html Report.
                    logger.GroupByOutcome.ChangeStatus(testCaseName, TestCaseStatus.Waiting);
                }
            }
        }


        private delegate void RunByFilterDelegate(string filterExpr);

        /// <summary>
        /// Runs the test suite using the given filter expression.
        /// </summary>
        /// <param name="filterExpr"></param>
        public void RunByFilter(string filterExpr)
        {
            var exception = new List<Exception>();
            try
            {
                htmlResultChecker = GetHtmlResultChecker();
                htmlResultChecker.UpdateCase = logger.UpdateCaseFromHtmlLog;
                htmlResultChecker.Start(this.WorkingDirectory);

                string args = ConstructVstestArgs(filterExpr);

                var innerException = Run(args);
                if (innerException != null)
                {
                    exception.Add(innerException);
                }
            }
            catch (Exception e)
            {
                exception.Add(e);
            }
            ExecutionFinished(exception);
        }

        /// <summary>
        /// Get HTML result checker.
        /// </summary>
        /// <returns>HTML result checker.</returns>
        protected abstract HtmlResultChecker GetHtmlResultChecker();

        private void ExecutionFinished(List<Exception> e)
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
            PipeSinkServer.Stop();
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

            logger.GroupByOutcome.IsAborted = true;
            // Sleep 0.5 second to make sure the IsAborted checking logic with updating Running and Waiting Cases' statuses will be executed before TerminateProcessTree.
            System.Threading.Thread.Sleep(500);
            TerminateProcessTree(vstestProcess.Id);
        }

        private void TerminateProcessTree(int pid)
        {
            try
            {
                var process = Process.GetProcessById(pid);

                process.CloseMainWindow();

                process.Kill();

                // Enumerate all child processes and terminate them.
                var searcher = new ManagementObjectSearcher($"select * from Win32_Process where ParentProcessId={pid}");
                var result = searcher.Get();
                foreach (var item in result)
                {
                    int childPid = Convert.ToInt32(item["ProcessId"]);
                    TerminateProcessTree(childPid);
                }

                process.WaitForExit();
            }
            catch (Exception ex)
            {
                Utility.LogException(new List<Exception> { ex });
            }
        }
    }

    public delegate void TestFinishedEvent(object sender, TestFinishedEventArgs args);

    public class TestFinishedEventArgs : EventArgs
    {
        public int Passed { get; set; }
        public int Failed { get; set; }
        public int Inconclusive { get; set; }
        public List<Exception> Exception { get; set; }

        public TestFinishedEventArgs(int pass, int fail, int inconclusive, List<Exception> e)
        {
            Passed = pass;
            Failed = fail;
            Inconclusive = inconclusive;
            Exception = e;
        }
    }
}
