// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Protocols.TestSuites.FileSharing.Common.Adapter;
using Microsoft.Protocols.TestSuites.FileSharing.Common.TestSuite;
using Microsoft.Protocols.TestTools;
using Microsoft.Protocols.TestTools.StackSdk.Swn;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Net;

namespace Microsoft.Protocols.TestSuites.FileSharing.ServerFailover.TestSuite
{
    /// <summary>
    /// This test class is for SWN GetInterfaceList.
    /// </summary>
    [TestClass]
    public class SWNGetInterfaceList : ServerFailoverTestBase
    {
        #region Variables

        /// <summary>
        /// SWN client to get interface list.
        /// </summary>
        private SwnClient swnClientForInterface;

        /// <summary>
        /// SWN client to witness.
        /// </summary>
        private SwnClient swnClientForWitness;

        private string disabledNode;

        #endregion

        #region Test Suite Initialization

        // Use ClassInitialize to run code before running the first test in the class
        [ClassInitialize()]
        public static void ClassInitialize(TestContext testContext)
        {
            TestClassBase.Initialize(testContext);
            SWNTestUtility.BaseTestSite = BaseTestSite;
        }

        // Use ClassCleanup to run code after all tests in a class have run
        [ClassCleanup()]
        public static void ClassCleanup()
        {
            TestClassBase.Cleanup();
        }

        #endregion

        // Use TestInitialize to run code before running every test in the class
        protected override void TestInitialize()
        {
            base.TestInitialize();

            swnClientForInterface = new SwnClient();
            swnClientForWitness = new SwnClient();
            disabledNode = null;
        }

        // Use TestCleanup to run code after every test in a class have run
        protected override void TestCleanup()
        {
            try
            {
                swnClientForInterface.SwnUnbind(TestConfig.Timeout);
            }
            catch (Exception ex)
            {
                BaseTestSite.Log.Add(LogEntryKind.Warning, "TestCleanup: Unexpected Exception: {0}", ex);
            }

            try
            {
                swnClientForWitness.SwnUnbind(TestConfig.Timeout);
            }
            catch (Exception ex)
            {
                BaseTestSite.Log.Add(LogEntryKind.Warning, "TestCleanup: Unexpected Exception: {0}", ex);
            }

            if (disabledNode != null)
            {
                RestoreClusterNodes(disabledNode);
                disabledNode = null;
            }

            base.TestCleanup();
        }

        [TestMethod]
        [TestCategory(TestCategories.Swn)]
        [TestCategory(TestCategories.NonSmb)]
        [TestCategory(TestCategories.Bvt)]
        [Description("Ensure that the file server sets the valid value for Version field of the structure WITNESS_INTERFACE_INFO.")]
        public void BVT_SWN_CheckProtocolVersion()
        {
            int ret;
            WITNESS_INTERFACE_LIST interfaceList = new WITNESS_INTERFACE_LIST();
            IPAddress currentAccessIpAddr;

            currentAccessIpAddr = SWNTestUtility.GetCurrentAccessIP(TestConfig.ClusteredScaleOutFileServerName);

            BaseTestSite.Log.Add(LogEntryKind.TestStep, "Get interface list to register.");
            DoUntilSucceed(() => SWNTestUtility.BindServer(swnClientForInterface, currentAccessIpAddr,
                TestConfig.DomainName, TestConfig.UserName, TestConfig.UserPassword, TestConfig.DefaultSecurityPackage,
                TestConfig.DefaultRpceAuthenticationLevel, TestConfig.Timeout, TestConfig.ClusteredScaleOutFileServerName), TestConfig.FailoverTimeout,
                "Retry BindServer until succeed within timeout span");

            DoUntilSucceed(() =>
            {
                ret = swnClientForInterface.WitnessrGetInterfaceList(out interfaceList);
                BaseTestSite.Assert.AreEqual<SwnErrorCode>(SwnErrorCode.ERROR_SUCCESS, (SwnErrorCode)ret, "WitnessrGetInterfaceList returns with result code = 0x{0:x8}", ret);
                return ret == (int)SwnErrorCode.ERROR_SUCCESS;
            }, TestConfig.FailoverTimeout, "Retry to call WitnessrGetInterfaceList until succeed within timeout span");

            BaseTestSite.Log.Add(LogEntryKind.TestStep, "Check if the Version field of the structure WITNESS_INTERFACE_INFO is correct.");
            SWNTestUtility.VerifyInterfaceList(interfaceList, TestConfig.Platform, true);
            swnClientForInterface.SwnUnbind(TestConfig.Timeout);
        }

