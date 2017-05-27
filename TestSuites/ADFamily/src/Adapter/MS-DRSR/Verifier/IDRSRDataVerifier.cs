// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Protocols.TestTools;
using Microsoft.Protocols.TestTools.StackSdk.Security.Sspi;
using Microsoft.Protocols.TestTools.StackSdk.ActiveDirectory.Drsr;

namespace Microsoft.Protocols.TestSuites.ActiveDirectory.Drsr
{
    /// <summary>
    /// This interface is used to verify data structure for requests and response in DRS calls
    /// </summary>
    public interface IDRSRVerifier
    {
        /// <summary>
        /// Initialize via PTF TestSite
        /// </summary>
        /// <param name="site">PTF Testsite</param>
        void Initilize(ITestSite site);

        /// <summary>
        /// Verify IDL_DRSBind
        /// </summary>
        /// <param name="machine">AD DS or LDS instance</param>
        /// <param name="user">the user credential used for DRSR session</param>
        /// <param name="drsHandle">DRS Handle</param>
        /// <param name="retVal">return value of IDL_DRSBind</param>
        /// <param name="clientDsa">puuidClientDsa in request</param>
        /// <param name="clientExt">pextClient in request</param>
        /// <param name="serverExt">ppextServer in response</param>
        void VerifyDRSBind(EnvironmentConfig.Machine machine, EnvironmentConfig.User user, IntPtr drsHandle, uint retVal, DRS_EXTENSIONS_INT serverExt);

        /// <summary>
        /// Verify IDL_DRSUnbind
        /// </summary>
        /// <param name="retVal">return value of IDL_DRSUnbind</param>
        void VerifyDRSUnbind(uint retVal);

        #region replication
        /// <summary>
        /// verify DrsGetReplInfo API
        /// </summary>
        /// <param name="dwInVersion">request message version</param>
        /// <param name="inMessage">request message</param>
        /// <param name="outVersion">response message version</param>
        /// <param name="outMessage">response message version</param>
        void VerifyDrsGetReplInfo(
            uint dwInVersion,
            DRS_MSG_GETREPLINFO_REQ? inMessage,
            uint? outVersion,
            DRS_MSG_GETREPLINFO_REPLY? outMessage);

        /// <summary>
        /// verify DrsGetNCChanges API
        /// </summary>
        /// <param name="dwInVersion">request message version</param>
        /// <param name="inMessage">request message</param>       
        /// <param name="outVersion">response message version</param>
        /// <param name="outMessage">response message version</param>
        void VerifyDrsGetNCChanges(
            uint retVal,
            uint dwInVersion,
            DRS_MSG_GETCHGREQ? inMessage,           
            uint? outVersion,
            DRS_MSG_GETCHGREPLY? outMessage);        

        /// <summary>
        /// verify DrsGetObjectExistence API
        /// </summary>
        /// <param name="dwInVersion">request message version</param>
        /// <param name="inMessage">request message</param>
        /// <param name="outVersion">response message version</param>
        /// <param name="outMessage">response message version</param>
        void VerifyDrsGetObjectExistence(
            uint dwInVersion,
            DRS_MSG_EXISTREQ? inMessage,
            uint? outVersion,
            DRS_MSG_EXISTREPLY? outMessage);
        #endregion
        #region dc locate
        /// <summary>
        /// Verify DRSDomainControllerInfo API
        /// </summary>
        /// <param name="retVal">return value</param>
        /// <param name="outVer">response version</param>
        /// <param name="req">request data</param>
        /// <param name="reply">response data</param>
        void VerifyDRSDomainControllerInfo(uint retVal, EnvironmentConfig.Machine svr, uint? outVer, DRS_MSG_DCINFOREQ? req, DRS_MSG_DCINFOREPLY? reply);

