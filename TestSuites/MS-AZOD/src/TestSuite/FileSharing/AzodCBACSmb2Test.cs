// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.IO;
using System.Diagnostics;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Protocols.TestTools;
using Microsoft.Protocol.TestSuites.Azod.Adapter;
using Microsoft.Protocol.TestSuites.Azod.Adapter.Util;

namespace Microsoft.Protocol.TestSuites.Azod.TestSuite
{
    [TestClass]
    public class AzodCbacKerberosSmb2TestSuite : AzodTestClassBase
    {
        [ClassInitialize]
        public static void ClassInitialize(TestContext testContext)
        {
            AzodTestClassBase.Initialize(testContext);           
        }

        [ClassCleanup]
        public static void ClassCleanup()
        {
            AzodTestClassBase.Cleanup();
        }

        protected override void TestCleanup()
        {                     
            base.TestCleanup();
        }

        [TestCategory("Non-BVT")]        
        [Priority(0)]
        [Description("Observer")]
        [TestMethod]
        public void CbacNtlmSmb2()
        {            
            string testName = string.Empty;            
            this.TestSite.Log.Add(LogEntryKind.Debug, "Initialize test case.");
            testName = System.Reflection.MethodInfo.GetCurrentMethod().Name;
            RunObserverTestCase(testName);
        }

        [TestCategory("Non-BVT")]        
        [Priority(0)]
        [Description("Observer")]
        [TestMethod]
        public void CbacKerberosSmb2()
        {
            string testName = string.Empty;
            this.TestSite.Log.Add(LogEntryKind.Debug, "Initialize test case.");
            testName = System.Reflection.MethodInfo.GetCurrentMethod().Name;
            RunObserverTestCase(testName);
        }
       
        [TestCategory("Non-BVT")]        
        [Priority(0)]
        [Description("Observer")]
        [TestMethod]
        public void CbacSfuSmb2()
        {
            string testName = string.Empty;
            this.TestSite.Log.Add(LogEntryKind.Debug, "Initialize test case.");
            testName = System.Reflection.MethodInfo.GetCurrentMethod().Name;
            RunObserverTestCase(testName);           
        }

        [TestCategory("Non-BVT")]
        [Priority(0)]
        [Description("Observer")]
        [TestMethod]
        public void CtaSmb2()
        {
            string testName = string.Empty;
            this.TestSite.Log.Add(LogEntryKind.Debug, "Initialize test case.");
            testName = System.Reflection.MethodInfo.GetCurrentMethod().Name;
            RunObserverTestCase(testName);
        }
        
        [TestCategory("BVT")]        
        [Priority(0)]        
        [TestMethod]
        public void CountryCodeEquals156Policy()
        {
            string testName = string.Empty;

            #region Initialize
            testName = System.Reflection.MethodInfo.GetCurrentMethod().Name;
            this.TestSite.Log.Add(LogEntryKind.Debug, "Get test case name: {0}", testName);
            System.Environment.SetEnvironmentVariable("MS_AZOD_TESTCASENAME", testName);
            #endregion

            #region config environment
            this.TestSite.Log.Add(LogEntryKind.Debug, "Turn on CBAC on client computer.");
            this.sutController.CbacSwitch("1", this.TestConfig.ClientComputerName, this.TestConfig.KdcDomainName + "\\" + this.TestConfig.KdcAdminUser, this.TestConfig.KdcAdminPwd);

            this.TestSite.Log.Add(LogEntryKind.Debug, "Clean up cached tickets on client computer.");
            this.sutController.CleanupCachedTickets(this.TestConfig.ClientComputerName, this.TestConfig.KdcDomainName + "\\" + this.TestConfig.KdcAdminUser, this.TestConfig.KdcAdminPwd);

            #endregion

            base.Logging();
            string tempString = testName.Substring(0, testName.Length - 6);
            string uncPath = string.Format(@"\\{0}.{1}\{2}Allowed", this.TestConfig.ApplicationServerName, this.TestConfig.KdcDomainName, tempString);
            string fileName = testName + ".txt";

            string password = "Password01@";
            string userName = "PayrollMember01";

            string domainName = this.TestConfig.KdcDomainName;
            bool returnValue = true;

            //Check whether the user has the write permisson to the share folder
            //Expect to succeed
            fileName = string.Format(@"{0}{1}.txt", testName, userName);
            returnValue = this.sutController.WriteToShareFolder(uncPath, fileName, userName, password, domainName, string.Format("{0}-{1}-{2}.log", testName,userName,"WriteAccess" ));

            this.TestSite.Assert.IsTrue(returnValue, string.Format("User {0} should have the write access control to share folder {1}. If failed, please check the user name, password, user claim or the folder permission.", userName, uncPath));

            password = "Password01@";
            userName = "PayrollMember02";

            //Check whether the user has the write permisson to the share folder.
            //Expect to fail
            fileName = string.Format(@"{0}{1}.txt", testName, userName);
            returnValue = this.sutController.WriteToShareFolder(uncPath, fileName, userName, password, domainName, string.Format("{0}-{1}-{2}.log", testName, userName, "WriteAccess"));

            this.TestSite.Assert.IsFalse(returnValue, string.Format("User {0} should not have the write access control to share folder {1}. If it has, please check the user name, password, user claims or the folder permission.", userName, uncPath));

            //Check whether the user has the read permisson to the share folder
            //Expect to fail
            returnValue = this.sutController.ReadShareFolder(uncPath, userName, password, domainName, string.Format("{0}-{1}-{2}.log", testName, userName, "ReadAccess"));

            this.TestSite.Assert.IsFalse(returnValue, string.Format("User {0} should not have the read access control to share folder {1}. If it has, please check the user name, password, user claims or the folder permission.", userName, uncPath));
                 

        }

