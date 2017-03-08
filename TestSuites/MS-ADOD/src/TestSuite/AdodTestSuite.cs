// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using Microsoft.Protocols.TestTools;
using Microsoft.Protocol.TestSuites.ADOD.Adapter;
using Microsoft.Protocol.TestSuites.ADOD.Adapter.Util;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Globalization;

namespace Microsoft.Protocol.TestSuites.ADOD.TestSuite
{
    [TestClass]
    public class ADODTestSuite : ADODTestClassBase
    {
        #region Test Suite Initialize

        /// <summary>
        /// Class Initialize
        /// </summary>
        /// <param name="testContext"></param>
        [ClassInitialize()]
        public static void ClassInitialize(TestContext testContext)
        {
            ADODTestClassBase.Initialize(testContext);
        }

        /// <summary>
        /// Class cleanup
        /// </summary>
        [ClassCleanup()]
        public static void ClassCleanup()
        {
            ADODTestClassBase.Cleanup();
        }

        #endregion

        #region [MS-ADOD] section 3.1 Domain-join Examples

        /// <summary>
        /// Locate Domain Controller
        /// </summary>
        [TestMethod]
        [Priority(0)]
        [Description("Locate DC DNS")]
        [TestCategory("BVT")]
        [DeploymentItem(@"C:\Program Files\Microsoft Message Analyzer\Microsoft.WindowsAzure.Storage.dll")]
        public void LocateDomainControllerDNS()
        {
            #region LocalVariables

            string testName = string.Empty;
            string testResult = string.Empty;
            bool testPass = false;

            #endregion

            #region Initialize

            this.TestSite.Log.Add(LogEntryKind.TestStep, "[INITIALIZE TEST CASE]");
            testName = System.Reflection.MethodInfo.GetCurrentMethod().Name;
            this.TestSite.Log.Add(LogEntryKind.Debug, "Get test case name: {0}", testName);
            System.Environment.SetEnvironmentVariable("MS_ADOD_TESTCASENAME", testName);
            this.TestSite.Log.Add(LogEntryKind.Debug, "Load MA capture verification information from PTFConfig and Message Analyzer Adapter.");
            this.TestConfig.UpdateExpectedSequenceFile(this.TestConfig.GetMAExpectedSequenceFile(testName));
            MaAdapter.ConfigureAdapter(
                    this.TestConfig.GetEndpointRoles(),
                    this.TestConfig.LocalCapFileName,
                    this.TestConfig.LocalCapFileName.Replace(".matp", "-Selected.matp"),
                    this.TestConfig.GetMAExpectedSequenceFile(testName));

            #endregion

            if (!this.TestConfig.TriggerDisabled)
            {
                this.TestSite.Log.Add(LogEntryKind.TestStep, "[START TEST CASE]");

                #region Start Capture

                this.TestSite.Log.Add(LogEntryKind.Debug, "Start Message Analyzer on driver computer.");
                MaAdapter.StartCapture(false);
                this.TestSite.Assert.IsTrue(true, "Start Message Analyzer on driver computer should succeed.");

                #endregion

                #region Trigger

                this.TestSite.Log.Add(LogEntryKind.Debug, "Trigger client to locate domain controller.");
                testResult = this.ClientControlAdapter.LocateDomainController();
                this.TestSite.Assert.IsNotNull(testResult, "Locating domain controller should succeed.");
                this.TestSite.Log.Add(LogEntryKind.Debug, "Domain Controllers located: {0}", testResult);

                #endregion

                #region Verify

                this.TestSite.Log.Add(LogEntryKind.Debug, "Verify test result: {0}", testResult);
                testPass = testResult.ToLower(CultureInfo.InvariantCulture).Contains(this.TestConfig.PDCComputerName.ToLower(CultureInfo.InvariantCulture));
                this.TestSite.Assert.IsTrue(testPass, "Locate domain controller should succeed.");

                #endregion

                #region Stop Capture

                this.TestSite.Log.Add(LogEntryKind.Debug, "Stop Message Analyzer on driver computer.");
                MaAdapter.StopCapture();
                this.TestSite.Assert.IsTrue(true, "Stop Message Analyzer on driver computer should succeed.");

                #endregion
            }

            #region Verify Capture File

            this.TestSite.Log.Add(LogEntryKind.TestStep, "[START VERIFY CAPTURE]");
            this.TestSite.Log.Add(LogEntryKind.Debug, "Start to verify capture file.");
            MaAdapter.ParseAndVerify(this.TestConfig.LocalCapFileName, true);

            #endregion

            this.TestSite.Log.Add(LogEntryKind.TestStep, "[STOP TEST CASE]");
        }

        /// <summary>
        /// Locate Domain Controller NetBIOS
        /// </summary>
        [TestMethod]
        [Priority(0)]
        [Description("Locate DC NetBIOS")]
        [TestCategory("non BVT")]
        [TestCategory("pre-win8")]
        [DeploymentItem(@"C:\Program Files\Microsoft Message Analyzer\Microsoft.WindowsAzure.Storage.dll")]
        public void LocateDomainControllerNetbios()
        {
            #region LocalVariables

            string testName = string.Empty;
            string testResult = string.Empty;
            bool testPass = false;

            #endregion

            #region Initialize

            this.TestSite.Log.Add(LogEntryKind.TestStep, "[INITIALIZE TEST CASE]");
            testName = System.Reflection.MethodInfo.GetCurrentMethod().Name;
            this.TestSite.Log.Add(LogEntryKind.Debug, "Get test case name: {0}", testName);
            System.Environment.SetEnvironmentVariable("MS_ADOD_TESTCASENAME", testName);
            this.TestSite.Log.Add(LogEntryKind.Debug, "Load MA capture verification information from PTFConfig and Message Analyzer Adapter.");
            this.TestConfig.UpdateExpectedSequenceFile(this.TestConfig.GetMAExpectedSequenceFile(testName));
            MaAdapter.ConfigureAdapter(
                    this.TestConfig.GetEndpointRoles(),
                    this.TestConfig.LocalCapFileName,
                    this.TestConfig.LocalCapFileName.Replace(".matp", "-Selected.matp"),
                    this.TestConfig.GetMAExpectedSequenceFile(testName));

            #endregion

            if (!this.TestConfig.TriggerDisabled)
            {
                this.TestSite.Log.Add(LogEntryKind.TestStep, "[START TEST CASE]");

                #region Start Message Analyzer

                this.TestSite.Log.Add(LogEntryKind.Debug, "Start Message Analyzer on driver computer.");
                MaAdapter.StartCapture(false);
                this.TestSite.Assert.IsTrue(true, "Start Message Analyzer on driver computer should succeed.");

                #endregion

                #region Trigger

                this.TestSite.Log.Add(LogEntryKind.Debug, "Trigger client to locate domain controller.");
                testResult = this.ClientControlAdapter.LocateDomainController();
                this.TestSite.Assert.IsNotNull(testResult, "Locating domain controller should succeed.");
                this.TestSite.Log.Add(LogEntryKind.Debug, "Domain Controllers located: {0}", testResult);

                #endregion

                #region Verify

                this.TestSite.Log.Add(LogEntryKind.Debug, "Verify test result: {0}", testResult);
                testPass = testResult.ToLower(CultureInfo.InvariantCulture).Contains(this.TestConfig.NetbiosComputerName.ToLower(CultureInfo.InvariantCulture));
                this.TestSite.Assert.IsTrue(testPass, "Locate domain controller should succeed.");

                #endregion

                #region Stop Message Analyzer

                this.TestSite.Log.Add(LogEntryKind.Debug, "Stop Message Analyzer on driver computer.");
                MaAdapter.StopCapture();
                this.TestSite.Assert.IsTrue(true, "Stop Message Analyzer on driver computer should succeed.");

                #endregion
            }

            #region Verify Capture File

            this.TestSite.Log.Add(LogEntryKind.TestStep, "[START VERIFY CAPTURE]");
            this.TestSite.Log.Add(LogEntryKind.Debug, "Start to verify capture file.");
            MaAdapter.ParseAndVerify(this.TestConfig.LocalCapFileName, true);

            #endregion

            this.TestSite.Log.Add(LogEntryKind.TestStep, "[STOP TEST CASE]");
        }

        /// <summary>
        /// Join Domain by Creating An Account
        /// </summary>
        [TestMethod]
        [Priority(0)]
        [Description("Join Domain by Creating An Account")]
        [TestCategory("BVT")]
        [DeploymentItem(@"C:\Program Files\Microsoft Message Analyzer\Microsoft.WindowsAzure.Storage.dll")]
        public void JoinDomainCreateAcct()
        {
            if (this.TestConfig.ClientOSVersion <= 6.0)
            {
                JoinDomainCreateAcctSAMR();
            }
            else
            {
                JoinDomainCreateAcctLDAP();
            }
        }


