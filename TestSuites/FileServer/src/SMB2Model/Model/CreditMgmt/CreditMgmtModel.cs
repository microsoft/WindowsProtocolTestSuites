// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Modeling;
using Microsoft.Protocols.TestSuites.FileSharing.Common.Adapter;
using Microsoft.Protocols.TestSuites.FileSharing.SMB2Model.Adapter;
using Microsoft.Protocols.TestSuites.FileSharing.SMB2Model.Adapter.CreditMgmt;
using Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Smb2;
using Microsoft.Xrt.Runtime;

[assembly: NativeType("System.Diagnostics.Tracing.*")]
namespace Microsoft.Protocols.TestSuites.FileSharing.SMB2Model.Model.CreditMgmt
{
    /// <summary>
    /// This models behavior of credit management for SMB2 server
    /// Assumptions/Restrictions:
    ///     1. Underlying connection supports multi-credit request (i.e. no NetBIOS transport used)
    ///     2. Async/Compounded/Cancel requests are not covered, will cover in traditional test
    ///     3. 
    /// </summary>
    public static class CreditMgmtModel
    {
        #region State

        /// <summary>
        /// Server model state
        /// </summary>
        public static ModelState state = ModelState.Uninitialized;

        /// <summary>
        /// The dialect after negotiation
        /// </summary>
        public static DialectRevision negotiateDialect;

        /// <summary>
        /// Indicate if supports multi-credit request
        /// </summary>
        public static bool isMultiCreditSupported;

        /// <summary>
        /// Server configuration related to model
        /// </summary>
        public static CreditMgmtConfig config;

        /// <summary>
        /// Request that server model is handling
        /// </summary>
        public static ModelSMB2Request request;

        /// <summary>
        /// Assistant state. Indicate if the parameters result in server terminates connection
        /// </summary>
        public static bool expectDisconnection;

        /// <summary>
        /// Assistant state. Indicate if reaching an accepting state to make sure all ending states are what we expect
        /// </summary>
        public static bool acceptingCondition;

        #endregion

        #region Actions

        /// <summary>
        /// Call for loading server configuration
        /// </summary>
        [Rule(Action = "call ReadConfig(out _)")]
        public static void ReadConfigCall()
        {
            Condition.IsTrue(state == ModelState.Uninitialized);
        }

        /// <summary>
        /// Return for loading server configuration
        /// </summary>
        /// <param name="c">Server configuration related to model</param>
        [Rule(Action = "return ReadConfig(out c)")]
        public static void ReadConfigReturn(CreditMgmtConfig c)
        {
            Condition.IsTrue(state == ModelState.Uninitialized);
            Condition.IsNotNull(c);

            Condition.IsTrue(
                c.MaxSmbVersionSupported == ModelDialectRevision.Smb2002
                || c.MaxSmbVersionSupported == ModelDialectRevision.Smb21
                || c.MaxSmbVersionSupported == ModelDialectRevision.Smb30
                || c.MaxSmbVersionSupported == ModelDialectRevision.Smb302);

            negotiateDialect = DialectRevision.Smb2Unknown;
            config = c;
            request = null;
            state = ModelState.Initialized;
            acceptingCondition = false;
        }

        /// <summary>
        /// Setup connection by perform following
        ///     1. Negotiate
        ///     2. SessionSetup
        ///     3. TreeConnect
        ///     4. Create
        /// </summary>
        /// <param name="clientMaxDialect">Max SMB2 dialect that client supports</param>
        [Rule]
        public static void SetupConnection(ModelDialectRevision clientMaxDialect)
        {
            Condition.IsTrue(state == ModelState.Initialized);
            Condition.IsNull(request);

            negotiateDialect = ModelHelper.DetermineNegotiateDialect(clientMaxDialect, config.MaxSmbVersionSupported);

            if ((negotiateDialect == DialectRevision.Smb21 || ModelUtility.IsSmb3xFamily(negotiateDialect))
                && config.IsMultiCreditSupportedOnServer)
            {
                ModelHelper.Log(
                    LogType.Requirement,
                    "3.3.5.4: If the common dialect is SMB 2.1 or 3.x dialect family and the underlying connection is either TCP port 445 or RDMA," +
                    "Connection.SupportsMultiCredit MUST be set to TRUE; otherwise, it MUST be set to FALSE.");
                ModelHelper.Log(
                    LogType.TestInfo,
                    "Common dialect is {0} and server implementation {1} multicredit", negotiateDialect,
                    config.IsMultiCreditSupportedOnServer ? "supports" : "does not support");

                isMultiCreditSupported = true;

                ModelHelper.Log(
                    LogType.TestInfo,
                    "Connection.SupportsMultiCredit is set to TRUE");
            }
            else
            {
                isMultiCreditSupported = false;

                ModelHelper.Log(
                    LogType.TestInfo,
                    "Connection.SupportsMultiCredit is set to FALSE");
            }

            state = ModelState.Connected;
        }

