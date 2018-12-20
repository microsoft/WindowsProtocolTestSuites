// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.IO;
using Microsoft.Protocols.TestSuites.FileSharing.Common.Adapter;
using Microsoft.Protocols.TestTools;
using Microsoft.Protocols.TestTools.StackSdk;
using Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Smb2;
using Microsoft.Protocols.TestTools.StackSdk.Security.Sspi;

namespace Microsoft.Protocols.TestSuites.FileSharing.SMB2Model.Adapter.ResilientHandle
{
    public class ResilientHandleAdapter : ModelManagedAdapterBase, IResilientHandleAdapter
    {
        public const uint NETWORK_RESILIENCY_REQUEST_SIZE = 8;

        #region Fields
        private ResilientHandleServerConfig resilientHandleConfig;
        private Smb2FunctionalClient prepareOpenClient;
        private Smb2FunctionalClient reconnectClient;
        private uint treeId;
        private FILEID fileId;
        private string fileName;
        private Guid clientGuid;
        private Guid createGuid;
        private DialectRevision dialect;
        #endregion

        #region Initialization

        public override void Initialize(ITestSite testSite)
        {
            base.Initialize(testSite);
        }

        public override void Reset()
        {
            if (prepareOpenClient != null)
            {
                prepareOpenClient.Disconnect();
            }
            if (reconnectClient != null)
            {
                reconnectClient.Disconnect();
            }

            base.Reset();
        }

        #endregion

        #region Event

        public event IoCtlResiliencyResponseEventHandler IoCtlResiliencyResponse;

        public event ReEstablishResilientOpenResponseEventHandler ReEstablishResilientOpenResponse;


        #endregion

        #region Action

        public void ReadConfig(out ResilientHandleServerConfig config)
        {
            resilientHandleConfig = new ResilientHandleServerConfig
            {
                MaxSmbVersionSupported = ModelUtility.GetModelDialectRevision(testConfig.MaxSmbVersionSupported),
                IsIoCtlCodeResiliencySupported = testConfig.IsIoCtlCodeSupported(CtlCode_Values.FSCTL_LMR_REQUEST_RESILIENCY),
                Platform = testConfig.Platform >= Platform.WindowsServer2016 ? Platform.WindowsServer2016 : testConfig.Platform
            };
            config = resilientHandleConfig;

            prepareOpenClient = null;
            Site.Log.Add(LogEntryKind.Debug, resilientHandleConfig.ToString());

            // Resilient only applies only to servers that implement the SMB 2.1 or the SMB 3.x dialect family.
            testConfig.CheckDialect(DialectRevision.Smb21);
        }

        public void PrepareOpen(ModelDialectRevision clientMaxDialect, DurableHandle durableHandle)
        {
            prepareOpenClient = new Smb2FunctionalClient(testConfig.Timeout, testConfig, this.Site);
            clientGuid = Guid.NewGuid();
            DialectRevision[] dialects = Smb2Utility.GetDialects(ModelUtility.GetDialectRevision(clientMaxDialect));

            // Connect to Share
            ConnectToShare(
                Site,
                testConfig,
                prepareOpenClient,
                dialects,
                clientGuid,
                testConfig.AccountCredential,
                out dialect,
                out treeId);
            Site.Assert.AreEqual(
                ModelUtility.GetDialectRevision(clientMaxDialect),
                dialect,
                "DialectRevision {0} is expected", ModelUtility.GetDialectRevision(clientMaxDialect));

            // SMB2 Create
            RequestedOplockLevel_Values opLockLevel = RequestedOplockLevel_Values.OPLOCK_LEVEL_NONE;
            Smb2CreateContextRequest[] createContextRequests = new Smb2CreateContextRequest[0];
            createGuid = Guid.Empty;

            if (durableHandle == DurableHandle.DurableHandle)
            {// durable handle request context with batch opLock
                opLockLevel = RequestedOplockLevel_Values.OPLOCK_LEVEL_BATCH;
                createGuid = Guid.NewGuid();

                testConfig.CheckCreateContext(CreateContextTypeValue.SMB2_CREATE_DURABLE_HANDLE_REQUEST);

                createContextRequests = new Smb2CreateContextRequest[]
                {
                    new Smb2CreateDurableHandleRequest
                    {
                        DurableRequest = createGuid
                    }
                };
            }

            // create
            Smb2CreateContextResponse[] createContextResponse;
            prepareOpenClient.Create(
                treeId,
                GetTestFileName(Smb2Utility.GetUncPath(testConfig.SutComputerName, testConfig.BasicFileShare)),
                CreateOptions_Values.FILE_NON_DIRECTORY_FILE,
                out fileId,
                out createContextResponse,
                requestedOplockLevel_Values: opLockLevel,
                createContexts: createContextRequests,
                checker: (header, response) =>
                    {
                        Site.Assert.AreEqual(
                            Smb2Status.STATUS_SUCCESS,
                            header.Status,
                            "{0} should succeed", header.Command);

                        if (durableHandle == DurableHandle.DurableHandle)
                        {
                            Site.Assert.AreEqual<OplockLevel_Values>(
                                OplockLevel_Values.OPLOCK_LEVEL_BATCH,
                                response.OplockLevel,
                                "OplockLevel should be OPLOCK_LEVEL_BATCH if Durable Handle");
                        }
                    }
                );

            if (durableHandle == DurableHandle.DurableHandle)
            {
                // check whether response contain Durable Context
                Site.Assert.IsTrue(
                    ContainDurableHandleResponse(createContextResponse),
                    "Durable Handle Response should be in the Create response.");
            }
        }