        [TestCategory("BVT")]    
        [Priority(0)]
        [TestMethod]
        public void CountryCodeNotEquals156Policy()
        {
            string testName = string.Empty;

            #region Initialize
            testName = System.Reflection.MethodInfo.GetCurrentMethod().Name;
            this.TestSite.Log.Add(LogEntryKind.Debug, "Get test case name: {0}", testName);
            System.Environment.SetEnvironmentVariable("MS_AZOD_TESTCASENAME", testName);
            #endregion

            #region config environment
            this.TestSite.Log.Add(LogEntryKind.Debug, "Turn on CBAC on client computer.");
            this.sutController.CbacSwitch("1", this.TestConfig.ClientComputerName, this.TestConfig.KdcDomainName + "\\" + this.TestConfig.KdcAdminUser, this.TestConfig.KdcAdminPwd);

            this.TestSite.Log.Add(LogEntryKind.Debug, "Clean up cached tickets on client computer.");
            this.sutController.CleanupCachedTickets(this.TestConfig.ClientComputerName, this.TestConfig.KdcDomainName + "\\" + this.TestConfig.KdcAdminUser, this.TestConfig.KdcAdminPwd);

            #endregion

            base.Logging();
            string tempString = testName.Substring(0, testName.Length - 6);
            string uncPath = string.Format(@"\\{0}.{1}\{2}Allowed", this.TestConfig.ApplicationServerName, this.TestConfig.KdcDomainName, tempString);
            string fileName = testName + ".txt";

            string password = "Password01@";
            string userName = "PayrollMember01";

            string domainName = this.TestConfig.KdcDomainName;
            bool returnValue = true;

            //Check whether the user has the write permission to the share folder.
            //Expect to fail
            fileName = string.Format(@"{0}{1}.txt", testName, userName);
            returnValue = this.sutController.WriteToShareFolder(uncPath, fileName, userName, password, domainName, string.Format("{0}-{1}-{2}.log", testName, userName, "WriteAccess"));
            this.TestSite.Assert.IsFalse(returnValue, string.Format("User {0} should not have the write access control to share folder {1}. If it has, please check the user name, password, user claims or the folder permission.", userName, uncPath));

            //Check whether the user has the read permission to the share folder
            //Expect to fail
            returnValue = this.sutController.ReadShareFolder(uncPath, userName, password, domainName, string.Format("{0}-{1}-{2}.log", testName, userName, "ReadAccess"));
            this.TestSite.Assert.IsFalse(returnValue, string.Format("User {0} should not have the read access control to share folder {1}. If it has, please check the user name, password, user claims or the folder permission.", userName, uncPath));
            
            password = "Password01@";
            userName = "PayrollMember02";

            //Check whether the user has the write permission to the share folder
            //Expect to succeed
            fileName = string.Format(@"{0}{1}.txt", testName, userName);
            returnValue = this.sutController.WriteToShareFolder(uncPath, fileName, userName, password, domainName, string.Format("{0}-{1}-{2}.log", testName, userName, "WriteAccess"));
            this.TestSite.Assert.IsTrue(returnValue, string.Format("User {0} should have the write access control to share folder {1}. If failed, please check the user name, password, user claim or the folder permission.", userName, uncPath));
        }

