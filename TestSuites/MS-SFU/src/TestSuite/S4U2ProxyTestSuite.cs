// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Protocols.TestTools;
using Microsoft.Protocols.TestTools.StackSdk.Security.KerberosLib;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Protocols.TestSuites.SFUProtocol.TestSuites
{
    [TestClass]
    public class S4U2ProxyTestSuite : SfuBasicTestSuite
    {
        [ClassInitialize]
        public static void ClassInitialize(TestContext testContext)
        {
            Initialize(testContext);
        }

        [ClassCleanup]
        public static void ClassCleanup()
        {
            Cleanup();
        }

        [TestCategory("BVT")]
        [TestCategory("SingleRealm")]
        [TestCategory("S4U2Proxy")]
        [TestMethod]
        public void BVT_SingleRealm_S4U2Proxy_UsingUserName()
        {
            Initialize(config.Service1aInfo);

            BaseTestSite.Log.Add(LogEntryKind.TestStep, "Use S4U2Self to get ticket to service1a for delegated user.");
            KerberosTgsResponse tgsRsp1;
            S4U2Self(config.DelegatedUserInfo, out tgsRsp1);

            BaseTestSite.Log.Add(LogEntryKind.TestStep, "Use S4U2Proxy to get ticket to service2 for delegated user.");
            S4U2Proxy(false, config.Service2Info, tgsRsp1);
        }

        [TestCategory("BVT")]
        [TestCategory("SingleRealm")]
        [TestCategory("S4U2Proxy")]
        [TestCategory("ResourceBased")]
        [TestMethod]
        public void BVT_SingleRealm_S4U2Proxy_ResourceBased_UsingUserName()
        {
            Initialize(config.Service1bInfo);

            BaseTestSite.Log.Add(LogEntryKind.TestStep, "Use S4U2Self to get ticket to service1b for delegated user.");
            KerberosTgsResponse tgsRsp1;
            S4U2Self(config.DelegatedUserInfo, out tgsRsp1);

            BaseTestSite.Log.Add(LogEntryKind.TestStep, "Use S4U2Proxy to get ticket to service2 for delegated user, resource-based constrained delegation flag is set.");
            S4U2Proxy(true, config.Service2Info, tgsRsp1);
        }

        [TestCategory("Negative")]
        [TestCategory("SingleRealm")]
        [TestCategory("S4U2Proxy")]
        [TestCategory("ResourceBased")]
        [TestMethod]
        public void Negative_SingleRealm_S4U2Proxy_ResourceBased_UsingUserName_FlagNotSet()
        {
            Initialize(config.Service1bInfo);

            BaseTestSite.Log.Add(LogEntryKind.TestStep, "Use S4U2Self to get ticket to service1b for delegated user.");
            KerberosTgsResponse tgsRsp1;
            S4U2Self(config.DelegatedUserInfo, out tgsRsp1);

            BaseTestSite.Log.Add(LogEntryKind.TestStep, "Use S4U2Proxy to get ticket to service2 for delegated user, resource-based constrained delegation flag is not set.");
            BaseTestSite.Log.Add(LogEntryKind.TestStep, "Expect SUT return KRB-ERR-BADOPTION with STATUS_NOT_SUPPORTED.");
            S4U2Proxy(false, config.Service2Info, tgsRsp1);
        }

        [TestCategory("Negative")]
        [TestCategory("SingleRealm")]
        [TestCategory("S4U2Proxy")]
        [TestCategory("Restricted")]
        [TestMethod]
        public void Negative_SingleRealm_S4U2Proxy_Restricted_UsingUserName()
        {
            Initialize(config.Service1aInfo);

            BaseTestSite.Log.Add(LogEntryKind.TestStep, "Use S4U2Self to get ticket to service1a for restricted user.");
            KerberosTgsResponse tgsRsp1;
            S4U2Self(config.RestrictedUserInfo, out tgsRsp1);

            BaseTestSite.Log.Add(LogEntryKind.TestStep, "Use S4U2Proxy to get ticket to service2 for restricted user.");
            BaseTestSite.Log.Add(LogEntryKind.TestStep, "Expect SUT return KRB-ERR-BADOPTION with STATUS_NO_MATCH.");
            S4U2Proxy(false, config.Service2Info, tgsRsp1);
        }

        [TestCategory("Negative")]
        [TestCategory("SingleRealm")]
        [TestCategory("S4U2Proxy")]
        [TestCategory("ResourceBased")]
        [TestCategory("Restricted")]
        [TestMethod]
        public void Negative_SingleRealm_S4U2Proxy_ResourceBased_Restricted_UsingUserName()
        {
            Initialize(config.Service1bInfo);

            BaseTestSite.Log.Add(LogEntryKind.TestStep, "Use S4U2Self to get ticket to service1b for restricted user.");
            KerberosTgsResponse tgsRsp1;
            S4U2Self(config.RestrictedUserInfo, out tgsRsp1);

            BaseTestSite.Log.Add(LogEntryKind.TestStep, "Use S4U2Proxy to get ticket to service2 for restricted user.");
            BaseTestSite.Log.Add(LogEntryKind.TestStep, "Expect SUT return KRB-ERR-BADOPTION with STATUS_ACCOUNT_RESTRICTION.");
            S4U2Proxy(true, config.Service2Info, tgsRsp1);
        }
    }
}
