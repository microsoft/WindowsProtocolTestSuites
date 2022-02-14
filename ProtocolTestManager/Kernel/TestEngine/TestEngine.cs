// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Protocols.TestManager.Common;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Loader;
using System.Text;
using System.Threading;
using System.Xml;

namespace Microsoft.Protocols.TestManager.Kernel
{
    public class TestEngine
    {
        private string EnginePath;
        private TestSuiteLogManager tsLogManager;
        public string PipeName { get; set; }
        public List<string> TestAssemblies { get; set; }
        public string WorkingDirectory { get; set; }
        public string ResultOutputFolder { get; set; }

        public string PtfConfigDirectory { get; set; }

        public string RunSettingsPath { get; set; }

        private List<TestCase> testcases;

        private List<TestCase> filteredTestcases;

        private const int ProcessWaitInterval = 100;

        public TestEngine(string enginePath)
        {
            EnginePath = enginePath;
        }

        public void InitializeLogger(List<TestCase> testcases)
        {
            tsLogManager = new TestSuiteLogManager();
            tsLogManager.Initialize(testcases);
            this.testcases = testcases;
        }

        /// <summary>
        /// Retrieves the TestSuiteLogManager object.
        /// </summary>
        public TestSuiteLogManager GetTestSuiteLogManager()
        {
            return tsLogManager;
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
            tsLogManager.ApplyFilteredList(filteredTestcases);
        }

