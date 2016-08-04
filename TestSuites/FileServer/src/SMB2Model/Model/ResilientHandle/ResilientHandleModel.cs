// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Text;
using Microsoft.Modeling;
using Microsoft.Protocols.TestSuites.FileSharing.Common.Adapter;
using Microsoft.Protocols.TestSuites.FileSharing.SMB2Model.Adapter;
using Microsoft.Protocols.TestSuites.FileSharing.SMB2Model.Adapter.ResilientHandle;
using Microsoft.Protocols.TestTools.StackSdk;
using Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Smb2;
using Microsoft.Xrt.Runtime;

[assembly: NativeType("System.Diagnostics.Tracing.*")]
namespace Microsoft.Protocols.TestSuites.FileSharing.SMB2Model.Model.ResilientHandle
{
    /// <summary>
    /// This models behavior of resilient handle for SMB2 server
    /// Assumptions/Restrictions/Notes:
    /// 1. Do not cover logic related to Lock/Unlock, which is in the scope of Lock/Unlock Model. 
    ///    The reason is that Open.IsResilient is the condition whether to compare SequenceNumber.
    /// 2. Do not cover logic related to Durable Handle, which is the scope of Handle Model.
    /// 2. Do not cover logic related to Resilient Open Scavenger Timer, which will be covered in traditional test case.
    /// 3. Do not cover logic related to feature combination of Resilient Handle and Lease/OpLock Break, which will be cover in future Model for feature combination. 
    /// 4. Do not cover logic related to feature combination of Persistent Handle and Resilient Handle, which will be covered in traditional test case.
    /// 5. Do not cover common logic related to Receiving IoCtl Request, which is the scope of IoCtrl Model.
    /// </summary>
    public static class ResilientHandleModel
    {
        #region State
        public static ResilientHandleServerConfig Config;

        public static ModelState State = ModelState.Uninitialized;

        public static DialectRevision Connection_Dialect;

        public static ModelSMB2Request Request = null;

        public static ModelOpen Open = null;

        #endregion

        #region Action

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
        public static void ReadConfigReturn(ResilientHandleServerConfig c)
        {
            Condition.IsTrue(State == ModelState.Uninitialized);

            Condition.IsTrue(c.MaxSmbVersionSupported == ModelDialectRevision.Smb2002
                || c.MaxSmbVersionSupported == ModelDialectRevision.Smb21
                || c.MaxSmbVersionSupported == ModelDialectRevision.Smb30
                || c.MaxSmbVersionSupported == ModelDialectRevision.Smb302);

            Config = c;
            State = ModelState.Initialized;
        }

        /// <summary>
        /// Connect, Negotiate, Session Set up, Tree Connect and Create
        /// </summary>
        /// <param name="clientMaxDialect"></param>
        /// <param name="isDurable">Indicate whether Durable Open is created successfully.</param>
        [Rule]
        public static void PrepareOpen(ModelDialectRevision clientMaxDialect, DurableHandle durableHandle)
        {
            Condition.IsTrue(State == ModelState.Initialized);

            Connection_Dialect = ModelHelper.DetermineNegotiateDialect(clientMaxDialect, Config.MaxSmbVersionSupported);

            State = ModelState.Connected;
            Open = new ModelOpen(durableHandle == DurableHandle.DurableHandle? true : false);
        }

        /// <summary>
        /// Client sends Resiliency Request
        /// </summary>
        /// <param name="inputCount">Indicate if the InputCount field is larger than the size of the input data.</param>
        /// <param name="timeout">Indicate if the TimeOut field is greater than MaxResiliencyTimeout.</param>
        [Rule]
        public static void IoCtlResiliencyRequest(
            IoCtlInputCount inputCount,
            ResilientTimeout timeout)
        {
            Condition.IsTrue(State == ModelState.Connected);
            Condition.IsNotNull(Open);
            Condition.IsNull(Request);

            Request = new ModelResiliencyRequest(inputCount, timeout);
        }

