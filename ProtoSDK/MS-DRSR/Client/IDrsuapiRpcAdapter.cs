// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Diagnostics.CodeAnalysis;

using Microsoft.Protocols.TestTools.StackSdk.Networking.Rpce;
using Microsoft.Protocols.TestTools.StackSdk.Security.Sspi;

namespace Microsoft.Protocols.TestTools.StackSdk.ActiveDirectory.Drsr
{
    /// <summary>
    /// DRSR RPC interface
    /// </summary>   
    [CLSCompliant(false)]
    public partial interface IDrsuapiRpcAdapter : IDisposable
    {
        /// <summary>
        /// Bind to DRSR RPC server.
        /// </summary>
        /// <param name="protocolSequence">
        /// RPC protocol sequence.
        /// </param>
        /// <param name="networkAddress">
        /// RPC network address.
        /// </param>
        /// <param name="endpoint">
        /// RPC endpoint.
        /// </param>
        /// <param name="securityContext">
        /// RPC security provider.
        /// </param>
        /// <param name="authenticationLevel">
        /// RPC authentication level.
        /// </param>
        /// <param name="timeout">
        /// Timeout
        /// </param>
        [CLSCompliant(false)]
        void Bind(
            string protocolSequence,
            string networkAddress,
            string endpoint,
            ClientSecurityContext securityContext,
            RpceAuthenticationLevel authenticationLevel,
            TimeSpan timeout);

        [CLSCompliant(false)]
        bool Bind(string server, string domain, string userName, string password, string spn);

        /// <summary>
        /// RPC unbind.
        /// </summary>
        void Unbind();


        /// <summary>
        /// RPC handle.
        /// </summary>
        IntPtr Handle
        {
            get;
        }


        /// <summary>
        ///  The IDL_DRSBind method creates a context handle that
        ///  is necessary to call any other method in this interface.
        ///  Opnum: 0 
        /// </summary>
        /// <param name="rpc_handle">
        ///  An RPC binding handle, as specified in [C706].
        /// </param>
        /// <param name="puuidClientDsa">
        ///  A pointer to a GUID that identifies the caller.
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
        [CLSCompliant(false)]
        uint IDL_DRSBind(
            IntPtr rpc_handle,
            Guid? puuidClientDsa,
            DRS_EXTENSIONS? pextClient,
            out DRS_EXTENSIONS_INT? ppextServer,
            out IntPtr? phDrs);


        /// <summary>
        ///  The IDL_DRSUnbind method destroys a context handle previously
        ///  created by the IDL_DRSBind method. Opnum: 1 
        /// </summary>
        /// <param name="phDrs">
        ///  A pointer to the RPC context handle returned by the
        ///  IDL_DRSBind method. The value is set to null on return.
        /// </param>
        [CLSCompliant(false)]
        uint IDL_DRSUnbind(ref IntPtr? phDrs);


        /// <summary>
        ///  The IDL_DRSReplicaSync method triggers replication from
        ///  another DC. This method is used only to diagnose,
        ///  monitor, and manage the replication implementation.
        ///  The structures requested and returned through this
        ///  method MAY have meaning to peer DCs and applications
        ///  but are not required for interoperation with Windows
        ///  clients. Opnum: 2 
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
        [CLSCompliant(false)]
        uint IDL_DRSReplicaSync(
            IntPtr hDrs,
            uint dwVersion,
            //[Switch("dwVersion")]
            DRS_MSG_REPSYNC? pmsgSync);


        /// <summary>
        ///  The IDL_DRSUpdateRefs method adds or deletes a value
        ///  from the repsTo of a specified NC replica. This method
        ///  is used only to diagnose, monitor, and manage the replication
        ///  implementation. The structures requested and returned
        ///  through this method MAY have meaning to peer DCs and
        ///  applications but are not required for interoperation
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
        [CLSCompliant(false)]
        uint IDL_DRSUpdateRefs(
            IntPtr hDrs,
            uint dwVersion,
            //[Switch("dwVersion")] 
            DRS_MSG_UPDREFS? pmsgUpdRefs);


