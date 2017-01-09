// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using Microsoft.Protocols.TestTools;
using Microsoft.Protocols.TestTools.Messages;
using Microsoft.Protocols.TestTools.Messages.Marshaling;
using Microsoft.Protocols.TestSuites.ActiveDirectory.Drsr.NativeTypes;

namespace Microsoft.Protocols.TestSuites.ActiveDirectory.Drsr
{
    public partial interface IMS_DRSR_RpcAdapter : IRpcAdapter
    {
        /// <summary>
        ///  The IDL_DRSBind method creates a context handle that
        ///  is necessary to call any other method in this interface.
        ///  Opnum: 0 
        /// </summary>
        /// <param name="rpc_handle">
        ///  An RPC binding handle, as specified in [C706].
        /// </param>
        /// <param name="puuidClientDsa">
        ///  A pointer to an implementation-specific identity of
        ///  the caller.
        /// </param>
        /// <param name="pextClient">
        ///  A pointer to client capabilities, for use in version
        ///  negotiation.
        /// </param>
        /// <param name="ppextServer">
        ///  A pointer to a pointer to server capabilities, for use
        ///  in version negotiation.
        /// </param>
        /// <param name="phDrs">
        ///  A pointer to an RPC context handle (as specified in
        ///  [C706]), which may be used in calls to other methods
        ///  in this interface.
        /// </param>
        /// <returns>0 if successful; otherwise, a Windows error code.</returns>
        uint IDL_DRSBind(
            System.IntPtr rpc_handle,
            [Indirect()] System.Guid puuidClientDsa,
            [Indirect()] DRS_EXTENSIONS pextClient,
            [Indirect()] out DRS_EXTENSIONS_INT? ppextServer,
             out System.IntPtr? phDrs);

        /// <summary>
        ///  The IDL_DRSUnbind method destroys a context handle previously
        ///  created by the IDL_DRSBind method. Opnum: 1 
        /// </summary>
        /// <param name="phDrs">
        ///  A pointer to the RPC context handle returned by the
        ///  IDL_DRSBind method. The value is set to NULL on return.
        /// </param>
        /// <returns>0 if successful; otherwise, a Windows error code.</returns>
        uint IDL_DRSUnbind(ref System.IntPtr? phDrs);

        /// <summary>
        ///  The IDL_DRSReplicaSync method triggers replication from
        ///  another DC. This method is used only to diagnose, monitor,
        ///  and manage the replication implementation. The structures
        ///  requested and returned through this method MAY have
        ///  meaning to peer DCs and applications, but are not required
        ///  for interoperation with Windows clients. Opnum: 2
        /// </summary>
        /// <param name="hDrs">
        ///  The RPC context handle returned by the IDL_DRSBind method.
        /// </param>
        /// <param name="dwVersion">
        ///  The version of the request message.
        /// </param>
        /// <param name="pmsgSync">
        ///  A pointer to the request message.
        /// </param>
        /// <returns>0 if successful; otherwise, a Windows error code.</returns>
        uint IDL_DRSReplicaSync(
            System.IntPtr hDrs,
            uint dwVersion,
            [Indirect()] [Switch("dwVersion")] DRS_MSG_REPSYNC? pmsgSync);

        /// <summary>
        /// IDL_DRSGetNCChanges :Opnum 3
        /// </summary>
        /// <param name="hDrs">
        ///  The RPC context handle returned by the IDL_DRSBind method.
        /// </param>
        /// <param name="dwInVersion">
        ///  The version of the request message.
        /// </param>
        /// <param name="pmsgIn">
        /// Pointer to the request message.
        /// </param>
        /// <param name="pdwOutVersion">
        ///  A pointer to the version of the response message. The
        ///  value is always 1 because that is the only version
        ///  supported.
        /// </param>
        /// <param name="pmsgOut">
        ///  A pointer to the response message.
        /// </param>
        /// <returns>0 if successful, otherwise a Windows error code.</returns>
        uint IDL_DRSGetNCChanges(
            System.IntPtr hDrs,
            uint dwInVersion,
            [Indirect()] [Switch("dwInVersion")] DRS_MSG_GETCHGREQ? pmsgIn,
            out System.UInt32? pdwOutVersion,
            [Switch("*pdwOutVersion")] out DRS_MSG_GETCHGREPLY? pmsgOut);


        /// <summary>
        ///  The IDL_DRSUpdateRefs method adds or deletes a value
        ///  from the repsTo of a specified NC replica. This method
        ///  is used only to diagnose, monitor, and manage the replication
        ///  implementation. The structures requested and returned
        ///  through this method MAY have meaning to peer DCs and
        ///  applications, but are not required for interoperation
        ///  with Windows clients. Opnum: 4 
        /// </summary>
        /// <param name="hDrs">
        ///  The RPC context handle returned by the IDL_DRSBind method.
        /// </param>
        /// <param name="dwVersion">
        ///  The version of the request message.
        /// </param>
        /// <param name="pmsgUpdRefs">
        ///  A pointer to the request message.
        /// </param>
        /// <returns>0 if successful; otherwise, a Windows error code.</returns>
        uint IDL_DRSUpdateRefs(
            System.IntPtr hDrs,
            uint dwVersion,
            [Indirect()] [Switch("dwVersion")] DRS_MSG_UPDREFS? pmsgUpdRefs);