        /// <summary>
        /// Join Domain by Creating An Account Using LDAP
        /// </summary>
        private void JoinDomainCreateAcctLDAP()
        {
            #region LocalVariables

            string testName = string.Empty;
            bool testPass = false;
            PingClient pingClient = new PingClient(this.TestConfig.ClientIP);

            #endregion

            #region Initialize

            this.TestSite.Log.Add(LogEntryKind.TestStep, "[INITIALIZE TEST CASE]");
            testName = System.Reflection.MethodInfo.GetCurrentMethod().Name;
            this.TestSite.Log.Add(LogEntryKind.Debug, "Get test case name: {0}", testName);
            System.Environment.SetEnvironmentVariable("MS_ADOD_TESTCASENAME", testName);
            this.TestSite.Log.Add(LogEntryKind.Debug, "Load MA capture verification information from PTFConfig and Message Analyzer Adapter.");
            this.TestConfig.UpdateExpectedSequenceFile(this.TestConfig.GetMAExpectedSequenceFile(testName));
            MaAdapter.ConfigureAdapter(
                    this.TestConfig.GetEndpointRoles(),
                    this.TestConfig.LocalCapFileName,
                    this.TestConfig.LocalCapFileName.Replace(".matp", "-Selected.matp"),
                    this.TestConfig.GetMAExpectedSequenceFile(testName));

            #endregion

            if (!this.TestConfig.TriggerDisabled)
            {
                this.TestSite.Log.Add(LogEntryKind.TestStep, "[START TEST CASE]");

                #region Start Message Analyzer

                this.TestSite.Log.Add(LogEntryKind.Debug, "Start Message Analyzer on driver computer.");
                MaAdapter.StartCapture(false);
                this.TestSite.Assert.IsTrue(true, "Start Message Analyzer on driver computer should succeed.");

                #endregion

                #region Trigger

                this.TestSite.Log.Add(LogEntryKind.Debug, "Trigger client to join domain by creating an account using LDAP.");
                testPass = this.ClientControlAdapter.JoinDomainCreateAcctLDAP();
                this.TestSite.Assert.IsTrue(testPass, "Join Domain by creating an account using LDAP should succeed.");
                if (this.TestConfig.ClientOperatingSystem.Equals("Windows"))
                {
                    testPass = DoUntilSucceed(() => pingClient.PingFailure(),
                        this.TestConfig.Timeout,
                        TimeSpan.FromSeconds(0.5));
                    this.TestSite.Assert.IsTrue(testPass, "Start Rebooting...");
                    Thread.Sleep(this.TestConfig.JoinDomainCreateAcctLDAPSleepTime);
                }

                #endregion

                #region Verify Server

                this.TestSite.Log.Add(LogEntryKind.Debug, "Verify server status.");
                testPass = this.DCControlAdapter.IsJoinDomainSuccess(this.TestConfig.ClientComputerName);
                this.TestSite.Assert.IsTrue(testPass, "Server status update should succeed after joining domain.");
                this.TestSite.Log.Add(LogEntryKind.Debug, "Join Domain Server Status Checked: (1)The client computer account has been successfully added to the Active Directory.");

                #endregion

                #region Verify Client

                this.TestSite.Log.Add(LogEntryKind.Debug, "Verify client status.");
                testPass = this.DoUntilSucceed(() => this.ClientControlAdapter.IsJoinDomainSuccess(),
                    this.TestConfig.Timeout,
                    TimeSpan.FromSeconds(3));
                this.TestSite.Assert.IsTrue(testPass, "Client status update should succeed after joining domain.");
                this.TestSite.Log.Add(LogEntryKind.Debug, "Join Domain Client Status Checked: (1)The client computer is domain joined. (2)The domain name for the client's currently joined domain is correct.");

                #endregion

                #region Stop Message Analyzer

                this.TestSite.Log.Add(LogEntryKind.Debug, "Stop Message Analyzer on driver computer.");
                MaAdapter.StopCapture();
                this.TestSite.Assert.IsTrue(true, "Stop Message Analyzer on driver computer should succeed.");

                #endregion
            }

            #region Verify Capture File

            this.TestSite.Log.Add(LogEntryKind.TestStep, "[START VERIFY CAPTURE]");
            this.TestSite.Log.Add(LogEntryKind.Debug, "Start to verify capture file.");
            MaAdapter.ParseAndVerify(this.TestConfig.LocalCapFileName, true);

            #endregion

            this.TestSite.Log.Add(LogEntryKind.TestStep, "[STOP TEST CASE]");

            if (!this.TestConfig.TriggerDisabled)
            {
                #region Clean up

                this.TestSite.Log.Add(LogEntryKind.Debug, "Clean up: JoinDomainbyCreatingAccountUsingLDAP test case.");
                this.TestSite.Log.Add(LogEntryKind.Debug, "Clean up: Trigger client to unjoin domain");
                testPass = this.ClientControlAdapter.UnjoinDomain();
                this.TestSite.Assert.IsTrue(testPass, "Clean up: Client unjoin domain should succeed.");
                if (this.TestConfig.ClientOperatingSystem.Equals("Windows"))
                {
                    testPass = DoUntilSucceed(() => pingClient.PingFailure(),
                        this.TestConfig.Timeout,
                        TimeSpan.FromSeconds(0.5));
                    this.TestSite.Assert.IsTrue(testPass, "Clean up: Start Rebooting...");
                    Thread.Sleep(this.TestConfig.JoinDomainCreateAcctLDAPSleepTime);
                }
                testPass = this.DoUntilSucceed(() => this.ClientControlAdapter.IsUnjoinDomainSuccess(),
                    this.TestConfig.Timeout,
                    TimeSpan.FromSeconds(3));
                this.TestSite.Assert.IsTrue(testPass, "Clean up: Client unjoin domain should succeed.");
                ActiveDirectoryHelper.DeleteComputer(this.TestConfig.ClientComputerName);
                #endregion
            }
        }

        /// <summary>
        /// Join Domain by Creating An Account Using SAMR
        /// </summary>
        private void JoinDomainCreateAcctSAMR()
        {
            #region LocalVariables

            string testName = string.Empty;
            bool testPass = false;
            PingClient pingClient = new PingClient(this.TestConfig.ClientIP);

            #endregion

            #region Initialize

            this.TestSite.Log.Add(LogEntryKind.TestStep, "[INITIALIZE TEST CASE]");
            testName = System.Reflection.MethodInfo.GetCurrentMethod().Name;
            this.TestSite.Log.Add(LogEntryKind.Debug, "Get test case name: {0}", testName);
            System.Environment.SetEnvironmentVariable("MS_ADOD_TESTCASENAME", testName);
            this.TestSite.Log.Add(LogEntryKind.Debug, "Load MA capture verification information from PTFConfig and Message Analyzer Adapter.");
            this.TestConfig.UpdateExpectedSequenceFile(this.TestConfig.GetMAExpectedSequenceFile(testName));
            MaAdapter.ConfigureAdapter(
                    this.TestConfig.GetEndpointRoles(),
                    this.TestConfig.LocalCapFileName,
                    this.TestConfig.LocalCapFileName.Replace(".matp", "-Selected.matp"),
                    this.TestConfig.GetMAExpectedSequenceFile(testName));

            #endregion

            if (!this.TestConfig.TriggerDisabled)
            {
                this.TestSite.Log.Add(LogEntryKind.TestStep, "[START TEST CASE]");

                #region Start Message Analyzer

                this.TestSite.Log.Add(LogEntryKind.Debug, "Start Message Analyzer on driver computer.");
                MaAdapter.StartCapture(false);
this.TestSite.Assert.IsTrue(true, "Start Message Analyzer on driver computer should succeed.");

                #endregion

                #region Trigger

                this.TestSite.Log.Add(LogEntryKind.Debug, "Trigger client to join domain by creating an account using SAMR.");
                testPass = this.ClientControlAdapter.JoinDomainCreateAcctSAMR();
                this.TestSite.Assert.IsTrue(testPass, "Join Domain by creating an account using SAMR should succeed.");
                if (this.TestConfig.ClientOperatingSystem.Equals("Windows"))
                {
                    testPass = DoUntilSucceed(() => pingClient.PingFailure(),
                        this.TestConfig.Timeout,
                        TimeSpan.FromSeconds(0.5));
                    this.TestSite.Assert.IsTrue(testPass, "Start Rebooting...");
                    Thread.Sleep(this.TestConfig.JoinDomainCreateAcctSAMRSleepTime);
                }

                #endregion

                #region Verify Server

                this.TestSite.Log.Add(LogEntryKind.Debug, "Verify server status.");
                testPass = this.DCControlAdapter.IsJoinDomainSuccess(this.TestConfig.ClientComputerName);
                this.TestSite.Assert.IsTrue(testPass, "Server status update should succeed after joining domain.");
                this.TestSite.Log.Add(LogEntryKind.Debug, "Join Domain Server Status Checked: (1)The client computer account has been successfully added to the Active Directory.");

                #endregion

                #region Verify Client

                this.TestSite.Log.Add(LogEntryKind.Debug, "Verify client status.");
                testPass = this.DoUntilSucceed(() => this.ClientControlAdapter.IsJoinDomainSuccess(),
                    this.TestConfig.Timeout,
                    TimeSpan.FromSeconds(3));
                this.TestSite.Assert.IsTrue(testPass, "Client status update should succeed after joining domain.");
                this.TestSite.Log.Add(LogEntryKind.Debug, "Join Domain Client Status Checked: (1)The client computer is domain joined. (2)The domain name for the client's currently joined domain is correct.");

                #endregion

                #region Stop Message Analyzer

                this.TestSite.Log.Add(LogEntryKind.Debug, "Stop Message Analyzer on driver computer.");
                MaAdapter.StopCapture();
                this.TestSite.Assert.IsTrue(true, "Stop Message Analyzer on driver computer should succeed.");

                #endregion
            }

            #region Verify Capture File

            this.TestSite.Log.Add(LogEntryKind.TestStep, "[START VERIFY CAPTURE]");
            this.TestSite.Log.Add(LogEntryKind.Debug, "Start to verify capture file.");
            MaAdapter.ParseAndVerify(this.TestConfig.LocalCapFileName, true);

            this.TestSite.Log.Add(LogEntryKind.TestStep, "[STOP TEST CASE]");

            #endregion

            if (!this.TestConfig.TriggerDisabled)
            {
                #region Clean up

                this.TestSite.Log.Add(LogEntryKind.Debug, "Clean up: JoinDomainbyCreatingAccountUsingSAMR test case.");
                this.TestSite.Log.Add(LogEntryKind.Debug, "Clean up: Trigger client to unjoin domain.");
                testPass = this.ClientControlAdapter.UnjoinDomain();
                this.TestSite.Assert.IsTrue(testPass, "Client unjoin domain should succeed.");
                if (this.TestConfig.ClientOperatingSystem.Equals("Windows"))
                {
                    testPass = DoUntilSucceed(() => pingClient.PingFailure(),
                        this.TestConfig.Timeout,
                        TimeSpan.FromSeconds(0.5));
                    this.TestSite.Assert.IsTrue(testPass, "Start Rebooting...");
                    Thread.Sleep(this.TestConfig.JoinDomainCreateAcctSAMRSleepTime);
                }
                testPass = this.DoUntilSucceed(() => this.ClientControlAdapter.IsUnjoinDomainSuccess(),
                    this.TestConfig.Timeout,
                    TimeSpan.FromSeconds(3));
                this.TestSite.Assert.IsTrue(testPass, "Client unjoin domain should succeed.");
                this.TestSite.Log.Add(LogEntryKind.Debug, "Clean up: Delete Computer Account in DC.");
                ActiveDirectoryHelper.DeleteComputer(this.TestConfig.ClientComputerName);

                #endregion
            }
        }

