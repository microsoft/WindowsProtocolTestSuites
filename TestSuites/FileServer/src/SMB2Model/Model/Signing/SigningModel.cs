// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Text;
using Microsoft.Modeling;
using Microsoft.Protocols.TestSuites.FileSharing.SMB2Model.Adapter;
using Microsoft.Protocols.TestSuites.FileSharing.SMB2Model.Adapter.Signing;
using Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Smb2;
using Microsoft.Xrt.Runtime;

[assembly: NativeType("System.Diagnostics.Tracing.*")]
namespace Microsoft.Protocols.TestSuites.FileSharing.SMB2Model.Model.Signing
{
    public static class SigningModel
    {
        #region State
        /// <summary>
        /// The dialect after negotiation
        /// </summary>
        public static DialectRevision NegotiateDialect;

        /// <summary>
        /// Server model state
        /// </summary>
        private static ModelState State = ModelState.Uninitialized;

        /// <summary>
        /// Server configuration related to model
        /// </summary>
        public static SigningConfig Config;

        /// <summary>
        /// If set, indicate all sessions on this connection(with the exception of anonymous and guest session) MUST have signing enabled.
        /// </summary>
        public static bool Connection_ShouldSign = false;

        /// <summary>
        /// Indicate all the messages for this session MUST be signed
        /// </summary>
        public static bool Session_SigningRequired;

        /// <summary>
        /// Indicate the user type used in session setup
        /// </summary>
        public static bool Session_IsAnonymous = false;

        /// <summary>
        /// Indicate the session exists or not
        /// </summary>
        public static bool Session_IsExisted = false;

        /// <summary>
        /// Request that server model is handling
        /// </summary>
        public static ModelSMB2Request Request;
        #endregion

        #region Actions
        /// <summary>
        /// Call for loading server configuration
        /// </summary>
        [Rule(Action = "call ReadConfig(out _)")]
        public static void ReadConfigCall()
        {
            Condition.IsTrue(State == ModelState.Uninitialized);
        }

        /// <summary>
        /// Return for loading server configuration
        /// </summary>
        /// <param name="c">Server configuration related to model</param>
        [Rule(Action = "return ReadConfig(out c)")]
        public static void ReadConfigReturn(SigningConfig c)
        {
            Condition.IsTrue(State == ModelState.Uninitialized);
            Condition.IsNotNull(c);

            NegotiateDialect = DialectRevision.Smb2Unknown;

            Condition.IsTrue(c.MaxSmbVersionSupported == ModelDialectRevision.Smb2002
                || c.MaxSmbVersionSupported == ModelDialectRevision.Smb21
                || c.MaxSmbVersionSupported == ModelDialectRevision.Smb30
                || c.MaxSmbVersionSupported == ModelDialectRevision.Smb302);
            Config = c;

            Request = null;

            State = ModelState.Initialized;
        }

        /// <summary>
        /// Negotiate request sent to server
        /// </summary>
        /// <param name="maxSmbVersionClientSupported">Indicate the max dialect revision client supports</param>
        /// <param name="signingFlagType">Indicate if the Negotiate request is signed</param>
        /// <param name="signingEnabledType">Indicate if SMB2_NEGOTIATE_SIGNING_ENABLED is set in SecurityMode field</param>
        /// <param name="signingRequiredType">Indicate if SMB2_NEGOTIATE_SIGNING_REQUIRED is set in SecurityMode field</param>
        [Rule]
        public static void NegotiateRequest(
            ModelDialectRevision maxSmbVersionClientSupported, 
            SigningFlagType signingFlagType, 
            SigningEnabledType signingEnabledType, 
            SigningRequiredType signingRequiredType)
        {
            Condition.IsTrue(State == ModelState.Initialized);
            Condition.IsNull(Request);

            // Add isolate for SigningFlagType to reduce redundant cases: If the signingFlagType is set to SignedFlagSet, server will always return error
            // code STATUS_INVALID_PARAMETER, we don't need to cover all the invalid combination with other params.
            Combination.Isolated(signingFlagType == SigningFlagType.SignedFlagSet);

            NegotiateDialect = ModelHelper.DetermineNegotiateDialect(maxSmbVersionClientSupported, Config.MaxSmbVersionSupported);

            Request = new SigningModelRequest(signingFlagType, signingEnabledType, signingRequiredType);

            State = ModelState.Connected;
        }

