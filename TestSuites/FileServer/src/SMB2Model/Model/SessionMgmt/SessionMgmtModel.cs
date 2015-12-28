// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Modeling;
using Microsoft.Protocols.TestSuites.FileSharing.Common.Adapter;
using Microsoft.Protocols.TestSuites.FileSharing.SMB2Model.Adapter;
using Microsoft.Protocols.TestSuites.FileSharing.SMB2Model.Adapter.SessionMgmt;
using Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Smb2;
using Microsoft.Xrt.Runtime;

[assembly: NativeType("System.Diagnostics.Tracing.*")]
namespace Microsoft.Protocols.TestSuites.FileSharing.SMB2Model.Model.SessionMgmt
{
    /// <summary>
    /// This models behavior of session management for SMB2 server
    /// Assumptions/Restrictions/Notes:
    /// 1. Does not model 2 sessions in 1 connection
    /// 2. Does not include SessionKey/SigningKey/Channel.SigningKey in ADM as it's only for signing purpose
    /// 3. Does not cover encryption related logic which has been covered in encryption model
    /// 4. Does not cover signing related logic which will be covered in signing model
    /// 5. Does not cover session expiration scenario
    /// 6. Does not cover authentication with guest/anonymous user as it only impacts session binding
    /// and such accounts are more likely not in place in partners environment
    /// </summary>
    public static class SessionMgmtModel
    {
        #region State

        /// <summary>
        /// Connections container which models 2 connections at most
        /// </summary>
        public static MapContainer<ModelConnectionId, ModelConnection> ConnectionList = null;

        /// <summary>
        /// GlobalSession table which models 2 sessions at most
        /// </summary>
        public static MapContainer<ModelSessionId, ModelSession> GlobalSessionTable = null;

        /// <summary>
        /// Server configuration related to model
        /// </summary>
        public static SessionMgmtConfig config;

        #endregion

        #region Rule

        /// <summary>
        /// Call for loading server configuration
        /// </summary>
        [Rule(Action = "call ReadConfig(out _)")]
        public static void ReadConfigCall()
        {
            Condition.IsTrue(ConnectionList == null);
        }

        /// <summary>
        /// Return for loading server configuration
        /// </summary>
        /// <param name="c">Server configuration related to model</param>
        [Rule(Action = "return ReadConfig(out c)")]
        public static void ReadConfigReturn(SessionMgmtConfig c)
        {
            Condition.IsTrue(ConnectionList == null);
            Condition.IsNotNull(c);
            Condition.IsTrue(c.MaxSmbVersionSupported == ModelDialectRevision.Smb2002
                || c.MaxSmbVersionSupported == ModelDialectRevision.Smb21
                || c.MaxSmbVersionSupported == ModelDialectRevision.Smb30
                || c.MaxSmbVersionSupported == ModelDialectRevision.Smb302);

            if (!ModelUtility.IsSmb3xFamily(c.MaxSmbVersionSupported))
            {
                Condition.IsFalse(c.IsMultiChannelCapable);
            }

            ConnectionList = new MapContainer<ModelConnectionId, ModelConnection>();
            GlobalSessionTable = new MapContainer<ModelSessionId, ModelSession>();

            config = c;
        }

        /// <summary>
        /// Setup connection by perform following
        ///     1. Negotiate
        /// </summary>
        /// <param name="connectionId">Connection that established</param>
        /// <param name="clientMaxDialect">Max SMB2 dialect that client supports</param>
        [Rule]
        public static void SetupConnection(ModelConnectionId connectionId, ModelDialectRevision clientMaxDialect)
        {
            // Restrict model contains 2 connections at most
            Condition.IsTrue(!ConnectionList.ContainsKey(connectionId));

            // Reduce states
            Combination.NWise(1, connectionId, clientMaxDialect);

            DialectRevision dialect = ModelHelper.DetermineNegotiateDialect(clientMaxDialect, config.MaxSmbVersionSupported);

            // Establish new connection
            ConnectionList.Add(
                connectionId,
                new ModelConnection(dialect));

            ConnectionList[connectionId].ConnectionState = ModelState.Connected;

            ModelHelper.Log(LogType.Requirement, "3.3.5.1: Connection.ConstrainedConnection is set to TRUE.");
            ConnectionList[connectionId].ConstrainedConnection = true;
        }