        /// <summary>
        /// Disconnect the connection on receiving fault request
        /// </summary>
        [Rule]
        public static void ExpectDisconnect()
        {
            Condition.IsTrue(state == ModelState.Connected);
            Condition.IsTrue(expectDisconnection);

            state = ModelState.Disconnected;
            acceptingCondition = true;

        }

        /// <summary>
        /// Request for credit operation
        /// </summary>
        /// <param name="midType">Message Id in the request</param>
        /// <param name="creditCharge">Credit charge in the request</param>
        /// <param name="creditRequestNum">Requested credit # in the request</param>
        /// <param name="payloadSize">Payload size in the request/response</param>
        /// <param name="payloadType">Payload type indicating if it's payload for request or response</param>
        [Rule]
        public static void CreditOperationRequest(
            ModelMidType midType,
            ModelCreditCharge creditCharge,
            ModelCreditRequestNum creditRequestNum,
            ModelPayloadSize payloadSize,
            ModelPayloadType payloadType)
        {
            Condition.IsTrue(state == ModelState.Connected);
            Condition.IsNull(request);

            Combination.Isolated(midType == ModelMidType.UsedMid);
            Combination.Isolated(midType == ModelMidType.UnavailableMid);
            Combination.Isolated(creditCharge == ModelCreditCharge.CreditChargeExceedBoundary);
            Combination.Isolated(payloadSize == ModelPayloadSize.PayloadSizeLargerThanBoundary);

            //Pairwise the rest parameters
            Combination.NWise(2, creditCharge, creditRequestNum, payloadSize, payloadType);

            request = new ModelCreditOperationRequest(
                midType,
                creditCharge,
                creditRequestNum,
                payloadSize,
                payloadType);

            // NOTE: creditCharge will be ignored if multicredit is not supported
            if (midType == ModelMidType.UsedMid
                || midType == ModelMidType.UnavailableMid
                || (isMultiCreditSupported && creditCharge == ModelCreditCharge.CreditChargeExceedBoundary))
            {
                ModelHelper.Log(
                    LogType.Requirement,
                    "3.3.5.2.3: If the server determines that the MessageId or the range of MessageIds for the incoming request is not valid," +
                    " the server SHOULD<202> terminate the connection. Otherwise, the server MUST remove the MessageId or the range of MessageIds from the Connection.CommandSequenceWindow.");
                //Not add Platform!=NonWindows because NonWindows could also drop connection

                if (midType == ModelMidType.UsedMid || midType == ModelMidType.UnavailableMid)
                {
                    ModelHelper.Log(LogType.TestTag, TestTag.InvalidIdentifier);
                }

                if (isMultiCreditSupported && creditCharge == ModelCreditCharge.CreditChargeExceedBoundary)
                {
                    ModelHelper.Log(LogType.TestTag, TestTag.OutOfBoundary);
                }

                expectDisconnection = true;

                ModelHelper.Log(
                    LogType.TestInfo,
                    "Test case is expecting server disconnect the connection");
                ModelHelper.Log(
                    LogType.TestInfo,
                    "Connection.SupportsMultiCredit is set to {0}, messageId type is {1}, creditCharge type is {2}",
                    isMultiCreditSupported, midType, creditCharge);
                return;
            }

            if (!isMultiCreditSupported
                && payloadSize == ModelPayloadSize.PayloadSizeLargerThanBoundary)
            {
                ModelHelper.Log(
                    LogType.Requirement,
                    "3.3.5.2: If Connection.SupportsMultiCredit is FALSE and the size of the request is greater than 68*1024 bytes," +
                    " the server SHOULD<200> terminate the connection");
                //Ignore following product behavior as known issue for now
                ModelHelper.Log(
                    LogType.Requirement,
                    "<200> Section 3.3.5.2: Windows 7 without [MSKB-2536275], and Windows Server 2008 R2 without [MSKB-2536275] terminate the connection when the size of the request is greater than 64*1024 bytes." +
                    " Windows Vista SP1 and Windows Server 2008 on Direct TCP transport disconnect the connection if the size of the message exceeds 128*1024 bytes, and Windows Vista SP1 and Windows Server 2008 on NetBIOS over TCP transport will disconnect the connection if the size of the message exceeds 64*1024 bytes");
                
                ModelHelper.Log(LogType.TestTag, TestTag.OutOfBoundary);

                expectDisconnection = true;

                ModelHelper.Log(
                    LogType.TestInfo,
                    "Test case is expecting server to drop the connection");
                ModelHelper.Log(
                    LogType.TestInfo,
                    "Connection.SupportsMultiCredit is set to {0}, messageId type is {1}, creditCharge type is {2}",
                    isMultiCreditSupported, midType, creditCharge);

            }
        }