        /// <summary>
        /// Verify DRSQuerySitesByCost API
        /// </summary>
        /// <param name="retVal">return value</param>
        /// <param name="outVer">response version</param>
        /// <param name="from">"from" this site</param>
        /// <param name="tp">"to" these sites</param>
        /// <param name="reply">response data</param>
        void VerifyDRSQuerySitesByCost(uint retVal, EnvironmentConfig.Machine svr, uint? outVer, DsSite from, DsSite[] to, DRS_MSG_QUERYSITESREPLY? reply);
        #endregion
        #region KCC
        /// <summary>
        /// Verify DRSExecuteKCC API
        /// </summary>
        /// <param name="retVal">return value</param>
        void VerifyDRSExecuteKCC(uint retVal);

        /// <summary>
        /// Verify DRSReplicaAdd API
        /// </summary>
        /// <param name="retVal">return value</param>
        /// <param name="machine">SUT</param>
        /// <param name="src">replication source</param>
        /// <param name="options">options</param>
        void VerifyDRSReplicaAdd(uint retVal, EnvironmentConfig.Machine machine, DsServer src, DRS_OPTIONS options);

        /// <summary>
        /// verify DRSReplicaDel API
        /// </summary>
        /// <param name="retVal">return value</param>
        /// <param name="machine">SUT</param>
        /// <param name="src">replication source</param>
        /// <param name="options">options</param>
        void VerifyDRSReplicaDel(uint retVal, EnvironmentConfig.Machine machine, DsServer src, DRS_OPTIONS options);

        /// <summary>
        /// Verify DRSReplicaModify API
        /// </summary>
        /// <param name="retVal">return value</param>
        /// <param name="machine">SUT</param>
        /// <param name="src">replication source</param>
        /// <param name="replicaFlags">replication flags</param>
        /// <param name="fields">modified fields</param>
        /// <param name="options">options</param>
        void VerifyDRSReplicaModify(uint retVal, EnvironmentConfig.Machine machine, DsServer src, DRS_OPTIONS replicaFlags, DRS_MSG_REPMOD_FIELDS fields, DRS_OPTIONS options);

        /// <summary>
        /// Verify DRSUpdateRefs API
        /// </summary>
        /// <param name="retVal">return value</param>
        /// <param name="machine">SUT</param>
        /// <param name="dest">replication destination</param>
        /// <param name="options">options</param>
        void VerifyDRSUpdateRefs(uint retVal, EnvironmentConfig.Machine machine, DsServer dest, DRS_OPTIONS options);
        #endregion

        #region Administration Tool Support
        /// <summary>
        /// verify DRSAddEntry API
        /// </summary>
        /// <param name="dwInVersion">request message version</param>
        /// <param name="inMessage">request message</param>
        /// <param name="outVersion">reply message version</param>
        /// <param name="outMessage">reply message</param>
        void VerifyDRSAddEntry(
            uint dwInVersion,
            DRS_MSG_ADDENTRYREQ? inMessage,
            uint? outVersion,
            DRS_MSG_ADDENTRYREPLY? outMessage);


        /// <summary>
        /// verify DRSAddSidHistory API
        /// </summary>
        /// <param name="dwInVersion">request message version</param>
        /// <param name="inMessage">request message</param>
        /// <param name="outVersion">reply message version</param>
        /// <param name="outMessage">reply message</param>
        void VerifyDRSAddSidHistory(
            uint retVal,
            string srcSid,
            string srcObjDn,
            ObjectNameType srcNameType,
            string destObjDn,
            ObjectNameType destNameType,
            EnvironmentConfig.Machine src,
            EnvironmentConfig.Machine dest,
            IDL_DRSAddSidHistory_dwInVersion_Values dwInVersion,
            DRS_MSG_ADDSIDREQ? inMessage,
            IDL_DRSAddSidHistory_pdwOutVersion_Values? outVersion,
            DRS_MSG_ADDSIDREPLY? outMessage);

        /// <summary>
        /// verify DRSWriteSPN API
        /// </summary>
        /// <param name="dwInVersion">request message version</param>
        /// <param name="inMessage">request message</param>
        /// <param name="outVersion">reply message version</param>
        /// <param name="outMessage">reply message</param>
        void VerifyDRSWriteSPN(
            dwInVersion_Values dwInVersion,
            DRS_MSG_SPNREQ? inMessage,
            pdwOutVersion_Values? outVersion,
            DRS_MSG_SPNREPLY? outMessage);