        /// <summary>
        ///  The IDL_DRSReplicaAdd method adds a replication source
        ///  reference for the specified NC. This method is used
        ///  only to diagnose, monitor, and manage the replication
        ///  implementation. The structures requested and returned
        ///  through this method MAY have meaning to peer DCs and
        ///  applications, but are not required for interoperation
        ///  with Windows clients. Opnum: 5 
        /// </summary>
        /// <param name="hDrs">
        ///  The RPC context handle returned by the IDL_DRSBind method.
        /// </param>
        /// <param name="dwVersion">
        ///  The version of the request message.
        /// </param>
        /// <param name="pmsgAdd">
        ///  A pointer to the request message.
        /// </param>
        /// <returns>0 if successful; otherwise, a Windows error code.</returns>
        uint IDL_DRSReplicaAdd(
            System.IntPtr hDrs,
            uint dwVersion,
            [Indirect()] [Switch("dwVersion")] DRS_MSG_REPADD? pmsgAdd);

        /// <summary>
        ///  The IDL_DRSReplicaDel method deletes a replication source
        ///  reference for the specified NC. This method is used
        ///  only to diagnose, monitor, and manage the replication
        ///  implementation. The structures requested and returned
        ///  through this method MAY have meaning to peer DCs and
        ///  applications, but are not required for interoperation
        ///  with Windows clients. Opnum: 6 
        /// </summary>
        /// <param name="hDrs">
        ///  The RPC context handle returned by IDL_DRSBind.
        /// </param>
        /// <param name="dwVersion">
        ///  The version of the request message.
        /// </param>
        /// <param name="pmsgDel">
        ///  A pointer to the request message.
        /// </param>
        /// <returns>0 if successful; otherwise, a Windows error code.</returns>
        uint IDL_DRSReplicaDel(
            System.IntPtr hDrs,
            uint dwVersion,
            [Indirect()] [Switch("dwVersion")] DRS_MSG_REPDEL? pmsgDel);

        /// <summary>
        ///  The IDL_DRSReplicaModify method updates the value for
        ///  repsFrom for the NC replica. This method is used only
        ///  to diagnose, monitor, and manage the replication implementation.
        ///  The structures requested and returned through this
        ///  method MAY have meaning to peer DCs and applications,
        ///  but are not required for interoperation with Windows
        ///  clients. Opnum: 7 
        /// </summary>
        /// <param name="hDrs">
        ///  The RPC context handle returned by IDL_DRSBind.
        /// </param>
        /// <param name="dwVersion">
        ///  The version of the request message.
        /// </param>
        /// <param name="pmsgMod">
        ///  A pointer to the request message.
        /// </param>
        /// <returns>0 if successful; otherwise, a Windows error code.</returns>
        uint IDL_DRSReplicaModify(
            System.IntPtr hDrs,
            uint dwVersion,
            [Indirect()] [Switch("dwVersion")] DRS_MSG_REPMOD? pmsgMod);

        /// <summary>
        /// The IDL_DRSVerifyNames method resolves a sequence of object identities. Opnum: 8
        /// </summary>
        /// <param name="hDrs">
        ///  The RPC context handle returned by IDL_DRSBind.
        /// </param>
        /// <param name="dwInVersion">
        ///  The version of the request message.
        /// </param>
        /// <param name="pmsgIn">
        ///  The request message.
        /// </param>
        /// <param name="pdwOutVersion">
        ///  The version of the response message.
        /// </param>
        /// <param name="pmsgOut">
        ///  The request message.
        /// </param>
        /// <returns>0 if successful; otherwise, a Windows error code.</returns>
        uint IDL_DRSVerifyNames(
            System.IntPtr hDrs,
            uint dwInVersion,
            [Indirect()] [Switch("dwInVersion")]  DRS_MSG_VERIFYREQ? pmsgIn,
            out System.UInt32? pdwOutVersion,
            [Switch("*pdwOutVersion")] out DRS_MSG_VERIFYREPLY? pmsgOut);