        /// <summary>
        ///  The IDL_DRSReplicaAdd method adds a replication source
        ///  reference for the specified NC. This method is used
        ///  only to diagnose, monitor, and manage the replication
        ///  implementation. The structures requested and returned
        ///  through this method MAY have meaning to peer DCs and
        ///  applications but are not required for interoperation
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
        [CLSCompliant(false)]
        uint IDL_DRSReplicaAdd(
            IntPtr hDrs,
            uint dwVersion,
            //[Switch("dwVersion")]
            DRS_MSG_REPADD? pmsgAdd);


        /// <summary>
        ///  The IDL_DRSReplicaDel method deletes a replication source
        ///  reference for the specified NC. This method is used
        ///  only to diagnose, monitor, and manage the replication
        ///  implementation. The structures requested and returned
        ///  through this method MAY have meaning to peer DCs and
        ///  applications but are not required for interoperation
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
        [CLSCompliant(false)]
        uint IDL_DRSReplicaDel(
            IntPtr hDrs,
            uint dwVersion,
            //[Switch("dwVersion")] 
            DRS_MSG_REPDEL? pmsgDel);


        /// <summary>
        ///  The IDL_DRSReplicaModify method updates the value for
        ///  repsFrom for the NC replica. This method is used only
        ///  to diagnose, monitor, and manage the replication implementation.
        ///  The structures requested and returned through this
        ///  method MAY have meaning to peer DCs and applications
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
        [CLSCompliant(false)]
        uint IDL_DRSReplicaModify(
            IntPtr hDrs,
            uint dwVersion,
            //[Switch("dwVersion")] 
            DRS_MSG_REPMOD? pmsgMod);


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
        [CLSCompliant(false)]
        uint IDL_DRSCrackNames(
            IntPtr hDrs,
            uint dwInVersion,
            //[Switch("dwInVersion")] 
            DRS_MSG_CRACKREQ? pmsgIn,
            out uint? pdwOutVersion,
            //[Switch("*pdwOutVersion")] 
            out DRS_MSG_CRACKREPLY? pmsgOut);


        /// <summary>
        ///  The IDL_DRSWriteSPN method updates the set of SPNs on
        ///  an object. Opnum: 13 
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
        [CLSCompliant(false)]
        uint IDL_DRSWriteSPN(
            IntPtr hDrs,
            dwInVersion_Values dwInVersion,
            //[Switch("dwInVersion")]
            DRS_MSG_SPNREQ? pmsgIn,
            out pdwOutVersion_Values? pdwOutVersion,
            //[Switch("*pdwOutVersion")] 
            out DRS_MSG_SPNREPLY? pmsgOut);


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
        uint IDL_DRSRemoveDsServer(
            IntPtr hDrs,
            IDL_DRSRemoveDsServer_dwInVersion_Values dwInVersion,
            //[Switch("dwInVersion")] 
            DRS_MSG_RMSVRREQ? pmsgIn,
            out IDL_DRSRemoveDsServer_pdwOutVersion_Values? pdwOutVersion,
            //[Switch("*pdwOutVersion")] 
            out DRS_MSG_RMSVRREPLY? pmsgOut);


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
        ///  to 1, because this is the only version supported.
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
        [CLSCompliant(false)]
        uint IDL_DRSRemoveDsDomain(
            IntPtr hDrs,
            IDL_DRSRemoveDsDomain_dwInVersion_Values dwInVersion,
            //[Switch("dwInVersion")] 
            DRS_MSG_RMDMNREQ? pmsgIn,
            out IDL_DRSRemoveDsDomain_pdwOutVersion_Values? pdwOutVersion,
            //[Switch("*pdwOutVersion")] 
            out DRS_MSG_RMDMNREPLY? pmsgOut);


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
        [CLSCompliant(false)]
        uint IDL_DRSDomainControllerInfo(
            IntPtr hDrs,
            uint dwInVersion,
           //[Switch("dwInVersion")]
            DRS_MSG_DCINFOREQ? pmsgIn,
            out uint? pdwOutVersion,
            //[Switch("*pdwOutVersion")]
            out DRS_MSG_DCINFOREPLY? pmsgOut);