        /// <summary>
        /// Negotiate response from server
        /// </summary>
        /// <param name="status">Status of the response</param>
        [Rule]
        public static void NegotiateResponse(ModelSmb2Status status, SigningEnabledType signingEnabledType, SigningRequiredType signingRequiredType, SigningConfig c)
        {
            Condition.IsTrue(State == ModelState.Connected);

            SigningModelRequest negotiateRequest = ModelHelper.RetrieveOutstandingRequest<SigningModelRequest>(ref Request);

            if (negotiateRequest.signingFlagType == SigningFlagType.SignedFlagSet)
            {
                ModelHelper.Log(LogType.Requirement,
                    "3.3.5.2.4: If the SMB2 Header of the SMB2 NEGOTIATE request has the SMB2_FLAGS_SIGNED bit set in the Flags field, " +
                    "the server MUST fail the request with STATUS_INVALID_PARAMETER.");
                ModelHelper.Log(LogType.TestInfo, "SMB2_FLAGS_SIGNED bit in the NEGOTIATE request is set.");
                ModelHelper.Log(LogType.TestTag, TestTag.UnexpectedFields);
                Condition.IsTrue(status == ModelSmb2Status.STATUS_INVALID_PARAMETER);
                State = ModelState.Uninitialized;

                return;
            }

            if (negotiateRequest.signingRequiredType == SigningRequiredType.SigningRequiredSet)
            {
                ModelHelper.Log(LogType.Requirement,
                    "3.3.5.4: If SMB2_NEGOTIATE_SIGNING_REQUIRED is set in SecurityMode, the server MUST set Connection.ShouldSign to TRUE.");
                ModelHelper.Log(LogType.TestInfo, "Connection.ShouldSign is set to TRUE.");
                Connection_ShouldSign = true;
            }

            ModelHelper.Log(LogType.Requirement, "3.3.5.4: SecurityMode MUST have the SMB2_NEGOTIATE_SIGNING_ENABLED bit set.");
            Condition.IsTrue(signingEnabledType == SigningEnabledType.SigningEnabledSet);

            Condition.IsTrue(Config.IsServerSigningRequired == c.IsServerSigningRequired);
            if (Config.IsServerSigningRequired)
            {
                ModelHelper.Log(LogType.Requirement,
                    "3.3.5.4: If RequireMessageSigning is TRUE, the server MUST also set SMB2_NEGOTIATE_SIGNING_REQUIRED in the SecurityMode field.");
                ModelHelper.Log(LogType.TestInfo, "RequireMessageSigning is TRUE.");
                Condition.IsTrue(signingRequiredType == SigningRequiredType.SigningRequiredSet);
            }

            Condition.IsTrue(status == ModelSmb2Status.STATUS_SUCCESS);
        }

        /// <summary>
        /// Session setup request send from client
        /// </summary>
        /// <param name="signingFlagType">Indicate if the request is signed</param>
        /// <param name="signingEnabledType">Indicate if SMB2_NEGOTIATE_SIGNING_ENABLED is set in SecurityMode field</param>
        /// <param name="signingRequiredType">Indicate if SMB2_NEGOTIATE_SIGNING_REQUIRED is set in SecurityMode field</param>
        /// <param name="userType">Indicate the type of user</param>
        [Rule]
        public static void SessionSetupRequest(
            SigningFlagType signingFlagType, 
            SigningEnabledType signingEnabledType, 
            SigningRequiredType signingRequiredType, 
            UserType userType)
        {
            Condition.IsTrue(State == ModelState.Connected);

            // Add isolate for SigningFlagType to reduce redundant cases: If the signingFlagType is set to SignedFlagSet, server will always return 
            // error code STATUS_USER_SESSION_DELETED while verifying signature, we don't need to cover all the invalid combination with other params.
            Combination.Isolated(signingFlagType == SigningFlagType.SignedFlagSet);

            ModelHelper.Log(LogType.Requirement, "3.3.5.5.1: The other values MUST be initialized as follows:");
            ModelHelper.Log(LogType.Requirement, "\tSession.SigningRequired is set to FALSE.");
            Session_SigningRequired = false;
            Request = new SigningModelRequest(signingFlagType);
        }