        [TestCategory("BVT")]    
        [Priority(0)]
        [TestMethod]
        public void CountryCodeLessThan392Policy()
        {
            string testName = string.Empty;

            #region Initialize
            testName = System.Reflection.MethodInfo.GetCurrentMethod().Name;
            this.TestSite.Log.Add(LogEntryKind.Debug, "Get test case name: {0}", testName);
            System.Environment.SetEnvironmentVariable("MS_AZOD_TESTCASENAME", testName);
            #endregion

            #region config environment
            this.TestSite.Log.Add(LogEntryKind.Debug, "Turn on CBAC on client computer.");
            this.sutController.CbacSwitch("1", this.TestConfig.ClientComputerName, this.TestConfig.KdcDomainName + "\\" + this.TestConfig.KdcAdminUser, this.TestConfig.KdcAdminPwd);

            this.TestSite.Log.Add(LogEntryKind.Debug, "Clean up cached tickets on client computer.");
            this.sutController.CleanupCachedTickets(this.TestConfig.ClientComputerName, this.TestConfig.KdcDomainName + "\\" + this.TestConfig.KdcAdminUser, this.TestConfig.KdcAdminPwd);

            #endregion

            base.Logging();
            string tempString = testName.Substring(0, testName.Length - 6);
            string uncPath = string.Format(@"\\{0}.{1}\{2}Allowed", this.TestConfig.ApplicationServerName, this.TestConfig.KdcDomainName, tempString);
            string fileName = testName + ".txt";

            string domainName = this.TestConfig.KdcDomainName;
            bool returnValue = true;

            string password = "Password01@";
            string userName = "PayrollMember01";

            //Check whether the user has the write permission to the share folder
            //Expect to succeed
            fileName = string.Format(@"{0}{1}.txt", testName, userName);
            returnValue = this.sutController.WriteToShareFolder(uncPath, fileName, userName, password, domainName, string.Format("{0}-{1}-{2}.log", testName, userName, "WriteAccess"));
            this.TestSite.Assert.IsTrue(returnValue, string.Format("User {0} should have the write access control to share folder {1}. If failed, please check the user name, password, user claim or the folder permission.", userName, uncPath));

            password = "Password01@";
            userName = "PayrollMember02";

            //Check whether the user has the write permission to the share folder.
            //Expect to fail
            fileName = string.Format(@"{0}{1}.txt", testName, userName);
            returnValue = this.sutController.WriteToShareFolder(uncPath, fileName, userName, password, domainName, string.Format("{0}-{1}-{2}.log", testName, userName, "WriteAccess"));
            this.TestSite.Assert.IsFalse(returnValue, string.Format("User {0} should not have the write access control to share folder {1}. If it has, please check the user name, password, user claims or the folder permission.", userName, uncPath));

            //Check whether the user has the read permission to the share folder
            //Expect to fail
            returnValue = this.sutController.ReadShareFolder(uncPath, userName, password, domainName, string.Format("{0}-{1}-{2}.log", testName, userName, "ReadAccess"));
            this.TestSite.Assert.IsFalse(returnValue, string.Format("User {0} should not have the read access control to share folder {1}. If it has, please check the user name, password, user claims or the folder permission.", userName, uncPath));

            password = "Password01@";
            userName = "PayrollMember03";

            //Check whether the user has the write permission to the share folder.
            //Expect to fail
            fileName = string.Format(@"{0}{1}.txt", testName, userName);
            returnValue = this.sutController.WriteToShareFolder(uncPath, fileName, userName, password, domainName, string.Format("{0}-{1}-{2}.log", testName, userName, "WriteAccess"));
            this.TestSite.Assert.IsFalse(returnValue, string.Format("User {0} should not have the write access control to share folder {1}. If it has, please check the user name, password, user claims or the folder permission.", userName, uncPath));

            //Check whether the user has the read permission to the share folder
            //Expect to fail
            returnValue = this.sutController.ReadShareFolder(uncPath, userName, password, domainName, string.Format("{0}-{1}-{2}.log", testName, userName, "ReadAccess"));
            this.TestSite.Assert.IsFalse(returnValue, string.Format("User {0} should not have the read access control to share folder {1}. If it has, please check the user name, password, user claims or the folder permission.", userName, uncPath));

        }

        [TestCategory("BVT")]    
        [Priority(0)]
        [TestMethod]
        public void CountryCodeLessThanOrEquals392Policy()
        {
            string testName = string.Empty;

            #region Initialize
            testName = System.Reflection.MethodInfo.GetCurrentMethod().Name;
            this.TestSite.Log.Add(LogEntryKind.Debug, "Get test case name: {0}", testName);
            System.Environment.SetEnvironmentVariable("MS_AZOD_TESTCASENAME", testName);
            #endregion

            #region config environment
            this.TestSite.Log.Add(LogEntryKind.Debug, "Turn on CBAC on client computer.");
            this.sutController.CbacSwitch("1", this.TestConfig.ClientComputerName, this.TestConfig.KdcDomainName + "\\" + this.TestConfig.KdcAdminUser, this.TestConfig.KdcAdminPwd);

            this.TestSite.Log.Add(LogEntryKind.Debug, "Clean up cached tickets on client computer.");
            this.sutController.CleanupCachedTickets(this.TestConfig.ClientComputerName, this.TestConfig.KdcDomainName + "\\" + this.TestConfig.KdcAdminUser, this.TestConfig.KdcAdminPwd);

            #endregion

            base.Logging();
            string tempString = testName.Substring(0, testName.Length - 6);
            string uncPath = string.Format(@"\\{0}.{1}\{2}Allowed", this.TestConfig.ApplicationServerName, this.TestConfig.KdcDomainName, tempString);
            string fileName = testName + ".txt";

            string domainName = this.TestConfig.KdcDomainName;
            bool returnValue = true;

            string password = "Password01@";
            string userName = "PayrollMember01";

            //Check whether the user has the write permission to the share folder
            //Expect to succeed
            fileName = string.Format(@"{0}{1}.txt", testName, userName);
            returnValue = this.sutController.WriteToShareFolder(uncPath, fileName, userName, password, domainName, string.Format("{0}-{1}-{2}.log", testName, userName, "WriteAccess"));
            this.TestSite.Assert.IsTrue(returnValue, string.Format("User {0} should have the write access control to share folder {1}. If failed, please check the user name, password, user claim or the folder permission.", userName, uncPath));

            password = "Password01@";
            userName = "PayrollMember02";

            //Check whether the user has the write permission to the share folder.
            //Expect to fail
            fileName = string.Format(@"{0}{1}.txt", testName, userName);
            returnValue = this.sutController.WriteToShareFolder(uncPath, fileName, userName, password, domainName, string.Format("{0}-{1}-{2}.log", testName, userName, "WriteAccess"));
            this.TestSite.Assert.IsFalse(returnValue, string.Format("User {0} should not have the write access control to share folder {1}. If it has, please check the user name, password, user claims or the folder permission.", userName, uncPath));

            //Check whether the user has the read permission to the share folder
            //Expect to fail
            returnValue = this.sutController.ReadShareFolder(uncPath, userName, password, domainName, string.Format("{0}-{1}-{2}.log", testName, userName, "ReadAccess"));
            this.TestSite.Assert.IsFalse(returnValue, string.Format("User {0} should not have the read access control to share folder {1}. If it has, please check the user name, password, user claims or the folder permission.", userName, uncPath));
            
            password = "Password01@";
            userName = "PayrollMember03";

            //Check whether the user has the write permission to the share folder.
            //Expect to succeed
            fileName = string.Format(@"{0}{1}.txt", testName, userName);
            returnValue = this.sutController.WriteToShareFolder(uncPath, fileName, userName, password, domainName, string.Format("{0}-{1}-{2}.log", testName, userName, "WriteAccess"));
            this.TestSite.Assert.IsTrue(returnValue, string.Format("User {0} should have the write access control to share folder {1}. If failed, please check the user name, password, user claim or the folder permission.", userName, uncPath));
            
        }

