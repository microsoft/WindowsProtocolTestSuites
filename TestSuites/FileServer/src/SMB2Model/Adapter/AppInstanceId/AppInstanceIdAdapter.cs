// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using Microsoft.Protocols.TestTools;
using Microsoft.Protocols.TestSuites.FileSharing.Common.Adapter;
using Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Smb2;
namespace Microsoft.Protocols.TestSuites.FileSharing.SMB2Model.Adapter.AppInstanceId
{
    public class AppInstanceIdAdapter: ModelManagedAdapterBase, IAppInstanceIdAdapter
    {
        private const string different = "_Different";
        #region Field
        private Smb2FunctionalClient prepareClient;
        private Guid connection_ClientGuid;
        private string fileName;
        private Guid appInstanceId;
        private Guid createGuid;
        private FILEID open_FileId;
        private uint treeConnect_TreeId;
        private Guid leaseKey;
        private ModelDialectRevision clientDialect; 

        #endregion

        #region Event
        public event OpenResponseEventHandler OpenResponse;
        #endregion

        #region Initialization & Reset
        public override void Initialize(ITestSite testSite)
        {
            base.Initialize(testSite);
        }

        public override void Reset()
        {
            if (prepareClient != null)
            {
                prepareClient.Disconnect();
                prepareClient = null;
            }

            base.Reset();
        }
        #endregion

        #region Action

        public void ReadConfig(out ModelDialectRevision dialectRevision)
        {
            // No need to run the case if SMB2_CREATE_APP_INSTANCE_ID is not supported.
            testConfig.CheckCreateContext(CreateContextTypeValue.SMB2_CREATE_APP_INSTANCE_ID);

            dialectRevision = ModelUtility.GetModelDialectRevision(testConfig.MaxSmbVersionSupported);
        }

        public void PrepareOpen(
            ModelDialectRevision dialect,
            AppInstanceIdType appInstanceIdType,
            CreateType createType)
        {
            // Connect, Negotiate, SessionSetup, TreeConnect
            prepareClient = new Smb2FunctionalClient(testConfig.Timeout, testConfig, this.Site);
            this.connection_ClientGuid = Guid.NewGuid();
            clientDialect = dialect;
            ConnectToShare(clientDialect, prepareClient, connection_ClientGuid, testConfig.BasicFileShare, out treeConnect_TreeId);

            // Create
            this.fileName = GetTestFileName(Smb2Utility.GetUncPath(testConfig.SutComputerName, testConfig.BasicFileShare));
            Smb2CreateContextResponse[] createContextResponse;

            prepareClient.Create(
                this.treeConnect_TreeId,
                this.fileName,
                CreateOptions_Values.FILE_NON_DIRECTORY_FILE,
                out this.open_FileId,
                out createContextResponse,
                createContexts: CreateContexts(appInstanceIdType, createType, true),
                shareAccess:ShareAccess_Values.NONE);

            if (createType == CreateType.CreateDurableThenDisconnect)
            {
                prepareClient.Disconnect();
            }
        }