        /// <summary>
        /// Session setup Response from the server
        /// </summary>
        /// <param name="status">Status of the response</param>
        /// <param name="sessionId">Indicate if the SessionId field is zero</param>
        /// <param name="signingFlagType">Indicate if the response is signed</param>
        /// <param name="sessionFlag">Indicate the value of SessionFlags field in response</param>
        /// <param name="c">Config of server</param>
        [Rule]
        public static void SessionSetupResponse(
            ModelSmb2Status status, 
            SigningModelSessionId sessionId, 
            SigningFlagType signingFlagType, 
            SessionFlags_Values sessionFlag, 
            SigningConfig c)
        {
            Condition.IsTrue(State == ModelState.Connected);
            Condition.IsTrue(Config.IsServerSigningRequired == c.IsServerSigningRequired);

            SigningModelRequest sessionSetupRequest = ModelHelper.RetrieveOutstandingRequest<SigningModelRequest>(ref Request);

            if (!VerifySignature(status, sessionSetupRequest))
            {
                State = ModelState.Uninitialized;
                return;
            }

            if (sessionSetupRequest.signingFlagType == SigningFlagType.SignedFlagSet
                || (!sessionFlag.HasFlag(SessionFlags_Values.SESSION_FLAG_IS_GUEST)
                    && !Session_IsAnonymous 
                    && (Connection_ShouldSign || c.IsServerSigningRequired)))
            {
                ModelHelper.Log(LogType.Requirement,
                    "3.3.5.5.3: 5. Session.SigningRequired MUST be set to TRUE under the following conditions:");
                ModelHelper.Log(LogType.Requirement,
                    "\tIf the SMB2_NEGOTIATE_SIGNING_REQUIRED bit is set in the SecurityMode field of the client request.");
                ModelHelper.Log(LogType.Requirement,
                    "\tIf the SMB2_SESSION_FLAG_IS_GUEST bit is not set in the SessionFlags field " +
                    "and Session.IsAnonymous is FALSE and either Connection.ShouldSign or global RequireMessageSigning is TRUE.");

                ModelHelper.Log(LogType.TestInfo, 
                    "SMB2_NEGOTIATE_SIGNING_REQUIRED is {0}set.", sessionSetupRequest.signingFlagType == SigningFlagType.SignedFlagSet ? "" : "not ");
                ModelHelper.Log(LogType.TestInfo,
                    "SMB2_SESSION_FLAG_IS_GUEST bit is {0}set.", sessionFlag.HasFlag(SessionFlags_Values.SESSION_FLAG_IS_GUEST) ? "" : "not ");
                ModelHelper.Log(LogType.TestInfo, "Session.IsAnonymous is {0}.", Session_IsAnonymous);
                ModelHelper.Log(LogType.TestInfo, "Connection.ShouldSign is {0}.", Connection_ShouldSign);
                ModelHelper.Log(LogType.TestInfo, "Global RequireMessageSigning is {0}.", c.IsServerSigningRequired);
                ModelHelper.Log(LogType.TestInfo, "So Session.SigningRequired is set to TRUE.");

                Session_SigningRequired = true;
            }

            VerifyResponseShouldSign(status, sessionSetupRequest, sessionId, signingFlagType);           

            Condition.IsTrue(status == ModelSmb2Status.STATUS_SUCCESS);

            Session_IsExisted = true;
        }

        /// <summary>
        /// Tree Connect Request send to server
        /// </summary>
        /// <param name="signingFlagType">Indicate if the request is signed</param>
        [Rule]
        public static void TreeConnectRequest(SigningFlagType signingFlagType)
        {
            Condition.IsTrue(State == ModelState.Connected);

            Request = new SigningModelRequest(signingFlagType);
        }

        /// <summary>
        /// Tree Connect Response from server
        /// </summary>
        /// <param name="status">Status of the response</param>
        /// <param name="sessionId">Indicate if the SessionId field is zero</param>
        /// <param name="signingFlagType">Indicate if the response is signed</param>
        [Rule]
        public static void TreeConnectResponse(ModelSmb2Status status, SigningModelSessionId sessionId, SigningFlagType signingFlagType)
        {
            Condition.IsTrue(State == ModelState.Connected);

            SigningModelRequest treeConnectRequest = ModelHelper.RetrieveOutstandingRequest<SigningModelRequest>(ref Request);

            if (!VerifySignature(status, treeConnectRequest))
            {
                return;
            }
            
            VerifyResponseShouldSign(status, treeConnectRequest, sessionId, signingFlagType);

            Condition.IsTrue(status == ModelSmb2Status.STATUS_SUCCESS);            
        }
        #endregion