        [TestCategory("BVT")]    
        [Priority(0)]
        [TestMethod]
        public void CountryCodeGreaterThan392Policy()
        {
            string testName = string.Empty;

            #region Initialize
            testName = System.Reflection.MethodInfo.GetCurrentMethod().Name;
            this.TestSite.Log.Add(LogEntryKind.Debug, "Get test case name: {0}", testName);
            System.Environment.SetEnvironmentVariable("MS_AZOD_TESTCASENAME", testName);
            #endregion

            #region config environment
            this.TestSite.Log.Add(LogEntryKind.Debug, "Turn on CBAC on client computer.");
            this.sutController.CbacSwitch("1", this.TestConfig.ClientComputerName, this.TestConfig.KdcDomainName + "\\" + this.TestConfig.KdcAdminUser, this.TestConfig.KdcAdminPwd);

            this.TestSite.Log.Add(LogEntryKind.Debug, "Clean up cached tickets on client computer.");
            this.sutController.CleanupCachedTickets(this.TestConfig.ClientComputerName, this.TestConfig.KdcDomainName + "\\" + this.TestConfig.KdcAdminUser, this.TestConfig.KdcAdminPwd);

            #endregion

            base.Logging();
            string tempString = testName.Substring(0, testName.Length - 6);
            string uncPath = string.Format(@"\\{0}.{1}\{2}Allowed", this.TestConfig.ApplicationServerName, this.TestConfig.KdcDomainName, tempString);
            string fileName = testName + ".txt";

            string domainName = this.TestConfig.KdcDomainName;
            bool returnValue = true;

            string password = "Password01@";
            string userName = "PayrollMember01";

           //Check whether the user has the write permission to the share folder.
            //Expect to fail
            fileName = string.Format(@"{0}{1}.txt", testName, userName);
            returnValue = this.sutController.WriteToShareFolder(uncPath, fileName, userName, password, domainName, string.Format("{0}-{1}-{2}.log", testName, userName, "WriteAccess"));
            this.TestSite.Assert.IsFalse(returnValue, string.Format("User {0} should not have the write access control to share folder {1}. If it has, please check the user name, password, user claims or the folder permission.", userName, uncPath));

            //Check whether the user has the read permission to the share folder
            //Expect to fail
            returnValue = this.sutController.ReadShareFolder(uncPath, userName, password, domainName, string.Format("{0}-{1}-{2}.log", testName, userName, "ReadAccess"));
            this.TestSite.Assert.IsFalse(returnValue, string.Format("User {0} should not have the read access control to share folder {1}. If it has, please check the user name, password, user claims or the folder permission.", userName, uncPath));          

            password = "Password01@";
            userName = "PayrollMember02";
            //Check whether the user has the write permission to the share folder
            //Expect to succeed
            fileName = string.Format(@"{0}{1}.txt", testName, userName);
            returnValue = this.sutController.WriteToShareFolder(uncPath, fileName, userName, password, domainName, string.Format("{0}-{1}-{2}.log", testName, userName, "WriteAccess"));
            this.TestSite.Assert.IsTrue(returnValue, string.Format("User {0} should have the write access control to share folder {1}. If failed, please check the user name, password, user claim or the folder permission.", userName, uncPath));

            password = "Password01@";
            userName = "PayrollMember03";

            //Check whether the user has the write permission to the share folder.
            //Expect to fail
            fileName = string.Format(@"{0}{1}.txt", testName, userName);
            returnValue = this.sutController.WriteToShareFolder(uncPath, fileName, userName, password, domainName, string.Format("{0}-{1}-{2}.log", testName, userName, "WriteAccess"));
            this.TestSite.Assert.IsFalse(returnValue, string.Format("User {0} should not have the write access control to share folder {1}. If it has, please check the user name, password, user claims or the folder permission.", userName, uncPath));

            //Check whether the user has the read permission to the share folder
            //Expect to fail
            returnValue = this.sutController.ReadShareFolder(uncPath, userName, password, domainName, string.Format("{0}-{1}-{2}.log", testName, userName, "ReadAccess"));
            this.TestSite.Assert.IsFalse(returnValue, string.Format("User {0} should not have the read access control to share folder {1}. If it has, please check the user name, password, user claims or the folder permission.", userName, uncPath));
          
        }