        public void OpenRequest(
            ClientGuidType clientGuidType,
            PathNameType pathNameType,
            CreateType createType,
            ShareType shareType,
            AppInstanceIdType appInstanceIdType)
        {
            Smb2FunctionalClient testClient = new Smb2FunctionalClient(testConfig.Timeout, testConfig, this.Site);
            uint treeId;
            string share;
            switch (shareType)
            {
                case ShareType.SameShare:
                    share = testConfig.BasicFileShare;
                    break;
                case ShareType.DifferentShareSameLocal:
                    share = testConfig.SameWithSMBBasic;
                    break;
                case ShareType.DifferentShareDifferentLocal:
                    share = testConfig.DifferentFromSMBBasic;
                    break;
                default:
                    throw new ArgumentException("shareType");                    
            }

            ConnectToShare(
                clientDialect,
                testClient,
                clientGuidType == ClientGuidType.SameClientGuid ? this.connection_ClientGuid : Guid.NewGuid(),
                share,
                out treeId);

            FILEID fileId;
            Smb2CreateContextResponse[] createContextResponse;
            uint status;
            string fileNameInOpen;
            if (pathNameType == PathNameType.SamePathName)
            {
                fileNameInOpen = fileName;
            }
            else
            {
                fileNameInOpen = fileName + different;
                AddTestFileName(Smb2Utility.GetUncPath(testConfig.SutComputerName, share), fileNameInOpen);
            }

            status = testClient.Create(
                treeId,
                fileNameInOpen,
                CreateOptions_Values.FILE_NON_DIRECTORY_FILE,
                out fileId,
                out createContextResponse,
                createContexts: CreateContexts(appInstanceIdType, createType, false),
                shareAccess: ShareAccess_Values.NONE,
                checker: (header, response) => { });

            testClient.Disconnect();
            testClient = null;

            bool ifClosed = CheckIfOpenClosed(clientDialect, createType);
            this.OpenResponse(ifClosed ? OpenStatus.OpenClosed : OpenStatus.OpenNotClosed);
        }


        #endregion

        private bool CheckIfOpenClosed(ModelDialectRevision dialect, CreateType createType)
        {
            if (createType == CreateType.ReconnectDurable)
            {
                /// prepareClient is disconnected, so
                /// Reconnect to the share and try to get the open, if the durable open can be reconnected, then it's not closed.
                Smb2FunctionalClient reconnectClient = new Smb2FunctionalClient(testConfig.Timeout, testConfig, this.Site);
                uint treeId;
                ConnectToShare(
                    dialect, 
                    reconnectClient, 
                    this.connection_ClientGuid, 
                    testConfig.BasicFileShare, 
                    out treeId);
                FILEID fileId;
                Smb2CreateContextResponse[] createContextResponse;
                uint status = reconnectClient.Create(
                    treeId,
                    this.fileName,
                    CreateOptions_Values.FILE_NON_DIRECTORY_FILE,
                    out fileId,
                    out createContextResponse,
                    createContexts: CreateContexts(AppInstanceIdType.NoAppInstanceId, createType, false),
                    shareAccess:ShareAccess_Values.NONE,
                    checker: (header, response) => {});

                Site.Log.Add(LogEntryKind.Debug, "CheckIfOpenClosed: status of reconnectClient.Create is " + Smb2Status.GetStatusCode(status));

                reconnectClient.Close(treeId, fileId, (header, response) => { });
                reconnectClient.Disconnect();
                return !(status == Smb2Status.STATUS_SUCCESS);     
            }
            else
            {
                /// Write (using the FileID got from the create response of PrepareOpen) to check if the Open is closed.
                uint status = prepareClient.Write(this.treeConnect_TreeId, this.open_FileId, "AppInstanceId", checker: (header, response) => { });
                Site.Log.Add(LogEntryKind.Debug, "CheckIfOpenClosed: status of Write is " + Smb2Status.GetStatusCode(status));
                prepareClient.Close(this.treeConnect_TreeId, this.open_FileId, (header, response) => { });
                return status == Smb2Status.STATUS_FILE_CLOSED;
            }
        }

        private void ConnectToShare(ModelDialectRevision dialect, Smb2FunctionalClient client, Guid guid, string share, out uint treeId)
        {
            #region Connect to server
            client.ConnectToServer(testConfig.UnderlyingTransport, testConfig.SutComputerName, testConfig.SutIPAddress);
            #endregion

            client.Negotiate(
                Smb2Utility.GetDialects(ModelUtility.GetDialectRevision(dialect)),
                testConfig.IsSMB1NegotiateEnabled,
                clientGuid: guid);

            client.SessionSetup(
                testConfig.DefaultSecurityPackage,
                testConfig.SutComputerName,
                testConfig.AccountCredential,
                testConfig.UseServerGssToken);

            client.TreeConnect(Smb2Utility.GetUncPath(testConfig.SutComputerName, share), out treeId);
        }

