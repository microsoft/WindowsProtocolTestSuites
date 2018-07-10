// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Protocols.TestTools;
using Microsoft.Protocols.TestTools.StackSdk.ActiveDirectory.Drsr;

namespace Microsoft.Protocols.TestSuites.ActiveDirectory.Drsr
{
    /// <summary>
    /// This interface is used for encapsulate single DRS call and the basic data structure verification process
    /// </summary>
    public interface IDRSRTestClient
    {
        /// <summary>
        /// Initialize the test client
        /// </summary>
        /// <param name="site">TestSite from PTF</param>
        void Initialize(ITestSite site);

        /// <summary>
        /// reset test client, dispose all resource and revert changes
        /// </summary>
        void Reset();

        /// <summary>
        /// sync two DCs
        /// </summary>
        /// <param name="dc1">dc 1</param>
        /// <param name="dc2">dc 2</param>
        /// <returns>true if succeeded</returns>
        bool SyncDCs(EnvironmentConfig.Machine dc1, EnvironmentConfig.Machine dc2);

        ITestSite Site { get; }

        #region context handle
        /// <summary>
        /// Set up RPC session and call IDL_DRSBind to specific AD DS or LDS instance
        /// </summary>
        /// <param name="machine">AD DS or LDS instance on specified SUT</param>
        /// <param name="user">user account for this DRSR session</param>
        /// <param name="extFlags">dwFlagsEx in DRS_EXETENSIONS structure</param>
        /// <returns>0 if IDL_DRSBind success, otherwise, failed</returns>
        uint DrsBind(EnvironmentConfig.Machine machine, EnvironmentConfig.User user, DRS_EXTENSIONS_IN_FLAGS extFlags, DRS_EXTENSIONS_IN_FLAGSEXT extFlagsExt, uint extCap);

        uint DrsBind(EnvironmentConfig.Machine machine, EnvironmentConfig.User user, DRS_EXTENSIONS_IN_FLAGS extFlags);
        /// <summary>
        /// Call IDL_DRSUnbind to release specified DRS handle
        /// </summary>
        /// <param name="machine">AD DS or LDS instance on specified SUT</param>
        /// <returns>0 if IDL_DRSBind success, otherwise, failed</returns>
        uint DrsUnbind(EnvironmentConfig.Machine machine, bool remove = true);

        #endregion

        #region Replication Methods

        /// <summary>
        /// Calling IDL_DRSGetReplInfo method to retrieve the replication state of the server, and verify the server response.
        /// </summary>
        /// <param name="svr">the bind DC response to the request</param>
        /// <param name="reqVersion">The version of the request.</param>
        /// <param name="neighborDC">The replication neighbor of the called DC server. It can be none.</param>
        /// <param name="infoType">The operation type code.</param>
        /// <param name="objectDN">DN of the object on which the operation should be performed. 
        /// The meaning of this parameter depends on the value of the infoType parameter.</param>
        /// <param name="attrName">Null, or the lDAPDisplayName of a link attribute</param>
        /// <param name="attrValueDN">Null, or the DN of the link value for which to retrieve a stamp.</param>
        /// <param name="outVersion">A pointer to the version of the response message.</param>
        /// <param name="outMessage">A pointer to the response message.</param>
        /// <returns>The return code of server.</returns>
        uint DrsGetReplInfo(
            EnvironmentConfig.Machine svr,
            DrsGetReplInfo_Versions reqVer,
            EnvironmentConfig.Machine neighborDC,
            DS_REPL_INFO infoType,
            string objectDN,
            string attrName,
            string attrValueDN,
            out uint? outVersion,
            out DRS_MSG_GETREPLINFO_REPLY? outMessage);

        /// <summary>
        /// Calling  IDL_DRSReplicaSync method to trigger replication from another DC, and verify the server response.
        /// </summary>
        ///  <param name="svr">the bind DC response to the request</param>
        /// <param name="reqVer">The version of request.</param>
        /// <param name="sourceDC">The source DC where to replicated from. None if replicate from all partners.</param>
        /// <param name="options">The operation options.</param>
        /// <param name="bUseDsaGuid">True if use the DSA GUID to specify the source DC, false if use NetworkAddress.</param>
        /// <param name="ncType">The naming context type.</param>
        /// <returns>The return code of server.</returns>
        uint DrsReplicaSync(
            EnvironmentConfig.Machine svr,
            DrsReplicaSync_Versions reqVer,
            EnvironmentConfig.Machine sourceDC,
            DRS_OPTIONS options,
            bool bUseDsaGuid,
            NamingContext ncType);


