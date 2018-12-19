// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Protocols.TestSuites.FileSharing.Common.Adapter;
using Microsoft.Protocols.TestTools;
using Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Smb2;
using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Net;

namespace Microsoft.Protocols.TestSuites.FileSharing.SMB2Model.Adapter.Handle
{
    [SuppressMessage("Microsoft.Maintainability", "CA1506:AvoidExcessiveClassCoupling")]
    public class HandleAdapter : ModelManagedAdapterBase, IHandleAdapter
    {
        #region Fields

        private Smb2FunctionalClient testClientBeforeDisconnection;
        private Smb2FunctionalClient testClientAfterDisconnection;
        private HandleConfig handleConfig;

        private string sharePath;
        private string fileName;
        private Guid clientGuid;
        private Guid leaseKey;
        private uint treeIdBeforeDisconnection = 0; 
        private uint treeIdAfterDisconnection = 0;
        private Guid createGuid;
        private FILEID fileIdBeforDisconnection;

        private bool isCAShare = false;
        private OplockLeaseType requestedContext;

        private DialectRevision[] requestDialect = null;
        private Capabilities_Values clientCapabilities;
        #endregion

        #region Initialization

        public override void Initialize(ITestSite testSite)
        {
            base.Initialize(testSite);
        }

        public override void Reset()
        {
            if (testClientBeforeDisconnection != null)
            {
                testClientBeforeDisconnection.Disconnect();
            }

            if (testClientAfterDisconnection != null)
            {
                testClientAfterDisconnection.Disconnect();
            }

            fileIdBeforDisconnection = FILEID.Zero;

            // Add test file to collection, to delete it in Reset() of base;
            AddTestFileName(sharePath, fileName);

            base.Reset();
        }

        #endregion

        #region Events

        public event ResponseEventHandler OpenResponse;

        #endregion

        #region Actions

        public void ReadConfig(out HandleConfig c)
        {
            c = new HandleConfig
            {
                MaxSmbVersionSupported = ModelUtility.GetModelDialectRevision(testConfig.MaxSmbVersionSupported),
                Platform = testConfig.Platform >= Platform.WindowsServer2016 ? Platform.WindowsServer2012R2 : testConfig.Platform,
                IsPersistentHandleSupported = testConfig.IsPersistentHandlesSupported,
                IsLeasingSupported = testConfig.IsLeasingSupported,
                IsDirectoryLeasingSupported = testConfig.IsDirectoryLeasingSupported
            };

            handleConfig = c;

            Site.Log.Add(LogEntryKind.Debug, handleConfig.ToString());
        }