        /// <summary>
        ///  The IDL_DRSExecuteKCC method validates the replication
        ///  interconnections of DCs and updates them if necessary.
        ///   This method is used only to diagnose, monitor, and
        ///  manage the replication topology implementation. The
        ///  structures requested and returned through this method
        ///  MAY have meaning to peer DCs and applications but are
        ///  not required for interoperation with Windows clients.
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
        [CLSCompliant(false)]
        uint IDL_DRSExecuteKCC(
            IntPtr hDrs,
            uint dwInVersion,
            //[Switch("dwInVersion")]
            DRS_MSG_KCC_EXECUTE? pmsgIn);


        /// <summary>
        ///  The IDL_DRSGetReplInfo method retrieves the replication
        ///  state of the server. This method is used only to diagnose,
        ///  monitor, and manage the replication implementation.
        ///  The structures requested and returned through this
        ///  method MAY have meaning to peer DCs and applications
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
        [CLSCompliant(false)]
        uint IDL_DRSGetReplInfo(
            IntPtr hDrs,
            uint dwInVersion,
           //[Switch("dwInVersion")] 
            DRS_MSG_GETREPLINFO_REQ? pmsgIn,
            out uint? pdwOutVersion,
            //[Switch("*pdwOutVersion")]
            out DRS_MSG_GETREPLINFO_REPLY? pmsgOut);


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
        [CLSCompliant(false)]
        uint IDL_DRSAddSidHistory(
            IntPtr hDrs,
            IDL_DRSAddSidHistory_dwInVersion_Values dwInVersion,
            //[Switch("dwInVersion")]
            DRS_MSG_ADDSIDREQ? pmsgIn,
            out IDL_DRSAddSidHistory_pdwOutVersion_Values? pdwOutVersion,
            //[Switch("*pdwOutVersion")]
            out DRS_MSG_ADDSIDREPLY? pmsgOut);


        /// <summary>
        ///  The IDL_DRSReplicaVerifyObjects method verifies the
        ///  existence of objects in an NC replica by comparing
        ///  against a replica of the same NC on a reference DC,
        ///  optionally deleting any objects that do not exist on
        ///  the reference DC. This method is used only to diagnose,
        ///  monitor, and manage the replication implementation.
        ///  The structures requested and returned through this
        ///  method MAY have meaning to peer DCs and applications
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
        [CLSCompliant(false)]
        uint IDL_DRSReplicaVerifyObjects(
            IntPtr hDrs,
            uint dwVersion,
            //[Switch("dwVersion")] 
            DRS_MSG_REPVERIFYOBJ? pmsgVerify);


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
        [CLSCompliant(false)]
        uint IDL_DRSQuerySitesByCost(
            IntPtr hDrs,
            uint dwInVersion,
            //[Switch("dwInVersion")]
            DRS_MSG_QUERYSITESREQ? pmsgIn,
            out uint? pdwOutVersion,
            //[Switch("*pdwOutVersion")]
            out DRS_MSG_QUERYSITESREPLY? pmsgOut);


        /// <summary>
        ///  The IDL_DRSInitDemotion method performs the first phase
        ///  of the removal of a DC from an AD LDS forest. This method
        ///  is supported only by AD LDS. This method is used only
        ///  to diagnose, monitor, and manage the implementation
        ///  of server-to-server DC demotion. The structures requested
        ///  and returned through this method MAY have meaning to
        ///  peer DCs and applications but are not required for
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
        [CLSCompliant(false)]
        uint IDL_DRSInitDemotion(
            IntPtr hDrs,
            uint dwInVersion,
            //[Switch("dwInVersion")]
            DRS_MSG_INIT_DEMOTIONREQ? pmsgIn,
            out uint? pdwOutVersion,
            //[Switch("*pdwOutVersion")] 
            out DRS_MSG_INIT_DEMOTIONREPLY? pmsgOut);


