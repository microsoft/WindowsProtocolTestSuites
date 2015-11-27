// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Modeling;
using Microsoft.Protocols.TestSuites.FileSharing.Common.Adapter;
using Microsoft.Protocols.TestSuites.FileSharing.SMB2Model.Adapter;
using Microsoft.Protocols.TestSuites.FileSharing.SMB2Model.Adapter.ValidateNegotiateInfo;
using Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Smb2;
using Microsoft.Xrt.Runtime;

[assembly: NativeType("System.Diagnostics.Tracing.*")]
namespace Microsoft.Protocols.TestSuites.FileSharing.SMB2Model.Model.ValidateNegotiateInfo
{
    public static class ValidateNegotiateInfoModel
    {
        #region Fields
        /// <summary>
        /// Server configuration related to model
        /// </summary>
        public static ValidateNegotiateInfoConfig Config;

        /// <summary>
        /// The state of the model.
        /// </summary>
        public static ModelState State = ModelState.Uninitialized;

        /// <summary>
        /// Indicate if everyone of the fields(dialect, capabilities, securityMode, clientGuid) in ValidateNegotiateInfo request is the same with the one in Negotiate request.
        /// </summary>
        public static bool IsSameWithNegotiate = true;

        /// <summary>
        /// The dialect of SMB2 negotiated with the client. This value MUST be either "2.002", "2.100", "3.000".
        /// </summary>
        public static ModelDialectRevision Connection_Dialect;

        /// <summary>
        /// The capabilities of the client of this connection 
        /// </summary>
        public static ModelCapabilities Connection_ClientCapabilities;

        /// <summary>
        /// The security mode sent by the client in the SMB2 Negotiate request on this connection
        /// </summary>
        public static SecurityMode_Values Connection_ClientSecurityMode;

        #endregion

        #region Rules
        /// <summary>
        /// The call for reading configuration.
        /// </summary>
        [Rule(Action = "call ReadConfig(out _)")]
        public static void ReadConfigCall()
        {
            Condition.IsTrue(State == ModelState.Uninitialized);
        }

        /// <summary>
        /// The return for reading configuration.
        /// </summary>
        /// <param name="c">Server configuration related to model</param>
        [Rule(Action = "return ReadConfig(out c)")]
        public static void ReadConfigReturn(ValidateNegotiateInfoConfig c)
        {
            Condition.IsTrue(State == ModelState.Uninitialized);
            Condition.IsNotNull(c);
            Config = c;
            State = ModelState.Initialized;

            Condition.IsTrue(Config.ValidateNegotiateInfoSupported ==
                             ValidateNegotiateInfoInServer.NotSupportValidateNegotiateInfo ||
                             Config.ValidateNegotiateInfoSupported ==
                             ValidateNegotiateInfoInServer.SupportValidateNegotiateInfo);
        }

        /// <summary>
        /// Preparation of testing ValidateNegotiateInfo
        /// Model should save the three fields coming from Negotiate request.
        /// </summary>
        /// <param name="dialect">The list of SMB2 dialects supported by the client:Smb2002, Smb21, Smb30</param>
        /// <param name="capabilities">Capabilities of the client: None, FullCapabilitiesForNonSmb30, FullCapabilitiesForSmb30, AllBitsSet</param>
        /// <param name="securityMode">SecurityMode of the client: NONE, SMB2_NEGOTIATE_SIGNING_ENABLED, SMB2_NEGOTIATE_SIGNING_REQUIRED</param>
        [Rule]
        public static void SetupConnection(ModelDialectRevision dialect,
                                           ModelCapabilities capabilities,
                                           SecurityMode_Values securityMode)
        {
            Condition.IsTrue(State == ModelState.Initialized);

            // capabilities, securityMode should be isolated and combined with dialect separately.
            Combination.Interaction(dialect, capabilities);
            Combination.Interaction(dialect, securityMode);

            State = ModelState.Connected;
            Connection_Dialect = dialect;
            Connection_ClientCapabilities = capabilities;
            Connection_ClientSecurityMode = securityMode;
        }