        /// <summary>
        /// Request for SessionSetup
        /// </summary>
        /// <param name="connectionId">Connection from which to send the request</param>
        /// <param name="sessionId">SessionId set in the request</param>
        /// <param name="previousSessionId">PreviousSessionId set in the request</param>
        /// <param name="signed">Indicates if SMB2_FLAGS_SIGNED bit is set in the Flags field in the header</param>
        /// <param name="flags">Indicates if this is to bind an existing session</param>
        /// <param name="user">Indicates the user credential used for session setup</param>
        /// <param name="ReAuthentication">Indicates if need to re-authenticate an existing session</param>
        [Rule]
        public static void SessionSetupRequest(
            ModelConnectionId connectionId,
            ModelSessionId sessionId,
            ModelSessionId previousSessionId,
            ModelSigned signed,
            ModelFlags flags,
            ModelUser user,
            ModelAllowReauthentication ReAuthentication)
        {

            VerifyConnection(connectionId);
            Condition.IsNull(ConnectionList[connectionId].Request);

            #region Combinations

            Combination.NWise(1, connectionId, sessionId, previousSessionId, signed, flags, user);
            #endregion

            #region Other contraints

            // Do not estabilish 2nd session if no session exist yet
            Condition.IfThen(GlobalSessionTable.Count == 0,
                sessionId != ModelSessionId.AlternativeSessionId
                && user == ModelUser.DefaultUser);

            // Do not establish new session if there's one in the requested connection
            Condition.IfThen(ConnectionList[connectionId].Session != null, sessionId != ModelSessionId.ZeroSessionId);

            // Do not establish new session if 2 sessions exist
            Condition.IfThen(GlobalSessionTable.Count == 2, sessionId != ModelSessionId.ZeroSessionId);

            // Do not set previousSessionId to session1 if currently establish session1
            Condition.IfThen(GlobalSessionTable.Count == 0
                && sessionId == ModelSessionId.ZeroSessionId,
                previousSessionId != ModelSessionId.MainSessionId);

            // If session id is zero, it's the same that the binding flag is set or not.
            Condition.IfThen(sessionId == ModelSessionId.ZeroSessionId, flags == ModelFlags.NotBinding);

            // Restrict the condition to no session exist yet as we only simulate one session in this Model
            Condition.IsNull(ConnectionList[connectionId].Session);

            // If we need to skip SessionSetup when it's re-auth an existing one
            if (ReAuthentication == ModelAllowReauthentication.NotAllowReauthentication)
            {
                Condition.IsTrue(ConnectionList[connectionId].Session == null
                    || ConnectionList[connectionId].Session.State != ModelSessionState.Valid);
            }

            #endregion

            ConnectionList[connectionId].Request = new ModelSessionSetupRequest(
                                                    connectionId,
                                                    sessionId,
                                                    previousSessionId,
                                                    signed == ModelSigned.SignFlagSet,
                                                    flags,
                                                    user);
        }

