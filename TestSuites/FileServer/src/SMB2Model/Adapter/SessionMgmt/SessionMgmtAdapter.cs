// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Protocols.TestSuites.FileSharing.Common.Adapter;
using Microsoft.Protocols.TestTools;
using Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Smb2;
using Microsoft.Protocols.TestTools.StackSdk.Security.Sspi;
using System;
using System.Collections.Generic;

namespace Microsoft.Protocols.TestSuites.FileSharing.SMB2Model.Adapter.SessionMgmt
{
    public class SessionMgmtAdapter : ModelManagedAdapterBase, ISessionMgmtAdapter
    {
        #region Fields

        private Dictionary<ModelConnectionId, Smb2FunctionalClient> connectionList;
        private Dictionary<ModelSessionId, ulong> sessionTable;
        private SessionMgmtConfig sessionMgmtConfig;
        private Guid clientGuid;
        #endregion

        #region Initialization

        public override void Initialize(ITestSite testSite)
        {
            base.Initialize(testSite);
        }

        public override void Reset()
        {
            try
            {
                foreach (KeyValuePair<ModelConnectionId, Smb2FunctionalClient> pair in connectionList)
                {
                    pair.Value.LogOff();
                    pair.Value.Disconnect();
                }
            }
            catch
            {
            }

            connectionList?.Clear();
            sessionTable?.Clear();
            base.Reset();
        }

        #endregion

        #region Events

        public event SessionSetupEventHandler SessionSetupResponse;
        public event LogOffEventHandler LogOffResponse;
        public event DisconnectionEventHandler ExpectDisconnect;

        #endregion

        #region Actions

        private Guid GetClientGuid(ModelConnectionId connectionId, DialectRevision[] dialects)
        {
            if (dialects.Length == 1 && dialects[0] == DialectRevision.Smb2002)
            {
                return Guid.Empty;
            }
            else if (connectionId == ModelConnectionId.MainConnection)
            {
                this.clientGuid = Guid.NewGuid();
                return this.clientGuid;
            }
            else
            {
                return this.clientGuid;
            }
        }

        public void ReadConfig(out SessionMgmtConfig c)
        {
            // TODO: some of these cases could pass when signing is not supported, should enable them in the future.
            testConfig.CheckSigning();

            connectionList = new Dictionary<ModelConnectionId, Smb2FunctionalClient>();
            sessionTable = new Dictionary<ModelSessionId, ulong>();

            // Adding reserved sessionId to the table for better assignment later
            sessionTable.Add(ModelSessionId.ZeroSessionId, 0);
            sessionTable.Add(ModelSessionId.InvalidSessionId, 0xFFFFFFFFFFFFFFFF);

            PrintCurrentSessionTable("Before Test Start");

            c = new SessionMgmtConfig
            {
                MaxSmbVersionSupported = ModelUtility.GetModelDialectRevision(testConfig.MaxSmbVersionSupported),
                IsMultiChannelCapable = testConfig.IsMultiChannelCapable,
                Platform = testConfig.Platform >= Platform.WindowsServer2016 ? Platform.WindowsServer2012R2 : testConfig.Platform
            };

            sessionMgmtConfig = c;
            Site.Log.Add(LogEntryKind.Debug, c.ToString());
        }

