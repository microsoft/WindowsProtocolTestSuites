// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Protocols.TestManager.Common;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Utilities;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.ExceptionServices;
using System.Runtime.Loader;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;

namespace Microsoft.Protocols.TestManager.Kernel
{
    public class TestEngine
    {
        private const string TestHostErrorLogFileName = "TestHostError.log";

        private static readonly TimeSpan ProcessTerminationTimeout = TimeSpan.FromSeconds(30);
        private static readonly TimeSpan StreamDrainTimeout = TimeSpan.FromSeconds(5);
        private static readonly TimeSpan TestStatusPropagationTimeout = TimeSpan.FromSeconds(2);

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

        private readonly ConcurrentDictionary<int, Process> activeProcesses = new ConcurrentDictionary<int, Process>();

        private readonly ConcurrentDictionary<string, StringBuilder> testLogs =
            new ConcurrentDictionary<string, StringBuilder>(StringComparer.Ordinal);

        private readonly ConcurrentDictionary<Guid, string> testNamesByPipeConnection =
            new ConcurrentDictionary<Guid, string>();

        private HashSet<string> testCaseFullNames = new HashSet<string>(StringComparer.Ordinal);

        public TestEngine(string enginePath)
        {
            EnginePath = enginePath;
        }

        public void InitializeLogger(List<TestCase> testcases)
        {
            tsLogManager = new TestSuiteLogManager();
            tsLogManager.Initialize(testcases);
            this.testcases = testcases;
            testCaseFullNames = testcases.Select(test => test.FullName).ToHashSet(StringComparer.Ordinal);
        }

        /// <summary>
        /// Retrieves the TestSuiteLogManager object.
        /// </summary>
        public TestSuiteLogManager GetTestSuiteLogManager()
        {
            return tsLogManager;
        }

        /// <summary>
        /// Gets the log lines received for a test during the current run.
        /// </summary>
        public string GetTestLogs(string testCaseFullName)
        {
            if (string.IsNullOrEmpty(testCaseFullName) ||
                !testLogs.TryGetValue(testCaseFullName, out var logs))
            {
                return null;
            }

            lock (logs)
            {
                return logs.Length > 0 ? logs.ToString() : null;
            }
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
            return ConstructVstestArgs(
                TestAssemblies,
                ResultOutputFolder,
                RunSettingsPath,
                PtfConfigDirectory,
                caseStack,
                useFullyQualifiedName: false);
        }

        private StringBuilder ConstructVstestArgs(
            IEnumerable<string> testAssemblies,
            string resultOutputFolder,
            string runSettingsPath,
            string ptfConfigDirectory,
            Stack<TestCase> caseStack,
            bool useFullyQualifiedName)
        {
            StringBuilder args = new StringBuilder();
            Uri wd = new Uri(WorkingDirectory);
            foreach (string file in testAssemblies)
            {
                args.AppendFormat("{0} ", wd.MakeRelativeUri(new Uri(file)).ToString().Replace('/', Path.DirectorySeparatorChar));
            }

            args.AppendFormat("--results-directory \"{0}\" ", resultOutputFolder);
            args.AppendFormat("--test-adapter-path {0} ", Directory.GetCurrentDirectory());
            args.AppendFormat("--logger html ");

            ConstructRunSettings(runSettingsPath, ptfConfigDirectory);
            args.AppendFormat("--settings \"{0}\" ", runSettingsPath);

            if (caseStack != null)
            {
                args.Append("--filter \"");
                TestCase testcase = caseStack.Pop();
                string filterProperty = useFullyQualifiedName ? "FullyQualifiedName" : "Name";
                string testCaseIdentifier = useFullyQualifiedName ? testcase.FullName : testcase.Name;
                args.AppendFormat("{0}={1}", filterProperty, EscapeFilterValue(testCaseIdentifier));
                while (caseStack.Count > 0)
                {
                    TestCase test = caseStack.Peek();
                    string testIdentifier = useFullyQualifiedName ? test.FullName : test.Name;
                    string escapedTestIdentifier = EscapeFilterValue(testIdentifier);
                    if (args.Length + escapedTestIdentifier.Length + filterProperty.Length + 3 + EnginePath.Length < 32000) //Max arg length for command line is 32699. For safety, use a shorter length, 32000.
                    {
                        test = caseStack.Pop();
                        args.AppendFormat("|{0}={1}", filterProperty, escapedTestIdentifier);
                    }
                    else break;
                }
                args.Append("\"");
            }

            Logger.AddLog(LogLevel.Debug, $"vstest arguments: {args}");
            return args;
        }