        /// <summary>
        /// Response of SessionSetup
        /// </summary>
        /// <param name="status">Status in response</param>
        /// <param name="connectionId">Connection from which to receive the response</param>
        /// <param name="c">Session management configuration</param>
        [Rule]
        public static void SessionSetupResponse(ModelSmb2Status status, ModelConnectionId connectionId, SessionMgmtConfig c)
        {
            VerifyConnection(connectionId);
            Condition.IsNotNull(ConnectionList[connectionId].Request);

            Condition.IsTrue(config.Platform == c.Platform);
            Condition.IsTrue(config.IsMultiChannelCapable == c.IsMultiChannelCapable);

            ModelSessionSetupRequest sessionSetupRequest = RetrieveOutstandingRequest<ModelSessionSetupRequest>(connectionId);
            if (sessionSetupRequest.isSigned)
            {
                ModelHelper.Log(LogType.Requirement, "3.3.5.2.4: If the SMB2 header of the request has SMB2_FLAGS_SIGNED set in the Flags field, the server MUST verify the signature.");
                ModelHelper.Log(LogType.TestInfo, "SMB2_FLAGS_SIGNED is set in the SMB2 header of the SessionSetup Request.");

                // If server does not support Multiple channel then whether binding is set is meaningless.
                if (config.IsMultiChannelCapable && sessionSetupRequest.flags == ModelFlags.Binding)
                {
                    ModelHelper.Log(LogType.Requirement,
                        "If the request is for binding the session, the server MUST look up the session in the GlobalSessionTable using the SessionId in the SMB2 header of the request. ");
                    ModelHelper.Log(LogType.TestInfo, "SMB2_SESSION_FLAG_BINDING bit is set.");

                    if (!GlobalSessionTable.ContainsKey(sessionSetupRequest.sessionId))
                    {
                        ModelHelper.Log(LogType.Requirement,
                            "If the session is not found, the request MUST be failed, as specified in section Sending an Error Response (section 3.3.4.4), " +
                            "with the error code STATUS_USER_SESSION_DELETED. ");
                        ModelHelper.Log(LogType.TestInfo, "The session is not found in GlobalSessionTable.");
                        ModelHelper.Log(LogType.TestTag, TestTag.InvalidIdentifier);
                        Condition.IsTrue(status == ModelSmb2Status.STATUS_USER_SESSION_DELETED);
                        return;
                    }
                }
                else
                {
                    ModelHelper.Log(LogType.Requirement,
                        "For all other requests, the server MUST look up the session in the Connection.SessionTable using the SessionId in the SMB2 header of the request.");
                    ModelHelper.Log(LogType.TestInfo, "SMB2_SESSION_FLAG_BINDING bit is not set.");

                    if (ConnectionList[connectionId].Session == null || ConnectionList[connectionId].Session.SessionId != sessionSetupRequest.sessionId)
                    {
                        ModelHelper.Log(LogType.Requirement,
                            "If the session is not found, the request MUST be failed, as specified in section Sending an Error Response (section 3.3.4.4), " +
                            "with the error code STATUS_USER_SESSION_DELETED. ");
                        ModelHelper.Log(LogType.TestInfo, "The session is not found in Connection.SessionTable.");
                        ModelHelper.Log(LogType.TestTag, TestTag.InvalidIdentifier);
                        Condition.IsTrue(status == ModelSmb2Status.STATUS_USER_SESSION_DELETED);
                        return;
                    }
                }
            }

            if (sessionSetupRequest.sessionId == ModelSessionId.ZeroSessionId)
            {
                ModelHelper.Log(LogType.Requirement, "3.3.5.5: 3. If SessionId in the SMB2 header of the request is zero, the server MUST process the authentication request as specified in section 3.3.5.5.1.");
                ModelHelper.Log(LogType.TestInfo, "The SessionId of the SessionSetup Request is zero");

                AuthNewSession(status, sessionSetupRequest);
                Condition.IsTrue(status == ModelSmb2Status.STATUS_SUCCESS || status == ModelSmb2Status.STATUS_MORE_PROCESSING_REQUIRED);
                return;
            }

            if (ModelUtility.IsSmb3xFamily(ConnectionList[sessionSetupRequest.connectionId].NegotiateDialect)
                && config.IsMultiChannelCapable
                && sessionSetupRequest.flags == ModelFlags.Binding)
            {
                ModelHelper.Log(LogType.Requirement,
                    "3.3.5.5: 4. If Connection.Dialect belongs to the SMB 3.x dialect family, IsMultiChannelCapable is TRUE," +
                    "and the SMB2_SESSION_FLAG_BINDING bit is set in the Flags field of the request, the server MUST perform the following:" +
                    "The server MUST look up the session in GlobalSessionTable using the SessionId from the SMB2 header.");
                ModelHelper.Log(LogType.TestInfo,
                    "Connection.Dialect is {0}, IsMultiChannelCapable is TRUE, and the SMB2_SESSION_FLAG_BINDING bit is set",
                    ConnectionList[sessionSetupRequest.connectionId].NegotiateDialect);

                if (!GlobalSessionTable.ContainsKey(sessionSetupRequest.sessionId))
                {
                    ModelHelper.Log(LogType.Requirement, "If the session is not found, the server MUST fail the session setup request with STATUS_USER_SESSION_DELETED.");
                    ModelHelper.Log(LogType.TestInfo, "The SessionId cannot be found in GlobalSessionTable");
                    ModelHelper.Log(LogType.TestTag, TestTag.InvalidIdentifier);
                    Condition.IsTrue(status == ModelSmb2Status.STATUS_USER_SESSION_DELETED);
                    return;
                }

                ModelHelper.Log(LogType.Requirement, "If a session is found, the server MUST do the following:");
                if (ConnectionList[sessionSetupRequest.connectionId].NegotiateDialect != GlobalSessionTable[sessionSetupRequest.sessionId].Dialect)
                {
                    ModelHelper.Log(LogType.Requirement, "If Connection.Dialect is not the same as Session.Connection.Dialect, the server MUST fail the request with STATUS_INVALID_PARAMETER.");
                    ModelHelper.Log(LogType.TestInfo,
                        "The Connection.Dialect is {0}, Session.Connection.Dialect is {1}",
                        ConnectionList[sessionSetupRequest.connectionId].NegotiateDialect,
                        GlobalSessionTable[sessionSetupRequest.sessionId].Dialect);
                    ModelHelper.Log(LogType.TestTag, TestTag.Compatibility);
                    Condition.IsTrue(status == ModelSmb2Status.STATUS_INVALID_PARAMETER);
                    return;
                }

                if (!sessionSetupRequest.isSigned)
                {
                    ModelHelper.Log(LogType.Requirement,
                        "If the SMB2_FLAGS_SIGNED bit is not set in the Flags field in the header, the server MUST fail the request with error STATUS_INVALID_PARAMETER.");
                    ModelHelper.Log(LogType.TestInfo, "The SMB2_FLAGS_SIGNED bit is not set in the SessionSetup Request");
                    ModelHelper.Log(LogType.TestTag, TestTag.UnexpectedFields);
                    Condition.IsTrue(status == ModelSmb2Status.STATUS_INVALID_PARAMETER);
                    return;
                }

                if (GlobalSessionTable[sessionSetupRequest.sessionId].State == ModelSessionState.InProgress)
                {
                    ModelHelper.Log(LogType.Requirement, "If Session.State is InProgress, the server MUST fail the request with STATUS_REQUEST_NOT_ACCEPTED.");
                    ModelHelper.Log(LogType.TestInfo, "Session.State is InProgress");
                    ModelHelper.Log(LogType.TestTag, TestTag.Compatibility);
                    Condition.IsTrue(status == ModelSmb2Status.STATUS_REQUEST_NOT_ACCEPTED);
                    return;
                }

                // If Session.IsAnonymousor Session.IsGuestis TRUE, the server MUST fail the request with STATUS_NOT_SUPPORTED.
                // Skip above requirement according to assumption 6.

                if (ConnectionList[sessionSetupRequest.connectionId].Session != null
                    && ConnectionList[sessionSetupRequest.connectionId].Session.SessionId == sessionSetupRequest.sessionId)
                {
                    ModelHelper.Log(LogType.Requirement,
                        "If there is a session in Connection.SessionTable identified by the SessionId in the request, the server MUST fail the request with STATUS_REQUEST_NOT_ACCEPTED.");
                    ModelHelper.Log(LogType.TestInfo, "There is a session in Connection.SessionTable which has a same SessionId in the request");
                    ModelHelper.Log(LogType.TestTag, TestTag.Compatibility);
                    Condition.IsTrue(status == ModelSmb2Status.STATUS_REQUEST_NOT_ACCEPTED);
                    return;
                }

                // The server MUST verify the signature as specified in section 3.3.5.2.4, using the Session.SessionKey.
                // Skip above requirement as it is verified in signing model

                if (sessionSetupRequest.user == ModelUser.DiffUser)
                {
                    ModelHelper.Log(LogType.Requirement,
                        "The server MUST obtain the security context from the GSS authentication subsystem, " +
                        "and it MUST invoke the GSS_Inquire_context call as specified in [RFC2743] section 2.2.6, " +
                        "passing the security context as the input parameter." +
                        "If the returned \"src_name\" does not match with the Session.Username, the server MUST fail the request with error code STATUS_NOT_SUPPORTED.");
                    ModelHelper.Log(LogType.TestInfo, "A different user is used when binding to an existing session");
                    ModelHelper.Log(LogType.TestTag, TestTag.Compatibility);
                    Condition.IsTrue(status == ModelSmb2Status.STATUS_NOT_SUPPORTED);
                    return;
                }
            }
            else
            {
                if (config.Platform == Platform.WindowsServer2012
                        && sessionSetupRequest.flags == ModelFlags.Binding)
                {
                    ModelHelper.Log(LogType.Requirement,
                        "<232> Section 3.3.5.5: Windows 8 and Windows Server 2012 look up the session in GlobalSessionTable using the SessionId from the SMB2 header " +
                        "if the SMB2_SESSION_FLAG_BINDING bit is set in the Flags field of the request. ");
                    ModelHelper.Log(LogType.TestInfo, "The SUT platform is {0}. The SMB2_SESSION_FLAG_BINDING bit is set.", config.Platform);
                    ModelHelper.Log(LogType.TestTag, TestTag.Compatibility);

                    if (GlobalSessionTable.ContainsKey(sessionSetupRequest.sessionId))
                    {
                        ModelHelper.Log(LogType.Requirement, "If the session is found, the server fails the request with STATUS_REQUEST_NOT_ACCEPTED. ");
                        ModelHelper.Log(LogType.TestInfo, "The session is found");
                        Condition.IsTrue(status == ModelSmb2Status.STATUS_REQUEST_NOT_ACCEPTED);
                        return;
                    }
                    else
                    {
                        ModelHelper.Log(LogType.Requirement, "If the session is not found, the server fails the request with STATUS_USER_SESSION_DELETED.");
                        ModelHelper.Log(LogType.TestInfo, "The session is not found");
                        Condition.IsTrue(status == ModelSmb2Status.STATUS_USER_SESSION_DELETED);
                        return;
                    }
                }
                if (ModelUtility.IsSmb3xFamily(config.MaxSmbVersionSupported)
                    && ((ModelUtility.IsSmb2Family(ConnectionList[sessionSetupRequest.connectionId].NegotiateDialect) || !config.IsMultiChannelCapable))
                    && sessionSetupRequest.flags == ModelFlags.Binding)
                {
                    ModelHelper.Log(LogType.Requirement,
                        "3.3.5.5: Otherwise, if the server implements the SMB 3.x dialect family, " +
                        "and Connection.Dialect is equal to \"2.002\" or \"2.100\" or IsMultiChannelCapable is FALSE, " +
                        "and SMB2_SESSION_FLAG_BINDING bit is set in the Flags field of the request, " +
                        "the server SHOULD<225> fail the session setup request with STATUS_REQUEST_NOT_ACCEPTED.");
                    ModelHelper.Log(LogType.TestInfo,
                        "Connection.Dialect is {0}, IsMultiChannelCapable is {1}, SUT platform is {2}, Max Smb version supported is {3} and SMB2_SESSION_FLAG_BINDING bit is set.",
                        ConnectionList[sessionSetupRequest.connectionId].NegotiateDialect,
                        config.IsMultiChannelCapable,
                        config.Platform,
                        config.MaxSmbVersionSupported);
                    ModelHelper.Log(LogType.TestTag, TestTag.Compatibility);

                    if (config.Platform != Platform.NonWindows)
                    {
                        Condition.IsTrue(status == ModelSmb2Status.STATUS_REQUEST_NOT_ACCEPTED);
                    }
                    else
                    {
                        ModelHelper.Log(LogType.TestInfo, "The SUT platform is NonWindows, so the server could fail the request with other error code.");
                        Condition.IsTrue(status != ModelSmb2Status.STATUS_SUCCESS);
                    }
                    return;
                }                
                else
                {
                    if (ConnectionList[sessionSetupRequest.connectionId].Session == null
                        || ConnectionList[sessionSetupRequest.connectionId].Session.SessionId != sessionSetupRequest.sessionId)
                    {
                        ModelHelper.Log(LogType.Requirement,
                            "3.3.5.5: Otherwise, the server MUST look up the session in Connection.SessionTable using the SessionId from the SMB2 header." +
                            "If the session is not found, the server MUST fail the session setup request with STATUS_USER_SESSION_DELETED. ");
                        ModelHelper.Log(LogType.TestInfo, "The session is not found using the SessionId of the request");
                        ModelHelper.Log(LogType.TestTag, TestTag.Compatibility);
                        Condition.IsTrue(status == ModelSmb2Status.STATUS_USER_SESSION_DELETED);
                        return;
                    }
                }
            }
            if (GlobalSessionTable[sessionSetupRequest.sessionId].State == ModelSessionState.Valid)
            {
                ModelHelper.Log(LogType.Requirement, "3.3.5.5: 6. If Session.State is Valid, the server SHOULD process the session setup request as specified in section 3.3.5.5.2.");
                ModelHelper.Log(LogType.TestInfo, "Session.State is Valid");
                if (config.Platform == Platform.WindowsServer2008)
                {
                    ModelHelper.Log(LogType.Requirement, "Footnote: Windows Vista SP1 and Windows Server 2008 servers fail the session setup request with STATUS_REQUEST_NOT_ACCEPTED.");
                    ModelHelper.Log(LogType.TestInfo, "The SUT platform is Windows Server 2008");
                    ModelHelper.Log(LogType.TestTag, TestTag.Compatibility);
                    Condition.IsTrue(status == ModelSmb2Status.STATUS_REQUEST_NOT_ACCEPTED);
                    return;
                }

                if (config.Platform != Platform.NonWindows)
                {
                    ModelHelper.Log(LogType.TestInfo, "The SUT platform is Windows");
                    ReAuthentication(status, sessionSetupRequest);

                    Condition.IsTrue(status == ModelSmb2Status.STATUS_SUCCESS || status == ModelSmb2Status.STATUS_MORE_PROCESSING_REQUIRED);
                    return;
                }
            }

            ModelHelper.Log(LogType.Requirement, "3.3.5.5: 7. The server MUST continue processing the request as specified in section 3.3.5.5.3.");
            HandleGssApiAuth(status, sessionSetupRequest);
            ModelHelper.Log(LogType.TestInfo, "The authentication should succeed.");
            Condition.IsTrue(status == ModelSmb2Status.STATUS_SUCCESS || status == ModelSmb2Status.STATUS_MORE_PROCESSING_REQUIRED);
        }