        /// <summary>
        /// Join Domain by a predefined computer account
        /// </summary>
        [TestMethod]
        [Priority(0)]
        [Description("Joining domain by a predefined computer account")]
        [TestCategory("BVT")]
        [DeploymentItem(@"C:\Program Files\Microsoft Message Analyzer\Microsoft.WindowsAzure.Storage.dll")]
        public void JoinDomainPredefAcct()
        {
            #region LocalVariables

            string testName = string.Empty;
            bool testPass = false;
            PingClient pingClient = new PingClient(this.TestConfig.ClientIP);

            #endregion

            #region Initialize

            this.TestSite.Log.Add(LogEntryKind.TestStep, "[INITIALIZE TEST CASE]");
            testName = System.Reflection.MethodInfo.GetCurrentMethod().Name;
            this.TestSite.Log.Add(LogEntryKind.Debug, "Get test case name: {0}", testName);
            System.Environment.SetEnvironmentVariable("MS_ADOD_TESTCASENAME", testName);
            this.TestSite.Log.Add(LogEntryKind.Debug, "Load MA capture verification information from PTFConfig and Message Analyzer Adapter.");
            this.TestConfig.UpdateExpectedSequenceFile(this.TestConfig.GetMAExpectedSequenceFile(testName));
            MaAdapter.ConfigureAdapter(
                    this.TestConfig.GetEndpointRoles(),
                    this.TestConfig.LocalCapFileName,
                    this.TestConfig.LocalCapFileName.Replace(".matp", "-Selected.matp"),
                    this.TestConfig.GetMAExpectedSequenceFile(testName));

            if (!this.TestConfig.TriggerDisabled)
            {
                this.TestSite.Log.Add(LogEntryKind.Debug, "Create a computer account in Active Directory first.");
                ActiveDirectoryHelper.CreateComputer(this.TestConfig.ClientComputerName);
            }

            #endregion

            if (!this.TestConfig.TriggerDisabled)
            {
                this.TestSite.Log.Add(LogEntryKind.TestStep, "[START TEST CASE]");

                #region Start Message Analyzer

                this.TestSite.Log.Add(LogEntryKind.Debug, "Start Message Analyzer on driver computer.");
                MaAdapter.StartCapture(false);
                this.TestSite.Assert.IsTrue(true, "Start Message Analyzer on driver computer should succeed.");

                #endregion

                #region Trigger

                this.TestSite.Log.Add(LogEntryKind.Debug, "Trigger client to join domain by a predefined account.");
                testPass = this.ClientControlAdapter.JoinDomainPredefAcct();
                this.TestSite.Assert.IsTrue(testPass, "Join Domain by Predefined Account should succeed.");
                if (this.TestConfig.ClientOperatingSystem.Equals("Windows"))
                {
                    testPass = DoUntilSucceed(() => pingClient.PingFailure(),
                        this.TestConfig.Timeout,
                        TimeSpan.FromSeconds(1));
                    this.TestSite.Assert.IsTrue(testPass, "Start Rebooting...");
                    Thread.Sleep(this.TestConfig.JoinDomainPredefAcctSleepTime);
                }

                #endregion

                #region Verify Server

                this.TestSite.Log.Add(LogEntryKind.Debug, "Verify server status.");
                testPass = this.DCControlAdapter.IsJoinDomainSuccess(this.TestConfig.ClientComputerName);
                this.TestSite.Assert.IsTrue(testPass, "Server status update should succeed after joining domain.");
                this.TestSite.Log.Add(LogEntryKind.Debug, "Join Domain Server Status Checked: (1)The client computer account has been successfully added to the Active Directory.");

                #endregion

                #region Verify Client

                this.TestSite.Log.Add(LogEntryKind.Debug, "Verify client status.");
                testPass = DoUntilSucceed(() => this.ClientControlAdapter.IsJoinDomainSuccess(),
                    this.TestConfig.Timeout,
                    TimeSpan.FromSeconds(3));
                this.TestSite.Assert.IsTrue(testPass, "Client status update should succeed after joining domain.");
                this.TestSite.Log.Add(LogEntryKind.Debug, "Join Domain Client Status Checked: (1)The client computer is domain joined. (2)The domain name for the client's currently joined domain is correct.");

                #endregion

                #region Stop Message Analyzer

                this.TestSite.Log.Add(LogEntryKind.Debug, "Stop Message Analyzer on driver computer.");
                MaAdapter.StopCapture();
                this.TestSite.Assert.IsTrue(true, "Stop Message Analyzer on driver computer should succeed.");

                #endregion
            }

            #region Verify Capture File

            this.TestSite.Log.Add(LogEntryKind.TestStep, "[START VERIFY CAPTURE]");
            this.TestSite.Log.Add(LogEntryKind.Debug, "Start to verify capture file.");
            MaAdapter.ParseAndVerify(this.TestConfig.LocalCapFileName, true);

            #endregion

            this.TestSite.Log.Add(LogEntryKind.TestStep, "[STOP TEST CASE]");

            if (!this.TestConfig.TriggerDisabled)
            {
                #region Clean up

                this.TestSite.Log.Add(LogEntryKind.Debug, "Clean up: JoinDomainbyPredefinedAccountUsingLDAP test case.");
                this.TestSite.Log.Add(LogEntryKind.Debug, "Clean up: Trigger client to unjoin domain");
                testPass = this.ClientControlAdapter.UnjoinDomain();
                this.TestSite.Assert.IsTrue(testPass, "Client unjoin domain should succeed.");
                if (this.TestConfig.ClientOperatingSystem.Equals("Windows"))
                {
                    testPass = DoUntilSucceed(() => pingClient.PingFailure(),
                        this.TestConfig.Timeout,
                        TimeSpan.FromSeconds(1));
                    this.TestSite.Assert.IsTrue(testPass, "Start Rebooting...");
                    Thread.Sleep(this.TestConfig.JoinDomainPredefAcctSleepTime);
                }
                testPass = this.DoUntilSucceed(() => this.ClientControlAdapter.IsUnjoinDomainSuccess(),
                    this.TestConfig.Timeout,
                    TimeSpan.FromSeconds(3));
                this.TestSite.Assert.IsTrue(testPass, "Client unjoin domain should succeed.");
                this.TestSite.Log.Add(LogEntryKind.Debug, "Clean up: Delete Computer account in DC.");
                ActiveDirectoryHelper.DeleteComputer(this.TestConfig.ClientComputerName);

                #endregion
            }
        }