        private static string EscapeFilterValue(string value)
        {
            return FilterHelper.Escape(value)
                .Replace(",", "%2C")
                .Replace("\"", "%22");
        }

        /// <summary>
        /// Construct .runsettings file to specify the location of ptfconfig files.
        /// </summary>
        private void ConstructRunSettings(string runsettingsPath)
        {
            ConstructRunSettings(runsettingsPath, PtfConfigDirectory);
        }

        private void ConstructRunSettings(string runsettingsPath, string ptfConfigDirectory)
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
            valueAttr.Value = Path.Combine(Directory.GetCurrentDirectory(), ptfConfigDirectory);
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

        public TestExecutionPlanResult RunExecutionPlan(
            TestExecutionPlan plan,
            CancellationToken cancellationToken)
        {
            if (plan == null)
            {
                throw new ArgumentNullException(nameof(plan));
            }

            TestExecutionPlanResult result = null;
            OperationCanceledException cancellation = null;
            try
            {
                PipeSinkServer.ParseLogMessage = ParseLogMessage;
                PipeSinkServer.Start(PipeName);
                tsLogManager.GroupByOutcome.ConcurrentExecution = true;

                var runner = new TestExecutionPlanRunner();
                result = runner.RunAsync(
                    plan,
                    RunExecutionUnitAsync,
                    cancellationToken).GetAwaiter().GetResult();
            }
            catch (OperationCanceledException exception) when (cancellationToken.IsCancellationRequested)
            {
                cancellation = exception;
            }
            catch (Exception exception)
            {
                result = new TestExecutionPlanResult(
                    new[]
                    {
                        new TestExecutionFailure("Execution plan", "coordinator", exception),
                    });
            }
            finally
            {
                // The pipe sink has to be released before the run is finalized, otherwise late
                // log messages can still change test case status after the final status is computed.
                PipeSinkServer.Stop();
                PipeSinkServer.ParseLogMessage = null;
                tsLogManager.GroupByOutcome.ConcurrentExecution = false;
            }

            if (cancellation != null)
            {
                ExecutionFinished(new List<Exception> { cancellation });
                ExceptionDispatchInfo.Capture(cancellation).Throw();
            }

            ExecutionFinished(result.Failures.Select(failure => failure.Exception).ToList());
            return result;
        }

