// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Protocols.TestSuites.FileSharing.Common.Adapter;
using Microsoft.Protocols.TestTools;
using Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Smb2;
using Microsoft.Protocols.TestTools.StackSdk.Security.Sspi;
using System;

namespace Microsoft.Protocols.TestSuites.FileSharing.SMB2Model.Adapter.TreeMgmt
{
    public class TreeMgmtAdapter : ModelManagedAdapterBase, ITreeMgmtAdapter
    {
        public const uint INVALID_TREE_ID = 0xffffffff;

        #region Fields
        private Smb2FunctionalClient testClient;
        private TreeMgmtServerConfig treeMgmtConfig;

        private uint treeId;
        #endregion

        public event TreeConnectEventHandler TreeConnectResponse;
        public event TreeDisconnectEventHandler TreeDisconnectResponse;

        #region Initialization

        public override void Initialize(ITestSite testSite)
        {
            base.Initialize(testSite);
        }

        public override void Reset()
        {
            base.Reset();

            if (testClient != null)
            {
                testClient.Disconnect();
                testClient = null;
            }
        }

        #endregion


        #region Actions

        public void ReadConfig(out TreeMgmtServerConfig config)
        {
            treeMgmtConfig = new TreeMgmtServerConfig()
            {
                Platform = testConfig.Platform
            };
            config = treeMgmtConfig;

            Site.Log.Add(LogEntryKind.Debug, config.ToString());
        }

        public void SetupConnection(ModelSessionSecurityContext securityContext)
        {
            testClient = new Smb2FunctionalClient(testConfig.Timeout, testConfig, this.Site);
            testClient.ConnectToServer(testConfig.UnderlyingTransport, testConfig.SutComputerName, testConfig.SutIPAddress);

            // SMB2 Negotiate 
            // Model cases only test Dialect lower than 3.11
            DialectRevision[] dialects = 
                Smb2Utility.GetDialects(testConfig.MaxSmbVersionClientSupported < DialectRevision.Smb311 ? testConfig.MaxSmbVersionClientSupported : DialectRevision.Smb302);
            testClient.Negotiate(
                dialects,
                testConfig.IsSMB1NegotiateEnabled);

            // SMB2 SESSION SETUP
            AccountCredential account = null;
            switch (securityContext)
            {
                case ModelSessionSecurityContext.Admin:
                    account = testConfig.AccountCredential;
                    break;
                case ModelSessionSecurityContext.NonAdmin:
                    account = testConfig.NonAdminAccountCredential;
                    break;
                default:
                    throw new InvalidOperationException(securityContext + " is not supported.");
            }

            testClient.SessionSetup(
                testConfig.DefaultSecurityPackage,
                testConfig.SutComputerName,
                account,
                testConfig.UseServerGssToken);

            // reset TreeId
            this.treeId = 0;
        }

        public void TreeConnectRequest(ModelSharePath sharePath)
        {
            string treeConnectShare = null;
            switch (sharePath)
            {
                case ModelSharePath.InvalidSharePath:
                    // generate GUID for the name of share folder
                    treeConnectShare = Guid.NewGuid().ToString();
                    Site.Log.Add(
                        LogEntryKind.Debug,
                        "Generate Invalid Share Name {0}", treeConnectShare);
                    break;
                case ModelSharePath.SpecialSharePath:
                    treeConnectShare = testConfig.SpecialShare;
                    break;
                case ModelSharePath.ValidSharePath:
                    treeConnectShare = testConfig.BasicFileShare;
                    break;
                default:
                    throw new InvalidOperationException(sharePath + " is not supported.");
            }

            TREE_CONNECT_Response? treeConnectResponse = null;
       
            uint status = testClient.TreeConnect(
                string.Format("\\\\{0}\\{1}", testConfig.SutComputerName, treeConnectShare),
                out this.treeId,
                checker: (header, response)=>
                {
                    treeConnectResponse = response;
                });

            TreeConnectResponse((ModelSmb2Status)status, treeConnectResponse.Value.ShareType, treeMgmtConfig);
        }

        public void TreeDisconnectRequest(ModelTreeId treeId)
        {
            uint treeDisconnectTreeId;
            switch (treeId)
            {
                case ModelTreeId.InvalidTreeId:
                    treeDisconnectTreeId = INVALID_TREE_ID;
                    break;
                case ModelTreeId.ValidExistTreeId:
                    // If TreeConnect, use the returned TreeID.
                    // If not TreeConnect, use the default value 0.
                    // If TreeConnect and then TreeDisconnect (second TreeDisconnect), use the former returned TreeId.
                    treeDisconnectTreeId = this.treeId;
                    break;
                case ModelTreeId.ValidNotExistTreeId:
                    // Invalid TreeId
                    treeDisconnectTreeId = this.treeId + 1;
                    break;
                default:
                    throw new InvalidOperationException(treeId + " is not supported.");
            }

            uint status = testClient.TreeDisconnect(treeDisconnectTreeId, (header, response) => { });
            TreeDisconnectResponse((ModelSmb2Status)status);

            // not reset this.treeId. Keep it for second TreeDisconnect.
        }

        #endregion
    }
}