        /// <summary>
        /// Response for credit operation
        /// </summary>
        /// <param name="status">Status in response</param>
        /// <param name="creditResponse">Credit granted by server</param>
        /// <param name="c">Server configurations</param>
        [Rule]
        public static void CreditOperationResponse(ModelSmb2Status status, uint creditResponse, CreditMgmtConfig c)
        {
            Condition.IsTrue(state == ModelState.Connected);

            Condition.IsTrue(c.Platform == config.Platform);

            ModelCreditOperationRequest creditOperationRequest = ModelHelper.RetrieveOutstandingRequest<ModelCreditOperationRequest>(ref request);

            if (config.Platform != Platform.NonWindows)
            {
                if (creditOperationRequest.creditRequestNum == ModelCreditRequestNum.CreditRequestSetNonZero)
                {
                    ModelHelper.Log(
                        LogType.Requirement,
                        "3.3.1.2: The server SHOULD<151> grant the client a non-zero value of credits in response to any non-zero value requested");
                    ModelHelper.Log(
                        LogType.TestInfo,
                        "Platform is {0}", config.Platform);

                    Condition.IsTrue(creditResponse != 0);
                }
            }

            if (creditOperationRequest.midType == ModelMidType.UsedMid
                || creditOperationRequest.midType == ModelMidType.UnavailableMid
                || (isMultiCreditSupported && creditOperationRequest.creditCharge == ModelCreditCharge.CreditChargeExceedBoundary))
            {
                ModelHelper.Log(
                    LogType.Requirement,
                    "3.3.5.2.3: If the server determines that the MessageId or the range of MessageIds for the incoming request is not valid," +
                    " the server SHOULD<202> terminate the connection. Otherwise, the server MUST remove the MessageId or the range of MessageIds from the Connection.CommandSequenceWindow.");

                if (creditOperationRequest.midType == ModelMidType.UsedMid || creditOperationRequest.midType == ModelMidType.UnavailableMid)
                {
                    ModelHelper.Log(LogType.TestTag, TestTag.InvalidIdentifier);
                }

                if (isMultiCreditSupported && creditOperationRequest.creditCharge == ModelCreditCharge.CreditChargeExceedBoundary)
                {
                    ModelHelper.Log(LogType.TestTag, TestTag.OutOfBoundary);
                }

                //Only NonWindows would run into this case when not following the SHOULD requirement
                Condition.IsTrue(config.Platform == Platform.NonWindows);
                Condition.IsTrue(status != ModelSmb2Status.STATUS_SUCCESS);
                acceptingCondition = true;

                return;
            }

            if (isMultiCreditSupported)
            {
                ModelHelper.Log(
                    LogType.Requirement,
                    "3.3.5.2.5: If Connection.SupportsMultiCredit is TRUE," +
                    " the server MUST verify the CreditCharge field in the SMB2 header and the payload size (the size of the data within the variable-length field) of the request or the maximum response size");
                ModelHelper.Log(
                    LogType.TestInfo,
                    "Connection.SupportsMultiCredit is TRUE");

                if (creditOperationRequest.creditCharge == ModelCreditCharge.CreditChargeSetZero)
                {
                    //NOTE: When multi-credit request is not supported or credit charge = 0
                    //     Treat PayloadSize > 64K if use "LargerThanCreditCharge"
                    if (creditOperationRequest.payloadSize == ModelPayloadSize.PayloadSizeLargerThanBoundary)
                    {
                        ModelHelper.Log(
                            LogType.Requirement,
                            "If CreditCharge is zero and the payload size of the request or the maximum response size is greater than 64 kilobytes," +
                            " the server MUST fail the request with the error code STATUS_INVALID_PARAMETER.");
                        ModelHelper.Log(
                            LogType.TestInfo,
                            "Credit charge type in request is {0}, payload size type is {1}",
                            creditOperationRequest.creditCharge, creditOperationRequest.payloadSize);

                        ModelHelper.Log(LogType.TestTag, TestTag.OutOfBoundary);

                        Condition.IsTrue(status == ModelSmb2Status.STATUS_INVALID_PARAMETER);
                        acceptingCondition = true; //Reaching an accepting condition of exploration
                        return;
                    }
                }
                else
                {
                    if (creditOperationRequest.payloadSize == ModelPayloadSize.PayloadSizeLargerThanBoundary)
                    {
                        ModelHelper.Log(
                            LogType.Requirement,
                            "If CreditCharge is greater than zero, the server MUST calculate the expected CreditCharge for the current operation using the formula specified in section 3.1.5.2." +
                            " If the calculated credit number is greater than the CreditCharge, the server MUST fail the request with the error code STATUS_INVALID_PARAMETER.");
                        ModelHelper.Log(
                            LogType.TestInfo,
                            "Credit charge type in request is {0}, payload size type is {1}," +
                            " that's calculated credit number based on payload size is greater than the CreditCharge",
                            creditOperationRequest.creditCharge, creditOperationRequest.payloadSize);

                        ModelHelper.Log(LogType.TestTag, TestTag.OutOfBoundary);

                        Condition.IsTrue(status == ModelSmb2Status.STATUS_INVALID_PARAMETER);
                        acceptingCondition = true; //Reaching an accepting condition of exploration
                        return;
                    }
                }
            }

            Condition.IsTrue(status == ModelSmb2Status.STATUS_SUCCESS);
            acceptingCondition = true;

        }

        [AcceptingStateCondition]
        public static bool AcceptingCondtion()
        {
            return acceptingCondition;
        }
        
        #endregion
    }
}