        private async Task RunExecutionUnitAsync(
            TestExecutionUnit unit,
            CancellationToken cancellationToken)
        {
            string unitOutputFolder = Path.Combine(ResultOutputFolder, unit.Id);
            Directory.CreateDirectory(unitOutputFolder);

            string runSettingsPath = Path.Combine(unitOutputFolder, $"{unit.Id}.runsettings");
            var cases = new Stack<TestCase>(unit.TestCases);

            while (cases.Count > 0)
            {
                cancellationToken.ThrowIfCancellationRequested();

                TestCase[] pendingCases = cases.ToArray();
                string arguments = ConstructVstestArgs(
                    unit.Assemblies,
                    unitOutputFolder,
                    runSettingsPath,
                    PtfConfigDirectory,
                    cases,
                    useFullyQualifiedName: true).ToString();
                int selectedCaseCount = pendingCases.Length - cases.Count;
                IReadOnlyList<TestCase> selectedCases = pendingCases
                    .Take(selectedCaseCount)
                    .ToList();

                ProcessExecutionResult processResult = await RunProcessAsync(
                    arguments,
                    cancellationToken,
                    diagnosticsFolder: unitOutputFolder,
                    trackAsCurrentProcess: false).ConfigureAwait(false);

                if (processResult.ExitCode != 0 &&
                    !await HaveAllTerminalStatusesAsync(selectedCases, cancellationToken).ConfigureAwait(false))
                {
                    throw new InvalidOperationException(
                        $"Execution unit '{unit.Id}' exited with code {processResult.ExitCode} " +
                        $"before every selected test case reported a final status. " +
                        $"See '{Path.Combine(unitOutputFolder, TestHostErrorLogFileName)}' for diagnostics.");
                }
            }
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

                PipeSinkServer.ParseLogMessage = ParseLogMessage;
                PipeSinkServer.Start(PipeName);
                RunProcessAsync(runArgs, cancellationToken).GetAwaiter().GetResult();
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

        private async Task<ProcessExecutionResult> RunProcessAsync(
            string runArgs,
            CancellationToken cancellationToken,
            string diagnosticsFolder = null,
            bool trackAsCurrentProcess = true)
        {
            using var process = new Process()
            {
                StartInfo = new ProcessStartInfo()
                {
                    WorkingDirectory = WorkingDirectory,
                    FileName = EnginePath,
                    UseShellExecute = false,
                    CreateNoWindow = false,
                    Arguments = "test " + runArgs,
                    RedirectStandardError = true,
                },
            };

            process.Start();

            // Concurrent execution units must not publish themselves as the current process,
            // otherwise the field ends up pointing at an arbitrary, already disposed process.
            if (trackAsCurrentProcess)
            {
                vstestProcess = process;
            }

            activeProcesses.TryAdd(process.Id, process);

            Task<string> errorTask = process.StandardError.ReadToEndAsync();

            try
            {
                await process.WaitForExitAsync(cancellationToken).ConfigureAwait(false);
            }
            catch (OperationCanceledException)
            {
                TerminateProcessTree(process.Id);

                // Drain the redirected stream before the process is disposed, otherwise the
                // pending read faults on a disposed handle as an unobserved task exception.
                await DrainAsync(errorTask, StreamDrainTimeout).ConfigureAwait(false);
                throw;
            }
            finally
            {
                activeProcesses.TryRemove(process.Id, out _);
            }

            string errorMessage = await DrainAsync(errorTask, StreamDrainTimeout).ConfigureAwait(false);
            if (process.ExitCode != 0)
            {
                Console.Error.WriteLine();
                Console.Error.WriteLine(StringResource.RunCaseError);
                Console.Error.WriteLine(errorMessage);

                ReportProcessError(diagnosticsFolder, process.ExitCode, errorMessage);
            }

            return new ProcessExecutionResult(process.ExitCode);
        }

        private static async Task<string> DrainAsync(Task<string> readTask, TimeSpan timeout)
        {
            try
            {
                Task completedTask = await Task.WhenAny(readTask, Task.Delay(timeout)).ConfigureAwait(false);
                if (ReferenceEquals(completedTask, readTask))
                {
                    return await readTask.ConfigureAwait(false);
                }

                ObserveFault(readTask);
                return null;
            }
            catch (Exception)
            {
                return null;
            }
        }

        private static void ObserveFault(Task task)
        {
            _ = task.ContinueWith(
                completedTask => _ = completedTask.Exception,
                CancellationToken.None,
                TaskContinuationOptions.ExecuteSynchronously | TaskContinuationOptions.OnlyOnFaulted,
                TaskScheduler.Default);
        }

        private static async Task<bool> HaveAllTerminalStatusesAsync(
            IReadOnlyList<TestCase> selectedCases,
            CancellationToken cancellationToken)
        {
            DateTime deadline = DateTime.UtcNow + TestStatusPropagationTimeout;
            do
            {
                if (selectedCases.All(testCase => IsTerminalStatus(testCase.Status)))
                {
                    return true;
                }

                await Task.Delay(50, cancellationToken).ConfigureAwait(false);
            }
            while (DateTime.UtcNow < deadline);

            return selectedCases.All(testCase => IsTerminalStatus(testCase.Status));
        }

        private static bool IsTerminalStatus(TestCaseStatus status)
        {
            return status == TestCaseStatus.Passed ||
                status == TestCaseStatus.Failed ||
                status == TestCaseStatus.Other;
        }

        private sealed class ProcessExecutionResult
        {
            public ProcessExecutionResult(int exitCode)
            {
                ExitCode = exitCode;
            }

            public int ExitCode { get; }
        }

        /// <summary>
        /// Persists the diagnostics of a failed test host process.
        /// A non zero exit code is also produced by ordinary test failures, so the run is not
        /// failed here. The output is persisted so that a unit which never executed any test
        /// case, for example because the test host could not start, is discoverable.
        /// </summary>
        private static void ReportProcessError(string diagnosticsFolder, int exitCode, string errorMessage)
        {
            try
            {
                if (!string.IsNullOrEmpty(diagnosticsFolder) && Directory.Exists(diagnosticsFolder))
                {
                    string diagnostics = string.IsNullOrWhiteSpace(errorMessage)
                        ? "The test host did not write diagnostic output to stderr."
                        : errorMessage;
                    File.AppendAllText(
                        Path.Combine(diagnosticsFolder, TestHostErrorLogFileName),
                        $"[{DateTime.Now:o}] Test host exited with code {exitCode}.{Environment.NewLine}{diagnostics}{Environment.NewLine}");
                }

                Utility.LogException(new List<Exception>
                {
                    new Exception($"Test host exited with code {exitCode}. {errorMessage ?? string.Empty}"),
                });
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine(ex.Message);
            }
        }

        private void ParseLogMessage(Guid connectionId, string message)
        {
            if (message.IndexOf(StringResource.InprogressTag) != -1 ||
                message.IndexOf(StringResource.PassedTag) != -1 ||
                message.IndexOf(StringResource.FailedTag) != -1 ||
                message.IndexOf(StringResource.InconclusiveTag) != -1)
            {
                // Status message: extract full test name
                string[] strings = message.Split(' ');
                string testCaseName = strings[strings.Length - 1];

                if (String.IsNullOrEmpty(testCaseName))
                {
                    return;
                }

                if (message.Contains(StringResource.InprogressTag))
                {
                    testNamesByPipeConnection[connectionId] = testCaseName;
                    var logs = testLogs.GetOrAdd(testCaseName, _ => new StringBuilder());
                    lock (logs)
                    {
                        logs.Clear();
                    }
                }

                AppendTestLog(testCaseName, message);

                // Update test status for UI
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
                    // Case status from Running -> Waiting
                    tsLogManager.GroupByOutcome.ChangeStatus(testCaseName, TestCaseStatus.Waiting);
                }

                if (!message.Contains(StringResource.InprogressTag))
                {
                    testNamesByPipeConnection.TryRemove(connectionId, out _);
                }
            }
            else if (TryGetTaggedTestLog(message, out var taggedTestName, out var taggedLog))
            {
                AppendTestLog(taggedTestName, taggedLog);
            }
            else if (testNamesByPipeConnection.TryGetValue(connectionId, out var testCaseName))
            {
                AppendTestLog(testCaseName, message);
            }
        }