        /// <summary>
        /// ValidateNegotiateInfoRequest sent by client
        /// Model should know if the four fields are the same with Negotiate request or not.
        /// </summary>
        /// <param name="dialectType">Type of the field "Dialect" in ValidateNegotiateInfo request</param>
        /// <param name="capabilitiesType">If the field "Capabilities" in ValidateNegotiateInfo request is same with the one in Negotiate request</param>
        /// <param name="securityModeType">If the field "SecurityMode" in ValidateNegotiateInfo request is same with the one in Negotiate request</param>
        /// <param name="clientGuidType">If the field "ClientGuid" in ValidateNegotiateInfo request is same with the one in Negotiate request</param>
        [Rule]
        public static void ValidateNegotiateInfoRequest(DialectType dialectType,
                                                        CapabilitiesType capabilitiesType,
                                                        SecurityModeType securityModeType,
                                                        ClientGuidType clientGuidType)
        {
            Condition.IsTrue(State == ModelState.Connected);

            // Those four parameters don’t need to be full-mesh. 
            Combination.Isolated(DialectType.None == dialectType);
            Combination.Isolated(DialectType.DialectDifferentFromNegotiate == dialectType);
            Combination.Isolated(capabilitiesType == CapabilitiesType.CapabilitiesDifferentFromNegotiate);
            Combination.Isolated(securityModeType == SecurityModeType.SecurityModeDifferentFromNegotiate);
            Combination.Isolated(clientGuidType == ClientGuidType.ClientGuidDifferentFromNegotiate);

            if (DialectType.DialectSameWithNegotiate != dialectType)
            {
                ModelHelper.Log(
                    LogType.Requirement,
                    "3.3.5.15.12: The server MUST determine the greatest common dialect between the dialects it implements and the Dialects array of the VALIDATE_NEGOTIATE_INFO request." +
                    " If no dialect is matched, or if the value is not equal to Connection.Dialect, the server MUST terminate the transport connection and free the Connection object");
                ModelHelper.Log(
                    LogType.TestInfo,
                    "No dialect is matched between the dialects it implements and the Dialects array of the VALIDATE_NEGOTIATE_INFO request, or if the value is not equal to Connection.Dialect");
                ModelHelper.Log(LogType.TestTag, TestTag.UnexpectedFields);
                IsSameWithNegotiate = false;
            }

            if (capabilitiesType == CapabilitiesType.CapabilitiesDifferentFromNegotiate)
            {
                ModelHelper.Log(
                    LogType.Requirement,
                    "3.3.5.15.12: If Connection.ClientCapabilities is not equal to the Capabilities received in the VALIDATE_NEGOTIATE_INFO request structure, the server MUST terminate the transport connection and free the Connection object");
                ModelHelper.Log(
                    LogType.TestInfo,
                    "Connection.ClientCapabilities is not equal to the Capabilities received in the VALIDATE_NEGOTIATE_INFO request");
                ModelHelper.Log(LogType.TestTag, TestTag.UnexpectedFields);
                IsSameWithNegotiate = false;
            }

            if (securityModeType == SecurityModeType.SecurityModeDifferentFromNegotiate)
            {
                ModelHelper.Log(
                    LogType.Requirement,
                    "3.3.5.15.12: If the SecurityMode received in the VALIDATE_NEGOTIATE_INFO request structure is not equal to Connection.ClientSecurityMode, the server MUST terminate the transport connection and free the Connection object");
                ModelHelper.Log(
                    LogType.TestInfo,
                    "SecurityMode received in the VALIDATE_NEGOTIATE_INFO request structure is not equal to Connection.ClientSecurityMode");
                ModelHelper.Log(LogType.TestTag, TestTag.UnexpectedFields);
                IsSameWithNegotiate = false;
            }

            if (clientGuidType == ClientGuidType.ClientGuidDifferentFromNegotiate)
            {
                ModelHelper.Log(
                    LogType.Requirement,
                    "3.3.5.15.12: If the Guid received in the VALIDATE_NEGOTIATE_INFO request structure is not equal to the Connection.ClientGuid, the server MUST terminate the transport connection and free the Connection object");
                ModelHelper.Log(
                    LogType.TestInfo,
                    "Guid received in the VALIDATE_NEGOTIATE_INFO request structure is not equal to the Connection.ClientGuid");
                ModelHelper.Log(LogType.TestTag, TestTag.UnexpectedFields);
                IsSameWithNegotiate = false;
            }

        }

        /// <summary>
        /// Return ValidateNegotiateInfoResponse to client with different status
        /// </summary>
        /// <param name="status">status of ValidateNegotiateInfoResponse</param>
        /// <param name="c">config</param>
        [Rule]
        public static void ValidateNegotiateInfoResponse(ModelSmb2Status status, ValidateNegotiateInfoConfig c)
        {
            Condition.IsTrue(State == ModelState.Connected);
            Condition.IsTrue(IsSameWithNegotiate);
            Condition.IsTrue(Config.Platform == c.Platform);

            State = ModelState.Disconnected;

            if (Config.ValidateNegotiateInfoSupported == ValidateNegotiateInfoInServer.SupportValidateNegotiateInfo)
            {
                ModelHelper.Log(
                    LogType.TestInfo,
                    "FSCTL_VALIDATE_NEGOTIATE_INFO is allowed on the server");

                Condition.IsTrue(status == ModelSmb2Status.STATUS_SUCCESS);
                return;
            }

            ModelHelper.Log(
                LogType.TestInfo,
                "FSCTL_VALIDATE_NEGOTIATE_INFO is not allowed on the server");

            if (Config.Platform != Platform.NonWindows)
            {
                ModelHelper.Log(
                    LogType.Requirement,
                    "3.3.5.15: The server SHOULD<299> fail the request with STATUS_NOT_SUPPORTED when an FSCTL is not allowed on the server");
                ModelHelper.Log(
                    LogType.TestInfo,
                    "SUT platform is {0}, so follow the SHOULD requirement", Config.Platform);
                ModelHelper.Log(LogType.TestTag, TestTag.Compatibility);
                Condition.IsTrue(status == ModelSmb2Status.STATUS_NOT_SUPPORTED);
            }
            else
            {
                ModelHelper.Log(
                    LogType.TestInfo,
                    "SUT platform is {0}, so only assert status is not equal to STATUS_SUCCESS", Config.Platform);

                Condition.IsFalse(status == ModelSmb2Status.STATUS_SUCCESS);
            }
        }

        [Rule]
        public static void TerminateConnection()
        {
            ModelHelper.Log(
                LogType.TestInfo,
                "Server MUST terminate the transport connection and free the Connection object");            
            Condition.IsTrue(State == ModelState.Connected);
            Condition.IsTrue(!IsSameWithNegotiate);
        }
        #endregion

    }
}