        /// <summary>
        ///  The IDL_DRSReplicaDemotion method  replicates initiates
        ///  server-to-server replication to replicate  off all
        ///  changes to the specified NC and moves any FSMOs held
        ///  to another server. This method is supported only by
        ///  AD LDS. This method is used only to diagnose, monitor,
        ///  and manage the replication and FSMO implementation
        ///  related to DC demotion. The structures requested and
        ///  returned through this method MAY have meaning to peer
        ///  DCs and applications but are not required for interoperation
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
        [CLSCompliant(false)]
        uint IDL_DRSReplicaDemotion(
            IntPtr hDrs,
            uint dwInVersion,
            //[Switch("dwInVersion")]
            DRS_MSG_REPLICA_DEMOTIONREQ? pmsgIn,
            out uint? pdwOutVersion,
            //[Switch("*pdwOutVersion")]
            out DRS_MSG_REPLICA_DEMOTIONREPLY? pmsgOut);


        /// <summary>
        ///  The IDL_DRSFinishDemotion method either performs one
        ///  or more steps toward the complete removal of a DC from
        ///  an AD LDS forest, or it undoes the effects of the first
        ///  phase of removal (performed by IDL_DRSInitDemotion).
        ///  This method is supported by AD LDS only. This method
        ///  is used only to diagnose, monitor, and manage the implementation
        ///  of server-to-server DC demotion. The structures requested
        ///  and returned through this method MAY have meaning to
        ///  peer DCs and applications but are not required for
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
        [CLSCompliant(false)]
        uint IDL_DRSFinishDemotion(
            IntPtr hDrs,
            uint dwInVersion,
           //[Switch("dwInVersion")] 
            DRS_MSG_FINISH_DEMOTIONREQ? pmsgIn,
            out uint? pdwOutVersion,
            //[Switch("*pdwOutVersion")] 
            out DRS_MSG_FINISH_DEMOTIONREPLY? pmsgOut);


        /// <summary>
        ///  The IDL_DRSGetNCChanges method replicates updates from an NC replica on the server. 
        ///  Opnum: 3 
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
        uint IDL_DRSGetNCChanges(
            IntPtr hDrs,
            uint dwInVersion,
            //[Switch("dwInVersion")] 
            DRS_MSG_GETCHGREQ? pmsgIn,
            out uint? pdwOutVersion,
            //[Switch("*pdwOutVersion")]
            out DRS_MSG_GETCHGREPLY? pmsgOut);


        /// <summary>
        ///  The IDL_DRSVerifyNames method resolves a sequence of object identities. 
        ///  Opnum: 8 
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
        uint IDL_DRSVerifyNames(
            IntPtr hDrs,
            uint dwInVersion,
            //[Switch("dwInVersion")] 
            DRS_MSG_VERIFYREQ? pmsgIn,
            out uint? pdwOutVersion,
            //[Switch("*pdwOutVersion")]
            out DRS_MSG_VERIFYREPLY? pmsgOut);


        /// <summary>
        ///  The IDL_DRSGetMemberships method retrieves group membership for an object. 
        ///  Opnum: 9 
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
        uint IDL_DRSGetMemberships(
            IntPtr hDrs,
            uint dwInVersion,
            //[Switch("dwInVersion")] 
            DRS_MSG_REVMEMB_REQ? pmsgIn,
            out uint? pdwOutVersion,
           //[Switch("*pdwOutVersion")] 
            out DRS_MSG_REVMEMB_REPLY? pmsgOut);


        /// <summary>
        ///  The IDL_DRSInterDomainMove method is a helper method used in a cross-NC move LDAP operation.
        ///  Opnum: 10 
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
        uint IDL_DRSInterDomainMove(
            IntPtr hDrs,
            uint dwInVersion,
            //[Switch("dwInVersion")]
            DRS_MSG_MOVEREQ? pmsgIn,
            out uint? pdwOutVersion,
           //[Switch("*pdwOutVersion")]
            out DRS_MSG_MOVEREPLY? pmsgOut);