        private Smb2CreateContextRequest[] CreateContexts(AppInstanceIdType appInstanceIdType, CreateType createType, bool prepare)
        {
            List<Smb2CreateContextRequest> contexts = new List<Smb2CreateContextRequest>();

            // Construct other context besides appInstanceId context.
            switch (createType)
            {
                case CreateType.CreateDurable:
                case CreateType.CreateDurableThenDisconnect:
                    this.createGuid = Guid.NewGuid();

                    testConfig.CheckCreateContext(CreateContextTypeValue.SMB2_CREATE_DURABLE_HANDLE_REQUEST_V2);

                    contexts.Add( 
                        new Smb2CreateDurableHandleRequestV2
                        {
                            CreateGuid = this.createGuid,
                        });

                    if (createType == CreateType.CreateDurableThenDisconnect)
                    {
                        testConfig.CheckCreateContext(CreateContextTypeValue.SMB2_CREATE_REQUEST_LEASE);

                        this.leaseKey = Guid.NewGuid();
                        contexts.Add(
                            new Smb2CreateRequestLease
                            {
                                LeaseKey = this.leaseKey,
                                LeaseState = LeaseStateValues.SMB2_LEASE_READ_CACHING | LeaseStateValues.SMB2_LEASE_HANDLE_CACHING | LeaseStateValues.SMB2_LEASE_WRITE_CACHING,
                            });
                    }
                    break;
              
                case CreateType.NoContext:
                    break;

                case CreateType.OtherContext:
                    testConfig.CheckCreateContext(CreateContextTypeValue.SMB2_CREATE_QUERY_ON_DISK_ID);

                    contexts.Add(new Smb2CreateQueryOnDiskId());
                    break;
                case CreateType.ReconnectDurable:
                    testConfig.CheckCreateContext(CreateContextTypeValue.SMB2_CREATE_DURABLE_HANDLE_RECONNECT_V2, CreateContextTypeValue.SMB2_CREATE_REQUEST_LEASE);

                    contexts.Add(
                        new Smb2CreateDurableHandleReconnectV2
                        {
                            CreateGuid = this.createGuid,
                            FileId = new FILEID { Persistent = this.open_FileId.Persistent },
                        });
                    contexts.Add(
                        new Smb2CreateRequestLease
                        {
                            LeaseKey = this.leaseKey,
                            LeaseState = LeaseStateValues.SMB2_LEASE_READ_CACHING | LeaseStateValues.SMB2_LEASE_HANDLE_CACHING | LeaseStateValues.SMB2_LEASE_WRITE_CACHING,
                        });

                    break;
                default:
                    throw new ArgumentException("createType");
            }

            Guid appInstanceGuid = default(Guid);
            switch (appInstanceIdType)
            {
                case AppInstanceIdType.NoAppInstanceId:
                    if (prepare)
                    {
                        Site.Assert.Fail("appInstanceIdType should not be None when preparing open.");
                    }
                    break;
                case AppInstanceIdType.AppInstanceIdIsZero:
                    appInstanceGuid = Guid.Empty;
                    break;
                case AppInstanceIdType.ValidAppInstanceId:
                    appInstanceGuid = prepare ? Guid.NewGuid() : this.appInstanceId;
                    break;
                case AppInstanceIdType.InvalidAppInstanceId:
                    if (prepare)
                    {
                        Site.Assert.Fail("appInstanceIdType should not be Invalid when preparing open.");
                    }                    
                    appInstanceGuid = Guid.NewGuid();
                    break;
                default:
                    throw new ArgumentException("appInstanceIdType");
            }

            if (prepare)
            {
                this.appInstanceId = appInstanceGuid;
            }

            if (appInstanceIdType != AppInstanceIdType.NoAppInstanceId)
            {
                contexts.Add(new Smb2CreateAppInstanceId { AppInstanceId = appInstanceGuid });
            }
            return contexts.ToArray();
        }
    }
}