        /// <summary>
        /// Resiliency response from server
        /// </summary>
        /// <param name="status">Status of the response</param>
        /// <param name="c">Config of server</param>
        [Rule]
        public static void IoCtlResiliencyResponse(ModelSmb2Status status, ResilientHandleServerConfig c)
        {
            Condition.IsTrue(State == ModelState.Connected);
            Condition.IsNotNull(Open);

            Condition.IsTrue(Config.Platform == c.Platform);
            Condition.IsTrue(Config.MaxSmbVersionSupported == c.MaxSmbVersionSupported);
            Condition.IsTrue(Config.IsIoCtlCodeResiliencySupported == c.IsIoCtlCodeResiliencySupported);

            ModelResiliencyRequest resiliencyRequest = ModelHelper.RetrieveOutstandingRequest<ModelResiliencyRequest>(ref Request);

            ModelHelper.Log(LogType.Requirement, "3.3.5.15.9 Handling a Resiliency Request");
            ModelHelper.Log(LogType.Requirement, "This section applies only to servers that implement the SMB 2.1 or the SMB 3.x dialect family.");
            if (c.MaxSmbVersionSupported == ModelDialectRevision.Smb2002)
            {
                ModelHelper.Log(LogType.TestInfo, "The server only supports SMB 2.002.");
                ModelHelper.Log(LogType.TestTag, TestTag.Compatibility);
                return;
            }

            if (Connection_Dialect == DialectRevision.Smb2002)
            {
                ModelHelper.Log(LogType.Requirement, "If Open.Connection.Dialect is \"2.002\", the server MAY<320> fail the request with STATUS_INVALID_DEVICE_REQUEST.");
                ModelHelper.Log(LogType.TestInfo, "Open.Connection.Dialect is \"2.002\".");
                ModelHelper.Log(LogType.TestTag, TestTag.Compatibility);
                if (c.Platform == Platform.NonWindows)
                {
                    ModelHelper.Log(LogType.TestInfo, "The SUT platform is NonWindows.");
                    if (status == ModelSmb2Status.STATUS_INVALID_DEVICE_REQUEST)
                    {
                        return;
                    }
                }
                else
                {
                    ModelHelper.Log(LogType.TestInfo, "The SUT platform is Windows.");
                    Condition.IsFalse(status == ModelSmb2Status.STATUS_INVALID_DEVICE_REQUEST);
                }
                
            }

            if (!c.IsIoCtlCodeResiliencySupported)
            {
                ModelHelper.Log(LogType.Requirement,
                    "Otherwise, if the server does not support FSCTL_LMR_REQUEST_RESILIENCY requests, the server SHOULD fail the request with STATUS_NOT_SUPPORTED.");
                ModelHelper.Log(LogType.TestInfo,
                    "The server does not support FSCTL_LMR_REQUEST_RESILIENCY requests.");
                ModelHelper.Log(LogType.TestTag, TestTag.Compatibility);
                if (c.Platform == Platform.NonWindows)
                {// Non Windows
                    ModelHelper.Log(LogType.TestInfo, "The SUT platform is NonWindows.");
                    Condition.IsFalse(status == ModelSmb2Status.STATUS_SUCCESS);
                }
                else
                {// Windows
                    ModelHelper.Log(LogType.TestInfo, "The SUT platform is Windows.");
                    Condition.IsTrue(status == ModelSmb2Status.STATUS_NOT_SUPPORTED);
                    return;
                }
            }

            if (resiliencyRequest.InputCount == IoCtlInputCount.InputCountSmallerThanRequestSize
                || resiliencyRequest.Timeout == ResilientTimeout.InvalidTimeout)
            {
                ModelHelper.Log(LogType.Requirement,
                    "If InputCount is smaller than the size of the NETWORK_RESILIENCY_REQUEST request as specified in section 2.2.31.3, " +
                    "or if the requested Timeout in seconds is greater than MaxResiliencyTimeout in seconds, the request MUST be failed with STATUS_INVALID_PARAMETER.");
                ModelHelper.Log(LogType.TestInfo,
                    "InputCount is {0}smaller than the size of the NETWORK_RESILIENCY_REQUEST request.",
                    resiliencyRequest.InputCount == IoCtlInputCount.InputCountSmallerThanRequestSize ? "" : "not ");
                ModelHelper.Log(LogType.TestInfo,
                    "The requested Timeout is {0} greater than MaxResiliencyTimeout.",
                    resiliencyRequest.Timeout == ResilientTimeout.InvalidTimeout ? "" : "not ");
                ModelHelper.Log(LogType.TestTag, TestTag.OutOfBoundary);
                Condition.IsTrue(status == ModelSmb2Status.STATUS_INVALID_PARAMETER);
                return;
            }

            ModelHelper.Log(LogType.Requirement, "Open.IsDurable MUST be set to FALSE. Open.IsResilient MUST be set to TRUE. ");
            ModelHelper.Log(LogType.TestInfo, "Open.IsDurable is set to FALSE. Open.IsResilient is set to TRUE.");
            Open.IsDurable = false;
            Open.IsResilient = true;

            ModelHelper.Log(LogType.Requirement, 
                "Open.DurableOwner MUST be set to a security descriptor accessible only by the user represented by Open.Session.SecurityContext.");
            ModelHelper.Log(LogType.TestInfo, "Open.DurableOwner is set to default user.");
            Open.DurableOwner = ModelUser.DefaultUser; // In Resilient Handle Mode, Resilient always be requested as default user

            Condition.IsTrue(status == ModelSmb2Status.STATUS_SUCCESS);
        }

        /// <summary>
        /// Log off from server
        /// </summary>
        [Rule]
        public static void LogOff()
        {
            Condition.IsTrue(State == ModelState.Connected);
            Condition.IsNull(Request);
            Condition.IsNotNull(Open);

            if (!Open.IsResilient && !Open.IsDurable)
            {
                ModelHelper.Log(LogType.Requirement,
                    "3.3.5.6: The server MUST close every Open in Session.OpenTable of the old session, " +
                    "where Open.IsDurable is FALSE and Open.IsResilient is FALSE, as specified in section 3.3.4.17.");
                ModelHelper.Log(LogType.TestInfo, "Open.IsDurable is FALSE and Open.IsResilient is FALSE, so the open is closed.");
                Open = null;
            }
        }

