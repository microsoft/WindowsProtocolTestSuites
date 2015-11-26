// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Net;
using System.Threading;
using Microsoft.Protocols.TestSuites.FileSharing.Common.Adapter;
using Microsoft.Protocols.TestSuites.FileSharing.Common.TestSuite;
using Microsoft.Protocols.TestSuites.FileSharing.SMB2.Adapter;
using Microsoft.Protocols.TestTools;
using Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Smb2;
using Microsoft.Protocols.TestTools.StackSdk.Security.Sspi;

namespace Microsoft.Protocols.TestSuites.FileSharing.SMB2.TestSuite
{
    public partial class SMB2TestBase
    {
        public enum ConnectShareType
        {
            /// <summary>
            /// Connect to the CAShare, need to assert the flag in TreeConnect response contains the continuous availability
            /// </summary>
            CAShare,

            /// <summary>
            /// Connect to the BasicShare, need to assert the flag in TreeConnect response does not contain the continuous availability
            /// </summary>
            BasicShare,

            /// <summary>
            /// Connect to BasicShare, no need to raise any assertion of the flag in TreeConnect response.
            /// </summary>
            BasicShareWithoutAssert
        }

        #region Variables
        protected string durableHandleUncSharePath;
        #endregion

        #region Methods
        /// <summary>
        /// Common method used to connect to target server, including the following message sequences:
        /// 1. Negotiate
        /// 2. Session Setup
        /// 3. Tree Connect
        /// </summary>
        /// <param name="smb2Dialect"></param>
        /// <param name="client"></param>
        /// <param name="clientGuid"></param>
        /// <param name="account"></param>
        /// <param name="connectShareType"></param>
        /// <param name="treeId"></param>
        /// <param name="clientBeforeDisconnection"></param>
        protected virtual void Connect(DialectRevision smb2Dialect, Smb2FunctionalClient client, Guid clientGuid, AccountCredential account, ConnectShareType connectShareType, out uint treeId, Smb2FunctionalClient clientBeforeDisconnection)
        {
            DialectRevision[] requestDialect = Smb2Utility.GetDialects(smb2Dialect);
            Capabilities_Values clientCapabilities = Capabilities_Values.GLOBAL_CAP_DFS | Capabilities_Values.GLOBAL_CAP_DIRECTORY_LEASING | Capabilities_Values.GLOBAL_CAP_LARGE_MTU | Capabilities_Values.GLOBAL_CAP_LEASING | Capabilities_Values.GLOBAL_CAP_PERSISTENT_HANDLES;
            SecurityMode_Values clientSecurityMode = SecurityMode_Values.NEGOTIATE_SIGNING_ENABLED;

            IPAddress targetIPAddress = (connectShareType == ConnectShareType.CAShare) ? testConfig.CAShareServerIP : testConfig.SutIPAddress;
            string targetServer = (connectShareType == ConnectShareType.CAShare) ? testConfig.CAShareServerName : testConfig.SutComputerName;

            client.ConnectToServer(TestConfig.UnderlyingTransport, targetServer, targetIPAddress);

            BaseTestSite.Log.Add(LogEntryKind.TestStep, "The client with clientGuid {0} sends NEGOTIATE request.", clientGuid);
            client.Negotiate(
                requestDialect,
                TestConfig.IsSMB1NegotiateEnabled,
                clientSecurityMode,
                clientCapabilities,
                clientGuid);

            if (null != clientBeforeDisconnection)
            {
                BaseTestSite.Log.Add(LogEntryKind.TestStep, "The client with clientGuid {0} sends SESSION_SETUP request to reconnect to the previous session.", clientGuid);
                client.ReconnectSessionSetup(
                    clientBeforeDisconnection,
                    testConfig.DefaultSecurityPackage,
                    targetServer,
                    account,
                    testConfig.UseServerGssToken);
            }
            else
            {
                BaseTestSite.Log.Add(LogEntryKind.TestStep, "The client with clientGuid {0} sends SESSION_SETUP request.", clientGuid);
                client.SessionSetup(
                    testConfig.DefaultSecurityPackage,
                    targetServer,
                    account,
                    testConfig.UseServerGssToken);
            }

            BaseTestSite.Log.Add(LogEntryKind.TestStep, "The client with clientGuid {0} sends TREE_CONNECT request.", clientGuid);
            client.TreeConnect(
                durableHandleUncSharePath,
                out treeId,
                checker: (header, response) =>
                {
                    BaseTestSite.Log.Add(
                        LogEntryKind.Debug,
                        "Capabilities in TREE_CONNECT response: {0}", response.Capabilities);

                    BaseTestSite.Assert.AreEqual(
                        Smb2Status.STATUS_SUCCESS,
                        header.Status,
                        "{0} should be successful", header.Command);

                    if (connectShareType == ConnectShareType.CAShare)
                    {
                        BaseTestSite.Assert.AreEqual(
                            Share_Capabilities_Values.SHARE_CAP_CONTINUOUS_AVAILABILITY,
                            Share_Capabilities_Values.SHARE_CAP_CONTINUOUS_AVAILABILITY & response.Capabilities,
                            "The share should have SMB2_SHARE_CAP_CONTINUOUS_AVAILABILITY capability");
                    }

                    if (connectShareType == ConnectShareType.BasicShare)
                    {
                        BaseTestSite.Assert.AreNotEqual(
                            Share_Capabilities_Values.SHARE_CAP_CONTINUOUS_AVAILABILITY,
                            Share_Capabilities_Values.SHARE_CAP_CONTINUOUS_AVAILABILITY & response.Capabilities,
                            "The share should not have SMB2_SHARE_CAP_CONTINUOUS_AVAILABILITY capability");
                    }
                });
        }
        #endregion
    }
}