        public void IoCtlResiliencyRequest(IoCtlInputCount inputCount, ResilientTimeout timeout)
        {
            uint timeoutValue = 0;
            switch (timeout)
            {
                case ResilientTimeout.InvalidTimeout:
                    // if the requested Timeout in seconds is greater than MaxResiliencyTimeout in seconds
                    timeoutValue = testConfig.MaxResiliencyTimeoutInSecond + 1;
                    break;
                case ResilientTimeout.ValidTimeout:
                    timeoutValue = testConfig.MaxResiliencyTimeoutInSecond;
                    break;
                case ResilientTimeout.ZeroTimeout:
                    timeoutValue = 0;
                    break;
                default:
                    throw new ArgumentException("timeout");
            }
            // convert seconds to milliseconds
            timeoutValue *= 1000;

            uint inputCountValue = 0;
            switch (inputCount)
            {
                case IoCtlInputCount.InputCountGreaterThanRequestSize:
                    inputCountValue = NETWORK_RESILIENCY_REQUEST_SIZE + 1;
                    break;
                case IoCtlInputCount.InputCountSmallerThanRequestSize:
                    inputCountValue = NETWORK_RESILIENCY_REQUEST_SIZE - 1;
                    break;
                case IoCtlInputCount.InputCountEqualToRequestSize:
                    inputCountValue = NETWORK_RESILIENCY_REQUEST_SIZE;
                    break;
                default:
                    throw new ArgumentException("inputCount");
            }

            Packet_Header ioCtlHeader;
            IOCTL_Response ioCtlResponse;
            byte[] inputInResponse;
            byte[] outputInResponse;
            uint status = prepareOpenClient.ResiliencyRequest(
                treeId,
                fileId,
                timeoutValue,
                inputCountValue,
                out ioCtlHeader,
                out ioCtlResponse,
                out inputInResponse,
                out outputInResponse,
                checker: (header, response) =>
                    {
                    // do nothing, skip the exception
                }
                );
            #region Verify IOCTL Response TD 3.3.5.15.9
            if ((NtStatus)status == NtStatus.STATUS_SUCCESS)
            {
                Site.Assert.AreEqual<CtlCode_Values>(
                    CtlCode_Values.FSCTL_LMR_REQUEST_RESILIENCY,
                    (CtlCode_Values)ioCtlResponse.CtlCode,
                    "CtlCode MUST be set to FSCTL_LMR_REQUEST_RESILIENCY.");

                Site.Assert.AreEqual<FILEID>(
                    fileId,
                    ioCtlResponse.FileId,
                    "FileId.Persistent MUST be set to Open.DurableFileId. FileId.Volatile MUST be set to Open.FileId.");

                if (testConfig.Platform != Platform.NonWindows)
                {// Windows
                    Site.Assert.IsTrue(
                        inputInResponse == null || inputInResponse.Length == 0,
                        "InputCount SHOULD be set to zero.");
                }

                Site.Assert.IsTrue(
                    outputInResponse == null || outputInResponse.Length == 0,
                    "OutputCount MUST be set to zero.");

                Site.Assert.AreEqual<uint>(
                    0,
                    (uint)ioCtlResponse.Flags,
                    "Flags MUST be set to zero.");
            }
            #endregion

            IoCtlResiliencyResponse((ModelSmb2Status)status, resilientHandleConfig);
        }