        public void OpenRequest(
            ModelDialectRevision clientMaxDialect,
            PersistentBitType persistentBit,
            CAShareType connectToCAShare,
            OplockLeaseType oplockLeaseType,
            DurableV1RequestContext durableV1RequestContext,
            DurableV2RequestContext durableV2RequestContext,
            DurableV1ReconnectContext durableV1ReconnectContext,
            DurableV2ReconnectContext durableV2ReconnectContext)
        {
            requestDialect = Smb2Utility.GetDialects(ModelUtility.GetDialectRevision(clientMaxDialect));
            clientCapabilities = Capabilities_Values.GLOBAL_CAP_DFS | Capabilities_Values.GLOBAL_CAP_DIRECTORY_LEASING |
                    Capabilities_Values.GLOBAL_CAP_LARGE_MTU | Capabilities_Values.GLOBAL_CAP_LEASING | Capabilities_Values.GLOBAL_CAP_MULTI_CHANNEL;
            if (persistentBit == PersistentBitType.PersistentBitSet)
            {
                clientCapabilities |= Capabilities_Values.GLOBAL_CAP_PERSISTENT_HANDLES;
            }

            clientGuid = Guid.NewGuid();
            requestedContext = oplockLeaseType;
            isCAShare = (connectToCAShare == CAShareType.CAShare);
            IPAddress targetIPAddress;
            string targetServer;

            #region Connect to Common Share or CA Share
            if (!isCAShare)
            {
                sharePath = Smb2Utility.GetUncPath(testConfig.SutComputerName, testConfig.BasicFileShare);
                fileName = "PrepareHandle_ConnectTo_CommonShareFile_" + Guid.NewGuid() + ".txt";
                targetIPAddress = testConfig.SutIPAddress;
                targetServer = testConfig.SutComputerName;
            }
            else
            {
                sharePath = Smb2Utility.GetUncPath(testConfig.CAShareServerName, testConfig.CAShareName);
                fileName = "PrepareHandle_ConnectTo_CAShareFile_" + Guid.NewGuid().ToString() + ".txt";
                targetIPAddress = testConfig.CAShareServerIP;
                targetServer = testConfig.CAShareServerName;
            }

            testClientBeforeDisconnection = new Smb2FunctionalClient(testConfig.Timeout, testConfig, this.Site);
            testClientBeforeDisconnection.CreditGoal = 20;
            testClientBeforeDisconnection.ConnectToServer(testConfig.UnderlyingTransport, targetServer, targetIPAddress);

            testClientBeforeDisconnection.Negotiate(
                requestDialect,
                testConfig.IsSMB1NegotiateEnabled,
                capabilityValue: clientCapabilities,
                clientGuid: clientGuid,
                checker: (header, response) =>
                {
                    if (Smb2Utility.IsSmb3xFamily(response.DialectRevision)
                        && handleConfig.IsPersistentHandleSupported
                        && persistentBit == PersistentBitType.PersistentBitSet)
                    {
                        Site.Assert.IsTrue(
                            response.Capabilities.HasFlag(NEGOTIATE_Response_Capabilities_Values.GLOBAL_CAP_PERSISTENT_HANDLES),
                            "The server MUST set SMB2_GLOBAL_CAP_PERSISTENT_HANDLES if Connection.Dialect belongs to the SMB 3.x dialect family, " +
                            "SMB2_GLOBAL_CAP_PERSISTENT_HANDLES is set in the Capabilities field of the request, and the server supports persistent handles. " +
                            "Actual capabilities are {0}", response.Capabilities);
                    }
                });

            testClientBeforeDisconnection.SessionSetup(
                    testConfig.DefaultSecurityPackage,
                    targetServer,
                    testConfig.AccountCredential,
                    testConfig.UseServerGssToken);

            testClientBeforeDisconnection.TreeConnect(sharePath, out treeIdBeforeDisconnection, delegate (Packet_Header responseHeader, TREE_CONNECT_Response response)
            {
                if (isCAShare)
                {
                    if (!response.Capabilities.HasFlag(Share_Capabilities_Values.SHARE_CAP_CONTINUOUS_AVAILABILITY))
                    {
                        // skip test case for CA share is invalid
                        Site.Assert.Inconclusive("This test case is applicable only when CA share is valid.");
                    }
                }
            });

            #endregion

            #region Construct Create Contexts
            Smb2CreateContextRequest[] smb2CreateContextRequest = GetOpenFileCreateContext(
                durableV1RequestContext,
                durableV2RequestContext,
                durableV1ReconnectContext,
                durableV2ReconnectContext,
                oplockLeaseType,
                false,
                false);
            #endregion

            #region Send Create request according to different context combination
            RequestedOplockLevel_Values requestedOplockLevel = RequestedOplockLevel_Values.OPLOCK_LEVEL_NONE;
            switch (oplockLeaseType)
            {
                case OplockLeaseType.NoOplockOrLease:
                    {
                        requestedOplockLevel = RequestedOplockLevel_Values.OPLOCK_LEVEL_NONE;
                    }
                    break;

                case OplockLeaseType.BatchOplock:
                    {
                        requestedOplockLevel = RequestedOplockLevel_Values.OPLOCK_LEVEL_BATCH;
                    }
                    break;

                case OplockLeaseType.LeaseV1:
                case OplockLeaseType.LeaseV2:
                    {
                        requestedOplockLevel = RequestedOplockLevel_Values.OPLOCK_LEVEL_LEASE;
                    }
                    break;
            }

            FILEID fileId;
            Smb2CreateContextResponse[] serverCreateContexts;
            uint status = OpenCreate(
                testClientBeforeDisconnection,
                treeIdBeforeDisconnection,
                fileName,
                out fileId,
                out serverCreateContexts,
                requestedOplockLevel,
                smb2CreateContextRequest);

            #endregion

            DurableHandleResponseContext durableHandleResponse;
            LeaseResponseContext leaseResponse;
            CheckResponseContexts(serverCreateContexts, out durableHandleResponse, out leaseResponse);
            OpenResponse((ModelSmb2Status)status, durableHandleResponse, leaseResponse, handleConfig);

            testClientBeforeDisconnection.TreeDisconnect(treeIdAfterDisconnection, (header, response) => { });
            testClientBeforeDisconnection.LogOff();
        }