        [TestMethod]
        [TestCategory(TestCategories.Swn)]
        [TestCategory(TestCategories.NonSmb)]
        [TestCategory(TestCategories.Bvt)]
        [Description("Ensure that the file server does not response the interface list correctly until there is an available node.")]
        public void BVT_SWNGetInterfaceList_ClusterSingleNode()
        {
            SWNGetInterfaceList_SingleNode(TestConfig.ClusteredFileServerName, FileServerType.GeneralFileServer);
        }

        [TestMethod]
        [TestCategory(TestCategories.Swn)]
        [TestCategory(TestCategories.NonSmb)]
        [TestCategory(TestCategories.Bvt)]
        [Description("Ensure that single node does not response the interface list correctly until there is an available node.")]
        public void BVT_SWNGetInterfaceList_ScaleOutSingleNode()
        {
            SWNGetInterfaceList_SingleNode(TestConfig.ClusteredScaleOutFileServerName, FileServerType.ScaleOutFileServer);
        }

        private void SWNGetInterfaceList_SingleNode(string server, FileServerType fsType)
        {
            int ret;
            WITNESS_INTERFACE_LIST interfaceList = new WITNESS_INTERFACE_LIST();
            IPAddress currentAccessIpAddr;
            WITNESS_INTERFACE_INFO registerInterface;

            #region Test Sequence
            currentAccessIpAddr = SWNTestUtility.GetCurrentAccessIP(server);            

            #region Get interface list to register.
            BaseTestSite.Log.Add(LogEntryKind.TestStep, "Get interface list to register.");
            DoUntilSucceed(() => SWNTestUtility.BindServer(swnClientForInterface, currentAccessIpAddr,
                TestConfig.DomainName, TestConfig.UserName, TestConfig.UserPassword, TestConfig.DefaultSecurityPackage,
                TestConfig.DefaultRpceAuthenticationLevel, TestConfig.Timeout, server), TestConfig.FailoverTimeout,
                "Retry BindServer until succeed within timeout span");

            DoUntilSucceed(() =>
            {
                ret = swnClientForInterface.WitnessrGetInterfaceList(out interfaceList);
                BaseTestSite.Assert.AreEqual<SwnErrorCode>(SwnErrorCode.ERROR_SUCCESS, (SwnErrorCode)ret, "WitnessrGetInterfaceList returns with result code = 0x{0:x8}", ret);
                return SWNTestUtility.VerifyInterfaceList(interfaceList, TestConfig.Platform);
            }, TestConfig.FailoverTimeout, "Retry to call WitnessrGetInterfaceList until succeed within timeout span");

            swnClientForInterface.SwnUnbind(TestConfig.Timeout);

            SWNTestUtility.GetRegisterInterface(interfaceList, out registerInterface);
            #endregion

            #region Disable register interface
            BaseTestSite.Log.Add(LogEntryKind.TestStep, "Disable interface {0}.", registerInterface.InterfaceGroupName);
            disabledNode = SWNTestUtility.GetPrincipleName(TestConfig.DomainName, registerInterface.InterfaceGroupName);

            BaseTestSite.Log.Add(LogEntryKind.TestStep, "Disable node {0} to trigger cluster failover.", disabledNode);
            FailoverClusterNode(disabledNode);


            // Wait to sync information between witness services
            System.Threading.Thread.Sleep(TestConfig.swnWitnessSyncTimeout);

            if (TestConfig.IsWindowsPlatform)
            {
                //Wait the file server back online
                string newOwnerNode = null;
                DoUntilSucceed(() =>
                {
                    newOwnerNode = sutController.GetClusterResourceOwner(server);
                    return (!string.IsNullOrEmpty(newOwnerNode));
                }, TestConfig.FailoverTimeout, "Retry to get cluster owner node until succeed within timeout span");

                BaseTestSite.Assert.IsNotNull(newOwnerNode, "The new owner of cluster is {0}", newOwnerNode);
            }
            #endregion

            #region Test that WitnessrGetInterfaceList is blocked
            BaseTestSite.Log.Add(LogEntryKind.TestStep, "Test that WitnessrGetInterfaceList is blocked.");
            if (fsType == FileServerType.ScaleOutFileServer)
            {
                WITNESS_INTERFACE_INFO accessInterface = new WITNESS_INTERFACE_INFO();
                SWNTestUtility.GetAccessInterface(interfaceList, out accessInterface);
                currentAccessIpAddr =
                    (accessInterface.Flags & (uint)SwnNodeFlagsValue.IPv4) != 0 ? new IPAddress(accessInterface.IPV4) : SWNTestUtility.ConvertIPV6(accessInterface.IPV6);
                server = accessInterface.InterfaceGroupName;
            }

            swnClientForInterface = new SwnClient();
            DoUntilSucceed(() => SWNTestUtility.BindServer(swnClientForInterface, currentAccessIpAddr,
                TestConfig.DomainName, TestConfig.UserName, TestConfig.UserPassword, TestConfig.DefaultSecurityPackage,
                TestConfig.DefaultRpceAuthenticationLevel, TestConfig.Timeout, server), TestConfig.FailoverTimeout,
                "Retry BindServer until succeed within timeout span");

            try
            {
                ret = swnClientForInterface.WitnessrGetInterfaceList(out interfaceList);
                SWNTestUtility.PrintInterfaceList(interfaceList);
                BaseTestSite.Assert.Fail("Expect WitnessrGetInterfaceList throws TimeoutException because there is no available node.");
            }
            catch (TimeoutException)
            {
                BaseTestSite.Assert.Pass("Expect WitnessrGetInterfaceList throws TimeoutException because there is no available node.");
            }
            swnClientForInterface.SwnUnbind(TestConfig.Timeout);
            #endregion

            #region Enable register interface
            BaseTestSite.Log.Add(LogEntryKind.TestStep, "Restore disabled node {0}.", disabledNode);
            RestoreClusterNodes(disabledNode);
            disabledNode = null;
            #endregion

            #region Call WitnessrGetInterfaceList successfully
            BaseTestSite.Log.Add(LogEntryKind.TestStep, "Call WitnessrGetInterfaceList successfully.");

            DoUntilSucceed(() => SWNTestUtility.BindServer(swnClientForInterface, currentAccessIpAddr,
                TestConfig.DomainName, TestConfig.UserName, TestConfig.UserPassword, TestConfig.DefaultSecurityPackage,
                TestConfig.DefaultRpceAuthenticationLevel, TestConfig.Timeout, server), TestConfig.FailoverTimeout,
                "Retry BindServer until succeed within timeout span");

            DoUntilSucceed(() =>
            {
                ret = swnClientForInterface.WitnessrGetInterfaceList(out interfaceList);
                BaseTestSite.Assert.AreEqual<int>(0, ret, "WitnessrGetInterfaceList returns with result code = 0x{0:x8}", ret);
                return SWNTestUtility.VerifyInterfaceList(interfaceList, TestConfig.Platform);
            }, TestConfig.FailoverTimeout, "Retry to call WitnessrGetInterfaceList until succeed within timeout span");

            swnClientForInterface.SwnUnbind(TestConfig.Timeout);
            #endregion

            #endregion
        }
    }
}