        private bool TryGetTaggedTestLog(string message, out string testCaseName, out string log)
        {
            testCaseName = null;
            log = null;

            int separatorIndex = message.IndexOf('|');
            if (separatorIndex <= 0)
            {
                return false;
            }

            string candidate = message.Substring(0, separatorIndex);
            if (!testCaseFullNames.Contains(candidate))
            {
                return false;
            }

            testCaseName = candidate;
            log = message.Substring(separatorIndex + 1);
            return true;
        }

        private void AppendTestLog(string testCaseName, string message)
        {
            if (string.IsNullOrEmpty(testCaseName) || string.IsNullOrEmpty(message))
            {
                return;
            }

            var logs = testLogs.GetOrAdd(testCaseName, _ => new StringBuilder());
            lock (logs)
            {
                logs.AppendLine(message);
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

            foreach (var process in activeProcesses.Values)
            {
                TerminateProcessTree(process.Id);
            }

        }

        private void TerminateProcessTree(int pid)
        {
            try
            {
                var process = Process.GetProcessById(pid);
                process.Kill(true);

                // Never block the caller indefinitely; Abort runs on the request thread.
                process.WaitForExit((int)ProcessTerminationTimeout.TotalMilliseconds);
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
                                    Name = method.Name,
                                    Assembly = DllFileName
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
                    Assembly = info.Assembly,
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