        public void PrepareOpen(
            ModelDialectRevision clientMaxDialect,
            PersistentBitType persistentBit,
            CAShareType connectToCAShare,
            ModelHandleType modelHandleType,
            OplockLeaseType oplockLeaseType)
        {
            // Lease V2 cases only apply on the server implements SMB 3.x family.
            if (oplockLeaseType == OplockLeaseType.LeaseV2)
                testConfig.CheckDialect(DialectRevision.Smb30);

            // Lease V1 cases only apply on the server implements SMB 2.1 and 3.x family.
            if (oplockLeaseType == OplockLeaseType.LeaseV1)
                testConfig.CheckDialect(DialectRevision.Smb21);

            if ((oplockLeaseType == OplockLeaseType.LeaseV1 || oplockLeaseType == OplockLeaseType.LeaseV2)
                && !testConfig.IsLeasingSupported)
                Site.Assert.Inconclusive("Test case is applicable in servers that support leasing.");

            requestDialect = Smb2Utility.GetDialects(ModelUtility.GetDialectRevision(clientMaxDialect));
            clientCapabilities = Capabilities_Values.GLOBAL_CAP_DFS | Capabilities_Values.GLOBAL_CAP_DIRECTORY_LEASING |
                    Capabilities_Values.GLOBAL_CAP_LARGE_MTU | Capabilities_Values.GLOBAL_CAP_LEASING | Capabilities_Values.GLOBAL_CAP_MULTI_CHANNEL;
            if (persistentBit == PersistentBitType.PersistentBitSet)
            {
                clientCapabilities |= Capabilities_Values.GLOBAL_CAP_PERSISTENT_HANDLES;
            }

            clientGuid = Guid.NewGuid();
            requestedContext = oplockLeaseType;
            isCAShare = (connectToCAShare == CAShareType.CAShare);
            IPAddress targetIPAddress;
            string targetServer;

            #region Connect to Common Share or CA Share
            if (!isCAShare)
            {
                sharePath = Smb2Utility.GetUncPath(testConfig.SutComputerName, testConfig.BasicFileShare);
                fileName = "PrepareHandle_ConnectTo_CommonShareFile_" + Guid.NewGuid() + ".txt";
                targetIPAddress = testConfig.SutIPAddress;
                targetServer = testConfig.SutComputerName;
            }
            else
            {
                sharePath = Smb2Utility.GetUncPath(testConfig.CAShareServerName, testConfig.CAShareName);
                fileName = "PrepareHandle_ConnectTo_CAShareFile_" + Guid.NewGuid().ToString() + ".txt";
                targetIPAddress = testConfig.CAShareServerIP;
                targetServer = testConfig.CAShareServerName;
            }

            testClientBeforeDisconnection = new Smb2FunctionalClient(testConfig.Timeout, testConfig, this.Site);
            testClientBeforeDisconnection.CreditGoal = 20;
            testClientBeforeDisconnection.ConnectToServer(testConfig.UnderlyingTransport, targetServer, targetIPAddress);

            testClientBeforeDisconnection.Negotiate(
                requestDialect,
                testConfig.IsSMB1NegotiateEnabled,
                capabilityValue: clientCapabilities,
                clientGuid: clientGuid,
                checker: (header, response) =>
                {
                    if (Smb2Utility.IsSmb3xFamily(response.DialectRevision)
                        && handleConfig.IsPersistentHandleSupported
                        && persistentBit == PersistentBitType.PersistentBitSet)
                    {
                        Site.Assert.IsTrue(
                            response.Capabilities.HasFlag(NEGOTIATE_Response_Capabilities_Values.GLOBAL_CAP_PERSISTENT_HANDLES),
                            "The server MUST set SMB2_GLOBAL_CAP_PERSISTENT_HANDLES if Connection.Dialect belongs to the SMB 3.x dialect family, " +
                            "SMB2_GLOBAL_CAP_PERSISTENT_HANDLES is set in the Capabilities field of the request, and the server supports persistent handles. " +
                            "Actual capabilities are {0}", response.Capabilities);
                    }
                });

            testClientBeforeDisconnection.SessionSetup(
                    testConfig.DefaultSecurityPackage,
                    targetServer,
                    testConfig.AccountCredential,
                    testConfig.UseServerGssToken);

            testClientBeforeDisconnection.TreeConnect(sharePath, out treeIdBeforeDisconnection);

            #endregion

            #region Create operation according to the handle type and context
            Smb2CreateContextRequest[] prepareRequestContext = null;
            Smb2CreateContextResponse[] serverCreateContexts = null;
            RequestedOplockLevel_Values requestedOplockLevel = RequestedOplockLevel_Values.OPLOCK_LEVEL_NONE;

            switch (oplockLeaseType)
            {
                case OplockLeaseType.LeaseV1:
                    {
                        testConfig.CheckCreateContext(CreateContextTypeValue.SMB2_CREATE_REQUEST_LEASE);

                        prepareRequestContext = GetPrepareOpenCreateContext(modelHandleType, oplockLeaseType);
                        requestedOplockLevel = RequestedOplockLevel_Values.OPLOCK_LEVEL_LEASE;
                    }
                    break;
                case OplockLeaseType.LeaseV2:
                    {
                        testConfig.CheckCreateContext(CreateContextTypeValue.SMB2_CREATE_REQUEST_LEASE_V2);

                        prepareRequestContext = GetPrepareOpenCreateContext(modelHandleType, oplockLeaseType);
                        requestedOplockLevel = RequestedOplockLevel_Values.OPLOCK_LEVEL_LEASE;
                    }
                    break;

                case OplockLeaseType.BatchOplock:
                    {
                        prepareRequestContext = GetPrepareOpenHandleContext(modelHandleType);
                        requestedOplockLevel = RequestedOplockLevel_Values.OPLOCK_LEVEL_BATCH;
                    }
                    break;

                case OplockLeaseType.NoOplockOrLease:
                    {
                        prepareRequestContext = GetPrepareOpenHandleContext(modelHandleType);
                        requestedOplockLevel = RequestedOplockLevel_Values.OPLOCK_LEVEL_NONE;
                    }
                    break;
            }

            PrepareOpenCreate(
                testClientBeforeDisconnection,
                treeIdBeforeDisconnection,
                fileName,
                out fileIdBeforDisconnection,
                out serverCreateContexts,
                requestedOplockLevel,
                prepareRequestContext);

            #endregion
        }

