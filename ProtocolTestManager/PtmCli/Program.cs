// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using CommandLine;
using CommandLine.Text;
using Microsoft.Protocols.TestManager.Kernel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Microsoft.Protocols.TestManager.CLI
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security", "CA1016 Mark assemblies with AssemblyVersionAttribute")]
    class Program
    {
        static void Main(string[] args)
        {
            var parser = new Parser(cfg => {
                cfg.CaseInsensitiveEnumValues = true;
                cfg.HelpWriter = Console.Error;
            });
            parser.ParseArguments<Options>(args)
                .WithParsed<Options>(opts => Run(opts))
                .WithNotParsed<Options>(errs => HandleArgumentError(errs));
        }

        static void Run(Options options)
        {
            bool isNewInstance = false;
            Mutex mutex = new Mutex(true, "{FE998190-5B44-4816-9A65-295E8A1EBBA1}", out isNewInstance);

            if (!isNewInstance)
            {
                Console.WriteLine("Protocol Test Manager or PtmCli is already running.");
                mutex = null;
                return;
            }

            try
            {
                Program p = new Program();
                p.Init();
                p.LoadTestSuite(options.Profile);

                List<TestCase> testCases = (options.Categories.Count() > 0) ? p.GetTestCases(options.Categories.ToList()) : p.GetTestCases(options.SelectedOnly);

                p.RunTestSuite(testCases);

                if (options.ReportFile == null)
                {
                    p.PrintTestReport(options.Outcome);
                }
                else
                {
                    p.SaveTestReport(options.ReportFile, options.ReportFormat, options.Outcome);
                }
                mutex.ReleaseMutex();
                mutex = null;
            }
            catch (Exception e)
            {
                Console.Error.WriteLine("ERROR:");
                Console.Error.WriteLine(e.Message);
                mutex.ReleaseMutex();
                mutex = null;
                Environment.Exit(-1);
            }
        }

        static void HandleArgumentError(IEnumerable<Error> errors)
        {
            Environment.Exit(-1);
        }

        Utility util;
        List<TestSuiteInfo> testSuites;

        /// <summary>
        /// Initialize
        /// </summary>
        public void Init()
        {
            util = new Utility();
            testSuites = util.TestSuiteIntroduction.SelectMany(tsFamily => tsFamily).ToList();
        }

        /// <summary>
        /// Load test suite.
        /// </summary>
        /// <param name="filename">Filename of the profile</param>
        public void LoadTestSuite(string filename)
        {
            TestSuiteInfo tsinfo;
            using (ProfileUtil profile = ProfileUtil.LoadProfile(filename))
            {
                tsinfo = testSuites.Find(ts => ts.TestSuiteName == profile.Info.TestSuiteName);
                if (tsinfo == null)
                {
                    throw new ArgumentException(String.Format(StringResources.UnknownTestSuiteMessage, profile.Info.TestSuiteName));
                }
            }

            util.LoadTestSuiteConfig(tsinfo);
            util.LoadTestSuiteAssembly();

            string newProfile;
            if (util.TryUpgradeProfileSettings(filename, out newProfile))
            {
                Console.WriteLine(String.Format(StringResources.PtmProfileUpgraded, newProfile));
                filename = newProfile;
            }
            util.LoadProfileSettings(filename);
        }

        /// <summary>
        /// Get test cases using profile
        /// </summary>
        /// <param name="selectedOnly">True to run only the test cases selected in the run page.</param>
        public List<TestCase> GetTestCases(bool selectedOnly)
        {
            List<TestCase> testCaseList = new List<TestCase>();
            foreach (TestCase testcase in util.GetSelectedCaseList())
            {
                if (!selectedOnly || testcase.IsChecked) testCaseList.Add(testcase);
            }
            return testCaseList;
        }

        /// <summary>
        /// Get test cases using category paramter
        /// </summary>
        /// <param name="category">The specific category of test cases to run</param>
        public List<TestCase> GetTestCases(List<string> categories)
        {
            List<TestCase> testCaseList = new List<TestCase>();

            Filter filter = new Filter(categories, RuleType.Selector);
            foreach (TestCase testcase in util.GetTestSuite().TestCaseList)
            {
                if (filter.FilterTestCase(testcase.Category)) testCaseList.Add(testcase);
            }
            return testCaseList;
        }

        /// <summary>
        /// Run test suite
        /// </summary>
        /// <param name="testCases">The list of test cases to run</param>
        public void RunTestSuite(List<TestCase> testCases)
        {
            using (ProgressBar progress = new ProgressBar())
            {
                util.InitializeTestEngine();

                int total = testCases.Count;
                int executed = 0;

                Logger logger = util.GetLogger();
                logger.GroupByOutcome.UpdateTestCaseList = (group, runningcase) =>
                {
                    executed++;
                    progress.Update((double)executed / total, $"Executing {runningcase.Name}");
                };

                progress.Update(0, "Loading test suite");
                util.SyncRunByCases(testCases);
            }
            Console.WriteLine("Finish running test cases.");
        }

        /// <summary>
        /// Print plain test report to console.
        /// </summary>
        public void PrintTestReport(IEnumerable<Outcome> outcomes)
        {
            bool pass = outcomes.Contains(Outcome.Pass);
            bool fail = outcomes.Contains(Outcome.Fail);
            bool inconclusive = outcomes.Contains(Outcome.Inconclusive);

            var testcases = util.SelectTestCases(pass, fail, inconclusive, false);
            PlainReport report = new PlainReport(testcases);
            Console.Write(report.GetPlainReport());
        }

        /// <summary>
        /// Save test report to disk.
        /// </summary>
        public void SaveTestReport(string filename, ReportFormat format, IEnumerable<Outcome> outcomes)
        {
            bool pass = outcomes.Contains(Outcome.Pass);
            bool fail = outcomes.Contains(Outcome.Fail);
            bool inconclusive = outcomes.Contains(Outcome.Inconclusive);

            var testcases = util.SelectTestCases(pass, fail, inconclusive, false);

            TestReport report = TestReport.GetInstance(format.ToString(), testcases);
            if (report == null)
            {
                throw new Exception(String.Format(StringResources.UnknownReportFormat, format.ToString()));
            }

            report.ExportReport(filename);
        }
    }
}
