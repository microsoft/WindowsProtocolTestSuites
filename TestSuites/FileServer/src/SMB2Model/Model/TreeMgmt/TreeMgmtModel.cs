// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Modeling;
using Microsoft.Protocols.TestSuites.FileSharing.SMB2Model.Adapter;
using Microsoft.Protocols.TestSuites.FileSharing.SMB2Model.Adapter.TreeMgmt;
using Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Smb2;
using Microsoft.Xrt.Runtime;

[assembly: NativeType("System.Diagnostics.Tracing.*")]
namespace Microsoft.Protocols.TestSuites.FileSharing.SMB2Model.Model.TreeMgmt
{
    /// <summary>
    /// This models behavior of tree management for SMB2 server
    /// Assumptions/Restrictions/Notes:
    /// 1. This model only contains single session with single tree connect.
    /// 2. Does not cover encryption related logic which has been covered in Encryption model.
    /// 3. Does not cover session related logic which has been covered in Session Management model.
    /// 4. Does not cover validation of Negotiate logic which has been covered in Validate Negotiate Info model.
    /// 5. Does not cover signing related logic which will be covered in Signing model.
    /// 6. Does not cover Share Flags SMB2_SHAREFLAG_DFS and SMB2_SHAREFLAG_DFS_ROOT and Capability SMB2_SHARE_CAP_DFS in TREE_CONNECT response, 
    ///       which is in the scope of DFS test suite.
    /// 7. Does not cover Capabilities SMB2_SHARE_CAP_SCALEOUT, SMB2_SHARE_CAP_CLUSTER and SMB2_SHARE_CAP_CONTINUOUS_AVAILABILITY
    ///       in TREE_CONNECT response, which is in the scope of Cluster test suite.
    /// 8. Does not cover Share Flag SMB2_SHAREFLAG_ENABLE_HASH in TREE_CONNECT response, which is in the 
    ///       scope of BranchCache test suite.
    /// 9. Does not cover Share Flag SMB2_SHAREFLAG_FORCE_LEVELII_OPLOCK in TREE_CONNECT response, 
    ///       which is in the scope of Leasing model and OpLock model.
    /// 10. Does not cover Share Flag SMB2_SHAREFLAG_ACCESS_BASED_DIRECTORY_ENUM in TREE_CONNECT response, which is in the scope of Set/Query Info model.
    /// 11. Does not cover Share Flags SMB2_SHAREFLAG_RESTRICT_EXCLUSIVE_OPENS and SMB2_SHAREFLAG_FORCE_SHARED_DELETE 
    ///       and MaximalAccess in TREE_CONNECT response, which is in the scope of Create/Close Model.
    /// 12. Does not cover Offline. It costs much effort to change the SUT control adapter for one not important feature.
    /// 13. Only cover ShareType.Disk. Currently Share Type "Pipe" and "Print" is not in the scope of FileSharing Test Suite Family.
    /// 14. Does not cover detail test of Share Path(example, server name is NetBIOS, FQDN or IP address) in TreeConnect request, which is in the scope of MS-SRVS.
    /// </summary>
    public class TreeMgmtModel
    {
        #region State
        public static TreeMgmtServerConfig Config;

        /// <summary>
        /// The state of the model.
        /// </summary>
        public static ModelState State = ModelState.Uninitialized;

        public static ModelSMB2Request Request;

        /// <summary>
        /// Indicate whether the TreeConnect exists in the Session.TreeConnectTable
        /// </summary>
        public static bool Session_TreeConnectExist = false;

        /// <summary>
        /// Session.SecurityContext. User that authenticated this session.
        /// </summary>
        public static ModelSessionSecurityContext Session_SecurityContext;

        #endregion

        #region Action

        [Rule(Action = "call ReadConfig(out _)")]
        public static void ReadConfig()
        {
            Condition.IsTrue(State == ModelState.Uninitialized);
        }

        /// <summary>
        /// Return for loading server configuration
        /// </summary>
        /// <param name="c">Server configuration related to model</param>
        [Rule(Action = "return ReadConfig(out c)")]
        public static void ReadConfigReturn(TreeMgmtServerConfig c)
        {
            Condition.IsTrue(State == ModelState.Uninitialized);

            Config = c;
            State = ModelState.Initialized;
        }

        /// <summary>
        /// Prepare Tree by perform following
        ///     1. Negotiate
        ///     2. Session Setup
        /// </summary>
        [Rule]
        public static void SetupConnection(ModelSessionSecurityContext securityContext)
        {
            Condition.IsTrue(State == ModelState.Initialized);
            Condition.IsFalse(Session_TreeConnectExist);

            State = ModelState.Connected;
            Session_TreeConnectExist = false;
            Session_SecurityContext = securityContext;
        }

        /// <summary>
        /// request of TREE_CONNECT
        /// </summary>
        [Rule]
        public static void TreeConnectRequest(ModelSharePath sharePath)
        {
            Condition.IsTrue(State == ModelState.Connected);
            Condition.IsFalse(Session_TreeConnectExist);
            Condition.IsNull(Request);

            // If Security Context is Non Admin, InvalidPath is not used.
            Condition.IfThen(
                Session_SecurityContext == ModelSessionSecurityContext.NonAdmin,
                sharePath != ModelSharePath.InvalidSharePath);

            Request = new ModelTreeMgmtTreeConnectRequest(sharePath);
        }

