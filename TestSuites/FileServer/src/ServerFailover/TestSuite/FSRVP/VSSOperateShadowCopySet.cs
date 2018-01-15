// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Protocols.TestSuites.FileSharing.Common.Adapter;
using Microsoft.Protocols.TestSuites.FileSharing.Common.TestSuite;
using Microsoft.Protocols.TestTools;
using Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Fsrvp;
using Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Smb2;
using Microsoft.Protocols.TestTools.StackSdk.Security.Sspi;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;

namespace Microsoft.Protocols.TestSuites.FileSharing.ServerFailover.TestSuite.FSRVP
{
    /// <summary>
    /// This test class is for VSS test case.
    /// </summary>
    [TestClass]
    public partial class VSSOperateShadowCopySet : ServerFailoverTestBase
    {
        #region Variables

        /// <summary>
        /// FSRVP client for creating shadow copy.
        /// </summary>
        private FsrvpClient fsrvpClientForCreation;
        /// <summary>
        /// The id of shadow copy set.
        /// </summary>
        private Guid shadowCopySetId;
        /// <summary>
        /// The list of shadow copy.
        /// </summary>
        private List<FsrvpClientShadowCopy> shadowCopyList = new List<FsrvpClientShadowCopy>();
        /// <summary>
        /// Indicates the status of the shadow copy set.
        /// </summary>
        private FsrvpStatus fsrvpStatus;

        #endregion

        #region Test Suite Initialization

        // Use ClassInitialize to run code before running the first test in the class
        [ClassInitialize()]
        public static void ClassInitialize(TestContext testContext)
        {
            TestClassBase.Initialize(testContext);
        }

        // Use ClassCleanup to run code after all tests in a class have run
        [ClassCleanup()]
        public static void ClassCleanup()
        {
            TestClassBase.Cleanup();
        }

        #endregion

        #region Test Case Initialization
        // Use TestInitialize to run code before running every test in the class
        protected override void TestInitialize()
        {
            base.TestInitialize();
            fsrvpStatus = FsrvpStatus.None;
            shadowCopySetId = Guid.Empty;
            shadowCopyList.Clear();
        }

        // Use TestCleanup to run code after every test in a class have run
        protected override void TestCleanup()
        {
            if (fsrvpClientForCreation != null)
            {
                try
                {
                    if (fsrvpStatus == FsrvpStatus.Started || fsrvpStatus == FsrvpStatus.Added)
                    {
                        fsrvpClientForCreation.AbortShadowCopySet(shadowCopySetId);
                    }

                    if (fsrvpStatus == FsrvpStatus.CreateInProgress)
                    {
                        fsrvpClientForCreation.CommitShadowCopySet(shadowCopySetId,
                            (uint)FsrvpUtility.FSRVPCommitTimeoutInSeconds * 1000);
                        fsrvpStatus = FsrvpStatus.Committed;
                    }

                    if (fsrvpStatus == FsrvpStatus.Committed)
                    {
                        fsrvpClientForCreation.ExposeShadowCopySet(shadowCopySetId,
                            (uint)FsrvpUtility.FSRVPExposeTimeoutInSeconds * 1000);
                        fsrvpStatus = FsrvpStatus.Exposed;

                        #region GetShareMapping
                        for (int i = 0; i < shadowCopyList.Count; i++)
                        {
                            FsrvpClientShadowCopy shadowCopy = shadowCopyList[i];
                            FSSAGENT_SHARE_MAPPING mapping;
                            int ret = fsrvpClientForCreation.GetShareMapping(shadowCopy.serverShadowCopyId,
                                shadowCopySetId,
                                shadowCopy.shareName,
                                (uint)FsrvpLevel.FSRVP_LEVEL_1,
                                out mapping);
                            if ((FsrvpErrorCode)ret == FsrvpErrorCode.FSRVP_SUCCESS)
                            {
                                shadowCopy.exposedName = mapping.ShareMapping1.ShadowCopyShareName;
                                shadowCopy.CreationTimestamp = mapping.ShareMapping1.CreationTimestamp;

                                shadowCopyList[i] = shadowCopy;
                            }
                        }
                        #endregion
                    }

                    if (fsrvpStatus == FsrvpStatus.Exposed)
                    {
                        fsrvpClientForCreation.RecoveryCompleteShadowCopySet(shadowCopySetId);
                            fsrvpStatus = FsrvpStatus.Recovered;

                        foreach (FsrvpClientShadowCopy shadowCopy in shadowCopyList)
                        {
                            fsrvpClientForCreation.DeleteShareMapping(shadowCopySetId,
                                shadowCopy.serverShadowCopyId,
                                shadowCopy.shareName);
                        }
                    }
                }
                catch (Exception ex)
                {
                    BaseTestSite.Log.Add(LogEntryKind.Warning, "TestCleanup at status {0} got an unexpected Exception: {1}", (uint)fsrvpStatus, ex.Message);
                }

                DisconnectServer(ref fsrvpClientForCreation);
            }
            shadowCopyList.Clear();
            fsrvpStatus = FsrvpStatus.None;

            base.TestCleanup();
        }

        #endregion

        #region Test Cases for VSSOperateShadowCopySet