        uint DrsGetNCChangesV2(
            EnvironmentConfig.Machine sourceEnum,
            DsServer source,
            DsServer dest,
            string objectDn,
            USN_VECTOR usnvecFrom,
            UPTODATE_VECTOR_V1_EXT utdvecDest,
            bool withSecrets,
            out uint? outVersion,
            out DRS_MSG_GETCHGREPLY? outMessage);
        /// <summary>
        /// Calling IDL_DRSGetNCChanges method to replicate updates from an NC replica on the server, and verify the server response.
        /// </summary>
        ///  <param name="svr">the bind DC response to the request</param>
        /// <param name="reqVer">The version of the request.</param>
        ///  <param name="destDC">Dsa Obj Dest DC to getting the changes</param>
        /// <param name="options">The operation options.</param>
        /// <param name="ncType">The naming context type.</param>
        /// <param name="exopCode">The code of extended operation.</param>
        /// <param name="fsmoRole">The fsmo role to perform the extended operation.</param>
        /// <param name="objectDN">Null, or the DN of an object. It is required when exopCode is either EXOP_REPL_OBJ or EXOP_REPL_SECRETS.</param>
        /// <param name="outVersion">A pointer to the version of the response message.</param>
        /// <param name="outMessage"> A pointer to the response message.</param>
        /// <returns>The return code of server.</returns>
        uint DrsGetNCChanges(
            EnvironmentConfig.Machine svr,
            DrsGetNCChanges_Versions reqVer,
            EnvironmentConfig.Machine destDC,
            DRS_OPTIONS options,
            NamingContext ncType,
            EXOP_REQ_Codes exopCode,
            FSMORoles fsmoRole,
            string objectDN,
            out uint? outVersion,
            out DRS_MSG_GETCHGREPLY? outMessage);

        /// <summary>
        /// Calling IDL_DRSReplicaVerifyObjects method to verify the existence of objects in an NC replica by comparing against a replica of the same NC on a reference DC.
        /// </summary>
        ///  <param name="svr">the bind DC response to the request</param>
        /// <param name="reqVer">The version of the request.</param>
        /// <param name="referenceDC">The reference DC.</param>
        /// <param name="options">The operation options.</param>
        /// <param name="ncType">The naming context type.</param>
        ///<returns>The return code of server.</returns>
        uint DrsReplicaVerifyObjects(
            EnvironmentConfig.Machine svr,
            DrsReplicaVerifyObjects_Versions reqVer,
            EnvironmentConfig.Machine referenceDC,
            DRS_MSG_REPVERIFYOBJ_OPTIONS options,
            NamingContext ncType);

        /// <summary>
        /// Calling IDL_DRSGetObjectExistence to check the consistency of object existence between its replica of an NC and the server's replica of the same NC. 
        /// </summary>
        ///  <param name="svr">the bind DC response to the request</param>
        /// <param name="reqVer">The version of request.</param>
        /// <param name="utdFilter">The filter excluding objects from the client's object sequence.</param>
        /// <param name="clientGuidSequence">The GUID sequence on caller DC.</param>
        /// <param name="flag">Flag that indicates how to set the MD5Digest.</param>
        /// <param name="flag">The naming context type.</param>
        /// <param name="outVersion">A pointer to the version of the response message.</param>
        /// <param name="outMessage"> A pointer to the response message.</param>
        /// <returns>The return code of server.</returns>
        uint DrsGetObjectExistence(
            EnvironmentConfig.Machine svr,
            DrsGetObjectExistence_Versions reqVer,
            UPTODATE_VECTOR_V1_EXT utdFilter,
            Md5Digest_Flags flag,
            Guid[] clientGuidSequence,
            NamingContext ncType,
            out uint? outVersion,
            out DRS_MSG_EXISTREPLY? outMessage);


        #endregion

