// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Protocols.TestTools;
using Microsoft.Protocol.TestSuites.Azod.Adapter;
using Microsoft.Protocol.TestSuites.Azod.Adapter.Util;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading;
using System.Diagnostics;
using System.IO;

namespace Microsoft.Protocol.TestSuites.Azod.TestSuite
{
    public abstract class AzodTestClassBase : TestClassBase
    {
        #region Adapter Interfaces
        //protected IAzodAdapter AzodAdapter;
        //protected ITriggerAdapter TriggerAdapter;
        protected ISutControlAdapter sutController;
        protected IMessageAnalyzerAdapter maAdapter;

        #endregion

        #region PTFConfig
        protected AzodTestConfig TestConfig;
        #endregion

        #region Variables
        protected ITestSite TestSite
        {
            get
            {
                return BaseTestSite;
            }
        }
        #endregion

        #region Initialize and Cleanup
         protected override void TestInitialize()
        {
            //Initialize all adapters
            base.TestInitialize();
            this.TestConfig = new AzodTestConfig(this.TestSite);
            this.sutController = BaseTestSite.GetAdapter<ISutControlAdapter>();

            maAdapter = BaseTestSite.GetAdapter<IMessageAnalyzerAdapter>();
            maAdapter.Reset();

            // Capture files location on the driver computer
            if (!Directory.Exists(TestConfig.LocalCapFilePath))
            {
                Directory.CreateDirectory(TestConfig.LocalCapFilePath);
            }
            // Logs Path for Powershell Adapter on the driver computer.
            if (!Directory.Exists(TestConfig.DriverLogPath))
            {
                Directory.CreateDirectory(TestConfig.DriverLogPath);
            }
            //other initialization setting if needed 
        }

        protected override void TestCleanup()
        {
            base.TestCleanup();
            //close connection if needed
        }

        protected void Logging()
        {
            StackTrace trace = new System.Diagnostics.StackTrace();
            StackFrame[] frames = trace.GetFrames();

            //find who is calling me
            System.Reflection.MethodBase method = frames[1].GetMethod();
            AzodTestAttribute.Site = this.Site;
            object[] attrs = method.GetCustomAttributes(false);
            if (attrs == null)
                return;

            foreach (object o in attrs)
            {
                //for out test attribute, invoke "Logging" method, for MSTEST attributes, do accordingly
                Type thisType = o.GetType();
                switch (thisType.Name)
                {
                    case "DescriptionAttribute":
                        {
                            Site.Log.Add(LogEntryKind.Comment, "Test description: " + typeof(DescriptionAttribute).GetProperty("Description").GetValue(o, null).ToString());
                        } break;
                    case "PriorityAttribute":
                        {
                            Site.Log.Add(LogEntryKind.Comment, "Implementation priority of this test case is: " + typeof(PriorityAttribute).GetProperty("Priority").GetValue(o, null).ToString());
                        } break;
                    case "TestCategoryAttribute":
                        {
                            IList<string> categories = (IList<string>)typeof(TestCategoryAttribute).GetProperty("TestCategories").GetValue(o, null);
                            if (categories.Count > 0)
                                Site.Log.Add(LogEntryKind.Comment, "This test case is belong to: " + categories[0] + "Test Category");
                        } break;
                    default:
                        if (thisType.BaseType == typeof(AzodTestAttribute))
                        {
                            try
                            {
                                thisType.GetMethod("Logging").Invoke(o, null);
                            }
                            catch (Exception e)
                            {
                                AzodTestAttribute.Site.Assert.Fail(e.InnerException.Message);                                
                            }
                        }
                        break;
                }

            }
        }
        #endregion

        /// <summary>
        /// Try the function until it does not throw exceptions or the time is out
        /// </summary>
        /// <param name="func">func is pointer that points to the function that is under trial, if the function throws out exception we would try the function again until succeed or timeout</param>
        /// <param name="timeout">timeout indicates the overall retry time span</param>
        /// <param name="retryInterval">retry interval</param>
        /// <returns></returns>
        protected bool DoUntilSucceed(Func<bool> func, TimeSpan timeout, TimeSpan retryInterval)
        {
            DateTime endTime = DateTime.Now.Add(timeout);

            bool result = false;
            do
            {
                try
                {
                    result = func();
                }
                catch
                {
                    Thread.Sleep(retryInterval);
                }
            } while (!result && DateTime.Now < endTime);

            return result;
        }