        [TestMethod]
        [TestCategory(TestCategories.Bvt)]
        [TestCategory(TestCategories.Fsrvp)]
        [TestCategory(TestCategories.NonSmb)]
        [Description("Check if the general file server supports the VSS provider to create a writable snapshot for remote files.")]
        public void BVT_VSSOperateShadowCopySet_WritableSnapshot_GeneralFileServer()
        {
            List<string> shareUncPaths = new List<string>();
            shareUncPaths.Add(@"\\" + TestConfig.ClusteredFileServerName + @"\" + TestConfig.ClusteredFileShare);
            TestShadowCopySet((ulong)FsrvpContextValues.FSRVP_CTX_BACKUP | (ulong)FsrvpShadowCopyAttributes.FSRVP_ATTR_AUTO_RECOVERY,
                shareUncPaths, FsrvpStatus.None, FsrvpSharePathsType.None);
        }


        [TestMethod]
        [TestCategory(TestCategories.Bvt)]
        [TestCategory(TestCategories.Fsrvp)]
        [TestCategory(TestCategories.NonSmb)]
        [Description("Check if the scaleout file server supports the VSS provider to create a writable snapshot for remote files.")]
        public void BVT_VSSOperateShadowCopySet_WritableSnapshot_ScaleoutFileServer()
        {
            List<string> shareUncPaths = new List<string>();
            shareUncPaths.Add(@"\\" + TestConfig.ClusteredScaleOutFileServerName + @"\" + TestConfig.ClusteredFileShare);
            TestShadowCopySet((ulong)FsrvpContextValues.FSRVP_CTX_BACKUP | (ulong)FsrvpShadowCopyAttributes.FSRVP_ATTR_AUTO_RECOVERY,
                shareUncPaths, FsrvpStatus.None, FsrvpSharePathsType.None);
        }


        [TestMethod]
        [TestCategory(TestCategories.FsrvpNonClusterRequired)]
        [TestCategory(TestCategories.NonSmb)]
        [TestCategory(TestCategories.Compatibility)]
        [Description("Check if the server returns correctly when adding different sharePath on multi nodes to a shadow copy set.")]
        public void VSSOperateShadowCopySet_DifferentNodeSharePath()
        {
            List<string> shareUncPaths = new List<string>();
            shareUncPaths.Add(@"\\" + TestConfig.ClusterNode01 + @"\" + TestConfig.BasicFileShare);
            shareUncPaths.Add(@"\\" + TestConfig.ClusterNode02 + @"\" + TestConfig.BasicFileShare);
            TestShadowCopySet((ulong)FsrvpContextValues.FSRVP_CTX_BACKUP | (ulong)FsrvpShadowCopyAttributes.FSRVP_ATTR_AUTO_RECOVERY,
                shareUncPaths, FsrvpStatus.None, FsrvpSharePathsType.OnDifferentNode);
        }


        [TestMethod]
        [TestCategory(TestCategories.Bvt)]
        [TestCategory(TestCategories.Fsrvp)]
        [TestCategory(TestCategories.NonSmb)]
        [Description("Check if the server returns correctly when adding different sharePath on the cluster and the owner node to a shadow copy set.")]
        public void BVT_VSSOperateShadowCopySet_ClusterSharePath_OwnerNode()
        {
            List<string> shareUncPaths = new List<string>();
            shareUncPaths.Add(@"\\" + TestConfig.ClusteredFileServerName + @"\" + TestConfig.ClusteredFileShare);

            BaseTestSite.Log.Add(LogEntryKind.TestStep, "Get the owner node of the clustered file server.");

            string ownerNode = GetClusterOwnerNode(TestConfig.ClusteredFileServerName);
            if (!Smb2Utility.GetPrincipleName(ownerNode).Equals(Smb2Utility.GetPrincipleName(TestConfig.ClusterNode01)))
            {
                // testConfig.BasicFileShare only exists on testConfig.ClusterNode01
                string resName = Smb2Utility.GetPrincipleName(TestConfig.ClusteredFileServerName);
                BaseTestSite.Log.Add(LogEntryKind.Debug, "Move the owner of {0} to {1}.", resName, TestConfig.ClusterNode01);
                bool ret = sutController.MoveClusterResourceOwner(resName, TestConfig.ClusterNode01);
                BaseTestSite.Assert.IsTrue(ret, "Expect that moving the owner node of {0} to {1} succeeds.",
                    resName, TestConfig.ClusterNode01);

                ownerNode = GetClusterOwnerNode(TestConfig.ClusteredFileServerName);
                BaseTestSite.Assert.IsTrue(string.Compare(Smb2Utility.GetPrincipleName(ownerNode), Smb2Utility.GetPrincipleName(TestConfig.ClusterNode01), true) == 0,
                    "Expect that the new owner is {0}. The actual owner is {1}.",
                    Smb2Utility.GetPrincipleName(TestConfig.ClusterNode01),
                    Smb2Utility.GetPrincipleName(ownerNode));
            }
            shareUncPaths.Add(@"\\" + ownerNode + @"\" + TestConfig.BasicFileShare);
            TestShadowCopySet((ulong)FsrvpContextValues.FSRVP_CTX_BACKUP | (ulong)FsrvpShadowCopyAttributes.FSRVP_ATTR_AUTO_RECOVERY,
                shareUncPaths, FsrvpStatus.None, FsrvpSharePathsType.OnClusterAndOwnerNode);
        }


        [TestMethod]
        [TestCategory(TestCategories.Bvt)]
        [TestCategory(TestCategories.Fsrvp)]
        [TestCategory(TestCategories.NonSmb)]
        [Description("Check if the server returns correctly when adding different sharePath on the cluster and the non-owner node to a shadow copy set.")]
        public void BVT_VSSOperateShadowCopySet_ClusterSharePath_NonOwnerNode()
        {
            string nonOwnerNode = "";
            List<string> shareUncPaths = new List<string>();
            shareUncPaths.Add(@"\\" + TestConfig.ClusteredFileServerName + @"\" + TestConfig.ClusteredFileShare);

            BaseTestSite.Log.Add(LogEntryKind.TestStep, "Get one non-owner node of the clustered file server.");

            string ownerNode = GetClusterOwnerNode(TestConfig.ClusteredFileServerName).ToUpper();
            if (!ownerNode.Contains(TestConfig.DomainName.ToUpper()))
            {
                ownerNode += "." + TestConfig.DomainName.ToUpper();
            }

            if (ownerNode == TestConfig.ClusterNode01.ToUpper())
            {
                nonOwnerNode = TestConfig.ClusterNode02;
            }
            else
            {
                nonOwnerNode = TestConfig.ClusterNode01;
            }
            BaseTestSite.Log.Add(LogEntryKind.Debug, "Select {0} as non-owner node.", nonOwnerNode);
            shareUncPaths.Add(@"\\" + nonOwnerNode + @"\" + TestConfig.BasicFileShare);
            TestShadowCopySet((ulong)FsrvpContextValues.FSRVP_CTX_BACKUP | (ulong)FsrvpShadowCopyAttributes.FSRVP_ATTR_AUTO_RECOVERY,
                shareUncPaths, FsrvpStatus.None, FsrvpSharePathsType.OnClusterAndNonOwnerNode);
        }
        #endregion

        #region Utilites
        /// <summary>
        /// Connect to server.
        /// </summary>
        /// <param name="client">Fsrvp client.</param>
        /// <param name="server">The name of server.</param>
        /// <returns>Return true if success, otherwise return false.</returns>
        private bool ConnectServer(ref FsrvpClient client, string server)
        {
            AccountCredential accountCredential =
                new AccountCredential(TestConfig.DomainName, TestConfig.UserName, TestConfig.UserPassword);

            ClientSecurityContext securityContext =
                new SspiClientSecurityContext(
                    TestConfig.DefaultSecurityPackage,
                    accountCredential,
                    Smb2Utility.GetCifsServicePrincipalName(server),
                    ClientSecurityContextAttribute.Connection
                        | ClientSecurityContextAttribute.DceStyle
                        | ClientSecurityContextAttribute.Integrity
                        | ClientSecurityContextAttribute.ReplayDetect
                        | ClientSecurityContextAttribute.SequenceDetect
                        | ClientSecurityContextAttribute.UseSessionKey,
                    SecurityTargetDataRepresentation.SecurityNativeDrep);

            // This indicates that the RPC message is just integrity-protected. 
            client.Context.AuthenticationLevel = TestConfig.DefaultRpceAuthenticationLevel;

            try
            {
                BaseTestSite.Log.Add(LogEntryKind.Debug, "Connect to server {0}.", server);
                client.BindOverNamedPipe(server, accountCredential, securityContext,
                    new TimeSpan(0, 0, (int)FsrvpUtility.FSRVPTimeoutInSeconds));
            }
            catch (Exception ex)
            {
                BaseTestSite.Log.Add(LogEntryKind.Debug,
                    "Connect to server {0} failed. Exception: {1}",
                    server,
                    ex.Message);
                client.Unbind(TestConfig.Timeout);
                return false;
            }

            BaseTestSite.Log.Add(LogEntryKind.Debug, "Connect to server {0} successfully.", server);
            return true;
        }

        /// <summary>
        /// Disconnect from server.
        /// </summary>
        /// <param name="client">Fsrvp client.</param>
        private void DisconnectServer(ref FsrvpClient client)
        {
            if (client != null)
            {
                try
                {
                    BaseTestSite.Log.Add(LogEntryKind.Debug, "Disconnect from server.");
                    client.Unbind(TestConfig.Timeout);
                    client = null;
                }
                catch (Exception ex)
                {
                    BaseTestSite.Log.Add(LogEntryKind.Warning, "TestCleanup: Unexpected Exception:", ex);
                }
            }
        }

        /// <summary>
        /// Get the hostname which the shareUncPath belongs to.
        /// </summary>
        /// <param name="shareUncPath">The full path of the share in UNC format.</param>
        /// <returns>The hostname which the shareUncPath belongs to.</returns>
        private string GetHostname(string shareUncPath)
        {
            BaseTestSite.Assume.IsFalse(string.IsNullOrEmpty(shareUncPath), "shareUncPath is not null or empty");
            BaseTestSite.Assume.IsTrue(shareUncPath.Length > 3, "The length of shareUncPath is bigger than 3.");

            int i = shareUncPath.IndexOf('\\', 2);

            return shareUncPath.Substring(2, i - 2);
        }

        /// <summary>
        /// Get the name of exposed share according to TD.
        /// </summary>
        /// <param name="shareUncPath">The full path of the share in UNC format.</param>
        /// <param name="shadowCopyId">The GUID of the shadow copy associated with the share.</param>
        /// <returns>The name of exposed share</returns>
        private string GetExposedShareName(string shareUncPath, Guid shadowCopyId)
        {
            BaseTestSite.Assume.IsFalse(string.IsNullOrEmpty(shareUncPath), "shareUncPath is not null or empty");

            int i = shareUncPath.IndexOf('\\', 2);

            return shareUncPath.Substring(i + 1) + "@{" + shadowCopyId.ToString() + "}"; ;
        }
        #endregion

        #region Test Methods
        /// <summary>
        /// Get the name of FSRVP server which the client MUST connect to create shadow copies of the specified shareName.
        /// </summary>
        /// <param name="shareName">The full path of the share in UNC format.</param>
        /// <returns>The name of FSRVP server.</returns>
        private string GetFsrvpServerName(string shareName)
        {
            int ret;
            string FsrvpServerName = "";
            string serverName;
            FsrvpClient fsrvpClientForQuery = new FsrvpClient();

            serverName = GetHostname(shareName);
            BaseTestSite.Assume.IsFalse(string.IsNullOrEmpty(serverName),
                "serverName is valid. The actual value is {0}.", serverName);

            try
            {
                DoUntilSucceed(() => ConnectServer(ref fsrvpClientForQuery, serverName), TestConfig.LongerTimeout,
                    "Retry ConnectServer until succeed within timeout span");

                BaseTestSite.Log.Add(LogEntryKind.Debug,
                    "Start to call IsPathSupported({0}, out SupportedByThisProvider, out OwnerMachineName)",
                    shareName);
                bool SupportedByThisProvider;
                ret = fsrvpClientForQuery.IsPathSupported(shareName, out SupportedByThisProvider, out FsrvpServerName);
                BaseTestSite.Assert.AreEqual<FsrvpErrorCode>(FsrvpErrorCode.FSRVP_SUCCESS, (FsrvpErrorCode)ret,
                    "The return value of IsPathSupported is expected to be 0x{0:x8}. The server actually returns 0x{1:x8}.",
                    (int)FsrvpErrorCode.FSRVP_SUCCESS, ret);
                BaseTestSite.Assert.IsTrue(SupportedByThisProvider,
                    "Expect that shadow copies of this share are supported by the server.");
                BaseTestSite.Assert.IsFalse(string.IsNullOrEmpty(FsrvpServerName),
                    "Expect that OwnerMachineName is not null or empty. The server actually returns {0}.", FsrvpServerName);
            }
            finally
            {
                DisconnectServer(ref fsrvpClientForQuery);
            }

            return FsrvpServerName;
        }

        private string GetClusterOwnerNode(string server)
        {
            string currentAccessNode = null;
            DoUntilSucceed(() =>
            {
                currentAccessNode = sutController.GetClusterResourceOwner(server);
                return (!string.IsNullOrEmpty(currentAccessNode));
            }, TestConfig.FailoverTimeout, "Retry to get cluster owner node until succeed within timeout span");

            BaseTestSite.Assert.AreNotEqual(
                true,
                string.IsNullOrEmpty(currentAccessNode),
                "Current owner node is NOT expected to be Null or Empty");
            BaseTestSite.Log.Add(
                LogEntryKind.Debug,
                "Current owner node is {0}", currentAccessNode);

            return currentAccessNode;
        }

        /// <summary>
        /// Test set context with invalid parameter.
        /// </summary>
        /// <param name="context">The context to be used for the shadow copy operations.</param>
        /// <param name="shareUncPaths">The full path list of the shares in UNC format.</param>
        private void TestInvalidSetContext(ulong context, List<string> shareUncPaths)
        {
            int ret;
            string FsrvpServerName;

            #region Query FSRVP Server Name
            FsrvpServerName = GetFsrvpServerName(shareUncPaths[0]);
            #endregion

            #region Connect to FSRVP server
            fsrvpClientForCreation = new FsrvpClient();
            DoUntilSucceed(() => ConnectServer(ref fsrvpClientForCreation, FsrvpServerName), TestConfig.LongerTimeout,
                "Retry ConnectServer until succeed within timeout span");
            #endregion

            #region GetSupportedVersion
            BaseTestSite.Log.Add(LogEntryKind.Debug, "Start to call GetSupportedVersion(out MinVersion, out MaxVersion)");
            uint MinVersion;
            uint MaxVersion;
            ret = fsrvpClientForCreation.GetSupportedVersion(out MinVersion, out MaxVersion);
            BaseTestSite.Assert.AreEqual<FsrvpErrorCode>(FsrvpErrorCode.FSRVP_SUCCESS, (FsrvpErrorCode)ret,
                "The return value of GetSupportedVersion is expected to be 0x{0:x8}. The server actually returns 0x{1:x8}.",
                (int)FsrvpErrorCode.FSRVP_SUCCESS, ret);
            BaseTestSite.Assert.AreEqual<uint>((uint)FsrvpVersionValues.FSRVP_RPC_VERSION_1,
                MinVersion,
                "Expect that the minimum version of the protocol supported by the server is 0x{0:x8}. The server actually returns 0x{1:x8}.",
                (uint)FsrvpVersionValues.FSRVP_RPC_VERSION_1,
                MinVersion);
            BaseTestSite.Assert.AreEqual<uint>((uint)FsrvpVersionValues.FSRVP_RPC_VERSION_1,
                MaxVersion,
                "Expect that the maximum version of the protocol supported by the server is 0x{0:x8}. The server actually returns 0x{1:x8}.",
                (uint)FsrvpVersionValues.FSRVP_RPC_VERSION_1,
                MaxVersion);
            #endregion

            #region SetContext
            BaseTestSite.Log.Add(LogEntryKind.Debug, "Start to call SetContext(0x{0:x8}).", context);
            ret = fsrvpClientForCreation.SetContext(context);
            BaseTestSite.Assert.AreEqual<FsrvpErrorCode>(FsrvpErrorCode.FSRVP_E_UNSUPPORTED_CONTEXT, (FsrvpErrorCode)ret,
                "The return value of SetContext is expected to be 0x{0:x8}. The server actually returns 0x{1:x8}.",
                (uint)FsrvpErrorCode.FSRVP_E_UNSUPPORTED_CONTEXT, ret);
            #endregion
        }

        /// <summary>
        /// Test the operation of shadow copy set.
        /// </summary>
        /// <param name="context">The context to be used for the shadow copy operations.</param>
        /// <param name="shareUncPaths">The full path list of the shares in UNC format.</param>
        /// <param name="statusToAbort">Indicates which status the server is in, the creation process will be aborted and exit.
        /// FsrvpStatus.None indicates that AbortShadowCopySet will not be called and all operations will be executed.</param>
        /// <param name="sharePathsType">Indicates the type of share paths.</param>
        private void TestShadowCopySet(ulong context, List<string> shareUncPaths, FsrvpStatus statusToAbort,
            FsrvpSharePathsType sharePathsType)
        {
            int ret;
            string FsrvpServerName;

            #region Query FSRVP Server Name
            BaseTestSite.Log.Add(LogEntryKind.TestStep, "Query FSRVP Server Name.");
            FsrvpServerName = GetFsrvpServerName(shareUncPaths[0]);
            #endregion

            #region Connect to FSRVP server
            BaseTestSite.Log.Add(LogEntryKind.TestStep, "Connect to FSRVP server.");
            fsrvpClientForCreation = new FsrvpClient();
            DoUntilSucceed(() => ConnectServer(ref fsrvpClientForCreation, FsrvpServerName), TestConfig.LongerTimeout,
                "Retry ConnectServer until succeed within timeout span");
            #endregion

            #region GetSupportedVersion
            BaseTestSite.Log.Add(LogEntryKind.TestStep, "Get supported FSRVP versions.");
            BaseTestSite.Log.Add(LogEntryKind.Debug, "Start to call GetSupportedVersion(out MinVersion, out MaxVersion)");
            uint MinVersion;
            uint MaxVersion;
            ret = fsrvpClientForCreation.GetSupportedVersion(out MinVersion, out MaxVersion);
            BaseTestSite.Assert.AreEqual<FsrvpErrorCode>(FsrvpErrorCode.FSRVP_SUCCESS, (FsrvpErrorCode)ret,
                "The return value of GetSupportedVersion is expected to be 0x{0:x8}. The server actually returns 0x{1:x8}.",
                (int)FsrvpErrorCode.FSRVP_SUCCESS, ret);
            BaseTestSite.Assert.AreEqual<uint>((uint)FsrvpVersionValues.FSRVP_RPC_VERSION_1,
                MinVersion,
                "Expect that the minimum version of the protocol supported by the server is 0x{0:x8}. The server actually returns 0x{1:x8}.",
                (uint)FsrvpVersionValues.FSRVP_RPC_VERSION_1,
                MinVersion);
            BaseTestSite.Assert.AreEqual<uint>((uint)FsrvpVersionValues.FSRVP_RPC_VERSION_1,
                MaxVersion,
                "Expect that the maximum version of the protocol supported by the server is 0x{0:x8}. The server actually returns 0x{1:x8}.",
                (uint)FsrvpVersionValues.FSRVP_RPC_VERSION_1,
                MaxVersion);
            #endregion

            #region SetContext
            BaseTestSite.Log.Add(LogEntryKind.TestStep, "SetContext.");
            BaseTestSite.Log.Add(LogEntryKind.Debug, "Start to call SetContext(0x{0:x8}).", context);
            ret = fsrvpClientForCreation.SetContext(context);
            BaseTestSite.Assert.AreEqual<FsrvpErrorCode>(FsrvpErrorCode.FSRVP_SUCCESS, (FsrvpErrorCode)ret,
                "The return value of SetContext is expected to be 0x{0:x8}. The server actually returns 0x{1:x8}.",
                (int)FsrvpErrorCode.FSRVP_SUCCESS, ret);
            #endregion

            #region StartShadowCopySet
            BaseTestSite.Log.Add(LogEntryKind.TestStep, "StartShadowCopySet.");
            Guid clientShadowCopySetId = Guid.NewGuid();
            BaseTestSite.Log.Add(LogEntryKind.Debug,
                "Start to call StartShadowCopySet({0}, out pShadowCopySetId)",
                clientShadowCopySetId);
            ret = fsrvpClientForCreation.StartShadowCopySet(clientShadowCopySetId, out shadowCopySetId);
            BaseTestSite.Assert.AreEqual<FsrvpErrorCode>(FsrvpErrorCode.FSRVP_SUCCESS, (FsrvpErrorCode)ret,
                "The return value of StartShadowCopySet is expected to be 0x{0:x8}. The server actually returns 0x{1:x8}.",
                (int)FsrvpErrorCode.FSRVP_SUCCESS, ret);
            BaseTestSite.Assert.AreNotEqual<Guid>(Guid.Empty, shadowCopySetId,
                "The server is expected to return a valid shadowCopySetId. But the shadowCopySetId which the server returns is empty.");
            fsrvpStatus = FsrvpStatus.Started;
            #endregion

            if (statusToAbort == FsrvpStatus.Started)
            {
                BaseTestSite.Log.Add(LogEntryKind.TestStep, "Abort StartShadowCopySet request.");
                BaseTestSite.Log.Add(LogEntryKind.Debug,
                    "Start to call AbortShadowCopySet({0})",
                    shadowCopySetId);
                ret = fsrvpClientForCreation.AbortShadowCopySet(shadowCopySetId);
                BaseTestSite.Assert.AreEqual<FsrvpErrorCode>(FsrvpErrorCode.FSRVP_SUCCESS, (FsrvpErrorCode)ret,
                    "The return value of AbortShadowCopySet is expected to be 0x{0:x8}. The server actually returns 0x{1:x8}.",
                    (int)FsrvpErrorCode.FSRVP_SUCCESS, ret);
                return;
            }

            #region AddToShadowCopySet
            BaseTestSite.Log.Add(LogEntryKind.TestStep, "AddToShadowCopySet.");
            if (sharePathsType == FsrvpSharePathsType.OnClusterAndNonOwnerNode ||
                sharePathsType == FsrvpSharePathsType.OnDifferentNode)
            {
                // Negative test cases
                BaseTestSite.Assume.IsTrue(shareUncPaths.Count == 2, "shareUncPaths should contains two paths.");

                #region Valid ShareName
                FsrvpClientShadowCopy shadowCopy = new FsrvpClientShadowCopy();
                shadowCopy.shareName = shareUncPaths[0];
                shadowCopy.clientShadowCopyId = Guid.NewGuid();
                BaseTestSite.Log.Add(LogEntryKind.Debug,
                    "Start to call AddToShadowCopySet({0},{1},{2},out pShadowCopyId)",
                    shadowCopy.clientShadowCopyId, shadowCopySetId, shadowCopy.shareName);
                ret = fsrvpClientForCreation.AddToShadowCopySet(shadowCopy.clientShadowCopyId,
                    shadowCopySetId,
                    shadowCopy.shareName,
                    out shadowCopy.serverShadowCopyId);

                BaseTestSite.Assert.AreEqual<FsrvpErrorCode>(FsrvpErrorCode.FSRVP_SUCCESS, (FsrvpErrorCode)ret,
                    "The return value of AddToShadowCopySet is expected to be 0x{0:x8}. The server actually returns 0x{1:x8}.",
                    (int)FsrvpErrorCode.FSRVP_SUCCESS, ret);
                BaseTestSite.Assert.AreNotEqual<Guid>(Guid.Empty, shadowCopy.serverShadowCopyId,
                    "The server is expected to send a valid shadowCopyId.  But the shadowCopyId which the server returns is empty.");
                shadowCopyList.Add(shadowCopy);
                fsrvpStatus = FsrvpStatus.Added;
                #endregion

                #region Invalid ShareName
                shadowCopy = new FsrvpClientShadowCopy();
                shadowCopy.shareName = shareUncPaths[1];
                shadowCopy.clientShadowCopyId = Guid.NewGuid();
                BaseTestSite.Log.Add(LogEntryKind.Debug,
                    "Start to call AddToShadowCopySet({0},{1},{2},out pShadowCopyId)",
                    shadowCopy.clientShadowCopyId, shadowCopySetId, shadowCopy.shareName);
                ret = fsrvpClientForCreation.AddToShadowCopySet(shadowCopy.clientShadowCopyId,
                    shadowCopySetId,
                    shadowCopy.shareName,
                    out shadowCopy.serverShadowCopyId);

                BaseTestSite.Assert.AreEqual<FsrvpErrorCode>(FsrvpErrorCode.FSRVP_E_INVALIDARG, (FsrvpErrorCode)ret,
                    "The return value of AddToShadowCopySet is expected to be 0x{0:x8}. The server actually returns 0x{1:x8}.",
                    (uint)FsrvpErrorCode.FSRVP_E_INVALIDARG, ret);
                BaseTestSite.Assert.AreEqual<Guid>(Guid.Empty, shadowCopy.serverShadowCopyId,
                    "The server is expected to send an empty shadowCopyId.  The actual shadowCopyId which the server returns is {0}.",
                    shadowCopy.serverShadowCopyId.ToString());
                return;
                #endregion
            }
            else
            {
                foreach (string shareUncPath in shareUncPaths)
                {
                    FsrvpClientShadowCopy shadowCopy = new FsrvpClientShadowCopy();
                    shadowCopy.shareName = shareUncPath;
                    shadowCopy.clientShadowCopyId = Guid.NewGuid();
                    BaseTestSite.Log.Add(LogEntryKind.Debug,
                        "Start to call AddToShadowCopySet({0},{1},{2},out pShadowCopyId)",
                        shadowCopy.clientShadowCopyId, shadowCopySetId, shareUncPath);
                    ret = fsrvpClientForCreation.AddToShadowCopySet(shadowCopy.clientShadowCopyId,
                        shadowCopySetId,
                        shareUncPath,
                        out shadowCopy.serverShadowCopyId);

                    BaseTestSite.Assert.AreEqual<FsrvpErrorCode>(FsrvpErrorCode.FSRVP_SUCCESS, (FsrvpErrorCode)ret,
                        "The return value of AddToShadowCopySet is expected to be 0x{0:x8}. The server actually returns 0x{1:x8}.",
                        (int)FsrvpErrorCode.FSRVP_SUCCESS, ret);
                    BaseTestSite.Assert.AreNotEqual<Guid>(Guid.Empty, shadowCopy.serverShadowCopyId,
                        "The server is expected to send a valid shadowCopyId.  But the shadowCopyId which the server returns is empty.");
                    shadowCopyList.Add(shadowCopy);

                    fsrvpStatus = FsrvpStatus.Added;
                }
            }
            #endregion

            if (statusToAbort == FsrvpStatus.Added)
            {
                BaseTestSite.Log.Add(LogEntryKind.TestStep, "Abort AddToShadowCopySet request.");
                BaseTestSite.Log.Add(LogEntryKind.Debug,
                    "Start to call AbortShadowCopySet({0})",
                    shadowCopySetId);
                ret = fsrvpClientForCreation.AbortShadowCopySet(shadowCopySetId);
                BaseTestSite.Assert.AreEqual<FsrvpErrorCode>(FsrvpErrorCode.FSRVP_SUCCESS, (FsrvpErrorCode)ret,
                    "The return value of AbortShadowCopySet is expected to be 0x{0:x8}. The server actually returns 0x{1:x8}.",
                    (int)FsrvpErrorCode.FSRVP_SUCCESS, ret);
                return;
            }

            #region PrepareShadowCopySet
            BaseTestSite.Log.Add(LogEntryKind.TestStep, "PrepareShadowCopySet.");
            BaseTestSite.Log.Add(LogEntryKind.Debug,
                "Start to call PrepareShadowCopySet({0}, {1})",
                shadowCopySetId, FsrvpUtility.FSRVPPrepareTimeoutInSeconds * 1000);
            ret = fsrvpClientForCreation.PrepareShadowCopySet(shadowCopySetId,
                (uint)FsrvpUtility.FSRVPPrepareTimeoutInSeconds * 1000);
            BaseTestSite.Assert.AreEqual<FsrvpErrorCode>(FsrvpErrorCode.FSRVP_SUCCESS, (FsrvpErrorCode)ret,
                "The return value of PrepareShadowCopySet is expected to be 0x{0:x8}. The server actually returns 0x{1:x8}.",
                (int)FsrvpErrorCode.FSRVP_SUCCESS, ret);
            fsrvpStatus = FsrvpStatus.CreateInProgress;
            #endregion

            #region CommitShadowCopySet
            BaseTestSite.Log.Add(LogEntryKind.TestStep, "CommitShadowCopySet.");
            BaseTestSite.Log.Add(LogEntryKind.Debug,
                "Start to call CommitShadowCopySet({0},{1})",
                shadowCopySetId,
                FsrvpUtility.FSRVPCommitTimeoutInSeconds * 1000);
            ret = fsrvpClientForCreation.CommitShadowCopySet(shadowCopySetId,
                (uint)FsrvpUtility.FSRVPCommitTimeoutInSeconds * 1000);
            BaseTestSite.Assert.AreEqual<FsrvpErrorCode>(FsrvpErrorCode.FSRVP_SUCCESS, (FsrvpErrorCode)ret,
                "The return value of CommitShadowCopySet is expected to be 0x{0:x8}. The server actually returns 0x{1:x8}.",
                (int)FsrvpErrorCode.FSRVP_SUCCESS, ret);
            fsrvpStatus = FsrvpStatus.Committed;
            #endregion
            
            #region ExposeShadowCopySet
            BaseTestSite.Log.Add(LogEntryKind.TestStep, "ExposeShadowCopySet.");
            BaseTestSite.Log.Add(LogEntryKind.Debug,
                "Start to call ExposeShadowCopySet({0}, {1})",
                shadowCopySetId,
                FsrvpUtility.FSRVPExposeTimeoutInSeconds * 1000);
            ret = fsrvpClientForCreation.ExposeShadowCopySet(shadowCopySetId,
                (uint)FsrvpUtility.FSRVPExposeTimeoutInSeconds * 1000);
            BaseTestSite.Assert.AreEqual<FsrvpErrorCode>(FsrvpErrorCode.FSRVP_SUCCESS, (FsrvpErrorCode)ret,
                "The return value of ExposeShadowCopySet is expected to be 0x{0:x8}. The server actually returns 0x{1:x8}.",
                (int)FsrvpErrorCode.FSRVP_SUCCESS, ret);
            fsrvpStatus = FsrvpStatus.Exposed;
            #endregion

            #region GetShareMapping
            BaseTestSite.Log.Add(LogEntryKind.TestStep, "GetShareMapping.");
            for (int i = 0; i < shadowCopyList.Count; i++)
            {
                FsrvpClientShadowCopy shadowCopy = shadowCopyList[i];
                FSSAGENT_SHARE_MAPPING mapping;
                BaseTestSite.Log.Add(LogEntryKind.Debug,
                    "Start to call GetShareMapping({0}, {1}, {2}, {3}, out ShareMapping)",
                    shadowCopy.serverShadowCopyId,
                    shadowCopySetId,
                    shadowCopy.shareName,
                    FsrvpLevel.FSRVP_LEVEL_1);
                ret = fsrvpClientForCreation.GetShareMapping(shadowCopy.serverShadowCopyId,
                    shadowCopySetId,
                    shadowCopy.shareName,
                    (uint)FsrvpLevel.FSRVP_LEVEL_1,
                    out mapping);
                BaseTestSite.Assert.AreEqual<FsrvpErrorCode>(FsrvpErrorCode.FSRVP_SUCCESS, (FsrvpErrorCode)ret,
                    "The return value of GetShareMapping is expected to be 0x{0:x8}. The server actually returns 0x{1:x8}.",
                    (int)FsrvpErrorCode.FSRVP_SUCCESS, ret);
                BaseTestSite.Assert.IsFalse(mapping.ShareMapping1IsNull,
                    "Expect ShareMapping.ShareMapping1 the server returns is not null.");
                string exposedName = GetExposedShareName(shadowCopy.shareName, shadowCopy.serverShadowCopyId);
                BaseTestSite.Assert.IsTrue(
                    mapping.ShareMapping1.ShadowCopyShareName.Equals(exposedName, StringComparison.InvariantCultureIgnoreCase),
                    "Expect the exposed sharename returns by server is valid.");
                BaseTestSite.Assert.IsTrue(
                    mapping.ShareMapping1.CreationTimestamp > 0,
                    "Expect the CreationTimestamp returns by server is valid.");

                shadowCopy.exposedName = mapping.ShareMapping1.ShadowCopyShareName;
                shadowCopy.CreationTimestamp = mapping.ShareMapping1.CreationTimestamp;

                shadowCopyList[i] = shadowCopy;
            }
            #endregion

            #region Create a file in the exposed share and expect the failure.
            BaseTestSite.Log.Add(LogEntryKind.TestStep, "Create a file in the exposed share and expect the failure.");
            foreach (FsrvpClientShadowCopy shadowCopy in shadowCopyList)
            {
                string exposedSharePath = @"\\" + FsrvpServerName + @"\" + shadowCopy.exposedName;
                BaseTestSite.Log.Add(LogEntryKind.Debug, "Create a file {0} in the share: {1}.",
                    CurrentTestCaseName,
                    exposedSharePath);
                bool result = sutProtocolController.CreateFile(exposedSharePath, CurrentTestCaseName, string.Empty);
                if ((context & (ulong)FsrvpShadowCopyAttributes.FSRVP_ATTR_AUTO_RECOVERY) != 0)
                {
                    // Test writable snapshot
                    BaseTestSite.Assert.IsTrue(result, "Expect that creating the file in the share succeeds.");
                }
                else
                {
                    // Test readonly snapshot
                    BaseTestSite.Assert.IsFalse(result, "Expect that creating the file in the share fails.");
                }
            }
            #endregion

            #region RecoveryCompleteShadowCopySet
            BaseTestSite.Log.Add(LogEntryKind.TestStep, "RecoveryCompleteShadowCopySet.");
            BaseTestSite.Log.Add(LogEntryKind.Debug,
                "Start to call RecoveryCompleteShadowCopySet({0})",
                shadowCopySetId);
            ret = fsrvpClientForCreation.RecoveryCompleteShadowCopySet(shadowCopySetId);
            BaseTestSite.Assert.AreEqual<FsrvpErrorCode>(FsrvpErrorCode.FSRVP_SUCCESS, (FsrvpErrorCode)ret,
                "The return value of RecoveryCompleteShadowCopySet is expected to be 0x{0:x8}. The server actually returns 0x{1:x8}.",
                (int)FsrvpErrorCode.FSRVP_SUCCESS, ret);
            fsrvpStatus = FsrvpStatus.Recovered;
            #endregion

            if (statusToAbort == FsrvpStatus.Recovered)
            {
                BaseTestSite.Log.Add(LogEntryKind.TestStep, "Abort RecoveryCompleteShadowCopySet request.");
                BaseTestSite.Log.Add(LogEntryKind.Debug,
                    "Start to call AbortShadowCopySet({0})",
                    shadowCopySetId);
                ret = fsrvpClientForCreation.AbortShadowCopySet(shadowCopySetId);
                BaseTestSite.Assert.AreEqual<FsrvpErrorCode>(FsrvpErrorCode.FSRVP_E_BAD_STATE, (FsrvpErrorCode)ret,
                    "The return value of AbortShadowCopySet is expected to be 0x{0:x8}. The server actually returns 0x{1:x8}.",
                    (uint)FsrvpErrorCode.FSRVP_E_BAD_STATE, ret);
            }

            #region IsPathShadowCopied
            BaseTestSite.Log.Add(LogEntryKind.TestStep, "Check if the share paths are shadow copied.");
            foreach (string shareName in shareUncPaths)
            {
                BaseTestSite.Log.Add(LogEntryKind.Debug,
                    "Start to call IsPathShadowCopied({0}, out ShadowCopyPresent, out ShadowCopyCompatibility)",
                    shareName);
                bool ShadowCopyPresent = false;
                long ShadowCopyCompatibility = 0;
                ret = fsrvpClientForCreation.IsPathShadowCopied(shareName, out ShadowCopyPresent, out ShadowCopyCompatibility);
                BaseTestSite.Assert.AreEqual<FsrvpErrorCode>(FsrvpErrorCode.FSRVP_SUCCESS, (FsrvpErrorCode)ret,
                    "The return value of IsPathShadowCopied is expected to be 0x{0:x8}. The server actually returns 0x{1:x8}.",
                    (int)FsrvpErrorCode.FSRVP_SUCCESS, ret);
                BaseTestSite.Assert.IsTrue(ShadowCopyPresent,
                    "Expect that ShadowCopyPresent returned by the server is true.");
                BaseTestSite.Assert.IsTrue((ShadowCopyCompatibility == 0)
                    || (ShadowCopyCompatibility == (long)FsrvpShadowCopyCompatibilityValues.FSRVP_DISABLE_CONTENTINDEX)
                    || (ShadowCopyCompatibility == (long)FsrvpShadowCopyCompatibilityValues.FSRVP_DISABLE_DEFRAG)
                    || (ShadowCopyCompatibility == (long)(FsrvpShadowCopyCompatibilityValues.FSRVP_DISABLE_CONTENTINDEX | FsrvpShadowCopyCompatibilityValues.FSRVP_DISABLE_DEFRAG)),
                    "Expect that ShadowCopyCompatibility returned by the server is valid. The server actually returns 0x{0:x8}.",
                    ShadowCopyCompatibility);
            }
            #endregion

            #region DeleteShareMapping
            BaseTestSite.Log.Add(LogEntryKind.TestStep, "DeleteShareMapping.");
            foreach (FsrvpClientShadowCopy shadowCopy in shadowCopyList)
            {
                BaseTestSite.Log.Add(LogEntryKind.Debug,
                    "Start to call DeleteShareMapping({0}, {1}, {2})",
                    shadowCopySetId,
                    shadowCopy.serverShadowCopyId,
                    shadowCopy.shareName);
                ret = fsrvpClientForCreation.DeleteShareMapping(shadowCopySetId,
                    shadowCopy.serverShadowCopyId,
                    shadowCopy.shareName);
                BaseTestSite.Assert.AreEqual<FsrvpErrorCode>(FsrvpErrorCode.FSRVP_SUCCESS, (FsrvpErrorCode)ret,
                    "The return value of DeleteShareMapping is expected to be 0x{0:x8}. The server actually returns 0x{1:x8}.",
                    (int)FsrvpErrorCode.FSRVP_SUCCESS, ret);
            }
            shadowCopyList.Clear();
            #endregion
        }

        #endregion
    }
}