        [TestCategory("BVT")]    
        [Priority(0)]
        [TestMethod]
        public void CountryCodeGreaterThanOrEquals392Policy()
        {
            string testName = string.Empty;

            #region Initialize
            testName = System.Reflection.MethodInfo.GetCurrentMethod().Name;
            this.TestSite.Log.Add(LogEntryKind.Debug, "Get test case name: {0}", testName);
            System.Environment.SetEnvironmentVariable("MS_AZOD_TESTCASENAME", testName);
            #endregion

            #region config environment
            this.TestSite.Log.Add(LogEntryKind.Debug, "Turn on CBAC on client computer.");
            this.sutController.CbacSwitch("1", this.TestConfig.ClientComputerName, this.TestConfig.KdcDomainName + "\\" + this.TestConfig.KdcAdminUser, this.TestConfig.KdcAdminPwd);

            this.TestSite.Log.Add(LogEntryKind.Debug, "Clean up cached tickets on client computer.");
            this.sutController.CleanupCachedTickets(this.TestConfig.ClientComputerName, this.TestConfig.KdcDomainName + "\\" + this.TestConfig.KdcAdminUser, this.TestConfig.KdcAdminPwd);

            #endregion

            base.Logging();
            string tempString = testName.Substring(0, testName.Length - 6);
            string uncPath = string.Format(@"\\{0}.{1}\{2}Allowed", this.TestConfig.ApplicationServerName, this.TestConfig.KdcDomainName, tempString);
            string fileName = testName + ".txt";

            string domainName = this.TestConfig.KdcDomainName;
            bool returnValue = true;

            string password = "Password01@";
            string userName = "PayrollMember01";

            //Check whether the user has the write permission to the share folder.
            //Expect to fail
            fileName = string.Format(@"{0}{1}.txt", testName, userName);
            returnValue = this.sutController.WriteToShareFolder(uncPath, fileName, userName, password, domainName, string.Format("{0}-{1}-{2}.log", testName, userName, "WriteAccess"));
            this.TestSite.Assert.IsFalse(returnValue, string.Format("User {0} should not have the write access control to share folder {1}. If it has, please check the user name, password, user claims or the folder permission.", userName, uncPath));

            //Check whether the user has the read permission to the share folder
            //Expect to fail
            returnValue = this.sutController.ReadShareFolder(uncPath, userName, password, domainName, string.Format("{0}-{1}-{2}.log", testName, userName, "ReadAccess"));
            this.TestSite.Assert.IsFalse(returnValue, string.Format("User {0} should not have the read access control to share folder {1}. If it has, please check the user name, password, user claims or the folder permission.", userName, uncPath));

            password = "Password01@";
            userName = "PayrollMember02";
            //Check whether the user has the write permission to the share folder
            //Expect to succeed
            fileName = string.Format(@"{0}{1}.txt", testName, userName);
            returnValue = this.sutController.WriteToShareFolder(uncPath, fileName, userName, password, domainName, string.Format("{0}-{1}-{2}.log", testName, userName, "WriteAccess"));
            this.TestSite.Assert.IsTrue(returnValue, string.Format("User {0} should have the write access control to share folder {1}. If failed, please check the user name, password, user claim or the folder permission.", userName, uncPath));

            password = "Password01@";
            userName = "PayrollMember03";
            //Check whether the user has the write permission to the share folder
            //Expect to succeed
            fileName = string.Format(@"{0}{1}.txt", testName, userName);
            returnValue = this.sutController.WriteToShareFolder(uncPath, fileName, userName, password, domainName, string.Format("{0}-{1}-{2}.log", testName, userName, "WriteAccess"));
            this.TestSite.Assert.IsTrue(returnValue, string.Format("User {0} should have the write access control to share folder {1}. If failed, please check the user name, password, user claim or the folder permission.", userName, uncPath));

        }

        [TestCategory("BVT")]    
        [Priority(0)]
        [TestMethod]
        [DeploymentItem(@"C:\Program Files\Microsoft Message Analyzer\Microsoft.WindowsAzure.Storage.dll")]
        public void CountryCodeAnyOf156Or840Policy()
        {
            string testName = string.Empty;

            #region Initialize
            testName = System.Reflection.MethodInfo.GetCurrentMethod().Name;
            this.TestSite.Log.Add(LogEntryKind.Debug, "Get test case name: {0}", testName);
            System.Environment.SetEnvironmentVariable("MS_AZOD_TESTCASENAME", testName);
            #endregion

            #region config environment
            this.TestSite.Log.Add(LogEntryKind.Debug, "Turn on CBAC on client computer.");
            this.sutController.CbacSwitch("1", this.TestConfig.ClientComputerName, this.TestConfig.KdcDomainName + "\\" + this.TestConfig.KdcAdminUser, this.TestConfig.KdcAdminPwd);

            this.TestSite.Log.Add(LogEntryKind.Debug, "Clean up cached tickets on client computer.");
            this.sutController.CleanupCachedTickets(this.TestConfig.ClientComputerName, this.TestConfig.KdcDomainName + "\\" + this.TestConfig.KdcAdminUser, this.TestConfig.KdcAdminPwd);

            #endregion

            base.Logging();
            string tempString = testName.Substring(0, testName.Length - 6);
            string uncPath = string.Format(@"\\{0}.{1}\{2}Allowed", this.TestConfig.ApplicationServerName, this.TestConfig.KdcDomainName, tempString);
            string fileName = testName + ".txt";

            string domainName = this.TestConfig.KdcDomainName;
            bool returnValue = true;

            string password = "Password01@";
            string userName = "PayrollMember01";

            //Check whether the user has the write permission to the share folder
            //Expect to succeed
            fileName = string.Format(@"{0}{1}.txt", testName, userName);
            returnValue = this.sutController.WriteToShareFolder(uncPath, fileName, userName, password, domainName, string.Format("{0}-{1}-{2}.log", testName, userName, "WriteAccess"));
            this.TestSite.Assert.IsTrue(returnValue, string.Format("User {0} should have the write access control to share folder {1}. If failed, please check the user name, password, user claim or the folder permission.", userName, uncPath));
            
            password = "Password01@";
            userName = "PayrollMember03";

            //Check whether the user has the write permission to the share folder.
            //Expect to fail
            fileName = string.Format(@"{0}{1}.txt", testName, userName);
            returnValue = this.sutController.WriteToShareFolder(uncPath, fileName, userName, password, domainName, string.Format("{0}-{1}-{2}.log", testName, userName, "WriteAccess"));
            this.TestSite.Assert.IsFalse(returnValue, string.Format("User {0} should not have the write access control to share folder {1}. If it has, please check the user name, password, user claims or the folder permission.", userName, uncPath));

            //Check whether the user has the read permission to the share folder
            //Expect to fail
            returnValue = this.sutController.ReadShareFolder(uncPath, userName, password, domainName, string.Format("{0}-{1}-{2}.log", testName, userName, "ReadAccess"));
            this.TestSite.Assert.IsFalse(returnValue, string.Format("User {0} should not have the read access control to share folder {1}. If it has, please check the user name, password, user claims or the folder permission.", userName, uncPath));
          
        }