        public void SetupConnection(ModelConnectionId connectionId, ModelDialectRevision clientMaxDialect)
        {
            connectionList.Add(connectionId, new Smb2FunctionalClient(testConfig.Timeout, testConfig, this.Site));

            if (connectionId == ModelConnectionId.MainConnection)
            {
                connectionList[connectionId].ConnectToServer(testConfig.UnderlyingTransport, testConfig.SutComputerName, testConfig.SutIPAddress, testConfig.ClientNic1IPAddress);
            }
            else
            {
                connectionList[connectionId].ConnectToServer(testConfig.UnderlyingTransport, testConfig.SutComputerName, testConfig.SutIPAddress, testConfig.ClientNic2IPAddress);
            }

            DialectRevision[] dialects = Smb2Utility.GetDialects(ModelUtility.GetDialectRevision(clientMaxDialect));

            uint status;
            NEGOTIATE_Response? negotiateResponse = null;
            status = connectionList[connectionId].Negotiate(
                dialects,
                testConfig.IsSMB1NegotiateEnabled,
                capabilityValue: Capabilities_Values.GLOBAL_CAP_MULTI_CHANNEL,
                clientGuid: GetClientGuid(connectionId, dialects),
                checker: (header, response) =>
                {
                    Site.Assert.AreEqual(
                        Smb2Status.STATUS_SUCCESS,
                        header.Status,
                        "{0} should succeed", header.Command);

                    negotiateResponse = response;
                });           

            DialectRevision expectedDialect;
            if (clientMaxDialect < sessionMgmtConfig.MaxSmbVersionSupported)
            {
                expectedDialect = ModelUtility.GetDialectRevision(clientMaxDialect);
            }
            else
            {
                expectedDialect = ModelUtility.GetDialectRevision(sessionMgmtConfig.MaxSmbVersionSupported);
            }

            Site.Assert.AreEqual(
                expectedDialect,
                negotiateResponse.Value.DialectRevision,
                "DialectRevision {0} is expected", expectedDialect);

            if (ModelUtility.IsSmb3xFamily(negotiateResponse.Value.DialectRevision) && sessionMgmtConfig.IsMultiChannelCapable)
            {
                // SMB2_GLOBAL_CAP_MULTI_CHANNEL if Connection.Dialect belongs to the SMB 3.x dialect family,
                // IsMultiChannelCapable is TRUE, and SMB2_GLOBAL_CAP_MULTI_CHANNEL is set in the Capabilities field of the request.
                Site.Assert.AreEqual(
                    Capabilities_Values.GLOBAL_CAP_MULTI_CHANNEL,
                    Capabilities_Values.GLOBAL_CAP_MULTI_CHANNEL & (Capabilities_Values)negotiateResponse.Value.Capabilities,
                    "");
            }
            else
            {
                Site.Assert.AreNotEqual(
                    Capabilities_Values.GLOBAL_CAP_MULTI_CHANNEL,
                    Capabilities_Values.GLOBAL_CAP_MULTI_CHANNEL & (Capabilities_Values)negotiateResponse.Value.Capabilities,
                    "");
            }

        }

        public void SessionSetupRequest(
            ModelConnectionId connectionId,
            ModelSessionId sessionId,
            ModelSessionId previousSessionId,
            ModelSigned signed,
            ModelFlags flags,
            ModelUser user,
            ModelAllowReauthentication ReAuthentication)
        {
            ulong adapterSessionId;
            ulong adapterPreviousSessionId;
            Packet_Header_Flags_Values headerFlags;
            SESSION_SETUP_Request_Flags sessionSetupFlags;
            AccountCredential credential;

            #region sessionId
            if (sessionTable.ContainsKey(sessionId))
            {
                adapterSessionId = sessionTable[sessionId];

                // For sessionId is 0 which indicates session creation
                // assign a new one
                if (sessionId == ModelSessionId.ZeroSessionId)
                {
                    if (!sessionTable.ContainsKey(ModelSessionId.MainSessionId))
                    {
                        sessionId = ModelSessionId.MainSessionId;
                    }
                    else if (!sessionTable.ContainsKey(ModelSessionId.AlternativeSessionId))
                    {
                        sessionId = ModelSessionId.AlternativeSessionId;
                    }
                }
            }
            else
            {
                Random r = new Random();
                adapterSessionId = (ulong)r.Next(1, int.MaxValue);
            }

            Site.Log.Add(
                LogEntryKind.Debug,
                "ModelSessionId: {0}, AdapterSessionId: 0x{1:x8}",
                sessionId, adapterSessionId);

            #endregion

            #region previousSessionId
            if (sessionTable.ContainsKey(previousSessionId))
            {
                adapterPreviousSessionId = sessionTable[previousSessionId];
            }
            else
            {
                Random r = new Random();
                adapterPreviousSessionId = (ulong)r.Next(1, int.MaxValue);
            }

            Site.Log.Add(
                LogEntryKind.Debug,
                "ModelSessionId: {0}, adapterPreviousSessionId: 0x{1:x8}",
                sessionId, adapterPreviousSessionId);

            #endregion

            #region isSigned

            headerFlags = (signed == ModelSigned.SignFlagSet) ? Packet_Header_Flags_Values.FLAGS_SIGNED : Packet_Header_Flags_Values.NONE;

            #endregion

            #region flags

            switch (flags)
            {
                case ModelFlags.Binding:
                    {
                        sessionSetupFlags = SESSION_SETUP_Request_Flags.SESSION_FLAG_BINDING;
                        break;
                    }
                case ModelFlags.NotBinding:
                    {
                        sessionSetupFlags = SESSION_SETUP_Request_Flags.NONE;
                        break;
                    }
                default:
                    throw new ArgumentException("flags");
            }

            #endregion

            #region user

            switch (user)
            {
                case ModelUser.DefaultUser:
                    {
                        credential = testConfig.AccountCredential;
                        break;
                    }
                case ModelUser.DiffUser:
                    {
                        credential = testConfig.NonAdminAccountCredential;
                        break;
                    }
                default:
                    throw new ArgumentException("user");
            }

            #endregion

            #region MultipleChannel
            // Multiple Channel only takes affect when Connection.Dialect belongs to the SMB 3.x dialect family
            bool isMultipleChannelSupported = sessionMgmtConfig.IsMultiChannelCapable && ModelUtility.IsSmb3xFamily(connectionList[connectionId].Dialect);
            #endregion

            PrintCurrentSessionTable("Before SessionSetup Request");

            #region Send Request

            uint status;
            SESSION_SETUP_Response? sessionSetupResponse = null;
            string serverName = testConfig.SutComputerName;
            SecurityPackageType securityPackageType = testConfig.DefaultSecurityPackage;
            bool useServerGssToken = testConfig.UseServerGssToken;

            // Use desired explored sessionId
            ulong oldSessionId = connectionList[connectionId].SessionId;
            connectionList[connectionId].SessionId = adapterSessionId;

            // alternative connection and never session setup
            if (connectionId == ModelConnectionId.AlternativeConnection
                && sessionId == ModelSessionId.MainSessionId
                && connectionList[connectionId].SessionKey == null
                && connectionList.ContainsKey(ModelConnectionId.MainConnection))
            {
                connectionList[connectionId].GenerateCryptoKeys(testConfig.SendSignedRequest, false, connectionList[ModelConnectionId.MainConnection], true);
            }

            status = connectionList[connectionId].SessionSetup(
                headerFlags,
                sessionSetupFlags,
                adapterPreviousSessionId,
                securityPackageType,
                serverName,
                credential,
                useServerGssToken,
                isMultipleChannelSupported,
                (header, response) =>
                {
                    sessionSetupResponse = response;
                });

            if (status != Smb2Status.STATUS_SUCCESS
                && status != Smb2Status.STATUS_MORE_PROCESSING_REQUIRED)
            {
                // Restore original sessionId if request failed
                connectionList[connectionId].SessionId = oldSessionId;
            }

            #endregion

            // Insert session to session table
            if (!sessionTable.ContainsKey(sessionId)
                && (status == Smb2Status.STATUS_SUCCESS
                || status == Smb2Status.STATUS_MORE_PROCESSING_REQUIRED))
            {
                sessionTable.Add(sessionId, connectionList[connectionId].SessionId);
            }

            PrintCurrentSessionTable("After SessionSetup Request");

            SessionSetupResponse((ModelSmb2Status)status, connectionId, sessionMgmtConfig);

        }