        public void LogOff()
        {
            testClientBeforeDisconnection.LogOff();
        }

        public void Disconnect()
        {
            Site.Log.Add(
                LogEntryKind.Debug,
                "Client Disconnect.");
            testClientBeforeDisconnection.Disconnect();
        }

        public void ReconnectOpenRequest(
            DurableV1ReconnectContext durableV1ReconnectContext,
            DurableV2ReconnectContext durableV2ReconnectContext,
            OplockLeaseType oplockLeaseType,
            LeaseKeyDifferentialType leaseKeyDifferentialType,
            ClientIdType clientIdType,
            CreateGuidType createGuidType)
        {
            if ((oplockLeaseType == OplockLeaseType.LeaseV1 || oplockLeaseType == OplockLeaseType.LeaseV2)
                && !testConfig.IsLeasingSupported)
                Site.Assert.Inconclusive("Test case is applicable in servers that support leasing.");

            bool isSameLeaseKey = (leaseKeyDifferentialType == LeaseKeyDifferentialType.SameLeaseKey);
            bool isSameClient = (clientIdType == ClientIdType.SameClient);
            bool isSameCreateGuid = (createGuidType == CreateGuidType.SameCreateGuid);

            FILEID fileIdAfterDisconnection;
            Smb2CreateContextResponse[] serverCreateContexts;
            IPAddress targetIPAddress;
            string targetServer;
            string targetShare;

            #region Construct Create Contexts
            Smb2CreateContextRequest[] smb2CreateContextRequest = GetOpenFileCreateContext(
                DurableV1RequestContext.DurableV1RequestContextNotExist,
                DurableV2RequestContext.DurableV2RequestContextNotExist,
                durableV1ReconnectContext,
                durableV2ReconnectContext,
                oplockLeaseType,
                isSameLeaseKey,
                isSameCreateGuid);
            #endregion

            #region Client reconnect to server

            Site.Log.Add(LogEntryKind.Debug, "Client reconnect to server");

            #region Reconnect to Common Share or CA Share
            if (!isCAShare)
            {
                targetIPAddress = testConfig.SutIPAddress;
                targetServer = testConfig.SutComputerName;
                targetShare = testConfig.BasicFileShare;
            }
            else
            {
                targetIPAddress = testConfig.CAShareServerIP;
                targetServer = testConfig.CAShareServerName;
                targetShare = testConfig.CAShareName;
            }

            // Connect to Server
            testClientAfterDisconnection = new Smb2FunctionalClient(testConfig.Timeout, testConfig, this.Site);
            testClientAfterDisconnection.CreditGoal = 10;
            testClientAfterDisconnection.ConnectToServer(testConfig.UnderlyingTransport, targetServer, targetIPAddress);

            // Negotiate
            testClientAfterDisconnection.Negotiate(
                requestDialect,
                testConfig.IsSMB1NegotiateEnabled,
                capabilityValue: clientCapabilities,
                // If the reconnect use the same client guid, then keep client guid the same value, otherwise use a new client guid.
                clientGuid: (isSameClient ? clientGuid : Guid.NewGuid()));

            uint status = testClientAfterDisconnection.SessionSetup(
                testConfig.DefaultSecurityPackage,
                targetServer,
                testConfig.AccountCredential,
                testConfig.UseServerGssToken);
            Site.Assert.AreEqual(Smb2Status.STATUS_SUCCESS, status, "Reconnect Session Setup should be successful, actual status is {0}", Smb2Status.GetStatusCode(status));

            // TreeConnect
            testClientAfterDisconnection.TreeConnect(sharePath, out treeIdAfterDisconnection);

            #endregion

            #region Send Create request according to different context combination

            RequestedOplockLevel_Values requestedOplockLevel = RequestedOplockLevel_Values.OPLOCK_LEVEL_NONE;
            switch (oplockLeaseType)
            {
                case OplockLeaseType.NoOplockOrLease:
                    {
                        requestedOplockLevel = RequestedOplockLevel_Values.OPLOCK_LEVEL_NONE;
                    }
                    break;

                case OplockLeaseType.BatchOplock:
                    {
                        requestedOplockLevel = RequestedOplockLevel_Values.OPLOCK_LEVEL_BATCH;
                    }
                    break;

                case OplockLeaseType.LeaseV1:
                case OplockLeaseType.LeaseV2:
                    {
                        requestedOplockLevel = RequestedOplockLevel_Values.OPLOCK_LEVEL_LEASE;
                    }
                    break;
            }

            status = OpenCreate(
                testClientAfterDisconnection,
                treeIdAfterDisconnection,
                fileName,
                out fileIdAfterDisconnection,
                out serverCreateContexts,
                requestedOplockLevel,
                smb2CreateContextRequest);

            #endregion

            DurableHandleResponseContext durableHandleResponse;
            LeaseResponseContext leaseResponse;
            CheckResponseContexts(serverCreateContexts, out durableHandleResponse, out leaseResponse);
            OpenResponse((ModelSmb2Status)status, durableHandleResponse, leaseResponse, handleConfig);

            testClientAfterDisconnection.TreeDisconnect(treeIdAfterDisconnection);
            testClientAfterDisconnection.LogOff();

            #endregion
        }