        /// <summary>
        /// IDL_DRSGetMemberships retrieves group membership for an object.
        /// </summary>
        /// <param name="hDrs">
        /// The RPC context handle returned by the IDL_DRSBind method.
        /// </param>
        /// <param name="dwInVersion">
        /// The version of the request message.
        /// </param>
        /// <param name="pmsgIn">
        /// Pointer to the request message.
        /// </param>
        /// <param name="pdwOutVersion">
        ///  A pointer to the version of the response message. The
        ///  value is always 1 because that is the only version
        ///  supported.
        /// </param>
        /// <param name="pmsgOut">
        ///  A pointer to the response message.
        /// </param>
        /// <returns>0 if successful; otherwise, a Windows error code.</returns>
        uint IDL_DRSGetMemberships(
            System.IntPtr hDrs,
            uint dwInVersion,
            [Indirect()] [Switch("dwInVersion")] DRS_MSG_REVMEMB_REQ? pmsgIn,
            out System.UInt32? pdwOutVersion,
            [Switch("*pdwOutVersion")] out DRS_MSG_REVMEMB_REPLY? pmsgOut);

        /// <summary>
        ///IDL_DRSInterDomainMove method is a helper method used in a cross-NC move LDAP operation.
        /// </summary>
        /// <param name="hDrs">
        /// The RPC context handle returned by the IDL_DRSBind method.
        /// </param>
        /// <param name="dwInVersion">
        /// The version of the request message.
        /// </param>
        /// <param name="pmsgIn">
        /// Pointer to the request message.
        /// </param>
        /// <param name="pdwOutVersion">
        ///  A pointer to the version of the response message. The
        ///  value is always 1 because that is the only version
        ///  supported.
        /// </param>
        /// <param name="pmsgOut">
        ///  A pointer to the response message.
        /// </param>
        /// <returns>0 if successful; otherwise, a Windows error code.</returns>
        uint IDL_DRSInterDomainMove(
            System.IntPtr hDrs,
            uint dwInVersion,
            [Indirect()] [Switch("dwInVersion")] DRS_MSG_MOVEREQ? pmsgIn,
            out System.UInt32? pdwOutVersion,
            [Switch("*pdwOutVersion")] out  DRS_MSG_MOVEREPLY? pmsgOut);

        /// <summary>
        /// IDL_DRSInterDomainMove method is a helper method used in a cross-NC move LDAP operation.
        /// </summary>
        /// <param name="hDrs">
        /// The RPC context handle returned by the IDL_DRSBind method.
        /// </param>
        /// <param name="dwInVersion">
        /// The version of the request message.
        /// </param>
        /// <param name="pmsgIn">
        /// Pointer to the request message.
        /// </param>
        /// <param name="pdwOutVersion">
        ///  A pointer to the version of the response message. The
        ///  value is always 1 because that is the only version
        ///  supported.
        /// </param>
        /// <param name="pmsgOut">
        ///  A pointer to the response message.
        /// </param>
        /// <returns>0 if successful; otherwise, a Windows error code.</returns>
        uint IDL_DRSGetNT4ChangeLog(
            System.IntPtr hDrs,
            uint dwInVersion,
            [Indirect()] [Switch("dwInVersion")] DRS_MSG_NT4_CHGLOG_REQ? pmsgIn,
            out System.UInt32? pdwOutVersion,
            [Switch("*pdwOutVersion")] out DRS_MSG_NT4_CHGLOG_REPLY? pmsgOut);

        /// <summary>
        ///  The IDL_DRSCrackNames method looks up each of a set
        ///  of objects in the directory and returns it to the caller
        ///  in the requested format. Opnum: 12 
        /// </summary>
        /// <param name="hDrs">
        ///  RPC context handle returned by the IDL_DRSBind method.
        /// </param>
        /// <param name="dwInVersion">
        ///  Version of the request message.
        /// </param>
        /// <param name="pmsgIn">
        ///  Pointer to the request message.
        /// </param>
        /// <param name="pdwOutVersion">
        ///  Pointer to the version of the response message.
        /// </param>
        /// <param name="pmsgOut">
        ///  Pointer to the response message.
        /// </param>
        /// <returns>0 if successful; otherwise, a Windows error code.</returns>
        uint IDL_DRSCrackNames(
            System.IntPtr hDrs,
            uint dwInVersion,
            [Indirect()] [Switch("dwInVersion")] DRS_MSG_CRACKREQ? pmsgIn,
            out System.UInt32? pdwOutVersion,
            [Switch("*pdwOutVersion")] out DRS_MSG_CRACKREPLY? pmsgOut);

        /// <summary>
        ///  The IDL_DRSWriteSPN method updates the set of SPNs on
        ///  an object. Opnum: 13 
        /// </summary>
        /// <param name="hDrs">
        ///  The RPC context handle returned by the IDL_DRSBind method.
        /// </param>
        /// <param name="dwInVersion">
        ///  The version of the request message. MUST be set to 1
        ///  because that is the only version supported.
        /// </param>
        /// <param name="pmsgIn">
        ///  A pointer to the request message.
        /// </param>
        /// <param name="pdwOutVersion">
        ///  A pointer to the version of the response message. The
        ///  value is always 1 because that is the only version
        ///  supported.
        /// </param>
        /// <param name="pmsgOut">
        ///  A pointer to the response message.
        /// </param>
        /// <returns>0 if successful; otherwise, a Windows error code.</returns>
        uint IDL_DRSWriteSPN(
            System.IntPtr hDrs,
            dwInVersion_Values dwInVersion,
            [Indirect()] [Switch("dwInVersion")] DRS_MSG_SPNREQ? pmsgIn,
            out pdwOutVersion_Values? pdwOutVersion,
            [Switch("*pdwOutVersion")] out DRS_MSG_SPNREPLY? pmsgOut);