        /// <summary>
        /// Request of LogOff
        /// </summary>
        /// <param name="connectionId">Connection from which to send the request</param>
        /// <param name="sessionId">SessionId set in the request</param>
        [Rule]
        public static void LogOffRequest(ModelConnectionId connectionId, ModelSessionId sessionId)
        {
            VerifyConnection(connectionId);
            Condition.IsNull(ConnectionList[connectionId].Request);

            // Reduce states
            Combination.NWise(1, connectionId, sessionId);

            // If no session exists before, then any session id is invalid
            Condition.IfThen(ConnectionList[connectionId].Session == null, sessionId == ModelSessionId.InvalidSessionId);

            // If the session id in LogOff request is different from the existing one, then the id is invalid.
            Condition.IfThen(ConnectionList[connectionId].Session != null && ConnectionList[connectionId].Session.SessionId != sessionId,
                sessionId == ModelSessionId.InvalidSessionId);

            if (ConnectionList[connectionId].Session == null
                && ConnectionList[connectionId].ConstrainedConnection)
            {
                ModelHelper.Log(LogType.Requirement,
                    "3.3.5.2.9: The server MUST look up the Session in Connection.SessionTable by using the SessionId in the SMB2 header of the request. " +
                    "If Connection.SessionTable is empty and Connection.ConstrainedConnection is TRUE, the server SHOULD<210> disconnect the connection. ");
                ModelHelper.Log(LogType.TestInfo, "Connection.SessionTable is empty and Connection.ConstrainedConnection is TRUE");
                ModelHelper.Log(LogType.TestTag, TestTag.Compatibility);
                // server SHOULD disconnect the connection
                ConnectionList[connectionId].ConnectionState = ModelState.Disconnected;
                return;
            }

            ConnectionList[connectionId].Request = new ModelLogOffRequest(connectionId, sessionId);
        }

