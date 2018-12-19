// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Protocols.TestSuites.FileSharing.Common.Adapter;
using Microsoft.Protocols.TestSuites.FileSharing.Common.TestSuite;
using Microsoft.Protocols.TestTools;
using Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Smb2;
using Microsoft.Protocols.TestTools.StackSdk.Swn;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Net;

namespace Microsoft.Protocols.TestSuites.FileSharing.ServerFailover.TestSuite
{
    /// <summary>
    /// This test class is for SWN Registration.
    /// </summary>
    [TestClass]
    public class SWNRegistration : ServerFailoverTestBase
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

        /// <summary>
        /// The registered handle of witness.
        /// </summary>
        private System.IntPtr pContext;

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

            swnClientForInterface = null;
            swnClientForWitness = null;
            pContext = IntPtr.Zero;
        }

        // Use TestCleanup to run code after every test in a class have run
        protected override void TestCleanup()
        {
            if (pContext != IntPtr.Zero)
            {
                if (swnClientForWitness != null)
                {
                    swnClientForWitness.WitnessrUnRegister(pContext);
                }
                pContext = IntPtr.Zero;
            }

            try
            {
                if (swnClientForInterface != null)
                {
                    swnClientForInterface.SwnUnbind(TestConfig.FailoverTimeout);
                    swnClientForInterface = null;
                }
                if (swnClientForWitness != null)
                {
                    swnClientForWitness.SwnUnbind(TestConfig.FailoverTimeout);
                    swnClientForInterface = null;
                }
            }
            catch (Exception ex)
            {
                BaseTestSite.Log.Add(LogEntryKind.Warning, "TestCleanup: Unexpected Exception: {0}", ex);
            }

            base.TestCleanup();
        }

        [TestMethod]
        [TestCategory(TestCategories.Swn)]
        [TestCategory(TestCategories.NonSmb)]
        [TestCategory(TestCategories.UnexpectedFields)]
        [Description("Register with WitnessrRegister and invalid netname.")]
        public void SWNRegistration_InvalidNetName()
        {
            SWNRegister(SwnRegisterType.InvalidNetName, SwnVersion.SWN_VERSION_1);
        }

        [TestMethod]
        [TestCategory(TestCategories.Swn)]
        [TestCategory(TestCategories.NonSmb)]
        [TestCategory(TestCategories.UnexpectedFields)]
        [Description("Register with WitnessrRegister and invalid version.")]
        public void SWNRegistration_InvalidVersion()
        {
            SWNRegister(SwnRegisterType.InvalidVersion, SwnVersion.SWN_VERSION_1);
        }

        [TestMethod]
        [TestCategory(TestCategories.Swn)]
        [TestCategory(TestCategories.NonSmb)]
        [TestCategory(TestCategories.UnexpectedFields)]
        [Description("Register with WitnessrRegister and invalid IpAddress.")]
        public void SWNRegistration_InvalidIpAddress()
        {
            SWNRegister(SwnRegisterType.InvalidIpAddress, SwnVersion.SWN_VERSION_1);
        }

        [TestMethod]
        [TestCategory(TestCategories.Swn)]
        [TestCategory(TestCategories.NonSmb)]
        [TestCategory(TestCategories.UnexpectedFields)]
        [Description("Register with WitnessrRegister and Unregister the client twice.")]
        public void SWNRegistration_InvalidUnRegister()
        {
            SWNRegister(SwnRegisterType.InvalidUnRegister, SwnVersion.SWN_VERSION_1);
        }

        [TestMethod]
        [TestCategory(TestCategories.Swn)]
        [TestCategory(TestCategories.NonSmb)]
        [TestCategory(TestCategories.UnexpectedContext)]
        [Description("Assure that server responses correctly with invalid context.")]
        public void SWNAsyncNotification_InvalidRequest()
        {
            SWNRegister(SwnRegisterType.InvalidRequest, SwnVersion.SWN_VERSION_1);
        }

        [TestMethod]
        [TestCategory(TestCategories.Swn)]
        [TestCategory(TestCategories.NonSmb)]
        [TestCategory(TestCategories.UnexpectedFields)]
        [Description("Register with WitnessrRegisterEx and invalid NetName.")]
        public void SWNRegistrationEx_InvalidNetName()
        {
            SWNRegister(SwnRegisterType.InvalidNetName, SwnVersion.SWN_VERSION_2);
        }

        [TestMethod]
        [TestCategory(TestCategories.Swn)]
        [TestCategory(TestCategories.NonSmb)]
        [TestCategory(TestCategories.UnexpectedFields)]
        [Description("Register with WitnessrRegisterEx and invalid version.")]
        public void SWNRegistrationEx_InvalidVersion()
        {
            SWNRegister(SwnRegisterType.InvalidVersion, SwnVersion.SWN_VERSION_2);
        }

        [TestMethod]
        [TestCategory(TestCategories.Swn)]
        [TestCategory(TestCategories.NonSmb)]
        [TestCategory(TestCategories.UnexpectedFields)]
        [Description("Register with WitnessrRegisterEx and invalid IpAddress.")]
        public void SWNRegistrationEx_InvalidIpAddress()
        {
            SWNRegister(SwnRegisterType.InvalidIpAddress, SwnVersion.SWN_VERSION_2);
        }

        [TestMethod]
        [TestCategory(TestCategories.Swn)]
        [TestCategory(TestCategories.NonSmb)]
        [TestCategory(TestCategories.UnexpectedFields)]
        [Description("Register with WitnessrRegisterEx and Unregister the client twice.")]
        public void SWNRegistrationEx_InvalidUnRegister()
        {
            SWNRegister(SwnRegisterType.InvalidUnRegister, SwnVersion.SWN_VERSION_2);
        }

        [TestMethod]
        [TestCategory(TestCategories.Swn)]
        [TestCategory(TestCategories.NonSmb)]
        [TestCategory(TestCategories.UnexpectedFields)]
        [Description("Register with invalid sharename.")]
        public void SWNRegistrationEx_InvalidShareName()
        {
            SWNRegister(SwnRegisterType.InvalidShareName, SwnVersion.SWN_VERSION_2);
        }

        [TestMethod]
        [TestCategory(TestCategories.Swn)]
        [TestCategory(TestCategories.NonSmb)]
        [TestCategory(TestCategories.Compatibility)]
        [Description("Get Timeout notification on scaleout cluster server.")]
        public void WitnessrRegisterEx_SWNAsyncNotification_Timeout()
        {
            SWNRegister(SwnRegisterType.KeepAliveTimeout, SwnVersion.SWN_VERSION_2);
        }

        private void SWNRegister(SwnRegisterType registerType, SwnVersion expectedVersion)
        {
            WITNESS_INTERFACE_INFO registerInterface;
            IntPtr pDuplicateContext = IntPtr.Zero;
            swnClientForInterface = new SwnClient();
            swnClientForWitness = new SwnClient();
            string server = TestConfig.ClusteredFileServerName;

            IPAddress currentAccessIpAddr = SWNTestUtility.GetCurrentAccessIP(server);

            #region Get SWN witness interface list
            BaseTestSite.Log.Add(LogEntryKind.TestStep, "Get SWN witness interface list.");

            DoUntilSucceed(() => SWNTestUtility.BindServer(swnClientForInterface, currentAccessIpAddr,
                TestConfig.DomainName, TestConfig.UserName, TestConfig.UserPassword, TestConfig.DefaultSecurityPackage,
                TestConfig.DefaultRpceAuthenticationLevel, TestConfig.FailoverTimeout, server), TestConfig.FailoverTimeout,
                "Retry BindServer until succeed within timeout span");

            int ret;
            WITNESS_INTERFACE_LIST interfaceList = new WITNESS_INTERFACE_LIST();

            DoUntilSucceed(() =>
            {
                ret = swnClientForInterface.WitnessrGetInterfaceList(out interfaceList);
                BaseTestSite.Assert.AreEqual<SwnErrorCode>(SwnErrorCode.ERROR_SUCCESS, (SwnErrorCode)ret, "WitnessrGetInterfaceList returns with result code = 0x{0:x8}", ret);
                return SWNTestUtility.VerifyInterfaceList(interfaceList, TestConfig.Platform);
            }, TestConfig.FailoverTimeout, "Retry to call WitnessrGetInterfaceList until succeed within timeout span");

            swnClientForInterface.SwnUnbind(TestConfig.FailoverTimeout);
            swnClientForInterface = null;

            SWNTestUtility.GetRegisterInterface(interfaceList, out registerInterface);

            SWNTestUtility.CheckVersion(expectedVersion, (SwnVersion)registerInterface.Version);
            #endregion

            #region Register SWN witness
            BaseTestSite.Log.Add(LogEntryKind.TestStep, "Register SWN witness with {0}.", registerType.ToString());

            DoUntilSucceed(() => SWNTestUtility.BindServer(swnClientForWitness,
                (registerInterface.Flags & (uint)SwnNodeFlagsValue.IPv4) != 0 ? new IPAddress(registerInterface.IPV4) : SWNTestUtility.ConvertIPV6(registerInterface.IPV6),
                TestConfig.DomainName, TestConfig.UserName, TestConfig.UserPassword, TestConfig.DefaultSecurityPackage,
                TestConfig.DefaultRpceAuthenticationLevel, TestConfig.FailoverTimeout), TestConfig.FailoverTimeout,
                "Retry BindServer until succeed within timeout span");

            SwnVersion registerVersion;
            if (SwnVersion.SWN_VERSION_2 == expectedVersion)
                registerVersion = (registerType == SwnRegisterType.InvalidVersion ? SwnVersion.SWN_VERSION_1 : SwnVersion.SWN_VERSION_2);
            else
                registerVersion = (registerType == SwnRegisterType.InvalidVersion ? SwnVersion.SWN_VERSION_2 : SwnVersion.SWN_VERSION_1);
            string registerNetName =
                (registerType == SwnRegisterType.InvalidNetName ? "XXXXInvalid.contoso.comXXXX" : SWNTestUtility.GetPrincipleName(TestConfig.DomainName, server));
            string accessIpAddr =
                (registerType == SwnRegisterType.InvalidIpAddress ? "255.255.255.255" : currentAccessIpAddr.ToString());
            string registerClientName = Guid.NewGuid().ToString();
            string shareName =
                (registerType == SwnRegisterType.InvalidShareName ? "XXXXInvalidShareNameXXXX" : TestConfig.ClusteredFileShare);
            uint keepAliveTimout =
                (registerType == SwnRegisterType.KeepAliveTimeout ? (uint)10 : (uint)120);

            BaseTestSite.Log.Add(LogEntryKind.Debug, "Register witness:");
            BaseTestSite.Log.Add(LogEntryKind.Debug, "\tVersion: {0:x8}", (uint)registerVersion);
            BaseTestSite.Log.Add(LogEntryKind.Debug, "\tNetName: {0}", registerNetName);
            BaseTestSite.Log.Add(LogEntryKind.Debug, "\tShareName: {0}", shareName);
            BaseTestSite.Log.Add(LogEntryKind.Debug, "\tIpAddress: {0}", accessIpAddr);
            BaseTestSite.Log.Add(LogEntryKind.Debug, "\tClientName: {0}", registerClientName);
            if (SwnVersion.SWN_VERSION_2 == expectedVersion)
            {
                ret = swnClientForWitness.WitnessrRegisterEx(registerVersion,
                    registerNetName,
                    shareName,
                    accessIpAddr,
                    registerClientName,
                    WitnessrRegisterExFlagsValue.WITNESS_REGISTER_NONE,
                    keepAliveTimout,
                    out pContext);
            }
            else
            {
                ret = swnClientForWitness.WitnessrRegister(registerVersion,
                    registerNetName,
                    accessIpAddr,
                    registerClientName,
                    out pContext);
            }

            BaseTestSite.Log.Add(LogEntryKind.TestStep, "Verify server response for WitnessrRegister request.");

            // TDI to be filed, original fix for TDI 66777 and 69401 is not accurate, need clarify in new TDI
            // Windows server won't fail the request if the target resource being monitored is ScaleOut FS share when request containst invalid IP address or invalid share name
            // TODO: Test case should have ability to choose different server, otherwise we do not need such condition
            bool isScaleOutFsShare = ShareContainsSofs(server, Smb2Utility.GetUncPath(server, TestConfig.ClusteredFileShare));

            if (registerType == SwnRegisterType.InvalidNetName)
            {
                BaseTestSite.Assert.AreEqual<SwnErrorCode>(SwnErrorCode.ERROR_INVALID_PARAMETER, (SwnErrorCode)ret, "WitnessrRegister returns with result code = 0x{0:x8}", ret);
            }
            else if (registerType == SwnRegisterType.InvalidVersion)
            {
                BaseTestSite.Assert.AreEqual<SwnErrorCode>(SwnErrorCode.ERROR_REVISION_MISMATCH, (SwnErrorCode)ret, "WitnessrRegister returns with result code = 0x{0:x8}", ret);
            }
            else if (registerType == SwnRegisterType.InvalidIpAddress)
            {
                if (!isScaleOutFsShare)
                {
                    BaseTestSite.Assert.AreEqual<SwnErrorCode>(SwnErrorCode.ERROR_SUCCESS, (SwnErrorCode)ret, "WitnessrRegister returns with result code = 0x{0:x8}", ret);
                }
                else
                {
                    BaseTestSite.Assert.AreEqual<SwnErrorCode>(SwnErrorCode.ERROR_INVALID_STATE, (SwnErrorCode)ret, "WitnessrRegister returns with result code = 0x{0:x8}", ret);
                }
            }
            else if (registerType == SwnRegisterType.InvalidShareName)
            {
                if (!isScaleOutFsShare)
                {
                    BaseTestSite.Assert.AreEqual<SwnErrorCode>(SwnErrorCode.ERROR_SUCCESS, (SwnErrorCode)ret, "WitnessrRegister returns with result code = 0x{0:x8}", ret);
                }
                else
                {
                    BaseTestSite.Assert.AreEqual<SwnErrorCode>(SwnErrorCode.ERROR_INVALID_STATE, (SwnErrorCode)ret, "WitnessrRegister returns with result code = 0x{0:x8}", ret);
                }
            }
            else
            {
                BaseTestSite.Assert.AreEqual<SwnErrorCode>(SwnErrorCode.ERROR_SUCCESS, (SwnErrorCode)ret, "WitnessrRegister returns with result code = 0x{0:x8}", ret);
            }

            if (registerType == SwnRegisterType.InvalidUnRegister)
            {
                ret = swnClientForWitness.WitnessrUnRegister(pContext);
                BaseTestSite.Assert.AreEqual<SwnErrorCode>(SwnErrorCode.ERROR_SUCCESS, (SwnErrorCode)ret, "WitnessrUnRegister returns with result code = 0x{0:x8}", ret);

                ret = swnClientForWitness.WitnessrUnRegister(pContext);
                if (TestConfig.Platform == Platform.WindowsServer2012)
                {
                    BaseTestSite.Assert.AreEqual<SwnErrorCode>(SwnErrorCode.ERROR_NOT_FOUND, (SwnErrorCode)ret, "WitnessrUnRegister returns with result code = 0x{0:x8}", ret);
                }
                else
                {
                    BaseTestSite.Assert.AreEqual<SwnErrorCode>(SwnErrorCode.ERROR_INVALID_PARAMETER, (SwnErrorCode)ret, "WitnessrUnRegister returns with result code = 0x{0:x8}", ret);
                }
                pContext = IntPtr.Zero;
                swnClientForWitness.SwnUnbind(TestConfig.FailoverTimeout);
            }

            if (registerType == SwnRegisterType.InvalidRequest)
            {
                ret = swnClientForWitness.WitnessrUnRegister(pContext);
                BaseTestSite.Assert.AreEqual<SwnErrorCode>(SwnErrorCode.ERROR_SUCCESS, (SwnErrorCode)ret, "WitnessrUnRegister returns with result code = 0x{0:x8}", ret);

                uint callId;
                callId = swnClientForWitness.WitnessrAsyncNotify(pContext);
                BaseTestSite.Assert.AreNotEqual<uint>(0, callId, "WitnessrAsyncNotify returns callId = {0}", callId);

                RESP_ASYNC_NOTIFY respNotify;
                ret = swnClientForWitness.ExpectWitnessrAsyncNotify(callId, out respNotify);
                BaseTestSite.Assert.AreEqual<SwnErrorCode>(SwnErrorCode.ERROR_NOT_FOUND, (SwnErrorCode)ret, "ExpectWitnessrAsyncNotify returns with result code = 0x{0:x8}", ret);

                pContext = IntPtr.Zero;
                swnClientForWitness.SwnUnbind(TestConfig.FailoverTimeout);
            }

            if (registerType == SwnRegisterType.KeepAliveTimeout)
            {
                uint callId;
                callId = swnClientForWitness.WitnessrAsyncNotify(pContext);
                BaseTestSite.Assert.AreNotEqual<uint>(0, callId, "WitnessrAsyncNotify returns callId = {0}", callId);

                RESP_ASYNC_NOTIFY respNotify;
                ret = swnClientForWitness.ExpectWitnessrAsyncNotify(callId, out respNotify);
                BaseTestSite.Assert.AreEqual<SwnErrorCode>(SwnErrorCode.ERROR_TIMEOUT, (SwnErrorCode)ret, "ExpectWitnessrAsyncNotify returns with result code = 0x{0:x8}", ret);

                pContext = IntPtr.Zero;
                swnClientForWitness.SwnUnbind(TestConfig.FailoverTimeout);
            }
            #endregion

            #region Cleanup
            pContext = IntPtr.Zero;
            swnClientForWitness.SwnUnbind(TestConfig.FailoverTimeout);
            swnClientForWitness = null;
            #endregion
        }

        /// <summary>
        /// Determine if a share has shi*_type set to STYPE_CLUSTER_SOFS
        /// </summary>
        /// <param name="server">Server of the share</param>
        /// <param name="sharePath">Path of the share</param>
        /// <returns>Return true if share has shi*_type set to STYPE_CLUSTER_SOFS, otherwise return false</returns>
        private bool ShareContainsSofs(string server, string sharePath)
        {
            Smb2FunctionalClient client = new Smb2FunctionalClient(TestConfig.Timeout, TestConfig, BaseTestSite);

            client.ConnectToServerOverTCP(SWNTestUtility.GetCurrentAccessIP(server));

            client.Negotiate(TestConfig.RequestDialects, TestConfig.IsSMB1NegotiateEnabled);

            client.SessionSetup(TestConfig.DefaultSecurityPackage, server, TestConfig.AccountCredential, false);

            uint treeId;
            bool ret = false;
            client.TreeConnect(sharePath, out treeId,
                (header, response) =>
                {
                    BaseTestSite.Assert.AreEqual(
                        Smb2Status.STATUS_SUCCESS,
                        header.Status,
                        "{0} should be successful.", header.Command);

                    ret = response.Capabilities.HasFlag(Share_Capabilities_Values.SHARE_CAP_SCALEOUT);
                });

            client.TreeDisconnect(treeId);

            client.LogOff();

            client.Disconnect();

            return ret;
        }
    }
}