        /// <summary>
        ///  The IDL_DRSRemoveDsServer method removes the representation
        ///  (also known as metadata) of a DC from the directory.
        ///  Opnum: 14 
        /// </summary>
        /// <param name="hDrs">
        ///  The RPC context handle returned by the IDL_DRSBind method.
        /// </param>
        /// <param name="dwInVersion">
        ///  The version of the request message. MUST be set to 1
        ///  because that is the only version supported.
        /// </param>
        /// <param name="pmsgIn">
        ///  A pointer to the request message.
        /// </param>
        /// <param name="pdwOutVersion">
        ///  A pointer to the version of the response message. The
        ///  value is always 1 because that is the only version
        ///  supported.
        /// </param>
        /// <param name="pmsgOut">
        ///  A pointer to the response message.
        /// </param>
        /// <returns>0 if successful; otherwise, a Windows error code.</returns>
        uint IDL_DRSRemoveDsServer(
            System.IntPtr hDrs,
            uint dwInVersion,
            [Indirect()] [Switch("dwInVersion")] DRS_MSG_RMSVRREQ? pmsgIn,
            out uint? pdwOutVersion,
            [Switch("*pdwOutVersion")] out DRS_MSG_RMSVRREPLY? pmsgOut);

        /// <summary>
        ///  The IDL_DRSRemoveDsDomain method removes the representation
        ///  (also known as metadata) of a domain from the directory.
        ///  Opnum: 15 
        /// </summary>
        /// <param name="hDrs">
        ///  The RPC context handle returned by the IDL_DRSBind method.
        /// </param>
        /// <param name="dwInVersion">
        ///  The version of the request message. This MUST be set
        ///  to 1, as this is the only version supported.
        /// </param>
        /// <param name="pmsgIn">
        ///  A pointer to the request message.
        /// </param>
        /// <param name="pdwOutVersion">
        ///  A pointer to the version of the response message. The
        ///  value is always 1 because that is the only version
        ///  supported.
        /// </param>
        /// <param name="pmsgOut">
        ///  A pointer to the response message.
        /// </param>
        /// <returns>0 if successful; otherwise, a Windows error code.</returns>
        uint IDL_DRSRemoveDsDomain(
            System.IntPtr hDrs,
            uint dwInVersion,
            [Indirect()] [Switch("dwInVersion")] DRS_MSG_RMDMNREQ? pmsgIn,
            out uint? pdwOutVersion,
            [Switch("*pdwOutVersion")] out DRS_MSG_RMDMNREPLY? pmsgOut);

        /// <summary>
        ///  The IDL_DRSDomainControllerInfo method retrieves information
        ///  about DCs in a given domain. Opnum: 16 
        /// </summary>
        /// <param name="hDrs">
        ///  RPC context handle returned by the IDL_DRSBind method.
        /// </param>
        /// <param name="dwInVersion">
        ///  Version of the request message.
        /// </param>
        /// <param name="pmsgIn">
        ///  Pointer to the request message.
        /// </param>
        /// <param name="pdwOutVersion">
        ///  Pointer to the version of the response message.
        /// </param>
        /// <param name="pmsgOut">
        ///  Pointer to the response message.
        /// </param>
        /// <returns>0 if successful; otherwise, a Windows error code.</returns>
        uint IDL_DRSDomainControllerInfo(
            System.IntPtr hDrs,
            uint dwInVersion,
            [Indirect()] [Switch("dwInVersion")] DRS_MSG_DCINFOREQ pmsgIn,
            out System.UInt32? pdwOutVersion,
            [Switch("*pdwOutVersion")] out DRS_MSG_DCINFOREPLY? pmsgOut);

        /// <summary>
        ///  The IDL_DRSAddEntry method adds one or more objects. Opnum: 17 
        /// </summary>
        /// <param name="hDrs">
        ///  RPC context handle returned by the IDL_DRSBind method.
        /// </param>
        /// <param name="dwInVersion">
        ///  Version of the request message.
        /// </param>
        /// <param name="pmsgIn">
        ///  Pointer to the request message.
        /// </param>
        /// <param name="pdwOutVersion">
        ///  Pointer to the version of the response message.
        /// </param>
        /// <param name="pmsgOut">
        ///  Pointer to the response message.
        /// </param>
        /// <returns>0 if successful; otherwise, a Windows error code.</returns>
        uint IDL_DRSAddEntry(
            System.IntPtr hDrs,
            uint dwInVersion,
            [Indirect()] [Switch("dwInVersion")] DRS_MSG_ADDENTRYREQ? pmsgIn,
            out System.UInt32? pdwOutVersion,
            [Switch("*pdwOutVersion")] out DRS_MSG_ADDENTRYREPLY? pmsgOut);

