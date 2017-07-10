// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Protocol.TestSuites.Kerberos.Adapter;
using Microsoft.Protocols.TestTools;
using Microsoft.Protocols.TestTools.StackSdk.Security.KerberosLib;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Protocol.TestSuites.Kerberos.TestSuite
{
    /// <summary>
    /// KKDCP tests re-use all other Kerberos cases.
    /// Cases defined in this class are mainly for negative test.
    /// </summary>
    [TestClass]
    public class KKDCPTestSuite : TraditionTestBase
    {
        [ClassInitialize()]
        public static void ClassInitialize(TestContext testContext)
        {
            TestClassBase.Initialize(testContext);
        }

        [ClassCleanup()]
        public static void ClassCleanup()
        {
            TestClassBase.Cleanup();
        }

        [TestMethod]
        [Priority(1)]
        [TestCategory(TestCategories.KPS)]
        [TestCategory(TestCategories.SingleRealm)]
        [TestCategory(TestCategories.DFL2K12)]
        [TestCategory(TestCategories.KKDCP)]
        [Description("This test case is designed to test KPS when Target Domain is not present.")]
        public void KPSNullTargetDomain()
        {
            base.Logging();

            if (!this.testConfig.UseProxy)
            {
                BaseTestSite.Assert.Inconclusive("This case is only applicable when Kerberos Proxy Service is in use.");
            }

            //Create kerberos test client and connect
            client = new KerberosTestClient(this.testConfig.LocalRealm.RealmName,
                this.testConfig.LocalRealm.User[1].Username,
                this.testConfig.LocalRealm.User[1].Password,
                KerberosAccountType.User,
                testConfig.LocalRealm.KDC[0].IPAddress,
                testConfig.LocalRealm.KDC[0].Port,
                testConfig.TransportType,
                testConfig.SupportedOid);

            KKDCPClient proxyClient = new KKDCPClient(proxyClientConfig);
            //Set the TargetDomain to NULL;
            proxyClient.TargetDomain = null;
            client.UseProxy = true;
            client.ProxyClient = proxyClient;

            KdcOptions options = KdcOptions.FORWARDABLE | KdcOptions.CANONICALIZE | KdcOptions.RENEWABLE;
            BaseTestSite.Log.Add(LogEntryKind.TestStep, "Client sends Proxy Message with Target Domain not present.");
            client.SendAsRequest(options, null);

            BaseTestSite.Assert.AreEqual(KKDCPError.STATUS_NO_LOGON_SERVERS, client.ProxyClient.Error, "Server should drop the connection.");
        }

        [TestMethod]
        [Priority(1)]
        [TestCategory(TestCategories.KPS)]
        [TestCategory(TestCategories.SingleRealm)]
        [TestCategory(TestCategories.DFL2K12)]
        [TestCategory(TestCategories.KKDCP)]
        [Description("This test case is designed to test KPS when Target Domain provided is not valid.")]
        public void KPSInvalidTargetDomain()
        {
            base.Logging();

            if (!this.testConfig.UseProxy)
            {
                BaseTestSite.Assert.Inconclusive("This case is only applicable when Kerberos Proxy Service is in use.");
            }
            //Create kerberos test client and connect
            client = new KerberosTestClient(this.testConfig.LocalRealm.RealmName,
                this.testConfig.LocalRealm.User[1].Username,
                this.testConfig.LocalRealm.User[1].Password,
                KerberosAccountType.User,
                testConfig.LocalRealm.KDC[0].IPAddress,
                testConfig.LocalRealm.KDC[0].Port,
                testConfig.TransportType,
                testConfig.SupportedOid);

            KKDCPClient proxyClient = new KKDCPClient(proxyClientConfig);
            //Set the TargetDomain to an invalid value;
            proxyClient.TargetDomain = "InvalidDomain";
            client.UseProxy = true;
            client.ProxyClient = proxyClient;

            KdcOptions options = KdcOptions.FORWARDABLE | KdcOptions.CANONICALIZE | KdcOptions.RENEWABLE;
            BaseTestSite.Log.Add(LogEntryKind.TestStep, "Client sends Proxy Message with invalid Target Domain.");
            client.SendAsRequest(options, null);

            BaseTestSite.Assert.AreEqual(KKDCPError.STATUS_NO_LOGON_SERVERS, client.ProxyClient.Error, "Server should drop the connection.");
        }
    }
}