        [TestCategory("BVT")]    
        [Priority(0)]
        [TestMethod]
        public void CountryCodeNotAnyOf156Or840Policy()
        {
            string testName = string.Empty;

            #region Initialize
            testName = System.Reflection.MethodInfo.GetCurrentMethod().Name;
            this.TestSite.Log.Add(LogEntryKind.Debug, "Get test case name: {0}", testName);
            System.Environment.SetEnvironmentVariable("MS_AZOD_TESTCASENAME", testName);
            #endregion

            #region config environment
            this.TestSite.Log.Add(LogEntryKind.Debug, "Turn on CBAC on client computer.");
            this.sutController.CbacSwitch("1", this.TestConfig.ClientComputerName, this.TestConfig.KdcDomainName + "\\" + this.TestConfig.KdcAdminUser, this.TestConfig.KdcAdminPwd);

            this.TestSite.Log.Add(LogEntryKind.Debug, "Clean up cached tickets on client computer.");
            this.sutController.CleanupCachedTickets(this.TestConfig.ClientComputerName, this.TestConfig.KdcDomainName + "\\" + this.TestConfig.KdcAdminUser, this.TestConfig.KdcAdminPwd);

            #endregion

            base.Logging();
            string tempString = testName.Substring(0, testName.Length - 6);
            string uncPath = string.Format(@"\\{0}.{1}\{2}Allowed", this.TestConfig.ApplicationServerName, this.TestConfig.KdcDomainName, tempString);
            string fileName = testName + ".txt";

            string domainName = this.TestConfig.KdcDomainName;
            bool returnValue = true;

            string password = "Password01@";
            string userName = "PayrollMember03";

            //Check whether the user has the write permission to the share folder
            //Expect to succeed
            fileName = string.Format(@"{0}{1}.txt", testName, userName);
            returnValue = this.sutController.WriteToShareFolder(uncPath, fileName, userName, password, domainName, string.Format("{0}-{1}-{2}.log", testName, userName, "WriteAccess"));
            this.TestSite.Assert.IsTrue(returnValue, string.Format("User {0} should have the write access control to share folder {1}. If failed, please check the user name, password, user claim or the folder permission.", userName, uncPath));

            password = "Password01@";
            userName = "PayrollMember01";

            //Check whether the user has the write permission to the share folder.
            //Expect to fail
            returnValue = this.sutController.WriteToShareFolder(uncPath, fileName, userName, password, domainName, string.Format("{0}-{1}-{2}.log", testName, userName, "WriteAccess"));
            this.TestSite.Assert.IsFalse(returnValue, string.Format("User {0} should not have the write access control to share folder {1}. If it has, please check the user name, password, user claims or the folder permission.", userName, uncPath));

            //Check whether the user has the read permission to the share folder
            //Expect to fail
            returnValue = this.sutController.ReadShareFolder(uncPath, userName, password, domainName, string.Format("{0}-{1}-{2}.log", testName, userName, "ReadAccess"));
            this.TestSite.Assert.IsFalse(returnValue, string.Format("User {0} should not have the read access control to share folder {1}. If it has, please check the user name, password, user claims or the folder permission.", userName, uncPath));          
        }