        /// <summary>
        ///  The IDL_DRSExecuteKCC method validates the replication
        ///  interconnections of DCs and updates them if necessary.
        ///  This method is used only to diagnose, monitor, and
        ///  manage the replication topology implementation. The
        ///  structures requested and returned through this method
        ///  MAY have meaning to peer DCs and applications, but
        ///  are not required for interoperation with Windows clients.
        ///  Opnum: 18 
        /// </summary>
        /// <param name="hDrs">
        ///  The RPC context handle returned by the IDL_DRSBind method.
        /// </param>
        /// <param name="dwInVersion">
        ///  The version of the request message.
        /// </param>
        /// <param name="pmsgIn">
        ///  A pointer to the request message.
        /// </param>
        /// <returns>0 if successful, otherwise a Windows error code.</returns>
        uint IDL_DRSExecuteKCC(
            System.IntPtr hDrs,
            uint dwInVersion,
            [Indirect()] [Switch("dwInVersion")] DRS_MSG_KCC_EXECUTE? pmsgIn);

        /// <summary>
        ///  The IDL_DRSGetReplInfo method retrieves the replication
        ///  state of the server. This method is used only to diagnose,
        ///  monitor, and manage the replication implementation.
        ///  The structures requested and returned through this
        ///  method MAY have meaning to peer DCs and applications,
        ///  but are not required for interoperation with Windows
        ///  clients. Opnum: 19 
        /// </summary>
        /// <param name="hDrs">
        ///  The RPC context handle returned by the IDL_DRSBind method.
        /// </param>
        /// <param name="dwInVersion">
        ///  The version of the request message.
        /// </param>
        /// <param name="pmsgIn">
        ///  A pointer to the request message.
        /// </param>
        /// <param name="pdwOutVersion">
        ///  A pointer to the version of the response message.
        /// </param>
        /// <param name="pmsgOut">
        ///  A pointer to the response message.
        /// </param>
        /// <returns>0 if successful, otherwise a Windows error code.</returns>
        uint IDL_DRSGetReplInfo(
            System.IntPtr hDrs,
            uint dwInVersion,
            [Indirect()] [Switch("dwInVersion")] DRS_MSG_GETREPLINFO_REQ? pmsgIn,
            out System.UInt32? pdwOutVersion,
            [Switch("*pdwOutVersion")] out DRS_MSG_GETREPLINFO_REPLY? pmsgOut);

        /// <summary>
        ///  The IDL_DRSAddSidHistory method adds one or more SIDs
        ///  to the sIDHistory attribute of a given object. Opnum
        ///  : 20 
        /// </summary>
        /// <param name="hDrs">
        ///  RPC context handle returned by the IDL_DRSBind method.
        /// </param>
        /// <param name="dwInVersion">
        ///  Version of the request message. Must be set to 1, because
        ///  no other version is supported.
        /// </param>
        /// <param name="pmsgIn">
        ///  Pointer to the request message.
        /// </param>
        /// <param name="pdwOutVersion">
        ///  Pointer to the version of the response message. The
        ///  value will always be 1, because no other version is
        ///  supported.
        /// </param>
        /// <param name="pmsgOut">
        ///  Pointer to the response message.
        /// </param>
        /// <returns>
        /// 0 or one of the following Windows error codes: ERROR_DS_MUST_RUN_ON_DST_DC or ERROR_INVALID_PARAMETER.
        /// </returns>
        uint IDL_DRSAddSidHistory(
            System.IntPtr hDrs,
            uint dwInVersion,
            [Indirect()] [Switch("dwInVersion")] DRS_MSG_ADDSIDREQ? pmsgIn,
            out uint? pdwOutVersion,
            [Switch("*pdwOutVersion")] out DRS_MSG_ADDSIDREPLY? pmsgOut);

        /// <summary>
        /// IDL_DRSGetMemberships2 method retrieves group memberships for a sequence of objects. 
        /// </summary>
        /// <param name="hDrs">
        /// The RPC context handle returned by the IDL_DRSBind method.
        /// </param>
        /// <param name="dwInVersion">
        /// The version of the request message.
        /// </param>
        /// <param name="pmsgIn">
        /// Pointer to the request message.
        /// </param>
        /// <param name="pdwOutVersion">
        ///  A pointer to the version of the response message. The
        ///  value is always 1 because that is the only version
        ///  supported.
        /// </param>
        /// <param name="pmsgOut">
        ///  A pointer to the response message.
        /// </param>
        /// <returns>0 if successful; otherwise, a Windows error code.</returns>
        uint IDL_DRSGetMemberships2(
            System.IntPtr hDrs,
            uint dwInVersion,
            [Indirect()] [Switch("dwInVersion")] DRS_MSG_GETMEMBERSHIPS2_REQ? pmsgIn,
            out System.UInt32? pdwOutVersion,
            [Switch("*pdwOutVersion")] out DRS_MSG_GETMEMBERSHIPS2_REPLY? pmsgOut);

