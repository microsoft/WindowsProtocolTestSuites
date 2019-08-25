// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using Microsoft.Protocols.TestTools;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Protocols.TestSuites.ActiveDirectory.Common;

namespace Microsoft.Protocols.TestSuites.ActiveDirectory.Apds
{
    /// <summary>
    /// Test suite to test the implementation for MS-APDS protocol.
    /// </summary>
    [TestClass]
    public partial class TestSuite : TestClassBase
    {
        #region Member Variable Declarations

        /// <summary>
        /// NTML & Digest Logon validation adapter.
        /// </summary>
        public ApdsServerAdapter apdsServerAdapter;
        /// <summary>
        /// The current OS of test server.
        /// </summary>
        private OSVersion currentOS;
        /// <summary>
        /// Adapter to control the test environment.
        /// </summary>
        public static IApdsSutControlAdapter serverControlAdapter;
        /// <summary>
        ///check the server OS is windows.
        /// </summary>
        private bool isServerWindows;
        /// <summary>
        /// The full name of resource DC.
        /// </summary>
        private string resourceDCName;
        /// <summary>
        /// The full name of resource DC IPAddress.
        /// </summary>
        private string resourceDCIPAddress;
        /// <summary>
        /// The full name of account DC.
        /// </summary>
        private string accountDCName;
        /// <summary>
        /// The full name of account DC IPAddress.
        /// </summary>
        private string accountDCIPAddress;
        /// <summary>
        /// Default value of resource DC blocker registry key.
        /// </summary>
        private int defaultResourceDCKey;
        /// <summary>
        /// Default value of account DC blocker registry key.
        /// </summary>
        private int defaultAccountDCKey;
        /// <summary>
        /// Default value of resource DC blocker exception registry key.
        /// </summary>
        private string defaultResourceDCException;
        /// <summary>
        /// The value for registry key to block DC.
        /// </summary>
        private int blockDCkey;
        /// <summary>
        /// The value for registry key to set exception server for block DC.
        /// </summary>
        private string blockCDException;
        /// <summary>
        /// The user principle used to test authentication policy.
        /// </summary>
        private string testUser;
        /// <summary>
        /// The managed service account used to test authentication policy.
        /// </summary>
        private string testManagedServiceAccount;

        #endregion

        /// <summary>
        /// Use ClassInitialize to run code before running the first test in the class
        /// </summary>
        /// <param name="testContext">Test context base on the base class.</param>
        [ClassInitialize()]
        public static void ClassInitialize(TestContext testContext)
        {
            // initialize test context to your ptf config file.
            TestClassBase.Initialize(testContext);
            serverControlAdapter = TestClassBase.BaseTestSite.GetAdapter<Microsoft.Protocols.TestSuites.ActiveDirectory.Apds.IApdsSutControlAdapter>();
            serverControlAdapter.ConfigNTLMRegistryKey();
        }

        /// <summary>
        /// Use ClassCleanup to run code after all tests in a class have run
        /// </summary>
        [ClassCleanup()]
        public static void ClassCleanup()
        {
            serverControlAdapter.RestoreNTLMRegistryKey();
            TestClassBase.Cleanup();
        }

        /// <summary>
        /// Use TestInitialize to run code before running each test 
        /// </summary>
        protected override void TestInitialize()
        {
            base.TestInitialize();
            Site.DefaultProtocolDocShortName = "MS-APDS";
            apdsServerAdapter = new ApdsServerAdapter();
            apdsServerAdapter.Initialize(Site);

            ServerVersion serverVersion = (ServerVersion)apdsServerAdapter.PDCOSVersion;
            if (serverVersion == ServerVersion.Win2008) currentOS = OSVersion.WINSVR2008;
            else if (serverVersion == ServerVersion.Win2008R2) currentOS = OSVersion.WINSVR2008R2;
            else if (serverVersion == ServerVersion.Win2012) currentOS = OSVersion.WINSVR2012;
            else if (serverVersion == ServerVersion.Win2012R2) currentOS = OSVersion.WINSVR2012R2;
            else if (serverVersion == ServerVersion.NonWin) currentOS = OSVersion.NONWINDOWS;
            else currentOS = OSVersion.OTHER;
            isServerWindows = currentOS != OSVersion.NONWINDOWS;

            resourceDCName = apdsServerAdapter.PDCNetbiosName + "." + apdsServerAdapter.PrimaryDomainDnsName;
            resourceDCIPAddress = apdsServerAdapter.PDCIPAddress;
            accountDCName = apdsServerAdapter.TDCNetbiosName + "." + apdsServerAdapter.TrustDomainDnsName;
            accountDCIPAddress = apdsServerAdapter.TDCIPAddress;
            // The value for registry key to block DC.
            blockDCkey = 7;
            // The value for registry key to set exception server for block DC.
            blockCDException = apdsServerAdapter.ENDPOINTNetbiosName;

            testUser = apdsServerAdapter.TrustDomainDnsName + "\\" + apdsServerAdapter.DomainAdministratorName;
            testManagedServiceAccount = apdsServerAdapter.ManagedServiceAccountName;


            // Get default registry key for DC blocker.
            if (currentOS >= OSVersion.WINSVR2008R2)
            {
                defaultResourceDCKey = serverControlAdapter.GetDCBlockValue(resourceDCIPAddress);
                defaultAccountDCKey = serverControlAdapter.GetDCBlockValue(accountDCIPAddress);
                defaultResourceDCException = serverControlAdapter.GetDCException(resourceDCIPAddress);
            }
        }

        /// <summary>
        /// Use TestCleanup to run code after each test has run
        /// </summary>
        protected override void TestCleanup()
        {
            if (apdsServerAdapter != null)
            {
                apdsServerAdapter.Dispose();
                apdsServerAdapter = null;
            }
        }
    }
}