        #region DC locate
        /// <summary>
        /// call IDL_DRSDomainControllerInfo to get DC info by requested
        /// </summary>
        /// <param name="machine">AD DS or LDS DC</param>
        /// <param name="reqVer">request version</param>
        /// <param name="name">valid FQDN or Netbios name or invalided one</param>
        /// <param name="infoLv">InfoLevel of response</param>
        /// <returns>result code</returns>
        uint DrsDomainControllerInfo(EnvironmentConfig.Machine machine, DRS_MSG_DCINFOREQ_Versions reqVer, string name, DRS_MSG_DCINFOREQ_INFOLEVEL infoLv);

        /// <summary>
        /// call IDL_DRSQuerySitesByCost to get site replication cost
        /// </summary>
        /// <param name="machine">AD DS or LDS DC</param>
        /// <param name="reqVer">request version</param>
        /// <param name="from">"from" this site</param>
        /// <param name="to">"to" these sites</param>
        /// <param name="cToSitesCorrect">if true, cToSItes will be number of "to" items, otherwise, will change it to a different value than that number</param>
        /// <returns>result code</returns>
        uint DrsQuerySitesByCost(EnvironmentConfig.Machine machine, DRS_MSG_QUERYSITESREQ_Versions reqVer, DsSite from, DsSite[] to, bool cToSitesCorrect = true);
        #endregion

        #region KCC
        /// <summary>
        /// IDL_DRSExecuteKCC
        /// </summary>
        /// <param name="machine">SUT</param>
        /// <param name="isAsync">async or sync</param>
        /// <returns>return value</returns>
        uint DrsExecuteKCC(EnvironmentConfig.Machine machine, bool isAsync);

        /// <summary>
        /// IDL_DRSReplicaAdd
        /// </summary>
        /// <param name="machine">SUT</param>
        /// <param name="inVer">request version</param>
        /// <param name="src">replication source</param>
        /// <param name="options">options</param>
        /// <returns>return value</returns>
        uint DrsReplicaAdd(EnvironmentConfig.Machine machine, DRS_MSG_REPADD_Versions inVer, DsServer src, DRS_OPTIONS options, NamingContext nc = NamingContext.ConfigNC);

        /// <summary>
        /// IDL_DRSReplicaDel
        /// </summary>
        /// <param name="machine">SUT</param>
        /// <param name="src">replication source</param>
        /// <param name="options">options</param>
        /// <returns>return value</returns>
        uint DrsReplicaDel(EnvironmentConfig.Machine machine, DsServer src, DRS_OPTIONS options, NamingContext nc = NamingContext.ConfigNC);

        /// <summary>
        /// IDL_DRSReplicaModify
        /// </summary>
        /// <param name="machine">SUT</param>
        /// <param name="src">replication source</param>
        /// <param name="replicaFlags">replication flags</param>
        /// <param name="modifyFields">modified fields</param>
        /// <param name="options">options</param>
        /// <returns>return value</returns>
        uint DrsReplicaModify(EnvironmentConfig.Machine machine, DsServer src, DRS_OPTIONS replicaFlags, DRS_MSG_REPMOD_FIELDS modifyFields, DRS_OPTIONS options);

        /// <summary>
        /// IDL_DRSUpdateRefs
        /// </summary>
        /// <param name="machine">SUT</param>
        /// <param name="reqVer">request version of DrsUpdateRefs</param>
        /// <param name="dest">replication destination</param>
        /// <param name="options">options</param>
        /// <returns>return value</returns>
        uint DrsUpdateRefs(EnvironmentConfig.Machine machine, DrsUpdateRefs_Versions reqVer, DsServer dest, DRS_OPTIONS options, NamingContext nc = NamingContext.ConfigNC);
        #endregion

        #region Administration Tool Support
        /// <summary>
        /// call DrsAddEntry to adds one or more objects
        /// </summary>
        /// <param name="svr">bind DC</param>
        /// <param name="reqVer">the version of the request</param>
        /// <param name="EntInfList">List contains ENTINF objects</param>
        /// <param name="pClientCreds">Client credentials</param>
        /// <param name="outVersion">the version of the reply</param>
        /// <param name="outMessage">the reply message</param>
        /// <returns>The return code of server.</returns>
        uint DrsAddEntry(
            EnvironmentConfig.Machine svr,
            DRS_AddEntry_Versions reqVer,
            ENTINFLIST EntInfList,
            DRS_SecBufferDesc[] pClientCreds,
            out uint? outVersion,
            out DRS_MSG_ADDENTRYREPLY? outMessage);