        public void LogOff()
        {
            prepareOpenClient.LogOff();
        }

        public void Disconnect()
        {
            prepareOpenClient.Disconnect();
        }

        public void ReEstablishResilientOpenRequest(ModelUser user)
        {
            reconnectClient = new Smb2FunctionalClient(testConfig.Timeout, testConfig, this.Site);

            AccountCredential account = testConfig.AccountCredential;
            if (user == ModelUser.DefaultUser)
            {
                account = testConfig.AccountCredential;
            }
            else if (user == ModelUser.DiffUser)
            {
                account = testConfig.NonAdminAccountCredential;
            }

            // Connect to Share
            ConnectToShare(
                Site,
                testConfig,
                reconnectClient,
                new DialectRevision[] { dialect },
                clientGuid,
                account,
                out dialect,
                out treeId);


            // Reconnect to the Open
            Smb2CreateContextResponse[] createContextResponse;

            testConfig.CheckCreateContext(CreateContextTypeValue.SMB2_CREATE_DURABLE_HANDLE_RECONNECT);

            uint status = reconnectClient.Create(
                treeId,
                fileName,
                CreateOptions_Values.NONE,
                out fileId,
                out createContextResponse,
                createContexts: new Smb2CreateContextRequest[]
                    {
                        new Smb2CreateDurableHandleReconnect()
                        {
                            Data = fileId
                        }
                    },
                checker: (header, response) =>
                    {
                    // do nothing, skip the exception 
                }
                );

            ReEstablishResilientOpenResponse((ModelSmb2Status)status);
        }

        #endregion

        #region Private methods

        private static void ConnectToShare(
            ITestSite testSite,
            SMB2ModelTestConfig testConfig,
            Smb2FunctionalClient client,
            DialectRevision[] dialects,
            Guid clientGuid,
            AccountCredential account,
            out DialectRevision responseDialect,
            out uint treeId)
        {
            client.ConnectToServer(testConfig.UnderlyingTransport, testConfig.SutComputerName, testConfig.SutIPAddress);

            // Negotiate
            NEGOTIATE_Response? negotiateResponse = null;
            client.Negotiate(
                dialects,
                testConfig.IsSMB1NegotiateEnabled,
                clientGuid: clientGuid,
                checker: (header, response) =>
                {
                    testSite.Assert.AreEqual(
                        Smb2Status.STATUS_SUCCESS,
                        header.Status,
                        "{0} should succeed", header.Command);

                    negotiateResponse = response;
                });

            responseDialect = negotiateResponse.Value.DialectRevision;

            // SMB2 SESSION SETUP
            client.SessionSetup(
                testConfig.DefaultSecurityPackage,
                testConfig.SutComputerName,
                account,
                testConfig.UseServerGssToken);

            // SMB2 Tree Connect
            client.TreeConnect(
                Smb2Utility.GetUncPath(testConfig.SutComputerName, testConfig.BasicFileShare),
                out treeId);
        }


        private static bool ContainDurableHandleResponse(Smb2CreateContextResponse[] createContextResponses)
        {
            if (createContextResponses != null)
            {
                foreach (Smb2CreateContextResponse createContextResponse in createContextResponses)
                {
                    Smb2CreateDurableHandleResponse durableHandleResponse = createContextResponse as Smb2CreateDurableHandleResponse;
                    if (durableHandleResponse != null)
                    {
                        return true;
                    }
                }
            }

            return false;
        }
        #endregion
    }
}