        /// <summary>
        ///  The IDL_DRSReplicaVerifyObjects method verifies the
        ///  existence of objects in an NC replica by comparing
        ///  against a replica of the same NC on a reference DC,
        ///  optionally deleting any objects that do not exist on
        ///  the reference DC. This method is used only to diagnose,
        ///  monitor, and manage the replication implementation.
        ///  The structures requested and returned through this
        ///  method MAY have meaning to peer DCs and applications,
        ///  but are not required for interoperation with Windows
        ///  clients. Opnum: 22 
        /// </summary>
        /// <param name="hDrs">
        ///  The RPC context handle returned by the IDL_DRSBind method.
        /// </param>
        /// <param name="dwVersion">
        ///  The version of the request message.
        /// </param>
        /// <param name="pmsgVerify">
        ///  A pointer to the request message.
        /// </param>
        /// <returns><returns>0 if successful; otherwise, a Windows error code.</returns></returns>
        uint IDL_DRSReplicaVerifyObjects(
            System.IntPtr hDrs,
            uint dwVersion,
            [Indirect()] [Switch("dwVersion")] DRS_MSG_REPVERIFYOBJ? pmsgVerify);

        /// <summary>
        /// IDL_DRSGetObjectExistence helps the client check the consistency of object 
        /// existence between its replica of an NC and the server's replica of the same NC.
        /// Checking the consistency of object existence means identifying objects that have 
        /// replicated to both replicas and that exist in one replica but not in the other. 
        /// For the purposes of this method, an object exists within a NC replica if it is 
        /// either an object or a tombstone.
        /// </summary>
        /// <param name="hDrs">
        /// The RPC context handle returned by the IDL_DRSBind method.
        /// </param>
        /// <param name="dwInVersion">
        /// The version of the request message.
        /// </param>
        /// <param name="pmsgIn">
        /// Pointer to the request message.
        /// </param>
        /// <param name="pdwOutVersion">
        ///  A pointer to the version of the response message. The
        ///  value is always 1 because that is the only version
        ///  supported.
        /// </param>
        /// <param name="pmsgOut">
        ///  A pointer to the response message.
        /// </param>
        /// <returns>0 if successful; otherwise, a Windows error code.</returns>
        uint IDL_DRSGetObjectExistence(
            System.IntPtr hDrs,
            uint dwInVersion,
            [Indirect()] [Switch("dwInVersion")] DRS_MSG_EXISTREQ? pmsgIn,
            out System.UInt32? pdwOutVersion,
            [Switch("*pdwOutVersion")] out DRS_MSG_EXISTREPLY? pmsgOut);

        /// <summary>
        ///  The IDL_DRSQuerySitesByCost method determines the communication
        ///  cost from a "from" site to one or more "to" sites.
        ///  Opnum: 24 
        /// </summary>
        /// <param name="hDrs">
        ///  The RPC context handle returned by the IDL_DRSBind method.
        /// </param>
        /// <param name="dwInVersion">
        ///  The version of the request message.
        /// </param>
        /// <param name="pmsgIn">
        ///  A pointer to the request message.
        /// </param>
        /// <param name="pdwOutVersion">
        ///  A pointer to the version of the response message.
        /// </param>
        /// <param name="pmsgOut">
        ///  A pointer to the response message.
        /// </param>
        /// <returns>0 if successful; otherwise, a Windows error code.</returns>
        uint IDL_DRSQuerySitesByCost(
            System.IntPtr hDrs,
            uint dwInVersion,
            [Indirect()] [Switch("dwInVersion")] DRS_MSG_QUERYSITESREQ? pmsgIn,
            out System.UInt32? pdwOutVersion,
            [Switch("*pdwOutVersion")] out DRS_MSG_QUERYSITESREPLY? pmsgOut);

        /// <summary>
        ///  The IDL_DRSInitDemotion method performs the first phase
        ///  of the removal of a DC from an AD LDS forest. This
        ///  method is supported only by AD LDS. This method is
        ///  used only to diagnose, monitor, and manage the implementation
        ///  of server-to-server DC demotion. The structures requested
        ///  and returned through this method MAY have meaning to
        ///  peer DCs and applications, but are not required for
        ///  interoperation with Windows clients. Opnum: 25 
        /// </summary>
        /// <param name="hDrs">
        ///  The RPC context handle returned by the IDL_DRSBind method.
        /// </param>
        /// <param name="dwInVersion">
        ///  The version of the request message.
        /// </param>
        /// <param name="pmsgIn">
        ///  A pointer to the request message.
        /// </param>
        /// <param name="pdwOutVersion">
        ///  A pointer to the version of the response message.
        /// </param>
        /// <param name="pmsgOut">
        ///  A pointer to the response message.
        /// </param>
        /// <returns>0 if successful; otherwise, a Windows error code.</returns>
        uint IDL_DRSInitDemotion(
            System.IntPtr hDrs,
            uint dwInVersion,
            [Indirect()] [Switch("dwInVersion")] DRS_MSG_INIT_DEMOTIONREQ? pmsgIn,
            out System.UInt32? pdwOutVersion,
            [Switch("*pdwOutVersion")] out DRS_MSG_INIT_DEMOTIONREPLY? pmsgOut);