        /// <summary>
        /// Unjoin Domain
        /// </summary>
        [TestMethod]
        [Priority(0)]
        [Description("Unjoin domain")]
        [TestCategory("BVT")]
        [DeploymentItem(@"C:\Program Files\Microsoft Message Analyzer\Microsoft.WindowsAzure.Storage.dll")]
        public void UnjoinDomain()
        {
            #region LocalVariables

            string testName = string.Empty;
            bool testPass = false;
            PingClient pingClient = new PingClient(this.TestConfig.ClientIP);

            #endregion

            #region Initialize

            this.TestSite.Log.Add(LogEntryKind.TestStep, "[INITIALIZE TEST CASE]");
            testName = System.Reflection.MethodInfo.GetCurrentMethod().Name;
            this.TestSite.Log.Add(LogEntryKind.Debug, "Get test case name: {0}", testName);
            System.Environment.SetEnvironmentVariable("MS_ADOD_TESTCASENAME", testName);
            this.TestSite.Log.Add(LogEntryKind.Debug, "Load MA capture verification information from PTFConfig and Message Analyzer Adapter.");
            this.TestConfig.UpdateExpectedSequenceFile(this.TestConfig.GetMAExpectedSequenceFile(testName));
            MaAdapter.ConfigureAdapter(
                    this.TestConfig.GetEndpointRoles(),
                    this.TestConfig.LocalCapFileName,
                    this.TestConfig.LocalCapFileName.Replace(".matp", "-Selected.matp"),
                    this.TestConfig.GetMAExpectedSequenceFile(testName));

            if (!this.TestConfig.TriggerDisabled)
            {
                this.TestSite.Log.Add(LogEntryKind.Debug, "Initialize: Join Domain by creating an account first.");
                testPass = this.ClientControlAdapter.JoinDomainCreateAcctLDAP();
                this.TestSite.Assert.IsTrue(testPass, "Initialize: Join Domain by creating an account using LDAP should succeed.");
                if (this.TestConfig.ClientOperatingSystem.Equals("Windows"))
                {
                    testPass = DoUntilSucceed(() => pingClient.PingFailure(),
                        this.TestConfig.Timeout,
                        TimeSpan.FromSeconds(1));
                    this.TestSite.Assert.IsTrue(testPass, "Start Rebooting...");
                    Thread.Sleep(this.TestConfig.UnjoinDomainSleepTime);
                }
                this.TestSite.Log.Add(LogEntryKind.Debug, "Initialize: Verify client status.");
                testPass = this.DoUntilSucceed(() => this.ClientControlAdapter.IsJoinDomainSuccess(),
                    this.TestConfig.Timeout,
                    TimeSpan.FromSeconds(3));
                this.TestSite.Assert.IsTrue(testPass, "Initialized: Client status update should succeed after joining domain.");
            }

            #endregion

            if (!this.TestConfig.TriggerDisabled)
            {
                this.TestSite.Log.Add(LogEntryKind.TestStep, "[START TEST CASE]");

                #region Start Message Analyzer

                this.TestSite.Log.Add(LogEntryKind.Debug, "Start Message Analyzer on driver computer.");
                MaAdapter.StartCapture(false);
                this.TestSite.Assert.IsTrue(true, "Start Message Analyzer on driver computer should succeed.");

                #endregion

                #region Trigger

                this.TestSite.Log.Add(LogEntryKind.Debug, "Trigger client to unjoin domain");
                testPass = this.ClientControlAdapter.UnjoinDomain();
                this.TestSite.Assert.IsTrue(testPass, "Unjoin Domain should succeed.");
                if (this.TestConfig.ClientOperatingSystem.Equals("Windows"))
                {
                    testPass = DoUntilSucceed(() => pingClient.PingFailure(),
                        this.TestConfig.Timeout,
                        TimeSpan.FromSeconds(1));
                    this.TestSite.Assert.IsTrue(testPass, "Start Rebooting...");
                    Thread.Sleep(this.TestConfig.UnjoinDomainSleepTime);
                }

                #endregion

                #region Verify Client

                this.TestSite.Log.Add(LogEntryKind.Debug, "Verify client status");
                testPass = DoUntilSucceed(() => this.ClientControlAdapter.IsUnjoinDomainSuccess(),
                    this.TestConfig.Timeout,
                    TimeSpan.FromSeconds(3));
                this.TestSite.Assert.IsTrue(testPass, "Client status update should succeed after unjoining domain.");
                this.TestSite.Log.Add(LogEntryKind.Debug, "Unjoin Domain Client Status Checked: (1)The client computer is NOT domain joined.");

                #endregion

                #region Stop Message Analyzer

                this.TestSite.Log.Add(LogEntryKind.Debug, "Stop Message Analyzer on driver computer.");
                MaAdapter.StopCapture();
                this.TestSite.Assert.IsTrue(true, "Stop Message Analyzer on driver computer should succeed.");

                #endregion
            }

            #region Verify Capture File

            this.TestSite.Log.Add(LogEntryKind.TestStep, "[START VERIFY CAPTURE]");
            this.TestSite.Log.Add(LogEntryKind.Debug, "Start to verify capture file.");
            MaAdapter.ParseAndVerify(this.TestConfig.LocalCapFileName, true);

            #endregion

            this.TestSite.Log.Add(LogEntryKind.TestStep, "[STOP TEST CASE]");

            if (!this.TestConfig.TriggerDisabled)
            {
                #region Clean up

                this.TestSite.Log.Add(LogEntryKind.Debug, "Clean up: Delete Computer account in DC.");
                ActiveDirectoryHelper.DeleteComputer(this.TestConfig.ClientComputerName);

                #endregion
            }
        }

        #endregion

        #region [MS-ADOD] section 3.2 Directory Examples

        /// <summary>
        /// Provision a User Account using LDAP
        /// </summary>
        [TestMethod]
        [Priority(0)]
        [Description("Provision a User Account using LDAP")]
        [TestCategory("non BVT")]
        [DeploymentItem(@"C:\Program Files\Microsoft Message Analyzer\Microsoft.WindowsAzure.Storage.dll")]
        public void ProvisionUserAcctLDAP()
        {
            #region LocalVariables

            string testName = string.Empty;
            bool testPass = false;

            #endregion

            #region Initialize

            this.TestSite.Log.Add(LogEntryKind.TestStep, "[INITIALIZE TEST CASE]");
            testName = System.Reflection.MethodInfo.GetCurrentMethod().Name;
            this.TestSite.Log.Add(LogEntryKind.Debug, "Get test case name: {0}", testName);
            System.Environment.SetEnvironmentVariable("MS_ADOD_TESTCASENAME", testName);
            this.TestSite.Log.Add(LogEntryKind.Debug, "Load MA capture verification information from PTFConfig and Message Analyzer Adapter.");
            this.TestConfig.UpdateExpectedSequenceFile(this.TestConfig.GetMAExpectedSequenceFile(testName));
            MaAdapter.ConfigureAdapter(
                    this.TestConfig.GetEndpointRoles(),
                    this.TestConfig.LocalCapFileName,
                    this.TestConfig.LocalCapFileName.Replace(".matp", "-Selected.matp"),
                    this.TestConfig.GetMAExpectedSequenceFile(testName));

            #endregion

            if (!this.TestConfig.TriggerDisabled)
            {
                this.TestSite.Log.Add(LogEntryKind.TestStep, "[START TEST CASE]");

                #region Start Message Analyzer

                this.TestSite.Log.Add(LogEntryKind.Debug, "Start Message Analyzer on driver computer.");
                MaAdapter.StartCapture(false);
                this.TestSite.Assert.IsTrue(true, "Start Message Analyzer on driver computer should succeed.");

                #endregion

                #region Trigger

                this.TestSite.Log.Add(LogEntryKind.Debug, "Trigger client to Provision a user account using LDAP.");
                testPass = this.ClientControlAdapter.ProvisionUserAcctLDAP();
                this.TestSite.Assert.IsTrue(testPass, "Provision a user account using LDAP should succeed.");

                #endregion

                #region Verify Server

                this.TestSite.Log.Add(LogEntryKind.Debug, "Start to check state change in server side");
                testPass = this.DCControlAdapter.IsProvisionUserAcctSuccess(
                    this.TestConfig.FullDomainName,
                    this.TestConfig.DomainNewUserUsername,
                    this.TestConfig.DomainNewUserPwd);
                this.TestSite.Assert.IsTrue(testPass, "Provision a user account using LDAP, check server state update after provisioning the user should succeed.");

                #endregion

                #region Stop Message Analyzer

                this.TestSite.Log.Add(LogEntryKind.Debug, "Stop Message Analyzer on driver computer.");
                MaAdapter.StopCapture();
                this.TestSite.Assert.IsTrue(true, "Stop Message Analyzer on driver computer should succeed.");

                #endregion
            }

            #region Verify Capture File

            this.TestSite.Log.Add(LogEntryKind.TestStep, "[START VERIFY CAPTURE]");
            this.TestSite.Log.Add(LogEntryKind.Debug, "Start to verify capture file.");
            MaAdapter.ParseAndVerify(this.TestConfig.LocalCapFileName, true);

            #endregion

            if (!this.TestConfig.TriggerDisabled)
            {
                this.TestSite.Log.Add(LogEntryKind.TestStep, "[STOP TEST CASE]");

                #region Clean up

                this.TestSite.Log.Add(LogEntryKind.Debug, "Clean up: ProvisionUserAcctLDAP test case.");
                this.TestSite.Log.Add(LogEntryKind.Debug, "Clean up: Delete the New User in DC.");
                ActiveDirectoryHelper.DeleteUser(this.TestConfig.DomainNewUserUsername);

                #endregion
            }
        }