        /// <summary>
        /// Response of LogOff
        /// </summary>
        /// <param name="status">Status in response</param>
        /// <param name="connectionId">Connection from which to receive the response</param>
        /// <param name="c">Session management configuration</param>
        [Rule]
        public static void LogOffResponse(ModelSmb2Status status, ModelConnectionId connectionId, SessionMgmtConfig c)
        {
            Condition.IsTrue(config.Platform == c.Platform);
            if (config.Platform == Platform.NonWindows
                && ConnectionList.ContainsKey(connectionId)
                && ConnectionList[connectionId].Session == null
                && ConnectionList[connectionId].ConstrainedConnection)
            {
                // The TD statements are logged in LogOffRequest
                ModelHelper.Log(LogType.TestInfo, "SUT platform is NonWindows, so server could return an error code instead of disconnect the connection.");
                Condition.IsTrue(status != ModelSmb2Status.STATUS_SUCCESS);
                return;
            }

            if (VerifyFootnote(connectionId, c, status))
                return;

            VerifyConnection(connectionId);
            Condition.IsNotNull(ConnectionList[connectionId].Request);

            ModelLogOffRequest logOffRequest = RetrieveOutstandingRequest<ModelLogOffRequest>(connectionId);

            if (ConnectionList[connectionId].Session != null && ConnectionList[connectionId].Session.SessionId != logOffRequest.sessionId)
            {
                ModelHelper.Log(LogType.Requirement,
                    "3.3.5.2.9: If Connection.SessionTable is not empty and SessionId is not found in Connection.SessionTable, " +
                    "the server MUST fail the request with STATUS_USER_SESSION_DELETED.");
                ModelHelper.Log(LogType.TestInfo, "Connection.SessionTable is not empty and SessionId of Logoff Request is not found in Connection.SessionTable");
                ModelHelper.Log(LogType.TestTag, TestTag.InvalidIdentifier);
                Condition.IsTrue(status == ModelSmb2Status.STATUS_USER_SESSION_DELETED);
                return;
            }

            if (!ConnectionList[connectionId].ConstrainedConnection && ConnectionList[connectionId].Session == null)
            {
                ModelHelper.Log(LogType.Requirement,
                    "3.3.5.2.9: If Connection.ConstrainedConnection is FALSE and Connection.SessionTable is empty, then the server MUST fail any request with STATUS_USER_SESSION_DELETED.");
                ModelHelper.Log(LogType.TestInfo, "Connection.ConstrainedConnection is FALSE and Connection.SessionTable is empty.");
                ModelHelper.Log(LogType.TestTag, TestTag.Compatibility);
                Condition.IsTrue(status == ModelSmb2Status.STATUS_USER_SESSION_DELETED);
                return;
            }

            if (GlobalSessionTable.ContainsKey(logOffRequest.sessionId))
            {
                if (GlobalSessionTable[logOffRequest.sessionId].State == ModelSessionState.InProgress)
                {
                    ModelHelper.Log(LogType.Requirement, "3.3.5.2.9: If Session.State is InProgress, the server MUST continue to process the SMB2 LOGOFF, SMB2 CLOSE, and SMB2 LOCK commands. ");
                    ModelHelper.Log(LogType.TestInfo, "Session.State is InProgress");
                    ModelHelper.Log(LogType.TestTag, TestTag.Compatibility);
                    // Do nothing for LOGOFF and continue
                }

                ModelHelper.Log(LogType.Requirement, "3.3.5.6: The server MUST remove this session from the GlobalSessionTable and also from the Connection.SessionTable");
                GlobalSessionTable.Remove(logOffRequest.sessionId);
                ConnectionList[logOffRequest.connectionId].Session = null;
            }

            ModelHelper.Log(LogType.TestInfo, "Verifying the Session succeeds.");
            Condition.IsTrue(status == ModelSmb2Status.STATUS_SUCCESS);

        }