        /// <summary>
        /// Verify DrsRemoveDsServer results.
        /// </summary>
        /// <param name="machine"></param>
        /// <param name="dwInVersion"></param>
        /// <param name="req"></param>
        /// <param name="outVersion"></param>
        /// <param name="reply"></param>
        void VerifyDrsRemoveDsServer(
            uint retVal,
            EnvironmentConfig.Machine machine,
            IDL_DRSRemoveDsServer_dwInVersion_Values dwInVersion,
            DRS_MSG_RMSVRREQ req,
            IDL_DRSRemoveDsServer_pdwOutVersion_Values? outVersion,
            DRS_MSG_RMSVRREPLY? reply);

        /// <summary>
        /// Verify DrsRemoveDsDomain results.
        /// </summary>
        /// <param name="machine"></param>
        /// <param name="dwInVersion"></param>
        /// <param name="req"></param>
        /// <param name="outVersion"></param>
        /// <param name="reply"></param>
        void VerifyDrsRemoveDsDomain(
            uint retVal,
            EnvironmentConfig.Machine machine,
            IDL_DRSRemoveDsDomain_dwInVersion_Values dwInVersion,
            DRS_MSG_RMDMNREQ req,
            IDL_DRSRemoveDsDomain_pdwOutVersion_Values? outVersion,
            DRS_MSG_RMDMNREPLY? reply);

        #endregion

        #region LDS Demotion
        /// <summary>
        /// verification LDS demotion init result
        /// </summary>
        /// <param name="retVal">return value</param>
        void VerifyDRSInitDemotion(uint retVal);

        /// <summary>
        /// Verify LDS demotion result
        /// </summary>
        /// <param name="retVal">return value</param>
        /// <param name="svr">lds target</param>
        /// <param name="ops">operations</param>
        /// <param name="verificationPartner">partner lds instance for verification</param>
        void VerifyDRSFinishDemotion(uint retVal, EnvironmentConfig.Machine svr, DRS_MSG_FINISH_DEMOTIONREQ_OPERATIONS ops, EnvironmentConfig.Machine verificationPartner = EnvironmentConfig.Machine.None);

        #endregion

        #region dsaop RPC methods

        /// <summary>
        /// Verification method for IDL_DSAPrepareScript.
        /// </summary>
        /// <param name="outVersion">A pointer to the version of response message.</param>
        /// <param name="outMessage">A pointer to the response message.</param>
        void VerifyDsaPrepareScript(uint? outVersion, DSA_MSG_PREPARE_SCRIPT_REPLY? outMessage);

        /// <summary>
        /// Verification method for IDL_DSAExecuteScript.
        /// </summary>
        /// <param name="outVersion">A pointer to the version of response message.</param>
        /// <param name="outMessage">A pointer to the response message.</param>
        void VerifyDsaExecuteScript(uint? outVersion, DSA_MSG_EXECUTE_SCRIPT_REPLY? outMessage);

        #endregion

        #region Lookups
        /// <summary>
        /// Verify DrsGetMemberships results.
        /// </summary>
        /// <param name="machine"></param>
        /// <param name="dwInVersion"></param>
        /// <param name="req"></param>
        /// <param name="outVersion"></param>
        /// <param name="reply"></param>
        void VerifyDrsGetMemberships(
            EnvironmentConfig.Machine machine,
            uint dwInVersion,
            DRS_MSG_REVMEMB_REQ_V1 req,
            uint? outVersion,
            DRS_MSG_REVMEMB_REPLY_V1? reply);

        /// <summary>
        /// Verify DrsGetMemberships2 results.
        /// </summary>
        /// <param name="machine"></param>
        /// <param name="dwInVersion"></param>
        /// <param name="req"></param>
        /// <param name="outVersion"></param>
        /// <param name="reply"></param>
        void VerifyDrsGetMemberships2(
            EnvironmentConfig.Machine machine,
            uint dwInVersion,
            DRS_MSG_GETMEMBERSHIPS2_REQ req,
            uint? outVersion,
            DRS_MSG_GETMEMBERSHIPS2_REPLY? reply);