        /// <summary>
        /// Provision a User Account using SAMR
        /// </summary>
        [TestMethod]
        [Priority(0)]
        [Description("Provision a User Account using SAMR")]
        [TestCategory("non BVT")]
        [DeploymentItem(@"C:\Program Files\Microsoft Message Analyzer\Microsoft.WindowsAzure.Storage.dll")]
        public void ProvisionUserAcctSAMR()
        {
            #region LocalVariables

            string testName = string.Empty;
            bool testPass = false;

            #endregion

            #region Initialize

            this.TestSite.Log.Add(LogEntryKind.TestStep, "[INITIALIZE TEST CASE]");
            testName = System.Reflection.MethodInfo.GetCurrentMethod().Name;
            this.TestSite.Log.Add(LogEntryKind.Debug, "Get test case name: {0}", testName);
            System.Environment.SetEnvironmentVariable("MS_ADOD_TESTCASENAME", testName);
            this.TestSite.Log.Add(LogEntryKind.Debug, "Load MA capture verification information from PTFConfig and Message Analyzer Adapter.");
            this.TestConfig.UpdateExpectedSequenceFile(this.TestConfig.GetMAExpectedSequenceFile(testName));
            MaAdapter.ConfigureAdapter(
                    this.TestConfig.GetEndpointRoles(),
                    this.TestConfig.LocalCapFileName,
                    this.TestConfig.LocalCapFileName.Replace(".matp", "-Selected.matp"),
                    this.TestConfig.GetMAExpectedSequenceFile(testName));

            #endregion

            if (!this.TestConfig.TriggerDisabled)
            {
                this.TestSite.Log.Add(LogEntryKind.TestStep, "[START TEST CASE]");

                #region Start Message Analyzer

                this.TestSite.Log.Add(LogEntryKind.Debug, "Start Message Analyzer on driver computer.");
                MaAdapter.StartCapture(false);
                this.TestSite.Assert.IsTrue(true, "Start Message Analyzer on driver computer should succeed.");

                #endregion

                #region Trigger

                this.TestSite.Log.Add(LogEntryKind.Debug, "Trigger client to Provision a user account using SAMR.");
                testPass = this.ClientControlAdapter.ProvisionUserAcctSAMR();
                this.TestSite.Assert.IsTrue(testPass, "Provision a user account using SAMR should succeed.");

                #endregion

                #region Verify Server

                this.TestSite.Log.Add(LogEntryKind.Debug, "Start to check state change in server side");
                testPass = this.DCControlAdapter.IsProvisionUserAcctSuccess(
                    this.TestConfig.FullDomainName,
                    this.TestConfig.DomainNewUserUsername,
                    this.TestConfig.DomainNewUserPwd);
                this.TestSite.Assert.IsTrue(testPass, "Provision a user account using SAMR, check server state update after provisioning the user should succeed.");

                #endregion

                #region Stop Message Analyzer

                this.TestSite.Log.Add(LogEntryKind.Debug, "Stop Message Analyzer on driver computer.");
                MaAdapter.StopCapture();
                this.TestSite.Assert.IsTrue(true, "Stop Message Analyzer on driver computer should succeed.");

                #endregion
            }

            #region Verify Capture File

            this.TestSite.Log.Add(LogEntryKind.TestStep, "[START VERIFY CAPTURE]");
            this.TestSite.Log.Add(LogEntryKind.Debug, "Start to verify capture file.");
            MaAdapter.ParseAndVerify(this.TestConfig.LocalCapFileName, true);

            #endregion

            this.TestSite.Log.Add(LogEntryKind.TestStep, "[STOP TEST CASE]");

            if (!this.TestConfig.TriggerDisabled)
            {

                #region Clean up

                this.TestSite.Log.Add(LogEntryKind.Debug, "Clean up: ProvisionUserAcctSAMR test case.");
                this.TestSite.Log.Add(LogEntryKind.Debug, "Clean up: Delete the New User in DC.");
                ActiveDirectoryHelper.DeleteUser(this.TestConfig.DomainNewUserUsername);

                #endregion
            }
        }

        /// <summary>
        /// Change a User Account Password using LDAP
        /// </summary>
        [TestMethod]
        [Priority(0)]
        [Description("Change a User Account Password using LDAP")]
        [TestCategory("non BVT")]
        [DeploymentItem(@"C:\Program Files\Microsoft Message Analyzer\Microsoft.WindowsAzure.Storage.dll")]
        public void ChangeUserAcctPasswordLDAP()
        {
            #region LocalVariables

            string testName = string.Empty;
            bool testPass = false;

            #endregion

            #region Initialize

            this.TestSite.Log.Add(LogEntryKind.TestStep, "[INITIALIZE TEST CASE]");
            testName = System.Reflection.MethodInfo.GetCurrentMethod().Name;
            this.TestSite.Log.Add(LogEntryKind.Debug, "Get test case name: {0}", testName);
            System.Environment.SetEnvironmentVariable("MS_ADOD_TESTCASENAME", testName);
            this.TestSite.Log.Add(LogEntryKind.Debug, "Load MA capture verification information from PTFConfig and Message Analyzer Adapter.");
            this.TestConfig.UpdateExpectedSequenceFile(this.TestConfig.GetMAExpectedSequenceFile(testName));
            MaAdapter.ConfigureAdapter(
                    this.TestConfig.GetEndpointRoles(),
                    this.TestConfig.LocalCapFileName,
                    this.TestConfig.LocalCapFileName.Replace(".matp", "-Selected.matp"),
                    this.TestConfig.GetMAExpectedSequenceFile(testName));

            if (!this.TestConfig.TriggerDisabled)
            {
                this.TestSite.Log.Add(LogEntryKind.Debug, "Initialize: Add a new user into Active Directory first.");
                ActiveDirectoryHelper.CreateUser(this.TestConfig.DomainNewUserUsername, this.TestConfig.DomainNewUserPwd);
            }

            #endregion

            if (!this.TestConfig.TriggerDisabled)
            {
                this.TestSite.Log.Add(LogEntryKind.TestStep, "[START TEST CASE]");

                #region Start Message Analyzer

                this.TestSite.Log.Add(LogEntryKind.Debug, "Start Message Analyzer on driver computer.");
                MaAdapter.StartCapture(false);
                this.TestSite.Assert.IsTrue(true, "Start Message Analyzer on driver computer should succeed.");

                #endregion

                #region Trigger

                this.TestSite.Log.Add(LogEntryKind.Debug, "Trigger client to change a user account password using LDAP.");
                testPass = this.ClientControlAdapter.ChangeUserAcctPasswordLDAP();
                this.TestSite.Assert.IsTrue(testPass, "Change a user account Password should succeed.");

                #endregion

                #region Verify Server

                this.TestSite.Log.Add(LogEntryKind.Debug, "Start to check state change in server side");
                testPass = this.DCControlAdapter.IsChangeUserAcctPasswordSuccess(
                    this.TestConfig.FullDomainName,
                    this.TestConfig.DomainNewUserUsername,
                    this.TestConfig.DomainNewUserNewPwd);
                this.TestSite.Assert.IsTrue(testPass, "Change a user account Password, check server state update after changing the user account password should succeed.");

                #endregion

                #region Stop Message Analyzer

                this.TestSite.Log.Add(LogEntryKind.Debug, "Stop Message Analyzer on driver computer.");
                MaAdapter.StopCapture();
                this.TestSite.Assert.IsTrue(true, "Stop Message Analyzer on driver computer should succeed.");

                #endregion
            }

            #region Verify Capture File

            this.TestSite.Log.Add(LogEntryKind.TestStep, "[START VERIFY CAPTURE]");
            this.TestSite.Log.Add(LogEntryKind.Debug, "Start to verify capture file.");
            MaAdapter.ParseAndVerify(this.TestConfig.LocalCapFileName, true);

            #endregion

            this.TestSite.Log.Add(LogEntryKind.TestStep, "[STOP TEST CASE]");

            if (!this.TestConfig.TriggerDisabled)
            {
                #region Clean up

                this.TestSite.Log.Add(LogEntryKind.Debug, "Clean up: ChangeUserAcctPasswordLDAP test case.");
                this.TestSite.Log.Add(LogEntryKind.Debug, "Clean up: Delete the New User in DC.");
                ActiveDirectoryHelper.DeleteUser(this.TestConfig.DomainNewUserUsername);

                #endregion
            }
        }

        /// <summary>
        /// Change a User Account Password using SAMR
        /// </summary>
        [TestMethod]
        [Priority(0)]
        [Description("Change a User Account Password using SAMR")]
        [TestCategory("non BVT")]
        [DeploymentItem(@"C:\Program Files\Microsoft Message Analyzer\Microsoft.WindowsAzure.Storage.dll")]
        public void ChangeUserAcctPasswordSAMR()
        {
            #region LocalVariables

            string testName = string.Empty;
            bool testPass = false;

            #endregion

            #region Initialize

            this.TestSite.Log.Add(LogEntryKind.TestStep, "[INITIALIZE TEST CASE]");
            testName = System.Reflection.MethodInfo.GetCurrentMethod().Name;
            this.TestSite.Log.Add(LogEntryKind.Debug, "Get test case name: {0}", testName);
            System.Environment.SetEnvironmentVariable("MS_ADOD_TESTCASENAME", testName);
            this.TestSite.Log.Add(LogEntryKind.Debug, "Load MA capture verification information from PTFConfig and Message Analyzer Adapter.");
            this.TestConfig.UpdateExpectedSequenceFile(this.TestConfig.GetMAExpectedSequenceFile(testName));
            MaAdapter.ConfigureAdapter(
                    this.TestConfig.GetEndpointRoles(),
                    this.TestConfig.LocalCapFileName,
                    this.TestConfig.LocalCapFileName.Replace(".matp", "-Selected.matp"),
                    this.TestConfig.GetMAExpectedSequenceFile(testName));

            if (!this.TestConfig.TriggerDisabled)
            {
                this.TestSite.Log.Add(LogEntryKind.Debug, "Initialize: Add a new user into Active Directory first.");
                ActiveDirectoryHelper.CreateUser(this.TestConfig.DomainNewUserUsername, this.TestConfig.DomainNewUserPwd);
            }

            #endregion

            if (!this.TestConfig.TriggerDisabled)
            {
                this.TestSite.Log.Add(LogEntryKind.TestStep, "[START TEST CASE]");

                #region Start Message Analyzer

                this.TestSite.Log.Add(LogEntryKind.Debug, "Start Message Analyzer on driver computer.");
                MaAdapter.StartCapture(false);
                this.TestSite.Assert.IsTrue(true, "Start Message Analyzer on driver computer should succeed.");

                #endregion

                #region Trigger

                this.TestSite.Log.Add(LogEntryKind.Debug, "Trigger client to change a user account password using SAMR.");
                testPass = this.ClientControlAdapter.ChangeUserAcctPasswordSAMR();
                this.TestSite.Assert.IsTrue(testPass, "Change a user account Password should succeed.");

                #endregion

                #region Verify Server

                this.TestSite.Log.Add(LogEntryKind.Debug, "Start to check state change in server side");
                testPass = this.DCControlAdapter.IsChangeUserAcctPasswordSuccess(
                    this.TestConfig.FullDomainName,
                    this.TestConfig.DomainNewUserUsername,
                    this.TestConfig.DomainNewUserNewPwd);
                this.TestSite.Assert.IsTrue(testPass, "Change a user account Password, check server state update after changing the user account password should succeed.");

                #endregion

                #region Stop Message Analyzer

                this.TestSite.Log.Add(LogEntryKind.Debug, "Stop Message Analyzer on driver computer.");
                MaAdapter.StopCapture();
                this.TestSite.Assert.IsTrue(true, "Stop Message Analyzer on driver computer should succeed.");

                #endregion
            }

            #region Verify Capture File

            this.TestSite.Log.Add(LogEntryKind.TestStep, "[START VERIFY CAPTURE]");
            this.TestSite.Log.Add(LogEntryKind.Debug, "Start to verify capture file.");
            MaAdapter.ParseAndVerify(this.TestConfig.LocalCapFileName, true);

            #endregion

            this.TestSite.Log.Add(LogEntryKind.TestStep, "[STOP TEST CASE]");

            if (!this.TestConfig.TriggerDisabled)
            {
                #region Clean up

                this.TestSite.Log.Add(LogEntryKind.Debug, "Clean up: ChangeUserAcctPasswordSAMR test case.");
                this.TestSite.Log.Add(LogEntryKind.Debug, "Clean up: Delete the New User in DC.");
                ActiveDirectoryHelper.DeleteUser(this.TestConfig.DomainNewUserUsername);

                #endregion
            }
        }