        /// <summary>
        /// Disconnect the underlying connection
        /// </summary>
        /// <param name="connectionId">Connection to be disconnected</param>
        [Rule]
        public static void TerminateConnection(ModelConnectionId connectionId)
        {
            VerifyConnection(connectionId);

            if (ConnectionList[connectionId].Session != null)
            {
                ModelHelper.Log(LogType.Requirement, "3.3.7.1: the Session MUST be removed from GlobalSessionTable and freed. ");
                GlobalSessionTable.Remove(ConnectionList[connectionId].Session.SessionId);
            }

            // Disconnect and remove the connection
            ConnectionList[connectionId].ConnectionState = ModelState.Disconnected;

            ModelHelper.Log(LogType.Requirement, "3.3.7.1: The connection MUST be removed from ConnectionList and MUST be freed.");
            ModelHelper.Log(LogType.TestTag, TestTag.Compatibility);
            ConnectionList.Remove(connectionId);
        }

        /// <summary>
        /// Client expects the disconnection when server terminates the connection
        /// </summary>
        /// <param name="connectionId">Connection terminated</param>
        /// <param name="c">Session management configuration</param>
        [Rule]
        public static void ExpectDisconnect(ModelConnectionId connectionId, SessionMgmtConfig c)
        {
            if (VerifyFootnote(connectionId, c, null))
                return;

            Condition.IsTrue(ConnectionList.ContainsKey(connectionId));
            Condition.IsTrue(ConnectionList[connectionId].ConnectionState == ModelState.Disconnected);

            if (ConnectionList[connectionId].Session != null)
            {
                ModelHelper.Log(LogType.Requirement, "3.3.7.1: the Session MUST be removed from GlobalSessionTable and freed. ");
                GlobalSessionTable.Remove(ConnectionList[connectionId].Session.SessionId);
            }

            ModelHelper.Log(LogType.Requirement, "3.3.7.1: The connection MUST be removed from ConnectionList and MUST be freed.");
            ModelHelper.Log(LogType.TestTag, TestTag.Compatibility);
            ConnectionList.Remove(connectionId);
        }