        /// <summary>
        /// call DrsAddSidHistory to adds one or more SIDs to the sIDHistory attribute of a given object
        /// </summary>
        /// <param name="svr">bind DC, use for DstDomain</param>
        /// <param name="reqVer">the version of the request</param>
        /// <param name="flag">DRS_ADDSID_FLAGS flag</param>
        /// <param name="srcDN">the setting of SrcPrincipal</param>
        /// <param name="desDN">the setting of desDN</param>
        /// <param name="pDC">setting for SrcDomain, SrcCredsDomain and SrcCredsDomain</param>
        /// <param name="User">setting for SrcCredsUser and SrcCredsPassword</param>
        /// <returns>The return code of server.</returns>
        uint DrsAddSidHistory(
            EnvironmentConfig.Machine svr,
            EnvironmentConfig.User cred,
            DomainEnum srcDomain,
            DomainEnum destDomain,
            IDL_DRSAddSidHistory_dwInVersion_Values reqVer,
            DRS_ADDSID_FLAGS flag,
            string srcDN,
            ObjectNameType srcNameType,
            string desDN,
            ObjectNameType destNameType,
            EnvironmentConfig.Machine srcDC,
            out IDL_DRSAddSidHistory_pdwOutVersion_Values? outVersion,
            out DRS_MSG_ADDSIDREPLY? outMessage
            );

        /// <summary>
        /// call DrsWriteSPN to updates the set of SPNs on an object
        /// </summary>
        /// <param name="svr">drsr bind machine</param>
        /// <param name="reqVer">the version of the request</param>
        /// <param name="operation">Operation to the spn</param>
        /// <param name="ncType">setting pwszAccount:  The DN of the object to modify</param>
        /// <param name="sPNList">sPN for the operation</param>
        /// <param name="outVersion">the version of the reply</param>
        /// <param name="outMessage">the reply message</param>
        /// <returns></returns>
        uint DrsWriteSPN(
            EnvironmentConfig.Machine svr,
            dwInVersion_Values reqVer,
            DS_SPN_OPREATION operation,
            string dn,
            string[] sPNList,
            out pdwOutVersion_Values? outVersion,
            out DRS_MSG_SPNREPLY? outMessage
            );

        /// <summary>
        /// Call IDL_DRSRemoveDsServer to perform remove actions of DCs.
        /// </summary>
        /// <param name="svr">DRS-binded DC.</param>
        /// <param name="reqVer">Request version.</param>
        /// <param name="serverDn">The DN of the server object of the DC to remove.</param>
        /// <param name="domainDn">The DN of the NC root of the domain that the DC to be removed belongs to.</param>
        /// <param name="commit">True if the DC's metadata should be actually removed. False otherwise.</param>
        /// <returns></returns>
        uint DrsRemoveDsServer(
            EnvironmentConfig.Machine svr,
            IDL_DRSRemoveDsServer_dwInVersion_Values reqVer,
            string serverDn,
            string domainDn,
            bool commit
            );

        /// <summary>
        /// Call IDL_DRSRemoveDsDomain to perform remove actions of domain.
        /// </summary>
        /// <param name="svr">DRS-binded DC.</param>
        /// <param name="reqVer">Request version.</param>
        /// <param name="domainDn">The DN of the domain to be removed.</param>
        /// <returns></returns>
        uint DrsRemoveDsDomain(
            EnvironmentConfig.Machine svr,
            IDL_DRSRemoveDsDomain_dwInVersion_Values reqVer,
            string domainDn
            );
        #endregion

        #region Lookups
        /// <summary>
        /// Wrapper for IDL_DRSGetMemberships.
        /// </summary>
        /// <param name="svr">DRSBinded machine.</param>
        /// <param name="reqVer">Request version.</param>
        /// <param name="name">The DSName of the object whose reverse membership is being requested.</param>
        /// <param name="isGetAttribute">Whether the attributes of the group membership is being queried.</param>
        /// <param name="operationType">Type of evaluation.</param>
        /// <param name="limitingDomain">Domain filter.</param>
        /// <returns>Return code.</returns>
        uint DrsGetMemberships(
            EnvironmentConfig.Machine svr,
            dwInVersion_Values reqVer,
            DSNAME name,
            bool isGetAttribute,
            REVERSE_MEMBERSHIP_OPERATION_TYPE operationType,
            DSNAME? limitingDomain);