        /// <summary>
        /// Determine the Group Membership of a User Account using LDAP
        /// </summary>
        [TestMethod]
        [Priority(0)]
        [Description("Determine the Group Membership of a User Account using LDAP")]
        [TestCategory("non BVT")]
        [DeploymentItem(@"C:\Program Files\Microsoft Message Analyzer\Microsoft.WindowsAzure.Storage.dll")]
        public void DetermineUserAcctMembershipLDAP()
        {
            #region LocalVariables

            string testName = string.Empty;
            string testResult = string.Empty;
            bool testPass = false;

            #endregion

            #region Initialize

            this.TestSite.Log.Add(LogEntryKind.TestStep, "[INITIALIZE TEST CASE]");
            testName = System.Reflection.MethodInfo.GetCurrentMethod().Name;
            this.TestSite.Log.Add(LogEntryKind.Debug, "Get test case name: {0}", testName);
            System.Environment.SetEnvironmentVariable("MS_ADOD_TESTCASENAME", testName);
            this.TestSite.Log.Add(LogEntryKind.Debug, "Load MA capture verification information from PTFConfig and Message Analyzer Adapter.");
            this.TestConfig.UpdateExpectedSequenceFile(this.TestConfig.GetMAExpectedSequenceFile(testName));
            MaAdapter.ConfigureAdapter(
                    this.TestConfig.GetEndpointRoles(),
                    this.TestConfig.LocalCapFileName,
                    this.TestConfig.LocalCapFileName.Replace(".matp", "-Selected.matp"),
                    this.TestConfig.GetMAExpectedSequenceFile(testName));

            if (!this.TestConfig.TriggerDisabled)
            {
                this.TestSite.Log.Add(LogEntryKind.Debug, "Initialize: Add a new group: {0} and a new user: {1} to AD.", this.TestConfig.DomainNewGroup, this.TestConfig.DomainNewUserUsername);
                ActiveDirectoryHelper.CreateGroup(this.TestConfig.DomainNewGroup);
                ActiveDirectoryHelper.CreateUser(this.TestConfig.DomainNewUserUsername, this.TestConfig.DomainNewUserPwd);
                this.TestSite.Log.Add(LogEntryKind.Debug, "Initialize: Add the new user to the new group.");
                ActiveDirectoryHelper.AddToGroup(this.TestConfig.DomainNewUserUsername, this.TestConfig.DomainNewGroup);
            }

            #endregion

            if (!this.TestConfig.TriggerDisabled)
            {
                this.TestSite.Log.Add(LogEntryKind.TestStep, "[START TEST CASE]");

                #region Start Message Analyzer

                this.TestSite.Log.Add(LogEntryKind.Debug, "Start Message Analyzer on driver computer.");
                MaAdapter.StartCapture(false);
                this.TestSite.Assert.IsTrue(true, "Start Message Analyzer on driver computer should succeed.");

                #endregion

                #region Trigger

                this.TestSite.Log.Add(LogEntryKind.Debug, "Trigger client to determine the group membership of a user account.");
                testResult = this.ClientControlAdapter.DetermineUserAcctMembershipLDAP();
                this.TestSite.Assert.IsNotNull(testResult, "Determine the group membership of a user account should succeed.");

                #endregion

                #region Verify

                testPass = testResult.Contains(this.TestConfig.DomainNewGroup);
                this.TestSite.Assert.IsTrue(testPass, "Verify Account Group Membership should succeed.");
                this.TestSite.Log.Add(LogEntryKind.CheckSucceeded, "Check correctness of determine group membership of a user account.", testPass);

                #endregion

                #region Stop Message Analyzer

                this.TestSite.Log.Add(LogEntryKind.Debug, "Stop Message Analyzer on driver computer.");
                MaAdapter.StopCapture();
                this.TestSite.Assert.IsTrue(true, "Stop Message Analyzer on driver computer should succeed.");

                #endregion
            }

            #region Verify Capture File

            this.TestSite.Log.Add(LogEntryKind.TestStep, "[START VERIFY CAPTURE]");
            this.TestSite.Log.Add(LogEntryKind.Debug, "Start to verify capture file.");
            MaAdapter.ParseAndVerify(this.TestConfig.LocalCapFileName, true);

            #endregion

            this.TestSite.Log.Add(LogEntryKind.TestStep, "[STOP TEST CASE]");

            if (!this.TestConfig.TriggerDisabled)
            {
                #region Clean up

                this.TestSite.Log.Add(LogEntryKind.Debug, "Clean up: DetermineUserAcctMembershipLDAP test case.");
                this.TestSite.Log.Add(LogEntryKind.Debug, "Clean up: Delete the New User in DC.");
                ActiveDirectoryHelper.DeleteUser(this.TestConfig.DomainNewUserUsername);
                this.TestSite.Log.Add(LogEntryKind.Debug, "Clean up: Delete the New Group in DC.");
                ActiveDirectoryHelper.DeleteGroup(this.TestConfig.DomainNewGroup);

                #endregion
            }
        }

        /// <summary>
        /// Determine the Group Membership of a User Account using SAMR
        /// </summary>
        [Priority(0)]
        [Description("Determine the Group Membership of a User Account using SAMR")]
        [TestCategory("non BVT")]
        [TestCategory("pre-win8")]
        public void DetermineUserAcctMembershipSAMR()
        {
            #region LocalVariables

            string testName = string.Empty;
            string testResult = string.Empty;
            bool testPass = false;

            #endregion

            #region Initialize

            this.TestSite.Log.Add(LogEntryKind.TestStep, "[INITIALIZE TEST CASE]");
            testName = System.Reflection.MethodInfo.GetCurrentMethod().Name;
            this.TestSite.Log.Add(LogEntryKind.Debug, "Get test case name: {0}", testName);
            System.Environment.SetEnvironmentVariable("MS_ADOD_TESTCASENAME", testName);
            this.TestSite.Log.Add(LogEntryKind.Debug, "Load MA capture verification information from PTFConfig and Message Analyzer Adapter.");
            this.TestConfig.UpdateExpectedSequenceFile(this.TestConfig.GetMAExpectedSequenceFile(testName));
            MaAdapter.ConfigureAdapter(
                    this.TestConfig.GetEndpointRoles(),
                    this.TestConfig.LocalCapFileName,
                    this.TestConfig.LocalCapFileName.Replace(".matp", "-Selected.matp"),
                    this.TestConfig.GetMAExpectedSequenceFile(testName));

            if (!this.TestConfig.TriggerDisabled)
            {
                this.TestSite.Log.Add(LogEntryKind.Debug, "Initialize: Add a new group: {0} and a new user: {1} to AD.", this.TestConfig.DomainNewGroup, this.TestConfig.DomainNewUserUsername);
                ActiveDirectoryHelper.CreateGroup(this.TestConfig.DomainNewGroup);
                ActiveDirectoryHelper.CreateUser(this.TestConfig.DomainNewUserUsername, this.TestConfig.DomainNewUserPwd);
                this.TestSite.Log.Add(LogEntryKind.Debug, "Initialize: Add the new user to the new group.");
                ActiveDirectoryHelper.AddToGroup(this.TestConfig.DomainNewUserUsername, this.TestConfig.DomainNewGroup);
            }

            #endregion

            if (!this.TestConfig.TriggerDisabled)
            {
                this.TestSite.Log.Add(LogEntryKind.TestStep, "[START TEST CASE]");

                #region Start Message Analyzer

                this.TestSite.Log.Add(LogEntryKind.Debug, "Start Message Analyzer on driver computer.");
                MaAdapter.StartCapture(false);
                this.TestSite.Assert.IsTrue(true, "Start Message Analyzer on driver computer should succeed.");

                #endregion

                #region Trigger

                this.TestSite.Log.Add(LogEntryKind.Debug, "Trigger client to determine the group membership of a user account.");
                testResult = this.ClientControlAdapter.DetermineUserAcctMembershipSAMR();
                this.TestSite.Assert.IsNotNull(testResult, "Determine the group membership of a user account should succeed.");

                #endregion

                #region Verify

                testPass = testResult.Contains(this.TestConfig.DomainNewGroup);
                this.TestSite.Assert.IsTrue(testPass, "Verify Account Group Membership should succeed.");
                this.TestSite.Log.Add(LogEntryKind.CheckSucceeded, "Check correctness of determining group membership of a user account.", testPass);

                #endregion

                #region Stop Message Analyzer

                this.TestSite.Log.Add(LogEntryKind.Debug, "Stop Message Analyzer on driver computer.");
                MaAdapter.StopCapture();
                this.TestSite.Assert.IsTrue(true, "Stop Message Analyzer on driver computer should succeed.");

                #endregion
            }

            #region Verify Capture File

            this.TestSite.Log.Add(LogEntryKind.TestStep, "[START VERIFY CAPTURE]");
            this.TestSite.Log.Add(LogEntryKind.Debug, "Start to verify capture file.");
            MaAdapter.ParseAndVerify(this.TestConfig.LocalCapFileName, true);

            #endregion

            this.TestSite.Log.Add(LogEntryKind.TestStep, "[STOP TEST CASE]");

            if (!this.TestConfig.TriggerDisabled)
            {
                #region Clean up

                this.TestSite.Log.Add(LogEntryKind.Debug, "Clean up: DetermineUserAcctMembershipSAMR test case.");
                this.TestSite.Log.Add(LogEntryKind.Debug, "Clean up: Delete the New User in DC.");
                ActiveDirectoryHelper.DeleteUser(this.TestConfig.DomainNewUserUsername);
                this.TestSite.Log.Add(LogEntryKind.Debug, "Clean up: Delete the New Group in DC.");
                ActiveDirectoryHelper.DeleteGroup(this.TestConfig.DomainNewGroup);

                #endregion
            }
        }