        /// <summary>
        ///  The IDL_DRSReplicaDemotion method replicatesinitiates
        ///  server-to-server replication to replicate off all changes
        ///  to the specified NC and moves any FSMOs held to another
        ///  server. This method is supported only by AD LDS. This
        ///  method is used only to diagnose, monitor, and manage
        ///  the replication and FSMO implementation related to
        ///  DC demotion. The structures requested and returned
        ///  through this method MAY have meaning to peer DCs and
        ///  applications, but are not required for interoperation
        ///  with Windows clients. Opnum: 26 
        /// </summary>
        /// <param name="hDrs">
        ///  The RPC context handle returned by the IDL_DRSBind method.
        /// </param>
        /// <param name="dwInVersion">
        ///  The version of the request message.
        /// </param>
        /// <param name="pmsgIn">
        ///  A pointer to the request message.
        /// </param>
        /// <param name="pdwOutVersion">
        ///  A pointer to the version of the response message.
        /// </param>
        /// <param name="pmsgOut">
        ///  A pointer to the response message.
        /// </param>
        /// <returns>0 if successful; otherwise, a Windows error code.</returns>
        uint IDL_DRSReplicaDemotion(
            System.IntPtr hDrs,
            uint dwInVersion,
            [Indirect()] [Switch("dwInVersion")] DRS_MSG_REPLICA_DEMOTIONREQ? pmsgIn,
            out System.UInt32? pdwOutVersion,
            [Switch("*pdwOutVersion")] out DRS_MSG_REPLICA_DEMOTIONREPLY? pmsgOut);

        /// <summary>
        ///  The IDL_DRSFinishDemotion method either performs one
        ///  or more steps toward the complete removal of a DC from
        ///  an AD LDS forest, or it undoes the effects of the first
        ///  phase of removal (performed by IDL_DRSInitDemotion).
        ///  This method is supported by AD LDS only. This method
        ///  is used only to diagnose, monitor, and manage the implementation
        ///  of server-to-server DC demotion. The structures requested
        ///  and returned through this method MAY have meaning to
        ///  peer DCs and applications, but are not required for
        ///  interoperation with Windows clients. Opnum: 27 
        /// </summary>
        /// <param name="hDrs">
        ///  The RPC context handle returned by the IDL_DRSBind method.
        /// </param>
        /// <param name="dwInVersion">
        ///  The version of the request message.
        /// </param>
        /// <param name="pmsgIn">
        ///  A pointer to the request message.
        /// </param>
        /// <param name="pdwOutVersion">
        ///  A pointer to the version of the response message.
        /// </param>
        /// <param name="pmsgOut">
        ///  A pointer to the response message.
        /// </param>
        /// <returns>0 if successful; otherwise, a Windows error code.</returns>
        uint IDL_DRSFinishDemotion(
            System.IntPtr hDrs,
            uint dwInVersion,
            [Indirect()] [Switch("dwInVersion")] DRS_MSG_FINISH_DEMOTIONREQ? pmsgIn,
            out System.UInt32? pdwOutVersion,
            [Switch("*pdwOutVersion")] out DRS_MSG_FINISH_DEMOTIONREPLY? pmsgOut);

        /// <summary>
        ///  The IDL_DSAPrepareScript method prepares the DC to run
        ///  a maintenance script. Opnum: 0 
        /// </summary>
        /// <param name="hRpc">
        ///  The RPC binding handle, as specified in [C706].
        /// </param>
        /// <param name="dwInVersion">
        ///  The version of the request message.
        /// </param>
        /// <param name="pmsgIn">
        ///  A pointer to the request message.
        /// </param>
        /// <param name="pdwOutVersion">
        ///  A pointer to the version of the response message.
        /// </param>
        /// <param name="pmsgOut">
        ///  A pointer to the response message.
        /// </param>
        /// <returns>0 if successful; otherwise, a Windows error code.</returns>
        uint IDL_DSAPrepareScript(
            System.IntPtr hRpc,
            uint dwInVersion,
            [Indirect()] [Switch("dwInVersion")] DSA_MSG_PREPARE_SCRIPT_REQ pmsgIn,
            out System.UInt32? pdwOutVersion,
            [Switch("*pdwOutVersion")] out DSA_MSG_PREPARE_SCRIPT_REPLY pmsgOut);

