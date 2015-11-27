// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Modeling;
using Microsoft.Protocols.TestSuites.FileSharing.SMB2Model.Adapter;
using Microsoft.Protocols.TestSuites.FileSharing.SMB2Model.Adapter.Negotiate;
using Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Smb2;
using Microsoft.Xrt.Runtime;

[assembly: NativeType("System.Diagnostics.Tracing.*")]
namespace Microsoft.Protocols.TestSuites.FileSharing.SMB2Model.Model.Negotiate
{
    public static class NegotiateModel
    {
        #region Fields
        /// <summary>
        /// The state of the model.
        /// </summary>
        public static ModelState State = ModelState.Uninitialized;

        /// <summary>
        /// The configuration for negotiate.
        /// </summary>
        public static NegotiateServerConfig Config;

        /// <summary>
        /// SMB2 request.
        /// </summary>
        public static ModelSMB2Request Request;

        /// <summary>
        /// The dialect revision after negotiation.
        /// </summary>
        public static DialectRevision NegotiateDialect;

        /// <summary>
        /// The identification of the message.
        /// </summary>
        public static ulong MessageId;
        #endregion

        #region Actions

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
        /// <param name="c">The configuration related to negotiate.</param>
        [Rule(Action = "return ReadConfig(out c)")]
        public static void ReadConfigReturn(NegotiateServerConfig c)
        {
            Condition.IsNotNull(c);
            Condition.IsTrue(c.MaxSmbVersionSupported == DialectRevision.Smb2002 ||
                             c.MaxSmbVersionSupported == DialectRevision.Smb21 ||
                             c.MaxSmbVersionSupported == DialectRevision.Smb30 ||
                             c.MaxSmbVersionSupported == DialectRevision.Smb302);
            Config = c;
            State = ModelState.Initialized;
        }

        /// <summary>
        /// Setup connection.
        /// </summary>
        [Rule]
        public static void SetupConnection()
        {
            Condition.IsTrue(State == ModelState.Initialized);

            Request = null;
            ModelHelper.Log(
                LogType.Requirement,
                "3.3.5.1: Connection.CommandSequenceWindow is set to a sequence window, as specified in section 3.3.1.1, with a starting receive sequence of 0 and a window size of 1");

            MessageId = 0;

            ModelHelper.Log(
                LogType.TestInfo,
                "Connection.CommandSequenceWindow to receive sequence of 0.");

            ModelHelper.Log(
                LogType.Requirement,
                "Connection.NegotiateDialect is set to 0xFFFF");

            NegotiateDialect = DialectRevision.Smb2Unknown;

            ModelHelper.Log(
                LogType.TestInfo,
                "Connection.NegotiateDialect is set to {0}", NegotiateDialect);

            State = ModelState.Connected;
        }

        /// <summary>
        /// Expect that the server disconnects the connection.
        /// </summary>
        [Rule]
        public static void ExpectDisconnect()
        {
            Condition.IsTrue(State == ModelState.Disconnected);
            Condition.IsNotNull(Request);

            State = ModelState.Uninitialized;
        }