        #endregion

        #region Private Methods
        /// <summary>
        ///  Get lease context for PrepareOpen
        /// </summary>
        private Smb2CreateContextRequest[] GetPrepareOpenLeaseContext(
            OplockLeaseType oplockLeaseType)
        {
            leaseKey = Guid.NewGuid();
            return GetLeaseContext(oplockLeaseType, leaseKey, GetLeaseState());
        }

        /// <summary>
        /// Get handle context for PrepareOpen
        /// </summary>
        private Smb2CreateContextRequest[] GetPrepareOpenHandleContext(
            ModelHandleType modelHandleType)
        {
            Smb2CreateContextRequest[] handleContext = new Smb2CreateContextRequest[] { };
            switch (modelHandleType)
            {
                case ModelHandleType.DurableHandleV1:
                    {
                        testConfig.CheckCreateContext(CreateContextTypeValue.SMB2_CREATE_DURABLE_HANDLE_REQUEST);

                        createGuid = Guid.Empty;
                        handleContext = handleContext.Append(new Smb2CreateDurableHandleRequest { DurableRequest = createGuid });
                        return handleContext;
                    }
                case ModelHandleType.DurableHandleV2:
                    {
                        testConfig.CheckCreateContext(CreateContextTypeValue.SMB2_CREATE_DURABLE_HANDLE_REQUEST_V2);

                        createGuid = Guid.NewGuid();
                        handleContext = handleContext.Append(new Smb2CreateDurableHandleRequestV2 { CreateGuid = createGuid });
                        return handleContext;
                    }
                case ModelHandleType.PersistentHandle:
                    {
                        testConfig.CheckCreateContext(CreateContextTypeValue.SMB2_CREATE_DURABLE_HANDLE_REQUEST_V2);

                        createGuid = Guid.NewGuid();
                        handleContext = handleContext.Append(
                            new Smb2CreateDurableHandleRequestV2
                            {
                                CreateGuid = createGuid,
                                Flags = CREATE_DURABLE_HANDLE_REQUEST_V2_Flags.DHANDLE_FLAG_PERSISTENT,
                            });
                        return handleContext;
                    }
                default:
                    throw new InvalidOperationException("Unsupported handle type");
            }
        }

