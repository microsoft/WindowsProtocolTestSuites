// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using Microsoft.Protocols.TestTools;
using Microsoft.Protocol.TestSuites.ADOD.Adapter;
using Microsoft.Protocol.TestSuites.ADOD.Adapter.Util;
using Microsoft.VisualStudio.TestTools.UnitTesting;


namespace Microsoft.Protocol.TestSuites.ADOD.TestSuite
{
    public abstract class ADODTestClassBase : TestClassBase
    {
        #region PTF Configurations

        // Protocol Test Framework configurations
        protected ADODTestConfig TestConfig;

        #endregion

        #region SUT Adapter Interfaces

        // SUT Control Adapters: Client and DC (Server)
        protected IClientControlAdapter ClientControlAdapter;
        protected IDCControlAdapter DCControlAdapter;

        #endregion
        
        #region MA Adapter Interfaces

        protected IMessageAnalyzerAdapter MaAdapter;

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

        #region Test Suite Initialize and Cleanup

        protected override void TestInitialize()
        {
            base.TestInitialize();
            this.TestSite.DefaultProtocolDocShortName = "MS-ADOD";

            //Initialize PTF Configurations
            this.TestConfig = new ADODTestConfig(this.TestSite);
            
            //Initialze SUT Adapters
            this.TestSite.Log.Add(LogEntryKind.Debug, "Initialize SUT control adapters.");
            this.ClientControlAdapter = (IClientControlAdapter)this.TestSite.GetAdapter(typeof(IClientControlAdapter));
            this.DCControlAdapter = (IDCControlAdapter)this.TestSite.GetAdapter(typeof(IDCControlAdapter));

            // Initialize Message Analyzer Adapter
            this.TestSite.Log.Add(LogEntryKind.Debug, "Initialize Message Analyzer adapters.");
            this.MaAdapter = (IMessageAnalyzerAdapter)this.TestSite.GetAdapter(typeof(IMessageAnalyzerAdapter));
            this.MaAdapter.Reset();

            // Initialize Test Environment
            this.TestSite.Log.Add(LogEntryKind.TestStep, "[INITIALIZE TEST ENVIRONMENT]");
            if (!this.TestConfig.TriggerDisabled)
            {
                this.TestSite.Assert.IsTrue(this.InitEnvironment(), "Initialize test case should succeed.");
            }
        }

        protected override void TestCleanup()
        {
            this.TestSite.Log.Add(LogEntryKind.TestStep, "[CLEAN UP TEST ENVIRONMENT]");
            if (!this.TestConfig.TriggerDisabled)
            {
                this.TestSite.Assert.IsTrue(this.InitEnvironment(), "Clean up test case should succeed.");
            }
            base.TestCleanup();
        }

        #endregion

        #region Test Suite Functions

        /// <summary>
        /// Try the function until it does not throw exceptions or the time is out.
        /// </summary>
        /// <param name="func">Specifies the pointer that points to the function that is under trial, if the function throws out exception we would try the function again until succeed or timeout.</param>
        /// <param name="timeout">Specifies the overall retry time span.</param>
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
            }while(!result && DateTime.Now < endTime);

            return result;
        }

        /// <summary>
        /// Clean up Test Environment
        /// </summary>
        /// <returns></returns>
        protected bool InitEnvironment()
        {
            bool cleanupSuccess = false;
            //use methodName to create logs.
            string methodName = System.Reflection.MethodInfo.GetCurrentMethod().Name;
            System.Environment.SetEnvironmentVariable("MS_ADOD_TESTCASENAME", methodName);

            //unjoin domain if the client is still domain joined
            this.TestSite.Log.Add(LogEntryKind.Debug, "Check if domain unjoined.");
            cleanupSuccess = this.ClientControlAdapter.IsUnjoinDomainSuccess();
            if (!cleanupSuccess)
            {
                this.TestSite.Log.Add(LogEntryKind.Comment, "Unjoining domain...");
                this.ClientControlAdapter.UnjoinDomain();
                cleanupSuccess = DoUntilSucceed(() => this.ClientControlAdapter.IsUnjoinDomainSuccess(),
                    this.TestConfig.Timeout,
                    TimeSpan.FromSeconds(5));
            }
            this.TestSite.Assert.IsTrue(cleanupSuccess, "Unjoin domain should succeed.");
            this.TestSite.Log.Add(LogEntryKind.Debug, "Clean up Computer Accounts from Active Directory.");
            if (ActiveDirectoryHelper.IsComputerExist(this.TestConfig.ClientComputerName))
            {
                ActiveDirectoryHelper.DeleteComputer(this.TestConfig.ClientComputerName);
            }

            //clean up information on dc
            this.TestSite.Log.Add(LogEntryKind.Debug, "Clean up Group Accounts from Active Directory.");
            if (ActiveDirectoryHelper.IsGroupExist(this.TestConfig.DomainNewGroup))
            {
                ActiveDirectoryHelper.DeleteGroup(this.TestConfig.DomainNewGroup);
            }
            this.TestSite.Log.Add(LogEntryKind.Debug, "Clean up User Accounts from Active Directory.");
            if (ActiveDirectoryHelper.IsUserExist(this.TestConfig.DomainNewUserUsername))
            {
                ActiveDirectoryHelper.DeleteUser(this.TestConfig.DomainNewUserUsername);
            }

            System.Environment.SetEnvironmentVariable("MS_ADOD_TESTCASENAME", string.Empty);
            return cleanupSuccess;
        }

        #endregion
    }
}