        /// <summary>
        /// Wrapper for IDL_DRSGetMemberships2.
        /// Note this wrapper creates a consecutive 7 identical requests.
        /// </summary>
        /// <param name="svr">DRSBinded machine.</param>
        /// <param name="reqVer">Request version.</param>
        /// <param name="name">The DSName of the object whose reverse membership is being requested.</param>
        /// <param name="isGetAttribute">Whether the attributes of the group membership is being queried.</param>
        /// <param name="operationType">Type of evaluation.</param>
        /// <param name="limitingDomain">Domain filter.</param>
        /// <returns>Return code.</returns>
        uint DrsGetMemberships2(
            EnvironmentConfig.Machine svr,
            dwInVersion_Values reqVer,
            DSNAME name,
            bool isGetAttribute,
            REVERSE_MEMBERSHIP_OPERATION_TYPE operationType,
            DSNAME limitingDomain);

        /// <summary>
        /// Wrapper for IDL_DRSCrackNames.
        /// </summary>
        /// <param name="svr">DRSBinded machine.</param>
        /// <param name="flags">Flags for CrackNames operation.</param>
        /// <param name="formatOffered">Format of the given names.</param>
        /// <param name="formatDesired">Format desired on return.</param>
        /// <param name="names">Names to be cracked.</param>
        /// <returns>Return code.</returns>
        uint DrsCrackNames(
            EnvironmentConfig.Machine svr,
            DRS_MSG_CRACKREQ_FLAGS flags,
            uint formatOffered,
            uint formatDesired,
            string[] names);

        /// <summary>
        /// Wrapper for IDL_DRSVerifyNames.
        /// </summary>
        /// <param name="svr">DRSBinded machine.</param>
        /// <param name="reqVer">Request version.</param>
        /// <param name="flags">Flags for VerifyNames operation.</param>
        /// <param name="names">DSNames of the objects to be verified.</param>
        /// <param name="objectDNs">DNs of the objects in names.</param>
        /// <param name="requiredAttrs">Attributes of the verified objects.</param>
        /// <param name="prefixTable">A prefix table used to translate ATTRTYPE to OID values.</param>
        /// <returns>Return code.</returns>
        uint DrsVerifyNames(
            EnvironmentConfig.Machine svr,
            dwInVersion_Values reqVer,
            DRS_MSG_VERIFYREQ_V1_dwFlags_Values flags,
            DSNAME[] names,
            string[] objectDNs,
            ATTRBLOCK requiredAttrs,
            SCHEMA_PREFIX_TABLE prefixTable);

        #endregion

        #region LDS Demotion
        /// <summary>
        /// init a lds demotion
        /// </summary>
        /// <param name="svr">target</param>
        /// <returns>return value</returns>
        uint DrsInitDemotion(EnvironmentConfig.Machine svr);

        /// <summary>
        /// finish lds demotion
        /// </summary>
        /// <param name="svr">target</param>
        /// <param name="ops">operations</param>
        /// <param name="verificationPartner">partner for verification</param>
        /// <returns>return value</returns>
        uint DrsFinishDemotion(EnvironmentConfig.Machine svr, DRS_MSG_FINISH_DEMOTIONREQ_OPERATIONS ops, EnvironmentConfig.Machine verificationPartner = EnvironmentConfig.Machine.None);
        #endregion

        #region clone DC
        /// <summary>
        /// clone a dc
        /// </summary>
        /// <param name="svr">target dc</param>
        /// <param name="machinename">cloned dc name</param>
        /// <param name="sitename">site where cloned dc is</param>
        /// <returns>return value</returns>
        uint DrsAddCloneDC(EnvironmentConfig.Machine svr, string machinename, string sitename);
        #endregion

        #region replica demotion
        /// <summary>
        /// IDL_DRSReplicaDemotion
        /// </summary>
        /// <param name="svr">target</param>
        /// <param name="flag">DRS_MSG_REPLICA_DEMOTIONREQ_FLAGS</param>
        /// <param name="ncType">NC type</param>
        /// <param name="verificationPartner">verification partner</param>
        /// <returns>return value</returns>
        uint DrsReplicaDemotion(EnvironmentConfig.Machine svr, DRS_MSG_REPLICA_DEMOTIONREQ_FLAGS flag, NamingContext ncType, EnvironmentConfig.Machine verificationPartner);

