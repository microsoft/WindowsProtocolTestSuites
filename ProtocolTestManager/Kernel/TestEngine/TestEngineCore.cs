// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Microsoft.Protocols.TestManager.Kernel
{
    /// <summary>
    /// Test engine for test suites based on .NET Core.
    /// </summary>
    public class TestEngineCore : TestEngine
    {
        /// <summary>
        /// Constructor of test engine core. 
        /// </summary>
        /// <param name="path">Path to test engine.</param>
        public TestEngineCore(string path) : base(path, true)
        {

        }

        /// <summary>
        /// Build vstest arguments by test cases.
        /// </summary>
        /// <param name="caseStack">Test cases to run.</param>
        /// <returns>The constructed args.</returns>
        protected override string ConstructVstestArgs(Stack<TestCase> caseStack)
        {
            StringBuilder args = new StringBuilder();

            ConstructCommonArgs(args);

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

            return args.ToString();
        }

        /// <summary>
        /// Build vstest arguments by filter.
        /// </summary>
        /// <param name="filterExpr">Filter expression</param>
        /// <returns>The constructed args.</returns>
        protected override string ConstructVstestArgs(string filterExpr)
        {
            StringBuilder args = new StringBuilder();

            ConstructCommonArgs(args);

            args.AppendFormat("--filter \"{0}\"", filterExpr);

            return args.ToString();
        }

        private void ConstructCommonArgs(StringBuilder args)
        {
            args.Append("test ");

            Uri wd = new Uri(WorkingDirectory);
            foreach (string file in TestAssemblies)
            {
                args.AppendFormat("{0} ", wd.MakeRelativeUri(new Uri(file)).ToString().Replace('/', Path.DirectorySeparatorChar));
            }
            args.AppendFormat("--results-directory {0} ", "HtmlTestResults");
            args.AppendFormat("--logger PTM ", ResultOutputFolder);
            args.AppendFormat("--test-adapter-path \"{0}\" ", Utility.GetPTMPath());
        }

        /// <summary>
        /// Get HTML result checker.
        /// </summary>
        /// <returns>HTML result checker.</returns>
        protected override HtmlResultChecker GetHtmlResultChecker()
        {
            return HtmlResultChecker.GetHtmlResultChecker(true);
        }

        /// <summary>
        /// Create a logger.
        /// </summary>
        /// <returns>The logger.</returns>
        protected override Logger CreateLogger()
        {
            var logger = new Logger(true);

            return logger;
        }
    }
}