        public void LogOffRequest(ModelConnectionId connectionId, ModelSessionId sessionId)
        {
            ulong adapterSessionId;

            #region sessionId
            if (sessionTable.ContainsKey(sessionId))
            {
                adapterSessionId = sessionTable[sessionId];
            }
            else
            {
                Random r = new Random();
                adapterSessionId = (ulong)r.Next(1, int.MaxValue);
            }

            #endregion

            PrintCurrentSessionTable("Before LogOff Request");

            #region Send Request
            try
            {
                uint status;
                // Use desired explored sessionId
                ulong oldSessionId = connectionList[connectionId].SessionId;
                connectionList[connectionId].SessionId = adapterSessionId;
                status = connectionList[connectionId].LogOff(
                    (header, response) =>
                    {
                        // do nothing, avoid exception when status != status_success
                    });
                // Restore original sessionId
                connectionList[connectionId].SessionId = oldSessionId;

                if (status == Smb2Status.STATUS_SUCCESS)
                {
                    sessionTable.Remove(sessionId);
                }

                PrintCurrentSessionTable("After LogOff Request");

                LogOffResponse((ModelSmb2Status)status, connectionId, sessionMgmtConfig);
                return;
            }
            catch
            {
            }

            Site.Assert.IsTrue(connectionList[connectionId].Smb2Client.IsServerDisconnected, "Logoff failure should be caused by transport connection termination");

            ExpectDisconnect(connectionId, sessionMgmtConfig);
            sessionTable.Remove(sessionId);
            connectionList.Remove(connectionId);

            #endregion
        }

        public void TerminateConnection(ModelConnectionId connectionId)
        {
            connectionList[connectionId].Disconnect();
            sessionTable.Remove(ModelSessionId.MainSessionId);
            connectionList.Remove(connectionId);
        }

        #endregion

        #region Private Methods
        private void PrintCurrentSessionTable(string testStep)
        {
            Site.Log.Add(LogEntryKind.Debug, "SessionTable " + testStep);
            foreach (KeyValuePair<ModelSessionId, ulong> pair in sessionTable)
            {
                Site.Log.Add(
                    LogEntryKind.Debug,
                    "ModelSessionId: {0}, AdapterSessionId: 0x{1:x8}",
                    pair.Key, pair.Value);
            }
        }
        #endregion
    }
}