        /// <summary>
        /// ComNegotiate request.
        /// </summary>
        /// <param name="dialects">Dialects.</param>
        [Rule(Action = "ComNegotiateRequest(dialects)")]
        public static void ComNegotiateRequest(Sequence<string> dialects)
        {
            Condition.IsTrue(State == ModelState.Connected);
            Condition.IsNull(Request);

            Request = new ModelComNegotiateRequest(dialects);

            if (MessageId > 0)
            {
                // Negotiate request is always expected to be the first message in a connection
                State = ModelState.Disconnected;
                return;
            }

            ModelHelper.Log(
                LogType.Requirement,
                "3.3.5.3: This request is defined in [MS-SMB] section 2.2.4.5.1, with the SMB header defined in section 2.2.3.1." +
                " If the request matches the format described there, and Connection.NegotiateDialect is 0xFFFF, processing MUST continue as specified in 3.3.5.3.1.");

            if (NegotiateDialect != DialectRevision.Smb2Unknown)
                //|| (!dialects.Contains(SMBDialects.SMB_2_002) && !dialects.Contains(SMBDialects.SMB_2_X))) // TODO: more comments?
            {
                ModelHelper.Log(
                    LogType.Requirement,
                    "3.3.5.3: Otherwise, the server MUST disconnect the connection, as specified in section 3.3.7.1, without sending a response.");
                ModelHelper.Log(LogType.TestTag, TestTag.Compatibility);
                State = ModelState.Disconnected;
                return;
            }

            if (!(Config.MaxSmbVersionSupported == DialectRevision.Smb21 || ModelUtility.IsSmb3xFamily(Config.MaxSmbVersionSupported)))
            {
                ModelHelper.Log(
                    LogType.Requirement,
                    "3.3.5.3.1: If the server does not implement the SMB 2.1 or 3.x dialect family, processing MUST continue as specified in 3.3.5.3.2.");

                ComNegotiateHandleSmb2002InRequest(dialects);
            }
            else
            {
                ModelHelper.Log(
                    LogType.Requirement,
                    "3.3.5.3.1: Otherwise, the server MUST scan the dialects provided for the dialect string \"SMB 2.???\".");

                if (!dialects.Contains(SMBDialects.SMB_2_X))
                {
                    ModelHelper.Log(
                        LogType.Requirement,
                        "3.3.5.3.1: If the string is not present, continue to section 3.3.5.3.2.");

                    ComNegotiateHandleSmb2002InRequest(dialects);
                }
                else
                {
                    ModelHelper.Log(
                        LogType.Requirement,
                        "3.3.5.3.1: If the string is present, the server MUST respond with an SMB2 NEGOTIATE Response as specified in 2.2.4");
                    ModelHelper.Log(
                        LogType.TestInfo,
                        "\"SMB 2.???\" is present in the dialects provided");
                }
            }

            // After every successful call, messageId MUST be incremented by 1. 
            // But in this model it is set to 1 to avoid to generate duplicate cases which only have different messageId.
            MessageId = 1;
        }

        /// <summary>
        /// Negotiate request.
        /// </summary>
        /// <param name="dialects">Dialects.</param>
        [Rule(Action = "NegotiateRequest(dialects)")]
        public static void NegotiateRequest(Sequence<DialectRevision> dialects)
        {
            Condition.IsTrue(State == ModelState.Connected);
            Condition.IsNull(Request);

            Request = new NegotiateRequest(dialects);

            if (NegotiateDialect == DialectRevision.Smb2002
                || NegotiateDialect == DialectRevision.Smb21
                || NegotiateDialect == DialectRevision.Smb30
                || NegotiateDialect == DialectRevision.Smb302)
            {
                ModelHelper.Log(
                    LogType.Requirement,
                    "3.3.5.4: If Connection.NegotiateDialect is 0x0202, 0x0210, 0x0300, or 0x0302 the server MUST disconnect the connection, as specified in section 3.3.7.1, and not reply.");
                ModelHelper.Log(
                    LogType.TestInfo,
                    "Connection.NegotiateDialect is {0}", NegotiateDialect);
                ModelHelper.Log(LogType.TestTag, TestTag.Compatibility);
                State = ModelState.Disconnected;
                return;
            }

            // After every successful call, messageId MUST be incremented by 1. 
            // But in this model it is set to 1 to avoid to generate duplicate cases which only have different messageId.
            MessageId = 1;
        }

