// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Protocols.TestSuites.FileSharing.Common.Adapter;
using Microsoft.Protocols.TestTools;
using Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Smb2;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;

namespace Microsoft.Protocols.TestSuites.FileSharing.Common.TestSuite
{
    public abstract partial class CommonTestBase : TestClassBase
    {
        protected TestConfigBase testConfig;

        protected ISutProtocolControlAdapter sutProtocolController;

        protected List<string> testDirectories = new List<string>();

        protected List<string> testFiles = new List<string>();


        protected string CurrentTestCaseName
        {
            get
            {
                string fullName = (string)BaseTestSite.TestProperties["CurrentTestCaseName"];
                return fullName.Split('.').LastOrDefault();
            }
        }

        protected Dictionary<string, Assembly> TestcaseAssemblies;

        protected override void TestInitialize()
        {
            base.TestInitialize();
            LogTestCaseDescription();
            sutProtocolController = BaseTestSite.GetAdapter<ISutProtocolControlAdapter>();
        }

        protected override void TestCleanup()
        {
            foreach (var directory in testDirectories)
            {
                try
                {
                    sutProtocolController.DeleteDirectory(Smb2Utility.GetShareName(directory), Smb2Utility.GetFileName(directory));
                }
                catch
                {
                }
            }

            foreach (var fileName in testFiles)
            {
                try
                {
                    sutProtocolController.DeleteFile(Smb2Utility.GetShareName(fileName), Smb2Utility.GetFileName(fileName));
                }
                catch
                {
                }
            }

            base.TestCleanup();
        }

        protected string GetTestFileName(string share)
        {
            string fileName = CurrentTestCaseName + "_" + Guid.NewGuid().ToString();
            testFiles.Add(Path.Combine(share, fileName));
            return fileName;
        }

        protected void AddTestFileName(string share, string fileName)
        {
            testFiles.Add(Path.Combine(share, fileName));
        }

        protected string CreateTestDirectory(string server, string share)
        {
            return CreateTestDirectory(string.Format(@"\\{0}\{1}", server, share));
        }

        protected string CreateTestDirectory(string share)
        {
            string testDirectory = CurrentTestCaseName + "_" + Guid.NewGuid().ToString();
            string testDirectoryFullPath = Path.Combine(share, testDirectory);
            testDirectories.Add(testDirectoryFullPath);
            sutProtocolController.CreateDirectory(share, testDirectory);
            BaseTestSite.Log.Add(LogEntryKind.Debug, "Create a directory {0} in the share {1}.",
                testDirectory, share);
            return testDirectory;
        }

        protected string GetTestDirectoryName(string share)
        {
            string directoryName = CurrentTestCaseName + "_" + Guid.NewGuid().ToString();
            testDirectories.Add(Path.Combine(share, directoryName));
            return directoryName;
        }

        protected void AddTestDirectoryName(string share, string directoryName)
        {
            testDirectories.Add(Path.Combine(share, directoryName));
        }

        protected uint DoUntilSucceed(Func<uint> func, TimeSpan timeout, string format, params object[] args)
        {
            DateTime endTime = DateTime.Now.Add(timeout);
            bool isUnderlyingConnectionClosed = false;
            uint retryCount = 0;
            uint result = 1;
            DateTime retryStart = DateTime.Now;
            string desc = string.Format(format, args);

            BaseTestSite.Log.Add(LogEntryKind.Debug, format, args);

            string lastException = null;
            do
            {
                if (retryCount != 0)
                {
                    BaseTestSite.Log.Add(LogEntryKind.Debug, "Retry {0}", retryCount);
                }
                try
                {
                    result = func();
                }
                catch (Exception e)
                {
                    lastException = e.Message;
                    BaseTestSite.Log.Add(LogEntryKind.Debug, "Throw an exception: {0}.", e.Message);
                    if (e.Message.Contains("Underlying connection has been closed.") || 
                        e.Message.Contains("An existing connection was forcibly closed by the remote host."))
                    {
                        BaseTestSite.Log.Add(LogEntryKind.Warning, "Stop retry because: {0}", e.Message);
                        isUnderlyingConnectionClosed = true;
                    }
                }
                if (result != 0)
                    Thread.Sleep(testConfig.RetryInterval);

                retryCount++;
            } while (DateTime.Now < endTime && result != 0 && !isUnderlyingConnectionClosed);

            TimeSpan retryDuration = DateTime.Now - retryStart;
            BaseTestSite.Log.Add(LogEntryKind.Debug, "Retry {0} after retry duration: {1}",
                result == 0 ? "succeed" : "fail",
                retryDuration.ToString());

            if (result != 0)
            {
                throw new InvalidOperationException(String.Format("Retry failed. The last exception is: {0}", lastException));
            }

            return result;
        }