        /// <summary>
        /// Verify DrsVerifyNames results.
        /// </summary>
        /// <param name="machine"></param>
        /// <param name="dwInVersion"></param>
        /// <param name="req"></param>
        /// <param name="outVersion"></param>
        /// <param name="reply"></param>
        /// <param name="objectDNs">The DNs of the object to be verified.</param>
        void VerifyDrsVerifyNames(
            EnvironmentConfig.Machine machine,
            uint dwInVersion,
            DRS_MSG_VERIFYREQ req,
            uint? outVersion,
            DRS_MSG_VERIFYREPLY? reply,
            string[] objectDNs
            );

        /// <summary>
        /// Verify CrackNames results.
        /// </summary>
        /// <param name="machine"></param>
        /// <param name="retVal"></param>
        /// <param name="dwInVersion"></param>
        /// <param name="req"></param>
        /// <param name="outVersion"></param>
        /// <param name="reply"></param>
        void VerifyDrsCrackNames(
            EnvironmentConfig.Machine machine,
            uint retVal,
            uint dwInVersion,
            DRS_MSG_CRACKREQ req,
            uint? outVersion,
            DRS_MSG_CRACKREPLY? reply);
        #endregion


        #region dc clone
        /// <summary>
        /// verify IDL_DRSAddCloneDC result
        /// </summary>
        /// <param name="retVal">return value</param>
        /// <param name="svr">target dc</param>
        /// <param name="machinename">cloned dc name</param>
        /// <param name="sitename">site where the cloned dc is</param>
        void VerifyDRSAddCloneDC(uint retVal, EnvironmentConfig.Machine svr, string machinename, string sitename);
        #endregion

        #region replica demotion
        /// <summary>
        /// verify IDL_DRSReplicaDemotion result
        /// </summary>
        /// <param name="retVal">return value</param>
        /// <param name="svr">target</param>
        /// <param name="ncType">nc type</param>
        /// <param name="reply">DRS_MSG_REPLICA_DEMOTIONREPLY</param>
        /// <param name="partner">replica partner</param>
        void VerifyDRSReplicaDemotion(uint retVal, EnvironmentConfig.Machine svr, NamingContext ncType, DRS_MSG_REPLICA_DEMOTIONREPLY reply, EnvironmentConfig.Machine partner);
        #endregion

        #region NGC Key
        /// <summary>
        /// verify DRSWriteNgcKey API
        /// </summary>
        /// <param name="dwInVersion">request message version</param>
        /// <param name="inMessage">request message</param>
        /// <param name="outVersion">reply message version</param>
        /// <param name="outMessage">reply message</param>
        void VerifyDRSWriteNgcKey(
            uint dwInVersion,
            DRS_MSG_WRITENGCKEYREQ? inMessage,
            uint? outVersion,
            DRS_MSG_WRITENGCKEYREPLY? outMessage);

        /// <summary>
        /// verify DRSReadNgcKey API
        /// </summary>
        /// <param name="dwInVersion">request message version</param>
        /// <param name="inMessage">request message</param>
        /// <param name="outVersion">reply message version</param>
        /// <param name="outMessage">reply message</param>
        void VerifyDRSReadNgcKey(
            uint dwInVersion,
            DRS_MSG_READNGCKEYREQ? inMessage,
            uint? outVersion,
            DRS_MSG_READNGCKEYREPLY? outMessage);
        #endregion

        /// <summary>
        /// Verification method for IDL_DRSInterDomainMove.
        /// </summary>
        /// <param name="reqVer">The request version.</param>
        /// <param name="repVer">The reply version.</param>
        /// <param name="reply">The response message.</param>
        /// <param name="ret">The return value.</param>
        void VerifyDrsInterDomainMove(DRSInterDomainMove_Versions reqVer, uint repVer, DRS_MSG_MOVEREPLY? reply, uint ret);

    }
}