        /// <summary>
        /// Negotiate response. 
        /// </summary>
        /// <param name="status">Status in the response.</param>
        /// <param name="dialectRevision">DialectRevision in the response.</param>
        [Rule(Action = "NegotiateResponse(status, dialectRevision)")]
        public static void NegotiateResponse(ModelSmb2Status status, DialectRevision dialectRevision)
        {
            Condition.IsTrue(State == ModelState.Connected);
            Condition.IsTrue(Request is ModelComNegotiateRequest || Request is NegotiateRequest);

            // Avoid "Microsoft.SpecExplorer.Runtime.Testing.UnboundVariableException: Variable's value cannot be read before it is bound"
            Condition.IsTrue(dialectRevision == DialectRevision.Smb2002 || dialectRevision == DialectRevision.Smb21 ||
                             dialectRevision == DialectRevision.Smb30 || dialectRevision == DialectRevision.Smb302 ||
                             dialectRevision == DialectRevision.Smb2Wildcard ||
                             dialectRevision == DialectRevision.Smb2Unknown);
            
            if (Request is ModelComNegotiateRequest)
            {
                ModelComNegotiateRequest comNegotiateReq = ModelHelper.RetrieveOutstandingRequest<ModelComNegotiateRequest>(ref Request);

                if (Config.MaxSmbVersionSupported != DialectRevision.Smb21 && !ModelUtility.IsSmb3xFamily(Config.MaxSmbVersionSupported))
                {
                    ModelHelper.Log(
                        LogType.Requirement,
                        "3.3.5.3.1: If the server does not implement the SMB 2.1 or 3.x dialect family, processing MUST continue as specified in 3.3.5.3.2.");

                    ComNegotiateHandleSmb2002InResponse(dialectRevision);
                }
                else
                {
                    ModelHelper.Log(
                        LogType.Requirement,
                        "3.3.5.3.1: Otherwise, the server MUST scan the dialects provided for the dialect string \"SMB 2.???\".");

                    if (comNegotiateReq.Dialects.Contains(SMBDialects.SMB_2_X))
                    {
                        ModelHelper.Log(
                            LogType.Requirement,
                            "3.3.5.3.1: If the string is present, the server MUST respond with an SMB2 NEGOTIATE Response as specified in 2.2.4.");

                        ModelHelper.Log(
                            LogType.Requirement,
                            "3.3.5.3.1: DialectRevision MUST be set to 0x02FF");

                        NegotiateDialect = DialectRevision.Smb2Wildcard;

                        ModelHelper.Log(
                            LogType.Requirement,
                            "3.3.5.3.1: Connection.NegotiateDialect MUST be set to 0x02FF, and the response is sent to the client");

                        Condition.IsTrue(dialectRevision == DialectRevision.Smb2Wildcard);
                    }
                    else
                    {
                        ModelHelper.Log(
                            LogType.Requirement,
                            "3.3.5.3.1: If the string is not present, continue to section 3.3.5.3.2");

                        ComNegotiateHandleSmb2002InResponse(dialectRevision);
                    }
                }
            }
            else
            {
                NegotiateRequest negotiateReq = ModelHelper.RetrieveOutstandingRequest<NegotiateRequest>(ref Request);

                if (negotiateReq.Dialects.Count == 0)
                {
                    ModelHelper.Log(
                        LogType.Requirement,
                        "3.3.5.4: If the DialectCount of the SMB2 NEGOTIATE Request is 0, the server MUST fail the request with STATUS_INVALID_PARAMETER");
                    ModelHelper.Log(
                        LogType.TestInfo,
                        "DialectCount of the SMB2 NEGOTIATE Request is 0");
                    ModelHelper.Log(LogType.TestTag, TestTag.UnexpectedFields);
                    Condition.IsTrue(status == ModelSmb2Status.STATUS_INVALID_PARAMETER);
                    return;
                }

                ModelHelper.Log(
                    LogType.Requirement,
                    "3.3.5.4: The server MUST select the greatest common dialect between the dialects it implements and the Dialects array of the SMB2 NEGOTIATE request");

                DialectRevision commonDialect = SelectCommonDialect(negotiateReq.Dialects);

                ModelHelper.Log(
                    LogType.TestInfo,
                    "Common dialect is {0}", commonDialect);
                if (commonDialect == DialectRevision.Smb2Unknown)
                {
                    ModelHelper.Log(
                        LogType.Requirement,
                        "3.3.5.4: If a common dialect is not found, the server MUST fail the request with STATUS_NOT_SUPPORTED");
                    ModelHelper.Log(LogType.TestTag, TestTag.UnexpectedFields);
                    Condition.IsTrue(status == ModelSmb2Status.STATUS_NOT_SUPPORTED);
                    return;
                }

                ModelHelper.Log(
                    LogType.Requirement,
                    "3.3.5.4: If a common dialect is found, the server MUST set Connection.Dialect to \"2.002\", \"2.100\", \"3.000\", or \"3.002\"," +
                    " and Connection.NegotiateDialect to 0x0202, 0x0210, 0x0300, or 0x0302 accordingly, to reflect the dialect selected");

                NegotiateDialect = commonDialect;

                ModelHelper.Log(
                    LogType.TestInfo,
                    "Connection.Dialect is set to {0}", NegotiateDialect);

                ModelHelper.Log(
                    LogType.Requirement,
                    "3.3.5.4: The server MUST then construct an SMB2 NEGOTIATE Response, as specified in section 2.2.4, with the following specific values, and return STATUS_SUCCESS to the client.");
                ModelHelper.Log(
                    LogType.Requirement,
                    "\tDialectRevision MUST be set to the common dialect");

                Condition.IsTrue(dialectRevision == commonDialect);
            }

            Condition.IsTrue(status == Smb2Status.STATUS_SUCCESS);

        }

