// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Protocols.TestTools;
using Microsoft.Protocols.TestTools.StackSdk.Security.KerberosLib;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Protocols.TestSuites.SFUProtocol.TestSuites
{
    [TestClass]
    public class S4U2SelfTestSuite : SfuBasicTestSuite
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
        [TestCategory("S4U2Self")]
        [TestMethod]
        public void BVT_SingleRealm_S4U2Self_UsingUserName()
        {
            Initialize(config.Service1aInfo);

            BaseTestSite.Log.Add(LogEntryKind.TestStep, "Use S4U2Self to get ticket to service1a for delegated user.");
            KerberosTgsResponse tgsRsp;
            S4U2Self(config.DelegatedUserInfo, out tgsRsp);
        }

        [TestCategory("BVT")]
        [TestCategory("SingleRealm")]
        [TestCategory("S4U2Self")]
        [TestCategory("ResourceBased")]
        [TestMethod]
        public void BVT_SingleRealm_S4U2Self_ResourceBased_UsingUserName()
        {
            Initialize(config.Service1bInfo);

            BaseTestSite.Log.Add(LogEntryKind.TestStep, "Use S4U2Self to get ticket to service1b for delegated user.");
            KerberosTgsResponse tgsRsp;
            S4U2Self(config.DelegatedUserInfo, out tgsRsp);
        }


        [TestCategory("BVT")]
        [TestCategory("SingleRealm")]
        [TestCategory("S4U2Self")]
        [TestCategory("Restricted")]
        [TestMethod]
        public void SingleRealm_S4U2Self_Restricted_UsingUserName()
        {
            Initialize(config.Service1aInfo);

            BaseTestSite.Log.Add(LogEntryKind.TestStep, "Use S4U2Self to get ticket to service1a for restricted user.");
            KerberosTgsResponse tgsRsp1;
            S4U2Self(config.RestrictedUserInfo, out tgsRsp1);
        }
    }
}