        /// <summary>
        /// Delete a User Account
        /// </summary>
        [TestMethod]
        [Priority(0)]
        [Description("Delete a User Account")]
        [TestCategory("non BVT")]
        [DeploymentItem(@"C:\Program Files\Microsoft Message Analyzer\Microsoft.WindowsAzure.Storage.dll")]
        public void DeleteUserAcct()
        {
            #region LocalVariables

            string testName = string.Empty;
            bool testPass = false;

            #endregion

            #region Initialize

            this.TestSite.Log.Add(LogEntryKind.TestStep, "[INITIALIZE TEST CASE]");
            testName = System.Reflection.MethodInfo.GetCurrentMethod().Name;
            this.TestSite.Log.Add(LogEntryKind.Debug, "Get test case name: {0}", testName);
            System.Environment.SetEnvironmentVariable("MS_ADOD_TESTCASENAME", testName);
            this.TestSite.Log.Add(LogEntryKind.Debug, "Load MA capture verification information from PTFConfig and Message Analyzer Adapter.");
            this.TestConfig.UpdateExpectedSequenceFile(this.TestConfig.GetMAExpectedSequenceFile(testName));
            MaAdapter.ConfigureAdapter(
                    this.TestConfig.GetEndpointRoles(),
                    this.TestConfig.LocalCapFileName,
                    this.TestConfig.LocalCapFileName.Replace(".matp", "-Selected.matp"),
                    this.TestConfig.GetMAExpectedSequenceFile(testName));

            if (!this.TestConfig.TriggerDisabled)
            {
                this.TestSite.Log.Add(LogEntryKind.Debug, "Initialize: Add a new user: {0} to AD.", this.TestConfig.DomainNewUserUsername);
                ActiveDirectoryHelper.CreateUser(this.TestConfig.DomainNewUserUsername, this.TestConfig.DomainNewUserPwd);
            }

            #endregion

            if (!this.TestConfig.TriggerDisabled)
            {
                this.TestSite.Log.Add(LogEntryKind.TestStep, "[START TEST CASE]");

                #region Start Message Analyzer

                this.TestSite.Log.Add(LogEntryKind.Debug, "Start Message Analyzer on driver computer.");
                MaAdapter.StartCapture(false);
                this.TestSite.Assert.IsTrue(true, "Start Message Analyzer on driver computer should succeed.");

                #endregion

                #region Trigger

                this.TestSite.Log.Add(LogEntryKind.Debug, "Trigger client to delete a user account.");
                testPass = this.ClientControlAdapter.DeleteUserAcct();
                this.TestSite.Assert.IsTrue(testPass, "Delete a user account should succeed.");

                #endregion

                #region Verify Server

                this.TestSite.Log.Add(LogEntryKind.Debug, "Start to check state change in server side");
                testPass = this.DCControlAdapter.IsDeleteUserAcctSuccess(this.TestConfig.DomainNewUserUsername);
                this.TestSite.Assert.IsTrue(testPass, "Delete a user account, check server state update after deleting the user account should succeed.");
                this.TestSite.Log.Add(LogEntryKind.CheckSucceeded, "Check if server state updated successfully after deleting the user account", testPass);

                #endregion

                #region Stop Message Analyzer

                this.TestSite.Log.Add(LogEntryKind.Debug, "Stop Message Analyzer on driver computer.");
                MaAdapter.StopCapture();
                this.TestSite.Assert.IsTrue(true, "Stop Message Analyzer on driver computer should succeed.");

                #endregion
            }

            #region Verify Capture File

            this.TestSite.Log.Add(LogEntryKind.TestStep, "[START VERIFY CAPTURE]");
            this.TestSite.Log.Add(LogEntryKind.Debug, "Start to verify capture file.");
            MaAdapter.ParseAndVerify(this.TestConfig.LocalCapFileName, true);

            #endregion

            this.TestSite.Log.Add(LogEntryKind.TestStep, "[STOP TEST CASE]");
        }

        /// <summary>
        /// Obtain a List of User Accounts using LDAP
        /// </summary>
        [TestMethod]
        [Priority(0)]
        [Description("Obtain a List of User Accounts using LDAP")]
        [TestCategory("non BVT")]
        [DeploymentItem(@"C:\Program Files\Microsoft Message Analyzer\Microsoft.WindowsAzure.Storage.dll")]
        public void ObtainUserAcctListLDAP()
        {
            #region LocalVariables

            string testName = string.Empty;
            string testResult = string.Empty;
            bool testPass = false;

            #endregion

            #region Initialize

            this.TestSite.Log.Add(LogEntryKind.TestStep, "[INITIALIZE TEST CASE]");
            testName = System.Reflection.MethodInfo.GetCurrentMethod().Name;
            this.TestSite.Log.Add(LogEntryKind.Debug, "Get test case name: {0}", testName);
            System.Environment.SetEnvironmentVariable("MS_ADOD_TESTCASENAME", testName);
            this.TestSite.Log.Add(LogEntryKind.Debug, "Load MA capture verification information from PTFConfig and Message Analyzer Adapter.");
            this.TestConfig.UpdateExpectedSequenceFile(this.TestConfig.GetMAExpectedSequenceFile(testName));
            MaAdapter.ConfigureAdapter(
                    this.TestConfig.GetEndpointRoles(),
                    this.TestConfig.LocalCapFileName,
                    this.TestConfig.LocalCapFileName.Replace(".matp", "-Selected.matp"),
                    this.TestConfig.GetMAExpectedSequenceFile(testName));

            if (!this.TestConfig.TriggerDisabled)
            {
                this.TestSite.Log.Add(LogEntryKind.Debug, "Initialize: Add a new user: {0} to AD first.", this.TestConfig.DomainNewUserUsername);
                ActiveDirectoryHelper.CreateUser(this.TestConfig.DomainNewUserUsername, this.TestConfig.DomainNewUserPwd);
            }

            #endregion

            if (!this.TestConfig.TriggerDisabled)
            {
                this.TestSite.Log.Add(LogEntryKind.TestStep, "[START TEST CASE]");

                #region Start Message Analyzer

                this.TestSite.Log.Add(LogEntryKind.Debug, "Start Message Analyzer on driver computer.");
                MaAdapter.StartCapture(false);
                this.TestSite.Assert.IsTrue(true, "Start Message Analyzer on driver computer should succeed.");

                #endregion

                #region Trigger

                this.TestSite.Log.Add(LogEntryKind.Debug, "Trigger client to obtain a list of user accounts.");
                //need to add some filters for user account listing?
                //here obtain all users in AD
                testResult = this.ClientControlAdapter.ObtainUserAcctListLDAP();
                this.TestSite.Assert.IsNotNull(testResult, "Obtain a list of user accounts should succeed.");

                #endregion

                #region Verify

                testPass = testResult.Contains(this.TestConfig.DomainNewUserUsername);
                this.TestSite.Assert.IsTrue(testPass, "Verify User Account List should succeed.");
                this.TestSite.Log.Add(LogEntryKind.CheckSucceeded, "Check correctness of obtained user account list.", testPass);

                #endregion

                #region Stop Message Analyzer

                this.TestSite.Log.Add(LogEntryKind.Debug, "Stop Message Analyzer on driver computer.");
                MaAdapter.StopCapture();
                this.TestSite.Assert.IsTrue(true, "Stop Message Analyzer on driver computer should succeed.");

                #endregion
            }

            #region Verify Capture File

            this.TestSite.Log.Add(LogEntryKind.TestStep, "[START VERIFY CAPTURE]");
            this.TestSite.Log.Add(LogEntryKind.Debug, "Start to verify capture file.");
            MaAdapter.ParseAndVerify(this.TestConfig.LocalCapFileName, true);

            #endregion

            this.TestSite.Log.Add(LogEntryKind.TestStep, "[STOP TEST CASE]");
    
            if (!this.TestConfig.TriggerDisabled)
            {
                #region Clean up

                this.TestSite.Log.Add(LogEntryKind.Debug, "Clean up: ObtainUserAcctListLDAP test case.");
                this.TestSite.Log.Add(LogEntryKind.Debug, "Clean up: Delete the New User in DC.");
                ActiveDirectoryHelper.DeleteUser(this.TestConfig.DomainNewUserUsername);

                #endregion
            }
        }