        #endregion

        #region Private Methods

        private static void AuthNewSession(ModelSmb2Status status, ModelSessionSetupRequest sessionSetupRequest)
        {
            ModelHelper.Log(LogType.Requirement, "3.3.5.5.1: A session object MUST be allocated for this request. ");

            ModelSession session = new ModelSession();
            ModelSessionId? sessionId = null;
            if (!GlobalSessionTable.ContainsKey(ModelSessionId.MainSessionId))
            {
                sessionId = ModelSessionId.MainSessionId;
            }
            else
            {
                sessionId = ModelSessionId.AlternativeSessionId;
            }

            // Update sessionId to new created one from zero
            sessionSetupRequest.sessionId = sessionId.Value;

            ModelHelper.Log(LogType.Requirement, "The other values MUST be initialized as follows:");
            session.Dialect = ConnectionList[sessionSetupRequest.connectionId].NegotiateDialect;

            ModelHelper.Log(LogType.Requirement, "Session.State is set to InProgress.");
            session.State = ModelSessionState.InProgress;
            session.SessionId = sessionId.Value;

            ModelHelper.Log(LogType.Requirement,
                "The session MUST be inserted into the GlobalSessionTable and a unique Session.SessionId is assigned to serve as a lookup key in the table. ");
            GlobalSessionTable.Add(sessionSetupRequest.sessionId, session);

            ModelHelper.Log(LogType.Requirement, "The session MUST be inserted into Connection.SessionTable. ");
            ConnectionList[sessionSetupRequest.connectionId].Session = session;

            ModelHelper.Log(LogType.Requirement, "Using this session, authentication is continued as specified in section 3.3.5.5.3.");
            HandleGssApiAuth(status, sessionSetupRequest);
        }

        private static void ReAuthentication(ModelSmb2Status status, ModelSessionSetupRequest sessionSetupRequest)
        {
            // Update both GlobalSessionTable and ConnectionList
            ModelHelper.Log(LogType.Requirement, "3.3.5.5.2: Session.State MUST be set to InProgress");
            GlobalSessionTable[sessionSetupRequest.sessionId].State = ModelSessionState.InProgress;

            ModelHelper.Log(LogType.Requirement, "Authentication is continued as specified in section 3.3.5.5.3.");
            HandleGssApiAuth(status, sessionSetupRequest);
        }

