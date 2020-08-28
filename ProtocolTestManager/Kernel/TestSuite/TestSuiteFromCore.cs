// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Protocols.TestManager.Common;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Web.Script.Serialization;

namespace Microsoft.Protocols.TestManager.Kernel
{
    /// <summary>
    /// Test suite targets to .NET Core.
    /// </summary>
    public class TestSuiteFromCore : TestSuite
    {
        // Wait 10 seconds before process end.
        private const int WaitTimeInMillisecondForProcessEnd = 10 * 1000;

        /// <summary>
        /// Load test case list from given dll path.
        /// </summary>
        /// <param name="dllPath">The dll path.</param>
        public override void LoadFrom(IEnumerable<string> dllPath)
        {
            IEnumerable<TestCaseInfo> testCaseInfos;

            try
            {
                testCaseInfos = RunTestCaseDiscover(dllPath);
            }
            catch
            {
                testCaseInfos = null;
            }

            if (testCaseInfos == null)
            {
                throw new Exception(StringResource.FailToRunTestCaseDiscover);
            }
            else
            {
                _testCaseList = testCaseInfos.Select(a => new TestCase
                {
                    FullName = a.FullName,
                    Name = a.Name,
                    Category = a.Category.ToList(),
                    ToolTipOnUI = a.ToolTipOnUI,
                    Description = a.Description,
                }).ToList();
            }
        }

        private IEnumerable<TestCaseInfo> RunTestCaseDiscover(IEnumerable<string> dllPath)
        {
            var inputFile = Path.GetTempFileName();

            var outputFile = Path.GetTempFileName();

            var process = new Process();

            try
            {
                string assemblyLocation = Assembly.GetExecutingAssembly().Location;

                var binPathInfo = Directory.GetParent(assemblyLocation);

                var binPath = binPathInfo.FullName;

                process.StartInfo.FileName = Path.Combine(binPath, StringResource.TestCaseDiscover);

                var serializer = new JavaScriptSerializer();

                var inputContent = serializer.Serialize(dllPath.ToArray());

                File.WriteAllText(inputFile, inputContent);

                process.StartInfo.Arguments = $@"""{inputFile}"" ""{outputFile}""";

                process.StartInfo.CreateNoWindow = true;

                process.Start();

                var ret = process.WaitForExit(WaitTimeInMillisecondForProcessEnd);

                if (!ret)
                {
                    return null;
                }

                var outputContent = File.ReadAllText(outputFile);

                var result = serializer.Deserialize<TestCaseInfo[]>(outputContent).ToList();

                return result;
            }
            finally
            {
                if (!process.HasExited)
                {
                    process.Kill();
                }

                process.Close();

                if (File.Exists(inputFile))
                {
                    File.Delete(inputFile);
                }

                if (File.Exists(outputFile))
                {
                    File.Delete(outputFile);
                }
            }
        }
    }
}