        /// <summary>
        /// Verify capture files with expected frames, return true if they match, otherwise, return false.
        /// </summary>
        /// <param name="captureFileFullPath">capture file path</param>
        /// <returns>true if match, otherwise, false</returns>
        public bool VerifyMessages(string captureFileFullPath)
        {
            string captureFileFullName = Path.GetFileName(captureFileFullPath);
            string captureExtension = Path.GetExtension(captureFileFullPath);
            string captureFileName = Path.GetFileNameWithoutExtension(captureFileFullPath);
            string searchPattern = captureFileName + "*" + captureExtension;

            bool isSuccess = true;

            this.TestSite.Log.Add(LogEntryKind.Comment, String.Format("Go through all captures with name {0} as prefix and verify the captures against expected sequences.", captureFileName));

            foreach (string fileName in Directory.GetFiles(this.TestConfig.LocalCapFilePath, searchPattern))
            {
                this.TestSite.Log.Add(LogEntryKind.Comment, String.Format("Verify capture file {0}.", fileName));

                string expectedSequenceFile = this.TestConfig.GetMAExpectedSequenceFile(Path.GetFileNameWithoutExtension(fileName));

                expectedSequenceFile = this.TestConfig.ExpectedSequenceFilePath + "\\" + expectedSequenceFile;

                if (!File.Exists(expectedSequenceFile))
                {
                    expectedSequenceFile = this.TestConfig.ExpectedSequenceFilePath + "\\" + this.TestConfig.GetMAExpectedSequenceFile(Path.GetFileNameWithoutExtension(captureFileName));
                }

                try
                {
                    maAdapter.Reset();
                    this.TestSite.Log.Add(LogEntryKind.Comment, String.Format("Load expected sequence file {0} for the capture file {1}.", expectedSequenceFile, fileName));
                    this.TestConfig.UpdateExpectedSequenceFile(expectedSequenceFile);

                    maAdapter.ConfigureAdapter(this.TestConfig.GetEndpointRoles(), fileName, captureFileName + "selected.matp", expectedSequenceFile);

                    this.TestSite.Log.Add(LogEntryKind.Comment, String.Format("Start to verify capture file {0} against sequence {1}.", fileName, expectedSequenceFile));
                    bool verifyStatus = maAdapter.ParseAndVerify(fileName, false);
                    isSuccess &= verifyStatus;

                    if (verifyStatus)
                    {
                        this.TestSite.Log.Add(LogEntryKind.CheckSucceeded, String.Format("Capture file {0} matches expected sequence {1}.xml.", fileName, expectedSequenceFile));
                        //break;
                    }
                    else
                    {
                        this.TestSite.Log.Add(LogEntryKind.CheckFailed, String.Format("Capture file {0} doesn't match expected sequence {1}.xml. Will try next expected sequence file.", fileName, expectedSequenceFile));
                    }
                }
                catch
                {
                    this.TestSite.Log.Add(LogEntryKind.CheckFailed, String.Format("Capture file {0} doesn't match expected sequence {1}.xml.", captureFileFullName, expectedSequenceFile));                    
                    return false;
                }
            }

            if (isSuccess)
            {
                this.TestSite.Log.Add(LogEntryKind.TestPassed, "Succeed to verify capture file.");
            }
            else
            {
                this.TestSite.Log.Add(LogEntryKind.TestFailed, "Failed to verify capture file.");
            }

            return isSuccess;
        }

        /// <summary>
        /// common function to run observer test case with specified caseName 
        /// </summary>
        /// <param name="caseName">the case to run</param>
        /// <returns>true for succeed, false for failed or inconclusive</returns>
        public void RunObserverTestCase(string caseName)
        {
            this.TestSite.Log.Add(LogEntryKind.Debug, "Get test case name: {0}", caseName);
            System.Environment.SetEnvironmentVariable("MS_AZOD_TESTCASENAME", caseName);
            
            //remove the log file before case runs
            string logFile=this.TestConfig.DriverLogPath+"\\"+caseName+".log";
            if (File.Exists(logFile))
            {
                File.Delete(logFile);
            }
            File.Create(logFile).Close();           

            #region verify messages
            Logging();

            bool isSuccess = false;

            string matpFile = string.Format(@"{0}{1}.matp", this.TestConfig.LocalCapFilePath, caseName);
            string matuFile = string.Format(@"{0}{1}.matu", this.TestConfig.LocalCapFilePath, caseName);

            bool matpExists = File.Exists(matpFile);
            bool matuExists = File.Exists(matuFile);

            if (!matpExists && !matuExists)
            {
                this.TestSite.Assert.Inconclusive(string.Format("Capture file for test case {0} doesn't exists. Please check the capture path {1}.", caseName, this.TestConfig.LocalCapFilePath));
            }

            isSuccess = VerifyMessages(matpFile);
            if (!isSuccess)
            {
                this.TestSite.Log.Add(LogEntryKind.TestFailed, "Capture files don't match all expected sequences. ");
            }
            else
            {
                this.TestSite.Log.Add(LogEntryKind.TestPassed, "Capture file matches the message sequences");
                return;
            }

            if (!isSuccess && matuExists)
            {
                isSuccess = VerifyMessages(matuFile);
                if (!isSuccess)
                {
                    this.TestSite.Log.Add(LogEntryKind.TestFailed, "Capture files don't match all expected sequences. ");
                }
                else
                {
                    this.TestSite.Log.Add(LogEntryKind.TestPassed, "Capture file matches the message sequences");
                    return;
                }
            }
            else if (!isSuccess && !File.Exists(this.TestConfig.LocalCapFilePath + caseName + ".matu"))
            {
                this.TestSite.Log.Add(LogEntryKind.TestFailed, String.Format("Cannot find capture files for case {0}.", caseName));
            }
            #endregion

            this.TestSite.Assert.IsTrue(isSuccess, "Capture file should match any one of expected sequence files.");
        }
    }
}