        [TestCategory("BVT")]    
        [Priority(0)]
        [TestMethod]
        public void CountryCodeEquals156AndITDepartmentPolicy()
        {
            string testName = string.Empty;

            #region Initialize
            testName = System.Reflection.MethodInfo.GetCurrentMethod().Name;
            this.TestSite.Log.Add(LogEntryKind.Debug, "Get test case name: {0}", testName);
            System.Environment.SetEnvironmentVariable("MS_AZOD_TESTCASENAME", testName);
            #endregion

            #region config environment
            this.TestSite.Log.Add(LogEntryKind.Debug, "Turn on CBAC on client computer.");
            this.sutController.CbacSwitch("1", this.TestConfig.ClientComputerName, this.TestConfig.KdcDomainName + "\\" + this.TestConfig.KdcAdminUser, this.TestConfig.KdcAdminPwd);

            this.TestSite.Log.Add(LogEntryKind.Debug, "Clean up cached tickets on client computer.");
            this.sutController.CleanupCachedTickets(this.TestConfig.ClientComputerName, this.TestConfig.KdcDomainName + "\\" + this.TestConfig.KdcAdminUser, this.TestConfig.KdcAdminPwd);

            #endregion

            base.Logging();
            string tempString = testName.Substring(0, testName.Length - 6);
            string uncPath = string.Format(@"\\{0}.{1}\{2}Allowed", this.TestConfig.ApplicationServerName, this.TestConfig.KdcDomainName, tempString);
            string fileName = testName + ".txt";

            string domainName = this.TestConfig.KdcDomainName;
            bool returnValue = true;

            string password = "Password01@";
            string userName = "PayrollMember01";

            //Check whether the user has the write permission to the share folder.
            //Expect to fail
            returnValue = this.sutController.WriteToShareFolder(uncPath, fileName, userName, password, domainName, string.Format("{0}-{1}-{2}.log", testName, userName, "WriteAccess"));
            this.TestSite.Assert.IsFalse(returnValue, string.Format("User {0} should not have the write access control to share folder {1}. If it has, please check the user name, password, user claims or the folder permission.", userName, uncPath));

            //Check whether the user has the read permission to the share folder
            //Expect to fail
            returnValue = this.sutController.ReadShareFolder(uncPath, userName, password, domainName, string.Format("{0}-{1}-{2}.log", testName, userName, "ReadAccess"));
            this.TestSite.Assert.IsFalse(returnValue, string.Format("User {0} should not have the read access control to share folder {1}. If it has, please check the user name, password, user claims or the folder permission.", userName, uncPath));

            password = "Password01@";
            userName = "PayrollMember02";

            //Check whether the user has the write permission to the share folder.
            //Expect to fail
            returnValue = this.sutController.WriteToShareFolder(uncPath, fileName, userName, password, domainName, string.Format("{0}-{1}-{2}.log", testName, userName, "WriteAccess"));
            this.TestSite.Assert.IsFalse(returnValue, string.Format("User {0} should not have the write access control to share folder {1}. If it has, please check the user name, password, user claims or the folder permission.", userName, uncPath));

            //Check whether the user has the read permission to the share folder
            //Expect to fail
            returnValue = this.sutController.ReadShareFolder(uncPath, userName, password, domainName, string.Format("{0}-{1}-{2}.log", testName, userName, "ReadAccess"));
            this.TestSite.Assert.IsFalse(returnValue, string.Format("User {0} should not have the read access control to share folder {1}. If it has, please check the user name, password, user claims or the folder permission.", userName, uncPath));

            password = "Password01@";
            userName = "ITMember01";

            //Check whether the user has the write permission to the share folder.
            //Expect to fail
            returnValue = this.sutController.WriteToShareFolder(uncPath, fileName, userName, password, domainName, string.Format("{0}-{1}-{2}.log", testName, userName, "WriteAccess"));
            this.TestSite.Assert.IsFalse(returnValue, string.Format("User {0} should not have the write access control to share folder {1}. If it has, please check the user name, password, user claims or the folder permission.", userName, uncPath));

            //Check whether the user has the read permission to the share folder
            //Expect to fail
            returnValue = this.sutController.ReadShareFolder(uncPath, userName, password, domainName, string.Format("{0}-{1}-{2}.log", testName, userName, "ReadAccess"));
            this.TestSite.Assert.IsFalse(returnValue, string.Format("User {0} should not have the read access control to share folder {1}. If it has, please check the user name, password, user claims or the folder permission.", userName, uncPath));

            password = "Password01@";
            userName = "ITAdmin01";

            //Check whether the user has the write permission to the share folder
            //Expect to succeed
            returnValue = this.sutController.WriteToShareFolder(uncPath, fileName, userName, password, domainName, string.Format("{0}-{1}-{2}.log", testName, userName, "WriteAccess"));
            this.TestSite.Assert.IsTrue(returnValue, string.Format("User {0} should have the write access control to share folder {1}. If failed, please check the user name, password, user claim or the folder permission.", userName, uncPath));
        
        }