        /// <summary>
        /// Response of TREE_CONNECT
        /// </summary>
        [Rule]
        public static void TreeConnectResponse(ModelSmb2Status status, ShareType_Values shareType, TreeMgmtServerConfig config)
        {
            Condition.IsTrue(Config.Platform == config.Platform);
            Condition.IsTrue(State == ModelState.Connected);

            ModelTreeMgmtTreeConnectRequest treeConnectRequest = ModelHelper.RetrieveOutstandingRequest<ModelTreeMgmtTreeConnectRequest>(ref Request);

            if (treeConnectRequest.sharePath == ModelSharePath.InvalidSharePath )
            {
                ModelHelper.Log(
                    LogType.Requirement,
                    "3.3.5.7: Otherwise, the server MUST provide the tuple <hostname, sharename> parsed from the request message to invoke the event specified in [MS-SRVS] section 3.1.6.8," +
                    " to normalize the hostname by resolving server aliases and evaluating share scope. The server MUST use <normalized hostname, sharename> to look up the Share in ShareList." +
                    " If no share with a matching share name and server name is found, the server MUST fail the request with STATUS_BAD_NETWORK_NAME");
                ModelHelper.Log(
                    LogType.TestInfo,
                    "Share path is an invalid share path");
                ModelHelper.Log(LogType.TestTag, TestTag.UnexpectedFields);
                Condition.IsTrue(status == ModelSmb2Status.STATUS_BAD_NETWORK_NAME);
                return;
            }

            // Only cover Windows behavior (SHOULD behavior) in one condition when the share is "IPC$" or has STYPE_SPECIAL bit set
            // Other cases for 3.3.5.7 statement below are not covered
            // Assume NonWindows will have same behvior regarding to 3.3.4.13 statement
            if (Session_SecurityContext == ModelSessionSecurityContext.NonAdmin
                && treeConnectRequest.sharePath == ModelSharePath.SpecialSharePath)
            {
                ModelHelper.Log(
                    LogType.Requirement,
                    "3.3.5.7: The server MUST determine whether the user represented by Session.SecurityContext should be granted access based on the authorization policy specified in Share.ConnectSecurity." +
                    " If the server determines that access should not be granted, the server MUST fail the request with STATUS_ACCESS_DENIED.");
                ModelHelper.Log(
                    LogType.Requirement,
                    "3.3.4.13: If Share.Name is equal to \"IPC$\" or Share.Type does not have the STYPE_SPECIAL bit set," +
                    " then Share.ConnectSecurity SHOULD be set to a security descriptor allowing all users." +
                    " Otherwise, Share.ConnectSecurity SHOULD be set to a security descriptor allowing only administrators.");
                ModelHelper.Log(
                    LogType.TestInfo,
                    "User authenticated the session is a NonAdmin, and share in the TreeConnect request has STYPE_SPECIAL bit set");
                ModelHelper.Log(LogType.TestTag, TestTag.Compatibility);
                Condition.IsTrue(status == ModelSmb2Status.STATUS_ACCESS_DENIED);
                return;
            }

            Condition.IsTrue(status == ModelSmb2Status.STATUS_SUCCESS);

            if (treeConnectRequest.sharePath != ModelSharePath.SpecialSharePath)
            {
                // only check on Basic Share, Share with Special Bit can be any Type of Share
                // currently, only DISK ShareType in FileSharing Family test suite
                // it's not TD requirement but assumption
                Condition.IsTrue(shareType == ShareType_Values.SHARE_TYPE_DISK);
            }

            Session_TreeConnectExist = true;
        }

        /// <summary>
        /// Request of TreeDisconnect
        /// </summary>
        [Rule]
        public static void TreeDisconnectRequest(ModelTreeId treeId)
        {
            Condition.IsTrue(State == ModelState.Connected);
            Condition.IsNull(Request);

            // If Security Context is Non Admin or no TreeConnect exists, only ValidTreeId.
            Condition.IfThen(
                !Session_TreeConnectExist
                || Session_SecurityContext == ModelSessionSecurityContext.NonAdmin,
                treeId == ModelTreeId.ValidExistTreeId || treeId == ModelTreeId.InvalidTreeId);

            Request = new ModelTreeMgmtTreeDisconnectRequest(treeId);
        }

        /// <summary>
        /// Response of TreeDisconnect
        /// </summary>
        [Rule]
        public static void TreeDisconnectResponse(ModelSmb2Status status)
        {
            Condition.IsTrue(State == ModelState.Connected);

            ModelTreeMgmtTreeDisconnectRequest treeDisconnectRequest = ModelHelper.RetrieveOutstandingRequest<ModelTreeMgmtTreeDisconnectRequest>(ref Request);

            if (!Session_TreeConnectExist
                || treeDisconnectRequest.treeId != ModelTreeId.ValidExistTreeId)
            {
                ModelHelper.Log(
                    LogType.Requirement,
                    "3.3.5.2.11: The server MUST look up the TreeConnect in Session.TreeConnectTable by using the TreeId in the SMB2 header of the request." +
                    " If no tree connect is found, the request MUST be failed with STATUS_NETWORK_NAME_DELETED");
                ModelHelper.Log(
                    LogType.TestInfo,
                    "TreeConnect {0} in Session.TreeConnectTable, tree id is {1}",
                    Session_TreeConnectExist ? "exists" : "does not exist", treeDisconnectRequest.treeId);
                ModelHelper.Log(LogType.TestTag, TestTag.InvalidIdentifier);
                Condition.IsTrue(status == ModelSmb2Status.STATUS_NETWORK_NAME_DELETED);
                return;
            }

            ModelHelper.Log(
                LogType.Requirement,
                "3.3.5.8: The tree connect MUST then be removed from Session.TreeConnectTable and freed");

            Condition.IsTrue(status == ModelSmb2Status.STATUS_SUCCESS);
            Session_TreeConnectExist = false;

            ModelHelper.Log(
                LogType.TestInfo,
                "TreeConnect is removed from Session.TreeConnectTable");
        }

        #endregion
    }

}