        /// <summary>
        /// Disconnect from server
        /// </summary>
        [Rule]
        public static void Disconnect()
        {
            Condition.IsTrue(State == ModelState.Connected);
            Condition.IsNull(Request);
            Condition.IsNotNull(Open);

            ModelHelper.Log(LogType.Requirement, "3.3.7.1 Handling Loss of a Connection");
            if (Open.IsResilient)
            {
                ModelHelper.Log(LogType.Requirement,
                    "If any of the following conditions is satisfied, it indicates that the Open is to be preserved for reconnect. ");
                ModelHelper.Log(LogType.Requirement, "\tOpen.IsResilient is TRUE.");     
                ModelHelper.Log(LogType.TestInfo, "The above conditions are met.");
                ModelHelper.Log(LogType.TestInfo, "The Open is preserved.");
            }
            else if (Open.IsDurable)
            {
                ModelHelper.Log(LogType.Requirement,
                    "If any of the following conditions is satisfied, it indicates that the Open is to be preserved for reconnect. ");
                ModelHelper.Log(LogType.Requirement, "\tOpen.OplockLevel is equal to SMB2_OPLOCK_LEVEL_BATCH and Open.OplockState is equal to Held, and Open.IsDurable is TRUE.");
                ModelHelper.Log(LogType.TestInfo, "The above conditions are met.");
                ModelHelper.Log(LogType.TestInfo, "The Open is preserved.");
            }
            else
            {
                ModelHelper.Log(LogType.Requirement, "If the Open is not to be preserved for reconnect, the server MUST close the Open as specified in section 3.3.4.17.");
                ModelHelper.Log(LogType.TestInfo, "The Open is closed.");
                Open = null;
            }

            State = ModelState.Disconnected;
        }

        /// <summary>
        /// Reconnect the resilient open
        /// </summary>
        /// <param name="user">Indicate if the user when reconnecting is the same when creating.</param>
        [Rule]
        public static void ReEstablishResilientOpenRequest(ModelUser user)
        {
            Condition.IsTrue(Request == null);

            /// restrict parameter combination
            Condition.IfThen(Open == null, user == ModelUser.DefaultUser);

            Request = new ModelReEstablishResilientOpenRequest(user);
        }

        /// <summary>
        /// Reconnect response
        /// </summary>
        /// <param name="status">Status of the response</param>
        [Rule]
        public static void ReEstablishResilientOpenResponse(ModelSmb2Status status)
        {
            if (Open == null)
            {
                ModelHelper.Log(LogType.Requirement,
                    "3.3.5.9.7: 1. The server MUST look up an existing open in the GlobalOpenTable by doing a lookup with the FileId.Persistent portion of the create context. " +
                    "If the lookup fails, the server SHOULD<268> fail the request with STATUS_OBJECT_NAME_NOT_FOUND " +
                    "and proceed as specified in \"Failed Open Handling\" in section 3.3.5.9.");
                ModelHelper.Log(LogType.TestInfo, "The lookup fails.");
                ModelHelper.Log(LogType.TestTag, TestTag.InvalidIdentifier);
                Condition.IsTrue(status == ModelSmb2Status.STATUS_OBJECT_NAME_NOT_FOUND);
                return;
            }

            if (!Open.IsResilient && !Open.IsDurable)
            {
                ModelHelper.Log(LogType.Requirement, 
                    "3.3.5.9.7: 5. If Open.IsDurable is FALSE and Open.IsResilient is FALSE or unimplemented, " + 
                    "the server MUST fail the request with STATUS_OBJECT_NAME_NOT_FOUND and proceed as specified in \"Failed Open Handling\" in section 3.3.5.9");
                ModelHelper.Log(LogType.TestInfo, "Open.IsDurable is FALSE and Open.IsResilient is FALSE.");
                ModelHelper.Log(LogType.TestTag, TestTag.Compatibility);
                Condition.IsTrue(status == ModelSmb2Status.STATUS_OBJECT_NAME_NOT_FOUND);
                return;
            }

            ModelReEstablishResilientOpenRequest reEstablishRequest =
                ModelHelper.RetrieveOutstandingRequest<ModelReEstablishResilientOpenRequest>(ref Request);
            if (Open.DurableOwner != reEstablishRequest.User)
            {
                ModelHelper.Log(LogType.Requirement, 
                    "3.3.5.9.7: 9. If the user represented by Session.SecurityContext is not the same user denoted by Open.DurableOwner, " + 
                    "the server MUST fail the request with STATUS_ACCESS_DENIED and proceed as specified in \"Failed Open Handling\" in section 3.3.5.9.");
                ModelHelper.Log(LogType.TestInfo, "The user is different.");
                ModelHelper.Log(LogType.TestTag, TestTag.Compatibility);
                Condition.IsTrue(status == ModelSmb2Status.STATUS_ACCESS_DENIED);
                return;
            }

            Condition.IsTrue(status == ModelSmb2Status.STATUS_SUCCESS);
        }

        #endregion
    }
}