        /// <summary>
        /// Manage Groups and Their Memberships
        /// </summary>
        [TestMethod]
        [Priority(0)]
        [Description("Manage Groups and Their Memberships")]
        [TestCategory("non BVT")]
        [DeploymentItem(@"C:\Program Files\Microsoft Message Analyzer\Microsoft.WindowsAzure.Storage.dll")]
        public void ManageGroupsandTheirMemberships()
        {
            #region LocalVariables

            string testName = string.Empty;
            string testResult = string.Empty;
            bool testPass = false;

            #endregion

            #region Initialize

            this.TestSite.Log.Add(LogEntryKind.TestStep, "[INITIALIZE TEST CASE]");
            testName = System.Reflection.MethodInfo.GetCurrentMethod().Name;
            this.TestSite.Log.Add(LogEntryKind.Debug, "Get test case name: {0}", testName);
            System.Environment.SetEnvironmentVariable("MS_ADOD_TESTCASENAME", testName);
            this.TestSite.Log.Add(LogEntryKind.Debug, "Load MA capture verification information from PTFConfig and Message Analyzer Adapter.");
            this.TestConfig.UpdateExpectedSequenceFile(this.TestConfig.GetMAExpectedSequenceFile(testName));
            MaAdapter.ConfigureAdapter(
                    this.TestConfig.GetEndpointRoles(),
                    this.TestConfig.LocalCapFileName,
                    this.TestConfig.LocalCapFileName.Replace(".matp", "-Selected.matp"),
                    this.TestConfig.GetMAExpectedSequenceFile(testName));

            if (!this.TestConfig.TriggerDisabled)
            {
                this.TestSite.Log.Add(LogEntryKind.Debug, "Initialize: Add a new user: {0} to AD first.", this.TestConfig.DomainNewUserUsername);
                ActiveDirectoryHelper.CreateUser(this.TestConfig.DomainNewUserUsername, this.TestConfig.DomainNewUserPwd);
            }

            #endregion

            if (!this.TestConfig.TriggerDisabled)
            {
                this.TestSite.Log.Add(LogEntryKind.TestStep, "[START TEST CASE]");

                #region Start Message Analyzer

                this.TestSite.Log.Add(LogEntryKind.Debug, "Start Message Analyzer on driver computer.");
                MaAdapter.StartCapture(false);
                this.TestSite.Assert.IsTrue(true, "Start Message Analyzer on driver computer should succeed.");

                #endregion

                #region Trigger

                this.TestSite.Log.Add(LogEntryKind.Debug, "Trigger client to obtain a list of user accounts.");
                testResult = this.ClientControlAdapter.ManageGroupsandTheirMemberships();
                this.TestSite.Assert.IsNotNull(testResult, "Obtain a list of user accounts should succeed.");

                #endregion

                #region Verify

                testPass = this.DCControlAdapter.IsManageGroupsandTheirMembershipsSuccess(this.TestConfig.DomainNewGroup);
                this.TestSite.Assert.IsTrue(testPass, "Verify New created Groups should succeed.");
                testPass = testResult.ToLower(CultureInfo.InvariantCulture).Contains(this.TestConfig.DomainNewUserUsername.ToLower(CultureInfo.InvariantCulture));
                this.TestSite.Assert.IsTrue(testPass, "Verify New created Groups and their memberships should succeed.");
                this.TestSite.Log.Add(LogEntryKind.CheckSucceeded, "Check correctness of obtained user account list.", testPass);

                #endregion

                #region Stop Message Analyzer

                this.TestSite.Log.Add(LogEntryKind.Debug, "Stop Message Analyzer on driver computer.");
                MaAdapter.StopCapture();
                this.TestSite.Assert.IsTrue(true, "Stop Message Analyzer on driver computer should succeed.");

                #endregion
            }

            #region Verify Capture File

            this.TestSite.Log.Add(LogEntryKind.TestStep, "[START VERIFY CAPTURE]");
            this.TestSite.Log.Add(LogEntryKind.Debug, "Start to verify capture file.");
            MaAdapter.ParseAndVerify(this.TestConfig.LocalCapFileName, true);

            #endregion

            this.TestSite.Log.Add(LogEntryKind.TestStep, "[STOP TEST CASE]");

            if (!this.TestConfig.TriggerDisabled)
            {
                #region Clean up

                this.TestSite.Log.Add(LogEntryKind.Debug, "Clean up: ManageGroupsandTheirMemberships test case.");
                this.TestSite.Log.Add(LogEntryKind.Debug, "Clean up: Delete the New User in DC.");
                ActiveDirectoryHelper.DeleteUser(this.TestConfig.DomainNewUserUsername);
                this.TestSite.Log.Add(LogEntryKind.Debug, "Clean up: Delete the New Group in DC.");
                ActiveDirectoryHelper.DeleteGroup(this.TestConfig.DomainNewGroup);

                #endregion
            }
        }

        /// <summary>
        /// Delete a Group
        /// </summary>
        [TestMethod]
        [Priority(0)]
        [Description("Delete a Group")]
        [TestCategory("non BVT")]
        [DeploymentItem(@"C:\Program Files\Microsoft Message Analyzer\Microsoft.WindowsAzure.Storage.dll")]
        public void DeleteGroup()
        {
            #region LocalVariables

            string testName = string.Empty;
            bool testPass = false;

            #endregion

            #region Initialize

            this.TestSite.Log.Add(LogEntryKind.TestStep, "[INITIALIZE TEST CASE]");
            testName = System.Reflection.MethodInfo.GetCurrentMethod().Name;
            this.TestSite.Log.Add(LogEntryKind.Debug, "Get test case name: {0}", testName);
            System.Environment.SetEnvironmentVariable("MS_ADOD_TESTCASENAME", testName);
            this.TestSite.Log.Add(LogEntryKind.Debug, "Load MA capture verification information from PTFConfig and Message Analyzer Adapter.");
            this.TestConfig.UpdateExpectedSequenceFile(this.TestConfig.GetMAExpectedSequenceFile(testName));
            MaAdapter.ConfigureAdapter(
                    this.TestConfig.GetEndpointRoles(),
                    this.TestConfig.LocalCapFileName,
                    this.TestConfig.LocalCapFileName.Replace(".matp", "-Selected.matp"),
                    this.TestConfig.GetMAExpectedSequenceFile(testName));

            if (!this.TestConfig.TriggerDisabled)
            {
                this.TestSite.Log.Add(LogEntryKind.Debug, "Initialize: Add a new group: {0} to AD first.", this.TestConfig.DomainNewGroup);
                ActiveDirectoryHelper.CreateGroup(this.TestConfig.DomainNewGroup);
            }

            #endregion

            if (!this.TestConfig.TriggerDisabled)
            {
                this.TestSite.Log.Add(LogEntryKind.TestStep, "[START TEST CASE]");

                #region Start Message Analyzer

                this.TestSite.Log.Add(LogEntryKind.Debug, "Start Message Analyzer on driver computer.");
                MaAdapter.StartCapture(false);
                this.TestSite.Assert.IsTrue(true, "Start Message Analyzer on driver computer should succeed.");

                #endregion

                #region Trigger

                this.TestSite.Log.Add(LogEntryKind.Debug, "Trigger client to obtain a list of user accounts.");
                testPass = this.ClientControlAdapter.DeleteGroup();
                this.TestSite.Assert.IsTrue(testPass, "Delete a group should succeed.");

                #endregion

                #region Verify

                testPass = this.DCControlAdapter.IsDeleteGroupSuccess(this.TestConfig.DomainNewGroup);
                this.TestSite.Assert.IsTrue(testPass, "Verify Delete Group should succeed.");
                this.TestSite.Log.Add(LogEntryKind.CheckSucceeded, "Check correctness of obtained user account list.", testPass);

                #endregion

                #region Stop Message Analyzer

                this.TestSite.Log.Add(LogEntryKind.Debug, "Stop Message Analyzer on driver computer.");
                MaAdapter.StopCapture();
                this.TestSite.Assert.IsTrue(true, "Stop Message Analyzer on driver computer should succeed.");

                #endregion
            }

            #region Verify Capture File

            this.TestSite.Log.Add(LogEntryKind.TestStep, "[START VERIFY CAPTURE]");
            this.TestSite.Log.Add(LogEntryKind.Debug, "Start to verify capture file.");
            MaAdapter.ParseAndVerify(this.TestConfig.LocalCapFileName, true);

            #endregion

            this.TestSite.Log.Add(LogEntryKind.TestStep, "[STOP TEST CASE]");

            if (!this.TestConfig.TriggerDisabled)
            {
                #region Clean up

                this.TestSite.Log.Add(LogEntryKind.Debug, "Clean up: DeleteGroup test case.");

                #endregion
            }
        }

        #endregion
    }
}