        #endregion

        #region Ngc Key
        /// <summary>
        /// Calling IDL_DRSWriteNgcKey method to compose and update the msDS-KeyCredentialLink value on an object
        /// </summary>
        /// <param name="svr">drsr bind machine</param>
        /// <param name="reqVer">the version of the request</param>
        /// <param name="dn">setting pwszAccount:  The DN of the object to modify</param>
        /// <param name="ngcKey">Ngc Key for the operation</param>
        /// <param name="outVersion">the version of the reply</param>
        /// <param name="outMessage">the reply message</param>
        /// <returns></returns>
        uint DrsWriteNgcKey(
            EnvironmentConfig.Machine svr,
            uint reqVer,
            string dn,
            byte[] ngcKey,
            out uint? outVersion,
            out DRS_MSG_WRITENGCKEYREPLY? outMessage
            );

        /// <summary>
        /// Calling IDL_DRSReadNgcKey method to read and parse the msDS-KeyCredentialLink value on an object
        /// </summary>
        /// <param name="svr">drsr bind machine</param>
        /// <param name="reqVer">the version of the request</param>
        /// <param name="dn">setting pwszAccount:  The DN of the object to modify</param>
        /// <param name="ngcKey">Ngc Key for the operation</param>
        /// <param name="outVersion">the version of the reply</param>
        /// <param name="outMessage">the reply message</param>
        /// <returns></returns>
        uint DrsReadNgcKey(
            EnvironmentConfig.Machine svr,
            uint reqVer,
            string dn,
            out uint? outVersion,
            out DRS_MSG_READNGCKEYREPLY? outMessage
            );
        #endregion

        /// <summary>
        /// Wrapper for IDL_DRSInterDomainMove method which is a helper method used in a cross-NC move LDAP operation.
        /// </summary>
        /// <param name="sourceDc">The source DC.</param>
        /// <param name="reqVer">The request version.</param>
        /// <param name="targetDc">The target DC.</param>
        /// <param name="oldDN">The old distinguish name of the object to be moved.</param>
        /// <param name="newDN">The new DN of the object.</param>
        /// <param name="replyMsg">The reply message.</param>
        /// <returns>0 if successful, otherwise a Windows error code.</returns>
        uint DrsInterDomainMove(EnvironmentConfig.Machine sourceDc, DRSInterDomainMove_Versions reqVer, EnvironmentConfig.Machine targetDc, string oldDN, string newDN, out DRS_MSG_MOVEREPLY replyMsg);

        #region dsaop RPC methods

        /// <summary>
        /// RPC bind to dsaop interface of server.
        /// </summary>
        /// <param name="svr">The server to bind.</param>
        /// <param name="user">The user credential.</param>
        /// <param name="ifType">The RPC interface type.</param>
        /// <returns>The RPC handle.</returns>
        void RPCBind(EnvironmentConfig.Machine svr, EnvironmentConfig.User user, DrsrRpcInterfaceType ifType = DrsrRpcInterfaceType.DRSUAPI);

        /// <summary>
        /// Wrapper for IDL_DSAPrepareScript.
        /// </summary>
        /// <param name="svr">The DC server.</param>
        /// <param name="inMessage">A pointer to the request message.</param>
        /// <param name="outVersion">A pointer to the version of the response message.</param>
        /// <returns>0 if successful, or a Windows error code if a failure occurs.</returns>
        uint DsaPrepareScript(EnvironmentConfig.Machine svr, DSAPrepareScript_Versions reqVer, out uint? outVersion, out DSA_MSG_PREPARE_SCRIPT_REPLY? outMessage);

        /// <summary>
        /// IDL_DSAExecuteScript 
        /// </summary>
        /// <param name="svr">The DC server.</param>
        /// <param name="pwdBytes">The password in bytes.</param>
        /// <param name="inMessage">A pointer to the request message.</param>
        /// <param name="outVersion">A pointer to the version of the response message.</param>
        /// <returns>0 if successful, or a Windows error code if a failure occurs.</returns>
        uint DsaExecuteScript(EnvironmentConfig.Machine svr, DSAExecuteScript_Versions reqVer, byte[] pwdBytes, out uint? outVersion, out DSA_MSG_EXECUTE_SCRIPT_REPLY? outMessage);

        #endregion

    }
}
