// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using Microsoft.Protocols.TestManager.Kernel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.Protocols.TestManager.CLI
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security", "CA1016 Mark assemblies with AssemblyVersionAttribute")]
    class Program
    {

        static void Main(string[] args)
        {
            try
            {
                Arguments arg = Arguments.Parse(args);

                if (arg.Help)
                {
                    PrintHelpText();
                    return;
                }

                Program p = new Program();
                p.Init();
                p.LoadTestSuite(arg.Profile);

                List<TestCase> testCases = (arg.Category != null) ? p.GetTestCases(arg.Category) : p.GetTestCases(arg.SelectedOnly);

                p.RunTestSuite(testCases);

                Utility.SortBy sortBy = Utility.SortBy.Name;
                CaseListItem.Separator separator = CaseListItem.Separator.Space;
                if (arg.SortBy != null) sortBy = Arguments.GetEnumArg<Utility.SortBy>("sortby", arg.SortBy);
                if (arg.Separator != null) separator = Arguments.GetEnumArg<CaseListItem.Separator>("separator", arg.Separator);
                string report = p.GenerateTextReport(arg.Report, arg.OutCome, sortBy, separator);

                if (arg.Report != null)
                {
                    using (StreamWriter sw = new StreamWriter(arg.Report))
                    {
                        sw.Write(report);
                    }
                }
                else
                {
                    Console.Write(report);
                }
            }
            catch (InvalidArgumentException e)
            {
                Console.Error.WriteLine("ERROR:");
                Console.Error.WriteLine(e.Message);
                Console.Error.WriteLine();
                PrintHelpText();
                Environment.Exit(-1);
            }
            catch (Exception e)
            {
                Console.Error.WriteLine("ERROR:");
                Console.Error.WriteLine(e.Message);
                Environment.Exit(-1);
            }
        }
        static void PrintHelpText()
        {
            var ass = Assembly.GetExecutingAssembly();
            System.Console.Write(StringResources.HelpText);
            
        }

        Utility util = new Utility();
        TestSuiteFamilies testSuites;

        /// <summary>
        /// Initialize
        /// </summary>
        public void Init()
        {
            testSuites = util.TestSuiteIntroduction;
        }

        /// <summary>
        /// Load test suite.
        /// </summary>
        /// <param name="filename">Filename of the profile</param>
        public void LoadTestSuite(string filename)
        {
            ProfileUtil profile = ProfileUtil.LoadProfile(filename);
            TestSuiteInfo tsinfo = null;
            foreach (var g in testSuites)
            {
                foreach (var info in g)
                {
                    if (profile.VerifyVersion(info.TestSuiteName, info.TestSuiteVersion))
                    {
                        tsinfo = info;
                        goto FindTestSuite;
                    }
                }
            }
            FindTestSuite:
            profile.Dispose();
            util.LoadTestSuiteConfig(tsinfo);
            util.LoadTestSuiteAssembly();
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
        public List<TestCase> GetTestCases(string category)
        {
            List<TestCase> testCaseList = new List<TestCase>();
            List<string> categories = new List<string>(category.Split(','));

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
            util.InitializeTestEngine();
            util.SyncRunByCases(testCases);
        }

        /// <summary>
        /// Generates text report.
        /// </summary>
        public string GenerateTextReport(string filename, string outcome, Utility.SortBy sortBy, CaseListItem.Separator separator)
        {
            string upperCaseOutcome = (outcome == null) ? null : outcome.ToUpper();
            bool pass = true, fail = true, inconclusive = false, notrun = false;
            if (outcome != null)
            {
                pass = upperCaseOutcome.Contains("PASS");
                fail = upperCaseOutcome.Contains("FAIL");
                inconclusive = upperCaseOutcome.Contains("INCONCLUSIVE");
                notrun = upperCaseOutcome.Contains("NOTRUN");
            }
            var list = util.GenerateTextCaseListItems(pass,fail, inconclusive, notrun);
            string report = Utility.GeneratePlainTextReport(list, true, sortBy, separator);
            return report;
        }
    }
}