        /// <summary>
        /// Get create context for PrepareOpen
        /// </summary>
        private Smb2CreateContextRequest[] GetPrepareOpenCreateContext(
            ModelHandleType modelHandleType,
            OplockLeaseType oplockLeaseType)
        {
            Smb2CreateContextRequest[] contexts = new Smb2CreateContextRequest[] { };
            contexts = GetPrepareOpenHandleContext(modelHandleType);
            contexts = ConcatContexts(contexts, GetPrepareOpenLeaseContext(oplockLeaseType));

            return contexts;
        }

        /// <summary>
        /// Create operation for PrepareOpen operation
        /// </summary>
        private void PrepareOpenCreate(
            Smb2FunctionalClient client,
            uint treeIdBeforeDisconnection,
            string fileName,
            out FILEID fileIdBeforDisconnection,
            out Smb2CreateContextResponse[] serverCreateContexts,
            RequestedOplockLevel_Values requestedOplocklevel,
            Smb2CreateContextRequest[] prepareContext)
        {
            client.Create(
                treeIdBeforeDisconnection,
                fileName,
                CreateOptions_Values.FILE_NON_DIRECTORY_FILE,
                out fileIdBeforDisconnection,
                out serverCreateContexts,
                requestedOplocklevel,
                prepareContext,
                shareAccess: ShareAccess_Values.NONE,
                checker: (HASH_HEADER, response) =>
                {
                });
        }


        /// <summary>
        /// Get lease context for OpenFile
        /// </summary>
        private Smb2CreateContextRequest[] GetOpenFileLeaseContext(
            OplockLeaseType oplockLeaseType,
            bool isSameLeaseKey)
        {
            Guid openFileLeaseKey;
            LeaseStateValues openFileLeaseState;

            openFileLeaseKey = isSameLeaseKey ? leaseKey : Guid.NewGuid();
            openFileLeaseState = GetLeaseState();

            return GetLeaseContext(oplockLeaseType, openFileLeaseKey, openFileLeaseState);
        }