        private static void HandleGssApiAuth(ModelSmb2Status status, ModelSessionSetupRequest sessionSetupRequest)
        {
            ModelHelper.Log(LogType.Requirement, "3.3.5.5.3: 1. The status code in the SMB2 header of the response MUST be set to STATUS_SUCCESS. ");
            if (status != ModelSmb2Status.STATUS_SUCCESS)
            {
                return;
            }

            if (ModelUtility.IsSmb3xFamily(ConnectionList[sessionSetupRequest.connectionId].NegotiateDialect))
            {
                ModelHelper.Log(LogType.Requirement, "If Connection.Dialect belongs to the SMB 3.x dialect family, the server MUST insert the Session into Connection.SessionTable. ");
                ModelHelper.Log(LogType.TestInfo, "Connection.Dialect is {0}", ConnectionList[sessionSetupRequest.connectionId].NegotiateDialect);
                ConnectionList[sessionSetupRequest.connectionId].Session
                    = GlobalSessionTable[sessionSetupRequest.sessionId];
            }

            ModelHelper.Log(LogType.Requirement, "If Session.IsAnonymous is FALSE, the server MUST set Connection.ConstrainedConnection to FALSE.");
            ModelHelper.Log(LogType.TestInfo, "Session.IsAnonymous is FALSE, so set Connection.ConstrainedConnection to FALSE");
            ConnectionList[sessionSetupRequest.connectionId].ConstrainedConnection = false;

            /*
             Assuming STATUS_SUCCESS indicates final message in the authentication exchange

             Skip following requirement according to assumption 6.
             If the returned anon_state is TRUE, the server MUST set Session.IsAnonymous to TRUE and the server MAY set
             the SMB2_SESSION_FLAG_IS_NULL flag in the SessionFlags field of the SMB2 SESSION_SETUP Response.
             Otherwise, if the returned src_name corresponds to an implementation-specific guest user,<211>
             the server MUST set the SMB2_SESSION_FLAG_IS_GUEST in the SessionFlags field of the SMB2 SESSION_SETUP Response
             and MUST set Session.IsGuest to TRUE.
             */

            if (sessionSetupRequest.previousSessionId != ModelSessionId.ZeroSessionId)
            {
                ModelHelper.Log(LogType.Requirement, "12. If the PreviousSessionId field of the request is not equal to zero, the server MUST take the following actions:");
                ModelHelper.Log(LogType.TestInfo, "PreviousSessionId is not equal to zero.");

                ModelHelper.Log(LogType.Requirement, "1. The server MUST look up the old session in GlobalSessionTable, where Session.SessionId matches PreviousSessionId. ");
                if (GlobalSessionTable.ContainsKey(sessionSetupRequest.previousSessionId))
                {
                    ModelHelper.Log(LogType.Requirement,
                        "If a session is found with Session.SessionId equal to PreviousSessionId, " +
                        "the server MUST determine if the old session and the newly established session are created by the same user " +
                        "by comparing the user identifiers obtained from the Session.SecurityContext on the new and old session.");
                    ModelHelper.Log(LogType.TestInfo, "There is a session with Session.SessionId equal to PreviousSessionId.");

                    if (sessionSetupRequest.previousSessionId == sessionSetupRequest.sessionId)
                    {
                        ModelHelper.Log(LogType.Requirement, "1. If the PreviousSessionId and SessionId values in the SMB2 header of the request are equal, " +
                            "the server SHOULD<230> ignore PreviousSessionId and no other processing is required.");
                        ModelHelper.Log(LogType.TestInfo, "PreviousSessionId is equal to SessionId.");
                        // Do nothing
                    }
                    else if (sessionSetupRequest.user == ModelUser.DefaultUser)
                    {
                        ModelHelper.Log(LogType.Requirement,
                            "2.	Otherwise, if the server determines the authentications were for the same user, " +
                            "the server MUST remove the old session from the GlobalSessionTable and also from the Connection.SessionTable, as specified in section 3.3.7.1.");
                        ModelHelper.Log(LogType.TestInfo, "The authentications were for the same user");
                        GlobalSessionTable.Remove(sessionSetupRequest.previousSessionId);
                    }
                }
            }

            ModelHelper.Log(LogType.Requirement, "13. Session.State MUST be set to Valid.");
            // Update both GlobalSessionTable and ConnectionList
            GlobalSessionTable[sessionSetupRequest.sessionId].State = ModelSessionState.Valid;
            ConnectionList[sessionSetupRequest.connectionId].Session.State = ModelSessionState.Valid;

        }

        private static void VerifyConnection(ModelConnectionId connectionId)
        {
            // Ensure connection exist and connected
            Condition.IsTrue(ConnectionList.ContainsKey(connectionId));
            Condition.IsTrue(ConnectionList[connectionId].ConnectionState == ModelState.Connected);
        }

        private static T RetrieveOutstandingRequest<T>(ModelConnectionId connectionId) where T : ModelSMB2Request
        {
            ModelSMB2Request request = ConnectionList[connectionId].Request;
            ConnectionList[connectionId].Request = null;

            Condition.IsTrue(request is T);
            return request as T;
        }

        private static bool VerifyFootnote(ModelConnectionId connectionId, SessionMgmtConfig c, ModelSmb2Status? status)
        {
            Condition.IsTrue(config.Platform == c.Platform);

            if ((config.Platform == Platform.WindowsServer2008 || config.Platform == Platform.WindowsServer2008R2)
                && ConnectionList.ContainsKey(connectionId) && ConnectionList[connectionId].Session == null)
            {
                ModelHelper.Log(LogType.Requirement,
                    "<210> Section 3.3.5.2.9: Windows Vista, Windows Server 2008, Windows 7, and Windows Server 2008 R2 fail the request with STATUS_USER_SESSION_DELETED. " +
                    "If Connection.ConstrainedConnection is FALSE and Connection.SessionTable is empty, then server MUST fail any request with STATUS_USER_SESSION_DELETED. ");
                // No matter Connection.ConstrainedConnection is FALSE or TRUE, Windows Server 2008 and 2008 R2 will fail the Log off request.
                ModelHelper.Log(LogType.TestInfo,
                    "The SUT platform is {0}, Connection.ConstrainedConnection is {1}, and Connection.SessionTable is empty",
                    config.Platform, ConnectionList[connectionId].ConstrainedConnection);
                ModelHelper.Log(LogType.TestTag, TestTag.Compatibility);
                if (status == null)
                {
                    // null means SUT terminates the connection.
                    // but SUT should return STATUS_USER_SESSION_DELETED
                    Condition.IsTrue(false);
                }
                else
                {
                    Condition.IsTrue(status.Value == ModelSmb2Status.STATUS_USER_SESSION_DELETED);
                }
                // Return true stands for this function handles the message, no more actions needed.
                return true;
            }
            // Go on processing message.
            return false;
        }

        #endregion

    }
}