        protected bool DoUntilSucceed(Func<bool> func, TimeSpan timeout, string format, params object[] args)
        {
            DateTime endTime = DateTime.Now.Add(timeout);
            bool isUnderlyingConnectionClosed = false;
            uint retryCount = 0;
            bool result = false;
            DateTime retryStart = DateTime.Now;

            BaseTestSite.Log.Add(LogEntryKind.Debug, format, args);
            string lastException = null;
            do
            {
                if (retryCount != 0)
                {
                    BaseTestSite.Log.Add(LogEntryKind.Debug, "Retry {0}", retryCount);
                }
                try
                {
                    result = func();
                }
                catch (Exception e)
                {
                    lastException = e.Message;
                    BaseTestSite.Log.Add(LogEntryKind.Debug, "Throw an exception: {0}.", e.Message);
                    if (e.Message.Contains("Underlying connection has been closed.") ||
                        e.Message.Contains("An existing connection was forcibly closed by the remote host."))
                    {
                        BaseTestSite.Log.Add(LogEntryKind.Warning, "Stop retry because: {0}", e.Message);
                        isUnderlyingConnectionClosed = true;
                    }
                }

                if (!result)
                    Thread.Sleep(testConfig.RetryInterval);

                retryCount++;
            } while (DateTime.Now < endTime && !result && !isUnderlyingConnectionClosed);

            TimeSpan retryDuration = DateTime.Now - retryStart;
            BaseTestSite.Log.Add(LogEntryKind.Debug, "Retry {0} after retry duration: {1}",
                result ? "succeed" : "fail",
                retryDuration.ToString());

            if (!result)
            {
                throw new InvalidOperationException(String.Format("Retry failed. The last exception is: {0}", lastException));
            }

            return result;
        }

        protected void DoUntilSucceed(Action func, TimeSpan timeout, string format, params object[] args)
        {
            DateTime endTime = DateTime.Now.Add(timeout);
            bool isUnderlyingConnectionClosed = false;
            uint retryCount = 0;
            bool result = false;
            DateTime retryStart = DateTime.Now;

            BaseTestSite.Log.Add(LogEntryKind.Debug, format, args);
            string lastException = null;
            do
            {
                if (retryCount != 0)
                {
                    BaseTestSite.Log.Add(LogEntryKind.Debug, "Retry {0}", retryCount);
                }
                try
                {
                    func();
                    result = true;
                }
                catch (Exception e)
                {
                    lastException = e.Message;
                    BaseTestSite.Log.Add(LogEntryKind.Debug, "Throw an exception: {0}.", e.Message);
                    if (e.Message.Contains("Underlying connection has been closed.") ||
                        e.Message.Contains("An existing connection was forcibly closed by the remote host."))
                    {
                        BaseTestSite.Log.Add(LogEntryKind.Warning, "Stop retry because: {0}", e.Message);
                        isUnderlyingConnectionClosed = true;
                    }
                }
                if (!result)
                    Thread.Sleep(testConfig.RetryInterval);

                retryCount++;
            } while (DateTime.Now < endTime && !result && !isUnderlyingConnectionClosed);

            TimeSpan retryDuration = DateTime.Now - retryStart;
            BaseTestSite.Log.Add(LogEntryKind.Debug, "Retry {0} after retry duration: {1}",
                result ? "succeed" : "fail",
                retryDuration.ToString());

            if (!result)
            {
                throw new InvalidOperationException(String.Format("Retry failed. The last exception is: {0}", lastException));
            }
        }

        /// <summary>
        /// Add test case description to log
        /// </summary>
        /// <param name="testcaseAssembly">Assembly where the test case existed.</param>
        protected void LogTestCaseDescription()
        {
            var testcase = (string)BaseTestSite.TestProperties["CurrentTestCaseName"];
            int lastDotIndex = testcase.LastIndexOf('.');
            string typeName = testcase.Substring(0, lastDotIndex);
            string methodName = testcase.Substring(lastDotIndex + 1);

            Assembly testcaseAssembly = GetTestcaseAssembly(BaseTestSite.TestAssemblyName);
            var type = testcaseAssembly.GetType(typeName);
            if (type == null)
            {
                BaseTestSite.Assert.Fail(String.Format("Test case type name {0} does not exist in test case assembly {1}.", typeName, testcaseAssembly.FullName));
            }
            else
            {
                var method = type.GetMethod(methodName);
                var attributes = method.GetCustomAttributes(typeof(DescriptionAttribute), false);
                if (attributes == null)
                {
                    BaseTestSite.Assert.Fail("No description is provided for this case.");
                }
                else
                {
                    foreach (DescriptionAttribute attribute in attributes)
                    {
                        BaseTestSite.Log.Add(LogEntryKind.Comment, attribute.Description);
                    }
                }
            }

        }

        /// <summary>
        /// Get test case assembly from assembly name.
        /// </summary>
        /// <param name="assemblyName">Assembly name where the test case exists.</param>
        /// <returns>Assembly</returns>
        protected Assembly GetTestcaseAssembly(string assemblyName)
        {
            Assembly testcaseAssembly;
            if (TestcaseAssemblies == null)
            {
                TestcaseAssemblies = new Dictionary<string, Assembly>();
            }

            if (TestcaseAssemblies.ContainsKey(assemblyName))
            {
                TestcaseAssemblies.TryGetValue(assemblyName, out testcaseAssembly);
            }
            else
            {
                testcaseAssembly = Assembly.LoadFrom(BaseTestSite.TestAssemblyName + ".dll");
                TestcaseAssemblies.Add(assemblyName, testcaseAssembly);
            }
            return testcaseAssembly;
        }
    }
}