        /// <summary>
        /// Get create context for OpenFile
        /// </summary>
        private Smb2CreateContextRequest[] GetOpenFileHandleContext(
            DurableV1RequestContext durableV1RequestContext,
            DurableV2RequestContext durableV2RequestContext,
            DurableV1ReconnectContext durableV1ReconnectContext,
            DurableV2ReconnectContext durableV2ReconnectContext,
            bool isSameCreateGuid)
        {
            Smb2CreateContextRequest[] handleContext = new Smb2CreateContextRequest[] { };

            if (durableV1RequestContext == DurableV1RequestContext.DurableV1RequestContextExist)
            {
                testConfig.CheckCreateContext(CreateContextTypeValue.SMB2_CREATE_DURABLE_HANDLE_REQUEST);

                handleContext = handleContext.Append(
                    new Smb2CreateDurableHandleRequest
                    {
                        DurableRequest = isSameCreateGuid ? Guid.Empty : Guid.NewGuid(),
                    });
            }
            if (durableV2RequestContext != DurableV2RequestContext.DurableV2RequestContextNotExist)
            {
                testConfig.CheckCreateContext(CreateContextTypeValue.SMB2_CREATE_DURABLE_HANDLE_REQUEST_V2);

                if (durableV2RequestContext == DurableV2RequestContext.DurableV2RequestContextExistWithoutPersistent)
                {
                    handleContext = handleContext.Append(
                        new Smb2CreateDurableHandleRequestV2
                        {
                            CreateGuid = isSameCreateGuid ? createGuid : Guid.NewGuid(),
                        });
                }
                else
                {
                    handleContext = handleContext.Append(
                        new Smb2CreateDurableHandleRequestV2
                        {
                            CreateGuid = isSameCreateGuid ? createGuid : Guid.NewGuid(),
                            Flags = CREATE_DURABLE_HANDLE_REQUEST_V2_Flags.DHANDLE_FLAG_PERSISTENT,
                        });
                }
            }
            if (durableV1ReconnectContext == DurableV1ReconnectContext.DurableV1ReconnectContextExist)
            {
                testConfig.CheckCreateContext(CreateContextTypeValue.SMB2_CREATE_DURABLE_HANDLE_RECONNECT);

                handleContext = handleContext.Append(
                    new Smb2CreateDurableHandleReconnect
                    {
                        Data = new FILEID { Persistent = fileIdBeforDisconnection.Persistent }
                    });
            }
            if (durableV2ReconnectContext != DurableV2ReconnectContext.DurableV2ReconnectContextNotExist)
            {
                testConfig.CheckCreateContext(CreateContextTypeValue.SMB2_CREATE_DURABLE_HANDLE_RECONNECT_V2);

                if (durableV2ReconnectContext == DurableV2ReconnectContext.DurableV2ReconnectContextExistWithoutPersistent)
                {
                    handleContext = handleContext.Append(
                        new Smb2CreateDurableHandleReconnectV2
                        {
                            CreateGuid = isSameCreateGuid ? createGuid : Guid.NewGuid(),
                            FileId = new FILEID { Persistent = fileIdBeforDisconnection.Persistent }
                        });
                }
                else
                {
                    handleContext = handleContext.Append(
                        new Smb2CreateDurableHandleReconnectV2
                        {
                            CreateGuid = isSameCreateGuid ? createGuid : Guid.NewGuid(),
                            Flags = CREATE_DURABLE_HANDLE_RECONNECT_V2_Flags.DHANDLE_FLAG_PERSISTENT,
                            FileId = new FILEID { Persistent = fileIdBeforDisconnection.Persistent }
                        });
                }
            }

            return handleContext;
        }

        /// <summary>
        /// Get handle context for OpenFile
        /// </summary>
        private Smb2CreateContextRequest[] GetOpenFileCreateContext(
            DurableV1RequestContext durableV1RequestContext,
            DurableV2RequestContext durableV2RequestContext,
            DurableV1ReconnectContext durableV1ReconnectContext,
            DurableV2ReconnectContext durableV2ReconnectContext,
            OplockLeaseType oplockLeaseType,
            bool isSameLeaseKey,
            bool isSameCreateGuid)
        {
            Smb2CreateContextRequest[] contexts = new Smb2CreateContextRequest[] { };
            contexts = GetOpenFileHandleContext(
                            durableV1RequestContext,
                            durableV2RequestContext,
                            durableV1ReconnectContext,
                            durableV2ReconnectContext,
                            isSameCreateGuid);

            contexts = ConcatContexts(contexts, GetOpenFileLeaseContext(
                oplockLeaseType,
                isSameLeaseKey));

            return contexts;
        }