        /// <summary>
        ///  If the server is the PDC emulator FSMO role owner, the IDL_DRSGetNT4ChangeLog method
        ///  returns either a sequence of PDC change log entries or the NT4 replication state, or
        ///  both, as requested by the client. 
        ///  Opnum: 11 
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
        uint IDL_DRSGetNT4ChangeLog(
            IntPtr hDrs,
            uint dwInVersion,
            //[Switch("dwInVersion")] 
            DRS_MSG_NT4_CHGLOG_REQ? pmsgIn,
            out uint? pdwOutVersion,
           //[Switch("*pdwOutVersion")]
            out DRS_MSG_NT4_CHGLOG_REPLY? pmsgOut);


        /// <summary>
        ///  The IDL_DRSAddEntry method adds one or more objects. 
        ///  Opnum: 17 
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
        uint IDL_DRSAddEntry(
            IntPtr hDrs,
            uint dwInVersion,
            //[Switch("dwInVersion")] 
            DRS_MSG_ADDENTRYREQ? pmsgIn,
            out uint? pdwOutVersion,
           //[Switch("*pdwOutVersion")] 
            out DRS_MSG_ADDENTRYREPLY? pmsgOut);


        /// <summary>
        ///  The IDL_DRSGetMemberships2 method retrieves group memberships for a sequence of objects. 
        ///  Opnum: 21 
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
        uint IDL_DRSGetMemberships2(
            IntPtr hDrs,
            uint dwInVersion,
            //[Switch("dwInVersion")]
            DRS_MSG_GETMEMBERSHIPS2_REQ? pmsgIn,
            out uint? pdwOutVersion,
           //[Switch("*pdwOutVersion")]
            out DRS_MSG_GETMEMBERSHIPS2_REPLY? pmsgOut);


        /// <summary>
        ///  The IDL_DRSGetObjectExistence method helps the client check the consistency of object
        ///  existence between its replica of an NC and the server's replica of the same NC. 
        ///  Checking the consistency of object existence means identifying objects that have 
        ///  replicated to both replicas and that exist in one replica but not in the other.
        ///  For the purposes of this method, an object exists within a NC replica if it is either
        ///  an object or a tombstone. See IDL_DRSReplicaVerifyObjects for a use of this method. 
        ///  Opnum: 23 
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
        uint IDL_DRSGetObjectExistence(
            IntPtr hDrs,
            uint dwInVersion,
            //[Switch("dwInVersion")] 
            DRS_MSG_EXISTREQ? pmsgIn,
            out uint? pdwOutVersion,
           //[Switch("*pdwOutVersion")] 
            out DRS_MSG_EXISTREPLY? pmsgOut);

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
            IntPtr hDrs,
            uint dwInVersion,
            DRS_MSG_ADDCLONEDCREQ? pmsgIn,
            out uint? pdwOutVersion,
            out DRS_MSG_ADDCLONEDCREPLY? pmsgOut);

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
        [CLSCompliant(false)]
        uint IDL_DRSWriteNgcKey(
            IntPtr hDrs,
            uint dwInVersion,
            //[Switch("dwInVersion")]
            DRS_MSG_WRITENGCKEYREQ? pmsgIn,
            out uint? pdwOutVersion,
            //[Switch("*pdwOutVersion")] 
            out DRS_MSG_WRITENGCKEYREPLY? pmsgOut);

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
        uint IDL_DRSReadNgcKey(
            IntPtr hDrs,
            uint dwInVersion,
            //[Switch("dwInVersion")]
            DRS_MSG_READNGCKEYREQ? pmsgIn,
            out uint? pdwOutVersion,
            //[Switch("*pdwOutVersion")] 
            out DRS_MSG_READNGCKEYREPLY? pmsgOut);
    }
}