        [TestCategory("BVT")]    
        [Priority(0)]
        [TestMethod]
        public void CountryCodeEquals156OrITDepartmentPolicy()
        {
            string testName = string.Empty;

            #region Initialize
            testName = System.Reflection.MethodInfo.GetCurrentMethod().Name;
            this.TestSite.Log.Add(LogEntryKind.Debug, "Get test case name: {0}", testName);
            System.Environment.SetEnvironmentVariable("MS_AZOD_TESTCASENAME", testName);
            #endregion

            #region config environment
            this.TestSite.Log.Add(LogEntryKind.Debug, "Turn on CBAC on client computer.");
            this.sutController.CbacSwitch("1", this.TestConfig.ClientComputerName, this.TestConfig.KdcDomainName + "\\" + this.TestConfig.KdcAdminUser, this.TestConfig.KdcAdminPwd);

            this.TestSite.Log.Add(LogEntryKind.Debug, "Clean up cached tickets on client computer.");
            this.sutController.CleanupCachedTickets(this.TestConfig.ClientComputerName, this.TestConfig.KdcDomainName + "\\" + this.TestConfig.KdcAdminUser, this.TestConfig.KdcAdminPwd);

            #endregion

            base.Logging();
            string tempString = testName.Substring(0, testName.Length - 6);
            string uncPath = string.Format(@"\\{0}.{1}\{2}Allowed", this.TestConfig.ApplicationServerName, this.TestConfig.KdcDomainName, tempString);
            string fileName = null;

            string domainName = this.TestConfig.KdcDomainName;
            bool returnValue = true;

            string password = "Password01@";
            string userName = "PayrollMember01";

            //Check whether the user has the write permission to the share folder
            //Expect to succeed
            fileName = string.Format(@"{0}{1}.txt",testName, userName );
            returnValue = this.sutController.WriteToShareFolder(uncPath, fileName, userName, password, domainName, string.Format("{0}-{1}-{2}.log", testName, userName, "WriteAccess"));
            this.TestSite.Assert.IsTrue(returnValue, string.Format("User {0} should have the write access control to share folder {1}. If failed, please check the user name, password, user claim or the folder permission.", userName, uncPath));

            password = "Password01@";
            userName = "ITMember01";

            //Check whether the user has the write permission to the share folder
            //Expect to succeed
            fileName = string.Format(@"{0}{1}.txt", testName, userName);
            returnValue = this.sutController.WriteToShareFolder(uncPath, fileName, userName, password, domainName, string.Format("{0}-{1}-{2}.log", testName, userName, "WriteAccess"));
            this.TestSite.Assert.IsTrue(returnValue, string.Format("User {0} should have the write access control to share folder {1}. If failed, please check the user name, password, user claim or the folder permission.", userName, uncPath));
           
            password = "Password01@";
            userName = "ITAdmin01";

            //Check whether the user has the write permission to the share folder
            //Expect to succeed
            fileName = string.Format(@"{0}{1}.txt", testName, userName);
            returnValue = this.sutController.WriteToShareFolder(uncPath, fileName, userName, password, domainName, string.Format("{0}-{1}-{2}.log", testName, userName, "WriteAccess"));
            this.TestSite.Assert.IsTrue(returnValue, string.Format("User {0} should have the write access control to share folder {1}. If failed, please check the user name, password, user claim or the folder permission.", userName, uncPath));
            
            password = "Password01@";
            userName = "PayrollMember02";

            //Check whether the user has the write permission to the share folder.
            //Expect to fail
            fileName = string.Format(@"{0}{1}.txt", testName, userName);
            returnValue = this.sutController.WriteToShareFolder(uncPath, fileName, userName, password, domainName, string.Format("{0}-{1}-{2}.log", testName, userName, "WriteAccess"));
            this.TestSite.Assert.IsFalse(returnValue, string.Format("User {0} should not have the write access control to share folder {1}. If it has, please check the user name, password, user claims or the folder permission.", userName, uncPath));

            //Check whether the user has the read permission to the share folder
            //Expect to fail
            returnValue = this.sutController.ReadShareFolder(uncPath, userName, password, domainName, string.Format("{0}-{1}-{2}.log", testName, userName, "ReadAccess"));
            this.TestSite.Assert.IsFalse(returnValue, string.Format("User {0} should not have the read access control to share folder {1}. If it has, please check the user name, password, user claims or the folder permission.", userName, uncPath));
                    
        }
        
        [TestCategory("BVT")]
        [Priority(0)]
        [TestMethod]
        public void NoUserClaimBlockWriteControl()
        {
            string testName = string.Empty;

            #region Initialize
            testName = System.Reflection.MethodInfo.GetCurrentMethod().Name;
            this.TestSite.Log.Add(LogEntryKind.Debug, "Get test case name: {0}", testName);
            System.Environment.SetEnvironmentVariable("MS_AZOD_TESTCASENAME", testName);
            #endregion

            #region config environment
            this.TestSite.Log.Add(LogEntryKind.Debug, "Turn on CBAC on client computer.");
            this.sutController.CbacSwitch("1", this.TestConfig.ClientComputerName, this.TestConfig.KdcDomainName + "\\" + this.TestConfig.KdcAdminUser, this.TestConfig.KdcAdminPwd);

            this.TestSite.Log.Add(LogEntryKind.Debug, "Clean up cached tickets on client computer.");
            this.sutController.CleanupCachedTickets(this.TestConfig.ClientComputerName, this.TestConfig.KdcDomainName + "\\" + this.TestConfig.KdcAdminUser, this.TestConfig.KdcAdminPwd);

            #endregion

            base.Logging();
            base.Logging();
            string tempString = testName.Substring(0, testName.Length - 6);
            string uncPath = string.Format(@"\\{0}.{1}\{2}Allowed", this.TestConfig.ApplicationServerName, this.TestConfig.KdcDomainName, tempString);
            string fileName = testName + ".txt";

            string domainName = this.TestConfig.KdcDomainName;
            bool returnValue = true;

            string password = "Password01@";
            string userName = "noclaimuser";

            //Check whether the user has the write permission to the share folder.
            //Expect to fail
            returnValue = this.sutController.WriteToShareFolder(uncPath, fileName, userName, password, domainName, string.Format("{0}-{1}-{2}.log", testName, userName, "WriteAccess"));
            this.TestSite.Assert.IsFalse(returnValue, string.Format("User {0} should not have the write access control to share folder {1}. If it has, please check the user name, password, user claims or the folder permission.", userName, uncPath));

            //Check whether the user has the read permission to the share folder
            //Expect to fail
            returnValue = this.sutController.ReadShareFolder(uncPath, userName, password, domainName, string.Format("{0}-{1}-{2}.log", testName, userName, "ReadAccess"));
            this.TestSite.Assert.IsFalse(returnValue, string.Format("User {0} should not have the read access control to share folder {1}. If it has, please check the user name, password, user claims or the folder permission.", userName, uncPath));
            
        } 
    }
}