        #endregion

        #region Private Methods
        /// <summary>
        /// Select the greatest common dialect between the dialects it implements and the Dialects array of the SMB2 NEGOTIATE request.
        /// </summary>
        /// <param name="requestDialects">The Dialects array of the SMB2 NEGOTIATE request.</param>
        /// <returns>Return a common dialect if found, otherwise return DialectRevision.Smb2Unknown(0xFFFF).</returns>
        private static DialectRevision SelectCommonDialect(Sequence<DialectRevision> requestDialects)
        {
            if (Config.MaxSmbVersionSupported >= DialectRevision.Smb302
                && requestDialects.Contains(DialectRevision.Smb302))
            {
                return DialectRevision.Smb302;
            }
            else if (Config.MaxSmbVersionSupported >= DialectRevision.Smb30
                && requestDialects.Contains(DialectRevision.Smb30))
            {
                return DialectRevision.Smb30;
            }
            else if (Config.MaxSmbVersionSupported >= DialectRevision.Smb21
                && requestDialects.Contains(DialectRevision.Smb21))
            {
                return DialectRevision.Smb21;
            }
            else if (Config.MaxSmbVersionSupported >= DialectRevision.Smb2002
                && requestDialects.Contains(DialectRevision.Smb2002))
            {
                return DialectRevision.Smb2002;
            }
            return DialectRevision.Smb2Unknown;
        }

        private static void ComNegotiateHandleSmb2002InRequest(Sequence<string> dialects)
        {
            ModelHelper.Log(
                LogType.Requirement,
                "3.3.5.3.2: The server MUST scan the dialects provided for the dialect string \"SMB 2.002\".<217>");

            if (dialects.Contains(SMBDialects.SMB_2_002))
            {
                ModelHelper.Log(
                    LogType.Requirement,
                    "3.3.5.3.2: If the string is present, the client understands SMB2, and the server MUST respond with an SMB2 NEGOTIATE Response.");
            }
            else
            {
                // Assume SUT does not implement SMB
                ModelHelper.Log(
                    LogType.Requirement,
                    "3.3.5.3.2: If the string is not present in the dialect list and the server does not implement SMB," +
                    " the server MUST disconnect the connection, as specified in section 3.3.7.1, without sending a response.");

                State = ModelState.Disconnected;
            }
        }

        private static void ComNegotiateHandleSmb2002InResponse(DialectRevision dialectRevision)
        {
            ModelHelper.Log(
                LogType.Requirement,
                "3.3.5.3.2: DialectRevision MUST be set to 0x0202.<218>");
            //<218> Section 3.3.5.3.2: A Windows Vista RTM–based server sets DialectRevision to 6

            Condition.IsTrue(dialectRevision == DialectRevision.Smb2002);

            ModelHelper.Log(
                LogType.Requirement,
                "3.3.5.3.2: Connection.Dialect MUST be set to \"2.002\", Connection.NegotiateDialect MUST be set to 0x0202, and the response is sent to the client");

            NegotiateDialect = DialectRevision.Smb2002;
        }

        #endregion
    }
}