        /// <summary>
        /// Create operation for Open operation
        /// </summary>
        private uint OpenCreate(
            Smb2FunctionalClient client,
            uint treeIdAfterDisconnection,
            string fileName,
            out FILEID fileIdAfterDisconnection,
            out Smb2CreateContextResponse[] serverCreateContexts,
            RequestedOplockLevel_Values requestedOplocklevel,
            Smb2CreateContextRequest[] openContext)
        {
            return client.Create(
                treeIdAfterDisconnection,
                fileName,
                CreateOptions_Values.FILE_NON_DIRECTORY_FILE,
                out fileIdAfterDisconnection,
                out serverCreateContexts,
                requestedOplocklevel,
                openContext,
                shareAccess: ShareAccess_Values.NONE,
                checker: (header, response) =>
                {
                });
        }

        /// <summary>
        /// Concat two contexts arrays
        /// </summary>
        private Smb2CreateContextRequest[] ConcatContexts(
            Smb2CreateContextRequest[] firstContext,
            Smb2CreateContextRequest[] secondContext)
        {
            foreach (var context in secondContext)
            {
                firstContext = firstContext.Append(context);
            }

            return firstContext;
        }

        /// <summary>
        /// Get LeaseState
        /// Use Handle Caching lease state in Model.
        /// </summary>
        private LeaseStateValues GetLeaseState()
        {
            return LeaseStateValues.SMB2_LEASE_READ_CACHING | LeaseStateValues.SMB2_LEASE_HANDLE_CACHING;
        }

        /// <summary>
        /// Get lease context according to leasekey and leasestate
        /// </summary>
        private Smb2CreateContextRequest[] GetLeaseContext(
            OplockLeaseType oplockLeaseType,
            Guid leaseKey,
            LeaseStateValues leaseState)
        {
            Smb2CreateContextRequest[] leaseContext = new Smb2CreateContextRequest[] { };

            if (oplockLeaseType == OplockLeaseType.LeaseV1)
            {
                testConfig.CheckCreateContext(CreateContextTypeValue.SMB2_CREATE_REQUEST_LEASE);

                leaseContext = leaseContext.Append(
                    new Smb2CreateRequestLease
                    {
                        LeaseKey = leaseKey,
                        LeaseState = leaseState
                    });
            }

            if (oplockLeaseType == OplockLeaseType.LeaseV2)
            {
                testConfig.CheckCreateContext(CreateContextTypeValue.SMB2_CREATE_REQUEST_LEASE_V2);

                leaseContext = leaseContext.Append(
                    new Smb2CreateRequestLeaseV2
                    {
                        LeaseKey = leaseKey,
                        LeaseState = leaseState
                    });
            }

            return leaseContext;
        }

        /// <summary>
        /// Check server response. Return the abstracted context type of response.
        /// </summary>
        private void CheckResponseContexts(
            Smb2CreateContextResponse[] serverCreateContexts,
            out DurableHandleResponseContext durableHandleResponseContext,
            out LeaseResponseContext leaseResponseContext)
        {
            durableHandleResponseContext = DurableHandleResponseContext.NONE;
            leaseResponseContext = LeaseResponseContext.NONE;

            if (serverCreateContexts != null)
            {
                foreach (Smb2CreateContextResponse response in serverCreateContexts)
                {
                    if (response is Smb2CreateDurableHandleResponse)
                    {
                        durableHandleResponseContext = DurableHandleResponseContext.SMB2_CREATE_DURABLE_HANDLE_RESPONSE;
                    }
                    else if (response is Smb2CreateDurableHandleResponseV2)
                    {
                        durableHandleResponseContext = DurableHandleResponseContext.SMB2_CREATE_DURABLE_HANDLE_RESPONSE_V2;
                        if ((response as Smb2CreateDurableHandleResponseV2).Flags.HasFlag(CREATE_DURABLE_HANDLE_RESPONSE_V2_Flags.DHANDLE_FLAG_PERSISTENT))
                            durableHandleResponseContext = DurableHandleResponseContext.SMB2_CREATE_DURABLE_HANDLE_RESPONSE_V2_WITH_PERSISTENT;
                    }
                    else if (response is Smb2CreateResponseLease)
                    {
                        leaseResponseContext = LeaseResponseContext.SMB2_CREATE_RESPONSE_LEASE;
                    }
                    else if (response is Smb2CreateResponseLeaseV2)
                    {
                        leaseResponseContext = LeaseResponseContext.SMB2_CREATE_RESPONSE_LEASE_V2;
                    }
                }
            }
        }
        #endregion
    }
}