        #region Private Methods
        /// <summary>
        /// The server MUST sign the message under the following conditions
        /// </summary>
        private static void VerifyResponseShouldSign(
            ModelSmb2Status status, 
            SigningModelRequest request, 
            SigningModelSessionId sessionId, 
            SigningFlagType signingFlagType)
        {         
            if (request.signingFlagType == SigningFlagType.SignedFlagSet 
                && sessionId == SigningModelSessionId.NonZeroSessionId
                && Session_SigningRequired)
            {
                ModelHelper.Log(LogType.Requirement, "3.3.4.1.1: The server SHOULD<182> sign the message under the following conditions:");
                ModelHelper.Log(LogType.Requirement,
                    "\tIf the request was signed by the client, the response message being sent contains a nonzero SessionId and a zero TreeId in the SMB2 header, " +
                    "and the session identified by SessionId has Session.SigningRequired equal to TRUE.");
                ModelHelper.Log(LogType.TestInfo, "The condition is met.");
                Condition.IsTrue(signingFlagType == SigningFlagType.SignedFlagSet);
            }
            else if (request.signingFlagType == SigningFlagType.SignedFlagSet)
            {
                ModelHelper.Log(LogType.Requirement, "3.3.4.1.1: The server SHOULD<182> sign the message under the following conditions:");
                ModelHelper.Log(LogType.Requirement,
                    "\tIf the request was signed by the client, and the response is not an interim response to an asynchronously processed request.");
                ModelHelper.Log(LogType.TestInfo, "The condition is met.");
                Condition.IsTrue(signingFlagType == SigningFlagType.SignedFlagSet);
            }
        }

        /// <summary>
        /// Cover section 3.3.5.2.4
        /// </summary>
        private static bool VerifySignature(ModelSmb2Status status, SigningModelRequest request)
        {
            ModelHelper.Log(LogType.Requirement, "3.3.5.2.4: Verifying the Signature");
            if (request.signingFlagType == SigningFlagType.SignedFlagSet)
            {
                ModelHelper.Log(LogType.Requirement,
                    "If the SMB2 header of the request has SMB2_FLAGS_SIGNED set in the Flags field, the server MUST verify the signature. ");
                ModelHelper.Log(LogType.TestInfo, "SMB2_FLAGS_SIGNED is set in SMB2 header.");

                if (!Session_IsExisted)
                {
                    ModelHelper.Log(LogType.Requirement,
                        "For all other requests, the server MUST look up the session in the Connection.SessionTable using the SessionId in the SMB2 header of the request. " +
                        "If the session is not found, the request MUST be failed, as specified in section Sending an Error Response (section 3.3.4.4), " +
                        "with the error code STATUS_USER_SESSION_DELETED.");
                    ModelHelper.Log(LogType.TestInfo, "The session is not found.");
                    ModelHelper.Log(LogType.TestTag, TestTag.InvalidIdentifier);
                    Condition.IsTrue(status == ModelSmb2Status.STATUS_USER_SESSION_DELETED);
                    return false;
                }
            }

            if (request.signingFlagType == SigningFlagType.SignedFlagNotSet)
            {
                ModelHelper.Log(LogType.Requirement,
                    "If the SMB2 header of the request does not have SMB2_FLAGS_SIGNED set in the Flags field, " + 
                    "the server MUST determine if the client failed to sign a packet that required it. " + 
                    "The server MUST look up the session in the GlobalSessionTable using the SessionId in the SMB2 header of the request."); 
                ModelHelper.Log(LogType.TestInfo, "SMB2_FLAGS_SIGNED is not set in the SMB2 header.");

                if (Session_IsExisted && Session_SigningRequired)
                {
                    ModelHelper.Log(LogType.Requirement,
                        "If the session is found and Session.SigningRequired is equal to TRUE, the server MUST fail this request with STATUS_ACCESS_DENIED. ");
                    ModelHelper.Log(LogType.TestInfo, "The session is found and Session.SigningRequired is TRUE.");
                    ModelHelper.Log(LogType.TestTag, TestTag.UnexpectedFields);
                    Condition.IsTrue(status == ModelSmb2Status.STATUS_ACCESS_DENIED);
                    return false;
                }
            }

            return true;
        }
        #endregion
    }
}