        /// <summary>
        ///  The IDL_DSAExecuteScript method executes a maintenance
        ///  script. Opnum: 1 
        /// </summary>
        /// <param name="hRpc">
        ///  The RPC binding handle, as specified in  [C706].
        /// </param>
        /// <param name="dwInVersion">
        ///  The version of the request message.
        /// </param>
        /// <param name="pmsgIn">
        ///  A pointer to the request message.
        /// </param>
        /// <param name="pdwOutVersion">
        ///  A pointer to the version of the response message.
        /// </param>
        /// <param name="pmsgOut">
        ///  A pointer to the response message.
        /// </param>
        /// <returns>0 if successful; otherwise, a Windows error code.</returns>
        uint IDL_DSAExecuteScript(
            System.IntPtr hRpc,
            uint dwInVersion,
            [Indirect()] [Switch("dwInVersion")] DSA_MSG_EXECUTE_SCRIPT_REQ pmsgIn,
            out System.UInt32? pdwOutVersion,
            [Switch("*pdwOutVersion")] out DSA_MSG_EXECUTE_SCRIPT_REPLY pmsgOut);

        /// <summary>
        ///  The IDL_DRSAddCloneDC method is used to create a new DC object by copying attributes 
        ///  from an existing DC object.
        ///  Opnum: 28 
        /// </summary>
        /// <param name="hDrs">
        ///  The RPC context handle returned by the IDL_DRSBind method.
        /// </param>
        /// <param name="dwInVersion">
        ///  The version of the request message.
        /// </param>
        /// <param name="pmsgIn">
        ///  A pointer to the request message.
        /// </param>
        /// <param name="pdwOutVersion">
        ///  A pointer to the version of the response message.
        /// </param>
        /// <param name="pmsgOut">
        ///  A pointer to the response message.
        /// </param>
        [CLSCompliant(false)]
        uint IDL_DRSAddCloneDC(
            System.IntPtr hDrs,
            uint dwInVersion,
            [Indirect()] [Switch("dwInVersion")] DRS_MSG_ADDCLONEDCREQ? pmsgIn,
            out System.UInt32? pdwOutVersion,
            [Switch("*pdwOutVersion")] out DRS_MSG_ADDCLONEDCREPLY? pmsgOut);

        /// <summary>
        ///  The IDL_DRSWriteNgcKey method composes and updates the
        ///  msDS-KeyCredentialLink value on an object.
        ///  Opnum: 29
        /// </summary>
        /// <param name="hDrs">
        ///  The RPC context handle returned by the IDL_DRSBind method.
        /// </param>
        /// <param name="dwInVersion">
        ///  The version of the request message. MUST be set to 1,
        ///  because that is the only version supported.
        /// </param>
        /// <param name="pmsgIn">
        ///  A pointer to the request message.
        /// </param>
        /// <param name="pdwOutVersion">
        ///  A pointer to the version of the response message. The
        ///  value is always 1 because that is the only version
        ///  supported.
        /// </param>
        /// <param name="pmsgOut">
        ///  A pointer to the response message.
        /// </param>
        /// <returns>0 if successful; otherwise, a Windows error code.</returns>
        [CLSCompliant(false)]
        uint IDL_DRSWriteNgcKey(
            IntPtr hDrs,
            uint dwInVersion,
            [Indirect()] [Switch("dwInVersion")] DRS_MSG_WRITENGCKEYREQ? pmsgIn,
            out uint? pdwOutVersion,
            [Switch("*pdwOutVersion")] out DRS_MSG_WRITENGCKEYREPLY? pmsgOut);

        /// <summary>
        ///  The IDL_DRSReadNgcKey method reads and parses the
        ///  msDS-KeyCredentialLink value on an object.
        ///  Opnum: 30
        /// </summary>
        /// <param name="hDrs">
        ///  The RPC context handle returned by the IDL_DRSBind method.
        /// </param>
        /// <param name="dwInVersion">
        ///  The version of the request message. MUST be set to 1,
        ///  because that is the only version supported.
        /// </param>
        /// <param name="pmsgIn">
        ///  A pointer to the request message.
        /// </param>
        /// <param name="pdwOutVersion">
        ///  A pointer to the version of the response message. The
        ///  value is always 1 because that is the only version
        ///  supported.
        /// </param>
        /// <param name="pmsgOut">
        ///  A pointer to the response message.
        /// </param>
        /// <returns>0 if successful; otherwise, a Windows error code.</returns>
        uint IDL_DRSReadNgcKey(
            IntPtr hDrs,
            uint dwInVersion,
            [Indirect()] [Switch("dwInVersion")] DRS_MSG_READNGCKEYREQ? pmsgIn,
            out uint? pdwOutVersion,
            [Switch("*pdwOutVersion")] out DRS_MSG_READNGCKEYREPLY? pmsgOut);
    }
}