        /// <summary>
        /// Removes the filter.
        /// </summary>
        public void RemoveFilter()
        {
            tsLogManager.ApplyFilteredList(testcases);
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

            args.AppendFormat("--results-directory \"{0}\" ", ResultOutputFolder);
            args.AppendFormat("--test-adapter-path {0} ", Directory.GetCurrentDirectory());
            args.AppendFormat("--logger html ");

            ConstructRunSettings(RunSettingsPath);
            args.AppendFormat("--settings \"{0}\" ", RunSettingsPath);

            if (caseStack != null)
            {
                args.Append("--filter \"");
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

            Logger.AddLog(LogLevel.Debug, $"vstest arguments: {args}");
            return args;
        }

        /// <summary>
        /// Construct .runsettings file to specify the location of ptfconfig files.
        /// </summary>
        private void ConstructRunSettings(string runsettingsPath)
        {
            //<RunSettings>
            //  <TestRunParameters>
            //    <Parameter name = "PtfconfigDirectory" value="/Ptfconfig/" />
            //  </TestRunParameters>
            //</RunSettings>

            XmlDocument doc = new XmlDocument();


            XmlNode parameterNode = doc.CreateElement("Parameter");
            XmlAttribute nameAttr = doc.CreateAttribute("name");
            nameAttr.Value = "PtfconfigDirectory";
            parameterNode.Attributes.Append(nameAttr);
            XmlAttribute valueAttr = doc.CreateAttribute("value");
            valueAttr.Value = Path.Combine(Directory.GetCurrentDirectory(), this.PtfConfigDirectory);
            parameterNode.Attributes.Append(valueAttr);

            XmlNode testRunParametersNode = doc.CreateElement("TestRunParameters");
            testRunParametersNode.AppendChild(parameterNode);

            XmlNode runSettingsNode = doc.CreateElement("RunSettings");
            runSettingsNode.AppendChild(testRunParametersNode);

            doc.AppendChild(runSettingsNode);

            doc.Save(runsettingsPath);
        }

        /// <summary>
        /// Construct vstest args for discovery.
        /// </summary>
        /// <param name="filterExpression">The filter expression.</param>
        /// <param name="outputDirectory">The output directory.</param>
        /// <returns>The args.</returns>
        private StringBuilder ConstructVstestArgsForDiscovery(string filterExpression, string outputDirectory)
        {
            StringBuilder args = new StringBuilder();
            Uri wd = new Uri(WorkingDirectory);
            foreach (string file in TestAssemblies)
            {
                args.AppendFormat("{0} ", wd.MakeRelativeUri(new Uri(file)).ToString().Replace('/', Path.DirectorySeparatorChar));
            }

            if (!String.IsNullOrEmpty(filterExpression))
            {
                args.AppendFormat("--filter \"{0}\" ", filterExpression);
            }

            args.AppendFormat("--results-directory \"{0}\" ", outputDirectory);
            args.AppendFormat("--test-adapter-path {0} ", AppDomain.CurrentDomain.BaseDirectory);
            args.AppendFormat("--logger Discovery ");
            args.AppendFormat("--list-tests");

            Logger.AddLog(LogLevel.Debug, $"vstest arguments: {args}");
            return args;
        }

        private delegate void RunByCaseDelegate(Stack<TestCase> caseStack);

        Stack<TestCase> runningCaseStack = null;
        /// <summary>
        /// Runs the specified test cases in the test suite.
        /// </summary>
        /// <param name="caseStack">Test Cases</param>
        public void RunByCase(Stack<TestCase> caseStack)
        {
            using var cancellationTokenSource = new CancellationTokenSource();

            RunByCase(caseStack, cancellationTokenSource.Token);
        }

        /// <summary>
        /// Runs the specified test cases in the test suite.
        /// </summary>
        /// <param name="caseStack">The test cases.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        public void RunByCase(Stack<TestCase> caseStack, CancellationToken cancellationToken)
        {
            runningCaseStack = caseStack;

            var exception = new List<Exception>();
            try
            {
                while (caseStack != null && caseStack.Count > 0)
                {
                    cancellationToken.ThrowIfCancellationRequested();

                    StringBuilder args = ConstructVstestArgs(caseStack);
                    var innerException = Run(args.ToString(), cancellationToken);
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
            using var cancellationTokenSource = new CancellationTokenSource();

            return Run(runArgs, cancellationTokenSource.Token);
        }

        private Exception Run(string runArgs, CancellationToken cancellationToken)
        {
            try
            {
                cancellationToken.ThrowIfCancellationRequested();

                vstestProcess = new Process()
                {
                    StartInfo = new ProcessStartInfo()
                    {
                        WorkingDirectory = WorkingDirectory,
                        FileName = EnginePath,
                        UseShellExecute = false,
                        CreateNoWindow = false,
                        Arguments = "test " + runArgs,
                        RedirectStandardError = true,
                    }
                };

                PipeSinkServer.ParseLogMessage = ParseLogMessage;
                PipeSinkServer.Start(PipeName);

                vstestProcess.Start();

                while (true)
                {
                    if (cancellationToken.IsCancellationRequested)
                    {
                        TerminateProcessTree(vstestProcess.Id);

                        cancellationToken.ThrowIfCancellationRequested();
                    }

                    if (vstestProcess.WaitForExit(ProcessWaitInterval))
                    {
                        break;
                    }
                }

                int err = vstestProcess.ExitCode;
                if (err != 0)
                {
                    string errorMsg = vstestProcess.StandardError.ReadToEnd();
                    Console.Error.WriteLine();
                    Console.Error.WriteLine(StringResource.RunCaseError);
                    Console.Error.WriteLine(errorMsg);
                };
            }
            catch (Exception exception)
            {
                PipeSinkServer.Stop();
                Console.WriteLine(exception.Message);
                return exception;
            }

            PipeSinkServer.Stop();
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

                if (message.Contains(StringResource.InprogressTag))
                {
                    tsLogManager.GroupByOutcome.ChangeStatus(testCaseName, TestCaseStatus.Running);
                }
                else if (message.Contains(StringResource.FailedTag))
                {
                    tsLogManager.GroupByOutcome.ChangeStatus(testCaseName, TestCaseStatus.Failed);
                }
                else if (message.Contains(StringResource.PassedTag))
                {
                    tsLogManager.GroupByOutcome.ChangeStatus(testCaseName, TestCaseStatus.Passed);
                }
                else if (message.Contains(StringResource.InconclusiveTag))
                {
                    tsLogManager.GroupByOutcome.ChangeStatus(testCaseName, TestCaseStatus.Other);
                }
                else
                {
                    // Case status from Running -> Waiting.
                    // Waiting QT close or Html Report.
                    tsLogManager.GroupByOutcome.ChangeStatus(testCaseName, TestCaseStatus.Waiting);
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
                StringBuilder args = ConstructVstestArgs();
                args.AppendFormat("/TestCaseFilter:\"{0}\" ", filterExpr);
                var innerException = Run(args.ToString());
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

        private void ExecutionFinished(List<Exception> e)
        {
            if (TestFinished != null)
            {
                TestFinished(this,
                    new TestFinishedEventArgs(
                        tsLogManager.GroupByOutcome.PassedTestCases.TestCaseList.Count,
                        tsLogManager.GroupByOutcome.FailedTestCases.TestCaseList.Count,
                        tsLogManager.GroupByOutcome.OtherTestCases.TestCaseList.Count,
                        e));
            }

            tsLogManager.FinishTest();
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

            TerminateProcessTree(vstestProcess.Id);
        }

        private void TerminateProcessTree(int pid)
        {
            try
            {
                var process = Process.GetProcessById(pid);

                process.CloseMainWindow();

                process.Kill();

                process.WaitForExit();
            }
            catch (Exception ex)
            {
                Utility.LogException(new List<Exception> { ex });
            }
        }

        /// <summary>
        /// Load test cases.
        /// </summary>
        /// <param name="filterExpression">The filter expression. If it is null, no filter will be used.</param>
        /// <returns>The test case list.</returns>
        public List<TestCase> LoadTestCases(string filterExpression = null)
        {
            return filterExpression == null ? LoadAllTestCases() : LoadFilteredTestCases(filterExpression);
        }

        /// <summary>
        /// Load test cases of given dll files.
        /// </summary>
        /// <param name="dllPath">The dll path.</param>
        /// <returns>The loaded test cases.</returns>
        private IEnumerable<TestCase> LoadDlls(string[] dllPath)
        {
            if (dllPath.Length == 0) throw new Exception("TestEngine LoadDlls failed due to no dllPath.");
            var _testCaseList = new List<TestCase>();

            // We use individual AssemblyLoadContext for each testsuite, so we can isolate different versions of assemblies with the same name in different testsuites without exceptions.
            // e.g. the version of Microsoft.Protocols.TestTools.dll is 2.1.0.0 in RDPServer testsuite, but its version is 2.2.0.0 in FileServer testsuite.
            // After we got the assemblies information, we can unload the assemblies in current AssemblyLoadContext.
            string assembleDirPath = Directory.GetParent(dllPath[0]).FullName;
            AssemblyLoadContext alc = new CollectibleAssemblyLoadContext(dllPath[0]);
            alc.Resolving += (context, assembleName) =>
            {
                string assemblyPath = Path.Combine(assembleDirPath, $"{assembleName.Name}.dll");
                if (assemblyPath != null)
                    return context.LoadFromAssemblyPath(assemblyPath);
                return null;
            };

            foreach (string DllFileName in dllPath)
            {
                Assembly assembly = alc.LoadFromAssemblyPath(DllFileName);
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

                                var testcase = new TestCase()
                                {
                                    FullName = caseFullName,
                                    Category = categories.ToList(),
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

            alc.Unload();

            return _testCaseList;
        }

        /// <summary>
        /// Load all test cases.
        /// </summary>
        /// <returns>The test case list.</returns>
        private List<TestCase> LoadAllTestCases()
        {
            try
            {
                return LoadDlls(TestAssemblies.ToArray()).ToList();
            }
            catch (Exception e)
            {
                Utility.LogException(new List<Exception> { e });

                throw;
            }
        }

        /// <summary>
        /// Load filtered test cases.
        /// </summary>
        /// <param name="filterExpression">The filter expression. If it is null, no filter will be used.</param>
        /// <returns>The test case list.</returns>
        private List<TestCase> LoadFilteredTestCases(string filterExpression)
        {
            try
            {
                string tempPath = Path.GetTempFileName();

                File.Delete(tempPath);

                Directory.CreateDirectory(tempPath);

                StringBuilder args = ConstructVstestArgsForDiscovery(filterExpression, tempPath);

                vstestProcess = new Process()
                {
                    StartInfo = new ProcessStartInfo()
                    {
                        WorkingDirectory = WorkingDirectory,
                        FileName = EnginePath,
                        UseShellExecute = false,
                        CreateNoWindow = true,
                        Arguments = "test " + args,
                        RedirectStandardError = true,
                    }
                };

                vstestProcess.Start();
                vstestProcess.WaitForExit();
                if (vstestProcess.HasExited && vstestProcess.ExitCode != 0)
                {
                    string errorContent = vstestProcess.StandardError.ReadToEnd();
                    Directory.Delete(tempPath, true);
                    throw new Exception(errorContent);
                }
                string infoPath = Path.Combine(tempPath, "TestCaseInfo.json");

                var content = File.ReadAllText(infoPath);

                var infos = System.Text.Json.JsonSerializer.Deserialize<TestCaseInfo[]>(content);

                var result = infos.Select(info => new TestCase
                {
                    FullName = info.FullName,
                    Name = info.Name,
                    Category = info.Category.ToList(),
                    ToolTipOnUI = info.ToolTipOnUI,
                    Description = info.Description,
                }).ToList();

                Directory.Delete(tempPath, true);

                return result;
            }
            catch (Exception e)
            {
                Utility.LogException(new List<Exception> { e });

                throw;
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
